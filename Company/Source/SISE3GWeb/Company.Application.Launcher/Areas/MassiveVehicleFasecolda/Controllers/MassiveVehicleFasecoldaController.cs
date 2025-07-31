using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.ComponentModel;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;
using System.Collections.Generic;
//using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.VehicleFasecolda.Controllers
{
    public class MassiveVehicleFasecoldaController : Controller
    {
        public ActionResult MassiveVehicleFasecolda()
        {
            return View();
        }

        public ActionResult UploadFileMassiveVehicleFasecolda()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCheckRequiredFields);
                }
                else if (Request.Files.Count > 0)
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
                    //using (Application.Utilities.Helper.NetworkConnection networkCon = new Application.Utilities.Helper.NetworkConnection(uri.LocalPath, new NetworkCredential(user, password, domain)))
                    //{
                    //    if (networkCon._resultConnection == 1219 || networkCon._resultConnection == 0)
                    //    {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(urlPath + fileName);
                            fileInfo.CopyTo(externalDirectoryPath.ToString() + fileName);
                            fileInfo.Delete();
                            break;
                    //    }
                    //    else
                    //    {
                    //        throw new Win32Exception(networkCon._resultConnection);
                    //    }
                    //}
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

        public ActionResult CreateLoadMassiveVehicleFasecolda(ProcessFasecoldaDTO fasecoldaDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProcessFasecoldaDTO result = new ProcessFasecoldaDTO();
                    fasecoldaDTO.Description = App_GlobalResources.Language.LabelProcess + " A" + " - " + App_GlobalResources.Language.LabelUser + " - " + SessionHelper.GetUserId() + " - " + App_GlobalResources.Language.NumberGuide + ": " + fasecoldaDTO.Description;

                    fasecoldaDTO.User = new User
                    {
                        UserId = SessionHelper.GetUserId(),
                        AccountName = SessionHelper.GetUserName(),
                        AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
                    };

                    result = DelegateService.companyMassiveVehicleParamApplicationService.GenerateLoadMassiveVehicleFasecolda(fasecoldaDTO);

                    return new UifJsonResult(!result.HasError, result);
                }
                else
                {
                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
                }
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatingUpload);
            }
        }

        public ActionResult GetErrorExcelProcessVehicleFasecolda(int loadProcessId)
        {
            try
            {
                string urlFile = null;
                urlFile = DelegateService.companyMassiveVehicleParamApplicationService.GetErrorExcelProcessVehicleFasecolda(loadProcessId);
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

        public ActionResult GetVehicleFasecoldaByProccessId(int loadProcessId)
        {
            try
            {
                var fasecoldaProcess = DelegateService.companyMassiveVehicleParamApplicationService.GetProcessMassiveVehiclefasecolda(loadProcessId);
                //foreach (var item in fasecoldaProcess)
                //{
                //    if (item.Id == 0)
                //    {
                //        return new UifJsonResult(true, new List<ProcessFasecoldaMassiveLoadDTO>());
                //    }
                //}
                
                return new UifJsonResult(true, fasecoldaProcess);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }

        public ActionResult GenerateProccessMassiveVehicleFasecolda(int processId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.companyMassiveVehicleParamApplicationService.GenerateProccessMassiveVehicleFasecolda(processId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGenerateProcess);
            }
        }
    }
}