using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class LineBusinessCoveredRiskType : BusinessView
    {
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
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
