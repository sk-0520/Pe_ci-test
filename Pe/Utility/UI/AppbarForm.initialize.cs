﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// </summary>
	public partial class AppbarForm
	{
		void Initialize()
		{
			AutoHide = false;
			BarSize = Size;
			DockScreen = Screen.PrimaryScreen;
			DesktopDockType = DesktopDockType.None;
			IsDocking = false;
			MessageString = "AppDesktopToolbar";
			HiddenSize = new Padding(SystemInformation.SizingBorderWidth);
			HiddenWaitTime = TimeSpan.FromSeconds(3);
			HiddenAnimateTime = TimeSpan.FromMilliseconds(500);
			
		}
	}
}