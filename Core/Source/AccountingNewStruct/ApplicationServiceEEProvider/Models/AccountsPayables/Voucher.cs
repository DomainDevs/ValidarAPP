using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
  

    /// <summary>
    ///     Factura del pago
    /// </summary>
    [DataContract]
    public class Voucher
    {
        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Tipo
        /// </summary>
        [DataMember]
        public VoucherType Type { get; set; }

        /// <summary>
        ///     Numero
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        ///     Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        ///     ExchangeRate
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// VoucherConcepts
        /// </summary>
        [DataMember]
        public List<VoucherConcept> VoucherConcepts { get; set; }
		
    }
}
