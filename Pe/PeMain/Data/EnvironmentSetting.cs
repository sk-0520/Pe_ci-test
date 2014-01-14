﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/26
 * 時刻: 22:58
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PeMain.Data
{
	/// <summary>
	/// ランチャアイテム起動時の環境変数設定
	/// 
	/// デフォルト環境変数(ON/OFF)に追加・変更を適用してから削除する。
	/// </summary>
	[Serializable]
	public class EnvironmentSetting: Item
	{
		public EnvironmentSetting()
		{
			UseDefault = true;
			Update = new List<KeyValuePair<string, string>>();
			Remove = new List<string>();
		}
		public bool UseDefault { get; set; }
		/// <summary>
		/// 追加・変更対象
		/// </summary>
		public List<KeyValuePair<string, string>> Update { get; set; }
		/// <summary>
		/// 削除変数
		/// </summary>
		public List<string> Remove { get; set; }
	}
}