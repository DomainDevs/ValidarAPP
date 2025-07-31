// -----------------------------------------------------------------------
// <copyright file="RequestEndorsementDAO.cs" company="SISTRAN">
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
    using Sistran.Core.Application.Request.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;

    /// <summary>
    /// Clase DAO del objeto RequestEndorsement.
    /// </summary>
    public class RequestEndorsementDAO
    {
        /// <summary>
        /// Obtiene la lista de solicitudes agrupadoras.
        /// </summary>
        /// <returns>Lista de solicitudes agrupadoras</returns>
        public Result<List<ParamRequestEndorsement>, ErrorModel> GetRequestEndorsements()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoRequestEndorsement)));
                Result<List<ParamRequestEndorsement>, ErrorModel> requestEndorsement = ModelAssembler.CreateRequestEndorsement(businessCollection);
                if (requestEndorsement is ResultError<List<ParamRequestEndorsement>, ErrorModel>)
                {
                    return requestEndorsement;
                }
                else
                {
                    List<ParamRequestEndorsement> resultValue = (requestEndorsement as ResultValue<List<ParamRequestEndorsement>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron solicitudes agrupadoras.");
                        return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return requestEndorsement;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de solicitudes agrupadoras. Comuniquese con el administrador");
                return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
        public Result<List<ParamRequestEndorsement>, ErrorModel> GetRequestEndorsementByRequestEndorsementCode(int requestEndorsementCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CoRequestEndorsement.Properties.RequestEndorsementId, requestEndorsementCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoRequestEndorsement), filter.GetPredicate()));
                Result<List<ParamRequestEndorsement>, ErrorModel> requestEndorsement = ModelAssembler.CreateRequestEndorsement(businessCollection);
                if (requestEndorsement is ResultError<List<ParamRequestEndorsement>, ErrorModel>)
                {
                    return requestEndorsement;
                }
                else
                {
                    List<ParamRequestEndorsement> resultValue = (requestEndorsement as ResultValue<List<ParamRequestEndorsement>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron solicitudes agrupadoras.");
                        return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return requestEndorsement;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de solicitudes agrupadoras. Comuniquese con el administrador");
                return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        public Result<List<ParamRequestEndorsement>, ErrorModel> GetCurrentRequestEndorsementsByPrefixCode(int prefixCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamRequestEndorsement> result = new List<ParamRequestEndorsement>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CoRequestEndorsement.Properties.PrefixCode, prefixCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoRequestEndorsement), filter.GetPredicate()));
                foreach (CoRequestEndorsement coRequestEndorsement in businessCollection)
                {
                    if (coRequestEndorsement.CurrentTo > DateTime.Now)
                    {
                        Result<List<ParamRequestEndorsement>, ErrorModel> item = GetRequestEndorsementByRequestEndorsementCode(coRequestEndorsement.RequestEndorsementId);
                        if (item is ResultError<List<ParamRequestEndorsement>, ErrorModel>)
                        {
                            return item;
                        }
                        else
                        {
                            List<ParamRequestEndorsement> resultItem = (item as ResultValue<List<ParamRequestEndorsement>, ErrorModel>).Value;
                            foreach (ParamRequestEndorsement paramRequest in resultItem)
                            {
                                result.Add(paramRequest);
                            }
                        }
                    }
                }
                if (result.Count>0)
                {
                    return new ResultValue<List<ParamRequestEndorsement>, ErrorModel>(result);
                }
                else
                {
                    errorModelListDescription.Add("No se encontraron solicitudes agrupadoras.");
                    return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de solicitudes agrupadoras. Comuniquese con el administrador");
                return new ResultError<List<ParamRequestEndorsement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
    }
}
