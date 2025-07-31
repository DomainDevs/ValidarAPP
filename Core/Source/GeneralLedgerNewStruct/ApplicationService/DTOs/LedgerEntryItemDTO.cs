#region Using

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Item Asiento de Mayor
    /// </summary>
    [DataContract]
    public class LedgerEntryItemDTO
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
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        ///  ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        ///  AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccountDTO AccountingAccount { get; set; }

        /// <summary>
        ///  ReconciliationMovementType
        /// </summary>
        [DataMember]
        public ReconciliationMovementTypeDTO ReconciliationMovementType { get; set; }

        /// <summary>
        ///  Receipt
        /// </summary>
        [DataMember]
        public ReceiptDTO Receipt { get; set; }

       
        /// <summary>
        ///  AccountingNature
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }
        
        /// <summary>
        ///     Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Amount
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        ///     LocalAmount
        /// </summary>
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        ///     Individual
        /// </summary>
        [DataMember]
        public IndividualDTO Individual { get; set; }

        /// <summary>
        ///     EntryType
        /// </summary>
        [DataMember]
        public EntryTypeDTO EntryType { get; set; }

        /// <summary>
        ///     CostCenter
        /// </summary>
        [DataMember]
        public List<CostCenterDTO> CostCenters { get; set; }

        /// <summary>
        ///     Analysis
        /// </summary>
        [DataMember]
        public List<AnalysisDTO> Analysis { get; set; }

        /// <summary>
        ///     PostDated
        /// </summary>
        [DataMember]
        public List<PostDatedDTO> PostDated { get; set; }


    }
}