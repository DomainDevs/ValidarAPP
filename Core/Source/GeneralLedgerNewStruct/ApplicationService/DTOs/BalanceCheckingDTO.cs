using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     DTO  que  carga  el  balance   de  comprobacion
    /// </summary>
    [DataContract]
    public class BalanceCheckingDTO 
    {
        /// <summary>
        ///     Cuenta  Contable
        /// </summary>
        [DataMember]
        public String AccountingAccount { get; set; }

        /// <summary>
        ///     Descripcion de la cuenta   contable
        /// </summary>
        [DataMember]
        public string DescriptionAccount { get; set; }

        /// <summary>
        ///     Suma de Debitos
        /// </summary>
        [DataMember]
        public decimal SumDebit { get; set; }

        /// <summary>
        ///     Suma de Creditos
        /// </summary>
        [DataMember]
        public decimal SumCredit { get; set; }

        /// <summary>
        ///     saldo de Creditos
        /// </summary>
        [DataMember]
        public decimal Creditbalance { get; set; }

        /// <summary>
        ///     Saldo de Debitos
        /// </summary>
        [DataMember]
        public decimal Debitbalance { get; set; }

        /// <summary>
        ///     Saldo de Debitos
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        ///     Saldo de Debitos
        /// </summary>
        [DataMember]
        public int Nature { get; set; }
    }
}