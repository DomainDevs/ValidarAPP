using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PolicyTypeView : BusinessView
    {
        public BusinessCollection CoPolicyTypes
        {
            get
            {
                return this["CoPolicyType"];
            }
        }

        public BusinessCollection CoProductPolicyTypes
        {
            get
            {
                return this["CoProductPolicyType"];
            }
        }
    }
}
