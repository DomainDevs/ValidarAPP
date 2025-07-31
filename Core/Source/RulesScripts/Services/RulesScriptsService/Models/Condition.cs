using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Condiciones
    /// </summary>
    [DataContract]
    public class Condition
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Expresion
        /// </summary>
        [DataMember]
        public string Expression { get; set; }

        /// <summary>
        /// Concepto
        /// </summary>
        [DataMember]
        public Concept Concept { get; set; }

        /// <summary>
        /// Comparador
        /// </summary>
        [DataMember]
        public Comparator Comparator { get; set; }


        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Tipo de Descripcion
        /// </summary>
        [DataMember]
        public string DescriptionValue { get; set; }

        /// <summary>
        /// Tipo de Valor 
        /// </summary>
        [DataMember]
        public Sistran.Core.Application.RulesScriptsServices.Enums.ValueType ValueType { get; set; }

        /// <summary>
        /// Valores Adicionaes Del Control
        /// </summary>
        [DataMember]
        public ConceptControl ConceptControl { get; set; }

        /// <summary>
        /// Valor del Concepto
        /// </summary>
        [DataMember]
        public Concept ConceptValue { get; set; }
    }
}
