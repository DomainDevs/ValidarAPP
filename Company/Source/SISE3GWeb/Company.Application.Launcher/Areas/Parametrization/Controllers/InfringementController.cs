// -----------------------------------------------------------------------
// <copyright file="InfringementController.cs" company="SISTRAN">
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
    /// Controlador de la vista de creación y modificación de Infracciones (Vehículos).
    /// Módulo de parametrización.
    /// </summary>
    public class InfringementController : Controller
    {
        private static List<InfringementGroupTypeServiceModel> listGroups;

        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de infracciones</returns>
        public ActionResult Infringement()
        {
            return View();
        }

        public ActionResult GetInfringementGroup()
        {
            InfringementGroupsTypeServiceModel groupsServiceModel = DelegateService.VehicleParamServices.GetInfringementGroupType();
            if (groupsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                listGroups = groupsServiceModel.InfringementGroupServiceModel;
                return new UifJsonResult(true, groupsServiceModel.InfringementGroupServiceModel);
            }
            else
            {
                return new UifJsonResult(false, new { groupsServiceModel.ErrorTypeService, groupsServiceModel.ErrorDescription });
            }
        }

        public ActionResult GetInfringementGroupActive()
        {
            InfringementGroupsTypeServiceModel groupsServiceModel = DelegateService.VehicleParamServices.GetInfringementGroupTypeActive();
            if (groupsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                listGroups = groupsServiceModel.InfringementGroupServiceModel;
                return new UifJsonResult(true, groupsServiceModel.InfringementGroupServiceModel);
            }
            else
            {
                return new UifJsonResult(false, new { groupsServiceModel.ErrorTypeService, groupsServiceModel.ErrorDescription });
            }
        }
        public ActionResult GetInfringement()
        {
            InfringementsServiceModel infringementsServiceModel = DelegateService.VehicleParamServices.GetInfringement();
            if (infringementsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<InfringementViewModel> infringementsViewModel = new List<InfringementViewModel>();
                var imapperInfringement = ModelAssembler.CreateMapInfringement();
                foreach (var item in infringementsServiceModel.InfringementServiceModel)
                {
                    InfringementViewModel itemVM = imapperInfringement.Map<InfringementServiceModel, InfringementViewModel>(item);
                    if (item.InfringementGroupCode != null)
                    {
                        if (listGroups != null)
                        {
                            if (listGroups.Find(x => x.InfringementGroupCode == item.InfringementGroupCode) != null)
                            {
                                itemVM.InfringementGroupDescription = listGroups.Find(x => x.InfringementGroupCode == item.InfringementGroupCode).InfringementGroupDescription.ToString();
                            }
                        }
                    }
                    else
                    {
                        itemVM.InfringementGroupDescription = "NINGUNO";
                    }
                    infringementsViewModel.Add(itemVM);
                }
                return new UifJsonResult(true, infringementsViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { infringementsServiceModel.ErrorTypeService, infringementsServiceModel.ErrorDescription });
            }
        }

        public ActionResult SaveInfringement(List<InfringementViewModel> lstInfringement)
        {
            List<InfringementServiceModel> infringementModel = new List<InfringementServiceModel>();
            var imapperInfringementView = ModelAssembler.CreateMapInfringementView();
            foreach (InfringementViewModel item in lstInfringement)
            {
                infringementModel.Add(imapperInfringementView.Map<InfringementViewModel, InfringementServiceModel>(item));
            }
            List<InfringementServiceModel> infringementServiceModels = DelegateService.VehicleParamServices.ExecuteOperationsInfringement(infringementModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();
            foreach (InfringementServiceModel item in infringementServiceModels)
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

        public ActionResult InfringementSearch()
        {
            return this.View();
        }

        public ActionResult GetInfringementByDescription(string description, string code, int? group)
        {
            InfringementsServiceModel infringementsServiceModel = DelegateService.VehicleParamServices.GetInfringementByDescription(description, code, group);
            if (infringementsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<InfringementViewModel> infringementsViewModel = new List<InfringementViewModel>();
                var imapperInfringement = ModelAssembler.CreateMapInfringement();
                foreach (var item in infringementsServiceModel.InfringementServiceModel)
                {
                    InfringementViewModel itemVM = imapperInfringement.Map<InfringementServiceModel, InfringementViewModel>(item);
                    if (item.InfringementGroupCode != null)
                    {
                        itemVM.InfringementGroupDescription = listGroups.Find(x => x.InfringementGroupCode == item.InfringementGroupCode).InfringementGroupDescription;
                    }
                    else
                    {
                        itemVM.InfringementGroupDescription = "NINGUNO";
                    }
                    infringementsViewModel.Add(itemVM);
                }
                return new UifJsonResult(true, infringementsViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { infringementsServiceModel.ErrorTypeService, infringementsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de aliados</returns>
        public ActionResult GenerateFileToExport()
        {
            InfringementsServiceModel infringementsServiceModel = DelegateService.VehicleParamServices.GetInfringement();
            ExcelFileServiceModel file = DelegateService.VehicleParamServices.GenerateFileToInfringement(infringementsServiceModel.InfringementServiceModel, App_GlobalResources.Language.FileNameInfringement);
            if (infringementsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
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