using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Comparator
    {
        /// <summary>
        /// Tipo de Operadores
        /// </summary>
        public System.CodeDom.CodeBinaryOperatorType CodeBinaryOperatorType;

        /// <summary>
        /// Codigo Comparador
        /// </summary>
        [DataMember]
        public int ComparatorCode { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Simbolo
        /// </summary>
        [DataMember]
        public string Symbol { get; set; }

    }
}
