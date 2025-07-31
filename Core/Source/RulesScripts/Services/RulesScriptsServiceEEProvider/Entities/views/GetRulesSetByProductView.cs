using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetRulesSetByProductView : BusinessView
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

        public BusinessCollection GroupCoverages
        {
            get
            {
                return this["GroupCoverage"];
            }
        }
    }
}

