// -----------------------------------------------------------------------
// <copyright file="ParametrizationAplicationServiceProvider.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServiceProvider
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Sistran.Company.Application.ParametrizationAplicationServiceProvider.Assemblers;
    using Sistran.Company.Application.ParametrizationAplicationServices;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Core.Framework.BAF;
    using Sistran.Company.Application.ParametrizationAplicationServiceProvider.Business;
    using System.ServiceModel;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;

    /// <summary>
    /// clase que implementa la interfaz "IParametrizationAplicationService"
    /// </summary>  
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ParametrizationAplicationServiceProvider : IParametrizationAplicationService
    {
        /// <summary>
        /// Initial. Prueba Incial.
        /// </summary>
        /// <returns>Strign. Inicial</returns>
        public string Initial()
        {
            return String.Empty;
        }

        #region BillingPeriod
        /// <summary>
        /// GetApplicationBillingPeriod. Obtiene el Listado de BillingPeriod desde el Negocio
        /// </summary>
        /// <returns>BillingPeriodQueryDTO</returns>
        public BillingPeriodQueryDTO GetApplicationBillingPeriod()
        {
            BillingPeriodQueryDTO BillingPeriodDTOS = null;
            try
            {
                BillingPeriodDTOS = BillingPeriodBusiness.GetApplicationBillingPeriod();
                return BillingPeriodDTOS;
            }
            catch (Exception ex)
            {
                if (BillingPeriodDTOS != null)
                {
                    BillingPeriodDTOS.Error.ErrorType = Utilities.Enums.ErrorType.BusinessFault;
                    return BillingPeriodDTOS;
                }
                else
                {
                    BillingPeriodDTOS = new BillingPeriodQueryDTO();
                    BillingPeriodDTOS.Error.ErrorType = Utilities.Enums.ErrorType.TechnicalFault;
                    BillingPeriodDTOS.Error.ErrorDescription.Add(ex.Message);
                    return BillingPeriodDTOS;
                }
            }
        }

        /// <summary>
        /// GenerateFileToApplicationBillingPeriod. Genera el archivo de reporte en formato excel desde el Negocio
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ExcelFileDTO</returns>
        public ExcelFileDTO GenerateFileToApplicationBillingPeriod(string fileName)
        {
            ExcelFileDTO BillPeriodExcelFileDTO = new ExcelFileDTO();

            try
            {
                BillPeriodExcelFileDTO = BillingPeriodBusiness.GenerateFileToBillingPeriods(fileName); ;
                return BillPeriodExcelFileDTO;
            }
            catch (Exception ex)
            {
                if (BillPeriodExcelFileDTO != null)
                {
                    BillPeriodExcelFileDTO.ErrorType = Utilities.Enums.ErrorType.BusinessFault;
                    return BillPeriodExcelFileDTO;
                }
                else
                {
                    BillPeriodExcelFileDTO = new ExcelFileDTO();
                    BillPeriodExcelFileDTO.ErrorType = Utilities.Enums.ErrorType.TechnicalFault;
                    BillPeriodExcelFileDTO.ErrorDescription.Add(ex.Message);
                    return BillPeriodExcelFileDTO;
                }
            }
        }

        #endregion

        #region BusinessType
        /// <summary>
        /// GetApplicationBusinessType. Obtiene el Listado de BusinessTypeQuery desde el Negocio
        /// </summary>
        /// <returns>BusinessTypeQueryDTO</returns>
        public BusinessTypeQueryDTO GetApplicationBusinessType()
        {
            BusinessTypeQueryDTO BusinessTypeDTOS = null;
            try
            {
                BusinessTypeDTOS = BusinessTypeBusiness.GetApplicationBusinessType();
                return BusinessTypeDTOS;
            }
            catch (Exception ex)
            {
                if (BusinessTypeDTOS != null)
                {
                    BusinessTypeDTOS.Error.ErrorType = Utilities.Enums.ErrorType.BusinessFault;
                    return BusinessTypeDTOS;
                }
                else
                {
                    BusinessTypeDTOS = new BusinessTypeQueryDTO();
                    BusinessTypeDTOS.Error.ErrorType = Utilities.Enums.ErrorType.TechnicalFault;
                    BusinessTypeDTOS.Error.ErrorDescription.Add(ex.Message);
                    return BusinessTypeDTOS;
                }
            }
        }

        /// <summary>
        /// GenerateFileToApplicationBusinessType. Genera el archivo de reporte en formato excel desde el Negocio
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ExcelFileDTO</returns>
        public ExcelFileDTO GenerateFileToApplicationBusinessType(string fileName)
        {
            ExcelFileDTO BusinessTypeExcelFileDTO = new ExcelFileDTO();

            try
            {
                BusinessTypeExcelFileDTO = BusinessTypeBusiness.GenerateFileToBusinessTypes(fileName); ;
                return BusinessTypeExcelFileDTO;
            }
            catch (Exception ex)
            {
                if (BusinessTypeExcelFileDTO != null)
                {
                    BusinessTypeExcelFileDTO.ErrorType = Utilities.Enums.ErrorType.BusinessFault;
                    return BusinessTypeExcelFileDTO;
                }
                else
                {
                    BusinessTypeExcelFileDTO = new ExcelFileDTO();
                    BusinessTypeExcelFileDTO.ErrorType = Utilities.Enums.ErrorType.TechnicalFault;
                    BusinessTypeExcelFileDTO.ErrorDescription.Add(ex.Message);
                    return BusinessTypeExcelFileDTO;
                }
            }
        }
        #endregion

        #region ciudades

        /// <summary>
        /// CreateApplicationCity: Registra informacion de una nueva ciudad
        /// </summary>
        /// <param name="cityDTO"></param>
        /// <returns></returns>
        public CityDTO CreateApplicationCity(CityDTO cityDTO)
        {
            List<string> errorDescriptions = new List<string>();
            CityDTO cityqueryDTO = new CityDTO();

            try
            {
                CompanyParamCity companyParamCity = Assemblers.AplicationCompanyAssembler.MappCity(cityDTO);
                ParametrizationParamBusinessServiceProvider providerBusiness = new ParametrizationParamBusinessServiceProvider();
                companyParamCity = providerBusiness.CreateBusinessCity(companyParamCity);
                cityqueryDTO = Assemblers.CompanyAplicationAssembler.MappParamCity(companyParamCity);
                cityqueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return cityqueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// DeleteApplicationCity: elimina el registro de una ciudad
        /// </summary>
        /// <param name="cityDTO"></param>
        /// <returns></returns>
        public CityDTO DeleteApplicationCity(CityDTO cityDTO)
        {
            try
            {
                CompanyParamCity companyParamCity = Assemblers.AplicationCompanyAssembler.MappCity(cityDTO);
                string result = DelegateService.provider.DeleteBusinessCity(companyParamCity);
                cityDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return cityDTO;
            }
            catch(Exception ex)
            {
                cityDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.BusinessFault };
                return cityDTO;
            }
        }

        /// <summary>
        /// Obtiene el listado de ciudades completo, implementa la capacidad de applicationService
        /// </summary>
        /// <returns></returns>
        public CityQueryDTO GetApplicationCity()
        {
            CityQueryDTO cityqueryDTO = new CityQueryDTO();
            List<string> errorDescriptions = new List<string>();
            try
            {
                ParametrizationParamBusinessServiceProvider providerBusiness = new ParametrizationParamBusinessServiceProvider();
                List<CompanyParamCity> cities = providerBusiness.GetBusinessCity();
                List<CityDTO> citiesDTO = Assemblers.CompanyAplicationAssembler.MappParamCities(cities);
                cityqueryDTO.CityDTO = citiesDTO;
                cityqueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return cityqueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// UpdateApplicationCity: actualiza la informacion de una ciudad 
        /// </summary>
        /// <param name="cityDTO"></param>
        /// <returns></returns>
        public CityDTO UpdateApplicationCity(CityDTO cityDTO)
        {
            try
            {
                CompanyParamCity companyParamCity = Assemblers.AplicationCompanyAssembler.MappCity(cityDTO);
                companyParamCity = DelegateService.provider.UpdateBusinessCity(companyParamCity);

                cityDTO = Assemblers.CompanyAplicationAssembler.MappParamCity(companyParamCity);
                cityDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return cityDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GenerateFileToCity:Llamado a metodo para generar el archivo excel con las ciudades registradas en BD
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ExcelFileDTO GenerateFileToCity(string fileName)
        {
            ExcelFileDTO excelFileDTO = new ExcelFileDTO();

            try
            {
                var companyExcel = DelegateService.provider.GenerateFileToCity(fileName);
                return CompanyAplicationAssembler.MappExcelFile(companyExcel);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetApplicationCityAdv: implementacion de consulta de ciudades para busqueda avanzada
        /// </summary>
        /// <param name="cityDTO"></param>
        /// <returns></returns>
        public CityQueryDTO GetApplicationCityAdv(CityDTO cityDTO)
        {
            CityQueryDTO cityqueryDTO = new CityQueryDTO();

            CompanyParamCity companyParamCity = Assemblers.AplicationCompanyAssembler.MappCity(cityDTO);
            try
            {
                var result = DelegateService.provider.GetBusinessCityAdv(companyParamCity);
                List<CityDTO> citiesDTO = Assemblers.CompanyAplicationAssembler.MappParamCities(result);
                cityqueryDTO.CityDTO = citiesDTO;
                cityqueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return cityqueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetApplicationCityAdv: implementacion de consulta de ciudades para busqueda avanzada
        /// </summary>
        /// <param name="cityDTO"></param>
        /// <returns></returns>
        public CityQueryDTO GetApplicationCityByDescription(string Description)
        {
            CityQueryDTO cityqueryDTO = new CityQueryDTO();
            try
            {
                var result = DelegateService.provider.GetByDescriptionCity(Description);
                List<CityDTO> citiesDTO = Assemblers.CompanyAplicationAssembler.MappParamCities(result);
                cityqueryDTO.CityDTO = citiesDTO;
                cityqueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return cityqueryDTO;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region SalesPoint
        public bool ValidateExistIdSalesPoint(int IdSalesPoint, int BranchId)
        {
            try
            {
                var result = DelegateService.provider.ValidateSalesPointIdBusiness(IdSalesPoint, BranchId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }


}
