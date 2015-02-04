﻿namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public abstract class ExForm: Form
	{ }

	public class AppForm: ExForm
	{ 
		public AppForm()
		{
			UIUtility.InitializeWindow(this);
		}
	}

	/// <summary>
	/// アプリケーションで使用する基本Form。
	/// </summary>
	public class CommonForm: AppForm, ISetCommonData
	{
		/// <summary>
		/// 共通データ。
		/// </summary>
		protected CommonData CommonData { get; set; }
		/// <summary>
		/// 初期化済みであるか。
		/// 
		/// ここで言う初期化は SetCommonData を実施済みであるかという意味。
		/// </summary>
		protected bool Initialized { get; set; }

		public void SetCommonData(CommonData commonData)
		{
			Initialized = false;

			CommonData = commonData;
			ApplySetting();

			Initialized = true;
		}

		protected virtual void ApplySetting()
		{
			Debug.Assert(CommonData != null);

			ApplyLanguage();
			ApplySkin();
		}

		protected virtual void ApplyLanguage()
		{
			Debug.Assert(CommonData.Language != null);
		}

		protected virtual void ApplySkin()
		{
			Debug.Assert(CommonData.Skin != null);
		}
	}
}