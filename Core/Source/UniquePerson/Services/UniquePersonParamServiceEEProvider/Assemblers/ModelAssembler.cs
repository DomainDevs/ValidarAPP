// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamService.EEProviders.EEProvider.Assemblers
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UniquePerson.Entities;
    using Sistran.Core.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.UniquePersonParamServices.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;    

    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelAssembler"/> class.
        /// </summary>
        protected ModelAssembler()
        {
        }
        
        /// <summary>
        /// Devuelve las personas 
        /// </summary>
        /// <param name="businessCollection">Collección de personas</param>
        /// <returns>Listado de ParamPerson</returns>
        public static List<ParamPerson> CreatePersons(BusinessCollection businessCollection)
        {
            List<ParamPerson> paramPersons = new List<ParamPerson>();          
            foreach (Person entityPerson in businessCollection)
            {
                paramPersons.Add(CreatePerson(entityPerson));
            }

            return paramPersons;
        }

        /// <summary>
        /// Devuelve modelo ParamPerson
        /// </summary>
        /// <param name="person">Entidad Person</param>
        /// <returns>Modelo ParamPerson</returns>
        public static ParamPerson CreatePerson(Person person)
        {
            return new ParamPerson
            {
                Name = person.Name,
                PersonId = person.IndividualId
            };
        }

        #region WorkerType
        public static Result<List<ParamWorkerType>, ErrorModel> CreateParamWorkerType(BusinessCollection businessCollection)
        {
            List<ParamWorkerType> cptWorkerType = new List<ParamWorkerType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamWorkerType, ErrorModel> result;
            foreach(WorkerType entityWorkerType in businessCollection)
            {
                result = CreateParamWorkerType(entityWorkerType);
                if(result is ResultError<ParamWorkerType,ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelWorker);
                    return new ResultError<List<ParamWorkerType>, ErrorModel>(ErrorModel.CreateErrorModel(
                       errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamWorkerType resultValue = (result as ResultValue<ParamWorkerType, ErrorModel>).Value;
                    cptWorkerType.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamWorkerType>, ErrorModel>(cptWorkerType);
        }

        public static ResultValue<ParamWorkerType, ErrorModel> CreateParamWorkerType(WorkerType workertype)
        {
            ResultValue<ParamWorkerType, ErrorModel> result = ParamWorkerType.GetParamWorkerType(workertype.WorkerTypeId, workertype.Description, workertype.SmallDescription, workertype.IsEnabled);
            return result;
        }
        #endregion WorkerType
    }


}
