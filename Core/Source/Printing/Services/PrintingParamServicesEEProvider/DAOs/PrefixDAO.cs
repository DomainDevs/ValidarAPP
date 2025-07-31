// -----------------------------------------------------------------------
// <copyright file="PrefixDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.PrintingParamServices.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;    
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Transactions;
    using System.Linq;
    using Sistran.Core.Framework.Queries;


    /// <summary>
    /// Clase DAO del objeto Prefix.
    /// </summary>
    public class PrefixDAO
    {
        /// <summary>
        /// Obtiene la lista de ramos comerciales.
        /// </summary>
        /// <returns>Lista de ramos comerciales.</returns>
        public Result<List<ParamPrefix>, ErrorModel> GetPrefixs()
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prefix)));
                Result<List<ParamPrefix>, ErrorModel> paramPrefixList = ModelAssembler.MappParamPrefixs(businessCollection);
                if (paramPrefixList is ResultError<List<ParamPrefix>, ErrorModel>)
                {
                    return paramPrefixList;
                }
                else
                {
                    List<ParamPrefix> resultValue = (paramPrefixList as ResultValue<List<ParamPrefix>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {                        
                        errorModelListDescription.Add(Resources.Errors.PrefixNotFound);
                        return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return paramPrefixList;
                    }
                }
            }
            catch (Exception ex)
            {                
                errorModelListDescription.Add(Resources.Errors.PrefixThecnicalError);
                return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
