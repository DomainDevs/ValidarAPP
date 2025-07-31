using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class ClausePrefixParametrizationView : BusinessView
    {
        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
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
