using Sistran.Company.Application.PendingOperationEntityServiceEEProvider;
using Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Extentions;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Helpers;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Business
{
    public class BusinessPendingOperation
    {

        public int CreatePendingOperation(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    PendingOperation pendingOperation = businessCollection.Trim().GetObject<PendingOperation>();
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                    return pendingOperation.Id;
                }
                catch (Exception ex)
                {
                    ExceptionHelper.LogError(ex.ToString());
                    throw ex;
                }
            }
            return 0;
        }

        public int CreatePendingOperationWithParent(string businessCollection)
        {
            if (businessCollection != null)
            {
                string[] objectsToSave = businessCollection.Split((char)007);
                if (objectsToSave.Count() == 2)
                {
                    try
                    {
                        PendingOperation pendingOperationPolicy = objectsToSave[0].Trim().GetObject<PendingOperation>();
                        PendingOperation pendingOperationRisk = objectsToSave[1].Trim().GetObject<PendingOperation>();
                        pendingOperationPolicy = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperationPolicy);
                        pendingOperationRisk.ParentId = pendingOperationPolicy.Id;
                        pendingOperationRisk = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperationRisk);
                        return pendingOperationPolicy.Id;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.LogError(ex.ToString());
                        throw ex;
                    }
                }
            }
            return 0;
        }

        public int UpdatePendingOperation(string businessCollection)
        {
            if (businessCollection != null)
            {
                string[] objectsToSave = businessCollection.Split(new[] { (char)007 });

                try
                {
                    PendingOperation pendingOperation = new PendingOperation();
                    PendingOperation pendingOperationRisk = new PendingOperation();
                    switch (objectsToSave.Last().Trim())
                    {
                        case nameof(MassiveEmissionRow):
                        case nameof(MassiveRenewalRow):
                            pendingOperation = objectsToSave[0].Trim().GetObject<PendingOperation>();
                            pendingOperationRisk = objectsToSave[1].Trim().GetObject<PendingOperation>();
                            DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                            pendingOperationRisk.ParentId = pendingOperation.Id;
                            DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperationRisk);
                            break;

                        case nameof(CollectiveEmissionRow):
                            pendingOperation = objectsToSave[0].Trim().GetObject<PendingOperation>();
                            this.SaveTemporalByPendingOperation(objectsToSave);
                            break;
                        case nameof(MassiveCancellationRow):
                        case nameof(CollectiveEmission):
                            pendingOperation = objectsToSave[0].Trim().GetObject<PendingOperation>();
                            CompanyPolicy policy = pendingOperation.Operation.GetObject<CompanyPolicy>();
                            DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                            DelegateService.underwritingService.RecordEndorsementOperation(policy.Endorsement.Id, pendingOperation.Id);
                            break;

                        default:
                            foreach (string pendingOperationString in objectsToSave)
                            {
                                pendingOperation = pendingOperationString.Trim().GetObject<PendingOperation>();
                                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                            }
                            break;
                    }

                    return pendingOperation.Id;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return 0;
        }

        public void SaveTemporalByPendingOperation(string[] objectsToSave)
        {
            switch (objectsToSave.Reverse().Skip(1).Take(1).FirstOrDefault())
            {
                case nameof(CompanyVehicle):
                    DelegateService.vehicleCollectiveService.SaveTemporalVehicle(objectsToSave);
                    break;
                case nameof(CompanyTplRisk):
                    DelegateService.thirdPartyLiabilityCollectiveService.SaveTemporalTpl(objectsToSave);
                    break;
            }
        }
    }
}
