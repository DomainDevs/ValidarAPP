using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Locations.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;

namespace Sistran.Core.Application.Locations.EEProvider.DAOs
{
    public class RiskUseEarthquakeDAO
    {
        /// <summary>
        /// Obtener lista de tipos de construccion
        /// </summary>
        /// <returns></returns>
        public List<Models.RiskUse> GetRiskUses()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskUseEarthquake), filter.GetPredicate()));
            return ModelAssembler.CreateRiskUses(businessCollection);
        }

    }
}
