﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/31
 * 時刻: 11:07
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	partial class SettingForm
	{
		void ToolbarSelectingPage()
		{
			this.selecterToolbar.SetItems(this.selecterLauncher.Items);
			this._imageToolbarItemGroup.Images.Clear();
			var treeImage = new Dictionary<int, Bitmap>() {
				{ TREE_TYPE_NONE, PeMain.Properties.Images.NotImpl },
				{ TREE_TYPE_GROUP, PeMain.Properties.Images.Group },
			};
			this._imageToolbarItemGroup.Images.AddRange(treeImage.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray());
			
			var seq = this.selecterLauncher.Items.Select(item => new { Name = item.Name, Icon = item.GetIcon(IconScale.Small, item.IconIndex)}).Where(item => item.Icon != null);
			foreach(var elemet in seq) {
				this._imageToolbarItemGroup.Images.Add(elemet.Name, elemet.Icon);
			}
			
			// イメージリスト再設定のために一度null初期化
			this.treeToolbarItemGroup.ImageList = null;
			this.treeToolbarItemGroup.StateImageList = null;
			// イメージリスト再設定
			this.treeToolbarItemGroup.ImageList = this._imageToolbarItemGroup;
			this.treeToolbarItemGroup.StateImageList = this._imageToolbarItemGroup;
		}
		
		void ToolbarSetSelectedItem(ToolbarItem toolbarItem)
		{
			toolbarItem.ToolbarPosition = (ToolbarPosition)this.selectToolbarPosition.SelectedValue;
			toolbarItem.Topmost = this.selectToolbarTopmost.Checked;
			toolbarItem.AutoHide = this.selectToolbarAutoHide.Checked;
			toolbarItem.Visible = this.selectToolbarVisible.Checked;
			toolbarItem.ShowText = this.selectToolbarShowText.Checked;
			toolbarItem.TextWidth = (int)this.inputToolbarTextWidth.Value;
			toolbarItem.FontSetting = this.commandToolbarFont.FontSetting;
			
			toolbarItem.IconScale = (IconScale)this.selectToolbarIcon.SelectedValue;
		}
		
		void ToolbarSelectedChangeToolbarItem(ToolbarItem toolbarItem)
		{
			Debug.Assert(toolbarItem != null);
			//this._toolbarLocation = toolbarSetting.FloatLocation;
			//this._toolbarSize = toolbarSetting.FloatSize;
			
			this.selectToolbarPosition.SelectedValue = toolbarItem.ToolbarPosition;
			this.selectToolbarIcon.SelectedValue = toolbarItem.IconScale;
			this.commandToolbarFont.FontSetting.Import(toolbarItem.FontSetting);
			this.commandToolbarFont.RefreshView();
			
			this.inputToolbarTextWidth.Value = toolbarItem.TextWidth;
			
			// 各ON/OFF
			this.selectToolbarAutoHide.Checked = toolbarItem.AutoHide;
			this.selectToolbarVisible.Checked = toolbarItem.Visible;
			this.selectToolbarTopmost.Checked = toolbarItem.Topmost;
			this.selectToolbarShowText.Checked = toolbarItem.ShowText;
			
			this._toolbarSelectedToolbarItem = toolbarItem;
		}
		
		TreeNode ToolbarAddGroup(string groupName)
		{
			var node = new TreeNode();
			node.Text = groupName;
			node.ImageIndex = TREE_TYPE_GROUP;
			node.SelectedImageIndex = TREE_TYPE_GROUP;
			this.treeToolbarItemGroup.Nodes.Add(node);
			
			return node;
		}
		
		void ToolbarSetItem(TreeNode node, LauncherItem item)
		{
			Debug.Assert(node != null);
			Debug.Assert(item != null);
			
			node.Text = item.Name;
			//if(this._imageToolbarItemGroup.Images.ContainsKey(item.Name)) {
				node.ImageKey = item.Name;
				node.SelectedImageKey = item.Name;
			//} else {
			//	node.ImageIndex = TREE_TYPE_NONE;
			//	node.SelectedImageIndex = TREE_TYPE_NONE;
			//}
			node.Tag = item;
		}
		
		void ToolbarAddItem(TreeNode parentNode, LauncherItem item)
		{
			Debug.Assert(parentNode != null);
			/*
			var items = this.selecterToolbar.Items;
			if(items != null && items.Count() > 0) {
				var item = this.selecterToolbar.SelectedItem;
				if(item == null) {
					item = items.First();
				}
				var node = new TreeNode();
				ToolbarSetItem(node, item);
				parentNode.Nodes.Add(node);
				if(!parentNode.IsExpanded) {
					parentNode.Expand();
				}
			}
			*/
			var node = new TreeNode();
			ToolbarSetItem(node, item);
			parentNode.Nodes.Add(node);
			if(!parentNode.IsExpanded) {
				parentNode.Expand();
			}
		}
		
		void ToolbarSelectedChangeGroupItem(LauncherItem item)
		{
			Debug.Assert(item != null);
			var showItem = this.selecterToolbar.ViewItems.Any(i => i == item);
			if(!showItem) {
				this.selecterToolbar.Filtering = false;
			}
			this.selecterToolbar.SelectedItem = item;
		}
		
		void ToolbarExportSetting(ToolbarSetting toolbarSetting)
		{
			ToolbarSetSelectedItem(this._toolbarSelectedToolbarItem);
			foreach(var itemData in this.selectToolbarItem.Items.Cast<ToolbarDisplayValue>()) {
				var item = itemData.Value;
				if(toolbarSetting.Items.Contains(item)) {
					toolbarSetting.Items.Remove(item);
				}
				toolbarSetting.Items.Add(item);
			}
			
				
			
			// ツリーからグループ項目構築
			foreach(TreeNode groupNode in this.treeToolbarItemGroup.Nodes) {
				var toolbarGroupItem = new ToolbarGroupItem();
				
				// グループ項目
				var groupName = groupNode.Text;
				toolbarGroupItem.Name = groupName;
				
				// グループに紐付くアイテム名
				toolbarGroupItem.ItemNames.AddRange(groupNode.Nodes.Cast<TreeNode>().Select(node => node.Text));

				toolbarSetting.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
		}
	}
}
