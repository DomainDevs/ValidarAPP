//using Sistran.Core.Application.ModelServices.Models;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    /// <summary>
    /// Controlador para la vista de validacion de vehiculos por placa, chasis, motor entre otros.
    /// </summary>
    public class ValidationPlateController : Controller
    {

        /// <summary>
        /// Modelo Limite Rc
        /// </summary>ValidationPlateDTO
        private static List<ValidationPlateDTO> ltsValidationPlateViewModel = new List<ValidationPlateDTO>();
        //private static List<ValidationPlateViewModel> ltsValidationPlateViewModel = new List<ValidationPlateViewModel>();
        private static List<Make> makes = new List<Make>();
        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de aliados</returns>
        public ActionResult ValidationPlate()
        {
            return this.View();
        }

        public ActionResult DetailValidationPlate()
        {
            return PartialView();
        }

        public UifJsonResult GetMakes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.vehicleApplicationService.GetVehiclesMake());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMakes);
            }
        }

        public ActionResult GetCauses()
        {
            try
            {
                List<SelectDTO> Causes = DelegateService.vehicleApplicationService.GetVehicleNotInsuredCauses();
                return new UifJsonResult(true, Causes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCause);
            }


        }
        public ActionResult GetModelsByMakeId(int makeId)
        {
            List<SelectDTO> vehicleModels = DelegateService.vehicleApplicationService.GetModelsByMake(makeId);
            return new UifJsonResult(true, vehicleModels);
        }
        /// <summary>
        /// Obtiene lista de versiones para marca y modelo seleccionada
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ActionResult GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            List<SelectDTO> versionServiceModelList = DelegateService.vehicleApplicationService.GetVersionByMakeIdModelId(makeId, modelId);
            return new UifJsonResult(true, versionServiceModelList.OrderBy(x => x.Description).ToList());            
        }

        public ActionResult GetFasecoldaCodeByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {

            FasecoldaDTO versionServiceModelList = DelegateService.vehicleApplicationService.GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId);

            return new UifJsonResult(true, versionServiceModelList);
            
        }

        public UifJsonResult GetFasecoldaByCode(string code, int year)
        {
            try
            {
                FasecoldaDTO versionServiceModelList = DelegateService.vehicleApplicationService.GetVehicleByFasecoldaCode(code, year);

                return new UifJsonResult(true, versionServiceModelList);
            }
            catch (Exception e)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchFasecolda);
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
        /// Obtiene todos los Vehiculos activos e inactivos
        /// </summary>
        /// <returns>Retorna listado de Vehiculos activos e inactivos</returns>
        public ActionResult GetValidationPlate()
        {
            try
            {
                this.GetListValidationPlate();
                return new UifJsonResult(true, ltsValidationPlateViewModel.OrderBy(x => x.Id).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error obteniendo sisisi este es Vehiculos activos");
            }
        }
        ///// <summary>
        ///// Obtiene listado de vehiculos activos e inactivos
        ///// </summary>
        ///// <returns>Retorna lista vehiculos activos e inactivos</returns>
        private List<ValidationPlateDTO> GetListValidationPlate()
        {
            try
            {
                if (ltsValidationPlateViewModel.Count == 0)
                {
                    //ValidationPlateDTO validationPlateServModel = DelegateService.vehicleApplicationService.GetValidationPlate();
                    ltsValidationPlateViewModel = DelegateService.vehicleApplicationService.GetValidationPlate();
                    //ltsValidationPlateViewModel = ModelAssembler.CreateLimitRc(validationPlateServModel.ValidationPlateModel);                
                    return ltsValidationPlateViewModel.OrderBy(x => x.Plate).ToList();
                }
            }
            catch (Exception e)
            {

                string d = "Error obteniendo sisisi este es Vehiculos activos";
                string ds = e.Message;
            }
            

            return ltsValidationPlateViewModel;
        }

    }
}