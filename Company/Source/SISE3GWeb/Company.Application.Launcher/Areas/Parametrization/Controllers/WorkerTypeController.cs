using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.ModelServices.Models.UniquePerson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using AutoMapper;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;

    public class WorkerTypeController : Controller
    {
       private static List<WorkerTypeServiceModel> listGroups;
        // GET: Parametrization/WorkerType
        public ActionResult WorkerType()
        {
            return View();
        }

        public ActionResult WorkerTypeSearch()
        {
            return this.View();
        }


        public ActionResult GetWorkerTypesByDescription(string description)
        {
            WorkerTypesServiceModel workerTypesServiceModel = DelegateService.companyUniquePersonParamService.GetWorkertypeByDescription(description);
            if (workerTypesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<WorkerTypeViewModel> workerTypesViewModel = new List<WorkerTypeViewModel>();
                var imapperWorkerType = ModelAssembler.CreateMapWorkerType();
                foreach (var item in workerTypesServiceModel.WorkerTypeServiceModel)
                {
                    workerTypesViewModel.Add(imapperWorkerType.Map<WorkerTypeServiceModel, WorkerTypeViewModel>(item));
                }
                return new UifJsonResult(true, workerTypesViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { workerTypesServiceModel.ErrorTypeService, workerTypesServiceModel.ErrorDescription });
            }
        }


        /// <summary>
        /// Obtiene el listado de tipo de trabajadores ordenados por descripcion
        /// </summary>
        /// <returns>Listado de tipo de trabajadores</returns>

        public ActionResult GetWorkerType()
        {
            WorkerTypesServiceModel workerTypesServiceModel = DelegateService.companyUniquePersonParamService.GetWorkertype();
            if (workerTypesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<WorkerTypeViewModel> workerTypeViewModel = new List<WorkerTypeViewModel>();
                var config = MapperCache.GetMapper<WorkerTypeServiceModel, WorkerTypeViewModel>(cfg =>
                {
                    cfg.CreateMap<WorkerTypeServiceModel, WorkerTypeViewModel>();
                });
                foreach (var item in workerTypesServiceModel.WorkerTypeServiceModel)
                {
                    workerTypeViewModel.Add(config.Map<WorkerTypeServiceModel, WorkerTypeViewModel>(item));
                    
                }
                return new UifJsonResult(true, workerTypeViewModel.OrderBy(x => x.Description));
            }
            else
            {
                return new UifJsonResult(false, new { workerTypesServiceModel.ErrorTypeService, workerTypesServiceModel.ErrorDescription });
            }
        }
        
        public ActionResult SaveWorkerTypes(List<WorkerTypeViewModel> lstWorkerTypes)
        {
            List<WorkerTypeServiceModel> workerTypeModel = new List<WorkerTypeServiceModel>();
            var config = MapperCache.GetMapper<WorkerTypeViewModel, WorkerTypeServiceModel>(cfg =>
            {
                cfg.CreateMap<WorkerTypeViewModel, WorkerTypeServiceModel>();
            });
            foreach (WorkerTypeViewModel item in lstWorkerTypes)
            {
                workerTypeModel.Add(config.Map<WorkerTypeViewModel, WorkerTypeServiceModel>(item));
            }
            List<WorkerTypeServiceModel> workerTypeServiceModels = DelegateService.companyUniquePersonParamService.ExecuteOperationsWorkerType(workerTypeModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();
            foreach (WorkerTypeServiceModel item in workerTypeServiceModels)
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
                        default:
                            break;
                    }
                }
            }
            return new UifJsonResult(true, parametrizationResult);
        }

        public ActionResult GenerateFileToExport()
        {
            WorkerTypesServiceModel workerTypesServiceModel = DelegateService.companyUniquePersonParamService.GetWorkertype(); 
            ExcelFileServiceModel file = DelegateService.companyUniquePersonParamService.GenerateFileToWorkerType(workerTypesServiceModel.WorkerTypeServiceModel, App_GlobalResources.Language.FileNameWorkerType);
            if (workerTypesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
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