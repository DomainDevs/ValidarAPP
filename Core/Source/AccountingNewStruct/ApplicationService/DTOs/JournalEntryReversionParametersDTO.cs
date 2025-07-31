using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class JournalEntryReversionParametersDTO
    {
        /// <summary>
        /// Technical transaction for reverse
        /// </summary>
        [DataMember]
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Technical transaction for new journal entry
        /// </summary>
        [DataMember]
        public int NewTechnicalTransaction { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Accounting date for new journal entry
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Branch identifier
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Module identifier
        /// </summary>
        [DataMember]
        public int ModuleId { get; set; }
    }
}
