using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class IssuanceCommission : BaseCommission
    {
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        [DataMember]    
        public Decimal? AgentPercentage { get; set; }
    }
}