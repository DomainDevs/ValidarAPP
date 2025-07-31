using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FID = Sistran.Company.Application.Finances.FidelityServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class FidelityCancellationController : CancellationController
    {
        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            try
            {
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                var policy = DelegateService.fidelityCancellationServiceCia.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception e)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }


        public void ExecuteThread(List<FID.CompanyFidelityRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                var policyExcute = DelegateService.fidelityCancellationServiceCia.ExecuteThread(risksThread, policy, risks);
            }
            catch (Exception)
            {

                new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

    }
}