using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class PropertyFilter
    {
        /// <summary>
        /// Identificador de la Entidad
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Nombre de la Propiedad
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Llave Primaria
        /// </summary>
        [DataMember]
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// Typo
        /// </summary>
        [DataMember]
        public string Type { get; set; }

    }
}
