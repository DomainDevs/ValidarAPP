using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class CityDAO
    {
        /// <summary>
        /// Obtener ciudades por pais
        /// </summary>
        /// <param name="country">pais</param>
        /// <returns></returns>
        public List<COMMML.City> GetCitiesByCountry(COMMML.Country country)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name);
            filter.Equal();
            filter.Constant(country.Id);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City), filter.GetPredicate()));
            return ModelAssembler.CreateCities(businessCollection);
        }

        /// <summary>
        /// Obtener ciudades por departamento
        /// </summary>
        /// <param name="state">estado</param>
        /// <returns></returns>
        public List<COMMML.City> GetCitiesByState(COMMML.State state)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name);
            filter.Equal();
            filter.Constant(state.Country.Id);
            filter.And();
            filter.Property(COMMEN.City.Properties.StateCode, typeof(COMMEN.City).Name);
            filter.Equal();
            filter.Constant(state.Id);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City), filter.GetPredicate()));
            List<COMMML.City> cities = ModelAssembler.CreateCities(businessCollection);

            foreach (COMMML.City item in cities)
            {
                item.State = state;
            }

            return cities;
        }

        /// <summary>
        /// Obtener ciudad por id ciudad
        /// </summary>
        /// <param name="city">ciudad</param>
        /// <returns></returns>
        public COMMML.City GetCityByCity(COMMML.City city)
        {
            PrimaryKey key = COMMEN.City.CreatePrimaryKey(city.State.Country.Id, city.Id, city.State.Id);
            COMMEN.City cityEntity = cityEntity = (COMMEN.City)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);


            city = ModelAssembler.CreateCity(cityEntity);

            StateDAO stateProvider = new StateDAO();
            city.State = stateProvider.GetStateByCity(city);

            return city;
        }

        /// <summary>
        /// Finds the specified country code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="cityCode">The city code.</param>
        /// <param name="stateCode">The state code.</param>
        /// <returns></returns>
        public static COMMEN.City Find(int countryCode, int cityCode, int stateCode)
        {
            PrimaryKey key = COMMEN.City.CreatePrimaryKey(countryCode, cityCode, stateCode);
            return (COMMEN.City)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Obtener Ciudades
        /// </summary>
        /// <returns></returns>
        public List<COMMML.City> GetCities()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City)));
            return ModelAssembler.CreateCities(businessCollection);
        }

        /// <summary>
        /// Obtener un listado de ciudades a partir del país y el estado
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <param name="stateId">Identificador del estado (departamento)</param>
        /// <returns>Listado de ciudades</returns>
        public List<COMMML.City> GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
      
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name);
            filter.Equal();
            filter.Constant(countryId);
            filter.And();
            filter.Property(COMMEN.City.Properties.StateCode, typeof(COMMEN.City).Name);
            filter.Equal();
            filter.Constant(stateId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City), filter.GetPredicate()));
            List<COMMML.City> cities = ModelAssembler.CreateCities(businessCollection);
            
            return cities;
        }
    }
}