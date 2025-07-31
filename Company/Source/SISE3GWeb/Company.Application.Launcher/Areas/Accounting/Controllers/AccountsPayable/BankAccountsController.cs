//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;
//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class BankAccountsController : Controller
    {
        #region Class

        /// <summary>
        /// State
        /// </summary>
        public class State
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// CheckType
        /// </summary>
        public class CheckType
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        #endregion Class

        #region Instance Variables
        readonly CommonController _commonController = new CommonController();

        #endregion Instance Variables       

        #region Public Methods

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// MainCompanyBankAccount
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCompanyBankAccount()
        {
            try
            {
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);
                return View("~/Areas/Accounting/Views/Parameters/CompanyBankAccount/MainCompanyBankAccount.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }  
        }

        /// <summary>
        /// SaveBankAccount
        /// </summary>
        /// <param name="companyBankAccountModel"></param>
        /// <returns>int</returns>
        public int SaveCompanyBankAccount(CompanyBankAccountModel companyBankAccountModel)
        {
            if (ValidateAccountNumber(companyBankAccountModel.AccountNumber, companyBankAccountModel.AccountTypeId,
                companyBankAccountModel.BranchId, companyBankAccountModel.BankId,companyBankAccountModel.AccountingAccountId) == 0)
            {
                BankAccountTypeDTO bankAccountType = new BankAccountTypeDTO()
                {
                    Id = companyBankAccountModel.AccountTypeId,
                    Description = companyBankAccountModel.Description
                };

                SCRDTO.AccountingAccountDTO accountingAccount = new SCRDTO.AccountingAccountDTO()
                {
                    AccountingAccountId=companyBankAccountModel.AccountingAccountId,
                };

                BankDTO bank = new BankDTO() { Id = companyBankAccountModel.BankId };

                SCRDTO.BranchDTO branch = new SCRDTO.BranchDTO() { Id = companyBankAccountModel.BranchId };

                SCRDTO.CurrencyDTO currency = new SCRDTO.CurrencyDTO();
                currency.Id = companyBankAccountModel.CurrencyId;

                DateTime? disabledDate = companyBankAccountModel.DisabledDate == null ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(companyBankAccountModel.DisabledDate);

                BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
                bankAccountCompany.Id = 0;
                bankAccountCompany.BankAccountType = bankAccountType;
                bankAccountCompany.Number = companyBankAccountModel.AccountNumber;
                bankAccountCompany.Bank = bank;
                bankAccountCompany.IsEnabled = Convert.ToBoolean(companyBankAccountModel.Enabled);
                bankAccountCompany.IsDefault = Convert.ToBoolean(companyBankAccountModel.Default);
                bankAccountCompany.Currency = currency;
                bankAccountCompany.DisableDate = Convert.ToDateTime(disabledDate);
                bankAccountCompany.Branch = branch;
                bankAccountCompany.AccountingAccount = accountingAccount;
                return DelegateService.accountingParameterService.SaveBankAccountCompany(bankAccountCompany).Id;
            }

            return -1;
        }

        /// <summary>
        /// UpdateCompanyBankAccount
        /// </summary>
        /// <param name="companyBankAccountId"></param>
        /// <param name="description"></param>
        /// <param name="disabledDate"></param>
        /// <param name="enabled"></param>
        /// <returns>int</returns>
        public int UpdateCompanyBankAccount(int companyBankAccountId, string description, string disabledDate, int enabled)
        {
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
            bankAccountCompany.Id = companyBankAccountId;
            bankAccountCompany.IsEnabled = Convert.ToBoolean(enabled);
            bankAccountCompany.Number = "-1";

            if (disabledDate == "")
            {
                disabledDate = null;
            }

            bankAccountCompany.DisableDate = Convert.ToDateTime(disabledDate);

            return DelegateService.accountingParameterService.UpdateBankAccountCompany(bankAccountCompany).Id;
        }

        /// <summary>
        /// GetCompanyBankAccountByBranchIdBankId
        /// Obtiene las cuentas bancarias por sucursal y banco
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCompanyBankAccountByBranchIdBankId(int branchId, int bankId)
        {
            List<object> bankAccounts = new List<object>();
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.Branch.Id.Equals(branchId))).ToList();

            foreach (BankAccountCompanyDTO companyBankAccount in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    CompanyBankAccountId = companyBankAccount.Id,
                    AccountTypeDescription = companyBankAccount.BankAccountType.Description,
                    AccountNumber = companyBankAccount.Number,
                    CurrencyDescription = companyBankAccount.Currency.Description,
                    CurrencyId = companyBankAccount.Currency.Id,
                    EnabledDescription = companyBankAccount.IsEnabled ? @Global.Enabled : @Global.Disabled,
                    Enabled = companyBankAccount.IsEnabled,
                    BankAccountTypeId = companyBankAccount.BankAccountType.Id,
                    DisableDate = companyBankAccount.DisableDate == Convert.ToDateTime("01/01/1900") ? "" : companyBankAccount.DisableDate?.ToString("dd/MM/yyyy"),
                    AccountingAccountId = companyBankAccount.AccountingAccount.AccountingAccountId,
                    AccountingNumberBankAccount = companyBankAccount.AccountingAccount.Number,
                    AccountingNameBank = companyBankAccount.AccountingAccount.Description
                });
            }
           
            return Json(new { aaData = bankAccounts, total = bankAccounts.Count }, JsonRequestBehavior.AllowGet);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// ValidateAccountNumber
        /// Valida la existencia de una cuenta bancaria en base al tipo de cuenta, sucursal y banco
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="accountTypeId"></param>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <param name="accountingAccountId"></param>
        /// <returns>int</returns>
        private int ValidateAccountNumber(string accountNumber, int accountTypeId, int branchId, int bankId, int accountingAccountId)
        {
            try
            {
                var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies().Where(r => (r.Number.Equals(accountNumber) &&
                                                            r.BankAccountType.Id.Equals(accountTypeId) &&
                                                            r.Branch.Id.Equals(branchId) && r.Bank.Id.Equals(bankId) && r.AccountingAccount.AccountingAccountId.Equals(accountingAccountId)));

                return bankAccountCompanies.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion Private Methods

    }
}