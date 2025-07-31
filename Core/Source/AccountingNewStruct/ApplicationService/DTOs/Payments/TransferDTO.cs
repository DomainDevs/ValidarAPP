using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;



namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    ///     Pago en Transferencia
    /// </summary>
    [DataContract]
    public class TransferDTO : CheckDTO
    {
        /// <summary>
        ///     Cuenta y banco Receptor
        /// </summary>
        [DataMember]
        public BankAccountPersonDTO ReceivingAccount { get; set; }
    }
}
