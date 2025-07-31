using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingOperatingQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class ApplyEndorsementDTO
    {   
        [DataMember]
        public int PolicyID { get; set; }

        [DataMember]
        public int Endorsement { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int EndorsementType { get; set; }

        [DataMember]
        public decimal AmountCoverage { get; set; }

        [DataMember]
        public int CurrencyType { get; set; }

        [DataMember]
        public string CurrencyTypeDesc { get; set; }

        [DataMember]
        public decimal ParticipationPercentage { get; set; }
    }
}
