using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    

    /// <summary>
    ///     Validar tarjeta de Credito
    /// </summary>
    [DataContract]
    public class CreditCardValidThru
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
