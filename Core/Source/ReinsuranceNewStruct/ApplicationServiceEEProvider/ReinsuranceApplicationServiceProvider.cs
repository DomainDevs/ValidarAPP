using System;
using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using System.Linq;
using System.Globalization;
using System.Data;
using System.Configuration;


using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Business;
using Sistran.Core.Application.ReinsuranceServices.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Helper;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider
{
    /// <summary>
    /// </summary>
    public class ReinsuranceApplicationServiceProvider : IReinsuranceApplicationService
    {

        #region Parametrización

        public bool DeleteReinsurancePrefix(ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            try
            {
                LineBusinessReinsuranceDAO lineBusinessReinsuranceDAO = new LineBusinessReinsuranceDAO();
                return lineBusinessReinsuranceDAO.DeleteLineBusinessReinsurance(reinsurancePrefixDTO.Id);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteReinsurancePrefix);
            }
        }

        public bool ValidateContractIssueAllocation(int contractId)
        {
            try
            {
                LevelCompanyDAO levelCompanyDAO = new LevelCompanyDAO();
                return levelCompanyDAO.ValidateContractIssueAllocation(contractId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateContractIssueAllocation);
            }
        }

        public ContractDTO GetContractById(int contractId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return DTOAssembler.ToDTO(contractDAO.GetContractById(contractId));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractById);
            }
        }

        public bool ValidateDuplicateContract(ContractDTO contractDTO)
        {
            try
            {
                List<ContractDTO> contractDTOs = new List<ContractDTO>();
                ContractDAO contractDAO = new ContractDAO();
                contractDTOs = contractDAO.GetContractsByContractTypeIdDescription(contractDTO.ToModel()).ToDTOs().ToList();
                contractDTOs.RemoveAll(x => x.ContractId == contractDTO.ContractId);

                if (contractDTOs.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateDuplicateContract);
            }
        }

        public bool CopyContract(int contractId, string smallDescription, int year, string description)
        {
            try
            {
                ContractDTO contractDTO = new ContractDTO();
                List<LevelDTO> levelDTOs = new List<LevelDTO>();
                List<LevelCompanyDTO> levelCompanyDTOs = new List<LevelCompanyDTO>();
                contractDTO = GetContractById(contractId);
                contractDTO.ContractId = 0;
                contractDTO.SmallDescription = smallDescription;
                contractDTO.Year = year;
                contractDTO.Description = description;

                if (!ValidateDuplicateContract(contractDTO))
                {
                    int newContractId;
                    levelDTOs = GetLevelsByContractId(contractId);
                    newContractId = SaveContract(contractDTO);
                    if (levelDTOs.Count > 0)
                    {
                        foreach (LevelDTO levelDTO in levelDTOs)
                        {
                            int contractLevelId = levelDTO.ContractLevelId;
                            int newContractLevelId;
                            levelCompanyDTOs = GetLevelCompaniesByLevelId(contractLevelId);
                            levelDTO.ContractLevelId = 0;
                            levelDTO.Contract.ContractId = newContractId;
                            newContractLevelId = SaveLevel(levelDTO);

                            if (levelCompanyDTOs.Count > 0)
                            {
                                foreach (LevelCompanyDTO levelCompanyDTO in levelCompanyDTOs)
                                {
                                    int newLevelCompanyId;
                                    levelCompanyDTO.LevelCompanyId = 0;
                                    levelCompanyDTO.ContractLevel.ContractLevelId = newContractLevelId;
                                    newLevelCompanyId = SaveLevelCompany(levelCompanyDTO);
                                }
                            }

                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorCopyContract);
            }
        }

        public bool ValidateCompleteContract(int contractId)
        {
            try
            {
                ContractDTO contractDTO = new ContractDTO();
                List<LevelDTO> levelDTOs = new List<LevelDTO>();
                List<LevelCompanyDTO> levelCompanyDTOs = new List<LevelCompanyDTO>();
                contractDTO = GetContractById(contractId);
                levelDTOs = GetLevelsByContractId(contractId);

                bool result = false;

                if (contractDTO.ContractType.ContractTypeId != (int)ContractTypeKeys.RETENCION)
                {

                    if (levelDTOs.Count > 0)
                    {
                        foreach (LevelDTO levelDTO in levelDTOs)
                        {
                            int contractLevelId = levelDTO.ContractLevelId;
                            levelCompanyDTOs = GetLevelCompaniesByLevelId(contractLevelId);
                            if (levelCompanyDTOs.Count > 0)
                            {
                                if (levelCompanyDTOs.Sum(x => x.GivenPercentage) < 100)
                                {
                                    result = false;
                                    break;
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                            else
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }

                return result;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateCompleteContract);
            }
        }

        public ContractLineDTO GetContractLineByContractLineId(int contractLineId)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                return contractLineDAO.GetContractLineById(contractLineId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractLineByContractLineId);
            }
        }

        public decimal GetParticipationPercentageByLevelId(int levelId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return levelDAO.GetParticipationPercentageByLevelId(levelId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetParticipationPercentageByLevelId);
            }
        }

        public int GetAssociationColumnId(int associationTypeId, string tableName, string columnIdName)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                return associationColumnValueDAO.GetAssociationColumnId(associationTypeId, tableName, columnIdName);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationColumnId);
            }
        }

        public int GetAssociationColumnValueId(int lineAssociationId, int associationColumnId)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                return associationColumnValueDAO.GetAssociationColumnValueId(lineAssociationId, associationColumnId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationColumnValueId);
            }
        }

        public int GetCountAssociationColumn(int associationTypeId)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                return associationColumnValueDAO.GetCountAssociationColumn(associationTypeId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCountAssociationColumn);
            }
        }

        public int GetLevelNumberByContractId(int contractId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return levelDAO.GetLevelNumberByContractId(contractId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelNumberByContractId);
            }
        }

        public int GetReinsuranceCompanyIdByLevelIdAndIndividualId(int levelId, int individualId)
        {
            try
            {
                LevelCompanyDAO levelCompanyDAO = new LevelCompanyDAO();
                return levelCompanyDAO.GetReinsuranceCompanyIdByLevelIdAndIndividualId(levelId, individualId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceCompanyIdByLevelIdAndIndividualId);
            }
        }

        public LevelDTO GetLevelByLevelId(int levelId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return DTOAssembler.ToDTO(levelDAO.GetLevelsByLevelId(levelId));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelByLevelId);
            }
        }

        public LevelCompanyDTO GetLevelCompanyByCompanyId(int levelCompanyId)
        {
            try
            {
                LevelCompanyDAO levelCompanyDAO = new LevelCompanyDAO();
                return DTOAssembler.ToDTO(levelCompanyDAO.GetLevelCompanyByCompanyId(levelCompanyId));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelCompanyByCompanyId);
            }
        }

        public LevelPaymentDTO GetLevelPayment(int levelPaymentId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetLevelPayment(levelPaymentId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelPayment);
            }
        }

        public LevelRestoreDTO GetLevelRestore(int levelRestoreId)
        {
            try
            {
                LevelRestoreDAO levelRestoreDAO = new LevelRestoreDAO();
                return levelRestoreDAO.GetLevelRestore(levelRestoreId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelRestore);
            }
        }

        public LineDTO GetContractLineByLineId(int lineId)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                return DTOAssembler.ToDTO(contractLineDAO.GetContractLineByLineId(lineId));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractLineByLineId);
            }
        }

        public LineDTO GetLineByLineId(int lineId)
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                return DTOAssembler.ToDTO(lineDAO.GetLineByLineId(lineId));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineByLineId);
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

        public LineAssociationDTO GetAssociationLineById(int associationLineId)
        {
            try
            {
                AssociationLineDAO associationLineDAO = new AssociationLineDAO();
                return associationLineDAO.GetAssociationLineById(associationLineId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationLineById);
            }
        }

        public LineAssociationDTO SaveLineAssociation(LineAssociationDTO lineAssociation)
        {
            try
            {
                LineAssociationDTO lineAssociationDTO = new LineAssociationDTO();
                AssociationLineDAO associationLineDAO = new AssociationLineDAO();

                if (lineAssociation.LineAssociationId == 0)
                {
                    // GRABA ASOCIACION DE LINEA
                    lineAssociationDTO = associationLineDAO.SaveAssociationLine(lineAssociation.ToModel()).ToDTO();
                    lineAssociation.LineAssociationId = lineAssociationDTO.LineAssociationId;
                }
                else
                {
                    lineAssociationDTO = lineAssociation;
                }
                int associationColumnId = 0;

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS)))//1 
                {
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, "COMM.LINE_BUSINESS", "LINE_BUSINESS_CD");
                    ByLineBusinessDTO byLineBusinessDTO = (ByLineBusinessDTO)lineAssociation.AssociationType;
                    SaveByLineBusiness(lineAssociation.LineAssociationId, associationColumnId, byLineBusinessDTO);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS)))//2
                {
                    ByLineBusinessSubLineBusinessDTO lineBusinessSubLineBusiness = (ByLineBusinessSubLineBusinessDTO)lineAssociation.AssociationType;

                    lineBusinessSubLineBusiness.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusiness.Description, "LINE_BUSINESS_CD");
                    SaveByLineBusinessSubLineBusiness(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusiness);

                    lineBusinessSubLineBusiness.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusiness.Description, "SUB_LINE_BUSINESS_CD");
                    SaveByLineBusinessSubLineBusiness(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusiness);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_OPERATION_TYPE_PREFIX)))//3
                {
                    ByOperationTypePrefixDTO operationTypePrefix = (ByOperationTypePrefixDTO)lineAssociation.AssociationType;

                    operationTypePrefix.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, operationTypePrefix.Description, "PREFIX_CD");
                    SaveByOperationTypePrefix(lineAssociation.LineAssociationId, associationColumnId, operationTypePrefix);

                    operationTypePrefix.Description = "PARAM.BUSINESS_TYPE";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, operationTypePrefix.Description, "BUSINESS_TYPE_CD");
                    SaveByOperationTypePrefix(lineAssociation.LineAssociationId, associationColumnId, operationTypePrefix);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_INSURED)))//4
                {
                    ByInsuredDTO insured = (ByInsuredDTO)lineAssociation.AssociationType;

                    insured.Description = "UP.INSURED";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, insured.Description, "INSURED_CD");
                    SaveByInsured(lineAssociation.LineAssociationId, associationColumnId, insured);
                }


                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX)))//5
                {
                    ByPrefixDTO prefix = (ByPrefixDTO)lineAssociation.AssociationType;

                    prefix.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, prefix.Description, "PREFIX_CD");
                    SaveByPrefix(lineAssociation.LineAssociationId, associationColumnId, prefix);
                }


                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_POLICY)))//5
                {
                    ByPolicyDTO policy = (ByPolicyDTO)lineAssociation.AssociationType;

                    policy.Description = "ISS.POLICY";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, policy.Description, "POLICY_ID");
                    SaveByPolicy(lineAssociation.LineAssociationId, associationColumnId, policy);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_FACULTATIVE_ISSUE)))//6
                {
                    ByFacultativeIssueDTO facultative = (ByFacultativeIssueDTO)lineAssociation.AssociationType;

                    facultative.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, facultative.Description, "PREFIX_CD");
                    SaveByFacultativeIssue(lineAssociation.LineAssociationId, associationColumnId, facultative);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_INSURED_PREFIX)))//7
                {
                    ByInsuredPrefixDTO insuredPrefix = (ByInsuredPrefixDTO)lineAssociation.AssociationType;

                    insuredPrefix.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, insuredPrefix.Description, "PREFIX_CD");
                    SaveByInsuredPrefix(lineAssociation.LineAssociationId, associationColumnId, insuredPrefix);

                    insuredPrefix.Description = "UP.INSURED";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, insuredPrefix.Description, "INSURED_CD");
                    SaveByInsuredPrefix(lineAssociation.LineAssociationId, associationColumnId, insuredPrefix);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS_RISK)))//8
                {
                    ByLineBusinessSubLineBusinessInsuredObjectDTO lineBusinessSubLineBusinessRisk = (ByLineBusinessSubLineBusinessInsuredObjectDTO)lineAssociation.AssociationType;

                    lineBusinessSubLineBusinessRisk.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessRisk.Description, "LINE_BUSINESS_CD");
                    if (GetAssociationColumnValueId(lineAssociation.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessRisk(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessRisk);
                    }

                    lineBusinessSubLineBusinessRisk.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessRisk.Description, "SUB_LINE_BUSINESS_CD");
                    if (GetAssociationColumnValueId(lineAssociation.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessRisk(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessRisk);
                    }

                    lineBusinessSubLineBusinessRisk.Description = "QUO.INSURED_OBJECT";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessRisk.Description, "INSURED_OBJECT_ID");
                    SaveByLineBusinessSubLineBusinessRisk(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessRisk);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX_RISK)))//9
                {
                    ByLineBusinessInsuredObjectDTO lineBusinessSubLineBusinessRisk = (ByLineBusinessInsuredObjectDTO)lineAssociation.AssociationType;

                    lineBusinessSubLineBusinessRisk.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessRisk.Description, "PREFIX_CD");
                    if (GetAssociationColumnValueId(lineAssociation.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByPrefixRisk(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessRisk);
                    }
                    lineBusinessSubLineBusinessRisk.Description = "QUO.INSURED_OBJECT";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessRisk.Description, "INSURED_OBJECT_ID");
                    SaveByPrefixRisk(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessRisk);

                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_POLICY_LINE_BUSINESS_SUB_LINE_BUSINESS)))//10
                {
                    ByPolicyLineBusinessSubLineBusinessDTO policyLineBusinessSubLineBusiness = (ByPolicyLineBusinessSubLineBusinessDTO)lineAssociation.AssociationType;

                    policyLineBusinessSubLineBusiness.Description = "ISS.POLICY";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, policyLineBusinessSubLineBusiness.Description, "POLICY_ID");
                    SaveByPolicyLineBusinessSubLineBusiness(lineAssociation.LineAssociationId, associationColumnId, policyLineBusinessSubLineBusiness);

                    policyLineBusinessSubLineBusiness.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, policyLineBusinessSubLineBusiness.Description, "LINE_BUSINESS_CD");
                    SaveByPolicyLineBusinessSubLineBusiness(lineAssociation.LineAssociationId, associationColumnId, policyLineBusinessSubLineBusiness);

                    policyLineBusinessSubLineBusiness.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, policyLineBusinessSubLineBusiness.Description, "SUB_LINE_BUSINESS_CD");
                    SaveByPolicyLineBusinessSubLineBusiness(lineAssociation.LineAssociationId, associationColumnId, policyLineBusinessSubLineBusiness);
                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESSCOVERAGE)))//11
                {
                    ByLineBusinessSubLineBusinessCoverageDTO lineBusinessSubLineBusinessCoverage = (ByLineBusinessSubLineBusinessCoverageDTO)lineAssociation.AssociationType;


                    lineBusinessSubLineBusinessCoverage.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessCoverage.Description, "LINE_BUSINESS_CD");
                    if (GetAssociationColumnValueId(lineAssociation.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessCoverage(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessCoverage);
                    }

                    lineBusinessSubLineBusinessCoverage.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessCoverage.Description, "SUB_LINE_BUSINESS_CD");
                    if (GetAssociationColumnValueId(lineAssociation.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessCoverage(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessCoverage);
                    }

                    lineBusinessSubLineBusinessCoverage.Description = "QUO.COVERAGE";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, lineBusinessSubLineBusinessCoverage.Description, "COVERAGE_ID");
                    SaveByLineBusinessSubLineBusinessCoverage(lineAssociation.LineAssociationId, associationColumnId, lineBusinessSubLineBusinessCoverage);

                }

                if (lineAssociation.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX_PRODUCT)))//12
                {
                    ByPrefixProductDTO prefixProduct = (ByPrefixProductDTO)lineAssociation.AssociationType;

                    prefixProduct.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, prefixProduct.Description, "PREFIX_CD");
                    SaveByPrefixProduct(lineAssociation.LineAssociationId, associationColumnId, prefixProduct);

                    prefixProduct.Description = "PROD.PRODUCT";
                    associationColumnId = GetAssociationColumnId(lineAssociation.AssociationType.LineAssociationTypeId, prefixProduct.Description, "PRODUCT_ID");
                    SaveByPrefixProduct(lineAssociation.LineAssociationId, associationColumnId, prefixProduct);
                }
                return lineAssociation;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLineAssociation);
            }
        }

        public bool ValidateDuplicateLineAssociation(AssociationLineDTO associationLineDTO)
        {
            try
            {
                List<LineAssociationDTO> lineAssociationDTOs = new List<LineAssociationDTO>();
                LineAssociationDTO lineAssociationDTO = new LineAssociationDTO();
                AssociationLineDAO associationLineDAO = new AssociationLineDAO();
                lineAssociationDTO = DTOAssembler.CreateLineAssociationDTOByAssociationLineDTO(associationLineDTO);
                lineAssociationDTOs = associationLineDAO.ValidateDuplicateLineAssociation(lineAssociationDTO.ToModel()).ToDTOs().ToList();
                lineAssociationDTOs.RemoveAll(x => x.LineAssociationId == lineAssociationDTO.LineAssociationId);

                if (lineAssociationDTOs.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateDuplicateContract);
            }
        }

        public LineAssociationDTO UpdateLineAssociation(LineAssociationDTO lineAssociationDTO)
        {
            try
            {
                AssociationLineDAO associationLineDAO = new AssociationLineDAO();
                associationLineDAO.UpdateAssociationLine(lineAssociationDTO.ToModel());
                UpdateAssociationColumnValueByAssociationLineId(lineAssociationDTO);
                return lineAssociationDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLineAssociation);
            }
        }

        public LineAssociationDTO UpdateAssociationColumnValueByAssociationLineId(LineAssociationDTO lineAssociationDTO)
        {
            try
            {
                int associationColumnId = 0;
                bool deleted = true;

                if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESSCOVERAGE)) ||
                    lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX_RISK)) ||
                    lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS_RISK)))  //11
                {
                    deleted = false;
                }
                if (deleted)  //11
                {
                    DeleteAssociationColumnValueByAssociationLineId(lineAssociationDTO.LineAssociationId);
                }

                if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS)))
                {
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, "COMM.LINE_BUSINESS", "LINE_BUSINESS_CD");

                    ByLineBusinessDTO byLineBusinessDTO = (ByLineBusinessDTO)lineAssociationDTO.AssociationType;
                    SaveByLineBusiness(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS)))
                {
                    ByLineBusinessSubLineBusinessDTO byLineBusinessSubLineBusinessDTO = (ByLineBusinessSubLineBusinessDTO)lineAssociationDTO.AssociationType;

                    byLineBusinessSubLineBusinessDTO.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessDTO.Description, "LINE_BUSINESS_CD");
                    SaveByLineBusinessSubLineBusiness(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessDTO);

                    byLineBusinessSubLineBusinessDTO.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessDTO.Description, "SUB_LINE_BUSINESS_CD");
                    SaveByLineBusinessSubLineBusiness(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_OPERATION_TYPE_PREFIX)))
                {
                    ByOperationTypePrefixDTO byOperationTypePrefixDTO = (ByOperationTypePrefixDTO)lineAssociationDTO.AssociationType;

                    byOperationTypePrefixDTO.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byOperationTypePrefixDTO.Description, "PREFIX_CD");
                    SaveByOperationTypePrefix(lineAssociationDTO.LineAssociationId, associationColumnId, byOperationTypePrefixDTO);

                    byOperationTypePrefixDTO.Description = "PARAM.BUSINESS_TYPE";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byOperationTypePrefixDTO.Description, "BUSINESS_TYPE_CD");
                    SaveByOperationTypePrefix(lineAssociationDTO.LineAssociationId, associationColumnId, byOperationTypePrefixDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_INSURED)))
                {
                    ByInsuredDTO byInsuredDTO = (ByInsuredDTO)lineAssociationDTO.AssociationType;

                    byInsuredDTO.Description = "UP.INSURED";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byInsuredDTO.Description, "INSURED_CD");
                    SaveByInsured(lineAssociationDTO.LineAssociationId, associationColumnId, byInsuredDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX)))//5
                {
                    ByPrefixDTO prefix = (ByPrefixDTO)lineAssociationDTO.AssociationType;

                    prefix.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, prefix.Description, "PREFIX_CD");
                    SaveByPrefix(lineAssociationDTO.LineAssociationId, associationColumnId, prefix);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_POLICY)))
                {
                    ByPolicyDTO byPolicyDTO = (ByPolicyDTO)lineAssociationDTO.AssociationType;

                    byPolicyDTO.Description = "ISS.POLICY";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byPolicyDTO.Description, "POLICY_ID");
                    SaveByPolicy(lineAssociationDTO.LineAssociationId, associationColumnId, byPolicyDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_FACULTATIVE_ISSUE)))
                {
                    ByFacultativeIssueDTO byFacultativeIssueDTO = (ByFacultativeIssueDTO)lineAssociationDTO.AssociationType;

                    byFacultativeIssueDTO.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byFacultativeIssueDTO.Description, "PREFIX_CD");
                    SaveByFacultativeIssue(lineAssociationDTO.LineAssociationId, associationColumnId, byFacultativeIssueDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_INSURED_PREFIX)))
                {
                    ByInsuredPrefixDTO byInsuredPrefixDTO = (ByInsuredPrefixDTO)lineAssociationDTO.AssociationType;

                    byInsuredPrefixDTO.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byInsuredPrefixDTO.Description, "PREFIX_CD");
                    SaveByInsuredPrefix(lineAssociationDTO.LineAssociationId, associationColumnId, byInsuredPrefixDTO);

                    byInsuredPrefixDTO.Description = "UP.INSURED";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byInsuredPrefixDTO.Description, "INSURED_CD");
                    SaveByInsuredPrefix(lineAssociationDTO.LineAssociationId, associationColumnId, byInsuredPrefixDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS_RISK)))
                {
                    ByLineBusinessSubLineBusinessInsuredObjectDTO byLineBusinessSubLineBusinessInsuredObjectDTO = (ByLineBusinessSubLineBusinessInsuredObjectDTO)lineAssociationDTO.AssociationType;

                    byLineBusinessSubLineBusinessInsuredObjectDTO.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessInsuredObjectDTO.Description, "LINE_BUSINESS_CD");

                    if (GetAssociationColumnValueId(lineAssociationDTO.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessRisk(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessInsuredObjectDTO);
                    }
                    byLineBusinessSubLineBusinessInsuredObjectDTO.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessInsuredObjectDTO.Description, "SUB_LINE_BUSINESS_CD");
                    if (GetAssociationColumnValueId(lineAssociationDTO.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessRisk(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessInsuredObjectDTO);
                    }

                    byLineBusinessSubLineBusinessInsuredObjectDTO.Description = "QUO.INSURED_OBJECT";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessInsuredObjectDTO.Description, "INSURED_OBJECT_ID");
                    SaveByLineBusinessSubLineBusinessRisk(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessInsuredObjectDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX_RISK)))
                {
                    ByLineBusinessInsuredObjectDTO byLineBusinessInsuredObjectDTO = (ByLineBusinessInsuredObjectDTO)lineAssociationDTO.AssociationType;

                    byLineBusinessInsuredObjectDTO.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessInsuredObjectDTO.Description, "PREFIX_CD");
                    if (GetAssociationColumnValueId(lineAssociationDTO.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByPrefixRisk(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessInsuredObjectDTO);
                    }

                    byLineBusinessInsuredObjectDTO.Description = "QUO.INSURED_OBJECT";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessInsuredObjectDTO.Description, "INSURED_OBJECT_ID");
                    SaveByPrefixRisk(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessInsuredObjectDTO);

                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_POLICY_LINE_BUSINESS_SUB_LINE_BUSINESS)))
                {
                    ByPolicyLineBusinessSubLineBusinessDTO byPolicyLineBusinessSubLineBusinessDTO = (ByPolicyLineBusinessSubLineBusinessDTO)lineAssociationDTO.AssociationType;

                    byPolicyLineBusinessSubLineBusinessDTO.Description = "ISS.POLICY";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byPolicyLineBusinessSubLineBusinessDTO.Description, "POLICY_ID");
                    SaveByPolicyLineBusinessSubLineBusiness(lineAssociationDTO.LineAssociationId, associationColumnId, byPolicyLineBusinessSubLineBusinessDTO);

                    byPolicyLineBusinessSubLineBusinessDTO.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byPolicyLineBusinessSubLineBusinessDTO.Description, "LINE_BUSINESS_CD");
                    SaveByPolicyLineBusinessSubLineBusiness(lineAssociationDTO.LineAssociationId, associationColumnId, byPolicyLineBusinessSubLineBusinessDTO);

                    byPolicyLineBusinessSubLineBusinessDTO.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byPolicyLineBusinessSubLineBusinessDTO.Description, "SUB_LINE_BUSINESS_CD");
                    SaveByPolicyLineBusinessSubLineBusiness(lineAssociationDTO.LineAssociationId, associationColumnId, byPolicyLineBusinessSubLineBusinessDTO);
                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESSCOVERAGE)))
                {
                    ByLineBusinessSubLineBusinessCoverageDTO byLineBusinessSubLineBusinessCoverageDTO = (ByLineBusinessSubLineBusinessCoverageDTO)lineAssociationDTO.AssociationType;

                    byLineBusinessSubLineBusinessCoverageDTO.Description = "COMM.LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessCoverageDTO.Description, "LINE_BUSINESS_CD");
                    if (GetAssociationColumnValueId(lineAssociationDTO.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessCoverage(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessCoverageDTO);
                    }

                    byLineBusinessSubLineBusinessCoverageDTO.Description = "COMM.SUB_LINE_BUSINESS";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessCoverageDTO.Description, "SUB_LINE_BUSINESS_CD");

                    if (GetAssociationColumnValueId(lineAssociationDTO.LineAssociationId, associationColumnId) == 0)
                    {
                        SaveByLineBusinessSubLineBusinessCoverage(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessCoverageDTO);
                    }

                    byLineBusinessSubLineBusinessCoverageDTO.Description = "QUO.COVERAGE";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byLineBusinessSubLineBusinessCoverageDTO.Description, "COVERAGE_ID");
                    SaveByLineBusinessSubLineBusinessCoverage(lineAssociationDTO.LineAssociationId, associationColumnId, byLineBusinessSubLineBusinessCoverageDTO);


                }
                else if (lineAssociationDTO.AssociationType.LineAssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue(ReinsuranceAssociationTypes.REINS_BY_PREFIX_PRODUCT)))
                {
                    ByPrefixProductDTO byPrefixProductDTO = (ByPrefixProductDTO)lineAssociationDTO.AssociationType;

                    byPrefixProductDTO.Description = "COMM.PREFIX";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byPrefixProductDTO.Description, "PREFIX_CD");
                    SaveByPrefixProduct(lineAssociationDTO.LineAssociationId, associationColumnId, byPrefixProductDTO);

                    byPrefixProductDTO.Description = "PROD.PRODUCT";
                    associationColumnId = GetAssociationColumnId(lineAssociationDTO.AssociationType.LineAssociationTypeId, byPrefixProductDTO.Description, "PRODUCT_ID");
                    SaveByPrefixProduct(lineAssociationDTO.LineAssociationId, associationColumnId, byPrefixProductDTO);
                }

                return lineAssociationDTO;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLineAssociation);
            }

        }

        public List<AffectationTypeDTO> GetAffectationTypes()
        {
            try
            {
                AffectationTypeDAO affectationTypeDAO = new AffectationTypeDAO();
                return affectationTypeDAO.GetAffectationTypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAffectationTypes);
            }
        }

        public List<AssociationLineDTO> GetAssociationLine(int year, int associationTypeId, int associationLineId)
        {
            try
            {
                AssociationLineDAO associationLineDAO = new AssociationLineDAO();
                return associationLineDAO.GetAssociationLine(year, associationTypeId, associationLineId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationLine);
            }
        }

        public List<ContractDTO> GetContractsByYearAndContractTypeId(int year, int contractTypeId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ContractDTO> contractsDTO = new List<ContractDTO>();
                List<CurrencyDTO> currenciesDTO = new List<CurrencyDTO>();
                contractsDTO = DTOAssembler.ToDTOs(contractDAO.GetContractsByYearAndContractTypeId(year, contractTypeId)).ToList();
                currenciesDTO = GetCurrencies();
                contractsDTO.ForEach(x =>
                {
                    x.Currency = currenciesDTO.Where(y => y.Id == x.Currency.Id).FirstOrDefault();
                });

                ContractTypeDTO contractTypeDTO = GetContractTypeByContractTypeId(contractTypeId);
                return reinsuranceBusiness.GetContractsByYearAndContractTypeId(year, contractTypeId, contractsDTO, contractTypeDTO).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractsByYearAndContractTypeId);
            }
        }

        public List<ContractDTO> GetEnabledContracts()
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return DTOAssembler.ToDTOs(contractDAO.GetEnabledContracts()).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetEnabledContracts);
            }
        }

        public List<ContractLineDTO> GetContractLines()
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                return contractLineDAO.GetContractLines().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractLines);
            }
        }

        public List<ContractTypeDTO> GetContractTypes()
        {
            try
            {
                ContractTypeDAO contractTypeDAO = new ContractTypeDAO();
                return contractTypeDAO.GetContractTypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractTypes);
            }
        }

        public List<CumulusTypeDTO> GetCumulusTypes()
        {
            try
            {
                CumulusTypeDAO cumulusTypeDAO = new CumulusTypeDAO();
                return cumulusTypeDAO.GetCumulusTypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusTypes);
            }
        }

        public List<EPITypeDTO> GetEPITypes()
        {
            try
            {
                EpiTypeDAO epiTypeDAO = new EpiTypeDAO();
                return epiTypeDAO.GetEPITypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetEPITypes);
            }
        }

        public List<InstallmentDTO> GetInstallmentsByLevelCompanyId(int levelCompanyId)
        {
            try
            {
                InstallmentDAO installmentDAO = new InstallmentDAO();
                return installmentDAO.GetInstallmentsByLevelCompanyId(levelCompanyId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetInstallmentsByLevelCompanyId);
            }
        }

        public List<LevelDTO> GetLevelsByContractId(int contractId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return DTOAssembler.ToDTOs(levelDAO.GetLevelsByContractId(contractId)).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelsByContractId);
            }
        }

        public List<LevelCompanyDTO> GetLevelCompaniesByLevelId(int levelId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return DTOAssembler.ToDTOs(levelDAO.GetLevelCompaniesByLevelId(levelId)).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelCompaniesByLevelId);
            }
        }

        public List<LevelPaymentDTO> GetLevelPaymentsByLevelId(int levelId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetLevelPaymentsByLevelId(levelId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelPaymentsByLevelId);
            }
        }

        public List<LevelRestoreDTO> GetLevelRestoresByLevelId(int levelId)
        {
            try
            {
                LevelRestoreDAO levelRestoreDAO = new LevelRestoreDAO();
                return levelRestoreDAO.GetLevelRestoresByLevelId(levelId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelRestoresByLevelId);
            }
        }

        public List<LineDTO> GetLines()
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                return lineDAO.GetLines().FindAll(x => x.Description != "LINEA DE RETENCIÓN TOTAL").ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLines);
            }
        }

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

        public List<LineDTO> GetReinsuranceLines()
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                return DTOAssembler.ToDTOs(lineDAO.GetReinsuranceLines()).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceLines);
            }
        }

        public List<LineAssociationTypeDTO> GetAssociationColumnByAssociationTypeId(int associationTypeId)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                return associationColumnValueDAO.GetAssociationColumnByAssociationTypeId(associationTypeId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationColumnByAssociationTypeId);
            }
        }

        public List<LineAssociationTypeDTO> GetAssociationTypes()
        {
            try
            {
                AssociationTypeDAO associationTypeDAO = new AssociationTypeDAO();
                return associationTypeDAO.GetAssociationTypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationTypes);
            }
        }

        public List<LineAssociationTypeDTO> GetLineOfBusinesses()
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                return associationColumnValueDAO.GetLineOfBusinesses().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineOfBusinesses);
            }
        }

        public List<LineCumulusTypeDTO> GetLineCumulusType()
        {
            try
            {
                CumulusTypeDAO cumulusTypeDAO = new CumulusTypeDAO();
                return cumulusTypeDAO.GetLineCumulusType().Where(x => x.Description != "LINEA DE RETENCIÓN TOTAL").ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineCumulusType);
            }
        }

        public List<ModuleDTO> GetModules()
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.GetModules().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetModules);
            }
        }

        public List<ReinsurancePrefixDTO> GetReinsurancePrefixes()
        {
            try
            {
                LineBusinessReinsuranceDAO lineBusinessReinsuranceDAO = new LineBusinessReinsuranceDAO();
                return lineBusinessReinsuranceDAO.GetLineBusinessReinsurances().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsurancePrefixes);
            }
        }

        public List<ResettlementTypeDTO> GetResettlementTypes()
        {
            try
            {
                ResettlementTypeDAO resettlementTypeDAO = new ResettlementTypeDAO();
                return resettlementTypeDAO.GetResettlementTypes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetResettlementTypes);
            }
        }

        public List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            try
            {
                List<CoverageDTO> coverageDTOs = new List<CoverageDTO>();
                coverageDTOs = DelegateService.underwritingIntegrationService.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId)
                                .Select(x => new CoverageDTO { Id = x.Id, Description = x.Description })
                                .OrderByDescending(x => x.Description).ToList();
                return coverageDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCoveragesByLineBusinessIdSubLineBusinessId);
            }
        }

        public List<InsuredObjectDTO> GetInsuredObjectByPrefixIdList(int prefixId)
        {
            try
            {
                List<InsuredObjectDTO> insuredObjectDTOs = new List<InsuredObjectDTO>();
                insuredObjectDTOs = DelegateService.underwritingIntegrationService.GetInsuredObjectByPrefixIdList(prefixId)
                                    .Select(x => new InsuredObjectDTO { Id = x.Id, Description = x.Description })
                                    .OrderByDescending(x => x.Description).ToList();
                return insuredObjectDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetInsuredObjectByPrefixIdList);
            }
        }

        public List<ModuleDateDTO> GetModuleDates()
        {
            try
            {
                List<ModuleDateDTO> moduleDateDTOs = new List<ModuleDateDTO>();
                moduleDateDTOs = ModelAssembler.CreateModuleDTOIntegration(DelegateService.tempCommonIntegrationService.GetModuleDates());
                return moduleDateDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetModuleDates);
            }
        }

        public List<CurrencyDTO> GetCurrencies()
        {
            try
            {
                List<CurrencyDTO> currencyDTOs = new List<CurrencyDTO>();
                currencyDTOs = DelegateService.commonIntegrationService.GetCurrencies().ToDTOs().ToList();
                return currencyDTOs;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCurrencies);
            }
        }

        public InsuredDTO GetInsuredByIndividualId(int individualId)
        {
            try
            {
                InsuredDTO insuredDTO = new InsuredDTO();
                insuredDTO = ModelAssembler.CreateInsuredByIndividualId(DelegateService.uniquePersonIntegrationService.GetInsuredByIndividualId(individualId));
                return insuredDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetInsuredByIndividualId);
            }
        }

        public List<IndividualDTO> GetReinsurerByName(string name, int reinsurance, int foreignReinsurance)
        {
            try
            {
                List<IndividualDTO> individualDTOs = new List<IndividualDTO>();
                individualDTOs = ModelAssembler.CreateMapIndividualIntegration(DelegateService.tempCommonIntegrationService.GetReinsurerByName(name, reinsurance, foreignReinsurance));
                return individualDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsurerByName);
            }
        }

        public List<BranchDTO> GetBranches()
        {
            try
            {
                List<BranchDTO> branchDTOs = new List<BranchDTO>();
                branchDTOs = DelegateService.commonIntegrationService.GetBranches().ToDTOs().OrderBy(b => b.Description).ToList();
                return branchDTOs;
            }
            catch (BusinessException)
            {

                throw new BusinessException(Resources.Resources.ErrorGetBranches);
            }
        }

        public List<PrefixDTO> GetPrefixes()
        {
            try
            {
                List<PrefixDTO> prefixDTOs = new List<PrefixDTO>();
                prefixDTOs = DelegateService.commonIntegrationService.GetPrefixes().ToDTOs().OrderBy(p => p.Description).ToList();
                return prefixDTOs;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPrefixes);
            }
        }

        public ReinsurancePrefixDTO SaveReinsurancePrefix(ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            try
            {
                LineBusinessReinsuranceDAO lineBusinessReinsuranceDAO = new LineBusinessReinsuranceDAO();
                return lineBusinessReinsuranceDAO.SaveLineBusinessReinsurance(reinsurancePrefixDTO.ToModel()).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveReinsurancePrefix);
            }
        }

        public ReinsurancePrefixDTO UpdateReinsurancePrefix(ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            try
            {
                LineBusinessReinsuranceDAO lineBusinessReinsuranceDAO = new LineBusinessReinsuranceDAO();
                return lineBusinessReinsuranceDAO.UpdateLineBusinessReinsurance(reinsurancePrefixDTO.ToModel()).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateReinsurancePrefix);
            }
        }

        public void DeleteAssociationColumnValue(int associationColumnValueId)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                associationColumnValueDAO.DeleteAssociationColumnValue(associationColumnValueId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteAssociationColumnValue);
            }
        }

        public void DeleteAssociationColumnValueByAssociationLineId(int associationLineId)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                associationColumnValueDAO.DeleteAssociationColumnValueByAssociationLineId(associationLineId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteAssociationColumnValueByAssociationLineId);
            }
        }

        public void DeleteAssociationLine(int associationLineId)
        {
            try
            {
                AssociationLineDAO associationLineDAO = new AssociationLineDAO();
                associationLineDAO.DeleteAssociationLine(associationLineId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteAssociationLine);
            }
        }

        public void DeleteContract(int contractId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                contractDAO.DeleteContract(contractId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteContract);
            }
        }

        public void DeleteContractLine(int contractLineId)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                contractLineDAO.DeleteContractLine(contractLineId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteContractLine);
            }
        }

        public void DeleteInstallment(InstallmentDTO installmentDTO)
        {
            try
            {
                InstallmentDAO installmentDAO = new InstallmentDAO();
                installmentDAO.DeleteInstallment(installmentDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteInstallment);
            }
        }

        public void DeleteLevel(int levelId)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                levelDAO.DeleteLevel(levelId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteLevel);
            }
        }

        public void DeleteLevelCompany(int levelCompanyId)
        {
            try
            {
                LevelCompanyDAO levelCompanyDAO = new LevelCompanyDAO();
                levelCompanyDAO.DeleteLevelCompany(levelCompanyId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteLevelCompany);
            }
        }

        public void DeleteLevelPayment(int levelPaymentId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                levelPaymentDAO.DeleteLevelPayment(levelPaymentId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteLevelPayment);
            }
        }

        public void DeleteLevelRestore(int levelRestoreId)
        {
            try
            {
                LevelRestoreDAO levelRestoreDAO = new LevelRestoreDAO();
                levelRestoreDAO.DeleteLevelRestore(levelRestoreId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteLevelRestore);
            }
        }

        public void DeleteLine(int lineId)
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                lineDAO.DeleteLine(lineId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteLine);
            }
        }

        public void SaveByFacultativeIssue(int associationLineId, int associationColumnId, ByFacultativeIssueDTO facultativeDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                if (facultativeDTO.Description == "COMM.PREFIX")
                {
                    foreach (PrefixDTO facultativeItem in facultativeDTO.Prefixes)
                    {
                        associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, facultativeItem.Id);
                    }
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByFacultativeIssue);
            }
        }

        public void SaveByInsured(int associationLineId, int associationColumnId, ByInsuredDTO insuredDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                if (insuredDTO.Description == "UP.INSURED")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, insuredDTO.Insured.IndividualId);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByInsured);
            }
        }

        public void SaveByPrefix(int associationLineId, int associationColumnId, ByPrefixDTO prefixDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                if (prefixDTO.Description == "COMM.PREFIX")
                {
                    foreach (PrefixDTO prefix in prefixDTO.Prefix)
                    {
                        associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, prefix.Id);
                    }
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByInsured);
            }
        }

        public void SaveByInsuredPrefix(int associationLineId, int associationColumnId, ByInsuredPrefixDTO insuredPrefixDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                if (insuredPrefixDTO.Description == "COMM.PREFIX")
                {
                    foreach (PrefixDTO prefixItem in insuredPrefixDTO.Prefixes)
                    {
                        associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, prefixItem.Id);
                    }
                }
                else if (insuredPrefixDTO.Description == "UP.INSURED")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, insuredPrefixDTO.Insured.IndividualId);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByInsuredPrefix);
            }
        }

        public void SaveByLineBusiness(int associationLineId, int associationColumnId, ByLineBusinessDTO byLineBusinessDTO)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                foreach (LineBusinessDTO lineBusinessDTO in byLineBusinessDTO.LineBusiness)
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessDTO.Id);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByLineBusiness);
            }
        }

        public void SaveByLineBusinessSubLineBusiness(int associationLineId, int associationColumnId, ByLineBusinessSubLineBusinessDTO lineBusinessSubLineBusinessDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                if (lineBusinessSubLineBusinessDTO.Description == "COMM.LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessDTO.LineBusiness.Id);
                }
                else if (lineBusinessSubLineBusinessDTO.Description == "COMM.SUB_LINE_BUSINESS")
                {
                    foreach (SubLineBusinessDTO lineBusinessItem in lineBusinessSubLineBusinessDTO.SubLineBusiness)
                    {
                        associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessItem.Id);
                    }
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByLineBusinessSubLineBusiness);
            }
        }

        public void SaveByLineBusinessSubLineBusinessCoverage(int associationLineId, int associationColumnId, ByLineBusinessSubLineBusinessCoverageDTO lineBusinessSubLineBusinessCoverageDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                if (lineBusinessSubLineBusinessCoverageDTO.Description == "COMM.LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessCoverageDTO.LineBusiness.Id);
                }
                else if (lineBusinessSubLineBusinessCoverageDTO.Description == "COMM.SUB_LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessCoverageDTO.SubLineBusiness.Id);
                }
                else if (lineBusinessSubLineBusinessCoverageDTO.Description == "QUO.COVERAGE")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessCoverageDTO.Coverage[0].Id);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByLineBusinessSubLineBusinessCoverage);
            }
        }

        public void SaveByLineBusinessSubLineBusinessRisk(int associationLineId, int associationColumnId, ByLineBusinessSubLineBusinessInsuredObjectDTO lineBusinessSubLineBusinessRiskDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                if (lineBusinessSubLineBusinessRiskDTO.Description == "COMM.LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessRiskDTO.LineBusiness.Id);
                }
                else if (lineBusinessSubLineBusinessRiskDTO.Description == "COMM.SUB_LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessRiskDTO.SubLineBusiness.Id);
                }
                else if (lineBusinessSubLineBusinessRiskDTO.Description == "QUO.INSURED_OBJECT")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, lineBusinessSubLineBusinessRiskDTO.InsuredObject[0].Id);   //.Risk.Id
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByLineBusinessSubLineBusinessRisk);
            }
        }

        public void SaveByOperationTypePrefix(int associationLineId, int associationColumnId, ByOperationTypePrefixDTO operationTypePrefixDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();
                if (operationTypePrefixDTO.Description == "COMM.PREFIX")
                {
                    foreach (PrefixDTO prefixDTO in operationTypePrefixDTO.Prefixes)
                    {
                        associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, prefixDTO.Id);
                    }
                }
                else if (operationTypePrefixDTO.Description == "PARAM.BUSINESS_TYPE")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, Convert.ToInt32(operationTypePrefixDTO.BusinessType));
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByOperationTypePrefix);
            }
        }

        public void SaveByPolicy(int associationLineId, int associationColumnId, ByPolicyDTO policyDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                if (policyDTO.Description == "ISS.POLICY")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, policyDTO.Policy.Id);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByPolicy);
            }
        }

        public void SaveByPolicyLineBusinessSubLineBusiness(int associationLineId, int associationColumnId, ByPolicyLineBusinessSubLineBusinessDTO policyLineBusinessSubLineBusinessDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                if (policyLineBusinessSubLineBusinessDTO.Description == "ISS.POLICY")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, policyLineBusinessSubLineBusinessDTO.Policy.Id);
                }
                else if (policyLineBusinessSubLineBusinessDTO.Description == "COMM.LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, policyLineBusinessSubLineBusinessDTO.LineBusiness.Id);
                }
                else if (policyLineBusinessSubLineBusinessDTO.Description == "COMM.SUB_LINE_BUSINESS")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, policyLineBusinessSubLineBusinessDTO.SubLineBusiness.Id);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByPolicyLineBusinessSubLineBusiness);
            }
        }

        public void SaveByPrefixProduct(int associationLineId, int associationColumnId, ByPrefixProductDTO prefixProductDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                if (prefixProductDTO.Description == "COMM.PREFIX")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, prefixProductDTO.Prefix.Id);
                }
                else if (prefixProductDTO.Description == "PROD.PRODUCT")
                {
                    foreach (ProductDTO productDTO in prefixProductDTO.Products)
                    {
                        associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, Convert.ToInt32(productDTO.Id));
                    }
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByPrefixProduct);
            }
        }

        public void SaveByPrefixRisk(int associationLineId, int associationColumnId, ByLineBusinessInsuredObjectDTO prefixRiskDTO)
        {
            try
            {
                AssociationColumnValueDAO associationColumnValueDAO = new AssociationColumnValueDAO();

                if (prefixRiskDTO.Description == "COMM.PREFIX")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, prefixRiskDTO.LineBusiness.Id); //prefixRisk.Prefix.Id
                }
                else if (prefixRiskDTO.Description == "QUO.INSURED_OBJECT")
                {
                    associationColumnValueDAO.SaveAssociationColumnValue(associationLineId, associationColumnId, prefixRiskDTO.InsuredObject[0].Id);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveByPrefixRisk);
            }
        }

        public int SaveContract(ContractDTO contractDTO)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return contractDAO.SaveContract(contractDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveContract);
            }
        }

        public void SaveContractLine(LineDTO lineDTO)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                contractLineDAO.SaveContractLine(lineDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveContractLine);
            }
        }

        public void SaveInstallment(InstallmentDTO installmentDTO)
        {
            try
            {
                InstallmentDAO installmentDAO = new InstallmentDAO();
                installmentDAO.SaveInstallment(installmentDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveInstallment);
            }
        }

        public int SaveLevel(LevelDTO levelDTO)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                return levelDAO.SaveLevel(levelDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLevel);
            }
        }

        public int SaveLevelCompany(LevelCompanyDTO levelCompanyDTO)
        {
            try
            {
                LevelCompanyDAO levelCompanyDAO = new LevelCompanyDAO();
                return levelCompanyDAO.SaveLevelCompany(levelCompanyDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLevelCompany);
            }
        }

        public void SaveLevelPayment(LevelPaymentDTO levelPaymentDTO)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                levelPaymentDAO.SaveLevelPayment(levelPaymentDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLevelPayment);
            }
        }

        public void SaveLevelRestore(LevelRestoreDTO levelRestoreDTO)
        {
            try
            {
                LevelRestoreDAO levelRestoreDAO = new LevelRestoreDAO();
                levelRestoreDAO.SaveLevelRestore(levelRestoreDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLevelRestore);
            }
        }

        public void SaveLine(LineDTO lineDTO)
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                lineDAO.SaveLine(lineDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLine);
            }
        }

        public void UpdateContract(ContractDTO contractDTO)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                contractDAO.UpdateContract(contractDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateContract);
            }
        }

        public void UpdateContractLine(LineDTO contractLineDTO)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                contractLineDAO.UpdateContractLine(contractLineDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateContractLine);
            }
        }

        public void UpdateInstallment(InstallmentDTO installmentDTO)
        {
            try
            {
                InstallmentDAO installmentDAO = new InstallmentDAO();
                installmentDAO.UpdateInstallment(installmentDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateInstallment);
            }
        }

        public void UpdateLevel(LevelDTO levelDTO)
        {
            try
            {
                LevelDAO levelDAO = new LevelDAO();
                levelDAO.UpdateLevel(levelDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLevel);
            }
        }

        public void UpdateLevelCompany(LevelCompanyDTO levelCompanyDTO)
        {
            try
            {
                LevelCompanyDAO levelCompanyDAO = new LevelCompanyDAO();
                levelCompanyDAO.UpdateLevelCompany(levelCompanyDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLevelCompany);
            }
        }

        public void UpdateLevelPayment(LevelPaymentDTO levelPaymentDTO)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                levelPaymentDAO.UpdateLevelPayment(levelPaymentDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLevelPayment);
            }
        }

        public void UpdateLevelRestore(LevelRestoreDTO levelRestoreDTO)
        {
            try
            {
                LevelRestoreDAO levelRestoreDAO = new LevelRestoreDAO();
                levelRestoreDAO.UpdateLevelRestore(levelRestoreDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLevelRestore);
            }
        }

        public void UpdateLine(LineDTO lineDTO)
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                lineDAO.UpdateLine(lineDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateLine);
            }
        }

        public ContractTypeDTO GetContractTypeByContractTypeId(int contractTypeId)
        {
            try
            {
                List<ContractTypeDTO> contractTypesDTO = GetContractTypes();
                ContractTypeDTO contractTypeDTO = new ContractTypeDTO();
                contractTypeDTO = contractTypesDTO.Where(ct => ct.ContractTypeId == contractTypeId).ToList().FirstOrDefault();
                return contractTypeDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractTypeByContractTypeId);
            }
        }

        public List<ContractDTO> GetEnabledContractsByGroupContract(string groupContract)
        {
            try
            {
                List<ContractDTO> contractEnables = GetEnabledContracts();
                List<ContractDTO> contractFind = contractEnables.Where(ct => ct.SmallDescription == groupContract).ToList();
                return contractFind;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetEnabledContractsByGroupContract);
            }
        }

        public ContractDTO AddContract(int contractId, int contractYear, int contractTypeId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ContractTypeDTO contractTypeDTO = GetContractTypeByContractTypeId(contractTypeId);
                ContractDTO contractDTO = new ContractDTO();
                List<ContractDTO> contractDTOs = new List<ContractDTO>();

                if (!contractId.Equals(0))
                {
                    contractDTO = GetContractById(contractId);
                    contractDTO.ContractType = contractTypeDTO;
                    contractDTOs = GetEnabledContractsByGroupContract(contractDTO.GroupContract);
                    if (contractDTOs.Count > 0)
                    {
                        contractDTO.GroupContract = contractDTOs.Where(x => x.SmallDescription == contractDTO.GroupContract).FirstOrDefault().ContractId.ToString();
                    }
                    else
                    {
                        contractDTO.GroupContract = contractDTO.ContractId.ToString();
                    }
                }
                else
                {
                    contractDTO = DTOAssembler.ToDTO(reinsuranceBusiness.AddContract(contractId, contractYear, contractTypeDTO));
                }

                return contractDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorAddContract);
            }
        }

        public List<ContractTypeDTO> GetContractTypeEnabled()
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ContractTypeDTO> contractTypes = GetContractTypes();
                return reinsuranceBusiness.GetContractTypeEnabled(contractTypes).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractTypeEnabled);
            }
        }

        public List<ContractTypeDTO> GetContractFuncionalityId(int contractTypeId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ContractTypeDTO> contractTypes = GetContractTypes();
                return reinsuranceBusiness.GetContractFuncionalityId(contractTypeId, contractTypes).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractFuncionalityId);
            }
        }

        public List<ContractDTO> GetCurrentPeriodContracts(int year)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ContractDTO> contracts = GetEnabledContracts();
                return DTOAssembler.ToDTOs(reinsuranceBusiness.GetCurrentPeriodContracts(year, contracts)).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCurrentPeriodContracts);
            }
        }

        public int ValidateBeforeDeleteContract(int contractId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<LevelDTO> levelsDTO = GetLevelsByContractId(contractId);
                List<ContractLineDTO> contractLinesDTO = GetContractLines();
                return reinsuranceBusiness.ValidateBeforeDeleteContract(contractId, levelsDTO, contractLinesDTO);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateBeforeDeleteContract);
            }

        }

        public int GetNextLevelNumberByLevelId(int levelId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<LevelPaymentDTO> levelPaymentsDTO = GetLevelPaymentsByLevelId(levelId);
                return reinsuranceBusiness.GetNextLevelNumberByLevelId(levelId, levelPaymentsDTO);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetNextLevelNumberByLevelId);
            }
        }

        public int GetNextNumberRestoreByLevelId(int levelId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<LevelRestoreDTO> levelRestoresDTO = GetLevelRestoresByLevelId(levelId);
                return reinsuranceBusiness.GetNextNumberRestoreByLevelId(levelId, levelRestoresDTO);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetNextNumberRestoreByLevelId);
            }
        }

        public bool DeleteLineByLineId(int lineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                LineDTO lineDTO = GetContractLineByLineId(lineId);
                if (lineDTO.ContractLines.Count.Equals(0))
                {
                    DeleteLine(lineId);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteLineByLineId);
            }
        }

        public LineDTO AddLine(int lineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                LineDTO lineDTO = new LineDTO();
                List<ModuleDateDTO> moduleDates = new List<ModuleDateDTO>();
                moduleDates = GetModuleDates();

                if (!lineId.Equals(0))
                {
                    lineDTO = GetLineByLineId(lineId);
                }
                return DTOAssembler.ToDTO(reinsuranceBusiness.AddLine(lineId, moduleDates, lineDTO));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorAddLine);
            }
        }

        public LineDTO AddContractLine(int contractLineId, int lineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ContractLineDTO contractLineDTO = new ContractLineDTO();
                ContractDTO contractDTO = new ContractDTO();
                LineDTO lineDTO = new LineDTO();

                if (!contractLineId.Equals(0))
                {
                    contractLineDTO = GetContractLineByContractLineId(contractLineId);
                    contractDTO = GetContractById(contractLineDTO.Contract.ContractId);
                }
                else
                {
                    lineDTO = GetContractLineByLineId(lineId);
                    if (lineDTO.ContractLines.Count.Equals(0))
                    {
                        contractLineDTO.Priority = 1;
                    }
                    else
                    {
                        contractLineDTO.Priority = (lineDTO.ContractLines[lineDTO.ContractLines.Count - 1].Priority) + 1;
                    }
                }
                return DTOAssembler.ToDTO(reinsuranceBusiness.AddContractLine(contractLineId, lineId, contractLineDTO, contractDTO));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorAddContractLine);
            }
        }

        public bool SaveContractLineByLine(LineDTO lineDTO)
        {
            try
            {
                LineDTO contractLineDTO = new LineDTO();
                contractLineDTO = GetContractLineByLineId(lineDTO.LineId);

                // Se recupera contracLine
                bool isContractExist = false;
                foreach (ContractLineDTO contractLineDto in contractLineDTO.ContractLines)
                {
                    if (contractLineDto.Contract.ContractId == lineDTO.ContractLines[0].Contract.ContractId)
                    {
                        isContractExist = true;
                        break;
                    }
                }

                // Save data in db.
                if (lineDTO.ContractLines[0].ContractLineId.Equals(0))
                {
                    if (lineDTO.ContractLines[0].Contract.ContractId > 0)
                    {
                        if (isContractExist)
                        {
                            return false;
                        }
                        else
                        {
                            SaveContractLine(lineDTO);
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (lineDTO.ContractLines[0].Priority != 0 && lineDTO.ContractLines[0].Contract.ContractId > 0)
                    {
                        if (isContractExist)
                        {
                            return false;
                        }
                        else
                        {
                            UpdateContractLine(lineDTO);
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveContractLineByLine);
            }
        }

        public bool LineIsUsed(int lineId)
        {
            try
            {
                ContractLineDAO contractLineDAO = new ContractLineDAO();
                return contractLineDAO.LineIsUsed(lineId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteContractLine);
            }
        }

        public bool DeleteContractLineByLine(int contractLineId, int lineId)
        {
            try
            {
                LineDTO lineDTO = new LineDTO();
                int cont = 1;
                lineDTO = GetContractLineByLineId(lineId);
                foreach (ContractLineDTO contractLineDTO in lineDTO.ContractLines)
                {
                    if (contractLineDTO.ContractLineId == contractLineId)
                    {
                        if (cont == lineDTO.ContractLines.Count)
                        {
                            DeleteContractLine(contractLineId);
                            return true;
                        }
                    }
                    else
                    {
                        cont++;
                    }
                }
                return false;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteContractLineByLine);
            }
        }

        public List<LineAssociationTypeDTO> GetLineAssociationTypes()
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<LineAssociationTypeDTO> lineAssociationTypes = new List<LineAssociationTypeDTO>();
                lineAssociationTypes = GetAssociationTypes().Where(x => x.Description != "RETENIDO POR EMISION").ToList();
                return lineAssociationTypes.Where(x => x.Enabled == true).OrderByDescending(x => x.Priority).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineAssociationTypes);
            }
        }

        public bool DeleteAssociationLines(int associationLineId)
        {
            try
            {
                // Borra detalle
                DeleteAssociationColumnValueByAssociationLineId(associationLineId);
                // Borra cabecera
                DeleteAssociationLine(associationLineId);
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteAssociationLines);
            }
        }

        public List<LineBusinessDTO> GetLineBusiness()
        {
            try
            {
                List<LineBusinessDTO> lineBusinessDTO = new List<LineBusinessDTO>();
                lineBusinessDTO = DelegateService.commonIntegrationService.GetLinesBusiness().OrderBy(x => x.Description).ToDTOs().ToList();
                return lineBusinessDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineBusiness);
            }
        }

        public List<SubLineBusinessDTO> GetSubLineBusiness(int lineBusiness)
        {
            try
            {
                List<SubLineBusinessDTO> subLineBusiness = new List<SubLineBusinessDTO>();
                subLineBusiness = DelegateService.commonIntegrationService.GetSubLineBusinessByLineBusinessId()
                                  .Where(sl => sl.LineBusinessId == lineBusiness).OrderBy(x => x.Description).ToDTOs().ToList();
                return subLineBusiness;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetSubLineBusiness);
            }
        }

        public int SaveLineAssociationByAssociationLine(AssociationLineDTO associationLineDTO)
        {
            try
            {
                // GRABACIÓN
                int newLineAssociationId = 0;
                bool someExecute = false;

                LineAssociationDTO lineAssociationDTO = new LineAssociationDTO();
                lineAssociationDTO.Line = new LineDTO();
                lineAssociationDTO.Line.LineId = associationLineDTO.LineId;
                lineAssociationDTO.DateFrom = Convert.ToDateTime(associationLineDTO.DateFrom);
                lineAssociationDTO.DateTo = Convert.ToDateTime(associationLineDTO.DateTo);
                lineAssociationDTO.AssociationType = new LineAssociationTypeDTO();
                // 1. POR RAMO(S) TECNICO
                if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS)))
                {
                    lineAssociationDTO.AssociationType = new ByLineBusinessDTO();
                    ByLineBusinessDTO byLineBusinessDTO = new ByLineBusinessDTO();
                    byLineBusinessDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;
                    byLineBusinessDTO.LineBusiness = new List<LineBusinessDTO>();

                    foreach (LineBusinessDTO lineBusinessDTO in associationLineDTO.ByLineBusiness.LineBusiness)
                    {
                        byLineBusinessDTO.LineBusiness.Add(lineBusinessDTO);
                    }

                    lineAssociationDTO.AssociationType = byLineBusinessDTO;

                }
                // 2.  POR RAMO / SUB RAMO(S) T�CNICO   
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS)))
                {
                    lineAssociationDTO.AssociationType = new ByLineBusinessSubLineBusinessDTO();
                    ByLineBusinessSubLineBusinessDTO byLineBusinessSubLineBusinessDTO = new ByLineBusinessSubLineBusinessDTO();
                    byLineBusinessSubLineBusinessDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byLineBusinessSubLineBusinessDTO.LineBusiness = new LineBusinessDTO();
                    byLineBusinessSubLineBusinessDTO.LineBusiness.Id = associationLineDTO.ByLineBusinessSubLineBusiness.LineBusiness.Id;

                    byLineBusinessSubLineBusinessDTO.SubLineBusiness = new List<SubLineBusinessDTO>();

                    foreach (SubLineBusinessDTO subLineBusinessDTO in associationLineDTO.ByLineBusinessSubLineBusiness.SubLineBusiness)
                    {
                        byLineBusinessSubLineBusinessDTO.SubLineBusiness.Add(subLineBusinessDTO);
                    }

                    lineAssociationDTO.AssociationType = byLineBusinessSubLineBusinessDTO;
                }
                // 3. POR RAMO / TIPO DE OPERACI�N
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_OPERATION_TYPE_PREFIX)))
                {
                    lineAssociationDTO.AssociationType = new ByOperationTypePrefixDTO();
                    BusinessTypeKeys businessType = BusinessTypeKeys.Accepted;
                    if (associationLineDTO.ByOperationTypePrefix.BusinessType == (int)BusinessTypeKeys.Accepted)
                    {
                        businessType = BusinessTypeKeys.Accepted;
                    }
                    else if (associationLineDTO.ByOperationTypePrefix.BusinessType == (int)BusinessTypeKeys.Assigned)
                    {
                        businessType = BusinessTypeKeys.Assigned;
                    }
                    else if (associationLineDTO.ByOperationTypePrefix.BusinessType == (int)BusinessTypeKeys.CompanyPercentage)
                    {
                        businessType = BusinessTypeKeys.CompanyPercentage;

                    }

                    ByOperationTypePrefixDTO byOperationTypePrefixDTO = new ByOperationTypePrefixDTO();
                    byOperationTypePrefixDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byOperationTypePrefixDTO.BusinessType = Convert.ToInt32(businessType);

                    byOperationTypePrefixDTO.Prefixes = new List<PrefixDTO>();

                    foreach (PrefixDTO prefixDTO in associationLineDTO.ByOperationTypePrefix.Prefixes)
                    {
                        byOperationTypePrefixDTO.Prefixes.Add(prefixDTO);
                    }

                    lineAssociationDTO.AssociationType = byOperationTypePrefixDTO;
                }
                // 4. POR ASEGURADO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_INSURED)))
                {
                    lineAssociationDTO.AssociationType = new ByInsuredDTO();

                    ByInsuredDTO byInsuredDTO = new ByInsuredDTO();
                    byInsuredDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    IndividualDTO individualDTO = new IndividualDTO() { IndividualId = associationLineDTO.ByInsured.Insured.IndividualId };

                    byInsuredDTO.Insured = individualDTO;
                    lineAssociationDTO.AssociationType = byInsuredDTO;
                }
                // 5 POR RAMO COMERCIAL 
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_PREFIX)))
                {
                    lineAssociationDTO.AssociationType = new ByPrefixDTO();
                    ByPrefixDTO byPrefixDTO = new ByPrefixDTO();
                    byPrefixDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;
                    byPrefixDTO.Prefix = new List<PrefixDTO>();

                    foreach (PrefixDTO prefixDTO in associationLineDTO.ByPrefix.Prefix)
                    {
                        byPrefixDTO.Prefix.Add(prefixDTO);
                    }

                    lineAssociationDTO.AssociationType = byPrefixDTO;
                }
                // 5. POR P�LIZA
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_POLICY)))
                {
                    lineAssociationDTO.AssociationType = new ByPolicyDTO();

                    ByPolicyDTO byPolicyDTO = new ByPolicyDTO();
                    byPolicyDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    PolicyDTO policyDTO = new PolicyDTO();
                    policyDTO.Id = associationLineDTO.ByPolicy.Policy.Id;

                    byPolicyDTO.Policy = policyDTO;
                    lineAssociationDTO.AssociationType = byPolicyDTO;
                }
                // 6.-POR FACULTATIVO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_FACULTATIVE_ISSUE)))
                {
                    lineAssociationDTO.AssociationType = new ByFacultativeIssueDTO();

                    ByFacultativeIssueDTO byFacultativeIssueDTO = new ByFacultativeIssueDTO();
                    byFacultativeIssueDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byFacultativeIssueDTO.Prefixes = new List<PrefixDTO>();

                    foreach (PrefixDTO prefixDTO in associationLineDTO.ByFacultativeIssue.Prefixes)
                    {
                        byFacultativeIssueDTO.Prefixes.Add(prefixDTO);
                    }

                    lineAssociationDTO.AssociationType = byFacultativeIssueDTO;
                }
                // 7. POR ASEGURADO / RAMO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_INSURED_PREFIX)))
                {
                    lineAssociationDTO.AssociationType = new ByInsuredPrefixDTO();

                    ByInsuredPrefixDTO byInsuredPrefixDTO = new ByInsuredPrefixDTO();
                    byInsuredPrefixDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byInsuredPrefixDTO.Insured = new IndividualDTO() { IndividualId = associationLineDTO.ByInsuredPrefix.Insured.IndividualId };

                    byInsuredPrefixDTO.Prefixes = new List<PrefixDTO>();

                    foreach (PrefixDTO prefixDTO in associationLineDTO.ByInsuredPrefix.Prefixes)
                    {
                        byInsuredPrefixDTO.Prefixes.Add(prefixDTO);
                    }

                    lineAssociationDTO.AssociationType = byInsuredPrefixDTO;
                }
                // 8. POR RAMO T�CNICO / SUBRAMO T�CNICO / RIESGO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESS_RISK)))
                {
                    someExecute = true;
                    lineAssociationDTO.AssociationType = new ByLineBusinessSubLineBusinessInsuredObjectDTO();
                    ByLineBusinessSubLineBusinessInsuredObjectDTO byLineBusinessSubLineBusinessInsuredObjectDTO = new ByLineBusinessSubLineBusinessInsuredObjectDTO();
                    byLineBusinessSubLineBusinessInsuredObjectDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byLineBusinessSubLineBusinessInsuredObjectDTO.LineBusiness = new LineBusinessDTO() { Id = associationLineDTO.ByLineBusinessSubLineBusinessRisk.LineBusinessId };
                    byLineBusinessSubLineBusinessInsuredObjectDTO.SubLineBusiness = new SubLineBusinessDTO() { Id = associationLineDTO.ByLineBusinessSubLineBusinessRisk.SubLineBusinessId };
                    byLineBusinessSubLineBusinessInsuredObjectDTO.InsuredObject = new List<InsuredObjectDTO>();


                    if (associationLineDTO.AssociationLineId > 0)
                    {
                        lineAssociationDTO.LineAssociationId = associationLineDTO.AssociationLineId;
                        // DELETE
                        DeleteAssociationColumnValueByAssociationLineId(lineAssociationDTO.LineAssociationId);
                        // EDICI�N
                        foreach (InsuredObjectDTO insuredObjectDTO in associationLineDTO.ByLineBusinessSubLineBusinessRisk.InsuredObject)
                        {
                            byLineBusinessSubLineBusinessInsuredObjectDTO.InsuredObject.Add(insuredObjectDTO);
                            lineAssociationDTO.AssociationType = byLineBusinessSubLineBusinessInsuredObjectDTO;
                            newLineAssociationId = UpdateLineAssociation(lineAssociationDTO).LineAssociationId;
                            byLineBusinessSubLineBusinessInsuredObjectDTO.InsuredObject.Clear();
                        }
                    }
                    else
                    {
                        foreach (InsuredObjectDTO insuredObjectDTO in associationLineDTO.ByLineBusinessSubLineBusinessRisk.InsuredObject)
                        {
                            byLineBusinessSubLineBusinessInsuredObjectDTO.InsuredObject.Add(insuredObjectDTO);
                            lineAssociationDTO.AssociationType = byLineBusinessSubLineBusinessInsuredObjectDTO;
                            newLineAssociationId = SaveLineAssociation(lineAssociationDTO).LineAssociationId;
                            lineAssociationDTO.LineAssociationId = newLineAssociationId;
                            byLineBusinessSubLineBusinessInsuredObjectDTO.InsuredObject.Clear();
                        }
                    }

                }
                // 9. POR RAMO / RIESGO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_PREFIX_RISK)))
                {
                    someExecute = true;
                    lineAssociationDTO.AssociationType = new ByLineBusinessInsuredObjectDTO();

                    ByLineBusinessInsuredObjectDTO byLineBusinessInsuredObjectDTO = new ByLineBusinessInsuredObjectDTO();
                    byLineBusinessInsuredObjectDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byLineBusinessInsuredObjectDTO.LineBusiness = new LineBusinessDTO();
                    byLineBusinessInsuredObjectDTO.LineBusiness.Id = associationLineDTO.ByPrefixRisk.PrefixId;
                    byLineBusinessInsuredObjectDTO.InsuredObject = new List<InsuredObjectDTO>();

                    if (associationLineDTO.AssociationLineId > 0)
                    {
                        lineAssociationDTO.LineAssociationId = associationLineDTO.AssociationLineId;
                        // DELETE
                        DeleteAssociationColumnValueByAssociationLineId(lineAssociationDTO.LineAssociationId);
                        // EDICI�N
                        foreach (InsuredObjectDTO insuredObjectDTO in associationLineDTO.ByPrefixRisk.InsuredObject)
                        {
                            byLineBusinessInsuredObjectDTO.InsuredObject.Add(insuredObjectDTO);
                            lineAssociationDTO.AssociationType = byLineBusinessInsuredObjectDTO;
                            newLineAssociationId = UpdateLineAssociation(lineAssociationDTO).LineAssociationId;
                            byLineBusinessInsuredObjectDTO.InsuredObject.Clear();
                        }
                    }
                    else
                    {
                        foreach (InsuredObjectDTO insuredObjectDTO in associationLineDTO.ByPrefixRisk.InsuredObject)
                        {
                            byLineBusinessInsuredObjectDTO.InsuredObject.Add(insuredObjectDTO);
                            lineAssociationDTO.AssociationType = byLineBusinessInsuredObjectDTO;
                            newLineAssociationId = SaveLineAssociation(lineAssociationDTO).LineAssociationId;
                            lineAssociationDTO.LineAssociationId = newLineAssociationId;
                            byLineBusinessInsuredObjectDTO.InsuredObject.Clear();
                        }
                    }
                }
                // 10. POR POLIZA /RAMO T�CNICO / SUBRAMO T�CNICO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_POLICY_LINE_BUSINESS_SUB_LINE_BUSINESS)))
                {
                    lineAssociationDTO.AssociationType = new ByPolicyLineBusinessSubLineBusinessDTO();

                    ByPolicyLineBusinessSubLineBusinessDTO byPolicyLineBusinessSubLineBusinessDTO = new ByPolicyLineBusinessSubLineBusinessDTO();
                    byPolicyLineBusinessSubLineBusinessDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;

                    byPolicyLineBusinessSubLineBusinessDTO.LineBusiness = new LineBusinessDTO() { Id = associationLineDTO.ByPolicyLineBusinessSubLineBusiness.LineBusiness.Id };

                    byPolicyLineBusinessSubLineBusinessDTO.SubLineBusiness = new SubLineBusinessDTO() { Id = associationLineDTO.ByPolicyLineBusinessSubLineBusiness.SubLineBusiness.Id };

                    byPolicyLineBusinessSubLineBusinessDTO.Policy = new PolicyDTO() { Id = associationLineDTO.ByPolicyLineBusinessSubLineBusiness.Policy.Id };

                    lineAssociationDTO.AssociationType = byPolicyLineBusinessSubLineBusinessDTO;
                }
                // 11. POR RAMO T�CNICO / SUBRAMO T�CNICO / COBERTURA
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_LINE_BUSINESS_SUB_LINE_BUSINESSCOVERAGE)))
                {
                    someExecute = true;
                    lineAssociationDTO.AssociationType = new ByLineBusinessSubLineBusinessCoverageDTO();
                    ByLineBusinessSubLineBusinessCoverageDTO byLineBusinessSubLineBusinessCoverageDTO = new ByLineBusinessSubLineBusinessCoverageDTO();
                    byLineBusinessSubLineBusinessCoverageDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;
                    byLineBusinessSubLineBusinessCoverageDTO.LineBusiness = new LineBusinessDTO() { Id = associationLineDTO.ByLineBusinessSubLineBusinessCoverage.LineBusiness.Id };
                    byLineBusinessSubLineBusinessCoverageDTO.SubLineBusiness = new SubLineBusinessDTO() { Id = associationLineDTO.ByLineBusinessSubLineBusinessCoverage.SubLineBusiness.Id };
                    byLineBusinessSubLineBusinessCoverageDTO.Coverage = new List<CoverageDTO>();

                    if (associationLineDTO.AssociationLineId > 0)
                    {
                        lineAssociationDTO.LineAssociationId = associationLineDTO.AssociationLineId;
                        // DELETE
                        DeleteAssociationColumnValueByAssociationLineId(lineAssociationDTO.LineAssociationId);
                        // EDICI�N
                        foreach (CoverageDTO coverageDTO in associationLineDTO.ByLineBusinessSubLineBusinessCoverage.Coverage)
                        {
                            byLineBusinessSubLineBusinessCoverageDTO.Coverage.Add(coverageDTO);
                            lineAssociationDTO.AssociationType = byLineBusinessSubLineBusinessCoverageDTO;
                            newLineAssociationId = UpdateLineAssociation(lineAssociationDTO).LineAssociationId;
                            byLineBusinessSubLineBusinessCoverageDTO.Coverage.Clear();
                        }
                    }
                    else
                    {
                        foreach (CoverageDTO coverageDTO in associationLineDTO.ByLineBusinessSubLineBusinessCoverage.Coverage)
                        {
                            byLineBusinessSubLineBusinessCoverageDTO.Coverage.Add(coverageDTO);
                            lineAssociationDTO.AssociationType = byLineBusinessSubLineBusinessCoverageDTO;
                            newLineAssociationId = SaveLineAssociation(lineAssociationDTO).LineAssociationId;
                            lineAssociationDTO.LineAssociationId = newLineAssociationId;
                            byLineBusinessSubLineBusinessCoverageDTO.Coverage.Clear();
                        }
                    }

                }
                // 12. POR RAMO COMERCIAL/PRODUCTO
                else if (associationLineDTO.AssociationTypeId == Convert.ToInt32(EnumHelper.GetEnumParameterValue<ReinsuranceAssociationTypes>(ReinsuranceAssociationTypes.REINS_BY_PREFIX_PRODUCT)))
                {
                    lineAssociationDTO.AssociationType = new ByPrefixProductDTO();

                    ByPrefixProductDTO byPrefixProductDTO = new ByPrefixProductDTO();
                    byPrefixProductDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;
                    byPrefixProductDTO.Prefix = new PrefixDTO();
                    byPrefixProductDTO.Prefix.Id = associationLineDTO.ByPrefixProduct.Prefix.Id;
                    byPrefixProductDTO.Products = new List<ProductDTO>();

                    foreach (ProductDTO productDTO in associationLineDTO.ByPrefixProduct.Products)
                    {
                        byPrefixProductDTO.Products.Add(productDTO);
                    }

                    lineAssociationDTO.AssociationType = byPrefixProductDTO;
                }


                if (!someExecute)
                {
                    if (associationLineDTO.AssociationLineId > 0)
                    {
                        lineAssociationDTO.LineAssociationId = associationLineDTO.AssociationLineId;
                        // EDICI�N
                        newLineAssociationId = UpdateLineAssociation(lineAssociationDTO).LineAssociationId;
                    }
                    else
                    {
                        // Devuelve el Id x si se lo requiere
                        newLineAssociationId = SaveLineAssociation(lineAssociationDTO).LineAssociationId;
                    }
                }

                return newLineAssociationId;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveLineAssociationByAssociationLine);
            }
        }

        public AssociationLineDTO AddAssociationLine(int lineAssociationTypeId, int associationLineId, int year)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                AssociationLineDTO associationLine = new AssociationLineDTO();
                List<AssociationLineDTO> associationLines = new List<AssociationLineDTO>();
                associationLines = GetAssociationLine(year, lineAssociationTypeId, associationLineId);

                if (associationLineId <= 0)
                {
                    associationLine.AssociationTypeId = lineAssociationTypeId;
                }
                else
                {
                    associationLine = associationLines.Where(x => x.AssociationLineId == associationLineId).FirstOrDefault();
                }

                return associationLine;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorAddAssociationLine);
            }
        }

        public List<LevelDTO> GetContractLevelByContractId(string contractId)
        {
            try
            {
                List<LevelDTO> levels = new List<LevelDTO>();
                levels = GetLevelsByContractId(Convert.ToInt32(contractId));
                return levels;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractLevelByContractId);
            }
        }

        public List<SelectDTO> GetContractYear()
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                return reinsuranceBusiness.GetContractYear();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractYear);
            }
        }

        public LevelDTO AddContractLevel(int contractId, int contractLevelId, int contractTypeId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                LevelDTO contractLevelDTO = new LevelDTO();
                ContractTypeDTO contractTypeDTO = new ContractTypeDTO();
                contractTypeDTO = GetContractTypeByContractTypeId(contractTypeId);

                if (contractLevelId.Equals(0))
                {
                    contractLevelDTO.Number = GetLevelNumberByContractId(contractId);
                    contractLevelDTO = reinsuranceBusiness.AddContractLevel(contractId, contractTypeId, contractLevelDTO.Number, contractTypeDTO);
                }
                else
                {
                    contractLevelDTO = GetLevelByLevelId(contractLevelId);
                    contractLevelDTO.Contract.ContractType = contractTypeDTO;
                }

                return contractLevelDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorAddContractLevel);
            }
        }

        public int SaveContractLevel(LevelDTO levelDTO)
        {
            try
            {
                if (levelDTO.ContractLevelId.Equals(0))
                {
                    SaveLevel(levelDTO);

                    // Contrato de Retenci�n
                    if (levelDTO.Contract.ContractType.ContractTypeId == 6)
                    {
                        // Guardar autom�ticamente LevelCompany
                        // ObtenerLevel id ingresado
                        List<LevelDTO> levelsResult = GetLevelsByContractId(levelDTO.Contract.ContractId);
                        int levelsFindId = levelsResult.Max(le => le.ContractLevelId);
                        if (levelsFindId > 0)
                        {
                            LevelCompanyDTO levelCompany = new LevelCompanyDTO
                            {
                                LevelCompanyId = 0,
                                ContractLevel = new LevelDTO { ContractLevelId = levelsFindId },
                                Agent = new AgentDTO { IndividualId = 0 },
                                Company = new CompanyDTO { IndividualId = 0 },
                                GivenPercentage = 100, // participation
                                ComissionPercentage = 0,
                                AdditionalCommissionPercentage = 0,
                                ReservePremiumPercentage = 0,
                                InterestReserveRelease = 0,
                                DragLossPercentage = 0,
                                ReinsuranceExpensePercentage = 0,
                                UtilitySharePercentage = 0, // profit
                                PresentationInformationType = 0,
                                IntermediaryCommission = false,
                                ClaimCommissionPercentage = 0,// LossCommissio,
                                DifferentialCommissionPercentage = 0,
                            };
                            SaveLevelCompany(levelCompany);
                        }
                    }
                }
                else
                {
                    UpdateLevel(levelDTO);
                }
                return levelDTO.Contract.ContractId;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveContractLevel);
            }
        }

        public List<int> ValidateBeforeDeleteContractLevel(int contractLevelId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<LevelCompanyDTO> levelCompaniesDTOs = new List<LevelCompanyDTO>();
                levelCompaniesDTOs = GetLevelCompaniesByLevelId(contractLevelId);
                return reinsuranceBusiness.ValidateBeforeDeleteContractLevel(contractLevelId, levelCompaniesDTOs);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateBeforeDeleteContractLevel);
            }
        }

        public bool DeleteContractLevel(int contractId, int contractLevelId, int level)
        {
            try
            {
                List<LevelDTO> levelDTOs = new List<LevelDTO>();
                ContractDTO contractDTO = new ContractDTO();
                levelDTOs = GetLevelsByContractId(Convert.ToInt32(contractId));
                contractDTO = GetContractById(contractId);

                int cont = 1;
                List<int> validResult = new List<int>();

                foreach (LevelDTO levelItem in levelDTOs)
                {
                    if (levelItem.Number == level)
                    {
                        if (cont == levelDTOs.Count)
                        {
                            validResult = ValidateBeforeDeleteContractLevel(contractLevelId);
                            // Retenci�n 
                            if (contractDTO.ContractType.ContractTypeId == 6)
                            {
                                // si recordNumber
                                if (validResult[0] == 1)
                                {
                                    //borrar level company
                                    DeleteLevelCompany(validResult[1]);
                                    validResult[0] = 0;
                                }
                            }

                            // si no existen comp�ias de Nivel/ borrar level 
                            if (validResult[0] == 0)
                            {
                                try
                                {
                                    DeleteLevel(contractLevelId);
                                    return true;
                                }
                                catch (BusinessException)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        cont++;
                    }
                }
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteContractLevel);
            }
        }

        public bool ResultSaveContract(ContractDTO contractDTO)
        {
            try
            {
                if (contractDTO.ContractId.Equals(0))
                {
                    SaveContract(contractDTO);
                }
                else
                {
                    UpdateContract(contractDTO);
                }
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorResultSaveContract);
            }
        }

        public List<LevelPaymentDTO> GetLevelPaymentsByLevelIdByLevelId(int levelId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<LevelPaymentDTO> levelPaymentDTOs = new List<LevelPaymentDTO>();
                levelPaymentDTOs = GetLevelPaymentsByLevelId(levelId);
                return reinsuranceBusiness.GetLevelPaymentsByLevelIdByLevelId(levelId, levelPaymentDTOs).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLevelPaymentsByLevelIdByLevelId);
            }
        }

        public List<CumulusTypeDTO> GetCumulusTypesOrderByDesc()
        {
            try
            {
                List<CumulusTypeDTO> cumulusTypeDTOs = new List<CumulusTypeDTO>();
                cumulusTypeDTOs = GetCumulusTypes();
                return cumulusTypeDTOs.OrderBy(x => x.Description).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusTypesOrderByDesc);
            }
        }

        public List<LineCumulusTypeDTO> GetLineCumulusTypeOrderByLineId()
        {
            try
            {
                List<LineCumulusTypeDTO> lineCumulusTypeDTOs = new List<LineCumulusTypeDTO>();
                lineCumulusTypeDTOs = GetLineCumulusType();
                return lineCumulusTypeDTOs.OrderByDescending(c => c.LineId).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLineCumulusTypeOrderByLineId);
            }
        }

        public List<ContractLineDTO> GetContractLineByLine(int lineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                LineDTO lineDTO = new LineDTO();
                lineDTO = GetContractLineByLineId(lineId);
                return reinsuranceBusiness.GetContractLineByLine(lineDTO).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetContractLineByLine);
            }
        }

        public List<AssociationLineDTO> GetAssociationLineByTypeLineYear(int year, int associationTypeId, int associationLineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<AssociationLineDTO> associationLineDTOs = new List<AssociationLineDTO>();
                associationLineDTOs = GetAssociationLine(year, associationTypeId, associationLineId);
                return reinsuranceBusiness.GetAssociationLineByTypeLineYear(year, associationTypeId, associationLineId, associationLineDTOs).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAssociationLineByTypeLineYear);
            }
        }

        public bool IsReinsurerActive(int individualId)
        {
            try
            {
                if (DelegateService.uniquePersonIntegrationService.GetReinsurerByIndividualId(individualId).DeclinedDate != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Masivos

        public void ReinsuranceMassive(int processId, int moduleId)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                switch (moduleId)
                {
                    case (int)Modules.Issuance:
                        reinsuranceDAO.IssueMasiveProcess(processId);
                        break;

                    case (int)Modules.Claim:
                        reinsuranceDAO.ClaimMasiveProcess(processId);
                        break;

                    case (int)Modules.Payment:
                        reinsuranceDAO.PaymentMasiveProcess(processId);
                        break;
                }
                CreateOperatingQuotaEventsByProcessId(processId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsuranceMasive);
            }
        }

        public void ReinsuranceMassiveByProccesIdModuleId(int processId, int moduleId)
        {
            try
            {
                WorkerFactory.Instance.CreateWorker(processId, moduleId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsuranceMasive);
            }
        }

        public void UpdateTempReinsuranceProcess(int tempReinsuranceProcessId, int? recordsProcessed, int? recordsFailed, DateTime endDate, int status)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                reinsuranceDAO.UpdateTempReinsuranceProcess(tempReinsuranceProcessId, recordsProcessed, recordsFailed, endDate, status);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateTempReinsuranceProcess);
            }
        }

        public List<TempReinsuranceProcessDTO> GetTempReinsuranceProcess(int? tempReinsuranceProcessId, int moduleId)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.GetTempReinsuranceProcess(tempReinsuranceProcessId, moduleId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempReinsuranceProcess);
            }
        }

        public List<TempReinsuranceProcessDTO> GetTempReinsuranceProcessDetails(int tempReinsuranceProcessId)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.GetTempReinsuranceProcessDetails(tempReinsuranceProcessId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempReinsuranceProcessDetails);
            }
        }

        #endregion

        #region Process Controller
        public List<ReinsuranceAllocationDTO> GetTempAllocationByLayerLineId(int tempLayerLineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ReinsuranceLineDTO reinsuranceLine = GetTempAllocation(tempLayerLineId);
                return reinsuranceBusiness.GetTempAllocationByLayerLineId(tempLayerLineId, reinsuranceLine.ToModel()).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempAllocationByLayerLineId);
            }
        }

        public List<ReinsuranceAllocationDTO> GetTotSumPrimeAllocation(int tempLayerLineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ReinsuranceLineDTO reinsuranceLine = new ReinsuranceLineDTO();
                reinsuranceLine = GetTempAllocation(tempLayerLineId);
                return reinsuranceBusiness.GetTotSumPrimeAllocation(tempLayerLineId, reinsuranceLine.ToModel()).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTotSumPrimeAllocation);
            }
        }

        public ReinsuranceAllocationDTO ModificationReinsuranceContractDialog(int tempIssueAllocationId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ReinsuranceAllocationDTO reinsuranceAllocation = new ReinsuranceAllocationDTO();
                reinsuranceAllocation = GetTempAllocationById(tempIssueAllocationId);
                return reinsuranceBusiness.ModificationReinsuranceContractDialog(tempIssueAllocationId, reinsuranceAllocation.ToModel()).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorModificationReinsuranceContractDialog);
            }
        }

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

        public List<TempReinsuranceProcessDTO> GetTempReinsuranceProcessByProcessId(int tempReinsuranceProcessId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                TempReinsuranceProcessDTO tempReinsuranceProcessDTO = GetTempReinsuranceProcess(tempReinsuranceProcessId, 0)[0];
                return reinsuranceBusiness.GetTempReinsuranceProcessByProcessId(tempReinsuranceProcessId, tempReinsuranceProcessDTO).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempReinsuranceProcessByProcessId);
            }
        }

        public ReinsuranceFacultativeDTO GetReinsuranceFacultative(int? tempFacultativeCompanyId, int endorsementId, int layerNumber, int lineId, string cumulusKey)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                reinsuranceLayerIssuanceDTOs = GetTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);
                ReinsuranceFacultativeDTO reinsuranceFacultativeDTO = new ReinsuranceFacultativeDTO();
                if (reinsuranceLayerIssuanceDTOs.Count > 0)
                {
                    if (tempFacultativeCompanyId > 0)
                    {
                        ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO = reinsuranceLayerIssuanceDTOs.Find(x => x.ReinsuranceLayerId == tempFacultativeCompanyId);

                        reinsuranceFacultativeDTO.TempFacultativeCompanyId = reinsuranceLayerIssuanceDTO.ReinsuranceLayerId;
                        reinsuranceFacultativeDTO.TempFacultativeId = reinsuranceLayerIssuanceDTO.TemporaryIssueId;
                        reinsuranceFacultativeDTO.ParticipationPercentage = reinsuranceLayerIssuanceDTO.LayerPercentage == 0 ? "" : reinsuranceLayerIssuanceDTO.LayerPercentage.ToString().Replace(",", ".");
                        reinsuranceFacultativeDTO.PremiumPercentage = reinsuranceLayerIssuanceDTO.PremiumPercentage == 0 ? "" : reinsuranceLayerIssuanceDTO.PremiumPercentage.ToString().Replace(",", ".");
                        reinsuranceFacultativeDTO.CommissionPercentage = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage == 0 ? "" :
                                                                               reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage.ToString().Replace(",", ".");
                        reinsuranceFacultativeDTO.BrokerReinsuranceId = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.IndividualId;
                        reinsuranceFacultativeDTO.ReinsuranceCompanyId = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.IndividualId;
                        reinsuranceFacultativeDTO.BrokerDescription = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.FullName;
                        reinsuranceFacultativeDTO.DescriptionCompany = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.FullName;
                        reinsuranceFacultativeDTO.DepositPercentage = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositPercentage;
                        reinsuranceFacultativeDTO.InterestOnReserve = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].InterestOnReserve;
                        reinsuranceFacultativeDTO.DepositReleaseDate = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositReleaseDate;

                    }
                    else if (tempFacultativeCompanyId == null)
                    {
                        reinsuranceFacultativeDTO.TempFacultativeCompanyId = reinsuranceLayerIssuanceDTOs[0].ReinsuranceLayerId;
                        reinsuranceFacultativeDTO.TempFacultativeId = reinsuranceLayerIssuanceDTOs[0].TemporaryIssueId;
                        reinsuranceFacultativeDTO.ParticipationPercentage = reinsuranceLayerIssuanceDTOs[0].LayerPercentage == 0 ? "" : reinsuranceLayerIssuanceDTOs[0].LayerPercentage.ToString().Replace(",", ".");
                        reinsuranceFacultativeDTO.PremiumPercentage = reinsuranceLayerIssuanceDTOs[0].PremiumPercentage == 0 ? "" : reinsuranceLayerIssuanceDTOs[0].PremiumPercentage.ToString().Replace(",", ".");
                        reinsuranceFacultativeDTO.CommissionPercentage = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage == 0 ? "" :
                                                                               reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage.ToString().Replace(",", ".");
                        reinsuranceFacultativeDTO.BrokerReinsuranceId = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.IndividualId;
                        reinsuranceFacultativeDTO.ReinsuranceCompanyId = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.IndividualId;
                        reinsuranceFacultativeDTO.BrokerDescription = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.FullName;
                        reinsuranceFacultativeDTO.DescriptionCompany = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.Name;
                        reinsuranceFacultativeDTO.DepositPercentage = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositPercentage;
                        reinsuranceFacultativeDTO.InterestOnReserve = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].InterestOnReserve;
                        reinsuranceFacultativeDTO.DepositReleaseDate = reinsuranceLayerIssuanceDTOs[0].Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositReleaseDate;

                    }
                    else
                    {
                        reinsuranceFacultativeDTO.TempFacultativeCompanyId = 0;

                    }
                }

                return reinsuranceFacultativeDTO;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorModificationReinsuranceFacultativeDialog);
            }
        }

        public List<TempFacultativeCompaniesDTO> GetTempFacultativeCompaniesByEndorsementId(int endorsementId, int layerNumber, int lineId, string cumulusKey)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                List<TempFacultativeCompaniesDTO> tempFacultativeCompaniesDTOs = new List<TempFacultativeCompaniesDTO>();
                reinsuranceLayerIssuanceDTOs = GetTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);
                if (reinsuranceLayerIssuanceDTOs.Count > 0)
                {
                    foreach (ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO in reinsuranceLayerIssuanceDTOs)
                    {
                        tempFacultativeCompaniesDTOs.Add(DTOAssembler.CreateTempFacultativeCompaniesByLayerIssuance(reinsuranceLayerIssuanceDTO));
                    }
                }

                return tempFacultativeCompaniesDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempFacultativeCompaniesByEndorsementId);
            }
        }

        public List<PlanFacultativeDTO> GetTempFacultativePayment(int levelCompanyId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<InstallmentDTO> installmentDTOs = new List<InstallmentDTO>();
                installmentDTOs = GetInstallmentsByLevelCompanyId(levelCompanyId);
                return reinsuranceBusiness.GetTempFacultativePayment(levelCompanyId, installmentDTOs);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempFacultativePayment);
            }
        }

        public bool SavePaymentPlanFacultative(int tmpFacultativeCompanyCode, int feeNumber, string paymentDate, decimal paymentAmount)
        {
            try
            {
                LevelCompanyDTO levelCompanyDTO = new LevelCompanyDTO()
                {
                    LevelCompanyId = tmpFacultativeCompanyCode
                };

                InstallmentDTO installmentDTO = new InstallmentDTO()
                {
                    LevelCompany = levelCompanyDTO,
                    InstallmentNumber = feeNumber,
                    PaidDate = Convert.ToDateTime(paymentDate),
                    PaidAmount = new AmountDTO
                    {
                        Value = paymentAmount,
                    }
                };
                SaveInstallment(installmentDTO);
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSavePaymentPlanFacultative);
            }
        }

        public bool DeletePlanFacultative(int facultativePaymentsId, int facultativeCompanyId)
        {
            try
            {
                LevelCompanyDTO levelCompanyDTO = new LevelCompanyDTO()
                {
                    LevelCompanyId = facultativeCompanyId
                };

                InstallmentDTO installmentDTO = new InstallmentDTO()
                {
                    Id = facultativePaymentsId,
                    LevelCompany = levelCompanyDTO
                };
                DeleteInstallment(installmentDTO);
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeletePlanFacultative);
            }
        }

        public List<FacultativeCompaniesDTO> SaveTempFacultativeCompanyByLineId(ReinsuranceFacultativeDTO reinsuranceFacultative,
                                int endorsementId, int layerNumber, int lineId, string cumulusKey)
        {
            try
            {
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                List<FacultativeCompaniesDTO> facultativeCompaniesDTOs = new List<FacultativeCompaniesDTO>();
                decimal facultativePercentageTop = 0;
                decimal facultativePremiumPercentageTop = 0;

                decimal commissionPercentage = Convert.ToDecimal(reinsuranceFacultative.CommissionPercentage);
                decimal depositPercentage = Convert.ToDecimal(reinsuranceFacultative.DepositPercentage);
                decimal interestOnReserve = Convert.ToDecimal(reinsuranceFacultative.InterestOnReserve);
                bool isSaveInstallment = false;

                reinsuranceLayerIssuanceDTOs = GetTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey);


                foreach (ReinsuranceLayerIssuanceDTO reinsuranceLayerDto in reinsuranceLayerIssuanceDTOs)
                {
                    if (reinsuranceFacultative.TempFacultativeCompanyId != 0)
                    {
                        if (reinsuranceFacultative.TempFacultativeCompanyId != reinsuranceLayerDto.ReinsuranceLayerId)
                        {
                            facultativePercentageTop = facultativePercentageTop +
                            Convert.ToDecimal(reinsuranceLayerDto.LayerPercentage);

                            facultativePremiumPercentageTop = facultativePremiumPercentageTop +
                            Convert.ToDecimal(reinsuranceLayerDto.PremiumPercentage);
                        }
                        else
                        {
                            facultativePercentageTop = facultativePercentageTop +
                            Convert.ToDecimal(reinsuranceFacultative.ParticipationPercentage.Replace(".", ","));

                            facultativePremiumPercentageTop = facultativePremiumPercentageTop +
                            Convert.ToDecimal(reinsuranceFacultative.PremiumPercentage.Replace(".", ","));
                        }
                    }
                    else
                    {
                        facultativePercentageTop = facultativePercentageTop + Convert.ToDecimal(reinsuranceLayerDto.LayerPercentage);
                        facultativePremiumPercentageTop = facultativePremiumPercentageTop + Convert.ToDecimal(reinsuranceLayerDto.PremiumPercentage);
                    }

                    if (reinsuranceFacultative.TempFacultativeCompanyId != reinsuranceLayerDto.ReinsuranceLayerId)
                    {
                        commissionPercentage = commissionPercentage +
                                Convert.ToDecimal(reinsuranceLayerDto.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage);
                        depositPercentage = depositPercentage + reinsuranceLayerDto.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositPercentage;
                        interestOnReserve = interestOnReserve +
                            Convert.ToDecimal(reinsuranceLayerDto.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].InterestOnReserve);
                    }
                }

                if (reinsuranceFacultative.TempFacultativeCompanyId == 0)
                {
                    facultativePercentageTop = facultativePercentageTop + Convert.ToDecimal(reinsuranceFacultative.ParticipationPercentage.Replace(".", ","));
                    facultativePremiumPercentageTop = facultativePremiumPercentageTop + Convert.ToDecimal(reinsuranceFacultative.PremiumPercentage.Replace(".", ","));
                }

                if (facultativePercentageTop <= 100 && facultativePremiumPercentageTop <= 100)
                {
                    isSaveInstallment = true;
                }
                else
                {
                    isSaveInstallment = false;
                    facultativeCompaniesDTOs.Add(new FacultativeCompaniesDTO { Status = 0 });
                    return facultativeCompaniesDTOs;
                }


                if (isSaveInstallment)
                {
                    InstallmentDTO installment = new InstallmentDTO();
                    LevelDTO level = new LevelDTO();
                    level.AssignmentPercentage = Convert.ToDecimal(reinsuranceFacultative.ParticipationPercentage.Replace(".", ","));

                    LevelCompanyDTO levelCompany = new LevelCompanyDTO();
                    levelCompany.Agent = new AgentDTO()
                    {
                        IndividualId = reinsuranceFacultative.BrokerReinsuranceId
                    };
                    levelCompany.Company = new CompanyDTO()
                    {
                        IndividualId = reinsuranceFacultative.ReinsuranceCompanyId
                    };
                    levelCompany.ComissionPercentage = Convert.ToDecimal(reinsuranceFacultative.CommissionPercentage.Replace(".", ","));
                    levelCompany.GivenPercentage = Convert.ToDecimal(reinsuranceFacultative.PremiumPercentage.Replace(".", ","));

                    levelCompany.DepositPercentage = reinsuranceFacultative.DepositPercentage;
                    levelCompany.InterestOnReserve = reinsuranceFacultative.InterestOnReserve;
                    levelCompany.DepositReleaseDate = reinsuranceFacultative.DepositReleaseDate;
                    levelCompany.LevelCompanyId = 0;

                    levelCompany.ContractLevel = level;
                    installment.LevelCompany = levelCompany;
                    installment.InstallmentNumber = reinsuranceFacultative.TempFacultativeId;
                    if (reinsuranceFacultative.TempFacultativeCompanyId == 0)
                    {
                        SaveInstallment(installment);
                    }
                    else
                    {
                        installment.Id = reinsuranceFacultative.TempFacultativeCompanyId;
                        UpdateInstallment(installment);
                    }
                }

                facultativeCompaniesDTOs.Add(new FacultativeCompaniesDTO
                {
                    Participation = facultativePercentageTop,
                    Premium = facultativePremiumPercentageTop
                });
                return facultativeCompaniesDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveTempFacultativeCompanyByLineId);
            }
        }

        public List<ReinsuranceLayerIssuanceDTO> SaveTempIssueLayerByEndorsementId(ReinsuranceLayerDTO reinsuranceLayerIssuanceDTO, int endorsementId)
        {
            try
            {
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuances = new List<ReinsuranceLayerIssuanceDTO>();
                ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuance = new ReinsuranceLayerIssuanceDTO();
                decimal layerPercentageTop = 0;
                decimal premiumPercentageTop = 0;
                int tempReinsuranceProcessId = 0;

                reinsuranceLayerIssuanceDTOs = GetTempLayerDistribution(endorsementId);
                reinsuranceLayerIssuanceDTO.TemporaryIssueId = reinsuranceLayerIssuanceDTO.ReinsSourceId;
                reinsuranceLayerIssuance.LayerPercentage = decimal.Parse(reinsuranceLayerIssuanceDTO.SumPercentage.ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                reinsuranceLayerIssuance.PremiumPercentage = decimal.Parse(reinsuranceLayerIssuanceDTO.PremiumPercentage.ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                reinsuranceLayerIssuance.ReinsuranceLayerId = reinsuranceLayerIssuanceDTO.ReinsuranceLayerId;
                reinsuranceLayerIssuance.TemporaryIssueId = reinsuranceLayerIssuanceDTO.TemporaryIssueId;
                reinsuranceLayerIssuance.LayerNumber = reinsuranceLayerIssuanceDTO.LayerNumber;
                tempReinsuranceProcessId = reinsuranceLayerIssuanceDTO.TempReinsuranceProcessId;

                // Suma la lista
                foreach (ReinsuranceLayerIssuanceDTO reinsuranceLayer in reinsuranceLayerIssuanceDTOs)
                {
                    if (reinsuranceLayerIssuance.ReinsuranceLayerId != 0)
                    {
                        if (reinsuranceLayerIssuance.ReinsuranceLayerId != reinsuranceLayer.ReinsuranceLayerId) // Edición
                        {
                            layerPercentageTop = layerPercentageTop + reinsuranceLayer.LayerPercentage;
                            premiumPercentageTop = premiumPercentageTop + reinsuranceLayer.PremiumPercentage;
                        }
                        else
                        {
                            layerPercentageTop = layerPercentageTop + Convert.ToDecimal(reinsuranceLayerIssuance.LayerPercentage);
                            premiumPercentageTop = premiumPercentageTop + Convert.ToDecimal(reinsuranceLayerIssuance.PremiumPercentage);
                        }
                    }
                    else
                    {
                        layerPercentageTop = layerPercentageTop + reinsuranceLayer.LayerPercentage;
                        premiumPercentageTop = premiumPercentageTop + reinsuranceLayer.PremiumPercentage;
                    }
                }

                if (reinsuranceLayerIssuance.ReinsuranceLayerId == 0)
                {
                    layerPercentageTop = layerPercentageTop + Convert.ToDecimal(reinsuranceLayerIssuance.LayerPercentage);
                    premiumPercentageTop = premiumPercentageTop + Convert.ToDecimal(reinsuranceLayerIssuance.PremiumPercentage);
                }

                if ((layerPercentageTop <= 100 && premiumPercentageTop <= 100)
                    && (layerPercentageTop > 0 && premiumPercentageTop > 0)
                    )
                {
                    // Graba o Edita
                    if (reinsuranceLayerIssuance.ReinsuranceLayerId == 0)
                    {
                        reinsuranceLayerIssuance.ReinsuranceLayerId = 0;
                        SaveTempIssueLayer(reinsuranceLayerIssuance);
                    }
                    else
                    {
                        // Validación
                        UpdateTempIssueLayer(reinsuranceLayerIssuance);
                    }

                    reinsuranceLayerIssuances.Add(new ReinsuranceLayerIssuanceDTO
                    {
                        LayerPercentage = layerPercentageTop,
                        PremiumPercentage = premiumPercentageTop
                    });
                }
                else
                {
                    return reinsuranceLayerIssuances;
                }

                return reinsuranceLayerIssuances;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveTempIssueLayerByEndorsementId);
            }
        }

        public bool UpdateTempAllocationByReinsuranceAllocation(ReinsuranceAllocationDTO reinsuranceAllocationDTO)
        {
            try
            {
                ReinsuranceAllocationDTO reinsuranceAllocation = new ReinsuranceAllocationDTO();
                reinsuranceAllocation.Amount = new AmountDTO();
                reinsuranceAllocation.Premium = new AmountDTO();
                reinsuranceAllocation.ReinsuranceAllocationId = reinsuranceAllocationDTO.ReinsuranceAllocationId;

                if (string.IsNullOrEmpty(reinsuranceAllocationDTO.Sum.Trim()))
                {
                    reinsuranceAllocation.Amount.Value = 0;
                }
                else
                {
                    reinsuranceAllocation.Amount.Value = decimal.Parse(reinsuranceAllocationDTO.Sum.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                }

                if (string.IsNullOrEmpty(reinsuranceAllocationDTO.PremiumAllocation.Trim()))
                {
                    reinsuranceAllocation.Premium.Value = 0;
                }
                else
                {
                    reinsuranceAllocation.Premium.Value = decimal.Parse(reinsuranceAllocationDTO.PremiumAllocation.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                }

                UpdateTempAllocation(reinsuranceAllocation);
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateTempAllocationByReinsuranceAllocation);
            }
        }

        public ReinsuranceDTO ReinsuranceClaim(int claimId, int claimModifyId, int userId)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                ReinsuranceDTO reinsuranceDTO = claimDAO.ReinsuranceClaim(claimId, claimModifyId, userId).ToDTO();

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

        public List<EndorsementReinsuranceDTO> GetReinsuranceByEndorsementId(int endorsementId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<EndorsementReinsuranceDTO> endorsementReinsuranceDTOs = new List<EndorsementReinsuranceDTO>();
                return endorsementReinsuranceDTOs = GetReinsuranceByEndorsement(endorsementId).OrderBy(x => x.LayerNumber).OrderBy(m => m.ReinsuranceNumber).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceByEndorsementId);
            }
        }

        public List<ReinsuranceDistributionDTO> GetDistributionByReinsuranceByLayerId(int layerId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ReinsuranceDistributionDTO> reinsuranceDistributionDTOs = new List<ReinsuranceDistributionDTO>();
                return reinsuranceDistributionDTOs = GetDistributionByReinsurance(layerId).OrderBy(x => x.CumulusKey).OrderBy(x => x.IsFacultative)
                                                .OrderBy(x => x.Contract).OrderBy(x => x.Reinsurer).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetDistributionByReinsuranceByLayerId);
            }
        }

        public ReinsuranceMassiveHeaderDTO SaveReinsuranceMassiveHeaderByTypeProcess(string dateFrom, string dateTo, int typeProcess, List<PrefixDTO> prefixDTOs, int userId)
        {
            try
            {
                ReinsuranceMassiveHeaderDTO reinsuranceMassiveHeaderDTO = new ReinsuranceMassiveHeaderDTO();
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                // VALIDACIÓN FECHA DE INICIO CONTRA LA FECHA DE CIERRE EN MODULE DATE
                int lastClosingYear = 0;
                int lastClosingMonth = 0;
                DateTime dateClose = DateTime.Now;

                List<ModuleDateDTO> moduleDates = GetModuleDates().Where(x => x.Description.Contains("REASEGURO")).ToList();

                if (moduleDates.Any()) // VALIDA QUE EXISTA LA PARAMETRIZACIÓN DE REASEGUROS EN MODULE DATE
                {
                    foreach (ModuleDateDTO module in moduleDates)
                    {
                        lastClosingYear = module.LastClosingYyyy;
                        lastClosingMonth = module.LastClosingMm;
                        int days = DateTime.DaysInMonth(lastClosingYear, lastClosingMonth);
                        dateClose = Convert.ToDateTime(days + "/" + lastClosingMonth + "/" + lastClosingYear);
                    }

                    if (Convert.ToDateTime(dateFrom) > dateClose)
                    {
                        int totalRecords = 0;
                        int processId = 0;

                        // Llamar al proceso que devuelve el Id de Proceso
                        switch (typeProcess)
                        {
                            case (int)Modules.Issuance: // Emisión
                                processId = LoadReinsuranceLayer(0, // EndorsementId 
                                    userId,
                                    (int)ProcessTypes.Massive, // Tipo proceso
                                    Convert.ToDateTime(dateFrom), // Fecha inicio
                                    Convert.ToDateTime(dateTo), // Fecha fin
                                    prefixDTOs); // Lista de Ramo
                                ValidatePriorityRetentionByProcessId(processId);
                                break;

                            case (int)Modules.Claim: //Siniestros null
                                processId = LoadReinsuranceClaim(0, userId,
                                             (int)ProcessTypes.Massive, // Tipo proceso masivo
                                             Convert.ToDateTime(dateFrom), // Fecha inicio
                                             Convert.ToDateTime(dateTo), prefixDTOs
                                             ).ReinsuranceId; // Fecha fin
                                break;
                            case (int)Modules.Payment:  // Pagos
                                processId = LoadReinsurancePayment(userId,
                                           Convert.ToDateTime(dateFrom),
                                           Convert.ToDateTime(dateTo), prefixDTOs).ReinsuranceId;
                                break;
                        }

                        if (processId > 0)
                        {
                            totalRecords = GetTempReinsuranceProcess(processId, 0).FirstOrDefault().RecordsNumber;
                        }

                        reinsuranceMassiveHeaderDTO.ProcessId = processId;
                        reinsuranceMassiveHeaderDTO.Records = totalRecords;
                        return reinsuranceMassiveHeaderDTO;
                    }
                    else
                    {
                        reinsuranceMassiveHeaderDTO.Option = "1";
                        reinsuranceMassiveHeaderDTO.DateClose = dateClose.ToShortDateString();
                        return reinsuranceMassiveHeaderDTO;
                    }
                }
                else
                {
                    reinsuranceMassiveHeaderDTO.Option = "2";
                    return reinsuranceMassiveHeaderDTO;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveReinsuranceMassiveHeaderByTypeProcess);
            }
        }

        public List<TempReinsuranceProcessDTO> LoadProcessMassiveDetailsReport(int tempReinsuranceProcessId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<TempReinsuranceProcessDTO> tempReinsuranceProcessDTOs = new List<TempReinsuranceProcessDTO>();
                tempReinsuranceProcessDTOs = GetTempReinsuranceProcessDetails(tempReinsuranceProcessId);
                return reinsuranceBusiness.LoadProcessMassiveDetailsReport(tempReinsuranceProcessId, tempReinsuranceProcessDTOs).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorLoadProcessMassiveDetailsReport);
            }
        }

        public List<ReinsuranceDTO> SaveReinsuranceByEndorsementId(int endorsementId, int userId, ReinsuranceDTO reinsuranceDTO)
        {
            try
            {
                List<ReinsuranceDTO> reinsurances = new List<ReinsuranceDTO>();
                PolicyDTO policy = new PolicyDTO();
                ReinsuranceLayerDTO resinsuranceLayerResult = new ReinsuranceLayerDTO();
                ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuance = new ReinsuranceLayerIssuanceDTO();
                List<ReinsuranceLayerIssuanceDTO> layers = new List<ReinsuranceLayerIssuanceDTO>();
                List<ReinsuranceLineDTO> reinsuranceLinesResult = new List<ReinsuranceLineDTO>();
                ReinsuranceLineDTO reinsuranceLineResult = new ReinsuranceLineDTO();
                List<ReinsuranceAllocationDTO> reinsuranceAllocationsResult = new List<ReinsuranceAllocationDTO>();
                ReinsuranceAllocationDTO reinsuranceAllocationResult = new ReinsuranceAllocationDTO();
                List<ReinsuranceLayerIssuanceDTO> facultativesResult = new List<ReinsuranceLayerIssuanceDTO>();

                DistributionDAO distributionDAO = new DistributionDAO();
                LineDAO lineDAO = new LineDAO();
                TempAllocationDAO tempAllocationDAO = new TempAllocationDAO();
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                InstallmentDAO installmentDAO = new InstallmentDAO();

                policy.EndorsmentId = endorsementId;
                reinsuranceDTO.ReinsuranceLayers = new List<ReinsuranceLayerDTO>();
                //Carga cabecera del Reaseguro
                reinsurances.Add(reinsuranceDTO);
                //Recupera capas
                layers = distributionDAO.GetTempLayerDistribution(endorsementId).ToDTOs().ToList();

                foreach (ReinsuranceLayerIssuanceDTO layer in layers)
                {
                    //Recupera líneas
                    reinsuranceLayerIssuance = lineDAO.GetTempLineCumulus(layer.ReinsuranceLayerId).ToDTO();

                    foreach (ReinsuranceLineDTO reinsuranceLine in reinsuranceLayerIssuance.Lines)
                    {
                        //Recupera Distribución
                        reinsuranceLineResult = tempAllocationDAO.GetTempAllocation(reinsuranceLine.ReinsuranceLineId).ToDTO();

                        foreach (ReinsuranceAllocationDTO reinsuranceAllocation in reinsuranceLineResult.ReinsuranceAllocations)
                        {
                            reinsuranceAllocationResult = new ReinsuranceAllocationDTO();
                            reinsuranceAllocationResult.Amount = reinsuranceAllocation.Amount;
                            reinsuranceAllocationResult.Premium = reinsuranceAllocation.Premium;
                            reinsuranceAllocationResult.Facultative = reinsuranceAllocation.Facultative;

                            reinsuranceAllocationResult.Contract = new ContractDTO();
                            reinsuranceAllocationResult.Contract.SmallDescription = reinsuranceAllocation.Contract.SmallDescription;
                            reinsuranceAllocationResult.Contract.FacultativePercentage = reinsuranceAllocation.Contract.FacultativePercentage;
                            reinsuranceAllocationResult.Contract.FacultativePremiumPercentage = reinsuranceAllocation.Contract.FacultativePremiumPercentage;

                            reinsuranceAllocationResult.Contract.ContractLevels = new List<LevelDTO>();

                            LevelDTO level = new LevelDTO();
                            level.Number = reinsuranceAllocation.Contract.ContractLevels[0].Number;

                            level.ContractLevelCompanies = new List<LevelCompanyDTO>();

                            if (reinsuranceAllocation.Facultative)
                            {
                                facultativesResult = tempFacultativeCompanyDAO.GetTempFacultativeCompanies(endorsementId, layer.LayerNumber, reinsuranceLine.Line.LineId,
                                                                                                     reinsuranceLine.CumulusKey).ToDTOs().ToList();
                                foreach (ReinsuranceLayerIssuanceDTO facultativeCompany in facultativesResult)
                                {
                                    LevelCompanyDTO levelCompany = new LevelCompanyDTO();

                                    levelCompany.LevelCompanyId = facultativeCompany.ReinsuranceLayerId;

                                    //COMPANIA REASEGURADORA
                                    levelCompany.Company = new CompanyDTO();
                                    levelCompany.Company.IndividualId = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.IndividualId;
                                    levelCompany.Company.Name = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.Name;
                                    levelCompany.ComissionPercentage = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage;

                                    //BROKER
                                    levelCompany.Agent = new AgentDTO();
                                    levelCompany.Agent.IndividualId = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.IndividualId;
                                    levelCompany.Agent.FullName = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.FullName;

                                    //FACULTATIVO
                                    levelCompany.DepositPercentage = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositPercentage;
                                    levelCompany.InterestOnReserve = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].InterestOnReserve;
                                    levelCompany.DepositReleaseDate = facultativeCompany.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].DepositReleaseDate;

                                    levelCompany.PaymentSchedule = new List<InstallmentDTO>();

                                    //CArga los planes de pago
                                    levelCompany.PaymentSchedule = installmentDAO.GetInstallmentsByLevelCompanyId(levelCompany.LevelCompanyId).ToDTOs().ToList();

                                    level.ContractLevelCompanies.Add(levelCompany);
                                }

                            }

                            reinsuranceAllocationResult.Contract.ContractLevels.Add(level);

                            reinsuranceAllocationsResult.Add(reinsuranceAllocationResult);
                        }

                        reinsuranceLineResult = new ReinsuranceLineDTO();
                        reinsuranceLineResult = reinsuranceLine;
                        reinsuranceLineResult.ReinsuranceAllocations = new List<ReinsuranceAllocationDTO>();
                        reinsuranceLineResult.ReinsuranceAllocations = reinsuranceAllocationsResult;
                        reinsuranceLinesResult.Add(reinsuranceLineResult);
                        reinsuranceAllocationsResult = new List<ReinsuranceAllocationDTO>();
                        //Carga del objeto Basado en el objeto LineCumulusKey (Línea,Claveúmulo,Riesgo,Cobertura) 
                    }

                    resinsuranceLayerResult = new ReinsuranceLayerDTO();
                    resinsuranceLayerResult = layer;
                    resinsuranceLayerResult.ReinsuranceLines = new List<ReinsuranceLineDTO>();
                    resinsuranceLayerResult.ReinsuranceLines = reinsuranceLinesResult;
                    reinsuranceDTO.ReinsuranceLayers.Add(resinsuranceLayerResult);
                    reinsuranceLinesResult = new List<ReinsuranceLineDTO>();
                }

                reinsurances = SaveIssueReinsurance(policy, reinsuranceDTO);

                if (reinsurances.Count > 0)
                {
                    if (reinsurances[0].ReinsuranceId == 0) //0 si el sp devuelve como no exitoso el proceso
                    {
                        List<IssGetDistributionErrorsDTO> distributionErrors = GetDistributionErrors(endorsementId);
                        if (distributionErrors.Count > 0)
                        {
                            reinsurances[0].ReinsuranceId = -1;
                            return reinsurances; // -1
                        }
                        return null;
                    }
                    else
                    {
                        List<int> issueReinsuranceIds = new List<int>();
                        foreach (ReinsuranceDTO reinsurance in reinsurances)
                        {
                            issueReinsuranceIds.Add(reinsurance.ReinsuranceId);
                        }
                        CreateOperatingQuotaEvents(endorsementId, issueReinsuranceIds);
                        return reinsurances;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveReinsuranceByEndorsementId);
            }
        }

        public ModuleDateDTO GetModuleDate(ModuleDateDTO moduleDateDTO)
        {
            try
            {
                ModuleDateDTO moduleDate = new ModuleDateDTO();
                moduleDate = DelegateService.tempCommonIntegrationService.GetModuleDate(moduleDateDTO.ToIntegrationDTO()).ToDTO();
                return moduleDate;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetModuleDate);
            }
        }

        public List<AgentDTO> GetAgentByName(string name)
        {
            try
            {
                List<AgentDTO> agentDTOs = new List<AgentDTO>();
                agentDTOs = DelegateService.tempCommonIntegrationService.GetAgentByName(name).ToDTOs().ToList();
                return agentDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAgentByName);
            }
        }

        public List<ProductDTO> GetProductsByPrefixId(int prefixId)
        {
            try
            {
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                productDTOs = DelegateService.tempCommonIntegrationService.GetProductsByPrefixId(prefixId).ToDTOs().ToList();
                return productDTOs;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetProductsByPrefixId);
            }
        }

        public List<BranchDTO> GetBranchesByUserId(int userId)
        {
            try
            {
                List<BranchDTO> branchDTOs = new List<BranchDTO>();
                branchDTOs = DelegateService.uniqueUserIntegrationService.GetBranchesByUserId(userId).ToDTOs().ToList();
                return branchDTOs;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetBranchesByUserId);
            }
        }

        public UserDTO GetUserByLogin(string login)
        {
            try
            {
                UserDTO userDTO = new UserDTO();
                userDTO = DelegateService.uniqueUserIntegrationService.GetUserByLogin(login).ToDTO();
                return userDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetUserByLogin);
            }
        }

        public PolicyDTO GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                PolicyDTO policyDTO = new PolicyDTO();
                policyDTO = DTOAssembler.CreatePolicyDTOByUNDDTOPolicyDTO(
                    DelegateService.underwritingIntegrationService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber)
                );

                return policyDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCurrentPolicyByPrefixIdBranchIdPolicyNumber);
            }
        }

        public List<SlipDTO> GetSlips(int processId, int endorsementId)
        {
            try
            {
                List<SlipDTO> slips = new List<SlipDTO>();
                TempFacultativeCompanyDAO facultativeCompanyDAO = new TempFacultativeCompanyDAO();
                slips = facultativeCompanyDAO.GetSlips(processId, endorsementId).ToDTOs().ToList();
                return slips;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCurrentPolicyByPrefixIdBranchIdPolicyNumber);
            }
        }
        public int ExpandFacultative(int processId, int endorsementId, int layerNumber, int facultativeId)
        {
            try
            {
                TempFacultativeCompanyDAO facultativeCompanyDAO = new TempFacultativeCompanyDAO();
                return facultativeCompanyDAO.ExpandFacultative(processId, endorsementId, layerNumber, facultativeId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCurrentPolicyByPrefixIdBranchIdPolicyNumber);
            }

        }

        #endregion

        #region Cúmulos

        public List<InsuredDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                string documentNumber;
                Int64 id = 0;
                Int64.TryParse(description, out id);
                List<InsuredDTO> insuredDTOs = new List<InsuredDTO>();
                List<EconomicGroupDTO> economicGroupDTOs = new List<EconomicGroupDTO>();
                insuredDTOs = DelegateService.underwritingIntegrationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType).ToDTOs().ToList();
                if (id > 0)
                {
                    description = null;
                    documentNumber = id.ToString();
                }
                else
                {
                    documentNumber = null;
                }
                economicGroupDTOs = DelegateService.uniquePersonIntegrationService.GetEconomicGroupByDocument(description, documentNumber).ToDTOs().ToList();
                insuredDTOs.AddRange(DTOAssembler.CreateInsuredsByEconomicGroups(economicGroupDTOs));
                return insuredDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        private List<ExchangeRateDTO> GetExchangeRates(DateTime? dateCumulus = null, int? CurrecyCode = null)
        {
            try
            {
                if (dateCumulus != null && CurrecyCode != null)
                {
                    return DelegateService.commonIntegrationService.GetExchangeRates(dateCumulus, CurrecyCode).ToDTOs();
                }
                return DelegateService.commonIntegrationService.GetExchangeRates().ToDTOs();
            }
            catch (Exception)
            {
                return new List<ExchangeRateDTO>();
            }
        }

        public bool CreateOperatingQuotaEvents(int endorsmentId, List<int> issueReinsuranceIds = null)
        {

            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                List<OperatingQuotaEventDTO> operatingQuotaEvents = new List<OperatingQuotaEventDTO>();
                List<IssueAllocationRiskCoverDTO> issueAllocationRiskCoverDTOs = new List<IssueAllocationRiskCoverDTO>();
                issueAllocationRiskCoverDTOs = reinsuranceDAO.GetIssueAllocationRiskCoveragesByEndorsementId(endorsmentId, issueReinsuranceIds).ToDTOs().ToList();
                operatingQuotaEvents = issueAllocationRiskCoverDTOs.Select(CreateOperatingQuotaEventByIssueAllocationRiskCoverDTO()).ToList();
                return DelegateService.reinsuranceOperatingQuotaIntegrationServices.CreateOperatingQuotaEvents(operatingQuotaEvents.CreateROQINTDTOOperatingQuotaByOperatingQuotaEventDTOs());

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
                operatingQuotaEventDTO = issueAllocationRiskCoverDTO.ToModel().CreateOperatingQuotaEventDTOByIssueAllocationRiskCoverDTO();
                operatingQuotaEventDTO.OperatingQuotaEventType = Convert.ToInt32(EventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
                operatingQuotaEventDTO.ApplyReinsurance = issueAllocationRiskCoverDTO.ToModel().CreateApplyReinsuranceDTOByIssueAllocationRiskCoverDTO();
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

                contractCoverageDTOs.Add(issueAllocationRiskCoverDTO.ToModel().CreateContractCoverageDTOByIssueAllocationRiskCover());
                operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage = contractCoverageDTOs;

                return operatingQuotaEventDTO;
            };
        }

        public bool CreateOperatingQuotaEventsByProcessId(int processId)
        {
            try
            {

                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                List<PolicyDTO> policyDTOs = new List<PolicyDTO>();

                policyDTOs = DTOAssembler.ToDTOs(reinsuranceDAO.GetIssueReinsuranceByProcessId(processId)).ToList();

                if (policyDTOs.Count > 0)
                {
                    foreach (PolicyDTO policyDTO in policyDTOs)
                    {
                        CreateOperatingQuotaEvents(policyDTO.Endorsement.Id);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateOperatingQuotaEvents);
            }

        }

        public ReinsuranceCumulusDTO GetCumulusByIndividual(int individualId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int subLineBusiness, int PrefixCd)
        {
            try
            {
                ReinsuranceCumulusDTO reinsuranceCumulusDTO = new ReinsuranceCumulusDTO();
                List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                List<ContractReinsuranceCumulusDTO> contractReinsuranceCumulusDTOs = new List<ContractReinsuranceCumulusDTO>();

                List<CurrencyDTO> currencyDTOs = DelegateService.commonIntegrationService.GetCurrencies().ToDTOs().ToList();

                DelegateService.reinsuranceOperatingQuotaIntegrationServices.GetCumulusCoveragesByIndividual(individualId, lineBusiness, dateCumulus, IsFuture, subLineBusiness, PrefixCd).Select(DTOAssembler.CreateOperatingQuotaEventDTO).Select(GetCumulusContractsByIndividual(currencyDTOs, dateCumulus)).ToList().ForEach(x =>
                {
                    contractReinsuranceCumulusDTOs.AddRange(x);
                });
                DateTime dateExchanceRate = contractReinsuranceCumulusDTOs.Where(x => x.Contract.EstimatedDate < dateCumulus.AddDays(1)).OrderByDescending(x => x.Contract.EstimatedDate).FirstOrDefault().Contract.EstimatedDate;
                reinsuranceCumulusDTO.ContractReinsuranceCumulusDTOs = contractReinsuranceCumulusDTOs.GroupBy(z => z.Contract.SmallDescription).Select(cr => new ContractReinsuranceCumulusDTO
                {
                    Contract = new ContractDTO
                    {
                        ContractId = cr.First().Contract.ContractId,
                        SmallDescription = cr.First().Contract.SmallDescription,
                        Currency = cr.First().Contract.Currency,
                        EstimatedDate = dateExchanceRate
                    },
                    LevelLimit = cr.First().LevelLimit,
                    AssignmentAmount = cr.Sum(x => x.AssignmentAmount),
                    AssignmentAmountLocalCurrency = cr.Sum(x => x.AssignmentAmountLocalCurrency),
                    AssignmentPremiumAmount = cr.Sum(x => x.AssignmentPremiumAmount),
                    AssignmentPremiumAmountLocalCurrency = cr.Sum(x => x.AssignmentPremiumAmountLocalCurrency),
                    RetentionAmount = cr.Sum(x => x.RetentionAmount),
                    RetentionPremiumAmount = cr.Sum(x => x.RetentionPremiumAmount),
                    RetentionAmountLocalCurrency = cr.Sum(x => x.RetentionAmountLocalCurrency),
                    RetentionPremiumAmountLocalCurrency = cr.Sum(x => x.RetentionPremiumAmountLocalCurrency),
                    AmountLocalCurrency = cr.Sum(x => x.AmountLocalCurrency)
                }).ToList();

                reinsuranceCumulusDTO.TotalCumulus = reinsuranceCumulusDTO.ContractReinsuranceCumulusDTOs.Sum(x => x.AssignmentAmountLocalCurrency + x.RetentionAmountLocalCurrency);
                reinsuranceCumulusDTO.TotalCumulus = reinsuranceCumulusDTO.TotalCumulus >= 0 ? reinsuranceCumulusDTO.TotalCumulus : 0;

                reinsuranceCumulusDTO.AssignmentTotalCumulus = reinsuranceCumulusDTO.ContractReinsuranceCumulusDTOs.Sum(x => x.AssignmentAmountLocalCurrency + x.RetentionAmountLocalCurrency);
                reinsuranceCumulusDTO.AssignmentTotalCumulus = reinsuranceCumulusDTO.AssignmentTotalCumulus >= 0 ? reinsuranceCumulusDTO.AssignmentTotalCumulus : 0;

                reinsuranceCumulusDTO.RetentionTotalCumulus = reinsuranceCumulusDTO.ContractReinsuranceCumulusDTOs.Sum(x => x.RetentionAmountLocalCurrency);
                reinsuranceCumulusDTO.RetentionTotalCumulus = reinsuranceCumulusDTO.RetentionTotalCumulus >= 0 ? reinsuranceCumulusDTO.RetentionTotalCumulus : 0;

                return reinsuranceCumulusDTO;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusByIndividual);
            }
        }

        private Func<OperatingQuotaEventDTO, IEnumerable<ContractReinsuranceCumulusDTO>> GetCumulusContractsByIndividual(List<CurrencyDTO> currencyDTOs, DateTime dateCumulus)
        {
            try
            {
                return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
                {
                    return operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage.Select(CreateContractReinsuranceCumulusDTOByoperatingQuotaEventDTO(operatingQuotaEventDTO, currencyDTOs, dateCumulus));
                };
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusByIndividual);
            }
        }

        private Func<ContractCoverageDTO, ContractReinsuranceCumulusDTO> CreateContractReinsuranceCumulusDTOByoperatingQuotaEventDTO(OperatingQuotaEventDTO operatingQuotaEventDTO, List<CurrencyDTO> currencies, DateTime dateCumulus)
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
                    List<ExchangeRateDTO> exchangeRates = new List<ExchangeRateDTO>();
                    exchangeRates = GetExchangeRates(dateCumulus, contractCoverageDTO.ContractCurrencyId);

                    exchangeRate = exchangeRates.FirstOrDefault().BuyAmount;
                    contractReinsuranceCumulusDTO.Contract.EstimatedDate = exchangeRates.FirstOrDefault().RateDate;
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

        public ReinsuranceCumulusDTO GetCumulusDetailByIndividual(int individualId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int subLineBusiness, int prefixCd)
        {
            ReinsuranceCumulusDTO reinsuranceCumulusDTO = new ReinsuranceCumulusDTO();
            List<CoverageReinsuranceCumulusDTO> coverageReinsuranceCumulusDTOs = new List<CoverageReinsuranceCumulusDTO>();
            List<ReinsuranceCumulusDetailDTO> reinsuranceCumulusDetailDTOs = new List<ReinsuranceCumulusDetailDTO>();

            //if (lineBusiness == (int)LineBusinessKeys.CUMPLIMIENTO || prefixCd == (int)PrefixTypes.CUMPLIMIENTO)
            //{
                List<CurrencyDTO> currencyDTOs = DelegateService.commonIntegrationService.GetCurrencies().ToDTOs().ToList();
                List<ExchangeRateDTO> exchangeRateDTOs = new List<ExchangeRateDTO>();
                exchangeRateDTOs = GetExchangeRates(dateCumulus);
                List<PrefixDTO> prefixDTOs = DelegateService.commonIntegrationService.GetPrefixes().ToDTOs().ToList();
                List<BranchDTO> branchDTOs = DelegateService.commonIntegrationService.GetBranches().ToDTOs().ToList();

                DelegateService.reinsuranceOperatingQuotaIntegrationServices.GetCumulusCoveragesByIndividual(individualId, lineBusiness, dateCumulus, IsFuture, subLineBusiness, prefixCd).Select(DTOAssembler.CreateOperatingQuotaEventDTO).Select(GetCumulusCoveragesByIndividual(currencyDTOs)).ToList().ForEach(x =>
                {
                    coverageReinsuranceCumulusDTOs.AddRange(x);
                });

                if (lineBusiness == 0)
                {
                    lineBusiness = DelegateService.commonIntegrationService.GetLinesBusinessByPrefixId(prefixCd).FirstOrDefault().Id;
                }

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual = coverageReinsuranceCumulusDTOs.FindAll(x => (x.Consortium.IndividualId == 0 && x.EconomicGroup.EconomicGroupId == 0) ||
                                                                                                                               (x.Consortium.IndividualId == individualId && x.EconomicGroup.EconomicGroupId == 0) ||
                                                                                                                               (x.EconomicGroup.EconomicGroupId == individualId && x.Consortium.IndividualId == 0));
                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual.GroupBy(c => new { c.CoverageId, c.DocumentNum })
                                                            .Select(crc => new CoverageReinsuranceCumulusDTO
                                                            {
                                                                CoverageId = crc.First().CoverageId,
                                                                DocumentNum = crc.First().DocumentNum,
                                                                CoverageCurrentFrom = crc.First().CoverageCurrentFrom,
                                                                CoverageCurrentTo = crc.First().CoverageCurrentTo,
                                                                Insured = new InsuredDTO
                                                                {
                                                                    IndividualId = crc.First().Insured.IndividualId
                                                                },
                                                                Consortium = new InsuredDTO
                                                                {
                                                                    IndividualId = crc.First().Insured.IndividualId
                                                                },
                                                                EconomicGroup = new EconomicGroupDTO
                                                                {
                                                                    EconomicGroupId = crc.First().EconomicGroup.EconomicGroupId
                                                                },
                                                                Currency = new CurrencyDTO
                                                                {
                                                                    Id = crc.First().Currency.Id
                                                                },
                                                                Branch = new BranchDTO
                                                                {
                                                                    Id = crc.First().Branch.Id
                                                                },
                                                                Prefix = new PrefixDTO
                                                                {
                                                                    Id = crc.First().Prefix.Id
                                                                },
                                                                Coverage = new CoverageDTO
                                                                {
                                                                    LimitAmount = new AmountDTO
                                                                    {
                                                                        Value = crc.Sum(x => x.Coverage.LimitAmount.Value),
                                                                    }
                                                                },
                                                                ContractReinsuranceCumulus = new ContractReinsuranceCumulusDTO
                                                                {
                                                                    AssignmentAmountLocalCurrency = crc.Sum(x => x.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency),
                                                                    RetentionAmountLocalCurrency = crc.Sum(x => x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency)
                                                                }
                                                            }).ToList();

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual.RemoveAll(x => x.Coverage.LimitAmount.Value == 0);

                if (reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual.Count > 0)
                {
                    reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual.Select(CreateCoverageReinsuranceCumulusDTO(currencyDTOs, prefixDTOs, branchDTOs, lineBusiness, prefixCd)).ToList();
                }

                ReinsuranceCumulusDetailDTO individualCumulusDetailDTO = new ReinsuranceCumulusDetailDTO
                {
                    Id = (int)CumulusDetailKeys.REINS_CUMULUS_DETAIL_INDIVIDUAL,
                    AssigmentAmount = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual.Sum(x => x.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency + x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency),
                    RetentionAmount = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual.Sum(x => x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency),
                    CumulusDetail = new CumulusDetailDTO
                    {
                        Description = EnumHelper.GetEnumParameterValue<CumulusDetailKeys>(CumulusDetailKeys.REINS_CUMULUS_DETAIL_INDIVIDUAL).ToString()
                    }
                };

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium = coverageReinsuranceCumulusDTOs.FindAll(x => (x.Consortium.IndividualId > 0) &&
                                                                                                                               (x.Consortium.IndividualId != individualId));

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium.GroupBy(c => new { c.CoverageId, c.DocumentNum })
                                                            .Select(crc => new CoverageReinsuranceCumulusDTO
                                                            {
                                                                CoverageId = crc.First().CoverageId,
                                                                DocumentNum = crc.First().DocumentNum,
                                                                CoverageCurrentFrom = crc.First().CoverageCurrentFrom,
                                                                CoverageCurrentTo = crc.First().CoverageCurrentTo,
                                                                Insured = new InsuredDTO
                                                                {
                                                                    IndividualId = crc.First().Insured.IndividualId
                                                                },
                                                                Consortium = new InsuredDTO
                                                                {
                                                                    IndividualId = crc.First().Consortium.IndividualId
                                                                },
                                                                EconomicGroup = new EconomicGroupDTO
                                                                {
                                                                    EconomicGroupId = crc.First().EconomicGroup.EconomicGroupId
                                                                },
                                                                Currency = new CurrencyDTO
                                                                {
                                                                    Id = crc.First().Currency.Id
                                                                },
                                                                Branch = new BranchDTO
                                                                {
                                                                    Id = crc.First().Branch.Id
                                                                },
                                                                Prefix = new PrefixDTO
                                                                {
                                                                    Id = crc.First().Prefix.Id
                                                                },
                                                                Coverage = new CoverageDTO
                                                                {
                                                                    LimitAmount = new AmountDTO
                                                                    {
                                                                        Value = crc.Sum(x => x.Coverage.LimitAmount.Value),
                                                                    }
                                                                },
                                                                ContractReinsuranceCumulus = new ContractReinsuranceCumulusDTO
                                                                {
                                                                    AssignmentAmountLocalCurrency = crc.Sum(x => x.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency),
                                                                    RetentionAmountLocalCurrency = crc.Sum(x => x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency)
                                                                }
                                                            }).ToList();

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium.RemoveAll(x => x.Coverage.LimitAmount.Value == 0);

                if (reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium.Count > 0)
                {
                    reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium.Select(CreateCoverageReinsuranceCumulusDTO(currencyDTOs, prefixDTOs, branchDTOs, lineBusiness, prefixCd)).ToList();
                }

                ReinsuranceCumulusDetailDTO consortiumCumulusDetailDTO = new ReinsuranceCumulusDetailDTO
                {
                    Id = (int)CumulusDetailKeys.REINS_CUMULUS_DETAIL_CONSORTIUM,
                    AssigmentAmount = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium.Sum(x => x.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency + x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency),
                    RetentionAmount = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium.Sum(x => x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency),
                    CumulusDetail = new CumulusDetailDTO
                    {
                        Description = EnumHelper.GetEnumParameterValue<CumulusDetailKeys>(CumulusDetailKeys.REINS_CUMULUS_DETAIL_CONSORTIUM).ToString()
                    }
                };

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup = coverageReinsuranceCumulusDTOs.FindAll(x => (x.EconomicGroup.EconomicGroupId > 0) &&
                                                                                                                                  (x.EconomicGroup.EconomicGroupId != individualId));
                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup.GroupBy(c => new { c.CoverageId, c.DocumentNum })
                                                            .Select(crc => new CoverageReinsuranceCumulusDTO
                                                            {
                                                                CoverageId = crc.First().CoverageId,
                                                                DocumentNum = crc.First().DocumentNum,
                                                                CoverageCurrentFrom = crc.First().CoverageCurrentFrom,
                                                                CoverageCurrentTo = crc.First().CoverageCurrentTo,
                                                                Insured = new InsuredDTO
                                                                {
                                                                    IndividualId = crc.First().Insured.IndividualId
                                                                },
                                                                Consortium = new InsuredDTO
                                                                {
                                                                    IndividualId = crc.First().Consortium.IndividualId
                                                                },
                                                                EconomicGroup = new EconomicGroupDTO
                                                                {
                                                                    EconomicGroupId = crc.First().EconomicGroup.EconomicGroupId,
                                                                },
                                                                Currency = new CurrencyDTO
                                                                {
                                                                    Id = crc.First().Currency.Id
                                                                },
                                                                Branch = new BranchDTO
                                                                {
                                                                    Id = crc.First().Branch.Id
                                                                },
                                                                Prefix = new PrefixDTO
                                                                {
                                                                    Id = crc.First().Prefix.Id
                                                                },
                                                                Coverage = new CoverageDTO
                                                                {
                                                                    LimitAmount = new AmountDTO
                                                                    {
                                                                        Value = crc.Sum(x => x.Coverage.LimitAmount.Value),
                                                                    }
                                                                },
                                                                ContractReinsuranceCumulus = new ContractReinsuranceCumulusDTO
                                                                {
                                                                    AssignmentAmountLocalCurrency = crc.Sum(x => x.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency),
                                                                    RetentionAmountLocalCurrency = crc.Sum(x => x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency)
                                                                }
                                                            }).ToList();

                reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup.RemoveAll(x => x.Coverage.LimitAmount.Value == 0);

                if (reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup.Count > 0)
                {
                    reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup.Select(CreateCoverageReinsuranceCumulusDTO(currencyDTOs, prefixDTOs, branchDTOs, lineBusiness, prefixCd)).ToList();
                }

                ReinsuranceCumulusDetailDTO economicGroupCumulusDetailDTO = new ReinsuranceCumulusDetailDTO
                {
                    Id = (int)CumulusDetailKeys.REINS_CUMULUS_DETAIL_ECONOMIC_GROUP,
                    AssigmentAmount = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup.Sum(x => x.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency + x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency),
                    RetentionAmount = reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup.Sum(x => x.ContractReinsuranceCumulus.RetentionAmountLocalCurrency),

                    CumulusDetail = new CumulusDetailDTO
                    {
                        Description = EnumHelper.GetEnumParameterValue<CumulusDetailKeys>(CumulusDetailKeys.REINS_CUMULUS_DETAIL_ECONOMIC_GROUP).ToString()

                    }
                };

                reinsuranceCumulusDetailDTOs.Add(individualCumulusDetailDTO);
                reinsuranceCumulusDetailDTOs.Add(consortiumCumulusDetailDTO);
                reinsuranceCumulusDetailDTOs.Add(economicGroupCumulusDetailDTO);

                reinsuranceCumulusDTO.ReinsuranceCumulusDetailDTOs = reinsuranceCumulusDetailDTOs;
            //}
            //else
            //{
            //    reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByIndividual = coverageReinsuranceCumulusDTOs;
            //    reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByConsortium = coverageReinsuranceCumulusDTOs;
            //    reinsuranceCumulusDTO.CoverageReinsuranceCumulusDTOsByEconomicGroup = coverageReinsuranceCumulusDTOs;
            //    reinsuranceCumulusDTO.ReinsuranceCumulusDetailDTOs = reinsuranceCumulusDetailDTOs;
            //}

            return reinsuranceCumulusDTO;
        }

        private Func<OperatingQuotaEventDTO, IEnumerable<CoverageReinsuranceCumulusDTO>> GetCumulusCoveragesByIndividual(List<CurrencyDTO> currencyDTOs)
        {
            try
            {
                return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
                {
                    return operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage.Select(CreateCoverageReinsuranceCumulusDTOsByOperatingQuotaEventDTOs(operatingQuotaEventDTO, currencyDTOs));
                };
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusByIndividual);
            }
        }

        private Func<ContractCoverageDTO, CoverageReinsuranceCumulusDTO> CreateCoverageReinsuranceCumulusDTOsByOperatingQuotaEventDTOs(OperatingQuotaEventDTO operatingQuotaEventDTO, List<CurrencyDTO> currencyDTOs)
        {
            return (ContractCoverageDTO contractCoverageDTO) =>
            {
                CoverageReinsuranceCumulusDTO coverageReinsuranceCumulusDTO = new CoverageReinsuranceCumulusDTO();
                coverageReinsuranceCumulusDTO = DTOAssembler.CreateCoverageReinsuranceCumulusDTOByOperatingQuotaEventDTO(operatingQuotaEventDTO);
                coverageReinsuranceCumulusDTO.ContractReinsuranceCumulus.LevelLimit = contractCoverageDTO.LevelLimit;
                coverageReinsuranceCumulusDTO.ContractReinsuranceCumulus.Contract = new ContractDTO
                {
                    ContractId = contractCoverageDTO.ContractId,
                    SmallDescription = contractCoverageDTO.ContractDescription,
                    Currency = new CurrencyDTO
                    {
                        Id = contractCoverageDTO.ContractCurrencyId,
                        Description = currencyDTOs.Find(x => x.Id == contractCoverageDTO.ContractCurrencyId).Description
                    }
                };

                if (contractCoverageDTO.ContractDescription == ContractTypeKeys.RETENCION.ToString())
                {
                    coverageReinsuranceCumulusDTO.ContractReinsuranceCumulus.RetentionAmountLocalCurrency = (contractCoverageDTO.Amount * operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage) / 100;
                }
                else
                {
                    coverageReinsuranceCumulusDTO.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency = (contractCoverageDTO.Amount * operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage) / 100;
                }

                coverageReinsuranceCumulusDTO.Coverage.LimitAmount.Value = coverageReinsuranceCumulusDTO.ContractReinsuranceCumulus.AssignmentAmountLocalCurrency + coverageReinsuranceCumulusDTO.ContractReinsuranceCumulus.RetentionAmountLocalCurrency;

                return coverageReinsuranceCumulusDTO;
            };
        }

        private Func<CoverageReinsuranceCumulusDTO, CoverageReinsuranceCumulusDTO> CreateCoverageReinsuranceCumulusDTO(List<CurrencyDTO> currencyDTOs, List<PrefixDTO> prefixDTOs, List<BranchDTO> branchDTOs, int lineBusinessID, int prefixCd)
        {
            return (CoverageReinsuranceCumulusDTO coverageReinsuranceCumulusDTO) =>
            {
                coverageReinsuranceCumulusDTO.Branch.Description = branchDTOs.Find(x => x.Id == coverageReinsuranceCumulusDTO.Branch.Id).Description;
                coverageReinsuranceCumulusDTO.Prefix.Description = prefixDTOs.Find(x => x.Id == coverageReinsuranceCumulusDTO.Prefix.Id).Description;
                coverageReinsuranceCumulusDTO.Currency.Description = currencyDTOs.Find(x => x.Id == coverageReinsuranceCumulusDTO.Currency.Id).Description;
                coverageReinsuranceCumulusDTO.Coverage.Description = DelegateService.underwritingIntegrationService.GetCoveragesByLineBusinessId(lineBusinessID).Where(x => x.CoverageId == coverageReinsuranceCumulusDTO.CoverageId).FirstOrDefault().Description;

                if (coverageReinsuranceCumulusDTO.Insured.IndividualId > 0)
                {
                    coverageReinsuranceCumulusDTO.Insured.FullName = DelegateService.uniquePersonIntegrationService.GetInsuredByIndividualId(coverageReinsuranceCumulusDTO.Insured.IndividualId).FullName;
                }
                if (coverageReinsuranceCumulusDTO.Consortium.IndividualId > 0)
                {
                    coverageReinsuranceCumulusDTO.Consortium.FullName = DelegateService.uniquePersonIntegrationService.GetInsuredByIndividualId(coverageReinsuranceCumulusDTO.Consortium.IndividualId).FullName;
                }
                if (coverageReinsuranceCumulusDTO.EconomicGroup.EconomicGroupId > 0)
                {
                    coverageReinsuranceCumulusDTO.EconomicGroup.EconomicGroupName = DelegateService.uniquePersonIntegrationService.GetEconomicGroupById(coverageReinsuranceCumulusDTO.EconomicGroup.EconomicGroupId).EconomicGroupName;
                }

                return coverageReinsuranceCumulusDTO;
            };
        }

        public List<DetailCumulusParticipantsEconomicGroupDTO> GetDetailCumulusParticipantsEconomicGroup(int economicGroupId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int sublineBusiness, int prefixCd)
        {
            try
            {
                List<DetailCumulusParticipantsEconomicGroupDTO> detailCumulusParticipantsEconomicGroupDTOs = new List<DetailCumulusParticipantsEconomicGroupDTO>();
                List<EconomicGroupDetailDTO> economicGroupDetailDTOs = new List<EconomicGroupDetailDTO>();
                List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                List<CurrencyDTO> currencyDTOs = DelegateService.commonIntegrationService.GetCurrencies().ToDTOs().ToList();
                EconomicGroupDTO economicGroupDTO = new EconomicGroupDTO();
                economicGroupDTO = DelegateService.uniquePersonIntegrationService.GetEconomicGroupById(economicGroupId).ToDTO();
                decimal totalCumulusEconomicGroup;
                List<int> idEconomicGroupParticipants = new List<int>();
                List<int> idEconomicGroupParticipantsWithCumulus = new List<int>();
                operatingQuotaEventDTOs = DelegateService.reinsuranceOperatingQuotaIntegrationServices.GetCumulusCoveragesByIndividual(economicGroupId, lineBusiness, dateCumulus, IsFuture, sublineBusiness, prefixCd).Select(DTOAssembler.CreateOperatingQuotaEventDTO).ToList();
                economicGroupDetailDTOs = DelegateService.uniquePersonIntegrationService.GetEconomicGroupDetailById(economicGroupId).ToDTOs()
                                                                                                                                    .ToList()
                                                                                                                                    .FindAll(x => x.Enabled == true);

                detailCumulusParticipantsEconomicGroupDTOs = operatingQuotaEventDTOs.Select(GetCumulusByEconomicGroupParticipant(economicGroupDTO, economicGroupDetailDTOs, currencyDTOs, dateCumulus))
                                                                                    .GroupBy(x => new { x.EconomicGroupDetail.IndividualId })
                                                                                    .Select(dcpeg => new DetailCumulusParticipantsEconomicGroupDTO
                                                                                    {
                                                                                        EconomicGroup = dcpeg.FirstOrDefault().EconomicGroup,
                                                                                        EconomicGroupDetail = dcpeg.FirstOrDefault().EconomicGroupDetail,
                                                                                        Insured = dcpeg.FirstOrDefault().Insured,
                                                                                        Enable = dcpeg.FirstOrDefault().Enable,
                                                                                        DateUpdated = dcpeg.FirstOrDefault().DateUpdated,
                                                                                        AssignmentTotalCumulusIndividual = dcpeg.Sum(x => x.AssignmentTotalCumulusIndividual),
                                                                                        RetentionTotalCumulusIndividual = dcpeg.Sum(x => x.RetentionTotalCumulusIndividual),
                                                                                        TotalCumulusIndividual = dcpeg.Sum(x => x.TotalCumulusIndividual),
                                                                                        TotalPremiumsIndividual = dcpeg.Sum(x => x.TotalPremiumsIndividual)
                                                                                    }).ToList();

                totalCumulusEconomicGroup = detailCumulusParticipantsEconomicGroupDTOs.Sum(x => x.TotalCumulusIndividual);

                idEconomicGroupParticipantsWithCumulus = detailCumulusParticipantsEconomicGroupDTOs.Select(x => x.Insured.IndividualId).ToList();
                idEconomicGroupParticipants = economicGroupDetailDTOs.Select(x => x.IndividualId).ToList();
                idEconomicGroupParticipants.RemoveAll(x => idEconomicGroupParticipantsWithCumulus.Contains(x));
                idEconomicGroupParticipants.RemoveAll(x => x == 0);


                if (idEconomicGroupParticipants.Count > 0)
                {
                    detailCumulusParticipantsEconomicGroupDTOs.AddRange(idEconomicGroupParticipants.Select(GetCumulusByEconomicGroupParticipantWithoutCumulus(economicGroupDTO, economicGroupDetailDTOs)));
                }

                detailCumulusParticipantsEconomicGroupDTOs = detailCumulusParticipantsEconomicGroupDTOs.Select(x =>
                {
                    x.TotalCumulusEconomicGroup = totalCumulusEconomicGroup;
                    return x;
                }).ToList();

                return detailCumulusParticipantsEconomicGroupDTOs;

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCumulusByIndividual);
            }
        }

        private Func<int, DetailCumulusParticipantsEconomicGroupDTO> GetCumulusByEconomicGroupParticipantWithoutCumulus(EconomicGroupDTO economicGroupDTO, List<EconomicGroupDetailDTO> economicGroupDetailDTOs)
        {
            return (int individualId) =>
            {

                EconomicGroupDetailDTO economicGroupDetailDTO = new EconomicGroupDetailDTO();
                economicGroupDetailDTO = economicGroupDetailDTOs.Find(x => x.IndividualId == individualId);
                List<ContractReinsuranceCumulusDTO> contractReinsuranceCumulusDTOs = new List<ContractReinsuranceCumulusDTO>();

                DetailCumulusParticipantsEconomicGroupDTO detailCumulusParticipantsEconomicGroupDTO = new DetailCumulusParticipantsEconomicGroupDTO();
                detailCumulusParticipantsEconomicGroupDTO.Insured = new InsuredDTO();
                detailCumulusParticipantsEconomicGroupDTO.EconomicGroup = economicGroupDTO;
                detailCumulusParticipantsEconomicGroupDTO.Insured.IndividualId = individualId;
                detailCumulusParticipantsEconomicGroupDTO.Insured.FullName = DelegateService.uniquePersonIntegrationService.GetInsuredByIndividualId(individualId).FullName;

                detailCumulusParticipantsEconomicGroupDTO.EconomicGroupDetail = economicGroupDetailDTO;
                detailCumulusParticipantsEconomicGroupDTO.Enable = economicGroupDetailDTO.Enabled;
                detailCumulusParticipantsEconomicGroupDTO.DateUpdated = economicGroupDTO.EnteredDate;

                return detailCumulusParticipantsEconomicGroupDTO;
            };
        }

        private Func<OperatingQuotaEventDTO, DetailCumulusParticipantsEconomicGroupDTO> GetCumulusByEconomicGroupParticipant(EconomicGroupDTO economicGroupDTO, List<EconomicGroupDetailDTO> economicGroupDetailDTOs, List<CurrencyDTO> currencyDTOs, DateTime dateCumulus)
        {
            return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
            {
                List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                operatingQuotaEventDTOs.Add(operatingQuotaEventDTO);
                EconomicGroupDetailDTO economicGroupDetailDTO = new EconomicGroupDetailDTO();
                economicGroupDetailDTO = economicGroupDetailDTOs.Find(x => x.IndividualId == operatingQuotaEventDTO.IdentificationId);
                List<ContractReinsuranceCumulusDTO> contractReinsuranceCumulusDTOs = new List<ContractReinsuranceCumulusDTO>();

                operatingQuotaEventDTOs.Select(GetCumulusContractsByIndividual(currencyDTOs, dateCumulus)).ToList().ForEach(x =>
                {
                    contractReinsuranceCumulusDTOs.AddRange(x);
                });

                DetailCumulusParticipantsEconomicGroupDTO detailCumulusParticipantsEconomicGroupDTO = new DetailCumulusParticipantsEconomicGroupDTO();
                detailCumulusParticipantsEconomicGroupDTO.Insured = new InsuredDTO();
                detailCumulusParticipantsEconomicGroupDTO.EconomicGroup = economicGroupDTO;
                detailCumulusParticipantsEconomicGroupDTO.Insured.IndividualId = operatingQuotaEventDTO.IdentificationId;
                detailCumulusParticipantsEconomicGroupDTO.Insured.FullName = DelegateService.uniquePersonIntegrationService.GetInsuredByIndividualId(operatingQuotaEventDTO.IdentificationId).FullName;

                detailCumulusParticipantsEconomicGroupDTO.EconomicGroupDetail = economicGroupDetailDTO;
                detailCumulusParticipantsEconomicGroupDTO.Enable = economicGroupDetailDTO.Enabled;
                detailCumulusParticipantsEconomicGroupDTO.DateUpdated = economicGroupDTO.EnteredDate;

                if (economicGroupDetailDTO.IndividualId == operatingQuotaEventDTO.IdentificationId)
                {
                    detailCumulusParticipantsEconomicGroupDTO.RetentionTotalCumulusIndividual = contractReinsuranceCumulusDTOs.Sum(crc => crc.RetentionAmountLocalCurrency);
                    detailCumulusParticipantsEconomicGroupDTO.AssignmentTotalCumulusIndividual = contractReinsuranceCumulusDTOs.Sum(crc => crc.AssignmentAmountLocalCurrency);
                    detailCumulusParticipantsEconomicGroupDTO.TotalCumulusIndividual = contractReinsuranceCumulusDTOs.Sum(crc => crc.AssignmentAmountLocalCurrency + crc.RetentionAmountLocalCurrency);
                    detailCumulusParticipantsEconomicGroupDTO.TotalPremiumsIndividual = contractReinsuranceCumulusDTOs.Sum(crc => crc.AssignmentPremiumAmountLocalCurrency + crc.RetentionPremiumAmountLocalCurrency);
                }

                return detailCumulusParticipantsEconomicGroupDTO;
            };
        }

        public string GenerateFileCumulusByIndividual(string fileName, List<CoverageReinsuranceCumulusDTO> coverageReinsuranceCumulusDTOs)
        {
            try
            {
                return CreateExcel(fileName, coverageReinsuranceCumulusDTOs);
            }
            catch (BusinessException)
            {
                throw new BusinessException("Error al generar excel");
            }
        }

        private string CreateExcel(string fileName, List<CoverageReinsuranceCumulusDTO> coverageReinsuranceCumulusDTOs)
        {
            string filePath = ConfigurationManager.AppSettings["ReportExportPath"] + "\\" + fileName + ".xlsx";
            using (SpreadsheetDocument spreedDoc = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wbPart = spreedDoc.WorkbookPart;
                if (wbPart == null)
                {
                    wbPart = spreedDoc.AddWorkbookPart();
                    wbPart.Workbook = new Workbook();
                }

                string sheetName = fileName;
                WorksheetPart worksheetPart = null;
                worksheetPart = wbPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();

                worksheetPart.Worksheet = new Worksheet(sheetData);

                if (wbPart.Workbook.Sheets == null)
                {
                    wbPart.Workbook.AppendChild<Sheets>(new Sheets());
                }

                var sheet = new Sheet()
                {
                    Id = wbPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = sheetName
                };

                WorkbookStylesPart wbStylesPart;
                wbStylesPart = wbPart.AddNewPart<WorkbookStylesPart>();
                wbStylesPart.Stylesheet = new Stylesheet();
                wbStylesPart.Stylesheet.NumberingFormats = new NumberingFormats();
                wbStylesPart.Stylesheet.CellFormats = new CellFormats();

                NumberingFormat numberingFormat = new NumberingFormat();
                numberingFormat.NumberFormatId = 4;
                numberingFormat.FormatCode = "\"" + CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol + "\"\\ " + "#,##0.00";
                wbStylesPart.Stylesheet.NumberingFormats.Append(numberingFormat);

                CellFormat cellFormat = new CellFormat()
                {
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0,
                    FormatId = 0,
                    NumberFormatId = numberingFormat.NumberFormatId,
                    ApplyNumberFormat = BooleanValue.FromBoolean(true),
                    ApplyFont = true,
                };

                wbStylesPart.Stylesheet.CellFormats.AppendChild<CellFormat>(cellFormat);
                wbStylesPart.Stylesheet.CellFormats.Count = UInt32Value.FromUInt32((uint)wbStylesPart.Stylesheet.CellFormats.ChildElements.Count);
                wbStylesPart.Stylesheet.Save();


                var workingSheet = ((WorksheetPart)wbPart.GetPartById(sheet.Id)).Worksheet;
                int rowindex = 1;

                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1) //Title
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row rowTitle = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        rowTitle.RowIndex = (UInt32)rowindex;
                        rowTitle.AppendChild(AddCellWithText("Pólizas del Afianzado : " + fileName, CellValues.String));
                        sheetData.AppendChild(rowTitle);
                        rowindex++;
                    }
                    else if (i == 2) //Header
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row rowHeader = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        rowHeader.RowIndex = (UInt32)rowindex;
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.Policy, CellValues.String));
                        if (coverageReinsuranceCumulusDTOs.FirstOrDefault().Consortium.FullName != null)
                        {
                            rowHeader.AppendChild(AddCellWithText(Resources.Resources.ConsortiumName, CellValues.String));
                        }
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.Coverage, CellValues.String));
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.ValidatyFrom, CellValues.String));
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.ValidatyUntil, CellValues.String));
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.ValueInsured, CellValues.String));
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.Currency, CellValues.String));
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.Insured, CellValues.String));
                        rowHeader.AppendChild(AddCellWithText(Resources.Resources.Branch, CellValues.String));
                        sheetData.AppendChild(rowHeader);
                        rowindex++;
                    }
                    else //Content
                    {
                        foreach (CoverageReinsuranceCumulusDTO coverageReinsuranceCumulusDTO in coverageReinsuranceCumulusDTOs)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row rowContent = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            rowContent.RowIndex = (UInt32)rowindex;

                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.DocumentNum.ToString(), CellValues.Number));
                            if (coverageReinsuranceCumulusDTO.Consortium.FullName != null)
                            {
                                rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.Consortium.FullName.ToString(), CellValues.String));
                            }
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.Coverage.Description.ToString(), CellValues.String));
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.CoverageCurrentFrom.ToString("dd/MM/yyyy"), CellValues.Date));
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.CoverageCurrentTo.ToString("dd/MM/yyyy"), CellValues.Date));
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.Coverage.LimitAmount.Value.ToString(), CellValues.Number));
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.Currency.Description.ToString(), CellValues.String));
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.Insured.FullName.ToString(), CellValues.String));
                            rowContent.AppendChild(AddCellWithText(coverageReinsuranceCumulusDTO.Branch.Description.ToString(), CellValues.String));
                            sheetData.AppendChild(rowContent);
                            rowindex++;
                        }
                    }
                }

                wbPart.Workbook.Sheets.AppendChild(sheet);
                wbPart.Workbook.Save();
                spreedDoc.Close();
            }

            return filePath;
        }

        private Cell AddCellWithText(string txt, CellValues dataType)
        {
            Cell cell = new Cell();

            switch ((int)dataType)
            {
                case 4:
                case 6:
                    cell.CellValue = new CellValue(txt);
                    cell.DataType = new EnumValue<CellValues>(dataType);
                    cell.StyleIndex = Convert.ToUInt32(0);
                    break;
                case 1:
                    cell.CellValue = new CellValue(txt);
                    cell.DataType = new EnumValue<CellValues>(dataType);
                    cell.StyleIndex = Convert.ToUInt32(0);
                    break;
            }
            return cell;
        }
        #endregion

        #region Retención Prioritaria

        public List<PriorityRetentionDTO> GetPriorityRetentions()
        {
            try
            {
                PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                return priorityRetentionDAO.GetPriorityRetentions().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPriorityRetentions);
            }
        }

        public List<PriorityRetentionDTO> SavePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionAdded)
        {
            try
            {
                foreach (PriorityRetentionDTO priorityRetentionDTO in lstPriorityRetentionAdded)
                {
                    PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                    priorityRetentionDAO.SavePriorityRetention(priorityRetentionDTO.ToModel());
                }
                return lstPriorityRetentionAdded;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSavePriorityRetentions);
            }
        }

        public List<PriorityRetentionDTO> UpdatePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionModified)
        {
            try
            {
                foreach (PriorityRetentionDTO priorityRetentionDTO in lstPriorityRetentionModified)
                {
                    PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                    priorityRetentionDAO.UpdatePriorityRetention(priorityRetentionDTO.ToModel());
                }
                return lstPriorityRetentionModified;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdatePriorityRetentions);
            }
        }

        public bool DeletePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionDelete)
        {
            try
            {
                foreach (PriorityRetentionDTO priorityRetentionDTO in lstPriorityRetentionDelete)
                {
                    PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                    priorityRetentionDAO.DeletePriorityRetention(priorityRetentionDTO.Id);
                }
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeletePriorityRetentions);
            }
        }

        public List<PriorityRetentionDTO> GetPriorityRetentionDTOsByPrefixCd(int prefixCd)
        {
            try
            {
                PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
                return priorityRetentionDAO.GetPriorityRetentionsByPrefixCd(prefixCd).Where(x => x.Enabled == true).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPriorityRetentionByLineBusiness);
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
                        priorityRetentionDTOs = GetPriorityRetentionDTOsByPrefixCd(policyDTO.Prefix.Id);
                        priorityRetentionDTO = priorityRetentionDTOs.Where(x => policyDTO.Endorsement.IssueDate >= x.ValidityFrom && policyDTO.Endorsement.IssueDate <= x.ValidityTo).FirstOrDefault();

                        if (priorityRetentionDTO != null)
                        {
                            List<CurrencyDTO> currencyDTOs = DelegateService.commonIntegrationService.GetCurrencies().ToDTOs().ToList();
                            DelegateService.reinsuranceOperatingQuotaIntegrationServices.GetCumulusCoveragesByIndividual(policyDTO.Endorsement.Risks.FirstOrDefault().IndividualId, 0, DateTime.Now, true, 0, policyDTO.Prefix.Id, true).Select(DTOAssembler.CreateOperatingQuotaEventDTO).Select(GetCumulusContractsByIndividual(currencyDTOs, DateTime.Now)).ToList().ForEach(x =>
                            {
                                contractReinsuranceCumulusDTOs.AddRange(x);
                            });

                            reinsuranceCumulusDTO.RetentionTotalCumulus = contractReinsuranceCumulusDTOs.Sum(x => x.RetentionAmountLocalCurrency);

                            PriorityRetentionDetailDTO priorityRetentionDetailDTO = new PriorityRetentionDetailDTO();
                            List<PriorityRetentionDetailDTO> priorityRetentionDetailDTOs = new List<PriorityRetentionDetailDTO>();
                            priorityRetentionDetailDTOs = priorityRetentionDAO.GetPriorityRetentionDetailsByPolicyIdEndorsementId(policyDTO.Id, policyDTO.Endorsement.Id).ToDTOs().ToList();

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
                                priorityRetentionDAO.SavePriorityRetentionDetail(priorityRetentionDetailDTO.ToModel());
                            }
                        }
                        break;
                };
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidatePriorityRetention);
            }
        }

        public void ValidatePriorityRetentionByProcessId(int processId)
        {
            try
            {
                List<PolicyDTO> policyDTOs = new List<PolicyDTO>();
                List<TempRiskCoverageDTO> tempRiskCoverageDTOs = new List<TempRiskCoverageDTO>();
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                TempRiskCoverageDAO tempRiskCoverageDAO = new TempRiskCoverageDAO();
                policyDTOs = reinsuranceDAO.GetTempIssuesByProcessId(processId).ToDTOs().ToList();
                foreach (PolicyDTO policyDTO in policyDTOs)
                {
                    ValidatePriorityRetention(policyDTO);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidatePriorityRetention);
            }
        }

        public bool CanPriorityRetentionUpdated(int priorityRetentionId)
        {
            PriorityRetentionDAO priorityRetentionDAO = new PriorityRetentionDAO();
            PriorityRetentionDetailDTO priorityRetentionDetailDTO = new PriorityRetentionDetailDTO();
            priorityRetentionDetailDTO = priorityRetentionDAO.GetPriorityRetentionDetailsByPriorityRetentionId(priorityRetentionId).ToDTOs().ToList().LastOrDefault();

            if (priorityRetentionDetailDTO != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region Endosos

        public List<TempLayerDistributionsDTO> GetTempLayerDistributionByEndorsementId(int endorsementId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<ReinsuranceLayerIssuanceDTO> layerIssuancesDTO = new List<ReinsuranceLayerIssuanceDTO>();
                layerIssuancesDTO = GetTempLayerDistribution(endorsementId);
                return reinsuranceBusiness.GetTempLayerDistributionByEndorsementId(endorsementId, layerIssuancesDTO).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempLayerDistributionByEndorsementId);
            }
        }

        public List<TempLineCumulusIssuanceDTO> GetTempLineeCumulusByIssuance(int tempIssueLayerId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ReinsuranceLayerIssuanceDTO reinsuranceLineDTO = GetTempLineCumulus(tempIssueLayerId);
                return reinsuranceBusiness.GetTempLineeCumulusByIssuance(tempIssueLayerId, reinsuranceLineDTO).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempLineeCumulusByIssuance);
            }
        }

        public List<ReinsuranceDistributionDTO> GetDistributionByReinsurance(int layerId)
        {
            try
            {
                DistributionDAO distributionDAO = new DistributionDAO();
                return distributionDAO.GetDistributionByReinsurance(layerId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetDistributionByReinsurance);
            }
        }

        public List<ReinsuranceLayerIssuanceDTO> GetTempFacultativeCompanies(int endorsementId, int layerNumber, int? lineId, string cumulusKey)
        {
            try
            {
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                return tempFacultativeCompanyDAO.GetTempFacultativeCompanies(endorsementId, layerNumber, lineId, cumulusKey).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempFacultativeCompanies);
            }
        }

        public List<ReinsuranceLayerIssuanceDTO> GetTempLayerDistribution(int endorsementId)
        {
            try
            {
                DistributionDAO distributionDAO = new DistributionDAO();
                return DTOAssembler.CreateTempLayerDistributions(distributionDAO.GetTempLayerDistribution(endorsementId));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempLayerDistribution);
            }
        }

        public ReinsuranceAllocationDTO GetTempAllocationById(int tempIssueAllocationId)
        {
            try
            {
                TempAllocationDAO tempAllocationDAO = new TempAllocationDAO();
                return tempAllocationDAO.GetTempAllocationById(tempIssueAllocationId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempAllocationById);
            }
        }

        public ReinsuranceLayerIssuanceDTO GetTempIssueLayerById(int tmpIssueLayerId)
        {
            try
            {
                TempIssueLayerDAO tempIssueLayerDAO = new TempIssueLayerDAO();
                return tempIssueLayerDAO.GetTempIssueLayerById(tmpIssueLayerId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempIssueLayerById);
            }
        }

        public ReinsuranceLayerIssuanceDTO GetTempLineCumulus(int tempIssueLayerId)
        {
            try
            {
                LineDAO lineDAO = new LineDAO();
                return lineDAO.GetTempLineCumulus(tempIssueLayerId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempLineCumulus);
            }
        }

        public ReinsuranceLineDTO GetTempAllocation(int tempLayerLineId)
        {
            try
            {
                TempAllocationDAO tempAllocationDAO = new TempAllocationDAO();
                return tempAllocationDAO.GetTempAllocation(tempLayerLineId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempAllocation);
            }
        }

        public ReinsuranceLineDTO GetTempLayerLineById(int tmpIssueLayerId)
        {
            try
            {
                TempLayerLineDAO tempLayerLineDAO = new TempLayerLineDAO();
                return tempLayerLineDAO.GetTempLayerLineById(tmpIssueLayerId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempLayerLineById);
            }
        }

        public void DeleteTempIssueLayer(int tempIssueLayerId)
        {
            try
            {
                TempIssueLayerDAO tempIssueLayerDAO = new TempIssueLayerDAO();
                tempIssueLayerDAO.DeleteTempIssueLayer(tempIssueLayerId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteTempIssueLayer);
            }
        }

        public void SaveTempFacultativeCompany(ReinsuranceAllocationDTO reinsuranceFacultativeDTO)
        {
            try
            {
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                tempFacultativeCompanyDAO.SaveTempFacultativeCompany(reinsuranceFacultativeDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveTempFacultativeCompany);
            }
        }

        public void SaveTempIssueLayer(ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO)
        {
            try
            {
                TempIssueLayerDAO tempIssueLayerDAO = new TempIssueLayerDAO();
                tempIssueLayerDAO.SaveTempIssueLayer(reinsuranceLayerIssuanceDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveTempIssueLayer);
            }
        }

        public void UpdateTempAllocation(ReinsuranceAllocationDTO tempAllocationDTO)
        {
            try
            {
                TempAllocationDAO tempAllocationDAO = new TempAllocationDAO();
                tempAllocationDAO.UpdateTempAllocation(tempAllocationDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateTempAllocation);
            }
        }

        public void UpdateTempFacultativeCompany(ReinsuranceAllocationDTO reinsuranceFacultativeDTO)
        {
            try
            {
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                tempFacultativeCompanyDAO.UpdateTempFacultativeCompany(reinsuranceFacultativeDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateTempFacultativeCompany);
            }
        }

        public void UpdateTempIssueLayer(ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO)
        {
            try
            {
                TempIssueLayerDAO tempIssueLayerDAO = new TempIssueLayerDAO();
                tempIssueLayerDAO.UpdateTempIssueLayer(reinsuranceLayerIssuanceDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateTempIssueLayer);
            }
        }

        public void UpdateTempLayerLine(ReinsuranceLineDTO reinsuranceLineDTO)
        {
            try
            {
                TempLayerLineDAO tempLayerLineDAO = new TempLayerLineDAO();
                tempLayerLineDAO.UpdateTempLayerLine(reinsuranceLineDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdateTempLayerLine);
            }
        }

        public int GetReinsuranceCompanyIdByFacultativeIdAndIndividualId(int facultativeId, int individualId)
        {
            try
            {
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                return tempFacultativeCompanyDAO.GetReinsuranceCompanyIdByFacultativeIdAndIndividualId(facultativeId, individualId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceCompanyIdByLevelIdAndIndividualId);
            }
        }

        public List<EndorsementDTO> GetEndorsementByPolicyId(int branchCode, int prefixCode, decimal documentNumber, int endorsementNumber)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                List<EndorsementDTO> endorsementDTOs = new List<EndorsementDTO>();
                endorsementDTOs = DTOAssembler.CreateEndorsementDTOsByUNDDTOEndorsementDTOs(DelegateService.underwritingIntegrationService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(Convert.ToInt32(prefixCode), Convert.ToInt32(branchCode), documentNumber));

                if (endorsementDTOs.Count > 0)
                {

                    List<EndorsementDTO> endorsementResultDTOs = new List<EndorsementDTO>();

                    foreach (EndorsementDTO endorsementDTO in endorsementDTOs)
                    {
                        endorsementResultDTOs.Add(ModelAssembler.CreateEndorsementDTOByTEMPIntegrationEndorsementDTO(DelegateService.tempCommonIntegrationService.GetEndorsementByPolicyIdEndorsementId(endorsementDTO.PolicyId, endorsementDTO.Id)));
                    }

                    if (endorsementNumber < 0)
                    {
                        // Trae todos los endosos
                        return endorsementResultDTOs.OrderByDescending(x => x.EndorsementNumber).ToList();
                    }
                    else
                    {
                        // De todos los endosos filtra solo el endoso requerido
                        return endorsementResultDTOs.Where(x => x.EndorsementNumber == endorsementNumber).ToList();

                    }

                }

                return new List<EndorsementDTO>();

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetEndorsementByPolicyId);
            }
        }

        public List<ReinsuranceDistributionHeaderDTO> GetReinsuranceDistributionByEndorsementId(int endorsementId)
        {
            try
            {
                DistributionDAO distributionDAO = new DistributionDAO();
                return distributionDAO.GetReinsuranceDistributionByEndorsementId(endorsementId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceDistributionByEndorsementId);
            }
        }

        public List<ReinsuranceLayerIssuanceDTO> ModificationReinsuranceLayer(int endorsementId, int tempIssueLayerId)
        {
            try
            {
                TempIssueLayerDAO tempIssueLayerDAO = new TempIssueLayerDAO();
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                List<ReinsuranceLayerIssuanceDTO> tempLayerDistributions = GetTempLayerDistribution(endorsementId);

                if (tempLayerDistributions.Count > 0)
                {
                    if (tempIssueLayerId == 0)
                    {
                        reinsuranceLayerIssuanceDTOs.Add(new ReinsuranceLayerIssuanceDTO
                        {
                            SumPercentage = 0,
                            PremiumPercentage = 0,
                            ReinsuranceLayerId = 0,
                            LayerNumber = tempLayerDistributions[tempLayerDistributions.Count - 1].LayerNumber + 1,
                            ReinsSourceId = GetTempIssueLayerById(tempLayerDistributions[0].ReinsuranceLayerId).TemporaryIssueId
                        });

                        return reinsuranceLayerIssuanceDTOs;
                    }

                    ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO = tempIssueLayerDAO.GetTempIssueLayerById(tempIssueLayerId).ToDTO();

                    reinsuranceLayerIssuanceDTOs.Add(new ReinsuranceLayerIssuanceDTO
                    {
                        SumPercentage = reinsuranceLayerIssuanceDTO.LayerPercentage == 0 ? 0 : Convert.ToDecimal(reinsuranceLayerIssuanceDTO.LayerPercentage.ToString().Replace(",", ".")),
                        PremiumPercentage = Convert.ToDecimal(reinsuranceLayerIssuanceDTO.PremiumPercentage == 0 ? "" : reinsuranceLayerIssuanceDTO.PremiumPercentage.ToString().Replace(",", ".")),
                        ReinsuranceLayerId = reinsuranceLayerIssuanceDTO.ReinsuranceLayerId,
                        LayerNumber = reinsuranceLayerIssuanceDTO.LayerNumber,
                        ReinsSourceId = reinsuranceLayerIssuanceDTO.TemporaryIssueId
                    });

                    return reinsuranceLayerIssuanceDTOs;
                }
                return new List<ReinsuranceLayerIssuanceDTO>();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorModificationReinsuranceLayerDialog);
            }
        }

        public bool DeleteTempIssueLayerByEndorsementId(int endorsementId, int tempIssueLayerId)
        {
            try
            {
                List<ReinsuranceLayerIssuanceDTO> tempLayerDistribution = GetTempLayerDistribution(endorsementId);
                // No permite borrar cuando solo haya una l�nea
                if (tempLayerDistribution.Count > 1)
                {
                    int cont = 1;

                    foreach (ReinsuranceLayerIssuanceDTO reinsuranceLayer in tempLayerDistribution)
                    {
                        if (reinsuranceLayer.ReinsuranceLayerId == tempIssueLayerId)
                        {
                            if (cont == tempLayerDistribution.Count)
                            {
                                DeleteTempIssueLayer(tempIssueLayerId);
                                return true;
                            }
                        }
                        else
                        {
                            cont++;
                        }
                    }
                }
                return false;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteTempIssueLayerByEndorsementId);
            }
        }

        public List<TempAllocationDTO> GetReinsuranceAllocationBytempLayerLineId(int tempLayerLineId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ReinsuranceLineDTO reinsuranceLineDTO = new ReinsuranceLineDTO();
                reinsuranceLineDTO = GetTempAllocation(tempLayerLineId);
                return reinsuranceBusiness.GetReinsuranceAllocationBytempLayerLineId(tempLayerLineId, reinsuranceLineDTO).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceAllocationBytempLayerLineId);
            }
        }

        public bool ValidateLineCumulus(PolicyDTO policyDTO)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.ValidateLineCumulus(policyDTO.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorValidateLineCumulus);
            }
        }

        public decimal[] CalculationValue(int processId, int? layerNumber, int? lineId, string cumulusKey)
        {
            try
            {
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                return tempFacultativeCompanyDAO.CalculationValue(processId, layerNumber, lineId, cumulusKey);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorCalculationValue);
            }
        }

        public ReinsuranceDTO ManualIssueReinsurance(PolicyDTO policy, int userId, int? processId)
        {
            try
            {
                ReinsuranceBusiness reinsuranceBusiness = new ReinsuranceBusiness();
                ReinsuranceDTO reinsuranceResult = new ReinsuranceDTO();
                List<ReinsuranceLayerDTO> reinsuranceLayersResult = new List<ReinsuranceLayerDTO>();
                List<ReinsuranceLineDTO> reinsuranceLinesResult = new List<ReinsuranceLineDTO>();
                List<ReinsuranceAllocationDTO> reinsuranceAllocationsResult = new List<ReinsuranceAllocationDTO>();
                List<ReinsuranceCumulusRiskCoverageDTO> reinsuranceCumulusRiskCoveragesResult = new List<ReinsuranceCumulusRiskCoverageDTO>();
                CumulusTypeDAO cumulusTypeDAO = new CumulusTypeDAO();
                LineDAO lineDAO = new LineDAO();
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                ValidatePriorityRetentionByProcessId(Convert.ToInt32(processId));

                List<LineCumulusKeyDTO> linesCumulusKey = lineDAO.GetCumulusKeyRiskCoverageByEndorsement(policy.Endorsement.Id).ToDTOs().ToList();

                if (linesCumulusKey.Count > 0)
                {
                    List<LineDTO> lineDTOs = new List<LineDTO>();

                    foreach (LineCumulusKeyDTO lineCumulusKey in linesCumulusKey)
                    {
                        LineDTO lineDTO = new LineDTO();
                        lineDTO = GetLineParametrizationByLineId(lineCumulusKey.Line.LineId);
                        lineDTOs.Add(lineDTO);
                    }

                    cumulusTypeDAO.SetCumulusByEndorsement(policy.Endorsement.Id);

                    DataTable resinsurance = reinsuranceDAO.CalculateLoadReinsurance(policy.Endorsement.Id, 1);

                    if (resinsurance != null && resinsurance.Rows.Count > 0)
                    {
                        if (resinsurance.Rows[0][1].ToString() == "0" && resinsurance.Columns.Count < 8)
                        {
                            reinsuranceResult.ReinsuranceId = 0;
                            return reinsuranceResult;
                        }

                        foreach (DataRow item in resinsurance.Rows)
                        {

                            reinsuranceResult.ReinsuranceId = Convert.ToInt32(item[0]);
                            reinsuranceResult.Number = Convert.ToInt32(item[1]);
                            reinsuranceResult.ProcessDate = Convert.ToDateTime(item[2]);
                            reinsuranceResult.IssueDate = Convert.ToDateTime(item[3]);
                            reinsuranceResult.ValidityFrom = Convert.ToDateTime(item[4]);
                            reinsuranceResult.ValidityTo = Convert.ToDateTime(item[5]);
                            reinsuranceResult.IsAutomatic = false;

                            switch (Convert.ToInt32(item[6]))
                            {
                                case 1:
                                    reinsuranceResult.Movements = ReinsuranceServices.Enums.Movements.Original;
                                    break;
                                case 2:
                                    reinsuranceResult.Movements = ReinsuranceServices.Enums.Movements.Counterpart;
                                    break;
                                case 3:
                                    reinsuranceResult.Movements = ReinsuranceServices.Enums.Movements.Adjustment;
                                    break;
                            }

                            reinsuranceResult.UserId = Convert.ToInt32(item[7]);

                            break;
                        }

                    }

                    DataTable layers = reinsuranceDAO.CalculateLoadReinsurance(policy.Endorsement.Id, 2);

                    DataTable lines = reinsuranceDAO.CalculateLoadReinsurance(policy.Endorsement.Id, 3);

                    DataTable allocations = reinsuranceDAO.CalculateLoadReinsurance(policy.Endorsement.Id, 4);

                    foreach (DataRow layer in layers.Rows)
                    {
                        reinsuranceLinesResult = new List<ReinsuranceLineDTO>();

                        foreach (DataRow line in lines.Rows)
                        {
                            if (Convert.ToInt32(layer[0]) == Convert.ToInt32(line[1]))
                            {
                                reinsuranceAllocationsResult = new List<ReinsuranceAllocationDTO>();

                                foreach (DataRow allocation in allocations.Rows)
                                {

                                    if (Convert.ToInt32(line[0]) == Convert.ToInt32(allocation[0]))
                                    {
                                        ReinsuranceAllocationDTO reinsuranceAllocation = new ReinsuranceAllocationDTO();
                                        reinsuranceAllocation.ReinsuranceAllocationId = Convert.ToInt32(allocation[1]);
                                        reinsuranceAllocation.Currency = new CurrencyDTO();
                                        reinsuranceAllocation.Currency.Id = Convert.ToInt32(allocation[2]);
                                        reinsuranceAllocation.Facultative = Convert.ToBoolean(allocation[3]);
                                        reinsuranceAllocation.Contract = new ContractDTO();
                                        reinsuranceAllocation.Contract.ContractId = Convert.ToInt32(allocation[4]);
                                        reinsuranceAllocation.Amount = new AmountDTO();
                                        reinsuranceAllocation.Amount.Value = Convert.ToDecimal(allocation[5]);
                                        reinsuranceAllocation.Premium = new AmountDTO();
                                        reinsuranceAllocation.Premium.Value = Convert.ToDecimal(allocation[6]);
                                        reinsuranceAllocation.Commission = new AmountDTO();
                                        reinsuranceAllocation.Commission.Value = Convert.ToDecimal(allocation[7]);
                                        reinsuranceAllocationsResult.Add(reinsuranceAllocation);
                                    }
                                }

                                ReinsuranceLineDTO reinsuranceLine = new ReinsuranceLineDTO();
                                reinsuranceLine.ReinsuranceLineId = Convert.ToInt32(line[0]);
                                reinsuranceLine.Line = new LineDTO();
                                reinsuranceLine.Line.LineId = Convert.ToInt32(line[2]);
                                reinsuranceLine.CumulusKey = line[3].ToString();
                                reinsuranceLine.ReinsuranceAllocations = reinsuranceAllocationsResult;

                                reinsuranceCumulusRiskCoveragesResult = new List<ReinsuranceCumulusRiskCoverageDTO>();
                                string keyMasterLevelUp = line[2].ToString() + line[3].ToString();

                                foreach (LineCumulusKeyDTO lineCumulusKey in linesCumulusKey)
                                {
                                    string keyMasterLevelDown = lineCumulusKey.Line.LineId.ToString() + lineCumulusKey.CumulusKey;

                                    if (keyMasterLevelUp == keyMasterLevelDown)
                                    {
                                        foreach (LineCumulusKeyRiskCoverageDTO lineCumulusKeyRiskCoverage in lineCumulusKey.LineCumulusKeyRiskCoverages)
                                        {
                                            ReinsuranceCumulusRiskCoverageDTO reinsuranceCumulusRiskCoverage = new ReinsuranceCumulusRiskCoverageDTO();
                                            reinsuranceCumulusRiskCoverage.RiskNumber = lineCumulusKeyRiskCoverage.RiskNumber;
                                            reinsuranceCumulusRiskCoverage.CoverageNumber = lineCumulusKeyRiskCoverage.CoverageNumber;
                                            reinsuranceCumulusRiskCoverage.ReinsuranceCumulusRiskCoverageId = lineCumulusKeyRiskCoverage.Id;
                                            reinsuranceCumulusRiskCoveragesResult.Add(reinsuranceCumulusRiskCoverage);
                                        }
                                    }

                                }

                                reinsuranceLine.ReinsuranceCumulusRiskCoverages = reinsuranceCumulusRiskCoveragesResult;
                                reinsuranceCumulusRiskCoveragesResult = new List<ReinsuranceCumulusRiskCoverageDTO>();
                                reinsuranceLinesResult.Add(reinsuranceLine);
                            }

                        }

                        ReinsuranceLayerDTO reinsuranceLayer = new ReinsuranceLayerDTO();
                        reinsuranceLayer.ReinsuranceLayerId = Convert.ToInt32(layer[0]);
                        reinsuranceLayer.LayerNumber = Convert.ToInt32(layer[2]);
                        reinsuranceLayer.LayerPercentage = Convert.ToDecimal(layer[3]);
                        reinsuranceLayer.PremiumPercentage = Convert.ToDecimal(layer[4]);
                        reinsuranceLayer.ReinsuranceLines = new List<ReinsuranceLineDTO>();
                        reinsuranceLayer.ReinsuranceLines = reinsuranceLinesResult;
                        reinsuranceLayersResult.Add(reinsuranceLayer);
                    }
                    reinsuranceResult.ReinsuranceLayers = new List<ReinsuranceLayerDTO>();
                    reinsuranceResult.ReinsuranceLayers = reinsuranceLayersResult;

                }
                else
                {
                    reinsuranceResult.ReinsuranceId = -1;
                }

                reinsuranceResult.UserId = userId;
                return reinsuranceResult;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorManualIssueReinsurance);
            }
        }

        public ReinsuranceDTO ReinsureEndorsement(PolicyDTO policyDTO, int userId, bool onLine)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                List<LineDTO> lineDTOs = new List<LineDTO>();
                policyDTO = DelegateService.tempCommonIntegrationService.GetPolicyReinsuranceByPolicyIdEndorsementId(policyDTO.PolicyId, policyDTO.Endorsement.Id).ToDTO();
                policyDTO.Prefix = new PrefixDTO();
                policyDTO.Prefix.Id = DTOAssembler.CreatePolicyDTOByUNDDTOPolicyDTO(DelegateService.underwritingIntegrationService.GetPolicyByPolicyId(policyDTO.Id)).Prefix.Id;
                ValidatePriorityRetention(policyDTO);
                lineDTOs = GetLinesParametrization(policyDTO);
                return reinsuranceDAO.ReinsureEndorsement(userId, policyDTO, lineDTOs).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReinsureEndorsement);
            }
        }

        public int LoadFacultative(int processId, int? layerNumber, int? lineId, string cumulusKey, string description, decimal sumPercentage, decimal premiumPercentage, int userId)
        {
            try
            {
                TempFacultativeCompanyDAO tempFacultativeCompanyDAO = new TempFacultativeCompanyDAO();
                return tempFacultativeCompanyDAO.LoadFacultative(processId, layerNumber, lineId, cumulusKey, description, sumPercentage, premiumPercentage, userId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorLoadFacultative);
            }
        }

        public int LoadReinsuranceLayer(int endorsementId, int userId, int processType, DateTime dateFrom, DateTime dateTo, List<PrefixDTO> prefixesDTO)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.LoadReinsuranceLayer(endorsementId, userId, processType, dateFrom, dateTo, prefixesDTO.ToModels().ToList());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorLoadReinsuranceLayer);
            }
        }

        public List<ReinsuranceDistributionHeaderDTO> GetReinsuranceDistributionHeaders(int? endorsementId)
        {
            try
            {
                DistributionDAO distributionDAO = new DistributionDAO();
                return distributionDAO.GetReinsuranceDistributionHeaders(endorsementId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceDistributionHeaders);
            }
        }

        public List<ReinsuranceDTO> SaveIssueReinsurance(PolicyDTO policy, ReinsuranceDTO temporalReinsurance)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return DTOAssembler.ToDTOs(reinsuranceDAO.SaveIssueReinsurance(policy.ToModel(), temporalReinsurance.ToModel())).ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveIssueReinsurance);
            }
        }

        public List<EndorsementReinsuranceDTO> GetReinsuranceByEndorsement(int endorsementId)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.GetReinsuranceByEndorsement(endorsementId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceByEndorsement);
            }
        }

        public List<IssGetDistributionErrorsDTO> GetDistributionErrors(int endorsementId)
        {
            try
            {
                DistributionDAO distributionDAO = new DistributionDAO();
                return distributionDAO.GetDistributionErrors(endorsementId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetDistributionErrors);
            }
        }

        public bool DeleteReinsurance(decimal documentNumber, int endorsementNumber)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.DeleteReinsurance(documentNumber, endorsementNumber);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorDeleteReinsurance);
            }
        }

        #endregion

        #region Siniestros

        public List<ReinsuranceClaimLayerDTO> GetClaimLayerByClaimIdClaimModifyId(int claimId, int claimModifyId, int movementSourceId, int claimCoverageCd)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                List<ReinsuranceClaimLayerDTO> reinsuranceClaimLayerDTOs = new List<ReinsuranceClaimLayerDTO>();
                reinsuranceClaimLayerDTOs = claimDAO.GetClaimLayerByClaimIdClaimModifyId(claimId, claimModifyId, movementSourceId, claimCoverageCd).ToDTOs().ToList().OrderBy(x => x.ReinsuranceNumber).OrderBy(x => x.LayerNumber).ToList();
                return reinsuranceClaimLayerDTOs;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetClaimLayerByClaimId);
            }
        }

        public List<ReinsuranceClaimDistributionDTO> GetDistributionClaimByClaimLayerId(int claimLayerId, int movementSourceId)
        {
            try
            {
                List<ReinsuranceClaimDistributionDTO> reinsuranceClaimDistributionDTOs = new List<ReinsuranceClaimDistributionDTO>();
                ClaimDAO claimDAO = new ClaimDAO();
                return claimDAO.GetDistributionClaimByClaimLayerId(claimLayerId, movementSourceId).ToDTOs().ToList().OrderBy(x => x.Line)
                                                    .OrderBy(x => x.CumulusKey).OrderBy(x => x.Description).OrderBy(x => x.Contract).ToList();

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetDistributionClaimByClaimLayerId);
            }
        }

        public void ModificationReinsuranceClaim(int tempClaimReinsSourceId, decimal newAmount)
        {
            try
            {
                TempClaimReinsSourceDAO tempClaimReinsSourceDAO = new TempClaimReinsSourceDAO();
                tempClaimReinsSourceDAO.UpdateTempClaimReinsSource(tempClaimReinsSourceId, newAmount);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorModificationReinsuranceClaimLine);
            }
        }

        public ReinsurancePaymentClaimDTO GetReinsuranceClaim(int claimId)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                return claimDAO.GetReinsuranceClaim(claimId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceClaim);
            }
        }

        public ReinsuranceDTO LoadReinsuranceClaim(int? claimCode, int userId, int processType, DateTime dateFrom, DateTime dateTo, List<PrefixDTO> prefixesDTOs)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                return DTOAssembler.ToDTO(claimDAO.LoadReinsuranceClaim(claimCode, userId, processType, dateFrom, dateTo, prefixesDTOs.ToModels().ToList()));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorLoadReinsuranceClaim);
            }
        }

        public List<ClaimDTO> GetClaims(int branchCode, int prefixCode, decimal policyNumber, int? claimNumber)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                return claimDAO.GetClaims(branchCode, prefixCode, policyNumber, claimNumber).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetClaims);
            }
        }

        public int SaveClaimReinsurance(int processId, int claimId, int claimModifyId, int userId)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                return claimDAO.SaveClaimReinsurance(processId, claimId, claimModifyId, userId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveClaimReinsurance);
            }
        }

        public int ManualClaimReinsurance(int claimId, int claimModifyId, int userId)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                DataTable dataTable = new DataTable();
                bool succeed = Convert.ToBoolean(claimDAO.WortableLoadClaims(claimId, claimModifyId, userId, 0).Rows[0][1]);
                int processId = 0;
                if (succeed)
                {
                    dataTable = claimDAO.WortableLoadClaims(claimId, claimModifyId, userId, 1);
                    return processId = Convert.ToInt32(dataTable.Rows[0][1]);
                }
                else
                {
                    return processId;
                }

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorManualClaimReinsurance);
            }
        }

        public List<ClaimDistributionDTO> GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode(int claimCode, int claimModifyCode, int movementSourceId, int claimCoverageCd)
        {
            try
            {
                ClaimDAO claimDAO = new ClaimDAO();
                return claimDAO.GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode(claimCode, claimModifyCode, movementSourceId, claimCoverageCd).ToDTOs().ToList();

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceClaimDistribution);
            }
        }

        public List<ClaimAllocationDTO> GetClaimAllocationByMovementSource(int processId, int movementSourceId, int claimCoverageCd)
        {
            try
            {
                TempAllocationDAO tempAllocationDAO = new TempAllocationDAO();
                return tempAllocationDAO.GetClaimTempAllocations(processId, movementSourceId, claimCoverageCd).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetClaimAllocationByMovementSource);
            }
        }

        #endregion

        #region Pagos

        public int SavePaymentReinsurance(int processId, int paymentRequestId, int userId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.SavePaymentReinsurance(processId, paymentRequestId, userId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSavePaymentReinsurance);
            }
        }

        public ReinsuranceDTO LoadReinsurancePayment(int userId, DateTime dateFrom, DateTime dateTo, List<PrefixDTO> prefixesDTOs)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return DTOAssembler.ToDTO(levelPaymentDAO.LoadReinsurancePayment(userId, dateFrom, dateTo, prefixesDTOs.ToModels().ToList()));
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorLoadReinsurancePayment);
            }
        }

        public ReinsuranceDTO ReinsurancePayment(int paymentRequestId, int userId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                ReinsuranceDTO reinsuranceDTO = levelPaymentDAO.ReinsurancePayment(paymentRequestId, userId).ToDTO();

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

        public int ManualPaymentReinsurance(int paymentRequestId, int userId)
        {
            try
            {

                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                DataTable dataTable = new DataTable();
                bool succeed;
                int processId = 0;
                succeed = Convert.ToBoolean(levelPaymentDAO.CalculationReinsurancePayment(paymentRequestId, userId, 0).Rows[0][1]);

                if (succeed)
                {
                    dataTable = levelPaymentDAO.CalculationReinsurancePayment(paymentRequestId, userId, 1);
                    processId = Convert.ToInt32(dataTable.Rows[0][1]);
                }
                return processId;
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorManualPaymentReinsurance);
            }
        }

        public ReinsurancePaymentClaimDTO GetReinsurancePayment(int paymentRequestId, int userId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetReinsurancePayment(paymentRequestId, userId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsurancePayment);
            }
        }

        public void ModificationReinsurancePayment(int tmpPaymentReinsSourceId, decimal amount)
        {
            try
            {
                TempPaymentReinsSourceDAO tempPaymentReinsSourceDAO = new TempPaymentReinsSourceDAO();
                tempPaymentReinsSourceDAO.UpdateTempPaymentReinsSource(tmpPaymentReinsSourceId, amount);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorModificationReinsurancePaymentLine);
            }
        }

        public List<ReinsurancePaymentLayerDTO> GetPaymentLayerByPaymentRequestId(int paymentRequestId, int voucherConceptCd, int claimCoverageCd)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetPaymentLayerByPaymentRequestId(paymentRequestId, voucherConceptCd, claimCoverageCd).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentLayerByPaymentRequestId);
            }
        }

        public List<ReinsurancePaymentDistributionDTO> GetDistributionPaymentByPaymentLayerId(int paymentLayerId)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetDistributionPaymentByPaymentLayerId(paymentLayerId).ToDTOs().ToList()
                                                                                                      .OrderBy(x => x.Line)
                                                                                                      .OrderBy(x => x.CumulusKey)
                                                                                                      .OrderBy(x => x.Description)
                                                                                                      .OrderBy(x => x.Contract).ToList();

            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetDistributionPaymentByPaymentLayerId);
            }
        }

        public List<PaymentRequestDTO> GetPaymentsRequest(int branchCode, int prefixCode, int policyNumber, int claimNumber, int? paymentRequestNumber)
        {
            try
            {
                List<PaymentRequestDTO> reinsuranceClaimPaymentRequestDTOs = new List<PaymentRequestDTO>();
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetPaymentsRequest(branchCode, prefixCode, policyNumber, claimNumber, paymentRequestNumber).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentsRequest);
            }
        }

        public List<PaymentDistributionDTO> GetReinsurancePaymentDistributionsByPaymentRequestId(int paymentRequestId, int movementSourceId, int voucherConceptCd, int claimCoverageCd)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetReinsurancePaymentDistributionsByPaymentRequestId(paymentRequestId, voucherConceptCd, claimCoverageCd).ToDTOs().ToList().FindAll(x => x.MovementSourceId == movementSourceId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsurancePaymentDistribution);
            }
        }

        public List<PaymentAllocationDTO> GetPaymentAllocationByMovementSource(int processId, int movementSourceId, int voucherConceptCd, int claimCoverageCd)
        {
            try
            {
                LevelPaymentDAO levelPaymentDAO = new LevelPaymentDAO();
                return levelPaymentDAO.GetPaymentTempAllocations(processId, movementSourceId, voucherConceptCd, claimCoverageCd).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentAllocationByPaymentRequest);
            }
        }

        #endregion

        #region Contable

        public List<ReinsuranceAccountingParameterDTO> GetReinsuranceAccountingParameters(int processId)
        {
            try
            {
                ReinsuranceDAO reinsuranceDAO = new ReinsuranceDAO();
                return reinsuranceDAO.GetReinsuranceAccountingParameters(processId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetReinsuranceAccountingParameters);
            }
        }

        #endregion



    }
}