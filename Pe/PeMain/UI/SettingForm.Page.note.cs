﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/07/29
 * 時刻: 1:46
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_note.
	/// </summary>
	public partial class SettingForm
	{
		void ToolbarExportSetting(NoteSetting noteSetting)
		{
			noteSetting.CreateHotKey = this.inputNoteCreate.HotKeySetting;
			noteSetting.HiddenHotKey = this.inputNoteHidden.HotKeySetting;
			noteSetting.CompactHotKey= this.inputNoteCompact.HotKeySetting;
		}
	}
}