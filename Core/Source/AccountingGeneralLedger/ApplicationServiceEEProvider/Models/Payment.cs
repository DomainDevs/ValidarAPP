using System.Runtime.Serialization;
using System.Collections.Generic;


namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Pago
    /// </summary>
    [DataContract]
    public class Payment
    {

        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Método de pago
        /// </summary>
        [DataMember]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        ///     Importe de pago
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        ///     Estado del Pago
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        ///     Total Impuestos
        /// </summary>
        [DataMember]
        public decimal Tax { get; set; }

        /// <summary>
        ///     Total Retenciones
        /// </summary>
        [DataMember]
        public decimal Retention { get; set; }

        /// <summary>
        ///     Impuestos a aplicar
        /// </summary>
        [DataMember]
        public List<PaymentTax> Taxes { get; set; }
    }
}
