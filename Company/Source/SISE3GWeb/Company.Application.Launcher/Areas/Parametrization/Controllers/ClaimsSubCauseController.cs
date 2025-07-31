using Sistran.Core.Application.ClaimServices.DTOs.Claims;
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
    public class ClaimsSubCauseController : Controller
    {
        // GET: Parametrization/ConfigurationPanels
        public ActionResult ClaimsSubCause()
        {
            return View();
        }

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

        public UifJsonResult GetCausesByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCausesByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCauses);
            }
        }

        public UifJsonResult GetSubCausesByCauseId(int causeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSubCausesByCause(causeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCauses);
            }
        }

        public UifJsonResult ExecuteSubCauseOperations(SubCauseDTO subCause)
        {
            try
            {
                if (subCause.Id == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateSubCause(subCause));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateSubCause(subCause));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveSubCauses);
            }
        }

        public UifJsonResult DeleteSubCause(int subCauseId)
        {
            try
            {
                DelegateService.claimApplicationService.DeleteSubCause(subCauseId);
                return new UifJsonResult(true, App_GlobalResources.Language.DeleteSubCause);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteSubCause);
            }
        }
    }
}