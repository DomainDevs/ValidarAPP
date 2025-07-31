// -----------------------------------------------------------------------
// <copyright file="ModelServiceAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamServices.EEProviders.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.UniquePerson;
    using Sistran.Core.Application.UniquePersonParamService.Models;
    

    /// <summary>
    /// Clase ensambladora para mapear modelos de negocio a modelos de servicios.
    /// </summary>
    public class ModelServiceAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelServiceAssembler"/> class.
        /// </summary>
        protected ModelServiceAssembler()
        {
        }
        
        public static List<PersonServiceModel> MappPersons(List<ParamPerson> paramPerson)
        {
            List<PersonServiceModel> personServiceModel = new List<PersonServiceModel>();
            foreach (ParamPerson item in paramPerson)
            {
                PersonServiceModel itemPerson = new PersonServiceModel
                {
                    PersonId = item.PersonId,
                    Name = item.Name
                };
                personServiceModel.Add(itemPerson);
            }          
            return personServiceModel;
        }

        #region WorkerType
        public static WorkerTypesServiceModel MappWorkerType(List<ParamWorkerType> paramWorkerType)
        {
            WorkerTypesServiceModel paramWorkerTypeServiceModel = new WorkerTypesServiceModel();
            List<WorkerTypeServiceModel> listWorkerTypeServiceModel = new List<WorkerTypeServiceModel>();
            foreach (ParamWorkerType paramWorkerTypeBusinessModel  in paramWorkerType)
            {
                WorkerTypeServiceModel itemInfringementServiceModel = new WorkerTypeServiceModel();
                itemInfringementServiceModel.Id = paramWorkerTypeBusinessModel.Id;
                itemInfringementServiceModel.Description = paramWorkerTypeBusinessModel.Description;
                itemInfringementServiceModel.SmallDescription = paramWorkerTypeBusinessModel.SmallDescription;
                itemInfringementServiceModel.IsEnabled = paramWorkerTypeBusinessModel.IsEnabled;
                itemInfringementServiceModel.StatusTypeService = StatusTypeService.Original;
                listWorkerTypeServiceModel.Add(itemInfringementServiceModel);
            }
            paramWorkerTypeServiceModel.ErrorDescription = new List<string>();
            paramWorkerTypeServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramWorkerTypeServiceModel.WorkerTypeServiceModel = listWorkerTypeServiceModel;


                return paramWorkerTypeServiceModel;
        }
        public static WorkerTypeServiceModel CreateWorkerTypeServiceModel(ParamWorkerType workerTypeResult)
        {
            return new WorkerTypeServiceModel()
            {
                Id = workerTypeResult.Id,
                Description = workerTypeResult.Description,
                SmallDescription = workerTypeResult.SmallDescription,
                IsEnabled = workerTypeResult.IsEnabled
            };
        }
        #endregion WorkerType
    }
}
