using System.Runtime.Serialization;

//Sistran

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PremiumReceivableItemDTO : PremiumReceivableSearchPolicyDTO
    {
        [DataMember]
        public int PremiumReceivableItemId { get; set; } //id de Item.
        [DataMember]
        public int ImputationId { get; set; }
        [DataMember]
        public decimal PayableAmount { get; set; }
        [DataMember]
        public int Upd { get; set; } //flag que indica si usa primas en depósito
        [DataMember]
        public int AccountingTransaction { get; set; } //número de transacción contable
        /// <summary>
        /// Si es una reversion de la prima
        /// </summary>
        /// <value>
        /// The accounting transaction.
        /// </value>
        [DataMember]
        public bool IsReversion { get; set; }
    }
}
