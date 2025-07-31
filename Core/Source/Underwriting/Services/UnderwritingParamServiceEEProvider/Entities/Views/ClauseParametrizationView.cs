using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class ClauseParametrizationView : BusinessView
    {
        public BusinessCollection Clauses
        {
            get
            {
                return this["Clause"];
            }
        }

        public BusinessCollection ClauseLevels
        {
            get
            {
                return this["ClauseLevel"];
            }
        }

        public BusinessCollection ConditionLevels
        {
            get
            {
                return this["ConditionLevel"];
            }
        }

       
    }
}
