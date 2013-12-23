﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 12/16/2013
 * 時刻: 22:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_initialize.
	/// </summary>
	public partial class SettingForm
	{
		void InitializeMainSetting(MainSetting mainSetting)
		{
			
		}
		
		void InitializeLauncher(LauncherSetting launcherSetting)
		{
			foreach(var item in launcherSetting.Items) {
				this._launcherItems.Add((LauncherItem)item.Clone());
			}
		}
		
		void InitializeCommand(CommandSetting commandSetting)
		{
		}
		
		void InitializeToolbar(ToolbarSetting toolbarSetting)
		{
		}
		
		void InitializeUI(MainSetting mainSetting)
		{
			ApplyLanguage();
			
			InitializeMainSetting(mainSetting);
			InitializeLauncher(mainSetting.Launcher);
			InitializeCommand(mainSetting.Command);
			InitializeToolbar(mainSetting.Toolbar);
		}
		
		void Initialize(Language language, MainSetting mainSetting)
		{
			this._launcherItems = new SortedSet<LauncherItem>();
			
			Language = language;
			
			InitializeUI(mainSetting);
		}
	}
}
