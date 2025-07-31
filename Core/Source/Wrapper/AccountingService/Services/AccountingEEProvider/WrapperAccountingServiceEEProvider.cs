using Sistran.Company.Application.WrapperAccountingServiceEEProvider.Business;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Application;
using Sistran.Core.Application.AccountingServices.DTOs.Filter;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.WrapperAccountingService;
using Sistran.Core.Application.WrapperAccountingService.DTOs;
using Sistran.Core.Application.WrapperAccountingService.Exception;
using Sistran.Core.Application.WrapperAccountingServiceEEProvide;
using Sistran.Core.Application.WrapperAccountingServiceEEProvider.DAOs;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Payment;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person;
using Sistran.Core.Integration.UndewritingIntegrationServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sistran.Core.Application.WrapperAccountingServiceEEProvider
{
    public class WrapperAccountingServiceEEProvider : IWrapperAccountingService
    {
        /// <summary>
        ///Grabar aplicacion de Primas
        /// </summary>
        /// <param name="applicationRequest">The application request.</param>
        /// <exception cref="FaultException{AccountingException}"></exception>
        /// <exception cref="AccountingException"></exception>       
        public void SaveApplication(ApplicationRequest applicationRequest)
        {
            try
            {
                if (applicationRequest != null
                    && applicationRequest.VoucherRequest != null
                    && applicationRequest.VoucherRequest.ConsignmentCashRequest != null
                    && !String.IsNullOrEmpty(applicationRequest.VoucherRequest.ConsignmentCashRequest.BallotNumber))
                {
                    var bNumber = applicationRequest.VoucherRequest.ConsignmentCashRequest.BallotNumber;
                    try
                    {
                        if (DelegateService.accountingApplicationService.ExistsPaymenTechnicalTransactioByBallotNumber(bNumber))
                            return;
                    } 
                    catch (Exception)
                    {
                        // Excepción al consultar
                    }
                }


                string character = Resources.Errors.PaymentJoinCharacter;
                string collectDescription = Resources.Errors.Payment;
                PremiumReceivableTransactionItemDTO premium;

                int AppId = ApplicationDAO.CreateTempApplication(applicationRequest.UserId, applicationRequest.AccountingDate);
                PremiumReceivableTransactionDTO premiumRecievableTransaction = new PremiumReceivableTransactionDTO();
                premiumRecievableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();
                foreach (RequestApplication requestApplication in applicationRequest.RequestApplication)
                {
                    PaymenQuotaDTO paymentDTO = DelegateService.undewritingIntegrationServices.GetPayment(new FilterBaseDTO { Id = requestApplication.Id });
                    var resultData = DelegateService.accountingApplicationService.ValidatePremiumTemporal(new PremiumFilterDTO { Id = paymentDTO.EndorsementId, IsReversion = false, Number = paymentDTO.Number, PayerId = paymentDTO.Id, PremiumId = -1 });
                    int quotaId = -1;
                    try
                    {
                        var policy = DelegateService.undewritingIntegrationServices.GetPaymentQuotaData(new FilterBaseDTO { Id = paymentDTO.EndorsementId });
                        if (policy != null)
                        {
                            quotaId = Convert.ToInt32(policy.PaymentNum);
                            collectDescription += String.Format(Resources.Errors.PolicyDescriptionFormat, policy.PrefixDescription, policy.DocumentNumber, policy.EndorsementDocumentNum)
                                + character;
                        }
                    }
                    catch (Exception ex)
                    {
                        quotaId = DelegateService.undewritingIntegrationServices.GetPaymentQuota(new FilterBaseDTO { Id = paymentDTO.EndorsementId });
                    }
                    paymentDTO.Number = quotaId;
                    paymentDTO.CurrencyId = requestApplication.Currency;
                    paymentDTO.ExchangeRate = requestApplication.ExchangeRate;
                    //if (quotaId == paymentDTO.Number && paymentDTO.StateQuota == (short)PaymentState.PENDING)
                    //{

                    //}
                    premium = CreateApplication.CreatePremium(requestApplication, paymentDTO);
                    premiumRecievableTransaction.PremiumReceivableItems.Add(premium);
                }
                if (collectDescription.LastIndexOf(character) >= 0)
                    collectDescription = collectDescription.Substring(0, collectDescription.LastIndexOf(character));
                long ballotNumber = DelegateService.accountingApplicationService.SavePaymentAuthorization(applicationRequest.VoucherRequest.ConsignmentCashRequest.BallotNumber);
                applicationRequest.VoucherRequest.ConsignmentCashRequest.BallotNumber = ballotNumber.ToString();
                DelegateService.accountingApplicationService.SaveTmpPremiumComponent(new PremiumRequestDTO { PremiumReceivableTransaction = premiumRecievableTransaction, ApplicationId = AppId, UserId = applicationRequest.UserId, RegisterDate = DateTime.Now, AccountingDate = applicationRequest.AccountingDate, ExchangeRate= applicationRequest.ExchangeRate });
                PersonDataDTO personRequestDTO = DelegateService.undewritingIntegrationServices.GetPersonByFilter(new PersonRequestDTO { DocumentNumber = applicationRequest.VoucherRequest.DocumentNumber, DocumentTypeId = applicationRequest.VoucherRequest.DocumentType });
                CollectGeneralLedgerDTO GeneralLedger = CreateApplication.CreateGeneralLedger(applicationRequest, personRequestDTO);
                GeneralLedger.CollectImputation = CreateApplication.CreateCollect(applicationRequest, personRequestDTO, collectDescription);
                GeneralLedger.CollectImputation.Application.Id = AppId;
                GeneralLedger.CollectImputation.Application.ApplicationItems.Add(premiumRecievableTransaction);
                MessageSuccessDTO messageSuccessDTO = DelegateService.accountingAccount.SaveCollectGeneralLedgerPayment(GeneralLedger);
                if (messageSuccessDTO != null)
                {
                    if (messageSuccessDTO.TechnicalTransaction > 0)
                    {
                        ballotNumber = DelegateService.accountingApplicationService.UpdatePaymenTechnicalTransactionforAuthorization(ballotNumber, messageSuccessDTO.TechnicalTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<AccountingException>(new AccountingException { Reason = ex.Message }, new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// Transactions the number.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{AccountingException}"></exception>
        /// <exception cref="AccountingException"></exception>
        public async Task<int> TransactionNumber(int branchId)
        {
            try
            {
                TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
                {
                    BranchId = branchId
                };
                TechnicalTransactionDTO technicalTransaction = await Task.Run(() => DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter));
                return technicalTransaction.Id;

            }
            catch (Exception ex)
            {
                throw new FaultException<AccountingException>(new AccountingException { Reason = ex.Message });
            }
        }

        public WrapperAccountingService.DTOs.BillReportDTO GetBillReportInformation(int technicalTransaction)
        {
            try
            {
                var billReportDTO = DelegateService.accountingPaymentService.GetBillReport(technicalTransaction);

                if (billReportDTO != null && billReportDTO.BranchName != "") {
                    return new WrapperAccountingService.DTOs.BillReportDTO()
                    {
                        BranchName = billReportDTO.BranchName,
                        ClientDocumentNumber = billReportDTO.ClientDocumentNumber,
                        ConceptDescription = billReportDTO.ConceptDescription,
                        CurrencyDescription = billReportDTO.CurrencyDescription,
                        Description = billReportDTO.Description,
                        PayerDocumentNumber = billReportDTO.PayerDocumentNumber,
                        PayerName = billReportDTO.PayerName,
                        PaymentDate = billReportDTO.PaymentDate,
                        PaymentMethod = billReportDTO.PaymentMethod,
                        TechnicalTransaction = billReportDTO.TechnicalTransaction,
                        Total = billReportDTO.Total,
                        TotalInLetters = billReportDTO.TotalInLetters
                    };
                }
                return new WrapperAccountingService.DTOs.BillReportDTO();
            }
            catch (Exception ex)
            {
                throw new FaultException<AccountingException>(new AccountingException { Reason = ex.Message });
            }
        }
    }
}
