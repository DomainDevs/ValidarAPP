// -----------------------------------------------------------------------
// <copyright file="VehicleDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.DAOs
{
    using Utilities.DataFacade;
    using Models;
    using System.Collections.Generic;
    using Utilities.Error;
    using Framework.DAF;
    using Common.Entities;
    using Assemblers;
    using Sistran.Core.Application.Utilities.Enums;
    using System;
    using System.Diagnostics;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Framework.Queries;
    using System.Linq;
    using COMMOD = Sistran.Core.Application.CommonService.Models;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Resources;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views;
    using Sistran.Core.Framework.DAF.Engine;
    using System.Data;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using Sistran.Co.Application.Data;
    using daosUtilitiesservices = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using enumsUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Acceso a base de datos para Versiones de Vehiculos
    /// </summary>
    public class VehicleVersionDAO
    {
        /// <summary>
        /// Obtiene la lista de Versiones de vehiculos.
        /// </summary>
        /// <returns>Lista de Versiones de vehiculos consultadas</returns>
        public Result<List<ParamVehicleVersion>, ErrorModel> GetParamVehicleVersion()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleVersion)));
                Result<List<ParamVehicleVersion>, ErrorModel> listParamVehicleVersion = ModelAssembler.CreateParamVehicleVersion(businessCollection);
                if (listParamVehicleVersion is ResultError<List<ParamVehicleVersion>, ErrorModel>)
                {
                    return listParamVehicleVersion;
                }
                else
                {
                    List<ParamVehicleVersion> resultValue = (listParamVehicleVersion as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;

                    return listParamVehicleVersion;
                }
            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.ErrorQueryVehicleVersion);
                return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }
        /// <summary>
        /// Creacion de Version de Vehiculo
        /// </summary>
        /// <param name="ParamVehicleVersion">Modelo a crear</param>
        /// <returns>Modelo Creado</returns>
        public Result<ParamVehicleVersion, ErrorModel> CreateParamVehicleVersion(ParamVehicleVersion ParamVehicleVersion)
        {
            List<string> listErrors = new List<string>();
            try
            {
                VehicleVersion ParamVehicleVersionEntity = EntityAssembler.CreateVehicleVersion(ParamVehicleVersion);
                List<ParamVehicleVersion> list = (GetParamVehicleVersion() as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                bool countExist = false;
                int cont = 0;
                while (!countExist && cont < list.Count)
                {
                    if (list[cont].Description == ParamVehicleVersion.Description)
                    {
                        countExist = true;
                    }
                    cont++;
                }



                if(!countExist)
                {
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(ParamVehicleVersionEntity);
                    ResultValue<ParamVehicleVersion, ErrorModel> ParamVehicleVersionResult = ModelAssembler.CreateParamVehicleVersion(ParamVehicleVersionEntity);
                    return ParamVehicleVersionResult;
                }
                else
                {
                    Exception exception = new Exception();
                    listErrors.Add(Errors.ErrorQueryDescExistVehicleVersion1);
                    return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(listErrors,ErrorType.BusinessFault, exception));
                }               
                
            }
            catch (Exception ex)
            {
                listErrors.Add(Errors.ErrorQueryCreateVehicleVersion);
                return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
        /// <summary>
        /// Actalizar la version del vehiculo
        /// </summary>
        /// <param name="ParamVehicleVersion">Modelo a Actualizar</param>
        /// <returns>Modelo Actualizado</returns>
        public Result<ParamVehicleVersion, ErrorModel> UpdateParamVehicleVersion(ParamVehicleVersion ParamVehicleVersion)
        {
            List<string> listErrors = new List<string>();
            try
            {
                //Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = VehicleVersion.CreatePrimaryKey(ParamVehicleVersion.Id, ParamVehicleVersion.ParamVehicleModel.Id, ParamVehicleVersion.ParamVehicleMake.Id);

                //Encuentra el objeto en referencia a la llave primaria
                VehicleVersion VehicleVersionEntity = (VehicleVersion)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                VehicleVersionEntity.Description = ParamVehicleVersion.Description;
                //VehicleVersionEntity.EngineCylQuantity = ParamVehicleVersion.EngineQuantity;
                VehicleVersionEntity.EngineCc= ParamVehicleVersion.EngineQuantity;
                VehicleVersionEntity.Horsepower = ParamVehicleVersion.HorsePower;
                VehicleVersionEntity.Weight = ParamVehicleVersion.Weight;
                VehicleVersionEntity.TonsQuantity = ParamVehicleVersion.TonsQuantity;
                VehicleVersionEntity.PassengerQuantity = ParamVehicleVersion.PassengerQuantity;

                if (ParamVehicleVersion.ParamVehicleFuel != null)
                {
                    VehicleVersionEntity.VehicleFuelCode = ParamVehicleVersion.ParamVehicleFuel.Id;
                }
                else
                {
                    VehicleVersionEntity.VehicleFuelCode = null;
                }
                if (ParamVehicleVersion.ParamVehicleTransmissionType!=null)
                {
                    VehicleVersionEntity.TransmissionTypeCode = ParamVehicleVersion.ParamVehicleTransmissionType.Id;
                }
                else
                {
                    VehicleVersionEntity.TransmissionTypeCode = null;
                }
                VehicleVersionEntity.VehicleBodyCode = ParamVehicleVersion.ParamVehicleBody.Id;
                VehicleVersionEntity.VehicleTypeCode = ParamVehicleVersion.ParamVehicleType.Id;
               
                VehicleVersionEntity.TopSpeed = ParamVehicleVersion.MaxSpeedQuantity;
                VehicleVersionEntity.DoorQuantity = ParamVehicleVersion.DoorQuantity;
                VehicleVersionEntity.NewVehiclePrice = ParamVehicleVersion.Price;
                VehicleVersionEntity.IsImported = ParamVehicleVersion.IsImported;
                VehicleVersionEntity.CurrencyCode = ParamVehicleVersion.ParamCurrency.Id;
                VehicleVersionEntity.LastModel = ParamVehicleVersion.LastModel;              

                List<ParamVehicleVersion> list = (GetParamVehicleVersion() as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                bool countExist = false;
                //int cont = 0;

                //while (!countExist && list.Count > cont)
                //{
                //    if (list[cont].Description == ParamVehicleVersion.Description)
                //    {
                //        if (cont > 1)
                //        {
                //            countExist = true;
                //        }
                        
                //    }
                //    cont++;
                //}

                if (!countExist)
                {
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(VehicleVersionEntity);
                    ResultValue<ParamVehicleVersion, ErrorModel> ParamVehicleVersionResult = ModelAssembler.CreateParamVehicleVersion(VehicleVersionEntity);
                    return ParamVehicleVersionResult;
                }
                else
                {
                    Exception exception = new Exception();
                    listErrors.Add(Errors.ErrorQueryDescExistVehicleVersion1);
                    return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, exception));
                }


            }
            catch (Exception ex)
            {
               
                listErrors.Add(Errors.ErrorQueryUpdateVehicleVersion);
                return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
        

        /// <summary>
        /// Borra la version del vehiculo
        /// </summary>
        /// <param name="ParamVehicleVersion">Modelo a Eliminar</param>
        /// <returns>Codigo tipo de version de vehiculo borrado</returns>
        public Result<int, ErrorModel> DeleteParamVehicleVersion(ParamVehicleVersion ParamVehicleVersion)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    PrimaryKey primaryKey = VehicleVersion.CreatePrimaryKey(ParamVehicleVersion.Id, ParamVehicleVersion.ParamVehicleModel.Id, ParamVehicleVersion.ParamVehicleMake.Id);
                    VehicleVersion VehicleVersionEntity = (VehicleVersion)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(VehicleVersionEntity);
                    transaction.Complete();
                    return new ResultValue<int, ErrorModel>(ParamVehicleVersion.Id);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    if (ex.Message.Contains("RELATED_OBJECT"))
                    {
                        return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorForeignKeyVehicleVersion }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                    }

                    return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorQueryDeleteVehicleVersion }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }

        /// <summary>
        /// Obtiene la lista de Versiones de vehiculos.
        /// </summary>
        /// <param name="makeCode">Codigo de la marca</param>
        /// <param name="descriptionMake">Descripcion de la marca</param>
        /// <param name="modelCode">Codigo del modelo</param>
        /// <param name="descriptionModel">Descripcion del modelo</param>
        /// <param name="versionCode">Codigo de la version</param>
        /// <param name="descriptionVersion">Descripcion de la version</param>
        /// <returns>Lista de Versiones de vehiculos consultadas</returns>
        public Result<List<ParamVehicleVersion>, ErrorModel> GetParamVehicleVersionByMakeModelVersion(int? makeCode, int? modelCode, string description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (!string.IsNullOrEmpty(description))
                {
                    filter.Property(VehicleVersion.Properties.Description, typeof(VehicleVersion).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }
                if (makeCode > 0)
                {
                    if (!string.IsNullOrEmpty(description))
                    {
                        filter.And();
                    }

                    filter.Property(VehicleVersion.Properties.VehicleMakeCode, typeof(VehicleVersion).Name);
                    filter.Equal();
                    filter.Constant(makeCode);
                }
                if (modelCode > 0)
                {
                    if (makeCode > 0)
                    {
                        filter.And();
                    }
                    filter.Property(VehicleVersion.Properties.VehicleModelCode, typeof(VehicleVersion).Name);
                    filter.Equal();
                    filter.Constant(modelCode);
                }

                ClauseVehicleVersion view = new ClauseVehicleVersion();
                ViewBuilder builder = new ViewBuilder("ClauseVehicleVersion");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                Result<List<ParamVehicleVersion>, ErrorModel> ResultVehicleVersion = ModelAssembler.CreateParamVehicleVersion(view.VehicleVersion);
                Result<List<ParamVehicleMake>, ErrorModel> ResultVehicleMake = ModelAssembler.CreateParamVehicleMake(view.VehicleMake);
                Result<List<ParamVehicleModel>, ErrorModel> ResultVehicleModel = ModelAssembler.CreateParamVehicleModels(view.VehicleModel);
                List<ParamVehicleVersion> Result = new List<ParamVehicleVersion>();
                if (ResultVehicleVersion is ResultError<List<ParamVehicleVersion>, ErrorModel> || ResultVehicleMake is ResultError<List<ParamVehicleMake>, ErrorModel> || ResultVehicleModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                {
                    errorModel.Add(Errors.ErrorQueryJoinVehicleVersion);
                    return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, null));
                }
                else
                {
                    List<ParamVehicleMake> ListMakes = ((ResultValue<List<ParamVehicleMake>, ErrorModel>)ResultVehicleMake).Value;
                    List<ParamVehicleModel> ListModel = ((ResultValue<List<ParamVehicleModel>, ErrorModel>)ResultVehicleModel).Value;
                    foreach (VehicleVersion VehicleVersion in view.VehicleVersion)
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
                        ParamVehicleMake Make = ListMakes.Where(f => f.Id == VehicleVersion.VehicleMakeCode).FirstOrDefault();
                        ParamVehicleModel Model = ListModel.Where(f => f.Id == VehicleVersion.VehicleModelCode && f.VehicleMake.Id == VehicleVersion.VehicleMakeCode).FirstOrDefault();
                        ResultValue<ParamVehicleVersion, ErrorModel> resultGetVersion =
                        ParamVehicleVersion.GetParamVehicleVersion
                        (
                            VehicleVersion.VehicleVersionCode,
                            VehicleVersion.Description,
                            Model,
                            VehicleVersion.EngineCc,
                            //VehicleVersion.EngineCylQuantity,
                            Make,
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
                        Result.Add(resultGetVersion.Value);
                    }
                    return new ResultValue<List<ParamVehicleVersion>, ErrorModel>(Result);
                }

            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.ErrorQueryVehicleVersion);
                return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene la lista de Versiones de vehiculos.
        /// </summary>
        /// <returns>Lista de Versiones de vehiculos consultadas</returns>
        public Result<List<ParamVehicleVersion>, ErrorModel> GetParamVehicleVersionByMakeModelVersion()
        {
            List<string> errorModel = new List<string>();
            try
            {
               
                ClauseVehicleVersion view = new ClauseVehicleVersion();
                ViewBuilder builder = new ViewBuilder("ClauseVehicleVersion");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                Result<List<ParamVehicleVersion>, ErrorModel> ResultVehicleVersion = ModelAssembler.CreateParamVehicleVersion(view.VehicleVersion);
                Result<List<ParamVehicleMake>, ErrorModel> ResultVehicleMake = ModelAssembler.CreateParamVehicleMake(view.VehicleMake);
                Result<List<ParamVehicleModel>, ErrorModel> ResultVehicleModel = ModelAssembler.CreateParamVehicleModels(view.VehicleModel);
                List<ParamVehicleVersion> Result = new List<ParamVehicleVersion>();
                if (ResultVehicleVersion is ResultError<List<ParamVehicleVersion>, ErrorModel> || ResultVehicleMake is ResultError<List<ParamVehicleMake>, ErrorModel> || ResultVehicleModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                {
                    errorModel.Add(Errors.ErrorQueryJoinVehicleVersion);
                    return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, null));
                }
                else
                {
                    List<ParamVehicleMake> ListMakes = ((ResultValue<List<ParamVehicleMake>, ErrorModel>)ResultVehicleMake).Value;
                    List<ParamVehicleModel> ListModel = ((ResultValue<List<ParamVehicleModel>, ErrorModel>)ResultVehicleModel).Value;
                    foreach (VehicleVersion VehicleVersion in view.VehicleVersion)
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
                        ParamVehicleMake Make = ListMakes.Where(f => f.Id == VehicleVersion.VehicleMakeCode).FirstOrDefault();
                        ParamVehicleModel Model = ListModel.Where(f => f.Id == VehicleVersion.VehicleModelCode && f.VehicleMake.Id == VehicleVersion.VehicleMakeCode).FirstOrDefault();
                        ResultValue<ParamVehicleVersion, ErrorModel> resultGetVersion =
                        ParamVehicleVersion.GetParamVehicleVersion
                        (
                            VehicleVersion.VehicleVersionCode,
                            VehicleVersion.Description,
                            Model,
                            VehicleVersion.EngineCylQuantity,
                            Make,
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
                        Result.Add(resultGetVersion.Value);
                    }
                    return new ResultValue<List<ParamVehicleVersion>, ErrorModel>(Result);
                }

            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.ErrorQueryVehicleVersion);
                return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }


        public Result<List<ParamVehicleVersion>, ErrorModel> GetParamVehicleVersionByDescription(string description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (!string.IsNullOrEmpty(description))
                {
                    filter.Property(VehicleVersion.Properties.Description, typeof(VehicleVersion).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }
                ClauseVehicleVersion view = new ClauseVehicleVersion();
                ViewBuilder builder = new ViewBuilder("ClauseVehicleVersion");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                Result<List<ParamVehicleVersion>, ErrorModel> ResultVehicleVersion = ModelAssembler.CreateParamVehicleVersion(view.VehicleVersion);
                Result<List<ParamVehicleMake>, ErrorModel> ResultVehicleMake = ModelAssembler.CreateParamVehicleMake(view.VehicleMake);
                Result<List<ParamVehicleModel>, ErrorModel> ResultVehicleModel = ModelAssembler.CreateParamVehicleModels(view.VehicleModel);
                List<ParamVehicleVersion> Result = new List<ParamVehicleVersion>();
                if (ResultVehicleVersion is ResultError<List<ParamVehicleVersion>, ErrorModel> || ResultVehicleMake is ResultError<List<ParamVehicleMake>, ErrorModel> || ResultVehicleModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                {
                    errorModel.Add("Error Consultando las entidades Relacionadas a VehicleVersion");
                    return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, null));
                }
                else
                {
                    List<ParamVehicleMake> ListMakes = ((ResultValue<List<ParamVehicleMake>, ErrorModel>)ResultVehicleMake).Value;
                    List<ParamVehicleModel> ListModel = ((ResultValue<List<ParamVehicleModel>, ErrorModel>)ResultVehicleModel).Value;
                    foreach (VehicleVersion VehicleVersion in view.VehicleVersion)
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
                        ParamVehicleMake Make = ListMakes.Where(f => f.Id == VehicleVersion.VehicleMakeCode).FirstOrDefault();
                        ParamVehicleModel Model = ListModel.Where(f => f.Id == VehicleVersion.VehicleModelCode && f.VehicleMake.Id == VehicleVersion.VehicleMakeCode).FirstOrDefault();
                        ResultValue<ParamVehicleVersion, ErrorModel> resultGetVersion =
                        ParamVehicleVersion.GetParamVehicleVersion
                        (
                            VehicleVersion.VehicleVersionCode,
                            VehicleVersion.Description,
                            Model,
                            VehicleVersion.EngineCc,
                            //VehicleVersion.EngineCylQuantity,
                            Make,
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
                        Result.Add(resultGetVersion.Value);
                    }
                    return new ResultValue<List<ParamVehicleVersion>, ErrorModel>(Result);
                }

            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la version del vehiculo. Comuniquese con el administrador");
                return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<string,ErrorModel> GenerateFileToVehicleVersion(string fileName, int? makeCode, int? modelCode) {
            try
            {
                daosUtilitiesservices.FileDAO utilitiesFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationVehicleVersion;

                modelsUtilities.File file = utilitiesFileDAO.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorNotExistsTemplate }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                if (!file.IsEnabled)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorTempleteIsNotEnable }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                file.Name = fileName;
                List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();

                Result<List<ParamVehicleVersion>, ErrorModel> ParamVehicleVersionResult = GetParamVehicleVersionByMakeModelVersion(makeCode, modelCode, null);//GetParamVehicleVersionStoreProcedure();

                if (ParamVehicleVersionResult is ResultError<List<ParamVehicleVersion>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<ParamVehicleVersion>, ErrorModel>)ParamVehicleVersionResult).Message);
                }
                else
                {
                    List<ParamVehicleVersion> VehicleVersion = ((ResultValue<List<ParamVehicleVersion>, ErrorModel>)ParamVehicleVersionResult).Value;
                    if (VehicleVersion.Count==0)
                    {
                        return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorVehicleVersionNoData }, Utilities.Enums.ErrorType.TechnicalFault,null));
                    }
                    foreach (ParamVehicleVersion itemVehicleVersion in VehicleVersion)
                    {
                        ///<summary>
                        ///Se realiza un ordenamiento de columnas utilizando el código OrderBy(x => x.Order) al momento de estar armando la tabla, 
                        ///para corregir los inconvenientes presentados por encabezados de tabla en desorden al generar el archivo Excel. 
                        ///</summary>
                        ///<author>Diego Leon</author>
                        ///<date>17/07/2018</date>
                        ///<purpose>REQ_#079</purpose>
                        ///<returns></returns>
                        List<modelsUtilities.Field> fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(p => new modelsUtilities.Field()
                        {
                            ColumnSpan = p.ColumnSpan,
                            Description = p.Description,
                            FieldType = p.FieldType,
                            Id = p.Id,
                            IsEnabled = p.IsEnabled,
                            IsMandatory = p.IsMandatory,
                            Order = p.Order,
                            RowPosition = p.RowPosition,
                            SmallDescription = p.SmallDescription
                        }).ToList();

                        if (fields.Count < 6)
                        {
                            return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;
                        fields[0].Value = itemVehicleVersion.ParamVehicleMake.Id.ToString();
                        fields[1].Value = itemVehicleVersion.ParamVehicleMake.Description.ToString();
                        fields[2].Value = itemVehicleVersion.ParamVehicleModel.Id.ToString();
                        fields[3].Value = itemVehicleVersion.ParamVehicleModel.Description.ToString();
                        fields[4].Value = itemVehicleVersion.Id.ToString();
                        fields[5].Value = itemVehicleVersion.Description.ToString();

                        rows.Add(new modelsUtilities.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string generateFile = utilitiesFileDAO.GenerateFile(file);
                    return new ResultValue<string, ErrorModel>(generateFile);
                }
            }
            catch (Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGenerateFile }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }
        public Result<string, ErrorModel> GenerateFileToVehicleVersion(string fileName)
        {
            try
            {
                daosUtilitiesservices.FileDAO utilitiesFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationVehicleVersion;

                modelsUtilities.File file = utilitiesFileDAO.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorNotExistsTemplate }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                if (!file.IsEnabled)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorTempleteIsNotEnable }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                file.Name = fileName;
                List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();

                Result<List<ParamVehicleVersion>, ErrorModel> ParamVehicleVersionResult = GetParamVehicleVersionByMakeModelVersion();//GetParamVehicleVersionStoreProcedure();

                if (ParamVehicleVersionResult is ResultError<List<ParamVehicleVersion>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<ParamVehicleVersion>, ErrorModel>)ParamVehicleVersionResult).Message);
                }
                else
                {
                    List<ParamVehicleVersion> VehicleVersion = ((ResultValue<List<ParamVehicleVersion>, ErrorModel>)ParamVehicleVersionResult).Value;
                    if (VehicleVersion.Count == 0)
                    {
                        return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorVehicleVersionNoData }, Utilities.Enums.ErrorType.TechnicalFault, null));
                    }
                    foreach (ParamVehicleVersion itemVehicleVersion in VehicleVersion)
                    {
                        List<modelsUtilities.Field> fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(p => new modelsUtilities.Field()
                        {
                            ColumnSpan = p.ColumnSpan,
                            Description = p.Description,
                            FieldType = p.FieldType,
                            Id = p.Id,
                            IsEnabled = p.IsEnabled,
                            IsMandatory = p.IsMandatory,
                            Order = p.Order,
                            RowPosition = p.RowPosition,
                            SmallDescription = p.SmallDescription
                        }).ToList();

                        if (fields.Count < 6)
                        {
                            return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;
                        fields[0].Value = itemVehicleVersion.ParamVehicleMake.Id.ToString();
                        fields[1].Value = itemVehicleVersion.ParamVehicleMake.Description.ToString();
                        fields[2].Value = itemVehicleVersion.ParamVehicleModel.Id.ToString();
                        fields[3].Value = itemVehicleVersion.ParamVehicleModel.Description.ToString();
                        fields[4].Value = itemVehicleVersion.Id.ToString();
                        fields[5].Value = itemVehicleVersion.Description.ToString();

                        rows.Add(new modelsUtilities.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string generateFile = utilitiesFileDAO.GenerateFile(file);
                    return new ResultValue<string, ErrorModel>(generateFile);
                }
            }
            catch (Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGenerateFile }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }
        public Result<List<ParamVehicleVersion>, ErrorModel> GetParamVehicleVersionStoreProcedure()
        {
            DataTable dataTable;
            List<ParamVehicleVersion> paramVehicleVersion = new List<ParamVehicleVersion>();
            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                dataTable = dataAccess.ExecuteSPDataTable("COMM.CPT_GET_VEHICLE_VERSION_EXCEL");
            }
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    VehicleVersionServiceModel serviceModelVehicleVersion = new VehicleVersionServiceModel();
                    serviceModelVehicleVersion.VehicleModelServiceQueryModel = new VehicleModelServiceQueryModel() {Id=Convert.ToInt32(dataTable.Rows[i][4].ToString()),Description=dataTable.Rows[i][5].ToString() };
                    serviceModelVehicleVersion.VehicleBodyServiceQueryModel = new ModelServices.Models.UnderwritingParam.VehicleBodyServiceQueryModel();
                    serviceModelVehicleVersion.VehicleTypeServiceQueryModel = new VehicleTypeServiceQueryModel();
                    serviceModelVehicleVersion.VehicleMakeServiceQueryModel = new VehicleMakeServiceQueryModel() { Id = Convert.ToInt32(dataTable.Rows[i][2].ToString()), Description = dataTable.Rows[i][3].ToString() };
                    serviceModelVehicleVersion.CurrencyServiceQueryModel = new ModelServices.Models.UnderwritingParam.CurrencyServiceQueryModel();
                    serviceModelVehicleVersion.Id = Convert.ToInt32(dataTable.Rows[i][0].ToString());
                    serviceModelVehicleVersion.Description = dataTable.Rows[i][1].ToString();
                    serviceModelVehicleVersion.Price = 0;
                    paramVehicleVersion.Add(((ResultValue<ParamVehicleVersion, ErrorModel>)ModelsServicesAssembler.CreateParamVehicleVersion(serviceModelVehicleVersion)).Value);
                }
            }
            return new ResultValue<List<ParamVehicleVersion>, ErrorModel>(paramVehicleVersion);
        }
    }
}
