using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs
{
    [DataContract]
    public class AmountDTO
    {
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }
}
