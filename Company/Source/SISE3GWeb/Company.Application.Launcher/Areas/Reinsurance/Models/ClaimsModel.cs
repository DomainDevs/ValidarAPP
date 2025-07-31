namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ClaimsModel
    {
        public int MovementSourceId { get; set; }

        public int ClaimBranchCode { get; set; }

        public int PrefixCode { get; set; }

        public decimal PolicyNumber { get; set; }
        public int ClaimCode { get; set; }
        public int ClaimNumber { get; set; }
        public string ClaimDate { get; set; }
        public string RegistrationDate { get; set; }
        public int EstimationTypeId { get; set; }
        public string EstimationType { get; set; }
        public decimal EstimationAmount { get; set; }
    }
}