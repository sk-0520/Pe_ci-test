﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 22:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class EnvUpdateControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.gridEnv = new System.Windows.Forms.DataGridView();
			this.headerKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.headerValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridEnv)).BeginInit();
			this.SuspendLayout();
			// 
			// gridEnv
			// 
			this.gridEnv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridEnv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.headerKey,
									this.headerValue});
			this.gridEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridEnv.Location = new System.Drawing.Point(0, 0);
			this.gridEnv.Name = "gridEnv";
			this.gridEnv.RowTemplate.Height = 21;
			this.gridEnv.Size = new System.Drawing.Size(150, 150);
			this.gridEnv.TabIndex = 0;
			// 
			// headerKey
			// 
			this.headerKey.HeaderText = "{KEY}";
			this.headerKey.Name = "headerKey";
			// 
			// headerValue
			// 
			this.headerValue.HeaderText = "{VALUE}";
			this.headerValue.Name = "headerValue";
			// 
			// EnvUpdateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridEnv);
			this.Name = "EnvUpdateControl";
			((System.ComponentModel.ISupportInitialize)(this.gridEnv)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.DataGridViewTextBoxColumn headerValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn headerKey;
		private System.Windows.Forms.DataGridView gridEnv;
	}
}