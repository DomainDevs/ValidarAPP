using System.CodeDom;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Operator
    {
        /// <summary>
        /// Codigo del Operador
        /// </summary>
        [DataMember]
        public int OperatorCode { get; set; }

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

        /// <summary>
        /// Tipo de Operador
        /// </summary>
        [DataMember]
        public CodeBinaryOperatorType CodeBinaryOperatorType { get; set; }
    }
}
