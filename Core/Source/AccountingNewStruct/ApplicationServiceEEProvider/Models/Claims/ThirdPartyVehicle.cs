using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims
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
