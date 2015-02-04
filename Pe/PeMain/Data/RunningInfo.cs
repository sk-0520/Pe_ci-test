﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// 実行情報。
	/// </summary>
	[Serializable]
	public class RunningInfo: Item
	{
		/// <summary>
		/// 自動アップデートチェック。
		/// </summary>
		public bool CheckUpdate { get; set; }
		/// <summary>
		/// RC版もアップデーチェック対象とする。
		/// </summary>
		public bool CheckUpdateRC { get; set; }
		/// <summary>
		/// Peの実行許可。
		/// </summary>
		public bool Running { get; set; }

		public ushort VersionMajor { get; set; }
		public ushort VersionMinor { get; set; }
		public ushort VersionRevision { get; set; }
		public ushort VersionBuild { get; set; }

		public void SetDefaultVersion()
		{
			var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
			VersionMajor = (ushort)assemblyVersion.Major;
			VersionMinor = (ushort)assemblyVersion.Minor;
			VersionRevision = (ushort)assemblyVersion.Revision;
			VersionBuild = (ushort)assemblyVersion.Build;
		}
	}
}