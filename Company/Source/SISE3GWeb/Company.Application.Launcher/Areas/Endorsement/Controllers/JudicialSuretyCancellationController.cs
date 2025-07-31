using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Models;
using JSM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    [Authorize]
    public class JudicialSuretyCancellationController : CancellationController
    {
        PendingOperation pendingOperation = new PendingOperation();
        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                    CompanyEndorsement.OnlyCancelation = true;
                    var policy = DelegateService.JudicialSuretyCancellationService.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                    return new UifJsonResult(true, policy);

                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }

            }
            catch (Exception e)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public void ExecuteThread(List<JSM.CompanyJudgement> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                var policyExcute = DelegateService.JudicialSuretyCancellationService.ExecuteThread(risksThread, policy, risks);
            }
            catch (Exception)
            {

                new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

    }
}