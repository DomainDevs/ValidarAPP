#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts
{
    /// <summary>
    ///    MovementType
    /// </summary>
    [DataContract]
    public class MovementTypeDTO
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
        public ConceptSourceDTO ConceptSource { get; set; }
    }
}