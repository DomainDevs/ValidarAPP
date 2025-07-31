using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Documentacion garantia Asegurado
    /// </summary>
    [DataContract]
    public class InsuredGuaranteeDocumentation : Extension
    {
        /// <summary>
        /// Obtiene o Setea El Id del Individuo
        /// </summary>
        /// <value>
        /// Id del Individuo
        /// </value>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o Setea El Id Garantia
        /// </summary>
        /// <value>
        /// Id Garantia
        /// </value>
        [DataMember]
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Obtiene o Setea El Codigo Fiador
        /// </summary>
        /// <value>
        /// Codigo Fiador
        /// </value>
        [DataMember]
        public int GuaranteeCode { get; set; }

        /// <summary>
        ///  Obtiene o Setea El Codigo de la Documentacion
        /// </summary>
        /// <value>
        /// Codigo de la Documentacion
        /// </value>
        [DataMember]
        public int DocumentCode { get; set; }
    }
}
