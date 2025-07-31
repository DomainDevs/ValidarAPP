// -----------------------------------------------------------------------
// <copyright file="VehicleBodyDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    /// <summary>
    /// Dao para carroceria
    /// </summary>
    public class VehicleBodyDAO
    {
		
		/// <summary>
        /// Obtiene el identificador de la Carrocería de vehículo
        /// </summary>
        /// <returns>Identificar nuevo de la Carrocería de vehículo</returns>
        public int GetIdVehicleBody()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleBody)));

            int vehicleBodyCode = businessCollection.Cast<VehicleBody>().Max(p => p.VehicleBodyCode);
            vehicleBodyCode++;
            return vehicleBodyCode;
        }
		
		/// <summary>
        /// Borra el Carrocería de vehículo
        /// </summary>
        /// <param name="vehicleBodyCode">Codigo Carrocería de vehículo</param>
        /// <returns>Codigo Carrocería de vehículo borrado</returns>
        public Result<int, ErrorModel> Delete(int vehicleBodyCode)
        {
            VehicleUseDAO vehicleUseDAO = new VehicleUseDAO();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    vehicleUseDAO.DeleteVehicleBodyUses(vehicleBodyCode);
                    var filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(VehicleBody.Properties.VehicleBodyCode, vehicleBodyCode);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(VehicleBody), filter.GetPredicate());
                    transaction.Complete();
                    return new ResultValue<int, ErrorModel>(vehicleBodyCode);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    if (ex.Message.Contains("FOREIGN_KEY"))
                    {
                        return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorForeingKeyVehicleBody }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                    }

                    return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeleteVehicleBody }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }
		
		/// <summary>
        /// Actualiza la Carrocería de vehículo
        /// </summary>
        /// <param name="vehicleBodyUse">Tipo de vehiculo y carroceria</param>
        /// <returns>Tipo de vehiculos y carrocerias</returns>
        public Result<Models.ParamVehicleBodyUse, ErrorModel> Update(ResultValue<Models.ParamVehicleBodyUse, ErrorModel> vehicleBodyUse)
        {
            VehicleUseDAO vehicleUseDAO = new VehicleUseDAO();
            List<string> errorModelListDescription = new List<string>();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(VehicleBody.Properties.VehicleBodyCode, typeof(VehicleBody).Name);
                    filter.Equal();
                    filter.Constant(vehicleBodyUse.Value.VehicleBody.Id);
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleBody), filter.GetPredicate()));
                    VehicleBody vehicleBody = businessCollection.Cast<VehicleBody>().FirstOrDefault();
                    vehicleBody.SmallDescription = vehicleBodyUse.Value.VehicleBody.Description;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(vehicleBody);

                    vehicleUseDAO.DeleteVehicleBodyUses(vehicleBody.VehicleBodyCode);
                    Result<List<Models.ParamVehicleUse>, ErrorModel> vehicleBodyBodiesResult = vehicleUseDAO.InsertVehicleBodyUses(vehicleBody.VehicleBodyCode, vehicleBodyUse.Value.VehicleUses);
                    if (vehicleBodyBodiesResult is ResultError<List<Models.ParamVehicleUse>, ErrorModel>)
                    {
                        errorModelListDescription.AddRange(((ResultError<List<Models.ParamVehicleUse>, ErrorModel>)vehicleBodyBodiesResult).Message.ErrorDescription);
                    }

                    Models.ParamVehicleBody paramVehicleBody = ((ResultValue<Models.ParamVehicleBody, ErrorModel>)ModelAssembler.CreateVehicleBody(vehicleBody)).Value;
                    List<Models.ParamVehicleUse> paramsVehicleBodies = ((ResultValue<List<Models.ParamVehicleUse>, ErrorModel>)vehicleBodyBodiesResult).Value;

                    if (errorModelListDescription.Count > 0)
                    {
                        return new ResultError<Models.ParamVehicleBodyUse, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.TechnicalFault, null));
                    }

                    transaction.Complete();

                    return (ResultValue<Models.ParamVehicleBodyUse, ErrorModel>)Models.ParamVehicleBodyUse.GetParamVehicleBodyUse(paramVehicleBody, paramsVehicleBodies);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return new ResultError<Models.ParamVehicleBodyUse, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorUpdateVehicleBody }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }
		
		/// <summary>
        /// Inserta la Carrocería de vehículo con las carrocerias
        /// </summary>
        /// <param name="vehicleBodyUse">Modelo Carrocería de vehículo y carroceria</param>
        /// <returns>Modelo Carrocería de vehículo y carrocerias</returns>
        public Result<Models.ParamVehicleBodyUse, ErrorModel> Insert(ResultValue<Models.ParamVehicleBodyUse, ErrorModel> vehicleBodyUse)
        {
            VehicleUseDAO vehicleUseDAO = new VehicleUseDAO();
            List<string> errorModelListDescription = new List<string>();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ResultValue<Models.ParamVehicleBody, ErrorModel> vehicleBodyValue = (ResultValue<Models.ParamVehicleBody, ErrorModel>)Models.ParamVehicleBody.GetParamVehicleBody(this.GetIdVehicleBody(), vehicleBodyUse.Value.VehicleBody.Description);
                    vehicleBodyUse = (ResultValue<Models.ParamVehicleBodyUse, ErrorModel>)Models.ParamVehicleBodyUse.GetParamVehicleBodyUse(vehicleBodyValue.Value, vehicleBodyUse.Value.VehicleUses);

                    VehicleBody vehicleBody = EntityAssembler.CreateVehicleBody(vehicleBodyUse.Value.VehicleBody);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(vehicleBody);

                    vehicleUseDAO.DeleteVehicleBodyUses(vehicleBody.VehicleBodyCode);

                    Result<List<Models.ParamVehicleUse>, ErrorModel> vehicleBodyBodiesResult = vehicleUseDAO.InsertVehicleBodyUses(vehicleBody.VehicleBodyCode, vehicleBodyUse.Value.VehicleUses);

                    if (vehicleBodyBodiesResult is ResultError<List<Models.ParamVehicleUse>, ErrorModel>)
                    {
                        errorModelListDescription.AddRange(((ResultError<List<Models.ParamVehicleUse>, ErrorModel>)vehicleBodyBodiesResult).Message.ErrorDescription);
                    }

                    Models.ParamVehicleBody paramVehicleBody = ((ResultValue<Models.ParamVehicleBody, ErrorModel>)ModelAssembler.CreateVehicleBody(vehicleBody)).Value;
                    List<Models.ParamVehicleUse> paramsVehicleBodies = ((ResultValue<List<Models.ParamVehicleUse>, ErrorModel>)vehicleBodyBodiesResult).Value;

                    if (errorModelListDescription.Count > 0)
                    {
                        return new ResultError<Models.ParamVehicleBodyUse, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.TechnicalFault, null));
                    }

                    transaction.Complete();
                    return (ResultValue<Models.ParamVehicleBodyUse, ErrorModel>)Models.ParamVehicleBodyUse.GetParamVehicleBodyUse(paramVehicleBody, paramsVehicleBodies);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return new ResultError<Models.ParamVehicleBodyUse, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatedVehicleBody }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }
		
		/// <summary>
        /// Genera el archivo
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta de archivo</returns>
        public Result<string, ErrorModel> GenerateFileToExportVehicleBody(string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationVehicleBody;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorNotExistsTemplate }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                if (!file.IsEnabled)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorTempleteIsNotEnable }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                file.Name = fileName;
                List<UTILMO.Row> rows = new List<UTILMO.Row>();

                Result<List<Models.ParamVehicleBodyUse>, ErrorModel> vehicleBodysResult = this.GetVehicleBodies();

                if (vehicleBodysResult is ResultError<List<Models.ParamVehicleBodyUse>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<Models.ParamVehicleBodyUse>, ErrorModel>)vehicleBodysResult).Message);
                }
                else
                {
                    List<Models.ParamVehicleBodyUse> vehicleBodys = ((ResultValue<List<Models.ParamVehicleBodyUse>, ErrorModel>)vehicleBodysResult).Value;
                    foreach (Models.ParamVehicleBodyUse vehicleBody in vehicleBodys)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new UTILMO.Field()
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

                        if (fields.Count < 2)
                        {
                            return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;

                        fields[0].Value = vehicleBody.VehicleBody.Id.ToString();
                        fields[1].Value = vehicleBody.VehicleBody.Description;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string generateFile = DelegateService.utilitiesServiceCore.GenerateFile(file);
                    return new ResultValue<string, ErrorModel>(generateFile);
                }
            }
            catch (Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGenerateFile }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }
		
		/// <summary>
        /// Obtiene los tipos de vehiculos
        /// </summary>
        /// <returns>Listado de Carrocería de vehículos</returns>
        public Result<List<Models.ParamVehicleBodyUse>, ErrorModel> GetVehicleBodies()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            List<Models.ParamVehicleBodyUse> paramVehicleBodyBodies = new List<Models.ParamVehicleBodyUse>();

            try
            {
                VehicleBodyUseView vehicleBodyUseView = new VehicleBodyUseView();
                ViewBuilder builder = new ViewBuilder("VehicleBodyUseView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, vehicleBodyUseView);

                foreach (VehicleBody vehicleBody in vehicleBodyUseView.VehicleBodies)
                {
                    Result<Models.ParamVehicleBodyUse, ErrorModel> paramVehicleBodyUse = ModelAssembler.CreateVehicleBodyUse(vehicleBody, vehicleBodyUseView.GetRelatedUses(vehicleBody));
                    if (paramVehicleBodyUse is ResultError<Models.ParamVehicleBodyUse, ErrorModel>)
                    {
                        return new ResultError<List<Models.ParamVehicleBodyUse>, ErrorModel>(((ResultError<Models.ParamVehicleBodyUse, ErrorModel>)paramVehicleBodyUse).Message);
                    }

                    paramVehicleBodyBodies.Add(((ResultValue<Models.ParamVehicleBodyUse, ErrorModel>)paramVehicleBodyUse).Value);
                }

                if (paramVehicleBodyBodies.Count == 0)
                {
                    errorModelListDescription.Add(Errors.ErrorVehicleBodyNotFound);
                }

                if (errorModelListDescription.Count == 0)
                {
                    return new ResultValue<List<Models.ParamVehicleBodyUse>, ErrorModel>(paramVehicleBodyBodies);
                }
                else
                {
                    return new ResultError<List<Models.ParamVehicleBodyUse>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorGetVehicleBody);
                return new ResultError<List<Models.ParamVehicleBodyUse>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
		
        /// <summary>
        /// Inserta los tipos de carroceria al tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeCode">Codigo tipo de vehiculo</param>
        /// <param name="vehicleTypeBody">Listado de carroceria</param>
        /// <returns>Listado de carrocerias</returns>
        public Result<List<ParamVehicleBody>, ErrorModel> InsertVehicleTypeBodies(int vehicleTypeCode, List<ParamVehicleBody> vehicleTypeBody)
        {
            try
            {
                List<ParamVehicleBody> paramVehicleBodies = new List<ParamVehicleBody>();
                if (vehicleTypeBody.Count > 0)
                {
                    List<VehicleTypeBody> vehicleTypeBodies = EntityAssembler.CreateVehicleTypeBody(vehicleTypeCode, vehicleTypeBody);
                    BusinessCollection businessCollection = new BusinessCollection();
                    businessCollection.AddRange(vehicleTypeBodies);
                    DataFacadeManager.Instance.GetDataFacade().InsertObjects(businessCollection);

                    foreach (VehicleTypeBody item in businessCollection)
                    {
                        paramVehicleBodies.Add(((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.CreateParamVehicleBody(item.VehicleBodyCode, "Description")).Value);
                    }
                }

                return new ResultValue<List<ParamVehicleBody>, ErrorModel>(paramVehicleBodies);
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamVehicleBody>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorInsertBodies }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Elimina las carrocerias de un tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeCode">Codigo tipo de vehiculo</param>
        public void DeleteVehicleTypeBodies(int vehicleTypeCode)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(VehicleTypeBody.Properties.VehicleTypeCode, vehicleTypeCode);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(VehicleTypeBody), filter.GetPredicate());
        }

        /// <summary>
        /// Obtiene los tipos de carrocerias por id
        /// </summary>
        /// <param name="idsVehicleBody">Listado de id</param>
        /// <returns>Listado de carrocerias</returns>
        public Result<List<ParamVehicleBody>, ErrorModel> GetVehicleBodiesByIds(List<int> idsVehicleBody)
        {
            try
            {
                List<ParamVehicleBody> vehicleBodies = new List<ParamVehicleBody>();
                if (idsVehicleBody != null)
                {
                    var filter = new ObjectCriteriaBuilder();
                    filter.Property(VehicleBody.Properties.VehicleBodyCode, typeof(VehicleBody).Name);
                    filter.In();
                    filter.ListValue();
                    foreach (int id in idsVehicleBody)
                    {
                        filter.Constant(id);
                    }

                    filter.EndList();
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleBody), filter.GetPredicate()));
                    foreach (VehicleBody vehicleBody in businessCollection)
                    {
                        ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(vehicleBody.VehicleBodyCode, vehicleBody.SmallDescription)).Value;
                        vehicleBodies.Add(paramVehicleBody);
                    }
                }

                return new ResultValue<List<ParamVehicleBody>, ErrorModel>(vehicleBodies);
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamVehicleBody>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorGetBodies }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Genera el archivo de excel
        /// </summary>
        /// <param name="vehicleTypeBody">Modelo de carroceria</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Error o archivo generado</returns>
        public Result<string, ErrorModel> GenerateFileToVehicleBody(ParamVehicleTypeBody vehicleTypeBody, string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationVehicleTypeBody;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorNotExistsTemplate }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                if (!file.IsEnabled)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorTempleteIsNotEnable }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                file.Name = fileName;
                List<UTILMO.Row> rows = new List<UTILMO.Row>();

                foreach (Models.ParamVehicleBody paramVehicleBody in vehicleTypeBody.VehicleBodies)
                {
                    List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new UTILMO.Field()
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

                    if (fields.Count < 3)
                    {
                        return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                    }

                    fields[0].Value = vehicleTypeBody.VehicleType.Id.ToString();
                    fields[1].Value = vehicleTypeBody.VehicleType.Description;
                    fields[2].Value = paramVehicleBody.Description;

                    rows.Add(new UTILMO.Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                string generateFile = DelegateService.utilitiesServiceCore.GenerateFile(file);
                return new ResultValue<string, ErrorModel>(generateFile);
            }
            catch (Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGenerateFile }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
