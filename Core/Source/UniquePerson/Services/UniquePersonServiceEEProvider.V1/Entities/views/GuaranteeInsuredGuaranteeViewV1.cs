using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class GuaranteeInsuredGuaranteeViewV1 : BusinessView
    {
        public BusinessCollection InsuredGuarantees
        {
            get
            {
                return this["InsuredGuarantee"];
            }
        }

        public BusinessCollection Guarantees
        {
            get
            {
                return this["Guarantee"];
            }
        }
    }
}
