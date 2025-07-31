using Sistran.Company.Application.PendingOperationEntityServiceEEProvider;
using Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Extentions;
using Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Business
{
    using Core.Application.AuthorizationPoliciesServices.Enums;

    public class BusinessIssuance
    {
        public string ExecuteCreatePolicy(string businessCollection)
        {
            CompanyPolicy companyPolicy = new CompanyPolicy();
            MassiveLoadStatus LoadStatus = new MassiveLoadStatus();
            if (businessCollection != null)
            {
                List<Validation> validations = new List<Validation>();
                List<ValidationLicensePlate> validationsLicensePlate = new List<ValidationLicensePlate>();
                string[] objectsToSave = businessCollection.Split(new[] { (char)007 });
                int massiveLoadId = 0, policyId = 0, endorsementId = 0;
                Core.Application.UnderwritingServices.Enums.EndorsementType endorsementType = Core.Application.UnderwritingServices.Enums.EndorsementType.Emission;
                try
                {
                    PendingOperation pendingOperationPolicy = new PendingOperation();

                    int userId = 0;

                    switch (objectsToSave.Last().Trim())
                    {
                        case nameof(MassiveEmissionRow):
                        case nameof(MassiveRenewalRow):
                        case nameof(MassiveCancellationRow):

                            pendingOperationPolicy = objectsToSave[0].Trim().GetObject<PendingOperation>();
                            List<PendingOperation> pendingOperationsRisk = new List<PendingOperation>();
                            companyPolicy = pendingOperationPolicy.Operation.GetObject<CompanyPolicy>();
                            companyPolicy.UserId = pendingOperationPolicy.UserId;

                            if (companyPolicy.Id == 0)
                            {
                                companyPolicy.Id = pendingOperationPolicy.Id;
                            }

                            if (Settings.UseReplicatedDatabase())
                            {
                                pendingOperationsRisk = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(pendingOperationPolicy.Id);
                            }
                            else
                            {
                                pendingOperationsRisk = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(pendingOperationPolicy.Id);
                            }

                            if (pendingOperationsRisk.Count() > 0)
                            {
                                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                                policyId = companyPolicy.Id;
                                endorsementId = companyPolicy.Endorsement.Id;
                                endorsementType = companyPolicy.Endorsement.EndorsementType.Value;
                                int maxRiskCount = companyPolicy.Summary.RiskCount;
                                foreach (PendingOperation po in pendingOperationsRisk)
                                {
                                    switch (objectsToSave.Reverse().Skip(1).Take(1).FirstOrDefault())
                                    {
                                        case nameof(CompanyVehicle):
                                            CompanyVehicle companyVehicle = po.Operation.GetObject<CompanyVehicle>();
                                            companyVehicle.Risk.Policy = companyPolicy;
                                            if (companyVehicle.Risk.Status == RiskStatusType.Original || companyVehicle.Risk.Status == RiskStatusType.Included)
                                            {
                                                companyVehicle.Risk.Number = ++maxRiskCount;
                                            }

                                            //Si el endoso actual es Emisión
                                            if (companyPolicy.Endorsement.EndorsementType.HasValue && companyPolicy.Endorsement.EndorsementType.Value == EndorsementType.Emission)
                                            {
                                                validationsLicensePlate.Add(CreateValidationLicencePlate(companyVehicle, companyPolicy));
                                                validations = DelegateService.vehicleService.GetVehicleLicensePlate(validations, validationsLicensePlate);
                                                foreach (Validation validation in validations)
                                                {
                                                    if (validation.ErrorMessage != string.Empty)
                                                    {
                                                        throw new Exception(validation.ErrorMessage);
                                                    }
                                                }
                                            }

                                            DelegateService.vehicleService.CreateRisk(companyVehicle);
                                            break;
                                        case nameof(CompanyTplRisk):
                                            CompanyTplRisk companyTpl = po.Operation.GetObject<CompanyTplRisk>();
                                            companyTpl.Risk.Policy = companyPolicy;

                                            //Si el endoso actual es Emisión
                                            if (companyPolicy.Endorsement.EndorsementType.HasValue && companyPolicy.Endorsement.EndorsementType.Value == EndorsementType.Emission)
                                            {
                                                validationsLicensePlate.Add(CreateValidationLicencePlate(companyTpl, companyPolicy));
                                                validations = DelegateService.thirdPartyLiabilityService.GetVehicleLicensePlate(validations, validationsLicensePlate);
                                                foreach (Validation validation in validations)
                                                {
                                                    if (validation.ErrorMessage != string.Empty)
                                                    {
                                                        throw new Exception(validation.ErrorMessage);
                                                    }
                                                }
                                            }

                                            if (companyTpl.Risk.Status == RiskStatusType.Original || companyTpl.Risk.Status == RiskStatusType.Included)
                                            {
                                                companyTpl.Risk.Number = ++maxRiskCount;
                                            }
                                            DelegateService.thirdPartyLiabilityService.CreateRisk(companyTpl);
                                            break;
                                    }
                                }

                                DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                                companyPolicy = DelegateService.underwritingService.UpdateCompanyPolicyDocumentNumber(companyPolicy);
                                pendingOperationPolicy.Operation = companyPolicy.GetJson();

                                switch (objectsToSave.Last().Trim())
                                {
                                    case nameof(MassiveEmissionRow):
                                        MassiveEmissionRow emissionRow = objectsToSave[1].Trim().GetObject<MassiveEmissionRow>();
                                        massiveLoadId = emissionRow.MassiveLoadId;
                                        emissionRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                                        emissionRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;
                                        emissionRow.Status = MassiveLoadProcessStatus.Finalized;
                                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(emissionRow);
                                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoadId.ToString(), emissionRow.TempId.ToString(), companyPolicy.Endorsement.Id.ToString());
                                        break;
                                    case nameof(MassiveRenewalRow):
                                        MassiveRenewalRow renewalRow = objectsToSave[1].Trim().GetObject<MassiveRenewalRow>();
                                        massiveLoadId = renewalRow.MassiveRenewalId;
                                        renewalRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                                        renewalRow.Risk.Policy.Endorsement = new Core.Application.UnderwritingServices.Models.Endorsement
                                        {
                                            Id = companyPolicy.Endorsement.Id
                                        };
                                        renewalRow.Status = MassiveLoadProcessStatus.Finalized;
                                        DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(renewalRow);
                                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoadId.ToString(), renewalRow.TemporalId.ToString(), companyPolicy.Endorsement.Id.ToString());
                                        break;
                                    case nameof(MassiveCancellationRow):
                                        MassiveCancellationRow cancellationRow = objectsToSave[1].Trim().GetObject<MassiveCancellationRow>();
                                        massiveLoadId = cancellationRow.MassiveLoadId;
                                        cancellationRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                                        cancellationRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;
                                        cancellationRow.Status = MassiveLoadProcessStatus.Finalized;
                                        DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(cancellationRow);
                                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoadId.ToString(), cancellationRow.tempId.ToString(), companyPolicy.Endorsement.Id.ToString());
                                        break;
                                }
                            }
                            DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoadId);

                            // Personalización WorkFlow y Grabado de textos largo para Polizas individuales
                            // Obtiene el id del usuario
                            userId = companyPolicy.UserId;
                            // Personalización: Servicio que Guardar el todos los texto largo de la emisión
                            DelegateService.underwritingService.SaveTextLarge(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id);
                            // Personalización: ejecuta el evento workflow
                            DelegateService.underwritingService.SaveControlPolicy(companyPolicy.Endorsement.PolicyId, endorsementId, policyId, (int)PolicyOrigin.Massive);
                            DelegateService.underwritingService.DeleteTemporalByOperationId(policyId, (long)companyPolicy.DocumentNumber, companyPolicy.Prefix.Id, companyPolicy.Branch.Id);
                            break;
                        case nameof(CollectiveEmissionRow):

                            CollectiveEmissionRow collectiveEmissionRow = objectsToSave[1].Trim().GetObject<CollectiveEmissionRow>();
                            PendingOperation pendingOperationRisk = new PendingOperation();
                            bool MassiveLoadWithErrors = DelegateService.massiveService.GetMassiveLoadErrorStatus(collectiveEmissionRow.MassiveLoadId);

                            companyPolicy = objectsToSave[0].Trim().GetObject<CompanyPolicy>();

                            pendingOperationRisk = DelegateService.utilitiesServiceCore.GetPendingOperationById(collectiveEmissionRow.Risk.RiskId);

                            switch (objectsToSave.Reverse().Skip(1).Take(1).FirstOrDefault())
                            {
                                case nameof(CompanyVehicle):
                                    CompanyVehicle companyVehicle = pendingOperationRisk.Operation.GetObject<CompanyVehicle>();
                                    companyVehicle.Risk.Policy = companyPolicy;
                                    if (collectiveEmissionRow.Risk != null && collectiveEmissionRow.Risk.Number > 0)
                                    { 
                                        companyVehicle.Risk.Number = collectiveEmissionRow.Risk.Number;
                                    }
                                    policyId = companyVehicle.Risk.Policy.Id;
                                    endorsementId = companyVehicle.Risk.Policy.Endorsement.Id;
                                    endorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType.Value;
                                    if (!MassiveLoadWithErrors)
                                    {
                                        if (companyPolicy.Endorsement.EndorsementType.HasValue && companyPolicy.Endorsement.EndorsementType.Value == EndorsementType.Emission)
                                        {
                                            validationsLicensePlate.Add(CreateValidationLicencePlate(companyVehicle, companyPolicy));
                                            validations = DelegateService.vehicleService.GetVehicleLicensePlate(validations, validationsLicensePlate);
                                            foreach (Validation validation in validations)
                                            {
                                                if (validation.ErrorMessage != string.Empty)
                                                {
                                                    throw new Exception(validation.ErrorMessage);
                                                }
                                            }
                                        }
                                        DelegateService.vehicleService.CreateRisk(companyVehicle);
                                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoadId.ToString(), companyVehicle.Risk.Policy.Id + "|" + companyVehicle.Risk.RiskId, companyPolicy.Endorsement.Id.ToString());
                                    }

                                    break;
                                case nameof(CompanyTplRisk):
                                    CompanyTplRisk companyTplRisk = pendingOperationRisk.Operation.GetObject<CompanyTplRisk>();
                                    companyTplRisk.Risk.Policy = companyPolicy;
                                    companyTplRisk.Risk.Number = collectiveEmissionRow.Risk.Number;
                                    policyId = companyTplRisk.Risk.Policy.Id;
                                    endorsementId = companyTplRisk.Risk.Policy.Endorsement.Id;
                                    endorsementType = companyTplRisk.Risk.Policy.Endorsement.EndorsementType.Value;
                                    if (!MassiveLoadWithErrors)
                                    {
                                        DelegateService.thirdPartyLiabilityService.CreateRisk(companyTplRisk);
                                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoadId.ToString(), companyTplRisk.Risk.Policy.Id + "|" + companyTplRisk.Risk.RiskId, companyPolicy.Endorsement.Id.ToString());
                                    }
                                    break;
                            }

                            collectiveEmissionRow.Status = Core.Application.CollectiveServices.Enums.CollectiveLoadProcessStatus.Finalized;
                            massiveLoadId = collectiveEmissionRow.MassiveLoadId;
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                            if (DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoadId))
                            {
                                DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);

                                if (Settings.UseReplicatedDatabase())
                                {
                                    pendingOperationPolicy = DelegateService.pendingOperationEntityService.GetPendingOperationById(pendingOperationRisk.ParentId);
                                }
                                else
                                {
                                    pendingOperationPolicy = DelegateService.utilitiesServiceCore.GetPendingOperationById(pendingOperationRisk.ParentId);
                                }

                                pendingOperationPolicy.Operation = companyPolicy.GetJson();
                                // Personalización WorkFlow y Grabado de textos largo para Polizas colectivas
                                // Obtiene el id del usuario
                                userId = companyPolicy.UserId;
                                // Personalización: Servicio que Guardar el todos los texto largo de la emisión
                                DelegateService.underwritingService.SaveTextLarge(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id);
                                companyPolicy = DelegateService.underwritingService.UpdateCompanyPolicyDocumentNumber(companyPolicy);
                                var collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, false);
                                LoadStatus = collectiveEmission.Status.Value;
                                collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
                                collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;
                                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                                if (MassiveLoadWithErrors)
                                {
                                    throw new Exception("Poliza Con Errores");
                                }

                                int riskCount = DelegateService.underwritingService.GetCurrentRiskNumByPolicyId(companyPolicy.Endorsement.PolicyId);
                                DelegateService.underwritingService.SaveControlPolicy(companyPolicy.Endorsement.PolicyId, endorsementId, policyId, (int)PolicyOrigin.Collective);
                                DelegateService.underwritingService.DeleteTemporalByOperationId(policyId, (long)companyPolicy.DocumentNumber, companyPolicy.Prefix.Id, companyPolicy.Branch.Id);
                            }
                            break;
                    }

                    if (!Settings.UseReplicatedDatabase())
                    {
                        DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperationPolicy);
                        DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
                        return string.Empty;
                    }
                    return string.IsNullOrEmpty(pendingOperationPolicy.Operation) ? string.Empty : pendingOperationPolicy.GetJson();
                }
                catch (Exception e)
                {
                    string[] messages = e.Message.Split('|');
                    switch (objectsToSave.Last().Trim())
                    {
                        case nameof(MassiveEmissionRow):
                            MassiveEmissionRow emissionRow = objectsToSave[1].Trim().GetObject<MassiveEmissionRow>();
                            massiveLoadId = emissionRow.MassiveLoadId;
                            emissionRow.Status = MassiveLoadProcessStatus.Finalized;
                            emissionRow.Observations = messages[0];
                            emissionRow.HasError = true;
                            DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(emissionRow);
                            break;
                        case nameof(MassiveRenewalRow):
                            MassiveRenewalRow renewalRow = objectsToSave[1].Trim().GetObject<MassiveRenewalRow>();
                            massiveLoadId = renewalRow.MassiveRenewalId;
                            renewalRow.Observations = messages[0];
                            renewalRow.HasError = true;
                            renewalRow.Status = MassiveLoadProcessStatus.Finalized;
                            DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(renewalRow);
                            break;
                        case nameof(MassiveCancellationRow):
                            MassiveCancellationRow cancellationRow = objectsToSave[1].Trim().GetObject<MassiveCancellationRow>();
                            massiveLoadId = cancellationRow.MassiveLoadId;
                            cancellationRow.Observations = messages[0];
                            cancellationRow.HasError = true;
                            cancellationRow.Status = MassiveLoadProcessStatus.Finalized;
                            DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(cancellationRow);
                            break;
                        case nameof(CollectiveEmissionRow):
                            CollectiveEmissionRow collectiveEmissionRow = objectsToSave[1].Trim().GetObject<CollectiveEmissionRow>();
                            collectiveEmissionRow.Observations = messages[0];
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Status = Core.Application.CollectiveServices.Enums.CollectiveLoadProcessStatus.Finalized;
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                            break;
                    }
                    if (DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoadId))
                    {
                        LoadStatus = MassiveLoadStatus.Issued;
                    }


                    if (LoadStatus == MassiveLoadStatus.Issued && companyPolicy.Endorsement.PolicyId > 0 && endorsementId > 0)
                    {
                        DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, endorsementId, endorsementType);
                    }
                    throw e;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Verificar si la placa posee  una póliza vigente.
        /// </summary>
        /// <param name="companyVehicle"></param>
        /// <param name="companyPolicy"></param>
        /// <returns>ValidationLicensePlate</returns>
        private ValidationLicensePlate CreateValidationLicencePlate(CompanyVehicle companyVehicle, CompanyPolicy companyPolicy)
        {
            return new ValidationLicensePlate
            {
                LicensePlate = companyVehicle.LicensePlate,
                Engine = companyVehicle.EngineSerial,
                Chassis = companyVehicle.ChassisSerial,
                CurrentFrom = companyPolicy.CurrentFrom,
                CurrentTo = companyPolicy.CurrentTo,
                Id = 1
            };
        }
        /// <summary>
        /// Verificar si la placa posee  una póliza vigente.
        /// </summary>
        /// <param name="companyVehicle"></param>
        /// <param name="companyPolicy"></param>
        /// <returns>ValidationLicensePlate</returns>
        private ValidationLicensePlate CreateValidationLicencePlate(CompanyTplRisk companyVehicle, CompanyPolicy companyPolicy)
        {
            return new ValidationLicensePlate
            {
                LicensePlate = companyVehicle.LicensePlate,
                Engine = companyVehicle.EngineSerial,
                Chassis = companyVehicle.ChassisSerial,
                CurrentFrom = companyPolicy.CurrentFrom,
                CurrentTo = companyPolicy.CurrentTo,
                Id = 1
            };
        }

        private CompanyPolicy GetPolicy2G(CompanyPolicy companyPolicy)
        {
            /* if (Settings.ImplementValidate2G())
             {
                 ProcessPolicy processPolicy = new ProcessPolicy();
                 processPolicy.BranchId = companyPolicy.Branch.Id;
                 processPolicy.Count_Policies = 1;
                 processPolicy = DelegateService.syBaseEntityService.GetKeyPolicy2G(processPolicy);
                 companyPolicy.Policy2G = processPolicy.Id_Ini;
             }*/
            return companyPolicy;
        }

        public void UpdatePOAndRecordEndorsementOperation(string businessCollection)
        {
            if (!String.IsNullOrEmpty(businessCollection))
            {
                PendingOperation pendingOperationPolicy = businessCollection.Trim().GetObject<PendingOperation>();
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperationPolicy);
                CompanyPolicy companyPolicy = pendingOperationPolicy.Operation.GetObject<CompanyPolicy>();
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
            }
        }

        

        public static EventAuthorization CreateCompanyEventAuthorizationEmision(CompanyPolicy policy, int userId)
        {
            EventAuthorization Event = new EventAuthorization();
            try
            {
                Event.OPERATION1_ID = policy.Endorsement.TicketNumber.ToString();
                Event.OPERATION2_ID = policy.Endorsement.Id.ToString();
                Event.AUTHO_USER_ID = userId;
                Event.EVENT_ID = (int)UnderwritingServices.Enums.EventTypes.SubscriptionMassive;
            }
            catch (Exception ex)
            {
            }
            return Event;
        }
    }
}
