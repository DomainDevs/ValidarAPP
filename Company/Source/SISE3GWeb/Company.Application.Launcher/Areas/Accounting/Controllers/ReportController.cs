//System
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CurrentAccount;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using PreLiquidationReport = Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.PreLiquidation;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    public class ReportController : Controller
    {
        #region Instance Variables
        readonly CommonController _commonController = new CommonController();
        readonly BaseController _baseController = new BaseController();
        #endregion

        #region Views

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region BillingClousureReport

        /// <summary>
        /// LoadBillControlReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billCode"></param>
        /// <param name="reportId"></param>
        public void LoadBillControlReport(int branchId, int billCode, int reportId)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            List<SEARCH.ReportCollectDTO> reportCollects = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "CollectView", billCode);

            // Trae detalle cobros
            ReportModel paymentModel = new ReportModel();

            List<CollectDetailsModel> collectDetailsModels = new List<CollectDetailsModel>();

            foreach (SEARCH.ReportCollectDTO reportCollect in reportCollects)
            {
                // Subreporte para items cobrados
                CollectDetailsModel collectDetails = new CollectDetailsModel();
                collectDetails.BillItemCode = Convert.ToInt16(reportCollect.CollectItemId);
                collectDetails.Quota = Convert.ToInt16(reportCollect.Quota);
                collectDetails.Endorsement = reportCollect.Endorsement.ToString();
                collectDetails.Amount = Convert.ToDouble(reportCollect.Amount);
                collectDetails.ReceivedAmount = Convert.ToDouble(reportCollect.AmountReceived);
                collectDetails.Policy = reportCollect.Policy.ToString();
                collectDetails.Bill = reportCollect.CollectId.ToString();
                collectDetails.PayerName = reportCollect.NamePayer.ToString();

                collectDetailsModels.Add(collectDetails);
            }

            paymentModel.SumaryCollectReport = collectDetailsModels;

            // Trae detallle pagos
            reportCollects = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "PaymentView", billCode);

            List<PaymentDetailsModel> paymentDetailsModels = new List<PaymentDetailsModel>();
            List<MainReportModel> mainReportModels = new List<MainReportModel>();

            foreach (SEARCH.ReportCollectDTO reportCollect in reportCollects)
            {
                // Subreporte para totales por tipo de pago
                PaymentDetailsModel paymentDetails = new PaymentDetailsModel();

                paymentDetails.Bill = reportCollect.CollectId.ToString();
                paymentDetails.PaymentMethod = reportCollect.PaymentMethod.ToString();
                paymentDetails.PaymentId = Convert.ToInt16(reportCollect.PaymentMethodId);
                paymentDetails.PaymentAmount = Convert.ToDouble(reportCollect.PaymentAmount);
                /*--------------------------------------------------------------*/
                paymentDetails.IncomeAmount = Convert.ToDouble(reportCollect.IncomeAmount);
                paymentDetails.ExchangeRate = Convert.ToDouble(reportCollect.ExchangeRate);
                paymentDetails.Holder = reportCollect.Holder.ToString();
                paymentDetails.DocumentNumber = reportCollect.DocumentNumber.ToString();
                paymentDetails.CurrencyId = Convert.ToInt16(reportCollect.CurrencyId);
                paymentDetails.CurrencyDescription = reportCollect.CurrencyDescription.ToString();

                paymentDetailsModels.Add(paymentDetails);

                // LLena cabecera
                MainReportModel reportModel = new MainReportModel();
                reportModel.User = reportCollect.AccountName.ToString();
                reportModel.Bill = reportCollect.CollectId.ToString();
                reportModel.Status = Convert.ToInt16(reportCollect.Status);  //estado de la caja
                reportModel.DateTransaction = String.Format("{0:dd/MM/yyyy}", reportCollect.DateTransaction);
                reportModel.Branch = reportCollect.BranchDescription.ToString();
                reportModel.BillingTotal = Convert.ToDecimal(reportCollect.TotalCollect);
                /*----------------------------------------------------------------------------*/
                reportModel.BillDescription = reportCollect.CollectDescription.ToString();
                reportModel.CollectConcept = Convert.ToInt16(reportCollect.CollectDescription);
                reportModel.CollectConceptDescription = reportCollect.CollectConceptDescription.ToString();

                // Filtra por el estatus de Factura solo las activas
                if (Convert.ToInt16(reportCollect.CollectStatus) == 1)
                {
                    mainReportModels.Add(reportModel);
                }
            }

            paymentModel.SumaryPaymentReport = paymentDetailsModels;
            paymentModel.SumaryMainReport = mainReportModels;

            TempData["billRptSource"] = paymentModel.SumaryMainReport;
            TempData["billRptSubSourceItems"] = paymentModel.SumaryCollectReport;
            TempData["billRptSubSource"] = paymentModel.SumaryPaymentReport;

            if (reportId == 1)
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports//PaymentCollectReport.rpt";
            }
            else if (reportId == 2)
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports/Billing//BillPaymentCollectReport.rpt";
            }
        }

        #endregion

        #region BillingReport

        /// <summary>
        /// LoadBillingReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billCode"></param>
        /// <param name="reportId"></param>
        public void LoadBillingReport(int branchId, int billCode, int reportId)
        {
            string payerName;
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            if (userId == 0)
            {
                userId = 9;
            }

            List<SEARCH.ReportCollectDTO> reportsCollects = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "CollectingView", billCode);

            // Trae detalle cobros
            ReportModel paymentModel = new ReportModel();
            List<BillingCollectDetailsModel> collectDetailsSumaries = new List<BillingCollectDetailsModel>();
            List<BillingCollectDetailsModel> collectDetailsSumaryReports = new List<BillingCollectDetailsModel>();

            foreach (SEARCH.ReportCollectDTO report in reportsCollects)
            {
                // Subreporte para items cobrados
                BillingCollectDetailsModel collectDetails = new BillingCollectDetailsModel()
                {
                    AccountNumber = report.AccountNumber,
                    Amount = Convert.ToDecimal(report.Amount),
                    CollectCode = Convert.ToInt32(report.CollectId),
                    CurrencyDescription = report.CurrencyDescription,
                    Description = report.Description,
                    DocumentNumber = report.DocumentNumber,
                    ExchangeRate = Convert.ToDecimal(report.ExchangeRate),
                    IncomeAmount = Convert.ToDecimal(report.IncomeAmount),
                    PayerId = Convert.ToInt32(report.PayerId),
                    PayerName = "",
                    PaymentDescription = report.PaymentDescription,
                    Status = Convert.ToInt32(report.Status)
                };

                collectDetailsSumaryReports.Add(collectDetails);

                // Filtra por el cheques
                if (report.PaymentId == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"])
                    || report.PaymentId == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]))
                {
                    collectDetailsSumaries.Add(collectDetails);
                }
            }

            //Trae detallle Pagos
            reportsCollects = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "PaymentView", billCode);

            List<MainReportModel> mainReportsModels = new List<MainReportModel>();

            foreach (SEARCH.ReportCollectDTO report in reportsCollects)
            {
                // LLena Cabecera
                char space = ' ';
                MainReportModel reportModel = new MainReportModel()
                {
                    Bill = report.CollectId.ToString(),
                    BillDescription = report.CollectDescription,
                    BillingTotal = Convert.ToDecimal(report.TotalCollect),
                    BillNumber = Convert.ToString(report.CollectNumber),
                    Branch = report.BranchDescription,
                    CollectConcept = Convert.ToInt16(report.CollectConceptId),
                    CollectConceptDescription = report.CollectConceptDescription,
                    DateTransaction = String.Format("{0:dd/MM/yyyy HH:mm:ss}", report.DateTransaction),
                    PayerName = "",
                    Status = Convert.ToInt16(report.Status),
                    TechnicalTransaction = "",
                    User = report.UserId.ToString(),
                    WaterMark = 0,
                };
                reportModel.DateTransaction = Convert.ToString(reportModel.DateTransaction.Split(space)[0]);

                if (Convert.ToInt32(report.PayerId) > 0)
                {
                    payerName = DelegateService.uniquePersonServiceV1.GetPersonByIndividualId(Convert.ToInt32(report.PayerId)).Name;
                }
                else
                {
                    payerName = "";
                }
                reportModel.PayerName = payerName;


                // Filtra por el estatus de factura solo las activas
                if (Convert.ToInt16(report.CollectStatus) == 1
                    || Convert.ToInt16(report.CollectStatus) == 2 //2 para q imprima cuando se ha ingresado a pantalla de Aplicar y se ha salido sin hacer ninguna accion internamente ya cambia a status 2 en Bill
                    || Convert.ToInt16(report.CollectStatus) == 3) //3 para q imprima desde boton de pagos
                {
                    mainReportsModels.Add(reportModel);
                }
            }

            paymentModel.SumaryMainReport = mainReportsModels;
            paymentModel.SumaryBillingReport = collectDetailsSumaries;
            paymentModel.SumaryBillingCollectReport = collectDetailsSumaryReports;

            TempData["billRptSource"] = paymentModel.SumaryMainReport;
            TempData["billRptSubSource"] = paymentModel.SumaryBillingReport;
            TempData["billRptSubSourceItems"] = paymentModel.SumaryBillingCollectReport;
            TempData["BillingReportName"] = "Areas//Accounting//Reports/Billing/BillingPaymentCollectReport.rpt";
        }

        /// <summary>
        /// LoadBillingReportCopy
        /// Visualiza una copia del reporte en formato pdf 
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billCode"></param>
        /// <param name="reportId"></param>
        /// <param name="waterMark"> </param>
        public void LoadBillingReportCopy(int branchId, int billCode, int reportId, int waterMark, int userId)
        {

            List<SEARCH.ReportCollectDTO> reports = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "CollectingView", billCode);

            // Trae detalle cobros
            ReportModel paymentModel = new ReportModel();
            List<BillingCollectDetailsModel> collectDetailsSumaries = new List<BillingCollectDetailsModel>();
            List<BillingCollectDetailsModel> collectDetailsSumaryReports = new List<BillingCollectDetailsModel>();

            foreach (SEARCH.ReportCollectDTO report in reports)
            {
                // Subreporte para items cobrados
                BillingCollectDetailsModel collectDetailsModel = new BillingCollectDetailsModel()
                {
                    AccountNumber = report.AccountNumber,
                    Amount = Convert.ToDecimal(report.Amount),
                    CollectCode = Convert.ToInt32(report.CollectId),
                    CurrencyDescription = report.CurrencyDescription,
                    Description = report.Description,
                    DocumentNumber = report.DocumentNumber,
                    ExchangeRate = Convert.ToDecimal(report.ExchangeRate),
                    IncomeAmount = Convert.ToDecimal(report.IncomeAmount),
                    PayerId = Convert.ToInt32(report.PayerId),
                    PayerName = "",
                    PaymentDescription = report.PaymentDescription,
                    Status = Convert.ToInt16(report.Status)
                };

                collectDetailsSumaryReports.Add(collectDetailsModel);

                //if (report.PaymentId == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"])
                //|| report.PaymentId == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]))
                //{
                //    collectDetailsSumaries.Add(collectDetailsModel);
                //}
            }

            // Trae detallle Pagos
            reports = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "PaymentView", billCode);

            List<MainReportModel> mainReportsModels = new List<MainReportModel>();

            foreach (SEARCH.ReportCollectDTO report in reports)
            {
                // LLena Cabecera
                MainReportModel reportModel = new MainReportModel()
                {
                    Bill = report.CollectId.ToString(),
                    BillDescription = report.CollectDescription,
                    BillingTotal = Convert.ToDecimal(report.TotalCollect),
                    BillNumber = Convert.ToString(report.CollectNumber),
                    Branch = report.BranchDescription,
                    CollectConcept = Convert.ToInt16(report.CollectConceptId),
                    CollectConceptDescription = report.CollectConceptDescription,
                    DateTransaction = String.Format("{0:dd/MM/yyyy HH:mm:ss}", report.DateTransaction),
                    PayerName = "",
                    PayerId = report.PayerId.ToString(),
                    Status = Convert.ToInt16(report.Status),
                    TechnicalTransaction = report.TechnicalTransaction.ToString(),
                    User = report.UserId.ToString(),
                    WaterMark = waterMark
                };

                if (Convert.ToInt32(report.PayerId) >= 0)
                {
                    {
                        Core.Application.AccountingServices.DTOs.IndividualDTO person = DelegateService.accountingAccountsPayableService.GetIndividualByIndividualId(Convert.ToInt32(report.PayerId));
                        if (person != null)
                        {
                            reportModel.PayerName = person.Name;
                            reportModel.PayerId = person.IdentificationDocument.Number =="" ? "0": person.IdentificationDocument.Number;
                        }
                    }
                }

                // Filtra por el estatus de factura solo las activas
                if (Convert.ToInt16(report.CollectStatus) == 1
                    || Convert.ToInt16(report.CollectStatus) == 2 //2 para q imprima cuando se ha ingresado a pantalla de Aplicar y se ha salido sin hacer ninguna accion internamente ya cambia a status 2 en Bill
                    || Convert.ToInt16(report.CollectStatus) == 3) //3 para q imprima desde boton de pagos)
                {
                    mainReportsModels.Add(reportModel);
                }
            }

            paymentModel.SumaryMainReport = mainReportsModels;
            paymentModel.SumaryBillingReport = collectDetailsSumaries;
            paymentModel.SumaryBillingCollectReport = collectDetailsSumaryReports;

            //SHOW REPORT
            bool isValid = true;

            var reportSource = paymentModel.SumaryMainReport;
            var reportSubSource = paymentModel.SumaryBillingReport;
            var reportSubSourceItems = paymentModel.SumaryBillingCollectReport;
            var reportName = "Areas//Accounting//Reports/Billing/BillingPaymentCollectReport.rpt";

            if (reportSource == null || reportSubSource == null || reportSubSourceItems == null)
            {
                isValid = false;
            }

            if (isValid)
            {   
                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                reportDocument.Load(reportPath);
                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    // Llena reporte principal
                    reportDocument.SetDataSource(reportSource);
                }

                // Llena subreporte 1
                reportDocument.Subreports[0].SetDataSource(reportSubSourceItems);

                // Llena subreporte 2
                //reportDocument.Subreports[1].SetDataSource(reportSubSource);


                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                                                    false, "BillingReport");
            }
        }

        /// <summary>
        /// ShowReceiptReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billCode"></param>
        /// <param name="reportId"></param>
        /// <param name="otherPayerName"></param>
        public void ShowReceiptReport(int branchId, int billCode, int reportId, string otherPayerName)
        {
            try
            {
                string payerName = string.Empty;
                int userId = _commonController.GetUserIdByName(User.Identity.Name);
                DateTime accountingDate;

                if (userId == 0)
                {
                    userId = 9;
                }

                List<SEARCH.ReportCollectDTO> reports = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "CollectingView", billCode);

                // Trae detalle cobros
                ReportModel paymentModel = new ReportModel();
                List<BillingCollectDetailsModel> collectDetails = new List<BillingCollectDetailsModel>();
                List<BillingCollectDetailsModel> billingCollectDetailsModels = new List<BillingCollectDetailsModel>();
                if (reports.Count > 0)
                {
                    foreach (SEARCH.ReportCollectDTO report in reports)
                    {
                        // Subreporte para items cobrados
                        BillingCollectDetailsModel collectDetailsModel = new BillingCollectDetailsModel();

                        collectDetailsModel.CollectCode = Convert.ToInt32(report.CollectId);
                        collectDetailsModel.Description = report.Description;
                        collectDetailsModel.AccountNumber = report.AccountNumber;
                        collectDetailsModel.DocumentNumber = report.DocumentNumber;
                        collectDetailsModel.CurrencyDescription = report.CurrencyDescription;
                        collectDetailsModel.Amount = Convert.ToDecimal(report.Amount);
                        collectDetailsModel.IncomeAmount = Convert.ToDecimal(report.IncomeAmount);
                        collectDetailsModel.ExchangeRate = Convert.ToDecimal(report.ExchangeRate);
                        collectDetailsModel.PaymentDescription = report.PaymentDescription;
                        collectDetailsModel.PayerId = Convert.ToInt32(report.PayerId);
                        collectDetailsModel.Status = Convert.ToInt16(report.Status);
                        accountingDate = report.AccountingDate;
                        billingCollectDetailsModels.Add(collectDetailsModel);

                        // Filtra por el cheques
                        //if (report.PaymentId == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"])
                        //    || report.PaymentId == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]))
                        //{
                        //    collectDetails.Add(collectDetailsModel);
                        //}
                    }
                }

                //Trae detallle Pagos
                reports = DelegateService.accountingPaymentService.GetReportPayment(userId, branchId, "PaymentView", billCode);

                List<MainReportModel> mainReportsModels = new List<MainReportModel>();

                decimal billingTotal = 0;
                MainReportModel reportModel = new MainReportModel();
                reportModel.BillingTotal = billingTotal;
                mainReportsModels.Add(reportModel);

                if (reports.Count > 0)
                {
                    mainReportsModels = new List<MainReportModel>();
                    foreach (SEARCH.ReportCollectDTO report in reports)
                    {
                        // LLena Cabecera                    
                        char space = ' ';

                        reportModel.User = report.UserId.ToString();
                        reportModel.Bill = report.CollectId.ToString();
                        reportModel.Status = Convert.ToInt16(report.Status);
                        reportModel.DateTransaction = String.Format("{0:dd/MM/yyyy HH:mm:ss}", report.AccountingDate);
                        reportModel.DateTransaction = Convert.ToString(reportModel.DateTransaction.Split(space)[0]);
                        reportModel.Branch = report.BranchDescription;
                        billingTotal = billingTotal + report.Amount; //despliega el total en moneda local
                        reportModel.BillingTotal = billingTotal;
                        reportModel.BillNumber = Convert.ToString(report.CollectNumber);
                        /*----------------------------------------------------------------------------*/
                        reportModel.BillDescription = report.CollectDescription;
                        reportModel.CollectConcept = Convert.ToInt16(report.CollectConceptId);
                        reportModel.CollectConceptDescription = report.CollectConceptDescription;
                        reportModel.TechnicalTransaction = report.TechnicalTransaction.ToString();
                        reportModel.PayerId = report.PayerId.ToString();
                        if (Convert.ToInt32(report.PayerId) > 0)
                        {
                            Core.Application.AccountingServices.DTOs.IndividualDTO person = DelegateService.accountingAccountsPayableService.GetIndividualByIndividualId(Convert.ToInt32(report.PayerId));
                            if (person != null)
                            {
                                payerName = person.Name;
                                reportModel.PayerId = person.IdentificationDocument.Number == "" ? "0": person.IdentificationDocument.Number;
                            }
                        }
                        else
                        {
                            payerName = otherPayerName;
                        }

                        reportModel.PayerName = payerName;

                        // Filtra por el estatus de factura solo las activas
                        if (Convert.ToInt16(report.CollectStatus) == 1
                            || Convert.ToInt16(report.CollectStatus) == 2  // Para que imprima cuando se ha ingresado a pantalla de Aplicar y se ha salido sin hacer ninguna accion internamente ya cambia a status 2 en Bill
                            || Convert.ToInt16(report.CollectStatus) == 3) // Para que imprima desde boton de pagos
                        {
                            mainReportsModels.Add(reportModel);
                        }
                    }
                }

                paymentModel.SumaryMainReport = mainReportsModels;
                paymentModel.SumaryBillingReport = collectDetails;
                billingCollectDetailsModels[0].PayerName = reportModel.PayerName;
                paymentModel.SumaryBillingCollectReport = billingCollectDetailsModels;

                var reportSource = paymentModel.SumaryMainReport;
                var reportSubSource = paymentModel.SumaryBillingReport;
                var reportSubSourceItems = paymentModel.SumaryBillingCollectReport;
                var reportName = "Areas//Accounting//Reports/Billing/BillingPaymentCollectReport.rpt";


                ReportDocument reportDocument = new ReportDocument();
                
                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                reportDocument.Load(reportPath);
                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    // Llena reporte principal
                    reportDocument.SetDataSource(reportSource);
                }

                // Llena subreporte 1
                reportDocument.Subreports[0].SetDataSource(reportSubSourceItems);

                // Llena subreporte 2
                //reportDocument.Subreports[1].SetDataSource(reportSubSource);

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                                                    false, "BillingReport");

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region InternalBallotCardReport

        /// <summary>
        /// LoadInternalBallotCardReport
        /// Llena el reporte con los datos de la boleta interna
        /// </summary>
        /// <param name="paymentTicketCode"> </param>
        /// <param name="paymentMethodTypeCode"> </param>
        /// <returns></returns>
        public void LoadInternalBallotCardReport(int paymentTicketCode, int paymentMethodTypeCode)
        {
            try
            {
                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                List<InternalBallotCardDetailsModel> internalBallotCardDetailsModels = new List<InternalBallotCardDetailsModel>();

                // Obtiene el detalle de la boleta interna
                if (paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["Creditable"]))
                {
                    List<SEARCH.InternalBallotCardReportDTO> cards = DelegateService.accountingPaymentTicketService.GetReportInternalBallotCard(userId,
                                                         paymentTicketCode, "PaymentTicketCreditCardView");
                    List<SEARCH.InternalBallotCardReportDTO> reportCardBallots;
                    reportCardBallots = cards;

                    foreach (SEARCH.InternalBallotCardReportDTO reportCard in reportCardBallots)
                    {
                        InternalBallotCardDetailsModel collectDetails = new InternalBallotCardDetailsModel()
                        {
                            Amount = Convert.ToDouble(reportCard.Amount),
                            AuthorizationNumber = reportCard.AuthorizationNumber,
                            BranchCode = reportCard.BranchCode,
                            BranchName = reportCard.BranchName,
                            CardAmount = Convert.ToDouble(reportCard.CardAmount),
                            CardCommissionAmount = Convert.ToDouble(reportCard.CardCommissionAmount),
                            CardDate = reportCard.CardDate,
                            CardNumber = ReplaceWithAsterisks(reportCard.CardNumber),
                            CardTaxAmount = Convert.ToDouble(reportCard.CardTaxAmount),
                            CashAmount = Convert.ToDouble(reportCard.CashAmount),
                            CommissionAmount = Convert.ToDouble(reportCard.CommissionAmount),
                            CreditCardTypeCode = reportCard.CreditCardTypeCode,
                            CreditCardTypeName = reportCard.CreditCardTypeName,
                            CurrencySymbol = reportCard.TinyDescription,
                            ExchangeRate = Convert.ToDouble(reportCard.ExchangeRate),
                            Holder = reportCard.Holder,
                            IncomeAmount = Convert.ToDouble(reportCard.IncomeAmount),
                            IssuingBankCode = reportCard.IssuingBankCode,
                            IssuingBankName = reportCard.IssuingBankName,
                            PaymentMethodTypeCode = reportCard.PaymentMethodTypeCode,
                            PaymentMethodTypeName = reportCard.PaymentMethodTypeName,
                            PaymentTicketCode = reportCard.PaymentTicketCode.ToString(),
                            ReceivingAccountNumber = reportCard.AccountNumber,
                            ReceivingBankCode = reportCard.BankCode,
                            ReceivingBankName = reportCard.BankName,
                            ReceivingCurrencyCode = reportCard.CurrencyCode,
                            ReceivingCurrencyName = reportCard.CurrencyName,
                            RegisterDate = reportCard.RegisterDate.Value,
                            TaxAmount = Convert.ToDouble(reportCard.TaxAmount),
                            TotalBallot = Convert.ToDouble(reportCard.Amount) + Convert.ToDouble(reportCard.CashAmount),
                            TownName = "",
                            UserCode = Convert.ToInt32(reportCard.UserId),
                            UserName = User.Identity.Name,
                            VoucherNumber = reportCard.VoucherNumber
                        };

                        internalBallotCardDetailsModels.Add(collectDetails);
                    }
                }
                else
                {
                    List<SEARCH.InternalBallotCardReportDTO> cards = DelegateService.accountingPaymentTicketService.GetReportInternalBallotCard(userId,
                                                                                                    paymentTicketCode, "PaymentTicketNotCreditCardView");
                    List<SEARCH.InternalBallotCardReportDTO> reportNCardBallots;
                    reportNCardBallots = cards;

                    foreach (SEARCH.InternalBallotCardReportDTO reportNCard in reportNCardBallots)
                    {
                        InternalBallotCardDetailsModel collectDetails = new InternalBallotCardDetailsModel()
                        {
                            Amount = Convert.ToDouble(reportNCard.Amount),
                            AuthorizationNumber = reportNCard.AuthorizationNumber,
                            BranchCode = reportNCard.BranchCode,
                            BranchName = reportNCard.BranchName,
                            CardAmount = Convert.ToDouble(reportNCard.CardAmount),
                            CardCommissionAmount = Convert.ToDouble(reportNCard.CardCommissionAmount),
                            CardDate = reportNCard.CardDate,
                            CardNumber = ReplaceWithAsterisks(reportNCard.CardNumber),
                            CashAmount = Convert.ToDouble(reportNCard.CashAmount),
                            CardTaxAmount = Convert.ToDouble(reportNCard.CardTaxAmount),
                            CommissionAmount = Convert.ToDouble(reportNCard.CommissionAmount),
                            CreditCardTypeCode = reportNCard.CreditCardTypeCode,
                            CreditCardTypeName = reportNCard.CreditCardTypeName,
                            CurrencySymbol = reportNCard.TinyDescription,
                            ExchangeRate = Convert.ToDouble(reportNCard.ExchangeRate),
                            Holder = reportNCard.Holder,
                            IncomeAmount = Convert.ToDouble(reportNCard.IncomeAmount),
                            IssuingBankCode = reportNCard.IssuingBankCode,
                            IssuingBankName = reportNCard.IssuingBankName,
                            PaymentMethodTypeCode = reportNCard.PaymentMethodTypeCode,
                            PaymentMethodTypeName = reportNCard.PaymentMethodTypeName,
                            PaymentTicketCode = reportNCard.PaymentTicketCode.ToString(),
                            ReceivingAccountNumber = reportNCard.AccountNumber,
                            ReceivingBankCode = reportNCard.BankCode,
                            ReceivingBankName = reportNCard.BankName,
                            ReceivingCurrencyCode = reportNCard.CurrencyCode,
                            ReceivingCurrencyName = reportNCard.CurrencyName,
                            RegisterDate = reportNCard.RegisterDate.Value,
                            TaxAmount = Convert.ToDouble(reportNCard.TaxAmount),
                            TotalBallot = Convert.ToDouble(reportNCard.Amount) + Convert.ToDouble(reportNCard.CashAmount),
                            TownName = "",
                            UserCode = Convert.ToInt32(reportNCard.UserId),
                            UserName = User.Identity.Name,
                            VoucherNumber = reportNCard.VoucherNumber
                        };

                        internalBallotCardDetailsModels.Add(collectDetails);
                    }
                }

                var reportSource = internalBallotCardDetailsModels;
                var reportName = "Areas//Accounting//Reports//InternalBallotCardReport.rpt";

                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                reportDocument.Load(reportPath);
                if (reportSource.GetType().ToString() != "System.String")
                {
                    // Llena Reporte Principal
                    reportDocument.SetDataSource(reportSource);
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "InternalBallotingReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region RejectedCheckReport

        /// <summary>
        /// ShowRejectedCheckReport
        /// Función Muestra Reporte de Cheques Rechazados
        /// </summary>
        public void ShowRejectedCheckReport()
        {
            try
            {
                bool isValid = true;
                var reportSource = TempData["billRptSource"];
                var reportName = TempData["BillingReportName"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();
                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                    reportDocument.Load(reportPath);
                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "RejectedCheckReport");
                    TempData["billRptSource"] = null;
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region CardDepositingPendingReport

        /// <summary>
        /// ShowCardsDepositingPendingReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <returns></returns>
        public void ShowCardsDepositingPendingReport()
        {
            try
            {
                bool isValid = true;

                var reportSource = TempData["billRptSource"];
                var reportName = TempData["BillingReportName"];


                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);
                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.PrintOptions.PaperSize = PaperSize.PaperA3;
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "CardDepositingPendingReport");

                    TempData["billRptSource"] = null;
                    TempData["BillingReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadCardsDepositingPendingReport
        /// Llena el reporte con los datos de los cheques pendientes de depositar
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="billCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void LoadCardsDepositingPendingReport(string creditCardTypeCode, string voucher, string documentNumber,
                                                     string billCode, string branchCode, string startDate, string endDate,
                                                     string status)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            string branch = "";

            branch = branchCode == "" ? "-1" : _commonController.GetBranchDescriptionByIdUserId(Convert.ToInt32(branchCode), userId);

            if (branchCode == "")
            {
                branchCode = branch;
                branch = "";
            }

            CreditCardTypeDTO creditCardType = new CreditCardTypeDTO();
            creditCardType.Id = Convert.ToInt32(creditCardTypeCode);

            List<SEARCH.CardVoucherDTO> cardVouchers = DelegateService.accountingPaymentService.GetReportCardsDepositingPending(creditCardTypeCode, voucher, documentNumber, billCode,
                                                             branchCode, startDate, endDate, status);

            List<CardsDepositingPendingDetailsModel> cardsDepositingPendingDetails = new List<CardsDepositingPendingDetailsModel>();

            List<SEARCH.CardVoucherDTO> cardVouchersResults;
            cardVouchersResults = cardVouchers;

            IFormatProvider culture = new CultureInfo("es-EC", true);

            foreach (SEARCH.CardVoucherDTO cardVoucher in cardVouchersResults)
            {
                CardsDepositingPendingDetailsModel cardsDepositingPending = new CardsDepositingPendingDetailsModel();

                // Llena cabecera
                cardsDepositingPending.CreditCardTypeId = Convert.ToInt32(creditCardTypeCode);
                cardsDepositingPending.CreditCardTypeName = DelegateService.accountingParameterService.GetCreditCardType(creditCardType.Id).Description;
                cardsDepositingPending.BranchCode = Convert.ToInt32(branchCode);
                cardsDepositingPending.BranchName = branch;
                cardsDepositingPending.StartDate = startDate;
                cardsDepositingPending.EndDate = endDate;
                cardsDepositingPending.UserCode = userId;
                cardsDepositingPending.UserName = User.Identity.Name;

                // Llena detalles
                cardsDepositingPending.BranchCodeDetail = cardVoucher.BranchCode;
                cardsDepositingPending.BranchNameDetail = cardVoucher.Description;
                cardsDepositingPending.ReceiptNumber = cardVoucher.CollectCode;
                cardsDepositingPending.CreditCardNumber = ReplaceWithAsterisks(cardVoucher.DocumentNumber);
                cardsDepositingPending.VoucherNumber = cardVoucher.Voucher;
                cardsDepositingPending.PaymentDate = cardVoucher.PaymentDate;
                cardsDepositingPending.CardDate = Convert.ToDateTime(cardVoucher.CardDate, culture); // DANC 2018-06-07
                cardsDepositingPending.CurrencyDescription = cardVoucher.CurrencyDescription;
                cardsDepositingPending.IncomeAmount = cardVoucher.IncomeAmount;
                cardsDepositingPending.ExchangeRate = 0;
                cardsDepositingPending.CardAmount = 0;
                cardsDepositingPending.CardTotal = cardVouchersResults.Count;
                cardsDepositingPending.StatusId = cardVoucher.Status;
                cardsDepositingPending.StatusDescription = cardVoucher.StatusDescription;
                cardsDepositingPending.TechnicalTransaction = cardVoucher.TechnicalTransaction.ToString();
                cardsDepositingPendingDetails.Add(cardsDepositingPending);
            }

            TempData["billRptSource"] = cardsDepositingPendingDetails;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//CardDepositingPendingReport.rpt";
        }

        /// <summary>
        /// LoadInternalBallotReport
        /// Llena el reporte con los datos de la boleta interna
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="branchId"></param>
        /// <param name="branchName"></param>
        public void LoadInternalBallotReport(int paymentTicketCode, int paymentMethodTypeCode, int branchId, string branchName)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            List<InternalBallotDetailsModel> internalBallotDetails = new List<InternalBallotDetailsModel>();

            // Obtiene el detalle de la boleta interna
            if (paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]))
            {
                List<SEARCH.InternalBallotReportDTO> cashes =
                    DelegateService.accountingPaymentTicketService.GetReportInternalBallot(userId, paymentTicketCode, "PaymentTicketCashView");

                List<SEARCH.InternalBallotReportDTO> reportCashBallots = cashes;

                foreach (SEARCH.InternalBallotReportDTO reportCash in reportCashBallots)
                {
                    InternalBallotDetailsModel collectDetails = new InternalBallotDetailsModel()
                    {
                        Amount = Convert.ToDouble(reportCash.Amount),
                        BranchCode = branchId,
                        BranchName = branchName,
                        CashAmount = Convert.ToDouble(reportCash.CashAmount),
                        CheckAmount = 0,
                        CheckNumber = "",
                        CurrencySymbol = reportCash.TinyDescription,
                        ExchangeRate = 0,
                        Holder = "",
                        IncomeAmount = 0,
                        IssuingAccountNumber = "",
                        IssuingBankCode = -1,
                        IssuingBankName = "",
                        PaymentTicketCode = reportCash.PaymentTicketCode.ToString(),
                        ReceivingAccountNumber = reportCash.AccountNumber,
                        ReceivingBankCode = reportCash.BankCode,
                        ReceivingBankName = reportCash.BankName,
                        ReceivingCurrencyCode = reportCash.CurrencyCode,
                        ReceivingCurrencyName = reportCash.CurrencyName,
                        RegisterDate = reportCash.RegisterDate,
                        TotalBallot = Convert.ToDouble(reportCash.Amount) + Convert.ToDouble(reportCash.CashAmount),
                        TownName = "",
                        UserCode = Convert.ToInt32(reportCash.UserId),
                        UserName = User.Identity.Name.ToUpper()
                    };

                    internalBallotDetails.Add(collectDetails);
                }
            }
            // Se pone el detalle de cheques corrientes/posfechados
            else if (paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"])
                     || paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"])
                     || paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"]))
            {
                List<SEARCH.InternalBallotReportDTO> checks =
                    DelegateService.accountingPaymentTicketService.GetReportInternalBallot(userId, paymentTicketCode, "PaymentTicketDetailView");

                List<SEARCH.InternalBallotReportDTO> reportCheckBallots;
                reportCheckBallots = checks;

                foreach (SEARCH.InternalBallotReportDTO reportCheck in reportCheckBallots)
                {
                    InternalBallotDetailsModel internalBallotDetailsModel = new InternalBallotDetailsModel()
                    {
                        Amount = Convert.ToDouble(reportCheck.Amount),
                        BranchCode = branchId,
                        BranchName = branchName,
                        CashAmount = Convert.ToDouble(reportCheck.CashAmount),
                        CheckAmount = Convert.ToDouble(reportCheck.CheckAmount),
                        CheckNumber = reportCheck.DocumentNumber,
                        CurrencySymbol = reportCheck.TinyDescription,
                        ExchangeRate = Convert.ToDouble(reportCheck.ExchangeRate),
                        Holder = reportCheck.Holder,
                        IncomeAmount = Convert.ToDouble(reportCheck.IncomeAmount),
                        IssuingAccountNumber = reportCheck.IssuingAccountNumber,
                        IssuingBankCode = reportCheck.IssuingBankCode,
                        IssuingBankName = reportCheck.IssuingBankName,
                        PaymentTicketCode = reportCheck.PaymentTicketCode.ToString(),
                        ReceivingAccountNumber = reportCheck.AccountNumber,
                        ReceivingBankCode = reportCheck.BankCode,
                        ReceivingBankName = reportCheck.BankName,
                        ReceivingCurrencyCode = reportCheck.CurrencyCode,
                        ReceivingCurrencyName = reportCheck.CurrencyName,
                        RegisterDate = reportCheck.RegisterDate,
                        TotalBallot = Convert.ToDouble(reportCheck.Amount) + Convert.ToDouble(reportCheck.CashAmount),
                        TownName = "",
                        UserCode = Convert.ToInt32(reportCheck.UserId),
                        UserName = User.Identity.Name
                    };

                    internalBallotDetails.Add(internalBallotDetailsModel);
                }
            }

            TempData["billRptSource"] = internalBallotDetails;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//InternalBallotReport.rpt";
        }

        /// <summary>
        /// ShowInternalBallotReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowInternalBallotReport()
        {
            try
            {
                bool isValid = true;
                var reportSource = TempData["billRptSource"];
                var reportName = TempData["BillingReportName"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);
                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "InternalBallotingReport");

                    TempData["billRptSource"] = null;
                    TempData["BillingReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region RejectedCheckReport

        /// <summary>
        /// RejectedCheckReport
        /// Función de Carga del Reporte de Cheques Rechazados
        /// </summary>
        /// <param name="arrayIds"></param>
        public void RejectedCheckReport(int[] arrayIds,int[] CollectIds)
        {
            int paymentMethodTypeCode = 0;
            int paymentCode = arrayIds[0];
            var rejectedChecks = new List<int>(arrayIds);
            string currenyName = "";
            string commissionValue = "";
            decimal totalCommision = 0;
            string specifier = "#,#.00#";


            // Se Llena la cabecera del reporte
            List<SEARCH.CheckInformationGridDTO> checkInformationsGrids = DelegateService.accountingPaymentService.GetCheckInformationGrid(paymentCode);
            SEARCH.CheckInformationGridDTO checkInformation;

            RejectedCheckReportModel reportRejectedCheckModel = new RejectedCheckReportModel();

            if (checkInformationsGrids.Count > 0)
            {
                checkInformation = checkInformationsGrids[0];

                if (checkInformationsGrids.Count > -1)
                {
                    BankDTO issuingBank = new BankDTO();

                    string issuingBankDescription = "";
                    issuingBank.Id = -1;
                    if (checkInformation.IssuingBankCode.ToString() != "-1")
                    {
                        issuingBankDescription = _commonController.GetBankById(checkInformation.IssuingBankCode).Description;
                    }

                    int index = checkInformation.DatePayment.IndexOf(" ");
                    string datePayment = checkInformation.DatePayment.Substring(0, index);

                    BankDTO receivingBank = new BankDTO();
                    receivingBank.Id = -1;
                    string receivingBankDescription = "";
                    if (checkInformation.ReceivingBankCode.ToString() != "-1")
                    {
                        receivingBankDescription = _commonController.GetBankById(checkInformation.ReceivingBankCode).Description;
                    }

                    Individual individual = new Individual();
                    individual.IndividualId = -1;
                    string individualDescription = "";
                    if (checkInformation.PayerId.ToString() != "" && checkInformation.PayerId != 0)
                    {
                        individual.IndividualId = checkInformation.PayerId;
                        Core.Application.AccountingServices.DTOs.IndividualDTO person = DelegateService.accountingAccountsPayableService.GetIndividualByIndividualId(Convert.ToInt32(individual.IndividualId));
                        if (person != null)
                        {
                            individualDescription = person.Name;
                        }
                    }

                    reportRejectedCheckModel.CheckNumber = checkInformation.DocumentNumber;  //DocumentNumber /CardNumber
                    reportRejectedCheckModel.DateCheck = datePayment;
                    reportRejectedCheckModel.Id = checkInformation.PaymentCode.ToString();
                    reportRejectedCheckModel.IssuerBank = issuingBankDescription;
                    reportRejectedCheckModel.ReceiverBank = receivingBankDescription;
                    reportRejectedCheckModel.Payer = individualDescription;
                    reportRejectedCheckModel.Place = checkInformation.BranchDescription;
                    reportRejectedCheckModel.ReceiverCheck = checkInformation.Holder;
                    reportRejectedCheckModel.Date = DateTime.Now.ToString("dd/MM/yyyy");
                    reportRejectedCheckModel.Currency = checkInformation.CurrencyDescription;
                    currenyName = checkInformation.CurrencyDescription;
                    reportRejectedCheckModel.Commission = ((decimal)checkInformation.Comission + (decimal)checkInformation.TaxComission).ToString();
                    totalCommision = (decimal)checkInformation.Comission + (decimal)checkInformation.TaxComission;

                    commissionValue = totalCommision.ToString(specifier);
                    reportRejectedCheckModel.Motive = checkInformation.RejectionDescription;
                    reportRejectedCheckModel.Amount = Convert.ToDecimal(checkInformation.Amount);
                    reportRejectedCheckModel.CreditCardDescription = checkInformation.CreditCardDescription;
                    reportRejectedCheckModel.CreditCardTypeCode = checkInformation.CreditCardTypeCode;
                    reportRejectedCheckModel.Voucher = checkInformation.Voucher;
                    paymentMethodTypeCode = checkInformation.PaymentMethodTypeCode;

                    if (paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                        paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"])
                        )
                    {
                        reportRejectedCheckModel.CheckNumber = ReplaceWithAsterisks(reportRejectedCheckModel.CheckNumber);
                    }
                }
            }

            List<RejectedCheckReportModel> rejectedCheckReportModels = new List<RejectedCheckReportModel>();

            if (arrayIds.Length <= 0)
            {
                rejectedCheckReportModels.Add(reportRejectedCheckModel);
            }
            else
            {
                // Se Llena la grilla del reporte
                for (int i = 0; i < arrayIds.Length; i++)
                {
                    if (i > 0)
                    {
                        reportRejectedCheckModel = new RejectedCheckReportModel();
                    }
                    
                    List<SEARCH.CollectItemPolicyDTO> collectItemPolicies = DelegateService.accountingCollectService.GetPoliciesByCollectId(Convert.ToInt32(CollectIds[i])).OrderBy(o => o.QuoteNum).ToList();
                    SEARCH.CollectItemPolicyDTO policy;
                    if (collectItemPolicies != null && collectItemPolicies.Count > 0)
                    {
                        policy = collectItemPolicies[0];
                        reportRejectedCheckModel.Branch = policy.BranchDescription;
                        reportRejectedCheckModel.Prefix = policy.PrefixDescription;
                        reportRejectedCheckModel.Amount = Convert.ToDecimal(policy.Amount);
                        reportRejectedCheckModel.Policy = policy.PolicyId.ToString();
                        reportRejectedCheckModel.Quote = policy.QuoteNum.ToString();
                        reportRejectedCheckModel.Endorsement = policy.Endorsement.ToString();
                    }
                    reportRejectedCheckModel.Currency = currenyName;
                    reportRejectedCheckModel.Commission = commissionValue;


                    rejectedCheckReportModels.Add(reportRejectedCheckModel);
                }
            }

            if (paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                paymentMethodTypeCode == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"])
                )
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports/VoucherRejection/RejectedVoucherReport.rpt";
            }
            else
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports/CheckingRejection/RejectedCheckReport.rpt";
            }

            TempData["billRptSource"] = rejectedCheckReportModels;
        }

        #endregion

        #region OtherPaymentsRequestReport

        /// <summary>
        /// LoadOtherPaymentsRequestReport
        /// Llena el reporte con los datos de la solicitud de pago
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public void LoadOtherPaymentsRequestReport(int paymentRequestId)
        {
            List<OtherPaymentsRequestReportModel> summariesReports = new List<OtherPaymentsRequestReportModel>();

            SEARCH.OtherPaymentsRequestReportHeaderDTO paymentsRequest = DelegateService.accountingAccountsPayableService.OtherPaymentRequestReport(paymentRequestId);

            // Llena Resumen de Movimientos
            foreach (SEARCH.OtherPaymentRequestReportDetails otherPaymentdetails in paymentsRequest.OtherPaymentRequestReportDetails)
            {
                OtherPaymentsRequestReportModel summaryMovement = new OtherPaymentsRequestReportModel()
                {
                    BillId = paymentsRequest.CollectId,
                    CurrencyDescription = paymentsRequest.CurrencyDescription,
                    CurrencyId = paymentsRequest.CurrencyId,
                    DocumentNumber = paymentsRequest.DocumentNumber,
                    EstimatedDate = paymentsRequest.EstimatedDate,
                    IndividualId = paymentsRequest.IndividualId,
                    Name = paymentsRequest.Name,
                    Number = paymentsRequest.Number,
                    PaymentMethodDescription = paymentsRequest.PaymentMethodDescription,
                    PaymentMethodId = paymentsRequest.PaymentMethodId,
                    PaymentRequestDescription = paymentsRequest.PaymentRequestDescription,
                    PaymentRequestId = paymentsRequest.PaymentRequestId,
                    PersonTypeDescription = paymentsRequest.PersonTypeDescription,
                    PersonTypeId = paymentsRequest.PersonTypeId,
                    RegistrationDate = paymentsRequest.RegistrationDate,
                    Retentions = otherPaymentdetails.Retentions,
                    Taxes = otherPaymentdetails.Taxes,
                    TotalAmount = otherPaymentdetails.TotalAmount,
                    TotalAmountHeader = paymentsRequest.TotalAmount,
                    UserAccountName = paymentsRequest.UserAccountName,
                    UserId = paymentsRequest.UserId,
                    VoucherNumber = otherPaymentdetails.VoucherNumber,
                    VoucherTypeDescription = otherPaymentdetails.VoucherTypeDescription,
                    VoucherTypeId = otherPaymentdetails.VoucherTypeId
                };

                summariesReports.Add(summaryMovement);
            }

            var reportSource = summariesReports;
            var reportName = "Areas//Accounting//Reports//OtherPaymentsRequest//OtherPaymentsRequest.rpt";

            ReportDocument reportDocument = new ReportDocument();

            string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

            reportDocument.Load(reportPath);

            // Llena Reporte Principal
            reportDocument.SetDataSource(reportSource);

            reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "OtherPaymentsRequest");
        }

        #endregion

        #region ChecksDepositingPendingReport

        /// <summary>
        /// ShowChecksDepositingPendingReport
        /// Llena el reporte con los datos de los cheques pendientes de depositar
        /// </summary>
        /// <param name="issuingBankCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        public void ShowChecksDepositingPendingReport(int issuingBankCode, string startDate,
                                                      string endDate, int branchCode)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            string branch;

            branch = branchCode == -1 ? "" : _commonController.GetBranchDescriptionByIdUserId(Convert.ToInt32(branchCode), userId);

            List<SEARCH.CheckInformationDTO> checks = DelegateService.accountingPaymentService.GetReportChecksDepositingPending(userId, issuingBankCode, startDate, endDate, branchCode);

            List<ChecksDepositingPendingDetailsModel> checksDepositingPendingDetailsReports = new List<ChecksDepositingPendingDetailsModel>();

            List<SEARCH.CheckInformationDTO> reportCheckPendings;
            reportCheckPendings = checks;

            foreach (SEARCH.CheckInformationDTO reportCheck in reportCheckPendings)
            {
                ChecksDepositingPendingDetailsModel checksDepositingPendingDetailsModel = new ChecksDepositingPendingDetailsModel();

                // Llena cabecera
                checksDepositingPendingDetailsModel.IssuingBankCode = reportCheck.IssuingBankCode;
                checksDepositingPendingDetailsModel.IssuingBankName = reportCheck.BankDescription;
                checksDepositingPendingDetailsModel.BranchCode = branchCode;
                checksDepositingPendingDetailsModel.BranchName = branch;
                checksDepositingPendingDetailsModel.StartDate = startDate;
                checksDepositingPendingDetailsModel.EndDate = endDate;
                checksDepositingPendingDetailsModel.UserCode = userId;
                checksDepositingPendingDetailsModel.UserName = User.Identity.Name.ToUpper();

                // Llena detalles
                checksDepositingPendingDetailsModel.BranchCodeDetail = reportCheck.BranchCode;
                checksDepositingPendingDetailsModel.BranchNameDetail = reportCheck.BranchDescription;
                checksDepositingPendingDetailsModel.ReceiptNumber = reportCheck.CollectCode;
                checksDepositingPendingDetailsModel.IssuingAccountNumber = reportCheck.ReceivingAccountNumber;
                checksDepositingPendingDetailsModel.CheckNumber = reportCheck.DocumentNumber;
                checksDepositingPendingDetailsModel.CheckDate = Convert.ToDateTime(reportCheck.DatePayment);
                checksDepositingPendingDetailsModel.CurrencySymbol = reportCheck.CurrencyDescription;
                checksDepositingPendingDetailsModel.IncomeAmount = Convert.ToDecimal(reportCheck.Amount);
                checksDepositingPendingDetailsModel.ExchangeRate = Convert.ToDecimal(reportCheck.ExchangeRate);
                checksDepositingPendingDetailsModel.CheckAmount = Convert.ToDecimal(reportCheck.Amount);
                checksDepositingPendingDetailsModel.CheckTotal = reportCheckPendings.Count;
                checksDepositingPendingDetailsModel.StatusDescription = reportCheck.StatusDescription;
                checksDepositingPendingDetailsModel.TechnicalTransaction = reportCheck.TechnicalTransaction;
                checksDepositingPendingDetailsReports.Add(checksDepositingPendingDetailsModel);
            }

            string reportName = "Areas//Accounting//Reports//NewChecksDepositingPendingReport.rpt";
            ReportDocument reportDocument = new ReportDocument();

            string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

            reportDocument.Load(reportPath);
            if (checksDepositingPendingDetailsReports.GetType().ToString() != "System.String")
            {
                // Llena Reporte Principal
                reportDocument.SetDataSource(checksDepositingPendingDetailsReports);
            }

            reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "NewChecksDepositingPendingReport");
        }

        /// <summary>
        /// LoadRePrintPaymentOrdersReport
        /// Llena el reporte con los datos de la orden de pago
        /// </summary>
        /// <param name="paymentOrdersMovementsModel"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>int</returns>
        public int LoadRePrintPaymentOrdersReport(PaymentOrdersMovementsModel paymentOrdersMovementsModel, int tempImputationId)
        {
            List<Models.Reports.PaymentOrders.PaymentOrdersReportModel> summaries = new List<Models.Reports.PaymentOrders.PaymentOrdersReportModel>();

            decimal amount = 0;
            ApplicationDTO imputation = new ApplicationDTO()
            {
                RegisterDate = DateTime.Now,
                Id = tempImputationId,
                UserId = _commonController.GetUserIdByName(User.Identity.Name)
            };

            if (tempImputationId == 0)
            {
                imputation = DelegateService.accountingApplicationService.GetTempApplicationBySourceCode(Convert.ToInt32(ApplicationTypes.PaymentOrder),
                                                                              Convert.ToInt32(paymentOrdersMovementsModel.PaymentOrderNumber));
                tempImputationId = imputation.Id;
            }

            imputation = DelegateService.accountingApplicationService.GetDebitsAndCreditsMovementTypes(imputation, Convert.ToDecimal(amount));

            string[] movementType = new string[10];
            movementType[0] = "Primas por Cobrar";
            movementType[1] = "Primas en Depósito";
            movementType[2] = "Comisiones Descontadas";
            movementType[3] = "Cta. Cte. Agentes";
            movementType[4] = "Cta. Cte. Coaseguros";
            movementType[5] = "Cta. Cte. Reaseguros";
            movementType[6] = "Contabilidad";
            movementType[7] = "Solicitud Pago Varios";
            movementType[8] = "Solicitud Pago Siniestros";
            movementType[9] = "Préstamos";

            // Llena Resumen de Movimientos
            for (int i = 0; i < imputation.ApplicationItems.Count; i++)
            {
                decimal debits = 0;
                decimal credits = 0;

                Models.Reports.PaymentOrders.PaymentOrdersReportModel summaryMovement = new Models.Reports.PaymentOrders.PaymentOrdersReportModel()
                {
                    BeneficiaryDocNumber = paymentOrdersMovementsModel.BeneficiaryDocNumber,
                    BeneficiaryName = paymentOrdersMovementsModel.BeneficiaryName,
                    BranchId = paymentOrdersMovementsModel.BranchId,
                    BranchName = paymentOrdersMovementsModel.BranchName,
                    CompanyId = paymentOrdersMovementsModel.CompanyId,
                    CompanyName = paymentOrdersMovementsModel.CompanyName,
                    CurrencyId = paymentOrdersMovementsModel.CurrencyId,
                    CurrencyName = paymentOrdersMovementsModel.CurrencyName,
                    Id = paymentOrdersMovementsModel.Id,
                    PaymentAmount = paymentOrdersMovementsModel.PaymentAmount,
                    PaymentBranchId = paymentOrdersMovementsModel.PaymentBranchId,
                    PaymentBranchName = paymentOrdersMovementsModel.PaymentBranchName,
                    PaymentDate = paymentOrdersMovementsModel.PaymentDate,
                    PaymentOrderNumber = paymentOrdersMovementsModel.PaymentOrderNumber,
                    PaymentTypeId = paymentOrdersMovementsModel.PaymentTypeId,
                    PaymentTypeName = paymentOrdersMovementsModel.PaymentTypeName,
                    PayToName = paymentOrdersMovementsModel.PayToName,
                    UserId = _commonController.GetUserIdByName(User.Identity.Name),
                    UserName = User.Identity.Name.ToUpper()
                };

                if ((imputation.ApplicationItems[i] != null) && (imputation.ApplicationItems[i].TotalDebit != null))
                {
                    debits = System.Math.Abs(Convert.ToDecimal(imputation.ApplicationItems[i].TotalDebit.Value));
                }

                if ((imputation.ApplicationItems[i] != null) && (imputation.ApplicationItems[i].TotalCredit != null))
                {
                    credits = System.Math.Abs(Convert.ToDecimal(imputation.ApplicationItems[i].TotalCredit.Value));
                }

                summaryMovement.DescriptionMovementSumary = movementType[i];
                summaryMovement.Debit = Convert.ToDouble(debits);
                summaryMovement.Credit = Convert.ToDouble(credits);

                summaries.Add(summaryMovement);
            }

            TempData["billRptSource"] = summaries;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//PaymentOrders//PaymentOrdersReport.rpt";

            return summaries[0].Id;

        }

        #endregion

        #region CommissionBalance

        /// <summary>
        /// ShowCommissionBalanceReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="companyId"></param>
        /// <param name="agentId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="type"></param>
        /// <returns>int</returns>
        public int ShowCommissionBalanceReport(int branchId, int companyId, int agentId, string startDate,
                                               string endDate, int type)
        {
            int result = 0;

            // lista de modelos con los que voy a llenar el reporte.
            List<CommissionBalanceModel> commissionBalanceModels = new List<CommissionBalanceModel>();

            DateTime dateStart = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", startDate) + " 00:00:00");
            DateTime dateEnd = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", endDate) + " 23:59:59");

            List<SEARCH.AgentCommissionBalanceItemDTO> agentCommissionBalanceItems =
                DelegateService.accountingApplicationService.GetCommissionBalance(branchId, companyId, agentId,
                                                       dateStart, dateEnd).OrderBy(o => o.BrokerCheckingAccountId).ToList();

            foreach (SEARCH.AgentCommissionBalanceItemDTO commissionBalance in agentCommissionBalanceItems)
            {
                // Subreporte para items cobrados
                CommissionBalanceModel commissionBalanceItem = new CommissionBalanceModel();

                // Cabecera
                commissionBalanceItem.AgentId = commissionBalance.AgentId;
                commissionBalanceItem.AgentTypeCode = commissionBalance.AgentTypeCode;
                commissionBalanceItem.AgentTypeDescription = commissionBalance.AgentTypeDescription;
                commissionBalanceItem.AgentName = commissionBalance.AgentName;
                commissionBalanceItem.DocumentNumAgent = commissionBalance.DocumentNumAgent;
                commissionBalanceItem.StartDate = Convert.ToDateTime(startDate);
                commissionBalanceItem.EndDate = Convert.ToDateTime(endDate);
                commissionBalanceItem.RegisterDate = Convert.ToDateTime(null);
                commissionBalanceItem.UserId = commissionBalance.UserId;
                commissionBalanceItem.BranchCode = commissionBalance.BranchCode;
                commissionBalanceItem.Branch = commissionBalance.Branch;

                // Detalle
                commissionBalanceItem.InsuredId = commissionBalance.InsuredId;
                commissionBalanceItem.DocumentNumInsured = commissionBalance.DocumentNumInsured;
                commissionBalanceItem.InsuredName = commissionBalance.InsuredName;
                commissionBalanceItem.PrefixId = commissionBalance.PrefixId;
                commissionBalanceItem.PrefixDescription = commissionBalance.PrefixDescription;
                commissionBalanceItem.LineBusinessCode = commissionBalance.LineBusinessCode;
                commissionBalanceItem.LineBusinessDescription = commissionBalance.LineBusinessDescription;
                commissionBalanceItem.PolicyId = commissionBalance.PolicyId;
                commissionBalanceItem.DocumentNumPolicy = commissionBalance.DocumentNumPolicy;
                commissionBalanceItem.EndorsementId = commissionBalance.EndorsementId;
                commissionBalanceItem.DocumentNumEndorsement = commissionBalance.DocumentNumEndorsement;
                commissionBalanceItem.CommissionTypeCode = commissionBalance.CommissionTypeCode;
                commissionBalanceItem.CommissionPercentage = commissionBalance.CommissionPercentage;
                commissionBalanceItem.CommissionAmount = commissionBalance.CommissionAmount;
                commissionBalanceItem.CommissionDiscounted = commissionBalance.CommissionDiscounted;
                commissionBalanceItem.CommissionTax = commissionBalance.CommissionTax;
                commissionBalanceItem.CommissionRetention = commissionBalance.CommissionRetention;
                commissionBalanceItem.CommissionBalance = commissionBalance.CommissionBalance;
                commissionBalanceItem.ParticipationPercentage = commissionBalance.ParticipationPercentage;
                commissionBalanceItem.BrokerCheckingAccountId = commissionBalance.BrokerCheckingAccountId;
                commissionBalanceItem.BrokerCheckingAccountDescription = commissionBalance.BrokerCheckingAccountDescription;
                commissionBalanceItem.AccountingNature = commissionBalance.AccountingNature;
                commissionBalanceItem.AccountingNatureDescription = commissionBalance.AccountingNature == Convert.ToInt32(AccountingNature.Credit) ? @Global.Credit : @Global.Debit;
                commissionBalanceItem.CurrencyId = commissionBalance.CurrencyId;
                commissionBalanceItem.CurrencyDescription = commissionBalance.CurrencyDescription;
                commissionBalanceItem.IncomeAmount = commissionBalance.IncomeAmount;
                commissionBalanceItem.Amount = commissionBalance.Amount;

                if (commissionBalance.CommissionPct > 0)
                {
                    commissionBalanceItem.CommissionTypeDescription = "NORMAL";
                }
                else if (commissionBalance.AdditCommissionPct > 0)
                {
                    commissionBalanceItem.CommissionTypeDescription = "EXTRA";
                }
                else
                {
                    commissionBalanceItem.CommissionTypeDescription = "";
                }

                commissionBalanceModels.Add(commissionBalanceItem);
            }

            result = commissionBalanceModels.Count;
            var reportName = "";

            if (type == 1) // Resumen
            {
                reportName = "Areas//Accounting//Reports//CurrentAccount//CommissionBalanceSummary.rpt";
            }
            else if (type == 2) // Detallado
            {
                reportName = "Areas//Accounting//Reports//CurrentAccount//CommissionBalanceDetailed.rpt";
            }

            if (result > 0)
            {
                bool isValid = true;

                var reportSource = commissionBalanceModels;

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                    reportDocument.Load(reportPath);
                    if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "CommssionBalanceRpt");
                }
            }

            return result;
        }

        /// <summary>
        /// ShowReportCommissionBalance
        /// </summary>
        /// <returns></returns> 
        public void ShowReportCommissionBalance()
        {
            try
            {
                bool isValid = true;

                var reportSource = TempData["CommissionBalanceReportSource"];
                var reportName = TempData["CommissionBalanceReportName"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                    reportDocument.Load(reportPath);
                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "CommssionBalanceRpt");

                    // Clear all temp data
                    TempData["CommissionBalanceReportSource"] = null;
                    TempData["CommissionBalanceReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadCommissionBalanceReport
        /// </summary>
        /// <param name="agentCommissionBalance"></param>
        /// <param name="type"></param>
        /// <returns>int</returns>
        public int LoadCommissionBalanceReport(AgentCommissionBalanceModel agentCommissionBalance, int type)
        {
            int result = 0;

            // lista de modelos con los que voy a llenar el reporte.
            List<CommissionBalanceModel> commissionBalanceModels = new List<CommissionBalanceModel>();

            DateTime dateStart = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", agentCommissionBalance.StartDate) + " 00:00:00");
            DateTime dateEnd = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", agentCommissionBalance.EndDate) + " 23:59:59");

            List<SEARCH.AgentCommissionBalanceItemDTO> agentCommissionBalanceItems =
            DelegateService.accountingApplicationService.GetCommissionBalance(agentCommissionBalance.BranchCode, agentCommissionBalance.CompanyCode,
                                                   agentCommissionBalance.AgentId, dateStart,
                                                   dateEnd).OrderBy(o => o.BrokerCheckingAccountId).ToList();

            foreach (SEARCH.AgentCommissionBalanceItemDTO commissionBalance in agentCommissionBalanceItems)
            {
                // Subreporte para items cobrados
                CommissionBalanceModel commissionBalanceItem = new CommissionBalanceModel();

                commissionBalanceItem.AgentId = commissionBalance.AgentId;
                commissionBalanceItem.AgentTypeCode = commissionBalance.AgentTypeCode;
                commissionBalanceItem.AgentTypeDescription = commissionBalance.AgentTypeDescription;
                commissionBalanceItem.AgentName = commissionBalance.AgentName;
                commissionBalanceItem.DocumentNumAgent = commissionBalance.DocumentNumAgent;
                commissionBalanceItem.StartDate = agentCommissionBalance.StartDate;
                commissionBalanceItem.EndDate = agentCommissionBalance.EndDate;
                commissionBalanceItem.RegisterDate = agentCommissionBalance.RegisterDate;
                commissionBalanceItem.UserId = commissionBalance.UserId;
                commissionBalanceItem.BranchCode = commissionBalance.BranchCode;
                commissionBalanceItem.Branch = commissionBalance.Branch;

                // Detalle
                commissionBalanceItem.InsuredId = commissionBalance.InsuredId;
                commissionBalanceItem.DocumentNumInsured = commissionBalance.DocumentNumInsured;
                commissionBalanceItem.InsuredName = commissionBalance.InsuredName;
                commissionBalanceItem.PrefixId = commissionBalance.PrefixId;
                commissionBalanceItem.PrefixDescription = commissionBalance.PrefixDescription;
                commissionBalanceItem.LineBusinessCode = commissionBalance.LineBusinessCode;
                commissionBalanceItem.LineBusinessDescription = commissionBalance.LineBusinessDescription;
                commissionBalanceItem.PolicyId = commissionBalance.PolicyId;
                commissionBalanceItem.DocumentNumPolicy = commissionBalance.DocumentNumPolicy;
                commissionBalanceItem.EndorsementId = commissionBalance.EndorsementId;
                commissionBalanceItem.DocumentNumEndorsement = commissionBalance.DocumentNumEndorsement;
                commissionBalanceItem.CommissionTypeCode = commissionBalance.CommissionTypeCode;
                commissionBalanceItem.CommissionPercentage = commissionBalance.CommissionPercentage;
                commissionBalanceItem.CommissionAmount = commissionBalance.CommissionAmount;
                commissionBalanceItem.CommissionDiscounted = commissionBalance.CommissionDiscounted;
                commissionBalanceItem.CommissionTax = commissionBalance.CommissionTax;
                commissionBalanceItem.CommissionRetention = commissionBalance.CommissionRetention;
                commissionBalanceItem.CommissionBalance = commissionBalance.CommissionBalance;
                commissionBalanceItem.ParticipationPercentage = commissionBalance.ParticipationPercentage;
                commissionBalanceItem.BrokerCheckingAccountId = commissionBalance.BrokerCheckingAccountId;
                commissionBalanceItem.BrokerCheckingAccountDescription = commissionBalance.BrokerCheckingAccountDescription;
                commissionBalanceItem.AccountingNature = commissionBalance.AccountingNature;
                commissionBalanceItem.AccountingNatureDescription = commissionBalance.AccountingNature == Convert.ToInt32(AccountingNature.Credit) ? @Global.Credit : @Global.Debit;
                commissionBalanceItem.CurrencyId = commissionBalance.CurrencyId;
                commissionBalanceItem.CurrencyDescription = commissionBalance.CurrencyDescription;
                commissionBalanceItem.IncomeAmount = commissionBalance.IncomeAmount;
                commissionBalanceItem.Amount = commissionBalance.Amount;

                if (commissionBalance.CommissionPct > 0)
                {
                    commissionBalanceItem.CommissionTypeDescription = "NORMAL";
                }
                else if (commissionBalance.AdditCommissionPct > 0)
                {
                    commissionBalanceItem.CommissionTypeDescription = "EXTRA";
                }
                else
                {
                    commissionBalanceItem.CommissionTypeDescription = "";
                }

                commissionBalanceModels.Add(commissionBalanceItem);
            }

            result = commissionBalanceModels.Count;

            TempData["CommissionBalanceReportSource"] = commissionBalanceModels;

            if (type == 1) // Resumen
            {
                TempData["CommissionBalanceReportName"] = "Areas//Accounting//Reports//CurrentAccount//CommissionBalanceSummary.rpt";
            }
            else if (type == 2) // Detallado
            {
                TempData["CommissionBalanceReportName"] = "Areas//Accounting//Reports//CurrentAccount//CommissionBalanceDetailed.rpt";
            }

            return result;
        }

        #endregion

        #region CommissionPaymentOrders

        /// <summary>
        /// ShowPaymentOrdersReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <returns></returns>
        public void ShowPaymentOrdersCommissionReport()
        {
            try
            {
                bool isValid = true;
                var reportSource = TempData["ReportSourceCPO"];
                var reportName = TempData["ReportNameCPO"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "PaymentOrdersReport");

                    TempData["ReportSourceCPO"] = null;
                    TempData["ReportNameCPO"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadPaymentOrdersCommissionReport
        /// Llena el reporte con los datos de la orden de pago comisiones
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="branchName"></param>
        /// <param name="companyId"></param>
        /// <param name="companyName"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="agentId"></param>
        /// <param name="paymentOrderModelList"></param>
        /// <returns>int</returns>
        public int LoadPaymentOrdersCommissionReport(int branchId, string branchName, int companyId,
                                                     string companyName, DateTime estimatedPaymentDate,
                                                     int agentId, PaymentOrderModelList paymentOrderModelList)
        {
            List<int> paymentOrders = new List<int>();

            if (paymentOrderModelList.PaymentOrderItem.Count > 0)
            {
                foreach (PaymentOrderModel paymentOrderModelItem in paymentOrderModelList.PaymentOrderItem)
                {
                    paymentOrders.Add(paymentOrderModelItem.PaymentOrderItemId);
                }
            }

            List<PaymentOrdersGenerateModel> paymentOrdersCommissions = new List<PaymentOrdersGenerateModel>();

            List<SEARCH.PaymentOrdersCommissionDTO> paymentOrdersCommissionsResults = DelegateService.accountingApplicationService.GetPaymentOrdersCommission(paymentOrders);

            // Llena las órdenes de pago comisiones
            foreach (SEARCH.PaymentOrdersCommissionDTO commission in paymentOrdersCommissionsResults)
            {
                if (commission.IncomeAmount > 0)
                {
                    PaymentOrdersGenerateModel paymentOrder = new PaymentOrdersGenerateModel()
                    {
                        AgentDocNumberName = commission.AgentDocNumberName,
                        AgentDocumentNumber = commission.AgentDocumentNumber,
                        AgentName = commission.AgentName,
                        Amount = commission.Amount,
                        BranchId = commission.BranchId,
                        BranchName = branchName,
                        CompanyId = commission.CompanyId,
                        CompanyName = companyName,
                        CurrencyId = commission.CurrencyId,
                        CurrencyName = commission.CurrencyName,
                        EstimatedDatePayment = commission.EstimatedDatePayment,
                        ExchangeRate = commission.ExchangeRate,
                        Id = commission.Id,
                        IncomeAmount = commission.IncomeAmount,
                        PaymentOrderNumber = commission.PaymentOrderNumber,
                        UserId = _commonController.GetUserIdByName(User.Identity.Name),
                        UserName = User.Identity.Name.ToUpper()
                    };

                    paymentOrdersCommissions.Add(paymentOrder);
                }
            }

            TempData["ReportSourceCPO"] = paymentOrdersCommissions;
            TempData["ReportNameCPO"] = "Areas//Accounting//Reports//CurrentAccount//PaymentOrdersCommissionReport.rpt";

            return paymentOrdersCommissions[0].Id;
        }

        #endregion

        #region PaymentOrdersReport

        /// <summary>
        /// ShowPaymentOrdersReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <returns></returns>
        public void ShowPaymentOrdersReport()
        {
            try
            {
                bool isValid = true;

                var reportSource = TempData["billRptSource"];
                var reportName = TempData["BillingReportName"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "PaymentOrdersReport");

                    TempData["billRptSource"] = null;
                    TempData["BillingReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadPaymentOrdersReport
        /// Llena el reporte con los datos de la orden de pago
        /// </summary>
        /// <param name="paymentOrdersMovementsModel"></param>
        /// <returns>int</returns>
        public int LoadPaymentOrdersReport(PaymentOrdersMovementsModel paymentOrdersMovementsModel)
        {
            List<Models.Reports.PaymentOrders.PaymentOrdersReportModel> summariesReports = new List<Models.Reports.PaymentOrders.PaymentOrdersReportModel>();

            int i = 0;
            string[] movementType = new string[10];
            movementType[0] = "Primas por Cobrar";
            movementType[1] = "Primas en Depósito";
            movementType[2] = "Comisiones Descontadas";
            movementType[3] = "Cta. Cte. Agentes";
            movementType[4] = "Cta. Cte. Coaseguros";
            movementType[5] = "Cta. Cte. Reaseguros";
            movementType[6] = "Contabilidad";
            movementType[7] = "Solicitud Pago Varios";
            movementType[8] = "Solicitud Pago Siniestros";
            movementType[9] = "Préstamos";

            // Llena Resumen de Movimientos
            foreach (MovementSummaryModel summary in paymentOrdersMovementsModel.MovementSummaryItems)
            {
                Models.Reports.PaymentOrders.PaymentOrdersReportModel summaryMovement = new Models.Reports.PaymentOrders.PaymentOrdersReportModel()
                {
                    BeneficiaryDocNumber = paymentOrdersMovementsModel.BeneficiaryDocNumber,
                    BeneficiaryName = paymentOrdersMovementsModel.BeneficiaryName,
                    BranchId = paymentOrdersMovementsModel.BranchId,
                    BranchName = paymentOrdersMovementsModel.BranchName,
                    CompanyId = paymentOrdersMovementsModel.CompanyId,
                    CompanyName = paymentOrdersMovementsModel.CompanyName,
                    Credit = summary.Credit,
                    CurrencyId = paymentOrdersMovementsModel.CurrencyId,
                    CurrencyName = paymentOrdersMovementsModel.CurrencyName,
                    Debit = summary.Debit,
                    DescriptionMovementSumary = movementType[i],
                    Id = paymentOrdersMovementsModel.Id,
                    PaymentAmount = paymentOrdersMovementsModel.PaymentAmount,
                    PaymentBranchId = paymentOrdersMovementsModel.PaymentBranchId,
                    PaymentBranchName = paymentOrdersMovementsModel.PaymentBranchName,
                    PaymentDate = paymentOrdersMovementsModel.PaymentDate,
                    PaymentOrderNumber = paymentOrdersMovementsModel.PaymentOrderNumber,
                    PaymentTypeId = paymentOrdersMovementsModel.PaymentTypeId,
                    PaymentTypeName = paymentOrdersMovementsModel.PaymentTypeName,
                    PayToName = paymentOrdersMovementsModel.PayToName,
                    UserId = _commonController.GetUserIdByName(User.Identity.Name),
                    UserName = User.Identity.Name.ToUpper(),
                };

                summariesReports.Add(summaryMovement);

                i++;
            }

            TempData["billRptSource"] = summariesReports;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//PaymentOrders//PaymentOrdersReport.rpt";

            return summariesReports[0].Id;
        }

        #endregion

        #region PreLiquidationReport

        /// <summary>
        /// ShowPreLiquidationReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <returns></returns>
        public void ShowPreLiquidationReport()
        {
            try
            {
                bool isValid = true;

                var reportSource = TempData["billRptSource"];
                var reportSubSourceSummary = TempData["billRptSubSource"];
                var reportName = TempData["BillingReportName"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    // Llena subreporte 1
                    reportDocument.Subreports[0].SetDataSource(reportSubSourceSummary);

                    if (reportSource.GetType().ToString() != "System.String")
                    {
                        // Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "CardDepositingPendingReport");

                    TempData["billRptSource"] = null;
                    TempData["billRptSubSource"] = null;
                    TempData["BillingReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadPreLiquidationReport
        /// Llena el reporte con los datos la preliquidación
        /// </summary>
        /// <param name="preLiquidationModel"></param>
        /// <param name="movementSumaryModel"></param>
        /// <returns>PreLiquidation Code</returns>
        public int LoadPreLiquidationReport(PreLiquidationModel preLiquidationModel,
                                            PreLiquidationMovementSumaryModel movementSumaryModel)
        {
            List<SEARCH.PremiumReceivableItemDTO> premiums =//GetTempPremiumReceivableItemByTempImputationId
                       DelegateService.accountingApplicationService.GetTempApplicationPremiumByApplicationId(preLiquidationModel.TempImputationId);

            List<PreLiquidationReport.PreLiquidationModel> preLiquidationsReports = new List<PreLiquidationReport.PreLiquidationModel>();

            List<SEARCH.PremiumReceivableItemDTO> detailsPremiumsReceivables = premiums;

            if (detailsPremiumsReceivables.Count > 0)
            {
                foreach (SEARCH.PremiumReceivableItemDTO premiumsReceivable in detailsPremiumsReceivables)
                {
                    PreLiquidationReport.PreLiquidationModel preLiquidation = new PreLiquidationReport.PreLiquidationModel();

                    // Llena cabecera
                    preLiquidation.Id = preLiquidationModel.Id;
                    preLiquidation.PreLiquidationBranch = preLiquidationModel.BranchDescription;
                    preLiquidation.PreLiquidationCompany = preLiquidationModel.CompanyDescription;
                    preLiquidation.UserName = User.Identity.Name;
                    preLiquidation.PayerDocumentNumberHeader = preLiquidationModel.PayerDocumentNumber;
                    preLiquidation.PayerDocumentNameHeader = preLiquidationModel.PayerName;
                    preLiquidation.DateTransaction = preLiquidationModel.RegisterDate;

                    // Llena detalles
                    preLiquidation.BranchPrefixPolicyEndorsement = premiumsReceivable.BranchPrefixPolicyEndorsement;
                    preLiquidation.PaymentNumber = premiumsReceivable.PaymentNumber;
                    preLiquidation.CurrencyDescription = premiumsReceivable.CurrencyDescription;
                    preLiquidation.PaymentAmount = premiumsReceivable.PaymentAmount;
                    preLiquidation.PaymentExpirationDate = premiumsReceivable.PaymentExpirationDate;
                    preLiquidation.PayerDocumentNumberName = premiumsReceivable.PayerDocumentNumberName;
                    preLiquidation.Address = premiumsReceivable.Address;

                    preLiquidationsReports.Add(preLiquidation);
                }
            }
            else
            {
                PreLiquidationReport.PreLiquidationModel preLiquidation = new PreLiquidationReport.PreLiquidationModel();

                // Llena cabecera
                preLiquidation.Id = preLiquidationModel.Id;
                preLiquidation.PreLiquidationBranch = preLiquidationModel.BranchDescription;
                preLiquidation.PreLiquidationCompany = preLiquidationModel.CompanyDescription;
                preLiquidation.UserName = User.Identity.Name;
                preLiquidation.PayerDocumentNumberHeader = preLiquidationModel.PayerDocumentNumber;
                preLiquidation.PayerDocumentNameHeader = preLiquidationModel.PayerName;
                preLiquidation.DateTransaction = preLiquidationModel.RegisterDate;

                // Llena detalles
                preLiquidation.BranchPrefixPolicyEndorsement = "";
                preLiquidation.PaymentNumber = 0;
                preLiquidation.CurrencyDescription = "";
                preLiquidation.PaymentAmount = 0;
                preLiquidation.PaymentExpirationDate = Convert.ToDateTime("01/01/2000");
                preLiquidation.PayerDocumentNumberName = "";
                preLiquidation.Address = "";

                preLiquidationsReports.Add(preLiquidation);
            }


            int i = 0;

            string[] movementType = new string[10];
            movementType[0] = "Primas por Cobrar";
            movementType[1] = "Primas en Depósito";
            movementType[2] = "Comisiones Descontadas";
            movementType[3] = "Cta. Cte. Agentes";
            movementType[4] = "Cta. Cte. Coaseguros";
            movementType[5] = "Cta. Cte. Reaseguros";
            movementType[6] = "Contabilidad";
            movementType[7] = "Solicitud Pago Varios";
            movementType[8] = "Solicitud Pago Siniestros";
            movementType[9] = "Préstamos";

            List<PreLiquidationReport.MovementSumaryModel> movementSumaries = new List<PreLiquidationReport.MovementSumaryModel>();
            // Llena Resumen de Movimientos
            foreach (MovementSumaryDetails summary in movementSumaryModel.MovementSumary)
            {
                PreLiquidationReport.MovementSumaryModel suamryMovement = new PreLiquidationReport.MovementSumaryModel()
                {
                    Credit = summary.Credit,
                    Debit = summary.Debit,
                    Description = movementType[i],
                    Id = summary.Id
                };

                movementSumaries.Add(suamryMovement);

                i++;
            }

            TempData["billRptSource"] = preLiquidationsReports;
            TempData["billRptSubSource"] = movementSumaries;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//PreLiquidation//PreLiquidationReport.rpt";

            return preLiquidationModel.Id;
        }

        /// <summary>
        /// LoadPreLiquidationReport
        /// Llena el reporte con los datos la preliquidación
        /// </summary>
        /// <param name="preLiquidationModel"></param>
        /// <param name="movementSumaryModel"></param>
        /// <returns>int</returns>
        public int LoadPreLiquidationReportA(PreLiquidationModel preLiquidationModel, PreLiquidationMovementSumaryModel movementSumaryModel)
        {
            List<SEARCH.PremiumReceivableItemDTO> premiums =
                        DelegateService.accountingApplicationService.GetTempApplicationPremiumByApplicationId(preLiquidationModel.TempImputationId);

            List<PreLiquidationReport.PreLiquidationModel> preLiquidationsReports = new List<PreLiquidationReport.PreLiquidationModel>();

            List<SEARCH.PremiumReceivableItemDTO> detailsPremiumsReceivables = premiums;

            // Llena cabecera
            PreLiquidationReport.PreLiquidationModel preLiquidation = new PreLiquidationReport.PreLiquidationModel()
            {
                DateTransaction = preLiquidationModel.RegisterDate,
                Id = preLiquidationModel.Id,
                PayerDocumentNameHeader = preLiquidationModel.PayerName,
                PayerDocumentNumberHeader = preLiquidationModel.PayerDocumentNumber,
                PreLiquidationBranch = preLiquidationModel.BranchDescription,
                PreLiquidationCompany = preLiquidationModel.CompanyDescription,
                UserName = User.Identity.Name
            };

            List<PreLiquidationReport.PremiumsReceivableModel> premiumsReceivables = new List<PreLiquidationReport.PremiumsReceivableModel>();

            if (detailsPremiumsReceivables.Count > 0)
            {
                foreach (SEARCH.PremiumReceivableItemDTO premiumsReceivable in detailsPremiumsReceivables)
                {
                    premiumsReceivables.Add(new PreLiquidationReport.PremiumsReceivableModel()
                    {
                        // Llena detalles
                        Id = preLiquidationModel.Id,
                        BranchPrefixPolicyEndorsement = premiumsReceivable.BranchPrefixPolicyEndorsement,
                        PaymentNumber = premiumsReceivable.PaymentNumber,
                        CurrencyDescription = premiumsReceivable.CurrencyDescription,
                        PaymentAmount = premiumsReceivable.PaymentAmount,
                        PaymentExpirationDate = premiumsReceivable.PaymentExpirationDate,
                        PayerDocumentNumberName = premiumsReceivable.PayerDocumentNumberName,
                        Address = premiumsReceivable.Address
                    });
                }
            }

            preLiquidation.premiumsReceivable = premiumsReceivables;
            preLiquidationsReports.Add(preLiquidation);

            List<PreLiquidationReport.MovementSumaryModel> movementSummaries = new List<PreLiquidationReport.MovementSumaryModel>();
            // Llena Resumen de Movimientos
            foreach (MovementSumaryDetails summary in movementSumaryModel.MovementSumary)
            {
                PreLiquidationReport.MovementSumaryModel summaryMovement = new PreLiquidationReport.MovementSumaryModel()
                {
                    Credit = summary.Credit,
                    Debit = summary.Debit,
                    Description = summary.Description,
                    Id = summary.Id
                };

                movementSummaries.Add(summaryMovement);
            }

            TempData["billRptSource"] = preLiquidationsReports;
            TempData["billRptSubSource"] = movementSummaries;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//PreLiquidation//PreLiquidationReport.rpt";

            return preLiquidationModel.Id;
        }

        /// <summary>
        /// LoadRePrintPreLiquidationsReport
        /// Llena el reporte con los datos de la preliquidación
        /// </summary>
        /// <param name="preLiquidationModel"></param>
        /// <returns>int</returns>
        public int LoadRePrintPreLiquidationsReport(Models.Imputation.PreLiquidationModel preLiquidationModel, int tempImputationId)
        {
            List<SEARCH.PremiumReceivableItemDTO> premiums =
            DelegateService.accountingApplicationService.GetTempApplicationPremiumByApplicationId(preLiquidationModel.TempImputationId);

            List<PreLiquidationReport.PreLiquidationModel> preLiquidationsReports = new List<PreLiquidationReport.PreLiquidationModel>();

            List<SEARCH.PremiumReceivableItemDTO> detailsPremiumsReceivables = premiums;

            if (detailsPremiumsReceivables.Count > 0)
            {
                foreach (SEARCH.PremiumReceivableItemDTO premiumsReceivable in detailsPremiumsReceivables)
                {
                    preLiquidationsReports.Add(new PreLiquidationReport.PreLiquidationModel()
                    {
                        // Llena cabecera
                        Id = preLiquidationModel.Id,
                        PreLiquidationBranch = preLiquidationModel.BranchDescription,
                        PreLiquidationCompany = preLiquidationModel.CompanyDescription,
                        UserName = User.Identity.Name,
                        PayerDocumentNumberHeader = preLiquidationModel.PayerDocumentNumber,
                        PayerDocumentNameHeader = preLiquidationModel.PayerName,
                        DateTransaction = preLiquidationModel.RegisterDate,

                        // Llena detalles
                        BranchPrefixPolicyEndorsement = premiumsReceivable.BranchPrefixPolicyEndorsement,
                        PaymentNumber = premiumsReceivable.PaymentNumber,
                        CurrencyDescription = premiumsReceivable.CurrencyDescription,
                        PaymentAmount = premiumsReceivable.PaymentAmount,
                        PaymentExpirationDate = premiumsReceivable.PaymentExpirationDate,
                        PayerDocumentNumberName = premiumsReceivable.PayerDocumentNumberName,
                        Address = premiumsReceivable.Address
                    });
                }
            }
            else
            {
                PreLiquidationReport.PreLiquidationModel preLiquidation = new PreLiquidationReport.PreLiquidationModel();

                // Llena cabecera
                preLiquidation.Id = preLiquidationModel.Id;
                preLiquidation.PreLiquidationBranch = preLiquidationModel.BranchDescription;
                preLiquidation.PreLiquidationCompany = preLiquidationModel.CompanyDescription;
                preLiquidation.UserName = User.Identity.Name;
                preLiquidation.PayerDocumentNumberHeader = preLiquidationModel.PayerDocumentNumber;
                preLiquidation.PayerDocumentNameHeader = preLiquidationModel.PayerName;
                preLiquidation.DateTransaction = preLiquidationModel.RegisterDate;

                // Llena detalles
                preLiquidation.BranchPrefixPolicyEndorsement = "";
                preLiquidation.PaymentNumber = 0;
                preLiquidation.CurrencyDescription = "";
                preLiquidation.PaymentAmount = 0;
                preLiquidation.PaymentExpirationDate = Convert.ToDateTime("01/01/2000");
                preLiquidation.PayerDocumentNumberName = "";
                preLiquidation.Address = "";

                preLiquidationsReports.Add(preLiquidation);
            }

            List<PreLiquidationReport.MovementSumaryModel> movementSummaries = new List<PreLiquidationReport.MovementSumaryModel>();

            decimal amount = 0;
            ApplicationDTO imputation = new ApplicationDTO()
            {
                RegisterDate = DateTime.Now,
                Id = tempImputationId,
                UserId = _commonController.GetUserIdByName(User.Identity.Name)
            };

            imputation = DelegateService.accountingApplicationService.GetDebitsAndCreditsMovementTypes(imputation, Convert.ToDecimal(amount));

            string[] movementType = new string[10];
            movementType[0] = "Primas por Cobrar";
            movementType[1] = "Primas en Depósito";
            movementType[2] = "Comisiones Descontadas";
            movementType[3] = "Cta. Cte. Agentes";
            movementType[4] = "Cta. Cte. Coaseguros";
            movementType[5] = "Cta. Cte. Reaseguros";
            movementType[6] = "Contabilidad";
            movementType[7] = "Solicitud Pago Varios";
            movementType[8] = "Solicitud Pago Siniestros";
            movementType[9] = "Préstamos";

            // Llena Resumen de Movimientos
            for (int i = 0; i < imputation.ApplicationItems.Count; i++)
            {
                decimal debits = 0;
                decimal credits = 0;

                PreLiquidationReport.MovementSumaryModel summaryMovement = new PreLiquidationReport.MovementSumaryModel();

                summaryMovement.Id = i;

                if ((imputation.ApplicationItems[i] != null) && (imputation.ApplicationItems[i].TotalDebit != null))
                {
                    debits = System.Math.Abs(Convert.ToDecimal(imputation.ApplicationItems[i].TotalDebit.Value));
                }

                if ((imputation.ApplicationItems[i] != null) && (imputation.ApplicationItems[i].TotalCredit != null))
                {
                    credits = System.Math.Abs(Convert.ToDecimal(imputation.ApplicationItems[i].TotalCredit.Value));
                }

                summaryMovement.Description = movementType[i];
                summaryMovement.Debit = Convert.ToDouble(debits);
                summaryMovement.Credit = Convert.ToDouble(credits);

                movementSummaries.Add(summaryMovement);
            }

            TempData["billRptSource"] = preLiquidationsReports;
            TempData["billRptSubSource"] = movementSummaries;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//PreLiquidation//PreLiquidationReport.rpt";

            return preLiquidationModel.Id;
        }

        #endregion

        #region CreditNote

        /// <summary>
        /// ShowCreditNoteReport
        /// </summary>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="payerId"></param>
        /// <returns></returns>
        public void ShowCreditNoteReport(string policyDocumentNumber, string branchId, string prefixId, int payerId)
        {
            List<CreditNoteReportModel> creditNoteReportModels = new List<CreditNoteReportModel>();

            List<SEARCH.EndorsementPaymentDTO> endorsementPayments = DelegateService.accountingApplicationService.GenerateCreditNoteReport(policyDocumentNumber, branchId, prefixId);

            foreach (SEARCH.EndorsementPaymentDTO endorsementPayment in endorsementPayments)
            {
                CreditNoteReportModel creditNoteReportItem = new CreditNoteReportModel();

                creditNoteReportItem.BranchId = endorsementPayment.BranchId;
                creditNoteReportItem.BranchDescription = endorsementPayment.BranchDescription;
                creditNoteReportItem.PrefixId = endorsementPayment.PrefixId;
                creditNoteReportItem.PrefixDescription = endorsementPayment.PrefixDescription;
                creditNoteReportItem.PolicyId = endorsementPayment.PolicyId;
                creditNoteReportItem.PolicyDocumentNumber = endorsementPayment.PolicyDocumentNumber;
                creditNoteReportItem.EndorsementId = endorsementPayment.EndorsementId;
                creditNoteReportItem.EndorsementDocumentNumber = endorsementPayment.EndorsementDocumentNumber;
                creditNoteReportItem.PayerIndividualId = payerId;
                
                creditNoteReportItem.PayerName = payerId == 0 ? "" : DelegateService.accountingAccountsPayableService.GetIndividualByIndividualId(Convert.ToInt32(payerId)).Name;
                creditNoteReportItem.PaymentNumer = endorsementPayment.PaymentNumber;
                creditNoteReportItem.Amount = endorsementPayment.Amount;
                creditNoteReportItem.ImputationId = endorsementPayment.ImputationId;

                creditNoteReportModels.Add(creditNoteReportItem);
            }

            ReportDocument rd = new ReportDocument();

            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//Accounting//Reports//CreditNotes//CreditNotesReport.rpt";
            rd.Load(strRptPath);

            rd.SetDataSource(creditNoteReportModels);

            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "CommssionBalanceRpt");

            // Clear all sessions value
            Session["billRptSource"] = null;
            Session["BillingReportName"] = null;
        }

        #endregion

        #region JournalEntry

        /// <summary>
        /// LoadRePrintJournalEntryBillReport
        /// </summary>
        /// <param name="accountingDate"></param>
        /// <param name="journalEntryId"></param>
        /// <param name="technicalTransaction"></param>
        public void LoadRePrintJournalEntryBillReport(string accountingDate, string journalEntryId, int technicalTransaction)
        {
            try
            {
                var journalEntryList = DelegateService.glAccountingApplicationService.SearchDailyEntryMovements(technicalTransaction,
                                                                                   Convert.ToDateTime("01/01/1900"),
                                                                                   Convert.ToDateTime("01/01/1900"), 0, 0, 0);

                var rptJournalEntrySource = (from EntryConsultationDTO dto in journalEntryList
                                             select new DailyEntryReportModel()
                                             {
                                                 Branch = _baseController.GetBranchDescriptionById(dto.BranchId, User.Identity.Name.ToUpper()),
                                                 AccountingAccountDescription = dto.AccountingAccountName,
                                                 AccountingNumber = Convert.ToDecimal(dto.AccountingAccountNumber),
                                                 Description = dto.EntryDescription,
                                                 EntryNumber = dto.DailyEntryHeaderId,
                                                 Date = Convert.ToDateTime(accountingDate),
                                                 CurrencyDescription = GetCurrencyDescriptionById(dto.CurrencyId),
                                                 Debit = dto.AccountingNature == (int)AccountingNature.Debit
                                                 ? dto.DebitsAmountLocalValue : 0,
                                                 Credit = dto.AccountingNature == (int)AccountingNature.Credit
                                                 ? dto.CreditsAmountLocalValue : 0,
                                                 CompanyDescription = "",
                                                 TechnicalTransaction = technicalTransaction,
                                                 AmountValue = (dto.AccountingNature == (int)AccountingNature.Debit) ? dto.DebitsAmountValue : dto.CreditsAmountValue
                                             }).ToList();

                var reportDocument = new ReportDocument();
                var reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//Accounting//Reports//JournalEntry//JournalEntryReport.rpt";

                reportDocument.Load(reportPath);

                if (rptJournalEntrySource != null && rptJournalEntrySource.GetType().ToString() != "System.String")
                {
                    reportDocument.SetDataSource(rptJournalEntrySource);
                }
                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "JournalEntryReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// GetCurrencyDescriptionById
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>string</returns>
        private string GetCurrencyDescriptionById(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currencyNames = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currencyNames[0].Description;
        }

        /// <summary>
        /// ReplaceWithAsterisks
        /// Reemplaza los números por asteríscos
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns>string</returns>
        private string ReplaceWithAsterisks(string cardNumber)
        {
            int row = 0;
            string valueWithAsterisk = "";

            if (cardNumber.Length >= 4)
            {
                string lastNumber = cardNumber.Substring(cardNumber.Length - 4);
                for (row = 0; row < cardNumber.Length - 2; row++)
                {
                    valueWithAsterisk += "*";
                }

                valueWithAsterisk = valueWithAsterisk + lastNumber;
                cardNumber = valueWithAsterisk;
            }
            return cardNumber;
        }
        #endregion

    }
}