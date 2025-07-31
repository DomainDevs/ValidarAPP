using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Vehicles.EEProvider.Entities.Views
{
    public class RiskSuretyGuaranteeView : BusinessView
    {
        public BusinessCollection RiskSuretyGuarantees
        {
            get
            {
                return this["RiskSuretyGuarantee"];
            }
        }

        public BusinessCollection InsuredGuarantees
        {
            get
            {
                return this["InsuredGuarantee"];
            }
        }
    }
}