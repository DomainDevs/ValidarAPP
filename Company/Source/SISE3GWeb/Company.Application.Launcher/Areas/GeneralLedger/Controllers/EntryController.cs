using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;

//Sistran FWK
//using Sistran.Core.Framework.BAF;
//using Sistran.Core.Framework.Exceptions;
//using Sistran.Core.Framework.UIF.Web.Resources;
//using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
//using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Assemblers;
using Sistran.Core.Framework.UIF.Web.Helpers;
//using Sistran.Core.Framework.UIF.Web.Models;
//using Sistran.Core.Framework.UIF2.Controls.UifTable;
//using Sistran.Core.Framework.UIF2.Services;

////Sistran Core
//using Sistran.Core.Application.AccountingServices;
//using Sistran.Core.Application.CommonService.Models;
//using Sistran.Core.Application.GeneralLedgerServices;
//using Sistran.Core.Application.GeneralLedgerServices.DTOs;
//using Sistran.Core.Application.GeneralLedgerServices.Enums;
//using Sistran.Core.Application.TempCommonServices;
//using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.Models;

////Sistran Company
//using Sistran.Company.Application.CommonServices;
//using Sistran.Company.Application.UniquePersonServices;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class EntryController : BaseController
    {
        /*
        #region Instance Variables

        readonly ICommonService DelegateService.commonService = ServiceManager.Instance.GetService<ICommonService>();
        readonly IAccountingService DelegateService.glAccountingApplicationService = ServiceManager.Instance.GetService<IAccountingService>();
        readonly IImputationService _imputationService = ServiceManager.Instance.GetService<IImputationService>();
        readonly IUniquePersonService _uniquePersonService = ServiceManager.Instance.GetService<IUniquePersonService>();
        readonly ITempCommonService DelegateService.tempCommonService = ServiceManager.Instance.GetService<ITempCommonService>();

        readonly BaseController _baseController = new BaseController();

        #endregion

        #region View

        /// <summary>
        /// Entry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Entry()
        {
            try
            {

                ViewBag.BranchDefault = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 0);
                ViewBag.BranchDisabled = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 1);
                ViewBag.SalePointBranchUserDefault = _imputationService.GetSalePointDefaultByUserIdAndBranchId(_baseController.SessionHelper.GetUserId(), ViewBag.BranchDefault);

                //OBTIENE SI ESTA CONFIGURADO COMO MULTICOMPANIA 1 TRUE 0 FALSE
                ViewBag.ParameterMulticompany = _baseController.GetParameterMulticompany();

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// EntryMenu
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult EntryMenu()
        {

            try
            {

                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// EntryRevertion
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult EntryRevertion()
        {
            try
            {

                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// EntryConsultation
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult EntryConsultation()
        {
            try
            {
                ViewBag.Active = Convert.ToInt32(AccountingEntryStatus.Active);
                ViewBag.Reverted = Convert.ToInt32(AccountingEntryStatus.Reverted);
                ViewBag.BranchDefault = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 0);
                ViewBag.BranchDisabled = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            } 
        }

        #endregion View

        #region Actions

        /// <summary>
        /// SaveEntryRequest
        /// </summary>
        /// <param name="accountingEntryModels"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult SaveEntryRequest(List<AccountingEntryModel> accountingEntryModels)
        {
            int saved = 0;

            try
            {
                int userId = SessionHelper.GetUserId());
                int moduleId = Convert.ToInt32(ConfigurationManager.AppSettings["EntryModule"]);

                GeneralLedgerModels.LedgerEntry entry = new GeneralLedgerModels.LedgerEntry();

                entry.LedgerEntryItems = new List<GeneralLedgerModels.LedgerEntryItem>();

                foreach (AccountingEntryModel accountingEntryModel in accountingEntryModels)
                {
                    GeneralLedgerModels.LedgerEntryItem accountingEntry = new GeneralLedgerModels.LedgerEntryItem();

                    //accountingEntry.Date = _baseController.GetAccountingDateByModule(moduleId); //se envía la fecha contable, la fecha de registro se enviará a nivel de servicio.
                    accountingEntry.AccountingNature = (GeneralLedgerModels.AccountingNatures)accountingEntryModel.AccountingNatureId;
                    accountingEntry.CostCenters = new List<GeneralLedgerModels.CostCenter>();
                    accountingEntry.Analysis = new List<GeneralLedgerModels.Analysis>();
                    accountingEntry.PostDated = new List<GeneralLedgerModels.PostDated>();

                    // Se arma la lista de centros de costos
                    if (accountingEntryModel.CostCenters != null)
                    {
                        foreach (CostCenterEntryModel costCenterEntryModel in accountingEntryModel.CostCenters)
                        {
                            accountingEntry.CostCenters.Add(ModelAssembler.GetCostCenterEntry(costCenterEntryModel));
                        }
                    }

                    // Se arma la lista de análisis
                    if (accountingEntryModel.Analyses != null)
                    {
                        foreach (AnalysisModel analysisModel in accountingEntryModel.Analyses)
                        {
                            accountingEntry.Analysis.Add(ModelAssembler.GetAnalysis(analysisModel));
                        }
                    }

                    // Se arma la lista de postfechados
                    if (accountingEntryModel.Postdated != null)
                    {
                        foreach (PostDatedModel postDatedModel in accountingEntryModel.Postdated)
                        {
                            accountingEntry.PostDated.Add(ModelAssembler.GetPostDated(postDatedModel));
                        }
                    }

                    entry.LedgerEntryItems.Add(accountingEntry);
                }

                //saved = DelegateService.glAccountingApplicationService.SaveEntryRequest(entry, false, userId, false);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }

            return new UifJsonResult(true, saved);
        }

        /// <summary>
        /// SearchEntryMovements
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="entryNumber"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <param name="isFiltered"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SearchEntryMovements(int? branchId, int year, int month, int entryNumber, 
                                                 int? destinationId, int? accountingMovementTypeId, bool isFiltered)
        {
            // Se arma las fechas para consulta
            string dateFrom = "01" + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
            int numberOfDays = DateTime.DaysInMonth(year, month);
            string dateTo = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
            dateTo = dateTo + " 23:59:59";

            List<EntryConsultationDTO> entries = DelegateService.glAccountingApplicationService.SearchEntryMovements(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), 
                                   Convert.ToInt32(branchId), Convert.ToInt32(destinationId), Convert.ToInt32(accountingMovementTypeId));

            if (entries.Count > 0)
            {
                foreach (var item in entries)
                {
                    item.CurrencyDescription = _baseController.GetCurrencyDescriptionById(item.CurrencyId);
                }
            }

            if (isFiltered)
            {
                var isSearch = entries.GroupBy(x => new { x.Date, x.EntryNumber, x.EntryDestinationId, x.EntryDestinationDescription, x.BranchId, x.AccountingMovementTypeId, x.AccountingMovementTypeDescription }).ToList();

                List<EntryConsultationDTO> entryConsultations = new List<EntryConsultationDTO>();

                foreach (var item in isSearch)
                {
                    entryConsultations.Add(new EntryConsultationDTO()
                    {
                        Date = item.Key.Date.ToString(),
                        EntryNumber = Convert.ToInt32(item.Key.EntryNumber),
                        EntryDestinationId = Convert.ToInt32(item.Key.EntryDestinationId),
                        EntryDestinationDescription = item.Key.EntryDestinationDescription,
                        AccountingMovementTypeId = Convert.ToInt32(item.Key.AccountingMovementTypeId),
                        AccountingMovementTypeDescription = item.Key.AccountingMovementTypeDescription,
                        BranchId = Convert.ToInt32(item.Key.BranchId),
                        BranchDescription =
                        _baseController.GetBranchDescriptionById(Convert.ToInt32(item.Key.BranchId), User.Identity.Name.ToUpper())
                    });
                }

                return new UifTableResult(entryConsultations);
            }
            else
            {
                foreach (var entryConsultationDTO in entries)
                {
                    entryConsultationDTO.AccountingNatureDescription = entryConsultationDTO.AccountingNature == Convert.ToInt32(GeneralLedgerModels.AccountingNatures.Credit) ? Global.Credits : Global.Debits;
                    entryConsultationDTO.StatusDescription = entryConsultationDTO.Status == AccountingEntryStatus.Active ? Global.Active : Global.Reverted;
                }

                return new UifTableResult(entries);
            }
        }

        /// <summary>
        /// EntryRevertion
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="entryNumber"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult EntryRevertion(int branchId, int year, int month, int entryNumber, int destinationId, int accountingMovementTypeId)
        {
            try
            {
                int reverted = 0;

                // Se arma las fechas para consulta
                string dateFrom = "01" + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
                int numberOfDays = DateTime.DaysInMonth(year, month);
                string dateTo = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
                dateTo = dateTo + " 23:59:59";

                int userId = SessionHelper.GetUserId());
                int transactionNumber = Convert.ToInt32(DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["JournalEntryTransactionNumber"])).NumberParameter); //obtiene parámetro de la BDD

                //reverted = DelegateService.glAccountingApplicationService.EntryRevertion(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), branchId, destinationId, accountingMovementTypeId, true, userId);

                return new UifJsonResult(true, reverted);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetEntryTypeModels
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetEntryTypeModels()
        {
            bool isSucessfully;
            List<EntryTypeModel> entryTypeModel = new List<EntryTypeModel>();
            try
            {
                entryTypeModel = ServiceModelAssembler.GetEntryTypeModels(DelegateService.glAccountingApplicationService.GetEntryTypes());
                isSucessfully = true;
            }
            catch
            {
                isSucessfully = false;
                entryTypeModel = new List<EntryTypeModel>();
            }

            return new UifJsonResult(isSucessfully, entryTypeModel);
        }

        /// <summary>
        /// GetEntryTypeAccountingsByEntryType
        /// </summary>
        /// <param name="entryTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEntryTypeAccountingsByEntryType(int entryTypeId)
        {
            try
            {
                var entryType = new GeneralLedgerModels.EntryType();
                entryType.EntryTypeId = entryTypeId;
                entryType = DelegateService.glAccountingApplicationService.GetEntryType(entryType);
                EntryTypeModel entryTypeModel = ServiceModelAssembler.GetEntryTypeModel(entryType);

                //ordenado por id
                var data = (from EntryTypeAccountingModel entryTypeAccountingModel in
                                entryTypeModel.EntryTypeAccountingModels
                            select entryTypeAccountingModel).OrderBy(c => c.EntryTypeAccountingId);

                return new UifJsonResult(true, data);
            }
            catch
            {
                return new UifJsonResult(false, new List<EntryTypeAccountingModel>());
            }
        }

        #endregion Actions

        #region MassiveProcess

        /// <summary>
        /// EntryMassiveLoad
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult EntryMassiveLoad()
        {

            try
            {

                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
            
        }

        /// <summary>
        /// ReadFileInMemory
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="fileType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile, int fileType)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });
            if (fileType == 0)
            {
                if (data[1] == "xls" || data[1] == "xlsx")
                {
                    return ExcelToStream(uploadedFile);
                }
            }

            if (fileType != 1)
            {
                fileLocationName = "BadFileExtension";
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExcelToStream
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExcelToStream(HttpPostedFileBase uploadedFile)
        {
            bool successful = false;
            string message = "";
            string fileLocationName = "";
            Byte[] arrayContent;
            bool validateHeader = false;
            int processedRows = 0;

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de typo byte y este a su vez a arrayContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);

            IExcelDataReader excelReader;
            
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
                DataSet dataSet = excelReader.AsDataSet();

                // Se borra la tabla de trabajo.
                DelegateService.glAccountingApplicationService.ClearEntryMassiveLoad();

                // Se deshabilita los registros de la tabla de log
                DelegateService.glAccountingApplicationService.DisableEntryMassiveLoadLog();

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    int branchId = 0;
                    int salePointId = 0;
                    int accountingCompanyId = 0;
                    int entryDestinationId = 0;
                    int accountingMovementTypeId = 0;
                    DateTime operationDate = DateTime.Now;
                    decimal debits = 0;
                    decimal credits = 0;
                    bool isBalanced = false;

                    // Se lee la cabecera                    
                    if (Convert.ToString(dataSet.Tables[0].Rows[1][0]) != "")
                    {
                        branchId = Convert.ToInt32(dataSet.Tables[0].Rows[1][0]);
                        validateHeader = true;
                    }
                    else
                    {
                        validateHeader = false;
                        message = "NoBranchId";
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[1][2]) != "")
                        {
                            salePointId = Convert.ToInt32(dataSet.Tables[0].Rows[1][2]);
                            validateHeader = true;
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoSalePointId";
                        }
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[1][4]) != "")
                        {
                            accountingCompanyId = Convert.ToInt32(dataSet.Tables[0].Rows[1][4]);
                            validateHeader = true;
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoAccountingCompanyId";
                        }
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[1][6]) != "")
                        {
                            entryDestinationId = Convert.ToInt32(dataSet.Tables[0].Rows[1][6]);
                            validateHeader = true;
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoEntryDestinationId";
                        }
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[1][8]) != "")
                        {
                            accountingMovementTypeId = Convert.ToInt32(dataSet.Tables[0].Rows[1][8]);
                            validateHeader = true;
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoAccountingMovementTypeId";
                        }
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[1][10]) != "")
                        {
                            operationDate = Convert.ToDateTime(dataSet.Tables[0].Rows[1][10]);
                            validateHeader = true;
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoOperationDate";
                        }
                    }
                    if (validateHeader)
                    {
                        // Se lee el detalle
                        var rows = dataSet.Tables[0].Rows;

                        for (int index = 4; index < rows.Count; index++)
                        {
                            // Se valida débitos y créditos.
                            if (Convert.ToString(rows[index][0]) == "")
                            {
                                break; //indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
                            }
                            else
                            {
                                if (Convert.ToString(rows[index][2]) == "")
                                {
                                    successful = false;
                                    message = "NoAccountingNature";
                                    break;
                                }
                                else
                                {
                                    if (Convert.ToInt32(rows[index][2]) == (int)GeneralLedgerModels.AccountingNatures.Credit)
                                    {
                                        credits = credits + Convert.ToDecimal(rows[index][6]);
                                    }
                                    if (Convert.ToInt32(rows[index][2]) == (int)GeneralLedgerModels.AccountingNatures.Debit)
                                    {
                                        debits = debits + Convert.ToDecimal(rows[index][6]);
                                    }
                                }
                            }
                        }
                        if (debits == credits)
                        {
                            isBalanced = true;
                        }
                        else
                        {
                            message = "Unbalanced";
                            successful = false;
                        }
                        if (isBalanced)
                        {
                            for (int index = 4; index < rows.Count; index++)
                            {
                                if (Convert.ToString(rows[index][0]) == "")
                                {
                                    break; //indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
                                }
                                else
                                {
                                    processedRows = processedRows + 1;
                                    // Se arma el dto para grabarlo en la base
                                    MassiveEntryDTO massiveEntryDTO = new MassiveEntryDTO();
                                    int error = 0;
                                    string errorDescription = "";

                                    // Cabecera
                                    massiveEntryDTO.Id = 0; //este dato es autonumérico
                                    massiveEntryDTO.BranchId = branchId;
                                    massiveEntryDTO.SalePointId = salePointId;
                                    massiveEntryDTO.AccoutingCompanyId = accountingCompanyId;
                                    massiveEntryDTO.EntryDestinationId = entryDestinationId;
                                    massiveEntryDTO.AccountingMovementTypeId = accountingMovementTypeId;
                                    massiveEntryDTO.OperationDate = operationDate;

                                    // Movimientos
                                    massiveEntryDTO.RowNumber = Convert.ToInt32(rows[index][0]);
                                    if (Convert.ToString(rows[index][1]) == "")
                                    {
                                        error = error + 1;
                                        errorDescription = Global.NoEnteredAccountingAccount;
                                    }
                                    else
                                    {
                                        List<GeneralLedgerModels.AccountingAccount> accountingAccounts = new List<GeneralLedgerModels.AccountingAccount>();
                                        GeneralLedgerModels.AccountingAccount accountingAccount = new GeneralLedgerModels.AccountingAccount();
                                        accountingAccount.Number = Convert.ToString(rows[index][1]);

                                        accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountsByNumberDescription(accountingAccount);

                                        if (accountingAccounts.Count > 0)
                                        {
                                            massiveEntryDTO.AccountingAccountId = accountingAccounts[0].AccountingAccountId;
                                        }
                                        else
                                        {
                                            error = error + 1;
                                            errorDescription = errorDescription == "" ? Global.NoAccountingAccountValidation : errorDescription + ", " + Global.NoAccountingAccountValidation;
                                        }
                                    }

                                    massiveEntryDTO.AccountingNature = Convert.ToInt32(rows[index][2]);
                                    if (Convert.ToString(rows[index][4]) == "")
                                    {
                                        error = error + 1;
                                        errorDescription = errorDescription == "" ? Global.NoCurrencyValidation : errorDescription + ", " + Global.NoCurrencyValidation;
                                        massiveEntryDTO.CurrencyId = -1;
                                    }
                                    else
                                    {
                                        massiveEntryDTO.CurrencyId = Convert.ToInt32(rows[index][4]);
                                        // Calculo la tasa de cambio
                                        ExchangeRate exchangeRate = _baseController.GetExchangeRateByCurrencyId(massiveEntryDTO.CurrencyId);
                                        massiveEntryDTO.ExchangeRate = exchangeRate.SellAmount;
                                    }

                                    if (Convert.ToString(rows[index][6]) == "")
                                    {
                                        error = error + 1;
                                        errorDescription = errorDescription == "" ? Global.NoAmountValidation : errorDescription + ", " + Global.NoAmountValidation;
                                        massiveEntryDTO.Amount = 0;
                                    }
                                    else
                                    {
                                        massiveEntryDTO.Amount = Convert.ToDecimal(rows[index][6]);
                                    }
                                    if (Convert.ToString(rows[index][7]) == "")
                                    {
                                        error = error + 1;
                                        errorDescription = errorDescription == "" ? Global.NoPersonDocumentNumberValidation : errorDescription + ", " + Global.NoPersonDocumentNumberValidation;
                                        massiveEntryDTO.IndividualId = 0;
                                    }
                                    else
                                    {
                                        var persons = _uniquePersonService.GetPersonByDocumentNumberSurnameMotherLastName(Convert.ToString(rows[index][7]), "", "", "", 3, null, null);
                                        if (persons.Count > 0)
                                        {
                                            massiveEntryDTO.IndividualId = persons[0].IndividualId;
                                        }
                                        else
                                        {
                                            error = error + 1;
                                            errorDescription = errorDescription == "" ? Global.PersonNotFound : errorDescription + ", " + Global.PersonNotFound;
                                            massiveEntryDTO.IndividualId = 0;
                                        }
                                    }
                                    if (Convert.ToString(rows[index][8]) == "")
                                    {
                                        error = error + 1;
                                        errorDescription = errorDescription == "" ? Global.NoMovementDescriptionValidation : errorDescription + ", " + Global.NoMovementDescriptionValidation;
                                        massiveEntryDTO.Description = "";
                                    }
                                    else
                                    {
                                        massiveEntryDTO.Description = Convert.ToString(rows[index][8]);
                                    }

                                    #region BankReconciliation

                                    bool isBankReconciliation = false;

                                    massiveEntryDTO.BankReconciliationId = rows[index][9] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][9]);

                                    // Se ingresó conciliación bancaria
                                    if (massiveEntryDTO.BankReconciliationId > 0)
                                    {
                                        // Se comprueba que es una conciliación bancaria válida.
                                        List<GeneralLedgerModels.ReconciliationMovementType> bankReconciliations = DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes();
                                        isBankReconciliation = bankReconciliations.Any(item => item.Id == massiveEntryDTO.BankReconciliationId);
                                        if (!isBankReconciliation)
                                        {
                                            error = error + 1;
                                            errorDescription = errorDescription == "" ? Global.BankConciliationIdValidation : errorDescription + ", " + Global.BankConciliationIdValidation;
                                            massiveEntryDTO.BankReconciliationId = 0;
                                        }
                                        else
                                        {
                                            if (rows[index][11] == DBNull.Value)
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoBankConciliationNumberValidation : errorDescription + ", " + Global.NoBankConciliationNumberValidation;
                                                massiveEntryDTO.ReceiptNumber = 0;
                                            }
                                            else
                                            {
                                                massiveEntryDTO.ReceiptNumber = Convert.ToInt32(rows[index][11]);
                                            }
                                            if (rows[index][12] == DBNull.Value)
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoBankConciliationDateValidation : errorDescription + ", " + Global.NoBankConciliationDateValidation;
                                                massiveEntryDTO.ReceiptDate = null;
                                            }
                                            else
                                            {
                                                massiveEntryDTO.ReceiptDate = Convert.ToDateTime(rows[index][12]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        massiveEntryDTO.ReceiptNumber = rows[index][11] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][11]);
                                        massiveEntryDTO.ReceiptDate = rows[index][12] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rows[index][12]);
                                    }

                                    #endregion BankReconciliation

                                    #region CostCenter

                                    bool isCostCenter = false;

                                    // Centro de costos
                                    massiveEntryDTO.CostCenterId = rows[index][13] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][13]);
                                    if (massiveEntryDTO.CostCenterId > 0)
                                    {
                                        // Se comprueba que es un centro de costos válido
                                        List<GeneralLedgerModels.CostCenter> costCenters = DelegateService.glAccountingApplicationService.GetCostCenters();
                                        isCostCenter = costCenters.Any(item => item.CostCenterId == massiveEntryDTO.CostCenterId);
                                        if (!isCostCenter)
                                        {
                                            error = error + 1;
                                            errorDescription = errorDescription == "" ? Global.CostCenterIdValidation : errorDescription + ", " + Global.CostCenterIdValidation;
                                            massiveEntryDTO.CostCenterId = 0;
                                        }
                                        else
                                        {
                                            if (rows[index][15] == DBNull.Value)
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoCostCenterPercentageValidation : errorDescription + ", " + Global.NoCostCenterPercentageValidation;
                                                massiveEntryDTO.Percentage = 0;
                                            }
                                            else
                                            {
                                                if (Convert.ToDecimal(rows[index][15]) != 100)
                                                {
                                                    error = error + 1;
                                                    errorDescription = errorDescription == "" ? Global.CostCenterPercentageValidation : errorDescription + ", " + Global.CostCenterPercentageValidation;
                                                    massiveEntryDTO.Percentage = 0;
                                                }
                                                else
                                                {
                                                    massiveEntryDTO.Percentage = Convert.ToDecimal(rows[index][15]);
                                                }
                                            }
                                        }
                                    }

                                    massiveEntryDTO.Percentage = rows[index][15] == DBNull.Value ? 0 : Convert.ToDecimal(rows[index][15]);

                                    #endregion CostCenter

                                    #region Analysis

                                    bool isAnalysis = false;
                                    // Análisis
                                    massiveEntryDTO.AnalysisId = rows[index][16] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][16]);

                                    if (massiveEntryDTO.AnalysisId > 0)
                                    {
                                        // Se comprueba que el código de análisis sea válido.
                                        List<GeneralLedgerModels.AnalysisCode> analysisCodes = DelegateService.glAccountingApplicationService.GetAnalysisCodes();
                                        isAnalysis = analysisCodes.Any(item => item.AnalysisCodeId == massiveEntryDTO.AnalysisId);
                                        if (!isAnalysis)
                                        {
                                            error = error + 1;
                                            errorDescription = errorDescription == "" ? Global.AnalysisCodeIdValidation : errorDescription + ", " + Global.AnalysisCodeIdValidation;
                                            massiveEntryDTO.AnalysisId = 0;
                                        }
                                        else
                                        {
                                            // Se valida el código de concepto
                                            bool isConceptCode = false;

                                            massiveEntryDTO.ConceptId = rows[index][18] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][18]);

                                            if (massiveEntryDTO.ConceptId > 0)
                                            {
                                                List<AnalysisConceptAnalysisDTO> analysisConceptAnalyses = DelegateService.glAccountingApplicationService.GetPaymentConceptsByAnalysisCode(Convert.ToInt32(massiveEntryDTO.AnalysisId));
                                                isConceptCode = analysisConceptAnalyses.Any(item => item.AnalysisConceptId == massiveEntryDTO.ConceptId);
                                                if (!isConceptCode)
                                                {
                                                    error = error + 1;
                                                    errorDescription = errorDescription == "" ? Global.ConceptCodeNotRelatedValidation : errorDescription + ", " + Global.ConceptCodeNotRelatedValidation;
                                                    massiveEntryDTO.ConceptId = 0;
                                                }
                                                else
                                                {
                                                    massiveEntryDTO.ConceptId = Convert.ToInt32(rows[index][18]);
                                                }

                                                // Se valida clave de análisis
                                                if (rows[index][20] == DBNull.Value)
                                                {
                                                    error = error + 1;
                                                    errorDescription = errorDescription == "" ? Global.NoConceptKeyValidation : errorDescription + ", " + Global.NoConceptKeyValidation;
                                                    massiveEntryDTO.ConceptKey = null;
                                                }
                                                else
                                                {
                                                    massiveEntryDTO.ConceptKey = Convert.ToString(rows[index][20]);
                                                }

                                                // Se valido descripción
                                                if (rows[index][21] == DBNull.Value)
                                                {
                                                    error = error + 1;
                                                    errorDescription = errorDescription == "" ? Global.NoAnalysisDescriptionValidation : errorDescription + ", " + Global.NoAnalysisDescriptionValidation;
                                                    massiveEntryDTO.AnalysisDescription = null;
                                                }
                                                else
                                                {
                                                    massiveEntryDTO.AnalysisDescription = Convert.ToString(rows[index][21]);
                                                }
                                            }
                                            else
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoConceptCodeValidation : errorDescription + ", " + Global.NoConceptCodeValidation;
                                                massiveEntryDTO.ConceptId = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        massiveEntryDTO.ConceptId = rows[index][18] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][18]);
                                        massiveEntryDTO.ConceptKey = rows[index][20] == DBNull.Value ? null : Convert.ToString(rows[index][20]);
                                        massiveEntryDTO.AnalysisDescription = rows[index][21] == DBNull.Value ? null : Convert.ToString(rows[index][21]);
                                    }

                                    #endregion Analysis

                                    #region Postdated

                                    bool isPostdated = false;
                                    ExchangeRate postdatedExchangeRate = new ExchangeRate();
                                    // Postfechados
                                    massiveEntryDTO.PostdatedId = rows[index][22] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][22]);

                                    if (massiveEntryDTO.PostdatedId > 0)
                                    {
                                        // Se comprueba que es un tipo de postfechado válido
                                        List<PostDatedTypeModel> postDatedTypeModels = ServiceModelAssembler.PostDatedTypes(Enum.GetNames(typeof(PostDateTypes)));
                                        isPostdated = postDatedTypeModels.Any(item => item.Id == massiveEntryDTO.PostdatedId);
                                        if (!isPostdated)
                                        {
                                            error = error + 1;
                                            errorDescription = errorDescription == "" ? Global.PostdatedIdValidation : errorDescription + ", " + Global.PostdatedIdValidation;
                                            massiveEntryDTO.PostdatedId = 0;
                                        }
                                        else
                                        {
                                            // Se valida moneda
                                            if (rows[index][24] == DBNull.Value)
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoPostdatedCurrencyValidation : errorDescription + ", " + Global.NoPostdatedCurrencyValidation;
                                                massiveEntryDTO.PostdatedCurrencyId = 0;
                                            }
                                            else
                                            {
                                                massiveEntryDTO.PostdatedCurrencyId = Convert.ToInt32(rows[index][24]);
                                                // Se valida tasa de cambio
                                                postdatedExchangeRate = _baseController.GetExchangeRateByCurrencyId(Convert.ToInt32(massiveEntryDTO.PostdatedCurrencyId));

                                                massiveEntryDTO.PostdatedExchangeRate = postdatedExchangeRate.SellAmount;
                                            }
                                            // Se valida número de documento
                                            if (rows[index][26] == DBNull.Value)
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoPostdatedDocumentValidation : errorDescription + ", " + Global.NoPostdatedDocumentValidation;
                                                massiveEntryDTO.PosdatedDocumentNumber = null;
                                            }
                                            else
                                            {
                                                massiveEntryDTO.PosdatedDocumentNumber = Convert.ToString(rows[index][26]);
                                            }
                                            // Se valida importe
                                            if (rows[index][27] == DBNull.Value)
                                            {
                                                error = error + 1;
                                                errorDescription = errorDescription == "" ? Global.NoPostdatedAmountValidation : errorDescription + ", " + Global.NoPostdatedAmountValidation;
                                                massiveEntryDTO.PostdatedAmount = 0;
                                            }
                                            else
                                            {
                                                massiveEntryDTO.PostdatedAmount = Convert.ToDecimal(rows[index][27]);
                                            }
                                        }
                                    }

                                    massiveEntryDTO.PostdatedCurrencyId = rows[index][24] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][24]);
                                    int postdatedCurrencyId = 0;
                                    postdatedCurrencyId = rows[index][24] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][24]);
                                    postdatedExchangeRate = _baseController.GetExchangeRateByCurrencyId(postdatedCurrencyId);
                                    massiveEntryDTO.PostdatedExchangeRate = postdatedExchangeRate.SellAmount;
                                    massiveEntryDTO.PosdatedDocumentNumber = rows[index][26] == DBNull.Value ? null : Convert.ToString(rows[index][26]);
                                    massiveEntryDTO.PostdatedAmount = rows[index][27] == DBNull.Value ? 0 : Convert.ToDecimal(rows[index][27]);

                                    #endregion Postdated

                                    // Método para grabar el error
                                    MassiveEntryLogDTO massiveEntryLogDTO = new MassiveEntryLogDTO();
                                    massiveEntryLogDTO.Id = 0; //autonumérico
                                    massiveEntryLogDTO.ProcessDate = DateTime.Now;
                                    massiveEntryLogDTO.OperationDate = massiveEntryDTO.OperationDate;
                                    massiveEntryLogDTO.RowNumber = massiveEntryDTO.RowNumber;
                                    massiveEntryLogDTO.ErrorDescription = errorDescription;
                                    massiveEntryLogDTO.Enabled = true;
                                    massiveEntryLogDTO.Success = true;

                                    if (error > 0)
                                    {
                                        massiveEntryLogDTO.Success = false;
                                    }

                                    // Método para grabar el movimiento en la tabla de trabajo.
                                    DelegateService.glAccountingApplicationService.SaveEntryMassiveLoadRequest(massiveEntryDTO);

                                    // Graba el log
                                    DelegateService.glAccountingApplicationService.SaveEntryMassiveLoadLogRequest(massiveEntryLogDTO);
                                    error = 0;
                                }
                            }
                        }
                        successful = true;
                    }
                    else
                    {
                        message = "IncompleteHeader";
                        successful = false;
                    }
                }

            }
            catch (FormatException)
            {
                message = "Exception";
                successful = false;
            }
            catch (OverflowException)
            {
                message = "Exception";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                message = "Exception";
                successful = false;
            }
            catch (InvalidCastException)
            {
                message = "Exception";
                successful = false;
            }
            catch (Exception)
            {
                message = "Exception";
                successful = false;
            }

            stream.Close();

            object[] jsonData = new object[2];

            jsonData[0] = message;
            jsonData[1] = successful;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetEntryMassiveLoadRecords
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetEntryMassiveLoadRecords()
        {
            EntryMassiveLoadResultDTO entryMassiveLoadResult = DelegateService.glAccountingApplicationService.GetEntryMassiveLoadRecords();

            return Json(entryMassiveLoadResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetMassiveEntryFailedRecords
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetMassiveEntryFailedRecords()
        {
            List<MassiveEntryLogDTO> massiveEntryLogs = DelegateService.glAccountingApplicationService.GetMassiveEntryFailedRecords();

            List<object> MassiveEntryFailedRecords = new List<object>();

            if (massiveEntryLogs.Count > 0)
            {
                foreach (MassiveEntryLogDTO massiveEntryLog in massiveEntryLogs)
                {
                    MassiveEntryFailedRecords.Add(new
                    {
                        Id = massiveEntryLog.Id,
                        ProcessDate = String.Format("{0:dd/MM/yyyy}", massiveEntryLog.ProcessDate),
                        OperationDate = String.Format("{0:dd/MM/yyyy}", massiveEntryLog.OperationDate),
                        RowNumber = massiveEntryLog.RowNumber,
                        Success = massiveEntryLog.Success,
                        Description = massiveEntryLog.Description,
                        Enabled = massiveEntryLog.Enabled,
                    });
                }
            }

            return new UifTableResult(MassiveEntryFailedRecords);
        }

        /// <summary>
        /// GenerateEntry
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GenerateEntry()
        {
            try
            {
                int entry = 0;
                int userId = 0;

                userId = SessionHelper.GetUserId());
                entry = DelegateService.glAccountingApplicationService.GenerateEntry(userId);

                return Json(entry, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion MassiveProcess

        #region Private Methods

        
        #endregion

    */
    }
}