// -----------------------------------------------------------------------
// <copyright file="ICommonParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Common;
    using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
    using PARCPSM = Sistran.Core.Application.ModelServices.Models.CommonParam;

    /// <summary>
    /// Interfaz de parametrización.
    /// </summary>
    [ServiceContract]
    public interface ICommonParamServiceWebCore
    {
        /// <summary>
        /// Obtiene lista de parametros
        /// </summary>
        /// <param name="ltsParameterSMo">lista modelo ParamParameter</param>
        /// <returns>lista de modelo ParameterServiceModel</returns>
        [OperationContract]
        List<ParameterServiceModel> GetParameter(List<ParameterServiceModel> ltsParameterSMo);

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="parameterServiceModel">Objeto de ParamParameter</param>
        /// <returns>Objeto de ParamParameter</returns>
        [OperationContract]
        List<ParameterServiceModel> ExecuteOperationsParameterServiceModel(List<ParameterServiceModel> parameterServiceModel);

        /// <summary>
        /// Llamado a DAOS respectivos para operacion del CRUD y operacion con result
        /// </summary>
        /// <param name="paramParameter">Modelo ParamParameter</param>
        /// <param name="paramDiscontinuityLog">Modelo ParamDiscontinuityLog</param>
        /// <param name="paramInfringementLog">Modelo ParamInfringementLog</param>
        /// <param name="statusTypeService">Estado de tipo de servicio</param>
        /// <returns>Modelo ParameterServiceModel</returns>
        [OperationContract]
        ParameterServiceModel OperationParameterServiceModel(ParamParameter paramParameter, ParamDiscontinuityLog paramDiscontinuityLog, ParamInfringementLog paramInfringementLog, StatusTypeService statusTypeService);

        #region Branch
        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <returns>Objeto de ParamParameter</returns>
        [OperationContract]
        PARCPSM.BranchesServiceQueryModel GetBranch();

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <returns>Objeto de ParamParameter</returns>
        [OperationContract]
        PARCPSM.BranchesServicesModel GetBranches();

        /// <summary>
        /// Generar archivo excel de sucursal
        /// </summary>
        /// <param name="branchServiceModel">Listado de sucursal</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToBranch(List<PARCPSM.BranchServiceQueryModel> branchServiceModel, string fileName);

        /// <summary>
        /// Obtener lista de sucursal
        /// </summary>
        /// <param name="description">Descripcion de sucursal</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de sucursal</returns>
        [OperationContract]
        PARCPSM.BranchesServiceQueryModel GetBranchesByDescription(string description);

        /// <summary>
        /// Validaciones dependencias entidad sucursales
        /// </summary>
        /// <param name="branchId">Codigo desucursal</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        [OperationContract]
        int ValidateBranch(int branchId);

        /// <summary>
        /// ejecuta el crud para sucursales
        /// </summary>
        /// <param name="branchServiceModel">lista de modelo de servicio de sucursales</param>
        /// <returns>lista del modelo de servicio de sucursales</returns>
        [OperationContract]
        List<PARCPSM.BranchServiceModel> ExecuteOperationsBranchServiceModel(List<PARCPSM.BranchServiceModel> branchServiceModel);

        /// <summary>
        /// Se consultan las CO-sucursales por descripcion
        /// </summary>
        /// <param name="description">descripcion</param>
        /// <returns>retorna las CO-sucursales</returns>
        [OperationContract]
        PARCPSM.BranchesServicesModel GetCoBranchesByDescription(string description);
        #endregion

        #region SalePoint
        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <returns>Objeto de ParamParameter</returns>
        [OperationContract]
        PARCPSM.SalePointsServiceModel GetSalePointes();

        /// <summary>
        /// Generar archivo excel de puntos de venta
        /// </summary>
        /// <param name="salePointServiceModel">Listado de puntos de venta</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        MODSM.ExcelFileServiceModel GenerateFileToSalePoint(List<PARCPSM.SalePointServiceModel> salePointServiceModel, string fileName);

        /// <summary>
        /// Obtener lista de sucursal
        /// </summary>
        /// <param name="description">Descripcion de puntos de venta</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de puntos de venta</returns>
        [OperationContract]
        PARCPSM.SalePointsServiceModel GetSalePointServiceModel(string description);

        /// <summary>
        /// Obtener lista de sucursal
        /// </summary>
        /// <param name="description">id sucursal del punto de venta</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de puntos de venta</returns>       
        [OperationContract]
        PARCPSM.SalePointsServiceModel GetSalePointsByBranchCode(int branchId);

        /// <summary>
        /// Validaciones dependencias entidad de puntos de venta
        /// </summary>
        /// <param name="salePointId">Codigo punto de venta</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        [OperationContract]
        int ValidateSalePoint(int salePointId, int branchId);
        #endregion

        #region Country State City
        /// <summary>
        /// Obtiene los paises
        /// </summary>
        /// <returns>Lista de paises</returns>
        [OperationContract]
        PARCPSM.CountriesServiceQueryModel GetCountries();

        /// <summary>
        /// Obtiene los estados por pais
        /// </summary>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        [OperationContract]
        PARCPSM.StatesServiceQueryModel GetStatesByCountry(int idCountry);

        /// <summary>
        /// Obtiene las cuidades por pais y estado
        /// </summary>
        /// <param name="idState">Identificador del estado</param>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        [OperationContract]
        PARCPSM.CitiesServiceRelationModel GetCitiesByStateCountry(int idState, int idCountry);

        #endregion

        #region Common data
        /// <summary>
        /// Obtiene los tipo de teléfono
        /// </summary>
        /// <returns>Lista de tipo de teléfono</returns>
        [OperationContract]
        PARCPSM.PhonesTypesServiceQueryModel GetPhoneType();

        /// <summary>
        /// Obtiene los tipos de direcciones
        /// </summary>
        /// <returns>Lista de tipos de direcciones</returns>
        [OperationContract]
        PARCPSM.AddressTypesServiceQueryModel GetAddressType();


        /// <summary>
        /// GetVehicleConcessionaires
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ParamVehicleConcessionaire> GetVehicleConcessionaires();

        #endregion
    }
}