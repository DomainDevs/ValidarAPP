using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;

using Excel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using AccountingPaymentModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.DTOs.Retentions;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{

    public class MassiveDataLoadController : Controller
    {
        #region Instance Variables   

        readonly BillingController _billingController = new BillingController();
        readonly CommonController _commonController = new CommonController();
        public const int PageSize = 1000;
        public const int PageIndex = 1;

        #endregion Interfaz

        #region View

        /// <summary>
        /// MainMassiveDataLoad
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainMassiveDataLoad()
        {
            try
            {

                int defaultValue = Convert.ToInt16(Global.DefaultValue);
                string defaultDescription = Global.DefaultDescription;
                List<Branch> branches = _commonController.GetBranchesByUserName(User.Identity.Name);
                branches.Insert(0, new Branch
                {
                    Id = defaultValue,
                    Description = defaultDescription
                });
                ViewBag.Branch = branches;

                // Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainMassiveDataGenerate
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainMassiveDataGenerate()
        {
            try
            {

                int defaultValue = Convert.ToInt16(Global.DefaultValue);
                string defaultDescription = (Global.DefaultDescription);
                List<Branch> branches = _commonController.GetBranchesByUserName(User.Identity.Name);
                branches.Insert(0, new Branch { Id = defaultValue, Description = defaultDescription });
                ViewBag.Branch = branches;

                // Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region ReadFilesInMemory

        /// <summary>
        /// ReadFileInMemory
        /// Lee un archivo sin guardar
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile)
        {
            string fileLocationName = "";

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ExelToStream(uploadedFile);
            }
            if (data[1] == "xml")
            {
                return XmlToStream(uploadedFile);
            }
            if (data[1] == "csv")
            {
                return CsvToStream(uploadedFile);
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExelToStream
        /// Lee un archivo en formato xls, xlsx sin guardar
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExelToStream(HttpPostedFileBase uploadedFile)
        {
            bool successful = true;
            string fileLocationName = "";
            Byte[] arrayContent;

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            //Lee el archivo y  guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);

            IExcelDataReader excelReader;
            List<MassiveDataLoadModel> massiveDataLoads = new List<MassiveDataLoadModel>();
            int recordCount = 0;
            decimal amount = 0;

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

                for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                {
                    System.Data.DataRow row = result.Tables[0].Rows[i];

                    if (row[0].ToString() != "")
                    {
                        if (IsNegative(Convert.ToInt32(row[0])) || IsNegative(Convert.ToInt32(row[1])) ||
                                IsNegative(Convert.ToInt32(row[2])) || IsNegative(Convert.ToInt32(row[3])) ||
                                IsNegative(Convert.ToInt32(row[4])) || IsNegative(Convert.ToDecimal(row[9]))
                        )
                        {
                            fileLocationName = "NegativeId";
                            successful = false;
                            break;
                        }

                        decimal convertedAmount = 0;
                        convertedAmount = row[9] == DBNull.Value ? 0 : Convert.ToDecimal(row[9]);

                        massiveDataLoads.Add(new MassiveDataLoadModel()
                        {
                            PolicyNumber = row[2] == DBNull.Value ? 0 : Convert.ToInt32(row[2]),
                            EndorsementNumber = row[3] == DBNull.Value ? 0 : Convert.ToInt32(row[3]),
                            CurrencyId = row[8] == DBNull.Value ? 0 : Convert.ToInt32(row[8]),
                            BranchId = row[0] == DBNull.Value ? 0 : Convert.ToInt32(row[0]),
                            PrefixId = row[1] == DBNull.Value ? 0 : Convert.ToInt32(row[1]),
                            AgentId = row[4] == DBNull.Value ? 0 : Convert.ToInt32(row[4]),
                            AgentName = row[5] == DBNull.Value ? "" : Convert.ToString(row[5]),
                            BeneficiaryDocumentNumber = row[6] == DBNull.Value ? "" : Convert.ToString(row[6]),
                            BeneficiaryName = row[7] == DBNull.Value ? "" : Convert.ToString(row[7]),
                            ExchangeRate = row[10] == DBNull.Value ? 0 : Convert.ToDecimal(row[10]),
                            PaymentsTotal = Convert.ToDecimal(convertedAmount),
                        });
                        amount += Convert.ToDecimal(convertedAmount);
                        recordCount++;
                    }
                }
            }
            catch (FormatException)
            {
                fileLocationName = "FormatException";
                successful = false;
            }
            catch (OverflowException)
            {
                fileLocationName = "OverflowException";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                fileLocationName = "IndexOutOfRangeException";
                successful = false;
            }
            catch (InvalidCastException)
            {
                fileLocationName = "InvalidCastException";
                successful = false;
            }
            catch (Exception)
            {
                fileLocationName = "Exception";
                successful = false;
            }

            TempData["dtoMassiveDataLoad"] = massiveDataLoads;

            stream.Close();

            if (successful)
            {
                fileLocationName += ";" + recordCount.ToString() + ";" + amount.ToString().Replace(",", ".");
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadXMLFile
        /// Lee un archivo en formato xml sin guardar
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult XmlToStream(HttpPostedFileBase uploadedFile)
        {
            try
            {
                string fileLocationName = "";
                int index = 0;
                Byte[] arrayContent;

                fileLocationName = uploadedFile.FileName;
                string[] data = fileLocationName.Split(new char[] { '.' });

                var buffer = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

                // Lee el archivo y lo guarda en arreglo de typo byte y este a su vez a arrContent
                arrayContent = buffer;

                Stream stream = new MemoryStream(arrayContent);
                stream.Position = 0;
                List<PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies;

                premiumReceivableSearchPolicies = new List<PremiumReceivableSearchPolicyDTO>();

                XmlDocument xDoc = new XmlDocument();

                xDoc.Load(stream);

                XmlNodeList policies = xDoc.GetElementsByTagName("policies");
                XmlNodeList policyNode = ((XmlElement)policies[0]).GetElementsByTagName("policy");

                foreach (XmlElement node in policyNode)
                {
                    try
                    {
                        XmlNodeList branchId = node.GetElementsByTagName("branchId");
                        XmlNodeList branch = node.GetElementsByTagName("branch");
                        XmlNodeList prefixId = node.GetElementsByTagName("prefixId");
                        XmlNodeList prefix = node.GetElementsByTagName("prefix");
                        XmlNodeList policyId = node.GetElementsByTagName("policyId");
                        XmlNodeList number = node.GetElementsByTagName("number");
                        XmlNodeList endorsementId = node.GetElementsByTagName("endorsementId");
                        XmlNodeList endorsement = node.GetElementsByTagName("endorsement");
                        XmlNodeList currencyId = node.GetElementsByTagName("currencyId");
                        XmlNodeList currency = node.GetElementsByTagName("currency");
                        XmlNodeList quota = node.GetElementsByTagName("quota");
                        XmlNodeList amount = node.GetElementsByTagName("amount");
                        XmlNodeList expirationDate = node.GetElementsByTagName("expirationdate");
                        XmlNodeList insuredId = node.GetElementsByTagName("insuredId");
                        XmlNodeList insured = node.GetElementsByTagName("insured");
                        XmlNodeList agentId = node.GetElementsByTagName("agentId");
                        XmlNodeList agent = node.GetElementsByTagName("agent");
                        XmlNodeList percentage = node.GetElementsByTagName("percentage");
                        XmlNodeList commission = node.GetElementsByTagName("commission");

                        if (IsNegative(Convert.ToInt32(branchId[0].InnerText)) || IsNegative(Convert.ToInt32(currencyId[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(endorsementId[0].InnerText)) || IsNegative(Convert.ToInt32(policyId[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(prefixId[0].InnerText)) || IsNegative(Convert.ToInt32(insuredId[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(agentId[0].InnerText)))
                        {
                            fileLocationName = "NegativeId";
                            break;
                        }

                        premiumReceivableSearchPolicies.Add(new PremiumReceivableSearchPolicyDTO()
                        {
                            PaymentAmount = Convert.ToDecimal(amount[0].InnerText.Replace(".", ",")),
                            BranchId = Convert.ToInt32(branchId[0].InnerText),
                            BranchDescription = Convert.ToString(branch[0].InnerText),
                            CurrencyId = Convert.ToInt32(currencyId[0].InnerText),
                            CurrencyDescription = Convert.ToString(currency[0].InnerText),
                            EndorsementId = Convert.ToInt32(endorsementId[0].InnerText),
                            EndorsementDocumentNumber = Convert.ToString(endorsement[0].InnerText),
                            PolicyId = Convert.ToInt32(policyId[0].InnerText),
                            PolicyDocumentNumber = Convert.ToString(number[0].InnerText),
                            PrefixId = Convert.ToInt32(prefixId[0].InnerText),
                            PrefixDescription = Convert.ToString(prefix[0].InnerText),
                            PaymentNumber = Convert.ToInt32(quota[0].InnerText),
                            PaymentExpirationDate = Convert.ToDateTime(expirationDate[0].InnerText),
                            InsuredId = Convert.ToInt32(insuredId[0].InnerText),
                            InsuredName = Convert.ToString(insured[0].InnerText),
                            PolicyAgentId = Convert.ToInt32(agentId[0].InnerText),
                            PolicyAgentName = Convert.ToString(agent[0].InnerText),
                            BranchPrefixPolicyEndorsement = Convert.ToString(branch[0].InnerText).Substring(0, 3) + '-' + Convert.ToString(prefix[0].InnerText).Substring(0, 3) + '-' + Convert.ToString(number[0].InnerText) + '-' + Convert.ToString(endorsement[0].InnerText),
                            ExchangeRate = 1,
                            StandarCommissionPercentage = Convert.ToDecimal(percentage[0].InnerText.Replace(".", ",")),
                            AgentParticipationPercentage = Convert.ToDecimal(commission[0].InnerText.Replace(".", ","))
                        });
                        index++;
                    }
                    catch (FormatException)
                    {
                        fileLocationName = "FormatException";
                        break;
                    }
                    catch (OverflowException)
                    {
                        fileLocationName = "OverflowException";
                        break;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        fileLocationName = "IndexOutOfRangeException";
                        break;
                    }
                    catch (InvalidCastException)
                    {
                        fileLocationName = "InvalidCastException";
                        break;
                    }
                    catch (NullReferenceException)
                    {
                        fileLocationName = "NullReferenceException";
                        break;
                    }
                }

                TempData["dtoXml"] = premiumReceivableSearchPolicies;

                stream.Close();

                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
            catch (XmlException)
            {
                string fileLocationName = "";
                fileLocationName = "XmlException";
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
            catch (NullReferenceException)
            {
                string fileLocationName = "";
                fileLocationName = "XmlException";
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetMassiveProcess
        /// Obtiene los procesos masivos pendientes de culminar
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetMassiveProcess()
        {
            int userId = 0;

            // Viene de Test (en Test no permite autenticar)
            if (User == null)
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }
            else
            {
                userId = _commonController.GetUserIdByName(User.Identity.Name);
            }

            List<MassiveProcessDTO> massiveProcesses = DelegateService.accountingCollectService.GetMassiveProcess(userId).OrderBy(o => o.BeginDate).ToList();

            List<object> massiveProcessesResponses = new List<object>();

            foreach (MassiveProcessDTO massiveProcess in massiveProcesses)
            {
                massiveProcessesResponses.Add(new
                {

                    BeginDate = Convert.ToString(massiveProcess.BeginDate),
                    Description = massiveProcess.Description,
                    EndDate = massiveProcess.EndDate,
                    FailedRecords = massiveProcess.FailedRecords,
                    PorcentageAdvance = Math.BusinessMath.Round(massiveProcess.PorcentageAdvance, 2),
                    ProcessId = massiveProcess.ProcessId,
                    StateId = massiveProcess.StateId,
                    StateDescription = massiveProcess.StateDescription,
                    SuccessfulRecords = massiveProcess.SuccessfulRecords,
                    TotalRecords = massiveProcess.TotalRecords,
                    UserId = massiveProcess.UserId,
                    UserName = massiveProcess.UserName
                });
            }

            return Json(new
            {
                aaData = massiveProcessesResponses,
                total = massiveProcessesResponses.Count
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SaveMassiveBillRequest

        /// <summary>
        /// SaveMassiveBillRequest
        /// Invoca al proceso de grabación masiva liberando a la vista
        /// </summary>
        /// <param name="branchId"> </param>
        /// <param name="billControlId"> </param>
        public void SaveMassiveBillRequest(int branchId, int billControlId)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            new Thread(() =>
            {
                SaveMassiveBill(branchId, billControlId, userId);
            }).Start();

        }

        /// <summary>
        /// SaveMassiveBill
        /// Graba masivamente recibos provenientes de un archivo plano
        /// </summary>
        /// <param name="branchId"> </param>
        /// <param name="billControlId"> </param>
        /// <param name="userId"> </param>
        public void SaveMassiveBill(int branchId, int billControlId, int userId)
        {
            int successfulRecords = 0;
            int failedRecords = 0; decimal exchange = decimal.Zero; int decimalPlaces = 2;
            List<MassiveDataLoadModel> massiveDataLoadModels = (List<MassiveDataLoadModel>)TempData["dtoMassiveDataLoad"];

            if (massiveDataLoadModels != null)
            {
                // GRABAR EN BILL.BILL_MASSIVE_PROCESS
                CollectMassiveProcessDTO billMassiveProcessDto = new CollectMassiveProcessDTO();
                billMassiveProcessDto.CollectMassiveProcessId = 0;
                billMassiveProcessDto.FailedRecords = 0;
                billMassiveProcessDto.BeginDate = DateTime.Now;
                billMassiveProcessDto.Status = false;
                billMassiveProcessDto.SuccessfulRecords = 0;
                billMassiveProcessDto.TotalRecords = massiveDataLoadModels.Count;
                billMassiveProcessDto.UserId = userId;

                CollectMassiveProcessDTO newBillMassiveProcessDto = DelegateService.accountingCollectService.SaveCollectMassiveProcess(billMassiveProcessDto);

                foreach (MassiveDataLoadModel massiveData in massiveDataLoadModels)
                {
                    try
                    {
                        BillModel frmBill = new BillModel();
                        var person = DelegateService.tempCommonService.GetPersonsByDocumentNumber(massiveData.BeneficiaryDocumentNumber);

                        frmBill.PayerId = person.FirstOrDefault().IndividualId;
                        frmBill.PaymentsTotal = System.Math.Round(Convert.ToDecimal(massiveData.PaymentsTotal), decimalPlaces);
                        frmBill.RegisterDate = DateTime.Now;
                        frmBill.BillControlId = billControlId;
                        frmBill.UserId = userId;
                        frmBill.PaymentSummary = new List<PaymentSummaryModel>();
                        exchange = massiveData.ExchangeRate;

                        ItemsToPayGridModel items = new ItemsToPayGridModel();
                        items.BillItem = new List<BillItemModel>();

                        BillItemModel billItemModel = new BillItemModel();

                        PremiumReceivableSearchPolicyDTO billrequestmodel = GetPolicyQuotaListView(massiveData.PolicyNumber.ToString(), "", "", massiveData.EndorsementNumber.ToString(), branchId.ToString(), "");//massiveData.PolicyPayerId.ToString());


                        billItemModel.Column1 = billrequestmodel.PolicyId.ToString(); // Póliza
                        billItemModel.Column2 = billrequestmodel.EndorsementId.ToString(); // Endoso
                        billItemModel.Column3 = billrequestmodel.PaymentNumber.ToString(); // Nro.Couta
                        billItemModel.Column4 = billrequestmodel.PayerId.ToString(); // Pagador
                        frmBill.Description = Global.labelPayment + " " + billrequestmodel.PrefixTyniDescription + " " + massiveData.PolicyNumber.ToString() + "-" + massiveData.EndorsementNumber.ToString();
                        
                        if (billrequestmodel.PaymentAmount > 0)
                        {
                            billItemModel.PaidAmount = System.Math.Round(massiveData.PaymentsTotal, decimalPlaces);
                            billItemModel.Amount = System.Math.Round(massiveData.PaymentsTotal, decimalPlaces);
                        }
                        else
                        {
                            failedRecords++;
                            billMassiveProcessDto.CollectMassiveProcessId = newBillMassiveProcessDto.CollectMassiveProcessId;
                            billMassiveProcessDto.FailedRecords = failedRecords;

                            LogMassiveDataPolicyDTO logdto = new LogMassiveDataPolicyDTO()
                            {

                                TechnicalTransaction = 0,
                                LogMessage = Global.PayedQuota,
                                IdProcess = newBillMassiveProcessDto.CollectMassiveProcessId,
                                EndorsementNumber = massiveData.EndorsementNumber,
                                PrefixId = massiveData.PrefixId,
                                BranchId = massiveData.BranchId,
                                PolicyNumber = massiveData.PolicyNumber,
                                Amount = System.Math.Round(Convert.ToDecimal(massiveData.PaymentsTotal), decimalPlaces),
                                ExchangeRate = exchange,
                                DateGenerate = DateTime.Now
                            };

                            DelegateService.accountingApplicationService.SaveLogMassiveDataPolicy(logdto);
                            DelegateService.accountingCollectService.UpdateCollectMassiveProcess(billMassiveProcessDto);
                            continue;
                        }

                        
                        billItemModel.ExchangeRate = exchange;// Moneda extranjera
                        billItemModel.CurrencyId = billrequestmodel.CurrencyId; // Moneda local
                        items.BillItem.Add(billItemModel);

                        PaymentSummaryModel paymentSummaryModel = new PaymentSummaryModel();

                        paymentSummaryModel.PaymentMethodId = Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]);
                        paymentSummaryModel.Amount = System.Math.Round(massiveData.PaymentsTotal, decimalPlaces);
                        paymentSummaryModel.LocalAmount = System.Math.Round(massiveData.PaymentsTotal, decimalPlaces);
                        paymentSummaryModel.ExchangeRate = exchange;
                        paymentSummaryModel.CurrencyId = massiveData.CurrencyId;
                        frmBill.PaymentSummary.Add(paymentSummaryModel);

                        var result = ((MainApplicationReceipt)_billingController.SaveBillRequest(frmBill, items, branchId, 0).Data);

                        LogMassiveDataPolicyDTO logMassiveDataPolicyDTO = new LogMassiveDataPolicyDTO()
                        {

                            TechnicalTransaction = Convert.ToInt32(result.TechnicalTransaction),
                            LogMessage = result.Message,
                            IdProcess = newBillMassiveProcessDto.CollectMassiveProcessId,
                            EndorsementNumber = massiveData.EndorsementNumber,
                            PrefixId = massiveData.PrefixId,
                            BranchId = massiveData.BranchId,
                            PolicyNumber = massiveData.PolicyNumber,
                            Amount = System.Math.Round(Convert.ToDecimal(massiveData.PaymentsTotal), decimalPlaces),
                            ExchangeRate = exchange,
                            DateGenerate = DateTime.Now
                        };

                        DelegateService.accountingApplicationService.SaveLogMassiveDataPolicy(logMassiveDataPolicyDTO);

                        // Actualización(BILL.BILL_MASSIVE_PROCESS)
                        billMassiveProcessDto.CollectMassiveProcessId = newBillMassiveProcessDto.CollectMassiveProcessId;
                        if (Convert.ToInt32(result.TechnicalTransaction) != 0)
                            billMassiveProcessDto.SuccessfulRecords = ++successfulRecords;
                        else
                            billMassiveProcessDto.FailedRecords = ++failedRecords;
                        billMassiveProcessDto.EndDate = DateTime.Now;
                        DelegateService.accountingCollectService.UpdateCollectMassiveProcess(billMassiveProcessDto);
                    }
                    catch (Exception e)
                    {
                        failedRecords++;

                        // Actualización(BILL.BILL_MASSIVE_PROCESS)
                        billMassiveProcessDto.CollectMassiveProcessId = newBillMassiveProcessDto.CollectMassiveProcessId;
                        billMassiveProcessDto.FailedRecords = failedRecords;

                        LogMassiveDataPolicyDTO logMassiveDataPolicyDTO = new LogMassiveDataPolicyDTO()
                        {

                            TechnicalTransaction = 0,
                            LogMessage = Global.PaidPolicy,
                            IdProcess = newBillMassiveProcessDto.CollectMassiveProcessId,
                            EndorsementNumber = massiveData.EndorsementNumber,
                            PrefixId = massiveData.PrefixId,
                            BranchId = massiveData.BranchId,
                            PolicyNumber = massiveData.PolicyNumber,
                            Amount = System.Math.Round(Convert.ToDecimal(massiveData.PaymentsTotal), decimalPlaces),
                            ExchangeRate = exchange,
                            DateGenerate = DateTime.Now
                        };

                        DelegateService.accountingApplicationService.SaveLogMassiveDataPolicy(logMassiveDataPolicyDTO);
                        DelegateService.accountingCollectService.UpdateCollectMassiveProcess(billMassiveProcessDto);
                    }
                }

                // Actualización(BILL.BILL_MASSIVE_PROCESS)
                billMassiveProcessDto.CollectMassiveProcessId = newBillMassiveProcessDto.CollectMassiveProcessId;
                billMassiveProcessDto.FailedRecords = failedRecords;
                billMassiveProcessDto.EndDate = DateTime.Now;
                billMassiveProcessDto.Status = true;
                billMassiveProcessDto.SuccessfulRecords = successfulRecords;
                DelegateService.accountingCollectService.UpdateCollectMassiveProcess(billMassiveProcessDto);
            }
        }
        #endregion

        #region MassiveDataGenerate

        /// <summary>
        /// MassiveDataForGenerate
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="issuedate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult MassiveDataForGenerate(int? branchId, string issuedate)
        {
            List<MassiveDataGenerateDTO> massiveDataGenerateDTOs =
            DelegateService.accountingApplicationService.MassiveDataForGenerate(Convert.ToDateTime(issuedate));

            // Se consulta por la sucursal requerida
            List<MassiveDataGenerateDTO> massiveDataGenerates = (from MassiveDataGenerateDTO item in massiveDataGenerateDTOs
                                                                 where item.BranchId == branchId
                                                                 orderby item.PayerId, item.EndorsementId, item.PolicyId
                                                                 select item).ToList();

            return Json(new
            {
                aaData = massiveDataGenerates,
                total = massiveDataGenerates.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// MassiveDataForGenerateExcel
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="issuedate"></param>
        /// <returns>ActionResult</returns>
        public ActionResult MassiveDataForGenerateExcel(int? branchId, string issuedate)
        {
            List<MassiveDataGenerateDTO> massiveDataGenerateDTOs = DelegateService.accountingApplicationService.MassiveDataForGenerate(Convert.ToDateTime(issuedate));

            // Ordenamiento por Cuota/Endoso/Póliza
            List<MassiveDataGenerateDTO> massiveDataOrders = (from order in massiveDataGenerateDTOs orderby order.PayerId, order.EndorsementId, order.PolicyId select order).ToList();

            // Se consulta por la sucursal requerida
            List<MassiveDataGenerateDTO> massiveDataGenerates = (from MassiveDataGenerateDTO item in massiveDataOrders
                                                                 where item.BranchId == branchId
                                                                 select item).ToList();

            List<MassiveDataGenerateDTO> convertedMassives = new List<MassiveDataGenerateDTO>();

            foreach (MassiveDataGenerateDTO item in massiveDataGenerates)
            {
                convertedMassives.Add(item);
            }

            massiveDataGenerateDTOs = convertedMassives;

            MemoryStream memoryStream = GetCurrentAuxiliaryData(ConvertPaginatedResponseToDataTable(massiveDataGenerateDTOs));

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "CargaMassivaExcel.xls");
        }

        /// <summary>
        /// ConvertPaginatedResponseToDataTable
        /// </summary>
        /// <param name="massiveDataGenerateDTOs"></param>
        /// <returns>DataTable</returns>
        public DataTable ConvertPaginatedResponseToDataTable(List<MassiveDataGenerateDTO> massiveDataGenerateDTOs)
        {
            DataTable dataTable = new DataTable();

            // Get the type of source object and create a new instance of that type
            var headerRow = new List<string>(10);

            headerRow.Add(Global.Branch);
            headerRow.Add(Global.Prefix);
            headerRow.Add(Global.PolicyNumber);
            headerRow.Add(Global.EndorsementNumber);
            headerRow.Add(Global.Agent);
            headerRow.Add(Global.AgentName);
            headerRow.Add(Global.Beneficiary);
            headerRow.Add(Global.BeneficiaryName);
            headerRow.Add(Global.CurrencyId);
            headerRow.Add(Global.Amount);
            headerRow.Add(Global.ExchangeRate);


            for (int j = 0; j < headerRow.Count; j++)
            {
                dataTable.Columns.Add(headerRow[j]);
            }

            try
            {
                foreach (MassiveDataGenerateDTO row in massiveDataGenerateDTOs)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow[0] = row.BranchId;
                    dataRow[1] = row.PrefixId;
                    dataRow[2] = row.PolicyNumber;
                    dataRow[3] = row.EndorsementNumber;
                    dataRow[4] = row.AgentId;
                    dataRow[5] = row.AgentName;
                    dataRow[6] = row.BeneficiaryDocumentNumber;
                    dataRow[7] = row.BeneficiaryName;
                    dataRow[8] = row.CurrencyId;
                    dataRow[9] = row.Amount;
                    dataRow[10] = row.ExchangeRate;


                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return dataTable;
        }

        #endregion

        #region Method Private

        /// <summary>
        /// SaveBillRequest
        /// </summary>
        /// <param name="frmBill"></param>
        /// <param name="itemsToPayGridModel"></param>
        /// <param name="branchId"></param>
        /// <param name="preliquidationBranch"></param>
        /// <returns></returns>
        private void SaveBillRequest(BillModel frmBill, ItemsToPayGridModel itemsToPayGridModel, int branchId, int preliquidationBranch)
        {
            CollectApplicationDTO collectImputation = new CollectApplicationDTO();

            int billControlId = frmBill.BillControlId;
            int number = 0;
            bool validateComponents = false; // Indica si una póliza tiene sus componentes registrados en el sistema.
            int invalidRecords = 0;
            int imputationTypeId = 0; //parámetro para la contabilización de la imputación

            try
            {
                // Se obtiene parámetro de la BDD
                number = _commonController.GetBillNumber();

                // Se actualiza parámetro de número de carátula.
                _commonController.UpdateBillNumber(number);

                CollectDTO collect = new CollectDTO();

                CollectConceptDTO billingConcept = new CollectConceptDTO();
                billingConcept.Id = frmBill.BillingConceptId;

                PersonDTO payer = new PersonDTO()
                {
                    IndividualId = frmBill.PayerId,
                    IdentificationDocument = new IdentificationDocumentDTO()
                    {
                        DocumentType = new DocumentTypeDTO() { Id = frmBill.PayerDocumentTypeId },
                        Number = frmBill.PayerDocumentNumber
                    },
                    Name = frmBill.PayerName == null ? null : frmBill.PayerName.ToUpper(),
                    PersonType = new PersonTypeDTO() { Id = frmBill.PayerTypeId }
                };

                int statusId = (int)CollectStatus.Active;

                if (frmBill.SourcePaymentId > 0)
                {
                    statusId = (int)CollectStatus.Applied;
                }

                int userId = 0;

                if (User != null)
                {
                    userId = _commonController.GetUserIdByName(User.Identity.Name);
                }
                else
                {
                    // Viene de una generación masiva de recibos
                    userId = frmBill.UserId;
                }

                collect.Description = frmBill.Description;

                collect.Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                collect.Concept = billingConcept;
                collect.PaymentsTotal = new AmountDTO
                {
                    Value = frmBill.PaymentsTotal // paymentsTotal.Value;
                };
                collect.Payer = payer;
                collect.Status = statusId;
                collect.Number = number;
                collect.CollectType = (int)CollectTypes.Incoming;
                collect.UserId = userId;
                collect.CompanyIndividualId = -1;// Quemado por el momento;
                collect.Branch.Id = branchId;

                collect.Payments = new List<PaymentDTO>();

                #region Payment

                if (frmBill.PaymentSummary != null)
                {
                    for (int j = 0; j < frmBill.PaymentSummary.Count; j++)
                    {
                        AccountingPaymentModels.PaymentMethodDTO paymentMethod = new AccountingPaymentModels.PaymentMethodDTO()
                        {
                            Id = frmBill.PaymentSummary[j].PaymentMethodId
                        };

                        #region PaymentMethodType

                        #region Cash

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };
                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };

                            collect.Payments.Add(new CashDTO()
                            {
                                PaymentMethod = paymentMethod,
                                Amount = amount,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                ExchangeRate = exchangeRate,
                                LocalAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount },
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region Check

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]) || paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"]) || paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDebit"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            BankDTO issuingBank = new BankDTO() { Id = frmBill.PaymentSummary[j].CheckPayments[0].IssuingBankId };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };

                            collect.Payments.Add(new CheckDTO()
                            {
                                PaymentMethod = paymentMethod,
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),
                                DocumentNumber = frmBill.PaymentSummary[j].CheckPayments[0].DocumentNumber,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                IssuerName = frmBill.PaymentSummary[j].CheckPayments[0].IssuerName,
                                IssuingAccountNumber = frmBill.PaymentSummary[j].CheckPayments[0].IssuingAccountNumber,
                                IssuingBank = issuingBank,
                                ExchangeRate = exchangeRate,
                                LocalAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount },
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region CreditCard

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPaymentCard"]))
                        {

                            decimal taxBase = frmBill.PaymentSummary[j].CreditPayments[0].TaxBase;
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };
                            BankDTO issuingBank = new BankDTO() { Id = frmBill.PaymentSummary[j].CreditPayments[0].IssuingBankId };
                            CreditCardTypeDTO creditCardType = new CreditCardTypeDTO() { Id = frmBill.PaymentSummary[j].CreditPayments[0].CreditCardTypeId };
                            CreditCardValidThruDTO creditCardValidThru = new CreditCardValidThruDTO()
                            {
                                Month = frmBill.PaymentSummary[j].CreditPayments[0].ValidThruMonth,
                                Year = frmBill.PaymentSummary[j].CreditPayments[0].ValidThruYear
                            };

                            List<PaymentTaxDTO> paymentTaxs = DelegateService.accountingPaymentService.GetTaxCreditCard(creditCardType.Id, branchId).Taxes;

                            decimal ivaCardAmount = 0;
                            decimal tax = 0;
                            decimal retention = 0;

                            if (paymentTaxs != null)
                            {
                                for (int i = 0; i < paymentTaxs.Count; i++)
                                {
                                    if (paymentTaxs[i].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxCardIvaId"]))
                                    {
                                        ivaCardAmount = taxBase * paymentTaxs[i].Rate / 100;
                                    }
                                }

                                // Calcula la comisión
                                creditCardType.Commission = DelegateService.accountingParameterService.GetCreditCardType(creditCardType.Id).Commission * (Convert.ToDecimal(localAmount.Value) - ivaCardAmount) / 100;

                                // Asigna las bases del impuesto
                                for (int f = 0; f < paymentTaxs.Count; f++)
                                {
                                    paymentTaxs[f].TaxBase = new AmountDTO();
                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxCardIvaId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = taxBase;
                                    }

                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardIcaId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = (frmBill.PaymentSummary[j].LocalAmount - ivaCardAmount);
                                    }

                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardIvaId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = ivaCardAmount;
                                    }

                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardSourceId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = (frmBill.PaymentSummary[j].LocalAmount - ivaCardAmount);
                                    }
                                }

                                // Calcula el valor del impuesto total
                                for (int f = 0; f < paymentTaxs.Count; f++)
                                {
                                    /*TODO LFREIRE No existe campo en modelo tax en TaxService
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
                            }

                            collect.Payments.Add(new CreditCardDTO()
                            {
                                Amount = amount,
                                AuthorizationNumber = frmBill.PaymentSummary[j].CreditPayments[0].AuthorizationNumber,
                                CardNumber = frmBill.PaymentSummary[j].CreditPayments[0].CardNumber,
                                Holder = frmBill.PaymentSummary[j].CreditPayments[0].Holder,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                IssuingBank = issuingBank,
                                ExchangeRate = exchangeRate,
                                LocalAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount },
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

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPaymentArea"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };
                            BankDTO issuingBank = new BankDTO() { Id = frmBill.PaymentSummary[j].TransferPayments[0].IssuingBankId };
                            BankAccountPersonDTO recievingAccount = new BankAccountPersonDTO()
                            {
                                Bank = new BankDTO() { Id = frmBill.PaymentSummary[j].TransferPayments[0].ReceivingBankId },
                                Number = frmBill.PaymentSummary[j].TransferPayments[0].ReceivingAccountNumber
                            };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                            collect.Payments.Add(new TransferDTO()
                            {
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),//frmBill.PaymentSummary[j].TransferPayments[0].Date,
                                DocumentNumber = frmBill.PaymentSummary[j].TransferPayments[0].DocumentNumber,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                IssuerName = frmBill.PaymentSummary[j].TransferPayments[0].IssuerName,
                                IssuingAccountNumber = frmBill.PaymentSummary[j].TransferPayments[0].IssuingAccountNumber,
                                IssuingBank = issuingBank,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                ReceivingAccount = recievingAccount,
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region Deposit

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            AmountDTO localAmount = new AmountDTO();
                            if (frmBill.PaymentSummary[j].LocalAmount.Equals(0))
                            {
                                localAmount.Value = frmBill.PaymentsTotal;
                            }
                            else
                            {
                                localAmount.Value = frmBill.PaymentSummary[j].LocalAmount;
                            }

                            BankAccountCompanyDTO receivingAccount = new BankAccountCompanyDTO()
                            {
                                Bank = new BankDTO() { Id = frmBill.PaymentSummary[j].DepositVouchers[0].ReceivingAccountBankId },
                                Number = frmBill.PaymentSummary[j].DepositVouchers[0].ReceivingAccountNumber
                            };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            localAmount.Value = frmBill.PaymentSummary[j].LocalAmount;

                            collect.Payments.Add(new DepositVoucherDTO()
                            {
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),
                                DepositorName = frmBill.PaymentSummary[j].DepositVouchers[0].DepositorName,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                ReceivingAccount = receivingAccount,
                                VoucherNumber = frmBill.PaymentSummary[j].DepositVouchers[0].VoucherNumber,
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region Retention

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };
                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };

                            PolicyDTO policy = new PolicyDTO()
                            {
                                Branch = new BranchDTO() { Id = frmBill.PaymentSummary[j].BranchId },
                                DefaultBeneficiaries = new List<BeneficiaryDTO>()
                                {
                                    new BeneficiaryDTO()
                                    {
                                        CustomerType = Convert.ToInt32(CustomerType.Individual),
                                        IndividualId = frmBill.PayerId
                                    }
                                },
                                DocumentNumber = Convert.ToInt32(frmBill.PaymentSummary[j].RetentionReceipts[0].PolicyNumber),
                                Endorsement = new EndorsementDTO() { Description = frmBill.PaymentSummary[j].RetentionReceipts[0].EndorsementNumber.ToString() },
                                Holder = new HolderDTO()
                                {
                                    CustomerType = Convert.ToInt32(CustomerType.Individual),
                                    IndividualId = frmBill.PayerId,
                                    IndividualType = Convert.ToInt32(IndividualType.Person),
                                    InsuredId = frmBill.PayerTypeId,
                                },
                                Id = Convert.ToInt32(frmBill.PaymentSummary[j].RetentionReceipts[0].SerialNumber), //policyId
                                IssueDate = frmBill.PaymentSummary[j].RetentionReceipts[0].IssueDate,
                                Prefix = new PrefixDTO() { Id = frmBill.PaymentSummary[j].PrefixId },
                                UserId = userId
                            };

                            collect.Payments.Add(new RetentionReceiptDTO()
                            {
                                Amount = amount,
                                AuthorizationNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].AuthorizationNumber,
                                BillNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].BillNumber,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),// frmBill.PaymentSummary[j].RetentionReceipts[0].Date,
                                ExchangeRate = exchangeRate,
                                ExpirationDate = frmBill.PaymentSummary[j].RetentionReceipts[0].ExpirationDate,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                InvoiceDate = frmBill.PaymentSummary[j].RetentionReceipts[0].InvoiceDate,
                                IssueDate = frmBill.PaymentSummary[j].RetentionReceipts[0].IssueDate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                Policy = policy,
                                RetentionConcept = new RetentionConceptDTO() { Id = frmBill.PaymentSummary[j].RetentionReceipts[0].RetentionConceptId },
                                SerialNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].SerialNumber,
                                SerialVoucherNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].SerialVoucherNumber,
                                Status = Convert.ToInt16(PaymentStatus.Active),
                                VoucherNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].VoucherNumber,
                            });
                        }

                        #endregion
                    }
                }
                #endregion
                #endregion

                #region Imputation

                ApplicationDTO imputation = new ApplicationDTO();

                if (itemsToPayGridModel.BillItem != null)
                {
                    if (itemsToPayGridModel.BillItem[0].Column1 == null)
                    {
                        imputation.Id = itemsToPayGridModel.BillItem[0].BillId;
                    }
                    else
                    {
                        imputation.Id = 0;
                    }

                    imputation.RegisterDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                    imputation.ModuleId = (int)ApplicationTypes.Collect;
                    imputationTypeId = Convert.ToInt32(ApplicationTypes.Collect);
                    if (frmBill.SourcePaymentId > 0)
                    {
                        imputation.ModuleId = (int)ApplicationTypes.PreLiquidation;
                        imputationTypeId = Convert.ToInt32(ApplicationTypes.PreLiquidation);
                    }
                    imputation.UserId = userId;
                    imputation.ApplicationItems = new List<TransactionTypeDTO>();
                    PremiumReceivableTransactionDTO premiumReceivableTransaction = new PremiumReceivableTransactionDTO();
                    premiumReceivableTransaction.Id = 0;
                    premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();
                    if (itemsToPayGridModel.BillItem != null)
                    {
                        for (int k = 0; k < itemsToPayGridModel.BillItem.Count; k++)
                        {
                            PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem = new PremiumReceivableTransactionItemDTO();
                            premiumReceivableTransactionItem.Policy = new PolicyDTO();
                            premiumReceivableTransactionItem.Id = 0;
                            premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<BeneficiaryDTO>()
                            {
                                new BeneficiaryDTO()
                                {
                                    CustomerType = Convert.ToInt32(CustomerType.Individual),
                                    IndividualId = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column4)
                                }
                            };
                            premiumReceivableTransactionItem.Policy.Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column1);
                            premiumReceivableTransactionItem.Policy.Endorsement = new EndorsementDTO() { Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column2) };
                            premiumReceivableTransactionItem.Policy.ExchangeRate = new ExchangeRateDTO()
                            {
                                Currency = new CurrencyDTO() { Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].CurrencyId) },
                                SellAmount = itemsToPayGridModel.BillItem[k].ExchangeRate
                            };
                            premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponentDTO>()
                            {
                                new PayerComponentDTO()
                                {
                                    Amount = itemsToPayGridModel.BillItem[k].Amount,
                                    BaseAmount = itemsToPayGridModel.BillItem[k].PaidAmount
                                }
                            };
                            premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlanDTO()
                            {
                                Quotas = new List<QuotaDTO>()
                                {
                                    new QuotaDTO { Number = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column3) }
                                }
                            };

                            premiumReceivableTransactionItem.DeductCommission = new AmountDTO();
                            premiumReceivableTransactionItem.DeductCommission.Value = 0; //no se graba comisiones
                            premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);

                            //valido que las primas a aplicar tienen registrados sus componentes en el sistema.
                            if (!DelegateService.accountingApplicationService.ValidatePolicyComponents(Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column1), Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column2)))
                            {
                                invalidRecords++;
                            }
                        }
                    }
                    imputation.ApplicationItems.Add(premiumReceivableTransaction);
                    collectImputation.Application = imputation;

                    if (invalidRecords > 0)
                    {
                        validateComponents = false;
                    }
                    else
                    {
                        validateComponents = true;
                    }
                }
                else
                {
                    validateComponents = true;
                }

                #endregion

                if (validateComponents)
                {
                    #region APLICACIÓN PRELIQUIDACIÓN EE

                    // Para aplicación de preliquidación. 
                    if (frmBill.SourcePaymentId > 0)
                    {
                        PreliquidationsDTO preliquidations = new PreliquidationsDTO();

                        preliquidations.BranchId = Convert.ToInt32(preliquidationBranch);
                        preliquidations.PreliquidationId = Convert.ToInt32(frmBill.SourcePaymentId);

                        List<PreliquidationsDTO> preLiquidations = DelegateService.accountingApplicationService.GetPreliquidations(preliquidations);
                        if (preLiquidations.Count > 0)
                        {
                            DelegateService.accountingApplicationService.SaveApplicationRequest(frmBill.SourcePaymentId,
                                                                                     Convert.ToInt32(preLiquidations[0].TempImputationId),
                                                                                     Convert.ToInt32(ApplicationTypes.PreLiquidation), collect.Description,
                                                                                     statusId, _commonController.GetUserIdByName(User.Identity.Name), frmBill.SourcePaymentId, 0, collect.Date);//todo se debe pasar tecnical transaction

                        }
                        // Recibo ya aplicado se borra par evitar conflictos en grabación.
                        collectImputation.Application = null;
                    }

                    #endregion

                    // Get transaction number
                    int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["TransactionNumber"]);
                    int technicalTransaction = Convert.ToInt32(DelegateService.commonService.GetParameterByParameterId(parameterId).NumberParameter);

                    //Update transaction number.
                    Parameter parameter = new Parameter()
                    {
                        Id = parameterId,
                        NumberParameter = technicalTransaction + 1,
                        Description = "NÚMERO DE TRANSACCIÓN"
                    };

                    DelegateService.commonService.UpdateParameter(parameter);
                    // Grabo bill

                    TransactionDTO transaction = new TransactionDTO() { Id = 0, TechnicalTransaction = technicalTransaction };
                    collectImputation.Id = frmBill.SourcePaymentId;
                    collect.Transaction = transaction;

                    collectImputation.Id = 0;
                    collectImputation.Collect = collect;

                    // Se Graba bill
                    collectImputation = _billingController.SaveReceiptRequest(collectImputation, billControlId);

                    #region Accounting

                    // Se Ejecuta método para iniciar contabilización
                    if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                    {
                        _billingController.RecordBill(collectImputation.Collect.Id, 1, userId, collectImputation.Transaction.TechnicalTransaction);
                    }

                    // Se obtiene los parámetros para la contabilización de aplicación
                    if ((ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true") && collectImputation.Collect.Status == Convert.ToInt32(CollectStatus.Applied))
                    {
                        _billingController.RecordApplication(collectImputation.Collect.Id, collectImputation.Collect.UserId);
                    }

                    #endregion Accounting
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// IsNegative
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private bool IsNegative(int id)
        {
            if (id < 0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// IsNegative
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private bool IsNegative(decimal id)
        {
            if (id < 0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// CsvToStream
        /// Lee un archivo en formato csv 
        /// </summary>
        /// <param name="uploadedFile"> </param>
        /// <returns>JsonResult</returns>
        private JsonResult CsvToStream(HttpPostedFileBase uploadedFile)
        {
            string fileLocationName = "";
            Byte[] arrayContent;

            fileLocationName = uploadedFile.FileName;

            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y  guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            List<PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies = new List<PremiumReceivableSearchPolicyDTO>();

            try
            {
                int index = 0;
                StreamReader streamReader = new StreamReader(stream);
                string sLine = "";

                while (sLine != null)
                {
                    try
                    {
                        sLine = streamReader.ReadLine();
                        if ((sLine != null) && (index > 0))
                        {
                            string[] data = sLine.Split(new char[] { ';' });
                            if (IsNegative(Convert.ToInt32(data[0])) || IsNegative(Convert.ToInt32(data[8])) ||
                                IsNegative(Convert.ToInt32(data[6])) || IsNegative(Convert.ToInt32(data[4])) ||
                                IsNegative(Convert.ToInt32(data[2])) || IsNegative(Convert.ToInt32(data[13])) ||
                                IsNegative(Convert.ToInt32(data[15])))
                            {
                                fileLocationName = "NegativeId";
                                break;
                            }

                            premiumReceivableSearchPolicies.Add(new PremiumReceivableSearchPolicyDTO()
                            {
                                PaymentAmount = Convert.ToDecimal(data[11]),
                                BranchId = Convert.ToInt32(data[0]),
                                BranchDescription = Convert.ToString(data[1]),
                                CurrencyId = Convert.ToInt32(data[8]),
                                CurrencyDescription = Convert.ToString(data[9]),
                                EndorsementId = Convert.ToInt32(data[6]),
                                EndorsementDocumentNumber = Convert.ToString(data[7]),
                                PolicyId = Convert.ToInt32(data[4]),
                                PolicyDocumentNumber = Convert.ToString(data[5]),
                                PrefixId = Convert.ToInt32(data[2]),
                                PrefixDescription = Convert.ToString(data[3]),
                                PaymentNumber = Convert.ToInt32(data[10]),
                                PaymentExpirationDate = Convert.ToDateTime(data[12]),
                                InsuredId = Convert.ToInt32(data[13]),
                                InsuredName = Convert.ToString(data[14]),
                                PolicyAgentId = Convert.ToInt32(data[15]),
                                PolicyAgentName = Convert.ToString(data[16]),
                                BranchPrefixPolicyEndorsement = Convert.ToString(data[1]).Substring(0, 3) + '-' + Convert.ToString(data[3]).Substring(0, 3) + '-' + Convert.ToString(data[5]) + '-' + Convert.ToString(data[7]),
                                ExchangeRate = 1,
                                StandarCommissionPercentage = Convert.ToDecimal(data[17]),
                                AgentParticipationPercentage = Convert.ToDecimal(data[18])
                            });

                        }
                        index++;
                    }
                    catch (FormatException)
                    {
                        fileLocationName = "FormatException";
                        break;
                    }
                    catch (OverflowException)
                    {
                        fileLocationName = "OverflowException";
                        break;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        fileLocationName = "IndexOutOfRangeException";
                        break;
                    }
                    catch (InvalidCastException)
                    {
                        fileLocationName = "InvalidCastException";
                        break;
                    }
                    catch (NullReferenceException)
                    {
                        fileLocationName = "NullReferenceException";
                        break;
                    }
                }
                streamReader.Close();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            TempData["dtoCsv"] = premiumReceivableSearchPolicies;

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCurrentAuxiliaryData
        /// Función para armar excel 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GetCurrentAuxiliaryData(DataTable dataTable)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            var fontdetail = workbook.CreateFont();
            fontdetail.FontName = "Tahoma";
            fontdetail.FontHeightInPoints = 8;
            fontdetail.Boldweight = 3;

            ICellStyle Amountstyle = workbook.CreateCellStyle();
            Amountstyle.SetFont(fontdetail);
            Amountstyle.Alignment = HorizontalAlignment.Right;
            Amountstyle.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.RightBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.TopBorderColor = HSSFColor.Grey40Percent.Index;
            //Amountstyle.DataFormat = workbook.CreateDataFormat().GetFormat("$#,#0.00");
            Amountstyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.##");
            Amountstyle.BorderBottom = BorderStyle.Double;
            Amountstyle.BorderLeft = BorderStyle.Double;
            Amountstyle.BorderRight = BorderStyle.Double;
            Amountstyle.BorderTop = BorderStyle.Double;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkGreen.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            ICellStyle styledetalle = workbook.CreateCellStyle();
            styledetalle.SetFont(fontdetail);
            styledetalle.BottomBorderColor = HSSFColor.Black.Index;
            styledetalle.LeftBorderColor = HSSFColor.Black.Index;
            styledetalle.RightBorderColor = HSSFColor.Black.Index;
            styledetalle.TopBorderColor = HSSFColor.Black.Index;
            styledetalle.BorderBottom = BorderStyle.Thin;
            styledetalle.BorderLeft = BorderStyle.Thin;
            styledetalle.BorderRight = BorderStyle.Thin;
            styledetalle.BorderTop = BorderStyle.Thin;

            var headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0).SetCellValue(Global.Branch);
            headerRow.CreateCell(1).SetCellValue(Global.Prefix);
            headerRow.CreateCell(2).SetCellValue(Global.PolicyNumber);
            headerRow.CreateCell(3).SetCellValue(Global.EndorsementNumber);
            headerRow.CreateCell(4).SetCellValue(Global.Agent);
            headerRow.CreateCell(5).SetCellValue(Global.AgentName);
            headerRow.CreateCell(6).SetCellValue(Global.NoDocumentHolder);
            headerRow.CreateCell(7).SetCellValue(Global.LabelTaker);
            headerRow.CreateCell(8).SetCellValue(Global.Currency + "_ME");//moneda emision
            headerRow.CreateCell(9).SetCellValue(Global.Amount);
            headerRow.CreateCell(10).SetCellValue(Global.ExchangeRate);
            sheet.SetColumnWidth(0, 20 * 100);
            sheet.SetColumnWidth(1, 20 * 50);
            sheet.SetColumnWidth(2, 20 * 200);
            sheet.SetColumnWidth(3, 20 * 100);
            sheet.SetColumnWidth(4, 20 * 100);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 100);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 20 * 100);
            sheet.SetColumnWidth(9, 20 * 200);
            sheet.SetColumnWidth(10, 20 * 200);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(0).CellStyle = styleHeader;
            headerRow.GetCell(1).CellStyle = styleHeader;
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            headerRow.GetCell(8).CellStyle = styleHeader;
            headerRow.GetCell(9).CellStyle = styleHeader;
            headerRow.GetCell(10).CellStyle = styleHeader;

            int rowNumber = 1;
            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styledetalle;
                    if (row.GetCell(i).ColumnIndex == 9 || row.GetCell(i).ColumnIndex == 10)
                    {
                        row.CreateCell(i).SetCellValue(Convert.ToDouble(item.ItemArray[i]));
                        row.GetCell(i).CellStyle = Amountstyle;
                    }
                }
            }

            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            return output;
        }

        #endregion

        #region getdata
        public PremiumReceivableSearchPolicyDTO GetPolicyQuotaListView(string policyNumber, string documentNumber, string payerName, string endorsementId, string branch,
                                                 string holderDocumentNumber)
        {
            List<PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies = new List<PremiumReceivableSearchPolicyDTO>();


            string insuredId = "";
            string payerId = "";

            List<PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs;

            //List<PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPoliciesOrders;

            //POR ID DE PÓLIZA
            if (String.IsNullOrEmpty(policyNumber) || !_billingController.IsNumeric(policyNumber))
            {
                policyNumber = "";
            }

            //POR NO. DOCUMENTO DEL TITULAR DE LA PÓLIZA
            if (String.IsNullOrEmpty(holderDocumentNumber.Trim()) || !_billingController.IsNumeric(holderDocumentNumber.Trim())) //- el documento viene con espacios
            {
                holderDocumentNumber = "";
            }
            else
            {
                insuredId = holderDocumentNumber;
            }

            //POR ENDOSO
            if (String.IsNullOrEmpty(endorsementId) || endorsementId == "-1")
            {
                endorsementId = "";
            }

            //POR SUCURSAL
            if (String.IsNullOrEmpty(branch) || branch == "-1")
            {
                branch = "";
            }
            //esta buscando por #póliza
            Integration.UndewritingIntegrationServices.DTOs.SearchPolicyPaymentDTO searchPolicyPayment = new Integration.UndewritingIntegrationServices.DTOs.SearchPolicyPaymentDTO();
            searchPolicyPayment.InsuredId = insuredId;
            searchPolicyPayment.PayerId = payerId;
            searchPolicyPayment.PolicyDocumentNumber = policyNumber;
            searchPolicyPayment.BranchId = branch;
            searchPolicyPayment.EndorsementDocumentNumber = endorsementId;
            searchPolicyPayment.PageIndex = Convert.ToString(PageIndex);
            searchPolicyPayment.PageSize = Convert.ToString(PageSize);


            List<Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> premiumSearchPolicyDTOs =
                              DelegateService.accountingApplicationService.GetPaymentQuotas(searchPolicyPayment);
            //premiumReceivableSearchPolicyDTOs =
            // DelegateService.accountingApplicationService.PremiumReceivableSearchPolicy(insuredId, payerId, "", "", "", policyNumber, "",
            //                                                branch, "", endorsementId, "", "", "", PageSize, PageIndex);
            List<Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> premiumReceivableSearchPoliciesOrders;
            if (premiumSearchPolicyDTOs.Count > 0)
            {
                //Ordenamiento por Cuota/Endoso/Póliza
                premiumReceivableSearchPoliciesOrders = (from order in premiumSearchPolicyDTOs orderby order.PaymentNumber, order.EndorsementId, order.PolicyId select order).ToList();

                foreach (Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO premiumReceivable in premiumReceivableSearchPoliciesOrders)
                {
                    premiumReceivableSearchPolicies.Add(new PremiumReceivableSearchPolicyDTO
                    {
                        PolicyId = premiumReceivable.PolicyId,
                        PolicyDocumentNumber = premiumReceivable.PolicyDocumentNumber,
                        EndorsementDocumentNumber = premiumReceivable.EndorsementDocumentNumber,
                        BranchId = premiumReceivable.BranchId,
                        BranchDescription = premiumReceivable.BranchDescription,
                        CurrencyId = premiumReceivable.CurrencyId,
                        CurrencyDescription = premiumReceivable.CurrencyDescription,
                        PayerId = premiumReceivable.PayerId,
                        PayerDocumentNumber = premiumReceivable.PayerDocumentNumber,
                        PayerName = premiumReceivable.PayerName,
                        PaymentNumber = premiumReceivable.PaymentNumber,
                        PaymentAmount = premiumReceivable.Amount,
                        //ItemId = premiumReceivable.id,
                        EndorsementId = premiumReceivable.EndorsementId,
                        CollectGroupId = premiumReceivable.CollectGroupId,
                        BussinessTypeId = premiumReceivable.BussinessTypeId,
                        Amount = premiumReceivable.Amount,
                        InsuredId = premiumReceivable.InsuredId,
                        ExchangeRate = premiumReceivable.ExchangeRate,
                        PrefixTyniDescription = premiumReceivable.PrefixTinyDescription
                    });
                }
            }


            return premiumReceivableSearchPolicies.FirstOrDefault();

        }
        #endregion
    }
}