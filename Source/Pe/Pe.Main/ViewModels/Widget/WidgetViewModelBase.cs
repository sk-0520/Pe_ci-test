using System;
using System.ComponentModel;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    public abstract class WidgetViewModelBase<TWidgetElement>: ElementViewModelBase<TWidgetElement>, IViewLifecycleReceiver, IPluginId
        where TWidgetElement : WidgetElement
    {
        protected WidgetViewModelBase(TWidgetElement model, IUserTracker userTracker, IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            WindowManager = windowManager;
        }

        #region property

        protected IWindowManager WindowManager { get; }

        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public virtual void ReceiveViewInitialized(Window window)
        {
            // ツールウィンドウを強制
            UIUtility.SetToolWindowStyle(window, false, false);
        }

        public virtual void ReceiveViewLoaded(Window window)
        { }

        public virtual void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public virtual void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public virtual void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
        }

        #endregion

        #region IPluginId

        public Guid PluginId => Model.PluginId;

        #endregion
    }
}
