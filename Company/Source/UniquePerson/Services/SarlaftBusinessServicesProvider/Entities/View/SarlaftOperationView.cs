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
    public class SarlaftOperationView : BusinessView
    {
        public BusinessCollection SarlaftOperations
        {
            get
            {
                return this["SarlaftOperation"];
            }
        }

        public BusinessCollection ProductTypes
        {
            get
            {
                return this["ProductType"];
            }
        }

        public BusinessCollection OperationTypes
        {
            get
            {
                return this["OperationType"];
            }
        }
    }
}
