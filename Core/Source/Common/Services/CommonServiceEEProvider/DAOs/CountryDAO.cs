using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    /// <summary>
    /// Paises
    /// </summary>
    public class CountryDAO
    {
        /// <summary>
        /// Obtener paises Departamentos Ciudades
        /// </summary>
        /// <returns></returns>
        public List<COMMML.Country> GetCountries()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Country)));
            List<COMMML.Country> countries = ModelAssembler.CreateCountries(businessCollection);
            BusinessCollection businessCollectionStates = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.State)));
            List<COMMML.State> States = ModelAssembler.CreateStates(businessCollectionStates);
            BusinessCollection businessCollectionCity = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.City)));
            List<COMMML.City> cities = ModelAssembler.CreateCities(businessCollectionCity);
            TP.Parallel.ForEach(countries,
                x =>
                {
                    x.States = States.Where(y => y.Country.Id == x.Id).Select(m => m).ToList();
                    x.States.AsParallel().ForAll(m => m.Cities = cities.Where(y => y.State.Id == m.Id && y.State.Country.Id == m.Country.Id).Select(u => u).ToList());
                });
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetCountries");
            return countries;
        }

        /// <summary>
        /// Obtiene el listado de países sin ninguna relación
        /// </summary>
        /// <returns>Listado de países</returns>
        public List<COMMML.Country> GetCountriesLite()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Country)));
            List<COMMML.Country> countries = ModelAssembler.CreateCountries(businessCollection);
            return countries;
        }

        /// <summary>
        /// Obtener pais por departamento
        /// </summary>
        /// <param name="state">departamento</param>
        /// <returns></returns>
        public COMMML.Country GetCountryByState(COMMML.State state)
        {
            COMMEN.Country country = null;
            PrimaryKey key = COMMEN.Country.CreatePrimaryKey(state.Country.Id);
            country = (COMMEN.Country)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return ModelAssembler.CreateCountry(country);
        }

        /// <summary>
        /// Finds the specified country code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        public static COMMEN.Country Find(int countryCode)
        {
            PrimaryKey key = COMMEN.Country.CreatePrimaryKey(countryCode);
            COMMEN.Country country = (COMMEN.Country)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return country;
        }

        public COMMML.City GetCountryByCountryIdByStateIdByCityId(int countryId, int stateId, int cityId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = COMMEN.City.CreatePrimaryKey(countryId, cityId, stateId);
            var city = (COMMEN.City)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            COMMML.City cityModel = ModelAssembler.CreateCity(city);
            return cityModel;
        }
    }
}
