﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PeMain.Properties;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_functions.
	/// </summary>
	public partial class ToolbarForm
	{
		static ToolbarPosition ToToolbarPosition(DockType value)
		{
			return new Dictionary<DockType, ToolbarPosition>() {
				{ DockType.Left,   ToolbarPosition.DesktopLeft },
				{ DockType.Top,    ToolbarPosition.DesktopTop },
				{ DockType.Right,  ToolbarPosition.DesktopRight },
				{ DockType.Bottom, ToolbarPosition.DesktopBottom },
			}[value];
		}
		static DockType ToDockType(ToolbarPosition value)
		{
			return new Dictionary<ToolbarPosition, DockType>() {
				{ToolbarPosition.DesktopLeft,   DockType.Left },
				{ToolbarPosition.DesktopTop,    DockType.Top },
				{ToolbarPosition.DesktopRight,  DockType.Right },
				{ToolbarPosition.DesktopBottom, DockType.Bottom },
			}[value];
		}
		
		public static bool IsHorizonMode(ToolbarPosition pos)
		{
			return pos.IsIn(
				ToolbarPosition.DesktopFloat,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopBottom,
				ToolbarPosition.WindowTop,
				ToolbarPosition.WindowBottom
			);
		}
		
		Padding GetBorderPadding()
		{
			var frame = SystemInformation.Border3DSize;
			return new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
		}
		
		Rectangle GetCaptionArea(ToolbarPosition pos)
		{
			var padding = GetBorderPadding();
			var point = new Point(padding.Left, padding.Top);
			var size = new Size();
			
			if(IsHorizonMode(pos)) {
				size.Width = SystemInformation.SmallCaptionButtonSize.Height / 2;
				size.Height = Height - Padding.Vertical;
			} else {
				size.Width = Width - Padding.Horizontal;
				size.Height = SystemInformation.SmallCaptionButtonSize.Height / 2;
			}
			
			return new Rectangle(point, size);
		}
		
		void SetPaddingArea(ToolbarPosition pos)
		{
			var borderPadding = GetBorderPadding();
			var captionArea = GetCaptionArea(pos);
			var captionPlus = new Size();
			if(IsHorizonMode(pos)) {
				captionPlus.Width = captionArea.Width;
			} else {
				captionPlus.Height =captionArea.Height; 
			}
			var padding = new Padding(
				borderPadding.Left + captionPlus.Width,
				borderPadding.Top  + captionPlus.Height,
				borderPadding.Right,
				borderPadding.Bottom
			);
			Padding = padding;
		}

		
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			ToolbarSetting = mainSetting.Toolbar;
			LauncherSetting = mainSetting.Launcher;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(ToolbarSetting != null);
			
			ApplyLanguage();
			
			Font = ToolbarSetting.FontSetting.Font;
			if(ToolbarSetting.ToolbarGroup.Groups.Count == 0) {
				// グループが存在しなければグループを作っておく
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = Language["setting/toolbar/add-group"];
				ToolbarSetting.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
			
			// グループメニュー基盤構築
			this.menuGroup.Items.Clear();
			foreach(var groupName in ToolbarSetting.ToolbarGroup.Groups) {
				var menuItem = new ToolStripMenuItem();
				
				menuItem.Text = groupName.Name;
				menuItem.Tag = groupName;

				menuItem.Click += new EventHandler(ToolbarForm_MenuItem_Click);
				
				this.menuGroup.Items.Add(menuItem);
			}
			
			SelectedGroup(ToolbarSetting.ToolbarGroup.Groups.First());
			
			// 表示
			if(IsDockingMode) {
				DockType = ToDockType(ToolbarSetting.ToolbarPosition);
			} else {
				DockType = DockType.None;
			}
			ItemSizeToFormSize();
			Visible = ToolbarSetting.Visible;
		}
		
		/// <summary>
		/// 表示タイプからウィンドウをそれっぽいサイズに変更
		/// </summary>
		void ItemSizeToFormSize()
		{
			// TODO: これから
		}
		
		void SetToolButtons(IconSize iconSize, IEnumerable<ToolStripItem> buttons)
		{
			this.toolLauncher.ImageScalingSize = iconSize.ToSize();
			
			// アイコン解放
			this.toolLauncher.Items
				.Cast<ToolStripItem>()
				.Where(item => item.Image != null)
				.Transform(item => item.Image.Dispose())
			;
			
			this.toolLauncher.Items.Clear();
			this.toolLauncher.Items.AddRange(buttons.ToArray());
		}
		
		void SelectedGroup(ToolbarGroupItem groupItem)
		{
			var toolItem = this.menuGroup.Items
				.Cast<ToolStripMenuItem>()
				.Transform(item => item.Checked = false)
				.Single(item => (ToolbarGroupItem)item.Tag == groupItem)
			;
			
			toolItem.Checked = true;
			
			// 表示アイテム生成
			var toolButtonList = new List<ToolStripItem>();
			toolButtonList.Add(CreateLauncherButton(null));
			foreach(var itemName in groupItem.ItemNames) {
				var launcherItem = LauncherSetting.Items.SingleOrDefault(item => item.IsNameEqual(itemName));
				if(launcherItem != null) {
					var itemButton = CreateLauncherButton(launcherItem);
					toolButtonList.Add(itemButton);
				}
			}
			SetToolButtons(ToolbarSetting.IconSize, toolButtonList);
		}
		
		ToolStripItem[] CreateFileLauncherMenuItems(LauncherItem item)
		{
			var result = new List<ToolStripItem>();
			
			var executeItem = new ToolStripMenuItem();
			var executeExItem = new ToolStripMenuItem();
			var pathItem = new ToolStripMenuItem();
			var fileItem = new ToolStripMenuItem();
			result.Add(executeItem);
			result.Add(executeExItem);
			result.Add(new ToolStripSeparator());
			result.Add(pathItem);
			result.Add(fileItem);
			
			executeItem.Text = Language["toolbar/menu/file/execute"];
			executeExItem.Text = Language["toolbar/menu/file/execute-ex"];
			pathItem.Text = Language["toolbar/menu/file/path"];
			fileItem.Text = Language["toolbar/menu/file/ls"];
			
			return result.ToArray();
		}
		
		ToolStripItem CreateLauncherButton(LauncherItem item)
		{
			ToolStripDropDownItem toolItem = null;
			
			if(item == null) {
				toolItem = new ToolStripDropDownButton();
				toolItem.Text = Language["toolbar/main/text"];
				toolItem.ToolTipText = Language["toolbar/main/text"];
				toolItem.Image = PeMain.Properties.Images.ToolbarMain;
			} else {
				toolItem = new ToolStripSplitButton();
				toolItem.Text = item.Name;
				toolItem.ToolTipText = item.Name;
				toolItem.Image = item.GetIcon(ToolbarSetting.IconSize).ToBitmap();
			}
			toolItem.TextImageRelation = TextImageRelation.ImageBeforeText;
			toolItem.AutoSize = true;
			//var buttonLayout = GetButtonLayout();
			//button.Margin  = new Padding(0);
			//button.Padding = new Padding(0);
			//button.Padding = buttonLayout.Padding;
			if(ToolbarSetting.ShowText) {
				toolItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			} else {
				toolItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			}
			//button.Size = buttonLayout.ClientSize;
			toolItem.Visible = true;
			if(item != null) {
				toolItem.Tag = item;
				if(item.LauncherType == LauncherType.File) {
					toolItem.DropDownItems.AddRange(CreateFileLauncherMenuItems(item));
				}
			} else {
				
			}
			if(item != null) {
				var clickItem = (ToolStripSplitButton)toolItem;
				clickItem.ButtonClick += new EventHandler(button_ButtonClick);
			}
			//button.DropDownItemClicked += new ToolStripItemClickedEventHandler(button_DropDownItemClicked);
			
			return toolItem;
		}
		
		void ExecuteItem(LauncherItem launcherItem)
		{
			Logger.Puts(LogType.Information, "exec item", launcherItem);
		}

	}
}
