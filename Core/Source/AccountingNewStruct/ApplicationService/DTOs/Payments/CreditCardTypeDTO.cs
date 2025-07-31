using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    /// Tipo de Tarjeta de Credito
    /// </summary>
    [DataContract]
    public class CreditCardTypeDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Decripción del conducto
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Comisión del conducto
        /// </summary>
        [DataMember]
        public decimal Commission { get; set; }
    }
}
