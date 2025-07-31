using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class DateEditorControl:ConceptControl
    {
        /// <summary>
        /// Valor Maximo
        /// </summary>
        [DataMember]
        public DateTime? MaxValue { get; set; }

        /// <summary>
        /// Valor Minimo
        /// </summary>
        [DataMember]
        public DateTime? MinValue { get; set; }
    }
}
