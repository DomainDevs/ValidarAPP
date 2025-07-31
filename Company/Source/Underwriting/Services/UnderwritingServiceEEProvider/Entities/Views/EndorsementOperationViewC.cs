using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class EndorsementOperationViewC : BusinessView
    {
        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }
        
        public BusinessCollection EndorsementOperations
        {
            get
            {
                return this["EndorsementOperation"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection TempPolicyControl
        {
            get
            {
                return this["TempPolicyControl"];
            }
        }

    }
}