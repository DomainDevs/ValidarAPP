using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Entities.Views
{
    [Serializable()]
    public class InsuredPersonSarlaftView : BusinessView
    {
        public BusinessCollection Persons
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection Insureds
        {
            get
            {
                return this["Insured"];
            }
        }
    }
}
