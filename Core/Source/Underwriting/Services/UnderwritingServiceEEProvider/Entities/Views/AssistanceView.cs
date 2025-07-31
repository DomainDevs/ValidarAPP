using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AssistanceView : BusinessView
    {
        public BusinessCollection CptAssistanceText
        {
            get
            {
                return this["CptAssistanceText"];
            }
        }

        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }
    }
}
