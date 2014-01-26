﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/26
 * 時刻: 20:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class MessageWindow
	{
		public void SetSettingData(MainSetting mainSetting)
		{
			MainSetting = mainSetting;
			
			ApplySetting();
		}
		
		void ApplySetting()
		{
			Debug.Assert(MainSetting != null);
			
			ApplyLanguage();
		}
	}
}
