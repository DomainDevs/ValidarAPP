using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{

    public class CompanyFinancialSarlaft 
    {
        [DataMember]
        public int SarlaftId { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal ExpenseAmount { get; set; }
        [DataMember]
        public decimal ExtraIncomeAmount { get; set; }
        [DataMember]
        public decimal AssetsAmount { get; set; }
        [DataMember]
        public decimal LiabilitiesAmount { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
