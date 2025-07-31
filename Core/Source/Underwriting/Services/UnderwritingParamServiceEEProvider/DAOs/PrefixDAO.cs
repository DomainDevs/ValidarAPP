// -----------------------------------------------------------------------
// <copyright file="PrefixDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;

    /// <summary>
    /// Clase DAO del objeto Prefix.
    /// </summary>
    public class PrefixDAO
    {
        /// <summary>
        /// Obtiene la lista de ramos comerciales.
        /// </summary>
        /// <returns>Lista de ramos comerciales</returns>
        public Result<List<ParamPrefix>, ErrorModel> GetPrefixes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prefix)));
                Result<List<ParamPrefix>, ErrorModel> prefix = ModelAssembler.CreatePrefixBusiness(businessCollection);
                if (prefix is ResultError<List<ParamPrefix>, ErrorModel>)
                {
                    return prefix;
                }
                else
                {
                    List<ParamPrefix> resultValue = (prefix as ResultValue<List<ParamPrefix>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron ramos comerciales.");
                        return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return prefix;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de ramos comerciales. Comuniquese con el administrador");
                return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
        public Result<List<ParamPrefix>, ErrorModel> GetPrefixByPrefixCode(int prefixCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Prefix.Properties.PrefixCode, prefixCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prefix), filter.GetPredicate()));
                Result<List<ParamPrefix>, ErrorModel> prefix = ModelAssembler.CreatePrefixBusiness(businessCollection);
                if (prefix is ResultError<List<ParamPrefix>, ErrorModel>)
                {
                    return prefix;
                }
                else
                {
                    List<ParamPrefix> resultValue = (prefix as ResultValue<List<ParamPrefix>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron ramos comerciales.");
                        return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return prefix;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de ramos comerciales. Comuniquese con el administrador");
                return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
    }
}
