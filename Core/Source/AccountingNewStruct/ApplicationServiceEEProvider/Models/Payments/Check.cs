using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    /// <summary>
    /// Pago en cheque
    /// </summary>   
    [DataContract]
    public class Check : Payment
    {

        /// <summary>
        /// Banco Emisor
        /// </summary>
        [DataMember]
        public Bank IssuingBank { get; set; }
        
        /// <summary>
        /// Nombre Emisor del cheque
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }
    }
}
