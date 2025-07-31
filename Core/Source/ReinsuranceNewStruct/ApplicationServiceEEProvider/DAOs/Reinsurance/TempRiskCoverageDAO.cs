using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs
{
    internal class TempRiskCoverageDAO
    {

        public List<TempRiskCoverage> GetTempRiskCoverageByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            BusinessCollection businessObjects = new BusinessCollection(); 
            filter.PropertyEquals(REINSEN.TempRiskCoverage.Properties.RiskCode, typeof(REINSEN.TempRiskCoverage).Name, riskId);
            businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.TempRiskCoverage), filter.GetPredicate());
            return ModelAssembler.CreateTempRisksCoverages(businessObjects);
        }

    }
}
