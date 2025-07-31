// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamServices.EEProvider.Assemblers
{
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UniquePerson.Entities;
    using Sistran.Core.Application.UniquePersonParamService.Models;

    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class EntityAssembler
    {
        
        #region WorkerType
        public static WorkerType CreateParamWorkerType(ParamWorkerType workerType)
        {
            return new WorkerType(workerType.Id)
            {
                WorkerTypeId = workerType.Id,
                Description = workerType.Description,
                SmallDescription = workerType.SmallDescription,
                IsEnabled = workerType.IsEnabled
            };
        }
        #endregion WorkerType


    }
}
