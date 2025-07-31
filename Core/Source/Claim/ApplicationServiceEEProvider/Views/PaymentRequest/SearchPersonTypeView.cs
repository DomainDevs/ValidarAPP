using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class SearchPersonTypeView : BusinessView
    {

        public BusinessCollection PersonType
        {
            get
            {
                return this["PersonType"];
            }
        }
        public BusinessCollection ClaimSearchPersonType
        {
            get
            {
                return this["ClaimSearchPersonType"];
            }
        }
    }
}
