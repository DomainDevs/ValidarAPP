using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class ReinsuranceOperatingQuotaEventDTO
    {
        [DataMember]
        public int ReinsOperatingQuotaEventId { get; set; }

        [DataMember]
        public int OperatingQuotaEventCd { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public bool CanUpdateValidity { get; set; }


    }
}
