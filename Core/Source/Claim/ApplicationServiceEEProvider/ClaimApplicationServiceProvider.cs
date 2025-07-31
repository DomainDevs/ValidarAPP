using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Business.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniqueUserServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.AircraftServices.DTOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rules = Sistran.Core.Framework.Rules;
using UNDMOD = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest;
using UTILTASK = Sistran.Core.Application.Utilities.Utility;
using UNPMOD = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.ClaimServices.EEProvider
{
    public class ClaimApplicationServiceProvider : IClaimApplicationService
    {
        public List<PrefixDTO> GetPrefixes()
        {
            return DTOAssembler.CreatePrefixes(DelegateService.commonServiceCore.GetPrefixes());
        }

        public List<LineBusinessDTO> GetLinesBusinessByPrefixId(int prefixId)
        {
            return DTOAssembler.CreateLinesBusiness(DelegateService.commonServiceCore.GetLineBusinessByPrefixId(prefixId));
        }

        public List<SubLineBusinessDTO> GetSubLinesBusinessByLineBusinessId(int lineBusinessId)
        {
            return DTOAssembler.CreateSubLinesBusiness(DelegateService.commonServiceCore.GetSubLinesBusinessByLineBusinessId(lineBusinessId));
        }

        public List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBussinessId, int subLineBussinessId)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBussinessId, subLineBussinessId));
        }

        public List<StatusDTO> GetStatusesByEstimationTypeId(int estimationTypeId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateStatuses(estimationTypeDAO.GetStatusesByEstimationTypeId(estimationTypeId));
        }

        public List<StatusDTO> GetEstimationTypeStatusUnassignedByEstimationTypeId(int estimationTypeId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateStatuses(estimationTypeDAO.GetEstimationTypeStatusUnassignedByEstimationTypeId(estimationTypeId));
        }

        public List<StatusDTO> GetStatuses()
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateStatuses(estimationTypeDAO.GetStatuses());
        }

        public List<EstimationTypeDTO> GetEstimationTypes()
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateEstimationTypes(estimationTypeDAO.GetEstimationTypes());
        }

        public List<SelectDTO> GetEstimationTypesByPrefixId(int prefixId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateEstimationTypeByPrefixId(estimationTypeDAO.GetEstimationTypesByPrefixId(prefixId));
        }

        public List<EstimationDTO> GetEstimationsByPrefixId(int prefixId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateEstimationsByPrefixId(estimationTypeDAO.GetEstimationTypesByPrefixId(prefixId));
        }

        public StatusDTO CreateStatusByEstimationType(StatusDTO statusDTO)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateStatus(estimationTypeDAO.CreateStatusByEstimationType(ModelAssembler.CreateStatus(statusDTO), statusDTO.EstimationType.Id));
        }

        public void DeleteStatusByEstimationType(StatusDTO statusDTO)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            estimationTypeDAO.DeleteStatusByEstimationType(ModelAssembler.CreateStatus(statusDTO), statusDTO.EstimationType.Id);
        }

        public List<ReasonDTO> GetReasonsByStatusIdPrefixId(int statusId, int prefixId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateReasons(estimationTypeDAO.GetReasonsByStatusIdPrefixId(statusId, prefixId));
        }

        public ReasonDTO CreateReason(ReasonDTO reasonDTO)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateReason(estimationTypeDAO.CreateReason(ModelAssembler.CreateReason(reasonDTO)));
        }

        public ReasonDTO UpdateReason(ReasonDTO reasonDTO)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateReason(estimationTypeDAO.UpdateReason(ModelAssembler.CreateReason(reasonDTO)));
        }

        public void DeleteReason(int reasonId, int statusId, int prefixId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            estimationTypeDAO.DeleteReason(reasonId, statusId, prefixId);
        }

        public List<PrefixDTO> GetPrefixesByEstimationTypeId(int estimationTypeId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreatePrefixes(estimationTypeDAO.GetPrefixesByEstimationTypeId(estimationTypeId));
        }

        public List<PrefixDTO> CreatePrefixesByEstimationType(int estimationTypeId, List<PrefixDTO> prefixesDTO)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            List<Prefix> originalPrefixes = estimationTypeDAO.GetPrefixesByEstimationTypeId(estimationTypeId);
            List<Prefix> prefixes = ModelAssembler.CreatePrefixes(prefixesDTO);

            foreach (Prefix prefix in prefixes)
            {
                if (!originalPrefixes.Exists(x => x.Id == prefix.Id))
                {
                    estimationTypeDAO.CreatePrefix(estimationTypeId, prefix);
                }

                originalPrefixes.RemoveAll(x => x.Id == prefix.Id);
            }

            foreach (Prefix prefix in originalPrefixes)
            {
                estimationTypeDAO.DeletePrefix(estimationTypeId, prefix.Id);
            }

            return DTOAssembler.CreatePrefixes(prefixes);
        }

        public List<EndorsementDTO> GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(int? prefixId, int? branchId, CoveredRiskType coveredRiskTypeId, decimal documentNumber, DateTime claimDate)
        {
            ClaimEndorsementDAO claimEndorsementDAO = new ClaimEndorsementDAO();
            return DTOAssembler.CreateEndorsements(claimEndorsementDAO.GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(prefixId, branchId, coveredRiskTypeId, documentNumber, claimDate));
        }

        public ClaimVehicleDTO CreateClaimVehicle(ClaimVehicleDTO claimVehicleDTO)
        {
            ClaimVehicleDAO claimVehicleDAO = new ClaimVehicleDAO();
            ClaimVehicle claimVehicle = ModelAssembler.CreateClaimVehicle(claimVehicleDTO);
            claimVehicle.Claim = ModelAssembler.CreateClaim(claimVehicleDTO);

            if (claimVehicle.Claim.BusinessTypeId == 3 && claimVehicle.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimVehicle.Claim.Endorsement.Id, claimVehicle.Claim.Endorsement.PolicyId));
                claimVehicle.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            if (claimVehicleDTO.TemporalId == 0)
            {
                claimVehicleDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimVehicle.Claim);

                if (claimVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!claimVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        claimVehicleDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimVehicle(claimVehicle)).Id;
                    }

                    return claimVehicleDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(claimVehicleDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.ClaimAuthorizationRequestPending);
                }

                if (claimVehicleDTO.AuthorizationPolicies.Any())
                {
                    claimVehicleDTO.AuthorizationPolicies.Clear();
                }
            }

            claimVehicle.Claim.AuthorizationPolicies = claimVehicleDTO.AuthorizationPolicies;
            claimVehicle = claimVehicleDAO.CreateClaimVehicle(claimVehicle);
            claimVehicleDTO = DTOAssembler.CreateClaimVehicle(claimVehicle);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceClaim(claimVehicle.Claim.Id, claimVehicle.Claim.Modifications.Last().Id, claimVehicle.Claim.Modifications.Last().UserId);
            }
            #endregion

            return claimVehicleDTO;
        }

        public ClaimSuretyDTO CreateClaimSurety(ClaimSuretyDTO claimSuretyDTO)
        {
            ClaimSuretyDAO claimSuretyDAO = new ClaimSuretyDAO();
            ClaimSurety claimSurety = ModelAssembler.CreateClaimSurety(claimSuretyDTO);
            claimSurety.Claim = ModelAssembler.CreateClaim(claimSuretyDTO);

            if (claimSurety.Claim.BusinessTypeId == 3 && claimSurety.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimSurety.Claim.Endorsement.PolicyId, claimSurety.Claim.Endorsement.Id));
                claimSurety.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            if (claimSuretyDTO.TemporalId == 0)
            {
                claimSuretyDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimSurety.Claim);

                if (claimSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!claimSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        claimSuretyDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimSurety(claimSurety)).Id;
                    }

                    return claimSuretyDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(claimSuretyDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.ClaimAuthorizationRequestPending);
                }

                if (claimSuretyDTO.AuthorizationPolicies.Any())
                {
                    claimSuretyDTO.AuthorizationPolicies.Clear();
                }
            }

            claimSurety.Claim.AuthorizationPolicies = claimSuretyDTO.AuthorizationPolicies;
            claimSurety = claimSuretyDAO.CreateClaimSurety(claimSurety);
            claimSuretyDTO = DTOAssembler.CreateClaimSurety(claimSurety);

            #region Reinsurance

            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceClaim(claimSurety.Claim.Id, claimSurety.Claim.Modifications.Last().Id, claimSurety.Claim.Modifications.Last().UserId);
            }
            #endregion

            return claimSuretyDTO;
        }

        public ClaimLocationDTO CreateClaimLocation(ClaimLocationDTO claimLocationDTO)
        {
            ClaimLocationDAO claimLocationDAO = new ClaimLocationDAO();
            ClaimLocation claimLocation = ModelAssembler.CreateClaimLocation(claimLocationDTO);
            claimLocation.Claim = ModelAssembler.CreateClaim(claimLocationDTO);

            if (claimLocation.Claim.BusinessTypeId == 3 && claimLocation.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimLocation.Claim.Endorsement.PolicyId, claimLocation.Claim.Endorsement.Id));
                claimLocation.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            if (claimLocationDTO.TemporalId == 0)
            {
                claimLocationDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimLocation.Claim);

                if (claimLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!claimLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        claimLocationDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimLocation(claimLocation)).Id;
                    }

                    return claimLocationDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(claimLocationDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.ClaimAuthorizationRequestPending);
                }

                if (claimLocationDTO.AuthorizationPolicies.Any())
                {
                    claimLocationDTO.AuthorizationPolicies.Clear();
                }
            }

            claimLocation.Claim.AuthorizationPolicies = claimLocationDTO.AuthorizationPolicies;
            claimLocation = claimLocationDAO.CreateClaimLocation(claimLocation);
            claimLocationDTO = DTOAssembler.CreateClaimLocation(claimLocation);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceClaim(claimLocation.Claim.Id, claimLocation.Claim.Modifications.Last().Id, claimLocation.Claim.Modifications.Last().UserId);
            }
            #endregion

            return claimLocationDTO;
        }

        public ClaimSuretyDTO UpdateClaimSurety(ClaimSuretyDTO claimSuretyDTO)
        {
            ClaimSuretyDAO claimSuretyDAO = new ClaimSuretyDAO();
            ClaimSurety claimSurety = ModelAssembler.CreateClaimSurety(claimSuretyDTO);
            claimSurety.Claim = ModelAssembler.CreateClaim(claimSuretyDTO);

            if (claimSurety.Claim.BusinessTypeId == 3 && claimSurety.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimSurety.Claim.Endorsement.PolicyId, claimSurety.Claim.Endorsement.Id));
                claimSurety.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimSuretyDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimSurety.Claim);

            if (claimSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimSuretyDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimSurety(claimSurety)).Id;
                }



                return claimSuretyDTO;
            }
            else
            {
                claimSurety.Claim.AuthorizationPolicies = claimSuretyDTO.AuthorizationPolicies;
                ClaimSuretyDTO ClaimSuretyDTO = DTOAssembler.CreateClaimSurety(claimSuretyDAO.UpdateClaimSurety(claimSurety));
                #region Reinsurance

                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimSurety.Claim.Id, claimSurety.Claim.Modifications.Last().Id, claimSurety.Claim.Modifications.Last().UserId);
                }
                #endregion

                return ClaimSuretyDTO;
            }
        }

        public ClaimLocationDTO UpdateClaimLocation(ClaimLocationDTO claimLocationDTO)
        {
            ClaimLocationDAO claimLocationDAO = new ClaimLocationDAO();
            ClaimLocation claimLocation = ModelAssembler.CreateClaimLocation(claimLocationDTO);
            claimLocation.Claim = ModelAssembler.CreateClaim(claimLocationDTO);

            if (claimLocation.Claim.BusinessTypeId == 3 && claimLocation.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimLocation.Claim.Endorsement.PolicyId, claimLocation.Claim.Endorsement.Id));
                claimLocation.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimLocationDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimLocation.Claim);

            if (claimLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimLocationDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimLocation(claimLocation)).Id;
                }
                return claimLocationDTO;
            }
            else
            {
                claimLocation.Claim.AuthorizationPolicies = claimLocationDTO.AuthorizationPolicies;
                ClaimLocationDTO ClaimLocationDTO = DTOAssembler.CreateClaimLocation(claimLocationDAO.UpdateClaimLocation(claimLocation));
                #region Reinsurance

                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimLocation.Claim.Id, claimLocation.Claim.Modifications.Last().Id, claimLocation.Claim.Modifications.Last().UserId);
                }
                #endregion
                return ClaimLocationDTO;
            }
        }

        public List<BranchDTO> GetBranches()
        {
            return DTOAssembler.CreateBranches(DelegateService.commonServiceCore.GetBranches());
        }

        public PolicyDTO GetPolicyByEndorsementIdModuleType(int endorsementId)
        {
            PolicyDTO policy = DTOAssembler.CreatePolicy(DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(endorsementId));
            if (DelegateService.commonServiceCore.GetParameterByParameterId(12191).BoolParameter.GetValueOrDefault())
            {
                policy.IsReinsurance = DelegateService.claimsReinsuranceWorkerIntegrationServices.ValidateEndorsementReinsurance(policy.EndorsementId);
            }            
            return policy;
        }

        public List<NoticeTypeDTO> GetNoticeTypes()
        {
            NoticeTypeDAO noticeTypeDAO = new NoticeTypeDAO();
            return DTOAssembler.CreateNoticeTypes(noticeTypeDAO.GetNoticeTypes());
        }

        public List<SelectDTO> GetCountries()
        {
            return DTOAssembler.CreateCountries(DelegateService.commonServiceCore.GetCountriesLite());
        }

        public List<SelectDTO> GetStatesByCountryId(int countryId)
        {
            return DTOAssembler.CreateStates(DelegateService.commonServiceCore.GetStatesByCountryId(countryId));
        }

        public List<SelectDTO> GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            return DTOAssembler.CreateCities(DelegateService.commonServiceCore.GetCitiesByCountryIdStateId(countryId, stateId));
        }

        public List<SelectDTO> GetBranchesByUserId(int userId)
        {
            return DTOAssembler.CreateBranchesUser(DelegateService.uniqueUserService.GetBranchesByUserId(userId));
        }

        public List<SelectDTO> GetCurrencies()
        {
            return DTOAssembler.CreateCurries(DelegateService.commonServiceCore.GetCurrencies());
        }

        public List<SelectDTO> GetDamageTypes()
        {
            DamageDAO damageDAO = new DamageDAO();
            return DTOAssembler.CreateDamageTypes(damageDAO.GetDamageTypes());
        }

        public List<SelectDTO> GetDamageResponsibilities()
        {
            DamageDAO damageDAO = new DamageDAO();
            return DTOAssembler.CreateDamageResponsibilities(damageDAO.GetDamageResponsibilities());
        }

        public List<SelectDTO> GetAnalizers()
        {
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            return DTOAssembler.CreateAnalizers(claimSupplierDAO.GetSuppliersBySupplierProfile(Sistran.Core.Services.UtilitiesServices.Enums.SupplierProfile.Analizer));
        }

        public List<SelectDTO> GetAdjusters()
        {
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            return DTOAssembler.CreateAdjusters(claimSupplierDAO.GetSuppliersBySupplierProfile(Sistran.Core.Services.UtilitiesServices.Enums.SupplierProfile.Adjuster));
        }

        public List<SelectDTO> GetResearchers()
        {
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            return DTOAssembler.CreateResearchers(claimSupplierDAO.GetSuppliersBySupplierProfile(Sistran.Core.Services.UtilitiesServices.Enums.SupplierProfile.Researcher));
        }

        public List<SelectDTO> GetPrefixesByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            return DTOAssembler.CreatePrefixesByCoveredRiskTypes(DelegateService.commonServiceCore.GetPrefixesByCoveredRiskType(coveredRiskType));
        }

        public List<SelectDTO> GetDocumentTypesByIndividualType(int typeDocument)
        {
            return DTOAssembler.CreateDocumentTypes(DelegateService.uniquePersonServiceCore.GetDocumentTypes(typeDocument));
        }

        public List<CoverageDTO> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime occurrenceDate, decimal companyParticipationPercentage)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(riskId, occurrenceDate, companyParticipationPercentage));
        }

        public List<CoverageDTO> GetCoveragesByRiskIdDescription(int riskId, string Description)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByRiskId(riskId)).Where(x => x.Description.Contains(Description)).ToList();
        }

        public List<CoverageDTO> GetCoveragesByRiskIdCoverageId(int riskId, int coverageId)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByRiskId(riskId)).Where(x => x.Id.Equals(coverageId)).ToList();
        }

        public List<SelectDTO> GetCatastrophes()
        {
            CatastrophicEventDAO catastrophicEventDAO = new CatastrophicEventDAO();
            return DTOAssembler.CreateCatastrophes(catastrophicEventDAO.GetCatastrophes());
        }

        public List<SelectDTO> GetCatastrophesByDescription(string query)
        {
            CatastrophicEventDAO catastrophicEventDAO = new CatastrophicEventDAO();
            return DTOAssembler.CreateCatastrophes(catastrophicEventDAO.GetCatastrophesByDescription(query));
        }

        public List<SupplierDTO> GetSuppliersBySupplierProfile(Sistran.Core.Services.UtilitiesServices.Enums.SupplierProfile supplierProfile)
        {
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            return DTOAssembler.CreateSuppliers(claimSupplierDAO.GetSuppliersBySupplierProfile(supplierProfile));
        }

        public List<SupplierDTO> GetSuppliersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            return DTOAssembler.CreateSuppliers(claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime movementDate)
        {
            return DelegateService.commonServiceCore.GetModuleDateIssue((int)moduleType, movementDate);
        }

        public List<SelectDTO> GetPersonTypes()
        {
            return DTOAssembler.CreatePersonTypes(DelegateService.uniquePersonServiceCore.GetPersonTypes());
        }

        public List<SelectDTO> GetMaritalStatus()
        {
            return DTOAssembler.CreateMaritalsStatus(DelegateService.uniquePersonServiceCore.GetMaritalStatus());
        }

        public List<InsuredDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            return DTOAssembler.CreateInsureds(DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public InsuredDTO GetInsuredByRiskId(int riskId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            int insuredId = claimDAO.GetInsuredIdByRiskId(riskId);
            return DTOAssembler.CreateInsured(DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(insuredId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());
        }

        public List<ThirdPartyDTO> GetThirdPartyByDescriptionInsuredSearchType(string description, InsuredSearchType insuredSearchType)
        {
            return DTOAssembler.CreateThirdsParty(DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(description, insuredSearchType));
        }

        public List<SelectDTO> GetGenders()
        {
            return DTOAssembler.CreateGenders();
        }

        public List<UserDTO> GetUserByName(string name)
        {
            return DTOAssembler.CreateUsers(DelegateService.uniqueUserService.GetUserByName(name));
        }

        public List<ClaimCoverageActivePanelDTO> GetClaimCoverageActivePanelsByLineBusinessIdSubLineBusinessId(int lineBusinessid, int subLineBusinessId)
        {
            ClaimCoverageActivePanelDAO claimCoverageActivePanelDAO = new ClaimCoverageActivePanelDAO();
            return DTOAssembler.CreateClaimCoverageActivePanels(claimCoverageActivePanelDAO.GetClaimCoverageActivePanelsByLineBusinessIdSubLineBusinessId(lineBusinessid, subLineBusinessId));
        }

        public ClaimCoverageActivePanelDTO UpdateCoverageActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanels)
        {
            ClaimCoverageActivePanelDAO claimCoverageActivePanelDAO = new ClaimCoverageActivePanelDAO();
            ClaimCoverageActivePanel claimCoverageActivePanel = claimCoverageActivePanelDAO.UpdateClaimCoverageActivePanel(ModelAssembler.CreateClaimCoverageActivePanel(claimCoverageActivePanels));
            return DTOAssembler.CreateClaimCoverageActivePanel(claimCoverageActivePanel);
        }

        public ClaimCoverageActivePanelDTO CreateCoverageActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanel)
        {
            ClaimCoverageActivePanelDAO claimCoverageActivePanelDAO = new ClaimCoverageActivePanelDAO();
            return DTOAssembler.CreateClaimCoverageActivePanel(claimCoverageActivePanelDAO.CreateClaimCoverageActivePanel(ModelAssembler.CreateClaimCoverageActivePanel(claimCoverageActivePanel)));
        }

        public List<DebtorDTO> GetDebtorsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            DebtorDAO debtorDAO = new DebtorDAO();
            return DTOAssembler.CreateDebtors(debtorDAO.GetDebtorByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public List<RecuperatorDTO> GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            RecuperatorDAO recuperatorDAO = new RecuperatorDAO();
            return DTOAssembler.CreateRecuperators(recuperatorDAO.GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public List<SelectDTO> GetCancellationReasons()
        {
            CancellationReasonDAO cancellationReasonDAO = new CancellationReasonDAO();
            return DTOAssembler.CreateCancellationReasons(cancellationReasonDAO.GetCancellationReasons());
        }

        public List<HolderDTO> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            return DTOAssembler.CreateHolders(DelegateService.underwritingIntegrationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public ClaimCoverageActivePanelDTO GetActivePanelsByCoverageId(int coverageId)
        {
            ClaimCoverageActivePanelDAO claimCoverageActivePanelDAO = new ClaimCoverageActivePanelDAO();
            return DTOAssembler.CreateCoverageActionPanel(claimCoverageActivePanelDAO.GetActivePanelsByCoverageId(coverageId));
        }

        public List<AffectedDTO> GetAffectedByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<Affected> affecteds = new List<Affected>();
            AffectedDAO affectedDAO = new AffectedDAO();

            int maxRows = 50;

            affecteds.AddRange(affectedDAO.GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            affecteds.AddRange(affectedDAO.GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));

            return DTOAssembler.CreateAffected(affecteds);
        }

        public List<PolicyDTO> GetClaimPoliciesByPolicy(PolicyDTO policyDTO)
        {
            List<PolicyDTO> policiesDTO = DTOAssembler.CreateClaimPolicies(DelegateService.underwritingIntegrationService.GetClaimPoliciesByPolicy(ModelAssembler.CreateClaimPolicy(policyDTO)));

            foreach (PolicyDTO policy in policiesDTO)
            {
                policy.BranchDescription = GetBranches().Where(x => x.Id == policy.BranchId).First().Description;
                policy.PrefixDescription = GetPrefixes().Where(x => x.Id == policy.PrefixId).First().Description;
            }

            return policiesDTO;
        }

        public ClaimVehicleDTO UpdateClaimVehicle(ClaimVehicleDTO claimVehicleDTO)
        {
            ClaimVehicleDAO claimVehicleDAO = new ClaimVehicleDAO();
            ClaimVehicle claimVehicle = ModelAssembler.CreateClaimVehicle(claimVehicleDTO);
            claimVehicle.Claim = ModelAssembler.CreateClaim(claimVehicleDTO);

            if (claimVehicle.Claim.BusinessTypeId == 3 && claimVehicle.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimVehicle.Claim.Endorsement.PolicyId, claimVehicle.Claim.Endorsement.Id));
                claimVehicle.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimVehicleDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimVehicle.Claim);

            if (claimVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimVehicleDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimVehicle(claimVehicle)).Id;
                }
                return claimVehicleDTO;
            }
            else
            {
                claimVehicle.Claim.AuthorizationPolicies = claimVehicleDTO.AuthorizationPolicies;
                ClaimVehicleDTO ClaimVehicleDTO =  DTOAssembler.CreateClaimVehicle(claimVehicleDAO.UpdateClaimVehicle(claimVehicle));
                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimVehicle.Claim.Id, claimVehicle.Claim.Modifications.Last().Id, claimVehicle.Claim.Modifications.Last().UserId);
                }
                #endregion
                return ClaimVehicleDTO;
            }
        }

        public CoverageDeductibleDTO GetCoverageDeductibleByCoverageId(int coverageId)
        {
            return DTOAssembler.CreateClaimDeductible(DelegateService.underwritingIntegrationService.GetCoverageDeductibleByCoverageId(coverageId));
        }

        public List<SelectDTO> GetSearchTypes()
        {
            return DTOAssembler.CreateSearchTypes();
        }

        public ClaimTransportDTO CreateClaimTransport(ClaimTransportDTO claimTransportDTO)
        {
            ClaimTransportDAO claimTransportDAO = new ClaimTransportDAO();
            ClaimTransport claimTransport = ModelAssembler.CreateClaimTransport(claimTransportDTO);
            claimTransport.Claim = ModelAssembler.CreateClaim(claimTransportDTO);

            if (claimTransport.Claim.BusinessTypeId == 3 && claimTransport.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimTransport.Claim.Endorsement.PolicyId, claimTransport.Claim.Endorsement.Id));
                claimTransport.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            if (claimTransportDTO.TemporalId == 0)
            {
                claimTransportDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimTransport.Claim);

                if (claimTransportDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!claimTransportDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        claimTransportDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimTransport(claimTransport)).Id;
                    }

                    return claimTransportDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(claimTransportDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.ClaimAuthorizationRequestPending);
                }

                if (claimTransportDTO.AuthorizationPolicies.Any())
                {
                    claimTransportDTO.AuthorizationPolicies.Clear();
                }
            }

            claimTransport.Claim.AuthorizationPolicies = claimTransportDTO.AuthorizationPolicies;
            claimTransport = claimTransportDAO.CreateClaimTransport(claimTransport);
            claimTransportDTO = DTOAssembler.CreateClaimTransport(claimTransport);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceClaim(claimTransport.Claim.Id, claimTransport.Claim.Modifications.Last().Id, claimTransport.Claim.Modifications.Last().UserId);
            }
            #endregion

            return claimTransportDTO;
        }

        public ClaimTransportDTO UpdateClaimTransport(ClaimTransportDTO claimTransportDTO)
        {
            ClaimTransportDAO claimTransportDAO = new ClaimTransportDAO();
            ClaimTransport claimTransport = ModelAssembler.CreateClaimTransport(claimTransportDTO);
            claimTransport.Claim = ModelAssembler.CreateClaim(claimTransportDTO);

            if (claimTransport.Claim.BusinessTypeId == 3 && claimTransport.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimTransport.Claim.Endorsement.PolicyId, claimTransport.Claim.Endorsement.Id));
                claimTransport.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimTransportDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimTransport.Claim);

            if (claimTransportDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimTransportDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimTransportDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimTransport(claimTransport)).Id;
                }
                return claimTransportDTO;
            }
            else
            {
                claimTransport.Claim.AuthorizationPolicies = claimTransportDTO.AuthorizationPolicies;
                ClaimTransportDTO ClaimTransportDTO = DTOAssembler.CreateClaimTransport(claimTransportDAO.UpdateClaimTransport(claimTransport));
                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimTransport.Claim.Id, claimTransport.Claim.Modifications.Last().Id, claimTransport.Claim.Modifications.Last().UserId);
                }
                #endregion
                return ClaimTransportDTO;
            }
        }
        public ClaimAirCraftDTO CreateClaimAirCraft(ClaimAirCraftDTO claimAirCraftDTO)
        {
            ClaimAirCraftDAO claimAirCraftDAO = new ClaimAirCraftDAO();
            ClaimAirCraft claimAirCraft = ModelAssembler.CreateClaimAirCraft(claimAirCraftDTO);
            claimAirCraft.Claim = ModelAssembler.CreateClaim(claimAirCraftDTO);

            if (claimAirCraft.Claim.BusinessTypeId == 3 && claimAirCraft.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimAirCraft.Claim.Endorsement.PolicyId, claimAirCraft.Claim.Endorsement.Id));
                claimAirCraft.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            if (claimAirCraftDTO.TemporalId == 0)
            {
                claimAirCraftDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimAirCraft.Claim);

                if (claimAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!claimAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        claimAirCraftDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimAircraft(claimAirCraft)).Id;
                    }

                    return claimAirCraftDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(claimAirCraftDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.ClaimAuthorizationRequestPending);
                }

                if (claimAirCraftDTO.AuthorizationPolicies.Any())
                {
                    claimAirCraftDTO.AuthorizationPolicies.Clear();
                }
            }

            claimAirCraft.Claim.AuthorizationPolicies = claimAirCraftDTO.AuthorizationPolicies;
            claimAirCraft = claimAirCraftDAO.CreateClaimAirCraft(claimAirCraft);
            claimAirCraftDTO = DTOAssembler.CreateClaimAirCraft(claimAirCraft);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceClaim(claimAirCraft.Claim.Id, claimAirCraft.Claim.Modifications.Last().Id, claimAirCraft.Claim.Modifications.Last().UserId);
            }
            #endregion

            return claimAirCraftDTO;
        }

        public ClaimAirCraftDTO UpdateClaimAirCraft(ClaimAirCraftDTO claimAirCraftDTO)
        {
            ClaimAirCraftDAO claimAirCraftDAO = new ClaimAirCraftDAO();
            ClaimAirCraft claimAirCraft = ModelAssembler.CreateClaimAirCraft(claimAirCraftDTO);
            claimAirCraft.Claim = ModelAssembler.CreateClaim(claimAirCraftDTO);

            if (claimAirCraft.Claim.BusinessTypeId == 3 && claimAirCraft.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimAirCraft.Claim.Endorsement.PolicyId, claimAirCraft.Claim.Endorsement.Id));
                claimAirCraft.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimAirCraftDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimAirCraft.Claim);

            if (claimAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimAirCraftDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimAircraft(claimAirCraft)).Id;
                }



                return claimAirCraftDTO;
            }
            else
            {
                claimAirCraft.Claim.AuthorizationPolicies = claimAirCraftDTO.AuthorizationPolicies;
                ClaimAirCraftDTO ClaimAirCraftDTO =  DTOAssembler.CreateClaimAirCraft(claimAirCraftDAO.UpdateClaimAirCraft(claimAirCraft));
                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimAirCraft.Claim.Id, claimAirCraft.Claim.Modifications.Last().Id, claimAirCraft.Claim.Modifications.Last().UserId);
                }
                #endregion
                return ClaimAirCraftDTO;
            }
        }

        public ClaimFidelityDTO CreateClaimFidelity(ClaimFidelityDTO claimFidelityDTO)
        {
            ClaimFidelityDAO claimFidelityDAO = new ClaimFidelityDAO();
            ClaimFidelity claimFidelity = ModelAssembler.CreateClaimFidelity(claimFidelityDTO);
            claimFidelity.Claim = ModelAssembler.CreateClaim(claimFidelityDTO);

            if (claimFidelity.Claim.BusinessTypeId == 3 && claimFidelity.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimFidelity.Claim.Endorsement.PolicyId, claimFidelity.Claim.Endorsement.Id));
                claimFidelity.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            if (claimFidelityDTO.TemporalId == 0)
            {
                claimFidelityDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimFidelity.Claim);

                if (claimFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!claimFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        claimFidelityDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimFidelity(claimFidelity)).Id;
                    }

                    return claimFidelityDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(claimFidelityDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Count > 0)
                {
                    throw new BusinessException(Resources.Resources.ClaimAuthorizationRequestPending);
                }

                if (claimFidelityDTO.AuthorizationPolicies.Any())
                {
                    claimFidelityDTO.AuthorizationPolicies.Clear();
                }
            }

            claimFidelity.Claim.AuthorizationPolicies = claimFidelityDTO.AuthorizationPolicies;
            claimFidelity = claimFidelityDAO.CreateClaimFidelity(claimFidelity);
            claimFidelityDTO = DTOAssembler.CreateClaimFidelity(claimFidelity);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                ReinsuranceClaim(claimFidelity.Claim.Id, claimFidelity.Claim.Modifications.Last().Id, claimFidelity.Claim.Modifications.Last().UserId);
            }
            #endregion

            return claimFidelityDTO;
        }

        public ClaimFidelityDTO UpdateClaimFidelity(ClaimFidelityDTO claimFidelityDTO)
        {
            ClaimFidelityDAO claimFidelityDAO = new ClaimFidelityDAO();
            ClaimFidelity claimFidelity = ModelAssembler.CreateClaimFidelity(claimFidelityDTO);
            claimFidelity.Claim = ModelAssembler.CreateClaim(claimFidelityDTO);

            if (claimFidelity.Claim.BusinessTypeId == 3 && claimFidelity.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimFidelity.Claim.Endorsement.PolicyId, claimFidelity.Claim.Endorsement.Id));
                claimFidelity.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimFidelityDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimFidelity.Claim);

            if (claimFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimFidelityDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimFidelity(claimFidelity)).Id;
                }
                return claimFidelityDTO;
            }
            else
            {
                claimFidelity.Claim.AuthorizationPolicies = claimFidelityDTO.AuthorizationPolicies;
                ClaimFidelityDTO ClaimFidelityDTO = DTOAssembler.CreateClaimFidelity(claimFidelityDAO.UpdateClaimFidelity(claimFidelity));
                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimFidelity.Claim.Id, claimFidelity.Claim.Modifications.Last().Id, claimFidelity.Claim.Modifications.Last().UserId);
                }
                #endregion
                return ClaimFidelityDTO;
            }
        }

        public List<CoverageDeductibleDTO> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum)
        {
            return DTOAssembler.CreateClaimDeductibles(DelegateService.underwritingIntegrationService.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNum));
        }

        public List<CoverageDTO> GetCoveragesByLineBusinessId(int lineBusinessId)
        {
            try
            {
                return DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByLineBusinessId(lineBusinessId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ModuleDTO> GetModule(string description)
        {
            return DTOAssembler.CreateModules(DelegateService.uniqueUserService.GetModulesByDescription(description));
        }

        public List<SubModuleDTO> GetSubModule(int moduleId)
        {
            return DTOAssembler.CreateSubModules(DelegateService.uniqueUserService.GetSubModulesByModuleId(moduleId));
        }

        public List<ClaimDocumentationDTO> GetDocumentationBySubmoduleId(int SubmoduleId)
        {
            ClaimDoumentationDAO claimDoumentationDAO = new ClaimDoumentationDAO();
            return DTOAssembler.CreateDocumentations(claimDoumentationDAO.GetDocumentationBySubmodule(SubmoduleId));
        }

        public void DeleteDocumentation(int DocumentationId)
        {
            ClaimDoumentationDAO claimDoumentationDAO = new ClaimDoumentationDAO();
            claimDoumentationDAO.DeleteDocumentation(DocumentationId);
        }

        public ClaimDocumentationDTO CreateDocumentationes(ClaimDocumentationDTO claimsDocumentationDTO)
        {
            ClaimDoumentationDAO claimDoumentationDAO = new ClaimDoumentationDAO();
            return DTOAssembler.CreateDocumentation(claimDoumentationDAO.CreateDocumentation(ModelAssembler.CreateDocumentations(claimsDocumentationDTO)));
        }

        public ClaimDocumentationDTO UpdateDocumentation(ClaimDocumentationDTO claimsDocumentationDTO)
        {
            ClaimDoumentationDAO claimDoumentationDAO = new ClaimDoumentationDAO();
            return DTOAssembler.CreateDocumentation(claimDoumentationDAO.UpdateDocumentation(ModelAssembler.CreateDocumentations(claimsDocumentationDTO)));
        }

        public List<IndividualTaxDTO> GetIndividualTaxesByIndividualIdRoleId(int individualId, int roleId)
        {
            return DTOAssembler.CreateIndividualTaxes(DelegateService.taxServices.GetIndividualTaxCategoryCondition(individualId, roleId));
        }

        public List<PaymentConceptDTO> GetPaymentConceptsByCoverageIdEstimationTypeId(int coverageId, int estimationTypeId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return null;//DTOAssembler.CreatePaymentConcepts(claimDAO.GetPaymentConceptsByCoverageIdEstimationTypeId(coverageId, estimationTypeId));
        }

        public PendingOperationDTO CreatePendingOperation(PendingOperationDTO pendingOperation)
        {
            PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
            return DTOAssembler.CreatePendingOperation(pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperation(pendingOperation)));
        }

        public PendingOperationDTO GetPendingOperationByPendingOperationId(int pendingOperationId)
        {
            PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
            return DTOAssembler.CreatePendingOperation(pendingOperationsDAO.GetPendingOperationByPendingOperationId(pendingOperationId));
        }

        public void DeletePendingOperationByPendingOperationId(int pendingOperationId)
        {
            PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
            pendingOperationsDAO.DeletePendingOperationByPendingOperationId(pendingOperationId);
        }

        public void CreateClaimByTemporalId(int temporalId)
        {
            try
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Claim, temporalId.ToString(), null, "Procesando");

                ClaimBusiness claimBusiness = new ClaimBusiness();
                PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                PendingOperationDTO pendingOperation = DTOAssembler.CreatePendingOperation(pendingOperationsDAO.GetPendingOperationByPendingOperationId(temporalId));

                if (pendingOperation != null)
                {
                    var coveredRiskType = new { Claim = new { CoveredRiskType = "" } };

                    coveredRiskType = JsonConvert.DeserializeAnonymousType(pendingOperation.Operation, coveredRiskType);

                    switch ((CoveredRiskType)Convert.ToInt32(coveredRiskType.Claim.CoveredRiskType))
                    {
                        case CoveredRiskType.Vehicle:
                            ClaimVehicle claimVehicle = JsonConvert.DeserializeObject<ClaimVehicle>(pendingOperation.Operation);
                            claimVehicle.Claim.TemporalId = temporalId;
                            claimVehicle.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimVehicle.Claim.Endorsement.PolicyId, claimVehicle.Claim.Endorsement.Id)));

                            ClaimVehicleDAO claimVehicleDAO = new ClaimVehicleDAO();
                            if (claimVehicle.Claim.Number != 0)
                            {
                                claimVehicle = claimVehicleDAO.UpdateClaimVehicle(claimVehicle);
                            }
                            else
                            {
                                claimVehicle = claimVehicleDAO.CreateClaimVehicle(claimVehicle);
                            }

                            #region Reinsurance
                            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                            {
                                ReinsuranceClaim(claimVehicle.Claim.Id, claimVehicle.Claim.Modifications.Last().Id, claimVehicle.Claim.Modifications.Last().UserId);
                            }
                            #endregion

                            CreateClaimNotificationByAuthorizationPolicies(claimVehicle.Claim);
                            break;
                        case CoveredRiskType.Location:
                            List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = new List<CoInsuranceAssignedDTO>();
                            ClaimLocation claimLocation = JsonConvert.DeserializeObject<ClaimLocation>(pendingOperation.Operation);
                            claimLocation.Claim.TemporalId = temporalId;
                            claimLocation.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimLocation.Claim.Endorsement.PolicyId, claimLocation.Claim.Endorsement.Id)));

                            ClaimLocationDAO claimLocationDAO = new ClaimLocationDAO();
                            if (claimLocation.Claim.Number != 0)
                            {
                                claimLocation = claimLocationDAO.UpdateClaimLocation(claimLocation);
                            }
                            else
                            {
                                claimLocation = claimLocationDAO.CreateClaimLocation(claimLocation);
                            }

                            #region Reinsurance
                            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                            {
                                ReinsuranceClaim(claimLocation.Claim.Id, claimLocation.Claim.Modifications.Last().Id, claimLocation.Claim.Modifications.Last().UserId);
                            }
                            #endregion

                            CreateClaimNotificationByAuthorizationPolicies(claimLocation.Claim);
                            break;
                        case CoveredRiskType.Surety:
                            ClaimSurety claimSurety = JsonConvert.DeserializeObject<ClaimSurety>(pendingOperation.Operation);
                            claimSurety.Claim.TemporalId = temporalId;
                            claimSurety.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimSurety.Claim.Endorsement.PolicyId, claimSurety.Claim.Endorsement.Id)));

                            ClaimSuretyDAO claimSuretyDAO = new ClaimSuretyDAO();
                            if (claimSurety.Claim.Number != 0)
                            {
                                claimSurety = claimSuretyDAO.UpdateClaimSurety(claimSurety);
                            }
                            else
                            {
                                claimSurety = claimSuretyDAO.CreateClaimSurety(claimSurety);
                            }

                            #region Reinsurance
                            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                            {
                                ReinsuranceClaim(claimSurety.Claim.Id, claimSurety.Claim.Modifications.Last().Id, claimSurety.Claim.Modifications.Last().UserId);
                            }
                            #endregion

                            CreateClaimNotificationByAuthorizationPolicies(claimSurety.Claim);
                            break;
                        case CoveredRiskType.Transport:
                            ClaimTransport claimTransport = JsonConvert.DeserializeObject<ClaimTransport>(pendingOperation.Operation);
                            claimTransport.Claim.TemporalId = temporalId;
                            claimTransport.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimTransport.Claim.Endorsement.PolicyId, claimTransport.Claim.Endorsement.Id)));

                            ClaimTransportDAO claimTransportDAO = new ClaimTransportDAO();
                            if (claimTransport.Claim.Number != 0)
                            {
                                claimTransport = claimTransportDAO.UpdateClaimTransport(claimTransport);
                            }
                            else
                            {
                                claimTransport = claimTransportDAO.CreateClaimTransport(claimTransport);
                            }

                            #region Reinsurance
                            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                            {
                                ReinsuranceClaim(claimTransport.Claim.Id, claimTransport.Claim.Modifications.Last().Id, claimTransport.Claim.Modifications.Last().UserId);
                            }
                            #endregion

                            CreateClaimNotificationByAuthorizationPolicies(claimTransport.Claim);
                            break;
                        case CoveredRiskType.Aircraft:
                            ClaimAirCraft claimAirCraft = JsonConvert.DeserializeObject<ClaimAirCraft>(pendingOperation.Operation);
                            claimAirCraft.Claim.TemporalId = temporalId;
                            claimAirCraft.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimAirCraft.Claim.Endorsement.PolicyId, claimAirCraft.Claim.Endorsement.Id)));

                            ClaimAirCraftDAO claimAirCraftDAO = new ClaimAirCraftDAO();
                            if (claimAirCraft.Claim.Number != 0)
                            {
                                claimAirCraft = claimAirCraftDAO.UpdateClaimAirCraft(claimAirCraft);
                            }
                            else
                            {
                                claimAirCraft = claimAirCraftDAO.CreateClaimAirCraft(claimAirCraft);
                            }

                            #region Reinsurance
                            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                            {
                                ReinsuranceClaim(claimAirCraft.Claim.Id, claimAirCraft.Claim.Modifications.Last().Id, claimAirCraft.Claim.Modifications.Last().UserId);
                            }
                            #endregion

                            CreateClaimNotificationByAuthorizationPolicies(claimAirCraft.Claim);
                            break;
                        default:
                            ClaimDAO claimDAO = new ClaimDAO();
                            ClaimReserve claimReserve = JsonConvert.DeserializeObject<ClaimReserve>(pendingOperation.Operation);
                            claimReserve.Claim.TemporalId = temporalId;
                            claimDAO.SetClaimReserve(claimReserve);
                            claimReserve.Claim = claimDAO.GetClaimByClaimId(claimReserve.Claim.Id);

                            #region Reinsurance
                            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                            {
                                ReinsuranceClaim(claimReserve.Claim.Id, claimReserve.Claim.Modifications.Last().Id, claimReserve.Claim.Modifications.Last().UserId);
                            }
                            #endregion

                            CreateClaimNotificationByAuthorizationPolicies(claimReserve.Claim, true);
                            break;
                    }

                    pendingOperationsDAO.DeletePendingOperationByPendingOperationId(temporalId);
                }
                else
                {
                    throw new BusinessException(Resources.Resources.TemporalNotFound);
                }
            }
            catch (Exception ex)
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Claim, temporalId.ToString(), null, "Error al Procesar");
                throw ex;
            }
        }

        #region Notice

        public void CreateClaimNoticeByTemporalId(int noticeTemporalId)
        {
            try
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.ClaimNotice, noticeTemporalId.ToString(), null, "Procesando");

                NoticeBusiness noticeBusiness = new NoticeBusiness();
                PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                PendingOperationDTO pendingOperation = DTOAssembler.CreatePendingOperation(pendingOperationsDAO.GetPendingOperationByPendingOperationId(noticeTemporalId));
                NoticeDAO noticeDAO = new NoticeDAO();

                if (pendingOperation != null)
                {
                    var coveredRiskType = new { Notice = new { CoveredRiskTypeId = "" } };

                    coveredRiskType = JsonConvert.DeserializeAnonymousType(pendingOperation.Operation, coveredRiskType);

                    switch ((CoveredRiskType)Convert.ToInt32(coveredRiskType.Notice.CoveredRiskTypeId))
                    {
                        case CoveredRiskType.Vehicle:
                            NoticeVehicle noticeVehicle = JsonConvert.DeserializeObject<NoticeVehicle>(pendingOperation.Operation);
                            Notice claimNoticeVehicle = noticeVehicle.Notice;
                            noticeVehicle.Notice.TemporalId = noticeTemporalId;
                            if (noticeVehicle.Notice.Number != 0)
                            {
                                noticeVehicle = noticeDAO.UpdateNoticeVehicle(noticeVehicle);
                            }
                            else
                            {
                                noticeVehicle = noticeDAO.CreateNoticeVehicle(noticeVehicle);
                                claimNoticeVehicle.Number = noticeVehicle.Notice.Number;
                            }
                            CreateClaimNoticeNotificationByAuthorizationPolicies(claimNoticeVehicle);
                            break;
                        case CoveredRiskType.Location:
                            NoticeLocation noticeLocation = JsonConvert.DeserializeObject<NoticeLocation>(pendingOperation.Operation);
                            Notice claimNoticeLocation = noticeLocation.Notice;
                            noticeLocation.Notice.TemporalId = noticeTemporalId;
                            if (noticeLocation.Notice.Number != 0)
                            {
                                noticeLocation = noticeDAO.UpdateNoticeLocation(noticeLocation);
                            }
                            else
                            {
                                noticeLocation = noticeDAO.CreateNoticeLocation(noticeLocation);
                                claimNoticeLocation.Number = noticeLocation.Notice.Number;
                            }
                            CreateClaimNoticeNotificationByAuthorizationPolicies(claimNoticeLocation);
                            break;
                        case CoveredRiskType.Surety:
                            NoticeSurety noticeSurety = JsonConvert.DeserializeObject<NoticeSurety>(pendingOperation.Operation);
                            Notice claimNoticeSurety = noticeSurety.Notice;
                            noticeSurety.Notice.TemporalId = noticeTemporalId;
                            if (noticeSurety.Notice.Number != 0)
                            {
                                noticeSurety = noticeDAO.UpdateNoticeSurety(noticeSurety);
                            }
                            else
                            {
                                noticeSurety = noticeDAO.CreateNoticeSurety(noticeSurety);
                                claimNoticeSurety.Number = noticeSurety.Notice.Number;
                            }
                            CreateClaimNoticeNotificationByAuthorizationPolicies(claimNoticeSurety);
                            break;
                        case CoveredRiskType.Transport:
                            NoticeTransport noticeTransport = JsonConvert.DeserializeObject<NoticeTransport>(pendingOperation.Operation);
                            Notice claimNoticeTransport = noticeTransport.Notice;
                            noticeTransport.Notice.TemporalId = noticeTemporalId;
                            if (noticeTransport.Notice.Number != 0)
                            {
                                noticeTransport = noticeDAO.UpdateNoticeTransport(noticeTransport);
                            }
                            else
                            {
                                noticeTransport = noticeDAO.CreateNoticeTransport(noticeTransport);
                                claimNoticeTransport.Number = noticeTransport.Notice.Number;
                            }
                            CreateClaimNoticeNotificationByAuthorizationPolicies(claimNoticeTransport);
                            break;
                        case CoveredRiskType.Aircraft:
                            NoticeAirCraft noticeAirCraft = JsonConvert.DeserializeObject<NoticeAirCraft>(pendingOperation.Operation);
                            Notice claimNoticeAirCraft = noticeAirCraft.Notice;
                            noticeAirCraft.Notice.TemporalId = noticeTemporalId;
                            if (noticeAirCraft.Notice.Number != 0)
                            {
                                noticeAirCraft = noticeDAO.UpdateNoticeAirCraft(noticeAirCraft);
                            }
                            else
                            {
                                noticeAirCraft = noticeDAO.CreateNoticeAirCraft(noticeAirCraft);
                                claimNoticeAirCraft.Number = noticeAirCraft.Notice.Number;
                            }
                            CreateClaimNoticeNotificationByAuthorizationPolicies(claimNoticeAirCraft);
                            break;
                    }

                    pendingOperationsDAO.DeletePendingOperationByPendingOperationId(noticeTemporalId);
                }
                else
                {
                    throw new BusinessException(Resources.Resources.TemporalNotFound);
                }
            }
            catch (Exception ex)
            {
                DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.ClaimNotice, noticeTemporalId.ToString(), null, "Error al Procesar");
                throw ex;
            }
        }

        public void DeleteNoticeCoverageByCoverage(int noticeId, int coverageId, int individualId, int estimateTypeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            noticeDAO.DeleteNoticeCoverage(noticeId, coverageId, individualId, estimateTypeId);
        }

        public List<NoticeDTO> SearchNotices(SearchNoticeDTO searchNoticeDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            List<NoticeDTO> notices = DTOAssembler.CreateNotices(noticeDAO.SearchNotices(ModelAssembler.CreateSearchClaimNotice(searchNoticeDTO)));
            foreach (NoticeDTO notice in notices)
            {
                if (notice.BranchId > 0)
                {
                    notice.BranchDescription = GetBranches().First(x => x.Id == notice.BranchId).Description;
                }

                if (notice.PrefixId > 0)
                {
                    notice.PrefixDescription = GetPrefixes().First(x => x.Id == notice.PrefixId).Description;
                }
            }

            return notices;
        }

        public NoticeDTO ObjectNotice(NoticeDTO notice)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateNotice(noticeDAO.ObjectNotice(ModelAssembler.CreateNotice(notice)));
        }

        public NoticeVehicleDTO CreateNoticeVehicle(NoticeVehicleDTO noticeVehicleDTO, ContactInformationDTO contactInformationDTO, VehicleDTO vehicleDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeVehicle noticeVehicle = ModelAssembler.CreateNoticeVehicle(noticeVehicleDTO, contactInformationDTO, vehicleDTO);
            if (noticeVehicleDTO.TemporalId == 0)
            {
                noticeVehicleDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeVehicle);

                if (noticeVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!noticeVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        noticeVehicleDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeVehicle(noticeVehicle)).Id;
                    }

                    return noticeVehicleDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(noticeVehicleDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Any())
                {
                    throw new BusinessException(Resources.Resources.ClaimNoticeAuthorizationRequestPending);
                }

                if (noticeVehicleDTO.AuthorizationPolicies.Any())
                {
                    noticeVehicleDTO.AuthorizationPolicies.Clear();
                }
            }

            NoticeVehicleDTO resultNoticeVehicleDTO = DTOAssembler.CreateNoticeVehicle(noticeDAO.CreateNoticeVehicle(noticeVehicle));
            resultNoticeVehicleDTO.AuthorizationPolicies = noticeVehicleDTO.AuthorizationPolicies;
            return resultNoticeVehicleDTO;
        }

        public NoticeVehicleDTO UpdateNoticeVehicle(NoticeVehicleDTO noticeVehicleDTO, ContactInformationDTO contactInformationDTO, VehicleDTO vehicleDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeVehicle noticeVehicle = ModelAssembler.CreateNoticeVehicle(noticeVehicleDTO, contactInformationDTO, vehicleDTO);

            noticeVehicleDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeVehicle);

            if (noticeVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!noticeVehicleDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    noticeVehicleDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeVehicle(noticeVehicle)).Id;
                }

                return noticeVehicleDTO;
            }
            else
            {
                NoticeVehicleDTO resultNoticeVehicleDTO = DTOAssembler.CreateNoticeVehicle(noticeDAO.UpdateNoticeVehicle(noticeVehicle));
                resultNoticeVehicleDTO.AuthorizationPolicies = noticeVehicleDTO.AuthorizationPolicies;
                return resultNoticeVehicleDTO;
            }
        }

        public NoticeLocationDTO CreateNoticeLocation(NoticeLocationDTO noticeLocationDTO, ContactInformationDTO contactInformationDTO, RiskLocationDTO locationDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeLocation noticeLocation = ModelAssembler.CreateNoticeLocation(noticeLocationDTO, contactInformationDTO, locationDTO);
            if (noticeLocationDTO.TemporalId == 0)
            {
                noticeLocationDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeLocation);

                if (noticeLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!noticeLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        noticeLocationDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeLocation(noticeLocation)).Id;
                    }

                    return noticeLocationDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(noticeLocationDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Any())
                {
                    throw new BusinessException(Resources.Resources.ClaimNoticeAuthorizationRequestPending);
                }

                if (noticeLocationDTO.AuthorizationPolicies.Any())
                {
                    noticeLocationDTO.AuthorizationPolicies.Clear();
                }
            }

            NoticeLocationDTO resultNoticeLocationDTO = DTOAssembler.CreateNoticeLocation(noticeDAO.CreateNoticeLocation(noticeLocation));
            resultNoticeLocationDTO.AuthorizationPolicies = noticeLocationDTO.AuthorizationPolicies;
            return resultNoticeLocationDTO;

        }

        public NoticeLocationDTO UpdateNoticeLocation(NoticeLocationDTO noticeLocationDTO, ContactInformationDTO contactInformationDTO, RiskLocationDTO locationDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeLocation noticeLocation = ModelAssembler.CreateNoticeLocation(noticeLocationDTO, contactInformationDTO, locationDTO);

            noticeLocationDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeLocation);

            if (noticeLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!noticeLocationDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    noticeLocationDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeLocation(noticeLocation)).Id;
                }

                return noticeLocationDTO;
            }
            else
            {
                NoticeLocationDTO resultNoticeLocationDTO = DTOAssembler.CreateNoticeLocation(noticeDAO.UpdateNoticeLocation(noticeLocation));
                resultNoticeLocationDTO.AuthorizationPolicies = noticeLocationDTO.AuthorizationPolicies;
                return resultNoticeLocationDTO;
            }
        }

        public NoticeSuretyDTO CreateNoticeSurety(NoticeSuretyDTO noticeSuretyDTO, ContactInformationDTO contactInformationDTO, SuretyDTO suretyDTO)
        {

            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeSurety noticeSurety = ModelAssembler.CreateNoticeSurety(noticeSuretyDTO, contactInformationDTO, suretyDTO);
            if (noticeSuretyDTO.TemporalId == 0)
            {
                noticeSuretyDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeSurety);

                if (noticeSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!noticeSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        noticeSuretyDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeSurety(noticeSurety)).Id;
                    }

                    return noticeSuretyDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(noticeSuretyDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Any())
                {
                    throw new BusinessException(Resources.Resources.ClaimNoticeAuthorizationRequestPending);
                }

                if (noticeSuretyDTO.AuthorizationPolicies.Any())
                {
                    noticeSuretyDTO.AuthorizationPolicies.Clear();
                }
            }

            NoticeSuretyDTO resultNoticeSuretyDTO = DTOAssembler.CreateNoticeSurety(noticeDAO.CreateNoticeSurety(noticeSurety));
            resultNoticeSuretyDTO.AuthorizationPolicies = noticeSuretyDTO.AuthorizationPolicies;
            return resultNoticeSuretyDTO;
        }

        public NoticeSuretyDTO UpdateNoticeSurety(NoticeSuretyDTO noticeSuretyDTO, ContactInformationDTO contactInformationDTO, SuretyDTO suretyDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeSurety noticeSurety = ModelAssembler.CreateNoticeSurety(noticeSuretyDTO, contactInformationDTO, suretyDTO);

            noticeSuretyDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeSurety);

            if (noticeSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!noticeSuretyDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    noticeSuretyDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeSurety(noticeSurety)).Id;
                }

                return noticeSuretyDTO;
            }
            else
            {
                NoticeSuretyDTO resultNoticeSuretyDTO = DTOAssembler.CreateNoticeSurety(noticeDAO.UpdateNoticeSurety(noticeSurety));
                resultNoticeSuretyDTO.AuthorizationPolicies = noticeSuretyDTO.AuthorizationPolicies;
                return resultNoticeSuretyDTO;
            }
        }

        public NoticeDTO GetNoticeByNoticeId(int noticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            AffectedDAO affectedDAO = new AffectedDAO();
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            NoticeDTO noticeDTO = DTOAssembler.CreateNotice(noticeDAO.GetNoticeByNoticeId(noticeId));
            // Completo la informaci�n de coberturas 
            if (noticeDTO.Coverages.Count > 0)
            {
                List<CoverageDTO> coverageDTO = DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByRiskId(Convert.ToInt32(noticeDTO.RiskId)));
                //buscamos el nombre y valor asegurado de la cobertura

                foreach (NoticeCoverageDTO noticeCoverageDTO in noticeDTO.Coverages)
                {
                    CoverageDTO coverage = coverageDTO.FirstOrDefault(x => x.Id.Equals(noticeCoverageDTO.CoverageId));

                    noticeCoverageDTO.CoverageName = coverage.Description;
                    noticeCoverageDTO.InsurableAmount = coverage.InsurableAmount;
                    if (!noticeCoverageDTO.IsProspect)
                    {
                        AffectedDTO affected = DTOAssembler.CreateAffected(affectedDAO.GetAffectedsByDescriptionInsuredSearchTypeCustomerType(noticeCoverageDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual)).FirstOrDefault();

                        if (affected != null)
                        {
                            noticeCoverageDTO.FullName = affected.FullName;
                            noticeCoverageDTO.DocumentNumber = affected.DocumentNumber;
                            noticeCoverageDTO.DocumentTypeId = affected.DocumentTypeId;
                        }
                    }
                    else
                    {
                        //UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(noticeCoverageDTO.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();
                        ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(noticeCoverageDTO.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual)?.FirstOrDefault();
                        if (claimSupplier != null)
                        {
                            noticeCoverageDTO.FullName = claimSupplier.FullName;
                            noticeCoverageDTO.DocumentNumber = claimSupplier.DocumentNumber;
                            noticeCoverageDTO.DocumentTypeId = Convert.ToInt16(claimSupplier.DocumentTypeId);
                        }
                    }
                }
            }

            return noticeDTO;
        }

        public VehicleDTO GetRiskVehicleByClaimNoticeId(int claimNoticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateClaimNoticeRiskVehicle(noticeDAO.GetRiskVehicleByClaimNoticeId(claimNoticeId));
        }

        public RiskLocationDTO GetRiskLocationByClaimNoticeId(int claimNoticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateRiskLocation(noticeDAO.GetRiskLocationByClaimNoticeId(claimNoticeId));
        }

        public SuretyDTO GetRiskSuretyByClaimNoticeId(int claimNoticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateRiskSurety(noticeDAO.GetRiskSuretyByClaimNoticeId(claimNoticeId));
        }

        public NoticeTransportDTO CreateNoticeTransport(NoticeTransportDTO noticeTransport, ContactInformationDTO contactInformationDTO, TransportDTO transportDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeTransport transportNotice = ModelAssembler.CreateNoticeTransport(noticeTransport, contactInformationDTO, transportDTO);
            if (noticeTransport.TemporalId == 0)
            {
                noticeTransport.AuthorizationPolicies = ValidateAuthorizationPolicies(transportNotice);

                if (noticeTransport.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!noticeTransport.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        noticeTransport.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeTransport(transportNotice)).Id;
                    }

                    return noticeTransport;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(noticeTransport.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Any())
                {
                    throw new BusinessException(Resources.Resources.ClaimNoticeAuthorizationRequestPending);
                }

                if (noticeTransport.AuthorizationPolicies.Any())
                {
                    noticeTransport.AuthorizationPolicies.Clear();
                }
            }

            NoticeTransportDTO resultNoticeTransportDTO = DTOAssembler.CreateNoticeTransport(noticeDAO.CreateNoticeTransport(transportNotice));
            resultNoticeTransportDTO.AuthorizationPolicies = noticeTransport.AuthorizationPolicies;
            return resultNoticeTransportDTO;
        }

        public NoticeTransportDTO UpdateNoticeTransport(NoticeTransportDTO noticeTransport, ContactInformationDTO contactInformationDTO, TransportDTO transportDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeTransport transportNotice = ModelAssembler.CreateNoticeTransport(noticeTransport, contactInformationDTO, transportDTO);

            noticeTransport.AuthorizationPolicies = ValidateAuthorizationPolicies(transportNotice);

            if (noticeTransport.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!noticeTransport.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    noticeTransport.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeTransport(transportNotice)).Id;
                }

                return noticeTransport;
            }
            else
            {
                NoticeTransportDTO resultNoticeTransportDTO = DTOAssembler.CreateNoticeTransport(noticeDAO.UpdateNoticeTransport(transportNotice));
                resultNoticeTransportDTO.AuthorizationPolicies = noticeTransport.AuthorizationPolicies;
                return resultNoticeTransportDTO;
            }

        }

        public TransportDTO GetRiskTransportByClaimNoticeId(int claimNoticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateRiskTransport(noticeDAO.GetRiskTransportByClaimNoticeId(claimNoticeId));
        }

        public NoticeAirCraftDTO CreateNoticeAirCraft(NoticeAirCraftDTO noticeAirCraftDTO, ContactInformationDTO contactInformationDTO, AirCraftDTO airCraftDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeAirCraft noticeAirCraft = ModelAssembler.CreateNoticeAirCraft(noticeAirCraftDTO, contactInformationDTO, airCraftDTO);
            if (noticeAirCraftDTO.TemporalId == 0)
            {
                noticeAirCraftDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeAirCraft);

                if (noticeAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!noticeAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        noticeAirCraft.Notice.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeAircraft(noticeAirCraft)).Id;
                    }

                    return noticeAirCraftDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(noticeAirCraftDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Any())
                {
                    throw new BusinessException(Resources.Resources.ClaimNoticeAuthorizationRequestPending);
                }

                if (noticeAirCraftDTO.AuthorizationPolicies.Any())
                {
                    noticeAirCraftDTO.AuthorizationPolicies.Clear();
                }
            }
            NoticeAirCraftDTO resultNoticeAirCraftDTO = DTOAssembler.CreateNoticeAirCraft(noticeDAO.CreateNoticeAirCraft(noticeAirCraft));
            resultNoticeAirCraftDTO.AuthorizationPolicies = noticeAirCraftDTO.AuthorizationPolicies;
            return resultNoticeAirCraftDTO;
        }

        public NoticeAirCraftDTO UpdateNoticeAirCraft(NoticeAirCraftDTO noticeAirCraftDTO, ContactInformationDTO contactInformationDTO, AirCraftDTO airCraftDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeAirCraft noticeAirCraft = ModelAssembler.CreateNoticeAirCraft(noticeAirCraftDTO, contactInformationDTO, airCraftDTO);

            noticeAirCraftDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeAirCraft);

            if (noticeAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!noticeAirCraftDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    noticeAirCraft.Notice.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeAircraft(noticeAirCraft)).Id;
                }

                return noticeAirCraftDTO;
            }
            else
            {
                NoticeAirCraftDTO resultNoticeAirCraftDTO = DTOAssembler.CreateNoticeAirCraft(noticeDAO.UpdateNoticeAirCraft(noticeAirCraft));
                resultNoticeAirCraftDTO.AuthorizationPolicies = noticeAirCraftDTO.AuthorizationPolicies;
                return resultNoticeAirCraftDTO;
            }
        }

        public AirCraftDTO GetRiskAirCraftByClaimNoticeId(int claimNoticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateRiskAirCraft(noticeDAO.GetRiskAirCraftByClaimNoticeId(claimNoticeId));
        }

        public NoticeFidelityDTO CreateNoticeFidelity(NoticeFidelityDTO noticeFidelityDTO, ContactInformationDTO contactInformationDTO, FidelityDTO fidelityDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeFidelity noticeFidelity = ModelAssembler.CreateNoticeFidelity(noticeFidelityDTO, contactInformationDTO, fidelityDTO);
            if (noticeFidelityDTO.TemporalId == 0)
            {
                noticeFidelityDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeFidelity);

                if (noticeFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    if (!noticeFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                    {
                        PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                        noticeFidelity.Notice.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeFidelity(noticeFidelity)).Id;
                    }

                    return noticeFidelityDTO;
                }
            }
            else
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(noticeFidelityDTO.TemporalId.ToString());

                if (authorizationRequests.Where(x => x.Status == TypeStatus.Pending).ToList().Any())
                {
                    throw new BusinessException(Resources.Resources.ClaimNoticeAuthorizationRequestPending);
                }

                if (noticeFidelityDTO.AuthorizationPolicies.Any())
                {
                    noticeFidelityDTO.AuthorizationPolicies.Clear();
                }
            }
            NoticeFidelityDTO resultNoticeFidelityDTO = DTOAssembler.CreateNoticeFidelity(noticeDAO.CreateNoticeFidelity(noticeFidelity));
            resultNoticeFidelityDTO.AuthorizationPolicies = noticeFidelityDTO.AuthorizationPolicies;
            return resultNoticeFidelityDTO;
        }

        public NoticeFidelityDTO UpdateNoticeFidelity(NoticeFidelityDTO noticeFidelityDTO, ContactInformationDTO contactInformationDTO, FidelityDTO fidelityDTO)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            NoticeFidelity noticeFidelity = ModelAssembler.CreateNoticeFidelity(noticeFidelityDTO, contactInformationDTO, fidelityDTO);

            noticeFidelityDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(noticeFidelity);

            if (noticeFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!noticeFidelityDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    noticeFidelity.Notice.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByNoticeFidelity(noticeFidelity)).Id;
                }

                return noticeFidelityDTO;
            }
            else
            {
                NoticeFidelityDTO resultNoticeFidelityDTO = DTOAssembler.CreateNoticeFidelity(noticeDAO.UpdateNoticeFidelity(noticeFidelity));
                resultNoticeFidelityDTO.AuthorizationPolicies = noticeFidelityDTO.AuthorizationPolicies;
                return resultNoticeFidelityDTO;
            }
        }

        public FidelityDTO GetRiskFidelityByClaimNoticeId(int claimNoticeId)
        {
            NoticeDAO noticeDAO = new NoticeDAO();
            return DTOAssembler.CreateRiskFidelity(noticeDAO.GetRiskFidelityByClaimNoticeId(claimNoticeId));
        }

        public async void SendEmailToAgendNotice(string subject, string message, string mailDestination)//pendiente
        {
            EmailCriteria email = new EmailCriteria
            {
                Subject = subject,
                Message = message,
                Addressed = new List<string> { mailDestination }
            };

            await DelegateService.utilitiesServiceCore.SendEmailAsync(email);
        }

        public string ScheduleNotice(string subject, string message, DateTime startEventDate, DateTime finishEventDate)
        {
            StringBuilder fileICS = new StringBuilder();
            DateTime now = DateTime.Now;

            message = string.Format("<html><head></head><body>{0}</body></html>", message.Replace("\n", "<br/>"));

            if (!string.IsNullOrEmpty(subject))
            {
                fileICS.AppendLine("BEGIN:VCALENDAR");
                fileICS.AppendLine("VERSION:2.0");
                fileICS.AppendLine("PRODID:-//ICSTestCS/");
                fileICS.AppendLine("CALSCALE:GREGORIAN");
                fileICS.AppendLine("BEGIN:VEVENT");
                fileICS.Append("DTSTART:");

                fileICS.Append(DTOAssembler.FormatDateValue(startEventDate.Year));
                fileICS.Append(DTOAssembler.FormatDateValue(startEventDate.Month));
                fileICS.Append(DTOAssembler.FormatDateValue(startEventDate.Day) + "T");
                fileICS.Append(DTOAssembler.FormatDateValue(startEventDate.Hour));
                fileICS.Append(DTOAssembler.FormatDateValue(startEventDate.Minute));
                fileICS.Append(DTOAssembler.FormatDateValue(startEventDate.Second));

                fileICS.AppendLine("");

                fileICS.Append("DTEND:");
                fileICS.Append(DTOAssembler.FormatDateValue(finishEventDate.Year));
                fileICS.Append(DTOAssembler.FormatDateValue(finishEventDate.Month));
                fileICS.Append(DTOAssembler.FormatDateValue(finishEventDate.Day) + "T");
                fileICS.Append(DTOAssembler.FormatDateValue(23)); // final del d�a
                fileICS.Append(DTOAssembler.FormatDateValue(30)); // final del d�a
                fileICS.Append(DTOAssembler.FormatDateValue(finishEventDate.Second));

                fileICS.AppendLine("");

                fileICS.AppendLine("SUMMARY:" + subject);
                fileICS.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + message);
                fileICS.AppendLine("DESCRIPTION:" + message);
                fileICS.AppendLine("UID:1");
                fileICS.AppendLine("SEQUENCE:0");

                fileICS.Append("DTSTAMP:" + DTOAssembler.FormatDateValue(now.Year));
                fileICS.Append(DTOAssembler.FormatDateValue(now.Month));
                fileICS.Append(DTOAssembler.FormatDateValue(now.Day) + "T");
                fileICS.Append(DTOAssembler.FormatDateValue(now.Hour));
                fileICS.AppendLine(DTOAssembler.FormatDateValue(now.Minute) + "00");

                fileICS.AppendLine("END:VEVENT");
                fileICS.AppendLine("END:VCALENDAR");

                return fileICS.ToString();
            }

            return null;
        }

        #endregion

        #region CauseCoverage

        public List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(int lineBussinessId, int subLineBussinessId, int causeId)
        {
            List<CoverageDTO> coverages = DTOAssembler.CreateCoverages(DelegateService.underwritingIntegrationService.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBussinessId, subLineBussinessId));
            CauseCoverageDAO causeCoverageDAO = new CauseCoverageDAO();
            List<CauseCoverage> causeCoverages = causeCoverageDAO.GetCauseCoveragesByCauseId(causeId);

            return coverages.Where(x => !causeCoverages.Any(y => y.Id == x.Id)).ToList();
        }

        public List<CoverageDTO> GetCoveragesByCauseId(int causeId)
        {
            CauseCoverageDAO causeCoverageDAO = new CauseCoverageDAO();
            return DTOAssembler.CreateCoverages(causeCoverageDAO.GetCauseCoveragesByCauseId(causeId));
        }

        public CoverageDTO CreateCoverageByCause(int causeId, CoverageDTO coverageDTO)
        {
            CauseCoverageDAO causeCoverageDAO = new CauseCoverageDAO();
            return DTOAssembler.CreateCoverage(causeCoverageDAO.CreateCauseCoverage(ModelAssembler.CreateCauseCoverage(causeId, coverageDTO)));
        }

        public void DeleteCoverageByCause(int causeId, int coverageId)
        {
            CauseCoverageDAO causeCoverageDAO = new CauseCoverageDAO();
            causeCoverageDAO.DeleteCauseCoverage(causeId, coverageId);
        }

        #endregion

        #region SubCause

        public List<SubCauseDTO> GetSubCausesByCause(int CauseId)
        {
            SubCauseDAO subCauseDAO = new SubCauseDAO();
            return DTOAssembler.CreateSubCauses(subCauseDAO.GetSubCausesByCauseId(CauseId));
        }

        public SubCauseDTO CreateSubCause(SubCauseDTO subCause)
        {
            SubCauseDAO subCauseDAO = new SubCauseDAO();
            return DTOAssembler.CreateSubCause(subCauseDAO.CreateSubCause(ModelAssembler.CreateSubCause(subCause)));
        }

        public SubCauseDTO UpdateSubCause(SubCauseDTO subCause)
        {
            SubCauseDAO subCauseDAO = new SubCauseDAO();
            return DTOAssembler.CreateSubCause(subCauseDAO.UpdateSubCause(ModelAssembler.CreateSubCause(subCause)));
        }

        public void DeleteSubCause(int subCauseId)
        {
            SubCauseDAO subCauseDAO = new SubCauseDAO();
            subCauseDAO.DeleteSubCause(subCauseId);
        }

        #endregion

        #region Cause

        public List<CauseDTO> GetCausesByPrefixId(int prefixId)
        {
            CauseDAO causeDAO = new CauseDAO();
            return DTOAssembler.CreateCauses(causeDAO.GetCausesByPrefixId(prefixId));
        }

        #endregion

        #region Claim

        public ClaimDTO GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            Claim claim = claimDAO.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);
            return DTOAssembler.CreateClaim(claim);
        }

        public List<SubClaimDTO> GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            Claim claim = claimDAO.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();

            if (claim != null)
            {
                subClaims = DTOAssembler.CreateSubClaims(claim.Modifications.First().Coverages, claim);
                subClaims.ForEach(subClaim =>
                {
                    subClaim.PaymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(subClaim.ClaimId, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CurrencyId);
                });
            }

            return subClaims;
        }

        public List<ClaimDTO> SearchClaims(SearchClaimDTO searchClaimDTO)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            NoticeDAO noticeDAO = new NoticeDAO();
            List<Claim> claims = claimDAO.SearchClaims(ModelAssembler.CreateSearchClaim(searchClaimDTO));

            claims.ForEach(x =>
            {
                if (x.CoveredRiskType == CoveredRiskType.Surety)
                {
                    x.RiskDescription = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(x.RiskDescription, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault()?.FullName;
                }
            });

            List<ClaimDTO> claimDTOs = DTOAssembler.CreateClaims(claims);

            foreach (ClaimDTO claimDTO in claimDTOs)
            {
                if (claimDTO.NoticeId > 0)
                {
                    ContactInformation contactInformation = noticeDAO.GetContactInformationByClaimNoticeCode(Convert.ToInt32(claimDTO.NoticeId));
                    if (contactInformation != null)
                    {
                        claimDTO.ContactInformation = contactInformation.Name;
                    }
                }
            }

            return claimDTOs;
        }

        public List<SubClaimDTO> SearchClaimsBySalaryEstimationCurrentYear(SearchClaimDTO searchClaimDTO, int currentYear)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            NoticeDAO noticeDAO = new NoticeDAO();
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            
            searchClaimDTO.CurrentMinimumSalaryValue = GetMinimumSalaryByYear(currentYear).SalaryMinimumMounth;

            List<Claim> claims = claimDAO.SearchClaimsBySalaryEstimation(ModelAssembler.CreateSearchClaim(searchClaimDTO));
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();
            decimal companyParticipationPercentage = 0;


            foreach (Claim claim in claims)
            {
                companyParticipationPercentage = 100;

                if (claim.BusinessTypeId == 3)
                {
                    UNDMOD.PolicyDTO policyDTO = DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(claim.Endorsement.Id);
                    companyParticipationPercentage = Convert.ToDecimal(policyDTO.CoInsurance?.FirstOrDefault()?.ParticipationOwn);
                }

                List<UNDMOD.CoverageDTO> coverages = DelegateService.underwritingIntegrationService.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(claim.Modifications.First().Coverages.First().RiskId, null, companyParticipationPercentage);

                foreach (ClaimCoverage claimCoverage in claim.Modifications.Last().Coverages)
                {
                    claimCoverage.SubLimitAmount = coverages.First(y => y.CoverageId == claimCoverage.CoverageId).InsuredAmountTotal;

                    if (claim.CoveredRiskType == CoveredRiskType.Surety)
                    {
                        claimCoverage.RiskDescription = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(claimCoverage.RiskDescription, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault()?.FullName;
                    }

                    if (claimCoverage.IsProspect)
                    {                        
                        ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(claimCoverage.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual)?.FirstOrDefault();

                        if (claimSupplier != null)
                        {
                            claimCoverage.AffectedFullName = claimSupplier.FullName;
                        }
                    }
                    else
                    {
                        claimCoverage.AffectedFullName = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(claimCoverage.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault()?.FullName;
                    }
                }

                subClaims.AddRange(DTOAssembler.CreateSubClaimEstimations(claim.Modifications.First().Coverages, claim));
            }

            subClaims.ForEach(subClaim =>
            {
                subClaim.PaymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(subClaim.ClaimId, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CurrencyId);
            });        

            return subClaims;
        }

        public int GetClaimPrefixCoveredRiskTypeByPrefixCode(int prefixCode)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return claimDAO.GetClaimPrefixCoveredRiskTypeByPrefixCode(prefixCode);
        }

        public ClaimCoverageDriverInformationDTO GetDriverByDocumentNumberFullName(string description, InsuredSearchType insuredSearchType)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateDriverByDocumentNumber(claimDAO.GetDriverByDocumentNumberFullName(description, insuredSearchType));
        }

        public List<SubClaimDTO> GetSubClaimsEstimationByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            Claim claim = claimDAO.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();

            if (claim != null)
            {
                claim.Modifications.First().Coverages.ForEach(claimCoverage =>
                {
                    claimCoverage.RiskDescription = claimDAO.GetRiskDescriptionByRiskIdPrefixId(claimCoverage.RiskId, claim.Prefix.Id);
                });

                subClaims = DTOAssembler.CreateSubClaimEstimations(claim.Modifications.First().Coverages, claim);

                UNDMOD.PolicyDTO policyDTO = DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(claim.Endorsement.Id);

                int lineBusinessId = GetLinesBusinessByPrefixId(claim.Prefix.Id).FirstOrDefault().Id;

                foreach (SubClaimDTO subClaim in subClaims)
                {
                    subClaim.LineBusinessId = lineBusinessId;
                    subClaim.PaymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(claim.Id, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CurrencyId);
                    subClaim.EstimationTypeStatusReasonDescription = estimationTypeDAO.GetReasonByReasonIdStatusIdPrefixId(subClaim.EstimationTypeEstatusReasonCode, subClaim.EstimationTypeEstatus, claim.Prefix.Id).Description;
                    subClaim.PolicyHolderName = policyDTO.HolderName;
                }
            }

            return subClaims;
        }

        public List<SubClaimDTO> GetEstimationByClaimId(int claimId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            Claim claim = claimDAO.GetEstimationByClaimId(claimId);


            decimal companyParticipationPercentage = 100;

            if (claim.BusinessTypeId == 3 && claim.IsTotalParticipation)
            {
                UNDMOD.PolicyDTO policyDTO = DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(claim.Endorsement.Id);
                companyParticipationPercentage = Convert.ToDecimal(policyDTO.CoInsurance?.FirstOrDefault()?.ParticipationOwn);
            }

            List<UNDMOD.CoverageDTO> coverages = DelegateService.underwritingIntegrationService.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(claim.Modifications.First().Coverages.First().RiskId, null, companyParticipationPercentage);

            foreach (ClaimCoverage claimCoverage in claim.Modifications.Last().Coverages)
            {
                claimCoverage.SubLimitAmount = coverages.First(y => y.CoverageId == claimCoverage.CoverageId).InsuredAmountTotal;

                if (claim.CoveredRiskType == CoveredRiskType.Surety)
                {
                    claimCoverage.RiskDescription = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(claimCoverage.RiskDescription, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault()?.FullName;
                }

                if (claimCoverage.IsProspect)
                {
                    //UNPMOD.ThirdPerson thirdPerson = DelegateService.uniquePersonServiceCore.GetThirdByDescriptionInsuredSearchType(claimCoverage.IndividualId.ToString(), InsuredSearchType.IndividualId)?.FirstOrDefault();
                    ClaimSupplier claimSupplier = claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(claimCoverage.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual)?.FirstOrDefault();

                    if (claimSupplier != null)
                    {
                        claimCoverage.AffectedFullName = claimSupplier.FullName;
                    }
                }
                else
                {
                    claimCoverage.AffectedFullName = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(claimCoverage.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault()?.FullName;
                }
            }

            List<SubClaimDTO> subClaims = DTOAssembler.CreateSubClaimEstimations(claim.Modifications.First().Coverages, claim);

            subClaims.ForEach(subClaim =>
            {
                subClaim.PaymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(subClaim.ClaimId, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CurrencyId);
            });

            return subClaims;
        }

        public ClaimCoverageDriverInformationDTO GetClaimDriverInformationByClaimCoverageId(int claimCoverageId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaimDriverInformation(claimDAO.GetClaimDriverInformationByClaimCoverageId(claimCoverageId));
        }

        public ClaimCoverageThirdPartyVehicleDTO GetClaimThirdPartyVehicleByClaimCoverageId(int claimCoverageId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaimThirdPartyVehicle(claimDAO.GetClaimThirdPartyVehicleByClaimCoverageId(claimCoverageId));
        }

        public ClaimSupplierDTO GetClaimSupplierByClaimId(int claimId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaimSupplier(claimDAO.GetInspectionByClaimId(claimId));
        }

        public CatastrophicInformationDTO GetClaimCatastrophicInformationByClaimId(int claimId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateCatastrophicInformation(claimDAO.GetCatastrophicEventByClaimId(claimId));
        }

        public ClaimReserveDTO SetClaimReserve(ClaimReserveDTO claimReserveDTO)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            claimReserveDTO.Modifications.First().AccountingDate = GetModuleDateByModuleTypeMovementDate(ModuleType.Claim, DateTime.Today);
            //    claimReserveDTO.Modifications.First().RegistrationDate = claimReserveDTO.Modifications.First().AccountingDate;
          

            ClaimReserve claimReserve = ModelAssembler.CreateClaimReserve(claimReserveDTO);

            if (claimReserve.Claim.BusinessTypeId == 3 && claimReserve.Claim.IsTotalParticipation)
            {
                List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claimReserve.Claim.Endorsement.PolicyId, claimReserve.Claim.Endorsement.Id));
                claimReserve.Claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            claimReserveDTO.AuthorizationPolicies = ValidateAuthorizationPolicies(claimReserve.Claim);

            if (claimReserveDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
            {
                if (!claimReserveDTO.AuthorizationPolicies.Any(x => x.Type == AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive))
                {
                    PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                    claimReserveDTO.TemporalId = pendingOperationsDAO.CreatePendingOperation(ModelAssembler.CreatePendingOperationByClaimReserve(claimReserve)).Id;
                }

                return claimReserveDTO;
            }
            else
            {
                claimReserve.Claim.AuthorizationPolicies = claimReserveDTO.AuthorizationPolicies;
                claimDAO.SetClaimReserve(claimReserve);

                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimReserve.Claim.Id, claimReserve.Claim.Modifications.Last().Id, claimReserve.Claim.Modifications.Last().UserId);
                }
                #endregion

                return DTOAssembler.CreateClaimReserve(claimReserve);
            }
        }

        public void SetClaimReserveByClaimIdSubClaimEstimationTypeIdPaymentUserId(int claimId, int subClaim, int estimationTypeId, int userId)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            ClaimDAO claimDAO = new ClaimDAO();
            Claim claim = claimDAO.GetEstimationByClaimId(claimId);
            int currencyId = claim.Modifications.Last().Coverages.Where(c => c.SubClaim == subClaim).FirstOrDefault().Estimations.Where(s => s.Type.Id == estimationTypeId).FirstOrDefault().Currency.Id;
            decimal paymentTotal = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(claimId, subClaim, estimationTypeId, currencyId);

            if (claim.BusinessTypeId == 3 && claim.IsTotalParticipation)
            {
                List<DTOs.Claims.CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(claim.Endorsement.PolicyId, claim.Endorsement.Id));
                claim.CoInsuranceAssigned = ModelAssembler.CreateCoInsuranceAssigneds(coInsuranceAssignedDTOs);
            }

            // Se captura la ultima modificación
            ClaimModify claimModify = claim.Modifications.Last();
            claimModify.UserId = userId;
            claimModify.Id = 0;
            claimModify.RegistrationDate = DateTime.Now;
            claimModify.AccountingDate = GetModuleDateByModuleTypeMovementDate(ModuleType.Claim, DateTime.Today);

            foreach (ClaimCoverage claimcoverage in claimModify.Coverages)
            {
                foreach (Estimation estimation in claimcoverage.Estimations)
                {
                    if (estimationTypeId == estimation.Type.Id && subClaim == claimcoverage.SubClaim)
                    {
                        if (estimation.MinimumSalaryValue != null && estimation.MinimumSalaryValue != 0)
                        {
                            estimation.MinimumSalariesNumber = decimal.Round(paymentTotal / Convert.ToDecimal(estimation.MinimumSalaryValue),2, MidpointRounding.AwayFromZero);
                        }
                        estimation.Amount = paymentTotal;
                        estimation.Reason.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_ESTIMATION_TYPE_STATUS_REASON_CLOSED_WITH_PAYMENT));
                        estimation.Reason.Status.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_ESTIMATION_TYPE_STATUS_CLOSED));                        
                    }
                    else
                    {
                        estimation.Amount = estimation.AmountAccumulate;
                    }

                    estimation.CreationDate = claimModify.AccountingDate;
                }
            }

            ClaimReserve claimReserve = new ClaimReserve();
            claimReserve.Claim = claim;
            claimDAO.SetClaimReserve(claimReserve);

            #region Reinsurance
            if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
            {
                DelegateService.claimsReinsuranceWorkerIntegrationServices.ReinsuranceClaim(claimReserve.Claim.Id, claimReserve.Claim.Modifications.Last().Id, claimReserve.Claim.Modifications.Last().UserId);                
            }
            #endregion
        }

        public List<SubClaimDTO> GetClaimReserveByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            Claim claim = claimDAO.GetClaimReserveByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();

            if (claim != null)
            {
                decimal companyParticipationPercentage = 100;

                if (claim.BusinessTypeId == 3 && claim.IsTotalParticipation)
                {
                    UNDMOD.PolicyDTO policyDTO = DelegateService.underwritingIntegrationService.GetClaimPolicyByEndorsementId(claim.Endorsement.Id);
                    companyParticipationPercentage = Convert.ToDecimal(policyDTO.CoInsurance?.FirstOrDefault()?.ParticipationOwn);
                }

                List<UNDMOD.CoverageDTO> coverages = DelegateService.underwritingIntegrationService.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(claim.Modifications.First().Coverages.First().RiskId, null, companyParticipationPercentage);
                claim.Modifications.First().Coverages.ForEach(claimCoverage =>
                {
                    claimCoverage.SubLimitAmount = coverages.First(y => y.CoverageId == claimCoverage.CoverageId).InsuredAmountTotal;
                    claimCoverage.RiskDescription = claimDAO.GetRiskDescriptionByRiskIdPrefixId(claimCoverage.RiskId, claim.Prefix.Id);
                });

                subClaims = DTOAssembler.CreateSubClaimReserve(claim.Modifications.First().Coverages, claim);

                subClaims.ForEach(subClaim =>
                {
                    subClaim.PaymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(subClaim.ClaimId, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CurrencyId);
                });
            }

            return subClaims;
        }


        public List<SubClaimDTO> GetClaimModifiesByClaimId(int claimId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            Claim claim = claimDAO.GetClaimModifiesByClaimId(claimId);

            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();
            List<UNDMOD.CoverageDTO> coverages = DelegateService.underwritingIntegrationService.GetCoveragesByRiskId(claim.Modifications.First().Coverages.First().RiskId);

            if (claim != null)
            {
                subClaims = DTOAssembler.CreateSubClaims(claim);

                foreach (UNDMOD.CoverageDTO coverage in coverages)
                {
                    foreach (SubClaimDTO subClaim in subClaims)
                    {
                        if (coverage.CoverageId == subClaim.CoverageId)
                        {
                            subClaim.MinimumSalaryValue = subClaim.MinimumSalaryValue;
                            subClaim.InsuredAmountTotal = coverage.InsuredAmountTotal;
                            subClaim.PaymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(subClaim.ClaimId, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CurrencyId, GetEndDateClaimModify(subClaims, subClaim.SubClaim, subClaim.EstimationTypeId, subClaim.CreationDate));
                            subClaim.Reservation = subClaim.EstimateAmount - subClaim.PaymentValue;
                        }
                    }
                }

            }

            return subClaims;
        }


        public List<SubClaimDTO> UpdateEstimationsSalaries(List<SubClaimDTO> subClaimsDTO, int currentYear)
        {
            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            ClaimDAO claimDAO = new ClaimDAO();          
            ClaimReserve claimReserve = new ClaimReserve();
            decimal paymentValue = 0;
            DateTime accountingDate = GetModuleDateByModuleTypeMovementDate(ModuleType.Claim, DateTime.Today);
            decimal currentMinimumSalary = GetMinimumSalaryByYear(currentYear).SalaryMinimumMounth;
            List<int> estimationTypesSalaries = GetEstimationTypesSalariesEstimation();
            subClaimsDTO = subClaimsDTO.DistinctBy(x => x.ClaimId).ToList();

            foreach (SubClaimDTO subClaim in subClaimsDTO)
            {                
                claimReserve.Claim = claimDAO.GetEstimationByClaimId(subClaim.ClaimId);

                // Se captura la ultima modificación
                ClaimModify claimModify = claimReserve.Claim.Modifications.Last();

                claimModify.Id = 0;
                claimModify.RegistrationDate = subClaim.CreationDate;
                claimModify.AccountingDate = accountingDate;
                claimModify.UserId = subClaim.UserId;

                foreach (ClaimCoverage claimCoverage in claimModify.Coverages)
                {
                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        if (estimationTypesSalaries.Contains(estimation.Type.Id))
                        {
                            paymentValue = paymentRequestDAO.GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(claimReserve.Claim.Id, claimCoverage.SubClaim, estimation.Type.Id, estimation.Currency.Id);

                            estimation.Version = 1;
                            estimation.Amount = decimal.Round((estimation.AmountAccumulate - paymentValue) / Convert.ToDecimal(estimation.MinimumSalaryValue) * currentMinimumSalary + paymentValue, 2, MidpointRounding.AwayFromZero);
                            estimation.MinimumSalaryValue = currentMinimumSalary;
                            estimation.MinimumSalariesNumber = decimal.Round(estimation.Amount / currentMinimumSalary, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            estimation.Amount = estimation.AmountAccumulate;
                        }

                        estimation.CreationDate = claimModify.AccountingDate;
                    }
                }

                claimDAO.SetClaimReserve(claimReserve);

                #region Reinsurance
                if (Convert.ToBoolean(DelegateService.commonServiceCore.GetParameterByDescription("ClaimsReinsuranceEnable").BoolParameter))
                {
                    ReinsuranceClaim(claimReserve.Claim.Id, claimReserve.Claim.Modifications.Last().Id, claimReserve.Claim.Modifications.Last().UserId);
                }
                #endregion
            }

            return subClaimsDTO;
        }

        public DateTime? GetEndDateClaimModify(List<SubClaimDTO> subClaims, int subClaimId, int estimationTypeId, DateTime creationDateSubclaim)
        {
            DateTime? endDateClaimModify = null;

            SubClaimDTO subClaim = subClaims.Where(x => x.SubClaim == subClaimId && x.EstimationTypeId == estimationTypeId && x.CreationDate > creationDateSubclaim).OrderBy(x => x.CreationDate).FirstOrDefault();

            if (subClaim != null)
            {
                endDateClaimModify = subClaim.CreationDate;
            }

            return endDateClaimModify;
        }

        public List<EstimationDTO> GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(int claimModifyId, int prefixId, int coverageId, int individualId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateEstimationByClaimId(claimDAO.GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(claimModifyId, prefixId, coverageId, individualId));
        }

        public ClaimDTO GetClaimByClaimId(int claimId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaim(claimDAO.GetClaimByClaimId(claimId));
        }

        public List<ClaimDTO> GetClaimsByPolicyId(int policyId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            List<Claim> claims = claimDAO.GetClaimsByPolicyId(policyId);
            List<ClaimDTO> claimsDTO = new List<ClaimDTO>();
            foreach (Claim claim in claims)
            {
                ClaimDTO claimDTO = DTOAssembler.CreateClaim(claim);
                decimal estimatedValue = 0;
                foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                {
                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        estimatedValue += estimation.AmountAccumulate;
                    }
                }

                claimDTO.EstimatedValue = estimatedValue;

                claimsDTO.Add(claimDTO);
            }

            return claimsDTO;
        }

        public List<ClaimDTO> GetClaimsByPolicyIdOccurrenceDate(int policyId, DateTime occurrenceDate)
        {

            ClaimDAO claimDAO = new ClaimDAO();
            List<Claim> claims = claimDAO.GetClaimsByPolicyIdOccurrenceDate(policyId, occurrenceDate);
            List<ClaimDTO> claimsDTO = new List<ClaimDTO>();
            foreach (Claim claim in claims)
            {
                ClaimDTO claimDTO = DTOAssembler.CreateClaim(claim);
                decimal estimatedValue = 0;
                foreach (ClaimCoverage claimCoverage in claim.Modifications.First().Coverages)
                {
                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        estimatedValue += estimation.Amount;
                    }
                }

                claimDTO.EstimatedValue = estimatedValue;

                claimsDTO.Add(claimDTO);
            }

            return claimsDTO;

        }

        public ClaimLimitDTO GetInsuredAmount(int policyId, int riskNum, int coverageId, int coverNum, int claimId, int subClaimId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaimInsuredAmount(claimDAO.GetInsuredAmount(policyId, riskNum, coverageId, coverNum, claimId, subClaimId));
        }

        public CoveragePaymentConceptDTO CreatePaymentConcept(CoveragePaymentConceptDTO coveragePaymentConceptDTO)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateCoveragePaymentConcept(claimDAO.CreatePaymentConcept(ModelAssembler.CreateCoveragePaymentConcepts(coveragePaymentConceptDTO)));
        }

        public void DeletePaymentConcept(int conceptId, int coverageId, int estimationTypeId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimDAO.DeletePaymentConcept(conceptId, coverageId, estimationTypeId);
        }

        public string GetAffectedPropertyByClaimCoverageId(int claimCoverageId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return claimDAO.GetAffectedPropertyByClaimCoverageId(claimCoverageId);
        }

        public ClaimCoverageDTO GetClaimedAmountByClaimCoverageId(int claimCoverageId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaimCoverage(claimDAO.GetClaimedAmountByClaimCoverageId(claimCoverageId));
        }

        public List<ThirdAffectedDTO> GetThirdAffectedByClaimCoverageId(int claimCoverageId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            return DTOAssembler.CreateClaimCoverageThirdAffecteds(claimDAO.GetThirdAffectedByClaimCoverageId(claimCoverageId));
        }

        #endregion

        #region Participant

        public ClaimParticipantDTO CreateParticipant(ClaimParticipantDTO claimParticipantDTO)
        {
            ParticipantDAO participantDAO = new ParticipantDAO();
            return DTOAssembler.CreateParticipant(participantDAO.CreateParticipant(ModelAssembler.CreateParticipant(claimParticipantDTO)));
        }

        public List<ClaimParticipantDTO> GetParticipantsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            ParticipantDAO participantDAO = new ParticipantDAO();
            return DTOAssembler.CreateParticipants(participantDAO.GetParticipantsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public ClaimParticipantDTO GetParticipantByParticipantId(int participantId)
        {
            ParticipantDAO participantDAO = new ParticipantDAO();
            return DTOAssembler.CreateParticipant(participantDAO.GetParticipantByParticipantId(participantId));
        }

        #endregion

        #region Vehicle              

        public List<SelectDTO> GetVehicleColors()
        {
            return DTOAssembler.CreateVehicleColors(DelegateService.vehicleIntegrationService.GetVehicleColors());
        }

        public List<VehicleDTO> GetRiskVehiclesByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleIntegrationService.GetRiskVehiclesByEndorsementId(endorsementId));
        }

        public List<VehicleDTO> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleIntegrationService.GetRiskVehiclesByEndorsementIdModuleType(endorsementId, moduleType));
        }

        public List<SelectDTO> GetVehicleMakes()
        {
            return DTOAssembler.CreateVehicleMakes(DelegateService.vehicleIntegrationService.GetVehicleMakes());
        }

        public List<SelectDTO> GetVehicleMakesByDescription(string description)
        {
            return DTOAssembler.CreateVehicleMakes(DelegateService.vehicleIntegrationService.GetVehicleMakesByDescription(description));
        }

        public List<SelectDTO> GetVehicleYearsByMakeIdModelIdVersionId(int MakeId, int ModelId, int VersionId)
        {
            return DTOAssembler.CreateVehicleYears(DelegateService.vehicleIntegrationService.GetVehicleYearsByMakeIdModelIdVersionId(MakeId, ModelId, VersionId));
        }

        public List<SelectDTO> GetVehicleModelsByMakeId(int makeId)
        {
            return DTOAssembler.CreateVehicleModels(DelegateService.vehicleIntegrationService.GetVehicleModelsByMakeId(makeId));
        }

        public List<SelectDTO> GetVehicleModelsByDescription(string description)
        {
            return DTOAssembler.CreateVehicleModels(DelegateService.vehicleIntegrationService.GetVehicleModelsByDescription(description));
        }

        public List<SelectDTO> GetVehicleVersionsByMakeIdModelId(int makeId, int modelId)
        {
            return DTOAssembler.CreateVersions(DelegateService.vehicleIntegrationService.GetVehicleVersionsByMakeIdModelId(makeId, modelId));

        }

        public List<VehicleDTO> GetRisksVehicleByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleIntegrationService.GetRisksVehicleByInsuredId(insuredId));
        }

        public List<VehicleDTO> GetRisksVehicleByLicensePlate(string licensePlate)
        {
            return DTOAssembler.CreateVehicles(DelegateService.vehicleIntegrationService.GetRisksVehicleByLicensePlate(licensePlate));
        }

        public VehicleDTO GetRiskVehicleByRiskId(int riskId)
        {
            return DTOAssembler.CreateVehicle(DelegateService.vehicleIntegrationService.GetRiskVehicleByRiskId(riskId));
        }

        public List<SelectDTO> GetSelectRisksVehicleByLicensePlate(string licencePlate)
        {
            return DTOAssembler.CreateSelectVehicles(DelegateService.vehicleIntegrationService.GetSelectRisksVehicleByLicensePlate(licencePlate));
        }

        #endregion

        #region Transport

        public List<TransportDTO> GetTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            return DTOAssembler.CreateTransports(DelegateService.transportIntegrationService.GetTransportByEndorsementIdModuleType(endorsementId, moduleType));
        }

        public List<TransportDTO> GetTransportsByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateTransports(DelegateService.transportIntegrationService.GetTransportsByInsuredId(insuredId));
        }

        public TransportDTO GetRiskTransportByRiskId(int riskId)
        {
            return DTOAssembler.CreateTransport(DelegateService.transportIntegrationService.GetRiskTransportByRiskId(riskId));
        }

        #endregion

        #region Surety

        public List<SuretyDTO> GetSuretiesByEndorsementIdPrefixId(int endorsementId, int prefixId)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyIntegrationService.GetSuretiesByEndorsementIdPrefixIdModuleType(endorsementId, prefixId, ModuleType.Claim));
        }

        public List<SuretyDTO> GetRisksSuretyByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyIntegrationService.GetRisksSuretyByInsuredId(insuredId));
        }

        public List<SuretyDTO> GetRisksSuretyBySuretyIdPrefixId(int suretyId, int prefixId)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyIntegrationService.GetRisksSuretyBySuretyIdPrefixId(suretyId, prefixId));
        }

        public SuretyDTO GetSuretyByRiskIdPrefixId(int riskId, int prefixId)
        {
            return DTOAssembler.CreateSurety(DelegateService.suretyIntegrationService.GetSuretyByRiskIdPrefixIdModuleType(riskId, prefixId, ModuleType.Claim));
        }

        public List<SuretyDTO> GetRisksBySurety(string description)
        {
            return DTOAssembler.CreateSureties(DelegateService.suretyIntegrationService.GetRisksBySurety(description));
        }

        #endregion

        #region Fidelity

        public List<FidelityDTO> GetRiskFidelitiesByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateFidelities(DelegateService.fidelityIntegrationService.GetRiskFidelitiesByEndorsementId(endorsementId));
        }

        public List<FidelityDTO> GetRiskFidelitiesByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateFidelities(DelegateService.fidelityIntegrationService.GetRiskFidelitiesByInsuredId(insuredId));
        }

        public FidelityDTO GetRiskFidelityByRiskId(int riskId)
        {
            return DTOAssembler.CreateFidelity(DelegateService.fidelityIntegrationService.GetRiskFidelityByRiskId(riskId));
        }

        public List<SelectDTO> GetRiskCommercialClasses()
        {
            return DTOAssembler.CreateRiskCommercialClasses(DelegateService.underwritingIntegrationService.GetRiskCommercialClasses());
        }

        public List<SelectDTO> GetOccupations()
        {
            return DTOAssembler.CreateOccupations(DelegateService.fidelityIntegrationService.GetOccupations());
        }

        #endregion

        #region Aircraft

        public List<AirCraftDTO> GetRiskAirCraftsByInsuredId(int insuredId)
        {
            List<AirCraftDTO> airCraftsDTO = DTOAssembler.CreateAircrafts(DelegateService.aircraftIntegrationService.GetRiskAircraftsByInsuredId(insuredId));

            List<AircraftMakeDTO> makes = DelegateService.aircraftIntegrationService.GetAircraftMakes();
            List<AircraftTypeDTO> types = DelegateService.aircraftIntegrationService.GetAircraftTypes();
            List<AircraftUseDTO> uses = DelegateService.aircraftIntegrationService.GetAircraftUses();
            List<AircraftRegisterDTO> registers = DelegateService.aircraftIntegrationService.GetAircraftRegisters();
            List<AircraftOperatorDTO> operators = DelegateService.aircraftIntegrationService.GetAircraftOperators();

            foreach (AirCraftDTO airCraftDTO in airCraftsDTO)
            {
                if (airCraftDTO.MakeId > 0)
                {
                    airCraftDTO.Make = makes.First(x => x.Id == airCraftDTO.MakeId).Description;

                    if (airCraftDTO.ModelId > 0)
                    {
                        airCraftDTO.Model = DelegateService.aircraftIntegrationService.GetAircraftModelsByMakeId(Convert.ToInt32(airCraftDTO.MakeId)).First(x => x.Id == airCraftDTO.ModelId).Description;
                    }
                }

                if (airCraftDTO.TypeId > 0)
                {
                    airCraftDTO.Type = types.First(x => x.Id == airCraftDTO.TypeId).Description;
                }

                if (airCraftDTO.UseId > 0)
                {
                    airCraftDTO.Use = uses.First(x => x.Id == airCraftDTO.UseId).Description;
                }

                if (airCraftDTO.RegisterId > 0)
                {
                    airCraftDTO.Register = registers.First(x => x.Id == airCraftDTO.RegisterId).Description;
                }

                if (airCraftDTO.OperatorId > 0)
                {
                    airCraftDTO.Operator = operators.First(x => x.Id == airCraftDTO.OperatorId).Description;
                }
            }

            return airCraftsDTO;
        }

        public AirCraftDTO GetRiskAirCraftByRiskId(int riskId)
        {
            return DTOAssembler.CreateAircraft(DelegateService.aircraftIntegrationService.GetRiskAircraftByRiskId(riskId));
        }

        public List<AirCraftDTO> GetRiskAirCraftByEndorsementIdPrefixId(int endorsementId, int prefixId)
        {
            List<AirCraftDTO> airCraftsDTO = new List<AirCraftDTO>();
            switch (prefixId)
            {
                case 10:
                    airCraftsDTO = DTOAssembler.CreateAircrafts(DelegateService.aircraftIntegrationService.GetRiskAircraftsByEndorsementId(endorsementId));

                    List<AircraftMakeDTO> makes = DelegateService.aircraftIntegrationService.GetAircraftMakes();
                    List<AircraftTypeDTO> types = DelegateService.aircraftIntegrationService.GetAircraftTypes();
                    List<AircraftUseDTO> uses = DelegateService.aircraftIntegrationService.GetAircraftUses();
                    List<AircraftRegisterDTO> registers = DelegateService.aircraftIntegrationService.GetAircraftRegisters();
                    List<AircraftOperatorDTO> operators = DelegateService.aircraftIntegrationService.GetAircraftOperators();

                    foreach (AirCraftDTO airCraftDTO in airCraftsDTO)
                    {
                        if (airCraftDTO.MakeId > 0)
                        {
                            airCraftDTO.Make = makes.First(x => x.Id == airCraftDTO.MakeId).Description;

                            if (airCraftDTO.ModelId > 0)
                            {
                                airCraftDTO.Model = DelegateService.aircraftIntegrationService.GetAircraftModelsByMakeId(Convert.ToInt32(airCraftDTO.MakeId)).First(x => x.Id == airCraftDTO.ModelId).Description;
                            }
                        }

                        if (airCraftDTO.TypeId > 0)
                        {
                            airCraftDTO.Type = types.First(x => x.Id == airCraftDTO.TypeId).Description;
                        }

                        if (airCraftDTO.UseId > 0)
                        {
                            airCraftDTO.Use = uses.First(x => x.Id == airCraftDTO.UseId).Description;
                        }

                        if (airCraftDTO.RegisterId > 0)
                        {
                            airCraftDTO.Register = registers.First(x => x.Id == airCraftDTO.RegisterId).Description;
                        }

                        if (airCraftDTO.OperatorId > 0)
                        {
                            airCraftDTO.Operator = operators.First(x => x.Id == airCraftDTO.OperatorId).Description;
                        }
                    }
                    break;
                case 6:
                    airCraftsDTO = DTOAssembler.CreateMarines(DelegateService.marineIntegrationService.GetMarinesByEndorsementIdModuleType(endorsementId));
                    break;
            }

            return airCraftsDTO;
        }

        public List<SelectDTO> GetAirCraftMakes()
        {
            return DTOAssembler.CreateAircraftMakes(DelegateService.aircraftIntegrationService.GetAircraftMakes());
        }

        public List<SelectDTO> GetAirCraftModelsByMakeId(int makeId)
        {
            return DTOAssembler.CreateAirCraftModels(DelegateService.aircraftIntegrationService.GetAircraftModelsByMakeId(makeId));
        }

        public List<SelectDTO> GetAirCraftUses()
        {
            return DTOAssembler.CreateAircraftUses(DelegateService.aircraftIntegrationService.GetAircraftUses());
        }

        public List<SelectDTO> GetAircraftRegisters()
        {
            return DTOAssembler.CreateAircraftRegisters(DelegateService.aircraftIntegrationService.GetAircraftRegisters());
        }

        public List<SelectDTO> GetAircraftOperators()
        {
            return DTOAssembler.CreateAircraftOperators(DelegateService.aircraftIntegrationService.GetAircraftOperators());
        }

        #endregion

        #region Location
        public List<RiskLocationDTO> GetRiskPropertiesByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateLocations(DelegateService.propertyIntegrationService.GetRiskPropertiesByEndorsementId(endorsementId));
        }

        public List<RiskLocationDTO> GetRiskPropertiesByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateLocations(DelegateService.propertyIntegrationService.GetRiskPropertiesByInsuredId(insuredId));
        }

        public RiskLocationDTO GetRiskPropertyByRiskId(int riskId)
        {
            return DTOAssembler.CreateLocation(DelegateService.propertyIntegrationService.GetRiskPropertyByRiskId(riskId));
        }

        public List<RiskLocationDTO> GetRiskPropertiesByAddress(string address)
        {
            return DTOAssembler.CreateLocations(DelegateService.propertyIntegrationService.GetRiskPropertiesByAddress(address));
        }

        #endregion Location

        public List<ReasonDTO> GetReasonsByPrefixId(int prefixId)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateReasons(estimationTypeDAO.GetReasonsByPrefixId(prefixId));
        }

        #region ValidateAuthorizationPolicies

        private List<PoliciesAut> ValidateAuthorizationPolicies(Claim claim)
        {            
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, claim.Modifications.Last().UserId);

            EntityAssembler.CreateFacadeClaim(facade, claim);
            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM)), 1, facade, FacadeType.RULE_FACADE_CLAIM));

            foreach (ClaimCoverage claimCoverage in claim.Modifications.Last().Coverages)
            {
                if (claimCoverage.IsInsured)
                {
                    claimCoverage.ThirdAffected = ModelAssembler.CreateThirdAffectedByInsured(DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(claimCoverage.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());
                }                                              

                foreach (Estimation estimation in claimCoverage.Estimations)
                {
                    EntityAssembler.CreateFacadeEstimation(facade, claimCoverage, estimation);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }


        private List<PoliciesAut> ValidateAuthorizationPolicies(NoticeVehicle noticeVehicle)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, noticeVehicle.Notice.UserId);

            EntityAssembler.CreateFacadeNotice(facade, noticeVehicle.Notice);
            EntityAssembler.CreateFacadeNoticeVehicle(facade, noticeVehicle);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_CLAIM_NOTICE));

            if (noticeVehicle.Notice.NoticeCoverages.Any())
            {
                foreach (NoticeCoverage noticeCoverage in noticeVehicle.Notice.NoticeCoverages)
                {
                    EntityAssembler.CreateFacadeNoticeCoverage(facade, noticeCoverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }

        private List<PoliciesAut> ValidateAuthorizationPolicies(NoticeLocation noticeLocation)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, noticeLocation.Notice.UserId);

            EntityAssembler.CreateFacadeNotice(facade, noticeLocation.Notice);
            EntityAssembler.CreateFacadeNoticeLocation(facade, noticeLocation);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_CLAIM_NOTICE));

            if (noticeLocation.Notice.NoticeCoverages.Any())
            {
                foreach (NoticeCoverage noticeCoverage in noticeLocation.Notice.NoticeCoverages)
                {
                    EntityAssembler.CreateFacadeNoticeCoverage(facade, noticeCoverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }

        private List<PoliciesAut> ValidateAuthorizationPolicies(NoticeFidelity noticeFidelity)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, noticeFidelity.Notice.UserId);

            EntityAssembler.CreateFacadeNotice(facade, noticeFidelity.Notice);
            EntityAssembler.CreateFacadeNoticeFidelity(facade, noticeFidelity);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_CLAIM_NOTICE));

            if (noticeFidelity.Notice.NoticeCoverages.Any())
            {
                foreach (NoticeCoverage noticeCoverage in noticeFidelity.Notice.NoticeCoverages)
                {
                    EntityAssembler.CreateFacadeNoticeCoverage(facade, noticeCoverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }

        private List<PoliciesAut> ValidateAuthorizationPolicies(NoticeTransport noticeTransport)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, noticeTransport.Notice.UserId);

            EntityAssembler.CreateFacadeNotice(facade, noticeTransport.Notice);
            EntityAssembler.CreateFacadeNoticeTransport(facade, noticeTransport);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_CLAIM_NOTICE));

            if (noticeTransport.Notice.NoticeCoverages.Any())
            {
                foreach (NoticeCoverage noticeCoverage in noticeTransport.Notice.NoticeCoverages)
                {
                    EntityAssembler.CreateFacadeNoticeCoverage(facade, noticeCoverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }

        private List<PoliciesAut> ValidateAuthorizationPolicies(NoticeAirCraft noticeAirCraft)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, noticeAirCraft.Notice.UserId);

            EntityAssembler.CreateFacadeNotice(facade, noticeAirCraft.Notice);
            EntityAssembler.CreateFacadeNoticeAirCraft(facade, noticeAirCraft);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_CLAIM_NOTICE));

            if (noticeAirCraft.Notice.NoticeCoverages.Any())
            {
                foreach (NoticeCoverage noticeCoverage in noticeAirCraft.Notice.NoticeCoverages)
                {
                    EntityAssembler.CreateFacadeNoticeCoverage(facade, noticeCoverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }

        private List<PoliciesAut> ValidateAuthorizationPolicies(NoticeSurety noticeSurety)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            facade.SetConcept(RuleConceptPolicies.UserId, noticeSurety.Notice.UserId);

            EntityAssembler.CreateFacadeNotice(facade, noticeSurety.Notice);
            EntityAssembler.CreateFacadeNoticeSurety(facade, noticeSurety);

            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_CLAIM_NOTICE));

            if (noticeSurety.Notice.NoticeCoverages.Any())
            {
                foreach (NoticeCoverage noticeCoverage in noticeSurety.Notice.NoticeCoverages)
                {
                    EntityAssembler.CreateFacadeNoticeCoverage(facade, noticeCoverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_PACKAGE_CLAIM_NOTICE)), 1, facade, FacadeType.RULE_FACADE_ESTIMATION));
                }
            }

            return policiesAuts;
        }

        private void CreateClaimNotificationByAuthorizationPolicies(Claim claim, bool? isClaimReserve = false)
        {
            DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Claim, claim.TemporalId.ToString(), null, claim.Id.ToString());
            var result = string.Empty;
            var notification = string.Empty;

            result += isClaimReserve == true ? "El cambio de reserva de la denuncia se ha generado con éxito </br>Número de denuncia " + claim.Number + ".</br>" : "La denuncia se ha generado con exito. </br>Número de denuncia " + claim.Number + ".</br>";

            notification = "Se genero" + (isClaimReserve == true ? " el cambio de reserva de" : "") + " la denuncia: " + claim.Number + " de la póliza: " + claim.Endorsement.PolicyNumber + ".";

            NotificationUser notificationUser = new NotificationUser
            {
                UserId = claim.Modifications.First().UserId,
                CreateDate = DateTime.Now,
                NotificationType = new NotificationType { Type = NotificationTypes.Claim },
                Message = notification,
                Parameters = new Dictionary<string, object>
                    {
                        { "SearchType", (int)NotificationTypes.Claim },
                        { "BranchId", claim.Branch.Id },
                        { "PrefixId", claim.Prefix.Id },
                        { "PolicyNumber", claim.Endorsement.PolicyNumber },
                        { "ClaimNumber", claim.Number },
                    }
            };

            UserPerson person = DelegateService.uniqueUserService.GetPersonByUserId(claim.Modifications.First().UserId);
            if (person != null && person.Emails.Any())
            {
                string strAddress = person.Emails.First().Description;

                EmailCriteria email = new EmailCriteria
                {
                    Addressed = new List<string> { strAddress },
                    Message = "<h3>Todas las politicas fueron autorizadas</h3>" + claim.Number,
                    Subject = "Politicas autorizadas - " + claim.Id
                };

                DelegateService.authorizationPoliciesService.SendEmail(email);
            }

            DelegateService.uniqueUserService.CreateNotification(notificationUser);
        }

        private void CreateClaimNoticeNotificationByAuthorizationPolicies(Notice notice)
        {
            DelegateService.authorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.ClaimNotice, notice.TemporalId.ToString(), null, notice.Id.ToString());
            var result = string.Empty;
            var notification = string.Empty;

            result += "El aviso se ha generado con exito. </br>Número de aviso " + notice.Number + ".</br>";

            notification = "Se genero el aviso: " + notice.Number + ".";

            NotificationUser notificationUser = new NotificationUser
            {
                UserId = notice.UserId,
                CreateDate = DateTime.Now,
                NotificationType = new NotificationType { Type = NotificationTypes.ClaimNotice },
                Message = notification,
                Parameters = new Dictionary<string, object>
                    {
                        { "SearchType", (int)NotificationTypes.ClaimNotice},
                        { "PrefixId", notice.Policy.PrefixId },
                        { "NoticeNumber", notice.Number },
                    }
            };

            UserPerson person = DelegateService.uniqueUserService.GetPersonByUserId(notice.UserId);
            if (person != null && person.Emails.Any())
            {
                string strAddress = person.Emails[0].Description;

                EmailCriteria email = new EmailCriteria
                {
                    Addressed = new List<string> { strAddress },
                    Message = "<h3>Todas las politicas fueron autorizadas</h3>" + notice.Number,
                    Subject = "Politicas autorizadas - " + notice.Id
                };

                DelegateService.authorizationPoliciesService.SendEmail(email);
            }

            DelegateService.uniqueUserService.CreateNotification(notificationUser);
        }
        #endregion


        public List<CoInsuranceAssignedDTO> GetCoInsuranceByPolicyIdByEndorsementId(int endorsementId, int policyId)
        {
            return DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingIntegrationService.GetCoInsuranceByPolicyIdByEndorsementId(endorsementId, policyId));
        }

        public List<AmountTypeDTO> GetAmountType()
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            return DTOAssembler.CreateAmountTypes(estimationTypeDAO.GetAmountType());
        }

        public MinimumSalaryDTO GetMinimumSalaryByYear(int year)
        {
            EstimationTypeDAO estimationTypeDAO = new EstimationTypeDAO();
            MinimumSalaryDTO minimumSalaryDTO = DTOAssembler.CreateMinimumSalary(estimationTypeDAO.GetMinimumSalaryByYear(year));

            if (minimumSalaryDTO == null)
            {
                throw new BusinessException(string.Format(Resources.Resources.SMMLVNotParameterizedForTheYear, year));
            }

            return minimumSalaryDTO;
        }

        public bool GetJudicialDecisionDateIsActiveByPrefixId(int prefixId)
        {
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("IsDateActiveForWhichPrefixes?");
            List<string> Prefixes = parameter.TextParameter.Split(';').ToList();

            return Prefixes.Any(x => x == prefixId.ToString());
        }

        public List<IndividualDTO> GetPaymentBeneficiariesByDescription(string description)
        {
            List<IndividualDTO> beneficiaries = new List<IndividualDTO>();
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            ParticipantDAO participantDAO = new ParticipantDAO();

            beneficiaries.AddRange(DTOAssembler.CreateBeneficiariesByInsureds(DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual)));
            beneficiaries.AddRange(DTOAssembler.CreateBeneficiariesBySuppliers(claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual)));
            beneficiaries.AddRange(DTOAssembler.CreateBeneficiariesByParticipants(participantDAO.GetParticipantsByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual)));

            return beneficiaries.DistinctBy(x => x.IndividualId).ToList();
        }

        public IndividualDTO GetPaymentBeneficiaryByBeneficiaryId(int beneficiaryId)
        {
            IndividualDTO beneficiary = new IndividualDTO();
            ClaimSupplierDAO claimSupplierDAO = new ClaimSupplierDAO();
            ParticipantDAO participantDAO = new ParticipantDAO();

            beneficiary = DTOAssembler.CreateBeneficiaryByInsured(DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(beneficiaryId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());

            if (beneficiary != null)
            {
                return beneficiary;
            }
            else
            {
                beneficiary = DTOAssembler.CreateBeneficiaryBySupplier(claimSupplierDAO.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(beneficiaryId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());

                if (beneficiary != null)
                {
                    return beneficiary;
                }
                else
                {
                    beneficiary = DTOAssembler.CreateBeneficiaryByParticipant(participantDAO.GetParticipantsByDescriptionInsuredSearchTypeCustomerType(beneficiaryId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());
                }
            }

            return null;
        }

        public List<int> GetEstimationTypesSalariesEstimation()
        {
            try
            {
                return EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_ESTIMATION_TYPE_SALARIES).ToString().Split(',').Select(x => Convert.ToInt32(x)).ToList();
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }

        private async void ReinsuranceClaim(int claimId, int claimModifyId, int userId)
        {
            await UTILTASK.Task.Run(() =>
            {
                DelegateService.claimsReinsuranceWorkerIntegrationServices.ReinsuranceClaim(claimId, claimModifyId, userId);
            });
        }
    }
}