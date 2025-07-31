// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodirguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Assemblers
{
    using System;
    using System.Collections.Generic;   
    using CommonService.Models;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.ModelServices.Models.Common;
    using Sistran.Core.Application.ModelServices.Models.CommonParam;
    using Sistran.Core.Application.Utilities.Error;    

    /// <summary>
    /// Clase ensambladora para mapear modelos de servicios a modelos de negocio.
    /// </summary>
    public class ServicesModelsAssembler
    {
        /// <summary>
        /// Mapeo del modelo ParameterServiceModel al modelo ParamParameter
        /// </summary>
        /// <param name="parameterServiceModel">Objeto de ParameterServiceModel</param>
        /// <returns>Modelo ParamParameter</returns>
        public static Result<ParamParameter, ErrorModel> CreateParamParameter(ParameterServiceModel parameterServiceModel)
        {
            Result<ParamParameter, ErrorModel> result = ParamParameter.CreateParamParameter(
                parameterServiceModel.ParameterId,
                parameterServiceModel.Description,
                parameterServiceModel.Value);
            return result;
        }

        /// <summary>
        /// Mapeo de lista del modelo ParameterServiceModel al modelo ParamParameter
        /// </summary>
        /// <param name="parameterServiceModel">Lista modelo ParameterServiceModel</param>
        /// <returns>Lista modelo ParamParameter</returns>
        public static List<ParamParameter> CreateParamParameters(List<ParameterServiceModel> parameterServiceModel)
        {
            List<ParamParameter> lstresultParamParameter = new List<ParamParameter>();
            Result<ParamParameter, ErrorModel> resultParamParameter;
            foreach (var item in parameterServiceModel)
            {
                resultParamParameter = ParamParameter.GetParamParameter(item.ParameterId, item.Description, item.Value);
                lstresultParamParameter.Add((resultParamParameter as ResultValue<ParamParameter, ErrorModel>).Value);
            }

            return lstresultParamParameter;
        }

        /// <summary>
        /// Mapeo del modelo ParameterServiceModel al modelo ParamParameter
        /// </summary>
        /// <param name="discontinuityLogServiceModel">Modelo DiscontinuityLogServiceModel</param>
        /// <returns>resultado Modelo ParamDiscontinuityLog</returns>
        public static Result<ParamDiscontinuityLog, ErrorModel> CreateParamDiscontinuityLog(DiscontinuityLogServiceModel discontinuityLogServiceModel)
        {
            Result<ParamDiscontinuityLog, ErrorModel> result = ParamDiscontinuityLog.CreateParamDiscontinuityLog(
                discontinuityLogServiceModel.daysDiscontinuity,
                discontinuityLogServiceModel.registrationDate,
                discontinuityLogServiceModel.userId);
            return result;
        }

        /// <summary>
        /// Mapeo del modelo InfringementLogServiceModel al modelo ParamParameter
        /// </summary>
        /// <param name="infringementLogServiceModel">Modelo InfringementLogServiceModel</param>
        /// <returns>resultado Modelo ParamInfringementLog</returns>
        public static Result<ParamInfringementLog, ErrorModel> CreateParamInfringementLog(InfringementLogServiceModel infringementLogServiceModel)
        {
            Result<ParamInfringementLog, ErrorModel> result = ParamInfringementLog.CreateParamInfringementLog(
                infringementLogServiceModel.daysValidateInfringement,
                infringementLogServiceModel.registrationDate,
                infringementLogServiceModel.userId);
            return result;
        }

        #region Branch
        /// <summary>
        /// convierte de modelo de servicio a modelo de negocio 
        /// </summary>
        /// <param name="branchServiceModel">modelo de servicio </param>
        /// <returns>lista de modelo de negocio </returns>
        public static List<ParamBranch> CreateParametrizationBranches(List<BranchServiceQueryModel> branchServiceModel)
        {
            List<ParamBranch> paramBranch = new List<ParamBranch>();
            foreach (var item in branchServiceModel)
            {
                paramBranch.Add(CreateParametrizationBranch(item));
            }

            return paramBranch;
        }

        /// <summary>
        /// convierte de modelo de servico a modelo negocio
        /// </summary>
        /// <param name="branchServiceModel">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        public static ParamCoBranch CreateParamCoBranches(BranchServiceModel branchServiceModel) => new ParamCoBranch
        {
            Address = branchServiceModel.Address,
            Branch = CreateParametrizationBranch(branchServiceModel.Branch),
            AddressType = CreateParamAdrressType(branchServiceModel.AddressType),
            City = CreateCity(branchServiceModel.City),
            Country = CreateCountry(branchServiceModel.Country),
            Id = branchServiceModel.Id,
            IsIssue = branchServiceModel.IsIssue,
            PhoneNumber = branchServiceModel.PhoneNumber,
            PhoneType = CreatePhoneType(branchServiceModel.PhoneType),
            State = CreateState(branchServiceModel.State)
        };

        /// <summary>
        /// convierte de modelo de servico a modelo negocio
        /// </summary>
        /// <param name="state">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        public static State CreateState(StateServiceQueryModel state) => new State
        {
            Description = state?.Description,
            Id = state.Id
        };

        /// <summary>
        /// convierte de modelo de servico a modelo negocio
        /// </summary>
        /// <param name="phoneType">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        public static ParamPhoneType CreatePhoneType(PhoneTypeServiceQueryModel phoneType) => new ParamPhoneType
        {
            Description = phoneType?.Description,
            Id = phoneType.Id
        };

        /// <summary>
        /// convierte de modelo de servico a modelo negocio
        /// </summary>
        /// <param name="country">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        public static Country CreateCountry(CountryServiceQueryModel country) => new Country
        {
            Id = country.Id,
            Description = country?.Description
        };

        /// <summary>
        /// convierte de modelo de servico a modelo negocio
        /// </summary>
        /// <param name="city">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        public static City CreateCity(CityServiceRelationModel city) => new City
        {
            Id = city.Id,
            Description = city?.Description
        };

        /// <summary>
        /// convierte de modelo de servico a modelo negocio
        /// </summary>
        /// <param name="addressType">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        public static ParamAddressType CreateParamAdrressType(AddressTypeServiceQueryModel addressType) => new ParamAddressType
        {
            Description = addressType?.Description,
            Id = addressType.Id
        };

        /// <summary>
        /// Convierte de modelo de servicio a modelo de negocio 
        /// </summary>
        /// <param name="branchServiceQueryModel">modelo de servicio </param>
        /// <returns>modelo de negocio</returns>
        private static ParamBranch CreateParametrizationBranch(BranchServiceQueryModel branchServiceQueryModel) => new ParamBranch
        {
            Description = branchServiceQueryModel.Description,
            Id = branchServiceQueryModel.Id,
            SmallDescription = branchServiceQueryModel.SmallDescription,
            Is_issue= branchServiceQueryModel.Is_issue
        };
        #endregion

        #region SalePoint
        /// <summary>
        /// convierte de lista de modelo de negocio a lista de modelo de servicio 
        /// </summary>
        /// <param name="salePointServiceModel">modelo de servicio de punto de ventas  </param>
        /// <returns>lista de modelo de negocio de puntos de venta </returns>
        public static List<ParamSalePoint> CreateParametrizationSalePointes(List<SalePointServiceModel> salePointServiceModel)
        {
            List<ParamSalePoint> paramSalePoint = new List<ParamSalePoint>();
            foreach (var item in salePointServiceModel)
            {
                paramSalePoint.Add(CreateParametrizationSalePoint(item));
            }

            return paramSalePoint;
        }

        /// <summary>
        /// convierte de modelo de negocio a modelo de servicio 
        /// </summary>
        /// <param name="item">modelo de servicio de punto de ventas </param>
        /// <returns>modelo de negocio de puntos de venta</returns>
        private static ParamSalePoint CreateParametrizationSalePoint(SalePointServiceModel item) => new ParamSalePoint
        {
            Description = item.Description,
            Id = item.Id,
            Branch = new ParamBranch
            {
                Description = item.Branch.Description,
                Id = item.Branch.Id
            },
            SmallDescription = item.SmallDescription
        };
        #endregion
    }
}
