using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class AircraftRenewalController : RenewalController
    {
        // GET: Endorsement/AircraftRenewal
        public ActionResult CreateTemporal(RenewalViewModel renewalModel)
        {
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyPolicyByRenewal(renewalModel);
                var policy = DelegateService.aircraftRenewalService.CreateRenewal(CompanyPolicy);
                if (policy != null)
                {
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