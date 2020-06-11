using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Plugins.Clock.Models.Data
{
    public enum ClockWidgetKind
    {
        SimpleAnalog,
    }

    public class ClockWidgetSetting
    {
        #region property

        public ClockWidgetKind ClockWidgetKind { get; set; }
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;

        #endregion
    }
}
