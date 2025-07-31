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
    public class CompanyVehicleType : BaseVehicleType
    {

        public const string isElectronicPolicy = "IsElectronicPolicy";

        [DataMember]
        public bool IsElectronicPolicy {
            get
            {
                return GetExtendedProperty<bool>(isElectronicPolicy);
            }
            set
            {
                SetExtendedProperty(isElectronicPolicy, value);
            }
        }

        [DataMember]
        public List<CompanyVehicleBody> VehicleBodies { get; set; }


    }
}
