// -----------------------------------------------------------------------
// <copyright file="GroupCoverageDAO.cs" company="SISTRAN">
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
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Product.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;

    /// <summary>
    /// Clase DAO del objeto GroupCoverage.
    /// </summary>
    public class GroupCoverageDAO
    {
        /// <summary>
        /// Obtiene la lista de grupos de coberturas.
        /// </summary>
        /// <returns>Lista de grupos de coberturas</returns>
        public Result<List<ParamGroupCoverage>, ErrorModel> GetGroupCoverages()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover)));
                Result<List<ParamGroupCoverage>, ErrorModel> groupCoverage = ModelAssembler.CreateGroupCoverage(businessCollection);
                if (groupCoverage is ResultError<List<ParamGroupCoverage>, ErrorModel>)
                {
                    return groupCoverage;
                }
                else
                {
                    List<ParamGroupCoverage> resultValue = (groupCoverage as ResultValue<List<ParamGroupCoverage>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron grupos de coberturas.");
                        return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return groupCoverage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de grupos de coberturas. Comuniquese con el administrador");
                return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
        public Result<List<ParamGroupCoverage>, ErrorModel> GetGroupCoverageByGroupCoverageCode(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GroupCoverage.Properties.CoverageId, coverageId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(GroupCoverage), filter.GetPredicate()));
                Result<List<ParamGroupCoverage>, ErrorModel> groupCoverage = ModelAssembler.CreateGroupCoverage(businessCollection);
                if (groupCoverage is ResultError<List<ParamGroupCoverage>, ErrorModel>)
                {
                    return groupCoverage;
                }
                else
                {
                    List<ParamGroupCoverage> resultValue = (groupCoverage as ResultValue<List<ParamGroupCoverage>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron grupos de coberturas.");
                        return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return groupCoverage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de grupos de coberturas. Comuniquese con el administrador");
                return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        public Result<List<ParamGroupCoverage>, ErrorModel> GetGroupCoverageByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ProductGroupCover.Properties.ProductId, productId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filter.GetPredicate()));
                Result<List<ParamGroupCoverage>, ErrorModel> groupCoverage = ModelAssembler.CreateGroupCoverage(businessCollection);
                if (groupCoverage is ResultError<List<ParamGroupCoverage>, ErrorModel>)
                {
                    return groupCoverage;
                }
                else
                {
                    List<ParamGroupCoverage> resultValue = (groupCoverage as ResultValue<List<ParamGroupCoverage>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron grupos de coberturas.");
                        return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return groupCoverage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de grupos de coberturas. Comuniquese con el administrador");
                return new ResultError<List<ParamGroupCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
    }
}
