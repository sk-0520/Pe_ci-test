﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 15:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace PeMain.Setting
{
	/// <summary>
	/// NonSerializedAttribute
	/// </summary>
	[Serializable]
	public class Word: NameItem
	{
		/// <summary>
		/// 
		/// </summary>
		[System.Xml.Serialization.XmlAttribute]
		public string Text { get; set; }
	}
	
	/// <summary>
	/// Description of Language.
	/// </summary>
	[Serializable]
	public class Language: NameItem
	{
		public Language()
		{
			Define = new List<Word>();
			Words = new List<Word>();
		}

		public List<Word> Define { get; set; }
		public List<Word> Words  { get; set; }
		
		[System.Xml.Serialization.XmlAttribute]
		public string Code { get; set; }
		
		private Word getWord(IEnumerable<Word> list, string key)
		{
			Word word = null;
			
			word = list.SingleOrDefault(item => item.Name == key);
			
			if(word == null) {
				word = new Word();
				word.Name = key;
				word.Text = "<" + key + ">";
			}
			
			return word;
		}
		
		public string getPlain(string key)
		{
			var word = getWord(Words, key);
			
			return word.Text;
		}
		
		/// <summary>
		/// 変換済み文字列の取得。
		/// 
		/// 定義済み文字列は展開される。
		/// </summary>
		public string this[string key]
		{
			get 
			{
				var text = getPlain(key);
				if(text.Any(c => c == '$')) {
					// ${...}
					var replacedText = Regex.Replace(text, @"\$\{(.*)\}", (Match m) => 
						getWord(Define, m.Groups[1].Value).Text
					);
					return replacedText;
				}
				
				return text;
			}
		}
	}
}