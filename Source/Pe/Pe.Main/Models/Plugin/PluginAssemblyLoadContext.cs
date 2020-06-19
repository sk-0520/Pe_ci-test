using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグイン読み込み用処理。
    /// </summary>
    public class PluginAssemblyLoadContext: AssemblyLoadContext
    {
        public PluginAssemblyLoadContext(FileInfo pluginFile, IReadOnlyList<DirectoryInfo> libraryDirectories, ILoggerFactory loggerFactory)
            : this(pluginFile, libraryDirectories, true, loggerFactory)
        { }

        public PluginAssemblyLoadContext(FileInfo pluginFile, IReadOnlyList<DirectoryInfo> libraryDirectories, bool isCollectible, ILoggerFactory loggerFactory)
            : base(isCollectible)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginFile = pluginFile;
            AssemblyDependencyResolver = new AssemblyDependencyResolver(Path.GetDirectoryName(PluginFile.FullName)!);
            LibraryDependencyResolvers = libraryDirectories.Select(i => new AssemblyDependencyResolver(i.FullName)).ToArray();
        }

        #region property

        FileInfo PluginFile { get; }
        ILogger Logger { get; }
        AssemblyDependencyResolver AssemblyDependencyResolver { get; }
        IReadOnlyList<AssemblyDependencyResolver> LibraryDependencyResolvers { get; }
        #endregion

        #region function

        public Assembly Load()
        {
            return LoadFromAssemblyPath(PluginFile.FullName);
        }

        #endregion

        #region AssemblyLoadContext

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPathFromPlugin = AssemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
            if(assemblyPathFromPlugin != null) {
                Logger.LogDebug("[{0}] 解決[plugin] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromPlugin);
                return LoadFromAssemblyPath(assemblyPathFromPlugin);
            }

            foreach(var resolver in LibraryDependencyResolvers) {
                var assemblyPathFromLibrary = resolver.ResolveAssemblyToPath(assemblyName);
                if(assemblyPathFromLibrary != null) {
                    Logger.LogDebug("[{0}] 解決[library] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromLibrary);
                    return LoadFromAssemblyPath(assemblyPathFromLibrary);
                }
            }

            var assemblyPathFromBase = base.Load(assemblyName);
            if(assemblyPathFromBase != null) {
                Logger.LogDebug("[{0}] 解決[base] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromBase);
                return assemblyPathFromBase;
            }

            Logger.LogDebug("[{0}] 未解決 {1}", PluginFile.Name, assemblyName);

            return assemblyPathFromBase;
        }

        #endregion
    }
}
