using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class PersonView : BusinessView
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
