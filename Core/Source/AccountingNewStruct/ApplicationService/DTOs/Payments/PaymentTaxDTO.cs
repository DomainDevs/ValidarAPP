using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{ 
    /// <summary>
    ///     Impuestos de Pago
    /// </summary>
    [DataContract]
    public class PaymentTaxDTO
    {
        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Impuesto
        /// </summary>
        [DataMember]
        public TaxDTO Tax { get; set; }

        /// <summary>
        ///     Tarifa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        ///     Importe Base para calculo del Impuesto
        /// </summary>
        [DataMember]
        public AmountDTO TaxBase { get; set; }

        /// <summary>
        ///     Sucursal
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }
    }
}

