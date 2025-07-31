namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Business
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Core.Application.AuthorizationPoliciesServices.Enums;
    using Core.Application.CommonService.Enums;
    using Core.Application.UnderwritingServices.Enums;
    using Core.Application.UniqueUserServices.Enums;
    using Core.Application.UniqueUserServices.Models;
    using Core.Framework.BAF;
    using Core.Services.UtilitiesServices.Models;
    using UnderwritingServices.Models;

    public class CreatePolicy
    {
        public void CreatePolicyAuthorization(int temporalId)
        {
            CompanyPolicy policy = DelegateService.UnderwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            try
            {
                if (policy != null)
                {
                    DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, "Procesando");

                    BusinessContext bc = new BusinessContext
                    {
                        UserId = policy.UserId
                    };
                    BusinessContext.Current = bc;
                    CompanyPolicyResult companyPolicyResult = this.CreatePolicyEndorsement(policy);

                    string notification;
                    if (companyPolicyResult.Errors != null && companyPolicyResult.Errors.Any())
                    {
                        notification = "El riesgo del temporal " + temporalId + " tiene una poliza vigente-";   

                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, null);

                        NotificationUser notificationUser = new NotificationUser
                        {
                            UserId = policy.UserId,
                            CreateDate = DateTime.Now,
                            NotificationType = new NotificationType { Type = NotificationTypes.ErrorEmission },
                            Message = notification
                        };

                        DelegateService.UniqueUserService.CreateNotification(notificationUser);
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry(
                                "Error (/Api/AuthorizationFunctionApi/CreatePolicyAuthorization) - Temporal no encontrado: " +
                                temporalId+ companyPolicyResult.Errors[0].Error, EventLogEntryType.Information, 0, 1);
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(policy.Branch.Description))
                        {
                            policy.Branch.Description = DelegateService.commonService.GetBranchById(policy.Branch.Id).Description;
                        }

                        if (string.IsNullOrEmpty(policy.Prefix.Description))
                        {
                            policy.Prefix.Description = DelegateService.commonService.GetPrefixById(policy.Prefix.Id).Description;
                        }


                        notification = $"{policy.Branch.Description} - {policy.Prefix.Description} - {companyPolicyResult.DocumentNumber} - {companyPolicyResult.EndorsementNumber}";
                        if (companyPolicyResult.PromissoryNoteNumCode != 0)
                        {
                            notification = $"{notification} - Pagare N° {companyPolicyResult.PromissoryNoteNumCode}";
                        }

                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, companyPolicyResult.EndorsementId.ToString());

                        NotificationUser notificationUser = new NotificationUser
                        {
                            UserId = policy.UserId,
                            CreateDate = DateTime.Now,
                            NotificationType = new NotificationType { Type = NotificationTypes.Emission },
                            Message = notification,
                            Parameters = new Dictionary<string, object>
                            {
                                {"BranchId", policy.Branch.Id},
                                {"PrefixId", policy.Prefix.Id},
                                {"PolicyNumber", companyPolicyResult.DocumentNumber},
                                {"EndorsementId", companyPolicyResult.EndorsementId},
                                {"ProductIsCollective", policy.Product.IsCollective}
                            }
                        };
                        UserPerson person = DelegateService.UniqueUserService.GetPersonByUserId(policy.UserId);

                        if (person?.Emails?.Count > 0)
                        {
                            string strAddress = person.Emails[0].Description;

                            EmailCriteria email = new EmailCriteria
                            {
                                Addressed = new List<string> { strAddress },
                                Message = "<h3>Todas las politicas fueron autorizadas</h3>" + companyPolicyResult.DocumentNumber,
                                Subject = "Politicas autorizadas - " + temporalId
                            };

                            DelegateService.AuthorizationPoliciesService.SendEmail(email);
                        }

                        DelegateService.UniqueUserService.CreateNotification(notificationUser);
                    }
                }

                //using (EventLog eventLog = new EventLog("Application"))
                //{
                //    eventLog.Source = "Application";
                //    eventLog.WriteEntry(
                //        "Error (/Api/AuthorizationFunctionApi/CreatePolicyAuthorization) - Temporal no encontrado: " + temporalId, EventLogEntryType.Information, 0, 1);
                //}
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, temporalId.ToString(), null, "Error al emitir");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(
                        "Error (/Api/AuthorizationFunctionApi/CreatePolicyAuthorization) - Error: " + ex,
                        EventLogEntryType.Information, 0, 1);
                }
            }
        }

        private CompanyPolicyResult CreatePolicyEndorsement(CompanyPolicy companyPolicy)
        {
            companyPolicy.Endorsement.TemporalId = companyPolicy.Id;
            companyPolicy.Endorsement.UserId = companyPolicy.UserId;

            switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType)
            {
                case SubCoveredRiskType.Vehicle:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.VehicleReversionServiceCia.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;

                        default:
                            List<string> existsRiskAuthorization = DelegateService.VehicleService.ExistsRiskAuthorization(companyPolicy.Id);
                            if (existsRiskAuthorization == null)
                            {
                                return DelegateService.VehicleService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                            }
                            else
                            {
                                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult
                                {
                                    Errors = new List<ErrorBase>()
                                };
                                companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = existsRiskAuthorization[0] });
                                return companyPolicyResult;
                            }
                    }
                    break;

                case SubCoveredRiskType.Lease:
                case SubCoveredRiskType.Surety:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.SuretyReversionEndorsementService.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;

                        case EndorsementType.ChangeTermEndorsement:
                            companyPolicy = DelegateService.SuretyChangeTermEndorsementService.CreateChangeTerms(companyPolicy.Endorsement, true);
                            break;

                        case EndorsementType.ChangeAgentEndorsement:
                            companyPolicy = DelegateService.SuretyChangeAgentEndorsementService.CreateEndorsementChangeAgent(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangeConsolidationEndorsement:
                            CompanyChangeConsolidation companyChangeConsolidation = new CompanyChangeConsolidation { Endorsement = companyPolicy.Endorsement };
                            companyPolicy = DelegateService.SuretyChangeConsolidationEndorsementService.CreateEndorsementChangeConsolidation(companyChangeConsolidation, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangePolicyHolderEndorsement:
                            CompanyChangePolicyHolder companyChangePolicyHolder = new CompanyChangePolicyHolder { Endorsement = companyPolicy.Endorsement };
                            companyPolicy = DelegateService.SuretyChangePolicyHolderEndorsementService.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangeCoinsuranceEndorsement:
                            companyPolicy = DelegateService.SuretyChangeCoinsuranceEndorsementService.CreateEndorsementChangeCoinsurance(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;

                        default:
                            return DelegateService.SuretyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true, null);

                    }
                    break;

                case SubCoveredRiskType.Property:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.PropertyReversionService.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;

                        default:
                            return DelegateService.PropertyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);

                    }
                    break;

                case SubCoveredRiskType.Liability:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.LiabilityReversionService.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;

                        case EndorsementType.ChangeAgentEndorsement:
                            companyPolicy = DelegateService.LiabilityChangeAgentService.CreateEndorsementChangeAgent(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangePolicyHolderEndorsement:
                            CompanyChangePolicyHolder companyChangePolicyHolder = new CompanyChangePolicyHolder { Endorsement = companyPolicy.Endorsement };
                            companyPolicy = DelegateService.LiabilityChangePolicyHolderService.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangeCoinsuranceEndorsement:
                            companyPolicy = DelegateService.LiabilityChangeCoinsuranceService.CreateEndorsementChangeCoinsurance(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;
                        case EndorsementType.ChangeTermEndorsement:
                            companyPolicy = DelegateService.liabilityChangeTermServiceCompany.CreateEndorsementChangeTerm(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;
                        default:
                            return DelegateService.LiabilityService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true, null);
                    }
                    break;

                case SubCoveredRiskType.ThirdPartyLiability:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.ThirdPartyLiabilityReversionService.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;

                        default:
                            return DelegateService.ThirdPartyLiabilityService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
                    }
                    break;

                case SubCoveredRiskType.Transport:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.TransportReversionService.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;
                        default:
                            return DelegateService.TransportBusinessService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);

                    }
                    break;

                case SubCoveredRiskType.JudicialSurety:
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EndorsementType.LastEndorsementCancellation:
                            companyPolicy = DelegateService.JudicialSuretyReversionService.CreateEndorsementReversion(companyPolicy.Endorsement, true);
                            break;

                        case EndorsementType.ChangeAgentEndorsement:
                            companyPolicy = DelegateService.judicialSuretyChangeAgentService.CreateEndorsementChangeAgent(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangePolicyHolderEndorsement:
                            CompanyChangePolicyHolder companyChangePolicyHolder = new CompanyChangePolicyHolder { Endorsement = companyPolicy.Endorsement };
                            companyPolicy = DelegateService.judicialSuretyChangePolicyHolderService.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder, true).FirstOrDefault();
                            break;

                        case EndorsementType.ChangeCoinsuranceEndorsement:
                            companyPolicy = DelegateService.judicialSuretyChangeCoinsuranceService.CreateEndorsementChangeCoinsurance(companyPolicy.Endorsement, true).FirstOrDefault();
                            break;

                        default:
                            return DelegateService.JudicialSuretyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, null, true);

                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }


            return new CompanyPolicyResult
            {
                DocumentNumber = companyPolicy.DocumentNumber,
                EndorsementId = companyPolicy.Endorsement.Id,
                EndorsementNumber = companyPolicy.Endorsement.Number
            };
        }
    }
}
