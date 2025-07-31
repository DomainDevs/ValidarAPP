using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class BeneficiaryView : BusinessView
    {
        public BusinessCollection Companies
        {
            get
            {
                return this["Company"];
            }
        }

        public BusinessCollection Persons
        {
            get
            {
                return this["Person"];
            }
        }
    }
}
