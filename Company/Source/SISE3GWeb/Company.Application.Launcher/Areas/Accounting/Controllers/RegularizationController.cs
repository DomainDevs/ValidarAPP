using System;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Generic;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using DTOs = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class RegularizationController : Controller
    {
        #region Instance Variables

        readonly CommonController _commonController = new CommonController();
        readonly BillingController _billingController = new BillingController();

        #endregion

        #region Actions

        /// <summary>
        /// ReplaceWithAsterisks
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns>string</returns>
        public string ReplaceWithAsterisks(string cellValue)
        {
            int row = 0;
            string valueWithAsterisk = "";

            string firstNumber = cellValue.Substring(0, 1);
            string lastNumber = cellValue.Substring(cellValue.Length - 1);

            for (row = 0; row < cellValue.Length - 2; row++)
            {
                valueWithAsterisk += "*";
            }

            valueWithAsterisk = firstNumber + valueWithAsterisk + lastNumber;
            cellValue = valueWithAsterisk;

            return cellValue;
        }

        #endregion

        #region CheckRegularization

        /// <summary>
        /// LoadCheckRegularization
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="documentNumber"></param>
        /// <param name="branchId"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadCheckRegularization(int paymentId, string documentNumber, int branchId)
        {
            TempData["PaymentId"] = paymentId;
            TempData["DocumentNumber"] = documentNumber;
            TempData["BranchId"] = branchId;
            
            return RedirectToAction("MainCheckRegularization");
        }

        /// <summary>
        /// GetEnabledPaymentMethodTypess
        /// Obtiene los métodos de pago habiltados para caja
        /// </summary>
        /// <returns>UifSelectResult</returns>
        public UifSelectResult GetEnabledPaymentMethodTypess()
        {
            List<DTOs.Payments.PaymentMethodDTO> paymentMethods = new List<DTOs.Payments.PaymentMethodDTO>();

            List<DTOs.Search.PaymentMethodTypeDTO> paymentMethodTypeDTOs = DelegateService.accountingParameterService.GetEnablePaymentMethodType(false, false, true);

            foreach (DTOs.Search.PaymentMethodTypeDTO paymentMethodTypeDto in paymentMethodTypeDTOs)
            {
                DTOs.Payments.PaymentMethodDTO paymentMethod = new DTOs.Payments.PaymentMethodDTO()
                {
                    Description = Convert.ToString(paymentMethodTypeDto.Description),
                    Id = Convert.ToInt16(paymentMethodTypeDto.PaymentTypeCode)
                };

                paymentMethods.Add(paymentMethod);
            }
            return new UifSelectResult(paymentMethods);
        }
        
        /// <summary>
        /// MainCheckRegularization
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCheckRegularization()
        {
            try
            {
                int defaultValue = Convert.ToInt16(@Global.DefaultValue);
                string defaultDescription = (@Global.DefaultDescription);

                List<DTOs.CollectConceptDTO> collectConcepts = DelegateService.accountingParameterService.GetCollectConcepts();
                collectConcepts.Insert(0, new DTOs.CollectConceptDTO { Id = defaultValue, Description = defaultDescription });
                ViewBag.IncomeConcept = collectConcepts;

                List<DTOs.Payments.PaymentMethodDTO> paymentMethods = new List<DTOs.Payments.PaymentMethodDTO>();
                ViewBag.PaymentMethod = paymentMethods;

                var currencies = DelegateService.commonService.GetCurrencies();
                ViewBag.Currency = currencies;

                List<object> months = new List<object>();
                for (int i = 1; i <= 12; i++)
                {
                    months.Add(new { Id = i, Description = i });
                }
                ViewBag.ValidationMonth = months;

                int localCurrencyId = Convert.ToInt32(ConfigurationManager.AppSettings["LocalCurrencyId"]);
                ViewBag.localCurrencyId = localCurrencyId;

                var userNicks = _commonController.GetUserByName(User.Identity.Name);
                ViewBag.UserNick = userNicks[0].AccountName;

                ViewBag.DefaultValue = defaultValue;
                ViewBag.DefaultDescription = defaultDescription;

                // Payment_Methods
                ViewBag.ParamPaymentMethodPostdatedCheck = ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"];
                ViewBag.ParamPaymentMethodCurrentCheck = ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"];
                ViewBag.ParamPaymentMethodCash = ConfigurationManager.AppSettings["ParamPaymentMethodCash"];
                ViewBag.ParamPaymentMethodCreditableCreditCard = ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"];
                ViewBag.ParamPaymentMethodUncreditableCreditCard = ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"];
                ViewBag.ParamPaymentMethodDebit = ConfigurationManager.AppSettings["ParamPaymentMethodDebit"];
                ViewBag.ParamPaymentMethodDirectConection = ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"];
                ViewBag.ParamPaymentMethodTransfer = ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"];
                ViewBag.ParamPaymentMethodDepositVoucher = ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"];
                ViewBag.ParamPaymentMethodRetentionReceipt = ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"];
                ViewBag.ParamPaymentMethodDataphone = ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"];
                ViewBag.ParamPaymentMethodElectronicPayment = ConfigurationManager.AppSettings["ParamPaymentMethodElectronicPayment"];
                ViewBag.ParamPaymentMethodPaymentArea = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentArea"];
                ViewBag.ParamPaymentMethodPaymentCard = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentCard"];

                // Recupera fecha contable
                DateTime dateAccounting = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(dateAccounting, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(dateAccounting.Date, 2);
                ViewBag.CurrentDate = DateTime.Now.Date;

                ViewBag.idBillControl = TempData["idBillControl"];
                ViewBag.idBranch = TempData["idBranch"];

                ViewBag.PaymentId = TempData["PaymentId"] ?? 0;
                ViewBag.DocumentNumber = TempData["DocumentNumber"] ?? 0;
                ViewBag.BranchIdOrigin = TempData["BranchId"] ?? 0;

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetRejectedCheckInfoByPaymentId 
        /// Obtiene los datos para mostrar en regularizacion de cheques
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRejectedCheckInfoByPaymentId(int paymentId)
        {
            return Json( DelegateService.accountingPaymentService.GetRejectedCheckInfoByPaymentId(paymentId), JsonRequestBehavior.AllowGet);
        }
        
        #region BillRequest

        /// <summary>
        /// SaveBillRequest
        /// Inserta un registro en la tabla BILL.BILL, BILL.PAYMENT
        /// </summary>
        /// <param name="frmBill"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBillRequest(BillModel frmBill, int branchId)
        {
            int billControlId = frmBill.BillControlId;
            int sourcePaymentId = frmBill.SourcePaymentId;
            // Obtiene parámetro de la BDD
            int number = _commonController.GetBillNumber();

            //calcular fecha contable
            int status = Convert.ToInt32(CollectControlStatus.Open);
            DateTime accountingDate;

            // Se actualiza parametro del número de carátula.
            _commonController.UpdateBillNumber(number);

            DTOs.CollectDTO collect = new DTOs.CollectDTO();

            DTOs.CollectConceptDTO collectConcept = new DTOs.CollectConceptDTO() { Id = frmBill.BillingConceptId };

            DTOs.AmountDTO paymentsTotal = new DTOs.AmountDTO() { Value = Convert.ToDecimal(frmBill.PaymentsTotal) };

            DTOs.PersonDTO payer = new DTOs.PersonDTO()
            {
                IdentificationDocument = new DTOs.IdentificationDocumentDTO()
                {
                    DocumentType = new DTOs.DocumentTypeDTO() { Id = frmBill.PayerDocumentTypeId }
                },
                IndividualId = frmBill.PayerId,
                PersonType = new DTOs.PersonTypeDTO() { Id = frmBill.PayerTypeId }
            };

            int statusId = Convert.ToInt16(CollectStatus.Active);
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            collect.Description = frmBill.Description;
            collect.Date = DateTime.Now;
            collect.Concept = collectConcept;
            collect.PaymentsTotal = new DTOs.AmountDTO
            {
                Value = frmBill.PaymentsTotal
            };                
            collect.Payer = payer;
            collect.Status = statusId;
            collect.Number = number;
            collect.CollectType = Convert.ToInt32(CollectTypes.Incoming);
            collect.UserId = userId;
            collect.CompanyIndividualId = -1; //accountingCompany;
            collect.AccountingCompany = new DTOs.CompanyDTO() {
                IndividualId = -1 
                };

            collect.Payments = new List<DTOs.Payments.PaymentDTO>();

            if (frmBill.PaymentSummary != null)
            {
                for (int j = 0; j < frmBill.PaymentSummary.Count; j++)
                {
                   DTOs.Payments.PaymentMethodDTO paymentMethod = new DTOs.Payments.PaymentMethodDTO() { Id = frmBill.PaymentSummary[j].PaymentMethodId };

                    #region PaymentMethodType

                    #region Cash

                    // Efectivo
                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]))
                    {
                       DTOs.AmountDTO amount = new DTOs.AmountDTO()
                        {
                            Currency = new DTOs.Search.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                            Value = frmBill.PaymentSummary[j].Amount
                        };

                        DTOs.ExchangeRateDTO exchangeRate = new DTOs.ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };

                        DTOs.AmountDTO localAmount = new DTOs.AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                        collect.Payments.Add(new DTOs.Payments.CashDTO()
                        {
                            PaymentMethod = paymentMethod,
                            Amount = amount,
                            ExchangeRate = exchangeRate,
                            Id = frmBill.PaymentSummary[j].PaymentId,
                            LocalAmount = localAmount,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }
                    #endregion

                    #region Check
                    // Cheque y débito
                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"]))
                    {
                        DTOs.AmountDTO amount = new DTOs.AmountDTO()
                        {
                            Currency = new DTOs.Search.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                            Value = frmBill.PaymentSummary[j].Amount
                        };

                        DTOs.BankDTO issuingBank = new DTOs.BankDTO() { Id = frmBill.PaymentSummary[j].CheckPayments[0].IssuingBankId };
                        DTOs.ExchangeRateDTO exchangeRate = new DTOs.ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                        DTOs.AmountDTO localAmount = new DTOs.AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                        collect.Payments.Add(new DTOs.Payments.CheckDTO()
                        {
                            PaymentMethod = paymentMethod,
                            Amount = amount,
                            Date = frmBill.PaymentSummary[j].CheckPayments[0].Date,
                            DocumentNumber = frmBill.PaymentSummary[j].CheckPayments[0].DocumentNumber,
                            ExchangeRate = exchangeRate,
                            Id = frmBill.PaymentSummary[j].PaymentId,
                            IssuerName = frmBill.PaymentSummary[j].CheckPayments[0].IssuerName,
                            IssuingAccountNumber = frmBill.PaymentSummary[j].CheckPayments[0].IssuingAccountNumber,
                            IssuingBank = issuingBank,
                            LocalAmount = localAmount,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion

                    #region CreditCard

                    // Tarjeta de crédito
                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"]))
                    {
                        decimal taxBase = frmBill.PaymentSummary[j].CreditPayments[0].TaxBase;
                        DTOs.AmountDTO amount = new DTOs.AmountDTO()
                        {
                            Currency = new DTOs.Search.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                            Value = frmBill.PaymentSummary[j].Amount
                        };

                        DTOs.BankDTO  issuingBank = new DTOs.BankDTO() { Id = frmBill.PaymentSummary[j].CreditPayments[0].IssuingBankId };
                        DTOs.Payments.CreditCardTypeDTO creditCardType = new DTOs.Payments.CreditCardTypeDTO() { Id = frmBill.PaymentSummary[j].CreditPayments[0].CreditCardTypeId };

                        DTOs.Payments.CreditCardValidThruDTO creditCardValidThru = new DTOs.Payments.CreditCardValidThruDTO()
                        {
                            Month = frmBill.PaymentSummary[j].CreditPayments[0].ValidThruMonth,
                            Year = frmBill.PaymentSummary[j].CreditPayments[0].ValidThruYear
                        };

                        List <DTOs.Payments.PaymentTaxDTO> paymentTaxs = DelegateService.accountingPaymentService.GetTaxCreditCard(creditCardType.Id, branchId).Taxes;

                        decimal ivaCardAmount = 0;
                        decimal tax = 0;
                        decimal retention = 0;

                        for (int i = 0; i < paymentTaxs.Count; i++)
                        {
                            if (paymentTaxs[i].Tax.Id == Convert.ToInt32(@Global.TaxCardIvaId))
                            {
                                ivaCardAmount = taxBase * paymentTaxs[i].Rate / 100;
                            }
                        }

                        creditCardType.Commission = DelegateService.accountingParameterService.GetCreditCardType(creditCardType.Id).Commission * (frmBill.PaymentSummary[j].LocalAmount - ivaCardAmount) / 100;

                        // Se asignan las base del impuesto
                        for (int f = 0; f < paymentTaxs.Count; f++)
                        {
                            paymentTaxs[f].TaxBase = new DTOs.AmountDTO();
                            if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxCardIvaId"]))
                            {
                                paymentTaxs[f].TaxBase.Value = taxBase;
                            }
                                
                            if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardIcaId"]))
                            {
                                paymentTaxs[f].TaxBase.Value = (frmBill.PaymentSummary[j].Amount - ivaCardAmount);
                            }
                                
                            if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardIvaId"]))
                            {
                                paymentTaxs[f].TaxBase.Value = ivaCardAmount;
                            }
                                
                            if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardSourceId"]))
                            {
                                paymentTaxs[f].TaxBase.Value = (frmBill.PaymentSummary[j].Amount - ivaCardAmount);
                            }
                        }

                        // Calcula el valor del impuesto total
                        for (int f = 0; f < paymentTaxs.Count; f++)
                        {
                            /*TODO LFREIRE No existe campo en modelo en TaxServices
                            if (!paymentTaxs[f].Tax.IsRetention)
                            {
                                tax = tax + (paymentTaxs[f].TaxBase.Value * paymentTaxs[f].Rate / 100);
                            }
                            else
                            {
                                retention = retention + (paymentTaxs[f].TaxBase.Value * paymentTaxs[f].Rate / 100);
                            }
                            */
                        }

                        DTOs.ExchangeRateDTO exchangeRate = new DTOs.ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                        DTOs.AmountDTO localAmount = new DTOs.AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                        collect.Payments.Add(new DTOs.Payments.CreditCardDTO()
                        {
                            Amount = amount,
                            AuthorizationNumber = frmBill.PaymentSummary[j].CreditPayments[0].AuthorizationNumber,
                            CardNumber = frmBill.PaymentSummary[j].CreditPayments[0].CardNumber,
                            Holder = frmBill.PaymentSummary[j].CreditPayments[0].Holder,
                            Id = frmBill.PaymentSummary[j].PaymentId,
                            IssuingBank = issuingBank,
                            ExchangeRate = exchangeRate,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            Type = creditCardType,
                            ValidThru = creditCardValidThru,
                            Voucher = frmBill.PaymentSummary[j].CreditPayments[0].Voucher,
                            Status = Convert.ToInt16(PaymentStatus.Active),
                            Taxes = paymentTaxs,
                            Tax = Convert.ToDecimal(tax),
                            Retention = Convert.ToDecimal(retention)
                        });
                    }

                    #endregion

                    #region Transfer

                    // Transferencia
                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"]))
                    {
                        DTOs.AmountDTO amount = new DTOs.AmountDTO()
                        {
                            Currency = new DTOs.Search.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                            Value = frmBill.PaymentSummary[j].Amount
                        };

                        DTOs.BankDTO issuingBank = new DTOs.BankDTO() { Id = frmBill.PaymentSummary[j].TransferPayments[0].IssuingBankId };

                        DTOs.BankAccounts.BankAccountPersonDTO recievingAccount = new DTOs.BankAccounts.BankAccountPersonDTO()
                        {
                            Bank = new DTOs.BankDTO() { Id = frmBill.PaymentSummary[j].TransferPayments[0].ReceivingBankId },
                            Number = frmBill.PaymentSummary[j].TransferPayments[0].ReceivingAccountNumber
                        };

                        DTOs.ExchangeRateDTO exchangeRate = new DTOs.ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                        DTOs.AmountDTO localAmount = new DTOs.AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                        collect.Payments.Add(new DTOs.Payments.TransferDTO()
                        {
                            Amount = amount,
                            Date = frmBill.PaymentSummary[j].TransferPayments[0].Date,
                            DocumentNumber = frmBill.PaymentSummary[j].TransferPayments[0].DocumentNumber,
                            ExchangeRate = exchangeRate,
                            Id = frmBill.PaymentSummary[j].PaymentId,
                            IssuerName = frmBill.PaymentSummary[j].TransferPayments[0].IssuerName,
                            IssuingAccountNumber = frmBill.PaymentSummary[j].TransferPayments[0].IssuingAccountNumber,
                            IssuingBank = issuingBank, 
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            ReceivingAccount = recievingAccount,
                            Status = 1 //Convert.ToInt16(PaymentStatus.Active)
                        });
                    }
                    #endregion

                    #region Deposit

                    // Boleta de depósito
                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"]))
                    {
                        DTOs.AmountDTO amount = new DTOs.AmountDTO()
                        {
                            Currency = new DTOs.Search.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                            Value = frmBill.PaymentSummary[j].Amount
                        };

                        DTOs.BankAccounts.BankAccountCompanyDTO receivingAccount = new DTOs.BankAccounts.BankAccountCompanyDTO()
                        {
                            Bank = new DTOs.BankDTO() { Id = frmBill.PaymentSummary[j].DepositVouchers[0].ReceivingAccountBankId },
                            Number = frmBill.PaymentSummary[j].DepositVouchers[0].ReceivingAccountNumber
                        };

                        DTOs.ExchangeRateDTO exchangeRate = new DTOs.ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                        DTOs.AmountDTO localAmount = new DTOs.AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                        collect.Payments.Add(new DTOs.Payments.DepositVoucherDTO()
                        {
                            Amount = amount,
                            Date = frmBill.PaymentSummary[j].DepositVouchers[0].Date,
                            DepositorName = frmBill.PaymentSummary[j].DepositVouchers[0].DepositorName,
                            ExchangeRate = exchangeRate,
                            Id = frmBill.PaymentSummary[j].PaymentId,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            ReceivingAccount = receivingAccount,
                            VoucherNumber = frmBill.PaymentSummary[j].DepositVouchers[0].VoucherNumber,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion

                    #region Retention

                    // Recibo de retención
                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"]))
                    {
                        DTOs.AmountDTO amount = new DTOs.AmountDTO()
                        {
                            Currency = new DTOs.Search.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                            Value = frmBill.PaymentSummary[j].Amount
                        };

                        DTOs.ExchangeRateDTO exchangeRate = new DTOs.ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                        DTOs.AmountDTO localAmount = new DTOs.AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                        collect.Payments.Add(new DTOs.Payments.RetentionReceiptDTO()
                        {
                            Amount = amount,
                            AuthorizationNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].AuthorizationNumber,
                            BillNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].BillNumber,
                            Date = frmBill.PaymentSummary[j].RetentionReceipts[0].Date,
                            ExchangeRate = exchangeRate,
                            Id = frmBill.PaymentSummary[j].PaymentId,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            SerialNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].SerialNumber,
                            SerialVoucherNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].SerialVoucherNumber,
                            VoucherNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].VoucherNumber,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }
                    #endregion

                    #endregion
                }
            }

            
            DTOs.CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(userId, Convert.ToInt32(branchId), Convert.ToDateTime(DateTime.Now).Date, status);
            accountingDate = collectControl != null ? collectControl.AccountingDate : DateTime.MinValue;
            // Save Collect

            var saveCheckingReGularizatedResult = DelegateService.accountingPaymentService.SaveRegularizationCollect(collect,
                                                            billControlId, sourcePaymentId, branchId, accountingDate, userId);
            //collect = DelegateService.accountingCollectService.SaveRegularizationCollect(collect, billControlId, sourcePaymentId, branchId);
            

            var saveRegularization = new
            {
                Id = saveCheckingReGularizatedResult.BillId,
                Description = collect.Description,
                TechnicalTransaction = Convert.ToString(saveCheckingReGularizatedResult.TechnicalTransaction),
                Status = collect.Status,
                ShowMessage = Convert.ToString(saveCheckingReGularizatedResult.ShowMessage),
                Message = saveCheckingReGularizatedResult.ImputationMessage,
                saveCheckingReGularizatedResult.GeneralLedgerSuccess
            };

            return Json(saveRegularization, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion
        
        #region MainCardVoucherRegularizationñ

        /// <summary>
        /// MainCardVoucherRegularization
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCardVoucherRegularization()
        {
            try
            {       

                int defaultValue = Convert.ToInt16(@Global.DefaultValue);
                string defaultDescription = (@Global.DefaultDescription);

                List<DTOs.Payments.PaymentMethodDTO> paymentMethods = new List<DTOs.Payments.PaymentMethodDTO>();
                ViewBag.PaymentMethod = paymentMethods;

                var currencies = DelegateService.commonService.GetCurrencies();
                ViewBag.Currency = currencies;

                List<object> validationMonths = new List<object>();
                for (int i = 1; i <= 12; i++)
                {
                    validationMonths.Add(new { Id = i, Description = i });
                }
                ViewBag.ValidationMonth = validationMonths;

                int localCurrencyId = Convert.ToInt32(ConfigurationManager.AppSettings["LocalCurrencyId"]);
                ViewBag.localCurrencyId = localCurrencyId;

                ViewBag.DefaultValue = defaultValue;
                ViewBag.DefaultDescription = defaultDescription;

                // Payment_Methods
                ViewBag.ParamPaymentMethodPostdatedCheck = ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"];
                ViewBag.ParamPaymentMethodCurrentCheck = ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"];
                ViewBag.ParamPaymentMethodCash = ConfigurationManager.AppSettings["ParamPaymentMethodCash"];
                ViewBag.ParamPaymentMethodCreditableCreditCard = ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"];
                ViewBag.ParamPaymentMethodUncreditableCreditCard = ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"];
                ViewBag.ParamPaymentMethodDebit = ConfigurationManager.AppSettings["ParamPaymentMethodDebit"];
                ViewBag.ParamPaymentMethodDirectConection = ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"];
                ViewBag.ParamPaymentMethodTransfer = ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"];
                ViewBag.ParamPaymentMethodDepositVoucher = ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"];
                ViewBag.ParamPaymentMethodRetentionReceipt = ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"];
                ViewBag.ParamPaymentMethodDataphone = ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"];
                ViewBag.ParamPaymentMethodElectronicPayment = ConfigurationManager.AppSettings["ParamPaymentMethodElectronicPayment"];
                ViewBag.ParamPaymentMethodPaymentArea = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentArea"];
                ViewBag.ParamPaymentMethodPaymentCard = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentCard"];

                // Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.idBillControl = TempData["idBillControl"];
                ViewBag.idBranch = TempData["idBranch"];

                ViewBag.PaymentId = TempData["PaymentId"] ?? 0;
                ViewBag.VoucherNumber = TempData["VoucherNumber"] ?? 0;
                ViewBag.BranchIdOrigin = TempData["BranchIdOrigin"] ?? 0;
                var userNicks = _commonController.GetUserByName(User.Identity.Name);
                ViewBag.UserNick = userNicks[0].AccountName;

                return View("~/Areas/Accounting/Views/Regularization/MainCardVoucherRegularization.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region LoadCardRegularization

        /// <summary>
        /// LoadVoucherRegularization
        /// Redirecciona a la página de regularización de voucher
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="voucherNumber"></param>
        /// <param name="branchIdOrigin"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadVoucherRegularization(int paymentId, string voucherNumber, int branchIdOrigin)
        {
            TempData["PaymentId"] = paymentId;
            TempData["VoucherNumber"] = voucherNumber;
            TempData["BranchIdOrigin"] = branchIdOrigin;

            return RedirectToAction("MainCardVoucherRegularization");
        }

        #endregion

        #region LoadRejectedCardVoucherInfo

        /// <summary>
        /// GetRejectedCardVoucherInfoByPaymentId 
        /// Obtiene los datos para mostrar en regularizacion de tarjetas
        /// Autor: Alejandro Villagrán
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRejectedCardVoucherInfoByPaymentId(int paymentId)
        {
            DTOs.Search.RejectedCardVoucherInfoDTO rejectedCardVoucherInfoDTO = DelegateService.accountingPaymentService.GetRejectedCardVoucherInfoByPaymentId(paymentId);

            List<object> rejectedCardVouchers = new List<object>();

            string DocumentNumber;

            if (rejectedCardVoucherInfoDTO.DocumentNumber != null)
            {
                DocumentNumber = ReplaceWithAsterisks(rejectedCardVoucherInfoDTO.DocumentNumber);
            }
            else
            {
                DocumentNumber = rejectedCardVoucherInfoDTO.DocumentNumber;
            }

            rejectedCardVouchers.Add(new
            {
                CreditCardTypeCode = rejectedCardVoucherInfoDTO.CreditCardTypeCode,
                CardDescription = rejectedCardVoucherInfoDTO.CardDescription,
                VoucherRejectCard = rejectedCardVoucherInfoDTO.Voucher,
                BillCode = rejectedCardVoucherInfoDTO.CollectCode,
                CardDate = rejectedCardVoucherInfoDTO.CardDate.ToString("dd/MM/yyyy"),
                CurrencyCode = rejectedCardVoucherInfoDTO.CurrencyCode,
                CurrencyDescription = rejectedCardVoucherInfoDTO.CurrencyDescription,
                AmountRejectCard = rejectedCardVoucherInfoDTO.Amount,
                Tax = rejectedCardVoucherInfoDTO.Tax,
                StatusRejectCard = rejectedCardVoucherInfoDTO.Status,
                StatusDescription = rejectedCardVoucherInfoDTO.StatusDescription,
                RejectionDate = rejectedCardVoucherInfoDTO.RejectionDate,
                RejectedPaymentCode = rejectedCardVoucherInfoDTO.RejectedPaymentCode,
                RejectionDescription = rejectedCardVoucherInfoDTO.RejectionDescription,
                Holder = rejectedCardVoucherInfoDTO.Holder,
                DocumentNumber = DocumentNumber
            });

            return Json(rejectedCardVouchers, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}