using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// DailyAccountingCostCenter: Centro de Costo del Item de Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingCostCenter
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
        public CostCenter CostCenter { get; set; }

        /// <summary>
        /// Percentage
        /// </summary>        
        [DataMember]
        public decimal Percentage { get; set; }
    }
}
