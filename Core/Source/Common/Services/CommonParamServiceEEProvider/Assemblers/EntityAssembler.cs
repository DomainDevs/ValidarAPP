// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Assemblers
{
    using System;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Convierte el modelo del servicio al  modelo de la entidad 
    /// </summary>
    public class EntityAssembler
    {
        /// <summary>
        /// Construye la entidad de parametros
        /// </summary>
        /// <param name="paramParameter">Modelo de parametros</param>
        /// <returns>Entidad de parametros</returns>
        public static Parameter CreateParameter(ParamParameter paramParameter)
        {
            return new Parameter(paramParameter.ParameterId)
            {
                ParameterId = paramParameter.ParameterId,
                Description = paramParameter.Description,
                NumberParameter = paramParameter.Value
            };
        }

        /// <summary>
        /// Construye la entidad CptParameter
        /// </summary>
        /// <param name="parameter">Entidaad CptParameter</param>
        /// <returns>result con resultado de construccion</returns>
        public static Result<ParamParameter, ErrorModel> CreateParamParameter(CptParameter parameter)
        {
            return ParamParameter.GetParamParameter(parameter.ParameterId, parameter.Description, parameter.NumberParameter.Value);
        }

        /// <summary>
        /// Construye la entidad CoParameter
        /// </summary>
        /// <param name="parameter">Entidad CoParameter</param>
        /// <returns>result con resultado de construccion</returns>
        public static Result<ParamParameter, ErrorModel> CreateParamCoParameter(CoParameter parameter)
        {
            return ParamParameter.GetParamParameter(parameter.ParameterId, parameter.Description, parameter.NumberParameter.Value);
        }

        /// <summary>
        /// Construye la entidad CptDaysValidateInfringementLog
        /// </summary>
        /// <param name="paramInfringementLog">Modelo ParamInfringementLog</param>
        /// <returns>Entidad CptDaysValidateInfringementLog</returns>
        public static CptDaysValidateInfringementLog CreateCptDaysValidateInfringementLog(ParamInfringementLog paramInfringementLog)
        {
            return new CptDaysValidateInfringementLog()
            {
                DaysValidateInfringement = Convert.ToInt32(paramInfringementLog.DaysValidateInfringement),
                RegistrationDate = paramInfringementLog.RegistrationDate,
                UserId = paramInfringementLog.UserId
            };
        }

        /// <summary>
        /// Construye la entidad CptDaysDiscontinuityLog
        /// </summary>
        /// <param name="paramDiscontinuityLog">Modelo ParamDiscontinuityLog</param>
        /// <returns>Entidad CptDaysDiscontinuityLog</returns>
        public static CptDaysDiscontinuityLog CreateCptDaysDiscontinuityLog(ParamDiscontinuityLog paramDiscontinuityLog)
        {
            return new CptDaysDiscontinuityLog()
            {
                DaysDiscontinuity = Convert.ToInt32(paramDiscontinuityLog.DaysDiscontinuity),
                RegistrationDate = paramDiscontinuityLog.RegistrationDate,
                UserId = paramDiscontinuityLog.UserId
            };
        }

        /// <summary>
        /// convierte de modelo de negocio a entidad
        /// </summary>
        /// <param name="branchParam">modelo de negocio</param>
        /// <returns>modelo de entidad</returns>
        public static CoBranch CreateCoBranch(ParamCoBranch branchParam) => new CoBranch(branchParam.Id)
        {
            Address = branchParam.Address,
            AddressTypeCode = branchParam.AddressType?.Id,
            BranchCode = branchParam.Id,
            CityCode = branchParam.City?.Id,
            CountryCode = branchParam.Country?.Id,
            IsIssue = branchParam.IsIssue,
            PhoneNumber = branchParam.PhoneNumber,
            PhoneTypeCode = branchParam.PhoneType?.Id,
            StateCode = branchParam.State?.Id
        };

        /// <summary>
        /// cnvierte de modelo negocio a modelo de entidad
        /// </summary>
        /// <param name="branchParam">modelo de negocio</param>
        /// <returns>modelo de entidad</returns>
        public static Branch CreateBranch(ParamBranch branchParam) => new Branch
        {
            BranchCode = branchParam.Id,
            Description = branchParam.Description,
            SmallDescription = branchParam.SmallDescription
        };       
    }
}
