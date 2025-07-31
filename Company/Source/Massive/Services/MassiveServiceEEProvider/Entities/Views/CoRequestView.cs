using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Entities.View
{
    [Serializable()]
    public class CoRequestView : BusinessView
    {
        public BusinessCollection CoRequest
        {
            get
            {
                return this["CoRequest"];
            }
        }

        public BusinessCollection CoRequestEndorsement
        {
            get
            {
                return this["CoRequestEndorsement"];
            }
        }

        public BusinessCollection CoRequestEndorsementAgent
        {
            get
            {
                return this["CoRequestEndorsementAgent"];
            }
        }

        public BusinessCollection CoRequestAgent
        {
            get
            {
                return this["CoRequestAgent"];
            }
        }

        public BusinessCollection Agent
        {
            get
            {
                return this["Agent"];
            }
        }

        public BusinessCollection BillingGroup
        {
            get
            {
                return this["BillingGroup"];
            }
        }
    }
}
