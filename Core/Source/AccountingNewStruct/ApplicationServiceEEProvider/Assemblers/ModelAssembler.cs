//System
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Amortizations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CreditNotes;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCEN = Sistran.Core.Application.Accounting.Entities;
using CLMMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using PAYMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
//Core
using COMMMOD = Sistran.Core.Application.CommonService.Models;
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using COMMENT = Sistran.Core.Application.Common.Entities;
using UPENT = Sistran.Core.Application.UniquePersonV1.Entities;
using PARAMENT = Sistran.Core.Application.Parameters.Entities;
using PAYENT = Sistran.Core.Application.Claims.Entities;
using IMPMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCENUM = Sistran.Core.Application.AccountingServices.Enums;
using UNDMOD = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AccountingServices.Enums;
using UNDDTOs = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
//using Sistran.Core.Application.Accounting.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Assemblers
{
    internal class ModelAssembler
    {

        #region Collect

        /// <summary>
        /// CreateCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <returns>Collect</returns>
        public static Collect CreateCollect(ACCEN.Collect collect)
        {
            CollectConcept collectConcept = new CollectConcept() { Id = Convert.ToInt32(collect.CollectConceptCode) };
            COMMMOD.Amount amount = new COMMMOD.Amount() { Value = Convert.ToDecimal(collect.PaymentsTotal) };

            Person payer = new Person()
            {
                IdentificationDocument = new IdentificationDocument()
                {
                    DocumentType = new DocumentType() { Id = Convert.ToInt32(collect.DocumentTypeId) },
                    Number = collect.OtherPayerDocumentNumber,
                },
                IndividualId = Convert.ToInt32(collect.IndividualId),
                Name = collect.OtherPayerName,
                PersonType = new PersonType() { Id = Convert.ToInt32(collect.PersonTypeId) }
            };

            CollectTypes collectTypes = (CollectTypes)collect.CollectType;
            Company company = new Company() { IndividualId = Convert.ToInt32(collect.AccountingCompanyCode) };

            Transaction transaction = new Transaction()
            {
                Id = Convert.ToInt32(collect.TechnicalTransaction),
                TechnicalTransaction = Convert.ToInt32(collect.TechnicalTransaction)
            };

            return new Collect()
            {
                Id = collect.CollectId,
                IsTemporal = Convert.ToBoolean(collect.IsTemporal),
                Date = Convert.ToDateTime(collect.RegisterDate),
                Description = collect.Description,
                Comments = collect.Comments,
                Concept = collectConcept,
                PaymentsTotal = amount,
                Payer = payer,
                Status = Convert.ToInt32(collect.Status),
                UserId = Convert.ToInt32(collect.UserId),
                Number = Convert.ToInt32(collect.Number),
                CollectType = collectTypes,
                Transaction = transaction,
                AccountingCompany = company,
                CollectControlId = collect.CollectControlCode ?? 0
            };
        }

        /// <summary>
        /// CreateCollects
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<AccoutingModels.Collect/></returns>
        public static List<Collect> CreateCollects(BusinessCollection businessCollection)
        {
            List<Collect> collects = new List<Collect>();
            foreach (ACCEN.Collect collectEntity in businessCollection.OfType<ACCEN.Collect>())
            {
                collects.Add(CreateCollect(collectEntity));
            }
            return collects;
        }

        #endregion

        #region collectingConcept

        /// <summary>
        /// CreateCollectConcept
        /// </summary>
        /// <param name="collectConcept"></param>
        /// <returns>CollectConcept</returns>
        public static CollectConcept CreateCollectConcept(ACCEN.CollectConcept collectConcept)
        {
            return new CollectConcept()
            {
                Id = collectConcept.CollectConceptId,
                Description = collectConcept.Description
            };
        }

        /// <summary>
        /// CreateCollectConcepts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<AccoutingModels.CollectConcept/></returns>
        public static List<CollectConcept> CreateCollectConcepts(BusinessCollection businessCollection)
        {
            List<CollectConcept> collectConcepts = new List<CollectConcept>();
            foreach (ACCEN.CollectConcept collectConceptEntity in businessCollection.OfType<ACCEN.CollectConcept>())
            {
                collectConcepts.Add(CreateCollectConcept(collectConceptEntity));
            }
            return collectConcepts;
        }

        #endregion

        #region Payment

        /// <summary>
        /// CreatePayment
        /// </summary>
        /// <param name="entityPayment"></param>
        /// <returns>Payment</returns>
        public static PaymentsModels.Payment CreatePayment(ACCEN.Payment entityPayment)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(entityPayment.CurrencyCode) },
                Value = entityPayment.IncomeAmount == null ? 0 : Convert.ToDecimal(entityPayment.IncomeAmount)
            };

            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { BuyAmount = entityPayment.ExchangeRate == null ? 0 : Convert.ToDecimal(entityPayment.ExchangeRate) };

            COMMMOD.Amount localAmount = new COMMMOD.Amount() { Value = entityPayment.Amount == null ? 0 : Convert.ToDecimal(entityPayment.Amount) };

            PaymentsModels.PaymentMethod paymentMethod = new PaymentsModels.PaymentMethod()
            {
                Id = entityPayment.PaymentMethodTypeCode == null ? 0 : Convert.ToInt32(entityPayment.PaymentMethodTypeCode),
                Description = entityPayment.RetentionConceptCode == null ? "-1" : entityPayment.RetentionConceptCode.ToString()
            };
            //COMMMOD.Bank bank = new COMMMOD.Bank() { Id = payment.IssuingBankCode ?? 0 };

            switch (paymentMethod.Id)
            {
                case (int)PaymentMethods.PostdatedCheck:
                case (int)PaymentMethods.CurrentCheck:
                    return new PaymentsModels.Check()
                    {
                        Id = entityPayment.PaymentCode,
                        PaymentMethod = paymentMethod,
                        Amount = amount,
                        Tax = entityPayment.Taxes == null ? 0 : Convert.ToDecimal(entityPayment.Taxes),
                        Retention = entityPayment.Retentions == null ? 0 : Convert.ToDecimal(entityPayment.Retentions),
                        Status = Convert.ToInt32(entityPayment.Status),
                        ExchangeRate = exchangeRate,
                        LocalAmount = localAmount,
                        DocumentNumber = entityPayment.DocumentNumber,
                        IssuingBankCode = entityPayment.IssuingBankCode ?? 0,
                        DatePayment = entityPayment.DatePayment,
                        Date = Convert.ToDateTime(entityPayment.DatePayment),
                        Holder = entityPayment.Holder,
                        IssuingAccountNumber = entityPayment.IssuingAccountNumber,
                        Retentions = entityPayment.Retentions ?? 0,
                        CollectId = entityPayment.CollectCode ?? 0
                    };
                case (int)PaymentMethods.DepositVoucher:
                    return new PaymentsModels.DepositVoucher()
                    {
                        Id = entityPayment.PaymentCode,
                        PaymentMethod = paymentMethod,
                        Amount = amount,
                        Tax = entityPayment.Taxes == null ? 0 : Convert.ToDecimal(entityPayment.Taxes),
                        Retention = entityPayment.Retentions == null ? 0 : Convert.ToDecimal(entityPayment.Retentions),
                        Status = Convert.ToInt32(entityPayment.Status),
                        ExchangeRate = exchangeRate,
                        LocalAmount = localAmount,
                        DocumentNumber = entityPayment.DocumentNumber,
                        IssuingBankCode = entityPayment.IssuingBankCode ?? 0,
                        DatePayment = entityPayment.DatePayment,
                        Date = Convert.ToDateTime(entityPayment.DatePayment),
                        Holder = entityPayment.Holder,
                        IssuingAccountNumber = entityPayment.IssuingAccountNumber,
                        Retentions = entityPayment.Retentions ?? 0,
                        CollectId = entityPayment.CollectCode ?? 0,
                        ReceivingAccount = new BankAccountCompany()
                        {
                            Number = entityPayment.ReceivingAccountNumber,
                            Bank = new COMMMOD.Bank()
                            {
                                Id = entityPayment.ReceivingBankCode ?? 0
                            }
                        },
                        VoucherNumber = entityPayment.DocumentNumber,
                        DepositorName = entityPayment.Holder
                    };
                case (int)PaymentMethods.ConsignmentByCheck:
                    return new PaymentsModels.ConsignmentCheck()
                    {
                        Id = entityPayment.PaymentCode,
                        PaymentMethod = paymentMethod,
                        Amount = amount,
                        Tax = entityPayment.Taxes == null ? 0 : Convert.ToDecimal(entityPayment.Taxes),
                        Retention = entityPayment.Retentions == null ? 0 : Convert.ToDecimal(entityPayment.Retentions),
                        Status = Convert.ToInt32(entityPayment.Status),
                        ExchangeRate = exchangeRate,
                        LocalAmount = localAmount,
                        DocumentNumber = entityPayment.DocumentNumber,
                        IssuingBankCode = entityPayment.IssuingBankCode ?? 0,
                        DatePayment = entityPayment.DatePayment,
                        Date = Convert.ToDateTime(entityPayment.DatePayment),
                        Holder = entityPayment.Holder,
                        IssuingBank = new COMMMOD.Bank() {
                            Id = Convert.ToInt32(entityPayment.IssuingBankCode)
                        },
                        IssuingAccountNumber = entityPayment.IssuingAccountNumber,
                        Retentions = entityPayment.Retentions ?? 0,
                        CollectId = entityPayment.CollectCode ?? 0
                    };
                default:
                    return new PaymentsModels.Payment()
                    {
                        Id = entityPayment.PaymentCode,
                        PaymentMethod = paymentMethod,
                        Amount = amount,
                        Tax = entityPayment.Taxes == null ? 0 : Convert.ToDecimal(entityPayment.Taxes),
                        Retention = entityPayment.Retentions == null ? 0 : Convert.ToDecimal(entityPayment.Retentions),
                        Status = Convert.ToInt32(entityPayment.Status),
                        ExchangeRate = exchangeRate,
                        LocalAmount = localAmount,
                        DocumentNumber = entityPayment.DocumentNumber,
                        IssuingBankCode = entityPayment.IssuingBankCode ?? 0,
                        DatePayment = entityPayment.DatePayment,
                        Holder = entityPayment.Holder,
                        IssuingAccountNumber = entityPayment.IssuingAccountNumber,
                        Retentions = entityPayment.Retentions ?? 0,
                        CollectId = entityPayment.CollectCode ?? 0
                    };
            }
        }
        /// <summary>
        /// CreatePayment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>Payment</returns>
        public static PaymentsModels.Check CreateCheckPayment(PaymentsModels.Payment payment)
        {
            COMMMOD.Amount amount = payment.Amount;

            COMMMOD.ExchangeRate exchangeRate = payment.ExchangeRate;

            COMMMOD.Amount localAmount = payment.Amount;

            PaymentsModels.PaymentMethod paymentMethod = payment.PaymentMethod;
            COMMMOD.Bank bank = new COMMMOD.Bank() { Id = payment.IssuingBankCode == 0 ? 0 : Convert.ToInt32(payment.IssuingBankCode) };

            return new PaymentsModels.Check()
            {
                Id = payment.Id,
                PaymentMethod = paymentMethod,
                Amount = amount,
                Tax = payment.Taxes == null ? 0 : Convert.ToDecimal(payment.Taxes),
                Retention = payment.Retentions == 0 ? 0 : Convert.ToDecimal(payment.Retentions),
                Status = Convert.ToInt32(payment.Status),
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount,
                DocumentNumber = payment.DocumentNumber,
                IssuingBankCode = payment.IssuingBankCode,
                IssuingBank = bank,
                DatePayment = payment.DatePayment,
                Date = payment.DatePayment ?? DateTime.Now,
                Holder = payment.Holder,
                IssuingAccountNumber = payment.IssuingAccountNumber,
                Retentions = payment.Retentions,
            };
        }

        /// <summary>
        /// CreatePayments
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Payment/></returns>
        public static List<PaymentsModels.Payment> CreatePayments(BusinessCollection businessCollection)
        {
            List<PaymentsModels.Payment> payments = new List<PaymentsModels.Payment>();
            foreach (ACCEN.Payment paymentEntity in businessCollection.OfType<ACCEN.Payment>())
            {
                payments.Add(CreatePayment(paymentEntity));
            }
            return payments;
        }

        #endregion

        #region PaymentTax

        /// <summary>
        /// CreatePaymentTax
        /// </summary>
        /// <param name="paymentTax"></param>
        /// <returns>PaymentTax</returns>
        public static PaymentTax CreatePaymentTax(ACCEN.PaymentTax paymentTax)
        {
            TaxServices.Models.Tax tax = new TaxServices.Models.Tax();
            tax.Id = Convert.ToInt32(paymentTax.TaxCode);

            COMMMOD.Amount taxBase = new COMMMOD.Amount();
            taxBase.Value = Convert.ToDecimal(paymentTax.TaxBase);

            return new PaymentTax()
            {
                Rate = Convert.ToDecimal(paymentTax.TaxRate),
                Tax = tax,
                TaxBase = taxBase,
                Id = paymentTax.PaymentTaxCode
            };
        }

        /// <summary>
        /// CreatePaymentTaxes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentTax/></returns>
        public static List<PaymentTax> CreatePaymentTaxes(BusinessCollection businessCollection)
        {
            List<PaymentTax> paymentTaxes = new List<PaymentTax>();
            foreach (ACCEN.PaymentTax paymentTaxEntity in businessCollection.OfType<ACCEN.PaymentTax>())
            {
                paymentTaxes.Add(CreatePaymentTax(paymentTaxEntity));
            }
            return paymentTaxes;
        }

        #endregion

        #region PaymenBallot

        /// <summary>
        /// CreatePaymenBallot
        /// </summary>
        /// <param name="paymentBallot"></param>
        /// <returns>PaymenBallot</returns>
        public static PaymentBallot CreatePaymentBallot(ACCEN.PaymentBallot paymentBallot)
        {
            COMMMOD.Amount ballotAmount = new COMMMOD.Amount();
            ballotAmount.Value = Convert.ToDecimal(paymentBallot.BallotAmount);

            COMMMOD.Amount bankAmount = new COMMMOD.Amount();
            bankAmount.Value = Convert.ToDecimal(paymentBallot.BankAmount);

            COMMMOD.Bank bank = new COMMMOD.Bank();
            bank.Id = Convert.ToInt32(paymentBallot.BankCode);

            COMMMOD.Currency currency = new COMMMOD.Currency();
            currency.Id = Convert.ToInt32(paymentBallot.CurrencyCode);

            Transaction transaction = new Transaction()
            {
                TechnicalTransaction = Convert.ToInt32(paymentBallot.TechnicalTransaction)
            };

            return new PaymentBallot()
            {
                Id = paymentBallot.PaymentBallotCode,
                BallotNumber = paymentBallot.PaymentBallotNumber,
                Bank = bank,
                AccountNumber = paymentBallot.AccountNumber,
                Currency = currency,
                BankDate = Convert.ToDateTime(paymentBallot.BankDate),
                Amount = ballotAmount,
                BankAmount = bankAmount,
                Status = Convert.ToInt32(paymentBallot.Status),
                Transaction = transaction
            };
        }

        /// <summary>
        /// CreatePaymentBallots
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentBallot/></returns>
        public static List<PaymentBallot> CreatePaymentBallots(BusinessCollection businessCollection)
        {
            List<PaymentBallot> paymentBallots = new List<PaymentBallot>();
            foreach (ACCEN.PaymentBallot paymentBallotEntity in businessCollection.OfType<ACCEN.PaymentBallot>())
            {
                paymentBallots.Add(CreatePaymentBallot(paymentBallotEntity));
            }
            return paymentBallots;
        }

        #endregion

        #region PaymentTicket

        /// <summary>
        /// CreatePaymentTicket
        /// </summary>
        /// <param name="entityPaymentTicket"></param>
        /// <returns>PaymentTicket</returns>
        public static PaymentTicket CreatePaymentTicket(ACCEN.PaymentTicket entityPaymentTicket)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch();
            branch.Id = Convert.ToInt32(entityPaymentTicket.BranchCode);

            COMMMOD.Amount amount = new COMMMOD.Amount();
            amount.Value = entityPaymentTicket.Amount.GetValueOrDefault();

            COMMMOD.Amount cashAmount = new COMMMOD.Amount();
            cashAmount.Value = entityPaymentTicket.CashAmount.GetValueOrDefault();

            COMMMOD.Bank bank = new COMMMOD.Bank();
            bank.Id = Convert.ToInt32(entityPaymentTicket.BankCode);

            COMMMOD.Amount commission = new COMMMOD.Amount();
            commission.Value = entityPaymentTicket.CommissionAmount.GetValueOrDefault();

            return new PaymentTicket()
            {
                Id = entityPaymentTicket.PaymentTicketCode,
                Branch = branch,
                Amount = amount,
                CashAmount = cashAmount,
                Bank = bank,
                Status = Convert.ToInt32(entityPaymentTicket.Status),
                Commission = commission,
                PaymentMethod = Convert.ToInt32(entityPaymentTicket.PaymentMethodTypeCode)
            };
        }

        /// <summary>
        /// CreatePaymentTicket
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentTicket></returns>
        public static List<PaymentTicket> CreatePaymentTicket(BusinessCollection businessCollection)
        {
            List<PaymentTicket> paymentTickets = new List<PaymentTicket>();
            foreach (ACCEN.PaymentTicket paymentTicketEntity in businessCollection.OfType<ACCEN.PaymentTicket>())
            {
                paymentTickets.Add(CreatePaymentTicket(paymentTicketEntity));
            }
            return paymentTickets;
        }

        #endregion

        #region PaymentTicketItem

        /// <summary>
        /// CreatePaymentTicketItem
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static PaymentTicket CreatePaymentTicketItem(BusinessCollection businessCollection)
        {
            List<PaymentsModels.Payment> payments = new List<PaymentsModels.Payment>();
            foreach (ACCEN.PaymentTicketItem paymentTicketItem in businessCollection.OfType<ACCEN.PaymentTicketItem>())
            {
                payments.Add(new PaymentsModels.Payment()
                {
                    Id = Convert.ToInt32(paymentTicketItem.PaymentCode)
                });
            }

            return new PaymentTicket()
            {
                Payments = payments
            };
        }

        #endregion

        #region CollectControl

        /// <summary>
        /// CreateCollectControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public static CollectControl CreateCollectControl(ACCEN.CollectControl collectControl)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(collectControl.BranchCode) };

            return new CollectControl()
            {
                Id = collectControl.CollectControlId,
                UserId = Convert.ToInt32(collectControl.UserId),
                Branch = branch,
                Status = Convert.ToInt32(collectControl.Status),
                AccountingDate = Convert.ToDateTime(collectControl.AccountingDate),
                OpenDate = Convert.ToDateTime(collectControl.OpenDate),
                CloseDate = Convert.ToDateTime(collectControl.CloseDate)
            };
        }

        /// <summary>
        /// CreateBillControls
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CollectControl/></returns>
        public static List<CollectControl> CreateBillControls(BusinessCollection businessCollection)
        {
            List<CollectControl> collectControls = new List<CollectControl>();

            foreach (ACCEN.CollectControl collectControlEntity in businessCollection.OfType<ACCEN.CollectControl>())
            {
                collectControls.Add(CreateCollectControl(collectControlEntity));
            }
            return collectControls;
        }

        #endregion

        #region CollectControlPayment

        /// <summary>
        /// CreateCollectControlPayment
        /// </summary>
        /// <param name="collectControlPayment"></param>
        /// <returns>CollectControlPayment</returns>
        public static CollectControlPayment CreateCollectControlPayment(ACCEN.CollectControlPayment collectControlPayment)
        {
            PaymentsModels.PaymentMethod paymentMethod = new PaymentsModels.PaymentMethod();
            paymentMethod.Id = Convert.ToInt32(collectControlPayment.PaymentMethodCode.Value);

            COMMMOD.Amount paymentTotalAdmitted = new COMMMOD.Amount();
            paymentTotalAdmitted.Value = Convert.ToDecimal(collectControlPayment.PaymentTotalAdmitted.Value);

            COMMMOD.Amount paymentsTotalReceived = new COMMMOD.Amount();
            paymentsTotalReceived.Value = Convert.ToDecimal(collectControlPayment.PaymentTotalReceived.Value);

            COMMMOD.Amount paymentsTotalDifference = new COMMMOD.Amount();
            paymentsTotalDifference.Value = Convert.ToDecimal(collectControlPayment.PaymentTotalDifference.Value);

            return new CollectControlPayment()
            {
                Id = collectControlPayment.CollectControlPaymentId,
                PaymentMethod = paymentMethod,
                PaymentTotalAdmitted = paymentTotalAdmitted,
                PaymentsTotalReceived = paymentsTotalReceived,
                PaymentsTotalDifference = paymentsTotalDifference
            };
        }

        #endregion

        #region Rejection

        /// <summary>
        /// CreateRejection
        /// </summary>
        /// <param name="rejection"></param>
        /// <returns>Rejection</returns>
        public static Rejection CreateRejection(ACCEN.Rejection rejection)
        {
            return new Rejection()
            {
                Id = rejection.RejectionCode,
                Description = rejection.Description
            };
        }

        /// <summary>
        /// CreateRejections
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Rejection/></returns>
        public static List<Rejection> CreateRejections(BusinessCollection businessCollection)
        {
            List<Rejection> rejections = new List<Rejection>();
            foreach (ACCEN.Rejection rejectionEntity in businessCollection.OfType<ACCEN.Rejection>())
            {
                rejections.Add(CreateRejection(rejectionEntity));
            }
            return rejections;
        }

        #endregion

        #region RejectedPayment

        /// <summary>
        /// CreateRejectedPayment
        /// </summary>
        /// <param name="rejectedPayment"></param>
        /// <returns>RejectedPayment</returns>
        public static RejectedPayment CreateRejectedPayment(ACCEN.RejectedPayment rejectedPayment)
        {
            PaymentsModels.Payment payment = new PaymentsModels.Payment();
            payment.Id = (int)rejectedPayment.PaymentCode;

            Rejection rejection = new Rejection();
            rejection.Id = (int)rejectedPayment.RejectionCode;

            COMMMOD.Amount commission = new COMMMOD.Amount();
            commission.Value = (decimal)rejectedPayment.Comission;

            COMMMOD.Amount taxCommission = new COMMMOD.Amount();
            taxCommission.Value = (decimal)rejectedPayment.TaxComission;

            return new RejectedPayment()
            {
                Id = rejectedPayment.RejectedPaymentCode,
                Payment = payment,
                Rejection = rejection,
                Date = (DateTime)rejectedPayment.RejectionDate,
                Commission = commission,
                TaxCommission = taxCommission,
                Description = rejectedPayment.Description
            };
        }

        #endregion

        #region CouponStatus

        /// <summary>
        /// CreateCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        public static CouponStatus CreateCouponStatus(ACCEN.CouponStatus couponStatus)
        {
            ACCENUM.CouponStatusTypes couponStatusTypes = Convert.ToBoolean(couponStatus.Applied) ? ACCENUM.CouponStatusTypes.Applied : ACCENUM.CouponStatusTypes.Rejected;

            return new CouponStatus()
            {
                Id = couponStatus.CouponStatusId,
                Description = couponStatus.Description,
                CouponStatusType = couponStatusTypes,
                IsEnabled = Convert.ToBoolean(couponStatus.Enabled),
                RetriesNumber = Convert.ToInt32(couponStatus.NumberOfRetries),
                SmallDescription = couponStatus.CouponStatusBankCode,
            };
        }

        /// <summary>
        /// CreateCouponStatus
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>CouponStatus</returns>
        public static List<CouponStatus> CreateCouponStatus(BusinessCollection businessCollection)
        {
            List<CouponStatus> couponStatus = new List<CouponStatus>();
            foreach (ACCEN.CouponStatus couponStatusEntity in businessCollection.OfType<ACCEN.CouponStatus>())
            {
                couponStatus.Add(CreateCouponStatus(couponStatusEntity));
            }
            return couponStatus;
        }

        #endregion

        #region BankNetworkStatus

        /// <summary>
        /// CreateBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public static BankNetworkStatus CreateBankNetworkStatus(ACCEN.BankNetworkStatus bankNetworkStatus)
        {
            var acceptedCouponStatus = new List<CouponStatus>();
            acceptedCouponStatus.Add(new CouponStatus()
            {
                SmallDescription = bankNetworkStatus.AppliedDefault,
                Id = bankNetworkStatus.CouponStatusCode,
                CouponStatusType = ACCENUM.CouponStatusTypes.Applied
            });

            var rejectedCouponStatus = new List<CouponStatus>();
            rejectedCouponStatus.Add(new CouponStatus()
            {
                SmallDescription = bankNetworkStatus.RejectionDefault,
                Id = bankNetworkStatus.CouponStatusCode,
                CouponStatusType = ACCENUM.CouponStatusTypes.Rejected
            });

            return new BankNetworkStatus()
            {
                Id = Convert.ToInt32(bankNetworkStatus.BankNetworkCode),
                AcceptedCouponStatus = acceptedCouponStatus,
                RejectedCouponStatus = rejectedCouponStatus,
                BankNetwork = new BankNetwork() { Id = bankNetworkStatus.BankNetworkCode }
            };
        }

        /// <summary>
        /// CreateRejectionBanks
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BankNetworkStatus/></returns>
        public static List<BankNetworkStatus> CreateRejectionBanks(BusinessCollection businessCollection)
        {
            List<BankNetworkStatus> bankNetworkStatus = new List<BankNetworkStatus>();
            foreach (ACCEN.BankNetworkStatus bankNetworkStatusEntity in businessCollection.OfType<ACCEN.BankNetworkStatus>())
            {
                bankNetworkStatus.Add(CreateBankNetworkStatus(bankNetworkStatusEntity));
            }
            return bankNetworkStatus;
        }


        #endregion

        #region ActionType

        /// <summary>
        /// CreateActionType
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns>ActionType</returns>
        public static ActionType CreateActionType(ACCEN.ActionType actionType)
        {
            return new ActionType()
            {
                Id = actionType.ActionTypeCode,
                Description = actionType.Description
            };
        }

        /// <summary>
        /// CreateActionType
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ActionType></returns>
        public static List<ActionType> CreateActionTypes(BusinessCollection businessCollection)
        {
            List<ActionType> actionTypes = new List<ActionType>();
            foreach (ACCEN.ActionType actionTypeEntity in businessCollection.OfType<ACCEN.ActionType>())
            {
                actionTypes.Add(CreateActionType(actionTypeEntity));
            }
            return actionTypes;
        }

        #endregion

        #region AccountingCompany

        /// <summary>
        /// CreateAccountingCompany
        /// </summary>
        /// <param name="companyBusinessCollection"></param>
        /// <returns>List<Company/></returns>
        public static List<Company> CreateAccountingCompany(BusinessCollection companyBusinessCollection)
        {
            List<Company> companies = new List<Company>();

            foreach (ACCEN.Company company in companyBusinessCollection.OfType<ACCEN.Company>())
            {
                companies.Add(CreateAccountingCompany(company));
            }
            return companies;
        }

        /// <summary>
        /// CreateAccountingCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        /// 
        public static Company CreateAccountingCompany(ACCEN.Company company)
        {
            return new Company()
            {
                IndividualId = company.CompanyId,
                FullName = company.Description
            };
        }

        #endregion

        #region TempPremiumReceivableTransaction

        /// <summary>
        /// CreateTempPremiumReceivableTransaction
        /// </summary>
        /// <param name="tempPremiumReceivable"></param>
        /// <returns>PremiumReceivableTransaction</returns>
        public static PremiumReceivableTransaction CreateTempPremiumReceivableTransaction(ACCEN.TempPremiumReceivableTrans tempPremiumReceivable)
        {
            Policy policy = new Policy() { Id = Convert.ToInt32(tempPremiumReceivable.PolicyId) };

            List<PremiumReceivableTransactionItem> tempPremiumReceivableTransactionItems = new List<PremiumReceivableTransactionItem>();
            tempPremiumReceivableTransactionItems.Add(new PremiumReceivableTransactionItem()
            {
                Policy = policy
            });

            return new PremiumReceivableTransaction()
            {
                Id = tempPremiumReceivable.TempPremiumReceivableTransCode,
                PremiumReceivableItems = tempPremiumReceivableTransactionItems
            };
        }

        /// <summary>
        /// CreateTempPremiumReceivableTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PremiumReceivableTransaction/></returns>
        public static List<PremiumReceivableTransaction> CreateTempPremiumReceivableTransactions(BusinessCollection businessCollection)
        {
            List<PremiumReceivableTransaction> premiumReceivableTransactions = new List<PremiumReceivableTransaction>();

            foreach (ACCEN.TempPremiumReceivableTrans tempPremiumReceivableEntity in businessCollection.OfType<ACCEN.TempPremiumReceivableTrans>())
            {
                premiumReceivableTransactions.Add(CreateTempPremiumReceivableTransaction(tempPremiumReceivableEntity));
            }
            return premiumReceivableTransactions;
        }

        #endregion

        #region TempPremiumReceivableTransactionItem

        /// <summary>
        /// CreateTempPremiumReceivableTransactionItem
        /// </summary>
        /// <param name="tempPremiumReceivable"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public static PremiumReceivableTransactionItem CreateTempPremiumReceivableTransactionItem(ACCEN.TempPremiumReceivableTrans tempPremiumReceivable)
        {
            Policy policy = new Policy();
            policy.DefaultBeneficiaries = new List<Beneficiary>() {
                new Beneficiary()
                {
                    IndividualId = Convert.ToInt32(tempPremiumReceivable.PayerId)
                }
            };
            policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(tempPremiumReceivable.EndorsementId) };
            policy.ExchangeRate = new COMMMOD.ExchangeRate()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempPremiumReceivable.CurrencyCode) }
            };
            policy.Id = Convert.ToInt32(tempPremiumReceivable.PolicyId);
            policy.PayerComponents = new List<PayerComponent>()
            {
                new PayerComponent()
                {
                    Amount = 1,
                    BaseAmount = Convert.ToDecimal(tempPremiumReceivable.IncomeAmount)
                }
            };
            policy.PaymentPlan = new PaymentPlan()
            {
                Quotas = new List<Quota>()
                {
                    new Quota() { Number = Convert.ToInt32(tempPremiumReceivable.PaymentNum) }
                }
            };

            COMMMOD.Amount deductCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(tempPremiumReceivable.DiscountedCommission) };

            return new PremiumReceivableTransactionItem()
            {
                Id = tempPremiumReceivable.TempPremiumReceivableTransCode,
                Policy = policy,
                DeductCommission = deductCommission
            };
        }

        public static PremiumReceivableTransactionItem CreateTempPremiumReceivableTransactionItem(ACCEN.TempApplicationPremium tempPremiumReceivable)
        {
            Policy policy = new Policy();
            policy.DefaultBeneficiaries = new List<Beneficiary>() {
                new Beneficiary()
                {
                    IndividualId = Convert.ToInt32(tempPremiumReceivable.PayerCode)
                }
            };
            policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(tempPremiumReceivable.EndorsementCode) };
            policy.ExchangeRate = new COMMMOD.ExchangeRate()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempPremiumReceivable.CurrencyCode) },
                SellAmount = tempPremiumReceivable.ExchangeRate,
                BuyAmount = tempPremiumReceivable.ExchangeRate
            };
            //policy.Id = Convert.ToInt32(tempPremiumReceivable.PolicyId);
            policy.PayerComponents = new List<PayerComponent>()
            {
                new PayerComponent()
                {
                    Amount = 1,
                    BaseAmount = Convert.ToDecimal(tempPremiumReceivable.MainAmount)
                }
            };
            policy.PaymentPlan = new PaymentPlan()
            {
                Quotas = new List<Quota>()
                {
                    new Quota() { Number = Convert.ToInt32(tempPremiumReceivable.PaymentNum) }
                }
            };

            COMMMOD.Amount deductCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(tempPremiumReceivable.DiscountedCommission) };

            return new PremiumReceivableTransactionItem()
            {
                Id = tempPremiumReceivable.TempAppPremiumCode,
                Policy = policy,
                DeductCommission = deductCommission
            };
        }

        /// <summary>
        /// CreateTempPremiumReceivableTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PremiumReceivableTransactionItem/></returns>
        public static List<PremiumReceivableTransactionItem> CreateTempPremiumReceivableTransactionItems(BusinessCollection businessCollection)
        {
            List<PremiumReceivableTransactionItem> listPremiumReceivableTransactions = new List<PremiumReceivableTransactionItem>();

            foreach (ACCEN.TempPremiumReceivableTrans tempPremiumReceivableEntity in businessCollection.OfType<ACCEN.TempPremiumReceivableTrans>())
            {
                listPremiumReceivableTransactions.Add(CreateTempPremiumReceivableTransactionItem(tempPremiumReceivableEntity));
            }
            return listPremiumReceivableTransactions;
        }

        #endregion

        #region PremiumReceivableTransaction

        /// <summary>
        /// CreatePremiumReceivableTransaction
        /// </summary>
        /// <param name="premiumReceivableTransaction"></param>
        /// <returns>PremiumReceivableTransaction</returns>


        ///este metodo recibe lo enviado por la liena 909 y a la variable ID se le asigna el valor de PremiumReceivableTransId debe ser de tipo PremiumReceivableTrans
        //Afectacion 4: El objetivo principal es retornar un Id de PremiumReceivableTransId

        public static PremiumReceivableTransaction CreatePremiumReceivableTransaction(int premiumReceivableTransaction)
        //public static PremiumReceivableTransaction CreatePremiumReceivableTransaction(ACCEN.PremiumReceivableTrans premiumReceivableTransaction
        {
            return new PremiumReceivableTransaction()
            {
                Id = int.MinValue
                //Id = premiumReceivableTransaction.PremiumReceivableTransId
            };
        }

        /// <summary>
        /// CreatePremiumReceivableTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PremiumReceivableTransaction/></returns>
        /// 
        public static List<PremiumReceivableTransaction> CreatePremiumReceivableTransactions(BusinessCollection businessCollection)
        {
            //Afectacion 1 : metodo CreatePremiumReceivableTransactions recibe tipo BusinessCollection
            List<PremiumReceivableTransaction> premiumReceivableTransactions = new List<PremiumReceivableTransaction>();
            //Crea objeto premiumReceivableTransactions tipo  List<PremiumReceivableTransaction>
            // aqui se afecta dehibyb
            /*foreach (ACCEN.PremiumReceivableTrans premiumReceivableEntity in businessCollection.OfType<ACCEN.PremiumReceivableTrans>())
            //El parametro businessCollection se convierte en tipo "PremiumReceivableTrans"  y se le asigna a la variable premiumReceivableEntity del mismo tipo
            {
                // El objeto premiumReceivableTransactions creado anteriormente adiciona un objeto que lo llama de CreatePremiumReceivableTransaction
                //y le envia como parametro todos los objetos de premiumReceivableEntity
                premiumReceivableTransactions.Add(CreatePremiumReceivableTransaction(premiumReceivableEntity));
            }*/
            return premiumReceivableTransactions; // Retorna todo lo del objeto premiumReceivableTransactions
        }
        public static List<DiscountedCommissionDTO> CreateAgentCommissions(List<UNDDTOs.IssuanceAgencyDTO> agentCommissions, List<DiscountedCommissionDTO> tempCommissions = null)
        {
            List<DiscountedCommissionDTO> agentCommissionsDTOs = new List<DiscountedCommissionDTO>();
            //DiscountedCommissionDTO agentCommissionsDTO = new DiscountedCommissionDTO();
            if (tempCommissions != null)
            {
                foreach (UNDDTOs.IssuanceAgencyDTO commissions in agentCommissions)
                {
                    DiscountedCommissionDTO agentCommissionDTO = new DiscountedCommissionDTO
                    ()
                    {
                        AgentIndividualId = commissions.AgentIndividualId,
                        BaseAmount = commissions.BaseAmount,
                        CommissionPercentage = commissions.CommissionPercentage,
                        AgentPercentageParticipation = commissions.AgentPercentageParticipation,
                        CommissionDiscountIncomeAmount = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().Amount,
                        AgentName = commissions.AgentName,
                        IsUsedCommission = commissions.IsUsedCommission,
                        AgentAgencyId = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().AgentAgencyId,
                        AgentId = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().AgentId,
                        AgentTypeCode = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().AgentTypeCode,
                        Amount = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().Amount,
                        CommissionType = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().CommissionType,
                        CurrencyId = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().CurrencyId,
                        LocalAmount = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().LocalAmount,
                        ExchangeRate = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().ExchangeRate,
                        ApplicationPremiumId = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().ApplicationPremiumId,
                        Id = tempCommissions.Where(x => x.AgentIndividualId == commissions.AgentIndividualId).FirstOrDefault().DiscountedCommissionId,
                    };
                    agentCommissionsDTOs.Add(agentCommissionDTO);
                }
            }
            else
            {
                foreach (UNDDTOs.IssuanceAgencyDTO commissions in agentCommissions)
                {
                    DiscountedCommissionDTO agentCommissionDTO = new DiscountedCommissionDTO
                    ()
                    {
                        AgentIndividualId = commissions.AgentIndividualId,
                        AgentTypeCode = commissions.AgentTypeCode,
                        BaseAmount = commissions.BaseAmount,
                        CommissionPercentage = commissions.CommissionPercentage,
                        AgentPercentageParticipation = commissions.AgentPercentageParticipation,
                        AgentName = commissions.AgentName,
                        IsUsedCommission = commissions.IsUsedCommission,
                        AgentAgencyId = commissions.AgentAgencyId
                    };
                    agentCommissionsDTOs.Add(agentCommissionDTO);
                }
            }
            return agentCommissionsDTOs;
        }

        #endregion

        #region PremiumReceivableTransactionItem



        /// <summary>
        /// CreateTempPremiumReceivableTransactionItem
        /// </summary>
        /// <param name="premiumReceivableTransaction"></param>
        /// <returns>PremiumReceivableTransaction</returns>
     /*   public static PremiumReceivableTransactionItem CreatePremiumReceivableTransactionItem(ACCEN.PremiumReceivableTrans premiumReceivableTransaction)
        {
            // aqui se afecta dehibyb
            Policy policy = new Policy();
            policy.DefaultBeneficiaries = new List<Beneficiary>() {
                new Beneficiary()
                {
                    IndividualId = Convert.ToInt32(premiumReceivableTransaction.PayerId)
                }
            };
            policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(premiumReceivableTransaction.EndorsementId) };
            policy.ExchangeRate = new COMMMOD.ExchangeRate()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(premiumReceivableTransaction.CurrencyCode) },
                SellAmount = Convert.ToDecimal(premiumReceivableTransaction.ExchangeRate)
            };
            policy.Id = Convert.ToInt32(premiumReceivableTransaction.PolicyId);
            policy.PayerComponents = new List<PayerComponent>()
            {
                new PayerComponent()
                {
                    Amount = Convert.ToDecimal(premiumReceivableTransaction.PaymentAmount),
                    BaseAmount = Convert.ToDecimal(premiumReceivableTransaction.IncomeAmount)
                }
            };
            policy.PaymentPlan = new PaymentPlan()
            {
                Quotas = new List<Quota>()
                {
                    new Quota() { Number = Convert.ToInt32(premiumReceivableTransaction.PaymentNum) }
                }
            };

            COMMMOD.Amount deductCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(premiumReceivableTransaction.DiscountedCommission) };

            //del parametro recibido de tipo PremiumReceivableTrans realiza unos calculos y asignaciones a variables de la linea
            //980 a 1010 y devuelve un objeto tipo PremiumReceivableTransactionItem
            return new PremiumReceivableTransactionItem()
            {
                Id = premiumReceivableTransaction.PremiumReceivableTransId,
                Policy = policy,
                DeductCommission = deductCommission
            };
        }
        */
        /// <summary>
        /// CreateTempPremiumReceivableTransactionItem
        /// </summary>
        /// <param name="premiumReceivableTransaction"></param>
        /// <returns>PremiumReceivableTransaction</returns>
        public static PremiumReceivableTransactionItem CreatePremiumReceivableTransactionItem(ACCEN.ApplicationPremium premiumReceivableTransaction)
        {
            Policy policy = new Policy();
            policy.DefaultBeneficiaries = new List<Beneficiary>() {
                new Beneficiary()
                {
                    IndividualId = Convert.ToInt32(premiumReceivableTransaction.PayerCode)
                }
            };
            policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(premiumReceivableTransaction.EndorsementCode) };
            policy.ExchangeRate = new COMMMOD.ExchangeRate()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(premiumReceivableTransaction.CurrencyCode) },
                SellAmount = Convert.ToDecimal(premiumReceivableTransaction.ExchangeRate)
            };
            //policy.Id = Convert.ToInt32(premiumReceivableTransaction.PolicyId);
            policy.PayerComponents = new List<PayerComponent>()
            {
                new PayerComponent()
                {
                    Amount = Convert.ToDecimal(premiumReceivableTransaction.LocalAmount),
                    BaseAmount = Convert.ToDecimal(premiumReceivableTransaction.LocalAmount)
                }
            };
            policy.PaymentPlan = new PaymentPlan()
            {
                Quotas = new List<Quota>()
                {
                    new Quota() { Number = Convert.ToInt32(premiumReceivableTransaction.PaymentNum) }
                }
            };

            COMMMOD.Amount deductCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(premiumReceivableTransaction.DiscountedCommission) };

            return new PremiumReceivableTransactionItem()
            {
                Id = premiumReceivableTransaction.AppPremiumCode,
                Policy = policy,
                DeductCommission = deductCommission
            };
        }

        /// <summary>
        /// CreatePremiumReceivableTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PremiumReceivableTransactionItem/></returns>
        public static List<PremiumReceivableTransactionItem> CreatePremiumReceivableTransactionItems(BusinessCollection businessCollection)
        {
            List<PremiumReceivableTransactionItem> premiumReceivableTransactionItems = new List<PremiumReceivableTransactionItem>();
            // aqui se afecta dehibyb
            foreach (ACCEN.ApplicationPremium premiumReceivableEntity in businessCollection.OfType<ACCEN.ApplicationPremium>())
            {
                premiumReceivableTransactionItems.Add(CreatePremiumReceivableTransactionItem(premiumReceivableEntity));
            }
            return premiumReceivableTransactionItems;
        }

        #endregion

        #region Application

        /// <summary>
        /// CreateImputation
        /// </summary>
        /// <param name="entityApplication"></param>
        /// <returns>Imputation</returns>
        public static Models.Imputations.Application CreateApplication(ACCEN.Application entityApplication)
        {
            return new Models.Imputations.Application()
            {
                Id = entityApplication.ApplicationCode,
                UserId = Convert.ToInt32(entityApplication.UserCode),
                RegisterDate = Convert.ToDateTime(entityApplication.RegisterDate),
                AccountingDate = Convert.ToDateTime(entityApplication.AccountingDate),
                ModuleId = entityApplication.ModuleCode,
                TechnicalTransaction = entityApplication.TechnicalTransaction ?? 0,
                SourceId = Convert.ToInt32(entityApplication.SourceCode),
                BranchId = Convert.ToInt32(entityApplication.BranchCode),
                IndividualId = Convert.ToInt32(entityApplication.IndividualCode),
                Description = Convert.ToString(entityApplication.Description)
            };
        }

        /// <summary>
        /// CreateCollectImputation
        /// </summary>
        /// <param name="entityApplication"></param>
        /// <returns>CollectImputation</returns>
        public static CollectApplication CreateCollectApplication(ACCEN.Application entityApplication)
        {
            Models.Imputations.Application applicationModel = new Models.Imputations.Application()
            {
                Id = entityApplication.ApplicationCode,
                UserId = Convert.ToInt32(entityApplication.UserCode),
                RegisterDate = Convert.ToDateTime(entityApplication.RegisterDate),
                ModuleId = entityApplication.ModuleCode,
                SourceId = Convert.ToInt32(entityApplication.SourceCode),
                BranchId = Convert.ToInt32(entityApplication.BranchCode),
                IndividualId = Convert.ToInt32(entityApplication.IndividualCode),
                Description = Convert.ToString(entityApplication.Description)
            };

            Collect collect = new Collect() { Id = Convert.ToInt32(entityApplication.SourceCode) };
            Transaction transaction = new Transaction()
            {
                TechnicalTransaction = Convert.ToInt32(entityApplication.TechnicalTransaction),
            };

            return new CollectApplication()
            {
                Collect = collect,
                Application = applicationModel,
                Transaction = transaction
            };
        }
        #endregion

        #region Imputation

        ///// <summary>
        ///// CreateImputation
        ///// </summary>
        ///// <param name="imputation"></param>
        ///// <returns>Imputation</returns>
        //public static Imputation CreateImputation(ACCEN.Imputation imputation)
        //{
        //    return new Imputation()
        //    {
        //        Id = imputation.ImputationCode,
        //        UserId = Convert.ToInt32(imputation.UserId),
        //        Date = Convert.ToDateTime(imputation.RegisterDate),
        //        ImputationType = (ImputationTypes)imputation.ImputationTypeCode,
        //    };
        //}

        ///// <summary>
        ///// CreateCollectImputation
        ///// </summary>
        ///// <param name="imputation"></param>
        ///// <returns>CollectImputation</returns>
        //public static CollectImputation CreateCollectImputation(ACCEN.Imputation imputation)
        //{
        //    Imputation imputationModel = new Imputation()
        //    {
        //        Id = imputation.ImputationCode,
        //        UserId = Convert.ToInt32(imputation.UserId),
        //        Date = Convert.ToDateTime(imputation.RegisterDate),
        //        ImputationType = (ImputationTypes)imputation.ImputationTypeCode
        //    };

        //    Collect collect = new Collect() { Id = Convert.ToInt32(imputation.SourceCode) };
        //    Transaction transaction = new Transaction()
        //    {
        //        TechnicalTransaction = Convert.ToInt32(imputation.TechnicalTransaction),
        //    };

        //    return new CollectImputation()
        //    {
        //        Collect = collect,
        //        Imputation = imputationModel,
        //        Transaction = transaction
        //    };
        //}

        #endregion

        #region TempImputation

        ///// <summary>
        ///// CreateTempImputation
        ///// </summary>
        ///// <param name="imputation"></param>
        ///// <returns>Imputation</returns>
        //public static Imputation CreateTempImputation(ACCEN.TempImputation imputation)
        //{
        //    TransactionType transactionType = new TransactionType();
        //    transactionType.Id = Convert.ToInt32(imputation.SourceCode);
        //    List<TransactionType> transactionTypes = new List<TransactionType>();
        //    transactionTypes.Add(transactionType);

        //    ImputationTypes imputationType = new ImputationTypes();

        //    switch (imputation.ImputationTypeCode)
        //    {
        //        case 1:
        //            imputationType = ImputationTypes.Collect;
        //            break;
        //        case 2:
        //            imputationType = ImputationTypes.JournalEntry;
        //            break;
        //        case 3:
        //            imputationType = ImputationTypes.PreLiquidation;
        //            break;
        //        case 4:
        //            imputationType = ImputationTypes.PaymentOrder;
        //            break;
        //    }

        //    return new Imputation()
        //    {
        //        Id = imputation.TempImputationCode,
        //        UserId = Convert.ToInt32(imputation.UserId),
        //        Date = Convert.ToDateTime(imputation.RegisterDate),
        //        IsTemporal = Convert.ToBoolean(imputation.IsRealSource),
        //        ImputationItems = transactionTypes,
        //        ImputationType = imputationType
        //    };
        //}

        ///// <summary>
        ///// CreateTempImputations
        ///// </summary>
        ///// <param name="businessCollection"></param>
        ///// <returns>List<Imputation></returns>
        //public static List<Imputation> CreateTempImputations(BusinessCollection businessCollection)
        //{
        //    List<Imputation> imputations = new List<Imputation>();

        //    foreach (ACCEN.TempImputation imputationEntity in businessCollection.OfType<ACCEN.TempImputation>())
        //    {
        //        imputations.Add(CreateTempImputation(imputationEntity));
        //    }
        //    return imputations;
        //}

        #endregion

        #region TempApplication

        /// <summary>
        /// CreateTempImputation
        /// </summary>
        /// <param name="tempApplication"></param>
        /// <returns>Imputation</returns>
        public static Models.Imputations.Application CreateApplicationByTempApplication(ACCEN.TempApplication entityTempApplication)
        {

            TransactionType transactionType = new TransactionType();
            transactionType.Id = Convert.ToInt32(entityTempApplication.SourceCode);
            List<TransactionType> transactionTypes = new List<TransactionType>();
            transactionTypes.Add(transactionType);

            return new Models.Imputations.Application()
            {
                Id = entityTempApplication.TempAppCode,
                UserId = entityTempApplication.UserCode,
                RegisterDate = entityTempApplication.RegisterDate,
                ModuleId = Convert.ToInt32(entityTempApplication.ModuleCode),
                IndividualId = entityTempApplication.IndividualCode,
                ApplicationItems = transactionTypes,
                AccountingDate = entityTempApplication.AccountingDate,
                SourceId = Convert.ToInt32(entityTempApplication.SourceCode)
            };
        }

        /// <summary>
        /// CreateTempImputations
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Imputation></returns>
        public static List<Models.Imputations.Application> CreateApplicationByTempApplications(BusinessCollection businessCollection)
        {
            List<Models.Imputations.Application> applications = new List<Models.Imputations.Application>();

            foreach (ACCEN.TempApplication applicationEntity in businessCollection.OfType<ACCEN.TempApplication>())
            {
                applications.Add(CreateApplicationByTempApplication(applicationEntity));
            }
            return applications;
        }

        /// <summary>
        /// CreateTempApplication
        /// </summary>
        /// <param name="entityTempApplication"></param>
        /// <returns>TempApplication</returns>
        public static Models.Imputations.TempApplication CreateTempApplication(ACCEN.TempApplication entityTempApplication)
        {
            return new Models.Imputations.TempApplication()
            {
                Id = entityTempApplication.TempAppCode,
                UserId = entityTempApplication.UserCode,
                RegisterDate = entityTempApplication.RegisterDate,
                ModuleId = entityTempApplication.ModuleCode,
                IndividualId = entityTempApplication.IndividualCode,
                AccountingDate = entityTempApplication.AccountingDate,
                Description = entityTempApplication.Descriptin,
                SourceId = Convert.ToInt32(entityTempApplication.SourceCode)
            };
        }

        /// <summary>
        /// CreateTempApplications
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Imputation></returns>
        public static List<Models.Imputations.TempApplication> CreateTempApplications(BusinessCollection businessCollection)
        {
            List<Models.Imputations.TempApplication> applications = new List<Models.Imputations.TempApplication>();

            foreach (ACCEN.TempApplication applicationEntity in businessCollection.OfType<ACCEN.TempApplication>())
            {
                applications.Add(CreateTempApplication(applicationEntity));
            }
            return applications;
        }

        #endregion

        #region TempApplicationcomponent

        /// <summary>
        /// CreateTempImputation
        /// </summary>
        /// <param name="application"></param>
        /// <returns>Imputation</returns>
        public static Models.Imputations.TempApplicationPremiumComponent CreateTempApplicationComponent(BusinessObject businessObject)
        {
            var entityTempApplicationPremiumComponent = (ACCEN.TempApplicationPremiumComponent)businessObject;
            return new Models.Imputations.TempApplicationPremiumComponent
            {
                Id = entityTempApplicationPremiumComponent.TempAppComponentCode,
                TempApplicationPremiumCode = entityTempApplicationPremiumComponent.TempAppPremiumCode,
                ComponentCode = entityTempApplicationPremiumComponent.ComponentCode,
                ComponentCurrencyCode = entityTempApplicationPremiumComponent.CurrencyCode,
                ExchangeRate = entityTempApplicationPremiumComponent.ExchangeRate,
                Amount = entityTempApplicationPremiumComponent.Amount,
                LocalAmount = entityTempApplicationPremiumComponent.LocalAmount,
                MainAmount = entityTempApplicationPremiumComponent.MainAmount,
                MainLocalAmount = entityTempApplicationPremiumComponent.MainLocalAmount,
                ComponentTinyDescription = "",

            };
        }

        /// <summary>
        /// CreateTempImputations
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Imputation></returns>
        public static List<Models.Imputations.TempApplicationPremiumComponent> CreateTempApplicationComponents(BusinessCollection businessCollection)
        {
            return businessCollection.Select(CreateTempApplicationComponent).ToList();
        }

        #endregion

        #region CheckingAccountPaymentConcept

        /// <summary>
        /// CreateCheckingAccountPaymentConcept
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CheckingAccountConcept/></returns>
        public static List<CheckingAccountConcept> CreateCheckingAccountPaymentConcept(BusinessCollection businessCollection)
        {
            List<CheckingAccountConcept> checkingAccountConcepts = new List<CheckingAccountConcept>();

            foreach (PARAMENT.CheckingAccountPaymentConcept checkingAccountPaymentConcept in businessCollection.OfType<PARAMENT.CheckingAccountPaymentConcept>())
            {
                checkingAccountConcepts.Add(CreateCheckingAccountPaymentConcept(checkingAccountPaymentConcept));
            }
            return checkingAccountConcepts;
        }

        /// <summary>
        /// CreateCheckingAccountPaymentConcept
        /// </summary>
        /// <param name="checkingAccountPaymentConcept"></param>
        /// <returns>CheckingAccountConcept</returns>
        public static CheckingAccountConcept CreateCheckingAccountPaymentConcept(PARAMENT.CheckingAccountPaymentConcept checkingAccountPaymentConcept)
        {
            return new CheckingAccountConcept()
            {
                Id = checkingAccountPaymentConcept.PaymentConceptCode,
                Description = checkingAccountPaymentConcept.Description
            };
        }

        #endregion

        #region TempBrokerCheckingAccountTransaction

        /// <summary>
        /// CreateTempBrokersCheckingAccountTransaction
        /// </summary>
        /// <param name="tempBrokerCheckingAccount"></param>
        /// <returns>BrokersCheckingAccountTransaction</returns>
        public static BrokersCheckingAccountTransaction CreateTempBrokersCheckingAccountTransaction(ACCEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempBrokerCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(tempBrokerCheckingAccount.Amount)
            };
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(tempBrokerCheckingAccount.ExchangeRate) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempBrokerCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempBrokerCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempBrokerCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempBrokerCheckingAccount.CheckingAccountConceptCode) };

            List<BrokersCheckingAccountTransactionItem> tempBrokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItem>();
            tempBrokersCheckingAccountTransactionItems.Add(new BrokersCheckingAccountTransactionItem()
            {
                Comments = tempBrokerCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = tempBrokerCheckingAccount.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                ExchangeRate = exchangeRate
            });

            return new BrokersCheckingAccountTransaction()
            {
                Id = tempBrokerCheckingAccount.TempBrokerCheckingAccTransCode,
                BrokersCheckingAccountTransactionItems = tempBrokersCheckingAccountTransactionItems
            };
        }

        /// <summary>
        /// CreateTempBrokersCheckingAccountTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BrokersCheckingAccountTransaction/></returns>
        public static List<BrokersCheckingAccountTransaction> CreateTempBrokersCheckingAccountTransactions(BusinessCollection businessCollection)
        {
            List<BrokersCheckingAccountTransaction> brokersCheckingAccountTransactions = new List<BrokersCheckingAccountTransaction>();

            foreach (ACCEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity in businessCollection.OfType<ACCEN.TempBrokerCheckingAccTrans>())
            {
                brokersCheckingAccountTransactions.Add(CreateTempBrokersCheckingAccountTransaction(tempBrokerCheckingAccountEntity));
            }
            return brokersCheckingAccountTransactions;
        }

        #endregion

        #region TempBrokerCheckingAccountTransactionItem

        /// <summary>
        /// CreateTempBrokersCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempBrokerCheckingAccount"></param>
        /// <returns>BrokersCheckingAccountTransactionItem</returns>
        public static BrokersCheckingAccountTransactionItem CreateTempBrokersCheckingAccountTransactionItem(ACCEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempBrokerCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(tempBrokerCheckingAccount.Amount)
            };
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(tempBrokerCheckingAccount.ExchangeRate) };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempBrokerCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempBrokerCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempBrokerCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempBrokerCheckingAccount.CheckingAccountConceptCode) };

            return new BrokersCheckingAccountTransactionItem()
            {
                Id = tempBrokerCheckingAccount.TempBrokerCheckingAccTransCode,
                Comments = tempBrokerCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = tempBrokerCheckingAccount.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                ExchangeRate = exchangeRate
            };
        }

        /// <summary>
        /// CreateTempBrokersCheckingAccountTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BrokersCheckingAccountTransactionItem/></returns>
        public static List<BrokersCheckingAccountTransactionItem> CreateTempBrokersCheckingAccountTransactionItems(BusinessCollection businessCollection)
        {
            List<BrokersCheckingAccountTransactionItem> brokersCheckingAccountTransactions = new List<BrokersCheckingAccountTransactionItem>();

            foreach (ACCEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity in businessCollection.OfType<ACCEN.TempBrokerCheckingAccTrans>())
            {
                brokersCheckingAccountTransactions.Add(CreateTempBrokersCheckingAccountTransactionItem(tempBrokerCheckingAccountEntity));
            }
            return brokersCheckingAccountTransactions;
        }

        #endregion

        #region BrokerCheckingAccount

        /// <summary>
        /// CreateBrokersCheckingAccountTransaction
        /// </summary>
        /// <param name="brokerCheckingAccountTrans"></param>
        /// <returns>BrokersCheckingAccountTransaction</returns>
        public static BrokersCheckingAccountTransaction CreateBrokersCheckingAccountTransaction(ACCEN.BrokerCheckingAccountTrans brokerCheckingAccountTrans)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(brokerCheckingAccountTrans.CurrencyCode) },
                Value = Convert.ToDecimal(brokerCheckingAccountTrans.Amount)
            };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(brokerCheckingAccountTrans.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(brokerCheckingAccountTrans.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(brokerCheckingAccountTrans.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(brokerCheckingAccountTrans.CheckingAccountConceptCode) };

            COMMMOD.Amount commissionPercentage = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.StCommissionPercentage) };
            COMMMOD.Amount commissionAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.StCommissionAmount) };
            COMMMOD.Amount discountedCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.DiscountedCommission) };
            COMMMOD.Amount commissionBalance = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.CommissionBalance) };

            List<BrokersCheckingAccountTransactionItem> brokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItem>();
            brokersCheckingAccountTransactionItems.Add(new BrokersCheckingAccountTransactionItem()
            {
                Comments = brokerCheckingAccountTrans.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = brokerCheckingAccountTrans.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                PolicyId = Convert.ToInt32(brokerCheckingAccountTrans.PolicyId),
                PrefixId = Convert.ToInt32(brokerCheckingAccountTrans.PrefixId),
                InsuredId = Convert.ToInt32(brokerCheckingAccountTrans.InsuredId),
                CommissionType = Convert.ToInt32(brokerCheckingAccountTrans.CommissionTypeCode),
                CommissionPercentage = commissionPercentage,
                CommissionAmount = commissionAmount,
                DiscountedCommission = discountedCommission,
                CommissionBalance = commissionBalance,
                IsAutomatic = false
            });

            return new BrokersCheckingAccountTransaction()
            {
                Id = brokerCheckingAccountTrans.BrokerCheckingAccountTransId,
                BrokersCheckingAccountTransactionItems = brokersCheckingAccountTransactionItems
            };
        }

        /// <summary>
        /// CreateBrokersCheckingAccountTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BrokersCheckingAccountTransaction/></returns>
        public static List<BrokersCheckingAccountTransaction> CreateBrokersCheckingAccountTransactions(BusinessCollection businessCollection)
        {
            List<BrokersCheckingAccountTransaction> brokersCheckingAccountTransactions = new List<BrokersCheckingAccountTransaction>();

            foreach (ACCEN.BrokerCheckingAccountTrans brokerCheckingAccountTransEntity in businessCollection.OfType<ACCEN.BrokerCheckingAccountTrans>())
            {
                brokersCheckingAccountTransactions.Add(CreateBrokersCheckingAccountTransaction(brokerCheckingAccountTransEntity));
            }
            return brokersCheckingAccountTransactions;
        }

        #endregion

        #region BrokerCheckingAccountItem

        /// <summary>
        /// CreateBrokersCheckingAccountTransactionItem
        /// </summary>
        /// <param name="brokerCheckingAccountTrans"> </param>
        /// <returns>BrokersCheckingAccountTransactionItem</returns>
        public static BrokersCheckingAccountTransactionItem CreateBrokersCheckingAccountTransactionItem(ACCEN.BrokerCheckingAccountTrans brokerCheckingAccountTrans)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount();
            amount.Value = Convert.ToDecimal(brokerCheckingAccountTrans.IncomeAmount);
            amount.Currency = new COMMMOD.Currency();
            amount.Currency.Id = Convert.ToInt32(brokerCheckingAccountTrans.CurrencyCode);
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(brokerCheckingAccountTrans.ExchangeRate) };
            COMMMOD.Amount localAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.Amount) };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(brokerCheckingAccountTrans.BranchCode) };

            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(brokerCheckingAccountTrans.SalePointCode) };

            Company company = new Company() { IndividualId = Convert.ToInt32(brokerCheckingAccountTrans.AccountingCompanyCode) };

            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(brokerCheckingAccountTrans.CheckingAccountConceptCode) };

            COMMMOD.Amount commissionPercentage = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.StCommissionPercentage) };

            COMMMOD.Amount commissionAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.StCommissionAmount) };

            COMMMOD.Amount discountedCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.DiscountedCommission) };

            COMMMOD.Amount commissionBalance = new COMMMOD.Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTrans.CommissionBalance) };

            COMMMOD.LineBusiness prefix = new COMMMOD.LineBusiness() { Id = Convert.ToInt32(brokerCheckingAccountTrans.LineBusinessCode) };

            COMMMOD.SubLineBusiness subPrefix = new COMMMOD.SubLineBusiness() { Id = Convert.ToInt32(brokerCheckingAccountTrans.SubLineBusinessCode) };

            Agent agent = new Agent();
            agent.FullName = Convert.ToString(brokerCheckingAccountTrans.AgentTypeCode);
            agent.IndividualId = Convert.ToInt32(brokerCheckingAccountTrans.AgentId);


            Agency agency = new Agency();
            agency.Id = Convert.ToInt32(brokerCheckingAccountTrans.AgentAgencyId);



            Policy policy = new Policy();
            policy.DefaultBeneficiaries = new List<Beneficiary>()
            {
                new Beneficiary() { IndividualId = Convert.ToInt32(brokerCheckingAccountTrans.PayerId) }
            };
            policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(brokerCheckingAccountTrans.EndorsementId) };
            policy.ExchangeRate = new COMMMOD.ExchangeRate()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(brokerCheckingAccountTrans.CurrencyCode) },
                SellAmount = Convert.ToDecimal(brokerCheckingAccountTrans.ExchangeRate)
            };
            policy.Id = Convert.ToInt32(brokerCheckingAccountTrans.PolicyId);
            policy.PayerComponents = new List<PayerComponent>()
            {
                new PayerComponent()
                {
                    Amount = 0,
                    BaseAmount = Convert.ToDecimal(brokerCheckingAccountTrans.IncomeAmount)
                }
            };
            policy.PaymentPlan = new PaymentPlan()
            {
                Quotas = new List<Quota>()
                {
                    new Quota() { Number = Convert.ToInt32(brokerCheckingAccountTrans.PaymentNum) }
                }
            };

            return new BrokersCheckingAccountTransactionItem()
            {
                Id = brokerCheckingAccountTrans.BrokerCheckingAccountTransId,
                Comments = brokerCheckingAccountTrans.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = brokerCheckingAccountTrans.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                Policy = policy,
                PrefixId = Convert.ToInt32(brokerCheckingAccountTrans.PrefixId),
                InsuredId = Convert.ToInt32(brokerCheckingAccountTrans.InsuredId),
                CommissionType = Convert.ToInt32(brokerCheckingAccountTrans.CommissionTypeCode),
                CommissionPercentage = commissionPercentage,
                CommissionAmount = commissionAmount,
                DiscountedCommission = discountedCommission,
                CommissionBalance = commissionBalance,
                IsAutomatic = Convert.ToBoolean(brokerCheckingAccountTrans.IsAutomatic),
                Prefix = prefix,
                SubPrefix = subPrefix,
                Holder = agent,
                Agencies = { agency },
                IsPayed = Convert.ToBoolean(brokerCheckingAccountTrans.Payed),
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount
            };
        }

        /// <summary>
        /// CreateBrokersCheckingAccountTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BrokersCheckingAccountTransactionItem/></returns>
        public static List<BrokersCheckingAccountTransactionItem> CreateBrokersCheckingAccountTransactionItems(BusinessCollection businessCollection)
        {
            List<BrokersCheckingAccountTransactionItem> brokersCheckingAccountTransactions = new List<BrokersCheckingAccountTransactionItem>();

            foreach (ACCEN.BrokerCheckingAccountTrans brokerCheckingAccountTransEntity in businessCollection.OfType<ACCEN.BrokerCheckingAccountTrans>())
            {
                brokersCheckingAccountTransactions.Add(ModelAssembler.CreateBrokersCheckingAccountTransactionItem(brokerCheckingAccountTransEntity));
            }
            return brokersCheckingAccountTransactions;
        }

        #endregion

        #region TempClaimPayment

        /// <summary>
        /// CreateTempClaimPayment
        /// </summary>
        /// <param name="tempPaymentRequestTransactionItem"></param>
        /// <returns>ClaimsPaymentRequestTransactionItem</returns>
        public static ClaimsPaymentRequestTransactionItem CreateTempClaimPayment(ACCEN.TempClaimPaymentReqTrans tempPaymentRequestTransactionItem)
        {
            PAYMOD.PaymentRequest paymentRequest = new PAYMOD.PaymentRequest();
            paymentRequest.Id = Convert.ToInt32(tempPaymentRequestTransactionItem.PaymentRequestCode);
            paymentRequest.Claim = new CLMMOD.Claim() { Id = Convert.ToInt32(tempPaymentRequestTransactionItem.ClaimCode) };

            paymentRequest.IndividualId = Convert.ToInt32(tempPaymentRequestTransactionItem.BeneficiaryId);
            paymentRequest.Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempPaymentRequestTransactionItem.CurrencyCode) };
            paymentRequest.TotalAmount = Convert.ToDecimal(tempPaymentRequestTransactionItem.IncomeAmount);
            paymentRequest.RegistrationDate = Convert.ToDateTime(tempPaymentRequestTransactionItem.RegistrationDate);
            paymentRequest.EstimatedDate = Convert.ToDateTime(tempPaymentRequestTransactionItem.EstimationDate);
            paymentRequest.MovementType = new PAYMOD.MovementType()
            {
                ConceptSource = new PAYMOD.ConceptSource() { Id = Convert.ToInt32(tempPaymentRequestTransactionItem.RequestType) }
            };

            if (tempPaymentRequestTransactionItem.RequestType == 3) // Recobro
            {
                paymentRequest.PaymentDate = Convert.ToDateTime(tempPaymentRequestTransactionItem.PaymentExpirationDate);
                paymentRequest.TemporalId = Convert.ToInt32(tempPaymentRequestTransactionItem.PaymentNum);
                paymentRequest.Type = new PAYMOD.PaymentRequestType() { Id = 3 };
            }
            if (tempPaymentRequestTransactionItem.RequestType == 2) // Salvamento
            {
                paymentRequest.PaymentDate = Convert.ToDateTime(tempPaymentRequestTransactionItem.PaymentExpirationDate);
                paymentRequest.TemporalId = Convert.ToInt32(tempPaymentRequestTransactionItem.PaymentNum);
                paymentRequest.Type = new PAYMOD.PaymentRequestType() { Id = 2 };
            }

            return new ClaimsPaymentRequestTransactionItem()
            {
                Id = tempPaymentRequestTransactionItem.TempClaimPaymentReqTransCode,
                BussinessType = Convert.ToInt32(tempPaymentRequestTransactionItem.BussinessType),
                PaymentRequest = paymentRequest
            };
        }

        #endregion

        #region DepositPremiumTransaction

        /// <summary>
        /// CreateDepositPremiumTransaction
        /// </summary>
        /// <param name="depositPremiumTransaction"></param>
        /// <returns>DepositPremiumTransaction</returns>
        public static DepositPremiumTransaction CreateDepositPremiumTransaction(ACCEN.DepositPremiumTransaction depositPremiumTransaction)
        {
            Collect collect = new Collect()
            {
                Id = Convert.ToInt32(depositPremiumTransaction.CollectCode),
                Payer = new Person()
                {
                    IndividualId = Convert.ToInt32(depositPremiumTransaction.PayerId)
                }
            };

            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(depositPremiumTransaction.CurrencyCode) },
                Value = Convert.ToInt32(depositPremiumTransaction.IncomeAmount)
            };
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(depositPremiumTransaction.ExchangeRate) };
            COMMMOD.Amount localAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(depositPremiumTransaction.Amount) };

            return new DepositPremiumTransaction()
            {
                Id = depositPremiumTransaction.DepositPremiumTransactionCode,
                Collect = collect,
                Amount = amount,
                Date = Convert.ToDateTime(depositPremiumTransaction.RegisterDate),
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount
            };
        }

        /// <summary>
        /// CreateDepositPremiumTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<DepositPremiumTransaction/></returns>
        public static List<DepositPremiumTransaction> CreateDepositPremiumTransactions(BusinessCollection businessCollection)
        {
            List<DepositPremiumTransaction> depositPremiumTransactions = new List<DepositPremiumTransaction>();

            foreach (ACCEN.DepositPremiumTransaction depositPremiumTransactionEntity in businessCollection.OfType<ACCEN.DepositPremiumTransaction>())
            {
                depositPremiumTransactions.Add(CreateDepositPremiumTransaction(depositPremiumTransactionEntity));
            }
            return depositPremiumTransactions;
        }

        #endregion

        #region TempBrokerCheckingAccountItem

        /// <summary>
        /// CreateTempBrokersCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempBrokerCheckingAccountItem"></param>
        /// <returns>BrokerCheckingAccountItem</returns>
        public static BrokerCheckingAccountItem CreateTempBrokerCheckingAccountItem(ACCEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItem)
        {
            return new BrokerCheckingAccountItem()
            {
                Id = tempBrokerCheckingAccountItem.TempBrokerCheckingAccountItemCode,
                BrokerCheckingAccountId = Convert.ToInt32(tempBrokerCheckingAccountItem.BrokerCheckingAccountCode),
                TempBrokerCheckingAccountId = Convert.ToInt32(tempBrokerCheckingAccountItem.TempBrokerCheckingAccTransCode)
            };
        }

        /// <summary>
        /// CreateTempBrokerCheckingAccountItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BrokerCheckingAccountItem/></returns>
        public static List<BrokerCheckingAccountItem> CreateTempBrokerCheckingAccountItems(BusinessCollection businessCollection)
        {
            List<BrokerCheckingAccountItem> brokerCheckingAccountItems = new List<BrokerCheckingAccountItem>();

            foreach (ACCEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItemEntity in businessCollection.OfType<ACCEN.TempBrokerCheckingAccountItem>())
            {
                brokerCheckingAccountItems.Add(ModelAssembler.CreateTempBrokerCheckingAccountItem(tempBrokerCheckingAccountItemEntity));
            }
            return brokerCheckingAccountItems;
        }

        #endregion

        #region TempReinsuranceCheckingAccount

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountTransaction
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccount"></param>
        /// <returns>ReInsuranceCheckingAccountTransaction</returns>
        public static ReInsuranceCheckingAccountTransaction CreateTempReinsuranceCheckingAccountTransaction(ACCEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(tempReinsuranceCheckingAccount.Amount)
            };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.CheckingAccountConceptCode) };

            COMMMOD.Prefix prefix = new COMMMOD.Prefix()
            {
                Id = Convert.ToInt32(tempReinsuranceCheckingAccount.LineBusinessCode),
                LineBusinessId = Convert.ToInt32(tempReinsuranceCheckingAccount.SubLineBusinessCode)
            };

            Company holder = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.ReinsuranceCompanyId) };
            Company broker = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.AgentId) };

            List<ReInsuranceCheckingAccountTransactionItem> tempReinsuranceCheckingAccountTransactionItems = new List<ReInsuranceCheckingAccountTransactionItem>();
            tempReinsuranceCheckingAccountTransactionItems.Add(new ReInsuranceCheckingAccountTransactionItem()
            {
                Comments = tempReinsuranceCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = (AccountingNature)tempReinsuranceCheckingAccount.AccountingNature,
                CheckingAccountConcept = checkingAccountConcept,
                Holder = holder,
                Prefix = prefix,
                Broker = broker,
                Year = Convert.ToInt32(tempReinsuranceCheckingAccount.ApplicationYear),
                Month = Convert.ToInt32(tempReinsuranceCheckingAccount.ApplicationMonth),
                PolicyId = Convert.ToInt32(tempReinsuranceCheckingAccount.PolicyId),
                EndorsementId = Convert.ToInt32(tempReinsuranceCheckingAccount.EndorsementId),
                SlipNumber = tempReinsuranceCheckingAccount.SlipNumber,
                Region = tempReinsuranceCheckingAccount.Region,
                Period = Convert.ToInt32(tempReinsuranceCheckingAccount.Period),
                IsFacultative = Convert.ToBoolean(tempReinsuranceCheckingAccount.IsFacultative),
                Section = tempReinsuranceCheckingAccount.Section,
                ContractNumber = Convert.ToString(tempReinsuranceCheckingAccount.ContractCode)  // No es ContractNumber, es ContractId
            });

            return new ReInsuranceCheckingAccountTransaction()
            {
                Id = tempReinsuranceCheckingAccount.TempReinsCheckingAccTransCode,
                ReInsuranceCheckingAccountTransactionItems = tempReinsuranceCheckingAccountTransactionItems
            };
        }

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ReInsuranceCheckingAccountTransaction/></returns>
        public static List<ReInsuranceCheckingAccountTransaction> CreateTempReinsuranceCheckingAccountTransactions(Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            List<ReInsuranceCheckingAccountTransaction> reinsuranceCheckingAccountTransactions = new List<ReInsuranceCheckingAccountTransaction>();

            foreach (ACCEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity in businessCollection.OfType<ACCEN.TempReinsCheckingAccTrans>())
            {
                reinsuranceCheckingAccountTransactions.Add(CreateTempReinsuranceCheckingAccountTransaction(tempReinsuranceCheckingAccountEntity));
            }
            return reinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region TempCoinsuranceCheckingAccount

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccountTransaction
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccount"></param>
        /// <returns>CoInsuranceCheckingAccountTransaction</returns>
        public static CoInsuranceCheckingAccountTransaction CreateTempCoinsuranceCheckingAccountTransaction(ACCEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(tempCoinsuranceCheckingAccount.Amount)
            };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.CheckingAccountConceptCode) };
            Company holder = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccount.CoinsuredCompanyId) };

            List<CoInsuranceCheckingAccountTransactionItem> tempCoinsuranceCheckingAccountTransactionItems = new List<CoInsuranceCheckingAccountTransactionItem>();

            tempCoinsuranceCheckingAccountTransactionItems.Add(new CoInsuranceCheckingAccountTransactionItem()
            {
                Id = tempCoinsuranceCheckingAccount.TempCoinsCheckingAccTransCode,
                Holder = holder,
                CoInsuranceType = tempCoinsuranceCheckingAccount.CoinsuranceType == 1 ? CoInsuranceTypes.Accepted : CoInsuranceTypes.Given,
                Comments = tempCoinsuranceCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = tempCoinsuranceCheckingAccount.AccountingNatureCode == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                AccountingDate = Convert.ToDateTime(tempCoinsuranceCheckingAccount.AccountingDate)
            });

            return new CoInsuranceCheckingAccountTransaction()
            {
                Id = tempCoinsuranceCheckingAccount.TempCoinsCheckingAccTransCode,
                CoInsuranceCheckingAccountTransactionItems = tempCoinsuranceCheckingAccountTransactionItems
            };
        }

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CoInsuranceCheckingAccountTransaction/></returns>
        public static List<CoInsuranceCheckingAccountTransaction> CreateTempCoinsuranceCheckingAccountTransactions(BusinessCollection businessCollection)
        {
            List<CoInsuranceCheckingAccountTransaction> coinsuranceCheckingAccountTransactions = new List<CoInsuranceCheckingAccountTransaction>();

            foreach (ACCEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity in businessCollection.OfType<ACCEN.TempCoinsCheckingAccTrans>())
            {
                coinsuranceCheckingAccountTransactions.Add(ModelAssembler.CreateTempCoinsuranceCheckingAccountTransaction(tempCoinsuranceCheckingAccountEntity));
            }
            return coinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region TempReinsuranceCheckingAccountTransactionItem

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccount"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionItem</returns>
        public static ReInsuranceCheckingAccountTransactionItem CreateTempReinsuranceCheckingAccountTransactionItem(ACCEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(tempReinsuranceCheckingAccount.Amount)
            };

            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(tempReinsuranceCheckingAccount.ExchangeRate) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.CheckingAccountConceptCode) };
            Company holder = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.ReinsuranceCompanyId) };
            Company broker = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.AgentId) };

            COMMMOD.Prefix prefix = new COMMMOD.Prefix()
            {
                Id = Convert.ToInt32(tempReinsuranceCheckingAccount.LineBusinessCode),
                LineBusinessId = Convert.ToInt32(tempReinsuranceCheckingAccount.SubLineBusinessCode)
            };

            return new ReInsuranceCheckingAccountTransactionItem()
            {
                Id = tempReinsuranceCheckingAccount.TempReinsCheckingAccTransCode,
                Comments = tempReinsuranceCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = tempReinsuranceCheckingAccount.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                Holder = holder,
                Broker = broker,
                Prefix = prefix,
                Year = Convert.ToInt32(tempReinsuranceCheckingAccount.ApplicationYear),
                Month = Convert.ToInt32(tempReinsuranceCheckingAccount.ApplicationMonth),
                PolicyId = Convert.ToInt32(tempReinsuranceCheckingAccount.PolicyId),
                EndorsementId = Convert.ToInt32(tempReinsuranceCheckingAccount.EndorsementId),
                SlipNumber = tempReinsuranceCheckingAccount.SlipNumber,
                Region = tempReinsuranceCheckingAccount.Region,
                Period = Convert.ToInt32(tempReinsuranceCheckingAccount.Period),
                IsFacultative = Convert.ToBoolean(tempReinsuranceCheckingAccount.IsFacultative),
                Section = tempReinsuranceCheckingAccount.Section,
                ContractTypeId = Convert.ToInt32(tempReinsuranceCheckingAccount.ContractTypeCode),
                ContractNumber = Convert.ToString(tempReinsuranceCheckingAccount.ContractCode), // No es ContractNumber, es ContractId
                TransactionNumber = Convert.ToInt32(tempReinsuranceCheckingAccount.TransactionNumber),
                ExchangeRate = exchangeRate
            };
        }

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ReInsuranceCheckingAccountTransactionItem/></returns>
        public static List<ReInsuranceCheckingAccountTransactionItem> CreateTempReinsuranceCheckingAccountTransactionItems(BusinessCollection businessCollection)
        {
            List<ReInsuranceCheckingAccountTransactionItem> reinsuranceCheckingAccountTransactions = new List<ReInsuranceCheckingAccountTransactionItem>();

            foreach (ACCEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity in businessCollection.OfType<ACCEN.TempReinsCheckingAccTrans>())
            {
                reinsuranceCheckingAccountTransactions.Add(ModelAssembler.CreateTempReinsuranceCheckingAccountTransactionItem(tempReinsuranceCheckingAccountEntity));
            }
            return reinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region TempCoinsuranceCheckingAccountTransactionItem

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccount"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public static CoInsuranceCheckingAccountTransactionItem CreateTempCoinsuranceCheckingAccountTransactionItem(ACCEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(tempCoinsuranceCheckingAccount.Amount)
            };

            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(tempCoinsuranceCheckingAccount.ExchangeRate) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.CheckingAccountConceptCode) };
            Company holder = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccount.CoinsuredCompanyId) };

            return new CoInsuranceCheckingAccountTransactionItem()
            {
                Id = tempCoinsuranceCheckingAccount.TempCoinsCheckingAccTransCode,
                Holder = holder,
                CoInsuranceType = tempCoinsuranceCheckingAccount.CoinsuranceType == 1 ? CoInsuranceTypes.Accepted : CoInsuranceTypes.Given,
                Comments = tempCoinsuranceCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = tempCoinsuranceCheckingAccount.AccountingNatureCode == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                AccountingDate = Convert.ToDateTime(tempCoinsuranceCheckingAccount.AccountingDate),
                ExchangeRate = exchangeRate
            };
        }

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccountTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CoInsuranceCheckingAccountTransactionItem/></returns>
        public static List<CoInsuranceCheckingAccountTransactionItem> CreateTempCoinsuranceCheckingAccountTransactionItems(BusinessCollection businessCollection)
        {
            List<CoInsuranceCheckingAccountTransactionItem> coinsuranceCheckingAccountTransactions = new List<CoInsuranceCheckingAccountTransactionItem>();

            foreach (ACCEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity in businessCollection.OfType<ACCEN.TempCoinsCheckingAccTrans>())
            {
                coinsuranceCheckingAccountTransactions.Add(CreateTempCoinsuranceCheckingAccountTransactionItem(tempCoinsuranceCheckingAccountEntity));
            }
            return coinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region TempReinsuranceCheckingAccountItem

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccountItem"></param>
        /// <returns>ReinsuranceCheckingAccountItem</returns>
        public static ReinsuranceCheckingAccountItem CreateTempReinsuranceCheckingAccountItem(ACCEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItem)
        {
            return new ReinsuranceCheckingAccountItem()
            {
                Id = tempReinsuranceCheckingAccountItem.TempReinsuranceCheckingAccountItemId,
                ReinsuranceCheckingAccountId = Convert.ToInt32(tempReinsuranceCheckingAccountItem.ReinsCheckingAccTransCode),
                TempReinsuranceCheckingAccountId = Convert.ToInt32(tempReinsuranceCheckingAccountItem.TempReinsCheckingAccTransCode)
            };
        }

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ReinsuranceCheckingAccountItem/></returns>
        public static List<ReinsuranceCheckingAccountItem> CreateTempReinsuranceCheckingAccountItems(BusinessCollection businessCollection)
        {
            List<ReinsuranceCheckingAccountItem> reinsuranceCheckingAccountItems = new List<ReinsuranceCheckingAccountItem>();

            foreach (ACCEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItemEntity in businessCollection.OfType<ACCEN.TempReinsuranceCheckingAccountItem>())
            {
                reinsuranceCheckingAccountItems.Add(CreateTempReinsuranceCheckingAccountItem(tempReinsuranceCheckingAccountItemEntity));
            }
            return reinsuranceCheckingAccountItems;
        }

        #endregion

        #region TempCoinsuranceCheckingAccountItem

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccountItem"></param>
        /// <returns>CoInsuranceCheckingAccountItem</returns>
        public static CoInsuranceCheckingAccountItem CreateTempCoinsuranceCheckingAccountItem(ACCEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItem)
        {
            return new CoInsuranceCheckingAccountItem()
            {
                Id = tempCoinsuranceCheckingAccountItem.TempCoinsuranceCheckingAccountItemCode,
                CoinsuranceCheckingAccountId = Convert.ToInt32(tempCoinsuranceCheckingAccountItem.CoinsuranceCheckingAccountCode),
                TempCoinsuranceCheckingAccountId = Convert.ToInt32(tempCoinsuranceCheckingAccountItem.TempCoinsCheckingAccTransCode)
            };
        }

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccountItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CoInsuranceCheckingAccountItem/></returns>
        public static List<CoInsuranceCheckingAccountItem> CreateTempCoinsuranceCheckingAccountItems(BusinessCollection businessCollection)
        {
            List<CoInsuranceCheckingAccountItem> coinsuranceCheckingAccountItems = new List<CoInsuranceCheckingAccountItem>();

            foreach (ACCEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItemEntity in businessCollection.OfType<ACCEN.TempCoinsuranceCheckingAccountItem>())
            {
                coinsuranceCheckingAccountItems.Add(CreateTempCoinsuranceCheckingAccountItem(tempCoinsuranceCheckingAccountItemEntity));
            }
            return coinsuranceCheckingAccountItems;
        }


        #endregion

        #region ReinsuranceCheckingAccount

        /// <summary>
        /// CreateReinsuranceCheckingAccountTransaction
        /// </summary>
        /// <param name="reinsuranceCheckingAccount"></param>
        /// <returns>ReInsuranceCheckingAccountTransaction</returns>
        public static ReInsuranceCheckingAccountTransaction CreateReinsuranceCheckingAccountTransaction(ACCEN.ReinsCheckingAccTrans reinsuranceCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(reinsuranceCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(reinsuranceCheckingAccount.Amount)
            };

            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(reinsuranceCheckingAccount.ExchangeRate) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(reinsuranceCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(reinsuranceCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(reinsuranceCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(reinsuranceCheckingAccount.CheckingAccountConceptCode) };
            Company holder = new Company() { IndividualId = Convert.ToInt32(reinsuranceCheckingAccount.ReinsuranceCompanyId) };
            Company broker = new Company() { IndividualId = Convert.ToInt32(reinsuranceCheckingAccount.AgentId) };

            COMMMOD.Prefix prefix = new COMMMOD.Prefix()
            {
                Id = Convert.ToInt32(reinsuranceCheckingAccount.LineBusinessCode),
                LineBusinessId = Convert.ToInt32(reinsuranceCheckingAccount.SubLineBusinessCode)
            };

            List<ReInsuranceCheckingAccountTransactionItem> reinsuranceCheckingAccountTransactionItems = new List<ReInsuranceCheckingAccountTransactionItem>();
            reinsuranceCheckingAccountTransactionItems.Add(new ReInsuranceCheckingAccountTransactionItem()
            {
                Comments = reinsuranceCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = reinsuranceCheckingAccount.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                PolicyId = Convert.ToInt32(reinsuranceCheckingAccount.PolicyId),
                EndorsementId = Convert.ToInt32(reinsuranceCheckingAccount.EndorsementId),
                Holder = holder,
                Broker = broker,
                Prefix = prefix,
                IsFacultative = Convert.ToBoolean(reinsuranceCheckingAccount.IsFacultative),
                ContractNumber = Convert.ToString(reinsuranceCheckingAccount.ContractCode), // No es ContractNumber, es ContractId
                Section = reinsuranceCheckingAccount.Section,
                Region = reinsuranceCheckingAccount.Region,
                Period = Convert.ToInt32(reinsuranceCheckingAccount.Period),
                Year = Convert.ToInt32(reinsuranceCheckingAccount.ApplicationYear),
                Month = Convert.ToInt32(reinsuranceCheckingAccount.ApplicationMonth),
                ContractTypeId = Convert.ToInt32(reinsuranceCheckingAccount.ContractTypeCode),
                TransactionNumber = 0,
                ExchangeRate = exchangeRate
            });

            return new ReInsuranceCheckingAccountTransaction()
            {
                Id = reinsuranceCheckingAccount.ReinsCheckingAccTransId,
                ReInsuranceCheckingAccountTransactionItems = reinsuranceCheckingAccountTransactionItems
            };
        }

        /// <summary>
        /// CreateReinsuranceCheckingAccountTransactions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ReInsuranceCheckingAccountTransaction/></returns>
        /// 
        public static List<ReInsuranceCheckingAccountTransaction> CreateReinsuranceCheckingAccountTransactions(BusinessCollection businessCollection)
        {
            List<ReInsuranceCheckingAccountTransaction> reinsuranceCheckingAccountTransactions = new List<ReInsuranceCheckingAccountTransaction>();

            foreach (ACCEN.ReinsCheckingAccTrans reinsuranceCheckingAccountEntity in businessCollection.OfType<ACCEN.ReinsCheckingAccTrans>())
            {
                reinsuranceCheckingAccountTransactions.Add(ModelAssembler.CreateReinsuranceCheckingAccountTransaction(reinsuranceCheckingAccountEntity));
            }
            return reinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region ReinsuranceCheckingAccountItem

        /// <summary>
        /// CreateReinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccount"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionItem</returns>
        public static ReInsuranceCheckingAccountTransactionItem CreateReinsuranceCheckingAccountTransactionItem(ACCEN.ReinsCheckingAccTrans reinsuranceCheckingAccount)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(reinsuranceCheckingAccount.CurrencyCode) },
                Value = Convert.ToDecimal(reinsuranceCheckingAccount.Amount)
            };

            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(reinsuranceCheckingAccount.ExchangeRate) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(reinsuranceCheckingAccount.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(reinsuranceCheckingAccount.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(reinsuranceCheckingAccount.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(reinsuranceCheckingAccount.CheckingAccountConceptCode) };
            Company holder = new Company() { IndividualId = Convert.ToInt32(reinsuranceCheckingAccount.ReinsuranceCompanyId) };
            Company broker = new Company() { IndividualId = Convert.ToInt32(reinsuranceCheckingAccount.AgentId) };

            COMMMOD.Prefix prefix = new COMMMOD.Prefix()
            {
                Id = Convert.ToInt32(reinsuranceCheckingAccount.LineBusinessCode),
                LineBusinessId = Convert.ToInt32(reinsuranceCheckingAccount.SubLineBusinessCode)
            };

            return new ReInsuranceCheckingAccountTransactionItem()
            {
                Id = reinsuranceCheckingAccount.ReinsCheckingAccTransId,
                Comments = reinsuranceCheckingAccount.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = reinsuranceCheckingAccount.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                CheckingAccountConcept = checkingAccountConcept,
                PolicyId = Convert.ToInt32(reinsuranceCheckingAccount.PolicyId),
                EndorsementId = Convert.ToInt32(reinsuranceCheckingAccount.EndorsementId),
                Holder = holder,
                Prefix = prefix,
                Broker = broker,
                IsFacultative = Convert.ToBoolean(reinsuranceCheckingAccount.IsFacultative),
                ContractNumber = Convert.ToString(reinsuranceCheckingAccount.ContractCode), // No es ContractNumber, es ContractId
                Section = reinsuranceCheckingAccount.Section,
                Region = reinsuranceCheckingAccount.Region,
                Period = Convert.ToInt32(reinsuranceCheckingAccount.Period),
                Year = Convert.ToInt32(reinsuranceCheckingAccount.ApplicationYear),
                Month = Convert.ToInt32(reinsuranceCheckingAccount.ApplicationMonth),
                ContractTypeId = Convert.ToInt32(reinsuranceCheckingAccount.ContractTypeCode),
                TransactionNumber = 0,
                ExchangeRate = exchangeRate
                //IsAutomatic = false
            };
        }

        /// <summary>
        /// CreateReinsuranceCheckingAccountTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ReInsuranceCheckingAccountTransactionItem/></returns>
        public static List<ReInsuranceCheckingAccountTransactionItem> CreateReinsuranceCheckingAccountTransactionItems(BusinessCollection businessCollection)
        {
            List<ReInsuranceCheckingAccountTransactionItem> reinsuranceCheckingAccountTransactions = new List<ReInsuranceCheckingAccountTransactionItem>();

            foreach (ACCEN.ReinsCheckingAccTrans reinsuranceCheckingAccountEntity in businessCollection.OfType<ACCEN.ReinsCheckingAccTrans>())
            {
                reinsuranceCheckingAccountTransactions.Add(ModelAssembler.CreateReinsuranceCheckingAccountTransactionItem(reinsuranceCheckingAccountEntity));
            }
            return reinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region CoinsuranceCheckingAccountItem

        /// <summary>
        /// CreateCoinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="coinsCheckingAccTrans"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionItem</returns>
        public static CoInsuranceCheckingAccountTransactionItem CreateCoinsuranceCheckingAccountTransactionItem(ACCEN.CoinsCheckingAccTrans coinsCheckingAccTrans)
        {
            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(coinsCheckingAccTrans.CurrencyCode) },
                Value = Convert.ToDecimal(coinsCheckingAccTrans.IncomeAmount)
            };

            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(coinsCheckingAccTrans.ExchangeRate) };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(coinsCheckingAccTrans.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(coinsCheckingAccTrans.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(coinsCheckingAccTrans.AccountingCompanyCode) };
            CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(coinsCheckingAccTrans.CheckingAccountConceptCode) };
            Company holder = new Company() { IndividualId = Convert.ToInt32(coinsCheckingAccTrans.CoinsuredCompanyId) };
            COMMMOD.LineBusiness lineBusiness = new COMMMOD.LineBusiness() { Id = Convert.ToInt32(coinsCheckingAccTrans.LineBusinessCode) };
            COMMMOD.SubLineBusiness subLineBusiness = new COMMMOD.SubLineBusiness() { Id = Convert.ToInt32(coinsCheckingAccTrans.SubLineBusinessCode) };

            Policy policy = new Policy();
            policy.Id = Convert.ToInt32(coinsCheckingAccTrans.PolicyId);

            COMMMOD.Amount administrativeExpenses = new COMMMOD.Amount() { Value = Convert.ToDecimal(coinsCheckingAccTrans.AdministrativeExpenses) };
            COMMMOD.Amount taxAdministrativeExpenses = new COMMMOD.Amount() { Value = Convert.ToDecimal(coinsCheckingAccTrans.TaxAdministrativeExpenses) };
            COMMMOD.Amount extraCommmission = new COMMMOD.Amount() { Value = Convert.ToDecimal(coinsCheckingAccTrans.ExtraCommission) };
            COMMMOD.Amount overCommission = new COMMMOD.Amount() { Value = Convert.ToDecimal(coinsCheckingAccTrans.OverCommission) };

            return new CoInsuranceCheckingAccountTransactionItem()
            {
                Id = coinsCheckingAccTrans.CoinsCheckingAccTransId,
                CoInsuranceType = (CoInsuranceTypes)coinsCheckingAccTrans.CoinsuranceType,
                Comments = coinsCheckingAccTrans.Description,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Amount = amount,
                AccountingNature = (AccountingNature)coinsCheckingAccTrans.AccountingNatureCode,
                CheckingAccountConcept = checkingAccountConcept,
                PolicyId = Convert.ToInt32(coinsCheckingAccTrans.PolicyId),
                Holder = holder,
                AccountingDate = Convert.ToDateTime(coinsCheckingAccTrans.AccountingDate),
                LineBusiness = lineBusiness,
                SubLineBusiness = subLineBusiness,
                Policy = policy,
                CliamsId = Convert.ToInt32(coinsCheckingAccTrans.ClaimCode),
                PaymentRequestId = Convert.ToInt32(coinsCheckingAccTrans.PaymentCode),
                AdministrativeExpenses = administrativeExpenses,
                TaxAdministrativeExpenses = taxAdministrativeExpenses,
                ExtraCommission = extraCommmission,
                OverCommission = overCommission,
                ExchangeRate = exchangeRate,
            };
        }

        /// <summary>
        /// CreateCoinsuranceCheckingAccountTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CoInsuranceCheckingAccountTransactionItem></returns>
        public static List<CoInsuranceCheckingAccountTransactionItem> CreateCoinsuranceCheckingAccountTransactionItems(BusinessCollection businessCollection)
        {
            List<CoInsuranceCheckingAccountTransactionItem> coinsuranceCheckingAccountTransactions = new List<CoInsuranceCheckingAccountTransactionItem>();

            foreach (ACCEN.CoinsCheckingAccTrans coinsCheckingAccTransEntity in businessCollection.OfType<ACCEN.CoinsCheckingAccTrans>())
            {
                coinsuranceCheckingAccountTransactions.Add(CreateCoinsuranceCheckingAccountTransactionItem(coinsCheckingAccTransEntity));
            }
            return coinsuranceCheckingAccountTransactions;
        }

        #endregion

        #region TempDailyAccounting

        /// <summary>
        /// CreateTempDailyAccountingTransactionItem
        /// </summary>
        /// <param name="tempDailyAccounting"></param>
        /// <returns>DailyAccountingTransactionItem</returns>
        public static DailyAccountingTransactionItem CreateTempDailyAccountingTransactionItem(ACCEN.TempDailyAccountingTrans tempDailyAccounting)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempDailyAccounting.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempDailyAccounting.SalePointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempDailyAccounting.CompanyCode) };
            Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempDailyAccounting.BeneficiaryId) };

            AccountingNature accountingNature = (AccountingNature)tempDailyAccounting.AccountingNature;

            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempDailyAccounting.CurrencyCode) },
                Value = Convert.ToDecimal(tempDailyAccounting.IncomeAmount)
            };
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(tempDailyAccounting.ExchangeRate) };
            COMMMOD.Amount localAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(tempDailyAccounting.Amount) };

            BookAccount bookAccount = new BookAccount();
            bookAccount.Id = Convert.ToInt32(tempDailyAccounting.BookAccountCode);

            return new DailyAccountingTransactionItem()
            {
                Id = tempDailyAccounting.TempDailyAccountingTransId,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Beneficiary = beneficiary,
                AccountingNature = accountingNature,
                Amount = amount,
                BookAccount = bookAccount,
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount
            };
        }

        /// <summary>
        /// CreateTempDailyAccountingTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<DailyAccountingTransactionItem/></returns>
        public static List<DailyAccountingTransactionItem> CreateTempDailyAccountingTransactionItems(BusinessCollection businessCollection)
        {
            List<DailyAccountingTransactionItem> dailyAccountingTransactionItems = new List<DailyAccountingTransactionItem>();

            foreach (ACCEN.TempDailyAccountingTrans tempDailyAccountingEntity in businessCollection.OfType<ACCEN.TempDailyAccountingTrans>())
            {
                dailyAccountingTransactionItems.Add(CreateTempDailyAccountingTransactionItem(tempDailyAccountingEntity));
            }
            return dailyAccountingTransactionItems;
        }

        #endregion TempDailyAccounting


        /// <summary>
        /// CreateTempDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="entityTempAppAccountingAnalysis"></param>
        /// <returns></returns>
        public static ApplicationAccountingAnalysis CreateTempDailyAccountingAnalysisCode(ACCEN.TempApplicationAccountingAnalysis entityTempAppAccountingAnalysis)
        {
            Models.Imputations.AnalysisCode analysisCode = new Models.Imputations.AnalysisCode();
            ApplicationAccountingAnalysis applicationAccountingAnalysis = new ApplicationAccountingAnalysis();
            analysisCode.AnalysisCodeId = entityTempAppAccountingAnalysis.AnalysisCode;
            analysisCode.Description = entityTempAppAccountingAnalysis.Description;

            Models.Imputations.AnalysisConcept analysisConcept = new Models.Imputations.AnalysisConcept();
            analysisConcept.AnalysisConceptId = entityTempAppAccountingAnalysis.AnalysisConceptCode;

            analysisCode.AnalisisConcepts = new List<Models.Imputations.AnalysisConcept>();
            analysisCode.AnalisisConcepts.Add(analysisConcept);

            applicationAccountingAnalysis.Id = entityTempAppAccountingAnalysis.TempAppAccountingAnalysisCode;
            applicationAccountingAnalysis.AnalysisId = entityTempAppAccountingAnalysis.AnalysisCode;
            applicationAccountingAnalysis.ConceptKey = entityTempAppAccountingAnalysis.ConceptKey;
            applicationAccountingAnalysis.AnalysisConcept = analysisConcept;
            applicationAccountingAnalysis.Description = entityTempAppAccountingAnalysis.Description;
            return applicationAccountingAnalysis;
        }

        #region TempDailyAccountingAnalysisCode

        /// <summary>
        /// CreateTempDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="tempDailyAccounting"></param>
        /// <returns></returns>
        public static DailyAccountingAnalysisCode CreateTempDailyAccountingAnalysisCode(ACCEN.TempDailyAccountingAnalysis tempDailyAccounting)
        {
            Models.Imputations.AnalysisCode analysisCode = new Models.Imputations.AnalysisCode();
            analysisCode.AnalysisCodeId = tempDailyAccounting.AnalysisCode;
            analysisCode.Description = tempDailyAccounting.Description;

            Models.Imputations.AnalysisConcept analysisConcept = new Models.Imputations.AnalysisConcept();
            analysisConcept.AnalysisConceptId = tempDailyAccounting.AnalysisConceptCode;

            analysisCode.AnalisisConcepts = new List<Models.Imputations.AnalysisConcept>();
            analysisCode.AnalisisConcepts.Add(analysisConcept);

            return new DailyAccountingAnalysisCode()
            {
                Id = tempDailyAccounting.TempDailyAccountingAnalysisId,
                AnalysisCode = analysisCode,
                KeyAnalysis = tempDailyAccounting.ConceptKey
            };
        }

        /// <summary>
        /// CreateTempDailyAccountingAnalysisCodes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<DailyAccountingAnalysisCode> CreateTempDailyAccountingAnalysisCodes(BusinessCollection businessCollection)
        {
            List<DailyAccountingAnalysisCode> dailyAccountingAnalysisCodes = new List<DailyAccountingAnalysisCode>();

            foreach (ACCEN.TempDailyAccountingAnalysis tempDailyAccountingAnalysisEntity in businessCollection.OfType<ACCEN.TempDailyAccountingAnalysis>())
            {
                dailyAccountingAnalysisCodes.Add(CreateTempDailyAccountingAnalysisCode(tempDailyAccountingAnalysisEntity));
            }
            return dailyAccountingAnalysisCodes;
        }

        #endregion TempDailyAccountingAnalysisCode


        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="entityTempAccountingCostCenter"></param>
        /// <returns></returns>
        public static ApplicationAccountingCostCenter CreateTempAccountingCostCenter(ACCEN.TempApplicationAccountingCostCenter entityTempAccountingCostCenter)
        {
            Models.Imputations.CostCenter costCenter = new Models.Imputations.CostCenter();
            costCenter.CostCenterId = entityTempAccountingCostCenter.CostCenterCode;

            return new ApplicationAccountingCostCenter()
            {
                Id = entityTempAccountingCostCenter.TempAppAccountingCostCenterCode,
                CostCenter = costCenter,
                Percentage = entityTempAccountingCostCenter.Percentage
            };
        }

        #region TempDailyAccountingCostCenter

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="tempDailyAccountingCostCenter"></param>
        /// <returns></returns>
        public static DailyAccountingCostCenter CreateTempDailyAccountingCostCenter(ACCEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenter)
        {
            Models.Imputations.CostCenter costCenter = new Models.Imputations.CostCenter();
            costCenter.CostCenterId = tempDailyAccountingCostCenter.CostCenterCode;

            return new DailyAccountingCostCenter()
            {
                Id = tempDailyAccountingCostCenter.TempDailyAccountingCostCenterId,
                CostCenter = costCenter,
                Percentage = tempDailyAccountingCostCenter.Percentage
            };
        }

        /// <summary>
        /// CreateTempDailyAccountingCostCenters
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<DailyAccountingCostCenter> CreateTempDailyAccountingCostCenters(BusinessCollection businessCollection)
        {
            List<DailyAccountingCostCenter> dailyAccountingCostCenters = new List<DailyAccountingCostCenter>();

            foreach (ACCEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenterEntity in businessCollection.OfType<ACCEN.TempDailyAccountingCostCenter>())
            {
                dailyAccountingCostCenters.Add(CreateTempDailyAccountingCostCenter(tempDailyAccountingCostCenterEntity));
            }
            return dailyAccountingCostCenters;
        }

        #endregion TempDailyAccountingCostCenter

        #region DailyAccountingTransaction

        /// <summary>
        /// CreateDailyAccountingTransactionItem
        /// </summary>
        /// <param name="dailyAccountingTrans"></param>
        /// <returns>DailyAccountingTransactionItem</returns>
        public static DailyAccountingTransactionItem CreateDailyAccountingTransactionItem(ACCEN.DailyAccountingTrans dailyAccountingTrans)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(dailyAccountingTrans.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(dailyAccountingTrans.SalesPointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(dailyAccountingTrans.CompanyCode) };
            Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(dailyAccountingTrans.BeneficiaryId) };

            AccountingNature accountingNature = (AccountingNature)dailyAccountingTrans.AccountingNature;

            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(dailyAccountingTrans.CurrencyCode) },
                Value = Convert.ToDecimal(dailyAccountingTrans.IncomeAmount)
            };
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { SellAmount = Convert.ToDecimal(dailyAccountingTrans.ExchangeRate) };
            COMMMOD.Amount localAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(dailyAccountingTrans.Amount) };

            BookAccount bookAccount = new BookAccount();
            bookAccount.Id = Convert.ToInt32(dailyAccountingTrans.BookAccountCode);

            return new DailyAccountingTransactionItem()
            {
                Id = dailyAccountingTrans.DailyAccountingTransId,
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Beneficiary = beneficiary,
                AccountingNature = accountingNature,
                Amount = amount,
                BookAccount = bookAccount,
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount
            };
        }

        /// <summary>
        /// CreateDailyAccountingTransactionItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<DailyAccountingTransactionItem/></returns>
        public static List<DailyAccountingTransactionItem> CreateDailyAccountingTransactionItems(BusinessCollection businessCollection)
        {
            List<DailyAccountingTransactionItem> dailyAccountingTransactionItems = new List<DailyAccountingTransactionItem>();

            foreach (ACCEN.DailyAccountingTrans dailyAccountingEntity in businessCollection.OfType<ACCEN.DailyAccountingTrans>())
            {
                dailyAccountingTransactionItems.Add(CreateDailyAccountingTransactionItem(dailyAccountingEntity));
            }
            return dailyAccountingTransactionItems;
        }

        #endregion

        /// <summary>
        /// CreateDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="entityAccountingAnalysis"></param>
        /// <returns></returns>
        public static ApplicationAccountingAnalysis CreateApplicationAccountingAnalysis(ACCEN.ApplicationAccountingAnalysis entityAccountingAnalysis)
        {
            ApplicationAccountingAnalysis applicationAccountingAnalysis = new ApplicationAccountingAnalysis();
            Models.Imputations.AnalysisCode analysisCode = new Models.Imputations.AnalysisCode();
            analysisCode.AnalysisCodeId = entityAccountingAnalysis.AnalysisCode;
            analysisCode.Description = entityAccountingAnalysis.Description;

            Models.Imputations.AnalysisConcept analysisConcept = new Models.Imputations.AnalysisConcept();
            analysisConcept.AnalysisConceptId = entityAccountingAnalysis.AnalysisConceptCode;

            analysisCode.AnalisisConcepts = new List<Models.Imputations.AnalysisConcept>();
            analysisCode.AnalisisConcepts.Add(analysisConcept);


            applicationAccountingAnalysis.Id = entityAccountingAnalysis.AppAccountingAnalysisCode;
            applicationAccountingAnalysis.AnalysisId = entityAccountingAnalysis.AnalysisCode;
            applicationAccountingAnalysis.ConceptKey = entityAccountingAnalysis.ConceptKey;
            applicationAccountingAnalysis.AnalysisConcept = analysisConcept;
            applicationAccountingAnalysis.Description = entityAccountingAnalysis.Description;
            return applicationAccountingAnalysis;
        }



        #region DailyAccountingAnalysisCode

        /// <summary>
        /// CreateDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="dailyAccounting"></param>
        /// <returns></returns>
        public static DailyAccountingAnalysisCode CreateDailyAccountingAnalysisCode(ACCEN.DailyAccountingAnalysis dailyAccounting)
        {
            Models.Imputations.AnalysisCode analysisCode = new Models.Imputations.AnalysisCode();
            analysisCode.AnalysisCodeId = dailyAccounting.AnalysisCode;
            analysisCode.Description = dailyAccounting.Description;

            Models.Imputations.AnalysisConcept analysisConcept = new Models.Imputations.AnalysisConcept();
            analysisConcept.AnalysisConceptId = dailyAccounting.AnalysisConceptCode;

            analysisCode.AnalisisConcepts = new List<Models.Imputations.AnalysisConcept>();
            analysisCode.AnalisisConcepts.Add(analysisConcept);

            return new DailyAccountingAnalysisCode()
            {
                Id = dailyAccounting.DailyAccountingAnalysisId,
                AnalysisCode = analysisCode,
                KeyAnalysis = dailyAccounting.ConceptKey
            };
        }

        /// <summary>
        /// CreateTempDailyAccountingAnalysisCodes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<DailyAccountingAnalysisCode> CreateDailyAccountingAnalysisCodes(BusinessCollection businessCollection)
        {
            List<DailyAccountingAnalysisCode> dailyAccountingAnalysisCodes = new List<DailyAccountingAnalysisCode>();

            foreach (ACCEN.DailyAccountingAnalysis dailyAccountingAnalysisEntity in businessCollection.OfType<ACCEN.DailyAccountingAnalysis>())
            {
                dailyAccountingAnalysisCodes.Add(CreateDailyAccountingAnalysisCode(dailyAccountingAnalysisEntity));
            }
            return dailyAccountingAnalysisCodes;
        }

        #endregion DailyAccountingAnalysisCode

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="entityApplicationAccountingCostCenter"></param>
        /// <returns></returns>
        public static ApplicationAccountingCostCenter CreateApplicationAccountingCostCenter(ACCEN.ApplicationAccountingCostCenter entityApplicationAccountingCostCenter)
        {
            Models.Imputations.CostCenter costCenter = new Models.Imputations.CostCenter() { CostCenterId = entityApplicationAccountingCostCenter.CostCenterCode };

            return new ApplicationAccountingCostCenter()
            {
                Id = entityApplicationAccountingCostCenter.AppAccountingCostCenterCode,
                CostCenter = costCenter,
                Percentage = entityApplicationAccountingCostCenter.Percentage
            };
        }

        #region DailyAccountingCostCenter

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="dailyAccountingCostCenter"></param>
        /// <returns></returns>
        public static DailyAccountingCostCenter CreateDailyAccountingCostCenter(ACCEN.DailyAccountingCostCenter dailyAccountingCostCenter)
        {
            Models.Imputations.CostCenter costCenter = new Models.Imputations.CostCenter() { CostCenterId = dailyAccountingCostCenter.CostCenterCode };

            return new DailyAccountingCostCenter()
            {
                Id = dailyAccountingCostCenter.DailyAccountingCostCenterId,
                CostCenter = costCenter,
                Percentage = dailyAccountingCostCenter.Percentage
            };
        }

        /// <summary>
        /// CreateTempDailyAccountingCostCenters
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<DailyAccountingCostCenter> CreateDailyAccountingCostCenters(BusinessCollection businessCollection)
        {
            List<DailyAccountingCostCenter> dailyAccountingCostCenters = new List<DailyAccountingCostCenter>();

            foreach (ACCEN.DailyAccountingCostCenter dailyAccountingCostCenterEntity in businessCollection.OfType<ACCEN.DailyAccountingCostCenter>())
            {
                dailyAccountingCostCenters.Add(CreateDailyAccountingCostCenter(dailyAccountingCostCenterEntity));
            }
            return dailyAccountingCostCenters;
        }

        #endregion DailyAccountingCostCenter

        #region TempJournalEntry

        /// <summary>
        /// CreateTempJournalEntry
        /// </summary>
        /// <param name="tempJournalEntry"></param>
        /// <returns>JournalEntry</returns>
        public static IMPMOD.JournalEntry CreateTempJournalEntry(ACCEN.TempJournalEntry tempJournalEntry)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempJournalEntry.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempJournalEntry.SalesPointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempJournalEntry.CompanyCode) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(tempJournalEntry.PersonTypeCode) };
            Individual payer = new Individual() { IndividualId = Convert.ToInt32(tempJournalEntry.IndividualId) };
            EEProvider.Models.Imputations.Application imputation = new EEProvider.Models.Imputations.Application() { Id = 1 };

            return new IMPMOD.JournalEntry()
            {
                Id = tempJournalEntry.TempJournalEntryCode,
                AccountingDate = Convert.ToDateTime(tempJournalEntry.AccountingDate),
                Branch = branch,
                Comments = tempJournalEntry.Comments,
                Company = company,
                Description = tempJournalEntry.Description,
                Imputation = imputation,
                Payer = payer,
                PersonType = personType,
                SalePoint = salePoint,
                Status = Convert.ToInt32(tempJournalEntry.Status.Value)
            };
        }

        /// <summary>
        /// CreateTempJournalEntry
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<JournalEntry/></returns>
        public static List<IMPMOD.JournalEntry> CreateTempJournalEntry(BusinessCollection businessCollection)
        {
            List<IMPMOD.JournalEntry> journalEntryTransactionItems = new List<IMPMOD.JournalEntry>();

            foreach (ACCEN.TempJournalEntry tempJournalEntryEntity in businessCollection.OfType<ACCEN.TempJournalEntry>())
            {
                journalEntryTransactionItems.Add(CreateTempJournalEntry(tempJournalEntryEntity));
            }
            return journalEntryTransactionItems;
        }

        #endregion

        #region JournalEntry

        /// <summary>
        /// CreateJournalEntry
        /// </summary>
        /// <param name="entityJournalEntry"></param>
        /// <returns>JournalEntry</returns>
        public static IMPMOD.JournalEntry CreateJournalEntry(ACCEN.JournalEntry entityJournalEntry)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(entityJournalEntry.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(entityJournalEntry.SalesPointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(entityJournalEntry.CompanyCode) };
            Individual payer = new Individual() { IndividualId = Convert.ToInt32(entityJournalEntry.IndividualId) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(entityJournalEntry.PersonTypeCode) };
            EEProvider.Models.Imputations.Application imputation = new EEProvider.Models.Imputations.Application() { Id = 1 };

            return new IMPMOD.JournalEntry()
            {
                Id = entityJournalEntry.JournalEntryCode,
                AccountingDate = Convert.ToDateTime(entityJournalEntry.AccountingDate),
                Branch = branch,
                Comments = entityJournalEntry.Comments,
                Company = company,
                Description = entityJournalEntry.Description,
                Imputation = imputation,
                Payer = payer,
                PersonType = personType,
                SalePoint = salePoint,
                Status = Convert.ToInt32(entityJournalEntry.Status.GetValueOrDefault())
            };
        }

        /// <summary>
        /// CreateJournalEntry
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<JournalEntry/></returns>
        public static List<IMPMOD.JournalEntry> CreateJournalEntry(BusinessCollection businessCollection)
        {
            List<IMPMOD.JournalEntry> journalEntryTransactionItems = new List<IMPMOD.JournalEntry>();

            foreach (ACCEN.JournalEntry journalEntryEntity in businessCollection.OfType<ACCEN.JournalEntry>())
            {
                journalEntryTransactionItems.Add(ModelAssembler.CreateJournalEntry(journalEntryEntity));
            }
            return journalEntryTransactionItems;
        }

        #endregion

        #region TempPreLiquidation

        /// <summary>
        /// CreateTempPreLiquidation
        /// </summary>
        /// <param name="tempPreliquidation"></param>
        /// <returns>PreLiquidation</returns>
        public static PreLiquidation CreateTempPreLiquidation(ACCEN.TempPreliquidation tempPreliquidation)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempPreliquidation.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(tempPreliquidation.SalesPointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempPreliquidation.CompanyCode) };
            Individual payer = new Individual() { IndividualId = Convert.ToInt32(tempPreliquidation.IndividualId) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(tempPreliquidation.PersonTypeCode) };

            return new PreLiquidation()
            {
                Id = tempPreliquidation.TempPreliquidationCode,
                RegisterDate = Convert.ToDateTime(tempPreliquidation.RegisterDate.Value),
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Description = tempPreliquidation.Description,
                Payer = payer,
                Status = Convert.ToInt32(tempPreliquidation.Status),
                PersonType = personType
            };
        }

        #endregion

        #region CheckbookControl

        /// <summary>
        /// CreateCheckBookControl
        /// </summary>
        /// <param name="checkbookControl"></param>
        /// <returns>CheckBookControl</returns>
        public static CheckBookControl CreateCheckBookControl(ACCEN.CheckbookControl checkbookControl)
        {
            BankAccountCompany bankAccountCompany = new BankAccountCompany() { Id = Convert.ToInt32(checkbookControl.AccountBankCode) };

            return new CheckBookControl()
            {
                Id = checkbookControl.CheckbookControlCode,
                AccountBank = bankAccountCompany,
                CheckFrom = Convert.ToInt32(checkbookControl.CheckFrom),
                CheckTo = Convert.ToInt32(checkbookControl.CheckTo),
                DisabledDate = Convert.ToDateTime(checkbookControl.DisabledDate),
                IsAutomatic = Convert.ToBoolean(checkbookControl.IsAutomatic),
                LastCheck = Convert.ToInt32(checkbookControl.LastCheck),
                Status = Convert.ToInt32(checkbookControl.Status.Value)
            };
        }

        /// <summary>
        /// CreateCheckBookControls
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>list<CheckBookControl/></returns>
        public static List<CheckBookControl> CreateCheckBookControls(BusinessCollection businessCollection)
        {
            List<CheckBookControl> checkBookControls = new List<CheckBookControl>();
            foreach (ACCEN.CheckbookControl checkbookControlEntity in businessCollection.OfType<ACCEN.CheckbookControl>())
            {
                checkBookControls.Add(ModelAssembler.CreateCheckBookControl(checkbookControlEntity));
            }
            return checkBookControls;
        }

        #endregion

        #region PreLiquidation

        /// <summary>
        /// CreatePreLiquidation
        /// </summary>
        /// <param name="preliquidation"></param>
        /// <returns>PreLiquidation</returns>
        public static PreLiquidation CreatePreLiquidation(ACCEN.Preliquidation preliquidation)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(preliquidation.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(preliquidation.SalesPointCode) };
            Company company = new Company() { IndividualId = Convert.ToInt32(preliquidation.CompanyCode) };
            Individual payer = new Individual() { IndividualId = Convert.ToInt32(preliquidation.IndividualId) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(preliquidation.PersonTypeCode) };

            return new PreLiquidation()
            {
                Id = preliquidation.PreliquidationCode,
                RegisterDate = Convert.ToDateTime(preliquidation.RegisterDate.Value),
                Branch = branch,
                SalePoint = salePoint,
                Company = company,
                Description = preliquidation.Description,
                Payer = payer,
                Status = Convert.ToInt32(preliquidation.Status),
                PersonType = personType
            };
        }

        #endregion

        #region TempPaymentOrder

        /// <summary>
        /// CreateTempPaymentOrder
        /// </summary>
        /// <param name="tempPaymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public static PaymentOrder CreateTempPaymentOrder(ACCEN.TempPaymentOrder tempPaymentOrder)
        {
            BankAccountPerson accountBank = new BankAccountPerson() { Id = Convert.ToInt32(tempPaymentOrder.AccountBankCode) };

            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(tempPaymentOrder.CurrencyCode) },
                Value = Convert.ToDecimal(tempPaymentOrder.Amount)
            };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempPaymentOrder.BranchCode) };
            COMMMOD.Branch paymentBranch = new COMMMOD.Branch() { Id = Convert.ToInt32(tempPaymentOrder.BranchCdPay) };
            Company company = new Company() { IndividualId = Convert.ToInt32(tempPaymentOrder.CompanyCode) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(tempPaymentOrder.PersonTypeCode) };
            Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempPaymentOrder.IndividualId) };
            PaymentsModels.PaymentMethod paymentMethod = new PaymentsModels.PaymentMethod() { Id = Convert.ToInt32(tempPaymentOrder.PaymentMethodCode) };
            EEProvider.Models.Claims.PaymentRequest.ConceptSource paymentSource = new EEProvider.Models.Claims.PaymentRequest.ConceptSource() { Id = Convert.ToInt32(tempPaymentOrder.PaymentSourceCode) };
            EEProvider.Models.Imputations.Application imputation = new EEProvider.Models.Imputations.Application() { Id = 1 };

            return new PaymentOrder()
            {
                Id = tempPaymentOrder.TempPaymentOrderCode,
                BankAccountPerson = accountBank,
                AccountingDate = Convert.ToDateTime(tempPaymentOrder.AccountingDate),
                Amount = amount,
                Beneficiary = beneficiary,
                Branch = branch,
                BranchPay = paymentBranch,
                Company = company,
                EstimatedPaymentDate = Convert.ToDateTime(tempPaymentOrder.EstimatedPaymentDate),
                Imputation = imputation,
                PaymentMethod = paymentMethod,
                PaymentSource = paymentSource,
                PayTo = tempPaymentOrder.PayTo,
                PersonType = personType,
                Status = Convert.ToInt32(tempPaymentOrder.Status.Value),
                Observation = tempPaymentOrder.Observation
            };
        }

        /// <summary>
        /// CreateTempPaymentOrder
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentOrder/></returns>
        public static List<PaymentOrder> CreateTempPaymentOrder(BusinessCollection businessCollection)
        {
            List<PaymentOrder> paymentOrders = new List<PaymentOrder>();

            foreach (ACCEN.TempPaymentOrder tempPaymentOrderEntity in businessCollection.OfType<ACCEN.TempPaymentOrder>())
            {
                paymentOrders.Add(CreateTempPaymentOrder(tempPaymentOrderEntity));
            }
            return paymentOrders;
        }

        #endregion

        #region PaymentOrder

        /// <summary>
        /// CreatePaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public static PaymentOrder CreatePaymentOrder(ACCEN.PaymentOrder paymentOrder)
        {
            BankAccountPerson accountBank = new BankAccountPerson();
            accountBank.Id = Convert.ToInt32(paymentOrder.AccountBankCode);

            COMMMOD.Amount amount = new COMMMOD.Amount()
            {
                Currency = new COMMMOD.Currency() { Id = Convert.ToInt32(paymentOrder.CurrencyCode) },
                Value = Convert.ToDecimal(paymentOrder.IncomeAmount)
            };
            COMMMOD.Amount localAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(paymentOrder.Amount) };
            COMMMOD.ExchangeRate exchangeRate = new COMMMOD.ExchangeRate() { BuyAmount = Convert.ToDecimal(paymentOrder.ExchangeRate) };

            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(paymentOrder.BranchCode) };
            COMMMOD.Branch branchPay = new COMMMOD.Branch() { Id = Convert.ToInt32(paymentOrder.BranchCdPay) };
            Company company = new Company() { IndividualId = Convert.ToInt32(paymentOrder.CompanyCode) };
            Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(paymentOrder.IndividualId) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(paymentOrder.PersonTypeCode) };
            PaymentsModels.PaymentMethod paymentMethod = new PaymentsModels.PaymentMethod() { Id = Convert.ToInt32(paymentOrder.PaymentMethodCode) };
            EEProvider.Models.Claims.PaymentRequest.ConceptSource paymentSource = new EEProvider.Models.Claims.PaymentRequest.ConceptSource() { Id = Convert.ToInt32(paymentOrder.PaymentSourceCode) };
            EEProvider.Models.Imputations.Application imputation = new EEProvider.Models.Imputations.Application() { Id = 1 };

            return new PaymentOrder()
            {
                Id = paymentOrder.PaymentOrderCode,
                BankAccountPerson = accountBank,
                AccountingDate = Convert.ToDateTime(paymentOrder.AccountingDate),
                Amount = amount,
                Beneficiary = beneficiary,
                Branch = branch,
                BranchPay = branchPay,
                Company = company,
                EstimatedPaymentDate = Convert.ToDateTime(paymentOrder.EstimatedPaymentDate),
                Imputation = imputation,
                PaymentMethod = paymentMethod,
                PaymentSource = paymentSource,
                PayTo = paymentOrder.PayTo,
                PersonType = personType,
                Status = Convert.ToInt32(paymentOrder.Status.Value),
                CancellationDate = Convert.ToDateTime(paymentOrder.CancellationDate),
                UserId = Convert.ToInt32(paymentOrder.CancellationUserId),
                PaymentDate = Convert.ToDateTime(paymentOrder.PaymentDate),
                Observation = paymentOrder.Observation,
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount
            };
        }

        /// <summary>
        /// CreatePaymentOrder
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentOrder/></returns>
        public static List<PaymentOrder> CreatePaymentOrder(BusinessCollection businessCollection)
        {
            List<PaymentOrder> paymentOrders = new List<PaymentOrder>();

            foreach (ACCEN.PaymentOrder paymentOrderEntity in businessCollection.OfType<ACCEN.PaymentOrder>())
            {
                paymentOrders.Add(CreatePaymentOrder(paymentOrderEntity));
            }
            return paymentOrders;
        }

        #endregion

        #region CheckPaymentOrder

        /// <summary>
        /// CreateCheckPaymentOrder
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>CheckPaymentOrder</returns>
        public static CheckPaymentOrder CreateCheckPaymentOrder(ACCEN.CheckPaymentOrder checkPaymentOrder)
        {
            BankAccountCompany bankAccountCompany = new BankAccountCompany() { Id = Convert.ToInt32(checkPaymentOrder.AccountBankCode) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(checkPaymentOrder.PersonTypeCode) };
            Individual delivery = new Individual() { IndividualId = Convert.ToInt32(checkPaymentOrder.IndividualId) };

            return new CheckPaymentOrder()
            {
                Id = checkPaymentOrder.CheckPaymentOrderCode,
                BankAccountCompany = bankAccountCompany,
                CheckNumber = Convert.ToInt32(checkPaymentOrder.CheckNumber),
                IsCheckPrinted = Convert.ToBoolean(checkPaymentOrder.IsCheckPrinted),
                PrintedUser = Convert.ToInt32(checkPaymentOrder.PrintedUserId),
                PrintedDate = Convert.ToDateTime(checkPaymentOrder.PrintedDate),
                DeliveryDate = Convert.ToDateTime(checkPaymentOrder.DeliveryDate),
                PersonType = personType,
                Delivery = delivery,
                Status = -1,
                CourierName = checkPaymentOrder.CourierName,
                CancellationDate = Convert.ToDateTime(checkPaymentOrder.CancellationDate),
                CancellationUser = Convert.ToInt32(checkPaymentOrder.CancellationUserId),
                RefundDate = Convert.ToDateTime(checkPaymentOrder.RefundDate)
            };
        }

        /// <summary>
        /// CreateCheckPaymentOrder
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CheckPaymentOrder/></returns>
        public static List<CheckPaymentOrder> CreateCheckPaymentOrder(BusinessCollection businessCollection)
        {
            List<CheckPaymentOrder> checkPaymentOrders = new List<CheckPaymentOrder>();

            foreach (ACCEN.CheckPaymentOrder checkPaymentOrderEntity in businessCollection.OfType<ACCEN.CheckPaymentOrder>())
            {
                checkPaymentOrders.Add(CreateCheckPaymentOrder(checkPaymentOrderEntity));
            }
            return checkPaymentOrders;
        }

        #endregion

        #region TransferPaymentOrder

        /// <summary>
        /// CreateTransferPaymentOrder
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        public static TransferPaymentOrder CreateTransferPaymentOrder(ACCEN.TransferPaymentOrder transferPaymentOrder)
        {
            BankAccountCompany bankAccountCompany = new BankAccountCompany() { Id = Convert.ToInt32(transferPaymentOrder.AccountBankCode) };

            return new TransferPaymentOrder()
            {
                Id = transferPaymentOrder.TransferPaymentOrderCode,
                BankAccountCompany = bankAccountCompany,
                CancellationDate = transferPaymentOrder.CancellationDate,
                DeliveryDate = Convert.ToDateTime(transferPaymentOrder.DeliveryDate),
                Status = Convert.ToInt32(transferPaymentOrder.Status),
                UserId = Convert.ToInt32(transferPaymentOrder.User)
            };
        }

        /// <summary>
        /// CreateTransferPaymentOrder
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<TransferPaymentOrder/></returns>
        public static List<TransferPaymentOrder> CreateTransferPaymentOrder(BusinessCollection businessCollection)
        {
            List<TransferPaymentOrder> transferPaymentOrders = new List<TransferPaymentOrder>();

            foreach (ACCEN.TransferPaymentOrder transferPaymentOrderEntity in businessCollection.OfType<ACCEN.TransferPaymentOrder>())
            {
                transferPaymentOrders.Add(CreateTransferPaymentOrder(transferPaymentOrderEntity));
            }
            return transferPaymentOrders;
        }

        #endregion

        #region PaymentOrderAgent

        /// <summary>
        /// CreatePaymentOrderAgent
        /// </summary>
        /// <param name="paymentOrderAgent"></param>
        /// <returns>int</returns>
        public static int CreatePaymentOrderAgent(ACCEN.PaymentOrderAgent paymentOrderAgent)
        {
            return paymentOrderAgent.PaymentOrderAgentCode;
        }

        #endregion

        #region PaymentOrderAgentItem

        /// <summary>
        /// CreatePaymentOrderAgentItem
        /// </summary>
        /// <param name="paymentOrderAgentItem"></param>
        /// <returns>int</returns>
        public static int CreatePaymentOrderAgentItem(ACCEN.PaymentOrderAgentItem paymentOrderAgentItem)
        {
            return paymentOrderAgentItem.PaymentOrderAgentItemCode;
        }

        #endregion

        #region BrokerBalance

        /// <summary>
        /// CreateBrokerBalance
        /// </summary>
        /// <param name="brokerBalance"></param>
        /// <returns>BrokerBalanceDTO</returns>
        public static BrokerBalanceDTO CreateBrokerBalance(ACCEN.BrokerBalance brokerBalance)
        {
            return new BrokerBalanceDTO()
            {
                BrokerBalanceId = brokerBalance.BrokerBalanceCode,
                AgentTypeCode = Convert.ToInt32(brokerBalance.AgentTypeCode),
                AgentCode = Convert.ToInt32(brokerBalance.AgentId),
                BalanceDate = Convert.ToDateTime(brokerBalance.BalanceDate),
                CurrencyId = Convert.ToInt32(brokerBalance.CurrencyCode),
                LastBalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate),
                PartialBalanceAmount = Convert.ToDecimal(brokerBalance.PartialBalanceAmount),
                PartialBalanceIncomeAmount = Convert.ToDecimal(brokerBalance.PartialBalanceIncomeAmount),
                TaxPartialSum = Convert.ToDecimal(brokerBalance.TaxPartialSum),
                TaxPartialSubtraction = Convert.ToDecimal(brokerBalance.TaxPartialSubtraction),
                TaxSum = Convert.ToDecimal(brokerBalance.TaxSum),
                TaxSubtraction = Convert.ToDecimal(brokerBalance.TaxSubtraction),
                NumSheet = Convert.ToInt32(brokerBalance.NumSheet)
            };
        }

        /// <summary>
        /// CreateBrokerBalances
        /// </summary>
        /// <returns>List<BrokerBalanceDTO/></returns>
        public static List<BrokerBalanceDTO> CreateBrokerBalances(BusinessCollection businessCollection)
        {
            List<BrokerBalanceDTO> brokerBalanceDTOs = new List<BrokerBalanceDTO>();

            foreach (ACCEN.BrokerBalance brokerBalanceEntity in businessCollection.OfType<ACCEN.BrokerBalance>())
            {
                brokerBalanceDTOs.Add(CreateBrokerBalance(brokerBalanceEntity));
            }
            return brokerBalanceDTOs;
        }

        #endregion

        #region CoinsuranceBalance

        /// <summary>
        /// CreateCoinsuranceBalance
        /// </summary>
        /// <param name="coinsuranceBalance"></param>
        /// <returns>CoinsuranceBalanceDTO</returns>
        public static CoinsuranceBalanceDTO CreateCoinsuranceBalance(ACCEN.CoinsuranceBalance coinsuranceBalance)
        {
            return new CoinsuranceBalanceDTO()
            {
                CoinsuranceBalanceId = coinsuranceBalance.CoinsuranceBalanceCode,
                CoinsuredCompanyId = Convert.ToInt32(coinsuranceBalance.CoinsuredCompanyId),
                BalanceDate = Convert.ToDateTime(coinsuranceBalance.BalanceDate),
                CurrencyId = Convert.ToInt32(coinsuranceBalance.CurrencyCode),
                LastBalanceDate = Convert.ToDateTime(coinsuranceBalance.LastBalanceDate),
                BalanceAmount = Convert.ToDecimal(coinsuranceBalance.BalanceAmount),
                BalanceIncomeAmount = Convert.ToDecimal(coinsuranceBalance.BalanceIncomeAmount),
                NumSheet = Convert.ToInt32(coinsuranceBalance.NumSheet)
            };
        }

        /// <summary>
        /// CreateCoinsuranceBalances
        /// </summary>
        /// <returns>List<CoinsuranceBalanceDTO/></returns>
        public static List<CoinsuranceBalanceDTO> CreateCoinsuranceBalances(BusinessCollection businessCollection)
        {
            List<CoinsuranceBalanceDTO> coinsuranceBalanceDTOs = new List<CoinsuranceBalanceDTO>();

            foreach (ACCEN.CoinsuranceBalance coinsuranceBalanceEntities in businessCollection.OfType<ACCEN.CoinsuranceBalance>())
            {
                coinsuranceBalanceDTOs.Add(CreateCoinsuranceBalance(coinsuranceBalanceEntities));
            }
            return coinsuranceBalanceDTOs;
        }

        #endregion

        #region CollectMassiveProcess

        /// <summary>
        /// CreateCollectMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        public static CollectMassiveProcessDTO CreateCollectMassiveProcess(ACCEN.CollectMassiveProcess collectMassiveProcess)
        {
            return new CollectMassiveProcessDTO()
            {
                CollectMassiveProcessId = Convert.ToInt32(collectMassiveProcess.CollectMassiveProcessId),
                BeginDate = Convert.ToDateTime(collectMassiveProcess.BeginDate),
                EndDate = Convert.ToDateTime(collectMassiveProcess.EndDate),
                UserId = Convert.ToInt32(collectMassiveProcess.UserId),
                Status = Convert.ToBoolean(collectMassiveProcess.Status),
                TotalRecords = Convert.ToInt32(collectMassiveProcess.TotalRecords),
                FailedRecords = Convert.ToInt32(collectMassiveProcess.FailedRecords),
                SuccessfulRecords = Convert.ToInt32(collectMassiveProcess.SuccessfulRecords)
            };
        }

        /// <summary>
        /// CreateCollectMassiveProcesses
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CollectMassiveProcessDTO/></returns>
        public static List<CollectMassiveProcessDTO> CreateCollectMassiveProcesses(BusinessCollection businessCollection)
        {
            List<CollectMassiveProcessDTO> collectMassiveProcessDTOs = new List<CollectMassiveProcessDTO>();

            foreach (ACCEN.CollectMassiveProcess collectMassiveProcessEntities in businessCollection.OfType<ACCEN.CollectMassiveProcess>())
            {
                collectMassiveProcessDTOs.Add(CreateCollectMassiveProcess(collectMassiveProcessEntities));
            }
            return collectMassiveProcessDTOs;
        }

        #endregion

        #region TempPaymentRequestClaim

        /// <summary>
        /// CreateTempPaymentRequestClaim
        /// </summary>
        /// <param name="paymentRequestClaim"></param>
        /// <returns>PaymentRequest</returns>
        public static PAYMOD.PaymentRequest CreateTempPaymentRequestClaim(PAYENT.TempPaymentRequestClaim paymentRequestClaim)
        {
            PAYMOD.PaymentRequest paymentRequestInfo = new PAYMOD.PaymentRequest();
            paymentRequestInfo.Id = paymentRequestClaim.PaymentRequestCode;

            CLMMOD.Claim claim = new CLMMOD.Claim()
            {
                Modifications = new List<CLMMOD.ClaimModify>()
                {
                    new CLMMOD.ClaimModify()
                    {
                        Coverages = new List<CLMMOD.ClaimCoverage>()
                        {
                            new CLMMOD.ClaimCoverage()
                            {
                                CoverageId = paymentRequestClaim.ClaimCode,
                                SubClaim = paymentRequestClaim.SubClaim,
                            }
                        }
                    }
                },
                Id = paymentRequestClaim.ClaimCode,
            };

            List<PAYMOD.Voucher> vouchers = new List<PAYMOD.Voucher>()
            {
                new PAYMOD.Voucher()
                {
                    Concepts = new List<PAYMOD.VoucherConcept>()
                    {
                        new PAYMOD.VoucherConcept()
                        {
                            Id = Convert.ToInt32(paymentRequestClaim.VoucherConceptCode)
                        }
                    }
                }
            };

            return new PAYMOD.PaymentRequest()
            {
                Claim = claim,
                Vouchers = vouchers,
            };
        }

        #endregion

        #region Automatic Debit

        /// <summary>
        /// CreateNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public static BankNetwork CreateNetwork(ACCEN.BankNetwork bankNetwork)
        {
            return new BankNetwork()
            {
                Id = bankNetwork.BankNetworkId,
                HasTax = Convert.ToBoolean(bankNetwork.Tax),
                Description = bankNetwork.Description != null ? Convert.ToString(bankNetwork.Description) : "",
                RequiresNotification = Convert.ToBoolean(bankNetwork.Prenotification),
            };
        }

        /// <summary>
        /// CreatePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public static PaymentMethodAccountType CreatePaymentMethodAccountType(ACCEN.PaymentMethodAccountType paymentMethodAccountType)
        {
            return new PaymentMethodAccountType()
            {
                Id = 0,
                BankAccountType = new BankAccountType()
                {
                    Description = ""
                }
            };
        }

        /// <summary>
        /// CreatePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public static PaymentMethodBankNetwork CreatePaymentMethodBankNetwork(ACCEN.PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return new PaymentMethodBankNetwork()
            {
                BankNetwork = new BankNetwork()
                {
                    Id = paymentMethodBankNetwork.BankNetworkCode,
                    Description = paymentMethodBankNetwork.Identifier
                },
                PaymentMethod = new PaymentsModels.PaymentMethod()
                {
                    Id = paymentMethodBankNetwork.PaymentMethodCode
                },
                BankAccountCompany = new BankAccountCompany()
                {
                    Id = paymentMethodBankNetwork.AccountBankCode
                },

                ToGenerate = (bool)paymentMethodBankNetwork.Generate,
            };
        }

        #region LogBankResponse

        /// <summary>
        /// CreateLogBankResponses
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Array/></returns>
        public static List<Array> CreateLogBankResponses(BusinessCollection businessCollection)
        {
            List<Array> shipmentStatus = new List<Array>();
            foreach (ACCEN.AutomaticDebitStatus automaticDebitStatusEntity in businessCollection.OfType<ACCEN.AutomaticDebitStatus>())
            {
                shipmentStatus.Add(CreateShipmentStatus(automaticDebitStatusEntity));
            }
            return shipmentStatus;
        }

        /// <summary>
        /// CreateLogBankResponse
        /// </summary>
        /// <param name="logBankResponseEntity"></param>
        /// <returns>Array</returns>
        public static Array CreateLogBankResponse(ACCEN.LogBankResponse logBankResponseEntity)
        {
            String[] logBankResponse = new String[15];
            logBankResponse[0] = logBankResponseEntity.LogBankResponseId.ToString();
            logBankResponse[1] = logBankResponseEntity.BankNetworkId.ToString();
            logBankResponse[2] = logBankResponseEntity.LotNumber;
            logBankResponse[3] = logBankResponseEntity.LineNumber.ToString();
            logBankResponse[4] = logBankResponseEntity.CardAccountNumber;
            logBankResponse[5] = logBankResponseEntity.VoucherNumber;
            logBankResponse[6] = logBankResponseEntity.RejectionCode;
            logBankResponse[7] = logBankResponseEntity.ApplicationDate.ToString();
            logBankResponse[8] = logBankResponseEntity.PremiumAmount.ToString();
            logBankResponse[9] = logBankResponseEntity.RegisterDate.ToString();
            logBankResponse[10] = logBankResponseEntity.AuthorizationNumber;
            logBankResponse[11] = logBankResponseEntity.DocumentNumber;
            logBankResponse[12] = logBankResponseEntity.IsPrenotificacion.ToString();
            logBankResponse[13] = logBankResponseEntity.DescriptionError;
            logBankResponse[14] = logBankResponseEntity.UserCode.ToString();

            return logBankResponse;
        }

        #endregion

        #region ShipmentStatus

        /// <summary>
        /// CreateAutomaticDebitStatus
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Array/></returns>
        public static List<Array> CreateAutomaticDebitStatus(BusinessCollection businessCollection)
        {
            List<Array> automaticDebitStatus = new List<Array>();
            foreach (ACCEN.AutomaticDebitStatus automaticDebitStatusEntity in businessCollection.OfType<ACCEN.AutomaticDebitStatus>())
            {
                automaticDebitStatus.Add(CreateShipmentStatus(automaticDebitStatusEntity));
            }
            return automaticDebitStatus;
        }

        /// <summary>
        /// CreateShipmentStatus
        /// </summary>
        /// <param name="automaticDebitStatusEntity"></param>
        /// <returns>Array</returns>
        public static Array CreateShipmentStatus(ACCEN.AutomaticDebitStatus automaticDebitStatusEntity)
        {
            String[] shipmentStatus = new String[2];
            shipmentStatus[0] = automaticDebitStatusEntity.AutomaticDebitStatusId.ToString();
            shipmentStatus[1] = automaticDebitStatusEntity.Description;

            return shipmentStatus;
        }

        #endregion

        /// <summary>
        /// CreateDebitDesign
        /// </summary>
        /// <param name="debitDesign"></param>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public static Format CreateDebitDesign(ACCEN.DebitDesign debitDesign, Format format)
        {
            return new Format()
            {
                Id = debitDesign.DebitDesignId,
                Description = "",
                FormatType = format.FormatType,
                FormatUsingType = format.FormatUsingType,
                Separator = format.Separator
            };
        }

        /// <summary>
        /// CreateFormatDesign
        /// </summary>
        /// <param name="formatDesign"></param>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public static Format CreateFormatDesign(ACCEN.FormatDesign formatDesign, Format format)
        {
            return new Format()
            {
                Id = formatDesign.FormatDesignId,
                Description = "",
                FormatType = format.FormatType,
                FormatUsingType = format.FormatUsingType
            };
        }

        /// <summary>
        /// CreateFormatDesignDetail
        /// </summary>
        /// <param name="formatDesignDetail"></param>
        /// <param name="format"></param>
        /// <returns>Format</returns>
        public static Format CreateFormatDesignDetail(ACCEN.FormatDesignDetail formatDesignDetail, Format format)
        {
            return new Format()
            {
                Id = 0,
                Description = "",
                FormatType = format.FormatType,
                FormatUsingType = format.FormatUsingType
            };
        }

        #endregion

        #region LogSpecialProcess

        /// <summary>
        /// CreateLogSpecialProcess
        /// </summary>
        /// <param name="logSpecialProcess"></param>
        /// <returns>CreditNote</returns>
        public static CreditNote CreateLogSpecialProcess(ACCEN.GetAutomaticCreditNoteProcessHeader logSpecialProcess)
        {
            CreditNote creditNote = new CreditNote();

            COMMMOD.Amount posAmount = new COMMMOD.Amount() { Value = logSpecialProcess.IncomeAmount };
            COMMMOD.Amount negAmount = new COMMMOD.Amount() { Value = logSpecialProcess.IncomeAmount * -1 };

            CreditNoteItem creditNoteItem = new CreditNoteItem();
            creditNoteItem.NegativePolicy = new Policy()
            {
                CurrentTo = Convert.ToDateTime(logSpecialProcess.EndDate),
                Endorsement = new Endorsement() { Id = Convert.ToInt32(logSpecialProcess.ImputationReceiptNumber) },
                Id = logSpecialProcess.TempImputationId
            };
            creditNoteItem.Id = Convert.ToInt32(logSpecialProcess.RecordsProcessed);

            List<CreditNoteItem> creditNoteItems = new List<CreditNoteItem>();
            creditNoteItems.Add(creditNoteItem);

            creditNote.Id = logSpecialProcess.LogSpecialProcessId;
            creditNote.Date = logSpecialProcess.ProcessDate;
            creditNote.PositiveAppliedTotal = posAmount;
            creditNote.NegativeAppliedTotal = negAmount;
            creditNote.UserId = logSpecialProcess.UserId;

            switch (logSpecialProcess.Status)
            {
                case 1:
                    creditNote.CreditNoteStatus = CreditNoteStatus.Actived;
                    break;
                case 2:
                    creditNote.CreditNoteStatus = CreditNoteStatus.Applied;
                    break;
                case 3:
                    creditNote.CreditNoteStatus = CreditNoteStatus.Rejected;
                    break;
                case 4:
                    creditNote.CreditNoteStatus = CreditNoteStatus.NoData;
                    break;
            }

            creditNote.CreditNoteItems = creditNoteItems;

            return creditNote;
        }

        /// <summary>
        /// CreateLogsSpecialProcess
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CreditNote/></returns>
        public static List<CreditNote> CreateLogsSpecialProcess(BusinessCollection businessCollection)
        {
            List<CreditNote> creditNotes = new List<CreditNote>();

            foreach (ACCEN.GetAutomaticCreditNoteProcessHeader logSpecialProcessEntity in businessCollection.OfType<ACCEN.GetAutomaticCreditNoteProcessHeader>())
            {
                creditNotes.Add(CreateLogSpecialProcess(logSpecialProcessEntity));
            }
            return creditNotes;
        }

        /// <summary>
        /// CreateLogAmortizationProcess
        /// </summary>
        /// <param name="logSpecialProcess"></param>
        /// <returns>CreditNote</returns>
        public static Amortization CreateLogAmortizationProcess(ACCEN.GetAutomaticCreditNoteProcessHeader logSpecialProcess)
        {
            Amortization amortization = new Amortization();

            COMMMOD.Amount positiveAmount = new COMMMOD.Amount();
            positiveAmount.Value = logSpecialProcess.Amount;

            COMMMOD.Amount negativeAmount = new COMMMOD.Amount();
            negativeAmount.Value = logSpecialProcess.Amount * -1;

            switch (logSpecialProcess.Status)
            {
                case 1:
                    amortization.AmortizationStatus = AmortizationStatus.Actived;
                    break;
                case 2:
                    amortization.AmortizationStatus = AmortizationStatus.Applied;
                    break;
                case 3:
                    amortization.AmortizationStatus = AmortizationStatus.Rejected;
                    break;
                case 4:
                    amortization.AmortizationStatus = AmortizationStatus.NoData;
                    break;
            }

            amortization.Date = logSpecialProcess.ProcessDate;
            amortization.Id = logSpecialProcess.LogSpecialProcessId;
            amortization.NegativeAppliedTotal = negativeAmount;

            List<Policy> policies = new List<Policy>();
            Policy policy = new Policy();
            policy.CurrentTo = Convert.ToDateTime(logSpecialProcess.EndDate);
            policy.Id = Convert.ToInt32(logSpecialProcess.RecordsProcessed);
            policy.EffectPeriod = logSpecialProcess.TempImputationId;
            policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(logSpecialProcess.ImputationReceiptNumber) };
            policies.Add(policy);

            amortization.Policies = policies;
            amortization.PositiveAppliedTotal = positiveAmount;
            amortization.UserId = logSpecialProcess.UserId;

            return amortization;
        }

        /// <summary>
        /// CreateLogsAmortizationProcess
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CreditNote/></returns>
        public static List<Amortization> CreateLogsAmortizationProcess(BusinessCollection businessCollection)
        {
            List<Amortization> amortizations = new List<Amortization>();

            foreach (ACCEN.GetAutomaticCreditNoteProcessHeader logSpecialProcessEntity in businessCollection.OfType<ACCEN.GetAutomaticCreditNoteProcessHeader>())
            {
                amortizations.Add(CreateLogAmortizationProcess(logSpecialProcessEntity));
            }
            return amortizations;
        }

        #endregion

        #region Range

        /// <summary>
        /// CreateRanges
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Range/></returns>
        public static List<Range> CreateRanges(BusinessCollection businessCollection)
        {
            List<Range> ranges = new List<Range>();
            foreach (ACCEN.Range range in businessCollection.OfType<ACCEN.Range>())
            {
                ranges.Add(new Range()
                {
                    Id = range.RangeCode,
                    Description = range.Description,
                    IsDefault = range.RangeDefault,
                });
            }

            return ranges;
        }

        #endregion

        #region CancellationPolicies

        #region CancellationLimit

        /// <summary>
        /// GetCancelationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimit</returns>
        public static CancellationLimit GetCancelationLimit(ACCEN.CancellationLimit cancellationLimit)
        {
            return new CancellationLimit()
            {
                Id = cancellationLimit.CancellationLimitId,
                CancellationLimitDays = cancellationLimit.CancellationLimitDays
            };
        }

        /// <summary>
        /// CreateCancellationLimits
        /// </summary>
        /// <param name="cancellationDayPrefixCollection"></param>
        /// <returns>List<CancellationLimit/></returns>
        public static List<CancellationLimit> CreateCancellationLimits(UIView cancellationDayPrefixCollection)
        {
            List<CancellationLimit> cancellationLimits = new List<CancellationLimit>();
            foreach (DataRow dataRow in cancellationDayPrefixCollection)
            {
                CancellationLimit cancellationLimit = new CancellationLimit();
                COMMMOD.Branch branch = new COMMMOD.Branch()
                {
                    Description = dataRow["Description"].ToString(),
                    Id = Convert.ToInt32(dataRow["PrefixCode"])
                };

                cancellationLimit.Id = Convert.ToInt32(dataRow["CancellationLimitId"]);
                cancellationLimit.Branch = branch;
                cancellationLimit.CancellationLimitDays = Convert.ToInt32(dataRow["CancellationLimitDays"]);
                cancellationLimits.Add(cancellationLimit);
            }
            return cancellationLimits;
        }

        #endregion

        #region CancelletionExclusion

        /// <summary>
        /// GetConcept
        /// </summary>
        /// <param name="exclusionEntity"></param>
        /// <returns>Exclusion</returns>
        public static Exclusion GetExclusion(ACCEN.Exclusion exclusionEntity)
        {
            Exclusion exclusion = new Exclusion();
            Person agent = new Person();
            Person insured = new Person();
            Policy policy = new Policy();

            exclusion.Id = exclusionEntity.ExclusionId;
            exclusion.Agent = agent;
            exclusion.Insured = insured;
            exclusion.Policy = policy;

            return exclusion;
        }

        /// <summary>
        /// CreateExclusionList
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <param name="exclusionType"></param>
        /// <returns>List<Exclusion/></returns>
        public static List<Exclusion> CreateExclusions(BusinessCollection businessCollection, int exclusionType)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch();
            COMMMOD.Prefix prefix = new COMMMOD.Prefix();

            List<Exclusion> exclusions = new List<Exclusion>();

            foreach (ACCEN.GetExclusion getExclusion in businessCollection.OfType<ACCEN.GetExclusion>())
            {
                Exclusion exclusion = new Exclusion();
                Policy policy = new Policy();
                Person agent = new Person();
                Person insured = new Person();

                exclusion.Id = getExclusion.ExclusionId;

                if (exclusionType == 1)
                {
                    policy.DocumentNumber = Convert.ToInt32(getExclusion.DocumentNum);
                    branch.Id = Convert.ToInt32(getExclusion.BranchCode);
                    branch.Description = getExclusion.BranchDescription;
                    prefix.Id = Convert.ToInt32(getExclusion.PrefixCode);
                    prefix.Description = getExclusion.PrefixDescription;

                    policy.Branch = branch;
                    policy.Prefix = prefix;
                }
                else if (exclusionType == 2)
                {
                    agent.IndividualId = Convert.ToInt32(getExclusion.IndividualId);
                    agent.Name = getExclusion.AgentName;
                    agent.IdentificationDocument = new IdentificationDocument() { Number = getExclusion.AgentDocumentNum };
                }
                else
                {
                    insured.IndividualId = Convert.ToInt32(getExclusion.IndividualId);
                    insured.Name = getExclusion.InsuredName;
                    insured.IdentificationDocument = new IdentificationDocument() { Number = getExclusion.InsuredDocumentNum };
                }

                exclusion.Agent = agent;
                exclusion.Insured = insured;
                exclusion.Policy = policy;
                exclusions.Add(exclusion);
            }

            return exclusions;
        }

        #endregion

        #endregion

        #region AccountType

        /// <summary>
        /// CreateAccountType
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns>BankAccountType</returns>
        public static BankAccountType CreateAccountType(COMMENT.AccountType accountType)
        {
            return new BankAccountType()
            {
                Id = accountType.AccountTypeCode,
                Description = accountType.Description,
                IsEnabled = Convert.ToBoolean(accountType.Enabled)
            };
        }

        /// <summary>
        /// CreateAccountTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BankAccountType/></returns>
        public static List<BankAccountType> CreateAccountTypes(BusinessCollection businessCollection)
        {
            List<BankAccountType> bankAccountTypes = new List<BankAccountType>();

            foreach (COMMENT.AccountType accountTypeEntity in businessCollection.OfType<COMMENT.AccountType>())
            {
                bankAccountTypes.Add(CreateAccountType(accountTypeEntity));
            }

            return bankAccountTypes;
        }

        #endregion AccountType

        #region AccountBank

        /// <summary>
        /// CreateAccountBank
        /// </summary>
        /// <param name="accountBank"></param>
        /// <returns>AccountBank</returns>
        public static BankAccountPerson CreateBankAccountPerson(UPENT.AccountBank accountBank)
        {
            BankAccountType bankAccountType = new BankAccountType() { Id = Convert.ToInt16(accountBank.AccountTypeCode) };
            Individual individual = new Individual() { IndividualId = Convert.ToInt32(accountBank.IndividualId) };

            return new BankAccountPerson()
            {
                Id = accountBank.AccountBankCode,
                BankAccountType = bankAccountType,
                Number = accountBank.Number,
                IsDefault = Convert.ToBoolean(accountBank.Default),
                IsEnabled = Convert.ToBoolean(accountBank.Enabled),
                Individual = individual
            };
        }

        /// <summary>
        /// CreatePersonBankAccounts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Payment/></returns>
        public static List<BankAccountPerson> CreatePersonBankAccounts(BusinessCollection businessCollection)
        {
            List<BankAccountPerson> personBankAccounts = new List<BankAccountPerson>();
            foreach (UPENT.AccountBank personBankAccountEntity in businessCollection.OfType<UPENT.AccountBank>())
            {
                personBankAccounts.Add(CreateBankAccountPerson(personBankAccountEntity));
            }
            return personBankAccounts;
        }

        #endregion

        #region PaymentMethodType

        /// <summary>
        /// CreatePaymentMethodType
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <returns>PaymentMethod</returns>
        public static PaymentsModels.PaymentMethod CreatePaymentMethodType(PARAMENT.PaymentMethodType paymentMethodType)
        {
            return new PaymentsModels.PaymentMethod()
            {
                Id = paymentMethodType.PaymentMethodTypeCode,
                Description = paymentMethodType.Description
            };
        }

        /// <summary>
        /// CreatePaymentMethodTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns> List<PaymentMethod/></returns>
        public static List<PaymentsModels.PaymentMethod> CreatePaymentMethodTypes(BusinessCollection businessCollection)
        {
            List<PaymentsModels.PaymentMethod> paymentMethods = new List<PaymentsModels.PaymentMethod>();
            foreach (PARAMENT.PaymentMethodType paymentMethodTypeEntity in businessCollection.OfType<PARAMENT.PaymentMethodType>())
            {
                paymentMethods.Add(CreatePaymentMethodType(paymentMethodTypeEntity));
            }
            return paymentMethods;
        }

        #endregion PaymentMethodType

        #region VoucherType

        /// <summary>
        /// CreateVoucherType
        /// </summary>
        /// <param name="voucherType"></param>
        /// <returns>VoucherType</returns>
        public static Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherType CreateVoucherType(PARAMENT.VoucherType voucherType)
        {
            return new Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherType()
            {
                Id = voucherType.VoucherTypeCode,
                Description = voucherType.Description
            };
        }

        /// <summary>
        /// CreateVoucherTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<VoucherType/></returns>
        public static List<Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherType> CreateVoucherTypes(BusinessCollection businessCollection)
        {
            List<Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherType> voucherTypes = new List<Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherType>();

            foreach (PARAMENT.VoucherType voucherTypeEntity in businessCollection.OfType<PARAMENT.VoucherType>())
            {
                voucherTypes.Add(CreateVoucherType(voucherTypeEntity));
            }

            return voucherTypes;
        }

        #endregion VoucherType

        #region TempVoucher

        /// <summary>
        /// CreateTempVoucher
        /// </summary>
        /// <param name="tempVoucher"></param>
        /// <returns>Voucher</returns>
        public static Models.AccountsPayables.Voucher CreateTempVoucher(COMMENT.TempVoucher tempVoucher)
        {
            Models.AccountsPayables.VoucherType voucherType = new Models.AccountsPayables.VoucherType() { Id = (int)(tempVoucher.VoucherTypeCode) };
            COMMMOD.Currency currency = new COMMMOD.Currency() { Id = (int)(tempVoucher.CurrencyCode) };

            return new Models.AccountsPayables.Voucher()
            {
                Id = tempVoucher.VoucherCode,
                Type = voucherType,
                Number = tempVoucher.Number,
                Date = (DateTime)(tempVoucher.Date),
                Currency = currency,
                ExchangeRate = Convert.ToDecimal(tempVoucher.ExchangeRate)
            };
        }

        #endregion TempVoucher

        #region BankAccountCompany

        /// <summary>
        /// CreateCompanyBankAccount
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>BankAccountCompany</returns>
        public static BankAccountCompany CreateCompanyBankAccount(ACCEN.BankAccountCompany bankAccountCompany)
        {
            COMMMOD.Bank bank = new COMMMOD.Bank() { Id = Convert.ToInt32(bankAccountCompany.BankCode) };
            BankAccountType bankAccountType = new BankAccountType() { Id = Convert.ToInt32(bankAccountCompany.AccountTypeCode) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(bankAccountCompany.BranchCode) };
            COMMMOD.Currency currency = new COMMMOD.Currency() { Id = Convert.ToInt32(bankAccountCompany.CurrencyCode) };
            Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingAccount accountingAccount =
                new Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingAccount() {
                    AccountingAccountId = Convert.ToInt32(bankAccountCompany.AccountingAccountId)
                };

            return new BankAccountCompany()
            {
                Bank = bank,
                BankAccountType = bankAccountType,
                Branch = branch,
                Currency = currency,
                DisableDate = Convert.ToDateTime(bankAccountCompany.DisabledDate),
                Id = bankAccountCompany.BankAccountCompanyId,
                IsDefault = Convert.ToBoolean(bankAccountCompany.Default),
                IsEnabled = Convert.ToBoolean(bankAccountCompany.Enabled),
                Number = bankAccountCompany.AccountNumber,
                AccountingAccount = accountingAccount
            };
        }

        /// <summary>
        /// CreateCompanyBankAccounts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Payment/></returns>
        public static List<BankAccountCompany> CreateCompanyBankAccounts(BusinessCollection businessCollection)
        {
            List<BankAccountCompany> companyBankAccounts = new List<BankAccountCompany>();
            foreach (ACCEN.BankAccountCompany bankAccountCompanyEntity in businessCollection.OfType<ACCEN.BankAccountCompany>())
            {
                companyBankAccounts.Add(CreateCompanyBankAccount(bankAccountCompanyEntity));
            }
            return companyBankAccounts;
        }

        #endregion

        #region PaymentRequestNumber

        /// <summary>
        /// CreatePaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns>PaymentRequestNumber</returns>
        public static PaymentRequestNumber CreatePaymentRequestNumber(ACCEN.PaymentRequestNumber paymentRequestNumber)
        {
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(paymentRequestNumber.BranchCode) };

            return new PaymentRequestNumber()
            {
                Branch = branch,
                Number = Convert.ToInt32(paymentRequestNumber.Number)
            };
        }

        /// <summary>
        /// CreatePaymentRequestNumbers
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentRequestNumber/></returns>
        public static List<PaymentRequestNumber> CreatePaymentRequestNumbers(BusinessCollection businessCollection)
        {
            List<PaymentRequestNumber> paymentRequestNumbers = new List<PaymentRequestNumber>();

            foreach (ACCEN.PaymentRequestNumber paymentRequestNumberEntity in businessCollection.OfType<ACCEN.PaymentRequestNumber>())
            {
                paymentRequestNumbers.Add(CreatePaymentRequestNumber(paymentRequestNumberEntity));
            }

            return paymentRequestNumbers;
        }

        #endregion PaymentRequestNumber

        #region PaymentRequest

        /// <summary>
        /// CreatePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        public static Models.AccountsPayables.PaymentRequest CreatePaymentRequest(ACCEN.PaymentRequest paymentRequest)
        {
            PaymentRequestNumber paymentRequestNumber = new PaymentRequestNumber() { Number = Convert.ToInt32(paymentRequest.Number) };
            Company company = new Company() { IndividualId = Convert.ToInt32(paymentRequest.CompanyCode) };
            COMMMOD.Branch branch = new COMMMOD.Branch() { Id = Convert.ToInt32(paymentRequest.BranchCode) };
            COMMMOD.SalePoint salePoint = new COMMMOD.SalePoint() { Id = Convert.ToInt32(paymentRequest.SalePointCode) };
            PersonType personType = new PersonType() { Id = Convert.ToInt32(paymentRequest.PersonTypeCode) };
            Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(paymentRequest.BeneficiaryCode) };
            PaymentsModels.PaymentMethod paymentMethod = new PaymentsModels.PaymentMethod() { Id = Convert.ToInt32(paymentRequest.PaymentMethodTypeCode) };
            COMMMOD.Currency currency = new COMMMOD.Currency() { Id = Convert.ToInt32(paymentRequest.CurrencyCode) };
            COMMMOD.Amount totalAmount = new COMMMOD.Amount() { Value = Convert.ToDecimal(paymentRequest.TotalAmount) };
            EEProvider.Models.Claims.PaymentRequest.MovementType movementType = new EEProvider.Models.Claims.PaymentRequest.MovementType()
            {
                ConceptSource = new EEProvider.Models.Claims.PaymentRequest.ConceptSource
                {
                    Id = Convert.ToInt32(paymentRequest.ConceptSourceCode)
                },
                Id = Convert.ToInt32(paymentRequest.MovementTypeCode)
            };
            Transaction transaction = new Transaction()
            {
                TechnicalTransaction = Convert.ToInt32(paymentRequest.TechnicalTransaction)
            };

            return new Models.AccountsPayables.PaymentRequest()
            {
                Id = paymentRequest.PaymentRequestId,
                PaymentRequestType = (PaymentRequestTypes)paymentRequest.PaymentRequestType,
                PaymentRequestNumber = paymentRequestNumber,
                MovementType = movementType,
                Company = company,
                Branch = branch,
                SalePoint = salePoint,
                PersonType = personType,
                Beneficiary = beneficiary,
                PaymentMethod = paymentMethod,
                Currency = currency,
                TotalAmount = totalAmount,
                EstimatedDate = Convert.ToDateTime(paymentRequest.EstimatedPaymentDate),
                AccountingDate = Convert.ToDateTime(paymentRequest.AccountingDate),
                Description = paymentRequest.Description,
                UserId = Convert.ToInt32(paymentRequest.UserId),
                Transaction = transaction
            };
        }

        /// <summary>
        /// CreatePaymentRequests
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<PaymentRequest/></returns>
        public static List<Models.AccountsPayables.PaymentRequest> CreatePaymentRequests(BusinessCollection businessCollection)
        {
            List<Models.AccountsPayables.PaymentRequest> paymentRequests = new List<Models.AccountsPayables.PaymentRequest>();

            foreach (ACCEN.PaymentRequest paymentRequestEntity in businessCollection.OfType<ACCEN.PaymentRequest>())
            {
                paymentRequests.Add(CreatePaymentRequest(paymentRequestEntity));
            }

            return paymentRequests;
        }

        #endregion PaymentRequest

        #region Voucher

        /// <summary>
        /// CreateVoucher
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns>Voucher</returns>
        public static Models.AccountsPayables.Voucher CreateVoucher(ACCEN.Voucher voucher)
        {
            Models.AccountsPayables.VoucherType type = new Models.AccountsPayables.VoucherType() { Id = Convert.ToInt32(voucher.VoucherTypeCode) };
            COMMMOD.Currency currency = new COMMMOD.Currency() { Id = Convert.ToInt32(voucher.CurrencyCode) };

            return new Models.AccountsPayables.Voucher()
            {
                Id = voucher.VoucherId,
                Type = type,
                Number = voucher.Number,
                Date = Convert.ToDateTime(voucher.Date),
                Currency = currency,
                ExchangeRate = Convert.ToDecimal(voucher.ExchangeRate)
            };
        }

        /// <summary>
        /// CreateVouchers
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Voucher/></returns>
        public static List<Models.AccountsPayables.Voucher> CreateVouchers(BusinessCollection businessCollection)
        {
            List<Models.AccountsPayables.Voucher> vouchers = new List<Models.AccountsPayables.Voucher>();

            foreach (ACCEN.Voucher voucherEntity in businessCollection.OfType<ACCEN.Voucher>())
            {
                vouchers.Add(CreateVoucher(voucherEntity));
            }

            return vouchers;
        }

        #endregion Voucher

        #region VoucherConcept

        /// <summary>
        /// CreateVoucherConcept
        /// </summary>
        /// <param name="voucherConcept"></param>
        /// <returns>VoucherConcept</returns>
        public static Models.AccountsPayables.VoucherConcept CreateVoucherConcept(ACCEN.VoucherConcept voucherConcept)
        {
            GeneralLedgerServices.EEProvider.Models.AccountingConcepts.AccountingConcept accountingConcept =
                new GeneralLedgerServices.EEProvider.Models.AccountingConcepts.AccountingConcept() {
                    Id = Convert.ToInt32(voucherConcept.AccountingConceptCode)
                };
            Models.Imputations.CostCenter costCenter = new Models.Imputations.CostCenter() { CostCenterId = Convert.ToInt32(voucherConcept.CostCenterCode) };
            COMMMOD.Amount amount = new COMMMOD.Amount() { Value = Convert.ToDecimal(voucherConcept.Amount) };

            return new Models.AccountsPayables.VoucherConcept()
            {
                Id = voucherConcept.VoucherConceptId,
                AccountingConcept = accountingConcept,
                CostCenter = costCenter,
                Amount = amount
            };
        }

        /// <summary>
        /// CreateVoucherConcepts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<VoucherConcept/></returns>
        public static List<Models.AccountsPayables.VoucherConcept> CreateVoucherConcepts(BusinessCollection businessCollection)
        {
            List<Models.AccountsPayables.VoucherConcept> voucherConcepts = new List<Models.AccountsPayables.VoucherConcept>();

            foreach (ACCEN.VoucherConcept voucherConceptEntity in businessCollection.OfType<ACCEN.VoucherConcept>())
            {
                voucherConcepts.Add(CreateVoucherConcept(voucherConceptEntity));
            }

            return voucherConcepts;
        }

        #endregion VoucherConcept

        #region VoucherConceptTax

        /// <summary>
        /// CreateVoucherConceptTax
        /// </summary>
        /// <param name="voucherConceptTax"></param>
        /// <returns>VoucherConceptTax</returns>
        public static Models.AccountsPayables.VoucherConceptTax CreateVoucherConceptTax(ACCEN.VoucherConceptTax voucherConceptTax)
        {
            TaxServices.Models.Tax tax = new TaxServices.Models.Tax() { Id = Convert.ToInt32(voucherConceptTax.TaxCode) };
            TaxServices.Models.TaxCondition taxCondition = new TaxServices.Models.TaxCondition() { Id = Convert.ToInt32(voucherConceptTax.TaxConditionCode) };
            TaxServices.Models.TaxCategory taxCategory = new TaxServices.Models.TaxCategory() { Id = Convert.ToInt32(voucherConceptTax.TaxCategoryCode) };

            return new Models.AccountsPayables.VoucherConceptTax()
            {
                Id = voucherConceptTax.VoucherConceptTaxId,
                Tax = tax,
                TaxCondition = taxCondition,
                TaxCategory = taxCategory,
                TaxeRate = Convert.ToDecimal(voucherConceptTax.TaxRate),
                TaxeBaseAmount = Convert.ToDecimal(voucherConceptTax.TaxBaseAmount),
                TaxValue = Convert.ToDecimal(voucherConceptTax.TaxValue)
            };
        }

        /// <summary>
        /// CreateVoucherConceptTaxes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<VoucherConceptTax/></returns>
        public static List<Models.AccountsPayables.VoucherConceptTax> CreateVoucherConceptTaxes(BusinessCollection businessCollection)
        {
            List<Models.AccountsPayables.VoucherConceptTax> voucherConceptTaxes = new List<Models.AccountsPayables.VoucherConceptTax>();

            foreach (ACCEN.VoucherConceptTax voucherConceptTaxEntity in businessCollection.OfType<ACCEN.VoucherConceptTax>())
            {
                voucherConceptTaxes.Add(CreateVoucherConceptTax(voucherConceptTaxEntity));
            }

            return voucherConceptTaxes;
        }

        #endregion VoucherConceptTax

        #region CreditCardType

        /// <summary>
        /// CreateCreditCardType
        /// </summary>
        /// <param name="creditCardType"></param>
        /// <returns>CreditCardType</returns>
        public static CreditCardType CreateCreditCardType(COMMENT.CreditCardType creditCardType)
        {
            return new CreditCardType()
            {
                Commission = Convert.ToDecimal(creditCardType.Commission),
                Description = creditCardType.Description,
                Id = creditCardType.CreditCardTypeCode
            };
        }

        /// <summary>
        /// CreateCreditCardTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CreditCardType/></returns>
        public static List<CreditCardType> CreateCreditCardTypes(BusinessCollection businessCollection)
        {
            List<CreditCardType> creditCardTypes = new List<CreditCardType>();
            foreach (COMMENT.CreditCardType creditCardTypeEntity in businessCollection.OfType<COMMENT.CreditCardType>())
            {
                creditCardTypes.Add(CreateCreditCardType(creditCardTypeEntity));
            }
            return creditCardTypes;
        }

        #endregion

        #region RetentionBase

        /// <summary>
        /// CreateRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>RetentionBase</returns>
        public static RetentionBase CreateRetentionBase(ACCEN.RetentionBase retentionBase)
        {
            return new RetentionBase()
            {
                Id = retentionBase.RetentionBaseCode,
                Description = retentionBase.Description
            };
        }

        /// <summary>
        /// CreateRetentionBases
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<AccoutingModels.RetentionBase/></returns>
        public static List<RetentionBase> CreateRetentionBases(BusinessCollection businessCollection)
        {
            List<RetentionBase> retentionBases = new List<RetentionBase>();
            foreach (ACCEN.RetentionBase retentionBaseEntity in businessCollection.OfType<ACCEN.RetentionBase>())
            {
                retentionBases.Add(CreateRetentionBase(retentionBaseEntity));
            }
            return retentionBases;
        }

        #endregion

        #region RetentionConcept

        /// <summary>
        /// CreateRetentionConcepts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<RetentionConcept/></returns>
        public static List<RetentionConcept> CreateRetentionConcepts(BusinessCollection businessCollection)
        {
            List<RetentionConcept> retentionConcepts = new List<RetentionConcept>();
            foreach (ACCEN.PerceivedRetention retentionConcept in businessCollection.OfType<ACCEN.PerceivedRetention>())
            {
                retentionConcepts.Add(new RetentionConcept()
                {
                    Id = retentionConcept.PerceivedRetentionId,
                    Description = retentionConcept.Description,
                    RetentionBase = new RetentionBase() { Id = retentionConcept.RetentionBaseCode },
                    Status = Convert.ToInt32(retentionConcept.IsActive),
                    DifferenceAmount = retentionConcept.MaximumDifferenceTax
                });
            }

            return retentionConcepts;
        }

        #endregion

        #region RetentionConceptPercentage

        /// <summary>
        /// CreateRetentionConceptPercentages
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<RetentionConceptPercentage/></returns>
        public static List<RetentionConceptPercentage> CreateRetentionConceptPercentages(BusinessCollection businessCollection)
        {
            List<RetentionConceptPercentage> retentionConceptPercentages = new List<RetentionConceptPercentage>();
            foreach (ACCEN.PerceivedRetentionValidity retentionConceptPercentage in businessCollection.OfType<ACCEN.PerceivedRetentionValidity>())
            {
                retentionConceptPercentages.Add(new RetentionConceptPercentage()
                {
                    Id = retentionConceptPercentage.PerceivedRetentionValidityId,
                    RetentionConcept = new RetentionConcept() { Id = retentionConceptPercentage.PerceivedRetentionCode },
                    Percentage = retentionConceptPercentage.RetentionPercentage,
                    DateFrom = retentionConceptPercentage.ValidityFrom,
                    DateTo = retentionConceptPercentage.ValidityTo,
                    ExternalCode = retentionConceptPercentage.PerceivedRetentionCode2
                });
            }

            return retentionConceptPercentages;
        }

        #endregion

        #region AuthorizationPaymentOrder
        /// <summary>
        /// CreateAuthorizationPaymentOrder
        /// </summary>
        /// <param name="paymentOrderAuthorization"></param>
        /// <returns>PaymentOrder</returns>
        public static PaymentOrder CreateAuthorizationPaymentOrder(ACCEN.PaymentOrderAuthorization paymentOrderAuthorization)
        {
            return new PaymentOrder()
            {
                Id = paymentOrderAuthorization.PaymentOrderCode,
                Status = Convert.ToInt32(paymentOrderAuthorization.AuthorizationLevel),
                PaymentDate = Convert.ToDateTime(paymentOrderAuthorization.AuthorizationDate),
                UserId = Convert.ToInt32(paymentOrderAuthorization.AuthorizerUserId)
            };
        }
        #endregion

        #region PolicyComponentDistribution

        public static ApplicationPremiumComponent CreateApplicationPremiumComponent(ACCEN.ApplicationPremiumComponent entityApplicationPremiumComponent)
        {
            return new ApplicationPremiumComponent
            {
                PremiumId = entityApplicationPremiumComponent.AppPremiumCode,
                ComponentId = entityApplicationPremiumComponent.ComponentCode,
                CurrencyId = entityApplicationPremiumComponent.CurrencyCode,
                ExchangeRate = entityApplicationPremiumComponent.ExchangeRate,
                Amount = entityApplicationPremiumComponent.Amount,
                LocalAmount = entityApplicationPremiumComponent.LocalAmount,
                MainAmount = entityApplicationPremiumComponent.MainAmount,
                MainLocalAmount = entityApplicationPremiumComponent.MainLocalAmount,
                AppComponentId = entityApplicationPremiumComponent.AppComponentCode
            };
        }

        public static ApplicationPremiumComponentLBSB CreateApplicationPremiumComponentLbsb(ACCEN.ApplicationPremiumComponentLbsb entityApplicationPremiumComponentLBSB)
        {
            return new ApplicationPremiumComponentLBSB
            {
                ApplicationComponenLSBSId = entityApplicationPremiumComponentLBSB.AppComponentLbsbCode,
                ApplicationComponentId = entityApplicationPremiumComponentLBSB.AppComponentCode,
                LineBussinesId = entityApplicationPremiumComponentLBSB.LineBusinessCode,
                SubLineBussinesId = entityApplicationPremiumComponentLBSB.SubLineBusinessCode,
                CurrencyId = entityApplicationPremiumComponentLBSB.CurrencyCode,
                ExchangeRateId = entityApplicationPremiumComponentLBSB.ExchangeRate,
                Amount = entityApplicationPremiumComponentLBSB.Amount,
                LocalAmount = entityApplicationPremiumComponentLBSB.LocalAmount,
                MainAmount = entityApplicationPremiumComponentLBSB.MainAmount,
                MainLocalAmount = entityApplicationPremiumComponentLBSB.MainLocalAmount,
            };
        }

        public static List<ApplicationPremiumComponentLBSB> CreateApplicationPremiumComponentsLbsb(BusinessCollection entitiesApplicationPremiumComponentLBSB)
        {

            List<ApplicationPremiumComponentLBSB> applicationAccountings = new List<ApplicationPremiumComponentLBSB>();
            entitiesApplicationPremiumComponentLBSB.ForEach(x =>
            {
                applicationAccountings.Add(CreateApplicationPremiumComponentLbsb((ACCEN.ApplicationPremiumComponentLbsb)x));
            });
            return applicationAccountings;
        }

        public static ApplicationPremium CreateApplicationPremium(ACCEN.ApplicationPremium entityApplicationPremium)
        {
            return new ApplicationPremium
            {
                Id = entityApplicationPremium.AppPremiumCode,
                AccountingDate = entityApplicationPremium.AccountingDate,
                Amount = entityApplicationPremium.Amount,
                ApplicationId = entityApplicationPremium.AppCode,
                Currencyid = entityApplicationPremium.CurrencyCode,
                EndorsementId = entityApplicationPremium.EndorsementCode,
                ExchangeRate = entityApplicationPremium.ExchangeRate,
                IsCoinsurancePremiumPaid = Convert.ToBoolean(entityApplicationPremium.IsCoinsPremiumPaid),
                IsCommissionPaid = Convert.ToBoolean(entityApplicationPremium.IsCommissionPaid),
                LocalAmount = entityApplicationPremium.LocalAmount,
                MainAmount = Convert.ToDecimal(entityApplicationPremium.MainAmount),
                MainLocalAmount = Convert.ToDecimal(entityApplicationPremium.MainLocalAmount),
                PayerId = entityApplicationPremium.PayerCode,
                PaymentNumber = entityApplicationPremium.PaymentNum,
                QuotaStatusId = Convert.ToInt32(entityApplicationPremium.PremiumQuotaStatusCode),
                RegisterDate = entityApplicationPremium.RegisterDate,
                DiscountCommission = entityApplicationPremium.DiscountedCommission ?? 0
            };
        }

        public static List<ApplicationPremium> CreateApplicationPremiums(List<ACCEN.ApplicationPremium> entityApplicationPremium)
        {
            List<ApplicationPremium> applicationPremiums = new List<ApplicationPremium>();

            foreach (ACCEN.ApplicationPremium applicationPremium in entityApplicationPremium)
            {
                applicationPremiums.Add(CreateApplicationPremium(applicationPremium));
            }
            return (applicationPremiums);
        }

        public static ApplicationPremium CreateTempApplicationPremium(ACCEN.TempApplicationPremium entityTempApplicationPremium)
        {
            return new ApplicationPremium
            {
                Id = entityTempApplicationPremium.TempAppPremiumCode,
                AccountingDate = entityTempApplicationPremium.AccountingDate,
                Amount = entityTempApplicationPremium.Amount,
                ApplicationId = entityTempApplicationPremium.TempAppCode,
                Currencyid = entityTempApplicationPremium.CurrencyCode,
                EndorsementId = entityTempApplicationPremium.EndorsementCode,
                ExchangeRate = entityTempApplicationPremium.ExchangeRate,
                IsCoinsurancePremiumPaid = Convert.ToBoolean(entityTempApplicationPremium.IsCoinsPremiumPaid),
                IsCommissionPaid = Convert.ToBoolean(entityTempApplicationPremium.IsCommissionPaid),
                LocalAmount = entityTempApplicationPremium.LocalAmount,
                MainAmount = Convert.ToDecimal(entityTempApplicationPremium.MainAmount),
                MainLocalAmount = Convert.ToDecimal(entityTempApplicationPremium.MainLocalAmount),
                PayerId = entityTempApplicationPremium.PayerCode,
                PaymentNumber = entityTempApplicationPremium.PaymentNum,
                QuotaStatusId = Convert.ToInt32(entityTempApplicationPremium.PremiumQuotaStatusCode),
                RegisterDate = entityTempApplicationPremium.RegisterDate,
                DiscountCommission = entityTempApplicationPremium.DiscountedCommission ?? 0
            };
        }
        public static List<ApplicationPremium> CreateTempApplicationPremiums(List<ACCEN.TempApplicationPremium> entityApplicationPremium)
        {
            List<ApplicationPremium> applicationPremiums = new List<ApplicationPremium>();

            foreach (ACCEN.TempApplicationPremium tempApplicationPremium in entityApplicationPremium)
            {
                applicationPremiums.Add(CreateTempApplicationPremium(tempApplicationPremium));
            }
            return applicationPremiums;
        }

        public static TempApplicationPremium CreateTemporalApplicationPremium(ACCEN.TempApplicationPremium entityTempApplicationPremium)
        {
            return new TempApplicationPremium
            {
                Id = entityTempApplicationPremium.TempAppPremiumCode,
                AccountingDate = entityTempApplicationPremium.AccountingDate,
                Amount = entityTempApplicationPremium.Amount,
                ApplicationId = entityTempApplicationPremium.TempAppCode,
                Currencyid = entityTempApplicationPremium.CurrencyCode,
                EndorsementId = entityTempApplicationPremium.EndorsementCode,
                ExchangeRate = entityTempApplicationPremium.ExchangeRate,
                LocalAmount = entityTempApplicationPremium.LocalAmount,
                MainAmount = Convert.ToDecimal(entityTempApplicationPremium.MainAmount),
                MainLocalAmount = Convert.ToDecimal(entityTempApplicationPremium.MainLocalAmount),
                PayerId = entityTempApplicationPremium.PayerCode,
                PaymentNumber = entityTempApplicationPremium.PaymentNum,
                QuotaStatusId = Convert.ToInt32(entityTempApplicationPremium.PremiumQuotaStatusCode),
                RegisterDate = entityTempApplicationPremium.RegisterDate,
                DiscountedCommission = Convert.ToDecimal(entityTempApplicationPremium.DiscountedCommission)
            };
        }

        public static List<TempApplicationPremium> CreateTemporalApplicationPremiums(List<ACCEN.TempApplicationPremium> entityApplicationPremium)
        {
            List<TempApplicationPremium> tempApplicationPremiums = new List<TempApplicationPremium>();

            entityApplicationPremium.ForEach(x => {
                tempApplicationPremiums.Add(CreateTemporalApplicationPremium(x));
            });
            return tempApplicationPremiums;
        }

        public static ApplicationAccounting CreateApplicationAccounting(ACCEN.ApplicationAccounting entityApplicationAccounting)
        {
            return new ApplicationAccounting
            {
                Id = entityApplicationAccounting.AppAccountingCode,
                ApplicationAccountingId = Convert.ToInt32(entityApplicationAccounting.AccountingAccountCode),
                Description = entityApplicationAccounting.Description,
                Branch = new COMMMOD.Branch { Id = Convert.ToInt32(entityApplicationAccounting.BranchCode) },
                SalePoint = new COMMMOD.SalePoint { Id = Convert.ToInt32(entityApplicationAccounting.SalePointCode) },
                Beneficiary = new Individual { IndividualId = Convert.ToInt32(entityApplicationAccounting.IndividualCode) },
                AccountingNature = Convert.ToInt32(entityApplicationAccounting.AccountingNature),
                BookAccount = new BookAccount { Id = Convert.ToInt32(entityApplicationAccounting.AccountingAccountCode) },
                ReceiptDate = entityApplicationAccounting.ReceiptDate,
                BankReconciliationId = Convert.ToInt32(entityApplicationAccounting.BankReconciliationCode),
                AccountingConcept = new IMPMOD.AccountingConcept { Id = Convert.ToString(Convert.ToInt32(entityApplicationAccounting.AccountingConceptCode)) },
                Amount = new COMMMOD.Amount { Value = Convert.ToDecimal(entityApplicationAccounting.Amount) },
                LocalAmount = new COMMMOD.Amount { Value = Convert.ToDecimal(entityApplicationAccounting.LocalAmount) },
                ApplicationId = Convert.ToInt32(entityApplicationAccounting.AppCode),
                CurrencyId = Convert.ToInt32(entityApplicationAccounting.CurrencyCode),
                ExchangeRate = new COMMMOD.ExchangeRate { SellAmount = Convert.ToDecimal(entityApplicationAccounting.ExchangeRate) }
            };
        }

        public static ApplicationAccounting CreateTemporalApplicationAccounting(ACCEN.TempApplicationAccounting entityApplicationAccounting)
        {
            return new ApplicationAccounting
            {
                ApplicationAccountingId = entityApplicationAccounting.TempAppAccountingCode,
                Description = entityApplicationAccounting.Description,
                Branch = new COMMMOD.Branch { Id = (int)entityApplicationAccounting.BranchCode },
                SalePoint = new COMMMOD.SalePoint { Id = (int)entityApplicationAccounting.SalePointCode },
                Beneficiary = new Individual { IndividualId = (int)entityApplicationAccounting.IndividualCode },
                AccountingNature = (int)entityApplicationAccounting.AccountingNature,
                //AccountingNatureDescription = applicationAccounting.nat ,
                BookAccount = new BookAccount { Id = (int)entityApplicationAccounting.AccountingAccountCode },/// id cuenta contable
                //ReceiptNumber = applicationAccounting. ,
                ReceiptDate = entityApplicationAccounting.ReceiptDate,
                BankReconciliationId = (int)entityApplicationAccounting.BankReconciliationCode,
                AccountingConcept = new IMPMOD.AccountingConcept { Id = Convert.ToString((int)entityApplicationAccounting.AccountingConceptCode) },
                Amount = new COMMMOD.Amount { Value = (decimal)entityApplicationAccounting.Amount },
                LocalAmount = new COMMMOD.Amount { Value = (decimal)entityApplicationAccounting.LocalAmount },
                ApplicationId = (int)entityApplicationAccounting.TempAppCode,
                CurrencyId = (int)entityApplicationAccounting.CurrencyCode,
                ExchangeRate = new COMMMOD.ExchangeRate { SellAmount = (int)entityApplicationAccounting.ExchangeRate }
            };
        }

        public static List<ApplicationAccounting> CreateTemporalApplicationAccountings(List<ACCEN.TempApplicationAccounting> entityApplicationAccounting)
        {
            List<ApplicationAccounting> applicationAccountings = new List<ApplicationAccounting>();
            entityApplicationAccounting.ForEach(x =>
            {
                applicationAccountings.Add(CreateTemporalApplicationAccounting(x));
            });
            return applicationAccountings;
        }

        #endregion

        #region TempApplicationPremiumCommission
        public static ApplicationPremiumCommision CretaTempApplicationPremiumCommision(ACCEN.TempApplicationPremiumCommiss entityTempApplicationPremiumCommiss)
        {
            return new ApplicationPremiumCommision
            {
                AgentAgencyId = entityTempApplicationPremiumCommiss.AgentAgencyId,
                AgentIndividualId = entityTempApplicationPremiumCommiss.AgentId,
                AgentTypeId = entityTempApplicationPremiumCommiss.AgentTypeCode,
                Amount = entityTempApplicationPremiumCommiss.Amount,
                CommissionType = entityTempApplicationPremiumCommiss.CommissionType,
                CurrencyId = entityTempApplicationPremiumCommiss.CurrencyCode,
                ExchangeRate = entityTempApplicationPremiumCommiss.ExchangeRate,
                Id = entityTempApplicationPremiumCommiss.TempAppPremiumCommissId,
                LocalAmount = entityTempApplicationPremiumCommiss.LocalAmount,
                AppComponentId = entityTempApplicationPremiumCommiss.TempAppPremiumId,
                CommissionDiscountIncomeAmount = entityTempApplicationPremiumCommiss.Amount,
                IsUsedCommission = true,
                ApplicationPremiumId = entityTempApplicationPremiumCommiss.TempAppPremiumId,
                DiscountedCommissionId= entityTempApplicationPremiumCommiss.TempAppPremiumCommissId,
            };
        }
        public static List<ApplicationPremiumCommision> CretaTempApplicationPremiumCommision(List<ACCEN.TempApplicationPremiumCommiss> entityTempApplicationPremiumCommisses)
        {
            List<ApplicationPremiumCommision> tempApplicationPremiumCommisses = new List<ApplicationPremiumCommision>();
            foreach (ACCEN.TempApplicationPremiumCommiss entityTempApplicationPremiumCommission in entityTempApplicationPremiumCommisses)
            {
                tempApplicationPremiumCommisses.Add(CretaTempApplicationPremiumCommision(entityTempApplicationPremiumCommission));
            }
            return tempApplicationPremiumCommisses;
        }
        public static List<ApplicationPremiumCommision> CretaTempApplicationPremiumCommision(BusinessCollection businessCollection)
        {
            List<ApplicationPremiumCommision> TempApplicationPremiumCommiss = new List<ApplicationPremiumCommision>();
            foreach (ACCEN.TempApplicationPremiumCommiss tempApplicationPremiumCommiss in businessCollection.OfType<ACCEN.TempApplicationPremiumCommiss>())
            {
                TempApplicationPremiumCommiss.Add(CretaTempApplicationPremiumCommision(tempApplicationPremiumCommiss));
            }
            return TempApplicationPremiumCommiss;
        }
        public static ApplicationPremiumCommision CretaApplicationPremiumCommision(ACCEN.ApplicationPremiumCommiss entityTempApplicationPremiumCommiss)
        {
            return new ApplicationPremiumCommision
            {
                AgentAgencyId = entityTempApplicationPremiumCommiss.AgentAgencyId,
                AgentIndividualId = entityTempApplicationPremiumCommiss.AgentId,
                AgentTypeId = entityTempApplicationPremiumCommiss.AgentTypeCode,
                Amount = entityTempApplicationPremiumCommiss.Amount,
                CommissionType = entityTempApplicationPremiumCommiss.CommissionType,
                CurrencyId = entityTempApplicationPremiumCommiss.CurrencyCode,
                ExchangeRate = entityTempApplicationPremiumCommiss.ExchangeRate,
                Id = entityTempApplicationPremiumCommiss.AppCommissId,
                LocalAmount = entityTempApplicationPremiumCommiss.LocalAmount,
                AppComponentId = entityTempApplicationPremiumCommiss.AppComponentId,
                CommissionDiscountIncomeAmount = entityTempApplicationPremiumCommiss.Amount,
                IsUsedCommission = true,
            };
        }
        public static List<ApplicationPremiumCommision> CretaApplicationPremiumCommision(List<ACCEN.ApplicationPremiumCommiss> entityApplicationPremiumCommisses)
        {
            List<ApplicationPremiumCommision> tempApplicationPremiumCommisses = new List<ApplicationPremiumCommision>();
            foreach (ACCEN.ApplicationPremiumCommiss entityApplicationPremiumCommission in entityApplicationPremiumCommisses)
            {
                tempApplicationPremiumCommisses.Add(CretaApplicationPremiumCommision(entityApplicationPremiumCommission));
            }
            return tempApplicationPremiumCommisses;
        }
        public static List<ApplicationPremiumCommision> CretaApplicationPremiumCommision(BusinessCollection businessCollection)
        {
            List<ApplicationPremiumCommision> TempApplicationPremiumCommiss = new List<ApplicationPremiumCommision>();
            foreach (ACCEN.ApplicationPremiumCommiss applicationPremiumCommiss in businessCollection.OfType<ACCEN.ApplicationPremiumCommiss>())
            {
                TempApplicationPremiumCommiss.Add(CretaApplicationPremiumCommision(applicationPremiumCommiss));
            }
            return TempApplicationPremiumCommiss;
        }
        #endregion

        #region ApplicationPremiumCommission
        public static ApplicationPremiumCommision CreateApplicacionPremiumCommission(ACCEN.ApplicationPremiumCommiss entityApplicationPremiumCommiss)
        {
            return new ApplicationPremiumCommision
            {
                AgentAgencyId = entityApplicationPremiumCommiss.AgentAgencyId,
                AgentIndividualId = entityApplicationPremiumCommiss.AgentId,
                AgentTypeId = entityApplicationPremiumCommiss.AgentTypeCode,
                Amount = entityApplicationPremiumCommiss.Amount,
                AppComponentId = entityApplicationPremiumCommiss.AppComponentId,
                CommissionType = entityApplicationPremiumCommiss.CommissionType,
                CurrencyId = entityApplicationPremiumCommiss.CurrencyCode,
                ExchangeRate = entityApplicationPremiumCommiss.ExchangeRate,
                Id = entityApplicationPremiumCommiss.AppCommissId,
                LocalAmount = entityApplicationPremiumCommiss.LocalAmount 
            };
        }
        public static List<ApplicationPremiumCommision> CreateApplicacionPremiumCommissions(List<ACCEN.ApplicationPremiumCommiss> entityApplicationPremiumCommisses)
        {
            List<ApplicationPremiumCommision> applicationPremiumCommisions = new List<ApplicationPremiumCommision>();
            foreach (ACCEN.ApplicationPremiumCommiss applicationPremiumCommiss in entityApplicationPremiumCommisses)
            {
                applicationPremiumCommisions.Add(CreateApplicacionPremiumCommission(applicationPremiumCommiss));
            }
            return applicationPremiumCommisions;
        }
        public static List<ApplicationPremiumCommision> CreateApplicacionPremiumCommission(BusinessCollection businessCollection)
        {
            List<ApplicationPremiumCommision> ApplicationPremiumCommission = new List<ApplicationPremiumCommision>();
            foreach (ACCEN.ApplicationPremiumCommiss applicationPremiumCommiss in businessCollection.OfType<ACCEN.ApplicationPremiumCommiss>())
            {
                ApplicationPremiumCommission.Add(CreateApplicacionPremiumCommission(applicationPremiumCommiss));
            }
            return ApplicationPremiumCommission;
        }

        public PremiumReceivableTransactionItem PremiumApplication(ACCEN.ApplicationPremium applicationPremium)
        {
            return new PremiumReceivableTransactionItem
            {
                DeductCommission = new COMMMOD.Amount {  Value = applicationPremium.LocalAmount}, 
                Id = applicationPremium.AppCode
            };
        }


        #endregion

        #region CollectApplicationControl
        public static Models.Integration2G.CollectApplicationControl CreateCollectApplicationControl(INTEN.CollectApplicationControl entityIntegration)
        {
            return new Models.Integration2G.CollectApplicationControl()
            {
                Id = entityIntegration.CollectApplicationControlId,
                CollectApplicationId = entityIntegration.CollectApplicationId,
                Action = entityIntegration.Action,
                Origin = entityIntegration.Origin
            };
        }

        public static Models.Integration2G.CollectApplicationControl CreateCollectApplicationControl(Models.Collect.Collect collect, bool update = false)
        {
            return new Models.Integration2G.CollectApplicationControl()
            {
                CollectApplicationId = collect.Id,
                Action = (update) ? "U" : "I",
                Origin = 1
            };
        }

        public static Models.Integration2G.CollectApplicationControl CreateCollectApplicationControl(Models.Imputations.Application application, bool update = false)
        {
            return new Models.Integration2G.CollectApplicationControl()
            {
                CollectApplicationId = application.Id,
                Action = (update) ? "U" : "I",
                Origin = 1
            };
        }
        #endregion


        public static Models.Imputations.ApplicationPremiumComponent CreateApplicationPremiumComponent(BusinessObject businessObject)
        {
            var entityTempApplicationPremiumComponent = (ACCEN.ApplicationPremiumComponent)businessObject;
            return new Models.Imputations.ApplicationPremiumComponent
            {
                AppComponentId = entityTempApplicationPremiumComponent.AppComponentCode,
                PremiumId = entityTempApplicationPremiumComponent.AppPremiumCode,
                ComponentId = entityTempApplicationPremiumComponent.ComponentCode,
                CurrencyId = entityTempApplicationPremiumComponent.CurrencyCode,
                ExchangeRate = entityTempApplicationPremiumComponent.ExchangeRate,
                Amount = entityTempApplicationPremiumComponent.Amount,
                LocalAmount = entityTempApplicationPremiumComponent.LocalAmount,
                MainAmount = entityTempApplicationPremiumComponent.MainAmount,
                MainLocalAmount = entityTempApplicationPremiumComponent.MainLocalAmount,
                ComponentTinyDescription = ""
            };
        }

        /// <summary>
        /// CreateTempImputations
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Imputation></returns>
        public static List<Models.Imputations.ApplicationPremiumComponent> CreateApplicationPremiumComponents(BusinessCollection businessCollection)
        {
            return businessCollection.Select(CreateApplicationPremiumComponent).ToList();
        }
    }
}