using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class PersonViewV1 : BusinessView
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
