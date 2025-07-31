using System.Collections.Generic;
using OQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Framework.DAF;
using AQENT = Sistran.Company.Application.AutomaticQuota.Entities;
using UPENT = Sistran.Company.Application.UniquePerson.Entities;
using UPCOENT = Sistran.Core.Application.UniquePersonV1.Entities;
using PAENT = Sistran.Company.Application.Parameters.Entities;
using System;
using Sistran.Company.Application.OperationQuotaServices.DTOs;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.RulesEngine;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Assemblers

{
    public static class ModelAssembler
    {
        public static List<OQMOD.AgentProgram> CreateAgentPrograms(BusinessCollection businessObjects)
        {
            List<OQMOD.AgentProgram> agentPrograms = new List<OQMOD.AgentProgram>();

            foreach (UPENT.AgentProgram AgentProgram in businessObjects)
            {
                agentPrograms.Add(CreateAgentProgram(AgentProgram));
            }

            return agentPrograms;
        }

        private static OQMOD.AgentProgram CreateAgentProgram(UPENT.AgentProgram agentProgram)
        {
            return new OQMOD.AgentProgram
            {
                Id = agentProgram.AgentProgramCode,
                SmallDescription = agentProgram.SmallDescription,
                Description = agentProgram.Description,
                Enabled = agentProgram.Enabled

            };
        }

        public static List<OQMOD.UtilityDetails> CreateUtilitiesDetails(BusinessCollection businessObjects)
        {
            List<OQMOD.UtilityDetails> utilities = new List<OQMOD.UtilityDetails>();

            foreach (PAENT.UtilityDetails utilitiesdetail in businessObjects)
            {
                utilities.Add(CreateUtilitiesDetail(utilitiesdetail));
            }

            return utilities;
        }

        public static OQMOD.UtilityDetails CreateUtilitiesDetail(PAENT.UtilityDetails utilityDetails)
        {
            return new OQMOD.UtilityDetails
            {
                Id = utilityDetails.UtilityDetailsCode,
                Description = utilityDetails.Description,
                Enabled = utilityDetails.Enabled,
                FormUtilitys = utilityDetails.FormUtilitysCode,
                //UtilityId = utilityDetails.UtilityDetailsCode,
                UtilitysSummaryCd = Convert.ToInt32(utilityDetails.UtilitySummaryCode),
                UtilitysTypeCd = Convert.ToInt32(utilityDetails.UtilityTypeCode)

            };
        }
        public static List<OQMOD.UtilitySummary> CreateUtilitiesDetailSummaries(BusinessCollection businessObjects)
        {
            List<OQMOD.UtilitySummary> utilitySummaries = new List<OQMOD.UtilitySummary>();

            foreach (PAENT.UtilitySummary utilitySummary in businessObjects)
            {
                utilitySummaries.Add(CreateUtilitiesDetailSummary(utilitySummary));
            }

            return utilitySummaries;
        }

        public static OQMOD.UtilitySummary CreateUtilitiesDetailSummary(PAENT.UtilitySummary utilitySummary)
        {
            return new OQMOD.UtilitySummary
            {
                Id = utilitySummary.UtilitySummaryCode,
                Description = utilitySummary.Description,
                Enabled = utilitySummary.Enabled,
            };
        }
        public static List<OQMOD.UtilityType> CreateUtilitiesDetailTypes(BusinessCollection businessObjects)
        {
            List<OQMOD.UtilityType> utilityTypes = new List<OQMOD.UtilityType>();

            foreach (PAENT.UtilityType utilityType in businessObjects)
            {
                utilityTypes.Add(CreateUtilitiesDetailType(utilityType));
            }

            return utilityTypes;
        }

        public static OQMOD.UtilityType CreateUtilitiesDetailType(PAENT.UtilityType utilityType)
        {
            return new OQMOD.UtilityType
            {
                Id = utilityType.UtilityTypeCode,
                Description = utilityType.Description,
                Enabled = utilityType.Enabled

            };
        }
        public static List<OQMOD.IndicatorConcept> CreateindicatorConcepts(BusinessCollection businessObjects)
        {
            List<OQMOD.IndicatorConcept> indicatorConcepts = new List<OQMOD.IndicatorConcept>();

            foreach (PAENT.IndicatorConcept indicatorConcept in businessObjects)
            {
                indicatorConcepts.Add(CreateindicatorConcept(indicatorConcept));
            }

            return indicatorConcepts;
        }

        private static OQMOD.IndicatorConcept CreateindicatorConcept(PAENT.IndicatorConcept indicatorConcept)
        {
            return new OQMOD.IndicatorConcept
            {
                Id = indicatorConcept.IndicatorConceptCode,
                Indicatortype = indicatorConcept.IndicatorTypeCode,
                Description = indicatorConcept.Description,
                Enabled = indicatorConcept.Enabled

            };
        }

        internal static List<OQMOD.RiskCenter> CreateRiskCenterList(BusinessCollection businessObjects)
        {
            List<OQMOD.RiskCenter> riskCenters = new List<OQMOD.RiskCenter>();

            foreach (PAENT.RiskCenterList riskCenterList in businessObjects)
            {
                riskCenters.Add(CreateriskCenter(riskCenterList));
            }

            return riskCenters;
        }
        private static OQMOD.RiskCenter CreateriskCenter(PAENT.RiskCenterList riskCenterList)
        {
            return new OQMOD.RiskCenter
            {
                Id = riskCenterList.RiskCenterListCode,
                SmallDescription = riskCenterList.SmallDescription,
                Description = riskCenterList.Description,
                Enabled = riskCenterList.Enabled

            };
        }

        internal static List<OQMOD.Restrictive> CreateRestrictiveList(BusinessCollection businessObjects)
        {
            List<OQMOD.Restrictive> restrictives = new List<OQMOD.Restrictive>();

            foreach (PAENT.RestrictiveList restrictiveList in businessObjects)
            {
                restrictives.Add(CreateRestrictiveList(restrictiveList));
            }

            return restrictives;
        }

        private static OQMOD.Restrictive CreateRestrictiveList(PAENT.RestrictiveList restrictiveList)
        {
            return new OQMOD.Restrictive
            {
                Id = restrictiveList.RestrictiveListCode,
                SmallDescription = restrictiveList.SmallDescription,
                Description = restrictiveList.Description,
                Enabled = restrictiveList.Enabled

            };
        }

        internal static List<OQMOD.ReportListSisconc> CreateReportListSisconc(BusinessCollection businessObjects)
        {
            List<OQMOD.ReportListSisconc> reportListSisconcs = new List<OQMOD.ReportListSisconc>();

            foreach (PAENT.ReportListSisconc reportListSisconc in businessObjects)
            {
                reportListSisconcs.Add(CreatereportSisconcs(reportListSisconc));
            }

            return reportListSisconcs;
        }

        private static OQMOD.ReportListSisconc CreatereportSisconcs(PAENT.ReportListSisconc reportListSisconc)
        {
            return new OQMOD.ReportListSisconc
            {
                Id = reportListSisconc.ReportListSisconcCode,
                SmallDescription = reportListSisconc.SmallDescription,
                Description = reportListSisconc.Description,
                Enabled = reportListSisconc.Enabled

            };
        }

        internal static List<OQMOD.PromissoryNoteSignature> CreatePromissoryNoteSignatures(BusinessCollection businessObjects)
        {
            List<OQMOD.PromissoryNoteSignature> promissoryNoteSignatures = new List<OQMOD.PromissoryNoteSignature>();

            foreach (PAENT.PromissoryNoteSignature promissoryNoteSignature in businessObjects)
            {
                promissoryNoteSignatures.Add(CreatePromissoryNoteSignature(promissoryNoteSignature));
            }

            return promissoryNoteSignatures;
        }

        private static OQMOD.PromissoryNoteSignature CreatePromissoryNoteSignature(PAENT.PromissoryNoteSignature promissoryNoteSignature)
        {
            return new OQMOD.PromissoryNoteSignature
            {
                Id = promissoryNoteSignature.PromissoryNoteSignatureCode,
                SmallDescrption = promissoryNoteSignature.SmallDescrption,
                Description = promissoryNoteSignature.Description,
                Enabled = promissoryNoteSignature.Enabled

            };
        }

        public static List<OQMOD.AutomaticQuotaOperation> CreateAutomaticQuotaOperations(BusinessCollection businessObjects)
        {
            List<OQMOD.AutomaticQuotaOperation> automaticQuotaOperations = new List<OQMOD.AutomaticQuotaOperation>();

            foreach (AQENT.AutomaticQuotaOperation automaticQuotaOperation in businessObjects)
            {
                automaticQuotaOperations.Add(CreateAutomaticQuotaOperation(automaticQuotaOperation));
            }

            return automaticQuotaOperations;
        }

        public static List<OQMOD.Third> CreateThirds(BusinessCollection businessObjects)
        {
            List<OQMOD.Third> thirds = new List<OQMOD.Third>();

            foreach (AQENT.Third third in businessObjects)
            {
                thirds.Add(CreateThird(third));
            }

            return thirds;
        }

        public static List<OQMOD.Utility> CreateUtilities(BusinessCollection businessObjects)
        {
            List<OQMOD.Utility> utilities = new List<OQMOD.Utility>();

            foreach (AQENT.Utility utility in businessObjects)
            {
                utilities.Add(CreateUtility(utility));
            }

            return utilities;
        }

        private static OQMOD.Utility CreateUtility(AQENT.Utility utility)
        {
            return new OQMOD.Utility
            {

            };
        }

        internal static List<OQMOD.Indicator> CreateIndicators(BusinessCollection businessObjects)
        {
            List<OQMOD.Indicator> indicators = new List<OQMOD.Indicator>();

            foreach (AQENT.Indicator indicator in businessObjects)
            {
                indicators.Add(CreateIndicator(indicator));
            }

            return indicators;
        }

        private static OQMOD.Indicator CreateIndicator(AQENT.Indicator indicator)
        {
            return new OQMOD.Indicator
            {

            };
        }

        internal static List<OQMOD.Prospect> CreateProspects(BusinessCollection businessObjects)
        {
            List<OQMOD.Prospect> prospects = new List<OQMOD.Prospect>();

            foreach (UPCOENT.Prospect prospect in businessObjects)
            {
                prospects.Add(CreateProspect(prospect));
            }

            return prospects;
        }

        internal static List<OQMOD.Prospect> CreateCompanies(BusinessCollection businessObjects)
        {
            List<OQMOD.Prospect> prospects = new List<OQMOD.Prospect>();

            foreach (UPCOENT.Company company in businessObjects)
            {
                prospects.Add(CreateCompany(company));
            }

            return prospects;
        }

        private static OQMOD.Prospect CreateCompany(UPCOENT.Company company)
        {
            return new OQMOD.Prospect
            {
                IndividualId = company.IndividualId
            };
        }

        internal static List<OQMOD.Prospect> CreatePersons(BusinessCollection businessObjects)
        {
            List<OQMOD.Prospect> prospects = new List<OQMOD.Prospect>();

            foreach (UPCOENT.Person person in businessObjects)
            {
                prospects.Add(CreatePerson(person));
            }

            return prospects;
        }

        private static OQMOD.Prospect CreatePerson(UPCOENT.Person person)
        {
            return new OQMOD.Prospect
            {
                IndividualId = person.IndividualId
            };
        }

        public static OQMOD.Prospect CreateProspect(UPCOENT.Prospect prospect)
        {
            return new OQMOD.Prospect
            {
                IndividualId = prospect.IndividualId
            };
        }

        private static OQMOD.Third CreateThird(AQENT.Third third)
        {
            return new OQMOD.Third
            {
                Id = third.ThirdCode,
                CifinQuery = third.CifinDate,
                Total = third.Total,
                PrincipalDebtor = third.PrincipalDeptor,
                Cosigner = third.Creditors,
                PromissoryNoteSignature = new OQMOD.PromissoryNoteSignature
                {
                    Id = third.PromissoryNoteSignatureCode
                },
                RiskCenter = new OQMOD.RiskCenter
                {
                    Id = third.RiskCenterListCode
                },
                ReportListSisconc = new OQMOD.ReportListSisconc
                {
                    Id = third.RiskCenterListCode
                },
                Restrictive = new OQMOD.Restrictive
                {
                    Id = third.RestrictiveListCode
                },
            };
        }

        private static OQMOD.AutomaticQuotaOperation CreateAutomaticQuotaOperation(AQENT.AutomaticQuotaOperation automaticQuotaOperation)
        {
            return new OQMOD.AutomaticQuotaOperation
            {
                Id = automaticQuotaOperation.Id,
                ParentId = (int)automaticQuotaOperation?.ParentId,
                AutomaticOperationType = (int)automaticQuotaOperation.AutomaticOperationType,
                User = (int)automaticQuotaOperation?.User,
                CreationDate = automaticQuotaOperation.CreationDate,
                ModificationDate = (DateTime)automaticQuotaOperation?.ModificationDate,
                Operation = automaticQuotaOperation.Operation
            };
        }

        internal static List<OQMOD.AutomaticQuota> CreateCompanyAutomaticQuotas(BusinessCollection businessObjects)
        {
            List<OQMOD.AutomaticQuota> automaticQuota = new List<OQMOD.AutomaticQuota>();

            foreach (AQENT.AutomaticQuota automatic in businessObjects)
            {
                automaticQuota.Add(CreateCompanyAutomaticQuota(automatic));
            }

            return automaticQuota;
        }

        private static OQMOD.AutomaticQuota CreateCompanyAutomaticQuota(AQENT.AutomaticQuota automaticQuota)
        {
            return new OQMOD.AutomaticQuota
            {
                //Indicatorid = automaticQuota.IndicatorCode,
                IndividualId = automaticQuota.IndividualId,
                CustomerTypeId = automaticQuota.CustomerTypeCode,
                SuggestedQuota = automaticQuota.SuggestedQuota,
                QuotaReconsideration = (decimal)automaticQuota.QuotaReconsideration,
                LegalizedQuota = automaticQuota.LegalizedQuota,
                CurrentQuota = automaticQuota.CurrentQuota,
                CurrentCluster = automaticQuota.CurrentCluster,
                QuotaPreparationDate = automaticQuota.QuotaDate,
                RequestedById = automaticQuota.RequestedBy,
                ElaboratedId = automaticQuota.PreparedBy,
                Observations = automaticQuota.Observation


            };
        }

        public static OQMOD.AutomaticQuota CreateModelAutomaticQuota(AutomaticQuotaDTO automatic)
        {

            OQMOD.AutomaticQuota automaticQuota = new OQMOD.AutomaticQuota
            {
                AutomaticQuotaId = automatic.AutomaticQuotaId,
                CustomerTypeId = automatic.CustomerTpeId,
                IndividualId = automatic.IndividualId,
                QuotaReconsideration = automatic.QuotaReConsideration,
                CollateralStatus = automatic.CollateralStatus,
                CurrentCluster = automatic.CurrentCluster,
                CurrentQuota = automatic.CurrentQuota,
                ElaboratedId = automatic.ElaboratedId,
                ElaboratedName = automatic.ElaboratedName,
                InfringementPolicies = automatic.InfringementPolicies,
                LegalizedQuota = automatic.LegalizedQuota,
                Observations = automatic.Observations,
                QuotaPreparationDate = automatic.QuotaPreparationDate,
                RequestedById = automatic.RequestedById,
                RequestedByName = automatic.RequestedByName,
                SuggestedQuota = automatic.SuggestedQuota,
                TypeCollateral = automatic.TypeCollateral,
                AverageRevenuePercentage = automatic?.AverageRevenuePercentage,
                EquityPercentage = automatic?.EquityPercentage,
                OperatingIncomePercentage = automatic?.OperatingIncomePercentage,
                ShareCapitalPercentage = automatic?.ShareCapitalPercentage,
                ScaleWeighted = automatic?.ScaleWeighted,
                ScoreCustomerVSMarket = automatic?.ScoreCustomerVSMarket,
                QuotaA = automatic?.QuotaA,
                QuotaB = automatic?.QuotaB,
                ScoreObjectiveCriteria = automatic?.ScoreObjectiveCriteria,
                ScoreSubjectiveCriteria = automatic?.ScoreSubjectiveCriteria,
                SubjectiveWeightedScore = automatic?.SubjectiveWeightedScore,
                TargetWeightedScore = automatic?.TargetWeightedScore,
                DynamicProperties = automatic?.DynamicProperties,
                CountryId = automatic.CountryId,
                StateId = automatic.StateId,
                CityId = automatic.CityId,
                Agent = new OQMOD.Agent
                {
                    Id = automatic.Agent.Id,
                    Description = automatic.Agent.Description == null ? string.Empty : automatic.Agent.Description
                },
                AgentProgram = new OQMOD.AgentProgram
                {
                    Id = automatic.AgentProgramDTO.Id,
                    Description = automatic.AgentProgramDTO.Description,
                    SmallDescription = automatic.AgentProgramDTO.SmallDescription,
                    Enabled = automatic.AgentProgramDTO.Enabled
                },

                Prospect = new OQMOD.Prospect
                {
                    AdditionalInfo = automatic.ProspecDTO.AdditionalInfo == null ? string.Empty : automatic.ProspecDTO.AdditionalInfo,
                    Address = automatic.ProspecDTO.Address == null ? string.Empty : automatic.ProspecDTO.Address,
                    AddressTypecd = automatic.ProspecDTO.AddressTypeCd,
                    Businessname = automatic.ProspecDTO.BusinessName == null ? string.Empty : automatic.ProspecDTO.BusinessName,
                    City = automatic.ProspecDTO.City,
                    CompanytypeCd = automatic.ProspecDTO.CompanytypeCd,
                    ConstitutionDate = automatic.ProspecDTO.ConstitutionDate,
                    CountryCd = automatic.ProspecDTO.CountryCd,
                    DocumentNumber = automatic.ProspecDTO.DocumentNumber == null ? string.Empty : automatic.ProspecDTO.DocumentNumber,
                    DocumentType = automatic.ProspecDTO.DocumentType,
                    EconomicActivity = automatic.ProspecDTO.EconomicActivity,
                    Email = automatic.ProspecDTO.Email == null ? string.Empty : automatic.ProspecDTO.Email,
                    FiscalReviewer = automatic.ProspecDTO.FiscalReviewer == null ? string.Empty : automatic.ProspecDTO.FiscalReviewer,
                    IndividualTypeCd = automatic.ProspecDTO.IndividualTypeCd,
                    LegalRepresentative = automatic.ProspecDTO.LegalRepresentative == null ? string.Empty : automatic.ProspecDTO.LegalRepresentative,
                    Phone = automatic.ProspecDTO.Phone,
                    StateCd = automatic.ProspecDTO.StateCd
                },
                AcidTest = new OQMOD.Base
                {
                    Value = automatic.AcidTest?.Value,
                    Qualification = automatic.AcidTest?.Qualification,
                    Weighted = automatic.AcidTest?.Weighted,
                    Score = automatic.AcidTest?.Score
                },
                ActiveAverage = new OQMOD.Base
                {
                    Value = automatic.ActiveAverage?.Value,
                    Qualification = automatic.ActiveAverage?.Qualification,
                    Weighted = automatic.ActiveAverage?.Weighted,
                    Score = automatic.ActiveAverage?.Score
                },
                AverageEquity = new OQMOD.Base
                {
                    Value = automatic.AverageEquity?.Value,
                    Qualification = automatic.AverageEquity?.Qualification,
                    Weighted = automatic.AverageEquity?.Weighted,
                    Score = automatic.AverageEquity?.Score
                },
                AverageNetIncome = new OQMOD.Base
                {
                    Value = automatic.AverageNetIncome?.Value,
                    Qualification = automatic.AverageNetIncome?.Qualification,
                    Weighted = automatic.AverageNetIncome?.Weighted,
                    Score = automatic.AverageNetIncome?.Score
                },
                AverageSales = new OQMOD.Base
                {
                    Value = automatic.AverageSales?.Value,
                    Qualification = automatic.AverageSales?.Qualification,
                    Weighted = automatic.AverageSales?.Weighted,
                    Score = automatic.AverageSales?.Score
                },
                AverageUtility = new OQMOD.Base
                {
                    Value = automatic.AverageUtility?.Value,
                    Qualification = automatic.AverageUtility?.Qualification,
                    Weighted = automatic.AverageUtility?.Weighted,
                    Score = automatic.AverageUtility?.Score
                },
                Capacity = new OQMOD.Base
                {
                    Value = automatic.Capacity?.Value,
                    Qualification = automatic.Capacity?.Qualification,
                    Weighted = automatic.Capacity?.Weighted,
                    Score = automatic.Capacity?.Score
                },
                CorporateGovernance = new OQMOD.Base
                {
                    Value = automatic.CorporateGovernance?.Value,
                    Qualification = automatic.CorporateGovernance?.Qualification,
                    Weighted = automatic.CorporateGovernance?.Weighted,
                    Score = automatic.CorporateGovernance?.Score
                },
                CurrentReason = new OQMOD.Base
                {
                    Value = automatic.CurrentReason?.Value,
                    Qualification = automatic.CurrentReason?.Qualification,
                    Weighted = automatic.CurrentReason?.Weighted,
                    Score = automatic.CurrentReason?.Score
                },
                CustomerKnowledgeAccess = new OQMOD.Base
                {
                    Value = automatic.CustomerKnowledgeAccess?.Value,
                    Qualification = automatic.CustomerKnowledgeAccess?.Qualification,
                    Weighted = automatic.CustomerKnowledgeAccess?.Weighted,
                    Score = automatic.CustomerKnowledgeAccess?.Score
                },
                CustomerReputation = new OQMOD.Base
                {
                    Value = automatic.CustomerReputation?.Value,
                    Qualification = automatic.CustomerReputation?.Qualification,
                    Weighted = automatic.CustomerReputation?.Weighted,
                    Score = automatic.CustomerReputation?.Score
                },
                EquityVariation = new OQMOD.Base
                {
                    Value = automatic.EquityVariation?.Value,
                    Qualification = automatic.EquityVariation?.Qualification,
                    Weighted = automatic.EquityVariation?.Weighted,
                    Score = automatic.EquityVariation?.Score
                },
                Etibda = new OQMOD.Base
                {
                    Value = automatic.Etibda?.Value,
                    Qualification = automatic.Etibda?.Qualification,
                    Weighted = automatic.Etibda?.Weighted,
                    Score = automatic.Etibda?.Score
                },
                Experience = new OQMOD.Base
                {
                    Value = automatic.Experience?.Value,
                    Qualification = automatic.Experience?.Qualification,
                    Weighted = automatic.Experience?.Weighted,
                    Score = automatic.Experience?.Score
                },
                Indebtedness = new OQMOD.Base
                {
                    Value = automatic.Indebtedness?.Value,
                    Qualification = automatic.Indebtedness?.Qualification,
                    Weighted = automatic.Indebtedness?.Weighted,
                    Score = automatic.Indebtedness?.Score
                },
                KnowledgeClient = new OQMOD.Base
                {
                    Value = automatic.KnowledgeClient?.Value,
                    Qualification = automatic.KnowledgeClient?.Qualification,
                    Weighted = automatic.KnowledgeClient?.Weighted,
                    Score = automatic.KnowledgeClient?.Score
                },
                MoralSolvency = new OQMOD.Base
                {
                    Value = automatic.MoralSolvency?.Value,
                    Qualification = automatic.MoralSolvency?.Qualification,
                    Weighted = automatic.MoralSolvency?.Weighted,
                    Score = automatic.MoralSolvency?.Score
                },
                NetIncomeVariation = new OQMOD.Base
                {
                    Value = automatic.NetIncomeVariation?.Value,
                    Qualification = automatic.NetIncomeVariation?.Qualification,
                    Weighted = automatic.NetIncomeVariation?.Weighted,
                    Score = automatic.NetIncomeVariation?.Score
                },
                RestrictiveList = new OQMOD.Base
                {
                    Value = automatic.RestrictiveList?.Value,
                    Qualification = automatic.RestrictiveList?.Qualification,
                    Weighted = automatic.RestrictiveList?.Weighted,
                    Score = automatic.RestrictiveList?.Score
                },
                RiskCenter = new OQMOD.Base
                {
                    Value = automatic.RiskCenter?.Value,
                    Qualification = automatic.RiskCenter?.Qualification,
                    Weighted = automatic.RiskCenter?.Weighted,
                    Score = automatic.RiskCenter?.Score
                },
                SalesGrowth = new OQMOD.Base
                {
                    Value = automatic.SalesGrowth?.Value,
                    Qualification = automatic.SalesGrowth?.Qualification,
                    Weighted = automatic.SalesGrowth?.Weighted,
                    Score = automatic.SalesGrowth?.Score
                },
                SignatureOrLetter = new OQMOD.Base
                {
                    Value = automatic.SignatureOrLetter?.Value,
                    Qualification = automatic.SignatureOrLetter?.Qualification,
                    Weighted = automatic.SignatureOrLetter?.Weighted,
                    Score = automatic.SignatureOrLetter?.Score
                },

            };
            if (automatic.ThirdDTO != null)
            {
                automaticQuota.Third = CreateModelThird(automatic.ThirdDTO);
            }
            if (automatic.UtilityDTO != null)
            {
                automaticQuota.Utility = CreateListUtility(automatic.UtilityDTO);
            }
            if (automatic.indicatorDTO != null)
            {
                automaticQuota.Indicator = CreateListIndicators(automatic.indicatorDTO);
            }
            //if (automatic.DynamicProperties!= null)
            //{
            //    automaticQuota.DynamicProperties = CreateDynamicProperties(automatic.DynamicProperties);
            //}

            return automaticQuota;
        }

        public static OQMOD.Third CreateModelThird(ThirdDTO third)
        {
            return new OQMOD.Third
            {
                CifinQuery = third.CifinQuery,
                Cosigner = third.Cosigner,
                PrincipalDebtor = third.PrincipalDebtor,
                Total = third.Total,
                InfringementPolicies = third.InfringementPolicies,
                Restrictive = new OQMOD.Restrictive
                {
                    Description = third.RestrictiveDTO.Description,
                    Id = third.RestrictiveDTO.Id,
                    Enabled = third.RestrictiveDTO.Enabled,
                    SmallDescription = third.RestrictiveDTO.SmallDescription
                },
                PromissoryNoteSignature = new OQMOD.PromissoryNoteSignature
                {
                    Description = third.PromissoryNoteSignatureDTO.Description,
                    Id = third.PromissoryNoteSignatureDTO.Id,
                    Enabled = third.PromissoryNoteSignatureDTO.Enabled,
                    SmallDescrption = third.PromissoryNoteSignatureDTO.SmallDescrption
                },
                ReportListSisconc = new OQMOD.ReportListSisconc
                {
                    Description = third.ReportListSisconcDTO.Description,
                    Id = third.ReportListSisconcDTO.Id,
                    Enabled = third.ReportListSisconcDTO.Enabled,
                    SmallDescription = third.ReportListSisconcDTO.SmallDescription
                },
                RiskCenter = new OQMOD.RiskCenter
                {
                    Description = third.RiskCenterDTO.Description,
                    Id = third.RiskCenterDTO.Id,
                    Enabled = third.RiskCenterDTO.Enabled,
                    SmallDescription = third.RiskCenterDTO.SmallDescription
                },


            };
        }

        public static List<OQMOD.Utility> CreateListUtility(List<UtilityDTO> utilitiesDTO)
        {
            List<OQMOD.Utility> companyUtilities = new List<OQMOD.Utility>();
            foreach (UtilityDTO utilities in utilitiesDTO)
            {
                companyUtilities.Add(CreateModelUtility(utilities));
            }

            return companyUtilities;
        }

        public static OQMOD.Utility CreateModelUtility(UtilityDTO utility)
        {
            return new OQMOD.Utility
            {
                Id = utility.Id,
                Start_Values = utility.Start_Values,
                End_value = utility.End_value,
                Var_Relativa = utility.Var_Relativa,
                Var_Abs = utility.Var_Abs,
                Description = utility.Description,
                UtilityDetails = new OQMOD.UtilityDetails
                {
                    Id = utility.UtilityDetails.Id,
                    FormUtilitys = utility.UtilityDetails.FormUtilitys,
                    UtilityId = utility.UtilityDetails.UtilityId,
                    UtilitysTypeCd = utility.UtilityDetails.UtilitysTypeCd
                }
            };

        }

        public static List<OQMOD.UtilityDetails> CreateListUtilityDetails(List<UtilityDetailsDTO> utilityDetailsDTO)
        {
            List<OQMOD.UtilityDetails> companyUtilityDetails = new List<OQMOD.UtilityDetails>();
            foreach (UtilityDetailsDTO utilities in utilityDetailsDTO)
            {
                companyUtilityDetails.Add(CreateModelUtilityDetails(utilities));
            }

            return companyUtilityDetails;
        }

        public static OQMOD.UtilityDetails CreateModelUtilityDetails(UtilityDetailsDTO utilityDetailsDTO)
        {
            return new OQMOD.UtilityDetails
            {
                Id = utilityDetailsDTO.Id,
                Description = utilityDetailsDTO.Description,
                Enabled = utilityDetailsDTO.Enabled,
                FormUtilitys = utilityDetailsDTO.FormUtilitys
              ,
                UtilityId = utilityDetailsDTO.UtilityId
            };

        }


        public static List<OQMOD.Indicator> CreateListIndicators(List<IndicatorDTO> indicatorDTOs)
        {
            List<OQMOD.Indicator> companyIndicators = new List<OQMOD.Indicator>();
            foreach (IndicatorDTO indicator in indicatorDTOs)
            {
                companyIndicators.Add(CreateModelIndicator(indicator));
            }

            return companyIndicators;
        }

        public static OQMOD.Indicator CreateModelIndicator(IndicatorDTO indicatorDTO)
        {
            return new OQMOD.Indicator
            {
                ConceptIndicatorcd = indicatorDTO.ConceptIndicatorCd,
                IndicatorIni = indicatorDTO.IndicatorIni,
                IndicatorFin = indicatorDTO.IndicatorFin,
                Observation = indicatorDTO.Observation,
                TypeIndicatorcd = indicatorDTO.TypeIndicatorCd,
                Description = indicatorDTO.Description

            };

        }

        public static List<OQMOD.AutomaticQuotaOperation> CreateListAutomaticOperation(List<AutomaticQuotaOperationDTO> operationDTOs)
        {
            List<OQMOD.AutomaticQuotaOperation> companyOperations = new List<OQMOD.AutomaticQuotaOperation>();
            foreach (AutomaticQuotaOperationDTO operation in operationDTOs)
            {
                companyOperations.Add(CreateModelAutomaticOperation(operation));
            }

            return companyOperations;
        }

        public static OQMOD.AutomaticQuotaOperation CreateModelAutomaticOperation(AutomaticQuotaOperationDTO automaticQuotaOperationDto)
        {
            return new OQMOD.AutomaticQuotaOperation
            {
                Id = automaticQuotaOperationDto.Id,
                ParentId = automaticQuotaOperationDto.ParentId,
                AutomaticOperationType = automaticQuotaOperationDto.AutomaticOperationType,
                User = automaticQuotaOperationDto.User,
                CreationDate = automaticQuotaOperationDto.CreationDate,
                ModificationDate = automaticQuotaOperationDto.ModificationDate,
                Operation = automaticQuotaOperationDto.Operation
            };
        }


        internal static List<OQMOD.AutomaticQuotaOperation> CreateListAutomaticOperation(BusinessCollection businessObjects)
        {
            List<OQMOD.AutomaticQuotaOperation> operations = new List<OQMOD.AutomaticQuotaOperation>();

            foreach (AQENT.AutomaticQuotaOperation automaticOperation in businessObjects)
            {
                operations.Add(CreateAutomaticOperation(automaticOperation));
            }

            return operations;
        }

        public static OQMOD.AutomaticQuotaOperation CreateAutomaticOperation(AQENT.AutomaticQuotaOperation operation)
        {
            return new OQMOD.AutomaticQuotaOperation
            {
                Id = operation.Id,
                ParentId = operation.ParentId,
                AutomaticOperationType = (int)operation.AutomaticOperationType,
                User = operation.User,
                CreationDate = operation.CreationDate,
                ModificationDate = operation.ModificationDate,
                Operation = operation.Operation

            };
        }

        internal static OQMOD.AutomaticQuota CreateModelRulesUtility(List<OQMOD.Utility> companyUtilities, OQMOD.AutomaticQuota automatic, Facade facade)
        {

            automatic.Utility = companyUtilities;

            if (automatic != null)
            {
                OQMOD.Indicator modIndicator = new OQMOD.Indicator();
                List<OQMOD.Indicator> listIndicator = new List<OQMOD.Indicator>();
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialWorkingCapital) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.CAPITAL_TRABAJO;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Liquidez;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialWorkingCapital), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndWorkingCapital), 2);
                    //modIndicator.Observation = facade.GetConcept<string>(RuleConceptAutomaticQuotaGeneral.InitialWorkingCapital);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialCurrentRatio) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.RAZON_CORRIENTE;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Liquidez;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialCurrentRatio), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndCurrentRatio), 2);
                    //modIndicator.Observation = facade.GetConcept<string>(RuleConceptAutomaticQuotaGeneral.CurrentRatioObservation);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialAcidTest) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.PRUEBA_ACIDA;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Liquidez;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialAcidTest), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndAcidTest), 2);
                    //modIndicator.Observation = facade.GetConcept<string>(RuleConceptAutomaticQuotaGeneral.AcidTestObservation);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialDebtLevel) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.NIVEL_ENDEUDAMIENTO;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Endeudamiento;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialDebtLevel), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndDebtLevel), 2);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialFinancialDebt) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.ENDEUDAMIENTO_FINANCIERO;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Endeudamiento;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialFinancialDebt), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndFinancialDebt), 2);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialLeverage) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.APALANCAMIENTO;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Endeudamiento;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialLeverage), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndLeverage), 2);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialROE) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.PATRIMONIO_ROE;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Rentabilidad;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialROE), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndROE), 2);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialROA) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.ACTIVO_ROA;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Rentabilidad;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialROA), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndROA), 2);
                    listIndicator.Add(modIndicator);
                }

                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialEBITDA) != 0 || facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndEBITDA) != 0)
                {
                    modIndicator = new OQMOD.Indicator();
                    modIndicator.ConceptIndicatorcd = (int)Enums.EnumIndicatorConcept.EBITDA;
                    modIndicator.TypeIndicatorcd = (int)Enums.EnumIndicatorType.Actividad;
                    modIndicator.IndicatorIni = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.InitialEBITDA), 2);
                    modIndicator.IndicatorFin = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EndEBITDA), 2);
                    listIndicator.Add(modIndicator);
                }
                automatic.Indicator = listIndicator;

                //Setear redondeos que no se pueden hac er por reglas.
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.MarginETIBDA) != 0)
                {
                    automatic.Etibda.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.MarginETIBDA), 2);
                }
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.NetIncomeVariation) != 0)
                {
                    automatic.NetIncomeVariation.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.NetIncomeVariation), 2);
                }
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.SalesGrowth) != 0)
                {
                    automatic.SalesGrowth.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.SalesGrowth), 2);
                }
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EquityVariation) != 0)
                {
                    automatic.EquityVariation.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.EquityVariation), 2);
                }
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.RunningReason) != 0)
                {
                    automatic.CurrentReason.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.RunningReason), 2);
                }
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.AcidTest) != 0)
                {
                    automatic.AcidTest.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.AcidTest), 2);
                }
                if (facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.Indebtedness) != 0)
                {
                    automatic.Indebtedness.Value = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.Indebtedness), 2);
                }

            }
            return automatic;
        }


        internal static OQMOD.AutomaticQuota CreateModelRulesGeneral(OQMOD.AutomaticQuota automatic, Facade facade)
        {

            if (automatic != null)
            {
                automatic.SuggestedQuota = Math.Round(facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.SuggestedQuota), 0);
                automatic.Capacity.Score = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ScoreCapacity);
                automatic.Capacity.FinancialScore = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ScoreFinancial);
                automatic.ScoreCustomerVSMarket = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ScoreCustomerVSMarket);
                automatic.TargetWeightedScore = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.TargetWeightedScore);
                automatic.SubjectiveWeightedScore = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.SubjectiveWeightedScore);
                automatic.ScaleWeighted = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ScaleWeighted);
                automatic.QuotaA = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.QuotaA);
                automatic.QuotaB = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.QuotaB);
                var clientClassification = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ClientClassification);
                automatic.ScoreObjectiveCriteria = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ScoreObjectiveCriteria);
                automatic.ScoreSubjectiveCriteria = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaGeneral.ScoreSubjectiveCriteria);



            }
            return automatic;
        }

    }
}