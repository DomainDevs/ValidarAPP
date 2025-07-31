using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO
{
    [DataContract]
    public class ListRiskDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int RiskListType { get; set; }
        [DataMember]
        public string RiskListTypeDescription { get; set; }
    }
}
