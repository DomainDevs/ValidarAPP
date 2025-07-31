using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UserSalePointView : BusinessView
    {
        public BusinessCollection SalesPoint
        {
            get
            {
                return this["SalePoint"];
            }
        }

        public BusinessCollection UserSalesPoint
        {
            get
            {
                return this["UserSalePoint"];
            }
        }

        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }

        public BusinessCollection UserBranches
        {
            get
            {
                return this["UserBranch"];
            }
        }
    }
}
