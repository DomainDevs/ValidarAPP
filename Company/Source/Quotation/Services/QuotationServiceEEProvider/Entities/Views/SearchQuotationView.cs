using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.QuotationServices.EEProvider.Entities.Views
{
    [Serializable]
    public class SearchQuotationView : BusinessView
    {
        public BusinessCollection TempSubscriptions
        {
            get
            {
                return this["TempSubscription"];
            }
        }
        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }
        public BusinessCollection Products
        {
            get
            {
                return this["Product"];
            }
        }
        public BusinessCollection TempRiskCoverages
        {
            get
            {
                return this["TempRiskCoverage"];
            }
        }
        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }
        public BusinessCollection TempPayerComponents
        {
            get
            {
                return this["TempPayerComponent"];
            }
        }
        public BusinessCollection TempRiskVehicles
        {
            get
            {
                return this["TempRiskVehicle"];
            }
        }
    }
}
