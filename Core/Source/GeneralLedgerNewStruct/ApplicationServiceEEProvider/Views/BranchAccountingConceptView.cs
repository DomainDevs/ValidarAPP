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
    public class BranchAccountingConceptView : BusinessView
    {
        public BusinessCollection AccountingConcepts
        {
            get
            {
                return this["AccountingConcept"];
            }
        }

        public BusinessCollection BranchAccountingConcepts
        {
            get
            {
                return this["BranchAccountingConcept"];
            }
        }
    }
}
