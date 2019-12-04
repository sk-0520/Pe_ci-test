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
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class KeyboardSettingEditorElement : SettingEditorElementBase
    {
        public KeyboardSettingEditorElement(IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public ObservableCollection<KeyboardReplaceJobSettingEditorElement> ReplaceJobEditors { get; } = new ObservableCollection<KeyboardReplaceJobSettingEditorElement>();
        public ObservableCollection<KeyboardDisableJobSettingEditorElement> DisableJobEditors { get; } = new ObservableCollection<KeyboardDisableJobSettingEditorElement>();
        public ObservableCollection<KeyboardPressedJobSettingEditorElement> PressedJobEditors { get; } = new ObservableCollection<KeyboardPressedJobSettingEditorElement>();
        /// <summary>
        /// 削除したアイテム。
        /// <para>最後にdelete流してあげる用。</para>
        /// </summary>
        IList<KeyboardJobSettingEditorElementBase> RemovedJobEditors { get; } = new List<KeyboardJobSettingEditorElementBase>();

        #endregion

        #region function

        void SaveNewActionData(KeyActionData data)
        {
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                keyActionsEntityDao.InsertKeyAction(data, DatabaseCommonStatus.CreateCurrentAccount());

                //TODO: insert empty mapping

                commander.Commit();
            }
        }

        private void StockRemoveItemIfSavedJob(KeyboardJobSettingEditorElementBase editor)
        {
            if(!editor.IsNewJob) {
                RemovedJobEditors.Add(editor);
            }
        }


        public void AddReplaceJob()
        {
            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Replace,
            };

            var editor = new KeyboardReplaceJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            editor.Initialize();
            ReplaceJobEditors.Add(editor);
        }

        public void RemoveReplaceJob(Guid keyActionId)
        {
            var editor = ReplaceJobEditors.First(i => i.KeyActionId == keyActionId);
            ReplaceJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        public void AddDisableJob()
        {
            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Disable,
            };

            var editor = new KeyboardDisableJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            editor.Initialize();

            DisableJobEditors.Add(editor);
        }

        public void RemoveDisableJob(Guid keyActionId)
        {
            var editor = DisableJobEditors.First(i => i.KeyActionId == keyActionId);
            DisableJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        public void AddPressedJob(KeyActionKind kind)
        {
            if(kind == KeyActionKind.Replace || kind == KeyActionKind.Disable) {
                throw new ArgumentException(nameof(kind));
            }

            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = kind,
            };

            var editor = new KeyboardPressedJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            editor.Initialize();

            PressedJobEditors.Add(editor);
        }

        public void RemovePressedJob(Guid keyActionId)
        {
            var editor = PressedJobEditors.First(i => i.KeyActionId == keyActionId);
            PressedJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }


        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            IReadOnlyList<KeyActionData> replaceKeyActions;
            IReadOnlyList<KeyActionData> disableKeyActions;
            IReadOnlyList<KeyActionData> pressedKeyActions;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                replaceKeyActions = keyActionsEntityDao.SelectAllKeyActionsFromKind(KeyActionKind.Replace).ToList();
                disableKeyActions = keyActionsEntityDao.SelectAllKeyActionsFromKind(KeyActionKind.Disable).ToList();
                pressedKeyActions = keyActionsEntityDao.SelectAllKeyActionsIgnoreKinds(new[] { KeyActionKind.Replace, KeyActionKind.Disable }).ToList();
            }

            var replaceJobEditor = replaceKeyActions.Select(i => new KeyboardReplaceJobSettingEditorElement(i, false, MainDatabaseBarrier, StatementLoader, LoggerFactory));
            ReplaceJobEditors.SetRange(replaceJobEditor);

            var disableJobEditor = disableKeyActions.Select(i => new KeyboardDisableJobSettingEditorElement(i, false, MainDatabaseBarrier, StatementLoader, LoggerFactory));
            DisableJobEditors.SetRange(disableJobEditor);

            var pressedJobEditor = pressedKeyActions.Select(i => new KeyboardPressedJobSettingEditorElement(i, false, MainDatabaseBarrier, StatementLoader, LoggerFactory));
            PressedJobEditors.SetRange(pressedJobEditor);

            var editors = ReplaceJobEditors
                .Cast<KeyboardJobSettingEditorElementBase>()
                .Concat(DisableJobEditors)
                .Concat(PressedJobEditors)
            ;
            foreach(var editor in editors) {
                editor.Initialize();
            }
        }


        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var jobs = ReplaceJobEditors
                .Cast<KeyboardJobSettingEditorElementBase>()
                .Concat(DisableJobEditors)
                .Concat(PressedJobEditors)
            ;
            foreach(var job in jobs) {
                job.Save(commandPack.Main.Commander, commandPack.Main.Implementation, commandPack.CommonStatus);
            }
            foreach(var job in RemovedJobEditors) {
                job.Remove(commandPack.Main.Commander, commandPack.Main.Implementation);
            }
        }


        #endregion
    }

}
