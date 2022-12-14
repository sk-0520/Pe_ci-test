using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginInstallItemViewModel: SingleModelViewModelBase<PluginInstallItemElement>
    {
        public PluginInstallItemViewModel(PluginInstallItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public PluginId PluginId => Model.Data.PluginId;
        public string PluginName => Model.Data.PluginName;
        public Version PluginVersion => Model.Data.PluginVersion;
        public PluginInstallMode InstallMode => Model.Data.PluginInstallMode;

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
