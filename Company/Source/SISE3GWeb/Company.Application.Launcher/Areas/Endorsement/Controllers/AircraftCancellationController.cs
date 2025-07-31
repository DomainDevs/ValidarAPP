using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class AircraftCancellationController : CancellationController
    {
        //GET: Endorsement/AircraftCancellation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            try
            {
                CompanyEndorsement companyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                CompanyPolicy policy = DelegateService.aircraftCancellationServiceCia.CreateTemporalEndorsementCancellation(companyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }
    }
}