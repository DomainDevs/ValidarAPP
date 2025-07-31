//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Entities;
using Sistran.Core.Application.ReportingServices.Models.Formats;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using debit = Sistran.Core.Application.AutomaticDebitServices.Models;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers
{
    static internal class ModelAssembler
    {
        #region AutomaticDebitStatus

        /// <summary>
        /// CreateAutomaticDebitStatus
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Array></returns>
        public static List<Array> CreateAutomaticDebitStatus(BusinessCollection businessCollection)
        {
            List<Array> automaticDebitStatus = new List<Array>();
            foreach (AutomaticDebitStatus automaticDebitStatusEntity in businessCollection.OfType<AutomaticDebitStatus>())
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
        public static Array CreateShipmentStatus(AutomaticDebitStatus automaticDebitStatusEntity)
        {
            String[] shipmentStatus = new String[2];
            shipmentStatus[0] = automaticDebitStatusEntity.AutomaticDebitStatusId.ToString();
            shipmentStatus[1] = automaticDebitStatusEntity.Description.ToString();

            return shipmentStatus;
        }

        #endregion

        #region BankNetwork

        /// <summary>
        /// CreateNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public static debit.BankNetwork CreateNetwork(BankNetwork bankNetwork)
        {
            return new debit.BankNetwork()
            {
                Id = bankNetwork.BankNetworkId,
                HasTax = bankNetwork.Tax != null ? Convert.ToBoolean(bankNetwork.Tax) : false,
                Description = bankNetwork.Description != null ? Convert.ToString(bankNetwork.Description) : "",
                RequiresNotification = bankNetwork.Prenotification != null ? Convert.ToBoolean(bankNetwork.Prenotification) : false,
            };
        }

        #endregion

        #region BankNetworkFormat

        /// <summary>
        /// CreateBankNetworkFormat
        /// </summary>
        /// <param name="bankNetworkFormat"></param>
        /// <returns></returns>
        public static debit.AutomaticDebitFormat CreateBankNetworkFormat(BankNetworkFormat bankNetworkFormat)
        {
            debit.FormatUsingTypes formatUsingTypes = debit.FormatUsingTypes.Reception;

            if (bankNetworkFormat.FormatUsingType == Convert.ToInt32(debit.FormatUsingTypes.Reception)) 
            {
                formatUsingTypes = debit.FormatUsingTypes.Reception;
            }
            if (bankNetworkFormat.FormatUsingType == Convert.ToInt32(debit.FormatUsingTypes.ReceptionNotification))
            {
                formatUsingTypes = debit.FormatUsingTypes.ReceptionNotification;
            }
            if (bankNetworkFormat.FormatUsingType == Convert.ToInt32(debit.FormatUsingTypes.Sending))
            {
                formatUsingTypes = debit.FormatUsingTypes.Sending;
            }
            if (bankNetworkFormat.FormatUsingType == Convert.ToInt32(debit.FormatUsingTypes.SendingNotification))
            {
                formatUsingTypes = debit.FormatUsingTypes.SendingNotification;
            }

            return new debit.AutomaticDebitFormat()
            {
                BankNetwork = new debit.BankNetwork() { Id = bankNetworkFormat.BankNetworkCode },
                Format = new Format()
                {
                    Id = bankNetworkFormat.FormatCode,
                    FileType = FileTypes.Excel,
                },
                FormatUsingType = formatUsingTypes,
                Id = bankNetworkFormat.BankNetworkFormatId
            };
        }

        /// <summary>
        /// CreateBankNetworkFormats
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<debit.AutomaticDebitFormat> CreateBankNetworkFormats(BusinessCollection businessCollection)
        {
            List<debit.AutomaticDebitFormat> automaticDebitFormats = new List<debit.AutomaticDebitFormat>();
            foreach (BankNetworkFormat bankNetworkFormatEntity in businessCollection.OfType<Entities.BankNetworkFormat>())
            {
                automaticDebitFormats.Add(CreateBankNetworkFormat(bankNetworkFormatEntity));
            }

            return automaticDebitFormats;
        }

        #endregion

        #region BankNetworkStatus

        /// <summary>
        /// CreateBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public static debit.BankNetworkStatus CreateBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            var acceptedCouponStatus = new List<debit.CouponStatus>();
            acceptedCouponStatus.Add(new debit.CouponStatus()
            {
                SmallDescription = bankNetworkStatus.AppliedDefault,
                Id = bankNetworkStatus.CouponStatusCode,
                CouponStatusType = debit.CouponStatusTypes.Applied
            });

            var rejectedCouponStatus = new List<debit.CouponStatus>();
            rejectedCouponStatus.Add(new debit.CouponStatus()
            {
                SmallDescription = bankNetworkStatus.RejectionDefault,
                Id = bankNetworkStatus.CouponStatusCode,
                CouponStatusType = debit.CouponStatusTypes.Rejected
            });

            return new debit.BankNetworkStatus()
            {
                Id = Convert.ToInt32(bankNetworkStatus.BankNetworkCode),
                AcceptedCouponStatus = acceptedCouponStatus,
                RejectedCouponStatus = rejectedCouponStatus,
                BankNetwork = new debit.BankNetwork() { Id = bankNetworkStatus.BankNetworkCode }
            };
        }

        /// <summary>
        /// CreateRejectionBanks
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<BankNetworkStatus></returns>
        public static List<debit.BankNetworkStatus> CreateRejectionBanks(BusinessCollection businessCollection)
        {
            List<debit.BankNetworkStatus> bankNetworkStatus = new List<debit.BankNetworkStatus>();
            foreach (BankNetworkStatus bankNetworkStatusEntity in businessCollection.OfType<BankNetworkStatus>())
            {
                bankNetworkStatus.Add(CreateBankNetworkStatus(bankNetworkStatusEntity));
            }
            return bankNetworkStatus;
        }

        #endregion

        #region CouponStatus

        /// <summary>
        /// CreateCouponStatus
        /// </summary>
        /// <param name="couponStatus"></param>
        /// <returns>CouponStatus</returns>
        public static debit.CouponStatus CreateCouponStatus(CouponStatus couponStatus)
        {
            debit.CouponStatusTypes couponStatusTypes = Convert.ToBoolean(couponStatus.Applied) ? debit.CouponStatusTypes.Applied : debit.CouponStatusTypes.Rejected;

            return new debit.CouponStatus()
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
        public static List<debit.CouponStatus> CreateCouponStatus(BusinessCollection businessCollection)
        {
            List<debit.CouponStatus> couponStatus = new List<debit.CouponStatus>();
            foreach (CouponStatus couponStatusEntity in businessCollection.OfType<CouponStatus>())
            {
                couponStatus.Add(CreateCouponStatus(couponStatusEntity));
            }
            return couponStatus;
        }


        #endregion

        #region LogBankResponse

        /// <summary>
        /// CreateLogBankResponses
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Array></returns>
        public static List<Array> CreateLogBankResponses(BusinessCollection businessCollection)
        {
            List<Array> shipmentStatus = new List<Array>();
            foreach (AutomaticDebitStatus automaticDebitStatusEntity in businessCollection.OfType<AutomaticDebitStatus>())
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
        public static Array CreateLogBankResponse(LogBankResponse logBankResponseEntity)
        {
            String[] logBankResponse = new String[15];
            logBankResponse[0] = logBankResponseEntity.LogBankResponseId.ToString();
            logBankResponse[1] = logBankResponseEntity.BankNetworkId.ToString();
            logBankResponse[2] = logBankResponseEntity.LotNumber.ToString();
            logBankResponse[3] = logBankResponseEntity.LineNumber.ToString();
            logBankResponse[4] = logBankResponseEntity.CardAccountNumber.ToString();
            logBankResponse[5] = logBankResponseEntity.VoucherNumber.ToString();
            logBankResponse[6] = logBankResponseEntity.RejectionCode.ToString();
            logBankResponse[7] = logBankResponseEntity.ApplicationDate.ToString();
            logBankResponse[8] = logBankResponseEntity.PremiumAmount.ToString();
            logBankResponse[9] = logBankResponseEntity.RegisterDate.ToString();
            logBankResponse[10] = logBankResponseEntity.AuthorizationNumber.ToString();
            logBankResponse[11] = logBankResponseEntity.DocumentNumber.ToString();
            logBankResponse[12] = logBankResponseEntity.IsPrenotificacion.ToString();
            logBankResponse[13] = logBankResponseEntity.DescriptionError.ToString();
            logBankResponse[14] = logBankResponseEntity.UserCode.ToString();

            return logBankResponse;
        }

        #endregion

        #region PaymentMethodAccountType

        /// <summary>
        /// CreatePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public static debit.PaymentMethodAccountType CreatePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            return new debit.PaymentMethodAccountType()
            {
                Id = 0,
                BankAccountType = new BankAccountTypeDTO()
                {
                    Description = ""
                }
            };
        }

        #endregion

        #region PaymentMethodBankNetwork

        /// <summary>
        /// CreatePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns>PaymentMethodBankNetwork</returns>
        public static debit.PaymentMethodBankNetwork CreatePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            return new debit.PaymentMethodBankNetwork()
            {
                BankNetwork = new debit.BankNetwork()
                {
                    Id = paymentMethodBankNetwork.BankNetworkCode,
                    Description = paymentMethodBankNetwork.Identifier
                },
                PaymentMethod = new PaymentMethodDTO()
                {
                    Id = paymentMethodBankNetwork.PaymentMethodCode
                },
                BankAccountCompany = new BankAccountCompanyDTO()
                {
                    Id = paymentMethodBankNetwork.AccountBankCode
                },

                ToGenerate = (bool)paymentMethodBankNetwork.Generate,
            };
        }

        #endregion

    }
}
