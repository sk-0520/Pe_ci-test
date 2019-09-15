using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Model;
using ContentTypeTextNet.Pe.Bridge.Model.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Model.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public interface IIconPack<TValue>
    {
        #region property

        TValue Small { get; }
        TValue Normal { get; }
        TValue Big { get; }
        TValue Large { get; }

        #endregion
    }


    public class IconLoader
    {
        const int sizeofGRPICONDIR_idCount = 4;
        const int offsetGRPICONDIRENTRY_nID = 12;
        const int offsetGRPICONDIRENTRY_dwBytesInRes = 8;
        static readonly int sizeofICONDIR;
        static readonly int sizeofICONDIRENTRY;
        static readonly int sizeofGRPICONDIRENTRY;

        static IconLoader()
        {
            sizeofICONDIR = Marshal.SizeOf<ICONDIR>();
            sizeofICONDIRENTRY = Marshal.SizeOf<ICONDIRENTRY>();
            sizeofGRPICONDIRENTRY = Marshal.SizeOf<GRPICONDIRENTRY>();
        }

        public IconLoader(ILogger logger)
        {
            Logger = logger;
        }

        public IconLoader(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        /// <summary>
        /// ファイルのサムネイルを取得。
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconSize"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public BitmapSource? GetThumbnailImage(string iconPath, IconSize iconSize)
        {
            try {
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                IShellItem iShellItem = null;
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                NativeMethods.SHCreateItemFromParsingName(iconPath, IntPtr.Zero, NativeMethods.IID_IShellItem, out iShellItem);

                var size = iconSize.ToSize();
                var siigbf = SIIGBF.SIIGBF_RESIZETOFIT;
                var hResultBitmap = IntPtr.Zero;
                using(var shellItem = new ComWrapper<IShellItem>(iShellItem)) {
                    ((IShellItemImageFactory)shellItem.Com).GetImage(PodStructUtility.Convert(size), siigbf, out hResultBitmap);
                }
                using(var hBitmap = new BitmapHandleWrapper(hResultBitmap)) {
                    var result = hBitmap.MakeBitmapSource();
                    return result;
                }
            } catch(COMException ex) {
                Logger.LogWarning(ex, ex.Message);
                return null;
            } catch(ArgumentException ex) {
                Logger.LogWarning(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// http://hp.vector.co.jp/authors/VA016117/rsrc2icon.html
        /// </summary>
        /// <param name="hModule"></param>
        /// <param name="name"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        byte[]? GetResourceBinaryData(IntPtr hModule, IntPtr name, ResType resType)
        {
            var hGroup = NativeMethods.FindResource(hModule, name, new IntPtr((int)resType));
            if(hGroup == IntPtr.Zero) {
                Debug.WriteLine($"return {nameof(NativeMethods.FindResource)}");
                return null;
            }

            var hLoadGroup = NativeMethods.LoadResource(hModule, hGroup);
            if(hLoadGroup == IntPtr.Zero) {
                Debug.WriteLine($"return {nameof(NativeMethods.LoadResource)}");
                return null;
            }

            var resData = NativeMethods.LockResource(hLoadGroup);
            if(resData == IntPtr.Zero) {
                Debug.WriteLine($"return {nameof(NativeMethods.LockResource)}");
                return null;
            }

            var resSize = NativeMethods.SizeofResource(hModule, hGroup);
            if(resSize == 0) {
                Logger.LogDebug($"return {nameof(NativeMethods.SizeofResource)}");
                return null;
            }

            var resBinary = new byte[resSize];
            Marshal.Copy(resData, resBinary, 0, resBinary.Length);

            return resBinary;
        }

        /// <summary>
        /// https://github.com/TsudaKageyu/IconExtractor
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        IList<byte[]> LoadIconResource(string resourcePath)
        {
            var hModule = NativeMethods.LoadLibraryEx(resourcePath, IntPtr.Zero, LOAD_LIBRARY.LOAD_LIBRARY_AS_DATAFILE);
            var binaryList = new List<byte[]>();
            EnumResNameProc proc = (hMod, type, name, lp) => {
                var binaryGroupIconData = GetResourceBinaryData(hMod, name, ResType.GROUP_ICON);
                if(binaryGroupIconData != null) {
                    var iconCount = BitConverter.ToUInt16(binaryGroupIconData, sizeofGRPICONDIR_idCount);
                    //Debug.WriteLine("iconCount = {0}", iconCount);

                    var totalSize = sizeofICONDIR + sizeofICONDIRENTRY * iconCount;
                    foreach(var i in Enumerable.Range(0, iconCount)) {
                        var readOffset = sizeofICONDIR + (sizeofGRPICONDIRENTRY * i) + offsetGRPICONDIRENTRY_dwBytesInRes;
                        if(binaryGroupIconData.Length < 0 && readOffset + sizeof(Int32) < binaryGroupIconData.Length) {
                            break;
                        }
                        var length = BitConverter.ToInt32(
                            binaryGroupIconData,
                            readOffset
                        );
                        //Debug.WriteLine("[{0}] = {1} byte", i, length);
                        totalSize += length;
                    }
                    //Debug.WriteLine("totalSize = {0}", totalSize);

                    // TODO:BinaryChunkedStreamがまともに動くなら切り替える
                    using(var stream = new BinaryWriter(new MemoryStream(totalSize))) {
                        stream.Write(binaryGroupIconData, 0, sizeofICONDIR);

                        var picOffset = sizeofICONDIR + sizeofICONDIRENTRY * iconCount;
                        foreach(var i in Enumerable.Range(0, iconCount)) {
                            stream.Seek(sizeofICONDIR + sizeofICONDIRENTRY * i, SeekOrigin.Begin);
                            var offsetWrite = sizeofICONDIR + sizeofGRPICONDIRENTRY * i;
                            if(binaryGroupIconData.Length <= offsetWrite + offsetGRPICONDIRENTRY_nID) {
                                continue;
                            }
                            stream.Write(binaryGroupIconData, offsetWrite, offsetGRPICONDIRENTRY_nID);
                            stream.Write(picOffset);

                            stream.Seek(picOffset, SeekOrigin.Begin);

                            ushort id = BitConverter.ToUInt16(binaryGroupIconData, sizeofICONDIR + sizeofGRPICONDIRENTRY * i + offsetGRPICONDIRENTRY_nID);
                            var pic = GetResourceBinaryData(hModule, new IntPtr(id), ResType.ICON);
                            if(pic != null) {
                                stream.Write(pic, 0, pic.Length);
                                picOffset += pic.Length;
                            }
                        }

                        binaryList.Add(((MemoryStream)stream.BaseStream).ToArray());
                    }
                }

                return true;
            };

            NativeMethods.EnumResourceNames(hModule, (int)ResType.GROUP_ICON, proc, IntPtr.Zero);

            return binaryList;
        }


        /// <summary>
        /// 16px, 32pxアイコン取得。
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconSize"></param>
        /// <param name="iconIndex"></param>
        /// <param name="hasIcon"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        BitmapSource? LoadNormalIcon(string iconPath, IconSize iconSize, int iconIndex, bool hasIcon)
        {
            Debug.Assert(new[] { IconSize.Kind.Small, IconSize.Kind.Normal }.Any(i => (int)i == iconSize.Width), iconSize.ToString());
            Debug.Assert(0 <= iconIndex, iconIndex.ToString());

            // 16, 32 px
            if(hasIcon) {
                var iconHandle = new IntPtr[1];
                if(iconSize.Width == (int)IconSize.Kind.Small) {
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    _ = NativeMethods.ExtractIconEx(iconPath, iconIndex, null, iconHandle, 1);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                } else {
                    Debug.Assert(iconSize.Width == (int)IconSize.Kind.Normal);
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    NativeMethods.ExtractIconEx(iconPath, iconIndex, iconHandle, null, 1);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                }
                if(iconHandle[0] != IntPtr.Zero) {
                    using(var hIcon = new IconHandleWrapper(iconHandle[0])) {
                        return hIcon.MakeBitmapSource();
                    }
                }
            }

            if(iconSize.Width == (int)IconSize.Kind.Normal) {
                try {
                    var thumbnailImage = GetThumbnailImage(iconPath, iconSize);
                    if(thumbnailImage != null) {
                        return thumbnailImage;
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
            }

            var fileInfo = new SHFILEINFO();
            SHGFI flag = SHGFI.SHGFI_ICON;
            if(iconSize.Width == (int)IconSize.Kind.Small) {
                flag |= SHGFI.SHGFI_SMALLICON;
            } else {
                Debug.Assert(iconSize.Width == (int)IconSize.Kind.Normal);
                flag |= SHGFI.SHGFI_LARGEICON;
            }
            var fileInfoResult = NativeMethods.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flag);
            if(fileInfo.hIcon != IntPtr.Zero) {
                using(var hIcon = new IconHandleWrapper(fileInfo.hIcon)) {
                    return hIcon.MakeBitmapSource();
                }
            }

            return null;
        }

        /// <summary>
        /// 48px以上のアイコン取得。
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconSize"></param>
        /// <param name="iconIndex"></param>
        /// <param name="hasIcon"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        BitmapSource? LoadLargeIcon(string iconPath, IconSize iconSize, int iconIndex, bool hasIcon)
        {
            //Debug.Assert(iconScale.IsIn(IconScale.Big, IconScale.Large), iconScale.ToString());
            Debug.Assert(new[] { (int)IconSize.Kind.Big, (int)IconSize.Kind.Large }.Any(i => (int)i == iconSize.Width), iconSize.ToString());
            Debug.Assert(0 <= iconIndex, iconIndex.ToString());

            if(hasIcon) {
                try {
                    var iconList = LoadIconResource(iconPath);
                    if(iconIndex < iconList.Count) {
                        var binary = iconList[iconIndex];
                        iconList.Clear();
                        var image = (BitmapSource)DrawingUtility.ImageSourceFromBinaryIcon(binary, iconSize.ToSize());
                        return image;
                    }
                } catch(Exception ex) {
                    Logger.LogDebug(ex, ex.Message);
                }
            }

            var thumbnailImage = GetThumbnailImage(iconPath, iconSize);
            if(thumbnailImage != null) {
                return thumbnailImage;
            }

            var shellImageList = iconSize.Width == (int)IconSize.Kind.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
            var fileInfo = new SHFILEINFO() {
                iIcon = iconIndex,
            };

            var infoFlags = SHGFI.SHGFI_SYSICONINDEX;
            var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

            IImageList? resultImageList = null;
            try {
                var getImageListResult = NativeMethods.SHGetImageList((int)shellImageList, ref NativeMethods.IID_IImageList, out resultImageList);

                if(getImageListResult == ComResult.S_OK) {
                    Debug.Assert(resultImageList != null);
                    using(var imageList = new ComWrapper<IImageList>(resultImageList)) {
                        int n = 0;
                        imageList.Com.GetImageCount(ref n);

                        var hResultIcon = IntPtr.Zero;
                        var hResult = imageList.Com.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hResultIcon);
                        if(hResultIcon != IntPtr.Zero) {
                            using(var hIcon = new IconHandleWrapper(hResultIcon)) {
                                return hIcon.MakeBitmapSource();
                            }
                        }
                    }
                }
            } catch(InvalidCastException ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// アイコンを取得。
        /// </summary>
        /// <param name="iconPath">対象ファイルパス。</param>
        /// <param name="iconSize">アイコンサイズ。</param>
        /// <param name="iconIndex">アイコンインデックス。</param>
        /// <param name="logger"></param>
        /// <returns>取得したアイコン。呼び出し側で破棄が必要。</returns>
        public BitmapSource? Load(string iconPath, IconSize iconSize, int iconIndex)
        {
            // 実行形式
            var hasIcon = PathUtility.HasIconPath(iconPath);
            var useIconIndex = Math.Abs(iconIndex);

            BitmapSource result;
            if(iconSize.Width == (int)IconSize.Kind.Small || iconSize.Width == (int)IconSize.Kind.Normal) {
                result = LoadNormalIcon(iconPath, iconSize, useIconIndex, hasIcon)!;
            } else {
                result = LoadLargeIcon(iconPath, iconSize, useIconIndex, hasIcon)!;
            }

            return result;
        }

    }
}
