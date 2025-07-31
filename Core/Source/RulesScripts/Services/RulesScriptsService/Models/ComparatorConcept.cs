using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Comparador de Concepto
    /// </summary>
    [DataContract]
    public class ComparatorConcept
    {
        /// <summary>
        /// Comparador de Texto
        /// </summary>
        [DataMember]
        public string ComparatorText { get; set; }

        /// <summary>
        /// Comparador de Simbolo
        /// </summary>
        [DataMember]
        public string ComparatorSymbol { get; set; }


        /// <summary>
        /// Comparador de Simbolo
        /// </summary>
        [DataMember]
        public string Symbol { get; set; }
    }
}
