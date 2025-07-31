using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CollectionNoticeAccountBankView : BusinessView
    {
        public BusinessCollection CollectionNoticeAccount
        {
            get
            {
                return this["CollectionNoticeAccount"];
            }
        }
        public BusinessCollection Bank
        {
            get
            {
                return this["Bank"];
            }
        }
    }
}
