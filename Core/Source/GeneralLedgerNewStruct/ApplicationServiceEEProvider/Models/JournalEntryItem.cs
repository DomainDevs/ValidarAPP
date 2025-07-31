#region Using

using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Asiento de Diario Detalle
    /// </summary>
    [DataContract]
    public class JournalEntryItem
    {
        
        /// <summary>
        ///  Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///  Currency
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        ///  ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        ///  AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccount AccountingAccount { get; set; }

        /// <summary>
        ///  ReconciliationMovementType
        /// </summary>
        [DataMember]
        public ReconciliationMovementType ReconciliationMovementType { get; set; }

        /// <summary>
        ///  Receipt
        /// </summary>
        [DataMember]
        public Receipt Receipt { get; set; }

       
        /// <summary>
        ///  AccountingNature
        /// </summary>
        [DataMember]
        public AccountingNatures AccountingNature { get; set; }
        
        /// <summary>
        ///     Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Amount
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        ///     LocalAmount
        /// </summary>
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        ///     Individual
        /// </summary>
        [DataMember]
        public Individual Individual { get; set; }

        /// <summary>
        ///     EntryType
        /// </summary>
        [DataMember]
        public EntryType EntryType { get; set; }

        /// <summary>
        ///     CostCenter
        /// </summary>
        [DataMember]
        public List<CostCenter> CostCenters { get; set; }

        /// <summary>
        ///     Analysis
        /// </summary>
        [DataMember]
        public List<Analysis> Analysis { get; set; }

        /// <summary>
        ///     PostDated
        /// </summary>
        [DataMember]
        public List<PostDated> PostDated { get; set; }

        /// <summary>
        /// SourceCode
        /// </summary>
        [DataMember]
        public int SourceCode { get; set; }

        /// <summary>
        /// SourceCode
        /// </summary>
        [DataMember]
        public int AccountingConcept { get; set; }

        /// <summary>
        /// SourceCode
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// SourceCode
        /// </summary>
        [DataMember]
        public SalePoint SalePoint { get; set; }
    }
}