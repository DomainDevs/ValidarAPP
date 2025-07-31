using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Data;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    ///  estados de contragarantías
    /// </summary>
    public class GuaranteeStatusDAO
    {
        /// <summary>
        /// Obtiene los estados de contragarantías
        /// </summary>
        /// <returns> Listado de contragarantías </returns>
        public List<GuaranteeStatus> GetGuaranteeStatus()
        {
            return ModelAssembler.CreateGuaranteesStatus(DataFacadeManager.GetObjects(typeof(COMMEN.GuaranteeStatus)));
        }

        public List<GuaranteeStatus> GetUnassignedGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId)
        {
            List<GuaranteeStatus> guaranteeStatuses = GetGuaranteeStatus();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.GuaranteeStatus.Properties.GuaranteeStatusCode, typeof(COMMEN.GuaranteeStatus).Name, guaranteeStatusId);

            BusinessCollection entitiesGuaranteeStatus = DataFacadeManager.GetObjects(typeof(COMMEN.GuaranteeStatus), filter.GetPredicate()); 

            if (entitiesGuaranteeStatus.Count > 0)
            {
                GuaranteeStatusView guaranteeStatusView = new GuaranteeStatusView();
                ViewBuilder viewBuilder = new ViewBuilder("GuaranteeStatusView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, guaranteeStatusView);

                List<COMMEN.StatusRoute> entityStatusRoutes = guaranteeStatusView.StatusRoutes.Cast<COMMEN.StatusRoute>().ToList();

                guaranteeStatuses.RemoveAll(x => entityStatusRoutes.Any(y => y.AssignedGuaranteeStatusCode == x.Id));
            }

            return guaranteeStatuses;
        }

        public List<GuaranteeStatus> GetGuaranteeStatusRoutesByGuaranteeStatusId(int guaranteeStatusId)
        {
            List<GuaranteeStatus> guaranteeStatuses = GetGuaranteeStatus();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.GuaranteeStatus.Properties.GuaranteeStatusCode, typeof(COMMEN.GuaranteeStatus).Name, guaranteeStatusId);
            BusinessCollection entitiesGuaranteeStatus = DataFacadeManager.GetObjects(typeof(COMMEN.GuaranteeStatus), filter.GetPredicate());
            if (entitiesGuaranteeStatus.Count > 0)
            {
                GuaranteeStatusView guaranteeStatusView = new GuaranteeStatusView();
                ViewBuilder viewBuilder = new ViewBuilder("GuaranteeStatusView");
                viewBuilder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, guaranteeStatusView);
                List<COMMEN.StatusRoute> entityStatusRoutes = guaranteeStatusView.StatusRoutes.Cast<COMMEN.StatusRoute>().ToList();
                guaranteeStatuses = guaranteeStatuses.Where(x => entityStatusRoutes.Any(y => y.AssignedGuaranteeStatusCode== x.Id)).ToList();
            }
            return guaranteeStatuses;
        }

        public List<GuaranteeStatus> CreateGuaranteeStatusRoutes(List<GuaranteeStatus> allGuaranteeEstatusAssign, int guaranteeStatusId)
        {
            try
            {
                DeleteGuaranteeStatusRoute(guaranteeStatusId);
                if(allGuaranteeEstatusAssign!=null && allGuaranteeEstatusAssign.Count > 0)
                {
                    SaveGuaranteeStatusRoute(allGuaranteeEstatusAssign, guaranteeStatusId);
                  
                }
                return allGuaranteeEstatusAssign;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error in CreateGuaranteeStatusRoutes", ex);
            }
        }

        public void SaveGuaranteeStatusRoute(List<GuaranteeStatus> allGuaranteeEstatusAssign, int guaranteeStatusId)
        {
            foreach(GuaranteeStatus GuaranteeEstatusAssign in allGuaranteeEstatusAssign)
            {
                // Convertir de model a entity
                COMMEN.StatusRoute entityGuaranteeStatusRoute = EntityAssembler.CreateStatusRoute(GuaranteeEstatusAssign.Id, guaranteeStatusId);
                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityGuaranteeStatusRoute);
            }
            
        }

        public void DeleteGuaranteeStatusRoute(int guaranteeStatusId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.StatusRoute.Properties.GuaranteeStatusCode);
                filter.Equal();
                filter.Constant(guaranteeStatusId);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.StatusRoute), filter.GetPredicate());
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error in DeleteGuaranteeStatusRoute", ex);
            }
        }

        public List<GuaranteeStatus> GetGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(COMMEN.GuaranteeStatus.Properties.GuaranteeStatusCode, typeof(COMMEN.GuaranteeStatus).Name, guaranteeStatusId);
                BusinessCollection guaranteeStatusEntity = DataFacadeManager.GetObjects(typeof(COMMEN.GuaranteeStatus), filter.GetPredicate());
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateteAgentPrefix");
                return ModelAssembler.CreateGuaranteesStatus(guaranteeStatusEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error in CreateGuaranteeStatusRoutes", ex);
}
        }
    }
}