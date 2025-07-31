using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models.Base;

namespace Sistran.Core.Application.Vehicles.VehicleServices.Models
{
    /// <summary>
    /// Uso
    /// </summary>
    [DataContract]
    public class Use : BaseUse
    {
        /// <summary>
        /// Tipo de Ramo Comercial
        /// </summary>
        [DataMember]
        public PrefixType PrefixType { get; set; }
    }
}
