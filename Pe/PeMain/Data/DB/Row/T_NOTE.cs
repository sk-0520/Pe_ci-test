﻿namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	[EntityMapping("T_NOTE")]
	public class TNoteRow: CommonDataRow
	{
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }
		[EntityMapping("NOTE_BODY")]
		public string Body { get; set ;}
	}
}