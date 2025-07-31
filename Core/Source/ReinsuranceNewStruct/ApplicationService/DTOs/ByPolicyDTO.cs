using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Póliza
    /// </summary>
    [DataContract]
    public class ByPolicyDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Póliza
        /// </summary>
        [DataMember]
        public PolicyDTO Policy { get; set; }
    }
}