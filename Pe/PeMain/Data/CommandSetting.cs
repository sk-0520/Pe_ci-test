﻿namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// コマンドランチャー設定
	/// </summary>
	[Serializable]
	public class CommandSetting: DisposableItem, IDisposable
	{
		//private Font _font = null;
		
		public CommandSetting()
		{
			Width = 200;
			Height = 200;
			IconScale = IconScale.Small;
			HiddenTime = Literal.commandHiddenTime.median;
			FontSetting = new FontSetting();
			HotKey = new HotKeySetting();
		}
		
		public override void CorrectionValue()
		{
			HiddenTime = Literal.commandHiddenTime.ToRounding(HiddenTime);
		}
		
		/// <summary>
		/// アイコンサイズ
		/// </summary>
		public IconScale IconScale { get; set; }
		/// <summary>
		/// フォント
		/// </summary>
		public FontSetting FontSetting { get; set; }
		/// <summary>
		/// 入力欄の横幅。
		/// </summary>
		public int Width { get; set; }
		/// <summary>
		/// 補助リストの高さ。
		/// </summary>
		public int Height { get; set; }
		/// <summary>
		/// 非アクティブからの非表示猶予。
		/// </summary>
		[XmlIgnore]
		public TimeSpan HiddenTime { get; set; }
		[XmlElement("HiddenTime", DataType = "duration")]
		public string _HiddenTime
		{
			get { return PropertyUtility.MixinTimeSpanGetter(HiddenTime); }
			set { HiddenTime = PropertyUtility.MixinTimeSpanSetter(value); }
		}

		/// <summary>
		/// 最前面表示。
		/// </summary>
		public bool TopMost { get; set; }
		
		public HotKeySetting HotKey { get; set; }

		protected override void Dispose(bool disposing)
		{
			FontSetting.ToDispose();

			base.Dispose(disposing);
		}
	}
}
