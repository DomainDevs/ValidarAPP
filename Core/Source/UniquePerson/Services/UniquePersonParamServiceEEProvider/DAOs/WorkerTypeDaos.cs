using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonParamServices.EEProvider.DAOs
{
    using Sistran.Core.Application.UniquePersonParamService.Models;
    using Utilities.Error;
    using Framework.DAF;
    using Utilities.DataFacade;    
    using Sistran.Core.Application.UniquePersonParamService.EEProviders.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.UniquePersonParamServices.EEProvider.Assemblers;
    using Sistran.Core.Framework.Queries;    
    using daosUtilitiesServicesProvider = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using entitiesCommon = Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;

    public class WorkerTypeDaos
    {
        /// <summary>
        /// Obtiene la lista de Tipo de Trabajador
        /// </summary>
        /// <returns>Lista de Tipo de Trabajador consultadas</returns>
        /// 
        public Result<List<ParamWorkerType>, ErrorModel> GetWorkerType()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entitiesCommon.WorkerType)));
                Result<List<ParamWorkerType>, ErrorModel> lstWorkerType = ModelAssembler.CreateParamWorkerType(businessCollection);
                if (lstWorkerType is ResultError<List<ParamWorkerType>, ErrorModel>)
                {
                    return lstWorkerType;
                }
                else
                {
                    List<ParamWorkerType> resultValue = (lstWorkerType as ResultValue<List<ParamWorkerType>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModel.Add(Resources.Errors.NoTypeOfWorkerWasFound);
                        return new ResultError<List<ParamWorkerType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstWorkerType;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add(Resources.Errors.ErrorQueryTheWorker);
                return new ResultError<List<ParamWorkerType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }
        public Result<ParamWorkerType, ErrorModel> CreateParamWorkerType(ParamWorkerType workerType)
        {
            try
            {
                int id = (GetWorkerType() as ResultValue<List<ParamWorkerType>, ErrorModel>).Value.Max(p => p.Id) + 1;
                ParamWorkerType item = ParamWorkerType.GetParamWorkerType(id, workerType.Description,workerType.SmallDescription,workerType.IsEnabled).Value;
                entitiesCommon.WorkerType workerTypeEntity = EntityAssembler.CreateParamWorkerType(item);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(workerTypeEntity);
                ResultValue<ParamWorkerType, ErrorModel> WorkerTypeResult = ModelAssembler.CreateParamWorkerType(workerTypeEntity);
                return WorkerTypeResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorSaveWorkerTypeAdded);
                return new ResultError<ParamWorkerType, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
        public Result<List<ParamWorkerType>, ErrorModel> GetWorkerTypeByDescription(string  description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(entitiesCommon.WorkerType.Properties.Description, typeof(entitiesCommon.WorkerType).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entitiesCommon.WorkerType), filter.GetPredicate()));
                Result<List<ParamWorkerType>, ErrorModel> lstWorkerType = ModelAssembler.CreateParamWorkerType(businessCollection);
                if (lstWorkerType is ResultError<List<ParamWorkerType>, ErrorModel>)
                {
                    return lstWorkerType;
                }
                else
                {
                    List<ParamWorkerType> resultValue = (lstWorkerType as ResultValue<List<ParamWorkerType>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModel.Add(Resources.Errors.NoTypeOfWorkerWasFound);
                        return new ResultError<List<ParamWorkerType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstWorkerType;
                    }
                }

            }
            catch (Exception ex)
            {
                errorModel.Add(Resources.Errors.ErrorQueryTheWorker);
                return new ResultError<List<ParamWorkerType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }
        public Result<ParamWorkerType, ErrorModel> UpdateWorkerType(ParamWorkerType workerType)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(entitiesCommon.WorkerType.Properties.WorkerTypeId, typeof(entitiesCommon.WorkerType).Name);
                filter.Equal();
                filter.Constant(workerType.Id);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entitiesCommon.WorkerType), filter.GetPredicate()));
                entitiesCommon.WorkerType workerTypeEntity = new entitiesCommon.WorkerType(workerType.Id);
                foreach (entitiesCommon.WorkerType item in businessCollection)
                {
                    item.Description = workerType.Description;
                    item.SmallDescription = workerType.SmallDescription;
                    item.IsEnabled = workerType.IsEnabled;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(item);
                }
                ResultValue<ParamWorkerType, ErrorModel> workerTypeGroupResult = ModelAssembler.CreateParamWorkerType(workerTypeEntity);
                return workerTypeGroupResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorSaveWorkerTypeModify);
                return new ResultError<ParamWorkerType, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }

        }

        public Result<string, ErrorModel> GenerateFileToWorkerType(List<ParamWorkerType> lstWorkerType, string fileName)
        {
            try
            {
                daosUtilitiesServicesProvider.FileDAO fileDAO = new daosUtilitiesServicesProvider.FileDAO();
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationWorkerType
                };
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                string url = String.Empty;
                if (file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();
                    foreach (ParamWorkerType insuredProfiles in lstWorkerType)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
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
                        fields[0].Value = insuredProfiles.Id.ToString();
                        fields[1].Value = insuredProfiles.SmallDescription;
                        fields[2].Value = insuredProfiles.Description;
                        fields[3].Value = insuredProfiles.IsEnabled ? "SI" : "NO";
                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    url = fileDAO.GenerateFile(file);
                }
                return new ResultValue<string, ErrorModel>(url);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Failed Creating File");
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
    }
}