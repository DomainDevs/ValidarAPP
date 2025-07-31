using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.CommonParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class CompanyLineBusinessPrefixView : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection PrefixLineBusiness
        {
            get
            {
                return this["PrefixLineBusiness"];
            }
        }
    }
}
