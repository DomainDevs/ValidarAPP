using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class GeneralLedgerDTO 
    {
        [DataMember]
        public int GeneralLedgerId { get; set; }
        [DataMember]
        public string AccountingNumber { get; set; }
        [DataMember]
        public string AccountingName { get; set; }
        [DataMember]
        public int IsMulticurrency { get; set; }
        [DataMember]
        public int DefaultCurrency { get; set; }
    }
}
