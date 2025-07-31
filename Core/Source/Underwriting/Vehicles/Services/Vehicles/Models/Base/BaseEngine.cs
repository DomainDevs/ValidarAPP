using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Motor
    /// </summary>
    [DataContract]
    public class BaseEngine : Extension
    {
        /// <summary>
        /// propiedad EngineCylQuantity
        /// </summary>
        [DataMember]
        public int? EngineCylQuantity { get; set; }

        /// <summary>
        /// propiedad EngineCc
        /// </summary>
        [DataMember]
        public int? EngineCc { get; set; }

        /// <summary>
        /// propiedad Horsepower
        /// </summary>
        [DataMember]
        public int? Horsepower { get; set; }

        /// <summary>
        /// propiedad TopSpeed
        /// </summary>
        [DataMember]
        public int? TopSpeed { get; set; }
    }
}
