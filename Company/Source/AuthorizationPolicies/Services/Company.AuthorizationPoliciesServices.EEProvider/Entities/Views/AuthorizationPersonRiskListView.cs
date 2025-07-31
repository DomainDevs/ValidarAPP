using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.AuthorizationPoliciesServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AuthorizationPersonRiskListView : BusinessView
    {
        public BusinessCollection CoEventAuthorizations
        {
            get
            {
                return this["CoEventAuthorization"];
            }
        }

        public BusinessCollection CoEventCompanies
        {
            get
            {
                return this["CoEventCompany"];
            }
        }
    }
}
