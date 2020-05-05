using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public enum CommandItemKind
    {
        #region property

        #region Pe
        /// <summary>
        /// コマンドはランチャーアイテム。
        /// </summary>
        LauncherItem,
        /// <summary>
        /// ランチャーアイテムの名前に一致。
        /// </summary>
        LauncherItemName,
        /// <summary>
        /// ランチャーアイテムのコードに一致。
        /// </summary>
        LauncherItemCode,
        /// <summary>
        /// ランチャーアイテムのタグに一致。
        /// </summary>
        LauncherItemTag,

        /// <summary>
        /// アプリケーション固有処理。
        /// </summary>
        ApplicationCommand,

        #endregion
        /// <summary>
        /// プラグイン処理により生成。
        /// </summary>
        Plugin,

        #endregion
    }

    /// <summary>
    /// コマンド型ランチャーで表示するアイテム。
    /// </summary>
    public interface ICommandItem
    {
        #region property

        /// <summary>
        /// 小さく表示する種別文言。
        /// </summary>
        CommandItemKind Kind { get; }

        /// <summary>
        /// メイン表示文字列。
        /// </summary>
        IReadOnlyList<HitValue> HeaderValues { get; }
        /// <summary>
        /// 追記文言。
        /// </summary>
        IReadOnlyList<HitValue> DescriptionValues { get; }


        int Score { get; }

        #endregion

        #region function

        /// <summary>
        /// アイコン取得。
        /// <para>UIスレッド上で実行を保証。</para>
        /// </summary>
        /// <param name="iconBox"></param>
        /// <returns>アイコンとなるデータ。</returns>
        object GetIcon(IconBox iconBox);

        /// <summary>
        /// コマンドアイテムの実行。
        /// </summary>
        /// <param name="screen">コマンドランチャーの所在地。</param>
        /// <param name="isExtend">拡張機能(コマンドアイテム依存)を用いるか。</param>
        void Execute(IScreen screen, bool isExtend);

        #endregion
    }
}
