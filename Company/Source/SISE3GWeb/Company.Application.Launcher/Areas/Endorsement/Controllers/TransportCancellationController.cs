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
    public class TransportCancellationController : CancellationController
    {
        // GET: Endorsement/TransportCancellation
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            try
            {
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                var policy = DelegateService.transportCancellationServiceCia.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception e)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }


        //public void ExecuteThread(List<VEM.CompanyVehicle> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        //{
        //    try
        //    {
        //        var policyExcute = DelegateService.vehicleCancellationServiceCia.ExecuteThread(risksThread, policy, risks);
        //    }
        //    catch (Exception)
        //    {

        //        new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
        //    }
        //}
    }
}