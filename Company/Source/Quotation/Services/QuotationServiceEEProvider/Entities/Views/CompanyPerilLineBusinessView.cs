using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.QuotationServices.EEProvider.Entities.Views
{
    [Serializable]
    public class CompanyPerilLineBusinessView : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }
        public BusinessCollection Perils
        {
            get
            {
                return this["Peril"];
            }
        }        
    }
}
