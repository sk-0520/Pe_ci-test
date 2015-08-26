﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class ClipboardUtility
	{
		#region define

		class CaseInsensitiveComparer: IComparer<string>
		{
			public int Compare(string x, string y)
			{
				return string.Compare(x, y, true);
			}
		}

		#endregion

		/// <summary>
		/// コピー処理の親玉。
		/// </summary>
		/// <param name="action">コピー処理。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		static void Copy(Action action, IClipboardWatcher watcher)
		{
			CheckUtility.DebugEnforceNotNull(watcher);

			bool? enabledWatch = null;
			if(!watcher.ClipboardEnabledApplicationCopy) {
				enabledWatch = watcher.ClipboardWatching;
				if(enabledWatch.Value) {
					watcher.ClipboardWatchingChange(false);
				}
			}

			try {
				action();
			} finally {
				if(enabledWatch.HasValue) {
					Debug.Assert(!watcher.ClipboardEnabledApplicationCopy);
					Debug.Assert(watcher != null);

					if(enabledWatch.Value) {
						watcher.ClipboardWatchingChange(true);
					}
				}
			}
		}

		/// <summary>
		/// 文字列をクリップボードへ転写。
		/// </summary>
		/// <param name="text">対象文字列。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyText(string text, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), watcher);
		}

		/// <summary>
		/// RTFをクリップボードへ転写。
		/// </summary>
		/// <param name="rtf">対象RTF。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyRtf(string rtf, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), watcher);
		}

		/// <summary>
		/// HTMLをクリップボードへ転写。
		/// </summary>
		/// <param name="html">対象HTML。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyHtml(string html, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(html, TextDataFormat.Html), watcher);
		}

		/// <summary>
		/// 画像をクリップボードへ転写。
		/// </summary>
		/// <param name="image">対象画像。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyImage(BitmapSource image, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetImage(image), watcher);
		}

		/// <summary>
		/// ファイルをクリップボードへ転写。
		/// </summary>
		/// <param name="file">対象ファイル。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyFile(IEnumerable<string> file, IClipboardWatcher watcher)
		{
			var sc = TextUtility.ToStringCollection(file);
			Copy(() => Clipboard.SetFileDropList(sc), watcher);
		}

		/// <summary>
		/// 複合データをクリップノードへ転写。
		/// <para>基本的にはvoid CopyClipboardItem(ClipboardItem, ClipboardSetting)を使用する</para>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyDataObject(IDataObject data, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetDataObject(data), watcher);
		}

		public static void CopyClipboardItem(ClipboardData clipboardItem, IClipboardWatcher watcher)
		{
			Debug.Assert(clipboardItem.Type != ClipboardType.None);

			var data = new DataObject();
			var typeFuncs = new Dictionary<ClipboardType, Action>() {
				{ ClipboardType.Text, () => data.SetText(clipboardItem.Body.Text, TextDataFormat.UnicodeText) },
				{ ClipboardType.Rtf, () => data.SetText(clipboardItem.Body.Rtf, TextDataFormat.Rtf) },
				{ ClipboardType.Html, () => data.SetText(clipboardItem.Body.Html, TextDataFormat.Html) },
				{ ClipboardType.Image, () => data.SetImage(clipboardItem.Body.Image) },
				{ ClipboardType.File, () => {
					data.SetFileDropList(TextUtility.ToStringCollection(clipboardItem.Body.Files)); 
				}},
			};
			foreach(var type in ClipboardUtility.GetClipboardTypeList(clipboardItem.Type)) {
				typeFuncs[type]();
			}
			CopyDataObject(data, watcher);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="range"></param>
		/// <param name="rawHtml"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		static string ConvertStringFromRawHtml(RangeModel<int> range, byte[] rawHtml, Encoding encoding)
		{
			if(-1 < range.Head && -1 < range.Tail && range.Head <= range.Tail) {
				var raw = rawHtml.Skip(range.Head).Take(range.Tail - range.Head);
				return Encoding.UTF8.GetString(raw.ToArray());
			}

			return null;
		}

		/// <summary>
		/// <para>UTF-8</para>
		/// </summary>
		/// <param name="range"></param>
		/// <param name="rawHtml"></param>
		/// <returns></returns>
		static string ConvertStringFromDefaultRawHtml(RangeModel<int> range, byte[] rawHtml)
		{
			return ConvertStringFromRawHtml(range, rawHtml, Encoding.UTF8);
		}

		public static ClipboardHtmlData ConvertClipboardHtmlFromFromRawHtml(string rawClipboardHtml, INonProcess nonProcess)
		{
			var result = new ClipboardHtmlData();

			//Version:0.9
			//StartHTML:00000213
			//EndHTML:00001173
			//StartFragment:00000247
			//EndFragment:00001137
			//SourceURL:file:///C:/Users/sk/Documents/Programming/SharpDevelop%20Project/Pe/Pe/PeMain/etc/lang/ja-JP.accept.html

			var map = new Dictionary<string, Action<string>>() {
				{ "version", s => result.Version = decimal.Parse(s) },
				{ "starthtml", s => result.Html.Head = int.Parse(s) },
				{ "endhtml", s => result.Html.Tail = int.Parse(s) },
				{ "startfragment", s => result.Fragment.Head = int.Parse(s) },
				{ "endfragment", s => result.Fragment.Tail = int.Parse(s) },
				{ "sourceurl", s => result.SourceURL = new Uri(s) },
			};
			var reg = new Regex(@"
				^\s*
				(?<KEY>
					Version
					|StartHTML
					|EndHTML
					|StartFragment
					|EndFragment
					|SourceURL
				)
				\s*:\s*
				(?<VALUE>
					.+?
				)
				\s*$
				",
				RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
			);
			for(var match = reg.Match(rawClipboardHtml); match.Success; match = match.NextMatch()) {
				var key = match.Groups["KEY"].Value.ToLower();
				var value = match.Groups["VALUE"].Value;
				try {
					map[key](value);
				} catch(Exception ex) {
					nonProcess.Logger.Warning(ex);
				}
			}

			var rawHtml = Encoding.UTF8.GetBytes(rawClipboardHtml);
			result.HtmlText = ConvertStringFromDefaultRawHtml(result.Html, rawHtml); ;
			result.FragmentText = ConvertStringFromDefaultRawHtml(result.Fragment, rawHtml);
			result.SelectionText = ConvertStringFromDefaultRawHtml(result.Selection, rawHtml);

			return result;
		}

		/// <summary>
		/// .NET Frameworkからクリップボードデータを取得
		/// </summary>
		/// <param name="enabledTypes"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static ClipboardData GetClipboardDataFromFramework(ClipboardType enabledTypes, INonProcess nonProcess)
		{
			var clipboardData = new ClipboardData();

			try {
				var clipboardObject = Clipboard.GetDataObject();
				if(clipboardObject != null) {
					if(enabledTypes.HasFlag(ClipboardType.Text)) {
						if(clipboardObject.GetDataPresent(DataFormats.UnicodeText)) {
							clipboardData.Body.Text = (string)clipboardObject.GetData(DataFormats.UnicodeText);
							clipboardData.Type |= ClipboardType.Text;
						} else if(clipboardObject.GetDataPresent(DataFormats.Text)) {
							clipboardData.Body.Text = (string)clipboardObject.GetData(DataFormats.Text);
							clipboardData.Type |= ClipboardType.Text;
						}
					}

					if(enabledTypes.HasFlag(ClipboardType.Rtf) && clipboardObject.GetDataPresent(DataFormats.Rtf)) {
						clipboardData.Body.Rtf = (string)clipboardObject.GetData(DataFormats.Rtf);
						clipboardData.Type |= ClipboardType.Rtf;
					}

					if(enabledTypes.HasFlag(ClipboardType.Html) && clipboardObject.GetDataPresent(DataFormats.Html)) {
						clipboardData.Body.Html = (string)clipboardObject.GetData(DataFormats.Html);
						clipboardData.Type |= ClipboardType.Html;
					}

					if(enabledTypes.HasFlag(ClipboardType.Image) && clipboardObject.GetDataPresent(DataFormats.Bitmap)) {
						var image = clipboardObject.GetData(DataFormats.Bitmap) as BitmapSource;
						if(image != null) {
							var bitmap = BitmapFrame.Create(image);

							clipboardData.Body.Image = bitmap;
							clipboardData.Type |= ClipboardType.Image;
						}
					}

					if(enabledTypes.HasFlag(ClipboardType.File) && clipboardObject.GetDataPresent(DataFormats.FileDrop)) {
						var files = clipboardObject.GetData(DataFormats.FileDrop) as string[];
						if(files != null) {
							var sortedFiles = files.OrderBy(s => s, new CaseInsensitiveComparer());
							clipboardData.Body.Files.AddRange(sortedFiles);
							clipboardData.Body.Text = string.Join(Environment.NewLine, sortedFiles);
							clipboardData.Type |= ClipboardType.Text | ClipboardType.File;
						}
					}
				}
			} catch(COMException ex) {
				nonProcess.Logger.Error(ex);
			}

			return clipboardData;
		}

		/// <summary>
		/// ハッシュ算出用アルゴリズムの取得。
		/// </summary>
		/// <param name="hashType"></param>
		/// <returns></returns>
		static HashAlgorithm GetHashAlgorithm(HashType hashType)
		{
			switch(hashType) {
				case HashType.SHA1:
					return new SHA1CryptoServiceProvider();

				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// 文字列からハッシュ値を算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="s"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromString(HashType hashType, string s, INonProcess nonProcess)
		{
			using(var hash = GetHashAlgorithm(hashType)) {
				var binary = Encoding.Unicode.GetBytes(s);
				return hash.ComputeHash(binary);
			}
		}

		/// <summary>
		/// テキストからハッシュ値を算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="bodyItem"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromText(HashType hashType, ClipboardBodyItemModel bodyItem, INonProcess nonProcess)
		{
			return CalculateHashCodeFromString(hashType, bodyItem.Text, nonProcess);
		}

		/// <summary>
		/// RTFからハッシュ値を算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="bodyItem"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromRtf(HashType hashType, ClipboardBodyItemModel bodyItem, INonProcess nonProcess)
		{
			return CalculateHashCodeFromString(hashType, bodyItem.Rtf, nonProcess);
		}

		/// <summary>
		/// HTMLからハッシュ値を算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="bodyItem"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromHtml(HashType hashType, ClipboardBodyItemModel bodyItem, INonProcess nonProcess)
		{
			return CalculateHashCodeFromString(hashType, bodyItem.Html, nonProcess);
		}

		/// <summary>
		/// 画像からハッシュ値を算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="bodyItem"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromImage(HashType hashType, ClipboardBodyItemModel bodyItem, INonProcess nonProcess)
		{
			var binaryWidth = BitConverter.GetBytes(bodyItem.Image.PixelWidth);
			var binaryHeight = BitConverter.GetBytes(bodyItem.Image.PixelHeight);
			var binaryImage = bodyItem.Image_Impl;
			var binaryList = new[] {
				binaryWidth,
				binaryHeight,
				binaryImage,
			};

			return CalculateHashCodeFromBinaryList(hashType, binaryList, nonProcess);
		}

		/// <summary>
		/// ファイルからハッシュ値を算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="bodyItem"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromFiles(HashType hashType, ClipboardBodyItemModel bodyItem, INonProcess nonProcess)
		{
			var binaryList = bodyItem.Files.Select((s, i) => Encoding.Unicode.GetBytes(s + i.ToString()));
			return CalculateHashCodeFromBinaryList(hashType, binaryList, nonProcess);
		}

		/// <summary>
		/// バイナリ群からハッシュ値の算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="binaryList"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		static byte[] CalculateHashCodeFromBinaryList(HashType hashType, IEnumerable<byte[]> binaryList, INonProcess nonProcess)
		{
			using(var stream = new MemoryStream()) {
				foreach(var binary in binaryList) {
					stream.Write(binary, 0, binary.Length);
				}

				using(var hash = GetHashAlgorithm(hashType)) {
					stream.Seek(0, SeekOrigin.Begin);
					return hash.ComputeHash(stream);
				}
			}
		}

		/// <summary>
		/// 各種値からハッシュ値の算出。
		/// </summary>
		/// <param name="hashType"></param>
		/// <param name="clipboardType"></param>
		/// <param name="bodyItem"></param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		public static byte[] CalculateHashCode(HashType hashType, ClipboardType clipboardType, ClipboardBodyItemModel bodyItem, INonProcess nonProcess)
		{
			var map = new Dictionary<ClipboardType, Func<HashType, ClipboardBodyItemModel, INonProcess, byte[]>>() {
				{ ClipboardType.Text, CalculateHashCodeFromText },
				{ ClipboardType.Rtf, CalculateHashCodeFromRtf },
				{ ClipboardType.Html, CalculateHashCodeFromHtml },
				{ ClipboardType.Image, CalculateHashCodeFromImage },
				{ ClipboardType.File, CalculateHashCodeFromFiles },
			};
			var binaryDataList = map
				.Where(p => clipboardType.HasFlag(p.Key))
				.Select(p => p.Value(hashType, bodyItem, nonProcess))
				.ToList()
			;
			var binaryClipboardType = BitConverter.GetBytes((int)clipboardType);
			binaryDataList.Add(binaryClipboardType);

			return CalculateHashCodeFromBinaryList(hashType, binaryDataList, nonProcess);
		}

		/// <summary>
		/// <see cref="GetClipboardData"/>の内部実装。
		/// </summary>
		/// <param name="enabledTypes"></param>
		/// <param name="hWnd"></param>
		/// <param name="nonProcess"></param>
		/// <param name="calcHash">ハッシュ値の算出を行うか。</param>
		/// <returns></returns>
		static ClipboardData GetClipboardData_Impl(ClipboardType enabledTypes, IntPtr hWnd, INonProcess nonProcess, bool calcHash)
		{
			var clipboardItem = GetClipboardDataFromFramework(enabledTypes, nonProcess);
			if(calcHash && clipboardItem.Type != ClipboardType.None) {
				clipboardItem.Hash.Type = HashType.SHA1;
				clipboardItem.Hash.Code = CalculateHashCode(HashType.SHA1, clipboardItem.Type, clipboardItem.Body, nonProcess);
			}
			return clipboardItem;

		}
		/// <summary>
		/// 現在のクリップボードからクリップボードアイテムを生成する。
		/// </summary>
		/// <param name="enabledTypes">取り込み対象とするクリップボード種別。</param>
		/// <returns>生成されたクリップボードアイテム。nullが返ることはない。</returns>
		public static ClipboardData GetClipboardData(ClipboardType enabledTypes, IntPtr hWnd, INonProcess nonProcess)
		{
			return GetClipboardData_Impl(enabledTypes, hWnd, nonProcess, true);
		}

		/// <summary>
		/// 指定ウィンドウハンドルに文字列を転送する。
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="outputText"></param>
		/// <param name="nonProcess"></param>
		/// <param name="clipboardWatcher"></param>
		public static void OutputTextForWindowHandle(IntPtr hWnd, string outputText, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
		{
			if (string.IsNullOrEmpty(outputText)) {
				nonProcess.Logger.Information("empty");
				return;
			}

			if (hWnd == IntPtr.Zero) {
				nonProcess.Logger.Warning("notfound");
				return;
			}

			NativeMethods.SetForegroundWindow(hWnd);
			if (clipboardWatcher.UsingClipboard) {
				// 現在クリップボードを一時退避
				var clipboardItem = ClipboardUtility.GetClipboardData_Impl(ClipboardType.All, hWnd, nonProcess, false);
				try {
					ClipboardUtility.CopyText(outputText, clipboardWatcher);
					NativeMethods.SendMessage(hWnd, WM.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
				} finally {
					if (clipboardItem.Type != ClipboardType.None) {
						ClipboardUtility.CopyClipboardItem(clipboardItem, clipboardWatcher);
					}
				}
			} else {
				SendKeysUtility.Send(outputText);
			}
		}

		/// <summary>
		/// 指定ウィンドウハンドルの次のウィンドウハンドルに文字列を転送する。
		/// </summary>
		/// <param name="hBaseWnd"></param>
		/// <param name="outputText"></param>
		/// <param name="nonProcess"></param>
		/// <param name="clipboardWatcher"></param>
		public static void OutputTextForNextWindow(IntPtr hBaseWnd, string outputText, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
		{

			var windowHandles = new List<IntPtr>();
			var hWnd = hBaseWnd;
			do {
				hWnd = NativeMethods.GetWindow(hWnd, GW.GW_HWNDNEXT);
				windowHandles.Add(hWnd);
			} while(!NativeMethods.IsWindowVisible(hWnd));

			OutputTextForWindowHandle(hWnd, outputText, nonProcess, clipboardWatcher);
		}

		private static IEnumerable<ClipboardType> GetEnabledClipboardTypeList(ClipboardType types, IEnumerable<ClipboardType> list)
		{
			return list.Where(t => types.HasFlag(t));
		}

		/// <summary>
		/// このアイテムが保持する有効なデータ種別を列挙する。
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ClipboardType> GetClipboardTypeList(ClipboardType types)
		{
			Debug.Assert(types != ClipboardType.None);

			var list = new[] {
				ClipboardType.Text,
				ClipboardType.Rtf,
				ClipboardType.Html,
				ClipboardType.Image,
				ClipboardType.File,
			};

			return GetEnabledClipboardTypeList(types, list);
		}

		public static ClipboardType GetSingleClipboardType(ClipboardType types)
		{
			var list = new[] {
				ClipboardType.Html,
				ClipboardType.Rtf,
				ClipboardType.File,
				ClipboardType.Text,
				ClipboardType.Image,
			};

			return GetEnabledClipboardTypeList(types, list).First();
		}
	}
}
