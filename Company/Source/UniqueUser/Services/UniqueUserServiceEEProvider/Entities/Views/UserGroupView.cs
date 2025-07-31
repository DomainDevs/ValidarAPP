using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UserGroupView : BusinessView
    {
        public BusinessCollection Groups
        {
            get
            {
                return this["Groups"];
            }
        }

        public BusinessCollection Users
        {
            get
            {
                return this["Users"];
            }
        }

    }
}
