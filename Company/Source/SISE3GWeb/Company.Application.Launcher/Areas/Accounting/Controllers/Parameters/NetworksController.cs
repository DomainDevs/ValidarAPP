using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;

// Sistran Core
using Sistran.Core.Application.AutomaticDebitServices;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Parameters
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class NetworksController : Controller
    {
        #region Instance Variables


        #endregion

        #region Views

        /// <summary>
        /// MainNetwork
        /// Pantalla de redes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainNetwork()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/AutomaticDebit/MainNetwork.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        /// <summary>
        /// MainDebitStatusCodes
        /// Pantalla estados de debito
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainDebitStatusCodes()
        {
            try
            {
                ViewBag.MaxLength = 5;
                return View("~/Areas/Accounting/Views/Parameters/AutomaticDebit/MainDebitStatusCodes.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }  
        }

        /// <summary>
        /// MainBankResponseCodes
        /// Pantalla respues del banco
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankResponseCodes()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/AutomaticDebit/MainBankResponseCodes.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }           
        }

        /// <summary>
        /// MainBankNetworkRelationship
        /// Pantalla Redes de bancos 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankNetworkRelationship()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/AutomaticDebit/MainBankNetworkRelationship.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        /// <summary>
        /// MainRelationshipAccountType
        /// Pantalla relación tipo de cuenta
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainRelationshipAccountType()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/AutomaticDebit/MainRelationshipAccountType.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        #endregion

        #region Networks

        /// <summary>
        /// GetNetworks
        /// Obtiene redes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetNetworks()
        {
            List<BankNetwork> bankNetworks = DelegateService.automaticDebitService.GetBankNetworks();

            var bankNetworksResponse = from items in bankNetworks
                           select new
                           {
                               Id = items.Id,
                               Description = items.Description,
                               IsHasTax = items.HasTax,
                               TaxCategoryId = items.TaxCategory.Id,
                               TaxCategoryDescription = items.TaxCategory.Description,
                               CommissionValue = string.Format(new CultureInfo("en-US"), "{0:C}", items.Commission.Value),
                               RetriesNumber = items.RetriesNumber,
                               PreNotifications = items.RequiresNotification
                           };            

            return new UifTableResult(bankNetworksResponse);
        }

        /// <summary>
        /// SaveBankNetwork
        /// Graba las redes del banco
        /// </summary>
        /// <param name="bankNetworkModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveBankNetwork(BankNetworkModel bankNetworkModel, string operationType)
        {
            List<object> bankNetworks = new List<object>();
            BankNetwork bankNetwork = new BankNetwork();

            bankNetwork.Commission = new Amount() { Value = bankNetworkModel.CommissionValue };
            bankNetwork.Description = bankNetworkModel.Description;
            bankNetwork.HasTax = bankNetworkModel.HasTax;
            bankNetwork.Id = bankNetworkModel.Id;
            bankNetwork.RequiresNotification = bankNetworkModel.RequiresNotification;
            bankNetwork.RetriesNumber = bankNetworkModel.RetriesNumber;
            bankNetwork.TaxCategory = new TaxCategory() { Id = bankNetworkModel.TaxTypeId };

            if (operationType.Equals("I"))
            {
                bankNetwork = DelegateService.automaticDebitService.SaveBankNetwork(bankNetwork);
            }
            if (operationType.Equals("U"))
            {
                bankNetwork = DelegateService.automaticDebitService.UpdateBankNetwork(bankNetwork);
            }
            if (operationType.Equals("D"))
            {
                DelegateService.automaticDebitService.DeleteBankNetwork(bankNetwork);
            }

            bankNetworks.Add(new
            {
                BankNetworkCode = bankNetwork.Id,
                MessageError = bankNetwork.Description
            });

            return Json(bankNetworks, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBankNetwork
        /// Borra Redes del banco
        /// </summary>
        /// <param name="bankNetworkModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteBankNetwork(BankNetworkModel bankNetworkModel, string operationType)
        {
            bool isDeleted = false;
            List<object> bankNetworResponses = new List<object>();
            BankNetwork bankNetwork = new BankNetwork();
            bankNetwork.Commission = new Amount() { Value = Convert.ToDecimal(bankNetworkModel.CommissionValue) };
            bankNetwork.Description = bankNetworkModel.Description;
            bankNetwork.HasTax = bankNetworkModel.HasTax;
            bankNetwork.Id = bankNetworkModel.Id;
            bankNetwork.RequiresNotification = bankNetworkModel.RequiresNotification;
            bankNetwork.RetriesNumber = bankNetworkModel.RetriesNumber;
            bankNetwork.TaxCategory = new TaxCategory() { Id = bankNetworkModel.TaxTypeId };

            if (operationType.Equals("D"))
            {
                isDeleted = DelegateService.automaticDebitService.DeleteBankNetwork(bankNetwork);
            }

            bankNetworResponses.Add(new
            {
                BankNetworkCode = (isDeleted) ? 0 : -1,
                MessageError = Global.YouCanNotDeleteTheRecord//"No se puede eliminar el registro, ya que tiene dependencias"
            });

            return Json(bankNetworResponses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DebitStatusCodes

        /// <summary>
        /// GetDebitStatusTables
        /// Obtiene estado de debito en tablas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetDebitStatusTables()
        {
            List<object> couponsStatusResponses = new List<object>();
            List<CouponStatus> couponsStatus = DelegateService.automaticDebitService.GetCouponStatusByGroup("T");

            foreach (CouponStatus couponStatus in couponsStatus)
            {
                couponsStatusResponses.Add(new
                {
                    Id = couponStatus.Id,
                    Description = couponStatus.ExternalDescription
                });
            }

            return new UifSelectResult(couponsStatusResponses);
        }

        /// <summary>
        /// GetDebitStatusCodesByTableId
        /// Obtiene los codigos de los estados del débito
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDebitStatusCodesByTableId(int tableId)
        {
            List<object> couponsStatusResponse = new List<object>();
            List<CouponStatus> couponsStatus = DelegateService.automaticDebitService.GetCouponStatusByGroup(tableId.ToString());

            foreach (CouponStatus couponStatus in couponsStatus)
            {
                couponsStatusResponse.Add(new
                           {
                               Id = couponStatus.Id,
                               SmallDescription = couponStatus.SmallDescription,
                               Description = couponStatus.Description,
                               IsEnabled = couponStatus.IsEnabled,
                               IsRetry = Convert.ToBoolean(couponStatus.RetriesNumber == 0 ? 0 : 1),
                               DebitStatusType = couponStatus.CouponStatusType == CouponStatusTypes.Rejected ? Global.Rejected: Global.Applied,
                               RetryDays = couponStatus.RetriesNumber
                           });
            }

            return Json(new { aaData = couponsStatusResponse, total = couponsStatusResponse.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveDebitStatusCodes
        /// Graba codigos de estado de débito
        /// </summary>
        /// <param name="debitStatusModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveDebitStatusCodes(DebitStatusCodesModel debitStatusModel, string operationType)
        {
            List<object> couponStatusResponses = new List<object>();
            CouponStatus couponStatus = new CouponStatus();

            couponStatus.CouponStatusType = debitStatusModel.DebitStatusType == "A" ? CouponStatusTypes.Applied : CouponStatusTypes.Rejected;
            couponStatus.Description = debitStatusModel.Description;
            couponStatus.ExternalDescription = "";
            couponStatus.GroupDescription = debitStatusModel.Id.ToString();
            couponStatus.Id = debitStatusModel.Id;
            couponStatus.IsEnabled = debitStatusModel.IsEnabled;
            couponStatus.RetriesNumber = debitStatusModel.RetryDays;
            couponStatus.SmallDescription = debitStatusModel.SmallDescription;

            if (operationType.Equals("I"))
            {
                couponStatus = DelegateService.automaticDebitService.SaveCouponStatus(couponStatus);
            }
            if (operationType.Equals("U"))
            {
                couponStatus = DelegateService.automaticDebitService.UpdateCouponStatus(couponStatus);
            }

            couponStatusResponses.Add(new
            {
                DebitStatusCode = couponStatus.Id,
                MessageError = couponStatus.Description
            });

            return Json(couponStatusResponses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteDebitStatusCodes
        /// Borra estados de debito
        /// </summary>
        /// <param name="debitStatusModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteDebitStatusCodes(DebitStatusCodesModel debitStatusModel, string operationType)
        {
            bool isDeleted = false;
            List<object> couponStatusResponses = new List<object>();
            CouponStatus couponStatus = new CouponStatus();

            couponStatus.CouponStatusType = debitStatusModel.DebitStatusType == "A" ? CouponStatusTypes.Applied : CouponStatusTypes.Rejected;
            couponStatus.Description = debitStatusModel.Description;
            couponStatus.ExternalDescription = "";
            couponStatus.GroupDescription = debitStatusModel.Id.ToString();
            couponStatus.Id = debitStatusModel.Id;
            couponStatus.IsEnabled = debitStatusModel.IsEnabled;
            couponStatus.RetriesNumber = debitStatusModel.RetryDays;
            couponStatus.SmallDescription = debitStatusModel.SmallDescription;

            if (operationType.Equals("D"))
            {
                isDeleted = DelegateService.automaticDebitService.DeleteCouponStatus(couponStatus);
            }

            couponStatusResponses.Add(new
            {
                DebitStatusCode = (isDeleted) ? 0 : -1,
                MessageError = couponStatus.Description
            });

            return Json(couponStatusResponses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BankNetworksStatus

        /// <summary>
        /// GetBankNetworkStatus
        /// Obtiene el estado de las redes del banco
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankNetworkStatus()
        {
            List<object> bankNetworkStatusResponses = new List<object>();

            List<BankNetworkStatus> bankNetworkStatus = DelegateService.automaticDebitService.BankNetworkStatus();

            foreach (BankNetworkStatus bankNetwork in bankNetworkStatus)
            {
                bankNetworkStatusResponses.Add(new
                {
                    Id = bankNetwork.BankNetwork.Id,
                    Description = bankNetwork.BankNetwork.Description,
                    TableCode = bankNetwork.Id,
                    RejectionDefaultCode = bankNetwork.RejectedCouponStatus[0].SmallDescription.Trim(),
                    RejectionDescription = bankNetwork.RejectedCouponStatus[0].Description,
                    AcceptedDefaultCode = bankNetwork.AcceptedCouponStatus[0].SmallDescription,
                    AcceptedDescription = bankNetwork.AcceptedCouponStatus[0].Description
                });                
            }

            return Json(new { aaData = bankNetworkStatusResponses, total = bankNetworkStatusResponses.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetRejectionStatusCodesByTableId
        /// Obtiene estado de rechazo por id de la tabla
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetRejectionStatusCodesByTableId(int tableId)
        {
            List<CouponStatus> couponsStatus = new List<CouponStatus>();

            couponsStatus = DelegateService.automaticDebitService.GetCouponStatusByGroup(tableId.ToString());

            var couponsStatusResponse = from items in couponsStatus
                               where items.CouponStatusType == CouponStatusTypes.Rejected
                           select new
                           {
                               Id = items.SmallDescription,
                               Description = items.Description
                           };

            return new UifSelectResult(couponsStatusResponse);
        }

        /// <summary>
        /// GetAcceptedStatusCodesByTableId
        /// Obtiene el estado aceptados por id de la tabla
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAcceptedStatusCodesByTableId(int tableId)
        {
            List<CouponStatus> couponsStatus = DelegateService.automaticDebitService.GetCouponStatusByGroup(tableId.ToString());

            var couponsStatusResponse = from items in couponsStatus
                           where items.CouponStatusType == CouponStatusTypes.Applied
                           select new
                           {
                               Id = items.SmallDescription,
                               Description = items.Description
                           };

            return new UifSelectResult(couponsStatusResponse);
        }

        /// <summary>
        /// SaveBankResponseCodes
        /// Graba respuesta del banco
        /// </summary>
        /// <param name="responseStatusModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveBankResponseCodes(BankResponseCodesModel responseStatusModel, string operationType)
        {
            List<object> bankNetworkStatusResponses = new List<object>();
            BankNetworkStatus bankNetworkStatus = new BankNetworkStatus();
            List<CouponStatus> rejectedCouponStatus = new List<CouponStatus>();
            List<CouponStatus> acceptedCouponStatus = new List<CouponStatus>();

            acceptedCouponStatus.Add(new CouponStatus()
            {
                CouponStatusType = CouponStatusTypes.Applied,
                SmallDescription = responseStatusModel.AcceptedCouponStatus,
                Id = responseStatusModel.TableCode,
            });

            rejectedCouponStatus.Add(new CouponStatus()
            {
                CouponStatusType = CouponStatusTypes.Rejected,
                SmallDescription = responseStatusModel.RejectedCouponStatus,
                Id = responseStatusModel.TableCode,
            });

            bankNetworkStatus.AcceptedCouponStatus = acceptedCouponStatus;
            bankNetworkStatus.BankNetwork = new BankNetwork() { Id = responseStatusModel.BankNetworkId };
            bankNetworkStatus.Id = responseStatusModel.TableCode;
            bankNetworkStatus.RejectedCouponStatus = rejectedCouponStatus;

            if (operationType.Equals("I"))
            {
                bankNetworkStatus = DelegateService.automaticDebitService.SaveBankNetworkStatus(bankNetworkStatus);
            }
            if (operationType.Equals("U"))
            {
                bankNetworkStatus = DelegateService.automaticDebitService.UpdateBankNetworkStatus(bankNetworkStatus);
            }

            bankNetworkStatusResponses.Add(new
            {
                BankResponseStatusCode = bankNetworkStatus.Id,
                MessageError = bankNetworkStatus.BankNetwork.Description
            });

            return Json(bankNetworkStatusResponses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBankResponseCodes
        /// Borra respuesta del banco
        /// </summary>
        /// <param name="responseStatusModel"></param>
        /// <param name="operationType"></param>
        /// <returns>>ActionResult</returns>
        public ActionResult DeleteBankResponseCodes(BankResponseCodesModel responseStatusModel, string operationType)
        {
            bool isDeleted = false;
            List<object> bankNetworkStatusResponses = new List<object>();
            BankNetworkStatus bankNetworkStatus = new BankNetworkStatus();

            List<CouponStatus> rejectedCouponStatus = new List<CouponStatus>();
            List<CouponStatus> acceptedCouponStatus = new List<CouponStatus>();

            acceptedCouponStatus.Add(new CouponStatus()
            {
                CouponStatusType = CouponStatusTypes.Applied,
                SmallDescription = responseStatusModel.AcceptedCouponStatus,
                Id = responseStatusModel.TableCode,
            });

            rejectedCouponStatus.Add(new CouponStatus()
            {
                CouponStatusType = CouponStatusTypes.Rejected,
                SmallDescription = responseStatusModel.RejectedCouponStatus,
                Id = responseStatusModel.TableCode,
            });

            bankNetworkStatus.AcceptedCouponStatus = acceptedCouponStatus;
            bankNetworkStatus.BankNetwork = new BankNetwork() { Id = responseStatusModel.BankNetworkId };
            bankNetworkStatus.Id = responseStatusModel.TableCode;
            bankNetworkStatus.RejectedCouponStatus = rejectedCouponStatus;

            if (operationType.Equals("D"))
            {
                isDeleted = DelegateService.automaticDebitService.DeleteBankNetworkStatus(bankNetworkStatus);
            }

            bankNetworkStatusResponses.Add(new
            {
                BankResponseStatusCode = (isDeleted) ? 0 : -1,
                MessageError = "No se puede eliminar el registro, ya que tiene dependencias"
            });

            return Json(bankNetworkStatusResponses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BankNetworkRelationship

        /// <summary>
        /// GetBankNetworkRelationshipByBankNetworkId
        /// Obtiene las redes relacionadas por la red del banco
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankNetworkRelationshipByBankNetworkId(int bankNetworkId)
        {
            PaymentMethodBankNetwork paymentMethodBankNetwork = new PaymentMethodBankNetwork();
            paymentMethodBankNetwork.BankNetwork = new BankNetwork() { Id = bankNetworkId };

            List<PaymentMethodBankNetwork> paymentMethodBankNetworks = DelegateService.automaticDebitService.GetPaymentMethodBankNetworks(paymentMethodBankNetwork);

            var paymentMethodBankNetworksResponse = from items in paymentMethodBankNetworks
                           select new
                           {
                               Id = items.BankNetwork.Id,
                               PaymentMethodId = items.PaymentMethod.Id,
                               PaymentMethodDescription = items.PaymentMethod.Description,
                               Generate = items.ToGenerate,
                               BankAndAccount = items.BankAccountCompany.Bank.Description + " - " + items.BankAccountCompany.Number, 
                               BankId = items.BankAccountCompany.Bank.Id,
                               AccountBankId = items.BankAccountCompany.Id
                           };

            return new UifTableResult(paymentMethodBankNetworksResponse);
        }

        /// <summary>
        /// SavePaymentMethodBankNetwork
        /// Graba tipo de pago de cuerdo a la red del banco
        /// </summary>
        /// <param name="paymentMethodModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SavePaymentMethodBankNetwork(PaymentMethodBankNetworkModel paymentMethodModel, string operationType)
        {
            List<object> paymentMethodBankNetworks = new List<object>();
            PaymentMethodBankNetwork paymentMethodBankNetwork = new PaymentMethodBankNetwork();

            paymentMethodBankNetwork.BankAccountCompany = new BankAccountCompanyDTO()
            {
              Id = paymentMethodModel.AccountBankId
            };
            paymentMethodBankNetwork.BankNetwork = new BankNetwork()
            {
              Id = paymentMethodModel.Id
            };
            paymentMethodBankNetwork.Id = 0;
            paymentMethodBankNetwork.PaymentMethod = new PaymentMethodDTO() { Id = paymentMethodModel.PaymentMethodId };
            paymentMethodBankNetwork.ToGenerate = paymentMethodModel.ToGenerate;

            if (operationType.Equals("I"))
            {
                paymentMethodBankNetwork = DelegateService.automaticDebitService.SavePaymentMethodBankNetwork(paymentMethodBankNetwork);
            }
            if (operationType.Equals("U"))
            {
                paymentMethodBankNetwork = DelegateService.automaticDebitService.UpdatePaymentMethodBankNetwork(paymentMethodBankNetwork);
            }

            paymentMethodBankNetworks.Add(new
            {
                PaymentMethodBankNetworkCode = paymentMethodBankNetwork.Id,
                MessageError = Global.DuplicatedRecord
            });

            return Json(paymentMethodBankNetworks, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeletePaymentMethodBankNetwork
        /// Borra tipo de pago de cuerdo a la red del banco
        /// </summary>
        /// <param name="paymentMethodModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeletePaymentMethodBankNetwork(PaymentMethodBankNetworkModel paymentMethodModel, string operationType)
        {
            bool isDeleted = false;
            List<object> paymentMethodResponses = new List<object>();
            PaymentMethodBankNetwork paymentMethodBankNetwork = new PaymentMethodBankNetwork();

            paymentMethodBankNetwork.BankAccountCompany = new BankAccountCompanyDTO()
            { Id = paymentMethodModel.AccountBankId };
            paymentMethodBankNetwork.BankNetwork = new BankNetwork() { Id = paymentMethodModel.Id };
            paymentMethodBankNetwork.Id = 0;
            paymentMethodBankNetwork.PaymentMethod = new PaymentMethodDTO() { Id = paymentMethodModel.PaymentMethodId };
            paymentMethodBankNetwork.ToGenerate = paymentMethodModel.ToGenerate;

            if (operationType.Equals("D"))
            {
                isDeleted = DelegateService.automaticDebitService.DeletePaymentMethodBankNetwork(paymentMethodBankNetwork);
            }

            paymentMethodResponses.Add(new
            {
                PaymentMethodBankNetworkCode = (isDeleted) ? 0 : -1,
                MessageError = "No se puede eliminar el registro, ya que tiene dependencias"
            });

            return Json(paymentMethodResponses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FormatTypes

        /// <summary>
        /// GetFormatTypes
        /// Obtiene tipos de formato
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFormatTypes()
        {
            List<object> formatTypes = new List<object>();

            formatTypes.Add(new { Id = 1, Description = Global.Header.ToUpper() });
            formatTypes.Add(new { Id = 2, Description = Global.Detailed.ToUpper() });
            formatTypes.Add(new { Id = 3, Description = Global.Summary.ToUpper() });

            return new UifSelectResult(formatTypes);
        }

        /// <summary>
        /// GetUseDebitFiles
        /// Obtiene archivo de debito por usuario
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetUseDebitFiles()
        {
            List<object> debitFiles = new List<object>();

            debitFiles.Add(new { Id = 1, Description = Global.Shipping.ToUpper() });
            debitFiles.Add(new { Id = 2, Description = Global.Reception.ToUpper() });
            debitFiles.Add(new { Id = 3, Description = Global.PrenotificationShipping.ToUpper() });
            debitFiles.Add(new { Id = 4, Description = Global.PrenotificationAnswer.ToUpper() });

            return new UifSelectResult(debitFiles);
        }

        /// <summary>
        /// GetFieldAlignment
        /// Obtiene archivo alineado
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFieldAlignment()
        {
            string lng = Request.UserLanguages[0];
            string urlReferrer = Request.UrlReferrer.ToString();

            List<object> fieldAlignments = new List<object>();

            fieldAlignments.Add(new { Id = "D", Description = Global.Right.ToUpper() });
            fieldAlignments.Add(new { Id = "I", Description = Global.Left.ToUpper() });

            return new UifSelectResult(fieldAlignments);
        }

        /// <summary>
        /// GetFieldFormats
        /// Obtiene archivo con formato
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFieldFormats()
        {
            List<object> formatsResponses = new List<object>();
            List<AutomaticDebitFormat> formats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(0).OrderBy(f => f.Format.Description).ToList();

            foreach (AutomaticDebitFormat format in formats)
            {
                formatsResponses.Add(new
                {
                    Id = format.BankNetwork.Description,
                    Description = format.Format.Description
                });
            }

            return new UifSelectResult(formatsResponses);
        }

        #endregion

        #region AccountTypeRelationship

        /// <summary>
        /// GetPaymentMethodAccountTypes
        /// Obtiene metodos de pago de acuerdo a tipos de cuenta
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentMethodAccountTypes()
        {
            List<PaymentMethodAccountType> paymentMethodAccountTypes = DelegateService.automaticDebitService.GetPaymentMethodAccountTypes();

            var PaymentMethodAccountTypeResponse = from items in paymentMethodAccountTypes
                           select new
                           {
                               Id = items.PaymentMethod.Id,
                               Description = items.PaymentMethod.Description,
                               AccountTypeId = items.BankAccountType.Id,
                               AccountTypeDescription = items.BankAccountType.Description,
                               DebitCode = items.SmallDescriptionDebit
                           };

            return new UifTableResult(PaymentMethodAccountTypeResponse);
        }

        /// <summary>
        /// SavePaymentMethodAccountType
        /// Graba tipo de pagos de acuerdo a tipo de cuenta
        /// </summary>
        /// <param name="paymentMethodModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SavePaymentMethodAccountType(PaymentMethodAccountTypeModel paymentMethodModel, string operationType)
        {
            List<object> paymentMethodAccountTypes = new List<object>();
            PaymentMethodAccountType paymentMethodAccountType = new PaymentMethodAccountType();

            paymentMethodAccountType.BankAccountType = new BankAccountTypeDTO() { Id = paymentMethodModel.AccountTypeId };
            paymentMethodAccountType.Id = 0;
            paymentMethodAccountType.PaymentMethod = new PaymentMethodDTO() { Id = paymentMethodModel.Id };
            paymentMethodAccountType.SmallDescriptionDebit = paymentMethodModel.DebitCode;

            if (operationType.Equals("I"))
            {
                paymentMethodAccountType = DelegateService.automaticDebitService.SavePaymentMethodAccountType(paymentMethodAccountType);
            }
            if (operationType.Equals("U"))
            {
                paymentMethodAccountType = DelegateService.automaticDebitService.UpdatePaymentMethodAccountType(paymentMethodAccountType);
            }

            paymentMethodAccountTypes.Add(new
            {
                PaymentMethodAccountTypeCode = paymentMethodAccountType.Id,
                MessageError = paymentMethodAccountType.BankAccountType.Description
            });

            return Json(paymentMethodAccountTypes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeletePaymentMethodAccountType
        /// Borra tipo de pago de acuerdo a tipo de cuenta
        /// </summary>
        /// <param name="paymentMethodModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeletePaymentMethodAccountType(PaymentMethodAccountTypeModel paymentMethodModel, string operationType)
        {
            bool isDeleted = false;
            List<object> paymentMethodAccountTypeResponse = new List<object>();
            PaymentMethodAccountType paymentMethodAccountType = new PaymentMethodAccountType();

            paymentMethodAccountType.BankAccountType = new BankAccountTypeDTO() { Id = paymentMethodModel.AccountTypeId };
            paymentMethodAccountType.Id = 0;
            paymentMethodAccountType.PaymentMethod = new PaymentMethodDTO() { Id = paymentMethodModel.Id };
            paymentMethodAccountType.SmallDescriptionDebit = paymentMethodModel.DebitCode;

            if (operationType.Equals("D"))
            {
                isDeleted = DelegateService.automaticDebitService.DeletePaymentMethodAccountType(paymentMethodAccountType);
            }

            paymentMethodAccountTypeResponse.Add(new
            {
                PaymentMethodAccountTypeCode = (isDeleted) ? 0 : -1,
                MessageError = Global.YouCanNotDeleteTheRecord //"No se puede eliminar el registro, ya que tiene dependencias"
            });

            return Json(paymentMethodAccountTypeResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

	}
}