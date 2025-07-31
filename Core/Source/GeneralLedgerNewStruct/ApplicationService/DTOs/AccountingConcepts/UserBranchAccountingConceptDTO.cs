#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts
{
    /// <summary>
    ///    UserBranchAccountingConcept
    /// </summary>
    [DataContract]
    public class UserBranchAccountingConceptDTO
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     BranchAccountingConcept
        /// </summary>
        [DataMember]
        public BranchAccountingConceptDTO BranchAccountingConcept { get; set; }

        /// <summary>
        ///     User
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        
    }
}