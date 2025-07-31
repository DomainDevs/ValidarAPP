
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF2.Controls.UifSelect;
    using Sistran.Core.Framework.UIF.Web.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Sistran.Core.Application.UniquePersonService.V1.Models;
    using Sistran.Core.Framework.UIF.Web.Models;

    public class GuaranteeStatusRouteController : Controller
    {
        // GET: Parametrization/GuaranteeStatusRoute
        public ActionResult GuaranteeStatusRoute()
        {
            return View();
        }


        

        public ActionResult SaveGuaranteStatusRoute(List<GuaranteeStatusRouteViewModel> guaranteeStatusRoute)
        {
            return View();
        }

        #region Combos
        public ActionResult GetGuaranteeStatus()
        {
            try
            {
                List<GuaranteeStatus> guaranteeStatuss = DelegateService.uniquePersonService.GetGuaranteeStatus();
                return new UifJsonResult (true,guaranteeStatuss.OrderBy(x => x.Description));

            }
            catch (System.Exception)
            {
                return new UifSelectResult("");
            }
        }

        public ActionResult CreateGuaranteeStatusRoutes(List<GuaranteeStatus> allGuaranteeEstatusAssign, int guaranteeStatusId)
        {
            try
            {
                List<GuaranteeStatus> guaranteeStatuses = DelegateService.uniquePersonService.CreateGuaranteeStatusRoutes(allGuaranteeEstatusAssign, guaranteeStatusId);
                return new UifJsonResult(true, "");
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveContract);
            }
        }

        public UifJsonResult GetUnassignedGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId)
        {
            try
            {
                List<GuaranteeStatus> guaranteeStatuses = DelegateService.uniquePersonService.GetUnassignedGuaranteeStatusByGuaranteeStatusId(guaranteeStatusId);
                guaranteeStatuses = guaranteeStatuses.OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, guaranteeStatuses);
            }
            catch(Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            
        }

        public UifJsonResult GetGuaranteeStatusRoutesByGuaranteeStatusId(int guaranteeStatusId)
        {
            try
            {
                List<GuaranteeStatus> guaranteeStatuses = DelegateService.uniquePersonService.GetGuaranteeStatusRoutesByGuaranteeStatusId(guaranteeStatusId);
                guaranteeStatuses = guaranteeStatuses.OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, guaranteeStatuses);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }

        }
        #endregion


    }

   
}