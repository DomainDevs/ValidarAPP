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
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ACCEN = Sistran.Core.Application.Accounting.Entities;
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using CLMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using UPENT = Sistran.Core.Application.UniquePersonV1.Entities;
using COMMENT = Sistran.Core.Application.Common.Entities;
using SCRMOD = Sistran.Core.Application.AccountingServices.DTOs.Search;
using PAYENT = Sistran.Core.Application.Claims.Entities;
using ACCENUM = Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.Enums;
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Core.Framework.DAF;
using COMMOD = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Assemblers
{
    internal static class EntityAssembler
    {

        #region Collect

        /// <summary>
        /// CreateBill
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <returns>Collect</returns>
        public static ACCEN.Collect CreateCollect(Collect collect, int collectControlId)
        {
            string name = "";
            string documentNumber = "";
            int payerId = 0;

            if (collect.Payer != null)
            {
                if (collect.Payer.IndividualId == 0)
                {
                    payerId = 0;
                    name = collect.Payer.Name;
                    documentNumber = collect.Payer.IdentificationDocument.Number;
                }
                else
                {
                    payerId = collect.Payer.IndividualId;
                    name = null;
                    documentNumber = null;
                }
            }

            return new ACCEN.Collect(collect.Id)
            {
                CollectId = collect.Id,
                CollectConceptCode = collect.Concept.Id,
                CollectControlCode = collectControlId,
                Description = collect.Description,
                PaymentsTotal = collect.PaymentsTotal.Value,
                RegisterDate = DateTime.Now,
                Status = collect.Status,
                UserId = collect.UserId,
                IndividualId = payerId,
                Number = collect.Number,
                IsTemporal = collect.IsTemporal,
                Comments = collect.Comments,
                AccountingCompanyCode = collect.AccountingCompany.IndividualId,
                CollectType = Convert.ToInt32(collect.CollectType),
                TechnicalTransaction = collect.Transaction.TechnicalTransaction,
                PersonTypeId = collect.Payer.PersonType.Id,
                DocumentTypeId = collect.Payer.IdentificationDocument.DocumentType.Id,
                OtherPayerDocumentNumber = documentNumber,
                OtherPayerName = name
            };
        }

        #endregion

        #region CollectingConcept

        /// <summary>
        /// CreateBillingConcept
        /// </summary>
        /// <param name="collectConcept"></param>
        /// <returns>CollectConcept</returns>
        public static ACCEN.CollectConcept CreateBillingConcept(CollectConcept collectConcept)
        {
            return new ACCEN.CollectConcept(collectConcept.Id)
            {
                CollectConceptId = collectConcept.Id,
                Description = collectConcept.Description
            };
        }

        #endregion

        #region Payment

        /// <summary>
        /// CreatePayment
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectCode"></param>
        /// <returns>Payment</returns>
        public static ACCEN.Payment CreatePayment(PaymentsModels.Payment payment, int collectCode)
        {
            ACCEN.Payment newPayment = new ACCEN.Payment(payment.Id);

            string enumResponse = "";
            string enumResponse2 = "";
            string enumResponse3 = "";

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CASH);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse))
            {
                newPayment = new ACCEN.Payment(payment.Id)
                {
                    PaymentCode = payment.Id,
                    Amount = payment.LocalAmount.Value,
                    CollectCode = collectCode,
                    CurrencyCode = payment.Amount.Currency.Id,
                    ExchangeRate = payment.ExchangeRate.SellAmount == 0 ? payment.ExchangeRate.BuyAmount : payment.ExchangeRate.SellAmount,
                    IncomeAmount = payment.Amount.Value,
                    PaymentMethodTypeCode = payment.PaymentMethod.Id,
                    Status = payment.Status,
                    DatePayment = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK);
            enumResponse2 = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse) ||
                enumResponse2 != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse2))
            {
                Check check = (Check)payment;
                newPayment = new ACCEN.Payment(payment.Id)
                {
                    PaymentCode = check.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = check.PaymentMethod.Id,
                    CurrencyCode = check.Amount.Currency.Id,
                    ExchangeRate = check.ExchangeRate.SellAmount == 0 ? check.ExchangeRate.BuyAmount : check.ExchangeRate.SellAmount,
                    IncomeAmount = check.Amount.Value,
                    Amount = check.LocalAmount.Value,
                    DocumentNumber = check.DocumentNumber,
                    IssuingBankCode = check.IssuingBankCode > 0 ? check.IssuingBankCode : check.IssuingBank.Id,
                    Holder = check.IssuerName,
                    DatePayment = check.Date,
                    Status = check.Status,
                    IssuingAccountNumber = check.IssuingAccountNumber
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PAYMENT_TYPE_CONSIGNMENT_CHECK);

            if (enumResponse != "" && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse))
            {
                ConsignmentCheck consignmentCheck = (ConsignmentCheck)payment;
                newPayment = new ACCEN.Payment(payment.Id)
                {
                    PaymentCode = consignmentCheck.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = consignmentCheck.PaymentMethod.Id,
                    CurrencyCode = consignmentCheck.Amount.Currency.Id,
                    ExchangeRate = consignmentCheck.ExchangeRate.SellAmount == 0 ? consignmentCheck.ExchangeRate.BuyAmount : consignmentCheck.ExchangeRate.SellAmount,
                    IncomeAmount = consignmentCheck.Amount.Value,
                    Amount = consignmentCheck.LocalAmount.Value,
                    DocumentNumber = consignmentCheck.DocumentNumber,
                    IssuingBankCode = consignmentCheck.IssuingBankCode,
                    Holder = consignmentCheck.IssuerName,
                    DatePayment = consignmentCheck.Date,
                    Status = consignmentCheck.Status,
                    IssuingAccountNumber = consignmentCheck.IssuingAccountNumber,
                    ReceivingBankCode = consignmentCheck.ReceivingAccount == null ? 0 : consignmentCheck.ReceivingAccount.Bank.Id,
                    ReceivingAccountNumber = consignmentCheck.ReceivingAccount == null ? "" : consignmentCheck.ReceivingAccount.AccountingAccount.Number
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD);
            enumResponse2 = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD);
            enumResponse3 = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DATA_PHONE);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse) ||
                enumResponse2 != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse2) ||
                enumResponse3 != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse3))
            {
                CreditCard creditCard = (CreditCard)payment;

                newPayment = new ACCEN.Payment(creditCard.Id)
                {
                    PaymentCode = creditCard.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = creditCard.PaymentMethod.Id,
                    CurrencyCode = creditCard.Amount.Currency.Id,
                    CreditCardTypeCode = creditCard.Type.Id,
                    ExchangeRate = creditCard.ExchangeRate.SellAmount == 0 ? creditCard.ExchangeRate.BuyAmount : creditCard.ExchangeRate.SellAmount,
                    IncomeAmount = creditCard.Amount.Value,
                    Amount = creditCard.LocalAmount.Value,
                    DocumentNumber = creditCard.CardNumber,
                    IssuingBankCode = creditCard.IssuingBank.Id,
                    Holder = creditCard.Holder,
                    AuthorizationNumber = creditCard.AuthorizationNumber,
                    DatePayment = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))),
                    Voucher = creditCard.Voucher,
                    ValidMonth = creditCard.ValidThru.Month,
                    ValidYear = creditCard.ValidThru.Year,
                    Status = creditCard.Status,
                    Taxes = creditCard.Tax,
                    Retentions = creditCard.Retention,
                    Commission = creditCard.Type.Commission
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DIRECT_CONECTION);
            enumResponse2 = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_TRANSFER);
            enumResponse3 = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_PAYMENT_AREA);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse) ||
                     enumResponse2 != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse2) ||
                     enumResponse3 != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse3))
            {
                Transfer transfer = (Transfer)payment;

                newPayment = new ACCEN.Payment(transfer.Id)
                {
                    PaymentCode = transfer.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = transfer.PaymentMethod.Id,
                    CurrencyCode = transfer.Amount.Currency.Id,
                    ExchangeRate = transfer.ExchangeRate.SellAmount == 0 ? transfer.ExchangeRate.BuyAmount : transfer.ExchangeRate.SellAmount,
                    IncomeAmount = transfer.Amount.Value,
                    Amount = transfer.LocalAmount.Value,
                    DocumentNumber = transfer.DocumentNumber,
                    IssuingBankCode = transfer.IssuingBank.Id,
                    IssuingAccountNumber = transfer.IssuingAccountNumber,
                    Holder = transfer.IssuerName,
                    ReceivingBankCode = transfer.ReceivingAccount.Bank.Id,
                    ReceivingAccountNumber = transfer.ReceivingAccount.Number,
                    DatePayment = transfer.Date,
                    Status = transfer.Status
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DEPOSIT_VOUCHER);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse))
            {
                DepositVoucher depositVoucher = (DepositVoucher)payment;

                newPayment = new ACCEN.Payment(depositVoucher.Id)
                {
                    PaymentCode = depositVoucher.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = depositVoucher.PaymentMethod.Id,
                    CurrencyCode = depositVoucher.Amount.Currency.Id,
                    ExchangeRate = depositVoucher.ExchangeRate.SellAmount == 0 ? depositVoucher.ExchangeRate.BuyAmount : depositVoucher.ExchangeRate.SellAmount,
                    IncomeAmount = depositVoucher.Amount.Value,
                    Amount = depositVoucher.LocalAmount.Value,
                    DocumentNumber = depositVoucher.VoucherNumber,
                    Holder = depositVoucher.DepositorName,
                    ReceivingBankCode = depositVoucher.ReceivingAccount.Bank.Id,
                    ReceivingAccountNumber = depositVoucher.ReceivingAccount.Number,
                    DatePayment = depositVoucher.Date,
                    Status = depositVoucher.Status
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_RETENTION_RECEIPT);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse))
            {
                RetentionReceipt retentionReceipt = (RetentionReceipt)payment;

                newPayment = new ACCEN.Payment(retentionReceipt.Id)
                {
                    PaymentCode = retentionReceipt.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = retentionReceipt.PaymentMethod.Id,
                    CurrencyCode = retentionReceipt.Amount.Currency.Id,
                    ExchangeRate = retentionReceipt.ExchangeRate.SellAmount == 0 ? retentionReceipt.ExchangeRate.BuyAmount : retentionReceipt.ExchangeRate.SellAmount,
                    IncomeAmount = retentionReceipt.Amount.Value,
                    Amount = retentionReceipt.LocalAmount.Value,
                    DocumentNumber = retentionReceipt.BillNumber,
                    AuthorizationNumber = retentionReceipt.AuthorizationNumber,
                    DatePayment = retentionReceipt.Date,
                    Voucher = retentionReceipt.VoucherNumber,
                    SerialVoucher = retentionReceipt.SerialVoucherNumber,
                    SerialNumber = retentionReceipt.SerialNumber,
                    Status = retentionReceipt.Status,
                    RetentionConceptCode = retentionReceipt.RetentionConcept.Id
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_TRANSFER);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse))
            {
                Transfer transferArea = (Transfer)payment;

                newPayment = new ACCEN.Payment(transferArea.Id)
                {
                    PaymentCode = transferArea.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = transferArea.PaymentMethod.Id,
                    CurrencyCode = transferArea.Amount.Currency.Id,
                    ExchangeRate = transferArea.ExchangeRate.SellAmount == 0 ? transferArea.ExchangeRate.BuyAmount : transferArea.ExchangeRate.SellAmount,
                    IncomeAmount = transferArea.Amount.Value,
                    Amount = transferArea.LocalAmount.Value,
                    DocumentNumber = transferArea.DocumentNumber,
                    IssuingBankCode = transferArea.IssuingBank.Id,
                    IssuingAccountNumber = transferArea.IssuingAccountNumber,
                    Holder = transferArea.IssuerName,
                    ReceivingBankCode = transferArea.ReceivingAccount.Bank.Id,
                    ReceivingAccountNumber = transferArea.ReceivingAccount.Number,
                    DatePayment = transferArea.Date,
                    Status = transferArea.Status
                };
            }

            enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_PAYMENT_CARD);

            if (enumResponse != ""
                && payment.PaymentMethod.Id == Convert.ToInt32(enumResponse))
            {
                CreditCard creditCardPayment = (CreditCard)payment;

                newPayment = new ACCEN.Payment(creditCardPayment.Id)
                {
                    PaymentCode = creditCardPayment.Id,
                    CollectCode = collectCode,
                    PaymentMethodTypeCode = creditCardPayment.PaymentMethod.Id,
                    CurrencyCode = creditCardPayment.Amount.Currency.Id,
                    CreditCardTypeCode = creditCardPayment.Type.Id,
                    ExchangeRate = creditCardPayment.ExchangeRate.SellAmount == 0 ? creditCardPayment.ExchangeRate.BuyAmount : creditCardPayment.ExchangeRate.SellAmount,
                    IncomeAmount = creditCardPayment.Amount.Value,
                    Amount = creditCardPayment.LocalAmount.Value,
                    DocumentNumber = creditCardPayment.CardNumber,
                    IssuingBankCode = creditCardPayment.IssuingBank.Id,
                    Holder = creditCardPayment.Holder,
                    AuthorizationNumber = creditCardPayment.AuthorizationNumber,
                    DatePayment = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))),
                    Voucher = creditCardPayment.Voucher,
                    ValidMonth = creditCardPayment.ValidThru.Month,
                    ValidYear = creditCardPayment.ValidThru.Year,
                    Status = creditCardPayment.Status,
                    Taxes = creditCardPayment.Tax,
                    Retentions = creditCardPayment.Retention,
                    Commission = creditCardPayment.Type.Commission
                };
            }

            return newPayment;
        }

        public static List<CollectControl> CreateCollectControls(BusinessCollection businessObjects)
        {
            List<CollectControl> collectControls = new List<CollectControl>();

            foreach (ACCEN.CollectControl entityCollectControl in businessObjects)
            {
                collectControls.Add(CreateCollectControl(entityCollectControl));
            }

            return collectControls;
        }

        private static CollectControl CreateCollectControl(ACCEN.CollectControl entityCollectControl)
        {
            COMMOD.Branch branch = new COMMOD.Branch() { Id = Convert.ToInt32(entityCollectControl.BranchCode) };

            if (entityCollectControl == null)
            {
                return null;
            }

            return new CollectControl()
            {
                Id = entityCollectControl.CollectControlId,
                UserId = Convert.ToInt32(entityCollectControl.UserId),
                Branch = branch,
                Status = Convert.ToInt32(entityCollectControl.Status),
                AccountingDate = Convert.ToDateTime(entityCollectControl.AccountingDate),
                OpenDate = Convert.ToDateTime(entityCollectControl.OpenDate),
                CloseDate = Convert.ToDateTime(entityCollectControl.CloseDate)

            };
        }

        #endregion

        #region PaymentTax

        ///<summary>
        /// CreatePaymentTax
        /// </summary>
        /// <param name="paymentTax"></param>
        /// <param name="paymentId"></param>
        /// <returns>PaymentTax</returns>
        public static ACCEN.PaymentTax CreatePaymentTax(PaymentTax paymentTax, int paymentId)
        {
            return new ACCEN.PaymentTax(paymentTax.Id)
            {
                PaymentTaxCode = paymentTax.Id,
                PaymentCode = paymentId,
                TaxCode = paymentTax.Tax.Id,
                TaxRate = Convert.ToDecimal(paymentTax.Rate),
                TaxAmount = paymentTax.Rate * paymentTax.TaxBase.Value / 100,
                TaxBase = Convert.ToDecimal(paymentTax.TaxBase.Value)
            };
        }

        #endregion

        #region RejectedPayment

        /// <summary>
        /// CreateRejectedPayment
        /// </summary>
        /// <param name="rejectedPayment"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>RejectedPayment</returns>
        public static ACCEN.RejectedPayment CreateRejectedPayment(RejectedPayment rejectedPayment, int userId, DateTime registerDate)
        {
            Amount commision = new Amount();
            commision.Value = rejectedPayment.Commission.Value;

            Amount commisionTax = new Amount();
            commisionTax.Value = rejectedPayment.TaxCommission.Value;

            PaymentsModels.Payment payment = new PaymentsModels.Payment();
            payment.Id = rejectedPayment.Payment.Id;

            return new ACCEN.RejectedPayment(rejectedPayment.Id)
            {
                RejectionCode = rejectedPayment.Rejection.Id,
                RejectionDate = rejectedPayment.Date,
                PaymentCode = payment.Id,
                Comission = commision.Value,
                TaxComission = commisionTax.Value,
                Description = rejectedPayment.Description,
                RegisterDate = registerDate,
                UserId = userId
            };
        }

        #endregion

        #region CouponStatus

        /// <summary>
        /// CreateCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        public static ACCEN.CouponStatus CreateCouponStatus(CouponStatus couponStatus)
        {
            return new ACCEN.CouponStatus(couponStatus.Id, couponStatus.SmallDescription)
            {
                Applied = couponStatus.CouponStatusType == ACCENUM.CouponStatusTypes.Applied,
                Description = couponStatus.Description,
                Enabled = couponStatus.IsEnabled,
                NumberOfRetries = couponStatus.RetriesNumber,
                Rejection = couponStatus.CouponStatusType == ACCENUM.CouponStatusTypes.Rejected,
                Retry = Convert.ToBoolean(couponStatus.RetriesNumber == 0 ? 0 : 1),
            };
        }

        #endregion

        #region Rejection Bank

        /// <summary>
        /// CreateRejectionBank
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public static ACCEN.BankNetworkStatus CreateRejectionBank(BankNetworkStatus bankNetworkStatus)
        {
            return new ACCEN.BankNetworkStatus(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id)
            {
                AppliedDefault = bankNetworkStatus.AcceptedCouponStatus[0].SmallDescription,
                RejectionDefault = bankNetworkStatus.RejectedCouponStatus[0].SmallDescription,
            };
        }

        #endregion

        #region PaymenBallot

        /// <summary>
        /// CreatePaymenBallot
        /// </summary>
        /// <param name="paymentBallot"></param>
        /// <param name="registerDate"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentBallot</returns>
        public static ACCEN.PaymentBallot CreatePaymenBallot(PaymentBallot paymentBallot, DateTime registerDate, int userId)
        {
            return new ACCEN.PaymentBallot(paymentBallot.Id)
            {
                PaymentBallotCode = paymentBallot.Id,
                PaymentBallotNumber = paymentBallot.BallotNumber,
                BankCode = paymentBallot.Bank.Id,
                AccountNumber = paymentBallot.AccountNumber,
                CurrencyCode = paymentBallot.Currency.Id,
                BankDate = paymentBallot.BankDate,
                BallotAmount = paymentBallot.Amount.Value,
                BankAmount = paymentBallot.BankAmount.Value,
                Status = paymentBallot.Status,
                RegisterDate = registerDate,
                UserId = userId,
                TechnicalTransaction = paymentBallot.Transaction.TechnicalTransaction
            };
        }

        #endregion

        #region PaymenTicketBallot

        /// <summary>
        /// CreatePaymenTicketBallot
        /// </summary>
        /// <param name="paymentTicketBallotId"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="paymentBallotId"></param>
        /// <returns>PaymenTicketBallot</returns>
        public static ACCEN.PaymentTicketBallot CreatePaymenTicketBallot(int paymentTicketBallotId, int paymentTicketId, int paymentBallotId)
        {
            return new ACCEN.PaymentTicketBallot(0)
            {
                PaymentTicketBallotCode = paymentTicketBallotId,
                PaymentTicketCode = paymentTicketId,
                PaymentBallotCode = paymentBallotId
            };
        }

        #endregion

        #region PaymentTicket

        /// <summary>
        /// CreatePaymentTicket
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="registerDate"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        public static ACCEN.PaymentTicket CreatePaymentTicket(PaymentTicket paymentTicket, DateTime registerDate, int userId)
        {
            Branch branch = new Branch() { Id = paymentTicket.Branch.Id };
            Amount amount = new Amount() { Value = paymentTicket.Amount.Value };
            Amount commission = new Amount() { Value = paymentTicket.Commission.Value };
            Bank bank = new Bank() { Id = paymentTicket.Bank.Id };
            paymentTicket.Status = (int)PaymentTicketStatus.Active;
            Amount cash = new Amount() { Value = paymentTicket.CashAmount.Value };
            Currency currency = new Currency() { Id = paymentTicket.Currency.Id };

            return new ACCEN.PaymentTicket(paymentTicket.Id)
            {
                PaymentTicketCode = paymentTicket.Id,
                PaymentMethodTypeCode = paymentTicket.PaymentMethod,
                BranchCode = branch.Id,
                Amount = amount.Value,
                CommissionAmount = commission.Value,
                BankCode = bank.Id,
                AccountNumber = paymentTicket.AccountNumber,
                CurrencyCode = currency.Id,
                CashAmount = cash.Value,
                RegisterDate = registerDate,
                UserId = userId,
                Status = paymentTicket.Status
            };
        }

        #endregion

        #region PaymentTicketItem

        /// <summary>
        /// CreatePaymentTicketItem
        /// </summary>
        /// <param name="paymentTicketItemId"></param>
        /// <param name="paymentId"></param>
        /// <param name="paymentTicketId"></param>
        /// <returns>PaymentTicketItem</returns>
        public static ACCEN.PaymentTicketItem CreatePaymentTicketItem(int paymentTicketItemId, int paymentId, int paymentTicketId)
        {
            return new ACCEN.PaymentTicketItem(paymentTicketItemId)
            {
                PaymentCode = paymentId,
                PaymentTicketCode = paymentTicketId
            };
        }

        #endregion

        #region CollectControl

        /// <summary>
        /// CreateBillControl
        /// </summary>
        /// <param name="collectControl"></param>
        /// <returns>CollectControl</returns>
        public static ACCEN.CollectControl CreateBillControl(CollectControl collectControl)
        {
            return new ACCEN.CollectControl(collectControl.Id)
            {
                CollectControlId = collectControl.Id,
                UserId = collectControl.UserId,
                BranchCode = collectControl.Branch.Id,
                Status = collectControl.Status,
                AccountingDate = collectControl.AccountingDate,
                OpenDate = collectControl.OpenDate,
                CloseDate = collectControl.CloseDate
            };
        }

        #endregion

        #region CollectControlPayment

        /// <summary>
        /// CreateCollectControlPayment
        /// </summary>
        /// <param name="collectControl"></param>
        /// <param name="registerNumber"></param>
        /// <returns>CollectControlPayment</returns>
        public static ACCEN.CollectControlPayment CreateCollectControlPayment(CollectControl collectControl, int registerNumber)
        {
            return new ACCEN.CollectControlPayment(collectControl.CollectControlPayments[registerNumber].Id)
            {
                CollectControlPaymentId = collectControl.CollectControlPayments[registerNumber].Id,
                CollectControlCode = collectControl.Id,
                PaymentMethodCode = collectControl.CollectControlPayments[registerNumber].PaymentMethod.Id,
                PaymentTotalAdmitted = collectControl.CollectControlPayments[registerNumber].PaymentTotalAdmitted.Value,
                PaymentTotalReceived = collectControl.CollectControlPayments[registerNumber].PaymentsTotalReceived.Value,
                PaymentTotalDifference = collectControl.CollectControlPayments[registerNumber].PaymentsTotalDifference.Value
            };
        }

        #endregion

        #region CollectPaymentMethodType

        /// <summary>
        /// CreateCollectPaymentMethodType
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="methodType"> </param>
        /// <param name="enabledTicket"> </param>
        /// <param name="enabledBilling"> </param>
        /// <param name="enabledPaymentOrder"> </param>
        /// <param name="enabledPaymentRequest"> </param>
        /// <returns>CollectPaymentMethodType</returns>
        public static ACCEN.PaymentType CreateCollectPaymentMethodType(int id, int methodType,
            int enabledTicket, int enabledBilling, int enabledPaymentOrder, int enabledPaymentRequest)
        {
            return new ACCEN.PaymentType(id)
            {
                PaymentTypeCode = methodType,
                EnabledTicket = enabledTicket,
                EnabledBilling = enabledBilling,
                EnabledPaymentOrder = enabledPaymentOrder,
                EnabledPaymentRequest = enabledPaymentRequest
            };
        }

        #endregion

        #region ExchangePayment

        /// <summary>
        /// CreateExchangePayment
        /// </summary>
        /// <param name="exchangePaymentId"></param>
        /// <param name="paymentSourceId"></param>
        /// <param name="paymentIdFinal"></param>
        /// <returns>ExchangePayment</returns>
        public static ACCEN.ExchangePayment CreateExchangePayment(int exchangePaymentId, int paymentSourceId, int paymentIdFinal)
        {
            return new ACCEN.ExchangePayment(exchangePaymentId)
            {
                ExchangePaymentCode = exchangePaymentId,
                PaymentCdSource = paymentSourceId,
                PaymentCdFinal = paymentIdFinal
            };
        }

        #endregion

        #region LegalPayment

        /// <summary>
        /// CreateLegalPayment
        /// </summary>
        /// <param name="legalPaymentId"></param>
        /// <param name="rejectedPaymentId"></param>
        /// <param name="legalDate"></param>
        /// <returns>LegalPayment</returns>
        public static ACCEN.LegalPayment CreateLegalPayment(int legalPaymentId, int rejectedPaymentId, DateTime legalDate)
        {
            return new ACCEN.LegalPayment(legalPaymentId)
            {
                LegalPaymentCode = legalPaymentId,
                RejectedPaymentCode = rejectedPaymentId,
                LegalDate = legalDate
            };
        }

        #endregion

        #region PaymentLog

        /// <summary>
        /// CreatePaymentLog
        /// </summary>
        /// <param name="itemParam"></param>
        /// <returns>PaymentLog</returns>
        public static ACCEN.PaymentLog CreatePaymentLog(Dictionary<string, string> itemParam)
        {
            return new ACCEN.PaymentLog(Convert.ToInt32(itemParam["Id"]))
            {
                PaymentLogCode = Convert.ToInt32(itemParam["Id"]),
                ActionTypeCode = Convert.ToInt32(itemParam["ActionTypeCode"]),
                SourceCode = Convert.ToInt32(itemParam["SourceCode"]),
                PaymentCode = Convert.ToInt32(itemParam["PaymentCode"]),
                Status = Convert.ToInt32(itemParam["Status"]),
                RegisterDate = Convert.ToDateTime(itemParam["RegisterDate"]),
                UserId = Convert.ToInt32(itemParam["UserId"]),
            };
        }

        #endregion

        #region RegularizedPayment

        /// <summary>
        /// CreateRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="billIdFinal"></param>
        /// <returns>RegularizePayment</returns>
        public static ACCEN.RegularizePayment CreateRegularizedPayment(int regularizedPaymentId, int paymentIdSource, int billIdFinal)
        {
            return new ACCEN.RegularizePayment(regularizedPaymentId)
            {
                RegularizePaymentCode = regularizedPaymentId,
                PaymentCdSource = paymentIdSource,
                BillCdFinal = billIdFinal
            };
        }

        #endregion

        #region ActionType

        /// <summary>
        /// CreateActionType
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns>ActionType</returns>
        public static ACCEN.ActionType CreateActionType(ActionType actionType)
        {
            return new ACCEN.ActionType(actionType.Id)
            {
                ActionTypeCode = actionType.Id,
                Description = actionType.Description
            };
        }


        #endregion

        #region StatusPayment

        /// <summary>
        /// CreateStatusPayment
        /// </summary>
        /// <param name="itemParam"></param>
        /// <returns>StatusPayment</returns>
        public static ACCEN.StatusPayment CreateStatusPayment(Dictionary<string, string> itemParam)
        {
            return new ACCEN.StatusPayment(Convert.ToInt32(itemParam["PaymentMethodTypeCode"]), Convert.ToInt32(itemParam["Status"]))
            {
                PaymentMethodTypeCode = Convert.ToInt32(itemParam["PaymentMethodTypeCode"]),
                Status = Convert.ToInt32(itemParam["Status"]),
                Description = Convert.ToString(itemParam["Description"]),
            };
        }

        #endregion

        #region TempPremiumReceivableTransaction

        /// <summary>
        /// CreateTempPremiumReceivable
        /// </summary>
        /// <param name="premiumReceivableTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>TempPremiumReceivableTrans</returns>
        public static ACCEN.TempPremiumReceivableTrans CreateTempPremiumReceivable(PremiumReceivableTransactionItem premiumReceivableTransactionItem,
                int tempImputationId, decimal exchangeRate, int userId, DateTime registerDate)
        {
            int? statusQuota = 0;

            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount < premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
            {
                statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaPartial; // PARCIAL
            }
            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
            {
                statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaTotal; // TOTAL
            }

            return new ACCEN.TempPremiumReceivableTrans(0)
            {
                TempPremiumReceivableTransCode = 0,
                TempApplicationCode = tempImputationId,
                PolicyId = premiumReceivableTransactionItem.Policy.Id,
                EndorsementId = premiumReceivableTransactionItem.Policy.Endorsement.Id,
                PaymentNum = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number,
                PaymentAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount,
                PayerId = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId,
                IncomeAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount,
                CurrencyCode = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id,
                ExchangeRate = exchangeRate,
                Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount * exchangeRate,
                RegisterDate = registerDate,
                DiscountedCommission = premiumReceivableTransactionItem.DeductCommission.Value,
                PremiumReceivableQuotaStatusCode = statusQuota
            };
        }

        #endregion

        #region TempApplicationPremium
        public static ACCEN.TempApplicationPremium CreateTempApplicationPremium(PremiumReceivableTransactionItem premiumReceivableTransactionItem,
                int tempImputationId, decimal exchangeRate, int userId, DateTime registerDate, DateTime accountingDate)
        {
            int? statusQuota = 0;

            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
            {
                statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaPartial; // PARCIAL
            }
            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount < premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
            {
                statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaTotal; // TOTAL
            }

            return new ACCEN.TempApplicationPremium(0)
            {
                TempAppPremiumCode = 0,
                TempAppCode = tempImputationId,
                //PolicyId = premiumReceivableTransactionItem.Policy.Id,
                EndorsementCode = premiumReceivableTransactionItem.Policy.Endorsement.Id,
                PaymentNum = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number,
                //PaymentAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount,
                MainAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount,
                PayerCode = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId,
                LocalAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount,
                CurrencyCode = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id,
                ExchangeRate = exchangeRate,
                //Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount * exchangeRate,
                Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount,
                RegisterDate = registerDate,
                DiscountedCommission = premiumReceivableTransactionItem.DeductCommission?.Value,
                PremiumQuotaStatusCode = statusQuota,
                AccountingDate = accountingDate,
                MainLocalAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount
            };
        }
        #endregion

        #region TempApplicationPremium
        public static ACCEN.TempApplicationPremiumComponent CreateTempApplicationPremiumComponent(TempApplicationPremiumComponent tempApplicationPremiumComponent)
        {
            return new ACCEN.TempApplicationPremiumComponent(0)
            {
                TempAppPremiumCode = tempApplicationPremiumComponent.TempApplicationPremiumCode,
                ComponentCode = tempApplicationPremiumComponent.ComponentCode,
                CurrencyCode = tempApplicationPremiumComponent.ComponentCurrencyCode,
                ExchangeRate = tempApplicationPremiumComponent.ExchangeRate,
                Amount = tempApplicationPremiumComponent.Amount,
                LocalAmount = tempApplicationPremiumComponent.LocalAmount,
                MainAmount = tempApplicationPremiumComponent.MainAmount,
                MainLocalAmount = tempApplicationPremiumComponent.MainLocalAmount,
            };
        }
        #endregion

        #region PremiumReceivableTransaction

        ///// <summary>
        ///// CreatePremiumReceivable
        ///// </summary>
        ///// <param name="premiumReceivableTransactionItem"></param>
        ///// <param name="imputationId"></param>
        ///// <param name="exchangeRate"></param>
        ///// <param name="registerDate"></param>
        ///// <returns>PremiumReceivableTrans</returns>
        //public static ACCEN.PremiumReceivableTrans CreatePremiumReceivable(PremiumReceivableTransactionItem premiumReceivableTransactionItem,
        //                                                                 int imputationId, decimal exchangeRate, DateTime registerDate)
        //{
        //    int? statusQuota = 0;

        //    if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount)
        //    {
        //        statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaPartial; // PARCIAL
        //    }
        //    if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
        //    {
        //        statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaTotal; // TOTAL
        //    }

        //    return new ACCEN.PremiumReceivableTrans(premiumReceivableTransactionItem.Id)
        //    {
        //        PremiumReceivableTransId = premiumReceivableTransactionItem.Id,// Autonumérico
        //        ApplicationCode = imputationId,
        //        PolicyId = premiumReceivableTransactionItem.Policy.Id,
        //        EndorsementId = premiumReceivableTransactionItem.Policy.Endorsement.Id,
        //        PaymentNum = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number,
        //        PaymentAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount,
        //        PayerId = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId,
        //        IncomeAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount,
        //        CurrencyCode = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id,
        //        ExchangeRate = exchangeRate,
        //        Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount * exchangeRate,
        //        RegisterDate = registerDate,
        //        DiscountedCommission = premiumReceivableTransactionItem.DeductCommission.Value,
        //        PremiumReceivableQuotaStatusCode = statusQuota
        //    };
        //}

        /// <summary>
        /// CreatePremiumReceivable
        /// </summary>
        /// <param name="premiumReceivableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTrans</returns>
        public static ACCEN.ApplicationPremium CreatePremiumReceivableApplication(PremiumReceivableTransactionItem premiumReceivableTransactionItem,
                                                                         int imputationId, decimal exchangeRate, DateTime registerDate, DateTime accountingDate)
        {
            int? statusQuota = 0;

            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount)
            {
                statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaPartial; // PARCIAL
            }
            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
            {
                statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaTotal; // TOTAL
            }

            return new ACCEN.ApplicationPremium(premiumReceivableTransactionItem.Id)
            {
                AppPremiumCode = premiumReceivableTransactionItem.Id,// Autonumérico
                AppCode = imputationId,
                //PolicyId = premiumReceivableTransactionItem.Policy.Id,
                EndorsementCode = premiumReceivableTransactionItem.Policy.Endorsement.Id,
                PaymentNum = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number,
                LocalAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount,
                PayerCode = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId,
                MainAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount,
                CurrencyCode = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id,
                ExchangeRate = exchangeRate,
                Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount * exchangeRate,
                RegisterDate = registerDate,
                DiscountedCommission = premiumReceivableTransactionItem.DeductCommission.Value,
                PremiumQuotaStatusCode = statusQuota,
                AccountingDate = accountingDate
            };
        }

        #endregion

        #region Imputation

        ///// <summary>
        ///// CreateImputation
        ///// </summary>
        ///// <param name="imputation"></param>
        ///// <param name="sourceCode"></param>
        ///// <returns>Imputation</returns>
        //public static ACCEN.Imputation CreateImputation(Imputation imputation, int sourceCode, int technicalTransaction)
        //{
        //    return new ACCEN.Imputation(imputation.Id)
        //    {
        //        ImputationCode = imputation.Id,
        //        ImputationTypeCode = Convert.ToInt32(imputation.ImputationType),
        //        SourceCode = sourceCode,
        //        RegisterDate = imputation.Date,
        //        UserId = imputation.UserId,
        //        TechnicalTransaction = technicalTransaction
        //    };
        //}

        public static ACCEN.LogMassiveDataPolicy LogMassiveDataPolicy(LogMassiveDataPolicy logMassiveDataPolicy)
        {
            return new ACCEN.LogMassiveDataPolicy()
            {
                ProcessId = logMassiveDataPolicy.IdProcess,
                DateGenerate = logMassiveDataPolicy.DateGenerate,
                EndorsementNumber = logMassiveDataPolicy.EndorsementNumber,
                BranchId = logMassiveDataPolicy.BranchId,
                PolicyNumber = logMassiveDataPolicy.PolicyNumber,
                PrefixId = logMassiveDataPolicy.PrefixId,
                LogMessage = logMassiveDataPolicy.LogMessage,
                TechnicalTransaction = logMassiveDataPolicy.TechnicalTransaction,
                ExchangeRate = logMassiveDataPolicy.ExchangeRate,
                Amount = logMassiveDataPolicy.Amount
            };
        }
        #endregion

        #region Application

        /// <summary>
        /// CreateImputation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public static ACCEN.Application CreateApplication(Models.Imputations.Application application, int sourceCode, int technicalTransaction)
        {
            return new ACCEN.Application(application.Id)
            {
                ApplicationCode = application.Id,
                ModuleCode = application.ModuleId,
                SourceCode = sourceCode,
                RegisterDate = application.RegisterDate,
                UserCode = application.UserId,
                TechnicalTransaction = technicalTransaction,
                IndividualCode = application.IndividualId,
                AccountingDate = application.AccountingDate,
                BranchCode = application.BranchId,
                Description = application.Description
            };
        }
        /// <summary>
        /// CreateImputation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public static ACCEN.Application CreateApplication(Models.Imputations.Application application)
        {
            return new ACCEN.Application(application.Id)
            {
                ApplicationCode = application.Id,
                ModuleCode = application.ModuleId,
                SourceCode = application.SourceId,
                RegisterDate = application.RegisterDate,
                UserCode = application.UserId,
                TechnicalTransaction = application.TechnicalTransaction,
                IndividualCode = application.IndividualId,
                AccountingDate = application.AccountingDate,
                BranchCode = application.BranchId,
                Description = application.Description
            };
        }

        #endregion

        #region Application

        ///// <summary>
        ///// CreateTempImputation
        ///// </summary>
        ///// <param name="imputation"></param>
        ///// <param name="sourceCode"></param>
        ///// <returns>TempImputation</returns>
        //public static ACCEN.TempImputation CreateTempImputation(Imputation imputation, int sourceCode)
        //{
        //    return new ACCEN.TempImputation(imputation.Id)
        //    {
        //        TempImputationCode = imputation.Id,
        //        ImputationTypeCode = Convert.ToInt32(imputation.ImputationType),
        //        SourceCode = sourceCode,
        //        RegisterDate = imputation.Date,
        //        UserId = imputation.UserId,
        //        IsRealSource = false
        //    };
        //}
        #endregion

        #region TempApplication
        /// <summary>
        /// CreateTempImputation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>TempImputation</returns>
        public static ACCEN.TempApplication CreateTempApplication(Models.Imputations.Application application, int sourceCode)
        {
            return new ACCEN.TempApplication(application.Id)
            {
                TempAppCode = application.Id,
                ModuleCode = application.ModuleId,
                SourceCode = sourceCode,
                RegisterDate = application.RegisterDate,
                UserCode = application.UserId,
                AccountingDate = application.AccountingDate,
                IndividualCode = application.IndividualId
            };
        }

        #endregion

        #region DepositPremiumTransaction

        /// <summary>
        /// CreateDepositPremiumTransaction
        /// </summary>
        /// <param name="depositPremiumTransaction"></param>
        /// <param name="premiumReceivableCode"></param>
        /// <param name="payerTypeId"></param>
        /// <returns>DepositPremiumTransaction</returns>
        public static ACCEN.DepositPremiumTransaction CreateDepositPremiumTransaction(DepositPremiumTransaction depositPremiumTransaction, int premiumReceivableCode, int payerTypeId)
        {
            return new ACCEN.DepositPremiumTransaction(depositPremiumTransaction.Id)
            {
                DepositPremiumTransactionCode = depositPremiumTransaction.Id,
                PremiumReceivableTransCode = premiumReceivableCode,
                CollectCode = depositPremiumTransaction.Collect.Id,
                PayerType = payerTypeId,
                PayerId = depositPremiumTransaction.Collect.Payer.IndividualId,
                RegisterDate = depositPremiumTransaction.Date,
                CurrencyCode = depositPremiumTransaction.Amount.Currency.Id,
                IncomeAmount = depositPremiumTransaction.Amount.Value,
                ExchangeRate = depositPremiumTransaction.ExchangeRate.SellAmount,
                Amount = depositPremiumTransaction.LocalAmount.Value
            };
        }

        #endregion

        #region TempBrokerCheckingAccountTransaction

        /// <summary>
        /// CreateTempBrokerCheckingAccount
        /// </summary>
        /// <param name="brokersCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempBrokerParentCode"></param>
        /// <param name="collectCode"></param>
        /// <param name="accountingDate"></param>
        /// <returns>TempBrokerCheckingAccount</returns>
        public static ACCEN.TempBrokerCheckingAccTrans CreateTempBrokerCheckingAccount(BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem, int tempImputationId, int tempBrokerParentCode, int collectCode, DateTime accountingDate)
        {
            int agentType = 0;
            if (brokersCheckingAccountTransactionItem.IsAutomatic)
            {
                agentType = 1;
            }
            else
            {
                Convert.ToInt32(brokersCheckingAccountTransactionItem.Holder.FullName);

            }


            return new ACCEN.TempBrokerCheckingAccTrans(0)
            {
                TempBrokerCheckingAccTransCode = brokersCheckingAccountTransactionItem.Id,
                TempApplicationCode = Convert.ToInt32(tempImputationId),
                TempBrokerParentCode = Convert.ToInt32(tempBrokerParentCode),
                AgentTypeCode = agentType,
                AgentId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Holder.IndividualId),
                AgentAgencyId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Agencies[0].Id),
                BranchCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Branch.Id),
                SalePointCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.SalePoint.Id),
                AccountingCompanyCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Company.IndividualId),
                AccountingDate = Convert.ToDateTime(accountingDate),
                AccountingNature = brokersCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2,
                CheckingAccountConceptCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.CheckingAccountConcept.Id),
                CurrencyCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Amount.Currency.Id),
                ExchangeRate = Convert.ToDecimal(brokersCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                IncomeAmount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.Amount.Value),
                Amount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.Amount.Value * brokersCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                Description = brokersCheckingAccountTransactionItem.Comments,
                TransactionNumber = Convert.ToInt32(collectCode)
            };
        }

        #endregion

        #region TempDepositPrimeDAO

        /// <summary>
        /// CreateTempDepositPrime
        /// </summary>
        /// <param name="tempPremiumReceivableCode"></param>
        /// <param name="depositPremiumTransactionCode"></param>
        /// <param name="amount"></param>
        /// <returns>TempDepositPrime</returns>
        public static ACCEN.TempDepositPrime CreateTempDepositPrime(int tempPremiumReceivableCode, int depositPremiumTransactionCode, decimal amount)
        {
            return new ACCEN.TempDepositPrime(0)
            {
                TempDepositPrimeCode = 0,
                TempPremiumReceivableCode = tempPremiumReceivableCode,
                DepositPremiumTransactionCode = depositPremiumTransactionCode,
                Amount = amount
            };
        }

        #endregion

        #region BrokerCheckingAccount

        /// <summary>
        /// CreateTempBrokerCheckingAccount
        /// </summary>
        /// <param name="brokersCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempBrokerParentId"></param>
        /// <param name="collectCode"></param>
        /// <param name="agentTypeId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>BrokerCheckingAccountTrans</returns>
        public static ACCEN.BrokerCheckingAccountTrans CreateBrokerCheckingAccount(BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem, int tempImputationId, int tempBrokerParentId, int collectCode, int agentTypeId, DateTime accountingDate)
        {
            return new ACCEN.BrokerCheckingAccountTrans(brokersCheckingAccountTransactionItem.Id)
            {
                BrokerCheckingAccountTransId = brokersCheckingAccountTransactionItem.Id,
                ApplicationCode = Convert.ToInt32(tempImputationId),
                BrokerParentCode = Convert.ToInt32(tempBrokerParentId),
                AgentTypeCode = Convert.ToInt32(agentTypeId),
                AgentId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Holder.IndividualId),
                AgentAgencyId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Agencies[0].Id),
                AccountingNature = Convert.ToInt32(brokersCheckingAccountTransactionItem.AccountingNature),
                BranchCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Branch.Id),
                SalePointCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.SalePoint.Id),
                AccountingCompanyCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Company.IndividualId),
                AccountingDate = Convert.ToDateTime(accountingDate),
                CheckingAccountConceptCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.CheckingAccountConcept.Id),
                CurrencyCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Amount.Currency.Id),
                ExchangeRate = Convert.ToDecimal(brokersCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                IncomeAmount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.Amount.Value),
                Amount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.Amount.Value * brokersCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                Description = brokersCheckingAccountTransactionItem.Comments,
                TransactionNumber = Convert.ToInt32(collectCode),
                IsAutomatic = brokersCheckingAccountTransactionItem.IsAutomatic,
                PolicyId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Policy.Id),
                PrefixId = Convert.ToInt32(brokersCheckingAccountTransactionItem.PrefixId),
                InsuredId = Convert.ToInt32(brokersCheckingAccountTransactionItem.InsuredId),
                CommissionTypeCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.CommissionType),
                StCommissionPercentage = Convert.ToDecimal(brokersCheckingAccountTransactionItem.CommissionPercentage.Value),
                StCommissionAmount = Convert.ToDecimal(brokersCheckingAccountTransactionItem.CommissionAmount.Value),
                DiscountedCommission = Convert.ToDecimal(brokersCheckingAccountTransactionItem.DiscountedCommission.Value),
                CommissionBalance = Convert.ToDecimal(brokersCheckingAccountTransactionItem.CommissionBalance.Value),
                EndorsementId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Policy.Endorsement.Id),
                PaymentNum = Convert.ToInt32(brokersCheckingAccountTransactionItem.Policy.PaymentPlan.Quotas[0].Number),
                PayerId = Convert.ToInt32(brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId),
                LineBusinessCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.Prefix.Id),
                SubLineBusinessCode = Convert.ToInt32(brokersCheckingAccountTransactionItem.SubPrefix.Id),
                Payed = Convert.ToBoolean(brokersCheckingAccountTransactionItem.IsPayed),
                //comision adicional
                AdditCommissionPercentage = brokersCheckingAccountTransactionItem.AdditionalCommissionPercentage,
                AdditCommissionAmount = brokersCheckingAccountTransactionItem.AdditionalCommissionAmount
            };
        }

        #endregion

        #region TempClaimPayment

        /// <summary>
        /// CreateTempClaimPayment
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>TempClaimPaymentReqTrans</returns>
        public static ACCEN.TempClaimPaymentReqTrans CreateTempClaimPayment(ClaimsPaymentRequestTransactionItem paymentRequestTransactionItem,
                                                              int imputationId, decimal exchangeRate, int paymentSourceId, int paymentNumber, DateTime firstPaymentDue)
        {
            int? paymentNum = 0;
            DateTime? paymentExpirationDate = DateTime.Now;
            int? claimCode = 0;
            int? bussinessType = 0;

            claimCode = Convert.ToInt32(paymentRequestTransactionItem.PaymentRequest.Claim.Id);
            bussinessType = paymentRequestTransactionItem.BussinessType;

            if (paymentSourceId == 3) // Recobro
            {
                paymentNum = paymentNumber;
                paymentExpirationDate = firstPaymentDue;
            }
            else if (paymentSourceId == 2) // Salvamento
            {
                paymentNum = paymentNumber;
                paymentExpirationDate = firstPaymentDue;
            }
            else if (paymentSourceId == 1) // Liquidación Siniestro
            {
                paymentNum = null;
                paymentExpirationDate = null;
            }
            else if (paymentSourceId == 4) // Pagos Varios
            {
                paymentNum = null;
                paymentExpirationDate = null;
                claimCode = 0;
                bussinessType = null;
            }

            if (paymentExpirationDate == Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                paymentExpirationDate = null;
            }

            return new ACCEN.TempClaimPaymentReqTrans(paymentRequestTransactionItem.Id)
            {
                TempClaimPaymentReqTransCode = paymentRequestTransactionItem.Id,
                TempApplicationCode = imputationId,
                PaymentRequestCode = Convert.ToInt32(paymentRequestTransactionItem.PaymentRequest.Number),
                ClaimCode = claimCode,
                ExchangeRate = exchangeRate,
                BeneficiaryId = paymentRequestTransactionItem.PaymentRequest.IndividualId,
                CurrencyCode = paymentRequestTransactionItem.PaymentRequest.Currency.Id,
                IncomeAmount = Convert.ToDecimal(paymentRequestTransactionItem.PaymentRequest.TotalAmount),
                Amount = Convert.ToDecimal(paymentRequestTransactionItem.PaymentRequest.TotalAmount) * exchangeRate,
                RegistrationDate = paymentRequestTransactionItem.PaymentRequest.RegistrationDate,
                EstimationDate = paymentRequestTransactionItem.PaymentRequest.EstimatedDate,
                BussinessType = bussinessType,
                RequestType = paymentSourceId,
                PaymentNum = paymentNum,
                PaymentExpirationDate = paymentExpirationDate
            };
        }

        #endregion

        #region ClaimPayment

        /// <summary>
        /// CreateClaimPayment
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ClaimPaymentRequestTrans</returns>
        public static ACCEN.ClaimPaymentRequestTrans CreateClaimPayment(ClaimsPaymentRequestTransactionItem paymentRequestTransactionItem,
                                                      int imputationId, decimal exchangeRate, int paymentSourceId, int paymentNumber, DateTime firstPaymentDue)
        {
            int? paymentNum = 0;
            DateTime? paymentExpirationDate = DateTime.Now;

            if (paymentSourceId == 3) // Recobro
            {
                paymentNum = paymentNumber;
                paymentExpirationDate = firstPaymentDue;
            }
            if (paymentSourceId == 2) // Salvamento
            {
                paymentNum = paymentNumber;
                paymentExpirationDate = firstPaymentDue;
            }
            if (paymentSourceId == 1) // Liquidación Siniestro
            {
                paymentNum = null;
                paymentExpirationDate = null;
            }

            if (paymentExpirationDate == Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                paymentExpirationDate = null;
            }

            return new ACCEN.ClaimPaymentRequestTrans(paymentRequestTransactionItem.Id)
            {
                ClaimPaymentRequestTransId = paymentRequestTransactionItem.Id,
                ApplicationCode = imputationId,
                PaymentRequestCode = Convert.ToInt32(paymentRequestTransactionItem.PaymentRequest.Number),
                ClaimCode = Convert.ToInt32(paymentRequestTransactionItem.PaymentRequest.Claim.Id),
                ExchangeRate = exchangeRate,
                BeneficiaryId = paymentRequestTransactionItem.PaymentRequest.IndividualId,
                CurrencyCode = paymentRequestTransactionItem.PaymentRequest.Currency.Id,
                IncomeAmount = Convert.ToDecimal(paymentRequestTransactionItem.PaymentRequest.TotalAmount),
                Amount = Convert.ToDecimal(paymentRequestTransactionItem.PaymentRequest.TotalAmount) * exchangeRate,
                RegistrationDate = paymentRequestTransactionItem.PaymentRequest.RegistrationDate,
                EstimationDate = paymentRequestTransactionItem.PaymentRequest.EstimatedDate,
                BussinessType = paymentRequestTransactionItem.BussinessType,
                RequestType = paymentSourceId,
                PaymentNum = paymentNum,
                PaymentExpirationDate = paymentExpirationDate
            };
        }

        #endregion

        #region TempPaymentRequestTrans

        /// <summary>
        /// CreateTempPaymentRequestTrans
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>TempClaimPaymentReqTrans</returns>
        public static ACCEN.TempPaymentRequestTrans CreateTempPaymentRequestTrans(PaymentRequestTransactionItem paymentRequestTransactionItem,
                                                              int imputationId, decimal exchangeRate)
        {
            return new ACCEN.TempPaymentRequestTrans(paymentRequestTransactionItem.Id)
            {
                Amount = paymentRequestTransactionItem.PaymentRequest.LocalAmount.Value,
                BeneficiaryId = paymentRequestTransactionItem.PaymentRequest.Beneficiary.IndividualId,
                BussinessType = null,
                CurrencyCode = paymentRequestTransactionItem.PaymentRequest.TotalAmount.Currency.Id,
                EstimationDate = paymentRequestTransactionItem.PaymentRequest.EstimatedDate,
                ExchangeRate = paymentRequestTransactionItem.PaymentRequest.ExchangeRate.BuyAmount,
                IncomeAmount = paymentRequestTransactionItem.PaymentRequest.TotalAmount.Value,
                PaymentExpirationDate = null,
                PaymentNumber = null,
                PaymentRequestCode = paymentRequestTransactionItem.PaymentRequest.Id,
                RegistrationDate = paymentRequestTransactionItem.PaymentRequest.RegisterDate,
                TempApplicationCode = imputationId,
                TempPaymentRequestTransId = paymentRequestTransactionItem.Id
            };
        }

        public static ACCEN.TempApplicationPremium CreateTempApplicationPremiumTrans(PaymentRequestTransactionItem paymentRequestTransactionItem,
                                                              int applicationId, decimal exchangeRate)
        {
            return new ACCEN.TempApplicationPremium(paymentRequestTransactionItem.Id)
            {
                Amount = paymentRequestTransactionItem.PaymentRequest.LocalAmount.Value,
                PayerCode = paymentRequestTransactionItem.PaymentRequest.Beneficiary.IndividualId,
                //BussinessType = null,
                CurrencyCode = paymentRequestTransactionItem.PaymentRequest.TotalAmount.Currency.Id,
                //EstimationDate = paymentRequestTransactionItem.PaymentRequest.EstimatedDate,
                ExchangeRate = paymentRequestTransactionItem.PaymentRequest.ExchangeRate.BuyAmount,
                LocalAmount = paymentRequestTransactionItem.PaymentRequest.TotalAmount.Value,
                //PaymentExpirationDate = null,
                //PaymentNumber = null,
                PaymentNum = 0,
                //PaymentRequestCode = paymentRequestTransactionItem.PaymentRequest.Id,
                RegisterDate = paymentRequestTransactionItem.PaymentRequest.RegisterDate,
                TempAppCode = applicationId,
                TempAppPremiumCode = paymentRequestTransactionItem.Id
            };
        }

        #endregion

        #region PaymentRequestTrans

        /// <summary>
        /// CreateClaimPayment
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ClaimPaymentRequestTrans</returns>
        public static ACCEN.PaymentRequestTrans CreatePaymentRequestTrans(PaymentRequestTransactionItem paymentRequestTransactionItem,
                                                      int imputationId, decimal exchangeRate)
        {
            return new ACCEN.PaymentRequestTrans(paymentRequestTransactionItem.Id)
            {
                Amount = paymentRequestTransactionItem.PaymentRequest.LocalAmount.Value,
                BeneficiaryId = paymentRequestTransactionItem.PaymentRequest.Beneficiary.IndividualId,
                BussinessType = null,
                CurrencyCode = paymentRequestTransactionItem.PaymentRequest.TotalAmount.Currency.Id,
                EstimationDate = paymentRequestTransactionItem.PaymentRequest.EstimatedDate,
                ExchangeRate = paymentRequestTransactionItem.PaymentRequest.ExchangeRate.BuyAmount,
                ApplicationCode = imputationId,
                IncomeAmount = paymentRequestTransactionItem.PaymentRequest.TotalAmount.Value,
                PaymentExpirationDate = null,
                PaymentNumber = null,
                PaymentRequestCode = paymentRequestTransactionItem.PaymentRequest.Id,
                PaymentRequestTransId = paymentRequestTransactionItem.Id,
                RegistrationDate = paymentRequestTransactionItem.PaymentRequest.RegisterDate
            };
        }

        #endregion

        #region ApplicationPremium
        /// <summary>
        /// CreateClaimPayment
        /// </summary>
        /// <param name="applicationPremium"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ClaimPaymentRequestTrans</returns>
        public static ACCEN.ApplicationPremium CreateApplicationPremium(ApplicationPremium applicationPremium)
        {
            return new ACCEN.ApplicationPremium(applicationPremium.Id)
            {
                AppPremiumCode = applicationPremium.Id,
                AppCode = applicationPremium.ApplicationId,
                EndorsementCode = applicationPremium.EndorsementId,
                PayerCode = applicationPremium.PayerId,
                PaymentNum = applicationPremium.PaymentNumber,
                CurrencyCode = applicationPremium.Currencyid,
                ExchangeRate = applicationPremium.ExchangeRate,
                AccountingDate = applicationPremium.AccountingDate,
                RegisterDate = applicationPremium.RegisterDate,
                Amount = applicationPremium.Amount,
                LocalAmount = applicationPremium.LocalAmount,
                MainAmount = applicationPremium.MainAmount,
                MainLocalAmount = applicationPremium.MainLocalAmount,
                IsCommissionPaid = applicationPremium.IsCommissionPaid,
                IsCoinsPremiumPaid = applicationPremium.IsCoinsurancePremiumPaid,
                PremiumQuotaStatusCode = applicationPremium.QuotaStatusId,
                DiscountedCommission = applicationPremium.DiscountCommission
            };
        }

        /// <summary>
        /// CreateClaimPayment
        /// </summary>
        /// <param name="entityTempApplicationPremium"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ClaimPaymentRequestTrans</returns>
        public static ACCEN.ApplicationPremium CreateApplicationPremium(ACCEN.TempApplicationPremium entityTempApplicationPremium, int status)
        {
            return new ACCEN.ApplicationPremium(0)
            {
                AppPremiumCode = 0,
                AppCode = entityTempApplicationPremium.TempAppCode,
                EndorsementCode = entityTempApplicationPremium.EndorsementCode,
                PayerCode = entityTempApplicationPremium.PayerCode,
                PaymentNum = entityTempApplicationPremium.PaymentNum,
                CurrencyCode = entityTempApplicationPremium.CurrencyCode,
                ExchangeRate = entityTempApplicationPremium.ExchangeRate,
                AccountingDate = entityTempApplicationPremium.AccountingDate,
                RegisterDate = entityTempApplicationPremium.RegisterDate,
                Amount = entityTempApplicationPremium.Amount,
                LocalAmount = entityTempApplicationPremium.LocalAmount,
                MainAmount = entityTempApplicationPremium.MainAmount,
                MainLocalAmount = entityTempApplicationPremium.MainLocalAmount,
                PremiumQuotaStatusCode = status
            };
        }


        /// <summary>
        /// CreateClaimPayment
        /// </summary>
        /// <param name="tempApplicationPremium"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ClaimPaymentRequestTrans</returns>
        public static ACCEN.TempApplicationPremium CreateTempApplicationPremium(TempApplicationPremium tempApplicationPremium)
        {
            return new ACCEN.TempApplicationPremium(tempApplicationPremium.Id)
            {
                TempAppPremiumCode = tempApplicationPremium.Id,
                TempAppCode = tempApplicationPremium.ApplicationId,
                EndorsementCode = tempApplicationPremium.EndorsementId,
                PayerCode = tempApplicationPremium.PayerId,
                PaymentNum = tempApplicationPremium.PaymentNumber,
                CurrencyCode = tempApplicationPremium.Currencyid,
                ExchangeRate = tempApplicationPremium.ExchangeRate,
                AccountingDate = tempApplicationPremium.AccountingDate,
                RegisterDate = tempApplicationPremium.RegisterDate,
                Amount = tempApplicationPremium.Amount,
                LocalAmount = tempApplicationPremium.LocalAmount,
                MainAmount = tempApplicationPremium.MainAmount,
                MainLocalAmount = tempApplicationPremium.MainLocalAmount,
                PremiumQuotaStatusCode = tempApplicationPremium.QuotaStatusId,
                DiscountedCommission = tempApplicationPremium.DiscountedCommission
            };
        }

        #endregion

        #region TempBrokerCheckingAccountItem

        /// <summary>
        /// CreateTempBrokerCheckingAccountItem
        /// </summary>
        /// <param name="brokerCheckingAccountItem"></param>
        /// <returns>TempBrokerCheckingAccountItem</returns>
        public static ACCEN.TempBrokerCheckingAccountItem CreateTempBrokerCheckingAccountItem(BrokerCheckingAccountItem brokerCheckingAccountItem)
        {
            return new ACCEN.TempBrokerCheckingAccountItem(0)
            {
                TempBrokerCheckingAccountItemCode = 0, // Autonumérico
                TempBrokerCheckingAccTransCode = Convert.ToInt32(brokerCheckingAccountItem.TempBrokerCheckingAccountId),
                BrokerCheckingAccountCode = Convert.ToInt32(brokerCheckingAccountItem.BrokerCheckingAccountId)
            };
        }

        #endregion

        #region TempUsedDepositPremium

        /// <summary>
        /// CreateTempUsedDepositPremium
        /// </summary>
        /// <param name="tempUsedDepositPremiumDto"></param>
        /// <returns>TempUsedDepositPremium</returns>
        public static ACCEN.TempUsedDepositPremium CreateTempUsedDepositPremium(SCRMOD.TempUsedDepositPremiumDTO tempUsedDepositPremiumDto)
        {
            return new ACCEN.TempUsedDepositPremium(tempUsedDepositPremiumDto.Id)
            {
                TempUsedDepositPremiumCode = tempUsedDepositPremiumDto.Id,
                DepositPremiumTransactionCode = tempUsedDepositPremiumDto.DepositPremiumTransactionId,
                TempPremiumReceivableTransCode = tempUsedDepositPremiumDto.TempPremiumReceivableItemId,
                Amount = tempUsedDepositPremiumDto.Amount
            };
        }

        #endregion

        #region Amount

        /// <summary>
        /// CreateUsedAmount
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="depositPremiumTransactionId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="localAmount"></param>
        /// <returns>entities.Amount</returns>
        public static ACCEN.Amount CreateUsedAmount(Amount amount, int depositPremiumTransactionId, ExchangeRate exchangeRate, Amount localAmount)
        {
            return new ACCEN.Amount(0)
            {
                AmountId = 0,
                DepositPremiumTransactionCode = depositPremiumTransactionId,
                IncomeAmount = amount.Value,
                ExchangeRate = exchangeRate.SellAmount,
                Value = localAmount.Value
            };
        }

        #endregion

        #region ComponentCollection

        /// <summary>
        /// CreateComponentCollection
        /// </summary>
        /// <param name="componentCollectionId"></param>
        /// <param name="premiumReceivableId"></param>
        /// <param name="componentId"></param>
        /// <param name="amount"></param>
        /// <param name="exchange"></param>
        /// <param name="localAmount"></param>
        /// <returns>ComponentCollection</returns>
        public static ACCEN.ComponentCollection CreateComponentCollection(int componentCollectionId, int premiumReceivableId, int componentId, Amount amount, ExchangeRate exchangeRate, Amount localAmount)
        {
            return new ACCEN.ComponentCollection(componentCollectionId)
            {
                ComponentCollectionCode = componentCollectionId,
                PremiumReceivableTransCode = premiumReceivableId,
                ComponentId = componentId,
                CurrencyCode = amount.Currency.Id,
                IncomeAmount = amount.Value,
                ExchangeRate = exchangeRate.SellAmount,
                Amount = localAmount.Value
            };
        }

        #endregion

        #region PrefixComponentCollection

        /// <summary>
        /// CreatePrefixComponentCollection
        /// </summary>
        /// <param name="prefixComponentCollectionId"></param>
        /// <param name="componentCollectionId"></param>
        /// <param name="lineBusinessId"></param>
        /// <param name="subLineBusinessId"></param>
        /// <param name="amount"></param>
        /// <param name="exchange"></param>
        /// <param name="localAmount"></param>
        /// <returns>PrefixComponentCollection</returns>
        public static ACCEN.PrefixComponentCollection CreatePrefixComponentCollection(int prefixComponentCollectionId, int componentCollectionId, int lineBusinessId, int subLineBusinessId, Amount amount, ExchangeRate exchangeRate, Amount localAmount)
        {
            return new ACCEN.PrefixComponentCollection(prefixComponentCollectionId)
            {
                PrefixComponentCollectionCode = prefixComponentCollectionId,
                ComponentCollectionCode = componentCollectionId,
                LineBusinessCode = lineBusinessId,
                SubLineBusinessCode = subLineBusinessId,
                IncomeAmount = amount.Value,
                ExchangeRate = exchangeRate.SellAmount,
                Amount = localAmount.Value
            };
        }

        #endregion

        #region TempReinsuranceCheckingAccountTransaction

        /// <summary>
        /// CreateTempReinsuranceCheckingAccount
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempReinsuranceParentCode"></param>
        /// <returns>TempReinsCheckingAccTrans</returns>
        public static ACCEN.TempReinsCheckingAccTrans CreateTempReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem, int tempImputationId, int tempReinsuranceParentCode)
        {
            return new ACCEN.TempReinsCheckingAccTrans(0)
            {
                TempReinsCheckingAccTransCode = 0,//autonumérico
                TempApplicationCode = Convert.ToInt32(tempImputationId),
                TempReinsuranceParentCode = Convert.ToInt32(tempReinsuranceParentCode), //0
                BranchCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Branch.Id),
                SalePointCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.SalePoint.Id),
                AccountingCompanyCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Company.IndividualId),
                LineBusinessCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Prefix.Id),
                SubLineBusinessCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Prefix.LineBusinessId),
                AgentId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Broker.IndividualId),
                ReinsuranceCompanyId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Holder.IndividualId),
                IsFacultative = Convert.ToBoolean(reinsuranceCheckingAccountTransactionItem.IsFacultative),
                SlipNumber = reinsuranceCheckingAccountTransactionItem.SlipNumber,
                ContractTypeCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.ContractTypeId),
                ContractCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.ContractNumber), // No es ContractNumber es ContractId
                Section = reinsuranceCheckingAccountTransactionItem.Section,
                Region = reinsuranceCheckingAccountTransactionItem.Region,
                Period = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Period),
                CheckingAccountConceptCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.CheckingAccountConcept.Id),
                AccountingNature = reinsuranceCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2,
                CurrencyCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Amount.Currency.Id),
                ExchangeRate = Convert.ToDecimal(reinsuranceCheckingAccountTransactionItem.ExchangeRate.BuyAmount),
                IncomeAmount = Convert.ToDecimal(reinsuranceCheckingAccountTransactionItem.Amount.Value),
                Amount = Convert.ToDecimal(reinsuranceCheckingAccountTransactionItem.Amount.Value * reinsuranceCheckingAccountTransactionItem.ExchangeRate.BuyAmount),
                Description = reinsuranceCheckingAccountTransactionItem.Comments,
                ApplicationYear = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Year),
                ApplicationMonth = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Month),
                PolicyId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.PolicyId),
                EndorsementId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.EndorsementId),
                TransactionNumber = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.TransactionNumber)
            };
        }

        #endregion

        #region TempReinsuranceCheckingAccountItem

        /// <summary>
        /// CreateTempReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccountItem"></param>
        /// <returns>TempReinsuranceCheckingAccountItem</returns>
        public static ACCEN.TempReinsuranceCheckingAccountItem CreateTempReinsuranceCheckingAccountItem(ReinsuranceCheckingAccountItem reinsuranceCheckingAccountItem)
        {
            return new ACCEN.TempReinsuranceCheckingAccountItem(0)
            {
                TempReinsuranceCheckingAccountItemId = 0,
                TempReinsCheckingAccTransCode = Convert.ToInt32(reinsuranceCheckingAccountItem.TempReinsuranceCheckingAccountId),
                ReinsCheckingAccTransCode = Convert.ToInt32(reinsuranceCheckingAccountItem.ReinsuranceCheckingAccountId)
            };
        }

        #endregion

        #region ReinsuranceCheckingAccount

        /// <summary>
        /// CreateReinsuranceCheckingAccount
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempReinsuranceParentId"></param>
        /// <returns>ReinsCheckingAccTrans</returns>
        public static ACCEN.ReinsCheckingAccTrans CreateReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem, int tempImputationId, int tempReinsuranceParentId)
        {
            return new ACCEN.ReinsCheckingAccTrans(0)
            {
                ReinsCheckingAccTransId = 0,
                ApplicationCode = Convert.ToInt32(tempImputationId),
                ReinsuranceParentCode = tempReinsuranceParentId,
                BranchCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Branch.Id),
                SalePointCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.SalePoint.Id),
                AccountingCompanyCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Company.IndividualId),
                LineBusinessCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Prefix.Id),
                SubLineBusinessCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Prefix.LineBusinessId),
                AgentId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Broker.IndividualId),
                ReinsuranceCompanyId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Holder.IndividualId),
                IsFacultative = Convert.ToBoolean(reinsuranceCheckingAccountTransactionItem.IsFacultative),
                ContractTypeCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.ContractTypeId),
                ContractCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.ContractNumber), // No es ContractNumber, es ContractId
                Section = reinsuranceCheckingAccountTransactionItem.Section,
                Region = reinsuranceCheckingAccountTransactionItem.Region,
                Period = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Period),
                CheckingAccountConceptCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.CheckingAccountConcept.Id),
                AccountingNature = reinsuranceCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2,
                CurrencyCode = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Amount.Currency.Id),
                ExchangeRate = Convert.ToDecimal(reinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                IncomeAmount = Convert.ToDecimal(reinsuranceCheckingAccountTransactionItem.Amount.Value),
                Amount = Convert.ToDecimal(reinsuranceCheckingAccountTransactionItem.Amount.Value * reinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                Description = reinsuranceCheckingAccountTransactionItem.Comments,
                ApplicationDate = Convert.ToDateTime(DateTime.Now),
                ApplicationYear = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Year),
                ApplicationMonth = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.Month),
                PolicyId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.PolicyId),
                EndorsementId = Convert.ToInt32(reinsuranceCheckingAccountTransactionItem.EndorsementId),
                TransactionNumber = 0
            };
        }

        #endregion

        #region TempCoinsuranceCheckingAccountTransaction

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempCoinsuranceParentCode"></param>
        /// <returns>TempCoinsCheckingAccTrans</returns>
        public static ACCEN.TempCoinsCheckingAccTrans CreateTempCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem, int tempImputationId, int tempCoinsuranceParentCode)
        {
            return new ACCEN.TempCoinsCheckingAccTrans(0)
            {
                TempCoinsCheckingAccTransCode = 0,
                TempApplicationCode = Convert.ToInt32(tempImputationId),
                TempCoinsuranceParentCode = Convert.ToInt32(tempCoinsuranceParentCode),
                BranchCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Branch.Id),
                SalePointCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.SalePoint.Id),
                AccountingCompanyCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Company.IndividualId),
                CoinsuranceType = coinsuranceCheckingAccountTransactionItem.CoInsuranceType == CoInsuranceTypes.Accepted ? 1 : 2,
                CoinsuredCompanyId = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Holder.IndividualId),
                AccountingDate = Convert.ToDateTime(coinsuranceCheckingAccountTransactionItem.AccountingDate),
                CheckingAccountConceptCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept.Id),
                CurrencyCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Amount.Currency.Id),
                ExchangeRate = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.ExchangeRate.BuyAmount),
                IncomeAmount = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.Amount.Value),
                Amount = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.Amount.Value * coinsuranceCheckingAccountTransactionItem.ExchangeRate.BuyAmount),
                Description = Convert.ToString(coinsuranceCheckingAccountTransactionItem.Comments),
                AccountingNatureCode = coinsuranceCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2,
            };
        }

        #endregion

        #region TempCoinsuranceCheckingAccountItem

        /// <summary>
        /// CreateTempCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="coinsuranceCheckingAccountItem"></param>
        /// <returns>TempCoinsuranceCheckingAccountItem</returns>
        public static ACCEN.TempCoinsuranceCheckingAccountItem CreateTempCoinsuranceCheckingAccountItem(CoInsuranceCheckingAccountItem coinsuranceCheckingAccountItem)
        {
            return new ACCEN.TempCoinsuranceCheckingAccountItem(0)
            {
                TempCoinsuranceCheckingAccountItemCode = 0,
                TempCoinsCheckingAccTransCode = Convert.ToInt32(coinsuranceCheckingAccountItem.TempCoinsuranceCheckingAccountId),
                CoinsuranceCheckingAccountCode = Convert.ToInt32(coinsuranceCheckingAccountItem.CoinsuranceCheckingAccountId)
            };
        }

        #endregion

        #region CoinsuranceCheckingAccount

        /// <summary>
        /// CreateCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="tempCoinsuranceParentId"></param>
        /// <returns>CoinsuranceCheckingAccount</returns>
        public static ACCEN.CoinsCheckingAccTrans CreateCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem, int tempImputationId, int tempCoinsuranceParentId)
        {
            return new ACCEN.CoinsCheckingAccTrans(0)
            {
                CoinsCheckingAccTransId = coinsuranceCheckingAccountTransactionItem.Id, // Autonumérico
                ApplicationCode = Convert.ToInt32(tempImputationId),
                CoinsuranceParentCode = tempCoinsuranceParentId,
                BranchCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Branch.Id),
                SalePointCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.SalePoint.Id),
                AccountingCompanyCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Company.IndividualId),
                CoinsuranceType = coinsuranceCheckingAccountTransactionItem.CoInsuranceType == CoInsuranceTypes.Accepted ? 2 : 3,
                CoinsuredCompanyId = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Holder.IndividualId),
                AccountingDate = Convert.ToDateTime(coinsuranceCheckingAccountTransactionItem.AccountingDate),
                CheckingAccountConceptCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept.Id),
                CurrencyCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Amount.Currency.Id),
                ExchangeRate = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                IncomeAmount = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.Amount.Value),
                Amount = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.Amount.Value * coinsuranceCheckingAccountTransactionItem.ExchangeRate.SellAmount),
                Description = coinsuranceCheckingAccountTransactionItem.Comments,
                AccountingNatureCode = coinsuranceCheckingAccountTransactionItem.AccountingNature == AccountingNature.Credit ? 1 : 2,
                PolicyId = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.Policy.Id),
                ClaimCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.CliamsId),
                PaymentCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.PaymentRequestId),
                AdministrativeExpenses = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses.Value),
                TaxAdministrativeExpenses = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses.Value),
                ExtraCommission = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.ExtraCommission.Value),
                OverCommission = Convert.ToDecimal(coinsuranceCheckingAccountTransactionItem.OverCommission.Value),
                LineBusinessCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.LineBusiness.Id),
                SubLineBusinessCode = Convert.ToInt32(coinsuranceCheckingAccountTransactionItem.SubLineBusiness.Id)
            };
        }

        #endregion

        #region TempDailyAccountingTrans

        /// <summary>
        /// CreateTempDailyAccounting
        /// </summary>
        /// <param name="dailyAccountingTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>TempDailyAccountingTrans</returns>
        public static ACCEN.TempDailyAccountingTrans CreateTempDailyAccountingTrans(DailyAccountingTransactionItem dailyAccountingTransactionItem, int tempImputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            return new ACCEN.TempDailyAccountingTrans(dailyAccountingTransactionItem.Id)
            {
                TempDailyAccountingTransId = dailyAccountingTransactionItem.Id,
                TempImputationCode = tempImputationId,
                BranchCode = dailyAccountingTransactionItem.Branch.Id,
                SalePointCode = dailyAccountingTransactionItem.SalePoint.Id,
                CompanyCode = dailyAccountingTransactionItem.Company.IndividualId,
                PaymentConceptCode = paymentConceptCode,
                BeneficiaryId = dailyAccountingTransactionItem.Beneficiary.IndividualId,
                BookAccountCode = dailyAccountingTransactionItem.BookAccount.Id,
                AccountingNature = Convert.ToInt32(dailyAccountingTransactionItem.AccountingNature),
                CurrencyCode = dailyAccountingTransactionItem.Amount.Currency.Id,
                IncomeAmount = dailyAccountingTransactionItem.Amount.Value,
                ExchangeRate = dailyAccountingTransactionItem.ExchangeRate.SellAmount,
                Amount = dailyAccountingTransactionItem.LocalAmount.Value,
                Description = description,
                BankReconciliationId = bankReconciliationId,
                ReceiptNumber = receiptNumber,
                ReceiptDate = receiptDate,
                PostdatedAmount = postdatedAmount
            };
        }

        #endregion TempDailyAccountingTrans

        /// <summary>
        /// CreateTempDailyAccounting
        /// </summary>
        /// <param name="applicationAccounting"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>TempDailyAccountingTrans</returns>
        public static ACCEN.TempApplicationAccounting CreateTempApplicationAccounting(ApplicationAccounting applicationAccounting)
        {
            return new ACCEN.TempApplicationAccounting(applicationAccounting.Id)
            {
                TempAppAccountingCode = applicationAccounting.Id,
                TempAppCode = applicationAccounting.ApplicationAccountingId,
                BranchCode = applicationAccounting.Branch.Id,
                SalePointCode = applicationAccounting.SalePoint.Id,
                AccountingConceptCode = Convert.ToInt32(applicationAccounting.AccountingConcept.Id),
                IndividualCode = applicationAccounting.Beneficiary.IndividualId,
                AccountingAccountCode = applicationAccounting.BookAccount.Id,
                AccountingNature = Convert.ToInt32(applicationAccounting.AccountingNature),
                CurrencyCode = applicationAccounting.Amount.Currency.Id,
                LocalAmount = applicationAccounting.LocalAmount.Value,
                ExchangeRate = applicationAccounting.ExchangeRate.SellAmount,
                Amount = applicationAccounting.Amount.Value,
                Description = applicationAccounting.Description,
                BankReconciliationCode = applicationAccounting.BankReconciliationId,
                ReceiptNumber = applicationAccounting.ReceiptNumber,
                ReceiptDate = applicationAccounting.ReceiptDate
            };
        }


        /// <summary>
        /// CreateTempDailyAccountingAnalysis
        /// </summary>
        /// <param name="tempApplicationAccountingAnalysis"></param>
        /// <param name="tempAccountingCode"></param>
        /// <returns></returns>
        public static ACCEN.TempApplicationAccountingAnalysis CreateTempAccountingAnalysis(ApplicationAccountingAnalysis tempApplicationAccountingAnalysis, int tempAccountingCode)
        {
            return new ACCEN.TempApplicationAccountingAnalysis(tempApplicationAccountingAnalysis.Id)
            {
                TempAppAccountingAnalysisCode = tempApplicationAccountingAnalysis.Id,
                TempAppAccountingCode = tempAccountingCode,
                AnalysisCode = tempApplicationAccountingAnalysis.AnalysisId,
                AnalysisConceptCode = tempApplicationAccountingAnalysis.AnalysisConcept.AnalysisConceptId,
                ConceptKey = tempApplicationAccountingAnalysis.ConceptKey,
                Description = tempApplicationAccountingAnalysis.Description
            };
        }

        #region TempDailyAccountingAnalysis

        /// <summary>
        /// CreateTempDailyAccountingAnalysis
        /// </summary>
        /// <param name="tempDailyAccountingAnalysisCode"></param>
        /// <param name="tempDailyAccountingTransId"></param>
        /// <returns></returns>
        public static ACCEN.TempDailyAccountingAnalysis CreateTempDailyAccountingAnalysis(DailyAccountingAnalysisCode tempDailyAccountingAnalysisCode, int tempDailyAccountingTransId)
        {
            return new ACCEN.TempDailyAccountingAnalysis(tempDailyAccountingAnalysisCode.Id)
            {
                TempDailyAccountingAnalysisId = tempDailyAccountingAnalysisCode.Id,
                TempDailyAccountingTransCode = tempDailyAccountingTransId,
                AnalysisCode = tempDailyAccountingAnalysisCode.AnalysisCode.AnalysisCodeId,
                AnalysisConceptCode = tempDailyAccountingAnalysisCode.AnalysisCode.AnalisisConcepts[0].AnalysisConceptId,
                ConceptKey = tempDailyAccountingAnalysisCode.KeyAnalysis,
                Description = tempDailyAccountingAnalysisCode.AnalysisCode.Description
            };
        }

        #endregion TempDailyAccountingAnalysis

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="tempApplicationAccountingCostCenter"></param>
        /// <param name="tempAppAccountingId"></param>
        /// <returns></returns>
        public static ACCEN.TempApplicationAccountingCostCenter CreateTempAccountingCostCenter(ApplicationAccountingCostCenter tempApplicationAccountingCostCenter, int tempAppAccountingId)
        {
            return new ACCEN.TempApplicationAccountingCostCenter(tempApplicationAccountingCostCenter.Id)
            {
                TempAppAccountingCostCenterCode = tempApplicationAccountingCostCenter.Id,
                TempAppAccountingCode = tempAppAccountingId,
                CostCenterCode = tempApplicationAccountingCostCenter.CostCenter.CostCenterId,
                Percentage = tempApplicationAccountingCostCenter.Percentage
            };
        }

        #region TempDailyAccountingCostCenter

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="tempDailyAccountingCostCenter"></param>
        /// <param name="tempDailyAccountingTransId"></param>
        /// <returns></returns>
        public static ACCEN.TempDailyAccountingCostCenter CreateTempDailyAccountingCostCenter(DailyAccountingCostCenter tempDailyAccountingCostCenter, int tempDailyAccountingTransId)
        {
            return new ACCEN.TempDailyAccountingCostCenter(tempDailyAccountingCostCenter.Id)
            {
                TempDailyAccountingCostCenterId = tempDailyAccountingCostCenter.Id,
                TempDailyAccountingTransCode = tempDailyAccountingTransId,
                CostCenterCode = tempDailyAccountingCostCenter.CostCenter.CostCenterId,
                Percentage = tempDailyAccountingCostCenter.Percentage
            };
        }

        #endregion TempDailyAccountingCostCenter

        #region DailyAccounting

        /// <summary>
        /// CreateDailyAccountingTrans
        /// </summary>
        /// <param name="dailyAccountingTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>DailyAccountingTrans</returns>
        public static ACCEN.DailyAccountingTrans CreateDailyAccountingTrans(DailyAccountingTransactionItem dailyAccountingTransactionItem, int imputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            return new ACCEN.DailyAccountingTrans(dailyAccountingTransactionItem.Id)
            {
                DailyAccountingTransId = dailyAccountingTransactionItem.Id,
                ImputationCode = imputationId,
                BranchCode = dailyAccountingTransactionItem.Branch.Id,
                SalesPointCode = dailyAccountingTransactionItem.SalePoint.Id,
                CompanyCode = dailyAccountingTransactionItem.Company.IndividualId,
                PaymentConceptCode = paymentConceptCode,
                BeneficiaryId = dailyAccountingTransactionItem.Beneficiary.IndividualId,
                BookAccountCode = dailyAccountingTransactionItem.BookAccount.Id,
                AccountingNature = Convert.ToInt32(dailyAccountingTransactionItem.AccountingNature),
                CurrencyCode = dailyAccountingTransactionItem.Amount.Currency.Id,
                IncomeAmount = dailyAccountingTransactionItem.Amount.Value,
                ExchangeRate = dailyAccountingTransactionItem.ExchangeRate.SellAmount,
                Amount = dailyAccountingTransactionItem.LocalAmount.Value,
                Description = description,
                BankReconciliationId = bankReconciliationId,
                ReceiptNumber = receiptNumber,
                ReceiptDate = receiptDate,
                PostdatedAmount = postdatedAmount
            };
        }

        #endregion

        /// <summary>
        /// CreateApplicationAccounting
        /// </summary>
        /// <param name="applicationAccountingDTO"></param>
        /// <param name="applicationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <returns>ApplicationAccounting</returns>
        public static ACCEN.ApplicationAccounting CreateApplicationAccounting(ApplicationAccounting applicationAccountingDTO, int applicationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate)
        {
            return new ACCEN.ApplicationAccounting(applicationAccountingDTO.Id)
            {
                AppAccountingCode = applicationAccountingDTO.Id,
                AppCode = applicationId,
                BranchCode = applicationAccountingDTO.Branch.Id,
                SalePointCode = applicationAccountingDTO.SalePoint.Id,
                AccountingConceptCode = paymentConceptCode,
                IndividualCode = applicationAccountingDTO.Beneficiary.IndividualId,
                AccountingAccountCode = applicationAccountingDTO.BookAccount.Id,
                AccountingNature = Convert.ToInt32(applicationAccountingDTO.AccountingNature),
                CurrencyCode = applicationAccountingDTO.Amount.Currency.Id,
                Amount = applicationAccountingDTO.Amount.Value,
                ExchangeRate = applicationAccountingDTO.ExchangeRate.SellAmount,
                LocalAmount = applicationAccountingDTO.LocalAmount.Value,
                Description = description,
                BankReconciliationCode = bankReconciliationId,
                ReceiptNumber = receiptNumber,
                ReceiptDate = receiptDate
            };
        }


        public static ACCEN.ApplicationAccounting CreateApplicationAccounting(ApplicationAccounting applicationAccounting)
        {
            return new ACCEN.ApplicationAccounting(applicationAccounting.Id)
            {
                AppAccountingCode = applicationAccounting.Id,
                AppCode = applicationAccounting.ApplicationId,
                BranchCode = applicationAccounting.Branch.Id,
                SalePointCode = applicationAccounting.SalePoint.Id,
                AccountingConceptCode = (applicationAccounting.AccountingConcept != null) ? Convert.ToInt32(applicationAccounting.AccountingConcept.Id) : 0,
                IndividualCode = applicationAccounting.Beneficiary.IndividualId,
                AccountingAccountCode = (applicationAccounting.BookAccount == null) ? applicationAccounting.ApplicationAccountingId : applicationAccounting.BookAccount.Id,
                AccountingNature = Convert.ToInt32(applicationAccounting.AccountingNature),
                CurrencyCode = (applicationAccounting.Amount.Currency != null) ? applicationAccounting.Amount.Currency.Id : applicationAccounting.CurrencyId,
                Amount = applicationAccounting.Amount.Value,
                ExchangeRate = applicationAccounting.ExchangeRate.SellAmount,
                LocalAmount = applicationAccounting.LocalAmount.Value,
                Description = applicationAccounting.Description,
                BankReconciliationCode = applicationAccounting.BankReconciliationId,
                ReceiptNumber = applicationAccounting.ReceiptNumber,
                ReceiptDate = applicationAccounting.ReceiptDate
            };
        }


        /// <summary>
        /// CreateDailyAccountingAnalysis
        /// </summary>
        /// <param name="applicationAccountingAnalysis"></param>
        /// <param name="appAccountingId"></param>
        /// <returns></returns>
        public static ACCEN.ApplicationAccountingAnalysis CreateApplicationAccountingAnalysis(ApplicationAccountingAnalysis applicationAccountingAnalysis, int appAccountingId)
        {
            ACCEN.ApplicationAccountingAnalysis entityAppAccountingAnalysis = new ACCEN.ApplicationAccountingAnalysis(applicationAccountingAnalysis.Id)
            {
                AppAccountingAnalysisCode = applicationAccountingAnalysis.Id,
                AppAccountingCode = appAccountingId,
                ConceptKey = applicationAccountingAnalysis.ConceptKey,

            };

            if (applicationAccountingAnalysis.AnalysisId > 0)
            {
                entityAppAccountingAnalysis.Description = applicationAccountingAnalysis.Description;
                entityAppAccountingAnalysis.AnalysisCode = applicationAccountingAnalysis.AnalysisId;
                if (applicationAccountingAnalysis.AnalysisConcept.AnalysisConceptId > 0)
                {
                    entityAppAccountingAnalysis.AnalysisConceptCode = applicationAccountingAnalysis.AnalysisConcept.AnalysisConceptId;
                }
            }

            return entityAppAccountingAnalysis;
        }


        #region DailyAccountingAnalysis

        /// <summary>
        /// CreateDailyAccountingAnalysis
        /// </summary>
        /// <param name="dailyAccountingAnalysisCode"></param>
        /// <param name="dailyAccountingTransId"></param>
        /// <returns></returns>
        public static ACCEN.DailyAccountingAnalysis CreateDailyAccountingAnalysis(DailyAccountingAnalysisCode dailyAccountingAnalysisCode, int dailyAccountingTransId)
        {
            return new ACCEN.DailyAccountingAnalysis(dailyAccountingAnalysisCode.Id)
            {
                DailyAccountingAnalysisId = dailyAccountingAnalysisCode.Id,
                DailyAccountingTransCode = dailyAccountingTransId,
                AnalysisCode = dailyAccountingAnalysisCode.AnalysisCode.AnalysisCodeId,
                AnalysisConceptCode = dailyAccountingAnalysisCode.AnalysisCode.AnalisisConcepts[0].AnalysisConceptId,
                ConceptKey = dailyAccountingAnalysisCode.KeyAnalysis,
                Description = dailyAccountingAnalysisCode.AnalysisCode.Description
            };
        }

        #endregion DailyAccountingAnalysis

        #region DailyAccountingCostCenter

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="dailyAccountingCostCenter"></param>
        /// <param name="dailyAccountingTransId"></param>
        /// <returns></returns>
        public static ACCEN.DailyAccountingCostCenter CreateDailyAccountingCostCenter(DailyAccountingCostCenter dailyAccountingCostCenter, int dailyAccountingTransId)
        {
            return new ACCEN.DailyAccountingCostCenter(dailyAccountingCostCenter.Id)
            {
                DailyAccountingCostCenterId = dailyAccountingCostCenter.Id,
                DailyAccountingTransCode = dailyAccountingTransId,
                CostCenterCode = dailyAccountingCostCenter.CostCenter.CostCenterId,
                Percentage = dailyAccountingCostCenter.Percentage
            };
        }

        #endregion DailyAccountingCostCenter

        /// <summary>
        /// CreateTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="appAccountingCostCenter"></param>
        /// <param name="appAccountingId"></param>
        /// <returns></returns>
        public static ACCEN.ApplicationAccountingCostCenter CreateApplicationAccountingCostCenter(ApplicationAccountingCostCenter appAccountingCostCenter, int appAccountingId)
        {
            return new ACCEN.ApplicationAccountingCostCenter(appAccountingCostCenter.Id)
            {
                AppAccountingCostCenterCode = appAccountingCostCenter.Id,
                AppAccountingCode = appAccountingId,
                CostCenterCode = appAccountingCostCenter.CostCenter.CostCenterId,
                Percentage = appAccountingCostCenter.Percentage
            };
        }

        #region TempJournalEntry

        /// <summary>
        /// CreateTempJournalEntry
        /// </summary>
        /// <param name="journalEntryTransactionItem"></param>
        /// <returns>TempJournalEntry</returns>
        public static ACCEN.TempJournalEntry CreateTempJournalEntry(JournalEntry journalEntryTransactionItem)
        {
            return new ACCEN.TempJournalEntry(journalEntryTransactionItem.Id)
            {
                TempJournalEntryCode = journalEntryTransactionItem.Id,
                AccountingDate = journalEntryTransactionItem.AccountingDate,
                BranchCode = journalEntryTransactionItem.Branch.Id,
                Comments = journalEntryTransactionItem.Comments,
                CompanyCode = journalEntryTransactionItem.Company.IndividualId,
                Description = journalEntryTransactionItem.Description,
                IndividualId = journalEntryTransactionItem.Payer.IndividualId,
                PersonTypeCode = journalEntryTransactionItem.PersonType.Id,
                SalesPointCode = journalEntryTransactionItem.SalePoint.Id,
                Status = journalEntryTransactionItem.Status
            };
        }

        #endregion

        #region JournalEntry

        /// <summary>
        /// CreateJournalEntry
        /// </summary>
        /// <param name="journalEntryTransactionItem"></param>
        /// <returns>JournalEntry</returns>
        public static ACCEN.JournalEntry CreateJournalEntry(JournalEntry journalEntryTransactionItem)
        {
            return new ACCEN.JournalEntry(journalEntryTransactionItem.Id)
            {
                JournalEntryCode = journalEntryTransactionItem.Id,
                AccountingDate = journalEntryTransactionItem.AccountingDate,
                BranchCode = journalEntryTransactionItem.Branch.Id,
                Comments = journalEntryTransactionItem.Comments,
                CompanyCode = journalEntryTransactionItem.Company.IndividualId,
                Description = journalEntryTransactionItem.Description,
                IndividualId = journalEntryTransactionItem.Payer.IndividualId,
                PersonTypeCode = journalEntryTransactionItem.PersonType.Id,
                SalesPointCode = journalEntryTransactionItem.SalePoint.Id,
                Status = journalEntryTransactionItem.Status
            };
        }

        #endregion

        #region TempPreLiquidation

        /// <summary>
        /// CreateTempPreliquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>TempPreliquidation</returns>
        public static ACCEN.TempPreliquidation CreateTempPreliquidation(PreLiquidation preLiquidation)
        {
            return new ACCEN.TempPreliquidation(preLiquidation.Id)
            {
                TempPreliquidationCode = preLiquidation.Id,
                RegisterDate = preLiquidation.RegisterDate
            };
        }


        #endregion

        #region PreLiquidation

        /// <summary>
        /// CreatePreliquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>Preliquidation</returns>
        public static ACCEN.Preliquidation CreatePreliquidation(PreLiquidation preLiquidation)
        {
            return new ACCEN.Preliquidation(preLiquidation.Id)
            {
                PreliquidationCode = preLiquidation.Id,
                BranchCode = preLiquidation.Branch.Id,
                SalesPointCode = preLiquidation.SalePoint.Id,
                CompanyCode = preLiquidation.Company.IndividualId,
                IndividualId = preLiquidation.Payer.IndividualId,
                Description = preLiquidation.Description,
                Status = preLiquidation.Status,
                RegisterDate = preLiquidation.RegisterDate.Date,
                PersonTypeCode = preLiquidation.PersonType.Id
            };
        }

        #endregion

        #region DiscountedCommission

        /// <summary>
        /// CreateDiscountedCommission
        /// </summary>
        /// <param name="discountedCommission"></param>
        /// <returns>DiscountedCommission</returns>
        public static ACCEN.DiscountedCommission CreateDiscountedCommission(SCRMOD.DiscountedCommissionDTO discountedCommission)
        {
            return new ACCEN.DiscountedCommission(discountedCommission.DiscountedCommissionId)
            {
                DiscountedCommissionCode = discountedCommission.DiscountedCommissionId,
                PremiumReceivableTransCode = discountedCommission.ApplicationPremiumId,
                AgentTypeCode = discountedCommission.AgentTypeCode,
                AgentIndividualId = discountedCommission.AgentIndividualId,
                CurrencyCode = discountedCommission.CurrencyId,
                ExchangeRate = discountedCommission.ExchangeRate,
                BaseIncomeAmount = discountedCommission.BaseIncomeAmount,
                BaseAmount = discountedCommission.BaseAmount,
                CommissionPercentage = discountedCommission.CommissionPercentage,
                CommissionType = discountedCommission.CommissionType,
                CommissionDiscountIncomeAmount = discountedCommission.CommissionDiscountIncomeAmount,
                CommissionDiscountAmount = discountedCommission.CommissionDiscountAmount
            };
        }

        #endregion

        #region CheckbookControl


        /// <summary>
        /// CreateCheckBookControl
        /// </summary>
        /// <param name="checkBookControl"></param>
        /// <returns>CheckbookControl</returns>
        public static ACCEN.CheckbookControl CreateCheckBookControl(CheckBookControl checkBookControl)
        {
            return new ACCEN.CheckbookControl(checkBookControl.Id)
            {
                CheckbookControlCode = checkBookControl.Id,
                AccountBankCode = checkBookControl.AccountBank.Id,
                IsAutomatic = checkBookControl.IsAutomatic,
                CheckFrom = checkBookControl.CheckFrom,
                CheckTo = checkBookControl.CheckTo,
                LastCheck = checkBookControl.LastCheck,
                Status = checkBookControl.Status,
                DisabledDate = checkBookControl.DisabledDate
            };
        }


        #endregion

        #region TempPaymentOrder

        /// <summary>
        /// CreateTempPaymentOrder
        /// </summary>
        /// <param name="paymentOrderTransactionItem"></param>
        /// <returns>TempPaymentOrder</returns>
        public static ACCEN.TempPaymentOrder CreateTempPaymentOrder(PaymentOrder paymentOrderTransactionItem)
        {
            return new ACCEN.TempPaymentOrder(paymentOrderTransactionItem.Id)
            {
                TempPaymentOrderCode = paymentOrderTransactionItem.Id,
                AccountBankCode = paymentOrderTransactionItem.BankAccountPerson.Id,
                AccountingDate = paymentOrderTransactionItem.AccountingDate,
                Amount = paymentOrderTransactionItem.LocalAmount.Value,
                BranchCdPay = paymentOrderTransactionItem.BranchPay.Id,
                BranchCode = paymentOrderTransactionItem.Branch.Id,
                CompanyCode = paymentOrderTransactionItem.Company.IndividualId,
                CurrencyCode = paymentOrderTransactionItem.Amount.Currency.Id,
                ExchangeRate = paymentOrderTransactionItem.ExchangeRate.BuyAmount,
                IncomeAmount = paymentOrderTransactionItem.Amount.Value,
                IndividualId = paymentOrderTransactionItem.Beneficiary.IndividualId,
                EstimatedPaymentDate = paymentOrderTransactionItem.EstimatedPaymentDate,
                PaymentMethodCode = paymentOrderTransactionItem.PaymentMethod.Id,
                PaymentSourceCode = paymentOrderTransactionItem.PaymentSource.Id,
                PayTo = paymentOrderTransactionItem.PayTo,
                PersonTypeCode = paymentOrderTransactionItem.PersonType.Id,
                RegisterDate = DateTime.Now,
                Status = paymentOrderTransactionItem.Status,
                Observation = paymentOrderTransactionItem.Observation
            };
        }

        #endregion

        #region PaymentOrder

        /// <summary>
        /// CreatePaymentOrder
        /// </summary>
        /// <param name="paymentOrderTransactionItem"></param>
        /// <returns>PaymentOrder</returns>
        public static ACCEN.PaymentOrder CreatePaymentOrder(PaymentOrder paymentOrderTransactionItem)
        {
            return new ACCEN.PaymentOrder(paymentOrderTransactionItem.Id)
            {
                PaymentOrderCode = paymentOrderTransactionItem.Id,
                AccountBankCode = paymentOrderTransactionItem.BankAccountPerson.Id,
                AccountingDate = paymentOrderTransactionItem.AccountingDate,
                Amount = paymentOrderTransactionItem.LocalAmount.Value,
                BranchCdPay = paymentOrderTransactionItem.BranchPay.Id,
                BranchCode = paymentOrderTransactionItem.Branch.Id,
                CompanyCode = paymentOrderTransactionItem.Company.IndividualId,
                CurrencyCode = paymentOrderTransactionItem.Amount.Currency.Id,
                ExchangeRate = paymentOrderTransactionItem.ExchangeRate.BuyAmount,
                IncomeAmount = paymentOrderTransactionItem.Amount.Value,
                IndividualId = paymentOrderTransactionItem.Beneficiary.IndividualId,
                EstimatedPaymentDate = paymentOrderTransactionItem.EstimatedPaymentDate,
                PaymentMethodCode = paymentOrderTransactionItem.PaymentMethod.Id,
                PaymentSourceCode = paymentOrderTransactionItem.PaymentSource.Id,
                PayTo = paymentOrderTransactionItem.PayTo,
                PersonTypeCode = paymentOrderTransactionItem.PersonType.Id,
                RegisterDate = DateTime.Now,
                Status = paymentOrderTransactionItem.Status,
                Observation = paymentOrderTransactionItem.Observation
            };
        }

        #endregion

        #region CheckPaymentOrder

        /// <summary>
        /// CreateCheckPaymentOrder
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>CheckPaymentOrder</returns>
        public static ACCEN.CheckPaymentOrder CreateCheckPaymentOrder(CheckPaymentOrder checkPaymentOrder)
        {
            return new ACCEN.CheckPaymentOrder(checkPaymentOrder.Id)
            {
                CheckPaymentOrderCode = checkPaymentOrder.Id,
                AccountBankCode = checkPaymentOrder.BankAccountCompany.Id,
                CheckNumber = Convert.ToInt32(checkPaymentOrder.CheckNumber),
                IsCheckPrinted = checkPaymentOrder.IsCheckPrinted,
                PrintedUserId = checkPaymentOrder.PrintedUser,
                PrintedDate = checkPaymentOrder.PrintedDate,
                DeliveryDate = checkPaymentOrder.DeliveryDate,
                PersonTypeCode = checkPaymentOrder.PersonType.Id,
                IndividualId = checkPaymentOrder.Delivery.IndividualId,
                CourierName = checkPaymentOrder.CourierName,
                RefundDate = checkPaymentOrder.RefundDate,
                CancellationDate = checkPaymentOrder.CancellationDate,
                CancellationUserId = checkPaymentOrder.CancellationUser,
                Status = checkPaymentOrder.Status
            };
        }


        #endregion

        #region PaymentOrderCheckPayment

        /// <summary>
        /// CreatePaymentOrderCheckPayment
        /// </summary>
        /// <param name="checkPaymentOrderId"></param>
        /// <param name="paymentOrderId"></param>
        /// <returns>PaymentOrderCheckPayment</returns>
        public static ACCEN.PaymentOrderCheckPayment CreatePaymentOrderCheckPayment(int checkPaymentOrderId, int paymentOrderId)
        {
            return new ACCEN.PaymentOrderCheckPayment(0)
            {
                CheckPaymentOrderCode = checkPaymentOrderId,
                PaymentOrderCode = paymentOrderId
            };
        }

        #endregion

        #region TransferFormatBank

        /// <summary>
        /// CreateTransferFormatBank
        /// </summary>
        /// <param name="transferFormatBankCode"></param>
        /// <param name ="bankCode"></param>
        /// <param name ="formatCode"></param>
        /// <returns>TransferFormatBank</returns>
        public static ACCEN.TransferFormatBank CreateTransferFormatBank(int transferFormatBankCode, int bankCode, int formatCode)
        {
            return new ACCEN.TransferFormatBank(transferFormatBankCode)
            {
                TransferFormatBankCode = transferFormatBankCode,
                BankCode = bankCode,
                FormatCode = formatCode
            };
        }

        #endregion

        #region PaymentOrderTransferPayment

        /// <summary>
        /// CreatePaymentOrderTransferPayment
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="transferPaymentOrderId"></param>
        /// <returns>PaymentOrderTransferPayment</returns>
        public static ACCEN.PaymentOrderTransferPayment CreatePaymentOrderTransferPayment(int paymentOrderId, int transferPaymentOrderId)
        {
            return new ACCEN.PaymentOrderTransferPayment(0)
            {
                PaymentOrderTransferPaymentCode = 0,
                PaymentOrderCode = paymentOrderId,
                TransferPaymentOrderCode = transferPaymentOrderId
            };
        }

        #endregion

        #region TransferPaymentOrder

        /// <summary>
        /// CreateTransferPaymentOrder
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        public static ACCEN.TransferPaymentOrder CreateTransferPaymentOrder(TransferPaymentOrder transferPaymentOrder)
        {
            return new ACCEN.TransferPaymentOrder(transferPaymentOrder.Id)
            {
                TransferPaymentOrderCode = transferPaymentOrder.Id,
                AccountBankCode = transferPaymentOrder.BankAccountCompany.Id,
                DeliveryDate = transferPaymentOrder.DeliveryDate,
                Status = transferPaymentOrder.Status,
                CancellationDate = transferPaymentOrder.CancellationDate,
                User = transferPaymentOrder.UserId
            };
        }

        #endregion

        #region OtherPaymentsRequest

        /// <summary>
        /// CreateOtherPaymentsRequest
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns>OtherPaymentsRequest</returns>
        public static ACCEN.OtherPaymentsRequest CreateOtherPaymentsRequest(int collectId, int paymentRequestId)
        {
            return new ACCEN.OtherPaymentsRequest(0)
            {
                OtherPaymentsRequestCode = 0,
                CollectCode = collectId,
                PaymentRequestCode = paymentRequestId
            };
        }

        /// <summary>
        /// CreatePaymentRequestVarious
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public static ACCEN.PaymentRequestVarious CreatePaymentRequestVarious(int collectId, int paymentRequestId)
        {
            int? collectCode = 0;

            if (collectId == 0)
            {
                collectCode = null;
            }
            else
            {
                collectCode = collectId;
            }

            return new ACCEN.PaymentRequestVarious(0)
            {
                PaymentRequestVariousId = 0,
                CollectCode = collectCode,
                PaymentRequestCode = paymentRequestId
            };
        }

        #endregion

        #region PaymentOrderAgent

        /// <summary>
        /// CreatePaymentOrderAgent
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="companyId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentOrderAgent</returns>
        public static ACCEN.PaymentOrderAgent CreatePaymentOrderAgent(int branchId, int companyId, DateTime estimatedPaymentDate, int userId)
        {
            return new ACCEN.PaymentOrderAgent(0)
            {
                BranchCode = branchId,
                CompanyCode = companyId,
                EstimatedPaymentDate = estimatedPaymentDate,
                PaymentOrderAgentCode = 0,
                RegisterDate = DateTime.Now,
                UserId = userId
            };
        }

        #endregion

        #region PaymentOrderBrokerAccount

        /// <summary>
        /// CreatePaymentOrderBrokerAccount
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="brokerCheckingAccountId"></param>
        /// <returns>PaymentOrderBrokerAccount</returns>
        public static ACCEN.PaymentOrderBrokerAccount CreatePaymentOrderBrokerAccount(int paymentOrderId, int brokerCheckingAccountId)
        {
            return new ACCEN.PaymentOrderBrokerAccount(0)
            {
                PaymentOrderId = paymentOrderId,
                BrokerCheckingAccountId = brokerCheckingAccountId
            };
        }

        #endregion PaymentOrderBrokerAccount

        #region PaymentOrderAgentItem

        /// <summary>
        /// CreatePaymentOrderAgentItem
        /// </summary>
        /// <param name="paymentOrderAgentId"></param>
        /// <param name="paymentOrderId"></param>
        /// <returns>PaymentOrderAgentItem</returns>
        public static ACCEN.PaymentOrderAgentItem CreatePaymentOrderAgentItem(int paymentOrderAgentId, int paymentOrderId)
        {
            return new ACCEN.PaymentOrderAgentItem(0)
            {
                PaymentOrderAgentCode = paymentOrderAgentId,
                PaymentOrderAgentItemCode = 0,
                PaymentOrderCode = paymentOrderId
            };
        }

        #endregion

        #region AccountingCompany

        /// <summary>
        /// CreateAccountingCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>AccountingCompany</returns>
        public static ACCEN.Company CreateAccountingCompany(Company company)
        {
            return new ACCEN.Company(company.IndividualId)
            {
                CompanyId = company.IndividualId,
                Description = company.FullName
            };
        }

        #endregion

        #region CurrencyDifference

        /// <summary>
        /// CreateCurrencyDifference
        /// </summary>
        /// <param name="currencyCode"> </param>
        /// <param name="maxDifference"> </param>
        /// <param name="percentageDifference"> </param>
        /// <returns>CurrencyDifference</returns>
        public static ACCEN.CurrencyDifference CreateCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference)
        {
            return new ACCEN.CurrencyDifference(currencyCode)
            {
                CurrencyCode = currencyCode,
                MaximumDifference = maxDifference,
                PercentageDifference = percentageDifference
            };

        }

        #endregion

        #region BrokerBalance

        /// <summary>
        /// CreateBrokerBalance
        /// </summary>
        /// <param name="brokerBalance"> </param>
        /// <returns>BrokerBalance</returns>
        public static ACCEN.BrokerBalance CreateBrokerBalance(SCRMOD.BrokerBalanceDTO brokerBalance)
        {
            return new ACCEN.BrokerBalance(brokerBalance.BrokerBalanceId)
            {
                BrokerBalanceCode = brokerBalance.BrokerBalanceId,
                AgentTypeCode = brokerBalance.AgentTypeCode,
                AgentId = brokerBalance.AgentCode,
                BalanceDate = Convert.ToDateTime(brokerBalance.BalanceDate),
                CurrencyCode = brokerBalance.CurrencyId,
                LastBalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate),
                PartialBalanceAmount = brokerBalance.PartialBalanceAmount,
                PartialBalanceIncomeAmount = brokerBalance.PartialBalanceIncomeAmount,
                TaxPartialSum = brokerBalance.TaxPartialSum,
                TaxPartialSubtraction = brokerBalance.TaxPartialSubtraction,
                TaxSum = brokerBalance.TaxSum,
                TaxSubtraction = brokerBalance.TaxSubtraction,
                NumSheet = brokerBalance.NumSheet
            };
        }

        #endregion

        #region CoinsuranceBalance

        /// <summary>
        /// CreateCoinsuranceBalance
        /// </summary>
        /// <param name="coinsuranceBalanceDto"> </param>
        /// <returns>CoinsuranceBalance</returns>
        public static ACCEN.CoinsuranceBalance CreateCoinsuranceBalance(SCRMOD.CoinsuranceBalanceDTO coinsuranceBalanceDto)
        {
            return new ACCEN.CoinsuranceBalance(coinsuranceBalanceDto.CoinsuranceBalanceId)
            {
                CoinsuranceBalanceCode = coinsuranceBalanceDto.CoinsuranceBalanceId,
                CoinsuredCompanyId = coinsuranceBalanceDto.CoinsuredCompanyId,
                BalanceDate = Convert.ToDateTime(coinsuranceBalanceDto.BalanceDate),
                CurrencyCode = Convert.ToInt32(coinsuranceBalanceDto.CurrencyId),
                LastBalanceDate = Convert.ToDateTime(coinsuranceBalanceDto.LastBalanceDate),
                BalanceAmount = Convert.ToDecimal(coinsuranceBalanceDto.BalanceAmount),
                BalanceIncomeAmount = Convert.ToDecimal(coinsuranceBalanceDto.BalanceIncomeAmount),
                NumSheet = Convert.ToInt32(coinsuranceBalanceDto.NumSheet)
            };
        }

        #endregion

        #region AgentCoinsuranceCheckingAccount

        /// <summary>
        /// CreateAgentCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="currencyCode"> </param>
        /// <param name="agentTypeCode"> </param>
        /// <param name="agentId"> </param>
        /// <param name="commissionAmount"> </param>
        /// <param name="incomeCommissionAmount"> </param>
        /// <returns>AgentCoinsuranceCheckingAccount</returns>
        public static ACCEN.AgentCoinsuranceCheckingAccount CreateAgentCoinsuranceCheckingAccount(int currencyCode, int agentTypeCode, int agentId, decimal commissionAmount, decimal incomeCommissionAmount)
        {
            return new ACCEN.AgentCoinsuranceCheckingAccount(0)
            {
                CurrencyCode = currencyCode,
                AgentTypeCode = agentTypeCode,
                AgentId = agentId,
                CommissionAmount = commissionAmount,
                IncomeCommissionAmount = incomeCommissionAmount
            };
        }

        #endregion

        #region AgentCommissionClosure

        /// <summary>
        /// CreateAgentCommissionClosure
        /// </summary>
        /// <param name="userId"> </param>
        /// <param name="startDate"> </param>
        /// <param name="endDate"> </param>
        /// <param name="registerDate"> </param>
        /// <param name="status"> </param>
        /// <returns>AgentCommissionClosure</returns>
        public static ACCEN.AgentCommissionClosure CreateAgentCommissionClosure(int userId, DateTime startDate, DateTime endDate, DateTime registerDate, int status)
        {
            return new ACCEN.AgentCommissionClosure(0)
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
                RegisterDate = registerDate,
                Status = status
            };
        }

        #endregion

        #region AgentCommissionBalance


        /// <summary>
        /// CreateAgentCommissionBalance
        /// </summary>
        /// <param name="agentCommissionBalance"> </param>
        /// <returns>AgentCommissionBalance</returns>
        public static ACCEN.AgentCommissionBalance CreateAgentCommissionBalance(SCRMOD.AgentCommissionBalanceDTO agentCommissionBalance)
        {
            return new ACCEN.AgentCommissionBalance(agentCommissionBalance.AgentCommissionBalanceCode)
            {
                AgentCommissionBalanceCode = agentCommissionBalance.AgentCommissionBalanceCode,
                BranchCode = agentCommissionBalance.BranchCode,
                CompanyCode = agentCommissionBalance.CompanyCode,
                StartDate = agentCommissionBalance.StartDate,
                EndDate = agentCommissionBalance.EndDate,
                AgentId = agentCommissionBalance.AgentId,
                UserId = agentCommissionBalance.UserId,
                RegisterDate = agentCommissionBalance.RegisterDate,
                Status = agentCommissionBalance.Status
            };
        }

        #endregion

        #region AgentCommissionBalanceItem


        /// <summary>
        /// CreateAgentCommissionBalanceItem
        /// </summary>
        /// <param name="agentCommissionBalanceitem"></param>
        /// <returns>AgentCommissionBalanceItem</returns>
        public static ACCEN.AgentCommissionBalanceItem CreateAgentCommissionBalanceItem(SCRMOD.AgentCommissionBalanceItemDTO agentCommissionBalanceitem)
        {
            return new ACCEN.AgentCommissionBalanceItem(agentCommissionBalanceitem.AgentCommissionBalanceItemCode)
            {
                AgentCommissionBalanceItemCode = agentCommissionBalanceitem.AgentCommissionBalanceItemCode,
                AgentCommissionBalanceCode = agentCommissionBalanceitem.AgentCommissionBalanceCode,
                BranchCode = agentCommissionBalanceitem.BranchCode,
                AgentId = agentCommissionBalanceitem.AgentId,
                InsuredId = agentCommissionBalanceitem.InsuredId,
                PrefixId = agentCommissionBalanceitem.PrefixId,
                LineBusinessCode = agentCommissionBalanceitem.LineBusinessCode,
                PolicyId = agentCommissionBalanceitem.PolicyId,
                EndorsementId = agentCommissionBalanceitem.EndorsementId,
                CommissionTypeCode = agentCommissionBalanceitem.CommissionTypeCode,
                CommissionPercentage = agentCommissionBalanceitem.CommissionPercentage,
                CommissionAmount = agentCommissionBalanceitem.CommissionAmount,
                CommissionDiscounted = agentCommissionBalanceitem.CommissionDiscounted,
                CommissionTax = agentCommissionBalanceitem.CommissionTax,
                CommissionRetention = agentCommissionBalanceitem.CommissionRetention,
                CommissionBalance = agentCommissionBalanceitem.CommissionBalance
            };
        }

        #endregion

        #region CollectMassiveProcess

        /// <summary>
        /// CreateBillMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"> </param>
        /// <returns>CollectMassiveProcess</returns>
        public static ACCEN.CollectMassiveProcess CreateBillMassiveProcess(SCRMOD.CollectMassiveProcessDTO collectMassiveProcess)
        {
            DateTime? endDate;

            if (collectMassiveProcess.EndDate == Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                endDate = null;
            }
            else
            {
                endDate = collectMassiveProcess.EndDate;
            }


            return new ACCEN.CollectMassiveProcess(collectMassiveProcess.CollectMassiveProcessId)
            {
                CollectMassiveProcessId = collectMassiveProcess.CollectMassiveProcessId,
                BeginDate = collectMassiveProcess.BeginDate,
                EndDate = endDate,
                UserId = collectMassiveProcess.UserId,
                Status = collectMassiveProcess.Status,
                TotalRecords = collectMassiveProcess.TotalRecords,
                FailedRecords = collectMassiveProcess.FailedRecords,
                SuccessfulRecords = collectMassiveProcess.SuccessfulRecords
            };
        }

        #endregion

        #region TempPaymentRequestClaim

        /// <summary>
        /// CreateTempPaymentRequestClaim
        /// </summary>
        /// <param name="paymentRequestInfo"></param>
        /// <param name="claim"></param>
        /// <param name="indexCoverage"></param>
        /// <param name="indexAmount"></param>
        /// <param name="indexVoucher"></param>
        /// <returns>TempPaymentRequestClaim</returns>
        public static PAYENT.TempPaymentRequestClaim CreateTempPaymentRequestClaim(Models.AccountsPayables.PaymentRequest paymentRequestInfo, CLMOD.Claim claim,
                                                                            int indexCoverage, int indexAmount, int indexVoucher, int estimationTypeId, int voucherConceptId, int paymentTypeId)
        {
            return new PAYENT.TempPaymentRequestClaim(paymentRequestInfo.Id, claim.Id, claim.Modifications.Last().Coverages[indexCoverage].SubClaim, estimationTypeId, voucherConceptId)
            {
                PaymentRequestCode = paymentRequestInfo.Id,
                ClaimCode = claim.Id,
                SubClaim = claim.Modifications.Last().Coverages[indexCoverage].SubClaim,
                VoucherConceptCode = voucherConceptId,
                EstimationTypeCode = estimationTypeId,
                PaymentTypeCode = paymentTypeId,
                BranchCode = claim.Branch.Id,
                PrefixCode = claim.Prefix.Id,
                ClaimNumber = claim.Number
            };
        }

        #endregion

        #region PaymentRequestClaim

        /// <summary>
        /// CreatePaymentRequestClaim
        /// </summary>
        /// <param name="paymentRequestClaim"></param>
        /// <returns>PaymentRequestClaim</returns>
        public static PAYENT.PaymentRequestClaim CreatePaymentRequestClaim(Array paymentRequestClaim)
        {
            return new PAYENT.PaymentRequestClaim(Convert.ToInt32(paymentRequestClaim.GetValue(0)), Convert.ToInt32(paymentRequestClaim.GetValue(1)),
                                           Convert.ToInt32(paymentRequestClaim.GetValue(2)), Convert.ToInt32(paymentRequestClaim.GetValue(3)),
                                           Convert.ToInt32(paymentRequestClaim.GetValue(4)))
            {
                PaymentRequestCode = Convert.ToInt32(paymentRequestClaim.GetValue(0)),
                ClaimCode = Convert.ToInt32(paymentRequestClaim.GetValue(1)),
                SubClaim = Convert.ToInt32(paymentRequestClaim.GetValue(2)),
                EstimationTypeCode = Convert.ToInt32(paymentRequestClaim.GetValue(3)),
                PaymentVoucherConceptCode = Convert.ToInt32(paymentRequestClaim.GetValue(4)),
                PaymentTypeCode = Convert.ToInt32(paymentRequestClaim.GetValue(5)),
                BranchCode = Convert.ToInt32(paymentRequestClaim.GetValue(6)),
                PrefixCode = Convert.ToInt32(paymentRequestClaim.GetValue(7)),
                ClaimNumber = Convert.ToInt32(paymentRequestClaim.GetValue(8))
            };
        }

        #endregion

        #region PaymentRecovery

        /// <summary>
        /// CreatePaymentRecovery
        /// </summary>
        /// <param name="paymentRecovery"></param>
        /// <returns>PaymentRecovery</returns>
        public static PAYENT.PaymentRecovery CreatePaymentRecovery(Array paymentRecovery)
        {
            return new PAYENT.PaymentRecovery(Convert.ToInt16(paymentRecovery.GetValue(0)))
            {
                PaymentRequestCode = Convert.ToInt16(paymentRecovery.GetValue(1)),
                RecoveryCode = Convert.ToInt16(paymentRecovery.GetValue(2))
            };
        }


        /// <summary>
        /// CreatePaymentSalvage
        /// </summary>
        /// <param name="paymentRecovery"></param>
        /// <returns>PaymentRecovery</returns>
        public static PAYENT.PaymentRecovery CreatePaymentSalvage(Array paymentRecovery)
        {
            return new PAYENT.PaymentRecovery(Convert.ToInt16(paymentRecovery.GetValue(0)))
            {
                PaymentRequestCode = Convert.ToInt16(paymentRecovery.GetValue(1)),
                SalvageCode = Convert.ToInt16(paymentRecovery.GetValue(2))
            };
        }


        #endregion

        #region Automatic Debit

        /// <summary>
        /// CreateNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public static ACCEN.BankNetwork CreateNetwork(BankNetwork bankNetwork)
        {
            return new ACCEN.BankNetwork(bankNetwork.Id)
            {
                BankNetworkId = bankNetwork.Id,
                Description = bankNetwork.Description,
                Commission = bankNetwork.Commission.Value > 0,
                Tax = bankNetwork.HasTax,
                MaximumCoupon = bankNetwork.RetriesNumber,
                TypePercentageCommission = bankNetwork.TaxCategory.Id,
                CommissionRate = 0,
                CommissionAmount = bankNetwork.Commission.Value,
                Header = false,
                Summary = false,
                Prenotification = bankNetwork.RequiresNotification
            };
        }

        /// <summary>
        /// CreateNetworkPaymentMethod
        /// Devuelve una relación del Conducto de Pago por Red y Banco
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public static ACCEN.PaymentMethodBankNetwork CreatePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return new ACCEN.PaymentMethodBankNetwork(paymentMethodBankNetwork.BankNetwork.Id, paymentMethodBankNetwork.PaymentMethod.Id,
                paymentMethodBankNetwork.BankAccountCompany.Id)
            {
                Generate = paymentMethodBankNetwork.ToGenerate,
                Identifier = ""
            };
        }

        /// <summary>
        /// CreatePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public static ACCEN.PaymentMethodAccountType CreatePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            return new ACCEN.PaymentMethodAccountType(paymentMethodAccountType.PaymentMethod.Id,
                                                paymentMethodAccountType.BankAccountType.Id)
            {
                AccountTypeCode = paymentMethodAccountType.BankAccountType.Id,
                PaymentMethodCode = paymentMethodAccountType.PaymentMethod.Id,
                DebitCode = paymentMethodAccountType.SmallDescriptionDebit
            };
        }

        /// <summary>
        /// CreateDebitDesign
        /// </summary>
        /// <param name="format"></param>
        /// <returns>DebitDesign</returns>
        public static ACCEN.DebitDesign CreateDebitDesign(Format format)
        {
            return new ACCEN.DebitDesign(0)
            {
                DebitDesignId = format.Id,
                Description = format.Description,
                BankNetworkCode = format.BankNetwork.Id,
                StartDate = format.Date,
            };
        }

        /// <summary>
        /// CreateFormatDesign
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatDesignId"></param>
        /// <param name="formatTypeId"></param>
        /// <param name="formatUsingTypeId"></param>
        /// <returns>FormatDesign</returns>
        public static ACCEN.FormatDesign CreateFormatDesign(Format format, int formatDesignId, int formatTypeId, int formatUsingTypeId)
        {
            return new ACCEN.FormatDesign(formatDesignId, format.Id)
            {
                FormatTypeCode = formatTypeId,
                UseFileCode = formatUsingTypeId,
                Separator = format.Separator
            };
        }

        /// <summary>
        /// CreateFormatDesignDetail
        /// </summary>
        /// <param name="format"></param>
        /// <returns>FormatDesignDetail</returns>
        public static ACCEN.FormatDesignDetail CreateFormatDesignDetail(Format format)
        {
            return new ACCEN.FormatDesignDetail(format.Id, Convert.ToInt32(format.Description), format.Fields[0].Id)
            {
                DebitDesignCode = format.Id,
                FormatDesignCode = Convert.ToInt32(format.Description),
                NumberColumn = format.Fields[0].Id,
                DescriptionColumn = format.Fields[0].Description,
                StartNumber = format.Fields[0].Start,
                NumberLength = format.Fields[0].Length,
                Value = format.Fields[0].Value ?? " ",
                Field = format.Fields[0].ExternalDescription ?? " ",
                Format = format.Fields[0].Mask,
                Filler = format.Fields[0].Filled ?? " ",
                Alignment = format.Fields[0].Align ?? " "
            };
        }

        #endregion

        #region ShipmentStatus

        /// <summary>
        /// CreateShipmentStatus
        /// </summary>
        /// <param name="automaticDebitStatusId"></param>
        /// <param name="automaticDebitDescription"></param>
        /// <returns>AutomaticDebitStatus</returns>
        public static ACCEN.AutomaticDebitStatus CreateShipmentStatus(int automaticDebitStatusId, string automaticDebitDescription)
        {
            return new ACCEN.AutomaticDebitStatus(automaticDebitStatusId)
            {
                Description = automaticDebitDescription,
            };
        }

        #endregion

        #region LogBankResponse

        /// <summary>
        /// CreateLogBankResponse
        /// </summary>
        /// <param name="logBankResponse"></param>
        /// <returns>LogBankResponse</returns>
        public static ACCEN.LogBankResponse CreateLogBankResponse(Array logBankResponse)
        {
            return new ACCEN.LogBankResponse(0)
            {

                BankNetworkId = Convert.ToInt32(logBankResponse.GetValue(1)),
                LotNumber = Convert.ToString(logBankResponse.GetValue(2)),
                LineNumber = Convert.ToInt32(logBankResponse.GetValue(3)),
                CardAccountNumber = Convert.ToString(logBankResponse.GetValue(4)),
                VoucherNumber = Convert.ToString(logBankResponse.GetValue(5)),
                RejectionCode = Convert.ToString(logBankResponse.GetValue(6)),
                ApplicationDate = Convert.ToDateTime(Convert.ToDateTime(logBankResponse.GetValue(7)).ToShortDateString()),
                PremiumAmount = Convert.ToDecimal(logBankResponse.GetValue(8)),
                RegisterDate = Convert.ToDateTime(Convert.ToDateTime(logBankResponse.GetValue(9)).ToShortDateString()),
                AuthorizationNumber = Convert.ToString(logBankResponse.GetValue(10)),
                DocumentNumber = Convert.ToString(logBankResponse.GetValue(11)),
                IsPrenotificacion = Convert.ToBoolean(logBankResponse.GetValue(12)),
                DescriptionError = Convert.ToString(logBankResponse.GetValue(13)),
                UserCode = Convert.ToInt32(logBankResponse.GetValue(14)),
            };
        }

        #endregion

        #region LogSpecialProcess

        /// <summary>
        /// CreateLogSpecialProcess
        /// </summary>
        /// <param name="creditNoteProcess"></param>
        /// <param name="processTypeId"></param>
        /// <param name="tempImpuationId"></param>
        /// <returns>LogSpecialProcess</returns>
        public static ACCEN.LogSpecialProcess CreateLogSpecialProcess(CreditNote creditNoteProcess, int processTypeId, int tempImpuationId)
        {
            return new ACCEN.LogSpecialProcess(creditNoteProcess.Id)
            {
                LogSpecialProcessId = creditNoteProcess.Id,
                ProcessDate = creditNoteProcess.Date,
                BranchId = creditNoteProcess.CreditNoteItems[0].NegativePolicy.Branch.Id,
                ProcessTypeId = processTypeId,
                IncomeAmount = 0,
                ExchangeRate = 0,
                Amount = 0,
                UserId = creditNoteProcess.UserId,
                ImputationId = 0,
                TempImputationId = tempImpuationId,
                StartDate = creditNoteProcess.Date,
                Status = (int)CreditNoteStatus.Actived
            };
        }

        /// <summary>
        /// CreateLogSpecialProcess
        /// </summary>
        /// <param name="amortization"></param>
        /// <param name="processTypeId"></param>
        /// <param name="tempImpuationId"></param>
        /// <returns></returns>
        public static ACCEN.LogSpecialProcess CreateLogSpecialProcess(Amortization amortization, int processTypeId, int tempImpuationId)
        {
            return new ACCEN.LogSpecialProcess(amortization.Id)
            {
                Amount = 0,
                BranchId = amortization.Policies[0].Branch.Id,
                EndDate = null,
                ExchangeRate = 0,
                ImputationId = 0,
                ImputationReceiptNumber = null,
                IncomeAmount = 0,
                LogSpecialProcessId = amortization.Id,
                LowDate = null,
                LowUserId = null,
                ProcessDate = amortization.Date,
                ProcessTypeId = processTypeId,
                RecordsProcessed = null,
                StartDate = amortization.Date,
                Status = (int)AmortizationStatus.Actived,
                TempImputationId = tempImpuationId,
                UserId = amortization.UserId
            };
        }

        #endregion

        #region Range

        /// <summary>
        /// CreateRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public static ACCEN.Range CreateRange(Range range)
        {
            return new ACCEN.Range(range.Id)
            {
                RangeCode = range.Id,
                Description = range.Description,
                RangeDefault = range.IsDefault
            };
        }

        /// <summary>
        /// CreateRanges
        /// </summary>
        /// <param name="rangeItem"></param>
        /// <param name="rangeCode"></param>
        /// <returns>RangeItem</returns>
        public static ACCEN.RangeItem CreateRanges(RangeItem rangeItem, int rangeCode)
        {
            return new ACCEN.RangeItem(rangeCode, rangeItem.Order)
            {
                RangeItemCode = rangeCode,
                RangeOrder = rangeItem.Order,
                FromValue = rangeItem.RangeFrom.ToString(),
                ToValue = rangeItem.RangeTo.ToString()
            };
        }

        #endregion

        #region CancellationPolicies

        /// <summary>
        /// CreateCancelationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationDayPrefix</returns>
        public static ACCEN.CancellationLimit CreateCancelationLimit(CancellationLimit cancellationLimit)
        {
            return new ACCEN.CancellationLimit(0)
            {
                PrefixCode = cancellationLimit.Branch.Id,
                CancellationLimitDays = cancellationLimit.CancellationLimitDays
            };
        }

        /// <summary>
        /// CreateExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        /// <returns>Exclusion</returns>
        public static ACCEN.Exclusion CreateExclusion(Exclusion exclusion)
        {
            ACCEN.Exclusion exclusionE = new ACCEN.Exclusion(exclusion.Id);
            int agentExclusionType = exclusion.Agent.IndividualId > 0 ? 2 : 3;
            int exclusionType = exclusion.Policy.Id > 0 ? 1 : agentExclusionType;

            exclusionE.ExclusionType = exclusionType;
            exclusionE.UserId = exclusion.Policy.UserId;
            exclusionE.DateEntry = DateTime.Now;

            if (exclusionType == 1)
            {
                exclusionE.IndividualId = null;
                exclusionE.PolicyId = exclusion.Policy.Id;
                exclusionE.BranchCode = exclusion.Policy.Branch.Id;
                exclusionE.PrefixCode = exclusion.Policy.Prefix.Id;
            }
            else
            {
                exclusionE.IndividualId = (exclusionType == 2 ? exclusion.Agent.IndividualId : exclusion.Insured.IndividualId);
                exclusionE.PolicyId = null;
                exclusionE.BranchCode = null;
                exclusionE.PrefixCode = null;
            }

            return exclusionE;
        }

        #endregion

        #region AccountBank

        /// <summary>
        /// CreateAccountBank
        /// </summary>
        /// <param name="accountBank"></param>
        /// <returns>AccountBank</returns>
        /// 
        public static UPENT.AccountBank CreateBankAccountPerson(BankAccountPerson accountBank)
        {
            return new UPENT.AccountBank(accountBank.Id, accountBank.Individual.IndividualId)
            {
                AccountBankCode = accountBank.Id,
                IndividualId = accountBank.Individual.IndividualId,
                AccountTypeCode = accountBank.BankAccountType.Id,
                Number = Convert.ToString(accountBank.Number),
                BankCode = accountBank.Bank.Id,
                Enabled = accountBank.IsEnabled,
                Default = accountBank.IsDefault,
                CurrencyCode = accountBank.Currency.Id
            };
        }

        #endregion AccountBank

        #region TempVoucher

        /// <summary>
        /// CreateTempVoucher
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns>TempVoucher</returns>
        public static COMMENT.TempVoucher CreateTempVoucher(Models.AccountsPayables.Voucher voucher)
        {
            return new COMMENT.TempVoucher(voucher.Id)
            {
                VoucherCode = voucher.Id,
                VoucherTypeCode = voucher.Type.Id,
                Number = voucher.Number,
                Date = voucher.Date,
                ExchangeRate = Convert.ToDecimal(voucher.ExchangeRate),
                CurrencyCode = voucher.Currency.Id
            };
        }

        #endregion TempVoucher

        #region BankAccountCompany

        /// <summary>
        /// CreateBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="description"></param>
        /// <returns>BankAccountCompany</returns>
        public static ACCEN.BankAccountCompany CreateBankAccountCompany(BankAccountCompany bankAccountCompany, string description)
        {
            DateTime? disabled;

            if ((bankAccountCompany.DisableDate.ToString() == "01/01/0001 0:00:00") ||
                (bankAccountCompany.DisableDate.ToString("dd/MM/yyyy") == "01/01/0001") ||
                (bankAccountCompany.DisableDate.ToString() == "01/01/1900 0:00:00") ||
                (bankAccountCompany.DisableDate.ToString("dd/MM/yyyy") == "01/01/1900")
                )
            {
                disabled = null;
            }
            else
            {
                disabled = bankAccountCompany.DisableDate;
            }

            return new ACCEN.BankAccountCompany(bankAccountCompany.Id)
            {
                BankAccountCompanyId = bankAccountCompany.Id,
                AccountTypeCode = bankAccountCompany.BankAccountType.Id,
                AccountNumber = Convert.ToString(bankAccountCompany.Number),
                BankCode = bankAccountCompany.Bank.Id,
                Enabled = bankAccountCompany.IsEnabled,
                Default = bankAccountCompany.IsDefault,
                CurrencyCode = bankAccountCompany.Currency.Id,
                AccountingAccountId = bankAccountCompany.AccountingAccount.AccountingAccountId,
                DisabledDate = disabled,
                BranchCode = bankAccountCompany.Branch.Id,
                Description = description,
            };
        }

        #endregion AccountBankCommon

        #region LogPolicyCancellationProcess


        /// <summary>
        /// CreateLogPolicyCancellation
        /// </summary>
        /// <param name="logPolicyCancellationId"></param>
        /// <param name="branch"></param>
        /// <param name="salePoint"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="intermediary"></param>
        /// <param name="grouper"></param>
        /// <param name="business"></param>
        /// <param name="dueDate"></param>
        /// <param name="issuanceDateFrom"></param>
        /// <param name="issuanceDateTo"></param>
        /// <param name="cancellationPolicyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ACCEN.LogPolicyCancellation CreateLogPolicyCancellation(int logPolicyCancellationId, Branch branch, SalePoint salePoint, Prefix prefix,
                                                                        Policy policy, Person insured, Person intermediary,
                                                                        int grouper, int business, DateTime dueDate,
                                                                        DateTime issuanceDateFrom, DateTime issuanceDateTo,
                                                                        CancellationPolicyType cancellationPolicyType, int userId)
        {
            return new ACCEN.LogPolicyCancellation(logPolicyCancellationId)
            {
                BranchCode = branch.Id,
                BusinessCode = (int)policy.BusinessType,
                CancellationDate = null,
                CancellationTypeCode = cancellationPolicyType.Id,
                CancellationUser = null,
                CutDate = dueDate,
                EndDate = null,
                ExecutionUser = userId,
                GrouperCode = null,
                InsuredCode = null,
                IntermediaryCode = null,
                IssueDateFrom = null,
                IssueDateTo = null,
                LogPolicyCancellationId = logPolicyCancellationId,
                PolicyNumber = null,
                PrefixCode = null,
                ProcessDate = DateTime.Now,
                ProcessedPolicies = null,
                ProcessStatus = 1,
                SalePointCode = null,
                StartDate = DateTime.Now,
                Status = 0
            };
        }


        #endregion

        #region PaymentRequestNumber

        /// <summary>
        /// CreatePaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns></returns>
        public static ACCEN.PaymentRequestNumber CreatePaymentRequestNumber(PaymentRequestNumber paymentRequestNumber)
        {
            return new ACCEN.PaymentRequestNumber(paymentRequestNumber.Branch.Id)
            {
                BranchCode = paymentRequestNumber.Branch.Id,
                Number = paymentRequestNumber.Number
            };
        }

        #endregion PaymentRequestNumber

        #region PaymentRequest

        /// <summary>
        /// CreatePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public static ACCEN.PaymentRequest CreatePaymentRequest(Models.AccountsPayables.PaymentRequest paymentRequest)
        {
            DateTime? estimatedPaymentDate;
            DateTime? accountingDate;
            int? personBankAccountCode;
            int? prefixId;

            if (paymentRequest.EstimatedDate == Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                estimatedPaymentDate = null;
            }
            else
            {
                estimatedPaymentDate = paymentRequest.EstimatedDate;
            }

            if (paymentRequest.AccountingDate == Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                accountingDate = null;
            }
            else
            {
                accountingDate = paymentRequest.AccountingDate;
            }

            if (paymentRequest.Branch.Description == "0")
            {
                personBankAccountCode = null;
            }
            else
            {
                personBankAccountCode = Convert.ToInt32(paymentRequest.Branch.Description); // no existe modelo de cuenta bancaria
            }

            prefixId = null;

            return new ACCEN.PaymentRequest(paymentRequest.Id)
            {
                PaymentRequestId = paymentRequest.Id,
                PaymentRequestType = Convert.ToInt32(paymentRequest.PaymentRequestType),
                Number = Convert.ToInt32(paymentRequest.PaymentRequestNumber.Number),
                ConceptSourceCode = Convert.ToInt32(paymentRequest.MovementType.ConceptSource.Id),
                MovementTypeCode = Convert.ToInt32(paymentRequest.MovementType.Id),
                CompanyCode = Convert.ToInt32(paymentRequest.Company.IndividualId),
                BranchCode = Convert.ToInt32(paymentRequest.Branch.Id),
                SalePointCode = Convert.ToInt32(paymentRequest.SalePoint.Id),
                PersonTypeCode = Convert.ToInt32(paymentRequest.PersonType.Id),
                BeneficiaryCode = Convert.ToInt32(paymentRequest.Beneficiary.IndividualId),
                PaymentMethodTypeCode = Convert.ToInt32(paymentRequest.PaymentMethod.Id),
                CurrencyCode = Convert.ToInt32(paymentRequest.Currency.Id),
                TotalAmount = Convert.ToDecimal(paymentRequest.TotalAmount.Value),
                EstimatedPaymentDate = estimatedPaymentDate,
                AccountingDate = accountingDate,
                RegisterDate = DateTime.Now,
                Description = paymentRequest.Description,
                UserId = Convert.ToInt32(paymentRequest.UserId),
                PaymentDate = null,           //pendiente definición para este dato.
                PaymentStatus = 0,            //pendiente definición para este dato.
                PaymentUserId = null,         //pendiente definición para este dato.
                PersonBankAccountCode = personBankAccountCode,
                PrefixCode = prefixId,
                TechnicalTransaction = null,
                AccountingTransaction = null
            };
        }

        #endregion PaymentRequest

        #region Voucher

        /// <summary>
        /// CreateVoucher
        /// </summary>
        /// <param name="voucher"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public static ACCEN.Voucher CreateVoucher(Models.AccountsPayables.Voucher voucher, int paymentRequestId)
        {
            DateTime? date = null;

            if (voucher.Date != Convert.ToDateTime("01/01/0001 0:00:00"))
            {
                date = voucher.Date;
            }

            return new ACCEN.Voucher(voucher.Id)
            {
                VoucherId = voucher.Id,
                PaymentRequestCode = paymentRequestId,
                VoucherTypeCode = voucher.Type.Id,
                Number = voucher.Number,
                Date = date,
                CurrencyCode = voucher.Currency.Id,
                ExchangeRate = voucher.ExchangeRate
            };
        }

        #endregion Voucher

        #region VoucherConcept

        /// <summary>
        /// CreateVoucherConcept
        /// </summary>
        /// <param name="voucherConcept"></param>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public static ACCEN.VoucherConcept CreateVoucherConcept(Models.AccountsPayables.VoucherConcept voucherConcept, int voucherId)
        {
            return new ACCEN.VoucherConcept(voucherConcept.Id)
            {
                VoucherConceptId = voucherConcept.Id,
                VoucherCode = voucherId,
                AccountingConceptCode = voucherConcept.AccountingConcept.Id,
                CostCenterCode = voucherConcept.CostCenter.CostCenterId,
                Amount = voucherConcept.Amount.Value,
            };
        }

        #endregion VoucherConcept

        #region VoucherConceptTax

        /// <summary>
        /// CreateVoucherConceptTax
        /// </summary>
        /// <param name="voucherConceptTax"></param>
        /// <param name="voucherConceptId"></param>
        /// <returns></returns>
        public static ACCEN.VoucherConceptTax CreateVoucherConceptTax(Models.AccountsPayables.VoucherConceptTax voucherConceptTax, int voucherConceptId)
        {
            int? taxId = null;
            int? taxCategoryId = null;
            int? taxConditionId = null;

            if (voucherConceptTax.Tax.Id > 0)
            {
                taxId = voucherConceptTax.Tax.Id;
            }

            if (voucherConceptTax.TaxCategory.Id > 0)
            {
                taxCategoryId = voucherConceptTax.TaxCategory.Id;
            }

            if (voucherConceptTax.TaxCondition.Id > 0)
            {
                taxConditionId = voucherConceptTax.TaxCondition.Id;
            }

            return new ACCEN.VoucherConceptTax(voucherConceptTax.Id)
            {
                VoucherConceptTaxId = voucherConceptTax.Id,
                VoucherConceptCode = voucherConceptId,
                TaxCode = taxId,
                TaxCategoryCode = taxCategoryId,
                TaxConditionCode = taxConditionId,
                TaxRate = voucherConceptTax.TaxeRate,
                TaxBaseAmount = voucherConceptTax.TaxeBaseAmount,
                TaxValue = voucherConceptTax.TaxValue
            };
        }

        #endregion VoucherConceptTax

        #region CreditCardType

        /// <summary>
        /// CreateCreditCardType
        /// </summary>
        /// <param name="creditCardType"></param>
        /// <returns></returns>
        public static COMMENT.CreditCardType CreateCreditCardType(CreditCardType creditCardType)
        {
            return new COMMENT.CreditCardType(creditCardType.Id)
            {
                Commission = creditCardType.Commission,
                CreditCardTypeCode = creditCardType.Id,
                Description = creditCardType.Description
            };
        }

        #endregion

        #region RetentionBase

        /// <summary>
        /// CreateRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>RetentionBase</returns>
        public static ACCEN.RetentionBase CreateRetentionBase(RetentionBase retentionBase)
        {
            return new ACCEN.RetentionBase(retentionBase.Id)
            {
                RetentionBaseCode = retentionBase.Id,
                Description = retentionBase.Description
            };

        }
        #endregion

        #region RetentionConcept

        /// <summary>
        /// CreateRetentionConcept
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>PerceivedRetention</returns>
        public static ACCEN.PerceivedRetention CreateRetentionConcept(RetentionConcept retentionConcept)
        {
            return new ACCEN.PerceivedRetention(retentionConcept.Id)
            {
                PerceivedRetentionId = retentionConcept.Id,
                Description = retentionConcept.Description,
                RetentionBaseCode = retentionConcept.RetentionBase.Id,
                IsActive = Convert.ToBoolean(retentionConcept.Status == 0 ? 0 : 1),//Convert.ToBoolean(retentionConcept.Status),
                MaximumDifferenceTax = retentionConcept.DifferenceAmount
            };
        }

        /// <summary>
        /// CreatePercentageRetentionConcept
        /// </summary>
        /// <param name="retentionConceptPercentaje"></param>
        /// <param name="perceivedRetentionCode"></param>
        /// <returns>RangeItem</returns>
        public static ACCEN.PerceivedRetentionValidity CreatePercentageRetentionConcept(RetentionConceptPercentage retentionConceptPercentage)
        {
            return new ACCEN.PerceivedRetentionValidity(retentionConceptPercentage.Id)
            {
                PerceivedRetentionValidityId = retentionConceptPercentage.Id,
                PerceivedRetentionCode = retentionConceptPercentage.RetentionConcept.Id,
                RetentionPercentage = retentionConceptPercentage.Percentage,
                ValidityFrom = retentionConceptPercentage.DateFrom,
                ValidityTo = retentionConceptPercentage.DateTo,
                PerceivedRetentionCode2 = retentionConceptPercentage.ExternalCode
            };
        }

        /// <summary>
        /// CreatePayment
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectCode"></param>
        /// <returns>Payment</returns>
        public static ACCEN.RetentionHistory CreateRetentionHistory(RetentionReceipt retentionReceipt, int collectCode)
        {
            return new ACCEN.RetentionHistory(retentionReceipt.Id)
            {
                Amount = retentionReceipt.LocalAmount.Value,
                AuthorizationNumber = retentionReceipt.AuthorizationNumber,
                BranchCode = retentionReceipt.Policy.Branch.Id,
                CurrencyCode = retentionReceipt.Amount.Currency.Id,
                EndorsementNumber = retentionReceipt.Policy.Endorsement.Number,
                ExcahngeRate = retentionReceipt.ExchangeRate.SellAmount == 0 ? retentionReceipt.ExchangeRate.BuyAmount : retentionReceipt.ExchangeRate.SellAmount,
                ExpirationDate = retentionReceipt.ExpirationDate,
                IncomeAmount = retentionReceipt.Amount.Value,
                IndividualId = retentionReceipt.Policy.Holder.IndividualId,
                InvoiceDate = retentionReceipt.InvoiceDate,
                InvoiceNumber = retentionReceipt.BillNumber,
                IssueDate = retentionReceipt.IssueDate,
                PerceivedRetentionCode = retentionReceipt.RetentionConcept.Id,
                PersonTypeCode = retentionReceipt.Policy.Holder.InsuredId,
                PolicyId = retentionReceipt.Policy.Id,
                PolicyNumber = retentionReceipt.Policy.DocumentNumber,
                PrefixCode = retentionReceipt.Policy.Prefix.Id,
                ReceiptNumber = collectCode,
                RegisterDate = DateTime.Now,
                RetentionHistoryId = retentionReceipt.Id,
                UserCode = retentionReceipt.Policy.UserId,
                VoucherNumber = retentionReceipt.VoucherNumber
            };
        }

        #endregion

        #region AuthorizationPaymentOrder

        /// <summary>
        /// CreateAuthorizationPaymentOrder
        /// </summary>
        /// <param name="authorizationPaymentOrder"></param>
        /// <returns></returns>
        public static ACCEN.PaymentOrderAuthorization CreateAuthorizationPaymentOrder(PaymentOrder authorizationPaymentOrder,
                                                                                            int authorizationLevel)
        {

            return new ACCEN.PaymentOrderAuthorization(authorizationPaymentOrder.Id, Convert.ToInt32(authorizationPaymentOrder.Status))
            {
                PaymentOrderCode = authorizationPaymentOrder.Id,
                AuthorizationLevel = authorizationLevel,
                AuthorizationDate = DateTime.Today,
                AuthorizerUserId = authorizationPaymentOrder.UserId
            };
        }

        #endregion

        #region PolicyComponentDistribution
        public static ACCEN.ApplicationPremiumComponent CreateApplicationPremiumComponent(ApplicationPremiumComponent applicationPremiumComponent)
        {
            return new ACCEN.ApplicationPremiumComponent(0)
            {
                AppPremiumCode = applicationPremiumComponent.PremiumId,
                ComponentCode = applicationPremiumComponent.ComponentId,
                CurrencyCode = applicationPremiumComponent.CurrencyId,
                ExchangeRate = applicationPremiumComponent.ExchangeRate,
                Amount = applicationPremiumComponent.Amount,
                LocalAmount = applicationPremiumComponent.LocalAmount,
                MainAmount = applicationPremiumComponent.MainAmount,
                MainLocalAmount = applicationPremiumComponent.MainLocalAmount
            };
        }
        public static ACCEN.ApplicationPremiumComponentLbsb CreateApplicationPremiumComponentLbsb(ApplicationPremiumComponentLBSB applicationPremiumComponentLBSB)
        {
            return new ACCEN.ApplicationPremiumComponentLbsb(0)
            {
                AppComponentCode = applicationPremiumComponentLBSB.ApplicationComponentId,
                LineBusinessCode = applicationPremiumComponentLBSB.LineBussinesId,
                SubLineBusinessCode = applicationPremiumComponentLBSB.SubLineBussinesId,
                CurrencyCode = applicationPremiumComponentLBSB.CurrencyId,
                ExchangeRate = applicationPremiumComponentLBSB.ExchangeRateId,
                Amount = applicationPremiumComponentLBSB.Amount,
                LocalAmount = applicationPremiumComponentLBSB.LocalAmount,
                MainAmount = applicationPremiumComponentLBSB.MainAmount,
                MainLocalAmount = applicationPremiumComponentLBSB.MainLocalAmount,
            };
        }
        #endregion

        #region TempApplicationPremiumCommission
        public static ACCEN.TempApplicationPremiumCommiss CreateTempApplicationPremiumCommission(TempApplicationPremiumCommiss tempApplicationPremiumCommiss)
        {
            return new ACCEN.TempApplicationPremiumCommiss(0)
            {
                AgentAgencyId = tempApplicationPremiumCommiss.AgentAgencyId,
                AgentId = tempApplicationPremiumCommiss.AgentId,
                AgentTypeCode = tempApplicationPremiumCommiss.AgentTypeId,
                Amount = tempApplicationPremiumCommiss.Amount,
                CommissionType = tempApplicationPremiumCommiss.CommissionType,
                CurrencyCode = tempApplicationPremiumCommiss.Currency,
                ExchangeRate = tempApplicationPremiumCommiss.ExchangeRate,
                LocalAmount = tempApplicationPremiumCommiss.LocalAmount,
                TempAppPremiumId = tempApplicationPremiumCommiss.TempApplicationPremiumId,
            };
        }
        public static ACCEN.TempApplicationPremiumCommiss CreateTempApplicationPremiumCommission(ApplicationPremiumCommision tempApplicationPremiumCommiss)
        {
            return new ACCEN.TempApplicationPremiumCommiss(0)
            {
                AgentAgencyId = tempApplicationPremiumCommiss.AgentAgencyId,
                AgentId = tempApplicationPremiumCommiss.AgentIndividualId,
                AgentTypeCode = tempApplicationPremiumCommiss.AgentTypeId,
                Amount = tempApplicationPremiumCommiss.Amount,
                CommissionType = tempApplicationPremiumCommiss.CommissionType,
                CurrencyCode = tempApplicationPremiumCommiss.CurrencyId,
                ExchangeRate = tempApplicationPremiumCommiss.ExchangeRate,
                LocalAmount = tempApplicationPremiumCommiss.LocalAmount,
                TempAppPremiumId = tempApplicationPremiumCommiss.ApplicationPremiumId,
            };
        }
        public static List<ACCEN.TempApplicationPremiumCommiss> CreateTempApplicationPremiumCommission(List<TempApplicationPremiumCommiss> tempApplicationPremiumCommiss)
        {
            List<ACCEN.TempApplicationPremiumCommiss> entityTempApplicationPremiumCommiss = new List<ACCEN.TempApplicationPremiumCommiss>();
            foreach (TempApplicationPremiumCommiss tempApplicationPremiumCommis in tempApplicationPremiumCommiss)
            {
                entityTempApplicationPremiumCommiss.Add(CreateTempApplicationPremiumCommission(tempApplicationPremiumCommis));
            }
            return entityTempApplicationPremiumCommiss;
        }

        #endregion

        #region ApplicationPremiumCommission
        public static ACCEN.ApplicationPremiumCommiss CreateApplicationPremiumCommission(ApplicationPremiumCommision applicationPremiumCommision)
        {
            return new ACCEN.ApplicationPremiumCommiss(0)
            {
                AgentAgencyId = applicationPremiumCommision.AgentAgencyId,
                AgentId = applicationPremiumCommision.AgentIndividualId,
                AgentTypeCode = applicationPremiumCommision.AgentTypeId,
                Amount = applicationPremiumCommision.Amount,
                AppCommissId = applicationPremiumCommision.Id,
                AppComponentId = applicationPremiumCommision.AppComponentId,
                CommissionType = applicationPremiumCommision.CommissionType,
                CurrencyCode = applicationPremiumCommision.CurrencyId,
                ExchangeRate = applicationPremiumCommision.ExchangeRate,
                LocalAmount = applicationPremiumCommision.LocalAmount,
            };
        }
        public static List<ACCEN.ApplicationPremiumCommiss> CreateApplicationPremiumCommission(List<ApplicationPremiumCommision> applicationPremiumCommisions)
        {
            List<ACCEN.ApplicationPremiumCommiss> applicationPremiumCommisses = new List<ACCEN.ApplicationPremiumCommiss>();
            foreach (ApplicationPremiumCommision applicationPremiumCommision in applicationPremiumCommisions)
            {
                applicationPremiumCommisses.Add(CreateApplicationPremiumCommission(applicationPremiumCommision));
            }
            return applicationPremiumCommisses;
        }
        #endregion

        #region CollectApplicationControl
        public static INTEN.CollectApplicationControl CreateCollectApplicationControl(Models.Integration2G.CollectApplicationControl collectApplicationControl)
        {
            return new INTEN.CollectApplicationControl(0)
            {
                CollectApplicationId = collectApplicationControl.CollectApplicationId,
                AppSource = collectApplicationControl.AppSource,
                Action = collectApplicationControl.Action,
                Origin = collectApplicationControl.Origin
            };
        }
        #endregion
    }
}