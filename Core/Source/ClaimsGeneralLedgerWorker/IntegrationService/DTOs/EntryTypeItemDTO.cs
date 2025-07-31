using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs.AccountingConcept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs
{
    /// <summary>
    ///     Detalle Aiento Tipo
    /// </summary>
    [DataContract]
    public class EntryTypeItemDTO
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
        public AccountingMovementTypeDTO AccountingMovementType { get; set; }

        /// <summary>
        ///     AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccountDTO AccountingAccount { get; set; }

        /// <summary>
        ///     AccountingConcept
        /// </summary>
        [DataMember]
        public AccountingConceptDTO AccountingConcept { get; set; }

        /// <summary>
        ///     AccountingNature
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Currency
        /// </summary>
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        ///     Analysis
        /// </summary>
        [DataMember]
        public AnalysisDTO Analysis { get; set; }

        /// <summary>
        ///     CostCenter
        /// </summary>
        [DataMember]
        public CostCenterDTO CostCenter { get; set; }
    }
}
