using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Salvage
{
    [Serializable()]
    public class SalvageSaleView : BusinessView
    {
        public BusinessCollection Salvage
        {
            get
            {
                return this["Salvage"];
            }
        }

        public BusinessCollection Sales
        {
            get
            {
                return this["Sale"];
            }
        }
    }
}