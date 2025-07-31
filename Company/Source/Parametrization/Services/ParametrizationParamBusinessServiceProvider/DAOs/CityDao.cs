using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Entities.Views;
using Sistran.Company.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs
{
    public class CityDao
    {
        /// <summary>
        /// Registra una ciudad
        /// </summary>
        /// <param name="companyParam"></param>
        /// <returns></returns>
        public CompanyParamCity CreateCity(CompanyParamCity companyParamCity)
        {
            try
            {
                int maxId = FindCity(companyParamCity.Country.Id, companyParamCity.State.Id);
                City entity = EntityAssembler.CreateParamCity(companyParamCity, maxId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                        daf.InsertObject(entity);
                }

                if (entity.CityCode != 0)
                {                    
                    companyParamCity.Id = entity.CityCode;
                    return companyParamCity;
                }
                else
                {
                    throw new BusinessException(Resources.Errors.ConsecutiveCity);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

           
        }
              
        /// <summary>
        /// Actualiza los campos de description y small description de la tabla comm.city
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public CompanyParamCity UpdateCity(CompanyParamCity companyParamCity)
        {
            try
            {
                PrimaryKey primaryKey = City.CreatePrimaryKey(companyParamCity.Country.Id, companyParamCity.Id, companyParamCity.State.Id);
                var cityEntity =(City) DataFacadeManager.GetObject(primaryKey);
                cityEntity.Description = companyParamCity.Description;
                cityEntity.SmallDescription = companyParamCity.SmallDescription;               
                DataFacadeManager.Update(cityEntity);
                return companyParamCity;
            }
             catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            
        }

        /// <summary>
        /// Obtiene el detalle de una ciudad a partir de los filtros de id , descripcion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public List<CompanyParamCity> GetByDescriptionCity(string descripcion)
        {           
            List<string> errorListDescription = new List<string>();
            try
            {
                List<CompanyParamCity> listCompanyParamCity = new List<CompanyParamCity>();
                CitiesView view = new CitiesView();
                ViewBuilder builder = new ViewBuilder("CitiesView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(City.Properties.Description, typeof(City).Name);
                filter.Like();
                filter.Constant("%"+descripcion+"%");

                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (City cities in view.City)
                {
                    Country country = view.Country.Cast<Country>().First(x => x.CountryCode == cities.CountryCode);
                    State state = view.State.Cast<State>().First(x => x.StateCode == cities.StateCode && x.CountryCode == cities.CountryCode);

                    CompanyParamCity resultCity = ModelAssembler.MappParamCity(cities, country, state);
                    listCompanyParamCity.Add(resultCity);
                }

               
                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina una ciudad a partir de el id del registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteCity(CompanyParamCity companyParamCity)
        {
           try
            {
                 PrimaryKey primaryKey = City.CreatePrimaryKey(companyParamCity.Country.Id, companyParamCity.Id, companyParamCity.State.Id);
                 bool result= DataFacadeManager.Delete(primaryKey);
                 return (result) ? "Ok" : "Error";
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            
        }

        /// <summary>
        /// retorna el consecutivo correspondiente a la combinacion de 
        /// country y state, el id generado se usa para insertar la ciudad
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public int FindCity(int countryId, int stateId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(City.Properties.CountryCode, countryId);
            filter.And();
            filter.PropertyEquals(City.Properties.StateCode, stateId);
            System.Collections.IList cityList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(City), filter.GetPredicate(), null));
            //cityList.MaxBy(x => x.Height);
            List<CompanyParamCity> cities = ModelAssembler.ConvertToModelCities(cityList);
            return cities.Max(x => x.Id) + 1;

        }

        /// <summary>
        /// Obtiene el listado de las ciudades
        /// </summary>
        /// <returns></returns>
        internal List<CompanyParamCity> GetAllCity()
        {
            List<string> errorListDescription = new List<string>();
            try
            {
                List<CompanyParamCity> listCompanyParamCity = new List<CompanyParamCity>();
                CitiesView view = new CitiesView();
                ViewBuilder builder = new ViewBuilder("CitiesView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (City cities in view.City)
                {
                    Country country = view.Country.Cast<Country>().First(x => x.CountryCode == cities.CountryCode);
                    State state = view.State.Cast<State>().First(x => x.StateCode == cities.StateCode && x.CountryCode== cities.CountryCode);

                    CompanyParamCity resultCity = ModelAssembler.MappParamCity(cities, country, state);
                    listCompanyParamCity.Add(resultCity);
                }

               
                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
               
                throw new BusinessException(ex.Message, ex);
            }

        }
     
        /// <summary>
        /// Obtiene el listado de las ciudades en la busqueda avanzada
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public List<CompanyParamCity> GetAdvCity(CompanyParamCity companyParamCity)
        {           
            List<string> errorListDescription = new List<string>();
            bool firstP=false;
            try
            {
                List<CompanyParamCity> listCompanyParamCity = new List<CompanyParamCity>();
                CitiesView view = new CitiesView();
                ViewBuilder builder = new ViewBuilder("CitiesView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
               
                if (companyParamCity.Country.Id != 0)
                {
                    filter.Property(City.Properties.CountryCode, typeof(City).Name);
                    filter.Equal();
                    filter.Constant(companyParamCity.Country.Id);
                    firstP=true;
                 }
               
                if(companyParamCity.State.Id!=0)
                 {
                    if (firstP)
                        filter.And();
                    filter.Property(City.Properties.StateCode, typeof(City).Name);
                    filter.Equal();
                    filter.Constant(companyParamCity.State.Id);
                    firstP=true;
                 }
               
                 if(companyParamCity.Description!=null)
                 {
                    if (firstP)
                        filter.And();
                    filter.Property(City.Properties.Description, typeof(City).Name);
                    filter.Like();
                    filter.Constant("%"+companyParamCity.Description+"%");
                   
                 }
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                foreach (City cities in view.City)
                {
                    Country country = view.Country.Cast<Country>().First(x => x.CountryCode == cities.CountryCode);
                    State state = view.State.Cast<State>().First(x => x.StateCode == cities.StateCode && x.CountryCode == cities.CountryCode);

                    CompanyParamCity resultCity = ModelAssembler.MappParamCity(cities, country, state);
                    listCompanyParamCity.Add(resultCity);
                }
               
                return listCompanyParamCity;
            }

            catch (System.Exception ex)
            {
               
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Genera el listado de las ciudades para el listado excel
        /// </summary>
        /// <returns></returns>
        public string GenerateExcelCity(string fileName)
        {
            var cities = GetAllCity();
            FileDAOs fileDAOs = new FileDAOs();
            return fileDAOs.GenerateFileToCity(cities, fileName);
        } 
    }
}
