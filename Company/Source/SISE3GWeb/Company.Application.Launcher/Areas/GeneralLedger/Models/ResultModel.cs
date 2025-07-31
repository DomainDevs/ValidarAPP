using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    [KnownType("ResultModel")]
    public class ResultModel
    {
        public int ResultId { get; set; }
        public int ConceptId { get; set; }
        public int ResultCode { get; set; }
        public int AccountingNatureId { get; set; }
        public string AccountingNatureDescription { get; set; }
        public int AccountingAccountId { get; set; }
        public string AccountingAccountNumber { get; set; }
        public string AccountingAccountName { get; set; }
        public int AccountingConceptId { get; set; }
        public string AccountingConceptDescription { get; set; }
        public int ParameterId { get; set; }
        public int ParameterOrder { get; set; }
        public string ParameterDescription { get; set; }
        public string Value { get; set; }
    }
}