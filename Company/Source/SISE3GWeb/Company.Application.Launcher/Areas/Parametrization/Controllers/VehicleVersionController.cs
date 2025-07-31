using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class VehicleVersionController : Controller
    {
        #region Interfaz

        #endregion Interfaz
        public ActionResult VehicleVersion()
        {
            return View();
        }
        public ActionResult GetMakes()
        {
            VehicleMakesServiceQueryModel Makes = DelegateService.VehicleParamServices.GetVehicleMake();
            if (Makes.VehicleMakeServiceQueryModel == null)
            {
                Makes.VehicleMakeServiceQueryModel = new List<VehicleMakeServiceQueryModel>();
            }
            return new UifSelectResult(Makes.VehicleMakeServiceQueryModel);
        }
        public ActionResult GetModel()
        {
            VehicleModelsServiceQueryModel Makes = DelegateService.VehicleParamServices.GetVehicleModel();
            if (Makes.VehicleModelServiceQueryModel == null)
            {
                Makes.VehicleModelServiceQueryModel = new List<VehicleModelServiceQueryModel>();
            }
            return new UifSelectResult(Makes.VehicleModelServiceQueryModel);
        }
        public ActionResult GetModelByMake(int MakeID)
        {
            VehicleModelsServiceQueryModel Makes = DelegateService.VehicleParamServices.GetVehicleModelByMake(MakeID);
            if (Makes.VehicleModelServiceQueryModel == null)
            {
                Makes.VehicleModelServiceQueryModel = new List<VehicleModelServiceQueryModel>();
            }
            return new UifJsonResult(true,Makes.VehicleModelServiceQueryModel);
        }
        public ActionResult GetFuelType()
        {
            VehicleFuelsServiceQueryModel Fuel = DelegateService.VehicleParamServices.GetVehicleFuel();
            if (Fuel.VehicleFuelServiceQueryModel == null)
            {
                Fuel.VehicleFuelServiceQueryModel = new List<VehicleFuelServiceQueryModel>();
            }
            return new UifSelectResult(Fuel.VehicleFuelServiceQueryModel);
        }
        public ActionResult GetBody()
        {
            VehicleBodysServiceQueryModel Body = DelegateService.VehicleParamServices.GetVehicleBody();
            if (Body.VehicleBodyServiceQueryModel == null)
            {
                Body.VehicleBodyServiceQueryModel = new List<VehicleBodyServiceQueryModel>();
            }
            return new UifSelectResult(Body.VehicleBodyServiceQueryModel);
        }
        public ActionResult GetTypeVehicle()
        {
            VehicleTypesServiceQueryModel TypeVehicle = DelegateService.VehicleParamServices.GetVehicleType();
            if (TypeVehicle.VehicleTypeServiceQueryModel == null)
            {
                TypeVehicle.VehicleTypeServiceQueryModel = new List<VehicleTypeServiceQueryModel>();
            }
            return new UifSelectResult(TypeVehicle.VehicleTypeServiceQueryModel);
        }
        public ActionResult GetTransmissionType()
        {
            VehicleTransmissionTypesServiceQueryModel TransmissionType = DelegateService.VehicleParamServices.GetVehicleTransmissionType();
            if (TransmissionType.VehicleTransmissionTypeServiceQueryModel == null)
            {
                TransmissionType.VehicleTransmissionTypeServiceQueryModel = new List<VehicleTransmissionTypeServiceQueryModel>();
            }
            return new UifSelectResult(TransmissionType.VehicleTransmissionTypeServiceQueryModel);
        }
        public ActionResult GetCurrency()
        {
            CurrenciesServiceQueryModel Currency = DelegateService.VehicleParamServices.GetCurreny();
            if (Currency.CurrencyServiceModel == null)
            {
                Currency.CurrencyServiceModel = new List<CurrencyServiceQueryModel>();
            }
            return new UifSelectResult(Currency.CurrencyServiceModel);
        }
        public JsonResult SaveVehicleVersion(VehicleVersionViewModel VehicleVersionViewModel)
        {
            VehicleVersionServiceModel Result =ModelAssembler.CreateVehicleVersion(VehicleVersionViewModel);
            Result = DelegateService.VehicleParamServices.ExecuteOperationVehicleVersion(Result);
            if (Result.StatusTypeService == StatusTypeService.Error)
            {
                return new UifJsonResult(false, Result.ErrorServiceModel.ErrorDescription);
            }
            else
            {
                if ((StatusTypeService)VehicleVersionViewModel.StatusTypeService == StatusTypeService.Create)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.VehicleVersionSaveSuccessfully);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.VehicleVersionSuccessfullyModified);
                }
            }
        }
        public ActionResult VehicleVersionSearch()
        {
            return PartialView();
        }
        public JsonResult VehicleVersionSearchAdvanzed(VehicleVersionViewModel VehicleVersionViewModel)
        {
            VehicleVersionServiceModel Result = new VehicleVersionServiceModel()
            {
                Description = VehicleVersionViewModel.Description,
                VehicleMakeServiceQueryModel = VehicleVersionViewModel.VehicleMakeServiceQueryModel > 0 ? new VehicleMakeServiceQueryModel() { Id = VehicleVersionViewModel.VehicleMakeServiceQueryModel } : new VehicleMakeServiceQueryModel(),
                VehicleModelServiceQueryModel= VehicleVersionViewModel.VehicleModelServiceQueryModel>0?new VehicleModelServiceQueryModel() { Id= VehicleVersionViewModel.VehicleModelServiceQueryModel }:new VehicleModelServiceQueryModel ()
            };
            Result.StatusTypeService = StatusTypeService.Original;
            VehicleVersionsServiceModel ResultService = DelegateService.VehicleParamServices.GetAdvanzedSearchVehicleVersion(Result);
            if (Result.StatusTypeService == StatusTypeService.Error)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchVehicleVersion);
            }

            else
            {
                List<VehicleVersionViewModel> List = ModelAssembler.CreateVehicleVersionsServiceModel(ResultService);
                return new UifJsonResult(true, List);
            }
        }

        public JsonResult SearchVehiculeVersionDescription(string Description)
        {
            VehicleVersionsServiceModel Result = DelegateService.VehicleParamServices.GetVehicleVersionByDescription(Description);
            List<VehicleVersionViewModel> List = ModelAssembler.CreateVehicleVersionsServiceModel(Result);
            return new UifJsonResult(true, List);
        }
        public JsonResult DeleteVehicleVersion(VehicleVersionViewModel VehicleVersionViewModel)
        {
            VehicleVersionServiceModel Result = ModelAssembler.CreateVehicleVersion(VehicleVersionViewModel);
            Result.StatusTypeService = StatusTypeService.Delete;
            Result = DelegateService.VehicleParamServices.DeleteVehicleVersion(Result);
            if (Result.StatusTypeService == StatusTypeService.Error)
            {
                return new UifJsonResult(true,new {message = Result.ErrorServiceModel.ErrorDescription,type= StatusTypeService.Error });
            }
            else
            {
                return new UifJsonResult(true, new { message = App_GlobalResources.Language.VehicleVersionDeleteSuccessfully, type = StatusTypeService.Original });
            }
        }
        public JsonResult ExportFileVehicleVersion()
        {
            ExcelFileServiceModel Result = DelegateService.VehicleParamServices.GenerateFileToVehicleVersion(App_GlobalResources.Language.NameFileVehicleVersion);
            if (Result.ErrorTypeService == ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.FileData);
            }
            else
            {
                return new UifJsonResult(false, string.Join(",", Result.ErrorDescription));
            }
        }
        //public JsonResult ExportFileVehicleVersion(int MakeID, int ModelID)
        //{
        //    ExcelFileServiceModel Result = DelegateService.VehicleParamServices.GenerateFileToVehicleVersion(App_GlobalResources.Language.NameFileVehicleVersion, MakeID, ModelID);
        //    if (Result.ErrorTypeService == ErrorTypeService.Ok)
        //    {
        //        return new UifJsonResult(true, DelegateService.CommonService.GetKeyApplication("TransferProtocol")+ Result.FileData);
        //    }
        //    else
        //    {
        //        return new UifJsonResult(false,string.Join(",",Result.ErrorDescription));
        //    }
        //}

       

    }
}
