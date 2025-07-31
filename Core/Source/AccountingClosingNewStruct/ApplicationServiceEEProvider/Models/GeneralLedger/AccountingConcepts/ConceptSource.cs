#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingConcepts
{
    /// <summary>
    ///    ConceptSource
    /// </summary>
    [DataContract]
    public class ConceptSource
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