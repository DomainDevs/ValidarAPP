using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PaymentPlanView : BusinessView
    {
        public BusinessCollection ProductFinancialPlans
        {
            get
            {
                return this["ProductFinancialPlan"];
            }
        }

        public BusinessCollection FinancialPlans
        {
            get
            {
                return this["FinancialPlan"];
            }
        }

        public BusinessCollection PaymentSchedules
        {
            get
            {
                return this["PaymentSchedule"];
            }
        }
    }
}
