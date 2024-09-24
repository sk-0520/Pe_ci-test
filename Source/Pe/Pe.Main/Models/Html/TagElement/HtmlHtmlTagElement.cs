using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Html.TagElement
{
    public sealed class HtmlHtmlTagElement: HtmlTagElement
    {
        public HtmlHtmlTagElement(HtmlDocument document)
            : base("html", document)
        { }

        #region property

        #endregion

        #region function

        #endregion

        #region HtmlTagElement

        public override bool IsInline => false;
        public override bool IsVoid => false;

        #endregion
    }
}
