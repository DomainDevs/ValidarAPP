using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class ConceptType
    {
        /// <summary>
        /// propiedad ConceptTypeId
        /// </summary>
        [DataMember]
        public int ConceptTypeId { get; set; }

        /// <summary>
        /// propiedad Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}