using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// CheckingAccountTransaction: Transacciones Cuenta Corriente
    /// </summary>
    [DataContract]
    public abstract class CheckingAccountTransaction 
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
        public Branch Branch { get; set; }

        /// <summary>
        /// SalePoint: Punto de Venta 
        /// </summary>
        [DataMember]
        public SalePoint SalePoint { get; set; }

        /// <summary>
        /// Company:Compañia
        /// </summary>
        [DataMember]
        public Company Company { get; set; }

        /// <summary>
        /// Amount: Importe
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// AccountingNature: Naturaleza de la Cuenta
        /// </summary>
        [DataMember]
        public AccountingNature AccountingNature { get; set; }

        /// <summary>
        /// CheckingAccountConcept: Concepto Cuenta Corriente
        /// </summary>
        [DataMember]
        public CheckingAccountConcept CheckingAccountConcept { get; set; }

        /// <summary>
        /// Holder: Propietario
        /// </summary>
        [DataMember]
        public virtual Individual Holder { get; set; }

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
        public Amount CommissionPercentage { get; set; }

        /// <summary>
        /// CommissionAmount: importe comisión
        /// </summary>
        [DataMember]
        public Amount CommissionAmount { get; set; }

        /// <summary>
        /// DiscountedCommission: comisión descontada
        /// </summary>
        [DataMember]
        public Amount DiscountedCommission { get; set; }

        /// <summary>
        /// CommissionBalance: saldo comisión
        /// </summary>
        [DataMember]
        public Amount CommissionBalance { get; set; }

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
