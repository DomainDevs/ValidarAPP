using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class CompanyCiaRatingZoneBranchView : BusinessView
    {
        public BusinessCollection CiaRatingZoneBranchs
        {
            get { return this["CiaRatingZoneBranch"]; }
        }

        public BusinessCollection RatingZones
        {
            get { return this["RatingZone"]; }
        }

    }
}
