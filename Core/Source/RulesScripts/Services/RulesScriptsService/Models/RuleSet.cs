using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Paquete de Reglas
    /// </summary>
    [DataContract]
    public class RuleSet
    {
        /// <summary>
        /// Identificador de Paquete de Reglas
        /// </summary>     
        [DataMember]
        public int RuleSetId { get; set; }

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
        /// Versión del paquete de reglas
        /// </summary>
        [DataMember]
        public int RuleSetVer { get; set; }

        /// <summary>
        /// XML de la Regla
        /// </summary>
        [DataMember]
        public byte[] RuleSetXml { get; set; }

        /// <summary>
        /// Modulo
        /// </summary>
        [DataMember]
        public Package Package { get; set; }

        /// <summary>
        /// Nivel
        /// </summary>
        [DataMember]
        public Level Level { get; set; }

        [DataMember]
        public bool IsTable { get; set ; }

        [DataMember]
        public bool? IsEvent { get; set; }

    }
}
