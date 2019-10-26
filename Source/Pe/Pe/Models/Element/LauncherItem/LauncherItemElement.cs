using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem
{
    public class LauncherItemElement : ElementBase
    {
        #region variable

        bool _nowCustomize;

        #endregion

        public LauncherItemElement(Guid launcherItemId, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, LauncherIconElement launcherIconElement, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            Icon = launcherIconElement;
        }

        #region property

        public Guid LauncherItemId { get; }

        IOrderManager OrderManager { get; }
        IClipboardManager ClipboardManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier? FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public string? Name { get; private set; }
        public string? Code { get; private set; }
        public LauncherItemKind Kind { get; private set; }
        public bool IsEnabledCommandLauncher { get; private set; }
        public string? Comment { get; private set; }

        public LauncherIconElement Icon { get; }

        public virtual bool NowCustomizing
        {
            get => this._nowCustomize;
            private set => SetProperty(ref this._nowCustomize, value);
        }

        #endregion

        #region function

        void LoadLauncherItem()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Code = launcherItemData.Code;
                Kind = launcherItemData.Kind;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;
            }
        }

        public LauncherFileDetailData LoadFileDetail()
        {
            LauncherExecutePathData pathData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                pathData = launcherFilesEntityDao.SelectPath(LauncherItemId);
            }

#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            var expandedPath = PathUtility.ExpandFilePath(pathData.Path);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
            var result = new LauncherFileDetailData() {
                PathData = pathData,
                FullPath = expandedPath,
            };

            return result;
        }

        IList<LauncherEnvironmentVariableData> GetMergeEnvironmentVariableItems(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            return launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
        }

        ILauncherExecuteResult ExecuteFile(Screen screen)
        {
            LauncherFileData fileData;
            IList<LauncherEnvironmentVariableData> envItems;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                if(fileData.IsEnabledCustomEnvironmentVariable) {
                    envItems = GetMergeEnvironmentVariableItems(commander, commander.Implementation);
                } else {
                    envItems = new List<LauncherEnvironmentVariableData>();
                }
            }

            var launcherExecutor = new LauncherExecutor(OrderManager, LoggerFactory);
            var result = launcherExecutor.Execute(Kind, fileData, fileData, envItems, screen);

            return result;
        }

        public ILauncherExecuteResult Execute(Screen screen)
        {
            try {
                ILauncherExecuteResult result;
                switch(Kind) {
                    case LauncherItemKind.File:
                        result = ExecuteFile(screen);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Debug.Assert(result != null);

                using(var commander = MainDatabaseBarrier.WaitWrite()) {
                    var dao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    dao.UpdateExecuteCountIncrement(LauncherItemId, DatabaseCommonStatus.CreateCurrentAccount());
                    commander.Commit();
                }

                return result;

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                return LauncherExecuteResult.Error(ex);
            }
        }

        public ILauncherExecuteResult ExecuteExtends(Screen screen)
        {
            throw new NotImplementedException();
        }

        ILauncherExecutePathParameter GetExecutePath()
        {
            Debug.Assert(Kind == LauncherItemKind.File);

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return launcherFilesEntityDao.SelectPath(LauncherItemId);
            }
        }

        public ILauncherExecuteResult OpenParentDirectory()
        {
            if(!(Kind == LauncherItemKind.File)) {
                throw new InvalidOperationException($"{Kind}");
            }

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(OrderManager, LoggerFactory);
            var result = launcherExecutor.OpenParentDirectory(Kind, pathData);

            return result;

        }

        public ILauncherExecuteResult OpenWorkingDirectory()
        {
            if(!(Kind == LauncherItemKind.File)) {
                throw new InvalidOperationException($"{Kind}");
            }

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(OrderManager, LoggerFactory);
            var result = launcherExecutor.OpenWorkingDirectory(Kind, pathData);

            return result;

        }

        public void CopyExecutePath()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = pathData.Path;
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void CopyParentDirectory()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = Path.GetDirectoryName(pathData.Path);
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void CopyOption()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = pathData.Option;
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void CopyWorkingDirectory()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = pathData.WorkDirectoryPath;
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void OpenCustomizeView(Screen screen)
        {
            if(NowCustomizing) {
                Logger.LogWarning("現在編集中: {0}", LauncherItemId);
                return;
            }

            //TODO: 確定時の処理
            NowCustomizing = true;
            var element = OrderManager.CreateCustomizeLauncherItemElement(LauncherItemId, Icon, screen);
            element.StartView();
        }

        public void ShowProperty()
        {
            if(!(Kind == LauncherItemKind.File)) {
                throw new InvalidOperationException($"{Kind}");
            }

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(OrderManager, LoggerFactory);
            launcherExecutor.ShowProperty(pathData);
        }

        #endregion

        #region ElementBase

        override protected void InitializeImpl()
        {
            LoadLauncherItem();
        }

        #endregion
    }
}
