﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public class HitState
	{
		#region define

		const uint leftBit = 0x0001;
		const uint rightBit = 0x0002;
		const uint topBit = 0x0004;
		const uint bottomBit = 0x0008;

		#endregion

		#region variable

		uint _flag;

		#endregion

		#region property

		public bool IsEnabled { get { return this._flag != 0; } }

		public bool Left
		{
			get { return Get(leftBit); }
			set { Set(leftBit, value); }
		}
		public bool Right
		{
			get { return Get(rightBit); }
			set { Set(rightBit, value); }
		}
		public bool Top
		{
			get { return Get(topBit); }
			set { Set(topBit, value); }
		}
		public bool Bottom
		{
			get { return Get(bottomBit); }
			set { Set(bottomBit, value); }
		}

		#endregion

		#region function

		private bool Get(uint bit)
		{
			return (this._flag & bit) == bit;
		}

		private void Set(uint bit, bool value)
		{
			if (value) {
				this._flag |= bit;
			} else {
				this._flag &= ~(this._flag & bit);
			}
		}

		/// <summary>
		/// 領域とパディング、カーソル位置から各種値の計算と設定。
		/// </summary>
		/// <param name="area">全体領域。</param>
		/// <param name="thickness">パディング領域。</param>
		/// <param name="point">判定座標。</param>
		public void CalculateAndSetValue(Rect area, Thickness thickness, Point point)
		{
			var workArea = new Rect();

			// 上
			workArea = area;
			workArea.Height = thickness.Top;
			Top = workArea.Contains(point);
			// 下
			workArea = area;
			workArea.Y = area.Height - thickness.Bottom;
			workArea.Height = thickness.Bottom;
			Bottom = workArea.Contains(point);
			// 左
			workArea = area;
			workArea.Width = thickness.Left;
			Left = workArea.Contains(point);
			// 右
			workArea = area;
			workArea.X = area.Width - thickness.Right;
			workArea.Width = thickness.Right;
			Right = workArea.Contains(point);
		}

		#endregion
	}
}
