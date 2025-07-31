// -----------------------------------------------------------------------
// <copyright file="CompanyTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonServices.DAOs
{
    using Sistran.Company.Application.UniquePersonServices.EEProvider.Assemblers;
    using Sistran.Company.Application.UniquePersonServices.EEProvider.Resources;
    using Sistran.Company.Application.UniquePersonServices.Models;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Clase DAO del objeto CompanyTypeDAO.
    /// </summary>
    public class CompanyTypeDAO
    {
        /// <summary>
        /// Obtiene la lista de tipo de compañia.
        /// </summary>
        /// <returns>Lista de tipo de compañia consultadas</returns>
        public Result<List<ParamCompanyType>, ErrorModel> GetCompanyTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                Result<List<ParamCompanyType>, ErrorModel> lstCompanyType = ModelAssembler.CreateCompanyTypes();
                if (lstCompanyType is ResultError<List<ParamCompanyType>, ErrorModel>)
                {
                    return lstCompanyType;
                }
                else
                {
                    List<ParamCompanyType> resultValue = (lstCompanyType as ResultValue<List<ParamCompanyType>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoTypeOfCompanyWasFound);
                        return new ResultError<List<ParamCompanyType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstCompanyType;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryTheCompany);
                return new ResultError<List<ParamCompanyType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }
    }
}