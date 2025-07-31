using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class InsuredObjectCoveredRiskTypeView : BusinessView
    {
   
		public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }

		public BusinessCollection LbInsuredObjects
        {
            get
            {
                return this["LbInsuredObject"];
            }
        }
		
        public BusinessCollection  CoveredRiskTypes
        {
            get
            {
                return this["CoveredRiskType"];
            }
        }
		
		public BusinessCollection  LineBusiness
        {
            get
            {
                return this["LineBusinessCoveredRiskType"];
            }
        }
    }
}
