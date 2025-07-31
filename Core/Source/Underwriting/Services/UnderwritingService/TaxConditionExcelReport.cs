
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices
{
    [DataContract]
    public class TaxConditionExcelReport
    {
        [DataMember]
        public int TaxCode { get; set; }

        [DataMember]
        public int TaxConditionCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string HasNationalRate { get; set; }

        [DataMember]
        public string IsIndependent { get; set; }

        [DataMember]
        public string IsDefault { get; set; }
    }
}
