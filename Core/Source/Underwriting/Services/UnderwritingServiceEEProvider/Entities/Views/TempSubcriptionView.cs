using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class TempSubcriptionView : BusinessView
    {
        public BusinessCollection TempSubscription
        {
            get
            {
                return this["TempSubscription"];
            }
        }

        public BusinessCollection TempSubscriptionAgents
        {
            get
            {
                return this["TempSubscriptionAgent"];
            }
        }

        public BusinessCollection AgentAgency
        {
            get
            {
                return this["AgentAgency"];
            }
        }

        public BusinessCollection Agent
        {
            get
            {
                return this["Agent"];
            }
        }

        public BusinessCollection TempSubscriptionPayers
        {
            get
            {
                return this["TempSubscriptionPayer"];
            }
        }

        public BusinessCollection TempPayerPayments
        {
            get
            {
                return this["TempPayerPayment"];
            }
        }
    }
}
