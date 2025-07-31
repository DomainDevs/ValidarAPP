using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using ENM = Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Assemblers
{
    class ModelAssembler
    {

        /// <summary>
        /// Crea el objeto CompanyCoverage
        /// </summary>
        /// <param name="coverageDTO"></param>
        /// <returns>CompanyCoverage</returns>
        public static CompanyCoverage CreateCompanyCoverage(CoverageDTO Coverage)
        {
            if (Coverage == null)
            {
                return null;
            }
            ENM.EndorsementType type;
            if (Coverage.EndorsementType != null)
            {
                type = (ENM.EndorsementType)Coverage.EndorsementType;
            }



            CompanyCoverage comnpanycoverage = new CompanyCoverage
            {
                Id = (Coverage.CoverageId != 0) ? Coverage.CoverageId : Coverage.Id,
                CurrentFrom = Coverage.CurrentFrom,
                CurrentTo = Coverage.CurrentTo,
                LimitAmount = Coverage.LimitAmount,
                Description = Coverage.Description,
                DeclaredAmount = Coverage.DeclaredAmount,
                SubLimitAmount = Coverage.SubLimitAmount,
                MaxLiabilityAmount = Coverage.MaxLiabilityAmount,
                LimitOccurrenceAmount = Coverage.LimitOccurrenceAmount,
                LimitClaimantAmount = Coverage.LimitClaimantAmount,
                Rate = Coverage.Rate,
                RateType = (RateType)Coverage.RateTypeId,
                CalculationType = (CalculationType)Coverage.CalculationTypeId,
                PremiumAmount = Coverage.PremiumAmount,
                IsDeclarative = Coverage.IsDeclarative,
                IsMandatory = Coverage.IsMandatory,
                IsSelected = Coverage.IsSelected,
                CoverStatus = (Core.Application.UnderwritingServices.Enums.CoverageStatusType)Coverage.CoverStatus,
                Number = Coverage.Number,
                CoverStatusName = Coverage.CoverStatusName,
                SubLineBusiness = CreateSubLineBusiness(Coverage.SubLineBusiness),
                DepositPremiumPercent = Coverage.DepositPremiumPercent,
                IsMinPremiumDeposit = Coverage.IsMinPremiumDeposit,
                OriginalLimitAmount = Coverage.OriginalLimitAmount,
                OriginalSubLimitAmount = Coverage.OriginalSubLimitAmount,
                CurrentFromOriginal = Coverage.CurrentFromOriginal,
                CurrentToOriginal = Coverage.CurrentToOriginal,
                RuleSetId = Coverage.RuleSetId,
                PosRuleSetId = Coverage.PosRuleSetId,
                IsPrimary = Coverage.IsPrimary,
                MainCoverageId = Coverage.MainCoverageId,
                AllyCoverageId = Coverage.AllyCoverageId,
                CoverageAllied = Coverage.CoverageAllied,
                SublimitPercentage = Coverage.SublimitPercentage,
                InsuredObject = new CompanyInsuredObject
                {
                    Id = Coverage.InsuredObject.Id,
                    Description = Coverage.InsuredObject.Description,
                    Amount = Coverage.InsuredObject.InsuredLimitAmount,
                    IsMandatory = Coverage.InsuredObject.IsMandatory,
                    IsSelected = Coverage.InsuredObject.IsSelected,
                    Premium = Coverage.InsuredObject.PremiumAmount,
                },
                EndorsementLimitAmount = Coverage.LimitAmount,
                EndorsementSublimitAmount = Coverage.SubLimitAmount,
            };
            if (Coverage.Deductible != null)
            {
                Coverage.Deductible.Rate = Coverage.Rate;
                Coverage.Deductible.RateType = (RateType)Coverage.RateTypeId;
                comnpanycoverage.Deductible = CreateDeductible(Coverage.Deductible);
            }
            if (Coverage.EndorsementType != null)
            {
                comnpanycoverage.EndorsementType = (ENM.EndorsementType)Coverage.EndorsementType;
            }
            if (Coverage.Text != null)
            {
                comnpanycoverage.Text = CreateText(Coverage.Text);
            }
            if (Coverage.Clauses != null && Coverage.Clauses.Count > 0)
            {
                comnpanycoverage.Clauses = CreateClauses(Coverage.Clauses);
            }
            return comnpanycoverage;
        }

        public static CompanySubLineBusiness CreateSubLineBusiness(SubLineBusiness subLineBusiness)
        {
            if (subLineBusiness == null)
            {
                return null;
            }
            return new CompanySubLineBusiness
            {
                Id = subLineBusiness.Id,
                Description = subLineBusiness.Description,
                ExtendedProperties = subLineBusiness.ExtendedProperties,
                LineBusinessDescription = subLineBusiness.LineBusinessDescription,
                LineBusinessId = subLineBusiness.LineBusinessId,
                SmallDescription = subLineBusiness.SmallDescription,
                Status = subLineBusiness.Status,
                LineBusiness = new CompanyLineBusiness
                {
                    Description = subLineBusiness.LineBusiness.Description,
                    Status = subLineBusiness.LineBusiness.Status,
                    ExtendedProperties = subLineBusiness.LineBusiness.ExtendedProperties,
                    Id = subLineBusiness.LineBusiness.Id,
                    IdLineBusinessbyRiskType = subLineBusiness.LineBusiness.IdLineBusinessbyRiskType,
                    ListInsurectObjects = subLineBusiness.LineBusiness.ListInsurectObjects,
                    ReportLineBusiness = subLineBusiness.LineBusiness.ReportLineBusiness,
                    ShortDescription = subLineBusiness.LineBusiness.ShortDescription,
                    TyniDescription = subLineBusiness.LineBusiness.TyniDescription
                }
            };
        }


        /// <summary>
        /// Convierte un listado de CoverageDTO a un conjunto de CompanyCoverage
        /// </summary>
        /// <param name="coverages">Listado de modelo CoverageDTO</param>
        /// <returns>Listado de cobeturas</returns>
        public static List<CompanyCoverage> CreateCompanyCoverages(List<CoverageDTO> coverages)
        {
            List<CompanyCoverage> coverageDTO = new List<CompanyCoverage>();

            foreach (var coverage in coverages)
            {
                coverageDTO.Add(CreateCompanyCoverage(coverage));
            }
            return coverageDTO;
        }

        /// <summary>
        /// Crea un TransportType a partir de un identificador
        /// </summary>
        /// <param name="id">Identificador del transport type</param>
        /// <returns>Modelo TransportType</returns>
        public static TransportType CreateTransportType(int id)
        {
            return new TransportType
            {
                Id = id
            };
        }

        /// <summary>
        /// Covierte un listado de identificadores en modelos TransportType
        /// </summary>
        /// <param name="ids">Listado de identificadores</param>
        /// <returns>Un listado de identificadores</returns>
        public static List<TransportType> CreateTransportTypes(List<int> ids)
        {
            List<TransportType> transportTypes = new List<TransportType>();
            foreach (var id in ids)
            {
                transportTypes.Add(CreateTransportType(id));
            }
            return transportTypes;
        }

        /// <summary>
        /// Crear un company risk a partir de un TransportDTO
        /// </summary>
        /// <param name="transportDTO">Objeto origen</param>
        /// <returns>Modelo CompanyRisk</returns>
        public static CompanyRisk CreateCompanyRisk(TransportDTO transportDTO, CompanyRisk companyRisk)
        {
            if (companyRisk == null)
            {
                companyRisk = new CompanyRisk();
            }

            companyRisk.Id = transportDTO.Id.GetValueOrDefault();
            companyRisk.RiskId = transportDTO.RiskId.GetValueOrDefault();
            companyRisk.Description = transportDTO.Description;
            companyRisk.Policy = new CompanyPolicy
            {
                Id = transportDTO.PolicyId,
            };

            companyRisk.Policy.PolicyType = new CompanyPolicyType
            {
                IsFloating = transportDTO.IsFloating
            };
            companyRisk.GroupCoverage = new GroupCoverage
            {
                Id = transportDTO.CoverageGroupId
            };
            companyRisk.MainInsured = new CompanyIssuanceInsured
            {
                BirthDate = transportDTO.BirthDate,
                Gender = transportDTO.Gender,
                IdentificationDocument = new IssuanceIdentificationDocument
                {
                    Number = transportDTO.DocumentNumber,
                    DocumentType = new IssuanceDocumentType
                    {
                        Id = transportDTO.DocumentType,
                        Description = transportDTO.DocumentTypeDescription,
                        SmallDescription = transportDTO.DocumentTypeSmallDescription
                    }                  
                },
                Name = transportDTO.Name,
                CompanyName = new IssuanceCompanyName
                {
                    TradeName = transportDTO.CompanyName,
                    NameNum = transportDTO.NameNum,
                    Address = new IssuanceAddress { Description = transportDTO.Address },
                    Phone = new IssuancePhone { Description = transportDTO.Phone },
                    Email = new IssuanceEmail { Description = transportDTO.Email },
                    IsMain = transportDTO.IsMain
                },
                IndividualId = transportDTO.IndividualId,
                IndividualType = (IndividualType)transportDTO.IndividualType,
                CustomerType = (CustomerType)transportDTO.CustomerType,
                CustomerTypeDescription = transportDTO.CustomerTypeDescription,
                Profile = transportDTO.Profile,
                ScoreCredit = new ScoreCredit
                {
                    ScoreCreditId = transportDTO.ScoreCredit.GetValueOrDefault()
                }
            };
            companyRisk.CoveredRiskType = CoveredRiskType.Transport;
            companyRisk.Status = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original;
            companyRisk.OriginalStatus = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original;
            companyRisk.IsPersisted = true;
            companyRisk.RiskActivity = new CompanyRiskActivity
            {
                Id = Convert.ToInt32(transportDTO.RiskCommercialClassId),
                Description = transportDTO.RiskCommercialClassDescription
            };
            companyRisk.IsFacultative = transportDTO.IsFacultative;

            return companyRisk;
        }

        public static CompanyTransport CreateTransport(TransportDTO transportDTO, CompanyTransport companyTransport)
        {
            if (transportDTO == null)
            {
                return null;
            }
            else
            {
                companyTransport.Risk.IsRetention = transportDTO.IsRetention;
                companyTransport.Risk = CreateCompanyRisk(transportDTO, companyTransport.Risk);
                companyTransport.AdjustPeriod = (transportDTO.BillingPeriodId == null) ? new AdjustPeriod() : new AdjustPeriod { Id = (int)transportDTO.BillingPeriodId, Description = transportDTO.BillingDescription };
                companyTransport.PackagingType = (transportDTO.TransportPackagingId == null) ? new PackagingType() : new PackagingType { Id = (int)transportDTO.TransportPackagingId, Description = transportDTO.PackagingDescription };
                companyTransport.DeclarationPeriod = (transportDTO.DeclarationPeriodId == null) ? new DeclarationPeriod() : new DeclarationPeriod { Id = (int)transportDTO.DeclarationPeriodId, Description = transportDTO.DeclarationDescription };
                companyTransport.CargoType = (transportDTO.CargoTypeId == null) ? new CargoType() : new CargoType { Id = (int)transportDTO.CargoTypeId, Description = transportDTO.CargoDescription };
                companyTransport.ViaType = (transportDTO.ViaId == null) ? new ViaType() : new ViaType { Id = (int)transportDTO.ViaId, Description = transportDTO.ViaDescription };
                companyTransport.CityFrom = (transportDTO.FromCityId == null) ? new City() : new City
                {
                    Id = (int)transportDTO.FromCityId,
                    State = (transportDTO.FromStateId == null) ? new State() : new State
                    {
                        Id = (int)transportDTO.FromStateId,
                        Country = (transportDTO.FromCountryId == null) ? new Country() : new Country
                        {
                            Id = (int)transportDTO.FromCountryId
                        }
                    }
                };
                companyTransport.CityTo = (transportDTO.ToCityId == null) ? new City() : new City
                {
                    Id = (int)transportDTO.ToCityId,
                    State = (transportDTO.ToStateId == null) ? new State() : new State
                    {
                        Id = (int)transportDTO.ToStateId,
                        Country = (transportDTO.ToCountryId == null) ? new Country() : new Country
                        {
                            Id = (int)transportDTO.ToCountryId
                        }
                    }

                };
                companyTransport.Destiny = transportDTO.Target;
                companyTransport.Source = transportDTO.Source;
                companyTransport.MinimumPremium = transportDTO.MinimumPremium;
                companyTransport.LimitMaxReleaseAmount = (decimal)transportDTO.LimitMaxRealeaseAmount.GetValueOrDefault();
                companyTransport.ReleaseAmount = Convert.ToDecimal(transportDTO.ReleaseAmount);
                companyTransport.FreightAmount = (decimal)transportDTO.FreightAmount.GetValueOrDefault();
                companyTransport.Types = CreateTransportTypes(transportDTO.TransportTypeIds);
                companyTransport.InsuredObjects = CreateInsuredObjects(transportDTO.InsuredObjects);
                companyTransport.HolderType = new CompanyHolderType
                {
                    Id = Convert.ToInt32(transportDTO.HolderTypeId),
                };
            }

            return companyTransport;
        }
        /// <summary>
        /// Crea lista de objetos del seguro
        /// </summary>
        /// <param name="insuredObjectDTOs"></param>
        /// <returns></returns>
        public static List<CompanyInsuredObject> CreateInsuredObjects(List<InsuredObjectDTO> insuredObjectDTOs)
        {

            List<CompanyInsuredObject> companyInsuredObjects = new List<CompanyInsuredObject>();

            foreach (var insuredObject in insuredObjectDTOs)
            {
                companyInsuredObjects.Add(CreateInsuredObject(insuredObject));
            }
            return companyInsuredObjects;
        }
        /// <summary>
        /// Crea objeto del seguro
        /// </summary>
        /// <param name="insuredObjectDTO"></param>
        /// <returns></returns>
        public static CompanyInsuredObject CreateInsuredObject(InsuredObjectDTO insuredObjectDTO)
        {

            if (insuredObjectDTO == null)
            {
                return null;
            }
            return new CompanyInsuredObject
            {
                Id = insuredObjectDTO.Id,
                Description = insuredObjectDTO.Description,
                IsSelected = insuredObjectDTO.IsSelected,
                IsMandatory = insuredObjectDTO.IsMandatory,
                Amount = insuredObjectDTO.InsuredLimitAmount,
                Premium = insuredObjectDTO.PremiumAmount,
                Rate = insuredObjectDTO.Rate,
                DepositPremiunPercent = insuredObjectDTO.DepositPremiumPercentage

            };
        }

        /// <summary>
        /// Crea lista de beneficiarios
        /// </summary>
        /// <param name="beneficiaryDTOs"></param>
        /// <returns></returns>
        public static List<CompanyBeneficiary> CreateBeneficiaries(List<BeneficiaryDTO> beneficiaryDTOs)
        {
            List<CompanyBeneficiary> companyBeneficiaries = new List<CompanyBeneficiary>();

            foreach (var beneficiary in beneficiaryDTOs)
            {
                companyBeneficiaries.Add(CreateBeneficiary(beneficiary));
            }
            return companyBeneficiaries;
        }

        /// <summary>
        /// Crea Beneficiario
        /// </summary>
        /// <param name="beneficiaryDTO"></param>
        /// <returns></returns>
        public static CompanyBeneficiary CreateBeneficiary(BeneficiaryDTO beneficiaryDTO)
        {
            if (beneficiaryDTO == null)
                return null;
            else
            {
                return new CompanyBeneficiary
                {
                    IndividualId = beneficiaryDTO.IndividualId,
                    BeneficiaryTypeDescription = beneficiaryDTO.Description
                };
            }
        }

        /// <summary>
        /// Creación de la lista de clausulas desde CompanyClause
        /// </summary>
        /// <param name="clausesDTOS"></param>
        /// <returns></returns>
        public static List<CompanyClause> CreateClauses(List<ClauseDTO> clausesDTOS)
        {
            List<CompanyClause> companyClauses = new List<CompanyClause>();

            foreach (var clause in clausesDTOS)
            {
                companyClauses.Add(CreateClause(clause));
            }
            return companyClauses;
        }

        /// <summary>
        /// Ensamblado de las clausulas unica
        /// </summary>
        /// <param name="clauseDTO"></param>
        /// <returns></returns>
        public static CompanyClause CreateClause(ClauseDTO clauseDTO)
        {
            return new CompanyClause
            {
                Id = clauseDTO.Id,
                Text = clauseDTO.Text,
                Name = clauseDTO.Name,
                IsMandatory = clauseDTO.IsMandatory,
                Title = clauseDTO.Title,
                ConditionLevel = clauseDTO.ConditionLevel
            };
        }

        /// <summary>
        /// Se crea el ensamblado de texto unico
        /// </summary>
        /// <param name="textDTO"></param>
        /// <returns></returns>
        public static CompanyText CreateText(TextDTO textDTO)
        {
            return new CompanyText
            {
                Id = textDTO.Id,
                TextTitle = textDTO.TextTitle,
                TextBody = textDTO.TextBody,
                Observations = textDTO.Observations
            };
        }

        public static CompanyDeductible CreateDeductible(DeductibleDTO deductibleDTO)
        {
            return new CompanyDeductible
            {
                Id = deductibleDTO.Id,
                Description = deductibleDTO.Description,
                AccDeductAmt = deductibleDTO.AccDeductAmt,
                Currency = deductibleDTO.Currency != null ? new Currency
                {
                    Id = deductibleDTO.Currency != null ? deductibleDTO.Currency.Id : 0,
                    Description = deductibleDTO.Currency.Description ?? "",
                    SmallDescription = deductibleDTO.Currency.SmallDescription ?? "",
                    TinyDescription = deductibleDTO.Currency.TinyDescription ?? "",
                } : null,
                DeductibleSubject = new DeductibleSubject
                {
                    Id = deductibleDTO.DeductibleSubject.Id,
                    Description = deductibleDTO.DeductibleSubject.Description
                },
                DeductibleUnit = new DeductibleUnit
                {
                    Id = deductibleDTO.DeductibleUnit.Id,
                    Description = deductibleDTO.DeductibleUnit.Description
                },
                DeductPremiumAmount = deductibleDTO.DeductPremiumAmount,
                DeductValue = deductibleDTO.DeductValue,
                IsDefault = deductibleDTO.IsDefault,
                Rate = deductibleDTO.Rate,
                RateType = (RateType)deductibleDTO.RateType,
                MaxDeductValue = deductibleDTO.MaxDeductValue,
                MinDeductValue = deductibleDTO.MinDeductValue,
                MaxDeductibleSubject = deductibleDTO.MaxDeductibleSubject != null ? new DeductibleSubject
                {
                    Id = deductibleDTO.MaxDeductibleSubject.Id,
                    Description = deductibleDTO.MaxDeductibleSubject.Description
                } : null,
                MaxDeductibleUnit = deductibleDTO.MaxDeductibleUnit != null ? new DeductibleUnit
                {
                    Id = deductibleDTO.MaxDeductibleUnit.Id,
                    Description = deductibleDTO.MaxDeductibleUnit.Description
                } : null,
                MinDeductibleSubject = new DeductibleSubject
                {
                    Id = deductibleDTO.MinDeductibleSubject.Id,
                    Description = deductibleDTO.MinDeductibleSubject.Description
                },
                MinDeductibleUnit = new DeductibleUnit
                {
                    Id = deductibleDTO.MinDeductibleUnit.Id,
                    Description = deductibleDTO.MinDeductibleUnit.Description
                }
            };
        }

        public static IMapper CreateMapDeductible()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Deductible, CompanyDeductible>();
            });
            return config.CreateMapper();
        }

        public static CompanyEndorsementPeriod CreateCompanyEndorsementPeriod(EndorsementPeriod endorsementPeriod)
        {
            if (endorsementPeriod == null)
            {
                return null;
            }
            return new CompanyEndorsementPeriod()
            {
                Id = endorsementPeriod.Id,
                PolicyId = endorsementPeriod.PolicyId,
                CurrentFrom = endorsementPeriod.CurrentFrom,
                CurrentTo = endorsementPeriod.CurrentTo,
                AdjustPeriod = endorsementPeriod.AdjustPeriod,
                DeclarationPeriod = endorsementPeriod.DeclarationPeriod,
                TotalAdjustment = endorsementPeriod.TotalAdjust,
                TotalDeclarations = endorsementPeriod.TotalDeclaration,
                Version = endorsementPeriod.Version
            };
        }
    }
}
