using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Vehicles.VehicleServices.DTOs
{
    [DataContract]
    public class PolicyEndorsementDTO
    {
        [DataMember]
        public int EndorsementNumber { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public decimal DocumentNumber { get; set; }
    }
}
