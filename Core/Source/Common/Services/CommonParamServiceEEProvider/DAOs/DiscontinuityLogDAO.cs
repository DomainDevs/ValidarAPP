// -----------------------------------------------------------------------
// <copyright file="DiscontinuityLogDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.DAOs
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.CommonParamService.Assemblers;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase DAO del objeto DiscontinuityLogDAO.
    /// </summary>
    public class DiscontinuityLogDAO
    {
        /// <summary>
        /// Crea el los de dias de Infracción
        /// </summary>
        /// <param name="paramDiscontinuityLog">Modelo ParamInfringementLog</param>
        /// <returns>retorna modelo ParamInfringementLog</returns>
        public Result<ParamDiscontinuityLog, ErrorModel> CreateParamDiscontinuityLo(ParamDiscontinuityLog paramDiscontinuityLog)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                CptDaysDiscontinuityLog cptDaysDiscontinuityLog = EntityAssembler.CreateCptDaysDiscontinuityLog(paramDiscontinuityLog);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(cptDaysDiscontinuityLog);
                Result<ParamDiscontinuityLog, ErrorModel> paramDiscontinuityLogResult = ModelAssembler.CreateCptDaysDiscontinuityLog(cptDaysDiscontinuityLog);
                return paramDiscontinuityLogResult;
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Falló, al crear el log dias de Infracción, por error en BD.");
                return new ResultError<ParamDiscontinuityLog, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
