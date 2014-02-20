﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/11
 * 時刻: 23:54
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.Data
{
	/// <summary>
	/// Description of LogSetting.
	/// </summary>
	[Serializable]
	public class LogSetting: Item
	{
		public LogSetting()
		{
			Size = new Size(
				Screen.PrimaryScreen.Bounds.Width / 4,
				Screen.PrimaryScreen.Bounds.Height / 2
			);
			var screenSize = Screen.PrimaryScreen.WorkingArea.Size;
			Point = new Point(screenSize.Width - Size.Width, screenSize.Height - Size.Height);
			AddShow = true;
			AddShowTrigger = LogType.Warning | LogType.Error;
		}
		
		public bool Visible { get; set; }
		public Point Point { get; set; }
		public Size Size { get; set; }
		/// <summary>
		/// ログ追加時に画面表示
		/// </summary>
		public bool AddShow { get; set; }
		public LogType AddShowTrigger { get; set; }
	}
}
