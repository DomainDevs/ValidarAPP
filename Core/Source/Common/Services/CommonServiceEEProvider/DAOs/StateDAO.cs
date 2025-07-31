using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class StateDAO
    {
        /// <summary>
        /// Obtener departamentos por pais
        /// </summary>
        /// <param name="country">pais</param>
        /// <returns></returns>
        public List<COMMML.State> GetStatesByCountry(COMMML.Country country)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.State.Properties.CountryCode, typeof(COMMEN.State).Name);
            filter.Equal();
            filter.Constant(country.Id);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.State), filter.GetPredicate()));
            List<COMMML.State> states = ModelAssembler.CreateStates(businessCollection);

            CityDAO cityProvider = new CityDAO();
            List<COMMML.City> cities = cityProvider.GetCitiesByCountry(country);

            foreach (COMMML.State item in states)
            {
                item.Country = country;
                item.Cities = cities.Where(x => x.State.Country.Id == item.Country.Id && x.State.Id == item.Id).ToList();
            }

            return states;
        }

        /// <summary>
        /// Obtener departamento por ciudad
        /// </summary>
        /// <param name="city">ciudad</param>
        /// <returns></returns>
        public COMMML.State GetStateByCity(COMMML.City city)
        {
            PrimaryKey key = COMMEN.State.CreatePrimaryKey(city.State.Country.Id, city.State.Id);
            COMMEN.State state = (COMMEN.State)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            COMMML.State stateModel = ModelAssembler.CreateState(state);

            CountryDAO countryProvider = new CountryDAO();
            stateModel.Country = countryProvider.GetCountryByState(stateModel);

            return stateModel;
        }

        /// <summary>
        /// Finds the specified country code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="stateCode">The state code.</param>
        /// <returns></returns>
        public static COMMEN.State Find(int countryCode, int stateCode)
        {
            PrimaryKey key = COMMEN.State.CreatePrimaryKey(countryCode, stateCode);
            COMMEN.State state = (COMMEN.State)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return state;
        }

        /// <summary>
        /// Obtener Departamentos
        /// </summary>
        /// <returns></returns>
        public List<COMMML.State> GetStates()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.State)));
            return ModelAssembler.CreateStates(businessCollection);
        }

        /// <summary>
        /// Obtener la lista de estados asociados al identificador del país
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <returns>Listado de estados</returns>
        public List<COMMML.State> GetStatesByCountryId(int countryId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.State.Properties.CountryCode, typeof(COMMEN.State).Name);
            filter.Equal();
            filter.Constant(countryId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.State), filter.GetPredicate()));
            List<COMMML.State> states = ModelAssembler.CreateStates(businessCollection);
            return states;
        }
    }
}
