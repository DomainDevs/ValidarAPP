using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CoverGroupRiskTypeDAO
    {
        /// <summary>
        /// Obtener Grupos de Coberturas
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        public List<Model.GroupCoverage> GetAllGroupCoverages()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoverGroupRiskType.Properties.Enabled, typeof(CoverGroupRiskType).Name);
            filter.Equal();
            filter.Constant(true);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoverGroupRiskType)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetAllGroupCoverages");

            return ModelAssembler.CreateGroupCoverages(businessCollection);
        }
        /// <summary>
        /// Obtener Grupos de Coberturas por tipo de riesgo
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        public List<Model.GroupCoverage> GetGroupCoveragesByRiskTypeId(int riskTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoverGroupRiskType.Properties.CoveredRiskTypeCode, typeof(CoverGroupRiskType).Name);
            filter.Equal();
            filter.Constant(riskTypeId);
            filter.And();
            filter.Property(CoverGroupRiskType.Properties.Enabled, typeof(CoverGroupRiskType).Name);
            filter.Equal();
            filter.Constant(true);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoverGroupRiskType), filter.GetPredicate()));
            return ModelAssembler.CreateGroupCoverages(businessCollection);
        }
    }
}
