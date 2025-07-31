// -----------------------------------------------------------------------
// <copyright file="InfringementGroupController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Application.ModelServices.Models.VehicleParam;
    using AutoMapper;
    using Framework.UIF.Web.Services;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de la vista de creación y modificación de Grupos de Infracciones (Vehículos).
    /// Módulo de parametrización.
    /// </summary>
    public class InfringementGroupController : Controller
    {
        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de grupo de infracciones</returns>
        public ActionResult InfringementGroup()
        {
            return View();
        }

        public ActionResult GetInfringementGroup()
        {
            InfringementGroupsServiceModel groupsServiceModel = DelegateService.VehicleParamServices.GetInfringementGroup();
            if (groupsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<InfringementGroupViewModel> groupsViewModel = new List<InfringementGroupViewModel>();
                var imapperInfringementGroup = ModelAssembler.CreateMapInfringementGroup();
                foreach (var item in groupsServiceModel.InfringementGroupServiceModel)
                {
                    groupsViewModel.Add(imapperInfringementGroup.Map<InfringementGroupServiceModel, InfringementGroupViewModel>(item));
                }
                return new UifJsonResult(true, groupsViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { groupsServiceModel.ErrorTypeService, groupsServiceModel.ErrorDescription });
            }
        }


        public ActionResult SaveInfringementGroups(List<InfringementGroupViewModel> lstInfringementGroups)
        {
            List<InfringementGroupServiceModel> infringementGroupModel = new List<InfringementGroupServiceModel>();
            var imapperInfringementGroupView = ModelAssembler.CreateMapInfringementGroupView();
            foreach (InfringementGroupViewModel item in lstInfringementGroups)
            {
                infringementGroupModel.Add(imapperInfringementGroupView.Map<InfringementGroupViewModel, InfringementGroupServiceModel>(item));
            }
            List<InfringementGroupServiceModel> infringementGroupServiceModels = DelegateService.VehicleParamServices.ExecuteOperationsInfringementGroups(infringementGroupModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();
            foreach (InfringementGroupServiceModel item in infringementGroupServiceModels)
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

        public ActionResult InfringementGroupSearch()
        {
            return this.View();
        }

        public ActionResult GetInfringementGroupsByDescription(string description)
        {
            InfringementGroupsServiceModel groupsServiceModel = DelegateService.VehicleParamServices.GetInfringementGroupsByDescription(description);
            if (groupsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<InfringementGroupViewModel> groupsViewModel = new List<InfringementGroupViewModel>();
                var imapperInfringementGroup = ModelAssembler.CreateMapInfringementGroup();
                foreach (var item in groupsServiceModel.InfringementGroupServiceModel)
                {
                    groupsViewModel.Add(imapperInfringementGroup.Map<InfringementGroupServiceModel, InfringementGroupViewModel>(item));
                }
                return new UifJsonResult(true, groupsViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { groupsServiceModel.ErrorTypeService, groupsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de aliados</returns>
        public ActionResult GenerateFileToExport()
        {
            InfringementGroupsServiceModel groupsServiceModel = DelegateService.VehicleParamServices.GetInfringementGroup();
            ExcelFileServiceModel file = DelegateService.VehicleParamServices.GenerateFileToInfringementGroup(groupsServiceModel.InfringementGroupServiceModel, App_GlobalResources.Language.FileNameInfringementGroup);
            if (groupsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
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