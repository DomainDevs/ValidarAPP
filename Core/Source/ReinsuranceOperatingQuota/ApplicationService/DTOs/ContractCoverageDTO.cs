using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceOperatingQuotaServices.DTOs
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
    }
}