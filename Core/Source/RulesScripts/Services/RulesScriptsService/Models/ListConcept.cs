using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class ListConcept:Concept
    {     
        /// <summary>
        /// Codigo de Listado de Entidades
        /// </summary>
        [DataMember]
        public int ListEntityCode { get; set; }
        
        /// <summary>
        /// Valor de la Entidad
        /// </summary>
        [DataMember]
        public List<EntityValue>  EntityValues {get; set; }
    
    }
}
