using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// CoInsuranceCheckingAccountTransaction:  Transaccion de Cuenta Corriente Coaseguros
    /// </summary>
    [DataContract]
    public class CoInsuranceCheckingAccountTransactionDTO : TransactionTypeDTO
    {
        /// <summary>
        /// CoInsuranceCheckingAccountTransactionItems:  Items Cuenta Corriente Coaseguro
        /// </summary>        
        [DataMember]
        public List<CoInsuranceCheckingAccountTransactionItemDTO> CoInsuranceCheckingAccountTransactionItems { get; set; }
    }
}
