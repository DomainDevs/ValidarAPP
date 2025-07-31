using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPEN = Sistran.Core.Application.Temporary.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper
{
    public class TemporalPolicyMapper
    {
        public TMPEN.TempSubscription tempSubscription { get; set; }
        public TMPEN.TempSubscriptionPayer tempSubscriptionPayer { get; set; }
    }
}
