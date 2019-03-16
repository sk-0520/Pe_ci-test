using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem
{
    public class LauncherItemElement : ElementBase
    {
        public LauncherItemElement(Guid launcherItemId, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public Guid LauncherItemId { get; }

        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public LauncherItemKind Kind { get; private set; }
        public LauncherCommandData Command { get; private set; } = LauncherCommandData.None;
        public IconData Icon { get; private set; } = IconData.None;
        public bool IsEnabledCommandLauncher { get; private set; }
        public string Note { get; private set; }
        #endregion

        #region function

        void LoadLauncherItem()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemsDao(commander, StatementLoader, this);
                var data = dao.SelectLauncherItem(LauncherItemId);

                Name = data.Name;
                Code = data.Code;
                Kind = data.Kind;
                Command = data.Command;
                Icon = data.Icon;
                IsEnabledCommandLauncher = data.IsEnabledCommandLauncher;
                Note = data.Note;
            }
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
