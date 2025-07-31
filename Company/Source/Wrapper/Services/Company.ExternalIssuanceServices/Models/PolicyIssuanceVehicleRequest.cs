using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class PolicyIssuanceVehicleRequest
    {
        [DataMember]
        public int TempId { get; set; }
        public DateTime CurrentFrom { get; set; }
        public int PaymentScheduleCode { get; set; }
        public Alliance Alliance { get; set; }
        public bool IsHolderInsured { get; set; }
        public Holder Holder { get; set; }
        public RiskVehicle RiskVehicle { get; set; }
    }
}
