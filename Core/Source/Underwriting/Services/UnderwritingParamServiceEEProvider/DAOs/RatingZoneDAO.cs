// -----------------------------------------------------------------------
// <copyright file="RatingZoneDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Assemblers;
    using CommonService.Models;
    using Framework.DAF;
    using Framework.DAF.Engine;
    using Framework.Queries;
    using Models;
    using CORENT = Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Utilities.DataFacade;
    using COMMEN = Common.Entities;
    using ENUMUT = Utilities.Enums;
    using UPENTV = Entities.Views;
    using UTMO = Utilities.Error;

    /// <summary>
    /// Acceso a datos para RatingZone
    /// </summary>
    public class RatingZoneDAO
    {
        /// <summary>
        /// Obtiene las zonas de tarifacion por el filtro
        /// </summary>
        /// <param name="ratingZoneCode">codigo de la zona de tarifacion</param>
        /// <param name="prefixId">id del ramo</param>
        /// <param name="filter">descripcion a buscar</param>
        /// <returns>Zonas de tarifacion MOD-B</returns>
        public UTMO.Result<List<ParamRatingZoneCity>, UTMO.ErrorModel> GetRatingZoneServiceModel(int? ratingZoneCode, int? prefixId, string filter)
        {
            try
            {
                List<ParamRatingZoneCity> paramRatingZoneCities = new List<ParamRatingZoneCity>();

                UPENTV.RatingZoneView view = new UPENTV.RatingZoneView();
                ViewBuilder builder = new ViewBuilder("RatingZoneView");
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(COMMEN.RatingZone.Properties.RatingZoneCode, typeof(COMMEN.RatingZone).Name).IsNotNull();

                if (ratingZoneCode.HasValue)
                {
                    where.And().Property(COMMEN.RatingZone.Properties.RatingZoneCode, typeof(COMMEN.RatingZone).Name).Equal().Constant(ratingZoneCode);
                }

                if (prefixId.HasValue)
                {
                    where.And().Property(COMMEN.RatingZone.Properties.PrefixCode, typeof(COMMEN.RatingZone).Name).Equal().Constant(prefixId);
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    where.And().Property(COMMEN.RatingZone.Properties.Description, typeof(COMMEN.RatingZone).Name).Like().Constant("%" + filter + "%");
                }

                builder.Filter = where.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                foreach (COMMEN.RatingZone ratingZone in view.RatingZone.Cast<COMMEN.RatingZone>())
                {
                    COMMEN.Prefix prefix = view.Prefix.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == ratingZone.PrefixCode);

                    ParamRatingZoneCity paramRatingZoneCity = new ParamRatingZoneCity
                    {
                        RatingZone = new ParamRatingZone
                        {
                            Id = ratingZone.RatingZoneCode,
                            Description = ratingZone.Description,
                            SmallDescription = ratingZone.SmallDescription,
                            IsDefault = ratingZone.IsDefault,
                            Prefix = new Prefix
                            {
                                Id = prefix.PrefixCode,
                                Description = prefix.Description
                            }
                        },
                        Cities = new List<City>()
                    };

                    foreach (COMMEN.CountryRatingZone countryRatingZone in view.CountryRatingZone.Cast<COMMEN.CountryRatingZone>().Where(x => x.PrefixCode == ratingZone.PrefixCode && x.RatingZoneCode == ratingZone.RatingZoneCode))
                    {

                        COMMEN.City city = view.City.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == countryRatingZone.CityCode && x.StateCode == countryRatingZone.StateCode && x.CountryCode == countryRatingZone.CountryCode);
                        COMMEN.State state = view.State.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == countryRatingZone.StateCode && x.CountryCode == countryRatingZone.CountryCode);
                        COMMEN.Country country = view.Country.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == countryRatingZone.CountryCode);

                        paramRatingZoneCity.Cities.Add(new City
                        {
                            Id = city?.CityCode ?? 0,
                            Description = city?.Description,
                            State = new State
                            {
                                Id = state?.StateCode ?? 0,
                                Description = state?.Description,
                                Country = new Country
                                {
                                    Id = country?.CountryCode ?? 0,
                                    Description = country?.Description ?? null
                                }
                            }
                        });
                    }

                    paramRatingZoneCities.Add(paramRatingZoneCity);
                }

                return new UTMO.ResultValue<List<ParamRatingZoneCity>, UTMO.ErrorModel>(paramRatingZoneCities.OrderBy(x => x.RatingZone.Description).ToList());
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetRatingZone);
                return new UTMO.ResultError<List<ParamRatingZoneCity>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene los paises
        /// </summary>
        /// <returns>Lista de paises</returns>
        public UTMO.Result<List<Country>, UTMO.ErrorModel> GetCountries()
        {
            try
            {
                List<COMMEN.Country> entityCountries = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Country))).Cast<COMMEN.Country>().ToList();
                List<Country> countries = entityCountries.OrderBy(x => x.Description).Select(x => new Country
                {
                    Id = x.CountryCode,
                    Description = x.Description
                }).ToList();

                return new UTMO.ResultValue<List<Country>, UTMO.ErrorModel>(countries);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetCountries);
                return new UTMO.ResultError<List<Country>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene los estados por pais
        /// </summary>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        public UTMO.Result<List<State>, UTMO.ErrorModel> GetStatesByCountry(int idCountry)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.State.Properties.CountryCode, typeof(COMMEN.State).Name).Equal().Constant(idCountry);
                List<COMMEN.State> entityStates = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.State), filter.GetPredicate())).Cast<COMMEN.State>().ToList();
                List<State> states = entityStates.OrderBy(x => x.Description).Select(x => new State
                {
                    Id = x.StateCode,
                    Description = x.Description,
                    Country = new Country
                    {
                        Id = x.CountryCode
                    }
                }).ToList();

                return new UTMO.ResultValue<List<State>, UTMO.ErrorModel>(states);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetStates);
                return new UTMO.ResultError<List<State>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene las cuidades por pais y estado
        /// </summary>
        /// <param name="idState">Identificador del estado</param>
        /// <param name="idCountry">identificador del pais</param>
        /// <param name="PrefixCode">identificador de la rama </param>
        /// <returns>lista de estados</returns>
        public UTMO.Result<List<City>, UTMO.ErrorModel> GetCitiesByStateCountry(int idState, int idCountry, int PrefixCode)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(COMMEN.CountryRatingZone.Properties.PrefixCode, typeof(COMMEN.CountryRatingZone).Name, PrefixCode);
                filter.And();
                filter.PropertyEquals(COMMEN.CountryRatingZone.Properties.CountryCode, typeof(COMMEN.CountryRatingZone).Name, idCountry);
                filter.And();
                filter.PropertyEquals(COMMEN.CountryRatingZone.Properties.StateCode, typeof(COMMEN.CountryRatingZone).Name, idState);

                List<COMMEN.CountryRatingZone> entityratingZones = new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(
                        typeof(COMMEN.CountryRatingZone
                        ), filter.GetPredicate()
                        )
                    ).Cast<COMMEN.CountryRatingZone>().Where(x => x.CityCode != null).ToList();
               
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name, idCountry);
                filter.And();
                filter.PropertyEquals(COMMEN.City.Properties.StateCode, typeof(COMMEN.City).Name, idState);


                if (entityratingZones.Count() > 0)
                {
                    filter.And();
                    filter.Not();
                    filter.Property(COMMEN.City.Properties.CityCode, typeof(COMMEN.City).Name).In().ListValue();

                    foreach (COMMEN.CountryRatingZone entityratingZone in entityratingZones)
                    {
                        filter.Constant(entityratingZone.CityCode);
                    }
                    filter.EndList();
                }

                List<COMMEN.City> entityCities;
                if (entityratingZones.Count == 0)
                {
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name, idCountry);
                    filter.And();
                    filter.PropertyEquals(COMMEN.City.Properties.StateCode, typeof(COMMEN.City).Name, idState);
                    entityCities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City), filter.GetPredicate())).Cast<COMMEN.City>().ToList();
                }
                else
                {
                    entityCities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City), filter.GetPredicate())).Cast<COMMEN.City>().ToList();
                }
                List<City> states = entityCities.OrderBy(x => x.Description).Select(x => new City
                {
                    Id = x.CityCode,
                    Description = x.Description,
                    State = new State
                    {
                        Id = x.StateCode,
                        Country = new Country
                        {
                            Id = x.CountryCode
                        }
                    }
                }).ToList();

                return new UTMO.ResultValue<List<City>, UTMO.ErrorModel>(states);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetCities);
                return new UTMO.ResultError<List<City>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Realiza el guardado de una zona de tarifacion
        /// </summary>
        /// <param name="paramRatingZoneCity">zona de tarifacion a crear</param>
        /// <returns>Zonas de tarifacion reultado del CRUD</returns>
        public UTMO.Result<ParamRatingZoneCity, UTMO.ErrorModel> CreateParamRatingZoneCity(ParamRatingZoneCity paramRatingZoneCity)
        {
            try
            {
                int ratingZoneCode = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.RatingZone))).Cast<COMMEN.RatingZone>().Max(x => x.RatingZoneCode);

                COMMEN.RatingZone ratingZone = EntityAssembler.CreateRatingZone(paramRatingZoneCity.RatingZone);
                List<COMMEN.City> cities = EntityAssembler.CreateCities(paramRatingZoneCity.Cities);

                ratingZone.RatingZoneCode = ratingZoneCode + 1;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(ratingZone);
                foreach (COMMEN.City city in cities)
                {
                    COMMEN.CountryRatingZone countryRatingZone = new COMMEN.CountryRatingZone
                    {
                        RatingZoneCode = ratingZone.RatingZoneCode,
                        PrefixCode = ratingZone.PrefixCode,
                        CountryCode = city.CountryCode,
                        StateCode = city.StateCode,
                        CityCode = city.CityCode
                    };

                    DataFacadeManager.Instance.GetDataFacade().InsertObject(countryRatingZone);
                }

                paramRatingZoneCity.RatingZone.Id = ratingZone.RatingZoneCode;

                return new UTMO.ResultValue<ParamRatingZoneCity, UTMO.ErrorModel>(paramRatingZoneCity);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorCreateParamRatingZone);
                return new UTMO.ResultError<ParamRatingZoneCity, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Realiza la actualizacion de una zona de tarifacion
        /// </summary>
        /// <param name="paramRatingZoneCity">zona de tarifacion a editar</param>
        /// <returns>resultado del CRUD</returns>
        public UTMO.Result<ParamRatingZoneCity, UTMO.ErrorModel> UpdateParamRatingZoneCity(ParamRatingZoneCity paramRatingZoneCity)
        {
            try
            {
                List<COMMEN.City> cities = EntityAssembler.CreateCities(paramRatingZoneCity.Cities);

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.RatingZone.Properties.RatingZoneCode, typeof(COMMEN.RatingZone).Name).Equal().Constant(paramRatingZoneCity.RatingZone.Id);
                COMMEN.RatingZone ratingZone = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.RatingZone), filter.GetPredicate())).Cast<COMMEN.RatingZone>().ToList().First();

                ratingZone.Description = paramRatingZoneCity.RatingZone.Description;
                ratingZone.SmallDescription = paramRatingZoneCity.RatingZone.SmallDescription;
                ratingZone.PrefixCode = paramRatingZoneCity.RatingZone.Prefix.Id;
                ratingZone.IsDefault = paramRatingZoneCity.RatingZone.IsDefault;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ratingZone);

                filter.Clear();
                filter.Property(COMMEN.CountryRatingZone.Properties.RatingZoneCode).Equal().Constant(paramRatingZoneCity.RatingZone.Id);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.CountryRatingZone), filter.GetPredicate());

                foreach (COMMEN.City city in cities)
                {
                    COMMEN.CountryRatingZone countryRatingZone = new COMMEN.CountryRatingZone
                    {
                        RatingZoneCode = ratingZone.RatingZoneCode,
                        PrefixCode = ratingZone.PrefixCode,
                        CountryCode = city.CountryCode,
                        StateCode = city.StateCode,
                        CityCode = city.CityCode
                    };

                    DataFacadeManager.Instance.GetDataFacade().InsertObject(countryRatingZone);
                }

                paramRatingZoneCity.RatingZone.Id = ratingZone.RatingZoneCode;

                return new UTMO.ResultValue<ParamRatingZoneCity, UTMO.ErrorModel>(paramRatingZoneCity);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorUpdateParamRatingZone);
                return new UTMO.ResultError<ParamRatingZoneCity, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Realiza el guardado de una zona de tarifacion
        /// </summary>
        /// <param name="paramRatingZoneCity">zona de tarifacion a eliminar</param>
        /// <returns>resultado del CRUD</returns>
        public UTMO.Result<ParamRatingZoneCity, UTMO.ErrorModel> DeleteParamRatingZoneCity(ParamRatingZoneCity paramRatingZoneCity)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Clear();
                filter.Property(COMMEN.CountryRatingZone.Properties.RatingZoneCode).Equal().Constant(paramRatingZoneCity.RatingZone.Id);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.CountryRatingZone), filter.GetPredicate());

                filter.Clear();
                filter.Property(COMMEN.RatingZone.Properties.RatingZoneCode, typeof(COMMEN.RatingZone).Name).Equal().Constant(paramRatingZoneCity.RatingZone.Id);
                COMMEN.RatingZone ratingZone = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.RatingZone), filter.GetPredicate())).Cast<COMMEN.RatingZone>().ToList().First();
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ratingZone);

                return new UTMO.ResultValue<ParamRatingZoneCity, UTMO.ErrorModel>(paramRatingZoneCity);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorDeleteParamRatingZone);
                return new UTMO.ResultError<ParamRatingZoneCity, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Generar archivo excel de planes de pago
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns> Archivo de excel</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToRatingZone(string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileDAO fileDao = new FileDAO();
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationServiceRatingZone
                };

                UTMO.Result<List<ParamRatingZoneCity>, UTMO.ErrorModel> ratingZoneServiceModel = this.GetRatingZoneServiceModel(null, null, string.Empty);
                List<ParamRatingZoneCity> paramRatingZoneCities = new List<ParamRatingZoneCity>();

                if (ratingZoneServiceModel is UTMO.ResultError<List<ParamRatingZoneCity>, UTMO.ErrorModel>)
                {
                    listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));                    
                }
                else if (ratingZoneServiceModel is UTMO.ResultValue<List<ParamRatingZoneCity>, UTMO.ErrorModel>)
                {
                    paramRatingZoneCities = (ratingZoneServiceModel as UTMO.ResultValue<List<ParamRatingZoneCity>, UTMO.ErrorModel>).Value;
                }

                File file = fileDao.GetFileByFileProcessValue(fileProcessValue);
                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamRatingZoneCity item in paramRatingZoneCities)
                    {
                        if (item.Cities.Count == 0)
                        {
                            List<Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                            {
                                ColumnSpan = x.ColumnSpan,
                                Description = x.Description,
                                FieldType = x.FieldType,
                                Id = x.Id,
                                IsEnabled = x.IsEnabled,
                                IsMandatory = x.IsMandatory,
                                Order = x.Order,
                                RowPosition = x.RowPosition,
                                SmallDescription = x.SmallDescription
                            }).ToList();

                            fields[0].Value = item.RatingZone.Prefix.Description;
                            fields[1].Value = item.RatingZone.Id.ToString();
                            fields[2].Value = item.RatingZone.Description;

                            rows.Add(new Row
                            {
                                Fields = fields
                            });
                        }
                        else {
                            foreach (City city in item.Cities.OrderBy(x => x.Description))
                            {
                                List<Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                                {
                                    ColumnSpan = x.ColumnSpan,
                                    Description = x.Description,
                                    FieldType = x.FieldType,
                                    Id = x.Id,
                                    IsEnabled = x.IsEnabled,
                                    IsMandatory = x.IsMandatory,
                                    Order = x.Order,
                                    RowPosition = x.RowPosition,
                                    SmallDescription = x.SmallDescription
                                }).ToList();

                                fields[0].Value = item.RatingZone.Prefix.Description;
                                fields[1].Value = item.RatingZone.Id.ToString();
                                fields[2].Value = item.RatingZone.Description;
                                fields[3].Value = city.State.Country.Description;
                                fields[4].Value = city.State.Description;
                                fields[5].Value = city.Description;

                                rows.Add(new Row
                                {
                                    Fields = fields
                                });
                            }
                        }
                       
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string result = fileDao.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new ArgumentException(Resources.Errors.ErrorDownloadingExcel)));
            }
            catch (Exception ex)
            {                
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        internal List<ParamRatingZone> GetRatingZonesByPrefixIdAndBranchId(int prefixId, int branchId)
        {
            List<ParamRatingZone> companyRatingZones = new List<ParamRatingZone>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CORENT.RatingZone.Properties.PrefixCode, typeof(CORENT.RatingZone).Name, prefixId);
            filter.And();
            filter.PropertyEquals(CORENT.CiaRatingZoneBranch.Properties.BranchCode, typeof(CORENT.CiaRatingZoneBranch).Name, branchId);
            
            Entities.Views.CiaRatingZoneBranchView ciaRatingZoneBranchView = new Entities.Views.CiaRatingZoneBranchView();
            ViewBuilder viewBuilder = new ViewBuilder("CiaRatingZoneBranchView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, ciaRatingZoneBranchView);

            if (ciaRatingZoneBranchView.RatingZones.Count > 0)
            {
                List<CORENT.RatingZone> entityRatingZone = ciaRatingZoneBranchView.RatingZones.Cast<CORENT.RatingZone>().ToList();
                List<CORENT.CiaRatingZoneBranch> entityCiaRatingZoneBranche = ciaRatingZoneBranchView.CiaRatingZoneBranchs.Cast<CORENT.CiaRatingZoneBranch>().ToList();

                entityRatingZone = entityRatingZone.Where(x => entityCiaRatingZoneBranche.Any(y => y.RatingZoneCode == x.RatingZoneCode)).ToList();

                companyRatingZones = ModelsServicesAssembler.CreateRatingZoneEntitys(entityRatingZone);
            }
            return companyRatingZones;
        }

        internal ParamRatingZone CreateRatingZones(ParamRatingZone paramRatingZones)
        {
            
            CORENT.RatingZone ModelToentity = EntityAssembler.CreateRatingZone(paramRatingZones);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(ModelToentity);
            ParamRatingZone companyRatingZones = ModelsServicesAssembler.CreateRatingZoneEntity(ModelToentity);
            return companyRatingZones;
        }

        internal ParamRatingZone UpdateRatingZones(ParamRatingZone paramRatingZones)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CORENT.RatingZone.Properties.RatingZoneCode, typeof(CORENT.RatingZone).Name).Equal().Constant(paramRatingZones.Id);
            CORENT.RatingZone ratingZone = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CORENT.RatingZone), filter.GetPredicate())).Cast<CORENT.RatingZone>().ToList().First();
            ratingZone.Description = paramRatingZones.Description;
            ratingZone.SmallDescription = paramRatingZones.SmallDescription;
            ratingZone.PrefixCode = paramRatingZones.Prefix.Id;
            ratingZone.IsDefault = paramRatingZones.IsDefault;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(ratingZone);
            ParamRatingZone companyRatingZones = ModelsServicesAssembler.CreateRatingZoneEntity(ratingZone);
            return companyRatingZones;
        }

        internal bool DeleteRatingZones(ParamRatingZone paramRatingZones)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Clear();
                filter.Property(CORENT.CountryRatingZone.Properties.RatingZoneCode).Equal().Constant(paramRatingZones.Id);
                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(CORENT.CountryRatingZone), filter.GetPredicate());

                filter.Clear();
                filter.Property(CORENT.RatingZone.Properties.RatingZoneCode, typeof(CORENT.RatingZone).Name).Equal().Constant(paramRatingZones.Id);
                CORENT.RatingZone ratingZone = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CORENT.RatingZone), filter.GetPredicate())).Cast<CORENT.RatingZone>().ToList().First();
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ratingZone);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}