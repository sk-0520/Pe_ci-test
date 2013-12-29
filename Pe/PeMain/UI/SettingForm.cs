﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 21:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm.
	/// </summary>
	public partial class SettingForm : Form
	{
		public SettingForm(Language language, MainSetting setting)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize(language, setting);
		}
		
		
		void SelecterLauncher_SelectChnagedItem(object sender, SelectedItemEventArg e)
		{
			if(this._launcherSelectedItem != null) {
				// 現在アイテムに入力内容を退避
				LauncherInputValueToItem(this._launcherSelectedItem);
			}
			if(e.Item == null) {
				// 未選択状態
				LauncherInputClear();
				this.splitContainer1.Panel2.Enabled = false; // NOTE: 暫定対応
				return;
			}
			if(e.Item == this._launcherSelectedItem) {
				// 現在選択中アイテム
				return;
			}
			this.splitContainer1.Panel2.Enabled = true; // NOTE: 暫定対応
			LauncherSelectItem(e.Item);
		}
		
		void SelecterLauncher_CreateItem(object sender, CreateItemEventArg e)
		{
			if(this._launcherSelectedItem != null) {
				// 現在アイテムに入力内容を退避
				LauncherInputValueToItem(this._launcherSelectedItem);
			}
			LauncherSelectItem(e.Item);
		}
		
		void TabSetting_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if(this._nowSelectedTabPage == this.pageLauncher) {
				e.Cancel = LauncherItemValid();
			}
			if(!e.Cancel) {
				if(e.TabPage == this.pageToolbar) {
					this.selecterToolbar.SetItems(this.selecterLauncher.Items);
					this.selecterToolbar.Refresh();
				}
				this._nowSelectedTabPage =  e.TabPage;
			}
		}
		
		void CommandLauncherFilePath_Click(object sender, EventArgs e)
		{
			OpenDialogFilePath(this.inputLauncherCommand);
		}
		
		void CommandLauncherDirPath_Click(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputLauncherCommand);
		}
		
		void CommandLauncherWorkDirPath_Click(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputLauncherWorkDirPath);
		}
		
		void CommandLauncherIconPath_Click(object sender, EventArgs e)
		{
			LauncherOpenIcon();
		}
		
		void CommandLauncherOptionFilePath_Click(object sender, EventArgs e)
		{
			OpenDialogFilePath(this.inputLauncherOption);
		}
		
		void CommandLauncherOptionDirPath_Click(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputLauncherOption);
		}
		
		void CommandMainNoteDirPathClick(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputMainNoteDirPath);
		}
		
		void CommandCommandFont_Click(object sender, EventArgs e)
		{
			var fontSetting = OpenDialogFontSetting(this.commandCommandFont, this._commandFont);
			if(fontSetting != null) {
				this._commandFont = fontSetting;
				SetViewMessage(this.commandCommandFont, this._commandFont);
			}
		}
		
		void CommandToolbarFont_Click(object sender, EventArgs e)
		{
			var fontSetting = OpenDialogFontSetting(this.commandToolbarFont, this._toolbarFont);
			if(fontSetting != null) {
				this._toolbarFont = fontSetting;
				SetViewMessage(this.commandToolbarFont, this._toolbarFont);
			}
		}
	}
}