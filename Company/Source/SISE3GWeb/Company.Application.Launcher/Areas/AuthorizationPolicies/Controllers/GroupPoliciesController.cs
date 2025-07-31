using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{

    [Authorize]
    public class GroupPoliciesController : Controller
    {
        #region Vistas

        [HttpGet]
        public ActionResult GroupPolicies()
        {
            return View();
        }
	
        public ActionResult AdvancedSearchGroupPolicies()
        {
            return View();
        }

        #endregion

        /// <summary>
        /// retorna todos los grupos de politicas de la BD
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GetGroupPolicies()
        {
            try
            {
                var groupPoliciesView = new List<GroupPoliciesModelView>();

                var groupPolicies = DelegateService.AuthorizationPoliciesService.GetGroupsPolicies();
                foreach (var group in groupPolicies)
                {
                    groupPoliciesView.Add(GroupPoliciesModelView.CreateGroupPoliciesModelView(group));
                }
                return new UifJsonResult(true, groupPoliciesView);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        /// <summary>
        /// retorna todos los grupos de politicas de la BD
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GetGroupPoliciesByDescription(string description, int module, int subModule, string prefix)
        {
            try
            {
                var groupPoliciesView = new List<GroupPoliciesModelView>();

                var groupPolicies = DelegateService.AuthorizationPoliciesService.GetGroupPoliciesByDescription(description, module, subModule, prefix);
                foreach (var group in groupPolicies)
                {
                    groupPoliciesView.Add(GroupPoliciesModelView.CreateGroupPoliciesModelView(group));
                }
                return new UifJsonResult(true, groupPoliciesView);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        /// </summary>
        /// Obtiene toda la informacion de módulo habilitados.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetPackage()
        {
            try
            {
                List<Package> packages = DelegateService.rulesEditorServices.GetPackages();
                if (packages.Count != 0)
                {
                    return new UifJsonResult(true, packages.Where(X => X.Disabled == false).OrderBy(x => x.Description));
                }

                return new UifJsonResult(false, App_GlobalResources.Language.NoModuleData);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public ActionResult ExecuteOperationGroupPolicies(List<GroupPoliciesModelView> groupPolicies)
        {
            List<GroupPolicies> GroupPoliciesServiceModel = new List<GroupPolicies>();
           
            foreach (var item in groupPolicies)
            {
                switch (item.StatusTypeService)
                {
                    case StatusTypeService.Create:
                        this.CreatePoliciesGroup(item);
                        break;
                    case StatusTypeService.Update:
                        this.UpdatePoliciesGroup(item);
                        break;
                    case StatusTypeService.Delete:
                         int PoliceAut = this.DeleteGroupPolicies(item.IdGroupPolicies);
                        if(PoliceAut == 0)
                        {
                            item.StatusTypeService = StatusTypeService.Error;
                        }
                        break;
                    default:
                        this.CreatePoliciesGroup(item);
                        break;

                }
            }
            return new UifJsonResult(true, groupPolicies);
        }

        [HttpPost]
        public int  DeleteGroupPolicies(int groupPoliciesId)
        {
            try
            {
                 int result = DelegateService.AuthorizationPoliciesService.DeleteGroupPolicies(groupPoliciesId);
                return  result;            
                
            }

            catch (Exception ex)
            {
                int error = 1;
                return error;
            }
        }

        [HttpPost]
        public ActionResult CreatePoliciesGroup(GroupPoliciesModelView group)
        {
            try
            {
                var groupPolicy = GroupPoliciesModelView.CreateGroupPolicies(group);
                DelegateService.AuthorizationPoliciesService.CreateGroupPolicies(groupPolicy);
                return new UifJsonResult(true, null);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdatePoliciesGroup(GroupPoliciesModelView group)
        {
            try
            {
                var groupPolicy = GroupPoliciesModelView.CreateGroupPolicies(group);
                DelegateService.AuthorizationPoliciesService.UpdateGroupPolicies(groupPolicy);
                return new UifJsonResult(true, null);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        /// </summary>
        /// Obtiene toda la informacion de módulo habilitados.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        ///       
        public ActionResult GetModules()
        {
            try
            {
                ModuleSubmoduleServicesQueryModel parametrizationDelegation = DelegateService.AuthorizationPoliciesParamService.GetModuleServiceModel();
                return new UifJsonResult(true, parametrizationDelegation.ModuleSubModuleQueryModel.OrderBy(b => b.Description));
            }
            catch (System.Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetModule);
            }
        }

        /// </summary>
        /// Obtiene toda la informacion de SubMódulo habilitados.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetSubModules(int module)
        {
            try
            {
                SubModulesServiceQueryModel parametrizationSubModules = DelegateService.AuthorizationPoliciesParamService.GetSubModuleForItemIdModuleServiceModel(module);
                return new UifJsonResult(true, parametrizationSubModules.SubModuleServiceQueryModels.OrderBy(b => b.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubModule);
            }
        }

        /// </summary>
        /// Obtiene toda Ramos 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        public ActionResult GetPrefixes()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
            return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
        }

        /// </summary>
        /// Obtiene toda Ramos 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        public ActionResult GetCoveredRiskType(int prefix)
        {
            List<CoveredRisk> CoveredRiskType = DelegateService.AuthorizationPoliciesService.GetCoveredRiskByPrefix(prefix);
            return new UifJsonResult(true, CoveredRiskType.OrderBy(x => x.Description).ToList());
        }
    }
}