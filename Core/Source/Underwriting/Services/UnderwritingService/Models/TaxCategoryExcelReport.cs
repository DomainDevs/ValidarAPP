
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class TaxCategoryExcelReport
    {
        [DataMember]
        public int TaxCode { get; set; }

        [DataMember]
        public int TaxCategoryCode { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
