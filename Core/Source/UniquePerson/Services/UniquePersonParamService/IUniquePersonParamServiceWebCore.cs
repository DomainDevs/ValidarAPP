// -----------------------------------------------------------------------
// <copyright file="IUniquePersonParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamService
{
    using System;
    using System.ServiceModel;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    using Sistran.Core.Application.ModelServices.Models.UniquePerson;


    /// <summary>
    /// Interfaz de parametrización.
    /// </summary>
    [ServiceContract]
    public interface IUniquePersonParamServiceWebCore
    {       
        
        /// <summary>
        /// Obtiene personas por Id
        /// </summary>
        /// <param name="personIds">Id de person</param>
        /// <returns>Modelo PersonsServiceModel</returns>
        [OperationContract]
        PersonsServiceModel GetPersonsByPersonIds(List<int> personIds);

        #region WorkerType

        [OperationContract]
        List<WorkerTypeServiceModel> ExecuteOperationsWorkerType(List<WorkerTypeServiceModel> WorkerTypesServiceModel);

        [OperationContract]
        WorkerTypesServiceModel GetWorkertypeByDescription(string description);

        [OperationContract]
        ExcelFileServiceModel GenerateFileToWorkerType(List<WorkerTypeServiceModel> lstIWorkerType, string fileName);

        [OperationContract]
        WorkerTypesServiceModel GetWorkertype();

        
        #endregion WorkerType
    }
}