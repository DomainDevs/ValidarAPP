// -----------------------------------------------------------------------
// <copyright file="ParametrizationParamBusinessServiceProvider.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider
{
    using System;
    using System.Data;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Core.Framework.BAF;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Business;
    using System.Collections.Generic;
    using Sistran.Company.Application.ParametrizationParamBusinessService;
    using Sistran.Core.Application.Utilities.Managers;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs;






    /// <summary>
    /// Clase que implementa la interfaz "IParametrizationParamBusinessService" 
    /// </summary>
    public class ParametrizationParamBusinessServiceProvider : IParametrizationParamBusinessService
    {
      
        #region BillingPeriod
        /// <summary>
        /// GetBusinessBillingPeriod.  Obtiene el Listado de BillingPeriod desde el DAOs
        /// </summary>
        /// <returns>List<CompanyParamBillingPeriod></returns>
        public List<CompanyParamBillingPeriod> GetBusinessBillingPeriod()
        {
            try
            {
                BillingPeriodBusiness BillingPeriodBus = new BillingPeriodBusiness(); 
                return BillingPeriodBus.GetBusinessBillingPeriod();
            }
            catch (Exception ex)
            {
                throw new Exception( Resources.Errors.GetBillPeriod + " "+ ex.Message);
            }

           

            
        }
        /// <summary>
        /// GenerateFileToBusinessBillingPeriod. Genera el archivo de reporte en formato excel desde el DAOs
        /// </summary>
        /// <param name="BillingPeriodList"></param>
        /// <param name="fileName"></param>
        /// <returns>v</returns>
        public string GenerateFileToBusinessBillingPeriod(List<CompanyParamBillingPeriod> BillingPeriodList, string fileName)
        {
            try
            {
                BillingPeriodBusiness file = new BillingPeriodBusiness();
                return file.GenerateFileToBillingPeriod(BillingPeriodList, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.Errors.GetFileExcel + " " + ex.Message);
            }
        }
        #endregion

        #region BusinessType

        /// <summary>
        /// GetBusinessBusinessType. Obtiene el Listado de BusinessTypeQuery desde el DAOs
        /// </summary>
        /// <returns> List<CompanyParamBusinessType> </returns>
        public List<CompanyParamBusinessType> GetBusinessBusinessType()
        {
            try
            {
                BusinessTypeBusiness BusinessTypeBus = new BusinessTypeBusiness();
                return BusinessTypeBus.GetBusinessBusinessType();

            }
            catch (Exception ex)
            {
                throw new Exception(Resources.Errors.GetBusinessType + " " + ex.Message);
                
            }
        }

        /// <summary>
        /// GenerateFileToBusinessBusinessType. Genera el archivo de reporte en formato excel desde DAOs
        /// </summary>
        /// <param name="BusinessTypeList"></param>
        /// <param name="fileName"></param>
        /// <returns>String</returns>
        public String GenerateFileToBusinessBusinessType(List<CompanyParamBusinessType> BusinessTypeList, string fileName)
        {
            try
            {
                BusinessTypeBusiness file = new BusinessTypeBusiness();
                return file.GenerateFileToBusinessType(BusinessTypeList, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.Errors.GetFileExcel + " " + ex.Message);
            }
        }

        #endregion

        #region city
        /// <summary>
        /// CreateBusinessCity: llamado a business para insertar informacion de nuevo registro de ciudad- table comm.city
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public CompanyParamCity CreateBusinessCity(CompanyParamCity companyParamCity)
        {
            try
            {
                CityBusiness city = new CityBusiness();
                return city.CreateBusinessCity(companyParamCity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// DeleteBusinessCity: llamado a business para eliminar el registro de una ciudad - table comm.city
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public string DeleteBusinessCity(CompanyParamCity companyParamCity)
        {
            try
            {
                CityBusiness city = new CityBusiness();
                return city.DeleteBusinessCity(companyParamCity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetBusinessCity: llamado a Business para hacer consultar listado de cities - table comm.city
        /// </summary>
        /// <returns></returns>
        public List<CompanyParamCity> GetBusinessCity()
        {
            try
            {
               CityBusiness city = new CityBusiness();
                return city.GetBusinessCity();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetBusinessCityAdv: Listado de ciudades con filtro de busuqeda avanzada
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public List<CompanyParamCity> GetBusinessCityAdv(CompanyParamCity companyParamCity)
        {
            try
            {
                CityBusiness city = new CityBusiness();
                return city.GetAdvCity(companyParamCity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetByDescriptionCity: listado de ciudades con filtro de busqueda simple por el campo description table comm.city
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<CompanyParamCity> GetByDescriptionCity( string description)
        {
            try
            {
                CityBusiness city = new CityBusiness();
                return city.GetByDescriptionCity(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// UpdateBusinessCity: actualiza la informacion de descripcion y abreviatura de una ciudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        public CompanyParamCity UpdateBusinessCity(CompanyParamCity companyParamCity)
        {
            try
            {
                CityBusiness city = new CityBusiness();
                return city.UpdateBusinessCity(companyParamCity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GenerateFileToCity: Genera el archivo excel de las ciudades registradas
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public CompanyExcel GenerateFileToCity(string fileName)
        {
            try
            {
                CityBusiness city = new CityBusiness();
                return new CompanyExcel { FileData = city.GenerateFileToCity(fileName) };
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region SalesPoint
        public bool ValidateSalesPointIdBusiness(int IdSalesPoint, int BranchId)
        {
            try
            {
                SalesPointBusiness salesPointBusiness = new SalesPointBusiness();
                return salesPointBusiness.GetSalesPointIdBusiness(IdSalesPoint, BranchId);
            }
            catch(Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion


        #region Parameter
        /// <summary>
        /// Obtiene la información de la tabla parametros
        /// </summary>
        /// <param name="parameterId">int</param>
        /// <param name="parameterDescription">string</param>
        /// <returns>Parameter</returns>
        public CompanyParameters GetParameterByParameterId(int parameterId)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetParameterByParameterId(parameterId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
        #endregion Parameter
    }

}

