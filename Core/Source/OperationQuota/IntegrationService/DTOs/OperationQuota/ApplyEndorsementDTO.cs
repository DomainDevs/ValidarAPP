using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class ApplyEndorsementDTO
    {   
        [DataMember]
        public int PolicyID { get; set; }

        [DataMember]
        public int Endorsement { get; set; }

        [DataMember]
        public int RiskId { get; set; }

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

        [DataMember]
        public DateTime Policy_Init_Date { get; set; }

        [DataMember]
        public DateTime Policy_End_Date { get; set; }

        [DataMember]
        public DateTime Cov_Init_Date { get; set; }

        [DataMember]
        public DateTime Cov_End_Date { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public bool IsSeriousOffer { get; set; }

        [DataMember]
        public bool IsConsortium { get; set; }
    }
}
