using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models;
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
    public class VehicleModelController : Controller
    {


        // GET: Common/VehicleModel

        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de infracciones</returns>
        public ActionResult VehicleModel()
        {
            return View();
        }
        /// <summary>
        /// Obtiene planes de pago
        /// </summary>
        /// <returns>Retorna listado de marcas</returns>
        public JsonResult GetMakes()
        {
            List<VehicelMakeServiceQueryModel> Makes = DelegateService.VehicleParamServices.GetVehicelMake();
            if (Makes == null)
            {
                Makes = new List<VehicelMakeServiceQueryModel>();
            }
            return new UifSelectResult(Makes);
        }

        public ActionResult SaveVehicleModel(VehicleModelViewModel listvehicleModelViewModel)
        {

            if (listvehicleModelViewModel.DescriptionModel == null || listvehicleModelViewModel.SmallDescriptionModel == null || listvehicleModelViewModel.MakeId_Id <=0)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.LlenarDatos);
            }

            VehicleModelServiceModel vehicleModel = ModelAssembler.CreateVehicleModel(listvehicleModelViewModel);            

            List<VehicleModelServiceModel> listModel = new List<VehicleModelServiceModel>();
            listModel.Add(vehicleModel);
            listModel = DelegateService.VehicleParamServices.ExecuteOperationVehicleModel(listModel);

            if (listvehicleModelViewModel.StatusTypeService == StatusTypeService.Error)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErroDeleteInformation);
            }
            else
            {
                if ((StatusTypeService)listvehicleModelViewModel.StatusTypeService == StatusTypeService.Create)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CreateModel);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ModifiedModel);
                }
            }
        }
        public ActionResult VehicleModelSearch()
        {
            return PartialView();
        }
       
        public JsonResult DeleteVehicleModel(VehicleModelViewModel vehicleModelView)
        {
            VehicleModelServiceModel Result = ModelAssembler.CreateVehicleModel(vehicleModelView);
            Result.StatusTypeService = StatusTypeService.Delete;
            Result = DelegateService.VehicleParamServices.DeleteVehicleModel(Result);
            if (Result.StatusTypeService == StatusTypeService.Error)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErroDeleteInformation);
            }
            else
            {
                if (Result.StatusTypeService == StatusTypeService.Delete)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ConfirmDelete);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.Error);
                }
            }
            ///<summary>
            ///Se comentará el código a partir de la línea 112 hasta la 126, hace referencia a un 
            ///problema de validación presente que tenía como resultado la generación de mensajes 
            ///erróneos después de realizar una eliminación de información.
            ///La respectiva solución propuesta está a partir de la línea 87 hasta la 101.
            ///</summary>
            ///<author>Diego Leon</author>
            ///<date>11/07/2018</date>
            ///<purpose>REQ_#076</purpose>
            ///<returns></returns>
            //if (vehicleModelView.StatusTypeService == StatusTypeService.Error)
            //{
            //    return new UifJsonResult(false, App_GlobalResources.Language.ErroDeleteInformation);
            //}
            //else
            //{
            //    if ((StatusTypeService)vehicleModelView.StatusTypeService == StatusTypeService.Delete)
            //    {
            //        return new UifJsonResult(true, App_GlobalResources.Language.ConfirmDelete);
            //    }
            //    else
            //    {
            //        return new UifJsonResult(true, App_GlobalResources.Language.Error);
            //    }
            //}

        }
        public ActionResult ModelVehicleSearchAdv(VehicleModelViewModel vehicleModelViewModel)
        {
            List<VehicelMakeServiceQueryModel> Makes = DelegateService.VehicleParamServices.GetVehicelMake();
            VehicleModelServiceModel Result = new VehicleModelServiceModel()
            {
                Description = vehicleModelViewModel.DescriptionModel,
                VehicelMakeServiceQueryModel = vehicleModelViewModel.MakeId_Id > 0 ? new VehicelMakeServiceQueryModel() { Id = vehicleModelViewModel.MakeId_Id } : new VehicelMakeServiceQueryModel(),

            };
            Result.StatusTypeService = StatusTypeService.Original;
            VehicleModelsServiceModel ResultService = DelegateService.VehicleParamServices.GetAdvanASearchVehicleModel(Result);
            if (ResultService.VehicleModelServiceModel != null)
            {
                foreach (VehicleModelServiceModel item in ResultService.VehicleModelServiceModel)
                {
                    item.VehicelMakeServiceQueryModel = new VehicelMakeServiceQueryModel()
                    {
                        Id = item.VehicelMakeServiceQueryModel.Id,
                        Description = Makes.Where(x => x.Id == item.VehicelMakeServiceQueryModel.Id).FirstOrDefault().Description

                    };
                }
            }
            if (Result.StatusTypeService == StatusTypeService.Error)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }

            else
            {                
                
                List<VehicleModelViewModel> list = ModelAssembler.CreateVehicleServiceModel(ResultService);
                
                return new UifJsonResult(true, list);
            }

        }
        public JsonResult SearchVehiculeModelDescription(string Description, int Make = 0)
        {
            VehicleModelViewModel vehicleModelViewModel = new VehicleModelViewModel();
            List<VehicelMakeServiceQueryModel> Makes = DelegateService.VehicleParamServices.GetVehicelMake();
            VehicleModelServiceModel Result = new VehicleModelServiceModel()
            {
                Description = vehicleModelViewModel.DescriptionModel,
                VehicelMakeServiceQueryModel = vehicleModelViewModel.MakeId_Id > 0 ? new VehicelMakeServiceQueryModel() { Id = vehicleModelViewModel.MakeId_Id } : new VehicelMakeServiceQueryModel(),

            };

            VehicleModelsServiceModel ResultService = DelegateService.VehicleParamServices.GetInVehicleModelByDescription(Description, Make);
            if (ResultService.VehicleModelServiceModel == null)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.Error);
            }
            else
            {
                foreach (VehicleModelServiceModel item in ResultService.VehicleModelServiceModel)
                {
                    item.VehicelMakeServiceQueryModel = new VehicelMakeServiceQueryModel()
                    {
                        Id = item.VehicelMakeServiceQueryModel.Id,
                        Description = Makes.Where(x => x.Id == item.VehicelMakeServiceQueryModel.Id).FirstOrDefault().Description

                    };
                }

                List<VehicleModelViewModel> List = ModelAssembler.CreateVehicleServiceModel(ResultService);
                return new UifJsonResult(true, List);
            }


           
        }

        public ActionResult GenerateFileToExport()
        {
            try
            {
                VehicleModelsServiceModel VehicleModel = DelegateService.VehicleParamServices.GetInVehicleModels();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.VehicleParamServices.GenerateFileToVehicleModel(VehicleModel.VehicleModelServiceModel, App_GlobalResources.Language.VehicleModel);
                  
                if (excelFileServiceModel.ErrorTypeService == ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportModel);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }

        }

    }

  
}



    
