using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database.Vender.Public.SQLite
{
    public class SqliteImplementation: DatabaseImplementation
    {
        #region DatabaseImplementation

        public override bool SupportedTransactionDDL { get; } = true;

        #endregion
    }
}
