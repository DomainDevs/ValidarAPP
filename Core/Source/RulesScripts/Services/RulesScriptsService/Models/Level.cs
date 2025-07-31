using System.Runtime.Serialization;


namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Nivel
    /// </summary>
    [DataContract]
    public class Level
    {
        /// <summary>
        /// Identificador de Nivel
        /// </summary>
        [DataMember]
        public int LevelId { get; set; }

        /// <summary>
        /// Identificador de Packete o Modulo
        /// </summary>
        [DataMember]
        public int PackageId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
