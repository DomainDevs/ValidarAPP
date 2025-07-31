//Sistran
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// DailyAccountingTransactionItem: Item de Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingTransactionItem 
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
        public Branch Branch { get; set; }
        /// <summary>
        /// SalePoint : Punto de Venta
        /// </summary>        
        [DataMember]
        public SalePoint SalePoint  { get; set; }
        /// <summary>
        /// Company : Compañia
        /// </summary>        
        [DataMember]
        public Company Company { get; set; }
        /// <summary>
        /// Beneficiary : Beneficiario
        /// </summary>        
        [DataMember]
        public Individual Beneficiary { get; set; }
        /// <summary>
        /// AccountingNature: Naturaleza de Cuenta
        /// </summary>        
        [DataMember]
        public AccountingNature AccountingNature { get; set; }
        
        /// <summary>
        /// Amount : Importe
        /// </summary>        
        [DataMember]
        public Amount Amount{ get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        [DataMember]
        public Amount LocalAmount { get; set; }


        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// BookAccount: Cuenta Contable
        /// </summary>        
        [DataMember]
        public BookAccount BookAccount { get; set; }

        /// <summary>
        /// DailyAccountingAnalysisCodes
        /// </summary>        
        [DataMember]
        public List<DailyAccountingAnalysisCode> DailyAccountingAnalysisCodes { get; set; }

        /// <summary>
        /// DailyAccountingCostCenters
        /// </summary>        
        [DataMember]
        public List<DailyAccountingCostCenter> DailyAccountingCostCenters { get; set; }
    }
}
