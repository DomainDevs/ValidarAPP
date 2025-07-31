using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class FinancialSarlaftDTO
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
