using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO
{
    [DataContract]
   public  class ListRiskStatusDTO
    {
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public int ProcessCount { get; set; }
        [DataMember]
        public int InsertedCount { get; set; }
        [DataMember]
        public int ProcessStatus { get; set; }
        [DataMember]
        public int HasError { get; set; }
        [DataMember]
        public DateTime BeginDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public string ErrorDescription { get; set; }
    }
}
