using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// AffectationType
    /// </summary>
    [DataContract]
    public class AffectationTypeDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripci�n del tipo de contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
    }
}