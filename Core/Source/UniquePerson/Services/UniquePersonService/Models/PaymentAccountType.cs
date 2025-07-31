using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Cuentas Metodos de pago
    /// </summary>
    [DataContract]
    public class PaymentAccountType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
