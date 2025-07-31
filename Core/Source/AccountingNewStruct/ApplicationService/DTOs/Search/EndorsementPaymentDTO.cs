using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class EndorsementPaymentDTO : PremiumReceivableSearchPolicyDTO
    {
        [DataMember]
        public int ImputationId { get; set; } //imputación - número de transacción 
    }
}
