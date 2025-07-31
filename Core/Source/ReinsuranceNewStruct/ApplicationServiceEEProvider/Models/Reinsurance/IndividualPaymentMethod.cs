using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Metodos de Pago individuo
    /// </summary>
    [DataContract]
    public class IndividualPaymentMethod
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

        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o Setea Indicando <see cref="IndividualPaymentMethod"/> Si esta Habilitado
        /// </summary>
        /// <value>
        ///   <c>true</c> Si esta Habilitado; Deshabilitado, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsEnabled { get; set; }


    }
}
