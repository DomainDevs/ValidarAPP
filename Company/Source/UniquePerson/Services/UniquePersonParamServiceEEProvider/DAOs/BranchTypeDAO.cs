// -----------------------------------------------------------------------
// <copyright file="BranchTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Clase DAO del objeto BranchTypeDAO.
    /// </summary>
    public class BranchTypeDAO
    {
        /// <summary>
        /// Obtiene la lista de tipo de sucursal.
        /// </summary>
        /// <returns>Lista de tipo de sucursal consultadas</returns>
        public Result<List<ParamBranchType>, ErrorModel> GetBranchTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoBranchType)));
                Result<List<ParamBranchType>, ErrorModel> lstBranchType = ModelAssembler.CreateBranchTypes(businessCollection);
                if (lstBranchType is ResultError<List<ParamBranchType>, ErrorModel>)
                {
                    return lstBranchType;
                }
                else
                {
                    List<ParamBranchType> resultValue = (lstBranchType as ResultValue<List<ParamBranchType>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoTypeOfBranchWasFound);
                        return new ResultError<List<ParamBranchType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstBranchType;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryTheBranch);
                return new ResultError<List<ParamBranchType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }


    }
}
