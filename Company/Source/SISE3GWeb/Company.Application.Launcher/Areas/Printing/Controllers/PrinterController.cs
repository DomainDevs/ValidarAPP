using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Generic;
using Sistran.Core.Framework.UIF.Web.Areas.Printing.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF.Web.Helpers;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Sistran.Company.Application.CollectionFormBusinessService.Models;
using Sistran.Core.Framework.UIF.Web.Controllers;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Net;
using System.ComponentModel;
using Sistran.Core.Application.UnderwritingServices.Enums;
using static Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Enums.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.ExternalProxyServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Printing.Controllers
{
    using UNSE = Sistran.Company.Application.UnderwritingServices.Enums;
    [Authorize]
    public class PrinterController : Controller
    {
        [NoDirectAccess]
        public ActionResult Printer(PrinterModelsView model)
        {

            return View("Printer", model);
        }

        [NoDirectAccess]
        public ActionResult CollectFormat(PrinterModelsView model)
        {

            return View("CollectFormat", model);
        }

        [NoDirectAccess]
        public ActionResult CounterGuarantee(PrinterModelsView model)
        {

            return View("CounterGuarantee", model);
        }

        public ActionResult MassivePrinter(PrinterModelsView model)
        {

            return View("MassivePrinter", model);
        }

        public ActionResult GetPrefixes()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
            return new UifSelectResult(prefixes.OrderBy(x => x.Description));
        }

        public ActionResult GetBranches()
        {
            List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
            return new UifSelectResult(branches.OrderBy(x => x.Description));
        }

        public ActionResult GetEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<UNMO.Endorsement> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);

                    if (endorsements != null)
                    {
                        endorsements.ForEach(m => m.Description = m.Description + " - " + App_GlobalResources.Language.ResourceManager.GetString(m.EndorsementType.ToString()));
                        return new UifJsonResult(true, endorsements);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyNoExist);
                    }
                }
                else
                {
                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicy);
            }
        }

        public ActionResult GetSummaryByEndorsementId(int endorsementId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCiaCurrentStatusPolicyByEndorsementIdIsCurrentCompany(endorsementId, false, true);

                if (policy != null)
                {
                    return new UifJsonResult(true, policy.Summary);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoInformationEndorsement);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInformationEndorsement);
            }
        }

        public ActionResult GetSummaryByTemporalId(int temporalId, bool temporalAutho)
        {
            try
            {
                TemporaryInfo temporaryInfo = new TemporaryInfo();

                temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(temporalId, temporalAutho);
                if (temporaryInfo != null)
                {
                    temporaryInfo.OperationId = temporalId;
                    return new UifJsonResult(true, temporaryInfo);
                }
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.NoFoundInformationTemporary);

            }
            catch (Exception ex)
            {
                string message = string.Empty;
                switch (ex.InnerException.Message)
                {
                    case "Temporary Quotation":
                        message = App_GlobalResources.Language.IsTemporaryQuotation;
                        break;
                    case "Temporary Authorization":
                        message = App_GlobalResources.Language.TempNotAuthorization;
                        break;
                    default:
                        message = App_GlobalResources.Language.ErrorQueryInformationTemporary;
                        break;

                }
                return new UifJsonResult(false, message);
            }
        }

        public ActionResult GetQuotationByTemporalId(int quotationlId, int branchId, int prefixId)
        {
            try
            {
                List<QuotationInfo> quotationInfos = DelegateService.printingService.GetQuotationByQuotationID(quotationlId, branchId, prefixId);
                if (quotationInfos.Count > 0)
                    return new UifJsonResult(true, quotationInfos);
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.QuotationNotFound);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.QuotationNotFound);
            }
        }

        public ActionResult GetPrintingProcessByPrintingId(int printingId)
        {
            try
            {
                string strPathError = string.Empty;
                CompanyPrinting companyPrinting = DelegateService.printingService.GetPrintingProcess(printingId);

                if (companyPrinting.PrintingTypeId == 2 || companyPrinting.PrintingTypeId == 3)
                {
                    string strPath = companyPrinting.UrlFile;
                    var filenamefromPath = companyPrinting.UrlFile.Split(new char[] { '\\' }).Last();

                    if (strPath.Contains(";") && string.IsNullOrEmpty(companyPrinting.UrlFileError.Trim()))
                    {
                        string[] sPathPoliza = companyPrinting.UrlFile.Split(';');
                        string strPathWithCollectFormat = sPathPoliza[0].ToString().Replace(".pdf", "_FR.pdf");

                        if (System.IO.File.Exists(sPathPoliza[0].ToString()))
                        {
                            bool bJoinPdf = DelegateService.printingService.MergeFiles(strPathWithCollectFormat, sPathPoliza);
                            if (bJoinPdf)
                            {
                                var filenamefromPathWitCollectFormat = strPathWithCollectFormat.Split(new char[] { '\\' }).Last();
                                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPathWithCollectFormat, Filename = filenamefromPathWitCollectFormat, FilePathResult = strPathWithCollectFormat });
                            }
                            else
                            {
                                return new UifJsonResult(false, companyPrinting);
                            }
                        }
                        else
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.PdfNotFound);
                        }
                    }
                    else if (strPath.Contains("_FR"))
                    {
                        filenamefromPath = strPath.Split(new char[] { '\\' }).Last();
                        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                    }
                    else
                    {
                        if (!System.IO.File.Exists(strPath) && string.IsNullOrEmpty(companyPrinting.UrlFileError.Trim()))
                        {
                            strPathError = ConfigurationManager.AppSettings["PathError"].ToString() + filenamefromPath.Replace(".pdf", ".err");

                            if (System.IO.File.Exists(strPathError))
                            {
                                companyPrinting.FinishDate = DateTime.Now;
                                companyPrinting.HasError = true;
                                companyPrinting.UrlFileError = strPathError;
                                DelegateService.printingService.UpdatePrintingProcess(companyPrinting);
                                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPdfJira);
                            }
                            else
                            {
                                return new UifJsonResult(false, App_GlobalResources.Language.PdfNotFound);
                            }
                        }
                        else if (companyPrinting.HasError)
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorPdfJira);
                        }
                        else
                        {
                            companyPrinting.FinishDate = DateTime.Now;
                            DelegateService.printingService.UpdatePrintingProcess(companyPrinting);
                            return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                        }
                    }
                }
                else if (companyPrinting.PrintingTypeId == 1)
                {
                    List<CompanyPrintingLog> companyPrintingLogs = DelegateService.printingService.ValidatePrintingByPrintingId(printingId);
                    int iTotalRisks = companyPrinting.Total;
                    int iPendingRisks = (companyPrinting.Total - companyPrintingLogs.Where(x => x.StatusCd == 2).Count());
                    int iErrorRisks = companyPrintingLogs.Where(x => x.StatusCd == 3).Count();

                    if (iTotalRisks == companyPrintingLogs.Where(x => x.StatusCd == 2).ToList().Count)
                    {
                        companyPrinting.FinishDate = DateTime.Now;
                        companyPrinting.HasError = false;
                        companyPrinting.UrlFileError = string.Empty;
                        DelegateService.printingService.UpdatePrintingProcess(companyPrinting);
                    }

                    return new UifJsonResult(true, new { TotalRisks = iTotalRisks, PendingRisks = iPendingRisks, ErrorRisks = iErrorRisks, TypePrinting = companyPrinting.PrintingTypeId, CompanyPrintingLogs = companyPrintingLogs });
                }
                else
                {
                    return new UifJsonResult(false, companyPrinting);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPrintingProcess);
            }
        }

        public ActionResult UpdatePrintingProcess(int printingId, string pathCollect)
        {
            try
            {
                CompanyPrinting companyPrinting = DelegateService.printingService.GetPrintingProcess(printingId);
                companyPrinting.UrlFile = companyPrinting.UrlFile + ";" + pathCollect;
                companyPrinting.FinishDate = DateTime.Now;
                companyPrinting = DelegateService.printingService.UpdatePrintingProcess(companyPrinting);
                string[] sPathPoliza = companyPrinting.UrlFile.Split(';');
                string strPathWithCollectFormat = sPathPoliza[0].ToString().Replace(".pdf", "_FR.pdf");
                bool bJoinPdf = DelegateService.printingService.MergeFiles(strPathWithCollectFormat, sPathPoliza);
                if (bJoinPdf)
                {
                    companyPrinting.UrlFile = strPathWithCollectFormat;
                    companyPrinting = DelegateService.printingService.UpdatePrintingProcess(companyPrinting);
                    return new UifJsonResult(true, companyPrinting.Id);
                }
                else
                {
                    return new UifJsonResult(true, companyPrinting.Id);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GenerateZip(int printingId)
        {
            try
            {
                List<CompanyPrintingLog> companyPrintingLogs = DelegateService.printingService.ValidatePrintingByPrintingId(printingId);

                if (companyPrintingLogs.Where(x => x.StatusCd == 2).ToList().Count > 0)
                {
                    string sPathZip = DelegateService.printingService.GenerateZipFile(companyPrintingLogs);
                    var filenamefromPath = sPathZip.Split(new char[] { '\\' }).Last();
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + sPathZip, Filename = filenamefromPath, FilePathResult = sPathZip });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PdfInGeneration);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.PdfInGeneration);
            }
        }

        public ActionResult GenerateReport(CompanyFilterReport companyFilterReport)
        {
            try
            {
                companyFilterReport.User = new User
                {
                    UserId = SessionHelper.GetUserId(),
                    AccountName = SessionHelper.GetUserName()
                };

                return new UifJsonResult(true, DelegateService.printingService.GenerateReport(companyFilterReport));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowPdfFile(string url)
        {
            string UserServiceConsumer = ConfigurationManager.AppSettings["UserServiceConsumer"];
            string PasswordServiceConsumer = ConfigurationManager.AppSettings["PasswordServiceConsumer"];
            string DomainOfUser = ConfigurationManager.AppSettings["DomainOfUser"];
            string reportSISE = ConfigurationManager.AppSettings["ReportExportPath"].ToString();
            
            using (NetworkConnection networkCon = new NetworkConnection(reportSISE, new NetworkCredential(UserServiceConsumer, PasswordServiceConsumer, DomainOfUser)))
            {
                try
                {
                var pathToTheFile = url;
                var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
                var extention = url.Split(new char[] { '.' }).Last();
                if (extention == "zip")
                    return new FileStreamResult(fileStream, "application/zip");
                else
                    return new FileStreamResult(fileStream, "application/pdf");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public ActionResult MergeFiles(string FilePolicy, string FileCollectFormat)
        {
            try
            {
                string strPathWithCollectFormat = string.Empty;

                if (System.IO.File.Exists(FilePolicy))
                {
                    strPathWithCollectFormat = FilePolicy.Replace(".pdf", "_FR.pdf");
                    string[] sPathPoliza = new string[2];
                    sPathPoliza[0] = FilePolicy;
                    sPathPoliza[1] = FileCollectFormat;
                    bool bJoinPdf = DelegateService.printingService.MergeFiles(strPathWithCollectFormat, sPathPoliza);
                    if (bJoinPdf)
                    {
                        var filenamefromPath = strPathWithCollectFormat.Split(new char[] { '\\' }).Last();
                        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPathWithCollectFormat, Filename = filenamefromPath, FilePathResult = strPathWithCollectFormat });
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorJoiningPdf);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorJoiningPdf);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        /// <summary>
        /// Metodo para impresion de poliza
        /// </summary>
        /// <param name="companyFilterReport"></param>
        /// <returns></returns>
        public ActionResult GenerateReportPolicy(int policyId, int endorsementId, int prefixId, bool endorsementText, bool currentFromFirst)
        {
            try
            {
                var validateReinsured = false;
                List<ReinsuranceDistributionHeaderDTO> reinsuranceDistributionHeaders = new List<ReinsuranceDistributionHeaderDTO>();
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsementId);

                EndorsementCompanyDTO endorsement = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(companyPolicy.Branch.Id, prefixId, companyPolicy.DocumentNumber).Where(x => x.Id == endorsementId).FirstOrDefault();
                validateReinsured = endorsement.AssuredSum != 0 || endorsement.TotalPremium != 0;
                Parameter parameter = new Parameter();
                parameter = DelegateService.commonService.GetParameterByParameterId(12191);
                if (validateReinsured)
                {
                    //Valida el parametro
                    if (parameter.NumberParameter == 0)
                    {
                        //Valida 2g
                        ResponseReinsurance responseReinsurance = new ResponseReinsurance();
                        RequestReinsurance requestReinsurance = new RequestReinsurance();
                        requestReinsurance.DocumentNumber = Convert.ToInt32(companyPolicy.DocumentNumber);
                        requestReinsurance.EndorsementNumber = endorsement.EndorsementNumber;
                        requestReinsurance.Prefix = companyPolicy.Prefix.Id;
                        requestReinsurance.Branch = companyPolicy.Branch.Id;
                        responseReinsurance = DelegateService.ExternalServiceWeb.GetReinsurancePolicy(requestReinsurance);
                        if (responseReinsurance.PolicyStatus == 1)
                        {
                            companyPolicy.IsReinsured = true;
                        }
                        else
                        {
                            companyPolicy.IsReinsured = false;
                        }
                    }
                    else if (parameter.NumberParameter == 1)
                    {
                        //Valida 3g
                        reinsuranceDistributionHeaders = DelegateService.reinsuranceService.GetReinsuranceDistributionHeaders(endorsementId);
                    }
                }
                if (reinsuranceDistributionHeaders.Count > 0 || !validateReinsured || companyPolicy.IsReinsured || parameter.NumberParameter == 2)
                {
                    int iIntestosImpresion = int.Parse(ConfigurationManager.AppSettings["IntentosImpresion"].ToString());
                    //PolicyInfo policyInfo = DelegateService.printingService.GetRisksByEndorsementId(endorsementId);
                    bool bError = false;
                    PolicyInfo policyInfo = new PolicyInfo { PolicyId = policyId, EndorsementId = endorsementId, CommonProperties = new CommonProperties { RiskSince = 1, RiskUntil = 1, PrefixId = prefixId, UserName = SessionHelper.GetUserName(), IsMassive = (companyPolicy.PolicyOrigin != PolicyOrigin.Individual) ? true : false }, TicketNumber = companyPolicy.Endorsement.TicketNumber, IsCollective = false, EndorsementText = endorsementText, CurrentFromFirst = currentFromFirst };
                    string strPath = DelegateService.printingService.GenerateReportNase(policyInfo);
                    int IsExternal = int.Parse(ConfigurationManager.AppSettings["IsExternal"].ToString());
                    var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();

                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });

                    //if (IsExternal == 1)
                    //{
                    //    string user = DelegateService.commonService.GetKeyApplication("UserDomain");
                    //    string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
                    //    string path = DelegateService.commonService.GetKeyApplication("UserRemotePath");

                    //    using (NetworkConnection networkCon = new NetworkConnection(path, new NetworkCredential(user, password)))
                    //    {
                    //        if (networkCon._resultConnection == 0)
                    //        {
                    //            for (int i = 0; i < iIntestosImpresion; i++)
                    //            {
                    //                if (!System.IO.File.Exists(strPath))
                    //                {
                    //                    System.Threading.Thread.Sleep(1000);
                    //                    bError = true;
                    //                }
                    //                else
                    //                {
                    //                    bError = false;
                    //                    break;
                    //                }
                    //            }

                    //            if (!bError)
                    //                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                    //            else
                    //            {
                    //                CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), policyInfo.CommonProperties.RiskUntil,0);
                    //                return new UifJsonResult(true, companyPrinting.Id);
                    //            }
                    //        }
                    //        else
                    //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorFindingFolderPrinting);
                    //    }
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < iIntestosImpresion; i++)
                    //    {
                    //        if (!System.IO.File.Exists(strPath))
                    //        {
                    //            System.Threading.Thread.Sleep(1000);
                    //            bError = true;
                    //        }
                    //        else
                    //        {
                    //            bError = false;
                    //            break;
                    //        }
                    //    }

                    //    if (!bError)
                    //        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                    //    else
                    //    {
                    //        CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), policyInfo.CommonProperties.RiskUntil,0);
                    //        return new UifJsonResult(true, companyPrinting.Id);
                    //    }
                    //}

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.EndorsmentNotReinsured);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        /// <summary>
        /// Metodo para impresion de poliza
        /// </summary>
        /// <param name="companyFilterReport"></param>
        /// <returns></returns>
        public ActionResult GenerateReportPolicyToEndoso(int policyId, int endorsementId, int prefixId)
        {
            try
            {
                var validateReinsured = false;
                List<ReinsuranceDistributionHeaderDTO> reinsuranceDistributionHeaders = new List<ReinsuranceDistributionHeaderDTO>();
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsementId);

                EndorsementCompanyDTO endorsement = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(companyPolicy.Branch.Id, prefixId, companyPolicy.DocumentNumber).Where(x => x.Id == endorsementId).FirstOrDefault();
                validateReinsured = endorsement.AssuredSum != 0 && endorsement.TotalPremium != 0;
                Parameter parameter = new Parameter();
                parameter = DelegateService.commonService.GetParameterByParameterId(12191);

                if (validateReinsured)
                {
                    //Valida el parametro
                    if (parameter.NumberParameter == 0)
                    {
                        //Valida 2g
                        ResponseReinsurance responseReinsurance = new ResponseReinsurance();
                        RequestReinsurance requestReinsurance = new RequestReinsurance();
                        requestReinsurance.DocumentNumber = Convert.ToInt32(companyPolicy.DocumentNumber);
                        requestReinsurance.EndorsementNumber = endorsement.EndorsementNumber;
                        requestReinsurance.Prefix = companyPolicy.Prefix.Id;
                        requestReinsurance.Branch = companyPolicy.Branch.Id;
                        responseReinsurance = DelegateService.ExternalServiceWeb.GetReinsurancePolicy(requestReinsurance);
                        if (responseReinsurance.PolicyStatus == 1)
                        {
                            companyPolicy.IsReinsured = true;
                        }
                        else
                        {
                            companyPolicy.IsReinsured = false;
                        }
                    }
                    else if (parameter.NumberParameter == 1)
                    {
                        //Valida 3g
                        reinsuranceDistributionHeaders = DelegateService.reinsuranceService.GetReinsuranceDistributionHeaders(endorsementId);
                    }
                }
                if (reinsuranceDistributionHeaders.Count > 0 || !validateReinsured || companyPolicy.IsReinsured || parameter.NumberParameter == 2)
                {
                    int iIntestosImpresion = int.Parse(ConfigurationManager.AppSettings["IntentosImpresion"].ToString());
                    PolicyInfo policyInfo = DelegateService.printingService.GetRisksByEndorsementId(endorsementId);
                    bool bError = false;
                    string strPath = DelegateService.printingService.GenerateReportNase(new PolicyInfo { PolicyId = policyId, EndorsementId = endorsementId, CommonProperties = new CommonProperties { RiskSince = policyInfo.CommonProperties.RiskSince, RiskUntil = policyInfo.CommonProperties.RiskUntil, PrefixId = prefixId, UserName = SessionHelper.GetUserName(), UserId = SessionHelper.GetUserId(), IsMassive = (companyPolicy.PolicyOrigin != PolicyOrigin.Individual) ? true : false }, TicketNumber = companyPolicy.Endorsement.TicketNumber, IsCollective = false });
                    int IsExternal = int.Parse(ConfigurationManager.AppSettings["IsExternal"].ToString());
                    var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();

                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.EndorsmentNotReinsured);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        /// <summary>
        /// Metodo para impresion de Temporario
        /// </summary>
        /// <param name="companyFilterReport"></param>
        /// <returns></returns>
        public ActionResult GenerateReportTemporary(int temporaryId, int prefixId, int riskSince, int riskUntil, int operationId, bool tempAuthorization)
        {
            try
            {
                int iIntestosImpresion = int.Parse(ConfigurationManager.AppSettings["IntentosImpresion"].ToString());
                bool bError = false;
                string strPath = DelegateService.printingService.GenerateReportNase(new TemporaryInfo { TempId = temporaryId, OperationId = operationId, CommonProperties = new CommonProperties { IsMassive = false, RiskSince = riskSince, RiskUntil = riskUntil, PrefixId = prefixId, UserName = SessionHelper.GetUserName() }, IsCollective = false, TempAuthorization = tempAuthorization });
                int IsExternal = int.Parse(ConfigurationManager.AppSettings["IsExternal"].ToString());
                var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();

                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });

                //if (IsExternal == 1)
                //{
                //    string user = DelegateService.commonService.GetKeyApplication("UserDomain");
                //    string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
                //    string path = DelegateService.commonService.GetKeyApplication("UserRemotePath");

                //    using (NetworkConnection networkCon = new NetworkConnection(path, new NetworkCredential(user, password)))
                //    {
                //        if (networkCon._resultConnection == 0)
                //        {
                //            for (int i = 0; i < iIntestosImpresion; i++)
                //            {
                //                if (!System.IO.File.Exists(strPath))
                //                {
                //                    System.Threading.Thread.Sleep(1000);
                //                    bError = true;
                //                }
                //                else
                //                {
                //                    bError = false;
                //                    break;
                //                }
                //            }

                //            if (!bError)
                //                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                //            else
                //            {
                //                CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), riskUntil,0);
                //                return new UifJsonResult(true, companyPrinting.Id);
                //            }
                //        }
                //        else
                //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorFindingFolderPrinting);
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < iIntestosImpresion; i++)
                //    {
                //        if (!System.IO.File.Exists(strPath))
                //        {
                //            System.Threading.Thread.Sleep(1000);
                //            bError = true;
                //        }
                //        else
                //        {
                //            bError = false;
                //            break;
                //        }
                //    }

                //    if (!bError)
                //        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                //    else
                //    {
                //        CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), riskUntil,0);
                //        return new UifJsonResult(true, companyPrinting.Id);
                //    }
                //}
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        /// <summary>
        /// Metodo para impresion de Temporario
        /// </summary>
        /// <param name="companyFilterReport"></param>
        /// <returns></returns>
        public ActionResult GenerateReportQuotation(int temporaryId, int prefixId, int quotationId, int versionId)
        {
            try
            {
                int iIntestosImpresion = int.Parse(ConfigurationManager.AppSettings["IntentosImpresion"].ToString());
                bool bError = false;
                TemporaryInfo temporaryInfo = DelegateService.printingService.GetTemporaryByTempID(temporaryId);
                string strPath = DelegateService.printingService.GenerateReportNase(new QuotationInfo { TempId = temporaryId, QuotationId = quotationId, VersionId = versionId, CommonProperties = new CommonProperties { PrefixId = prefixId, UserName = SessionHelper.GetUserName(), IsMassive = false, RiskSince = temporaryInfo.CommonProperties.RiskSince, RiskUntil = temporaryInfo.CommonProperties.RiskUntil } });
                int IsExternal = int.Parse(ConfigurationManager.AppSettings["IsExternal"].ToString());
                var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();

                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });

                //if (IsExternal == 1)
                //{
                //    string user = DelegateService.commonService.GetKeyApplication("UserDomain");
                //    string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
                //    string path = DelegateService.commonService.GetKeyApplication("UserRemotePath");

                //    using (NetworkConnection networkCon = new NetworkConnection(path, new NetworkCredential(user, password)))
                //    {
                //        if (networkCon._resultConnection == 0)
                //        {
                //            for (int i = 0; i < iIntestosImpresion; i++)
                //            {
                //                if (!System.IO.File.Exists(strPath))
                //                {
                //                    System.Threading.Thread.Sleep(1000);
                //                    bError = true;
                //                }
                //                else
                //                {
                //                    bError = false;
                //                    break;
                //                }
                //            }

                //            if (!bError)
                //                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                //            else
                //            {
                //                CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), 1,0);
                //                return new UifJsonResult(true, companyPrinting.Id);
                //            }
                //        }
                //        else
                //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorFindingFolderPrinting);
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < iIntestosImpresion; i++)
                //    {
                //        if (!System.IO.File.Exists(strPath))
                //        {
                //            System.Threading.Thread.Sleep(1000);
                //            bError = true;
                //        }
                //        else
                //        {
                //            bError = false;
                //            break;
                //        }
                //    }
                //    if (!bError)
                //        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                //    else
                //    {
                //        CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), 1,0);
                //        return new UifJsonResult(true, companyPrinting.Id);
                //    }
                //}

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetDetailByMassiveLoadId(int massiveLoadId)
        {
            try
            {

                MassiveLoad massiveLoad = DelegateService.massiveUnderwritingService.IssuanceMassiveEmission(massiveLoadId);


                if (massiveLoad.Status == MassiveLoadStatus.Issued)
                {

                    return new UifJsonResult(true, massiveLoad);

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.UploadNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoadingUpload);
            }

        }

        public ActionResult GetMassiveLoadById(int massiveLoadId)
        {
            try
            {
                MassiveLoad massiveLoad = DelegateService.massiveUnderwritingService.IssuanceMassiveEmission(massiveLoadId);
                return new UifJsonResult(true, massiveLoad);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoadingUpload);
            }
        }

        public ActionResult GetPolicyForCollectFormat(int branchId, int prefixId, int policyNumber, int endorsementNumber)
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
                                    bError = true;
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
                        if (!bError)
                            listEndorsements.Add(endorsementList);
                    }
                }

                if (listEndorsements != null)
                {
                    if (bError)
                        return new UifJsonResult(false, App_GlobalResources.Language.QuotesNotFound);

                    return new UifJsonResult(true, new { lstQuotes = listEndorsements.Where(x => x.EndorsementNumber == endorsementNumber).Select(y => y.Quotes).FirstOrDefault(), listEndorsement = listEndorsements, validationPolicy = reportPolicy.Textbody });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoadingUpload);
            }
        }

        public ActionResult GenerateReportCollectFormat(List<ReportEndorsement> endorsementList, string[] idCuotes, int branchId, int prefixId, int endorsementId, int policyNumber)
        {
            string[] paths = new String[2];

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
                                if (Convert.ToInt32(idCuotes[i]) == quote.QuoteNumber)
                                {
                                    ReportPayer payer = new ReportPayer();
                                    payer.IndividualId = insured.IndividualId;
                                    payer.Name = quote.PayerName;
                                    ReportPaymentSchedule paymentSchedule = new ReportPaymentSchedule();
                                    List<ReportInstallment> listInstallment = new List<ReportInstallment>();
                                    ReportInstallment installment = new ReportInstallment();
                                    installment.DueDate = Convert.ToDateTime(quote.Date);
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

                        endorsement.Payers = lstTeportPayers;
                        endorsements.Add(endorsement);
                    }
                }
                reportPolicy.Endorsements = endorsements;
                reportPolicy.userId = SessionHelper.GetUserId();
                collectionForm.Policy = reportPolicy;

                paths = DelegateService.collectionFormBusinessService.GenerateCollectionForm(collectionForm);
                string strPath = paths[0];
                if (strPath != "-1")
                {
                    var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();
                    int IsExternal = int.Parse(ConfigurationManager.AppSettings["IsExternal"].ToString());

                    if (IsExternal == 1)
                    {
                        string user = DelegateService.commonService.GetKeyApplication("UserDomain");
                        string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
                        string path = DelegateService.commonService.GetKeyApplication("pathPdf");

                        using (NetworkConnection networkCon = new NetworkConnection(path, new NetworkCredential(user, password)))
                        {
                            if (networkCon._resultConnection == 0)
                                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                            else
                                return new UifJsonResult(false, App_GlobalResources.Language.ErrorFindingFolderPrinting);
                        }
                    }
                    else
                        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingCollectFormat);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoadingUpload);
            }
        }

        public ActionResult GetParameterUniquePolicy()
        {
            try
            {
                Parameter parameter = DelegateService.commonService.GetParameterByParameterId((int)ParameterEnum.UniquePolicy);
                return new UifJsonResult(true, parameter);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }

        }

        #region printer counterGuarantee
        public UifJsonResult GetHoldersByDocumentOrDescription(string description, CustomerType? customerType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                if (holders != null && holders.Any())
                    return new UifJsonResult(true, holders);
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.SecuredNotFound);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.SecuredNotFound);
            }
        }

        public UifJsonResult GetHoldersByIndividualId(string individualId, CustomerType? customerType)
        {
            try
            {

                Tuple<Holder, List<IssuanceCompanyName>> result = DelegateService.underwritingService.GetHolderByIndividualId(individualId, customerType);
                if (result != null)
                    return new UifJsonResult(true, new
                    {
                        holder = result.Item1,
                        details = result.Item2
                    });
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.SecuredNotFound);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult GetCounterGuaranteesByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetCounterGuaranteesByIndividualId(individualId));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult PrintCounterGuarantees(int guaranteeId, int individualId)
        {
            try
            {
                string strPath = DelegateService.printingService.PrintCounterGuarantee(guaranteeId, individualId);
                var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();
                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Printer") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }
        #endregion


        #region Event_Autho_WorkFlo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<ActionResult> SendToEmail(string email, string filePath, string pathCollectFormat)
        {
            try
            {
                //Primero prueba
                EmailCriteria emailCriteria = new EmailCriteria();
                emailCriteria.Addressed = new List<string>();
                emailCriteria.Files = new List<string>();
                emailCriteria.Addressed.Add(email);
                emailCriteria.Message = "Envio Documento Previsora";
                emailCriteria.Subject = "Previsora";

                if (System.IO.File.Exists(filePath))
                    emailCriteria.Files.Add(filePath);

                if (System.IO.File.Exists(pathCollectFormat))
                    emailCriteria.Files.Add(pathCollectFormat);

                var resp = await DelegateService.utilitiesServiceCore.SendEmailAsync(emailCriteria);
                string respond = (resp) ? App_GlobalResources.Language.SentEmail : App_GlobalResources.Language.ErrorSentEmail;
                return new UifJsonResult(true, resp);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSentEmail);
            }
        }


        #endregion

        public ActionResult GetReinsurancePolicy(int policyId, int endorsementId, int prefixId)
        {
            var validateReinsured = false;
            List<ReinsuranceDistributionHeaderDTO> reinsuranceDistributionHeaders = new List<ReinsuranceDistributionHeaderDTO>();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsementId);

            EndorsementCompanyDTO endorsement = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(companyPolicy.Branch.Id, prefixId, companyPolicy.DocumentNumber).Where(x => x.Id == endorsementId).FirstOrDefault();
            validateReinsured = endorsement.AssuredSum != 0 || endorsement.TotalPremium != 0;
            Parameter parameter = new Parameter();
            parameter = DelegateService.commonService.GetParameterByParameterId(12191);
            //Valida el parametro
            if (parameter.NumberParameter == 0)
            {
                //Valida 2g
                ResponseReinsurance responseReinsurance = new ResponseReinsurance();
                RequestReinsurance requestReinsurance = new RequestReinsurance();
                requestReinsurance.DocumentNumber = Convert.ToInt32(companyPolicy.DocumentNumber);
                requestReinsurance.EndorsementNumber = endorsement.EndorsementNumber;
                requestReinsurance.Prefix = companyPolicy.Prefix.Id;
                requestReinsurance.Branch = companyPolicy.Branch.Id;
                responseReinsurance = DelegateService.ExternalServiceWeb.GetReinsurancePolicy(requestReinsurance);
                if (responseReinsurance.PolicyStatus == 1)
                {
                    companyPolicy.IsReinsured = true;
                }
                else
                {
                    companyPolicy.IsReinsured = false;
                }
            }
            else if (parameter.NumberParameter == 1)
            {
                //Valida 3g
                reinsuranceDistributionHeaders = DelegateService.reinsuranceService.GetReinsuranceDistributionHeaders(endorsementId);
                if (reinsuranceDistributionHeaders.Count > 0)
                {
                    companyPolicy.IsReinsured = true;
                }
            }
            return new UifJsonResult(true, companyPolicy.IsReinsured);
        }
    }
}