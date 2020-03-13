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
    internal class AppCommandSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string IconBox { get; set; } = string.Empty;
        public double Width { get; set; }
        public TimeSpan HideWaitTime { get; set; }
        public bool FindTag { get; set; }

        #endregion
    }

    public class AppCommandSettingEntityDao : EntityDaoBase
    {
        public AppCommandSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string Width { get; } = "Width";

            #endregion
        }

        #endregion

        #region function

        public Guid SelectCommandSettingFontId()
        {
            var statement = LoadStatement();
            return Commander.QueryFirst<Guid>(statement);
        }


        public SettingAppCommandSettingData SelectSettingCommandSetting()
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppCommandSettingEntityDto>(statement);
            var result = new SettingAppCommandSettingData() {
                FontId = dto.FontId,
                IconBox = iconBoxTransfer.ToEnum(dto.IconBox),
                Width = dto.Width,
                HideWaitTime = dto.HideWaitTime,
                FindTag = dto.FindTag,
            };
            return result;
        }


        public bool UpdateSettingCommandSetting(SettingAppCommandSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = new AppCommandSettingEntityDto() {
                FontId = data.FontId,
                IconBox = iconBoxTransfer.ToString(data.IconBox),
                Width = data.Width,
                HideWaitTime = data.HideWaitTime,
                FindTag = data.FindTag,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }

        public bool UpdatCommandSettingWidth(double width, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.Width] = width;
            return Commander.Execute(statement, parameter) == 1;
        }


        #endregion
    }
}