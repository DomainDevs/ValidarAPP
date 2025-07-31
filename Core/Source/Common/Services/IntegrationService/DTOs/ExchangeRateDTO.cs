using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.CommonServices.DTOs
{
    public class ExchangeRateDTO
    {
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        [DataMember]
        public DateTime RateDate { get; set; }

        /// <summary>
        /// Importe de venta
        /// </summary>
        [DataMember]
        public decimal SellAmount { get; set; }

        /// <summary>
        /// Importe de compra
        /// </summary>
        [DataMember]
        public decimal BuyAmount { get; set; }
    }
}
