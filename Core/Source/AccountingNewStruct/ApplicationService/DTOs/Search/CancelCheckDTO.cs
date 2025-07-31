using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CancelCheckDTO 
    {
        [DataMember]
        public int AccountBankId { get; set; }
		
        [DataMember]
        public decimal Amount { get; set; }
		
        [DataMember]
        public int CheckNumber { get; set; }
		
        [DataMember]
        public int CheckPaymentOrderCode { get; set; }
		
        [DataMember]
        public int CheckPaymentOrderStatus { get; set; }
		
        [DataMember]
        public decimal ExchangeRate { get; set; }
		
        [DataMember]
        public decimal IncomeAmount { get; set; }
		
		[DataMember]
        public int IndividualId { get; set; }		
		
		[DataMember]
        public int IsCheckPrinted { get; set; }		
		
		[DataMember]
        public string PaymentDate { get; set; }
		
        [DataMember]
        public int PaymentOrderCode { get; set; }
			
		[DataMember]
        public int PaymetOrderStatus { get; set; }
		
        [DataMember]
        public string PayTo { get; set; }
		
        [DataMember]
        public string PaymetOrderStatusDescription { get; set; }
		
        [DataMember]
        public int TempImputationCode { get; set; }
    }
}
