// -----------------------------------------------------------------------
// <copyright file="IParametrizationParamBusinessService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationParamBusinessService
{
    using System.Collections.Generic;
    using System.Data;
    using System.ServiceModel;
    using ParametrizationParamBusinessService.Model;



    /// <summary>
    /// Interfaz IParametrizationParamBusinessService
    /// </summary>
    [ServiceContract]
    public interface IParametrizationParamBusinessService
    {

        #region Parameter
        /// <summary>
        /// GetParameterByParameterId. 
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns>CompanyParameters</returns>   
        [OperationContract]
        CompanyParameters GetParameterByParameterId(int parameterId);
        #endregion

        #region BillingPeriod
        /// <summary>
        /// IGetBusinessBillingPeriod. Obtiene el Listado de CompanyParamBillingPeriod 
        /// </summary>
        /// <returns>List<CompanyParamBillingPeriod></returns>
        [OperationContract]
        List<CompanyParamBillingPeriod> GetBusinessBillingPeriod();

        /// <summary>
        /// GenerateFileToBusinessBillingPeriod. Genera el archivo de reporte en formato excel 
        /// </summary>
        /// <param name="BillingPeriodList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToBusinessBillingPeriod(List<CompanyParamBillingPeriod> BillingPeriodList, string fileName);
        #endregion

        #region BusinessType
        /// <summary>
        /// iGetBusinessBusinessType. Obtiene el Listado de CompanyParamBusinessType 
        /// </summary>
        /// <returns>List<CompanyParamBusinessType></returns>
        [OperationContract]
        List<CompanyParamBusinessType> GetBusinessBusinessType();

        /// <summary>
        /// GenerateFileToBusinessBusinessType. Genera el archivo de reporte en formato excel 
        /// </summary>
        /// <param name="BusinessTypeList"></param>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateFileToBusinessBusinessType(List<CompanyParamBusinessType> BusinessTypeList, string fileName);
        #endregion

        #region city
        /// <summary>
        /// Registra ciudades
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyParamCity CreateBusinessCity(CompanyParamCity companyParamCity);

       
        /// <summary>
        /// Obtiene el listado de ciudades par ala busqued aavanzada
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyParamCity> GetBusinessCityAdv(CompanyParamCity companyParamCity);

        
        /// <summary>
        ///Obiente el listado total de las ciudades 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyParamCity> GetBusinessCity();

        /// <summary>
        /// Actualiza la informacion de una ciudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyParamCity UpdateBusinessCity(CompanyParamCity companyParamCity);

        /// <summary>
        /// Elimina reistro de una ciiudad
        /// </summary>
        /// <param name="companyParamCity"></param>
        /// <returns></returns>
        [OperationContract]
        string DeleteBusinessCity(CompanyParamCity companyParamCity);

   
        [OperationContract]
        CompanyExcel GenerateFileToCity(string fileName);

        #endregion

        #region SalesPoint
        /// <summary>
        /// GenerateFileToBusinessBillingPeriod. Genera el archivo de reporte en formato excel 
        /// </summary>
        /// <param name="IdSalesPoint"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateSalesPointIdBusiness(int IdSalesPoint, int BranchId);
        #endregion

       

    }

}
