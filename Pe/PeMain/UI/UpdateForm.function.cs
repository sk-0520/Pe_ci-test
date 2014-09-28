﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	partial class UpdateForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplyLanguage();
			ApplySetting();
		}
		
		void ApplySetting()
		{
			if(UpdateInfo.IsRcVersion) {
				this.labelVersion.BorderStyle = BorderStyle.FixedSingle;
				this.labelVersion.ForeColor = Color.Red;
				this.labelVersion.BackColor = Color.Black;
			}
		}
	}
}