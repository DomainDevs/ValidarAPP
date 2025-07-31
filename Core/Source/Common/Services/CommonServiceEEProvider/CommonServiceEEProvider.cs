using Sistran.Company.Application.CommonServices.EEProvider.Resources;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Helper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.Threading.Tasks;
using COMMML = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.CommonServices.EEProvider.Helper;
using System.Diagnostics;
using Sistran.Company.Application.CommonServices.EEProvider.DAOs;

namespace Sistran.Core.Application.CommonServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CommonServiceEEProvider : ICommonServiceCore
    {

        /// <summary>
        /// Devuelve el listado de países
        /// </summary>
        /// <returns></returns>      
        public List<COMMML.Country> GetCountries()
        {
            try
            {
                CountryDAO countryDAO = new CountryDAO();
                return countryDAO.GetCountries();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCountryList), ex);
            }
        }

        /// <summary>
        /// /// Devuelve listado de tipos de monedas
        /// </summary>
        /// <returns></returns>        
        public List<COMMML.Currency> GetCurrencies()
        {
            try
            {
                CurrencyDAO currencyProvider = new CurrencyDAO();
                return currencyProvider.GetCurrencies();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTypesOfCurrencyList), ex);
            }
        }

        /// <summary>
        /// /// Devuelve listado de tipos de monedas por producto
        /// </summary>
        /// <returns></returns>        
        public List<COMMML.Currency> GetCurrenciesByProductId(int productId)
        {
            try
            {
                CurrencyDAO currencyProvider = new CurrencyDAO();
                return currencyProvider.GetCurrenciesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetlistCurrencyTypesByProduct), ex);
            }
        }



        /// <summary>
        /// Obtener listado de Bancos
        /// </summary>
        /// <returns></returns>
        public List<COMMML.Bank> GetBanks()
        {
            try
            {
                BankDAO bankprovider = new BankDAO();
                return bankprovider.GetBanks();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfBanks), ex);
            }
        }

        /// <summary>
        /// Obtener lista de los difentes medios de pagos
        /// </summary>
        /// <returns></returns>
        public List<COMMML.PaymentMethod> GetPaymentMethods()
        {
            try
            {
                PaymentMethodDAO paymentMethodProvider = new PaymentMethodDAO();
                return paymentMethodProvider.GetPaymentMethods();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfDifferentMeansOfPayments), ex);
            }
        }

        public List<COMMML.Parameter> GetParametersByParameterIds(List<COMMML.Parameter> parameters)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetParametersByParameterIds(parameters);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfParametersById), ex);
            }
        }

        public List<COMMML.Parameter> GetParametersByIds(List<int> ids)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetParametersByIds(ids);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfParametersById), ex);
            }
        }

        public List<COMMML.Parameter> GetParametersByDescriptions(List<string> parameters)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetParametersByDescriptions(parameters);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfParametersByDescription), ex);
            }
        }

        public COMMML.Parameter GetParameterByDescription(string description)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetParameterByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfParametersByDescription), ex);
            }
        }

        /// <summary>
        /// Obtener lista de parametros por id's
        /// </summary>
        /// <param name="parameters">Lista de id's</param>
        /// <returns>Lista de parametros</returns>
        public List<COMMML.Parameter> GetExtendedParameters(List<COMMML.Parameter> parameters)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetExtendedParameters(parameters);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExtendedParameters), ex);
            }
        }

        /// <summary>
        /// Obtener lista de ramos tecnicos por ramo comercial
        /// Consulta tablas [COMM].[LINE_BUSINESS] [COMM].[PREFIX_LINE_BUSINESS] [QUO].[PERIL_LINE_BUSINESS] [QUO].[INS_OBJ_LINE_BUSINESS] [QUO].[CLAUSE_LEVEL] [QUO].[PERIL] [QUO].[INSURED_OBJECT] [QUO].[CLAUSE]
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLinesBusinessByPrefixId(int prefixId)
        {
            try
            {
                LineBusinessDAO lineBusinessProvider = new LineBusinessDAO();
                return lineBusinessProvider.GetLinesBusinessByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfTechnicalBranchByPrefix), ex);
            }
        }

        /// <summary>
        /// Obtener e ramo tecnico por ramo comercial
        /// Consulta tablas [COMM].[LINE_BUSINESS] [COMM].[PREFIX_LINE_BUSINESS] [QUO].[PERIL_LINE_BUSINESS] [QUO].[INS_OBJ_LINE_BUSINESS] [QUO].[CLAUSE_LEVEL] [QUO].[PERIL] [QUO].[INSURED_OBJECT] [QUO].[CLAUSE]
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public int GetLinesBusinessCodeByPrefixId(int prefixId)
        {
            try
            {
                LineBusinessDAO lineBusinessProvider = new LineBusinessDAO();
                return lineBusinessProvider.GetLinesBusinessCodeByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfTechnicalBranchByPrefix), ex);
            }
        }

        /// <summary>
        /// Obtener lista de ramos tecnicos por ramo comercial
        /// Consulta tablas [COMM].[LINE_BUSINESS] [COMM].[PREFIX_LINE_BUSINESS] 
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLineBusinessByPrefixId(int prefixId)
        {
            try
            {
                LineBussinessDAO lineBusinessProvider = new LineBussinessDAO();
                return lineBusinessProvider.GetLineBusinessByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfTechnicalBranchByPrefix), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<COMMML.Branch> GetBranches()
        {
            try
            {
                BranchDAO brnchprovider = new BranchDAO();
                return brnchprovider.GetBranches();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfBranches), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListAdded"> Lista de branchs(sucursales) para ser agregados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public List<COMMML.Branch> CreateBranchs(List<COMMML.Branch> ListAdded)
        {
            try
            {
                BranchDAO branch = new BranchDAO();
                return branch.CreateBranchs(ListAdded);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForBranches), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListEdited">Lista de branchs(sucursales) para ser modificados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public List<COMMML.Branch> UpdateBranchs(List<COMMML.Branch> ListEdited)
        {
            try
            {
                BranchDAO branch = new BranchDAO();
                return branch.UpdateBranchs(ListEdited);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForBranches), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListDeleted">Lista de branchs(sucursales) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public List<COMMML.Branch> DeleteBranchs(List<COMMML.Branch> ListDeleted)
        {
            try
            {
                BranchDAO branch = new BranchDAO();
                return branch.DeleteBranchs(ListDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForBranches), ex);
            }
        }

        /// <summary>
        /// Get all UserSalePoint 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns> List of UserSalePoint</returns>
        public List<COMMML.SalePoint> GetSalePointByBranchId(int branchId, bool isEnabled)
        {
            try
            {
                SalePointDAO saleprovider = new SalePointDAO();
                return saleprovider.GetSalePointByBranchId(branchId, isEnabled);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAllUserPointSale), ex);
            }
        }

        /// <summary>
        /// Obtener lista de ramos comerciales
        /// </summary>
        /// <returns></returns>
        public List<COMMML.Prefix> GetPrefixes()
        {
            try
            {
                PrefixDAO prefixProvider = new PrefixDAO();
                return prefixProvider.GetPrefixes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfPrefix), ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de póliza
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Lista de tipos de póliza</returns>
        public List<COMMML.PolicyType> GetPolicyTypesByProductId(int productId)
        {
            try
            {
                DAOs.PolicyTypeDAO policyTypeProvider = new DAOs.PolicyTypeDAO();
                return policyTypeProvider.GetPolicyTypesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfPolicyTypes), ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de póliza
        /// </summary>
        /// <param name="productId">Id Ramo</param>
        /// <returns>Lista de tipos de póliza</returns>
        public List<COMMML.PolicyType> GetPolicyTypesByPrefixId(int prefixId)
        {
            try
            {
                DAOs.PolicyTypeDAO policyTypeProvider = new DAOs.PolicyTypeDAO();
                return policyTypeProvider.GetPolicyTypesByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfPolicyTypesByBusinessBranch), ex);
            }
        }

        /// <summary>
        /// Obtener puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <returns>Lista de puntos de venta</returns>
        public List<COMMML.SalePoint> GetSalePointsByBranchId(int branchId)
        {
            try
            {
                SalePointDAO salePointProvider = new SalePointDAO();
                return salePointProvider.GetSalePointsByBranchId(branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPointSaleByBranch), ex);
            }
        }

        /// <summary>
        /// Obtener el listado de sucursales dependiendo del banco seleccionado
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public List<COMMML.BankBranch> GetBankBranches(int bankId)
        {
            try
            {
                BankBranchDAO bankBranchProvider = new BankBranchDAO();
                return bankBranchProvider.GetBankBranches(bankId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfBranchesDependingSelectedBank), ex);
            }
        }

        /// <summary>
        /// Obtener el importe de cambio por fecha y moneda
        /// </summary>
        /// <param name="rateDate"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public COMMML.ExchangeRate GetExchangeRateByRateDateCurrencyId(DateTime rateDate, int currencyId)
        {
            try
            {
                ExchangeRateDAO exchangeRateProvider = new ExchangeRateDAO();
                return exchangeRateProvider.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExchangeAmountByDateAndCurrency), ex);
            }
        }

        /// <summary>
        /// Obtener rango de tolerancia
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public bool CalculateExchangeRateTolerance(decimal newRate, int currencyId)
        {
            try
            {
                ExchangeRateDAO exchangeRateProvider = new ExchangeRateDAO();
                return exchangeRateProvider.CalculateExchangeRateTolerance(newRate, currencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDate), ex);
            }
        }

        public DateTime GetDate()
        {
            try
            {
                return HelperDate.Now;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDate), ex);
            }
        }

        /// <summary>
        /// Obtener lista de subramos tecnicos
        /// </summary>
        /// <param name="lineBusinessId">Id ramo tecnico</param>
        /// <returns></returns>
        public List<COMMML.SubLineBusiness> GetSubLinesBusinessByLineBusinessId(int lineBusinessId)
        {
            try
            {
                SubLineDAO subLineBusinessProvider = new SubLineDAO();
                return subLineBusinessProvider.GetSubLinesBusinessByLineBusinessId(lineBusinessId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfTechnicalSubBranches), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<COMMML.SubLineBusiness> GetSubLineBusinessByLineBusinessId()
        {
            try
            {
                SubLineDAO subLineBusinessProvider = new SubLineDAO();
                return subLineBusinessProvider.GetSubLineBusinessByLineBusinessId();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListOfTechnicalSubBranches), ex);
            }
        }


        /// <summary>
        /// Obtener fecha de emisión
        /// </summary>
        /// <param name="moduleCode">Codigo de Modulo</param>
        /// <param name="issueDate">Fecha de Emisión</param>
        /// <returns>Fecha real de emisión</returns>
        public DateTime GetModuleDateIssue(int moduleCode, DateTime issueDate)
        {
            try
            {
                ModuleDateDAO coModuleDateDAO = new ModuleDateDAO();
                return coModuleDateDAO.GetDateIssue(moduleCode, issueDate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetIssueDate), ex);
            }
        }

        /// <summary>
        /// Actualizar parametro
        /// </summary>
        /// <param name="parameter">Modelo de parameter</param>
        /// <returns>Modelo parameter<Parameter Model></Parameter></returns>
        public COMMML.Parameter UpdateParameter(COMMML.Parameter parameter)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.UpdateParameters(parameter);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateParameters), ex);
            }
        }

        /// <summary>
        /// Obtener parametro por parameterId COMM.PARAMETER
        /// </summary>
        /// <param name="parameter">Id parametro</param>
        /// <returns>list<Parameter Model></Parameter></returns>
        public COMMML.Parameter GetParameterByParameterId(int parameterId)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.GetParameterByParameterId(parameterId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetParameterByParameterId), ex);
            }
        }

        /// <summary>
        /// Actualizar parametro
        /// </summary>
        /// <param name="parameter">Lista Modelo de parameter</param>
        /// <returns>list<Parameter Model></Parameter></returns>
        public COMMML.Parameter UpdateParameters(COMMML.Parameter parameters)
        {
            try
            {
                ParameterDAO parameterProvider = new ParameterDAO();
                return parameterProvider.UpdateParameters(parameters);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateParameters), ex);
            }
        }

        /* Autor: Jose Manrique Fecha: 01/02/2016 >>*/

        /// <summary>
        /// Buscar un Parametro especifico tabla CO_PARAMETER
        /// </summary>
        /// <param name="parameterId">Id paremetro</param>
        /// <returns>Parameter</returns>
        public COMMML.Parameter GetExtendedParameterByParameterId(int parameterId)
        {
            try
            {
                return ParameterDAO.GetExtendedParameterByParameterId(parameterId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExtendedParameterByParameterId), ex);
            }
        }



        /// <summary>
        /// Actualizar parametro CO
        /// </summary>
        /// <param name="parameter">Parametro</param>
        /// <returns>Parametro</returns>
        public COMMML.Parameter UpdateExtendedParameter(COMMML.Parameter parameter)
        {
            try
            {
                return ParameterDAO.UpdateExtendedParameter(parameter);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateExtendedParameter), ex);
            }
        }



        /// <summary>
        /// Obtener Ramo comercial
        /// </summary>
        /// <param name="id">Id ramo comercial</param>
        /// <returns>Ramo comercial</returns>
        public COMMML.Prefix GetPrefixById(int id)
        {
            try
            {
                PrefixDAO prefixProvider = new PrefixDAO();
                return prefixProvider.GetPrefixById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrefix), ex);
            }
        }


        /// <summary>
        /// Obtener Sucursal por ID
        /// </summary>
        /// <param name="id">Id sucursal</param>
        /// <returns>Sucursal</returns>
        public COMMML.Branch GetBranchById(int id)
        {
            try
            {
                BranchDAO branchProvider = new BranchDAO();
                return branchProvider.GetBranchById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBranchById), ex);
            }
        }

        ///// <summary>
        ///// realiza el envio de un email
        ///// </summary>
        ///// <param name="email">detalles del email</param>
        ///// <returns></returns>
        //public bool SendEmail(COMMML.EmailCriteria email)
        //{
        //    try
        //    {
        //        return HelperEmail.SendEmail(email);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}




        /// <summary>
        /// Obtener lista de negocio
        /// </summary>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLinesBusiness()
        {
            try
            {
                LineBusinessDAO lineBusinessDAO = new LineBusinessDAO();
                return lineBusinessDAO.GetLinesBusiness();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetLineBusiness), ex);
            }
        }



        /// <summary>
        /// Obtener lista de Risk Type de acuerdo a parametro
        /// </summary>
        /// <returns></returns>
        //public List<COMMML.HardRiskType> GetHardRiskTypesByCoveredRiskType(CoveredRiskType coveredRiskType)
        //{
        //    try
        //    {
        //        HardRiskTypeDAO hardRiskTypeDAO = new HardRiskTypeDAO();
        //        return hardRiskTypeDAO.GetHardRiskTypesByCoveredRiskType(coveredRiskType);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        //public COMMML.HardRiskType GetHardRiskTypeByLineBusinessIdCoveredRiskType(int lineBusinessId, CoveredRiskType coveredRiskType)
        //{
        //    try
        //    {
        //        HardRiskTypeDAO hardRiskTypeDAO = new HardRiskTypeDAO();
        //        return hardRiskTypeDAO.GetHardRiskTypeByLineBusinessIdCoveredRiskType(lineBusinessId, coveredRiskType);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}



        /// <summary>
        /// Listado de conceptos de pago
        /// </summary>
        //public List<COMMML.PaymentConcept> GetPaymentConcept()
        //{
        //    try
        //    {
        //        PaymentDAO paymentDAO = new PaymentDAO();
        //        return paymentDAO.GetPaymentConcept();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPaymentConcept), ex);
        //    }
        //}

        /// <summary>
        /// Busca valores por defecto de un usuario, modulo y submodulo
        /// </summary>
        /// <param name="defaultValue">defaultValue.</param>
        /// <returns>Lista de DefaultValue</returns>       
        public List<COMMML.DefaultValue> GetDefaultValueByDefaultValue(COMMML.DefaultValue defaultValue)
        {
            try
            {
                DefaultValueDAO defaultValueDAO = new DefaultValueDAO();
                return defaultValueDAO.GetDefaultValueByDefaultValue(defaultValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSearchDefaultValuesOfUserModuleAndSubmodule), ex);
            }
        }


        /// <summary>
        /// Obtiene llave del application
        /// </summary>
        /// <param name="key">nombre de la llave</param>
        /// <returns>string</returns>
        public string GetKeyApplication(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetApplicationKey), ex);
            }
        }

        /// <summary>
        /// Obtener ciudad por id ciudad
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public COMMML.City GetCityByCity(COMMML.City city)
        {
            try
            {
                CityDAO cityDAO = new CityDAO();
                return cityDAO.GetCityByCity(city);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCityByCityId), ex);
            }
        }


        /// <summary>
        /// Obtener bancos por id
        /// </summary>
        /// <param name="bankId">identificador de banco</param>
        /// <returns>Modelo Bank</returns>
        public COMMML.Bank GetBanksByBankId(int bankId)
        {
            try
            {
                BankDAO bankProvider = new BankDAO();
                return bankProvider.GetBanksByBankId(bankId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBanksByBankId), ex);
            }
        }




        public COMMML.LineBusiness CreateLineBussiness(COMMML.LineBusiness linebussines)
        {
            try
            {
                LineBussinessDAO lineBusinessDAO = new LineBussinessDAO();
                return lineBusinessDAO.CreateLineBussiness(linebussines);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateLineBussiness), ex);
            }
        }

        public COMMML.LineBusiness GetLineBusinessById(string descriptionLineBusiness, int IdLineBusiness)
        {
            try
            {
                LineBussinessDAO lineBusinessDAO = new LineBussinessDAO();
                return lineBusinessDAO.GetLineBusinessById(descriptionLineBusiness, IdLineBusiness);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetLineBusinessById), ex);
            }
        }

        public COMMML.SubLineBusiness GetSubLineBusinessById(int Id, int lineBusinessId)
        {
            try
            {
                LineBussinessDAO lineBusinessDAO = new LineBussinessDAO();
                return lineBusinessDAO.GetSubLineBusinessById(Id, lineBusinessId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetSubLineBusinessById), ex);
            }
        }

        public List<COMMML.PrefixType> GetPrefixType()
        {
            try
            {
                PrefixDAO PrefixTypeDAO = new PrefixDAO();
                return PrefixTypeDAO.GetPrefixType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrefixType), ex);
            }
        }

        public COMMML.PrefixType GetPrefixTypeByPrefixId(int PrefixId)
        {
            try
            {
                PrefixDAO PrefixTypeDAO = new PrefixDAO();
                return PrefixTypeDAO.GetPrefixTypeByPrefixId(PrefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrefixTypeByPrefixId), ex);
            }
        }

        public List<COMMML.LineBusiness> GetRiskTypeByLineBusinessId()
        {
            try
            {
                LineBussinessDAO lineBusinessDAO = new LineBussinessDAO();
                return lineBusinessDAO.GetRiskTypeByLineBusinessId();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRiskTypeByLineBusinessId), ex);
            }
        }


        /// <summary>
        /// Genera archivo excel subramo técnico
        /// </summary>
        /// <param name="subLinebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToSubLinebusiness(List<COMMML.SubLineBusiness> subLinebusiness, string fileName)
        {
            try
            {
                LineBussinessDAO fileDAO = new LineBussinessDAO();
                return fileDAO.GenerateFileToSubLinebusiness(subLinebusiness, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToSubLinebusiness), ex);
            }
        }

        /// <summary>
        /// Genera archivo excel sucursales
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToBranch(List<COMMML.Branch> branch, string fileName)
        {
            try
            {
                BranchDAO fileDAO = new BranchDAO();
                return fileDAO.GenerateFileToBranch(branch, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToBranch), ex);
            }
        }

        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <param name="linebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileLinebusiness(List<COMMML.LineBusiness> linebusiness, string fileName)
        {
            try
            {
                LineBussinessDAO fileDAO = new LineBussinessDAO();
                return fileDAO.GenerateFileLinebusiness(linebusiness, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileLinebusiness), ex);
            }
        }


        public List<COMMML.Prefix> GetPrefixAll()
        {
            try
            {
                var prefixDao = new PrefixDAO();
                return prefixDao.GetPrefixAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrefixAll), ex);
            }
        }

        /// <summary>
        /// Obtener todos los ramos comerciales
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error in GetPrefixAll</exception>
        public List<COMMML.Prefix> GetAllPrefix()
        {
            try
            {
                var prefixDao = new PrefixDAO();
                return prefixDao.GetAllPrefix();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAllPrefix), ex);
            }
        }


        /// <summary>
        /// Obtener Ramos Tecnicos
        /// </summary>
        /// <param name="coveredRiskType">Tipo De Riesgo</param>
        /// <returns>Ramos Tecnicos</returns>
        public List<COMMML.LineBusiness> GetLinesBusinessByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            try
            {
                LineBusinessDAO lineBusinessDAO = new LineBusinessDAO();
                return lineBusinessDAO.GetLinesBusinessByCoveredRiskType(coveredRiskType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFileByFilters), ex);
            }
        }

        public List<COMMML.State> GetStates()
        {
            try
            {
                StateDAO stateProvider = new StateDAO();
                return stateProvider.GetStates();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetStates), ex);
            }
        }

        public List<COMMML.City> GetCities()
        {
            try
            {
                CityDAO cityProvider = new CityDAO();
                return cityProvider.GetCities();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCities), ex);
            }
        }

        /// <summary>
        ///Obtener Tipo de poliza por ramo y tipo de poliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="Id">Identificador del tipo de poliza</param>
        /// <returns></returns>
        public COMMML.PolicyType GetPolicyTypesByPrefixIdById(int prefixId, int id)
        {
            try
            {
                DAOs.PolicyTypeDAO policyTypeDAO = new DAOs.PolicyTypeDAO();
                return policyTypeDAO.GetPolicyTypesByPrefixIdById(prefixId, id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorPolicyType), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para subramo técnico
        /// </summary>
        /// <param name="subLineBusinessAdd">Lista de subramo técnico para ser agregados</param>
        /// <returns></returns>
        public List<COMMML.SubLineBusiness> CreateSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessAdd)
        {
            try
            {
                SubLineDAO sublinebusinessDAO = new SubLineDAO();
                return sublinebusinessDAO.CreateSubLineBusiness(subLineBusinessAdd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForSubBranches), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para subramo técnico
        /// </summary>
        /// <param name="subLineBusinessEdit">Lista de subramo técnico para ser editados</param>
        /// <returns></returns>
        public List<COMMML.SubLineBusiness> UpdateSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessEdit)
        {
            try
            {
                SubLineDAO sublinebusinessDAO = new SubLineDAO();
                return sublinebusinessDAO.UpdateSubLineBusiness(subLineBusinessEdit);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForSubBranches), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para subramo técnico
        /// </summary>
        /// <param name="subLineBusinessDelete">Lista de subramo técnico para ser eliminados</param>
        /// <returns></returns>
        public List<COMMML.SubLineBusiness> DeleteSubLineBusiness(List<COMMML.SubLineBusiness> subLineBusinessDelete)
        {
            try
            {
                SubLineDAO sublinebusinessDAO = new SubLineDAO();
                return sublinebusinessDAO.DeleteSubLineBusiness(subLineBusinessDelete);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForSubBranches), ex);
            }
        }



        /// <summary>
        ///  Obtener Codigos CoveredRiskType de HardRiskType
        /// </summary>
        /// <param name="coveredRiskType">Tipo De Riesgo</param>
        /// <returns>Ramos Tecnicos</returns>
        public List<COMMML.LineBusiness> GetHardRiskTypeByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            try
            {
                LineBusinessDAO lineBusinessDAO = new LineBusinessDAO();
                return lineBusinessDAO.GetHardRiskTypeByCoveredRiskType(coveredRiskType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFileByFilters), ex);
            }
        }

        public List<COMMML.LineBusiness> GetLineBusinessBySubCoveredRiskType(SubCoveredRiskType subCoveredRiskType)
        {
            try
            {
                LineBussinessDAO lineBusinessDAO = new LineBussinessDAO();
                return lineBusinessDAO.GetLineBusinessBySubCoveredRiskType(subCoveredRiskType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public SubCoveredRiskType GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(int prefixId, int? coveredRiskType)
        {
            try
            {
                LineBussinessDAO lineBusinessDAO = new LineBussinessDAO();
                return lineBusinessDAO.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(prefixId, coveredRiskType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene tipos de avisos
        /// </summary>
        /// <returns></returns>
        public List<COMMML.ClaimNoticeType> GetClaimNoticeTypes()
        {
            try
            {
                var claimDAO = new ClaimDAO();
                return claimDAO.GetClaimNoticeTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene el listado de países sin ninguna relación
        /// </summary>
        /// <returns>Listado de países</returns>
        public List<Country> GetCountriesLite()
        {
            try
            {
                CountryDAO countryDAO = new CountryDAO();
                return countryDAO.GetCountriesLite();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCountryListLite), ex);
            }
        }

        /// <summary>
        /// Obtener la lista de estados asociados al identificador del país
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <returns>Listado de estados</returns>
        public List<State> GetStatesByCountryId(int countryId)
        {
            try
            {
                StateDAO stateProvider = new StateDAO();
                return stateProvider.GetStatesByCountryId(countryId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetStatesByCountryId), ex);
            }
        }

        /// <summary>
        /// Obtener un listado de ciudades a partir del país y el estado
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <param name="stateId">Identificador del estado (departamento)</param>
        /// <returns>Listado de ciudades</returns>
        public List<City> GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                CityDAO cityProvider = new CityDAO();
                return cityProvider.GetCitiesByCountryIdStateId(countryId, stateId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCitiesByCountryIdStateId), ex);
            }
        }
        public List<Prefix> GetPrefixesByUserId(int userId)
        {
            try
            {
                PrefixDAO prefixDAO = new PrefixDAO();
                return prefixDAO.GetPrefixesByUserId(userId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener un listado de tasas de cambio a partir de la moneda
        /// </summary>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <returns>Listado de tasas de cambio</returns>
        public ExchangeRate GetExchangeRateByCurrencyId(int currencyId)
        {
            try
            {
                ExchangeRateDAO exchangeRate = new ExchangeRateDAO();
                return exchangeRate.GetExchangeRateByCurrencyId(currencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExchangeRateByCurrencyId), ex);
            }
        }


        /// <summary>
        /// Obtener el listado de tasas de cambio
        /// </summary>
        /// <returns>Listado de tasas de cambio</returns>
        public List<ExchangeRate> GetExchangeRates(DateTime? dateCumulus = null, int? CurrecyCode = null)
        {
            try
            {
                ExchangeRateDAO exchangeRateDAO = new ExchangeRateDAO();
                return exchangeRateDAO.GetExchangeRates(dateCumulus, CurrecyCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExchangeRateByCurrencyId), ex);
            }
        }

        public DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime movementDate)
        {
            try
            {
                ModuleDateDAO moduleDateDAO = new ModuleDateDAO();
                return moduleDateDAO.GetModuleDateByModuleTypeMovementDate(moduleType, movementDate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetIssueDate), ex);
            }
        }

        public List<Prefix> GetPrefixesByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            try
            {
                PrefixDAO prefixDAO = new PrefixDAO();
                return prefixDAO.GetPrefixesByCoveredRiskType(coveredRiskType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Task GetAsyncHelper(string baseUrl, string url, Dictionary<string, string> parameters)
        {
            try
            {
                Task result = ApiProxy.GetAsync(baseUrl, url, parameters);

                return result;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Error GetAsyncHelper- baseUrl: " + baseUrl + " url: " + url, ex.Message, EventLogEntryType.Error);
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Task PostAsyncHelper(string baseUrl, string url, dynamic parameters)
        {
            try
            {
                Task result = ApiProxy.PostAsync(baseUrl, url, parameters);

                return result;
            }
            catch (Exception ex)
            {

                EventLog.WriteEntry("Error PostAsyncHelper- baseUrl: " + baseUrl + " url: " + url, ex.Message, EventLogEntryType.Error);
                throw new BusinessException(ex.Message, ex);
            }
        }

        public COMMML.City GetCityByCountryIdByStateIdByCityId(int countryId, int stateId, int cityId)
        {
            try
            {
                CountryDAO countryDAO = new CountryDAO();
                return countryDAO.GetCountryByCountryIdByStateIdByCityId(countryId, stateId, cityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCountryList), ex);
            }
        }

        public FileProcessValue GetFileProcessValue(int fileId)
        {
            FileDAO fileDAO = new FileDAO();
            return fileDAO.GetFileProcessValue(fileId);
        }


        public List<City> GetCitiesByCountry(Country country)
        {
            CityDAO cityDAO = new CityDAO();
            return cityDAO.GetCitiesByCountry(country);
        }

        public List<COMMML.City> GetCitiesByState(COMMML.State state)
        {
            try
            {
                CityDAO cityDAO = new CityDAO();
                return cityDAO.GetCitiesByState(state);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCitiesByCountryIdStateId), ex);
            }
        }

        #region BankAccountType

        /// <summary>
        /// GetBankAccountTypes
        /// </summary>
        /// <returns>List<BankAccountType/></returns>
        public List<AccountType> GetBankAccountTypes()
        {
            try
            {
                AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
                return accountTypeDAO.GetBankAccountTypes();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion
        public List<ComponentType> GetComponentType()
        {
            try
            {
                ComponentTypeDAO componentTypeDAO = new ComponentTypeDAO();
                return componentTypeDAO.GetComponentType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCountryList), ex);
            }
        }
    }
}

