// -----------------------------------------------------------------------
// <copyright file="IUnderwritingParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Application.UnderwritingParamService
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Collections.Generic;
    using System.ServiceModel;
    using ENUMUD = Sistran.Core.Application.UnderwritingServices.Enums;
    using MODCO = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using PARUSM = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Interfaz para UnderwritingParamService
    /// </summary>
    [ServiceContract]
    public interface IUnderwritingParamServiceWeb
    {

        /// <summary>
        /// /// Obtiene la lista de tipos de riesgo cubierto.
        /// </summary>
        /// <returns>Modelo de sevicio del tipo de riesgo cubierto.</returns>
        [OperationContract]
        PARUSM.CoveredRiskTypesServiceModel GetCoveredRiskTypes();

        /// <summary>
        /// Genera archivo excel de Perfiles de Asegurado.
        /// </summary>
        /// <param name="coveredRiskTypesList">Listado de tipos de riesgo cubiertos.</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>Path archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToCoveredRiskTypes(List<PARUSM.CoveredRiskTypeServiceModel> coveredRiskTypesList, string fileName);

        /// <summary>
        /// Obtener lista de planes pago
        /// </summary>
        /// <returns>Obtiene el listado de planes de pago</returns>
        [OperationContract]
        PARUPSM.PaymentPlansServiceModel GetPaymentPlansServiceModel();

        /// <summary>
        /// CRUD de planes de pagos
        /// </summary>
        /// <param name="paymentPlanServiceModel">Planes de pago a Modificar</param>
        /// <returns>Retorna el resultado de la operacion del CRUD </returns>
        [OperationContract]
        List<PARUPSM.PaymentPlanServiceModel> ExecuteOperationsPaymentPlanServiceModel(List<PARUPSM.PaymentPlanServiceModel> paymentPlanServiceModel);

        /// <summary>
        /// Generar archivo excel de plan de pagos
        /// </summary>
        /// <param name="paymentPlans">Listado de planes de pagos</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToPaymentPlan(List<PARUPSM.PaymentPlanServiceModel> paymentPlans, string fileName);

        /// <summary>
        /// Obtene las unidades de los deducibles
        /// </summary>
        /// <returns>Modelo DeductibleUnitsServiceModel</returns>
        [OperationContract]
        PARUPSM.DeductibleUnitsServiceQueryModel GetDeductibleUnits();

        /// <summary>
        /// Obtene el asunto de los deducibles
        /// </summary>
        /// <returns>Modelo DeductibleUnitsServiceModel</returns>
        [OperationContract]
        PARUPSM.DeductibleSubjectsServiceQueryModel GetDeductibleSubjects();

        /// <summary>
        /// Obtene las monedas
        /// </summary>
        /// <returns>Modelo CurrenciesServiceModel</returns>
        [OperationContract]
        PARUPSM.CurrenciesServiceQueryModel GetCurrencies();

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Listado de deducible</returns>
        [OperationContract]
        PARUPSM.DeductiblesServiceModel GetDeductibles();

        /// <summary>
        /// Obtene las lineas de negocio
        /// </summary>
        /// <returns>Modelo LinesBusinessServiceModel</returns>
        [OperationContract]
        PARUPSM.LinesBusinessServiceQueryModel GetLinesBusiness();

        /// <summary>
        /// Validaciones dependencias entidad Deducible
        /// </summary>
        /// <param name="deductibleId">Id de deducible</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        [OperationContract]
        int ValidateDeductible(int deductibleId);

        /// <summary>
        /// Genera archivo excel de coverturas
        /// </summary>
        /// <param name="deductibles">Listado de deducibles</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToDeductible(List<PARUPSM.DeductibleServiceModel> deductibles, string fileName);
        
        #region Amparos
        /// <summary>
        /// Generar archivo excel de amparos
        /// </summary>
        /// <param name="perils">Listado amparos</param>
        /// <param name="fileName">Nombre archivo</param>      
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToPeril(List<PARUPSM.PerilServiceModel> perils, string fileName);

        /// <summary>
        /// Validación de amparos
        /// </summary>
        /// <param name="perilId">Codigo de amparos</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        [OperationContract]
        int ValidatePeril(int perilId);
        #endregion

        #region Surcharge
        /// <summary>
        /// CRUD de recargos
        /// </summary>
        /// <param name="surchargeModel">recargos a Modificar</param>
        /// <returns>Retorna el resultado de la operacion del CRUD </returns>
        [OperationContract]
        List<PARUPSM.SurchargeServiceModel> ExecuteOperationsSurchargeServiceModel(List<PARUPSM.SurchargeServiceModel> surchargeModel);

        /// <summary>
        /// Obtener lista de recargos
        /// </summary>
        /// <returns>Obtiene el listado de recargos</returns>
        [OperationContract]
        PARUPSM.SurchargesServiceModel GetSurchargeServiceModel();

        /// <summary>
        /// Generar archivo excel de recargos
        /// </summary>
        /// <param name="surcharge">Listado de recargos</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToSurcharge(List<PARUPSM.SurchargeServiceModel> surcharge, string fileName);
        #endregion

        #region Discount
        /// <summary>
        /// CRUD de descuentos
        /// </summary>
        /// <param name="surchargeModel">descuentos a Modificar</param>
        /// <returns>Retorna el resultado de la operacion del CRUD </returns>
        [OperationContract]
        List<PARUPSM.DiscountServiceModel> ExecuteOperationsDiscountServiceModel(List<PARUPSM.DiscountServiceModel> surchargeModel);

        /// <summary>
        /// Obtener lista de descuentos
        /// </summary>
        /// <returns>Obtiene el listado de descuentos</returns>
        [OperationContract]
        PARUPSM.DiscountsServiceModel GetDiscountServiceModel();

        /// <summary>
        /// Generar archivo excel de descuentos
        /// </summary>
        /// <param name="discount">Listado descuentos</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToDiscount(List<PARUPSM.DiscountServiceModel> discount, string fileName);
        #endregion

        #region SubRamo Tecnico
        /// <summary>
        /// Obtiene listado de subRamo Tecnico
        /// </summary>
        /// <returns>Retorna listad de subRamo Tecnico</returns>
        [OperationContract]
        PARUPSM.SubLineBranchsServiceModel GetSubLinesBusiness();

        /// <summary>
        /// Obtiene listado de subRamo Tecnico por nombre
        /// </summary>
        /// <param name="name">Recibe Nombre</param>
        /// <returns>Retorna listado de SubRamo Tecnico</returns>
        [OperationContract]
        PARUPSM.SubLineBranchsServiceModel GetSubLineBusinessByName(string name);

        /// <summary>
        /// Genera archivo excel subRamo Tecnico
        /// </summary>
        /// <param name="subLineBusiness">lista de subRamos</param>
        /// <param name="fileName">nombre archivo</param>
        /// <returns>Retorna archivo excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToSubLineBusiness(List<PARUPSM.SubLineBranchServiceModel> subLineBusiness, string fileName);

        /// <summary>
        /// Valida subRamo por nombre
        /// </summary>
        /// <param name="description">Recibe descripcion</param>
        /// <returns>Retorna true o false</returns>
        [OperationContract]
        bool ValidateIfSuBlineBusinessExists(string description);
        #endregion

        #region Detail
      
        /// <summary>
        /// Obtiene los tipos de detalle
        /// </summary>
        /// <returns>Tipos de detalle</returns>
        [OperationContract]
        PARUPSM.DetailTypesServiceQueryModel GetDetailTypes();

        /// <summary>
        /// Obtiene el listado de Detalle
        /// </summary>
        /// <returns>Modelo DetailsServiceModel</returns> 
        [OperationContract]
        PARUPSM.DetailsServiceModel GetParametrizationDetails();

        /// <summary>
        /// Validación de detaller
        /// </summary>
        /// <param name="detailId">Codigo de detalle</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        [OperationContract]
        int ValidateDetail(int detailId);

        /// <summary>
        /// Generar archivo excel de detalles
        /// </summary>
        /// <param name="details">Listado de detalles</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToDetail(List<PARUPSM.DetailServiceModel> details, string fileName);
        #endregion

        #region AllyCoverage
        [OperationContract]
        ParamQueryCoverage CreateBusinessAllyCoverage(ParamQueryCoverage allyCoverage);

        [OperationContract]
        List<ParamQueryCoverage> GetAplicationBusinessAllyCoverageByDescription(string data, int num);

        [OperationContract]
        List<ParamQueryCoverage> GetBusinessAllyCoverageAvd(ParamQueryCoverage allyCoverage);

        [OperationContract]
        string GenerateFileBusinessToAllyCoverage(string fileName);

        [OperationContract]
        string GenerateFileBusinessToAllyCoverageList(List<ParamQueryCoverage> li_paramquery, string fileName);

        /// <summary>
        /// Listado de coberturas aliadas, carga inicial
        /// </summary>
        /// <returns>ParamQueryCoverage. Objeto Cobertura aliada DTO</returns>
        [OperationContract]
        List<ParamQueryCoverage> GetBusinessAllyCoverage();

        [OperationContract]
        ParamQueryCoverage UpdateBusinessAllyCoverage(ParamQueryCoverage allyCoverage, ParamQueryCoverage allyCoverageOld);

        [OperationContract]
        ParamQueryCoverage DeleteBusinessAllyCoverage(ParamQueryCoverage allyCoverage);

        [OperationContract]
        BaseQueryAllyCoverage GetBusinessCoveragePrincipal();

        [OperationContract]
        BaseQueryAllyCoverage GetBusinessCoverageAllyByOnejectinsuredId(int data);
       
        #endregion

        [OperationContract]
        PARUPSM.VehicleTypesServiceModel GetVehicleTypes();

        [OperationContract]
        List<PARUPSM.VehicleTypeServiceModel> ExecuteOperationsVehicleType(List<PARUPSM.VehicleTypeServiceModel> vehicleTypes);

        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToVehicleType(string fileName);

        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToVehicleBody(PARUPSM.VehicleTypeServiceModel vehicleType, string fileName);

        /// <summary>
        ///  Obtiene subramos técnicos
        /// </summary>
        /// <param name="lineBusinessId">Id ramo tecnico</param>
        /// <returns>MS-subramos técnicos</returns>
        [OperationContract]
        PARUSM.SubLinesBusinessServiceQueryModel GetSubLinesBusinessByLineBusinessId(int lineBusinessId);

        /// <summary>
        /// Listado de amparos asociados al id del ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">id del ramo tecnico</param>
        /// <returns>Amparos asociados al id de ramo tecnico - MOD-S</returns>
        [OperationContract]
        PARUSM.PerilsServiceQueryModel GetPerilsServiceQueryModelByLineBusinessId(int lineBusinessId);

        /// <summary>
        /// Listado de objetos del seguro por id de ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">id ramo tecnico</param>
        /// <returns>Objetos del seguro => MOD-S</returns>
        [OperationContract]
        PARUSM.InsuredObjectsServiceQueryModel GetInsuredObjectsServiceQueryModel(int lineBusinessId);

     

        /// <summary>
        /// Obtiene clausuas relacionadas con el nivel condicion(Cobertura)
        /// </summary>
        /// <param name="conditionLevelType">nivel condición</param>
        /// <returns>Clausulas relacionadas con el nivel condicion</returns>
        [OperationContract]
        PARUPSM.ClausesServiceQueryModel GetClausesSQByConditionLevelType(ENUMUD.ConditionLevelType conditionLevelType);

        /// <summary>
        /// Consulta listado de deducibles
        /// </summary>
        /// <returns>listado de deducibles</returns>
        [OperationContract]
        PARUPSM.DeductiblesServiceQueryModel GetDeductiblesSQM();


        /// <summary>
        /// Consulta tipo  de detalles
        /// REQ_#57
        /// </summary>
        /// <returns>listado de deducibles</returns>
        [OperationContract]
        PARUPSM.DetailTypesServiceQueryModel GetDetailTypeSQM();

        /// <summary>
        /// Obtiene coberturas asociadas a la descripcion y ramo tecnico
        /// </summary>
        /// <param name="description">descripcion de cobertura, puede ser descripcion o id de cobertura</param>
        /// <param name="technicalBranchId">id ramo tecnico</param>
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        PARUPSM.CoveragesServiceModel GetCoveragesSMByDescriptionTechnicalBranchId(string description, int? technicalBranchId);

        /// <summary>
        /// Obtiene coberturas de acuerdo a parametros de busqueda avanzada
        /// </summary>
        /// <param name="coverageServiceModel">cobertura con parametros de busqueda</param>
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        PARUPSM.CoveragesServiceModel GetCoverageSMBySearchAdv(PARUPSM.CoverageServiceModel coverageServiceModel);

        /// <summary>
        /// Genera el archivo de excel de todas las coberturas
        /// </summary>
        /// <param name="fileName">nombre de archivo a generar</param>
        /// <returns></returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToCoverage(string fileName);

        /// <summary>
        /// Obtiene listado de coberturas
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        PARUPSM.CoveragesServiceModel GetCoveragesServiceModel();

        /// <summary>
        /// CRUD de Cobertura
        /// </summary>
        /// <param name="coverage">cobertura a modificar</param>
        /// <returns>resultado operacion cobertura</returns>
        [OperationContract]
        PARUPSM.CoverageServiceModel ExecuteOperationCoverage(PARUPSM.CoverageServiceModel coverage);

        #region InsuredObject
        /// <summary>
        /// Generar archivo excel objetos del seguro
        /// </summary>
        /// <param name="insuredObjectServiceModel">Listado de objetos del seguro</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToInsuredObject(List<PARUPSM.InsuredObjectServiceModel> insuredObjectServiceModel, string fileName);

        /// <summary>
        /// Obtener lista de objetos del seguro
        /// </summary>
        /// <param name="description">descripcion de objetos del seguro</param>
        /// <returns>Obtiene el listado de objetos del seguro</returns>
        [OperationContract]
        PARUPSM.InsurancesObjectsServiceModel GetInsuredObjectServiceModelByDescription(string description);

        /// <summary>
        /// Obtener lista de objetos del seguro
        /// </summary>       
        /// <returns>Obtiene el listado de objetos del seguro</returns>
        [OperationContract]
        PARUPSM.InsurancesObjectsServiceModel GetInsuredObjectServiceModel();

        /// <summary>
        /// Validación de objetos de seguro 
        /// </summary>
        /// <param name="insuredObjectId">Codigo de objeto de seguro</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        [OperationContract]
        int ValidateInsuredObject(int insuredObjectId);
        #endregion

        #region GrupoCobertura
        /// <summary>
        /// Generar archivo excel de deducibles
        /// </summary>
        /// <param name="deductibles">Listado deducibles</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToCoverageGroup(List<PARUPSM.CoverageGroupRiskTypeServiceModel> coverageGroupRiskTypeServiceModel, string fileName);
        #endregion

        #region ExpenseComponen
        /// <summary>
        /// Obtiene listado de Gastos de ejecucion
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        ExpensesServiceModel GetExpenseServiceModel();

        /// <summary>
        /// Ejecuta el CRUP de gastos de suscripcion
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        List<ExpenseServiceModel> ExecuteOperationsExpense(List<ExpenseServiceModel> expenseService);

        /// <summary>
        /// Obtiene listado de Gastos de ejecucion por descripcion
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        ExpensesServiceModel GetExpenseByDescription(string description);

        /// <summary>
        /// Obtiene listado de tipod e ejecucion
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        RateTypeServicesQueryModel GetRateType();

        /// <summary>
        /// Obtiene listado en Excel Expense
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        ExcelFileServiceModel GenerateFileToExpense(string fileName);

        /// <summary>
        /// Obtiene listado de reglas
        /// </summary>        
        /// <returns>listado de reglas</returns>
        [OperationContract]
        RulesSetServiceQueryModel GetRuleSet();

        #endregion

        #region Clauses
        /// <summary>
        /// Obtener lista de clausulas
        /// </summary>
        /// <returns>Obtiene el listado de las clausulas</returns>
        [OperationContract]
        PARUSM.ClausesServiceModel GetClausesServiceModel();

        /// <summary>
        /// CRUD de clausulas
        /// </summary>
        /// <param name="clauseServiceModel">Clausulas a modificar</param>
        /// <returns>Retorna el resultado de la operacion del CRUD</returns>
        [OperationContract]
        List<PARUSM.ClauseServiceModel> ExecuteOperationsClauseServiceModel(List<PARUSM.ClauseServiceModel> clauseServiceModel);

        /// <summary>
        /// Obtiene lista de niveles
        /// </summary>
        /// <returns>retorna lista niveles</returns>
        [OperationContract]
        PARUSM.ConditionLevelsServiceModel GetClausesLevelsServiceModel();

        /// <summary>
        /// Obtiene lista de ramos comerciales
        /// </summary>
        /// <returns>retorna lista ramos comerciales</returns>
        [OperationContract]
        PARUSM.PrefixsServiceQueryModel GetCommercialBranch();
        /// <summary>
        /// Obtiene Lista de tipos de riesgo
        /// </summary>
        /// <returns>Retorna lista tipo de riesgo</returns>
        [OperationContract]
        PARUSM.RiskTypesServiceModel GetCoveredRiskType();

        /// <summary>
        /// Obtiene listado de textos precatalogados
        /// </summary>
        /// <param name="name">Recibe nombreo codigo</param>
        /// <returns>Retorna listado de textos</returns>
        [OperationContract]
        PARUSM.TextsServiceModel GetTextServiceModel(string name);

        /// <summary>
        /// Obtiene listado de coberturas
        /// </summary>
        /// <param name="name">Recibe nombre o codigo</param>
        /// <returns>Retorna listado de coberturas</returns>
        [OperationContract]
        PARUSM.CoveragesClauseServiceModel GetCoverageByName(string name);

        /// <summary>
        /// Obtiene listado de clausulas por nombre o titulo
        /// </summary>
        /// <param name="name">Recibe nombre o titulo</param>
        /// <returns>Retorna listado de clausulas</returns>
        [OperationContract]
        PARUSM.ClausesServiceModel GetClauseByNameAndTitle(string name);

        /// <summary>
        /// Generar archivo excel de clausulas
        /// </summary>
        /// <param name="clauses">Listado clausulas</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToClause(List<PARUSM.ClauseServiceModel> clauses, string fileName);
        #endregion

        /// <summary>
        /// Obtiene la lista de Ramos comerciales.
        /// </summary>
        /// <returns>Modelo de sevicio para Ramos comerciales.</returns>
        [OperationContract]
        List<PARUSM.PrefixServiceQueryModel> GetPrefixes();

        /// <summary>
        /// Obtiene la lista de Solicitudes agrupadoras vigentes por ramo comercial.
        /// </summary>
        /// <returns>Modelo de sevicio para Solicitudes agrupadoras vigentes por ramo comercial.</returns>
        [OperationContract]
        List<PARUSM.RequestEndorsementServiceQueryModel> GetCurrentRequestEndorsementByPrefixCode(int prefixCode);

        /// <summary>
        /// Obtiene lista de tipos de niveles de influencia
        /// </summary>
        /// <returns>Modelo de servicio para niveles de influencia</returns>
        [OperationContract]
        PARUSM.CompositionTypesServiceQueryModel GetCompositionTypes();
        
        /// <summary>
        /// Obtiene la lista de productos vigentes por ramo comercial.
        /// </summary>
        /// <returns>Modelo de sevicio para productos vigentes por ramo comercial.</returns>
        [OperationContract]
        List<PARUSM.ProductServiceQueryModel> GetCurrentProductByPrefixCode(int prefixCode);

        /// <summary>
        /// Obtiene la lista de coberturas por producto.
        /// </summary>
        /// <returns>Modelo de sevicio para coberturas por producto.</returns>
        [OperationContract]
        List<PARUSM.GroupCoverageServiceQueryModel> GetGroupCoverageByProductCode(int productCode);

        /// <summary>
        /// Obtiene la lista de asistencias por producto.
        /// </summary>
        /// <returns>Modelo de sevicio para asistencias por producto.</returns>
        [OperationContract]
        List<PARUSM.AssistanceTypeServiceQueryModel> GetAssistanceTypeByProductCode(int productCode);

        /// <summary>
        /// Obtiene la lista de negocios.
        /// </summary>
        /// <returns>Modelo de sevicio para negocios.</returns>
        [OperationContract]
        PARUSM.BusinessServiceQueryModel GetBusinessConfiguration();

        /// <summary>
        /// Realiza las operaciones CRUD para el negocio y acuerdos de negocio.
        /// </summary>
        /// <param name="listBusiness">lista de negocio y acuerdos de negocio.</param>
        /// <returns>Resumen de las operaciones</returns>
        [OperationContract]
        UTILMO.ParametrizationResponse<PARUSM.BusinessServiceModel> SaveBusiness(PARUSM.BusinessServiceQueryModel listBusiness);

        /// <summary>
        /// Generar archivo excel de negocios y acuerdos de negocios
        /// </summary>        
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToBusiness();

        #region QuotationNumber

        /// <summary>
        /// Consulta los números de cotización por sucursal
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <returns>Lista de números de cotización</returns>
        [OperationContract]
        PARUPSM.QuotationNumbersServiceModel GetParametrizationQuotationNumbersByBranchId(int branchId);

        /// <summary>
        /// Consulta los números de cotización por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <param name="prefixId">Id de ramo</param>
        /// <returns>Lista de números de cotización</returns>
        [OperationContract]
        PARUPSM.QuotationNumbersServiceModel GetParametrizationQuotationNumbersByBranchIdPrefixId(int branchId, int prefixId);

        /// <summary>
        /// Generar archivo excel de números de cotización
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToQuotationNumber(string fileName);

        /// <summary>
        /// Validación de numeración de cotización
        /// </summary>
        /// <param name="branchId">Codigo de sucursal</param>
        /// <param name="prefixId">Codigo de ramo</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        [OperationContract]
        int ValidateQuotationNumber(int branchId, int prefixId);
        #endregion

        #region PolicyNumber

        /// <summary>
        /// Consulta los números de póliza por sucursal
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <returns>Lista de números de cotización</returns>
        [OperationContract]
        PARUPSM.PolicyNumbersServiceModel GetParamPolicyNumbersByBranchId(int branchId);

        /// <summary>
        /// Consulta los números de póliza por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <param name="prefixId">Id de ramo</param>
        /// <returns>Lista de números de póliza</returns>
        [OperationContract]
        PARUPSM.PolicyNumbersServiceModel GetParamPolicyNumbersByBranchIdPrefixId(int branchId, int prefixId);

        /// <summary>
        /// Generar archivo excel de números de póliza
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToPolicyNumber(string fileName);

        /// <summary>
        /// Validación de numeración de póliza
        /// </summary>
        /// <param name="branchId">Codigo de sucursal</param>
        /// <param name="prefixId">Codigo de ramo</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        [OperationContract]
        int ValidatePolicyNumber(int branchId, int prefixId);
        #endregion

        #region Parametrización ramo técnico

        /// <summary>
        /// CRUD de Ramo técnico
        /// </summary>
        /// <param name="businessLineServiceModel">Ramo técnico MOD-S</param>
        /// <returns>Listado de plan de pago producto de la operacion del CRUD</returns>
        [OperationContract]
        PARUPSM.LineBusinessServiceModel ExecuteOperationsLineBusinessServiceModel(PARUPSM.LineBusinessServiceModel businessLineServiceModel);

        [OperationContract]
        PARUPSM.LineBusinessServiceModel DeleteLineBusiness(PARUPSM.LineBusinessServiceModel businessLineSM);

        [OperationContract]
        List<ClauseLevelServiceModel> ExecuteOperationsClause(int idLineBusiness, List<ClauseLevelServiceModel> clausesSM);

        [OperationContract]
        List<PARUPSM.InsuredObjectServiceModel> ExecuteOperationsInsuredObject(int idLineBusiness, List<PARUPSM.InsuredObjectServiceModel> insuredObjectsSM);

        [OperationContract]
        List<PARUPSM.PerilServiceModel> ExecuteOperationsPeril(int idLineBusiness, List<PARUPSM.PerilServiceModel> perilsSM);

        /// <summary>
        /// Obtener ramo técnico por Id
        /// </summary>
        /// <param name="lineBusinessId">iId del ramo técnico</param>
        /// <returns>Ramo Técnico</returns>
        [OperationContract]
        PARUPSM.LineBusinessServiceModel GetBusinesLinesServiceModelByLineBusinessId(int lineBusinessId);

        /// <summary>
        /// Obtiene la lista de ramos técnicos filtrados por descripción o Id
        /// </summary>
        /// <param name="description">Descripción de ramo técnico</param>
        /// <param name="id">Id de ramo técnico</param>
        /// <returns>Bool de respuesta</returns>
        [OperationContract]
        bool? GetLineBusinessByDescriptionById(string description, int id);

        /// <summary>
        /// Obtiene la lista de ramos técnicos filtrados por descripción o Id
        /// </summary>
        /// <param name="description">Descripción del ramo técnico</param>
        /// <param name="id">Id del ramo técnico</param>
        /// <returns>Lista de ramos técnicos</returns>
        [OperationContract]
        List<PARUPSM.LineBusinessServiceModel> GetLineBusinessServiceModelByDescriptionById(string description, int id);

        /// <summary>
        /// Obtiene la lista de ramos técnicos filtrados por descripción o tipo de riesgo cubierto
        /// </summary>
        /// <param name="description">Descripción del ramo técnico</param>
        /// <param name="coveredRiskType">Tipo de tiesgo cubierto del ramo técnico</param>
        /// <returns>Lista de ramos técnicos</returns>
        [OperationContract]
        List<PARUPSM.LineBusinessServiceModel> GetLineBusinessServiceModelByAdvancedSearch(string description, int coveredRiskType);

        [OperationContract]
        List<ClauseLevelServiceModel> GetClausesByLineBussinessId(int idLineBusiness);

        [OperationContract]
        List<PARUPSM.InsuredObjectServiceModel> GetInsuredObjectsByLineBussinessId(int idLineBusiness);

        [OperationContract]
        List<PARUPSM.PerilServiceModel> GetProtectionsByLineBussinessId(int idLineBusiness);

        /// <summary>
        /// Genera el reporte de ramos técnicos
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Modelo de archivo excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateLineBusinessServiceModel(string fileName);

        /// <summary>
        /// Consulta las clausulas relacionadas al level
        /// </summary>
        /// <param name="emissionLevel">nivel de la cláusula</param>
        /// <param name="conditionLevelId">Relación para correspondiente de la cláusula</param>
        /// <returns>Cláusulas - MOD-S</returns>
        [OperationContract]
        PARUPSM.ClausesServiceQueryModel GetClausesSQByEmissionLevelConditionLevelId(ENUMUD.EmissionLevel emissionLevel, int conditionLevelId);

        /// <summary>
        /// Listado de amparos
        /// </summary>
        /// <returns>Amparos - MOD-S</returns>
        [OperationContract]
        PARUSM.PerilsServiceQueryModel GetPerilsServiceQueryModel();

        /// <summary>
        /// Listado de objetos del seguro
        /// </summary>
        /// <returns>Objetos del seguro => MOD-S</returns>
        [OperationContract]
        PARUSM.InsuredObjectsServiceQueryModel GetInsuredObjectsServiceQueryModels();

        /// <summary>
        /// Obtener Grupos de Coberturas
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        [OperationContract]
        PARUSM.CoveredRiskTypesQueryServiceModel GetAllGroupCoverages();
        #endregion
        #region RatingZone

        /// <summary>
        /// Obtiene las zonas de tarifacion por el filtro
        /// </summary>
        /// <param name="ratingZoneCode">codigo de la zona de tarifacion</param>
        /// <param name="prefixId">id del ramo</param>
        /// <param name="filter">descripcion a buscar</param>
        /// <returns>Zonas de tarifacion MOD-S</returns>
        [OperationContract]
        PARUPSM.RatingZonesServiceModel GetRatingZoneServiceModel(int? ratingZoneCode, int? prefixId, string filter);

        /// <summary>
        /// CRUD de zonas de tarifacion
        /// </summary>
        /// <param name="ratingZoneServiceModels">zonas de tarifacion MOD-S</param>
        /// <returns>Listado de zonas de tarifacion de la operacion del CRUD</returns>
        [OperationContract]
        List<PARUPSM.RatingZoneServiceModel> ExecuteOperationsRatingZoneServiceModel(List<PARUPSM.RatingZoneServiceModel> ratingZoneServiceModels);

        /// <summary>
        /// Obtiene los paises
        /// </summary>
        /// <returns>Lista de paises</returns>
        [OperationContract]
        MODCO.CountriesServiceQueryModel GetCountries();

        /// <summary>
        /// Obtiene los estados por pais
        /// </summary>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        [OperationContract]
        MODCO.StatesServiceQueryModel GetStatesByCountry(int idCountry);

        /// <summary>
        /// Obtiene las cuidades por pais y estado
        /// </summary>
        /// <param name="idState">Identificador del estado</param>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        [OperationContract]
        MODCO.CitiesServiceRelationModel GetCitiesByStateCountry(int idState, int idCountry, int PrefixCode);

        /// <summary>
        /// Generar archivo excel de planes de pago
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToRatingZone(string fileName);

        [OperationContract]
        List<PARUPSM.RatingZoneServiceModel> GetRatingZone(int prefixId, int branchId);
        [OperationContract]
        void ExecuteOperationRatingZone(List<PARUPSM.RatingZoneServiceModel> ratingZoneService);
        #endregion
		
		#region Carrocería de vehículo
        /// <summary>
        /// Obtiene las Carrocerías
        /// </summary>
        /// <returns>Tipos de vehiculos</returns>
        [OperationContract]
        PARUPSM.VehicleBodiesServiceModel GetVehicleBodies();

        /// <summary>
        /// Ejecuta las operaciones de creacion, actualizacion y eliminacion
        /// </summary>
        /// <param name="vehicleBodies">Carrocería a realizar operacion</param>
        /// <returns>Listado de Carrocería</returns>
        [OperationContract]
        List<PARUPSM.VehicleBodyServiceModel> ExecuteOperationsVehicleBody(List<PARUPSM.VehicleBodyServiceModel> vehicleBodies);

        /// <summary>
        /// Genera el archivo para Carrocería
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta archivo</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToExportVehicleBody(string fileName);

        /// <summary>
        /// Genera el archivo para Carrocería
        /// </summary>
        /// <param name="vehicleBody">Tipo de vehiculo</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta de archivo</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToVehicleUse(PARUPSM.VehicleBodyServiceModel vehicleBody, string fileName);
        #endregion Carrocería de vehículo
		
		/// <summary>
        /// Obtiene las coverturas de 2G que hacen referencia a los objeto de seguro de vehiculos
        /// </summary>
        /// <returns>Coverturas 2G</returns>
        [OperationContract]
        PARUPSM.Coverages2GServiceModel GetCoverages2GByVehicleInsuredObject();

        #region FinancialPlan
        /// <summary>
        /// Consulta medios de pago
        /// </summary>
        /// <returns>Returna medios de pago</returns>
        [OperationContract]
        PARUPSM.PaymentMethodsServiceQueryModel GetMethodPaymentServiceModel();

        /// <summary>
        /// Consulta componentes
        /// </summary>
        /// <returns>Retorna lista componentes</returns>
        [OperationContract]
        PARUPSM.ComponentRelationsServiceModel GetComponentRelationServiceModel();

        /// <summary>
        /// Archivo excel plan financiero
        /// </summary>
        /// <returns>Lista de planes financieros</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToFinancialPlan();

        /// <summary>
        /// CRUD plan financiero
        /// </summary>
        /// <returns>planes financieros</returns>
        [OperationContract]
        PARUPSM.FinancialPlanServiceModel ExecuteOperationFinancialPlan(PARUPSM.FinancialPlanServiceModel financialPlanServiceModel);

        /// <summary>
        /// Consulta por item plan financiero
        /// </summary>
        /// <returns>planes financieros</returns>
        [OperationContract]
        PARUPSM.FinancialPlansServiceModel GetFinancialPlanForItems(int idPaymentPlan, int idPaymentMethod, int idCurrency);
        #endregion
		
		#region Limit Rc

        /// <summary>
        /// Obtener lista de limites rc
        /// </summary>
        /// <returns>Retorna LimitsRcServiceModel</returns>
        [OperationContract]
        PARUPSM.LimitsRcServiceModel GetLimitsRc();

        /// <summary>
        /// Validación de limite rc
        /// </summary>
        /// <param name="limitRcCode">codigo de limite rc</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns>
        [OperationContract]
        int ValidateLimitc(int limitRcCode);

        /// <summary>
        /// Generar archivo excel de limites Rc
        /// </summary>
        /// <param name="limitRcServiceModel">Modelo de LimitRcServiceModel</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Retorna Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToLimitRc(List<PARUPSM.LimitRcServiceModel> limitRcServiceModel, string fileName);

        #endregion

        #region "Metodos_Pago"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listPaymentMethodServiceModel"></param>
        /// <returns></returns>
        [OperationContract]
        List<PARUSM.PaymentMethodServiceModel> ExecuteOperationPaymentMethod(List<PARUSM.PaymentMethodServiceModel> listPaymentMethodServiceModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        PARUSM.PaymentMethodsServiceModel GetPaymentMethodByDescription(string description);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PARUSM.PaymentMethodTypeServiceQueryModel> GetPaymentMethodType();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        PARUSM.PaymentMethodsServiceModel GetPaymentMethod();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentPlans"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        ExcelFileServiceModel GenerateFileToPaymentMethod(List<PARUSM.PaymentMethodServiceModel> paymentPlans, string fileName);

        #endregion "Metodos_Pago"
        #region Technical Plan
        /// <summary>
        /// Obtiene listado de coberturas filtrado por Objeto del seguro
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        PARUPSM.CoveragesServiceModel GetCoveragesServiceModelByInsuredObject(int insuredObjectId);

        /// <summary>
        /// Obtiene listado de coberturas aliadas
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        [OperationContract]
        PARUPSM.AllyCoveragesServiceModel GetCoverageAlliedByCoverageId(int coverageId);


        /// <summary>
        /// Obtener lista de objetos del seguro
        /// </summary>
        /// <param name="coveredRiskType">Codigo de Tipo de Riesgo Cubierto</param>
        /// <returns>Obtiene el listado de objetos del seguro</returns> 
        [OperationContract]
        PARUPSM.InsurancesObjectsServiceModel GetInsuredObjectsByCoveredRiskType(int coveredRiskType);
        
        /// <summary>
        /// Obtener lista de Planes Técnicos
        /// </summary>
        /// <param name="description">criterio de busqueda por descripción</param>
        /// <param name="coveredRiskType">criterio de busqueda por tipo de riesgo cubierto</param>
        /// <returns>Obtiene el listado de Planes Técnicos</returns> 
        [OperationContract]
        PARUPSM.TechnicalPlansServiceQueryModel GetTechnicalPlanByDescriptionOrCoveredRiskType(string description, int coveredRiskType);
        /// <summary>
        /// Obtener lista de Coberturas
        /// </summary>
        /// <param name="technicalPlanId">criterio de busqueda por Id del Plan Técnico</param>
        /// <returns>Obtiene el listado de Coberturas</returns> 
        [OperationContract]
        PARUPSM.TechnicalPlanCoveragesServiceRelationModel GetCoveragesByTechnicalPlanId(int technicalPlanId);

        /// <summary>
        /// CRUD de Plan Técnico
        /// </summary>
        /// <param name="technicalPlan">Plan Técnico a modificar</param>
        /// <returns>resultado operacion Plan Técnico</returns>
        [OperationContract]
        PARUPSM.TechnicalPlanServiceModel ExecuteOperationTechnicalPlan(PARUPSM.TechnicalPlanServiceModel technicalPlan);

        /// <summary>
        /// Obtener lista de planes técnicos
        /// </summary>
        /// <returns>Obtiene el listado de planes tecnicos</returns>
        [OperationContract]
        PARUPSM.TechnicalPlansServiceModel GetTechnicalPlansServiceModel();

        /// <summary>
        /// Generar archivo excel de plan técnico
        /// </summary>
        /// <param name="technicalPlans">Listado de planes técnicos</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToTechnicalPlan(List<PARUPSM.TechnicalPlanServiceModel> technicalPlans, string fileName);
        #endregion

        /// <summary>
        /// Aceso a DB para consultar listado de plan de pagos asociados a la description
        /// </summary>
        /// <param name="description">descripción del plan de pago a consultar</param>
        /// <returns>Listado Result consulta en DB de Planes de pago</returns>
        [OperationContract]
        PARUPSM.PaymentPlansServiceModel GetPaymentPlansByDescription(string description);

      
    }
}
