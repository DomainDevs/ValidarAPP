using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class VehicleCancellationController : CancellationController
    {
        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                    CompanyEndorsement.Text.TextBody = underwritingController.unicode_iso8859(CompanyEndorsement.Text.TextBody);
                var policy = DelegateService.vehicleCancellationServiceCia.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, e.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }

        }


        public void ExecuteThread(List<VEM.CompanyVehicle> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                var policyExcute = DelegateService.vehicleCancellationServiceCia.ExecuteThread(risksThread, policy, risks);
            }
            catch (Exception)
            {

                new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

      
    }
}