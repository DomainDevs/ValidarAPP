// -----------------------------------------------------------------------
// <copyright file="ICompanyUnderwritingParamApplicationService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UnderwritingParamApplicationService
{
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using System.Collections.Generic;
    using Sistran.Company.Application.Utilities.DTO;
    using System.ServiceModel;
    using Sistran.Company.Application.UnderwritingServices.Models;

    /// <summary>
    /// ICompanyUnderwritingParamApplicationService. Interfaz De servicio de aplicación
    /// </summary>
    [ServiceContract]
    public interface ICompanyUnderwritingParamApplicationService
    {

        #region VehicleType_Previsora
        [OperationContract]
        List<VehicleTypeDTO> ExecuteOperationsApplicationVehicleType(List<VehicleTypeDTO> vehicleTypesDTO);

        [OperationContract]
        List<VehicleTypeDTO> GetApplicationVehicleTypes();

        [OperationContract]
        string GenerateFileToApplicationVehicleType(string fileName);

        [OperationContract]
        string GenerateFileToApplicationVehicleBody(VehicleTypeDTO vehicleTypeDTO, string fileName);

        #endregion

        #region Prima Minima

        [OperationContract]
        MinPremiunRelationDTO CreateApplicationMinPremiunRelation(MinPremiunRelationDTO MinPremiunRelationDTO);

        [OperationContract]
        MinPremiunRelationQueryDTO GetApplicationMinPremiunRelationByPrefixIdAndProductName(int PrefixId, string ProductName);

        [OperationContract]
        MinPremiunRelationQueryDTO GetApplicationMinPremiunRelation();

        [OperationContract]
        MinPremiunRelationDTO UpdateApplicationMinPremiunRelation(MinPremiunRelationDTO MinPremiunRelationDTO);

        [OperationContract]
        MinPremiunRelationDTO DeleteApplicationMinPremiunRelation(MinPremiunRelationDTO MinPremiunRelationDTO);

        [OperationContract]
        string GenerateFileToMinPremiunRelation(string fileName);

        [OperationContract]
        List<CoverageDTO> GetCoverageByPrefixId(int PrefixId);


        [OperationContract]
        List<CoverageDTO> GetAllMinRange();
        #endregion

        #region AllyCoverage

        //[OperationContract]
        //AllyCoverageQueryDTO GetAplicationAllyCoverageAdv(AllyCoverageDTO allyCoverage);

        /// <summary>
        /// Obtiene resultados de una busqueda avanzada de Coberturas aliadas
        /// </summary>
        /// <param name="allyCoverageQueryDTO">Objeto Cobertura aliada DTO</param>
        /// <returns>allyCoverageQueryDTO. Objeto Cobertura aliada DTO</returns>
        [OperationContract]
        AllyCoverageQueryDTO GetAplicationAllyCoverageAdv(AllyCoverageQueryDTO allyCoverageQueryDTO);

        [OperationContract]
        AllyCoverageQueryDTO GetAplicationAllyCoverageByDescription(string data, int num);

        [OperationContract]
        ExcelFileDTO GenerateFileAplicationToAllyCoverage(string fileName);

        [OperationContract]
        ExcelFileDTO GenerateFileAplicationToAllyCoverageList(List<QueryAllyCoverageDTO> li_allyCoverage, string fileName);

        [OperationContract]
        AllyCoverageQueryDTO GetAplicationAllyCoverage();

        [OperationContract]
        AllyCoverageDTO CreateAplicationAllyCoverage(AllyCoverageDTO allyCoverage);

        [OperationContract]
        AllyCoverageDTO UpdateAplicationAllyCoverage(AllyCoverageDTO allyCoverage, AllyCoverageDTO allyCoverageOld);

        [OperationContract]
        AllyCoverageDTO DeleteAplicationAllyCoverage(AllyCoverageDTO allyCoverage);

        [OperationContract]
        AllyCoverageQueryDTO GetAplicationAllyCoveragePrincipal();

        [OperationContract]
        AllyCoverageQueryDTO GetAplicationCoverageAlly(int position);
        
        #endregion

        #region Coverage
        /// <summary>
        /// CreateApplicationCoCoverageValue: metodo que crea un registro de pesos de cobertura - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoCoverageValueDTO CreateApplicationCoCoverageValue(CoCoverageValueDTO coCoverageValueDTO);

        /// <summary>
        /// GetApplicationCoCoverageValueByDescription: metodo de consulta del listado de pesos de coberturas a partir del prefixId - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [OperationContract]
        CoCoverageValueQueryDTO GetApplicationCoCoverageValueByPrefixId(int prefixId);


        /// <summary>
        /// GenerateFileApplicationToCoCoverage: Metodo que genera el archivo excel del listado de pesos de coberturas - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ExcelFileDTO GenerateFileApplicationToCoCoverage(string fileName);

        /// <summary>
        /// GetApplicationCoCoverageValue: Metodo que consulta el listado completo de pesos de cobertura - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoCoverageValueQueryDTO GetApplicationCoCoverageValue();

        /// <summary>
        /// UpdateApplicationCoCoverageValue: metodo que actualiza el registro de pesos de cobertura - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoCoverageValueDTO UpdateApplicationCoCoverageValue(CoCoverageValueDTO coCoverageValueDTO);

        /// <summary>
        /// DeleteApplicationCoCoverageValue: Metdodo que elimina el registro de pesos de coberturas - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoCoverageValueDTO DeleteApplicationCoCoverageValue (CoCoverageValueDTO coCoverageValueDTO);

        /// <summary>
        /// GetApplicationCoCoverageValueAdv: metodo que consulta el listado de pesos de coberturas a partir de los filtros ingresados en la busqueda avanzada - tabla: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoCoverageValueQueryDTO GetApplicationCoCoverageValueAdv(CoCoverageValueDTO coCoverageValueDTO);

        /// <summary>
        /// GetApplicationCoverageByPrefixId: Metodo que consulta listado de coberturas por prefixId
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetApplicationCoverageByPrefixId(int prefixId);
        #endregion

        #region Condition Text
        /// <summary>
        /// CreateApplicationConditiontext. Crea un nuevo Texto Precatalogado
        /// </summary>
        /// <param name="conditionText">Modelo DTO Conditional Text.</param>
        /// <returns>ConditionTextDTO. Modelo DTO Conditional Text.</returns>
        [OperationContract]
        ConditionTextDTO CreateApplicationConditiontext(ConditionTextDTO conditionText);

        /// <summary>
        /// UpdateApplicationConditiontext. Edita un nuevo Texto Precatalogado.
        /// </summary>
        /// <param name="conditionText">Modelo DTO Conditional Text.</param>
        /// <returns>ConditionTextDTO. Modelo DTO Conditional Text.</returns>
        
        [OperationContract]
        ConditionTextDTO UpdateApplicationConditiontext(ConditionTextDTO conditionText);

        /// <summary>
        /// DeleteApplicationConditiontext. Elimina un nuevo Texto Precatalogado.
        /// </summary>
        /// <param name="conditionText">Modelo DTO Conditional Text.</param>
        /// <returns>ConditionTextDTO. Modelo DTO Conditional Text.</returns>
        [OperationContract]
        string DeleteApplicationConditiontext(ConditionTextDTO conditionText);

        /// <summary>
        /// GetApplicationConditiontext. Retorna Lista de textos precatalogados
        /// </summary>
        /// <returns>ConditionTextQueryDTO. Lista Modelos DTO Conditional Text.</returns>
        [OperationContract]
        ConditionTextQueryDTO GetApplicationConditiontext();

        /// <summary>
        /// GetApplicationConditiontextByDescription. Retorna Lista de textos precatalogados a partir de Id o descripción.
        /// </summary>
        /// <param name="integer">Id Texto Precatalogado</param>
        /// <param name="description">Descripción Texto Precatalogado</param>
        /// <returns>ConditionTextQueryDTO. Lista Modelos DTO Conditional Text.</returns>
        [OperationContract]
        ConditionTextQueryDTO GetApplicationConditiontextByDescription(string description="");

        /// <summary>
        /// ExcelFileDTO. Retorna objeto Excel DTO.        
        /// </summary>
        /// <returns>ExcelFileDTO. Objeto Excel DTO</returns>
        [OperationContract]
        ExcelFileDTO GenerateFileApplicationToConditiontext(string File);
        #endregion

        #region Tax

        #region TaxMethods
        /// <summary>
        /// CreateApplicationTax: metodo que inserta un impuesto nuevo
        /// </summary>
        /// <param name="TaxDTO"></param>
        /// <returns>mappedTaxDTO</returns>
        [OperationContract]
        TaxDTO CreateApplicationTax(TaxDTO TaxDTO);

        /// <summary>
        /// UpdateApplicationTax: metodo que actualiza un impuesto creado
        /// </summary>
        /// <param name="TaxDTO"></param>
        /// <returns>mappedTaxDTO</returns>
        [OperationContract]
        TaxDTO UpdateApplicationTax(TaxDTO TaxDTO);


        /// <summary>
        /// GetApplicationBusinessTaxByDescription: metodo que consulta un impuesto en BD por descripcion
        /// </summary>
        /// <param name="TaxDescription"></param>
        /// <returns>mappedTaxQueryDTO</returns>
        [OperationContract]
        TaxQueryDTO GetApplicationTaxByDescription(string TaxDescription);


        /// <summary>
        /// GetApplicationTaxById: metodo que consulta un impuesto en BD por id y descripcion
        /// </summary>
        /// <param name="taxId"></param>
        /// <param name="taxDescription"></param>
        /// <returns>mappedTaxQueryDTO</returns>
        [OperationContract]
        TaxQueryDTO GetApplicationTaxByIdAndDescription(int taxId, string taxDescription);


        /// <summary>
        /// GenerateFileApplicationToTax: metodo que genera un excel de impuesto
        /// </summary>
        /// <returns>ExcelFileDTO</returns>
        [OperationContract]
        ExcelFileDTO GenerateFileApplicationToTax();
        #endregion

        #region TaxRateMethods

        /// <summary>
        /// CreateApplicationTaxRate: metodo que inserta una tasa de impuesto nuevo
        /// </summary>
        /// <param name="TaxRateDTO"></param>
        /// <returns>mappedTaxRateDTO</returns>
        [OperationContract]
        TaxRateDTO CreateApplicationTaxRate(TaxRateDTO taxRateDTO);

        /// <summary>
        /// UpdateApplicationTax: metodo que actualiza una tasa de impuesto creado
        /// </summary>
        /// <param name="TaxRateDTO"></param>
        /// <returns>mappedTaxRateDTO</returns>
        [OperationContract]
        TaxRateDTO UpdateApplicationTaxRate(TaxRateDTO taxRateDTO);


        /// <summary>
        /// GetApplicationBusinessTaxRateByTaxId: metodo que consulta una tasa de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>mappedTaxRateQueryDTO</returns>
        [OperationContract]
        TaxRateQueryDTO GetApplicationTaxRateByTaxId(int TaxId);

        [OperationContract]
        TaxRateDTO getApplicationTaxRateByTaxIdByAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId);
        [OperationContract]
        TaxRateDTO GetApplicationTaxRateById(int taxRateId);
        #endregion


        #region TaxCategory Methods

        /// <summary>
        /// CreateApplicationTaxCategory: metodo que inserta una categoria de impuesto nuevo
        /// </summary>
        /// <param name="TaxCategoryDTO"></param>
        /// <returns>mappedTaxCategoryDTO</returns>
        [OperationContract]
        TaxCategoryDTO CreateApplicationTaxCategory(TaxCategoryDTO taxCategoryDTO);

        /// <summary>
        /// UpdateApplicationTaxCategory: metodo que actualiza una categoria de impuesto creado
        /// </summary>
        /// <param name="TaxCategoryDTO"></param>
        /// <returns>mappedTaxCategoryDTO</returns>
        [OperationContract]
        TaxCategoryDTO UpdateApplicationTaxCategory(TaxCategoryDTO taxCategoryDTO);


        /// <summary>
        /// GetApplicationTaxCategoriesByTaxId: metodo que consulta una lista de categoria de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>mappedTaxCategoriesQueryDTO</returns>
        [OperationContract]
        TaxCategoryQueryDTO GetApplicationTaxCategoriesByTaxId(int TaxId);


        /// <summary>
        /// DeleteApplicationTaxCategoriesByTaxId: metodo que borra una lista de categorias de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>bool CategoriesDeleted</returns>
        [OperationContract]
        bool DeleteApplicationTaxCategoriesByTaxId(int categoryId, int taxId);


        #endregion

        #region TaxCondition Methods

        /// <summary>
        /// CreateApplicationTaxCondition: metodo que inserta una condicion de impuesto nuevo
        /// </summary>
        /// <param name="taxConditionDTO"></param>
        /// <returns>mappedTaxConditionDTO</returns>
        [OperationContract]
        TaxConditionDTO CreateApplicationTaxCondition(TaxConditionDTO taxConditionDTO);

        /// <summary>
        /// UpdateApplicationTaxCondition: metodo que actualiza una condicion de impuesto creado
        /// </summary>
        /// <param name="TaxConditionDTO"></param>
        /// <returns>mappedTaxConditionDTO</returns>
        [OperationContract]
        TaxConditionDTO UpdateApplicationTaxCondition(TaxConditionDTO taxConditionDTO);


        /// <summary>
        /// GetApplicationTaxConditionsByTaxId: metodo que consulta una lista de condiciones de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>mappedTaxConditionsQueryDTO</returns>
        [OperationContract]
        TaxConditionQueryDTO GetApplicationTaxConditionsByTaxId(int TaxId);

        /// <summary>
        /// DeleteApplicationTaxCategoriesByTaxId: metodo que borra una lista de condiciones de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>bool ConditionsDeleted</returns>
        [OperationContract]
        bool DeleteApplicationTaxConditionsByTaxId(int conditionId, int taxId);
        #endregion

        #endregion

    }
}
