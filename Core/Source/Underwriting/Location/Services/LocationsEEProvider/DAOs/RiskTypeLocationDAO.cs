using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Locations.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using UNDModel = Sistran.Core.Application.UnderwritingServices.Models;
namespace Sistran.Core.Application.Locations.EEProvider.DAOs
{
    public class RiskTypeLocationDAO
    {
        /// <summary>
        /// Obtener lista de tipos de construccion
        /// </summary>
        /// <returns>Lista de tipos de riesgos</returns>
        public List<UNDModel.RiskType> GetRiskTypes()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskTypeLocation), filter.GetPredicate()));
            return ModelAssembler.CreateRiskTypes(businessCollection);
        }

    }
}
