//System
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Reflection;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Services.UtilitiesServices.Enums;
using UNWDTO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;


//Sistran Company
using Sistran.Core.Application.AccountingServices.Enums;
using System.Configuration;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using Sistran.Core.Framework.UIF.Web.Resources;
using NPOI.SS.UserModel;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class PremiumsReceivableController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;

        #endregion

        #region Instance Variables
        readonly CommonController _commonController = new CommonController();
        readonly TemporarySearchController _temporarySearchController = new TemporarySearchController();
        readonly PaymentOrdersController _paymentOrdersController = new PaymentOrdersController();

        #endregion

        #region PremiumReceivableSearch


        public JsonResult GetPoliciesQuotas(UNWDTO.SearchPolicyPaymentDTO searchPolicyPaymentDTO)
        {
            List<UNWDTO.PremiumSearchPolicyDTO> policiesQuotas =
                              DelegateService.underwritingIntegrationService.GetPremiuPaymSearchPolicies(searchPolicyPaymentDTO);

            // Ordenamiento por Cuota/Endoso/Póliza
            policiesQuotas = (from order in policiesQuotas
                              orderby order.PolicyDocumentNumber, order.EndorsementId, order.PaymentNumber
                              select order).ToList();

            List<object> premiumReceivableSearchPoliciesResponses = new List<object>();

            foreach (UNWDTO.PremiumSearchPolicyDTO premiumSearchPolicyDTO in policiesQuotas)
            {

                premiumReceivableSearchPoliciesResponses.Add(new
                {
                    InsuredName = premiumSearchPolicyDTO.InsuredName,
                    InsuredDocumentNumberName = premiumSearchPolicyDTO.InsuredDocumentNumber + "-" + premiumSearchPolicyDTO.InsuredName,
                    PayerDocumentNumberName = premiumSearchPolicyDTO.PayerDocumentNumber + "-" + premiumSearchPolicyDTO.PayerName,
                    BranchPrefixPolicyEndorsement = premiumSearchPolicyDTO.BranchPrefixPolicyEndorsement,
                    BranchDescription = premiumSearchPolicyDTO.BranchDescription,
                    BranchId = premiumSearchPolicyDTO.BranchId,
                    BussinessTypeDescription = premiumSearchPolicyDTO.BussinessTypeDescription,
                    BussinessTypeId = premiumSearchPolicyDTO.BussinessTypeId,
                    CurrencyDescription = premiumSearchPolicyDTO.CurrencyDescription,
                    CurrencyId = premiumSearchPolicyDTO.CurrencyId,
                    InsuredDocumentNumber = premiumSearchPolicyDTO.InsuredDocumentNumber,
                    PolicyDocumentNumber = premiumSearchPolicyDTO.PolicyDocumentNumber,
                    PrefixId = premiumSearchPolicyDTO.PrefixId,
                    PrefixDescription = premiumSearchPolicyDTO.PrefixDescription,
                    PolicyId = premiumSearchPolicyDTO.PolicyId,
                    PayerDocumentNumber = premiumSearchPolicyDTO.PayerDocumentNumber,
                    PayerName = premiumSearchPolicyDTO.PayerName,
                    PaymentNumber = premiumSearchPolicyDTO.PaymentNumber,
                    PaymentExpirationDate = Convert.ToDateTime(premiumSearchPolicyDTO.PaymentExpirationDate).ToShortDateString(),
                    EndorsementId = premiumSearchPolicyDTO.EndorsementId,
                    EndorsementDocumentNumber = premiumSearchPolicyDTO.EndorsementDocumentNumber,
                    InsuredId = premiumSearchPolicyDTO.InsuredId,
                    PayerId = premiumSearchPolicyDTO.PayerId,
                    ExcessPayment = premiumSearchPolicyDTO.ExcessPayment,
                    ExchangeRate = premiumSearchPolicyDTO.ExchangeRate,
                    PolicyAgentDocumentNumberName = premiumSearchPolicyDTO.PolicyAgentDocumentNumberName,
                    PolicyAgentId = premiumSearchPolicyDTO.PolicyAgentId,
                    PolicyAgentDocumentNumber = premiumSearchPolicyDTO.PolicyAgentDocumentNumber,
                    PolicyAgentName = premiumSearchPolicyDTO.PolicyAgentName,
                    EndorsementTypeId = premiumSearchPolicyDTO.EndorsementTypeId,
                    EndorsementTypeDescription = premiumSearchPolicyDTO.EndorsementTypeDescription,
                    Address = premiumSearchPolicyDTO.Address,
                    PaymentAmount = premiumSearchPolicyDTO.Amount,
                    Amount = premiumSearchPolicyDTO.Amount,
                    // Aplicación de Preliquidación
                    PolicyNumber = premiumSearchPolicyDTO.PolicyDocumentNumber,
                    EndorsementNumber = premiumSearchPolicyDTO.EndorsementDocumentNumber,
                    BranchName = premiumSearchPolicyDTO.BranchDescription,
                    //QuotaAmount = premiumSearchPolicyDTO.Amount,
                    QuotaNumber = premiumSearchPolicyDTO.PaymentNumber,
                    QuotaValue = premiumSearchPolicyDTO.Amount,
                    TotalPremium = premiumSearchPolicyDTO.TotalPremium,
                    //ComponentId = premiumSearchPolicyDTO.ComponentId
                    ValueToCollect = premiumSearchPolicyDTO.Amount
                });
            }

            return Json(premiumReceivableSearchPoliciesResponses, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// PremiumReceivableSearchPolicy
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="payerId"></param>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="salesTicket"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementDocumentNumber"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PremiumReceivableSearchPolicy(UNWDTO.SearchPolicyPaymentDTO searchPolicyPayment)
        {

            List<object> premiumReceivableSearchPoliciesResponses = new List<object>();

            List<UNWDTO.PremiumSearchPolicyDTO> premiumReceivableSearchPolicies =
                              DelegateService.accountingApplicationService.GetPaymentQuotas(searchPolicyPayment);

            // Ordenamiento por Cuota/Endoso/Póliza
            premiumReceivableSearchPolicies = (from order in premiumReceivableSearchPolicies
                                               orderby order.PolicyDocumentNumber, order.EndorsementId, order.PaymentNumber
                                               select order).ToList();


            //************************************************************************************************
            //ORDENAMIENTO POR NUMERO DE CUOTAS            
            List<UNWDTO.PremiumSearchPolicyDTO> premiumSearchPolicyDTOs = new List<UNWDTO.PremiumSearchPolicyDTO>();

            foreach (UNWDTO.PremiumSearchPolicyDTO premiumSearchPolicyDTO in premiumReceivableSearchPolicies)
            {
                premiumSearchPolicyDTOs.Add(premiumSearchPolicyDTO);
            }

            foreach (UNWDTO.PremiumSearchPolicyDTO premiumSearchPolicyDTO in premiumSearchPolicyDTOs)
            {

                premiumReceivableSearchPoliciesResponses.Add(new
                {
                    InsuredName = premiumSearchPolicyDTO.InsuredName,
                    InsuredDocumentNumberName = premiumSearchPolicyDTO.InsuredDocumentNumber + "-" + premiumSearchPolicyDTO.InsuredName,
                    PayerDocumentNumberName = premiumSearchPolicyDTO.PayerDocumentNumber + "-" + premiumSearchPolicyDTO.PayerName,
                    BranchPrefixPolicyEndorsement = premiumSearchPolicyDTO.BranchPrefixPolicyEndorsement,
                    BranchDescription = premiumSearchPolicyDTO.BranchDescription,
                    BranchId = premiumSearchPolicyDTO.BranchId,
                    BussinessTypeDescription = premiumSearchPolicyDTO.BussinessTypeDescription,
                    BussinessTypeId = premiumSearchPolicyDTO.BussinessTypeId,
                    CurrencyDescription = premiumSearchPolicyDTO.CurrencyDescription,
                    CurrencyId = premiumSearchPolicyDTO.CurrencyId,
                    InsuredDocumentNumber = premiumSearchPolicyDTO.InsuredDocumentNumber,
                    PolicyDocumentNumber = premiumSearchPolicyDTO.PolicyDocumentNumber,
                    PrefixId = premiumSearchPolicyDTO.PrefixId,
                    PrefixDescription = premiumSearchPolicyDTO.PrefixDescription,
                    PolicyId = premiumSearchPolicyDTO.PolicyId,
                    PayerDocumentNumber = premiumSearchPolicyDTO.PayerDocumentNumber,
                    PayerName = premiumSearchPolicyDTO.PayerName,
                    PaymentNumber = premiumSearchPolicyDTO.PaymentNumber,
                    PaymentExpirationDate = Convert.ToDateTime(premiumSearchPolicyDTO.PaymentExpirationDate).ToShortDateString(),
                    EndorsementId = premiumSearchPolicyDTO.EndorsementId,
                    EndorsementDocumentNumber = premiumSearchPolicyDTO.EndorsementDocumentNumber,
                    InsuredId = premiumSearchPolicyDTO.InsuredId,
                    PayerId = premiumSearchPolicyDTO.PayerId,
                    ExcessPayment = premiumSearchPolicyDTO.ExcessPayment,
                    ExchangeRate = premiumSearchPolicyDTO.ExchangeRate,
                    PolicyAgentDocumentNumberName = premiumSearchPolicyDTO.PolicyAgentDocumentNumberName,
                    PolicyAgentId = premiumSearchPolicyDTO.PolicyAgentId,
                    PolicyAgentDocumentNumber = premiumSearchPolicyDTO.PolicyAgentDocumentNumber,
                    PolicyAgentName = premiumSearchPolicyDTO.PolicyAgentName,
                    EndorsementTypeId = premiumSearchPolicyDTO.EndorsementTypeId,
                    EndorsementTypeDescription = premiumSearchPolicyDTO.EndorsementTypeDescription,
                    Address = premiumSearchPolicyDTO.Address,
                    PaymentAmount = premiumSearchPolicyDTO.Amount,
                    Amount = premiumSearchPolicyDTO.Amount,
                    // Aplicación de Preliquidación
                    PolicyNumber = premiumSearchPolicyDTO.PolicyDocumentNumber,
                    EndorsementNumber = premiumSearchPolicyDTO.EndorsementDocumentNumber,
                    BranchName = premiumSearchPolicyDTO.BranchDescription,
                    //QuotaAmount = premiumSearchPolicyDTO.Amount,
                    QuotaNumber = premiumSearchPolicyDTO.PaymentNumber,
                    QuotaValue = premiumSearchPolicyDTO.Amount,
                    TotalPremium = premiumSearchPolicyDTO.TotalPremium,
                    //ComponentId = premiumSearchPolicyDTO.ComponentId
                    ValueToCollect = premiumSearchPolicyDTO.Amount
                });
            }

            return Json(premiumReceivableSearchPoliciesResponses, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempPremiumReceivableItemByTempImputationId(int tempImputationId)
        {
            List<object> premiumReceivableItemResponse = new List<object>();

            try
            {

                List<SCRDTO.PremiumReceivableItemDTO> premiumReceivableItems =
                DelegateService.accountingApplicationService.GetTempApplicationPremiumByApplicationId(tempImputationId).OrderBy(o => o.PaymentNumber).ToList();

                List<SCRDTO.PremiumReceivableItemDTO> premiumReceivableItemResults = new List<SCRDTO.PremiumReceivableItemDTO>();
                foreach (SCRDTO.PremiumReceivableItemDTO premiumReceivableItemDto in premiumReceivableItems)
                {
                    premiumReceivableItemResults.Add(premiumReceivableItemDto);
                }

                //************************************************************************************************
                //ORDENAMIENTO POR NUMERO DE CUOTAS
                //DDV
                var premiumReceivableItemResultOrderByQuotas = from premiums in premiumReceivableItemResults
                                                               orderby premiums.BranchId, premiums.PrefixId,
                                                                   premiums.PolicyDocumentNumber, premiums.EndorsementDocumentNumber, premiums.PaymentNumber
                                                               select premiums;

                foreach (SCRDTO.PremiumReceivableItemDTO premiumReceivableItem in premiumReceivableItemResultOrderByQuotas)
                {

                    var discountedCommissions = DelegateService.accountingApplicationService.GetTempApplicationPremiumCommissByTempAppId(string.Empty, string.Empty, premiumReceivableItem.PremiumReceivableItemId);
                    premiumReceivableItemResponse.Add(new
                    {
                        InsuredName = premiumReceivableItem.InsuredName,
                        ApplicationPremiumId = premiumReceivableItem.PremiumReceivableItemId,
                        InsuredDocumentNumberName = premiumReceivableItem.InsuredDocumentNumberName,
                        PayerDocumentNumberName = premiumReceivableItem.PayerDocumentNumberName,
                        BranchPrefixPolicyEndorsement = premiumReceivableItem.BranchPrefixPolicyEndorsement,
                        BranchDescription = premiumReceivableItem.BranchDescription,
                        BranchId = premiumReceivableItem.BranchId,
                        BussinessTypeDescription = premiumReceivableItem.BussinessTypeDescription,
                        BussinessTypeId = premiumReceivableItem.BussinessTypeId,
                        CurrencyDescription = premiumReceivableItem.CurrencyDescription,
                        CurrencyId = premiumReceivableItem.CurrencyId,
                        DiscountedCommission = premiumReceivableItem.DiscountedCommission,
                        InsuredDocumentNumber = premiumReceivableItem.InsuredDocumentNumber,
                        PolicyDocumentNumber = premiumReceivableItem.PolicyDocumentNumber,
                        PrefixId = premiumReceivableItem.PrefixId,
                        PrefixDescription = premiumReceivableItem.PrefixDescription,
                        PrefixTyniDescription = premiumReceivableItem.PrefixTyniDescription,
                        PolicyId = premiumReceivableItem.PolicyId,
                        ImputationId = premiumReceivableItem.ImputationId,
                        PayerDocumentNumber = premiumReceivableItem.PayerDocumentNumber,
                        PayerName = premiumReceivableItem.PayerName,
                        PaymentNumber = premiumReceivableItem.PaymentNumber,
                        PaymentExpirationDate = Convert.ToDateTime(premiumReceivableItem.PaymentExpirationDate).ToShortDateString(),
                        EndorsementId = premiumReceivableItem.EndorsementId,
                        EndorsementDocumentNumber = premiumReceivableItem.EndorsementDocumentNumber,
                        InsuredId = premiumReceivableItem.InsuredId,
                        PayerId = premiumReceivableItem.PayerId,
                        PaymentAmount = premiumReceivableItem.Amount,
                        Amount = premiumReceivableItem.Amount,
                        ExcessPayment = premiumReceivableItem.ExcessPayment,
                        PendingCommission = premiumReceivableItem.PendingCommission,
                        ExchangeRate = premiumReceivableItem.ExchangeRate,
                        PayableAmount = premiumReceivableItem.Amount,
                        PolicyAgentDocumentNumberName = premiumReceivableItem.PolicyAgentDocumentNumberName,
                        PolicyAgentId = premiumReceivableItem.PolicyAgentId,
                        PolicyAgentDocumentNumber = premiumReceivableItem.PolicyAgentDocumentNumber,
                        PolicyAgentName = premiumReceivableItem.PolicyAgentName,
                        EndorsementTypeId = premiumReceivableItem.EndorsementTypeId,
                        EndorsementTypeDescription = premiumReceivableItem.EndorsementTypeDescription,
                        Address = premiumReceivableItem.Address,
                        Upd = premiumReceivableItem.Upd,
                        // Aplicación de Preliquidación
                        PolicyNumber = premiumReceivableItem.PolicyDocumentNumber,
                        EndorsementNumber = premiumReceivableItem.EndorsementDocumentNumber,
                        BranchName = premiumReceivableItem.BranchDescription,
                        QuotaNumber = premiumReceivableItem.PaymentNumber,
                        QuotaValue = premiumReceivableItem.PaymentAmount,
                        ValueToCollect = premiumReceivableItem.PayableAmount,
                        DiscountedCommissions = discountedCommissions,
                        IsReversion = premiumReceivableItem.IsReversion
                    });
                }

                return Json(premiumReceivableItemResponse, JsonRequestBehavior.AllowGet);

            }
            catch (UnhandledException)
            {
                return Json(premiumReceivableItemResponse, JsonRequestBehavior.AllowGet);
            }

        }

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRealTempPremiumReceivableItemByTempImputationId(int tempImputationId)
        {
            List<object> premiumReceivableItemResponse = new List<object>();

            try
            {

                List<SCRDTO.PremiumReceivableItemDTO> premiumReceivableItems =
                DelegateService.accountingApplicationService.GetTempApplicationPremiumByApplicationId(tempImputationId).OrderBy(o => o.PaymentNumber).ToList();

                List<SCRDTO.PremiumReceivableItemDTO> premiumReceivableItemResults = new List<SCRDTO.PremiumReceivableItemDTO>();
                foreach (SCRDTO.PremiumReceivableItemDTO premiumReceivableItemDto in premiumReceivableItems)
                {
                    premiumReceivableItemResults.Add(premiumReceivableItemDto);
                }

                //************************************************************************************************
                //ORDENAMIENTO POR NUMERO DE CUOTAS
                //DDV
                var premiumReceivableItemResultOrderByQuotas = from premiums in premiumReceivableItemResults
                                                               orderby premiums.BranchId, premiums.PrefixId,
                                                                   premiums.PolicyDocumentNumber, premiums.EndorsementDocumentNumber, premiums.PaymentNumber
                                                               select premiums;

                foreach (SCRDTO.PremiumReceivableItemDTO premiumReceivableItem in premiumReceivableItemResultOrderByQuotas)
                {
                    decimal balance = premiumReceivableItem.PaymentAmount - premiumReceivableItem.PaidAmount;

                    if (balance < 0) { balance = 0; }

                    premiumReceivableItemResponse.Add(new
                    {
                        InsuredName = premiumReceivableItem.InsuredName,
                        PremiumReceivableItemId = premiumReceivableItem.PremiumReceivableItemId,
                        InsuredDocumentNumberName = premiumReceivableItem.InsuredDocumentNumberName,
                        PayerDocumentNumberName = premiumReceivableItem.PayerDocumentNumberName,
                        BranchPrefixPolicyEndorsement = premiumReceivableItem.BranchPrefixPolicyEndorsement,
                        BranchDescription = premiumReceivableItem.BranchDescription,
                        BranchId = premiumReceivableItem.BranchId,
                        BussinessTypeDescription = premiumReceivableItem.BussinessTypeDescription,
                        BussinessTypeId = premiumReceivableItem.BussinessTypeId,
                        CurrencyDescription = premiumReceivableItem.CurrencyDescription,
                        CurrencyId = premiumReceivableItem.CurrencyId,
                        DiscountedCommission = premiumReceivableItem.DiscountedCommission,
                        InsuredDocumentNumber = premiumReceivableItem.InsuredDocumentNumber,
                        PolicyDocumentNumber = premiumReceivableItem.PolicyDocumentNumber,
                        PrefixId = premiumReceivableItem.PrefixId,
                        PrefixDescription = premiumReceivableItem.PrefixDescription,
                        PolicyId = premiumReceivableItem.PolicyId,
                        ImputationId = premiumReceivableItem.ImputationId,
                        PayerDocumentNumber = premiumReceivableItem.PayerDocumentNumber,
                        PayerName = premiumReceivableItem.PayerName,
                        PaymentNumber = premiumReceivableItem.PaymentNumber,
                        PaymentExpirationDate = Convert.ToDateTime(premiumReceivableItem.PaymentExpirationDate).ToShortDateString(),
                        EndorsementId = premiumReceivableItem.EndorsementId,
                        EndorsementDocumentNumber = premiumReceivableItem.EndorsementDocumentNumber,
                        InsuredId = premiumReceivableItem.InsuredId,
                        PayerId = premiumReceivableItem.PayerId,
                        PaymentAmount = balance,
                        ExcessPayment = premiumReceivableItem.ExcessPayment,
                        PendingCommission = premiumReceivableItem.PendingCommission,
                        ExchangeRate = premiumReceivableItem.ExchangeRate,
                        PayableAmount = premiumReceivableItem.PaymentAmount,
                        PolicyAgentDocumentNumberName = premiumReceivableItem.PolicyAgentDocumentNumberName,
                        PolicyAgentId = premiumReceivableItem.PolicyAgentId,
                        PolicyAgentDocumentNumber = premiumReceivableItem.PolicyAgentDocumentNumber,
                        PolicyAgentName = premiumReceivableItem.PolicyAgentName,
                        EndorsementTypeId = premiumReceivableItem.EndorsementTypeId,
                        EndorsementTypeDescription = premiumReceivableItem.EndorsementTypeDescription,
                        Address = premiumReceivableItem.Address,
                        Upd = premiumReceivableItem.Upd,
                        // Aplicación de Preliquidación
                        PolicyNumber = premiumReceivableItem.PolicyDocumentNumber,
                        EndorsementNumber = premiumReceivableItem.EndorsementDocumentNumber,
                        BranchName = premiumReceivableItem.BranchDescription,
                        QuotaNumber = premiumReceivableItem.PaymentNumber,
                        QuotaValue = premiumReceivableItem.PaymentAmount,
                        ValueToCollect = premiumReceivableItem.PayableAmount
                    });
                }

                return Json(premiumReceivableItemResponse, JsonRequestBehavior.AllowGet);

            }
            catch (UnhandledException)
            {
                return Json(premiumReceivableItemResponse, JsonRequestBehavior.AllowGet);
            }

        }

        ///<summary>
        /// SaveTempPremiumReceivableRequest
        /// </summary>
        /// <param name="premiumReceivable"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempPremiumReceivableRequest(PremiumReceivableModel premiumReceivable)
        {
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            int saveTempPremiumReceivableResponse = 0;


            PremiumReceivableTransactionDTO premiumReceivableTransaction = new PremiumReceivableTransactionDTO();
            premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();

            if (premiumReceivable.PremiumReceivableItems != null)
            {
                int premiumReceivableItemsCount = premiumReceivable.PremiumReceivableItems.Count;
                int userId = SessionHelper.GetUserId();
                int premiumReceivableItemsRow = 0;

                for (int i = 0; i < premiumReceivable.PremiumReceivableItems.Count; i++)
                {
                    if (!ExistPremiumReceivable(premiumReceivable.ImputationId, premiumReceivable.PremiumReceivableItems[i].EndorsementId, premiumReceivable.PremiumReceivableItems[i].PaymentNum, premiumReceivable.PremiumReceivableItems[i].PayerId))
                    {
                        if (premiumReceivable.PremiumReceivableItems[i].PremiumReceivableItemId == 0)
                        {
                            PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem = new PremiumReceivableTransactionItemDTO();

                            premiumReceivableTransactionItem.DeductCommission = new ACCDTO.AmountDTO()
                            {
                                Value = (-1 * premiumReceivable.PremiumReceivableItems[i].DiscountedCommisson) //las comisiones descontadas van al débito
                            };

                            premiumReceivableTransactionItem.Policy = new ACCDTO.PolicyDTO();
                            premiumReceivableTransactionItem.Policy.Id = premiumReceivable.PremiumReceivableItems[i].PolicyId;
                            premiumReceivableTransactionItem.Policy.Endorsement = new ACCDTO.EndorsementDTO()
                            {
                                Id = premiumReceivable.PremiumReceivableItems[i].EndorsementId
                            };
                            premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<ACCDTO.BeneficiaryDTO>()
                            {
                                new ACCDTO.BeneficiaryDTO()
                                {
                                    CustomerType = Convert.ToInt32(CustomerType.Individual),
                                    IndividualId = premiumReceivable.PremiumReceivableItems[i].PayerId
                                },
                            };
                            premiumReceivableTransactionItem.Policy.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                            {
                                Currency = new SCRDTO.CurrencyDTO() { Id = premiumReceivable.PremiumReceivableItems[i].CurrencyCode }
                            };
                            premiumReceivableTransactionItem.Policy.PayerComponents = new List<ACCDTO.PayerComponentDTO>()
                            {
                                new ACCDTO.PayerComponentDTO()
                                {
                                    Amount = premiumReceivable.PremiumReceivableItems[i].Amount,
                                    BaseAmount = premiumReceivable.PremiumReceivableItems[i].LocalAmount,
                                    Id = premiumReceivable.IsDiscountedCommisson ? premiumReceivable.PremiumReceivableItems[i].PremiumReceivableItemId : 0,
                                }
                            };
                            premiumReceivableTransactionItem.Policy.PaymentPlan = new ACCDTO.PaymentPlanDTO()
                            {
                                Quotas = new List<ACCDTO.QuotaDTO>()
                                {
                                    new ACCDTO.QuotaDTO() { Number = premiumReceivable.PremiumReceivableItems[i].PaymentNum }
                                }
                            };

                            premiumReceivableTransactionItem.Id = premiumReceivable.PremiumReceivableItems[i].PremiumReceivableItemId;

                            premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);
                        }
                        else
                        {
                            PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem = new PremiumReceivableTransactionItemDTO();

                            premiumReceivableTransactionItem.DeductCommission = new ACCDTO.AmountDTO()
                            {
                                Value = (-1 * premiumReceivable.PremiumReceivableItems[i].DiscountedCommisson)
                            };
                            premiumReceivableTransactionItem.Policy = new ACCDTO.PolicyDTO();
                            premiumReceivableTransactionItem.Policy.Id = premiumReceivable.PremiumReceivableItems[i].PolicyId;

                            premiumReceivableTransactionItem.Policy.Endorsement = new ACCDTO.EndorsementDTO() { Id = -1 };
                            premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<ACCDTO.BeneficiaryDTO>()
                            {
                                new ACCDTO.BeneficiaryDTO() {
                                    CustomerType = Convert.ToInt32(CustomerType.Individual),
                                    IndividualId = premiumReceivable.PremiumReceivableItems[i].PayerId
                                }
                            };
                            premiumReceivableTransactionItem.Policy.Endorsement = new ACCDTO.EndorsementDTO() { Id = premiumReceivable.PremiumReceivableItems[i].EndorsementId };
                            premiumReceivableTransactionItem.Policy.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                            {
                                Currency = new SCRDTO.CurrencyDTO() { Id = premiumReceivable.PremiumReceivableItems[i].CurrencyCode }
                            };
                            premiumReceivableTransactionItem.Policy.PayerComponents = new List<ACCDTO.PayerComponentDTO>()
                            {
                                new ACCDTO.PayerComponentDTO()
                                {
                                    Amount = premiumReceivable.PremiumReceivableItems[i].PaymentAmount,
                                    BaseAmount = premiumReceivable.PremiumReceivableItems[i].LocalAmount,
                                    Id = premiumReceivable.IsDiscountedCommisson ? premiumReceivable.PremiumReceivableItems[i].PremiumReceivableItemId : 0,
                                }
                            };
                            premiumReceivableTransactionItem.Policy.PaymentPlan = new ACCDTO.PaymentPlanDTO()
                            {
                                Quotas = new List<ACCDTO.QuotaDTO>()
                            {
                                new ACCDTO.QuotaDTO() { Number = premiumReceivable.PremiumReceivableItems[i].PaymentNum }
                            }
                            };
                            premiumReceivableTransactionItem.Id = premiumReceivable.PremiumReceivableItems[i].PremiumReceivableItemId;
                            premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);
                        }

                        if (premiumReceivableItemsRow == 9 && premiumReceivableItemsCount > 10)
                        {
                            saveTempPremiumReceivableResponse = DelegateService.accountingApplicationService.SaveTempPremiumRecievableTransaction(premiumReceivableTransaction,
                                                premiumReceivable.ImputationId, premiumReceivable.PremiumReceivableItems[0].ExchangeRate,
                                                userId, DateTime.Now, accountingDate);
                            premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();
                            premiumReceivableItemsRow = -1;
                        }
                        premiumReceivableItemsRow++;
                    }
                }

                saveTempPremiumReceivableResponse = DelegateService.accountingApplicationService.SaveTempPremiumRecievableTransaction(premiumReceivableTransaction,
                                premiumReceivable.ImputationId, premiumReceivable.PremiumReceivableItems[0].ExchangeRate,
                                userId, DateTime.Now, accountingDate);
            }

            return Json(saveTempPremiumReceivableResponse, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// SaveTempPremiumReceivableRequest
        /// </summary>
        /// <param name="premiumReceivable"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempApplicationPremiumComponents(PremiumReceivableItemModel premiumReceivableItemModel, List<TempApplicationPremiumCommissDTO> commissionDiscountedModel, int tempApplicationPremiumId)
        {
            try
            {
                int userId = SessionHelper.GetUserId();
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

                TempApplicationPremiumDTO tempApplicationPremiumDTO = new TempApplicationPremiumDTO()
                {
                    Amount = premiumReceivableItemModel.PaymentAmount,
                    LocalAmount = premiumReceivableItemModel.PaymentLocalAmount,
                    ExchangeRate = premiumReceivableItemModel.ExchangeRate,
                    Currencyid = premiumReceivableItemModel.CurrencyCode,
                    ApplicationId = tempApplicationPremiumId,
                    userId = userId,
                    RegisterDate = DateTime.Now,
                    AccountingDate = accountingDate,
                    Tax = premiumReceivableItemModel.Tax,
                    EndorsementId = premiumReceivableItemModel.EndorsementId,
                    PaymentNumber = premiumReceivableItemModel.PaymentNum,
                    PayerId = premiumReceivableItemModel.PayerId,
                    NoExpenses = premiumReceivableItemModel.NoExpenses,
                    Commissions = commissionDiscountedModel,
                };
                var response = DelegateService.accountingApplicationService.SaveTempApplicationPremiumComponents(tempApplicationPremiumDTO);

                return Json(new { success = true, result = response }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { success = false, result = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        ///<summary>
        /// DeleteTempPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="tempImputationCode"></param>
        /// <param name="tempPremiumReceivableCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempPremiumRecievableTransactionItem(int tempImputationCode, int tempPremiumReceivableCode, bool isReversion = false)
        {
            bool isDeletedTempPremiumRecievableTransactionItem = DelegateService.accountingApplicationService.DeleteTempPremiumRecievableTransactionItem(tempImputationCode, tempPremiumReceivableCode, isReversion);

            return Json(isDeletedTempPremiumRecievableTransactionItem, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearhDiscountedCommission(string policyId, string endorsementId)
        {
            List<SCRDTO.DiscountedCommissionDTO> discountedCommissionDTO = DelegateService.accountingApplicationService.SearhDiscountedCommission(policyId, endorsementId);
            return Json(discountedCommissionDTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearhTempDiscountedCommission(string endorsementId, string policyId, string tempApplicationId)
        {
            List<SCRDTO.DiscountedCommissionDTO> discountedCommissionDTO = DelegateService.accountingApplicationService.GetTempApplicationPremiumCommissByTempAppId(policyId, endorsementId, Convert.ToInt32(tempApplicationId));
            return Json(discountedCommissionDTO, JsonRequestBehavior.AllowGet);
        }
        #endregion PremiumReceivableSearch

        #region ExcessPayment

        ///<summary>
        /// SaveTempDepositPrime
        /// </summary>
        /// <param name="usedDepositPremiumModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempDepositPrime(UsedDepositPremiumModel usedDepositPremiumModel)
        {
            List<SCRDTO.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = new List<SCRDTO.TempUsedDepositPremiumDTO>();

            if (usedDepositPremiumModel.UsedAmounts != null)
            {
                for (int i = 0; i < usedDepositPremiumModel.UsedAmounts.Count; i++)
                {
                    SCRDTO.TempUsedDepositPremiumDTO tempUsedDepositPremium = new SCRDTO.TempUsedDepositPremiumDTO();

                    tempUsedDepositPremium.Id = usedDepositPremiumModel.UsedAmounts[i].UsedDepositPremiumId;
                    tempUsedDepositPremium.DepositPremiumTransactionId =
                        usedDepositPremiumModel.UsedAmounts[i].DepositPremiumTrasactionId;
                    tempUsedDepositPremium.TempPremiumReceivableItemId =
                        usedDepositPremiumModel.PremiumReceivableItemId;
                    tempUsedDepositPremium.Amount = usedDepositPremiumModel.UsedAmounts[i].Amount;

                    tempUsedDepositPremiums.Add(tempUsedDepositPremium);
                }
            }
            return Json(DelegateService.accountingApplicationService.SaveTempUsedDepositPremiumRequest(tempUsedDepositPremiums), JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// GetDepositPremiumTransactionByPayerId
        /// </summary>
        /// <param name="payerId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDepositPremiumTransactionByPayerId(string payerId)
        {
            List<object> depositPremiumTransactions = new List<object>();
            List<SCRDTO.DepositPremiumTransactionDTO> depositPremiumTransactionsDTOs = DelegateService.accountingApplicationService.GetDepositPremiumTransactionByPayerId(Convert.ToInt32(payerId));

            foreach (SCRDTO.DepositPremiumTransactionDTO depositPremiumTransaction in depositPremiumTransactionsDTOs)
            {
                depositPremiumTransactions.Add(new
                {
                    DepositPremiumTransactionId = depositPremiumTransaction.DepositPremiumTransactionId,
                    BillId = depositPremiumTransaction.CollectId,
                    PayerId = depositPremiumTransaction.PayerId,
                    RegisterDate = Convert.ToDateTime(depositPremiumTransaction.RegisterDate).ToShortDateString(),
                    CurrencyId = depositPremiumTransaction.CurrencyId,
                    CurrencyDescription = depositPremiumTransaction.CurrencyDescription,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", depositPremiumTransaction.Amount),
                    TempAmount = String.Format(new CultureInfo("en-US"), "{0:C}", depositPremiumTransaction.TempAmount),
                    Used = String.Format(new CultureInfo("en-US"), "{0:C}", depositPremiumTransaction.Used),
                    TotalAmount = depositPremiumTransaction.TotalAmount < 0 ? "$" + depositPremiumTransaction.TotalAmount : String.Format(new CultureInfo("en-US"), "{0:C}", depositPremiumTransaction.TotalAmount),
                    UsedAmount = String.Format(new CultureInfo("en-US"), "{0:C}", depositPremiumTransaction.UsedAmount),
                });
            }

            return new UifTableResult(depositPremiumTransactions);
        }

        /// <summary>
        /// DeleteTempUsedDepositPremiumRequest
        /// Método para eliminar las primas en depósito en temporales.
        /// </summary>
        /// <param name="tempPremiumReceivableId"></param>
        public void DeleteTempUsedDepositPremiumRequest(int tempPremiumReceivableId)
        {
            DelegateService.accountingApplicationService.DeleteTempUsedDepositPremiumRequest(tempPremiumReceivableId);
        }

        #endregion

        #region BillingGroup

        /// <summary>
        /// GetBillingGroupByNumber
        /// Obtiene una lista de grupos al que pertenecen las pólizas
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBillingGroupByNumber(string query)
        {
            int typeSearch = 1;

            return Json(_commonController.GetBillingGroup(query, typeSearch), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GetBillingGroupByName
        /// Obtiene una lista de grupos al que pertenecen las pólizas
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBillingGroupByName(string query)
        {

            int typeSearch = 2;

            return Json(_commonController.GetBillingGroup(query, typeSearch), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region PremiumReceivableValidation


        public JsonResult GetTemporalApplicationByEndorsementIdPaymentNumber(int tempApplicationId, int endorsementId, int paymentNum)
        {
            try
            {
                ApplicationDTO tempApplication = DelegateService.accountingApplicationService.
                    GetTempApplicationByEndorsementIdQuotaNumber(tempApplicationId, endorsementId, paymentNum);

                return Json(new { success = true, result = tempApplication });
            }
            catch (BusinessException ex)
            {
                return Json(new { success = false, result = ex.Message });
            }
        }

        public bool ExistPremiumReceivable(int temporalId, int endorsementId, int paymentNum, int payerIndividualId)
        {
            ApplicationDTO application = DelegateService.accountingApplicationService.GetTempApplicationByEndorsementIdQuotaNumber(temporalId, endorsementId, paymentNum);

            if (application.Id > 0)
                return true;

            return false;
        }

        #endregion

        #region ValidatePolicyComponents

        /// <summary>
        /// ValidatePolicyComponents
        /// Valida que la póliza tenga componentes.
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidatePolicyComponents(int policyId, int endorsementId)
        {
            return Json(DelegateService.accountingApplicationService.ValidatePolicyComponents(policyId, endorsementId), JsonRequestBehavior.AllowGet);
        }

        #endregion ValidatePolicyComponents


        /// <summary>
        /// GetEchangeRateByCollect
        /// Obtiene la tasa de cambio de los ingresos del recibo
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="accountingDate"></param>
        /// <param name="applicationCollectId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEchangeRateByCollect(int currencyCode, string accountingDate, int applicationCollectId)
        {
            decimal echangeRate = 0;

            try
            {
                List<PaymentDTO> payments = DelegateService.accountingApplicationService.GetPaymentsByCollectId(applicationCollectId);
                foreach (PaymentDTO payment in payments)
                {
                    if (payment.Amount.Currency.Id == currencyCode)
                    {
                        echangeRate = payment.ExchangeRate.BuyAmount;//Tasa de Cambio del Recibo
                        break;
                    }
                }

                /*si no existen Ingresos de caja con la moneda de la póliza se busca en la tabla de Tasa de cambio*/
                if (echangeRate == 0)
                {
                    echangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(Convert.ToDateTime(accountingDate), currencyCode).SellAmount;
                }
                return Json(echangeRate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        /// <summary>
        /// GetEchangeRateByCollect
        /// Obtiene la tasa de cambio de los ingresos del recibo
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <param name="accountingDate"></param>
        /// <param name="applicationCollectId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEchangeRateByCurrencyId(int currencyCode)
        {
            decimal echangeRate = 0;

            try
            {
                /*si no existen Ingresos de caja con la moneda de la póliza se busca en la tabla de Tasa de cambio*/
                echangeRate = DelegateService.commonService.GetExchangeRateByCurrencyId(currencyCode).SellAmount;
                return Json(echangeRate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        public JsonResult GetTempApplicationPremiumComponentsByTemApp(int tempApp)
        {

            try
            {
                /*si no existen Ingresos de caja con la moneda de la póliza se busca en la tabla de Tasa de cambio*/
                var components = DelegateService.accountingApplicationService.GetTempApplicationPremiumComponentsByTemApp(tempApp);
                return Json(components, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        public JsonResult UpdTempApplicationPremiumComponents(UpdTempApplicationPremiumComponentDTO updTempApplicationPremiumComponentDTO)
        {
            try
            {
                var components = DelegateService.accountingApplicationService.UpdTempApplicationPremiumComponents(updTempApplicationPremiumComponentDTO);
                return Json(components, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        public JsonResult UpdTempApplicationPremiumCommission(TempApplicationPremiumCommissDTO applicationPremiumCommisionDTO)
        {
            try
            {
                var components = DelegateService.accountingApplicationService.UpdateTempApplicationPremiumCommisses(applicationPremiumCommisionDTO);
                return Json(components, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(exception.Message, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// GenerateCollectItemsExcel
        /// Genera y Descarga en formato Excel las primas temporales 
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GeneratePolicyPaymentsExcel(int tempApplicationId)
        {

            List<PremiumSearchPolicyDTO> premiumReceivableSearchPolicies =
                              DelegateService.accountingApplicationService.GetTempApplicationsPremiumByTempApplicationId(tempApplicationId);
            MemoryStream memoryStream = new MemoryStream();

            if (premiumReceivableSearchPolicies != null && premiumReceivableSearchPolicies.Count > 0)
            {

                try
                {
                    memoryStream = ExportPolicyPayments(premiumReceivableSearchPolicies);
                    return Json(DelegateService.commonService.GetKeyApplication("TransferProtocol") + UploadReport(memoryStream.ToArray(), "EstadoPagosPolizas.xls"), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(App_GlobalResources.Language.ErrorExportExcel, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                return Json(Global.NoRecordsFound, JsonRequestBehavior.DenyGet);
            }
        }


        private MemoryStream ExportPolicyPayments(List<PremiumSearchPolicyDTO> tempApplicationPremium)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();


            #region Font

            font.FontName = "Arial";
            font.FontHeightInPoints = 8;
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Black.Index;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Arial";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            //TITLE FONT
            var fontTitle = workbook.CreateFont();
            fontTitle.FontName = "Arial";
            fontTitle.FontHeightInPoints = 20;
            fontTitle.Boldweight = (short)FontBoldWeight.Bold;
            fontTitle.Color = HSSFColor.Black.Index;

            //HEADER FONT
            var fontHeader = workbook.CreateFont();
            fontHeader.FontName = "Arial";
            fontHeader.FontHeightInPoints = 12;
            fontHeader.Boldweight = (short)FontBoldWeight.Bold;
            fontHeader.Color = HSSFColor.Black.Index;

            var fontHeaderDescription = workbook.CreateFont();
            fontHeaderDescription.FontName = "Arial";
            fontHeaderDescription.FontHeightInPoints = 12;
            fontHeaderDescription.Color = HSSFColor.Black.Index;

            #endregion

            #region style

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;
            styleHeader.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.RightBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.TopBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.BorderBottom = BorderStyle.Double;
            styleHeader.BorderLeft = BorderStyle.Double;
            styleHeader.BorderRight = BorderStyle.Double;
            styleHeader.BorderTop = BorderStyle.Double;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.RightBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.TopBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.BorderBottom = BorderStyle.Double;
            styleDetail.BorderLeft = BorderStyle.Double;
            styleDetail.BorderRight = BorderStyle.Double;
            styleDetail.BorderTop = BorderStyle.Double;

            ICellStyle Amountstyle = workbook.CreateCellStyle();
            Amountstyle.SetFont(fontDetail);
            Amountstyle.Alignment = HorizontalAlignment.Right;
            Amountstyle.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.RightBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.TopBorderColor = HSSFColor.Grey40Percent.Index;
            //Amountstyle.DataFormat = workbook.CreateDataFormat().GetFormat("$#,#0.00");
            Amountstyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.##");
            Amountstyle.BorderBottom = BorderStyle.Double;
            Amountstyle.BorderLeft = BorderStyle.Double;
            Amountstyle.BorderRight = BorderStyle.Double;
            Amountstyle.BorderTop = BorderStyle.Double;

            ICellStyle styleTitle = workbook.CreateCellStyle();
            styleTitle.SetFont(fontTitle);
            styleTitle.FillForegroundColor = HSSFColor.White.Index;
            styleTitle.FillPattern = FillPattern.SolidForeground;

            ICellStyle Headerstyle = workbook.CreateCellStyle();
            Headerstyle.SetFont(fontHeader);

            ICellStyle HeaderDetailstyle = workbook.CreateCellStyle();
            HeaderDetailstyle.SetFont(fontHeaderDescription);

            #endregion

            #region Header

            var titleRow = sheet.CreateRow(0);
            titleRow.CreateCell(0).SetCellValue(Global.AccountingPolicies);
            titleRow.GetCell(0).CellStyle = styleTitle;

            var sheetdate = sheet.CreateRow(2);
            sheetdate.CreateCell(0).SetCellValue(Global.Date.ToUpper());
            sheetdate.GetCell(0).CellStyle = Headerstyle;

            sheetdate.CreateCell(1).SetCellValue(DateTime.Now.ToShortDateString());
            sheetdate.GetCell(1).CellStyle = HeaderDetailstyle;


            #endregion

            #region Detail

            var headerRow = sheet.CreateRow(3);

            headerRow.CreateCell(0).SetCellValue(@Global.Branch.ToUpper());
            headerRow.CreateCell(1).SetCellValue(@Global.Prefix.ToUpper());
            headerRow.CreateCell(2).SetCellValue(@Global.Policy.ToUpper());
            headerRow.CreateCell(3).SetCellValue(@Global.Endorsement.ToUpper());
            headerRow.CreateCell(4).SetCellValue(@Global.QuotaNumber.ToUpper());
            headerRow.CreateCell(5).SetCellValue(@Global.Insured.ToUpper());
            headerRow.CreateCell(6).SetCellValue(@Global.Payer.ToUpper());
            headerRow.CreateCell(7).SetCellValue(@Global.Currency.ToUpper());//moneda
            headerRow.CreateCell(8).SetCellValue(@Global.ExchangeRate.ToUpper());//tasa de cambio
            headerRow.CreateCell(9).SetCellValue(@Global.QuotaValue.ToUpper());//valor cuota
            headerRow.CreateCell(10).SetCellValue(@Global.PayedQuota.ToUpper());//valor pagado
            headerRow.CreateCell(11).SetCellValue(@Global.DialogDepositPremiumsFeeBalance.ToUpper());//saldo por pagar
            headerRow.CreateCell(12).SetCellValue(@Global.Tax.ToUpper());//impuestos
            headerRow.CreateCell(13).SetCellValue(@Global.Commission.ToUpper());//comisiones
            
            for (int it = 0; it < 14; it++)
                sheet.SetColumnWidth(it, 20 * 256);

            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 4;
            int i;
            foreach (PremiumSearchPolicyDTO item in tempApplicationPremium)
            {
                var row = sheet.CreateRow(rowNumber++);
                i = 0;
                row.CreateCell(i).SetCellValue(item.BranchDescription);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.PrefixDescription);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.PolicyDocumentNumber);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.EndorsementDocumentNumber);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.PaymentNumber);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.InsuredDocumentNumberName);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.PayerDocumentNumberName);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.CurrencyDescription);//moneda
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(Convert.ToDouble(item.ExchangeRate));//tasa de cambio
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(Convert.ToDouble(item.Amount * item.ExchangeRate));//valor cuota
                row.GetCell(i).CellStyle = Amountstyle;
                i++;
                row.CreateCell(i).SetCellValue(Convert.ToDouble(item.PaidAmount * item.ExchangeRate));//valor Pagado
                row.GetCell(i).CellStyle = Amountstyle;
                i++;
                row.CreateCell(i).SetCellValue(Convert.ToDouble((item.Amount - item.PaidAmount) * item.ExchangeRate));//saldo a pagar
                row.GetCell(i).CellStyle = Amountstyle;
                i++;
                row.CreateCell(i).SetCellValue(Convert.ToDouble(item.Tax));//Iva
                row.GetCell(i).CellStyle = Amountstyle;
                i++;
                row.CreateCell(i).SetCellValue(Convert.ToDouble(item.DiscountedCommission));//Comsioness
                row.GetCell(i).CellStyle = Amountstyle;
            }

            #endregion

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }




        private string UploadReport(byte[] byteArray, string fileName)
        {
            string path = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + this.User.Identity.Name + @"\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += Guid.NewGuid() + Path.GetExtension(fileName);
                System.IO.File.WriteAllBytes(path, byteArray);

                return path;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                throw ex;
            }
        }

    }
}