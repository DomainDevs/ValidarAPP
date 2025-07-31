using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class LineBusinessClausesView : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection ClauseLevel
        {
            get
            {
                return this["ClauseLevel"];
            }
        }

        public BusinessCollection Clause
        {
            get
            {
                return this["Clause"];
            }
        }
    }
}
