using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class EstimationTypeStatusController : Controller
    {

        public ActionResult EstimationTypeStatus()
        {
            return View();
        }

        /// <summary>
        /// GetEstimationTypes
        /// Retorna lista de conceptos de estimacion
        /// </summary>
        /// <returns>JsonResult</returns>
        public UifJsonResult GetEstimationTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEstimationType);
            }
        }

        /// <summary>
        /// GetPrefix
        /// Retorna lista Ramo
        /// </summary>
        /// <returns>JsonResult</returns>
        public UifJsonResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// GetEstimationsTypeStatus
        /// Carga la tabla Listado de Estados Del Aviso
        /// </summary>
        /// <returns>JsonResult</returns>
        public ActionResult GetStatusesByEstimationTypeId(int estimationTypeId)
        {
            List<StatusDTO> listStatus = DelegateService.claimApplicationService.GetStatusesByEstimationTypeId(estimationTypeId);
                        
            return new UifJsonData(true, listStatus);
        }
        /// <summary>
        /// GetEstimationTypeStatusUnassigned
        /// Carga la tabla Listado de Estados no asignados a una estimación
        /// </summary>
        /// <returns>JsonResult</returns>
        public ActionResult GetEstimationTypeStatusUnassignedByEstimationTypeId(int estimationTypeId)
        {
            List<StatusDTO> listStatus = DelegateService.claimApplicationService.GetEstimationTypeStatusUnassignedByEstimationTypeId(estimationTypeId);
            
            return new UifJsonData(true, listStatus);
        }

        /// <summary>
        /// GetStatuses
        /// Carga la tabla Listado de Estados
        /// </summary>
        /// <returns>JsonResult</returns>
        public ActionResult GetStatuses()
        {
            List<StatusDTO> listStatus = DelegateService.claimApplicationService.GetStatuses();

            return new UifJsonData(true, listStatus);
        }

        /// <summary>
        /// GetReasonsByStatusIdAndPrefixId
        /// Obtener razones por id estado
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public ActionResult GetReasonsByStatusIdPrefixId(int statusId, int prefixId)
        {
            try
            {
                List<ReasonDTO> reasonDTO = DelegateService.claimApplicationService.GetReasonsByStatusIdPrefixId(statusId, prefixId);
                
                return new UifJsonResult(true, reasonDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStatusReazon);
            }
        }
        public UifJsonResult CreateStatusByEstimationType(StatusDTO statusDTO)
        {
            try
            {
                DelegateService.claimApplicationService.CreateStatusByEstimationType(statusDTO);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateEstimationTypeStatus);
            }
        }

        public UifJsonResult DeleteStatusByEstimationType(StatusDTO statusDTO)
        {
            try
            {
                DelegateService.claimApplicationService.DeleteStatusByEstimationType(statusDTO);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteEstimationTypeStatus);
            }
        }

        /// <summary>
        /// CreateEstimationTypeStatusReason
        /// Crear Razones de Estados por Conceptos de Estimación
        /// </summary>
        /// <param name="statusDTO"></param>
        /// <returns></returns>
        public UifJsonResult CreateReason(ReasonDTO reasonDTO)
        {
            try
            {
                DelegateService.claimApplicationService.CreateReason(reasonDTO);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateStatusReazon);
            }
        }

        /// <summary>
        /// UpdateEstimationTypeStatusReason
        /// Actualizar Razones de Estados por Conceptos de Estimación
        /// </summary>
        /// <param name="statusDTO"></param>
        /// <returns></returns>
        public UifJsonResult UpdateReason(ReasonDTO reasonDTO)
        {
            try
            {
                DelegateService.claimApplicationService.UpdateReason(reasonDTO);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdateStatusReazon);
            }
        }

        /// <summary>
        /// DeleteEstimationTypeStatusReason
        /// Crear Razones de Estados por Conceptos de Estimación
        /// </summary>
        /// <param name="statusDTO"></param>
        /// <returns></returns>
        public UifJsonResult DeleteReason(int reasonId, int statusId, int prefixId)
        {
            try
            {
                DelegateService.claimApplicationService.DeleteReason(reasonId, statusId, prefixId);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteStatusReazon);
            }
        }

    }
}