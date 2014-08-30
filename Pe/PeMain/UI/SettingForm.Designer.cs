﻿using PInvoke.Windows;
/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 21:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class SettingForm
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tabSetting = new System.Windows.Forms.TabControl();
			this.tabSetting_pageMain = new System.Windows.Forms.TabPage();
			this.groupMainSystemEnv = new System.Windows.Forms.GroupBox();
			this.labelSystemEnvExt = new System.Windows.Forms.Label();
			this.inputSystemEnvExt = new PeMain.UI.PeHotkeyControl();
			this.labelSystemEnvHiddenFile = new System.Windows.Forms.Label();
			this.inputSystemEnvHiddenFile = new PeMain.UI.PeHotkeyControl();
			this.groupMainLog = new System.Windows.Forms.GroupBox();
			this.selectLogAddShow = new System.Windows.Forms.CheckBox();
			this.groupLogTrigger = new System.Windows.Forms.GroupBox();
			this.selectLogTrigger_error = new System.Windows.Forms.CheckBox();
			this.selectLogTrigger_warning = new System.Windows.Forms.CheckBox();
			this.selectLogTrigger_information = new System.Windows.Forms.CheckBox();
			this.selectLogVisible = new System.Windows.Forms.CheckBox();
			this.selectMainLanguage = new System.Windows.Forms.ComboBox();
			this.labelMainLanguage = new System.Windows.Forms.Label();
			this.tabSetting_pageLauncher = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.selecterLauncher = new PeMain.UI.LauncherItemSelectControl();
			this.tabLauncher = new System.Windows.Forms.TabControl();
			this.tabLauncher_pageCommon = new System.Windows.Forms.TabPage();
			this.groupLauncherType = new System.Windows.Forms.GroupBox();
			this.selectLauncherType_uri = new System.Windows.Forms.RadioButton();
			this.selectLauncherType_file = new System.Windows.Forms.RadioButton();
			this.labelLauncherName = new System.Windows.Forms.Label();
			this.labelLauncherOption = new System.Windows.Forms.Label();
			this.inputLauncherName = new System.Windows.Forms.TextBox();
			this.inputLauncherOption = new System.Windows.Forms.TextBox();
			this.labelLauncherCommand = new System.Windows.Forms.Label();
			this.commandLauncherOptionDirPath = new System.Windows.Forms.Button();
			this.commandLauncherFilePath = new System.Windows.Forms.Button();
			this.commandLauncherOptionFilePath = new System.Windows.Forms.Button();
			this.commandLauncherDirPath = new System.Windows.Forms.Button();
			this.commandLauncherWorkDirPath = new System.Windows.Forms.Button();
			this.labelLauncherIconPath = new System.Windows.Forms.Label();
			this.commandLauncherIconPath = new System.Windows.Forms.Button();
			this.labelLauncherWorkDirPath = new System.Windows.Forms.Label();
			this.inputLauncherCommand = new System.Windows.Forms.TextBox();
			this.inputLauncherWorkDirPath = new System.Windows.Forms.TextBox();
			this.inputLauncherIconPath = new System.Windows.Forms.TextBox();
			this.tabLauncher_pageEnv = new System.Windows.Forms.TabPage();
			this.panelEnv = new System.Windows.Forms.SplitContainer();
			this.envLauncherUpdate = new PeMain.UI.EnvUpdateControl();
			this.envLauncherRemove = new PeMain.UI.EnvRemoveControl();
			this.selectLauncherEnv = new System.Windows.Forms.CheckBox();
			this.tabLauncher_pageOthers = new System.Windows.Forms.TabPage();
			this.selectLauncherAdmin = new System.Windows.Forms.CheckBox();
			this.labelLauncherTag = new System.Windows.Forms.Label();
			this.selectLauncherStdStream = new System.Windows.Forms.CheckBox();
			this.inputLauncherTag = new System.Windows.Forms.TextBox();
			this.inputLauncherNote = new System.Windows.Forms.TextBox();
			this.labelLauncherNote = new System.Windows.Forms.Label();
			this.tabSetting_pageToolbar = new System.Windows.Forms.TabPage();
			this.groupToolbar = new System.Windows.Forms.GroupBox();
			this.selectToolbarItem = new System.Windows.Forms.ComboBox();
			this.labelToolbarFont = new System.Windows.Forms.Label();
			this.inputToolbarTextWidth = new System.Windows.Forms.NumericUpDown();
			this.commandToolbarFont = new PeMain.UI.FontSplitButton();
			this.selectToolbarShowText = new System.Windows.Forms.CheckBox();
			this.selectToolbarTopmost = new System.Windows.Forms.CheckBox();
			this.selectToolbarAutoHide = new System.Windows.Forms.CheckBox();
			this.labelToolbarTextWidth = new System.Windows.Forms.Label();
			this.selectToolbarVisible = new System.Windows.Forms.CheckBox();
			this.labelToolbarIcon = new System.Windows.Forms.Label();
			this.selectToolbarPosition = new System.Windows.Forms.ComboBox();
			this.selectToolbarIcon = new System.Windows.Forms.ComboBox();
			this.labelToolbarPosition = new System.Windows.Forms.Label();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.treeToolbarItemGroup = new System.Windows.Forms.TreeView();
			this.toolToolbarGroup = new System.Windows.Forms.ToolStrip();
			this.toolToolbarGroup_addGroup = new System.Windows.Forms.ToolStripButton();
			this.toolToolbarGroup_addItem = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolToolbarGroup_up = new System.Windows.Forms.ToolStripButton();
			this.toolToolbarGroup_down = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolToolbarGroup_remove = new System.Windows.Forms.ToolStripButton();
			this.selecterToolbar = new PeMain.UI.LauncherItemSelectControl();
			this.tabSetting_pageCommand = new System.Windows.Forms.TabPage();
			this.labelCommandHotkey = new System.Windows.Forms.Label();
			this.inputCommandHotkey = new PeMain.UI.PeHotkeyControl();
			this.labelCommandIcon = new System.Windows.Forms.Label();
			this.selectCommandIcon = new System.Windows.Forms.ComboBox();
			this.inputCommandHideTime = new System.Windows.Forms.NumericUpDown();
			this.selectCommandTopmost = new System.Windows.Forms.CheckBox();
			this.labelCommandHideTime = new System.Windows.Forms.Label();
			this.commandCommandFont = new PeMain.UI.FontSplitButton();
			this.labelCommandFont = new System.Windows.Forms.Label();
			this.tabSetting_pageNote = new System.Windows.Forms.TabPage();
			this.groupNoteItem = new System.Windows.Forms.GroupBox();
			this.gridNoteItems = new System.Windows.Forms.DataGridView();
			this.gridNoteItems_columnRemove = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridNoteItems_columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridNoteItems_columnVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridNoteItems_columnLocked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.gridNoteItems_columnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridNoteItems_columnBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridNoteItems_columnFont = new System.Windows.Forms.DataGridViewButtonColumn();
			this.gridNoteItems_columnFore = new System.Windows.Forms.DataGridViewButtonColumn();
			this.gridNoteItems_columnBack = new System.Windows.Forms.DataGridViewButtonColumn();
			this.groupNoteKey = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.labelNoteCreate = new System.Windows.Forms.Label();
			this.inputNoteCreate = new PeMain.UI.PeHotkeyControl();
			this.labelNoteHiddent = new System.Windows.Forms.Label();
			this.inputNoteHidden = new PeMain.UI.PeHotkeyControl();
			this.labelNoteCompact = new System.Windows.Forms.Label();
			this.inputNoteCompact = new PeMain.UI.PeHotkeyControl();
			this.commandNoteCaptionFont = new PeMain.UI.FontSplitButton();
			this.labelNoteCaptionFont = new System.Windows.Forms.Label();
			this.tabSetting_pageDisplay = new System.Windows.Forms.TabPage();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.tabSetting.SuspendLayout();
			this.tabSetting_pageMain.SuspendLayout();
			this.groupMainSystemEnv.SuspendLayout();
			this.groupMainLog.SuspendLayout();
			this.groupLogTrigger.SuspendLayout();
			this.tabSetting_pageLauncher.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabLauncher.SuspendLayout();
			this.tabLauncher_pageCommon.SuspendLayout();
			this.groupLauncherType.SuspendLayout();
			this.tabLauncher_pageEnv.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelEnv)).BeginInit();
			this.panelEnv.Panel1.SuspendLayout();
			this.panelEnv.Panel2.SuspendLayout();
			this.panelEnv.SuspendLayout();
			this.tabLauncher_pageOthers.SuspendLayout();
			this.tabSetting_pageToolbar.SuspendLayout();
			this.groupToolbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputToolbarTextWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.toolToolbarGroup.SuspendLayout();
			this.tabSetting_pageCommand.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).BeginInit();
			this.tabSetting_pageNote.SuspendLayout();
			this.groupNoteItem.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridNoteItems)).BeginInit();
			this.groupNoteKey.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabSetting
			// 
			this.tabSetting.Controls.Add(this.tabSetting_pageMain);
			this.tabSetting.Controls.Add(this.tabSetting_pageLauncher);
			this.tabSetting.Controls.Add(this.tabSetting_pageToolbar);
			this.tabSetting.Controls.Add(this.tabSetting_pageCommand);
			this.tabSetting.Controls.Add(this.tabSetting_pageNote);
			this.tabSetting.Controls.Add(this.tabSetting_pageDisplay);
			this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSetting.Location = new System.Drawing.Point(3, 4);
			this.tabSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new System.Drawing.Size(749, 294);
			this.tabSetting.TabIndex = 0;
			this.tabSetting.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabSetting_Selecting);
			// 
			// tabSetting_pageMain
			// 
			this.tabSetting_pageMain.Controls.Add(this.groupMainSystemEnv);
			this.tabSetting_pageMain.Controls.Add(this.groupMainLog);
			this.tabSetting_pageMain.Controls.Add(this.selectMainLanguage);
			this.tabSetting_pageMain.Controls.Add(this.labelMainLanguage);
			this.tabSetting_pageMain.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageMain.Name = "tabSetting_pageMain";
			this.tabSetting_pageMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageMain.Size = new System.Drawing.Size(741, 266);
			this.tabSetting_pageMain.TabIndex = 0;
			this.tabSetting_pageMain.Text = ":setting/tab/main";
			this.tabSetting_pageMain.UseVisualStyleBackColor = true;
			// 
			// groupMainSystemEnv
			// 
			this.groupMainSystemEnv.Controls.Add(this.labelSystemEnvExt);
			this.groupMainSystemEnv.Controls.Add(this.inputSystemEnvExt);
			this.groupMainSystemEnv.Controls.Add(this.labelSystemEnvHiddenFile);
			this.groupMainSystemEnv.Controls.Add(this.inputSystemEnvHiddenFile);
			this.groupMainSystemEnv.Location = new System.Drawing.Point(248, 62);
			this.groupMainSystemEnv.Name = "groupMainSystemEnv";
			this.groupMainSystemEnv.Size = new System.Drawing.Size(379, 100);
			this.groupMainSystemEnv.TabIndex = 18;
			this.groupMainSystemEnv.TabStop = false;
			this.groupMainSystemEnv.Text = ":setting/group/system-env";
			// 
			// labelSystemEnvExt
			// 
			this.labelSystemEnvExt.AutoSize = true;
			this.labelSystemEnvExt.Location = new System.Drawing.Point(6, 53);
			this.labelSystemEnvExt.Name = "labelSystemEnvExt";
			this.labelSystemEnvExt.Size = new System.Drawing.Size(147, 15);
			this.labelSystemEnvExt.TabIndex = 17;
			this.labelSystemEnvExt.Text = ":setting/label/extension";
			// 
			// inputSystemEnvExt
			// 
			this.inputSystemEnvExt.BackColor = System.Drawing.Color.White;
			this.inputSystemEnvExt.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.inputSystemEnvExt.Hotkey = System.Windows.Forms.Keys.None;
			this.inputSystemEnvExt.HotKeySetting = null;
			this.inputSystemEnvExt.Location = new System.Drawing.Point(112, 50);
			this.inputSystemEnvExt.Modifiers = PInvoke.Windows.MOD.None;
			this.inputSystemEnvExt.Name = "inputSystemEnvExt";
			this.inputSystemEnvExt.ReadOnly = true;
			this.inputSystemEnvExt.Registered = false;
			this.inputSystemEnvExt.Size = new System.Drawing.Size(252, 23);
			this.inputSystemEnvExt.TabIndex = 16;
			this.inputSystemEnvExt.Text = "None";
			// 
			// labelSystemEnvHiddenFile
			// 
			this.labelSystemEnvHiddenFile.AutoSize = true;
			this.labelSystemEnvHiddenFile.Location = new System.Drawing.Point(6, 26);
			this.labelSystemEnvHiddenFile.Name = "labelSystemEnvHiddenFile";
			this.labelSystemEnvHiddenFile.Size = new System.Drawing.Size(146, 15);
			this.labelSystemEnvHiddenFile.TabIndex = 17;
			this.labelSystemEnvHiddenFile.Text = ":setting/label/hiddenfile";
			// 
			// inputSystemEnvHiddenFile
			// 
			this.inputSystemEnvHiddenFile.BackColor = System.Drawing.Color.White;
			this.inputSystemEnvHiddenFile.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.inputSystemEnvHiddenFile.Hotkey = System.Windows.Forms.Keys.None;
			this.inputSystemEnvHiddenFile.HotKeySetting = null;
			this.inputSystemEnvHiddenFile.Location = new System.Drawing.Point(112, 20);
			this.inputSystemEnvHiddenFile.Modifiers = PInvoke.Windows.MOD.None;
			this.inputSystemEnvHiddenFile.Name = "inputSystemEnvHiddenFile";
			this.inputSystemEnvHiddenFile.ReadOnly = true;
			this.inputSystemEnvHiddenFile.Registered = false;
			this.inputSystemEnvHiddenFile.Size = new System.Drawing.Size(252, 23);
			this.inputSystemEnvHiddenFile.TabIndex = 16;
			this.inputSystemEnvHiddenFile.Text = "None";
			// 
			// groupMainLog
			// 
			this.groupMainLog.Controls.Add(this.selectLogAddShow);
			this.groupMainLog.Controls.Add(this.groupLogTrigger);
			this.groupMainLog.Controls.Add(this.selectLogVisible);
			this.groupMainLog.Location = new System.Drawing.Point(15, 62);
			this.groupMainLog.Name = "groupMainLog";
			this.groupMainLog.Size = new System.Drawing.Size(200, 163);
			this.groupMainLog.TabIndex = 2;
			this.groupMainLog.TabStop = false;
			this.groupMainLog.Text = ":setting/group/log";
			// 
			// selectLogAddShow
			// 
			this.selectLogAddShow.AutoSize = true;
			this.selectLogAddShow.Location = new System.Drawing.Point(16, 47);
			this.selectLogAddShow.Name = "selectLogAddShow";
			this.selectLogAddShow.Size = new System.Drawing.Size(172, 19);
			this.selectLogAddShow.TabIndex = 1;
			this.selectLogAddShow.Text = ":setting/check/add-show";
			this.selectLogAddShow.UseVisualStyleBackColor = true;
			// 
			// groupLogTrigger
			// 
			this.groupLogTrigger.Controls.Add(this.selectLogTrigger_error);
			this.groupLogTrigger.Controls.Add(this.selectLogTrigger_warning);
			this.groupLogTrigger.Controls.Add(this.selectLogTrigger_information);
			this.groupLogTrigger.Location = new System.Drawing.Point(6, 45);
			this.groupLogTrigger.Name = "groupLogTrigger";
			this.groupLogTrigger.Size = new System.Drawing.Size(188, 106);
			this.groupLogTrigger.TabIndex = 2;
			this.groupLogTrigger.TabStop = false;
			// 
			// selectLogTrigger_error
			// 
			this.selectLogTrigger_error.AutoSize = true;
			this.selectLogTrigger_error.Location = new System.Drawing.Point(17, 75);
			this.selectLogTrigger_error.Name = "selectLogTrigger_error";
			this.selectLogTrigger_error.Size = new System.Drawing.Size(118, 19);
			this.selectLogTrigger_error.TabIndex = 0;
			this.selectLogTrigger_error.Text = "#LogType.Error";
			this.selectLogTrigger_error.UseVisualStyleBackColor = true;
			// 
			// selectLogTrigger_warning
			// 
			this.selectLogTrigger_warning.AutoSize = true;
			this.selectLogTrigger_warning.Location = new System.Drawing.Point(17, 50);
			this.selectLogTrigger_warning.Name = "selectLogTrigger_warning";
			this.selectLogTrigger_warning.Size = new System.Drawing.Size(136, 19);
			this.selectLogTrigger_warning.TabIndex = 0;
			this.selectLogTrigger_warning.Text = "#LogType.Warning";
			this.selectLogTrigger_warning.UseVisualStyleBackColor = true;
			// 
			// selectLogTrigger_information
			// 
			this.selectLogTrigger_information.AutoSize = true;
			this.selectLogTrigger_information.Location = new System.Drawing.Point(17, 25);
			this.selectLogTrigger_information.Name = "selectLogTrigger_information";
			this.selectLogTrigger_information.Size = new System.Drawing.Size(158, 19);
			this.selectLogTrigger_information.TabIndex = 0;
			this.selectLogTrigger_information.Text = "#LogType.Information";
			this.selectLogTrigger_information.UseVisualStyleBackColor = true;
			// 
			// selectLogVisible
			// 
			this.selectLogVisible.AutoSize = true;
			this.selectLogVisible.Location = new System.Drawing.Point(16, 22);
			this.selectLogVisible.Name = "selectLogVisible";
			this.selectLogVisible.Size = new System.Drawing.Size(157, 19);
			this.selectLogVisible.TabIndex = 0;
			this.selectLogVisible.Text = ":common/label/visible";
			this.selectLogVisible.UseVisualStyleBackColor = true;
			// 
			// selectMainLanguage
			// 
			this.selectMainLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectMainLanguage.FormattingEnabled = true;
			this.selectMainLanguage.Location = new System.Drawing.Point(108, 16);
			this.selectMainLanguage.Name = "selectMainLanguage";
			this.selectMainLanguage.Size = new System.Drawing.Size(121, 23);
			this.selectMainLanguage.TabIndex = 1;
			// 
			// labelMainLanguage
			// 
			this.labelMainLanguage.AutoSize = true;
			this.labelMainLanguage.Location = new System.Drawing.Point(15, 19);
			this.labelMainLanguage.Name = "labelMainLanguage";
			this.labelMainLanguage.Size = new System.Drawing.Size(143, 15);
			this.labelMainLanguage.TabIndex = 0;
			this.labelMainLanguage.Text = ":setting/label/language";
			// 
			// tabSetting_pageLauncher
			// 
			this.tabSetting_pageLauncher.AllowDrop = true;
			this.tabSetting_pageLauncher.Controls.Add(this.splitContainer1);
			this.tabSetting_pageLauncher.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageLauncher.Name = "tabSetting_pageLauncher";
			this.tabSetting_pageLauncher.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting_pageLauncher.Size = new System.Drawing.Size(741, 266);
			this.tabSetting_pageLauncher.TabIndex = 1;
			this.tabSetting_pageLauncher.Text = ":setting/tab/launcher";
			this.tabSetting_pageLauncher.UseVisualStyleBackColor = true;
			this.tabSetting_pageLauncher.DragDrop += new System.Windows.Forms.DragEventHandler(this.PageLauncher_DragDrop);
			this.tabSetting_pageLauncher.DragEnter += new System.Windows.Forms.DragEventHandler(this.PageLauncher_DragEnter);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.selecterLauncher);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabLauncher);
			this.splitContainer1.Panel2.Enabled = false;
			this.splitContainer1.Size = new System.Drawing.Size(735, 258);
			this.splitContainer1.SplitterDistance = 192;
			this.splitContainer1.TabIndex = 0;
			// 
			// selecterLauncher
			// 
			this.selecterLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterLauncher.Filtering = false;
			this.selecterLauncher.FilterType = PeMain.UI.LauncherItemSelecterType.Full;
			this.selecterLauncher.IconScale = PeUtility.IconScale.Small;
			this.selecterLauncher.ItemEdit = true;
			this.selecterLauncher.Location = new System.Drawing.Point(0, 0);
			this.selecterLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterLauncher.Name = "selecterLauncher";
			this.selecterLauncher.SelectedItem = null;
			this.selecterLauncher.Size = new System.Drawing.Size(192, 258);
			this.selecterLauncher.TabIndex = 0;
			this.selecterLauncher.CreateItem += new System.EventHandler<PeMain.UI.CreateItemEventArg>(this.SelecterLauncher_CreateItem);
			this.selecterLauncher.SelectChangedItem += new System.EventHandler<PeMain.UI.SelectedItemEventArg>(this.SelecterLauncher_SelectChnagedItem);
			// 
			// tabLauncher
			// 
			this.tabLauncher.Controls.Add(this.tabLauncher_pageCommon);
			this.tabLauncher.Controls.Add(this.tabLauncher_pageEnv);
			this.tabLauncher.Controls.Add(this.tabLauncher_pageOthers);
			this.tabLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabLauncher.Location = new System.Drawing.Point(0, 0);
			this.tabLauncher.Name = "tabLauncher";
			this.tabLauncher.SelectedIndex = 0;
			this.tabLauncher.Size = new System.Drawing.Size(539, 258);
			this.tabLauncher.TabIndex = 13;
			// 
			// tabLauncher_pageCommon
			// 
			this.tabLauncher_pageCommon.Controls.Add(this.groupLauncherType);
			this.tabLauncher_pageCommon.Controls.Add(this.labelLauncherName);
			this.tabLauncher_pageCommon.Controls.Add(this.labelLauncherOption);
			this.tabLauncher_pageCommon.Controls.Add(this.inputLauncherName);
			this.tabLauncher_pageCommon.Controls.Add(this.inputLauncherOption);
			this.tabLauncher_pageCommon.Controls.Add(this.labelLauncherCommand);
			this.tabLauncher_pageCommon.Controls.Add(this.commandLauncherOptionDirPath);
			this.tabLauncher_pageCommon.Controls.Add(this.commandLauncherFilePath);
			this.tabLauncher_pageCommon.Controls.Add(this.commandLauncherOptionFilePath);
			this.tabLauncher_pageCommon.Controls.Add(this.commandLauncherDirPath);
			this.tabLauncher_pageCommon.Controls.Add(this.commandLauncherWorkDirPath);
			this.tabLauncher_pageCommon.Controls.Add(this.labelLauncherIconPath);
			this.tabLauncher_pageCommon.Controls.Add(this.commandLauncherIconPath);
			this.tabLauncher_pageCommon.Controls.Add(this.labelLauncherWorkDirPath);
			this.tabLauncher_pageCommon.Controls.Add(this.inputLauncherCommand);
			this.tabLauncher_pageCommon.Controls.Add(this.inputLauncherWorkDirPath);
			this.tabLauncher_pageCommon.Controls.Add(this.inputLauncherIconPath);
			this.tabLauncher_pageCommon.Location = new System.Drawing.Point(4, 24);
			this.tabLauncher_pageCommon.Name = "tabLauncher_pageCommon";
			this.tabLauncher_pageCommon.Padding = new System.Windows.Forms.Padding(3);
			this.tabLauncher_pageCommon.Size = new System.Drawing.Size(531, 230);
			this.tabLauncher_pageCommon.TabIndex = 0;
			this.tabLauncher_pageCommon.Text = ":setting/tab/launcher/basic";
			this.tabLauncher_pageCommon.UseVisualStyleBackColor = true;
			// 
			// groupLauncherType
			// 
			this.groupLauncherType.Controls.Add(this.selectLauncherType_uri);
			this.groupLauncherType.Controls.Add(this.selectLauncherType_file);
			this.groupLauncherType.Location = new System.Drawing.Point(6, 3);
			this.groupLauncherType.Name = "groupLauncherType";
			this.groupLauncherType.Size = new System.Drawing.Size(221, 59);
			this.groupLauncherType.TabIndex = 0;
			this.groupLauncherType.TabStop = false;
			this.groupLauncherType.Text = ":setting/group/item-type";
			// 
			// selectLauncherType_uri
			// 
			this.selectLauncherType_uri.Location = new System.Drawing.Point(117, 23);
			this.selectLauncherType_uri.Name = "selectLauncherType_uri";
			this.selectLauncherType_uri.Size = new System.Drawing.Size(104, 24);
			this.selectLauncherType_uri.TabIndex = 1;
			this.selectLauncherType_uri.TabStop = true;
			this.selectLauncherType_uri.Text = "#LauncherType.URI";
			this.selectLauncherType_uri.UseVisualStyleBackColor = true;
			this.selectLauncherType_uri.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// selectLauncherType_file
			// 
			this.selectLauncherType_file.Location = new System.Drawing.Point(7, 23);
			this.selectLauncherType_file.Name = "selectLauncherType_file";
			this.selectLauncherType_file.Size = new System.Drawing.Size(104, 24);
			this.selectLauncherType_file.TabIndex = 0;
			this.selectLauncherType_file.TabStop = true;
			this.selectLauncherType_file.Text = "#LauncherType.File";
			this.selectLauncherType_file.UseVisualStyleBackColor = true;
			this.selectLauncherType_file.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// labelLauncherName
			// 
			this.labelLauncherName.AutoSize = true;
			this.labelLauncherName.Location = new System.Drawing.Point(6, 68);
			this.labelLauncherName.Name = "labelLauncherName";
			this.labelLauncherName.Size = new System.Drawing.Size(156, 15);
			this.labelLauncherName.TabIndex = 4;
			this.labelLauncherName.Text = ":setting/label/item-name";
			// 
			// labelLauncherOption
			// 
			this.labelLauncherOption.AutoSize = true;
			this.labelLauncherOption.Location = new System.Drawing.Point(6, 127);
			this.labelLauncherOption.Name = "labelLauncherOption";
			this.labelLauncherOption.Size = new System.Drawing.Size(127, 15);
			this.labelLauncherOption.TabIndex = 10;
			this.labelLauncherOption.Text = ":setting/label/option";
			// 
			// inputLauncherName
			// 
			this.inputLauncherName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherName.Location = new System.Drawing.Point(112, 65);
			this.inputLauncherName.Name = "inputLauncherName";
			this.inputLauncherName.Size = new System.Drawing.Size(319, 23);
			this.inputLauncherName.TabIndex = 3;
			this.inputLauncherName.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherOption
			// 
			this.inputLauncherOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherOption.Location = new System.Drawing.Point(112, 124);
			this.inputLauncherOption.Name = "inputLauncherOption";
			this.inputLauncherOption.Size = new System.Drawing.Size(236, 23);
			this.inputLauncherOption.TabIndex = 9;
			this.inputLauncherOption.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// labelLauncherCommand
			// 
			this.labelLauncherCommand.AutoSize = true;
			this.labelLauncherCommand.Location = new System.Drawing.Point(6, 98);
			this.labelLauncherCommand.Name = "labelLauncherCommand";
			this.labelLauncherCommand.Size = new System.Drawing.Size(149, 15);
			this.labelLauncherCommand.TabIndex = 4;
			this.labelLauncherCommand.Text = ":setting/label/command";
			// 
			// commandLauncherOptionDirPath
			// 
			this.commandLauncherOptionDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandLauncherOptionDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandLauncherOptionDirPath.Location = new System.Drawing.Point(398, 122);
			this.commandLauncherOptionDirPath.Name = "commandLauncherOptionDirPath";
			this.commandLauncherOptionDirPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherOptionDirPath.TabIndex = 7;
			this.commandLauncherOptionDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherOptionDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherOptionDirPath.Click += new System.EventHandler(this.CommandLauncherOptionDirPath_Click);
			// 
			// commandLauncherFilePath
			// 
			this.commandLauncherFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandLauncherFilePath.Image = global::PeMain.Properties.Images.File;
			this.commandLauncherFilePath.Location = new System.Drawing.Point(359, 93);
			this.commandLauncherFilePath.Name = "commandLauncherFilePath";
			this.commandLauncherFilePath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherFilePath.TabIndex = 2;
			this.commandLauncherFilePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherFilePath.UseVisualStyleBackColor = true;
			this.commandLauncherFilePath.Click += new System.EventHandler(this.CommandLauncherFilePath_Click);
			// 
			// commandLauncherOptionFilePath
			// 
			this.commandLauncherOptionFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandLauncherOptionFilePath.Image = global::PeMain.Properties.Images.File;
			this.commandLauncherOptionFilePath.Location = new System.Drawing.Point(359, 122);
			this.commandLauncherOptionFilePath.Name = "commandLauncherOptionFilePath";
			this.commandLauncherOptionFilePath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherOptionFilePath.TabIndex = 8;
			this.commandLauncherOptionFilePath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherOptionFilePath.UseVisualStyleBackColor = true;
			this.commandLauncherOptionFilePath.Click += new System.EventHandler(this.CommandLauncherOptionFilePath_Click);
			// 
			// commandLauncherDirPath
			// 
			this.commandLauncherDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandLauncherDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandLauncherDirPath.Location = new System.Drawing.Point(398, 94);
			this.commandLauncherDirPath.Name = "commandLauncherDirPath";
			this.commandLauncherDirPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherDirPath.TabIndex = 2;
			this.commandLauncherDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherDirPath.Click += new System.EventHandler(this.CommandLauncherDirPath_Click);
			// 
			// commandLauncherWorkDirPath
			// 
			this.commandLauncherWorkDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandLauncherWorkDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandLauncherWorkDirPath.Location = new System.Drawing.Point(359, 151);
			this.commandLauncherWorkDirPath.Name = "commandLauncherWorkDirPath";
			this.commandLauncherWorkDirPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherWorkDirPath.TabIndex = 2;
			this.commandLauncherWorkDirPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherWorkDirPath.UseVisualStyleBackColor = true;
			this.commandLauncherWorkDirPath.Click += new System.EventHandler(this.CommandLauncherWorkDirPath_Click);
			// 
			// labelLauncherIconPath
			// 
			this.labelLauncherIconPath.AutoSize = true;
			this.labelLauncherIconPath.Location = new System.Drawing.Point(6, 185);
			this.labelLauncherIconPath.Name = "labelLauncherIconPath";
			this.labelLauncherIconPath.Size = new System.Drawing.Size(145, 15);
			this.labelLauncherIconPath.TabIndex = 4;
			this.labelLauncherIconPath.Text = ":setting/label/icon-path";
			// 
			// commandLauncherIconPath
			// 
			this.commandLauncherIconPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandLauncherIconPath.Image = global::PeMain.Properties.Images.File;
			this.commandLauncherIconPath.Location = new System.Drawing.Point(359, 182);
			this.commandLauncherIconPath.Name = "commandLauncherIconPath";
			this.commandLauncherIconPath.Size = new System.Drawing.Size(33, 25);
			this.commandLauncherIconPath.TabIndex = 2;
			this.commandLauncherIconPath.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.commandLauncherIconPath.UseVisualStyleBackColor = true;
			this.commandLauncherIconPath.Click += new System.EventHandler(this.CommandLauncherIconPath_Click);
			// 
			// labelLauncherWorkDirPath
			// 
			this.labelLauncherWorkDirPath.AutoSize = true;
			this.labelLauncherWorkDirPath.Location = new System.Drawing.Point(6, 156);
			this.labelLauncherWorkDirPath.Name = "labelLauncherWorkDirPath";
			this.labelLauncherWorkDirPath.Size = new System.Drawing.Size(139, 15);
			this.labelLauncherWorkDirPath.TabIndex = 4;
			this.labelLauncherWorkDirPath.Text = ":setting/label/work-dir";
			// 
			// inputLauncherCommand
			// 
			this.inputLauncherCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherCommand.Location = new System.Drawing.Point(112, 95);
			this.inputLauncherCommand.Name = "inputLauncherCommand";
			this.inputLauncherCommand.Size = new System.Drawing.Size(236, 23);
			this.inputLauncherCommand.TabIndex = 3;
			this.inputLauncherCommand.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherWorkDirPath
			// 
			this.inputLauncherWorkDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherWorkDirPath.Location = new System.Drawing.Point(112, 153);
			this.inputLauncherWorkDirPath.Name = "inputLauncherWorkDirPath";
			this.inputLauncherWorkDirPath.Size = new System.Drawing.Size(236, 23);
			this.inputLauncherWorkDirPath.TabIndex = 3;
			this.inputLauncherWorkDirPath.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherIconPath
			// 
			this.inputLauncherIconPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherIconPath.Location = new System.Drawing.Point(112, 182);
			this.inputLauncherIconPath.Name = "inputLauncherIconPath";
			this.inputLauncherIconPath.Size = new System.Drawing.Size(236, 23);
			this.inputLauncherIconPath.TabIndex = 3;
			this.inputLauncherIconPath.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// tabLauncher_pageEnv
			// 
			this.tabLauncher_pageEnv.Controls.Add(this.panelEnv);
			this.tabLauncher_pageEnv.Controls.Add(this.selectLauncherEnv);
			this.tabLauncher_pageEnv.Location = new System.Drawing.Point(4, 22);
			this.tabLauncher_pageEnv.Name = "tabLauncher_pageEnv";
			this.tabLauncher_pageEnv.Padding = new System.Windows.Forms.Padding(3);
			this.tabLauncher_pageEnv.Size = new System.Drawing.Size(531, 234);
			this.tabLauncher_pageEnv.TabIndex = 1;
			this.tabLauncher_pageEnv.Text = ":common/tab/env";
			this.tabLauncher_pageEnv.UseVisualStyleBackColor = true;
			// 
			// panelEnv
			// 
			this.panelEnv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panelEnv.Location = new System.Drawing.Point(6, 31);
			this.panelEnv.Name = "panelEnv";
			// 
			// panelEnv.Panel1
			// 
			this.panelEnv.Panel1.Controls.Add(this.envLauncherUpdate);
			// 
			// panelEnv.Panel2
			// 
			this.panelEnv.Panel2.Controls.Add(this.envLauncherRemove);
			this.panelEnv.Size = new System.Drawing.Size(519, 126);
			this.panelEnv.SplitterDistance = 339;
			this.panelEnv.TabIndex = 18;
			// 
			// envLauncherUpdate
			// 
			this.envLauncherUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envLauncherUpdate.Location = new System.Drawing.Point(0, 0);
			this.envLauncherUpdate.Name = "envLauncherUpdate";
			this.envLauncherUpdate.Size = new System.Drawing.Size(339, 126);
			this.envLauncherUpdate.TabIndex = 15;
			this.envLauncherUpdate.ValueChanged += new System.EventHandler<System.EventArgs>(this.EnvLauncherUpdate_ValueChanged);
			// 
			// envLauncherRemove
			// 
			this.envLauncherRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envLauncherRemove.Location = new System.Drawing.Point(0, 0);
			this.envLauncherRemove.Name = "envLauncherRemove";
			this.envLauncherRemove.Size = new System.Drawing.Size(176, 126);
			this.envLauncherRemove.TabIndex = 16;
			this.envLauncherRemove.ValueChanged += new System.EventHandler<System.EventArgs>(this.EnvLauncherRemove_ValueChanged);
			// 
			// selectLauncherEnv
			// 
			this.selectLauncherEnv.AutoSize = true;
			this.selectLauncherEnv.Location = new System.Drawing.Point(6, 6);
			this.selectLauncherEnv.Name = "selectLauncherEnv";
			this.selectLauncherEnv.Size = new System.Drawing.Size(170, 19);
			this.selectLauncherEnv.TabIndex = 14;
			this.selectLauncherEnv.Text = ":execute/check/edit-env";
			this.selectLauncherEnv.UseVisualStyleBackColor = true;
			this.selectLauncherEnv.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// tabLauncher_pageOthers
			// 
			this.tabLauncher_pageOthers.Controls.Add(this.selectLauncherAdmin);
			this.tabLauncher_pageOthers.Controls.Add(this.labelLauncherTag);
			this.tabLauncher_pageOthers.Controls.Add(this.selectLauncherStdStream);
			this.tabLauncher_pageOthers.Controls.Add(this.inputLauncherTag);
			this.tabLauncher_pageOthers.Controls.Add(this.inputLauncherNote);
			this.tabLauncher_pageOthers.Controls.Add(this.labelLauncherNote);
			this.tabLauncher_pageOthers.Location = new System.Drawing.Point(4, 22);
			this.tabLauncher_pageOthers.Name = "tabLauncher_pageOthers";
			this.tabLauncher_pageOthers.Size = new System.Drawing.Size(531, 234);
			this.tabLauncher_pageOthers.TabIndex = 2;
			this.tabLauncher_pageOthers.Text = ":setting/tab/launcher/others";
			this.tabLauncher_pageOthers.UseVisualStyleBackColor = true;
			// 
			// selectLauncherAdmin
			// 
			this.selectLauncherAdmin.AutoSize = true;
			this.selectLauncherAdmin.Location = new System.Drawing.Point(6, 31);
			this.selectLauncherAdmin.Name = "selectLauncherAdmin";
			this.selectLauncherAdmin.Size = new System.Drawing.Size(163, 19);
			this.selectLauncherAdmin.TabIndex = 13;
			this.selectLauncherAdmin.Text = ":common/check/admin";
			this.selectLauncherAdmin.UseVisualStyleBackColor = true;
			this.selectLauncherAdmin.CheckedChanged += new System.EventHandler(this.InputLauncherIconIndex_ValueChanged);
			// 
			// labelLauncherTag
			// 
			this.labelLauncherTag.AutoSize = true;
			this.labelLauncherTag.Location = new System.Drawing.Point(8, 65);
			this.labelLauncherTag.Name = "labelLauncherTag";
			this.labelLauncherTag.Size = new System.Drawing.Size(116, 15);
			this.labelLauncherTag.TabIndex = 4;
			this.labelLauncherTag.Text = ":setting/label/tags";
			// 
			// selectLauncherStdStream
			// 
			this.selectLauncherStdStream.AutoSize = true;
			this.selectLauncherStdStream.Location = new System.Drawing.Point(6, 6);
			this.selectLauncherStdStream.Name = "selectLauncherStdStream";
			this.selectLauncherStdStream.Size = new System.Drawing.Size(181, 19);
			this.selectLauncherStdStream.TabIndex = 12;
			this.selectLauncherStdStream.Text = ":setting/check/std-stream";
			this.selectLauncherStdStream.UseVisualStyleBackColor = true;
			this.selectLauncherStdStream.CheckedChanged += new System.EventHandler(this.SelectLauncherType_file_CheckedChanged);
			// 
			// inputLauncherTag
			// 
			this.inputLauncherTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherTag.Location = new System.Drawing.Point(84, 62);
			this.inputLauncherTag.Name = "inputLauncherTag";
			this.inputLauncherTag.Size = new System.Drawing.Size(433, 23);
			this.inputLauncherTag.TabIndex = 3;
			this.inputLauncherTag.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// inputLauncherNote
			// 
			this.inputLauncherNote.AcceptsReturn = true;
			this.inputLauncherNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.inputLauncherNote.Location = new System.Drawing.Point(84, 91);
			this.inputLauncherNote.Multiline = true;
			this.inputLauncherNote.Name = "inputLauncherNote";
			this.inputLauncherNote.Size = new System.Drawing.Size(433, 98);
			this.inputLauncherNote.TabIndex = 3;
			this.inputLauncherNote.TextChanged += new System.EventHandler(this.InputLauncherName_TextChanged);
			// 
			// labelLauncherNote
			// 
			this.labelLauncherNote.AutoSize = true;
			this.labelLauncherNote.Location = new System.Drawing.Point(8, 94);
			this.labelLauncherNote.Name = "labelLauncherNote";
			this.labelLauncherNote.Size = new System.Drawing.Size(117, 15);
			this.labelLauncherNote.TabIndex = 6;
			this.labelLauncherNote.Text = ":setting/label/note";
			// 
			// tabSetting_pageToolbar
			// 
			this.tabSetting_pageToolbar.Controls.Add(this.groupToolbar);
			this.tabSetting_pageToolbar.Controls.Add(this.splitContainer2);
			this.tabSetting_pageToolbar.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageToolbar.Name = "tabSetting_pageToolbar";
			this.tabSetting_pageToolbar.Size = new System.Drawing.Size(741, 266);
			this.tabSetting_pageToolbar.TabIndex = 3;
			this.tabSetting_pageToolbar.Text = ":setting/tab/toolbar";
			this.tabSetting_pageToolbar.UseVisualStyleBackColor = true;
			// 
			// groupToolbar
			// 
			this.groupToolbar.Controls.Add(this.selectToolbarItem);
			this.groupToolbar.Controls.Add(this.labelToolbarFont);
			this.groupToolbar.Controls.Add(this.inputToolbarTextWidth);
			this.groupToolbar.Controls.Add(this.commandToolbarFont);
			this.groupToolbar.Controls.Add(this.selectToolbarShowText);
			this.groupToolbar.Controls.Add(this.selectToolbarTopmost);
			this.groupToolbar.Controls.Add(this.selectToolbarAutoHide);
			this.groupToolbar.Controls.Add(this.labelToolbarTextWidth);
			this.groupToolbar.Controls.Add(this.selectToolbarVisible);
			this.groupToolbar.Controls.Add(this.labelToolbarIcon);
			this.groupToolbar.Controls.Add(this.selectToolbarPosition);
			this.groupToolbar.Controls.Add(this.selectToolbarIcon);
			this.groupToolbar.Controls.Add(this.labelToolbarPosition);
			this.groupToolbar.Location = new System.Drawing.Point(7, 3);
			this.groupToolbar.Name = "groupToolbar";
			this.groupToolbar.Size = new System.Drawing.Size(242, 260);
			this.groupToolbar.TabIndex = 18;
			this.groupToolbar.TabStop = false;
			this.groupToolbar.Text = "☃";
			// 
			// selectToolbarItem
			// 
			this.selectToolbarItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarItem.FormattingEnabled = true;
			this.selectToolbarItem.Location = new System.Drawing.Point(6, 0);
			this.selectToolbarItem.Name = "selectToolbarItem";
			this.selectToolbarItem.Size = new System.Drawing.Size(186, 23);
			this.selectToolbarItem.TabIndex = 19;
			this.selectToolbarItem.SelectedValueChanged += new System.EventHandler(this.SelectToolbarItem_SelectedValueChanged);
			// 
			// labelToolbarFont
			// 
			this.labelToolbarFont.AutoSize = true;
			this.labelToolbarFont.Location = new System.Drawing.Point(6, 229);
			this.labelToolbarFont.Name = "labelToolbarFont";
			this.labelToolbarFont.Size = new System.Drawing.Size(125, 15);
			this.labelToolbarFont.TabIndex = 5;
			this.labelToolbarFont.Text = ":common/label/font";
			// 
			// inputToolbarTextWidth
			// 
			this.inputToolbarTextWidth.Location = new System.Drawing.Point(129, 195);
			this.inputToolbarTextWidth.Name = "inputToolbarTextWidth";
			this.inputToolbarTextWidth.Size = new System.Drawing.Size(103, 23);
			this.inputToolbarTextWidth.TabIndex = 17;
			// 
			// commandToolbarFont
			// 
			this.commandToolbarFont.AutoSize = true;
			this.commandToolbarFont.Location = new System.Drawing.Point(79, 224);
			this.commandToolbarFont.Name = "commandToolbarFont";
			this.commandToolbarFont.Size = new System.Drawing.Size(153, 25);
			this.commandToolbarFont.TabIndex = 6;
			this.commandToolbarFont.Text = "{FAMILY} {PT} ...";
			this.commandToolbarFont.UseVisualStyleBackColor = true;
			// 
			// selectToolbarShowText
			// 
			this.selectToolbarShowText.AutoSize = true;
			this.selectToolbarShowText.Location = new System.Drawing.Point(6, 170);
			this.selectToolbarShowText.Name = "selectToolbarShowText";
			this.selectToolbarShowText.Size = new System.Drawing.Size(175, 19);
			this.selectToolbarShowText.TabIndex = 16;
			this.selectToolbarShowText.Text = ":setting/check/show-text";
			this.selectToolbarShowText.UseVisualStyleBackColor = true;
			// 
			// selectToolbarTopmost
			// 
			this.selectToolbarTopmost.AutoSize = true;
			this.selectToolbarTopmost.Location = new System.Drawing.Point(6, 95);
			this.selectToolbarTopmost.Name = "selectToolbarTopmost";
			this.selectToolbarTopmost.Size = new System.Drawing.Size(170, 19);
			this.selectToolbarTopmost.TabIndex = 7;
			this.selectToolbarTopmost.Text = ":common/label/topmost";
			this.selectToolbarTopmost.UseVisualStyleBackColor = true;
			// 
			// selectToolbarAutoHide
			// 
			this.selectToolbarAutoHide.AutoSize = true;
			this.selectToolbarAutoHide.Location = new System.Drawing.Point(6, 145);
			this.selectToolbarAutoHide.Name = "selectToolbarAutoHide";
			this.selectToolbarAutoHide.Size = new System.Drawing.Size(171, 19);
			this.selectToolbarAutoHide.TabIndex = 7;
			this.selectToolbarAutoHide.Text = ":setting/check/auto-hide";
			this.selectToolbarAutoHide.UseVisualStyleBackColor = true;
			// 
			// labelToolbarTextWidth
			// 
			this.labelToolbarTextWidth.AutoSize = true;
			this.labelToolbarTextWidth.Location = new System.Drawing.Point(6, 198);
			this.labelToolbarTextWidth.Name = "labelToolbarTextWidth";
			this.labelToolbarTextWidth.Size = new System.Drawing.Size(152, 15);
			this.labelToolbarTextWidth.TabIndex = 12;
			this.labelToolbarTextWidth.Text = ":setting/label/text-width";
			// 
			// selectToolbarVisible
			// 
			this.selectToolbarVisible.AutoSize = true;
			this.selectToolbarVisible.Location = new System.Drawing.Point(6, 120);
			this.selectToolbarVisible.Name = "selectToolbarVisible";
			this.selectToolbarVisible.Size = new System.Drawing.Size(157, 19);
			this.selectToolbarVisible.TabIndex = 8;
			this.selectToolbarVisible.Text = ":common/label/visible";
			this.selectToolbarVisible.UseVisualStyleBackColor = true;
			// 
			// labelToolbarIcon
			// 
			this.labelToolbarIcon.AutoSize = true;
			this.labelToolbarIcon.Location = new System.Drawing.Point(6, 69);
			this.labelToolbarIcon.Name = "labelToolbarIcon";
			this.labelToolbarIcon.Size = new System.Drawing.Size(101, 15);
			this.labelToolbarIcon.TabIndex = 12;
			this.labelToolbarIcon.Text = ":enum/icon-size";
			// 
			// selectToolbarPosition
			// 
			this.selectToolbarPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarPosition.FormattingEnabled = true;
			this.selectToolbarPosition.Location = new System.Drawing.Point(111, 37);
			this.selectToolbarPosition.Name = "selectToolbarPosition";
			this.selectToolbarPosition.Size = new System.Drawing.Size(121, 23);
			this.selectToolbarPosition.TabIndex = 9;
			// 
			// selectToolbarIcon
			// 
			this.selectToolbarIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectToolbarIcon.FormattingEnabled = true;
			this.selectToolbarIcon.Location = new System.Drawing.Point(111, 66);
			this.selectToolbarIcon.Name = "selectToolbarIcon";
			this.selectToolbarIcon.Size = new System.Drawing.Size(121, 23);
			this.selectToolbarIcon.TabIndex = 11;
			// 
			// labelToolbarPosition
			// 
			this.labelToolbarPosition.AutoSize = true;
			this.labelToolbarPosition.Location = new System.Drawing.Point(6, 40);
			this.labelToolbarPosition.Name = "labelToolbarPosition";
			this.labelToolbarPosition.Size = new System.Drawing.Size(142, 15);
			this.labelToolbarPosition.TabIndex = 10;
			this.labelToolbarPosition.Text = ":enum/toolbar-position";
			// 
			// splitContainer2
			// 
			this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer2.Location = new System.Drawing.Point(255, 3);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.toolStripContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.selecterToolbar);
			this.splitContainer2.Size = new System.Drawing.Size(481, 260);
			this.splitContainer2.SplitterDistance = 286;
			this.splitContainer2.TabIndex = 15;
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.treeToolbarItemGroup);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(286, 235);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(286, 260);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolToolbarGroup);
			// 
			// treeToolbarItemGroup
			// 
			this.treeToolbarItemGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeToolbarItemGroup.HideSelection = false;
			this.treeToolbarItemGroup.LabelEdit = true;
			this.treeToolbarItemGroup.Location = new System.Drawing.Point(0, 0);
			this.treeToolbarItemGroup.Name = "treeToolbarItemGroup";
			this.treeToolbarItemGroup.ShowPlusMinus = false;
			this.treeToolbarItemGroup.Size = new System.Drawing.Size(286, 235);
			this.treeToolbarItemGroup.TabIndex = 13;
			this.treeToolbarItemGroup.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeToolbarItemGroup_BeforeLabelEdit);
			this.treeToolbarItemGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeToolbarItemGroup_AfterSelect);
			this.treeToolbarItemGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeToolbarItemGroup_KeyDown);
			// 
			// toolToolbarGroup
			// 
			this.toolToolbarGroup.Dock = System.Windows.Forms.DockStyle.None;
			this.toolToolbarGroup.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolToolbarGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolToolbarGroup_addGroup,
									this.toolToolbarGroup_addItem,
									this.toolStripSeparator1,
									this.toolToolbarGroup_up,
									this.toolToolbarGroup_down,
									this.toolStripSeparator2,
									this.toolToolbarGroup_remove});
			this.toolToolbarGroup.Location = new System.Drawing.Point(0, 0);
			this.toolToolbarGroup.Name = "toolToolbarGroup";
			this.toolToolbarGroup.Size = new System.Drawing.Size(286, 25);
			this.toolToolbarGroup.Stretch = true;
			this.toolToolbarGroup.TabIndex = 0;
			this.toolToolbarGroup.Text = "toolStrip1";
			// 
			// toolToolbarGroup_addGroup
			// 
			this.toolToolbarGroup_addGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_addGroup.Image = global::PeMain.Properties.Images.Group;
			this.toolToolbarGroup_addGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_addGroup.Name = "toolToolbarGroup_addGroup";
			this.toolToolbarGroup_addGroup.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_addGroup.ToolTipText = ":setting/tips/add-group";
			this.toolToolbarGroup_addGroup.Click += new System.EventHandler(this.ToolToolbarGroup_addGroup_Click);
			// 
			// toolToolbarGroup_addItem
			// 
			this.toolToolbarGroup_addItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_addItem.Image = global::PeMain.Properties.Images.AddItem;
			this.toolToolbarGroup_addItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_addItem.Name = "toolToolbarGroup_addItem";
			this.toolToolbarGroup_addItem.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_addItem.ToolTipText = ":setting/tips/add-item";
			this.toolToolbarGroup_addItem.Click += new System.EventHandler(this.ToolToolbarGroup_addItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolToolbarGroup_up
			// 
			this.toolToolbarGroup_up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_up.Image = global::PeMain.Properties.Images.Up;
			this.toolToolbarGroup_up.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_up.Name = "toolToolbarGroup_up";
			this.toolToolbarGroup_up.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_up.ToolTipText = ":setting/tips/up-item";
			this.toolToolbarGroup_up.Click += new System.EventHandler(this.ToolToolbarGroup_up_Click);
			// 
			// toolToolbarGroup_down
			// 
			this.toolToolbarGroup_down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_down.Image = global::PeMain.Properties.Images.Down;
			this.toolToolbarGroup_down.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_down.Name = "toolToolbarGroup_down";
			this.toolToolbarGroup_down.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_down.ToolTipText = ":setting/tips/down-item";
			this.toolToolbarGroup_down.Click += new System.EventHandler(this.ToolToolbarGroup_down_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolToolbarGroup_remove
			// 
			this.toolToolbarGroup_remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolToolbarGroup_remove.Image = global::PeMain.Properties.Images.Remove;
			this.toolToolbarGroup_remove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolToolbarGroup_remove.Name = "toolToolbarGroup_remove";
			this.toolToolbarGroup_remove.Size = new System.Drawing.Size(23, 22);
			this.toolToolbarGroup_remove.ToolTipText = ":setting/tips/remove-item";
			this.toolToolbarGroup_remove.Click += new System.EventHandler(this.ToolToolbarGroup_remove_Click);
			// 
			// selecterToolbar
			// 
			this.selecterToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selecterToolbar.Filtering = false;
			this.selecterToolbar.FilterType = PeMain.UI.LauncherItemSelecterType.Full;
			this.selecterToolbar.IconScale = PeUtility.IconScale.Small;
			this.selecterToolbar.ItemEdit = false;
			this.selecterToolbar.Location = new System.Drawing.Point(0, 0);
			this.selecterToolbar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.selecterToolbar.Name = "selecterToolbar";
			this.selecterToolbar.SelectedItem = null;
			this.selecterToolbar.Size = new System.Drawing.Size(191, 260);
			this.selecterToolbar.TabIndex = 14;
			this.selecterToolbar.SelectChangedItem += new System.EventHandler<PeMain.UI.SelectedItemEventArg>(this.SelecterToolbar_SelectChangedItem);
			// 
			// tabSetting_pageCommand
			// 
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandHotkey);
			this.tabSetting_pageCommand.Controls.Add(this.inputCommandHotkey);
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandIcon);
			this.tabSetting_pageCommand.Controls.Add(this.selectCommandIcon);
			this.tabSetting_pageCommand.Controls.Add(this.inputCommandHideTime);
			this.tabSetting_pageCommand.Controls.Add(this.selectCommandTopmost);
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandHideTime);
			this.tabSetting_pageCommand.Controls.Add(this.commandCommandFont);
			this.tabSetting_pageCommand.Controls.Add(this.labelCommandFont);
			this.tabSetting_pageCommand.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageCommand.Name = "tabSetting_pageCommand";
			this.tabSetting_pageCommand.Padding = new System.Windows.Forms.Padding(3);
			this.tabSetting_pageCommand.Size = new System.Drawing.Size(741, 266);
			this.tabSetting_pageCommand.TabIndex = 2;
			this.tabSetting_pageCommand.Text = ":setting/tab/command";
			this.tabSetting_pageCommand.UseVisualStyleBackColor = true;
			// 
			// labelCommandHotkey
			// 
			this.labelCommandHotkey.Location = new System.Drawing.Point(35, 109);
			this.labelCommandHotkey.Name = "labelCommandHotkey";
			this.labelCommandHotkey.Size = new System.Drawing.Size(100, 23);
			this.labelCommandHotkey.TabIndex = 16;
			this.labelCommandHotkey.Text = "{HOT KEY}";
			// 
			// inputCommandHotkey
			// 
			this.inputCommandHotkey.BackColor = System.Drawing.Color.White;
			this.inputCommandHotkey.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.inputCommandHotkey.Hotkey = System.Windows.Forms.Keys.None;
			this.inputCommandHotkey.HotKeySetting = null;
			this.inputCommandHotkey.Location = new System.Drawing.Point(156, 105);
			this.inputCommandHotkey.Modifiers = PInvoke.Windows.MOD.None;
			this.inputCommandHotkey.Name = "inputCommandHotkey";
			this.inputCommandHotkey.ReadOnly = true;
			this.inputCommandHotkey.Registered = false;
			this.inputCommandHotkey.Size = new System.Drawing.Size(252, 23);
			this.inputCommandHotkey.TabIndex = 15;
			this.inputCommandHotkey.Text = "None";
			// 
			// labelCommandIcon
			// 
			this.labelCommandIcon.Location = new System.Drawing.Point(40, 187);
			this.labelCommandIcon.Name = "labelCommandIcon";
			this.labelCommandIcon.Size = new System.Drawing.Size(100, 23);
			this.labelCommandIcon.TabIndex = 14;
			this.labelCommandIcon.Text = ":enum/icon-size";
			// 
			// selectCommandIcon
			// 
			this.selectCommandIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selectCommandIcon.FormattingEnabled = true;
			this.selectCommandIcon.Location = new System.Drawing.Point(146, 190);
			this.selectCommandIcon.Name = "selectCommandIcon";
			this.selectCommandIcon.Size = new System.Drawing.Size(121, 23);
			this.selectCommandIcon.TabIndex = 13;
			// 
			// inputCommandHideTime
			// 
			this.inputCommandHideTime.Location = new System.Drawing.Point(156, 76);
			this.inputCommandHideTime.Name = "inputCommandHideTime";
			this.inputCommandHideTime.Size = new System.Drawing.Size(120, 23);
			this.inputCommandHideTime.TabIndex = 5;
			// 
			// selectCommandTopmost
			// 
			this.selectCommandTopmost.AutoSize = true;
			this.selectCommandTopmost.Location = new System.Drawing.Point(36, 160);
			this.selectCommandTopmost.Name = "selectCommandTopmost";
			this.selectCommandTopmost.Size = new System.Drawing.Size(175, 19);
			this.selectCommandTopmost.TabIndex = 4;
			this.selectCommandTopmost.Text = "::common/label/topmost";
			this.selectCommandTopmost.UseVisualStyleBackColor = true;
			// 
			// labelCommandHideTime
			// 
			this.labelCommandHideTime.Location = new System.Drawing.Point(35, 78);
			this.labelCommandHideTime.Name = "labelCommandHideTime";
			this.labelCommandHideTime.Size = new System.Drawing.Size(100, 23);
			this.labelCommandHideTime.TabIndex = 2;
			this.labelCommandHideTime.Text = "{HIDE TIME}";
			// 
			// commandCommandFont
			// 
			this.commandCommandFont.AutoSize = true;
			this.commandCommandFont.Location = new System.Drawing.Point(146, 23);
			this.commandCommandFont.Name = "commandCommandFont";
			this.commandCommandFont.Size = new System.Drawing.Size(171, 25);
			this.commandCommandFont.TabIndex = 1;
			this.commandCommandFont.Text = "{FAMILY} {PT} ...";
			this.commandCommandFont.UseVisualStyleBackColor = true;
			// 
			// labelCommandFont
			// 
			this.labelCommandFont.Location = new System.Drawing.Point(35, 28);
			this.labelCommandFont.Name = "labelCommandFont";
			this.labelCommandFont.Size = new System.Drawing.Size(100, 23);
			this.labelCommandFont.TabIndex = 0;
			this.labelCommandFont.Text = ":common/label/font";
			// 
			// tabSetting_pageNote
			// 
			this.tabSetting_pageNote.Controls.Add(this.groupNoteItem);
			this.tabSetting_pageNote.Controls.Add(this.groupNoteKey);
			this.tabSetting_pageNote.Controls.Add(this.commandNoteCaptionFont);
			this.tabSetting_pageNote.Controls.Add(this.labelNoteCaptionFont);
			this.tabSetting_pageNote.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageNote.Name = "tabSetting_pageNote";
			this.tabSetting_pageNote.Size = new System.Drawing.Size(741, 266);
			this.tabSetting_pageNote.TabIndex = 6;
			this.tabSetting_pageNote.Text = ":setting/tab/note";
			this.tabSetting_pageNote.UseVisualStyleBackColor = true;
			// 
			// groupNoteItem
			// 
			this.groupNoteItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupNoteItem.Controls.Add(this.gridNoteItems);
			this.groupNoteItem.Location = new System.Drawing.Point(272, 8);
			this.groupNoteItem.Name = "groupNoteItem";
			this.groupNoteItem.Size = new System.Drawing.Size(456, 258);
			this.groupNoteItem.TabIndex = 12;
			this.groupNoteItem.TabStop = false;
			this.groupNoteItem.Text = ":setting/group/item";
			// 
			// gridNoteItems
			// 
			this.gridNoteItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridNoteItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.gridNoteItems_columnRemove,
									this.gridNoteItems_columnId,
									this.gridNoteItems_columnVisible,
									this.gridNoteItems_columnLocked,
									this.gridNoteItems_columnTitle,
									this.gridNoteItems_columnBody,
									this.gridNoteItems_columnFont,
									this.gridNoteItems_columnFore,
									this.gridNoteItems_columnBack});
			this.gridNoteItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridNoteItems.Location = new System.Drawing.Point(3, 19);
			this.gridNoteItems.Name = "gridNoteItems";
			this.gridNoteItems.RowTemplate.Height = 21;
			this.gridNoteItems.Size = new System.Drawing.Size(450, 236);
			this.gridNoteItems.TabIndex = 0;
			// 
			// gridNoteItems_columnRemove
			// 
			this.gridNoteItems_columnRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridNoteItems_columnRemove.FillWeight = 30F;
			this.gridNoteItems_columnRemove.HeaderText = ":setting/column/note/remove";
			this.gridNoteItems_columnRemove.Name = "gridNoteItems_columnRemove";
			this.gridNoteItems_columnRemove.Width = 189;
			// 
			// gridNoteItems_columnId
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.gridNoteItems_columnId.DefaultCellStyle = dataGridViewCellStyle1;
			this.gridNoteItems_columnId.FillWeight = 40F;
			this.gridNoteItems_columnId.HeaderText = ":setting/column/note/id";
			this.gridNoteItems_columnId.Name = "gridNoteItems_columnId";
			this.gridNoteItems_columnId.ReadOnly = true;
			this.gridNoteItems_columnId.Width = 40;
			// 
			// gridNoteItems_columnVisible
			// 
			this.gridNoteItems_columnVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridNoteItems_columnVisible.FillWeight = 30F;
			this.gridNoteItems_columnVisible.HeaderText = ":setting/column/note/visible";
			this.gridNoteItems_columnVisible.Name = "gridNoteItems_columnVisible";
			this.gridNoteItems_columnVisible.Width = 180;
			// 
			// gridNoteItems_columnLocked
			// 
			this.gridNoteItems_columnLocked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridNoteItems_columnLocked.HeaderText = ":setting/column/note/locked";
			this.gridNoteItems_columnLocked.Name = "gridNoteItems_columnLocked";
			this.gridNoteItems_columnLocked.Width = 181;
			// 
			// gridNoteItems_columnTitle
			// 
			this.gridNoteItems_columnTitle.HeaderText = ":setting/column/note/title";
			this.gridNoteItems_columnTitle.Name = "gridNoteItems_columnTitle";
			// 
			// gridNoteItems_columnBody
			// 
			this.gridNoteItems_columnBody.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnBody.DefaultCellStyle = dataGridViewCellStyle2;
			this.gridNoteItems_columnBody.HeaderText = ":setting/column/note/body";
			this.gridNoteItems_columnBody.MinimumWidth = 100;
			this.gridNoteItems_columnBody.Name = "gridNoteItems_columnBody";
			this.gridNoteItems_columnBody.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.gridNoteItems_columnBody.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// gridNoteItems_columnFont
			// 
			this.gridNoteItems_columnFont.FillWeight = 80F;
			this.gridNoteItems_columnFont.HeaderText = ":setting/column/note/font";
			this.gridNoteItems_columnFont.Name = "gridNoteItems_columnFont";
			this.gridNoteItems_columnFont.Width = 80;
			// 
			// gridNoteItems_columnFore
			// 
			this.gridNoteItems_columnFore.FillWeight = 80F;
			this.gridNoteItems_columnFore.HeaderText = ":setting/column/note/fore";
			this.gridNoteItems_columnFore.Name = "gridNoteItems_columnFore";
			this.gridNoteItems_columnFore.Width = 80;
			// 
			// gridNoteItems_columnBack
			// 
			this.gridNoteItems_columnBack.FillWeight = 80F;
			this.gridNoteItems_columnBack.HeaderText = ":setting/column/note/back";
			this.gridNoteItems_columnBack.Name = "gridNoteItems_columnBack";
			this.gridNoteItems_columnBack.Width = 80;
			// 
			// groupNoteKey
			// 
			this.groupNoteKey.Controls.Add(this.flowLayoutPanel2);
			this.groupNoteKey.Location = new System.Drawing.Point(8, 8);
			this.groupNoteKey.Name = "groupNoteKey";
			this.groupNoteKey.Size = new System.Drawing.Size(256, 168);
			this.groupNoteKey.TabIndex = 11;
			this.groupNoteKey.TabStop = false;
			this.groupNoteKey.Text = ":setting/group/key";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.labelNoteCreate);
			this.flowLayoutPanel2.Controls.Add(this.inputNoteCreate);
			this.flowLayoutPanel2.Controls.Add(this.labelNoteHiddent);
			this.flowLayoutPanel2.Controls.Add(this.inputNoteHidden);
			this.flowLayoutPanel2.Controls.Add(this.labelNoteCompact);
			this.flowLayoutPanel2.Controls.Add(this.inputNoteCompact);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 19);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(250, 146);
			this.flowLayoutPanel2.TabIndex = 8;
			// 
			// labelNoteCreate
			// 
			this.labelNoteCreate.AutoSize = true;
			this.labelNoteCreate.Location = new System.Drawing.Point(3, 0);
			this.labelNoteCreate.Name = "labelNoteCreate";
			this.labelNoteCreate.Size = new System.Drawing.Size(159, 15);
			this.labelNoteCreate.TabIndex = 1;
			this.labelNoteCreate.Text = ":setting/label/note-create";
			// 
			// inputNoteCreate
			// 
			this.inputNoteCreate.BackColor = System.Drawing.Color.White;
			this.inputNoteCreate.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteCreate.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteCreate.HotKeySetting = null;
			this.inputNoteCreate.Location = new System.Drawing.Point(3, 18);
			this.inputNoteCreate.Modifiers = PInvoke.Windows.MOD.None;
			this.inputNoteCreate.Name = "inputNoteCreate";
			this.inputNoteCreate.ReadOnly = true;
			this.inputNoteCreate.Registered = false;
			this.inputNoteCreate.Size = new System.Drawing.Size(211, 23);
			this.inputNoteCreate.TabIndex = 3;
			this.inputNoteCreate.Text = "None";
			// 
			// labelNoteHiddent
			// 
			this.labelNoteHiddent.AutoSize = true;
			this.labelNoteHiddent.Location = new System.Drawing.Point(3, 44);
			this.labelNoteHiddent.Name = "labelNoteHiddent";
			this.labelNoteHiddent.Size = new System.Drawing.Size(160, 15);
			this.labelNoteHiddent.TabIndex = 4;
			this.labelNoteHiddent.Text = ":setting/label/note-hidden";
			// 
			// inputNoteHidden
			// 
			this.inputNoteHidden.BackColor = System.Drawing.Color.White;
			this.inputNoteHidden.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteHidden.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteHidden.HotKeySetting = null;
			this.inputNoteHidden.Location = new System.Drawing.Point(3, 62);
			this.inputNoteHidden.Modifiers = PInvoke.Windows.MOD.None;
			this.inputNoteHidden.Name = "inputNoteHidden";
			this.inputNoteHidden.ReadOnly = true;
			this.inputNoteHidden.Registered = false;
			this.inputNoteHidden.Size = new System.Drawing.Size(211, 23);
			this.inputNoteHidden.TabIndex = 5;
			this.inputNoteHidden.Text = "None";
			// 
			// labelNoteCompact
			// 
			this.labelNoteCompact.AutoSize = true;
			this.labelNoteCompact.Location = new System.Drawing.Point(3, 88);
			this.labelNoteCompact.Name = "labelNoteCompact";
			this.labelNoteCompact.Size = new System.Drawing.Size(172, 15);
			this.labelNoteCompact.TabIndex = 4;
			this.labelNoteCompact.Text = ":setting/label/note-compact";
			// 
			// inputNoteCompact
			// 
			this.inputNoteCompact.BackColor = System.Drawing.Color.White;
			this.inputNoteCompact.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.inputNoteCompact.Hotkey = System.Windows.Forms.Keys.None;
			this.inputNoteCompact.HotKeySetting = null;
			this.inputNoteCompact.Location = new System.Drawing.Point(3, 106);
			this.inputNoteCompact.Modifiers = PInvoke.Windows.MOD.None;
			this.inputNoteCompact.Name = "inputNoteCompact";
			this.inputNoteCompact.ReadOnly = true;
			this.inputNoteCompact.Registered = false;
			this.inputNoteCompact.Size = new System.Drawing.Size(213, 23);
			this.inputNoteCompact.TabIndex = 6;
			this.inputNoteCompact.Text = "None";
			// 
			// commandNoteCaptionFont
			// 
			this.commandNoteCaptionFont.AutoSize = true;
			this.commandNoteCaptionFont.Location = new System.Drawing.Point(112, 184);
			this.commandNoteCaptionFont.Name = "commandNoteCaptionFont";
			this.commandNoteCaptionFont.Size = new System.Drawing.Size(152, 25);
			this.commandNoteCaptionFont.TabIndex = 10;
			this.commandNoteCaptionFont.Text = "{FAMILY} {PT} ...";
			this.commandNoteCaptionFont.UseVisualStyleBackColor = true;
			// 
			// labelNoteCaptionFont
			// 
			this.labelNoteCaptionFont.Location = new System.Drawing.Point(8, 184);
			this.labelNoteCaptionFont.Name = "labelNoteCaptionFont";
			this.labelNoteCaptionFont.Size = new System.Drawing.Size(100, 23);
			this.labelNoteCaptionFont.TabIndex = 9;
			this.labelNoteCaptionFont.Text = ":common/label/font";
			// 
			// tabSetting_pageDisplay
			// 
			this.tabSetting_pageDisplay.Location = new System.Drawing.Point(4, 24);
			this.tabSetting_pageDisplay.Name = "tabSetting_pageDisplay";
			this.tabSetting_pageDisplay.Size = new System.Drawing.Size(741, 266);
			this.tabSetting_pageDisplay.TabIndex = 5;
			this.tabSetting_pageDisplay.Text = ":setting/tab/display";
			this.tabSetting_pageDisplay.UseVisualStyleBackColor = true;
			// 
			// commandCancel
			// 
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(96, 4);
			this.commandCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(87, 29);
			this.commandCancel.TabIndex = 1;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// commandSubmit
			// 
			this.commandSubmit.Location = new System.Drawing.Point(3, 4);
			this.commandSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandSubmit.Name = "commandSubmit";
			this.commandSubmit.Size = new System.Drawing.Size(87, 29);
			this.commandSubmit.TabIndex = 0;
			this.commandSubmit.Text = "{OK}";
			this.commandSubmit.UseVisualStyleBackColor = true;
			this.commandSubmit.Click += new System.EventHandler(this.CommandSubmit_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tabSetting, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(755, 354);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(566, 305);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 37);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// SettingForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(755, 354);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = global::PeMain.Properties.Images.Pe;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/setting";
			this.tabSetting.ResumeLayout(false);
			this.tabSetting_pageMain.ResumeLayout(false);
			this.tabSetting_pageMain.PerformLayout();
			this.groupMainSystemEnv.ResumeLayout(false);
			this.groupMainSystemEnv.PerformLayout();
			this.groupMainLog.ResumeLayout(false);
			this.groupMainLog.PerformLayout();
			this.groupLogTrigger.ResumeLayout(false);
			this.groupLogTrigger.PerformLayout();
			this.tabSetting_pageLauncher.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabLauncher.ResumeLayout(false);
			this.tabLauncher_pageCommon.ResumeLayout(false);
			this.tabLauncher_pageCommon.PerformLayout();
			this.groupLauncherType.ResumeLayout(false);
			this.tabLauncher_pageEnv.ResumeLayout(false);
			this.tabLauncher_pageEnv.PerformLayout();
			this.panelEnv.Panel1.ResumeLayout(false);
			this.panelEnv.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelEnv)).EndInit();
			this.panelEnv.ResumeLayout(false);
			this.tabLauncher_pageOthers.ResumeLayout(false);
			this.tabLauncher_pageOthers.PerformLayout();
			this.tabSetting_pageToolbar.ResumeLayout(false);
			this.groupToolbar.ResumeLayout(false);
			this.groupToolbar.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputToolbarTextWidth)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolToolbarGroup.ResumeLayout(false);
			this.toolToolbarGroup.PerformLayout();
			this.tabSetting_pageCommand.ResumeLayout(false);
			this.tabSetting_pageCommand.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).EndInit();
			this.tabSetting_pageNote.ResumeLayout(false);
			this.tabSetting_pageNote.PerformLayout();
			this.groupNoteItem.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridNoteItems)).EndInit();
			this.groupNoteKey.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.DataGridViewCheckBoxColumn gridNoteItems_columnLocked;
		private System.Windows.Forms.DataGridViewCheckBoxColumn gridNoteItems_columnRemove;
		private System.Windows.Forms.DataGridViewButtonColumn gridNoteItems_columnBack;
		private System.Windows.Forms.DataGridViewButtonColumn gridNoteItems_columnFore;
		private System.Windows.Forms.DataGridViewButtonColumn gridNoteItems_columnFont;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridNoteItems_columnBody;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridNoteItems_columnTitle;
		private System.Windows.Forms.DataGridViewCheckBoxColumn gridNoteItems_columnVisible;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridNoteItems_columnId;
		private System.Windows.Forms.DataGridView gridNoteItems;
		private System.Windows.Forms.GroupBox groupNoteItem;
		private System.Windows.Forms.GroupBox groupNoteKey;
		private System.Windows.Forms.Label labelNoteCaptionFont;
		private PeMain.UI.FontSplitButton commandNoteCaptionFont;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label labelNoteCompact;
		private PeMain.UI.PeHotkeyControl inputNoteCompact;
		private System.Windows.Forms.Label labelNoteHiddent;
		private PeMain.UI.PeHotkeyControl inputNoteHidden;
		private System.Windows.Forms.Label labelNoteCreate;
		private PeMain.UI.PeHotkeyControl inputNoteCreate;
		private System.Windows.Forms.CheckBox selectLogTrigger_information;
		private System.Windows.Forms.CheckBox selectLogTrigger_warning;
		private System.Windows.Forms.CheckBox selectLogTrigger_error;
		private System.Windows.Forms.GroupBox groupLogTrigger;
		private PeMain.UI.EnvRemoveControl envLauncherRemove;
		private PeMain.UI.EnvUpdateControl envLauncherUpdate;
		private System.Windows.Forms.SplitContainer panelEnv;
		private System.Windows.Forms.CheckBox selectLauncherEnv;
		private System.Windows.Forms.CheckBox selectLauncherAdmin;
		private System.Windows.Forms.TabPage tabLauncher_pageOthers;
		private System.Windows.Forms.TabPage tabLauncher_pageEnv;
		private System.Windows.Forms.TabPage tabLauncher_pageCommon;
		private System.Windows.Forms.TabControl tabLauncher;
		private System.Windows.Forms.Label labelSystemEnvExt;
		private PeMain.UI.PeHotkeyControl inputSystemEnvHiddenFile;
		private PeMain.UI.PeHotkeyControl inputSystemEnvExt;
		private System.Windows.Forms.Label labelSystemEnvHiddenFile;
		private System.Windows.Forms.GroupBox groupMainSystemEnv;
		private System.Windows.Forms.ComboBox selectToolbarItem;
		private System.Windows.Forms.GroupBox groupToolbar;
		private System.Windows.Forms.Label labelToolbarTextWidth;
		private System.Windows.Forms.NumericUpDown inputToolbarTextWidth;
		private System.Windows.Forms.Label labelCommandHotkey;
		private PeMain.UI.PeHotkeyControl inputCommandHotkey;
		private System.Windows.Forms.CheckBox selectLogAddShow;
		private System.Windows.Forms.CheckBox selectLogVisible;
		private System.Windows.Forms.GroupBox groupMainLog;
		private System.Windows.Forms.CheckBox selectToolbarShowText;
		private System.Windows.Forms.CheckBox selectToolbarAutoHide;
		private System.Windows.Forms.CheckBox selectLauncherStdStream;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_remove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_down;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_up;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_addItem;
		private System.Windows.Forms.ToolStripButton toolToolbarGroup_addGroup;
		private System.Windows.Forms.ToolStrip toolToolbarGroup;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ComboBox selectCommandIcon;
		private System.Windows.Forms.Label labelCommandIcon;
		private System.Windows.Forms.Button commandLauncherOptionFilePath;
		private System.Windows.Forms.Button commandLauncherOptionDirPath;
		private System.Windows.Forms.TextBox inputLauncherOption;
		private System.Windows.Forms.Label labelLauncherOption;
		private System.Windows.Forms.Label labelMainLanguage;
		private System.Windows.Forms.ComboBox selectMainLanguage;
		private System.Windows.Forms.Button commandLauncherDirPath;
		private System.Windows.Forms.TextBox inputLauncherTag;
		private System.Windows.Forms.TextBox inputLauncherNote;
		private System.Windows.Forms.Label labelLauncherTag;
		private System.Windows.Forms.Label labelLauncherNote;
		private System.Windows.Forms.Button commandLauncherFilePath;
		private System.Windows.Forms.Button commandLauncherWorkDirPath;
		private System.Windows.Forms.Button commandLauncherIconPath;
		private System.Windows.Forms.TextBox inputLauncherCommand;
		private System.Windows.Forms.TextBox inputLauncherName;
		private System.Windows.Forms.TextBox inputLauncherWorkDirPath;
		private System.Windows.Forms.TextBox inputLauncherIconPath;
		private System.Windows.Forms.Label labelLauncherCommand;
		private System.Windows.Forms.Label labelLauncherName;
		private System.Windows.Forms.Label labelLauncherWorkDirPath;
		private System.Windows.Forms.Label labelLauncherIconPath;
		private System.Windows.Forms.RadioButton selectLauncherType_file;
		private System.Windows.Forms.RadioButton selectLauncherType_uri;
		private System.Windows.Forms.GroupBox groupLauncherType;
		private PeMain.UI.LauncherItemSelectControl selecterToolbar;
		private PeMain.UI.LauncherItemSelectControl selecterLauncher;
		private System.Windows.Forms.TreeView treeToolbarItemGroup;
		private System.Windows.Forms.ComboBox selectToolbarPosition;
		private System.Windows.Forms.Label labelToolbarPosition;
		private System.Windows.Forms.ComboBox selectToolbarIcon;
		private System.Windows.Forms.Label labelToolbarIcon;
		private System.Windows.Forms.CheckBox selectToolbarVisible;
		private System.Windows.Forms.Label labelToolbarFont;
		private PeMain.UI.FontSplitButton commandToolbarFont;
		private System.Windows.Forms.CheckBox selectToolbarTopmost;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabPage tabSetting_pageNote;
		private System.Windows.Forms.TabPage tabSetting_pageDisplay;
		private System.Windows.Forms.TabPage tabSetting_pageToolbar;
		private System.Windows.Forms.NumericUpDown inputCommandHideTime;
		private System.Windows.Forms.Label labelCommandHideTime;
		private System.Windows.Forms.CheckBox selectCommandTopmost;
		private PeMain.UI.FontSplitButton commandCommandFont;
		private System.Windows.Forms.Label labelCommandFont;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.TabPage tabSetting_pageCommand;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandSubmit;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TabPage tabSetting_pageLauncher;
		private System.Windows.Forms.TabPage tabSetting_pageMain;
		private System.Windows.Forms.TabControl tabSetting;
	}
}
