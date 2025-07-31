using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
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
    public class SubModuleController : Controller
    {
        List<SubModuleModelsView> subModulesByModule = new List<SubModuleModelsView>();
        List<SubModuleModelsView> subModules = new List<SubModuleModelsView>();
        List<SubModuleModelsView> enabledSubModules = new List<SubModuleModelsView>();
        List<SubModule> submodulesModel = new List<SubModule>();

        public ActionResult SubModule()
        {
            return View("SubModule");
        }

        [HttpGet]
        public ActionResult SubModuleAdvancedSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateFileToExport()
        {
            try
            {
                GetListModules();
                if (submodulesModel.Count > 0)
                {
                    string urlFile = DelegateService.uniqueUserService.GenerateFileToSubmodules(submodulesModel, App_GlobalResources.Language.SubModules);

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
        public ActionResult GetListSubModules()
        {
            try
            {
                GetListModules();
                return new UifJsonResult(true, subModules.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult SaveSubModule(List<SubModuleModelsView> modulesModelView)
        {
            try
            {
                modulesModelView = modulesModelView.Where(x => x.Status == enums.StatusItem.Modified.ToString() || x.Status == enums.StatusItem.Deleted.ToString()).ToList();
                List<SubModule> modules = ModelAssembler.CreateSubModules(modulesModelView);
                bool result = true;
                List<SubModule> list = new List<Application.UniqueUserServices.Models.SubModule>();
                if (modules.Count > 0)
                {
                    list = DelegateService.uniqueUserService.CreateSubModules(modules);
                    subModules = new List<SubModuleModelsView>();
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
                        parametrizationResult.Message += "" + " " + App_GlobalResources.Language.NotDeleteSubModule + "</br>";
                    }


                }
                return new UifJsonResult(result, parametrizationResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchSubModules);
            }
        }

        public ActionResult GetSubModulesByModuleId(int moduleId)
        {
            try
            {
                GetListModules();
                subModulesByModule = subModules.Where(x => x.ModuleId == moduleId).ToList();
                return new UifSelectResult(subModulesByModule.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchSubModules);
            }
        }
        public ActionResult GetEnabledSubModulesByModuleId(int moduleId)
        {
            try
            {
                GetListModules();

                if (moduleId != 0)
                {
                    enabledSubModules = subModules.Where(x => x.ModuleId == moduleId).ToList();
                }

                return new UifJsonResult(true, enabledSubModules.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchSubModules);
            }
        }
        public ActionResult GetCoHierarchiesAssociationByModuleSubModule(int moduleId, int subModuleId)
        {
            try
            {
                List<CoHierarchyAssociation> coHierarchiesAssociation = new List<CoHierarchyAssociation>();
                coHierarchiesAssociation = DelegateService.uniqueUserService.GetCoHierarchiesAssociationByModuleSubModule(moduleId, subModuleId);
                return new UifSelectResult(coHierarchiesAssociation.OrderBy(x => x.Description).ToList());
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
                return Json(subModules.Where(x => TextHelper.replaceAccentMarks(x.Description.ToLower()).Contains(TextHelper.replaceAccentMarks(query.ToLower()))).OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchSubModules);
            }
        }
        public void GetListModules()
        {
            if (subModules.Count == 0)
            {
                submodulesModel = DelegateService.uniqueUserService.GetSubModulesByModuleId(0);
                submodulesModel.ForEach(x => x.EnabledDescription = x.IsEnabled == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled);
                subModules = ModelAssembler.CreateSubModules(submodulesModel);
            }
        }


    }
}