using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ProductServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ProductHardRiskTypeView : BusinessView
    {
        public BusinessCollection Products
        {
            get
            {
                return this["Product"];
            }
        }

        public BusinessCollection ProductCoverRiskTypes
        {
            get
            {
                return this["ProductCoverRiskType"];
            }
        }

        public BusinessCollection HardRiskTypes
        {
            get
            {
                return this["HardRiskType"];
            }
        }
    }
}