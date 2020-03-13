using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupsSettingEditorElement : SettingEditorElementBase
    {
        public LauncherGroupsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups, INotifyManager notifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
            GroupItems = allLauncherGroups;
            NotifyManager = notifyManager;
        }

        #region property

        INotifyManager NotifyManager { get; }

        public ObservableCollection<LauncherGroupSettingEditorElement> GroupItems { get; }
        public ObservableCollection<WrapModel<Guid>> LauncherItems { get; } = new ObservableCollection<WrapModel<Guid>>();

        #endregion

        #region function

        public void MoveGroupItem(int startIndex, int insertIndex)
        {
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var item = GroupItems[startIndex];
            GroupItems.RemoveAt(startIndex);
            GroupItems.Insert(insertIndex, item);

            //foreach(var group in GroupItems.Counting()) {
            //    group.Value.Sequence = group.Number * launcherFactory.GroupItemStep;
            //}
        }

        public void RemoveGroup(Guid launcherGroupId)
        {
            var targetItem = GroupItems.First(i => i.LauncherGroupId == launcherGroupId);
            GroupItems.Remove(targetItem);

            // DB から物理削除
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherGroupId(targetItem.LauncherGroupId);
                launcherGroupsEntityDao.DeleteGroup(targetItem.LauncherGroupId);

                commander.Commit();
            }

            targetItem.Dispose();
        }

        public Guid AddNewGroup(LauncherGroupKind kind)
        {
            var newGroupName = TextUtility.ToUnique(Properties.Resources.String_LauncherGroup_NewItem_Name, GroupItems.Select(i => i.Name).ToList(), StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}({n})");

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            var groupData = launcherFactory.CreateGroupData(newGroupName, kind);

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherGroupsDao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                //var groupStep = launcherFactory.GroupItemStep;
                //var sequence = launcherGroupsDao.SelectMaxSequence() + groupStep;
                launcherGroupsDao.InsertNewGroup(groupData, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            var group = new LauncherGroupSettingEditorElement(groupData.LauncherGroupId, MainDatabaseBarrier, StatementLoader, IdFactory, LoggerFactory);
            group.Initialize();
            GroupItems.Add(group);

            return group.LauncherGroupId;
        }

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            ThrowIfDisposed();

            IReadOnlyList<Guid> launcherItemIds;
            //IReadOnlyList<Guid> groupIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIds = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();

                //var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                //groupIds = launcherGroupsEntityDao.SelectAllLauncherGroupIds().ToList();
            }

            LauncherItems.SetRange(launcherItemIds.Select(i => WrapModel.Create(i, LoggerFactory)));

            //GroupItems.Clear();
            //foreach(var groupId in groupIds) {
            //    var element = new LauncherGroupSettingEditorElement(groupId, MainDatabaseBarrier, StatementLoader, IdFactory, LoggerFactory);
            //    element.Initialize();
            //    GroupItems.Add(element);
            //}
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);
            foreach(var group in GroupItems.Counting()) {
                group.Value.Sequence = group.Number * launcherFactory.GroupItemStep;
            }

            foreach(var group in GroupItems) {
                group.Save(commandPack);
            }
        }

        #endregion
    }

}