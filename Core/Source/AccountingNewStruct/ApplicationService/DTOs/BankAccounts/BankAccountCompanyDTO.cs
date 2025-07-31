using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.BankAccounts
{
    /// <summary>
    ///     Cuenta bancaria de la Compañia
    /// </summary>
    [DataContract]
    public class BankAccountCompanyDTO
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
        public BankAccountTypeDTO BankAccountType { get; set; }
        
        /// <summary>
        ///     Numero de Cuenta
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        ///     Banco
        /// </summary>
        [DataMember]
        public BankDTO Bank { get; set; }

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
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        ///     Sucursal
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        ///     Fecha Deshabilitar
        /// </summary>
        [DataMember]
        public DateTime? DisableDate { get; set; }

        /// <summary>
        ///     AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccountDTO AccountingAccount { get; set; }
    }
}

