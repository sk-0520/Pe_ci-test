﻿namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using System.Windows.Threading;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data.Event;
	using System.ComponentModel;
	using ContentTypeTextNet.Pe.PeMain.View.Parts;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using System.Windows.Interop;
	using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
	using Xceed.Wpf.Toolkit;
	using System.Windows.Controls.Primitives;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Define;

	/// <summary>
	/// ToolbarWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherToolbarWindow : ViewModelCommonDataWindow<LauncherToolbarViewModel>, IApplicationDesktopToolbar, IHavingWindowKind
	{
		public LauncherToolbarWindow()
		{
			InitializeComponent();
		}

		#region property

		//ScreenModel Screen { get; set; }

		ApplicationDesktopToolbar Appbar { get; set; }
		VisualStyle VisualStyle { get; set; }
		WindowAreaCorrection WindowAreaCorrection { get; set; }
		WidthResizeHitTest WindowHitTest { get; set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = new LauncherToolbarDataModel() {
				LauncherItems = CommonData.LauncherItemSetting.Items,
				GroupItems = CommonData.LauncherGroupSetting.Groups,
			};

			ToolbarItemModel toolbar;
			var screen = ExtensionData as ScreenModel;
			if (screen != null) {
				if(!CommonData.MainSetting.Toolbar.Items.TryGetValue(screen.DeviceName, out toolbar)) {
					toolbar = new ToolbarItemModel();
					toolbar.Id = screen.DeviceName;
					CommonData.Logger.Information("create toolbar", screen);
					CommonData.MainSetting.Toolbar.Items.Add(toolbar);
				}
			} else {
				CommonData.Logger.Debug("dummy toolbar");
				toolbar = new ToolbarItemModel();
			}
			SettingUtility.InitializeToolbar(toolbar, Constants.assemblyVersion, CommonData.NonProcess);
			model.Toolbar = toolbar;

			ViewModel = new LauncherToolbarViewModel(model, this, screen, CommonData.NonProcess, CommonData.AppSender);
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			int exStyle = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_EXSTYLE);
			exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
			WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

			// ノート側はこれつけなきゃいけないの腑に落ちん
			//var style = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_STYLE);
			//style &= ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
			//WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_STYLE, (IntPtr)style);
			
			base.OnLoaded(sender, e);

			Appbar = new ApplicationDesktopToolbar(this, ViewModel, CommonData.NonProcess);
			VisualStyle = new VisualStyle(this, ViewModel, CommonData.NonProcess);
			WindowAreaCorrection = new WindowAreaCorrection(this, ViewModel, CommonData.NonProcess);
			WindowHitTest = new WidthResizeHitTest(this, ViewModel, CommonData.NonProcess);
		}

		protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			//if(Appbar != null) {
			//	Appbar.WndProc(hWnd, msg, wParam, lParam, ref handled);
			//}
			//if (VisualStyle != null) {
			//	VisualStyle.WndProc(hWnd, msg, wParam, lParam, ref handled);
			//}
			//if (WindowAreaCorrection != null) {
			//	WindowAreaCorrection.WndProc(hWnd, msg, wParam, lParam, ref handled);
			//}
			//if (handled) {
			//	return IntPtr.Zero;
			//}


			var extends = new IHavingWndProc[] {
				VisualStyle,
				Appbar,
				WindowAreaCorrection,
				WindowHitTest,
			};
			foreach (var extend in extends.Where(e => e != null)) {
				var result = extend.WndProc(hWnd, msg, wParam, lParam, ref handled);
				if(handled) {
					return result;
				}
			}

			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if(Appbar != null) {
				Appbar.Dispose();
				Appbar = null;
			}
		}

		#endregion

		#region IApplicationDesktopToolbar

		public void Docking(DockType dockType, bool autoHide)
		{
			if (Appbar != null) {
				Appbar.Docking(dockType, autoHide);
				if(VisualStyle != null) {
					//VisualStyle.UnsetStyle();
					VisualStyle.SetStyle();
				}
				//NativeMethods.UpdateWindow(Handle);
			}
		}

		#endregion

		#region IHavingWindowKind

		public WindowKind WindowKind { get { return WindowKind.LauncherToolbar; } }

		#endregion

		#region function


		#endregion

		//private void Caption_MouseLeftButton(object sender, MouseButtonEventArgs e)
		//{
		//	if (e.LeftButton == MouseButtonState.Pressed) {
		//		if (ViewModel.CanWindowDrag) {
		//			DragMove();
		//		}
		//	}
		//}

		private void Element_Click(object sender, RoutedEventArgs e)
		{
			// ダルイ、全部閉じちゃおう
			foreach(var button in UIUtility.FindVisualChildren<DropDownButton>(this).Where(b => b.IsOpen)) {
				button.IsOpen = false;
			}
		}

		private void PART_ToggleButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void PART_ToggleButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
		}

		private void PART_Popup_Opened(object sender, EventArgs e)
		{
			var popup = (Popup)sender;

			LanguageUtility.RecursiveSetLanguage(popup, CommonData.Language);
		}
	}
}
