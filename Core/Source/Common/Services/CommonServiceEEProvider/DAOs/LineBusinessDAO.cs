using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class LineBusinessDAO
    {
        /// <summary>
        /// Obtener lista de ramos tecnicos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLinesBusinessByPrefixId(int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            LineBusinessView view = new LineBusinessView();
            ViewBuilder builder = new ViewBuilder("LineBusinessView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.PrefixLineBusiness.Properties.PrefixCode, typeof(COMMEN.PrefixLineBusiness).Name);
            filter.Equal();
            filter.Constant(prefixId);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetLinesBusinessByPrefixId");
            return ModelAssembler.CreateLinesBusiness(view.LineBusiness);
        }

        /// <summary>
        /// Obtener lista de ramos tecnicos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public int GetLinesBusinessCodeByPrefixId(int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.PrefixLineBusiness.Properties.PrefixCode, typeof(COMMEN.PrefixLineBusiness).Name);
            filter.Equal();
            filter.Constant(prefixId);
            

            var linebusiness = (COMMEN.PrefixLineBusiness) DataFacadeManager.GetObjects(typeof(COMMEN.PrefixLineBusiness),filter.GetPredicate()).First();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetLinesBusinessCodeByPrefixId");
            return linebusiness.LineBusinessCode;
        }

        /// <summary>
        /// Obtener lista de negocio
        /// </summary>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLinesBusiness()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection prefixList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.LineBusiness)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetLinesBusiness");
            return ModelAssembler.CreateLinesBusiness(prefixList);
        }

        /// <summary>
        /// Obtener Ramos Tecnicos
        /// </summary>
        /// <param name="coveredRiskType">Tipo De Riesgo</param>
        /// <returns>Ramos Tecnicos</returns>
        public List<COMMML.LineBusiness> GetLinesBusinessByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            List<COMMML.LineBusiness> linesBusiness = new List<COMMML.LineBusiness>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name).Equal().Constant(coveredRiskType);

            LineBusinessHardRiskTypeView view = new LineBusinessHardRiskTypeView();
            ViewBuilder builder = new ViewBuilder("LineBusinessHardRiskTypeView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.LinesBusiness.Count > 0)
            {
                linesBusiness = ModelAssembler.CreateLinesBusiness(view.LinesBusiness);
            }

            return linesBusiness;
        }


        /// <summary>
        /// Obtener Codigos CoveredRiskType de HardRiskType
        /// </summary>
        /// <param name="coveredRiskType">Tipo De Riesgo</param>
        /// <returns>Ramos Tecnicos</returns>
        public List<COMMML.LineBusiness> GetHardRiskTypeByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            List<COMMML.LineBusiness> linesBusiness = new List<COMMML.LineBusiness>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name).Equal().Constant(coveredRiskType);

            LineBusinessHardRiskTypeView view = new LineBusinessHardRiskTypeView();
            ViewBuilder builder = new ViewBuilder("LineBusinessHardRiskTypeView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.LinesBusiness.Count > 0)
            {
                linesBusiness = ModelAssembler.CreateCoveredRiskType(view.LinesBusiness, view.HardRiskTypes);
            }

            return linesBusiness;
        }
    }
}