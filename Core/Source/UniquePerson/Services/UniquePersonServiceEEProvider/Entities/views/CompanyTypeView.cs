using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class CompanyTypeView : BusinessView
    {
        public BusinessCollection CompanyTypes
        {
            get
            {
                return this["CompanyType"];
            }
        }

        public BusinessCollection CoAssociationCompanyTypes
        {
            get
            {
                return this["CoAssociationCompanyType"];
            }
        }
    }
}
