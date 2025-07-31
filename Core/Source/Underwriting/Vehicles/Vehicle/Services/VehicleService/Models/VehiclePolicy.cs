using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.Vehicles.VehicleServices.Models
{
    /// <summary>
    /// Vehículo - Póliza
    /// </summary>
    [DataContract]
    public class VehiclePolicy : Policy
    {
        /// <summary>
        /// Riesgos
        /// </summary>
        [DataMember]
        public virtual List<Vehicle> Vehicles { get; set; }
    }
}
