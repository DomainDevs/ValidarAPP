using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    /// <summary>
    /// Metodos de Pago
    /// </summary>
    [DataContract]
    public class BaseIndividualPaymentMethod : Extension
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o Setea el Id Pago
        /// </summary>
        /// <value>
        /// Id Pago
        /// </value>
        [DataMember]
        public int PaymentId { get; set; }

        /// <summary>
        /// Obtiene o Setea el Id Individuo
        /// </summary>
        /// <value>
        /// Id Individuo
        /// </value>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o Setea el Role
        /// </summary>
        /// <value>
        /// Role
        /// </value>
        [DataMember]
        public int RoleId { get; set; }

        /// <summary>
        /// Obtiene o Setea Indicando <see cref="IndividualPaymentMethod"/> Si esta Habilitado
        /// </summary>
        /// <value>
        ///   <c>true</c> Si esta Habilitado; Deshabilitado, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
