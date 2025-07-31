using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class LineBusinessClauseLevelParametrizationView : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
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
