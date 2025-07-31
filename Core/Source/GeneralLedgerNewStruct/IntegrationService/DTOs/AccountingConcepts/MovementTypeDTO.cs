using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs.AccountingConcepts
{
    /// <summary>
    /// MovementType
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
        /// Description
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
