using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Collections.Generic;
using System.Runtime.Serialization;


//Sistran


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    //TODO DGUERRON : se cambia Payer por Individual no se tiene esta funcionalidad
    /// <summary>
    /// CollectPayer:   Pagador 
    /// </summary>
    [DataContract]
    public class CollectPayerDTO: IndividualDTO
    {
        /// <summary>
        /// DepositPremiumTransactions 
        /// </summary>        
        [DataMember]
        public List<DepositPremiumTransactionDTO> DepositPremiumTransactions { get; set; }
    }
}
