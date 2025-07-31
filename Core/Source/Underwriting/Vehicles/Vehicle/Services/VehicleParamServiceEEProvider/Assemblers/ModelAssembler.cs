// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.Assemblers
{

    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.VehicleParamService.Models;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using System;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views;


    public class ModelAssembler
    {
        public static Result<List<InfringementGroup>, ErrorModel> CreateInfringementGroup(BusinessCollection businessCollection)
        {
            List<InfringementGroup> cptGroupInfringement = new List<InfringementGroup>();
            List<string> errorModelListDescription = new List<string>();
            Result<InfringementGroup, ErrorModel> result;
            foreach (CoCptGroupInfringement entityGroupInfringement in businessCollection)
            {
                result = CreateInfringementGroup(entityGroupInfringement);
                if (result is ResultError<InfringementGroup, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad tipo de riesgo cubierto a modelo de negocio.");
                    return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    InfringementGroup resultValue = (result as ResultValue<InfringementGroup, ErrorModel>).Value;
                    cptGroupInfringement.Add(resultValue);
                }
            }

            return new ResultValue<List<InfringementGroup>, ErrorModel>(cptGroupInfringement);
        }

        public static Result<List<InfringementGroup>, ErrorModel> CreateInfringementGroupActive(BusinessCollection businessCollection)
        {
            List<InfringementGroup> cptGroupInfringement = new List<InfringementGroup>();
            List<string> errorModelListDescription = new List<string>();
            Result<InfringementGroup, ErrorModel> result;
            foreach (CoCptGroupInfringement entityGroupInfringement in businessCollection)
            {
                if (entityGroupInfringement.SnActive)
                {
                    result = CreateInfringementGroup(entityGroupInfringement);
                    if (result is ResultError<InfringementGroup, ErrorModel>)
                    {
                        errorModelListDescription.Add("Ocurrio un error mapeando la entidad tipo de riesgo cubierto a modelo de negocio.");
                        return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(
                            errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        InfringementGroup resultValue = (result as ResultValue<InfringementGroup, ErrorModel>).Value;
                        cptGroupInfringement.Add(resultValue);
                    }
                }
            }

            return new ResultValue<List<InfringementGroup>, ErrorModel>(cptGroupInfringement);
        }

        public static Result<List<Infringement>, ErrorModel> CreateInfringement(BusinessCollection businessCollection)
        {
            List<Infringement> cptInfringement = new List<Infringement>();
            List<string> errorModelListDescription = new List<string>();
            Result<Infringement, ErrorModel> result;
            foreach (CoInfringement entityInfringement in businessCollection)
            {
                result = CreateInfringement(entityInfringement);
                if (result is ResultError<Infringement, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad Infringement a modelo de negocio.");
                    return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    Infringement resultValue = (result as ResultValue<Infringement, ErrorModel>).Value;
                    cptInfringement.Add(resultValue);
                }
            }
            return new ResultValue<List<Infringement>, ErrorModel>(cptInfringement);
        }

        public static Result<List<InfringementState>, ErrorModel> CreateInfringementState(BusinessCollection businessCollection)
        {
            List<InfringementState> cptInfringementState = new List<InfringementState>();
            List<string> errorModelListDescription = new List<string>();
            Result<InfringementState, ErrorModel> result;
            foreach (CoInfringementState entityInfringement in businessCollection)
            {
                result = CreateInfringementState(entityInfringement);
                if (result is ResultError<InfringementState, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad Infringement a modelo de negocio.");
                    return new ResultError<List<InfringementState>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    InfringementState resultValue = (result as ResultValue<InfringementState, ErrorModel>).Value;
                    cptInfringementState.Add(resultValue);
                }
            }
            return new ResultValue<List<InfringementState>, ErrorModel>(cptInfringementState);
        }

        public static ResultValue<InfringementGroup, ErrorModel> CreateInfringementGroup(CoCptGroupInfringement infrigement)
        {
            ResultValue<InfringementGroup, ErrorModel> result = InfringementGroup.GetInfringementGroup(infrigement.GroupInfringementCode, infrigement.Description, infrigement.InfringementOneYear, infrigement.InfringementThreeYears, infrigement.SnActive);
            return result;
        }

        public static ResultValue<Infringement, ErrorModel> CreateInfringement(CoInfringement infrigement)
        {
            ResultValue<Infringement, ErrorModel> result = Infringement.GetInfringement(infrigement.InfringementNewCode, infrigement.InfringementPreviousCode, infrigement.Description, infrigement.GroupInfringementCode, String.Empty);
            return result;
        }

        public static ResultValue<InfringementState, ErrorModel> CreateInfringementState(CoInfringementState infringementState)
        {
            ResultValue<InfringementState, ErrorModel> result = InfringementState.GetInfringementState(infringementState.InfringementStateCode, infringementState.Description);
            return result;
        }
        #region MakeModel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection make"></param>
        /// <returns></returns>
        public static Result<List<ParamVehicleMake>, ErrorModel> CreateVehicleMakes(BusinessCollection businessCollection)
        {
            List<ParamVehicleMake> paramVehicleMakes = new List<ParamVehicleMake>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleMake, ErrorModel> result;
            foreach (VehicleMake entityVehicleMake in businessCollection)
            {
                result = CreateVehicleMake(entityVehicleMake);
                if (result is ResultError<ParamVehicleMake, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad vehicle make a modelo de negocio.");
                    return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleMake resultValue = (result as ResultValue<ParamVehicleMake, ErrorModel>).Value;
                    paramVehicleMakes.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleMake>, ErrorModel>(paramVehicleMakes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <returns></returns>
        public static Result<ParamVehicleModel, ErrorModel> CreateVehicleModel(VehicleModel vehicleModel)
        {

            VehicleMake vehicleMake = new VehicleMake(vehicleModel.VehicleMakeCode);
            Result<ParamVehicleMake, ErrorModel> Type = CreateVehicleMake(vehicleMake);
            ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)Type).Value;
            Result<ParamVehicleModel, ErrorModel> result = ParamVehicleModel.CreateParamVehicleModel(vehicleModel.VehicleModelCode, vehicleModel.Description, vehicleModel.SmallDescription, paramVehicleMake);
            return result;
        }



        #endregion
        #region Fasecolda

        /// <summary>
        /// Funcion para crear modelo de Marca
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static Result<List<ParamVehicleMake>, ErrorModel> CreateMake(BusinessCollection businessCollection)
        {
            List<ParamVehicleMake> Make = new List<ParamVehicleMake>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleMake, ErrorModel> result;
            foreach (VehicleMake entityVehicleMake in businessCollection)
            {
                result = CreateVehicleMake(entityVehicleMake);
                if (result is ResultError<ParamVehicleMake, ErrorModel>)
                {
                    errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                    return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleMake resultValue = (result as ResultValue<ParamVehicleMake, ErrorModel>).Value;
                    Make.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamVehicleMake>, ErrorModel>(Make);
        }

        /// <summary>
        /// Mapeo de la entidad VehicleMake al modelo de negocio ParamMake.
        /// </summary>
        /// <param name="make"></param>
        /// <returns></returns>
        public static Result<ParamVehicleMake, ErrorModel> CreateVehicleMake(VehicleMake make)
        {
            Result<ParamVehicleMake, ErrorModel> result = ParamVehicleMake.GetParamVehicleMake(make.VehicleMakeCode, make.SmallDescription);
            return result;
        }

        /// <summary>
        /// Funcion para crear Modelo de Modelos
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static Result<List<ParamVehicleModel>, ErrorModel> CreateModel(BusinessCollection businessCollection, int makeId)
        {
            List<ParamVehicleModel> Model = new List<ParamVehicleModel>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleModel, ErrorModel> result;
            foreach (VehicleModel entityVehicleModel in businessCollection)
            {
                if (makeId == entityVehicleModel.VehicleMakeCode)
                {
                    result = CreateVehicleModel(entityVehicleModel);
                    if (result is ResultError<ParamVehicleModel, ErrorModel>)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(
                            errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamVehicleModel resultValue = (result as ResultValue<ParamVehicleModel, ErrorModel>).Value;
                        Model.Add(resultValue);
                    }
                }
            }
            return new ResultValue<List<ParamVehicleModel>, ErrorModel>(Model);
        }



        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo 
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public static Result<List<ParamVehicleVersion>, ErrorModel> CreateVersion(BusinessCollection businessCollection, int makeId, int modelId)
        {
            List<ParamVehicleVersion> Version = new List<ParamVehicleVersion>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleVersion, ErrorModel> result;
            foreach (Common.Entities.VehicleVersion entityVehicleVersion in businessCollection)
            {
               // if (makeId == entityVehicleVersion.VehicleMakeCode && modelId == entityVehicleVersion.VehicleModelCode)
                //{
                    result = CreateVehicleVersion(entityVehicleVersion);
                    if (result is ResultError<ParamVehicleVersion, ErrorModel>)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(
                            errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamVehicleVersion resultValue = (result as ResultValue<ParamVehicleVersion, ErrorModel>).Value;
                        Version.Add(resultValue);
                    }
                //}
            }
            return new ResultValue<List<ParamVehicleVersion>, ErrorModel>(Version);
        }


        public static Result<ParamVehicleVersion, ErrorModel> CreateVehicleVersion(VehicleVersion version)
        {
            ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
            ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake)).Value;
            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
            Result<ParamVehicleVersion, ErrorModel> result = ParamVehicleVersion.GetParamVehicleVersion(version.VehicleVersionCode, version.Description, paramVehicleModel, 0, paramVehicleMake, 0, 0, 0, 0,
                paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency);
            return result;
        }

        public static Result<List<ParamVersionVehicleFasecolda>, ErrorModel> CreateVehicleVersionFasecolda(BusinessCollection businessCollection)
        {
            List<ParamVersionVehicleFasecolda> VersionVehicleFasecolda = new List<ParamVersionVehicleFasecolda>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVersionVehicleFasecolda, ErrorModel> result;
            foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
            {
                result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
                if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                {
                    errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                    return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
                    VersionVehicleFasecolda.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVersionVehicleFasecolda>, ErrorModel>(VersionVehicleFasecolda);
        }
        public static Result<List<ParamVersionVehicleFasecolda>, ErrorModel> CreateVehicleVersionFasecolda(BusinessCollection businessCollection, int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            List<ParamVersionVehicleFasecolda> VersionVehicleFasecolda = new List<ParamVersionVehicleFasecolda>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVersionVehicleFasecolda, ErrorModel> result;
            fasecoldaMakeId = fasecoldaMakeId == "0" ? string.Empty : fasecoldaMakeId;
            fasecoldaModelId = fasecoldaModelId == "0" ? string.Empty : fasecoldaModelId;

            versionId = versionId == null ? 0 : versionId;
            modelId = modelId == null ? 0 : modelId;
            makeId = makeId == null ? 0 : makeId;

            if (versionId == 0 && modelId == 0 && makeId == 0 && fasecoldaMakeId == string.Empty && fasecoldaModelId == string.Empty)
            {
                foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
                {
                    result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
                    if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                            errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
                        VersionVehicleFasecolda.Add(resultValue);
                    }
                }
            }

            if (fasecoldaMakeId != string.Empty && fasecoldaModelId != string.Empty)
            {
                foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
                {
                    if (fasecoldaMakeId == entityCoVehicleVersionFasecolda.FasecoldaMakeId && fasecoldaModelId == entityCoVehicleVersionFasecolda.FasecoldaModelId)
                    {
                        result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
                        if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                        {
                            errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                            return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                errorModelListDescription, ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
                            VersionVehicleFasecolda.Add(resultValue);
                        }
                    }
                }
            }
            if (versionId > 0 && modelId > 0 && makeId > 0)
            {
                foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
                {
                    if (versionId == entityCoVehicleVersionFasecolda.VehicleVersionCode && modelId == entityCoVehicleVersionFasecolda.VehicleModelCode && makeId == entityCoVehicleVersionFasecolda.VehicleMakeCode)
                    {
                        result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
                        if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                        {
                            errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                            return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                errorModelListDescription, ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
                            VersionVehicleFasecolda.Add(resultValue);
                        }
                    }
                }
            }
            else if (versionId == 0 && modelId > 0 && makeId > 0)
            {
                foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
                {
                    if (modelId == entityCoVehicleVersionFasecolda.VehicleModelCode && makeId == entityCoVehicleVersionFasecolda.VehicleMakeCode)
                    {
                        result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
                        if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                        {
                            errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                            return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                errorModelListDescription, ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
                            VersionVehicleFasecolda.Add(resultValue);
                        }
                    }
                }
            }
            else if (versionId == 0 && modelId == 0 && makeId > 0)
            {
                foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
                {
                    if (makeId == entityCoVehicleVersionFasecolda.VehicleMakeCode)
                    {
                        result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
                        if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                        {
                            errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                            return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                errorModelListDescription, ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
                            VersionVehicleFasecolda.Add(resultValue);
                        }
                    }
                }
            }

            return new ResultValue<List<ParamVersionVehicleFasecolda>, ErrorModel>(VersionVehicleFasecolda);
        }

        public static Result<List<ParamFasecolda>, ErrorModel> CreateFasecoldaToExport(List<ParamFasecolda> businessCollection, int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            List<ParamFasecolda> VersionVehicleFasecolda = new List<ParamFasecolda>();

            //List<string> errorModelListDescription = new List<string>();
            //Result<ParamFasecolda, ErrorModel> result;
            //fasecoldaMakeId = fasecoldaMakeId == "0" ? string.Empty : fasecoldaMakeId;
            //fasecoldaModelId = fasecoldaModelId == "0" ? string.Empty : fasecoldaModelId;

            //versionId = versionId == null ? 0 : versionId;
            //modelId = modelId == null ? 0 : modelId;
            //makeId = makeId == null ? 0 : makeId;

            //if (versionId == 0 && modelId == 0 && makeId == 0 && fasecoldaMakeId == string.Empty && fasecoldaModelId == string.Empty)
            //{
            //    foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
            //    {
            //        result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
            //        if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
            //        {
            //            errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
            //            return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
            //                errorModelListDescription, ErrorType.BusinessFault, null));
            //        }
            //        else
            //        {
            //            ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
            //            VersionVehicleFasecolda.Add(resultValue);
            //        }
            //    }
            //}

            //if (fasecoldaMakeId != string.Empty && fasecoldaModelId != string.Empty)
            //{
            //    foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
            //    {
            //        if (fasecoldaMakeId == entityCoVehicleVersionFasecolda.FasecoldaMakeId && fasecoldaModelId == entityCoVehicleVersionFasecolda.FasecoldaModelId)
            //        {
            //            result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
            //            if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
            //            {
            //                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
            //                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
            //                    errorModelListDescription, ErrorType.BusinessFault, null));
            //            }
            //            else
            //            {
            //                ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
            //                VersionVehicleFasecolda.Add(resultValue);
            //            }
            //        }
            //    }
            //}
            //if (versionId > 0 && modelId > 0 && makeId > 0)
            //{
            //    foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
            //    {
            //        if (versionId == entityCoVehicleVersionFasecolda.VehicleVersionCode && modelId == entityCoVehicleVersionFasecolda.VehicleModelCode && makeId == entityCoVehicleVersionFasecolda.VehicleMakeCode)
            //        {
            //            result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
            //            if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
            //            {
            //                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
            //                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
            //                    errorModelListDescription, ErrorType.BusinessFault, null));
            //            }
            //            else
            //            {
            //                ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
            //                VersionVehicleFasecolda.Add(resultValue);
            //            }
            //        }
            //    }
            //}
            //else if (versionId == 0 && modelId > 0 && makeId > 0)
            //{
            //    foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
            //    {
            //        if (modelId == entityCoVehicleVersionFasecolda.VehicleModelCode && makeId == entityCoVehicleVersionFasecolda.VehicleMakeCode)
            //        {
            //            result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
            //            if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
            //            {
            //                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
            //                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
            //                    errorModelListDescription, ErrorType.BusinessFault, null));
            //            }
            //            else
            //            {
            //                ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
            //                VersionVehicleFasecolda.Add(resultValue);
            //            }
            //        }
            //    }
            //}
            //else if (versionId == 0 && modelId == 0 && makeId > 0)
            //{
            //    foreach (CoVehicleVersionFasecolda entityCoVehicleVersionFasecolda in businessCollection)
            //    {
            //        if (makeId == entityCoVehicleVersionFasecolda.VehicleMakeCode)
            //        {
            //            result = CreateVehicleVersionFasecolda(entityCoVehicleVersionFasecolda);
            //            if (result is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
            //            {
            //                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
            //                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
            //                    errorModelListDescription, ErrorType.BusinessFault, null));
            //            }
            //            else
            //            {
            //                ParamVersionVehicleFasecolda resultValue = (result as ResultValue<ParamVersionVehicleFasecolda, ErrorModel>).Value;
            //                VersionVehicleFasecolda.Add(resultValue);
            //            }
            //        }
            //    }
            //}

            return new ResultValue<List<ParamFasecolda>, ErrorModel>(VersionVehicleFasecolda);
        }


        public static Result<List<ParamFasecolda>, ErrorModel> CreateAllVehicleVersionFasecolda(CoVehicleVersionFasecoldaView businessCollection, int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            List<ParamFasecolda> VersionVehicleFasecolda = new List<ParamFasecolda>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamFasecolda, ErrorModel> result;
            fasecoldaMakeId = fasecoldaMakeId == "0" ? string.Empty : fasecoldaMakeId;
            fasecoldaModelId = fasecoldaModelId == "0" ? string.Empty : fasecoldaModelId;

            versionId = versionId == null ? 0 : versionId;
            modelId = modelId == null ? 0 : modelId;
            makeId = makeId == null ? 0 : makeId;

            if (versionId == 0 && modelId == 0 && makeId == 0 && fasecoldaMakeId == string.Empty && fasecoldaModelId == string.Empty)
            {
                foreach (CoVehicleVersionFasecolda entityFasecolda in businessCollection.CoVehicleVersionFasecolda)
                {

                    ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                    ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake)).Value;
                    ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
                    ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
                    ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
                    ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
                    ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
                    ParamVehicleVersion paramVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(0, string.Empty, paramVehicleModel,
                        0, paramVehicleMake, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;
                    ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVersion, paramVehicleModel, paramVehicleMake, entityFasecolda.FasecoldaModelId, entityFasecolda.FasecoldaMakeId)).Value;
                    result = CreateAllFasecolda(paramFasecolda);
                    if (result is ResultError<ParamFasecolda, ErrorModel>)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                            errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamFasecolda resultValue = (result as ResultValue<ParamFasecolda, ErrorModel>).Value;
                        VersionVehicleFasecolda.Add(resultValue);
                    }

                }
            }
            if (fasecoldaMakeId != string.Empty && fasecoldaModelId != string.Empty)
            {
                foreach (CoVehicleVersionFasecolda entityFasecolda in businessCollection.CoVehicleVersionFasecolda)
                {
                    foreach (VehicleVersion entityVersion in businessCollection.VehicleVersion)
                    {

                        if (entityFasecolda.VehicleVersionCode == entityVersion.VehicleVersionCode)
                        {
                            ParamVehicleMake paramVehicleMake_A = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                            ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake_A)).Value;
                            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
                            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
                            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
                            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
                            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
                            ParamVehicleVersion paramVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(entityVersion.VehicleVersionCode, entityVersion.Description, paramVehicleModel,
                                0, paramVehicleMake_A, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;

                            foreach (VehicleModel entityModel in businessCollection.VehicleModel)
                            {

                                if (entityFasecolda.VehicleModelCode == entityModel.VehicleModelCode)
                                {
                                    ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                                    ParamVehicleModel paramModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(entityModel.VehicleModelCode, entityModel.Description, string.Empty, paramVehicleMake)).Value;
                                    foreach (VehicleMake entityMake in businessCollection.VehicleMake)
                                    {
                                        if (entityFasecolda.VehicleMakeCode == entityMake.VehicleMakeCode)
                                        {
                                            ParamVehicleMake paramMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(entityMake.VehicleMakeCode, entityMake.SmallDescription)).Value;
                                            ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVersion, paramModel, paramMake, entityFasecolda.FasecoldaModelId, entityFasecolda.FasecoldaMakeId)).Value;
                                            result = CreateAllFasecolda(paramFasecolda);
                                            if (result is ResultError<ParamFasecolda, ErrorModel>)
                                            {
                                                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                                                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                                    errorModelListDescription, ErrorType.BusinessFault, null));
                                            }
                                            else
                                            {
                                                ParamFasecolda resultValue = (result as ResultValue<ParamFasecolda, ErrorModel>).Value;
                                                VersionVehicleFasecolda.Add(resultValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (versionId > 0 && modelId > 0 && makeId > 0)
            {
                foreach (CoVehicleVersionFasecolda entityFasecolda in businessCollection.CoVehicleVersionFasecolda)
                {
                    foreach (VehicleVersion entityVersion in businessCollection.VehicleVersion)
                    {

                        if (entityFasecolda.VehicleVersionCode == entityVersion.VehicleVersionCode)
                        {
                            ParamVehicleMake paramVehicleMake_A = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                            ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake_A)).Value;
                            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
                            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
                            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
                            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
                            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
                            ParamVehicleVersion paramVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(entityVersion.VehicleVersionCode, entityVersion.Description, paramVehicleModel,
                                0, paramVehicleMake_A, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;

                            foreach (VehicleModel entityModel in businessCollection.VehicleModel)
                            {

                                if (entityFasecolda.VehicleModelCode == entityModel.VehicleModelCode)
                                {
                                    ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                                    ParamVehicleModel paramModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(entityModel.VehicleModelCode, entityModel.Description, string.Empty, paramVehicleMake)).Value;
                                    foreach (VehicleMake entityMake in businessCollection.VehicleMake)
                                    {
                                        if (entityFasecolda.VehicleMakeCode == entityMake.VehicleMakeCode)
                                        {
                                            ParamVehicleMake paramMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(entityMake.VehicleMakeCode, entityMake.SmallDescription)).Value;
                                            ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVersion, paramModel, paramMake, entityFasecolda.FasecoldaModelId, entityFasecolda.FasecoldaMakeId)).Value;
                                            result = CreateAllFasecolda(paramFasecolda);
                                            if (result is ResultError<ParamFasecolda, ErrorModel>)
                                            {
                                                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                                                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                                    errorModelListDescription, ErrorType.BusinessFault, null));
                                            }
                                            else
                                            {
                                                ParamFasecolda resultValue = (result as ResultValue<ParamFasecolda, ErrorModel>).Value;
                                                VersionVehicleFasecolda.Add(resultValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (versionId == 0 && modelId > 0 && makeId > 0)
            {
                foreach (CoVehicleVersionFasecolda entityFasecolda in businessCollection.CoVehicleVersionFasecolda)
                {
                    foreach (VehicleVersion entityVersion in businessCollection.VehicleVersion)
                    {

                        if (entityFasecolda.VehicleVersionCode == entityVersion.VehicleVersionCode)
                        {
                            ParamVehicleMake paramVehicleMake_A = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                            ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake_A)).Value;
                            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
                            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
                            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
                            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
                            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
                            ParamVehicleVersion paramVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(entityVersion.VehicleVersionCode, entityVersion.Description, paramVehicleModel,
                               0, paramVehicleMake_A, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;

                            foreach (VehicleModel entityModel in businessCollection.VehicleModel)
                            {

                                if (entityFasecolda.VehicleModelCode == entityModel.VehicleModelCode)
                                {
                                    ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                                    //ParamVehicleModel paramModel = new ParamVehicleModel(0, entityModel.VehicleModelCode, entityModel.Description, string.Empty, paramVehicleMake);
                                    ParamVehicleModel paramModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(entityModel.VehicleModelCode, entityModel.Description, string.Empty, paramVehicleMake)).Value;
                                    foreach (VehicleMake entityMake in businessCollection.VehicleMake)
                                    {
                                        if (entityFasecolda.VehicleMakeCode == entityMake.VehicleMakeCode)
                                        {
                                            ParamVehicleMake paramMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(entityMake.VehicleMakeCode, entityMake.SmallDescription)).Value;
                                            //ParamFasecolda paramFasecolda = new ParamFasecolda(paramVersion, paramModel, paramMake, entityFasecolda.FasecoldaModelId, entityFasecolda.FasecoldaMakeId);
                                            ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVersion, paramModel, paramMake, entityFasecolda.FasecoldaModelId, entityFasecolda.FasecoldaMakeId)).Value;
                                            result = CreateAllFasecolda(paramFasecolda);
                                            if (result is ResultError<ParamFasecolda, ErrorModel>)
                                            {
                                                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                                                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                                    errorModelListDescription, ErrorType.BusinessFault, null));
                                            }
                                            else
                                            {
                                                ParamFasecolda resultValue = (result as ResultValue<ParamFasecolda, ErrorModel>).Value;
                                                VersionVehicleFasecolda.Add(resultValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (versionId == 0 && modelId == 0 && makeId > 0)
            {
                foreach (CoVehicleVersionFasecolda entityFasecolda in businessCollection.CoVehicleVersionFasecolda)
                {
                    foreach (VehicleVersion entityVersion in businessCollection.VehicleVersion)
                    {

                        if (entityFasecolda.VehicleVersionCode == entityVersion.VehicleVersionCode)
                        {
                            ParamVehicleMake paramVehicleMake_A = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                            //ParamVehicleModel paramVehicleModel = new ParamVehicleModel(0, 0, string.Empty, string.Empty, paramVehicleMake_A);
                            ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake_A)).Value;
                            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
                            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
                            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
                            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
                            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
                            ParamVehicleVersion paramVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(0, string.Empty, paramVehicleModel,
                               0, paramVehicleMake_A, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;

                            foreach (VehicleModel entityModel in businessCollection.VehicleModel)
                            {

                                if (entityFasecolda.VehicleModelCode == entityModel.VehicleModelCode)
                                {
                                    ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(0, string.Empty)).Value;
                                    //ParamVehicleModel paramModel = new ParamVehicleModel(0, 0, string.Empty, string.Empty, paramVehicleMake);
                                    ParamVehicleModel paramModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(0, string.Empty, string.Empty, paramVehicleMake)).Value;
                                    foreach (VehicleMake entityMake in businessCollection.VehicleMake)
                                    {
                                        if (entityFasecolda.VehicleMakeCode == entityMake.VehicleMakeCode)
                                        {
                                            ParamVehicleMake paramMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(entityMake.VehicleMakeCode, entityMake.SmallDescription)).Value;
                                            ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVersion, paramModel, paramMake, entityFasecolda.FasecoldaModelId, entityFasecolda.FasecoldaMakeId)).Value;
                                            result = CreateAllFasecolda(paramFasecolda);
                                            if (result is ResultError<ParamFasecolda, ErrorModel>)
                                            {
                                                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                                                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(
                                                    errorModelListDescription, ErrorType.BusinessFault, null));
                                            }
                                            else
                                            {
                                                ParamFasecolda resultValue = (result as ResultValue<ParamFasecolda, ErrorModel>).Value;
                                                VersionVehicleFasecolda.Add(resultValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new ResultValue<List<ParamFasecolda>, ErrorModel>(VersionVehicleFasecolda);
        }

        public static Result<ParamVersionVehicleFasecolda, ErrorModel> CreateVehicleVersionFasecolda(CoVehicleVersionFasecolda vehicleVersionFasecolda)
        {
            Result<ParamVersionVehicleFasecolda, ErrorModel> result = ParamVersionVehicleFasecolda.GetVersionVehicleFasecolda(vehicleVersionFasecolda.VehicleVersionCode, vehicleVersionFasecolda.VehicleModelCode, vehicleVersionFasecolda.VehicleMakeCode, vehicleVersionFasecolda.FasecoldaModelId, vehicleVersionFasecolda.FasecoldaMakeId);
            return result;
        }


        public static Result<ParamFasecolda, ErrorModel> CreateAllFasecolda(ParamFasecolda paramFasecolda)
        {
            Result<ParamFasecolda, ErrorModel> result = ParamFasecolda.GetVersionVehicleFasecolda(paramFasecolda.Version, paramFasecolda.Model, paramFasecolda.Make, paramFasecolda.FasecoldaModelId, paramFasecolda.FasecoldaMakeId);
            return result;
        }

        #endregion

        #region VehicleVersionYear
        /// <summary>
        /// Modelo de negocio ParamVehicleVersionYear
        /// </summary>
        /// <param name="entityVehicleVersionYear">entidad valor de vehiculo por año</param>
        /// <param name="makeDescription">descripcion de la marca</param>
        /// <param name="modelDescription">descripcion del modelo</param>
        /// <param name="versionDescription">descripcion de la version</param>
        /// <returns>MOD-negocio Param de valor de vehiculo por año</returns>
        public static Result<ParamVehicleVersionYear, ErrorModel> CreateParamVehicleVersionYear(VehicleVersionYear entityVehicleVersionYear, string makeDescription, string modelDescription, string versionDescription)
        {
            return new ResultValue<ParamVehicleVersionYear, ErrorModel>(new ParamVehicleVersionYear
            {
                ParamVehicleMake = (ParamVehicleMake.GetParamVehicleMake(entityVehicleVersionYear.VehicleMakeCode, makeDescription) as ResultValue<ParamVehicleMake, ErrorModel>).Value,
                ParamVehicleModel = (ParamVehicleModel.GetParamVehicleModel(entityVehicleVersionYear.VehicleModelCode, modelDescription, string.Empty, (ParamVehicleMake.GetParamVehicleMake(0, string.Empty) as ResultValue<ParamVehicleMake, ErrorModel>).Value) as ResultValue<ParamVehicleModel, ErrorModel>).Value,
                ParamVehicleVersion = ParamVehicleVersion.GetParamVehicleVersionIdDescription(entityVehicleVersionYear.VehicleVersionCode, versionDescription).Value,
                ParamCurrency = ParamCurrency.GetParamCurrency(entityVehicleVersionYear.CurrencyCode, string.Empty).Value,
                Price = entityVehicleVersionYear.VehiclePrice,
                Year = entityVehicleVersionYear.VehicleYear
            });
        }

        /// <summary>
        /// Obtiene la lista del modelo de negocio de valor de vehiculo por año
        /// </summary>
        /// <param name="businessCollection">colleccion de modelo de vehiculo por año</param>
        /// <param name="vehicleMakes">listado de entidad de marcas</param>
        /// <param name="vehicleModels">listado de entidad de modelos</param>
        /// <param name="vehicleVersions">listado de entidad de versiones</param>
        /// <returns>lista de valores de vehiculo por años - Modelo de negocio</returns>
        public static Result<List<ParamVehicleVersionYear>, ErrorModel> CreateParamVehicleVersionYears(BusinessCollection businessCollection, List<VehicleMake> vehicleMakes, List<VehicleModel> vehicleModels, List<VehicleVersion> vehicleVersions)
        {
            List<ParamVehicleVersionYear> paramVehicleVersionYears = new List<ParamVehicleVersionYear>();
            foreach (VehicleVersionYear item in businessCollection)
            {
                string makeDescription = vehicleMakes.Find(b => b.VehicleMakeCode == item.VehicleMakeCode).SmallDescription;
                string modelDescription = vehicleModels.Find(b => b.VehicleModelCode == item.VehicleModelCode).SmallDescription;
                string versionDescription = vehicleVersions.Find(b => b.VehicleVersionCode == item.VehicleVersionCode).Description;
                paramVehicleVersionYears.Add((CreateParamVehicleVersionYear(item, makeDescription, modelDescription, versionDescription) as ResultValue<ParamVehicleVersionYear, ErrorModel>).Value);
            }
            return new ResultValue<List<ParamVehicleVersionYear>, ErrorModel>(paramVehicleVersionYears);
        }
        #endregion

        #region VehicleVersion
        public static Result<List<ParamVehicleVersion>, ErrorModel> CreateParamVehicleVersion(BusinessCollection businessCollection)
        {
            List<ParamVehicleVersion> cptVehicleVersion = new List<ParamVehicleVersion>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleVersion, ErrorModel> result;
            foreach (VehicleVersion entityVehicleVersion in businessCollection)
            {
                result = CreateParamVehicleVersion(entityVehicleVersion);
                if (result is ResultError<ParamVehicleVersion, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleVersion a modelo de negocio.");
                    return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleVersion resultValue = (result as ResultValue<ParamVehicleVersion, ErrorModel>).Value;
                    cptVehicleVersion.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleVersion>, ErrorModel>(cptVehicleVersion);
        }
        public static ResultValue<ParamVehicleVersion, ErrorModel> CreateParamVehicleVersion(VehicleVersion VehicleVersion)
        {
            ParamVehicleFuel VehicleFuel = null;
            ParamVehicleTransmissionType TransmissionType = null;
            ParamCurrency CurrencyCode = null;
            if (VehicleVersion.VehicleFuelCode != null)
            {
                VehicleFuel = ParamVehicleFuel.GetParamVehicleFuel((int)VehicleVersion.VehicleFuelCode, null).Value;
            }
            if (VehicleVersion.TransmissionTypeCode != null)
            {
                TransmissionType = ParamVehicleTransmissionType.GetParamVehicleTransmissionType((int)VehicleVersion.TransmissionTypeCode, null).Value;
            }
            if (VehicleVersion.CurrencyCode != null)
            {
                CurrencyCode = ParamCurrency.GetParamCurrency((int)VehicleVersion.CurrencyCode, null).Value;
            }
            ResultValue<ParamVehicleVersion, ErrorModel> result =
                ParamVehicleVersion.GetParamVehicleVersion
                (
                    VehicleVersion.VehicleVersionCode,
                    VehicleVersion.Description,
                    ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(VehicleVersion.VehicleModelCode, null, null, null)).Value,
                    VehicleVersion.EngineCc,
                    //VehicleVersion.EngineCylQuantity,
                    ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(VehicleVersion.VehicleMakeCode, null)).Value,
                    VehicleVersion.Horsepower,
                    VehicleVersion.Weight,
                    VehicleVersion.TonsQuantity,
                    VehicleVersion.PassengerQuantity,
                    VehicleFuel,
                    ParamVehicleBody.GetParamVehicleBody(VehicleVersion.VehicleBodyCode, null).Value,
                    ParamVehicleType.GetParamVehicleType(VehicleVersion.VehicleTypeCode, null).Value,
                    TransmissionType,
                    VehicleVersion.TopSpeed,
                    VehicleVersion.DoorQuantity,
                    VehicleVersion.NewVehiclePrice,
                    VehicleVersion.IsImported,
                    VehicleVersion.LastModel,
                    CurrencyCode
                    );
            return result;
        }
        #endregion
        #region VehicleBody
        public static Result<List<ParamVehicleBody>, ErrorModel> CreateParamVehicleBody(BusinessCollection businessCollection)
        {
            List<ParamVehicleBody> cptParamVehicleBody = new List<ParamVehicleBody>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleBody, ErrorModel> result;
            foreach (VehicleBody entityParamVehicleBody in businessCollection)
            {
                result = CreateParamVehicleBody(entityParamVehicleBody);
                if (result is ResultError<ParamVehicleBody, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleBody a modelo de negocio.");
                    return new ResultError<List<ParamVehicleBody>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleBody resultValue = (result as ResultValue<ParamVehicleBody, ErrorModel>).Value;
                    cptParamVehicleBody.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleBody>, ErrorModel>(cptParamVehicleBody);
        }
        public static ResultValue<ParamVehicleBody, ErrorModel> CreateParamVehicleBody(VehicleBody VehicleBody)
        {
            ResultValue<ParamVehicleBody, ErrorModel> result = ParamVehicleBody.GetParamVehicleBody(VehicleBody.VehicleBodyCode, VehicleBody.SmallDescription);
            return result;
        }
        #endregion
        #region VehicleFuel
        public static Result<List<ParamVehicleFuel>, ErrorModel> CreateParamVehicleFuel(BusinessCollection businessCollection)
        {
            List<ParamVehicleFuel> cptParamVehicleFuel = new List<ParamVehicleFuel>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleFuel, ErrorModel> result;
            foreach (VehicleFuel entityParamVehicleFuel in businessCollection)
            {
                result = CreateParamVehicleFuel(entityParamVehicleFuel);
                if (result is ResultError<ParamVehicleFuel, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleFuel a modelo de negocio.");
                    return new ResultError<List<ParamVehicleFuel>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleFuel resultValue = (result as ResultValue<ParamVehicleFuel, ErrorModel>).Value;
                    cptParamVehicleFuel.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleFuel>, ErrorModel>(cptParamVehicleFuel);
        }
        public static ResultValue<ParamVehicleFuel, ErrorModel> CreateParamVehicleFuel(VehicleFuel VehicleFuel)
        {
            ResultValue<ParamVehicleFuel, ErrorModel> result = ParamVehicleFuel.GetParamVehicleFuel(VehicleFuel.VehicleFuelCode, VehicleFuel.SmallDescription);
            return result;
        }
        #endregion
        #region VehicleTransmissionType
        public static Result<List<ParamVehicleTransmissionType>, ErrorModel> CreateParamVehicleTransmissionType(BusinessCollection businessCollection)
        {
            List<ParamVehicleTransmissionType> cptParamVehicleTransmissionType = new List<ParamVehicleTransmissionType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleTransmissionType, ErrorModel> result;
            foreach (TransmissionType entityParamVehicleTransmissionType in businessCollection)
            {
                result = CreateParamVehicleTransmissionType(entityParamVehicleTransmissionType);
                if (result is ResultError<ParamVehicleTransmissionType, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleTransmissionType a modelo de negocio.");
                    return new ResultError<List<ParamVehicleTransmissionType>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleTransmissionType resultValue = (result as ResultValue<ParamVehicleTransmissionType, ErrorModel>).Value;
                    cptParamVehicleTransmissionType.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleTransmissionType>, ErrorModel>(cptParamVehicleTransmissionType);
        }
        public static ResultValue<ParamVehicleTransmissionType, ErrorModel> CreateParamVehicleTransmissionType(TransmissionType VehicleTransmissionType)
        {
            ResultValue<ParamVehicleTransmissionType, ErrorModel> result = ParamVehicleTransmissionType.GetParamVehicleTransmissionType(VehicleTransmissionType.TransmissionTypeCode, VehicleTransmissionType.SmallDescription);
            return result;
        }
        #endregion
        #region VehicleType
        public static Result<List<ParamVehicleType>, ErrorModel> CreateParamVehicleType(BusinessCollection businessCollection)
        {
            List<ParamVehicleType> cptParamVehicleType = new List<ParamVehicleType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleType, ErrorModel> result;
            foreach (VehicleType entityParamVehicleType in businessCollection)
            {
                result = CreateParamVehicleType(entityParamVehicleType);
                if (result is ResultError<ParamVehicleType, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad VehicleType a modelo de negocio.");
                    return new ResultError<List<ParamVehicleType>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleType resultValue = (result as ResultValue<ParamVehicleType, ErrorModel>).Value;
                    cptParamVehicleType.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleType>, ErrorModel>(cptParamVehicleType);
        }
        public static ResultValue<ParamVehicleType, ErrorModel> CreateParamVehicleType(VehicleType VehicleType)
        {
            ResultValue<ParamVehicleType, ErrorModel> result = ParamVehicleType.GetParamVehicleType(VehicleType.VehicleTypeCode, VehicleType.SmallDescription);
            return result;
        }
        #endregion
        #region Currency
        public static Result<List<ParamCurrency>, ErrorModel> CreateParamCurrency(BusinessCollection businessCollection)
        {
            List<ParamCurrency> cptParamCurrency = new List<ParamCurrency>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCurrency, ErrorModel> result;
            foreach (Currency entityParamCurrency in businessCollection)
            {
                result = CreateParamCurrency(entityParamCurrency);
                if (result is ResultError<ParamCurrency, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad Currency a modelo de negocio.");
                    return new ResultError<List<ParamCurrency>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamCurrency resultValue = (result as ResultValue<ParamCurrency, ErrorModel>).Value;
                    cptParamCurrency.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamCurrency>, ErrorModel>(cptParamCurrency);
        }
        public static ResultValue<ParamCurrency, ErrorModel> CreateParamCurrency(Currency Currency)
        {
            ResultValue<ParamCurrency, ErrorModel> result = ParamCurrency.GetParamCurrency(Currency.CurrencyCode, Currency.SmallDescription);
            return result;
        }
        #endregion

        #region MakeModel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection make"></param>
        /// <returns></returns>
        public static Result<List<ParamVehicleMake>, ErrorModel> CreateParamVehicleMake(BusinessCollection businessCollection)
        {
            List<ParamVehicleMake> paramVehicleMakes = new List<ParamVehicleMake>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleMake, ErrorModel> result;
            foreach (VehicleMake entityVehicleMake in businessCollection)
            {
                result = CreateParamVehicleMake(entityVehicleMake);
                if (result is ResultError<ParamVehicleMake, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad vehicle make a modelo de negocio.");
                    return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(
                        errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleMake resultValue = (result as ResultValue<ParamVehicleMake, ErrorModel>).Value;
                    paramVehicleMakes.Add(resultValue);
                }
            }
            return new ResultValue<List<ParamVehicleMake>, ErrorModel>(paramVehicleMakes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleMake"></param>
        /// <returns></returns>
        /// 
        public static Result<ParamVehicleMake, ErrorModel> CreateParamVehicleMake(VehicleMake vehicleMake)
        {
            Result<ParamVehicleMake, ErrorModel> result = ParamVehicleMake.GetParamVehicleMake(vehicleMake.VehicleMakeCode, vehicleMake.SmallDescription);
            return result;
        }
        #endregion
        #region ParamVehicleModels
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <returns></returns>
        /// 
        public static Result<List<ParamVehicleModel>, ErrorModel> CreateParamVehicleModels(BusinessCollection businessCollection)
        {
            List<ParamVehicleModel> paramVehicleModelMethods = new List<ParamVehicleModel>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamVehicleModel, ErrorModel> result;
            foreach (VehicleModel entityParamPaymentMethod in businessCollection)
            {
                result = CreateParamVehicleModel(entityParamPaymentMethod);
                if (result is ResultError<ParamVehicleModel, ErrorModel>)
                {
                    errorModelListDescription.Add("Ocurrio un error mapeando la entidad metodo de pago a modelo de negocio.");
                    return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamVehicleModel resultValue = (result as ResultValue<ParamVehicleModel, ErrorModel>).Value;
                    paramVehicleModelMethods.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamVehicleModel>, ErrorModel>(paramVehicleModelMethods);
        }

        public static Result<ParamVehicleModel, ErrorModel> CreateParamVehicleModel(VehicleModel vehicleModel)
        {

            VehicleMake vehicleMake = new VehicleMake(vehicleModel.VehicleMakeCode);
            Result<ParamVehicleMake, ErrorModel> Type = CreateParamVehicleMake(vehicleMake);
            ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)Type).Value;
            Result<ParamVehicleModel, ErrorModel> result = ParamVehicleModel.CreateParamVehicleModel(vehicleModel.VehicleModelCode, vehicleModel.Description, vehicleModel.SmallDescription, paramVehicleMake);
            return result;
        }
        #endregion

    }

}


