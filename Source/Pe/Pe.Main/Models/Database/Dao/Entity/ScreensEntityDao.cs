using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class ScreensRowDto : CommonDtoBase
    {
        #region property

        public string ScreenName { get; set; } = string.Empty;
        [PixelKind(Px.Device)]
        public long ScreenX { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenY { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenWidth { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenHeight { get; set; }

        #endregion
    }

    public class ScreensEntityDao : EntityDaoBase
    {
        public ScreensEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public bool SelectExistsScreen(string? screenName)
        {
            var statement = LoadStatement();
            var param = new {
                ScreenName = screenName ?? string.Empty,
            };
            return Commander.QuerySingle<bool>(statement, param);
        }

        public bool InsertScreen(IScreen screen, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new ScreensRowDto() {
                ScreenName = screen.DeviceName,
                ScreenX = (long)screen.DeviceBounds.X,
                ScreenY = (long)screen.DeviceBounds.Y,
                ScreenWidth = (long)screen.DeviceBounds.Width,
                ScreenHeight = (long)screen.DeviceBounds.Height,
            };
            commonStatus.WriteCommon(dto);

            return Commander.Execute(statement, dto) == 1;
        }

        #endregion
    }
}