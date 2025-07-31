using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts
{
    /// <summary>
    ///     Cuenta bancaria de la Compañia
    /// </summary>
    [DataContract]
    public class BankAccountCompany
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de Cuenta
        /// </summary>
        [DataMember]
        public BankAccountType BankAccountType { get; set; }
        
        /// <summary>
        ///     Numero de Cuenta
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        ///     Banco
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        ///     Si/No Habilitado
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Si/No Por Defecto
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        ///     Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        ///     Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        ///     Fecha Deshabilitar
        /// </summary>
        [DataMember]
        public DateTime DisableDate { get; set; }

        /// <summary>
        ///     AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccount AccountingAccount { get; set; }
    }
}

