﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/02
 * 時刻: 17:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.IF;
using PInvoke.Windows;
using PInvoke.Windows.root.CIMV2;

namespace PeMain.Logic
{
	/// <summary>
	/// スクリーン共通処理。
	/// </summary>
	public static class ScreenUtility
	{
		private static string NeviceToId(string deviceName)
		{
			return new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
		}
		
		private static IEnumerable<Win32_DesktopMonitor> GetScreens(string deviceName, ILogger logger)
		{
			string query = "SELECT * FROM Win32_DesktopMonitor";
			if(!string.IsNullOrWhiteSpace(deviceName)) {
				//var id = new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
				var id = NeviceToId(deviceName);
				query = string.Format("SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
			}
			using(var searcher = new ManagementObjectSearcher(query)) {
				foreach(ManagementObject mng in searcher.Get()) {
					var item = new Win32_DesktopMonitor();
					try {
						item.Import(mng);
					} catch(Exception ex) {
						if(logger != null) {
							logger.Puts(LogType.Warning, ex.Message, ex);
						} else {
							Debug.WriteLine(ex);
						}
						continue;
					}
					
					yield return item;
				}
			}
		}

		/// <summary>
		/// スクリーンの名前を取得。
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static string GetScreenName(Screen screen, ILogger logger)
		{
			/*
			var id = new string(screen.DeviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
			var query = string.Format("SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
			using(var searcher = new ManagementObjectSearcher(query)) {
				foreach(ManagementObject mng in searcher.Get()) {
					try {
						var item = new Win32_DesktopMonitor();
						item.Import(mng);
						if(!string.IsNullOrWhiteSpace(item.Name)) {
							return string.Format("{0}. {1}", id, item.Name);
						}
					} catch(Exception ex) {
						if(logger != null) {
							logger.Puts(LogType.Warning, ex.Message, ex);
						} else {
							Debug.WriteLine(ex);
						}
					}
					break;
				}
			}
			 */
			foreach(var screem in GetScreens(screen.DeviceName, logger)) {
				if(!string.IsNullOrWhiteSpace(screem.Name)) {
					var id = NeviceToId(screen.DeviceName);
					return string.Format("{0}. {1}", id, screem.Name);
				}
				break;
			}
			
			var device = new DISPLAY_DEVICE();
			device.cb = Marshal.SizeOf(device);
			API.EnumDisplayDevices(screen.DeviceName, 0, ref device, 1);
			
			//return screen.DeviceName;
			return device.DeviceString;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static string GetScreenName(Screen screen)
		{
			return GetScreenName(screen, null);
		}
		
		public static string GetScreenName(string screenDeviceName, ILogger logger)
		{
			var screen = Screen.AllScreens.SingleOrDefault(s => s.DeviceName == screenDeviceName);
			if(screen != null) {
				return GetScreenName(screen, logger);
			}
			return screenDeviceName;
		}
		
		public static Screen GetCurrentCursor()
		{
			return Screen.FromPoint(Cursor.Position);
		}
	}
}
