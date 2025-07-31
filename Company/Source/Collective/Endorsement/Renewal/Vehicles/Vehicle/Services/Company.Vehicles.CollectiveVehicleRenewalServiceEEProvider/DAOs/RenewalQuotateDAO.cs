using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.DAOs
{
    public class RenewalQuotateDAO
    {
        /// <summary>
        /// Quotates the collective load.
        /// </summary>
        /// <param name="collectiveLoadIds">The collective load ids.</param>
        public void QuotateCollectiveLoads(List<int> collectiveLoadIds)
        {
            foreach (int collectiveLoadId in collectiveLoadIds)
            {
                CollectiveEmission collectiveLoad = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoadId, false);

                if (collectiveLoad != null)
                {
                    try
                    {
                        Task.Run(() => QuotateCollectiveEmissionRows(collectiveLoad));
                    }
                    catch (Exception ex)
                    {
                        collectiveLoad.ErrorDescription = ex.Message;
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                    }
                }
            }
        }
        public void QuotateCollectiveRenewal(int collectiveEmissionId)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveEmissionId, false);

            if (collectiveEmission != null)
            {
                try
                {
                    Task.Run(() => QuotateCollectiveEmissionRows(collectiveEmission));
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

        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(collectiveEmission.Id);
            if (massiveLoad != null)
            {
                collectiveEmission.Description = massiveLoad.Description;
                collectiveEmission.ErrorDescription = massiveLoad.ErrorDescription;
                collectiveEmission.File = massiveLoad.File;
                collectiveEmission.LoadType = massiveLoad.LoadType;
                collectiveEmission.HasError = massiveLoad.HasError;
                collectiveEmission.Status = massiveLoad.Status;
                collectiveEmission.User = massiveLoad.User;
                collectiveEmission.TotalRows = massiveLoad.TotalRows;
            }
            List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Validation);

            if (collectiveEmissionRows.Count > 0)
            {
                collectiveEmission.Status = MassiveLoadStatus.Tariffing;
                collectiveEmission.TotalRows = collectiveEmissionRows.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(collectiveEmission.TemporalId);
                CompanyPolicy companyPolicy = new CompanyPolicy();

                companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);

                companyPolicy.Id = pendingOperation.Id;
                companyPolicy.IsPersisted = true;

                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                companyPolicy = QuotateCollectiveEmissionRows(collectiveEmissionRows, companyPolicy, collectiveEmission.IsAutomatic, collectiveEmission.TemporalId);

                collectiveEmission.Premium = companyPolicy.Summary.Premium;
                collectiveEmission.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                collectiveEmission.HasEvents = companyPolicy.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
        }

        private CompanyPolicy QuotateCollectiveEmissionRows(List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, bool? isAutomatic, int tempId)
        {
            List<CompanyRisk> risks = new List<CompanyRisk>();

            PendingOperation pendingOperation = new PendingOperation();

            foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
            {
                try
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById((int)collectiveEmissionRow.Risk.RiskId);

                    CompanyVehicle companyVehicleRisk = new CompanyVehicle();
                    companyVehicleRisk = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperation.Operation);
                    companyVehicleRisk.Risk.Id = pendingOperation.Id;
                    companyVehicleRisk.Risk.IsPersisted = true;

                    companyVehicleRisk.Risk.Policy = companyPolicy;
                    CompanyVehicle companyVehicleQuotated = DelegateService.vehicleService.QuotateVehicle(companyVehicleRisk, true, true, 0);
                    //Mapper.CreateMap(companyVehicleQuotated.GetType(), companyVehicleRisk.GetType());
                    //Mapper.Map(companyVehicleQuotated, companyVehicleRisk);
                    companyVehicleRisk.Risk.Status = RiskStatusType.Included;
                    risks.Add(companyVehicleRisk.Risk);

                    companyVehicleRisk.Risk.Policy = companyPolicy;
                    DelegateService.vehicleService.CreateVehicleTemporal(companyVehicleRisk, true);
                    companyPolicy.Holder.CompanyName = companyVehicleRisk.Risk.MainInsured.CompanyName;
                    //para validar prima
                    collectiveEmissionRow.Risk.Description = companyVehicleRisk.Risk.Description;
                    collectiveEmissionRow.Risk.RiskId = companyVehicleRisk.Risk.Id;
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                    collectiveEmissionRow.HasEvents = companyVehicleRisk.Risk.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                    collectiveEmissionRow.Premium = companyVehicleRisk.Risk.Premium;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);

                    pendingOperation.Operation = JsonConvert.SerializeObject(companyVehicleRisk);
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                }
                catch (Exception ex)
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Tariff;
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = ex.Message;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
        
            companyPolicy.PayerComponents =DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
 
            companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);
            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotasByCompanyPolicy(companyPolicy);
            companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);

            companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(tempId);
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
            DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

            return companyPolicy;
        }


        public MassiveLoad IssuanceCollectiveRenewal(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                List<CollectiveEmissionRow> massiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                if (massiveEmissionRows.Count > 0)
                {
                    Task.Run(() => IssuanceCollectiveRenewalRows(collectiveLoad, colectiveEmission, massiveEmissionRows));
                }
                else
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = "ErrorRecordsNotFoundToIssue";
                }
            }

            return collectiveLoad;

        }

        public void IssuanceCollectiveRenewalRows(MassiveLoad collectiveLoad, CollectiveEmission colectiveEmission, List<CollectiveEmissionRow> massiveEmissionRows)
        {
            try
            {
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
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetPolicyByOperationId(collectiveEmission.TemporalId);
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
                    emissionRow.Observations = "ErrorRisksNotFound";
                    return;
                }
                var companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(riskPendingOperation.Operation);
                companyVehicle.Risk.Policy = companyPolicy;
                //DelegateService.vehicleService.CreateRisk(companyVehicle);
            }
            catch (Exception ex)
            {
                emissionRow.HasError = true;
                emissionRow.Observations = $"{"ErrorIssuing"} | {ex.Message}";
            }
            finally
            {
                emissionRow.Status = CollectiveLoadProcessStatus.Finalized;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
            }
        }
    }
}