using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    /// <summary>
    ///     Factura del pago
    /// </summary>
    [DataContract]
    public class VoucherDTO
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
        public VoucherTypeDTO Type { get; set; }

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
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        ///     ExchangeRate
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// VoucherConcepts
        /// </summary>
        [DataMember]
        public List<VoucherConceptDTO> VoucherConcepts { get; set; }
		
    }
}
