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
    class CiaZonesCirculationByBranchView : BusinessView
    {
        public BusinessCollection CiaRatingZoneBranchs
        {
            get { return this["CiaRatingZoneBranch"]; }
        }
        public BusinessCollection RatingZones
        {
            get { return this["RatingZone"]; }
        }
        public BusinessCollection Prefixes
        {
            get { return this["Prefix"]; }
        }
        public BusinessCollection Branchs
        {
            get { return this["Branch"]; }
        }


    }
}
