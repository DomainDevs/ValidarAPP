using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Application.ClaimServices.DTOs.Recovery;
using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using Sistran.Core.Services.UtilitiesServices.Enums;
using UPV1MO = Sistran.Core.Application.UniquePersonService.V1.Models;
using VHMO = Sistran.Core.Integration.VehicleServices.DTOs;
using TAXMO = Sistran.Core.Application.TaxServices.DTOs;
using UUMO = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using COMMENUM = Sistran.Core.Application.CommonService.Enums;
using UNDMO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using FDMO = Sistran.Core.Integration.FidelityServices.DTOs;
using AIRMO = Sistran.Core.Integration.AircraftServices.DTOs;
using TRMO = Sistran.Core.Integration.TransportServices.DTOs;
using SRMO = Sistran.Core.Integration.SuretyServices.DTOs;
using MRMO = Sistran.Core.Integration.MarineServices.DTOs;
using PPMO = Sistran.Core.Integration.PropertyServices.DTOs;
using CLMWOR = Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;
using Sistran.Core.Application.ClaimServices.EEProvider.Helpers;
using GLWKDTO = Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        #region Claims

        internal static ThirdPartyDTO CreateThirdParty(UPV1MO.ThirdPerson thirdPerson)
        {
            if (thirdPerson == null)
                return null;

            return new ThirdPartyDTO
            {
                Id = thirdPerson.Id,
                IndividualId = thirdPerson.IndividualId,
                FullName = thirdPerson.Fullname,
                DocumentNumber = thirdPerson.DocumentNumber,
                DocumentTypeId = thirdPerson.DocumentTypeId
            };
        }

        internal static List<ThirdPartyDTO> CreateThirdsParty(List<UPV1MO.ThirdPerson> thirdsPerson)
        {
            List<ThirdPartyDTO> thirdsParty = new List<ThirdPartyDTO>();

            foreach (UPV1MO.ThirdPerson thirdPerson in thirdsPerson)
            {
                thirdsParty.Add(CreateThirdParty(thirdPerson));
            }

            return thirdsParty;
        }

        internal static InsuredDTO CreateInsured(UNDMO.InsuredDTO insured)
        {
            if (insured == null)
                return null;
            
            return new InsuredDTO
            {
                Id = insured.Id,
                IndividualId = insured.IndividualId,
                DocumentNumber = insured.DocumentNumber,
                FullName = insured.FullName,
                DocumentTypeId = insured.DocumentTypeId
            };
        }

        internal static List<InsuredDTO> CreateInsureds(List<UNDMO.InsuredDTO> insureds)
        {
            List<InsuredDTO> insuredDTOs = new List<InsuredDTO>();

            foreach (UNDMO.InsuredDTO insured in insureds)
            {
                insuredDTOs.Add(CreateInsured(insured));
            }

            return insuredDTOs;
        }

        internal static IndividualDTO CreateBeneficiaryByInsured(UNDMO.InsuredDTO insured)
        {
            if (insured == null)
                return null;

            return new IndividualDTO
            {
                IndividualId = insured.IndividualId,
                DocumentNumber = insured.DocumentNumber,
                DocumentType = insured.DocumentTypeId,
                FullName = insured.FullName
            };
        }

        internal static List<IndividualDTO> CreateBeneficiariesByInsureds(List<UNDMO.InsuredDTO> insureds)
        {
            List<IndividualDTO> beneficiaries = new List<IndividualDTO>();

            foreach (UNDMO.InsuredDTO insured in insureds)
            {
                beneficiaries.Add(CreateBeneficiaryByInsured(insured));
            }

            return beneficiaries;
        }

        internal static IndividualDTO CreateBeneficiaryBySupplier(ClaimSupplier supplier)
        {
            if (supplier == null)
            {
                return null;
            }

            return new IndividualDTO
            {
                IndividualId = supplier.IndividualId,
                FullName = supplier.FullName,
                DocumentNumber = supplier.DocumentNumber,
                DocumentType = supplier.DocumentTypeId
            };
        }

        internal static List<IndividualDTO> CreateBeneficiariesBySuppliers(List<ClaimSupplier> suppliers)
        {
            List<IndividualDTO> beneficiaries = new List<IndividualDTO>();

            foreach (ClaimSupplier supplier in suppliers)
            {
                beneficiaries.Add(CreateBeneficiaryBySupplier(supplier));
            }

            return beneficiaries;
        }

        internal static IndividualDTO CreateBeneficiaryByParticipant(Participant participant)
        {
            if (participant == null)
            {
                return null;
            }

            return new IndividualDTO
            {
                IndividualId = participant.Id,
                DocumentNumber = participant.DocumentNumber,
                FullName = participant.Fullname,
                DocumentType = Convert.ToInt32(participant.DocumentTypeId)
            };
        }

        internal static List<IndividualDTO> CreateBeneficiariesByParticipants(List<Participant> participants)
        {
            List<IndividualDTO> beneficiaries = new List<IndividualDTO>();

            foreach (Participant participant in participants)
            {
                beneficiaries.Add(CreateBeneficiaryByParticipant(participant));
            }

            return beneficiaries;
        }

        internal static CauseDTO CreateCause(Cause cause)
        {
            return new CauseDTO
            {
                Id = cause.Id,
                Description = cause.Description,
                PrefixId = cause.Prefix.Id,
                PrefixDescription = cause.Prefix.Description,
                IsDriverInformationRequired = cause.DriverInformationRequired,
                IsDriverInformationRequiredDescription = cause.DriverInformationRequired ? Resources.Resources.Yes : Resources.Resources.Not,
                IsPoliceComplaintRequired = cause.PoliceComplaintRequired,
                IsPoliceComplaintRequiredDescription = cause.PoliceComplaintRequired ? Resources.Resources.Yes : Resources.Resources.Not,
                IsInspectionDateRequired = cause.InspectionDateRequired,
                IsInspectionDateRequiredDescription = cause.InspectionDateRequired ? Resources.Resources.Yes : Resources.Resources.Not
            };
        }

        internal static List<CauseDTO> CreateCauses(List<Cause> causes)
        {
            List<CauseDTO> causesDTO = new List<CauseDTO>();

            foreach (Cause cause in causes)
            {
                causesDTO.Add(CreateCause(cause));
            }

            return causesDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SelectDTO CreateEstimationTypeByPrefixId(EstimationType estimationType)
        {
            return new SelectDTO
            {
                Id = estimationType.Id,
                Description = estimationType.Description
            };
        }

        internal static List<SelectDTO> CreateEstimationTypeByPrefixId(List<EstimationType> estimationTypes)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (EstimationType estimationType in estimationTypes)
            {
                selectDTO.Add(CreateEstimationTypeByPrefixId(estimationType));
            }

            return selectDTO.OrderBy(x => x.Description).ToList();
        }

        internal static EstimationDTO CreateEstimationByPrefixId(EstimationType estimation)
        {
            return new EstimationDTO
            {
                Id = estimation.Id,
                Description = estimation.Description,
                StatusCode = "",
                StatusCodeId = 0,
                Reason = "",
                ReasonId = 0,
                CurrencyReason = "",
                CurrencyReasonId = 0,
                EstimateAmount = 0,
                EstimateAmountAccumulate = 0,
                Deductible = 0,
                DeductibleAmount = 0,
                Payments = 0,
                PendingReservation = 0
            };
        }

        internal static List<EstimationDTO> CreateEstimationsByPrefixId(List<EstimationType> estimations)
        {
            List<EstimationDTO> estimationDTO = new List<EstimationDTO>();

            foreach (EstimationType estimation in estimations)
            {
                estimationDTO.Add(CreateEstimationByPrefixId(estimation));
            }

            return estimationDTO;
        }

        internal static CoverageDTO CreateCoverage(CauseCoverage causeCoverage)
        {
            return new CoverageDTO
            {
                Id = causeCoverage.Id,
                Description = causeCoverage.Description
            };
        }

        internal static SelectDTO CreateBranchUser(Branch branch)
        {
            return new SelectDTO
            {
                Id = branch.Id,
                Description = branch.Description
            };
        }

        internal static List<SelectDTO> CreateBranchesUser(List<Branch> branches)
        {
            List<SelectDTO> listSelectDTO = new List<SelectDTO>();

            foreach (Branch branch in branches)
            {
                listSelectDTO.Add(CreateBranchUser(branch));
            }
            return listSelectDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SelectDTO CreateCurrency(Currency currency)
        {
            return new SelectDTO
            {
                Id = currency.Id,
                Description = currency.Description
            };
        }

        internal static List<SelectDTO> CreateCurries(List<Currency> currencies)
        {
            List<SelectDTO> listSelectDTO = new List<SelectDTO>();

            foreach (Currency currency in currencies)
            {
                listSelectDTO.Add(CreateCurrency(currency));
            }
            return listSelectDTO;
        }

        internal static List<CoverageDTO> CreateCoverages(List<CauseCoverage> causeCoverages)
        {
            List<CoverageDTO> coveragesDTO = new List<CoverageDTO>();

            foreach (CauseCoverage causeCoverage in causeCoverages)
            {
                coveragesDTO.Add(CreateCoverage(causeCoverage));
            }

            return coveragesDTO.OrderBy(x => x.Description).ToList();
        }

        internal static EstimationTypeDTO CreateCauseCoverage(EstimationType estimationType)
        {
            return new EstimationTypeDTO
            {
                Id = estimationType.Id,
                Description = estimationType.Description,
                Enabled = estimationType.IsEnabled
            };
        }

        internal static StatusDTO CreateStatus(Status status)
        {
            return new StatusDTO
            {
                Id = status.Id,
                Description = status.Description,
                Enabled = status.IsEnabled,
                InternalStatus = status.InternalStatus == null ? new InternalStatusDTO() : CreateInternalStatus(status.InternalStatus),
            };
        }

        internal static List<InternalStatusDTO> CreateInternalStatuses(List<InternalStatus> internalStatuses)
        {
            List<InternalStatusDTO> internalStatusDTOs = new List<InternalStatusDTO>();
            foreach (InternalStatus internalStatus in internalStatuses)
            {
                internalStatusDTOs.Add(CreateInternalStatus(internalStatus));
            }
            return internalStatusDTOs;
        }

        internal static InternalStatusDTO CreateInternalStatus(InternalStatus internalStatus)
        {
            return new InternalStatusDTO
            {
                Id = internalStatus.Id,
                Description = internalStatus.Description
            };
        }

        internal static List<StatusDTO> CreateStatuses(List<Status> status)
        {
            List<StatusDTO> statusDTOs = new List<StatusDTO>();

            foreach (Status statusModel in status)
            {
                statusDTOs.Add(CreateStatus(statusModel));
            }

            return statusDTOs.OrderBy(x => x.Description).ToList();
        }

        internal static List<ReasonDTO> CreateReasons(List<Reason> reason)
        {
            List<ReasonDTO> reasonDTOs = new List<ReasonDTO>();

            foreach (Reason reasonModel in reason)
            {
                reasonDTOs.Add(CreateReason(reasonModel));
            }

            return reasonDTOs.OrderBy(x => x.Description).ToList();
        }

        internal static ReasonDTO CreateReason(Reason reason)
        {
            return new ReasonDTO
            {
                Id = reason.Id,
                Description = reason.Description
            };
        }

        internal static EstimationTypeDTO CreateEstimationType(EstimationType estimationType)
        {
            return new EstimationTypeDTO
            {
                Id = estimationType.Id,
                Description = estimationType.Description,
                Enabled = Convert.ToBoolean(estimationType.IsEnabled),
                ShowSummary = Convert.ToBoolean(estimationType.ShowSummary)
            };
        }

        internal static List<EstimationTypeDTO> CreateEstimationTypes(List<EstimationType> estimationTypes)
        {
            List<EstimationTypeDTO> estimationTypesDTO = new List<EstimationTypeDTO>();
            foreach (EstimationType estimationType in estimationTypes)
            {
                estimationTypesDTO.Add(CreateEstimationType(estimationType));
            }

            return estimationTypesDTO.ToList();
        }

        internal static PrefixDTO CreatePrefix(Prefix prefix)
        {
            return new PrefixDTO
            {
                Id = prefix.Id,
                Description = prefix.Description
            };
        }

        internal static List<PrefixDTO> CreatePrefixes(List<Prefix> prefixes)
        {
            List<PrefixDTO> prefixesDTO = new List<PrefixDTO>();

            foreach (Prefix prefix in prefixes)
            {
                prefixesDTO.Add(CreatePrefix(prefix));
            }

            return prefixesDTO.OrderBy(x => x.Description).ToList();
        }

        internal static LineBusinessDTO CreateLineBusiness(LineBusiness lineBusiness)
        {
            return new LineBusinessDTO
            {
                Id = lineBusiness.Id,
                Description = lineBusiness.Description
            };
        }

        internal static List<LineBusinessDTO> CreateLinesBusiness(List<LineBusiness> linesBusiness)
        {
            List<LineBusinessDTO> linesBusinessDTO = new List<LineBusinessDTO>();

            foreach (LineBusiness lineBusiness in linesBusiness)
            {
                linesBusinessDTO.Add(CreateLineBusiness(lineBusiness));
            }

            return linesBusinessDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SubLineBusinessDTO CreateSubLineBusiness(SubLineBusiness subLineBusiness)
        {
            return new SubLineBusinessDTO
            {
                Id = subLineBusiness.Id,
                Description = subLineBusiness.Description
            };
        }

        internal static List<SubLineBusinessDTO> CreateSubLinesBusiness(List<SubLineBusiness> subLinesBusiness)
        {
            List<SubLineBusinessDTO> subLinesBusinessDTO = new List<SubLineBusinessDTO>();

            foreach (SubLineBusiness subLineBusiness in subLinesBusiness)
            {
                subLinesBusinessDTO.Add(CreateSubLineBusiness(subLineBusiness));
            }

            return subLinesBusinessDTO.OrderBy(x => x.Description).ToList();
        }

        internal static CoverageDTO CreateCoverage(UNDMO.CoverageDTO coverageDTO)
        {
            return new CoverageDTO
            {
                Id = coverageDTO.Id,
                Description = coverageDTO.Description,
                InsuredAmountTotal = coverageDTO.InsuredAmountTotal,
                InsurableAmount = coverageDTO.InsurableAmount,
                OcurrencyLimit = coverageDTO.LimitOccurrenceAmount,
                PersonLimit = coverageDTO.PersonLimit,
                AggregateLimit = coverageDTO.AggregateLimit,
                CoverageId = coverageDTO.Id,
                RiskCoverageId = coverageDTO.RiskCoverageId,
                SubLineBusinessCode = coverageDTO.SubLineBusinessCode,
                LineBusinessCode = coverageDTO.LineBusinessCode,
                SubLineBusinessDescription = coverageDTO.SubLineBusinessDescription,
                InsuredObjectId = coverageDTO.InsuredObjectId,
                InsuredObjectDescription = coverageDTO.InsuredObjectDescription,
                Number = coverageDTO.Number,
                LimitOccurrenceAmount = coverageDTO.LimitOccurrenceAmount
            };
        }

        internal static List<CoverageDTO> CreateCoverages(List<UNDMO.CoverageDTO> coveragesDTO)
        {
            List<CoverageDTO> coverages = new List<CoverageDTO>();

            foreach (UNDMO.CoverageDTO coverageDTO in coveragesDTO)
            {
                coverages.Add(CreateCoverage(coverageDTO));
            }

            return coverages.OrderBy(x => x.Description).ToList();
        }

        public static List<ClaimDTO> CreateClaims(List<Claim> claims)
        {
            List<ClaimDTO> claimDTOs = new List<ClaimDTO>();
            foreach (Claim item in claims)
            {
                claimDTOs.Add(CreateClaim(item));
            }

            return claimDTOs;
        }

        internal static ClaimDTO CreateClaim(Claim claim)
        {
            if (claim == null)
            {
                return null;
            }
            return new ClaimDTO
            {
                ClaimId = claim.Id,
                OccurrenceDate = claim.OccurrenceDate,
                Modifications = CreateClaimModifications(claim.Modifications),
                AuthorizationPolicies = claim.AuthorizationPolicies,
                EstimatedValue = 0,
                PaidValue = 0,
                Number = claim.Number,
                BranchId = claim.Branch.Id,
                BranchDescription = claim.Branch.Description,
                NoticeDate = claim.NoticeDate,
                PolicyId = Convert.ToInt32(claim.Endorsement.PolicyId),
                PrefixId = claim.Prefix.Id,
                PrefixDescription = claim.Prefix.Description,
                NoticeId = claim.NoticeId,
                RiskDescription = claim.RiskDescription,
                CoveredRiskType = (int)claim.CoveredRiskType,
                TemporalId = claim.TemporalId,
                CityId = Convert.ToInt32(claim.City?.Id),
                StateId = Convert.ToInt32(claim.City?.State?.Id),
                CountryId = Convert.ToInt32(claim.City?.State?.Country?.Id),
                Location = claim.Location,
                CauseId = Convert.ToInt32(claim.Cause?.Id),
                DamageResponsabilityId = Convert.ToInt32(claim.DamageResponsability?.Id),
                DamageTypeId = Convert.ToInt32(claim.DamageType?.Id),
                EndorsementId = claim.Endorsement.Id,
                PolicyDocumentNumber = claim.Endorsement.PolicyNumber,
                Description = claim.TextOperation?.Operation,
                IsTotalParticipation = claim.IsTotalParticipation,
                JudicialDecisionDate = claim.JudicialDecisionDate,
                BusinessTypeId = claim.BusinessTypeId,
                IndividualId = claim.IndividualId,
                NoticeInternalConsecutive = claim.NoticeInternalConsecutive
            };
        }

        public static List<SubClaimDTO> CreateClaimsToSubClaims(List<Claim> claims)
        {
            if (claims == null)
                return new List<SubClaimDTO>();
            List<SubClaimDTO> claimDTOs = new List<SubClaimDTO>();
            foreach (Claim item in claims)
            {
                claimDTOs.Add(CreateClaimToSubClaim(item));
            }

            return claimDTOs;
        }

        internal static SubClaimDTO CreateClaimToSubClaim(Claim claim)
        {
            if (claim == null)
            {
                return null;
            }
            return new SubClaimDTO
            {
                ClaimId = claim.Id,
                SubClaim = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().SubClaim,
                OccurrenceDate = claim.OccurrenceDate,
                JudicialDecisionDate = claim.JudicialDecisionDate,
                Number = claim.Number,
                BusinessTypeId = claim.BusinessTypeId,
                PolicyDocumentNumber = claim.Endorsement.PolicyNumber,
                PolicyId = claim.Endorsement.PolicyId,
                EndorsementId = claim.Endorsement.Id,
                BranchCode = claim.Branch.Id,
                PrefixId = claim.Prefix.Id,
                CreationDate = claim.Modifications.FirstOrDefault().RegistrationDate,
                CoverageId = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().CoverageId,
                RiskId = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().RiskId,
                RiskDescription = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().RiskDescription,
                EstimationDate = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().CreationDate,
                EstimateAmount = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Amount,
                EstimationTypeId = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Type.Id,
                EstimationType = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Type.Description,
                EstimationTypeStatusReasonId = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Reason.Id,
                EstimationTypeStatusId = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Reason.Status.Id,
                EstimationCurrencyId = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Currency.Id,
                Currency = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Estimations.FirstOrDefault().Currency.Description,
                Insured = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().IsInsured ? "ASEGURADO" : "TERCERO",
                CoverageDescription = claim.Modifications.FirstOrDefault().Coverages.FirstOrDefault().Description,
                TotalConcept = claim.Vouchers.FirstOrDefault().Concepts.Sum(x => x.Value),
                TotalRetention = claim.Vouchers.FirstOrDefault().Concepts.Sum(x => x.Retention),
                TotalTax = claim.Vouchers.FirstOrDefault().Concepts.Sum(x => x.VoucherConceptTaxes == null ? 0 : x.VoucherConceptTaxes.Sum(z => z.TaxValue + z.Retention)),

                Vouchers = CreateVouchers(claim.Vouchers)
            };
        }

        internal static List<ClaimModifyDTO> CreateClaimModifications(List<ClaimModify> claimModifications)
        {
            List<ClaimModifyDTO> claimModificationsDTO = new List<ClaimModifyDTO>();

            foreach (ClaimModify claimModification in claimModifications)
            {
                claimModificationsDTO.Add(CreateClaimModification(claimModification));
            }

            return claimModificationsDTO;
        }

        internal static ClaimModifyDTO CreateClaimModification(ClaimModify claimModification)
        {
            return new ClaimModifyDTO
            {
                ClaimModifyId = claimModification.Id,
                AccountingDate = claimModification.AccountingDate,
                RegistrationDate = claimModification.RegistrationDate,
                UserId = claimModification.UserId,
                ClaimCoverages = CreateClaimCoverages(claimModification.Coverages)
            };
        }

        internal static ClaimVehicleDTO CreateClaimVehicle(ClaimVehicle claimVehicle)
        {
            return new ClaimVehicleDTO
            {
                AuthorizationPolicies = claimVehicle.Claim.AuthorizationPolicies,
                /*RAMO*/
                PolicyDocumentNumber = claimVehicle.Claim.Endorsement.PolicyNumber,
                Number = claimVehicle.Claim.Number,
                ClaimId = claimVehicle.Claim.Id,
                CoveredRiskType = (int)claimVehicle.Claim.CoveredRiskType,
                TemporalId = claimVehicle.Claim.TemporalId
            };
        }

        internal static ClaimSuretyDTO CreateClaimSurety(ClaimSurety claimSurety)
        {
            return new ClaimSuretyDTO
            {
                AuthorizationPolicies = claimSurety.Claim.AuthorizationPolicies,
                Number = claimSurety.Claim.Number,
                ClaimId = claimSurety.Claim.Id,
                CoveredRiskType = (int)claimSurety.Claim.CoveredRiskType,
                TemporalId = claimSurety.Claim.TemporalId
            };

        }

        internal static ClaimCoverageDTO CreateClaimCoverage(ClaimCoverage claimCoverage)
        {

            return new ClaimCoverageDTO
            {
                ClaimedAmount = claimCoverage.ClaimedAmount,
                IsClaimedAmount = claimCoverage.IsClaimedAmount,
                SubClaim = claimCoverage.SubClaim,
                RiskId = claimCoverage.RiskId,
                RiskNum = claimCoverage.RiskNumber,
                CoverageId = claimCoverage.CoverageId,
                CoverageNum = claimCoverage.CoverageNumber,
                IsInsured = claimCoverage.IsInsured,
                IsProspect = claimCoverage.IsProspect,
                Description = claimCoverage.TextOperation?.Operation,
                Estimations = CreateEstimations(claimCoverage.Estimations)
            };
        }

        internal static List<EstimationDTO> CreateEstimations(List<Estimation> estimations)
        {
            List<EstimationDTO> estimationDTOs = new List<EstimationDTO>();

            foreach (Estimation estimation in estimations)
            {
                estimationDTOs.Add(createEstimation(estimation));
            }

            return estimationDTOs;
        }

        internal static EstimationDTO createEstimation(Estimation estimation)
        {
            return new EstimationDTO
            {
                EstimateAmount = estimation.Amount,
                EstimateAmountAccumulate = estimation.AmountAccumulate,
                DeductibleAmount = estimation.DeductibleAmount,
                CurrencyId = estimation.Currency.Id,
                CurrencyReason = estimation.Reason.Description,
                EstimationType = estimation.Type.Id,
                Payments = Convert.ToDecimal(estimation.PaymentAmount),
                ReasonId = estimation.Reason.Id,
                IsMinimumSalary = estimation.IsMinimumSalary,
                MinimumSalariesNumber = estimation.MinimumSalariesNumber
            };
        }

        internal static List<ClaimCoverageDTO> CreateClaimCoverages(List<ClaimCoverage> claimCoverages)
        {
            List<ClaimCoverageDTO> claimCoveragesDTO = new List<ClaimCoverageDTO>();

            foreach (ClaimCoverage claimCoverage in claimCoverages)
            {
                claimCoveragesDTO.Add(CreateClaimCoverage(claimCoverage));
            }

            return claimCoveragesDTO;
        }

        internal static List<PolicyDTO> CreatePolicies(List<UNDMO.PolicyDTO> policyDTOs)
        {
            List<PolicyDTO> policies = new List<PolicyDTO>();

            foreach (UNDMO.PolicyDTO policy in policyDTOs)
            {
                policies.Add(CreatePolicy(policy));
            }

            return policies;
        }

        internal static PolicyDTO CreatePolicy(UNDMO.PolicyDTO policyDTO)
        {
            return new PolicyDTO
            {
                Id = policyDTO.Id,
                DocumentNumber = policyDTO.DocumentNumber,
                EndorsementId = policyDTO.EndorsementId,
                BranchId = policyDTO.BranchId,
                BranchDescription = policyDTO.BranchDescription,
                PrefixId = policyDTO.PrefixId,
                PrefixDescription = policyDTO.PrefixDescription,
                CurrencyId = policyDTO.CurrencyId,
                CurrencyDescription = policyDTO.CurrencyDescription,
                HolderId = policyDTO.HolderId,
                HolderDocumentNumber = policyDTO.HolderDocumentNumber,
                HolderName = policyDTO.HolderName,
                BusinessType = policyDTO.BusinessType,
                BusinessTypeId = policyDTO.BusinessTypeId,
                ClaimsQuantity = "0",
                EndorsementType = policyDTO.EndorsementType,
                EndorsementTypeId = policyDTO.EndorsementTypeId,
                Inforce = 1,
                PendingDebt = "0",
                IssueDate = policyDTO.IssueDate,
                Premium = policyDTO.Premium,
                FullPremium = policyDTO.FullPremium,
                PolicyTypeId = policyDTO.PolicyTypeId,
                PolicyType = policyDTO.PolicyType,
                TaxExpenses = policyDTO.TaxExpenses,
                CurrentFrom = policyDTO.CurrentFrom,
                CurrentTo = policyDTO.CurrentTo,
                Agent = policyDTO.Agent,
                CoInsurance = CreateCoInsurances(policyDTO.CoInsurance),
                EndorsementDocumentNum = policyDTO.EndorsementDocumentNum,
                ProductId = policyDTO.ProductId,
                SalePointId = policyDTO.SalePointId
            };
        }

        internal static CoInsuranceDTO CreateCoInsurance(UNDMO.CoInsuranceDTO coInsurance)
        {
            return new CoInsuranceDTO
            {
                CompanyId = coInsurance.CompanyId,
                Company = coInsurance.Company,
                Participation = coInsurance.Participation,
                SumAssuredAmount = coInsurance.SumAssuredAmount,
                PremiumAmount = coInsurance.PremiumAmount,
                ParticipationOwn = coInsurance.ParticipationOwn
            };
        }

        internal static List<CoInsuranceDTO> CreateCoInsurances(List<PaymentRequestCoInsurance> coInsurances)
        {
            List<CoInsuranceDTO> coInsurancesDTO = new List<CoInsuranceDTO>();
            if (coInsurances != null)
            {
                foreach (PaymentRequestCoInsurance paymentRequestCoInsurance in coInsurances)
                {
                    coInsurancesDTO.Add(CreateCoInsurance(paymentRequestCoInsurance));
                }
            }

            return coInsurancesDTO;
        }

        internal static CoInsuranceDTO CreateCoInsurance(PaymentRequestCoInsurance coInsurance)
        {
            return new CoInsuranceDTO
            {
                CompanyId = coInsurance.CompanyId,
                Participation = coInsurance.PartCiaPct
            };
        }

        internal static List<CoInsuranceDTO> CreateCoInsurances(List<UNDMO.CoInsuranceDTO> coInsurances)
        {
            List<CoInsuranceDTO> coInsurancesDTO = new List<CoInsuranceDTO>();
            foreach (UNDMO.CoInsuranceDTO issuanceCoInsuranceCompany in coInsurances)
            {
                coInsurancesDTO.Add(CreateCoInsurance(issuanceCoInsuranceCompany));
            }

            return coInsurancesDTO;
        }

        internal static EndorsementDTO CreateEndorsement(ClaimEndorsement endorsement)
        {
            if (endorsement == null)
            {
                return null;
            }

            return new EndorsementDTO()
            {
                Id = endorsement.Id,
                Description = endorsement.Number.ToString()
            };
        }

        internal static List<EndorsementDTO> CreateEndorsements(List<ClaimEndorsement> endorsements)
        {
            List<EndorsementDTO> endorsementsDTO = new List<EndorsementDTO>();

            foreach (ClaimEndorsement endorsement in endorsements)
            {
                endorsementsDTO.Add(CreateEndorsement(endorsement));
            }
            return endorsementsDTO;
        }


        internal static ClaimCoverageActivePanelDTO CreateCoverageActionPanel(ClaimCoverageActivePanel claimCoverageActivePanel)
        {
            if (claimCoverageActivePanel == null)
            {
                return null;
            }
            return new ClaimCoverageActivePanelDTO
            {
                Id = claimCoverageActivePanel.CoverageId,
                IsEnabledDriver = claimCoverageActivePanel.EnabledDriver,
                IsEnabledThird = claimCoverageActivePanel.EnabledThird,
                IsEnabledThirdPartyVehicle = claimCoverageActivePanel.EnabledThirdPartyVehicle,
                IsEnabledAffectedProperty = claimCoverageActivePanel.EnabledAffectedProperty
            };
        }

        internal static SupplierDTO CreateSupplier(ClaimSupplier supplier)
        {
            return new SupplierDTO
            {
                Id = supplier.Id,
                IndividualId = supplier.IndividualId,
                Name = supplier.FullName,
                DocumentTypeId = supplier.DocumentTypeId,
                DocumentNumber = supplier.DocumentNumber,
                EconomicActivityId = supplier.EconomicActivityId
            };
        }

        internal static List<SupplierDTO> CreateSuppliers(List<ClaimSupplier> suppliers)
        {
            List<SupplierDTO> suppliersDTO = new List<SupplierDTO>();

            foreach (ClaimSupplier supplier in suppliers)
            {
                suppliersDTO.Add(CreateSupplier(supplier));
            }

            return suppliersDTO;
        }

        internal static List<SelectDTO> CreateCoveredRiskTypes()
        {
            List<SelectDTO> selectDTOs = new List<SelectDTO>();
            foreach (CoveredRiskType item in Enum.GetValues(typeof(CoveredRiskType)))
            {
                selectDTOs.Add(new SelectDTO
                {
                    Id = (int)item,
                    Description = Resources.Resources.ResourceManager.GetString("List" + item.ToString())
                });
            }
            return selectDTOs;
        }

        internal static BranchDTO CreateBranch(Branch branch)
        {
            return new BranchDTO
            {
                Id = branch.Id,
                Description = branch.Description
            };
        }

        internal static List<BranchDTO> CreateBranches(List<Branch> branches)
        {
            List<BranchDTO> listBranchDTO = new List<BranchDTO>();

            foreach (Branch branch in branches)
            {
                listBranchDTO.Add(CreateBranch(branch));
            }

            return listBranchDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SelectDTO CreateCountry(Country country)
        {
            return new SelectDTO
            {
                Id = country.Id,
                Description = country.Description
            };
        }

        internal static List<SelectDTO> CreateCountries(List<Country> countries)
        {
            List<SelectDTO> listSelectDTO = new List<SelectDTO>();

            foreach (Country country in countries)
            {
                listSelectDTO.Add(CreateCountry(country));
            }

            return listSelectDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SelectDTO CreateState(State state)
        {
            return new SelectDTO
            {
                Id = state.Id,
                Description = state.Description
            };
        }

        internal static List<SelectDTO> CreateStates(List<State> states)
        {
            List<SelectDTO> listSelectDTO = new List<SelectDTO>();

            foreach (State state in states)
            {
                listSelectDTO.Add(CreateState(state));
            }
            return listSelectDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SelectDTO CreateCity(City city)
        {
            return new SelectDTO
            {
                Id = city.Id,
                Description = city.Description
            };
        }

        internal static List<SelectDTO> CreateCities(List<City> cities)
        {
            List<SelectDTO> listSelectDTO = new List<SelectDTO>();

            foreach (City city in cities)
            {
                listSelectDTO.Add(CreateCity(city));
            }
            return listSelectDTO.OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        /// Met�do que trae un tipo de notificaci�n
        /// </summary>
        /// <returns>NoticeTypeDTO</returns>
        internal static NoticeTypeDTO CreateNoticeType(NoticeType noticeType)
        {
            return new NoticeTypeDTO
            {
                Id = noticeType.Id,
                Description = noticeType.Description
            };
        }

        internal static List<NoticeTypeDTO> CreateNoticeTypes(List<NoticeType> noticeTypes)
        {
            List<NoticeTypeDTO> noticeTypesDTO = new List<NoticeTypeDTO>();

            foreach (NoticeType noticeType in noticeTypes)
            {
                noticeTypesDTO.Add(CreateNoticeType(noticeType));
            }

            return noticeTypesDTO;
        }

        internal static IndividualDTO CreateIndividual(UPV1MO.Person person)
        {
            return new IndividualDTO
            {
                IndividualId = person.IndividualId,
                FullName = person.FullName
            };
        }

        internal static List<IndividualDTO> CreateIndividuals(List<UPV1MO.Person> persons)
        {
            List<IndividualDTO> individualDTO = new List<IndividualDTO>();

            foreach (UPV1MO.Person person in persons)
            {
                individualDTO.Add(CreateIndividual(person));
            }

            return individualDTO;
        }

        internal static IndividualDTO CreateIndividual(UNDMO.InsuredDTO insuredDTO)
        {
            return new IndividualDTO
            {
                IndividualId = insuredDTO.IndividualId,
                DocumentNumber = insuredDTO.DocumentNumber,
                FullName = insuredDTO.FullName,
            };
        }

        internal static List<IndividualDTO> CreateIndividuals(List<UNDMO.InsuredDTO> insuredsDTO)
        {
            List<IndividualDTO> individualDTO = new List<IndividualDTO>();

            foreach (UNDMO.InsuredDTO insuredDTO in insuredsDTO)
            {
                individualDTO.Add(CreateIndividual(insuredDTO));
            }

            return individualDTO;
        }

        internal static SelectDTO CreateDamageType(DamageType damageType)
        {
            return new SelectDTO
            {
                Id = damageType.Id,
                Description = damageType.Description,
            };
        }

        internal static List<SelectDTO> CreateDamageTypes(List<DamageType> damageTypes)
        {
            List<SelectDTO> damageTypesDTO = new List<SelectDTO>();

            foreach (DamageType damageType in damageTypes)
            {
                damageTypesDTO.Add(CreateDamageType(damageType));
            }

            return damageTypesDTO;
        }

        internal static SelectDTO CreateDamageResponsibility(DamageResponsibility damageResponsability)
        {
            return new SelectDTO
            {
                Id = damageResponsability.Id,
                Description = damageResponsability.Description,
            };
        }

        internal static List<SelectDTO> CreateDamageResponsibilities(List<DamageResponsibility> damageResponsabilities)
        {
            List<SelectDTO> damageResponsabilitiesDTO = new List<SelectDTO>();

            foreach (DamageResponsibility damageResponsability in damageResponsabilities)
            {
                damageResponsabilitiesDTO.Add(CreateDamageResponsibility(damageResponsability));
            }

            return damageResponsabilitiesDTO;
        }

        internal static SelectDTO CreatePrefixesByCoveredRiskType(Prefix prefix)
        {
            return new SelectDTO
            {
                Id = prefix.Id,
                Description = prefix.Description
            };
        }

        internal static List<SelectDTO> CreatePrefixesByCoveredRiskTypes(List<Prefix> prefix)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (Prefix prefixes in prefix)
            {
                selectDTO.Add(CreatePrefixesByCoveredRiskType(prefixes));
            }

            return selectDTO;
        }

        internal static SelectDTO CreateDocumentType(UPV1MO.DocumentType documentType)
        {
            return new SelectDTO
            {
                Id = documentType.Id,
                Description = documentType.Description,
            };
        }

        internal static List<SelectDTO> CreateDocumentTypes(List<UPV1MO.DocumentType> documentTypes)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (UPV1MO.DocumentType documentType in documentTypes)
            {
                selectDTO.Add(CreateDocumentType(documentType));
            }

            return selectDTO;
        }

        internal static List<SelectDTO> CreateCatastrophes(List<Catastrophe> catastrophes)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (Catastrophe catastrophe in catastrophes)
            {
                selectDTO.Add(CreateCatastrophe(catastrophe));
            }

            return selectDTO.OrderBy(x => x.Description).ToList();
        }

        internal static SelectDTO CreateCatastrophe(Catastrophe catastrophe)
        {
            return new SelectDTO
            {
                Id = catastrophe.Id,
                Description = catastrophe.Description
            };
        }

        internal static List<SelectDTO> CreateAnalizers(List<ClaimSupplier> analizers)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (ClaimSupplier analizer in analizers)
            {
                selectDTO.Add(CreateAnalizer(analizer));
            }

            return selectDTO;
        }

        internal static SelectDTO CreateAnalizer(ClaimSupplier analizer)
        {
            return new SelectDTO
            {
                Id = analizer.Id,
                Description = analizer.FullName
            };
        }

        internal static List<SelectDTO> CreateAdjusters(List<ClaimSupplier> adjusters)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (ClaimSupplier adjuster in adjusters)
            {
                selectDTO.Add(CreateAdjusters(adjuster));
            }

            return selectDTO;
        }

        internal static SelectDTO CreateAdjusters(ClaimSupplier adjuster)
        {
            return new SelectDTO
            {
                Id = adjuster.Id,
                Description = adjuster.FullName
            };
        }

        internal static List<SelectDTO> CreateResearchers(List<ClaimSupplier> researchers)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (ClaimSupplier researcher in researchers)
            {
                selectDTO.Add(CreateResearcher(researcher));
            }

            return selectDTO;
        }

        internal static SelectDTO CreateResearcher(ClaimSupplier researcher)
        {
            return new SelectDTO
            {
                Id = researcher.Id,
                Description = researcher.FullName
            };
        }

        internal static VehicleDTO CreateClaimNoticeRiskVehicle(VHMO.VehicleDTO riskVehicle)
        {
            if (riskVehicle == null)
            {
                return null;
            }

            return new VehicleDTO
            {
                Plate = riskVehicle.Plate,
                Year = riskVehicle.Year,
                MakeId = riskVehicle.MakeId,
                ModelId = riskVehicle.ModelId,
                ColorId = riskVehicle.ColorId,
                VersionId = riskVehicle.VersionId
            };
        }

        internal static List<SubClaimDTO> CreateSubClaims(Claim claim)
        {
            int modificationNumber = 1;
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();
            foreach (ClaimModify claimModify in claim.Modifications)
            {
                foreach (ClaimCoverage claimCoverage in claimModify.Coverages)
                {
                    foreach (Estimation estimation in claimCoverage.Estimations)
                    {
                        SubClaimDTO subClaim = new SubClaimDTO();
                        subClaim.ModificationNumber = modificationNumber;
                        subClaim.EstimateAmount = estimation.AmountAccumulate;
                        subClaim.EstimateAmountAccumulate = estimation.AmountAccumulate;
                        subClaim.ExchangeRate = estimation.ExchangeRate;
                        subClaim.Currency = estimation.Currency.Description;
                        subClaim.EstimationTypeId = estimation.Type.Id;
                        subClaim.EstimationType = estimation.Type.Description;
                        subClaim.EstimationTypeStatusDescription = estimation.EstimationTypeStatus.Description;
                        subClaim.ClaimModifyId = claimModify.Id;
                        subClaim.AffectedFullName = claimModify.UserName;
                        subClaim.CreationDate = claimModify.RegistrationDate;
                        subClaim.AccountingDate = claimModify.AccountingDate;
                        subClaim.ClaimId = claim.Id;
                        subClaim.SubClaim = claimCoverage.SubClaim;
                        subClaim.RiskId = claimCoverage.RiskId;
                        subClaim.RiskDescription = claimCoverage.RiskDescription;
                        subClaim.RiskNumber = claimCoverage.RiskNumber;
                        subClaim.CoverageId = claimCoverage.CoverageId;
                        subClaim.CoverageDescription = claimCoverage.Description;
                        subClaim.CoverageNumber = claimCoverage.CoverageNumber;
                        subClaim.IsInsured = claimCoverage.IsInsured;
                        subClaim.Insured = claimCoverage.IsInsured ? "ASEGURADO" : "TERCERO";
                        subClaim.IsProspect = claimCoverage.IsProspect;
                        subClaim.EndorsementId = claim.Endorsement.Id;
                        subClaim.IndividualId = claimCoverage.IndividualId;
                        subClaim.PaymentValue = 0;
                        subClaim.TotalConcept = 0;
                        subClaim.TotalTax = 0;
                        subClaim.IsMinimumSalary = estimation.IsMinimumSalary;
                        subClaim.MinimumSalariesNumber = estimation.MinimumSalariesNumber;
                        subClaim.TypeAmountDescription = estimation.IsMinimumSalary ? "SALARIOS" : "VALOR";
                        subClaim.MinimumSalaryValue = estimation.MinimumSalaryValue;
                        subClaims.Add(subClaim);
                    }
                }
                modificationNumber++;
            }
            return subClaims;
        }

        internal static SubClaimDTO CreateSubClaim(ClaimCoverage claimCoverage, Claim claim)
        {
            decimal estimationValue = 0;
            string estimationTypedescription = "";
            int estimationTypeId = 0;
            string currencyDescription = "";
            bool IsMinimumSalary = false;
            decimal MinimumSalaryNumber = 0;

            foreach (Estimation estimation in claimCoverage.Estimations)
            {
                estimationValue += estimation.Amount;
                estimationTypedescription = estimation.Type.Description;
                estimationTypeId = estimation.Type.Id;
                currencyDescription = estimation.Currency.Description;
                IsMinimumSalary = estimation.IsMinimumSalary;
                MinimumSalaryNumber = estimation.MinimumSalariesNumber;
            }

            return new SubClaimDTO
            {
                ClaimId = claim.Id,
                ClaimModifyId = claim.Modifications.First().Id,
                SubClaim = claimCoverage.SubClaim,
                Currency = currencyDescription,
                RiskId = claimCoverage.RiskId,
                RiskDescription = claimCoverage.RiskDescription,
                RiskNumber = claimCoverage.RiskNumber,
                CoverageId = claimCoverage.CoverageId,
                CoverageDescription = claimCoverage.Description,
                CoverageNumber = claimCoverage.CoverageNumber,
                IsInsured = claimCoverage.IsInsured,
                Insured = claimCoverage.IsInsured ? "ASEGURADO" : "TERCERO",
                IsProspect = claimCoverage.IsProspect,
                EndorsementId = claim.Endorsement.Id,
                IndividualId = claimCoverage.IndividualId,
                EstimationTypeId = estimationTypeId,
                EstimationType = estimationTypedescription,
                PaymentValue = 0,
                EstimateAmount = estimationValue,
                TotalConcept = 0,
                TotalTax = 0,
                IsMinimumSalary = IsMinimumSalary,
                MinimumSalariesNumber = MinimumSalaryNumber,
                PrefixId = claim.Prefix.Id,
                BranchCode = claim.Branch.Id
            };
        }

        internal static List<SubClaimDTO> CreateSubClaims(List<ClaimCoverage> claimCoverages, Claim claim)
        {
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();

            foreach (ClaimCoverage claimCoverage in claimCoverages)
            {
                subClaims.Add(CreateSubClaim(claimCoverage, claim));
            }

            return subClaims;
        }

        internal static List<SubClaimDTO> CreateSubClaimEstimations(List<ClaimCoverage> claimCoverages, Claim claim)
        {
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();

            foreach (ClaimCoverage claimCoverage in claimCoverages)
            {
                foreach (Estimation estimation in claimCoverage.Estimations)
                {
                    subClaims.Add(CreateSubClaimEstimation(claimCoverage, claim, estimation));
                }
            }

            return subClaims;
        }

        internal static SubClaimDTO CreateSubClaimEstimation(ClaimCoverage claimCoverage, Claim claim, Estimation estimation)
        {
            return new SubClaimDTO
            {
                ClaimId = claim.Id,
                OccurrenceDate = claim.OccurrenceDate,
                JudicialDecisionDate = claim.JudicialDecisionDate,
                BusinessTypeId = claim.BusinessTypeId,
                ClaimModifyId = claim.Modifications.First().Id,
                ClaimCoverageId = claimCoverage.Id,
                Number = claim.Number,
                SubClaim = claimCoverage.SubClaim,
                CreationDate = claim.Modifications.Last().RegistrationDate,
                Currency = estimation.Currency.Description,
                CurrencyId = estimation.Currency.Id,
                RiskId = claimCoverage.RiskId,
                BranchCode = claim.Branch.Id,
                BranchDescription = claim.Branch?.Description,
                PolicyDocumentNumber = claim.Endorsement.PolicyNumber.ToString(),
                RiskDescription = claimCoverage.RiskDescription,
                RiskNumber = claimCoverage.RiskNumber,
                CoverageId = claimCoverage.CoverageId,
                CoverageDescription = claimCoverage.Description,
                CoverageNumber = claimCoverage.CoverageNumber,
                IsInsured = claimCoverage.IsInsured,
                Insured = claimCoverage.IsInsured ? "ASEGURADO" : "TERCERO",
                IsProspect = claimCoverage.IsProspect,
                InsuredAmountTotal = claimCoverage.SubLimitAmount,
                EndorsementId = claim.Endorsement.Id,
                IndividualId = claimCoverage.IndividualId,
                AffectedFullName = claimCoverage.AffectedFullName,
                EstimationTypeId = estimation.Type.Id,
                EstimationType = estimation.Type.Description,
                PaymentValue = 0,
                EstimateAmount = estimation.AmountAccumulate,
                TotalConcept = 0,
                TotalTax = 0,
                TotalRetention = 0,
                DeductibleAmount = estimation.DeductibleAmount,
                DeductibleNet = 0, //Validar donde debe calcularse el NETO del DEDUCIBLE
                EstimationTypeEstatus = estimation.Reason.Status.Id,
                EstimationTypeStatusId = estimation.Reason.Status.Id,
                EstimationTypeInternalStatusId = (estimation.Reason.Status.InternalStatus != null) ? estimation.Reason.Status.InternalStatus.Id : 0,
                EstimationTypeEstatusReasonCode = estimation.Reason.Id,
                EstimationTypeStatusReasonId = estimation.Reason.Id,
                PolicyId = claim.Endorsement.PolicyId,
                IsTotalParticipation = claim.IsTotalParticipation,
                EstimationTypeStatusDescription = claim.Modifications.First().Coverages.First().Estimations.First().Reason.Status.Description,
                EstimationDate = estimation.CreationDate,
                IsMinimumSalary = estimation.IsMinimumSalary,
                MinimumSalariesNumber = estimation.MinimumSalariesNumber,
                MinimumSalaryValue = estimation.MinimumSalaryValue,
                PrefixId = claim.Prefix.Id,
                PrefixDescription = claim.Prefix?.Description,
                EstimateAmountAccumulate = estimation.AmountAccumulate,
                ExchangeRate = estimation.ExchangeRate,
                ClaimedAmount = claimCoverage.ClaimedAmount,
                IsClaimedAmount = claimCoverage.IsClaimedAmount
            };
        }

        internal static SelectDTO CreatePersonType(UPV1MO.PersonType personType)
        {
            return new SelectDTO
            {
                Id = personType.Id,
                Description = personType.Description
            };
        }

        internal static List<SelectDTO> CreatePersonTypes(List<UPV1MO.PersonType> personTypes)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (UPV1MO.PersonType personType in personTypes)
            {
                selectDTO.Add(CreatePersonType(personType));
            }

            return selectDTO;
        }

        internal static SelectDTO CreateMaritalStatus(UPV1MO.MaritalStatus maritalStatus)
        {
            return new SelectDTO
            {
                Id = maritalStatus.Id,
                Description = maritalStatus.Description
            };
        }

        internal static List<SelectDTO> CreateMaritalsStatus(List<UPV1MO.MaritalStatus> maritalsStatus)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (UPV1MO.MaritalStatus maritalStatus in maritalsStatus)
            {
                selectDTO.Add(CreateMaritalStatus(maritalStatus));
            }

            return selectDTO;
        }

        internal static List<SelectDTO> CreateGenders()
        {
            List<SelectDTO> selectDTOs = new List<SelectDTO>();
            foreach (COMMENUM.Genders item in Enum.GetValues(typeof(COMMENUM.Genders)))
            {
                selectDTOs.Add(new SelectDTO
                {
                    Id = (int)item,
                    Description = Resources.Resources.ResourceManager.GetString("List" + item.ToString())
                });
            }

            return selectDTOs;
        }

        internal static UserDTO CreateUser(UUMO.User user)
        {
            return new UserDTO
            {
                Id = user.UserId,
                AccountName = user.AccountName,
            };
        }

        internal static List<UserDTO> CreateUsers(List<User> user)
        {
            List<UserDTO> userDto = new List<UserDTO>();
            foreach (User itemUser in user)
            {
                userDto.Add(CreateUser(itemUser));
            }
            return userDto;
        }

        internal static ClaimCoverageActivePanelDTO CreateClaimCoverageActivePanel(ClaimCoverageActivePanel claimCoverageActivePanel)
        {
            if (claimCoverageActivePanel == null)
            {
                return null;
            }
            return new ClaimCoverageActivePanelDTO
            {
                CoverageId = claimCoverageActivePanel.CoverageId,
                IsEnabledDriver = claimCoverageActivePanel.EnabledDriver,
                IsEnabledThirdPartyVehicle = claimCoverageActivePanel.EnabledThirdPartyVehicle,
                IsEnabledThird = claimCoverageActivePanel.EnabledThird,
                PrintDescription = claimCoverageActivePanel.CoverageDescription,
                IsEnabledAffectedProperty = claimCoverageActivePanel.EnabledAffectedProperty
            };
        }

        internal static List<ClaimCoverageActivePanelDTO> CreateClaimCoverageActivePanels(List<ClaimCoverageActivePanel> claimCoverageActivePanels)
        {
            List<ClaimCoverageActivePanelDTO> claimCoverangeActivePanelDto = new List<ClaimCoverageActivePanelDTO>();
            foreach (ClaimCoverageActivePanel claimCoverangeActivePanel in claimCoverageActivePanels)
            {
                claimCoverangeActivePanelDto.Add(CreateClaimCoverageActivePanel(claimCoverangeActivePanel));
            }

            return claimCoverangeActivePanelDto;
        }

        internal static DebtorDTO CreateDebtor(Debtor debtor)
        {
            return new DebtorDTO
            {
                IndividualId = debtor.Id,
                FullName = debtor.FullName,
                DocumentNumber = debtor.DocumentNumber,
                Address = debtor.Address,
                Phone = debtor.Phone
            };
        }

        internal static List<DebtorDTO> CreateDebtors(List<Debtor> deptors)
        {
            List<DebtorDTO> debtorsDTO = new List<DebtorDTO>();

            foreach (Debtor deptor in deptors)
            {
                debtorsDTO.Add(CreateDebtor(deptor));
            }

            return debtorsDTO;
        }

        internal static RecuperatorDTO CreateRecuperator(Recuperator recuperator)
        {
            return new RecuperatorDTO
            {
                IndividualId = recuperator.Id,
                FullName = recuperator.FullName,
                DocumentNumber = recuperator.DocumentNumber
            };
        }

        internal static List<RecuperatorDTO> CreateRecuperators(List<Recuperator> recuperators)
        {
            List<RecuperatorDTO> recuperatorsDTO = new List<RecuperatorDTO>();

            foreach (Recuperator recuperator in recuperators)
            {
                recuperatorsDTO.Add(CreateRecuperator(recuperator));
            }

            return recuperatorsDTO;
        }

        internal static SelectDTO CreateCancellationReason(ClaimCancellationReason cancellationReason)
        {
            return new SelectDTO
            {
                Id = cancellationReason.Id,
                Description = cancellationReason.Description
            };
        }

        internal static List<SelectDTO> CreateCancellationReasons(List<ClaimCancellationReason> cancellationReasons)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (ClaimCancellationReason cancellationReason in cancellationReasons)
            {
                selectDTO.Add(CreateCancellationReason(cancellationReason));
            }

            return selectDTO.OrderBy(x => x.Description).ToList();
        }

        public static List<NoticeDTO> CreateNotices(List<Notice> notices)
        {
            List<NoticeDTO> noticeDTOs = new List<NoticeDTO>();
            foreach (Notice notice in notices)
            {
                noticeDTOs.Add(CreateNotice(notice));
            }

            return noticeDTOs;
        }

        internal static NoticeDTO CreateNotice(Notice notice)
        {
            List<NoticeCoverageDTO> coveragesDTO = CreateNoticeCoverages(notice.NoticeCoverages);

            return new NoticeDTO
            {
                Id = notice.Id,
                RiskId = notice.Risk.RiskId,
                RiskNumber = notice.Risk.Number,
                CityId = notice.City.Id,
                NoticeDate = notice.CreationDate,
                OcurrenceDate = notice.ClaimDate,
                PrefixId = Convert.ToInt32(notice.Policy?.PrefixId),
                BranchId = Convert.ToInt32(notice.Policy?.BranchId),
                PolicyId = Convert.ToInt32(notice.Policy?.Id),
                DocumentNumber = Convert.ToString(notice.Policy?.DocumentNumber),
                RiskDescription = notice.Risk.Description,
                CoveredRiskTypeId = notice.CoveredRiskTypeId,
                ObjectedDescription = notice.ObjectedReason,
                StateId = notice.City.State.Id,
                CountryId = notice.City.State.Country.Id,
                Description = notice.Description,
                NoticeTypeDescription = notice.Type?.Description,
                Number = notice.Number,
                IndividualId = notice.IndividualId,
                Location = notice.Address,
                NoticeTypeId = Convert.ToInt32(notice.Type?.Id),
                EndorsementId = Convert.ToInt32(notice.Endorsement?.Id),
                ContactName = notice.ContactInformation?.Name,
                PhoneNumber = notice.ContactInformation?.Phone,
                Email = notice.ContactInformation?.Mail,
                DamageResponsibilityId = notice.DamageResponsability?.Id,
                DamageTypeId = notice.DamageType?.Id,
                NoticeReasonId = notice.NoticeReason.Id,
                NoticeReasonDescription = notice.NoticeReason.Description,
                NoticeStateId = notice.NoticeState.Id,
                NoticeStateDescription = notice.NoticeState.Description,
                NumberObjected = notice.NumberObjected,
                OthersAffected = notice.OthersAffected,
                ClaimedAmount = notice.ClaimedAmount,
                ClaimReasonOthers = notice.ClaimReasonOthers,
                InternalConsecutive = notice.InternalConsecutive,

                Coverages = coveragesDTO
            };
        }

        internal static List<NoticeCoverageDTO> CreateNoticeCoverages(List<NoticeCoverage> noticeCoverages)
        {
            List<NoticeCoverageDTO> coveragesDTO = new List<NoticeCoverageDTO>();

            if (noticeCoverages != null)
            {
                foreach (NoticeCoverage coverage in noticeCoverages)
                {
                    NoticeCoverageDTO coverageDTO = new NoticeCoverageDTO
                    {
                        CoverageId = coverage.CoverageId,
                        CoverNum = coverage.CoverageNumber,
                        EstimateAmount = coverage.EstimateAmount,
                        EstimateTypeId = coverage.EstimateTypeId,
                        IndividualId = coverage.IndividualId,
                        IsInsured = coverage.IsInsured,
                        IsProspect = coverage.IsProspect,
                        RiskNum = coverage.RiskNumber,
                        FullName = coverage.FullName,
                        DocumentNumber = coverage.DocumentNumber,
                        DocumentTypeId = coverage.DocumentTypeId,
                    };

                    coveragesDTO.Add(coverageDTO);
                }
            }
            return coveragesDTO;
        }

        internal static HolderDTO CreateHolder(UNDMO.HolderDTO holder)
        {
            return new HolderDTO
            {
                Id = holder.Id,
                IndividualId = holder.IndividualId,
                DocumentNumber = holder.DocumentNumber,
                FullName = holder.FullName,
                DocumentTypeId = holder.DocumentTypeId
            };
        }

        internal static List<HolderDTO> CreateHolders(List<UNDMO.HolderDTO> holders)
        {
            List<HolderDTO> holdersDTO = new List<HolderDTO>();
            foreach (UNDMO.HolderDTO holder in holders)
            {
                holdersDTO.Add(CreateHolder(holder));
            }

            return holdersDTO;
        }

        internal static ClaimCoverageDriverInformationDTO CreateDriverByDocumentNumber(Driver driver)
        {
            if (driver == null)
            {
                return null;
            }
            return new ClaimCoverageDriverInformationDTO
            {
                LicenseNumber = driver.LicenseNumber,
                LicenseType = driver.LicenseType,
                LicenseValidThru = driver.LicenseValidThru,
                Age = driver.Age
            };
        }

        internal static CoveragePaymentConceptDTO CreateCoveragePaymentConcept(CoveragePaymentConcept coveragePaymentConcept)
        {
            return new CoveragePaymentConceptDTO
            {
                CoverageId = coveragePaymentConcept.CoverageId,
                ConceptId = coveragePaymentConcept.ConceptId,
                EstimationTypeId = coveragePaymentConcept.EstimationTypeId
            };
        }

        internal static List<AffectedDTO> CreateAffected(List<Affected> affecteds)
        {
            List<AffectedDTO> affectedDTO = new List<AffectedDTO>();

            foreach (Affected affected in affecteds)
            {
                affectedDTO.Add(CreateAffected(affected));
            }

            return affectedDTO;
        }

        internal static AffectedDTO CreateAffected(Affected affected)
        {
            return new AffectedDTO
            {
                IndividualId = affected.Id,
                FullName = affected.FullName,
                DocumentTypeId = affected.DocumentTypeId,
                DocumentNumber = affected.DocumentNumber
            };
        }

        internal static PolicyDTO CreateClaimPolicy(UNDMO.PolicyDTO policy)
        {
            return new PolicyDTO
            {
                Id = policy.Id,
                DocumentNumber = policy.DocumentNumber,
                RiskId = policy.RiskId,
                RiskDescription = policy.RiskDescription,
                BranchId = policy.BranchId,
                BusinessTypeId = policy.BusinessTypeId,
                BusinessType = policy.BusinessType,
                ClaimsQuantity = policy.ClaimsQuantity,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.CurrentTo,
                EndorsementId = policy.EndorsementId,
                HolderId = Convert.ToInt32(policy.HolderId),
                HolderDocumentNumber = policy.HolderDocumentNumber,
                HolderName = policy.HolderName,
                IssueDate = policy.IssueDate,
                PrefixId = policy.PrefixId,
                IndividualId = policy.IndividualId,
                ProducDescription = policy.ProductDescription
            };
        }

        internal static List<PolicyDTO> CreateClaimPolicies(List<UNDMO.PolicyDTO> policies)
        {
            List<PolicyDTO> policiesDTO = new List<PolicyDTO>();
            foreach (UNDMO.PolicyDTO policy in policies)
            {
                policiesDTO.Add(CreateClaimPolicy(policy));
            }

            return policiesDTO;
        }

        public static ClaimCoverageDriverInformationDTO CreateClaimDriverInformation(Driver driver)
        {
            if (driver == null)
            {
                return null;
            }
            return new ClaimCoverageDriverInformationDTO
            {
                LicenseValidThru = driver.LicenseValidThru,
                LicenseType = driver.LicenseType,
                LicenseNumber = driver.LicenseNumber,
                Age = driver.Age,
                Name = driver.Name,
                DocumentNumber = driver.DocumentNumber
            };
        }

        public static ClaimCoverageThirdPartyVehicleDTO CreateClaimThirdPartyVehicle(ThirdPartyVehicle thirdPartyVehicle)
        {
            if (thirdPartyVehicle == null)
            {
                return null;
            }
            return new ClaimCoverageThirdPartyVehicleDTO
            {
                ChasisNumber = thirdPartyVehicle.ChasisNumber,
                ColorCode = thirdPartyVehicle.ColorCode,
                Description = thirdPartyVehicle.Description,
                EngineNumber = thirdPartyVehicle.EngineNumber,
                Make = thirdPartyVehicle.Make,
                Model = thirdPartyVehicle.Model,
                Plate = thirdPartyVehicle.Plate,
                VinCode = thirdPartyVehicle.VinCode,
                Year = thirdPartyVehicle.Year
            };
        }

        public static ClaimSupplierDTO CreateClaimSupplier(Inspection inspection)
        {
            if (inspection == null)
            {
                return null;
            }
            return new ClaimSupplierDTO
            {
                AdjusterId = inspection.AdjusterId,
                AnalizerId = inspection.AnalizerId,
                ResearcherId = inspection.ResearcherId,
                DateInspection = inspection.RegistrationDate,
                LossDescription = inspection.LossDescription,
                AffectedProperty = inspection.AffectedProperty,
                HourInspection = inspection.RegistrationHour
            };
        }

        public static CatastrophicInformationDTO CreateCatastrophicInformation(CatastrophicEvent catastrophicEvent)
        {
            if (catastrophicEvent == null)
            {
                return null;
            }
            return new CatastrophicInformationDTO
            {
                CatastropheDescription = catastrophicEvent.Description,
                Description = catastrophicEvent.Catastrophe.Description,
                Address = catastrophicEvent.FullAddress,
                CatastrophicId = catastrophicEvent.Catastrophe.Id,
                City = new CityDTO
                {
                    Id = catastrophicEvent.City.Id,
                    State = new StateDTO
                    {
                        Id = catastrophicEvent.City.State.Id,
                        Country = new CountryDTO
                        {
                            Id = catastrophicEvent.City.State.Country.Id
                        }
                    },
                },
                DateTimeFrom = catastrophicEvent.CurrentFrom,
                DateTimeTo = catastrophicEvent.CurrentTo
            };
        }

        internal static CoverageDeductibleDTO CreateCoverageDeductible(UNDMO.DeductibleDTO deductibleDTO)
        {
            if (deductibleDTO == null)
            {
                return null;
            }

            return new CoverageDeductibleDTO
            {
                Code = deductibleDTO.Code,
                Description = deductibleDTO.Description
            };
        }

        internal static RiskLocationDTO CreateLocation(PPMO.RiskLocationDTO riskLocation)
        {
            return new RiskLocationDTO
            {
                ConstructionYear = riskLocation.ConstructionYear,
                FloorNumber = riskLocation.FloorNumber,
                HasNomenclature = riskLocation.HasNomenclature,
                Latitude = riskLocation.Latitude,
                Longitude = riskLocation.Longitude,
                FullAddress = riskLocation.FullAddress,
                IsDeclarative = riskLocation.IsDeclarative,
                PML = riskLocation.PML,
                Square = riskLocation.Square,
                RiskAge = riskLocation.RiskAge,
                RiskId = riskLocation.RiskId,
                AmountInsured = riskLocation.AmountInsured,
                CoveredRiskType = riskLocation.CoveredRiskType,
                Country = riskLocation.Country,
                CountryId = riskLocation.CountryId,
                State = riskLocation.State,
                StateId = riskLocation.StateId,
                City = riskLocation.City,
                CityId = riskLocation.CityId,
                DocumentNum = riskLocation.DocumentNum,
                PolicyId = riskLocation.PolicyId,
                EndorsementId = riskLocation.EndorsementId,
                InsuredId = riskLocation.InsuredId,
                RiskNumber = riskLocation.RiskNumber
            };
        }

        internal static List<RiskLocationDTO> CreateLocations(List<PPMO.RiskLocationDTO> riskLocations)
        {
            List<RiskLocationDTO> riskLocationsDTO = new List<RiskLocationDTO>();

            foreach (PPMO.RiskLocationDTO riskLocation in riskLocations)
            {
                riskLocationsDTO.Add(CreateLocation(riskLocation));
            }

            return riskLocationsDTO;
        }

        internal static ClaimLocationDTO CreateClaimLocation(ClaimLocation claimLocation)
        {
            return new ClaimLocationDTO
            {
                AuthorizationPolicies = claimLocation.Claim.AuthorizationPolicies,
                Number = claimLocation.Claim.Number,
                ClaimId = claimLocation.Claim.Id,
                CoveredRiskType = (int)claimLocation.Claim.CoveredRiskType,
                TemporalId = claimLocation.Claim.TemporalId
            };
        }

        internal static List<SelectDTO> CreateSearchTypes()
        {
            List<SelectDTO> selectDTOs = new List<SelectDTO>();
            foreach (SearchType item in Enum.GetValues(typeof(SearchType)))
            {
                selectDTOs.Add(new SelectDTO
                {
                    Id = (int)item,
                    Description = Resources.Resources.ResourceManager.GetString("List" + item.ToString())
                });
            }
            return selectDTOs;
        }

        internal static ClaimReserveDTO CreateClaimReserve(ClaimReserve claimReserve)
        {
            return new ClaimReserveDTO
            {
                AuthorizationPolicies = claimReserve.Claim.AuthorizationPolicies 
            };
        }

        internal static List<ClaimReserveDTO> CreateClaimReserve(List<ClaimReserve> claimsReserve)
        {
            List<ClaimReserveDTO> ClaimReserveDTO = new List<ClaimReserveDTO>();

            foreach (ClaimReserve claimReserve in claimsReserve)
            {
                ClaimReserveDTO.Add(CreateClaimReserve(claimReserve));
            }

            return ClaimReserveDTO;
        }

        internal static SubClaimDTO CreateSubClaimReserve(ClaimCoverage claimCoverage, Claim claim)
        {
            return new SubClaimDTO
            {
                ClaimId = claim.Id,
                NoticeId = claim.NoticeId,
                NoticeDate = claim.NoticeDate,
                OccurrenceDate = claim.OccurrenceDate,
                JudicialDecisionDate = claim.JudicialDecisionDate,
                CauseId = claim.Cause.Id,
                Operation = claim.TextOperation.Operation,
                Location = claim.Location,
                CountryId = claim.City?.State?.Country?.Id,
                StateId = claim.City?.State?.Id,
                CityId = claim.City?.Id,
                Number = claim.Number,
                PrefixId = claim.Prefix.Id,
                ClaimCoverageId = claimCoverage.Id,
                ClaimModifyId = claim.Modifications.First().Id,
                SubClaim = claimCoverage.SubClaim,
                RiskId = claimCoverage.RiskId,
                RiskDescription = claimCoverage.RiskDescription,
                RiskNumber = claimCoverage.RiskNumber,
                CoverageId = claimCoverage.CoverageId,
                CoverageDescription = claimCoverage.Description,
                CoverageNumber = claimCoverage.CoverageNumber,
                IsInsured = claimCoverage.IsInsured,
                Insured = claimCoverage.IsInsured ? "ASEGURADO" : "TERCERO",
                IsProspect = claimCoverage.IsProspect,
                InsuredAmountTotal = claimCoverage.SubLimitAmount,
                EndorsementId = claim.Endorsement.Id,
                IndividualId = claimCoverage.IndividualId,
                PaymentValue = 0,
                BusinessTypeId = claim.BusinessTypeId,
                PolicyId = claim.Endorsement.PolicyId,
                DamageTypeCode = claim.DamageType.Id,
                DamageResponsibilityCode = claim.DamageResponsability.Id
            };
        }

        internal static List<SubClaimDTO> CreateSubClaimReserve(List<ClaimCoverage> claimCoverages, Claim claim)
        {
            List<SubClaimDTO> subClaims = new List<SubClaimDTO>();

            foreach (ClaimCoverage claimCoverage in claimCoverages)
            {
                foreach (Estimation estimation in claimCoverage.Estimations)
                {
                    SubClaimDTO subClaimDTO = CreateSubClaimReserve(claimCoverage, claim);
                    subClaimDTO.Currency = estimation.AmountAccumulate > 0 ? estimation.Currency.Description : null;
                    subClaimDTO.EstimateAmount = estimation.AmountAccumulate;
                    subClaimDTO.EstimateAmountAccumulate = estimation.AmountAccumulate;
                    subClaimDTO.ExchangeRate = estimation.ExchangeRate;
                    subClaimDTO.EstimationType = estimation.Type.Description;
                    subClaimDTO.EstimationTypeEstatusReasonCode = estimation.Reason.Id;
                    subClaimDTO.EstimationTypeEstatus = estimation.Reason.Status.Id;
                    subClaimDTO.EstimationTypeInternalStatusId = estimation.Reason.Status.InternalStatus.Id;
                    subClaimDTO.EstimationTypeId = estimation.Type.Id;
                    subClaimDTO.DeductibleAmount = estimation.DeductibleAmount;
                    subClaimDTO.CreationDate = estimation.CreationDate;
                    subClaimDTO.IsMinimumSalary = estimation.IsMinimumSalary;
                    subClaimDTO.MinimumSalariesNumber = estimation.MinimumSalariesNumber;
                    subClaimDTO.MinimumSalaryValue = estimation.MinimumSalaryValue;
                    subClaimDTO.CurrencyId = estimation.AmountAccumulate > 0 ? estimation.Currency.Id : 0;
                    subClaimDTO.ClaimedAmount = claimCoverage.ClaimedAmount;
                    subClaimDTO.IsClaimedAmount = claimCoverage.IsClaimedAmount;
                    subClaimDTO.IsTotalParticipation = claim.IsTotalParticipation;

                    subClaims.Add(subClaimDTO);
                }
            }

            return subClaims;
        }

        internal static EstimationDTO CreateEstimationByClaimId(Estimation estimation)
        {
            return new EstimationDTO
            {
                Id = estimation.Type.Id,
                Description = estimation.Type.Description,
                StatusCode = estimation.Reason == null ? "" : estimation.Reason.Status.Description,
                StatusCodeId = estimation.Reason == null ? 0 : estimation.Reason.Status.Id,
                Reason = estimation.Reason == null ? "" : estimation.Reason.Description,
                ReasonId = estimation.Reason == null ? 0 : estimation.Reason.Id,
                CurrencyReason = estimation.Currency == null ? "" : estimation.Currency.Description,
                CurrencyReasonId = estimation.Currency == null ? 0 : estimation.Currency.Id,
                EstimateAmount = estimation.Amount,
                EstimateAmountAccumulate = estimation.AmountAccumulate,
                ExchangeRate = estimation.ExchangeRate,
                Deductible = 0,
                DeductibleAmount = estimation.DeductibleAmount,
                Payments = Convert.ToDecimal(estimation.PaymentAmount),
                PendingReservation = 0,
                IsMinimumSalary = estimation.IsMinimumSalary,
                MinimumSalariesNumber = estimation.MinimumSalariesNumber
            };
        }

        internal static List<EstimationDTO> CreateEstimationByClaimId(List<Estimation> estimations)
        {
            List<EstimationDTO> estimationsDTO = new List<EstimationDTO>();

            foreach (Estimation estimation in estimations)
            {
                estimationsDTO.Add(CreateEstimationByClaimId(estimation));
            }

            return estimationsDTO;
        }

        internal static List<TransportDTO> CreateTransports(List<TRMO.TransportDTO> transports)
        {
            List<TransportDTO> transportsDTO = new List<TransportDTO>();

            foreach (TRMO.TransportDTO transport in transports)
            {
                transportsDTO.Add(CreateTransport(transport));
            }

            return transportsDTO;
        }

        internal static TransportDTO CreateTransport(TRMO.TransportDTO transport)
        {
            return new TransportDTO
            {
                RiskId = transport.RiskId,
                Risk = transport.Risk,
                RiskNumber = transport.RiskNumber,
                CargoTypeDescription = transport.CargoTypeDescription,
                CargoTypeId = transport.CargoTypeId,
                PackagingTypeDescription = transport.PackagingTypeDescription,
                PackagingTypeId = transport.PackagingTypeId,
                CityFromDescription = transport.CityFromDescription,
                CityToDescription = transport.CityToDescription,
                CityFromId = transport.CityFromId,
                CityToId = transport.CityToId,
                InsuredAmount = transport.InsuredAmount,
                CoveredRiskType = transport.CoveredRiskType,
                ViaTypeDescription = transport.ViaTypeDescription,
                ViaTypeId = transport.ViaTypeId,
                EndorsementId = transport.EndorsementId,
                PolicyId = transport.PolicyId,
                PolicyDocumentNumber = transport.PolicyDocumentNumber,
                InsuredId = transport.InsuredId
            };
        }

        internal static List<SuretyDTO> CreateSureties(List<SRMO.SuretyDTO> suretyDTOs)
        {
            List<SuretyDTO> suretyDTO = new List<SuretyDTO>();

            foreach (SRMO.SuretyDTO surety in suretyDTOs)
            {
                suretyDTO.Add(CreateSurety(surety));
            }

            return suretyDTO;
        }

        internal static SuretyDTO CreateSurety(SRMO.SuretyDTO surety)
        {
            if (surety == null)
            {
                return null;
            }
            return new SuretyDTO
            {
                ArticleId = surety.ArticleId,
                ArticleDescription = surety.ArticleDescription,
                Endorsement = surety.Endorsement,
                EndorsementId = surety.EndorsementId,
                DocumentNum = surety.DocumentNum,
                CoveredRiskType = surety.CoveredRiskType,
                EstimationAmount = surety.EstimationAmount,
                PremiumAmt = surety.PremiumAmt,
                ContractAmt = surety.ContractAmt,
                CourtNum = surety.CourtNum,
                BidNumber = surety.BidNumber,
                Bonded = surety.Bonded,
                IdentificationDocument = surety.IdentificationDocument,
                SuretyDocumentNumber = surety.SuretyDocumentNumber,
                SuretyName = surety.SuretyName,
                IndividualId = surety.IndividualId,
                RiskNumber = surety.RiskNumber,
                PolicyId = surety.PolicyId,
                RiskId = surety.RiskId,
                InsuredId = surety.InsuredIndividualId,
                InsuredAmount = surety.InsuredAmount
            };
        }

        internal static FidelityDTO CreateFidelity(FDMO.FidelityDTO fidelity)
        {
            return new FidelityDTO
            {
                CommercialClassId = fidelity.CommercialClassId,
                CommercialClassDescription = fidelity.CommercialClassDescription,
                OccupationId = fidelity.OccupationId,
                OccupationDescription = fidelity.Description,
                Description = fidelity.Description,
                DiscoveryDate = fidelity.DiscoveryDate,
                RiskId = fidelity.RiskId,
                Risk = fidelity.Risk,
                RiskNumber = fidelity.RiskNumber,
                CoveredRiskType = fidelity.CoveredRiskType,
                EndorsementId = fidelity.EndorsementId,
                InsuredId = fidelity.InsuredId,
                PolicyId = fidelity.PolicyId,
                PolicyDocumentNumber = fidelity.PolicyDocumentNumber,
                InsuredAmount = fidelity.InsuredAmount
            };
        }

        internal static List<FidelityDTO> CreateFidelities(List<FDMO.FidelityDTO> fidelities)
        {
            List<FidelityDTO> fidelitiesDTO = new List<FidelityDTO>();

            foreach (FDMO.FidelityDTO fidelity in fidelities)
            {
                fidelitiesDTO.Add(CreateFidelity(fidelity));
            }

            return fidelitiesDTO;
        }

        internal static ClaimTransportDTO CreateClaimTransport(ClaimTransport claimTransport)
        {
            return new ClaimTransportDTO
            {
                AuthorizationPolicies = claimTransport.Claim.AuthorizationPolicies,
                Number = claimTransport.Claim.Number,
                ClaimId = claimTransport.Claim.Id,
                CoveredRiskType = (int)claimTransport.Claim.CoveredRiskType,
                TemporalId = claimTransport.Claim.TemporalId
            };
        }

        internal static ClaimAirCraftDTO CreateClaimAirCraft(ClaimAirCraft claimAirCraft)
        {
            return new ClaimAirCraftDTO
            {
                AuthorizationPolicies = claimAirCraft.Claim.AuthorizationPolicies,
                Number = claimAirCraft.Claim.Number,
                ClaimId = claimAirCraft.Claim.Id,
                CoveredRiskType = (int)claimAirCraft.Claim.CoveredRiskType,
                TemporalId = claimAirCraft.Claim.TemporalId
            };
        }

        internal static ClaimFidelityDTO CreateClaimFidelity(ClaimFidelity claimFidelity)
        {
            return new ClaimFidelityDTO
            {
                AuthorizationPolicies = claimFidelity.Claim.AuthorizationPolicies,
                Number = claimFidelity.Claim.Number,
                ClaimId = claimFidelity.Claim.Id,
                CoveredRiskType = (int)claimFidelity.Claim.CoveredRiskType,
                TemporalId = claimFidelity.Claim.TemporalId
            };
        }

        internal static NoticeVehicleDTO CreateNoticeVehicle(NoticeVehicle noticeVehicle)
        {
            if (noticeVehicle == null)
            {
                return null;
            }
            return new NoticeVehicleDTO
            {
                Id = noticeVehicle.Notice.Id,
                Number = noticeVehicle.Notice.Number
            };
        }

        internal static NoticeLocationDTO CreateNoticeLocation(NoticeLocation noticeLocation)
        {
            return new NoticeLocationDTO
            {
                Id = noticeLocation.Notice.Id,
                Number = noticeLocation.Notice.Number
            };
        }

        internal static NoticeSuretyDTO CreateNoticeSurety(NoticeSurety noticeSurety)
        {
            return new NoticeSuretyDTO
            {
                Id = noticeSurety.Notice.Id,
                Number = noticeSurety.Notice.Number
            };
        }

        internal static NoticeTransportDTO CreateNoticeTransport(NoticeTransport noticeTransport)
        {
            return new NoticeTransportDTO
            {
                Id = noticeTransport.Notice.Id,
                Number = noticeTransport.Notice.Number
            };
        }

        internal static NoticeAirCraftDTO CreateNoticeAirCraft(NoticeAirCraft noticeAirCraft)
        {
            return new NoticeAirCraftDTO
            {
                Id = noticeAirCraft.Notice.Id,
                Number = noticeAirCraft.Notice.Number
            };
        }

        internal static NoticeFidelityDTO CreateNoticeFidelity(NoticeFidelity noticeFidelity)
        {
            return new NoticeFidelityDTO
            {
                Id = noticeFidelity.Notice.Id,
                Number = noticeFidelity.Notice.Number
            };
        }

        //internal static RiskLocationDTO CreateCompanyLocation(CompanyPropertyRisk companyPropertyRisk)
        //{
        //    if (companyPropertyRisk == null)
        //    {
        //        return null;
        //    }
        //    return new RiskLocationDTO
        //    {
        //        AmountInsured = companyPropertyRisk.Risk.AmountInsured,
        //        FullAddress = companyPropertyRisk.FullAddress,
        //        RiskId = companyPropertyRisk.Risk.RiskId,
        //        CoveredRiskType = Convert.ToInt32(companyPropertyRisk.Risk.CoveredRiskType),
        //        City = companyPropertyRisk.City?.Description,
        //        CityId = companyPropertyRisk.City?.Id,
        //        Country = companyPropertyRisk.City?.State.Country?.Description,
        //        CountryId = companyPropertyRisk.City?.State.Country.Id,
        //        State = companyPropertyRisk.City?.State.Description,
        //        StateId = companyPropertyRisk.City?.State?.Id,
        //        DocumentNum = companyPropertyRisk.Risk.Policy.DocumentNumber,
        //        PolicyId = companyPropertyRisk.Risk.Policy.Id,
        //        EndorsementId = companyPropertyRisk.Risk.Policy.Endorsement.Id,
        //        InsuredId = Convert.ToInt32(companyPropertyRisk.Risk?.MainInsured?.IndividualId)
        //    };
        //}

        internal static RiskLocationDTO CreateRiskLocation(ClaimLocation claimLocation)
        {
            if (claimLocation == null)
            {
                return null;
            }
            return new RiskLocationDTO
            {
                FullAddress = claimLocation.Address,
                CityId = claimLocation.City?.Id,
                CountryId = claimLocation.City?.State.Country.Id,
                StateId = claimLocation.City?.State?.Id,
            };
        }

        internal static SuretyDTO CreateRiskSurety(NoticeSurety noticeSurety)
        {
            return new SuretyDTO
            {
                BidNumber = noticeSurety.BidNumber,
                CourtNum = noticeSurety.CourtNum,
                SuretyName = noticeSurety.Name,
                SuretyDocumentNumber = noticeSurety.DocumentNumber
            };
        }

        internal static TransportDTO CreateRiskTransport(NoticeTransport noticeTransport)
        {
            return new TransportDTO
            {
                CargoTypeDescription = noticeTransport.CargoType,
                PackagingTypeDescription = noticeTransport.PackagingType,
                CityFromDescription = noticeTransport.Origin,
                CityToDescription = noticeTransport.Destiny,
                ViaTypeDescription = noticeTransport.TransportType
            };
        }

        internal static AirCraftDTO CreateRiskAirCraft(NoticeAirCraft noticeAirCraft)
        {
            return new AirCraftDTO
            {
                MakeId = noticeAirCraft.MakeId,
                ModelId = noticeAirCraft.ModelId,
                TypeId = noticeAirCraft.TypeId,
                UseId = noticeAirCraft.UseId,
                RegisterId = noticeAirCraft.RegisterId,
                OperatorId = noticeAirCraft.OperatorId,
                RegisterNumber = noticeAirCraft.RegisterNumer,
                Year = noticeAirCraft.Year
            };
        }

        internal static FidelityDTO CreateRiskFidelity(NoticeFidelity noticeFidelity)
        {
            return new FidelityDTO
            {
                CommercialClassId = noticeFidelity.RiskCommercialClassId,
                OccupationId = noticeFidelity.OccupationId,
                Description = noticeFidelity.Description,
                DiscoveryDate = noticeFidelity.DiscoveryDate
            };
        }

        internal static SelectDTO CreateRiskCommercialClass(UNDMO.RiskCommercialClassDTO riskCommercialClass)
        {
            return new SelectDTO
            {
                Id = riskCommercialClass.Id,
                Description = riskCommercialClass.Description
            };
        }

        internal static List<SelectDTO> CreateRiskCommercialClasses(List<UNDMO.RiskCommercialClassDTO> riskCommercialClasses)
        {
            List<SelectDTO> riskCommercialClassesDTO = new List<SelectDTO>();

            foreach (UNDMO.RiskCommercialClassDTO riskCommercialClass in riskCommercialClasses)
            {
                riskCommercialClassesDTO.Add(CreateRiskCommercialClass(riskCommercialClass));
            }

            return riskCommercialClassesDTO;
        }

        internal static SelectDTO CreateOccupation(FDMO.OccupationDTO occupation)
        {
            return new SelectDTO
            {
                Id = occupation.Id,
                Description = occupation.Description
            };
        }

        internal static List<SelectDTO> CreateOccupations(List<FDMO.OccupationDTO> occupations)
        {
            List<SelectDTO> selects = new List<SelectDTO>();

            foreach (FDMO.OccupationDTO occupation in occupations)
            {
                selects.Add(CreateOccupation(occupation));
            }

            return selects;
        }

        internal static ClaimLimitDTO CreateClaimInsuredAmount(Limit limit)
        {
            return new ClaimLimitDTO
            {
                Payment = limit.Payment,
                Consumtion = limit.Consumption,
                InsuredAmount = limit.InsuredAmount,
                PaymentConsumtion = limit.PaymentConsumption,
                Reserve = limit.Reserve

            };
        }

        internal static List<CoverageDeductibleDTO> CreateClaimDeductibles(List<UNDMO.DeductibleDTO> deductibles)
        {
            List<CoverageDeductibleDTO> deductiblesDTO = new List<CoverageDeductibleDTO>();

            foreach (UNDMO.DeductibleDTO deductible in deductibles)
            {
                deductiblesDTO.Add(CreateClaimDeductible(deductible));
            }

            return deductiblesDTO;
        }

        internal static CoverageDeductibleDTO CreateClaimDeductible(UNDMO.DeductibleDTO deductible)
        {
            if (deductible == null)
            {
                return null;
            }
            return new CoverageDeductibleDTO
            {
                Code = deductible.Code,
                Description = deductible.Description
            };
        }

        internal static string FormatDateValue(int value)
        {
            return value < 10 ? "0" + value.ToString() : value.ToString();
        }

        internal static SubCauseDTO CreateSubCause(SubCause subcause)
        {
            return new SubCauseDTO
            {
                Id = subcause.Id,
                Description = subcause.Description,
                CauseId = subcause.Cause.Id
            };
        }

        internal static List<SubCauseDTO> CreateSubCauses(List<SubCause> subCauses)
        {
            List<SubCauseDTO> subCauseDTOs = new List<SubCauseDTO>();

            foreach (SubCause subcause in subCauses)
            {
                subCauseDTOs.Add(CreateSubCause(subcause));
            }

            return subCauseDTOs.OrderBy(x => x.Description).ToList();
        }

        internal static List<ModuleDTO> CreateModules(List<UUMO.Module> modules)
        {
            List<ModuleDTO> moduleDTOs = new List<ModuleDTO>();

            foreach (UUMO.Module module in modules)
            {
                moduleDTOs.Add(CreateModule(module));
            }

            return moduleDTOs;
        }

        internal static ModuleDTO CreateModule(UUMO.Module module)
        {
            return new ModuleDTO
            {
                Id = module.Id,
                Description = module.Description,
                IsEnabled = module.IsEnabled,
                EnabledDescription = module.EnabledDescription,
                ExpirationDate = module.ExpirationDate,
                Status = module.Status,
                VirtualFolder = module.VirtualFolder
            };
        }

        internal static List<SubModuleDTO> CreateSubModules(List<UUMO.SubModule> subModules)
        {
            List<SubModuleDTO> subModuleDTOs = new List<SubModuleDTO>();

            foreach (UUMO.SubModule model in subModules)
            {
                subModuleDTOs.Add(CreateSubModule(model));
            }

            return subModuleDTOs;
        }

        internal static SubModuleDTO CreateSubModule(UUMO.SubModule subModule)
        {
            return new SubModuleDTO
            {
                SubModuleId = subModule.Id,
                Description = subModule.Description,
                Enabled = subModule.IsEnabled,
                ModuleId = subModule.Module.Id

            };
        }


        internal static ClaimDocumentationDTO CreateDocumentation(ClaimDocumentation documentation)
        {
            return new ClaimDocumentationDTO
            {
                Id = documentation.Id,
                ModuleId = documentation.ModuleId,
                SubmoduleId = documentation.SubmoduleId,
                Description = documentation.Description,
                prefix = documentation.prefix.Id,
                IsRequired = documentation.IsRequired,
                Enable = documentation.Enable
            };
        }

        internal static List<ClaimDocumentationDTO> CreateDocumentations(List<ClaimDocumentation> documentations)
        {
            List<ClaimDocumentationDTO> claimsDocumentationDTOs = new List<ClaimDocumentationDTO>();

            foreach (ClaimDocumentation documentation in documentations)
            {
                claimsDocumentationDTOs.Add(CreateDocumentation(documentation));
            }

            return claimsDocumentationDTOs.OrderBy(x => x.Description).ToList();
        }


        internal static List<IndividualTaxDTO> CreateIndividualTaxes(List<TAXMO.IndividualTaxCategoryConditionDTO> taxes)
        {
            List<IndividualTaxDTO> individualTaxDTOs = new List<IndividualTaxDTO>();

            foreach (TAXMO.IndividualTaxCategoryConditionDTO tax in taxes)
            {
                individualTaxDTOs.Add(CreateIndividualTax(tax));
            }

            return individualTaxDTOs;
        }

        internal static IndividualTaxDTO CreateIndividualTax(TAXMO.IndividualTaxCategoryConditionDTO tax)
        {
            return new IndividualTaxDTO
            {
                TaxId = tax.TaxId,
                TaxDescription = tax.TaxDescription,
                TaxCategoryId = tax.TaxCategoryId,
                TaxCategoryDescription = tax.TaxCategoryDescription,
                TaxConditionId = tax.TaxConditionId,
                TaxConditionDescription = tax.TaxConditionDescription,
                IndividualId = tax.IndividualId,
                BaseConditionTaxId = tax.BaseConditionTaxId,
                IsRetention = tax.IsRetention,
                Enabled = tax.Enabled,
                Rate = tax.Rate,
                MinBaseAmount = tax.MinBaseAmount,
                CurrentFrom = tax.CurrentFrom,
                CurrentTo = tax.CurrentTo,
                BranchId = tax.BranchId,
                BranchDescription = tax.BranchDescription,
                CountryId = tax.CountryId,
                CoverageId = tax.CoverageId,
                EconomicActivityId = tax.EconomicActivityTaxId,
                StateId = tax.StateId,
                LineBusinessId = tax.LineBusinessId,
                RateTypeId = tax.RateTypeId,
                RateTypeDescription = tax.RateTypeDescription,
            };
        }

        internal static List<ClaimParticipantDTO> CreateParticipants(List<Participant> participants)
        {
            List<ClaimParticipantDTO> claimParticipantsDTO = new List<ClaimParticipantDTO>();

            foreach (Participant participant in participants)
            {
                claimParticipantsDTO.Add(CreateParticipant(participant));
            }

            return claimParticipantsDTO;
        }

        internal static ClaimParticipantDTO CreateParticipant(Participant participant)
        {
            return new ClaimParticipantDTO
            {
                Id = participant.Id,
                DocumentTypeId = participant.DocumentTypeId,
                DocumentNumber = participant.DocumentNumber,
                Fullname = participant.Fullname,
                Phone = participant.Phone,
                Address = participant.Address
            };
        }

        internal static List<ThirdAffectedDTO> CreateClaimCoverageThirdAffecteds(List<ThirdAffected> affecteds)
        {
            List<ThirdAffectedDTO> affectedDTOs = new List<ThirdAffectedDTO>();

            foreach (ThirdAffected affected in affecteds)
            {
                affectedDTOs.Add(CreateClaimCoverageThirdAffected(affected));
            }

            return affectedDTOs;
        }

        internal static ThirdAffectedDTO CreateClaimCoverageThirdAffected(ThirdAffected thirdAffected)
        {
            return new ThirdAffectedDTO
            {
                Id = thirdAffected.ClaimCoverageId,
                DocumentNumber = thirdAffected.DocumentNumber,
                FullName = thirdAffected.FullName
            };
        }

        internal static PendingOperationDTO CreatePendingOperation(PendingOperation pendingOperation)
        {
            return new PendingOperationDTO
            {
                Id = pendingOperation.Id,
                CreationDate = pendingOperation.CreationDate,
                UserId = pendingOperation.UserId,
                Operation = pendingOperation.Operation
            };
        }



        #region Vehicles

        internal static List<VehicleDTO> CreateVehicles(List<VHMO.VehicleDTO> Vehicles)
        {
            List<VehicleDTO> vehiclesDTO = new List<VehicleDTO>();

            foreach (VHMO.VehicleDTO vehicle in Vehicles)
            {
                vehiclesDTO.Add(CreateVehicle(vehicle));
            }

            return vehiclesDTO;
        }

        internal static VehicleDTO CreateVehicle(VHMO.VehicleDTO vehicle)
        {
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleDTO
            {
                RiskId = vehicle.RiskId,
                RiskNumber = vehicle.RiskNumber,
                Plate = vehicle.Plate,
                Year = vehicle.Year,
                Make = vehicle.Make,
                MakeId = vehicle.MakeId,
                Model = vehicle.Model,
                ModelId = vehicle.ModelId,
                Color = vehicle.Color,
                ColorId = vehicle.ColorId,
                InsuredAmount = vehicle.InsuredAmount,
                Chasis = vehicle.Chasis,
                Motor = vehicle.Motor,
                NumberBeneficiarie = vehicle.NumberBeneficiarie,
                NameBeneficiarie = vehicle.NameBeneficiarie,
                ParticipationBeneficiarie = vehicle.ParticipationBeneficiarie,
                InsuredDocumentNum = vehicle.InsuredDocumentNum,
                InsuredName = vehicle.InsuredName,
                VersionId = vehicle.VersionId,
                DocumentNumber = vehicle.DocumentNumber,
                EndorsementId = vehicle.EndorsementId,
                CoveredRiskType = vehicle.CoveredRiskType,
                InsuredId = vehicle.IndividualId
            };
        }

        internal static List<SelectDTO> CreateSelectVehicles(List<VHMO.VehicleDTO> Vehicles)
        {
            List<SelectDTO> vehiclesDTO = new List<SelectDTO>();

            foreach (VHMO.VehicleDTO vehicle in Vehicles)
            {
                vehiclesDTO.Add(CreateSelectVehicle(vehicle));
            }

            return vehiclesDTO;
        }

        internal static SelectDTO CreateSelectVehicle(VHMO.VehicleDTO vehicle)
        {
            if (vehicle == null)
            {
                return null;
            }

            return new SelectDTO
            {
                Id = vehicle.RiskId,
                Description = vehicle.Plate
            };
        }

        internal static SelectDTO CreateVehicleMake(VHMO.MakeDTO vehicleMake)
        {
            return new SelectDTO
            {
                Id = vehicleMake.Id,
                Description = vehicleMake.Description
            };
        }

        internal static List<SelectDTO> CreateVehicleMakes(List<VHMO.MakeDTO> vehicleMakes)
        {
            List<SelectDTO> VehicleMakesDTO = new List<SelectDTO>();

            foreach (VHMO.MakeDTO vehicleMake in vehicleMakes)
            {
                VehicleMakesDTO.Add(CreateVehicleMake(vehicleMake));
            }

            return VehicleMakesDTO;
        }

        internal static SelectDTO CreateVehicleModel(VHMO.ModelDTO vehicleModel)
        {
            return new SelectDTO
            {
                Id = vehicleModel.Id,
                Description = vehicleModel.Description
            };
        }

        internal static List<SelectDTO> CreateVehicleModels(List<VHMO.ModelDTO> vehicleModels)
        {
            List<SelectDTO> vehicleModelDTO = new List<SelectDTO>();

            foreach (VHMO.ModelDTO vehicleModel in vehicleModels)
            {
                vehicleModelDTO.Add(CreateVehicleModel(vehicleModel));
            }

            return vehicleModelDTO;
        }

        internal static SelectDTO CreateVehicleColor(VHMO.ColorDTO vehicleColor)
        {
            return new SelectDTO
            {
                Id = vehicleColor.Id,
                Description = vehicleColor.Description
            };
        }

        internal static List<SelectDTO> CreateVehicleColors(List<VHMO.ColorDTO> vehicleColors)
        {
            List<SelectDTO> vehicleColorDTO = new List<SelectDTO>();

            foreach (VHMO.ColorDTO vehicleColor in vehicleColors)
            {
                vehicleColorDTO.Add(CreateVehicleColor(vehicleColor));
            }

            return vehicleColorDTO;
        }

        internal static SelectDTO CreateVehicleYear(VHMO.YearDTO year)
        {
            return new SelectDTO
            {
                Id = Convert.ToInt32(year.Description),
                Description = year.Description
            };
        }

        internal static List<SelectDTO> CreateVehicleYears(List<VHMO.YearDTO> years)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (VHMO.YearDTO year in years)
            {
                selectDTO.Add(CreateVehicleYear(year));
            }

            return selectDTO.OrderByDescending(x => x.Description).ToList();
        }

        internal static SelectDTO CreateVersion(VHMO.VersionDTO version)
        {
            return new SelectDTO
            {
                Id = version.Id,
                Description = version.Description
            };
        }

        internal static List<SelectDTO> CreateVersions(List<VHMO.VersionDTO> versions)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (VHMO.VersionDTO version in versions)
            {
                selectDTO.Add(CreateVersion(version));
            }
            return selectDTO;
        }

        #endregion Vehicles

        #region Aircraft

        internal static SelectDTO CreateAircraftRegister(AIRMO.AircraftRegisterDTO register)
        {
            return new SelectDTO
            {
                Id = register.Id,
                Description = register.Description
            };
        }

        internal static List<SelectDTO> CreateAircraftRegisters(List<AIRMO.AircraftRegisterDTO> registers)
        {
            List<SelectDTO> aircraftResgistersDTO = new List<SelectDTO>();

            foreach (AIRMO.AircraftRegisterDTO register in registers)
            {
                aircraftResgistersDTO.Add(CreateAircraftRegister(register));
            }

            return aircraftResgistersDTO;
        }

        internal static SelectDTO CreateAircraftOperator(AIRMO.AircraftOperatorDTO @perator)
        {
            return new SelectDTO
            {
                Id = @perator.Id,
                Description = @perator.Description
            };
        }

        internal static List<SelectDTO> CreateAircraftOperators(List<AIRMO.AircraftOperatorDTO> operators)
        {
            List<SelectDTO> aircraftOperatorsDTO = new List<SelectDTO>();

            foreach (AIRMO.AircraftOperatorDTO @operator in operators)
            {
                aircraftOperatorsDTO.Add(CreateAircraftOperator(@operator));
            }

            return aircraftOperatorsDTO;
        }

        internal static SelectDTO CreateAircraftUse(AIRMO.AircraftUseDTO use)
        {
            return new SelectDTO
            {
                Id = use.Id,
                Description = use.Description
            };
        }

        internal static List<SelectDTO> CreateAircraftUses(List<AIRMO.AircraftUseDTO> uses)
        {
            List<SelectDTO> aircraftUsesDTO = new List<SelectDTO>();

            foreach (AIRMO.AircraftUseDTO use in uses)
            {
                aircraftUsesDTO.Add(CreateAircraftUse(use));
            }

            return aircraftUsesDTO;
        }

        internal static SelectDTO CreateAircraftModel(AIRMO.AicraftModelDTO model)
        {
            return new SelectDTO
            {
                Id = model.Id,
                Description = model.Description
            };
        }

        internal static List<SelectDTO> CreateAirCraftModels(List<AIRMO.AicraftModelDTO> models)
        {
            List<SelectDTO> aircraftModelsDTO = new List<SelectDTO>();

            foreach (AIRMO.AicraftModelDTO model in models)
            {
                aircraftModelsDTO.Add(CreateAircraftModel(model));
            }

            return aircraftModelsDTO;
        }

        internal static SelectDTO CreateAircraftMake(AIRMO.AircraftMakeDTO make)
        {
            return new SelectDTO
            {
                Id = make.Id,
                Description = make.Description
            };
        }

        internal static List<SelectDTO> CreateAircraftMakes(List<AIRMO.AircraftMakeDTO> makes)
        {
            List<SelectDTO> aircraftMakesDTO = new List<SelectDTO>();

            foreach (AIRMO.AircraftMakeDTO make in makes)
            {
                aircraftMakesDTO.Add(CreateAircraftMake(make));
            }

            return aircraftMakesDTO;
        }

        internal static AirCraftDTO CreateAircraft(AIRMO.AircraftDTO aircraft)
        {
            return new AirCraftDTO
            {
                MakeId = aircraft.MakeId,
                ModelId = aircraft.ModelId,
                TypeId = aircraft.TypeId,
                UseId = aircraft.UseId,
                RegisterId = aircraft.RegisterId,
                OperatorId = aircraft.OperatorId,
                RegisterNumber = aircraft.RegisterNumber,
                RiskId = aircraft.RiskId,
                Risk = aircraft.Risk,
                RiskNumber = aircraft.RiskNumber,
                CoveredRiskType = aircraft.CoveredRiskType,
                EndorsementId = aircraft.EndorsementId,
                InsuredId = aircraft.InsuredId,
                PolicyId = aircraft.PolicyId,
                PolicyDocumentNumber = aircraft.PolicyDocumentNumber,
                InsuredAmount = aircraft.InsuredAmount
            };
        }

        internal static List<AirCraftDTO> CreateAircrafts(List<AIRMO.AircraftDTO> aircrafts)
        {
            List<AirCraftDTO> airCraftsDTO = new List<AirCraftDTO>();

            foreach (AIRMO.AircraftDTO aircraft in aircrafts)
            {
                airCraftsDTO.Add(CreateAircraft(aircraft));
            }

            return airCraftsDTO;
        }

        internal static AirCraftDTO CreateMarine(MRMO.AirCraftDTO marine)
        {
            return new AirCraftDTO
            {
                UseId = marine.UseId,
                Use = marine.Use,
                OperatorId = marine.OperatorId,
                RegisterNumber = marine.RegisterNumber,
                RiskId = marine.RiskId,
                Risk = marine.Risk,
                RiskNumber = marine.RiskNumber,
                CoveredRiskType = marine.CoveredRiskType,
                EndorsementId = marine.EndorsementId,
                InsuredId = marine.InsuredId,
                PolicyId = marine.PolicyId,
                PolicyDocumentNumber = marine.PolicyDocumentNumber,
                InsuredAmount = marine.InsuredAmount
            };
        }

        internal static List<AirCraftDTO> CreateMarines(List<MRMO.AirCraftDTO> marines)
        {
            List<AirCraftDTO> airCraftsDTO = new List<AirCraftDTO>();

            foreach (MRMO.AirCraftDTO marine in marines)
            {
                airCraftsDTO.Add(CreateMarine(marine));
            }

            return airCraftsDTO;
        }

        #endregion Aircraft

        internal static List<CoInsuranceAssignedDTO> CreateCoInsuranceAssigneds(List<UNDMO.CoInsuranceAssignedDTO> coInsuranceAssignedDTOs)
        {
            List<CoInsuranceAssignedDTO> coInsuranceAssigneds = new List<CoInsuranceAssignedDTO>();

            foreach (UNDMO.CoInsuranceAssignedDTO coInsuranceAssignedDTO in coInsuranceAssignedDTOs)
            {
                coInsuranceAssigneds.Add(CreateCoInsuranceAssignedDTO(coInsuranceAssignedDTO));
            }

            return coInsuranceAssigneds;
        }

        internal static CoInsuranceAssignedDTO CreateCoInsuranceAssignedDTO(UNDMO.CoInsuranceAssignedDTO coInsuranceAssignedDTO)
        {
            return new CoInsuranceAssignedDTO
            {
                EndorsementId = coInsuranceAssignedDTO.EndorsementId,
                PolicyId = coInsuranceAssignedDTO.PolicyId,
                CompanyNum = coInsuranceAssignedDTO.CompanyNum,
                PartCiaPercentage = coInsuranceAssignedDTO.PartCiaPercentage,
                ExpensesPercentage = coInsuranceAssignedDTO.ExpensesPercentage,
                InsuranceCompanyId = coInsuranceAssignedDTO.InsuranceCompanyId

            };
        }

        internal static CLMWOR.CoInsuranceAssignedDTO CreateAccountingCoInsuranceAssigned(CoInsuranceAssignedDTO coInsuranceAssignedDTO)
        {
            return new CLMWOR.CoInsuranceAssignedDTO
            {
                EndorsementId = coInsuranceAssignedDTO.EndorsementId,
                PolicyId = coInsuranceAssignedDTO.PolicyId,
                CompanyNum = coInsuranceAssignedDTO.CompanyNum,
                PartCiaPercentage = coInsuranceAssignedDTO.PartCiaPercentage,
                ExpensesPercentage = coInsuranceAssignedDTO.ExpensesPercentage,
                InsuranceCompanyId = coInsuranceAssignedDTO.InsuranceCompanyId

            };
        }

        internal static List<CLMWOR.CoInsuranceAssignedDTO> CreateAccountingCoInsuranceAssigneds(List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs)
        {
            List<CLMWOR.CoInsuranceAssignedDTO> coInsuranceAssigneds = new List<CLMWOR.CoInsuranceAssignedDTO>();

            foreach (CoInsuranceAssignedDTO coInsuranceAssignedDTO in coInsuranceAssignedDTOs)
            {
                coInsuranceAssigneds.Add(CreateAccountingCoInsuranceAssigned(coInsuranceAssignedDTO));
            }

            return coInsuranceAssigneds;
        }

        #endregion

        #region Recovery

        internal static RecoveryDTO CreateRecovery(Recovery recovery)
        {
            if (recovery == null)
            {
                return null;
            }
            else
            {
                RecoveryDTO recoveryDTO = new RecoveryDTO
                {
                    Id = recovery.Id,
                    CreatedDate = recovery.CreatedDate,
                    Description = recovery.Description,
                    CancellationReasonId = Convert.ToInt32(recovery.CancellationReason?.Id),
                    CancellationDate = recovery.CancellationDate,
                    RecuperatorId = Convert.ToInt32(recovery.Recuperator?.Id),
                    RecuperatorFullName = Convert.ToString(recovery.Recuperator?.FullName),
                    RecuperatorDocumentNumber = Convert.ToString(recovery.Recuperator?.DocumentNumber),
                    RecoveryTypeId = Convert.ToInt32(recovery.RecoveryType?.Id),
                    RecoveryTypeDescription = Convert.ToString(recovery.RecoveryType?.Description),
                    PrescriptionDate = recovery.PrescriptionDate,
                    Voucher = recovery.Voucher,
                    LossResponsible = recovery.LossResponsible,
                    AssignedCourt = recovery.AssignedCourt,
                    ExpedientNumber = recovery.ExpedientNumber,
                    AttorneyAssignmentDate = recovery.AttorneyAssingmentDate,
                    LastReportDate = recovery.LastReportDate,
                    DebtorId = Convert.ToInt32(recovery.Debtor?.Id),
                    DebtorFullName = Convert.ToString(recovery.Debtor?.FullName),
                    DebtorDocumentNumber = Convert.ToString(recovery.Debtor?.DocumentNumber),
                    DebtorAddress = Convert.ToString(recovery.Debtor?.Address),
                    DebtorPhone = Convert.ToString(recovery.Debtor?.Phone),
                    TotalAmount = recovery.TotalAmmount,
                    CompanyId = recovery.CompanyId,
                    ClaimId = recovery.ClaimId,
                    SubClaimId = recovery.SubClaimId,
                    PrefixId = Convert.ToInt32(recovery.Prefix?.Id),
                    BranchId = Convert.ToInt32(recovery.Branch?.Id),
                    ClaimNumber = recovery.ClaimNumber,
                    RecoveryClassId = recovery.RecoveryClassId,
                    DebtorIsParticipant = recovery.DebtorIsParticipant,
                    RecuperatorNameDocument = Convert.ToString(recovery.Recuperator?.DocumentNumber) + "-" + Convert.ToString(recovery.Recuperator?.FullName),
                    RecoveryAmount = recovery.RecoveryAmount
                };

                if (recovery.PaymentPlan != null)
                {
                    recoveryDTO.PaymentPlanId = recovery.PaymentPlan.Id;
                    recoveryDTO.CurrencyId = recovery.PaymentPlan.Currency?.Id;
                    recoveryDTO.PaymentClassId = recovery.PaymentPlan.PaymentClass?.Id;
                    recoveryDTO.TaxPercentage = recovery.PaymentPlan.Tax;
                    recoveryDTO.PaymentQuotas = recovery.PaymentPlan.PaymentQuotas != null ? CreatePaymentQuotas(recovery.PaymentPlan.PaymentQuotas) : new List<PaymentQuotaDTO>();
                }

                return recoveryDTO;
            }
        }
        internal static List<RecoveryDTO> CreateRecoveries(List<Recovery> recoveries)
        {
            List<RecoveryDTO> recoveriesDTO = new List<RecoveryDTO>();

            foreach (Recovery recovery in recoveries)
            {
                recoveriesDTO.Add(CreateRecovery(recovery));
            }

            return recoveriesDTO;
        }

        internal static RecoveryTypeDTO CreateRecoveryType(RecoveryType recoveryType)
        {
            return new RecoveryTypeDTO
            {
                Id = recoveryType.Id,
                Description = recoveryType.Description
            };
        }

        internal static List<RecoveryTypeDTO> CreateRecoveryTypes(List<RecoveryType> recoveryTypes)
        {
            List<RecoveryTypeDTO> recoveryTypeDTO = new List<RecoveryTypeDTO>();

            foreach (RecoveryType recoveryType in recoveryTypes)
            {
                recoveryTypeDTO.Add(CreateRecoveryType(recoveryType));
            }
            return recoveryTypeDTO;
        }

        internal static PaymentQuotaDTO CreatePaymentQuota(PaymentQuota paymentQuota)
        {
            return new PaymentQuotaDTO
            {
                Id = paymentQuota.Id,
                Amount = paymentQuota.Amount,
                ExpirationDate = paymentQuota.ExpirationDate,
                Number = paymentQuota.Number
            };
        }

        internal static List<PaymentQuotaDTO> CreatePaymentQuotas(List<PaymentQuota> paymentQuotas)
        {
            List<PaymentQuotaDTO> paymentQuotasDTO = new List<PaymentQuotaDTO>();

            foreach (PaymentQuota paymentQuota in paymentQuotas)
            {
                paymentQuotasDTO.Add(CreatePaymentQuota(paymentQuota));
            }

            return paymentQuotasDTO;
        }
        #endregion

        #region PaymentRequest

        internal static List<PaymentSourceDTO> CreatePaymentSources(List<CLMWOR.AccountingConcept.ConceptSourceDTO> conceptSources)
        {
            List<PaymentSourceDTO> paymentSourceDTO = new List<PaymentSourceDTO>();

            foreach (CLMWOR.AccountingConcept.ConceptSourceDTO conceptSource in conceptSources)
            {
                paymentSourceDTO.Add(CreatePaymentSource(conceptSource));
            }

            return paymentSourceDTO.OrderBy(x => x.Description).ToList();
        }

        private static PaymentSourceDTO CreatePaymentSource(CLMWOR.AccountingConcept.ConceptSourceDTO conceptSource)
        {
            return new PaymentSourceDTO
            {
                Id = conceptSource.Id,
                Description = conceptSource.Description
            };
        }

        internal static List<PaymentMovementTypeDTO> CreateMovementTypes(List<MovementType> movementTypes)
        {
            List<PaymentMovementTypeDTO> movementTypeDTO = new List<PaymentMovementTypeDTO>();

            foreach (MovementType movementType in movementTypes)
            {
                movementTypeDTO.Add(CreateMovementType(movementType));
            }

            return movementTypeDTO.OrderBy(x => x.Description).ToList();
        }

        private static PaymentMovementTypeDTO CreateMovementType(MovementType movementType)
        {
            return new PaymentMovementTypeDTO
            {
                Id = movementType.Id,
                Description = movementType.Description
            };
        }

        internal static List<RoleDTO> CreateRole(List<Role> roles)
        {
            List<RoleDTO> roleDTO = new List<RoleDTO>();

            foreach (Role role in roles)
            {
                roleDTO.Add(CreateRole(role));
            }

            return roleDTO.OrderBy(x => x.Description).ToList();
        }

        private static RoleDTO CreateRole(Role role)
        {
            return new RoleDTO
            {
                Id = role.Id,
                Description = role.Description
            };
        }

        internal static List<VoucherTypeDTO> CreateVoucherTypes(List<VoucherType> voucherTypes)
        {
            List<VoucherTypeDTO> voucherTypeDTO = new List<VoucherTypeDTO>();

            foreach (VoucherType voucherType in voucherTypes)
            {
                voucherTypeDTO.Add(CreateVoucherType(voucherType));
            }

            return voucherTypeDTO.OrderBy(x => x.Description).ToList();
        }

        private static VoucherTypeDTO CreateVoucherType(VoucherType voucherType)
        {
            return new VoucherTypeDTO
            {
                Id = voucherType.Id,
                Description = voucherType.Description
            };
        }

        internal static List<PaymentMethodDTO> CreatePaymentMethods(List<ClaimPaymentMethod> paymentMethods)
        {
            List<PaymentMethodDTO> paymentMethodDTO = new List<PaymentMethodDTO>();

            foreach (ClaimPaymentMethod paymentMethod in paymentMethods)
            {
                paymentMethodDTO.Add(CreatePaymentMethod(paymentMethod));
            }

            return paymentMethodDTO.OrderBy(x => x.Description).ToList();
        }

        private static PaymentMethodDTO CreatePaymentMethod(ClaimPaymentMethod paymentMethod)
        {
            return new PaymentMethodDTO
            {
                Id = paymentMethod.Id,
                Description = paymentMethod.Description
            };
        }

        internal static List<TaxDTO> CreateTaxs(List<TaxServices.Models.Tax> taxs)
        {
            List<TaxDTO> taxDTO = new List<TaxDTO>();

            foreach (TaxServices.Models.Tax tax in taxs)
            {
                taxDTO.Add(CreateTax(tax));
            }

            return taxDTO.OrderBy(x => x.Description).ToList();
        }

        private static TaxDTO CreateTax(TaxServices.Models.Tax Tax)
        {
            return new TaxDTO
            {
                Id = Tax.Id,
                Description = Tax.Description
            };
        }

        internal static List<ExchangeRateDTO> CreateExchangeRates(List<ExchangeRate> exchangeRates)
        {
            List<ExchangeRateDTO> exchangeRatesDTO = new List<ExchangeRateDTO>();

            foreach (ExchangeRate exchangeRate in exchangeRates)
            {
                exchangeRatesDTO.Add(CreateExchangeRate(exchangeRate));
            }

            return exchangeRatesDTO.OrderBy(x => x.RateDate).ToList();
        }

        internal static ExchangeRateDTO CreateExchangeRate(ExchangeRate exchangeRate)
        {
            return new ExchangeRateDTO
            {
                RateDate = exchangeRate.RateDate,
                Currency = new CurrencyDTO
                {
                    Id = exchangeRate.Currency.Id,
                    Description = exchangeRate.Currency.Description,
                    SmallDescription = exchangeRate.Currency.SmallDescription,
                    TinyDescription = exchangeRate.Currency.TinyDescription,
                },
                BuyAmount = exchangeRate.BuyAmount,
                SellAmount = exchangeRate.SellAmount
            };
        }

        #region PERSON TYPE
        internal static List<PersonTypeDTO> CreatePersonTypes(List<ClaimPersonType> personTypes)
        {
            List<PersonTypeDTO> personTypeDTO = new List<PersonTypeDTO>();

            foreach (ClaimPersonType personType in personTypes)
            {
                personTypeDTO.Add(CreatePersonType(personType));
            }

            return personTypeDTO.OrderBy(x => x.Description).ToList();
        }

        internal static PersonTypeDTO CreatePersonType(ClaimPersonType personType)
        {
            return new PersonTypeDTO
            {
                Id = personType.Id,
                Description = personType.Description,
                Enable = personType.IsEnabled,
                BillEnabled = personType.IsBillEnabled,
                PaymentOrderEnable = personType.IsPaymentOrderEnabled,
                PreaplicationEnables = personType.IsPreaplicationEnabled
            };
        }
        #endregion

        internal static List<SelectDTO> CreateMovementTypesSelect(List<CLMWOR.AccountingConcept.MovementTypeDTO> movementTypes)
        {
            List<SelectDTO> selectsDTO = new List<SelectDTO>();

            foreach (CLMWOR.AccountingConcept.MovementTypeDTO movementType in movementTypes)
            {
                selectsDTO.Add(CreateMovementTypeSelect(movementType));
            }

            return selectsDTO.OrderBy(x => x.Id).ToList();
        }

        internal static SelectDTO CreateMovementTypeSelect(CLMWOR.AccountingConcept.MovementTypeDTO movementType)
        {
            return new SelectDTO
            {
                Id = movementType.Id,
                Description = movementType.Description
            };
        }


        internal static List<SelectDTO> CreateAccountingConceptsSelect(List<CLMWOR.AccountingConcept.AccountingConceptDTO> accountingConcepts)
        {
            List<SelectDTO> selectsDTO = new List<SelectDTO>();

            foreach (CLMWOR.AccountingConcept.AccountingConceptDTO accountingConcept in accountingConcepts)
            {
                selectsDTO.Add(CreateAccountingConceptSelect(accountingConcept));
            }

            return selectsDTO.OrderBy(x => x.Id).ToList();
        }

        internal static SelectDTO CreateAccountingConceptSelect(CLMWOR.AccountingConcept.AccountingConceptDTO accountingConcept)
        {
            return new SelectDTO
            {
                Id = accountingConcept.Id,
                Description = accountingConcept.Description
            };
        }

        internal static AccountBankDTO CreateAccounBankDTO(AccountBank accountBank)
        {

            return new AccountBankDTO
            {
                Id = accountBank.Id,
                AccountTypeId = Convert.ToInt32(accountBank.AccountTypeId),
                Number = accountBank.Number,
                BankId = Convert.ToInt32(accountBank.BankId),
                Enabled = Convert.ToBoolean(accountBank.Enabled),
                Default = Convert.ToBoolean(accountBank.Default),
                CurrencyId = Convert.ToInt32(accountBank.CurrencyId),
                GeneralLedgerId = Convert.ToInt32(accountBank.GeneralLedgerId),
                DisabledDate = Convert.ToDateTime(accountBank.DisabledDate),
                BranchId = Convert.ToInt32(accountBank.BranchId)
            };
        }
        internal static List<AccountBankDTO> CreateAccounBankDTOs(List<AccountBank> accountBanks)
        {
            List<AccountBankDTO> accountBankDTO = new List<AccountBankDTO>();

            foreach (AccountBank accountBank in accountBanks)
            {
                accountBankDTO.Add(CreateAccounBankDTO(accountBank));
            }

            return accountBankDTO;
        }

        internal static PaymentRequestDTO CreatePaymentRequest(PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
            {
                return null;
            }

            PaymentRequestDTO paymentRequestDTO = new PaymentRequestDTO
            {
                Id = paymentRequest.Id,
                TemporalId = paymentRequest.TemporalId,
                Description = paymentRequest.Description,
                AccountBankId = paymentRequest.AccountBankId,
                BranchId = Convert.ToInt32(paymentRequest.Branch?.Id),
                BranchDescription = paymentRequest.Branch?.Description,
                CurrencyId = Convert.ToInt32(paymentRequest.Currency?.Id),
                CurrencyDescription = paymentRequest.Currency?.Description,
                EstimatedDate = paymentRequest.EstimatedDate,
                IndividualId = paymentRequest.IndividualId,
                IsPrinted = paymentRequest.IsPrinted,
                MovementTypeId = Convert.ToInt32(paymentRequest.MovementType?.Id),
                Number = paymentRequest.Number,
                AccountingDate = paymentRequest.AccountingDate,
                PersonTypeId = Convert.ToInt32(paymentRequest.PersonType?.Id),
                PrefixId = Convert.ToInt32(paymentRequest.Prefix?.Id),
                PrefixDescription = paymentRequest.Prefix?.Description,
                RegistrationDate = paymentRequest.RegistrationDate,
                TotalAmount = paymentRequest.TotalAmount,
                PaymentRequestTypeId = Convert.ToInt32(paymentRequest.Type?.Id),
                UserId = paymentRequest.UserId,
                PaymentSourceId = Convert.ToInt32(paymentRequest.MovementType?.Source?.Id),
                PaymentMethodId = Convert.ToInt32(paymentRequest.PaymentMethod?.Id),
                PaymentMethodDescription = paymentRequest.PaymentMethod?.Description,
                AuthorizationPolicies = paymentRequest.AuthorizationPolicies,
                CoInsurance = CreateCoInsurances(paymentRequest.CoInsurance),
                TechnicalTransaction = paymentRequest.TechnicalTransaction,
                PolicySalePointId = paymentRequest.SalePointId,
                ClaimNumber = paymentRequest.Claims != null && paymentRequest.Claims.Count > 0 ? paymentRequest.Claims.FirstOrDefault().Number : 0
            };

            paymentRequestDTO.Claims = new List<SubClaimDTO>();
            paymentRequestDTO.Claims.AddRange(CreateClaimsToSubClaims(paymentRequest.Claims == null ? null : paymentRequest.Claims));
            paymentRequestDTO.TotalTax = Convert.ToDecimal(paymentRequest.Claims?.Sum(y => y.Vouchers?.Sum(z => z.Concepts.Sum(x => x.VoucherConceptTaxes == null ? 0 : x.VoucherConceptTaxes.Sum(v => v.TaxValue + v.Retention)))));

            return paymentRequestDTO;
        }

        internal static PaymentRequestReportDTO CreatePaymentRequestReport(PaymentRequest paymentRequest)
        {
            List<ClaimReportDTO> claims = new List<ClaimReportDTO>();
            List<TaxReportDTO> taxes = new List<TaxReportDTO>();

            paymentRequest.Claims.ForEach(claim => {
                claims.Add(CreateClaimsReport(claim));
                taxes.AddRange(CreateTaxesReport(claim.Vouchers));
            });

            return new PaymentRequestReportDTO
            {
                Number = paymentRequest.Number.ToString(),
                ReportDate = DateTime.Now.ToShortDateString(),
                Branch = paymentRequest.Branch.Description,
                ContractCity = paymentRequest.Branch.Description,
                PolicyAgent = null, //
                Prefix = paymentRequest.Prefix.Description,
                PolicyNumber = paymentRequest.Claims.FirstOrDefault().Endorsement.PolicyNumber,
                ClaimNumber = paymentRequest.Claims.FirstOrDefault().Number.ToString(),
                ClaimRegistrationDate = paymentRequest.Claims.FirstOrDefault().OccurrenceDate.ToShortDateString(),
                PolicyHolder = null, //
                PolicyInsured = null, //
                PaymentBeneficiaryPersonType = paymentRequest.PersonType.Description,
                PaymentBeneficiaryName = null, //
                PaymentBeneficiaryDocumentNumber = null, //
                PaymentTechnicalTransaction = paymentRequest.TechnicalTransaction.ToString(),
                VoucherType = paymentRequest.Claims.FirstOrDefault()?.Vouchers.FirstOrDefault()?.VoucherType.Description,
                PaymentMethod = paymentRequest.PaymentMethod.Description,
                PaymentCurrency = paymentRequest.Currency.Description,
                VoucherCurrency = paymentRequest.Claims.FirstOrDefault()?.Vouchers.FirstOrDefault()?.Currency.Description,
                CostCenter = paymentRequest.Branch.Description,
                PaymentTotalAmount = paymentRequest.TotalAmount.ToCurrency(),
                TRM = null, //
                TotalAmountConcepts = paymentRequest.Claims.FirstOrDefault()?.Vouchers.Sum(x => x.Concepts.Sum(y => (y.Value + y.TaxValue))).ToCurrency(),
                PaymentDescription = paymentRequest.Description,
                Claims = claims,
                Taxes = taxes,
                Coinsurances = CreateCoInsurancesReport(paymentRequest.CoInsurance)
            };
        }

        internal static ClaimReportDTO CreateClaimsReport(Claim claim)
        {
            return new ClaimReportDTO
            {
                ClaimNumber = claim.Number.ToString(),
                SubClaim = claim.Modifications.First().Coverages.First().SubClaim.ToString(),
                BusinessTurn = "0",
                Coverage = claim.Modifications.First().Coverages.First().Description,
                Deducible = claim.Modifications.First().Coverages.First().Estimations.First().DeductibleAmount.ToCurrency(),
                Compensation = claim.Modifications.First().Coverages.First().Estimations.First().Type.Id == 1 ? claim.Modifications.First().Coverages.First().Estimations.First().AmountAccumulate.ToCurrency() : new decimal(0.00).ToCurrency(),
                Expenses = claim.Modifications.First().Coverages.First().Estimations.First().Type.Id != 1 ? claim.Modifications.First().Coverages.First().Estimations.First().AmountAccumulate.ToCurrency() : new decimal(0.00).ToCurrency(),
                Reinsurance = null,
            };
        }

        internal static List<TaxReportDTO> CreateTaxesReport(List<Voucher> vouchers)
        {
            List<TaxReportDTO> taxes = new List<TaxReportDTO>();

            foreach (Voucher voucher in vouchers)
            {
                foreach (VoucherConcept concept in voucher.Concepts)
                {
                    if (concept.VoucherConceptTaxes != null)
                    {
                        foreach (VoucherConceptTax tax in concept.VoucherConceptTaxes)
                        {
                            taxes.Add(CreateTaxesReport(tax));
                        }
                    }
                }
            }

            return taxes;
        }

        internal static TaxReportDTO CreateTaxesReport(VoucherConceptTax tax)
        {
            return new TaxReportDTO
            {
                TaxCode = tax.Id.ToString(),
                TaxCategory = tax.CategoryDescription,
                TaxDescription = tax.TaxDescription,
                TaxBaseAmount = tax.TaxBaseAmount.ToCurrency(),
                TaxValue = tax.TaxValue.ToCurrency()
            };
        }

        internal static List<CoInsuranceReportDTO> CreateCoInsurancesReport(List<PaymentRequestCoInsurance> coInsurances)
        {
            List<CoInsuranceReportDTO> coinsurances = new List<CoInsuranceReportDTO>();

            foreach (PaymentRequestCoInsurance coInsurance in coInsurances)
            {
                coinsurances.Add(CreateCoInsuranceReport(coInsurance));
            }

            return coinsurances;
        }

        internal static CoInsuranceReportDTO CreateCoInsuranceReport(PaymentRequestCoInsurance coInsurance)
        {
            return new CoInsuranceReportDTO
            {
                Amount = coInsurance.Amount.ToCurrency(),
                Participation = (coInsurance.PartCiaPct / 100).ToString("P"),
                Company = coInsurance.Company,
            };
        }

        internal static List<AccountingReportDTO> CreateAccountingsReport(List<GLWKDTO.JournalEntryItemDTO> journalEntryItemsDTO)
        {
            List<AccountingReportDTO> accountingReports = new List<AccountingReportDTO>();

            foreach (GLWKDTO.JournalEntryItemDTO journalEntryItemDTO in journalEntryItemsDTO)
            {
                accountingReports.Add(CreateAccountingReport(journalEntryItemDTO));
            }

            return accountingReports;
        }

        internal static AccountingReportDTO CreateAccountingReport(GLWKDTO.JournalEntryItemDTO journalEntryItemDTO)
        {
            return new AccountingReportDTO
            {
                Account = journalEntryItemDTO.AccountingAccount.Number,
                Description = journalEntryItemDTO.AccountingAccount.Description,
                CreditAmount = journalEntryItemDTO.AccountingNature == 1 ? journalEntryItemDTO.Amount.Value.ToCurrency() : "0.00",
                DebitAmount = journalEntryItemDTO.AccountingNature == 2 ? journalEntryItemDTO.Amount.Value.ToCurrency() : "0.00"
            };
        }

        internal static List<PaymentRequestDTO> CreatePaymentRequests(List<PaymentRequest> paymentRequests)
        {
            List<PaymentRequestDTO> paymentRequestsDTO = new List<PaymentRequestDTO>();

            foreach (PaymentRequest paymentRequest in paymentRequests)
            {
                paymentRequestsDTO.Add(CreatePaymentRequest(paymentRequest));
            }

            return paymentRequestsDTO;
        }

        internal static ChargeRequestDTO CreateChargeRequest(ChargeRequest chargeRequest)
        {
            if (chargeRequest == null)
            {
                return null;
            }

            ChargeRequestDTO chargeRequestDTO = new ChargeRequestDTO
            {
                Id = chargeRequest.Id,
                Description = chargeRequest.Description,
                AccountBankId = chargeRequest.AccountBankId,
                BranchId = (chargeRequest.Branch != null) ? chargeRequest.Branch.Id : 0,
                BranchDescription = chargeRequest.Branch?.Description,
                CurrencyId = (chargeRequest.Currency != null) ? chargeRequest.Currency.Id : 0,
                EstimatedDate = chargeRequest.EstimatedDate,
                IndividualId = chargeRequest.IndividualId,
                IsPrinted = chargeRequest.IsPrinted,
                MovementTypeId = (chargeRequest.MovementType != null) ? chargeRequest.MovementType.Id : 0,
                Number = chargeRequest.Number,
                AccountingDate = chargeRequest.AccountingDate,
                PersonTypeId = (chargeRequest.PersonType != null) ? chargeRequest.PersonType.Id : 0,
                PrefixId = (chargeRequest.Prefix != null) ? chargeRequest.Prefix.Id : 0,
                PrefixDescription = chargeRequest.Prefix?.Description,
                RegistrationDate = chargeRequest.RegistrationDate,
                RecoveryOrSalvageAmount = chargeRequest.RecoveryOrSalvageAmount,
                TotalAmount = chargeRequest.TotalAmount,
                PaymentRequestTypeId = (chargeRequest.Type != null) ? chargeRequest.Type.Id : 0,
                UserId = chargeRequest.UserId,
                PaymentSourceId = (chargeRequest.MovementType != null) ? chargeRequest.MovementType.Source.Id : 0,
                PaymentMethodId = (chargeRequest.PaymentMethod != null) ? chargeRequest.PaymentMethod.Id : 0,
                SalvageId = chargeRequest.SalvageId,
                RecoveryId = chargeRequest.RecoveryId,
                Voucher = CreateVoucher(chargeRequest.Voucher)
            };

            if (chargeRequest.Claim != null)
            {
                chargeRequestDTO.ClaimId = chargeRequest.Claim.Id;
                chargeRequestDTO.ClaimNumber = chargeRequest.Claim.Number;
                chargeRequestDTO.ClaimBranchId = Convert.ToInt32(chargeRequest.Claim.Branch?.Id);
                chargeRequestDTO.ClaimBranchDescription = chargeRequest.Claim.Branch?.Description;
                chargeRequestDTO.PolicyDocumentNumber = chargeRequest.Claim.Endorsement?.PolicyNumber;

                if (Convert.ToInt32(chargeRequest.Claim.Modifications?.LastOrDefault()?.Coverages?.Count) > 0)
                {
                    chargeRequestDTO.SubClaim = chargeRequest.Claim.Modifications.Last().Coverages.First().SubClaim;
                    chargeRequestDTO.RiskId = chargeRequest.Claim.Modifications.Last().Coverages.First().RiskId;
                }
            }

            return chargeRequestDTO;
        }

        internal static List<ChargeRequestDTO> CreateChargeRequests(List<ChargeRequest> chargeRequests)
        {
            List<ChargeRequestDTO> chargeRequestsDTO = new List<ChargeRequestDTO>();

            foreach (ChargeRequest chargeRequest in chargeRequests)
            {
                chargeRequestsDTO.Add(CreateChargeRequest(chargeRequest));
            }

            return chargeRequestsDTO;
        }

        internal static List<VoucherDTO> CreateVouchers(List<Voucher> vouchers)
        {
            List<VoucherDTO> vouchersDTO = new List<VoucherDTO>();

            foreach (Voucher voucher in vouchers)
            {
                vouchersDTO.Add(CreateVoucher(voucher));
            }

            return vouchersDTO.OrderBy(x => x.Id).ToList();
        }

        internal static VoucherDTO CreateVoucher(Voucher voucher)
        {
            if (voucher == null)
            {
                return null;
            }

            return new VoucherDTO
            {
                Id = voucher.Id,
                Number = voucher.Number,
                Date = voucher.Date,
                ExchangeRate = voucher.ExchangeRate,
                VoucherTypeId = voucher.VoucherType.Id,
                VoucherTypeDescription = voucher.VoucherType.Description,
                CurrencyId = voucher.Currency.Id,
                CurrencyDescription = voucher.Currency.Description,
                Concepts = voucher.Concepts != null ? CreateVoucherConcepts(voucher.Concepts) : new List<VoucherConceptDTO>()
            };
        }

        internal static List<VoucherConceptDTO> CreateVoucherConcepts(List<VoucherConcept> voucherConcepts)
        {
            List<VoucherConceptDTO> voucherConceptsDTO = new List<VoucherConceptDTO>();

            foreach (VoucherConcept voucherConcept in voucherConcepts)
            {
                voucherConceptsDTO.Add(CreateVoucherConcept(voucherConcept));
            }

            return voucherConceptsDTO.OrderBy(x => x.Id).ToList();
        }

        internal static VoucherConceptDTO CreateVoucherConcept(VoucherConcept voucherConcept)
        {
            return new VoucherConceptDTO
            {
                Id = voucherConcept.Id,
                Value = voucherConcept.Value,
                TaxValue = voucherConcept.TaxValue,
                Retention = voucherConcept.Retention,
                CostCenter = voucherConcept.CostCenter,
                PaymentConceptId = voucherConcept.PaymentConcept.Id,
                PaymentConcept = voucherConcept.PaymentConcept.Description,
                ConceptTaxes = voucherConcept.VoucherConceptTaxes != null ? CreateVoucherConceptTaxes(voucherConcept.VoucherConceptTaxes) : new List<VoucherConceptTaxDTO>()
            };
        }

        internal static List<VoucherConceptTaxDTO> CreateVoucherConceptTaxes(List<VoucherConceptTax> voucherConceptTaxes)
        {
            List<VoucherConceptTaxDTO> voucherConceptTaxesDTO = new List<VoucherConceptTaxDTO>();

            foreach (VoucherConceptTax voucherConceptTax in voucherConceptTaxes)
            {
                voucherConceptTaxesDTO.Add(CreateVoucherConceptTax(voucherConceptTax));
            }

            return voucherConceptTaxesDTO.OrderBy(x => x.Id).ToList();
        }

        internal static VoucherConceptTaxDTO CreateVoucherConceptTax(VoucherConceptTax voucherConceptTax)
        {
            return new VoucherConceptTaxDTO
            {
                Id = voucherConceptTax.Id,
                TaxDescription = voucherConceptTax.TaxDescription,
                TaxValue = voucherConceptTax.TaxValue,
                CategoryId = voucherConceptTax.CategoryId,
                CategoryDescription = voucherConceptTax.CategoryDescription,
                ConditionId = voucherConceptTax.ConditionId,
                ConditionDescription = voucherConceptTax.ConditionDescription,
                TaxBaseAmount = voucherConceptTax.TaxBaseAmount,
                TaxRate = voucherConceptTax.TaxRate,
                TaxId = voucherConceptTax.TaxId,
                Retention = voucherConceptTax.Retention,
                AccountingConceptId = voucherConceptTax.AccountingConceptId
            };
        }

        internal static List<SelectDTO> CreateSelectPaymentConcepts(List<PaymentConcept> paymentConcepts)
        {
            List<SelectDTO> selectsDTO = new List<SelectDTO>();

            foreach (PaymentConcept paymentConcept in paymentConcepts)
            {
                selectsDTO.Add(CreateSelectPaymentConcept(paymentConcept));
            }

            return selectsDTO.OrderBy(x => x.Id).ToList();
        }

        internal static SelectDTO CreateSelectPaymentConcept(PaymentConcept paymentConcept)
        {
            return new SelectDTO
            {
                Id = paymentConcept.Id,
                Description = paymentConcept.Description
            };
        }

        internal static PaymentConceptDTO CreatePaymentConcept(PaymentConcept paymentConcept)
        {
            return new PaymentConceptDTO
            {
                Id = paymentConcept.Id,
                Description = paymentConcept.Description
            };
        }

        internal static List<PaymentConceptDTO> CreatePaymentConcepts(List<PaymentConcept> paymentConcepts)
        {
            List<PaymentConceptDTO> paymentConceptsDTO = new List<PaymentConceptDTO>();

            foreach (PaymentConcept paymentConcept in paymentConcepts)
            {
                paymentConceptsDTO.Add(CreatePaymentConcept(paymentConcept));
            }

            return paymentConceptsDTO;
        }

        internal static List<CurrencyDTO> CreateCurrencies(List<Currency> currencies)
        {
            List<CurrencyDTO> currencyDTO = new List<CurrencyDTO>();

            foreach (Currency currency in currencies)
            {
                currencyDTO.Add(CreatePaymentCurrency(currency));
            }

            return currencyDTO;
        }

        private static CurrencyDTO CreatePaymentCurrency(Currency currency)
        {
            return new CurrencyDTO
            {
                Id = currency.Id,
                Description = currency.Description
            };
        }

        internal static CLMWOR.AccountingPaymentRequestDTO CreateAccountingPaymentRequest(PaymentRequestDTO paymentRequestDTO, List<CoInsuranceAssignedDTO> coInsuranceAssignedsDTO, int technicalTransaction)
        {
            CLMWOR.AccountingPaymentRequestDTO accountingPaymentRequest = new CLMWOR.AccountingPaymentRequestDTO
            {
                AccountingDate = paymentRequestDTO.AccountingDate,
                Description = paymentRequestDTO.Description,
                Amount = paymentRequestDTO.Claims.Sum(x => x.Vouchers.Sum(y => y.Concepts.Sum(z => z.Value))),
                BranchId = paymentRequestDTO.BranchId,
                Id = paymentRequestDTO.Id,
                IndividualId = paymentRequestDTO.IndividualId,
                Number = paymentRequestDTO.Number,
                PrefixId = paymentRequestDTO.PrefixId,
                UserId = paymentRequestDTO.UserId,
                TechnicalTransaction = technicalTransaction,
                CurrencyId = paymentRequestDTO.CurrencyId,
                ExchangeRate = paymentRequestDTO.ExchangeRate,
                SalePointId = paymentRequestDTO.PolicySalePointId,
                PaymentSourceId = paymentRequestDTO.PaymentSourceId,
                BusinessTypeId = paymentRequestDTO.BusinessTypeId,
                Vouchers = new List<CLMWOR.VoucherDTO>(),
                CoInsuranceAssigneds = coInsuranceAssignedsDTO.Any() ? CreateAccountingCoInsuranceAssigneds(coInsuranceAssignedsDTO) : new List<CLMWOR.CoInsuranceAssignedDTO>()
            };

            paymentRequestDTO.Claims.ForEach(x => { accountingPaymentRequest.Vouchers.AddRange(CreateVouchers(x.Vouchers));  });

            return accountingPaymentRequest;
        }

        internal static CLMWOR.AccountingPaymentRequestDTO CreateAccountingChargeRequest(ChargeRequestDTO chargeRequestDTO, int technicalTransaction)
        {
            return new CLMWOR.AccountingPaymentRequestDTO
            {
                AccountingDate = chargeRequestDTO.AccountingDate,
                Description = chargeRequestDTO.Description,
                Amount = chargeRequestDTO.Voucher.Concepts.Sum(x => x.Value),
                BranchId = chargeRequestDTO.BranchId,
                Id = chargeRequestDTO.Id,
                IndividualId = chargeRequestDTO.IndividualId,
                Number = chargeRequestDTO.Number,
                PrefixId = chargeRequestDTO.PrefixId,
                UserId = chargeRequestDTO.UserId,
                TechnicalTransaction = technicalTransaction,
                CurrencyId = chargeRequestDTO.CurrencyId,
                ExchangeRate = 1,
                SalePointId = 0,
                PaymentSourceId = chargeRequestDTO.PaymentSourceId,
                BusinessTypeId = 1,
                Vouchers = new List<CLMWOR.VoucherDTO> {
                    CreateVoucher(chargeRequestDTO.Voucher)
                },
                CoInsuranceAssigneds = new List<CLMWOR.CoInsuranceAssignedDTO>(),
                SalvageId = Convert.ToInt32(chargeRequestDTO.SalvageId),
                RecoveryId = Convert.ToInt32(chargeRequestDTO.RecoveryId)
            };
        }

        private static List<CLMWOR.VoucherDTO> CreateVouchers(List<VoucherDTO> vouchers)
        {
            List<CLMWOR.VoucherDTO> voucherDTOs = new List<CLMWOR.VoucherDTO>();

            foreach (VoucherDTO voucherDTO in vouchers)
            {
                voucherDTOs.Add(CreateVoucher(voucherDTO));
            }

            return voucherDTOs;
        }

        private static CLMWOR.VoucherDTO CreateVoucher(VoucherDTO voucherDTO)
        {
            return new CLMWOR.VoucherDTO
            {
                Id = voucherDTO.Id,
                Concepts = voucherDTO.Concepts != null ? CreateConcepts(voucherDTO.Concepts) : new List<CLMWOR.ConceptDTO>(),
                CurrencyId = voucherDTO.CurrencyId,
                ExchangeRate = voucherDTO.ExchangeRate
            };
        }

        private static List<CLMWOR.ConceptDTO> CreateConcepts(List<VoucherConceptDTO> concepts)
        {
            List<CLMWOR.ConceptDTO> conceptDTOs = new List<CLMWOR.ConceptDTO>();

            foreach (VoucherConceptDTO voucherConceptDTO in concepts)
            {
                conceptDTOs.Add(CreateConcept(voucherConceptDTO));
            }

            return conceptDTOs;
        }

        private static CLMWOR.ConceptDTO CreateConcept(VoucherConceptDTO voucherConceptDTO)
        {
            return new CLMWOR.ConceptDTO
            {
                Id = voucherConceptDTO.PaymentConceptId,
                Amount = voucherConceptDTO.Value,
                Description = voucherConceptDTO.PaymentConcept,
                Taxes = voucherConceptDTO.ConceptTaxes != null ? CreateTaxes(voucherConceptDTO.ConceptTaxes) : new List<CLMWOR.TaxDTO>(),
            };
        }

        private static List<CLMWOR.TaxDTO> CreateTaxes(List<VoucherConceptTaxDTO> conceptTaxes)
        {
            List<CLMWOR.TaxDTO> taxDTOs = new List<CLMWOR.TaxDTO>();

            foreach (VoucherConceptTaxDTO voucherConceptTaxDTO in conceptTaxes)
            {
                taxDTOs.Add(CreateTax(voucherConceptTaxDTO));
            }

            return taxDTOs;
        }

        private static CLMWOR.TaxDTO CreateTax(VoucherConceptTaxDTO voucherConceptTaxDTO)
        {
            return new CLMWOR.TaxDTO
            {
                Id = voucherConceptTaxDTO.TaxId,
                Amount = voucherConceptTaxDTO.TaxValue + voucherConceptTaxDTO.Retention,
                Description = voucherConceptTaxDTO.TaxDescription,
                AccountingConceptId = voucherConceptTaxDTO.AccountingConceptId
            };
        }


        internal static CLMWOR.AccountingPaymentRequestDTO CreateAccountingPaymentRequest(AccountingPaymentRequest accountingPaymentRequest)
        {
            return new CLMWOR.AccountingPaymentRequestDTO
            {
                Id = accountingPaymentRequest.Id,
                Amount = accountingPaymentRequest.Amount,
                ExchangeRate = accountingPaymentRequest.ExchangeRate,
                PrefixId = accountingPaymentRequest.PrefixId,
                BranchId = accountingPaymentRequest.BranchId,
                IndividualId = accountingPaymentRequest.IndividualId,
                Number = accountingPaymentRequest.Number,
                AccountingDate = accountingPaymentRequest.AccountingDate,
                UserId = accountingPaymentRequest.UserId,
                Description = accountingPaymentRequest.Description,
                CurrencyId = accountingPaymentRequest.CurrencyId,
                TechnicalTransaction = accountingPaymentRequest.TechnicalTransaction,
                AccountingAccountId = accountingPaymentRequest.AccountingAccountId,
                AccountingAccountNumber = accountingPaymentRequest.AccountingAccountNumber,
                AccountingNatureId = accountingPaymentRequest.AccountingNatureId,
                SalePointId = accountingPaymentRequest.SalePointId,
                BusinessTypeId = accountingPaymentRequest.BusinessTypeId,
                PaymentSourceId = accountingPaymentRequest.PaymentSourceId,
                SalvageId = accountingPaymentRequest.SalvageId,
                RecoveryId = accountingPaymentRequest.RecoveryId,
                Vouchers = accountingPaymentRequest.Vouchers != null ? CreateVouchers(accountingPaymentRequest.Vouchers) : new List<CLMWOR.VoucherDTO>(),
                CoInsuranceAssigneds = accountingPaymentRequest.CoInsuranceAssigneds != null ? CreateCoInsuranceAssigneds(accountingPaymentRequest.CoInsuranceAssigneds) : new List<CLMWOR.CoInsuranceAssignedDTO>()
            };
        }

        internal static List<CLMWOR.VoucherDTO> CreateVouchers(List<AccountingVoucher> accountingVouchers)
        {
            List<CLMWOR.VoucherDTO> vouchersDTOs = new List<CLMWOR.VoucherDTO>();

            foreach (AccountingVoucher accountingVoucher in accountingVouchers)
            {
                vouchersDTOs.Add(CreateVoucher(accountingVoucher));
            }

            return vouchersDTOs;
        }

        internal static CLMWOR.VoucherDTO CreateVoucher(AccountingVoucher accountingVoucher)
        {
            return new CLMWOR.VoucherDTO
            {
                Concepts = accountingVoucher.Concepts != null ? CreateConcepts(accountingVoucher.Concepts) : new List<CLMWOR.ConceptDTO>(),
                Id = accountingVoucher.Id,
                CurrencyId = accountingVoucher.CurrencyId,
                ExchangeRate = accountingVoucher.ExchangeRate
            };
        }

        internal static List<CLMWOR.ConceptDTO> CreateConcepts(List<AccountingConcept> accountingConcepts)
        {
            List<CLMWOR.ConceptDTO> conceptDTOs = new List<CLMWOR.ConceptDTO>();

            foreach (AccountingConcept accountingConcept in accountingConcepts)
            {
                conceptDTOs.Add(CreateConcept(accountingConcept));
            }

            return conceptDTOs;
        }

        internal static CLMWOR.ConceptDTO CreateConcept(AccountingConcept accountingConcept)
        {
            return new CLMWOR.ConceptDTO
            {
                Id = accountingConcept.Id,
                Description = accountingConcept.Description,
                Amount = accountingConcept.Amount,
                AccountingAccountId = accountingConcept.AccountingAccountId,
                AccountingAccountNumber = accountingConcept.AccountingAccountNumber,
                AccountingNatureId = accountingConcept.AccountingNatureId,
                Taxes = accountingConcept.Taxes != null ? CreateTaxes(accountingConcept.Taxes) : new List<CLMWOR.TaxDTO>(),
            };
        }

        internal static List<CLMWOR.TaxDTO> CreateTaxes(List<AccountingTax> accountingTaxes)
        {
            List<CLMWOR.TaxDTO> taxesDTOs = new List<CLMWOR.TaxDTO>();

            foreach (AccountingTax accountingTax in accountingTaxes)
            {
                taxesDTOs.Add(CreateAccountingTax(accountingTax));
            }

            return taxesDTOs;
        }

        internal static CLMWOR.TaxDTO CreateAccountingTax(AccountingTax accountingTax)
        {
            return new CLMWOR.TaxDTO
            {
                Id = accountingTax.Id,
                Description = accountingTax.Description,
                Amount = accountingTax.Amount,
                AccountingConceptId = accountingTax.AccountingConceptId,
                AccountingAccountId = accountingTax.AccountingAccountId,
                AccountingAccountNumber = accountingTax.AccountingAccountNumber,
                AccountingNatureId = accountingTax.AccountingNatureId
            };
        }

        internal static List<CLMWOR.CoInsuranceAssignedDTO> CreateCoInsuranceAssigneds(List<AccountingCoInsuranceAssigned> accountingCoInsuranceAssigneds)
        {
            List<CLMWOR.CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = new List<CLMWOR.CoInsuranceAssignedDTO>();

            foreach (AccountingCoInsuranceAssigned accountingCoInsuranceAssigned in accountingCoInsuranceAssigneds)
            {
                coInsuranceAssignedDTOs.Add(CreateCoInsuranceAssigned(accountingCoInsuranceAssigned));
            }

            return coInsuranceAssignedDTOs;
        }

        internal static CLMWOR.CoInsuranceAssignedDTO CreateCoInsuranceAssigned(AccountingCoInsuranceAssigned accountingCoInsuranceAssigned)
        {
            return new CLMWOR.CoInsuranceAssignedDTO
            {
                EndorsementId = accountingCoInsuranceAssigned.EndorsementId,
                PolicyId = accountingCoInsuranceAssigned.PolicyId,
                InsuranceCompanyId = accountingCoInsuranceAssigned.InsuranceCompanyId,
                PartCiaPercentage = accountingCoInsuranceAssigned.PartCiaPercentage,
                ExpensesPercentage = accountingCoInsuranceAssigned.ExpensesPercentage,
                CompanyNum = accountingCoInsuranceAssigned.CompanyNum,
                AccountingAccountId = accountingCoInsuranceAssigned.AccountingAccountId,
                AccountingAccountNumber = accountingCoInsuranceAssigned.AccountingAccountNumber,
                AccountingNatureId = accountingCoInsuranceAssigned.AccountingNatureId
            };
        }

        #endregion

        #region Salvage

        internal static List<SalvageDTO> CreateSalvages(List<Salvage> salvages)
        {
            List<SalvageDTO> salvagesDTO = new List<SalvageDTO>();

            foreach (Salvage salvage in salvages)
            {
                salvagesDTO.Add(CreateSalvage(salvage));
            }

            return salvagesDTO;
        }

        internal static SalvageDTO CreateSalvage(Salvage salvage)
        {
            if (salvage == null)
            {
                return null;
            }
            else
            {
                return new SalvageDTO
                {
                    Id = salvage.Id,
                    BranchId = Convert.ToInt32(salvage.Branch.Id),
                    ClaimId = salvage.ClaimId,
                    ClaimNumber = salvage.ClaimNumber,
                    PrefixId = Convert.ToInt32(salvage.Prefix.Id),
                    AssignmentDate = salvage.AssignmentDate,
                    CreationDate = salvage.CreationDate,
                    Description = salvage.Description,
                    EndDate = salvage.EndDate,
                    Location = salvage.Location,
                    Observations = salvage.Observations,
                    SubClaimId = salvage.SubClaimId,
                    EstimatedSale = salvage.EstimatedSale,
                    UnitsQuantity = Convert.ToInt32(salvage.UnitsQuantity),
                    TotalAmount = salvage.TotalAmount,
                    RecoveryAmount = salvage.RecoveryAmount
                };
            }
        }

        internal static SelectDTO CreateGetPaymentClass(PaymentClass paymentClass)
        {
            return new SelectDTO
            {
                Id = paymentClass.Id,
                Description = paymentClass.Description
            };
        }

        internal static List<SelectDTO> CreateGetPaymentClasses(List<PaymentClass> paymentClass)
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();

            foreach (PaymentClass paymentClasses in paymentClass)
            {
                selectDTO.Add(CreateGetPaymentClass(paymentClasses));
            }

            return selectDTO.OrderBy(x => x.Description).ToList();
        }

        internal static List<SaleDTO> CreateSales(List<Sale> sales)
        {
            List<SaleDTO> saleDTOs = new List<SaleDTO>();

            foreach (Sale sale in sales)
            {
                saleDTOs.Add(CreateSale(sale));
            }

            return saleDTOs;
        }

        internal static SaleDTO CreateSale(Sale sale)
        {
            if (sale == null)
            {
                return null;
            }
            else
            {
                SaleDTO saleDTO = new SaleDTO
                {
                    Id = sale.Id,
                    Date = sale.CreationDate,
                    Description = sale.Description,
                    TotalAmount = sale.TotalAmount,
                    QuantitySold = sale.SoldQuantity,
                    CancellationReasonId = sale.CancellationReason.Id,
                    CancellationDate = sale.CancellationDate,
                    BuyerId = sale.Buyer.Id,
                    BuyerDocumentNumber = sale.Buyer.DocumentNumber,
                    BuyerFullName = sale.Buyer.FullName,
                    BuyerAddress = sale.Buyer.Address,
                    BuyerPhone = sale.Buyer.Phone,
                    IsParticipant = sale.IsParticipant
                };

                if (sale.PaymentPlan != null)
                {

                    saleDTO.PaymentPlanId = sale.PaymentPlan.Id;
                    saleDTO.CurrencyId = sale.PaymentPlan.Currency.Id;
                    saleDTO.PaymentClassId = sale.PaymentPlan.PaymentClass.Id;
                    saleDTO.Tax = sale.PaymentPlan.Tax;
                    saleDTO.PaymentQuotas = sale.PaymentPlan.PaymentQuotas != null ? CreatePaymentQuotas(sale.PaymentPlan.PaymentQuotas) : new List<PaymentQuotaDTO>();
                }

                return saleDTO;
            }
        }

        internal static BuyerDTO CreateBuyer(Buyer buyer)
        {
            return new BuyerDTO
            {
                IndividualId = buyer.Id,
                FullName = buyer.FullName,
                DocumentNumber = buyer.DocumentNumber,
                Address = buyer.Address,
                Phone = buyer.Phone
            };
        }

        internal static List<BuyerDTO> CreateBuyers(List<Buyer> buyers)
        {
            List<BuyerDTO> buyersDTO = new List<BuyerDTO>();

            foreach (Buyer buyer in buyers)
            {
                buyersDTO.Add(CreateBuyer(buyer));
            }

            return buyersDTO;
        }

        internal static PaymentQuotaDTO CreatePaymentQuota(int paymentNumber, DateTime date, string currencyDescription, decimal amount)
        {
            return new PaymentQuotaDTO
            {
                Number = paymentNumber,
                ExpirationDate = date,
                CurrencyDescription = currencyDescription,
                Amount = amount
            };
        }

        #endregion

        internal static AmountTypeDTO CreateAmountType(AmountType amountType)
        {
            return new AmountTypeDTO
            {
                Id = amountType.Id,
                Description = amountType.Description
            };
        }

        internal static List<AmountTypeDTO> CreateAmountTypes(List<AmountType> amountTypes)
        {
            List<AmountTypeDTO> amountTypeDTOs = new List<AmountTypeDTO>();

            foreach (AmountType amountType in amountTypes)
            {
                amountTypeDTOs.Add(CreateAmountType(amountType));
            }

            return amountTypeDTOs.OrderBy(x => x.Description).ToList();
        }

        public static MinimumSalaryDTO CreateMinimumSalary(MinimumSalary minimumSalary)
        {
            if (minimumSalary == null)
            {
                return null;
            }

            return new MinimumSalaryDTO
            {
                year = minimumSalary.year,
                SalaryMinimumDay = minimumSalary.SalaryMinimumDay,
                SalaryMinimumMounth = minimumSalary.SalaryMinimumMounth
            };
        }
    }
}