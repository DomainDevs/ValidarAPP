using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Framework.UIF.Web.Services;
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class EntryTypeController : BaseController
    {
        #region View

        /// <summary>
        /// EntryType
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult EntryType()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion View

        #region Actions

        /// <summary>
        /// GetEntriesTypes
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetEntryTypes()
        {
            return new UifTableResult(DelegateService.glAccountingApplicationService.GetEntryTypes());
        }

        /// <summary>
        /// GetAccountsByEntryType
        /// </summary>
        /// <param name="entryTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountsByEntryType(int entryTypeId)
        {
            var entryType = DelegateService.glAccountingApplicationService.GetEntryType(new EntryTypeDTO()
            {
                EntryTypeId = entryTypeId
            });

            var accounts = (from entryTypeAccounting in entryType.EntryTypeItems
                            select new
                            {
                                EntryTypeAccountingId = entryTypeAccounting.Id,
                                EntryTypeCd = entryType.EntryTypeId,
                                Description = entryTypeAccounting.Description,
                                AccountingAccountId = entryTypeAccounting.AccountingAccount.AccountingAccountId,
                                AccountingAccountNumber = entryTypeAccounting.AccountingAccount.Number,
                                AccountingAccountDescription = entryTypeAccounting.AccountingAccount.Description,
                                AccountingMovementTypeId = entryTypeAccounting.AccountingMovementType.AccountingMovementTypeId,
                                AccountingMovementTypeDescription = Convert.ToInt32(entryTypeAccounting.AccountingMovementType.AccountingMovementTypeId) == 0 ? "" : DelegateService.glAccountingApplicationService.GetAccountingMovementType(new AccountingMovementTypeDTO() { AccountingMovementTypeId = Convert.ToInt32(entryTypeAccounting.AccountingMovementType.AccountingMovementTypeId) }).Description,
                                CurrencyId = entryTypeAccounting.Currency.Id,
                                CurrencyDescription = GetCurrencyDescriptionById(entryTypeAccounting.Currency.Id),
                                AccountingNatureId = Convert.ToInt32(entryTypeAccounting.AccountingNature),
                                AccountingNatureDescription = Convert.ToInt32(entryTypeAccounting.AccountingNature) == 1 ? Global.Credits : @Global.Debits,
                                AnalysisId = Convert.ToInt32(entryTypeAccounting.Analysis.AnalysisId),
                                AnalysisDescription = Convert.ToInt32(entryTypeAccounting.Analysis.AnalysisId) == 0 ? "" : DelegateService.glAccountingApplicationService.GetAnalysisCode(Convert.ToInt32(entryTypeAccounting.Analysis.AnalysisId)).Description,
                                CostCenterId = entryTypeAccounting.CostCenter.CostCenterId,
                                CostCenterDescription = Convert.ToInt32(entryTypeAccounting.CostCenter.CostCenterId) == 0 ? "" : DelegateService.glAccountingApplicationService.GetCostCenter(new CostCenterDTO() { CostCenterId = Convert.ToInt32(entryTypeAccounting.CostCenter.CostCenterId) }).Description,
                                PaymentConceptCd = entryTypeAccounting.AccountingConcept.Id,
                                PaymentConceptDescription = DelegateService.glAccountingApplicationService.GetAccountingConcept(new AccountingConceptDTO()
                                {
                                    Id = entryTypeAccounting.AccountingConcept.Id
                                }).Description
                            });

            return new UifTableResult(accounts);
        }

        /// <summary>
        /// EntryTypeModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult EntryTypeModal(int? id)
        {
            var entryTypeModel = new EntryTypeModel();

            if (id.HasValue)
            {
                var entryType = DelegateService.glAccountingApplicationService.GetEntryType(new EntryTypeDTO()
                {
                    EntryTypeId = id.Value
                });

                entryTypeModel.EntryTypeId = entryType.EntryTypeId;
                entryTypeModel.EntryTypeDescription = entryType.Description;
                entryTypeModel.EntryTypeSmallDescription = entryType.SmallDescription;
            }

            return View(entryTypeModel);
        }

        /// <summary>
        /// SaveEntryType
        /// </summary>
        /// <param name="entryTypeModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        [ValidateAntiForgeryTokenPost]
        public JsonResult SaveEntryType(EntryTypeModel entryTypeModel)
        {
            var entryType = new EntryTypeDTO()
            {
                EntryTypeId = entryTypeModel.EntryTypeId,
                Description = entryTypeModel.EntryTypeDescription,
                SmallDescription = entryTypeModel.EntryTypeSmallDescription,
                EntryTypeItems = null
            };

            if (entryType.EntryTypeId == 0)
            {
                DelegateService.glAccountingApplicationService.SaveEntryType(entryType);
                return Json(entryTypeModel.EntryTypeId, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DelegateService.glAccountingApplicationService.UpdateEntryType(entryType);
                return Json(entryTypeModel.EntryTypeId, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteEntryType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteEntryType(int id)
        {
            bool isDeleted = false;

            try
            {
                DelegateService.glAccountingApplicationService.DeleteEntryTypeRequest(id);
                isDeleted = true;
            }
            catch (Exception)
            {
                isDeleted = false;
            }

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccounts
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccounts(string query)
        {
            try
            {
                var accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountsByNumberDescription(new AccountingAccountDTO()
                {
                    Number = query
                });

                var jsonData = (from accountingAccount in accountingAccounts
                                select new
                                {
                                    AccountingAccountId = accountingAccount.AccountingAccountId,
                                    AccountingAccountNumber = accountingAccount.Number,
                                    CurrencyId = accountingAccount.Currency.Id,
                                    AccountingNatureId = Convert.ToInt32(accountingAccount.AccountingNature),
                                    AccountingAccountNumberName = accountingAccount.Number + " - " + accountingAccount.Description
                                }).ToList();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var AccountingAccountNumberName = ex.Message;
                return Json(AccountingAccountNumberName, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// AccountingModal
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entryTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingModal(int? id, int entryTypeId)
        {
            var entryType = DelegateService.glAccountingApplicationService.GetEntryType(new EntryTypeDTO()
            {
                EntryTypeId = entryTypeId
            });

            var entryTypeAccountingModel = new EntryTypeAccountingModel()
            {
                EntryTypeCd = entryTypeId,
                CurrencyId = -1
            };

            if (id.HasValue)
            {
                var entryTypeItem = entryType.EntryTypeItems.Find(r => r.Id == id);
                entryTypeAccountingModel.EntryTypeAccountingId = entryTypeItem.Id;
                entryTypeAccountingModel.Description = entryTypeItem.Description;
                entryTypeAccountingModel.AccountingAccountId = entryTypeItem.AccountingAccount.AccountingAccountId;
                AccountingAccountDTO accountingAccount = entryTypeItem.AccountingAccount.AccountingAccountId == 0 ? new AccountingAccountDTO() : DelegateService.glAccountingApplicationService.GetAccountingAccount(entryTypeItem.AccountingAccount.AccountingAccountId);
                entryTypeAccountingModel.AccountingAccountNumber = entryTypeItem.AccountingAccount.AccountingAccountId == 0 ? "" : accountingAccount.Number;
                entryTypeAccountingModel.AccountingAccountDescription = entryTypeItem.AccountingAccount.AccountingAccountId == 0 ? "" : accountingAccount.Number + " - " + accountingAccount.Description;
                entryTypeAccountingModel.AccountingMovementTypeId = entryTypeItem.AccountingMovementType.AccountingMovementTypeId;
                entryTypeAccountingModel.CurrencyId = entryTypeItem.Currency.Id;
                entryTypeAccountingModel.AccountingNatureId = Convert.ToInt32(entryTypeItem.AccountingNature);
                entryTypeAccountingModel.AnalysisId = Convert.ToInt32(entryTypeItem.Analysis.AnalysisId);
                entryTypeAccountingModel.CostCenterId = entryTypeItem.CostCenter.CostCenterId;
                entryTypeAccountingModel.PaymentConceptCd = entryTypeItem.AccountingConcept.Id;
            }

            return View(entryTypeAccountingModel);
        }

        /// <summary>
        /// SaveEntryTypeAccounting
        /// </summary>
        /// <param name="entryTypeAccountingModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveEntryTypeAccounting(EntryTypeAccountingModel entryTypeAccountingModel)
        {
            int saved = 0;
            EntryTypeDTO entryType = new EntryTypeDTO();
            entryType.EntryTypeId = entryTypeAccountingModel.EntryTypeCd;
            List<EntryTypeItemDTO> entryTypeItems = new List<EntryTypeItemDTO>();
            entryTypeItems.Add(new EntryTypeItemDTO()
            {
                AccountingAccount = new AccountingAccountDTO() { AccountingAccountId = entryTypeAccountingModel.AccountingAccountId },
                AccountingConcept = new AccountingConceptDTO() { Id = entryTypeAccountingModel.PaymentConceptCd },
                AccountingMovementType = new AccountingMovementTypeDTO() { AccountingMovementTypeId = entryTypeAccountingModel.AccountingMovementTypeId },
                AccountingNature = entryTypeAccountingModel.AccountingNatureId,
                Analysis = new AnalysisDTO() { AnalysisId = entryTypeAccountingModel.AnalysisId },
                CostCenter = new CostCenterDTO() { CostCenterId = entryTypeAccountingModel.CostCenterId },
                Currency = new CurrencyDTO() { Id = entryTypeAccountingModel.CurrencyId },
                Description = entryTypeAccountingModel.Description,
                Id = entryTypeAccountingModel.EntryTypeAccountingId
            });

            entryType.EntryTypeItems = entryTypeItems;

            if (entryTypeItems[0].Id == 0)
            {
                entryType = DelegateService.glAccountingApplicationService.SaveEntryType(entryType);
            }
            else
            {
                DelegateService.glAccountingApplicationService.UpdateEntryType(entryType);
            }
            saved = entryType.EntryTypeId;

            return Json(saved, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteEntryTypeAccount
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteEntryTypeAccount(int id)
        {
            try
            {
                return Json(DelegateService.glAccountingApplicationService.DeleteEntryTypeAccounting(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Actions

    }
}