﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContentTypeTextNet.Pe.Main.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ContentTypeTextNet.Pe.Main.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   &lt;FlowDocument
        ///  xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot;
        ///  xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot;
        ///&gt;
        ///  &lt;FlowDocument.Resources&gt;
        ///    &lt;Style x:Key=&quot;Header&quot; TargetType=&quot;Paragraph&quot;&gt;
        ///      &lt;Setter Property=&quot;Margin&quot; Value=&quot;0, 20, 0, 10&quot; /&gt;
        ///      &lt;Setter Property=&quot;FontSize&quot; Value=&quot;16pt&quot; /&gt;
        ///      &lt;Setter Property=&quot;FontWeight&quot; Value=&quot;Bold&quot; /&gt;
        ///    &lt;/Style&gt;
        ///  &lt;/FlowDocument.Resources&gt;
        ///
        ///  &lt;Paragraph
        ///    FontSize=&quot;20pt&quot;
        ///    FontWeight=&quot;Bold&quot;
        ///    TextDecorations=&quot;Und [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string File_Accept_AcceptDocument {
            get {
                return ResourceManager.GetString("File_Accept_AcceptDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;SyntaxDefinition xmlns=&quot;http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008&quot; name=&quot;EnvVar_Update&quot;&gt;
        ///	&lt;!--TODO:キーと値で分離--&gt;
        ///	&lt;Color name=&quot;Key&quot; fontWeight=&quot;bold&quot; foreground=&quot;Blue&quot; /&gt;
        ///	&lt;Color name=&quot;Variable&quot; foreground=&quot;Maroon&quot; /&gt;
        ///	&lt;RuleSet ignoreCase=&quot;true&quot;&gt;
        ///		&lt;Rule color=&quot;Key&quot;&gt;
        ///			\w+
        ///		&lt;/Rule&gt;
        ///	&lt;/RuleSet&gt;
        ///&lt;/SyntaxDefinition&gt;
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string File_Highlighting_EnvironmentVariable_Merge {
            get {
                return ResourceManager.GetString("File_Highlighting_EnvironmentVariable_Merge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;SyntaxDefinition xmlns=&quot;http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008&quot; name=&quot;EnvVar_Remove&quot;&gt;
        ///	&lt;!--TODO:キーに限定--&gt;
        ///	&lt;Color name=&quot;Key&quot; fontWeight=&quot;bold&quot; foreground=&quot;Blue&quot; /&gt;
        ///	&lt;RuleSet ignoreCase=&quot;true&quot;&gt;
        ///		&lt;Rule color=&quot;Key&quot;&gt;
        ///			\w+
        ///		&lt;/Rule&gt;
        ///	&lt;/RuleSet&gt;
        ///&lt;/SyntaxDefinition&gt;
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string File_Highlighting_EnvironmentVariable_Remove {
            get {
                return ResourceManager.GetString("File_Highlighting_EnvironmentVariable_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;SyntaxDefinition xmlns=&quot;http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008&quot; name=&quot;EnvVar_Remove&quot;&gt;
        ///	&lt;!--きちんとする--&gt;
        ///	&lt;Color name=&quot;Key&quot; /&gt;
        ///	&lt;RuleSet ignoreCase=&quot;true&quot;&gt;
        ///		&lt;Rule color=&quot;Key&quot;&gt;
        ///			\w+
        ///		&lt;/Rule&gt;
        ///	&lt;/RuleSet&gt;
        ///
        ///&lt;/SyntaxDefinition&gt;
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string File_Highlighting_Tag {
            get {
                return ResourceManager.GetString("File_Highlighting_Tag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   使用許諾  に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Accept_Caption {
            get {
                return ResourceManager.GetString("String_Accept_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   使用する に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Accept_Command_Affirmative {
            get {
                return ResourceManager.GetString("String_Accept_Command_Affirmative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   使用しない(N) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Accept_Command_Negative {
            get {
                return ResourceManager.GetString("String_Accept_Command_Negative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   使用する に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Accept_PlaceHolder_Affirmative {
            get {
                return ResourceManager.GetString("String_Accept_PlaceHolder_Affirmative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   使用しない に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Accept_PlaceHolder_Negative {
            get {
                return ResourceManager.GetString("String_Accept_PlaceHolder_Negative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   情報(_A) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_About {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_About", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ヘルプ(_H) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_About_Help {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_About_Help", resourceCulture);
            }
        }
        
        /// <summary>
        ///   バージョン情報(_V) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_About_Version {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_About_Version", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コマンド(_C) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Command {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Command", resourceCulture);
            }
        }
        
        /// <summary>
        ///   終了(_X) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Exit {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Exit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ノート(_N) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note", resourceCulture);
            }
        }
        
        /// <summary>
        ///   最後面へ移動(_B) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note_Bottom {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note_Bottom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   最小化(_N) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note_Compact {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note_Compact", resourceCulture);
            }
        }
        
        /// <summary>
        ///   新規作成(_C) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note_Create {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   非表示(_H) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note_Hidden {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note_Hidden", resourceCulture);
            }
        }
        
        /// <summary>
        ///   最前面へ移動(_T) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note_Topmost {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note_Topmost", resourceCulture);
            }
        }
        
        /// <summary>
        ///   表示中(_V) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Note_Visible {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Note_Visible", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Pe 設定(_S) ... に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Setting {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Setting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ツールバー(_T) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_AppManager_Menu_Toolbar {
            get {
                return ResourceManager.GetString("String_AppManager_Menu_Toolbar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   キャンセル(_C) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Common_Command_Cancel {
            get {
                return ResourceManager.GetString("String_Common_Command_Cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   閉じる(_X) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Common_Command_Close {
            get {
                return ResourceManager.GetString("String_Common_Command_Close", resourceCulture);
            }
        }
        
        /// <summary>
        ///   OK に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Common_Command_Ok {
            get {
                return ResourceManager.GetString("String_Common_Command_Ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ランチャーアイテム取り込み に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_ImportPrograms_Caption {
            get {
                return ResourceManager.GetString("String_ImportPrograms_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   取り込み(_I) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_ImportPrograms_Command_Import {
            get {
                return ResourceManager.GetString("String_ImportPrograms_Command_Import", resourceCulture);
            }
        }
        
        /// <summary>
        ///   自動設定グループ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_LauncherGroup_ImportItem_Name {
            get {
                return ResourceManager.GetString("String_LauncherGroup_ImportItem_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   新しいグループ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_LauncherGroup_NewItem_Name {
            get {
                return ResourceManager.GetString("String_LauncherGroup_NewItem_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   新しいアイテム に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_LauncherItem_NewItem_Name {
            get {
                return ResourceManager.GetString("String_LauncherItem_NewItem_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   command に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_Command {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_Command", resourceCulture);
            }
        }
        
        /// <summary>
        ///   execute に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_Execute {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_Execute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   general に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_General {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_General", resourceCulture);
            }
        }
        
        /// <summary>
        ///   note に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_Note {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_Note", resourceCulture);
            }
        }
        
        /// <summary>
        ///   stdio に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_StandardInputOutput {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_StandardInputOutput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   update に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_Update {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   window に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_General_Header_Window {
            get {
                return ResourceManager.GetString("String_Setting_General_Header_Window", resourceCulture);
            }
        }
        
        /// <summary>
        ///   general に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_Header_General {
            get {
                return ResourceManager.GetString("String_Setting_Header_General", resourceCulture);
            }
        }
        
        /// <summary>
        ///   keyboard に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_Header_Keyboard {
            get {
                return ResourceManager.GetString("String_Setting_Header_Keyboard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   groups に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_Header_LauncherGroups {
            get {
                return ResourceManager.GetString("String_Setting_Header_LauncherGroups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   items に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_Header_LauncherItems {
            get {
                return ResourceManager.GetString("String_Setting_Header_LauncherItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   toolbars に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Setting_Header_LauncherToolbars {
            get {
                return ResourceManager.GetString("String_Setting_Header_LauncherToolbars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ${ITEM} の標準入出力 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Caption {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   自動スクロール に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Command_AutoScroll {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Command_AutoScroll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   クリア に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Command_Clear {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Command_Clear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   終了 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Command_Kill {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Command_Kill", resourceCulture);
            }
        }
        
        /// <summary>
        ///   保存 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Command_Save {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Command_Save", resourceCulture);
            }
        }
        
        /// <summary>
        ///   最前面 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Command_Topmost {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Command_Topmost", resourceCulture);
            }
        }
        
        /// <summary>
        ///   折り返し表示 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Command_WordWrap {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Command_WordWrap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   終了コード: に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_ExitCode {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_ExitCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   実行中 ... に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_Running {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_Running", resourceCulture);
            }
        }
        
        /// <summary>
        ///   標準入出力 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_StandardInputOutput_Header {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_StandardInputOutput_Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   入力 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_StandardInputOutput_StandardInputOutput_Send {
            get {
                return ResourceManager.GetString("String_StandardInputOutput_StandardInputOutput_Send", resourceCulture);
            }
        }
        
        /// <summary>
        ///   スタート に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Startup_Caption {
            get {
                return ResourceManager.GetString("String_Startup_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   プログラム自動取り込み(_P) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Startup_Command_ImportPrograms {
            get {
                return ResourceManager.GetString("String_Startup_Command_ImportPrograms", resourceCulture);
            }
        }
        
        /// <summary>
        ///   通知領域設定(_N) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Startup_Command_NotifyArea {
            get {
                return ResourceManager.GetString("String_Startup_Command_NotifyArea", resourceCulture);
            }
        }
        
        /// <summary>
        ///   スタートアップ登録(_S) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string String_Startup_Command_RegisterStartup {
            get {
                return ResourceManager.GetString("String_Startup_Command_RegisterStartup", resourceCulture);
            }
        }
    }
}
