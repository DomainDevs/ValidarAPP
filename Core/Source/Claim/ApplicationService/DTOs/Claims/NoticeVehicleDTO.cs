using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class NoticeVehicleDTO : NoticeDTO
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Mail { get; set; }

        [DataMember]
        public int RiskNum { get; set; }

        [DataMember]
        public int CoverNum { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public bool IsInsured { get; set; }

        [DataMember]
        public int EstimateTypeId { get; set; }

        [DataMember]
        public decimal EstimateAmount { get; set; }

        [DataMember]
        public bool IsProspect { get; set; }
    }
}
