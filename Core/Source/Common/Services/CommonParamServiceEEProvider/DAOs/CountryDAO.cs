// -----------------------------------------------------------------------
// <copyright file="CountryDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>

namespace Sistran.Core.Application.CommonParamService.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;  
    using CommonService.Models;
    using Framework.DAF;  
    using Framework.Queries;   
    using Utilities.DataFacade;
    using COMMEN = Common.Entities;
    using ENUMUT = Utilities.Enums;   
    using UTMO = Utilities.Error;

    /// <summary>
    /// Acceso a datos 
    /// </summary>
    public class CountryDAO
    {

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
        /// <returns>lista de estados</returns>
        public UTMO.Result<List<City>, UTMO.ErrorModel> GetCitiesByStateCountry(int idState, int idCountry)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name).Equal().Constant(idCountry);
                filter.And().Property(COMMEN.City.Properties.StateCode, typeof(COMMEN.City).Name).Equal().Constant(idState);

                List<COMMEN.City> entityCities = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City), filter.GetPredicate())).Cast<COMMEN.City>().ToList();
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
    }
}
