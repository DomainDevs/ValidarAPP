/*
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniqueUserServices;

//Sistran Core
//using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.GeneralLedgerServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class JournalEntryController : Controller
    {
        #region Instance Variables

        readonly ICommonService DelegateService.commonService = ServiceManager.Instance.GetService<ICommonService>();
        readonly IAccountingService DelegateService.glAccountingApplicationService = ServiceManager.Instance.GetService<IAccountingService>();
        readonly IImputationService _imputationService = ServiceManager.Instance.GetService<IImputationService>();
        readonly IUniqueUserService DelegateService.uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();

        readonly BaseController _baseController = new BaseController();

        #endregion Instance Variables

        #region View

        /// <summary>
        /// JournalEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult JournalEntry()
        {
            try
            {

                ViewBag.BranchDefault = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 0);
                ViewBag.BranchDisabled = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 1);
                ViewBag.SalePointBranchUserDefault = _imputationService.GetSalePointDefaultByUserIdAndBranchId(_baseController.SessionHelper.GetUserId(), ViewBag.BranchDefault);
                ViewBag.ViewBagModuleId = Convert.ToInt32(ConfigurationManager.AppSettings["EntryModule"]);

                //Obtiene si es el uso del tercero en  1 TRUE 0 FALSE
                ViewBag.ThirdAccountingUsed = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ThirdAccountingUsed"])).NumberParameter;


                //OBTIENE SI ESTA CONFIGURADO COMO MULTICOMPANIA 1 TRUE 0 FALSE
                ViewBag.ParameterMulticompany = _baseController.GetParameterMulticompany();
                //Se obtiene la compañía contable por defecto
                ViewBag.DefaultAccountingCompany = _baseController.GetDefaultAccountingCompany();

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// JournalEntrySearch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult JournalEntrySearch()
        {

            try
            {
                ViewBag.Active = Convert.ToInt32(AccountingEntryStatus.Active);
                ViewBag.Reverted = Convert.ToInt32(AccountingEntryStatus.Reverted);

                ViewBag.BranchDefault = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 0);
                ViewBag.BranchDisabled = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion View

        #region Public Methods

        /// <summary>
        /// SaveDailyEntryRequest
        /// </summary>
        /// <param name="journalEntryModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveJournalEntry(JournalEntryModel journalEntryModel)
        {
            try
            {
                
                JournalEntry journalEntry = new JournalEntry();

                journalEntry.Id = 0; //autonumérico
                journalEntry.AccountingCompany = new AccountingCompany();
                journalEntry.AccountingCompany.AccountingCompanyId = journalEntryModel.AccountingCompanyId;
                journalEntry.ModuleDateId = Convert.ToInt32(ConfigurationManager.AppSettings["EntryModule"]);
                journalEntry.Branch = new Branch();
                journalEntry.Branch.Id = journalEntryModel.BranchId;
                journalEntry.SalePoint = new SalePoint();
                journalEntry.SalePoint.Id = journalEntryModel.SalePointId;
                journalEntry.TransactionNumber = 0; //se genera el momento de la integración
                journalEntry.EntryNumber = 0; // Se obtiene en la grabación del asiento
                journalEntry.Description = journalEntryModel.Description;
                journalEntry.AccountingDate = Convert.ToDateTime(journalEntryModel.Date);
                journalEntry.RegisterDate = DateTime.Now;
                journalEntry.Status = 1;
                journalEntry.UserId = DelegateService.uniqueUserService.GetUserByName(User.Identity.Name)[0].UserId;
                journalEntry.JournalEntryItems = new List<JournalEntryItem>();

                if (journalEntryModel.JournalEntryItems != null)
                {
                    foreach (JournalEntryItemModel journalEntryItemModel in journalEntryModel.JournalEntryItems)
                    {
                        JournalEntryItem journalEntryItem = new JournalEntryItem();
                        journalEntryItem.Id = 0; //autonumérico
                        journalEntryItem.Currency = new Currency() { Id = journalEntryItemModel.CurrencyId };
                        journalEntryItem.AccountingAccount = new AccountingAccount() { AccountingAccountId = journalEntryItemModel.AccountingAccountId };
                        journalEntryItem.ReconciliationMovementType = new ReconciliationMovementType() { Id = journalEntryItemModel.BankReconciliationId };
                        journalEntryItem.Receipt = new Receipt();
                        journalEntryItem.Receipt.Number = journalEntryItemModel.ReceiptNumber;
                        journalEntryItem.Receipt.Date = Convert.ToDateTime(journalEntryItemModel.ReceiptDate);
                        journalEntryItem.AccountingNature = (AccountingNatures)journalEntryItemModel.AccountingNatureId;
                        journalEntryItem.Description = journalEntryItemModel.Description;
                        journalEntryItem.Amount = new Amount();
                        journalEntryItem.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, journalEntryItemModel.CurrencyId); //new ExchangeRate() { SellAmount = Convert.ToDecimal(journalEntryItemModel.ExchangeRate) }; 
                        journalEntryItem.Amount.Value = Convert.ToDecimal(journalEntryItemModel.Amount);
                        journalEntryItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(journalEntryItemModel.Amount) * Convert.ToDecimal(journalEntryItem.ExchangeRate.SellAmount) };
                        journalEntryItem.Individual = new Individual() { IndividualId = journalEntryItemModel.IndividualId };
                        journalEntryItem.CostCenters = new List<CostCenter>();
                        journalEntryItem.Analysis = new List<Analysis>();
                        journalEntryItem.PostDated = new List<PostDated>();

                        //se llena centro de costos
                        if (journalEntryItemModel.CostCenters != null)
                        {
                            foreach (CostCenterEntryModel costCenterModel in journalEntryItemModel.CostCenters)
                            {
                                CostCenter costCenter = new CostCenter();
                                costCenter.CostCenterId = costCenterModel.CostCenterId;
                                costCenter.CostCenterType = new CostCenterType();
                                costCenter.PercentageAmount = costCenterModel.PercentageAmount;

                                journalEntryItem.CostCenters.Add(costCenter);
                            }
                        }

                        //se llena analisis
                        if (journalEntryItemModel.Analysis != null)
                        {
                            foreach (AnalysisModel analysisModel in journalEntryItemModel.Analysis)
                            {
                                Analysis analysis = new Analysis();
                                analysis.AnalysisConcept = new AnalysisConcept();
                                analysis.AnalysisConcept.AnalysisCode = new AnalysisCode();
                                analysis.AnalysisConcept.AnalysisCode.AnalysisCodeId = analysisModel.AnalysisId;
                                analysis.AnalysisConcept.AnalysisConceptId = analysisModel.AnalysisConceptId;
                                analysis.Key = analysisModel.Key;
                                analysis.Description = analysisModel.Description;

                                journalEntryItem.Analysis.Add(analysis);
                            }
                        }

                        //se llena postfechados
                        if (journalEntryItemModel.Postdated != null)
                        {
                            foreach (PostDatedModel postDatedModel in journalEntryItemModel.Postdated)
                            {
                                PostDated postDated = new PostDated();
                                postDated.PostDatedId = 0; //autonumérico
                                postDated.PostDateType = (PostDateTypes)postDatedModel.PostDateTypeId;
                                postDated.Amount = new Amount();
                                postDated.Amount.Value = postDatedModel.IssueAmount;
                                postDated.Amount.Currency = new Currency() { Id = postDatedModel.CurrencyId };
                                postDated.ExchangeRate = new ExchangeRate() { SellAmount = postDatedModel.ExchangeRate };
                                postDated.LocalAmount = new Amount() { Value = postDatedModel.LocalAmount };
                                postDated.DocumentNumber = postDatedModel.DocumentNumber;

                                journalEntryItem.PostDated.Add(postDated);
                            }
                        }

                        journalEntry.JournalEntryItems.Add(journalEntryItem);
                    }
                }

                int saved = 0;
                saved = DelegateService.glAccountingApplicationService.SaveJournalEntry(journalEntry);

                return new UifJsonResult(true, saved);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SearchDailyEntryMovements
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="entryNumber"></param>
        /// <returns>ActionResult</returns>
        public JsonResult SearchDailyEntryMovements(int branchId, int year, int month, int entryNumber)
        {
            // Se arma las fechas para consulta
            string dateFrom = "01" + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);

            int numberOfDays = DateTime.DaysInMonth(year, month);
            string dateTo = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
            dateTo = dateTo + " 23:59:59";

            List<EntryConsultationDTO> entries = DelegateService.glAccountingApplicationService.SearchDailyEntryMovements(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), branchId, 0, 0);

            foreach (var entryConsultationDto in entries)
            {
                entryConsultationDto.AccountingNatureDescription = entryConsultationDto.AccountingNature == Convert.ToInt32(AccountingNatures.Credit) ? Global.Credits : Global.Debits;
                entryConsultationDto.StatusDescription = entryConsultationDto.Status == AccountingEntryStatus.Active ? Global.Active : Global.Reverted;
                entryConsultationDto.CurrencyDescription = GetCurrencyDescriptionByCurrencyId(entryConsultationDto.CurrencyId);
            }

            return Json(entries, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReverseJournalEntry
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns></returns>
        public ActionResult ReverseJournalEntry(int journalEntryId)
        {
            try
            {
                int reversedEntryId = 0;
                int userId = DelegateService.uniqueUserService.GetUserByName(User.Identity.Name)[0].UserId;

                reversedEntryId = ReverseJournalEntryRequest(journalEntryId, userId);

                return new UifJsonResult(true, reversedEntryId);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ReverseJournalEntryRequest
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ReverseJournalEntryRequest(int journalEntryId, int userId)
        {
            try
            {
                int reversedEntryId = 0;

                JournalEntry journalEntry = new JournalEntry();
                journalEntry.Id = journalEntryId;
                journalEntry.EntryNumber = 0;// Se obtiene en la grabación del Asiento
                journalEntry.UserId = userId;
                //se genera una nueva fecha contable para ser usada en el asiento de reversa
                //journalEntry.AccountingDate = _parameterService.GetAccountingDate(Convert.ToInt32(ConfigurationManager.AppSettings["EntryModule"]));descomentar hasta que añada la referencia 
                journalEntry.AccountingDate = DateTime.Now;

                reversedEntryId = DelegateService.glAccountingApplicationService.ReverseJournalEntry(journalEntry);

                return reversedEntryId; 
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// GetCurrencyDescriptionByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>string</returns>
        private string GetCurrencyDescriptionByCurrencyId(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currencyNames = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currencyNames[0].Description;
        }

        #endregion Private Methods

    }
}

*/