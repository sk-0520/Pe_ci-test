﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/07
 * 時刻: 21:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using PeMain.Data;
using PeUtility;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SystemSkin.
	/// </summary>
	public class SystemSkin: Skin
	{
		Color VisualColor { get; set;}
		private void SetVisualStyle(Form target)
		{
			Debug.Assert(EnabledVisualStyle);
			
			/*
			var margin = new MARGINS();
			margin.leftWidth = -1;
			API.DwmExtendFrameIntoClientArea(target.Handle, ref margin);
			*/
			var blurHehind = new DWM_BLURBEHIND();
			blurHehind.fEnable = true;
			blurHehind.hRgnBlur = IntPtr.Zero;
			blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE | DWM_BB.DWM_BB_BLURREGION;
			API.DwmEnableBlurBehindWindow(target.Handle, ref blurHehind);
			
			// 設定色を取得
			uint rawColor;
			bool blend;
			API.DwmGetColorizationColor(out rawColor, out blend);
			VisualColor = Color.FromArgb(Convert.ToInt32(rawColor));
			//target.BackColor = VisualColor;
		}
		
		public override void Start(Form target)
		{
			base.Start(target);
			
			if(EnabledVisualStyle) {
				SetVisualStyle(target);
			}
		}
		
		public override void Close(Form target)
		{
			if(EnabledVisualStyle) {
				var margin = new MARGINS();
				margin.leftWidth = 0;
				margin.rightWidth = 0;
				margin.topHeight = 0;
				margin.bottomHeight = 0;
				API.DwmExtendFrameIntoClientArea(target.Handle, ref margin);
			}
		}
		
		public override Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition)
		{
			var frame = SystemInformation.Border3DSize;
			return new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
		}
		
		public override Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize)
		{
			var padding = GetToolbarBorderPadding(toolbarPosition);
			var point = new Point(padding.Left, padding.Top);
			var size = new Size();
			
			if(ToolbarPositionUtility.IsHorizonMode(toolbarPosition)) {
				size.Width = SystemInformation.SmallCaptionButtonSize.Height / 2;
				size.Height = parentSize.Height - padding.Vertical;
			} else {
				size.Width = parentSize.Width - padding.Horizontal;
				size.Height = SystemInformation.SmallCaptionButtonSize.Height / 2;
			}
			
			return new Rectangle(point, size);
		}
		
		public override Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize)
		{
			var borderPadding = GetToolbarBorderPadding(toolbarPosition);
			var captionArea = GetToolbarCaptionArea(toolbarPosition, parentSize);
			var captionPlus = new System.Drawing.Size();
			if(ToolbarPositionUtility.IsHorizonMode(toolbarPosition)) {
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
			
			return padding;
		}
		
		public override SkinToolbarButtonLayout GetToolbarButtonLayout(IconSize iconSize, bool showText, int textWidth)
		{
			var iconBox = iconSize.ToSize();
			var systemBorderSize = SystemInformation.Border3DSize;
			var systemPaddingSize = SystemInformation.FixedFrameBorderSize;
			var padding = new Padding(
				systemBorderSize.Width + systemPaddingSize.Width / 2,
				systemBorderSize.Height + systemPaddingSize.Height / 2,
				systemBorderSize.Width + systemPaddingSize.Width / 2,
				systemBorderSize.Height + systemPaddingSize.Height / 2
			);
			var buttonSize = new Size();
			var menuWidth = 12;
			
			buttonSize.Width = iconBox.Width + padding.Right + padding.Horizontal + menuWidth;
			if(showText) {
				buttonSize.Width += textWidth > 0 ? textWidth: Literal.toolbarTextWidth;
			}
			buttonSize.Height = iconBox.Height + padding.Vertical;
			
			var buttonLayout = new SkinToolbarButtonLayout();
			buttonLayout.Size = buttonSize;
			buttonLayout.Padding = padding;
			buttonLayout.MenuWidth = menuWidth;
			return buttonLayout;
		}

		
		public override void DrawToolbarEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolPosition)
		{
			var borderPadding = GetToolbarBorderPadding(toolPosition);
			//
			var light = active ? SystemBrushes.ControlLight: SystemBrushes.ControlLightLight;
			var dark = active ? SystemBrushes.ControlDarkDark: SystemBrushes.ControlDark;
			
			// 下
			g.FillRectangle(dark, 0, drawArea.Height - borderPadding.Bottom, drawArea.Width, borderPadding.Bottom);
			// 右
			g.FillRectangle(dark, drawArea.Width - borderPadding.Right, 0, borderPadding.Right, drawArea.Height);
			// 左
			g.FillRectangle(dark, 0, 0, borderPadding.Left, drawArea.Height);
			// 上
			g.FillRectangle(dark, 0, 0, drawArea.Width, borderPadding.Top);
		}

		
		public override void DrawToolbarCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
			Color headColor;
			Color tailColor;
			if(active) {
				headColor = SystemColors.GradientActiveCaption;
				tailColor = SystemColors.ActiveCaption;
			} else {
				headColor = SystemColors.GradientInactiveCaption;
				tailColor = SystemColors.InactiveCaption;
			}
			var mode = ToolbarPositionUtility.IsHorizonMode(toolbarPosition) ? LinearGradientMode.Vertical: LinearGradientMode.Horizontal;
			using(var brush = new LinearGradientBrush(drawArea, headColor, tailColor, mode)) {
				g.FillRectangle(brush, drawArea);
			}
		}
		
				
		public override void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition position)
		{
			e.Graphics.Clear(VisualColor);
		}
		
		public override void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition position)
		{
			//e.Graphics.FillRectangle(SystemBrushes.Desktop, e.ConnectedArea);
		}
		
		public override void DrawToolbarArrow(ToolStripArrowRenderEventArgs e)
		{
			if(e.Item.Pressed) {
				// 押されている
				e.Graphics.FillRectangle(SystemBrushes.ControlLightLight, e.ArrowRectangle);
			} else if(e.Item.Selected) {
				// 選ばれている
				e.Graphics.FillRectangle(SystemBrushes.ActiveCaptionText, e.ArrowRectangle);
			} else {
				// 通常
			}
		}
		
		
		public override void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem)
		{
			var buttonLayout = GetToolbarButtonLayout(toolbarItem.IconSize, false, 0);
			var iconSize = toolbarItem.IconSize.ToSize();
			e.Graphics.DrawImage(e.Image, buttonLayout.Padding.Left, buttonLayout.Padding.Top, iconSize.Width, iconSize.Height);
		}
		
		public override void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem)
		{
			using(var brush = new SolidBrush(Color.FromArgb(254, e.TextColor))) {
				using(var format = ToStringFormat(e.TextFormat)) {
					format.LineAlignment = StringAlignment.Center;
					var buttonLayout = GetToolbarButtonLayout(toolbarItem.IconSize, toolbarItem.ShowText, toolbarItem.TextWidth);
					var iconSize = toolbarItem.IconSize.ToSize();
					var textArea = new Rectangle(
						buttonLayout.Padding.Vertical + iconSize.Width,
						buttonLayout.Padding.Top,
						buttonLayout.Size.Width - iconSize.Width - buttonLayout.Padding.Right - buttonLayout.Padding.Horizontal - buttonLayout.MenuWidth,
						buttonLayout.Size.Height - buttonLayout.Padding.Vertical
					);
					e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
					e.Graphics.DrawString(e.Text, e.TextFont, brush, textArea, format);
				}
			}
		}
		
		public override void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active, Rectangle itemArea)
		{
			if(e.Item.Pressed) {
				// 押されている
				e.Graphics.FillRectangle(SystemBrushes.ButtonHighlight, itemArea);
			} else if(item.Selected) {
				// 選ばれている
				e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, itemArea);
			} else {
				// 通常
			}
		}
		
		public override void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active, Rectangle itemArea)
		{
			if(e.Item.Pressed) {
				// 押されている
				e.Graphics.FillRectangle(SystemBrushes.ButtonHighlight, itemArea);
			} else if(item.Selected) {
				// 選ばれている
				e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, itemArea);
			} else {
				// 通常
			}
		}
		
		
		public override bool IsDefaultDrawToolbarBackground { get { return false; } }
		public override bool IsDefaultDrawToolbarBorder { get { return false; } }
		public override bool IsDefaultDrawToolbarArrow { get { return false; } }
		public override bool IsDefaultDrawToolbarButtonImage { get { return false; } }
		public override bool IsDefaultDrawToolbarButtonText { get { return false; } }
		public override bool IsDefaultDrawToolbarDropDownButtonBackground { get { return false; } }
		public override bool IsDefaultDrawToolbarSplitButtonBackground { get { return false; } }
		


	}
}