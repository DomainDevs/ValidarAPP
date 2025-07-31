using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CheckBookControlDTO 
    {
        [DataMember]
        public int AccountBankCode { get; set; }
		
        [DataMember]
        public int CheckbookControlCode { get; set; }
		
        [DataMember]
        public int CheckFrom { get; set; }
		
        [DataMember]
        public int CheckTo { get; set; }
		
        [DataMember]
        public string DisabledDate { get; set; }
		
        [DataMember]
        public int IsAutomatic { get; set; }
		
        [DataMember]
        public int LastCheck { get; set; }
		
        [DataMember]
        public int Status { get; set; }
		
        [DataMember]
        public string Number { get; set; }
		
        [DataMember]
        public string DescriptionBank { get; set; }
		
        [DataMember]
        public string SmallDescriptionBranch { get; set; }
		
        [DataMember]
        public int BankCode { get; set; }
		
        [DataMember]
        public int BranchCode { get; set; }
        
    }

}
