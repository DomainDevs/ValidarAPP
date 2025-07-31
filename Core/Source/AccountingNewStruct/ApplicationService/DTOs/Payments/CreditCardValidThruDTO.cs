using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    ///     Validar tarjeta de Credito
    /// </summary>
    [DataContract]
    public class CreditCardValidThruDTO
    {
      
        /// <summary>
        ///     Año
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        ///     Mes
        /// </summary>
        [DataMember]
        public int Month { get; set; }
    }
}
