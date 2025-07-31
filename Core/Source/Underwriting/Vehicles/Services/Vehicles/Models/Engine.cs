using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models
{
    /// <summary>
    /// Motor
    /// </summary>
    [DataContract]
    public class Engine : BaseEngine
    {
        /// <summary>
        /// propiedad EngineType
        /// </summary>
        [DataMember]
        public EngineType EngineType { get; set; }

    }
}
