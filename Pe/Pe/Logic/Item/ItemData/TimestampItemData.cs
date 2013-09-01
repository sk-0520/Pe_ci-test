﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 18:46
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	/// <summary>
	/// 
	/// </summary>
	public class TimestampItemData: ItemData
	{
		const string AttributeCreate = "create";
		const string AttributeUpdate = "update";
		
		public TimestampItemData()
		{
			Create = DateTime.MinValue;
			Update = DateTime.MinValue;
		}
		
		public override string Name { get { return "timestamp"; } }
		
		public DateTime Create { get; set; }
		public DateTime Update { get; set; }
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			// TODO: 変換未実装
			var unsafeCreate = result.GetAttribute(AttributeCreate);
			var unsafeUpdate = result.GetAttribute(AttributeUpdate);
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var create = Create.ToString("o");
			var update = Update.ToString("o");
			
			element.SetAttribute(AttributeCreate, create);
			element.SetAttribute(AttributeUpdate, update);
		}
	}
}
