using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PendingCommissionDTO
    {
        [DataMember]
        public decimal PendingCommission  { get; set; }
        [DataMember]
        public decimal CommissionPercentage { get; set; }
        [DataMember]
        public decimal AgentParticipationPercentage { get; set; }
        
      
    }
}
