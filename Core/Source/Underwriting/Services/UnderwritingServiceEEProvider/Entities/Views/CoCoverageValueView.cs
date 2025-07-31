using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoCoverageValueView: BusinessView
    {
        public BusinessCollection Coverage
        {
            get
            {
                return this["Coverage"];
            }
        }
      
        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection CoCoverageValue
        {
            get
            {
                return this["CoCoverageValue"];
            }
        }

    }
}
