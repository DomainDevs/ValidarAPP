using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class ConstEffectiveness
    {
        [DataMember]
        public int ConceptConstEffectivenessCd { get; set; }
        [DataMember]
        public int TypeConstEffectivenesscd { get; set; }
        [DataMember]
        public decimal ConstEffectivenessIni { get; set; }
        [DataMember]
        public decimal ConstEffectivenessFin { get; set; }
        [DataMember]
        public string Observation { get; set; }
    }
}
