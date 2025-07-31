// -----------------------------------------------------------------------
// <copyright file="AllianceController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Controllers
{
    /// <summary>
    /// Controlador de la vista de creación y modificación de motivos de rechazo.
    /// Módulo de parametrización.
    /// </summary>
    public class GroundsRejectionController : Controller
    {
        /// <summary>
        /// Metodo. Llamado de vista inicial
        /// Vista de motivos de rechazo.
        /// </summary>        
        public ActionResult GroundsRejection()
        {
            return View();
        }

        /// <summary>
        /// Metodo. Llamado de vista busqueda avanzada
        /// Vista busqueda avanzada de motivos de rechazo.
        /// </summary>        
        public ActionResult AdvancedSearch()
        {
            return View();
        }

        /// <summary>
        /// Metodo obtiene listado de todos los motivos de rechazo
        /// Vista busqueda avanzada de motivos de rechazo.
        /// </summary>        
        public ActionResult GetRejectionCausesAll()
        {
           RejectionCausesServiceModel rejectionCausesServiceModel = DelegateService.AuthorizationPoliciesParamService.CompanyGetRejectionCauses();
            return new UifJsonResult(true, rejectionCausesServiceModel.RejectionCauseServiceModel.OrderBy(b => b.description));
        }

        /// <summary>
        /// Metodo obtiene listado de todos los motivos de rechazo por grupo de poliza       
        public ActionResult GetRejectionCausesByGroupPolicyId(int groupPolicyId)
        {
            RejectionCausesServiceModel rejectionCausesServiceModel = DelegateService.AuthorizationPoliciesParamService.CompanyGetRejectionCausesByGroupPolicyId(groupPolicyId);
            return new UifJsonResult(true, rejectionCausesServiceModel.RejectionCauseServiceModel.OrderBy(b => b.description));
        }

        /// <summary>
        /// Metodo obtiene listado de grupo de politicas
        /// Vista busqueda avanzada de motivos de rechazo.
        /// </summary>        
        public ActionResult GetGroupPolicies()
        {
            GenericModelsServicesQueryModel genericModelsServicesQueryModel = DelegateService.AuthorizationPoliciesParamService.CompanyGetGroupPolicies();
            return new UifJsonResult(true, genericModelsServicesQueryModel.GenericModelServicesQueryModel.OrderBy(b => b.description));
        }

        public ActionResult OperationBaseRejectionCauses(List<BaseRejectionCausesViewModel> ListBaseRejectionCauses)
        {
            List<RejectionCauseServiceModel> ListRejectionCauseServiceModel = new List<RejectionCauseServiceModel>();
            ListRejectionCauseServiceModel = ModelAssembler.CreateRejectionCauses(ListBaseRejectionCauses);
            ListRejectionCauseServiceModel = DelegateService.AuthorizationPoliciesParamService.CompanyExecuteOperationsRejectionCausesServiceModel(ListRejectionCauseServiceModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();
            foreach (var item in ListRejectionCauseServiceModel)
            {
                if (item.ErrorServiceModel.ErrorTypeService != ErrorTypeService.Ok)
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
                    switch ((StatusTypeService)item.StatusTypeService)
                    {
                        case StatusTypeService.Create:
                            parametrizationResult.TotalAdded++;
                            break;
                        case StatusTypeService.Update:
                            parametrizationResult.TotalModified++;
                            break;
                        case StatusTypeService.Delete:
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
        /// Metodo obtiene listado de todos los motivos de rechazo
        /// Vista busqueda avanzada de motivos de rechazo.
        /// </summary>        
        public ActionResult GetRejectionCausesByDescription(string description , int groupPolicie)
        {
            RejectionCausesServiceModel rejectionCausesServiceModel = DelegateService.AuthorizationPoliciesParamService.CompanyGetRejectionCauseByDescription(description , groupPolicie);
            return new UifJsonResult(true, rejectionCausesServiceModel.RejectionCauseServiceModel.OrderBy(b => b.description));
        }

        /// <summary>
        /// Genera archivo excel de delegaciones
        /// </summary>
        /// <returns>Arhivo de excel de delegaciones</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                var filename = App_GlobalResources.Language.ListRejectionCauses;
                ExcelFileServiceModel excelFileServiceModel = DelegateService.AuthorizationPoliciesParamService.CompanyGenerateFileToRejectionCause(filename);
                if (excelFileServiceModel.ErrorTypeService == ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}