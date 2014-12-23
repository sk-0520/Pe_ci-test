﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 03/07/2014
 * 時刻: 00:34
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// 各種情報を取得する。
	/// </summary>
	public class InformationCollection: IDisposable
	{
		protected ManagementClass _managementOS = new ManagementClass("Win32_OperatingSystem");
		protected ManagementClass _managementCPU = new ManagementClass("Win32_Processor");
		
		public virtual FileVersionInfo GetVersionInfo
		{
			get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
		}

		protected virtual void Dispose(bool disposing)
		{
			this._managementOS.Dispose();
			this._managementCPU.Dispose();
		}
		
		public void Dispose()
		{
			Dispose(true);
		}
		
		protected virtual InformationGroup GetInfo(ManagementClass managementClass, string groupName, IEnumerable<string> keys)
		{
			var result = new InformationGroup(groupName);
			if(keys != null) {
				using(var mc = managementClass.GetInstances()) {
					foreach(var mo in mc) {
						foreach(var key in keys) {
							try {
								result.Items[key] = mo[key];
							} catch(ManagementException ex) {
								result.Items[key] = ex;
							}
						}
					}
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// http://www.wmifun.net/library/win32_processor.html
		/// </summary>
		/// <returns></returns>
		protected virtual InformationGroup GetCPU()
		{
			var keys = new [] {
				// アドレス幅
				"AddressWidth",
				// アーキテクチャ
				"Architecture",
				// 状態
				"Availability",
				// エラーコード
				"ConfigManagerErrorCode",
				// 構成
				"ConfigManagerUserConfig",
				// 使用状況から起こる状態の変化
				"CpuStatus",
				// 現在の速度 (MHz)
				"CurrentClockSpeed",
				// 電圧
				"CurrentVoltage",
				// データ幅
				"DataWidth",
				// 説明
				"Description",
				// デバイス
				"DeviceID",
				// 外部クロックの周波数
				"ExtClock",
				// プロセッサ ファミリ
				"Family",
				"L2CacheSize",
				"L2CacheSpeed",
				"L3CacheSize",
				"L3CacheSpeed",
				"Level",
				"LoadPercentage",
				"Manufacturer",
				"MaxClockSpeed",
				"Name",
				"NumberOfCores",
				"NumberOfLogicalProcessors",
				"OtherFamilyDescription",
				"PNPDeviceID",
				"PowerManagementCapabilities",
				"PowerManagementSupported",
				"ProcessorId",
				"ProcessorType",
				"Revision",
				"Role",
				"SecondLevelAddressTranslationExtensions",
				"SocketDesignation",
				"Status",
				"StatusInfo",
				"Stepping",
				"SystemCreationClassName",
				"SystemName",
				"UniqueId",
				"UpgradeMethod",
				"Version",
				"VirtualizationFirmwareEnabled",
				"VMMonitorModeExtensions",
				"VoltageCaps",
			};
			
			var result = GetInfo(this._managementCPU, "CPU", keys);
			return result;
		}
		
		/// <summary>
		/// メモリ情報取得
		/// </summary>
		/// <returns></returns>
		protected virtual InformationGroup GetMemory()
		{
			var keys = new [] {
				// 物理メモリ(合計:KB)
				"TotalVisibleMemorySize",
				// 物理メモリ(空き)
				"FreePhysicalMemory",
				// 仮想メモリ(合計)
				"TotalVirtualMemorySize",
				// 仮想メモリ(空き)
				"FreeVirtualMemory",
			};
			var result = GetInfo(this._managementOS, "memory", keys);
			return result;
		}
		
		public virtual InformationGroup GetEnvironment()
		{
			var result = new InformationGroup("Environment");
			
			result.Items["CommandLine"] = Environment.CommandLine;
			result.Items["CurrentDirectory"] = Environment.CurrentDirectory;
			result.Items["CurrentManagedThreadId"] = Environment.CurrentManagedThreadId;
			result.Items["ExitCode"] = Environment.ExitCode;
			result.Items["HasShutdownStarted"] = Environment.HasShutdownStarted;
			result.Items["Is64BitOperatingSystem"] = Environment.Is64BitOperatingSystem;
			result.Items["Is64BitProcess"] = Environment.Is64BitProcess;
			result.Items["MachineName"] = Environment.MachineName;
			result.Items["NewLine"] =  BitConverter.ToString(Encoding.UTF8.GetBytes(Environment.NewLine));
			result.Items["OSVersion"] = Environment.OSVersion;
			result.Items["ProcessorCount"] = Environment.ProcessorCount;
			//result.Items["StackTrace"] = Environment.StackTrace;
			result.Items["SystemDirectory"] = Environment.SystemDirectory;
			result.Items["SystemPageSize"] = Environment.SystemPageSize;
			result.Items["TickCount"] = Environment.TickCount;
			result.Items["UserDomainName"] = Environment.UserDomainName;
			result.Items["UserInteractive"] = Environment.UserInteractive;
			result.Items["UserName"] = Environment.UserName;
			result.Items["Version"] = Environment.Version;
			result.Items["WorkingSet"] = Environment.WorkingSet;
			
			foreach(DictionaryEntry pair in Environment.GetEnvironmentVariables()) {
				result.Items[(string)pair.Key] = pair.Value;
			}
			
			return result;
		}
		
		public virtual InformationGroup GetApplication()
		{
			var result = new InformationGroup("Application");
			
			var versionInfo = GetVersionInfo;
			
			// バージョン番号
			result.Items["FileVersion"] = versionInfo.FileVersion;
			// メジャーバージョン番号
			result.Items["FileMajorPart"] = versionInfo.FileMajorPart;
			// マイナバージョン番号
			result.Items["FileMinorPart"] = versionInfo.FileMinorPart;
			// プライベートパート番号
			result.Items["FilePrivatePart"] = versionInfo.FilePrivatePart;
			// ビルド番号
			result.Items["FileBuildPart"] = versionInfo.FileBuildPart;
			// プライベートバージョン
			result.Items["PrivateBuild"] = versionInfo.PrivateBuild;
			// スペシャルビルド
			result.Items["SpecialBuild"] = versionInfo.SpecialBuild;

			// 説明
			result.Items["FileDescription"] = versionInfo.FileDescription;
			// 著作権
			result.Items["LegalCopyright"] = versionInfo.LegalCopyright;
			// 会社名
			result.Items["CompanyName"] = versionInfo.CompanyName;
			// コメント
			result.Items["Comments"] = versionInfo.Comments;
			// 内部名
			result.Items["InternalName"] = versionInfo.InternalName;
			// 言語
			result.Items["Language"] = versionInfo.Language;
			// 商標
			result.Items["LegalTrademarks"] = versionInfo.LegalTrademarks;
			// オリジナルファイル名
			result.Items["OriginalFilename"] = versionInfo.OriginalFilename;

			// 製品名
			result.Items["ProductName"] = versionInfo.ProductName;
			// 製品バージョン
			result.Items["ProductVersion"] = versionInfo.ProductVersion;
			// 製品メジャーバージョン番号
			result.Items["ProductMajorPart"] = versionInfo.ProductMajorPart;
			// 製品マイナバージョン番号
			result.Items["ProductMinorPart"] = versionInfo.ProductMinorPart;
			// 製品プライベートバージョン番号
			result.Items["ProductPrivatePart"] = versionInfo.ProductPrivatePart;
			// 製品ビルド番号
			result.Items["ProductBuildPart"] = versionInfo.ProductBuildPart;

			// デバッグ情報があるか
			result.Items["IsDebug"] = versionInfo.IsDebug;
			// パッチされているか
			result.Items["IsPatched"] = versionInfo.IsPatched;
			// プレリリースか
			result.Items["IsPreRelease"] = versionInfo.IsPreRelease;
			// スペシャルビルドか
			result.Items["IsSpecialBuild"] = versionInfo.IsSpecialBuild;

			return result;
		}
		

		public virtual InformationGroup GetScreen()
		{
			var result = new InformationGroup("Screen");
			var screens = Screen.AllScreens;
			for(var i = 0; i < screens.Length; i++) {
				var screen = screens[i];
				var head = string.Format("screen[{0}].", i);
				result.Items.Add(head + "BitsPerPixel", screen.BitsPerPixel);
				result.Items.Add(head + "Bounds", screen.Bounds);
				result.Items.Add(head + "DeviceName", screen.DeviceName);
				result.Items.Add(head + "Primary", screen.Primary);
				result.Items.Add(head + "WorkingArea", screen.WorkingArea);
			}
			
			return result;
		}

		public virtual IEnumerable<InformationGroup> Get()
		{
			return new [] {
				GetApplication(),
				GetCPU(),
				GetMemory(),
				GetEnvironment(),
				GetScreen(),
			};
		}
		
		
		public override string ToString()
		{
			var list = new List<string>();
			
			foreach(var infoGroup in Get()) {
				list.Add(infoGroup.ToString());
			}
			
			return string.Join(Environment.NewLine, list);
		}
	}
}