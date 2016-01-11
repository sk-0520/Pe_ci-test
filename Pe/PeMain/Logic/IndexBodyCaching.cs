﻿/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using Library.PeData.Define;
    using Utility;
    public class IndexBodyCaching: DisposeFinalizeBase
    {
        public IndexBodyCaching(int templateLimit, int clipboardLimit, VariableConstants variableConstants)
        {
            if(templateLimit <= 0) {
                templateLimit = Constants.indexBodyCachingSize;
            }
            if(clipboardLimit <= 0) {
                clipboardLimit = Constants.indexBodyCachingSize;
            }

            NoteItems = new IndexBodyPairItemCollection<NoteBodyItemModel>(0);
            TemplateItems = new IndexBodyPairItemCollection<TemplateBodyItemModel>(templateLimit);
            ClipboardItems = new IndexBodyPairItemCollection<ClipboardBodyItemModel>(clipboardLimit);

            NoteArchive.OpenIfExists(IndexKind.Note, variableConstants);
            TemplateArchive.OpenIfExists(IndexKind.Template, variableConstants);
            ClipboardArchive.OpenIfExists(IndexKind.Clipboard, variableConstants);

            NoteItems.StockRemovedItem = true;
            TemplateItems.StockRemovedItem = true;
            ClipboardItems.StockRemovedItem = true;
        }

        #region property

        public IndexBodyPairItemCollection<NoteBodyItemModel> NoteItems { get; private set; }
        public IndexBodyArchive NoteArchive { get; } = new IndexBodyArchive();
        public IndexBodyPairItemCollection<TemplateBodyItemModel> TemplateItems { get; private set; }
        public IndexBodyArchive TemplateArchive { get; } = new IndexBodyArchive();
        public IndexBodyPairItemCollection<ClipboardBodyItemModel> ClipboardItems { get; private set; }
        public IndexBodyArchive ClipboardArchive { get; } = new IndexBodyArchive();

        #endregion

        #region function

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                var archives = new[] {
                    NoteArchive,
                    TemplateArchive,
                    ClipboardArchive,
                };
                foreach(var archive in archives) {
                    archive.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
