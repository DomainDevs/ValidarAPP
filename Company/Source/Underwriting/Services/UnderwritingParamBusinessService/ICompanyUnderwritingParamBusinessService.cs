    // -----------------------------------------------------------------------
// <copyright file="ICompanyUnderwritingParamBusinessService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UnderwritingParamBusinessService
{
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Company.Application.Utilities.DTO;
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    ///     public interface ICompanyUnderwritingParamBusinessService. Proveedor del servicio de aplicación.
    /// </summary>
    [ServiceContract]
    public interface ICompanyUnderwritingParamBusinessService
    {
        #region VehicleType_Previsora
    
        [OperationContract]
        List<CompanyVehicleType> ExecuteOperationsBusinessVehicleType(List<CompanyVehicleType> vehicleTypes);

        [OperationContract]
        List<CompanyVehicleType> GetBusinessVehicleTypes();

        [OperationContract]
        string GenerateFileToBusinessVehicleType(string fileName);

        [OperationContract]
        string GenerateFileToBusinessVehicleBody(CompanyVehicleType vehicleType, string fileName);

         #endregion

        [OperationContract]
        CompanyParamMinPremiunRelation CreateApplicationMinPremiunRelation(CompanyParamMinPremiunRelation MinPremiunRelationDTO);

        [OperationContract]
        List<CompanyParamMinPremiunRelation> GetApplicationMinPremiunRelationByPrefixIdAndProductName(int PrefixId, string ProductName);

        [OperationContract]
        List<CompanyParamMinPremiunRelation> GetApplicationMinPremiunRelation();

        [OperationContract]
        CompanyParamMinPremiunRelation UpdateApplicationMinPremiunRelation(CompanyParamMinPremiunRelation MinPremiunRelationDTO);

        [OperationContract]
        string DeleteApplicationMinPremiunRelation(CompanyParamMinPremiunRelation MinPremiunRelationDTO);

        [OperationContract]
        List<CompanyParamCoverage> GetCoverageByPrefixId(int PrefixId);

        #region Coverage

        /// <summary>
        /// CreateBusinessCoCoverageValue: metodo que inserta una cobertura
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyParamCoCoverageValue CreateBusinessCoCoverageValue(CompanyParamCoCoverageValue companyParamCoCoverageValue);

        /// <summary>
        /// GetBusinessCoverageValueAdv: metodo que consulta el listado de coberturas a partir de los filtros ingresados en la busqueda avanzada
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyParamCoCoverageValue> GetBusinessCoverageValueAdv(CompanyParamCoCoverageValue companyParamCoCoverageValue);

        /// <summary>
        /// GetBusinessCoverageValueByDescription: metodo que consulta el listado de coberturas a partir de la descripcion ingresada en la busqueda simple
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyParamCoCoverageValue> GetBusinessCoverageValueByPrefixId(int prefixId);

        /// <summary>
        /// GenerateFileBusinessToCoCoverageValue: Metodo que genera el archivo excel del listado de las coberturas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CompanyExcel GenerateFileBusinessToCoCoverageValue(string fileName);

        /// <summary>
        /// GetBusinessCoCoverageValue: metodo que consulta el listado completo de las coberturas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyParamCoCoverageValue> GetBusinessCoCoverageValue();

        /// <summary>
        /// UpdateBusinessCocoVerageValue: metodo que actualiza la informacion de una cobertura
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyParamCoCoverageValue UpdateBusinessCocoVerageValue (CompanyParamCoCoverageValue companyParamCoCoverageValue);

        /// <summary>
        /// DeleteBusinessCocoVerageValue: metodo que elimina el registro de una cobertura
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        string DeleteBusinessCocoVerageValue(CompanyParamCoCoverageValue companyParamCoCoverageValue);

        /// <summary>
        /// GetCoverageByPrefixId: metodo que consulta listado de coberturas por linebusiness y prefixId
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyParamCoverage> GetBusinessCoverageByPrefixId(int prefixId);

        #endregion

        #region AllyCoverage
        [OperationContract]
        CompanyParamAllyCoverage GetBusinessAllyCoverage();

        [OperationContract]
        CompanyParamAllyCoverage CreateBusinessAllyCoverage(CompanyParamAllyCoverage allyCoverage);

        [OperationContract]
        CompanyParamAllyCoverage UpdateBusinessAllyCoverage(CompanyParamAllyCoverage companyParamAllyCoverage, CompanyParamAllyCoverage companyParamAllyCoverageold);

        [OperationContract]
        CompanyParamAllyCoverage DeleteBusinessAllyCoverage(CompanyParamAllyCoverage allyCoverage);

        [OperationContract]
        CompanyExcel GenerateFileToAllyCoverage(string fileName);

        [OperationContract]
        CompanyExcel GenerateFileToAllyCoverageList(List<CompanyParamQueryAllyCoverage> li_allyCoverage, string fileName);
        #endregion

        #region ConditionalText
        /// <summary>
        /// CreateApplicationConditiontext. Crea un nuevo Texto Precatalogado
        /// </summary>
        /// <param name="conditionText">Modelo Company Conditional Text.</param>
        /// <returns>CompanyParamConditionText. Modelo Company Conditional Text.</returns>
        [OperationContract]
        CompanyParamConditionText CreateBusinessConditiontext(CompanyParamConditionText conditionText);

        /// <summary>
        /// UpdateBusinessConditiontext. Edita un nuevo Texto Precatalogado.
        /// </summary>
        /// <param name="conditionText">Modelo Company Conditional Text.</param>
        /// <returns>ConditionTextDTO. Modelo Company Conditional Text.</returns>

        [OperationContract]
        CompanyParamConditionText UpdateBusinessConditiontext(CompanyParamConditionText conditionText);

        /// <summary>
        /// DeleteBusinessConditiontext. Elimina un nuevo Texto Precatalogado.
        /// </summary>
        /// <param name="conditionText">Modelo Company Conditional Text.</param>
        /// <returns>ConditionTextDTO. Modelo Company Conditional Text.</returns>
        [OperationContract]
        string DeleteBusinessConditiontext(CompanyParamConditionText conditionText);

        /// <summary>
        /// GetBusinessConditiontext. Retorna Lista de textos precatalogados
        /// </summary>
        /// <returns>List<CompanyParamConditionText>. Lista Modelos Company Conditional Text.</returns>
        [OperationContract]
        List<CompanyParamConditionText> GetBusinessConditiontext();

        /// <summary>
        /// GetBusinessConditiontextByDescription. Retorna Lista de textos precatalogados a partir de Id o descripción.
        /// </summary>
        /// <param name="integer">Id Texto Precatalogado</param>
        /// <param name="description">Descripción Texto Precatalogado</param>
        /// <returns>List<CompanyParamConditionText>. Lista Modelos DTO Conditional Text.</returns>
        [OperationContract]
        List<CompanyParamConditionText> GetBusinessConditiontextByDescription( string description = "");

        /// <summary>
        /// ExcelFileDTO. Retorna objeto Excel DTO.        
        /// </summary>
        /// <returns>ExcelFileDTO. Objeto Excel DTO</returns>
        [OperationContract]
        CompanyExcel GenerateFileBusinessToConditiontext(string File);
        #endregion

        #region Tax

        #region TaxMethods
        /// <summary>
        /// CreateBusinessTax: metodo que inserta un impuesto nuevo
        /// </summary>
        /// <param name="CompanyParamTax"></param>
        /// <returns>mappedCompanyParamTax</returns>
        [OperationContract]
        CompanyParamTax CreateBusinessTax(CompanyParamTax Tax);


        /// <summary>
        /// UpdateBusinessTax: metodo que actualiza un impuesto creado
        /// </summary>
        /// <param name="mappedCompanyParamTax"></param>
        /// <returns>mappedCompanyParamTax</returns>
        [OperationContract]
        CompanyParamTax UpdateBusinessTax(CompanyParamTax Tax);


        /// <summary>
        /// GetBusinessTaxByDescription: metodo que busca una lista de impuestos por descripcion
        /// </summary>
        /// <param name="TaxDescription"></param>
        /// <returns>mappedCompanyParamTaxList</returns>
        [OperationContract]
        List<CompanyParamTax> GetBusinessTaxByDescription(string TaxDescription);


        /// <summary>
        /// GetBusinessTaxByDescription: metodo que busca una lista de impuestos por id
        /// </summary>
        /// <param name="taxId"></param>
        /// <param name="taxDescription"></param>
        /// <returns>mappedCompanyParamTaxList</returns>
        [OperationContract]
        List<CompanyParamTax> GetBusinessTaxByIdAndDescription(int taxId, string Description);


        /// <summary>
        /// GenerateFileBusinessToTax: metodo que genera un excel de impuesto
        /// </summary>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateFileBusinessToTax();

        #endregion

        #region TaxRateMethods

        /// <summary>
        /// CreateBusinessTaxRate: metodo que inserta una tasa de impuesto nuevo
        /// </summary>
        /// <param name="CompanyParamTaxRate"></param>
        /// <returns>mappedCompanyParamTaxRate</returns>
        [OperationContract]
        CompanyParamTaxRate CreateBusinessTaxRate(CompanyParamTaxRate TaxRate);


        /// <summary>
        /// UpdateBusinessTaxRate: metodo que actualiza una tasa de impuesto creado
        /// </summary>
        /// <param name="CompanyParamTaxRate"></param>
        /// <returns>mappedCompanyParamTaxRate</returns>
        [OperationContract]
        CompanyParamTaxRate UpdateBusinessTaxRate(CompanyParamTaxRate TaxRate);


        /// <summary>
        /// GetBusinessTaxRateByTaxId: metodo que consulta una tasa de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>mappedCompanyParamTaxRateList</returns>
        [OperationContract]
        List<CompanyParamTaxRate> GetBusinessTaxRateByTaxId(int TaxId);

        #endregion

        #region TaxCategoryMethods

        /// <summary>
        /// CreateBusinessTaxCategory: metodo que inserta una categoria de impuesto nuevo
        /// </summary>
        /// <param name="CompanyParamTaxCategory"></param>
        /// <returns>mappedCompanyParamTaxCategory</returns>
        [OperationContract]
        CompanyParamTaxCategory CreateBusinessTaxCategory(CompanyParamTaxCategory companyTaxCategory);


        /// <summary>
        /// UpdateBusinessTaxCategory: metodo que actualiza una categoria de impuesto creado
        /// </summary>
        /// <param name="CompanyParamTaxCategory"></param>
        /// <returns>mappedCompanyParamTaxCategory</returns>
        [OperationContract]
        CompanyParamTaxCategory UpdateBusinessTaxCategory(CompanyParamTaxCategory companyTaxCategory);


        /// <summary>
        /// GetBusinessTaxCategoryByTaxId: metodo que consulta una lista de categoria de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>mappepCompanyParamTaxCategoryList</returns>
        [OperationContract]
        List<CompanyParamTaxCategory> GetBusinessTaxCategoriesByTaxId(int TaxId);

        /// <summary>
        /// DeleteBusinessTaxCategoriesByTaxId: metodo que borra una lista de categorias de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>bool CategoriesDeleted</returns>
        [OperationContract]
        bool DeleteBusinessTaxCategoriesByTaxId(int categoryId, int TaxId);

        #endregion

        #region TaxConditionMethods

        /// <summary>
        /// CreateBusinessTaxCondition: metodo que inserta una condicion de impuesto nuevo
        /// </summary>
        /// <param name="CompanyParamTaxCondition"></param>
        /// <returns>mappedCompanyParamTaxCondition</returns>
        [OperationContract]
        CompanyParamTaxCondition CreateBusinessTaxCondition(CompanyParamTaxCondition companyTaxCondition);


        /// <summary>
        /// UpdateBusinessTaxCondition: metodo que actualiza una condicion de impuesto creado
        /// </summary>
        /// <param name="CompanyParamTaxCondition"></param>
        /// <returns>mappedCompanyParamTaxCondition</returns>
        [OperationContract]
        CompanyParamTaxCondition UpdateBusinessTaxCondition(CompanyParamTaxCondition companyTaxCondition);


        /// <summary>
        /// GetBusinessTaxConditionsByTaxId: metodo que consulta una lista de condiciones de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>mappepCompanyParamTaxConditionList</returns>
        [OperationContract]
        List<CompanyParamTaxCondition> GetBusinessTaxConditionsByTaxId(int TaxId);


        /// <summary>
        /// DeleteBusinessTaxConditionsByTaxId: metodo que borra una lista de condiciones de impuesto por id de impuesto
        /// </summary>
        /// <param name="TaxId"></param>
        /// <returns>bool ConditionsDeleted</returns>
        [OperationContract]
        bool DeleteBusinessTaxConditionsByTaxId(int conditionId, int TaxId);

        #endregion

        #endregion
    }
}
