using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    public class TaxInformationDTO 
    {
        [DataMember]
        public int PaymentTaxCode { get; set; }
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int TaxCode { get; set; }
        [DataMember]
        public double TaxRate { get; set; }
        [DataMember]
        public double TaxAmount { get; set; }
        [DataMember]
        public double TaxBase { get; set; }
        [DataMember]
        public string Description { get; set; } //BranchDescription
        
    }
}
