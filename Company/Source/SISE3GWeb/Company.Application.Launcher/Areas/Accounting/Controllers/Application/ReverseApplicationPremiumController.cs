//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Filter;
using Sistran.Core.Application.CommonService.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
//Sistran Company
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Integration.AccountingServices.DTOs.Reversion;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ISSDTO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    public class ReverseApplicationPremiumController : Controller
    {
        // GET: Accounting/ReverseApplicationPremiun
        public ActionResult ReverseApplicationPremium()
        {
            return View();
        }
        #region combos parametricos
        /// <summary>
        /// Obtener las Sucursales
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBranches()
        {
            List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
            return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
        }

        /// <summary>
        ///Obtener los ramos
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixes()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
            return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
        }
        #endregion combos parametricos

        /// <summary>
        /// GetEndorsementsByPolicyFilter
        /// Obtiene endoso a partir del id de la póliza
        /// </summary>
        /// <param name="policyFilter">The policy filter.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        public JsonResult GetEndorsementsByPolicyFilter(PolicyFilterDTO policyFilter)
        {
            try
            {
                List<ISSDTO.EndorsementBaseDTO> endorsementDTOs = DelegateService.underwritingIntegrationService.GetEndorsementsByPolicyFilter(policyFilter);
                return new UifJsonResult(true, endorsementDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }

        }



        /// <summary>
        /// Gets the policies by query.
        /// </summary>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="branchId">The branch identifier.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPoliciesByQuery(string query)
        {
            try
            {
                ViewBag.SyncOrAsync = "Asynchronous";
                List<decimal> policies = await DelegateService.underwritingIntegrationService.GetPoliciesByPolicyFilter(new ISSDTO.PolicyFilterDTO { DocumentNumber = Convert.ToDecimal(query), BranchId = 0, PrefixId = 0 });
                var result = policies.Select(m => new { Id = m }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetApplicationPremium(int endorsementId)
        {
            try
            {
                List<QuotaPremiumDTO> ApplicationPremiums = DelegateService.accountingIntegrationService.GetApplicationPremiumByEndorsementId(endorsementId);
                return new UifJsonResult(true, ApplicationPremiums);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSelectReversionReason);
            }

        }
        public ActionResult SaveTempPremiumReversion(ReversionFilterDTO reversionFilterDTO)
        {
            try
            {
                bool result = DelegateService.accountingReversionService.ReversionTempPremium(reversionFilterDTO);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSelectReversionReason);
            }

        }

        public JsonResult ReverseApplicationPremiumByPremiumId(int AppPremiumId)
        {
            try
            {
                bool ApplicationPremium = DelegateService.accountingReversionService.ReversionPremiumByFilterApp(new ReversionFilterDTO { AppId = AppPremiumId });
                return new UifJsonResult(ApplicationPremium, null);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSelectReversionReason);
            }
        }
        public JsonResult ValidatePremiumTemporal(PremiumFilterDTO premiumFilterDTO)
        {
            try
            {
                TemporalPremiumDTO ApplicationPremium = DelegateService.accountingApplicationService.ValidatePremiumTemporal(new PremiumFilterDTO { Id = premiumFilterDTO.Id, IsReversion = premiumFilterDTO.IsReversion, Number = premiumFilterDTO.Number, PayerId = premiumFilterDTO.PayerId, PremiumId = premiumFilterDTO.PremiumId });
                return new UifJsonResult(true, ApplicationPremium);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSelectReversionReason);
            }
        }
    }

}