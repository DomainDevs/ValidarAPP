using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.PrintingServicesEEProvider.Entities.views
{
    [Serializable()]
    public class TempRiskVehiclePrintingView : BusinessView
    {
        public BusinessCollection TempRisks
        {
            get
            {
                return this["TempRisk"];
            }
        }

        public BusinessCollection TempRiskVehicles
        {
            get
            {
                return this["TempRiskVehicle"];
            }
        }
    }
}
