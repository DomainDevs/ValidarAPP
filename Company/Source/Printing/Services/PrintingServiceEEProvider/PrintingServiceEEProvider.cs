using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.PrintingServicesEEProvider.DAOs;
using Sistran.Company.Application.PrintingServicesEEProvider.Resources;
using Sistran.Core.Application.PrintingServicesEEProvider;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using JTFORM = Sistran.Company.PrintingService.JetForm.Clases;
using Sistran.Company.Application.PrintingServices.Clases;
using Sistran.Company.PrintingService.JetForm.Clases;
using System.Configuration;
using Sistran.Company.Application.PrintingServices.Enums;
using System.Text;
using Ionic.Zip;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Concurrent;
using System.Linq;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.PrintingServicesEEProvider
{
    using Sistran.Company.Application.CollectionFormBusinessService.Models;
    using Sistran.Company.Application.PrintingServices.Models;
    using Sistran.Company.Application.PrintingServicesNase.Enum;
    using Sistran.Core.Application.PrintingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using System.Globalization;
    using System.Net;

    public class PrintingServiceEEProvider : PrintingServiceEEProviderCore, IPrintingService
    {
        private static string reportDigital = ConfigurationSettings.AppSettings["PathDigitalFirm"];
        private static string reportSigned = ConfigurationSettings.AppSettings["SignedPath"];
        private static string reportNotSigned = ConfigurationSettings.AppSettings["NotSignedPath"];
        int repeatInterval = int.Parse(ConfigurationSettings.AppSettings["RepeatIntervale"].ToString());
        int intervalSeconds = int.Parse(ConfigurationSettings.AppSettings["TimeInternalSeconds"].ToString());
        private static string UserServiceConsumer = (ConfigurationSettings.AppSettings["UserServiceConsumer"]);
        private static string PasswordServiceConsumer = (ConfigurationSettings.AppSettings["PasswordServiceConsumer"]);
        private static string DomainOfUser = (ConfigurationSettings.AppSettings["DomainOfUser"]);
        private static string reportSISE = ConfigurationSettings.AppSettings["ReportExportPath"];

        /// <summary>
        /// Generar reporte de un póliza de individual
        /// </summary>
        /// <param name="filterReport">Filtro</param>
        /// <returns>Ruta Reporte</returns>
        public string GenerateReport(CompanyFilterReport companyFilterReport)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                return printingDAO.GenerateReport(companyFilterReport);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateReport), ex);
            }
        }

        /// <summary>
        /// Generar reporte masivo
        /// </summary>
        /// <param name = "companyFilterReports" > Lista de Pólizas</param>
        /// <returns>Id Proceso</returns>
        public string GenerateReportMassive(List<CompanyFilterReport> companyFilterReports, int massiveLoadId)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                return printingDAO.GenerateReportMassive(companyFilterReports, massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateReportMassive), ex);
            }
        }

        /// <summary>
        /// Generar Reporte Colectivo
        /// </summary>
        /// <param name="companyFilterReport">Póliza</param>
        /// <returns>Id Proceso</returns>
        public string GenerateReportCollective(List<CompanyFilterReport> companyFilterReports, int massiveLoadId)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                return printingDAO.GenerateReportCollective(companyFilterReports, massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateReportCollective), ex);
            }
        }

        public string GenerateReportMassiveJetForm(List<PrintingInfo> lstPrintingInfos, string urlPdf, int printingType, int userId, int totalRisks, int massiveLoadId, bool collectFormat, string[] cuotas)
        {
            try
            {
                CompanyPrinting companyPrinting = SavePrintingProcess(urlPdf, printingType, userId, totalRisks, massiveLoadId);
                TP.Task.Run(() => SaveReportMassive(lstPrintingInfos, companyPrinting.Id, collectFormat, cuotas));
                return companyPrinting.Id.ToString();
            }
            catch (Exception ex)
            {
                return "Error Generando archivos: " + ex.Message;
            }
        }

        private void SaveReportMassive(List<PrintingInfo> lstPrintingInfos, int processPrintingId, bool collectFormat, string[] cuotas)
        {
            CompanyPrinting companyPrintingSave = new CompanyPrinting();

            try
            {
                string sUrlFile = string.Empty;
                string sUrlCollectFormatFile = string.Empty;
                int Id = 1;
                DataSet dsIdsForPrinting = JTFORM.ReportServiceHelper.GetIDsforMassivePrinter(int.Parse(ConfigurationManager.AppSettings["PARAM_REPORT"].ToString()), lstPrintingInfos.Count);
                int IdImpresionDesde = int.Parse(dsIdsForPrinting.Tables[0].Rows[0]["CURRENT_ID"].ToString());

                foreach (PrintingInfo printingInfo in lstPrintingInfos)
                {
                    if (printingInfo is PolicyInfo policyInfo)
                    {
                        IdImpresionDesde = IdImpresionDesde + 1;
                        policyInfo.NumberReport = IdImpresionDesde;
                    }
                }

                foreach (var item in lstPrintingInfos)
                {
                    sUrlFile = GenerateReportJetForm(item);

                    if (collectFormat)
                    {
                        if (item is PolicyInfo policyInfo)
                        {
                            List<ReportEndorsement> lstEndorsements = GetPolicyForCollectFormat((int)policyInfo.BranchId, policyInfo.CommonProperties.PrefixId, (int)policyInfo.PolicyNumber);
                            if (lstEndorsements != null)
                            {
                                sUrlCollectFormatFile = GenerateReportCollectFormat(lstEndorsements, cuotas, (int)policyInfo.BranchId, policyInfo.CommonProperties.PrefixId, (int)policyInfo.EndorsementNum, (int)policyInfo.PolicyNumber);
                                sUrlFile = sUrlFile + ";" + sUrlCollectFormatFile;
                            }
                        }
                    }

                    SavePrintingLog(Id, processPrintingId, sUrlFile, 1, 0);
                    Id++;
                }

                CompanyPrinting companyPrinting = GetPrintingProcess(processPrintingId);
                companyPrinting.UrlFileError = "Validando archivos";
                companyPrinting.FinishDate = Convert.ToDateTime("01/01/1900");
                UpdatePrintingProcess(companyPrinting);

                List<CompanyPrintingLog> companyPrintingLogs = GetPrintingProcessLog(processPrintingId);
                ValidateUrlFiles(companyPrintingLogs, processPrintingId);

                companyPrinting.UrlFileError = string.Empty;
                companyPrinting.FinishDate = DateTime.Now;
                UpdatePrintingProcess(companyPrinting);
            }
            catch (Exception ex)
            {
                companyPrintingSave = GetPrintingProcess(processPrintingId);
                companyPrintingSave.UrlFile = "";
                companyPrintingSave.FinishDate = DateTime.Now;
                companyPrintingSave.HasError = true;
                companyPrintingSave.UrlFileError = ex.Message;
                UpdatePrintingProcess(companyPrintingSave);
            }
        }

        /// <summary>
        /// Genera el reporte por medio de JetForm
        /// </summary>
        /// <param name="printingInfo"></param>
        /// <returns></returns>
        public string GenerateReportJetForm(PrintingInfo printingInfo)
        {
            try
            {
                int processId = GetPrinterProcessId("PARAM_REPORT");

                if (printingInfo is PolicyInfo policyInfo)
                {

                    if (policyInfo.NumberReport == null)
                    {
                        policyInfo.NumberReport = processId;
                    }

                    ReportPr report = new ReportPr(0, (int)policyInfo.NumberReport, 0, 0, policyInfo.PolicyId, policyInfo.EndorsementId, 0, (int)ReportType.COMPLETE_POLICY, string.Empty, policyInfo.CommonProperties.UserName, 0, policyInfo.CommonProperties.PrefixId, policyInfo.CommonProperties.RiskSince, policyInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)policyInfo.CommonProperties.IsMassive, (bool)policyInfo.IsCollective);
                    DataSet dsOutput = new DataSet();
                    StringReader sr = new StringReader(PrinterHelper.getPrintedFileInfo(report));
                    dsOutput.ReadXml(sr);
                    DataSet dsReportResponse = dsOutput;
                    JTFORM.PendingPrint pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                    pending.FileName = Path.GetFileName(pending.FilePath);



                    return pending.FilePath;

                }
                else if (printingInfo is TemporaryInfo temporaryInfo)
                {
                    ReportPr report = new ReportPr(0, processId, temporaryInfo.OperationId, 0, 0, 0, 0, (int)ReportType.TEMPORARY, string.Empty, temporaryInfo.CommonProperties.UserName, temporaryInfo.TempId, temporaryInfo.CommonProperties.PrefixId, temporaryInfo.CommonProperties.RiskSince, temporaryInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)temporaryInfo.CommonProperties.IsMassive, (bool)temporaryInfo.IsCollective);
                    DataSet dsOutput = new DataSet();
                    StringReader sr = new StringReader(PrinterHelper.getPrintedFileInfo(report));
                    dsOutput.ReadXml(sr);
                    DataSet dsReportResponse = dsOutput;
                    JTFORM.PendingPrint pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                    pending.FileName = Path.GetFileName(pending.FilePath);

                    return pending.FilePath;
                }
                else if (printingInfo is QuotationInfo quotationInfo)
                {

                    ReportPr report = new ReportPr(0, processId, 0, 0, 0, 0, quotationInfo.QuotationId, (int)ReportType.QUOTATION, string.Empty, quotationInfo.CommonProperties.UserName, quotationInfo.TempId, quotationInfo.CommonProperties.PrefixId, quotationInfo.CommonProperties.RiskSince, quotationInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, false, false);
                    DataSet dsOutput = new DataSet();
                    StringReader sr = new StringReader(PrinterHelper.getPrintedFileInfo(report));
                    dsOutput.ReadXml(sr);
                    DataSet dsReportResponse = dsOutput;
                    JTFORM.PendingPrint pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                    pending.FileName = Path.GetFileName(pending.FilePath);
                    return pending.FilePath;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        private int GetPrinterProcessId(string keyName)
        {
            Parameter parameterPrinter = new Parameter();
            parameterPrinter = DelegateService.commonService.GetExtendedParameterByParameterId(int.Parse(ConfigurationManager.AppSettings[keyName].ToString()));
            parameterPrinter.NumberParameter = parameterPrinter.NumberParameter + 1;
            DelegateService.commonService.UpdateExtendedParameter(parameterPrinter);
            return (int)parameterPrinter.NumberParameter;
        }

        public TemporaryInfo GetTemporaryByTempID(int tempID)
        {
            try
            {
                DataSet tempInfo = JTFORM.ReportServiceHelper.GetTemporaryByTempId(tempID);
                TemporaryInfo temporaryInfo = new TemporaryInfo();
                CommonProperties commonProperties = new CommonProperties();
                commonProperties.PrefixId = int.Parse(tempInfo.Tables[0].Rows[0]["PREFIX_CD"].ToString());
                temporaryInfo.CommonProperties = commonProperties;
                temporaryInfo.TempId = int.Parse(tempInfo.Tables[0].Rows[0]["TEMP_ID"].ToString());
                commonProperties.RiskSince = int.Parse(tempInfo.Tables[0].Rows[0]["RIESGO_INI"].ToString());
                commonProperties.RiskUntil = int.Parse(tempInfo.Tables[0].Rows[0]["RIESGO_FIN"].ToString());
                return temporaryInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<QuotationInfo> GetQuotationByQuotationID(int quotationID, int branchID, int PrefixID)
        {
            try
            {
                DataSet tempInfo = JTFORM.ReportServiceHelper.GetQuotationByQuotationId(quotationID, branchID, PrefixID);
                List<QuotationInfo> quotationInfos = new List<QuotationInfo>();
                CommonProperties commonProperties = new CommonProperties();

                for (int i = 0; i < tempInfo.Tables[0].Rows.Count; i++)
                {
                    QuotationInfo quotationInfo = new QuotationInfo();
                    commonProperties.PrefixId = int.Parse(tempInfo.Tables[0].Rows[i]["PREFIX_CD"].ToString());
                    quotationInfo.CommonProperties = commonProperties;
                    quotationInfo.TempId = int.Parse(tempInfo.Tables[0].Rows[i]["TEMP_ID"].ToString());
                    quotationInfo.VersionId = int.Parse(tempInfo.Tables[0].Rows[i]["QUOT_VERSION"].ToString());
                    quotationInfo.QuotationId = quotationID;
                    if (quotationInfos.Exists(x => x.QuotationId == quotationID && x.TempId == quotationInfo.TempId && x.VersionId == quotationInfo.VersionId))
                    {
                        //Si existe no lo vuelve a agregar
                    }
                    else
                    {
                        quotationInfos.Add(quotationInfo);
                    }
                }
                return quotationInfos;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public DataTable GetRisksByPolicyId(int branchId, int prefixId, int policyNumber)
        {
            try
            {
                DataSet risks = JTFORM.ReportServiceHelper.GetRisksForPrintingByPolicyId(policyNumber, branchId, prefixId);
                return risks.Tables[0];
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public PolicyInfo GetRisksByEndorsementId(int endorsementId)
        {
            try
            {
                DataSet policyInfoData = JTFORM.ReportServiceHelper.GetRisksByEndorsementId(endorsementId);
                PolicyInfo policyInfo = new PolicyInfo();
                CommonProperties commonProperties = new CommonProperties();
                commonProperties.RiskSince = int.Parse(policyInfoData.Tables[0].Rows[0]["RIESGO_INI"].ToString());
                commonProperties.RiskUntil = int.Parse(policyInfoData.Tables[0].Rows[0]["RIESGO_FIN"].ToString());
                policyInfo.CommonProperties = commonProperties;
                return policyInfo;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public TemporaryInfo GetTemporaryByOpetarionId(int operationId, bool tempAutho = false)
        {
            try
            {
                DataSet temporaryData = JTFORM.ReportServiceHelper.GetTemporaryByOperationId(operationId);
                TemporaryInfo temporaryInfo = new TemporaryInfo();
                //No se imprimen temporales de cotización = 4 (TMP.TEMP_TYPE)
                if (int.Parse(temporaryData.Tables[0].Rows[0]["TEMP_TYPE_CD"].ToString()) != 4)
                {
                    //if (tempAutho)
                    //{
                    //    DataSet authoData = Sistran.Company.PrintingService.NASE.Clases.ReportServiceHelper.getDataOperationAutho(operationId);
                    //    if (authoData.Tables[0].Rows.Count == 0)
                    //    {
                    //        throw new BusinessException("Temporary Authorization");
                    //    }
                    //}
                    temporaryInfo = GetTemporaryByTempID(int.Parse(temporaryData.Tables[0].Rows[0]["TEMP_ID"].ToString()));
                    return temporaryInfo;
                }
                else
                {
                    throw new BusinessException("Temporary Quotation");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }

        }

        public CompanyPrinting SavePrintingProcess(string urlPdf, int printingType, int userId, int totalRisks, int massiveLoadId)
        {
            try
            {
                int processId = GetPrinterProcessId("PARAM_PRINTING_PROCESS");

                CompanyPrinting printing = new CompanyPrinting
                {
                    Id = processId,
                    PrintingTypeId = printingType,
                    KeyId = massiveLoadId,
                    UrlFile = urlPdf,
                    Total = totalRisks,
                    BeginDate = DateTime.Now,
                    FinishDate = DateTime.Now,
                    UserId = userId,
                    HasError = false,
                    UrlFileError = string.Empty
                };

                JTFORM.ReportServiceHelper.SavePrinting(printing, 0);

                return printing;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public CompanyPrinting UpdatePrintingProcess(CompanyPrinting companyPrinting)
        {
            try
            {
                DataSet companyPrintingData = JTFORM.ReportServiceHelper.SavePrinting(companyPrinting, 1);
                return companyPrinting;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public CompanyPrinting GetPrintingProcess(int processId)
        {
            try
            {
                DataSet printingData = JTFORM.ReportServiceHelper.GetPrintingByPrintingId(processId);
                CompanyPrinting companyPrinting = new CompanyPrinting
                {
                    Id = int.Parse(printingData.Tables[0].Rows[0]["ID"].ToString()),
                    PrintingTypeId = int.Parse(printingData.Tables[0].Rows[0]["PRINTING_TYPE_ID"].ToString()),
                    KeyId = int.Parse(printingData.Tables[0].Rows[0]["KEY_ID"].ToString()),
                    UrlFile = printingData.Tables[0].Rows[0]["URL_FILE"].ToString(),
                    Total = int.Parse(printingData.Tables[0].Rows[0]["TOTAL"].ToString()),
                    BeginDate = DateTime.Parse(printingData.Tables[0].Rows[0]["BEGIN_DATE"].ToString()),
                    FinishDate = (!string.IsNullOrEmpty(printingData.Tables[0].Rows[0]["FINISH_DATE"].ToString())) ? (DateTime?)DateTime.Parse(printingData.Tables[0].Rows[0]["FINISH_DATE"].ToString()) : null,
                    UserId = int.Parse(printingData.Tables[0].Rows[0]["USER_ID"].ToString()),
                    HasError = bool.Parse(printingData.Tables[0].Rows[0]["HAS_ERROR"].ToString()),
                    UrlFileError = printingData.Tables[0].Rows[0]["URL_FILE_ERROR"].ToString()
                };

                return companyPrinting;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public List<CompanyPrintingLog> ValidatePrintingByPrintingId(int printingProcessId)
        {
            try
            {
                CompanyPrinting companyPrinting = GetPrintingProcess(printingProcessId);
                List<CompanyPrintingLog> companyPrintingLogs = GetPrintingProcessLog(printingProcessId);

                if (companyPrinting.UrlFileError == "Validando archivos")
                {
                    return companyPrintingLogs;
                }
                else
                {
                    if (companyPrintingLogs.Where(x => x.StatusCd == 1).ToList().Count > 0)
                    {
                        TP.Task.Run(() => ValidateUrlFiles(companyPrintingLogs.Where(x => x.StatusCd == 1).ToList(), printingProcessId));
                    }

                    return companyPrintingLogs;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        private void ValidateUrlFiles(List<CompanyPrintingLog> companyPrintingLogs, int printingProcessId)
        {
            try
            {
                CompanyPrinting companyPrinting = GetPrintingProcess(printingProcessId);
                companyPrinting.UrlFileError = "Validando archivos";
                companyPrinting.FinishDate = Convert.ToDateTime("01/01/1900");
                UpdatePrintingProcess(companyPrinting);

                string strPathError = string.Empty;

                foreach (var item in companyPrintingLogs)
                {
                    string[] sPathPoliza = item.UrlFile.Split(';');

                    strPathError = ConfigurationManager.AppSettings["PathError"].ToString() + sPathPoliza[0].ToString().Replace(".pdf", ".err");

                    if (!string.IsNullOrEmpty(sPathPoliza[0]))
                    {
                        if (File.Exists(sPathPoliza[0]))
                        {
                            SavePrintingLog(item.Id, item.PrintingId, item.UrlFile, 2, 1);
                        }
                        else if (File.Exists(strPathError))
                        {
                            SavePrintingLog(item.Id, item.PrintingId, item.UrlFile, 3, 1);
                        }
                    }
                }

                companyPrinting.UrlFileError = string.Empty;
                companyPrinting.FinishDate = DateTime.Now;
                UpdatePrintingProcess(companyPrinting);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public List<CompanyPrintingLog> GetPrintingProcessLog(int processId)
        {
            try
            {
                DataSet printingData = JTFORM.ReportServiceHelper.GetPrintingLogByPrintingId(processId);
                List<CompanyPrintingLog> companyPrintingLogs = new List<CompanyPrintingLog>();

                for (int i = 0; i < printingData.Tables[0].Rows.Count; i++)
                {
                    CompanyPrintingLog companyPrintingLog = new CompanyPrintingLog
                    {
                        Id = int.Parse(printingData.Tables[0].Rows[i]["ID"].ToString()),
                        PrintingId = int.Parse(printingData.Tables[0].Rows[i]["PRINTING_ID"].ToString()),
                        UrlFile = printingData.Tables[0].Rows[i]["DESCRIPTION"].ToString(),
                        StatusCd = int.Parse(printingData.Tables[0].Rows[i]["STATUS_CD"].ToString())
                    };
                    companyPrintingLogs.Add(companyPrintingLog);
                }

                return companyPrintingLogs;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public void SavePrintingLog(int Id, int PrintingId, string UrlFile, int StatusCd, int TypeOperation)
        {
            try
            {
                CompanyPrintingLog companyPrintingLog = new CompanyPrintingLog
                {
                    Id = Id,
                    PrintingId = PrintingId,
                    UrlFile = UrlFile,
                    StatusCd = StatusCd
                };

                JTFORM.ReportServiceHelper.SavePrintingLog(companyPrintingLog, TypeOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public string GenerateZipFile(List<CompanyPrintingLog> companyPrintingLogs)
        {
            try
            {
                string date = DateTime.Now.ToString("ddMMyyyyhhmmssfff");
                StringBuilder sb = new StringBuilder();
                string sPathZip = sb.Append(ConfigurationManager.AppSettings["UserRemotePath"]).Append(0).Append("_").Append(date).Append(".zip").ToString();
                ConcurrentBag<string> lstPdfPaths = new ConcurrentBag<string>();
                string strPathWithCollectFormat = string.Empty;

                foreach (var item in companyPrintingLogs.Where(x => x.StatusCd == 2))
                {
                    string[] sPathPoliza = item.UrlFile.Split(';');

                    if (sPathPoliza.Count() > 1)
                    {
                        if (sPathPoliza[1].ToString() != "Error en la generacion del formato de recaudo")
                        {
                            strPathWithCollectFormat = sPathPoliza[0].ToString().Replace(".pdf", "_FR.pdf");
                            MergeFiles(strPathWithCollectFormat, sPathPoliza);
                            lstPdfPaths.Add(strPathWithCollectFormat);
                        }
                        else
                        {
                            lstPdfPaths.Add(sPathPoliza[0]);
                        }
                    }
                    else
                    {
                        lstPdfPaths.Add(item.UrlFile);
                    }
                }

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(lstPdfPaths);
                    zip.Save(sPathZip);
                }

                return sPathZip;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public bool MergeFiles(string destinationFile, string[] sourceFiles)
        {
            try
            {
                JTFORM.ReportServiceHelper.MergeFiles(destinationFile, sourceFiles);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        #region CollectFormat

        private List<ReportEndorsement> GetPolicyForCollectFormat(int branchId, int prefixId, int policyNumber)
        {
            try
            {
                ReportPolicy reportPolicy = DelegateService.collectionFormBusinessService.GetPolicybyBranchPrefixDocument(branchId, prefixId, policyNumber);
                List<ReportEndorsement> listEndorsements = new List<ReportEndorsement>();
                bool bError = false;

                if (reportPolicy.Endorsements != null)
                {
                    for (int i = 0; i < reportPolicy.Endorsements.Count; i++)
                    {
                        ReportEndorsement endorsementList = new ReportEndorsement();

                        endorsementList.EndorsementNumber = reportPolicy.Endorsements[i].Id;
                        endorsementList.BranchId = Convert.ToInt32(branchId);
                        endorsementList.PrefixId = Convert.ToInt32(prefixId);
                        endorsementList.DocumentNumber = policyNumber.ToString();
                        endorsementList.FailureText = "OK";
                        endorsementList.Quotes = new List<PolicyQuote>();
                        for (int j = 0; j < reportPolicy.Endorsements[i].Payers.Count; j++)
                        {
                            for (int k = 0; k < reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments.Count; k++)
                            {
                                PolicyQuote policyQuote = new PolicyQuote();

                                if (Convert.ToDateTime(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].DueDate).Date < DateTime.Now.Date)
                                {
                                    policyQuote.PayerName = "-1";
                                    endorsementList.Quotes.Add(policyQuote);
                                    bError = true;
                                    break;
                                }
                                else
                                {
                                    policyQuote.PayerName = reportPolicy.Endorsements[i].Payers[j].Name;
                                    policyQuote.IndividualPayerId = reportPolicy.Endorsements[i].Payers[j].IndividualId;
                                    policyQuote.QuoteNumber = reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].InstallmentNumber;
                                    policyQuote.Date = (Convert.ToDateTime(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].DueDate)).ToString("dd/MM/yyyy");
                                    policyQuote.TotalValue = Convert.ToDouble(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].Amount.Value);
                                    policyQuote.State = (!reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].IsPartialPaid) ? "PENDIENTE" : "PARCIAL";
                                    policyQuote.QuoteValue = Convert.ToDouble(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].PaidAmount.Value);
                                    endorsementList.Quotes.Add(policyQuote);
                                    bError = false;
                                }
                            }
                        }
                        listEndorsements.Add(endorsementList);
                    }
                }
                if (!bError)
                {
                    return listEndorsements;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GenerateReportCollectFormat(List<ReportEndorsement> endorsementList, string[] idCuotes, int branchId, int prefixId, int endorsementId, int policyNumber)
        {
            string[] paths = new String[2];
            CultureInfo co = new CultureInfo("es-CO");

            try
            {

                CollectionForm collectionForm = new CollectionForm();
                ReportPolicy reportPolicy = new ReportPolicy();
                List<ReportEndorsement> endorsements = new List<ReportEndorsement>();

                if (idCuotes != null)
                {
                    foreach (var endoso in endorsementList.Where(x => x.EndorsementNumber == endorsementId))
                    {
                        reportPolicy.BranchId = endoso.BranchId;
                        reportPolicy.PrefixId = endoso.PrefixId;
                        reportPolicy.DocumentNumber = policyNumber;
                        reportPolicy.EndorsementId = policyNumber;
                        ReportEndorsement endorsement = new ReportEndorsement();
                        endorsement.EndorsementNumber = endoso.EndorsementNumber;
                        List<ReportPayer> lstPayers = new List<ReportPayer>();

                        var insured = DelegateService.uniquePersonService.GetInsuredByIndividualId(endoso.Quotes[0].IndividualPayerId);

                        List<ReportPayer> lstTeportPayers = new List<ReportPayer>();
                        foreach (var quote in endoso.Quotes)
                        {
                            for (Int32 i = 0; i < idCuotes.Count(); i++)
                            {
                                if (Convert.ToInt32(idCuotes[i]) == quote.QuoteNumber)
                                {
                                    ReportPayer payer = new ReportPayer();
                                    payer.IndividualId = insured.IndividualId;
                                    payer.Name = quote.PayerName;
                                    ReportPaymentSchedule paymentSchedule = new ReportPaymentSchedule();
                                    List<ReportInstallment> listInstallment = new List<ReportInstallment>();
                                    ReportInstallment installment = new ReportInstallment();
                                    installment.DueDate = Convert.ToDateTime(quote.Date, co);
                                    installment.InstallmentNumber = quote.QuoteNumber;
                                    installment.IsPaid = (quote.State.Equals("PENDIENTE")) ? Convert.ToBoolean(0) : Convert.ToBoolean(1);
                                    ReportAmount paidAmount = new ReportAmount();
                                    paidAmount.Value = Convert.ToDecimal(quote.QuoteValue);
                                    installment.PaidAmount = paidAmount;
                                    listInstallment.Add(installment);
                                    paymentSchedule.Installments = listInstallment;
                                    payer.PaymentSchedule = paymentSchedule;
                                    lstTeportPayers.Add(payer);
                                }
                            }
                        }

                        endorsement.Payers = lstTeportPayers;
                        endorsements.Add(endorsement);
                    }
                }
                reportPolicy.Endorsements = endorsements;
                reportPolicy.userId = 1;
                collectionForm.Policy = reportPolicy;

                paths = DelegateService.collectionFormBusinessService.GenerateCollectionForm(collectionForm);
                string strPath = paths[0];
                if (strPath != "-1")
                {
                    var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();
                    return strPath;
                }
                else
                {
                    return "Error en la generacion del formato de recaudo";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion


        public string GenerateReportNase(PrintingInfo printingInfo)
        {
            try
            {

                int processId = GetPrinterProcessId("PARAM_REPORT");

                if (printingInfo is PolicyInfo policyInfo)
                {
                    //Implementación firma digital NASE-1285
                    Parameter paramDigitalFirm = new Parameter();
                    paramDigitalFirm = DelegateService.commonService.GetParameterByDescription("Enabled_DigitalFirm");
                    JTFORM.PendingPrint pending = new JTFORM.PendingPrint();

                    if (paramDigitalFirm.NumberParameter == 1)
                    {
                        var printStatus = DelegateService.printingServiceCore.GetLogPrintStatus(policyInfo.EndorsementId);
                        var folder = string.Empty;
                        if (printStatus != null)
                        {
                            if (printStatus.StatusId == (int)EnumStatusPrint.Firmado)//validar si ya fue impresa
                            {
                                //recupera la url firmados y muestra pdf
                                folder = ValidateFolderSigned(policyInfo.PolicyId, policyInfo.EndorsementId);

                                if (folder == string.Empty)
                                {
                                    folder = ValidateFolderTemporalesSISE(policyInfo.PolicyId, policyInfo.EndorsementId);
                                    if (folder == string.Empty)
                                    {
                                        string validateError = ReadErrorLog(policyInfo.PolicyId, policyInfo.EndorsementId);
                                        if (validateError != string.Empty)
                                        {
                                            InsertLogError(policyInfo, validateError);
                                            UpdateLogPrint(printStatus.Id, policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                            throw new Exception(validateError);
                                        }
                                        else
                                        {
                                            InsertLogError(policyInfo, Resources.Errors.ErrorFileNotFoundInFolderSigned);
                                            UpdateLogPrint(printStatus.Id, policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                            throw new Exception(Resources.Errors.ErrorFileNotFoundInFolderSigned);
                                        }
                                    }
                                    
                                }
                                pending.FilePath = folder;
                            }
                            else if (printStatus.StatusId == (int)EnumStatusPrint.NoFirmado) //No impresa
                            {
                                folder = ValidateFolderSigned(policyInfo.PolicyId, policyInfo.EndorsementId);
                                if (folder != string.Empty)
                                {
                                    pending.FilePath = folder;
                                    UpdateLogPrint(printStatus.Id,policyInfo, pending.FilePath, (int)EnumStatusPrint.Firmado);
                                }
                                else
                                {
                                    folder = ValidateFolderTemporalesSISE(policyInfo.PolicyId, policyInfo.EndorsementId);
                                    if (folder != string.Empty)
                                    {
                                        pending.FilePath = folder;
                                    }
                                    else
                                    {
                                        if (policyInfo.NumberReport == null)
                                        {
                                            policyInfo.NumberReport = processId;
                                        }

                                        ReportPr report = new ReportPr(0, (int)policyInfo.NumberReport, 0, 0, policyInfo.PolicyId, policyInfo.EndorsementId, 0, (int)ReportType.COMPLETE_POLICY, string.Empty, policyInfo.CommonProperties.UserName, 0, policyInfo.CommonProperties.PrefixId, policyInfo.CommonProperties.RiskSince, policyInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)policyInfo.CommonProperties.IsMassive, (bool)policyInfo.IsCollective, policyInfo.CurrentFromFirst, policyInfo.EndorsementText);
                                        DataSet dsOutput = new DataSet();
                                        StringReader sr = new StringReader(Sistran.Company.PrintingService.NASE.Clases.PrinterHelperNASE.getPrintedFileInfo(report));
                                        dsOutput.ReadXml(sr);
                                        DataSet dsReportResponse = dsOutput;
                                        //pasar de temp a originales
                                        MoveFileTempToOriginal(policyInfo.PolicyId, policyInfo.EndorsementId);
                                        folder = ValidateFolderSigned(policyInfo.PolicyId, policyInfo.EndorsementId);
                                        if (folder == string.Empty)
                                        {
                                            //si genera error registro 2 tablas de log con url firmados
                                            string validateError = ReadErrorLog(policyInfo.PolicyId, policyInfo.EndorsementId);
                                            if (validateError != string.Empty)
                                            {
                                                InsertLogError(policyInfo, validateError);
                                                UpdateLogPrint(printStatus.Id, policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                                throw new Exception(validateError);
                                            }
                                            else
                                            {
                                                InsertLogError(policyInfo, Resources.Errors.ErrorFileNotFoundInFolderSigned);
                                                UpdateLogPrint(printStatus.Id, policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                                InsertLogPrint(policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                                throw new Exception(Resources.Errors.ErrorFileNotFoundInFolderSigned);
                                            }
                                        }
                                        pending.FilePath = folder;
                                        //genera registro en la printStatus y genera impresión
                                        UpdateLogPrint(printStatus.Id, policyInfo, pending.FilePath, (int)EnumStatusPrint.Firmado);
                                    }
                                    
                                }
                            }
                            else if (printStatus.StatusId == (int)EnumStatusPrint.Modificado) //modificar data
                            {
                                //reportNotSigned
                                if (policyInfo.NumberReport == null)
                                {
                                    policyInfo.NumberReport = processId;
                                }

                                ReportPr report = new ReportPr(0, (int)policyInfo.NumberReport, 0, 0, policyInfo.PolicyId, policyInfo.EndorsementId, 0, (int)ReportType.COMPLETE_POLICY, string.Empty, policyInfo.CommonProperties.UserName, 0, policyInfo.CommonProperties.PrefixId, policyInfo.CommonProperties.RiskSince, policyInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)policyInfo.CommonProperties.IsMassive, (bool)policyInfo.IsCollective, policyInfo.CurrentFromFirst, policyInfo.EndorsementText);
                                DataSet dsOutput = new DataSet();
                                StringReader sr = new StringReader(Sistran.Company.PrintingService.NASE.Clases.PrinterHelperNASE.getPrintedFileInfo(report));
                                dsOutput.ReadXml(sr);
                                DataSet dsReportResponse = dsOutput;
                                pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                                pending.FileName = Path.GetFileName(pending.FilePath);
                                //Genero impresion sin firma
                                //Guardo pdf en No firmados
                                UpdateLogPrint(printStatus.Id, policyInfo, pending.FilePath, (int)EnumStatusPrint.Modificado);
                                
                            }

                        }
                        else // generar proceso de firma difital
                        {
                            if (policyInfo.NumberReport == null)
                            {
                                policyInfo.NumberReport = processId;
                            }

                            ReportPr report = new ReportPr(0, (int)policyInfo.NumberReport, 0, 0, policyInfo.PolicyId, policyInfo.EndorsementId, 0, (int)ReportType.COMPLETE_POLICY, string.Empty, policyInfo.CommonProperties.UserName, 0, policyInfo.CommonProperties.PrefixId, policyInfo.CommonProperties.RiskSince, policyInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)policyInfo.CommonProperties.IsMassive, (bool)policyInfo.IsCollective, policyInfo.CurrentFromFirst, policyInfo.EndorsementText);
                            DataSet dsOutput = new DataSet();
                            StringReader sr = new StringReader(Sistran.Company.PrintingService.NASE.Clases.PrinterHelperNASE.getPrintedFileInfo(report));
                            dsOutput.ReadXml(sr);
                            DataSet dsReportResponse = dsOutput;
                            //pasar de temp a originales
                            MoveFileTempToOriginal(policyInfo.PolicyId, policyInfo.EndorsementId);
                            folder = ValidateFolderSigned(policyInfo.PolicyId, policyInfo.EndorsementId);
                            if (folder == string.Empty)
                            {
                                //si genera error registro 2 tablas de log con url firmados
                                string validateError = ReadErrorLog(policyInfo.PolicyId, policyInfo.EndorsementId);
                                if (validateError != string.Empty)
                                {
                                    InsertLogError(policyInfo, validateError);
                                    InsertLogPrint(policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                    throw new Exception(Resources.Errors.ErrorWorkerFirma);
                                }
                                else
                                {
                                    InsertLogError(policyInfo, Resources.Errors.ErrorFileNotFoundInFolderSigned);
                                    InsertLogPrint(policyInfo, pending.FilePath, (int)EnumStatusPrint.NoFirmado);
                                    throw new Exception(Resources.Errors.ErrorFileNotFoundInFolderSigned);
                                }
                            }
                            pending.FilePath = folder;
                            //genera registro en la printStatus y genera impresión
                            InsertLogPrint(policyInfo, pending.FilePath, (int)EnumStatusPrint.Firmado);
                        }
                    }
                    else
                    {
                        //Genera proceso sin firma
                        if (policyInfo.NumberReport == null)
                        {
                            policyInfo.NumberReport = processId;
                        }

                        ReportPr report = new ReportPr(0, (int)policyInfo.NumberReport, 0, 0, policyInfo.PolicyId, policyInfo.EndorsementId, 0, (int)ReportType.COMPLETE_POLICY, string.Empty, policyInfo.CommonProperties.UserName, 0, policyInfo.CommonProperties.PrefixId, policyInfo.CommonProperties.RiskSince, policyInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)policyInfo.CommonProperties.IsMassive, (bool)policyInfo.IsCollective, policyInfo.CurrentFromFirst, policyInfo.EndorsementText);
                        DataSet dsOutput = new DataSet();
                        StringReader sr = new StringReader(Sistran.Company.PrintingService.NASE.Clases.PrinterHelperNASE.getPrintedFileInfo(report));
                        dsOutput.ReadXml(sr);
                        DataSet dsReportResponse = dsOutput;
                        pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                        pending.FileName = Path.GetFileName(pending.FilePath);

                    }

                    return pending.FilePath;

                }
                else if (printingInfo is TemporaryInfo temporaryInfo)
                {
                    ReportPr report = new ReportPr(0, processId, temporaryInfo.OperationId, 0, 0, 0, 0, (int)ReportType.TEMPORARY, string.Empty, temporaryInfo.CommonProperties.UserName, temporaryInfo.TempId, temporaryInfo.CommonProperties.PrefixId, temporaryInfo.CommonProperties.RiskSince, temporaryInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, (bool)temporaryInfo.CommonProperties.IsMassive, (bool)temporaryInfo.IsCollective, false, false, temporaryInfo.TempAuthorization);
                    DataSet dsOutput = new DataSet();
                    StringReader sr = new StringReader(Sistran.Company.PrintingService.NASE.Clases.PrinterHelperNASE.getPrintedFileInfo(report));
                    dsOutput.ReadXml(sr);
                    DataSet dsReportResponse = dsOutput;
                    JTFORM.PendingPrint pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                    pending.FileName = Path.GetFileName(pending.FilePath);

                    return pending.FilePath;
                }
                else if (printingInfo is QuotationInfo quotationInfo)
                {

                    ReportPr report = new ReportPr(0, processId, 0, 0, 0, 0, quotationInfo.QuotationId, (int)ReportType.QUOTATION, string.Empty, quotationInfo.CommonProperties.UserName, quotationInfo.TempId, quotationInfo.CommonProperties.PrefixId, quotationInfo.CommonProperties.RiskSince, quotationInfo.CommonProperties.RiskUntil, string.Empty, string.Empty, 0, 0, false, false, false);
                    DataSet dsOutput = new DataSet();
                    StringReader sr = new StringReader(Sistran.Company.PrintingService.NASE.Clases.PrinterHelperNASE.getPrintedFileInfo(report));
                    dsOutput.ReadXml(sr);
                    DataSet dsReportResponse = dsOutput;
                    JTFORM.PendingPrint pending = new JTFORM.PendingPrint(dsReportResponse.Tables["PendingPrintData"].Rows[0]);
                    pending.FileName = Path.GetFileName(pending.FilePath);
                    return pending.FilePath;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }

        public string ValidateFolderSigned(int policyId, int endorsementId)
        {
            using (NetworkConnection networkCon = new NetworkConnection(reportSigned, new NetworkCredential(UserServiceConsumer, PasswordServiceConsumer, DomainOfUser)))
            {
                var route = String.Empty;

                for (int i = 0; i < repeatInterval; i++)
                {
                    string[] fileSigned = Directory.GetFiles(reportSigned);
                    for (int j = 0; j < fileSigned.Length; j++)
                    {
                        bool exist = false;
                        exist = fileSigned[j].Contains(policyId.ToString() + "_" + endorsementId.ToString());

                        if (exist)
                        {
                            char delimit = 'P';
                            string[] fileName = fileSigned[j].Split(delimit);
                            File.Move(fileSigned[j], reportSISE +"P"+ fileName[1]);
                            route = reportSISE + "P" + fileName[1];
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(intervalSeconds);
                }

                return route;
            }
        }


        public string ValidateFolderTemporalesSISE(int policyId, int endorsementId)
        {
            using (NetworkConnection networkCon = new NetworkConnection(reportSISE, new NetworkCredential(UserServiceConsumer, PasswordServiceConsumer, DomainOfUser)))
            {
                var routeTemp = String.Empty;
                string[] fileTemp = Directory.GetFiles(reportSISE);
                foreach (var temp in fileTemp)
                {
                    bool exist = false;
                    exist = temp.Contains(policyId.ToString() + "_" + endorsementId.ToString());

                    if (exist)
                    {
                        routeTemp = temp;
                        break;
                    }
                }
                
                return routeTemp;
            }
        }


        public string MoveFileTempToOriginal(int policyId, int endorsementId)
        {
            using (NetworkConnection networkCon = new NetworkConnection(reportDigital, new NetworkCredential(UserServiceConsumer, PasswordServiceConsumer, DomainOfUser)))
            {
                var routeTemp = String.Empty;
                string[] fileTemp = Directory.GetFiles(reportSISE);
                for (int i = 0; i < fileTemp.Length; i++)
                {
                    bool exist = false;
                    exist = fileTemp[i].Contains(policyId.ToString() + "_" + endorsementId.ToString());

                    if (exist)
                    {
                        char delimit = 'P';
                        string[] fileName = fileTemp[i].Split(delimit);
                        File.Move(fileTemp[i], reportDigital + "P" + fileName[1]);
                        routeTemp = reportDigital + "P" + fileName[1];
                        break;
                    }
                }

                return routeTemp;
            }
        }



        public string ReadErrorLog(int policyId, int endorsementId)
        {
            
            using (NetworkConnection networkCon = new NetworkConnection(reportSigned, new NetworkCredential(UserServiceConsumer, PasswordServiceConsumer, DomainOfUser)))
            {
                string[] messageTxt;
                string[] text;
                string result = string.Empty;
                string[] errorTxt = Directory.GetFiles((reportSigned + "/WhitError"), "*.txt");
                for (int j = 0; j < errorTxt.Length; j++)
                {
                    bool exist = false;
                    exist = errorTxt[j].Contains(policyId.ToString() + "_" + endorsementId.ToString());

                    if (exist)
                    {
                        messageTxt = File.ReadAllLines(errorTxt[j]);
                        text = messageTxt[2].Split('"');
                        result = text[3];
                        break;
                    }
                }

                return result;// messageTxt;
            }
        }

        public void InsertLogPrint(PolicyInfo policy, string url, int status)
        {
            LogPrintStatusDTO logStatus = new LogPrintStatusDTO
            {
                PolicyId = policy.PolicyId,
                EndorsementId = policy.EndorsementId,
                Observacion = "",
                StatusId = status, 
                Date = DateTime.Now,
                UserName = policy.CommonProperties.UserName, //validar usuario
                Url = url
            };

            DelegateService.printingServiceCore.RegisterLogPrintStatus(logStatus);
        }

        public void UpdateLogPrint(int id, PolicyInfo policy, string url, int status)
        {
            LogPrintStatusDTO logStatus = new LogPrintStatusDTO
            {
                Id = id,
                PolicyId = policy.PolicyId,
                EndorsementId = policy.EndorsementId,
                Observacion = "",
                StatusId = status, 
                Date = DateTime.Now,
                UserName = policy.CommonProperties.UserName, 
                Url = url
            };

            DelegateService.printingServiceCore.UpdateLogPrintStatus(logStatus);
        }

        public void InsertLogError(PolicyInfo policy, string error)
        {
            LogErrorPrintDTO logError = new LogErrorPrintDTO
            {
                Description = error,
                DateError = DateTime.Now,
                EndorsementId = policy.EndorsementId,
                PolicyId = policy.PolicyId
            };
            DelegateService.printingServiceCore.RegisterLogErrorPrint(logError);
        }



        public string PrintCounterGuarantee(int guaranteeID, int individualID)
        {
            try
            {
                int processId = GetPrinterProcessId("PARAM_REPORT");

                Sistran.Company.PrintingService.NASE.Clases.Report report = new PrintingService.NASE.Clases.Report();
                return report.PrintCounterGuarantees(processId, guaranteeID, individualID);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message), ex);
            }
        }
    }
}