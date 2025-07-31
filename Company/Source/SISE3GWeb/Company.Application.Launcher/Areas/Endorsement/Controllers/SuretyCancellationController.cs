using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using SUM = Sistran.Company.Application.Sureties.SuretyServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyCancellationController : CancellationController
    {

        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                CompanyEndorsement.OnlyCancelation = true;
                if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                    CompanyEndorsement.Text.TextBody = underwritingController.unicode_iso8859(CompanyEndorsement.Text.TextBody);
                var policy = DelegateService.suretyCancellationServiceCia.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }

            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }

            }
        }


        public void ExecuteThread(List<SUM.CompanyContract> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                var policyExcute = DelegateService.suretyCancellationServiceCia.ExecuteThread(risksThread, policy, risks);
            }
            catch (Exception)
            {

                new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }
    }
}