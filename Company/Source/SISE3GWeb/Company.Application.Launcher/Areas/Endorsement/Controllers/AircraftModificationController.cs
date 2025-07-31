
using System;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class AircraftModificationController : ModificationController
    {
        // GET: Endorsement/AircraftModification
        public ActionResult CreateTemporal(ModificationViewModel modificationModel)
        {
            try
            {
                if (modificationModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                    ModelState["modificationModel.BusinessTypeDescription"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                    var policy = DelegateService.aircraftModificationService.CreateTemporal(CompanyEndorsement, false);
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
                var policy = DelegateService.aircraftTextService.CreateTexts(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
            }

        }
        /// <summary>
        /// Crea 
        /// </summary>
        /// <param name="modificationModel"></param>
        /// <returns></returns>
        public ActionResult CreateClauses(ModificationViewModel modificationModel)
        {
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyPolicy(modificationModel);
                var policy = DelegateService.aircraftClauseService.CreateClauses(CompanyPolicy);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.SelectAllRequiredClauses);
                }
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
            }
        }


    }
}