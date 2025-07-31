using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas: por Asegurado
    /// </summary>
    [DataContract]
    public class ByInsuredDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public IndividualDTO Insured { get; set; }  
    }
}