using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class BeneficiaryBankAccountsDTO 
    {
        [DataMember]
        public int AccountBankCode { get; set; }

        [DataMember]
        public int BankCode { get; set; }

        [DataMember]
        public string BankName { get; set; }
		
        [DataMember]
        public string AccountNumber { get; set; }
		
        [DataMember]
        public int AccountTypeCode { get; set; }
		
        [DataMember]
        public string AccountTypeName { get; set; }
		
        [DataMember]
        public int CurrencyCode { get; set; }
		
        [DataMember]
        public string CurrencyName { get; set; }
		
        [DataMember]
        public string IndividualId { get; set; }
		
        [DataMember]
        public string TinyDescription { get; set; }

        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }
    }
}
