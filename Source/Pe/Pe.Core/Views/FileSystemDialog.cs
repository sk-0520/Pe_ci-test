using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using static ContentTypeTextNet.Pe.PInvoke.Windows.NativeMethods;

namespace ContentTypeTextNet.Pe.Core.Views
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class FileSystemFosAttribute : Attribute
    {
        public FileSystemFosAttribute(FOS fos)
        {
            Fos = fos;
        }

        #region property
        public FOS Fos { get; }
        #endregion
    }

    public class FileSystemDialog: DisposerBase
    {
        public FileSystemDialog()
        {
            var dialog = new FileOpenDialog();
            FileOpenDialog = (IFileOpenDialog)dialog;
        }

        #region property

        IFileOpenDialog FileOpenDialog { get; }

        /// <summary>
        /// フォルダ選択を行うか。
        /// </summary>
        [FileSystemFos(FOS.FOS_PICKFOLDERS)]
        public bool PickFolders { get; set; }

        /// <summary>
        /// ファイルシステムを強制する。
        /// </summary>
        [FileSystemFos(FOS.FOS_FORCEFILESYSTEM)]
        public bool ForceFileSystem { get; set; } = true;

        /// <summary>
        /// 新規作成時に確認するか。
        /// </summary>
        [FileSystemFos(FOS.FOS_CREATEPROMPT)]
        public bool CreatePrompt { get; set; } = true;
        /// <summary>
        /// 上書き時に確認するか。
        /// </summary>
        [FileSystemFos(FOS.FOS_OVERWRITEPROMPT)]
        public bool OverwritePrompt { get; set; } = true;
        /// <summary>
        /// 無効なパスに警告。
        /// </summary>
        [FileSystemFos(FOS.FOS_FILEMUSTEXIST)]
        public bool CheckPathExists { get; set; } = true;
        /// <summary>
        /// 有効な Win32 ファイル名だけを受け入れる。
        /// </summary>
        [FileSystemFos(FOS.FOS_PATHMUSTEXIST)]
        public bool ValidateNames { get; set; } = true;
        [FileSystemFos(FOS.FOS_NOCHANGEDIR)]
        public bool NoChangeDirectory { get; set; } = false;
        /// <summary>
        /// ショートカットで参照されたファイルの場所を返すか。
        /// </summary>
        [FileSystemFos(FOS.FOS_NODEREFERENCELINKS)]
        public bool DereferenceLinks { get; set; } = true;
        /// <summary>
        /// 最近使用したアイテムを隠すか。
        /// </summary>
        [FileSystemFos(FOS.FOS_HIDEMRUPLACES)]
        public bool HideMruPlaces { get; set; } = false;
        /// <summary>
        /// デフォルトで表示されるアイテムを隠すか。
        /// </summary>
        [FileSystemFos(FOS.FOS_HIDEPINNEDPLACES)]
        public bool HidePinnedPlaces { get; set; } = false;
        /// <summary>
        /// 読み取り専用は返さない？。
        /// </summary>
        [FileSystemFos(FOS.FOS_NOREADONLYRETURN)]
        public bool NoReadOnlyReturn { get; set; } = false;

        public string Title { get; set; } = string.Empty;

        public string InitialDirectory { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        #endregion

        #region function

        FOS GetFos()
        {
            var options = default(FOS);
            var type = GetType();
            var propertInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var fosAttributes = propertInfos
                .Select(i => new { Property = i, Attribute = i.GetCustomAttribute<FileSystemFosAttribute>() })
                .Where(i => i.Attribute != null)
            ;
            foreach(var item in fosAttributes) {
                var isChecked = (bool)item.Property.GetValue(this)!;
                if(isChecked) {
                    options |= item.Attribute.Fos;
                }
            }

            return options;
        }

        ComWrapper<IShellItem>? CreateFileItem(string path)
        {
            IShellItem item;
            IntPtr idl;
            uint atts = 0;
            if(NativeMethods.SHILCreateFromPath(path, out idl, ref atts) == 0) {
                if(NativeMethods.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0) {
                    return ComWrapper.Create(item);
                }
            }

            return null;
        }

        public bool? ShowDialog(Window parent) => ShowDialog(HandleUtility.GetWindowHandle(parent));
        public bool? ShowDialog(IntPtr hWnd)
        {
            var cleaner = new GroupDisposer();

            var options = GetFos();
            FileOpenDialog.SetOptions(options);

            if(!string.IsNullOrEmpty(Title)) {
                FileOpenDialog.SetTitle(Title);
            }

            if(!string.IsNullOrEmpty(InitialDirectory)) {
                var item = CreateFileItem(InitialDirectory);
                if(item != null) {
                    FileOpenDialog.SetDefaultFolder(item.Com);
                    cleaner.Add(item);
                }
            }

            if(!string.IsNullOrEmpty(FileName)) {
                var parentDirPath = Path.GetDirectoryName(FileName);
                if(parentDirPath != null) {
                    var item = CreateFileItem(parentDirPath);
                    if(item != null) {
                        FileOpenDialog.SetFolder(item.Com);
                        cleaner.Add(item);
                    }
                }
                FileOpenDialog.SetFileName(FileName);
            }


            var reuslt = FileOpenDialog.Show(hWnd);
            if(reuslt == (uint)ERROR.ERROR_CANCELLED) {
                return false;
            }
            if(reuslt != 0) {
                return null;
            }

            IShellItem resultItem;
            FileOpenDialog.GetResult(out resultItem);
            cleaner.Add(ComWrapper.Create(resultItem));
            IntPtr pszPath = IntPtr.Zero;
            resultItem.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out pszPath);
            if(pszPath != IntPtr.Zero) {
                var path = Marshal.PtrToStringAuto(pszPath);
                Marshal.FreeCoTaskMem(pszPath);
                if(path != null) {
                    FileName = path;
                    return true;
                }
            }

            return null;
        }

        #endregion

        #region DiposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Marshal.ReleaseComObject(FileOpenDialog);
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class OpenFileDialog : FileSystemDialog
    {
        public OpenFileDialog()
            : base()
        {
        }
    }

    public class FolderBrowserDialog: FileSystemDialog
    {
        public FolderBrowserDialog()
            : base()
        {
            PickFolders = true;
            CreatePrompt = false;
            OverwritePrompt = false;
        }
    }
}
