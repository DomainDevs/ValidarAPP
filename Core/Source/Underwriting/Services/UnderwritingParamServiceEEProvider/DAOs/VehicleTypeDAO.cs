// -----------------------------------------------------------------------
// <copyright file="VehicleTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using COMMOD = Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Co.Application.Data;

    /// <summary>
    /// Dao para tipo de vehiculo
    /// </summary>
    public class VehicleTypeDAO
    {
        /// <summary>
        /// Obtiene el identificador del tipo de vehiculo
        /// </summary>
        /// <returns>Identificar nuevo del tipo de vehiculo</returns>
        public int GetIdVehicleType()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleType)));

            int vehicleTypeCode = businessCollection.Cast<VehicleType>().Max(p => p.VehicleTypeCode);
            vehicleTypeCode++;
            return vehicleTypeCode;
        }

        /// <summary>
        /// Borra el tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeCode">Codigo tipo de vehiculo</param>
        /// <returns>Codigo tipo de vehiculo borrado</returns>
        public Result<int, ErrorModel> Delete(int vehicleTypeCode)
        {
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    vehicleBodyDAO.DeleteVehicleTypeBodies(vehicleTypeCode);
                    var filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(VehicleType.Properties.VehicleTypeCode, vehicleTypeCode);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(VehicleType), filter.GetPredicate());
                    transaction.Complete();
                    return new ResultValue<int, ErrorModel>(vehicleTypeCode);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    if (ex.Message.Contains("FOREIGN_KEY"))
                    {
                        return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorForeingKeyVehicleType }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                    }

                    return new ResultError<int, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeleteVehicleType }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }

        /// <summary>
        /// Actualiza el tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeBody">Tipo de vehiculo y carroceria</param>
        /// <returns>Tipo de vehiculos y carrocerias</returns>
        public Result<Models.ParamVehicleTypeBody, ErrorModel> Update(ResultValue<Models.ParamVehicleTypeBody, ErrorModel> vehicleTypeBody)
        {
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            List<string> errorModelListDescription = new List<string>();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(VehicleType.Properties.VehicleTypeCode, typeof(VehicleType).Name);
                    filter.Equal();
                    filter.Constant(vehicleTypeBody.Value.VehicleType.Id);
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleType), filter.GetPredicate()));
                    VehicleType vehicleType = businessCollection.Cast<VehicleType>().FirstOrDefault();
                    vehicleType.Description = vehicleTypeBody.Value.VehicleType.Description;
                    vehicleType.Enabled = vehicleTypeBody.Value.VehicleType.IsEnable;
                    vehicleType.IsTruck = vehicleTypeBody.Value.VehicleType.IsTruck;
                    vehicleType.SmallDescription = vehicleTypeBody.Value.VehicleType.SmallDescription;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(vehicleType);

                    vehicleBodyDAO.DeleteVehicleTypeBodies(vehicleType.VehicleTypeCode);
                    Result<List<Models.ParamVehicleBody>, ErrorModel> vehicleTypeBodiesResult = vehicleBodyDAO.InsertVehicleTypeBodies(vehicleType.VehicleTypeCode, vehicleTypeBody.Value.VehicleBodies);
                    if (vehicleTypeBodiesResult is ResultError<List<Models.ParamVehicleBody>, ErrorModel>)
                    {
                        errorModelListDescription.AddRange(((ResultError<List<Models.ParamVehicleBody>, ErrorModel>)vehicleTypeBodiesResult).Message.ErrorDescription);
                    }

                    Models.ParamVehicleType paramVehicleType = ((ResultValue<Models.ParamVehicleType, ErrorModel>)ModelAssembler.CreateVehicleType(vehicleType)).Value;
                    List<Models.ParamVehicleBody> paramsVehicleBodies = ((ResultValue<List<Models.ParamVehicleBody>, ErrorModel>)vehicleTypeBodiesResult).Value;

                    if (errorModelListDescription.Count > 0)
                    {
                        return new ResultError<Models.ParamVehicleTypeBody, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.TechnicalFault, null));
                    }

                    transaction.Complete();

                    return (ResultValue<Models.ParamVehicleTypeBody, ErrorModel>)Models.ParamVehicleTypeBody.GetParamVehicleTypBody(paramVehicleType, paramsVehicleBodies);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return new ResultError<Models.ParamVehicleTypeBody, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorUpdateVehicleType }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }
        }

        /// <summary>
        /// Inserta el tipo de vehiculo con las carrocerias
        /// </summary>
        /// <param name="vehicleTypeBody">Modelo tipo de vehiculo y carroceria</param>
        /// <returns>Modelo tipo de vehiculo y carrocerias</returns>
        public Result<Models.ParamVehicleTypeBody, ErrorModel> Insert(ResultValue<Models.ParamVehicleTypeBody, ErrorModel> vehicleTypeBody)
        {
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            List<string> errorModelListDescription = new List<string>();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ResultValue<Models.ParamVehicleType, ErrorModel> vehicleTypeValue = (ResultValue<Models.ParamVehicleType, ErrorModel>)Models.ParamVehicleType.GetParamVehicleType(this.GetIdVehicleType(), vehicleTypeBody.Value.VehicleType.Description, vehicleTypeBody.Value.VehicleType.SmallDescription, vehicleTypeBody.Value.VehicleType.IsEnable, vehicleTypeBody.Value.VehicleType.IsTruck);
                    vehicleTypeBody = (ResultValue<Models.ParamVehicleTypeBody, ErrorModel>)Models.ParamVehicleTypeBody.GetParamVehicleTypBody(vehicleTypeValue.Value, vehicleTypeBody.Value.VehicleBodies);

                    VehicleType vehicleType = EntityAssembler.CreateVehicleType(vehicleTypeBody.Value.VehicleType);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(vehicleType);
                    
                    vehicleBodyDAO.DeleteVehicleTypeBodies(vehicleType.VehicleTypeCode);

                    Result<List<Models.ParamVehicleBody>, ErrorModel> vehicleTypeBodiesResult = vehicleBodyDAO.InsertVehicleTypeBodies(vehicleType.VehicleTypeCode, vehicleTypeBody.Value.VehicleBodies);

                    if (vehicleTypeBodiesResult is ResultError<List<Models.ParamVehicleBody>, ErrorModel>)
                    {
                        errorModelListDescription.AddRange(((ResultError<List<Models.ParamVehicleBody>, ErrorModel>)vehicleTypeBodiesResult).Message.ErrorDescription);
                    }

                    Models.ParamVehicleType paramVehicleType = ((ResultValue<Models.ParamVehicleType, ErrorModel>)ModelAssembler.CreateVehicleType(vehicleType)).Value;
                    List<Models.ParamVehicleBody> paramsVehicleBodies = ((ResultValue<List<Models.ParamVehicleBody>, ErrorModel>)vehicleTypeBodiesResult).Value;

                    if (errorModelListDescription.Count > 0)
                    {
                        return new ResultError<Models.ParamVehicleTypeBody, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.TechnicalFault, null));
                    }

                    transaction.Complete();
                    return (ResultValue<Models.ParamVehicleTypeBody, ErrorModel>)Models.ParamVehicleTypeBody.GetParamVehicleTypBody(paramVehicleType, paramsVehicleBodies);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return new ResultError<Models.ParamVehicleTypeBody, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatedVehicleType }, Utilities.Enums.ErrorType.TechnicalFault, ex));
                }
            }   
        }
        
        /// <summary>
        /// Genera el archivo
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta de archivo</returns>
        public Result<string, ErrorModel> GenerateFileToVehicleType(string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationVehicleType;

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

                Result<List<Models.ParamVehicleTypeBody>, ErrorModel> vehicleTypesResult = this.GetVehicleTypes();

                if (vehicleTypesResult is ResultError<List<Models.ParamVehicleTypeBody>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<Models.ParamVehicleTypeBody>, ErrorModel>)vehicleTypesResult).Message);
                }
                else
                {
                    List<Models.ParamVehicleTypeBody> vehicleTypes = ((ResultValue<List<Models.ParamVehicleTypeBody>, ErrorModel>)vehicleTypesResult).Value;
                    foreach (Models.ParamVehicleTypeBody vehicleType in vehicleTypes)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(p => new UTILMO.Field()
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

                        fields[0].Value = vehicleType.VehicleType.Id.ToString();
                        fields[1].Value = vehicleType.VehicleType.Description;
                        fields[2].Value = vehicleType.VehicleType.SmallDescription;
                        fields[3].Value = vehicleType.VehicleType.IsTruck.ToStringFieldExcel();
                        fields[4].Value = vehicleType.VehicleType.IsEnable.ToStringFieldExcel();

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
        /// <returns>Listado de tipo de vehiculos</returns>
        public Result<List<Models.ParamVehicleTypeBody>, ErrorModel> GetVehicleTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            List<Models.ParamVehicleTypeBody> paramVehicleTypeBodies = new List<Models.ParamVehicleTypeBody>();
            
            try
            {
                VehicleTypeBodyView vehicleTypeBodyView = new VehicleTypeBodyView();
                ViewBuilder builder = new ViewBuilder("VehicleTypeBodyView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, vehicleTypeBodyView);

                foreach (VehicleType vehicleType in vehicleTypeBodyView.VehicleTypes)
                {
                    Result<Models.ParamVehicleTypeBody, ErrorModel> paramVehicleTypeBody = ModelAssembler.CreateVehicleTypeBody(vehicleType, vehicleTypeBodyView.GetRelatedBodies(vehicleType));
                    
                    if (paramVehicleTypeBody is ResultError<Models.ParamVehicleTypeBody, ErrorModel>)
                    {
                        return new ResultError<List<Models.ParamVehicleTypeBody>, ErrorModel>(((ResultError<Models.ParamVehicleTypeBody, ErrorModel>)paramVehicleTypeBody).Message);
                    }

                    paramVehicleTypeBodies.Add(((ResultValue<Models.ParamVehicleTypeBody, ErrorModel>)paramVehicleTypeBody).Value);
                }

                if (paramVehicleTypeBodies.Count == 0)
                {
                    errorModelListDescription.Add(Errors.ErrorVehicleTypeNotFound);
                }

                if (errorModelListDescription.Count == 0)
                {
                    return new ResultValue<List<Models.ParamVehicleTypeBody>, ErrorModel>(paramVehicleTypeBodies);
                }
                else
                {
                    return new ResultError<List<Models.ParamVehicleTypeBody>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorGetVehicleType);
                return new ResultError<List<Models.ParamVehicleTypeBody>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
    }
}
