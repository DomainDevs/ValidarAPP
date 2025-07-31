using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Extensions
{
    /// <summary>
    /// Propiedad Extendida
    /// </summary>
    [DataContract]
    [Serializable]
    public class ExtendedProperty
    {
        /// <summary>
        /// Obtiene o setea el nombre de la propiedad Extendida
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o setea el Valor de la propiedad Extendida
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public object Value { get; set; }
    }
}