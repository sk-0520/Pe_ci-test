using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherGroup;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupsSettingEditorViewModel: SettingEditorViewModelBase<LauncherGroupsSettingEditorElement>
    {
        #region variable

        private bool _isPopupCreateGroupMenu;
        private LauncherGroupSettingEditorViewModel? _selectedGroup;
        private LauncherItemSettingEditorViewModel? _selectedLauncherItem;

        private string _nameFilterQuery = string.Empty;

        #endregion

        public LauncherGroupsSettingEditorViewModel(LauncherGroupsSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> allLauncherGroupCollection, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            //LauncherCollection = new ActionModelViewModelObservableCollectionManager<LauncherElementWithIconElement<CommonLauncherItemElement>, LauncherItemWithIconViewModel<CommonLauncherItemViewModel>>(Model.LauncherItems) {
            //    ToViewModel = m => LauncherItemWithIconViewModel.Create(new CommonLauncherItemViewModel(m.Element, LoggerFactory), new LauncherIcon.LauncherIconViewModel(m.Icon, DispatcherWrapper, loggerFactory), LoggerFactory),
            //};
            //LauncherItems = LauncherCollection.GetDefaultView();
            AllLauncherItemCollection = allLauncherItemCollection;
            AllLauncherItems = AllLauncherItemCollection.CreateView();
            AllLauncherItems.Filter = FilterAllLauncherItems;

            GroupCollection = allLauncherGroupCollection;
            GroupItems = GroupCollection.GetDefaultView();

            var iconMaker = new LauncherGroupIconMaker();

            var groupImageItems = Enum.GetValues<LauncherGroupImageName>()
                .OrderBy(i => (int)i)
                .Select(i => new ThemeIconViewModel<LauncherGroupImageName>(i, c => iconMaker.GetGroupImage(i, c, IconBox.Small, IconSize.DefaultScale, false), LoggerFactory))
            ;
            GroupIconItems = new ObservableCollection<ThemeIconViewModel<LauncherGroupImageName>>(groupImageItems);

            var launcherItemDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = LauncherItemCanDragStart,
                DragEnterAction = LauncherItemDragOrverOrEnter,
                DragOverAction = LauncherItemDragOrverOrEnter,
                DragLeaveAction = LauncherItemDragLeave,
                DropAction = LauncherItemDrop,
                GetDragParameter = LauncherItemGetDragParameter,
            };
            launcherItemDragAndDrop.DragStartSize = new Size(launcherItemDragAndDrop.DragStartSize.Width, 0);
            LauncherItemDragAndDrop = launcherItemDragAndDrop;

            var groupsDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = GroupsCanDragStart,
                DragEnterAction = GroupsDragOrverOrEnter,
                DragOverAction = GroupsDragOrverOrEnter,
                DragLeaveAction = GroupsDragLeave,
                DropAction = GroupsDrop,
                GetDragParameter = GroupsGetDragParameter,
            };
            groupsDragAndDrop.DragStartSize = new Size(groupsDragAndDrop.DragStartSize.Width, 0);
            GroupsDragAndDrop = groupsDragAndDrop;

            var launcherItemsDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = LauncherItemsCanDragStart,
                DragEnterAction = LauncherItemsDragOrverOrEnter,
                DragOverAction = LauncherItemsDragOrverOrEnter,
                DragLeaveAction = LauncherItemsDragLeave,
                DropAction = LauncherItemsDrop,
                GetDragParameter = LauncherItemsGetDragParameter,
            };
            launcherItemsDragAndDrop.DragStartSize = new Size(launcherItemsDragAndDrop.DragStartSize.Width, 0);
            LauncherItemsDragAndDrop = launcherItemsDragAndDrop;

            SimpleRegexFactory = new SimpleRegexFactory(LoggerFactory);
            NameFilterQueryRegex = SimpleRegexFactory.AllMatchRegex;

        }

        #region property

        private SimpleRegexFactory SimpleRegexFactory { get; }

        public IDragAndDrop GroupsDragAndDrop { get; }
        public IDragAndDrop LauncherItemDragAndDrop { get; }
        public IDragAndDrop LauncherItemsDragAndDrop { get; }

        //ModelViewModelObservableCollectionManagerBase<LauncherElementWithIconElement<CommonLauncherItemElement>, LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherCollection { get; }
        //public ICollectionView LauncherItems { get; }
        [IgnoreValidation]
        private ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ICollectionView AllLauncherItems { get; }

        [IgnoreValidation]
        private ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> GroupCollection { get; }
        [IgnoreValidation]
        public ICollectionView GroupItems { get; }


        [IgnoreValidation]
        public ObservableCollection<ThemeIconViewModel<LauncherGroupImageName>> GroupIconItems { get; }

        public bool IsPopupCreateGroupMenu
        {
            get => this._isPopupCreateGroupMenu;
            set => SetProperty(ref this._isPopupCreateGroupMenu, value);
        }

        public LauncherGroupSettingEditorViewModel? SelectedGroup
        {
            get => this._selectedGroup;
            set
            {
                var prev = this._selectedGroup;
                if(prev != null && !prev.IsDisposed) {
                    prev.PropertyChanged -= SelectedGroup_PropertyChanged;
                    //if(prev.Validate()) {
                    //    prev.SaveWithoutSequence();
                    //}
                    prev.Validate();
                }
                SetProperty(ref this._selectedGroup, value);
                if(this._selectedGroup != null) {
                    this._selectedGroup.PropertyChanged += SelectedGroup_PropertyChanged;
                }
                ChangeGroupIconColorFromCurrentGroup();

                RaisePropertyChanged(nameof(IsEnabledSelectedGroup));
            }
        }

        public bool IsEnabledSelectedGroup => SelectedGroup != null;

        public LauncherItemSettingEditorViewModel? SelectedLauncherItem
        {
            get => this._selectedLauncherItem;
            set
            {
                SetProperty(ref this._selectedLauncherItem, value);
            }
        }

        private Regex NameFilterQueryRegex { get; set; }
        public string NameFilterQuery
        {
            get => this._nameFilterQuery;
            set
            {
                SetProperty(ref this._nameFilterQuery, value);
                NameFilterQueryRegex = SimpleRegexFactory.CreateFilterRegex(this._nameFilterQuery);
                AllLauncherItems.Refresh();
            }
        }

        #endregion

        #region command

        public ICommand AddNewNormalGroupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                AddNewGroup(LauncherGroupKind.Normal);
            }
        ));

        public ICommand RemoveSelectedGroupCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 var launcherGroupId = SelectedGroup!.LauncherGroupId;
                 SelectedGroup = null;
                 Model.RemoveGroup(launcherGroupId);
             },
             () => SelectedGroup != null && 1 < GroupCollection.ViewModels.Count
         ).ObservesProperty(() => SelectedGroup));

        public ICommand UpSelectedGroupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var currentIndex = GroupCollection.IndexOf(SelectedGroup!);
                var nextIndex = currentIndex - 1;
                Model.MoveGroupItem(currentIndex, nextIndex);
                SelectedGroup = GroupCollection.ViewModels[nextIndex];
            },
            () => SelectedGroup != null && 0 < GroupCollection.IndexOf(SelectedGroup)
        ).ObservesProperty(() => SelectedGroup));

        public ICommand DownSelectedGroupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var currentIndex = GroupCollection.IndexOf(SelectedGroup!);
                var nextIndex = currentIndex + 1;
                Model.MoveGroupItem(currentIndex, nextIndex);
                SelectedGroup = GroupCollection.ViewModels[nextIndex];
            },
            () => SelectedGroup != null && GroupCollection.IndexOf(SelectedGroup) != GroupCollection.ViewModels.Count - 1
        ).ObservesProperty(() => SelectedGroup));


        public ICommand RemoveSelectedLauncherItemCommand => GetOrCreateCommand(
            () => new DelegateCommand(
                () => {
                    SelectedGroup!.RemoveLauncherItem(SelectedGroup.SelectedLauncherItem!);
                },
                () => SelectedGroup != null && SelectedGroup.SelectedLauncherItem != null
            )
            .ObservesProperty(() => SelectedGroup)
            .ObservesProperty(() => SelectedGroup!.SelectedLauncherItem)
        );

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2692:\"IndexOf\" checks should not be for positive numbers")]
        public ICommand UpSelectedLauncherItemCommand => GetOrCreateCommand(
            () => new DelegateCommand(
                () => {
                    var currentIndex = SelectedGroup!.LauncherItems.IndexOf(SelectedGroup.SelectedLauncherItem!);
                    var nextIndex = currentIndex - 1;
                    SelectedGroup.MoveLauncherItem(currentIndex, nextIndex);
                    SelectedGroup.SelectedLauncherItem = SelectedGroup!.LauncherItems[nextIndex];
                },
                () => SelectedGroup != null && SelectedGroup.SelectedLauncherItem != null && 0 < SelectedGroup!.LauncherItems.IndexOf(SelectedGroup.SelectedLauncherItem)
            )
            .ObservesProperty(() => SelectedGroup)
            .ObservesProperty(() => SelectedGroup!.SelectedLauncherItem)
        );

        public ICommand DownSelectedLauncherItemCommand => GetOrCreateCommand(
            () => new DelegateCommand(
                () => {
                    var currentIndex = SelectedGroup!.LauncherItems.IndexOf(SelectedGroup.SelectedLauncherItem!);
                    var nextIndex = currentIndex + 1;
                    SelectedGroup.MoveLauncherItem(currentIndex, nextIndex);
                    SelectedGroup.SelectedLauncherItem = SelectedGroup!.LauncherItems[nextIndex];
                },
                () => SelectedGroup != null && SelectedGroup.SelectedLauncherItem != null && SelectedGroup.LauncherItems.IndexOf(SelectedGroup.SelectedLauncherItem) != SelectedGroup.LauncherItems.Count - 1
            )
            .ObservesProperty(() => SelectedGroup)
            .ObservesProperty(() => SelectedGroup!.SelectedLauncherItem)
        );

        public ICommand AddSelectedLauncheritemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                SelectedGroup!.InsertNewLauncherItem(SelectedGroup.LauncherItems.Count, SelectedLauncherItem!);
                SelectedGroup.SelectedLauncherItem = SelectedGroup.LauncherItems[SelectedGroup.LauncherItems.Count - 1];
            },
            () => SelectedLauncherItem != null
        ).ObservesProperty(() => SelectedLauncherItem));

        #endregion

        #region function
        private void AddNewGroup(LauncherGroupKind kind)
        {
            IsPopupCreateGroupMenu = false;

            var launcherGroupId = Model.AddNewGroup(kind);
            /*
            var newLauncherGroupId = Model.CreateNewGroup(kind);
            var newItem = ItemCollection.ViewModels.First(i => i.LauncherItemId == newLauncherItemId);
            SelectedItem = newItem;
            ScrollSelectedItemRequest.Send();
            */
            SelectedGroup = GroupCollection.ViewModels.First(i => i.LauncherGroupId == launcherGroupId);
            //ScrollSelectedItemRequest.Send();

        }

        private void ChangeGroupIconColorFromCurrentGroup()
        {
            if(SelectedGroup != null) {
                foreach(var groupIcon in GroupIconItems) {
                    groupIcon.ChangeColor(SelectedGroup.ImageColor);
                }
            }
        }

        #region DragAndDrop

        private bool LauncherItemCanDragStart(UIElement sender, MouseEventArgs e)
        {
            return true;
        }

        private void LauncherItemDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            var canDrag = false;
            if(e.Data.TryGet<LauncherItemDragData>(out var dragData)) {
                if(dragData.FromAllItems) {
                    canDrag = true;
                } else {
                    if(e.OriginalSource is DependencyObject dependencyObject) {
                        var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                        if(listBoxItem != null) {
                            var currentItem = (LauncherItemSettingEditorViewModel)listBoxItem.DataContext;
                            if(currentItem != dragData.Item) {
                                canDrag = true;
                            }
                        }
                    }
                }
            }

            if(canDrag) {
                Debug.Assert(dragData != null);
                if(dragData.FromAllItems) {
                    e.Effects = DragDropEffects.Copy;
                } else {
                    e.Effects = DragDropEffects.Move;
                }
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void LauncherItemDragLeave(UIElement sender, DragEventArgs e)
        { }

        private void LauncherItemDrop(UIElement sender, DragEventArgs e)
        {
            if(SelectedGroup == null) {
                return;
            }

            if(e.Data.TryGet<LauncherItemDragData>(out var dragData)) {
                if(e.OriginalSource is DependencyObject dependencyObject) {
                    var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                    if(listBoxItem != null) {
                        var currentItem = (LauncherItemSettingEditorViewModel)listBoxItem.DataContext;
                        if(dragData.FromAllItems) {
                            // アイテム一覧からD&Dされた
                            var currentIndex = SelectedGroup.LauncherItems.IndexOf(currentItem);
                            // 複製しておかないと選択状態が死ぬ
                            //var baseLauncherItem = LauncherCollection.ViewModels.First(i => i == dragData.Item);
                            //var newLauncherItem = new LauncherItemWithIconViewModel<CommonLauncherItemViewModel>(baseLauncherItem.Item, baseLauncherItem.Icon, LoggerFactory);
                            //SelectedGroup.LauncherItems.Insert(currentIndex, newLauncherItem);
                            SelectedGroup.InsertNewLauncherItem(currentIndex, dragData.Item);
                            SelectedLauncherItem = AllLauncherItemCollection.ViewModels[currentIndex];
                            UIUtility.GetVisualClosest<ListBox>(listBoxItem)!.Focus();
                        } else {
                            // 現在アイテム内での並び替え
                            var selfIndex = SelectedGroup.LauncherItems.IndexOf(dragData.Item);
                            var currentIndex = SelectedGroup.LauncherItems.IndexOf(currentItem);

                            // 自分自身より上のアイテムであれば自分自身をさらに上に設定
                            SelectedGroup.MoveLauncherItem(selfIndex, currentIndex);
                            /*
                            if(currentIndex < selfIndex) {
                                SelectedGroup.LauncherItems.RemoveAt(selfIndex);
                                SelectedGroup.LauncherItems.Insert(currentIndex, dragData.Item);
                            } else {
                                // 自分自身より下のアイテムであれば自分自身をさらに下に設定
                                Debug.Assert(selfIndex < currentIndex);
                                SelectedGroup.LauncherItems.RemoveAt(selfIndex);
                                SelectedGroup.LauncherItems.Insert(currentIndex, dragData.Item); // 自分消えてるからインデックスずれていいかんじになるはず
                            }
                            */
                            SelectedGroup.SelectedLauncherItem = SelectedGroup!.LauncherItems[currentIndex];
                        }
                    } else if(dragData.FromAllItems) {
                        // 一覧から持ってきた際にデータが空っぽだとここ
                        /*
                        var baseLauncherItem = LauncherCollection.ViewModels.First(i => i == dragData.Item);
                        var newLauncherItem = new LauncherItemWithIconViewModel<CommonLauncherItemViewModel>(baseLauncherItem.Item, baseLauncherItem.Icon, LoggerFactory);
                        SelectedGroup.LauncherItems.Add(newLauncherItem);
                        SelectedLauncherItem = newLauncherItem;
                        */
                        SelectedGroup.InsertNewLauncherItem(SelectedGroup.LauncherItems.Count, dragData.Item);
                        SelectedGroup.SelectedLauncherItem = SelectedGroup!.LauncherItems.Last();
                        UIUtility.GetVisualClosest<ListBox>(dependencyObject)!.Focus();
                    }
                }
            }
        }

        private IResultSuccess<DragParameter> LauncherItemGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherItemInLauncherGroupDragAndDrop(DispatcherWrapper, LoggerFactory);
            var parameter = dd.GetDragParameter(false, sender, e, d => {
                SelectedLauncherItem = d;
            });
            return parameter;
        }

        #endregion

        #region GroupsDragAndDrop
        private bool GroupsCanDragStart(UIElement sender, MouseEventArgs e)
        {
            return true;
        }

        private void GroupsDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            var canDrag = false;
            if(e.Data.TryGet<LauncherGroupSettingEditorViewModel>(out var drafItem)) {
                if(e.OriginalSource is DependencyObject dependencyObject) {
                    var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                    if(listBoxItem != null) {
                        var currentItem = (LauncherGroupSettingEditorViewModel)listBoxItem.DataContext;
                        if(currentItem != drafItem) {
                            canDrag = true;
                        }
                    }
                }
            }

            if(canDrag) {
                e.Effects = DragDropEffects.Move;
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;

        }

        private void GroupsDragLeave(UIElement sender, DragEventArgs e)
        { }

        private void GroupsDrop(UIElement sender, DragEventArgs e)
        {
            if(SelectedGroup == null) {
                return;
            }
            if(e.Data.TryGet<LauncherGroupSettingEditorViewModel>(out var dragData)) {
                if(e.OriginalSource is DependencyObject dependencyObject) {
                    var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                    if(listBoxItem != null) {
                        var currentItem = (LauncherGroupSettingEditorViewModel)listBoxItem.DataContext;
                        // 現在アイテム内での並び替え
                        var selfIndex = GroupCollection.IndexOf(dragData);
                        var currentIndex = GroupCollection.IndexOf(currentItem);

                        Model.MoveGroupItem(selfIndex, currentIndex);

                        //// 自分自身より上のアイテムであれば自分自身をさらに上に設定
                        //if(currentIndex < selfIndex) {
                        //    GroupCollection.ViewModels.RemoveAt(selfIndex);
                        //    GroupCollection.ViewModels.Insert(currentIndex, dragData);
                        //} else {
                        //    // 自分自身より下のアイテムであれば自分自身をさらに下に設定
                        //    Debug.Assert(selfIndex < currentIndex);
                        //    GroupCollection.ViewModels.RemoveAt(selfIndex);
                        //    GroupCollection.ViewModels.Insert(currentIndex, dragData);
                        //}
                        SelectedGroup = GroupCollection.ViewModels[currentIndex];
                    }
                }
            }
        }

        private IResultSuccess<DragParameter> GroupsGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            if(e.Source is ListBox listbox) {
                var scollbar = UIUtility.GetVisualClosest<ScrollBar>((DependencyObject)e.OriginalSource);
                if(scollbar == null && listbox.SelectedItem != null) {
                    var item = (LauncherGroupSettingEditorViewModel)listbox.SelectedItem;
                    SelectedGroup = item;
                    var data = new DataObject(typeof(LauncherGroupSettingEditorViewModel), item);
                    return Result.CreateSuccess(new DragParameter(sender, DragDropEffects.Move, data));
                }
            }

            return Result.CreateFailure<DragParameter>();
        }

        #endregion

        #region LauncherItemsDragAndDrop
        private bool LauncherItemsCanDragStart(UIElement sender, MouseEventArgs e)
        {
            return true;
        }

        private void LauncherItemsDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void LauncherItemsDragLeave(UIElement sender, DragEventArgs e)
        { }

        private void LauncherItemsDrop(UIElement sender, DragEventArgs e)
        { }

        private IResultSuccess<DragParameter> LauncherItemsGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherItemInLauncherGroupDragAndDrop(DispatcherWrapper, LoggerFactory);
            var parameter = dd.GetDragParameter(true, sender, e, d => {
                SelectedLauncherItem = d;
            });
            return parameter;
        }

        #endregion

        #region AllLauncherItemItems

        private bool FilterAllLauncherItems(object obj)
        {
            if(string.IsNullOrWhiteSpace(NameFilterQuery)) {
                return true;
            }

            var item = (LauncherItemSettingEditorViewModel)obj;
            if(item == SelectedLauncherItem) {
                return true;
            }
            return NameFilterQueryRegex.IsMatch(item.Common.Name);
        }

        #endregion

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_LauncherGroups_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }


        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(SelectedGroup != null) {
                        SelectedGroup.PropertyChanged -= SelectedGroup_PropertyChanged;
                    }

                    GroupCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //public override void Load()
        //{
        //    SelectedGroup = null;
        //    SelectedLauncherItem = null;
        //    base.Load();
        //}
        //public override void Save()
        //{
        //    SelectedGroup?.SaveWithoutSequence();
        //    base.Save();
        //}

        #endregion

        private void SelectedGroup_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedGroup.ImageColor)) {
                ChangeGroupIconColorFromCurrentGroup();
            }
        }
    }
}
