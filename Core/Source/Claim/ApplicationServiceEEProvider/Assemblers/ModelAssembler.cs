using Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using Sistran.Core.Application.ClaimServices.DTOs.Recovery;
using System;
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;
using System.Linq;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using UNDMO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using VHMO = Sistran.Core.Integration.VehicleServices.DTOs;
using GLWKDTO = Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;
using Newtonsoft.Json;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Assemblers
{
    static internal class ModelAssembler
    {
        #region Claims

        #region EntityToModel

        internal static CauseCoverage CreateCauseCoverage(CLMEN.CauseCoverage entityCauseCoverage)
        {
            return new CauseCoverage
            {
                Id = entityCauseCoverage.CoverageId,
                LackPeriod = Convert.ToInt32(entityCauseCoverage.LackPeriod),
                Cause = new Cause
                {
                    Id = entityCauseCoverage.CauseId
                }
            };
        }

        internal static List<CauseCoverage> CreateCauseCoverages(BusinessCollection businessCollection)
        {
            List<CauseCoverage> causeCoverages = new List<CauseCoverage>();

            foreach (CLMEN.CauseCoverage entityCauseCoverage in businessCollection)
            {
                causeCoverages.Add(CreateCauseCoverage(entityCauseCoverage));
            }

            return causeCoverages;
        }

        internal static Status CreateStatus(PARAMEN.EstimationTypeStatus entityEstimationTypeStatus)
        {
            return new Status
            {
                Id = entityEstimationTypeStatus.EstimationTypeStatusCode,
                Description = entityEstimationTypeStatus.Description,
                IsEnabled = Convert.ToBoolean(entityEstimationTypeStatus.Enabled),
                InternalStatus = new InternalStatus
                {
                    Id = entityEstimationTypeStatus.InternalStatusCode
                }
            };
        }

        internal static List<Status> CreateStatuses(BusinessCollection businessCollection)
        {
            List<Status> listStatus = new List<Status>();

            foreach (PARAMEN.EstimationTypeStatus entityEstimationTypeStatus in businessCollection)
            {
                listStatus.Add(CreateStatus(entityEstimationTypeStatus));
            }

            return listStatus;
        }

        internal static PaymentRequestCoInsurance CreatePaymentRequestCoInsurance(PaymentRequest paymentRequest, CoInsuranceAssigned coInsuranceAssigned)
        {
            return new PaymentRequestCoInsurance
            {
                PaymentRequestId = paymentRequest.Id,
                CompanyId = coInsuranceAssigned.InsuranceCompanyId,
                CoverageId = paymentRequest.CoverageId,
                CurrencyId = paymentRequest.Currency.Id,
                IndividualId = paymentRequest.IndividualId,
                Amount = Convert.ToDecimal((paymentRequest.Claims.Sum(x => x.Vouchers.Sum(y => y.Concepts.Sum(z => z.Value))) * coInsuranceAssigned.PartCiaPercentage) / 100),
                Number = paymentRequest.Number,
                PartCiaPct = coInsuranceAssigned.PartCiaPercentage,
                UserId = paymentRequest.UserId,
                TypePaymentRequestId = paymentRequest.Type.Id
            };
        }

        internal static PaymentRequestCoInsurance CreatePaymentRequestCoInsurance(PaymentRequest paymentRequest, CoInsuranceAssigned coInsuranceAssigned, int claimCoverageId)
        {
            return new PaymentRequestCoInsurance
            {
                PaymentRequestId = paymentRequest.Id,
                CompanyId = coInsuranceAssigned.InsuranceCompanyId,
                CoverageId = claimCoverageId,
                CurrencyId = paymentRequest.Currency.Id,
                IndividualId = paymentRequest.IndividualId,
                Amount = Convert.ToDecimal((paymentRequest.Claims.Sum(x => x.Vouchers.Sum(y => y.Concepts.Sum(z => z.Value))) * coInsuranceAssigned.PartCiaPercentage) / 100),
                Number = paymentRequest.Number,
                PartCiaPct = coInsuranceAssigned.PartCiaPercentage,
                UserId = paymentRequest.UserId,
                TypePaymentRequestId = paymentRequest.Type.Id
            };
        }

        internal static ClaimSupplier CreateSupplier(UPEN.Person entityPerson, UPEN.Supplier entitySupplier)
        {
            return new ClaimSupplier
            {
                Id = entitySupplier.SupplierCode,
                IsEnabled = Convert.ToBoolean(entitySupplier.Enabled),
                DeclinedDate = Convert.ToDateTime(entitySupplier.DeclinedDate),
                IndividualId = entityPerson.IndividualId,
                FullName = entityPerson.Surname + " " + (string.IsNullOrEmpty(entityPerson.MotherLastName) ? "" : entityPerson.MotherLastName + " ") + entityPerson.Name,
                DocumentTypeId = entityPerson.IdCardTypeCode,
                DocumentNumber = entityPerson.IdCardNo
            };
        }

        internal static ClaimSupplier CreateSupplier(UPEN.Company entityCompany, UPEN.Supplier entitySupplier)
        {
            return new ClaimSupplier
            {
                Id = entitySupplier.SupplierCode,
                IsEnabled = Convert.ToBoolean(entitySupplier.Enabled),
                DeclinedDate = Convert.ToDateTime(entitySupplier.DeclinedDate),
                IndividualId = entityCompany.IndividualId,
                FullName = entityCompany.TradeName,
                DocumentTypeId = entityCompany.TributaryIdTypeCode,
                DocumentNumber = entityCompany.TributaryIdNo
            };
        }

        internal static Status CreateStatusByEstimationType(PARAMEN.RelationTypeStatus entityRelationTypeStatus)
        {
            return new Status
            {
                Id = entityRelationTypeStatus.EstimationTypeStatusCode
            };
        }

        internal static List<Status> CreateStatusesByEstimationType(BusinessCollection businessCollection)
        {
            List<Status> relationTypeStatuses = new List<Status>();

            foreach (PARAMEN.RelationTypeStatus relationTypeStatus in businessCollection)
            {
                relationTypeStatuses.Add(CreateStatusByEstimationType(relationTypeStatus));
            }

            return relationTypeStatuses;
        }

        internal static Reason CreateReason(PARAMEN.EstimationTypeStatusReason entityReason)
        {
            return new Reason
            {
                Id = entityReason.EstimationTypeStatusReasonCode,
                Description = entityReason.Description,
                Prefix = new Prefix
                {
                    Id = entityReason.PrefixCode
                },
                Status = new Status
                {
                    Id = entityReason.EstimationTypeStatusCode
                }
            };
        }

        internal static List<Reason> CreateEstimationTypesStatusReasons(BusinessCollection businessCollection)
        {
            List<Reason> estimationTypeStatus = new List<Reason>();

            foreach (PARAMEN.EstimationTypeStatusReason entityEstimationTypeStatusReason in businessCollection)
            {
                estimationTypeStatus.Add(CreateReason(entityEstimationTypeStatusReason));
            }

            return estimationTypeStatus;
        }

        internal static ClaimCoverageCoInsurance CreateClaimCoverageCoInsurance(Estimation estimation, ClaimCoverage claimCoverage, CoInsuranceAssigned coInsuranceAssigned)
        {
            return new ClaimCoverageCoInsurance
            {
                ClaimCoverageId = claimCoverage.Id,
                CoverageId = claimCoverage.CoverageId,
                CompanyId = coInsuranceAssigned.CompanyNum,
                CurrencyId = estimation.Currency.Id,
                Date = DateTime.Now,
                DeducibleAmount = estimation.DeductibleAmount,
                EstimationAmount = (estimation.Amount * coInsuranceAssigned.PartCiaPercentage) / 100,
                EstimationTypeId = estimation.Type.Id,
                EstimationTypeStatusId = estimation.Reason.Status.Id,
                EstimationTypeStatusReasonId = estimation.Reason.Id,
                EstimationAmountAccumulate = (estimation.AmountAccumulate * coInsuranceAssigned.PartCiaPercentage) / 100,
                PaticipationCompany = coInsuranceAssigned.PartCiaPercentage
            };
        }

        internal static PendingOperation CreatePendingOperationByClaimVehicle(ClaimVehicle claimVehicle)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimVehicle.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimVehicle)
            };
        }

        internal static EstimationType CreateEstimationType(PARAMEN.EstimationType entityEstimationType)
        {
            return new EstimationType
            {
                Id = entityEstimationType.EstimateTypeCode,
                Description = entityEstimationType.Description,
                IsEnabled = Convert.ToBoolean(entityEstimationType.Enabled),
                ShowSummary = Convert.ToBoolean(entityEstimationType.ShowSummary)
            };
        }

        internal static List<AmountType> CreateAmountTypes(BusinessCollection businessCollection)
        {
            List<AmountType> amountTypes = new List<AmountType>();

            foreach (PARAMEN.AmountType entityAmountType in businessCollection)
            {
                amountTypes.Add(CreateAmountType(entityAmountType));
            }

            return amountTypes;
        }

        internal static AmountType CreateAmountType(PARAMEN.AmountType entityamountType)
        {
            return new AmountType
            {
                Id = entityamountType.AmountTypeId,
                Description = entityamountType.Description
            };
        }


        internal static MinimumSalary CreateMinimumSalaryByYear(COMMEN.MinimumSalary entityMinimumSalary)
        {
            if (entityMinimumSalary == null)
                return null;

            return new MinimumSalary
            {
                SalaryMinimumDay = entityMinimumSalary.SalaryMinimumDay,
                SalaryMinimumMounth = entityMinimumSalary.SalaryMinimumMonth,
                year = entityMinimumSalary.Year
            };
        }

        internal static List<EstimationType> CreateEstimationTypes(BusinessCollection businessCollection)
        {
            List<EstimationType> estimationTypes = new List<EstimationType>();

            foreach (PARAMEN.EstimationType entityEstimationType in businessCollection)
            {
                estimationTypes.Add(CreateEstimationType(entityEstimationType));
            }

            return estimationTypes;
        }

        internal static InternalStatus CreateInternalStatus(PARAMEN.EstimationTypeInternalStatus entityInternalStatus)
        {
            return new InternalStatus
            {
                Id = entityInternalStatus.InternalStatusCode,
                Description = entityInternalStatus.Description
            };
        }

        internal static PendingOperation CreatePendingOperationByClaimSurety(ClaimSurety claimSurety)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimSurety.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimSurety)
            };
        }

        internal static List<InternalStatus> CreateInternalStatuses(BusinessCollection businessCollection)
        {
            List<InternalStatus> estimationTypeInternalStatus = new List<InternalStatus>();

            foreach (PARAMEN.EstimationTypeInternalStatus entityEstimationTypeInternalStatus in businessCollection)
            {
                estimationTypeInternalStatus.Add(CreateInternalStatus(entityEstimationTypeInternalStatus));
            }

            return estimationTypeInternalStatus;
        }

        internal static List<Estimation> CreateEstimations(List<CLMEN.ClaimCoverageAmount> entityClaimCoverageAmounts)
        {
            List<Estimation> estimations = new List<Estimation>();

            foreach (CLMEN.ClaimCoverageAmount entityClaimCoverageAmount in entityClaimCoverageAmounts)
            {
                estimations.Add(CreateEstimation(entityClaimCoverageAmount));
            }

            return estimations;
        }

        internal static Status CreateEstimationTypeStatus(PARAMEN.EstimationTypeInternalStatus estimationTypeInternalStatus)
        {
            return new Status
            {
                Id = estimationTypeInternalStatus.InternalStatusCode,
                Description = estimationTypeInternalStatus.Description
            };
        }

        internal static PendingOperation CreatePendingOperationByClaimLocation(ClaimLocation claimLocation)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimLocation.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimLocation)
            };
        }

        internal static Claim createPaymentClaim(CLMEN.PaymentRequestClaim entityPaymentRequestClaim)
        {
            return new Claim
            {
                Id = entityPaymentRequestClaim.ClaimCode,
                Number = Convert.ToInt32(entityPaymentRequestClaim.ClaimNumber),
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entityPaymentRequestClaim.BranchCode)
                },
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityPaymentRequestClaim.PrefixCode)
                }
            };
        }

        internal static Estimation CreateEstimation(CLMEN.ClaimCoverageAmount entityClaimCoverageAmount)
        {
            return new Estimation
            {
                Type = new EstimationType
                {
                    Id = entityClaimCoverageAmount.EstimationTypeCode
                },
                Reason = new Reason
                {
                    Id = Convert.ToInt32(entityClaimCoverageAmount.EstimationTypeStatusReasonCode),
                    Status = new Status
                    {
                        Id = Convert.ToInt32(entityClaimCoverageAmount.EstimationTypeStatusCode)
                    }
                },
                Amount = Convert.ToDecimal(entityClaimCoverageAmount.EstimateAmount),
                DeductibleAmount = Convert.ToDecimal(entityClaimCoverageAmount.DeductibleAmount),
                Version = Convert.ToInt32(entityClaimCoverageAmount.VersionCode),
                CreationDate = Convert.ToDateTime(entityClaimCoverageAmount.Date),
                Currency = new Currency
                {
                    Id = Convert.ToInt32(entityClaimCoverageAmount.CurrencyCode)
                },
                AmountAccumulate = Convert.ToDecimal(entityClaimCoverageAmount.EstimateAmountAccumulate),
                IsMinimumSalary = Convert.ToBoolean(entityClaimCoverageAmount.IsMinimumSalary),
                MinimumSalariesNumber = Convert.ToDecimal(entityClaimCoverageAmount.MinimumSalariesNumber),
                MinimumSalaryValue = entityClaimCoverageAmount.MinimumSalaryValue,
                ExchangeRate = entityClaimCoverageAmount.ExchangeRate
            };
        }

        public static Cause CreateCause(CLMEN.Cause entityCause)
        {
            return new Cause
            {
                Id = entityCause.CauseId,
                Description = entityCause.Description,
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityCause.PrefixCode)
                },
                PoliceComplaintRequired = Convert.ToBoolean(entityCause.PoliceComplaint),
                DriverInformationRequired = Convert.ToBoolean(entityCause.DriverInformation),
                InspectionDateRequired = Convert.ToBoolean(entityCause.InspectionDateRequired)
            };
        }
        public static SubCause CreateSubCause(CLMEN.SubCause entitySubcause)
        {
            return new SubCause
            {
                Id = entitySubcause.SubcauseId,
                Description = entitySubcause.Description,
                Cause = new Cause
                {
                    Id = Convert.ToInt32(entitySubcause.CauseCode)
                },

            };
        }

        internal static Currency CreateCurrency(COMMEN.Currency entityCurrency)
        {
            return new Currency
            {
                Id = entityCurrency.CurrencyCode,
                Description = entityCurrency.Description
            };
        }

        internal static ClaimCoverage CreateClaimCoverage(CLMEN.ClaimCoverage entityClaimCoverage)
        {
            if (entityClaimCoverage == null)
            {
                return null;
            }

            return new ClaimCoverage
            {
                Id = entityClaimCoverage.ClaimCoverageCode,
                SubClaim = Convert.ToInt32(entityClaimCoverage.SubClaim),
                RiskId = Convert.ToInt32(entityClaimCoverage.RiskId),
                RiskNumber = entityClaimCoverage.RiskNum,
                CoverageId = Convert.ToInt32(entityClaimCoverage.CoverageId),
                CoverageNumber = entityClaimCoverage.CoverageNum,
                IsInsured = Convert.ToBoolean(entityClaimCoverage.IsInsured),
                EndorsementId = Convert.ToInt32(entityClaimCoverage.EndorsementId),
                IsProspect = Convert.ToBoolean(entityClaimCoverage.IsProspect),
                TextOperation = new TextOperation
                {
                    Id = Convert.ToInt32(entityClaimCoverage.TextOperationCode)
                },
                IndividualId = entityClaimCoverage.IndividualId,
                Estimations = new List<Estimation>(),
                IsClaimedAmount = entityClaimCoverage.IsClaimedAmount,
                ClaimedAmount = Convert.ToDecimal(entityClaimCoverage.ClaimedAmount)
            };
        }

        internal static List<ClaimCoverage> CreateClaimCoverages(BusinessCollection businessCollection)
        {
            List<ClaimCoverage> claimCoverages = new List<ClaimCoverage>();

            foreach (CLMEN.ClaimCoverage entityClaimCoverage in businessCollection)
            {
                claimCoverages.Add(CreateClaimCoverage(entityClaimCoverage));
            }

            return claimCoverages;
        }

        public static List<Cause> CreateCauses(BusinessCollection businessCollection)
        {
            List<Cause> causes = new List<Cause>();

            foreach (CLMEN.Cause causeEntity in businessCollection)
            {
                causes.Add(CreateCause(causeEntity));
            }

            return causes;
        }

        public static List<SubCause> CreateSubCause(BusinessCollection businessCollection)
        {
            List<SubCause> subCauses = new List<SubCause>();

            foreach (CLMEN.SubCause entitySubcause in businessCollection)
            {
                subCauses.Add(CreateSubCause(entitySubcause));
            }

            return subCauses;
        }
        public static Catastrophe CreateCatastrophe(PARAMEN.Catastrophe entityCatastrophe)
        {
            return new Catastrophe
            {
                Id = entityCatastrophe.CatastropheCode,
                Description = entityCatastrophe.Description
            };
        }

        public static List<Catastrophe> CreateCatastrophes(BusinessCollection businessCollection)
        {
            List<Catastrophe> catastrophes = new List<Catastrophe>();

            foreach (PARAMEN.Catastrophe catastropheEntity in businessCollection)
            {
                catastrophes.Add(CreateCatastrophe(catastropheEntity));
            }

            return catastrophes;
        }

        internal static List<Claim> CreateClaims(BusinessCollection businessObjects)
        {
            List<Claim> claims = new List<Claim>();
            foreach (CLMEN.Claim entityClaim in businessObjects)
            {
                claims.Add(CreateClaim(entityClaim));
            }
            return claims;
        }
        internal static Claim CreateClaim(CLMEN.Claim entityClaim, Voucher voucher)
        {
            if (entityClaim == null)
            {
                return null;
            }

            Claim claim = new Claim
            {
                Id = entityClaim.ClaimCode,
                OccurrenceDate = Convert.ToDateTime(entityClaim.OccurrenceDate),
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entityClaim.ClaimBranchCode)
                },
                Endorsement = new ClaimEndorsement
                {
                    Id = Convert.ToInt32(entityClaim.EndorsementId),
                    PolicyId = Convert.ToInt32(entityClaim.PolicyId),
                    PolicyNumber = Convert.ToString(entityClaim.DocumentNumber)
                },
                BusinessTypeId = Convert.ToInt32(entityClaim.BusinessTypeCode),
                IndividualId = Convert.ToInt32(entityClaim.IndividualId),
                Number = Convert.ToInt32(entityClaim.Number),
                NoticeId = Convert.ToInt32(entityClaim.ClaimNoticeCode),
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityClaim.PrefixCode)
                },
                Modifications = new List<ClaimModify>(),
                City = new City
                {
                    Id = Convert.ToInt32(entityClaim.CityCode),
                    State = new State
                    {
                        Id = Convert.ToInt32(entityClaim.StateCode),
                        Country = new Country
                        {
                            Id = Convert.ToInt32(entityClaim.CountryCode)
                        }
                    }
                },
                Location = entityClaim.Location,
                DamageResponsability = new DamageResponsibility
                {
                    Id = Convert.ToInt32(entityClaim.ClaimDamageResponsibilityCode)
                },
                DamageType = new DamageType
                {
                    Id = Convert.ToInt32(entityClaim.ClaimDamageTypeCode)
                },
                Cause = new Cause
                {
                    Id = Convert.ToInt32(entityClaim.CauseId)
                },
                TextOperation = new TextOperation
                {
                    Id = Convert.ToInt32(entityClaim.TextOperationCode)
                },
                IsTotalParticipation = entityClaim.IsTotalParticipation,
                JudicialDecisionDate = entityClaim.JudicialDecisionDate
            };

            if (entityClaim.ClaimNoticeCode.HasValue)
            {
                claim.NoticeId = Convert.ToInt32(entityClaim.ClaimNoticeCode);
                claim.NoticeDate = Convert.ToDateTime(entityClaim.NoticeDate);
            }

            return claim;
        }
        internal static Claim CreateClaim(CLMEN.Claim entityClaim)
        {
            if (entityClaim == null)
            {
                return null;
            }

            Claim claim = new Claim
            {
                Id = entityClaim.ClaimCode,
                OccurrenceDate = Convert.ToDateTime(entityClaim.OccurrenceDate),
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entityClaim.ClaimBranchCode)
                },
                Endorsement = new ClaimEndorsement
                {
                    Id = Convert.ToInt32(entityClaim.EndorsementId),
                    PolicyId = Convert.ToInt32(entityClaim.PolicyId),
                    PolicyNumber = Convert.ToString(entityClaim.DocumentNumber)
                },
                BusinessTypeId = Convert.ToInt32(entityClaim.BusinessTypeCode),
                IndividualId = Convert.ToInt32(entityClaim.IndividualId),
                Number = Convert.ToInt32(entityClaim.Number),
                NoticeId = Convert.ToInt32(entityClaim.ClaimNoticeCode),
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityClaim.PrefixCode)
                },
                Modifications = new List<ClaimModify>(),
                City = new City
                {
                    Id = Convert.ToInt32(entityClaim.CityCode),
                    State = new State
                    {
                        Id = Convert.ToInt32(entityClaim.StateCode),
                        Country = new Country
                        {
                            Id = Convert.ToInt32(entityClaim.CountryCode)
                        }
                    }
                },
                Location = entityClaim.Location,
                DamageResponsability = new DamageResponsibility
                {
                    Id = Convert.ToInt32(entityClaim.ClaimDamageResponsibilityCode)
                },
                DamageType = new DamageType
                {
                    Id = Convert.ToInt32(entityClaim.ClaimDamageTypeCode)
                },
                Cause = new Cause
                {
                    Id = Convert.ToInt32(entityClaim.CauseId)
                },
                TextOperation = new TextOperation
                {
                    Id = Convert.ToInt32(entityClaim.TextOperationCode)
                },
                IsTotalParticipation = entityClaim.IsTotalParticipation,
                JudicialDecisionDate = entityClaim.JudicialDecisionDate
            };

            if (entityClaim.ClaimNoticeCode.HasValue)
            {
                claim.NoticeId = Convert.ToInt32(entityClaim.ClaimNoticeCode);
                claim.NoticeDate = Convert.ToDateTime(entityClaim.NoticeDate);
            }

            return claim;
        }

        internal static ClaimModify CreateClaimModify(CLMEN.ClaimModify entityClaimModify)
        {
            return new ClaimModify
            {
                Id = entityClaimModify.ClaimModifyCode,
                RegistrationDate = entityClaimModify.RegistrationDate,
                AccountingDate = entityClaimModify.AccountingDate,
                UserId = entityClaimModify.UserId,
                Coverages = new List<ClaimCoverage>()
            };
        }

        internal static List<ClaimModify> CreateClaimModifies(CLMEN.ClaimModify entityClaimModify)
        {
            List<ClaimModify> claimModifies = new List<ClaimModify>();

            claimModifies.Add(CreateClaimModify(entityClaimModify));

            return claimModifies;
        }

        internal static List<Notice> CreateNotices(BusinessCollection businessObjects)
        {
            List<Notice> notices = new List<Notice>();
            foreach (CLMEN.ClaimNotice entityClaimNotice in businessObjects)
            {
                notices.Add(CreateNotice(entityClaimNotice));
            }
            return notices;
        }

        internal static Notice CreateNotice(CLMEN.ClaimNotice entityClaimNotice)
        {
            Notice notice = new Notice
            {
                Id = entityClaimNotice.ClaimNoticeCode,
                ClaimDate = Convert.ToDateTime(entityClaimNotice.ClaimDate),
                CreationDate = entityClaimNotice.ClaimNoticeDate,
                Address = entityClaimNotice.Location,
                City = new City
                {
                    Id = Convert.ToInt32(entityClaimNotice.CityCode),
                    State = new State
                    {
                        Id = Convert.ToInt32(entityClaimNotice.StateCode),
                        Country = new Country
                        {
                            Id = Convert.ToInt32(entityClaimNotice.CountryCode)
                        }
                    }
                },
                Description = entityClaimNotice.Description,
                ObjectedReason = entityClaimNotice.ObjectedDescription,
                UserId = Convert.ToInt32(entityClaimNotice.UserId),
                CoveredRiskTypeId = entityClaimNotice.CoveredRiskTypeCode,
                Type = new NoticeType
                {
                    Id = Convert.ToInt32(entityClaimNotice.ClaimNoticeTypeId)
                },
                Risk = new Risk
                {
                    RiskId = Convert.ToInt32(entityClaimNotice.RiskId)
                },
                DamageResponsability = new DamageResponsibility
                {
                    Id = Convert.ToInt32(entityClaimNotice.ClaimDamageResponsibilityCode)
                },
                DamageType = new DamageType
                {
                    Id = Convert.ToInt32(entityClaimNotice.ClaimDamageTypeCode)
                },
                NoticeReason = new NoticeReason
                {
                    Id = Convert.ToInt32(entityClaimNotice.ClaimNoticeReasonCode)
                },
                Number = entityClaimNotice.Number,
                NumberObjected = entityClaimNotice.NumberObjected,
                IndividualId = entityClaimNotice.IndividualId,
                OthersAffected = entityClaimNotice.OthersAffected,
                InternalConsecutive = entityClaimNotice.InternalConsecutive,
                ClaimedAmount = Convert.ToDecimal(entityClaimNotice.ClaimedAmount),
                ClaimReasonOthers = entityClaimNotice.ClaimReasonOthers,
                NoticeState = new NoticeState
                {
                    Id = entityClaimNotice.ClaimNoticeStateCode
                }
            };

            if (entityClaimNotice.EndorsementId.HasValue)
            {
                notice.Endorsement = new ClaimEndorsement
                {
                    Id = Convert.ToInt32(entityClaimNotice.EndorsementId),
                    PolicyId = Convert.ToInt32(entityClaimNotice.PolicyId)
                };
            }

            if (entityClaimNotice.PolicyId.HasValue)
            {
                notice.Policy = new Policy
                {
                    Id = Convert.ToInt32(entityClaimNotice.PolicyId)
                };
            }

            if (!string.IsNullOrEmpty(entityClaimNotice.Latitude) && !string.IsNullOrEmpty(entityClaimNotice.Longitude))
            {
                notice.Latitude = Convert.ToDecimal(entityClaimNotice.Latitude);
                notice.Longitude = Convert.ToDecimal(entityClaimNotice.Longitude);
            }

            return notice;
        }

        internal static ClaimEndorsement CreateClaimEndorsement(ISSEN.Endorsement entityEndorsement)
        {
            if (entityEndorsement == null)
            {
                return null;
            }

            return new ClaimEndorsement
            {
                Id = entityEndorsement.EndorsementId,
                PolicyId = entityEndorsement.PolicyId,
                Number = entityEndorsement.DocumentNum
            };
        }

        internal static List<ClaimEndorsement> CreateClaimEndorsements(List<ISSEN.Endorsement> entityEndorsements)
        {
            List<ClaimEndorsement> claimEndorsements = new List<ClaimEndorsement>();

            foreach (ISSEN.Endorsement entityEndorsement in entityEndorsements)
            {
                claimEndorsements.Add(CreateClaimEndorsement(entityEndorsement));
            }
            return claimEndorsements;
        }

        internal static NoticeType CreateNoticeType(PARAMEN.ClaimNoticeType entityClaimNoticeType)
        {
            if (entityClaimNoticeType == null)
                return null;

            return new NoticeType
            {
                Id = entityClaimNoticeType.ClaimNoticeTypeId,
                Description = entityClaimNoticeType.Description
            };
        }

        internal static List<NoticeType> CreateNoticeTypes(BusinessCollection businessCollection)
        {
            List<NoticeType> noticeTypes = new List<NoticeType>();

            foreach (PARAMEN.ClaimNoticeType entityClaimNoticeType in businessCollection)
            {
                noticeTypes.Add(CreateNoticeType(entityClaimNoticeType));
            }

            return noticeTypes;
        }

        internal static List<EstimationType> CreateEstimationsType(BusinessCollection businessCollection)
        {
            List<EstimationType> estimationsType = new List<EstimationType>();

            foreach (PARAMEN.EstimationType estimationType in businessCollection)
            {
                estimationsType.Add(CreateEstimationType(estimationType));
            }

            return estimationsType;
        }

        internal static PendingOperation CreatePendingOperationByClaimTransport(ClaimTransport claimTransport)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimTransport.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimTransport)
            };
        }

        internal static Prefix CreateEstimationTypePrefix(PARAMEN.EstimationTypePrefix estimationType)
        {
            return new Prefix
            {
                Id = estimationType.PrefixCode
            };
        }

        internal static List<Prefix> CreateEstimationsTypePrefix(BusinessCollection businessCollection)
        {
            List<Prefix> Prefix = new List<Prefix>();

            foreach (PARAMEN.EstimationTypePrefix estimationTypePrefix in businessCollection)
            {
                Prefix.Add(CreateEstimationTypePrefix(estimationTypePrefix));
            }

            return Prefix;
        }

        internal static List<NoticeSurety> CreateClaimNoticeSureties(BusinessCollection businessCollection)
        {
            List<NoticeSurety> noticeSurities = new List<NoticeSurety>();

            foreach (CLMEN.ClaimNoticeRiskSurety claimNotice in businessCollection)
            {
                noticeSurities.Add(CreateClaimNoticeSurety(claimNotice));
            }

            return noticeSurities;
        }

        internal static NoticeSurety CreateClaimNoticeSurety(CLMEN.ClaimNoticeRiskSurety entityClaimSurety)
        {
            if (entityClaimSurety == null)
            {
                return null;
            }

            return new NoticeSurety()
            {
                CourtNum = entityClaimSurety.CourtNum,
                BidNumber = entityClaimSurety.BidNumber,
                Name = entityClaimSurety.Name,
                DocumentNumber = entityClaimSurety.IdCardNo
            };
        }

        internal static List<PaymentRequestCoInsurance> CreatePaymentRequestCoInsurances(List<PAYMEN.PaymentRequestCoinsurance> paymentRequestCoInsurance)
        {
            List<PaymentRequestCoInsurance> paymentRequestCoInsurances = new List<PaymentRequestCoInsurance>();
            foreach (PAYMEN.PaymentRequestCoinsurance paymentRequestCoinsurance in paymentRequestCoInsurance)
            {
                paymentRequestCoInsurances.Add(CreatePaymentRequestCoInsurance(paymentRequestCoinsurance));
            }

            return paymentRequestCoInsurances;
        }

        internal static PaymentRequestCoInsurance CreatePaymentRequestCoInsurance(PAYMEN.PaymentRequestCoinsurance paymentRequestCoInsurance)
        {
            if (paymentRequestCoInsurance == null)
            {
                return null;
            }

            return new PaymentRequestCoInsurance
            {
                PaymentRequestId = paymentRequestCoInsurance.PaymentRequestCode,
                CompanyId = paymentRequestCoInsurance.CompanyCode,
                CoverageId = Convert.ToInt32(paymentRequestCoInsurance.CoverageCode),
                CurrencyId = Convert.ToInt32(paymentRequestCoInsurance.CurrencyCode),
                IndividualId = Convert.ToInt32(paymentRequestCoInsurance.IndividualId),
                Amount = Convert.ToDecimal(paymentRequestCoInsurance.Amount),
                Number = Convert.ToInt32(paymentRequestCoInsurance.Number),
                PartCiaPct = Convert.ToDecimal(paymentRequestCoInsurance.PartCiaPercentage),
                UserId = Convert.ToInt32(paymentRequestCoInsurance.UserId),
                TypePaymentRequestId = Convert.ToInt32(paymentRequestCoInsurance.TypePaymentRequestCode)
            };
        }

        internal static NoticeVehicle CreateNoticeVehicle(CLMEN.ClaimNoticeVehicle entityClaimNoticeVehicle)
        {
            return new NoticeVehicle
            {
                Plate = entityClaimNoticeVehicle.Plate,
                MakeId = Convert.ToInt32(entityClaimNoticeVehicle.VehicleMakeCode),
                ModelId = Convert.ToInt32(entityClaimNoticeVehicle.VehicleModelCode),
                VersionId = Convert.ToInt32(entityClaimNoticeVehicle.VehicleVersionCode),
                Year = Convert.ToInt32(entityClaimNoticeVehicle.VehicleVersionYearCode),
                ColorId = Convert.ToInt32(entityClaimNoticeVehicle.VehicleColor),
                DriverName = entityClaimNoticeVehicle.Driver
            };
        }

        internal static PendingOperation CreatePendingOperationByClaimAircraft(ClaimAirCraft claimAirCraft)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimAirCraft.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimAirCraft)
            };
        }

        internal static ClaimLocation CreateNoticeLocation(CLMEN.ClaimNoticeRiskLocation claimNoticeRiskLocation)
        {
            return new ClaimLocation
            {
                Address = claimNoticeRiskLocation.Address,
                City = new City
                {
                    Id = Convert.ToInt32(claimNoticeRiskLocation.CityCode),
                    State = new State
                    {
                        Id = Convert.ToInt32(claimNoticeRiskLocation.StateCode),
                        Country = new Country
                        {
                            Id = Convert.ToInt32(claimNoticeRiskLocation.CountryCode)
                        }
                    }
                }
            };
        }

        internal static NoticeTransport CreateNoticeTransport(CLMEN.ClaimNoticeRiskTransport entityClaimNoticeRiskTransport)
        {
            return new NoticeTransport
            {
                CargoType = entityClaimNoticeRiskTransport.TransportCargoType,
                PackagingType = entityClaimNoticeRiskTransport.TransportPackagingType,
                Origin = entityClaimNoticeRiskTransport.Source,
                Destiny = entityClaimNoticeRiskTransport.Destiny,
                TransportType = entityClaimNoticeRiskTransport.TransportViaType
            };
        }

        internal static NoticeAirCraft CreateNoticeAirCraft(CLMEN.ClaimNoticeRiskAircraft entityClaimNoticeRiskAircraft)
        {
            return new NoticeAirCraft
            {
                MakeId = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftMakeCode),
                ModelId = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftModelCode),
                TypeId = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftTypeCode),
                UseId = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftUseCode),
                RegisterId = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftRegisterCode),
                OperatorId = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftOperatorCode),
                RegisterNumer = entityClaimNoticeRiskAircraft.AircraftRegisterNo,
                Year = Convert.ToInt32(entityClaimNoticeRiskAircraft.AircraftYear)
            };
        }

        internal static NoticeFidelity CreateNoticeFidelity(CLMEN.ClaimNoticeRiskFidelity entityClaimNoticeRiskFidelity)
        {
            if (entityClaimNoticeRiskFidelity == null)
            {
                return null;
            }

            return new NoticeFidelity
            {
                RiskCommercialClassId = entityClaimNoticeRiskFidelity.RiskCommercialClassCode,
                OccupationId = entityClaimNoticeRiskFidelity.OccupationCode,
                DiscoveryDate = entityClaimNoticeRiskFidelity.DiscoveryDate,
                Description = entityClaimNoticeRiskFidelity.Description
            };
        }

        internal static PendingOperation CreatePendingOperationByClaimFidelity(ClaimFidelity claimFidelity)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimFidelity.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimFidelity)
            };
        }

        internal static DamageType CreateDamageType(CLMEN.ClaimDamageType entityDamageType)
        {
            return new DamageType()
            {
                Id = entityDamageType.ClaimDamageTypeId,
                Description = entityDamageType.Description,
                IsEnabled = Convert.ToBoolean(entityDamageType.Enabled)
            };
        }
        internal static List<DamageType> CreateDamageTypes(BusinessCollection businessCollection)
        {
            List<DamageType> damageTypes = new List<DamageType>();

            foreach (CLMEN.ClaimDamageType entityDamageType in businessCollection)
            {
                damageTypes.Add(CreateDamageType(entityDamageType));
            }

            return damageTypes;
        }
        internal static DamageResponsibility CreateDamageResponsability(CLMEN.ClaimDamageResponsibility entityDamageResponsability)
        {
            return new DamageResponsibility()
            {
                Id = entityDamageResponsability.ClaimDamageResponsibilityId,
                Description = entityDamageResponsability.Description,
                IsEnabled = Convert.ToBoolean(entityDamageResponsability.Enabled)
            };
        }
        internal static List<DamageResponsibility> CreateDamageResponsibilities(BusinessCollection businessCollection)
        {
            List<DamageResponsibility> damageResponsabilities = new List<DamageResponsibility>();

            foreach (CLMEN.ClaimDamageResponsibility entityDamageResponsabilities in businessCollection)
            {
                damageResponsabilities.Add(CreateDamageResponsability(entityDamageResponsabilities));
            }
            return damageResponsabilities;
        }


        internal static ClaimCoverage CreateClaimCoverageAmount(CLMEN.ClaimCoverageAmount claimCoverageAmount)
        {
            List<Estimation> estimations = new List<Estimation>();
            estimations.Add(new Estimation()
            {
                Type = new EstimationType()
                {
                    Id = claimCoverageAmount.EstimationTypeCode
                }
            });

            return new ClaimCoverage
            {
                Estimations = estimations
            };
        }

        internal static List<ClaimCoverage> CreateClaimCoverageAmounts(BusinessCollection businessCollection)
        {
            List<ClaimCoverage> claimCoverageAmounts = new List<ClaimCoverage>();

            foreach (CLMEN.ClaimCoverageAmount entityClaimCoverageAmount in businessCollection)
            {
                claimCoverageAmounts.Add(CreateClaimCoverageAmount(entityClaimCoverageAmount));
            }

            return claimCoverageAmounts;
        }

        internal static ClaimCoverageActivePanel CreateClaimCoverageActivePanel(PARAMEN.ClaimCoverageActivePanel claimCoverageActivePanel)
        {
            if (claimCoverageActivePanel == null)
            {
                return null;
            }
            return new ClaimCoverageActivePanel()
            {
                CoverageId = claimCoverageActivePanel.CoverageId,
                EnabledDriver = Convert.ToBoolean(claimCoverageActivePanel.EnabledDriver),
                EnabledThirdPartyVehicle = Convert.ToBoolean(claimCoverageActivePanel.EnabledThirdPartyVehicle),
                EnabledThird = Convert.ToBoolean(claimCoverageActivePanel.EnabledThird),
                EnabledAffectedProperty = Convert.ToBoolean(claimCoverageActivePanel.EnabledAffectedProperty)
            };
        }

        internal static Affected CreateAffectedByPerson(UPEN.Person entityPerson)
        {
            return new Affected
            {
                Id = entityPerson.IndividualId,
                DocumentTypeId = entityPerson.IdCardTypeCode,
                DocumentNumber = entityPerson.IdCardNo,
                FullName = entityPerson.Surname + " " + (string.IsNullOrEmpty(entityPerson.MotherLastName) ? "" : entityPerson.MotherLastName + " ") + entityPerson.Name
            };
        }

        internal static List<Affected> CreateAffectedsByPersons(BusinessCollection businessCollection)
        {
            List<Affected> affecteds = new List<Affected>();

            foreach (UPEN.Person entityPerson in businessCollection)
            {
                affecteds.Add(CreateAffectedByPerson(entityPerson));
            }

            return affecteds;
        }

        internal static Debtor CreateDebtorByPerson(UPEN.Person entityPerson)
        {
            return new Debtor
            {
                Id = entityPerson.IndividualId,
                DocumentNumber = entityPerson.IdCardNo,
                FullName = entityPerson.Surname + " " + (string.IsNullOrEmpty(entityPerson.MotherLastName) ? "" : entityPerson.MotherLastName + " ") + entityPerson.Name
            };
        }

        internal static List<Debtor> CreateDebtorByPersons(BusinessCollection businessCollection)
        {
            List<Debtor> debtor = new List<Debtor>();

            foreach (UPEN.Person entityPerson in businessCollection)
            {
                debtor.Add(CreateDebtorByPerson(entityPerson));
            }

            return debtor;
        }

        internal static Debtor CreateDebtorByCompany(UPEN.Company entityCompany)
        {
            return new Debtor
            {
                Id = entityCompany.IndividualId,
                DocumentNumber = entityCompany.TributaryIdNo,
                FullName = entityCompany.TradeName
            };
        }

        internal static List<Debtor> CreateDebtorByCompanies(BusinessCollection businessCollection)
        {
            List<Debtor> debtors = new List<Debtor>();

            foreach (UPEN.Company entityCompany in businessCollection)
            {
                debtors.Add(CreateDebtorByCompany(entityCompany));
            }

            return debtors;
        }

        internal static Recuperator CreateRecuperatorsByPerson(UPEN.Person entityPerson)
        {
            return new Recuperator
            {
                Id = entityPerson.IndividualId,
                DocumentNumber = entityPerson.IdCardNo,
                FullName = entityPerson.Surname + " " + (string.IsNullOrEmpty(entityPerson.MotherLastName) ? "" : entityPerson.MotherLastName + " ") + entityPerson.Name
            };
        }

        internal static List<Recuperator> CreateRecuperatorsByPersons(BusinessCollection businessCollection)
        {
            List<Recuperator> recuperators = new List<Recuperator>();

            foreach (UPEN.Person entityPerson in businessCollection)
            {
                recuperators.Add(CreateRecuperatorsByPerson(entityPerson));
            }

            return recuperators;
        }

        internal static Recuperator CreateRecuperatorsByCompany(UPEN.Company entityCompany)
        {
            return new Recuperator
            {
                Id = entityCompany.IndividualId,
                DocumentNumber = entityCompany.TributaryIdNo,
                FullName = entityCompany.TradeName
            };
        }

        internal static List<Recuperator> CreateRecuperatorsByCompanies(BusinessCollection businessCollection)
        {
            List<Recuperator> recuperators = new List<Recuperator>();

            foreach (UPEN.Company entityCompany in businessCollection)
            {
                recuperators.Add(CreateRecuperatorsByCompany(entityCompany));
            }

            return recuperators;
        }

        internal static ClaimCancellationReason CreateCancellationReason(PARAMEN.CancellationReason entityCancellationReason)
        {
            return new ClaimCancellationReason
            {
                Id = entityCancellationReason.CancellationReasonCode,
                Description = entityCancellationReason.Description
            };
        }

        internal static List<ClaimCancellationReason> CreateCancellationReasons(BusinessCollection businessCollection)
        {
            List<ClaimCancellationReason> cancellationReasons = new List<ClaimCancellationReason>();

            foreach (PARAMEN.CancellationReason entityCancellationReason in businessCollection)
            {
                cancellationReasons.Add(CreateCancellationReason(entityCancellationReason));
            }

            return cancellationReasons;
        }

        internal static ContactInformation CreateContactInformation(CLMEN.ClaimNoticeContactInformation entityClaimNoticeContactInformation)
        {
            return new ContactInformation()
            {
                ClaimNoticeId = entityClaimNoticeContactInformation.ClaimNoticeCode,
                Name = entityClaimNoticeContactInformation.Name,
                Phone = entityClaimNoticeContactInformation.Phone,
                Mail = entityClaimNoticeContactInformation.Mail
            };
        }

        internal static List<NoticeCoverage> NoticeCoverages(List<CLMEN.ClaimNoticeCoverage> entityClaimNoticeCoverages)
        {
            List<NoticeCoverage> noticeCoverages = new List<NoticeCoverage>();

            foreach (CLMEN.ClaimNoticeCoverage entityCoverage in entityClaimNoticeCoverages)
            {
                NoticeCoverage coverage = new NoticeCoverage
                {
                    RiskId = entityCoverage.RiskNum,
                    RiskNumber = entityCoverage.RiskNum,
                    CoverageId = entityCoverage.CoverageId,
                    CoverageNumber = entityCoverage.CoverNum,
                    IndividualId = entityCoverage.IndividualId,
                    EstimateTypeId = entityCoverage.EstimateTypeCode,
                    EstimateAmount = entityCoverage.EstimateAmount,
                    IsProspect = Convert.ToBoolean(entityCoverage.IsProspect),
                    IsInsured = Convert.ToBoolean(entityCoverage.IsInsured)
                };
                noticeCoverages.Add(coverage);
            }

            return noticeCoverages;
        }


        internal static Affected CreateAffectedByCompanies(UPEN.Company entityCompany)
        {
            return new Affected
            {
                Id = entityCompany.IndividualId,
                DocumentTypeId = entityCompany.TributaryIdTypeCode,
                DocumentNumber = entityCompany.TributaryIdNo,
                FullName = entityCompany.TradeName
            };
        }

        internal static List<Affected> CreateAffectedsByCompanies(BusinessCollection businessCollection)
        {
            List<Affected> affecteds = new List<Affected>();

            foreach (UPEN.Company entityCompany in businessCollection)
            {
                affecteds.Add(CreateAffectedByCompanies(entityCompany));
            }

            return affecteds;
        }

        internal static Driver CreateDriver(CLMEN.ClaimCoverageDriverInformation claimCoverageDriverInformation)
        {
            return new Driver
            {
                Name = claimCoverageDriverInformation.Name,
                LicenseValidThru = Convert.ToDateTime(claimCoverageDriverInformation.LicenseValidThru),
                LicenseType = claimCoverageDriverInformation.LicenseType,
                LicenseNumber = claimCoverageDriverInformation.LicenseNumber,
                DocumentNumber = claimCoverageDriverInformation.DocumentNumber,
                Id = claimCoverageDriverInformation.ClaimCoverageCode,
                Age = Convert.ToInt32(claimCoverageDriverInformation.Age)
            };
        }

        internal static ThirdPartyVehicle CreateThirdPartyVehicle(CLMEN.ClaimCoverageThirdPartyVehicle claimCoverageThirdPartyVehicle)
        {
            return new ThirdPartyVehicle
            {
                ChasisNumber = claimCoverageThirdPartyVehicle.ChasisNumber,
                ColorCode = Convert.ToInt32(claimCoverageThirdPartyVehicle.VehicleColorCode),
                Description = claimCoverageThirdPartyVehicle.Description,
                EngineNumber = claimCoverageThirdPartyVehicle.EngineNumber,
                Id = claimCoverageThirdPartyVehicle.ClaimCoverageCode,
                Make = claimCoverageThirdPartyVehicle.VehicleMake,
                Model = claimCoverageThirdPartyVehicle.VehicleModel,
                Plate = claimCoverageThirdPartyVehicle.Plate,
                VinCode = claimCoverageThirdPartyVehicle.VinCode,
                Year = Convert.ToInt32(claimCoverageThirdPartyVehicle.VehicleYear)
            };
        }

        internal static Inspection CreateInspection(CLMEN.ClaimSupplier claimSupplier)
        {
            return new Inspection
            {
                AdjusterId = Convert.ToInt32(claimSupplier.AdjusterCode),
                AnalizerId = Convert.ToInt32(claimSupplier.AnalizerCode),
                ResearcherId = Convert.ToInt32(claimSupplier.ResearcherCode),
                AffectedProperty = claimSupplier.AffectedProperty,
                RegistrationDate = Convert.ToDateTime(claimSupplier.DateInspection),
                RegistrationHour = claimSupplier.HourInspection,
                LossDescription = claimSupplier.LossDescription
            };
        }

        internal static CatastrophicEvent CreateClaimCatastrophicInformation(CLMEN.ClaimCatastrophicInformation claimCatastrophicInformation)
        {
            return new CatastrophicEvent
            {
                Description = claimCatastrophicInformation.Description,
                FullAddress = claimCatastrophicInformation.Address,
                CurrentFrom = Convert.ToDateTime(claimCatastrophicInformation.Datetimefrom),
                CurrentTo = Convert.ToDateTime(claimCatastrophicInformation.Datetimeto),
                Catastrophe = new Catastrophe
                {
                    Id = Convert.ToInt32(claimCatastrophicInformation.CatastropheCode)
                },
                City = new City
                {
                    Id = Convert.ToInt32(claimCatastrophicInformation.CityCode),
                    State = new State
                    {
                        Id = Convert.ToInt32(claimCatastrophicInformation.StateCode),
                        Country = new Country
                        {
                            Id = Convert.ToInt32(claimCatastrophicInformation.CountryCode)
                        }
                    }
                }
            };
        }

        internal static ClaimCoverage CreateClaimCoveraByModel(ClaimCoverage claimCoverage, EstimationType estimationType, List<COMMEN.Currency> entityCurrencies)
        {
            ClaimCoverage coverage = new ClaimCoverage()
            {
                Id = claimCoverage.Id,
                SubClaim = claimCoverage.SubClaim,
                RiskId = claimCoverage.RiskId,
                RiskNumber = claimCoverage.RiskNumber,
                CoverageId = claimCoverage.CoverageId,
                CoverageNumber = claimCoverage.CoverageNumber,
                IsInsured = Convert.ToBoolean(claimCoverage.IsInsured),
                Description = claimCoverage.Description,
                IsProspect = Convert.ToBoolean(claimCoverage.IsProspect),
                IndividualId = claimCoverage.IndividualId,
                Estimations = new List<Estimation>()
            };

            coverage.Estimations.Add(new Estimation()
            {
                Type = new EstimationType
                {
                    Description = estimationType.Description,
                    Id = estimationType.Id
                },
                Reason = new Reason
                {
                    Status = new Status
                    {
                        InternalStatus = new InternalStatus
                        {
                        }
                    }
                },
                Currency = new Currency
                {
                    Description = entityCurrencies.FirstOrDefault().Description
                }
            });

            return coverage;
        }

        internal static Estimation CreatePaymentEstimation(PARAMEN.EstimationType entityEstimationType)
        {
            return new Estimation
            {
                Type = new EstimationType
                {
                    Id = entityEstimationType.EstimateTypeCode,
                    Description = entityEstimationType.Description
                }
            };
        }

        internal static List<Estimation> CreatePaymentEstimation(BusinessCollection businessCollection)
        {
            List<Estimation> estimations = new List<Estimation>();

            foreach (PARAMEN.EstimationType entityEstimationType in businessCollection)
            {
                estimations.Add(CreatePaymentEstimation(entityEstimationType));
            }

            return estimations;
        }

        internal static NoticeVehicle CreateNoticeRiskVehicle(CLMEN.ClaimNoticeVehicle claimNoticeVehicle, Notice notice)
        {
            return new NoticeVehicle
            {
                Notice = new Notice
                {
                    Id = claimNoticeVehicle.ClaimNoticeCode,
                    Number = notice.Number,
                    Description = notice.Description
                }
            };
        }

        internal static NoticeLocation CreateNoticeRiskLocation(CLMEN.ClaimNoticeRiskLocation claimNoticeLocation, Notice notice)
        {
            return new NoticeLocation
            {
                Notice = new Notice
                {
                    Id = claimNoticeLocation.ClaimNoticeCode,
                    Number = notice.Number,
                    Description = notice.Description
                }
            };
        }

        internal static NoticeSurety CreateNoticeSurety(CLMEN.ClaimNoticeRiskSurety ClaimNoticeSurety, Notice notice)
        {
            return new NoticeSurety
            {
                Notice = new Notice
                {
                    Id = ClaimNoticeSurety.ClaimNoticeCode,
                    Number = notice.Number,
                    Description = notice.Description
                }
            };
        }

        internal static NoticeTransport CreateNoticeRiskTransport(CLMEN.ClaimNoticeRiskTransport entityClaimNoticeRiskTransport, Notice notice)
        {
            return new NoticeTransport
            {
                Notice = new Notice
                {
                    Id = entityClaimNoticeRiskTransport.ClaimNoticeCode,
                    Number = notice.Number,
                    Description = notice.Description
                }
            };
        }

        internal static PendingOperation CreatePendingOperationByNoticeVehicle(NoticeVehicle noticeVehicle)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = noticeVehicle.Notice.UserId,
                Operation = JsonConvert.SerializeObject(noticeVehicle)
            };
        }

        internal static NoticeAirCraft CreateNoticeRiskAirCraft(CLMEN.ClaimNoticeRiskAircraft entityClaimNoticeRiskAircraft, Notice notice)
        {
            return new NoticeAirCraft
            {
                Notice = new Notice
                {
                    Id = entityClaimNoticeRiskAircraft.ClaimNoticeCode,
                    Number = notice.Number,
                    Description = notice.Description
                }
            };
        }

        internal static NoticeFidelity CreateNoticeRiskFidelity(CLMEN.ClaimNoticeRiskFidelity entityClaimNoticeRiskFidelity, Notice notice)
        {
            return new NoticeFidelity
            {
                Notice = new Notice
                {
                    Id = entityClaimNoticeRiskFidelity.ClaimNoticeCode,
                    Number = notice.Number,
                    Description = notice.Description
                }
            };
        }

        public static VHMO.VehicleDTO CreateClaimNoticeRiskVehicle(CLMEN.ClaimNoticeVehicle noticeVehicle)
        {
            return new VHMO.VehicleDTO
            {
                Plate = noticeVehicle.Plate,
                MakeId = Convert.ToInt32(noticeVehicle.VehicleMakeCode),
                ModelId = Convert.ToInt32(noticeVehicle.VehicleModelCode),
                VersionId = Convert.ToInt32(noticeVehicle.VehicleVersionCode),
                Year = Convert.ToInt32(noticeVehicle.VehicleVersionYearCode),
                ColorId = Convert.ToInt32(noticeVehicle.VehicleColor),
            };
        }

        internal static ClaimDocumentation CreateDocumentation(PARAMEN.Documentation documentation)
        {
            return new ClaimDocumentation
            {
                Id = documentation.DocumentationCode,
                ModuleId = Convert.ToInt32(documentation.ModuleCode),
                SubmoduleId = Convert.ToInt32(documentation.SubmoduleCode),
                Description = documentation.Description,
                prefix = new Prefix
                {
                    Id = Convert.ToInt32(documentation.PrefixCode)
                },
                IsRequired = documentation.IsRequired,
                Enable = documentation.Enabled

            };
        }

        internal static List<ClaimDocumentation> CreateDocumentations(BusinessCollection businessCollection)
        {
            List<ClaimDocumentation> claimDocumentations = new List<ClaimDocumentation>();

            foreach (PARAMEN.Documentation documentation in businessCollection)
            {
                claimDocumentations.Add(CreateDocumentation(documentation));
            }

            return claimDocumentations;
        }

        internal static PendingOperation CreatePendingOperationByNoticeLocation(NoticeLocation noticeLocation)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = noticeLocation.Notice.UserId,
                Operation = JsonConvert.SerializeObject(noticeLocation)
            };
        }

        internal static CoveragePaymentConcept CreatePaymentConcept(CLMEN.CoveragePaymentConcept coveragePaymentConcept)
        {
            return new CoveragePaymentConcept
            {
                CoverageId = coveragePaymentConcept.CoverageId,
                EstimationTypeId = coveragePaymentConcept.EstimateTypeCode,
                ConceptId = coveragePaymentConcept.PaymentConceptCode

            };
        }

        internal static List<Participant> CreateParticipants(BusinessCollection businessCollection)
        {
            List<Participant> participants = new List<Participant>();

            foreach (CLMEN.Participant entityParticipant in businessCollection)
            {
                participants.Add(CreateParticipant(entityParticipant));
            }

            return participants;
        }

        internal static Participant CreateParticipant(CLMEN.Participant participant)
        {
            return new Participant
            {
                Id = participant.ParticipantCode,
                DocumentNumber = participant.DocumentNumber,
                DocumentTypeId = participant.DocumentTypeCode,
                Fullname = participant.Fullname,
                Phone = participant.Phone,
                Address = participant.Address
            };
        }

        internal static ThirdAffected CreateThirdAffected(CLMEN.ClaimCoverageThirdAffected claimCoverageThirdAffected)
        {
            return new ThirdAffected
            {
                ClaimCoverageId = claimCoverageThirdAffected.ClaimCoverageCode,
                DocumentNumber = claimCoverageThirdAffected.DocumentNumber,
                FullName = claimCoverageThirdAffected.Fullname
            };
        }

        internal static ThirdAffected CreateThirdAffectedByInsured(UNDMO.InsuredDTO insuredDTO)
        {
            if (insuredDTO == null)
                return null;

            return new ThirdAffected
            {
                DocumentTypeId = insuredDTO.DocumentTypeId,
                DocumentNumber = insuredDTO.DocumentNumber,
                FullName = insuredDTO.FullName
            };
        }

        internal static List<ThirdAffected> CreateThirdAffecteds(BusinessCollection businessCollection)
        {
            List<ThirdAffected> thirdAffecteds = new List<ThirdAffected>();

            foreach (CLMEN.ClaimCoverageThirdAffected affected in businessCollection)
            {
                thirdAffecteds.Add(CreateThirdAffected(affected));
            }

            return thirdAffecteds;
        }

        internal static Debtor CreateDebtorByParticipant(CLMEN.Participant participant)
        {
            if (participant == null)
            {
                return null;
            }

            return new Debtor
            {
                Id = participant.ParticipantCode,
                DocumentNumber = participant.DocumentNumber,
                FullName = participant.Fullname,
                Address = participant.Address,
                Phone = participant.Phone
            };
        }

        internal static PendingOperation CreatePendingOperationByNoticeSurety(NoticeSurety noticeSurety)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = noticeSurety.Notice.UserId,
                Operation = JsonConvert.SerializeObject(noticeSurety)
            };
        }

        internal static List<Debtor> CreateDebtorsByParticipants(BusinessCollection businessCollection)
        {
            List<Debtor> debtors = new List<Debtor>();

            foreach (CLMEN.Participant entityParticipant in businessCollection)
            {
                debtors.Add(CreateDebtorByParticipant(entityParticipant));
            }

            return debtors;
        }

        internal static List<TextOperation> CreateTextOperations(BusinessCollection businessCollection)
        {
            List<TextOperation> textOperations = new List<TextOperation>();

            foreach (CLMEN.TextOperation entityTextOperation in businessCollection)
            {
                textOperations.Add(CreateTextOperation(entityTextOperation));
            }

            return textOperations;
        }

        internal static TextOperation CreateTextOperation(CLMEN.TextOperation entityTextOperation)
        {
            if (entityTextOperation == null)
            {
                return null;
            }

            return new TextOperation
            {
                Id = entityTextOperation.Id,
                Operation = entityTextOperation.TextDescription
            };
        }

        internal static PendingOperation CreatePendingOperation(CLMEN.PendingOperations entityPendingOperation)
        {
            if (entityPendingOperation == null)
            {
                return null;
            }

            return new PendingOperation
            {
                Id = entityPendingOperation.Id,
                CreationDate = entityPendingOperation.CreationDate,
                UserId = Convert.ToInt32(entityPendingOperation.User),
                Operation = entityPendingOperation.Operation
            };
        }

        internal static Participant CreateParticipant(ClaimCoverage claimCoverage)
        {
            return new Participant
            {
                DocumentNumber = claimCoverage.ThirdAffected.DocumentNumber,
                Fullname = claimCoverage.ThirdAffected.FullName
            };
        }

        internal static ClaimControl CreateClaimControl(INTEN.ClmClaimControl entityCoTmpClmClaimControl)
        {
            return new ClaimControl
            {
                Id = entityCoTmpClmClaimControl.ClaimControlId,
                ClaimId = entityCoTmpClmClaimControl.ClaimCode,
                ClaimModifyId = entityCoTmpClmClaimControl.ClaimModifyCode,
                Action = entityCoTmpClmClaimControl.Action
            };
        }

        internal static ClaimControl CreateClaimControl(Claim claim)
        {
            return new ClaimControl
            {
                ClaimId = claim.Id,
                ClaimModifyId = claim.Modifications.Last().Id
            };
        }

        internal static ClaimControl CreateClaimControl(ClaimReserve claimReserve)
        {
            return new ClaimControl
            {
                ClaimId = claimReserve.Claim.Id,
                ClaimModifyId = claimReserve.Claim.Modifications.Last().Id
            };
        }

        #endregion

        #region DtoToModel

        internal static CauseCoverage CreateCauseCoverage(int causeId, CoverageDTO causeCoverageDTO)
        {
            return new CauseCoverage
            {
                Id = causeCoverageDTO.Id,
                Cause = new Cause
                {
                    Id = causeId
                }
            };
        }

        internal static Status CreateEstimationTypeStatus(StatusDTO statusDTO)
        {
            return new Status
            {
                Id = statusDTO.Id,
                Description = statusDTO.Description,
                IsEnabled = statusDTO.Enabled,
                InternalStatus = CreateInternalStatus(statusDTO.InternalStatus)
            };
        }

        internal static InternalStatus CreateInternalStatus(InternalStatusDTO internalStatusDTO)
        {
            return new InternalStatus
            {
                Id = internalStatusDTO.Id,
                Description = internalStatusDTO.Description
            };
        }

        internal static Reason CreateReason(ReasonDTO reasonDTO)
        {
            return new Reason
            {
                Id = reasonDTO.Id,
                Description = reasonDTO.Description,
                Status = new Status
                {
                    Id = reasonDTO.EstimationTypeStatusId
                },
                Prefix = new Prefix
                {
                    Id = reasonDTO.PrefixId
                },

                Enabled = reasonDTO.Enabled
            };
        }

        internal static EstimationType CreateEstimationType(int estimationTypeCode)
        {
            return new EstimationType
            {
                Id = estimationTypeCode
            };
        }

        internal static Prefix CreatePrefix(PrefixDTO Prefix)
        {
            return new Prefix
            {
                Id = Prefix.Id,
                Description = Prefix.Description
            };
        }

        internal static List<Prefix> CreatePrefixes(List<PrefixDTO> PrefixesDTO)
        {
            List<Prefix> prefixes = new List<Prefix>();

            foreach (PrefixDTO prefixDTO in PrefixesDTO)
            {

                prefixes.Add(CreatePrefix(prefixDTO));

            }

            return prefixes;
        }

        internal static Status CreateStatus(StatusDTO statusDTO)
        {
            return new Status
            {
                Id = statusDTO.Id,
                Description = statusDTO.Description,
                IsEnabled = statusDTO.Enabled,
                InternalStatus = statusDTO.InternalStatus == null ? new InternalStatus() : CreateInternalStatus(statusDTO.InternalStatus)
            };
        }

        internal static PendingOperation CreatePendingOperationByNoticeTransport(NoticeTransport transportNotice)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = transportNotice.Notice.UserId,
                Operation = JsonConvert.SerializeObject(transportNotice)
            };

        }

        internal static List<Status> CreateStatuses(List<StatusDTO> statusesDTO)
        {
            List<Status> statuses = new List<Status>();

            foreach (StatusDTO statusDTO in statusesDTO)
            {

                statuses.Add(CreateStatus(statusDTO));

            }

            return statuses.OrderBy(s => s.Description).ToList();
        }

        internal static Claim CreateClaim(ClaimDTO claimDTO)
        {
            Claim claim = new Claim
            {
                Id = claimDTO.ClaimId,
                OccurrenceDate = claimDTO.OccurrenceDate,
                IndividualId = claimDTO.IndividualId,
                Branch = new Branch
                {
                    Id = claimDTO.BranchId
                },
                Endorsement = new ClaimEndorsement
                {
                    Id = claimDTO.EndorsementId,
                    Number = claimDTO.EndorsementNumber,
                    PolicyId = claimDTO.PolicyId,
                    PolicyNumber = claimDTO.PolicyDocumentNumber,
                    Policy = new Policy
                    {
                        Id = claimDTO.PolicyId,
                        BusinessTypeId = claimDTO.PolicyBusinessTypeId.GetValueOrDefault(),
                        DocumentNumber = claimDTO.PolicyDocumentNumber,
                        TypeId = claimDTO.PolicyTypeId,
                        ProductId = claimDTO.PolicyProductId
                    }
                },
                Number = claimDTO.Number,
                Prefix = new Prefix
                {
                    Id = claimDTO.PrefixId
                },
                NoticeId = claimDTO.NoticeId.GetValueOrDefault(),
                NoticeDate = claimDTO.NoticeDate.GetValueOrDefault(),
                Modifications = new List<ClaimModify>(),
                CoveredRiskType = (CoveredRiskType)claimDTO.CoveredRiskType,
                City = new City
                {
                    Id = claimDTO.CityId,
                    State = new State
                    {
                        Id = claimDTO.StateId,
                        Country = new Country
                        {
                            Id = claimDTO.CountryId
                        }
                    }
                },
                Location = claimDTO.Location,
                Cause = new Cause
                {
                    Id = claimDTO.CauseId
                },
                DamageType = new DamageType
                {
                    Id = claimDTO.DamageTypeId
                },
                DamageResponsability = new DamageResponsibility
                {
                    Id = claimDTO.DamageResponsabilityId
                },
                TextOperation = new TextOperation
                {
                    Operation = claimDTO.Description,
                },
                IsTotalParticipation = claimDTO.IsTotalParticipation,
                BusinessTypeId = claimDTO.BusinessTypeId,
                JudicialDecisionDate = claimDTO.JudicialDecisionDate,
                TemporalId = claimDTO.TemporalId
            };

            if (claimDTO.Catastrophe != null)
            {
                claim.CatastrophicEvent = new CatastrophicEvent
                {
                    Catastrophe = new Catastrophe
                    {
                        Id = claimDTO.Catastrophe.Id,
                        Description = claimDTO.Catastrophe.Description
                    },
                    Description = claimDTO.CatastropheDescription,
                    CurrentFrom = Convert.ToDateTime(claimDTO.CatastropheDateTimeFrom),
                    CurrentTo = Convert.ToDateTime(claimDTO.CatastropheDateTimeTo),
                    FullAddress = claimDTO.CatastropheAddress,
                    City = new City
                    {
                        Id = claimDTO.CatastropheCityId,
                        State = new State
                        {
                            Id = claimDTO.CatastropheStateId,
                            Country = new Country
                            {
                                Id = claimDTO.CatastropheCountryId
                            }
                        }
                    }
                };
            }

            if (claimDTO.AnalizerId > 0)
            {
                claim.Inspection = new Inspection
                {
                    AdjusterId = claimDTO.AdjusterId,
                    AnalizerId = claimDTO.AnalizerId,
                    ResearcherId = claimDTO.ResearcherId,
                    RegistrationDate = claimDTO.DateInspection,
                    RegistrationHour = claimDTO.HourInspection,
                    LossDescription = claimDTO.LossDescription
                };
            }

            foreach (ClaimModifyDTO claimModificationDTO in claimDTO.Modifications)
            {

                ClaimModify claimModify = new ClaimModify
                {
                    Id = claimModificationDTO.ClaimModifyId,
                    RegistrationDate = claimModificationDTO.RegistrationDate,
                    AccountingDate = claimModificationDTO.AccountingDate,
                    UserId = claimModificationDTO.UserId,
                    UserProfileId = claimModificationDTO.UserProfileId,
                    Coverages = new List<ClaimCoverage>()
                };

                foreach (ClaimCoverageDTO claimCoverageDTO in claimModificationDTO.ClaimCoverages)
                {
                    ClaimCoverage claimCoverage = new ClaimCoverage()
                    {
                        Id = claimCoverageDTO.Id,
                        CoverageNumber = claimCoverageDTO.CoverageNum,
                        CoverageInsuredAmount = claimCoverageDTO.CoverageInsuredAmount,
                        CoverageId = claimCoverageDTO.CoverageId,
                        EndorsementId = claimCoverageDTO.EndorsementId,
                        RiskId = claimCoverageDTO.RiskId,
                        IsProspect = claimCoverageDTO.IsProspect,
                        IsInsured = claimCoverageDTO.IsInsured,
                        SubClaim = claimCoverageDTO.SubClaim,
                        RiskNumber = claimCoverageDTO.RiskNum,
                        TextOperation = new TextOperation
                        {
                            Operation = claimCoverageDTO.AffectedProperty
                        },
                        IndividualId = claimCoverageDTO.IndividualId,
                        ClaimedAmount = claimCoverageDTO.ClaimedAmount,
                        IsClaimedAmount = claimCoverageDTO.IsClaimedAmount,
                        Estimations = new List<Estimation>()
                    };


                    if (claimCoverageDTO.DriverInformationDTO != null)
                    {
                        claimCoverage.Driver = new Driver
                        {
                            LicenseType = claimCoverageDTO.DriverInformationDTO.LicenseType,
                            LicenseNumber = claimCoverageDTO.DriverInformationDTO.LicenseNumber,
                            LicenseValidThru = Convert.ToDateTime(claimCoverageDTO.DriverInformationDTO.LicenseValidThru),
                            Age = Convert.ToInt32(claimCoverageDTO.DriverInformationDTO.Age),
                            DocumentNumber = claimCoverageDTO.DriverInformationDTO.DocumentNumber,
                            Name = claimCoverageDTO.DriverInformationDTO.Name
                        };
                    }

                    if (claimCoverageDTO.ThirdAffectedDTO != null)
                    {
                        claimCoverage.ThirdAffected = new ThirdAffected
                        {
                            DocumentNumber = claimCoverageDTO.ThirdAffectedDTO.DocumentNumber,
                            FullName = claimCoverageDTO.ThirdAffectedDTO.FullName
                        };
                    }

                    if (claimCoverageDTO.ThirdPartyVehicleDTO != null)
                    {
                        claimCoverage.ThirdPartyVehicle = new ThirdPartyVehicle
                        {
                            Plate = claimCoverageDTO.ThirdPartyVehicleDTO.Plate,
                            Model = claimCoverageDTO.ThirdPartyVehicleDTO.VehicleModel,
                            Make = claimCoverageDTO.ThirdPartyVehicleDTO.VehicleMake,
                            Year = claimCoverageDTO.ThirdPartyVehicleDTO.VehicleYear,
                            ColorCode = claimCoverageDTO.ThirdPartyVehicleDTO.VehicleColorId,
                            ChasisNumber = claimCoverageDTO.ThirdPartyVehicleDTO.ChasisNumber,
                            EngineNumber = claimCoverageDTO.ThirdPartyVehicleDTO.EngineNumber,
                            VinCode = claimCoverageDTO.ThirdPartyVehicleDTO.VinCode,
                            Description = claimCoverageDTO.ThirdPartyVehicleDTO.Description

                        };
                    }

                    foreach (EstimationDTO itemEstimationDTO in claimCoverageDTO.Estimations)
                    {
                        claimCoverage.Estimations.Add(new Estimation()
                        {
                            Type = new EstimationType
                            {
                                Id = itemEstimationDTO.Id
                            },
                            Reason = new Reason
                            {
                                Id = itemEstimationDTO.ReasonId,
                                Status = new Status
                                {
                                    Id = itemEstimationDTO.StatusCodeId,
                                    InternalStatus = new InternalStatus
                                    {
                                        Id = itemEstimationDTO.InternalStatusId
                                    }
                                }
                            },
                            Amount = itemEstimationDTO.EstimateAmount,
                            DeductibleAmount = itemEstimationDTO.DeductibleAmount,
                            Version = 1,//itemEstimationDTO.Version,
                            Currency = new Currency
                            {
                                Id = itemEstimationDTO.CurrencyId
                            },
                            CreationDate = Convert.ToDateTime(itemEstimationDTO.CreationDate),
                            IsMinimumSalary = itemEstimationDTO.IsMinimumSalary,
                            MinimumSalariesNumber = itemEstimationDTO.MinimumSalariesNumber,
                            MinimumSalaryValue = itemEstimationDTO.MinimumSalaryValue,
                            AmountAccumulate = itemEstimationDTO.EstimateAmountAccumulate,
                            ExchangeRate = itemEstimationDTO.ExchangeRate,
                            CoverageInsuredAmountEquivalent = itemEstimationDTO.CoverageInsuredAmountEquivalent
                        });
                    }
                    claimModify.Coverages.Add(claimCoverage);
                }

                claim.Modifications.Add(claimModify);
            }

            return claim;
        }

        internal static PendingOperation CreatePendingOperationByNoticeFidelity(NoticeFidelity noticeFidelity)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = noticeFidelity.Notice.UserId,
                Operation = JsonConvert.SerializeObject(noticeFidelity)
            };
        }

        internal static PendingOperation CreatePendingOperationByNoticeAircraft(NoticeAirCraft noticeAirCraft)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = noticeAirCraft.Notice.UserId,
                Operation = JsonConvert.SerializeObject(noticeAirCraft)
            };
        }

        internal static ClaimVehicle CreateClaimVehicle(ClaimVehicleDTO claimVehicleDTO)
        {
            ClaimVehicle claimVehicle = new ClaimVehicle
            {
            };

            return claimVehicle;
        }

        internal static ClaimSurety CreateClaimSurety(ClaimSuretyDTO claimSuretyDTO)
        {
            ClaimSurety ClaimSurety = new ClaimSurety
            {
            };

            return ClaimSurety;
        }

        internal static ClaimLocation CreateClaimLocation(ClaimLocationDTO claimLocationDTO)
        {
            ClaimLocation ClaimLocation = new ClaimLocation
            {
            };

            return ClaimLocation;

        }

        internal static Branch CreateBranch(BranchDTO branch)
        {
            return new Branch
            {
                Id = branch.Id,
                Description = branch.Description
            };
        }

        internal static SearchClaim CreateSearchClaim(SearchClaimDTO searchClaimDTO)
        {
            return new SearchClaim
            {
                BranchId = searchClaimDTO.BranchId,
                ClaimDateFrom = searchClaimDTO.ClaimDateFrom,
                ClaimDateTo = searchClaimDTO.ClaimDateTo,
                ClaimNumber = searchClaimDTO.ClaimNumber,
                NoticeDateFrom = searchClaimDTO.NoticeDateFrom,
                NoticeDateTo = searchClaimDTO.NoticeDateTo,
                PrefixId = searchClaimDTO.PrefixId,
                TemporaryNumber = searchClaimDTO.TemporaryNumber,
                UserId = searchClaimDTO.UserId,
                IndividualId = searchClaimDTO.IndividualId,
                DocumentNumber = searchClaimDTO.DocumentNumber,
                HolderId = searchClaimDTO.HolderId,
                IsMinimumSalary = searchClaimDTO.IsMinimumSalary,
                CurrentMinimumSalaryValue = searchClaimDTO.CurrentMinimumSalaryValue
            };
        }

        internal static ClaimCoverageActivePanel CreateClaimCoverageActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanelDTO)
        {
            return new ClaimCoverageActivePanel
            {
                CoverageId = claimCoverageActivePanelDTO.CoverageId,
                EnabledDriver = claimCoverageActivePanelDTO.IsEnabledDriver,
                EnabledThirdPartyVehicle = claimCoverageActivePanelDTO.IsEnabledThirdPartyVehicle,
                EnabledThird = claimCoverageActivePanelDTO.IsEnabledThird,
                EnabledAffectedProperty = claimCoverageActivePanelDTO.IsEnabledAffectedProperty

            };
        }

        internal static List<ClaimCoverageActivePanel> CreateClaimCoverageActivePanels(List<ClaimCoverageActivePanelDTO> claimCoverageActivePanelDTO)
        {
            List<ClaimCoverageActivePanel> listClaimCoverangeActivePanel = new List<ClaimCoverageActivePanel>();
            foreach (ClaimCoverageActivePanelDTO claimCoverangeActivePanel in claimCoverageActivePanelDTO)
            {
                listClaimCoverangeActivePanel.Add(CreateClaimCoverageActivePanel(claimCoverangeActivePanel));
            }
            return listClaimCoverangeActivePanel;
        }

        internal static SearchClaimNotice CreateSearchClaimNotice(SearchNoticeDTO searchNoticeDTO)
        {
            return new SearchClaimNotice
            {
                NoticeNumber = searchNoticeDTO.NoticeNumber,
                PrefixId = searchNoticeDTO.PrefixId,
                DateOcurrenceFrom = searchNoticeDTO.DateOcurrenceFrom,
                DateOcurrenceTo = searchNoticeDTO.DateOcurrenceTo,
                DateNoticeFrom = searchNoticeDTO.DateNoticeFrom,
                DateNoticeTo = searchNoticeDTO.DateNoticeTo,
                UserId = searchNoticeDTO.UserId,
                VehicleMakeId = searchNoticeDTO.VehicleMakeId,
                VehicleModelId = searchNoticeDTO.VehicleModelId,
                VehicleVersionId = searchNoticeDTO.VehicleVersionId,
                VehicleYear = searchNoticeDTO.VehicleYear,
                LicensePlate = searchNoticeDTO.LicensePlate,
                Address = searchNoticeDTO.Address,
                CountryId = searchNoticeDTO.CountryId,
                StateId = searchNoticeDTO.StateId,
                CityId = searchNoticeDTO.CityId,
                SuretyId = searchNoticeDTO.SuretyIndividualId,
                BidNumber = searchNoticeDTO.BidNumber,
                CourtNumber = searchNoticeDTO.CourtNumber,
                RiskId = searchNoticeDTO.RiskId,
                IndividualId = searchNoticeDTO.IndividualId,
                DocumentNumber = searchNoticeDTO.DocumentNumber,
                HolderId = searchNoticeDTO.HolderId,
                BranchId = searchNoticeDTO.BranchId
            };
        }

        internal static Notice CreateNotice(NoticeDTO noticeDTO)
        {
            return new Notice
            {
                Id = noticeDTO.Id,
                CreationDate = noticeDTO.NoticeDate,
                ClaimDate = noticeDTO.OcurrenceDate,
                ObjectedReason = noticeDTO.ObjectedDescription,
                Policy = new Policy
                {
                    Id = Convert.ToInt32(noticeDTO.PolicyId),
                    PrefixId = noticeDTO.PrefixId,
                    BranchId = noticeDTO.BranchId,
                    TypeId = Convert.ToInt32(noticeDTO.PolicyTypeId),
                    BusinessTypeId = Convert.ToInt32(noticeDTO.PolicyBusinessTypeId),
                    ProductId = Convert.ToInt32(noticeDTO.PolicyProductId)
                },
                Risk = new Risk
                {

                },
                City = new City
                {
                    Id = noticeDTO.CityId,
                    State = new State
                    {
                        Id = noticeDTO.StateId,
                        Country = new Country
                        {
                            Id = noticeDTO.CountryId
                        }
                    }
                },
                Type = new NoticeType
                {
                    Id = noticeDTO.NoticeTypeId
                },
                NoticeReason = new NoticeReason
                {
                    Id = Convert.ToInt32(noticeDTO.NoticeReasonId),
                    Description = noticeDTO.NoticeReasonDescription
                },

                NoticeState = new NoticeState
                {
                    Id = Convert.ToInt32(noticeDTO.NoticeStateId),
                    Description = noticeDTO.NoticeStateDescription
                },
                NumberObjected = noticeDTO.NumberObjected,
                OthersAffected = noticeDTO.OthersAffected,
                ClaimedAmount = noticeDTO.ClaimedAmount,
                ClaimReasonOthers = noticeDTO.ClaimReasonOthers,
                Number = noticeDTO.Number,
                IndividualId = noticeDTO.IndividualId,
                UserId = noticeDTO.UserId,
                UserProfileId = noticeDTO.UserProfileId
            };
        }

        internal static ClaimReserve CreateClaimReserve(ClaimReserveDTO claimReserveDTO)
        {
            ClaimReserve claimReserve = new ClaimReserve();

            claimReserve.Claim = CreateClaim(claimReserveDTO);

            return claimReserve;
        }

        internal static ClaimTransport CreateClaimTransport(ClaimTransportDTO claimTransportDTO)
        {
            ClaimTransport claimTransport = new ClaimTransport
            {
            };

            return claimTransport;
        }

        internal static ClaimAirCraft CreateClaimAirCraft(ClaimAirCraftDTO claimAirCraftDTO)
        {
            ClaimAirCraft claimAirCraft = new ClaimAirCraft
            {
            };

            return claimAirCraft;
        }

        internal static ClaimFidelity CreateClaimFidelity(ClaimFidelityDTO claimFidelityDTO)
        {
            ClaimFidelity claimFidelity = new ClaimFidelity
            {
            };

            return claimFidelity;
        }

        internal static List<NoticeCoverage> CreateNoticeCoverage(List<NoticeCoverageDTO> coverages)
        {
            List<NoticeCoverage> noticeCoverages = new List<NoticeCoverage>();

            if (coverages != null)
            {
                foreach (NoticeCoverageDTO coverage in coverages.ToList())
                {
                    noticeCoverages.Add(new NoticeCoverage
                    {
                        //RiskId = coverage.RiskNum,
                        RiskNumber = coverage.RiskNum,
                        CoverageId = coverage.CoverageId,
                        CoverageNumber = coverage.CoverNum,
                        IndividualId = coverage.IndividualId,
                        IsInsured = coverage.IsInsured,
                        EstimateTypeId = coverage.EstimateTypeId,
                        EstimateAmount = coverage.EstimateAmount,
                        FullName = coverage.FullName,
                        DocumentNumber = coverage.DocumentNumber,
                        DocumentTypeId = coverage.DocumentTypeId,
                        CurrencyId = coverage.CurrencyId
                    });
                }
            }
            return noticeCoverages;
        }

        internal static NoticeVehicle CreateNoticeVehicle(NoticeVehicleDTO noticeVehicleDTO, ContactInformationDTO contactInformationDTO, VehicleDTO vehicleDTO)
        {
            NoticeVehicle noticeVehicle = new NoticeVehicle
            {
                Plate = vehicleDTO.Plate,
                MakeId = Convert.ToInt32(vehicleDTO.MakeId),
                ModelId = Convert.ToInt32(vehicleDTO.ModelId),
                VersionId = Convert.ToInt32(vehicleDTO.VersionId),
                Year = vehicleDTO.Year,
                ColorId = Convert.ToInt32(vehicleDTO.ColorId),
                DriverName = vehicleDTO.DriverName,
                Notice = new Notice
                {
                    CreationDate = noticeVehicleDTO.NoticeDate,
                    ClaimDate = noticeVehicleDTO.OcurrenceDate,
                    Address = noticeVehicleDTO.Location,
                    City = new City
                    {
                        Id = noticeVehicleDTO.CityId,
                        State = new State
                        {
                            Id = noticeVehicleDTO.StateId,
                            Country = new Country
                            {
                                Id = noticeVehicleDTO.CountryId
                            }
                        }
                    },
                    Description = noticeVehicleDTO.Description,
                    Endorsement = new ClaimEndorsement
                    {
                        Id = Convert.ToInt32(noticeVehicleDTO.EndorsementId),
                        PolicyId = Convert.ToInt32(noticeVehicleDTO.PolicyId),
                        Number = Convert.ToInt32(noticeVehicleDTO.EndorsementNumber)
                    },
                    ObjectedReason = noticeVehicleDTO.ObjectedDescription,
                    UserId = noticeVehicleDTO.UserId,
                    UserProfileId = noticeVehicleDTO.UserProfileId,
                    CoveredRiskTypeId = Convert.ToInt32(noticeVehicleDTO.CoveredRiskTypeId),
                    Type = new NoticeType
                    {
                        Id = noticeVehicleDTO.NoticeTypeId
                    },
                    Id = noticeVehicleDTO.Id,
                    Risk = new Risk
                    {
                        RiskId = Convert.ToInt32(noticeVehicleDTO.RiskId)
                    },
                    ContactInformation = new ContactInformation
                    {
                        Name = contactInformationDTO.Name,
                        Phone = contactInformationDTO.Phone,
                        Mail = contactInformationDTO.Mail
                    },
                    DamageType = new DamageType
                    {
                        Id = Convert.ToInt32(noticeVehicleDTO.DamageTypeId)
                    },
                    DamageResponsability = new DamageResponsibility
                    {
                        Id = Convert.ToInt32(noticeVehicleDTO.DamageResponsibilityId)
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = Convert.ToInt32(noticeVehicleDTO.NoticeReasonId)
                    },

                    NoticeState = new NoticeState
                    {
                        Id = Convert.ToInt32(noticeVehicleDTO.NoticeStateId)
                    },
                    NumberObjected = noticeVehicleDTO.NumberObjected,
                    OthersAffected = noticeVehicleDTO.OthersAffected,
                    InternalConsecutive = noticeVehicleDTO.InternalConsecutive,
                    ClaimedAmount = noticeVehicleDTO.ClaimedAmount,
                    ClaimReasonOthers = noticeVehicleDTO.ClaimReasonOthers,
                    Number = noticeVehicleDTO.Number,
                    IndividualId = noticeVehicleDTO.IndividualId,
                    NoticeCoverages = CreateNoticeCoverage(noticeVehicleDTO.Coverages),
                    Policy = new Policy
                    {
                        BranchId = noticeVehicleDTO.BranchId,
                        PrefixId = noticeVehicleDTO.PrefixId,
                        Id = Convert.ToInt32(noticeVehicleDTO.PolicyId),
                        DocumentNumber = noticeVehicleDTO.DocumentNumber,
                        CurrentFrom = Convert.ToDateTime(noticeVehicleDTO.PolicyCurrentFrom),
                        CurrentTo = Convert.ToDateTime(noticeVehicleDTO.PolicyCurrentTo)
                    },
                    TemporalId = noticeVehicleDTO.TemporalId
                }
            };

            return noticeVehicle;
        }

        internal static PendingOperation CreatePendingOperationByClaimReserve(ClaimReserve claimReserve)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = claimReserve.Claim.Modifications.Last().UserId,
                Operation = JsonConvert.SerializeObject(claimReserve)
            };
        }

        internal static NoticeLocation CreateNoticeLocation(NoticeLocationDTO noticeLocationDTO, ContactInformationDTO contactInformationDTO, RiskLocationDTO locationDTO)
        {
            NoticeLocation noticeLocation = new NoticeLocation
            {
                Address = locationDTO.FullAddress,
                CountryId = Convert.ToInt32(locationDTO.CountryId),
                StateId = Convert.ToInt32(locationDTO.StateId),
                CityId = Convert.ToInt32(locationDTO.CityId),
                Notice = new Notice
                {
                    Id = noticeLocationDTO.Id,
                    CreationDate = noticeLocationDTO.NoticeDate,
                    ClaimDate = noticeLocationDTO.OcurrenceDate,
                    Address = noticeLocationDTO.Location,
                    City = new City
                    {
                        Id = noticeLocationDTO.CityId,
                        State = new State
                        {
                            Id = noticeLocationDTO.StateId,
                            Country = new Country
                            {
                                Id = noticeLocationDTO.CountryId
                            }
                        }
                    },
                    Description = noticeLocationDTO.Description,
                    Endorsement = new ClaimEndorsement
                    {
                        Id = Convert.ToInt32(noticeLocationDTO.EndorsementId),
                        PolicyId = Convert.ToInt32(noticeLocationDTO.PolicyId),
                        Number = Convert.ToInt32(noticeLocationDTO.EndorsementNumber)
                    },
                    ObjectedReason = noticeLocationDTO.ObjectedDescription,
                    UserId = noticeLocationDTO.UserId,
                    UserProfileId = noticeLocationDTO.UserProfileId,
                    CoveredRiskTypeId = Convert.ToInt32(noticeLocationDTO.CoveredRiskTypeId),
                    Type = new NoticeType
                    {
                        Id = noticeLocationDTO.NoticeTypeId
                    },
                    Risk = new Risk
                    {
                        RiskId = Convert.ToInt32(noticeLocationDTO.RiskId)
                    },
                    ContactInformation = new ContactInformation
                    {
                        Name = contactInformationDTO.Name,
                        Phone = contactInformationDTO.Phone,
                        Mail = contactInformationDTO.Mail
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = Convert.ToInt32(noticeLocationDTO.NoticeReasonId)
                    },

                    NoticeState = new NoticeState
                    {
                        Id = Convert.ToInt32(noticeLocationDTO.NoticeStateId)
                    },
                    NumberObjected = noticeLocationDTO.NumberObjected,
                    OthersAffected = noticeLocationDTO.OthersAffected,
                    InternalConsecutive = noticeLocationDTO.InternalConsecutive,
                    ClaimedAmount = noticeLocationDTO.ClaimedAmount,
                    ClaimReasonOthers = noticeLocationDTO.ClaimReasonOthers,
                    Number = noticeLocationDTO.Number,
                    IndividualId = noticeLocationDTO.IndividualId,
                    NoticeCoverages = CreateNoticeCoverage(noticeLocationDTO.Coverages),
                    Policy = new Policy
                    {
                        BranchId = noticeLocationDTO.BranchId,
                        PrefixId = noticeLocationDTO.PrefixId,
                        Id = Convert.ToInt32(noticeLocationDTO.PolicyId),
                        DocumentNumber = noticeLocationDTO.DocumentNumber,
                        CurrentFrom = Convert.ToDateTime(noticeLocationDTO.PolicyCurrentFrom),
                        CurrentTo = Convert.ToDateTime(noticeLocationDTO.PolicyCurrentTo),
                        TypeId = Convert.ToInt32(noticeLocationDTO.PolicyTypeId),
                        BusinessTypeId = Convert.ToInt32(noticeLocationDTO.PolicyBusinessTypeId),
                        ProductId = Convert.ToInt32(noticeLocationDTO.PolicyProductId)
                    },
                    TemporalId = noticeLocationDTO.TemporalId
                }
            };

            return noticeLocation;
        }

        internal static NoticeSurety CreateNoticeSurety(NoticeSuretyDTO noticeSuretyDTO, ContactInformationDTO contactInformationDTO, SuretyDTO suretyDTO)
        {
            NoticeSurety noticeSurety = new NoticeSurety
            {
                Name = suretyDTO.SuretyName,
                DocumentNumber = suretyDTO.SuretyDocumentNumber,
                CourtNum = suretyDTO.CourtNum,
                BidNumber = suretyDTO.BidNumber,
                Notice = new Notice
                {
                    CreationDate = noticeSuretyDTO.NoticeDate,
                    ClaimDate = noticeSuretyDTO.OcurrenceDate,
                    Address = noticeSuretyDTO.Location,
                    City = new City
                    {
                        Id = noticeSuretyDTO.CityId,
                        State = new State
                        {
                            Id = noticeSuretyDTO.StateId,
                            Country = new Country
                            {
                                Id = noticeSuretyDTO.CountryId
                            }
                        }
                    },
                    Description = noticeSuretyDTO.Description,
                    Endorsement = new ClaimEndorsement
                    {
                        Id = Convert.ToInt32(noticeSuretyDTO.EndorsementId),
                        PolicyId = Convert.ToInt32(noticeSuretyDTO.PolicyId),
                        Number = Convert.ToInt32(noticeSuretyDTO.EndorsementNumber)
                    },
                    ObjectedReason = noticeSuretyDTO.ObjectedDescription,
                    UserId = noticeSuretyDTO.UserId,
                    UserProfileId = noticeSuretyDTO.UserProfileId,
                    CoveredRiskTypeId = Convert.ToInt32(noticeSuretyDTO.CoveredRiskTypeId),
                    Type = new NoticeType
                    {
                        Id = noticeSuretyDTO.NoticeTypeId
                    },
                    Id = noticeSuretyDTO.Id,
                    Risk = new Risk
                    {
                        RiskId = Convert.ToInt32(noticeSuretyDTO.RiskId)
                    },
                    ContactInformation = new ContactInformation
                    {
                        Name = contactInformationDTO.Name,
                        Phone = contactInformationDTO.Phone,
                        Mail = contactInformationDTO.Mail
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = Convert.ToInt32(noticeSuretyDTO.NoticeReasonId)
                    },

                    NoticeState = new NoticeState
                    {
                        Id = Convert.ToInt32(noticeSuretyDTO.NoticeStateId)
                    },
                    NumberObjected = noticeSuretyDTO.NumberObjected,
                    OthersAffected = noticeSuretyDTO.OthersAffected,
                    InternalConsecutive = noticeSuretyDTO.InternalConsecutive,
                    ClaimedAmount = noticeSuretyDTO.ClaimedAmount,
                    ClaimReasonOthers = noticeSuretyDTO.ClaimReasonOthers,
                    Number = noticeSuretyDTO.Number,
                    IndividualId = noticeSuretyDTO.IndividualId,
                    NoticeCoverages = CreateNoticeCoverage(noticeSuretyDTO.Coverages),
                    Policy = new Policy
                    {
                        BranchId = noticeSuretyDTO.BranchId,
                        PrefixId = noticeSuretyDTO.PrefixId,
                        Id = Convert.ToInt32(noticeSuretyDTO.PolicyId),
                        DocumentNumber = noticeSuretyDTO.DocumentNumber,
                        CurrentFrom = Convert.ToDateTime(noticeSuretyDTO.PolicyCurrentFrom),
                        CurrentTo = Convert.ToDateTime(noticeSuretyDTO.PolicyCurrentTo),
                        TypeId = Convert.ToInt32(noticeSuretyDTO.PolicyTypeId),
                        BusinessTypeId = Convert.ToInt32(noticeSuretyDTO.PolicyBusinessTypeId),
                        ProductId = Convert.ToInt32(noticeSuretyDTO.PolicyProductId)
                    },
                    TemporalId = noticeSuretyDTO.TemporalId
                }
            };

            return noticeSurety;
        }

        internal static NoticeTransport CreateNoticeTransport(NoticeTransportDTO noticeTransportDTO, ContactInformationDTO contactInformationDTO, TransportDTO transportDTO)
        {
            NoticeTransport noticeTransport = new NoticeTransport
            {
                CargoType = transportDTO.CargoTypeDescription,
                PackagingType = transportDTO.PackagingTypeDescription,
                Origin = transportDTO.CityFromDescription,
                Destiny = transportDTO.CityToDescription,
                TransportType = transportDTO.ViaTypeDescription,
                Notice = new Notice
                {
                    CreationDate = noticeTransportDTO.NoticeDate,
                    ClaimDate = noticeTransportDTO.OcurrenceDate,
                    Address = noticeTransportDTO.Location,
                    City = new City
                    {
                        Id = noticeTransportDTO.CityId,
                        State = new State
                        {
                            Id = noticeTransportDTO.StateId,
                            Country = new Country
                            {
                                Id = noticeTransportDTO.CountryId
                            }
                        }
                    },
                    Description = noticeTransportDTO.Description,
                    Endorsement = new ClaimEndorsement
                    {
                        Id = Convert.ToInt32(noticeTransportDTO.EndorsementId),
                        PolicyId = Convert.ToInt32(noticeTransportDTO.PolicyId),
                        Number = Convert.ToInt32(noticeTransportDTO.EndorsementNumber)
                    },
                    ObjectedReason = noticeTransportDTO.ObjectedDescription,
                    UserId = noticeTransportDTO.UserId,
                    UserProfileId = noticeTransportDTO.UserProfileId,
                    CoveredRiskTypeId = Convert.ToInt32(noticeTransportDTO.CoveredRiskTypeId),
                    Type = new NoticeType
                    {
                        Id = noticeTransportDTO.NoticeTypeId
                    },
                    Id = noticeTransportDTO.Id,
                    Risk = new Risk
                    {
                        RiskId = Convert.ToInt32(noticeTransportDTO.RiskId)
                    },
                    ContactInformation = new ContactInformation
                    {
                        Name = contactInformationDTO.Name,
                        Phone = contactInformationDTO.Phone,
                        Mail = contactInformationDTO.Mail
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = Convert.ToInt32(noticeTransportDTO.NoticeReasonId)
                    },

                    NoticeState = new NoticeState
                    {
                        Id = Convert.ToInt32(noticeTransportDTO.NoticeStateId)
                    },
                    NumberObjected = noticeTransportDTO.NumberObjected,
                    OthersAffected = noticeTransportDTO.OthersAffected,
                    InternalConsecutive = noticeTransportDTO.InternalConsecutive,
                    ClaimedAmount = noticeTransportDTO.ClaimedAmount,
                    ClaimReasonOthers = noticeTransportDTO.ClaimReasonOthers,
                    Number = noticeTransportDTO.Number,
                    IndividualId = noticeTransportDTO.IndividualId,
                    NoticeCoverages = CreateNoticeCoverage(noticeTransportDTO.Coverages),
                    Policy = new Policy
                    {
                        BranchId = noticeTransportDTO.BranchId,
                        PrefixId = noticeTransportDTO.PrefixId,
                        Id = Convert.ToInt32(noticeTransportDTO.PolicyId),
                        DocumentNumber = noticeTransportDTO.DocumentNumber,
                        CurrentFrom = Convert.ToDateTime(noticeTransportDTO.PolicyCurrentFrom),
                        CurrentTo = Convert.ToDateTime(noticeTransportDTO.PolicyCurrentTo),
                        TypeId = Convert.ToInt32(noticeTransportDTO.PolicyTypeId),
                        BusinessTypeId = Convert.ToInt32(noticeTransportDTO.PolicyBusinessTypeId),
                        ProductId = Convert.ToInt32(noticeTransportDTO.PolicyProductId)
                    },
                    TemporalId = noticeTransportDTO.TemporalId
                }
            };

            return noticeTransport;
        }

        internal static NoticeAirCraft CreateNoticeAirCraft(NoticeAirCraftDTO noticeAirCraftDTO, ContactInformationDTO contactInformationDTO, AirCraftDTO airCraftDTO)
        {
            NoticeAirCraft noticeAirCraft = new NoticeAirCraft
            {
                MakeId = airCraftDTO.MakeId,
                ModelId = airCraftDTO.ModelId,
                TypeId = airCraftDTO.TypeId,
                UseId = airCraftDTO.UseId,
                RegisterId = airCraftDTO.RegisterId,
                OperatorId = airCraftDTO.OperatorId,
                RegisterNumer = airCraftDTO.RegisterNumber,
                Year = airCraftDTO.Year,
                Notice = new Notice
                {
                    CreationDate = noticeAirCraftDTO.NoticeDate,
                    ClaimDate = noticeAirCraftDTO.OcurrenceDate,
                    Address = noticeAirCraftDTO.Location,
                    City = new City
                    {
                        Id = noticeAirCraftDTO.CityId,
                        State = new State
                        {
                            Id = noticeAirCraftDTO.StateId,
                            Country = new Country
                            {
                                Id = noticeAirCraftDTO.CountryId
                            }
                        }
                    },
                    Description = noticeAirCraftDTO.Description,
                    Endorsement = new ClaimEndorsement
                    {
                        Id = Convert.ToInt32(noticeAirCraftDTO.EndorsementId),
                        PolicyId = Convert.ToInt32(noticeAirCraftDTO.PolicyId),
                        Number = Convert.ToInt32(noticeAirCraftDTO.EndorsementNumber)
                    },
                    ObjectedReason = noticeAirCraftDTO.ObjectedDescription,
                    UserId = noticeAirCraftDTO.UserId,
                    UserProfileId = noticeAirCraftDTO.UserProfileId,
                    CoveredRiskTypeId = Convert.ToInt32(noticeAirCraftDTO.CoveredRiskTypeId),
                    Type = new NoticeType
                    {
                        Id = noticeAirCraftDTO.NoticeTypeId
                    },
                    Id = noticeAirCraftDTO.Id,
                    Risk = new Risk
                    {
                        RiskId = Convert.ToInt32(noticeAirCraftDTO.RiskId)
                    },
                    ContactInformation = new ContactInformation
                    {
                        Name = contactInformationDTO.Name,
                        Phone = contactInformationDTO.Phone,
                        Mail = contactInformationDTO.Mail
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = Convert.ToInt32(noticeAirCraftDTO.NoticeReasonId)
                    },

                    NoticeState = new NoticeState
                    {
                        Id = Convert.ToInt32(noticeAirCraftDTO.NoticeStateId)
                    },
                    NumberObjected = noticeAirCraftDTO.NumberObjected,
                    OthersAffected = noticeAirCraftDTO.OthersAffected,
                    InternalConsecutive = noticeAirCraftDTO.InternalConsecutive,
                    ClaimedAmount = noticeAirCraftDTO.ClaimedAmount,
                    ClaimReasonOthers = noticeAirCraftDTO.ClaimReasonOthers,
                    Number = noticeAirCraftDTO.Number,
                    IndividualId = noticeAirCraftDTO.IndividualId,
                    NoticeCoverages = CreateNoticeCoverage(noticeAirCraftDTO.Coverages),
                    Policy = new Policy
                    {
                        BranchId = noticeAirCraftDTO.BranchId,
                        PrefixId = noticeAirCraftDTO.PrefixId,
                        Id = Convert.ToInt32(noticeAirCraftDTO.PolicyId),
                        DocumentNumber = noticeAirCraftDTO.DocumentNumber,
                        CurrentFrom = Convert.ToDateTime(noticeAirCraftDTO.PolicyCurrentFrom),
                        CurrentTo = Convert.ToDateTime(noticeAirCraftDTO.PolicyCurrentTo),
                        TypeId = Convert.ToInt32(noticeAirCraftDTO.PolicyTypeId),
                        BusinessTypeId = Convert.ToInt32(noticeAirCraftDTO.PolicyBusinessTypeId),
                        ProductId = Convert.ToInt32(noticeAirCraftDTO.PolicyProductId)
                    },
                    TemporalId = noticeAirCraftDTO.TemporalId
                }
            };

            return noticeAirCraft;
        }

        internal static NoticeFidelity CreateNoticeFidelity(NoticeFidelityDTO noticeFidelityDTO, ContactInformationDTO contactInformationDTO, FidelityDTO fidelityDTO)
        {
            NoticeFidelity noticeFidelity = new NoticeFidelity
            {
                RiskCommercialClassId = fidelityDTO.CommercialClassId,
                OccupationId = fidelityDTO.OccupationId,
                DiscoveryDate = fidelityDTO.DiscoveryDate,
                Description = fidelityDTO.Description,
                Notice = new Notice
                {
                    CreationDate = noticeFidelityDTO.NoticeDate,
                    ClaimDate = noticeFidelityDTO.OcurrenceDate,
                    Address = noticeFidelityDTO.Location,
                    City = new City
                    {
                        Id = noticeFidelityDTO.CityId,
                        State = new State
                        {
                            Id = noticeFidelityDTO.StateId,
                            Country = new Country
                            {
                                Id = noticeFidelityDTO.CountryId
                            }
                        }
                    },
                    Description = noticeFidelityDTO.Description,
                    Endorsement = new ClaimEndorsement
                    {
                        Id = Convert.ToInt32(noticeFidelityDTO.EndorsementId),
                        PolicyId = Convert.ToInt32(noticeFidelityDTO.PolicyId),
                        Number = Convert.ToInt32(noticeFidelityDTO.EndorsementNumber)
                    },
                    ObjectedReason = noticeFidelityDTO.ObjectedDescription,
                    UserId = noticeFidelityDTO.UserId,
                    UserProfileId = noticeFidelityDTO.UserProfileId,
                    CoveredRiskTypeId = Convert.ToInt32(noticeFidelityDTO.CoveredRiskTypeId),
                    Type = new NoticeType
                    {
                        Id = noticeFidelityDTO.NoticeTypeId
                    },
                    Id = noticeFidelityDTO.Id,
                    Risk = new Risk
                    {
                        RiskId = Convert.ToInt32(noticeFidelityDTO.RiskId)
                    },
                    ContactInformation = new ContactInformation
                    {
                        Name = contactInformationDTO.Name,
                        Phone = contactInformationDTO.Phone,
                        Mail = contactInformationDTO.Mail
                    },
                    NoticeReason = new NoticeReason
                    {
                        Id = Convert.ToInt32(noticeFidelityDTO.NoticeReasonId)
                    },

                    NoticeState = new NoticeState
                    {
                        Id = Convert.ToInt32(noticeFidelityDTO.NoticeStateId)
                    },
                    NumberObjected = noticeFidelityDTO.NumberObjected,
                    OthersAffected = noticeFidelityDTO.OthersAffected,
                    InternalConsecutive = noticeFidelityDTO.InternalConsecutive,
                    ClaimedAmount = noticeFidelityDTO.ClaimedAmount,
                    ClaimReasonOthers = noticeFidelityDTO.ClaimReasonOthers,
                    Number = noticeFidelityDTO.Number,
                    IndividualId = noticeFidelityDTO.IndividualId,
                    NoticeCoverages = CreateNoticeCoverage(noticeFidelityDTO.Coverages),
                    Policy = new Policy
                    {
                        BranchId = noticeFidelityDTO.BranchId,
                        PrefixId = noticeFidelityDTO.PrefixId,
                        Id = Convert.ToInt32(noticeFidelityDTO.PolicyId),
                        DocumentNumber = noticeFidelityDTO.DocumentNumber,
                        CurrentFrom = Convert.ToDateTime(noticeFidelityDTO.PolicyCurrentFrom),
                        CurrentTo = Convert.ToDateTime(noticeFidelityDTO.PolicyCurrentTo),
                        TypeId = Convert.ToInt32(noticeFidelityDTO.PolicyTypeId),
                        BusinessTypeId = Convert.ToInt32(noticeFidelityDTO.PolicyBusinessTypeId),
                        ProductId = Convert.ToInt32(noticeFidelityDTO.PolicyProductId)
                    },
                    TemporalId = noticeFidelityDTO.TemporalId
                }
            };

            return noticeFidelity;
        }



        internal static List<ClaimModify> CreateClaimModifies(List<CLMEN.ClaimModify> entityclaimModifies)
        {
            List<ClaimModify> claimModifies = new List<ClaimModify>();

            foreach (CLMEN.ClaimModify entityClaimModify in entityclaimModifies.ToList())
            {
                ClaimModify claimModify = new ClaimModify
                {
                    Id = entityClaimModify.ClaimModifyCode,
                    RegistrationDate = entityClaimModify.RegistrationDate,
                    AccountingDate = entityClaimModify.AccountingDate,
                    UserId = entityClaimModify.UserId,
                    Coverages = new List<ClaimCoverage>()
                };
                claimModifies.Add(claimModify);
            }

            return claimModifies;
        }

        internal static List<ClaimCoverage> CreateClaimCoverages(List<CLMEN.ClaimCoverage> entityClaimCoverages)
        {
            List<ClaimCoverage> claimCoverages = new List<ClaimCoverage>();

            foreach (CLMEN.ClaimCoverage entityClaimCoverage in entityClaimCoverages.ToList())
            {
                ClaimCoverage claimCoverage = new ClaimCoverage
                {
                    Id = entityClaimCoverage.ClaimCoverageCode,
                    SubClaim = Convert.ToInt32(entityClaimCoverage.SubClaim),
                    RiskId = Convert.ToInt32(entityClaimCoverage.RiskId),
                    RiskNumber = entityClaimCoverage.RiskNum,
                    CoverageId = Convert.ToInt32(entityClaimCoverage.CoverageId),
                    CoverageNumber = entityClaimCoverage.CoverageNum,
                    IsInsured = Convert.ToBoolean(entityClaimCoverage.IsInsured),
                    IsProspect = Convert.ToBoolean(entityClaimCoverage.IsProspect),
                    TextOperation = new TextOperation
                    {
                        Id = Convert.ToInt32(entityClaimCoverage.TextOperationCode)
                    },
                    IndividualId = entityClaimCoverage.IndividualId,
                    Estimations = new List<Estimation>()
                };
                claimCoverages.Add(claimCoverage);
            }

            return claimCoverages;
        }

        internal static List<Estimation> CreateClaimCoverageAmounts(List<CLMEN.ClaimCoverageAmount> entityClaimCoverageAmounts)
        {
            List<Estimation> Estimations = new List<Estimation>();

            foreach (CLMEN.ClaimCoverageAmount entityClaimCoverageAmount in entityClaimCoverageAmounts.ToList())
            {
                Estimation estimation = new Estimation
                {
                    Amount = Convert.ToDecimal(entityClaimCoverageAmount.EstimateAmount),
                    DeductibleAmount = Convert.ToDecimal(entityClaimCoverageAmount.DeductibleAmount),
                    Currency = new Currency
                    {
                        Id = Convert.ToInt32(entityClaimCoverageAmount.CurrencyCode)
                    },
                    AmountAccumulate = Convert.ToDecimal(entityClaimCoverageAmount.EstimateAmountAccumulate),
                    Reason = new Reason
                    {
                        Id = Convert.ToInt32(entityClaimCoverageAmount.EstimationTypeStatusReasonCode)
                    },
                    Type = new EstimationType
                    {
                        Id = entityClaimCoverageAmount.EstimationTypeCode
                    },
                    EstimationTypeStatus = new EstimationTypeStatus
                    {
                        Id = Convert.ToInt32(entityClaimCoverageAmount.EstimationTypeStatusCode)
                    },
                    IsMinimumSalary = Convert.ToBoolean(entityClaimCoverageAmount.IsMinimumSalary),
                    MinimumSalariesNumber = Convert.ToDecimal(entityClaimCoverageAmount.MinimumSalariesNumber),
                    MinimumSalaryValue = entityClaimCoverageAmount.MinimumSalaryValue,
                    CreationDate = Convert.ToDateTime(entityClaimCoverageAmount.Date)
                };
                Estimations.Add(estimation);
            }

            return Estimations;
        }

        internal static SubCause CreateSubCause(SubCauseDTO subCauseDTO)
        {
            return new SubCause
            {
                Id = subCauseDTO.Id,
                Description = subCauseDTO.Description,
                Cause = new Cause
                {
                    Id = subCauseDTO.CauseId
                }
            };
        }

        internal static ClaimDocumentation CreateDocumentations(ClaimDocumentationDTO claimsDocumentationDTO)
        {
            return new ClaimDocumentation
            {
                Id = claimsDocumentationDTO.Id,
                ModuleId = claimsDocumentationDTO.ModuleId,
                SubmoduleId = claimsDocumentationDTO.SubmoduleId,
                Description = claimsDocumentationDTO.Description,
                prefix = new Prefix
                {
                    Id = claimsDocumentationDTO.prefix
                },
                IsRequired = claimsDocumentationDTO.IsRequired,
                Enable = claimsDocumentationDTO.Enable

            };
        }

        internal static CoveragePaymentConcept CreateCoveragePaymentConcepts(CoveragePaymentConceptDTO coveragePaymentConceptDTO)
        {
            return new CoveragePaymentConcept
            {
                CoverageId = coveragePaymentConceptDTO.CoverageId,
                ConceptId = coveragePaymentConceptDTO.ConceptId,
                EstimationTypeId = coveragePaymentConceptDTO.EstimationTypeId
            };
        }


        internal static Participant CreateParticipant(ClaimParticipantDTO claimsParticipantDTO)
        {
            return new Participant
            {
                Id = claimsParticipantDTO.Id,
                DocumentNumber = claimsParticipantDTO.DocumentNumber,
                Fullname = claimsParticipantDTO.Fullname,
                Phone = claimsParticipantDTO.Phone,
                Address = claimsParticipantDTO.Address
            };
        }

        internal static UNDMO.PolicyDTO CreateClaimPolicy(PolicyDTO policy)
        {
            return new UNDMO.PolicyDTO
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
                PersonTypeId = policy.PersonTypeId,
                ProductDescription = policy.ProducDescription
            };
        }

        internal static PendingOperation CreatePendingOperation(PendingOperationDTO pendingOperationDTO)
        {
            return new PendingOperation
            {
                Id = pendingOperationDTO.Id,
                CreationDate = pendingOperationDTO.CreationDate,
                UserId = pendingOperationDTO.UserId,
                Operation = pendingOperationDTO.Operation
            };
        }

        #endregion

        #endregion

        #region PaymentRequest

        #region ModelToModel

        internal static PendingOperation CreatePendingOperationByPaymentRequest(PaymentRequest paymentRequest)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = paymentRequest.UserId,
                Operation = JsonConvert.SerializeObject(paymentRequest)
            };
        }

        internal static PendingOperation CreatePendingOperationByChargeRequest(ChargeRequest chargeRequest)
        {
            return new PendingOperation
            {
                CreationDate = DateTime.Now,
                UserId = chargeRequest.UserId,
                Operation = JsonConvert.SerializeObject(chargeRequest)
            };
        }

        internal static PaymentRequestControl CreatePaymentRequestControl(PaymentRequest paymentRequest)
        {
            return new PaymentRequestControl
            {
                PaymentRequestId = paymentRequest.Id,
                Action = "I"
            };
        }

        internal static PaymentRequestControl CreatePaymentRequestControl(PAYMEN.PaymentRequest entityPaymentRequest)
        {
            return new PaymentRequestControl
            {
                PaymentRequestId = entityPaymentRequest.PaymentRequestCode,
                Action = "I"
            };
        }

        #endregion

        #region EntityToModel
        //internal static List<ConceptSource> CreatePaymentSources(BusinessCollection businessCollection)
        //{
        //    List<ConceptSource> paymentSource = new List<ConceptSource>();

        //    foreach (PAYMEN.PaymentConceptSource entityPaymentConceptSource in businessCollection)
        //    {
        //        paymentSource.Add(CreatePaymentSource(entityPaymentConceptSource));
        //    }

        //    return paymentSource;
        //}

        //internal static ConceptSource CreatePaymentSource(PAYMEN.PaymentConceptSource entityPaymentConceptSource)
        //{
        //    return new ConceptSource
        //    {
        //        Id = entityPaymentConceptSource.ConceptSourceCode,
        //        isChargeRequest = entityPaymentConceptSource.ChargeRequestEnable
        //    };
        //}

        internal static List<Role> CreateRoles(BusinessCollection businessCollection)
        {
            List<Role> roles = new List<Role>();

            foreach (UPEN.Role entityRole in businessCollection)
            {
                roles.Add(CreateRole(entityRole));
            }

            return roles;
        }

        internal static Role CreateRole(UPEN.Role entityClaimSurety)
        {
            return new Role
            {
                Id = entityClaimSurety.RoleCode,
                Description = entityClaimSurety.Description
            };
        }

        internal static List<VoucherType> CreateVoucherTypes(BusinessCollection businessCollection)
        {
            List<VoucherType> voucherTypes = new List<VoucherType>();

            foreach (PARAMEN.VoucherType entityVoucherType in businessCollection)
            {
                voucherTypes.Add(CreateVoucherType(entityVoucherType));
            }

            return voucherTypes;
        }

        internal static VoucherType CreateVoucherType(PARAMEN.VoucherType entityClaimSurety)
        {
            return new VoucherType
            {
                Id = entityClaimSurety.VoucherTypeCode,
                Description = entityClaimSurety.Description
            };
        }

        internal static List<ClaimPaymentMethod> CreatePaymentMethods(BusinessCollection businessCollection)
        {
            List<ClaimPaymentMethod> paymentMethods = new List<ClaimPaymentMethod>();

            foreach (CLMEN.PaymentMethod entityPaymentMethod in businessCollection)
            {
                paymentMethods.Add(CreatePaymentMethod(entityPaymentMethod));
            }

            return paymentMethods;
        }

        internal static ClaimPaymentMethod CreatePaymentMethod(CLMEN.PaymentMethod entityPaymentMethod)
        {
            return new ClaimPaymentMethod
            {
                Id = entityPaymentMethod.PaymentMethodCode,
                Description = entityPaymentMethod.Description
            };
        }

        internal static List<PaymentRequest> CreatePaymentRequests(BusinessCollection businessCollection)
        {
            List<PaymentRequest> paymentRequests = new List<PaymentRequest>();

            foreach (PAYMEN.PaymentRequest entityPaymentRequest in businessCollection)
            {
                paymentRequests.Add(CreatePaymentRequest(entityPaymentRequest));
            }

            return paymentRequests;
        }

        internal static PaymentRequest CreatePaymentRequest(PAYMEN.PaymentRequest entityPaymentRequest)
        {
            return new PaymentRequest
            {
                Id = entityPaymentRequest.PaymentRequestCode,
                Description = entityPaymentRequest.Description,
                AccountBankId = Convert.ToInt32(entityPaymentRequest.AccountBankCode),
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entityPaymentRequest.BranchCode)
                },
                Currency = new Currency
                {
                    Id = Convert.ToInt32(entityPaymentRequest.CurrencyCode)
                },
                EstimatedDate = Convert.ToDateTime(entityPaymentRequest.EstimatedDate),
                IndividualId = Convert.ToInt32(entityPaymentRequest.IndividualId),
                IsPrinted = Convert.ToBoolean(entityPaymentRequest.IsPrinted),
                MovementType = new MovementType
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PaymentMovementTypeCode),
                    Source = new ConceptSource
                    {
                        Id = Convert.ToInt32(entityPaymentRequest.PaymentSourceCode)
                    }
                },
                Number = Convert.ToInt32(entityPaymentRequest.Number),
                AccountingDate = Convert.ToDateTime(entityPaymentRequest.PaymentDate),
                PersonType = new ClaimPersonType
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PersonTypeCode)
                },
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PrefixCode)
                },
                RegistrationDate = Convert.ToDateTime(entityPaymentRequest.RegistrationDate),
                TotalAmount = Convert.ToDecimal(entityPaymentRequest.TotalAmount),
                Type = new Models.PaymentRequest.PaymentRequestType
                {
                    Id = Convert.ToInt32(entityPaymentRequest.TypePaymentRequestCode)
                },
                UserId = Convert.ToInt32(entityPaymentRequest.UserId),
                PaymentMethod = new ClaimPaymentMethod
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PaymentMethodCode)
                },
                TechnicalTransaction = Convert.ToInt32(entityPaymentRequest.TechnicalTransaction)
            };
        }

        internal static List<ClaimPersonType> CreatePersonTypes(BusinessCollection businessCollection)
        {
            List<ClaimPersonType> claimPersonTypes = new List<ClaimPersonType>();

            foreach (PARAMEN.PersonType entityPersonType in businessCollection)
            {
                claimPersonTypes.Add(CreatePersonType(entityPersonType));
            }

            return claimPersonTypes;
        }

        internal static ClaimPersonType CreatePersonType(PARAMEN.PersonType entityPersonType)
        {
            ClaimPersonType claimPersonType = new ClaimPersonType
            {
                Id = entityPersonType.PersonTypeCode,
                Description = entityPersonType.Description,
                IsEnabled = Convert.ToBoolean(entityPersonType.Enabled),
                IsBillEnabled = Convert.ToBoolean(entityPersonType.BillEnabled),
                IsPreaplicationEnabled = Convert.ToBoolean(entityPersonType.PreaplicationEnabled),
                SmallDescription = entityPersonType.Description,
                IsPaymentOrderEnabled = Convert.ToBoolean(entityPersonType.PaymentOrderEnable)
            };

            return claimPersonType;
        }

        internal static AccountBank CreateAccountBank(COMMEN.AccountBank entityAccountBank)
        {
            return new AccountBank
            {
                Id = entityAccountBank.AccountBankCode,
                AccountTypeId = entityAccountBank.AccountTypeCode,
                Number = entityAccountBank.Number,
                BankId = entityAccountBank.BankCode,
                Enabled = entityAccountBank.Enabled,
                Default = entityAccountBank.Default,
                CurrencyId = entityAccountBank.CurrencyCode,
                GeneralLedgerId = entityAccountBank.GeneralLedgerCode,
                DisabledDate = entityAccountBank.DisabledDate,
                BranchId = entityAccountBank.BranchCode,
                Description = entityAccountBank.Description
            };
        }

        internal static List<AccountBank> CreateAccountBanks(BusinessCollection businessCollection)
        {
            List<AccountBank> accountBanks = new List<AccountBank>();

            foreach (COMMEN.AccountBank entityAccountBank in businessCollection)
            {
                accountBanks.Add(CreateAccountBank(entityAccountBank));
            }

            return accountBanks;
        }

        internal static Voucher CreateVoucher(PAYMEN.PaymentVoucher entityPaymentVoucher)
        {
            return new Voucher()
            {
                Id = entityPaymentVoucher.PaymentVoucherCode,
                Number = entityPaymentVoucher.Number,
                Date = Convert.ToDateTime(entityPaymentVoucher.Date),
                ExchangeRate = Convert.ToDecimal(entityPaymentVoucher.ExchangeRate),
                Currency = new Currency
                {
                    Id = Convert.ToInt32(entityPaymentVoucher.CurrencyCode)
                },
                VoucherType = new VoucherType
                {
                    Id = Convert.ToInt32(entityPaymentVoucher.VoucherTypeCode)
                }
            };
        }
        internal static List<Voucher> CreateVouchers(List<PAYMEN.PaymentVoucher> businessCollection)
        {
            List<Voucher> vouchers = new List<Voucher>();

            foreach (PAYMEN.PaymentVoucher entityPaymentVoucher in businessCollection)
            {
                vouchers.Add(CreateVoucher(entityPaymentVoucher));
            }

            return vouchers;
        }

        internal static List<Voucher> CreateVouchers(BusinessCollection businessCollection)
        {
            List<Voucher> vouchers = new List<Voucher>();

            foreach (PAYMEN.PaymentVoucher entityPaymentVoucher in businessCollection)
            {
                vouchers.Add(CreateVoucher(entityPaymentVoucher));
            }

            return vouchers;
        }

        internal static VoucherConcept CreateVoucherConcept(PAYMEN.PaymentVoucherConcept entityPaymentVoucherConcept)
        {
            return new VoucherConcept()
            {
                Id = entityPaymentVoucherConcept.PaymentVoucherConceptCode,
                Value = Convert.ToDecimal(entityPaymentVoucherConcept.Value),
                CostCenter = Convert.ToInt32(entityPaymentVoucherConcept.CostCenter),
                PaymentConcept = new PaymentConcept
                {
                    Id = Convert.ToInt32(entityPaymentVoucherConcept.PaymentConceptCode)
                },
                Retention = Convert.ToDecimal(entityPaymentVoucherConcept.Retention),
                TaxValue = Convert.ToDecimal(entityPaymentVoucherConcept.TaxValue)
            };
        }

        internal static VoucherConceptTax CreateVoucherConceptTax(PAYMEN.PaymentVoucherConceptTax entityPaymentRequestTax)
        {
            return new VoucherConceptTax
            {
                Id = entityPaymentRequestTax.PaymentVoucherConceptTaxCode,
                TaxValue = Convert.ToDecimal(entityPaymentRequestTax.TaxValue),
                CategoryId = Convert.ToInt32(entityPaymentRequestTax.TaxCategoryCode),
                ConditionId = Convert.ToInt32(entityPaymentRequestTax.TaxConditionCode),
                TaxBaseAmount = Convert.ToDecimal(entityPaymentRequestTax.TaxBaseAmount),
                TaxRate = Convert.ToDecimal(entityPaymentRequestTax.TaxRate),
                TaxId = Convert.ToInt32(entityPaymentRequestTax.TaxCode)
            };
        }


        internal static List<VoucherConcept> CreateVoucherConcepts(BusinessCollection businessCollection)
        {
            List<VoucherConcept> voucherConcepts = new List<VoucherConcept>();

            foreach (PAYMEN.PaymentVoucherConcept entityPaymentVoucherConcept in businessCollection)
            {
                voucherConcepts.Add(CreateVoucherConcept(entityPaymentVoucherConcept));
            }

            return voucherConcepts;
        }


        internal static List<VoucherConceptTax> CreateVoucherConceptTaxesByVoucherConceptId(BusinessCollection businessCollection)
        {
            List<VoucherConceptTax> voucherConceptTaxes = new List<VoucherConceptTax>();

            foreach (PAYMEN.PaymentVoucherConceptTax entityPaymentRequestTax in businessCollection)
            {
                voucherConceptTaxes.Add(CreateVoucherConceptTax(entityPaymentRequestTax));
            }

            return voucherConceptTaxes;
        }

        internal static Models.PaymentRequest.PaymentPlan CreatePaymentPlan(PAYMEN.PaymentPlan entityPaymentPlan)
        {
            if (entityPaymentPlan == null)
                return null;

            return new PaymentPlan
            {
                Id = entityPaymentPlan.PaymentPlanCode,
                Currency = new Currency
                {
                    Id = Convert.ToInt32(entityPaymentPlan.CurrencyCode)
                },
                PaymentClass = new PaymentClass
                {
                    Id = Convert.ToInt32(entityPaymentPlan.ClassPaymentCode)
                },
                Tax = Convert.ToDecimal(entityPaymentPlan.TaxPercentage)
            };
        }

        internal static List<PaymentPlan> CreatePaymentPlans(BusinessCollection businessCollection)
        {
            List<PaymentPlan> paymentPlans = new List<PaymentPlan>();

            foreach (PAYMEN.PaymentPlan entityPaymentPlan in businessCollection)
            {
                paymentPlans.Add(CreatePaymentPlan(entityPaymentPlan));
            }

            return paymentPlans;
        }

        internal static PaymentQuota CreatePaymentQuota(PAYMEN.PaymentSchedule entityPaymentSchedule)
        {
            return new PaymentQuota
            {
                Id = entityPaymentSchedule.PaymentScheduleCode,
                Amount = Convert.ToDecimal(entityPaymentSchedule.NetAmount),
                ExpirationDate = Convert.ToDateTime(entityPaymentSchedule.ExpirationDate),
                Number = Convert.ToInt32(entityPaymentSchedule.PaymentNum)
            };
        }

        internal static List<PaymentQuota> CreatePaymentQuotas(BusinessCollection businessCollection)
        {
            List<PaymentQuota> paymentQuotas = new List<PaymentQuota>();

            foreach (PAYMEN.PaymentSchedule entityPaymentSchedule in businessCollection)
            {
                paymentQuotas.Add(CreatePaymentQuota(entityPaymentSchedule));
            }

            return paymentQuotas;
        }

        internal static ChargeRequest CreateChargeRequest(PAYMEN.PaymentRequest entityPaymentRequest)
        {
            return new ChargeRequest
            {
                Id = entityPaymentRequest.PaymentRequestCode,
                Description = entityPaymentRequest.Description,
                AccountBankId = Convert.ToInt32(entityPaymentRequest.AccountBankCode),
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entityPaymentRequest.BranchCode)
                },
                Currency = new Currency
                {
                    Id = Convert.ToInt32(entityPaymentRequest.CurrencyCode)
                },
                EstimatedDate = Convert.ToDateTime(entityPaymentRequest.EstimatedDate),
                IndividualId = Convert.ToInt32(entityPaymentRequest.IndividualId),
                IsPrinted = Convert.ToBoolean(entityPaymentRequest.IsPrinted),
                MovementType = new MovementType
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PaymentMovementTypeCode),
                    Source = new ConceptSource
                    {
                        Id = Convert.ToInt32(entityPaymentRequest.PaymentSourceCode)
                    }
                },
                Number = Convert.ToInt32(entityPaymentRequest.Number),
                AccountingDate = Convert.ToDateTime(entityPaymentRequest.PaymentDate),
                PersonType = new ClaimPersonType
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PersonTypeCode)
                },
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PrefixCode)
                },
                RegistrationDate = Convert.ToDateTime(entityPaymentRequest.RegistrationDate),
                TotalAmount = Convert.ToDecimal(entityPaymentRequest.TotalAmount),
                Type = new Models.PaymentRequest.PaymentRequestType
                {
                    Id = Convert.ToInt32(entityPaymentRequest.TypePaymentRequestCode)
                },
                UserId = Convert.ToInt32(entityPaymentRequest.UserId),
                PaymentMethod = new ClaimPaymentMethod
                {
                    Id = Convert.ToInt32(entityPaymentRequest.PaymentMethodCode)
                }
            };
        }

        internal static List<ChargeRequest> CreateChargeRequests(BusinessCollection businessCollection)
        {
            List<ChargeRequest> chargeRequests = new List<ChargeRequest>();

            foreach (PAYMEN.PaymentRequest chargeRequest in businessCollection)
            {
                chargeRequests.Add(CreateChargeRequest(chargeRequest));
            }

            return chargeRequests;
        }

        internal static List<ConceptSource> CreatePaymentSourcesByChargeRequests(BusinessCollection businessCollection)
        {
            List<ConceptSource> paymentSource = new List<ConceptSource>();

            foreach (PAYMEN.PaymentConceptSource entityPaymentConceptSource in businessCollection)
            {
                paymentSource.Add(CreatePaymentSourcesByChargeRequest(entityPaymentConceptSource));
            }

            return paymentSource;
        }

        internal static ConceptSource CreatePaymentSourcesByChargeRequest(PAYMEN.PaymentConceptSource entityPaymentConceptSource)
        {
            return new ConceptSource
            {
                Id = entityPaymentConceptSource.ConceptSourceCode,
                isChargeRequest = entityPaymentConceptSource.ChargeRequestEnable
            };
        }

        internal static Participant CreateParticipant(ChargeRequestDTO chargeRequest)
        {
            return new Participant
            {
                DocumentNumber = chargeRequest.BeneficiaryDocumentNumber,
                Fullname = chargeRequest.BeneficiaryFullName
            };
        }

        internal static PaymentRequestControl CreatePaymentRequestControl(INTEN.ClmPaymentControl entityCoTmpClmPaymentControl)
        {
            return new PaymentRequestControl
            {
                Id = entityCoTmpClmPaymentControl.PaymentControlId,
                PaymentRequestId = entityCoTmpClmPaymentControl.PaymentRequestCode,
                Action = entityCoTmpClmPaymentControl.Action
            };
        }

        #endregion

        #region DtoToModel
        internal static Branch CreatePaymentBranch(DTOs.Claims.BranchDTO branch)
        {
            return new Branch
            {
                Id = branch.Id,
                Description = branch.Description
            };
        }


        internal static PaymentRequest CreatePaymentRequest(PaymentRequestDTO paymentRequestDTO)
        {
            return new PaymentRequest
            {
                Id = paymentRequestDTO.Id,
                CoverageId = paymentRequestDTO.CoverageId,
                TemporalId = paymentRequestDTO.TemporalId,
                Description = paymentRequestDTO.Description,
                TechnicalTransaction = paymentRequestDTO.TechnicalTransaction,
                AccountBankId = paymentRequestDTO.AccountBankId,
                SalePointId = paymentRequestDTO.PolicySalePointId,
                IsTotal = paymentRequestDTO.IsTotal,
                SaveDailyEntryMessage = paymentRequestDTO.SaveDailyEntryMessage,
                ExchangeRate = paymentRequestDTO.ExchangeRate,
                Branch = new Branch
                {
                    Id = paymentRequestDTO.BranchId
                },
                Currency = new Currency
                {
                    Id = paymentRequestDTO.CurrencyId
                },
                EstimatedDate = paymentRequestDTO.EstimatedDate,
                IndividualId = paymentRequestDTO.IndividualId,
                IndividualName = paymentRequestDTO.BeneficiaryFullName,
                IndividualDocumentNumber = paymentRequestDTO.BeneficiaryDocumentNumber,
                IndividualDocumentTypeId = Convert.ToInt32(paymentRequestDTO.BeneficiaryDocumentType),
                AccountingDate = paymentRequestDTO.AccountingDate,
                IsPrinted = paymentRequestDTO.IsPrinted,
                MovementType = new MovementType
                {
                    Id = paymentRequestDTO.MovementTypeId,
                    Source = new ConceptSource
                    {
                        Id = paymentRequestDTO.PaymentSourceId
                    }
                },
                Number = paymentRequestDTO.Number,
                PersonType = new ClaimPersonType
                {
                    Id = paymentRequestDTO.PersonTypeId
                },
                Prefix = new Prefix
                {
                    Id = paymentRequestDTO.PrefixId,
                    LineBusinessId = paymentRequestDTO.LineBusinessId
                },
                RegistrationDate = DateTime.Now,
                TotalAmount = paymentRequestDTO.TotalAmount,
                Type = new Models.PaymentRequest.PaymentRequestType
                {
                    Id = paymentRequestDTO.PaymentRequestTypeId
                },
                UserId = paymentRequestDTO.UserId,
                PaymentMethod = new ClaimPaymentMethod
                {
                    Id = paymentRequestDTO.PaymentMethodId
                },
                Claims = paymentRequestDTO.Claims == null ? new List<Claim>() : CreateClaims(paymentRequestDTO.Claims),
            };
        }

        internal static List<Claim> CreateClaims(List<SubClaimDTO> subclaims)
        {
            List<Claim> claim = new List<Claim>();
            subclaims.ForEach(x => claim.Add(CreateClaim(x)));
            return claim;
        }

        internal static Claim CreateClaim(SubClaimDTO subclaim)
        {
            return new Claim
            {
                Id = Convert.ToInt32(subclaim.ClaimId),
                SubClaim = Convert.ToInt32(subclaim.SubClaim),
                OccurrenceDate = Convert.ToDateTime(subclaim.OccurrenceDate),
                JudicialDecisionDate = Convert.ToDateTime(subclaim.JudicialDecisionDate),
                Number = Convert.ToInt32(subclaim.Number),
                BusinessTypeId = subclaim.BusinessTypeId,
                IsTotalParticipation = subclaim.IsTotalParticipation,
                Endorsement = new ClaimEndorsement
                {
                    PolicyNumber = subclaim.PolicyDocumentNumber,
                    PolicyId = subclaim.PolicyId,
                    Id = subclaim.EndorsementId,
                    Policy = new Policy
                    {
                        BusinessTypeId = subclaim.BusinessTypeId,
                        ProductId = subclaim.PolicyProductId
                    }
                },
                Branch = new Branch
                {
                    Id = Convert.ToInt32(subclaim.BranchCode)
                },
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(subclaim.PrefixId)
                },
                Modifications = new List<ClaimModify>
                    {
                        new ClaimModify
                        {
                            RegistrationDate = Convert.ToDateTime(subclaim.CreationDate),
                            Coverages = new List<ClaimCoverage>
                            {
                                new ClaimCoverage
                                {
                                    CoverageId = subclaim.CoverageId,
                                    ClaimCoverageCode = (subclaim.ClaimCoverageId == null ? 0 : Convert.ToInt32(subclaim.ClaimCoverageId)),
                                    SubClaim = Convert.ToInt32(subclaim.SubClaim),
                                    RiskId = Convert.ToInt32(subclaim.RiskId),
                                    Estimations = new List<Estimation>
                                    {
                                        new Estimation
                                        {
                                            CreationDate = Convert.ToDateTime(subclaim.EstimationDate),
                                            Amount = Convert.ToDecimal(subclaim.EstimateAmount),
                                            AmountAccumulate = subclaim.EstimateAmountAccumulate,
                                            IsMinimumSalary = subclaim.IsMinimumSalary,
                                            MinimumSalariesNumber = subclaim.MinimumSalariesNumber,
                                            MinimumSalaryValue = subclaim.MinimumSalaryValue,
                                            ExchangeRate = subclaim.ExchangeRate,
                                            Type = new EstimationType
                                            {
                                                Id = subclaim.EstimationTypeId
                                            },
                                            Reason = new Reason
                                            {
                                                Id = subclaim.EstimationTypeStatusReasonId,
                                                Status = new Status
                                                {
                                                    Id = subclaim.EstimationTypeStatusId,
                                                }
                                            },
                                            Currency = new Currency
                                            {
                                                Id = subclaim.EstimationCurrencyId
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                ,
                Vouchers = CreateVouchers(subclaim.Vouchers)
            };

        }

        internal static Participant CreateParticipant(PaymentRequestDTO paymentRequestDTO)
        {
            return new Participant()
            {
                Id = paymentRequestDTO.IndividualId,
                DocumentNumber = paymentRequestDTO.BeneficiaryDocumentNumber,
                Fullname = paymentRequestDTO.BeneficiaryFullName
            };
        }

        internal static Participant CreateParticipant(PaymentRequest paymentRequest)
        {
            return new Participant()
            {
                Id = paymentRequest.IndividualId,
                DocumentNumber = paymentRequest.IndividualDocumentNumber,
                Fullname = paymentRequest.IndividualName
            };
        }

        internal static List<Voucher> CreateVouchers(List<VoucherDTO> vouchersDTO)
        {
            List<Voucher> vouchers = new List<Voucher>();

            if (vouchersDTO != null)
            {
                foreach (VoucherDTO voucher in vouchersDTO)
                {
                    vouchers.Add(CreateVoucher(voucher));
                }
            }

            return vouchers;
        }

        internal static Voucher CreateVoucher(VoucherDTO voucherDTO)
        {
            if (voucherDTO == null)
                return null;

            Voucher voucher = new Voucher();
            voucher.Id = voucherDTO.Id;
            voucher.Currency = CreateCurrency(voucherDTO);
            voucher.VoucherType = CreateVoucherType(voucherDTO);
            voucher.Number = voucherDTO.Number;
            voucher.Date = voucherDTO.Date;
            voucher.ExchangeRate = voucherDTO.ExchangeRate;
            voucher.Concepts = voucherDTO.Concepts != null ? CreateVoucherConcepts(voucherDTO.Concepts) : new List<VoucherConcept>();
            voucher.EstimationType = CreateEstimationType(voucherDTO);

            return voucher;
        }

        internal static EstimationType CreateEstimationType(VoucherDTO voucher)
        {
            return new EstimationType
            {
                Id = voucher.EstimationTypeId
            };
        }

        internal static VoucherType CreateVoucherType(VoucherDTO voucher)
        {
            return new VoucherType
            {
                Id = voucher.VoucherTypeId
            };
        }

        internal static Currency CreateCurrency(VoucherDTO voucher)
        {
            return new Currency
            {
                Id = voucher.CurrencyId
            };
        }

        internal static List<VoucherConcept> CreateVoucherConcepts(List<VoucherConceptDTO> voucherConceptsDTO)
        {
            List<VoucherConcept> voucherConcepts = new List<VoucherConcept>();

            foreach (VoucherConceptDTO voucherConcept in voucherConceptsDTO)
            {
                voucherConcepts.Add(CreateVoucherConcept(voucherConcept));
            }

            return voucherConcepts;
        }
        internal static VoucherConcept CreateVoucherConcept(VoucherConceptDTO voucherConcept)
        {

            if (voucherConcept == null)
            {
                return null;
            }

            return new VoucherConcept
            {
                Id = voucherConcept.Id,
                Value = voucherConcept.Value,
                TaxValue = voucherConcept.TaxValue,
                Retention = voucherConcept.Retention,
                CostCenter = voucherConcept.CostCenter,
                VoucherConceptTaxes = voucherConcept.ConceptTaxes != null ? CreateVoucherConceptTaxes(voucherConcept.ConceptTaxes) : new List<VoucherConceptTax>(),
                PaymentConcept = new PaymentConcept
                {
                    Id = voucherConcept.PaymentConceptId,
                    Description = voucherConcept.PaymentConcept
                }
            };
        }

        internal static List<VoucherConceptTax> CreateVoucherConceptTaxes(List<VoucherConceptTaxDTO> voucherConceptTaxesDTO)
        {
            List<VoucherConceptTax> voucherConceptTaxes = new List<VoucherConceptTax>();

            foreach (VoucherConceptTaxDTO voucherConceptTax in voucherConceptTaxesDTO)
            {
                voucherConceptTaxes.Add(CreateVoucherConceptTax(voucherConceptTax));
            }

            return voucherConceptTaxes;
        }

        internal static VoucherConceptTax CreateVoucherConceptTax(VoucherConceptTaxDTO voucherConceptTaxDTO)
        {

            if (voucherConceptTaxDTO == null)
            {
                return null;
            }

            return new VoucherConceptTax
            {
                Id = voucherConceptTaxDTO.Id,
                TaxRate = voucherConceptTaxDTO.TaxRate,
                CategoryId = voucherConceptTaxDTO.CategoryId,
                ConditionId = voucherConceptTaxDTO.ConditionId,
                TaxId = voucherConceptTaxDTO.TaxId,
                TaxBaseAmount = voucherConceptTaxDTO.TaxBaseAmount,
                TaxValue = voucherConceptTaxDTO.TaxValue,
                Retention = voucherConceptTaxDTO.Retention,
                AccountingConceptId = voucherConceptTaxDTO.AccountingConceptId,
                TaxDescription = voucherConceptTaxDTO.TaxDescription
            };
        }

        internal static List<ChargeRequest> CreateChargeRequests(List<ChargeRequestDTO> chargeRequestsDTO)
        {
            List<ChargeRequest> chargeRequests = new List<ChargeRequest>();

            foreach (ChargeRequestDTO chargeRequestDTO in chargeRequestsDTO)
            {
                chargeRequests.Add(CreateChargeRequest(chargeRequestDTO));
            }

            return chargeRequests;
        }

        internal static ChargeRequest CreateChargeRequest(ChargeRequestDTO chargeRequestDTO)
        {
            return new ChargeRequest
            {
                Id = chargeRequestDTO.Id,
                Description = chargeRequestDTO.Description,
                AccountBankId = chargeRequestDTO.AccountBankId,
                Branch = new Branch
                {
                    Id = chargeRequestDTO.BranchId
                },
                Currency = new Currency
                {
                    Id = chargeRequestDTO.CurrencyId
                },
                EstimatedDate = chargeRequestDTO.EstimatedDate,
                IndividualId = chargeRequestDTO.IndividualId,
                IsPrinted = chargeRequestDTO.IsPrinted,
                MovementType = new MovementType
                {
                    Id = chargeRequestDTO.MovementTypeId,
                    Source = new ConceptSource
                    {
                        Id = chargeRequestDTO.PaymentSourceId
                    }
                },
                Number = chargeRequestDTO.Number,
                AccountingDate = chargeRequestDTO.AccountingDate,
                PersonType = new ClaimPersonType
                {
                    Id = chargeRequestDTO.PersonTypeId
                },
                Prefix = new Prefix
                {
                    Id = chargeRequestDTO.PrefixId
                },
                RegistrationDate = chargeRequestDTO.RegistrationDate,
                TotalAmount = chargeRequestDTO.TotalAmount,
                Type = new Models.PaymentRequest.PaymentRequestType
                {
                    Id = chargeRequestDTO.PaymentRequestTypeId
                },
                UserId = chargeRequestDTO.UserId,
                PaymentMethod = new ClaimPaymentMethod
                {
                    Id = chargeRequestDTO.PaymentMethodId
                },
                TechnicalTransaction = chargeRequestDTO.TechnicalTransaction,
                Claim = new Claim
                {
                    Id = Convert.ToInt32(chargeRequestDTO.ClaimId),
                    Number = Convert.ToInt32(chargeRequestDTO.ClaimNumber),
                    Endorsement = new ClaimEndorsement
                    {
                        Policy = new Policy
                        {
                            DocumentNumber = chargeRequestDTO.PolicyDocumentNumber
                        }
                    },
                    Branch = new Branch
                    {
                        Id = Convert.ToInt32(chargeRequestDTO.ClaimBranchId)
                    },
                    Modifications = new List<ClaimModify>
                    {
                        new ClaimModify
                        {
                            Coverages = new List<ClaimCoverage>
                            {
                                new ClaimCoverage
                                {
                                    SubClaim = Convert.ToInt32(chargeRequestDTO.SubClaim)
                                }
                            }
                        }
                    }
                },
                Participant = new Participant
                {
                    DocumentTypeId = chargeRequestDTO.BeneficiaryDocumentType,
                    DocumentNumber = chargeRequestDTO.BeneficiaryDocumentNumber,
                    Fullname = chargeRequestDTO.BeneficiaryFullName
                },
                Voucher = CreateVoucher(chargeRequestDTO.Voucher),
                SalvageId = Convert.ToInt32(chargeRequestDTO.SalvageId),
                RecoveryId = Convert.ToInt32(chargeRequestDTO.RecoveryId)
            };
        }

        internal static CoInsuranceAssigned CreateCoInsuranceAssigned(CoInsuranceAssignedDTO coInsuranceAssignedDTO)
        {
            return new CoInsuranceAssigned
            {
                EndorsementId = coInsuranceAssignedDTO.EndorsementId,
                PolicyId = coInsuranceAssignedDTO.PolicyId,
                CompanyNum = coInsuranceAssignedDTO.CompanyNum,
                PartCiaPercentage = coInsuranceAssignedDTO.PartCiaPercentage,
                ExpensesPercentage = coInsuranceAssignedDTO.ExpensesPercentage,
                InsuranceCompanyId = coInsuranceAssignedDTO.InsuranceCompanyId

            };
        }

        internal static List<CoInsuranceAssigned> CreateCoInsuranceAssigneds(List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs)
        {
            List<CoInsuranceAssigned> coInsuranceAssigneds = new List<CoInsuranceAssigned>();

            foreach (CoInsuranceAssignedDTO coInsuranceAssignedDTO in coInsuranceAssignedDTOs)
            {
                coInsuranceAssigneds.Add(CreateCoInsuranceAssigned(coInsuranceAssignedDTO));
            }

            return coInsuranceAssigneds;
        }

        internal static AccountingPaymentRequest CreateAccountingPaymentRequest(GLWKDTO.AccountingPaymentRequestDTO accountingPaymentRequestDTO)
        {
            return new AccountingPaymentRequest
            {
                Id = accountingPaymentRequestDTO.Id,
                Amount = accountingPaymentRequestDTO.Amount,
                ExchangeRate = accountingPaymentRequestDTO.ExchangeRate,
                PrefixId = accountingPaymentRequestDTO.PrefixId,
                BranchId = accountingPaymentRequestDTO.BranchId,
                IndividualId = accountingPaymentRequestDTO.IndividualId,
                Number = accountingPaymentRequestDTO.Number,
                AccountingDate = accountingPaymentRequestDTO.AccountingDate,
                UserId = accountingPaymentRequestDTO.UserId,
                Description = accountingPaymentRequestDTO.Description,
                CurrencyId = accountingPaymentRequestDTO.CurrencyId,
                TechnicalTransaction = accountingPaymentRequestDTO.TechnicalTransaction,
                AccountingAccountId = accountingPaymentRequestDTO.AccountingAccountId,
                AccountingAccountNumber = accountingPaymentRequestDTO.AccountingAccountNumber,
                AccountingNatureId = accountingPaymentRequestDTO.AccountingNatureId,
                SalePointId = accountingPaymentRequestDTO.SalePointId,
                BusinessTypeId = accountingPaymentRequestDTO.BusinessTypeId,
                PaymentSourceId = accountingPaymentRequestDTO.PaymentSourceId,
                SalvageId = accountingPaymentRequestDTO.SalvageId,
                RecoveryId = accountingPaymentRequestDTO.RecoveryId,
                Vouchers = accountingPaymentRequestDTO.Vouchers != null ? CreateAccountingVouchers(accountingPaymentRequestDTO.Vouchers) : new List<AccountingVoucher>(),
                CoInsuranceAssigneds = accountingPaymentRequestDTO.CoInsuranceAssigneds != null ? CreateAccountingCoInsuranceAssigneds(accountingPaymentRequestDTO.CoInsuranceAssigneds) : new List<AccountingCoInsuranceAssigned>()
            };
        }

        internal static List<AccountingVoucher> CreateAccountingVouchers(List<GLWKDTO.VoucherDTO> voucherDTOs)
        {
            List<AccountingVoucher> accountingVouchers = new List<AccountingVoucher>();

            foreach (GLWKDTO.VoucherDTO voucherDTO in voucherDTOs)
            {
                accountingVouchers.Add(CreateAccountingVoucher(voucherDTO));
            }

            return accountingVouchers;
        }

        internal static AccountingVoucher CreateAccountingVoucher(GLWKDTO.VoucherDTO voucherDTO)
        {
            return new AccountingVoucher
            {
                Concepts = voucherDTO.Concepts != null ? CreateAccountingConcepts(voucherDTO.Concepts) : new List<AccountingConcept>(),
                Id = voucherDTO.Id,
                CurrencyId = voucherDTO.CurrencyId,
                ExchangeRate = voucherDTO.ExchangeRate
            };
        }

        internal static List<AccountingConcept> CreateAccountingConcepts(List<GLWKDTO.ConceptDTO> conceptDTOs)
        {
            List<AccountingConcept> accountingConcepts = new List<AccountingConcept>();

            foreach (GLWKDTO.ConceptDTO conceptDTO in conceptDTOs)
            {
                accountingConcepts.Add(CreateAccountingConcept(conceptDTO));
            }

            return accountingConcepts;
        }

        internal static AccountingConcept CreateAccountingConcept(GLWKDTO.ConceptDTO conceptDTO)
        {
            return new AccountingConcept
            {
                Id = conceptDTO.Id,
                Description = conceptDTO.Description,
                Amount = conceptDTO.Amount,
                AccountingAccountId = conceptDTO.AccountingAccountId,
                AccountingAccountNumber = conceptDTO.AccountingAccountNumber,
                AccountingNatureId = conceptDTO.AccountingNatureId,
                Taxes = conceptDTO.Taxes != null ? CreateAccountingTaxes(conceptDTO.Taxes) : new List<AccountingTax>(),
            };
        }

        internal static List<AccountingTax> CreateAccountingTaxes(List<GLWKDTO.TaxDTO> taxDTOs)
        {
            List<AccountingTax> accountingTaxes = new List<AccountingTax>();

            foreach (GLWKDTO.TaxDTO taxDTO in taxDTOs)
            {
                accountingTaxes.Add(CreateAccountingTax(taxDTO));
            }

            return accountingTaxes;
        }

        internal static AccountingTax CreateAccountingTax(GLWKDTO.TaxDTO taxDTO)
        {
            return new AccountingTax
            {
                Id = taxDTO.Id,
                Description = taxDTO.Description,
                Amount = taxDTO.Amount,
                AccountingConceptId = taxDTO.AccountingConceptId,
                AccountingAccountId = taxDTO.AccountingAccountId,
                AccountingAccountNumber = taxDTO.AccountingAccountNumber,
                AccountingNatureId = taxDTO.AccountingNatureId
            };
        }

        internal static List<AccountingCoInsuranceAssigned> CreateAccountingCoInsuranceAssigneds(List<GLWKDTO.CoInsuranceAssignedDTO> coInsuranceAssignedDTOs)
        {
            List<AccountingCoInsuranceAssigned> accountingCoInsuranceAssigneds = new List<AccountingCoInsuranceAssigned>();

            foreach (GLWKDTO.CoInsuranceAssignedDTO coInsuranceAssignedDTO in coInsuranceAssignedDTOs)
            {
                accountingCoInsuranceAssigneds.Add(CreateAccountingCoInsuranceAssigned(coInsuranceAssignedDTO));
            }

            return accountingCoInsuranceAssigneds;
        }

        internal static AccountingCoInsuranceAssigned CreateAccountingCoInsuranceAssigned(GLWKDTO.CoInsuranceAssignedDTO coInsuranceAssignedDTO)
        {
            return new AccountingCoInsuranceAssigned
            {
                EndorsementId = coInsuranceAssignedDTO.EndorsementId,
                PolicyId = coInsuranceAssignedDTO.PolicyId,
                InsuranceCompanyId = coInsuranceAssignedDTO.InsuranceCompanyId,
                PartCiaPercentage = coInsuranceAssignedDTO.PartCiaPercentage,
                ExpensesPercentage = coInsuranceAssignedDTO.ExpensesPercentage,
                CompanyNum = coInsuranceAssignedDTO.CompanyNum,
                AccountingAccountId = coInsuranceAssignedDTO.AccountingAccountId,
                AccountingAccountNumber = coInsuranceAssignedDTO.AccountingAccountNumber,
                AccountingNatureId = coInsuranceAssignedDTO.AccountingNatureId
            };
        }

        #endregion

        #endregion

        #region Recovery

        #region EntityToModel

        internal static Recovery CreateRecovery(CLMEN.Recovery entityRecovery)
        {
            return new Recovery
            {
                Id = entityRecovery.RecoveryCode,
                RecoveryType = new RecoveryType
                {
                    Id = Convert.ToInt32(entityRecovery.RecoveryTypeCode)
                },
                CreatedDate = Convert.ToDateTime(entityRecovery.CreatedDate),
                Description = entityRecovery.Description,
                CancellationDate = entityRecovery.CancellationDate,
                Recuperator = new Recuperator
                {
                    Id = Convert.ToInt32(entityRecovery.RecuperatorId),
                },
                CancellationReason = new ClaimCancellationReason
                {
                    Id = Convert.ToInt32(entityRecovery.CancellationReasonCode),
                },
                PrescriptionDate = entityRecovery.PrescriptionDate,
                CompanyId = Convert.ToInt32(entityRecovery.CompanyCode),
                Voucher = entityRecovery.Voucher,
                RecoveryClassId = Convert.ToInt32(entityRecovery.RecoveryClassCode),
                LossResponsible = entityRecovery.LossResponsible,
                AssignedCourt = entityRecovery.AssignedCourt,
                ExpedientNumber = entityRecovery.ExpedientNumber,
                AttorneyAssingmentDate = entityRecovery.AttorneyAssignmentDate,
                LastReportDate = entityRecovery.LastReportDate,
                Debtor = entityRecovery.DebtorCode != null ? new Debtor
                {
                    Id = Convert.ToInt32(entityRecovery.DebtorCode),
                } : null,
                ClaimId = Convert.ToInt32(entityRecovery.ClaimCode),
                SubClaimId = Convert.ToInt32(entityRecovery.SubClaimCode),
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entityRecovery.BranchCode),
                },
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entityRecovery.PrefixCode),
                },
                ClaimNumber = Convert.ToInt32(entityRecovery.ClaimNumber),
                TotalAmmount = Convert.ToDecimal(entityRecovery.TotalAmount),
                PaymentPlan = new Models.PaymentRequest.PaymentPlan
                {
                    Id = Convert.ToInt32(entityRecovery.PaymentPlanCode)
                },
                DebtorIsParticipant = Convert.ToBoolean(entityRecovery.DebtorIsParticipant)
            };
        }

        internal static List<Recovery> CreateRecoveries(BusinessCollection businessCollection)
        {
            {
                List<Recovery> recoveries = new List<Recovery>();

                foreach (CLMEN.Recovery entityRecovery in businessCollection)
                {
                    recoveries.Add(CreateRecovery(entityRecovery));
                }
                return recoveries;
            }
        }

        internal static RecoveryType CreateRecoveyType(PARAMEN.RecoveryType entityRecoveryType)
        {
            return new RecoveryType
            {
                Id = entityRecoveryType.RecoveryTypeCode,
                Description = entityRecoveryType.Description
            };
        }

        internal static List<RecoveryType> CreateRecoveryTypes(BusinessCollection businessCollection)
        {
            {
                List<RecoveryType> recoveryTypes = new List<RecoveryType>();
                foreach (PARAMEN.RecoveryType entityRecoveryType in businessCollection)
                {
                    recoveryTypes.Add(CreateRecoveyType(entityRecoveryType));
                }
                return recoveryTypes;
            }
        }

        internal static Debtor CreateDebtorByParticipant(Participant participant)
        {
            if (participant == null)
            {
                return null;
            }

            return new Debtor
            {
                Id = participant.Id,
                DocumentNumber = participant.DocumentNumber,
                FullName = participant.Fullname,
                Address = participant.Address,
                Phone = participant.Phone
            };
        }

        internal static Participant CreateParticipantByDebtor(Debtor debtor)
        {
            return new Participant
            {
                Id = debtor.Id,
                DocumentNumber = debtor.DocumentNumber,
                Fullname = debtor.FullName,
                Address = debtor.Address,
                Phone = debtor.Phone
            };
        }


        internal static RecoveryControl CreateRecoveryControl(INTEN.ClmRecoveryControl entityCoTmpClmRecoveryControl)
        {
            return new RecoveryControl
            {
                Action = entityCoTmpClmRecoveryControl.Action,
                PaymentPlanId = Convert.ToInt32(entityCoTmpClmRecoveryControl.PaymentPlanCode),
                RecoveryId = entityCoTmpClmRecoveryControl.RecoveryCode
            };
        }
        #endregion

        #region DtoToModel

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
                    DebtorIsParticipant = recovery.DebtorIsParticipant
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

        internal static Recovery CreateRecovery(RecoveryDTO recoveryDTO)
        {
            return new Recovery
            {
                Id = recoveryDTO.Id,
                RecoveryType = new RecoveryType
                {
                    Id = recoveryDTO.RecoveryTypeId,
                },
                CreatedDate = Convert.ToDateTime(recoveryDTO.CreatedDate),
                Description = recoveryDTO.Description,
                CancellationDate = recoveryDTO.CancellationDate,
                Recuperator = new Recuperator
                {
                    Id = recoveryDTO.RecuperatorId,
                },
                DocumentationId = recoveryDTO.Documentation,
                CancellationReason = new ClaimCancellationReason
                {
                    Id = Convert.ToInt32(recoveryDTO.CancellationReasonId),
                },
                PrescriptionDate = recoveryDTO.PrescriptionDate,
                CompanyId = recoveryDTO.CompanyId,
                Voucher = recoveryDTO.Voucher,
                LossResponsible = recoveryDTO.LossResponsible,
                AssignedCourt = recoveryDTO.AssignedCourt,
                ExpedientNumber = recoveryDTO.ExpedientNumber,
                AttorneyAssingmentDate = recoveryDTO.AttorneyAssignmentDate,
                LastReportDate = recoveryDTO.LastReportDate,
                Debtor = new Debtor
                {
                    Id = Convert.ToInt32(recoveryDTO.DebtorId),
                    FullName = recoveryDTO.DebtorFullName,
                    DocumentNumber = recoveryDTO.DebtorDocumentNumber,
                    Address = recoveryDTO.DebtorAddress,
                    Phone = recoveryDTO.DebtorPhone
                },
                ClaimId = recoveryDTO.ClaimId,
                SubClaimId = recoveryDTO.SubClaimId,
                Prefix = new Prefix
                {
                    Id = recoveryDTO.PrefixId,
                },
                Branch = new Branch
                {
                    Id = recoveryDTO.BranchId,
                },
                ClaimNumber = recoveryDTO.ClaimNumber,
                TotalAmmount = recoveryDTO.TotalAmount,
                RecoveryClassId = recoveryDTO.RecoveryClassId,
                PaymentPlan = recoveryDTO.PaymentPlanId != null || (recoveryDTO.PaymentClassId != null && recoveryDTO.CurrencyId != null && recoveryDTO.TaxPercentage != null) ? new PaymentPlan
                {
                    Id = Convert.ToInt32(recoveryDTO.PaymentPlanId),
                    PaymentClass = new PaymentClass
                    {
                        Id = Convert.ToInt32(recoveryDTO.PaymentClassId)
                    },
                    Currency = new Currency
                    {
                        Id = Convert.ToInt32(recoveryDTO.CurrencyId)
                    },
                    Tax = Convert.ToDecimal(recoveryDTO.TaxPercentage),
                    PaymentQuotas = recoveryDTO.PaymentQuotas != null ? CreatePaymentQuotas(recoveryDTO.PaymentQuotas) : new List<PaymentQuota>()
                } : null,
                DebtorIsParticipant = recoveryDTO.DebtorIsParticipant
            };
        }
        internal static Participant CreateParticipant(RecoveryDTO recoveryDTO)
        {
            return new Participant
            {
                DocumentNumber = recoveryDTO.DebtorDocumentNumber,
                Fullname = recoveryDTO.DebtorFullName,
                Address = recoveryDTO.DebtorAddress,
                Phone = recoveryDTO.DebtorPhone
            };
        }
        #endregion

        #endregion

        #region Salvage

        #region EntityToModel

        internal static List<Salvage> CreateSalvages(BusinessCollection businessCollection)
        {
            List<Salvage> salvages = new List<Salvage>();

            foreach (CLMEN.Salvage entitySalvage in businessCollection)
            {
                salvages.Add(CreateSalvage(entitySalvage));
            }

            return salvages;
        }

        internal static Salvage CreateSalvage(CLMEN.Salvage entitySalvage)
        {
            return new Salvage
            {
                Id = entitySalvage.SalvageCode,
                Description = entitySalvage.Description,
                AssignmentDate = entitySalvage.AssignmentDate,
                Branch = new Branch
                {
                    Id = Convert.ToInt32(entitySalvage.BranchCode),
                    Description = entitySalvage.Description
                },
                ClaimId = Convert.ToInt32(entitySalvage.ClaimCode),
                ClaimNumber = Convert.ToInt32(entitySalvage.ClaimNumber),
                CreationDate = Convert.ToDateTime(entitySalvage.CreationDate),
                EndDate = entitySalvage.EndDate,
                EstimatedSale = Convert.ToDecimal(entitySalvage.EstimatedSale),
                Location = entitySalvage.Location,
                Observations = entitySalvage.Observation,
                Prefix = new Prefix
                {
                    Id = Convert.ToInt32(entitySalvage.PrefixCode)
                },
                SubClaimId = Convert.ToInt32(entitySalvage.SubclaimCode),
                UnitsQuantity = entitySalvage.UnitQuantity
            };
        }

        internal static List<Sale> CreateSales(BusinessCollection businessCollection)
        {
            List<Sale> sales = new List<Sale>();

            foreach (CLMEN.Sale entitySale in businessCollection)
            {
                sales.Add(CreateSale(entitySale));
            }

            return sales;
        }

        internal static Sale CreateSale(CLMEN.Sale entitySale)
        {
            return new Sale
            {
                Id = entitySale.SaleCode,
                CreationDate = Convert.ToDateTime(entitySale.Date),
                CancellationDate = entitySale.CancellationDate,
                Description = entitySale.Description,
                TotalAmount = Convert.ToDecimal(entitySale.TotalAmount),
                CancellationReason = new ClaimCancellationReason
                {
                    Id = Convert.ToInt32(entitySale.CancellationReasonCode)
                },
                SoldQuantity = Convert.ToInt32(entitySale.SoldQuantity),
                Buyer = new Buyer
                {
                    Id = Convert.ToInt32(entitySale.Buyer)
                },
                IsParticipant = entitySale.IsParticipant
            };
        }

        internal static PaymentClass CreatePaymentClass(COMMEN.PaymentClass entityPaymentClass)
        {
            return new PaymentClass
            {
                Id = entityPaymentClass.ClassPaymentCode,
                Description = entityPaymentClass.Description
            };
        }

        internal static List<PaymentClass> CreatePaymentClasses(BusinessCollection businessCollection)
        {
            List<PaymentClass> paymentClasses = new List<PaymentClass>();

            foreach (COMMEN.PaymentClass entityPaymentClass in businessCollection)
            {
                paymentClasses.Add(CreatePaymentClass(entityPaymentClass));
            }

            return paymentClasses;
        }


        internal static Buyer CreateBuyerByPerson(UPEN.Person entityPerson)
        {
            return new Buyer
            {
                Id = entityPerson.IndividualId,
                DocumentNumber = entityPerson.IdCardNo,
                FullName = entityPerson.Surname + " " + (string.IsNullOrEmpty(entityPerson.MotherLastName) ? "" : entityPerson.MotherLastName + " ") + entityPerson.Name
            };
        }

        internal static List<Buyer> CreateBuyersByPersons(BusinessCollection businessCollection)
        {
            List<Buyer> Buyers = new List<Buyer>();

            foreach (UPEN.Person entityPerson in businessCollection)
            {
                Buyers.Add(CreateBuyerByPerson(entityPerson));
            }

            return Buyers;
        }


        internal static Buyer CreateBuyerByCompany(UPEN.Company entityCompany)
        {
            return new Buyer
            {
                Id = entityCompany.IndividualId,
                DocumentNumber = entityCompany.TributaryIdNo,
                FullName = entityCompany.TradeName
            };
        }

        internal static List<Buyer> CreateBuyersByCompanies(BusinessCollection businessCollection)
        {
            List<Buyer> Buyers = new List<Buyer>();

            foreach (UPEN.Company entityCompany in businessCollection)
            {
                Buyers.Add(CreateBuyerByCompany(entityCompany));
            }

            return Buyers;
        }
        internal static Buyer CreateBuyerByParticipant(Participant participant)
        {
            if (participant == null)
            {
                return null;
            }

            return new Buyer
            {
                Id = participant.Id,
                DocumentNumber = participant.DocumentNumber,
                FullName = participant.Fullname,
                Address = participant.Address,
                Phone = participant.Phone
            };
        }

        internal static Buyer CreateBuyerByParticipant(CLMEN.Participant participant)
        {
            if (participant == null)
            {
                return null;
            }

            return new Buyer
            {
                Id = participant.ParticipantCode,
                DocumentNumber = participant.DocumentNumber,
                FullName = participant.Fullname,
                Address = participant.Address,
                Phone = participant.Phone
            };
        }

        internal static List<Buyer> CreateBuyersByParticipants(BusinessCollection businessCollection)
        {
            List<Buyer> Buyers = new List<Buyer>();

            foreach (CLMEN.Participant entityParticipant in businessCollection)
            {
                Buyers.Add(CreateBuyerByParticipant(entityParticipant));
            }

            return Buyers;
        }

        internal static Participant CreateParticipantByBuyer(Buyer buyer)
        {
            return new Participant
            {
                Id = buyer.Id,
                DocumentNumber = buyer.DocumentNumber,
                Fullname = buyer.FullName,
                Address = buyer.Address,
                Phone = buyer.Phone
            };
        }

        #endregion

        #region DtoToModel

        internal static Salvage CreateSalvage(SalvageDTO salvage)
        {
            return new Salvage
            {
                Id = Convert.ToInt32(salvage.Id),
                Branch = new Branch
                {
                    Id = salvage.BranchId
                },
                Prefix = new Prefix
                {
                    Id = salvage.PrefixId
                },
                SubClaimId = salvage.SubClaimId,
                ClaimId = salvage.ClaimId,
                ClaimNumber = salvage.ClaimNumber,
                Description = salvage.Description,
                AssignmentDate = salvage.AssignmentDate,
                CreationDate = salvage.CreationDate,
                EndDate = salvage.EndDate,
                Location = salvage.Location,
                Observations = salvage.Observations,
                EstimatedSale = salvage.EstimatedSale,
                UnitsQuantity = salvage.UnitsQuantity
            };
        }

        internal static Sale CreateSale(SaleDTO sale)
        {
            return new Sale
            {
                Id = sale.Id,
                CreationDate = DateTime.Now,
                CancellationDate = sale.CancellationDate,
                Description = sale.Description,
                TotalAmount = sale.TotalAmount,
                SoldQuantity = sale.QuantitySold,
                Buyer = new Buyer
                {
                    Id = sale.BuyerId,
                    FullName = sale.BuyerFullName,
                    DocumentNumber = sale.BuyerDocumentNumber,
                    Address = sale.BuyerAddress,
                    Phone = sale.BuyerPhone
                },
                CancellationReason = new ClaimCancellationReason
                {
                    Id = Convert.ToInt32(sale.CancellationReasonId)
                },
                PaymentPlan = new Models.PaymentRequest.PaymentPlan
                {
                    Id = Convert.ToInt32(sale.PaymentPlanId),
                    PaymentClass = new PaymentClass
                    {
                        Id = Convert.ToInt32(sale.PaymentClassId)
                    },
                    Currency = new Currency
                    {
                        Id = Convert.ToInt32(sale.CurrencyId)
                    },
                    Tax = Convert.ToDecimal(sale.Tax),
                    PaymentQuotas = sale.PaymentQuotas != null ? CreatePaymentQuotas(sale.PaymentQuotas) : new List<PaymentQuota>()
                },
                IsParticipant = sale.IsParticipant
            };
        }

        internal static PaymentQuota CreatePaymentQuota(PaymentQuotaDTO paymentQuota)
        {
            return new PaymentQuota
            {
                Id = paymentQuota.Id,
                Amount = paymentQuota.Amount,
                ExpirationDate = paymentQuota.ExpirationDate,
                Number = paymentQuota.Number
            };
        }

        internal static List<PaymentQuota> CreatePaymentQuotas(List<PaymentQuotaDTO> paymentQuotasDTO)
        {
            List<PaymentQuota> paymentQuotas = new List<PaymentQuota>();

            foreach (PaymentQuotaDTO paymentQuotaDTO in paymentQuotasDTO)
            {
                paymentQuotas.Add(CreatePaymentQuota(paymentQuotaDTO));
            }

            return paymentQuotas;
        }

        internal static Participant CreateParticipant(SaleDTO sale)
        {
            return new Participant
            {
                DocumentNumber = sale.BuyerDocumentNumber,
                Fullname = sale.BuyerFullName,
                Address = sale.BuyerAddress,
                Phone = sale.BuyerPhone
            };
        }





        #endregion

        #endregion
        internal static ClaimCoverageCoInsurance CreateClaimCoverageCoInsurance(CLMEN.ClaimCoverageCoinsurance claimCoverageCoinsurance)
        {
            return new ClaimCoverageCoInsurance
            {
                ClaimCoverageId = claimCoverageCoinsurance.ClaimCoverageCode,
                CoverageId = Convert.ToInt32(claimCoverageCoinsurance.CoverageCode),
                CompanyId = claimCoverageCoinsurance.CompanyCode,
                CurrencyId = Convert.ToInt32(claimCoverageCoinsurance.CurrencyCode),
                Date = claimCoverageCoinsurance.Date,
                DeducibleAmount = claimCoverageCoinsurance.DeductibleAmount,
                EstimationAmount = claimCoverageCoinsurance.EstimationAmount,
                EstimationAmountAccumulate = Convert.ToDecimal(claimCoverageCoinsurance.EstimateAmountAccumulate),
                EstimationTypeId = claimCoverageCoinsurance.EstimationTypeCode,
                EstimationTypeStatusId = Convert.ToInt32(claimCoverageCoinsurance.EstimationTypeStatusCode),
                EstimationTypeStatusReasonId = Convert.ToInt32(claimCoverageCoinsurance.EstimationTypeStatusReasonCode),
                VersionId = Convert.ToInt32(claimCoverageCoinsurance.VersionCode)
            };
        }

        internal static List<ClaimSearchPersonType> CreateClaimSearchPersonType(BusinessCollection claimSearchPersonType)
        {

            List<ClaimSearchPersonType> listSearchPersonType = new List<ClaimSearchPersonType>();
            foreach (CLMEN.ClaimSearchPersonType SearchPersonTypeEntity in claimSearchPersonType)
            {
                listSearchPersonType.Add(new ClaimSearchPersonType
                {
                    PersonTypeId = SearchPersonTypeEntity.PersonTypeCode,
                    PrefixId = SearchPersonTypeEntity.PrefixCode
                });
            }
            return listSearchPersonType;
        }
    }
}