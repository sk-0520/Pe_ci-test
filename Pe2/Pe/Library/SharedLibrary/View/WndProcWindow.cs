﻿namespace ContentTypeTextNet.Library.SharedLibrary.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// Windowsのウィンドウプロシージャを持つWindow。
	/// </summary>
	public abstract class WndProcWindow: WindowsAPIWindow, IDisposable, IIsDisposed
	{
		#region variable

		HwndSource _hWndSource;

		#endregion

		public WndProcWindow()
			: base()
		{ }

		~WndProcWindow()
		{
			Dispose(false);
		}

		#region property

		protected HwndSource HandleSource 
		{ 
			get 
			{
				if(this._hWndSource == null) {
					this._hWndSource = HwndSource.FromHwnd(Handle);
				}

				return this._hWndSource;
			} 
		}

		#endregion

		#region IIsDisposed

		public bool IsDisposed { get; private set; }

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
			}

			HandleSource.RemoveHook(WndProc);

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		#region WindowsAPIWindow

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
 			 base.OnLoaded(sender, e);

			 HandleSource.AddHook(WndProc);
		}

		protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			return IntPtr.Zero;
		}

		#endregion
	}
}
