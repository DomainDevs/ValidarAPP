//System
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;

// Sistran FWK
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Resources;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using AccountingDTO = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class TemporarySearchController : Controller
    {
        #region Constants

        public const string SortOrder = "ASC";

        #endregion

        #region Instance Variables

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainTemporarySearch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainTemporarySearch()
        {
            try
            {

                ViewBag.ExchangeRate = 1.00000;

                // Tipo de Beneficiario
                ViewBag.SupplierCode = Convert.ToInt32(ConfigurationManager.AppSettings["SupplierCode"]); // 10
                ViewBag.InsuredCode = Convert.ToInt32(ConfigurationManager.AppSettings["InsuredCode"]); //7
                ViewBag.CoinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"]); //2
                ViewBag.ThirdPartyCode = Convert.ToInt32(ConfigurationManager.AppSettings["ThirdPartyCode"]);//8
                ViewBag.AgentCode = Convert.ToInt32(ConfigurationManager.AppSettings["AgentCode"]); //1
                ViewBag.ProducerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ProducerCode"]); //1
                ViewBag.EmployeeCode = Convert.ToInt32(ConfigurationManager.AppSettings["EmployeeCode"]);//11
                ViewBag.ReinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ReinsurerCode"]); //2
                ViewBag.TradeConsultant = Convert.ToInt32(ConfigurationManager.AppSettings["TradeConsultant"]); //8
                ViewBag.ContractorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ContractorCode"]); //13

                // Carga tipos de imputaciones de aplicación
                ViewBag.AplicationPaymentOrder = ConfigurationManager.AppSettings["AplicationPaymentOrder"];
                ViewBag.AplicationBill = ConfigurationManager.AppSettings["AplicationBill"];
                ViewBag.AplicationPreLiquidation = ConfigurationManager.AppSettings["AplicationPreLiquidation"];
                ViewBag.AplicationJournalEntry = ConfigurationManager.AppSettings["AplicationJournalEntry"];

                // Setear valor por defaul de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                // Trae la data de la preliquidación temporal
                ViewBag.TempPreliquidationBranchId = TempData["TempPreliquidationBranchId"];
                ViewBag.TempImputationId = TempData["TempImputationId"];

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region TemporarySearch

        /// <summary>
        /// SearchTemporaryItems
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="imputationId"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchTemporaryItems(string branchId, string imputationId, string userId,
                                               string imputationTypeId, string startDate, string endDate)
        {
            List<object> temporaryItemSearchsResponses = new List<object>();

            try
            {

                if (String.IsNullOrEmpty(branchId))
                {
                    branchId = "-1";
                }
                if (String.IsNullOrEmpty(imputationId))
                {
                    imputationId = "-1";
                }
                if (String.IsNullOrEmpty(userId))
                {
                    userId = "-1";
                }
                if (String.IsNullOrEmpty(imputationTypeId))
                {
                    imputationTypeId = "-1";
                }
                if (String.IsNullOrEmpty(startDate))
                {
                    startDate = "*";
                }
                if (String.IsNullOrEmpty(endDate))
                {
                    endDate = "*";
                }

                if (endDate != null && endDate != "*")
                {
                    endDate = endDate + " 23:59:59";
                }

                AccountingDTO.Search.SearchParameterTemporaryDTO searchParameter = new AccountingDTO.Search.SearchParameterTemporaryDTO()
                {
                    Branch = new AccountingDTO.Search.BranchDTO() { Id = Convert.ToInt32(branchId) },
                    EndDate = endDate,
                    ImputationType = new ApplicationTypeDTO() { Id = Convert.ToInt32(imputationTypeId) },
                    StartDate = startDate,
                    TemporaryNumber = Convert.ToInt32(imputationId),
                    UserId = Convert.ToInt32(userId)
                };

                List<AccountingDTO.Search.TemporaryItemSearchDTO> temporaryItemSearchs = DelegateService.accountingApplicationService.TemporarySearch(searchParameter);

                List<AccountingDTO.Search.TemporaryItemSearchDTO> distributionErrorsOrder = (from x in temporaryItemSearchs orderby x.TemporaryNumber descending, x.BranchCode select x).ToList();

                foreach (AccountingDTO.Search.TemporaryItemSearchDTO temporaryItemSearch in distributionErrorsOrder)
                {
                    temporaryItemSearchsResponses.Add(new
                    {
                        SourceCode = temporaryItemSearch.SourceCode,
                        TemporaryNumber = temporaryItemSearch.TemporaryNumber,
                        BranchCode = temporaryItemSearch.BranchCode,
                        BranchName = temporaryItemSearch.BranchName,
                        UserCode = temporaryItemSearch.UserCode,
                        UserName = temporaryItemSearch.UserName,
                        RegisterDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(temporaryItemSearch.RegisterDate)),
                        ImputationTypeCode = temporaryItemSearch.ImputationTypeCode,
                        ImputationTypeName = temporaryItemSearch.ImputationTypeName
                    });
                }

                return Json(new { aaData = temporaryItemSearchsResponses, total = temporaryItemSearchsResponses.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {

                temporaryItemSearchsResponses.Add(new
                {
                    SourceCode = "",
                    TemporaryNumber = Global.UnhandledExceptionMsj,
                    BranchCode = "",
                    BranchName = "",
                    UserCode = "",
                    UserName = "",
                    RegisterDate = "",
                    ImputationTypeCode = "",
                    ImputationTypeName = ""
                });

                return Json(new { aaData = temporaryItemSearchsResponses, total = temporaryItemSearchsResponses.Count }, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// LoadTempPreliquidationSearch
        /// </summary>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadTempPreliquidationSearch(int branchId, int tempImputationId)
        {
            TempData["TempPreliquidationBranchId"] = branchId;
            TempData["TempImputationId"] = tempImputationId;

            return RedirectToAction("MainTemporarySearch");
        }

        #endregion

        #region ApplicationType

        /// <summary>
        /// LoadApplicationType
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult LoadApplicationType()
        {
            List<AccountingDTO.Imputations.ApplicationTypeDTO> applicationTypes = DelegateService.accountingApplicationService.GetApplicationTypes();

            return new UifSelectResult(applicationTypes);
        }

        /// <summary>
        /// GetPosByBranchId
        /// Obtiene los puntos de venta en base a la sucursal y usuario
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="defValue"></param>
        /// <param name="defDescription"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPosByBranchId(int branchId, int defValue, string defDescription)
        {
            return Json(_commonController.GetSalesPointByBranchId(branchId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadTemporarySearch
        /// </summary>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadTemporarySearch()
        {
            return RedirectToAction("MainTemporarySearch");
        }

        #endregion

        #region Temporary


        /// <summary>
        /// DeleteTemporaryApplication
        /// </summary>
        /// <param name="temporals"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTemporaryApplication(Models.Imputation.ItemsToDeleteModel temporals)
        {
            try
            {

                bool isDeletedTemporaryApplication = false;

                foreach (Models.Imputation.TemporalModel temporal in temporals.Temporals)
                {
                    isDeletedTemporaryApplication = DelegateService.accountingApplicationService.DeleteTemporaryApplicationRequest(temporal.TempImputationId, temporal.ImputationTypeId, temporal.SourceId);
                }

                return Json(isDeletedTemporaryApplication, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountRequest
        /// Permite actualizar la tasa de cambio a la fecha actual a una imputación temporal
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult RecalculatingForeignCurrencyAmountRequest(int tempImputationId, int imputationTypeId, int sourceId)
        {
            List<AccountingDTO.Search.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates = _commonController.GetForeignCurrencyExchangeRate();

            bool isRecalculatingForeignCurrencyAmount = DelegateService.accountingApplicationService.RecalculatingForeignCurrencyAmount(tempImputationId, imputationTypeId, sourceId, foreignCurrencyExchangeRates);

            return Json(isRecalculatingForeignCurrencyAmount, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}