using System.Runtime.Serialization;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class ContractCoverageDTO
    {
        [DataMember]
        public int ContractId { get; set; }
        [DataMember]
        public int ContractCompanyId { get; set; }
        [DataMember]
        public string ContractDescription { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal LevelLimit { get; set; }
        [DataMember]
        public int ContractCurrencyId { get; set; }
    }
}
