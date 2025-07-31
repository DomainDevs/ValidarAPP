using System.Runtime.Serialization;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class Alliance
    {
        public int Code { get; set; }
        public int BranchCode { get; set; }
        public int SalePointCode { get; set; }
        public int ProposalNumber { get; set; }

    }
}