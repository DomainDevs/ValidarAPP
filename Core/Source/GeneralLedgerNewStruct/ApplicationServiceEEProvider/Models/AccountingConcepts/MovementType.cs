#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts
{
    /// <summary>
    ///    MovementType
    /// </summary>
    [DataContract]
    public class MovementType
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
        ///     ConceptSource
        /// </summary>
        [DataMember]
        public ConceptSource ConceptSource { get; set; }
    }
}