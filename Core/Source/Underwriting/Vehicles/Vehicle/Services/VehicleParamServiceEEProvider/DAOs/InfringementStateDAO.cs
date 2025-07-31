// -----------------------------------------------------------------------
// <copyright file="InfringementStateStateDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.DAOs
{
    using Assemblers;
    using Common.Entities;
    using Framework.DAF;
    using Models;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utilities.DataFacade;
    using Utilities.Error;
    using daosUtilitiesservices = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using enumsUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;

    public class InfringementStateDAO
    {
        public Result<List<InfringementState>, ErrorModel> GetInfringementState()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoInfringementState)));
                Result<List<InfringementState>, ErrorModel> lstInfringementState = ModelAssembler.CreateInfringementState(businessCollection);
                if (lstInfringementState is ResultError<List<InfringementState>, ErrorModel>)
                {
                    return lstInfringementState;
                }
                else
                {
                    List<InfringementState> resultValue = (lstInfringementState as ResultValue<List<InfringementState>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra la infracción.");
                        return new ResultError<List<InfringementState>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfringementState;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de infracciones. Comuniquese con el administrador");
                return new ResultError<List<InfringementState>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<List<InfringementState>, ErrorModel> GetInfringementStateByDescription(string description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CoInfringement.Properties.Description, typeof(CoInfringement).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoInfringementState), filter.GetPredicate()));
                Result<List<InfringementState>, ErrorModel> lstInfringementState = ModelAssembler.CreateInfringementState(businessCollection);
                if (lstInfringementState is ResultError<List<InfringementState>, ErrorModel>)
                {
                    return lstInfringementState;
                }
                else
                {
                    List<InfringementState> resultValue = (lstInfringementState as ResultValue<List<InfringementState>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra la infracción.");
                        return new ResultError<List<InfringementState>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfringementState;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de infracciones. Comuniquese con el administrador");
                return new ResultError<List<InfringementState>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<InfringementState, ErrorModel> CreateInfringementState(InfringementState infrigement)
        {
            try
            {
                int id = (GetInfringementState() as ResultValue<List<InfringementState>, ErrorModel>).Value.Max(p => p.InfringementStateCode) + 1;
                InfringementState item = InfringementState.GetInfringementState(id, infrigement.Description).Value;
                CoInfringementState infrigementEntity = EntityAssembler.CreateInfringementState(item);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(infrigementEntity);
                ResultValue<InfringementState, ErrorModel> infringementResult = ModelAssembler.CreateInfringementState(infrigementEntity);
                return infringementResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedCreatingPaymentPlanErrorBD*/"Failed Creating InfringementState");
                return new ResultError<InfringementState, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<InfringementState, ErrorModel> UpdateInfringementState(InfringementState infringement)
        {
            try
            {
                PrimaryKey key = CoInfringementState.CreatePrimaryKey(infringement.InfringementStateCode);
                CoInfringementState infringementStateEntity = (CoInfringementState)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                infringementStateEntity.Description = infringement.Description;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(infringementStateEntity);
                ResultValue<InfringementState, ErrorModel> infringementStateResult = ModelAssembler.CreateInfringementState(infringementStateEntity);
                return infringementStateResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedUpdatingPaymentPlanErrorBD*/"Failed Updating InfringementState");
                return new ResultError<InfringementState, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<string, ErrorModel> GenerateFileToInfringementState(List<InfringementState> lstInfringementState, string fileName)
        {
            try
            {
                daosUtilitiesservices.FileDAO commonFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationInfringementState;
                modelsUtilities.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);
                string url = String.Empty;
                if (file.IsEnabled)
                {
                    file.Name = fileName;
                    List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();
                    foreach (InfringementState insuredProfiles in lstInfringementState)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.Select(x => new modelsUtilities.Field
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
                        fields[0].Value = insuredProfiles.InfringementStateCode.ToString();
                        fields[1].Value = insuredProfiles.Description;
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
