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
    public class CoveragePaymentConceptsView : BusinessView
    {
        public BusinessCollection PaymentConcepts
        {
            get
            {
                return this["PaymentConcept"];
            }
        }
        public BusinessCollection CoveragePaymentConcepts
        {
            get
            {
                return this["CoveragePaymentConcept"];
            }
        }
    }
}
