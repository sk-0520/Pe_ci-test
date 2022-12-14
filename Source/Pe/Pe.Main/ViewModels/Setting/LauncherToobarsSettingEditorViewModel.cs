using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherToobarsSettingEditorViewModel: SettingEditorViewModelBase<LauncherToobarsSettingEditorElement>
    {
        #region variable

        private LauncherToobarSettingEditorViewModel? _selectedToolbar;

        #endregion
        public LauncherToobarsSettingEditorViewModel(LauncherToobarsSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> allLauncherGroupCollection, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AllLauncherGroupCollection = allLauncherGroupCollection;
            AllLauncherGroupItems = AllLauncherGroupCollection.CreateView();
            GeneralTheme = generalTheme;
            ToolbarCollection = new ActionModelViewModelObservableCollectionManager<LauncherToobarSettingEditorElement, LauncherToobarSettingEditorViewModel>(Model.Toolbars) {
                ToViewModel = m => new LauncherToobarSettingEditorViewModel(m, AllLauncherGroupCollection, () => IsSelected, GeneralTheme, DispatcherWrapper, LoggerFactory),
            };
            ToolbarItems = ToolbarCollection.GetDefaultView();
        }

        #region property
        private IGeneralTheme GeneralTheme { get; }
        public RequestSender ShowAllScreensRequest { get; } = new RequestSender();

        private ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> AllLauncherGroupCollection { get; }
        public ICollectionView AllLauncherGroupItems { get; }

        private ModelViewModelObservableCollectionManagerBase<LauncherToobarSettingEditorElement, LauncherToobarSettingEditorViewModel> ToolbarCollection { get; }
        public ICollectionView ToolbarItems { get; }

        public LauncherToobarSettingEditorViewModel? SelectedToolbar
        {
            get => this._selectedToolbar;
            set => SetProperty(ref this._selectedToolbar, value);
        }

        #endregion

        #region function

        #endregion

        #region command

        public ICommand ShowAllScreensCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowAllScreensRequest.Send();
            }
        ));


        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_LauncherToolbars_Header;

        public override void Flush()
        {
        }

        public override void Load()
        {
            base.Load();
            SelectedToolbar = ToolbarCollection.ViewModels.First();
        }

        public override void Refresh()
        {
            SelectedToolbar?.Refresh();
        }

        #endregion
    }
}
