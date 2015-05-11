﻿namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// 実行情報。
	/// </summary>
	[Serializable]
	public class RunningSetting: Item, ICloneable
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

		/// <summary>
		/// プログラムの実行回数。
		/// </summary>
		public int ExecuteCount { get; set; }

		#region ICloneable

		public object Clone()
		{
			return new RunningSetting() {
				CheckUpdate = this.CheckUpdate,
				CheckUpdateRC = this.CheckUpdateRC,
				Running = this.Running,
				VersionMajor = this.VersionMajor,
				VersionMinor = this.VersionMinor,
				VersionRevision = this.VersionRevision,
				VersionBuild = this.VersionBuild,
				ExecuteCount = this.ExecuteCount,
			};
		}

		#endregion

		#region function

		public void SetDefaultVersion()
		{
			var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
			VersionMajor = (ushort)assemblyVersion.Major;
			VersionMinor = (ushort)assemblyVersion.Minor;
			VersionRevision = (ushort)assemblyVersion.Revision;
			VersionBuild = (ushort)assemblyVersion.Build;
		}

		public void IncrementExecuteCount()
		{
			if(ExecuteCount < int.MaxValue) {
				ExecuteCount += 1;
			}
		}

		#endregion
	}
}