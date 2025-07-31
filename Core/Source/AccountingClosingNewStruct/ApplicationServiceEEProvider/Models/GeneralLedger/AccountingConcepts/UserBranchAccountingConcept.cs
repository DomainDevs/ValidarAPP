#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingConcepts
{
    /// <summary>
    ///    UserBranchAccountingConcept
    /// </summary>
    [DataContract]
    public class UserBranchAccountingConcept
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
        public BranchAccountingConcept BranchAccountingConcept { get; set; }

        /// <summary>
        ///     User
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        
    }
}