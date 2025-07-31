using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ClauseView : BusinessView
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
