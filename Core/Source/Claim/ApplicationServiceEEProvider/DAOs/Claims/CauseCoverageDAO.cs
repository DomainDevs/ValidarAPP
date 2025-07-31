using CLMEN = Sistran.Core.Application.Claims.Entities;
using System.Collections.Generic;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class CauseCoverageDAO
    {
        public CauseCoverage CreateCauseCoverage(CauseCoverage causeCoverage)
        {
            CLMEN.CauseCoverage entityCauseCoverage = EntityAssembler.CreateCauseCoverage(causeCoverage);
            return ModelAssembler.CreateCauseCoverage((CLMEN.CauseCoverage)DataFacadeManager.Insert(entityCauseCoverage));
        }

        public void DeleteCauseCoverage(int causeId, int coverageId)
        {
            PrimaryKey primaryKey = CLMEN.CauseCoverage.CreatePrimaryKey(causeId, coverageId);
            DataFacadeManager.Delete(primaryKey);
        }

        public List<CauseCoverage> GetCauseCoveragesByCauseId(int causeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.CauseCoverage.Properties.CauseId, typeof(CLMEN.CauseCoverage).Name, causeId);

            return ModelAssembler.CreateCauseCoverages(DataFacadeManager.GetObjects(typeof(CLMEN.CauseCoverage), filter.GetPredicate()));
        }
    }
}
