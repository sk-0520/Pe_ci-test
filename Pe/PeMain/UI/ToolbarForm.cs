﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:15
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm.
	/// </summary>
	public partial class ToolbarForm : AppbarForm
	{
		public ToolbarForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void ToolbarForm_MenuItem_Click(object sender, EventArgs e)
		{
			var menuItem = (ToolStripItem)sender;
			var group = (ToolbarGroupItem)menuItem.Tag;
			SelectedGroup(group);
		}
		
		void button_ButtonClick(object sender, EventArgs e)
		{
			var toolItem = (ToolStripItem)sender;
			var launcherItem = (LauncherItem)toolItem.Tag;
			ExecuteItem(launcherItem);
		}
		
		void ToolbarForm_Paint(object sender, PaintEventArgs e)
		{
			DrawFull(e.Graphics, ClientRectangle, Form.ActiveForm == this);
		}
		
		void ToolbarForm_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				// タイトルバーっぽければ移動させとく
				if(ToolbarSetting.ToolbarPosition == ToolbarPosition.DesktopFloat) {
					var captionArea = GetCaptionArea(ToolbarSetting.ToolbarPosition);
					if(captionArea.Contains(e.Location)) {
						API.ReleaseCapture();
						API.SendMessage(Handle, WM.WM_NCLBUTTONDOWN, (IntPtr)HT.HT_CAPTION, IntPtr.Zero);
					}
				}
			}
		}
	}
}
