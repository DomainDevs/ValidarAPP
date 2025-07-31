using Sistran.Company.Application.PendingOperationEntityServiceEEProvider;
using Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Extentions;
using Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Business
{
    public class BusinessMassiveRows
    {
        public void UpdateMassiveEmissionRows(string businessCollection, string error)
        {
            string[] objectsToSave = businessCollection.Split((char)007);
            int massiveLoadId = 0;
            if (string.IsNullOrEmpty(error))
            {
                try
                {
                    if (objectsToSave.Length == 3)
                    {
                        switch (objectsToSave.Last().Trim())
                        {
                            case nameof(MassiveEmissionRow):
                                MassiveEmissionRow emissionRow = objectsToSave[0].GetObject<MassiveEmissionRow>();
                                massiveLoadId = emissionRow.MassiveLoadId;
                                emissionRow.Risk.Policy.Id = System.Convert.ToInt32(objectsToSave[1]);
                                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(emissionRow);
                                break;
                            case nameof(MassiveRenewalRow):
                                MassiveRenewalRow renewalRow = objectsToSave[0].GetObject<MassiveRenewalRow>();
                                massiveLoadId = renewalRow.MassiveRenewalId;
                                renewalRow.TemporalId = System.Convert.ToInt32(objectsToSave[1]);
                                renewalRow.Risk.Policy.Id = System.Convert.ToInt32(objectsToSave[1]);
                                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(renewalRow);
                                break;
                            case nameof(CollectiveEmissionRow):
                                CollectiveEmissionRow collectiveRow = objectsToSave[0].Trim().GetObject<CollectiveEmissionRow>();
                                collectiveRow.Risk = collectiveRow.Risk ?? new Core.Application.UnderwritingServices.Models.Risk();
                                collectiveRow.Risk.RiskId = System.Convert.ToInt32(objectsToSave[1]);
                                massiveLoadId = collectiveRow.MassiveLoadId;
                                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveRow);
                                break;
                            case nameof(MassiveCancellationRow):
                                MassiveCancellationRow cancellationRow = objectsToSave[0].GetObject<MassiveCancellationRow>();
                                massiveLoadId = cancellationRow.MassiveLoadId;
                                cancellationRow.Risk.Policy.Id = System.Convert.ToInt32(objectsToSave[1]);
                                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(cancellationRow);
                                break;
                        }
                        DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoadId);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHelper.LogError(ex.ToString());
                    throw ex;
                }
            }
            else
            {
                try
                {
                    switch (objectsToSave.Last().Trim())
                    {
                        case nameof(MassiveEmissionRow):
                            MassiveEmissionRow emissionRow = objectsToSave[0].Trim().GetObject<MassiveEmissionRow>();
                            massiveLoadId = emissionRow.MassiveLoadId;
                            emissionRow.HasError = true;
                            emissionRow.Observations = error;
                            DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(emissionRow);
                            break;
                        case nameof(MassiveRenewalRow):
                            MassiveRenewalRow renewalRow = objectsToSave[0].Trim().GetObject<MassiveRenewalRow>();
                            massiveLoadId = renewalRow.MassiveRenewalId;
                            renewalRow.HasError = true;
                            renewalRow.Observations = error;
                            DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(renewalRow);
                            break;
                        case nameof(CollectiveEmissionRow):
                            CollectiveEmissionRow collectiveRow = objectsToSave[0].Trim().GetObject<CollectiveEmissionRow>();
                            massiveLoadId = collectiveRow.MassiveLoadId;
                            collectiveRow.HasError = true;
                            collectiveRow.Observations = error;
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveRow);
                            break;
                        case nameof(MassiveCancellationRow):
                            MassiveCancellationRow cancellationRow = objectsToSave[0].Trim().GetObject<MassiveCancellationRow>();
                            massiveLoadId = cancellationRow.MassiveLoadId;
                            cancellationRow.HasError = true;
                            cancellationRow.Observations = error;
                            DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(cancellationRow);
                            break;
                    }
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoadId);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.LogError(ex.ToString());
                    throw ex;
                }
            }
        }

    }
}
