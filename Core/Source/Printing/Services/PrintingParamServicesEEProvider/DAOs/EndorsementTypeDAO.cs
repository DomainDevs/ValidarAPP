// -----------------------------------------------------------------------
// <copyright file="EndorsementTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.PrintingParamServices.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Enums;

    /// <summary>
    /// Clase DAO del objeto EndorsementType.
    /// </summary>
    public class EndorsementTypeDAO
    {
        /// <summary>
        /// Obtiene la lista de tipos de endoso.
        /// </summary>
        /// <returns>Lista lista de tipos de endoso.</returns>
        public Result<List<ParamEndoresementType>, ErrorModel> GetEndoresementTypes()
        {            
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EndorsementType)));
                Result<List<ParamEndoresementType>, ErrorModel> paramEndoresementTypeList = ModelAssembler.MappEndoresementTypes(businessCollection);
                if (paramEndoresementTypeList is ResultError<List<ParamEndoresementType>, ErrorModel>)
                {
                    return paramEndoresementTypeList;
                }
                else
                {
                    List<ParamEndoresementType> resultValue = (paramEndoresementTypeList as ResultValue<List<ParamEndoresementType>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {                        
                        errorModelListDescription.Add(Resources.Errors.EndorsementTypeNotFound);
                        return new ResultError<List<ParamEndoresementType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return paramEndoresementTypeList;
                    }
                }
            }
            catch (Exception ex)
            {                
                errorModelListDescription.Add(Resources.Errors.EndorsementTypeThecnicalError);
                return new ResultError<List<ParamEndoresementType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }            
        }
    }
}
