using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class FidelityModificationController : ModificationController
    {
        public ActionResult CreateTemporal(ModificationViewModel modificationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                    var policy = DelegateService.fidelityModificationService.CreateTemporal(CompanyEndorsement, false);
                    return new UifJsonResult(true, policy);

                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }


        public ActionResult CreateTexts(ModificationViewModel modificationModel)
        {
            try
            {
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                var policy = DelegateService.fidelityTextService.CreateTexts(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
            }

        }
        public ActionResult CreateClauses(ModificationViewModel modificationModel)
        {
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyPolicy(modificationModel);
                var policy = DelegateService.fidelityClauseService.CreateClauses(CompanyPolicy);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.SelectAllRequiredClauses);
                }

            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
            }

        }
    }
}