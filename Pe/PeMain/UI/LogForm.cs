﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/10/2014
 * 時刻: 23:49
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm.
	/// </summary>
	public partial class LogForm : Form, ILogger
	{
		public LogForm(IEnumerable<LogItem> initLog)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Initialize(initLog);
		}
		
		
		void ListLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			if (e.ItemIndex < this._logs.Count) {
				if(e.Item == null) {
					e.Item = new ListViewItem();
					e.Item.SubItems.Add(new ListViewItem.ListViewSubItem());
				}
				
				var logItem = this._logs[e.ItemIndex];
				e.Item.ImageIndex = (int)logItem.LogType;
				
				var dateItem = e.Item;
				var titleItem = e.Item.SubItems[1];
				
				dateItem.Text = logItem.DateTime.ToString();
				//dateItem.ImageKey = logItem.LogType.ToString();
				titleItem.Text = logItem.Title;
			}
		}
		
		void LogForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}
		
		void ListLog_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearDetail();
			if(this.listLog.FocusedItem != null) {
				var listItem = this.listLog.FocusedItem;
				var logItem = this._logs[listItem.Index];
				SetDetail(logItem);
			}
		}
	}
}