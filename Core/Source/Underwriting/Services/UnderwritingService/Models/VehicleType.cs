using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Textos
    /// </summary>
    [DataContract]
    public class VehicleType : BaseVehicleType
    {
        /// <summary>
        /// VehicleBodies
        /// </summary>
        public List<VehicleBody> VehicleBodies { get; set; }
    }
}
