// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamServices.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.UniquePerson;
    using Sistran.Core.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.UniquePersonParamServices.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase ensambladora para mapear modelos de servicios a modelos de negocio.
    /// </summary>
    public class ServicesModelsAssembler
    {
        
        #region WorkerType
        public static ParamWorkerType CreateParamWorkerType(WorkerTypeServiceModel itemSM)
        {
            List<ParamWorkerType> cptGroupWorkerType = new List<ParamWorkerType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamWorkerType, ErrorModel> result;
            result = ParamWorkerType.GetParamWorkerType(itemSM.Id, itemSM.Description, itemSM.SmallDescription, itemSM.IsEnabled);
            return (result as ResultValue<ParamWorkerType, ErrorModel>).Value;
        }
        #endregion WorkerType
    }

}
