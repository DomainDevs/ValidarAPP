using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RuleFunction
    {
        /// <summary>
        /// Identificador de Función de regla
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de Modulo
        /// </summary>
        [DataMember]
        public int PackageId { get; set; }

        /// <summary>
        /// Identificador de Nivel 
        /// </summary>
        [DataMember]
        public int LevelId { get; set; }

        /// <summary>
        /// Nombre real de la Funcion Acción
        /// </summary>
        [DataMember]
        public string FunctionName { get; set; }

        /// <summary>
        /// Descripción de la Funcion Acción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    
    }
}
