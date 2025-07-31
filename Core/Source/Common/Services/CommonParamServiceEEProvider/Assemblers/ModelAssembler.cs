// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Assemblers
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using COMEN = Sistran.Core.Application.Common.Entities;
    using System.Linq;

    /// <summary>
    /// Clase ensambladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelAssembler"/> class.
        /// </summary>
        protected ModelAssembler()
        {
        }

        /// <summary>
        /// Mapeo de la entidad CptParameter al modelo ParamParameter
        /// </summary>
        /// <param name="parameter">Entidad CptParameter</param>
        /// <returns>Modelo ParamParameter </returns>
        public static Result<ParamParameter, ErrorModel> CreateParamParameter(CptParameter parameter)
        {
            Result<ParamParameter, ErrorModel> result = ParamParameter.GetParamParameter(parameter.ParameterId, parameter.Description, parameter.NumberParameter.Value);
            return result;
        }

        /// <summary>
        /// Mapeo de la entidad CoParameter al modelo ParamParameter
        /// </summary>
        /// <param name="parameter">Entidad CoParameter</param>
        /// <returns>Modelo ParamParameter </returns>
        public static Result<ParamParameter, ErrorModel> CreateParamCoParameter(CoParameter parameter)
        {
            Result<ParamParameter, ErrorModel> result = ParamParameter.GetParamParameter(parameter.ParameterId, parameter.Description, parameter.NumberParameter.Value);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo Parameter
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Modelo ParamParameter </returns>
        public static Result<List<ParamParameter>, ErrorModel> CreateLstParameter(BusinessCollection businessCollection)
        {
            List<ParamParameter> parameter = new List<ParamParameter>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamParameter, ErrorModel> result;
            foreach (CptParameter entityParameter in businessCollection)
            {
                result = CreateParamParameter(entityParameter);
                if (result is ResultError<ParamParameter, ErrorModel>)
                {
                    errorModelListDescription.Add(Resources.Errors.ErrorMappingTheEntity);
                    return new ResultError<List<ParamParameter>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamParameter resultValue = (result as ResultValue<ParamParameter, ErrorModel>).Value;
                    parameter.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamParameter>, ErrorModel>(parameter);
        }

        /// <summary>
        /// Mapeo lista de objeto CptDaysValidateInfringementLog a lista modelo ParamInfringementLog
        /// </summary>
        /// <param name="cptDaysValidateInfringementLog">Entidad CptDaysValidateInfringementLog</param>
        /// <returns>Modelo ParamInfringementLog</returns>
        public static Result<ParamInfringementLog, ErrorModel> CreateCptDaysValidateInfringementLog(CptDaysValidateInfringementLog cptDaysValidateInfringementLog)
        {
            Result<ParamInfringementLog, ErrorModel> result = ParamInfringementLog.GetParamInfringementLog(cptDaysValidateInfringementLog.DaysValidateInfringement, cptDaysValidateInfringementLog.RegistrationDate, cptDaysValidateInfringementLog.UserId);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto CptDaysDiscontinuityLog a lista modelo ParamDiscontinuityLog
        /// </summary>
        /// <param name="cptDaysDiscontinuityLog">Entidad CptDaysDiscontinuityLog</param>
        /// <returns>Modelo ParamDiscontinuityLog</returns>
        public static Result<ParamDiscontinuityLog, ErrorModel> CreateCptDaysDiscontinuityLog(CptDaysDiscontinuityLog cptDaysDiscontinuityLog)
        {
            Result<ParamDiscontinuityLog, ErrorModel> result = ParamDiscontinuityLog.GetParamDiscontinuityLog(cptDaysDiscontinuityLog.DaysDiscontinuity, cptDaysDiscontinuityLog.RegistrationDate, cptDaysDiscontinuityLog.UserId);
            return result;
        }

        /// <summary>
        /// Pasa de Entidad a modelo de negocio 
        /// </summary>
        /// <param name="businessCollection">business Collection</param>       
        /// <returns>lista del Modelo de sucursales</returns>
        public static List<ParamBranch> CreateCompanyBranchs(BusinessCollection businessCollection)
        {
            List<ParamBranch> companyBranchs = new List<ParamBranch>();

            foreach (COMEN.Branch entityBranch in businessCollection)
            {
                companyBranchs.Add(CreateCompanyBranch(entityBranch));
            }

            return companyBranchs;
        }

        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityBranch">entidad de sucursales</param>       
        /// <returns>Modelo de sucursales</returns>
        private static ParamBranch CreateCompanyBranch(COMEN.Branch entityBranch) => new ParamBranch
        {
            Id = entityBranch.BranchCode,
            Description = entityBranch.Description,
            SmallDescription = entityBranch.SmallDescription

        };

        /// <summary>
        /// Pasa de Entidad a modelo de negocio 
        /// </summary>
        /// <param name="businessCollection">business Collection</param>
        /// <returns>lista del Modelo de puntos de venta</returns>
        public static List<ParamSalePoint> CreateCompanySalePoints(BusinessCollection businessCollection)
        {
            List<ParamSalePoint> paramSalePoint = new List<ParamSalePoint>();

            foreach (SalePoint salePoint in businessCollection)
            {
                paramSalePoint.Add(CreateCompanySalePoint(salePoint));
            }

            return paramSalePoint;
        }

        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityBranch">entidad de puntos de venta</param>       
        /// <returns>Modelo de puntos de venta</returns>
        private static ParamSalePoint CreateCompanySalePoint(SalePoint entitySalePoint) => new ParamSalePoint
        {
            Description = entitySalePoint.Description,
            Id = (int)entitySalePoint.SalePointCode,
            Branch = new ParamBranch
            {
                Id = entitySalePoint.BranchCode
            },
            SmallDescription = entitySalePoint.SmallDescription,
            Enabled = Convert.ToBoolean(entitySalePoint.Enabled)
        };

        /// <summary>
        /// convierte de entidad a modelo de negocio 
        /// </summary>
        /// <param name="branch">modelo entidad</param>
        /// <returns>retorna modelo de negocio</returns>
        public static ParamBranch CreateBranchss(Branch branch, CoBranch coBranch) => new ParamBranch
        {
            Description = branch.Description,
            SmallDescription = branch.SmallDescription,
            Id = branch.BranchCode,
            Is_issue = coBranch.IsIssue
        };

        /// <summary>
        /// convierte de entidad a modelo de negocio 
        /// </summary>
        /// <param name="branch">modelo entidad</param>
        /// <returns>retorna modelo de negocio</returns>
        public static ParamBranch CreateBranchs(Branch branch) => new ParamBranch
        {
            Description = branch.Description,
            Id = branch.BranchCode
        };

        /// <summary>
        /// convierte de entidad a modelo de negocio 
        /// </summary>
        /// <param name="coBranch">modelo entidad</param>
        /// <returns>retorna modelo de negocio</returns>
        public static ParamCoBranch CreateCoBranches(CoBranch coBranch) => new ParamCoBranch
        {
            Id = coBranch.BranchCode,
            AddressType = new ParamAddressType { Id = coBranch.AddressTypeCode.Value },
            Address = coBranch.Address,
            City = new CommonService.Models.City { Id = coBranch.CityCode.Value },
            Country = new CommonService.Models.Country { Id = coBranch.CountryCode.Value },
            IsIssue = coBranch.IsIssue,
            PhoneNumber = coBranch.PhoneNumber.Value,
            PhoneType = new ParamPhoneType { Id = coBranch.PhoneTypeCode.Value },
            State = new CommonService.Models.State { Id = coBranch.StateCode.Value }
        };

        internal static List<ParamVehicleConcessionaire> CreateVehicleConcessionaires(BusinessCollection businessCollection)
        {
            List<ParamVehicleConcessionaire> paramVehicleConcessionaires = new List<ParamVehicleConcessionaire>();

            foreach (CiaVehicleConcessionaire ciaVehicleConcessionaire in businessCollection)
            {
                paramVehicleConcessionaires.Add(CreateVehicleConcessionaire(ciaVehicleConcessionaire));
            }
            return paramVehicleConcessionaires;
        }

        internal static ParamVehicleConcessionaire CreateVehicleConcessionaire(CiaVehicleConcessionaire ciaVehicleConcessionaire)
        {
            return new ParamVehicleConcessionaire()
            {
                Id = ciaVehicleConcessionaire.ConcessionaireId,
                Description = ciaVehicleConcessionaire.ConcessionaireDescription,
                vehicleMake = new VehicleMake() { IaVehicleMakeCode = ciaVehicleConcessionaire.VehicleMakeCode },
                IsEnabled = ciaVehicleConcessionaire.IsEnabled
            };
        }

    }
}
