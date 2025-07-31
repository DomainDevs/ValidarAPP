using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Views
{
    [Serializable()]
    public class JournalEntryView : BusinessView
    {
        public BusinessCollection JournalEntries
        {
            get
            {
                return this["JournalEntry"];
            }
        }

        public BusinessCollection JournalEntryItems
        {
            get
            {
                return this["JournalEntryItem"];
            }
        }

        public BusinessCollection AccountingAccounts
        {
            get
            {
                return this["AccountingAccount"];
            }
        }
    }
}
