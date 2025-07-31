using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Massive.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Controllers
{
    [Authorize]
    public class CancellationMassiveController : Controller
    {
        public ActionResult CancellationMassive()
        {
            return View();
        }

        public ActionResult UploadFile()
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
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUploadFile);
            }
        }
        #region get
        public ActionResult GetMassiveLoadsByDescription(string description)
        {
            return RedirectToAction("GetMassiveLoadsByDescription", "Massive", new { description = description });
        }

        public ActionResult GenerateFileToMassiveLoad(int massiveLoadProccessId)
        {
            try
            {
                string urlFile = DelegateService.massiveUnderwritingService.GenerateFileErrorsMassiveEmission(massiveLoadProccessId);

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        public ActionResult GetIssuanceResults(string description)
        {
            try
            {
                List<MassiveEmissionRow> policies = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(Convert.ToInt32(description), MassiveLoadProcessStatus.Issuance,false,false);

                if (policies.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoChargesFound);
                }
                else
                {

                    return new UifJsonResult(true, policies);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoad);
            }
        }
        public ActionResult GetTariffResults(string description)
        {
            try
            {
                List<MassiveEmissionRow> policies = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(Convert.ToInt32(description), MassiveLoadProcessStatus.Tariff,false,false);

                if (policies.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoChargesFound);
                }
                else
                {

                    return new UifJsonResult(true, policies);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoad);
            }
        }
        #endregion get
        public ActionResult IssuePolicy(MassiveLoad massiveLoad)
        {
            //massiveLoad = DelegateService.CancellationMassiveEndorsementServices.CreateIssuePolicy(massiveLoad);
            return new UifJsonResult(true, App_GlobalResources.Language.InitiatedIssue);
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
                    using (NetworkConnection networkCon = new NetworkConnection(uri.LocalPath, new NetworkCredential(user, password, domain)))
                    {
                        if (networkCon._resultConnection == 0)
                        {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(urlPath + fileName);
                            fileInfo.CopyTo(externalDirectoryPath.ToString() + fileName);
                            fileInfo.Delete();
                            break;
                        }
                        else
                        {
                            throw new Win32Exception(networkCon._resultConnection);
                        }
                    }
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
        public ActionResult CreateLoad(CancellationMassiveViewModel cancellationMassiveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MassiveLoad massiveLoad = ModelAssembler.CreateCancellationMassiveLoad(cancellationMassiveViewModel);
                    massiveLoad.User = new Application.UniqueUserServices.Models.User
                    {
                        UserId = SessionHelper.GetUserId(),
                        AccountName = SessionHelper.GetUserName(),
                        AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
                    };
                    massiveLoad.Description = App_GlobalResources.Language.MassiveCancellation;
                    //massiveLoad = DelegateService.CancellationMassiveEndorsementServices.CreateMassiveLoad(massiveLoad);
                    if (massiveLoad == null)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateLoad);
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.MessageGeneratedLoadNo + " " + massiveLoad.Id);
                    }

                }
                else
                {
                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatingUpload);
            }
        }

        public ActionResult TariffedLoad(MassiveLoad massiveLoad)
        {
            try
            {

                //DelegateService.CancellationMassiveEndorsementServices.CancellationMassiveByMassiveLoadId(massiveLoad);
                return new UifJsonResult(true, App_GlobalResources.Language.InitiatedTariff);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.InitiatedTariff);
            }
        }
    }
}