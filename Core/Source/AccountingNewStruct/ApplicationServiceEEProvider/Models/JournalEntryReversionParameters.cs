using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    public class JournalEntryReversionParameters
    {
        /// <summary>
        /// Technical transaction for reverse
        /// </summary>
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Technical transaction for new journal entry
        /// </summary>
        public int NewTechnicalTransaction { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Accounting date for new journal entry
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Branch identifier
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Module identifier
        /// </summary>
        public int ModuleId { get; set; }
    }
}
