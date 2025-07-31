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
    public class InsuredCompanySarlaftView : BusinessView
    {
        public BusinessCollection Companies
        {
            get
            {
                return this["Company"];
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
