using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PersonUUView : BusinessView
    {
        public BusinessCollection People
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection Emails
        {
            get
            {
                return this["Email"];
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
