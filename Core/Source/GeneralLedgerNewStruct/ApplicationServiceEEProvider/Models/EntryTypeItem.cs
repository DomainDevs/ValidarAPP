#region Using

using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Detalle Aiento Tipo
    /// </summary>
    [DataContract]
    public class EntryTypeItem
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Description 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///    AccountingMovementType
        /// </summary>
        [DataMember]
        public AccountingMovementType AccountingMovementType { get; set; }

        /// <summary>
        ///     AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccount AccountingAccount { get; set; }

        /// <summary>
        ///     AccountingConcept
        /// </summary>
        [DataMember]
        public AccountingConcept AccountingConcept { get; set; }

        /// <summary>
        ///     AccountingNature
        /// </summary>
        [DataMember]
        public AccountingNatures AccountingNature { get; set; }

        /// <summary>
        ///     Currency
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        ///     Analysis
        /// </summary>
        [DataMember]
        public Analysis Analysis { get; set; }

        /// <summary>
        ///     CostCenter
        /// </summary>
        [DataMember]
        public CostCenter CostCenter { get; set; }
    }
}