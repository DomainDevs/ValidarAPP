using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;


namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ProfileGuaranteeStatusView : BusinessView
    {
        public BusinessCollection ProfileGuaranteeStatus
        {
            get
            {
                return this["ProfileGuaranteeStatus"];
            }
        }
        public BusinessCollection Profiles
        {
            get
            {
                return this["Profiles"];
            }
        }
        public BusinessCollection GuaranteeStatus
        {
            get
            {
                return this["GuaranteeStatus"];
            }
        }

    }
}
