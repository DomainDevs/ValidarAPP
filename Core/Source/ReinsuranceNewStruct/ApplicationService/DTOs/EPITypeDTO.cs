using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// EPIType
    /// </summary>
    [DataContract]
    public class EPITypeDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción del tipo de contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
    }
}