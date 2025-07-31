using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class JudicialSuretyModificationController : ModificationController
    {
        [Authorize]
        public ActionResult CreateTemporal(ModificationViewModel modificationModel)
        {
            try
            {
                if (modificationModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                    ModelState["modificationModel.BusinessTypeDescription"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                    var policy = DelegateService.JudicialSuretyModificationService.CreateTemporal(CompanyEndorsement, false);
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
                var policy = DelegateService.JudicialSuretyTextService.CreateTexts(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
            }
        }
        public ActionResult CreateClauses(ModificationViewModel modificationModel)///pendiente
        {
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyPolicy(modificationModel);
                var policy = DelegateService.JudicialSuretyClauseService.CreateClauses(CompanyPolicy);
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