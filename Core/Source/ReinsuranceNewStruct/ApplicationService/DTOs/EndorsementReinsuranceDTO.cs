using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class EndorsementReinsuranceDTO
    {
        [DataMember]
        public int IssueLayerId { get; set; }

        [DataMember]
        public int ReinsuranceNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime ProcessDate { get; set; }

        [DataMember]
        public int LayerNumber { get; set; }

        [DataMember]
        public decimal LayerPercentage { get; set; }

        [DataMember]
        public decimal PremiumPercentage { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public DateTime ValidityFrom { get; set; }

        [DataMember]
        public DateTime ValidityTo { get; set; }

        [DataMember]
        public string IsAutomatic { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal Premium { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }
    }
}
