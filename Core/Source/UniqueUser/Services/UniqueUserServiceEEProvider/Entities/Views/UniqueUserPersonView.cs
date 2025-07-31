using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UniqueUserPersonView : BusinessView
    {
        public BusinessCollection People
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection UniqueUser
        {
            get
            {
                return this["UniqueUsers"];
            }
        }
    }
}
