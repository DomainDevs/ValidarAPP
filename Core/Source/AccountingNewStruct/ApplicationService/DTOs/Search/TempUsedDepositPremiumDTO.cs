using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class TempUsedDepositPremiumDTO 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int TempPremiumReceivableItemId { get; set; }
        [DataMember]
        public int DepositPremiumTransactionId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
