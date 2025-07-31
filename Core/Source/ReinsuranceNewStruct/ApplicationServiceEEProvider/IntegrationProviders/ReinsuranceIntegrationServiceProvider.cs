using Sistran.Core.Application.ReinsuranceServices.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.ReinsuranceIntegrationServices;
using Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.IntegrationProviders
{
    public class ReinsuranceIntegrationServiceProvider : IReinsuranceIntegrationServices
    {
        public ReinsuranceDTO ReinsuranceIssue(int policyId, int endorsementId, int userId)
        {
            try
            {
                ReinsuranceDTO reinsuranceDTO = new ReinsuranceDTO();
                PolicyDTO policyDTO = new PolicyDTO();
                policyDTO.Endorsement = new EndorsementDTO();
                policyDTO.PolicyId = policyId;
                policyDTO.Endorsement.Id = endorsementId;
                reinsuranceDTO = ReinsureEndorsement(policyDTO, userId, true);
                if (reinsuranceDTO.ReinsuranceId > 0)
                {
                    CreateOperatingQuotaEvents(endorsementId);
                }
                return reinsuranceDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsuranceProcess);
            }

        }

        public ReinsuranceDTO ReinsuranceClaim(int claimId, int claimModifyId, int userId)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                ReinsuranceDTO reinsuranceDTO = claimDAO.ReinsuranceClaim(claimId, claimModifyId, userId).ToIntegrationDTO();

                if (reinsuranceDTO.Number >= 0)
                {
                    reinsuranceDTO.IsReinsured = true;
                }

                return reinsuranceDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsuranceClaim);
            }
        }

        public ReinsuranceDTO ReinsurancePayment(int paymentRequestId, int userId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                ReinsuranceDTO reinsuranceDTO = levelPaymentDAO.ReinsurancePayment(paymentRequestId, userId).ToIntegrationDTO();

                if (reinsuranceDTO.ReinsuranceId > 0)
                {
                    reinsuranceDTO.IsReinsured = true;
                }

                return reinsuranceDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsurancePayment);
            }
        }                

        private ReinsuranceDTO ReinsureEndorsement(PolicyDTO policyDTO, int userId, bool onLine)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                List<LineDTO> lineDTOs = new List<LineDTO>();
                policyDTO = DelegateService.tempCommonIntegrationService.GetPolicyReinsuranceByPolicyIdEndorsementId(policyDTO.PolicyId, policyDTO.Endorsement.Id).ToDTO().ToIntegrationDTO();
                policyDTO.Prefix = new PrefixDTO();
                policyDTO.Prefix.Id = DTOAssembler.CreatePolicyDTOByUNDDTOPolicyDTO(DelegateService.underwritingIntegrationService.GetPolicyByPolicyId(policyDTO.Id)).Prefix.Id;
                ValidatePriorityRetention(policyDTO);
                lineDTOs = GetLinesParametrization(policyDTO);
                return reinsuranceDAO.ReinsureEndorsement(userId, policyDTO.ToDTO(), lineDTOs.ToDTOs().ToList()).ToIntegrationDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsureEndorsement);
            }
        }

        #region Líneas

        Func<RiskDTO, List<LineDTO>> OrderRiskCoversByLine()
        {
            return (RiskDTO riskDTO) =>
            {
                List<LineDTO> lineDTOs = new List<LineDTO>();
                lineDTOs = riskDTO.Coverages.Select(GetLineByCoverage()).ToList();
                return lineDTOs;
            };
        }

        Func<CoverageDTO, LineDTO> GetLineByCoverage()
        {
            return (CoverageDTO coverageDTO) =>
            {
                return new LineDTO
                {
                    LineId = coverageDTO.LineId
                };
            };

        }

        public List<LineDTO> GetLinesParametrization(PolicyDTO policyDTO)
        {
            try
            {
                List<LineDTO> lineDTOs = new List<LineDTO>();
                List<RiskDTO> riskDTOs = new List<RiskDTO>();
                riskDTOs = policyDTO.Endorsement.Risks;
                riskDTOs.Select(OrderRiskCoversByLine()).ToList().ForEach(x =>
                    lineDTOs.AddRange(x)
                );

                List<LineDTO> groupedLineDTOs = new List<LineDTO>();
                List<LineDTO> orderedLinesDTOs = new List<LineDTO>();
                List<LineDTO> linesDTOsResult = new List<LineDTO>();

                groupedLineDTOs = lineDTOs.GroupBy(x => x.LineId).Select(y => new LineDTO { LineId = y.FirstOrDefault().LineId }).ToList();
                orderedLinesDTOs = groupedLineDTOs.OrderBy(x => x.LineId).ToList();

                linesDTOsResult = orderedLinesDTOs.Select(x => GetLineParametrizationByLineId(x.LineId)).ToList();

                return linesDTOsResult;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetParametrizationLines);
            }
        }

        Func<ContractLineDTO, ContractLineDTO> CreateContractLine()
        {
            return (ContractLineDTO contractLineDTO) =>
            {
                ContractLineDTO contractLineDTOResult = new ContractLineDTO();
                ContractDTO contractDTO = new ContractDTO();
                List<LevelDTO> levelDTOs = new List<LevelDTO>();
                contractLineDTOResult = contractLineDTO;
                contractDTO = GetContractById(contractLineDTO.Contract.ContractId);
                contractLineDTOResult.Contract = contractDTO;
                levelDTOs = GetLevelsByContractId(contractDTO.ContractId);
                contractLineDTOResult.Contract.ContractLevels = levelDTOs.Select(CreateLevel()).ToList();
                return contractLineDTO;
            };
        }

        Func<LevelDTO, LevelDTO> CreateLevel()
        {
            return (LevelDTO levelDTO) =>
            {
                LevelDTO levelDTOResult = new LevelDTO();
                List<LevelCompanyDTO> levelCompanyDTOs = new List<LevelCompanyDTO>();
                levelDTOResult = levelDTO;
                levelCompanyDTOs = GetLevelCompaniesByLevelId(levelDTO.ContractLevelId);
                levelDTOResult.ContractLevelCompanies = levelCompanyDTOs.Select(CreateLevelCompany()).ToList();
                return levelDTOResult;
            };
        }

        Func<LevelCompanyDTO, LevelCompanyDTO> CreateLevelCompany()
        {
            return (LevelCompanyDTO levelCompanyDTO) =>
            {
                LevelCompanyDTO levelCompanyDTOResult = new LevelCompanyDTO();
                levelCompanyDTOResult = levelCompanyDTO;
                UniquePerson.IntegrationService.Models.CompanyDTO company = DelegateService.uniquePersonIntegrationService.GetCompanyByIndividualId(levelCompanyDTO.Company.IndividualId);

                CompanyDTO companyDTO = new CompanyDTO()
                {
                    IndividualId = company.IndividualId,
                    FullName = company.FullName
                };

                levelCompanyDTOResult.Company = new CompanyDTO();
                levelCompanyDTOResult.Company = companyDTO;

                if (levelCompanyDTO.Agent.IndividualId > 0)
                {
                    UniquePerson.IntegrationService.Models.CompanyDTO agentCompany = DelegateService.uniquePersonIntegrationService.GetCompanyByIndividualId(levelCompanyDTO.Agent.IndividualId);
                    AgentDTO agent = new AgentDTO
                    {
                        IndividualId = agentCompany.IndividualId,
                        FullName = agentCompany.FullName
                    };

                    levelCompanyDTOResult.Agent = new AgentDTO();
                    levelCompanyDTOResult.Agent = agent;
                }

                return levelCompanyDTOResult;
            };
        }

        public LineDTO GetLineParametrizationByLineId(int lineId)
        {
            try
            {
                LineDTO lineDTO = new LineDTO();
                List<ContractLineDTO> contractLineDTOs = new List<ContractLineDTO>();
                lineDTO = GetLineByLineId(lineId);
                contractLineDTOs = GetContractLineByLineId(lineId).ContractLines;
                lineDTO.ContractLines = contractLineDTOs.Select(CreateContractLine()).ToList();
                return lineDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetParametrizationLineByLineId);
            }
        }

        private LineDTO GetLineByLineId(int lineId)
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                return lineDAO.GetLineByLineId(lineId).ToIntegrationDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineByLineId);
            }
        }

        private LineDTO GetContractLineByLineId(int lineId)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                return contractLineDAO.GetContractLineByLineId(lineId).ToIntegrationDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractLineByLineId);
            }
        }

        private ContractDTO GetContractById(int contractId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return contractDAO.GetContractById(contractId).ToIntegrationDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractById);
            }
        }

        private List<LevelDTO> GetLevelsByContractId(int contractId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return levelDAO.GetLevelsByContractId(contractId).ToIntegrationDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelsByContractId);
            }
        }

        private List<LevelCompanyDTO> GetLevelCompaniesByLevelId(int levelId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return levelDAO.GetLevelCompaniesByLevelId(levelId).ToIntegrationDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelCompaniesByLevelId);
            }
        }

        #endregion 

        #region Cúmulos 

        public bool CreateOperatingQuotaEvents(int endorsmentId)
        {
            try
            {
                try
                {
                    ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                    List<OperatingQuotaEventDTO> operatingQuotaEvents = new List<OperatingQuotaEventDTO>();
                    List<IssueAllocationRiskCoverDTO> issueAllocationRiskCoverDTOs = new List<IssueAllocationRiskCoverDTO>();
                    issueAllocationRiskCoverDTOs = reinsuranceDAO.GetIssueAllocationRiskCoveragesByEndorsementId(endorsmentId).ToIntegrationDTOs().ToList();
                    operatingQuotaEvents = issueAllocationRiskCoverDTOs.Select(CreateOperatingQuotaEventByIssueAllocationRiskCoverDTO()).ToList();
                    return DelegateService.reinsuranceOperatingQuotaIntegrationServices.CreateOperatingQuotaEvents(operatingQuotaEvents.ToDTOs().CreateROQINTDTOOperatingQuotaByOperatingQuotaEventDTOs());
                }
                catch (BusinessException)
                {
                    throw new BusinessException(Resources.Resources.ErrorCreateOperatingQuotaEvents);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateOperatingQuotaEvents);
            }
        }
        
        public bool MigrateReinsuranceCumulus()
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                List<OperatingQuotaEventDTO> operatingQuotaEvents = new List<OperatingQuotaEventDTO>();
                List<IssueAllocationRiskCoverDTO> issueAllocationRiskCoverDTOs = new List<IssueAllocationRiskCoverDTO>();
                issueAllocationRiskCoverDTOs = reinsuranceDAO.GetIssueAllocationRiskCoverages().ToIntegrationDTOs().ToList();
                operatingQuotaEvents = issueAllocationRiskCoverDTOs.Select(CreateOperatingQuotaEventByIssueAllocationRiskCoverDTO()).ToList();
                return DelegateService.reinsuranceOperatingQuotaIntegrationServices.MigrateReinsuranceCumulus(operatingQuotaEvents.ToDTOs().CreateROQINTDTOOperatingQuotaByOperatingQuotaEventDTOs());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateOperatingQuotaEvents);
            }
        }

        Func<IssueAllocationRiskCoverDTO, OperatingQuotaEventDTO> CreateOperatingQuotaEventByIssueAllocationRiskCoverDTO()
        {
            return (IssueAllocationRiskCoverDTO issueAllocationRiskCoverDTO) =>
            {
                OperatingQuotaEventDTO operatingQuotaEventDTO = new OperatingQuotaEventDTO();
                List<ContractCoverageDTO> contractCoverageDTOs = new List<ContractCoverageDTO>();
                ContractCoverageDTO contractCoverageDTO = new ContractCoverageDTO();
                operatingQuotaEventDTO = issueAllocationRiskCoverDTO.ToModel().CreateOperatingQuotaEventDTOByIssueAllocationRiskCoverDTO().ToIntegrationDTO();
                operatingQuotaEventDTO.OperatingQuotaEventType = Convert.ToInt32(EventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
                operatingQuotaEventDTO.ApplyReinsurance = issueAllocationRiskCoverDTO.ToModel().CreateApplyReinsuranceDTOByIssueAllocationRiskCoverDTO().ToIntegrationDTO();
                operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage = 100;
              
                switch (operatingQuotaEventDTO.LineBusinessID)
                {
                    case (int)LineBusinessKeys.CUMPLIMIENTO:
                    case (int)LineBusinessKeys.ARRENDAMIENTOS:
                        operatingQuotaEventDTO.IdentificationId = issueAllocationRiskCoverDTO.IndividualCd;
                        operatingQuotaEventDTO.ApplyReinsurance.IndividualId = issueAllocationRiskCoverDTO.InsuredCd;
                        break;
                    default:
                        operatingQuotaEventDTO.IdentificationId = issueAllocationRiskCoverDTO.InsuredCd;
                        operatingQuotaEventDTO.ApplyReinsurance.IndividualId = issueAllocationRiskCoverDTO.InsuredCd;
                        break;
                }

                contractCoverageDTOs.Add(issueAllocationRiskCoverDTO.ToModel().CreateContractCoverageDTOByIssueAllocationRiskCover().ToIntegrationDTO());
                operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage = contractCoverageDTOs;

                return operatingQuotaEventDTO;
            };
        }
        
        private Func<OperatingQuotaEventDTO, IEnumerable<ContractReinsuranceCumulusDTO>> GetCumulusContractsByIndividual(List<CurrencyDTO> currencyDTOs, List<ExchangeRateDTO> exchangeRateDTOs)
        {
            try
            {
                return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
                {
                    return operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage.Select(CreateContractReinsuranceCumulusDTOByoperatingQuotaEventDTO(operatingQuotaEventDTO, currencyDTOs, exchangeRateDTOs));
                };
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusByIndividual);
            }
        }

        private List<ExchangeRateDTO> GetExchangeRates()
        {
            try
            {
                return DelegateService.commonIntegrationService.GetExchangeRates().ToDTOs().ToIntegrationDTOs();
            }
            catch (Exception)
            {
                return new List<ExchangeRateDTO>();
            }
        }

        private Func<ContractCoverageDTO, ContractReinsuranceCumulusDTO> CreateContractReinsuranceCumulusDTOByoperatingQuotaEventDTO(OperatingQuotaEventDTO operatingQuotaEventDTO, List<CurrencyDTO> currencies, List<ExchangeRateDTO> exchangeRates)
        {
            return (ContractCoverageDTO contractCoverageDTO) =>
            {
                ContractReinsuranceCumulusDTO contractReinsuranceCumulusDTO = new ContractReinsuranceCumulusDTO();
                contractReinsuranceCumulusDTO.LevelLimit = contractCoverageDTO.LevelLimit;

                contractReinsuranceCumulusDTO.Contract = new ContractDTO
                {
                    ContractId = contractCoverageDTO.ContractId,
                    SmallDescription = contractCoverageDTO.ContractDescription,
                    Currency = new CurrencyDTO
                    {
                        Id = contractCoverageDTO.ContractCurrencyId,
                        Description = currencies.Find(x => x.Id == contractCoverageDTO.ContractCurrencyId).Description
                    }
                };

                if (contractCoverageDTO.ContractDescription == ContractTypeKeys.RETENCION.ToString())
                {
                    contractReinsuranceCumulusDTO.RetentionAmountLocalCurrency = (contractCoverageDTO.Amount * operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage) / 100;
                    contractReinsuranceCumulusDTO.RetentionPremiumAmountLocalCurrency = (contractCoverageDTO.Premium * operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage) / 100;
                    contractReinsuranceCumulusDTO.AmountLocalCurrency = contractReinsuranceCumulusDTO.RetentionAmountLocalCurrency;
                }
                else
                {
                    contractReinsuranceCumulusDTO.AssignmentAmountLocalCurrency = (contractCoverageDTO.Amount * operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage) / 100;
                    contractReinsuranceCumulusDTO.AssignmentPremiumAmountLocalCurrency = (contractCoverageDTO.Premium * operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage) / 100;
                    contractReinsuranceCumulusDTO.AmountLocalCurrency = contractReinsuranceCumulusDTO.AssignmentAmountLocalCurrency;
                }

                if (contractCoverageDTO.ContractCurrencyId != 0)
                {
                    decimal exchangeRate;

                    if(exchangeRates.FindAll(x => x.Currency.Id == contractCoverageDTO.ContractCurrencyId && x.RateDate == DateTime.Now).Count > 0)
                    {
                        exchangeRate = exchangeRates.FindAll(x => x.Currency.Id == contractCoverageDTO.ContractCurrencyId && x.RateDate == DateTime.Now).OrderByDescending(y => y.RateDate)
                                                                                                                                                        .FirstOrDefault()
                                                                                                                                                        .BuyAmount;
                    }
                    else
                    {
                        exchangeRate = exchangeRates.FindAll(x => x.Currency.Id == contractCoverageDTO.ContractCurrencyId && x.RateDate <= DateTime.Now).OrderByDescending(y => y.RateDate)
                                                                                                                                                        .FirstOrDefault()
                                                                                                                                                        .BuyAmount;
                    }

                    if (contractCoverageDTO.ContractDescription == ContractTypeKeys.RETENCION.ToString())
                    {
                        contractReinsuranceCumulusDTO.RetentionAmount = contractReinsuranceCumulusDTO.RetentionAmountLocalCurrency / exchangeRate;
                        contractReinsuranceCumulusDTO.RetentionPremiumAmount = contractReinsuranceCumulusDTO.RetentionPremiumAmountLocalCurrency / exchangeRate;
                    }
                    else
                    {
                        contractReinsuranceCumulusDTO.AssignmentAmount = contractReinsuranceCumulusDTO.AssignmentAmountLocalCurrency / exchangeRate;
                        contractReinsuranceCumulusDTO.AssignmentPremiumAmount = contractReinsuranceCumulusDTO.AssignmentPremiumAmountLocalCurrency / exchangeRate;
                    }
                }
                else
                {
                    if (contractCoverageDTO.ContractDescription == ContractTypeKeys.RETENCION.ToString())
                    {
                        contractReinsuranceCumulusDTO.RetentionAmount = contractReinsuranceCumulusDTO.RetentionAmountLocalCurrency;
                        contractReinsuranceCumulusDTO.RetentionPremiumAmount = contractReinsuranceCumulusDTO.RetentionPremiumAmountLocalCurrency;
                    }
                    else
                    {
                        contractReinsuranceCumulusDTO.AssignmentAmount = contractReinsuranceCumulusDTO.AssignmentAmountLocalCurrency;
                        contractReinsuranceCumulusDTO.AssignmentPremiumAmount = contractReinsuranceCumulusDTO.AssignmentPremiumAmountLocalCurrency;
                    }
                }

                return contractReinsuranceCumulusDTO;
            };
        }

        #endregion

        #region Retención Prioritaria

        private List<PriorityRetentionDTO> GetPriorityRetentionDTOsByPrefixCd(int prefixCd)
        {
            try
            {
                PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                return priorityRetentionDAO.GetPriorityRetentionsByPrefixCd(prefixCd).ToDTOs().ToIntegrationDTOs().Where(x => x.Enabled == true).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLevelPayment);
            }
        }

        public void ValidatePriorityRetention(PolicyDTO policyDTO)
        {
            try
            {
                switch (policyDTO.Prefix.Id)
                {
                    case (int)PrefixTypes.CUMPLIMIENTO:
                    case (int)PrefixTypes.CAUCION_JUDICIAL:
                    case (int)PrefixTypes.RESPONSABILIDAD_CIVIL:
                    case (int)PrefixTypes.ARRENDAMIENTOS:
                        PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                    ReinsuranceCumulusDTO reinsuranceCumulusDTO = new ReinsuranceCumulusDTO();
                    List<PriorityRetentionDTO> priorityRetentionDTOs = new List<PriorityRetentionDTO>();
                    PriorityRetentionDTO priorityRetentionDTO = new PriorityRetentionDTO();
                    List<ContractReinsuranceCumulusDTO> contractReinsuranceCumulusDTOs = new List<ContractReinsuranceCumulusDTO>();
                    List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                    priorityRetentionDTOs = GetPriorityRetentionDTOsByPrefixCd(policyDTO.Prefix.Id); //Implementar
                    priorityRetentionDTO = priorityRetentionDTOs.Where(x => policyDTO.Endorsement.IssueDate >= x.ValidityFrom && policyDTO.Endorsement.IssueDate <= x.ValidityTo).FirstOrDefault();

                    if (priorityRetentionDTO != null)
                    {
                        List<CurrencyDTO> currencyDTOs = DelegateService.commonIntegrationService.GetCurrencies().ToDTOs().ToIntegrationDTOs().ToList();
                        List<ExchangeRateDTO> exchangeRateDTOs = new List<ExchangeRateDTO>();
                        exchangeRateDTOs = GetExchangeRates();

                        DelegateService.reinsuranceOperatingQuotaIntegrationServices.GetCumulusCoveragesByIndividual(policyDTO.Endorsement.Risks.FirstOrDefault().IndividualId, 0, DateTime.Now, true, 0, policyDTO.Prefix.Id, true).Select(op => DTOAssembler.CreateOperatingQuotaEventDTO(op).ToIntegrationDTO()).Select(GetCumulusContractsByIndividual(currencyDTOs, exchangeRateDTOs)).ToList().ForEach(x =>
                        {
                            contractReinsuranceCumulusDTOs.AddRange(x);
                        });

                        reinsuranceCumulusDTO.RetentionTotalCumulus = contractReinsuranceCumulusDTOs.Sum(x => x.RetentionAmountLocalCurrency);

                        PriorityRetentionDetailDTO priorityRetentionDetailDTO = new PriorityRetentionDetailDTO();
                        List<PriorityRetentionDetailDTO> priorityRetentionDetailDTOs = new List<PriorityRetentionDetailDTO>();
                        priorityRetentionDetailDTOs = priorityRetentionDAO.GetPriorityRetentionDetailsByPolicyIdEndorsementId(policyDTO.Id, policyDTO.Endorsement.Id).ToDTOs().ToIntegrationDTOs().ToList();

                        if (priorityRetentionDetailDTOs.Count == 0)
                        {
                            priorityRetentionDetailDTO.PriorityRetentionId = priorityRetentionDTO.Id;
                            priorityRetentionDetailDTO.ProcessDate = DateTime.Now;
                            priorityRetentionDetailDTO.IssueDate = policyDTO.Endorsement.IssueDate;
                            priorityRetentionDetailDTO.EndorsementId = policyDTO.Endorsement.Id;
                            priorityRetentionDetailDTO.PolicyId = policyDTO.Id;
                            priorityRetentionDetailDTO.PrefixCd = policyDTO.Prefix.Id;
                            priorityRetentionDetailDTO.IndividualId = policyDTO.Endorsement.Risks.FirstOrDefault().IndividualId;
                            priorityRetentionDetailDTO.PriorityRetentionAmount = priorityRetentionDTO.PriorityRetentionAmount;
                            priorityRetentionDetailDTO.RetentionCumulus = reinsuranceCumulusDTO.RetentionTotalCumulus;
                            priorityRetentionDetailDTO.CurrentPriorityRetentionAmount = priorityRetentionDTO.PriorityRetentionAmount - reinsuranceCumulusDTO.RetentionTotalCumulus;
                            priorityRetentionDetailDTO.CurrentPriorityRetentionAmount = priorityRetentionDetailDTO.CurrentPriorityRetentionAmount < 0 ? 0 : priorityRetentionDetailDTO.CurrentPriorityRetentionAmount;
                            priorityRetentionDAO.SavePriorityRetentionDetail(priorityRetentionDetailDTO.ToDTO().ToModel());
                        }
                    }
                    break;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidatePriorityRetention);
            }
        }

        #endregion

        #region Siniestros

        public ReinsuranceDTO ReinsuranceCancellationPayment(int paymentRequestId, int userId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                ReinsuranceDTO reinsuranceDTO = levelPaymentDAO.ReinsuranceCancellationPayment(paymentRequestId, userId).ToIntegrationDTO();

                if (reinsuranceDTO.ReinsuranceId > 0)
                {
                    reinsuranceDTO.IsReinsured = true;
                }

                return reinsuranceDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsurancePayment);
            }
        }

        public bool ValidateClaimPaymentRequestReinsurance(int paymentRequestId)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                return claimDAO.ValidateClaimPaymentRequestReinsurance(paymentRequestId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidatingPaymentRequestReinsurance);
            }
        }

        public bool ValidateEndorsementReinsurance(int endorsementId)
        {
            try
            {
                DistributionDAO distributionDAO = new DistributionDAO();
                return distributionDAO.ValidateEndorsementReinsurance(endorsementId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidatingEndorsementReinsurance);
            }
        }

        #endregion
    }
}
