// -----------------------------------------------------------------------
// <copyright file="UniquePersonParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamServices.EEProvider
    
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UniquePerson;
    using Sistran.Core.Application.UnderwritingParamServices.EEProviders.EEProvider.Assemblers;
    using Sistran.Core.Application.UniquePersonParamService;
    using Sistran.Core.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.UniquePersonParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UniquePersonParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.Error;    

    /// <summary>
    /// Clase que implementa la interfaz IUnderwritingParamServiceWeb.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonParamServiceEEProviderWebCore : IUniquePersonParamServiceWebCore
    {
        
               
        /// <summary>
        /// Obtiene personas por Id
        /// </summary>
        /// <param name="personIds">Id de person</param>
        /// <returns>Modelo PersonsServiceModel</returns>
        public PersonsServiceModel GetPersonsByPersonIds(List<int> personIds)
        {
            PersonDAO personDAO = new PersonDAO();
            PersonsServiceModel personsServiceModel = new PersonsServiceModel();
            Result<List<ParamPerson>, ErrorModel> result = personDAO.GetPersonByIndividualId(personIds);
            if (result is ResultError<List<ParamPerson>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamPerson>, ErrorModel>).Message;
                personsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                personsServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamPerson> resultValue = (result as ResultValue<List<ParamPerson>, ErrorModel>).Value;
                personsServiceModel.PersonServiceModel = ModelServiceAssembler.MappPersons(resultValue);
            }

            return personsServiceModel;
        }

        #region Workertype

        public List<WorkerTypeServiceModel> ExecuteOperationsWorkerType(List<WorkerTypeServiceModel> WorkerTypesServiceModel)
        {
            List<WorkerTypeServiceModel> result = new List<WorkerTypeServiceModel>();
            foreach (WorkerTypeServiceModel itemSM in WorkerTypesServiceModel)
            {
                ParamWorkerType item = ServicesModelsAssembler.CreateParamWorkerType(itemSM);
                WorkerTypeServiceModel itemResult = new WorkerTypeServiceModel();
                itemResult = this.OperationWorkerTypeServiceModel(item, itemSM.StatusTypeService);
                result.Add(itemResult);
            }
            return result;
        }
        public WorkerTypeServiceModel OperationWorkerTypeServiceModel(ParamWorkerType workerType, StatusTypeService statusTypeService, bool deleteIsPrincipal = false)
        {
            WorkerTypeDaos workerTypeDAO = new WorkerTypeDaos();
            WorkerTypeServiceModel workerTypeServiceModelResult = new WorkerTypeServiceModel()
            {
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                }
            };
            Result<ParamWorkerType, ErrorModel> result;
            switch (statusTypeService)
            {
                case StatusTypeService.Create:
                    result = workerTypeDAO.CreateParamWorkerType(workerType);
                    break;
                case StatusTypeService.Update:
                    result = workerTypeDAO.UpdateWorkerType(workerType);
                    break;
                default:
                    result = workerTypeDAO.CreateParamWorkerType(workerType);
                    break;
            }
            if (result is ResultError<ParamWorkerType, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamWorkerType, ErrorModel>).Message;
                workerTypeServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                workerTypeServiceModelResult.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                workerTypeServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<ParamWorkerType, ErrorModel>)
            {
                ParamWorkerType workertypeResult = (result as ResultValue<ParamWorkerType, ErrorModel>).Value;
                workerTypeServiceModelResult = ModelServiceAssembler.CreateWorkerTypeServiceModel(workertypeResult);
                workerTypeServiceModelResult.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                };
                workerTypeServiceModelResult.StatusTypeService = statusTypeService;
            }
            return workerTypeServiceModelResult;
        }

        /// <summary>
        /// Consultar todos los tipos de trabajador
        /// </summary>
        /// <returns>Listado de tipos de trabajador (Modelo de servicio)</returns>

        public WorkerTypesServiceModel GetWorkertype()
        {
            WorkerTypeDaos workerTypeGroupDAO = new WorkerTypeDaos();
            WorkerTypesServiceModel workerTypeServiceModel = new WorkerTypesServiceModel();
            Result<List<ParamWorkerType>, ErrorModel> resultGetWorkerType = workerTypeGroupDAO.GetWorkerType();
            if (resultGetWorkerType is ResultError<List<ParamWorkerType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetWorkerType as ResultError<List<ParamWorkerType>, ErrorModel>).Message;
                workerTypeServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                workerTypeServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamWorkerType> resultValue = (resultGetWorkerType as ResultValue<List<ParamWorkerType>, ErrorModel>).Value;
                workerTypeServiceModel = ModelServiceAssembler.MappWorkerType(resultValue);
            }
            return workerTypeServiceModel;
        }
        public WorkerTypesServiceModel GetWorkertypeByDescription(string description)
        {
            WorkerTypeDaos workerTypeGroupDAO = new WorkerTypeDaos();
            WorkerTypesServiceModel workerTypeServiceModel = new WorkerTypesServiceModel();
            Result<List<ParamWorkerType>, ErrorModel> resultGetWorkerType = workerTypeGroupDAO.GetWorkerTypeByDescription(description);
            if (resultGetWorkerType is ResultError<List<ParamWorkerType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetWorkerType as ResultError<List<ParamWorkerType>, ErrorModel>).Message;
                workerTypeServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                workerTypeServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamWorkerType> resultValue = (resultGetWorkerType as ResultValue<List<ParamWorkerType>, ErrorModel>).Value;
                workerTypeServiceModel = ModelServiceAssembler.MappWorkerType(resultValue);
            }
            return workerTypeServiceModel;
        }

        public ExcelFileServiceModel GenerateFileToWorkerType(List<WorkerTypeServiceModel> lstWorkerTypeSM, string fileName)
        {
            WorkerTypeDaos fileDAO = new WorkerTypeDaos();
            ExcelFileServiceModel workerTypeState = new ExcelFileServiceModel()
            {
                ErrorDescription = new List<string>(),
                ErrorTypeService = ErrorTypeService.Ok
            };
            List<ParamWorkerType> lstWorkerType = new List<ParamWorkerType>();
            foreach (WorkerTypeServiceModel itemSM in lstWorkerTypeSM)
            {
                lstWorkerType.Add(ServicesModelsAssembler.CreateParamWorkerType(itemSM));
            }
            Result<string, ErrorModel> result = fileDAO.GenerateFileToWorkerType(lstWorkerType, fileName);
            if (result is ResultError<string, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<string, ErrorModel>).Message;
               workerTypeState.ErrorDescription = errorModelResult.ErrorDescription;
                workerTypeState.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else if (result is ResultValue<string, ErrorModel>)
            {
               workerTypeState.FileData = (result as ResultValue<string, ErrorModel>).Value;
            }
            return workerTypeState;
        }
        #endregion WorkerType
    }

}