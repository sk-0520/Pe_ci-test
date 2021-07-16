﻿#pragma once
#include "text.h"

typedef enum tag_LOCALE_TYPE
{
    /// <summary>
    /// ロケール非依存。
    /// </summary>
    LOCALE_TYPE_INVARIANT = LOCALE_INVARIANT,
    /// <summary>
    /// システムのロケール。
    /// </summary>
    LOCALE_TYPE_SYSTEM_DEFAULT = LOCALE_SYSTEM_DEFAULT,
    /// <summary>
    /// ユーザーのロケール。
    /// </summary>
    LOCALE_TYPE_USER_DEFAULT = LOCALE_USER_DEFAULT,
} LOCALE_TYPE;

typedef enum tag_TEXT_COMPARE_MODE
{
    TEXT_COMPARE_MODE_NONE = 0,
    /// <summary>
    /// 大文字と小文字を区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_CASE = NORM_IGNORECASE,
    /// <summary>
    /// ひらがなとカタカナを区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_KANA = NORM_IGNOREKANATYPE,
    /// <summary>
    /// アクセントとかを区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_NONSPACE = NORM_IGNORENONSPACE,
    /// <summary>
    /// 記号を区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_SYMBOLS = NORM_IGNORESYMBOLS,
    /// <summary>
    /// 全角半角を区別しない。
    /// </summary>
    TEXT_COMPARE_MODE_IGNORE_WIDTH = NORM_IGNOREWIDTH,
    /// <summary>
    /// 区切り記号を記号として扱う。
    /// </summary>
    TEXT_COMPARE_MODE_STRING_SORT = SORT_STRINGSORT,
} TEXT_COMPARE_MODE;

typedef struct tag_TEXT_COMPARE_RESULT
{
    /// <summary>
    /// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
    /// </summary>
    int compare;
    /// <summary>
    /// 成功したか。
    /// </summary>
    bool success;
} TEXT_COMPARE_RESULT;


/// <summary>
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索テキスト。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>見つかったテキストを開始とする参照テキスト、見つからない場合は無効テキスト。解放不要。</returns>
TEXT findText(const TEXT* haystack, const TEXT* needle, bool ignoreCase);

/// <summary>
/// テキスト検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索文字。</param>
/// <returns>見つかったテキストを開始とする参照テキスト、見つからない場合は無効テキスト。解放不要。</returns>
TEXT findCharacter2(const TEXT* haystack, TCHAR needle);

/// <summary>
/// テキスト内の文字位置を検索。
/// </summary>
/// <param name="haystack">検索対象テキスト。</param>
/// <param name="needle">検索文字。</param>
/// <returns>一致文字のインデックス。見つからない場合は0未満。</returns>
ssize_t indexOfCharacter(const TEXT* haystack, TCHAR needle);

/// <summary>
/// テキスト比較。
/// </summary>
/// <param name="a">比較対象テキスト1。</param>
/// <param name="b">比較対象テキスト2。</param>
/// <param name="ignoreCase">大文字小文字を無視するか。</param>
/// <returns>a &lt; b: 負, a = b: 0, a &gt; b: 正。</returns>
int compareText(const TEXT* a, const TEXT* b, bool ignoreCase);

/// <summary>
/// 詳細版テキスト比較。
/// <para>CompareStringばんざーい</para>
/// </summary>
/// <param name="a">比較対象テキスト1。</param>
/// <param name="b">比較対象テキスト2。</param>
/// <param name="width">比較サイズ。1以上を指定した際に比較対象テキスト長を超過した場合はテキスト長に補正される。0未満を指定した場合は対象テキスト長を使用する。0を指定した場合、テキスト長の短い方が使用される。</param>
/// <param name="mode">比較方法。組み合わせて使用。</param>
/// <param name="locale">ロケール。</param>
/// <returns>比較結果。</returns>
TEXT_COMPARE_RESULT compareTextDetail(const TEXT* a, const TEXT* b, ssize_t width, TEXT_COMPARE_MODE mode, LOCALE_TYPE locale);

/// <summary>
/// 指定のテキストで始まるか。
/// </summary>
/// <param name="text">対象テキスト。</param>
/// <param name="word">検索テキスト。</param>
/// <returns>始まる場合に真。</returns>
bool startsWithText(const TEXT* text, const TEXT* word);
