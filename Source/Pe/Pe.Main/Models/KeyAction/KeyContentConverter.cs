using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyContentConverterBase
    {
        #region function

        protected Key ToKey(string content)
        {
            var keyConverter = new KeyConverter();
            return (Key)keyConverter.ConvertFromInvariantString(content);
        }

        #endregion
    }

    public sealed class ReplaceContentConverter : KeyContentConverterBase
    {
        #region function

        public Key ToReplaceKey(string content) => ToKey(content);

        public string ToContent(Key key) => key.ToString();

        #endregion
    }

    public sealed class LauncherItemContentConverter : KeyContentConverterBase
    {
        #region function

        public KeyActionContentLauncherItem ToKeyActionContentLauncherItem(string content)
        {
            return EnumUtility.Parse<KeyActionContentLauncherItem>(content);
        }

        public string ToContent(KeyActionContentLauncherItem keyActionContentLauncherItem)
        {
            return keyActionContentLauncherItem.ToString();
        }

        #endregion
    }

    public sealed class LauncherToolbarContentConverter : KeyContentConverterBase
    {
        #region function

        public KeyActionContentLauncherToolbar ToKeyActionContentLauncherToolbar(string content)
        {
            return EnumUtility.Parse<KeyActionContentLauncherToolbar>(content);
        }

        public string ToContent(KeyActionContentLauncherToolbar keyActionContentLauncherToolbar)
        {
            return keyActionContentLauncherToolbar.ToString();
        }

        #endregion
    }

    public sealed class NoteContentConverter : KeyContentConverterBase
    {
        #region function

        public KeyActionContentNote ToKeyActionContentNote(string content)
        {
            return EnumUtility.Parse<KeyActionContentNote>(content);
        }

        public string ToContent(KeyActionContentNote keyActionContentNote)
        {
            return keyActionContentNote.ToString();
        }

        #endregion
    }

}