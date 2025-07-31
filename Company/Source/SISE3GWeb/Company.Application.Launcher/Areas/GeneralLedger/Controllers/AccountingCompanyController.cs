using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class AccountingCompanyController : Controller
    {
        #region View

        /// <summary>
        /// AccountingCompany
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingCompany()
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

        /// <summary>
        /// AccountingCompanyModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingCompanyModal(int? id)
        {
            var accountingCompanyModel = new AccountingCompanyModel();

            if (id.HasValue)
            {
                var accountingCompany = new AccountingCompanyDTO()
                {
                    AccountingCompanyId = id.Value
                };
                accountingCompany = DelegateService.glAccountingApplicationService.GetAccountingCompany(accountingCompany);

                accountingCompanyModel.AccountingCompanyId = accountingCompany.AccountingCompanyId;
                accountingCompanyModel.Description = accountingCompany.Description;
                accountingCompanyModel.Default = accountingCompany.Default;
            }

            return View(accountingCompanyModel);
        }

        #endregion View

        #region Methods

        /// <summary>
        /// GetAccountingCompanies
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingCompanies()
        {
            var accountingCompanies = DelegateService.glAccountingApplicationService.GetAccountingCompanies();

            var jsonData = from accountingCompany in accountingCompanies
                           select new
                           {
                               AccountingCompanyId = accountingCompany.AccountingCompanyId,
                               Description = accountingCompany.Description,
                               Default = accountingCompany.Default,
                               DefaultDescription = (accountingCompany.Default) ? Global.Yes : Global.No
                           };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAccountingCompany
        /// </summary>
        /// <param name="accountingCompanyModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveAccountingCompany(AccountingCompanyModel accountingCompanyModel)
        {
            var accountingCompany = new AccountingCompanyDTO()
            {
                AccountingCompanyId = accountingCompanyModel.AccountingCompanyId,
                Description = accountingCompanyModel.Description,
                Default = Convert.ToBoolean(accountingCompanyModel.Default)
            };

            if (DelegateService.glAccountingApplicationService.GetAccountingCompanies().Any(r => r.Default) && accountingCompanyModel.Default)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (accountingCompany.AccountingCompanyId == 0)
            {
                var result = DelegateService.glAccountingApplicationService.SaveAccountingCompany(accountingCompany);
                return Json(result.AccountingCompanyId, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = DelegateService.glAccountingApplicationService.UpdateAccountingCompany(accountingCompany);
                return Json(result.AccountingCompanyId, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteAccountingCompany
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteAccountingCompany(int id)
        {
            try
            {
                if (DelegateService.glAccountingApplicationService.VerifyCompanyUsed(id))
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                else if (DelegateService.glAccountingApplicationService.GetAccountingCompany(new AccountingCompanyDTO()
                {
                    AccountingCompanyId = id
                }).Default)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DelegateService.glAccountingApplicationService.DeleteAccountingCompany(id);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Methods

    }
}