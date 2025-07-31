using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Company.Application.VehicleModificationService.EEProvider.DAOs
{
    /// <summary>
    /// Tarifacion Autos Colectivas
    /// </summary>
    public class QuotateInclutionVehicleDAO
    {
        /// <summary>
        /// Quotates the collective load.
        /// </summary>
        /// <param name="collectiveLoadIds">The collective load ids.</param>
        public void QuotateCollectiveLoad(List<int> collectiveLoadIds)
        {
            foreach (int collectiveLoadId in collectiveLoadIds)
            {
                CollectiveEmission collectiveLoad = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoadId, false);

                if (collectiveLoad != null)
                {
                    try
                    {
                        Task.Run(() => QuotateCollectiveLoadProcesses(collectiveLoad));
                    }
                    catch (Exception ex)
                    {
                        collectiveLoad.ErrorDescription = ex.Message;
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Quotates the incluition.
        /// </summary>
        /// <param name="collectiveEmissionId">The collective emission identifier.</param>
        public void QuotateIncluition(int collectiveEmissionId)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveEmissionId, false);

            if (collectiveEmission != null)
            {
                try
                {
                    Task.Run(() => QuotateCollectiveLoadProcesses(collectiveEmission));
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

        /// <summary>
        /// Quotates the collective load processes.
        /// </summary>
        /// <param name="collectiveLoad">The collective load.</param>
        private void QuotateCollectiveLoadProcesses(CollectiveEmission collectiveLoad)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(collectiveLoad.Id);
            if (massiveLoad != null)
            {
                collectiveLoad.Description = massiveLoad.Description;
                collectiveLoad.ErrorDescription = massiveLoad.ErrorDescription;
                collectiveLoad.File = massiveLoad.File;
                collectiveLoad.LoadType = massiveLoad.LoadType;
                collectiveLoad.HasError = massiveLoad.HasError;
                collectiveLoad.Status = massiveLoad.Status;
                collectiveLoad.User = massiveLoad.User;
                collectiveLoad.TotalRows = massiveLoad.TotalRows;
            }

            List<CollectiveEmissionRow> collectiveLoadProcesses = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Validation);

            if (collectiveLoadProcesses.Count > 0)
            {
                collectiveLoad.Status = MassiveLoadStatus.Tariffing;
                collectiveLoad.TotalRows = collectiveLoadProcesses.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);

                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(collectiveLoad.TemporalId);
                CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.IsPersisted = true;
                companyPolicy.Id = pendingOperation.Id;

                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                QuotateCollectiveLoadProcesses(collectiveLoadProcesses, companyPolicy, collectiveLoad.TemporalId);

                collectiveLoad.Status = MassiveLoadStatus.Tariffed;
                collectiveLoad.Premium = companyPolicy.Summary.Premium;
                collectiveLoad.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                collectiveLoad.HasEvents = companyPolicy.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                DelegateService.collectiveService.ValidateAuthorizationPolicies(collectiveLoad);
            }
        }

        /// <summary>
        /// Quotates the collective load processes.
        /// </summary>
        /// <param name="collectiveLoadProcesses">The collective load processes.</param>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="tempId">The temporary identifier.</param>
        /// <returns></returns>
        private CompanyPolicy QuotateCollectiveLoadProcesses(List<CollectiveEmissionRow> collectiveLoadProcesses, CompanyPolicy companyPolicy, int tempId)
        {

            Parallel.ForEach(collectiveLoadProcesses, ParallelHelper.DebugParallelFor(), collectiveLoadProcess =>
            {
                try
                {
                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById((int)collectiveLoadProcess.Risk.RiskId);
                    CompanyVehicle companyRisk = new CompanyVehicle();
                    companyRisk = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperation.Operation);
                    companyRisk.Risk.Id = pendingOperation.Id;
                    companyRisk.Risk.IsPersisted = true;
                    companyRisk.Risk.Policy = companyPolicy;
                    companyRisk = DelegateService.vehicleService.QuotateVehicle(companyRisk, true, true, 0);
                    companyRisk.Risk.Policy = companyPolicy;
                    companyRisk = DelegateService.vehicleService.CreateVehicleTemporal(companyRisk, true);

                    pendingOperation.Operation = JsonConvert.SerializeObject(companyRisk);
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                    companyPolicy.Holder.CompanyName = companyRisk.Risk.MainInsured.CompanyName;
                    //para validar prima
                    collectiveLoadProcess.Risk.Description = companyRisk.LicensePlate;
                    collectiveLoadProcess.Risk.Id = companyRisk.Risk.Id;
                    collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                    collectiveLoadProcess.HasEvents = companyRisk.Risk.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                    collectiveLoadProcess.Premium = companyRisk.Risk.Premium;

                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                }
                catch (Exception ex)
                {
                    collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                    collectiveLoadProcess.HasError = true;
                    collectiveLoadProcess.Observations = ex.Message;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }

            });
            return companyPolicy;
        }
    }
}