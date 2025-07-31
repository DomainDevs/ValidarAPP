using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.View
{
    [Serializable()]
    public class BelongingToIndividualIdView : BusinessView
    {
        public BusinessCollection OperationQuotaEvents
        {
            get
            {
                return this["OperationQuotaEvent"];
            }
        }

        public BusinessCollection ConsortiumEvents
        {
            get
            {
                return this["ConsortiumEvent"];
            }
        }

        public BusinessCollection EconomicGroupEvents
        {
            get
            {
                return this["EconomicGroupEvent"];
            }
        }
    }
}
