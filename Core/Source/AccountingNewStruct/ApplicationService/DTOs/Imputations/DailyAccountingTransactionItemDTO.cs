using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.Search;



namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// DailyAccountingTransactionItem: Item de Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingTransactionItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Branch : Sucursal
        /// </summary>        
        [DataMember]
        public BranchDTO Branch { get; set; }
        /// <summary>
        /// SalePoint : Punto de Venta
        /// </summary>        
        [DataMember]
        public SalePointDTO SalePoint  { get; set; }
        /// <summary>
        /// Company : Compañia
        /// </summary>        
        [DataMember]
        public CompanyDTO Company { get; set; }
        /// <summary>
        /// Beneficiary : Beneficiario
        /// </summary>        
        [DataMember]
        public IndividualDTO Beneficiary { get; set; }
        /// <summary>
        /// AccountingNature: Naturaleza de Cuenta
        /// </summary>        
        [DataMember]
        public int AccountingNature { get; set; }
        
        /// <summary>
        /// Amount : Importe
        /// </summary>        
        [DataMember]
        public AmountDTO Amount{ get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        /// BookAccount: Cuenta Contable
        /// </summary>        
        [DataMember]
        public BookAccountDTO BookAccount { get; set; }

        /// <summary>
        /// DailyAccountingAnalysisCodes
        /// </summary>        
        [DataMember]
        public List<DailyAccountingAnalysisCodeDTO> DailyAccountingAnalysisCodes { get; set; }

        /// <summary>
        /// DailyAccountingCostCenters
        /// </summary>        
        [DataMember]
        public List<DailyAccountingCostCenterDTO> DailyAccountingCostCenters { get; set; }
    }
}
