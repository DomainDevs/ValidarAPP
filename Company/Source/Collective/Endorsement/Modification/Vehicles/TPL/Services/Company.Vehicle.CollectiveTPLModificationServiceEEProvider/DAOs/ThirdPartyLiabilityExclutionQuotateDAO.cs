using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.Resources;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.DAOs
{
    public class ThirdPartyLiabilityExclutionQuotateDAO
    {
        public void QuotateCollectiveExclusion(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            collectiveEmission.User.UserId = massiveLoad.User.UserId;

            if (collectiveEmission != null)
            {
                try
                {
                    TP.Task.Run(() => QuotateCollectiveExclusionRows(collectiveEmission));
                }
                catch (Exception ex)
                {
                    collectiveEmission.ErrorDescription = ex.Message;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
        }

        private void QuotateCollectiveExclusionRows(CollectiveEmission collectiveEmission)
        {
            try
            {
                List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();
                List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Validation);
                if (collectiveEmissionRows.Count > 0)
                {
                    collectiveEmission.Status = MassiveLoadStatus.Tariffing;
                    collectiveEmission.TotalRows = collectiveEmissionRows.Count;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    else  // with Replicated Database
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    if (pendingOperation == null)
                    {
                        throw new ValidationException(Errors.ErrorTemporalNotFound);
                    }
                    CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.IsPersisted = true;
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.UserId = collectiveEmission.User.UserId;
                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);


                    this.QuotateCollectiveExclusionRows(collectiveEmission, collectiveEmissionRows, companyPolicy, collectiveEmission.IsAutomatic, collectiveEmission.TemporalId);
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);
                }
            }
            catch (Exception ex)
            {
                collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = string.Format(Errors.PoliciesRestrictive, $"</br>{ex.Message}");
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
        }

        private void QuotateCollectiveExclusionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, bool isAutomatic, int temporalId)
        {
            List<CompanyRisk> risks = new List<CompanyRisk>();

            string key = companyPolicy.Prefix.Id + "," + (int)companyPolicy.Product.CoveredRisk.CoveredRiskType;

            int hierarchy = DelegateService.AuthorizationPoliciesService.GetHierarchyByIdUser(10, collectiveEmission.User.UserId);
            List<int> ruleToValidateRisk = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_RISK);
            List<int> ruleToValidateCoverage = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_COVERAGE);

            foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
            {
                try
                {
                    PendingOperation pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }
                    else  // with Replicated Database
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }

                    if (pendingOperation != null)
                    {
                        CompanyTplRisk companyTplRisk = JsonConvert.DeserializeObject<CompanyTplRisk>(pendingOperation.Operation);
                        if (companyTplRisk != null)
                        {
                            //Fecha del riesgo excluido debe ser el menor en la vigencia desde de la póliza
                            DateTime currentFrom = companyTplRisk.Risk.Coverages.FirstOrDefault().CurrentFrom;
                            companyPolicy.CurrentFrom = currentFrom;
                            if (currentFrom < companyPolicy.CurrentFrom)
                            {
                                companyPolicy.CurrentFrom = currentFrom;
                            }
                            companyTplRisk.Risk.Id = pendingOperation.Id;
                            companyTplRisk.Risk.IsPersisted = true;
                            companyTplRisk.Risk.Policy = companyPolicy;
                            companyTplRisk.Risk.Status = RiskStatusType.Excluded;
                            if (companyTplRisk?.Risk?.Coverages != null)
                            {
                                if (currentFrom != null)
                                {
                                    if (currentFrom < companyPolicy.CurrentFrom || currentFrom > companyPolicy.CurrentTo)
                                    {
                                        throw new Exception(Errors.ErrorCurrentFrom);
                                    }
                                }
                                companyTplRisk?.Risk?.Coverages.AsParallel().ForAll(x =>
                                {
                                    x.EndorsementType = companyPolicy?.Endorsement?.EndorsementType;
                                    x.CurrentTo = companyPolicy.CurrentTo;
                                    x.CoverStatus = CoverageStatusType.Excluded;
                                });
                            }
                            else
                            {
                                throw new ValidationException(Errors.ErrorCoverageNotFound);
                            }
                            CompanyTplRisk companyVehicleQuotated = DelegateService.tplService.QuotateThirdPartyLiability(companyTplRisk, true, true);

                            companyPolicy.Holder.CompanyName = companyTplRisk.Risk.Policy.Holder.CompanyName;
                            CompanyRisk risk = companyVehicleQuotated.Risk;
                            risk.Coverages = companyVehicleQuotated.Risk.Coverages;
                            risks.Add(risk);
                            companyTplRisk.Risk.Policy = new CompanyPolicy()
                            {
                                Id = companyPolicy.Id,
                                Endorsement = companyPolicy.Endorsement
                            };
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyVehicleQuotated);
                            pendingOperation.UserId = companyPolicy.UserId;
                            if (!Settings.UseReplicatedDatabase())
                            {
                                DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                            }
                            else
                            {
                                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}",
                                    COMUT.JsonHelper.SerializeObjectToJson(pendingOperation),
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmissionRow),
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmission.Id),
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmission.User),
                                    (char)007, hierarchy,
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(ruleToValidateRisk),
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(ruleToValidateCoverage),
                                    (char)007, nameof(CompanyTplRisk),
                                    (char)007, nameof(CollectiveEmissionRow));
                                QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                            }
                        }
                        else
                        {
                            collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                    }
                }
                catch (Exception ex)
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = ex.Message;
                    string[] messages = ex.GetBaseException().Message.Split('|');
                    collectiveEmissionRow.Observations += string.Format(Errors.ErrorQuotate, messages[0]);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
        }
    }
}