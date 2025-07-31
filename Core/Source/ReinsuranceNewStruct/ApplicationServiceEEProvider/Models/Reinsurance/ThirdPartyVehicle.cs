using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ThirdPartyVehicle
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClaimCoverageId { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int ColorCode { get; set; }

        [DataMember]
        public string EngineNumber { get; set; }

        [DataMember]
        public string ChasisNumber { get; set; }

        [DataMember]
        public string VinCode { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
