using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class UtilityDetails
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int FormUtilitys { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public int UtilitysTypeCd { get; set; }
        [DataMember]
        public int UtilitysSummaryCd { get; set; }
        [DataMember]
        public int UtilityId { get; set; }
    }
}
