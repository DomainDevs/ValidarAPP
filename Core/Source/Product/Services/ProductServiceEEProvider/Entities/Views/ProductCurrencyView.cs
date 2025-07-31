using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ProductServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ProductCurrencyView : BusinessView
    {
        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }

        public BusinessCollection ProductCurrencies
        {
            get
            {
                return this["ProductCurrency"];
            }
        }
    }
}
