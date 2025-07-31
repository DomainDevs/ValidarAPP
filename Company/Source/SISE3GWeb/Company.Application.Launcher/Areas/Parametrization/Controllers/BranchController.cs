
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Application.ModelServices.Models.CommonParam;
using Sistran.Company.Application.ModelServices.Models;  //pr
using Sistran.Company.Application.ModelServices.Models.Param; //pr
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using MODSER = Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Application.UnderwritingServices.Models;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;    

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    /// <summary>
    /// Controlador de objetos del seguro
    /// </summary>
    public class BranchController : Controller
    {
        /// <summary>
        /// Modelo sucursales
        /// </summary>
        private List<BranchViewModel> branch = new List<BranchViewModel>();            

        /// <summary>
        /// Carga la vista principal
        /// </summary>
        /// <returns>Action result</returns> 
        public ActionResult Branch()
        {
            return this.View();
        }

        /// <summary>
        /// Carga vista emergente
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult BranchAdvancedSearch()
        {
            return this.View();
        }


        /// <summary>
        /// Obtiene las sucrsales
        /// </summary>           
        /// <returns>lista de sucursales</returns>
        [HttpPost]
        public JsonResult GetBranches()
        {
            try
            {
      
                 BranchesServicesModel branches = DelegateService.companyCommonParamService.GetBranches();

                if (branches.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, branches.BranchServiceModel);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorQueryingCities);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorQueryingCities);
            }
        }

        /// <summary>
        /// lista las sucursales por descripción
        /// </summary>
        /// <param name="description"> descripcion de sucursales</param>
        /// <returns>lista de sucursales</returns>
        [HttpPost]
        public JsonResult GetCoBranchesByDescription(string description)
        {
            try
            {
                BranchesServicesModel branches = DelegateService.companyCommonParamService.GetCoBranchesByDescription(description);
                if (branches.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, branches.BranchServiceModel);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorQueryingCities);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorQueryingCities);
            }
        }

        /// <summary>
        /// Lista los sucursales
        /// </summary>  
        /// <param name="description">descripcion de sucursales</param>
        /// <returns> retorna Lista de sucursales </returns>
        [HttpPost]
        public ActionResult GetBranchByDescription(string description)
        {
            try
            {
                this.GetListBranch(description);
                return new UifJsonResult(true, this.branch.OrderBy(x => x.LongDescription).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
        }
		        

        [HttpPost]

        /// <summary>
        /// CRUD sucursales
        /// </summary>  
        /// <param name="branch">modelo de sucursales</param>
        /// <returns> retorna mensaje de sucursales </returns>
        public ActionResult SaveBranches(List<CoBranchViewModel> branch)
        {
            try
            {               
                List<BranchServiceModel> branchServiceModel = new List<BranchServiceModel>();
                branchServiceModel = ModelAssembler.CreateCoBranches(branch);

                List<BranchServiceModel> branchServiceModels = DelegateService.companyCommonParamService.ExecuteOperationsBranchServiceModel(branchServiceModel);
                MODSER.ParametrizationResult parametrizationResult = new MODSER.ParametrizationResult();

                foreach (var item in branchServiceModels)
                {
                    if (item.ErrorServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.Ok)
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
                            case ENUMSM.StatusTypeService.Create:
                                parametrizationResult.TotalAdded++;
                                break;
                            case ENUMSM.StatusTypeService.Update:
                                parametrizationResult.TotalModified++;
                                break;
                            case ENUMSM.StatusTypeService.Delete:
                                parametrizationResult.TotalDeleted++;
                                break;
                            default:
                                break;
                        }
                    }
                }

                return new UifJsonResult(true, parametrizationResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSave);
            }
        }

        /// <summary>
        /// Genera archivo excel de sucursales
        /// </summary>
        /// <returns>Archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                string descripcion = null;
                this.GetListBranch(descripcion);
                MODSER.ExcelFileServiceModel excelFileServiceModel = DelegateService.companyCommonParamService.GenerateFileToBranch(ModelAssembler.CreateBranchesServiceModel(this.branch), App_GlobalResources.Language.LabelBranch);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorFileNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// Obtiene los tipos de teléfono
        /// </summary>
        /// <returns>Lista tipos de teléfono</returns>
        [HttpGet]
        public JsonResult GetPhoneType()
        {
            try
            {
                PhonesTypesServiceQueryModel phoneType = DelegateService.companyCommonParamService.GetPhoneType();
                if (phoneType.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, phoneType.PhonesTypes);
                }

                return new UifJsonResult(false, Language.ErrorQueryingCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorQueryingCountries);
            }
        }

        /// <summary>
        /// Obtiene los tipos de dirección
        /// </summary>
        /// <returns>Lista tipos de dirección</returns>
        [HttpGet]
        public JsonResult GetAddressType()
        {
            try
            {
                AddressTypesServiceQueryModel address = DelegateService.companyCommonParamService.GetAddressType();
                if (address.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, address.AddressTypesService);
                }

                return new UifJsonResult(false, Language.ErrorQueryingCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorQueryingCountries);
            }
        }

        /// <summary>
        /// Obtiene los paises
        /// </summary>
        /// <returns>Lista de paises</returns>
        [HttpPost]
        public JsonResult GetCountries()
        {
            try
            {
                CountriesServiceQueryModel countries = DelegateService.companyCommonParamService.GetCountries();
                if (countries.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, countries.Counties);
                }

                return new UifJsonResult(false, Language.ErrorQueryingCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorQueryingCountries);
            }
        }

        /// <summary>
        /// Obtiene los estados por pais
        /// </summary>
        /// <param name="countryId">identificador del pais</param>
        /// <returns>lista de estados</returns>
        [HttpPost]
        public JsonResult GetStatesByCountry(int countryId)
        {
            try
            {
                StatesServiceQueryModel states = DelegateService.companyCommonParamService.GetStatesByCountry(countryId);
                if (states.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, states.States);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorConsultingDepartments);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorConsultingDepartments);
            }
        }

        /// <summary>
        /// Obtiene las cuidades por pais y estado
        /// </summary>
        /// <param name="countryId">identificador del pais</param>
        /// <param name="stateId">Identificador del estado</param>
        /// <returns>lista de estados</returns>
        [HttpPost]
        public JsonResult GetCitiesByCountryState(int countryId, int stateId)
        {
            try
            {
                CitiesServiceRelationModel cities = DelegateService.companyCommonParamService.GetCitiesByStateCountry(stateId, countryId);
                if (cities.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, cities.Cities);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorQueryingCities);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorQueryingCities);
            }
        }

        /// <summary>
        /// obtiene lista de sucursales
        /// </summary>
        /// <param name="description">descripcion de sucursales</param>
        /// <returns> retorna la vista de sucursales </returns>
        private List<BranchViewModel> GetListBranch(string description)
        {
            if (this.branch.Count == 0)
            {
                BranchesServiceQueryModel branchServiceModel = DelegateService.companyCommonParamService.GetBranchesByDescription(description);

                this.branch = ModelAssembler.CreateBranchesViewModel(branchServiceModel.BranchServiceQueryModel);
                return this.branch.OrderBy(x => x.LongDescription).ToList();
            }

            return this.branch;
        }
    }
}