using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class AppCommandSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string IconBox { get; set; } = string.Empty;
        public TimeSpan HideWaitTime { get; set; }
        public bool FindTag { get; set; }
        public bool FindFile { get; set; }

        #endregion
    }
}
