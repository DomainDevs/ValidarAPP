using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// InsuredLoanTransaction:  Transaccion de Préstamos de Asegurados
    /// </summary>
    [DataContract]
    public class InsuredLoanTransactionDTO  : TransactionTypeDTO
    {
        /// <summary>
        /// InsuredLoansTransactionItems:  Items Préstamos Asegurados
        /// </summary>        
        [DataMember]
        public List<InsuredLoanTransactionItemDTO> InsuredLoanTransactionItems { get; set; }
    }

}
