using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class HardRiskTypeCoveredRiskType : BusinessView
    {
        public BusinessCollection HardRiskTypes
        {
            get
            {
                return this["HardRiskType"];
            }
        }

        public BusinessCollection LineBusinessByCoveredRiskType
        {
            get
            {
                return this["LineBusinessCoveredRiskType"];
            }
        }
    }
}
