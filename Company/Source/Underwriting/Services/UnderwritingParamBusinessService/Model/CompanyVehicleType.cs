using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyVehicleType : BaseVehicleType
    {

       
        [DataMember]
        public List<CompanyVehicleBody> VehicleBodies { get; set; }

    }
}
