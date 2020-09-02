using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherIconFactory
    {
        public LauncherIconFactory(Guid launcherItemId, LauncherItemKind launcherItemKind, ILauncherItemAddonFinder launcherItemAddonFinder, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            LauncherItemId = launcherItemId;
            LauncherItemKind = launcherItemKind;
            LauncherItemAddonFinder = launcherItemAddonFinder;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        public Guid LauncherItemId { get; }
        public LauncherItemKind LauncherItemKind { get; }
        ILauncherItemAddonFinder LauncherItemAddonFinder { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        #endregion

        #region function

        private IconImageLoaderBase CreateFileIconLoader(IDispatcherWrapper dispatcherWrapper)
        {
            return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
        }

        public object CreateIconSource(IDispatcherWrapper dispatcherWrapper)
        {
            switch(LauncherItemKind) {
                case LauncherItemKind.File: {
                        return new LauncherIconLoader(LauncherItemId, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, dispatcherWrapper, LoggerFactory);
                    }

                case LauncherItemKind.Addon: {
                        var pluginId = MainDatabaseBarrier.ReadData(c => {
                            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            return launcherAddonsEntityDao.SelectAddonPluginId(LauncherItemId);
                        });
                        if(!LauncherItemAddonFinder.Exists(pluginId)) {
                            Logger.LogError("プラグイン取得失敗: ランチャーアイテム = {0}, プラグインID = {1}", LauncherItemId, pluginId);
                            return default!; // null は上流で何とかしてちょ
                        }
                        var addon = LauncherItemAddonFinder.Find(LauncherItemId, pluginId);
                        return addon;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public object CreateView(object? iconSource, bool useCache, IDispatcherWrapper dispatcherWrapper)
        {
            if(iconSource == null) {
                // アイコンがないパターン
                // 読み込み失敗(要調査: IconImageLoaderBase は多分出来上がってる)・アドオンがない
                return null!;
            }

            switch(iconSource) {
                case IconImageLoaderBase iconImageLoader:
                    return new IconViewerViewModel(iconImageLoader, dispatcherWrapper, LoggerFactory) {
                        UseCache = useCache,
                    };

                case ILauncherItemExtension launcherItemExtension:
                    return new IconViewerViewModel(LauncherItemId, launcherItemExtension, dispatcherWrapper, LoggerFactory) {
                        UseCache = useCache,
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
