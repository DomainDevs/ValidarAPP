using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Concepto
    /// </summary>
    [DataContract]
    public class ReferenceConcept: Concept
    {
        /// <summary>
        /// Entidad Relacionada
        /// </summary>
        [DataMember]
        public int FEntityId { get; set; }

        /// <summary>
        /// Entidad Relacionada
        /// </summary>
        [DataMember]
        public List<EntityValue> EntityValues {get; set; }
    }
}
