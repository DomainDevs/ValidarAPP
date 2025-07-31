using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class ReportListSisconc
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
    }
}
