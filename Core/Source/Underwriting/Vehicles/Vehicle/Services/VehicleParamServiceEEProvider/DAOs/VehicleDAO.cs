// -----------------------------------------------------------------------
// <copyright file="VehicleDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder camilo Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.DAOs
{
    using Assemblers;
    using Common.Entities;
    using Framework.DAF;
    using Models;
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Resources;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using Utilities.DataFacade;
    using Utilities.Error;
    using daosUtilitiesservices = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using enumsUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Acceso a base de datos para grupos de infracciones
    /// </summary>
    public class VehicleDAO
    {
        /// <summary>
        /// Obtiene la lista de Firma Representante Legal.
        /// </summary>
        /// <returns>Lista de Firma Representante Legal consultadas</returns>
        public Result<List<InfringementGroup>, ErrorModel> GetInfringementGroup()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoCptGroupInfringement)));
                Result<List<InfringementGroup>, ErrorModel> lstCptLegalReprSign = ModelAssembler.CreateInfringementGroup(businessCollection);
                if (lstCptLegalReprSign is ResultError<List<InfringementGroup>, ErrorModel>)
                {
                    return lstCptLegalReprSign;
                }
                else
                {
                    List<InfringementGroup> resultValue = (lstCptLegalReprSign as ResultValue<List<InfringementGroup>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encontraron Firma Representante Legal.");
                        return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstCptLegalReprSign;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de Firma Representante Legal. Comuniquese con el administrador");
                return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        #region Fasecolda

        /// <summary>
        /// Obtiene Listado de Marcas
        /// </summary>
        /// <returns>List<ParamMake></returns>
        public Result<List<ParamVehicleMake>, ErrorModel> GetMakes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleMake)));
                Result<List<ParamVehicleMake>, ErrorModel> makeServiceModel = ModelAssembler.CreateMake(businessCollection);
                if (makeServiceModel is ResultError<List<ParamVehicleMake>, ErrorModel>)
                {
                    return makeServiceModel;
                }
                else
                {
                    List<ParamVehicleMake> resultValue = (makeServiceModel as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return makeServiceModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtiene listado de modelos por Marca
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public Result<List<ParamVehicleModel>, ErrorModel> GetModelsByMakeId(int makeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleModel)));
                Result<List<ParamVehicleModel>, ErrorModel> modelServiceModel = ModelAssembler.CreateModel(businessCollection, makeId);
                if (modelServiceModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                {
                    return modelServiceModel;
                }
                else
                {
                    List<ParamVehicleModel> resultValue = (modelServiceModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return modelServiceModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtiene listado de version de Vehiculo por marca y modelo
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public Result<List<ParamVehicleVersion>, ErrorModel> GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(VehicleVersion.Properties.VehicleMakeCode, typeof(VehicleVersion).Name);
                filter.Equal();
                filter.Constant(makeId);
                filter.And();
                filter.Property(VehicleVersion.Properties.VehicleModelCode, typeof(VehicleVersion).Name);
                filter.Equal();
                filter.Constant(modelId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleVersion), filter.GetPredicate()));
               
                
                Result<List<ParamVehicleVersion>, ErrorModel> versionServiceModel = ModelAssembler.CreateVersion(businessCollection, makeId, modelId);
                if (versionServiceModel is ResultError<List<ParamVehicleVersion>, ErrorModel>)
                {
                    return versionServiceModel;
                }
                else
                {
                    List<ParamVehicleVersion> resultValue = (versionServiceModel as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryVersions);
                        return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return versionServiceModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamVehicleVersion>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }
        /// <summary>
        /// Obtiene la version de Vehiculo segun Marca y Modelo
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public Result<List<ParamVersionVehicleFasecolda>, ErrorModel> GetVersionVehicleFasecoldaByFasecoldaMakeIdByFasecoldaModelId(string fasecoldaMakeId, string fasecoldaModelId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();


                filter.Property(CoVehicleVersionFasecolda.Properties.FasecoldaMakeId, typeof(CoVehicleVersionFasecolda).Name);
                filter.Equal();
                filter.Constant(fasecoldaMakeId);
                filter.And();
                filter.Property(CoVehicleVersionFasecolda.Properties.FasecoldaModelId, typeof(CoVehicleVersionFasecolda).Name);
                filter.Equal();
                filter.Constant(fasecoldaModelId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoVehicleVersionFasecolda), filter.GetPredicate()));
                Result<List<ParamVersionVehicleFasecolda>, ErrorModel> versionServiceModel = ModelAssembler.CreateVehicleVersionFasecolda(businessCollection);
                if (versionServiceModel is ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>)
                {
                    return versionServiceModel;
                }
                else
                {
                    List<ParamVersionVehicleFasecolda> resultValue = (versionServiceModel as ResultValue<List<ParamVersionVehicleFasecolda>, ErrorModel>).Value;
                    if (resultValue == null)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return versionServiceModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// COnsulta la informacion de fasecolda por marca, modelo, version,. codigo de marca y modelo fasecolda
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="modelId"></param>
        /// <param name="makeId"></param>
        /// <param name="fasecoldaMakeId"></param>
        /// <param name="fasecoldaModelId"></param>
        /// <returns></returns>
        public Result<List<ParamVersionVehicleFasecolda>, ErrorModel> GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoVehicleVersionFasecolda)));
                Result<List<ParamVersionVehicleFasecolda>, ErrorModel> versionServiceModel = ModelAssembler.CreateVehicleVersionFasecolda(businessCollection, versionId, modelId, makeId, fasecoldaMakeId, fasecoldaModelId);
                if (versionServiceModel is ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>)
                {
                    return versionServiceModel;
                }
                else
                {
                    List<ParamVersionVehicleFasecolda> resultValue = (versionServiceModel as ResultValue<List<ParamVersionVehicleFasecolda>, ErrorModel>).Value;
                    //if (resultValue.Count == 0)
                    if (resultValue == null)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return versionServiceModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// COnsulta la informacion de fasecolda por marca, modelo, version,. codigo de marca y modelo fasecolda
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="modelId"></param>
        /// <param name="makeId"></param>
        /// <param name="fasecoldaMakeId"></param>
        /// <param name="fasecoldaModelId"></param>
        /// <returns></returns>
        public Result<List<ParamFasecolda>, ErrorModel> GetFasecoldaByMakeIdByModelIdByVersionIdToExport(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoVehicleVersionFasecolda)));
                Result<List<ParamVehicleMake>, ErrorModel> makes = GetMakes();
                List<ParamFasecolda> ListFasecolda = GetDescriptionsMakeModelVersion(businessCollection);

                Result<List<ParamFasecolda>, ErrorModel> versionServiceModel = ModelAssembler.CreateFasecoldaToExport(ListFasecolda, versionId, modelId, makeId, fasecoldaMakeId, fasecoldaModelId);
                if (versionServiceModel is ResultError<List<ParamFasecolda>, ErrorModel>)
                {
                    return versionServiceModel;
                }
                else
                {
                    List<ParamFasecolda> resultValue = (versionServiceModel as ResultValue<List<ParamFasecolda>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                        return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return versionServiceModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }

        public List<ParamFasecolda> GetDescriptionsMakeModelVersion(BusinessCollection businessCollection)
        {
            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;

            List<ParamFasecolda> ListParamFasecolda = new List<ParamFasecolda>();
            Result<List<ParamVehicleMake>, ErrorModel> ListMake = GetMakes();
            List<ParamVehicleMake> resultListMake = (ListMake as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;
            foreach (CoVehicleVersionFasecolda item in businessCollection)
            {
                foreach (ParamVehicleMake make in resultListMake)
                {
                    if (item.VehicleMakeCode == make.Id)
                    {
                        Result<List<ParamVehicleModel>, ErrorModel> ListModel = GetModelsByMakeId(item.VehicleMakeCode);
                        List<ParamVehicleModel> resultListModel = (ListModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                        foreach (ParamVehicleModel model in resultListModel)
                        {
                            if (item.VehicleModelCode == model.Id)
                            {
                                ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(item.VehicleMakeCode, make.Description)).Value;
                                ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(item.VehicleModelCode, model.Description, string.Empty, paramVehicleMake)).Value;

                                Result<List<ParamVehicleVersion>, ErrorModel> ListVersion = GetVersionsByMakeIdModelId(item.VehicleMakeCode, item.VehicleModelCode);
                                List<ParamVehicleVersion> resultListVersion = (ListVersion as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                                foreach (ParamVehicleVersion version in resultListVersion)
                                {
                                    if (version.Id == item.VehicleVersionCode)
                                    {
                                        ParamVehicleVersion paramVehicleVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(item.VehicleVersionCode, version.Description, paramVehicleModel, 0, paramVehicleMake, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;
                                        ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVehicleVersion, paramVehicleModel, paramVehicleMake, item.FasecoldaModelId, item.FasecoldaMakeId)).Value;
                                        ListParamFasecolda.Add(paramFasecolda);
                                    }
                                }
                            }
                        }

                    }
                }


            }
            return ListParamFasecolda;
        }

        public Result<List<ParamFasecolda>, ErrorModel> GetAllVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                CoVehicleVersionFasecoldaView view = new CoVehicleVersionFasecoldaView();
                ViewBuilder builder = new ViewBuilder("CoVehicleVersionFasecoldaView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                BusinessCollection businessCollection = new BusinessCollection();

                if (view != null)
                {
                    Result<List<ParamFasecolda>, ErrorModel> versionServiceModel = ModelAssembler.CreateAllVehicleVersionFasecolda(view, versionId, modelId, makeId, fasecoldaMakeId, fasecoldaModelId);
                    return versionServiceModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorQueryMakes);
                return new ResultError<List<ParamFasecolda>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.VehicleParamService.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Inserta el tipo de vehiculo con las carrocerias
        /// </summary>
        /// <param name="versionVehicleFasecolda">Modelo tipo de vehiculo y carroceria</param>
        /// <returns>Modelo tipo de vehiculo y carrocerias</returns>
        public Result<ParamVersionVehicleFasecolda, ErrorModel> Insert(ResultValue<ParamVersionVehicleFasecolda, ErrorModel> versionVehicleFasecolda)
        {
            //VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            List<string> errorModelListDescription = new List<string>();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ResultValue<ParamVersionVehicleFasecolda, ErrorModel> versionVehicleFasecoldaValue = (ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)ParamVersionVehicleFasecolda.GetVersionVehicleFasecolda(versionVehicleFasecolda.Value.versionId, versionVehicleFasecolda.Value.modelId, versionVehicleFasecolda.Value.makeId, versionVehicleFasecolda.Value.fasecoldaModelId, versionVehicleFasecolda.Value.fasecoldaMakeId);

                    CoVehicleVersionFasecolda coVehicleVersionFasecolda = EntityAssembler.CreateCoVehicleVersionFasecolda(versionVehicleFasecolda.Value);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(coVehicleVersionFasecolda);

                    ParamVersionVehicleFasecolda paramVersionVehicleFasecolda = ((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)ModelAssembler.CreateVehicleVersionFasecolda(coVehicleVersionFasecolda)).Value;

                    transaction.Complete();
                    return (ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)ParamVersionVehicleFasecolda.GetVersionVehicleFasecolda(paramVersionVehicleFasecolda.versionId, paramVersionVehicleFasecolda.modelId, paramVersionVehicleFasecolda.makeId, paramVersionVehicleFasecolda.fasecoldaModelId, paramVersionVehicleFasecolda.fasecoldaMakeId);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return new ResultError<ParamVersionVehicleFasecolda, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "Error creando los datos de fasecolda" }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }

        /// <summary>
        /// Actualiza el registro de Fasecolda
        /// </summary>
        /// <param name="versionVehicleFasecolda">Tipo de vehiculo y carroceria</param>
        /// <returns>Tipo de vehiculos y carrocerias</returns>
        public Result<ParamVersionVehicleFasecolda, ErrorModel> Update(ResultValue<ParamVersionVehicleFasecolda, ErrorModel> versionVehicleFasecolda)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            List<string> errorModelListDescription = new List<string>();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(CoVehicleVersionFasecolda.Properties.VehicleVersionCode, typeof(VehicleType).Name);
                    filter.Equal();
                    filter.Constant(versionVehicleFasecolda.Value.versionId);
                    filter.And();
                    filter.Property(CoVehicleVersionFasecolda.Properties.VehicleModelCode, typeof(VehicleType).Name);
                    filter.Equal();
                    filter.Constant(versionVehicleFasecolda.Value.modelId);
                    filter.And();
                    filter.Property(CoVehicleVersionFasecolda.Properties.VehicleMakeCode, typeof(VehicleType).Name);
                    filter.Equal();
                    filter.Constant(versionVehicleFasecolda.Value.makeId);
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoVehicleVersionFasecolda), filter.GetPredicate()));
                    CoVehicleVersionFasecolda coVehicleVersionFasecolda = businessCollection.Cast<CoVehicleVersionFasecolda>().FirstOrDefault();
                    coVehicleVersionFasecolda.FasecoldaMakeId = versionVehicleFasecolda.Value.fasecoldaMakeId;
                    coVehicleVersionFasecolda.FasecoldaModelId = versionVehicleFasecolda.Value.fasecoldaModelId;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(coVehicleVersionFasecolda);

                    ParamVersionVehicleFasecolda paramVersionVehicleFasecolda = ((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)ModelAssembler.CreateVehicleVersionFasecolda(coVehicleVersionFasecolda)).Value;

                    transaction.Complete();

                    return (ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)ParamVersionVehicleFasecolda.GetVersionVehicleFasecolda(paramVersionVehicleFasecolda.versionId, paramVersionVehicleFasecolda.modelId, paramVersionVehicleFasecolda.makeId, paramVersionVehicleFasecolda.fasecoldaModelId, paramVersionVehicleFasecolda.fasecoldaMakeId);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return new ResultError<ParamVersionVehicleFasecolda, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "error actualizando fasecolda"/*Errors.ErrorUpdateVehicleType*/ }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }

        /// <summary>
        /// Borra el tipo de vehiculo
        /// </summary>
        /// <param name="fasecoldaCode">Codigo tipo de vehiculo</param>
        /// <returns>Codigo tipo de vehiculo borrado</returns>
        public Result<int, ErrorModel> Delete(int fasecoldaCode)
        {
            //VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    var filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(CoVehicleVersionFasecolda.Properties.VehicleVersionCode, fasecoldaCode);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(CoVehicleVersionFasecolda), filter.GetPredicate());

                    transaction.Complete();
                    return new ResultValue<int, ErrorModel>(fasecoldaCode);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    if (ex.Message.Contains("FOREIGN_KEY"))
                    {
                        return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "Error de llave foranea"/*Errors.ErrorForeingKeyVehicleType*/ }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                    }

                    return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "Error eliminando registro de fasecolda"/*Errors.ErrorDeleteVehicleType*/ }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }


        public Result<string, ErrorModel> GenerateFileToVehicleType(string fileName)
        {
            try
            {
                daosUtilitiesservices.FileDAO utilitiesFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationFasecolda;

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

                //Result<List<ParamFasecolda>, ErrorModel> versionVehicleFasecoldasResult = this.GetAllVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(0, 0, 0, string.Empty, string.Empty);
                Result<List<ParamFasecolda>, ErrorModel> versionVehicleFasecoldasResult = this.GenerateFileFasecolda();


                if (versionVehicleFasecoldasResult is ResultError<List<ParamFasecolda>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<ParamFasecolda>, ErrorModel>)versionVehicleFasecoldasResult).Message);
                }
                else
                {
                    List<ParamFasecolda> versionVehicleFasecoldas = ((ResultValue<List<ParamFasecolda>, ErrorModel>)versionVehicleFasecoldasResult).Value;
                    foreach (ParamFasecolda versionVehicleFasecolda in versionVehicleFasecoldas)
                    {
                        List<modelsUtilities.Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new modelsUtilities.Field()
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

                        if (fields.Count < 5)
                        {
                            return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;

                        fields[0].Value = versionVehicleFasecolda.Version.Description.ToString() + " (" + versionVehicleFasecolda.Version.Id + ")";
                        fields[1].Value = versionVehicleFasecolda.Model.Description.ToString() + " (" + versionVehicleFasecolda.Model.Id + ")";
                        fields[2].Value = versionVehicleFasecolda.Make.Description.ToString();
                        fields[3].Value = versionVehicleFasecolda.fasecoldaMakeId;
                        fields[4].Value = versionVehicleFasecolda.fasecoldaModelId;
                        fields[5].Value = versionVehicleFasecolda.fasecoldaMakeId+versionVehicleFasecolda.fasecoldaModelId;

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

        public Result<string, ErrorModel> GenerateFileToFasecolda(string fileName)
        {
            try
            {
                daosUtilitiesservices.FileDAO utilitiesFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationFasecolda;

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

                Result<List<ParamFasecolda>, ErrorModel> versionVehicleFasecoldasResult = this.GetFasecoldaByMakeIdByModelIdByVersionIdToExport(0, 0, 0, string.Empty, string.Empty);


                if (versionVehicleFasecoldasResult is ResultError<List<ParamFasecolda>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<ParamFasecolda>, ErrorModel>)versionVehicleFasecoldasResult).Message);
                }
                else
                {
                    List<ParamFasecolda> versionVehicleFasecoldas = ((ResultValue<List<ParamFasecolda>, ErrorModel>)versionVehicleFasecoldasResult).Value;
                    foreach (ParamFasecolda versionVehicleFasecolda in versionVehicleFasecoldas)
                    {
                        List<modelsUtilities.Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new modelsUtilities.Field()
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

                        if (fields.Count < 5)
                        {
                            return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;

                        fields[0].Value = versionVehicleFasecolda.Version.Description.ToString();
                        fields[1].Value = versionVehicleFasecolda.Model.Description.ToString();
                        fields[2].Value = versionVehicleFasecolda.Make.Description.ToString();
                        fields[3].Value = versionVehicleFasecolda.fasecoldaMakeId;
                        fields[4].Value = versionVehicleFasecolda.fasecoldaModelId;

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

        public Result<List<ParamFasecolda>, ErrorModel> GenerateFileFasecolda()
        {
            DataTable dataTable;
            List<FasecoldaServiceModel> ListFasecolda = new List<FasecoldaServiceModel>();
            List<ParamFasecolda> paramFasecolda = new List<ParamFasecolda>();
            using (DynamicDataAccess dataAccess = new DynamicDataAccess())
            {
                dataTable = dataAccess.ExecuteSPDataTable("COMM.GET_FASECOLDA");
            }

            string generateFile = string.Empty;//DelegateService.commonServiceCore.GenerateFile(file);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    FasecoldaServiceModel Fasecolda = new FasecoldaServiceModel();
                    Fasecolda.Make = new MakeServiceModel();
                    Fasecolda.Version = new VersionServiceModel();
                    Fasecolda.Model = new ModelServiceModel();


                    Fasecolda.Version.Id = Convert.ToInt32(dataTable.Rows[i][0].ToString());
                    Fasecolda.Version.Description = dataTable.Rows[i][1].ToString();
                    Fasecolda.Model.Id = Convert.ToInt32(dataTable.Rows[i][2].ToString());
                    Fasecolda.Model.Description = dataTable.Rows[i][3].ToString();
                    Fasecolda.Make.Id = Convert.ToInt32(dataTable.Rows[i][4].ToString());
                    Fasecolda.Make.Description = dataTable.Rows[i][5].ToString();
                    Fasecolda.FasecoldaModelId = dataTable.Rows[i][6].ToString();
                    Fasecolda.FasecoldaMakeId = dataTable.Rows[i][7].ToString();

                    paramFasecolda.Add(ServicesModelsAssembler.CreateFasecolda(Fasecolda));
                }
            }


            return new ResultValue<List<ParamFasecolda>, ErrorModel>(paramFasecolda);
        }

        #endregion

        #region Consulta DE YEAR

        /// <summary>
        /// Obtiene el valor del vehiculo 
        /// </summary>
        /// <param name="makeId">id de la marca del vehiculo</param>
        /// <param name="modelId">id del modelo del vehiculo</param>
        /// <param name="versionId">id de la version del vehiculo</param>
        /// <param name="year">año del vehiculo</param>
        /// <returns>listado de valor de vehiculos con id de caracteristicas</returns>
        public Result<List<ParamVehicleVersionYear>, ErrorModel> GetParamVehicleVersionYearsByMakeIdModelIdVersionIdYear(int? makeId, int? modelId, int? versionId, int? year)
        {
            try
            {
                bool bandAdd = false;
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (makeId > 0)
                {
                    filter.Property(VehicleMake.Properties.VehicleMakeCode, typeof(VehicleVersionYear).Name);
                    filter.Equal();
                    filter.Constant(makeId);
                    bandAdd = true;
                }
                if (modelId > 0)
                {
                    if (bandAdd)
                    {
                        filter.And();
                    }
                    filter.Property(VehicleVersionYear.Properties.VehicleModelCode, typeof(VehicleVersionYear).Name);
                    filter.Equal();
                    filter.Constant(modelId);
                    bandAdd = true;
                }
                if (versionId > 0)
                {
                    if (bandAdd)
                    {
                        filter.And();
                    }
                    filter.Property(VehicleVersionYear.Properties.VehicleVersionCode, typeof(VehicleVersionYear).Name);
                    filter.Equal();
                    filter.Constant(versionId);
                    bandAdd = true;
                }
                if (year > 0)
                {
                    if (bandAdd)
                    {
                        filter.And();
                    }
                    filter.Property(VehicleVersionYear.Properties.VehicleYear, typeof(VehicleVersionYear).Name);
                    filter.Equal();
                    filter.Constant(year);
                }
                
                VehicleVersionYearView view = new VehicleVersionYearView();
                ViewBuilder builder = new ViewBuilder("VehicleVersionYearView");

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                return ModelAssembler.CreateParamVehicleVersionYears(view.VehicleVersionYears, view.VehicleMakes.Cast<VehicleMake>().ToList(), view.VehicleModels.Cast<VehicleModel>().ToList(), view.VehicleVersions.Cast<VehicleVersion>().ToList());
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamVehicleVersionYear>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { "Error al consultar información asociada al vehiculo, por fallo en DB" }, ErrorType.TechnicalFault, ex));
            }
        }
        #endregion

        #region Exportar excel        
        /// <summary>
        /// Genera el archivo de excel 
        /// </summary>
        /// <param name="makeId">id de la marca</param>
        /// <param name="modelId">id del modelo</param>
        /// <param name="versionId">id de la version</param>
        /// <returns>archivo excel</returns>
        public Result<string, ErrorModel> GenerateFileToVehicleVersionYear(int makeId, int modelId, int versionId)
        {
            try
            {
                string fileName = Resources.Errors.MakeModelVersion;
                Result<List<ParamVehicleVersionYear>, ErrorModel> paramVehicles = GetParamVehicleVersionYearsByMakeIdModelIdVersionIdYear(makeId, modelId, versionId, null);
                if (paramVehicles is ResultError<List<ParamVehicleVersionYear>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel((paramVehicles as ResultError<List<ParamVehicleVersionYear>, ErrorModel>).Message.ErrorDescription, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }

                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue()

                {
                    Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationVehicleVersionYear
                };
                daosUtilitiesservices.FileDAO fileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();

                    foreach (ParamVehicleVersionYear item in (paramVehicles as ResultValue<List<ParamVehicleVersionYear>, ErrorModel>).Value)
                    {
                        ///<summary>
                        ///Se realiza un ordenamiento de columnas utilizando el código OrderBy(x => x.Order) al momento de estar armando la tabla, 
                        ///para corregir los inconvenientes presentados por encabezados de tabla en desorden al generar el archivo Excel. 
                        ///</summary>
                        ///<author>Diego Leon</author>
                        ///<date>17/07/2018</date>
                        ///<purpose>REQ_#080</purpose>
                        ///<returns></returns>
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new modelsUtilities.Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();

                        fields[0].Value = item.ParamVehicleMake?.Description + "(" + item.ParamVehicleMake?.Id.ToString() + ")";
                        fields[1].Value = item.ParamVehicleModel?.Description + "(" + item.ParamVehicleModel?.Id.ToString() + ")";
                        fields[2].Value = item.ParamVehicleVersion?.Description + "(" + item.ParamVehicleVersion?.Id.ToString() + ")";
                        fields[3].Value = item.Year.ToString();
                        fields[4].Value = item.Price.ToString();

                        rows.Add(new modelsUtilities.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new ResultValue<string, ErrorModel>(result);
                }
                else
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDownloadingExcel }, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDownloadingExcel }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
        #endregion
    }
}
