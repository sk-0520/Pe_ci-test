﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/21
 * 時刻: 20:08
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of AcceptForm_language.
	/// </summary>
	partial class AcceptForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			var acceptFilePath = Path.Combine(Literal.PeLanguageDirPath, CommonData.Language.AcceptFileName);
			//this.webDocument.DocumentText = File.ReadAllText(acceptFilePath);;
			this.webDocument.Navigate(acceptFilePath);
		}
	}
}