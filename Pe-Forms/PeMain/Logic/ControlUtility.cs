﻿namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	/// <summary>
	/// コモンダイアログ共通処理。
	/// </summary>
	public static class DialogUtility
	{
		/// <summary>
		/// コントロールに対してファイルダイアログを適用する。
		/// </summary>
		/// <param name="input"></param>
		/// <param name="whitespaceIsQuotation"></param>
		public static void OpenDialogWithFilePath(Control input, bool whitespaceIsQuotation = true)
		{
			var path = input.Text.Trim();
			using(var dialog = new OpenFileDialog()) {
				if(path.Length > 0 && File.Exists(path)) {
					dialog.InitialDirectory = Path.GetDirectoryName(path);
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					var filePath = dialog.FileName;
					if(whitespaceIsQuotation) {
						filePath = (new []{filePath}).WhitespaceToQuotation().First();
					}
					input.Text = filePath;
				}
			}
		}
		
		/// <summary>
		/// コントロールに対してフォルダダイアログを適用する。
		/// </summary>
		/// <param name="input"></param>
		public static void OpenDialogWithDirectoryPath(Control input)
		{
			var path = input.Text.Trim();
			using(var dialog = new FolderBrowserDialog()) {
				dialog.ShowNewFolderButton = true;
				
				if(path.Length > 0 && Directory.Exists(path)) {
					dialog.SelectedPath = path;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					input.Text = dialog.SelectedPath;
				}
			}
		}

		/// <summary>
		/// フォントダイアログにFontSettingを適用する。
		/// </summary>
		/// <param name="dialog"></param>
		/// <param name="fontSetting"></param>
		public static void SetFontSetting(this FontDialog dialog, FontSetting fontSetting)
		{
			if(!fontSetting.IsDefault) {
				dialog.Font = fontSetting.Font;
			} else {
				dialog.Font = fontSetting.DefaultFont;
			}
		}
	}
	
	/// <summary>
	/// ツリービュー共通処理。
	/// </summary>
	public static class TreeViewUtility
	{
		/// <summary>
		/// ツリービューから全ノードを取得する。
		/// </summary>
		/// <param name="treeView"></param>
		/// <returns></returns>
		public static IList<TreeNode> GetChildrenNodes(this TreeView treeView)
		{
			var result = new List<TreeNode>();
			foreach(TreeNode node in treeView.Nodes) {
				result.Add(node);
				var nodes = node.GetChildrenNodes();
				if(nodes.Count > 0) {
					result.AddRange(nodes);
				}
			}
			
			return result;
		}
	}
	
	/// <summary>
	/// ツリーノード共通処理。
	/// </summary>
	public static class TreeNodeUtility
	{
		/// <summary>
		/// 対象の子ノードをすべて取得
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static IList<TreeNode> GetChildrenNodes(this TreeNode parent)
		{
			var result = new List<TreeNode>();
			
			foreach(TreeNode node in parent.Nodes) {
				result.Add(node);
				if(node.Nodes.Count > 0) {
					var list = GetChildrenNodes(node);
					if(list.Count > 0) {
						result.AddRange(list);
					}
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// ノード選択。
		/// 
		/// ユーザーコードでは基本的に使用しない。
		/// </summary>
		/// <param name="node"></param>
		/// <param name="toSelect"></param>
		public static void ToSelect(this TreeNode node, bool toSelect)
		{
			if(toSelect) {
				var view = node.TreeView;
				view.SelectedNode = node;
			}
		}
		
		/// <summary>
		/// 対象ノードを指定ノードの子にする
		/// </summary>
		/// <param name="fromNode"></param>
		/// <param name="toParent">親となるノード</param>
		/// <param name="toSelect"></param>
		public static void MoveToChild(this TreeNode fromNode, TreeNode toParent, bool toSelect)
		{
			Debug.Assert(fromNode != null);
			Debug.Assert(toParent != null);
			
			var tree = fromNode.TreeView;
			fromNode.Remove();
			toParent.Nodes.Add(fromNode);
			toParent.Expand();
			ToSelect(fromNode, toSelect);
		}
		
		/// <summary>
		/// 対象ノードを親ノード位置へ移動する
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToOut(this TreeNode targetNode, bool toSelect)
		{
			var parentNode = targetNode.Parent;
			var superParentNode = parentNode.Parent;
			if(parentNode == null) {
				return;
			}
			targetNode.Remove();
			superParentNode.Nodes.Insert(parentNode.Index + 1, targetNode);
			superParentNode.Expand();
			parentNode.Expand();
			ToSelect(targetNode, toSelect);
		}
		
		/// <summary>
		/// 上の兄弟を親ノードとする
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToIn(this TreeNode targetNode, bool toSelect)
		{
			var prevNode = targetNode.PrevNode;
			targetNode.Remove();
			prevNode.Nodes.Add(targetNode);
			prevNode.Expand();
			ToSelect(targetNode, toSelect);
		}

		/// <summary>
		/// 兄弟要素間で上に移動する。
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToUp(this TreeNode targetNode, bool toSelect)
		{
			var parentNode = targetNode.Parent;
			var prevNode = targetNode.PrevNode;
			var nodes = parentNode != null ? parentNode.Nodes: targetNode.TreeView.Nodes;
			if(prevNode == null) {
				return;
			}
			targetNode.Remove();
			nodes.Insert(prevNode.Index, targetNode);
			var tree = targetNode.TreeView;
			ToSelect(targetNode, toSelect);
		}
		
		/// <summary>
		/// 兄弟要素間で下に移動する。
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="toSelect"></param>
		public static void MoveToDown(this TreeNode targetNode, bool toSelect)
		{
			var parentNode = targetNode.Parent;
			var nextNode = targetNode.NextNode;
			var nodes = parentNode != null ? parentNode.Nodes: targetNode.TreeView.Nodes;
			if(nextNode == null) {
				return;
			}
			targetNode.Remove();
			nodes.Insert(nextNode.Index + 1, targetNode);
			var tree = targetNode.TreeView;
			ToSelect(targetNode, toSelect);
		}
	}
	
	/// <summary>
	/// ツールストリップ共通処理
	/// </summary>
	public static class ToolStripUtility
	{
		/// <summary>
		/// コンテキストメニュー用位置補正イベント。
		/// 
		/// ユーザーコードでは使用しない。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void EventDropDownOpeningMenuInScreen(object sender, object e)
		{
			var toolMenu = sender as ToolStripDropDown;
			if(toolMenu == null) {
				return;
			}
			var parent = (Control)toolMenu.Tag;
			//Debug.WriteLine(toolMenu.Location);
			var showScreen = Screen.FromPoint(toolMenu.Location);
			var parentScreen = Screen.FromControl(parent);
			
			if(toolMenu.HasChildren) {
				foreach(ToolStripItem childItem in toolMenu.Items) {
					var childDropItem = childItem as ToolStripDropDownItem;
					if(childDropItem != null) {
						AttachmentOpeningMenuInScreen(childDropItem);
					}
				}
			}

			if(showScreen.DeviceName != parentScreen.DeviceName) {
				if(parentScreen.Bounds.Left - showScreen.Bounds.Left < 0) {
					parent.ContextMenuStrip.Opening -= EventDropDownOpeningMenuInScreen;
					parent.ContextMenuStrip.Close();
					parent.ContextMenuStrip.Show(parent, parent.PointToClient(Cursor.Position), ToolStripDropDownDirection.Left);
					parent.ContextMenuStrip.Opening += EventDropDownOpeningMenuInScreen;
				}
			}
		}
		
		/// <summary>
		/// メニュー用位置補正イベント。
		/// 
		/// ユーザーコードでは使用しない。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void EventDropDownItemOpeningMenuInScreen(object sender, object e)
		{
			var toolMenu = sender as ToolStripDropDownItem;
			if(toolMenu == null) {
				return;
			}
			
			ToolStripItem owner = toolMenu;
			//owner.Bounds;
			while(owner.OwnerItem != null) {
				owner = owner.OwnerItem;
			}

			var showScreen = Screen.FromPoint(toolMenu.DropDown.Location);
			Screen parentScreen;
			if(owner.Owner != null) {
				parentScreen = Screen.FromControl(owner.Owner);
			} else {
				parentScreen = Screen.FromControl(toolMenu.Owner);
			}
			// #34, とりあえず右側だけ対処で行ける気がする
			//Debug.WriteLine("{0} > {1}:{2} - {3}:{4}", DateTime.Now, parentScreen.DeviceName, owner.Owner.Location, showScreen.DeviceName, toolMenu.DropDown.Location);
			if(showScreen.DeviceName != parentScreen.DeviceName) {
				if(parentScreen.Bounds.Left - showScreen.Bounds.Left < 0) {
					toolMenu.DropDownOpening -= EventDropDownItemOpeningMenuInScreen;
					toolMenu.DropDown.Close();
					toolMenu.DropDownDirection = ToolStripDropDownDirection.Left;
					toolMenu.ShowDropDown();
					toolMenu.DropDownOpening += EventDropDownItemOpeningMenuInScreen;
				}
			}
		}

		/// <summary>
		/// 位置補正イベントのアタッチ。
		/// </summary>
		public static void AttachmentOpeningMenuInScreen(IEnumerable<ToolStripItem> toolItems)
		{
			foreach(var toolItem in toolItems.Select(t => t as ToolStripDropDownItem).Where(t => t != null)) {
				AttachmentOpeningMenuInScreen(toolItem);
			}
		}
		/// <summary>
		/// 位置補正イベントのアタッチ。
		/// </summary>
		public static void AttachmentOpeningMenuInScreen(ToolStripDropDownItem toolItem)
		{
			/*
			toolItem.DropDown.MouseWheel -= EventDropDownMouseWheel;
			toolItem.DropDown.MouseWheel += EventDropDownMouseWheel;
			 */
			
			// HACK: 一体何をどう信じればいいのか。。。
			toolItem.DropDownOpening -= EventDropDownItemOpeningMenuInScreen;
			toolItem.DropDownOpening += EventDropDownItemOpeningMenuInScreen;
			toolItem.DropDownOpened -= EventDropDownItemOpeningMenuInScreen;
			toolItem.DropDownOpened += EventDropDownItemOpeningMenuInScreen;
			
			if(toolItem.HasDropDownItems) {
				foreach(ToolStripItem childItem in toolItem.DropDownItems) {
					var childDropItem = childItem as ToolStripDropDownItem;
					if(childDropItem != null) {
						AttachmentOpeningMenuInScreen(childDropItem);
					}
				}
			}
		}
		
		/// <summary>
		/// コンテキストメニュー用位置補正イベントのアタッチ。
		/// </summary>
		/// <param name="parent">コンテキストメニューを持つ親コントロール</param>
		public static void AttachmentOpeningMenuInScreen(Control parent)
		{
			var toolItem = parent.ContextMenuStrip;
			Debug.Assert(toolItem.Tag == null);
			/*
			toolItem.MouseWheel -= EventDropDownMouseWheel;
			toolItem.MouseWheel += EventDropDownMouseWheel;
			 */
			toolItem.Tag = parent;
			toolItem.Opening -= EventDropDownOpeningMenuInScreen;
			toolItem.Opening += EventDropDownOpeningMenuInScreen;
		}

		/// <summary>
		/// 安全にショートカットキーを設定する。
		/// </summary>
		/// <param name="toolItem"></param>
		/// <param name="key"></param>
		/// <param name="logger"></param>
		static void SetSafeShortcutKeys(ToolStripMenuItem toolItem, Keys key, ILogger logger) 
		{
			try {
				toolItem.ShortcutKeys = key;
			} catch(InvalidEnumArgumentException ex) {
				logger.Puts(LogType.Warning, key.ToString(), ex);
			}
		}

		public static void SetSafeShortcutKeysAndDisplayKey(ToolStripMenuItem toolItem, HotKeySetting hotkeySetting, Language language, ILogger logger)
		{
			SetSafeShortcutKeys(toolItem, hotkeySetting.GetShorcutKey(), logger);
			if(hotkeySetting.Enabled) {
				toolItem.ShortcutKeyDisplayString = LanguageUtility.HotkeySettingToDisplayText(language, hotkeySetting);
			}
		}
		
	}
	
	/// <summary>
	/// WebBrowser共通処理。
	/// </summary>
	public static class WebBrowserUtility
	{
		static void EventNewWindow(object sender, CancelEventArgs e)
		{
			var webBrowser = (WebBrowser)sender;
			var link = webBrowser.StatusText;
			e.Cancel = true;
			Executor.RunCommand(link, null);
		}

		/// <summary>
		/// リンククリック時に外部ブラウザで開く。
		/// </summary>
		/// <param name="target"></param>
		public static void AttachmentNewWindow(WebBrowser target)
		{
			target.NewWindow += EventNewWindow;
		}
	}

	/// <summary>
	/// ユーザーインターフェイス共通処理。
	/// </summary>
	public static class UIUtility
	{
		private static bool CanReplaceWord(string s)
		{
			return !(string.IsNullOrWhiteSpace(s) || s.Length > 0 && s[0] != ':');
		}
		private static string GetWord(Language language, string key, IDictionary<string, string> map)
		{
			/*
			if(string.IsNullOrEmpty(key) || key[0] != ':') {
				return "{" + key + "}";
			}
			 */
			if(CanReplaceWord(key)) {
				return language[key.Substring(1), map];
			}
			
			return "{" + key + "}";
		}
		
		public static void SetDefaultText(Form target, Language language, IDictionary<string, string> map = null)
		{
			target.SetLanguage(language, map);
#if DEBUG || BETA
			target.Text = "(" + Literal.BuildType + ") " + target.Text;
#endif

			var acceptButton = target.AcceptButton as Button;
			if(acceptButton != null) {
				acceptButton.Text = language["common/command/ok"];
			}
			
			var cancelButton = target.CancelButton as Button;
			if(cancelButton != null) {
				cancelButton.Text = language["common/command/cancel"];
			}
		}

		public static Font GetSystemDialogFont()
		{
			var baseFont = SystemFonts.MessageBoxFont;
			//return new Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			var family = FontFamily.Families.FirstOrDefault(f => f.Name == "Meiryo UI");
			if(family != null) {
				baseFont = new Font(family, baseFont.SizeInPoints, baseFont.Style, GraphicsUnit.Point);
			}
			return baseFont;
		}

		public static void InitializeWindow(Form form)
		{
			form.SuspendLayout();

			form.Font = GetSystemDialogFont();

			form.ResumeLayout(false);
		}

		/// <summary>
		/// NOTE: 絶対的に場所が違う。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="language"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		public static string GetLanguage(string target, Language language, IDictionary<string, string> map = null)
		{
			return GetWord(language, target, map);
		}
		
		public static void SetLanguage(this Control target, Language language, IDictionary<string, string> map = null)
		{
			target.Text = GetWord(language, target.Text, map);
		}
		
		
		public static void SetLanguage(this DataGridViewColumn target, Language language, IDictionary<string, string> map = null)
		{
			target.HeaderText = GetWord(language, target.HeaderText, map);
		}
		
		
		public static void SetLanguage(this ColumnHeader target, Language language, IDictionary<string, string> map = null)
		{
			target.Text = GetWord(language, target.Text, map);
		}
		
		
		public static void SetLanguage(this ToolStripItem target, Language language, IDictionary<string, string> map = null)
		{
			if(!string.IsNullOrEmpty(target.ToolTipText) && target.Text != target.ToolTipText) {
				target.ToolTipText = GetWord(language, target.ToolTipText, map);
			}
			target.Text = GetWord(language, target.Text, map);
		}
		/*
		public static void SetLanguage(this ToolStrip target, Language language, IDictionary<string, string> map = null)
		{
			target.Text = GetWord(language, target.Text, map);
		}
		 */
		
		/// <summary>
		/// 指定ウィンドウを前面へ表示する。
		/// 
		/// 最前面への固定ではなく前面に移動させるだけであることに注意。
		/// </summary>
		/// <param name="target"></param>
		public static void ShowFront(Form target)
		{
			var topmost = target.TopMost;
			target.TopMost = true;
			NativeMethods.SetWindowPos(target.Handle, (IntPtr)HWND.HWND_TOP, 0, 0, 0, 0, SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
			target.TopMost = topmost;
		}
		
		public static void ShowFrontActive(Form target)
		{
			ShowFront(target);
			target.Activate();
		}
		
		/// <summary>
		/// プライマリウィンドウの中央に移動させる。
		/// </summary>
		/// <param name="target"></param>
		public static void ShowCenterInPrimaryScreen(Form target)
		{
			var primaryArea = Screen.PrimaryScreen.Bounds;
			target.Location = new Point(
				primaryArea.Width / 2 - target.Width / 2,
				primaryArea.Height / 2 - target.Height / 2
			);
		}

		/// <summary>
		/// AutoSizeの設定された項目をうまい具合に再設定する
		/// </summary>
		/// <param name="control"></param>
		/// <param name="recursive"></param>
		public static void ResizeAutoSize(Control target, bool recursive)
		{
			if(recursive && target.HasChildren) {
				foreach(var control in target.Controls.Cast<Control>()) {
					ResizeAutoSize(control, recursive);
				}
			}
			if(target.AutoSize) {
				target.Size = Size.Empty;
			}
		}

	}

	/// <summary>
	/// 突貫工事。
	/// </summary>
	public static class UpDownUtility
	{
		static void SetRange(this NumericUpDown target, decimal min, decimal max)
		{
			target.Minimum = min;
			target.Maximum = max;
		}

		public static void SetRange(this NumericUpDown target, TripleRange<decimal> range)
		{
			SetRange(target, range.minimum, range.maximum);
			var defaultValueUpDown = target as DefaultValueNumericUpDown;
			if(defaultValueUpDown != null) {
				defaultValueUpDown.DefaultValue = range.median;
			}
		}
		public static void SetRange(this NumericUpDown target, TripleRange<int> range)
		{
			var convertedRange = new TripleRange<decimal>(
				range.minimum,
				range.median,
				range.maximum
			);
			SetRange(target, convertedRange);
		}

		public static void SetValue(this NumericUpDown target, TripleRange<decimal> range, decimal value)
		{
			SetRange(target, range);
			target.Value = value;
		}
		public static void SetValue(this NumericUpDown target, TripleRange<int> range, int value)
		{
			var convertedRange = new TripleRange<decimal>(
				range.minimum,
				range.median,
				range.maximum
			);
			SetValue(target, convertedRange, (decimal)value);
		}

		public static void SetValue(this NumericUpDown target, TripleRange<TimeSpan> range, TimeSpan value)
		{
			var msRange = new TripleRange<decimal>(
				(decimal)range.minimum.TotalMilliseconds,
				(decimal)range.median.TotalMilliseconds,
				(decimal)range.maximum.TotalMilliseconds
			);
			SetValue(target, msRange, (decimal)value.TotalMilliseconds);
		}
	}
}
