using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.WrapperAccountingService.DTOs;
using Sistran.Core.Application.WrapperAccountingService.Enums;
using Sistran.Core.Application.WrapperAccountingServiceEEProvide;
using Sistran.Core.Application.WrapperAccountingServiceEEProvider.DAOs;
using Sistran.Core.Application.WrapperAccountingServiceEEProvider.Models;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Payment;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using ENUMACC = Sistran.Core.Application.WrapperAccountingService.Enums;
namespace Sistran.Company.Application.WrapperAccountingServiceEEProvider.Business
{
    public class CreateApplication
    {
        #region creacion modelos
        /// <summary>
        /// Creates the premium.
        /// </summary>
        /// <param name="requestApplication">The request application.</param>
        /// <param name="paymentDTO">The payment dto.</param>
        /// <returns></returns>
        internal static PremiumReceivableTransactionItemDTO CreatePremium(RequestApplication requestApplication, PaymenQuotaDTO paymentDTO)
        {
            PremiumReceivableTransactionItemDTO premiumReceivableTransactionItemDTO = new PremiumReceivableTransactionItemDTO
            {
                Policy = new PolicyDTO
                {
                    Endorsement = new EndorsementDTO
                    {
                        Id = paymentDTO.EndorsementId
                    },
                    DefaultBeneficiaries = new List<BeneficiaryDTO>()
                            {
                                new BeneficiaryDTO()
                                {
                                    CustomerType = Convert.ToInt32(CustomerType.Individual),
                                    IndividualId = paymentDTO.Id
                                }
                            },
                    PaymentPlan = new PaymentPlanDTO()
                    {
                        Quotas = new List<QuotaDTO>()
                                {
                                    new QuotaDTO() { Number = paymentDTO.Number }
                                },
                        Id = requestApplication.Id

                    },
                    ExchangeRate = new ExchangeRateDTO()
                    {
                        Currency = new CurrencyDTO() { Id = paymentDTO.CurrencyId  },
                        SellAmount = paymentDTO.ExchangeRate,
                        BuyAmount = paymentDTO.ExchangeRate
                    },
                    PayerComponents = new List<PayerComponentDTO>()
                            {
                                new PayerComponentDTO()
                                {
                                    Amount = requestApplication.Amount,
                                    BaseAmount = requestApplication.Amount,
                                    Id =  0
                                }
                            }
                }
            };
            return premiumReceivableTransactionItemDTO;
        }

        /// <summary>
        /// Creates the general.
        /// </summary>
        /// <param name="requestApplication">The request application.</param>
        /// <param name="payment">The payment.</param>
        /// <returns></returns>
        internal static CollectGeneralLedgerDTO CreateGeneralLedger(ApplicationRequest requestApplication, PersonDataDTO personRequestDTO)
        {
            CollectControlDTO collectControlResult = null;
            collectControlResult = DelegateService.accountingCollectControlService.NeedCloseCollect(requestApplication.UserId, requestApplication.BranchId, requestApplication.AccountingDate, Convert.ToInt32(CollectControlStatus.Open));
            //if (DelegateService.accountingCollectControlService.AllowOpenCollect(requestApplication.UserId, requestApplication.BranchId, requestApplication.AccountingDate, Convert.ToInt32(CollectControlStatus.OPEN));
            if (collectControlResult.Id == 0)
            {
                var collectControl = CreateCollectControl(new CollectControl { UserId = requestApplication.UserId, Branch = requestApplication.BranchId, accountingDate = requestApplication.AccountingDate });
                collectControlResult = DelegateService.accountingCollectControlService.SaveCollectControl(collectControl);
            }
           
            CollectGeneralLedgerDTO collectGeneralLedgerDTO = new CollectGeneralLedgerDTO();

            collectGeneralLedgerDTO.Bill = new BillDTO
            {
                BillControlId = collectControlResult.Id,//Id Control Caja
                BillId = 0,
                BillingConceptId = Convert.ToInt32(IncomeConceptType.PremiumCollection),
                Description = "",
                PayerDocumentNumber = requestApplication.VoucherRequest.DocumentNumber,
                PayerDocumentTypeId = requestApplication.VoucherRequest.DocumentType,
                PayerId = personRequestDTO?.IndividualId ?? 0,
                PayerName = personRequestDTO?.FullName ?? "",
                PayerTypeId = 0,
                PaymentsTotal = requestApplication.TotalAmount,
                RegisterDate = DateTime.Now,
                SourcePaymentId = 0,
                UserId = requestApplication.UserId,

            };
            collectGeneralLedgerDTO.Bill.PaymentSummary = new List<PaymentSummaryDTO>();

            var paymentDTO = new PaymentSummaryDTO()
            {
                Amount = requestApplication.VoucherRequest.ConsignmentCashRequest.Amount,
                BillId = 0,
                CurrencyId = requestApplication.VoucherRequest.ConsignmentCashRequest.Currency,
                ExchangeRate = requestApplication.VoucherRequest.ConsignmentCashRequest.ExchangeRate,
                LocalAmount = Decimal.Round(requestApplication.VoucherRequest.ConsignmentCashRequest.Amount * requestApplication.VoucherRequest.ConsignmentCashRequest.ExchangeRate, QuoteManager.PremiumRoundValue),
                PaymentId = requestApplication.Id,
                PaymentMethodId = Convert.ToInt32(PaymentMethods.DepositVoucher),
                ConsignmentChecks = new List<ConsignmentCheckDTO>()
            };
            collectGeneralLedgerDTO.Bill.PaymentSummary.Add(paymentDTO);
            collectGeneralLedgerDTO.CollectImputation = new CollectApplicationDTO { Id = 0 };
            collectGeneralLedgerDTO.BillControlId = collectControlResult.Id;
            collectGeneralLedgerDTO.PreliquidationBranch = 0;
            collectGeneralLedgerDTO.StatusId = Convert.ToInt32(CollectStatus.Active);
            collectGeneralLedgerDTO.UserId = requestApplication.UserId;
            return collectGeneralLedgerDTO;
        }

        internal static CollectApplicationDTO CreateCollect(ApplicationRequest requestApplication, PersonDataDTO personRequestDTO, string description = "")
        {
            CollectApplicationDTO collectImputation = new CollectApplicationDTO();

            CollectDTO collect = new CollectDTO();
            collect.Concept = new CollectConceptDTO { Id = Convert.ToInt32(CollectConcetpType.ConceptPremium) };
            collect.PaymentsTotal = new AmountDTO { Value = requestApplication.TotalAmount };
            collect.Payer = new PersonDTO()
            {
                IndividualId = personRequestDTO.IndividualId,

                IdentificationDocument = new IdentificationDocumentDTO()
                {
                    Number = requestApplication.VoucherRequest.DocumentNumber,
                    DocumentType = new DocumentTypeDTO { Id = requestApplication.VoucherRequest.DocumentType }
                },
                Name = personRequestDTO?.FullName,
                PersonType = new PersonTypeDTO() { Id = 0 }
            };
            collect.Description = description;
            collect.Status = Convert.ToInt16(CollectStatus.Active);
            collect.Number = ParametersDAO.GetBillNumber();
            collect.CollectType   = (int)CollectTypes.Incoming;
            collect.UserId = requestApplication.UserId;
            collect.AccountingCompany = new CompanyDTO()
            {
                IndividualId = -1 // Quemado por el momento.
            };
            collect.Branch = new BranchDTO { Id = requestApplication.BranchId };
            collect.Date = DateTime.Now;
            collect.Payments = CreatePayments(requestApplication.VoucherRequest, requestApplication.AccountingDate);
            collectImputation.Id = 0;
            collectImputation.Collect = collect;
            collectImputation.Application = new ApplicationDTO()
            {
                AccountingDate = requestApplication.AccountingDate,
                RegisterDate = DateTime.Now,
                ModuleId = Convert.ToInt32(ApplicationTypes.Collect),
                UserId = requestApplication.UserId,
                ApplicationItems = new List<TransactionTypeDTO>()
            };
            collect.Transaction = new TransactionDTO { Id = 0, TechnicalTransaction = requestApplication.Id };
            collectImputation.Transaction = new TransactionDTO { Id = 0 };
            return collectImputation;
        }

        /// <summary>
        /// Creacion de Pagos 
        /// </summary>
        /// <param name="voucherRequest">Comprobante Ingreso.</param>
        /// <param name="accountingDate">Fecha Contable.</param>
        /// <returns></returns>
        internal static List<PaymentDTO> CreatePayments(VoucherRequest voucherRequest, DateTime accountingDate)
        {
            // DateTime accountingDate = Convert.ToDateTime(DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingKey>(AccountingKey.ACC_MODULE_DATE_ACCOUNTING));  );
            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();
            if (voucherRequest.PaymentMethodId == Convert.ToInt32(ENUMACC.PaymentMethod.DepositVoucher))
            {
                paymentDTOs.Add(new DepositVoucherDTO()
                {
                    Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = voucherRequest.ConsignmentCashRequest.Currency },
                        Value = voucherRequest.ConsignmentCashRequest.Amount
                    },
                    Date = accountingDate,
                    DepositorName = voucherRequest.ConsignmentCashRequest.DepositorName,
                    Id = 0,
                    ExchangeRate = new ExchangeRateDTO { SellAmount = voucherRequest.ConsignmentCashRequest.ExchangeRate },
                    LocalAmount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = voucherRequest.ConsignmentCashRequest.Currency },
                        Value = Decimal.Round(voucherRequest.ConsignmentCashRequest.Amount * voucherRequest.ConsignmentCashRequest.ExchangeRate, QuoteManager.PremiumRoundValue)
                    },
                    PaymentMethod = new PaymentMethodDTO { Id = voucherRequest.PaymentMethodId },
                    ReceivingAccount = new BankAccountCompanyDTO()
                    {
                        Bank = new BankDTO() { Id = voucherRequest.ConsignmentCashRequest.BankId },
                        Number = voucherRequest.ConsignmentCashRequest.AccountNumber
                    },
                    VoucherNumber = voucherRequest.ConsignmentCashRequest.BallotNumber,
                    Status = Convert.ToInt16(PaymentStatus.Active)
                });
            }

            return paymentDTOs;
        }
        internal static TransactionTypeDTO CreateApplicationPremiums(PremiumReceivableTransactionDTO premiumReceivableTransactionDTO)
        {
            TransactionTypeDTO transactionTypeDTO = premiumReceivableTransactionDTO;
            return transactionTypeDTO;
        }
        public static CollectControlDTO CreateCollectControl(CollectControl collectControlModel)
        {
            CollectControlDTO collectControl = new CollectControlDTO();
            collectControl.Branch = new BranchDTO();
            collectControl.Branch.Id = collectControlModel.Branch;
            collectControl.UserId = collectControlModel.UserId;
            collectControl.AccountingDate = collectControlModel.accountingDate;
            collectControl.OpenDate = DateTime.Now;
            collectControl.Status = (int)CollectControlStatus.Open;
            return collectControl;
        }
        #endregion creacion modelos
        public void CreatAplication(int transactionNumber)
        {

        }
    }
}
