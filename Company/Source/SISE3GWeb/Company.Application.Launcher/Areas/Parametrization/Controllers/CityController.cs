
using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Company.Application.Utilities.DTO;
using Sistran.Company.Application.Utilities.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class CityController : Controller
    {
        private Helpers.PostEntity entityCountry = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Country" };
        private Helpers.PostEntity entityState = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.State" };
        //carga generica de paises
        private List<GenericViewModel> countries = new List<GenericViewModel>();
        private static List<Country> countriesCommon = new List<Country>();

        //carga generica de departamentos
        private List<GenericViewModel> states = new List<GenericViewModel>();
        private static List<State> statesCommon = new List<State>();
                      

        // GET: Parametrization/City
     
        #region views
        public ActionResult City()
        {
            return View();
        }
        public ActionResult SearchAdvCity()
        {
            return PartialView();
        }

       
        #endregion
        
        #region carga inicial vista
         /// <summary>
        /// GetCountries: Carga listado de todas los paises con combo generico, tabla: country
        /// </summary>        
        public ActionResult GetCountries()
        {
            try
            {
                this.GetListCountries();
                return new UifJsonResult(true, this.countries);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCountries);
            }
        }
        
        /// <summary>
        /// GetListCountries: Carga el listado de paises: table comm.Country con combos genericos
        /// </summary>
        public void GetListCountries()
        {
            if (this.countries.Count == 0)
            {
                this.countries = ModelAssembler.CreateCities(ModelAssembler.DynamicToDictionaryList(this.entityCountry.CRUDCliente.Find(this.entityCountry.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

         /// <summary>
        /// Carga listado de todas los departamentos, combo
        /// </summary>
        public ActionResult GetStates()
        {
            try
            {
                this.GetListStates();
                return new UifJsonResult(true, states);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStates);
            }
        }

          /// <summary>
        /// GetListStates:  Carga el listado de paises: table comm.State
        /// </summary>
        public void GetListStates()
        {
            if (states.Count == 0)
            {
                states = ModelAssembler.CreateStates(ModelAssembler.DynamicToDictionaryList(this.entityState.CRUDCliente.Find(this.entityState.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
            }
        }
               

        /// <summary>
        /// Carga listado de ciudades, toma los 50 primeros, listview
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCities()
        {
            CityQueryDTO cities = new CityQueryDTO();
            try
            {
                cities= DelegateService.parametrizationAplicationService.GetApplicationCity();
                return new UifJsonResult(true, cities.CityDTO.OrderBy(x => x.Description).Take(50).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }

        #endregion
    

        /// <summary>
        /// CreateCity: inserta el registro de una ciudad table Comm.City
        /// </summary>
        /// <param name="cityViewModel"></param>
        /// <returns></returns>
        public ActionResult CreateCity(CityViewModel cityViewModel)
        {
            CityDTO cityDTO = new CityDTO();
            cityDTO = ModelAssembler.MappCityVMApplication(cityViewModel);
            cityDTO=DelegateService.parametrizationAplicationService.CreateApplicationCity(cityDTO);
            cityViewModel.Id = cityDTO.Id;
         
            if (cityDTO.ErrorDTO.ErrorType == ErrorType.Ok)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.CitiesSaveSuccessfully);
            }
            else
            {
                return new UifJsonResult(true, App_GlobalResources.Language.CitiesSaveError);

            }
            
        }
              

        /// <summary>
        /// Obtiene el listado de los departamentos a partir del id del pais
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public ActionResult GetStatesByCountryId(int countryId)
        {
            try
             {
                if (states.Count == 0)
                {
                    states = ModelAssembler.CreateStates(ModelAssembler.DynamicToDictionaryList(this.entityState.CRUDCliente.Find(this.entityState.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
                }
                       
                if (states != null)
                {
                    var list = states.Where(c=>c.IdC==countryId).ToList();
                    return new UifJsonResult(true, list.OrderBy(x => x.DescriptionLong));
                }
                else
                {
                    return new UifJsonResult(false, new List<State>());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStates);
            }

        }


        /// <summary>
        /// Obtiene el listado de los departamentos a partir del id del pais
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public ActionResult GetCitiesByDescription(string cityDescription)
        {
            CityQueryDTO cities = new CityQueryDTO();
            try
            {
                cities = DelegateService.parametrizationAplicationService.GetApplicationCityByDescription(cityDescription);
                if(cities.CityDTO.Count==0)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorDataNotFound);
                }
                return new UifJsonResult(true, cities.CityDTO.OrderBy(x => x.Description).Take(50).ToList());
               
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }           

        }

       
        /// <summary>
        /// GetCityAdvancedSearch: Consulta listado de ciudades por busqueda avanzada
        /// </summary>
        /// <param name="cityViewModel"></param>
        /// <returns></returns>
        public ActionResult GetCityAdvancedSearch(CityViewModel cityViewModel)
        {            
            CityQueryDTO cities = new CityQueryDTO();
            CityDTO cityDTO = new CityDTO();
            cityDTO = ModelAssembler.MappCityVMApplication(cityViewModel);
            try
            {
                cities = DelegateService.parametrizationAplicationService.GetApplicationCityAdv(cityDTO);
                if(cities.CityDTO.Count==0)
                {
                     return new UifJsonResult(true, cities.CityDTO);
                }
                return new UifJsonResult(true, cities.CityDTO.OrderBy(x => x.Description).Take(50).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }

        /// <summary>
        /// ExportFileCities: Consulta listado de ciudades y genera el excel a descargar
        /// </summary>
        /// <returns></returns>
        public JsonResult ExportFileCities()
        {
            ExcelFileDTO Result = DelegateService.parametrizationAplicationService.GenerateFileToCity(App_GlobalResources.Language.FileNameCities);
            try
            {
                if (Result.ErrorType == ErrorType.Ok && !String.IsNullOrEmpty(Result.File))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.File);
                }
                
                return new UifJsonResult(false, string.Join(",", App_GlobalResources.Language.ErrorThereIsNoDataToExport));                
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportCity);
            } 
            
        }

        /// <summary>
        /// UpdateCity: actualiza la informacion de description y smalldescription de una ciudad
        /// </summary>
        /// <param name="cityViewModel"></param>
        /// <returns></returns>
        public ActionResult UpdateCity(CityViewModel cityViewModel)
        {
            CityDTO cityDTO = new CityDTO();
            try
            {
                cityDTO = ModelAssembler.MappCityVMApplication(cityViewModel);
                cityDTO=DelegateService.parametrizationAplicationService.UpdateApplicationCity(cityDTO);
                cityViewModel.Id = cityDTO.Id;         
                if (cityDTO.ErrorDTO.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CitiesSaveSuccessfully);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CitiesUpdateError);
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.CitiesUpdateError);
            }
            
            
        }

        /// <summary>
        /// DeleteCity: Elimina el registro de una ciudad, table comm.city
        /// </summary>
        /// <param name="cityViewModel"></param>
        /// <returns></returns>
        public ActionResult DeleteCity(CityViewModel cityViewModel)
        {
            CityDTO cityDTO = new CityDTO();
            try
            {
                cityDTO = ModelAssembler.MappCityVMApplication(cityViewModel);
                cityDTO=DelegateService.parametrizationAplicationService.DeleteApplicationCity(cityDTO);
                if(cityDTO.ErrorDTO.ErrorType.Equals(ErrorType.BusinessFault))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCityOccup);
                }
                return new UifJsonResult(true, App_GlobalResources.Language.MessageDeletedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.CitiesUpdateError);
            }
        }

    }
}