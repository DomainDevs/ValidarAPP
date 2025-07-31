//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using AccountingDTO = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Transfers;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class CheckInternalDepositSlipController : Controller
    {
        #region Instance variables

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainCheckInternalDepositSlip
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCheckInternalDepositSlip()
        {
            try
            {

                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                ViewBag.PaymentMethodCash = Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// GetAccountNumbers
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountNumbers(int bankId)
        {
            List<object> bankAccounts = new List<object>();
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))).ToList();

            foreach (BankAccountCompanyDTO companyBankAccount in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    Id = companyBankAccount.Id,
                    Description = companyBankAccount.Number
                });
            }

            return new UifSelectResult(bankAccounts);
        }

        /// <summary>
        /// GetAccountCurrencyByBankId
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountCurrencyByBankId(int bankId, string accountNumber)
        {
            BankAccountCompanyDTO bankAccountCompany = _commonController.GetAccountCurrencyByBankId(bankId, accountNumber);

            return Json(bankAccountCompany, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetEnabledPaymentMethodTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetEnabledPaymentMethodTypes()
        {
            List<object> paymentMethodTypes = new List<object>();

            List<PaymentMethodTypeDTO> paymentMethodTypeDTOs =
            DelegateService.accountingParameterService.GetEnablePaymentMethodType(false, false, true);

            if (paymentMethodTypeDTOs.Count != 0)
            {
                foreach (PaymentMethodTypeDTO paymentMethodType in paymentMethodTypeDTOs)
                {
                    paymentMethodTypes.Add(new
                    {
                        Id = paymentMethodType.PaymentTypeCode,
                        Description = paymentMethodType.Description
                    });
                }
            }

            return new UifSelectResult(paymentMethodTypes);
        }

        /// <summary>
        /// GetBanks
        /// Obtiene los bancos de la tabla COMM.BANK, para el autocomplete
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBanks(string query)
        {
            List<object> bankResponses = new List<object>();
            try
            {

                //NA es de common
                int length = query.Length;

                List<Bank> banks = DelegateService.commonService.GetBanks();

                foreach (Bank bank in banks)
                {
                    if ((length <= bank.Description.Length) && (((bank.Description).IndexOf(query.ToUpper())) > -1))
                    {
                        bankResponses.Add(new
                        {
                            bankId = bank.Id,
                            bankDescription = bank.Description
                        });
                    }
                }

                if (bankResponses.Count == 0)
                {
                    bankResponses.Add(new
                    {
                        bankId = 0,
                        bankDescription = @Global.RegisterNotFound,
                    });
                }

                return Json(bankResponses, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetChecksToDepositBallot
        /// Obtiene los cheques corrientes/postfechados a depositar en una boleta interna. Se filtra por el tipo de pago y 
        /// moneda (obligatorio), banco emisor y número de cuenta emisora (opcionales)
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="checkNumber"></param>
        /// <param name="currencyCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetChecksToDepositBallot(string branchId, string paymentMethodTypeCode, string issuingBankCode,
                                                   string checkNumber, string currencyCode)
        {
            List<object> checksToDepositInternalBallots = new List<object>();

            if (String.IsNullOrEmpty(branchId))
            {
                branchId = "-1";
            }
            if (String.IsNullOrEmpty(paymentMethodTypeCode))
            {
                paymentMethodTypeCode = "-1";
            }
            if (String.IsNullOrEmpty(currencyCode))
            {
                currencyCode = "-1";
            }
            if (issuingBankCode == "undefined" || issuingBankCode == "")
            {
                issuingBankCode = "-1";
            }
            checkNumber = checkNumber == "" ? "-1" : checkNumber;


            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            List<CheckToDepositInternalBallotDTO> checksToDepositInternalBallotDTOs = new List<CheckToDepositInternalBallotDTO>();
            checksToDepositInternalBallotDTOs = DelegateService.accountingPaymentTicketService.GetChecksToDepositBallot(Convert.ToInt32(branchId), Convert.ToInt32(paymentMethodTypeCode),
             Convert.ToInt32(currencyCode), Convert.ToInt32(issuingBankCode), Convert.ToInt32(checkNumber), userId);

            //Ordenamos la data
            List<CheckToDepositInternalBallotDTO> checkToDepositInternalBallotOrders = (from order in checksToDepositInternalBallotDTOs
                                                                                        orderby order.BranchName, order.CheckDate
                                                                                        select order).ToList();

            foreach (CheckToDepositInternalBallotDTO checkToDepositInternalBallot in checkToDepositInternalBallotOrders)
            {
                checksToDepositInternalBallots.Add(new
                {
                    PaymentTicketItemId = checkToDepositInternalBallot.PaymentTicketItemId,
                    PaymentCode = checkToDepositInternalBallot.PaymentCode,
                    PaymentMethodTypeCode = checkToDepositInternalBallot.PaymentMethodTypeCode,
                    CurrencyCode = checkToDepositInternalBallot.CurrencyCode,
                    BranchCode = checkToDepositInternalBallot.BranchCode,
                    BranchName = checkToDepositInternalBallot.BranchName,
                    BankName = checkToDepositInternalBallot.BankName,
                    IssuingAccountNumber = checkToDepositInternalBallot.IssuingAccountNumber,
                    CheckNumber = checkToDepositInternalBallot.CheckNumber,
                    ReceiptNumber = checkToDepositInternalBallot.ReceiptNumber,
                    CurrencyName = checkToDepositInternalBallot.CurrencyName,
                    IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", checkToDepositInternalBallot.IncomeAmount),
                    CheckDate = checkToDepositInternalBallot.CheckDate.ToString("dd/MM/yyyy"),
                    TechnicalTransaction = checkToDepositInternalBallot.TechnicalTransaction,
                    Holder = checkToDepositInternalBallot.Holder
                });
            }

            return Json(new
            {
                aaData = checksToDepositInternalBallots,
                total = checksToDepositInternalBallots.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateCashAmount
        /// Valida que el importe en efectivo ingresado no sea mayor total del efectivo recibido por caja menos el efectivo que ya esté asociado a 
        /// otras boletas internas del mismo usuario y fecha de caja abierta
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currencyId"></param>
        /// <param name="cashAmountAdmitted"></param>
        /// <param name="registerDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateCashAmount(int branchId, int currencyId, decimal cashAmountAdmitted,
                                             string registerDate, int paymentTicketId)
        {
            try
            {
                var cashAllow = DelegateService.accountingPaymentTicketService.ValidateCashAmount(branchId, currencyId, _commonController.GetUserIdByName(User.Identity.Name.ToUpper()),
                                                                         cashAmountAdmitted, registerDate == "" ? DateTime.Now.ToString() : registerDate, paymentTicketId);
                return new UifJsonResult(true, cashAllow);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
                throw;
            }

        }

        /// <summary>
        /// GetBallotsNotDeposited
        /// Obtiene las boletas internas de depósito que no están depositadas para su respectiva modificación
        /// </summary>
        /// <param name="internalBallotNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBallotsNotDeposited(int internalBallotNumber)
        {
            List<BallotNotDepositedDTO> ballotNotDepositedDTOs = DelegateService.accountingPaymentTicketService.GetInternalBallotNotDeposited(internalBallotNumber);

            List<object> ballotNotDepositeds = new List<object>();

            if (ballotNotDepositedDTOs.Count > 0)
            {
                foreach (BallotNotDepositedDTO ballot in ballotNotDepositedDTOs)
                {
                    ballotNotDepositeds.Add(new
                    {
                        PaymentTicketCode = ballot.PaymentTicketCode,
                        BankCode = ballot.BankCode,
                        BankName = ballot.BankName,
                        BranchCode = ballot.BranchCode,
                        BranchName = ballot.BranchName,
                        AccountNumber = ballot.AccountNumber,
                        CurrencyCode = ballot.CurrencyCode,
                        CurrencyName = ballot.CurrencyName,
                        CashAmount = ballot.CashAmount,
                        CheckAmount = ballot.CheckAmount,
                        PaymentMethodTypeCode = ballot.PaymentMethodTypeCode,
                        RegisterDate = ballot.RegisterDate
                    });
                }
            }
            else
            {
                ballotNotDepositeds = null;
            }

            return Json(ballotNotDepositeds, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDetailInternalBallotNotDeposited
        /// Obtiene el detalle de una boleta interna de depósito para su respectiva modificación
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="checkDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDetailInternalBallotNotDeposited(string branchCode, string paymentTicketCode, string paymentStatus, string checkDate)
        {
            List<object> ballotNotDepositedDetails = new List<object>();

            List<DetailBallotNotDepositedDTO> detailBallotNotDepositedDTOs =
            DelegateService.accountingPaymentTicketService.GetDetailInternalBallotNotDeposited(Convert.ToInt32(branchCode), Convert.ToInt32(paymentTicketCode), Convert.ToInt32(paymentStatus),
                                                                                              checkDate == "-1" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(checkDate));
            if (detailBallotNotDepositedDTOs.Count > 0)
            {
                foreach (DetailBallotNotDepositedDTO detailBallotNotDeposited in detailBallotNotDepositedDTOs)
                {
                    ballotNotDepositedDetails.Add(new
                    {
                        PaymentCode = detailBallotNotDeposited.PaymentCode,
                        BranchCode = detailBallotNotDeposited.BranchCode,
                        BranchName = detailBallotNotDeposited.BranchName,
                        PaymentMethodTypeCode = detailBallotNotDeposited.PaymentMethodTypeCode,
                        PaymentMethodTypeName = detailBallotNotDeposited.PaymentMethodTypeName,
                        IssuingBankCode = detailBallotNotDeposited.IssuingBankCode,
                        BankName = detailBallotNotDeposited.BankName,
                        IssuingAccountNumber = detailBallotNotDeposited.IssuingAccountNumber,
                        CheckNumber = detailBallotNotDeposited.CheckNumber,
                        ReceiptNumber = detailBallotNotDeposited.ReceiptNumber,
                        CurrencyCode = detailBallotNotDeposited.CurrencyCode,
                        CurrencyName = detailBallotNotDeposited.CurrencyName,
                        ExchangeRate = detailBallotNotDeposited.ExchangeRate,
                        IncomeAmount = detailBallotNotDeposited.IncomeAmount,
                        Amount = detailBallotNotDeposited.Amount,
                        CheckDate = String.Format("{0:dd/MM/yyyy}", detailBallotNotDeposited.CheckDate),
                        Holder = detailBallotNotDeposited.Holder,
                        PaymentTicketCode = detailBallotNotDeposited.PaymentTicketCode,
                        PaymentTicketItemCode = detailBallotNotDeposited.PaymentTicketItemCode,
                        Correlative = detailBallotNotDeposited.Correlative,
                    });
                }
            }

            return new UifTableResult(ballotNotDepositedDetails);
        }

        /// <summary>
        /// SaveInternalBallotDeposit
        /// </summary>
        /// <param name="tblChecksModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveInternalBallotDeposit(Models.Transfers.TblChecksModel tblChecksModel)
        {
            try
            {
                AccountingDTO.PaymentTicketDTO paymentTicket = new AccountingDTO.PaymentTicketDTO();
                List<PaymentDTO> payments = new List<PaymentDTO>();

                decimal total = 0;
                decimal commission = 0;

                if ((tblChecksModel.PaymentTicket != null) && (tblChecksModel.PaymentTicket.Count > 0))
                {
                    for (int i = 0; i < tblChecksModel.PaymentTicket.Count; i++)
                    {
                        total = (total + Convert.ToDecimal(tblChecksModel.PaymentTicket[i].Amount));
                        commission = (commission + Convert.ToDecimal(tblChecksModel.PaymentTicket[i].CommissionAmount));

                        if (i == (tblChecksModel.PaymentTicket.Count - 1))
                        {
                            //Objeto PaymentTicket
                            paymentTicket.Amount = new AmountDTO();
                            paymentTicket.Amount.Value = total;
                            paymentTicket.Commission = new AmountDTO();
                            paymentTicket.Commission.Value = commission;
                            paymentTicket.Branch = new BranchDTO();
                            paymentTicket.Branch.Id = tblChecksModel.PaymentTicket[i].Branch != null ? (int)tblChecksModel.PaymentTicket[i].Branch : -1;
                            paymentTicket.Bank = new BankDTO();
                            paymentTicket.Bank.Id = tblChecksModel.PaymentTicket[i].Bank != null ? (int)tblChecksModel.PaymentTicket[i].Bank : -1;
                            paymentTicket.Bank.Description = tblChecksModel.PaymentTicket[i].BankDescription;
                            paymentTicket.CashAmount = new AmountDTO();
                            paymentTicket.CashAmount.Value = Convert.ToDecimal(tblChecksModel.PaymentTicket[i].CashAmount);
                            paymentTicket.AccountNumber = tblChecksModel.PaymentTicket[i].AccountNumber;
                            paymentTicket.PaymentMethod = tblChecksModel.PaymentTicket[i].PaymentMethodId;
                            paymentTicket.Currency = new CurrencyDTO();
                            paymentTicket.Currency.Id = Convert.ToInt32(tblChecksModel.PaymentTicket[i].Currency);
                        }
                    }

                    for (int i = 0; i < tblChecksModel.PaymentTicket.Count; i++)
                    {
                        PaymentDTO payment = new PaymentDTO();
                        if (tblChecksModel.PaymentTicket[i].PaymentId > 0)
                        {
                            payment.Id = tblChecksModel.PaymentTicket[i].PaymentId;
                            payments.Add(payment);
                        }
                    }
                    paymentTicket.Payments = payments;
                }

                paymentTicket = DelegateService.accountingPaymentTicketService.SaveInternalBallot(paymentTicket, _commonController.GetUserIdByName(User.Identity.Name.ToUpper()));

                List<object> paymentTicketResponses = new List<object>();

                paymentTicketResponses.Add(new
                {
                    Id = paymentTicket.Id,
                    Total = total,
                    Bank = tblChecksModel.PaymentTicket[0].BankDescription,
                    Date = DateTime.Now.ToString(),
                    User = (User.Identity.Name).ToUpper(),
                    MethodType = tblChecksModel.PaymentTicket[0].PaymentMethodId
                });

                return Json(paymentTicketResponses, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = unhandledException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateInternalBallot
        /// Actualiza un registro existente en la tabla ACC.PAYMENT_TICKET, ACC.PAYMENT_ICKET_ITEM e inserta en ACC.PAYMENT_LOG
        /// </summary>
        /// <param name="checksModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateInternalBallot(Models.Transfers.TblChecksModel checksModel)
        {
            AccountingDTO.PaymentTicketDTO paymentTicket = new AccountingDTO.PaymentTicketDTO();
            List<PaymentDTO> payments = new List<PaymentDTO>();

            decimal total = 0;
            decimal commission = 0;

            if ((checksModel.PaymentTicket != null) && (checksModel.PaymentTicket.Count > 0))
            {
                total = checksModel.PaymentTicket.Sum(x => Convert.ToDecimal(x.Amount));
                commission = checksModel.PaymentTicket.Sum(x => Convert.ToDecimal(x.CommissionAmount));

                PaymentTicketModel ptm = checksModel.PaymentTicket.First();
                paymentTicket.Id = ptm.PaymentTicketId;
                paymentTicket.Amount = new AmountDTO
                {
                    Value = total
                };
                paymentTicket.Commission = new AmountDTO
                {
                    Value = commission
                };
                paymentTicket.Branch = new BranchDTO()
                {
                    Id = ptm.Branch != null ? (int)ptm.Branch : -1
                };
                paymentTicket.Bank = new BankDTO()
                {
                    Id = ptm.Bank != null ? (int)ptm.Bank : -1,
                    Description = ptm.BankDescription
                };
                paymentTicket.Status = 1;
                paymentTicket.CashAmount = new AmountDTO()
                {
                    Value = Convert.ToDecimal(ptm.CashAmount)
                };
                paymentTicket.AccountNumber = ptm.AccountNumber;
                paymentTicket.PaymentMethod = ptm.PaymentMethodId;
                paymentTicket.Currency = new CurrencyDTO()
                {
                    Id = Convert.ToInt32(ptm.Currency)
                };

                checksModel.PaymentTicket.Where(x => x.PaymentId > 0).ToList().ForEach(x =>
                {
                    payments.Add(new PaymentDTO()
                    {
                        Id = x.PaymentId
                    });
                });
                
                paymentTicket.Payments = payments;
                paymentTicket = DelegateService.accountingPaymentTicketService.UpdateInternalBallot(paymentTicket, _commonController.GetUserIdByName(User.Identity.Name));
            }

            object result = new
            {
                paymentTicket.Id,
                CashAmount = paymentTicket.CashAmount.Value,
                Amount = paymentTicket.Amount.Value,
                Total = paymentTicket.Amount.Value + paymentTicket.CashAmount.Value,
                Bank = checksModel.PaymentTicket[0].BankDescription,
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                User = (User.Identity.Name).ToUpper(),
                MethodType = paymentTicket.PaymentMethod
            };

            return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteInternalBallot
        /// </summary>
        /// <param name="checksModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteInternalBallot(Models.Transfers.TblChecksModel checksModel)
        {
            try
            {
                AccountingDTO.PaymentTicketDTO paymentTicket = new AccountingDTO.PaymentTicketDTO();
                List<PaymentDTO> payments = new List<PaymentDTO>();
                List<PaymentTaxDTO> paymentTaxes;

                for (int i = 0; i < checksModel.PaymentTicket.Count; i++)
                {
                    if (checksModel.PaymentTicket[i].PaymentTicketItemId > 0)
                    {
                        paymentTaxes = new List<PaymentTaxDTO>();
                        paymentTaxes.Add(new PaymentTaxDTO()
                        {
                            Id = checksModel.PaymentTicket[i].PaymentTicketItemId
                        });

                        payments.Add(new PaymentDTO()
                        {
                            Id = checksModel.PaymentTicket[i].PaymentId,
                            Taxes = paymentTaxes
                        });
                    }
                    else if (checksModel.PaymentTicket[i].PaymentId > 0)
                    {
                        payments.Add(new PaymentDTO()
                        {
                            Id = checksModel.PaymentTicket[i].PaymentId,
                        });
                    }
                }

                paymentTicket.Id = checksModel.PaymentTicket[0].PaymentTicketId;
                paymentTicket.Payments = payments;
                DelegateService.accountingPaymentTicketService.DeleteInternalBallot(paymentTicket, _commonController.GetUserIdByName(User.Identity.Name));

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = unhandledException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}