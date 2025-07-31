using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models.Base;


namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.Models
{
    [DataContract]
    public class CompanyEndorsementCreditNote : BaseEndorsementType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool HasQuotation { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public string DescriptionRisk { get; set; }

        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public string DescriptionCoverage { get; set; }
        [DataMember]
        public int EndorsementId { get; set;}
        [DataMember]
        public DateTime CurrentFrom { get;set;}
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public int? Days { get; set; }
    }
}
