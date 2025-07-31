using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims
{
    [Serializable()]
    public class SupplierView : BusinessView
    {
        public BusinessCollection Suppliers
        {
            get
            {
                return this["Supplier"];
            }
        }

        public BusinessCollection Individuals
        {
            get
            {
                return this["Individual"];
            }
        }

        public BusinessCollection Persons
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection Companys
        {
            get
            {
                return this["Company"];
            }
        }
    }
}
