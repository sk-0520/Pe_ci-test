using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppExecuteSettingEntityDao : EntityDaoBase
    {
        public AppExecuteSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public SettingAppExecuteSettingData SelectSettingExecuteSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppExecuteSettingEntityDto>(statement);
            var result = new SettingAppExecuteSettingData() {
                UserId = dto.UserId,
                SendUsageStatistics = dto.SendUsageStatistics,
            };
            return result;
        }


        #endregion
    }
}
