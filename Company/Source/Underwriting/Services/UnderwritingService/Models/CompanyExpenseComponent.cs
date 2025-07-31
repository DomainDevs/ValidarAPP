using System.Runtime.Serialization;
using Sistran.Company.Application.UnderwritingServices.Enums;


namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyExpenseComponent
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        //[DataMember]
        //public CustomerType ExecutionType { get; set; }
        [DataMember]
        public CompanyRateType RateType { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        [DataMember]
        public CompanyRuleSet RuleSet { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
        [DataMember]
        public bool IsInitially { get; set; }

    }
}
