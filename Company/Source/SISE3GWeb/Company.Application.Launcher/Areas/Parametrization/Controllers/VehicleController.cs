// -----------------------------------------------------------------------
// <copyright file="VehicleController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Ramirez</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using Sistran.Core.Application.Vehicles.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;    

    public class VehicleController : Controller
    {
        #region Variables y Constantes

        private static List<Make> makes = new List<Make>();

        #endregion
        
        #region LLamado a Vistas

        /// <summary>
        /// Abre vista de parametrizacion de Fasecolda
        /// </summary>
        /// <returns>Vista Fasecolda</returns>
        public ActionResult Fasecolda()
        {
            return View();
        }

        /// <summary>
        /// Abre vista de Busqueda Avanzada
        /// </summary>
        /// <returnsAdvancedSearch> AdvancedSearch</returns>
        public PartialViewResult AdvancedSearchFasecolda()
        {
            return PartialView();
        }

        /// <summary>
        /// Concessionaire
        /// </summary>
        /// <returns></returns>
        public ActionResult Concessionaire()
        {
            return View();
        }
        #endregion

        #region Funciones

        /// <summary>
        /// Obtiene las marcas de BD para vehiculo
        /// </summary>
        /// <returns>Lista de Marcas</returns>
        public ActionResult GetMakes()
        {
            MakesServiceModel makeServiceModelList = DelegateService.VehicleParamServices.GetMakes();
            ErrorTypeService errorTypeProcess = makeServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<MakeViewModel> makesList = ModelAssembler.GetMakes(makeServiceModelList);
                return new UifJsonResult(true, makesList.OrderBy(x => x.Description).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(makeServiceModelList.ErrorDescription));
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList"></param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(List<string> errorList)
        {
            string errorMessages = string.Empty;
            foreach (string errorMessageItem in errorList)
            {
                errorMessages = errorMessages + errorMessageItem + " <br>";
            }
            return errorMessages;
        }

        /// <summary>
        /// Obtien listado de Modelos de acuerdo a la placa seleccionada
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>Lista de Modelos</returns>
        public ActionResult GetModelsByMakeId(int makeId)
        {
            ModelsServiceModel modelServiceModelList = DelegateService.VehicleParamServices.GetModelsByMakeId(makeId);
            ErrorTypeService errorTypeProcess = modelServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<ModelViewModel> modelsList = ModelAssembler.GetModels(modelServiceModelList);
                return new UifJsonResult(true, modelsList.OrderBy(x => x.Description).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(modelServiceModelList.ErrorDescription));
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// Obtiene lista de versiones para marca y modelo seleccionada
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ActionResult GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            VersionsServiceModel versionServiceModelList = DelegateService.VehicleParamServices.GetVersionsByMakeIdModelId(makeId, modelId);
            ErrorTypeService errorTypeProcess = versionServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<VersionViewModel> versionList = ModelAssembler.GetVersions(versionServiceModelList);
                return new UifJsonResult(true, versionList.OrderBy(x => x.Description).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(versionServiceModelList.ErrorDescription));
            }
            else
            {
                return null;
            }            
        }
        /// <summary>
        /// Metodo para obtener listado de versiones por codifo fasecolda
        /// </summary>
        /// <param name="fasecoldaId"></param>
        /// <returns></returns>
        public ActionResult GetVersionVehicleFasecoldaByFasecoldaId(string fasecoldaId)
        {
            VersionVehicleFasecoldasServiceModel fasecoldasServiceModelList = DelegateService.VehicleParamServices.GetVersionVehicleFasecoldaByFasecoldaId(fasecoldaId);
            ErrorTypeService errorTypeProcess = fasecoldasServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<FasecoldaViewModel> fasecoldaList = ModelAssembler.GetVersionVehicleFasecolda(fasecoldasServiceModelList);
                return new UifJsonResult(true, fasecoldaList);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(fasecoldasServiceModelList.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener listado de versiones de fasecolda de vehiculos por marca, modelo y version
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param> 
        /// <param name="versionId"></param>
        /// <returns></returns>
        public ActionResult GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? makeId, int? modelId, int? versionId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            VersionVehicleFasecoldasServiceModel fasecoldasServiceModelList = DelegateService.VehicleParamServices.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(versionId, modelId,makeId, fasecoldaMakeId, fasecoldaModelId);
            fasecoldaMakeId = fasecoldaModelId=="0"?string.Empty: fasecoldaMakeId;
            fasecoldaModelId = fasecoldaModelId == "0" ? string.Empty : fasecoldaModelId;
            ErrorTypeService errorTypeProcess = fasecoldasServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<FasecoldaViewModel> fasecoldaList = ModelAssembler.GetVersionVehicleFasecolda(fasecoldasServiceModelList);
                return new UifJsonResult(true, fasecoldaList);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(fasecoldasServiceModelList.ErrorDescription));
            }
            else
            {
                return null;
            }
        }
        

        /**/
        /// <summary>
        /// Guarda los codigos fasecolda
        /// </summary>
        /// <param name="fasecoldaViewModel">Listado de fasecolda a guardar</param>
        /// <returns>Resultado de la operacion</returns>
        [HttpPost]
        public ActionResult SaveFasecolda(List<FasecoldaViewModel> fasecoldaViewModel)
        {
            try
            {
               var fasecoldaViewModel2 = fasecoldaViewModel.Where(p => p.State == (int)StatusTypeService.Create || p.State == (int)StatusTypeService.Update || p.State == (int)StatusTypeService.Delete).ToList();
                //validar existencias del codigo fasecolda
                foreach (var fasecolda in fasecoldaViewModel.Where(p=> p.State == (int)StatusTypeService.Create || p.State == (int)StatusTypeService.Update))
                {
                   var found= DelegateService.VehicleParamServices.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(fasecolda.versionVehicle.Id, fasecolda.modelVehicle.Id, fasecolda.makeVehicle.Id,  fasecolda.MakeVehicleCode , fasecolda.ModelVehicleCode);
                    if (found.ErrorTypeService == ErrorTypeService.Ok)
                    {
                        if (ModelAssembler.GetVersionVehicleFasecolda(found).Count() > 0)
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorExistFasecolda + ": "+ (fasecolda.MakeVehicleCode + fasecolda.ModelVehicleCode));
                        }                      
                    }
                }

                List<VersionVehicleFasecoldaServiceModel> resultSave = DelegateService.VehicleParamServices.ExecuteOperationsFasecolda(ModelAssembler.CreateVehicleFasecoldaServiceModel(fasecoldaViewModel2));

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
                    messageCreated = $"{App_GlobalResources.Language.FasecoldaCreated}: {totalCreated}";
                }

                if (totalUpdated > 0)
                {
                    messageUpdated = $"{App_GlobalResources.Language.FasecoldaUpdated}: {totalUpdated}";
                }

                if (totalDeleted > 0)
                {
                    messageDeleted = $"{App_GlobalResources.Language.FasecoldaDeleted}: {totalDeleted}";
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
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavedFasecolda);
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
                ExcelFileServiceModel urlFile = DelegateService.VehicleParamServices.GenerateFileToVehicleType(App_GlobalResources.Language.FasecoldaFileName);
                if (urlFile.ErrorTypeService == ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile.FileData);
                }
                else
                {
                    return new UifJsonResult(false, string.Join("<br />", urlFile.ErrorDescription));
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }


        #endregion

    }
}