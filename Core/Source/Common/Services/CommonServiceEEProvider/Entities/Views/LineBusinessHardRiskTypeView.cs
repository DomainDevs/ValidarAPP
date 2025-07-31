using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class LineBusinessHardRiskTypeView : BusinessView
    {
        public BusinessCollection LinesBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection HardRiskTypes
        {
            get
            {
                return this["HardRiskType"];
            }
        }
    }
}