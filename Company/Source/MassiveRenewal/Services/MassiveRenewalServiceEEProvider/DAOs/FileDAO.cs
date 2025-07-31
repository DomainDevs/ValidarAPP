using Newtonsoft.Json;
using Sistran.Company.Application.MassiveRenewalServices.EEProvider.Resources;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using coreDao = Sistran.Core.Application.MassiveRenewalServices.EEProvider.DAOs;
using Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Entities.Views;
using System.Collections.Concurrent;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class FileDAO
    {
        public string GenerateFileToPayrollByAgent(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            switch (massiveRenewalRows[0].Risk.Policy.Summary.CoveredRiskType.Value)
            {
                case CoveredRiskType.Aeronavigation:
                    return "";
                case CoveredRiskType.Location:
                    return GenerateFileToPayrollByAgentLocation(massiveRenewalRows, fileName);
                case CoveredRiskType.Surety:
                    return "";
                case CoveredRiskType.Transport:
                    return "";
                case CoveredRiskType.Vehicle:
                    return GenerateFileToPayrollByAgentVehicle(massiveRenewalRows, fileName);
                default:
                    return "";
            }
        }

        private string GenerateFileToPayrollByAgentVehicle(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            try
            {
                coreDao.MassiveRenewalDAO massiveRenewalDAO = new coreDao.MassiveRenewalDAO();
                File file = massiveRenewalDAO.GenerateFileToPayrollByAgentVehicle(massiveRenewalRows, fileName);

                //Para agregar más columnas en company
                //foreach (Row row in file.Templates[0].Rows)
                //{
                //    List<COUPMO.IndividualSarlaft> sarlaftHolder = DelegateService.uniquePersonService.GetIndividualSarlaftByIndividualId(int.Parse(row.Fields.Find(x => x.Description == "No POLIZA").Value));
                //Field dataField = new Field();
                //dataField = NewField(file.Templates[0].Rows[0].Fields[13]);
                //dataField.Value = renewalProcess.Policy.Summary.Premium.ToString();
                //row.Fields.Add(dataField);
                //}

                return DelegateService.utilitiesService.GenerateFile(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateFileToPayrollByAgentLocation(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            coreDao.MassiveRenewalDAO massiveRenewalDAO = new coreDao.MassiveRenewalDAO();
            return massiveRenewalDAO.GenerateFileToPayrollByAgentLocation(massiveRenewalRows, fileName);
        }

        public string GenerateFileErrorsMassiveRenewals(int massiveLoadId)
        {
            MassiveRenewal massiveRenewal = new MassiveRenewal();
            massiveRenewal = DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoadId, true, null,null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();

            switch (subCoveredRiskType)
            {

                case SubCoveredRiskType.Vehicle:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;

                    break;
                case SubCoveredRiskType.ThirdPartyLiability:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.ThirdPartyLiability;

                    break;
                case SubCoveredRiskType.Property:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Property;

                    break;
                case SubCoveredRiskType.Liability:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Liability;

                    break;

                default:
                    break;
            }

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Errores",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Eventos",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });

            massiveRenewal.Rows = massiveRenewal.Rows.OrderBy(x => x.RowNumber).ToList();

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            CompanyMassiveRenewalGetAuthorizations view = new CompanyMassiveRenewalGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(massiveLoadId.ToString());
            where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Distinct().Constant(Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Authorized);
            builder.Filter = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.AsParallel().Cast<APEntity.AuthorizationRequest>().ToList();
            List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.AsParallel().Cast<APEntity.AuthorizationAnswer>().ToList();
            List<APEntity.Policies> totalPolicies = view.Policies.AsParallel().Cast<APEntity.Policies>().ToList();
            List<Core.Application.UniqueUser.Entities.UniqueUsers> totalUsers = view.Users.AsParallel().Cast<Core.Application.UniqueUser.Entities.UniqueUsers>().ToList();

            foreach (MassiveRenewalRow proccess in massiveRenewal.Rows)
            {
                File fileSerialized = JsonConvert.DeserializeObject<File>(proccess.SerializedRow);
                ConcurrentBag<string> eventMessaje = new ConcurrentBag<string>();
                foreach (Template t in fileSerialized.Templates)
                {
                    FormatRows(t.Rows);
                    file.Templates.Find(x => x.PropertyName == t.PropertyName).Rows.AddRange(t.Rows);
                }

                file.Templates[0].Rows.Last().Fields.Add(new Field
                {
                    ColumnSpan = 1,
                    FieldType = FieldType.String,
                    Value = proccess.Observations,
                    Description = "Errores",
                    IsEnabled = true,
                    IsMandatory = false,
                    Id = 0,
                    Order = file.Templates[0].Rows.Last().Fields.Count(),
                    RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                });

                if (proccess.HasEvents && proccess.SerializedRow != null)
                {
                    List<APEntity.AuthorizationRequest> requests = totalRequests.AsParallel().Where(x => x.Key2 == proccess.Risk.Policy.Id.ToString()).ToList();

                    ParallelHelper.ForEach(requests, request =>
                    {
                        APEntity.Policies policie = totalPolicies.AsParallel().First(x => x.PoliciesId == request.PoliciesId);
                        APEntity.AuthorizationAnswer answer = totalAnswers.AsParallel().First(x => x.AuthorizationRequestId == request.AuthorizationRequestId);
                        Core.Application.UniqueUser.Entities.UniqueUsers user = totalUsers.AsParallel().First(x => x.UserId == answer.UserAnswerId);

                        if (request.StatusId == (int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Rejected)
                        {
                            eventMessaje.Add(Errors.Rejected + ": " + policie.Message + " (" + Errors.AuthorizingUser + user.AccountName + ")");
                        }
                        else if (request.StatusId == (int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Pending)
                        {
                            eventMessaje.Add(Errors.Authorization + ": " + policie.Message + " (" + Errors.AuthorizingUser + user.AccountName + ")");
                        }
                    });
                }

                file.Templates[0].Rows.Last().Fields.Add(new Field
                {
                    ColumnSpan = 1,
                    FieldType = FieldType.String,
                    Description = "Eventos",
                    Value = string.Join(" |", eventMessaje),
                    IsEnabled = true,
                    IsMandatory = false,
                    Id = 0,
                    Order = file.Templates[0].Rows.Last().Fields.Count(),
                    RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                });
            }

            file.Name = "Errores_" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesService.GenerateFile(file);
        }

        private List<PoliciesAut> GetInfringementPolicies(MassiveRenewal massiveRenewal, MassiveRenewalRow proccess, int tempId)
        {
            Policy policy = new Policy();
            List<Risk> risks = new List<Risk>();
            List<PoliciesAut> result = new List<PoliciesAut>();

            var pendingOperationPolicy = new PendingOperation();

            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperationPolicy = DelegateService.utilitiesService.GetPendingOperationById(tempId);
                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperationPolicy = DelegateService.utilitiesService.GetPendingOperationById(tempId);
                /* with Replicated Database */
            }

            switch (massiveRenewal.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    var pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(tempId);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(tempId);
                        /* with Replicated Database */
                    }

                    policy = JsonConvert.DeserializeObject<Policy>(pendingOperationPolicy.Operation);
                    foreach (PendingOperation item in pendingOperations)
                    {
                        risks.Add(JsonConvert.DeserializeObject<Risk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => risks.Add(JsonConvert.DeserializeObject<Risk>(x)));
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => risks.Add(JsonConvert.DeserializeObject<Risk>(x)));
                        /* with Replicated Database */
                    }
                    break;
            }

            if (policy.InfringementPolicies != null)
            {
                result.AddRange(policy.InfringementPolicies);
            }
            risks.Where(r => r.InfringementPolicies != null).ToList().ForEach(r => result.AddRange(r.InfringementPolicies));
            return result;
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
    }
}
