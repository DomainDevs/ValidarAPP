using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseExchangeRate : Extension
    {
        /// <summary>
        /// Fecha de importe
        /// </summary>
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
