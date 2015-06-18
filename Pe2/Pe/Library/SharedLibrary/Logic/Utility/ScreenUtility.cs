﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Management;
	using System.Runtime.InteropServices;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.PInvoke.Windows.root.CIMV2;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// スクリーン共通処理。
	/// </summary>
	public static class ScreenUtility
	{
		private static string DeviceToId(string deviceName)
		{
			return new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
		}

		private static IEnumerable<Win32_DesktopMonitor> GetScreens(string deviceName, ILogger logger = null)
		{
			string query = "SELECT * FROM Win32_DesktopMonitor";
			if (!string.IsNullOrWhiteSpace(deviceName)) {
				//var id = new string(deviceName.Trim().SkipWhile(c => !char.IsNumber(c)).ToArray());
				var id = DeviceToId(deviceName);
				query = string.Format("SELECT * FROM Win32_DesktopMonitor where DeviceID like \"DesktopMonitor{0}\"", id);
			}
			using (var searcher = new ManagementObjectSearcher(query)) {
				foreach (ManagementBaseObject mng in searcher.Get()) {
					var item = new Win32_DesktopMonitor();
					try {
						item.Import(mng);
					} catch (Exception ex) {
						logger.SafeWarning(ex);
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
		/// <param name = "logger"></param>
		/// <returns></returns>
		public static string GetScreenName(ScreenModel screen, ILogger logger = null)
		{
			foreach (var screem in GetScreens(screen.DeviceName, logger)) {
				if (!string.IsNullOrWhiteSpace(screem.Name)) {
					var id = DeviceToId(screen.DeviceName);
					return string.Format("{0}. {1}", id, screem.Name);
				}
				break;
			}

			var device = new DISPLAY_DEVICE();
			device.cb = Marshal.SizeOf(device);
			NativeMethods.EnumDisplayDevices(screen.DeviceName, 0, ref device, 1);

			//return screen.DeviceName;
			return device.DeviceString;
		}

		public static string GetScreenName(string screenDeviceName, ILogger logger = null)
		{
			var screen = Screen.AllScreens.SingleOrDefault(s => s.DeviceName == screenDeviceName);
			if (screen != null) {
				return GetScreenName(screen, logger);
			}
			return screenDeviceName;
		}

		//public static Screen GetCurrentCursor()
		//{
		//	return Screen.FromPoint(Cursor.Position);
		//}
	}
}
