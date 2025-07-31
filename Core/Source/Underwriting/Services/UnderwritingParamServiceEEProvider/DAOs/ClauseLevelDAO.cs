// -----------------------------------------------------------------------
// <copyright file="ClauseLevelDao.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PRODEN = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;   

    /// <summary>
    /// clase nivel de clausulaDAO
    /// </summary>
    public class ClauseLevelDAO
    {
        /// <summary>
        /// Metodo para crear Nivel por clausula
        /// </summary>
        /// <param name="level">Recibe level</param>
        /// <returns>Retorna ParamClauseLevel</returns>
        public UTMO.Result<ParamClauseLevel, UTMO.ErrorModel> CreateLevel(ParamClauseLevel level)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                PRODEN.ClauseLevel clauseLevelEntity = EntityAssembler.CreateClauseLevelParam(level);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(clauseLevelEntity);
                ParamClauseLevel levelResult = ModelAssembler.CreateClauseLevel(clauseLevelEntity);
                return new UTMO.ResultValue<ParamClauseLevel, UTMO.ErrorModel>(levelResult);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorGetLevelClause);
                return new UTMO.ResultError<ParamClauseLevel, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
