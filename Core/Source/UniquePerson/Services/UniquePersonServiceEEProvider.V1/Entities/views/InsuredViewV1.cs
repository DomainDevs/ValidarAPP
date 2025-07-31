using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class InsuredViewV1 : BusinessView
    {
        public BusinessCollection Individuals
        {
            get
            {
                return this["Individual"];
            }
        }

        public BusinessCollection Persons
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection Companies
        {
            get
            {
                return this["Company"];
            }
        }

        public BusinessCollection Insureds
        {
            get
            {
                return this["Insured"];
            }
        }
    }
}
