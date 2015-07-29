﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public abstract class LauncherItemViewModelBase: SingleModelWrapperViewModelBase<LauncherItemModel>, IHavingAppNonProcess, IHavingAppSender
	{
		#region variable

		static readonly Color defualtIconColor = Colors.Transparent;
		Color _iconColor = defualtIconColor;

		#endregion

		public LauncherItemViewModelBase(LauncherItemModel model, IAppNonProcess nonProcess, IAppSender appSender)
			: base(model)
		{
			NonProcess = nonProcess;
			AppSender = appSender;
		}

		#region property

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		public virtual LauncherKind LauncherKind
		{
			get { return Model.LauncherKind; }
			set { throw new NotSupportedException(); }
		}

		public string Command
		{
			get { return Model.Command; }
			set { SetModelValue(value); }
		}

		public virtual string WorkDirectoryPath
		{
			get { return Model.WorkDirectoryPath; }
			set { SetModelValue(value); }
		}

		public virtual string Option
		{
			get { return Model.Option; }
			set { SetModelValue(value); }
		}

		public string Comment
		{
			get { return Model.Comment; }
			set { SetModelValue(value); }
		}

		public IconItemModel Icon
		{
			get { return Model.Icon; }
			set { SetModelValue(value); }
		}

		public virtual bool StdStreamOutput
		{
			get { return Model.StdStream.OutputWatch; }
			set { SetPropertyValue(Model.StdStream, value, "OutputWatch"); }
		}

		public virtual bool Administrator
		{
			get { return Model.Administrator; }
			set { SetModelValue(value); }
		}

		public string Tags
		{
			get { return string.Join(", ", Model.Tag.Items); }
			set
			{
				var items = value.Split(',')
					.Where(s => !string.IsNullOrWhiteSpace(s))
					.Select(s => s.Trim())
					.OrderBy(s => s)
					.Distinct()
				;
				Model.Tag.Items = new CollectionModel<string>(items);
				OnPropertyChanged();
			}
		}

		#endregion

		#region function

		public BitmapSource GetIcon(IconScale iconScale)
		{
			CheckUtility.DebugEnforceNotNull(NonProcess.LauncherIconCaching);

			return NonProcess.LauncherIconCaching[iconScale].Get(Model, () => LauncherItemUtility.GetIcon(Model, iconScale, NonProcess));
		}

		public Color GetIconColor(IconScale iconScale)
		{
			if(this._iconColor == Colors.Transparent) {
				var icon = GetIcon(iconScale);
				this._iconColor = MediaUtility.GetPredominantColorFromBitmapSource(icon);
			}

			return this._iconColor;
		}

		protected void Execute()
		{
			try {
				ExecuteUtility.RunItem(Model, NonProcess, AppSender);
				SettingUtility.IncrementLauncherItem(Model, null, null, NonProcess);
			} catch (Exception ex) {
				NonProcess.Logger.Warning(ex);
			}
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
