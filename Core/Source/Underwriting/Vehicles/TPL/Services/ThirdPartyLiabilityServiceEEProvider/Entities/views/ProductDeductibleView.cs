using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities.views
{
    [Serializable()]
    public class ProductDeductibleView : BusinessView
    {
        public BusinessCollection DeductibleProducts
        {
            get
            {
                return this["DeductibleProduct"];
            }
        }

        public BusinessCollection Deductibles
        {
            get
            {
                return this["Deductible"];
            }
        }

        public BusinessCollection DeductibleUnits
        {
            get
            {
                return this["DeductibleUnit"];
            }
        }

        public BusinessCollection MinimumDeductibleUnits
        {
            get
            {
                return this["MinimumDeductibleUnit"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }
    }
}
