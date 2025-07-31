//Sistran Company
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.FinancialPlan;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
// sistran FWK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ISSDTO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
// Sistran Core

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    /// <summary>
    /// Recuotificacion Plan Financiero
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class FinancialPlanController : Controller
    {

        /// <summary>
        /// Recuotificacion Plan Financiero
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        public ActionResult FinancialPlan()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

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
        public async Task<ActionResult> GetPoliciesByQuery(int prefixId, int branchId, string query)
        {
            try
            {
                ViewBag.SyncOrAsync = "Asynchronous";
                List<decimal> policies = await DelegateService.underwritingIntegrationService.GetPoliciesByPolicyFilter(new ISSDTO.PolicyFilterDTO { DocumentNumber = Convert.ToDecimal(query), BranchId = branchId, PrefixId = prefixId });
                var result = policies.Select(m => new { Id = m }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the payers by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public ActionResult GetPayersByEndorsementId(int endorsementId)
        {
            try
            {
                List<PayerBaseDTO> payers = DelegateService.underwritingIntegrationService.GetPayersByEndorsementFilter(new FilterBaseDTO { Id = endorsementId });
                return new UifJsonResult(true, payers);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, null);
            }

        }
        #region Adicioan
        public ActionResult GetFinancialPolicyInfo(int endorsementId)
        {
            try
            {
                BasePaymentPlan payers = DelegateService.underwritingIntegrationService.GetFinancialPolicyInfo(new FilterBaseDTO { Id = endorsementId });
                return new UifJsonResult(true, payers);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, null);
            }

        }
        public ActionResult GetPaymentsScheduleByProductId(int productId)
        {
            try
            {
                List<ComboBaseDTO> comboBaseDTOs = DelegateService.underwritingIntegrationService.GetPaymentsScheduleByProductId(new FilterBaseDTO { Id = productId });
                return new UifJsonResult(true, comboBaseDTOs);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, null);
            }

        }

        public ActionResult QuotePaymentPlan(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            try
            {
                FinancialDTO financialDTO = DelegateService.financialPlanService.CreateFinancialPlan(filterFinancialPlanDTO);
                return new UifJsonResult(true, financialDTO);

            }
            catch (BusinessException ex)
            {

                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePaymentPlan);
            }
        }

        public ActionResult CreatePaymentPlan(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            try
            {
                FinancialPlanValidate.Validate(filterFinancialPlanDTO);
                bool result = DelegateService.financialPlanService.CreateQuotas(filterFinancialPlanDTO);
                if (result)
                    return new UifJsonResult(true, result);
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePaymentPlan);

            }
            catch (BusinessException ex)
            {

                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePaymentPlan);
            }
        }
        public ActionResult GetPaymentMethod()
        {
            try
            {
                var payementMethod = DelegateService.commonService.GetPaymentMethods();
                return new UifJsonResult(true, payementMethod);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, null);
            }
        }
        #endregion

    }
}