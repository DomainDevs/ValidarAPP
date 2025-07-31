using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class JournalEntryParametersDTO
    {
        /// <summary>
        /// TypeId
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }
        /// <summary>
        /// BillId
        /// </summary>
        public int BillId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// TechnicalTransaction
        /// </summary>
        [DataMember]
        public int TechnicalTransaction { get; set; }
        /// <summary>
        /// JournalEntryListParameters
        /// </summary>
        [DataMember]
        public List<JournalEntryListParametersDTO> JournalEntryListParameters { get; set; }
    }
}
