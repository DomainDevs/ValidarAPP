//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Entities;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using System;
using debit = Sistran.Core.Application.AutomaticDebitServices.Models;


namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers
{
    internal static class EntityAssembler
    {
        #region BankNetwork

        /// <summary>
        /// CreateNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public static Entities.BankNetwork CreateNetwork(debit.BankNetwork bankNetwork)
        {
            return new Entities.BankNetwork(bankNetwork.Id)
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
                Prenotification = bankNetwork.RequiresNotification,
                ReferencePayment = false,
                GlobalReceipt = false,
                SeparatorLine = false
            };
        }

        #endregion

        #region BankNetworkFormat

        /// <summary>
        /// CreateBankNetworkFormat
        /// </summary>
        /// <param name="automaticDebitFormat"></param>
        /// <returns>BankNetwork</returns>
        public static BankNetworkFormat CreateBankNetworkFormat(AutomaticDebitFormat automaticDebitFormat)
        {
            return new BankNetworkFormat(automaticDebitFormat.Id)
            {
                BankNetworkCode = automaticDebitFormat.BankNetwork.Id,
                BankNetworkFormatId = automaticDebitFormat.Id,
                FormatCode = automaticDebitFormat.Format.Id,
                FormatUsingType = Convert.ToInt32(automaticDebitFormat.FormatUsingType)
            };
        }

        #endregion

        #region BankNetworkStatus

        /// <summary>
        /// CreateNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public static Entities.BankNetworkStatus CreateBankNetworkStatus(debit.BankNetworkStatus bankNetworkStatus)
        {
            return new Entities.BankNetworkStatus(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id)
            {
                AppliedDefault = bankNetworkStatus.AcceptedCouponStatus[0].SmallDescription,
                RejectionDefault = bankNetworkStatus.RejectedCouponStatus[0].SmallDescription,
            };
        }

        #endregion

        #region CouponStatus

        /// <summary>
        /// CreateCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        public static Entities.CouponStatus CreateCouponStatus(debit.CouponStatus couponStatus)
        {
            return new Entities.CouponStatus(couponStatus.Id, couponStatus.SmallDescription)
            {
                Applied = couponStatus.CouponStatusType == CouponStatusTypes.Applied,
                Description = couponStatus.Description,
                Enabled = couponStatus.IsEnabled,
                NumberOfRetries = couponStatus.RetriesNumber,
                Rejection = couponStatus.CouponStatusType == CouponStatusTypes.Rejected,
                Retry = Convert.ToBoolean(couponStatus.RetriesNumber == 0 ? 0 : 1),
            };
        }

        #endregion

        #region LogBankResponse

        /// <summary>
        /// CreateLogBankResponse
        /// </summary>
        /// <param name="logBankResponse"></param>
        /// <returns>LogBankResponse</returns>
        public static LogBankResponse CreateLogBankResponse(Array logBankResponse)
        {
            return new LogBankResponse(0)
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

        #region PaymentMethodAccountType

        /// <summary>
        /// CreatePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public static Entities.PaymentMethodAccountType CreatePaymentMethodAccountType(debit.PaymentMethodAccountType paymentMethodAccountType)
        {
            return new Entities.PaymentMethodAccountType(paymentMethodAccountType.PaymentMethod.Id,
                                                paymentMethodAccountType.BankAccountType.Id)
            {
                AccountTypeCode = paymentMethodAccountType.BankAccountType.Id,
                PaymentMethodCode = paymentMethodAccountType.PaymentMethod.Id,
                DebitCode = paymentMethodAccountType.SmallDescriptionDebit
            };
        }

        #endregion

        #region PaymentMethodBankNetwork

        /// <summary>
        /// CreateNetworkPaymentMethod
        /// Devuelve una relación del Conducto de Pago por Red y Banco
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public static Entities.PaymentMethodBankNetwork CreatePaymentMethodBankNetwork(debit.PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return new Entities.PaymentMethodBankNetwork(paymentMethodBankNetwork.BankNetwork.Id, paymentMethodBankNetwork.PaymentMethod.Id,
                paymentMethodBankNetwork.BankAccountCompany.Id)
            {
                Generate = paymentMethodBankNetwork.ToGenerate,
                Identifier = ""
            };
        }

        #endregion

    }
}
