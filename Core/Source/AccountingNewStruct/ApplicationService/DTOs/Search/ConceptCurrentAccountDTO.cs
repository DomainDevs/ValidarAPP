using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class ConceptCurrentAccountDTO 
    {
        [DataMember]
        public int ConceptId { get; set; }
        [DataMember]
        public string ConceptDescription { get; set; }
        [DataMember]
        public int AgentEnabled { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public double PercentageDiference { get; set; }
        [DataMember]
        public int ItemsEnabled { get; set; }
        [DataMember]
        public int AccountingConceptId { get; set; }
    }
}


