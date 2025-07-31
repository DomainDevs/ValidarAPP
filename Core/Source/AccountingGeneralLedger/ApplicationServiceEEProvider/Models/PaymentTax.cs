using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
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
