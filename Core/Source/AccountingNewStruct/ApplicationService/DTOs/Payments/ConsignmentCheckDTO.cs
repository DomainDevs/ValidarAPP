using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    /// Consignación de cheque
    /// </summary>
    [DataContract]
    public class ConsignmentCheckDTO : PaymentDTO
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

        /// <summary>
        /// Número de documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Banco Emisor
        /// </summary>
        [DataMember]
        public BankDTO IssuingBank { get; set; }

        /// <summary>
        /// Número de cuenta Emisor
        /// </summary>
        [DataMember]
        public string IssuingAccountNumber { get; set; }

        /// <summary>
        /// Nombre Emisor del cheque
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember]
        public DateTime CheckDate { get; set; }

    }
}
