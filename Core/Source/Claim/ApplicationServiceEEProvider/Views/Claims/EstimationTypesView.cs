using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims
{
    [Serializable()]
    public class EstimationTypesView : BusinessView
    {
        public BusinessCollection EstimationTypePrefixies
        {
            get
            {
                return this["EstimationTypePrefix"];
            }
        }

        public BusinessCollection EstimationTypes
        {
            get
            {
                return this["EstimationType"];
            }
        }
    }
}
