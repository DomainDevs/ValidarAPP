using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class PaymentDTO
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
        public PaymentMethodDTO PaymentMethod { get; set; }

        /// <summary>
        ///     Importe de pago
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

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
        public List<PaymentTaxDTO> Taxes { get; set; }
    }
}
