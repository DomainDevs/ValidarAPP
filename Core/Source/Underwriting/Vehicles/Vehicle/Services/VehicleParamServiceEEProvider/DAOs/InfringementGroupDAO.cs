// -----------------------------------------------------------------------
// <copyright file="InfringementGroupDAO.cs" company="SISTRAN">
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
    using Sistran.Core.Framework.Queries;    
    using daosUtilitiesservices = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using enumsUtilitiesServices = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;    
    using System.Linq;

    /// <summary>
    /// Acceso a base de datos para grupos de infracciones
    /// </summary>
    public class InfringementGroupDAO
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
                Result<List<InfringementGroup>, ErrorModel> lstInfrigementGroup = ModelAssembler.CreateInfringementGroup(businessCollection);
                if (lstInfrigementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
                {
                    return lstInfrigementGroup;
                }
                else
                {
                    List<InfringementGroup> resultValue = (lstInfrigementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuontra el grupo de infracción.");
                        return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfrigementGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de grupos de infracciones. Comuniquese con el administrador");
                return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<List<InfringementGroup>, ErrorModel> GetInfringementGroupActive()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoCptGroupInfringement)));
                Result<List<InfringementGroup>, ErrorModel> lstInfrigementGroup = ModelAssembler.CreateInfringementGroupActive(businessCollection);
                if (lstInfrigementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
                {
                    return lstInfrigementGroup;
                }
                else
                {
                    List<InfringementGroup> resultValue = (lstInfrigementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuontra el grupo de infracción.");
                        return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfrigementGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de grupos de infracciones. Comuniquese con el administrador");
                return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<List<InfringementGroup>, ErrorModel> GetInfringementGroupsByDescription(string description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CoInfringement.Properties.Description, typeof(CoInfringement).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoCptGroupInfringement), filter.GetPredicate()));
                Result<List<InfringementGroup>, ErrorModel> lstInfrigementGroup = ModelAssembler.CreateInfringementGroup(businessCollection);
                if (lstInfrigementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
                {
                    return lstInfrigementGroup;
                }
                else
                {
                    List<InfringementGroup> resultValue = (lstInfrigementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra el grupo de infracción.");
                        return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstInfrigementGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de grupos de infracciones. Comuniquese con el administrador");
                return new ResultError<List<InfringementGroup>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<InfringementGroup, ErrorModel> CreateInfringementGroup(InfringementGroup infrigementGroup)
        {
            try
            {
                CoCptGroupInfringement infrigementEntity = EntityAssembler.CreateInfringementGroup(infrigementGroup);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(infrigementEntity);
                ResultValue<InfringementGroup, ErrorModel> infringementGroupResult = ModelAssembler.CreateInfringementGroup(infrigementEntity);
                return infringementGroupResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedCreatingPaymentPlanErrorBD*/"Failed Creating InfringementGroup");
                return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<InfringementGroup, ErrorModel> UpdateInfringementGroup(InfringementGroup parametrizationPaymentPlan)
        {
            try
            {
                PrimaryKey key = CoCptGroupInfringement.CreatePrimaryKey(parametrizationPaymentPlan.InfringementGroupCode);
                CoCptGroupInfringement infringementGroupEntity = (CoCptGroupInfringement)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                infringementGroupEntity.Description = parametrizationPaymentPlan.Description;
                infringementGroupEntity.InfringementOneYear = parametrizationPaymentPlan.InfringementOneYear;
                infringementGroupEntity.InfringementThreeYears = parametrizationPaymentPlan.InfringementThreeYear;
                infringementGroupEntity.SnActive = parametrizationPaymentPlan.IsActive;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(infringementGroupEntity);
                ResultValue<InfringementGroup, ErrorModel> infringementGroupResult = ModelAssembler.CreateInfringementGroup(infringementGroupEntity);
                return infringementGroupResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(/*Resources.Errors.FailedUpdatingPaymentPlanErrorBD*/"Failed Updating InfringementGroup");
                return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<string, ErrorModel> GenerateFileToInfringementGroup(List<InfringementGroup> lstInfringementGroup, string fileName)
        {
            try
            {
                daosUtilitiesservices.FileDAO commonFileDAO = new daosUtilitiesservices.FileDAO();
                modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
                fileProcessValue.Key1 = (int)enumsUtilitiesServices.FileProcessType.ParametrizationInfringementGroup;
                modelsUtilities.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);
                string url = String.Empty;
                if (file.IsEnabled)
                {
                    file.Name = fileName;
                    List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();
                    foreach (InfringementGroup insuredProfiles in lstInfringementGroup)
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
                        fields[0].Value = insuredProfiles.InfringementGroupCode.ToString();
                        fields[1].Value = insuredProfiles.Description.ToString();
                        fields[2].Value = insuredProfiles.IsActive ? "HABILITADO" : "INHABILITADO";
                        fields[3].Value = insuredProfiles.InfringementOneYear.ToString();
                        fields[4].Value = insuredProfiles.InfringementThreeYear.ToString();
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

        //public Result<InfringementGroup, ErrorModel> DeleteInfringementGroup(InfringementGroup parametrizationPaymentPlan, bool isPrincipal)
        //{
        //    List<string> listErrors = new List<string>();
        //    try
        //    {
        //        DataTable result;
        //        NameValue[] parameters = new NameValue[2];
        //        parameters[0] = new NameValue("@PAYMENT_SCHEDULE_ID", parametrizationPaymentPlan.Id);
        //        parameters[1] = new NameValue("@PRINCIPAL", isPrincipal);
        //        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
        //        {
        //            result = dynamicDataAccess.ExecuteSPDataTable("PROD.DELETE_PAYMENT_SCHEDULE_PARAMETRIZATION", parameters);
        //        }

        //        if (((int)result.Rows[0][0]) == 1)
        //        {
        //            parametrizationPaymentPlan.ParametrizationQuotas = new List<ParametrizationQuota>();
        //            return new ResultValue<InfringementGroup, ErrorModel>(parametrizationPaymentPlan);
        //        }

        //        if (((int)result.Rows[0][0]) == -1)
        //        {
        //            listErrors.Add(Resources.Errors.NoDeletePaymentPlanUse);
        //            return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, null));
        //        }
        //        listErrors.Add(Resources.Errors.FailedDeletingPaymentPlanErrorBD);
        //        return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, null));
        //    }
        //    catch (System.Exception ex)
        //    {
        //        listErrors.Add(Resources.Errors.FailedDeletingPaymentPlanErrorBD);
        //        return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
        //    }
        //}
    }
}
