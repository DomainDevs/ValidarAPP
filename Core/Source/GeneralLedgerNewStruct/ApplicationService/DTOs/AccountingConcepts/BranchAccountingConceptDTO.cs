#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts
{
    /// <summary>
    ///    BranchAccountingConcept
    /// </summary>
    [DataContract]
    public class BranchAccountingConceptDTO
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Branch
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        ///     AccountingConcept
        /// </summary>
        [DataMember]
        public AccountingConceptDTO AccountingConcept { get; set; }

        /// <summary>
        ///     MovementType
        /// </summary>
        [DataMember]
        public MovementTypeDTO MovementType { get; set; }

    }
}