using System.Runtime.Serialization;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredGuaranteeDocumentationDTO 
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


        /// <summary>
        /// Enumeracion para el codigo de la accion
        /// </summary>
        /// <value>
        /// Codigo de la accion
        /// </value>
        [DataMember]
        public ParametrizationStatus ParametrizationStatus { get; set; }


    }
}
