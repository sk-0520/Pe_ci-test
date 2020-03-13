using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface ISettingEditorViewModel: IFlushable
    {
        #region property

        string Header { get; }
        bool IsSelected { get; set; }

        #endregion

        #region function

        void Load();
        void Refresh();

        #endregion
    }

    public abstract class SettingEditorViewModelBase<TSettingEditorElement> : SingleModelViewModelBase<TSettingEditorElement>, ISettingEditorViewModel
        where TSettingEditorElement : SettingEditorElementBase
    {
        #region variable

        bool _isSelected;

        #endregion

        public SettingEditorViewModelBase(TSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region ISettingEditorViewModel

        public abstract string Header { get; }
        public bool IsSelected
        {
            get => this._isSelected;
            set => SetProperty(ref this._isSelected, value);
        }

        public virtual void Load()
        {
            if(Model.IsLoaded) {
                Refresh();
                return;
            }
            Model.Load();
        }

        public abstract void Flush();

        public abstract void Refresh();

        #endregion

    }
}