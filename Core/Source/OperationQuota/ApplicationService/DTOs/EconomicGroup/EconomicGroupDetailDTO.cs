using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup
{
    [DataContract]
    public class EconomicGroupDetailDTO
    {
        [DataMember]
        public int EconomicGroupId { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
    }
}
