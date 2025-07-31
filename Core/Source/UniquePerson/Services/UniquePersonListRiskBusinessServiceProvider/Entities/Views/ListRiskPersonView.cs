using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class ListRiskPersonView : BusinessView
    {
        public BusinessCollection RiskMaintenances
        {
            get { return this["RiskMaintenance"]; }
        }
        public BusinessCollection RiskAssignedLists
        {
            get { return this["RiskAssignedList"]; }
        }
        public BusinessCollection RiskLists
        {
            get { return this["RiskList"]; }
        }
    }
}
