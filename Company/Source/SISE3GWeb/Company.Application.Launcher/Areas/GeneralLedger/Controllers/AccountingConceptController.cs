using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class AccountingConceptController : Controller
    {
        #region Instance Variables
        readonly BaseController _baseController = new BaseController();

        #endregion

        #region View

        /// <summary>
        /// MainAccountingConcept
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAccountingConcept()
        {

            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View("~/Areas/GeneralLedger/Views/AccountingConcept/MainAccountingConcept.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }          
        }

        /// <summary>
        /// DuplicatePaymentConcept
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DuplicatePaymentConcept()
        {
            try
            {
                return View("~/Areas/GeneralLedger/Views/AccountingConcept/DuplicatePaymentConcept.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }           
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetAccountingConcept
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingConcepts()
        {
            List<AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetLiteAccountingConcepts();

            var accountingConceptResponse = from items in accountingConcepts
                                            select new
                                            {
                                                items.Id,
                                                items.AccountingAccount.AccountingAccountId,
                                                items.Description,
                                                items.AgentEnabled,
                                                items.CoInsurancedEnabled,
                                                items.ReInsuranceEnabled,
                                                items.InsuredEnabled,
                                                AccountingAccount = new
                                                {
                                                    items.AccountingAccount.Number,
                                                    items.AccountingAccount.Description
                                                },
                                                items.ItemEnabled
                                            };

            return new UifTableResult(accountingConceptResponse);
        }

        /// <summary>
        /// SaveAccountingConcept
        /// </summary>
        /// <param name="accountingConceptModal"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveAccountingConcept(AccountingConceptModel accountingConceptModal)
        {
            bool isSucessfully = false;
            int saved = 0;
            AccountingConceptDTO accountingConcept = new AccountingConceptDTO();

            try
            {
                accountingConcept.Id = accountingConceptModal.Id;
                accountingConcept.AccountingAccount = new AccountingAccountDTO();
                accountingConcept.AccountingAccount.AccountingAccountId = accountingConceptModal.AccountingAccountId;
                accountingConcept.Description = accountingConceptModal.Description;
                accountingConcept.AgentEnabled = accountingConceptModal.AgentEnabled;
                accountingConcept.CoInsurancedEnabled = accountingConceptModal.CoInsurancedEnabled;
                accountingConcept.ReInsuranceEnabled = accountingConceptModal.ReInsuranceEnabled;
                accountingConcept.InsuredEnabled = accountingConceptModal.InsuredEnabled;
                accountingConcept.ItemEnabled = accountingConceptModal.ItemEnabled;
                if (accountingConcept.Id == 0)
                {
                    accountingConcept = DelegateService.glAccountingApplicationService.SaveAccountingConcept(accountingConcept);
                }
                else
                {
                    accountingConcept = DelegateService.glAccountingApplicationService.UpdateAccountingConcept(accountingConcept);
                }

                isSucessfully = true;
                saved = accountingConcept.Id;
            }
            catch (Exception)
            {
                isSucessfully = false;
                saved = 0;
            }

            return Json(new { success = isSucessfully, result = saved }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAccountingConcept
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteAccountingConcept(int accountingConceptId)
        {
            bool isDeleted = false;
            int deleted = 0;

            try
            {
                AccountingConceptDTO accountingConcept = new AccountingConceptDTO();
                accountingConcept.Id = accountingConceptId;
                isDeleted = DelegateService.glAccountingApplicationService.DeleteAccountingConcept(accountingConcept);
                deleted = 1;
            }
            catch (Exception)
            {
                isDeleted = false;
                deleted = 0;
            }

            return Json(new { success = isDeleted, result = deleted }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingConceptsById
        /// Autocomplete para Conceptos de pagos.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingConceptsById(string query)
        {
            List<AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConcepts();
            List<AccountingConceptModel> filteredConcepts = new List<AccountingConceptModel>();

            accountingConcepts = accountingConcepts.Where(a => Convert.ToString(a.Id).Contains(query)).ToList();

            if (accountingConcepts.Count > 0)
            {
                foreach (AccountingConceptDTO accountingConcept in accountingConcepts)
                {
                    AccountingConceptModel accountingConceptModel = new AccountingConceptModel();
                    accountingConceptModel.Id = accountingConcept.Id;
                    accountingConceptModel.Description = accountingConcept.Id + " - " + accountingConcept.Description;

                    filteredConcepts.Add(accountingConceptModel);
                }
            }

            return Json(filteredConcepts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetSourceBranches
        /// Obtiene todas las sucursales parametrizadas para el concepto seleccionado.
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetSourceBranches(int accountingConceptId)
        {
            List<Branch> sourceBranches = GetBranchesByAccountingConceptId(accountingConceptId);
            return new UifSelectResult(sourceBranches);
        }

        /// <summary>
        /// GetDestinationBranches
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDestinationBranches(int accountingConceptId)
        {
            //Modelo para la opción "Todas las sucursales"
            Branch branch = new Branch();
            branch.Id = 0;
            string branchDescription = Global.AllBranches;
            branch.Description = branchDescription.ToUpper();

            List<Branch> destinationBranches = GetRemainingBranchesByAccountingConceptId(accountingConceptId);
            destinationBranches.Insert(0, branch);

            return new UifSelectResult(destinationBranches);
        }

        /// <summary>
        /// SavePaymentConceptDuplication
        /// </summary>
        /// <param name="paymentConceptId"></param>
        /// <param name="sourceBranch"></param>
        /// <param name="destinationBranch"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SavePaymentConceptDuplication(int paymentConceptId, int sourceBranch, int destinationBranch)
        {
            int result = 0;

            try
            {
                if (destinationBranch > 0) //una sola sucursal
                {
                    SaveDestinationBranchAccountingConcept(paymentConceptId, sourceBranch, destinationBranch);
                    result = 1;
                }
                else //todas las sucursales
                {
                    List<Branch> destinationBranches = GetRemainingBranchesByAccountingConceptId(paymentConceptId);

                    if (destinationBranches.Count > 0)
                    {
                        foreach (var item in destinationBranches)
                        {
                            SaveDestinationBranchAccountingConcept(paymentConceptId, sourceBranch, item.Id);
                        }

                        result = 1;
                    }
                    else
                    {
                        result = 2; //todas las sucursales ya han sido parametrizadas
                    }
                }
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                result = 0;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// GetBranchesByAccountingConceptId
        /// Obtiene las sucursales por concepto contable.
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <returns>List<Branch></returns>
        private List<Branch> GetBranchesByAccountingConceptId(int accountingConceptId)
        {
            List<BranchAccountingConceptDTO> branchAccountingConcepts = DelegateService.glAccountingApplicationService.GetBranchAccountingConcepts();
            List<Branch> branches = new List<Branch>();

            List<BranchAccountingConceptDTO> filteredBranchAccountingConcepts = branchAccountingConcepts.Where(b => b.AccountingConcept.Id == accountingConceptId).ToList();

            if (filteredBranchAccountingConcepts.Count > 0)
            {
                foreach (BranchAccountingConceptDTO item in filteredBranchAccountingConcepts)
                {
                    Branch branch = new Branch();
                    branch.Id = item.Branch.Id;
                    branch.Description = DelegateService.commonService.GetBranchById(item.Branch.Id).Description;

                    branches.Add(branch);
                }
            }

            return branches;
        }

        /// <summary>
        /// GetRemainingBranchesByAccountingConceptId
        /// Obtiene las sucursales que no se encuentran parametrizadas para el concepto contable.
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <returns>List<Branch></returns>
        private List<Branch> GetRemainingBranchesByAccountingConceptId(int accountingConceptId)
        {
            //Se obtienen las sucursales parametrizadas por concepto contable.
            List<Branch> setupBranches = GetBranchesByAccountingConceptId(accountingConceptId);
            List<int> setupBranchesIds = new List<int>();

            if (setupBranches.Count > 0)
            {
                foreach (var item in setupBranches)
                {
                    setupBranchesIds.Add(item.Id);
                }
            }

            //Listado de todas las sucursales
            List<Branch> branches = _baseController.GetBranchesByUserId(SessionHelper.GetUserId());

            //Se obtienen las sucursales restantes
            List<Branch> remainingBranches = (from Branch b in branches where !setupBranchesIds.Contains(b.Id) select b).ToList();

            return remainingBranches;
        }

        /// <summary>
        /// GetBranchAccountingConceptByBranch
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="sourceBranchId"></param>
        /// <returns>BranchAccountingConcept</returns>
        private BranchAccountingConceptDTO GetBranchAccountingConceptByConceptIdAndSourceBranchId(int conceptId, int sourceBranchId)
        {
            BranchAccountingConceptDTO branchAccountingConcept = new BranchAccountingConceptDTO();

            //se obtiene el branch_accounting_concept a partir del concepto
            List<BranchAccountingConceptDTO> branchAccountingConcepts = DelegateService.glAccountingApplicationService.GetBranchAccountingConcepts();
            branchAccountingConcepts = (from BranchAccountingConceptDTO b in branchAccountingConcepts where b.AccountingConcept.Id == conceptId && b.Branch.Id == sourceBranchId select b).ToList();

            if (branchAccountingConcepts.Count > 0)
            {
                branchAccountingConcept = branchAccountingConcepts[0];
            }

            return branchAccountingConcept;
        }

        /// <summary>
        /// SaveDestinationBranchAccountingConcept
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="sourceBranch"></param>
        /// <param name="destinationBranch"></param>
        private void SaveDestinationBranchAccountingConcept(int conceptId, int sourceBranch, int destinationBranch)
        {
            UserBranchAccountingConceptDTO userBranchAccountingConcept = new UserBranchAccountingConceptDTO();

            try
            {
                //se obtiene el registro de BRANCH_ACCOUNTING_CONCEPT por concepto y sucursal de origen
                BranchAccountingConceptDTO branchAccountingConcept = GetBranchAccountingConceptByConceptIdAndSourceBranchId(conceptId, sourceBranch);
                branchAccountingConcept.Branch.Id = destinationBranch; //se actualiza el branch de destino
                branchAccountingConcept.Id = 0; //es un nuevo registro.

                //se graba el nuevo registro
                branchAccountingConcept = DelegateService.glAccountingApplicationService.SaveBranchAccountingConcept(branchAccountingConcept);

                //se graba la relación del nuevo registro con el usuario.
                userBranchAccountingConcept.BranchAccountingConcept = new BranchAccountingConceptDTO();
                userBranchAccountingConcept.BranchAccountingConcept.Id = branchAccountingConcept.Id;
                userBranchAccountingConcept.UserId = SessionHelper.GetUserId();
                DelegateService.glAccountingApplicationService.SaveUserBranchAccountingConcept(userBranchAccountingConcept);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }      

        #endregion PrivateMethods
    }
}