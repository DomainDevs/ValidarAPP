using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.ComponentModel;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson.Controllers
{
    public class ListRiskFileController : Controller
    {
        public ActionResult ListRiskFile()
        {
            return View();
        }

        public ActionResult UploadFileListRisk()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    String urlPath = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + SessionHelper.GetUserName() + @"\";

                    if (!System.IO.Directory.Exists(urlPath))
                    {
                        System.IO.Directory.CreateDirectory(urlPath);
                    }

                    HttpPostedFileBase httpPostedFileBase = Request.Files[0] as HttpPostedFileBase;
                    string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                    httpPostedFileBase.SaveAs(urlPath + fileName);

                    if (CopyFile(fileName, urlPath))
                    {
                        return new UifJsonResult(true, fileName);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorUploadFile);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotUploadFile);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUploadFile);
            }
        }

        private bool CopyFile(string fileName, string urlPath)
        {
            string externalDirectoryPath = "";
            externalDirectoryPath = DelegateService.commonService.GetKeyApplication("ExternalFolderFiles") + @"\" + SessionHelper.GetUserName() + @"\";

            if (!System.IO.Directory.Exists(externalDirectoryPath))
            {
                System.IO.Directory.CreateDirectory(externalDirectoryPath);
            }

            string user = DelegateService.commonService.GetKeyApplication("UserDomain");
            string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
            string domain = DelegateService.commonService.GetKeyApplication("Domain");

            Uri uri = new Uri(externalDirectoryPath, UriKind.Absolute);

            int retries = 0;

            while (retries < 5)
            {
                try
                {

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(urlPath + fileName);
                    fileInfo.CopyTo(externalDirectoryPath.ToString() + fileName);
                    fileInfo.Delete();
                    break;

                }
                catch (Exception)
                {
                    retries++;
                    if (retries == 5)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public ActionResult CreateLoadListRiskFile(ListRiskLoadDTO listRiskLoadDTO)
        {
            try
            {
                ListRiskLoadDTO result = new ListRiskLoadDTO();
                listRiskLoadDTO.Description = App_GlobalResources.Language.LabelProcess + " A" + " - " + App_GlobalResources.Language.LabelUser + " - " + SessionHelper.GetUserId() + " - " + /*App_GlobalResources.Language.NumberGuide +*/ ": " + listRiskLoadDTO.Description;
                listRiskLoadDTO.User = new User
                {
                    UserId = SessionHelper.GetUserId(),
                    AccountName = SessionHelper.GetUserName(),
                    AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
                };

                result = DelegateService.uniquePersonListRiskApplication.GenerateLoadListRisk(listRiskLoadDTO);

                return new UifJsonResult(!result.HasError, result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatingUpload);
            }
        }

        public ActionResult GetProcessStatusById(int processId)
        {
            try
            {
                ListRiskStatusDTO result = DelegateService.uniquePersonListRiskApplication.GetStatusByProcessId(processId);
                return new UifJsonResult(true, result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcessStatus);
            }

        }

        public ActionResult GetErrorExcelProcessListRisk(int loadProcessId)
        {
            try
            {
                string urlFile = null;
                urlFile = DelegateService.uniquePersonListRiskApplication.GetErrorExcelProcessListRisk(loadProcessId);
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        public ActionResult SetInitialProcessFile(int listRiskProcessId)
        {
            try
            {
                ListRiskLoadDTO listRiskLoadDTO = DelegateService.uniquePersonListRiskApplication.GetMassiveListRiskByProcessId(listRiskProcessId);
                var result = DelegateService.uniquePersonListRiskApplication.RecordListRisk(listRiskLoadDTO);
                return new UifJsonResult(true, result);

            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, false);
            }

        }
    }


}