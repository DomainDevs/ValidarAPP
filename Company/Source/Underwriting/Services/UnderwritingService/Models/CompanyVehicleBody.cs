using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Textos Cia
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UnderwritingServices.Models.Policy" />
    [DataContract]
    public class CompanyVehicleBody : BaseVehicleBody
    {
        [DataMember]
        public List<CompanyVehicleUse> VehicleUses { get; set; }
    }
}
