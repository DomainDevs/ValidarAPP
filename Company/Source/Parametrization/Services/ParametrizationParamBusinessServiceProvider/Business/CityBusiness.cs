using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Business
{
    /// <summary>
    /// Realiza el llamado a los DAOs correspondientes para los diferentes eventos de la vista de ABM ciudades
    /// </summary>
    class CityBusiness
    {

        /// <summary>
        /// llamado a DAO para consultar el listado completo de ciudades
        /// </summary>
        /// <returns></returns>
        public List<CompanyParamCity> GetBusinessCity()
        {
            try
            {
                CityDao cityDao = new CityDao();
                return cityDao.GetAllCity();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Llamado a DAO para insertar la informacion de un  nuevo registro de ciudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public CompanyParamCity CreateBusinessCity(CompanyParamCity companyParamCity)
        {
            CityDao cityDao = new CityDao();
            return cityDao.CreateCity(companyParamCity);
        }

        /// <summary>
        /// Llamado a DAO para insertar la informacion de un  nuevo registro de ciudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public string DeleteBusinessCity(CompanyParamCity companyParamCity)
        {           
            CityDao cityDao = new CityDao();
            return cityDao.DeleteCity(companyParamCity);
        }

        /// <summary>
        /// Llamado a DAO para insertar la informacion de un  nuevo registro de ciudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public string GenerateFileToCity(string fileName)
        {
            CityDao cityDao = new CityDao();
            return cityDao.GenerateExcelCity(fileName);
        }

        /// <summary>
        /// UpdateBusinessCity: actualiza la informacion de una ciudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public CompanyParamCity UpdateBusinessCity(CompanyParamCity companyParamCity)
        {
            CityDao cityDao = new CityDao();
            return cityDao.UpdateCity(companyParamCity);
        }

        /// <summary>
        /// Consulta el listado de ciudades por la descripcion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<CompanyParamCity> GetByDescriptionCity(string description)
        {
            CityDao cityDao = new CityDao();
            return cityDao.GetByDescriptionCity(description);
        }

        /// <summary>
        /// consulta el listado de ciudades de la bsuqeda avanzada
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public List<CompanyParamCity> GetAdvCity(CompanyParamCity companyParamCity)
        {
            CityDao cityDao = new CityDao();
            return cityDao.GetAdvCity(companyParamCity);
        }
    }
}
