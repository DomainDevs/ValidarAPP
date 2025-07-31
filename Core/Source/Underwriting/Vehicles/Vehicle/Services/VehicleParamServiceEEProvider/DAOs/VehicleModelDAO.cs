// -----------------------------------------------------------------------
// <copyright file="ParamVehicleModelDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>John Jairo Peralta</author>
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
    using Framework.Queries;
    using Sistran.Core.Framework.Transactions;    
    using System.Linq;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views;
    using CommonService.Enums;
    using CommonService.Models;
    using daosUtilitiesservices = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using enumsUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;


    public class VehicleModelDAO
    {
        public Result<List<ParamVehicleModel>, ErrorModel> GetParamVehicleModelsByMake(int MakeID)
        {
            using (Transaction transaction = new Transaction())
            {
                List<string> errorModel = new List<string>();
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(VehicleModel.Properties.VehicleMakeCode, MakeID);
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleModel), filter.GetPredicate()));

                    Result<List<ParamVehicleModel>, ErrorModel> lstParamVehicleModel = ModelAssembler.CreateParamVehicleModels(businessCollection);

                    if (lstParamVehicleModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                    {
                        return lstParamVehicleModel;
                    }
                    else
                    {
                        List<ParamVehicleModel> resultValue = (lstParamVehicleModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;

                        if (resultValue.Count == 0)
                        {
                            errorModel.Add("No se encuentra El metodo de pago.");
                            return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                        }
                        else
                        {
                            return lstParamVehicleModel;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorModel.Add("Ocurrio un error inesperado en la consulta de metodos de pago. Comuniquese con el administrador");
                    return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
                }
            }
        }
        public Result<List<ParamVehicleModel>, ErrorModel> GetParamVehicleModels()
        {
            using (Transaction transaction = new Transaction())
            {
                List<string> errorModel = new List<string>();
                try
                {
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleModel)));
                    Result<List<ParamVehicleModel>, ErrorModel> lstParamVehicleModel = ModelAssembler.CreateParamVehicleModels(businessCollection);

                    if (lstParamVehicleModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                    {
                        return lstParamVehicleModel;
                    }
                    else
                    {
                        List<ParamVehicleModel> resultValue = (lstParamVehicleModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;

                        if (resultValue.Count == 0)
                        {
                            errorModel.Add("No se encuentra el modelo");
                            return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                        }
                        else
                        {
                            return lstParamVehicleModel;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorModel.Add(Resources.Errors.ErrorAdmin);
                    return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
                }
            }
        }
        public Result<ParamVehicleModel, ErrorModel> ParamVehicleModelCreate(ParamVehicleModel paramVehicleModel)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleModel)));
                    var enumerable = businessCollection.Select(x => (VehicleModel)x);
                    int id = enumerable.Max(p => p.VehicleModelCode) + 1;
                    Result<ParamVehicleModel, ErrorModel> item = ParamVehicleModel.CreateParamVehicleModel(id, paramVehicleModel.Description, paramVehicleModel.SmallDescription, paramVehicleModel.VehicleMake);

                    if (item is ResultError<ParamVehicleModel, ErrorModel>)
                    {
                        transaction.Dispose();
                        List<string> listErrors = new List<string>();
                        listErrors.Add(Resources.Errors.FailedCreatingModel);
                        return new ResultError<ParamVehicleModel, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, null));
                    }
                    else
                    {

                        VehicleModel vehicle = EntityAssembler.CreateVehicleModel(((ResultValue<ParamVehicleModel, ErrorModel>)item).Value);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(vehicle);

                        Result<ParamVehicleModel, ErrorModel> resultParamVehicle = ModelAssembler.CreateVehicleModel(vehicle);
                        transaction.Complete();
                        return resultParamVehicle;
                    }

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add(Resources.Errors.FailedCreatingModel);
                    return new ResultError<ParamVehicleModel, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
                }
            }

        }
        public Result<ParamVehicleModel, ErrorModel> paramVehicleModelUpdate(ParamVehicleModel paramVehicleUpdate)
        {
            try
            {
                //Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = VehicleModel.CreatePrimaryKey(paramVehicleUpdate.Id, paramVehicleUpdate.VehicleMake.Id);

                //Encuentra el objeto en referencia a la llave primaria
                VehicleModel vehicleModel = (VehicleModel)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                vehicleModel.Description = paramVehicleUpdate.Description;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(vehicleModel);
                Result<ParamVehicleModel, ErrorModel> ParamVehicleModelResult = ModelAssembler.CreateVehicleModel(vehicleModel);
                return ParamVehicleModelResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
              // listErrors.Add(listErrors.);
                return new ResultError<ParamVehicleModel, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
    
        public Result<ParamVehicleModel, ErrorModel> paramVehicleModelDelete(ParamVehicleModel paramVehicleModelDelete)
        {
           
            {
                try
                    
                {
                    PrimaryKey primaryKey = VehicleModel.CreatePrimaryKey(paramVehicleModelDelete.Id, paramVehicleModelDelete.VehicleMake.Id);
                    VehicleModel vehicleModel = (VehicleModel)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                    

                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(vehicleModel);
                    Result<ParamVehicleModel, ErrorModel> ParamVehicleModelResult = ModelAssembler.CreateVehicleModel(vehicleModel);
                    return ParamVehicleModelResult;
                }
                catch (Exception ex)
                {
                  
                    List<string> listErrors = new List<string>();
                    // listErrors.Add(listErrors);
                    return new ResultError<ParamVehicleModel, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
                }
            }

        }
        public Result<List<ParamVehicleModel>, ErrorModel> GetVehicleModelByDescription(string description, int makeId)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (!String.IsNullOrEmpty(description))
                {
                    filter.Property(VehicleModel.Properties.Description, typeof(VehicleModel).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }
                if (makeId > 0)
                {
                    if (!String.IsNullOrEmpty(description))
                    {
                        filter.And();
                    }
                    filter.Property(VehicleModel.Properties.VehicleMakeCode, typeof(VehicleModel).Name);
                    filter.Equal();
                    filter.Constant(makeId);
                }


                ClauseVehicleModel view = new ClauseVehicleModel();
                ViewBuilder builder = new ViewBuilder("ClauseVehicleModel");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                
                Result<List<ParamVehicleMake>, ErrorModel> ResultVehicleMake = ModelAssembler.CreateVehicleMakes(view.VehicleMake);
                Result<List<ParamVehicleModel>, ErrorModel> ResultVehicleModel = ModelAssembler.CreateParamVehicleModels(view.VehicleModel);
                List<ParamVehicleModel> Result = new List<ParamVehicleModel>();

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleModel), filter.GetPredicate()));
                Result<List<ParamVehicleModel>, ErrorModel> lstVehicleModel = ModelAssembler.CreateParamVehicleModels(businessCollection);
                if (lstVehicleModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
                {
                    return lstVehicleModel;
                }
                else
                {
                    List<ParamVehicleModel> resultValue = (lstVehicleModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add(Resources.Errors.FailedCreatingModel);
                        
                        return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstVehicleModel;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add(Resources.Errors.ErrorAdmin);
                return new ResultError<List<ParamVehicleModel>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<string, ErrorModel> GenerateFileToVehicleModel(List<ParamVehicleModel> lstVehicleModel, string fileName)
        {
            try
            {
                VehicleMakeDAO vehicleMake = new VehicleMakeDAO();
                List<ParamVehicleMake> listMake = ((vehicleMake.GetVehicleMakes()) as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;

                daosUtilitiesservices.FileDAO commonFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationVehicleModel;
                modelsUtilities.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);
                string url = String.Empty;
                if (file.IsEnabled)
                {
                    file.Name = fileName;
                    List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();
                    foreach (ParamVehicleModel vehicleModel in lstVehicleModel)
                    {
                        ///<summary>
                        ///Se realiza un ordenamiento de columnas utilizando el código OrderBy(x => x.Order) al momento de estar armando la tabla, 
                        ///para corregir los inconvenientes presentados por encabezados de tabla en desorden al generar el archivo Excel. 
                        ///</summary>
                        ///<author>Diego Leon</author>
                        ///<date>17/07/2018</date>
                        ///<purpose>REQ_#076</purpose>
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
                        var marca = listMake.Where(x => x.Id == vehicleModel.VehicleMake.Id).FirstOrDefault();
                        if (marca is null) {
                            fields[0].Value = "";
                            fields[1].Value = "";
                        }
                        else
                        {
                            fields[0].Value = vehicleModel.VehicleMake.Id.ToString();
                            fields[1].Value = marca.Description.ToString();
                        }                    
                        fields[2].Value = vehicleModel.Id.ToString();
                        fields[3].Value = vehicleModel.Description;

                        rows.Add(new modelsUtilities.Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    url = commonFileDAO.GenerateFile(file);
                }
                return new ResultValue<string, ErrorModel>(url);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedCreatingPaymentPlanErrorBD*/"Failed Creating File");
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

    }
} 