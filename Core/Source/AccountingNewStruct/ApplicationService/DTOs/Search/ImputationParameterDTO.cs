using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ImputationParameterDTO 
    {
        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public int SubModuleId { get; set; }
        [DataMember]
        public int SourceCode { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int ModuleDateId { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int AccountingNature { get; set; }
        [DataMember]
        public int AccountingAccountId { get; set; }
        [DataMember]
        public int MovementType { get; set; }
        [DataMember]
        public int ImputationId { get; set; }
        [DataMember]
        public int ImputationTypeId { get; set; }
        [DataMember]
        public string Component { get; set; }
        [DataMember]
        public int BusinessTypeId { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int BankReconciliationId { get; set; }
        [DataMember]
        public int ReceiptNumber { get; set; }
        [DataMember]
        public DateTime ReceiptDate { get; set; }
    
       
    }
}
