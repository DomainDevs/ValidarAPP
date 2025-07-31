using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    
    /// <summary>
    ///     Impuestos de Pago
    /// </summary>
    [DataContract]
    public class PaymentTax
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
        public Tax Tax { get; set; }

        /// <summary>
        ///     Tarifa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        ///     Importe Base para calculo del Impuesto
        /// </summary>
        [DataMember]
        public Amount TaxBase { get; set; }

        /// <summary>
        ///     Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }
    }
}

