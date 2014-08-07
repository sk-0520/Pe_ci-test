﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of NoteForm.
	/// </summary>
	public partial class NoteForm : Form, ISetCommonData
	{
		public NoteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		protected override CreateParams CreateParams 
		{
			get 
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassStyle |= (int)CS.CS_DROPSHADOW;
				return createParams;
			}
		}
		
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);
		}
		
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}
		
		void NoteForm_Paint(object sender, PaintEventArgs e)
		{
			using(var bmp = new Bitmap(Width, Height, e.Graphics)) {
				using(var memG = Graphics.FromImage(bmp)) {
					var rect = new Rectangle(Point.Empty, Size);
					DrawFull(memG, rect, this == Form.ActiveForm);
					e.Graphics.DrawImage(bmp, 0, 0);
				}
			}
		}
		
		void NoteForm_Activated(object sender, EventArgs e)
		{
			DrawFullActivaChanged(true);
		}
		
		void NoteForm_Deactivate(object sender, EventArgs e)
		{
			if(this._initialized) {
				HiddenInputTitleArea();
				HiddenInputBodyArea();
			}
			
			
			DrawFullActivaChanged(false);
			
			SaveItem();
		}
		
		
		void NoteForm_MouseDown(object sender, MouseEventArgs e)
		{
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					var left = (MouseButtons & MouseButtons.Left) == MouseButtons.Left;
					if(left && isIn) {
						return ButtonState.Pressed;
					} else {
						return ButtonState.Normal;
					}
				},
				null,
				null,
				false
			);
		}
		
		void NoteForm_MouseUp(object sender, MouseEventArgs e)
		{
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					var left = (MouseButtons & MouseButtons.Left) == MouseButtons.Left;
					if(left && isIn) {
						return ButtonState.Selected;
					} else {
						return ButtonState.Normal;
					}
				},
				command => {
					if(this._commandStateMap[command] == ButtonState.Pressed) {
						ExecCommand(command);
					}
				},
				null,
				true
			);
		}
		
		void NoteForm_DoubleClick(object sender, EventArgs e)
		{
			ShowInputBodyArea();
		}
		
		void NoteForm_Resize(object sender, EventArgs e)
		{
			if(!this._initialized && NoteItem.Compact) {
				//ChangeCompact(true, Size.Empty);
				Height = 20;
			} else {
				ResizeInputTitleArea();
				ResizeInputBodyArea();
				
				if(!NoteItem.Compact) {
					NoteItem.Size = Size;
					Changed = true;
				}
			}
		}
		
		void NoteForm_Move(object sender, EventArgs e)
		{
			NoteItem.Location = Location;
			Changed = true;
		}
		
		void Input_Leave(object sender, EventArgs e)
		{
			HiddenInputTitleArea();
			HiddenInputBodyArea();
		}
		
		void ContextMenu_title_Click(object sender, EventArgs e)
		{
			ShowInputTitleArea();
		}
		
		void ContextMenu_body_Click(object sender, EventArgs e)
		{
			ShowInputBodyArea();
		}

		
		
		void NoteForm_Load(object sender, EventArgs e)
		{
			// 生成前の高さがWindowsにより補正されるためここでリサイズ
			ChangeCompact(NoteItem.Compact, NoteItem.Size);
		}
	}
}