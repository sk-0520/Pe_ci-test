using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupSettingEditorElement>, ILauncherGroupId
    {
        #region variable

        LauncherItemSettingEditorViewModel? _selectedLauncherItem;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItemCollection, ILauncherGroupTheme launcherGroupTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            LauncherGroupTheme = launcherGroupTheme;
            DispatcherWrapper = dispatcherWrapper;
            AllLauncherItemCollection = allLauncherItemCollection;

            LauncherCollection = new ActionModelViewModelObservableCollectionManager<WrapModel<Guid>, LauncherItemSettingEditorViewModel>(Model.LauncherItems) {
                RemoveViewModelToDispose = false, // 共有アイテムを使用しているので破棄させない
                ToViewModel = (m) => {
                    var itemVm = AllLauncherItemCollection.ViewModels.First(i => i.LauncherItemId == m.Data);
                    return itemVm.Clone();
                },
            };
            LauncherItems = LauncherCollection.ViewModels;
        }

        #region property

        /// <summary>
        /// 共用しているランチャーアイテム一覧。
        /// <para>親元でアイコンと共通項目構築済みのランチャーアイテム。毎回作るのあれだし。</para>
        /// </summary>
        [IgnoreValidation]
        ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }

        /// <summary>
        /// 所属ランチャーアイテム。
        /// <para>注意: 設定中データ状態はモデル側に送らない。</para>
        /// </summary>
        //public ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherItems { get; }
        [IgnoreValidation]
        ModelViewModelObservableCollectionManagerBase<WrapModel<Guid>, LauncherItemSettingEditorViewModel> LauncherCollection { get; }
        [IgnoreValidation]
        public ReadOnlyObservableCollection<LauncherItemSettingEditorViewModel> LauncherItems { get; }

        ILauncherGroupTheme LauncherGroupTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }


        [Required]
        public string Name
        {
            get => Model.Name;
            set
            {
                SetModelValue(value);
                ValidateProperty(value);
            }
        }

        public Color ImageColor
        {
            get => Model.ImageColor;
            set
            {
                SetModelValue(value);
                ReloadGroupIcon();
            }
        }

        public LauncherGroupImageName ImageName
        {
            get => Model.ImageName;
            set
            {
                SetModelValue(value);
                ReloadGroupIcon();
            }
        }

        //public long Sequence
        //{
        //    get => Model.Sequence;
        //    set
        //    {
        //        SetModelValue(value);
        //        ReloadGroupIcon();
        //    }
        //}

        public LauncherGroupKind Kind => Model.Kind;

        public object GroupIcon => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconBox.Small, false);

        [IgnoreValidation]
        public LauncherItemSettingEditorViewModel? SelectedLauncherItem
        {
            get => this._selectedLauncherItem;
            set
            {
                SetProperty(ref this._selectedLauncherItem, value);
            }
        }

        #endregion

        #region command


        #endregion

        #region function

        void ReloadGroupIcon()
        {
            RaisePropertyChanged(nameof(GroupIcon));
        }

        /*
        public void SaveWithoutSequence()
        {
            Model.SaveWithoutSequence();
        }
        */

        public void InsertNewLauncherItem(int index, ILauncherItemId launcherItem)
        {
            Model.InsertLauncherItemId(index, launcherItem.LauncherItemId);
        }

        public void MoveLauncherItem(int startIndex, int insertIndex)
        {
            Model.MoveLauncherItemId(startIndex, insertIndex);
        }

        public void RemoveLauncherItem(LauncherItemSettingEditorViewModel launcherItem)
        {
            var index = LauncherItems.IndexOf(launcherItem);
            Model.RemoveLauncherItemAt(index);
        }

        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId => Model.LauncherGroupId;

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    LauncherCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}