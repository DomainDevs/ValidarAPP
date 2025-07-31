using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// InsuredLoanTransaction:  Transaccion de Préstamos de Asegurados
    /// </summary>
    [DataContract]
    public class InsuredLoanTransaction : TransactionType
    {
        /// <summary>
        /// InsuredLoansTransactionItems:  Items Préstamos Asegurados
        /// </summary>        
        [DataMember]
        public List<InsuredLoanTransactionItem> InsuredLoanTransactionItems { get; set; }
    }

}
