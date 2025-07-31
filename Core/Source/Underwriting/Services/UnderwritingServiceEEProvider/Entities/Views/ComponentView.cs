using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ComponentView : BusinessView
    {
        public BusinessCollection Components
        {
            get
            {
                return this["Component"];
            }
        }

        public BusinessCollection ExpenseComponents
        {
            get
            {
                return this["ExpenseComponent"];
            }
        }  
    }
}
