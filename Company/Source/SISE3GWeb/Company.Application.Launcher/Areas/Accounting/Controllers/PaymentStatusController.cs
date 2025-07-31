//System
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

// Sistran FWK
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using SearchDTOs = Sistran.Core.Application.AccountingServices.DTOs.Search;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class PaymentStatusController : Controller
    {
        #region Constants

        public const string SortOrder = "ASC";

        #endregion

        #region Instance Variables
       
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region Actions

        /// <summary>
        /// MainPaymentStatus
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPaymentStatus()
        {
            try
            {  

                ViewBag.SearchByForPaymentStatus = _commonController.GetPaymentSearchBy();

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementDocumentNumber"></param>
        /// <param name="policiesWithPortfolio"></param>
        /// <param name="pageNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PremiumReceivableStatus(string insuredId, string policyDocumentNumber, int branchId,
                                                  int prefixId, string endorsementDocumentNumber,
                                                  string policiesWithPortfolio, int pageNumber, string ExpirationDateFrom, string ExpirationDateTo)
        {
            string branchCode = (branchId != -1) ? Convert.ToString(branchId) : "";
            string prefixCode = (prefixId != -1) ? Convert.ToString(prefixId) : "";
            int pageSize = 100; // Para mostrar de 100 en 100 en el ListView

            try
            {

                List<SearchDTOs.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs =
                DelegateService.accountingApplicationService.PremiumReceivableStatus(insuredId, policyDocumentNumber, branchCode, prefixCode,
                                                      endorsementDocumentNumber, policiesWithPortfolio, pageSize, pageNumber, ExpirationDateFrom, ExpirationDateTo);

                List<object> premiumReceivablePolicies = new List<object>();


                foreach (SearchDTOs.PremiumReceivableSearchPolicyDTO premiumReceivableSearchPolicy in premiumReceivableSearchPolicyDTOs)
                {
                    premiumReceivablePolicies.Add(new
                    {
                        BranchPrefixPolicyEndorsement = premiumReceivableSearchPolicy.BranchPrefixPolicyEndorsement,
                        InsuredDocumentNumberName = premiumReceivableSearchPolicy.InsuredDocumentNumberName,
                        PayerDocumentNumberName = premiumReceivableSearchPolicy.PayerDocumentNumberName,
                        PaymentNumber = premiumReceivableSearchPolicy.PaymentNumber,
                        CurrencyDescription = premiumReceivableSearchPolicy.CurrencyDescription,
                        Amount = String.Format(new CultureInfo("en-US"), "{0:C}", premiumReceivableSearchPolicy.Amount),
                        PaidAmount = String.Format(new CultureInfo("en-US"), "{0:C}", premiumReceivableSearchPolicy.PaidAmount),
                        PaymentAmount = String.Format(new CultureInfo("en-US"), "{0:C}", premiumReceivableSearchPolicy.PaymentAmount),
                        PaymentExpirationDate = Convert.ToDateTime(premiumReceivableSearchPolicy.PaymentExpirationDate).ToShortDateString(),
                        // Hidden
                        BussinessTypeDescription = premiumReceivableSearchPolicy.BussinessTypeDescription,
                        PayerDocumentNumber = premiumReceivableSearchPolicy.PayerDocumentNumber,
                        InsuredDocumentNumber = premiumReceivableSearchPolicy.InsuredDocumentNumber,
                        PolicyId = premiumReceivableSearchPolicy.PolicyId,
                        PolicyDocumentNumber = premiumReceivableSearchPolicy.PolicyDocumentNumber,
                        EndorsementId = premiumReceivableSearchPolicy.EndorsementId,
                        EndorsementDocumentNumber = premiumReceivableSearchPolicy.EndorsementDocumentNumber,
                        BranchDescription = premiumReceivableSearchPolicy.BranchDescription,
                        PrefixDescription = premiumReceivableSearchPolicy.PrefixDescription,
                        InsuredName = premiumReceivableSearchPolicy.InsuredName,
                        PayerId = premiumReceivableSearchPolicy.PayerId,
                        PayerName = premiumReceivableSearchPolicy.PayerName,
                        PayableAmount = premiumReceivableSearchPolicy.PaidAmount,
                        PendingCommission = premiumReceivableSearchPolicy.PendingCommission,
                        PolicyAgentDocumentNumber = premiumReceivableSearchPolicy.PolicyAgentDocumentNumber,
                        PolicyAgentName = premiumReceivableSearchPolicy.PolicyAgentName,
                        CurrencyId = premiumReceivableSearchPolicy.CurrencyId,
                        ExchangeRate = premiumReceivableSearchPolicy.ExchangeRate,
                        Observations = premiumReceivableSearchPolicy.Observations
                    });
                }

                return Json(premiumReceivablePolicies, JsonRequestBehavior.AllowGet);
            }
            catch (TimeoutException)
            {
                List<object> premiumReceivablePolicies = new List<object>();

                premiumReceivablePolicies.Add(new
                {
                    ErrorTimeOut = Global.InternalErrorOccurredInTheDatabase,
                });

                return Json(premiumReceivablePolicies, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {

                List<object> premiumReceivablePolicies = new List<object>();

                premiumReceivablePolicies.Add(new
                {
                    ErrorTimeOut = Global.InternalErrorOccurredInTheDatabase,
                });

                return Json(premiumReceivablePolicies, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}