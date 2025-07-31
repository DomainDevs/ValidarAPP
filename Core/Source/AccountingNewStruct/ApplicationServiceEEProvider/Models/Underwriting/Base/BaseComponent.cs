using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    /// <summary>
    /// Componentes Base
    /// </summary>
    [DataContract]
    public class BaseComponent : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Componente
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Tipo de componente
        /// </summary>
        [DataMember]
        public ComponentType? ComponentType { get; set; }

        /// <summary>
        /// Clase de componente
        /// </summary>
        [DataMember]
        public ComponentClassType? ComponentClass { get; set; }
    }
}
