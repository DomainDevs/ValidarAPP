using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Valor de la Entidad
    /// </summary>
    [DataContract]
    public class EntityValue
    {
        /// <summary>
        /// Valor Entidad
        /// </summary>
        [DataMember]
        public int EntityValueCode { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}
