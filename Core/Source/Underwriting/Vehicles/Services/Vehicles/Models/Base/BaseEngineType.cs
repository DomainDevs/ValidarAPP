using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Tipo de motor
    /// </summary>
    [DataContract]
    public class BaseEngineType : Extension
    {
        /// <summary>
        /// Identificador tipo de motor
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion tipo de motor
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
