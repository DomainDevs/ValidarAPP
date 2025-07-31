using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class BrokerCheckingAccountItemDTO
    {
        [DataMember]
        public int BrokerCheckingAccountId { get; set; }
        [DataMember]
        public int BrokerCheckingAccountItemId { get; set; }
        [DataMember]
        public int TempImputationId { get; set; }
        [DataMember]
        public int TempBrokerParentId { get; set; }
        [DataMember]
        public int BrokerCheckingAccountItemChildId { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int PosCode { get; set; }
        [DataMember]
        public string PosName { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public int AgentCode { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public string AgentDocumentNumber { get; set; }
        [DataMember]
        public int CheckingAccountConceptCode { get; set; }
        [DataMember]
        public string ConceptName { get; set; }
        [DataMember]
        public int DebitCreditCode { get; set; }
        [DataMember]
        public string DebitCreditName { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public decimal CurrencyChange { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; } //aumentado por Alejo
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentAgencyCode { get; set; }
        [DataMember]
        public int AccountNature { get; set; }
        [DataMember]
        public int CollectNumber { get; set; }
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public int SalePointCode { get; set; }
        [DataMember]
        public string SalePointName { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; } //aumentado por Alejo
        [DataMember]
        public string PolicyDocumentNumber { get; set; }
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public string PrefixName { get; set; }
        [DataMember]
        public int InsuredId { get; set; }
        [DataMember]
        public string InsuredDocNumber { get; set; }
        [DataMember]
        public string InsuredName { get; set; }
        [DataMember]
        public int CommissionTypeCode { get; set; }
        [DataMember]
        public string CommissionTypeName { get; set; }
        [DataMember]
        public decimal CommissionPercentage { get; set; }
        [DataMember]
        public decimal CommissionAmount { get; set; }
        [DataMember]
        public decimal CommissionDiscounted { get; set; }
        [DataMember]
        public decimal CommissionBalance { get; set; }
        [DataMember]
        public decimal Items { get; set; }
        [DataMember]
        public int TransactionNumber { get; set; }
        [DataMember]
        public int Payed { get; set; } //Aumentado por Byron
        [DataMember]
        public string FactorCommission { get; set; } 
        
        //Aumentado por Byron
        /// <summary>> 
        [DataMember]
        public int LineBusiness { get; set; }  
        
        //Aumentado por Alejo
        /// <summary>
        /// SubRamo del negocio
        /// </summary>
        [DataMember]
        public int SubLineBusiness { get; set; } //Aumentado por Alejo

        [DataMember]
        public decimal AgentParticipationPercentage { get; set; } //ACE-194

        [DataMember]
        public decimal AdditionalCommissionPercentage { get; set; } //ACE-194

        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }
    }

}
