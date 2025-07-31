using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Condicion de Filtro
    /// </summary>
    [DataContract]
    public class ConditionFilter
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Nombre de la Propiedad
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }

        /// <summary>
        /// Comparador de Simbolo
        /// </summary>
        [DataMember]
        public string SymbolComparator { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public object Value { get; set; }

    }
}
