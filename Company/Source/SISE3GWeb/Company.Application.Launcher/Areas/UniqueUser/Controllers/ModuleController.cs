using Newtonsoft.Json;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using enums = Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Controllers
{
    public class ModuleController : Controller
    {
        List<ModuleModelsView> modules = new List<ModuleModelsView>();
        List<ModuleModelsView> enabledModules = new List<ModuleModelsView>();
        List<Module> modulesModel = new List<Module>();

        public ActionResult Module()
        {
            return View("Module");
        }

        [HttpGet]
        public ActionResult ModuleAdvancedSearch()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GenerateFileToExport()
        {
            try
            {
                GetListModules();
                if (modulesModel.Count > 0)
                {
                    string urlFile = DelegateService.uniqueUserService.GenerateFileToModules(modulesModel, App_GlobalResources.Language.Modules);

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
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        public ActionResult GetModules()
        {
            try
            {
                GetListModules();
                return new UifJsonResult(true, modules.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }
        public ActionResult GetJSONModules(string query)
        {
            try
            {
                GetListModules();
                return Json(modules.Where(x => TextHelper.replaceAccentMarks(x.Description.ToLower()).Contains(TextHelper.replaceAccentMarks(query.ToLower()))).OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }
        public ActionResult GetEnabledModules()
        {
            try
            {
                GetListModules();
                enabledModules = modules.Where(x => x.Enabled == true).ToList();
                return new UifJsonResult(true, enabledModules.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult SaveModule(List<ModuleModelsView> modulesModelView)
        {
            try
            {
                modulesModelView = modulesModelView.Where(x => x.Status == enums.StatusItem.Modified.ToString() || x.Status == enums.StatusItem.Deleted.ToString()).ToList();
                List<Module> saveModules = ModelAssembler.CreateModules(modulesModelView);
                bool result = true;
                List<Module> list = new List<Application.UniqueUserServices.Models.Module>();
                if (saveModules.Count > 0)
                {

                    list = DelegateService.uniqueUserService.CreateModules(saveModules);
                    modules = new List<ModuleModelsView>();
                    GetListModules();
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
                        parametrizationResult.Message += "" + " " + App_GlobalResources.Language.NotDeleteModule + "</br>";
                    }



                }

                return new UifJsonResult(result, parametrizationResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }
        public void GetListModules()
        {
            if (modules.Count == 0)
            {
                modulesModel = DelegateService.uniqueUserService.GetModulesByDescription("");
                modulesModel.ForEach(x => x.EnabledDescription = x.IsEnabled == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled);
                modules = ModelAssembler.CreateModules(modulesModel);
            }
        }


    }
}