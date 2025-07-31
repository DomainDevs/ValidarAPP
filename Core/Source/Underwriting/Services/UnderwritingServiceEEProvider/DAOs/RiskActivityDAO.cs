using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RiskActivityDAO
    {
        /// Obtener lista de actividades del riesgo
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="description">descripcion</param>
        /// <returns>Lista de actividades</returns>
        public List<RiskActivity> GetRiskActivitiesByProductIdDescription(int productId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ProductRiskCommercialClass.Properties.ProductId, typeof(COMMEN.ProductRiskCommercialClass).Name);
            filter.Equal();
            filter.Constant(productId);
            RiskActivityView view = new RiskActivityView();
            ViewBuilder builder = new ViewBuilder("RiskActivityView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<RiskActivity> riskActivities = ModelAssembler.CreateRiskActivities(view.RiskCommercialClasses);
            return riskActivities;
        }

        /// <summary>
        /// Obtener actividad por id
        /// </summary>
        /// <param name="activityId">Id de actividad</param>
        /// <returns>actividad</returns>
        public RiskActivity GetRiskActivityByActivityId(int activityId)
        {
            PrimaryKey keyRiskCommercialClass = PARAMEN.RiskCommercialClass.CreatePrimaryKey(activityId);
            PARAMEN.RiskCommercialClass riskCommercialClassEntity = (PARAMEN.RiskCommercialClass)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyRiskCommercialClass);

            if (riskCommercialClassEntity != null)
            {
                return ModelAssembler.CreateRiskActivity(riskCommercialClassEntity);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener actividad por tipo
        /// </summary>
        /// <param name="activityId">Id de actividad</param>
        /// <returns>actividad</returns>
        public List<RiskActivity> GetRiskActivityTypeByActivityId(int activityId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.RiskCommercialType.Properties.RiskCommercialClassCode, typeof(PARAMEN.RiskCommercialType).Name);
            filter.Equal();
            filter.Constant(activityId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.RiskCommercialType), filter.GetPredicate()));
            return ModelAssembler.CreateRiskActivitiesType(businessCollection);
        }
    }
}
