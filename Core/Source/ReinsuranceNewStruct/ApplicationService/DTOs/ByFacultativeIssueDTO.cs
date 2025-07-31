using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas: Facultativo Por Emision
    /// </summary>
    [DataContract]
    public class ByFacultativeIssueDTO : LineAssociationTypeDTO
    {
       
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public List<PrefixDTO> Prefixes { get; set; }
    }
}