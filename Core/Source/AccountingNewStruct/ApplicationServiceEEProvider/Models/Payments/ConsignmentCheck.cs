using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    [DataContract]
    public class ConsignmentCheck : Payment
    {
        /// <summary>
        ///     Número de cuenta y banco receptor de la compañia
        /// </summary>
        [DataMember]
        public BankAccountCompany ReceivingAccount { get; set; }

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

        /// <summary>
        /// Banco Emisor
        /// </summary>
        [DataMember]
        public Bank IssuingBank { get; set; }

        /// <summary>
        /// Nombre Emisor del cheque
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }

        /// <summary>
        /// Fecha cheque
        /// </summary>
        [DataMember]
        public DateTime CheckDate { get; set; }
    }
}
