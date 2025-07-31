using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

// Sstran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.SpecialProcess;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;

//Sistran Company
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.DTOs.Amortizations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.SpecialProcesses
{
    [NoDirectAccess]
    public class AutomaticAmortizationController : Controller
    {
        #region Class

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
            /// <param name="amount"></param>
            public void CreateWorker(int operationType, BranchDTO branch, PrefixDTO prefix, PolicyDTO policy, IndividualDTO insured, decimal amount)
            {
                Thread thread = new Thread(() => DelegateService.accountingAmortizationService.GenerateAmortization(operationType, branch, prefix, policy, insured, amount));
                thread.Start();
            }
        }

        #endregion

        #region Instance Variables

        readonly CommonController _commonController = new CommonController();
        readonly JournalEntryController _journalEntryController = new JournalEntryController();

        #endregion

        #region Views

        /// <summary>
        /// MainAutomaticAmortization
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAutomaticAmortization()
        {
            try
            {

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AutomaticAmortization/MainAutomaticAmortization.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// GetOperations
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetOperations()
        {
            return new UifSelectResult(_commonController.GetBusinessTypes());
        }

        /// <summary>
        /// GetAmortizationStatus
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAmortizationStatus()
        {
            List<object> statusProcesses = new List<object>();

            statusProcesses.Add(new
            {
                Id = (int)AmortizationStatus.Actived,
                Name = AmortizationStatus.Actived.ToString()
            });
            statusProcesses.Add(new
            {
                Id = (int)AmortizationStatus.Applied,
                Name = AmortizationStatus.Applied.ToString()
            });
            statusProcesses.Add(new
            {
                Id = (int)AmortizationStatus.Rejected,
                Name = AmortizationStatus.Rejected.ToString()
            });
            statusProcesses.Add(new
            {
                Id = (int)AmortizationStatus.NoData,
                Name = AmortizationStatus.NoData.ToString()
            });

            return new UifSelectResult(statusProcesses);
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
        /// <param name="limitAmount"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult GenerateAutomaticAmortization(int operationTypeId, int branchId, int prefixId,
                                                          int individualId, string policyNumber, decimal limitAmount,
                                                          string startDate, string endDate)
        {

            List<object> amortizations = new List<object>();
            if (startDate == "")
            {
                startDate = "01/01/1900";
            }
            if (endDate == "")
            {
                endDate = "01/01/1900";
            }

            BranchDTO branch = new BranchDTO() { Id = branchId, Description = User.Identity.Name };
            PrefixDTO prefix = new PrefixDTO() { Id = prefixId };
            PolicyDTO policy = new PolicyDTO()
            {
                CurrentFrom = Convert.ToDateTime(startDate),
                CurrentTo = Convert.ToDateTime(endDate),
                DocumentNumber = Convert.ToInt32(policyNumber),
                Id = 0,
                UserId = _commonController.GetUserIdByName(User.Identity.Name),
            };
            IndividualDTO insured = new IndividualDTO() { IndividualId = individualId };

            WorkerFactory.Instance.CreateWorker(operationTypeId, branch, prefix, policy, insured, limitAmount);

            amortizations.Add(new
            {
                ProcessNumber = 1,
                MessageError = "OK"
            });

            return Json(amortizations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetProcessPending
        /// Obtiene los procesos pendientes de Amortizaciones
        /// </summary>
        /// <param name="processTypeId"></param>
        /// <returns>UifTableResult</returns>
        public ActionResult GetProcessPending(int processTypeId)
        {
            List<object> amortizations = new List<object>();
            AmortizationDTO amortization = new AmortizationDTO()
            {
                AmortizationStatus = Convert.ToInt16(AmortizationStatus.Actived),
                Id = -1,
                UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
            };
            amortization = DelegateService.accountingAmortizationService.GetAmortizationById(amortization.Id);

            if (amortization.Policies != null)
            {
                foreach (PolicyDTO amortizationItem in amortization.Policies)
                {
                    var progress = "";

                    if (Convert.ToDateTime(amortizationItem.CurrentTo) == Convert.ToDateTime("01/01/0001 0:00:00"))
                    {
                        progress = @Global.InProcess;
                    }
                    else
                    {
                        progress = @Global.Finalized;
                    }

                    amortizations.Add(new
                    {
                        Id = amortizationItem.Id,
                        UserName = amortizationItem.Agencies[0].Agent.FullName,
                        IsApplied = amortizationItem.Holder.Name,
                        ReceiptNumber = amortizationItem.Prefix.Id,
                        EntryNumber = amortizationItem.BillingGroup.Description,
                        IsActive = amortizationItem.Product.Description,
                        Progress = progress
                    });
                }
            }

            return new UifTableResult(amortizations);
        }

        /// <summary>
        /// GetAmortizationProcessByStatus
        /// Obtiene los procesos pendientes de amortización
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAmortizationProcessByStatus(AmortizationStatus? statusId)
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
            List<AmortizationDTO> amortizations = DelegateService.accountingAmortizationService.GetAmortizations();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;

            List<AmortizationDTO> amortizationOrders;

            if (statusId > 0)
            {
                amortizationOrders = (from amortization in amortizations
                                      where amortization.AmortizationStatus == (int)statusId && amortization.UserId == userId
                                      orderby amortization.Id descending
                                      select amortization).ToList();
            }
            else
            {
                amortizationOrders = (from amortization in amortizations
                                      where amortization.UserId == userId
                                      orderby amortization.Id descending
                                      select amortization).ToList();
            }

            foreach (AmortizationDTO amortization in amortizationOrders)
            {
                var progress = "";

                if (Convert.ToDateTime(amortization.Policies[0].CurrentTo) == Convert.ToDateTime("01/01/0001 0:00:00"))
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
                    EntryNumber = 0,
                    amortization.Id,
                    IsActive = amortization.AmortizationStatus.ToString(),
                    IsApplied = @Global.No,
                    NegativeTotalApplied = amortization.NegativeAppliedTotal.Value < 0 ?
                           amortization.NegativeAppliedTotal.Value.ToString("C") :
                            string.Format(new CultureInfo("en-US"), "{0:C}", amortization.NegativeAppliedTotal.Value),
                    PositiveTotalApplied = string.Format(new CultureInfo("en-US"), "{0:C}", amortization.PositiveAppliedTotal.Value),
                    ProcessDate = amortization.Date.ToString("dd/MM/yyyy H:mm:ss"),
                    Progress = progress,
                    ReceiptNumber = amortization.Policies[0].Endorsement.Id,
                    UserName = DelegateService.uniqueUserService.GetUserById(amortization.UserId).AccountName,
                });
            }

            return Json(new
            {
                aaData = processes,
                total = processes.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAmortizationByProcessNumber
        /// </summary>
        /// <param name="processNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAmortizationByProcessNumber(string processNumber)
        {
            List<object> amortizations = new List<object>();
            AmortizationDTO amortization = new AmortizationDTO()
            {
                AmortizationStatus = Convert.ToInt16(AmortizationStatus.Actived),
                Id = Convert.ToInt32(processNumber),
                NegativeAppliedTotal = new AmountDTO() { Value = 0 }
            };

            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());
            if (userId == 0)
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }
            amortization.UserId = userId;
            amortization = DelegateService.accountingAmortizationService.GetAmortizationById(amortization.Id);
            if (amortization.Policies != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;

                foreach (PolicyDTO policy in amortization.Policies)
                {
                    amortizations.Add(new
                    {
                        Id = policy.Id,
                        BranchId = policy.Branch.Id,
                        Branch = policy.Branch.Description,
                        PrefixId = policy.Prefix.Id,
                        Prefix = policy.Prefix.Description,
                        PolicyId = policy.Id,
                        Policy = policy.DocumentNumber,
                        Endorsement = policy.Endorsement.Number,
                        Insured = policy.Holder.Name,
                        Payer = policy.DefaultBeneficiaries[0].Name,
                        CurrencyId = policy.ExchangeRate.Currency.Id,
                        Currency = policy.ExchangeRate.Currency.Description,
                        ImputationId = policy.BillingGroup.Id,
                        ApplicationReceiptNumber = policy.BillingGroup.Description,
                        EntryNumber = "",
                        ProcessNumber = processNumber,
                        InsuredPersonType = 1,
                        AgentPersonType = 1,
                        PayerPersonType = 1,                        
                        PrincipalAgent = policy.Agencies[0].Agent.FullName,

                        Amount = policy.Agencies[0].Commissions[0].Amount < 0 ?
                           policy.Agencies[0].Commissions[0].Amount.ToString("C") :
                           string.Format(new CultureInfo("en-US"), "{0:C}", policy.Agencies[0].Commissions[0].Amount),

                        Change = String.Format(new CultureInfo("en-US"), "{0:C}", policy.ExchangeRate.SellAmount),

                        LocalAmount = policy.Agencies[0].Commissions[0].CalculateBase < 0 ?
                           policy.Agencies[0].Commissions[0].CalculateBase.ToString("C") :
                           string.Format(new CultureInfo("en-US"), "{0:C}", policy.Agencies[0].Commissions[0].CalculateBase),

                        ItemId = policy.Endorsement.EndorsementReasonId
                    });
                }
            }

            return Json(new
            {
                aaData = amortizations,
                total = amortizations.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetHeaderAmortizationByProcessNumber
        /// Obtiene la cabecera del proceso amortizacion
        /// </summary>
        /// <param name="processNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetHeaderAmortizationByProcessNumber(string processNumber)
        {

            try
            {
                if (Convert.ToInt64(processNumber) > Int32.MaxValue)
                {
                    return Json(new { success = false, result = Global.ValidateEnterNumber }, JsonRequestBehavior.AllowGet);
                }

                List<object> amortizations = new List<object>();
                AmortizationDTO amortization = new AmortizationDTO()
                {
                    AmortizationStatus = Convert.ToInt16(AmortizationStatus.Actived),
                    Id = Convert.ToInt32(processNumber),
                    NegativeAppliedTotal = new AmountDTO() { Value = -1 },
                    UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
                };

                amortization = DelegateService.accountingAmortizationService.GetAmortizationById(amortization.Id);

                if (amortization != null)
                {
                    if (amortization.UserId > 0 && amortization.AmortizationStatus == Convert.ToInt16(AmortizationStatus.Actived))
                    { 
                        amortizations.Add(new
                        {
                            ProcessDate = amortization.Date.ToString("dd/MM/yyyy H:mm:ss"),
                            UserName = DelegateService.uniqueUserService.GetUserById(amortization.UserId).AccountName,
                            TotalCredits = System.Math.Abs(amortization.NegativeAppliedTotal.Value),
                            TotalDebits = System.Math.Abs(amortization.PositiveAppliedTotal.Value)
                        });
                    }
                    else
                    {
                        amortizations.Add(new
                        {
                            UserName = "-1",
                        });
                    }
                }
                return Json(amortizations, JsonRequestBehavior.AllowGet);
            }
            catch (OverflowException)
            {
                return Json(new { success = false, result = Global.ValidateEnterNumber }, JsonRequestBehavior.AllowGet);
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
        /// GetAmortizationByProcessNumberReport
        /// Obtiene la cabecera y detalle de las amorttizaciones pendientes de aplicar
        /// </summary>
        /// <param name="processAmortization"></param>
        public void GetAmortizationByProcessNumberReport(string processAmortization)
        {
            List<AutomaticAmortizationModel> automaticAmortizationModels = new List<AutomaticAmortizationModel>();

            AmortizationDTO amortization = new AmortizationDTO()
            {
                AmortizationStatus = Convert.ToInt16(AmortizationStatus.Actived),
                Id = Convert.ToInt32(processAmortization),
                NegativeAppliedTotal = new AmountDTO() { Value = -1 },
                UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
            };

            // Se obtiene la cabecera
            amortization = DelegateService.accountingAmortizationService.GetAmortizationById(amortization.Id);

            var processDate = "";
            decimal totalCredits = 0;
            decimal totalDebits = 0;

            if (amortization != null && amortization.UserId > 0)
            {
                processDate = amortization.Date.ToString("dd/MM/yyyy H:mm:ss");
                totalCredits = Convert.ToDecimal(amortization.NegativeAppliedTotal.Value);
                totalDebits = Convert.ToDecimal(amortization.PositiveAppliedTotal.Value);

                // Se obtiene el detalle
                amortization = new AmortizationDTO()
                {
                    AmortizationStatus = Convert.ToInt16(AmortizationStatus.Actived),
                    Id = Convert.ToInt32(processAmortization),
                    NegativeAppliedTotal = new AmountDTO() { Value = 0 },
                    UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
                };

                amortization = DelegateService.accountingAmortizationService.GetAmortizationById(amortization.Id);

                foreach (PolicyDTO amortizationItem in amortization.Policies)
                {
                    automaticAmortizationModels.Add(new AutomaticAmortizationModel()
                    {
                        Amount = amortizationItem.Agencies[0].Commissions[0].Amount,
                        ProcessAmortization = Convert.ToInt32(processAmortization),
                        ProcessDate = Convert.ToDateTime(processDate),
                        UserName = User.Identity.Name,
                        TotalDebits = totalDebits,
                        TotalCredits = totalCredits,
                        BranchName = amortizationItem.Branch.Description,
                        PrefixName = amortizationItem.Prefix.Description,
                        Policy = amortizationItem.DocumentNumber.ToString(),
                        Endorsement = amortizationItem.Endorsement.Number,
                        Insured = amortizationItem.Holder.Name,
                        Payer = amortizationItem.DefaultBeneficiaries[0].Name,
                        Currency = amortizationItem.ExchangeRate.Currency.Description,
                        ApplicationReceiptNumber = amortizationItem.BillingGroup.Description,
                        Change = amortizationItem.ExchangeRate.SellAmount,
                        PrincipalAgent = amortizationItem.Agencies[0].Agent.FullName


                    });
                }
            }
         
            TempData["amortizationRptSource"] = automaticAmortizationModels;
            TempData["amortizationReportName"] = "Areas//Accounting//Reports//SpecialProcess//AutomaticAmortizationReport.rpt";
        }

        /// <summary>
        /// ShowAutomaticAmortization
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowAutomaticAmortization()
        {
            try
            {
                bool isValid = true;

                var reportSource = TempData["amortizationRptSource"];
                var reportName = TempData["amortizationReportName"];

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
                        //Lena Reporte Principal
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "AutomaticAmortizationReport");

                    TempData["amortizationRptSource"] = null;
                    TempData["amortizationReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LowAutomaticAmortization
        /// Dar de baja al proceso de amortizaciones
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult LowAutomaticAmortization(int processId)
        {
            List<object> amortizations = new List<object>();
            AmortizationDTO amortization = new AmortizationDTO()
            {
                AmortizationStatus = Convert.ToInt16(AmortizationStatus.Rejected),
                Id = processId,
                NegativeAppliedTotal = new AmountDTO() { Value = 0 },
                UserId = _commonController.GetUserIdByName(User.Identity.Name)
            };

            amortization = DelegateService.accountingAmortizationService.UpdateAmortization(amortization);
            amortizations.Add(new
            {
                ProcessNumber = amortization.Id,
                MessageError = amortization.Policies.Count > 0 ? amortization.Policies[0].TemporalTypeDescription : ""
            });

            return Json(amortizations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAutomaticAmortization
        /// Elimina las notas de crédito temporales 
        /// </summary>
        /// <param name="itemsToAppliedModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteAutomaticAmortization(ItemsAmortizationToAppliedModel itemsToAppliedModel)
        {
            List<object> amortizations = new List<object>();
            int count = 0;
            try
            {

                AmortizationDTO amortization = new AmortizationDTO()
                {
                    AmortizationStatus = Convert.ToInt16(AmortizationStatus.Actived),
                    NegativeAppliedTotal = new AmountDTO() { Value = 0 },
                    UserId = _commonController.GetUserIdByName(User.Identity.Name)
                };

                if (itemsToAppliedModel.AmortizationItem != null)
                {
                    foreach (AmortizationItemModel automaticAmortization in itemsToAppliedModel.AmortizationItem)
                    {
                        List<PolicyDTO> policies = new List<PolicyDTO>();

                        amortization.Id = automaticAmortization.ProcessId;
                        PolicyDTO policy = new PolicyDTO();
                        policy.Agencies = new List<IssuanceAgencyDTO>()
                            {
                                new IssuanceAgencyDTO()
                                {
                                     Commissions = new List<IssuanceCommissionDTO>()
                                     {
                                         new IssuanceCommissionDTO()
                                         {
                                             Amount = Convert.ToDecimal(automaticAmortization.Amount),
                                             CalculateBase = Convert.ToDecimal(automaticAmortization.IncomeAmount)
                                         }
                                     },
                                }
                            };

                        policy.Branch = new BranchDTO()
                        {
                            Id = automaticAmortization.BranchId,
                            Description = automaticAmortization.BranchName
                        };
                        policy.Endorsement = new EndorsementDTO()
                        {
                            Id = automaticAmortization.AmortizationItemId,
                            Number = automaticAmortization.EndorsementNumber
                        };
                        policy.ExchangeRate = new ExchangeRateDTO()
                        {
                            Currency = new CurrencyDTO()
                            {
                                Id = automaticAmortization.CurrencyId,
                                Description = automaticAmortization.CurrencyName
                            },
                            SellAmount = Convert.ToDecimal(automaticAmortization.Exchange)
                        };
                        policy.Id = automaticAmortization.ImputationId;

                        policies.Add(policy);

                        amortization.Policies = policies;
                        amortization = DelegateService.accountingAmortizationService.UpdateAmortization(amortization);

                        if (amortization.Policies[0].TemporalTypeDescription == "" || amortization.Policies[0].TemporalTypeDescription == null)
                        {
                            count++;
                        }
                        else
                        {
                            return Json(new { success = false, result = amortization.Policies[0].TemporalTypeDescription }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }

                amortizations.Add(new
                {
                    ProcessNumber = amortization.Id,
                    MessageError = amortization.Policies[0].TemporalTypeDescription
                });
                return Json(new { success = true, result = amortizations }, JsonRequestBehavior.AllowGet);
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
        /// ApplyAmortizations
        /// Aplicación de las amortizaciones
        /// </summary>
        /// <param name="itemsToAppliedModel"></param>
        /// <param name="totalDebits"></param>
        /// <param name="totalCredits"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ApplyAmortizations(ItemsAmortizationToAppliedModel itemsToAppliedModel, string totalDebits, string totalCredits)
        {
            string amortizationMessage = "";
            List<object> amortizations = new List<object>();
            try
            {
                AmortizationDTO amortization = new AmortizationDTO();
                List<PolicyDTO> policies = new List<PolicyDTO>();

                amortization.AmortizationStatus = Convert.ToInt16(AmortizationStatus.Applied);
                amortization.NegativeAppliedTotal = new AmountDTO() { Value = decimal.Parse(totalDebits, CultureInfo.InvariantCulture.NumberFormat) };
                amortization.PositiveAppliedTotal = new AmountDTO() { Value = decimal.Parse(totalCredits, CultureInfo.InvariantCulture.NumberFormat) };
                amortization.UserId = _commonController.GetUserIdByName(User.Identity.Name);

                if (itemsToAppliedModel.AmortizationItem != null)
                {
                    amortization.Id = itemsToAppliedModel.AmortizationItem[0].ProcessId;
                    PolicyDTO policy = new PolicyDTO();
                    policy.Id = itemsToAppliedModel.AmortizationItem[0].ImputationId;
                    policies.Add(policy);

                }

                amortization.Policies = policies;
                amortization = DelegateService.accountingAmortizationService.ApplyAmortization(amortization);

                #region Accounting

                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    if (amortization.Policies[0].TemporalTypeDescription == "" || amortization.Policies[0].TemporalTypeDescription == null)
                    {
                        amortizationMessage = _journalEntryController.RecordJournalEntry(amortization.Policies[0].PolicyType.Id, amortization.UserId);
                        if (amortizationMessage == Global.AccountingIntegrationUnbalanceEntry ||
                            amortizationMessage == Global.EntryRecordingError)
                        {
                            return Json(new { success = false, result = amortizationMessage}, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else {
                        return Json(new { success = false, result = amortization.Policies[0].TemporalTypeDescription }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                else
                {
                    amortizationMessage = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                }

                #endregion Accounting


                if (amortizationMessage.IndexOf(':') == -1)
                {
                   return Json(new { success = false, result = amortizationMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    amortizations.Add(new
                    {
                        ProcessNumber = amortization.Id,
                        ReceiptNumber = amortizationMessage.Substring(amortizationMessage.IndexOf(':'), amortizationMessage.Length - amortizationMessage.IndexOf(':')),// itemsToAppliedModel.AmortizationItem[0].ImputationReceiptNumber,
                        MessageError = Convert.ToString(amortizationMessage),
                    });
                    return Json(new { success = true, result = amortizations }, JsonRequestBehavior.AllowGet);
                }
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
        #endregion



    }
}