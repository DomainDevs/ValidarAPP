using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Collections.Concurrent;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicle.ModificationService.EEProvider.DAOs
{
    using Core.Application.AuthorizationPoliciesServices.Enums;

    public class VehicleModificationIssueDAO
    {
        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                colectiveEmission.User.UserId = collectiveLoad.User.UserId;
                List<CollectiveEmissionRow> colectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                if (colectiveEmissionRows.Count(x => x.HasEvents == true) > 0)
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.PolicyHasEvents;
                }
                else
                {
                    TP.Task.Run(() => IssuanceCollectiveEmissionRows(collectiveLoad, colectiveEmission, colectiveEmissionRows));

                }
            }
            return collectiveLoad;
        }

        public void IssuanceCollectiveEmissionRows(MassiveLoad collectiveLoad, CollectiveEmission colectiveEmission, List<CollectiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                colectiveEmission.Status = MassiveLoadStatus.Issuing;
                colectiveEmission.TotalRows = massiveEmissionRows.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(colectiveEmission);

                collectiveLoad.Status = MassiveLoadStatus.Issuing;
                collectiveLoad.TotalRows = massiveEmissionRows.Count;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                ExecuteCreatePolicy(massiveEmissionRows, colectiveEmission);
                collectiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
            }
            catch (Exception e)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = e.InnerException.Message;
                collectiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
            }

        }

        private void ExecuteCreatePolicy(List<CollectiveEmissionRow> emissionRows, CollectiveEmission collectiveEmission)
        {
            int userId = 0;
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetPolicyByOperationId(collectiveEmission.TemporalId);

            try
            {
                //Fecha de emisión
                companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                companyPolicy.UserId = collectiveEmission.User.UserId;//USUARIO
                companyPolicy = ValidatePolicyAmounts(emissionRows, companyPolicy);

                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
                collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                List<PendingOperation> pendingOperations;
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
                }
                else
                {
                    pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
                }

                emissionRows = emissionRows.Where(x => x.HasEvents == false).ToList();
                ParallelHelper.ForEach(emissionRows, emissionRow =>
                {
                    CreateRisk(emissionRow, pendingOperations, companyPolicy);
                });
                UpdateJSONPolicyAndRecordEndorsementOperation(collectiveEmission, companyPolicy);
                DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);

                // Personalización WorkFlow y Grabado de textos largo para Polizas colectivas

                // Obtiene el id del usuario
                userId = companyPolicy.UserId;
                // Personalización: Servicio que Guardar el todos los texto largo de la emisión
                DelegateService.underwritingService.SaveTextLarge(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id);
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Collective, collectiveEmission.Id.ToString(), null, collectiveEmission.EndorsementId.ToString());
                DelegateService.underwritingService.SaveControlPolicy(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, collectiveEmission.TemporalId, (int)PolicyOrigin.Collective);
                DelegateService.underwritingService.DeleteTemporalByOperationId(collectiveEmission.TemporalId, (long)companyPolicy.DocumentNumber, companyPolicy.Prefix.Id, companyPolicy.Branch.Id);
            }
            catch (Exception e)
            {
                if (companyPolicy.Endorsement.PolicyId > 0)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                }
            }
        }


        

        private void CreateRisk(CollectiveEmissionRow emissionRow, List<PendingOperation> pendingOperations, CompanyPolicy companyPolicy)
        {
            try
            {
                emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                PendingOperation riskPendingOperation = pendingOperations.Find(po => po.Id == emissionRow.Risk.RiskId);
                if (riskPendingOperation == null)
                {
                    emissionRow.HasError = true;
                    emissionRow.Observations = Errors.ErrorRisksNotFound + KeySettings.ReportErrorSeparatorMessage();
                    return;
                }
                var companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(riskPendingOperation.Operation);
                companyVehicle.Risk.Policy = companyPolicy;
                DelegateService.vehicleService.CreateRisk(companyVehicle);
            }
            catch (Exception ex)
            {
                emissionRow.HasError = true;
                emissionRow.Observations = $"{Errors.ErrorIssuing} | {ex.Message}";
            }
            finally
            {
                emissionRow.Status = CollectiveLoadProcessStatus.Finalized;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
            }
        }

        private void UpdateJSONPolicyAndRecordEndorsementOperation(CollectiveEmission collectiveEmission, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);

                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                PendingOperation pendingOperationPolicy = new PendingOperation
                {
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    Id = companyPolicy.Id
                };

                DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperationPolicy);
                /* with Replicated Database */
            }

        }

        public CompanyPolicy ValidatePolicyAmounts(List<CollectiveEmissionRow> emissionRows, CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy.Summary.RiskCount > 0 && companyPolicy.Summary.RiskCount != emissionRows.Count)
                {
                    ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();

                    ParallelHelper.ForEach(emissionRows, emissionRow =>
                    {
                        try
                        {
                            PendingOperation pendingOperationRisk = new PendingOperation();
                            if (!Settings.UseReplicatedDatabase())
                            {
                                pendingOperationRisk = DelegateService.utilitiesService.GetPendingOperationById(emissionRow.Risk.Id);
                            }
                            else
                            {
                                pendingOperationRisk = DelegateService.pendingOperationEntityService.GetPendingOperationById(emissionRow.Risk.RiskId);
                            }
                            CompanyVehicle vehicle = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperationRisk.Operation);
                            risks.Add(vehicle.Risk);

                        }
                        catch (Exception ex)
                        {
                            emissionRow.HasError = true;
                            emissionRow.Observations = ex.Message;
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                        }
                        finally
                        {
                            DataFacadeManager.Dispose();
                        }
                    });

                    companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks.ToList());

                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(companyPolicy.Id);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(companyPolicy.Id);
                    }


                    pendingOperation.Operation = JsonHelper.SerializeObjectToJson(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                    }

                }

                return companyPolicy;

            }
            catch (Exception)
            {

                throw new ValidationException(Errors.ValidatePolicyAmounts);
            }

        }
    }
}