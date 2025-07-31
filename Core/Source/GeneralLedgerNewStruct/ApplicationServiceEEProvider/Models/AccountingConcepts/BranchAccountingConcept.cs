#region Using

using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts
{
    /// <summary>
    ///    BranchAccountingConcept
    /// </summary>
    [DataContract]
    public class BranchAccountingConcept
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
        public Branch Branch { get; set; }

        /// <summary>
        ///     AccountingConcept
        /// </summary>
        [DataMember]
        public AccountingConcept AccountingConcept { get; set; }

        /// <summary>
        ///     MovementType
        /// </summary>
        [DataMember]
        public MovementType MovementType { get; set; }
              


    }
}