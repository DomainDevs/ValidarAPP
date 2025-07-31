using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class NumericEditorControl: ConceptControl
    {
        /// <summary>
        /// Valor Maximo
        /// </summary>
        [DataMember]
        public int? MaxValue { get; set; }

        /// <summary>
        /// Valor Minimo
        /// </summary>
        [DataMember]
        public int? MinValue { get; set; }

        /// <summary>
        /// Presicion Decimal 
        /// </summary>
        [DataMember]
        public int? DecimalPrecision { get; set; }
    }
}
