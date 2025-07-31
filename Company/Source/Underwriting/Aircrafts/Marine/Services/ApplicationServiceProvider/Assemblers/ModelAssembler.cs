using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Resources;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;

namespace Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Assemblers
{
    class ModelAssembler
    {

        /// <summary>
        /// Crea el objeto CompanyCoverage
        /// </summary>
        /// <param name="coverageDTO"></param>
        /// <returns>CompanyCoverage</returns>
        public static CompanyCoverage CreateCoverage(CoverageDTO coverage)
        {
            if (coverage == null)
            {
                return null;
            }
            return new CompanyCoverage
            {
                Id = coverage.CoverageId,
                CurrentFrom = coverage.CurrentFrom,
                CurrentTo = coverage.CurrentTo,
                LimitAmount = coverage.LimitAmount,
                Description = coverage.Description,
                DeclaredAmount = coverage.DeclaredAmount,
                SubLimitAmount = coverage.SubLimitAmount,
                MaxLiabilityAmount = coverage.MaxLiabilityAmount,
                LimitOccurrenceAmount = coverage.LimitOccurrenceAmount,
                LimitClaimantAmount = coverage.LimitClaimantAmount,
                Rate = coverage.Rate,
                RateType = (RateType)coverage.RateTypeId,
                CalculationType = (CalculationType)coverage.CalculationTypeId,
                PremiumAmount = coverage.PremiumAmount,
                IsDeclarative = coverage.IsDeclarative,
                IsMandatory = coverage.IsMandatory,
                IsSelected = coverage.IsSelected,
                CoverStatus = (Core.Application.UnderwritingServices.Enums.CoverageStatusType)coverage.CoverStatus,
                Number = coverage.Number,
                CoverStatusName = ((Core.Application.UnderwritingServices.Enums.CoverageStatusType)coverage.CoverStatus).ToString(),
                SubLineBusiness = CreateSubLineBusiness(coverage.SubLineBusiness),
                DepositPremiumPercent = coverage.DepositPremiumPercent,
                IsMinPremiumDeposit = coverage.IsMinPremiumDeposit,
                OriginalLimitAmount = coverage.OriginalLimitAmount,
                OriginalSubLimitAmount = coverage.OriginalSubLimitAmount,
                CurrentFromOriginal = coverage.CurrentFromOriginal,
                CurrentToOriginal = coverage.CurrentToOriginal,
                InsuredObject = new CompanyInsuredObject
                {
                    Id = coverage.InsuredObject.Id,
                    Description = coverage.InsuredObject.Description,
                    Amount = coverage.InsuredObject.InsuredLimitAmount,
                    IsMandatory = coverage.InsuredObject.IsMandatory,
                    IsSelected = coverage.InsuredObject.IsSelected,
                    Premium = coverage.InsuredObject.PremiumAmount
                }
            };
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
        public static List<CompanyCoverage> CreateCoverages(List<CoverageDTO> coverages)
        {
            List<CompanyCoverage> coverageDTO = new List<CompanyCoverage>();

            foreach (var coverage in coverages)
            {
                coverageDTO.Add(CreateCoverage(coverage));
            }
            return coverageDTO;
        }

        /// <summary>
        /// Crea un MarineType a partir de un identificador
        /// </summary>
        /// <param name="id">Identificador del Marine type</param>
        /// <returns>Modelo MarineType</returns>
        public static MarineType CreateMarineType(int id)
        {
            return new MarineType
            {
                Id = id
            };
        }

        /// <summary>
        /// Covierte un listado de identificadores en modelos MarineType
        /// </summary>
        /// <param name="ids">Listado de identificadores</param>
        /// <returns>Un listado de identificadores</returns>
        public static List<MarineType> CreateMarineTypes(List<int> ids)
        {
            List<MarineType> MarineTypes = new List<MarineType>();
            foreach (var id in ids)
            {
                MarineTypes.Add(CreateMarineType(id));
            }
            return MarineTypes;
        }

        /// <summary>
        /// Crear un company risk a partir de un MarineDTO
        /// </summary>
        /// <param name="MarineDTO">Objeto origen</param>
        /// <returns>Modelo CompanyRisk</returns>
        public static CompanyRisk CreateCompanyRisk(MarineDTO MarineDTO, CompanyRisk companyRisk)
        {
            if (companyRisk == null)
            {
                companyRisk = new CompanyRisk();
            }

            companyRisk.Id = MarineDTO.Id.GetValueOrDefault();
            companyRisk.RiskId = MarineDTO.RiskId.GetValueOrDefault();
            companyRisk.Description = MarineDTO.Description;
            companyRisk.Policy = new CompanyPolicy
            {
                Id = MarineDTO.PolicyId,
            };
            companyRisk.GroupCoverage = new GroupCoverage
            {
                Id = MarineDTO.CoverageGroupId
            };
            companyRisk.MainInsured = new CompanyIssuanceInsured
            {
                BirthDate = MarineDTO.BirthDate,
                Gender = MarineDTO.Gender,
                IdentificationDocument = new IssuanceIdentificationDocument
                {
                    Number = MarineDTO.DocumentNumber,
                    DocumentType = new IssuanceDocumentType
                    {
                        Id = MarineDTO.DocumentType,
                        Description = MarineDTO.DocumentTypeDescription,
                        SmallDescription = MarineDTO.DocumentTypeSmallDescription
                    },
                    //ExpeditionDate = MarineDTO.DocumentExpedition
                },
                Name = MarineDTO.Name,
                CompanyName = new IssuanceCompanyName
                {
                    TradeName = MarineDTO.CompanyName,
                    NameNum = MarineDTO.NameNum,
                    Address = new IssuanceAddress { Description = MarineDTO.Address },
                    Phone = new IssuancePhone { Description = MarineDTO.Phone },
                    Email = new IssuanceEmail { Description = MarineDTO.Email },
                    IsMain = MarineDTO.IsMain
                },
                IndividualId = MarineDTO.IndividualId,
                IndividualType = (IndividualType)MarineDTO.IndividualType,
                CustomerType = (CustomerType)MarineDTO.CustomerType,
                CustomerTypeDescription = MarineDTO.CustomerTypeDescription,
                Profile = MarineDTO.Profile,
                ScoreCredit = new ScoreCredit
                {
                    ScoreCreditId = MarineDTO.ScoreCredit.GetValueOrDefault()
                }
            };
            companyRisk.CoveredRiskType = CoveredRiskType.Aircraft;
            companyRisk.Status = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original;
            companyRisk.OriginalStatus = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original;
            companyRisk.IsPersisted = true;

            return companyRisk;
        }

        /// <summary>
        /// Crear un company risk a partir de un MarineDTO
        /// </summary>
        /// <param name="marineDTO">Objeto origen</param>
        /// <returns>Modelo CompanyRisk</returns>
        public static CompanyRisk CreateCompanyRisk(MarineDTO marineDTO)
        {
            if (marineDTO == null)
            {
                return null;
            }

            return new CompanyRisk
            {
                Id = marineDTO.Id.GetValueOrDefault(),
                RiskId = marineDTO.RiskId.GetValueOrDefault(),
                Description = marineDTO.Description,
                Policy = new CompanyPolicy
                {
                    Id = marineDTO.PolicyId,
                },
                GroupCoverage = new GroupCoverage
                {
                    Id = marineDTO.CoverageGroupId
                },
                MainInsured = new CompanyIssuanceInsured
                {
                    BirthDate = marineDTO.BirthDate,
                    Gender = marineDTO.Gender,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = marineDTO.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = marineDTO.DocumentType,
                            Description = marineDTO.DocumentTypeDescription,
                            SmallDescription = marineDTO.DocumentTypeSmallDescription
                        },
                        //ExpeditionDate = marineDTO.DocumentExpedition
                    },
                    Name = marineDTO.Name,
                    CompanyName = new IssuanceCompanyName
                    {
                        TradeName = marineDTO.CompanyName,
                        NameNum = marineDTO.NameNum,
                        Address = new IssuanceAddress { Description = marineDTO.Address },
                        Phone = new IssuancePhone { Description = marineDTO.Phone },
                        Email = new IssuanceEmail { Description = marineDTO.Email },
                        IsMain = marineDTO.IsMain
                    },
                    IndividualId = marineDTO.IndividualId,
                    IndividualType = (IndividualType)marineDTO.IndividualType,
                    CustomerType = (CustomerType)marineDTO.CustomerType,
                    CustomerTypeDescription = marineDTO.CustomerTypeDescription,
                    Profile = marineDTO.Profile,
                    ScoreCredit = new ScoreCredit
                    {
                        ScoreCreditId = marineDTO.ScoreCredit.GetValueOrDefault()
                    }
                },
                CoveredRiskType = CoveredRiskType.Aircraft,
                Status = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original,
                OriginalStatus = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original,
                IsPersisted = true
            };
        }


        public static CompanyMarine CreateMarine(MarineDTO marineDTO)
        {
            if (marineDTO == null)
            {
                return null;
            }
            else
            {
                return new CompanyMarine
                {
                    Risk = CreateCompanyRisk(marineDTO),
                    UseId = marineDTO.UseId,
                    UseDescription = marineDTO.UseDescription,
                    BoatName = marineDTO.BoatName,
                    CurrentManufacturing = marineDTO.YearManufacturing,

                    InsuredObjects = CreateInsuredObjects(marineDTO.InsuredObjects)
                };
            }
        }
        /// <summary>
        /// Crea lista de objetos del seguro
        /// </summary>
        /// <param name="insuredObjectDTOs"></param>
        /// <returns></returns>
        public static List<InsuredObject> CreateInsuredObjects(List<InsuredObjectDTO> insuredObjectDTOs)
        {

            List<InsuredObject> companyInsuredObjects = new List<InsuredObject>();

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
        public static InsuredObject CreateInsuredObject(InsuredObjectDTO insuredObjectDTO)
        {

            if (insuredObjectDTO == null)
            {
                return null;
            }
            return new InsuredObject
            {
                Id = insuredObjectDTO.Id,
                Description = insuredObjectDTO.Description,
                IsSelected = insuredObjectDTO.IsSelected,
                IsMandatory = insuredObjectDTO.IsMandatory,
                Amount = insuredObjectDTO.InsuredLimitAmount,
                Premium = insuredObjectDTO.PremiumAmount
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
                    IndividualId = beneficiaryDTO.Id,
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
    }
}
