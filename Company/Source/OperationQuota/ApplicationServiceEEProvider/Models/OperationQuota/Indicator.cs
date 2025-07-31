using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class Indicator
    {
        [DataMember]
        public int ConceptIndicatorcd { get; set; }
        [DataMember]
        public int TypeIndicatorcd { get; set; }
        [DataMember]
        public decimal IndicatorIni { get; set; }
        [DataMember]
        public decimal IndicatorFin { get; set; }
        [DataMember]
        public string Observation { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
