using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("BalanceCheking")]
    public class BalanceChekingModel
    {
        /// <summary>
        /// Cuenta Contable
        /// </summary>
        [DataMember]
        public string AccountingAccount { get; set; }

        /// <summary>
        /// Descripción de la cuenta contable
        /// </summary>
        [DataMember]
        public string DescriptionAccount { get; set; }

        /// <summary>
        /// Suma de Débitos
        /// </summary>
        [DataMember]
        public decimal SumDebit { get; set; }

        /// <summary>
        /// Suma de Créditos
        /// </summary>
        [DataMember]
        public decimal SumCredit { get; set; }

        /// <summary>
        /// Saldo de Créditos
        /// </summary>
        [DataMember]
        public decimal Creditbalance { get; set; }

        /// <summary>
        /// Saldo de Débitos
        /// </summary>
        [DataMember]
        public decimal Debitbalance { get; set; }
    }
}