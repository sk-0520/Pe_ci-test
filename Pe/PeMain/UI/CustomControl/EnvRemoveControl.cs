﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 22:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// Description of EnvRemoveControl.
	/// </summary>
	public partial class EnvRemoveControl : UserControl
	{
		public EnvRemoveControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void InputEnv_TextChanged(object sender, EventArgs e)
		{
			if(this._event && ValueChanged != null) {
				ValueChanged(this, new EventArgs());
			}
		}
	}
}
