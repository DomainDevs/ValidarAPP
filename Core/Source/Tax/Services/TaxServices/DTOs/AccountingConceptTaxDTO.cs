using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.TaxServices.DTOs
{
    [DataContract]
    public class AccountingConceptTaxDTO
    {
        [DataMember]
        public int AccountingConceptId { get; set; }

        [DataMember]
        public int AccountingConceptTaxId { get; set; }

        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public string TaxDescription { get; set; }

        [DataMember]
        public int TaxCategoryId { get; set; }

        [DataMember]
        public string TaxCategoryDescription { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public bool EnableAddExpense { get; set; }

        [DataMember]
        public bool EnableAutomatic { get; set; }

        [DataMember]
        public int RateTypeId { get; set; }

        [DataMember]
        public string RateTypeDescription { get; set; }
    }
}
