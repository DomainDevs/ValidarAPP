using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class IssuanceAgencyDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public DateTime? DateDeclined { get; set; }
        [DataMember]
        public bool IsPrincipal { get; set; }
        [DataMember]
        public decimal Participation { get; set; }
        [DataMember]
        public IssuanceAgentDTO Agent { get; set; }
        [DataMember]
        public List<IssuanceCommissionDTO> Commissions { get; set; }
    }
}
