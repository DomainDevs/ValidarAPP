using System.Collections.Generic;
using Sistran.Company.Application.OperationQuotaServices.DTOs;
using Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using AQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Assemblers
{
    public static class DTOAssembler
    {
        internal static AgentProgramDTO CreateAgentProgram(AQMOD.AgentProgram agentProgram)
        {
            return new AgentProgramDTO
            {
                Id = agentProgram.Id,
                Description = agentProgram.Description,
                SmallDescription = agentProgram.SmallDescription,
                Enabled = agentProgram.Enabled
            };
        }

        public static List<AgentProgramDTO> CreateAgentPrograms(List<AQMOD.AgentProgram> programs)
        {
            List<AgentProgramDTO> agentProgramDTOs = new List<AgentProgramDTO>();

            foreach (AQMOD.AgentProgram AgentProgram in programs)
            {
                agentProgramDTOs.Add(CreateAgentProgram(AgentProgram));
            }

            return agentProgramDTOs;
        }

        public static List<UtilityDetailsDTO> CreateUtilitiesDetails(List<AQMOD.UtilityDetails> utilityDetails)
        {
            List<UtilityDetailsDTO> utilityDetailsDTOs = new List<UtilityDetailsDTO>();

            foreach (AQMOD.UtilityDetails utilityDetail in utilityDetails)
            {
                utilityDetailsDTOs.Add(CreateUtilitiesDetail(utilityDetail));
            }

            return utilityDetailsDTOs;
        }

        public static UtilityDetailsDTO CreateUtilitiesDetail(AQMOD.UtilityDetails utilityDetails)
        {
            return new UtilityDetailsDTO
            {
                Id = utilityDetails.Id,
                Description = utilityDetails.Description,
                FormUtilitys = utilityDetails.FormUtilitys,
                Enabled = utilityDetails.Enabled,
                UtilitysTypeCd = utilityDetails.UtilitysTypeCd,
                UtilitysSummaryCd = utilityDetails.UtilitysSummaryCd,
                UtilityId = utilityDetails.UtilityId
            };
        }

        public static List<IndicatorConceptDTO> CreateindicatorConcepts(List<AQMOD.IndicatorConcept> indicatorConcepts)
        {
            List<IndicatorConceptDTO> indicatorConceptDTOs = new List<IndicatorConceptDTO>();

            foreach (AQMOD.IndicatorConcept indicatorConcept in indicatorConcepts)
            {
                indicatorConceptDTOs.Add(CreateindicatorConcept(indicatorConcept));
            }

            return indicatorConceptDTOs;
        }

        internal static IndicatorConceptDTO CreateindicatorConcept(AQMOD.IndicatorConcept indicatorConcept)
        {
            return new IndicatorConceptDTO
            {
                Id = indicatorConcept.Id,
                Description = indicatorConcept.Description,
                IndicatorType = indicatorConcept.Indicatortype,
                Enabled = indicatorConcept.Enabled
            };
        }

        public static List<ReportListSisconcDTO> CreateReportListSisconc(List<AQMOD.ReportListSisconc> reportListSisconcs)
        {
            List<ReportListSisconcDTO> reportListSisconcDTOs = new List<ReportListSisconcDTO>();

            foreach (AQMOD.ReportListSisconc reportListSisconc in reportListSisconcs)
            {
                reportListSisconcDTOs.Add(CreatereportListSisconc(reportListSisconc));
            }

            return reportListSisconcDTOs;
        }

        internal static ReportListSisconcDTO CreatereportListSisconc(AQMOD.ReportListSisconc reportListSisconc)
        {
            return new ReportListSisconcDTO
            {
                Id = reportListSisconc.Id,
                Description = reportListSisconc.Description,
                SmallDescription = reportListSisconc.SmallDescription,
                Enabled = reportListSisconc.Enabled
            };
        }

        public static List<RiskCenterDTO> CreateRiskCenterList(List<AQMOD.RiskCenter> riskCenters)
        {
            List<RiskCenterDTO> riskCenterDTOs = new List<RiskCenterDTO>();

            foreach (AQMOD.RiskCenter riskCenter in riskCenters)
            {
                riskCenterDTOs.Add(CreateriskCenter(riskCenter));
            }

            return riskCenterDTOs;
        }

        public static List<PromissoryNoteSignatureDTO> CreatePromissoryNoteSignatures(List<AQMOD.PromissoryNoteSignature> promissoryNoteSignatures)
        {
            List<PromissoryNoteSignatureDTO> promissoryNoteSignatureDTOs = new List<PromissoryNoteSignatureDTO>();

            foreach (AQMOD.PromissoryNoteSignature promissoryNoteSignature in promissoryNoteSignatures)
            {
                promissoryNoteSignatureDTOs.Add(CreaterpromissoryNoteSignature(promissoryNoteSignature));
            }

            return promissoryNoteSignatureDTOs;
        }

        internal static List<AutomaticQuotaOperationDTO> CreateAutomaticQuotaOperations(List<AutomaticQuotaOperation> automaticQuotaOperations)
        {
            List<AutomaticQuotaOperationDTO> automaticQuotaOperationDTOs = new List<AutomaticQuotaOperationDTO>();

            foreach (AQMOD.AutomaticQuotaOperation automaticQuotaOperation in automaticQuotaOperations)
            {
                automaticQuotaOperationDTOs.Add(CreateAutomaticQuotaOperation(automaticQuotaOperation));
            }

            return automaticQuotaOperationDTOs;
        }

        public static AutomaticQuotaOperationDTO CreateAutomaticQuotaOperation(AQMOD.AutomaticQuotaOperation automaticQuotaOperation)
        {
            return new AutomaticQuotaOperationDTO
            {
                Id = automaticQuotaOperation.Id,
                ParentId = automaticQuotaOperation?.ParentId,
                AutomaticOperationType = automaticQuotaOperation.AutomaticOperationType,
                User = automaticQuotaOperation?.User,
                CreationDate = automaticQuotaOperation.CreationDate,
                ModificationDate = automaticQuotaOperation?.ModificationDate,
                Operation = automaticQuotaOperation.Operation
            };
        }

        internal static PromissoryNoteSignatureDTO CreaterpromissoryNoteSignature(PromissoryNoteSignature promissoryNoteSignature)
        {
            return new PromissoryNoteSignatureDTO
            {
                Id = promissoryNoteSignature.Id,
                Description = promissoryNoteSignature.Description,
                SmallDescrption = promissoryNoteSignature.SmallDescrption,
                Enabled = promissoryNoteSignature.Enabled
            };
        }

        internal static RiskCenterDTO CreateriskCenter(AQMOD.RiskCenter riskCenter)
        {
            return new RiskCenterDTO
            {
                Id = riskCenter.Id,
                Description = riskCenter.Description,
                SmallDescription = riskCenter.SmallDescription,
                Enabled = riskCenter.Enabled
            };
        }

        public static List<RestrictiveDTO> CreateRestrictiveList(List<AQMOD.Restrictive> restrictives)
        {
            List<RestrictiveDTO> restrictiveDTOs = new List<RestrictiveDTO>();

            foreach (AQMOD.Restrictive restrictive in restrictives)
            {
                restrictiveDTOs.Add(Createrestrictive(restrictive));
            }

            return restrictiveDTOs;
        }

        internal static RestrictiveDTO Createrestrictive(AQMOD.Restrictive restrictive)
        {
            return new RestrictiveDTO
            {
                Id = restrictive.Id,
                Description = restrictive.Description,
                SmallDescription = restrictive.SmallDescription,
                Enabled = restrictive.Enabled
            };
        }

        public static List<AutomaticQuotaDTO> CreateAutomaticQuotas(List<AQMOD.AutomaticQuota> automaticQuotas)
        {
            List<AutomaticQuotaDTO> automaticQuotasDTOs = new List<AutomaticQuotaDTO>();

            foreach (AQMOD.AutomaticQuota automaticQuota in automaticQuotas)
            {
                automaticQuotasDTOs.Add(CreateautomaticQuota(automaticQuota));
            }

            return automaticQuotasDTOs;
        }

        internal static AutomaticQuotaDTO CreateautomaticQuota(AQMOD.AutomaticQuota automaticQuota)
        {
            AutomaticQuotaDTO automaticQuotaDto = new AutomaticQuotaDTO
            {
                DynamicProperties = automaticQuota.DynamicProperties,
                AutomaticQuotaId = automaticQuota.AutomaticQuotaId,
                CollateralStatus = automaticQuota.CollateralStatus,
                CurrentCluster = automaticQuota.CurrentCluster,
                CurrentQuota = automaticQuota.CurrentQuota,
                ElaboratedId = automaticQuota.ElaboratedId,
                ElaboratedName = automaticQuota.ElaboratedName,
                InfringementPolicies = automaticQuota.InfringementPolicies,
                LegalizedQuota = automaticQuota.LegalizedQuota,
                Observations = automaticQuota.Observations,
                IndividualId = automaticQuota.IndividualId,
                CustomerTpeId = automaticQuota.CustomerTypeId,
                QuotaPreparationDate = automaticQuota.QuotaPreparationDate,
                QuotaReConsideration = automaticQuota.QuotaReconsideration,
                RequestedById = automaticQuota.RequestedById,
                RequestedByName = automaticQuota.RequestedByName,
                SuggestedQuota = automaticQuota.SuggestedQuota,
                TypeCollateral = automaticQuota.TypeCollateral,
                AverageRevenuePercentage = automaticQuota?.AverageRevenuePercentage,
                EquityPercentage = automaticQuota?.EquityPercentage,
                OperatingIncomePercentage = automaticQuota?.OperatingIncomePercentage,
                ShareCapitalPercentage = automaticQuota?.ShareCapitalPercentage,
                ScaleWeighted = automaticQuota?.ScaleWeighted,
                ScoreCustomerVSMarket = automaticQuota?.ScoreCustomerVSMarket,
                QuotaA = automaticQuota?.QuotaA,
                QuotaB = automaticQuota?.QuotaB,
                ScoreObjectiveCriteria = automaticQuota?.ScoreObjectiveCriteria,
                ScoreSubjectiveCriteria = automaticQuota?.ScoreSubjectiveCriteria,
                SubjectiveWeightedScore = automaticQuota?.SubjectiveWeightedScore,
                TargetWeightedScore = automaticQuota?.TargetWeightedScore,
                CountryId = automaticQuota.CountryId,
                StateId = automaticQuota.StateId,
                CityId = automaticQuota.CityId,
                Agent = new AgentDTO
                {
                    Id = automaticQuota.Agent.Id,
                    Description = automaticQuota.Agent.Description
                },
                AgentProgramDTO = new AgentProgramDTO
                {
                    Id = automaticQuota.AgentProgram.Id,
                    Description = automaticQuota.AgentProgram.Description,
                    SmallDescription = automaticQuota.AgentProgram.SmallDescription,
                    Enabled = automaticQuota.AgentProgram.Enabled
                },
                ProspecDTO = new ProspectDTO
                {
                    AdditionalInfo = automaticQuota.Prospect.AdditionalInfo,
                    Address = automaticQuota.Prospect.Address,
                    AddressTypeCd = automaticQuota.Prospect.AddressTypecd,
                    BusinessName = automaticQuota.Prospect.Businessname,
                    City = automaticQuota.Prospect.City,
                    CompanytypeCd = automaticQuota.Prospect.CompanytypeCd,
                    ConstitutionDate = automaticQuota.Prospect.ConstitutionDate,
                    CountryCd = automaticQuota.Prospect.CountryCd,
                    DocumentNumber = automaticQuota.Prospect.DocumentNumber,
                    DocumentType = automaticQuota.Prospect.DocumentType,
                    EconomicActivity = automaticQuota.Prospect.EconomicActivity,
                    Email = automaticQuota.Prospect.Email,
                    FiscalReviewer = automaticQuota.Prospect.FiscalReviewer,
                    IndividualTypeCd = automaticQuota.Prospect.IndividualTypeCd,
                    LegalRepresentative = automaticQuota.Prospect.LegalRepresentative,
                    Phone = automaticQuota.Prospect.Phone,
                    StateCd = automaticQuota.Prospect.StateCd,
                    IndividualId = automaticQuota.Prospect.IndividualId
                },
                AcidTest = new BaseDTO
                {
                    Value = automaticQuota.AcidTest?.Value,
                    Qualification = automaticQuota.AcidTest?.Qualification,
                    Weighted = automaticQuota.AcidTest?.Weighted,
                    Score = automaticQuota.AcidTest?.Score
                },
                ActiveAverage = new BaseDTO
                {
                    Value = automaticQuota.ActiveAverage?.Value,
                    Qualification = automaticQuota.ActiveAverage?.Qualification,
                    Weighted = automaticQuota.ActiveAverage?.Weighted,
                    Score = automaticQuota.ActiveAverage?.Score
                },
                AverageEquity = new BaseDTO
                {
                    Value = automaticQuota.AverageEquity?.Value,
                    Qualification = automaticQuota.AverageEquity?.Qualification,
                    Weighted = automaticQuota.AverageEquity?.Weighted,
                    Score = automaticQuota.AverageEquity?.Score
                },
                AverageNetIncome = new BaseDTO
                {
                    Value = automaticQuota.AverageNetIncome?.Value,
                    Qualification = automaticQuota.AverageNetIncome?.Qualification,
                    Weighted = automaticQuota.AverageNetIncome?.Weighted,
                    Score = automaticQuota.AverageNetIncome?.Score
                },
                AverageSales = new BaseDTO
                {
                    Value = automaticQuota.AverageSales?.Value,
                    Qualification = automaticQuota.AverageSales?.Qualification,
                    Weighted = automaticQuota.AverageSales?.Weighted,
                    Score = automaticQuota.AverageSales?.Score
                },
                AverageUtility = new BaseDTO
                {
                    Value = automaticQuota.AverageUtility?.Value,
                    Qualification = automaticQuota.AverageUtility?.Qualification,
                    Weighted = automaticQuota.AverageUtility?.Weighted,
                    Score = automaticQuota.AverageUtility?.Score
                },
                Capacity = new BaseDTO
                {
                    Value = automaticQuota.Capacity?.Value,
                    Qualification = automaticQuota.Capacity?.Qualification,
                    Weighted = automaticQuota.Capacity?.Weighted,
                    Score = automaticQuota.Capacity?.Score
                },
                CorporateGovernance = new BaseDTO
                {
                    Value = automaticQuota.CorporateGovernance?.Value,
                    Qualification = automaticQuota.CorporateGovernance?.Qualification,
                    Weighted = automaticQuota.CorporateGovernance?.Weighted,
                    Score = automaticQuota.CorporateGovernance?.Score
                },
                CurrentReason = new BaseDTO
                {
                    Value = automaticQuota.CurrentReason?.Value,
                    Qualification = automaticQuota.CurrentReason?.Qualification,
                    Weighted = automaticQuota.CurrentReason?.Weighted,
                    Score = automaticQuota.CurrentReason?.Score
                },
                CustomerKnowledgeAccess = new BaseDTO
                {
                    Value = automaticQuota.CustomerKnowledgeAccess?.Value,
                    Qualification = automaticQuota.CustomerKnowledgeAccess?.Qualification,
                    Weighted = automaticQuota.CustomerKnowledgeAccess?.Weighted,
                    Score = automaticQuota.CustomerKnowledgeAccess?.Score
                },
                CustomerReputation = new BaseDTO
                {
                    Value = automaticQuota.CustomerReputation?.Value,
                    Qualification = automaticQuota.CustomerReputation?.Qualification,
                    Weighted = automaticQuota.CustomerReputation?.Weighted,
                    Score = automaticQuota.CustomerReputation?.Score
                },
                EquityVariation = new BaseDTO
                {
                    Value = automaticQuota.EquityVariation?.Value,
                    Qualification = automaticQuota.EquityVariation?.Qualification,
                    Weighted = automaticQuota.EquityVariation?.Weighted,
                    Score = automaticQuota.EquityVariation?.Score
                },
                Etibda = new BaseDTO
                {
                    Value = automaticQuota.Etibda?.Value,
                    Qualification = automaticQuota.Etibda?.Qualification,
                    Weighted = automaticQuota.Etibda?.Weighted,
                    Score = automaticQuota.Etibda?.Score
                },
                Experience = new BaseDTO
                {
                    Value = automaticQuota.Experience?.Value,
                    Qualification = automaticQuota.Experience?.Qualification,
                    Weighted = automaticQuota.Experience?.Weighted,
                    Score = automaticQuota.Experience?.Score
                },
                Indebtedness = new BaseDTO
                {
                    Value = automaticQuota.Indebtedness?.Value,
                    Qualification = automaticQuota.Indebtedness?.Qualification,
                    Weighted = automaticQuota.Indebtedness?.Weighted,
                    Score = automaticQuota.Indebtedness?.Score
                },
                KnowledgeClient = new BaseDTO
                {
                    Value = automaticQuota.KnowledgeClient?.Value,
                    Qualification = automaticQuota.KnowledgeClient?.Qualification,
                    Weighted = automaticQuota.KnowledgeClient?.Weighted,
                    Score = automaticQuota.KnowledgeClient?.Score
                },
                MoralSolvency = new BaseDTO
                {
                    Value = automaticQuota.MoralSolvency?.Value,
                    Qualification = automaticQuota.MoralSolvency?.Qualification,
                    Weighted = automaticQuota.MoralSolvency?.Weighted,
                    Score = automaticQuota.MoralSolvency?.Score
                },
                NetIncomeVariation = new BaseDTO
                {
                    Value = automaticQuota.NetIncomeVariation?.Value,
                    Qualification = automaticQuota.NetIncomeVariation?.Qualification,
                    Weighted = automaticQuota.NetIncomeVariation?.Weighted,
                    Score = automaticQuota.NetIncomeVariation?.Score
                },
                RestrictiveList = new BaseDTO
                {
                    Value = automaticQuota.RestrictiveList?.Value,
                    Qualification = automaticQuota.RestrictiveList?.Qualification,
                    Weighted = automaticQuota.RestrictiveList?.Weighted,
                    Score = automaticQuota.RestrictiveList?.Score
                },
                RiskCenter = new BaseDTO
                {
                    Value = automaticQuota.RiskCenter?.Value,
                    Qualification = automaticQuota.RiskCenter?.Qualification,
                    Weighted = automaticQuota.RiskCenter?.Weighted,
                    Score = automaticQuota.RiskCenter?.Score
                },
                SalesGrowth = new BaseDTO
                {
                    Value = automaticQuota.SalesGrowth?.Value,
                    Qualification = automaticQuota.SalesGrowth?.Qualification,
                    Weighted = automaticQuota.SalesGrowth?.Weighted,
                    Score = automaticQuota.SalesGrowth?.Score
                },
                SignatureOrLetter = new BaseDTO
                {
                    Value = automaticQuota.SignatureOrLetter?.Value,
                    Qualification = automaticQuota.SignatureOrLetter?.Qualification,
                    Weighted = automaticQuota.SignatureOrLetter?.Weighted,
                    Score = automaticQuota.SignatureOrLetter?.Score
                },

            };
            if (automaticQuota.Third != null)
            {
                automaticQuotaDto.ThirdDTO = CreateCompanyThird(automaticQuota.Third);
            }
            if (automaticQuota.Utility != null)
            {
                automaticQuotaDto.UtilityDTO = CreateCompanyUtilities(automaticQuota.Utility);
            }
            if (automaticQuota.Indicator != null)
            {
                automaticQuotaDto.indicatorDTO = CreateCompanyIndicators(automaticQuota.Indicator);
            }

            return automaticQuotaDto;
        }


        public static ThirdDTO CreateCompanyThird(AQMOD.Third third)
        {
            return new ThirdDTO
            {
                Id = third.Id,
                CifinQuery = third.CifinQuery,
                Cosigner = third.Cosigner,
                PrincipalDebtor = third.PrincipalDebtor,
                Total = third.Total,
                InfringementPolicies = third.InfringementPolicies,
                PromissoryNoteSignatureDTO = new PromissoryNoteSignatureDTO
                {
                    Id = third.PromissoryNoteSignature.Id,
                    Description = third.PromissoryNoteSignature.Description,
                    SmallDescrption = third.PromissoryNoteSignature.SmallDescrption,
                    Enabled = third.PromissoryNoteSignature.Enabled
                },
                ReportListSisconcDTO = new ReportListSisconcDTO
                {
                    Id = third.ReportListSisconc.Id,
                    Description = third.ReportListSisconc.Description,
                    SmallDescription = third.ReportListSisconc.SmallDescription,
                    Enabled = third.ReportListSisconc.Enabled
                },
                RestrictiveDTO = new RestrictiveDTO
                {
                    Id = third.Restrictive.Id,
                    Description = third.Restrictive.Description,
                    SmallDescription = third.Restrictive.SmallDescription,
                    Enabled = third.Restrictive.Enabled
                },
                RiskCenterDTO = new RiskCenterDTO
                {
                    Id = third.RiskCenter.Id,
                    Description = third.RiskCenter.Description,
                    SmallDescription = third.RiskCenter.SmallDescription,
                    Enabled = third.RiskCenter.Enabled
                }

            };
        }
        public static List<UtilityDTO> CreateCompanyUtilities(List<AQMOD.Utility> utilities)
        {
            List<UtilityDTO> utilityDTOs = new List<UtilityDTO>();

            foreach (AQMOD.Utility utility in utilities)
            {
                utilityDTOs.Add(CreateCompanyUtility(utility));
            }

            return utilityDTOs;
        }

        public static UtilityDTO CreateCompanyUtility(AQMOD.Utility utility)
        {
            return new UtilityDTO
            {
                Id = utility.Id,
                Description = utility.Description,
                Start_Values = utility.Start_Values,
                End_value = utility.End_value,
                Var_Abs = utility.Var_Abs,
                Var_Relativa = utility.Var_Relativa,
                UtilityDetails = new UtilityDetailsDTO
                {
                    Id = utility.UtilityDetails.Id,
                    FormUtilitys = utility.UtilityDetails.FormUtilitys,
                    UtilitysTypeCd = utility.UtilityDetails.UtilitysTypeCd,
                    UtilitysSummaryCd = utility.UtilityDetails.UtilitysSummaryCd,
                    UtilityId = utility.UtilityDetails.UtilityId

                }
            };
        }

        public static List<IndicatorDTO> CreateCompanyIndicators(List<AQMOD.Indicator> indicators)
        {
            List<IndicatorDTO> indicatorDTOs = new List<IndicatorDTO>();

            foreach (AQMOD.Indicator indicator in indicators)
            {
                indicatorDTOs.Add(CreateCompanyIndicator(indicator));
            }

            return indicatorDTOs;
        }

        public static IndicatorDTO CreateCompanyIndicator(AQMOD.Indicator indicator)
        {
            return new IndicatorDTO
            {
                ConceptIndicatorCd = indicator.ConceptIndicatorcd,
                IndicatorIni = indicator.IndicatorIni,
                IndicatorFin = indicator.IndicatorFin,
                Observation = indicator.Observation,
                TypeIndicatorCd = indicator.TypeIndicatorcd,
                Description = indicator.Description


            };
        }
    }
}