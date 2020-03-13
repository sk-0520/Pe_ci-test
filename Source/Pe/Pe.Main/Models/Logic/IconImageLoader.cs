using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public abstract class IconImageLoaderBase : BindModelBase
    {
        public IconImageLoaderBase(IconBox iconBox, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconBox = iconBox;
            DispatcherWrapper = dispatcherWrapper;
            RunningStatusImpl = new RunningStatus(LoggerFactory);
        }

        #region property

        public IconBox IconBox { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        RunningStatus RunningStatusImpl { get; }
        public IRunningStatus RunningStatus => RunningStatusImpl;

        public static IReadOnlyCollection<string> ImageFileExtensions { get; } = new[] { "png", "bmp", "jpeg", "jpg" };

        protected BitmapSource? CachedImage { get; private set; }

        #endregion

        #region function

        protected BitmapSource? ToImage(IReadOnlyList<byte[]>? imageBynaryItems)
        {
            ThrowIfDisposed();

            if(imageBynaryItems == null || imageBynaryItems.Count == 0) {
                return null;
            }

            using(var stream = new BinaryChunkedStream()) {
                using(var writer = new BinaryWriter(new KeepStream(stream))) {
                    foreach(var imageBinary in imageBynaryItems) {
                        writer.Write(imageBinary);
                    }
                }
                stream.Position = 0;
                var iconImage = DispatcherWrapper.Get(() => {
                    var imageLoader = new ImageLoader(LoggerFactory);
                    var image = imageLoader.Load(stream);
                    FreezableUtility.SafeFreeze(image);
                    return image;
                });
                return iconImage;
            }
        }

        /// <summary>
        /// <see cref="IconBox"/> より大きい場合にががっと縮小する。
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        protected BitmapSource ResizeImage(BitmapSource bitmapSource)
        {
            ThrowIfDisposed();

            var iconSize = new IconSize(IconBox);

            if(iconSize.Width < bitmapSource.PixelWidth || iconSize.Height < bitmapSource.PixelHeight) {
                Logger.LogDebug("アイコンサイズを縮小: アイコン({0}x{1}), 指定({2}x{3})", bitmapSource.PixelWidth, bitmapSource.PixelHeight, iconSize.Width, iconSize.Height);
                var scaleX = iconSize.Width / (double)bitmapSource.PixelWidth;
                var scaleY = iconSize.Height / (double)bitmapSource.PixelHeight;
                Logger.LogTrace("scale: {0}x{1}", scaleX, scaleY);
                DispatcherWrapper.Get(() => {
                    var transformedBitmap = FreezableUtility.GetSafeFreeze(new TransformedBitmap(bitmapSource, new ScaleTransform(scaleX, scaleY)));
                    return FreezableUtility.GetSafeFreeze(new WriteableBitmap(transformedBitmap));
                });
            }

            return bitmapSource;
        }

        protected Task<BitmapSource?> GetIconImageAsync(IconData iconData, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var path = TextUtility.SafeTrim(iconData.Path);
            if(string.IsNullOrEmpty(path)) {
                return Task.FromResult(default(BitmapSource));
            }
            return Task.Run(() => {
                var isFile = File.Exists(path);
                var isDir = !isFile && Directory.Exists(path);
                if(!isFile && !isDir) {
                    return null;
                }

                BitmapSource? iconImage = null;

                if(isFile && PathUtility.HasExtensions(path, ImageFileExtensions)) {
                    Logger.LogDebug("画像ファイルとして読み込み {0}", path);
                    var imageLoader = new ImageLoader(LoggerFactory);
                    using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        iconImage = DispatcherWrapper.Get(() => {
                            var image = imageLoader.Load(stream);
                            return FreezableUtility.GetSafeFreeze(image);
                        });
                    }
                } else {
                    Logger.LogDebug("アイコンファイルとして読み込み {0}", path);
                    var iconLoader = new IconLoader(LoggerFactory);
                    iconImage = DispatcherWrapper.Get(() => {
                        var image = iconLoader.Load(path, new IconSize(IconBox), iconData.Index);
                        return FreezableUtility.GetSafeFreeze(image!);
                    });
                }

                return iconImage;
            });
        }

        protected abstract Task<BitmapSource?> LoadImplAsync(CancellationToken cancellationToken);

        public async Task<BitmapSource?> LoadAsync(bool useCache, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if(useCache && CachedImage != null) {
                Logger.LogTrace("メモリキャッシュされたイメージの使用");
                RunningStatusImpl.State = RunningState.End;
                return CachedImage;
            }

            RunningStatusImpl.State = RunningState.Running;
            try {
                var iconImage = await LoadImplAsync(cancellationToken);
                RunningStatusImpl.State = RunningState.End;
                if(useCache) {
                    CachedImage = iconImage;
                }
                return iconImage;
            } catch(OperationCanceledException ex) {
                Logger.LogWarning(ex, ex.Message);
                RunningStatusImpl.State = RunningState.Cancel;
                throw;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                RunningStatusImpl.State = RunningState.Error;
                throw;
            }
        }

        public void ClearCache()
        {
            CachedImage = null;
        }

        #endregion

        #region BindModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    ClearCache();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class IconImageLoader : IconImageLoaderBase
    {
        public IconImageLoader(IconData iconData, IconBox iconBox, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory) : base(iconBox, dispatcherWrapper, loggerFactory)
        {
            IconData = iconData;
        }

        #region property

        IconData IconData { get; }

        #endregion

        #region IconImageLoaderBase

        protected override Task<BitmapSource?> LoadImplAsync(CancellationToken cancellationToken)
        {
            return GetIconImageAsync(IconData, cancellationToken);
        }

        #endregion
    }

    public class IconImageLoaderPack : IIconPack<IconImageLoaderBase>
    {
        #region variable

        IReadOnlyDictionary<IconBox, IconImageLoaderBase>? _iconItems;

        #endregion

        public IconImageLoaderPack(IEnumerable<IconImageLoaderBase> iconImageLoaders)
        {
            var map = iconImageLoaders.ToDictionary(i => i.IconBox, i => i);
            Small = map[IconBox.Small];
            Normal = map[IconBox.Normal];
            Big = map[IconBox.Big];
            Large = map[IconBox.Large];
        }

        #region IIconPack

        public IconImageLoaderBase Small { get; }
        public IconImageLoaderBase Normal { get; }
        public IconImageLoaderBase Big { get; }
        public IconImageLoaderBase Large { get; }

        public IReadOnlyDictionary<IconBox, IconImageLoaderBase> IconItems => this._iconItems ??= new Dictionary<IconBox, IconImageLoaderBase>() {
            [IconBox.Small] = Small,
            [IconBox.Normal] = Normal,
            [IconBox.Big] = Big,
            [IconBox.Large] = Large,
        };


        #endregion
    }
}