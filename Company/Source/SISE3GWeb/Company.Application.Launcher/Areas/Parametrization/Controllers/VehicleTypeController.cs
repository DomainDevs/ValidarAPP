//-----------------------------------------------------------------------
// <copyright file="VehicleTypeController.cs" company="Sistran">
// Copyright (c) Sistran. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
//-----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.Vehicles.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Controls.UifTable;

    /// <summary>
    /// Controlador de tipo de vehiculos
    /// </summary>
    public class VehicleTypeController : Controller
    {
        /// <summary>
        /// Abre la vista de Tipo de Vehiculos
        /// </summary>
        /// <returns>Vista de tipo de vehiculo</returns>
        [NoDirectAccess]
        public ActionResult VehicleType()
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
        /// Obtiene las carrocerias ordenadas alfabeticamente
        /// </summary>
        /// <returns>Listado de carrocerias</returns>
        public ActionResult GetBodies()
        {
            try
            {
                List<Body> bodies = DelegateService.vehicleService.GetBodies();
                return new UifTableResult(bodies.OrderBy(p => p.Description));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el listado de tipo de vehiculos ordenados por descripcion
        /// </summary>
        /// <returns>Listado de tipo de vehiculos</returns>
        [HttpGet]
        public ActionResult GetVehicleTypes()
        {
            try
            {             
                List<VehicleTypeViewModel> vehicleType = ModelAssembler.CreateVehicleTypeViewModelDTO(DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationVehicleTypes());
                return new UifJsonResult(true, vehicleType.OrderBy(p => p.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleTypes);
            }
        }

        /// <summary>
        /// Guarda los tipos de vehiculos
        /// </summary>
        /// <param name="vehicleTypesView">Listado de vehiculos</param>
        /// <returns>Resultado de la operacion</returns>
        [HttpPost]
        public ActionResult Save(List<VehicleTypeViewModel> vehicleTypesView)
        {
            try
            {
                vehicleTypesView = vehicleTypesView.Where(p => p.State == (int)StatusTypeService.Create || p.State == (int)StatusTypeService.Update || p.State == (int)StatusTypeService.Delete).ToList();
                List<VehicleTypeDTO> resultSave = DelegateService.CompanyUnderwritingParamApplicationService.ExecuteOperationsApplicationVehicleType(ModelAssembler.CreateVehicleTypeServiceModelDTO(vehicleTypesView));

                //string[] errorCreated = resultSave.Where(p => p.State == (int)StatusTypeService.Create && p.ErrorServiceModel != null).SelectMany(p => p.ErrorServiceModel.ErrorDescription).ToArray();
                //string[] errorUpdated = resultSave.Where(p => p.State == (int)StatusTypeService.Update && p.ErrorServiceModel != null).SelectMany(p => p.ErrorServiceModel.ErrorDescription).ToArray();
                //string[] errorDeleted = resultSave.Where(p => p.State == (int)StatusTypeService.Delete && p.ErrorServiceModel != null).SelectMany(p => p.ErrorServiceModel.ErrorDescription).ToArray();

                int totalCreated = resultSave.Count(p => p.State == (int)StatusTypeService.Create);
                int totalUpdated = resultSave.Count(p => p.State == (int)StatusTypeService.Update);
                int totalDeleted = vehicleTypesView.Count(p => p.State == (int)StatusTypeService.Delete);
                string messageCreated = null;
                string messageUpdated = null;
                string messageDeleted = null;
                if (totalCreated > 0)
                {
                    messageCreated = $"{App_GlobalResources.Language.VehicleTypeCreated}: {totalCreated}";
                }

                if (totalUpdated > 0)
                {
                    messageUpdated = $"{App_GlobalResources.Language.VehicleTypeUpdated}: {totalUpdated}";
                }

                if (totalDeleted > 0)
                {
                    messageDeleted = $"{App_GlobalResources.Language.VehicleTypeDeleted}: {totalDeleted}";
                }

                return new UifJsonResult(
                    true,
                    new
                    {
                        //errorCreated = errorCreated,
                        //errorUpdated = errorUpdated,
                        //errorDeleted = errorDeleted,
                        messageCreated = messageCreated,
                        messageUpdated = messageUpdated,
                        messageDeleted = messageDeleted
                    });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveVehicleTypes);
            }
        }

        /// <summary>
        /// Genera el archivo de excel
        /// </summary>
        /// <returns>Link del archivo de excel</returns>
        public ActionResult GenerateFileVehicleTypeToExport()
        {
            try
            {
                ExcelFileServiceModel urlFile = new ExcelFileServiceModel()
                {
                    FileData = DelegateService.CompanyUnderwritingParamApplicationService.GenerateFileToApplicationVehicleType(App_GlobalResources.Language.VehicleTypeFileName)
                };

                if (!string.IsNullOrEmpty(urlFile.FileData))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile.FileData);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }

        }

        /// <summary>
        /// Genera el archivo de exel para las carrocerias
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <returns>Link del archivo de excel</returns>
        public ActionResult GenerateFileVehicleBodyToExport(VehicleTypeViewModel vehicleType)
        {
            try
            {
                ExcelFileServiceModel urlFile = new ExcelFileServiceModel()
                {
                    FileData = DelegateService.CompanyUnderwritingParamApplicationService.GenerateFileToApplicationVehicleBody(ModelAssembler.CreateVehicleTypeServiceModelDTO(vehicleType), App_GlobalResources.Language.VehicleTypeBodyFileName)
                };
                if (!string.IsNullOrEmpty(urlFile.FileData))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile.FileData);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}