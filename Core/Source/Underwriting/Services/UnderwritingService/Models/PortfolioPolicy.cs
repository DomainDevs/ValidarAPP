using System.Runtime.Serialization;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class PortfolioPolicy
    {
        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int PortfolioDays { get; set; }

        [DataMember]
        public double IssueValue { get; set; }

        [DataMember]
        public double CollectedValue { get; set; }

        [DataMember]
        public double PendingValue { get; set; }
    }
}
