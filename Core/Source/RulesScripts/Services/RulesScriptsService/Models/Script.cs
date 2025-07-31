using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Acciones
    /// </summary>
    [DataContract]
    public class Script
    {
        /// <summary>
        /// Id Del Guion
        /// </summary>
        [DataMember]
        public int ScriptId { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id Del Nivel
        /// </summary>
        [DataMember]
        public int LevelId { get; set; }

        /// <summary>
        /// Descripcion del Nivel
        /// </summary>
        [DataMember]
        public string LevelDescription { get; set; }

        /// <summary>
        /// Id del Modulo
        /// </summary>
        [DataMember]
        public int PackageId { get; set; }

        /// <summary>
        /// Descripcion del Modulo
        /// </summary>
        [DataMember]
        public string PackageDescription { get; set; }

        /// <summary>
        /// Id del Nodo
        /// </summary>
        [DataMember]
        public int? NodeId { get; set; }

    }
}
