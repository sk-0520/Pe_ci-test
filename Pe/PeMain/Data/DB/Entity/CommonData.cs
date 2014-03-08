﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/08
 * 時刻: 13:24
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeUtility;

namespace PeMain.Data.DB
{
	public abstract class CommonDataEntity: Entity
	{
		[TargetName("CMN_ENABLED")]
		public bool CommonEnabled { get; set; }
		[TargetName("CMN_CREATE")]
		public DateTime CommonCreate { get; set; }
		[TargetName("CMN_UPDATE")]
		public DateTime CommonUpdate { get; set; }
	}
}