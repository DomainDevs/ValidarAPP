using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Assemblers
{
    internal static class EntityAssembler
    {

        #region Facade
        public static void CreateFacadeClaim(Rules.Facade facade, Claim claim)
        {
            facade.SetConcept(RuleConceptClaim.ClaimId, claim.Id);
            facade.SetConcept(RuleConceptClaim.ClaimDate, claim.OccurrenceDate);
            facade.SetConcept(RuleConceptClaim.JudicialDecisionDate, claim.JudicialDecisionDate);
            facade.SetConcept(RuleConceptClaim.ClaimNoticeDate, claim.NoticeDate);
            facade.SetConcept(RuleConceptClaim.RegistrationDate, claim.Modifications.Last().RegistrationDate);
            facade.SetConcept(RuleConceptClaim.NoticeId, claim.NoticeId);
            facade.SetConcept(RuleConceptClaim.ClaimBranchId, claim.Branch.Id);
            facade.SetConcept(RuleConceptClaim.ClaimUserId, claim.Modifications.Last().UserId);
            facade.SetConcept(RuleConceptClaim.ClaimUserProfile, claim.Modifications.Last().UserProfileId);
            facade.SetConcept(RuleConceptClaim.PolicyId, claim.Endorsement.PolicyId);
            facade.SetConcept(RuleConceptClaim.EndorsementId, claim.Endorsement.Id);
            facade.SetConcept(RuleConceptClaim.ClaimEndorsementNumber, claim.Endorsement.Number);
            facade.SetConcept(RuleConceptClaim.ClaimPolicyBusinessTypeId, claim.Endorsement.Policy.BusinessTypeId);
            facade.SetConcept(RuleConceptClaim.ClaimPolicyTypeId, claim.Endorsement.Policy.TypeId);
            facade.SetConcept(RuleConceptClaim.ClaimPolicyProductId, claim.Endorsement.Policy.ProductId);
            facade.SetConcept(RuleConceptClaim.ClaimNumber, claim.Number);
            facade.SetConcept(RuleConceptClaim.PrefixId, claim.Prefix.Id);
            facade.SetConcept(RuleConceptClaim.PolicyNumber, claim.Endorsement.PolicyNumber);
            facade.SetConcept(RuleConceptClaim.ClaimCauseId, claim.Cause.Id);
            facade.SetConcept(RuleConceptClaim.ClaimDetail, claim.TextOperation.Operation);
            facade.SetConcept(RuleConceptClaim.ClaimDamageTypeId, claim.DamageType?.Id);
            facade.SetConcept(RuleConceptClaim.ClaimDamageResponsibilityId, claim.DamageResponsability?.Id);
            facade.SetConcept(RuleConceptClaim.ClaimOccurenceLocation, claim.Location);
            facade.SetConcept(RuleConceptClaim.CountryId, claim.City?.State?.Country?.Id);
            facade.SetConcept(RuleConceptClaim.StateId, claim.City?.State?.Id);
            facade.SetConcept(RuleConceptClaim.CityId, claim.City?.Id);

            if (claim.Inspection != null)
            {
                facade.SetConcept(RuleConceptClaim.InspectionDate, Convert.ToDateTime(claim.Inspection.RegistrationDate));
                facade.SetConcept(RuleConceptClaim.Investigator, claim.Inspection.ResearcherId);
                facade.SetConcept(RuleConceptClaim.Ajuster, claim.Inspection.AdjusterId);
                facade.SetConcept(RuleConceptClaim.Analyst, claim.Inspection.AnalizerId);
            }

            if (claim.CatastrophicEvent != null)
            {
                facade.SetConcept(RuleConceptClaim.Catastrophe, claim.CatastrophicEvent.Catastrophe);
                facade.SetConcept(RuleConceptClaim.CatastropheCurrentFrom, claim.CatastrophicEvent.CurrentFrom);
                facade.SetConcept(RuleConceptClaim.CatastropheCurrentTo, claim.CatastrophicEvent.CurrentTo);
                facade.SetConcept(RuleConceptClaim.AddressOfSinister, claim.CatastrophicEvent.FullAddress);
                facade.SetConcept(RuleConceptClaim.DescriptionOfSinister, claim.CatastrophicEvent.Description);
                facade.SetConcept(RuleConceptClaim.CatastropheCountryId, claim.CatastrophicEvent.City?.State?.Country?.Id);
                facade.SetConcept(RuleConceptClaim.CatastropheStateId, claim.CatastrophicEvent.City?.State?.Id);
                facade.SetConcept(RuleConceptClaim.CatastropheCityId, claim.CatastrophicEvent.City?.Id);
            }

            int? currencyId = null;
            bool differentCurrency = false;
            foreach (ClaimCoverage itemCoverage in claim.Modifications?.Last().Coverages)
            {
                foreach (Estimation itemEstimations in itemCoverage.Estimations)
                {
                    if (currencyId == null)
                    {
                        currencyId = itemEstimations.Currency.Id;
                    }
                    else if (currencyId != itemEstimations.Currency.Id)
                    {
                        differentCurrency = true;
                        break;
                    }
                }
                if (differentCurrency == true)
                    break;
            }

            facade.SetConcept(RuleConceptClaim.EstimationWithDifferentCurrency, differentCurrency);

        }
        public static void CreateFacadeEstimation(Rules.Facade facade, ClaimCoverage claimCoverage, Estimation estimation)
        {
            facade.SetConcept(RuleConceptEstimation.ClaimCoverageId, claimCoverage.Id);
            facade.SetConcept(RuleConceptEstimation.SubClaimId, claimCoverage.SubClaim);
            facade.SetConcept(RuleConceptEstimation.RiskNumber, claimCoverage.RiskNumber);
            facade.SetConcept(RuleConceptEstimation.CoverageNumber, claimCoverage.CoverageNumber);
            facade.SetConcept(RuleConceptEstimation.CoverageInsuredAmount, estimation.CoverageInsuredAmountEquivalent);
            facade.SetConcept(RuleConceptEstimation.RiskId, claimCoverage.RiskId);
            facade.SetConcept(RuleConceptEstimation.AffectedId, claimCoverage.IndividualId);
            facade.SetConcept(RuleConceptEstimation.AffectedName, claimCoverage.ThirdAffected?.FullName);
            facade.SetConcept(RuleConceptEstimation.AffectedDocumentNumber, claimCoverage.ThirdAffected?.DocumentNumber);
            facade.SetConcept(RuleConceptEstimation.AffectedDocumentType, claimCoverage.ThirdAffected?.DocumentTypeId ?? 0);
            facade.SetConcept(RuleConceptEstimation.CoverageId, claimCoverage.CoverageId);
            facade.SetConcept(RuleConceptEstimation.AffectedIsInsured, claimCoverage.IsInsured);
            facade.SetConcept(RuleConceptEstimation.AffectedIsProspect, claimCoverage.IsProspect);
            facade.SetConcept(RuleConceptEstimation.EstimationTypeId, estimation.Type.Id);
            facade.SetConcept(RuleConceptEstimation.EstimationStatusId, estimation.Reason.Status.Id);
            facade.SetConcept(RuleConceptEstimation.EstimationReasonId, estimation.Reason.Id);
            facade.SetConcept(RuleConceptEstimation.EstimationAmount, estimation.Amount); 
            facade.SetConcept(RuleConceptEstimation.DeductibleAmount, estimation.DeductibleAmount);
            facade.SetConcept(RuleConceptEstimation.EstimationDate, estimation.CreationDate);
            facade.SetConcept(RuleConceptEstimation.CurrencyId, estimation.Currency.Id);
            facade.SetConcept(RuleConceptEstimation.EstimationAmountAccumulated, estimation.AmountAccumulate);
            facade.SetConcept(RuleConceptEstimation.ClaimedAmount, claimCoverage.ClaimedAmount);
            facade.SetConcept(RuleConceptEstimation.IsClaimedAmount, claimCoverage.IsClaimedAmount);            
        }

        public static void CreateFacadeNotice(Rules.Facade facade, Notice notice)
        {
            facade.SetConcept(RuleConceptNotice.NoticeId, notice.Id);
            facade.SetConcept(RuleConceptNotice.ContactInformationName, notice.ContactInformation.Name);
            facade.SetConcept(RuleConceptNotice.ContactInformationPhone, notice.ContactInformation.Phone);
            facade.SetConcept(RuleConceptNotice.ContactInformationMail, notice.ContactInformation.Mail);
            facade.SetConcept(RuleConceptNotice.BranchCode, notice.Policy?.BranchId);
            facade.SetConcept(RuleConceptNotice.PrefixCode, notice.Policy?.PrefixId);
            facade.SetConcept(RuleConceptNotice.PolicyDocumentNumber, notice.Policy?.DocumentNumber);
            facade.SetConcept(RuleConceptNotice.EndorsementNumber, notice.Endorsement?.Number);
            facade.SetConcept(RuleConceptNotice.PolicyTypeId, notice.Policy?.TypeId);
            facade.SetConcept(RuleConceptNotice.PolicyBusinessTypeId, notice.Policy?.BusinessTypeId);
            facade.SetConcept(RuleConceptNotice.PolicyProductId, notice.Policy?.ProductId);
            facade.SetConcept(RuleConceptNotice.CurrentFrom, notice.Policy?.CurrentFrom);
            facade.SetConcept(RuleConceptNotice.CurrentTo, notice.Policy?.CurrentTo);
            facade.SetConcept(RuleConceptNotice.NoticeDate, notice.CreationDate);
            facade.SetConcept(RuleConceptNotice.NoticeNumber, notice.Number);
            facade.SetConcept(RuleConceptNotice.ObjetedDescription, notice.ObjectedReason);
            facade.SetConcept(RuleConceptNotice.OccurrenceDate, notice.ClaimDate);
            facade.SetConcept(RuleConceptNotice.DaysAfterNoticeAndOccurrence, (notice.CreationDate - notice.ClaimDate).TotalDays);
            facade.SetConcept(RuleConceptNotice.OccurrenceLocation, notice.Address);
            facade.SetConcept(RuleConceptNotice.OccurrenceCountryId, notice.City?.State?.Country?.Id);
            facade.SetConcept(RuleConceptNotice.OccurrenceStateId, notice.City?.State?.Id);
            facade.SetConcept(RuleConceptNotice.OccurrenceCityId, notice.City?.Id);
            facade.SetConcept(RuleConceptNotice.Description, notice.Description);
            facade.SetConcept(RuleConceptNotice.NoticeReasonId, notice.NoticeReason.Id);
            facade.SetConcept(RuleConceptNotice.NoticeClaimedAmount, notice.ClaimedAmount);
            facade.SetConcept(RuleConceptNotice.NoticeUserId, notice.UserId);
            facade.SetConcept(RuleConceptNotice.NoticeUserProfile, notice.UserProfileId);
        }

        public static void CreateFacadeNoticeCoverage(Rules.Facade facade, NoticeCoverage noticeCoverage)
        {
            facade.SetConcept(RuleConceptEstimation.ClaimCoverageId, noticeCoverage.Id);
            facade.SetConcept(RuleConceptEstimation.RiskNumber, noticeCoverage.RiskNumber);
            facade.SetConcept(RuleConceptEstimation.CoverageNumber, noticeCoverage.CoverageNumber);
            facade.SetConcept(RuleConceptEstimation.RiskId, noticeCoverage.RiskId);
            facade.SetConcept(RuleConceptEstimation.AffectedId, noticeCoverage.IndividualId);
            facade.SetConcept(RuleConceptEstimation.CoverageId, noticeCoverage.CoverageId);
            facade.SetConcept(RuleConceptEstimation.AffectedIsInsured, noticeCoverage.IsInsured);
            facade.SetConcept(RuleConceptEstimation.AffectedIsProspect, noticeCoverage.IsProspect);
            facade.SetConcept(RuleConceptEstimation.EstimationTypeId, noticeCoverage.EstimateTypeId);
            facade.SetConcept(RuleConceptEstimation.EstimationAmount, noticeCoverage.EstimateAmount);
            facade.SetConcept(RuleConceptEstimation.CurrencyId, noticeCoverage.CurrencyId);
            facade.SetConcept(RuleConceptEstimation.AffectedName, noticeCoverage.FullName);
            facade.SetConcept(RuleConceptEstimation.AffectedDocumentNumber, noticeCoverage.DocumentNumber);
            facade.SetConcept(RuleConceptEstimation.AffectedDocumentType, noticeCoverage.DocumentTypeId);
        }

        public static void CreateFacadeNoticeVehicle(Rules.Facade facade, NoticeVehicle noticeVehicle)
        {
            facade.SetConcept(RuleConceptNotice.LicensePlateId, noticeVehicle.Plate);
            facade.SetConcept(RuleConceptNotice.VehicleMakeId, noticeVehicle.MakeId);
            facade.SetConcept(RuleConceptNotice.VehicleModelId, noticeVehicle.ModelId);
            facade.SetConcept(RuleConceptNotice.VehicleVersionId, noticeVehicle.VersionId);
            facade.SetConcept(RuleConceptNotice.VehicleYear, noticeVehicle.Year);
            facade.SetConcept(RuleConceptNotice.ColorId, noticeVehicle.ColorId);
            facade.SetConcept(RuleConceptNotice.DamageTypeId, noticeVehicle.Notice.DamageType.Id);
            facade.SetConcept(RuleConceptNotice.DamageResponsibilityId, noticeVehicle.Notice.DamageResponsability.Id);
            facade.SetConcept(RuleConceptNotice.AditionalInformation, noticeVehicle.Notice.AdditionalInformation);
        }

        public static void CreateFacadeNoticeLocation(Rules.Facade facade, NoticeLocation noticeLocation)
        {
            facade.SetConcept(RuleConceptNotice.RiskLocation, noticeLocation.Address);
            facade.SetConcept(RuleConceptNotice.CountryLocationId, noticeLocation.CountryId);
            facade.SetConcept(RuleConceptNotice.StateLocationId, noticeLocation.StateId);
            facade.SetConcept(RuleConceptNotice.CityLocationId, noticeLocation.CityId);
        }

        public static void CreateFacadeNoticeFidelity(Rules.Facade facade, NoticeFidelity noticeFidelity)
        {
            facade.SetConcept(RuleConceptNotice.ActivityRiskId, noticeFidelity.RiskCommercialClassId);
            facade.SetConcept(RuleConceptNotice.OccupationId, noticeFidelity.OccupationId);
            facade.SetConcept(RuleConceptNotice.DiscoveryDate, noticeFidelity.DiscoveryDate);

        }

        public static void CreateFacadeNoticeTransport(Rules.Facade facade, NoticeTransport noticeTransport)
        {
            facade.SetConcept(RuleConceptNotice.MerchandiseTypeId, noticeTransport.CargoType);
            facade.SetConcept(RuleConceptNotice.PackagingTypeId, noticeTransport.PackagingType);
            facade.SetConcept(RuleConceptNotice.OriginTransportId, noticeTransport.Origin);
            facade.SetConcept(RuleConceptNotice.DestinyTransportId, noticeTransport.Destiny);
            facade.SetConcept(RuleConceptNotice.TransportTypeId, noticeTransport.TransportType);
        }

        public static void CreateFacadeNoticeAirCraft(Rules.Facade facade, NoticeAirCraft noticeAirCraft)
        {
            facade.SetConcept(RuleConceptNotice.RegisterNumberId, noticeAirCraft.RegisterNumer);
            facade.SetConcept(RuleConceptNotice.AirCraftMakeId, noticeAirCraft.MakeId);
            facade.SetConcept(RuleConceptNotice.AirCraftModelId, noticeAirCraft.ModelId);
            facade.SetConcept(RuleConceptNotice.AirCraftUseId, noticeAirCraft.UseId);
            facade.SetConcept(RuleConceptNotice.AirCraftRegisterId, noticeAirCraft.RegisterId);
            facade.SetConcept(RuleConceptNotice.AirCraftOperatorId, noticeAirCraft.OperatorId);
        }

        public static void CreateFacadeNoticeSurety(Rules.Facade facade, NoticeSurety noticeSurety)
        {
            facade.SetConcept(RuleConceptNotice.Surety, noticeSurety.Name);
            facade.SetConcept(RuleConceptNotice.ContractNumberId, noticeSurety.BidNumber);
            facade.SetConcept(RuleConceptNotice.JudgmentNumberId, noticeSurety.CourtNum);
        }
        #endregion Facade

        internal static CLMEN.CauseCoverage CreateCauseCoverage(CauseCoverage causeCoverage)
        {
            return new CLMEN.CauseCoverage(causeCoverage.Cause.Id, causeCoverage.Id)
            {
                LackPeriod = causeCoverage.LackPeriod
            };
        }

        internal static CLMEN.Claim CreateClaim(Claim claim)
        {
            CLMEN.Claim entityClaim = new CLMEN.Claim
            {
                OccurrenceDate = claim.OccurrenceDate,
                NoticeDate = claim.NoticeDate,
                ClaimNoticeCode = claim.NoticeId,
                ClaimBranchCode = claim.Branch.Id,
                PolicyId = claim.Endorsement.PolicyId,
                EndorsementId = claim.Endorsement.Id,
                IndividualId = claim.IndividualId,
                BusinessTypeCode = claim.BusinessTypeId,
                Number = claim.Number,
                PrefixCode = claim.Prefix.Id,
                DocumentNumber = Convert.ToDecimal(claim.Endorsement.PolicyNumber),
                CauseId = claim.Cause?.Id,
                CityCode = claim.City?.Id,
                StateCode = claim.City?.State?.Id,
                CountryCode = claim.City?.State?.Country?.Id,
                Location = claim.Location,
                ClaimDamageResponsibilityCode = claim.DamageResponsability?.Id,
                ClaimDamageTypeCode = claim.DamageType?.Id,
                TextOperationCode = claim.TextOperation?.Id,
                IsTotalParticipation = claim.IsTotalParticipation,
                JudicialDecisionDate = claim.JudicialDecisionDate
            };

            if (claim.Id > 0)
            {
                entityClaim.ClaimCode = claim.Id;
            }
            if (claim.TextOperation == null)
            {
                entityClaim.TextOperationCode = null;
            }

            return entityClaim;
        }

        internal static CLMEN.ClaimModify CreateClaimModify(ClaimModify claimModify, int claimId)
        {
            CLMEN.ClaimModify entityClaimModify = new CLMEN.ClaimModify()
            {
                ClaimCode = claimId,
                RegistrationDate = claimModify.RegistrationDate,
                AccountingDate = claimModify.AccountingDate,
                UserId = claimModify.UserId
            };

            if (claimModify.Id > 0)
            {
                entityClaimModify.ClaimModifyCode = claimModify.Id;
            }

            return entityClaimModify;
        }

        internal static CLMEN.ClaimNotice CreateClaimNotice(Notice notice)
        {
            CLMEN.ClaimNotice entityClaimNotice = new CLMEN.ClaimNotice()
            {
                ClaimNoticeDate = notice.CreationDate,
                PolicyId = notice.Endorsement?.PolicyId,
                ClaimDate = notice.ClaimDate,
                Location = notice.Address,
                CountryCode = notice.City.State.Country.Id,
                StateCode = notice.City.State.Id,
                CityCode = notice.City.Id,
                Description = notice.Description,
                EndorsementId = notice.Endorsement?.Id,
                ObjectedDescription = notice.ObjectedReason,
                UserId = notice.UserId,
                CoveredRiskTypeCode = notice.CoveredRiskTypeId,
                Latitude = notice.Latitude.ToString(),
                Longitude = notice.Longitude.ToString(),
                ClaimNoticeTypeId = notice.Type.Id,
                RiskId = notice.Risk.RiskId,
                ClaimDamageResponsibilityCode = notice.DamageResponsability?.Id,
                ClaimDamageTypeCode = notice.DamageType?.Id,
                ClaimNoticeReasonCode = notice.NoticeReason?.Id,
                Number = notice.Number,
                NumberObjected = notice.NumberObjected,
                IndividualId = notice.IndividualId,
                OthersAffected = notice.OthersAffected,
                InternalConsecutive = notice.InternalConsecutive,
                ClaimedAmount = notice.ClaimedAmount,
                ClaimReasonOthers = notice.ClaimReasonOthers,
                ClaimNoticeStateCode = notice.NoticeState.Id,
            };

            if (notice.Id > 0)
            {
                entityClaimNotice.ClaimNoticeCode = notice.Id;
            }

            return entityClaimNotice;
        }

        internal static CLMEN.ClaimNoticeContactInformation CreateClaimContactInformation(ContactInformation contactInformation, int claimNoticeCode)
        {
            return new CLMEN.ClaimNoticeContactInformation(claimNoticeCode)
            {
                Name = contactInformation.Name,
                Phone = contactInformation.Phone,
                Mail = contactInformation.Mail
            };
        }

        internal static PARAMEN.EstimationTypePrefix CreateEstimationTypePrefix(int estimationTypeId, Prefix prefix)
        {
            return new PARAMEN.EstimationTypePrefix(estimationTypeId, prefix.Id)
            {
            };
        }

        internal static PARAMEN.EstimationTypeStatus CreateStatus(Status status)
        {
            return new PARAMEN.EstimationTypeStatus(status.Id)
            {
                Description = status.Description,
                Enabled = status.IsEnabled,
                InternalStatusCode = status.InternalStatus.Id
            };
        }

        internal static PARAMEN.EstimationTypeStatusReason CreateReason(Reason reason)
        {
            return new PARAMEN.EstimationTypeStatusReason(reason.Id, reason.Status.Id, reason.Prefix.Id)
            {
                Description = reason.Description,
                Enabled = reason.Enabled
            };
        }

        internal static PARAMEN.RelationTypeStatus CreateStatusByEstimationType(Status status, int estimationTypeId)
        {
            return new PARAMEN.RelationTypeStatus(status.Id, estimationTypeId)
            {
            };
        }

        internal static CLMEN.ClaimNoticeRiskLocation CreateClaimNoticeRiskLocation(ClaimLocation claimLocation, int noticeId)
        {
            return new CLMEN.ClaimNoticeRiskLocation(noticeId)
            {
                Address = claimLocation.Address,
                CountryCode = claimLocation.City.State.Country.Id,
                StateCode = claimLocation.City.State.Id,
                CityCode = claimLocation.City.Id
            };
        }

        internal static CLMEN.ClaimNoticeRiskSurety CreateClaimNoticeSurety(NoticeSurety noticeSurety, int noticeId)
        {
            return new CLMEN.ClaimNoticeRiskSurety(noticeId)
            {
                Name = noticeSurety.Name,
                IdCardNo = noticeSurety.DocumentNumber,
                CourtNum = noticeSurety.CourtNum,
                BidNumber = noticeSurety.BidNumber
            };
        }

        internal static CLMEN.ClaimNoticeVehicle CreateNoticeVehicle(NoticeVehicle noticeVehicle, int noticeId)
        {
            return new CLMEN.ClaimNoticeVehicle(noticeId)
            {
                Plate = noticeVehicle.Plate,
                VehicleMakeCode = noticeVehicle.MakeId,
                VehicleModelCode = noticeVehicle.ModelId,
                VehicleVersionCode = noticeVehicle.VersionId,
                VehicleVersionYearCode = noticeVehicle.Year,
                VehicleColor = noticeVehicle.ColorId,
                Driver = noticeVehicle.DriverName
            };
        }

        internal static CLMEN.ClaimDamageType CreateDamageType(DamageType damageType)
        {
            return new CLMEN.ClaimDamageType(damageType.Id)
            {
                Description = damageType.Description,
                Enabled = damageType.IsEnabled,
            };

        }
        internal static CLMEN.ClaimDamageResponsibility CreateDamageResponsability(DamageResponsibility damageResponsability)
        {
            return new CLMEN.ClaimDamageResponsibility(damageResponsability.Id)
            {
                Description = damageResponsability.Description,
                Enabled = damageResponsability.IsEnabled,
            };

        }

        internal static CLMEN.ClaimCoverage CreateClaimCoverage(ClaimCoverage claimCoverage, int claimModifyId)
        {
            CLMEN.ClaimCoverage entityClaimCoverage = new CLMEN.ClaimCoverage()
            {
                SubClaim = claimCoverage.SubClaim,
                ClaimModifyCode = claimModifyId,
                RiskNum = claimCoverage.RiskNumber,
                EndorsementId = claimCoverage.EndorsementId,
                RiskId = claimCoverage.RiskId,
                CoverageNum = claimCoverage.CoverageNumber,
                IndividualId = claimCoverage.IndividualId,
                CoverageId = claimCoverage.CoverageId,
                IsInsured = claimCoverage.IsInsured,
                IsProspect = claimCoverage.IsProspect,
                TextOperationCode = claimCoverage.TextOperation?.Id,
                ClaimedAmount = claimCoverage.ClaimedAmount,
                IsClaimedAmount = claimCoverage.IsClaimedAmount

            };
            if (claimCoverage.TextOperation == null)
            {
                entityClaimCoverage.TextOperationCode = null;
            }
            return entityClaimCoverage;
        }

        internal static CLMEN.ClaimCoverageAmount CreateClaimCoverageAmount(Estimation estimation, int claimCoverageId)
        {
            return new CLMEN.ClaimCoverageAmount()
            {
                ClaimCoverageCode = claimCoverageId,
                EstimationTypeCode = estimation.Type.Id,
                EstimationTypeStatusCode = estimation.Reason.Status.Id,
                EstimationTypeStatusReasonCode = estimation.Reason.Id,
                EstimateAmount = estimation.Amount,
                DeductibleAmount = estimation.DeductibleAmount,
                VersionCode = estimation.Version,
                Date = estimation.CreationDate > DateTime.MinValue ? estimation.CreationDate : DateTime.Now,
                CurrencyCode = estimation.Currency.Id,
                EstimateAmountAccumulate = estimation.AmountAccumulate,
                ExchangeRate = estimation.ExchangeRate,
                IsMinimumSalary = estimation.IsMinimumSalary,
                MinimumSalariesNumber = Convert.ToDecimal(estimation.MinimumSalariesNumber),
                MinimumSalaryValue = estimation.MinimumSalaryValue
            };
        }

        internal static PARAMEN.Catastrophe CreateCatastrophe(Catastrophe catastrophe)
        {
            return new PARAMEN.Catastrophe()
            {
                Description = catastrophe.Description
            };
        }

        internal static CLMEN.ClaimCatastrophicInformation CreateCatastrophicInformation(CatastrophicEvent catastropheInformation, int claimCode)
        {
            return new CLMEN.ClaimCatastrophicInformation(claimCode)
            {
                Datetimefrom = catastropheInformation.CurrentFrom,
                Datetimeto = catastropheInformation.CurrentTo,
                Address = catastropheInformation.FullAddress,
                CityCode = catastropheInformation.City.Id,
                StateCode = catastropheInformation.City.State.Id,
                CountryCode = catastropheInformation.City.State.Country.Id,
                CatastropheCode = catastropheInformation.Catastrophe.Id,
                Description = catastropheInformation.Description
            };
        }

        internal static PARAMEN.ClaimCoverageActivePanel CreateClaimCoverageActivePanel(ClaimCoverageActivePanel ClaimCoverageActivePanel)
        {
            if (ClaimCoverageActivePanel == null)
            {
                return null;
            }
            return new PARAMEN.ClaimCoverageActivePanel(ClaimCoverageActivePanel.CoverageId)
            {
                CoverageId = ClaimCoverageActivePanel.CoverageId,
                EnabledDriver = ClaimCoverageActivePanel.EnabledDriver,
                EnabledThirdPartyVehicle = ClaimCoverageActivePanel.EnabledThirdPartyVehicle,
                EnabledThird = ClaimCoverageActivePanel.EnabledThird,
                EnabledAffectedProperty = ClaimCoverageActivePanel.EnabledAffectedProperty
            };
        }

        internal static CLMEN.ClaimCoverageDriverInformation CreateDriver(Driver claimCoverageDriverInformation)
        {
            if (claimCoverageDriverInformation == null)
            {
                return null;
            }

            return new CLMEN.ClaimCoverageDriverInformation(claimCoverageDriverInformation.Id)
            {
                LicenseNumber = claimCoverageDriverInformation.LicenseNumber,
                LicenseType = claimCoverageDriverInformation.LicenseType,
                LicenseValidThru = claimCoverageDriverInformation.LicenseValidThru,
                Age = claimCoverageDriverInformation.Age,
                DocumentNumber = claimCoverageDriverInformation.DocumentNumber,
                Name = claimCoverageDriverInformation.Name
            };
        }

        internal static CLMEN.ClaimCoverageThirdPartyVehicle CreateThirdPartyVehicle(ThirdPartyVehicle claimCoverageThirdPartyVehicle)
        {
            if (claimCoverageThirdPartyVehicle == null)
            {
                return null;
            }
            return new CLMEN.ClaimCoverageThirdPartyVehicle(claimCoverageThirdPartyVehicle.Id)
            {
                Plate = claimCoverageThirdPartyVehicle.Plate,
                VehicleMake = claimCoverageThirdPartyVehicle.Make,
                VehicleModel = claimCoverageThirdPartyVehicle.Model,
                VehicleColorCode = claimCoverageThirdPartyVehicle.ColorCode,
                EngineNumber = claimCoverageThirdPartyVehicle.EngineNumber,
                ChasisNumber = claimCoverageThirdPartyVehicle.ChasisNumber,
                VehicleYear = claimCoverageThirdPartyVehicle.Year,
                Description = claimCoverageThirdPartyVehicle.Description,
                VinCode = claimCoverageThirdPartyVehicle.VinCode
            };
        }

        internal static CLMEN.ClaimSupplier CreateClaimSupplier(Inspection inspection, int claimId)
        {
            return new CLMEN.ClaimSupplier(claimId)
            {
                AdjusterCode = inspection.AdjusterId,
                AnalizerCode = inspection.AnalizerId,
                ResearcherCode = inspection.ResearcherId,
                DateInspection = inspection.RegistrationDate,
                HourInspection = inspection.RegistrationHour,
                AffectedProperty = inspection.AffectedProperty,
                LossDescription = inspection.LossDescription
            };
        }

        internal static CLMEN.ClaimNoticeRiskLocation CreateNoticeLocation(NoticeLocation noticeLocation, int noticeId)
        {
            return new CLMEN.ClaimNoticeRiskLocation(noticeId)
            {
                Address = noticeLocation.Address,
                CountryCode = noticeLocation.CountryId,
                StateCode = noticeLocation.StateId,
                CityCode = noticeLocation.CityId
            };
        }

        internal static CLMEN.ClaimNoticeCoverage CreateClaimNoticeCoverage(NoticeCoverage noticeCoverage, int claimNoticeCode)
        {
            return new CLMEN.ClaimNoticeCoverage(claimNoticeCode, noticeCoverage.CoverageId, noticeCoverage.IndividualId, noticeCoverage.EstimateTypeId)
            {
                CoverageId = noticeCoverage.CoverageId,
                CoverNum = noticeCoverage.CoverageNumber,
                RiskNum = noticeCoverage.RiskNumber,
                IndividualId = noticeCoverage.IndividualId,
                IsInsured = noticeCoverage.IsInsured,
                EstimateTypeCode = noticeCoverage.EstimateTypeId,
                EstimateAmount = noticeCoverage.EstimateAmount,
                IsProspect = !noticeCoverage.IsInsured
            };
        }

        internal static CLMEN.ClaimNoticeRiskTransport CreateNoticeTransport(NoticeTransport noticeTransport, int noticeId)
        {
            return new CLMEN.ClaimNoticeRiskTransport(noticeId)
            {
                TransportCargoType = noticeTransport.CargoType,
                TransportPackagingType = noticeTransport.PackagingType,
                Source = noticeTransport.Origin,
                Destiny = noticeTransport.Destiny,
                TransportViaType = noticeTransport.TransportType
            };
        }

        internal static CLMEN.ClaimNoticeRiskAircraft CreateNoticeAirCraft(NoticeAirCraft noticeAirCraft, int noticeId)
        {
            return new CLMEN.ClaimNoticeRiskAircraft(noticeId)
            {
                AircraftMakeCode = noticeAirCraft.MakeId,
                AircraftModelCode = noticeAirCraft.ModelId,
                AircraftTypeCode = noticeAirCraft.TypeId,
                AircraftOperatorCode = noticeAirCraft.OperatorId,
                AircraftRegisterCode = noticeAirCraft.RegisterId,
                AircraftRegisterNo = noticeAirCraft.RegisterNumer,
                AircraftUseCode = noticeAirCraft.UseId,
                AircraftYear = noticeAirCraft.Year
            };
        }

        internal static CLMEN.ClaimNoticeRiskFidelity CreateNoticeFidelity(NoticeFidelity noticeFidelity, int noticeId)
        {
            return new CLMEN.ClaimNoticeRiskFidelity(noticeId)
            {
                RiskCommercialClassCode = noticeFidelity.RiskCommercialClassId,
                OccupationCode = noticeFidelity.OccupationId,
                DiscoveryDate = noticeFidelity.DiscoveryDate,
                Description = noticeFidelity.Description
            };
        }

        internal static CLMEN.SubCause CreateSubCause(SubCause subCause)
        {
            return new CLMEN.SubCause
            {
                SubcauseId = subCause.Id,
                Description = subCause.Description,
                CauseCode = subCause.Cause.Id
            };
        }

        internal static PARAMEN.Documentation CreateDocumentation(ClaimDocumentation documentation)
        {
            return new PARAMEN.Documentation(documentation.Id)
            {
                DocumentationCode = documentation.Id,
                ModuleCode = documentation.ModuleId,
                SubmoduleCode = documentation.SubmoduleId,
                PrefixCode = documentation.prefix.Id,
                Description = documentation.Description,
                IsRequired = documentation.IsRequired,
                Enabled = documentation.Enable

            };
        }

        internal static CLMEN.CoveragePaymentConcept CreatePaymentConcept(CoveragePaymentConcept coveragePaymentConcepts)
        {
            return new CLMEN.CoveragePaymentConcept(coveragePaymentConcepts.CoverageId, coveragePaymentConcepts.EstimationTypeId, coveragePaymentConcepts.ConceptId)
            {
            };
        }

        internal static PAYMEN.PaymentRequestCoinsurance CreatePaymentRequestCoInsurance(PaymentRequestCoInsurance paymentRequestCoInsurance, int patmentRequestId, int CompanyId)
        {
            return new PAYMEN.PaymentRequestCoinsurance(patmentRequestId, CompanyId)
            {
                PaymentRequestCode = paymentRequestCoInsurance.PaymentRequestId,
                CompanyCode = paymentRequestCoInsurance.CompanyId,
                CoverageCode = paymentRequestCoInsurance.CoverageId,
                CurrencyCode = paymentRequestCoInsurance.CurrencyId,
                IndividualId = paymentRequestCoInsurance.IndividualId,
                Amount = paymentRequestCoInsurance.Amount,
                Number = paymentRequestCoInsurance.Number,
                PartCiaPercentage = paymentRequestCoInsurance.PartCiaPct,
                UserId = paymentRequestCoInsurance.UserId,
                TypePaymentRequestCode = paymentRequestCoInsurance.TypePaymentRequestId
            };
        }

        internal static PAYMEN.PaymentRequestCoinsurance CreateCancellationPaymentRequestCoInsurance(PAYMEN.PaymentRequestCoinsurance entityPaymentRequestCoInsurance, int paymentRequestId, int companyId)
        {
            return new CLMEN.PaymentRequestCoinsurance(paymentRequestId, companyId)
            {
                CoverageCode = entityPaymentRequestCoInsurance.CoverageCode,
                CurrencyCode = entityPaymentRequestCoInsurance.CurrencyCode,
                IndividualId = entityPaymentRequestCoInsurance.IndividualId,
                Amount = entityPaymentRequestCoInsurance.Amount * -1,
                Number = entityPaymentRequestCoInsurance.Number,
                PartCiaPercentage = entityPaymentRequestCoInsurance.PartCiaPercentage,
                UserId = entityPaymentRequestCoInsurance.UserId,
                TypePaymentRequestCode = entityPaymentRequestCoInsurance.TypePaymentRequestCode
            };
        }

        internal static CLMEN.Participant CreateParticipant(Participant participant)
        {
            return new CLMEN.Participant()
            {
                ParticipantCode = participant.Id,
                DocumentNumber = participant.DocumentNumber,
                DocumentTypeCode = participant.DocumentTypeId,
                Fullname = participant.Fullname,
                Address = participant.Address,
                Phone = participant.Phone
            };
        }

        internal static CLMEN.ClaimCoverageThirdAffected CreateThirdAffected(ThirdAffected thirdAffected)
        {
            if (thirdAffected == null)
            {
                return null;
            }

            return new CLMEN.ClaimCoverageThirdAffected(thirdAffected.ClaimCoverageId)
            {
                ClaimCoverageCode = thirdAffected.ClaimCoverageId,
                DocumentNumber = thirdAffected.DocumentNumber,
                Fullname = thirdAffected.FullName
            };
        }

        internal static CLMEN.TextOperation CreateTextOperation(TextOperation textOperation)
        {
            return new CLMEN.TextOperation()
            {
                Id = textOperation.Id,
                TextDescription = textOperation.Operation
            };
        }

        internal static CLMEN.PendingOperations CreatePendingOperation(PendingOperation pendingOperation)
        {
            return new CLMEN.PendingOperations()
            {
                Id = pendingOperation.Id,
                CreationDate = pendingOperation.CreationDate,
                User = pendingOperation.UserId,
                Operation = pendingOperation.Operation
            };
        }

        internal static INTEN.ClmClaimControl CreateClaimControl(ClaimControl claimControl)
        {
            return new INTEN.ClmClaimControl
            {
                ClaimCode = claimControl.ClaimId,
                ClaimModifyCode = claimControl.ClaimModifyId,
                Action = claimControl.Action
            };
        }

        #region Recovery

        internal static CLMEN.Recovery CreateRecovery(Recovery recovery)
        {
            CLMEN.Recovery entityRecovery = new CLMEN.Recovery()
            {
                RecoveryTypeCode = recovery.RecoveryType.Id,
                CreatedDate = recovery.CreatedDate,
                Description = recovery.Description,
                CancellationDate = recovery.CancellationDate,
                RecuperatorId = recovery.Recuperator.Id,
                CancellationReasonCode = recovery.CancellationReason.Id,
                PrescriptionDate = recovery.PrescriptionDate,
                CompanyCode = recovery.CompanyId,
                Voucher = recovery.Voucher,
                RecoveryClassCode = recovery.RecoveryClassId,
                LossResponsible = recovery.LossResponsible,
                AssignedCourt = recovery.AssignedCourt,
                ExpedientNumber = recovery.ExpedientNumber,
                AttorneyAssignmentDate = recovery.AttorneyAssingmentDate,
                LastReportDate = recovery.LastReportDate,
                DebtorCode = recovery.Debtor.Id,
                ClaimCode = recovery.ClaimId,
                SubClaimCode = recovery.SubClaimId,
                BranchCode = recovery.Branch.Id,
                PrefixCode = recovery.Prefix.Id,
                ClaimNumber = recovery.ClaimNumber,
                TotalAmount = recovery.TotalAmmount,
                PaymentPlanCode = recovery.PaymentPlan?.Id,
                DebtorIsParticipant = recovery.DebtorIsParticipant
            };

            if (recovery.Id > 0)
            {
                entityRecovery.RecoveryCode = recovery.Id;
            }

            return entityRecovery;
        }


        internal static INTEN.ClmRecoveryControl CreateRecoveryControl(RecoveryControl recoveryControl)
        {
            return new INTEN.ClmRecoveryControl
            {
                RecoveryCode = recoveryControl.RecoveryId,
                PaymentPlanCode = recoveryControl.PaymentPlanId,
                Action = recoveryControl.Action
            };
        }
        #endregion

        #region PaymentRequest
        internal static void CreateFacadePaymentRequest(Rules.Facade facade, PaymentRequest paymentRequest, Claim claim)
        {
            facade.SetConcept(RuleConceptPaymentRequest.TotalAmount, paymentRequest.TotalAmount);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentBranchId, paymentRequest.Branch.Id);
            facade.SetConcept(RuleConceptPaymentRequest.EstimatedDate, paymentRequest.EstimatedDate);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentSourceId, paymentRequest.MovementType.Source.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentCreatedDate, paymentRequest.RegistrationDate);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentDate, paymentRequest.AccountingDate);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentMethodId, paymentRequest.PaymentMethod.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentCurrencyId, paymentRequest.Currency.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentMovementTypeId, paymentRequest.MovementType.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentClaimLineBusinessId, paymentRequest.Prefix.LineBusinessId);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentClaimPrefixId, paymentRequest.Prefix.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentTypeId, paymentRequest.Type.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PayTo, paymentRequest.PersonType.Id);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentIndividualId, paymentRequest.IndividualId);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentIndividualName, paymentRequest.IndividualName);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentIndividualDocumentNumber, paymentRequest.IndividualDocumentNumber);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentIndividualDocumentType, paymentRequest.IndividualDocumentTypeId);
            facade.SetConcept(RuleConceptPaymentRequest.DescriptionPaymentRequest, paymentRequest.Description);
            facade.SetConcept(RuleConceptPaymentRequest.EstimationType, claim.Modifications.First().Coverages.First().Estimations.First().Type.Id);
            facade.SetConcept(RuleConceptPaymentRequest.EstimationStatus, claim.Modifications.First().Coverages.First().Estimations.First().Reason.Status.Id);
            facade.SetConcept(RuleConceptPaymentRequest.ClaimCurrency, claim.Modifications.First().Coverages.First().Estimations.First().Currency.Id);
            facade.SetConcept(RuleConceptPaymentRequest.EstimationAmount, claim.Modifications.First().Coverages.First().Estimations.First().Amount);
            facade.SetConcept(RuleConceptPaymentRequest.EstimationReason, claim.Modifications.First().Coverages.First().Estimations.First().Reason.Id);
            facade.SetConcept(RuleConceptPaymentRequest.Coverage, claim.Modifications.First().Coverages.First().CoverageId);
            facade.SetConcept(RuleConceptPaymentRequest.PolicyId, claim.Endorsement.PolicyId);
            facade.SetConcept(RuleConceptPaymentRequest.PolicyNumber, claim.Endorsement.PolicyNumber);
            facade.SetConcept(RuleConceptPaymentRequest.ClaimNumber, claim.Number);
            facade.SetConcept(RuleConceptPaymentRequest.EstimationDate, claim.Modifications.First().Coverages.First().Estimations.First().CreationDate);
            facade.SetConcept(RuleConceptPaymentRequest.DaysAfterPayAndOccurrence, (paymentRequest.EstimatedDate - Convert.ToDateTime(claim.JudicialDecisionDate)).TotalDays);
            facade.SetConcept(RuleConceptPaymentRequest.PaymentRequestPolicyProductId, claim.Endorsement.Policy.ProductId);
            facade.SetConcept(RuleConceptPaymentRequest.RiskId, claim.Modifications.Last().Coverages.Last().RiskId);
            facade.SetConcept(RuleConceptPaymentRequest.JudicialDecisionDate, claim.JudicialDecisionDate);            
        }

        internal static void CreateFacadeVoucher(Rules.Facade facade, Voucher voucher, List<VoucherConcept> voucherConcepts)
        {
            facade.SetConcept(RuleConceptVoucher.VoucherTypeId, voucher.VoucherType.Id);
            facade.SetConcept(RuleConceptVoucher.VoucherDate, voucher.Date);
            facade.SetConcept(RuleConceptVoucher.VoucherValue, voucherConcepts.Sum(x => x.Value));
            facade.SetConcept(RuleConceptVoucher.VoucherCurrencyId, voucher.Currency.Id);
        }

        internal static void CreateFacadeVoucherConcept(Rules.Facade facade, VoucherConcept voucherConcept)
        {
            facade.SetConcept(RuleConceptVoucherConcept.PaymentConceptId, voucherConcept.PaymentConcept.Id);
            facade.SetConcept(RuleConceptVoucherConcept.ConceptValue, voucherConcept.Value);
            facade.SetConcept(RuleConceptVoucherConcept.ConceptTax, voucherConcept.TaxValue);
        }

        internal static void CreateFacadeChargeRequest(Rules.Facade facade, ChargeRequest chargeRequest, Claim claim)
        {
            facade.SetConcept(RuleConceptChargeRequest.TotalAmount, chargeRequest.TotalAmount);
            facade.SetConcept(RuleConceptChargeRequest.ChargeBranchId, chargeRequest.Branch.Id);
            facade.SetConcept(RuleConceptChargeRequest.PaymentSourceId, chargeRequest.MovementType.Source.Id);
            facade.SetConcept(RuleConceptChargeRequest.ChargeCreatedDate, chargeRequest.RegistrationDate);
            facade.SetConcept(RuleConceptChargeRequest.AccountingDate, chargeRequest.AccountingDate);
            facade.SetConcept(RuleConceptChargeRequest.ChargeMovementTypeId, chargeRequest.MovementType.Id);
            facade.SetConcept(RuleConceptChargeRequest.ChargeClaimPrefixId, chargeRequest.Prefix.Id);
            facade.SetConcept(RuleConceptChargeRequest.ChargeTo, chargeRequest.PersonType.Id);
            facade.SetConcept(RuleConceptChargeRequest.ChargeIndividualId, chargeRequest.IndividualId);
            facade.SetConcept(RuleConceptChargeRequest.ChargeIndividualName, chargeRequest.Participant.Fullname);
            facade.SetConcept(RuleConceptChargeRequest.ChargeIndividualDocumentNumber, chargeRequest.Participant.DocumentNumber);
            facade.SetConcept(RuleConceptChargeRequest.ChargeIndividualDocumentType, chargeRequest.Participant.DocumentTypeId);
            //facade.SetConcept(RuleConceptChargeRequest.EstimationType, claim.Modifications.First().Coverages.First().Estimations?.First().Type.Id);
            //facade.SetConcept(RuleConceptChargeRequest.EstimationStatus, claim.Modifications.First().Coverages.First().Estimations?.First().Reason.Status.Id);
            //facade.SetConcept(RuleConceptChargeRequest.ClaimCurrency, claim.Modifications.First().Coverages.First().Estimations?.First().Currency.Id);
            //facade.SetConcept(RuleConceptChargeRequest.EstimationAmount, claim.Modifications.First().Coverages.First().Estimations?.First().Amount);
            //facade.SetConcept(RuleConceptChargeRequest.EstimationReason, claim.Modifications.First().Coverages.First().Estimations?.First().Reason.Id);
            //facade.SetConcept(RuleConceptChargeRequest.Coverage, claim.Modifications.First().Coverages.First().Id);
            //facade.SetConcept(RuleConceptChargeRequest.PolicyId, claim.Endorsement?.PolicyId);
            facade.SetConcept(RuleConceptChargeRequest.PolicyNumber, claim.Endorsement?.Policy?.DocumentNumber);
            facade.SetConcept(RuleConceptChargeRequest.ClaimNumber, claim.Number);
            //facade.SetConcept(RuleConceptChargeRequest.EstimationDate, claim.Modifications.First().Coverages.First().Estimations?.First().CreationDate);
            //facade.SetConcept(RuleConceptChargeRequest.DaysAfterPayAndOccurrence, (chargeRequest.EstimatedDate - Convert.ToDateTime(claim.JudicialDecisionDate)).TotalDays);
            //facade.SetConcept(RuleConceptChargeRequest.PaymentRequestPolicyProductId, claim.Endorsement?.Policy.ProductId);
            //facade.SetConcept(RuleConceptChargeRequest.RiskId, claim.Modifications.Last().Coverages.Last().RiskId);****
            //facade.SetConcept(RuleConceptChargeRequest.JudicialDecisionDate, claim.JudicialDecisionDate);
        }

        internal static PAYMEN.PaymentRequest CreatePaymentRequest(PaymentRequest paymentRequest)
        {
            PAYMEN.PaymentRequest entityPaymentRequest = new PAYMEN.PaymentRequest()
            {
                Description = paymentRequest.Description,
                AccountBankCode = paymentRequest.AccountBankId,
                BranchCode = paymentRequest.Branch.Id,
                CurrencyCode = paymentRequest.Currency.Id,
                EstimatedDate = paymentRequest.EstimatedDate,
                IndividualId = paymentRequest.IndividualId,
                IsPrinted = paymentRequest.IsPrinted,
                PaymentMovementTypeCode = paymentRequest.MovementType.Id,
                Number = paymentRequest.Number,
                PaymentDate = paymentRequest.AccountingDate,
                PersonTypeCode = paymentRequest.PersonType.Id,
                PrefixCode = paymentRequest.Prefix.Id,
                RegistrationDate = paymentRequest.RegistrationDate,
                TotalAmount = paymentRequest.TotalAmount,
                TypePaymentRequestCode = paymentRequest.Type.Id,
                UserId = paymentRequest.UserId,
                PaymentMethodCode = paymentRequest.PaymentMethod.Id,
                PaymentSourceCode = paymentRequest.MovementType.Source.Id,
                TechnicalTransaction = paymentRequest.TechnicalTransaction,
                SalePointCode = paymentRequest.SalePointId
            };

            if (paymentRequest.Id > 0)
            {
                entityPaymentRequest.PaymentRequestCode = paymentRequest.Id;
            }

            return entityPaymentRequest;
        }

        internal static PAYMEN.PaymentRequest CreatePaymentRequest(PAYMEN.PaymentRequest entityPaymentRequest)
        {
            return new PAYMEN.PaymentRequest
            {
                PaymentSourceCode = entityPaymentRequest.PaymentSourceCode,
                BranchCode = entityPaymentRequest.BranchCode,
                EstimatedDate = entityPaymentRequest.EstimatedDate,
                RegistrationDate = entityPaymentRequest.RegistrationDate,                
                PaymentDate = entityPaymentRequest.PaymentDate,
                PersonTypeCode = entityPaymentRequest.PersonTypeCode,
                IndividualId = entityPaymentRequest.IndividualId,
                PaymentMethodCode = entityPaymentRequest.PaymentMethodCode,
                AccountBankCode = entityPaymentRequest.AccountBankCode,
                CurrencyCode = entityPaymentRequest.CurrencyCode,
                UserId = entityPaymentRequest.UserId,
                TotalAmount = entityPaymentRequest.TotalAmount,
                Number = entityPaymentRequest.Number,
                Description = entityPaymentRequest.Description,
                IsPrinted = entityPaymentRequest.IsPrinted,
                PaymentMovementTypeCode = entityPaymentRequest.PaymentMovementTypeCode,
                PrefixCode = entityPaymentRequest.PrefixCode,
                TypePaymentRequestCode = entityPaymentRequest.TypePaymentRequestCode,
                StatusPayment = entityPaymentRequest.StatusPayment,
                ExecutedPaymentDate = entityPaymentRequest.ExecutedPaymentDate,
                UserIdPayment = entityPaymentRequest.UserIdPayment,
                CompanyCode = entityPaymentRequest.CompanyCode,
                SalePointCode = entityPaymentRequest.SalePointCode,
                TechnicalTransaction = entityPaymentRequest.TechnicalTransaction
            };
        }


        internal static PAYMEN.PaymentRequestClaim CreateClaimPaymentRequest(PaymentRequest paymentRequest, Claim claim, Voucher voucher, int voucherConceptId)
        {
            return new PAYMEN.PaymentRequestClaim()
            {
                PaymentRequestCode = paymentRequest.Id,
                ClaimCode = claim.Id,
                SubClaim = Convert.ToInt32(claim.Modifications.Last().Coverages?.First().SubClaim),
                EstimationTypeCode = Convert.ToInt32(claim.Modifications.Last().Coverages?.First().Estimations.First().Type.Id),
                PaymentVoucherConceptCode = voucherConceptId,
                PaymentTypeCode = paymentRequest.Type.Id,
                BranchCode = claim.Branch.Id,
                PrefixCode = claim.Prefix.Id,
                ClaimNumber = claim.Number
            };
        }

        internal static PAYMEN.PaymentRequestClaim CreateClaimPaymentRequest(ChargeRequest chargeRequest, Voucher voucher, int voucherConceptId)
        {
            return new PAYMEN.PaymentRequestClaim()
            {
                PaymentRequestCode = chargeRequest.Id,
                ClaimCode = chargeRequest.Claim.Id,
                SubClaim = Convert.ToInt32(chargeRequest.Claim.Modifications.Last().Coverages?.First().SubClaim),
                EstimationTypeCode = voucher.EstimationType.Id,
                PaymentVoucherConceptCode = voucherConceptId,
                PaymentTypeCode = chargeRequest.Type.Id,
                BranchCode = chargeRequest.Claim.Branch.Id,
                PrefixCode = chargeRequest.Prefix.Id,
                ClaimNumber = chargeRequest.Claim.Number
            };
        }

        internal static PAYMEN.PaymentRequestClaim CreateClaimPaymentRequest(PAYMEN.PaymentRequestClaim entityPaymentRequestClaim, PAYMEN.PaymentRequest entityPaymentRequest, int voucherConceptId)
        {
            return new PAYMEN.PaymentRequestClaim()
            {
                PaymentRequestCode = entityPaymentRequest.PaymentRequestCode,
                ClaimCode = entityPaymentRequestClaim.ClaimCode,
                SubClaim = entityPaymentRequestClaim.SubClaim,
                EstimationTypeCode = entityPaymentRequestClaim.EstimationTypeCode,
                PaymentVoucherConceptCode = voucherConceptId,
                PaymentTypeCode = entityPaymentRequest.TypePaymentRequestCode,
                BranchCode = entityPaymentRequestClaim.BranchCode,
                PrefixCode = entityPaymentRequestClaim.PrefixCode,
                ClaimNumber = entityPaymentRequestClaim.ClaimNumber
            };
        }

        internal static PAYMEN.PaymentVoucher CreateVoucher(Voucher voucher)
        {
            PAYMEN.PaymentVoucher entityVoucher = new PAYMEN.PaymentVoucher()
            {
                VoucherTypeCode = voucher.VoucherType.Id,
                Number = voucher.Number,
                Date = voucher.Date,
                ExchangeRate = voucher.ExchangeRate,
                CurrencyCode = voucher.Currency.Id,
                PaymentRequestCode = voucher.PaymentRequestId
            };

            if (voucher.Id > 0)
            {
                entityVoucher.PaymentVoucherCode = voucher.Id;
            }

            return entityVoucher;
        }

        internal static PAYMEN.PaymentVoucher CreateCancellationVoucher(PAYMEN.PaymentVoucher entityVoucher, int paymentRequestId)
        {
            return new PAYMEN.PaymentVoucher()
            {
                VoucherTypeCode = entityVoucher.VoucherTypeCode,
                Number = entityVoucher.Number,
                Date = entityVoucher.Date,
                ExchangeRate = entityVoucher.ExchangeRate,
                CurrencyCode = entityVoucher.CurrencyCode,
                PaymentRequestCode = paymentRequestId
            };
        }

        internal static PAYMEN.PaymentVoucherConcept CreateVoucherConcept(VoucherConcept voucherConcept, int voucherId)
        {
            PAYMEN.PaymentVoucherConcept entityVoucherConcept = new PAYMEN.PaymentVoucherConcept()
            {
                PaymentVoucherConceptCode = voucherConcept.Id,
                PaymentConceptCode = voucherConcept.PaymentConcept.Id,
                PaymentVoucherCode = voucherId,
                Value = voucherConcept.Value,
                TaxValue = voucherConcept.TaxValue,
                Retention = voucherConcept.Retention,
                CostCenter = voucherConcept.CostCenter
            };

            if (voucherConcept.Id > 0)
            {
                entityVoucherConcept.PaymentVoucherCode = voucherConcept.Id;
            }

            return entityVoucherConcept;
        }

        internal static PAYMEN.PaymentVoucherConcept CreateCancellationVoucherConcept(PAYMEN.PaymentVoucherConcept entityPaymentVoucherConcept, int voucherId)
        {
            return new PAYMEN.PaymentVoucherConcept()
            {
                PaymentConceptCode = entityPaymentVoucherConcept.PaymentConceptCode,
                PaymentVoucherCode = voucherId,
                Value = entityPaymentVoucherConcept.Value * -1,
                TaxValue = entityPaymentVoucherConcept.TaxValue * -1,
                Retention = entityPaymentVoucherConcept.Retention * -1,
                CostCenter = entityPaymentVoucherConcept.CostCenter
            };
        }

        internal static PAYMEN.PaymentVoucherConceptTax CreateVoucherConceptTax(VoucherConceptTax voucherConceptTax, int voucherConceptId)
        {
            PAYMEN.PaymentVoucherConceptTax entityPaymentRequestTax = new PAYMEN.PaymentVoucherConceptTax();

            entityPaymentRequestTax.PaymentVoucherConceptCode = voucherConceptId;
            entityPaymentRequestTax.TaxCode = voucherConceptTax.TaxId;
            entityPaymentRequestTax.TaxConditionCode = voucherConceptTax.ConditionId;
            entityPaymentRequestTax.TaxCategoryCode = voucherConceptTax.CategoryId;
            entityPaymentRequestTax.TaxValue = Math.Round(voucherConceptTax.TaxValue + voucherConceptTax.Retention, 2, MidpointRounding.ToEven);
            entityPaymentRequestTax.TaxRate = voucherConceptTax.TaxRate;
            entityPaymentRequestTax.TaxBaseAmount = voucherConceptTax.TaxBaseAmount;

            return entityPaymentRequestTax;
        }

        internal static PAYMEN.PaymentVoucherConceptTax CreateCancellationVoucherConceptTax(PAYMEN.PaymentVoucherConceptTax entityPaymentVoucherConceptTax, int voucherConceptId)
        {
            PAYMEN.PaymentVoucherConceptTax entityPaymentRequestTax = new PAYMEN.PaymentVoucherConceptTax();

            entityPaymentRequestTax.PaymentVoucherConceptCode = voucherConceptId;
            entityPaymentRequestTax.TaxCode = entityPaymentVoucherConceptTax.TaxCode;
            entityPaymentRequestTax.TaxConditionCode = entityPaymentVoucherConceptTax.TaxConditionCode;
            entityPaymentRequestTax.TaxCategoryCode = entityPaymentVoucherConceptTax.TaxCategoryCode;
            entityPaymentRequestTax.TaxValue = entityPaymentVoucherConceptTax.TaxValue * -1;
            entityPaymentRequestTax.TaxRate = entityPaymentVoucherConceptTax.TaxRate;
            entityPaymentRequestTax.TaxBaseAmount = entityPaymentVoucherConceptTax.TaxBaseAmount * -1;

            return entityPaymentRequestTax;
        }

        internal static PAYMEN.PaymentRequestCancellation CreatePaymentRequestCancellation(int paymentRequestId, int paymentRequestCancellationId)
        {
            return new PAYMEN.PaymentRequestCancellation(paymentRequestId, paymentRequestCancellationId);
        }

        internal static PAYMEN.PaymentPlan CreatePaymentPlan(PaymentPlan paymentPlan)
        {
            PAYMEN.PaymentPlan entityPaymentPlan = new PAYMEN.PaymentPlan()
            {
                ClassPaymentCode = paymentPlan.PaymentClass.Id,
                CurrencyCode = paymentPlan.Currency.Id,
                TaxPercentage = paymentPlan.Tax
            };

            if (paymentPlan.Id > 0)
            {
                entityPaymentPlan.PaymentPlanCode = paymentPlan.Id;
            }

            return entityPaymentPlan;
        }

        public static PAYMEN.PaymentSchedule CreatePaymentSchedule(PaymentQuota paymentQuota, int paymentPlanId)
        {
            PAYMEN.PaymentSchedule entityPaymentSchedule = new PAYMEN.PaymentSchedule()
            {
                PaymentPlanCode = paymentPlanId,
                NetAmount = paymentQuota.Amount,
                ExpirationDate = paymentQuota.ExpirationDate,
                PaymentNum = paymentQuota.Number
            };

            if (paymentQuota.Id > 0)
            {
                entityPaymentSchedule.PaymentScheduleCode = paymentQuota.Id;
            }

            return entityPaymentSchedule;
        }

        internal static PAYMEN.PaymentRequest CreateChargeRequest(ChargeRequest chargeRequest)
        {
            PAYMEN.PaymentRequest entityPaymentRequest = new PAYMEN.PaymentRequest()
            {
                Description = chargeRequest.Description,
                AccountBankCode = chargeRequest.AccountBankId,
                BranchCode = chargeRequest.Branch.Id,
                CurrencyCode = chargeRequest.Currency.Id,
                EstimatedDate = chargeRequest.EstimatedDate,
                IndividualId = chargeRequest.IndividualId,
                IsPrinted = chargeRequest.IsPrinted,
                PaymentMovementTypeCode = chargeRequest.MovementType.Id,
                Number = chargeRequest.Number,
                PaymentDate = chargeRequest.AccountingDate,
                PersonTypeCode = chargeRequest.PersonType.Id,
                PrefixCode = chargeRequest.Prefix.Id,
                RegistrationDate = chargeRequest.RegistrationDate,
                TotalAmount = chargeRequest.Voucher.Concepts.Sum(x => x.Value),
                TypePaymentRequestCode = chargeRequest.Type.Id,
                UserId = chargeRequest.UserId,
                PaymentMethodCode = chargeRequest.PaymentMethod.Id,
                PaymentSourceCode = chargeRequest.MovementType.Source.Id,
                TechnicalTransaction = chargeRequest.TechnicalTransaction
            };

            if (chargeRequest.Id > 0)
            {
                entityPaymentRequest.PaymentRequestCode = chargeRequest.Id;
            }

            return entityPaymentRequest;
        }

        internal static PAYMEN.PaymentRecovery CreatePaymentRecovery(ChargeRequest chargeRequest)
        {
            return new PAYMEN.PaymentRecovery
            {
                PaymentRequestCode = chargeRequest.Id,
                SalvageCode = chargeRequest.SalvageId,
                RecoveryCode = chargeRequest.RecoveryId
            };
        }

        internal static INTEN.ClmPaymentControl CreatePaymentRequestControl(PaymentRequestControl paymentRequestControl)
        {
            return new INTEN.ClmPaymentControl
            {
                PaymentRequestCode = paymentRequestControl.PaymentRequestId,
                Action = paymentRequestControl.Action
            };
        }

        #endregion

        #region Salvage

        public static CLMEN.Salvage CreateSalvage(Salvage salvage)
        {
            return new CLMEN.Salvage(salvage.Id)
            {
                CreationDate = salvage.CreationDate,
                AssignmentDate = salvage.AssignmentDate,
                EndDate = salvage.EndDate,
                Description = salvage.Description,
                EstimatedSale = salvage.EstimatedSale,
                Location = salvage.Location,
                Observation = salvage.Observations,
                ClaimCode = salvage.ClaimId,
                SubclaimCode = salvage.SubClaimId,
                BranchCode = salvage.Branch.Id,
                PrefixCode = salvage.Prefix.Id,
                ClaimNumber = salvage.ClaimNumber,
                UnitQuantity = salvage.UnitsQuantity
            };
        }

        public static CLMEN.Sale CreateSale(Sale sale, int salvageId)
        {
            CLMEN.Sale entitySale = new CLMEN.Sale()
            {
                SalvageCode = salvageId,
                Date = sale.CreationDate,
                CancellationDate = sale.CancellationDate,
                Description = sale.Description,
                TotalAmount = sale.TotalAmount,
                Buyer = sale.Buyer.Id,
                CancellationReasonCode = sale.CancellationReason.Id,
                SoldQuantity = sale.SoldQuantity,
                PaymentPlanCode = sale.PaymentPlan.Id,
                IsParticipant = sale.IsParticipant
            };

            if (sale.Id > 0)
            {
                entitySale.SaleCode = sale.Id;
            }

            return entitySale;
        }

        internal static CLMEN.ClaimCoverageCoinsurance CreateClaimCoverageCoInsurance(ClaimCoverageCoInsurance claimCoverageCoInsurance, int claimcoverage, int companyId, int estimationTypeId)
        {
            return new CLMEN.ClaimCoverageCoinsurance(claimcoverage, companyId, estimationTypeId)
            {
                ClaimCoverageCode = claimcoverage,
                CompanyCode = companyId,
                CoverageCode = claimCoverageCoInsurance.CoverageId,
                CurrencyCode = claimCoverageCoInsurance.CurrencyId,
                Date = claimCoverageCoInsurance.Date,
                DeductibleAmount = claimCoverageCoInsurance.DeducibleAmount,
                EstimationAmount = claimCoverageCoInsurance.EstimationAmount,
                EstimateAmountAccumulate = Convert.ToDecimal(claimCoverageCoInsurance.EstimationAmountAccumulate),
                EstimationTypeCode = estimationTypeId,
                EstimationTypeStatusCode = claimCoverageCoInsurance.EstimationTypeStatusId,
                EstimationTypeStatusReasonCode = claimCoverageCoInsurance.EstimationTypeStatusReasonId,
                PartCiaPct = claimCoverageCoInsurance.PaticipationCompany,
                VersionCode = claimCoverageCoInsurance.VersionId
            };
        }


        #endregion

        #region
        internal static CLMEN.ClaimSearchPersonType CreateClaimSearchPersonType(ClaimSearchPersonType claimSearchPersonType)
        {
            return new CLMEN.ClaimSearchPersonType(claimSearchPersonType.PersonTypeId, claimSearchPersonType.PrefixId, claimSearchPersonType.SearchType)
            {
                PersonTypeCode = claimSearchPersonType.PersonTypeId,
                PrefixCode = claimSearchPersonType.PrefixId,
                SearchType = claimSearchPersonType.SearchType
            };
        }
        #endregion
    }
}