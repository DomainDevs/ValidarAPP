using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Metodos de Pago individuo
    /// </summary>
    [DataContract]
    public class IndividualPaymentMethod : BaseIndividualPaymentMethod
    {
        /// <summary>
        /// PaymentMethodAccount
        /// </summary>
        [DataMember]
        public PaymentAccount Account { get; set; }

        /// <summary>
        /// PaymentMethod
        /// </summary>
        [DataMember]
        public PaymentMethod Method { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        [DataMember]
        public Role Rol { get; set; }

    }
}
