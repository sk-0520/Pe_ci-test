using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class NoteContentsEntityDao : EntityDaoBase
    {
        public NoteContentsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function
        private NoteContentsEntityDto ConvertFromData(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var dto = new NoteContentsEntityDto() {
                NoteId = data.NoteId,
                ContentKind = noteContentKindTransfer.ToString(data.ContentKind),
                Content = data.Content,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelectExistsContent(Guid noteId, NoteContentKind contentKind)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
                ContentKind = noteContentKindTransfer.ToString(contentKind),
            };
            return Commander.QueryFirst<bool>(statement, param);
        }

        public string SelectFullContent(Guid noteId, NoteContentKind contentKind)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
                ContentKind = noteContentKindTransfer.ToString(contentKind),
            };
            return Commander.QueryFirst<string>(statement, param);
        }

        public bool InsertNewContent(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(data, databaseCommonStatus);
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateContent(NoteContentData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(data, databaseCommonStatus);
            return Commander.Execute(statement, param) == 1;
        }

        #endregion
    }
}
