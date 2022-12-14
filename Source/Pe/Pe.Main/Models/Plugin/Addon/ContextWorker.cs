using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public abstract class ContextWorkerBase<TPluginContextFactory>
        where TPluginContextFactory : PluginContextFactoryBase
    {
        protected ContextWorkerBase(TPluginContextFactory pluginContextFactory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginContextFactory = pluginContextFactory;
        }

        #region property

        protected TPluginContextFactory PluginContextFactory { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        #endregion
    }

    public class ContextWorker: ContextWorkerBase<PluginContextFactory>, IContextWorker
    {
        public ContextWorker(PluginContextFactory pluginContextFactory, ILoggerFactory loggerFactory)
            : base(pluginContextFactory, loggerFactory)
        { }

        #region IContextWorker

        public void RunPlugin(Action<IPluginContext> callback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class LauncherItemAddonContextWorker: ContextWorkerBase<LauncherItemAddonContextFactory>, ILauncherItemAddonContextWorker
    {
        public LauncherItemAddonContextWorker(LauncherItemAddonContextFactory launcherItemAddonContextFactory, IPluginInformations pluginInformations, LauncherItemId launcherItemId, ILoggerFactory loggerFactory)
            : base(launcherItemAddonContextFactory, loggerFactory)
        {
            PluginInformations = pluginInformations;
            LauncherItemId = launcherItemId;
        }

        #region property
        public IPluginInformations PluginInformations { get; }
        public LauncherItemId LauncherItemId { get; }

        #endregion

        #region ILauncherItemAddonContextWorker

        public void RunLauncherItemAddon(Action<ILauncherItemAddonContext> callback)
        {
            using var databaseContextsPack = PluginContextFactory.BarrierWrite();
            using var context = PluginContextFactory.CreateContext(PluginInformations, LauncherItemId, databaseContextsPack, false);
            callback(context);
        }

        #endregion
    }
}
