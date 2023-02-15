using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Standard.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// ファイルのローテートを行う。
    /// </summary>
    public class FileRotator
    {
        #region function

        /// <summary>
        /// ローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="regex"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="order">ソート。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        private int ExecuteCore(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Order order, Func<Exception, bool> exceptionCacther)
        {
            parentDirectory.Refresh();
            if(!parentDirectory.Exists) {
                return -1;
            }

            var targetFiles = parentDirectory
                .EnumerateFiles("*")
                .Where(i => regex.IsMatch(i.Name))
                .OrderBy(order, i => i.Name)
                .Skip(leaveCount)
                .ToArray()
            ;

            var removedCount = 0;
            for(var i = 0; i < targetFiles.Length; i++) {
                try {
                    File.Delete(targetFiles[i].FullName);
                    removedCount += 1;
                } catch(Exception ex) {
                    if(!exceptionCacther(ex)) {
                        return removedCount;
                    }
                }
            }

            return removedCount;
        }

        /// <summary>
        /// 正規表現に該当したファイルのローテート処理。
        /// <para>降順で列挙する。</para>
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="regex"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteRegex(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteCore(parentDirectory, regex, leaveCount, Order.Descending, exceptionCacther);
        }
        /// <summary>
        /// 正規表現に該当したファイルのローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="regex"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="order">ソート。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteRegex(DirectoryInfo parentDirectory, Regex regex, int leaveCount, Order order, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteCore(parentDirectory, regex, leaveCount, order, exceptionCacther);
        }

        /// <summary>
        /// ワイルドカードに該当したファイルのローテート処理。
        /// <para>降順で列挙する。</para>
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="wildCard"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteWildcard(DirectoryInfo parentDirectory, string wildCard, int leaveCount, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteWildcard(parentDirectory, wildCard, leaveCount, Order.Descending, exceptionCacther);
        }
        /// <summary>
        /// ワイルドカードに該当したファイルのローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="wildCard"><paramref name="parentDirectory"/>直下の対象ファイル。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="order">ソート。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteWildcard(DirectoryInfo parentDirectory, string wildCard, int leaveCount, Order order, Func<Exception, bool> exceptionCacther)
        {
            var wildcardPattern = "^" + Regex.Escape(wildCard).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            var wildcardRegex = new Regex(wildcardPattern, RegexOptions.IgnoreCase);

            return ExecuteCore(parentDirectory, wildcardRegex, leaveCount, order, exceptionCacther);
        }

        /// <summary>
        /// 拡張子に該当したファイルのローテート処理。
        /// <para>降順で列挙する。</para>
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="extensions"><paramref name="parentDirectory"/>直下の拡張子。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteExtensions(DirectoryInfo parentDirectory, IEnumerable<string> extensions, int leaveCount, Func<Exception, bool> exceptionCacther)
        {
            return ExecuteExtensions(parentDirectory, extensions, leaveCount, Order.Descending, exceptionCacther);
        }
        /// <summary>
        /// 拡張子に該当したファイルのローテート処理。
        /// </summary>
        /// <param name="parentDirectory">親ディレクトリ。</param>
        /// <param name="extensions"><paramref name="parentDirectory"/>直下の拡張子。</param>
        /// <param name="leaveCount">列挙されたファイルの残す数。</param>
        /// <param name="order">ソート。</param>
        /// <param name="exceptionCacther">ファイル削除中に例外を受け取った場合の処理。trueを返すと継続、falseで処理終了。</param>
        /// <returns>削除した数。ディレクトリが存在しない場合は -1 を返す。</returns>
        public int ExecuteExtensions(DirectoryInfo parentDirectory, IEnumerable<string> extensions, int leaveCount, Order order, Func<Exception, bool> exceptionCacther)
        {
            var extensionPatterns = extensions
                .Select(i => Regex.Escape(i))
                .Select(i => "(" + i + ")")
                .JoinString("|")
            ;
            var extensionPattern = "(" + extensionPatterns + ")";
            var extensionRegex = new Regex(extensionPattern, RegexOptions.IgnoreCase);

            return ExecuteCore(parentDirectory, extensionRegex, leaveCount, order, exceptionCacther);
        }

        #endregion
    }
}
