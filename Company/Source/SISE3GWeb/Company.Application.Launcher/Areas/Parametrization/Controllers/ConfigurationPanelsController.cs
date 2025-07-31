using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class ConfigurationPanelsController : Controller
    {
        // GET: Parametrization/ConfigurationPanels
        public ActionResult ConfigurationPanels()
        {
            return View();
        }

        public UifJsonResult GetCoveragesByLineBusinessIdSubLineBusiness(int lineBusinessId, int subLineBusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverage);
            }
        }

        public UifJsonResult GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimCoverageActivePanelsByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverage);
            }
        }

        public UifJsonResult CreateActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanels)
        {
            try
            {
                    ClaimCoverageActivePanelDTO claimCoverageActivePane = DelegateService.claimApplicationService.CreateCoverageActivePanel(claimCoverageActivePanels);
                    return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorActivePanel);
            }
        }

        public UifJsonResult UpdateActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanels)
        {
            try
            {
                ClaimCoverageActivePanelDTO claimCoverageActivePane = DelegateService.claimApplicationService.UpdateCoverageActivePanel(claimCoverageActivePanels);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorActivePanelUpdate);
            }
        }
    }
}