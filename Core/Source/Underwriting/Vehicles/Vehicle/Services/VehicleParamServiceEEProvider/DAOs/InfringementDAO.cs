// -----------------------------------------------------------------------
// <copyright file="InfringementDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
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
    using daosUtilitiesservices = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using enumsUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;    
    using System.Linq;

    public class InfringementDAO
    {
        /// <summary>
        /// Obtiene la lista de Infracciones.
        /// </summary>
        /// <returns>Lista de infracciones consultadas</returns>
        public Result<List<Infringement>, ErrorModel> GetInfringement()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoInfringement)));
                Result<List<Infringement>, ErrorModel> lstInfrigement = ModelAssembler.CreateInfringement(businessCollection);
                if (lstInfrigement is ResultError<List<Infringement>, ErrorModel>)
                {
                    return lstInfrigement;
                }
                else
                {
                    List<Infringement> resultValue = (lstInfrigement as ResultValue<List<Infringement>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra la infracción.");
                        return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfrigement;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de infracciones. Comuniquese con el administrador");
                return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene la lista de Infracciones que se encuentren activos.
        /// </summary>
        /// <returns>Lista de infracciones consultadas</returns>
        public Result<List<Infringement>, ErrorModel> GetInfringementIsActive()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoInfringement)));
                Result<List<Infringement>, ErrorModel> lstInfrigement = ModelAssembler.CreateInfringement(businessCollection);
                if (lstInfrigement is ResultError<List<Infringement>, ErrorModel>)
                {
                    return lstInfrigement;
                }
                else
                {
                    List<Infringement> resultValue = (lstInfrigement as ResultValue<List<Infringement>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra la infracción.");
                        return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfrigement;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de infracciones. Comuniquese con el administrador");
                return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<Infringement, ErrorModel> CreateInfringement(Infringement infrigement)
        {
            try
            {
                CoInfringement infrigementEntity = EntityAssembler.CreateInfringement(infrigement);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(infrigementEntity);
                ResultValue<Infringement, ErrorModel> infringementResult = ModelAssembler.CreateInfringement(infrigementEntity);
                return infringementResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedCreatingPaymentPlanErrorBD*/"Failed Creating Infringement");
                return new ResultError<Infringement, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<Infringement, ErrorModel> UpdateInfringement(Infringement infrigement)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CoInfringement.Properties.InfringementNewCode, typeof(CoInfringement).Name);
                filter.Equal();
                filter.Constant(infrigement.InfringementCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoInfringement), filter.GetPredicate()));
                CoInfringement infringementEntity = new CoInfringement();
                foreach (CoInfringement item in businessCollection)
                {
                    item.Description = infrigement.Description;
                    item.InfringementPreviousCode = infrigement.InfrigementPreviousCode;
                    item.GroupInfringementCode = infrigement.InfrigemenGroupCode;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(item);
                }
                ResultValue<Infringement, ErrorModel> infringementGroupResult = ModelAssembler.CreateInfringement(infringementEntity);
                return infringementGroupResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedUpdatingPaymentPlanErrorBD*/"Failed Updating Infringement");
                return new ResultError<Infringement, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<List<Infringement>, ErrorModel> GetInfringementByDescription(string description, string code, int? group)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (description != String.Empty)
                {
                    filter.Property(CoInfringement.Properties.Description, typeof(CoInfringement).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                    filter.Or();
                    filter.Property(CoInfringement.Properties.InfringementNewCode, typeof(CoInfringement).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");

                }
                if (code != String.Empty)
                {
                    if (description != String.Empty)
                        filter.And();
                    filter.Property(CoInfringement.Properties.InfringementNewCode, typeof(CoInfringement).Name);
                    filter.Equal();
                    filter.Constant(code);
                }
                if (group != null)
                {
                    if (description != String.Empty || code != String.Empty)
                        filter.And();
                    filter.Property(CoInfringement.Properties.GroupInfringementCode, typeof(CoInfringement).Name);
                    filter.Equal();
                    filter.Constant(group);
                }
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoInfringement), filter.GetPredicate()));
                Result<List<Infringement>, ErrorModel> lstInfrigement = ModelAssembler.CreateInfringement(businessCollection);
                if (lstInfrigement is ResultError<List<Infringement>, ErrorModel>)
                {
                    return lstInfrigement;
                }
                else
                {
                    List<Infringement> resultValue = (lstInfrigement as ResultValue<List<Infringement>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra la infracción.");
                        return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfrigement;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de infracciones. Comuniquese con el administrador");
                return new ResultError<List<Infringement>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<string, ErrorModel> GenerateFileToInfringement(List<Infringement> lstInfringement, string fileName)
        {
            try
            {
                daosUtilitiesservices.FileDAO commonFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationInfringement;
                modelsUtilities.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);
                string url = String.Empty;
                if (file.IsEnabled)
                {
                    file.Name = fileName;
                    List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();
                    foreach (Infringement insuredProfiles in lstInfringement)
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
                        fields[0].Value = insuredProfiles.InfringementCode.ToString();
                        fields[1].Value = insuredProfiles.InfrigementPreviousCode.ToString();
                        fields[2].Value = insuredProfiles.Description;
                        fields[3].Value = insuredProfiles.InfrigemenGroupDescription.ToString() + "(" + insuredProfiles.InfrigemenGroupCode.ToString() + ")";
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
