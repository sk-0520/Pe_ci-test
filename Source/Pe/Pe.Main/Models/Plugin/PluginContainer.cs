using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginContainer
    {
        public PluginContainer(AddonContainer addonContainer, ThemeContainer themeContainer, ILoggerFactory loggerFactory)
        {
            Addon = addonContainer;
            Theme = themeContainer;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        ISet<IPlugin> Plugins { get; } = new HashSet<IPlugin>();

        public AddonContainer Addon { get; }
        public ThemeContainer Theme { get; }

        #endregion

        #region function

        public IEnumerable<IPlugin> GetPlugins()
        {
            yield return new DefaultTheme();
        }

        public void AddPlugin(IPlugin plugin)
        {
            Plugins.Add(plugin);

            if(plugin is ITheme theme) {
                Theme.Add(theme);
            }
        }

        #endregion
    }
}