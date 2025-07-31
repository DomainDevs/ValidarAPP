
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.Models
{
    [DataContract]
    public class GoodExperienceYearsThridPart
    {
      

        [DataMember]
        public int IdCardTypeCode { get; set; }

        [DataMember]
        public string IdCardNo { get; set; }
    }
}
