using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Assemblers
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
        /// Crea un AircraftType a partir de un identificador
        /// </summary>
        /// <param name="id">Identificador del Aircraft type</param>
        /// <returns>Modelo AircraftType</returns>
        public static AircraftType CreateAircraftType(int id)
        {
            return new AircraftType
            {
                Id = id
            };
        }

        /// <summary>
        /// Covierte un listado de identificadores en modelos AircraftType
        /// </summary>
        /// <param name="ids">Listado de identificadores</param>
        /// <returns>Un listado de identificadores</returns>
        public static List<AircraftType> CreateAircraftTypes(List<int> ids)
        {
            List<AircraftType> AircraftTypes = new List<AircraftType>();
            foreach (var id in ids)
            {
                AircraftTypes.Add(CreateAircraftType(id));
            }
            return AircraftTypes;
        }

        /// <summary>
        /// Crear un company risk a partir de un AircraftDTO
        /// </summary>
        /// <param name="aircraftDTO">Objeto origen</param>
        /// <returns>Modelo CompanyRisk</returns>
        public static CompanyRisk CreateCompanyRisk(AircraftDTO aircraftDTO)
        {
            if (aircraftDTO == null)
            {
                return null;
            }

            return new CompanyRisk
            {
                Id = aircraftDTO.Id.GetValueOrDefault(),
                RiskId = aircraftDTO.RiskId.GetValueOrDefault(),
                Description = aircraftDTO.Description,
                Policy = new CompanyPolicy
                {
                    Id = aircraftDTO.PolicyId,
                },
                GroupCoverage = new GroupCoverage
                {
                    Id = aircraftDTO.CoverageGroupId
                },
                MainInsured = new CompanyIssuanceInsured
                {
                    BirthDate = aircraftDTO.BirthDate,
                    Gender = aircraftDTO.Gender,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = aircraftDTO.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = aircraftDTO.DocumentType,
                            Description = aircraftDTO.DocumentTypeDescription,
                            SmallDescription = aircraftDTO.DocumentTypeSmallDescription
                        },
                        //ExpeditionDate = aircraftDTO.DocumentExpedition
                    },
                    Name = aircraftDTO.Name,
                    CompanyName = new IssuanceCompanyName
                    {
                        TradeName = aircraftDTO.CompanyName,
                        NameNum = aircraftDTO.NameNum,
                        Address = new IssuanceAddress { Description = aircraftDTO.Address },
                        Phone = new IssuancePhone { Description = aircraftDTO.Phone },
                        Email = new IssuanceEmail { Description = aircraftDTO.Email },
                        IsMain = aircraftDTO.IsMain
                    },
                    IndividualId = aircraftDTO.IndividualId,
                    IndividualType = (IndividualType)aircraftDTO.IndividualType,
                    CustomerType = (CustomerType)aircraftDTO.CustomerType,
                    CustomerTypeDescription = aircraftDTO.CustomerTypeDescription,
                    Profile = aircraftDTO.Profile,
                    ScoreCredit = new ScoreCredit
                    {
                        ScoreCreditId = aircraftDTO.ScoreCredit.GetValueOrDefault()
                    }
                },
                CoveredRiskType = CoveredRiskType.Aircraft,
                Status = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original,
                OriginalStatus = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original,
                IsPersisted = true
            };
        }

        public static CompanyAircraft CreateAircraft(AircraftDTO aircraftDTO)
        {
            if (aircraftDTO == null)
            {
                return null;
            }
            else
            {
                return new CompanyAircraft
                {
                    Risk = CreateCompanyRisk(aircraftDTO),
                    MakeId = (int)aircraftDTO.MakeId,
                    ModelId = (int)aircraftDTO.ModelId,
                    TypeId = (int)aircraftDTO.TypeId,
                    UseId = (int)aircraftDTO.UseId,
                    OperatorId = (int)aircraftDTO.OperatorId,
                    CurrentManufacturing = (int)aircraftDTO.CurrentManufacturing,
                    RegisterId = (int)aircraftDTO.RegisterId,
                    NumberRegister = aircraftDTO.NumberRegister,
                    Model = CreateCompanyModel(aircraftDTO),
                    Make = CreateCompanyMake(aircraftDTO),

                    InsuredObjects = CreateInsuredObjects(aircraftDTO.InsuredObjects)
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

        /// <summary>
        /// Obtiene un objeto CompanyModel a partir del riesgo
        /// </summary>
        /// <param name="aircraftDTO">Riesgo de tipo Aviación</param>
        /// <returns>Modelo</returns>
        public static CompanyModel CreateCompanyModel(AircraftDTO aircraftDTO)
        {
            if (aircraftDTO == null)
                return null;

            return new CompanyModel
            {
                Id = (int)aircraftDTO.ModelId,
                Description = aircraftDTO.ModelDescription
            };
        }

        /// <summary>
        /// Obtiene la marca a partir del Riesgo
        /// </summary>
        /// <param name="aircraftDTO">Riesgo de tipo aviación</param>
        /// <returns>Marca</returns>
        public static CompanyMake CreateCompanyMake(AircraftDTO aircraftDTO)
        {
            if (aircraftDTO == null)
                return null;

            return new CompanyMake
            {
                Id = (int)aircraftDTO.MakeId,
                Description = aircraftDTO.MakeDescription
            };
        }
    }
}
