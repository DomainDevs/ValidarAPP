using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// DailyAccountingCostCenter: Centro de Costo del Item de Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingCostCenterDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// CostCenter 
        /// </summary>        
        [DataMember]
        public CostCenterDTO CostCenter { get; set; }

        /// <summary>
        /// Percentage
        /// </summary>        
        [DataMember]
        public decimal Percentage { get; set; }
    }
}
