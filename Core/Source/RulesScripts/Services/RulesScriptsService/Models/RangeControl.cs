using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RangeControl:ConceptControl
    {
        /// <summary>
        /// Valor del Rango de la Lista
        /// </summary>
        [DataMember]
        public List<RangeEntityValue> ListRangeEntityValues { get; set; }
    }
}
