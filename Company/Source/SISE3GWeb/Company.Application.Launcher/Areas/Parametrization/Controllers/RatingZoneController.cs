// -----------------------------------------------------------------------
// <copyright file="RatingZoneController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Core.Application.Utilities.Cache;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Application.ModelServices.Models.Param;
    using Application.ModelServices.Models.Underwriting;
    using Application.ModelServices.Models.UnderwritingParam;
    using MODCO = Application.ModelServices.Models.CommonParam;
    using AutoMapper;
    using Models;
    using Services;
    using Web.Models;
    using WebGrease.Css.Extensions;
    using ENUMSM = Application.ModelServices.Enums;
    
    /// <summary>
    /// Controlador para la parametrizacion de zonas de tarifacion
    /// </summary>
    public class RatingZoneController : Controller
    {
        #region ViewResult
        /// <summary>
        /// Renderiza la pagina RatingZone/RatingZone
        /// </summary>
        /// <returns>Vista RatingZone/RatingZone</returns>
        public ViewResult RatingZone()
        {
            return this.View();
        }

        /// <summary>
        /// Renderiza la pagina RatingZone/AdvancedSearch
        /// </summary>
        /// <returns>Vista RatingZone/AdvancedSearch</returns>
        [HttpGet]
        public ViewResult AdvancedSearch()
        {
            return this.View();
        }
        #endregion

        #region JsonResult
        /// <summary>
        /// Obtiene las zonas de tarifacion por el filtro
        /// </summary>
        /// <param name="ratingZoneCode">codigo de la zona de tarifacion</param>
        /// <param name="filter">descripcion a buscar</param>
        /// <returns>Zonas de tarifacion MOD-B</returns>
        [HttpPost]
        public JsonResult GetRatingZonesByFilter(int? ratingZoneCode,int? prefixCode, string filter)
        {
            RatingZonesServiceModel ratingZoneServiceModel = DelegateService.UnderwritingParamServiceWeb.GetRatingZoneServiceModel(ratingZoneCode, prefixCode, filter);
            if (ratingZoneServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                List<RatingZoneViewModel> ratingZones = new List<RatingZoneViewModel>();
                var config = MapperCache.GetMapper<RatingZoneServiceModel, RatingZoneViewModel>(cfg =>
                {
                    cfg.CreateMap<RatingZoneServiceModel, RatingZoneViewModel>()
                    .ForMember(x => x.PrefixCode, x => x.MapFrom(y => y.Prefix.PrefixCode))
                    .ForMember(x => x.PrefixDescription, x => x.MapFrom(y => y.Prefix.PrefixDescription));
                });
                foreach (RatingZoneServiceModel item in ratingZoneServiceModel.RatingZones)
                {
                    ratingZones.Add(config.Map<RatingZoneServiceModel, RatingZoneViewModel>(item));
                }

                return new UifJsonResult(true, ratingZones);
            }

            return new UifJsonResult(false, ratingZoneServiceModel.ErrorDescription);
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
                MODCO.CountriesServiceQueryModel countries = DelegateService.UnderwritingParamServiceWeb.GetCountries();
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
                MODCO.StatesServiceQueryModel states = DelegateService.UnderwritingParamServiceWeb.GetStatesByCountry(countryId);
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
        public JsonResult GetCitiesByCountryState(int countryId, int stateId, int PrefixCode)
        {
            try
            {
                MODCO.CitiesServiceRelationModel cities = DelegateService.UnderwritingParamServiceWeb.GetCitiesByStateCountry(stateId, countryId,PrefixCode);
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
        /// CRUD de zonas de tarifacion
        /// </summary>
        /// <param name="ratingZoneViewModels">zonas de tarifacion MOD-S</param>
        /// <returns>Listado de zonas de tarifacion de la operacion del CRUD</returns>
        [HttpPost]
        public JsonResult ExecuteOperationsParametrizationRatingZone(List<RatingZoneViewModel> ratingZoneViewModels)
        {
            try
            {
                RatingZonesServiceModel ratingZonesServiceModel = new RatingZonesServiceModel();
                ratingZonesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetRatingZoneServiceModel(null, null, null);
                List<RatingZoneServiceModel> ratingZones = new List<RatingZoneServiceModel>();
                var config = MapperCache.GetMapper<RatingZoneViewModel, RatingZoneServiceModel>(cfg =>
                {
                    cfg.CreateMap<RatingZoneViewModel, RatingZoneServiceModel>();
                });
                //string[] name = ratingZoneViewModels.Select(x =>  x.Description).ToArray();
                //List<RatingZoneServiceModel> totalModelsFilter = (from x in ratingZonesServiceModel.RatingZones where (from y in name select y).Contains(x.Description) select x).ToList();
                foreach (RatingZoneViewModel viewModel in ratingZoneViewModels)
                {
                //if (totalModelsFilter.Count == 0 || (totalModelsFilter.Count != 0 && viewModel.StatusTypeService != 2 ))
                //    {
                        RatingZoneServiceModel map = config.Map<RatingZoneViewModel, RatingZoneServiceModel>(viewModel);
                        map.Prefix = new PrefixServiceQueryModel
                        {
                            PrefixCode = viewModel.PrefixCode
                        };
                        ratingZones.Add(map);
                   //}

                }

                List<RatingZoneServiceModel> ratingZoneServiceModel = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsRatingZoneServiceModel(ratingZones);

                int totalAdded = ratingZoneServiceModel.Count(x => x.StatusTypeService == ENUMSM.StatusTypeService.Create);
                int totalModified = ratingZoneServiceModel.Count(x => x.StatusTypeService == ENUMSM.StatusTypeService.Update);
                int totalDeleted = ratingZoneServiceModel.Count(x => x.StatusTypeService == ENUMSM.StatusTypeService.Delete);

                StringBuilder errorMessage = new StringBuilder();
                ratingZoneServiceModel.Where(x => x.StatusTypeService == ENUMSM.StatusTypeService.Error).ForEach(x => x.ErrorServiceModel.ErrorDescription.ForEach(y=> errorMessage.Append("</br>" + y) ));

                

                return new UifJsonResult(true, new ParametrizationResult { TotalAdded = totalAdded, TotalDeleted = totalDeleted, TotalModified = totalModified, Message = errorMessage.ToString() });

                    
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Genera archivo excel de zonas de tarifacion
        /// </summary>
        /// <returns>Arhivo de excel zonas de tarifacion</returns>
        [HttpPost]
        public JsonResult GenerateFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToRatingZone(Language.LabelRateZone);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    string urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }

                return new UifJsonResult(false, Language.ErrorFileNotFound);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGeneratingFile);
            }
        }
        #endregion
    }
}