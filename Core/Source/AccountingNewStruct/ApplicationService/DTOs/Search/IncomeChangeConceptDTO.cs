using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class IncomeChangeConceptDTO 
    {
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public int CollectControlCode { get; set; }
        [DataMember]
        public int CollectConceptCode { get; set; }
        [DataMember]
        public string CollectConceptName { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public DateTime AccountingDate { get; set; }
        [DataMember]
        public int CollectStatus { get; set; }
        [DataMember]
        public int CollectControlStatus { get; set; }

    }
}
