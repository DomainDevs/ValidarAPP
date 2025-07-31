using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// ReInsuranceCheckingAccountTransaction:  Transaccion de Cuenta Corriente ReAseguros
    /// </summary>
    [DataContract]
    public class ReInsuranceCheckingAccountTransactionDTO : TransactionTypeDTO
    {
        /// <summary>
        /// ReInsuranceCheckingAccountTransactionItems:  Items Cuenta Corriente Reaseguro
        /// </summary>        
        [DataMember]
        public List<ReInsuranceCheckingAccountTransactionItemDTO> ReInsuranceCheckingAccountTransactionItems { get; set; }
    }
}
