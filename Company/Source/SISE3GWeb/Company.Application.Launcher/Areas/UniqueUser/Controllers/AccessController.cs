using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using enumUtilities = Sistran.Core.Application.UtilitiesServices.Enums;
using enums = Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes;
//using enumCommon = Sistran.Core.Application.CommonService.Enums.AccessObjectType;

using Sistran.Core.Application.CommonService.Enums;
using Newtonsoft.Json;
using Sistran.Core.Application.ModelServices.Models.Param;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Controllers
{
    public class AccessController : Controller
    {
        private static List<AccessModelsView> accessModels = new List<AccessModelsView>();
        private static List<AccessObject> accessObject = new List<AccessObject>();

        public ActionResult Access()
        {
            return View("Access");
        }

        [HttpPost]
        public ActionResult GenerateFileToExport()
        {
            try
            {
                GetListAccess();
                if (accessObject.Count > 0)
                {
                    string urlFile = DelegateService.uniqueUserService.GenerateFileToAccess(accessObject, App_GlobalResources.Language.AccessList);
                    if (string.IsNullOrEmpty(urlFile))
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                    }
                    else
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        public ActionResult GetListAccessObject(int moduleId, int subModuleId)
        {
            try
            {
                GetListAccess();
                if (accessModels.Count > 0)
                {
                    if (subModuleId != 0)
                    {
                        List<AccessModelsView> accessParent = accessModels.Where(x => x.ModuleId == moduleId && x.SubModuleId == subModuleId && x.AccessTypeId != (int)enumUtilities.AccessObjectType.BUTTON).ToList();

                        return new UifJsonResult(true, accessParent.OrderBy(x => x.ModuleDescription).ToList());
                    }
                    else
                    {
                        if (moduleId != 0)
                        {
                            return new UifJsonResult(true, accessModels.Where(x => x.ModuleId == moduleId).OrderBy(x => x.ModuleDescription).ToList());
                        }
                        else
                        {
                            return new UifJsonResult(true, accessModels.OrderBy(x => x.ModuleDescription).ToList());
                        }
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.AccessNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }

        }
        public ActionResult SaveAccess(List<AccessModelsView> accessModelView)
        {
            try
            {
                accessModelView = accessModelView.Where(x => x.Status == enums.StatusItem.Modified.ToString() || x.Status == enums.StatusItem.Deleted.ToString()).ToList();
                List<AccessObject> access = ModelAssembler.CreateAccessObject(accessModelView);
                bool result = true;
                List<AccessObject> list = new List<Application.UniqueUserServices.Models.AccessObject>();
                if (access.Count > 0)
                {
                    list = DelegateService.uniqueUserService.CreateAccessObject(access);
                    accessModels = new List<AccessModelsView>();
                    GetListAccess();
                }
                ParametrizationResult parametrizationResult = new ParametrizationResult();
                foreach (var item in list)
                {

                    switch (item.Status)
                    {
                        case "Modified":
                            parametrizationResult.TotalAdded++;
                            break;
                        case "Update":
                            parametrizationResult.TotalModified++;
                            break;
                        case "Deleted":
                            parametrizationResult.TotalDeleted++;
                            break;
                        default:
                            break;
                    }

                    string error = "";
                    parametrizationResult.Message += "" + " " + error + "</br>";
                    if (item.Status == "NotDelete" && parametrizationResult.TotalAdded == 0 && parametrizationResult.TotalModified == 0 && parametrizationResult.TotalDeleted == 0)
                    {
                        parametrizationResult.Message += "" + " " + App_GlobalResources.Language.NotDeleteAccess + "</br>";
                    }


                }
                return new UifJsonResult(result, parametrizationResult);
            }

            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAccess);
            }
        }

        public ActionResult GetStatus()
        {
            try
            {
                List<SelectListItem> status = new List<SelectListItem>();
                status = EnumsHelper.GetItems<EnabledStatus>().Where(x => Convert.ToInt16(x.Value) != (int)EnabledStatus.All).ToList();
                return new UifJsonResult(true, status);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAccessType);
            }
        }
        public ActionResult GetJSONAccess(string query)
        {
            try
            {
                GetListAccess();
                return Json(accessModels.Where(x => TextHelper.replaceAccentMarks(x.Description.ToLower()).Contains(TextHelper.replaceAccentMarks(query.ToLower()))).OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult GetAccessAdvancedSearch(AccessObject access)
        {
            try
            {
                if (Convert.ToInt16(access.Status) == (int)EnabledStatus.Disabled)
                {
                    access.IsEnabled = false;
                }
                else
                {
                    access.IsEnabled = true;
                }
                List<AccessModelsView> modelSearch = ModelAssembler.CreateAccessObject(DelegateService.uniqueUserService.GetAccessesByAccess(access));
                return Json(modelSearch, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public PartialViewResult AccessAdvancedSearch()
        {
            return PartialView();
        }

        public void GetListAccess()
        {
            if (accessModels.Count == 0)
            {
                accessObject = DelegateService.uniqueUserService.GetAccessObject(false);
                accessObject.ForEach(x => x.EnabledDescription = x.IsEnabled == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled);
                accessModels = ModelAssembler.CreateAccessObject(DelegateService.uniqueUserService.GetAccessObject(false));
            }
        }
    }
}