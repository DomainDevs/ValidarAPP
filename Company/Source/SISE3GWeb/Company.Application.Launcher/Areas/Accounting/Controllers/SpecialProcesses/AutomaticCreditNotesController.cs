using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Sistran.Core.Application.AccountingServices.DTOs;
// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.CreditNotes;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.SpecialProcess;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
//Sistran Company
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.SpecialProcesses
{
    [NoDirectAccess]
    public class AutomaticCreditNotesController : Controller
    {
        #region  Class

        /// <summary>
        /// WorkerFactory
        /// Ejecuta proceso de manera asíncrona
        /// </summary>
        public sealed class WorkerFactory
        {
            private static volatile WorkerFactory _instance;
            private static object syncRoot = new Object();

            public static WorkerFactory Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (syncRoot)
                        {
                            if (_instance == null)
                                _instance = new WorkerFactory();
                        }
                    }
                    return _instance;
                }
            }

            /// <summary>
            /// CreateWorker
            /// Inicia el proceso asíncrono
            /// </summary>
            /// <param name="operationType"></param>
            /// <param name="branch"></param>
            /// <param name="prefix"></param>
            /// <param name="policy"></param>
            /// <param name="insured"></param>
            public void CreateWorker(int operationType, BranchDTO branch, PrefixDTO prefix, PolicyDTO policy, IndividualDTO insured)
            {
                Task.Run(() => DelegateService.accountingCreditNoteService.SaveCreditNote(operationType, branch, prefix, policy, insured)).Wait();
            }

        }

        #endregion

        #region Instance Variables

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region Views

        /// <summary>
        /// MainAutomaticCreditNotes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAutomaticCreditNotes()
        {

            try
            {
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AutomaticCreditNotes/MainAutomaticCreditNotes.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        #endregion

        #region Actions

        /// <summary>
        /// GetStatusProcess
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetStatusProcess()
        {
            List<object> statusProcesses = new List<object>();

            statusProcesses.Add(new
            {
                Id = (int)CreditNoteStatus.Actived,
                Name = Global.Active.ToString()
            });
            statusProcesses.Add(new
            {
                Id = (int)CreditNoteStatus.Applied,
                Name = Global.Applied.ToString()
            });
            statusProcesses.Add(new
            {
                Id = (int)CreditNoteStatus.Rejected,
                Name = Global.Rejected.ToString()
            });
            statusProcesses.Add(new
            {
                Id = (int)CreditNoteStatus.NoData,
                Name = Global.NoData.ToString()
            });

            return new UifSelectResult(statusProcesses);
        }

        /// <summary>
        /// GetOperations
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetOperations()
        {
            return new UifSelectResult(_commonController.GetBusinessTypes());
        }

        /// <summary>
        /// GenerateAutomaticCreditNotes
        /// Genera el cruce automático de notas de crédito
        /// </summary>
        /// <param name="operationTypeId"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="individualId"></param>
        /// <param name="policyNumber"></param>
        /// <param name="currencyId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GenerateAutomaticCreditNotes(int operationTypeId, int branchId, int prefixId,
                                                         int individualId, string policyNumber, int currencyId)
        {
            List<object> messages = new List<object>();

            CurrencyDTO currency = new CurrencyDTO();
            currency.Id = currencyId;

            BranchDTO branch = new BranchDTO()
            {
                Id = branchId
            };

            PrefixDTO prefix = new PrefixDTO() { Id = prefixId };

            PolicyDTO policy = new PolicyDTO()
            {
                Id = 0,
                DocumentNumber = Convert.ToInt32(policyNumber),
                UserId = _commonController.GetUserIdByName(User.Identity.Name),
                ExchangeRate = new ExchangeRateDTO() { Currency = currency }
            };

            IndividualDTO insured = new IndividualDTO()
            {
                IndividualId = individualId
            };

            WorkerFactory.Instance.CreateWorker(operationTypeId, branch, prefix, policy, insured);

            messages.Add(new
            {
                ProcessNumber = 1,
                MessageError = "OK"
            });

            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetProcessByStatus
        /// Obtiene los procesos pendientes de notas de crédito
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetProcessByStatus(CreditNoteStatus? statusId)
        {
            int userId = 0;
            if (User != null)
            {
                userId = _commonController.GetUserIdByName(User.Identity.Name);
            }
            else
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }

            List<object> processes = new List<object>();
            List<CreditNoteDTO> creditNotes = DelegateService.accountingCreditNoteService.GetCreditNotes();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;

            List<CreditNoteDTO> creditNotesOrders;

            if (statusId > 0)
            {
                creditNotesOrders = (from creditNote in creditNotes
                                     where creditNote.CreditNoteStatus == (int)statusId && creditNote.UserId == userId
                                     orderby creditNote.Id descending
                                     select creditNote).ToList();
            }
            else
            {
                creditNotesOrders = (from creditNote in creditNotes
                                     where creditNote.UserId == userId
                                     orderby creditNote.Id descending
                                     select creditNote).ToList();
            }

            foreach (CreditNoteDTO creditNote in creditNotesOrders)
            {
                var progress = "";

                if (Convert.ToDateTime(creditNote.CreditNoteItems[0].NegativePolicy.CurrentTo) == Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    progress = @Global.InProcess;
                }
                else
                {
                    progress = @Global.Finalized;
                }

                //FILTRA SOLO POR EL USUARIO LOGUEADO
                processes.Add(new
                {
                    Id = creditNote.Id,
                    ProcessDate = creditNote.Date.ToString("dd/MM/yyyy H:mm:ss"),
                    PositiveTotalApplied = String.Format(new CultureInfo("en-US"), "{0:C}", creditNote.PositiveAppliedTotal.Value),
                    NegativeTotalApplied = creditNote.NegativeAppliedTotal.Value < 0 ?
                           creditNote.NegativeAppliedTotal.Value.ToString("C") :
                            String.Format(new CultureInfo("en-US"), "{0:C}", creditNote.NegativeAppliedTotal.Value),
                    UserName = DelegateService.uniqueUserService.GetUserById(creditNote.UserId).AccountName,
                    ReceiptNumber = creditNote.CreditNoteItems[0].NegativePolicy.Endorsement.Id,
                    IsActive = Resources.Global.ResourceManager.GetString(creditNote.CreditNoteStatus.ToString()),
                    Progress = creditNote.CreditNoteItems[0].Id + " " + Global.ProcessedRecords + " / " + progress,
                    ProgressStatus = progress
                });
            }

            return Json(new
            {
                aaData = processes,
                total = processes.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCreditNotesByProcessNumber
        /// Obtiene el detalle del proceso cruce automático notas de crédito
        /// </summary>
        /// <param name="processNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetCreditNotesByProcessNumber(string processNumber)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;
            List<object> creditNoteItems = new List<object>();
            try
            {



            CreditNoteDTO creditNote = new CreditNoteDTO()
            {
                CreditNoteStatus = (int)CreditNoteStatus.Actived,
                Id = Convert.ToInt32(processNumber),
                NegativeAppliedTotal = new AmountDTO() { Value = 0 },
            };

            int userId = 0;
            if (User != null)
            {
                userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());
            }
            else
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }

            creditNote.UserId = userId;
            creditNote = DelegateService.accountingCreditNoteService.GetCreditNote(creditNote);

            if (creditNote.CreditNoteItems != null)
            {
                int top = 0;
                foreach (CreditNoteItemDTO creditNoteItem in creditNote.CreditNoteItems)
                {
                    top++;
                    creditNoteItems.Add(new
                    {
                        Id = creditNoteItem.Id,//TempPremiumReceivableCode
                        BranchId = creditNoteItem.NegativePolicy.Branch.Id,
                        Branch = creditNoteItem.NegativePolicy.Branch.Description,
                        PrefixId = creditNoteItem.NegativePolicy.Prefix.Id,
                        Prefix = creditNoteItem.NegativePolicy.Prefix.Description,
                        PolicyId = creditNoteItem.NegativePolicy.Id,
                        Policy = creditNoteItem.NegativePolicy.DocumentNumber,
                        Endorsement = creditNoteItem.NegativePolicy.Endorsement.Number,
                        Insured = creditNoteItem.NegativePolicy.Holder.Name,
                        Payer = creditNoteItem.NegativePolicy.DefaultBeneficiaries[0].Name,
                        PrincipalAgent = creditNoteItem.NegativePolicy.Agencies[0].Agent.FullName,                       
                        CurrencyId = creditNoteItem.NegativePolicy.ExchangeRate.Currency.Id,
                        Currency = creditNoteItem.NegativePolicy.ExchangeRate.Currency.Description,
                        Amount = creditNoteItem.NegativePolicy.PayerComponents[0].Amount < 0 ?
                           creditNoteItem.NegativePolicy.PayerComponents[0].Amount.ToString("C") :
                           string.Format(new CultureInfo("en-US"), "{0:C}", creditNoteItem.NegativePolicy.PayerComponents[0].Amount),

                        ExchangeRate = string.Format(new CultureInfo("en-US"), "{0:C}", creditNoteItem.NegativePolicy.ExchangeRate.BuyAmount),

                        LocalAmount = creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount < 0 ?
                           creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount.ToString("C") :
                           string.Format(new CultureInfo("en-US"), "{0:C}", creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount),
                        ImputationId = creditNote.Id, //TempImputationCode
                        ApplicationReceiptNumber = 0, //Se genera después de la aplicación
                        EntryNumber = "",             //Se genera después de la aplicación
                        ProcessNumber = processNumber,
                        InsuredPersonType = "",
                        AgentPersonType = "",
                        PayerPersonType = ""
                    });
                }
            }

                return Json(new { aaData = creditNoteItems, total = creditNoteItems.Count }, JsonRequestBehavior.AllowGet);
            }

            catch (UnhandledException ex)
            {
                creditNoteItems.Add(new
                {
                    Id = ""
,
                    BranchId = "",
                    Branch = Global.UnhandledExceptionMsj,
                    PrefixId = "",
                    Prefix = "",
                    PolicyId = "",
                    Policy = "",
                    Endorsement = "",
                    Insured = "",
                    Payer = "",
                    PrincipalAgent = "",
                    CurrencyId = "",
                    Currency = "",
                    Amount = "",
                    ExchangeRate = "",
                    LocalAmount = "",
                    ImputationId = "",
                    ApplicationReceiptNumber = "",
                    EntryNumber = "",
                    ProcessNumber = "",
                    InsuredPersonType = "",
                    AgentPersonType = "",
                    PayerPersonType = ""
                });
                return Json(new { aaData = creditNoteItems, total = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// GetHeaderCreditNotesByProcessNumber
        /// Obtiene la cabecera del proceso cruce automático notas de crédito
        /// </summary>
        /// <param name="processNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetHeaderCreditNotesByProcessNumber(string processNumber)
        {
            List<object> creditNoteHeaders = new List<object>();

            try
            {

                if (!String.IsNullOrEmpty(processNumber))
                {
                    List<CreditNoteDTO> creditNotes = DelegateService.accountingCreditNoteService.GetCreditNotes();
                    var notes = creditNotes.Where(r => (r.UserId.Equals(_commonController.GetUserIdByName(User.Identity.Name)) &&
                                                        r.Id.Equals(Convert.ToInt32(processNumber)))).ToList();

                    if (notes.Count > 0)
                    {
                        foreach (CreditNoteDTO creditNote in notes)
                        {
                            if (creditNote.Id == Convert.ToInt32(processNumber) && creditNote.CreditNoteStatus == (int)CreditNoteStatus.Actived)//FILTRA SOLO LOS PROCESOS ACTIVOS
                            {
                                creditNoteHeaders.Add(new
                                {
                                    ProcessDate = creditNote.Date.ToString("dd/MM/yyyy H:mm:ss"),
                                    UserName = _commonController.GetUserByName(User.Identity.Name)[0].AccountName,
                                    TotalCredits = creditNote.NegativeAppliedTotal.Value,
                                    TotalDebits = creditNote.PositiveAppliedTotal.Value
                                });
                            }
                        }
                    }
                    else
                    {
                        creditNoteHeaders.Add(new
                        {
                            UserName = "-1",
                        });
                    }

                }

            return Json(creditNoteHeaders, JsonRequestBehavior.AllowGet);

            }
            catch (OverflowException)
            {
                return Json(new { success = false, result = Global.ErrorMaxlength }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetCreditNotesByProcessNumberReport
        /// Obtiene la cabecera y detalle de las notas de crédito pendientes de aplicar
        /// </summary>
        /// <param name="processCreditNotes"></param>
        public void GetCreditNotesByProcessNumberReport(string processCreditNotes)
        {
            List<AutomaticCreditNotesModel> automaticCreditNotesModels = new List<AutomaticCreditNotesModel>();
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            CreditNoteDTO creditNote = new CreditNoteDTO()
            {
                CreditNoteStatus = (int)CreditNoteStatus.Actived,
                Id = Convert.ToInt32(processCreditNotes),
                NegativeAppliedTotal = new AmountDTO() { Value = -1 },
                UserId = userId
            };

            // Se obtiene la cabecera
            creditNote = DelegateService.accountingCreditNoteService.GetCreditNote(creditNote);

            var processDate = "";
            decimal totalCredits = 0;
            decimal totalDebits = 0;

            if (creditNote != null && creditNote.UserId > 0)
            {
                processDate = creditNote.Date.ToString("dd/MM/yyyy H:mm:ss");
                totalCredits = Convert.ToDecimal(creditNote.NegativeAppliedTotal.Value);
                totalDebits = Convert.ToDecimal(creditNote.PositiveAppliedTotal.Value);

                // Se obtiene el detalle
                creditNote = new CreditNoteDTO()
                {
                    CreditNoteStatus = (int)CreditNoteStatus.Actived,
                    Id = Convert.ToInt32(processCreditNotes),
                    NegativeAppliedTotal = new AmountDTO() { Value = 0 },
                    UserId = userId
                };
                creditNote = DelegateService.accountingCreditNoteService.GetCreditNote(creditNote);

                foreach (CreditNoteItemDTO creditNoteItem in creditNote.CreditNoteItems)
                {
                    automaticCreditNotesModels.Add(new AutomaticCreditNotesModel()
                    {
                        ProcessCreditNotes = Convert.ToInt32(processCreditNotes),
                        ProcessDate = Convert.ToDateTime(processDate),
                        UserName = User.Identity.Name,
                        TotalDebits = totalDebits,
                        TotalCredits = totalCredits,
                        BranchName = _commonController.GetBranchDescriptionByIdUserId(creditNoteItem.NegativePolicy.Branch.Id, userId),
                        PrefixName = _commonController.GetPrefixDescriptionById(creditNoteItem.NegativePolicy.Prefix.Id),
                        Policy = creditNoteItem.NegativePolicy.DocumentNumber.ToString(),
                        Endorsement = creditNoteItem.NegativePolicy.Endorsement.Number,
                        Insured = creditNoteItem.NegativePolicy.Holder.Name,
                        Payer = creditNoteItem.NegativePolicy.DefaultBeneficiaries[0].Name,
                        PrincipalAgent = creditNoteItem.NegativePolicy.Agencies[0].Agent.FullName,
                        Currency = creditNoteItem.NegativePolicy.ExchangeRate.Currency.Description,
                        Amount = creditNoteItem.NegativePolicy.PayerComponents[0].Amount,
                        Change = creditNoteItem.NegativePolicy.ExchangeRate.BuyAmount,
                        LocalAmount = creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount,
                        ApplicationReceiptNumber = ""
                    });
                }
            }

            TempData["creditNotesRptSource"] = automaticCreditNotesModels;
            TempData["creditNotesReportName"] = "Areas//Accounting//Reports//SpecialProcess//AutomaticCreditNotesReport.rpt";
        }



        /// <summary>
        /// GetHeaderAppliedCreditNotesByProcessNumber
        /// Obtiene la cabecera de las notas de crédito aplicadas
        /// </summary>
        /// <param name="processNumber"></param>
        /// <returns>JsonResult</returns>
        [NoDirectAccess]
        public JsonResult GetHeaderAppliedCreditNotesByProcessNumber(string processNumber)
        {
            List<object> jsonData = new List<object>();

            try
            {


                CreditNoteDTO newCreditNote;
                CreditNoteDTO creditNote = new CreditNoteDTO()
                {
                    CreditNoteStatus = (int)CreditNoteStatus.Applied,
                    Id = Convert.ToInt32(processNumber),
                    NegativeAppliedTotal = new AmountDTO() { Value = -2 },
                    UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
                };

                newCreditNote = DelegateService.accountingCreditNoteService.GetCreditNote(creditNote);

                if (newCreditNote.Id > 0)
                {
                    jsonData.Add(new
                    {
                        ProcessDate = newCreditNote.Date.ToString("dd/MM/yyyy H:mm:ss"),
                        UserName = DelegateService.uniqueUserService.GetUserById(newCreditNote.UserId).AccountName,
                        TotalCredits = newCreditNote.NegativeAppliedTotal.Value,
                        TotalDebits = newCreditNote.PositiveAppliedTotal.Value
                    });

                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            catch (TimeoutException ex)
            {
                return Json(new { success = false, result = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (OverflowException)
            {
                return Json(new { success = false, result = Global.ErrorMaxlength }, JsonRequestBehavior.AllowGet);
            }

            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// GetAppliedCreditNotesByProcessNumber
        /// Obtiene el detalle de las notas de crédito aplicadas
        /// </summary>
        /// <param name="processNumber"></param>
        /// <returns>ActionResult</returns>
        [NoDirectAccess]
        public ActionResult GetAppliedCreditNotesByProcessNumber(string processNumber)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;

            List<object> jsonData = new List<object>();


            try
            {


                CreditNoteDTO creditNote = new CreditNoteDTO()
                {
                    CreditNoteStatus = (int)CreditNoteStatus.Applied,
                    Id = Convert.ToInt32(processNumber),
                    NegativeAppliedTotal = new AmountDTO() { Value = -3 },
                    UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
                };

                creditNote = DelegateService.accountingCreditNoteService.GetCreditNote(creditNote);

                if (creditNote.CreditNoteItems != null)
                {
                    foreach (CreditNoteItemDTO creditNoteItem in creditNote.CreditNoteItems)
                    {
                        jsonData.Add(new
                        {
                            Id = creditNoteItem.Id,
                            BranchId = creditNoteItem.NegativePolicy.Branch.Id,
                            Branch = creditNoteItem.NegativePolicy.Branch.Description,

                            Amount = creditNoteItem.NegativePolicy.PayerComponents[0].Amount < 0 ?
                               creditNoteItem.NegativePolicy.PayerComponents[0].Amount.ToString("C") :
                                String.Format(new CultureInfo("en-US"), "{0:C}", creditNoteItem.NegativePolicy.PayerComponents[0].Amount),

                            ExchangeRate = String.Format(new CultureInfo("en-US"), "{0:C}", creditNoteItem.NegativePolicy.ExchangeRate.BuyAmount),

                            LocalAmount = creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount < 0 ?
                               creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount.ToString("C") :
                                String.Format(new CultureInfo("en-US"), "{0:C}", creditNoteItem.NegativePolicy.PayerComponents[0].BaseAmount),

                            ImputationId = creditNote.Id,
                            EntryNumber = creditNoteItem.PositivePolicy.Id == 0 ? "" : creditNoteItem.PositivePolicy.Id.ToString(),
                            ApplyDate = creditNote.Date.ToString() == "1/1/0001 12:00:00 AM" ? "" : creditNote.Date.ToString()
                        });
                    }
                }

                return new UifTableResult(jsonData);

            }

            catch (TimeoutException ex)
            {
                jsonData.Add(new
                {
                    Id = "",
                    BranchId = "",
                    Branch = ex.Message,
                    Amount = "",
                    ExchangeRate = "",
                    LocalAmount = "",
                    ImputationId = "",
                    EntryNumber = "",
                    ApplyDate = ""
                });

                return new UifTableResult(jsonData);

            }

            catch (UnhandledException)
            {

                jsonData.Add(new
                {
                    Id = "",
                    BranchId = "",
                    Branch = Global.UnhandledExceptionMsj,
                    Amount = "",
                    ExchangeRate = "",
                    LocalAmount = "",
                    ImputationId = "",
                    EntryNumber = "",
                    ApplyDate = ""
                });

                return new UifTableResult(jsonData);
            }
        }


        /// <summary>
        /// ShowAutomaticCreditNotes
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowAutomaticCreditNotes()
        {
            try
            {
                bool isValid = true;

                var reportSource = TempData["creditNotesRptSource"];
                var reportName = TempData["creditNotesReportName"];

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
                        //Llena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "AutomaticCreditNotesReport");

                    TempData["creditNotesRptSource"] = null;
                    TempData["creditNotesReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LowAutomaticCreditNotes
        /// Dar de baja al proceso de cruce automático de notas de crédito
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult LowAutomaticCreditNotes(int processId)
        {
            try
            {

                CreditNoteDTO creditNote = new CreditNoteDTO()
                {
                    CreditNoteStatus = (int)CreditNoteStatus.Rejected,
                    Id = processId,
                    UserId = SessionHelper.GetUserId()
                };

                DelegateService.accountingCreditNoteService.UpdateCreditNote(creditNote);

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (BusinessException)
            {
                return Json(new { success = false, result = Global.MessageInternalError }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// DeleteAutomaticCreditNotes
        /// Elimina las notas de crédito temporales 
        /// </summary>
        /// <param name="itemsToAppliedModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteAutomaticCreditNotes(ItemsToAppliedModel itemsToAppliedModel)
        {
            List<object> creditNotes = new List<object>();

            CreditNoteDTO creditNote = new CreditNoteDTO();
            List<CreditNoteItemDTO> creditNoteItems = new List<CreditNoteItemDTO>();

            creditNote.CreditNoteStatus = (int)CreditNoteStatus.Actived;

            creditNote.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            if (itemsToAppliedModel.CreditNoteItem != null)
            {
                foreach (CreditNoteItemModel noteItem in itemsToAppliedModel.CreditNoteItem)
                {
                    creditNote.Id = noteItem.ProcessId;
                    CreditNoteItemDTO creditNoteItem = new CreditNoteItemDTO();

                    creditNoteItem.Id = noteItem.CreditNoteItemId;
                    creditNoteItems.Add(creditNoteItem);
                    creditNote.CreditNoteItems = creditNoteItems;
                    creditNoteItems = new List<CreditNoteItemDTO>();

                    // Borra registro a registro ya que el FWK no soporta enviar un objeto muy grande al servicio
                    creditNote = DelegateService.accountingCreditNoteService.UpdateCreditNote(creditNote);
                    creditNote.CreditNoteItems = new List<CreditNoteItemDTO>();
                }
            }

            if (creditNote.Id == 0)
            {
                creditNotes.Add(new
                {
                    ProcessNumber = creditNote.Id
                });
            }
            else
            {
                creditNotes.Add(new
                {
                    ProcessNumber = creditNote.Id,
                    MessageError = creditNote.CreditNoteItems[0].NegativePolicy.BillingGroup.Description
                });

            }


            return Json(creditNotes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ApplyAutomaticCreditNotes
        /// Aplicación de las notas de crédito
        /// </summary>
        /// <param name="itemsToAppliedModel"></param>
        /// <param name="totalDebits"></param>
        /// <param name="totalCredits"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ApplyAutomaticCreditNotes(ItemsToAppliedModel itemsToAppliedModel, string totalDebits, string totalCredits)
        {
            List<object> creditNotes = new List<object>();

            try
            {
                CreditNoteDTO creditNote = new CreditNoteDTO();
                List<CreditNoteItemDTO> creditNoteItems = new List<CreditNoteItemDTO>();

                creditNote.CreditNoteStatus = (int)CreditNoteStatus.Applied;
                creditNote.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                creditNote.NegativeAppliedTotal = new AmountDTO() { Value = Convert.ToDecimal(totalDebits) };
                creditNote.PositiveAppliedTotal = new AmountDTO() { Value = Convert.ToDecimal(totalCredits) };

                if (itemsToAppliedModel.CreditNoteItem != null)
                {
                    foreach (CreditNoteItemModel noteItem in itemsToAppliedModel.CreditNoteItem)
                    {
                        creditNote.Id = noteItem.ProcessId;
                        CreditNoteItemDTO creditNoteItem = new CreditNoteItemDTO();
                        creditNoteItem.Id = noteItem.CreditNoteItemId;

                        PolicyDTO policy = new PolicyDTO();
                        policy.Branch = new BranchDTO()
                        {
                            Id = noteItem.BranchId,
                            Description = noteItem.BranchName
                        };
                        policy.ExchangeRate = new ExchangeRateDTO()
                        {

                            Currency = new CurrencyDTO()
                            {

                                Id = noteItem.CurrencyId,
                                Description = noteItem.CurrencyName
                            }
                        };

                        policy.Endorsement = new EndorsementDTO()
                        {
                            Id = noteItem.PayerPersonTypeId
                        };

                        policy.Id = noteItem.ImputationId;
                        creditNoteItem.NegativePolicy = policy;

                        creditNoteItems.Add(creditNoteItem);
                    }
                }
                creditNote.CreditNoteItems = creditNoteItems;

                creditNote = DelegateService.accountingCreditNoteService.UpdateCreditNote(creditNote);

                #region Accounting

                string messageFromGeneralLedger = "";                

                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    messageFromGeneralLedger = new JournalEntryController().RecordJournalEntry(creditNote.Id, creditNote.UserId);
                }
                else
                {
                    messageFromGeneralLedger = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                }               

                #endregion Accounting

                creditNotes.Add(new
                {
                    ResultGeneralLedger = messageFromGeneralLedger, //No. de AccountTransaction
                    JournalEntryId = creditNote.CreditNoteItems[0].Id //No. TechnicalTransaction                   
                });
            }
            catch (BusinessException)
            {
                return Json(new { success = false, result = Global.MessageInternalError }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }

            return Json(creditNotes, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}