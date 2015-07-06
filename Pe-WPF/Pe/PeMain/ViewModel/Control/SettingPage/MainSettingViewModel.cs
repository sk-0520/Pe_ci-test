﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class MainSettingViewModel: SettingPageViewModelBase
	{
		public MainSettingViewModel(RunningInformationItemModel runningInformation, LanguageItemModel language, LoggingItemModel logging, INonProcess nonProcess)
			: base(nonProcess)
		{
			RunningInformation = runningInformation;
			Language = language;
			Logging = logging;
		}

		#region property

		RunningInformationItemModel RunningInformation { get; set; }
		LanguageItemModel Language { get; set; }
		LoggingItemModel Logging { get; set; }

		#endregion
	}
}
