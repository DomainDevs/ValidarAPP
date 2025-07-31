using System.Collections.Generic;
using System.Runtime.Serialization;

//Sistran


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    //TODO DGUERRON : se cambia Payer por Individual no se tiene esta funcionalidad
    /// <summary>
    /// CollectPayer:   Pagador 
    /// </summary>
    [DataContract]
    public class CollectPayer : UniquePersonService.V1.Models.Individual
    {
        /// <summary>
        /// DepositPremiumTransactions 
        /// </summary>        
        [DataMember]
        public List<DepositPremiumTransaction> DepositPremiumTransactions { get; set; }
    }
}
