using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// CheckingAccountTransaction: Transacciones Cuenta Corriente
    /// </summary>
    [DataContract]
    public abstract class CheckingAccountTransactionDTO
    {

        /// <summary>
        /// Comments 
        /// </summary>        
        [DataMember]
        public string Comments { get; set; }

        /// <summary>
        /// Branch: Sucursal 
        /// </summary> 
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// SalePoint: Punto de Venta 
        /// </summary>
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

        /// <summary>
        /// Company:Compañia
        /// </summary>
        [DataMember]
        public CompanyDTO Company { get; set; }

        /// <summary>
        /// Amount: Importe
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        /// AccountingNature: Naturaleza de la Cuenta
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        /// CheckingAccountConcept: Concepto Cuenta Corriente
        /// </summary>
        [DataMember]
        public CheckingAccountConceptDTO CheckingAccountConcept { get; set; }

        /// <summary>
        /// Holder: Propietario
        /// </summary>
        [DataMember]
        public virtual IndividualDTO Holder { get; set; }

        /// <summary>
        /// AccountingDate 
        /// </summary>        
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// PolicyId
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// PrefixId: ramo 
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// InsuredId: id asegurado
        /// </summary>
        [DataMember]
        public int InsuredId { get; set; }

        /// <summary>
        /// CommissionType: tipo comisión
        /// </summary>
        [DataMember]
        public int CommissionType { get; set; }
   
        /// <summary>
        /// CommissionPercentage: porcentaje comisión
        /// </summary>
        [DataMember]
        public AmountDTO CommissionPercentage { get; set; }

        /// <summary>
        /// CommissionAmount: importe comisión
        /// </summary>
        [DataMember]
        public AmountDTO CommissionAmount { get; set; }

        /// <summary>
        /// DiscountedCommission: comisión descontada
        /// </summary>
        [DataMember]
        public AmountDTO DiscountedCommission { get; set; }

        /// <summary>
        /// CommissionBalance: saldo comisión
        /// </summary>
        [DataMember]
        public AmountDTO CommissionBalance { get; set; }

        /// <summary>
        /// AgentParticipationPercentage
        /// </summary>  
        [DataMember]
        public decimal AdditionalCommissionAmount { get; set; } //ACE-194

        /// <summary>
        /// AdditionalCommissionPercentage
        /// </summary>  
        [DataMember]
        public decimal AdditionalCommissionPercentage { get; set; } //ACE-194


    }
}
