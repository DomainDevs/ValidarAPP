using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    [DataContract]
    public class IndebtednessDTO
    {
        [DataMember]
        public int ConceptIndebtednessCd { get; set; }
        [DataMember]
        public int TypeIndebtednessCd { get; set; }
        [DataMember]
        public decimal IndebtednessIni { get; set; }
        [DataMember]
        public decimal IndebtednessFin { get; set; }
        [DataMember]
        public string Observation { get; set; }
    }
}
