﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

	/// <summary>
	/// 文字列処理共通。
	/// </summary>
	public static class TextUtility
	{
		/// <summary>
		/// 指定データを集合の中から単一である値に変換する。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="list"></param>
		/// <param name="dg">nullの場合はデフォルト動作</param>
		/// <returns></returns>
		public static string ToUnique(string target, IEnumerable<string> list, Func<string, int, string> dg)
		{
			Debug.Assert(dg != null);

			var changeName = target;

			int n = 1;
			RETRY:
			foreach(var value in list) {
				if(value == changeName) {
					changeName = dg(target, ++n);
					goto RETRY;
				}
			}

			return changeName;
		}

		/// <summary>
		/// 指定データを集合の中から単一である値に変換する。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="list"></param>
		/// <returns>集合の中に同じものがなければtarget, 存在すればtarget(n)。</returns>
		public static string ToUniqueDefault(string target, IEnumerable<string> list)
		{
			return ToUnique(target, list, (string source, int index) => string.Format("{0}({1})", source, index));
		}

		/// <summary>
		/// 指定範囲の値を指定処理で置き換える。
		/// </summary>
		/// <param name="src">対象。</param>
		/// <param name="head">置き換え開始文字列。</param>
		/// <param name="tail">置き換え終了文字列。</param>
		/// <param name="dg">処理。</param>
		/// <returns></returns>
		public static string ReplaceRange(this string src, string head, string tail, Func<string, string> dg)
		{
			var escHead = Regex.Escape(head);
			var escTail = Regex.Escape(tail);
			var pattern = escHead + "(.+?)" + escTail;
			var replacedText = Regex.Replace(src, pattern, (Match m) => dg(m.Groups[1].Value));
			return replacedText;
		}

		/// <summary>
		/// 指定範囲の値を指定のコレクションで置き換える。
		/// </summary>
		/// <param name="src">対象。</param>
		/// <param name="head">置き換え開始文字列。</param>
		/// <param name="tail">置き換え終了文字列。</param>
		/// <param name="map">置き換え対象文字列と置き換え後文字列のペアであるコレクション。</param>
		/// <returns></returns>
		public static string ReplaceRangeFromDictionary(this string src, string head, string tail, IDictionary<string, string> map)
		{
			return src.ReplaceRange(head, tail, s => map.ContainsKey(s) ? map[s] : head + s + tail);
		}

		/// <summary>
		/// 文字列を連想配列のキーから値に変換する。
		/// </summary>
		/// <param name="src"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		public static string ReplaceFromDictionary(this string src, IDictionary<string, string> map)
		{
			var pattern = string.Format("(?<HIT>{0})", string.Join("|", map.Keys.Select(s => Regex.Escape(s)).Select(s => string.Format("({0})", s))));
			var reg = new Regex(pattern);
			return reg.Replace(src, (Match m) => {
				var key = m.Groups["HIT"].Value;
				return map[key];
			});
		}
	}
}
