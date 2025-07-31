using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class CumulusTypeDTO
    {
        [DataMember]
        public int CumulusTypeId { get; set; }
        [DataMember]
        public String Description { get; set; }
    }
}
