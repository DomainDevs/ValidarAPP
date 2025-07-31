//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;

//Excel
using Excel;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF2.Controls.UifTable;

//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using AccountingPaymentModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class TransferController : Controller
    {
        #region Constants

        public const string SortOrder = "ASC";
        public const string PathTransfer = "~/Temp/transfers"; //~/App_Data/transfers

        #endregion

        #region Instance Variables
        readonly BillingController _billingController = new BillingController();
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainTransfer
        /// Invoca a la vista de transferencias
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainTransfer()
        {
            return View("~/Areas/Accounting/Views/AccountsPayable/Transfer/MainTransfer.cshtml");
        }

        #endregion

        #region SearchTransfer

        /// <summary>
        /// SearchTransferPaymentOrders
        /// Busca transferencias de órdenes de pago
        /// </summary>
        /// <param name="paymentDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchTransferPaymentOrders(string paymentDate)
        {
            if (paymentDate == String.Empty)
            {
                paymentDate = "01/01/1900";
            }

            SCRDTO.SearchParameterPaymentOrdersDTO searchParameterPaymentOrders = new SCRDTO.SearchParameterPaymentOrdersDTO();

            searchParameterPaymentOrders.PaymentMethod = new AccountingPaymentModels.PaymentMethodDTO()
            {
                Id = Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"])
            };
            searchParameterPaymentOrders.PaymentDate = Convert.ToDateTime(paymentDate);
            searchParameterPaymentOrders.StatusId = Convert.ToInt32(PaymentOrderStatus.Active);

            List<SCRDTO.PaymentOrderDTO> paymentOrderDTOs = DelegateService.accountingAccountsPayableService.SearchTransferPaymentOrders(searchParameterPaymentOrders);

            List<object> paymentOrders = new List<object>();

            foreach (SCRDTO.PaymentOrderDTO paymentOrder in paymentOrderDTOs)
            {
                paymentOrders.Add(new
                {
                    AccountBankCode = paymentOrder.AccountBankCode,
                    AdmissionDate = paymentOrder.AdmissionDate,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentOrder.Amount),    
                    BankAccountNumber = paymentOrder.BankAccountNumberPerson,
                    BankCode = paymentOrder.BankCode,
                    BankName = paymentOrder.BankNamePerson,
                    BeneficiaryDocumentNumber = paymentOrder.BeneficiaryDocumentNumber,
                    BeneficiaryName = paymentOrder.BeneficiaryName,
                    BranchCode = paymentOrder.BranchCode,
                    BranchName = paymentOrder.BranchName,
                    BranchPayCode = paymentOrder.BranchPayCode,
                    BranchPayName = paymentOrder.BranchPayName,
                    CancellationDate = paymentOrder.CancellationDate,
                    CheckNumber = paymentOrder.CheckNumber,
                    CompanyCode = paymentOrder.CompanyCode,
                    CompanyName = paymentOrder.CompanyName,
                    CurrencyCode = paymentOrder.CurrencyCode,
                    CurrencyName = paymentOrder.CurrencyName,
                    EstimatedPaymentDate = Convert.ToDateTime(paymentOrder.EstimatedPaymentDate).ToShortDateString(),
                    ExchangeRate = paymentOrder.ExchangeRate,
                    PayerName = paymentOrder.PayerName,
                    IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentOrder.IncomeAmount),
                    IndividualId = paymentOrder.IndividualId,
                    PaymentMethodCode = paymentOrder.PaymentMethodCode,
                    PaymentMethodName = paymentOrder.PaymentMethodName,
                    PaymentOrderCode = paymentOrder.PaymentOrderCode,
                    PaymentSourceCode = paymentOrder.PaymentSourceCode,
                    PaymentSourceName = paymentOrder.PaymentSourceName,
                    PayTo = paymentOrder.PayTo,
                    PersonTypeCode = paymentOrder.PersonTypeCode,
                    PersonTypeName = paymentOrder.PersonTypeName,
                    TempImputationCode = paymentOrder.TempImputationCode,
                    UserId = paymentOrder.UserId,
                    UserName = paymentOrder.UserName,
                });
            }

            return new UifTableResult(paymentOrders);
        }

        #endregion

        #region SaveTransfer

        /// <summary>
        /// SaveTransferRequest
        /// Graba la transferencia de órden de pago
        /// </summary>
        /// <param name="transferPaymentOrderModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTransferRequest(TransferPaymentOrderModel transferPaymentOrderModel)
        {
            try
            {
                string transferPaymentOrderMessage = "";
                bool isEnabledGeneralLedger = true;

                TransferPaymentOrderDTO transferPaymentOrder = new TransferPaymentOrderDTO();

                transferPaymentOrder.Id = transferPaymentOrderModel.Id;
                transferPaymentOrder.BankAccountCompany = new BankAccountCompanyDTO();
                transferPaymentOrder.BankAccountCompany.Id = transferPaymentOrderModel.AccountBankId;
                transferPaymentOrder.DeliveryDate = transferPaymentOrderModel.DeliveryDate;
                transferPaymentOrder.Status = Convert.ToInt32(CollectStatus.Active);
                transferPaymentOrder.UserId = _commonController.GetUserByName(User.Identity.Name)[0].UserId;
                transferPaymentOrder.PaymentOrders = new List<PaymentOrderDTO>();

                if (transferPaymentOrderModel.PaymentOrdersItems != null)
                {
                    foreach (PaymentOrderTransferModel item in transferPaymentOrderModel.PaymentOrdersItems)
                    {
                        BankAccountPersonDTO accountBank = new BankAccountPersonDTO();
                        accountBank.Id = item.AccountBankId;
                        accountBank.Bank = new BankDTO();
                        accountBank.Bank.Id = item.BankId;
                        accountBank.Number = item.AccountBankNumber;

                        transferPaymentOrder.PaymentOrders.Add(new PaymentOrderDTO()
                        {
                            Id = item.PaymentOrderId,
                            BankAccountPerson = accountBank
                        });
                    }
                }

                transferPaymentOrder = DelegateService.accountingAccountsPayableService.SaveTransferRequest(transferPaymentOrder);

                #region Accounting

                // Disparo el método de contabilidad
                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    transferPaymentOrderMessage = RecordPaymentOrder(transferPaymentOrder.PaymentOrders[0].Id);
                }
                else
                {
                    transferPaymentOrderMessage = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                    isEnabledGeneralLedger = false;
                }

                #endregion Accounting

                var transferPaymentOrderResponse = new
                {
                    TransferPaymentOrder = Convert.ToString(transferPaymentOrder.Id),
                    TransferPaymentOrderMessage = transferPaymentOrderMessage,
                    IsEnabledGeneralLedger = isEnabledGeneralLedger,
                    PaymenOrderId = Convert.ToString(transferPaymentOrder.PaymentOrders[0].Id)
                };

                return Json(transferPaymentOrderResponse, JsonRequestBehavior.AllowGet);
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
        /// GenerateTransferFile
        /// Genera el archivo de transferencia, lo guarda en el servidor y lo envía al cliente para guardarlo localmente.
        /// </summary>
        /// <param name="transferPaymentOrderId"></param>
        /// <param name="paymentOrderId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GenerateTransferFile(int transferPaymentOrderId, int paymentOrderId)
        {
            TransferPaymentOrderDTO transferPaymentOrder = new TransferPaymentOrderDTO();
            transferPaymentOrder.Id = transferPaymentOrderId;
            transferPaymentOrder.BankAccountCompany = new BankAccountCompanyDTO();
            transferPaymentOrder.PaymentOrders = new List<PaymentOrderDTO>();
            PaymentOrderDTO paymentOrder = new PaymentOrderDTO();
            paymentOrder.Id = paymentOrderId;
            transferPaymentOrder.PaymentOrders.Add(paymentOrder);

            // Los archivos se guardaran en Temp/transfers
            var path = Server.MapPath(PathTransfer);
            string fileName = DelegateService.accountingAccountsPayableService.GenerateTransferFile(transferPaymentOrder, path); // Devuelve la ruta completa del archivo en el servidor

            return Json(fileName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download
        /// Genera el archivo de transferencia, lo guarda en el servidor y lo envía al cliente para guardarlo localmente.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult Download(string fileName)
        {
            byte[] path = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath(PathTransfer), fileName));

            return new FileDownloadResult(fileName, path);
        }

        #endregion

        #region TransferCancellation

        /// <summary>
        /// MainTransferNullification
        /// Invoca a la vista de anulación de transferencias
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainTransferNullification()
        {
            return View("~/Areas/Accounting/Views/AccountsPayable/Transfer/MainTransferNullification.cshtml");
        }

        ///<summary>
        /// LoadTransferCancellation
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="pathFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult LoadTransferCancellation(int accountBankId, string pathFile)
        {
            //Page Parameter
            Dictionary<string, string> gridSetting = new Dictionary<string, string>();
            gridSetting.Add("pageSize", Convert.ToString("1"));
            gridSetting.Add("pageIndex", Convert.ToString("1000"));
            gridSetting.Add("sortOrder", SortOrder);
            TempData["accountBankId"] = accountBankId;

            List<SCRDTO.PaymentOrderDTO> paymentOrderDTOs = new List<SCRDTO.PaymentOrderDTO>();

            if (pathFile != "" && pathFile != null)
            {
                try
                {
                    string[] data = pathFile.Split(new char[] { '.' });

                    if (data[1] == "csv")
                    {
                        paymentOrderDTOs = (List<SCRDTO.PaymentOrderDTO>)TempData["dtoCsv"];
                    }
                    if (data[1] == "xml")
                    {
                        paymentOrderDTOs = (List<SCRDTO.PaymentOrderDTO>)TempData["dtoXml"];
                    }

                    if ((data[1] == "xlsx") || (data[1] == "xls"))
                    {
                        paymentOrderDTOs = (List<SCRDTO.PaymentOrderDTO>)TempData["dtoExel"];
                    }
                    if (data[1] == "txt")
                    {
                        paymentOrderDTOs = (List<SCRDTO.PaymentOrderDTO>)TempData["dtoTxt"];
                    }
                }
                catch (Exception exc)
                {
                    throw new BusinessException(exc.Message);
                }
            }

            List<object> paymentOrders = new List<object>();

            foreach (SCRDTO.PaymentOrderDTO paymentOrder in paymentOrderDTOs)
            {
                paymentOrders.Add(new
                {
                    PaymentOrderCode = paymentOrder.PaymentOrderCode,
                    BeneficiaryName = paymentOrder.BeneficiaryName,
                    CancellationDate = paymentOrder.CancellationDate,
                    BankCode = paymentOrder.BankCode,
                    BankName = paymentOrder.BankName,
                    AccountBankCode = paymentOrder.AccountBankCode,
                    BankAccountNumber = paymentOrder.BankAccountNumber,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentOrder.Amount),
                    Status = paymentOrder.Status
                });
            }

            return new UifTableResult(paymentOrders);
        }

        /// <summary>
        /// CancelTransferBank
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="typeOperation"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CancelTransferBank(int paymentOrderId, int typeOperation)
        {
            int userId = 0;

            if (User == null)
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }
            else
            {
                userId = _commonController.GetUserIdByName(User.Identity.Name);
            }
            
            var transferBank = DelegateService.accountingAccountsPayableService.CancelTransferBank(paymentOrderId, userId, typeOperation);

            #region Accounting

            if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true") //contabilización para EE
            {
                // Reversión del asiento generado en la orden de pago, se obtiene el id del asiento
                int entryId = 0;                

                CollectApplicationDTO collectImputation = new CollectApplicationDTO();

                collectImputation.Collect = new CollectDTO() { Id = paymentOrderId };
                    collectImputation.Application = new ApplicationDTO() { ModuleId = Convert.ToInt32(ApplicationTypes.PaymentOrder) };

                List<CollectApplicationDTO> collectImputations = DelegateService.accountingCollectService.GetCollectImputations(collectImputation);

                if (collectImputations.Count > 0)
                {
                    entryId = collectImputations[0].Transaction.TechnicalTransaction;                    
                }

                // Se obtiene el asiento.
                GeneralLedgerModels.JournalEntryDTO journalEntry = new GeneralLedgerModels.JournalEntryDTO();

                if (entryId > 0)
                {
                    journalEntry.Id = entryId;
                    journalEntry = DelegateService.glAccountingApplicationService.GetJournalEntry(journalEntry);
                    DelegateService.glAccountingApplicationService.ReverseJournalEntry(journalEntry);
                }                
            }

            #endregion Accounting

            return Json(transferBank, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ReadFilesInMemory

        /// <summary>
        /// ReadFileInMemory
        /// Lee un archivo sin guardar
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="accountBankId"> </param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile, int accountBankId)
        {
            string fileLocationName = String.Empty;

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ExcelToStream(uploadedFile, accountBankId);
            }
            if (data[1] == "xml")
            {
                return XmlToStream(uploadedFile);
            }
            if (data[1] == "csv")
            {
                return CsvToStream(uploadedFile);
            }
            if (data[1] == "txt")
            {
                return TxtToString(uploadedFile, accountBankId);
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExcelToStream
        /// Lee un archivo en formato xls, xlsx sin guardar
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExcelToStream(HttpPostedFileBase uploadedFile, int accountBankId)
        {
            string fileLocationName = String.Empty;
            Byte[] arrayContent;

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y  guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);

            IExcelDataReader excelReader;

            if (data[1] == "xls")
            {
                //1. Lee desde  binary Excel  ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //2. Lee desde  binary OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            //3. DataSet - El resultado sera creado en result.Tables
            DataSet dataSet = excelReader.AsDataSet();

            //4. DataSet - Crea column names en primera fila
            excelReader.IsFirstRowAsColumnNames = true;

            List<SCRDTO.PaymentOrderDTO> paymentOrders = new List<SCRDTO.PaymentOrderDTO>();

            for (int i = 1; i < dataSet.Tables[0].Rows.Count; i++)
            {
                DataRow row = dataSet.Tables[0].Rows[i];

                try
                {
                    if (Convert.ToInt32(row[5]) == accountBankId)  //Filtra solo por la cuenta ingresada por el usuario
                    {
                        paymentOrders.Add(new SCRDTO.PaymentOrderDTO()
                        {
                            PaymentOrderCode = row[0] == DBNull.Value ? "" : Convert.ToString(row[0]),
                            BeneficiaryName = row[1] == DBNull.Value ? "" : Convert.ToString(row[1]),
                            CancellationDate = row[2] == DBNull.Value ? "" : Convert.ToString(row[2]),
                            BankName = row[4] == DBNull.Value ? "" : Convert.ToString(row[3]),
                            BankAccountNumber = row[6] == DBNull.Value ? "" : Convert.ToString(row[5]),
                            Amount = row[7] == DBNull.Value ? 0 : Convert.ToDecimal(Convert.ToString(row[6]).Replace(",", ".")),
                            Status = row[8] == DBNull.Value ? -1 : Convert.ToInt32(row[7])
                        });
                    }
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
            }
            TempData["dtoExel"] = paymentOrders;
            stream.Close();

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// IsNegative
        /// Verifica si es negativo un número
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
        /// ReadXMLFile
        /// Lee un archivo en formato xml sin guardar
        /// AUrresta
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult XmlToStream(HttpPostedFileBase uploadedFile)
        {
            try
            {
                string fileLocationName = String.Empty;
                Byte[] arrayContent;
                int accountBankId;
                int index = 0;

                fileLocationName = uploadedFile.FileName;

                // Convertir a Bytes
                var buffer = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

                // Leer el archivo y lo guarda en arreglo de typo byte y este a su vez a arrContent
                arrayContent = buffer;

                Stream stream = new MemoryStream(arrayContent);
                stream.Position = 0;
                List<SCRDTO.PaymentOrderDTO> paymentOrders = new List<SCRDTO.PaymentOrderDTO>();
                XmlDocument xDocument = new XmlDocument();
                xDocument.Load(stream);

                XmlNodeList policies = xDocument.GetElementsByTagName("policies");
                XmlNodeList policyNode = ((XmlElement)policies[0]).GetElementsByTagName("policy");

                foreach (XmlElement xmlElement in policyNode)
                {
                    try
                    {
                        XmlNodeList paymentOrderCode = xmlElement.GetElementsByTagName("paymentOrderCode");
                        XmlNodeList beneficiaryName = xmlElement.GetElementsByTagName("beneficiaryName");
                        XmlNodeList cancellationDate = xmlElement.GetElementsByTagName("cancellationDate");
                        XmlNodeList bankName = xmlElement.GetElementsByTagName("bankName");
                        XmlNodeList bankAccountNumber = xmlElement.GetElementsByTagName("bankAccountNumber");
                        XmlNodeList amount = xmlElement.GetElementsByTagName("amount");
                        XmlNodeList status = xmlElement.GetElementsByTagName("status");
                        XmlNodeList naccountBankId = xmlElement.GetElementsByTagName("accountBankId");

                        if (IsNegative(Convert.ToInt32(paymentOrderCode[0].InnerText)) || IsNegative(Convert.ToInt32(beneficiaryName[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(status[0].InnerText)) || IsNegative(Convert.ToInt32(bankName[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(bankAccountNumber[0].InnerText)))
                        {
                            fileLocationName = "NegativeId";
                            break;
                        }

                        accountBankId = Convert.ToInt32(TempData["accountBankId"] ?? 0);
                        if (Convert.ToInt32(naccountBankId) == accountBankId)  //Filtra solo por la cuenta ingresada por el usuario
                        {
                            paymentOrders.Add(new SCRDTO.PaymentOrderDTO()
                            {
                                PaymentOrderCode = Convert.ToString(paymentOrderCode[0].InnerText.Replace(",", ".")),
                                BeneficiaryName = Convert.ToString(beneficiaryName[0].InnerText),
                                CancellationDate = Convert.ToString(cancellationDate[0].InnerText),
                                BankName = Convert.ToString(bankName[0].InnerText),
                                BankAccountNumber = Convert.ToString(bankAccountNumber[0].InnerText),
                                Amount = Convert.ToDecimal(amount[0].InnerText),
                                Status = Convert.ToInt32(status[0].InnerText),
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

                TempData["dtoXml"] = paymentOrders;

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
        /// CsvToStream
        /// Lee un archivo en formato csv 
        /// </summary>
        /// <param name="uploadedFile"> </param>
        /// <returns>JsonResult</returns>
        private JsonResult CsvToStream(HttpPostedFileBase uploadedFile)
        {
            string fileLocationName = String.Empty;
            Byte[] arrayContent;
            int accountBankId;
            fileLocationName = uploadedFile.FileName;

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            List<SCRDTO.PaymentOrderDTO> paymentOrders = new List<SCRDTO.PaymentOrderDTO>();

            try
            {
                int index = 0;
                StreamReader streamReader = new StreamReader(stream);
                string line = String.Empty;

                while (line != null)
                {
                    try
                    {
                        line = streamReader.ReadLine();
                        if ((line != null) && (index > 0))
                        {
                            string[] data = line.Split(new char[] { ';' });
                            if (IsNegative(Convert.ToInt32(data[6])) || IsNegative(Convert.ToInt32(data[7])))
                            {
                                fileLocationName = "NegativeId";
                                break;
                            }

                            accountBankId = Convert.ToInt32(TempData["accountBankId"] ?? 0);
                            if (Convert.ToInt32(data[4]) == accountBankId)  //Filtra solo por la cuenta ingresada por el usuario
                            {
                                paymentOrders.Add(new SCRDTO.PaymentOrderDTO()
                                {
                                    PaymentOrderCode = Convert.ToString(data[0]),
                                    BeneficiaryName = Convert.ToString(data[1]),
                                    CancellationDate = Convert.ToString(data[2]),
                                    BankName = Convert.ToString(data[3]),
                                    BankAccountNumber = Convert.ToString(data[5]),
                                    Amount = Convert.ToDecimal(data[6]),
                                    Status = Convert.ToInt32(data[7])
                                });
                            }
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
            catch (Exception exc)
            {
                throw new BusinessException(exc.Message);
            }

            TempData["dtoCsv"] = paymentOrders;

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// TxtToString
        /// Lee las transferencias rechazadas desde un archivo txt
        /// AUrresta
        /// </summary>
        /// <param name="uploadedFile"> </param>
        /// <param name="accountBankId"> </param>
        /// <returns>JsonResult</returns>
        private JsonResult TxtToString(HttpPostedFileBase uploadedFile, int accountBankId)
        {
            string fileLocationName = String.Empty;
            Byte[] arrayContent;
            fileLocationName = uploadedFile.FileName;
            List<SCRDTO.PaymentOrderDTO> paymentOrders;
            string issuingAccountBankId = "";

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            try
            {
                paymentOrders = new List<SCRDTO.PaymentOrderDTO>();
                StreamReader file = new StreamReader(stream);

                int index = 0;
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        string paymentOrderCode = line.Substring(0, 10).Trim();
                        string beneficiaryName = line.Substring(10, 30).Trim();
                        string cancellationDate = line.Substring(40, 11).Trim();
                        string bankCode = line.Substring(51, 3).Trim();
                        string bankName = line.Substring(54, 26).Trim();
                        string accountBankCode = line.Substring(82, 7).Trim();
                        string bankAccountNumber = line.Substring(92, 12).Trim();
                        string amount = line.Substring(104, 13).Trim();
                        string status = line.Substring(119, 1).Trim();

                        //issuingAccountBankId = line.Substring(111, 3).Trim(); //ID DE LA CUENTA EMISORA
                        issuingAccountBankId = line.Substring(82, 7).Trim(); //ID DE LA CUENTA EMISORA

                        //FILTRA POR LOS ID DE LA CUENTA EMISORA
                        if (Convert.ToInt32(issuingAccountBankId) == accountBankId)
                        {
                            paymentOrders.Add(new SCRDTO.PaymentOrderDTO()
                            {
                                PaymentOrderCode = paymentOrderCode,
                                BeneficiaryName = beneficiaryName,
                                CancellationDate = cancellationDate,
                                BankCode = Convert.ToInt32(bankCode),
                                BankName = bankName,
                                AccountBankCode = Convert.ToInt32(accountBankCode),
                                BankAccountNumber = bankAccountNumber,
                                Amount = Convert.ToDecimal(amount.Replace(",", ".")),
                                Status = Convert.ToInt32(status)
                            });
                        }

                        index++;
                    }
                    catch (FormatException ex)
                    {
                        fileLocationName = "FormatException" + ex.Message;
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
                file.Close();
            }
            catch (Exception exc)
            {
                throw new BusinessException(exc.Message);
            }

            TempData["dtoTxt"] = paymentOrders;

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AccountPaymentOrder

        /// <summary>
        /// RecordPaymentOrder
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>string</returns>
        public string RecordPaymentOrder(int paymentOrderId)
        {
            string message = "";

            int moduleDateId = 0;
            int moduleId = 0;
            int subModuleId = (int)PaymentTypes.Transfer;

            try
            {
                List<SCRDTO.ImputationParameterDTO> imputationParameters = DelegateService.accountingAccountService.GetImputationParameters(paymentOrderId, Convert.ToInt32(@Global.ImputationTypePaymentOrder),
                    _commonController.GetUserIdByName(User.Identity.Name), moduleId, subModuleId, moduleDateId);
                message = _billingController.RecordImputation(imputationParameters, _commonController.GetUserIdByName(User.Identity.Name), 2);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
            return message;
        }
        #endregion

    }
}