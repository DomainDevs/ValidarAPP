// -----------------------------------------------------------------------
// <copyright file="InfringementLogDAO.cs" company="SISTRAN">
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
    /// Clase DAO del objeto ParameterDAO.
    /// </summary>
    public class InfringementLogDAO
    {
        /// <summary>
        /// Crea el los de dias de Infracción
        /// </summary>
        /// <param name="paramInfringementLog">Modelo ParamInfringementLog</param>
        /// <returns>Retorna modelo ParamInfringementLog</returns>
        public Result<ParamInfringementLog, ErrorModel> CreateParamInfringementLog(ParamInfringementLog paramInfringementLog)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                CptDaysValidateInfringementLog cptDaysValidateInfringementLog = EntityAssembler.CreateCptDaysValidateInfringementLog(paramInfringementLog);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(cptDaysValidateInfringementLog);
                Result<ParamInfringementLog, ErrorModel> paramInfringementLogResult = ModelAssembler.CreateCptDaysValidateInfringementLog(cptDaysValidateInfringementLog);
                return paramInfringementLogResult;
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Falló, al crear el log dias de Infracción, por error en BD.");
                return new ResultError<ParamInfringementLog, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
