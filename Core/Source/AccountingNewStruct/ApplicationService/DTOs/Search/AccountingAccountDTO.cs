using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AccountingAccountDTO 
    {
        [DataMember]
        public int AccountingAccountId { get; set; }
        [DataMember]
        public string AccountingNumber { get; set; }
        [DataMember]
        public string AccountingName { get; set; }
        [DataMember]
        public int IsMulticurrency { get; set; }
        [DataMember]
        public int DefaultCurrency { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int AnalysisId { get; set; }
        [DataMember]
        public bool? RequireAnalysis { get; set; }

    }
}
