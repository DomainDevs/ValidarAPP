using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Salvage
{
    [Serializable()]
    public class SaleView : BusinessView
    {
        public BusinessCollection Sales
        {
            get
            {
                return this["Sale"];
            }
        }

        public BusinessCollection PaymentPlans
        {
            get
            {
                return this["PaymentPlan"];
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