using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Modulo
    /// </summary>
    [DataContract]
    public class Package
    {
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

        /// <summary>
        /// Espacio de nombre
        /// </summary>
        [DataMember]
        public string NameSpace { get; set; }

        /// <summary>
        /// Desabilitado
        /// </summary>
        [DataMember]
        public bool Disabled { get; set; }
    }
}
