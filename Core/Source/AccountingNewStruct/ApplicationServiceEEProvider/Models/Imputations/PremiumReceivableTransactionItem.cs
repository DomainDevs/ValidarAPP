using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// PremiumsReceivable:  Primas por cobrar
    /// </summary>
    [DataContract]
    public class PremiumReceivableTransactionItem
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Policy:Poliza que se carga al item de imputación
        /// </summary>        
        [DataMember]
        public Policy Policy { get; set; }
        
        /// <summary>
        /// DeductCommission: comisión descontada
        /// </summary>
        [DataMember]
        public Amount DeductCommission { get; set; }
        

    }
}
