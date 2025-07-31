using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class EstimationTypePrefixController : Controller
    {
        public ActionResult EstimationTypePrefix()
        {
            return View();
        }

        public ActionResult GetEstimationTypes()
        {
            return new UifSelectResult(DelegateService.claimApplicationService.GetEstimationTypes());
        }
        
        public JsonResult CreateEstimationTypePrefix(int estimationTypeId, List<PrefixDTO> PrefixesDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.CreatePrefixesByEstimationType(estimationTypeId, PrefixesDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateEstimationTypePrefix);
            }  
        }
        
        public JsonResult GetPrefixesByEstimationId(int estimationTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixesByEstimationTypeId(estimationTypeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        public JsonResult GetPrefixes()
        {
            return new UifSelectResult(DelegateService.claimApplicationService.GetPrefixes());
        }
    }
}