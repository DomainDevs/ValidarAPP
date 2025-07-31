using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas: por Asegurado Ramo
    /// </summary>
    [DataContract]
    public class ByInsuredPrefixDTO : LineAssociationTypeDTO
    {

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public IndividualDTO Insured { get; set; }


        /// <summary>
        /// Ramos 
        /// </summary>
        [DataMember]
        public List<PrefixDTO> Prefixes { get; set; }
    }
}