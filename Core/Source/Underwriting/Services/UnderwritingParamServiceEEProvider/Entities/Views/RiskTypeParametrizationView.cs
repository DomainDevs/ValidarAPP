using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class RiskTypeParametrizationView : BusinessView
    {
        public BusinessCollection RiskType
        {
            get
            {
                return this["RiskType"];
            }
        }
        public BusinessCollection ClauseLevels
        {
            get
            {
                return this["ClauseLevel"];
            }
        }
    }
}
