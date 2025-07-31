using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.MassiveServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class MassiveLoadView : BusinessView
    {
        public BusinessCollection MassiveLoads
        {
            get
            {
                return this["MassiveLoad"];
            }
        }

        public BusinessCollection LoadTypes
        {
            get
            {
                return this["LoadType"];
            }
        }
    }
}