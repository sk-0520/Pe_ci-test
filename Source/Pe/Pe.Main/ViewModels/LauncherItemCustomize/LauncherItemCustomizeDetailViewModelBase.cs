using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public abstract class LauncherItemCustomizeDetailViewModelBase : SingleModelViewModelBase<LauncherItemCustomizeEditorElement>, ILauncherItemId
    {
        public LauncherItemCustomizeDetailViewModelBase(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(model) + "." + nameof(Model.IsInitialized));
            }
            DispatcherWrapper = dispatcherWrapper;

            Kind = Model.Kind;
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }

        public LauncherItemKind Kind { get; }
        #endregion

        #region command
        #endregion

        #region function

        protected abstract void InitializeImpl();

        public void Initialize()
        {
            InitializeImpl();
        }

        #endregion

        #region SingleModelViewModelBase
        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion
    }
}