// -----------------------------------------------------------------------
// <copyright file="InfringementStateController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using AutoMapper;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de la vista de creación y modificación de Estado de Infracciones (Vehículos).
    /// Módulo de parametrización.
    /// </summary>
    public class InfringementStateController : Controller
    {
        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de infracciones</returns>
        public ActionResult InfringementState()
        {
            return View();
        }

        public ActionResult GetInfringementState()
        {
            InfringementStatesServiceModel infringementStatesServiceModel = DelegateService.VehicleParamServices.GetInfringementState();
            if (infringementStatesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<InfringementStateViewModel> infringementStatesViewModel = new List<InfringementStateViewModel>();
                var imapperInfringementState = ModelAssembler.CreateMapInfringementState();
                foreach (InfringementStateServiceModel item in infringementStatesServiceModel.InfringementStateServiceModel)
                {
                    infringementStatesViewModel.Add(imapperInfringementState.Map<InfringementStateServiceModel, InfringementStateViewModel>(item));
                }
                return new UifJsonResult(true, infringementStatesViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { infringementStatesServiceModel.ErrorTypeService, infringementStatesServiceModel.ErrorDescription });
            }
        }

        public ActionResult SaveInfringementState(List<InfringementStateViewModel> lstInfringementStates)
        {
            List<InfringementStateServiceModel> infringementStateModel = new List<InfringementStateServiceModel>();
            var imapperInfringementStateView = ModelAssembler.CreateMapInfringementStateView();
            foreach (InfringementStateViewModel item in lstInfringementStates)
            {
                infringementStateModel.Add(imapperInfringementStateView.Map<InfringementStateViewModel, InfringementStateServiceModel>(item));
            }
            List<InfringementStateServiceModel> infringementStateServiceModels = DelegateService.VehicleParamServices.ExecuteOperationsInfringementState(infringementStateModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();
            foreach (InfringementStateServiceModel item in infringementStateServiceModels)
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
                    switch (item.StatusTypeService)
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

        public ActionResult InfringementStateSearch()
        {
            return this.View();
        }

        public ActionResult GetInfringementStatesByDescription(string description)
        {
            InfringementStatesServiceModel statesServiceModel = DelegateService.VehicleParamServices.GetInfringementStateByDescription(description);
            if (statesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<InfringementStateViewModel> statesViewModel = new List<InfringementStateViewModel>();
                var imapperInfringementState = ModelAssembler.CreateMapInfringementState();
                foreach (var item in statesServiceModel.InfringementStateServiceModel)
                {
                    statesViewModel.Add(imapperInfringementState.Map<InfringementStateServiceModel, InfringementStateViewModel>(item));
                }
                return new UifJsonResult(true, statesViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { statesServiceModel.ErrorTypeService, statesServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de aliados</returns>
        public ActionResult GenerateFileToExport()
        {
            InfringementStatesServiceModel infringementStatesServiceModel = DelegateService.VehicleParamServices.GetInfringementState();
            ExcelFileServiceModel file = DelegateService.VehicleParamServices.GenerateFileToInfringementState(infringementStatesServiceModel.InfringementStateServiceModel, App_GlobalResources.Language.FileNameInfringementState);
            if (infringementStatesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                if (string.IsNullOrEmpty(file.FileData))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + file.FileData);
                }
            }
            else
            {
                return new UifJsonResult(false, file.ErrorDescription);
            }
        }
    }
}