using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
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
        /// Obtiene o Setea Indicando <see cref="IndividualPaymentMethod"/> Si esta Habilitado
        /// </summary>
        /// <value>
        ///   <c>true</c> Si esta Habilitado; Deshabilitado, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
