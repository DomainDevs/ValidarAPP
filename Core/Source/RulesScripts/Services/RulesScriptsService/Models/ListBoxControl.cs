using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class ListBoxControl:ConceptControl
    {
        /// <summary>
        /// Valor de la lista de la entidad
        /// </summary>
        [DataMember]
        public List<ListEntityValue> ListListEntityValues { get; set; }

        /// <summary>
        /// Codigo de la lista de la entidad
        /// </summary>
        [DataMember]
        public int ListEntityCode { get; set;}

    }
}
