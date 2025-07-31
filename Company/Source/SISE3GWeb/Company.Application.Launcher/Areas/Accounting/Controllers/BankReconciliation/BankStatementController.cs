using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Excel;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.Reconciliation;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Application.ReportingServices;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.UniquePersonService.V1.Models;

//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.BankReconciliation
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class BankStatementController : Controller
    {
        #region Class

        /// <summary>
        /// SelectDTO
        /// </summary>
        public class SelectDto
        {
            public int Id
            {
                get; set;
            }
            public string Description
            {
                get; set;
            }
        }

        #endregion

        #region Instance Variables


        //private readonly ICommonService _commonService = ServiceManager.Instance.GetService<ICommonService>();
        //private readonly IParameterService _parameterService = ServiceManager.Instance.GetService<IParameterService>();
        //private readonly IReconciliationService _reconciliationService = ServiceManager.Instance.GetService<IReconciliationService>();
        //private readonly IAccountingService _generalLedgerService = ServiceManager.Instance.GetService<IAccountingService>();
        //private readonly IReportingService _reportingService = ServiceManager.Instance.GetService<IReportingService>();

        readonly CommonController _commonController = new CommonController();

        List<FormatField> _bankAccountFormats = new List<FormatField>();
        List<FormatField> _bankNameFormats = new List<FormatField>();
        List<FormatField> _bankSendDateFormats = new List<FormatField>();
        List<FormatField> _bankMovementTypeFormats = new List<FormatField>();
        List<FormatField> _bankVoucherNumberFormats = new List<FormatField>();
        List<FormatField> _bankMovementDateFormats = new List<FormatField>();
        List<FormatField> _bankAmountFormats = new List<FormatField>();
        List<FormatField> _bankDescriptionFormats = new List<FormatField>();
        List<FormatField> _bankBranchDescriptionFormats = new List<FormatField>();
        List<FormatField> _bankIndividualFormats = new List<FormatField>();


        bool _head = true;

        #endregion

        #region Views

        /// <summary>
        /// MainBankStatement
        /// Pantalla Estracto Bancario
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankStatement()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }       
        }

        /// <summary>
        /// MainBankFileFormat
        /// Pantalla de Formato de archivo
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankFileFormat()
        {
            try
            {
                ViewBag.ControlFieldExtractBank = ConfigurationManager.AppSettings["ControlFieldExtractBank"];

                return View("~/Areas/Accounting/Views/Parameters/BankReconciliation/MainBankFileFormat.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }       
        }

        /// <summary>
        /// MainReportsReconciliation
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainReportsReconciliation()
        {
            try
            {
                return View("~/Areas/Accounting/Views/BankStatement/MainReportsReconciliation.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        /// <summary>
        /// MainReconciliationFormat
        /// Pantalla asignación formato conciliación a cuenta bancaria compañía
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainReconciliationFormat()
        {
            try
            {
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
        /// GetBanks
        /// Obtiene bancos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBanks()
        {
            return new UifSelectResult(DelegateService.commonService.GetBanks());
        }

        /// <summary>
        /// GetAccountTypes
        /// Obtiene tipo de cuentas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountTypes()
        {
            List<BankAccountTypeDTO> bankAccountTypes =  DelegateService.accountingParameterService.GetBankAccountTypes();

            return new UifSelectResult(bankAccountTypes);
        }

        /// <summary>
        /// GetAccountNumbers
        /// Obtiene números de cuenta
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="accountTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountNumbers(int bankId, int accountTypeId)
        {
            List<SelectDto> companyAccountNumbers = new List<SelectDto>();

            List<BankAccountCompanyDTO> bankAccountCompanies = _commonController.GetCompanyBankAccountsByBankId(bankId);

            foreach (BankAccountCompanyDTO bankAccountCompany in bankAccountCompanies)
            {
                if (bankAccountCompany.BankAccountType.Id == accountTypeId)
                {
                    companyAccountNumbers.Add(new SelectDto
                    {
                        Id = bankAccountCompany.Id,
                        Description = bankAccountCompany.Number
                    });
                }
            }

            return new UifSelectResult(companyAccountNumbers);
        }

        /// <summary>
        /// GetBranchs
        /// Obtiene Ramo
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBranchs()
        {
            List<SelectDto> branches = new List<SelectDto>();

            List<Branch> branchs = _commonController.GetBranchesByUserId(_commonController.GetUserIdByName(User.Identity.Name.ToUpper()));

            if (branchs.Count > 0)
            {
                foreach (Branch branch in branchs)
                {
                    branches.Add(new SelectDto
                    {
                        Id = branch.Id,
                        Description = branch.Description
                    });
                }
            }

            return new UifSelectResult(branches);
        }


        /// <summary>
        /// GetMovementTypes
        /// Obtiene tipo de movimientos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetMovementTypes()
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes());
        }

        /// <summary>
        /// GetConciliationReportsOptions
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetConciliationReportsOptions()
        {
            List<object> reconciliationOptions = new List<object>();

            reconciliationOptions.Add(new
            {
                Id = 1,
                Description = "Conciliados/Todos los Movimientos"
            });
            reconciliationOptions.Add(new
            {
                Id = 2,
                Description = "Conciliados/Del Banco"
            });
            reconciliationOptions.Add(new
            {
                Id = 3,
                Description = "Conciliados/De SISE"
            });
            reconciliationOptions.Add(new
            {
                Id = 4,
                Description = "Pendientes Internos/Todos"
            });
            reconciliationOptions.Add(new
            {
                Id = 5,
                Description = "Pendientes Internos/Contabilidad Diaria"
            });
            reconciliationOptions.Add(new
            {
                Id = 6,
                Description = "Pendientes Internos/Contabilidad Central"
            });
            reconciliationOptions.Add(new
            {
                Id = 7,
                Description = "Pendientes Bancos"
            });
            reconciliationOptions.Add(new
            {
                Id = 8,
                Description = "Provisión Conciliación"
            });

            return new UifSelectResult(reconciliationOptions);
        }

        /// <summary>
        /// GetPendingBanks
        /// Llena el reporte con los datos de los bancos pendientes
        /// </summary>
        /// <param name="option"></param>
        /// <param name="bankId"></param>
        /// <param name="accountType"></param>
        /// <param name="accountNumber"></param>
        /// <param name="dateTo"></param>
        public void GetPendingBanks(int option, int bankId, int accountType, string accountNumber, string dateTo)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Bank = new Sistran.Core.Application.AccountingServices.DTOs.BankDTO() { Id = bankId },
                BankAccountType = new BankAccountTypeDTO() { Id = accountType  },
                Id = -1,
                Number = accountNumber
            };

            List<Statement> statements =   DelegateService.reconciliationService.GetFailedBankStatementsByAccountBank(bankAccountCompany, Convert.ToDateTime(dateTo));

            List<PendingBanksModel> pendingBanksModels = new List<PendingBanksModel>();

            foreach (Statement statement in statements)
            {
                pendingBanksModels.Add(new PendingBanksModel()
                {
                    BankId = statement.BankAccountCompany.Bank.Id,
                    BankName = statement.BankAccountCompany.Bank.Description,
                    AccountType = statement.BankAccountCompany.BankAccountType.Id,
                    AccountTypeName = statement.BankAccountCompany.BankAccountType.Description,
                    BranchCode = statement.Branch.Id,
                    BranchName = statement.Branch.Description,
                    UserName = User.Identity.Name,
                    Date = statement.Date.ToString("dd/MM/yyyy"),
                    VoucherNumber = statement.DocumentNumber,
                    MovementCode = statement.ProcessNumber,
                    Amount = statement.LocalAmount.Value.ToString(),
                    Currency = statement.BankAccountCompany.Currency.Description,
                    MovementDescription = statement.Description,
                    AccountNumber = statement.BankAccountCompany.Number,
                    ReconciliationNumber = statement.ReconciliationMovementType.Id,
                    ReconciliationType = statement.ReconciliationMovementType.Description,
                    ReconciliationDate = statement.ProcessDate.ToString("dd/MM/yyyy"),
                    UserCode = userId,
                    EndDate = dateTo,
                });
            }

            TempData["billRptSource"] = pendingBanksModels;

            if (bankAccountCompany.Id == 7)
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports//Reconciliation//PendingBanksReconciliation.rpt";
            }
            else if (bankAccountCompany.Id == 5)
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports//Reconciliation//PendingDailyAccountingReconciliation.rpt";
            }
            else if (bankAccountCompany.Id == 2)
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports//Reconciliation//ReconciledBankMovements.rpt";
            }
            else if (bankAccountCompany.Id == 4)
            {
                TempData["BillingReportName"] = "Areas//Accounting//Reports//Reconciliation//AllPendingInternal.rpt";
            }
        }

        /// <summary>
        /// GetConciliatedMovements
        /// Llena el reporte todos los movimientos conciliados
        /// </summary>
        /// <param name="option"></param>
        /// <param name="bankId"></param>
        /// <param name="accountType"></param>
        /// <param name="accountNumber"></param>
        /// <param name="dateTo"></param>
        public void GetConciliatedMovements(int option, int bankId, int accountType,
                                            string accountNumber, string dateTo)
        {
            List<ConciliatedMovementsModel> reconciliationMovements = new List<ConciliatedMovementsModel>();

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = bankId,
                Number = accountNumber
            };
            Sistran.Core.Application.AccountingServices.DTOs.BankDTO bankUser = new Sistran.Core.Application.AccountingServices.DTOs.BankDTO()
            {
                Id = _commonController.GetUserIdByName(User.Identity.Name),
                Description = User.Identity.Name.ToUpper()
            };
            bankAccountCompany.Bank = bankUser;

            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            List<Reconciliation> reconciliations =DelegateService.reconciliationService.GetReconciliationsByStatementTypes(bankAccountCompany, Convert.ToDateTime(dateTo), false, false, false, userId);

            foreach (Reconciliation reconciliation in reconciliations)
            {
                ConciliatedMovementsModel movement = new ConciliatedMovementsModel();
                movement.AccountNumber = reconciliation.BankStatements[0].BankAccountCompany.Number;
                movement.AccountType = reconciliation.BankStatements[0].BankAccountCompany.BankAccountType.Description;
                movement.AmountBankMovements = Convert.ToDecimal(reconciliation.BankStatements[0].LocalAmount.Value);
                movement.BankName = reconciliation.BankStatements[0].BankAccountCompany.Bank.Description;
                movement.ConciliationId = reconciliation.Id;
                movement.DateBankMovements = reconciliation.BankStatements[0].Date;
                movement.DateConciliation = reconciliation.Date;
                movement.ConciliationMovementTypeBankMovements = reconciliation.BankStatements[0].ReconciliationMovementType.Id;
                movement.Type = reconciliation.ReconciliationType == ReconciliationTypes.Automatic ? "AUTOMATICA" : "MANUAL";
                movement.VoucherBankMovements = reconciliation.BankStatements[0].DocumentNumber;

                foreach (Statement statement in reconciliation.CompanyStatements)
                {
                    if (statement.StatementType == StatementTypes.CentralAccounting)
                    {
                        movement.AmountAccountingMovement = Convert.ToDecimal(statement.LocalAmount.Value);
                        movement.DateAccountingMovement = statement.Date;
                        movement.ConciliationMovementTypeAccountingMovement = statement.ReconciliationMovementType.Id;
                        movement.VoucherAccountingMovement = statement.DocumentNumber;
                    }
                    else if (statement.StatementType == StatementTypes.DailyAccounting)
                    {
                        movement.AmountDailyAccountingMovement = Convert.ToDecimal(statement.LocalAmount.Value);
                        movement.DateDailyAccountingMovement = statement.Date;
                        movement.ConciliationMovementTypeDailyAccountingMovement = statement.ReconciliationMovementType.Id;
                        movement.VoucherDailyAccountingMovement = statement.DocumentNumber;
                    }
                }
                reconciliationMovements.Add(movement);
            }

            TempData["billRptSource"] = reconciliationMovements;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//Reconciliation//ReconciliationMovements.rpt";
        }

        /// <summary>
        /// ShowPendingBanks
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowPendingBanks()
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
                    //Lena Reporte Principal
                    reportDocument.SetDataSource(reportSource);

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "PendingBanksReconciliation");

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
        /// GetBankStatementsByAccounBankId
        /// Obtiene estracto bancario por número de cuenta y id de banco
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="bankName"></param>
        /// <param name="processDate"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankStatementsByAccounBankId(int accountBankId, string bankName, string processDate)
        {
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
            bankAccountCompany.Id = accountBankId;
            bankAccountCompany.IsDefault = false;

            List<Statement> statements = DelegateService.reconciliationService.GetBankStatementsByAccountBank(bankAccountCompany, Convert.ToDateTime(processDate));

            List<BankStatementModel> bankStatements = new List<BankStatementModel>();

            foreach (Statement statement in statements)
            {
                bankStatements.Add(new BankStatementModel()
                {
                    BankStatementId = Convert.ToInt32(statement.Id),
                    BankDescription = bankName,
                    BankId = accountBankId,
                    BankingMovementTypeDescription = statement.ReconciliationMovementType.Description,
                    BankingMovementTypeId = statement.ReconciliationMovementType.Id,
                    BranchDescription = statement.Branch.Description,
                    BranchId = statement.Branch.Id,
                    MovementAmount = statement.LocalAmount.Value.ToString(),
                    MovementDate = statement.Date.ToString("dd/MM/yyyy"),
                    MovementDescription = statement.Description,
                    MovementThird = statement.ThirdPerson.FullName,
                    VoucherNumber = statement.DocumentNumber
                });
            }

            return Json(new
            {
                aaData = bankStatements,
                total = bankStatements.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDateProcess
        /// Obtiene el estado de progreso de un proceso
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDateProcess(int accountBankId)
        {
            DateTime processDate = DateTime.Now;
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
            bankAccountCompany.Id = accountBankId;
            bankAccountCompany.IsDefault = true;

            List<Statement> statements =  DelegateService.reconciliationService.GetBankStatementsByAccountBank(bankAccountCompany, processDate);

            List<SelectDto> statementSelects = new List<SelectDto>();

            if (statements.Count > 0)
            {
                foreach (Statement statement in statements)
                {
                    statementSelects.Add(new SelectDto()
                    {
                        Id = statement.Status,
                        Description = statement.ProcessDate.ToString("dd/MM/yyyy HH:mm")
                    });
                }
            }
            List<SelectDto> statementsResponses = statementSelects.GroupBy(p => new { p.Description }).Select(g => g.First()).ToList();

            return new UifSelectResult(statementsResponses);
        }

        /// <summary>
        /// GetDocumentFormat 
        /// Consulta el tipo de formato y validar si es Excel o txt
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDocumentFormat(int accountBankId)
        {
            DateTime processDate = Convert.ToDateTime("01/01/1900");

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = accountBankId,
                IsDefault = true
            };

            List<Statement> statements = DelegateService.reconciliationService.GetBankStatementsByAccountBank(bankAccountCompany, processDate);

            List<SelectDto> status = new List<SelectDto>();

            if (statements.Count > 0)
            {
                status.Add(new SelectDto()
                {
                    Id = statements[0].Status
                });
            }

            return new UifSelectResult(status);
        }

        /// <summary>
        /// ReadFileInMemory
        /// Lee archivo en memoria
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="accountBankId"></param>
        /// <param name="fileType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile, int? accountBankId, int fileType)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if ((fileType == 0) && (data[1] == "xls" || data[1] == "xlsx"))
            {
                return ReadExcelToStream(uploadedFile, Convert.ToInt32(accountBankId));
            }

            if ((fileType == 1) && (data[1] == "txt"))
            {
                return ReadTextFileToStream(uploadedFile, Convert.ToInt32(accountBankId));
            }
            else
            {
                fileLocationName = "BadFileExtension";
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AddBankStatement
        /// Añadir estracto bancario
        /// </summary>
        /// <param name="bankStatementModel"></param>
        /// <param name="processDate"></param>
        /// <param name="processId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AddBankStatement(BankStatementModel bankStatementModel, DateTime processDate, int processId)
        {
            string isSavedBankStatements = "true";
            Statement bankStatements = SetStatements(bankStatementModel, processDate, processId);
            List<Statement> statements = new List<Statement>();
            statements.Add(bankStatements);
            try
            {
                DelegateService.reconciliationService.SaveBankStatements(statements);
            }
            catch (Exception)
            {
                isSavedBankStatements = "Exception";
            }

            return Json(isSavedBankStatements, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateBankStatement
        /// Actualiza estracto bancario
        /// </summary>
        /// <param name="bankStatementModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateBankStatement(BankStatementModel bankStatementModel)
        {
            Statement bankStatements = SetStatements(bankStatementModel, DateTime.Now, 1);
            DelegateService.reconciliationService.UpdateBankStatement(bankStatements);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBankStatement
        /// Borrar estracto bancario
        /// </summary>
        /// <param name="bankStatementModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteBankStatement(BankStatementModel bankStatementModel)
        {
            Statement bankStatements = SetStatements(bankStatementModel, DateTime.Now, 1);
            DelegateService.reconciliationService.DeleteBankStatement(bankStatements);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FileFormat


        /// <summary>
        /// GetFormatHeaderByAccountBankId
        /// Obtiene la cabecera de la cuenta bancaria por número de cuenta
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="formatTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetFormatHeaderByAccountBankId(int accountBankId, int formatTypeId)
        {
            List<object> formats = new List<object>();

            formats.Add(new
            {
                FormatId = -1,
                Description = "",
                Separator = ""
            });

            return Json(formats, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetFormatBankByBankId
        /// Obtiene formato de banco por id de banco
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="fileType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetFormatBankByBankId(int accountBankId, int fileType)
        {
            List<object> fields = new List<object>();

            return Json(new
            {
                aaData = fields,
                total = fields.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveBankFieldFormat
        /// Graba Formato de archivo del banco
        /// </summary>
        /// <param name="fieldFormatModel"></param>
        /// <param name="operationType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBankFieldFormat(BankFieldFormatModels fieldFormatModel, string operationType)
        {
            List<object> fieldFormats = new List<object>();

            fieldFormats.Add(new
            {
                FieldFormatCode = 1,
                MessageError = ";"
            });

            return Json(fieldFormats, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBankFieldFormat
        /// Borra archivo de formato de banco
        /// </summary>
        /// <param name="fieldFormatModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteBankFieldFormat(BankFieldFormatModels fieldFormatModel, string operationType)
        {
            List<object> fieldFormats = new List<object>();
            fieldFormats.Add(new
            {
                FieldFormatCode = 0,
                MessageError = Global.YouCanNotDeleteTheRecord
            });

            return Json(fieldFormats, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetReconciliationFormats 
        /// Obtiene los formatos de conciliación bancaria
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetReconciliationFormats()
        {

            List<ReconciliationFormat> reconciliationFormats = DelegateService.reconciliationService.GetReconciliationFormats();

            var formats = from items in reconciliationFormats
                          select new
                          {
                              Id = items.Id,
                              BankAccountCompanyId = items.Bank.Id,
                              BankAccountCompanyName = items.Bank.Description,
                              FormatId = items.Format.Id,
                              FormatName = items.Format.Description,
                          };

            return new UifTableResult(formats);
        }

        /// <summary>
        /// SaveReconciliationFormat
        /// Graba / actualiza la asignación de formato de conciliación a cuenta bancaria
        /// </summary>
        /// <param name="reconciliationFormatModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveReconciliationFormat(ReconciliationFormatModel reconciliationFormatModel)
        {
            List<object> reconciliationFormats = new List<object>();

            ReconciliationFormat reconciliationFormat = new ReconciliationFormat();

            reconciliationFormat.Id = reconciliationFormatModel.Id;
            reconciliationFormat.Bank = new Bank() { Id = reconciliationFormatModel.BankAccountCompanyId };
            reconciliationFormat.Format = new Format()
            {
                FileType = FileTypes.Excel,
                Id = reconciliationFormatModel.FormatId
            };

            if (reconciliationFormatModel.OperationType.Equals("I"))
            {
                try
                {
                    DelegateService.reconciliationService.SaveReconciliationFormat(reconciliationFormat);
                    reconciliationFormats.Add(new
                    {
                        ReconciliationFormatCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    reconciliationFormats.Add(new
                    {
                        ReconciliationFormatCode = -1,
                        MessageError = Global.MessageErrorSaveReconciliation + ex.Message
                    });
                }
            }
            if (reconciliationFormatModel.OperationType.Equals("U"))
            {
                try
                {
                    DelegateService.reconciliationService.UpdateReconciliationFormat(reconciliationFormat);
                    reconciliationFormats.Add(new
                    {
                        ReconciliationFormatCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    reconciliationFormats.Add(new
                    {
                        ReconciliationFormatCode = -1,
                        MessageError = Global.MessageErrorUpdateReconciliation + ex.Message
                    });
                }
            }

            return Json(reconciliationFormats, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteReconciliationFormat
        /// Borra la asignación de formatos de conciliación a cuenta bancaria
        /// </summary>
        /// <param name="reconciliationFormatModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteReconciliationFormat(ReconciliationFormatModel reconciliationFormatModel)
        {
            List<object> reconciliationFormats = new List<object>();

            try
            {
                if (reconciliationFormatModel.OperationType.Equals("D"))
                {
                    DelegateService.reconciliationService.DeleteReconciliationFormat(reconciliationFormatModel.Id);
                    reconciliationFormats.Add(new
                    {
                        ReconciliationFormatCode = 0,
                    });
                }
            }
            catch (Exception ex)
            {
                reconciliationFormats.Add(new
                {
                    ReconciliationFormatCode = -1,
                    MessageError = Global.MessageErrorUpdateReconciliation + ex.Message
                });
            }

            return Json(reconciliationFormats, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Method Private

        /// <summary>
        /// SetStatements
        /// Enviar estracto
        /// </summary>
        /// <param name="bankStatementModel"></param>
        /// <param name="processDate"></param>
        /// <param name="processId"></param>
        /// <returns>Statement</returns>
        private Statement SetStatements(BankStatementModel bankStatementModel, DateTime processDate, int processId)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            Statement bankStatements = new Statement();

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = bankStatementModel.BankId,
            };


            Branch branch = new Branch()
            {
                Id = bankStatementModel.BranchId,
                Description = bankStatementModel.BranchDescription,
            };

            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO()
            {
                Id = bankStatementModel.BankingMovementTypeId,
                Description = bankStatementModel.BankingMovementTypeDescription
            };

            Amount amount = new Amount() { Value = Convert.ToDecimal(bankStatementModel.MovementAmount) };
            Amount localAmount = new Amount() { Value = Convert.ToDecimal(bankStatementModel.MovementAmount) };

            Individual individual = new Individual()
            {
                CustomerTypeDescription = "",
                FullName  = String.IsNullOrEmpty(bankStatementModel.MovementThird) ? "" : bankStatementModel.MovementThird
            };
            bankStatements.BankAccountCompany = bankAccountCompany;
            bankStatements.Id = userId;
            bankStatements.Branch = branch;
            bankStatements.StatementType = StatementTypes.Bank;
            bankStatements.Date = Convert.ToDateTime(bankStatementModel.MovementDate);
            bankStatements.ReconciliationMovementType = reconciliationMovementType;
            bankStatements.DocumentNumber = bankStatementModel.VoucherNumber;
            bankStatements.Amount = amount;
            bankStatements.LocalAmount = localAmount;
            bankStatements.ThirdPerson = individual;
            bankStatements.Description = bankStatementModel.MovementDescription;
            bankStatements.Id = bankStatementModel.BankStatementId;
            bankStatements.Status = processId;
            bankStatements.ProcessDate = processDate;
            bankStatements.UserId = userId;

            return bankStatements;
        }

        /// <summary>
        /// GetFailedBankStatementsByAccountBank
        /// Obtiene estracto con errores por cuenta bancaria
        /// </summary>
        /// <param name="bankAccountCompany">Cuenta Bancaria</param>
        /// <param name="processDate">Fecha de Proceso</param>
        /// <returns>List<BankStatementModel/></returns>
        private List<BankStatementModel> GetFailedBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime processDate)
        {
            List<BankStatementModel> bankStatements = new List<BankStatementModel>();

            List<Statement> statements =  DelegateService.reconciliationService.GetFailedBankStatementsByAccountBank(bankAccountCompany, processDate);

            if (statements.Count > 0)
            {
                string[] parts;

                foreach (Statement statement in statements)
                {
                    parts = statement.ThirdPerson.CustomerTypeDescription.ToString().Split(new string[] { ":" }, StringSplitOptions.None);

                    if (parts.Length > 0)
                    {
                        if (parts[0] == "MovementTypeDescription")
                        {
                            statement.ThirdPerson.CustomerTypeDescription = @Global.MovementTypeNotConfigured + parts[2];
                            statement.ReconciliationMovementType.Description = parts[2];
                        }
                        if (parts[0] == "BranchDescription")
                        {
                            statement.ThirdPerson.CustomerTypeDescription = @Global.AgencyNotConfigured + parts[2];
                            statement.Branch.Description = parts[2];
                        }
                        if (parts[0] == "Duplicate")
                        {
                            statement.ThirdPerson.CustomerTypeDescription = @Global.Duplicate;
                        }
                    }

                    bankStatements.Add(new BankStatementModel()
                    {
                        BankStatementId = Convert.ToInt32(statement.Id),
                        BankDescription = "",
                        BankId = statement.BankAccountCompany.Id,
                        BankingMovementTypeDescription = statement.ReconciliationMovementType.Description,
                        BankingMovementTypeId = statement.ReconciliationMovementType.Id,
                        BranchDescription = statement.Branch.Description,
                        BranchId = statement.Branch.Id,
                        MovementAmount = statement.LocalAmount.Value.ToString(),
                        MovementDate = statement.Date.ToString("dd/MM/yyyy") == "01/01/1900" ? "" : statement.Date.ToString("dd/MM/yyyy"),
                        MovementDescription = statement.Description,
                        MovementThird = statement.ThirdPerson.FullName,
                        MovementOrigin = statement.ThirdPerson.CustomerTypeDescription,
                        VoucherNumber = statement.DocumentNumber
                    });
                }
            }

            return bankStatements;
        }

        /// <summary>
        /// GetBranchIdByDescription
        /// </summary>
        /// <param name="description"></param>
        /// <returns>int</returns>
        private int GetBranchIdByDescription(string description)
        {
            int branchId = 0;
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());
            List<Branch> branches = _commonController.GetBranchesByUserId(userId).Where(b => b.Description.Equals(description)).ToList();

            foreach (Branch branch in branches)
            {
                branchId = branch.Id;
            }

            return branchId;
        }

        ///<summary>
        ///ExcelToStream
        ///Excel 
        ///</summary>
        ///<param name="uploadedFile"></param>
        ///<param name="bankAccountId"></param>
        ///<returns>JsonResult</returns>
        private JsonResult ReadExcelToStream(HttpPostedFileBase uploadedFile, int bankAccountId)
        {
            bool successful = true;
            string fileLocationName = "";
            Byte[] arrayContent;

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);

            IExcelDataReader excelReader;
            List<BankStatementModel> statements = new List<BankStatementModel>();

            int dataCount = 0;

            try
            {
                if (data[1] == "xls")
                {
                    //1. Lee desde binary Excel  ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else
                {
                    //2. Lee desde binary OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                //3. DataSet - El resultado sera creado en result.Tables
                DataSet result = excelReader.AsDataSet();

                int rowGeneric;
                int length;
                bool validateHeader = false;
                bool emptyField = true;

                string bankAccountNumber = GetBankAccountNumberByBankAccountCompanyId(bankAccountId);
                string bankName = GetBankNameByBankAccountCompanyId(bankAccountId);

                Format format = new Format()
                {
                    Id =  DelegateService.reconciliationService.GetReconciliationFormat(bankAccountId),
                    FileType = FileTypes.Text
                };
                List<FormatDetail> formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);

                if (formatDetails.Count > 0)
                {
                    //Se crean las listas de campos
                    SettingFormatsByBank(formatDetails);

                    bool validateStatement = false;
                    bool saveBankStatement = true;

                    DateTime processDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                    Parameter parameter = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ReconciliationProcessNumber"]));
                    int processId = (int)parameter.NumberParameter;
                    parameter.NumberParameter = processId + 1;
                    DelegateService.commonService.UpdateParameter(parameter);

                    int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

                    //Se utiliza el campo number para enviar el nombre del usuario
                    BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO() { Id = bankAccountId, Number = User.Identity.Name.ToUpper() };

                    for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = result.Tables[0].Rows[i];
                        List<Statement> bankStatements = new List<Statement>();
                        Statement statement = new Statement();
                        Individual thirdPerson = new Individual() { CustomerTypeDescription = "" };

                        //Lectura y validación de cabecera
                        if (!_head)
                        {
                            if (_bankAccountFormats.Count > 0)
                            {
                                //El número de fila siempre debe ser mayor que cero
                                if (_bankAccountFormats[0].RowNumber == 0)
                                {
                                    successful = false;
                                    fileLocationName = "InvalidFileNumber";
                                    break;
                                }

                                if (i + 1 == (_bankAccountFormats[0].RowNumber))
                                {
                                    rowGeneric = _bankAccountFormats[0].ColumnNumber - 1;

                                    if (row[rowGeneric].ToString() != "")
                                    {
                                        //Se valida el número de cuenta bancaria
                                        if (bankAccountNumber != Convert.ToString(row[rowGeneric]))
                                        {
                                            successful = false;
                                            fileLocationName = "InvalidBankAccountNumber";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        successful = false;
                                        fileLocationName = "fieldEmpty";
                                        break;
                                    }
                                    validateHeader = true;
                                }
                            }
                            //Nombre banco
                            if ((_bankNameFormats.Count > 0) && (i + 1 == (_bankNameFormats[0].RowNumber)) && _bankNameFormats[0].IsRequired)
                            {
                                rowGeneric = _bankNameFormats[0].ColumnNumber - 1;

                                if (row[rowGeneric].ToString() == "")
                                {
                                    validateStatement = true;
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.BankNameRequired + "; ";
                                }
                                else
                                {
                                    //Se valida el nombre del banco al que corresponde la cuenta bancaria
                                    if (bankName != Convert.ToString(row[rowGeneric]))
                                    {
                                        successful = false;
                                        fileLocationName = "InvalidBankName";
                                        break;
                                    }
                                }
                            }

                            //Fecha de envio
                            if ((_bankSendDateFormats.Count > 0) && (i + 1 == (_bankSendDateFormats[0].RowNumber)) && _bankSendDateFormats[0].IsRequired)
                            {
                                rowGeneric = _bankSendDateFormats[0].ColumnNumber - 1;
                                DateTime dateValue;

                                if (row[rowGeneric].ToString() != "")
                                {
                                    if (DateTime.TryParse(row[rowGeneric].ToString(), out dateValue))
                                    {
                                        statement.Date = Convert.ToDateTime("01/01/1900");
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.WrongDateFormat + ": " + row[rowGeneric].ToString();
                                        validateStatement = true;
                                    }
                                }
                                else
                                {
                                    statement.Date = Convert.ToDateTime("01/01/1900");
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.SendDateRequired + "; ";
                                    validateStatement = true;
                                }
                            }
                            if (_bankMovementTypeFormats.Count > 0 && (i + 2 == (_bankMovementTypeFormats[0].RowNumber)))
                            {
                                _head = true;
                            }
                        }
                        //Fin lectura cabecera

                        //Validación del formato correspondiente al banco seleccionado
                        if (_head)
                        {
                            if (!validateHeader && (_bankAccountFormats.Count > 0) && (i + 1 == (_bankAccountFormats[0].RowNumber)))
                            {
                                rowGeneric = _bankAccountFormats[0].ColumnNumber - 1;

                                if (row[rowGeneric].ToString() != "")
                                {
                                    if (bankAccountNumber != Convert.ToString(row[rowGeneric]))
                                    {
                                        successful = false;
                                        fileLocationName = "InvalidBankAccountNumber";
                                        break;
                                    }
                                }
                                else
                                {
                                    successful = false;
                                    fileLocationName = "fieldEmpty";
                                    break;
                                }
                                validateHeader = true;
                            }

                            statement.BankAccountCompany = bankAccountCompany;
                            if ((_bankMovementTypeFormats.Count > 0) && (i + 1 >= (_bankMovementTypeFormats[0].RowNumber)))
                            {
                                _head = true;
                                var isEmpty = row.ItemArray.All(x => x == null || (x != null && string.IsNullOrWhiteSpace(x.ToString())));

                                if (i + 1 == (_bankMovementTypeFormats[0].RowNumber) && isEmpty)
                                {
                                    emptyField = false;
                                    break;
                                }

                                if (!isEmpty)
                                {
                                    dataCount++;
                                    //Branch
                                    Branch branch = new Branch();
                                    if (_bankBranchDescriptionFormats.Count > 0)
                                    {
                                        if (_bankBranchDescriptionFormats[0].IsRequired)
                                        {
                                            branch.Id = 0;
                                            rowGeneric = _bankBranchDescriptionFormats[0].ColumnNumber - 1;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                branch.Id = GetBranchIdByDescription(row[rowGeneric].ToString());
                                                branch.Description = row[rowGeneric].ToString();
                                                statement.Branch = branch;
                                            }
                                            else
                                            {
                                                branch.Description = "";
                                                statement.Branch = branch;
                                                validateStatement = true;
                                                thirdPerson.CustomerTypeDescription = @Global.AgencyIsRequired + "; ";
                                            }
                                        }
                                        else
                                        {
                                            fileLocationName = "RequiredField";
                                            break;
                                        }
                                    }

                                    //MovementType en el formato éste campo es descriptivo no se valida el tipo de dato
                                    if (_bankMovementTypeFormats.Count > 0)
                                    {
                                        if (_bankMovementTypeFormats[0].IsRequired)
                                        {
                                            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO();

                                            rowGeneric = _bankMovementTypeFormats[0].ColumnNumber - 1;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                reconciliationMovementType.Description = row[rowGeneric].ToString();
                                                reconciliationMovementType.Id = 0;
                                                statement.ReconciliationMovementType = reconciliationMovementType;
                                            }
                                            else
                                            {
                                                reconciliationMovementType.Description = "";
                                                reconciliationMovementType.Id = 0;
                                                statement.ReconciliationMovementType = reconciliationMovementType;
                                                validateStatement = true;
                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.MovementTypesRequired + "; ";
                                            }
                                        }
                                        else
                                        {
                                            fileLocationName = "RequiredField";
                                            break;
                                        }
                                    }

                                    //Individual
                                    Individual individual = new Individual();
                                    if (_bankIndividualFormats.Count > 0)
                                    {
                                        if (_bankIndividualFormats[0].IsRequired)
                                        {
                                            rowGeneric = _bankIndividualFormats[0].ColumnNumber - 1;
                                            length = _bankIndividualFormats[0].Length;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                if (length < row[rowGeneric].ToString().Length)
                                                {
                                                    individual.FullName = row[rowGeneric].ToString().Substring(0, length);
                                                    thirdPerson.FullName = row[rowGeneric].ToString().Substring(0, length);
                                                }
                                                else
                                                {
                                                    individual.FullName = row[rowGeneric].ToString();
                                                    thirdPerson.FullName = row[rowGeneric].ToString();
                                                }

                                                statement.ThirdPerson = individual;
                                            }
                                            else
                                            {
                                                individual.FullName = "";
                                                thirdPerson.FullName = "";
                                                statement.ThirdPerson = individual;
                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.ThirdPartyRequired + "; ";
                                                validateStatement = true;
                                            }
                                        }
                                        else
                                        {
                                            individual.FullName = "";
                                            thirdPerson.FullName = "";
                                            statement.ThirdPerson = individual;
                                        }
                                    }

                                    //Amount
                                    if (_bankAmountFormats.Count > 0)
                                    {
                                        if (_bankAmountFormats[0].IsRequired)
                                        {
                                            rowGeneric = _bankAmountFormats[0].ColumnNumber - 1;
                                            decimal localAmount = 0;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                if (decimal.TryParse(row[rowGeneric].ToString(), out localAmount))
                                                {
                                                    Amount amount = new Amount() { Value = Convert.ToDecimal(row[rowGeneric]) };
                                                    statement.Amount = amount;
                                                    statement.LocalAmount = new Amount() { Value = Convert.ToDecimal(row[rowGeneric]) };
                                                }
                                                else
                                                {
                                                    Amount amount = new Amount() { Value = 0 };
                                                    statement.Amount = amount;
                                                    statement.LocalAmount = new Amount() { Value = 0 };
                                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.AmountShouldBeNumeric + ": " + row[rowGeneric].ToString() + "; ";
                                                    validateStatement = true;
                                                }
                                            }
                                            else
                                            {
                                                Amount amount = new Amount() { Value = 0 };
                                                statement.Amount = amount;
                                                statement.LocalAmount = new Amount() { Value = 0 };
                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.AmountRequired + "; ";
                                                validateStatement = true;
                                            }
                                        }
                                        else
                                        {
                                            fileLocationName = "RequiredField";
                                            break;
                                        }
                                    }
                                    //StatementType
                                    statement.StatementType = StatementTypes.Bank;
                                    //Date
                                    if (_bankMovementDateFormats.Count > 0)
                                    {
                                        if (_bankMovementDateFormats[0].IsRequired)
                                        {
                                            rowGeneric = _bankMovementDateFormats[0].ColumnNumber - 1;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                DateTime outDate;
                                                if (DateTime.TryParse(row[rowGeneric].ToString(), out outDate))
                                                {
                                                    statement.Date = DateTime.Parse(row[rowGeneric].ToString());
                                                }
                                                else
                                                {
                                                    statement.Date = Convert.ToDateTime("01/01/1900");
                                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.WrongDateFormat + ": " + row[rowGeneric].ToString();
                                                    validateStatement = true;
                                                }
                                            }
                                            else
                                            {
                                                statement.Date = Convert.ToDateTime("01/01/1900");
                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.DateRequired + "; ";
                                                validateStatement = true;
                                            }
                                        }
                                        else
                                        {
                                            fileLocationName = "RequiredField";
                                            break;
                                        }
                                    }
                                    //Número Comprobante
                                    if (_bankVoucherNumberFormats.Count > 0)
                                    {
                                        if (_bankVoucherNumberFormats[0].IsRequired)
                                        {
                                            rowGeneric = _bankVoucherNumberFormats[0].ColumnNumber - 1;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                long voucher = 0;
                                                if (long.TryParse(row[rowGeneric].ToString(), out voucher))
                                                {
                                                    statement.DocumentNumber = row[rowGeneric].ToString();
                                                }
                                                else
                                                {
                                                    statement.DocumentNumber = "0";
                                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.VoucherShouldNumeric + ": " + row[rowGeneric].ToString();
                                                    validateStatement = true;
                                                }
                                            }
                                            else
                                            {
                                                statement.DocumentNumber = "";
                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.VoucherRequired + "; ";
                                                validateStatement = true;
                                            }
                                        }
                                        else
                                        {
                                            statement.DocumentNumber = "0";
                                        }
                                    }
                                    //Descripción
                                    if (_bankDescriptionFormats.Count > 0)
                                    {
                                        if (_bankDescriptionFormats[0].IsRequired)
                                        {
                                            rowGeneric = _bankDescriptionFormats[0].ColumnNumber - 1;
                                            length = _bankDescriptionFormats[0].Length;

                                            if (row[rowGeneric].ToString() != "")
                                            {
                                                if (length < row[rowGeneric].ToString().Length)
                                                {
                                                    statement.Description = row[rowGeneric].ToString().Substring(0, length);
                                                }
                                                else
                                                {
                                                    statement.Description = row[rowGeneric].ToString();
                                                }
                                            }
                                            else
                                            {
                                                statement.Description = "";
                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.DescriptionRequired + "; ";
                                                validateStatement = true;
                                            }
                                        }
                                        else
                                        {
                                            statement.Description = "";
                                        }
                                    }
                                    statement.Id = 0;
                                    statement.ProcessDate = processDate;
                                    statement.Status = processId;
                                    statement.Branch = branch;
                                    statement.UserId = userId;
                                    statement.ThirdPerson = thirdPerson;
                                    bankStatements.Add(statement);

                                    //Si existen errores de formato se envian solo esos datos de conciliación a la tabla temporal
                                    if (validateStatement)
                                    {
                                        if (thirdPerson.CustomerTypeDescription != "")
                                        {
                                            saveBankStatement = true;
                                        }
                                        else
                                        {
                                            saveBankStatement = false;
                                        }
                                    }
                                    if (!validateStatement)
                                    {
                                        saveBankStatement = true;
                                    }
                                    if (saveBankStatement)
                                    {
                                        DelegateService.reconciliationService.SaveBankStatements(bankStatements);
                                    }
                                }
                            }
                        }
                    }//fin de bucle

                    if (emptyField)
                    {
                        bankAccountCompany.Id = bankAccountId;
                        statements = GetFailedBankStatementsByAccountBank(bankAccountCompany, processDate);
                    }//fin valida espacios vacios
                    else
                    {
                        fileLocationName = "ValidateRowsExcel/" + _bankMovementTypeFormats[0].Start;
                        successful = false;
                    }

                }//fin Existe formato
                else
                {
                    fileLocationName = "NotExistsFormat";
                    successful = false;
                }
            }
            catch (FormatException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (OverflowException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (InvalidCastException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (Exception ex)
            {
                fileLocationName = "Exception";
                successful = false;
            }

            stream.Close();

            if (!successful)
            {
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }

            return Json(statements, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SettingFormatsByBank
        /// </summary>
        /// <param name="formatDetails"></param>
        private void SettingFormatsByBank(List<FormatDetail> formatDetails)
        {
            _bankAccountFormats = new List<FormatField>();
            _bankNameFormats = new List<FormatField>();
            _bankSendDateFormats = new List<FormatField>();
            _bankMovementTypeFormats = new List<FormatField>();
            _bankVoucherNumberFormats = new List<FormatField>();
            _bankMovementDateFormats = new List<FormatField>();
            _bankAmountFormats = new List<FormatField>();
            _bankDescriptionFormats = new List<FormatField>();
            _bankBranchDescriptionFormats = new List<FormatField>();
            _bankIndividualFormats = new List<FormatField>();

            foreach (FormatDetail formatDetail in formatDetails)
            {
                if (formatDetail.FormatType == FormatTypes.Head)
                {
                    _head = false;
                }

                foreach (FormatField formatField in formatDetail.Fields)
                {
                    /*Formato cabecera*/
                    if (formatField.Value == ConfigurationManager.AppSettings["BankAccountNumber"].ToString())
                    {
                        _bankAccountFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["BankName"].ToString())
                    {
                        _bankNameFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }

                    /*Formato Detalle*/
                    if (formatField.Value == ConfigurationManager.AppSettings["BankReconciliationCode"].ToString())
                    {
                        _bankMovementTypeFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["VoucherNumber"].ToString())
                    {
                        _bankVoucherNumberFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["MovementDate"].ToString())
                    {
                        _bankMovementDateFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["ConcilationAmount"].ToString())
                    {
                        _bankAmountFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["Description"].ToString())
                    {
                        _bankDescriptionFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["SourceBranchCode"].ToString())
                    {
                        _bankBranchDescriptionFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                    if (formatField.Value == ConfigurationManager.AppSettings["ThirdDescription"].ToString())
                    {
                        _bankIndividualFormats.Add(BankFormatField(formatField, formatDetail.Separator));
                    }
                }
            }
        }

        ///<summary>
        ///BankFormatField
        ///Formato del banco
        ///</summary>
        ///<param name="formatField"></param>
        ///<param name="separator"></param>
        ///<returns>FormatField</returns>
        private FormatField BankFormatField(FormatField formatField, string separator)
        {
            return new FormatField()
            {
                Align = separator, //formatField.Align,
                ColumnNumber = formatField.ColumnNumber,
                Description = formatField.Description,
                Filled = formatField.Filled,
                Id = formatField.Id,
                IsRequired = formatField.IsRequired,
                Length = formatField.Length,
                Order = formatField.Order,
                RowNumber = formatField.RowNumber,
                Start = formatField.Start,
                Value = formatField.Value
            };
        }

        /// <summary>
        /// ReadTextFileToStream
        /// Lee archivo en txt
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="bankAccountId"></param>
        /// <returns>JsonResult</returns>
        private JsonResult ReadTextFileToStream(HttpPostedFileBase uploadedFile, int bankAccountId)
        {
            bool successful = true;
            string fileLocationName = String.Empty;
            List<BankStatementModel> statements = new List<BankStatementModel>();

            Byte[] arrayContent;
            fileLocationName = uploadedFile.FileName;

            //Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            //Lee el archivo y guarda en arreglo de tipo byte y este a su vez a arrayContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            StreamReader streamReader = new StreamReader(stream);

            try
            {
                string bankAccountNumber = GetBankAccountNumberByBankAccountCompanyId(bankAccountId);
                string bankName = GetBankNameByBankAccountCompanyId(bankAccountId);

                Format format = new Format()
                {
                    Id =  DelegateService.reconciliationService.GetReconciliationFormat(bankAccountId),
                    FileType = FileTypes.Text
                };
                List<FormatDetail> formatDetails =  DelegateService.reportingService.GetFormatDetailsByFormat(format);

                SettingFormatsByBank(formatDetails);

                bool validateStatement = false;
                bool saveBankStatement = true;
                string separator = "";

                int length;
                bool ValidateHeader = false;

                DateTime processDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                Parameter parameter = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ReconciliationProcessNumber"]));
                int processId = (int)parameter.NumberParameter;

                parameter.NumberParameter = processId + 1;
                DelegateService.commonService.UpdateParameter(parameter);

                int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

                //Se utiliza el campo number para enviar el nombre del usuario
                BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
                {
                    Id = bankAccountId,
                    Number = User.Identity.Name.ToUpper()
                };

                while (!streamReader.EndOfStream)
                {
                    List<Statement> bankStatements = new List<Statement>();
                    Statement statement = new Statement();
                    Individual thirdPerson = new Individual() {  CustomerTypeDescription = "" };
                    validateStatement = false;

                    //Lectura y validación de cabecera
                    if (!_head && (_bankAccountFormats.Count > 0))
                    {
                        separator = _bankAccountFormats[0].Align;
                        string[] data = streamReader.ReadLine().Split(new Char[] { Convert.ToChar(separator) });

                        if (data.Length <= 1)
                        {
                            successful = false;
                            fileLocationName = "InvalidSeparator";
                            break;
                        }
                        if (data[_bankAccountFormats[0].Order - 1] != "")
                        {
                            //Se valida el número de cuenta bancaria
                            if (bankAccountNumber != Convert.ToString(data[_bankAccountFormats[0].Order - 1]))
                            {
                                successful = false;
                                fileLocationName = "InvalidBankAccountNumber";
                                break;
                            }
                            else
                            {
                                //Nombre banco
                                if ((_bankNameFormats.Count > 0) && (_bankNameFormats[0].IsRequired))
                                {
                                    if (data.Length >= _bankNameFormats[0].Order)
                                    {
                                        if (data[_bankNameFormats[0].Order - 1].ToString() == "")
                                        {
                                            successful = false;
                                            fileLocationName = "fieldEmpty";
                                            break;
                                        }
                                        else
                                        {
                                            //Se valida el nombre del banco al que corresponde la cuenta bancaria
                                            if (bankName != data[_bankNameFormats[0].Order - 1].ToString())
                                            {
                                                successful = false;
                                                fileLocationName = "InvalidBankName";
                                                break;
                                            }
                                            else
                                            {
                                                //Fecha de envio
                                                if (_bankSendDateFormats.Count > 0)
                                                {
                                                    separator = _bankSendDateFormats[0].Align;
                                                    if (_bankSendDateFormats[0].IsRequired)
                                                    {
                                                        if (data.Length >= _bankSendDateFormats[0].Order)
                                                        {
                                                            if (data[_bankSendDateFormats[0].Order - 1].ToString() != "")
                                                            {
                                                                DateTime dateValue;
                                                                if (!DateTime.TryParse(data[_bankSendDateFormats[0].Order - 1].ToString(), out dateValue))
                                                                {
                                                                    statement.Date = Convert.ToDateTime("01/01/1900");
                                                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.WrongDateFormat + ": " + data[_bankSendDateFormats[0].Order - 1].ToString();
                                                                    validateStatement = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                statement.Date = Convert.ToDateTime("01/01/1900");
                                                                thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.SendDateRequired + "; ";
                                                                validateStatement = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            successful = false;
                                                            fileLocationName = "SendDateRequired";
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        successful = false;
                                        fileLocationName = "fieldEmpty";
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            successful = false;
                            fileLocationName = "fieldEmpty";
                            break;
                        }
                        ValidateHeader = true;
                    }
                    //Fin lectura cabecera

                    //Validación del formato correspondiente al banco seleccionado
                    if (_head)
                    {
                        if (_bankBranchDescriptionFormats.Count > 0)
                        {
                            separator = _bankBranchDescriptionFormats[0].Align;
                        }
                        else if (_bankAccountFormats.Count > 0)
                        {
                            separator = _bankAccountFormats[0].Align;
                        }
                        else if (_bankMovementTypeFormats.Count > 0)
                        {
                            separator = _bankMovementTypeFormats[0].Align;
                        }
                        else if (_bankIndividualFormats.Count > 0)
                        {
                            separator = _bankIndividualFormats[0].Align;
                        }
                        else if (_bankAmountFormats.Count > 0)
                        {
                            separator = _bankAmountFormats[0].Align;
                        }
                        else if (_bankMovementDateFormats.Count > 0)
                        {
                            separator = _bankMovementDateFormats[0].Align;
                        }
                        else if (_bankVoucherNumberFormats.Count > 0)
                        {
                            separator = _bankVoucherNumberFormats[0].Align;
                        }
                        else if (_bankDescriptionFormats.Count > 0)
                        {
                            separator = _bankDescriptionFormats[0].Align;
                        }

                        string[] data = streamReader.ReadLine().Split(new Char[] { Convert.ToChar(separator) });

                        if ((_bankBranchDescriptionFormats.Count > 0) && (data.Length <= 1))
                        {
                            successful = false;
                            fileLocationName = "InvalidSeparator";
                            break;
                        }
                        if (!ValidateHeader && (_bankAccountFormats.Count > 0))
                        {
                            if (data[_bankAccountFormats[0].Order - 1].ToString() != "")
                            {
                                //Valida el número de cuenta bancaria
                                if (bankAccountNumber != Convert.ToString(data[_bankAccountFormats[0].Order - 1]))
                                {
                                    successful = false;
                                    fileLocationName = "InvalidBankAccountNumber";
                                    break;
                                }
                            }
                            else
                            {
                                successful = false;
                                fileLocationName = "fieldEmpty";
                                break;
                            }
                            ValidateHeader = true;
                        }

                        statement.BankAccountCompany = bankAccountCompany;

                        //Branch
                        Branch branch = new Branch();
                        if (_bankBranchDescriptionFormats.Count > 0)
                        {
                            if (_bankBranchDescriptionFormats[0].IsRequired)
                            {
                                branch.Id = 0;
                                if (data.Length >= _bankBranchDescriptionFormats[0].Order)
                                {
                                    if (data[_bankBranchDescriptionFormats[0].Order - 1] != "")
                                    {
                                        branch.Id = GetBranchIdByDescription(data[_bankBranchDescriptionFormats[0].Order - 1].ToString());
                                        branch.Description = data[_bankBranchDescriptionFormats[0].Order - 1].ToString();
                                        statement.Branch = branch;
                                    }
                                    else
                                    {
                                        branch.Description = "";
                                        statement.Branch = branch;
                                        validateStatement = true;
                                        thirdPerson.CustomerTypeDescription = @Global.AgencyIsRequired + "; ";
                                    }
                                }
                                else
                                {
                                    branch.Description = "";
                                    statement.Branch = branch;
                                    validateStatement = true;
                                    thirdPerson.CustomerTypeDescription = @Global.AgencyIsRequired + "; ";
                                }
                            }
                            else
                            {
                                fileLocationName = "RequiredField";
                                break;
                            }
                        }
                        //MovementType en el formato esta campo es descriptivo no se valida el tipo de dato
                        if (_bankMovementTypeFormats.Count > 0)
                        {
                            if (_bankMovementTypeFormats[0].IsRequired)
                            {
                                ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO();
                                if (data.Length >= _bankMovementTypeFormats[0].Order)
                                {
                                    if (data[_bankMovementTypeFormats[0].Order - 1] != "")
                                    {
                                        reconciliationMovementType.Description = data[_bankMovementTypeFormats[0].Order - 1];
                                        reconciliationMovementType.Id = 0;
                                        statement.ReconciliationMovementType = reconciliationMovementType;
                                    }
                                    else
                                    {
                                        reconciliationMovementType.Description = "";
                                        reconciliationMovementType.Id = 0;
                                        statement.ReconciliationMovementType = reconciliationMovementType;
                                        validateStatement = true;
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.MovementTypesRequired + "; ";
                                    }
                                }
                                else
                                {
                                    reconciliationMovementType.Description = "";
                                    reconciliationMovementType.Id = 0;
                                    statement.ReconciliationMovementType = reconciliationMovementType;
                                    validateStatement = true;
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.MovementTypesRequired + "; ";
                                }
                            }
                            else
                            {
                                fileLocationName = "RequiredField";
                                break;
                            }
                        }
                        //Individual
                        Individual individual = new Individual();
                        if (_bankIndividualFormats.Count > 0)
                        {
                            if (_bankIndividualFormats[0].IsRequired)
                            {
                                length = _bankIndividualFormats[0].Length;
                                if (data.Length >= _bankIndividualFormats[0].Order)
                                {
                                    if (data[_bankIndividualFormats[0].Order - 1] != "")
                                    {
                                        if (length < data[_bankIndividualFormats[0].Order - 1].Length)
                                        {
                                            individual.FullName = data[_bankIndividualFormats[0].Order - 1].Substring(0, length);
                                            thirdPerson.FullName = data[_bankIndividualFormats[0].Order - 1].Substring(0, length);
                                        }
                                        else
                                        {
                                            individual.FullName = data[_bankIndividualFormats[0].Order - 1];
                                            thirdPerson.FullName = data[_bankIndividualFormats[0].Order - 1];
                                        }

                                        statement.ThirdPerson = individual;
                                    }
                                    else
                                    {
                                        individual.FullName = "";
                                        thirdPerson.FullName = "";
                                        statement.ThirdPerson = individual;
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.ThirdPartyRequired + "; ";
                                        validateStatement = true;
                                    }
                                }
                                else
                                {
                                    individual.FullName = "";
                                    thirdPerson.FullName = "";
                                    statement.ThirdPerson = individual;
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.ThirdPartyRequired + "; ";
                                    validateStatement = true;
                                }
                            }
                            else
                            {
                                individual.FullName  = "";
                                thirdPerson.FullName = "";
                                statement.ThirdPerson = individual;
                            }
                        }
                        //Amount
                        if (_bankAmountFormats.Count > 0)
                        {
                            if (_bankAmountFormats[0].IsRequired)
                            {
                                if (data.Length >= _bankAmountFormats[0].Order)
                                {
                                    if (data[_bankAmountFormats[0].Order - 1] != "")
                                    {
                                        decimal localAmount = 0;
                                        if (decimal.TryParse(data[_bankAmountFormats[0].Order - 1].ToString(), out localAmount))
                                        {
                                            Amount amount = new Amount() { Value = Convert.ToDecimal(data[_bankAmountFormats[0].Order - 1]) };
                                            statement.Amount = amount;
                                            statement.LocalAmount = new Amount() { Value = Convert.ToDecimal(data[_bankAmountFormats[0].Order - 1]) };
                                        }
                                        else
                                        {
                                            Amount amount = new Amount() { Value = 0 };
                                            statement.Amount = amount;
                                            statement.LocalAmount = new Amount() { Value = 0 };
                                            thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.AmountShouldBeNumeric + ": " + data[_bankAmountFormats[0].Order - 1] + "; ";
                                            validateStatement = true;
                                        }
                                    }
                                    else
                                    {
                                        Amount amount = new Amount() { Value = 0 };
                                        statement.Amount = amount;
                                        statement.LocalAmount = new Amount() { Value = 0 };
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.AmountRequired + "; ";
                                        validateStatement = true;
                                    }
                                }
                                else
                                {
                                    Amount amount = new Amount() { Value = 0 };
                                    statement.Amount = amount;
                                    statement.LocalAmount = new Amount() { Value = 0 };
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.AmountRequired + "; ";
                                    validateStatement = true;
                                }
                            }
                            else
                            {
                                fileLocationName = "RequiredField";
                                break;
                            }
                        }
                        //StatementType
                        statement.StatementType = StatementTypes.Bank;

                        //Date
                        if (_bankMovementDateFormats.Count > 0)
                        {
                            if (_bankMovementDateFormats[0].IsRequired)
                            {
                                if (data.Length >= _bankMovementDateFormats[0].Order)
                                {
                                    if (data[_bankMovementDateFormats[0].Order - 1] != "")
                                    {
                                        DateTime outDate;
                                        if (DateTime.TryParse(data[_bankMovementDateFormats[0].Order - 1].ToString(), out outDate))
                                        {
                                            statement.Date = DateTime.Parse(data[_bankMovementDateFormats[0].Order - 1]);
                                        }
                                        else
                                        {
                                            statement.Date = Convert.ToDateTime("01/01/1900");
                                            thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.WrongDateFormat + ": " + data[_bankMovementDateFormats[0].Order - 1];
                                            validateStatement = true;
                                        }
                                    }
                                    else
                                    {
                                        statement.Date = Convert.ToDateTime("01/01/1900");
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.DateRequired + "; ";
                                        validateStatement = true;
                                    }
                                }
                                else
                                {
                                    statement.Date = Convert.ToDateTime("01/01/1900");
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.DateRequired + "; ";
                                    validateStatement = true;
                                }
                            }
                            else
                            {
                                fileLocationName = "RequiredField";
                                break;
                            }
                        }
                        //Número Comprobante
                        if (_bankVoucherNumberFormats.Count > 0)
                        {
                            if (_bankVoucherNumberFormats[0].IsRequired)
                            {
                                if (data.Length >= _bankVoucherNumberFormats[0].Order)
                                {
                                    if (data[_bankVoucherNumberFormats[0].Order - 1] != "")
                                    {
                                        long voucher = 0;
                                        if (long.TryParse(data[_bankVoucherNumberFormats[0].Order - 1].ToString(), out voucher))
                                        {
                                            statement.DocumentNumber = data[_bankVoucherNumberFormats[0].Order - 1];
                                        }
                                        else
                                        {
                                            statement.DocumentNumber = data[_bankVoucherNumberFormats[0].Order - 1];
                                            thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.VoucherShouldNumeric + ": " + data[_bankVoucherNumberFormats[0].Order - 1];
                                            validateStatement = true;
                                        }
                                    }
                                    else
                                    {
                                        statement.DocumentNumber = "";
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.VoucherRequired + "; ";
                                        validateStatement = true;
                                    }
                                }
                                else
                                {
                                    statement.DocumentNumber = "";
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.VoucherRequired + "; ";
                                    validateStatement = true;
                                }
                            }
                            else
                            {
                                statement.DocumentNumber = "0";
                            }
                        }
                        //Descripción
                        if (_bankDescriptionFormats.Count > 0)
                        {
                            if (_bankDescriptionFormats[0].IsRequired)
                            {
                                length = _bankDescriptionFormats[0].Length;
                                if (data.Length >= _bankDescriptionFormats[0].Order)
                                {
                                    if (data[_bankDescriptionFormats[0].Order - 1] != "")
                                    {
                                        if (length < data[_bankDescriptionFormats[0].Order - 1].Length)
                                        {
                                            statement.Description = data[_bankDescriptionFormats[0].Order - 1].Substring(0, length);
                                        }
                                        else
                                        {
                                            statement.Description = data[_bankDescriptionFormats[0].Order - 1];
                                        }
                                    }
                                    else
                                    {
                                        statement.Description = "";
                                        thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.DescriptionRequired + "; ";
                                        validateStatement = true;
                                    }
                                }
                                else
                                {
                                    statement.Description = "";
                                    thirdPerson.CustomerTypeDescription = thirdPerson.CustomerTypeDescription + @Global.DescriptionRequired + "; ";
                                    validateStatement = true;
                                }
                            }
                            else
                            {
                                statement.Description = "";
                            }
                        }
                        statement.Id = 0;
                        statement.ProcessDate = processDate;
                        statement.Status = processId;
                        statement.Branch = branch;
                        statement.ThirdPerson = thirdPerson;
                        statement.UserId = userId;
                        bankStatements.Add(statement);

                        //Si existen errores de formato se envian solo esos datos de conciliacion a la tabla temporal
                        if (validateStatement)
                        {
                            if (thirdPerson.CustomerTypeDescription != "")
                            {
                                saveBankStatement = true;
                            }
                            else
                            {
                                saveBankStatement = false;
                            }
                        }
                        if (!validateStatement)
                        {
                            saveBankStatement = true;
                        }
                        if (saveBankStatement)
                        {
                            DelegateService.reconciliationService.SaveBankStatements(bankStatements);
                        }
                    }
                    _head = true;
                }//Fin de bucle
                bankAccountCompany.Id = bankAccountId;
                statements = GetFailedBankStatementsByAccountBank(bankAccountCompany, processDate);
            }
            catch (FormatException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (OverflowException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (InvalidCastException)
            {
                fileLocationName = "Exception";
                successful = false;
            }
            catch (Exception)
            {
                fileLocationName = "Exception";
                successful = false;
            }

            streamReader.Close();

            if (!successful)
            {
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
            return Json(statements, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetBankAccountNumberByBankAccountCompanyId
        /// </summary>
        /// <param name="bankAccountCompanyId"></param>
        /// <returns>string</returns>
        private string GetBankAccountNumberByBankAccountCompanyId(int bankAccountCompanyId)
        {
            return DelegateService.accountingParameterService.GetBankAccountCompany(new BankAccountCompanyDTO() { Id = bankAccountCompanyId }).Number;
        }

        /// <summary>
        /// GetBankNameByBankAccountCompanyId
        /// </summary>
        /// <param name="bankAccountCompanyId"></param>
        /// <returns>string</returns>
        private string GetBankNameByBankAccountCompanyId(int bankAccountCompanyId)
        {
            return DelegateService.accountingParameterService.GetBankAccountCompany(new BankAccountCompanyDTO() { Id = bankAccountCompanyId }).Bank.Description;
        }


        #endregion

    }
}