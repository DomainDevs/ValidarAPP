using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Controls.UifTreeView;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Framework.UIF.Web.Services;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class AccountingAccountController : Controller
    {
        #region Interface
        readonly BaseController _baseController = new BaseController();

        #endregion Interface

        #region Views

        /// <summary>
        /// AccountingAccount
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingAccount()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.CompleteWithCeros = Convert.ToInt32(ConfigurationManager.AppSettings["CompleteAccountWithCeros"]); //indica si la cuenta contable se lo ingresa con 0 a la derecha
                ViewBag.AccoutingAccountLength = Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]); //tamaño de la cuenta contable
                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetAccountingAccount
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccount(int accountingAccountId)
        {
            var accountingAccountModel = new AccountingAccountModel();

            var accountingAccount = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingAccountId);

            accountingAccountModel.AccountingAccountId = accountingAccount.AccountingAccountId;
            accountingAccountModel.AccountingAccountParentId = accountingAccount.AccountingAccountParentId;
            accountingAccountModel.AccountingAccountNumber = accountingAccount.Number;
            accountingAccountModel.AccountingAccountName = accountingAccount.Description;
            accountingAccountModel.BranchId = accountingAccount.Branch.Id;
            accountingAccountModel.AccountingNatureId = Convert.ToInt32(accountingAccount.AccountingNature);
            accountingAccountModel.CurrencyId = Convert.ToInt32(accountingAccount.Currency.Id);
            accountingAccountModel.RequireAnalysis = Convert.ToInt32(accountingAccount.RequiresAnalysis);
            accountingAccountModel.AnalysisId = accountingAccount.Analysis.AnalysisId;
            accountingAccountModel.RequireCostCenter = Convert.ToInt32(accountingAccount.RequiresCostCenter);
            accountingAccountModel.Comments = accountingAccount.Comments;
            accountingAccountModel.AccountingAccountApplication = (int)accountingAccount.AccountingAccountApplication;
            accountingAccountModel.AccountingAccountType = accountingAccount.AccountingAccountType.Id;
            accountingAccountModel.PrefixId = accountingAccount.Prefixes.Count > 0 ? accountingAccount.Prefixes[0].Id : 0;
            accountingAccountModel.CostCenters = new List<int>();
            accountingAccountModel.IsReclassify = accountingAccount.IsReclassify;
            accountingAccountModel.AccountClassify = accountingAccount.RecAccounting;
            accountingAccountModel.IsRevalue = accountingAccount.IsRevalue;
            accountingAccountModel.ReevaluePositive = accountingAccount.RevAcountingPos;
            accountingAccountModel.ReevalueNegative = accountingAccount.RevAcountingNeg;


            if (accountingAccount.CostCenters != null)
            {
                foreach (CostCenterDTO costCenter in accountingAccount.CostCenters)
                {
                    accountingAccountModel.CostCenters.Add(costCenter.CostCenterId);
                }
            }

            accountingAccountModel.AccountingAccountApplication = (int)accountingAccount.AccountingAccountApplication;


            return Json(accountingAccountModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DuplicateAccountingAccount
        /// Propagar Bloqueos de Cuentas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountBlockadeSpreading()
        {
            return View("~/Areas/GeneralLedger/Views/AccountingAccount/AccountBlockadeSpreading.cshtml");
        }

        #endregion Views

        #region Actions

        /// <summary>
        /// SaveAccountingAccount
        /// </summary>
        /// <param name="accountingAccountModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveAccountingAccount(AccountingAccountModel accountingAccountModel)
        {
            List<CostCenterDTO> costCenters = new List<CostCenterDTO>();
            List<PrefixDTO> prefixes = new List<PrefixDTO>();

            string prefixId = "00";
            string branchId = "00";

            // Se arma el sufijo de ramo para cuenta contable
            if (accountingAccountModel.PrefixId > 0)
            {
                prefixId = Convert.ToString(accountingAccountModel.PrefixId);
                if (prefixId.Length < 2)
                {
                    prefixId = "0" + prefixId;
                }
            }

            // Se arma el sufijo de sucursal para cuenta contable
            if (accountingAccountModel.BranchId > 0)
            {
                branchId = Convert.ToString(accountingAccountModel.BranchId);
                if (branchId.Length < 2)
                {
                    branchId = "0" + branchId;
                }
            }

            if (accountingAccountModel.CostCenters != null)
            {
                foreach (int cost in accountingAccountModel.CostCenters)
                {
                    costCenters.Add(new CostCenterDTO() { CostCenterId = cost });
                }
            }

            int accoutingAccountLength = Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]); //tamaño de la cuenta contable
            string accountNumber = accountingAccountModel.AccountingAccountNumber.Trim();
            if (accountNumber.Length > accoutingAccountLength)
            {
                accountNumber = accountNumber.Substring(0, accoutingAccountLength);
            }
            accountNumber = accountNumber.PadRight(accoutingAccountLength, '0');

            prefixes.Add(new PrefixDTO() { Id = accountingAccountModel.PrefixId });
            var accountingAccount = new AccountingAccountDTO()
            {
                AccountingAccountId = accountingAccountModel.AccountingAccountId,
                AccountingAccountParentId = accountingAccountModel.AccountingAccountParentId,
                AccountingNature = accountingAccountModel.AccountingNatureId,
                Branch = new BranchDTO()
                {
                    Id = accountingAccountModel.BranchId
                },
                Currency = new CurrencyDTO()
                {
                    Id = accountingAccountModel.CurrencyId
                },
                Analysis = new AnalysisDTO()
                {
                    AnalysisId = accountingAccountModel.AnalysisId
                },
                Number = accountNumber,
                Description = accountingAccountModel.AccountingAccountName,
                RequiresAnalysis = Convert.ToBoolean(accountingAccountModel.RequireAnalysis),
                RequiresCostCenter = Convert.ToBoolean(accountingAccountModel.RequireCostCenter),
                CostCenters = costCenters,
                Prefixes = prefixes,
                Comments = accountingAccountModel.Comments,
                AccountingAccountApplication = accountingAccountModel.AccountingAccountApplication,
                AccountingAccountType = new AccountingAccountTypeDTO() { Id = accountingAccountModel.AccountingAccountType },
                RecAccounting = accountingAccountModel.AccountClassify,
                RevAcountingPos = accountingAccountModel.ReevaluePositive,
                RevAcountingNeg = accountingAccountModel.ReevalueNegative,
                IsRevalue = (accountingAccountModel.ReevaluePositive != "" && accountingAccountModel.ReevaluePositive != null) && (accountingAccountModel.ReevalueNegative != "" && accountingAccountModel.ReevalueNegative != null) ? true : false,
                IsReclassify = accountingAccountModel.AccountClassify != "" && accountingAccountModel.AccountClassify != null ? true : false,
            };

            if (accountingAccount.AccountingAccountId == 0)
            {
                accountingAccount = DelegateService.glAccountingApplicationService.SaveAccountingAccount(accountingAccount);
            }
            else
            {
                accountingAccount = DelegateService.glAccountingApplicationService.UpdateAccountingAccount(accountingAccount);
            }

            return Json(accountingAccount, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAccountingAccount
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteAccountingAccount(int accountingAccountId)
        {
            try
            {
                return Json(DelegateService.glAccountingApplicationService.DeleteAccountingAccount(accountingAccountId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// LoadTree
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult LoadTree()
        {
            // Se carga los padres del árbol
            try
            {
                // Se obtiene las cuentas padres
                var parentAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountParents();

                var treeViewItems = (from accountingAccount in parentAccounts
                                     select new TreeViewItem()
                                     {
                                         Id = accountingAccount.AccountingAccountId.ToString(),
                                         Text = accountingAccount.AccountingAccountId + " - " + accountingAccount.Description
                                     }).ToList();

                return new UifTreeViewResult(treeViewItems);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        public ActionResult AdvancedConsultationSearch()
        {
            return this.View();
        }

        /// <summary>
        /// GetAccountingAccountsByParentId
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountsByParentId(int parentId)
        {
            var treeViewItems = new List<TreeViewItem>();

            try
            {
                // Select a los registros por parentId y ordenado por número de cuenta contable
                var accountsQuery = (DelegateService.glAccountingApplicationService.GetAccountingAccountsByParentId(parentId)).OrderBy(c => c.Number);

                // Se carga de datos en el modelo del árbol y llama a la recursividad para cargar las cuentas hijas. 
                if (accountsQuery.Any())
                {
                    treeViewItems = (from accountingAccount in accountsQuery
                                     select new TreeViewItem()
                                     {
                                         Id = accountingAccount.AccountingAccountId.ToString(),
                                         Text = accountingAccount.Number + " - "
                                         + accountingAccount.Description
                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return Json(treeViewItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateAccountingAccount
        /// </summary>
        /// <param name="accountingAccountModel"></param>
        /// <param name="edit"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult ValidateAccountingAccount(AccountingAccountModel accountingAccountModel, int edit)
        {
            try
            {
                var accountingAccount = new AccountingAccountDTO()
                {
                    AccountingAccountId = accountingAccountModel.AccountingAccountId,
                    AccountingAccountParentId = accountingAccountModel.AccountingAccountParentId,
                    AccountingNature = accountingAccountModel.AccountingNatureId,
                    Branch = new BranchDTO()
                    {
                        Id = accountingAccountModel.BranchId
                    },
                    Currency = new CurrencyDTO()
                    {
                        Id = accountingAccountModel.CurrencyId
                    },
                    Analysis = new AnalysisDTO()
                    {
                        AnalysisId = accountingAccountModel.AnalysisId
                    },

                    Number = accountingAccountModel.AccountingAccountNumber.Trim().Substring(0, 12),
                    Description = accountingAccountModel.AccountingAccountName,
                    RequiresAnalysis = Convert.ToBoolean(accountingAccountModel.RequireAnalysis),
                    RequiresCostCenter = Convert.ToBoolean(accountingAccountModel.RequireCostCenter),
                    Comments = accountingAccountModel.Comments,
                    IsReclassify = accountingAccountModel.IsReclassify,
                    RecAccounting = accountingAccountModel.AccountClassify,
                    IsRevalue = accountingAccountModel.IsRevalue,
                    RevAcountingPos = accountingAccountModel.ReevaluePositive,
                    RevAcountingNeg = accountingAccountModel.ReevalueNegative
                };

                return Json(DelegateService.glAccountingApplicationService.ValidateAccountingAccount(accountingAccount, edit), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// HasChildren
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult HasChildren(int accountingAccountId)
        {
            return Json(DelegateService.glAccountingApplicationService.HasChildren(accountingAccountId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// OnEntry
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult OnEntry(int accountingAccountId)
        {
            return Json(DelegateService.glAccountingApplicationService.OnEntry(accountingAccountId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// OnConcept
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult OnConcept(int accountingAccountId)
        {
            List<AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConcepts().Where(ac => ac.AccountingAccount.AccountingAccountId == accountingAccountId).ToList();

            if (accountingConcepts.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ReplicateAccount
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReplicateAccount(int accountingAccountId)
        {
            bool isSuccessfully = false;
            List<AccountingAccountDTO> accountingAccounts = new List<AccountingAccountDTO>();
            AccountingAccountDTO accountAccount = new AccountingAccountDTO();

            try
            {
                // Se obtiene la cuenta contable base.
                accountAccount = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingAccountId);

                // Se obtiene los ramos distintos al que tiene la cuenta base.
                List<Prefix> prefixes;

                if (accountAccount.Prefixes.Count > 0)
                {
                    prefixes = DelegateService.commonService.GetPrefixes().Select(p => p).Where(h => (h.Id != accountAccount.Prefixes[0].Id)).ToList();
                }
                else
                {
                    prefixes = DelegateService.commonService.GetPrefixes();
                }

                // Se obtiene las sucursales distintas al que tiene la cuenta base
                List<Branch> branches = _baseController.GetBranchesByUserId(_baseController.GetUserIdByName(User.Identity.Name)).Select(p => p).Where(h => (h.Id != accountAccount.Branch.Id)).ToList();

                if (prefixes.Count > 0)
                {
                    foreach (Prefix prefix in prefixes)
                    {
                        string prefixId = "00";

                        // Se arma el sufijo de ramo para cuenta contable
                        if (prefix.Id > 0)
                        {
                            prefixId = Convert.ToString(prefix.Id);
                            if (prefixId.Length < 2)
                            {
                                prefixId = "0" + prefixId;
                            }
                        }

                        if (branches.Count > 0)
                        {
                            foreach (Branch branchItem in branches)
                            {
                                string branchId = "00";

                                if (branchItem.Id > 0)
                                {
                                    branchId = Convert.ToString(branchItem.Id);
                                    if (branchId.Length < 2)
                                    {
                                        branchId = "0" + branchId;
                                    }
                                }

                                // Se arma la nueva cuenta contable.
                                AccountingAccountDTO newAccountingAccount = new AccountingAccountDTO();
                                newAccountingAccount.AccountingAccountId = 0;
                                newAccountingAccount.AccountingAccountParentId = accountAccount.AccountingAccountParentId;
                                newAccountingAccount.Number = accountAccount.Number.Trim().Substring(0, 12) + prefixId + branchId;
                                newAccountingAccount.Description = accountAccount.Description + "_" + prefix.Description.Substring(0, 3) + "_" + branchItem.Description.Substring(0, 3);
                                newAccountingAccount.Branch = new BranchDTO();
                                newAccountingAccount.Branch.Id = branchItem.Id;
                                newAccountingAccount.Prefixes = new List<PrefixDTO>();
                                newAccountingAccount.Prefixes.Add(new PrefixDTO() { Id = prefix.Id });
                                newAccountingAccount.CostCenters = new List<CostCenterDTO>();
                                newAccountingAccount.CostCenters = accountAccount.CostCenters;
                                newAccountingAccount.Currency = new CurrencyDTO();
                                newAccountingAccount.Currency.Id = accountAccount.Currency.Id;
                                newAccountingAccount.RequiresAnalysis = accountAccount.RequiresAnalysis;
                                newAccountingAccount.RequiresCostCenter = accountAccount.RequiresCostCenter;
                                newAccountingAccount.Analysis = new AnalysisDTO();
                                newAccountingAccount.Analysis = accountAccount.Analysis;
                                newAccountingAccount.AccountingAccountType = new AccountingAccountTypeDTO();
                                newAccountingAccount.AccountingAccountType = accountAccount.AccountingAccountType;
                                newAccountingAccount.AccountingNature = accountAccount.AccountingNature;
                                newAccountingAccount.AccountingAccountApplication = accountAccount.AccountingAccountApplication;
                                newAccountingAccount.IsReclassify = accountAccount.IsReclassify;
                                newAccountingAccount.RecAccounting = accountAccount.RecAccounting;
                                newAccountingAccount.IsRevalue = accountAccount.IsRevalue;
                                newAccountingAccount.RevAcountingPos = accountAccount.RevAcountingPos;
                                newAccountingAccount.RevAcountingNeg = accountAccount.RevAcountingNeg;


                                // Se graba la cuenta contable.
                                newAccountingAccount = DelegateService.glAccountingApplicationService.SaveAccountingAccount(newAccountingAccount);

                                accountingAccounts.Add(newAccountingAccount);
                                isSuccessfully = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                isSuccessfully = false;
            }

            return new UifJsonResult(isSuccessfully, accountingAccounts);
        }

        /// <summary>
        /// LoadSourceAccounts
        /// Método para cargar las cuentas contables para propagación de bloqueos
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBlockadeSpreadingAccountingAccounts(string query)
        {
            try
            {
                List<AccountingAccountModel> accountingAccounts = GetAccountingAccounts(query);
                return Json(accountingAccounts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateAccountsToBeModified
        /// Obtiene el número de registros que podrían ser afectados a partir de un número de cuenta base.
        /// </summary>
        /// <param name="baseNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateAccountsToBeModified(string baseNumber)
        {
            int result;

            try
            {
                result = GetAccountingAccountsByBaseNumber(baseNumber).Count;
            }
            catch (Exception)
            {
                result = 0;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SpreadAccountBlockade
        /// Realiza la propagación de bloqueo de cuentas.
        /// </summary>
        /// <param name="accountingAccountSourceId"></param>
        /// <param name="accountingAccountDestinationId"></param>
        /// <param name="baseNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SpreadAccountBlockade(int accountingAccountSourceId, int accountingAccountDestinationId, string baseNumber)
        {
            bool result = false;

            try
            {
                //Se obtiene la cuenta contable de origen.
                AccountingAccountDTO accountingAccountSource;
                accountingAccountSource = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingAccountSourceId);

                if (accountingAccountDestinationId > 0)
                {
                    SaveBlockadeDestination(accountingAccountSource, accountingAccountDestinationId);
                }
                else
                {
                    //Se obtienen las cuentas por medio del número base
                    List<AccountingAccountDTO> destinationAccounts = GetAccountingAccountsByBaseNumber(baseNumber);

                    if (destinationAccounts.Count > 0)
                    {
                        foreach (AccountingAccountDTO item in destinationAccounts)
                        {
                            SaveBlockadeDestination(accountingAccountSource, item.AccountingAccountId);
                        }
                    }
                }

                result = true;
            }
            catch (BusinessException)
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods

        #region PrivateMethods

        /// <summary>
        /// Obtiene el listado de cuentas contables formateado para autocomplete
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>List<AccountingAccountModel></returns>
        private List<AccountingAccountModel> GetAccountingAccounts(string accountNumber)
        {
            List<AccountingAccountModel> accounts = new List<AccountingAccountModel>();
            AccountingAccountDTO accountingAccount = new AccountingAccountDTO() { Number = accountNumber };

            try
            {
                List<AccountingAccountDTO> accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountsByNumberDescription(accountingAccount);

                if (accountingAccounts.Count > 0)
                {
                    foreach (var item in accountingAccounts)
                    {
                        AccountingAccountModel accountModel = new AccountingAccountModel();
                        accountModel.AccountingAccountId = item.AccountingAccountId;
                        accountModel.AccountingAccountName = item.Description;
                        accountModel.AccountingAccountNumber = item.Number;
                        accountModel.FullName = item.Number + " - " + item.Description;
                        accountModel.AccountingAccountParentId = item.AccountingAccountParentId;
                        accounts.Add(accountModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accounts;
        }

        /// <summary>
        /// GetAccountingAccountsByBaseNumber
        /// Obtiene la cuentas a partir de una cuenta "base"
        /// </summary>
        /// <param name="baseNumber"></param>
        /// <returns>List<AccountingAccount></returns>
        private List<AccountingAccountDTO> GetAccountingAccountsByBaseNumber(string baseNumber)
        {
            if (baseNumber.Contains("%"))
            {
                var split = baseNumber.Split('%');
                baseNumber = split[0];
            }
            List<AccountingAccountDTO> accountingAccounts = new List<AccountingAccountDTO>();

            try
            {
                accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccounts();
                accountingAccounts = (from i in accountingAccounts where i.Number.StartsWith(baseNumber) select i).ToList();
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return accountingAccounts;
        }

        /// <summary>
        /// SaveBlockadeDestination
        /// </summary>
        /// <param name="accountingAccountSource"></param>
        /// <param name="accountingAccountDestinationId"></param>
        private void SaveBlockadeDestination(AccountingAccountDTO accountingAccountSource, int accountingAccountDestinationId)
        {
            //Se obtiene la cuenta contable de destino
            AccountingAccountDTO accountingAccountDestination = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingAccountDestinationId);

            try
            {
                //se igualan los campos que se van a replicar en la cuenta contable destino (los valores que se mantienen son el id de la cuenta, id de la cuenta contable padre, el número y el nombre)
                accountingAccountDestination.AccountingAccountApplication = accountingAccountSource.AccountingAccountApplication;
                accountingAccountDestination.AccountingAccountType = accountingAccountSource.AccountingAccountType;
                accountingAccountDestination.AccountingConcepts = accountingAccountSource.AccountingConcepts;
                accountingAccountDestination.AccountingNature = accountingAccountSource.AccountingNature;
                accountingAccountDestination.Analysis = accountingAccountSource.Analysis;
                accountingAccountDestination.Branch = accountingAccountSource.Branch;
                accountingAccountDestination.Comments = accountingAccountSource.Comments;
                accountingAccountDestination.CostCenters = accountingAccountSource.CostCenters;
                accountingAccountDestination.Currency = accountingAccountSource.Currency;
                accountingAccountDestination.Prefixes = accountingAccountSource.Prefixes;
                accountingAccountDestination.RequiresAnalysis = accountingAccountSource.RequiresAnalysis;
                accountingAccountDestination.RequiresCostCenter = accountingAccountSource.RequiresCostCenter;


                DelegateService.glAccountingApplicationService.UpdateAccountingAccount(accountingAccountDestination);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion PrivateMethods
    }
}