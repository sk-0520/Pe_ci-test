using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using ContentTypeTextNet.Pe.Main.Views.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar
{
    public class LauncherToolbarViewModel: ElementViewModelBase<LauncherToolbarElement>, IReadOnlyAppDesktopToolbarExtendData, IViewLifecycleReceiver
    {
        #region variable

        private LauncherDetailViewModelBase? _contextMenuOpendItem;
        private bool _showWaiting;

        #endregion

        public LauncherToolbarViewModel(LauncherToolbarElement model, IKeyGestureGuide keyGestureGuide, LauncherToolbarConfiguration launcherToolbarConfiguration, IPlatformTheme platformThemeLoader, ILauncherToolbarTheme launcherToolbarTheme, IGeneralTheme generalTheme, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            KeyGestureGuide = keyGestureGuide;
            LauncherToolbarConfiguration = launcherToolbarConfiguration;
            PlatformThemeLoader = platformThemeLoader;
            LauncherToolbarTheme = launcherToolbarTheme;
            GeneralTheme = generalTheme;

            LauncherGroupCollection = new ActionModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupViewModel>(Model.LauncherGroups) {
                ToViewModel = (m) => new LauncherGroupViewModel(m, DispatcherWrapper, LoggerFactory),
            };
            LauncherGroupItems = LauncherGroupCollection.ViewModels;

            LauncherItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemElement, LauncherDetailViewModelBase>(Model.LauncherItems) {
                ToViewModel = (m) => LauncherItemViewModelFactory.Create(m, DockScreen, KeyGestureGuide, DispatcherWrapper, LauncherToolbarTheme, LoggerFactory),
            };
            LauncherItems = LauncherItemCollection.GetDefaultView();

            ViewDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = ViewCanDragStart,
                DragEnterAction = ViewDragOrverOrEnter,
                DragOverAction = ViewDragOrverOrEnter,
                DragLeaveAction = ViewDragLeave,
                DropAction = ViewDrop,
                GetDragParameter = ViewGetDragParameter,
            };
            ItemDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = ItemCanDragStart,
                DragEnterAction = ItemDragOrverOrEnter,
                DragOverAction = ItemDragOrverOrEnter,
                DragLeaveAction = ItemDragLeave,
                DropAction = ItemDrop,
                GetDragParameter = ItemGetDragParameter,
            };

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddProperties<IReadOnlyAppDesktopToolbarExtendData>();
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), nameof(IsVerticalLayout));
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.ToolbarPosition), ChangeToolbarPositionCommand);
            PropertyChangedHooker.AddHook(nameof(IAppDesktopToolbarExtendData.IsAutoHide), nameof(IsAutoHide));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendAppMenu), nameof(IsOpendAppMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendFileItemMenu), nameof(IsOpendFileItemMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendStoreAppItemMenu), nameof(IsOpendStoreAppItemMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsOpendAddonItemMenu), nameof(IsOpendAddonItemMenu));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.IsTopmost), nameof(IsTopmost));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.SelectedLauncherGroup), nameof(SelectedLauncherGroup));
            PropertyChangedHooker.AddHook(nameof(LauncherToolbarElement.ExistsFullScreenWindow), nameof(ExistsFullScreenWindow));

            PlatformThemeLoader.Changed += PlatformThemeLoader_Changed;
            ThemeProperties = new ThemeProperties(this);
        }

        #region property

        public RequestSender ExpandShortcutFileRequest { get; } = new RequestSender();

        public AppDesktopToolbarExtend? AppDesktopToolbarExtend { get; set; }
        private IKeyGestureGuide KeyGestureGuide { get; }
        private LauncherToolbarConfiguration LauncherToolbarConfiguration { get; }
        private IPlatformTheme PlatformThemeLoader { get; }
        private ILauncherToolbarTheme LauncherToolbarTheme { get; }
        private IGeneralTheme GeneralTheme { get; }
        private PropertyChangedHooker PropertyChangedHooker { get; }
        private ThemeProperties ThemeProperties { get; }

        private DispatcherTimer? AutoHideShowWaitTimer { get; set; }

        public IconBox IconBox => Model.IconBox;
        public Thickness ButtonPadding => Model.ButtonPadding;
        public Thickness IconMargin => Model.IconMargin;
        public bool IsIconOnly => Model.IsIconOnly;
        public double TextWidth => Model.TextWidth;

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        public bool IsTopmost
        {
            get => Model.IsTopmost;
        }

        [ThemeProperty]
        public object ToolbarMainIcon
        {
            get => GeneralTheme.GetPathImage(GeneralPathImageKind.Menu, new IconScale(IconBox, IconSize.DefaultScale));
        }

        [ThemeProperty]
        public Brush ToolbarBackground
        {
            get => LauncherToolbarTheme.GetToolbarBackground(ToolbarPosition, ViewState.Active, new IconScale(IconBox, IconSize.DefaultScale), IsIconOnly, TextWidth);
        }

        [ThemeProperty]
        public Brush ToolbarForeground
        {
            get => LauncherToolbarTheme.GetToolbarForeground();
        }

        public LauncherToolbarIconDirection IconDirection => Model.IconDirection;

        public bool IsOpendAppMenu
        {
            get => Model.IsOpendAppMenu;
            set => SetModelValue(value);
        }

        public bool IsOpendFileItemMenu
        {
            get => Model.IsOpendFileItemMenu;
            set => SetModelValue(value);
        }
        public bool IsOpendStoreAppItemMenu
        {
            get => Model.IsOpendStoreAppItemMenu;
            set => SetModelValue(value);
        }
        public bool IsOpendAddonItemMenu
        {
            get => Model.IsOpendAddonItemMenu;
            set => SetModelValue(value);
        }


        public LauncherDetailViewModelBase? ContextMenuOpendItem
        {
            get => this._contextMenuOpendItem;
            set => SetProperty(ref this._contextMenuOpendItem, value);
        }

        public bool ShowWaiting
        {
            get => this._showWaiting;
            set => SetProperty(ref this._showWaiting, value);
        }

        private ModelViewModelObservableCollectionManagerBase<LauncherGroupElement, LauncherGroupViewModel> LauncherGroupCollection { get; }
        public ReadOnlyObservableCollection<LauncherGroupViewModel> LauncherGroupItems { get; }

        private ModelViewModelObservableCollectionManagerBase<LauncherItemElement, LauncherDetailViewModelBase> LauncherItemCollection { get; }
        public ICollectionView LauncherItems { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();

        public bool IsVerticalLayout => ToolbarPosition == AppDesktopToolbarPosition.Left || ToolbarPosition == AppDesktopToolbarPosition.Right;

        public FontViewModel Font { get; }
        public IDragAndDrop ViewDragAndDrop { get; }
        public IDragAndDrop ItemDragAndDrop { get; }

        public LauncherGroupViewModel? SelectedLauncherGroup
        {
            get => Model?.SelectedLauncherGroup != null ? LauncherGroupCollection.GetViewModel(Model.SelectedLauncherGroup) : null;
        }

        public LauncherToolbarContentDropMode ContentDropMode => Model.ContentDropMode;
        public LauncherGroupPosition GroupMenuPosition => Model.GroupMenuPosition;

        private LauncherToolbarIconMaker IconMaker { get; } = new LauncherToolbarIconMaker();

        #region theme

        [ThemeProperty]
        public DependencyObject ToolbarPositionLeftIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Left, IconBox.Small);
        [ThemeProperty]
        public DependencyObject ToolbarPositionTopIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Top, IconBox.Small);
        [ThemeProperty]
        public DependencyObject ToolbarPositionRightIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Right, IconBox.Small);
        [ThemeProperty]
        public DependencyObject ToolbarPositionBottomIcon => IconMaker.GetToolbarPositionImage(AppDesktopToolbarPosition.Bottom, IconBox.Small);

        [ThemeProperty]
        public ControlTemplate LauncherItemNormalButtonControlTemplate => LauncherToolbarTheme.GetLauncherItemNormalButtonControlTemplate();
        [ThemeProperty]
        public ControlTemplate LauncherItemToggleButtonControlTemplate => LauncherToolbarTheme.GetLauncherItemToggleButtonControlTemplate();

        #endregion

        #endregion

        #region command

        public ICommand ChangeToolbarPositionCommand => GetOrCreateCommand(() => new DelegateCommand<AppDesktopToolbarPosition?>(
            o => {
                if(o.HasValue) {
                    Model.ChangeToolbarPositionDelaySave(o.Value);
                    ChangeLauncherGroupCommand.ExecuteIfCanExecute(SelectedLauncherGroup);
                } else {
                    Logger.LogTrace("???????????????");
                }
            },
            o => o.HasValue && o.Value != ToolbarPosition
        ));

        public ICommand ToggleTopmostCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ChangeTopmostDelaySave(!Model.IsTopmost);
            }
        ));

        public ICommand ToggleAutoHideCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ChangeAutoHideDelaySave(!Model.IsAutoHide);
            }
        ));

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.ChangeVisibleDelaySave(false);

                 //var notification = new Notification();
                 //CloseRequest.Raise(notification);
                 CloseRequest.Send();
             }
        ));

        public ICommand ChangeLauncherGroupCommand => GetOrCreateCommand(() => new DelegateCommand<LauncherGroupViewModel>(
           o => {
               ChangeLauncherGroup(o);
           }
       ));

        public ICommand RemoveCommand => GetOrCreateCommand(() => new DelegateCommand<LauncherDetailViewModelBase>(
            o => {
                Debug.Assert(SelectedLauncherGroup != null);

                var targetIndex = LauncherItemCollection.ViewModels
                    .Where(i => i.LauncherItemId == o.LauncherItemId)
                    .Counting()
                    .First(i => i.Value == o)
                    .Number
                ;

                Model.RemoveLauncherItem(SelectedLauncherGroup.LauncherGroupId, o.LauncherItemId, targetIndex);
            }
        ));

        public ICommand AutoHideToHideCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                HideAndShowWaiting();
            }
        ));

        public ICommand PreviewMouseDownCommand => GetOrCreateCommand(() => new DelegateCommand<MouseButtonEventArgs>(
            o => {
                if(SelectedLauncherGroup == null) {
                    Logger.LogError("????????????");
                    return;
                }
                if(LauncherGroupCollection.Count == 1) {
                    Logger.LogDebug("????????????????????????");
                    return;
                }

                var prev = o.MouseDevice.XButton1.HasFlag(MouseButtonState.Pressed);
                var next = o.MouseDevice.XButton2.HasFlag(MouseButtonState.Pressed);
                if((prev || next) && o.ButtonState == MouseButtonState.Pressed) {

                    var currentIndex = LauncherGroupCollection.IndexOf(SelectedLauncherGroup);
                    int nextIndex;
                    if(prev) {
                        nextIndex = currentIndex == 0
                            ? LauncherGroupCollection.Count - 1
                            : currentIndex - 1
                        ;
                    } else {
                        Debug.Assert(next);
                        nextIndex = currentIndex == LauncherGroupCollection.Count - 1
                            ? 0
                            : currentIndex + 1
                        ;
                    }

                    var vm = LauncherGroupCollection.ViewModels[nextIndex];
                    ChangeLauncherGroup(vm);

                    o.Handled = true;
                }
            }
        ));

        public ICommand AddNewGroupCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 var newGroupElement = Model.AddNewGroup(LauncherGroupKind.Normal);
                 var newGroupViewModel = LauncherGroupCollection.GetViewModel(newGroupElement);
                 if(newGroupViewModel is null) {
                     Logger.LogError("????????????????????????????????????????????????: {0}", newGroupElement.LauncherGroupId);
                     return;
                 }
                 ChangeLauncherGroup(newGroupViewModel);
             }
         ));

        #endregion

        #region function

        private void ChangeLauncherGroup(LauncherGroupViewModel targetGroup)
        {
            var currentLauncherItems = LauncherItemCollection.ViewModels.ToList();

            var groupModel = LauncherGroupCollection.GetModel(targetGroup)!;
            Model.ChangeLauncherGroup(groupModel);
            LauncherItems.Refresh();

            foreach(var vm in currentLauncherItems) {
                vm.Dispose();
            }
        }


        #region ViewDragAndDrop

        private IResultSuccessValue<DragParameter> ViewGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.GetDragParameter(sender, e);
        }

        private bool ViewCanDragStart(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.CanDragStart(sender, e);
        }


        private void ViewDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragOrverOrEnter(sender, e);
        }

        private void ViewDrop(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.Drop(sender, e, s => dd.RegisterDropFile(ExpandShortcutFileRequest, s, Model.RegisterFile));
        }

        private void ViewDragLeave(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragLeave(sender, e);
        }

        #endregion

        #region ItemDragAndDrop
        private IResultSuccessValue<DragParameter> ItemGetDragParameter(UIElement sender, MouseEventArgs e) => ResultSuccessValue.Failure<DragParameter>();

        private bool ItemCanDragStart(UIElement sender, MouseEventArgs e) => false;

        private void ItemDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.Move;
            } else if(e.Data.IsTextPresent()) {
                e.Effects = DragDropEffects.Move;
            }

            e.Handled = true;
        }

        private void ItemDrop(UIElement sender, DragEventArgs e)
        {
            LauncherItemId launcherItemId = LauncherItemId.Empty;
            var frameworkElement = (FrameworkElement)sender;
            var launcherContentControl = (LauncherContentControl)frameworkElement.DataContext;
            if(launcherContentControl != null) {
                var launcherItem = (ILauncherItemId)launcherContentControl.DataContext;
                launcherItemId = launcherItem.LauncherItemId;
            }

            if(LauncherItemId.Empty == launcherItemId) {
                Logger.LogError("???????????????????????????ID???????????????, {0}, {1}", sender, e);
                return;
            }

            if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                var argument = string.Join(' ', filePaths.Select(i => CommandLine.Escape(i)));
                DispatcherWrapper.Begin(() => ExecuteExtendDropData(launcherItemId, argument));
            } else if(e.Data.IsTextPresent()) {
                var argument = TextUtility.JoinLines(e.Data.GetText());
                DispatcherWrapper.Begin(() => ExecuteExtendDropData(launcherItemId, argument));
            }

            e.Handled = true;
        }

        private void ItemDragLeave(UIElement sender, DragEventArgs e)
        { }

        #endregion

        void ExecuteExtendDropData(LauncherItemId launcherItemId, string argument)
        {
            switch(ContentDropMode) {
                case LauncherToolbarContentDropMode.ExtendsExecute:
                    Model.OpenExtendsExecuteView(launcherItemId, argument, DockScreen);
                    break;

                case LauncherToolbarContentDropMode.DirectExecute:
                    Model.ExecuteWithArgument(launcherItemId, argument);
                    break;
            }
        }

        /// <summary>
        /// ?????????????????????????????????????????????????????????????????????????????????
        /// </summary>
        public void HideAndShowWaiting()
        {
            if(!IsVisible) {
                return;
            }
            if(!IsAutoHide) {
                return;
            }
            if(IsHiding) {
                return;
            }

            DispatcherWrapper.VerifyAccess();

            if(TimeSpan.Zero < LauncherToolbarConfiguration.AutoHideShowWaitTime) {
                if(AutoHideShowWaitTimer != null) {
                    AutoHideShowWaitTimer.Stop();
                    AutoHideShowWaitTimer.Tick -= AutoHideShowWaitTimer_Tick;
                    AutoHideShowWaitTimer = null;
                }

                AutoHideShowWaitTimer = new DispatcherTimer(DispatcherPriority.Background) {
                    Interval = LauncherToolbarConfiguration.AutoHideShowWaitTime,
                };
                AutoHideShowWaitTimer.Tick += AutoHideShowWaitTimer_Tick;
            }

            AppDesktopToolbarExtend!.HideView(true);
            if(AutoHideShowWaitTimer != null) {
                Logger.LogTrace("???????????????????????????????????????????????????????????????");
                ShowWaiting = true;
                AutoHideShowWaitTimer.Start();
            }
        }

        #endregion

        #region IAppDesktopToolbarExtendData

        /// <summary>
        /// ????????????????????????
        /// </summary>
        public AppDesktopToolbarPosition ToolbarPosition => Model.ToolbarPosition;

        /// <summary>
        /// ????????????????????????
        /// </summary>
        public bool IsDocking => Model.IsDocking;

        /// <summary>
        /// ????????????????????????
        /// </summary>
        public bool IsAutoHide => Model.IsAutoHide;
        /// <summary>
        /// ?????????????????????????????????????????????????????????
        /// </summary>
        public bool PausingAutoHide => Model.PausingAutoHide;
        /// <summary>
        /// ?????????????????????
        /// </summary>
        public bool IsHiding => Model.IsHiding;
        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        public TimeSpan AutoHideTime => Model.AutoHideTime;

        /// <summary>
        /// ????????????????????????
        /// <para><see cref="AppDesktopToolbarPosition"/>??????????????????</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size DisplaySize => Model.DisplaySize;

        /// <summary>
        /// ????????????????????????????????????
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect DisplayBarArea => Model.DisplayBarArea;
        /// <summary>
        /// ??????????????????????????????????????????
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size HiddenSize => Model.HiddenSize;
        /// <summary>
        /// ?????????????????????????????????????????????
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HiddenBarArea => Model.HiddenBarArea;

        /// <summary>
        /// ?????????????????????????????????????????????????????????
        /// </summary>
        public bool ExistsFullScreenWindow => Model.ExistsFullScreenWindow;


        /// <summary>
        /// ???????????????????????????
        /// </summary>
        public IScreen DockScreen => Model.DockScreen;

        #endregion

        //#region ILoggerFactory

        //public ILogger CreateLogger(string header) => LoggerFactory.CreateLogger(header);

        //#endregion

        #region IViewLifecycleReceiver
        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            if(!IsVisible) {
                window.Visibility = Visibility.Collapsed;
            }
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }


        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
        }


        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();
            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();
            Model.PropertyChanged -= Model_PropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PlatformThemeLoader.Changed -= PlatformThemeLoader_Changed;
                    LauncherItemCollection.Dispose();
                    LauncherGroupCollection.Dispose();
                    Font.Dispose();
                }

            }
            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

        private void PlatformThemeLoader_Changed(object? sender, EventArgs e)
        {
            DispatcherWrapper.Begin(vm => {
                if(vm.IsDisposed) {
                    return;
                }

                foreach(var propertName in vm.ThemeProperties.GetPropertyNames()) {
                    vm.RaisePropertyChanged(propertName);
                }
            }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void AutoHideShowWaitTimer_Tick(object? sender, EventArgs e)
        {
            Logger.LogTrace("???????????????????????????????????????????????????????????????");
            ShowWaiting = false;

            if(AutoHideShowWaitTimer != null) {
                AutoHideShowWaitTimer.Tick -= AutoHideShowWaitTimer_Tick;
                AutoHideShowWaitTimer.Stop();
                AutoHideShowWaitTimer = null;
            }
        }
    }
}
