using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class FidelityExtensionController : ExtensionController
    {
        public ActionResult CreateTemporal(ExtensionViewModel extensionModel)
        {
            try
            {
                if (extensionModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                    ModelState["extensionModel.BusinessTypeDescription"].Errors.Clear();

                if (ModelState.IsValid)
                {
                    var companyPolicy = ModelAssembler.CreateCompanyEndorsement(extensionModel);
                    var policy = DelegateService.fidelityExtensionService.CreateExtension(companyPolicy);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }
    }
}