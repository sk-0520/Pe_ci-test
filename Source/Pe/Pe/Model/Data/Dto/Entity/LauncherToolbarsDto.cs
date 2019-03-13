using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    public class LauncherToolbarsScreenRowDto : CommonDtoBase
    {
        #region property

        public Guid LauncherToolbarId { get; set; }

        public string ScreenName { get; set; }
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

    public class LauncherToolbarsDisplayRowDto : CommonDtoBase
    {
        #region property

        public Guid LauncherToolbarId { get; set; }
        public Guid LauncherGroupId { get; set; }
        public string PositionKind { get; set; }
        public string IconKind { get; set; }
        public Guid FontId { get; set; }
        public string AutoHideTimeout { get; set; }
        public long TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }

        #endregion
    }
}
