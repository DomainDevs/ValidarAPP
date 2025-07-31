using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.View
{
     public class UtilitysDetailsView : BusinessView
    {
        public BusinessCollection UtilityDetails
        {
            get
            {
                return this["UtilityDetails"];
            }
        }

        public BusinessCollection UtilityType
        {
            get
            {
                return this["UtilityType"];
            }
        }

        public BusinessCollection UtilitySummary
        {
            get
            {
                return this["UtilitySummary"];
            }
        }
    }
}
