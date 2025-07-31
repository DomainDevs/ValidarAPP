using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// CoInsuranceCheckingAccountTransaction:  Transaccion de Cuenta Corriente Coaseguros
    /// </summary>
    [DataContract]
    public class CoInsuranceCheckingAccountTransaction : TransactionType
    {
        /// <summary>
        /// CoInsuranceCheckingAccountTransactionItems:  Items Cuenta Corriente Coaseguro
        /// </summary>        
        [DataMember]
        public List<CoInsuranceCheckingAccountTransactionItem> CoInsuranceCheckingAccountTransactionItems { get; set; }
    }
}
