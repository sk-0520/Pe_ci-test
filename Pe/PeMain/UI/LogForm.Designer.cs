﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/10/2014
 * 時刻: 23:49
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class LogForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.statusLog = new System.Windows.Forms.StatusStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listLog = new System.Windows.Forms.ListView();
			this.headerTimestamp = new System.Windows.Forms.ColumnHeader();
			this.headerTitle = new System.Windows.Forms.ColumnHeader();
			this.panelDetail = new System.Windows.Forms.TableLayoutPanel();
			this.treeDetail = new System.Windows.Forms.TreeView();
			this.listStack = new System.Windows.Forms.ListView();
			this.headerFile = new System.Windows.Forms.ColumnHeader();
			this.headerLine = new System.Windows.Forms.ColumnHeader();
			this.headerFunction = new System.Windows.Forms.ColumnHeader();
			this.toolLog = new System.Windows.Forms.ToolStrip();
			this.toolLog_save = new System.Windows.Forms.ToolStripButton();
			this.toolLog_clear = new System.Windows.Forms.ToolStripButton();
			this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panelDetail.SuspendLayout();
			this.toolLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.BottomToolStripPanel
			// 
			this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusLog);
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
			this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(436, 229);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(436, 276);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolLog);
			// 
			// statusLog
			// 
			this.statusLog.Dock = System.Windows.Forms.DockStyle.None;
			this.statusLog.Location = new System.Drawing.Point(0, 0);
			this.statusLog.Name = "statusLog";
			this.statusLog.Size = new System.Drawing.Size(436, 22);
			this.statusLog.TabIndex = 0;
			this.statusLog.Text = "statusStrip1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listLog);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panelDetail);
			this.splitContainer1.Size = new System.Drawing.Size(436, 229);
			this.splitContainer1.SplitterDistance = 135;
			this.splitContainer1.TabIndex = 0;
			// 
			// listLog
			// 
			this.listLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.headerTimestamp,
									this.headerTitle});
			this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listLog.FullRowSelect = true;
			this.listLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listLog.Location = new System.Drawing.Point(0, 0);
			this.listLog.MultiSelect = false;
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(436, 135);
			this.listLog.TabIndex = 0;
			this.listLog.UseCompatibleStateImageBehavior = false;
			this.listLog.View = System.Windows.Forms.View.Details;
			this.listLog.VirtualMode = true;
			this.listLog.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListLog_RetrieveVirtualItem);
			this.listLog.SelectedIndexChanged += new System.EventHandler(this.ListLog_SelectedIndexChanged);
			// 
			// headerTimestamp
			// 
			this.headerTimestamp.Text = "{TIME}";
			// 
			// headerTitle
			// 
			this.headerTitle.Text = "{TITLE}";
			// 
			// panelDetail
			// 
			this.panelDetail.ColumnCount = 2;
			this.panelDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelDetail.Controls.Add(this.treeDetail, 0, 0);
			this.panelDetail.Controls.Add(this.listStack, 1, 0);
			this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDetail.Location = new System.Drawing.Point(0, 0);
			this.panelDetail.Name = "panelDetail";
			this.panelDetail.RowCount = 1;
			this.panelDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelDetail.Size = new System.Drawing.Size(436, 90);
			this.panelDetail.TabIndex = 1;
			// 
			// treeDetail
			// 
			this.treeDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeDetail.Location = new System.Drawing.Point(3, 3);
			this.treeDetail.Name = "treeDetail";
			this.treeDetail.Size = new System.Drawing.Size(212, 84);
			this.treeDetail.TabIndex = 0;
			// 
			// listStack
			// 
			this.listStack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.headerFile,
									this.headerLine,
									this.headerFunction});
			this.listStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listStack.FullRowSelect = true;
			this.listStack.GridLines = true;
			this.listStack.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listStack.Location = new System.Drawing.Point(221, 3);
			this.listStack.MultiSelect = false;
			this.listStack.Name = "listStack";
			this.listStack.ShowGroups = false;
			this.listStack.Size = new System.Drawing.Size(212, 84);
			this.listStack.TabIndex = 0;
			this.listStack.UseCompatibleStateImageBehavior = false;
			this.listStack.View = System.Windows.Forms.View.Details;
			// 
			// headerFile
			// 
			this.headerFile.Text = "{FILE}";
			// 
			// headerLine
			// 
			this.headerLine.Text = "{LINE}";
			// 
			// headerFunction
			// 
			this.headerFunction.Text = "{FUNC}";
			// 
			// toolLog
			// 
			this.toolLog.Dock = System.Windows.Forms.DockStyle.None;
			this.toolLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolLog_save,
									this.toolLog_clear});
			this.toolLog.Location = new System.Drawing.Point(0, 0);
			this.toolLog.Name = "toolLog";
			this.toolLog.Size = new System.Drawing.Size(436, 25);
			this.toolLog.Stretch = true;
			this.toolLog.TabIndex = 0;
			// 
			// toolLog_save
			// 
			this.toolLog_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLog_save.Image = global::PeMain.Properties.Images.Save;
			this.toolLog_save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLog_save.Name = "toolLog_save";
			this.toolLog_save.Size = new System.Drawing.Size(23, 22);
			this.toolLog_save.Text = "{SAVE}";
			this.toolLog_save.ToolTipText = "toolLog_save";
			// 
			// toolLog_clear
			// 
			this.toolLog_clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLog_clear.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLog_clear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLog_clear.Name = "toolLog_clear";
			this.toolLog_clear.Size = new System.Drawing.Size(23, 22);
			this.toolLog_clear.Text = "{CLEAR}";
			this.toolLog_clear.ToolTipText = "toolLog_clear";
			// 
			// LogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 276);
			this.Controls.Add(this.toolStripContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LogForm";
			this.ShowInTaskbar = false;
			this.Text = "LogForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
			this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.panelDetail.ResumeLayout(false);
			this.toolLog.ResumeLayout(false);
			this.toolLog.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ColumnHeader headerFunction;
		private System.Windows.Forms.ColumnHeader headerLine;
		private System.Windows.Forms.ColumnHeader headerFile;
		private System.Windows.Forms.ListView listStack;
		private System.Windows.Forms.TreeView treeDetail;
		private System.Windows.Forms.TableLayoutPanel panelDetail;
		private System.Windows.Forms.ColumnHeader headerTitle;
		private System.Windows.Forms.ColumnHeader headerTimestamp;
		private System.Windows.Forms.ListView listLog;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripButton toolLog_clear;
		private System.Windows.Forms.ToolStripButton toolLog_save;
		private System.Windows.Forms.ToolStrip toolLog;
		private System.Windows.Forms.StatusStrip statusLog;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
	}
}