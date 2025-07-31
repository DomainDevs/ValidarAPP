#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts
{
    /// <summary>
    ///    ConceptSource
    /// </summary>
    [DataContract]
    public class ConceptSourceDTO
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

      
    }
}