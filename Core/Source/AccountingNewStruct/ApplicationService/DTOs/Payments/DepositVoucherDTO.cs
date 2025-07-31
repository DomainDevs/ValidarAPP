using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{   
    /// <summary>
    ///     Voucher de Deposito
    /// </summary>
    [DataContract]
    public class DepositVoucherDTO : PaymentDTO
    {
        /// <summary>
        ///     Número de cuenta y banco receptor de la compañia
        /// </summary>
        [DataMember]
        public BankAccountCompanyDTO ReceivingAccount { get; set; }
        

        /// <summary>
        ///     Numéro de la boleta de deposito
        /// </summary>
        [DataMember]
        public string VoucherNumber { get; set; }

        /// <summary>
        ///     Fecha de la boleta de deposito
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Nombre del depositante
        /// </summary>
        [DataMember]
        public string DepositorName { get; set; }
    }
}
