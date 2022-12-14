using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class KeyboardSettingEditorViewModel: SettingEditorViewModelBase<KeyboardSettingEditorElement>
    {
        #region variable

        private bool _isPopupCreateJobMenu;

        #endregion

        public KeyboardSettingEditorViewModel(KeyboardSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ReplaceJobEditorCollection = new ActionModelViewModelObservableCollectionManager<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode>(model.ReplaceJobEditors) {
                ToViewModel = m => new KeyboardReplaceJobSettingEditorViewMode(m, DispatcherWrapper, LoggerFactory),
            };
            ReplaceJobEditors = ReplaceJobEditorCollection.GetDefaultView();

            DisableJobEditorCollection = new ActionModelViewModelObservableCollectionManager<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel>(Model.DisableJobEditors) {
                ToViewModel = m => new KeyboardDisableJobSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
            };
            DisableJobEditors = DisableJobEditorCollection.GetDefaultView();

            AllLauncherItemCollection = allLauncherItemCollection;

            PressedJobEditorCollection = new ActionModelViewModelObservableCollectionManager<KeyboardPressedJobSettingEditorElement, KeyboardPressedJobSettingEditorViewModelBase>(Model.PressedJobEditors) {
                ToViewModel = m => m.Kind switch {
                    KeyActionKind.Command => new KeyboardCommandJobSettingEditorViewModel(m, DispatcherWrapper, loggerFactory),
                    KeyActionKind.LauncherItem => new KeyboardLauncherItemJobSettingEditorViewModel(m, AllLauncherItemCollection, DispatcherWrapper, loggerFactory),
                    KeyActionKind.LauncherToolbar => new KeyboardLauncherToolbarJobSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
                    KeyActionKind.Note => new KeyboardNoteJobSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
                    _ => throw new NotImplementedException(),
                },
            };
            PressedJobEditors = PressedJobEditorCollection.GetDefaultView();


            var replaceKeyItems = Enum.GetValues<Key>()
                .Select(i => (int)i)
                .Distinct()
                .Select(i => (Key)i)
            ;
            ReplaceKeyItems = new ObservableCollection<Key>(replaceKeyItems);

            var disableKeyItems = Enum.GetValues<Key>()
                .Select(i => (int)i)
                .Distinct()
                .Select(i => (Key)i)
            ;
            DisableKeyItems = new ObservableCollection<Key>(disableKeyItems);

            var pressedIgnoreKeys = new[] {
                Key.LeftShift,
                Key.RightShift,
                Key.LeftCtrl,
                Key.RightCtrl,
                Key.LeftAlt,
                Key.RightAlt,
                Key.LWin,
                Key.RWin,
            };
            var pressedKeyItems = Enum.GetValues<Key>()
                .Select(i => (int)i)
                .Distinct()
                .Select(i => (Key)i)
                .Where(i => !pressedIgnoreKeys.Any(ii => ii == i))
            ;
            PressedKeyItems = new ObservableCollection<Key>(pressedKeyItems);

        }

        #region property

        private ModelViewModelObservableCollectionManagerBase<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode> ReplaceJobEditorCollection { get; }
        public ICollectionView ReplaceJobEditors { get; }

        private ModelViewModelObservableCollectionManagerBase<KeyboardDisableJobSettingEditorElement, KeyboardDisableJobSettingEditorViewModel> DisableJobEditorCollection { get; }
        public ICollectionView DisableJobEditors { get; }

        private ModelViewModelObservableCollectionManagerBase<KeyboardPressedJobSettingEditorElement, KeyboardPressedJobSettingEditorViewModelBase> PressedJobEditorCollection { get; }
        public ICollectionView PressedJobEditors { get; }


        [IgnoreValidation]
        private ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }

        public bool IsPopupCreateJobMenu
        {
            get => this._isPopupCreateJobMenu;
            set => SetProperty(ref this._isPopupCreateJobMenu, value);
        }

        [IgnoreValidation]
        public ObservableCollection<Key> ReplaceKeyItems { get; }
        [IgnoreValidation]
        public ObservableCollection<Key> DisableKeyItems { get; }
        [IgnoreValidation]
        public ObservableCollection<Key> PressedKeyItems { get; }

        #endregion

        #region command

        public ICommand AddReplaceJobCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.AddReplaceJob();
             }
         ));
        public ICommand RemoveReplaceJobCommand => GetOrCreateCommand(() => new DelegateCommand<KeyboardReplaceJobSettingEditorViewMode>(
             o => {
                 Model.RemoveReplaceJob(o.KeyActionId);
             }
         ));

        public ICommand AddDisableJobCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.AddDisableJob();
             }
         ));
        public ICommand RemoveDisableJobCommand => GetOrCreateCommand(() => new DelegateCommand<KeyboardDisableJobSettingEditorViewModel>(
             o => {
                 Model.RemoveDisableJob(o.KeyActionId);
             }
         ));

        public ICommand RemovePressedJobCommand => GetOrCreateCommand(() => new DelegateCommand<KeyboardPressedJobSettingEditorViewModelBase>(
            o => {
                Model.RemovePressedJob(o.KeyActionId);
            }
        ));

        public ICommand AddCommandJobCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                AddPressedJob(KeyActionKind.Command);
            }
        ));

        public ICommand AddLauncherItemJobCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                AddPressedJob(KeyActionKind.LauncherItem);
            }
        ));

        public ICommand AddLauncherToolbarJobCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                AddPressedJob(KeyActionKind.LauncherToolbar);
            }
        ));

        public ICommand AddNoteJobCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                AddPressedJob(KeyActionKind.Note);
            }
        ));

        #endregion

        #region function

        private void AddPressedJob(KeyActionKind kind)
        {
            Model.AddPressedJob(kind);
            IsPopupCreateJobMenu = false;
        }

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Keyboard_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    ReplaceJobEditorCollection.Dispose();
                    DisableJobEditorCollection.Dispose();
                    PressedJobEditorCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
