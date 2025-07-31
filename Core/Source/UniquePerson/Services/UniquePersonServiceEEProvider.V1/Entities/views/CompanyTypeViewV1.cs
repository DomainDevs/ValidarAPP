using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class CompanyTypeViewV1 : BusinessView
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
