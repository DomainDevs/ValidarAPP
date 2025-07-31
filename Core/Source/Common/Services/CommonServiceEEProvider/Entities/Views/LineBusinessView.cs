using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class LineBusinessView : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection PrefixLineBusiness
        {
            get
            {
                return this["PrefixLineBusiness"];
            }
        }

        public BusinessCollection PerilLineBusiness
        {
            get
            {
                return this["PerilLineBusiness"];
            }
        }

        public BusinessCollection InsObjLineBusiness
        {
            get
            {
                return this["InsObjLineBusiness"];
            }
        }

        public BusinessCollection ClauseLevel
        {
            get
            {
                return this["ClauseLevel"];
            }
        }

        public BusinessCollection Peril
        {
            get
            {
                return this["Peril"];
            }
        }

        public BusinessCollection InsuredObject
        {
            get
            {
                return this["InsuredObject"];
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
