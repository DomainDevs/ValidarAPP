using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using COMMENUM = Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Application.CommonService
{
    [ServiceContract]
    public interface ICommonServiceCore
    {

        /// <summary>
        /// Devuelve el listado de países
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Country> GetCountries();


        [OperationContract]
        List<Country> GetCountriesLite();

        /// <summary>   
        /// /// Devuelve listado de tipos de monedas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Currency> GetCurrencies();

        /// <summary>
        /// Obteber listado de Bancos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Bank> GetBanks();

        /// <summary>
        /// Obetener lista de los diferentes medios de pago
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.PaymentMethod> GetPaymentMethods();

        [OperationContract]
        List<Models.Parameter> GetParametersByParameterIds(List<Models.Parameter> parameters);

        [OperationContract]
        List<Models.Parameter> GetParametersByIds(List<int> ids);

        [OperationContract]
        List<Models.Parameter> GetParametersByDescriptions(List<string> parameters);

        [OperationContract]
        Models.Parameter GetParameterByDescription(string description);

        /// <summary>
        /// Obtener lista de parametros por id's
        /// </summary>
        /// <param name="parameters">Lista de id's</param>
        /// <returns>Lista de parametros</returns>
        [OperationContract]
        List<Models.Parameter> GetExtendedParameters(List<Models.Parameter> parameters);

        /// <summary>
        /// Obtener lista de ramos tecnicos por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        [OperationContract]
        List<LineBusiness> GetLinesBusinessByPrefixId(int prefixId);

        /// <summary>
        /// Obtener e ramo tecnico por ramo comercial
        /// Consulta tablas [COMM].[LINE_BUSINESS] [COMM].[PREFIX_LINE_BUSINESS] [QUO].[PERIL_LINE_BUSINESS] [QUO].[INS_OBJ_LINE_BUSINESS] [QUO].[CLAUSE_LEVEL] [QUO].[PERIL] [QUO].[INSURED_OBJECT] [QUO].[CLAUSE]
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        int GetLinesBusinessCodeByPrefixId(int prefixId);

        /// <summary>
        /// Obtener Sucursales
        /// </summary>
        /// <returns>Obtener Lista de Branchs</returns>
        [OperationContract]
        List<Branch> GetBranches();


        /// <summary>
        /// Get all UserSalePoint 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns> List of UserSalePoint</returns>
        [OperationContract]
        List<Models.SalePoint> GetSalePointByBranchId(int branchId, bool isEnabled);

        /// <summary>
        /// Obtener los Ramos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Prefix> GetPrefixes();


        /// <summary>
        /// Obtener lista de tipos de póliza
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Lista de tipos de póliza</returns>
        [OperationContract]
        List<PolicyType> GetPolicyTypesByProductId(int productId);


        /// <summary>
        /// Obtener puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <returns>Lista de puntos de venta</returns>
        [OperationContract]
        List<Models.SalePoint> GetSalePointsByBranchId(int branchId);



        /// <summary>
        /// Obtener el listado de sucursales de pendiendo del banco seleccionado
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.BankBranch> GetBankBranches(int bankId);

        /// <summary>
        /// Obtener el importe de cambio por fecha y moneda
        /// </summary>
        /// <param name="rateDate"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        [OperationContract]
        ExchangeRate GetExchangeRateByRateDateCurrencyId(DateTime rateDate, int currencyId);

        /// <summary>
        /// Obtener rango de tolerancia
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        [OperationContract]
        bool CalculateExchangeRateTolerance(decimal newRate, int currencyId);

        /// <summary>
        /// Obtener fecha Servidor
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DateTime GetDate();

        /// <summary>
        /// Obtener lista de subramos tecnicos
        /// </summary>
        /// <param name="lineBusinessId">Id ramo tecnico</param>
        /// <returns></returns>
        [OperationContract]
        List<SubLineBusiness> GetSubLinesBusinessByLineBusinessId(int lineBusinessId);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.SubLineBusiness> GetSubLineBusinessByLineBusinessId();

        /// <summary>
        /// Obtener fecha de emisión
        /// </summary>
        /// <param name="moduleCode">Codigo de Modulo</param>
        /// <param name="issueDate">Fecha de Emisión</param>
        /// <returns>Fecha real de emisión</returns>
        [OperationContract]
        DateTime GetModuleDateIssue(int moduleCode, DateTime issueDate);

        /// <summary>
        /// Buscar un Parametro especifico tabla CO_PARAMETER
        /// </summary>
        /// <param name="parameterId">Id paremetro</param>
        /// <returns>Parameter</returns>
        [OperationContract]
        Models.Parameter GetExtendedParameterByParameterId(int parameterId);

        /// <summary>
        /// Actualizar parametro
        /// </summary>
        /// <param name="parameter">Lista Modelo de parameter</param>
        /// <returns>list<Parameter Model></Parameter></returns>
        [OperationContract]
        Models.Parameter UpdateParameters(Models.Parameter parameters);

        /// <summary>
        /// Actualizar parametro
        /// </summary>
        /// <param name="parameter">Lista Modelo de parameter</param>
        /// <returns>Model.Parameter<Parameter Model></Parameter></returns>
        [OperationContract]
        Models.Parameter UpdateParameter(Models.Parameter parameter);



        /// <summary>
        /// Actualizar parametro CO
        /// </summary>
        /// <param name="parameter">Parametro</param>
        /// <returns>Parametro</returns>
        Parameter UpdateExtendedParameter(Parameter parameter);

        /// <summary>
        /// Obtener parametro por parametro Id
        /// </summary>
        /// <param name="parameter">Id Parameter</param>
        /// <returns>Model.Parameter<Parameter Model></Parameter></returns>
        [OperationContract]
        Models.Parameter GetParameterByParameterId(int parameterId);



        /// <summary>
        /// Obtener Ramo comercial
        /// </summary>
        /// <param name="id">Id ramo comercial</param>
        /// <returns>Ramo comercial</returns>
        [OperationContract]
        Prefix GetPrefixById(int id);

        /// <summary>
        /// Obtener Sucursal
        /// </summary>
        /// <param name="id">Id sucursal</param>
        /// <returns>Sucursal</returns>
        [OperationContract]
        Branch GetBranchById(int id);

        ///// <summary>
        ///// realiza el envio de un email
        ///// </summary>
        ///// <param name="email">detalles del email</param>
        ///// <returns></returns>
        //[OperationContract]
        //bool SendEmail(EmailCriteria email);




        /// <summary>
        /// Obtener lista de tipos de póliza
        /// </summary>
        /// <param name="productId">Id Ramo Comercial</param>
        /// <returns>Lista de tipos de póliza</returns>
        [OperationContract]
        List<Models.PolicyType> GetPolicyTypesByPrefixId(int prefixId);

        /// <summary>
        /// Obtener lista de negocio
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.LineBusiness> GetLinesBusiness();



        // <summary>
        /// Obtener lista de Risk Type de acuerdo a parametro
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        //List<HardRiskType> GetHardRiskTypesByCoveredRiskType(CoveredRiskType coveredRiskType);

        //[OperationContract]
        //HardRiskType GetHardRiskTypeByLineBusinessIdCoveredRiskType(int lineBusinessId, CoveredRiskType coveredRiskType);


        /// <summary>
        /// Listado de concetos de pago
        /// </summary>
        //[OperationContract]
        //List<Models.PaymentConcept> GetPaymentConcept();


        /// <summary>
        /// Busca valores por defecto de un usuario, modulo y submodulo
        /// </summary>
        /// <param name="defaultValue">defaultValue.</param>
        /// <returns>Lista de DefaultValue</returns>   
        [OperationContract]
        List<Models.DefaultValue> GetDefaultValueByDefaultValue(Models.DefaultValue defaultValue);





        /// <summary>
        /// Obtiene llave del application
        /// </summary>
        /// <param name="key">nombre de la llave</param>
        /// <returns>string</returns>
        [OperationContract]
        string GetKeyApplication(string key);

        [OperationContract]
        Models.City GetCityByCity(Models.City city);



        /// <summary>
        /// Obtener bancos por id
        /// </summary>
        /// <param name="bankId">identificador de banco</param>
        /// <returns>Modelo Bank</returns>
        [OperationContract]
        Models.Bank GetBanksByBankId(int bankId);


        /// <summary>
        /// Validar Campos Plantillas
        /// </summary>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos Plantillas</returns>
        //[OperationContract]
        //Models.File ValidateTemplateRows(Models.Template template);



        /// <summary>
        /// obtiene los ramo tecnicos por descripcion
        /// </summary>
        /// <param name="descriptionLineBusiness"></param>
        /// <param name="IdLineBusiness"></param>
        /// <returns></returns>
        [OperationContract]
        LineBusiness GetLineBusinessById(string descriptionLineBusiness, int IdLineBusiness);


        /// <summary>
        /// obtiene el subramo tecnico por Id
        /// </summary>
        /// <param name="descriptionLineBusiness"></param>
        /// <param name="IdLineBusiness"></param>
        /// <returns></returns>
        [OperationContract]
        SubLineBusiness GetSubLineBusinessById(int Id, int lineBusinessId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.LineBusiness> GetRiskTypeByLineBusinessId();


        [OperationContract]
        List<Models.PrefixType> GetPrefixType();

        [OperationContract]
        Models.PrefixType GetPrefixTypeByPrefixId(int PrefixId);


        /// <summary>
        /// Genera archivo excel subramo técnico
        /// </summary>
        /// <param name="subLinebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToSubLinebusiness(List<SubLineBusiness> subLinebusiness, string fileName);

        /// <summary>
        /// Genera archivo excel sucursales
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToBranch(List<Branch> branch, string fileName);

        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <param name="linebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileLinebusiness(List<LineBusiness> linebusiness, string fileName);

        /// <summary>
        /// Obtiene ramo comercial
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Prefix> GetPrefixAll();



        /// Obtener los ramos asociados
        /// </summary>
        /// <param name="coveredRiskTypeCode">coveredRiskTypeCode</param>
        /// <returns></returns>
        //[OperationContract]
        //List<HardRiskType> GetPrefixAssociated(int coveredRiskTypeCode);


        /// <summary>
        /// Obtener todos los ramos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Prefix> GetAllPrefix();

        /// <summary>
        /// /// Devuelve listado de tipos de monedas por producto
        /// </summary>
        /// <returns></returns> 
        [OperationContract]
        List<Models.Currency> GetCurrenciesByProductId(int productId);


        /// <summary>
        /// Obtener Ramos Tecnicos
        /// </summary>
        /// <param name="coveredRiskType">Tipo De Riesgo</param>
        /// <returns>Ramos Tecnicos</returns>
        [OperationContract]
        List<LineBusiness> GetLinesBusinessByCoveredRiskType(COMMENUM.CoveredRiskType coveredRiskType);

        /// <summary>
        /// Obtener Departamentos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.State> GetStates();

        [OperationContract]
        List<Models.State> GetStatesByCountryId(int countryId);

        /// <summary>
        /// Obtener Ciudades
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.City> GetCities();

        [OperationContract]
        List<Models.City> GetCitiesByCountryIdStateId(int countryId, int stateId);

        /// <summary>
        ///Obtener Tipo de poliza por ramo y tipo de poliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="Id">Identificador del tipo de poliza</param>
        /// <returns></returns>
        [OperationContract]
        Models.PolicyType GetPolicyTypesByPrefixIdById(int prefixId, int id);

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListAdded"> Lista de branchs(sucursales) para ser agregados</param>
        /// <param name="ListEdited">Lista de branchs(sucursales) para ser modificados</param>
        /// <param name="ListDeleted">Lista de branchs(sucursales) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        [OperationContract]
        List<Branch> CreateBranchs(List<Branch> ListAdded);

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListAdded"> Lista de branchs(sucursales) para ser agregados</param>
        /// <param name="ListEdited">Lista de branchs(sucursales) para ser modificados</param>
        /// <param name="ListDeleted">Lista de branchs(sucursales) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        [OperationContract]
        List<Branch> UpdateBranchs(List<Branch> ListEdited);

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListAdded"> Lista de branchs(sucursales) para ser agregados</param>
        /// <param name="ListEdited">Lista de branchs(sucursales) para ser modificados</param>
        /// <param name="ListDeleted">Lista de branchs(sucursales) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        [OperationContract]
        List<Branch> DeleteBranchs(List<Branch> ListDeleted);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sublinesbusiness"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubLineBusiness> CreateSubLineBusiness(List<Models.SubLineBusiness> subLineBusinessAdd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sublinesbusiness"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubLineBusiness> UpdateSubLineBusiness(List<Models.SubLineBusiness> subLineBusinessEdit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sublinesbusiness"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubLineBusiness> DeleteSubLineBusiness(List<Models.SubLineBusiness> subLineBusinessDelete);


        /// <summary>
        /// crear un nuevo ramo tecnico
        /// </summary>
        /// <param name="linebussiness"></param>
        /// <returns></returns>
        [OperationContract]
        Models.LineBusiness CreateLineBussiness(Models.LineBusiness linebussines);

        /// <summary>
        /// Obtener lista de ramos tecnicos por ramo comercial
        /// Consulta tablas [COMM].[LINE_BUSINESS] [COMM].[PREFIX_LINE_BUSINESS] 
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.LineBusiness> GetLineBusinessByPrefixId(int prefixId);

        /// <summary>
        /// Obtener Codigos CoveredRiskType de HardRiskType
        /// </summary>
        /// <param name="coveredRiskType">Tipo De Riesgo</param>
        /// <returns>Ramos Tecnicos</returns>
        [OperationContract]
        List<LineBusiness> GetHardRiskTypeByCoveredRiskType(COMMENUM.CoveredRiskType coveredRiskType);

        [OperationContract]
        List<LineBusiness> GetLineBusinessBySubCoveredRiskType(COMMENUM.SubCoveredRiskType subCoveredRiskType);

        [OperationContract]
        COMMENUM.SubCoveredRiskType GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(int prefixId, int? coveredRiskType);

        [OperationContract]
        List<Models.ClaimNoticeType> GetClaimNoticeTypes();
        /// <summary>
        /// Obtener los Ramos Por Usuario
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Prefix> GetPrefixesByUserId(int userId);



        /// <summary>
        /// Obtener un listado de tasas de cambio a partir de la moneda
        /// </summary>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <returns>Listado de tasas de cambio</returns>
        [OperationContract]
        ExchangeRate GetExchangeRateByCurrencyId(int currencyId);
        [OperationContract]
        DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime movementDate);
        /// <summary>
        /// Consultar ramos comerciales por tipo de riesgo cubierto
        /// </summary>
        /// <param name="coveredRiskType"></param>
        /// <returns></returns>
        [OperationContract]
        List<Prefix> GetPrefixesByCoveredRiskType(COMMENUM.CoveredRiskType coveredRiskType);
        [OperationContract]
        Task GetAsyncHelper(string baseUrl, string url, Dictionary<string, string> parameters);

        [OperationContract]
        Task PostAsyncHelper(string baseUrl, string url, dynamic parameters);

        /// <summary>
        /// Devuelve la ciudad
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        City GetCityByCountryIdByStateIdByCityId(int countryId, int stateId, int cityId);
        
        [OperationContract]
        FileProcessValue GetFileProcessValue(int fileId);

        [OperationContract]
        List<City> GetCitiesByCountry(Country country);

        [OperationContract]
        List<City> GetCitiesByState(State state);
        
        [OperationContract]
        List<AccountType> GetBankAccountTypes();

        [OperationContract]
        List<ComponentType> GetComponentType();

        [OperationContract]
        List<ExchangeRate> GetExchangeRates(DateTime? dateCumulus = null, int? CurrecyCode = null);
    }
		
}