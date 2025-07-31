using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CoInsuranceDTO
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public decimal Participation { get; set; }

        [DataMember]
        public decimal ParticipationOwn { get; set; }

        [DataMember]
        public decimal SumAssuredAmount { get; set; }

        [DataMember]
        public decimal PremiumAmount { get; set; }
    }
}
