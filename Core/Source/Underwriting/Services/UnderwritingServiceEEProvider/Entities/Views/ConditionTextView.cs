using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ConditionTextView : BusinessView
    {
        public BusinessCollection ConditionText
        {
            get
            {
                return this["ConditionText"];
            }
        }

        public BusinessCollection CondTextLevel
        {
            get
            {
                return this["CondTextLevel"];
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