using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginSettingsEntityDao: EntityDaoBase
    {
        #region define

        private class PluginSettingDto: CommonDtoBase
        {
            #region property

            public PluginId PluginId { get; set; }
            public string PluginSettingKey { get; set; } = string.Empty;
            public string DataType { get; set; } = string.Empty;
            public string DataValue { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string DataType { get; } = "DataType";
            public static string DataValue { get; } = "DataValue";

            #endregion
        }

        #endregion

        public PluginSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginSettingRawValue ConvertFromDto(PluginSettingDto dto)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var data = new PluginSettingRawValue(
                pluginPersistentFormatTransfer.ToEnum(dto.DataType),
                dto.DataValue
            );
            return data;
        }

        private PluginSettingDto ConvertFromData(PluginId pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var dto = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
                DataType = pluginPersistentFormatTransfer.ToString(data.Format),
                DataValue = data.Value,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public bool SelecteExistsPluginSetting(PluginId pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Context.QueryFirst<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginSettingValue(PluginId pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            var dto = Context.QueryFirstOrDefault<PluginSettingDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertPluginSetting(PluginId pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, key, data, databaseCommonStatus);

            Context.InsertSingle(statement, parameter);
        }

        public void UpdatePluginSetting(PluginId pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, key, data, databaseCommonStatus);

            Context.UpdateByKey(statement, parameter);
        }

        public bool DeletePluginSetting(PluginId pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }

        public int DeleteAllPluginSettings(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
            };

            return Context.Delete(statement, parameter);
        }


        #endregion

    }
}
