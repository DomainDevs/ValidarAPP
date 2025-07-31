using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    [DataContract]
    public class IndicatorDTO
    {
        [DataMember]
        public int ConceptIndicatorCd { get; set; }
        [DataMember]
        public int TypeIndicatorCd { get; set; }
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
