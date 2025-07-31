using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

// Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Framework.UIF.Web.Services;

// Sistran FWK
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class AccountingController : Controller
    {
        #region Instance Variables
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region PaymentConcept

        /// <summary>
        /// GetPaymentConceptById
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentConceptById(string query, string param)
        {
            try
            {
                string[] data = param.Split('/');
                List<object> paymentConcepts = new List<object>();

                if (data.Length > 0)
                {
                    int paymentConceptId = query == "" ? 0 : Convert.ToInt32(query);
                    int branchId = data[0] == "" ? 0 : Convert.ToInt32(data[0]);
                    int payerId = data[1] == "" ? 0 : Convert.ToInt32(data[1]);

                    int userId = _commonController.GetUserIdByName(User.Identity.Name);

                    paymentConcepts = _commonController.GetAccountingConceptById(paymentConceptId, branchId, userId, payerId);
                    if (paymentConcepts != null && paymentConcepts.Count > 0)
                    {
                        return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        paymentConcepts.Add(new
                        {
                            Name = Sistran.Core.Framework.UIF.Web.Resources.Global.RegisterNotFound,
                            Id = Sistran.Core.Framework.UIF.Web.Resources.Global.RegisterNotFound,
                        });
                    }
                }
                return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// GetPaymentConceptById
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingConcepstByFilter(string query, string filter)
        {
            try
            {
                string[] data = filter.Split('/');
                List<object> paymentConcepts = new List<object>();

                if (data.Length > 0)
                {
                    AccountingAccountFilterDTO accountingAccountFilterDTO = new AccountingAccountFilterDTO()
                    {
                        ConceptId = Convert.ToInt32(query),
                        BranchId = Convert.ToInt32(data[0]),
                        UserId = _commonController.GetUserIdByName(User.Identity.Name)
                    };
                    paymentConcepts = _commonController.GetAccountingConceptsByFilter(accountingAccountFilterDTO);
                }
                return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetPaymentConceptByDescription
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentConceptByDescription(string query, string filter)
        {
            try
            {
                string[] data = filter.Split('/');
                List<object> paymentConcepts = new List<object>();

                if (data.Length > 0)
                {
                    AccountingAccountFilterDTO accountingAccountFilterDTO = new AccountingAccountFilterDTO()
                    {
                        ConceptDescription = query,
                        BranchId = Convert.ToInt32(data[0]),
                        UserId = _commonController.GetUserIdByName(User.Identity.Name)
                    };
                    paymentConcepts = _commonController.GetAccountingConceptsByFilter(accountingAccountFilterDTO);
                }
                return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion PaymentConcept

        #region BankReconciliation

        /// <summary>
        /// IsBankReconciliation
        /// Me indica si la cuenta contable seleccionada pertenece a una cuenta bancaria
        /// </summary>
        /// <param name="generalLedgerId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult IsBankReconciliation(int generalLedgerId)
        {
            var companyBankAccounts = (from companyBankAccount in DelegateService.accountingParameterService.GetBankAccountCompanies() where companyBankAccount.AccountingAccount.AccountingAccountId == generalLedgerId select companyBankAccount).ToList();
            if (companyBankAccounts.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion BankReconciliation

        #region AccountingAccount

        /// <summary>
        /// GetAccountingAccountByNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountByNumber(string query, string filter)
        {
            try
            {
                string[] data = filter.Split('/');
                List<object> paymentConcepts = new List<object>();

                if (data.Length > 0)
                {
                    AccountingAccountFilterDTO accountingAccountFilterDTO = new AccountingAccountFilterDTO()
                    {
                        AccountingNumber = query,
                        BranchId = Convert.ToInt32(data[0]),
                        UserId = _commonController.GetUserIdByName(User.Identity.Name)
                    };
                    paymentConcepts = _commonController.GetAccountingConceptsByFilter(accountingAccountFilterDTO);
                }
                return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetAccountingAccountByDescription
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountByDescription(string query, string filter)
        {
            try
            {
                string[] data = filter.Split('/');
                List<object> paymentConcepts = new List<object>();

                if (data.Length > 0)
                {
                    AccountingAccountFilterDTO accountingAccountFilterDTO = new AccountingAccountFilterDTO()
                    {
                        AccountingDescription = query,
                        BranchId = Convert.ToInt32(data[0]),
                        UserId = _commonController.GetUserIdByName(User.Identity.Name)
                    };
                    paymentConcepts = _commonController.GetAccountingConceptsByFilter(accountingAccountFilterDTO);
                }
                return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion AccountingAccount

        #region AccountingTransaction

        ///<summary>
        /// GetTempAccountingTransactionItemByTempApplicationId
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempAccountingTransactionItemByTempApplicationId(int tempApplicationId)
        {
            List<ApplicationAccountingDTO> tempApplicationAccountings = new List<ApplicationAccountingDTO>();
            List<ApplicationAccountingAnalysisDTO> applicationAccountingAnalysisDTO = new List<ApplicationAccountingAnalysisDTO>();

            if (tempApplicationId > 0)
            {
                tempApplicationAccountings =
                    DelegateService.accountingApplicationService.GetTempAccountingTransactionItemByTempApplicationId(tempApplicationId);


            }
            return Json(tempApplicationAccountings, JsonRequestBehavior.AllowGet);
        }
        ///<summary>
        /// GetTempAccountingTransactionByTempAccountingApplicationId
        /// </summary>
        /// <param name="tempAccountingApplicationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempAccountingTransactionByTempAccountingApplicationId(int tempAccountingApplicationId)
        {
            ApplicationAccountingDTO tempApplicationAccounting =
                    DelegateService.accountingApplicationService.GetTempAccountingTransactionByTempAccountingApplicationId(tempAccountingApplicationId);

            return Json(tempApplicationAccounting, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveTempAccountingTransactionRequest
        /// </summary>
        /// <param name="accountingTransactionModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempAccountingTransactionRequest(ApplicationAccountingDTO accountingTransactionModel)
        {
            int tempDailyAccountingCode = 0;
            /*DateTime? date = null;
            int bankReconciliationId = 0;

            if (accountingTransactionModel.ReceiptDate == Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                date = null;
            }
            else
            {
                date = accountingTransactionModel.ReceiptDate;
            }

            if (accountingTransactionModel.BankReconciliationId < 0)
            {
                bankReconciliationId = 0;
            }
            else
            {
                bankReconciliationId = accountingTransactionModel.BankReconciliationId;
            }

            ApplicationAccountingTransactionDTO dailyAccountingTransaction = new ApplicationAccountingTransactionDTO();
            dailyAccountingTransaction.Description = accountingTransactionModel.Description;
            dailyAccountingTransaction.ApplicationAccountingItems = new List<ApplicationAccountingDTO>();

            ApplicationAccountingDTO applicationAccountingItem = new ApplicationAccountingDTO();

            SCRDTO.BranchDTO branch = new SCRDTO.BranchDTO();
            branch.Id = Convert.ToInt32(accountingTransactionModel.Branch.Id);

            ACCDTO.SalePointDTO salePoint = new ACCDTO.SalePointDTO();
            salePoint.Id = Convert.ToInt32(accountingTransactionModel.SalePoint.Id);

            ACCDTO.IndividualDTO beneficiary = new ACCDTO.IndividualDTO() { IndividualId = Convert.ToInt32(accountingTransactionModel.BeneficiaryId) };

            AccountingNatures accountingNature = (AccountingNatures)accountingTransactionModel.NatureId;

            ACCDTO.AmountDTO amount = new ACCDTO.AmountDTO()
            {
                Currency = new SCRDTO.CurrencyDTO() { Id = Convert.ToInt32(accountingTransactionModel.CurrencyId) },
                Value = Convert.ToDecimal(accountingTransactionModel.Amount)
            };
            ACCDTO.ExchangeRateDTO exchangeRate = new ACCDTO.ExchangeRateDTO() {
                SellAmount = Convert.ToDecimal(accountingTransactionModel.Exchange)
            };
            ACCDTO.AmountDTO localAmount = new ACCDTO.AmountDTO() {
                Currency = new SCRDTO.CurrencyDTO() { Id = Convert.ToInt32(accountingTransactionModel.CurrencyId) },
                Value = (Convert.ToDecimal(accountingTransactionModel.Exchange) * Convert.ToDecimal(accountingTransactionModel.Amount)) };

            BookAccountDTO bookAccount = new BookAccountDTO();
            bookAccount.Id = Convert.ToInt32(accountingTransactionModel.AccountId);
            //Añadido para BE
            bookAccount.AccountNumber = Convert.ToString(accountingTransactionModel.AccountNumber);

            applicationAccountingItem.Id = accountingTransactionModel.Id;
            applicationAccountingItem.Branch = branch;
            applicationAccountingItem.SalePoint = salePoint;
            //dailyAccountingTransactionItem.Company = company;
            applicationAccountingItem.Beneficiary = beneficiary;
            applicationAccountingItem.AccountingNature = Convert.ToInt32(accountingNature);
            applicationAccountingItem.Amount = amount;
            applicationAccountingItem.ExchangeRate = exchangeRate;
            applicationAccountingItem.LocalAmount = localAmount;
            applicationAccountingItem.BookAccount = bookAccount;
            */
            //codigos de análisis
            //accountingTransactionModel.AccountingAnalysisCodes = new List<ApplicationAccountingAnalysisDTO>();
            //ApplicationAccountingDTO applicationAccountingItem = new ApplicationAccountingDTO();

            //if (accountingTransactionModel != null && accountingTransactionModel.AccountingAnalysisCodes.Count > 0)
            //{
            //    foreach (ApplicationAccountingAnalysisDTO analysis in accountingTransactionModel.AccountingAnalysisCodes)
            //    {
            //        ApplicationAccountingAnalysisDTO dailyAccountingAnalysisCode = new ApplicationAccountingAnalysisDTO();
            //        dailyAccountingAnalysisCode.Id = 0; //identificador del registro
            //        dailyAccountingAnalysisCode.AnalysisId = analysis.AnalysisId;
            //        dailyAccountingAnalysisCode.AnalysisConceptId = analysis.AnalysisConceptId;
            //        dailyAccountingAnalysisCode.ConceptKey = analysis.ConceptKey;
            //        dailyAccountingAnalysisCode.Description = analysis.Description;

            //        applicationAccountingItem.AccountingAnalysisCodes.Add(dailyAccountingAnalysisCode);
            //    }
            //}
            /*
            //centros de costos
            applicationAccountingItem.AccountingCostCenters = new List<ApplicationAccountingCostCenterDTO>();

            if (accountingTransactionModel.CostCenters != null && accountingTransactionModel.CostCenters.Count > 0)
            {
                foreach (CostCenterModel costCenter in accountingTransactionModel.CostCenters)
                {
                    ApplicationAccountingCostCenterDTO dailyAccountingCostCenter = new ApplicationAccountingCostCenterDTO();
                    dailyAccountingCostCenter.Id = 0; //identificador del registro
                    dailyAccountingCostCenter.CostCenter = new SCRDTO.CostCenterDTO();
                    dailyAccountingCostCenter.CostCenter.CostCenterId = costCenter.CostCenterId;
                    dailyAccountingCostCenter.Percentage = costCenter.Percentage;

                    applicationAccountingItem.AccountingCostCenters.Add(dailyAccountingCostCenter);
                }
            }*/

            ApplicationAccountingTransactionDTO applicationAccountingTransaction = new ApplicationAccountingTransactionDTO();
            applicationAccountingTransaction.Description = accountingTransactionModel.Description;
            applicationAccountingTransaction.ApplicationAccountingItems = new List<ApplicationAccountingDTO>();
            applicationAccountingTransaction.ApplicationAccountingItems.Add(accountingTransactionModel);



            tempDailyAccountingCode = DelegateService.accountingApplicationService.SaveTempAccountingTransaction(applicationAccountingTransaction);

            return Json(tempDailyAccountingCode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteTempApplicationAccounting
        /// </summary>
        /// <param name="tempApplicationAccountingId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempApplicationAccounting(int tempApplicationAccountingId)
        {
            bool isDeletedTempDailyAccounting = DelegateService.accountingApplicationService.DeleteTempApplicationAccounting(tempApplicationAccountingId);
            return Json(isDeletedTempDailyAccounting, JsonRequestBehavior.AllowGet);
        }
        #endregion AccountingTransaction


        #region Codigos de Analisis 
        public JsonResult GetTempApplicationAccountingAnalysisByTempAppAccountingId(int tempAppAccountingId)
        {
            List<ApplicationAccountingAnalysisDTO> applicationAccountingAnalysisDTO = DelegateService.accountingApplicationService.GetTempApplicationAccountingAnalysisByTempAppAccountingId(tempAppAccountingId);
            return Json(applicationAccountingAnalysisDTO, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult GetTempApplicationAccountingCostCenterByTempAppAccountingId(int tempAppAccountingId)
        {
            List<ApplicationAccountingCostCenterDTO> applicationAccountingAnalysisDTO = DelegateService.accountingApplicationService.GetTempApplicationAccountingCostCentersByTempAppAccountingId(tempAppAccountingId);
            return Json(applicationAccountingAnalysisDTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckoutAnalysisCodeByAnalysisConceptKeyId(int analysisConceptKeyId, string keyDescription)
        {

            MessageDTO messageDTO = new MessageDTO()
            {
                Success = true,
                Info = ""
            };
            try
            {
                messageDTO.Code = DelegateService.accountingApplicationService.CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(analysisConceptKeyId, keyDescription);
            }
            catch (BusinessException exception)
            {
                messageDTO.Success = false;
                messageDTO.Info = exception.Message;
            }
            return Json(messageDTO, JsonRequestBehavior.AllowGet);
        }
    }
}