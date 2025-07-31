using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsuranceClaimLayerDTO
    {
        [DataMember]
        public int ClaimId { get; set; }
        [DataMember]
        public int ClaimModifyId { get; set; }
        [DataMember]
        public int ClaimLayerId { get; set; }
        [DataMember]
        public int ReinsuranceNumber { get; set; } 
        [DataMember]
        public string Description { get; set; } 
        [DataMember]
        public DateTime ProcessDate { get; set; }  
        [DataMember]
        public DateTime RegistrationDate { get; set; }  
        [DataMember]
        public string IsAutomatic { get; set; }
        [DataMember]
        public int LayerNumber { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string MovementType { get; set; }
  
    }
}
