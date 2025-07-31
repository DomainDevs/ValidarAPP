using Sistran.Core.Application.CollectiveServices.Models;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Core.Application.CollectiveServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using TypePolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.CollectiveServices.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Co.Application.Data;
using System.Data;
using Sistran.Core.Framework;
using Sistran.Core.Application.CollectiveServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Constants;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.DAOs
{
    using Entities.Views;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Utilities.Helper;

    public class CollectiveEmissionDAO
    {
        /// <summary>
        /// Crear Cargue de colectivas
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.CreateMassiveLoad(collectiveEmission);

            if (massiveLoad != null)
            {
                collectiveEmission.Id = massiveLoad.Id;
                MSVEN.CollectiveEmission entityCollectiveEmission = EntityAssembler.CreateCollectiveEmission(collectiveEmission);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCollectiveEmission);

                if (entityCollectiveEmission != null)
                {
                    return collectiveEmission;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar Proceso De Cargue colectivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public CollectiveEmission UpdateCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);

            PrimaryKey primaryKey = MSVEN.CollectiveEmission.CreatePrimaryKey(collectiveEmission.Id);
            MSVEN.CollectiveEmission entityCollectiveEmission = (MSVEN.CollectiveEmission)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityCollectiveEmission != null)
            {
                entityCollectiveEmission.AgencyId = collectiveEmission.Agency.Id;
                entityCollectiveEmission.AgentId = collectiveEmission.Agency.Agent.IndividualId;
                entityCollectiveEmission.BranchId = collectiveEmission.Branch.Id;
                entityCollectiveEmission.PrefixId = collectiveEmission.Prefix.Id;
                entityCollectiveEmission.TempId = collectiveEmission.TemporalId;
                entityCollectiveEmission.ProductId = collectiveEmission.Product.Id;
                entityCollectiveEmission.Commiss = collectiveEmission.Commiss;
                entityCollectiveEmission.DocumentNumber = collectiveEmission.DocumentNumber;
                entityCollectiveEmission.Premium = collectiveEmission.Premium;
                entityCollectiveEmission.HasEvents = collectiveEmission.HasEvents;
                entityCollectiveEmission.EndorsementNumber = collectiveEmission.EndorsementNumber;
                entityCollectiveEmission.EndorsementId = collectiveEmission.EndorsementId;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCollectiveEmission);

                return collectiveEmission;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Obtener Cargue por Id
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns>Cargue Colectivas</returns>
        public CollectiveEmission GetCollectiveEmissionByMassiveLoadId(int massiveLoadId)
        {
            CollectiveEmissionView view = new CollectiveEmissionView();
            ViewBuilder builder = new ViewBuilder("CollectiveEmissionView");

            ObjectCriteriaBuilder filterView = new ObjectCriteriaBuilder();
            filterView.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name);
            filterView.Equal();
            filterView.Constant(massiveLoadId);
            builder.Filter = filterView.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MSVEN.MassiveLoad massiveLoadView = view.MassiveLoad.Cast<MSVEN.MassiveLoad>().FirstOrDefault();
            MSVEN.CollectiveEmission collectiveEmissionView = view.CollectiveEmission.Cast<MSVEN.CollectiveEmission>().FirstOrDefault();

            CollectiveEmission massiveEmission = ModelAssembler.CreateCollectiveEmission(collectiveEmissionView);

            return massiveEmission;
        }

        ///// <summary>
        ///// Actualizar Proceso De Cargue Masivo colectivas
        ///// </summary>
        ///// <param name="massiveLoadProcess">Proceso De Cargue Masivo colectivas</param>
        ///// <returns>Proceso De Cargue Masivo</returns>
        //public List<CollectiveEmissionRow> GetCollectiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadProcessId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name);
        //    filter.Equal();
        //    filter.Constant(massiveLoadProcessId);

        //    if (massiveLoadProcessStatus.HasValue)
        //    {
        //        filter.And();
        //        filter.Property(MSVEN.CollectiveEmissionRow.Properties.StatusId, typeof(MSVEN.CollectiveEmissionRow).Name);
        //        filter.Equal();
        //        filter.Constant(massiveLoadProcessStatus.Value);
        //    }
        //    if (withError.HasValue)
        //    {
        //        filter.And();
        //        filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasError, typeof(MSVEN.CollectiveEmissionRow).Name);
        //        filter.Equal();
        //        filter.Constant(withError.Value);
        //    }
        //    if (withEvent.HasValue)
        //    {
        //        filter.And();
        //        filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasEvents, typeof(MSVEN.CollectiveEmissionRow).Name);
        //        filter.Equal();
        //        filter.Constant(withEvent.Value);
        //    }
        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));
        //    List<CollectiveEmissionRow> collectiveEmissionRows = ModelAssembler.CreateCollectiveEmissionRows(businessCollection);

        //    return collectiveEmissionRows;
        //}

        /// <summary>
        /// Actualizar Proceso De Cargue coletivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public CollectiveEmission GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(int massiveLoadId, bool withRows, bool? withErrors, bool? withEvents)
        {
            CollectiveEmissionView view = new CollectiveEmissionView();
            ViewBuilder builder = new ViewBuilder("CollectiveEmissionView");

            ObjectCriteriaBuilder filterView = new ObjectCriteriaBuilder();
            filterView.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name);
            filterView.Equal();
            filterView.Constant(massiveLoadId);
            builder.Filter = filterView.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MSVEN.MassiveLoad massiveLoadView = view.MassiveLoad.Cast<MSVEN.MassiveLoad>().FirstOrDefault();
            MSVEN.CollectiveEmission collectiveEmissionView = view.CollectiveEmission.Cast<MSVEN.CollectiveEmission>().FirstOrDefault();

            if (collectiveEmissionView == null)
            {
                return null;
            }
            CollectiveEmission collectiveEmission = ModelAssembler.CreateCollectiveEmission(collectiveEmissionView, massiveLoadView);

            if (withRows)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(massiveLoadId);

                if (withErrors.HasValue && withErrors.Value && withEvents.HasValue && withEvents.Value)
                {
                    filter.And();
                    filter.OpenParenthesis();
                    filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasError, typeof(MSVEN.CollectiveEmissionRow).Name);
                    filter.Equal();
                    filter.Constant(withErrors.Value);
                    filter.Or();
                    filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasEvents, typeof(MSVEN.CollectiveEmissionRow).Name);
                    filter.Equal();
                    filter.Constant(withEvents.Value);
                    filter.CloseParenthesis();
                }
                else
                {

                    if (withErrors.HasValue)
                    {
                        filter.And();
                        filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasError, typeof(MSVEN.CollectiveEmissionRow).Name);
                        filter.Equal();
                        filter.Constant(withErrors.Value);
                    }
                    if (withEvents.HasValue)
                    {
                        filter.And();
                        filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasEvents, typeof(MSVEN.CollectiveEmissionRow).Name);
                        filter.Equal();
                        filter.Constant(withEvents.Value);
                    }
                }
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));
                List<CollectiveEmissionRow> collectiveLoadProcesses = ModelAssembler.CreateCollectiveEmissionRow(businessCollection);
                collectiveEmission.Rows = collectiveLoadProcesses;
            }
            return collectiveEmission;
        }

        public List<CollectiveEmission> GetCollectiveEmissionsByTempId(int tempId, bool withRows, bool withEvents)
        {

            List<CollectiveEmission> collectiveEmissions = new List<CollectiveEmission>();
            CollectiveEmissionView view = new CollectiveEmissionView();
            ViewBuilder builder = new ViewBuilder("CollectiveEmissionView");

            ObjectCriteriaBuilder filterView = new ObjectCriteriaBuilder();
            filterView.Property(MSVEN.CollectiveEmission.Properties.TempId, typeof(MSVEN.CollectiveEmission).Name);
            filterView.Equal();
            filterView.Constant(tempId);
            builder.Filter = filterView.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<MSVEN.MassiveLoad> massiveLoads = view.MassiveLoad.Cast<MSVEN.MassiveLoad>().ToList();
            foreach (MSVEN.MassiveLoad massiveLoad in massiveLoads)
            {
                MSVEN.CollectiveEmission collectiveEmissionView = view.CollectiveEmission.Cast<MSVEN.CollectiveEmission>().FirstOrDefault(x => x.MassiveLoadId == massiveLoad.Id);

                CollectiveEmission collectiveEmission = ModelAssembler.CreateCollectiveEmission(collectiveEmissionView, massiveLoad);

                if (withRows)
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name);
                    filter.Equal();
                    filter.Constant(massiveLoad.Id);
                    if (withEvents)
                    {
                        filter.And();
                        filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasEvents, typeof(MSVEN.CollectiveEmissionRow).Name);
                        filter.Equal();
                        filter.Constant(withEvents);
                    }
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));
                    List<CollectiveEmissionRow> collectiveLoadProcesses = ModelAssembler.CreateCollectiveEmissionRow(businessCollection);
                    collectiveEmission.Rows = collectiveLoadProcesses;
                }

                collectiveEmissions.Add(collectiveEmission);
            }
            return collectiveEmissions;
        }

        /// <summary>
        /// Genera el archivo de error del proceso de emisión colectiva
        /// </summary>
        /// <param name="massiveLoadProccessId"></param>
        /// <returns></returns>
        public string GenerateFileErrorsCollective(int massiveLoadId, FileProcessType fileProcessType)
        {
            CollectiveEmission collectiveEmission = new CollectiveEmission();
            collectiveEmission = this.GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(massiveLoadId, true, null, null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonServiceCore.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            FileProcessValue fileProcessValue = new FileProcessValue();

            fileProcessValue.Key1 = (int)fileProcessType;
            if (fileProcessType == FileProcessType.CollectiveExclusion || fileProcessType == FileProcessType.CollectiveInclusion)
            {
                fileProcessValue.Key2 = (int)EndorsementType.Modification;
            }
            fileProcessValue.Key4 = collectiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            int headersCount = file.Templates.First(x => x.IsPrincipal).Rows.Count;

            for (int i = 0; i < headersCount - 1; i++)
            {
                file.Templates.First(x => x.IsPrincipal).Rows[i].Fields.Add(new Field
                {
                    Order = file.Templates.First(x => x.IsPrincipal).Rows[i].Fields.Max(y => y.Order) + 1,
                    ColumnSpan = 2,
                    RowPosition = file.Templates.First(x => x.IsPrincipal).Rows[i].Fields.First().RowPosition,
                    Description = ""
                });
            }

            Template emissionTemplate = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmisionAutos || x.PropertyName == TemplatePropertyName.EmissionLiability || x.PropertyName == TemplatePropertyName.EmissionProperty
                || x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability || x.PropertyName == TemplatePropertyName.Renewal || x.PropertyName == TemplatePropertyName.Policy);
            emissionTemplate.Rows.Last().Fields.Add(new Field
            {
                Order = emissionTemplate.Rows.Last().Fields.Max(y => y.Order) + 1,
                ColumnSpan = 1,
                RowPosition = emissionTemplate.Rows.Last().Fields.First().RowPosition,
                FieldType = FieldType.String,
                Description = "Eventos"
            });

            if (fileProcessType == FileProcessType.CollectiveRenewal)
            {
                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.Last().Fields.Add(new Field
                {
                    Order = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.Max(y => y.Order) + 1,
                    ColumnSpan = 1,
                    RowPosition = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.First().RowPosition,
                    FieldType = FieldType.String,
                    Description = "Errores"
                });

                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.Last().Fields.Add(new Field
                {
                    Order = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.Max(y => y.Order) + 1,
                    ColumnSpan = 1,
                    RowPosition = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.First().RowPosition,
                    FieldType = FieldType.String,
                    Description = "Eventos"
                });
            }
            else
            {
                file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.Add(new Field
                {
                    Order = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.Max(y => y.Order) + 1,
                    ColumnSpan = 1,
                    RowPosition = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.First().RowPosition,
                    FieldType = FieldType.String,
                    Description = "Errores"
                });

                file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.Add(new Field
                {
                    Order = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.Max(y => y.Order) + 1,
                    ColumnSpan = 1,
                    RowPosition = file.Templates.First(x => x.IsPrincipal).Rows.Last().Fields.First().RowPosition,
                    FieldType = FieldType.String,
                    Description = "Eventos"
                });
            }

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            CollectiveGetAuthorizations view = new CollectiveGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(massiveLoadId.ToString());
            where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Distinct().Constant((int)AuthorizationPoliciesServices.Enums.TypeStatus.Authorized).ToString();
            builder.Filter = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.AsParallel().Cast<APEntity.AuthorizationRequest>().ToList();
            List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.AsParallel().Cast<APEntity.AuthorizationAnswer>().ToList();
            List<APEntity.Policies> totalPolicies = view.Policies.AsParallel().Cast<APEntity.Policies>().ToList();
            List<UniqueUser.Entities.UniqueUsers> totalUsers = view.Users.AsParallel().Cast<UniqueUser.Entities.UniqueUsers>().ToList();

            if (collectiveEmission.Rows.Any())
            {
                foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmission.Rows.OrderBy(r => r.RowNumber))
                {
                    File fileSerialized = JsonConvert.DeserializeObject<File>(collectiveEmissionRow.SerializedRow);

                    foreach (Template template in fileSerialized.Templates)
                    {
                        if (template.IsPrincipal)
                        {
                            ConcurrentBag<string> eventMessaje = new ConcurrentBag<string>();
                            template.Rows.Last().Fields.Add(new Field
                            {
                                Order = template.Rows.Last().Fields.Max(y => y.Order) + 1,
                                ColumnSpan = 1,
                                RowPosition = template.Rows.Last().Fields.First().RowPosition,
                                FieldType = FieldType.String,
                                Value = collectiveEmissionRow.Observations
                            });

                            if (collectiveEmissionRow.HasEvents)
                            {
                                List<APEntity.AuthorizationRequest> requests = totalRequests.AsParallel().Where(x => x.Key2.Contains("|" + collectiveEmissionRow.Risk.Id.ToString())).ToList();

                                ParallelHelper.ForEach(requests, request =>
                                {
                                    APEntity.Policies policie = totalPolicies.AsParallel().First(x => x.PoliciesId == request.PoliciesId);
                                    APEntity.AuthorizationAnswer answer = totalAnswers.AsParallel().First(x => x.AuthorizationRequestId == request.AuthorizationRequestId);
                                    UniqueUser.Entities.UniqueUsers user = totalUsers.AsParallel().First(x => x.UserId == answer.UserAnswerId);

                                    if (request.StatusId == (int)AuthorizationPoliciesServices.Enums.TypeStatus.Rejected)
                                    {
                                        eventMessaje.Add(Errors.Rejected + ": " + policie.Message + " (" + Errors.AuthorizingUser + " " + user.AccountName + ")");
                                    }
                                    else if (request.StatusId == (int)AuthorizationPoliciesServices.Enums.TypeStatus.Pending)
                                    {
                                        eventMessaje.Add(Errors.Authorization + ": " + policie.Message + " (" + Errors.AuthorizingUser + " " + user.AccountName + ")");
                                    }
                                });
                            }

                            template.Rows.Last().Fields.Add(new Field
                            {
                                Order = template.Rows.Last().Fields.Max(y => y.Order) + 1,
                                ColumnSpan = 1,
                                RowPosition = template.Rows.Last().Fields.First().RowPosition,
                                FieldType = FieldType.String,
                                Value = string.Join(" |", eventMessaje)
                            });
                        }

                        if (file.Templates.First(x => x.Order == 1).Rows.Count == headersCount || template.Order != 1)
                        {
                            file.Templates.First(x => x.PropertyName == template.PropertyName).Rows.AddRange(template.Rows);
                        }
                    }
                }
            }

            foreach (Template template in file.Templates)
            {
                if (template.PropertyName == emissionTemplate.PropertyName)
                {
                    ConcurrentBag<string> eventMessaje = new ConcurrentBag<string>();
                    if (collectiveEmission.HasEvents)
                    {
                        List<APEntity.AuthorizationRequest> requests = totalRequests.AsParallel().Where(x => !x.Key2.Contains("|")).ToList();

                        ParallelHelper.ForEach(requests, request =>
                        {
                            APEntity.Policies policie = totalPolicies.AsParallel().First(x => x.PoliciesId == request.PoliciesId);
                            APEntity.AuthorizationAnswer answer = totalAnswers.AsParallel().First(x => x.AuthorizationRequestId == request.AuthorizationRequestId);
                            UniqueUser.Entities.UniqueUsers user = totalUsers.AsParallel().First(x => x.UserId == answer.UserAnswerId);

                            if (request.StatusId == (int)AuthorizationPoliciesServices.Enums.TypeStatus.Rejected)
                            {
                                eventMessaje.Add(Errors.Rejected + ": " + policie.Message + " (" + Errors.AuthorizingUser + " " + user.AccountName + ")");
                            }
                            else if (request.StatusId == (int)AuthorizationPoliciesServices.Enums.TypeStatus.Pending)
                            {
                                eventMessaje.Add(Errors.Authorization + ": " + policie.Message + " (" + Errors.AuthorizingUser + " " + user.AccountName + ")");
                            }
                        });
                    }

                    template.Rows.Last().Fields.Add(new Field
                    {
                        Order = template.Rows.Last().Fields.Max(y => y.Order) + 1,
                        ColumnSpan = 1,
                        RowPosition = template.Rows.Last().Fields.First().RowPosition,
                        FieldType = FieldType.String,
                        Value = string.Join(" |", eventMessaje)
                    });
                }

                this.FormatRows(template.Rows);
            }

            file.Name = "Errores_" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesService.GenerateFile(file);
        }

        private void FormatRows(List<Row> rows)
        {
            TP.Parallel.ForEach(rows.AsParallel(),
            (row) =>
            {
                foreach (Field field in row.Fields.Where(u => u.FieldType == FieldType.Boolean))
                {
                    switch (field.Value)
                    {
                        case "True":
                            field.Value = "SI";
                            break;
                        case "False":
                            field.Value = "NO";
                            break;
                    }
                }
            });
        }

        private List<Risk> GetCompanyRisk(CollectiveEmission collectiveEmission, CollectiveEmissionRow proccess, int tempId)
        {
            List<Risk> risks = new List<Risk>();
            switch (collectiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(tempId);
                    foreach (PendingOperation item in pendingOperations)
                    {
                        risks.Add(JsonConvert.DeserializeObject<Risk>(item.Operation));
                    }
                    break;
                case MassiveLoadStatus.Issued:
                    risks = this.GetRisksByPrefixBranchDocumentNumberEndorsementType(collectiveEmission.Prefix.Id, collectiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Emission);
                    break;
            }
            return risks;
        }

        private List<Risk> GetRisksByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType)
        {
            List<Risk> risks = new List<Risk>();
            NameValue[] parameters = new NameValue[5];
            parameters[0] = new NameValue("@PREFIX_ID", prefixId);
            parameters[1] = new NameValue("@BRANCH_ID", branchId);
            parameters[2] = new NameValue("@DOCUMENT_NUM", documentNumber);
            parameters[3] = new NameValue("@ENDORSEMENT_TYPE_ID", endorsementType);
            parameters[4] = new NameValue("@ONLY_POLICY", 0);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REPORT.REPORT_GET_OPERATION", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow arrayItem in result.Rows)
                {
                    risks.Add(JsonConvert.DeserializeObject<Risk>(arrayItem[0].ToString()));
                }
            }
            return risks;
        }

        /// <summary>
        /// Actualiza los temporales a excluir
        /// </summary>
        /// <param name="massiveLoadId">Id del cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Usuario Actual</param>
        /// <param name="deleteTemporal">Si debe eliminar el temporal</param>
        public CollectiveEmission ExcludeCollectiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            CollectiveEmissionView view = new CollectiveEmissionView();
            ViewBuilder builder = new ViewBuilder("CollectiveEmissionView");

            ObjectCriteriaBuilder filterView = new ObjectCriteriaBuilder();
            filterView.Property(MSVEN.MassiveLoad.Properties.Id, typeof(MSVEN.MassiveLoad).Name);
            filterView.Equal();
            filterView.Constant(massiveLoadId);
            builder.Filter = filterView.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            MSVEN.MassiveLoad massiveLoadView = view.MassiveLoad.Cast<MSVEN.MassiveLoad>().FirstOrDefault();
            MSVEN.CollectiveEmission collectiveEmissionView = view.CollectiveEmission.Cast<MSVEN.CollectiveEmission>().FirstOrDefault();

            CollectiveEmission collectiveEmission = ModelAssembler.CreateCollectiveEmission(collectiveEmissionView, massiveLoadView);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasError, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.RowNumber, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.In().ListValue();
            foreach (int temporal in temps)
            {
                filter.Constant(temporal);
            }
            filter.EndList();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                List<CollectiveEmissionRow> collectiveEmissionRows = ModelAssembler.CreateCollectiveEmissionRows(businessCollection);
                collectiveEmissionRows.RemoveAll(u => u.HasEvents);
                if (collectiveEmissionRows.Count > 0)
                {
                    CollectiveEmissionRowDAO collectiveEmissionRowDAO = new CollectiveEmissionRowDAO();

                    foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.MessageExcludedByUser + userName;
                        collectiveEmissionRowDAO.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        DelegateService.underwritingServiceCore.DeleteTemporalsByOperationId(collectiveEmissionRow.Risk.Id);
                    }

                    
                }
                else
                {
                    return null;
                }
            }

            return collectiveEmission;
        }

        #region Eventos
        public List<AuthorizationRequest> ValidateAuthorizationPolicies(List<PoliciesAut> policiesAuthorization, MassiveLoad massiveLoad, int policyId)
        {
            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();

            if (policiesAuthorization != null)
            {
                List<string> errorAuthorizationAnswers = new List<string>();

                foreach (PoliciesAut policiesAut in policiesAuthorization.Where(x => x.Type == TypePolicies.Authorization))
                {
                    AuthorizationRequest authorizationRequest = new AuthorizationRequest
                    {
                        FunctionType = AuthorizationPoliciesServices.Enums.TypeFunction.Collective,
                        Key = massiveLoad.Id.ToString(),
                        Key2 = policyId.ToString(),
                        DateRequest = DateTime.Now,
                        Policies = policiesAut,
                        UserRequest = massiveLoad.User,
                        DescriptionRequest = "Solicitud Automatica Colectivas.  Cargue: " + massiveLoad.Id,
                        Description = string.Join("|", policiesAut.ConceptsDescription.Select(x => x.Description + " : " + x.Value)),
                        HierarchyRequest = new CoHierarchyAssociation { Id = policiesAut.IdHierarchyPolicy },
                        Status = AuthorizationPoliciesServices.Enums.TypeStatus.Pending,
                        NotificationUsers = new List<User>(),
                        NumberAut = policiesAut.NumberAut
                    };
                    try
                    {

                        authorizationRequest.AuthorizationAnswers = new List<AuthorizationAnswer>{DelegateService.AuthorizationPoliciesService.GetUsersAutorizationByIdPoliciesIdHierarchy(policiesAut.IdPolicies, policiesAut.IdHierarchyAut).Where(a => a.Default)
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
                    throw new BusinessException(string.Format(Errors.ErrorAuthorizeUser , string.Join(",", errorAuthorizationAnswers.Distinct())));
                }
            }
            return authorizationRequests;
        }

        public List<AuthorizationRequest> ValidateAuthorizationPoliciesRisk(List<PoliciesAut> policiesAuthorization, MassiveLoad massiveLoad, int policyId, int riskId)
        {
            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();

            if (policiesAuthorization != null)
            {
                List<string> errorAuthorizationAnswers = new List<string>();

                foreach (PoliciesAut policiesAut in policiesAuthorization.Where(x => x.Type == TypePolicies.Authorization))
                {
                    AuthorizationRequest authorizationRequest = new AuthorizationRequest
                    {
                        FunctionType = AuthorizationPoliciesServices.Enums.TypeFunction.Collective,
                        Key = massiveLoad.Id.ToString(),
                        Key2 = policyId + "|" + riskId,
                        DateRequest = DateTime.Now,
                        Policies = policiesAut,
                        UserRequest = massiveLoad.User,
                        DescriptionRequest = "Solicitud Automatica Colectivas.  Cargue: " + massiveLoad.Id,
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
        #endregion

        public void UpdateCollectiveLoadAuthorization(int loadId, int temporalId, List<int> risks)
        {
            UpdateQuery update = new UpdateQuery();

            if (loadId > 0 && temporalId > 0 && risks.Count()==0)
            {
                update.Table = new ClassNameTable(typeof(MSVEN.CollectiveEmission));
                update.ColumnValues.Add(new Column(MSVEN.CollectiveEmission.Properties.HasEvents), new Constant(false, System.Data.DbType.Boolean));
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(MSVEN.CollectiveEmission.Properties.MassiveLoadId);
                filter.Equal();
                filter.Constant(loadId);
                filter.And();
                filter.Property(MSVEN.CollectiveEmission.Properties.TempId);
                filter.Equal();
                filter.Constant(temporalId);
                update.Where = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().Execute(update);
            }

            if (risks.Count() > 0)
            {
                update = new UpdateQuery();
                update.Table = new ClassNameTable(typeof(MSVEN.CollectiveEmissionRow));
                update.ColumnValues.Add(new Column(MSVEN.CollectiveEmissionRow.Properties.HasEvents), new Constant(false, System.Data.DbType.Boolean));
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId);
                filter.Equal();
                filter.Constant(loadId);
                filter.And();
                filter.Property(MSVEN.CollectiveEmissionRow.Properties.RiskId);
                filter.In().ListValue();
                risks.ToList().ForEach(x => filter.Constant(x));
                filter.EndList();

                update.Where = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().Execute(update);
            }
        }
    }
}