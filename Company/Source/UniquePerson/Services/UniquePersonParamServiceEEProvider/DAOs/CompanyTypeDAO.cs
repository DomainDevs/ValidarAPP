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
    using Sistran.Company.Application.UniquePersonParamService.Enums;
    using Sistran.Company.Application.UniquePersonParamService.Models;
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

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamCompanyType
        /// </summary>
        /// <returns>Lista de Modelos ParamCompanyType</returns>
        public static Result<List<ParamCompanyType>, ErrorModel> CreateCompanyTypes()
        {
            List<ParamCompanyType> branchType = new List<ParamCompanyType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCompanyType, ErrorModel> result;
            foreach (var value in Enum.GetValues(typeof(CompanyType)))
            {
                result = ParamCompanyType.GetParamCompanyType((int)value, ((CompanyType)value).ToString());
                if (result is ResultError<ParamCompanyType, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamLegalRepresentativeSing);
                    return new ResultError<List<ParamCompanyType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamCompanyType resultValue = (result as ResultValue<ParamCompanyType, ErrorModel>).Value;
                    branchType.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamCompanyType>, ErrorModel>(branchType);
        }
    }
}
