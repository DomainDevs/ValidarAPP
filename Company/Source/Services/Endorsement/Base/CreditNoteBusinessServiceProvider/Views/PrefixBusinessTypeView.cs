using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService.EEProvider.Views
{
    [Serializable()]
    public class PrefixBusinessTypeView : BusinessView
    {
        public BusinessCollection PrefixBusinessTypes
        {
            get { return this["PrefixBusinessType"]; }
        }

        public BusinessCollection Policies
        {
            get { return this["Policy"]; }
        }
    }
}
