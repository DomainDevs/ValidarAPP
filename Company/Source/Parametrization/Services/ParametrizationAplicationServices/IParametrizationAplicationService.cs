// -----------------------------------------------------------------------
// <copyright file="IParametrizationAplicationService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServices
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using Sistran.Company.Application.Utilities.DTO;    

    /// <summary>
    /// interfaz "IParametrizationAplicationService"
    /// </summary>  
    [ServiceContract]
    public interface IParametrizationAplicationService
    {
        /// <summary>
        /// Operacion de prueba
        /// </summary>
        /// <returns>String.Cadena de Texto</returns>
        [OperationContract]
        string Initial();

      

        #region BillingPeriod
        /// <summary>
        /// IGetApplicationBillingPeriod. Obtiene el Listado de BillingPeriod 
        /// </summary>
        /// <returns>BillingPeriodQueryDTO</returns>
        [OperationContract]
        BillingPeriodQueryDTO GetApplicationBillingPeriod();

        /// <summary>
        /// IGenerateFileToApplicationBillingPeriod. Genera el archivo de reporte en formato excel 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ExcelFileDTO</returns>
        [OperationContract]
        ExcelFileDTO GenerateFileToApplicationBillingPeriod(string fileName);
        #endregion

        #region BusinessType

        /// <summary>
        /// IGetApplicationBusinessType. Obtiene el Listado de BusinessTypeQuery 
        /// </summary>
        /// <returns>BusinessTypeQueryDTO</returns>
        [OperationContract]
        BusinessTypeQueryDTO GetApplicationBusinessType();

        /// <summary>
        /// IGenerateFileToApplicationBusinessType. Genera el archivo de reporte en formato excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ExcelFileDTO</returns>
        [OperationContract]
        ExcelFileDTO GenerateFileToApplicationBusinessType(string fileName);

        #endregion

        #region Ciudadades

        /// <summary>
        /// Crear una ciudad
        /// </summary>
        /// <param name="cityDTO">Objeto Ciudad DTO</param>
        /// <returns>CityQueryDTO. Objeto Ciudad DTO</returns>
        [OperationContract]
        CityDTO CreateApplicationCity(CityDTO cityDTO);

        /// <summary>
        /// Obtiene resultados de una busqueda avanzada de ciudades
        /// </summary>
        /// <param name="cityDTO">Objeto Ciudad DTO</param>
        /// <returns>CityQueryDTO. Objeto Ciudad DTO</returns>
        [OperationContract]
        CityQueryDTO GetApplicationCityAdv(CityDTO city);

        /// <summary>
        /// Listado de ciudades, carga inicial
        /// </summary>
        /// <returns>CityQueryDTO. Objeto Ciudad DTO</returns>
        [OperationContract]
        CityQueryDTO GetApplicationCity();
               

        /// <summary>
        /// Elimina el registro de una ciudad
        /// </summary>
        /// <param name="cityDTO"> Objeto Ciudad DTO</param>
        /// <returns>CityDTO.  Objeto Ciudad DTO</returns>
        [OperationContract]
        CityDTO DeleteApplicationCity(CityDTO cityDTO);

        /// <summary>
        /// GenerateFileToCity:Llamado a metodo para generar el archivo excel con las ciudades registradas en BD
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        ExcelFileDTO GenerateFileToCity(string fileName);

        /// <summary>
        /// Actualiza la informacion de una ciudad
        /// </summary>
        /// <param name="cityDTO"> Objeto ciudad DTO.</param>
        /// <returns>CityDTO.  Objeto Ciudad DTO</returns>
        [OperationContract]
        CityDTO UpdateApplicationCity(CityDTO cityDTO);

        /// <summary>
        /// conuslta del listado de las ciudaddes por description table comm.city
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [OperationContract]
        CityQueryDTO GetApplicationCityByDescription(string Description);
        #endregion

        #region SalesPoint
        /// <summary>
        /// Listado de ciudades, carga inicial
        /// </summary>
        /// <returns>CityQueryDTO. Objeto Ciudad DTO</returns>
        [OperationContract]
        bool ValidateExistIdSalesPoint(int IdSalesPoint, int BranchId);
        #endregion
    }
}
