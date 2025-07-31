using Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
using MASS = Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models;
using AutoMapper;

namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Controllers
{
    public class DelegationController : Controller
    {
        // GET: ParamAuthorizationPolicies/Delegation
        public ActionResult Delegation()
        {
            return View();
        }

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

        [HttpGet]
        public ActionResult GetSubModules()
        {
            try
            {
                SubModulesServiceQueryModel parametrizationSubModules = DelegateService.AuthorizationPoliciesParamService.GetSubModuleServiceModel();
                return new UifJsonResult(true, parametrizationSubModules.SubModuleServiceQueryModels.OrderBy(b => b.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubModule);
            }
        }



        public ActionResult GetSubModulesForItem(int idModule)
        {
            try
            {
                SubModulesServiceQueryModel parametrizationSubModules = DelegateService.AuthorizationPoliciesParamService.GetSubModuleForItemIdModuleServiceModel(idModule);
                return new UifJsonResult(true, parametrizationSubModules.SubModuleServiceQueryModels.OrderBy(b => b.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubModule);
            }
        }

        [HttpGet]
        public ActionResult GetHierarchies()
        {
            try
            {
                HierarchiesServiceQueryModel parametrizationHierarchies = DelegateService.AuthorizationPoliciesParamService.GetHierarchyServiceModel();
                return new UifJsonResult(true, parametrizationHierarchies.HierarchyServiceQueryModels.OrderBy(b => b.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetHierarchy);
            }
        }


        /// <summary>
        /// Obtiene delegados
        /// </summary>
        /// <returns>Retorna listado de delegados</returns>
        public ActionResult GetParametrizationDelegation()
        {
            HierarchiesAssociationServiceModel delegationServiceModel = DelegateService.AuthorizationPoliciesParamService.GetDelegationServiceModel();
            if (delegationServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                List<DelegationViewModel> parametrizationDelegationVM = new List<DelegationViewModel>();
                return new UifJsonResult(true, delegationServiceModel.HierarchyAssociationServiceModel);
            }
            else
            {
                return new UifJsonResult(false, new { delegationServiceModel.ErrorTypeService, delegationServiceModel.ErrorDescription });
            }
        }


        /// <summary>
        /// Metodo que obtiene lista busqueda simple
        /// </summary>
        /// <param name="description">Parametro descripcion</param>
        /// <returns>Retorna lista de Delegaciones</returns>
        public ActionResult GetListDelegation(string description)
        {
            try
            {
                HierarchiesAssociationServiceModel parametrizationDelegation = DelegateService.AuthorizationPoliciesParamService.GetDelegationByNameServiceModel(description);

                if (parametrizationDelegation.HierarchyAssociationServiceModel.Count != 0)
                {
                    return new UifJsonResult(true, parametrizationDelegation.HierarchyAssociationServiceModel);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDelegation);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDelegation);
            }
        }


        /// <summary>
        /// CRUD de delegacion
        /// </summary>
        /// <param name="parametrizationDelegationVM">Listado delegaciones VM</param>
        /// <returns>Conteo de CRUD de la operacion</returns>
        public ActionResult CreateParametrizationDelegation(List<DelegationViewModel> parametrizationDelegationVM)
        {
            List<HierarchyAssociationServiceModel> delegationServiceModel = new List<HierarchyAssociationServiceModel>();
            delegationServiceModel = MASS.ModelAssembler.CreateDelegations(parametrizationDelegationVM);
            List<HierarchyAssociationServiceModel> delegationsServiceModels = DelegateService.AuthorizationPoliciesParamService.ExecuteOperationsDelegationServiceModel(delegationServiceModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();

            foreach (var item in delegationsServiceModels)
            {
                if (item.ErrorServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.Ok)
                {
                    string errores = string.Empty;
                    foreach (var itemError in item.ErrorServiceModel.ErrorDescription)
                    {
                        errores += itemError;
                    }

                    parametrizationResult.Message += errores + "</br>";
                }
                else
                {
                    switch (item.StatusTypeService)
                    {
                        case ENUMSM.StatusTypeService.Create:
                            parametrizationResult.TotalAdded++;
                            break;
                        case ENUMSM.StatusTypeService.Update:
                            parametrizationResult.TotalModified++;
                            break;
                        case ENUMSM.StatusTypeService.Delete:
                            parametrizationResult.TotalDeleted++;
                            break;
                        default:
                            break;
                    }
                }
            }

            return new UifJsonResult(true, parametrizationResult);
        }

        /// <summary>
        /// Genera archivo excel de delegaciones
        /// </summary>
        /// <returns>Arhivo de excel de delegaciones</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                HierarchiesAssociationServiceModel delegationServiceModel = DelegateService.AuthorizationPoliciesParamService.GetDelegationServiceModel();
                if (delegationServiceModel.HierarchyAssociationServiceModel != null && delegationServiceModel.HierarchyAssociationServiceModel.Count > 0)
                {
                    ExcelFileServiceModel excelFileServiceModel = DelegateService.AuthorizationPoliciesParamService.GenerateFileToDelegation(delegationServiceModel.HierarchyAssociationServiceModel, App_GlobalResources.Language.ListDelegation);
                    if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                    {
                        var urlFile = excelFileServiceModel.FileData;
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
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
    }
}