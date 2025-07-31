using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Asiento de Diario Detalle
    /// </summary>
    [DataContract]
    public class JournalEntryItemDTO
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

        /// <summary>
        /// SourceCode
        /// </summary>
        [DataMember]
        public int SourceCode { get; set; }

        /// <summary>
        ///  AccountingConcept
        /// </summary>
        [DataMember]
        public int AccountingConcept { get; set; }

        /// <summary>
        ///  Branch
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        ///  SalePoint
        /// </summary>
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

        /// <summary>
        /// Código del paquete de reglas a buscar
        /// </summary>
        [DataMember]
        public int PackageRuleCodeId { get; set; }

        /// <summary>
        /// Identificador de la cuenta puente
        /// </summary>
        [DataMember]
        public int BridgeAccountId { get; set; }
    }
}
