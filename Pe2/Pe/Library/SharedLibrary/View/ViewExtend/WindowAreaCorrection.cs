﻿namespace ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using System.Runtime.InteropServices;

	public class WindowAreaCorrection : WindowsViewExtendBase<IWindowAreaCorrectionData>
	{
		public WindowAreaCorrection(Window view, IWindowAreaCorrectionData restrictionViewModel, INonProcess nonProcess)
			: base(view, restrictionViewModel, nonProcess)
		{ }

		#region WindowsViewExtendBase

		public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (RestrictionViewModel.UsingMultipleResize) {
				CorrectionSizing(hWnd, msg, wParam, lParam, ref handled);
			}
			if (RestrictionViewModel.UsingMoveLimitArea) {
				CorrectionMoving(hWnd, msg, wParam, lParam, ref handled);
			}
			if (RestrictionViewModel.UsingMaxMinSuppression) {
				SuppressionMaxMin(hWnd, msg, wParam, lParam, ref handled);
			}

			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}

		#endregion

		#region function

		IntPtr CorrectionSizing(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WM.WM_SIZING) {
				var logicalRect = UIUtility.ToLogicalPixel(View, PodStructUtility.Convert(WindowsUtility.ConvertRECTFromLParam(lParam)));

				var l = logicalRect.Left;
				var t = logicalRect.Top;
				var r = logicalRect.Right;
				var b = logicalRect.Bottom;

				var correctionSize = new Size(
					logicalRect.Width - RestrictionViewModel.MultipleThickness.GetHorizon(),
					logicalRect.Height - RestrictionViewModel.MultipleThickness.GetVertical()
				);

				var width = logicalRect.Width - (correctionSize.Width % RestrictionViewModel.MultipleSize.Width);
				var height = logicalRect.Height - (correctionSize.Height % RestrictionViewModel.MultipleSize.Height);

				var sizing = WindowsUtility.ConvertWMSZFromWParam(wParam);
				switch (sizing) {
					case WMSZ.WMSZ_LEFT:
						l = r - width;
						break;
					case WMSZ.WMSZ_RIGHT:
						r = l + width;
						break;
					case WMSZ.WMSZ_TOP:
						t = b - height;
						break;
					case WMSZ.WMSZ_BOTTOM:
						b = t + height;
						break;
					case WMSZ.WMSZ_TOPLEFT:
						t = b - height;
						l = r - width;
						break;
					case WMSZ.WMSZ_TOPRIGHT:
						r = l + width;
						t = b - height;
						break;
					case WMSZ.WMSZ_BOTTOMLEFT:
						l = r - width;
						b = t + height;
						break;
					case WMSZ.WMSZ_BOTTOMRIGHT:
						r = l + width;
						b = t + height;
						break;
				}

				var resizedLogicalRect = new Rect(l, t, r - l, b - t);
				var resizedDeviceRawRect = PodStructUtility.Convert(UIUtility.ToDevicePixel(View, resizedLogicalRect));
				Marshal.StructureToPtr(resizedDeviceRawRect, lParam, false);

				handled = true;
			}

			return IntPtr.Zero;
		}
		
		IntPtr CorrectionMoving(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WM.WM_MOVING) {
				var rawRect = WindowsUtility.ConvertRECTFromLParam(lParam);
				var logicalRect = UIUtility.ToLogicalPixel(View, PodStructUtility.Convert(rawRect));
				
				var x = logicalRect.Left;
				var y = logicalRect.Top;

				if (logicalRect.X < RestrictionViewModel.MoveLimitArea.X) {
					// 左
					x = RestrictionViewModel.MoveLimitArea.X;
				} else if (logicalRect.Right > RestrictionViewModel.MoveLimitArea.Right) {
					// 右
					x = RestrictionViewModel.MoveLimitArea.Right - logicalRect.Width;
				}

				if (logicalRect.Y < RestrictionViewModel.MoveLimitArea.Y) {
					// 上
					y = RestrictionViewModel.MoveLimitArea.Y;
				} else if (logicalRect.Bottom > RestrictionViewModel.MoveLimitArea.Bottom) {
					// 下
					y = RestrictionViewModel.MoveLimitArea.Bottom - logicalRect.Height;
				}

				var logicalPoint = new Point(x, y);
				var devicePoint = UIUtility.ToDevicePixel(View, logicalPoint);

				rawRect.X = (int)devicePoint.X;
				rawRect.Y = (int)devicePoint.Y;

				Marshal.StructureToPtr(rawRect, lParam, false);

				handled = true;
			}

			return IntPtr.Zero;
		}

		IntPtr SuppressionMaxMin(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == (int)WM.WM_SYSCOMMAND) {
				var sc = WindowsUtility.ConvertSCFromWParam(wParam);
				var set = new HashSet<SC>() {
					SC.SC_MINIMIZE,
					SC.SC_MAXIMIZE,
					SC.SC_RESTORE,
				};
				if(!set.Add(sc)) {
					handled = true;
				}
			}

			return IntPtr.Zero;
		}

		#endregion
	}
}
