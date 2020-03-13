using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPluginVersions
    {
        #region property

        /// <summary>
        /// プラグインのバージョン情報。
        /// </summary>
        Version PluginVersion { get; }

        /// <summary>
        /// プラグインが動作可能な Pe の最低バージョン(以上)。
        /// <para>0.0.0 で全バージョン稼働OK。</para>
        /// </summary>
        Version MinimumSupportVersion { get; }
        /// <summary>
        /// プラグインが動作可能な Pe の最大バージョン(以下)。
        /// <para>0.0.0 で全バージョン稼働OK。</para>
        /// </summary>
        Version MaximumSupportVersion { get; }

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IPluginVersions"/>
    /// </summary>
    public class PluginVersions : IPluginVersions
    {
        public PluginVersions(Version pluginVersion, Version minimumSupportVersion, Version maximumSupportVersion)
        {
            PluginVersion = pluginVersion;
            MinimumSupportVersion = minimumSupportVersion;
            MaximumSupportVersion = maximumSupportVersion;

        }
        #region IPluginVersion

        /// <summary>
        /// <inheritdoc cref="IPluginVersions.PluginVersion"/>
        /// </summary>
        public Version PluginVersion { get; }
        /// <summary>
        /// <inheritdoc cref="IPluginVersions.MinimumSupportVersion"/>
        /// </summary>
        public Version MinimumSupportVersion { get; }
        /// <summary>
        /// <inheritdoc cref="IPluginVersions.MaximumSupportVersion"/>
        /// </summary>
        public Version MaximumSupportVersion { get; }

        #endregion
    }

    public static class PluginLicense
    {
        #region property

        public static string Unknown => "unknown";

        public static string DoWhatTheF_ckYouWantToPublicLicense1 => "WTFPLv1";
        public static string DoWhatTheF_ckYouWantToPublicLicense2 => "WTFPLv2";
        public static string GnuGeneralPublicLicense1 => "GPLv1";
        public static string GnuGeneralPublicLicense2 => "GPLv2";
        public static string GnuGeneralPublicLicense3 => "GPLv3";

        #endregion
    }

    public interface IPluginAuthors
    {
        IAuthor PluginAuthor { get; }
        string PluginLicense { get; }
    }

    /// <summary>
    /// <inheritdoc cref="IPluginAuthors"/>
    /// </summary>
    public class PluginAuthors : IPluginAuthors
    {
        public PluginAuthors(IAuthor pluginAuthor, string pluginLicense)
        {
            PluginAuthor = pluginAuthor;
            PluginLicense = pluginLicense;
        }
        #region IPluginAuthors

        /// <summary>
        /// <inheritdoc cref="IPluginAuthors.PluginAuthor"/>
        /// </summary>
        public IAuthor PluginAuthor { get; }
        /// <summary>
        /// <inheritdoc cref="IPluginAuthors.PluginLicense"/>
        /// </summary>
        public string PluginLicense { get; }

        #endregion
    }


    public interface IPluginInformations
    {
        IPluginVersions PluginVersions { get; }
        IPluginAuthors PluginAuthors { get; }
    }

    public class PluginInformations : IPluginInformations
    {
        public PluginInformations(IPluginVersions pluginVersions, IPluginAuthors pluginAuthors)
        {
            PluginVersions = pluginVersions;
            PluginAuthors = pluginAuthors;
        }

        #region IPluginInformations

        public IPluginVersions PluginVersions { get; }

        public IPluginAuthors PluginAuthors { get; }

        #endregion
    }
}