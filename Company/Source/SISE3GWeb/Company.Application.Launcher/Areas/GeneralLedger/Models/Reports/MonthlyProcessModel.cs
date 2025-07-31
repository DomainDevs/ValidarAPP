using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("MonthlyProcessModel")]
    public class MonthlyProcessModel
    {
        /// <summary>
        /// Código de la Sucursal
        /// </summary>
        [DataMember]
        public int BrachCd { get; set; }

        /// <summary>
        /// Descripción de la Susursal
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Código del Ramo comercial
        /// </summary>
        [DataMember]
        public int PrefixCd { get; set; }

        /// <summary>
        /// Decripción del Ramo comercial
        /// </summary>
        [DataMember]
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Código de moneda
        /// </summary>
        [DataMember]
        public int CurrencyCd { get; set; }

        /// <summary>
        /// Descripción de moneda
        /// </summary>
        [DataMember]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Código de débito/crédito
        /// </summary>
        [DataMember]
        public string AccountNatureCd { get; set; }

        /// <summary>
        /// Código de la cuenta contable
        /// </summary>
        [DataMember]
        public string AccountingAccountCd { get; set; }

        /// <summary>
        /// Descripción de la cuenta contable
        /// </summary>
        [DataMember]
        public string AccountingAccountDescription { get; set; }

        /// <summary>
        /// Número de Asiento
        /// </summary>
        [DataMember]
        public int EntryNumber { get; set; }

        /// <summary>
        /// Débito
        /// </summary>
        [DataMember]
        public decimal Debit { get; set; }

        // <summary>
        /// Crédito
        /// </summary>
        [DataMember]
        public decimal Credit { get; set; }

        // <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        // <summary>
        /// AccountDate
        /// </summary>
        [DataMember]
        public string AccountDate { get; set; }

        // <summary>
        /// AccountDate
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}