// -----------------------------------------------------------------------
// <copyright file="ParameterDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.CommonParamService.Assemblers;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;

    /// <summary>
    /// Clase DAO del objeto ParameterDAO.
    /// </summary>
    public class ParameterDAO
    {
        /// <summary>
        /// Obtiene lista de parametros
        /// </summary>
        /// <param name="paramParameter">Modelo ParamParameter</param>
        /// <returns>Retorna modelo ParamParameter</returns>
        public Result<List<ParamParameter>, ErrorModel> GetParameter(List<ParamParameter> paramParameter)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            List<ParamParameter> lstparameter = new List<ParamParameter>();
            try
            {
                Result<List<ParamParameter>, ErrorModel> paramlstParameter;

                foreach (var item in paramParameter)
                {
                    if (item.ParameterId == 1008 || item.ParameterId == 1011)
                    {
                        PrimaryKey key = CptParameter.CreatePrimaryKey(item.ParameterId);
                        CptParameter parameter = (CptParameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        lstparameter.Add((EntityAssembler.CreateParamParameter(parameter) as ResultValue<ParamParameter, ErrorModel>).Value);
                    }

                    if (item.ParameterId == 1009)
                    {
                        PrimaryKey keyCoParameter = CoParameter.CreatePrimaryKey(item.ParameterId);
                        CoParameter parameter = (CoParameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCoParameter);
                        lstparameter.Add((EntityAssembler.CreateParamCoParameter(parameter) as ResultValue<ParamParameter, ErrorModel>).Value);
                    }
                }

                paramlstParameter = new ResultValue<List<ParamParameter>, ErrorModel>(lstparameter);

                if (paramlstParameter is ResultError<List<ParamParameter>, ErrorModel>)
                {
                    return paramlstParameter;
                }
                else
                {
                    List<ParamParameter> resultValue = (paramlstParameter as ResultValue<List<ParamParameter>, ErrorModel>).Value;

                    if (resultValue == null)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorGetParameter);
                        return new ResultError<List<ParamParameter>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return paramlstParameter;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorGetParameter);
                return new ResultError<List<ParamParameter>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonParamService.DAOs");
            }
        }

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="paramParamete">Objeto de ParamParameter</param>
        /// <returns>Retorna mbjeto de ParamParameter</returns>
        public Result<ParamParameter, ErrorModel> UpdateParamParameter(ParamParameter paramParamete)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ParamParameter paramParameter = paramParamete;

                if (paramParamete.ParameterId == 1008 || paramParamete.ParameterId == 1011)
                {
                    PrimaryKey key = CptParameter.CreatePrimaryKey(paramParamete.ParameterId);
                    CptParameter parameter = (CptParameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    parameter.NumberParameter = paramParamete.Value;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(parameter);
                    paramParameter = (ModelAssembler.CreateParamParameter(parameter) as ResultValue<ParamParameter, ErrorModel>).Value;
                }

                if (paramParamete.ParameterId == 1009)
                {
                    PrimaryKey key = CoParameter.CreatePrimaryKey(paramParamete.ParameterId);
                    CoParameter parameter = (CoParameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    parameter.NumberParameter = paramParamete.Value;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(parameter);
                    paramParameter = (ModelAssembler.CreateParamCoParameter(parameter) as ResultValue<ParamParameter, ErrorModel>).Value;
                }

                return new ResultValue<ParamParameter, ErrorModel>(paramParameter);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorUpdatingTheParameter);
                return new ResultError<ParamParameter, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
