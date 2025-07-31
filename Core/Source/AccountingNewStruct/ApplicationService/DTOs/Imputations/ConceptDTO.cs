using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Concept:   Concepto
    /// </summary>
    [DataContract]
    public class ConceptDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Description 
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        
    }
}
