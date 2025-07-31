using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// ReInsuranceCheckingAccountTransaction:  Transaccion de Cuenta Corriente ReAseguros
    /// </summary>
    [DataContract]
    public class ReInsuranceCheckingAccountTransaction : TransactionType
    {
        /// <summary>
        /// ReInsuranceCheckingAccountTransactionItems:  Items Cuenta Corriente Reaseguro
        /// </summary>        
        [DataMember]
        public List<ReInsuranceCheckingAccountTransactionItem> ReInsuranceCheckingAccountTransactionItems { get; set; }
    }
}
