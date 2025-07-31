using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class ClaimsDocumentController : Controller
    {
        // GET: Parametrization/ClaimsDocument
        public ActionResult ClaimsDocument()
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

        public UifJsonResult GetCompanyModule(string description)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetModule(""));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }

        }

        public UifJsonResult GetCompanySubModule(int moduleId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSubModule(moduleId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }

        }

        public UifJsonResult GetDocumentBySubmoduleId(int SubmoduleId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDocumentationBySubmoduleId(SubmoduleId));
            }
            catch (Exception ex )
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        public UifJsonResult ExecuteDocumentOperatios(ClaimDocumentationDTO claimsDocumentationDTO)
        {
            try
            {
                if (claimsDocumentationDTO.Id == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateDocumentationes(claimsDocumentationDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateDocumentation(claimsDocumentationDTO));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveSubCauses);
            }
        }

        public UifJsonResult DeleteDocumentation(int Id)
        {
            try
            {
                DelegateService.claimApplicationService.DeleteDocumentation(Id);
                return new UifJsonResult(true, App_GlobalResources.Language.DeleteSubCause);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteSubCause);
            }
        }
    }
}