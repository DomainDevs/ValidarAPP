using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class Activity
    {
        [DataMember]
        public int ConceptActivityCd { get; set; }
        [DataMember]
        public int TypeActivityCd { get; set; }
        [DataMember]
        public decimal EBITDAIni { get; set; }
        [DataMember]
        public decimal EBITDAFin { get; set; }

        [DataMember]
        public string Observation { get; set; }
    }
}
