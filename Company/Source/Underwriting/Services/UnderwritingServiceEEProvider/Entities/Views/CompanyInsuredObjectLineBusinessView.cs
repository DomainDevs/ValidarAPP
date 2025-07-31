using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class CompanyInsuredObjectLineBusinessView : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }
        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }        
    }
}
