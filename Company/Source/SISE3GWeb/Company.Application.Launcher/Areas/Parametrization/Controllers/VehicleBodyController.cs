//-----------------------------------------------------------------------
// <copyright file="VehicleBodyController.cs" company="Sistran">
// Copyright (c) Sistran. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
//-----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Controls.UifTable;
    using VECO = Sistran.Core.Application.Vehicles.VehicleServices.Models;

    /// <summary>
    /// Controlador de Carrocería de vehículo
    /// </summary>
    public class VehicleBodyController : Controller
    {
        /// <summary>
        /// Abre la vista de Carrocerías de vehículo
        /// </summary>
        /// <returns>Vista de Carrocerías de vehículo</returns>
        [NoDirectAccess]
        public ActionResult VehicleBody()
        {
            return this.View();
        }

        /// <summary>
        /// Abre la vista de busqueda avanzada
        /// </summary>
        /// <returns>Vista de busqueda avanzada</returns>
        public ActionResult AdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene los usos ordenadas alfabeticamente
        /// </summary>
        /// <returns>Listado de carrocerias</returns>
        public ActionResult GetUses()
        {
            try
            {
                List<VECO.Use> uses = DelegateService.vehicleService.GetUses();
                return new UifTableResult(uses.OrderBy(p => p.Description));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el listado de Carrocería de vehículo ordenados por descripcion
        /// </summary>
        /// <returns>Listado de Carrocería de vehículo</returns>
        [HttpGet]
        public ActionResult GetVehicleBodies()
        {
            try
            {
                VehicleBodiesServiceModel vehicleBodiesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetVehicleBodies();
                List<VehicleBodyViewModel> vehicleBody = ModelAssembler.CreateVehicleBodyViewModel(vehicleBodiesServiceModel);
                return new UifJsonResult(true, vehicleBody.OrderBy(p => p.ShortDescription));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleBodies);
            }
        }

        /// <summary>
        /// Guarda las Carrocerías de vehículo
        /// </summary>
        /// <param name="vehicleBodiesView">Listado de Carrocería de vehículo</param>
        /// <returns>Resultado de la operacion</returns>
        [HttpPost]
        public ActionResult Save(List<VehicleBodyViewModel> vehicleBodiesView)
        {
            try
            {
                vehicleBodiesView = vehicleBodiesView.Where(p => p.State == (int)StatusTypeService.Create || p.State == (int)StatusTypeService.Update || p.State == (int)StatusTypeService.Delete).ToList();
                List<VehicleBodyServiceModel> resultSave = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsVehicleBody(ModelAssembler.CreateVehicleBodyServiceModel(vehicleBodiesView));

                string[] errorCreated = resultSave.Where(p => p.StatusTypeService == StatusTypeService.Create && p.ErrorServiceModel != null).SelectMany(p => p.ErrorServiceModel.ErrorDescription).ToArray();
                string[] errorUpdated = resultSave.Where(p => p.StatusTypeService == StatusTypeService.Update && p.ErrorServiceModel != null).SelectMany(p => p.ErrorServiceModel.ErrorDescription).ToArray();
                string[] errorDeleted = resultSave.Where(p => p.StatusTypeService == StatusTypeService.Delete && p.ErrorServiceModel != null).SelectMany(p => p.ErrorServiceModel.ErrorDescription).ToArray();

                int totalCreated = resultSave.Count(p => p.StatusTypeService == StatusTypeService.Create && p.ErrorServiceModel == null);
                int totalUpdated = resultSave.Count(p => p.StatusTypeService == StatusTypeService.Update && p.ErrorServiceModel == null);
                int totalDeleted = resultSave.Count(p => p.StatusTypeService == StatusTypeService.Delete && p.ErrorServiceModel == null);
                string messageCreated = null;
                string messageUpdated = null;
                string messageDeleted = null;
                if (totalCreated > 0)
                {
                    messageCreated = $"{App_GlobalResources.Language.VehicleBodyCreated}: {totalCreated}";
                }

                if (totalUpdated > 0)
                {
                    messageUpdated = $"{App_GlobalResources.Language.VehicleBodyUpdated}: {totalUpdated}";
                }

                if (totalDeleted > 0)
                {
                    messageDeleted = $"{App_GlobalResources.Language.VehicleBodyDeleted}: {totalDeleted}";
                }

                return new UifJsonResult(
                    true,
                    new
                    {
                        errorCreated = errorCreated,
                        errorUpdated = errorUpdated,
                        errorDeleted = errorDeleted,
                        messageCreated = messageCreated,
                        messageUpdated = messageUpdated,
                        messageDeleted = messageDeleted
                    });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveVehicleBodies);
            }
        }

        /// <summary>
        /// Genera el archivo de excel
        /// </summary>
        /// <returns>Link del archivo de excel</returns>
        public ActionResult GenerateFileVehicleBodyToExport()
        {
            try
            {
                ExcelFileServiceModel urlFile = DelegateService.UnderwritingParamServiceWeb.GenerateFileToExportVehicleBody(App_GlobalResources.Language.VehicleBodyFileName);
                if (urlFile.ErrorTypeService == ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile.FileData);
                }
              
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);                
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// Genera el archivo de exel para los usos
        /// </summary>
        /// <param name="vehicleBody">Carrocería de vehículo</param>
        /// <returns>Link del archivo de excel</returns>
        public ActionResult GenerateFileVehicleUseToExport(VehicleBodyViewModel vehicleBody)
        {
            try
            {
                if (vehicleBody.VehicleUses != null)
                {
                    ExcelFileServiceModel urlFile = DelegateService.UnderwritingParamServiceWeb.GenerateFileToVehicleUse(ModelAssembler.CreateVehicleBodyServiceModel(vehicleBody), App_GlobalResources.Language.VehicleBodyUseFileName);
                    if (urlFile.ErrorTypeService == ErrorTypeService.Ok)
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile.FileData);
                    }
                    
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);                    
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistVehicleUse);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}