using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// ResettlementType
    /// </summary>
    [DataContract]
    public class ResettlementTypeDTO
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