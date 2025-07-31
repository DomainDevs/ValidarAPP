using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.TaxServices.views
{
    [Serializable()]
    public class TaxAttributeView : BusinessView
    {
        public BusinessCollection TaxAttributes
        {
            get
            {
                return this["TaxAttribute"];
            }
        }

        public BusinessCollection TaxAttributeTypes
        {
            get
            {
                return this["TaxAttributeType"];
            }
        }
    }
}