using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CollectMassiveProcessDTO
    {
        [DataMember]
        public int CollectMassiveProcessId { get; set; }
        [DataMember]
        public DateTime BeginDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public int SuccessfulRecords { get; set; }
        [DataMember]
        public int FailedRecords { get; set; }
        [DataMember]
        public bool isValid { get; set; }
        [DataMember]
        public bool TechnicalTransaction { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int QuotaNumber { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public int PolicyNumber { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
    }
}
