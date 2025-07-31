// -----------------------------------------------------------------------
// <copyright file="VehicleVersionYearController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using ENUMO = Sistran.Core.Application.ModelServices.Enums;
    using MSVP = Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using MSUP = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Controlador de valor de vehiculo por año
    /// </summary>
    public class VehicleVersionYearController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Common.Entities.VehicleVersionYear", KeyType = KeyType.None };

        /// <summary>
        /// Section principal de valor de vehiculo por año
        /// </summary>
        /// <returns>vista principal</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Busqueda avanzada del valor de vehiculo por año
        /// </summary>
        /// <returns>vista de busqueda avanzada</returns>
        public ActionResult AdvancedSearch()
        {
            return View();
        }
        
        /// <summary>
        /// Consulta las marcas
        /// </summary>
        /// <param name="selectedId">id por default</param>
        /// <returns>listado de marcas</returns>
        public ActionResult GetMakes(int? selectedId)
        {
            MSVP.MakesServiceModel makes = DelegateService.VehicleParamServices.GetMakes();
            if (makes.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, new { items = makes.ListMakesServiceModel, selectedId = selectedId });
            }
            else
            {
                return new UifJsonResult(false, new { makes.ErrorTypeService, makes.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene los modelos por el id de la marca
        /// </summary>
        /// <param name="makeId">id de la marca</param>
        /// <returns>listado de modelos</returns>
        public ActionResult GetModelsByMakeId(int makeId, int? selectedId)
        {
            MSVP.ModelsServiceModel models = DelegateService.VehicleParamServices.GetModelsByMakeId(makeId);
            if (models.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, new { items = models.ListModelServiceModel, selectedId = selectedId });
            }
            else
            {
                return new UifJsonResult(false, new { models.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene las versiones 
        /// </summary>
        /// <param name="makeId">id de la marca</param>
        /// <param name="modelId">id del modelo</param>
        /// <param name="selectedId">valor por defecto</param>
        /// <returns>listado de versiones</returns>
        public ActionResult GetVersionsByMakeIdModelId(int makeId, int modelId, int? selectedId)
        {
            MSVP.VersionsServiceModel versions = DelegateService.VehicleParamServices.GetVersionsByMakeIdModelId(makeId, modelId);
            if (versions.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, new { items = versions.ListVersionServiceModel, selectedId = selectedId });
            }
            else
            {
                return new UifJsonResult(false, new {  versions.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene los valores de vehiculo por año
        /// </summary>
        /// <param name="vehicle">filtro en listado</param>
        /// <returns>listado de valores de vehiculos por año</returns>
        public ActionResult GetVehicleVersionYearServiceModel(VehicleVersionYearViewModel vehicle)

            
        {
           

            MSVP.VehicleVersionYearsServiceModel result = DelegateService.VehicleParamServices.GetVehicleVersionYearsSMByMakeIdModelIdVersionIdYear(vehicle.MakeId, vehicle.ModelId, vehicle.VersionId, vehicle.Year);
            if (result.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {                  
                return new UifJsonResult(true, result.VehicleVersionYearServiceModels);
            }
            else
            {
                return new UifJsonResult(false, new { result.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene los tipos de moneda
        /// </summary>
        /// <returns>listado de tipos de moneda</returns>
        public ActionResult GetCurrencies()
        {
            MSUP.CurrenciesServiceQueryModel currencies = DelegateService.UnderwritingParamServiceWeb.GetCurrencies();
            if (currencies.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, currencies.CurrencyServiceModel);
            }
            else
            {
                return new UifJsonResult(false, new { currencies.ErrorTypeService, currencies.ErrorDescription });
            }
        }

        /// <summary>
        /// Exporta archivo de excel
        /// </summary>
        /// <param name="makeId">id de la marca</param>
        /// <param name="modelId">id del modelo</param>
        /// <param name="versionId">id de la version</param>
        /// <returns>archivo de excel de valor de vehiculo por año</returns>
        public ActionResult GenerateFileToExport(int makeId, int modelId, int versionId)
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.VehicleParamServices.GenerateFileToVehicleVersionYear(makeId, modelId, versionId);
                if (excelFileServiceModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
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

        #region CRUD
        /// <summary>
        /// Crud de valor de vehiculo por año
        /// </summary>
        /// <param name="vehicleVM">modelo de servicio de valor de vehiculo por año</param>
        /// <returns>resultado de operacion CRUD Dinamico</returns>
        public ActionResult Save(VehicleVersionYearViewModel vehicleVM)
        {
            string resultCRUD="";
            switch (vehicleVM.Status)
            {
                case Application.EntityServices.Enums.StatusTypeService.Create:                    
                    default:
                    resultCRUD= Create(vehicleVM);
                    break;
                case Application.EntityServices.Enums.StatusTypeService.Update:
                    resultCRUD = Update(vehicleVM);
                    break;
                case Application.EntityServices.Enums.StatusTypeService.Delete:
                    resultCRUD = Delete(vehicleVM);
                    break;                                
            }
            return new UifJsonResult(true, new { result= resultCRUD });
        }

        /// <summary>
        /// Operacion Crear
        /// </summary>
        /// <param name="vehicleVersionYearViewModel">valor de vehiculo por año</param>
        /// <returns>resultado operacion</returns>
        public string Create(VehicleVersionYearViewModel vehicleVersionYearViewModel)
        {
            string result = string.Format(App_GlobalResources.Language.ErrorCRUDCONNEX, App_GlobalResources.Language.ValueVehicleYear);
            try
            {
                this.AssignPostEntity(vehicleVersionYearViewModel);
                this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                Field field = this.postEntity.Fields.First(x => x.Name == "VehicleVersionCode");
                if (field.Value != "0")
                {
                    result = string.Format(App_GlobalResources.Language.CreateCRUD, App_GlobalResources.Language.ValueVehicleYear);
                }
            }
            catch (Exception ex)
            {
                result = string.Format(ex.Message + " " + vehicleVersionYearViewModel.Id + "<br/>");
            }            
            return result;
        }

        /// <summary>
        /// Operacion Actualizar
        /// </summary>
        /// <param name="vehicleVersionYearViewModel">valor de vehiculo por año</param>
        /// <returns>resultado operacion</returns>
        public string Update(VehicleVersionYearViewModel vehicleVersionYearViewModel)
        {            
            string result = string.Format(App_GlobalResources.Language.ErrorCRUDCONNEX, App_GlobalResources.Language.ValueVehicleYear);
            try
            {
                this.AssignPostEntity(vehicleVersionYearViewModel);
                DelegateService.EntityServices.Update(this.postEntity);
                result = string.Format(App_GlobalResources.Language.UpdateCRUD, App_GlobalResources.Language.ValueVehicleYear);
            }
            catch (Exception ex)
            {
                result = string.Format(ex.Message + " " + vehicleVersionYearViewModel.Id + "<br/>");
            }
            return result;
        }

        /// <summary>
        /// Operacion eliminar
        /// </summary>
        /// <param name="vehicleVersionYearViewModel">valor de vehiculo por año</param>
        /// <returns>resultado operacion</returns>
        public string Delete(VehicleVersionYearViewModel vehicleVersionYearViewModel)
        {
            string result = string.Format(App_GlobalResources.Language.ErrorCRUDCONNEX, App_GlobalResources.Language.ValueVehicleYear);
            ///<summary>
            ///Se hace comentario el código que va de la línea 259 a 261 y líneas de la 270 a 274, que contenía una validación 
            ///relacionada a los deducibles la cual no era necesaria después de hablarlo con Diana Lorena Oyola. Esta validación 
            ///no permitía la eliminación de registros. 
            ///</summary>
            ///<author>Diego Leon</author>
            ///<date>12/07/2018</date>
            ///<purpose>REQ_#080</purpose>
            ///<returns></returns>
            //int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidateDeductible(vehicleVersionYearViewModel.Id);
            //if (hasDependencies == 0)
            //{
            List<Field> fields = new List<Field>();
                fields.Add(new Field { Name = "VehicleVersionCode", Value = vehicleVersionYearViewModel.VersionId.ToString() });
                fields.Add(new Field { Name = "VehicleModelCode", Value = vehicleVersionYearViewModel.ModelId.ToString() });
                fields.Add(new Field { Name = "VehicleMakeCode", Value = vehicleVersionYearViewModel.MakeId.ToString() });
                fields.Add(new Field { Name = "VehicleYear", Value = vehicleVersionYearViewModel.Year.ToString() });
                this.postEntity.Fields = fields;
                DelegateService.EntityServices.Delete(this.postEntity);
                result = string.Format(App_GlobalResources.Language.DeleteCRUD, App_GlobalResources.Language.ValueVehicleYear);
            //}
            //else
            //{
            //    result = string.Format(App_GlobalResources.Language.ErrorDeleteWithDependencies, vehicleVersionYearViewModel.Id);
            //}
            return result;
        }

        /// <summary>
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item">modelo VehicleVersionYearViewModel</param>
        private void AssignPostEntity(VehicleVersionYearViewModel item)
        {
            List<Field> fields = new List<Field>();            
            fields.Add(new Field { Name = "VehicleVersionCode", Value = item.VersionId.ToString() });
            fields.Add(new Field { Name = "VehicleModelCode", Value = item.ModelId.ToString() });
            fields.Add(new Field { Name = "VehicleMakeCode", Value = item.MakeId.ToString() });
            fields.Add(new Field { Name = "VehicleYear", Value = item.Year.ToString() });
            fields.Add(new Field { Name = "VehiclePrice", Value = item.Price.ToString() });
            fields.Add(new Field { Name = "CurrencyCode", Value = item.CurrencyId.ToString() });
            this.postEntity.Fields = fields;
            this.postEntity.KeyType = KeyType.None;
            this.postEntity.Status = item.Status;
        }
        #endregion

    }
}