using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using System.Diagnostics;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.MassiveServices.EEProvider.Assemblers;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Framework.Queries;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using System.Collections;
using Sistran.Core.Application.MassiveServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Concurrent;
using Sistran.Core.Application.Utilities.Constants;
using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.MassiveServices.EEProvider.Resources;
using Sistran.Core.Framework;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.MassiveServices.EEProvider.DAOs
{
    public class MassiveLoadDAO
    {
        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();

        /// <summary>
        /// Obtener Tipos de Cargue Por Tipo de Proceso
        /// </summary>
        /// <param name="massiveProcessType">Tipo de Proceso</param>
        /// <returns>Tipos de Cargue</returns>
        public List<LoadType> GetLoadTypesByMassiveProcessType(MassiveProcessType massiveProcessType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.LoadType.Properties.ProcessTypeCode, typeof(MSVEN.LoadType).Name);
            filter.Equal();
            filter.Constant(massiveProcessType);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.LoadType), filter.GetPredicate()));
            return ModelAssembler.CreateLoadTypes(businessCollection);
        }

        /// <summary>
        /// Crear Cargue
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad)
        {
            MSVEN.MassiveLoad entityMassiveLoad = EntityAssembler.CreateMassiveLoad(massiveLoad);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityMassiveLoad);

            if (entityMassiveLoad != null)
            {
                massiveLoad.Id = entityMassiveLoad.Id;
                TP.Task.Run(() => CreateMassiveLoadLog(massiveLoad));
                return massiveLoad;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar Cargue
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad UpdateMassiveLoad(MassiveLoad massiveLoad)
        {
            PrimaryKey primaryKey = MSVEN.MassiveLoad.CreatePrimaryKey(massiveLoad.Id);
            MSVEN.MassiveLoad entityMassiveLoad = (MSVEN.MassiveLoad)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityMassiveLoad != null)
            {
                bool recordLog = false;

                if (entityMassiveLoad.StatusId != (int)massiveLoad.Status || massiveLoad.HasError)
                {
                    recordLog = true;
                }

                entityMassiveLoad.LoadTypeId = (int)massiveLoad.LoadType.Id;
                entityMassiveLoad.Description = massiveLoad.Description;
                entityMassiveLoad.FileName = massiveLoad.File.Name;
                entityMassiveLoad.UserId = massiveLoad.User.UserId;
                entityMassiveLoad.StatusId = (int)massiveLoad.Status;
                entityMassiveLoad.HasError = massiveLoad.HasError;
                entityMassiveLoad.ErrorDescription = massiveLoad.ErrorDescription;
                entityMassiveLoad.TotalRows = massiveLoad.TotalRows;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityMassiveLoad);

                if (recordLog)
                {
                    TP.Task.Run(() => CreateMassiveLoadLog(massiveLoad));
                }

                return massiveLoad;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Cargue Por Identificador
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad GetMassiveLoadByMassiveLoadId(int massiveLoadId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name).Equal().Constant(massiveLoadId);

            MassiveLoadView massiveLoadView = new MassiveLoadView();
            ViewBuilder builder = new ViewBuilder("MassiveLoadView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, massiveLoadView);

            if (massiveLoadView.MassiveLoads.Count > 0)
            {
                MassiveLoad massiveLoad = ModelAssembler.CreateMassiveLoad(massiveLoadView.MassiveLoads.Cast<MSVEN.MassiveLoad>().First());
                massiveLoad.LoadType.ProcessType = (MassiveProcessType)massiveLoadView.LoadTypes.Cast<MSVEN.LoadType>().First().ProcessTypeCode;

                return massiveLoad;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Cargues Por Descripción
        /// </summary>
        /// <param name="description">Descripción</param>
        /// <returns>Cargues</returns>
        public List<MassiveLoad> GetMassiveLoadsByDescription(string description)
        {
            List<MassiveLoad> massiveLoads = new List<MassiveLoad>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int32 massiveLoadId = 0;
            Int32.TryParse(description, out massiveLoadId);

            if (massiveLoadId > 0)
            {
                filter.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name).Equal().Constant(massiveLoadId);
            }
            else
            {
                filter.Property(MSVEN.MassiveLoad.Properties.Description, typeof(MSVEN.MassiveLoad).Name).Like().Constant("%" + description + "%");
            }

            MassiveLoadView massiveLoadView = new MassiveLoadView();
            ViewBuilder builder = new ViewBuilder("MassiveLoadView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, massiveLoadView);

            if (massiveLoadView.MassiveLoads.Count > 0)
            {
                massiveLoads = ModelAssembler.CreateMassiveLoads(massiveLoadView.MassiveLoads);

                if (massiveLoads.Count == 1)
                {
                    massiveLoads[0].LoadType.ProcessType = (MassiveProcessType)massiveLoadView.LoadTypes.Cast<MSVEN.LoadType>().First().ProcessTypeCode;

                    NameValue[] parameters = new NameValue[1];
                    parameters[0] = new NameValue("MASSIVE_LOAD_ID", massiveLoadId);

                    DataTable resultTable;

                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        resultTable = dynamicDataAccess.ExecuteSPDataTable("MSV.MASSIVE_LOAD_SUMMARY", parameters);
                    }

                    if (resultTable != null && resultTable.Rows.Count > 0)
                    {
                        massiveLoads[0].TotalRows = (int)resultTable.Rows[0][0];
                        massiveLoads[0].Processeds = (int)resultTable.Rows[0][1];
                        massiveLoads[0].Pendings = (int)resultTable.Rows[0][2];
                        massiveLoads[0].WithEvents = (int)resultTable.Rows[0][3];
                        massiveLoads[0].WithErrors = (int)resultTable.Rows[0][4];

                        massiveLoads[0].Tariffed = (int)resultTable.Rows[0][5];
                        massiveLoads[0].ForRate = (int)resultTable.Rows[0][6];
                        massiveLoads[0].Issued = (int)resultTable.Rows[0][7];
                        massiveLoads[0].ForIssue = (int)resultTable.Rows[0][8];

                    }
                }
            }

            return massiveLoads;
        }

        /// <summary>
        /// Obtener Cargues Por Fecha, Tipo de Proceso, Tipo de Cargue, Descripción, Usuario
        /// </summary>
        /// <param name="rangeFrom">Fecha Desde</param>
        /// <param name="rangeTo">Fecha Hasta</param>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargues</returns>
        public List<MassiveLoad> GetMassiveLoadsByRangeFromRangeToMassiveLoad(DateTime rangeFrom, DateTime rangeTo, MassiveLoad massiveLoad)
        {
            List<MassiveLoad> massiveLoads = new List<MassiveLoad>();
            NameValue[] parameters = new NameValue[6];

            parameters[0] = new NameValue("FROM_DATE", rangeFrom.ToString("MM/dd/yyyy"));
            parameters[1] = new NameValue("TO_DATE", rangeTo.ToString("MM/dd/yyyy"));

            if (massiveLoad != null)
            {
                if (massiveLoad.LoadType != null && massiveLoad.LoadType.ProcessType.HasValue)
                {
                    parameters[2] = new NameValue("PROCESS_TYPE_ID", (int)massiveLoad.LoadType.ProcessType);
                }
                else
                {
                    parameters[2] = new NameValue("PROCESS_TYPE_ID", DBNull.Value, DbType.Int32);
                }

                if (massiveLoad.Status.HasValue)
                {
                    parameters[3] = new NameValue("PROCESS_STATUS_ID", (int)massiveLoad.Status);
                }
                else
                {
                    parameters[3] = new NameValue("PROCESS_STATUS_ID", DBNull.Value, DbType.Int32);
                }

                if (!string.IsNullOrEmpty(massiveLoad.Description))
                {
                    parameters[4] = new NameValue("LOAD_DESCRIPTION", massiveLoad.Description);
                }
                else
                {
                    parameters[4] = new NameValue("LOAD_DESCRIPTION", DBNull.Value, DbType.String);
                }

                if (massiveLoad.User != null && massiveLoad.User.UserId > 0)
                {
                    parameters[5] = new NameValue("USER_ID", massiveLoad.User.UserId);
                }
                else
                {
                    parameters[5] = new NameValue("USER_ID", DBNull.Value, DbType.Int32);
                }
            }
            else
            {
                parameters[2] = new NameValue("PROCESS_TYPE_ID", DBNull.Value, DbType.Int32);
                parameters[3] = new NameValue("PROCESS_STATUS_ID", DBNull.Value, DbType.Int32);
                parameters[4] = new NameValue("LOAD_DESCRIPTION", DBNull.Value, DbType.String);
                parameters[5] = new NameValue("USER_ID", DBNull.Value, DbType.Int32);
            }

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("MSV.GET_MASSIVE_PROCESS", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                var massiveLoadIds = from rows in result.AsEnumerable()
                                     group rows by new
                                     {
                                         Id = rows.Field<int>("NumeroDeCargue")
                                     } into g
                                     select new
                                     {
                                         g.Key.Id
                                     };

                foreach (var item in massiveLoadIds)
                {
                    MassiveLoad newMassiveLoad = new MassiveLoad
                    {
                        Id = item.Id
                    };

                    List<DataRow> dataRows = result.AsEnumerable().Where(x => x.Field<int>("NumeroDeCargue") == newMassiveLoad.Id).ToList();

                    newMassiveLoad.Description = (string)dataRows[0][2];
                    newMassiveLoad.TotalRows = (int)dataRows[0][3];
                    newMassiveLoad.Status = (MassiveLoadStatus)dataRows[0][4];
                    newMassiveLoad.User = new User
                    {
                        UserId = (int)dataRows[0][7],
                        AccountName = (string)dataRows[0].ItemArray[8]
                    };
                    newMassiveLoad.LoadType = new LoadType
                    {
                        ProcessType = (MassiveProcessType)dataRows[0][0]
                    };
                    newMassiveLoad.Logs = new List<MassiveLoadLog>();

                    foreach (DataRow dataRow in dataRows)
                    {
                        if (dataRow[5] != DBNull.Value)
                        {
                            newMassiveLoad.Logs.Add(new MassiveLoadLog
                            {
                                Description = (string)dataRow[5],
                                Time = (DateTime)dataRow[6]
                            });
                        }
                    }
                    massiveLoads.Add(newMassiveLoad);
                }
            }

            return massiveLoads;
        }

        /// <summary>
        /// Crear Log
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        private void CreateMassiveLoadLog(MassiveLoad massiveLoad)
        {
            try
            {
                PrimaryKey primaryKey = MSVEN.MassiveLoadStatus.CreatePrimaryKey((int)massiveLoad.Status);
                MSVEN.MassiveLoadStatus entityMassiveLoadStatus = (MSVEN.MassiveLoadStatus)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                MSVEN.MassiveLoadLog entityMassiveLoadLog = new MSVEN.MassiveLoadLog
                {
                    MassiveLoadId = massiveLoad.Id,
                    Description = entityMassiveLoadStatus.Description,
                    Time = DateTime.Now
                };

                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityMassiveLoadLog);

            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }

        public Printing CreatePrinting(Printing printLog)
        {
            MSVEN.Printing entityPrinting = new MSVEN.Printing()
            {
                PrintingTypeId = printLog.PrintingTypeId,
                KeyId = printLog.KeyId,
                Total = printLog.Total,
                BeginDate = printLog.BeginDate,
                UserId = printLog.UserId,
            };

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityPrinting);

            printLog.Id = entityPrinting.Id;
            return printLog;
        }

        public void CreatePrintLog(PrintingLog PrintingLog)
        {
            MSVEN.PrintingLog entityPrinting = new MSVEN.PrintingLog()
            {
                PrintingId = PrintingLog.Id,
                Description = PrintingLog.Description
            };

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityPrinting);
        }

        public Printing UpdatePrinting(Printing printing)
        {
            PrimaryKey primaryKey = MSVEN.Printing.CreatePrimaryKey(printing.Id);
            MSVEN.Printing entityPrinting = (MSVEN.Printing)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            entityPrinting.UrlFile = printing.UrlFile;
            entityPrinting.FinishDate = printing.FinishDate;
            //entityPrinting.HasError = printing.HasError;
            //entityPrinting.ErrorMessage = printing.ErrorMessage;
            //entityPrinting.IsEnabled = printing.IsEnabled;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityPrinting);
            return printing;
        }

        public Printing GetPrintingByPrintingId(int printingId)
        {
            PrimaryKey primaryKey = MSVEN.Printing.CreatePrimaryKey(printingId);
            MSVEN.Printing entityPrinting = (MSVEN.Printing)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityPrinting != null)
            {
                return ModelAssembler.CreatePrinting(entityPrinting);
            }
            else
            {
                return null;
            }
        }

        public Printing GetPrinting(int massiveLoadId, int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(MSVEN.Printing.Properties.KeyId, massiveLoadId);
            filter.And();
            filter.PropertyEquals(MSVEN.Printing.Properties.UserId, userId);
            //filter.And();
            //filter.PropertyEquals(MSVEN.Printing.Properties.IsEnabled, true);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.Printing), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreatePrinting(businessCollection.Cast<MSVEN.Printing>().First());
            }
            else
            {
                return null;
            }
        }

        public string DeleteProcess(MassiveLoad massiveLoad)
        {
            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("MASSIVE_LOAD_ID", massiveLoad.Id);
            parameters[1] = new NameValue("LOAD_TYPE_ID", massiveLoad.LoadType.Id);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("MSV.DELETE_MASSIVE_PROCESS", parameters);
            }

            if (result.Rows.Count > 0)
            {
                return (string)result.Rows[0][0];
            }
            else
            {
                return "";
            }
        }

        #region Reportes
        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        public string GenerateMassiveProcessReport(List<MassiveLoad> massiveLoads)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ProcessReport;

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null && !massiveLoads.Any())
            {
                return "";
            }


            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";
            List<int> packages = DataFacadeManager.GetPackageProcesses(massiveLoads.Count(), "MaxThreadMassive");
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;

            foreach (int package in packages)
            {
                List<MassiveLoad> packageMassiveLoad = massiveLoads.Take(package).ToList();
                massiveLoads.RemoveRange(0, package);

                TP.Parallel.ForEach(packageMassiveLoad,
                   (process) =>
                   {
                       FillVehicleFields(process, serializeFields);
                   });

                if (concurrentRows.Count >= bulkExcel || massiveLoads.Count == 0)
                {
                    file.Templates[0].Rows = concurrentRows.ToList();
                    file.Name = "Reporte Procesos_" + key;
                    filePath = DelegateService.utilitiesService.GenerateFile(file);
                    concurrentRows = new ConcurrentBag<Row>();
                }
            }
            return filePath;
        }
        private void FillVehicleFields(MassiveLoad proccess, string serializeFields)
        {
            try
            {
                List<Field> fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);

                fields.Find(u => u.PropertyName == FieldPropertyName.ProcessType).Value = proccess.LoadType.ProcessTypeDescription;
                fields.Find(u => u.PropertyName == FieldPropertyName.LoadNumber).Value = proccess.Id.ToString();
                fields.Find(u => u.PropertyName == FieldPropertyName.Load).Value = proccess.Description;
                fields.Find(u => u.PropertyName == FieldPropertyName.RisksNumber).Value = proccess.TotalRows.ToString();
                fields.Find(u => u.PropertyName == FieldPropertyName.UserName).Value = proccess.User.AccountName;

                foreach (MassiveLoadLog item in proccess.Logs)
                {
                    if (item.Description == "VALIDACIÓN ARCHIVO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeStartProcessLoad).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                    if (item.Description == "VALIDADO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeEndProcessLoad).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }

                    if (item.Description == "CONSULTANDO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeStartExternalProcess).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                    if (item.Description == "CONSULTADO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeEndExternalProcess).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                    if (item.Description == "TARIFANDO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeStartRateProcess).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                    if (item.Description == "TARIFADO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeEndRateProcess).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                    if (item.Description == "EMITIENDO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeStartIssuanceProcess).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                    if (item.Description == "EMITIDO")
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.DateTimeEndIssuanceProcess).Value = item.Time.ToShortDateString() + " " + item.Time.ToShortTimeString();
                    }
                }

                concurrentRows.Add(new Row
                {
                    Id = proccess.Id,
                    Fields = fields
                });

            }
            catch (Exception)
            {

            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        #endregion


        #region Eventos

        public List<AuthorizationRequest> GetAuthorizationPolicies(Risk risk, MassiveLoad massiveLoad)
        {
            var authorizationRequests = new List<AuthorizationRequest>();
            if (risk.InfringementPolicies == null)
            {
                risk.InfringementPolicies = new List<PoliciesAut>();
            }
            if (risk.Policy.InfringementPolicies == null)
            {
                risk.Policy.InfringementPolicies = new List<PoliciesAut>();
            }

            var infringementPolicies = risk.InfringementPolicies.Concat(risk.Policy.InfringementPolicies).ToList();

            if (infringementPolicies.Count > 0)
            {
                List<string> errorAuthorizationAnswers = new List<string>();

                foreach (PoliciesAut policiesAut in infringementPolicies.Where(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization))
                {
                    var authorizationRequest  = new AuthorizationRequest
                    {
                        FunctionType = AuthorizationPoliciesServices.Enums.TypeFunction.Massive,
                        Key = massiveLoad.Id.ToString(),
                        Key2 = risk.Policy.Id.ToString(),
                        DateRequest = DateTime.Now,
                        Policies = policiesAut,
                        UserRequest = massiveLoad.User,
                        DescriptionRequest = "Solicitud Automatica Masivos.  Cargue: " + massiveLoad.Id,
                        Description = string.Join("|", policiesAut.ConceptsDescription.Select(x => x.Description + " : " + x.Value)),
                        HierarchyRequest = new CoHierarchyAssociation { Id = policiesAut.IdHierarchyPolicy },
                        Status = AuthorizationPoliciesServices.Enums.TypeStatus.Pending,
                        NotificationUsers = new List<User>(),
                        NumberAut = policiesAut.NumberAut
                    };

                    try
                    {
                        authorizationRequest.AuthorizationAnswers = new List<AuthorizationAnswer>{ DelegateService.AuthorizationPoliciesService.GetUsersAutorizationByIdPoliciesIdHierarchy(policiesAut.IdPolicies, policiesAut.IdHierarchyAut)
                            .Where(a => a.Default)
                            .Select(a => new AuthorizationAnswer
                            {
                                AuthorizationAnswerId = a.User.UserId,
                                HierarchyAnswer = new CoHierarchyAssociation { Id = policiesAut.IdHierarchyAut },
                                Status = AuthorizationPoliciesServices.Enums.TypeStatus.Pending,
                                Required = a.Required,
                                Enabled = true,
                                UserAnswer = a.User,
                                DateAnswer = DateTime.Now
                            }).First()};

                        authorizationRequests.Add(authorizationRequest);
                    }
                    catch (Exception)
                    {
                        errorAuthorizationAnswers.Add(policiesAut.Description);
                    }
                }
                if (errorAuthorizationAnswers.Count > 0)
                {
                    throw new BusinessException(string.Format(Errors.ErrorAuthorizeUser, string.Join(",", errorAuthorizationAnswers.Distinct())));
                }
            }
            return authorizationRequests;
        }

        public List<AuthorizationRequest> ValidateAuthorizationPolicies(List<PoliciesAut> infringementPolicies, MassiveLoad massiveLoad, int temporalId)
        {
            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();

            if (infringementPolicies.Count > 0)
            {
                List<string> errorAuthorizationAnswers = new List<string>();

                foreach (PoliciesAut policiesAut in infringementPolicies.Where(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization))
                {
                    AuthorizationRequest authorizationRequest = new AuthorizationRequest
                    {
                        FunctionType = AuthorizationPoliciesServices.Enums.TypeFunction.Massive,
                        Key = massiveLoad.Id.ToString(),
                        Key2 = temporalId.ToString(),
                        DateRequest = DateTime.Now,
                        Policies = policiesAut,
                        UserRequest = massiveLoad.User,
                        DescriptionRequest = "Solicitud Automatica Masivos.  Cargue: " + massiveLoad.Id,
                        Description = string.Join("|", policiesAut.ConceptsDescription.Select(x => x.Description + " : " + x.Value)),
                        HierarchyRequest = new CoHierarchyAssociation { Id = policiesAut.IdHierarchyPolicy },
                        Status = AuthorizationPoliciesServices.Enums.TypeStatus.Pending,
                        NotificationUsers = new List<User>(),
                        NumberAut = policiesAut.NumberAut
                    };

                    try
                    {
                        authorizationRequest.AuthorizationAnswers = new List<AuthorizationAnswer>{ DelegateService.AuthorizationPoliciesService.GetUsersAutorizationByIdPoliciesIdHierarchy(policiesAut.IdPolicies, policiesAut.IdHierarchyAut)
                            .Where(a => a.Default)
                            .Select(a => new AuthorizationAnswer
                            {
                                AuthorizationAnswerId = a.User.UserId,
                                HierarchyAnswer = new CoHierarchyAssociation { Id = policiesAut.IdHierarchyAut },
                                Status = AuthorizationPoliciesServices.Enums.TypeStatus.Pending,
                                Required = a.Required,
                                Enabled = true,
                                UserAnswer = a.User,
                                DateAnswer = DateTime.Now
                            }).First()};

                        authorizationRequests.Add(authorizationRequest);
                    }
                    catch (Exception)
                    {
                        errorAuthorizationAnswers.Add(policiesAut.Description);
                    }
                }
                if (errorAuthorizationAnswers.Count > 0)
                {
                    throw new BusinessException(string.Format(Errors.ErrorAuthorizeUser, string.Join(",", errorAuthorizationAnswers.Distinct())));
                }
            }

            return authorizationRequests;
        }

        /// <summary>
        /// Realiza el envio de las solicitudes de autorizacion de los eventos
        /// </summary>
        /// <param name="authorizationRequest"></param>
        /// <returns></returns>
        private Task SendAuthorizationPolicies(List<AuthorizationRequest> authorizationRequest)
        {
            return TP.Task.Run(() =>
            {
                try
                {
                    DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequest);
                }
                catch (Exception)
                {
                    //
                }
            });
        }

        public void UpdateMassiveLoadAuthorization(string massiveLoadId, IEnumerable<string> temporalId)
        {
            int massiveLoadID;
            int temporalID;
            if (int.TryParse(massiveLoadId, out massiveLoadID))
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name).Equal().Constant(massiveLoadID);

                MassiveLoadView massiveLoadView = new MassiveLoadView();
                ViewBuilder builder = new ViewBuilder("MassiveLoadView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, massiveLoadView);

                if (massiveLoadView.MassiveLoads.Any())
                {
                    var massiveLoadEntity = massiveLoadView.MassiveLoads.Cast<MSVEN.MassiveLoad>().First();
                    UpdateQuery update = new UpdateQuery();
                    switch (massiveLoadEntity.LoadTypeId)
                    {
                        case (int)SubMassiveProcessType.MassiveEmissionWithoutRequest:
                        case (int)SubMassiveProcessType.MassiveEmissionWithRequest:
                            if (temporalId.Count() > 0)
                            {
                                update.Table = new ClassNameTable(typeof(MSVEN.MassiveEmissionRow));
                                update.ColumnValues.Add(new Column(MSVEN.MassiveEmissionRow.Properties.HasEvents), new Constant(false, System.Data.DbType.Boolean));
                                
                                filter.Clear();
                                filter = new ObjectCriteriaBuilder();
                                filter.Property(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId);
                                filter.Equal();
                                filter.Constant(int.Parse(massiveLoadId));
                                filter.And();
                                filter.Property(MSVEN.MassiveEmissionRow.Properties.TempId);
                                filter.In().ListValue();
                                temporalId.ToList().ForEach(x => filter.Constant(int.Parse(x)));
                                filter.EndList();
                                update.Where = filter.GetPredicate();
                                
                                DataFacadeManager.Instance.GetDataFacade().Execute(update);
                            }             
                           break;

                        case (int)SubMassiveProcessType.MassiveRenewal:
                            if (temporalId.Count() > 0)
                            {
                                update.Table = new ClassNameTable(typeof(MSVEN.MassiveRenewalRow));
                                update.ColumnValues.Add(new Column(MSVEN.MassiveRenewalRow.Properties.HasEvents), new Constant(false, System.Data.DbType.Boolean));

                                filter.Clear();
                                filter = new ObjectCriteriaBuilder();
                                filter.Property(MSVEN.MassiveRenewalRow.Properties.MassiveLoadId);
                                filter.Equal();
                                filter.Constant(int.Parse(massiveLoadId));
                                filter.And();
                                filter.Property(MSVEN.MassiveRenewalRow.Properties.TempId);
                                filter.In().ListValue();
                                temporalId.ToList().ForEach(x => filter.Constant(int.Parse(x)));
                                filter.EndList();
                                update.Where = filter.GetPredicate();

                                DataFacadeManager.Instance.GetDataFacade().Execute(update);
                            }
                            break;

                        case (int)SubMassiveProcessType.CancellationMassive:
                            if (temporalId.Count() > 0)
                            {
                                update.Table = new ClassNameTable(typeof(MSVEN.MassiveCancellationRow));
                                update.ColumnValues.Add(new Column(MSVEN.MassiveCancellationRow.Properties.HasEvents), new Constant(false, System.Data.DbType.Boolean));

                                filter.Clear();
                                filter = new ObjectCriteriaBuilder();
                                filter.Property(MSVEN.MassiveCancellationRow.Properties.MassiveLoadId);
                                filter.Equal();
                                filter.Constant(int.Parse(massiveLoadId));
                                filter.And();
                                filter.Property(MSVEN.MassiveCancellationRow.Properties.TempId);
                                filter.In().ListValue();
                                temporalId.ToList().ForEach(x => filter.Constant(int.Parse(x)));
                                filter.EndList();
                                update.Where = filter.GetPredicate();

                                DataFacadeManager.Instance.GetDataFacade().Execute(update);
                            }
                            break;
                        case (int)SubMassiveProcessType.CollectiveEmission:
                        case (int)SubMassiveProcessType.CollectiveRenewal:
                        case (int)SubMassiveProcessType.Inclusion:
                        case (int)SubMassiveProcessType.Exclusion:
                            foreach (string singleTemporalId in temporalId)
                            {
                                if (singleTemporalId.Contains('|'))
                                {
                                    String[] temporal = singleTemporalId.Split('|');
                                    MSVEN.CollectiveEmissionRow collectiveEmissionRow = GetCollectiveEmissionRowEntityByMassiveLoadIdTempId(massiveLoadEntity.Id, Convert.ToInt32(temporal[1]));
                                    if (collectiveEmissionRow != null)
                                    {
                                        collectiveEmissionRow.HasEvents = false;
                                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(collectiveEmissionRow);
                                    }
                                }
                                else if (int.TryParse(singleTemporalId, out temporalID))
                                {
                                    MSVEN.CollectiveEmission collectiveEmission = this.GetCollectiveEmissionEntityByMassiveLoadIdTempId(massiveLoadEntity.Id, temporalID);
                                    if (collectiveEmission != null)
                                    {
                                        collectiveEmission.HasEvents = false;
                                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(collectiveEmission);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Obtener Cargue Por Identificador
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        public MSVEN.MassiveEmissionRow GetMassiveEmissionRowEntityByMassiveLoadIdTempId(int massiveLoadId, int tempId)
        {
             

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.HasError, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.Commiss, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.DocumentNumber, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.HasEvents, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.Id, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.Observations, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.PolicyType, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.PrefixId, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.Premium, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.RiskDescription, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.RowNumber, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.SerializedRow, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.StatusId, "ms")));
            select.AddSelectValue(new SelectValue(new Column(MSVEN.MassiveEmissionRow.Properties.TempId, "ms")));

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId, "ms").Equal().Constant(massiveLoadId);
            where.And();
            where.Property(MSVEN.MassiveEmissionRow.Properties.TempId, "ms").Equal().Constant(tempId);

            MSVEN.MassiveEmissionRow massiveEmissionRow = new MSVEN.MassiveEmissionRow();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    massiveEmissionRow.HasError = Convert.ToBoolean(reader["HasError"].ToString());
                    massiveEmissionRow.Commiss = Convert.ToDecimal(reader["Commiss"].ToString());
                    massiveEmissionRow.DocumentNumber = Convert.ToDecimal(reader["DocumentNumber"].ToString());
                    massiveEmissionRow.HasEvents = Convert.ToBoolean(reader["HasEvents"].ToString());
                    massiveEmissionRow.Id = Convert.ToInt32(reader["Id"].ToString());
                    massiveEmissionRow.MassiveLoadId = Convert.ToInt32(reader["MassiveLoadId"].ToString());
                    massiveEmissionRow.Observations = Convert.ToString(reader["Observations"].ToString());
                    massiveEmissionRow.PolicyType = Convert.ToString(reader["PolicyType"].ToString());
                    massiveEmissionRow.Premium = Convert.ToDecimal(reader["Premium"].ToString());
                    massiveEmissionRow.RiskDescription = Convert.ToString(reader["RiskDescription"].ToString());
                    massiveEmissionRow.RowNumber = Convert.ToInt32(reader["RowNumber"].ToString());
                    massiveEmissionRow.SerializedRow = Convert.ToString(reader["SerializedRow"].ToString());
                    massiveEmissionRow.StatusId = Convert.ToInt32(reader["StatusId"].ToString());
                    massiveEmissionRow.TempId = Convert.ToInt32(reader["TempId"].ToString());

                }
            }

            return massiveEmissionRow;


        }

        public MSVEN.MassiveRenewalRow GetMassiveRenewalRowEntityByMassiveLoadIdTempId(int massiveLoadId, int tempId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveRenewalRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveRenewalRow).Name).Equal().Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.MassiveRenewalRow.Properties.TempId, typeof(MSVEN.MassiveRenewalRow).Name).Equal().Constant(tempId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveRenewalRow), filter.GetPredicate()));

            if (businessCollection.Any())
            {
                return businessCollection.Cast<MSVEN.MassiveRenewalRow>().First();
            }
            else
            {
                return null;
            }
        }

        public MSVEN.MassiveCancellationRow GetMassiveCancellationRowEntityByMassiveLoadIdTempId(int massiveLoadId, int tempId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveCancellationRow).Name).Equal().Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.TempId, typeof(MSVEN.MassiveCancellationRow).Name).Equal().Constant(tempId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveCancellationRow), filter.GetPredicate()));

            if (businessCollection.Any())
            {
                return businessCollection.Cast<MSVEN.MassiveCancellationRow>().First();
            }
            else
            {
                return null;
            }
        }

        public MSVEN.CollectiveEmissionRow GetCollectiveEmissionRowEntityByMassiveLoadIdTempId(int massiveLoadId, int tempId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name).Equal().Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.RiskId, typeof(MSVEN.CollectiveEmissionRow).Name).Equal().Constant(tempId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));

            if (businessCollection.Any())
            {
                return businessCollection.Cast<MSVEN.CollectiveEmissionRow>().First();
            }
            else
            {
                return null;
            }
        }

        public MSVEN.CollectiveEmission GetCollectiveEmissionEntityByMassiveLoadIdTempId(int massiveLoadId, int tempId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.CollectiveEmission.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name).Equal().Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.CollectiveEmission.Properties.TempId, typeof(MSVEN.CollectiveEmissionRow).Name).Equal().Constant(tempId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmission), filter.GetPredicate()));

            if (businessCollection.Any())
            {
                return businessCollection.Cast<MSVEN.CollectiveEmission>().First();
            }
            else
            {
                return null;
            }
        }

        #endregion Eventos
    }
}