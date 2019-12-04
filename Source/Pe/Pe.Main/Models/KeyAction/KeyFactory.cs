using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public class KeyActionFactory
    {
        public KeyActionFactory(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }


        #endregion

        #region function

        KeyItem CreateKeyItem(KeyActionData keyAction, KeyOptionsEntityDao keyOptionsEntityDao, KeyMappingsEntityDao keyMappingsEntityDao)
        {
            var options = keyOptionsEntityDao.SelectOptions(keyAction.KeyActionId);
            var mappings = keyMappingsEntityDao.SelectMappings(keyAction.KeyActionId);

            var result = new KeyItem(
                keyAction,
                options.ToDictionary(i => i.Key, i => i.Value),
                mappings.ToList()
            );

            return result;
        }

        IReadOnlyList<KeyItem> LoadKeyItems(KeyActionKind keyActionKind)
        {
            var result = new List<KeyItem>();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var keyOptionsEntityDao = new KeyOptionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var keyMappingsEntityDao = new KeyMappingsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                foreach(var keyAction in keyActionsEntityDao.SelectAllKeyActionsFromKind(keyActionKind)) {
                    var keyItem = CreateKeyItem(keyAction, keyOptionsEntityDao, keyMappingsEntityDao);
                    result.Add(keyItem);
                }
            }

            return result;
        }

        IReadOnlyList<KeyItem> LoadKeyActionPressedData()
        {
            var result = new List<KeyItem>();

            var noPressedKinds = new[] { KeyActionKind.Replace, KeyActionKind.Disable };

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var keyOptionsEntityDao = new KeyOptionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var keyMappingsEntityDao = new KeyMappingsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                foreach(var keyAction in keyActionsEntityDao.SelectAllKeyActionsIgnoreKinds(noPressedKinds)) {
                    var keyItem = CreateKeyItem(keyAction, keyOptionsEntityDao, keyMappingsEntityDao);
                    result.Add(keyItem);
                }
            }

            return result;
        }

        IEnumerable<TJob> CreateJobs<TJob>(IReadOnlyList<KeyItem> items, Func<Guid, KeyItem, TJob> func)
        {
            foreach(var item in items) {
                TJob result;
                try {
                    result = func(item.Action.KeyActionId, item);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message + " {0}", item.Action.KeyActionId);
                    continue;
                }
                yield return result;
            }
        }

        //KeyMappingData ConvertKeyMapping(KeyActionData data)
        //{
        //    return new KeyMappingData() {
        //        Key = data.Key,
        //        Shift = data.Shift,
        //        Control = data.Contrl,
        //        Alt = data.Alt,
        //        Super = data.Super,
        //    };
        //}

        public IEnumerable<KeyActionReplaceJob> CreateReplaceJobs()
        {
            var items = LoadKeyItems(KeyActionKind.Replace);
            return CreateJobs(items, (id, item) => {
                //var replaceOptionConverter = new ReplaceOptionConverter();
                var keyConverter = new KeyConverter();
                var data = new KeyActionReplaceData(
                    item.Action.KeyActionId,
                    (Key)keyConverter.ConvertFromInvariantString(item.Action.KeyActionContent)
                );

                return new KeyActionReplaceJob(data, item.Mappings.First());
            });
        }

        public IEnumerable<KeyActionDisableJob> CreateDisableJobs()
        {
            var items = LoadKeyItems(KeyActionKind.Disable);
            return CreateJobs(items, (id, item) => {
                var disableOptionConverter = new DisableOptionConverter();
                var data = new KeyActionDisableData(
                    item.Action.KeyActionId,
                    disableOptionConverter.ToForever(item.Options)
                );

                return new KeyActionDisableJob(data, item.Mappings.First());
            });
        }

        KeyActionLauncherItemJob CreateLauncherItemJob(KeyItem item)
        {
            var keyLauncherItemContentConverter = new KeyLauncherItemContentConverter();
            var launcherItemOptionConverter = new LauncherItemOptionConverter();

            var data = new KeyActionLauncherItemData(
                item.Action.KeyActionId,
                keyLauncherItemContentConverter.ToKeyActionContentLauncherItem(item.Action.KeyActionContent),
                launcherItemOptionConverter.ToLauncherItemId(item.Options)
            );

            data.ConveySystem = launcherItemOptionConverter.ToConveySystem(item.Options);

            return new KeyActionLauncherItemJob(data, item.Mappings);
        }

        public IEnumerable<KeyActionPressedJobBase> CreatePressedJobs()
        {
            var items = LoadKeyActionPressedData();
            return CreateJobs(items, (id, item) => {
                KeyActionPressedJobBase job = item.Action.KeyActionKind switch
                {
                    KeyActionKind.LauncherItem => CreateLauncherItemJob(item),
                    _ => throw new NotImplementedException(),
                };
                return job;
            });
        }

        #endregion
    }

    public class KeyMappingFactory
    {
        #region property

        public int MappingStep { get; } = 10;

        #endregion
    }
}
