using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class FontsRowDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public double Height { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsStrikeThrough { get; set; }

        #endregion
    }

    public class FontsEntityDao : EntityDaoBase
    {
        public FontsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string FontId { get; } = "FontId";
            public static string FamilyName { get; } = "FamilyName";
            public static string Height { get; } = "Height";
            public static string IsBold { get; } = "IsBold";
            public static string IsItalic { get; } = "IsItalic";
            public static string IsUnderline { get; } = "IsUnderline";
            public static string IsStrikeThrough { get; } = "IsStrikeThrough";

            #endregion
        }

        #endregion

        #region function

        FontData ConvertFromDto(FontsRowDto dto)
        {
            var data = new FontData() {
                FamilyName = dto.FamilyName,
                Size = dto.Height,
                IsBold = dto.IsBold,
                IsItalic = dto.IsItalic,
                IsUnderline = dto.IsUnderline,
                IsStrikeThrough = dto.IsStrikeThrough,
            };

            return data;
        }

        FontsRowDto ConvertFromData(IReadOnlyFontData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new FontsRowDto() {
                FamilyName = data.FamilyName,
                Height = data.Size,
                IsBold = data.IsBold,
                IsItalic = data.IsItalic,
                IsUnderline = data.IsUnderline,
                IsStrikeThrough = data.IsStrikeThrough,
            };
            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public FontData SelectFont(Guid fontId)
        {
            var statement = LoadStatement();
            var param = new {
                FontId = fontId,
            };
            var dto = Commander.QueryFirst<FontsRowDto>(statement, param);
            return ConvertFromDto(dto);
        }

        public bool InsertFont(Guid fontId, FontData fontData, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(fontData, databaseCommonStatus);
            param.FontId = fontId;
            return Commander.Execute(statement, param) == 1;
        }

        public bool InsertCopyFont(Guid sourceFontId, Guid destinationFontId, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter["SrcFontId"] = sourceFontId;
            parameter["DstFontId"] = destinationFontId;
            return Commander.Execute(statement, parameter) == 1;
        }


        public bool UpdateFamilyName(Guid fontId, string familyName, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.FamilyName] = familyName;
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateBold(Guid fontId, bool isBold, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.IsBold] = isBold;
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateItalic(Guid fontId, bool isItalic, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.IsItalic] = isItalic;
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateHeight(Guid fontId, double height, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.Height] = height;
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateFont(Guid fontId, FontData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(data, commonStatus);
            parameter.FontId = fontId;
            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}