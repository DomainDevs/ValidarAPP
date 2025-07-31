// -----------------------------------------------------------------------
// <copyright file="VariableController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Sistran.Core.Application.ModelServices.Models.Common;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    ///  Controlador de variabler
    /// </summary>
    public class VariableController : Controller
    {
        /// <summary>
        /// Inicia la vista variable
        /// </summary>
        /// <returns>vista variable</returns>
        public ActionResult Variable()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene parametros
        /// </summary>
        /// <param name="parParameters">Lista de parametros</param>
        /// <returns>Lista de parametros</returns>
        public ActionResult GetParametersByParameterIds(List<ParametroViewModel> parParameters)
        {
            parParameters = new List<ParametroViewModel>();

            string parametrosIds = DelegateService.commonService.GetKeyApplication("ParametrosIds");

            string[] param = parametrosIds.Split(';');
            for (int i = 0; i < param.Length; i++)
            {
                ParametroViewModel parametersViewModel = new ParametroViewModel();
                parametersViewModel.ParameterId = System.Convert.ToInt32(param[i]);
                parParameters.Add(parametersViewModel);
            }
            
            List<ParameterServiceModel> parameterServiceModel = ModelAssembler.CreateParameter(parParameters);
            foreach (var item in parameterServiceModel)
            {
                item.ParametricServiceModel = new ParametricServiceModel();
                item.ParametricServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Original;
            }

            List<ParameterServiceModel> lstParameterServiceModel = DelegateService.companyCommonParamService.GetParameter(parameterServiceModel);
            List<ParametroViewModel> parametroViewModel = new List<ParametroViewModel>();
            foreach (var item in lstParameterServiceModel)
            {
                if (item.ParametricServiceModel.ErrorServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(false, new { item.ParametricServiceModel.ErrorServiceModel.ErrorTypeService, item.ParametricServiceModel.ErrorServiceModel.ErrorDescription });
                }
            }

            parametroViewModel = ModelAssembler.GetParameter(lstParameterServiceModel);

            return new UifJsonResult(true, parametroViewModel);
        }

        /// <summary>
        /// Accion para actializar el parametro
        /// </summary>
        /// <param name="parametroVM">Modelo MVC ParametroViewModel</param>
        /// <returns>resultado json</returns>
        public ActionResult CreateParameter(List<ParametroViewModel> parametroVM)
        {
            List<ParameterServiceModel> parameterServiceModel = new List<ParameterServiceModel>();
            parameterServiceModel = ModelAssembler.CreateParameter(parametroVM);
            foreach (var item in parameterServiceModel)
            {
                item.ParametricServiceModel = new ParametricServiceModel();
                item.ParametricServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Update;
                if (item.ParameterId == 1008)
                {
                    item.InfringementLogServiceModel = new InfringementLogServiceModel();
                    item.InfringementLogServiceModel.daysValidateInfringement = item.Value;
                    item.InfringementLogServiceModel.registrationDate = DateTime.Now;
                    item.InfringementLogServiceModel.userId = Helpers.SessionHelper.GetUserId();
                }
                else if (item.ParameterId == 1009)
                {
                    item.DiscontinuityLogServiceModel = new DiscontinuityLogServiceModel();
                    item.DiscontinuityLogServiceModel.daysDiscontinuity = item.Value;
                    item.DiscontinuityLogServiceModel.registrationDate = DateTime.Now;
                    item.DiscontinuityLogServiceModel.userId = Helpers.SessionHelper.GetUserId();
                }
            }

            List<ParameterServiceModel> lstparameterServiceModel = DelegateService.companyCommonParamService.ExecuteOperationsParameterServiceModel(parameterServiceModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();

            foreach (var item in lstparameterServiceModel)
            {
                if (item.ParametricServiceModel.ErrorServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.Ok)
                {
                    string errores = string.Empty;
                    foreach (var itemError in item.ParametricServiceModel.ErrorServiceModel.ErrorDescription)
                    {
                        errores += itemError;
                    }

                    parametrizationResult.Message += errores + "</br>";
                }
                else
                {
                    switch (item.ParametricServiceModel.StatusTypeService)
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
    }
}