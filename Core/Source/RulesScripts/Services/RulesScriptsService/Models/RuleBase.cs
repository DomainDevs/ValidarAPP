using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RuleBase
    {
        /// <summary>
        /// Id Tabla de Decision
        /// </summary>
        [DataMember]
        public int RuleBaseId { get; set; }

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
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Fecha de inicio de Vigencia
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha de inicio de Vigencia
        /// </summary>
        [DataMember]
        public string Current { get; set; }

        /// <summary>
        /// Versión del paquete de reglas
        /// </summary>
        [DataMember]
        public int RuleBaseVersion { get; set; }

        /// <summary>
        /// Esta Publicada
        /// </summary>
        [DataMember]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Cantidad de reglas que tiene la Tabla de Decisión
        /// </summary>
        [DataMember]
        public int RuleEnumerator { get; set; }

        /// <summary>
        /// Descriptión del paquete
        /// </summary>
        [DataMember]
        public string PackageDescription { get; set; }

        /// <summary>
        /// Descripcion del nivel
        /// </summary>
        [DataMember]
        public string LevelsDescription { get; set; }

        /// <summary>
        /// Descripcion del nivel
        /// </summary>
        [DataMember]
        public string Published { get; set; }
    }
}
