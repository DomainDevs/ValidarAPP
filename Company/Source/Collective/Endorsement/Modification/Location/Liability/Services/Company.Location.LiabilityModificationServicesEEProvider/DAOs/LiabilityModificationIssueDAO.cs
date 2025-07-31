using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Helper;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.DAOs
{
    public class LiabilityModificationIssueDAO
    {

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                List<CollectiveEmissionRow> massiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                if (massiveEmissionRows.Count > 0)
                {
                    TP.Task.Run(() => IssuanceCollectiveEmissionRows(collectiveLoad, colectiveEmission, massiveEmissionRows));
                }
                else
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }

            return collectiveLoad;

        }

        public void IssuanceCollectiveEmissionRows(MassiveLoad collectiveLoad, CollectiveEmission colectiveEmission, List<CollectiveEmissionRow> massiveEmissionRows)
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
            companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            List<PendingOperation> pendingOperations;
            if (!Settings.UseReplicatedDatabase())
            {
                pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
            }
            else
            {
                pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
            }
            //foreach (var emissionRow in emissionRows)
            ParallelHelper.ForEach(emissionRows, emissionRow =>
            {
                CreateRisk(emissionRow, pendingOperations, companyPolicy);
            });
            DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
            }
            else
            {
                /* with Replicated Database */
                DelegateService.pendingOperationEntityService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
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
                    emissionRow.Observations ="Riesgo no encotrado" + KeySettings.ReportErrorSeparatorMessage();
                    return;
                }
                var propertyRisk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(riskPendingOperation.Operation);
                propertyRisk.CompanyPolicy = companyPolicy;
                //DelegateService.liabilityService.CreateRisk(propertyRisk);
                emissionRow.Status = CollectiveLoadProcessStatus.Finalized;
            }
            catch (Exception ex)
            {
                emissionRow.HasError = true;
                emissionRow.Observations =ex.Message;
            }
            finally
            {
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
            }
        }

    }
}
