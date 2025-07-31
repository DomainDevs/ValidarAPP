using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.TaxServices.Models
{
    [DataContract]
    public class AccountingConceptTax
    {
        [DataMember]
        public int AccountingConceptTaxId { get; set; }

        [DataMember]
        public int AccountingConceptId { get; set; }
        
        [DataMember]
        public Tax Tax { get; set; }

        [DataMember]
        public TaxCategory TaxCategory { get; set; }

        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public bool EnableAddExpense { get; set; }

        [DataMember]
        public bool EnableAutomatic { get; set; }

        [DataMember]
        public RateType RateType { get; set; }
    }
}
