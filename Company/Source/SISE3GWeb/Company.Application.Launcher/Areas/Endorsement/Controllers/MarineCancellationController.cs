using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class MarineCancellationController : CancellationController
    {
        // GET: Endorsement/MarineCancellation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            try
            {
                CompanyEndorsement companyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                CompanyPolicy policy = DelegateService.marineCancellationServiceCia.CreateTemporalEndorsementCancellation(companyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }
    }
}