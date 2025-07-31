using Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Assemblers;
using Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.DAOs;
using Sistran.Company.Application.OperationQuotaServices.DTOs;
using AQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.BAF;
using System.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Co.Application.Data;
using System.Configuration;
using Sistran.Company.Application.OperationQuotaServices.EEProvider.Utilities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using CPEMCV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using COMM = Sistran.Core.Application.CommonService.Models;
using ENUM = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider
{
    public class OperationQuotaCompanyApplicationServiceEEProvider : IOperationQuotaCompanyService
    {

        private static int ruleIdIndicators = Convert.ToInt16(ConfigurationSettings.AppSettings["RuleIdIndicators"]);
        private static int ruleIdAqGeneral = Convert.ToInt16(ConfigurationSettings.AppSettings["RuleIdAutomaticQuota"]);
        public List<AgentProgramDTO> GetAgentProgramDTOs()
        {
            try
            {
                List<AgentProgramDTO> agentProgramDTOs = new List<AgentProgramDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return agentProgramDTOs = DTOAssembler.CreateAgentPrograms(operationQuotaDAO.GetAgentProgram());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UtilityDetailsDTO> GetUtilityDTOs()
        {
            try
            {
                List<UtilityDetailsDTO> utilityDetailsDTOs = new List<UtilityDetailsDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return utilityDetailsDTOs = DTOAssembler.CreateUtilitiesDetails(operationQuotaDAO.GetUtility());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IndicatorConceptDTO> GetIndicatorConceptsDTOs()
        {
            try
            {
                List<IndicatorConceptDTO> indicatorConceptDTOs = new List<IndicatorConceptDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return indicatorConceptDTOs = DTOAssembler.CreateindicatorConcepts(operationQuotaDAO.GetIndicatorConcepts());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ReportListSisconcDTO> GetReportListSisconcDTOs()
        {
            try
            {
                List<ReportListSisconcDTO> reportListSisconcDTOs = new List<ReportListSisconcDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return reportListSisconcDTOs = DTOAssembler.CreateReportListSisconc(operationQuotaDAO.GetReportListSisconc());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<RiskCenterDTO> GetRiskCenterListDTOs()
        {
            try
            {
                List<RiskCenterDTO> riskCenterDTOs = new List<RiskCenterDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return riskCenterDTOs = DTOAssembler.CreateRiskCenterList(operationQuotaDAO.GetRiskCenterList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<RestrictiveDTO> GetRestrictiveListDTOs()
        {
            try
            {
                List<RestrictiveDTO> restrictiveDTOs = new List<RestrictiveDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return restrictiveDTOs = DTOAssembler.CreateRestrictiveList(operationQuotaDAO.GetRestrictiveList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<PromissoryNoteSignatureDTO> GetPromissoryNoteSignatureDTOs()
        {
            try
            {
                List<PromissoryNoteSignatureDTO> promissoryNoteSignatureDTOs = new List<PromissoryNoteSignatureDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return promissoryNoteSignatureDTOs = DTOAssembler.CreatePromissoryNoteSignatures(operationQuotaDAO.GetPromissoryNoteSignature());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AutomaticQuotaOperationDTO> GetAutomaticQuotaOperation(int Id)
        {
            try
            {
                List<AutomaticQuotaOperationDTO> automaticQuotaOperationDTOs = new List<AutomaticQuotaOperationDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return automaticQuotaOperationDTOs = DTOAssembler.CreateAutomaticQuotaOperations(operationQuotaDAO.GetAutomaticQuotaOperation(Id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AutomaticQuotaOperationDTO> GetAutomaticQuotaOperationByParentId(int ParentId)
        {
            try
            {
                List<AutomaticQuotaOperationDTO> automaticQuotaOperationDTOs = new List<AutomaticQuotaOperationDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return automaticQuotaOperationDTOs = DTOAssembler.CreateAutomaticQuotaOperations(operationQuotaDAO.GetAutomaticQuotaOperationByParentId(ParentId));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AutomaticQuotaDTO> GetAutomaticQuota(int Id)
        {
            try
            {
                List<AutomaticQuotaDTO> automaticQuotaDTOs = new List<AutomaticQuotaDTO>();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return automaticQuotaDTOs = DTOAssembler.CreateAutomaticQuotas(operationQuotaDAO.GetAutomaticQuota(Id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Politicas

        public AutomaticQuotaDTO SaveAutomaticQuotaGeneral(AutomaticQuotaDTO automaticQuotaDto, bool validatePolicies = true)
        {
            try
            {
                List<PoliciesAut> infringementPoliciesGeneral = new List<PoliciesAut>();
                List<PoliciesAut> infringementPoliciesThird = new List<PoliciesAut>();
                List<PoliciesAut> infringementPoliciesBusiness = new List<PoliciesAut>();
                List<PoliciesAut> listPolicies = new List<PoliciesAut>();
                AutomaticQuotaDTO automaticQuotaDTO = new AutomaticQuotaDTO();
                //Deserializa JSON
                List<AutomaticQuotaOperationDTO> automaticQuotaOperation = DelegateService.operationQuotaCompanyService.GetAutomaticQuotaOperation(automaticQuotaDto.AutomaticQuotaId);
                AQMOD.AutomaticQuota automaticQuota = JsonConvert.DeserializeObject<AQMOD.AutomaticQuota>(automaticQuotaOperation.FirstOrDefault().Operation);

                bool withPolicies = false;
                if (validatePolicies)
                {
                    automaticQuota.LegalizedQuota = automaticQuotaDto.LegalizedQuota;
                    automaticQuota.QuotaReconsideration = automaticQuotaDto.QuotaReConsideration;

                    infringementPoliciesGeneral.AddRange(ValidateAuthorizationPoliciesGeneral(automaticQuota));
                    infringementPoliciesThird.AddRange(ValidateAuthorizationPoliciesThird(automaticQuota.Third));
                    infringementPoliciesBusiness.AddRange(ValidateAuthorizationPoliciesUtility(automaticQuota.Utility));

                    automaticQuota.InfringementPolicies = infringementPoliciesGeneral;
                    automaticQuota.Third.InfringementPolicies = infringementPoliciesThird;
                    automaticQuota.Utility.FirstOrDefault().InfringementPolicies = infringementPoliciesBusiness;

                    if (automaticQuota.InfringementPolicies.Any() || automaticQuota.Third.InfringementPolicies.Any() || automaticQuota.Utility?.FirstOrDefault().InfringementPolicies?.Any() == true)
                    {
                        if (automaticQuota.InfringementPolicies.Any(x => x.Type == ENUM.TypePolicies.Restrictive)
                            || automaticQuota.Third.InfringementPolicies.Any(x => x.Type == ENUM.TypePolicies.Restrictive)
                            || automaticQuota.Utility.FirstOrDefault().InfringementPolicies.Any(z => z.Type == ENUM.TypePolicies.Restrictive))
                        {
                            withPolicies = false;
                            validatePolicies = false;
                            automaticQuota.Utility = null;
                            automaticQuota.Indicator = null;
                        }
                        else
                        {
                            withPolicies = true;
                            automaticQuota.InfringementPolicies.ForEach(x => { listPolicies.Add(x); });
                            automaticQuota.Third.InfringementPolicies.ForEach(y => { listPolicies.Add(y); });
                            automaticQuota.Utility.FirstOrDefault().InfringementPolicies.ForEach(z => { listPolicies.Add(z); });
                            automaticQuota.InfringementPolicies = listPolicies;
                            //Al ejecutar politicas se debe tomar siempre la reconsideración de cupo.
                            automaticQuota.SuggestedQuota = automaticQuota.QuotaReconsideration;
                            //Se actualiza el json para recuperacion del wrapper.
                            automaticQuota.AutomaticQuotaId = this.UpdateCompanyAutomaticQuotaOperation(
                               new AutomaticQuotaOperationDTO
                               {
                                   Id = automaticQuota.AutomaticQuotaId,
                                   ParentId = 0,
                                   AutomaticOperationType = (int)EnumAutomaticOperationType.General,
                                   CreationDate = DateTime.Now,
                                   ModificationDate = DateTime.Now,
                                   User = automaticQuota.RequestedById,
                                   Operation = JsonConvert.SerializeObject(automaticQuota),
                               }
                             ).Id;

                        }

                        automaticQuotaDTO = DTOAssembler.CreateautomaticQuota(automaticQuota);
                    }
                }
                if (withPolicies == false || validatePolicies == false)
                {
                    automaticQuota = CreateAutomaticTemporal(automaticQuota);
                    //Insercion en Tablas Event
                    if (automaticQuota.CustomerTypeId == (int)CustomerType.Individual)
                    {
                        COMM.Parameter paramDate = new COMM.Parameter();
                        paramDate = DelegateService.commonService.GetParameterByDescription("CURRENT_DATE_OPERATION_QUOTA");
                        List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                        OperatingQuotaEventDTO operatingQuotaEventDTO = new OperatingQuotaEventDTO();
                        operatingQuotaEventDTO.IndividualOperatingQuota = new IndividualOperatingQuotaDTO();
                        operatingQuotaEventDTO.IdentificationId = automaticQuota.IndividualId;
                        operatingQuotaEventDTO.OperatingQuotaEventType = (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA;
                        operatingQuotaEventDTO.IssueDate = DateTime.Now;
                        operatingQuotaEventDTO.LineBusinessID = 30;
                        operatingQuotaEventDTO.SubLineBusinessID = 0;
                        operatingQuotaEventDTO.IndividualOperatingQuota.IndividualID = automaticQuota.IndividualId;
                        operatingQuotaEventDTO.IndividualOperatingQuota.InitDateOpQuota = DateTime.Now;
                        operatingQuotaEventDTO.IndividualOperatingQuota.EndDateOpQuota = (DateTime)paramDate.DateParameter;
                        operatingQuotaEventDTO.IndividualOperatingQuota.LineBusinessID = 30;
                        operatingQuotaEventDTO.IndividualOperatingQuota.ParticipationPercentage = 100;
                        if (automaticQuota.QuotaReconsideration > automaticQuota.SuggestedQuota)
                        {
                            operatingQuotaEventDTO.IndividualOperatingQuota.ValueOpQuotaAMT = automaticQuota.QuotaReconsideration;
                        }
                        else if (automaticQuota.SuggestedQuota >= automaticQuota.QuotaReconsideration)
                        {
                            operatingQuotaEventDTO.IndividualOperatingQuota.ValueOpQuotaAMT = automaticQuota.SuggestedQuota;
                        }
                        else if (automaticQuota.SuggestedQuota == 0 && automaticQuota.QuotaReconsideration == 0)
                        {
                            operatingQuotaEventDTO.IndividualOperatingQuota.ValueOpQuotaAMT = automaticQuota.LegalizedQuota;
                        }
                        operatingQuotaEventDTOs.Add(operatingQuotaEventDTO);
                        DelegateService.operationQuotaService.InsertOperatingQuotaEvent(operatingQuotaEventDTOs);
                        automaticQuotaDTO = DTOAssembler.CreateautomaticQuota(automaticQuota);
                        DeleteAutomaticOperation(automaticQuota.AutomaticQuotaId);
                        DeleteAutomaticOperationsByParentId(automaticQuota.AutomaticQuotaId);

                    }
                    else if (automaticQuotaDTO.CustomerTpeId == (int)CustomerType.Prospect)
                    {
                        SaveProspect(automaticQuotaDTO);
                        DeleteAutomaticOperation(automaticQuota.AutomaticQuotaId);
                        DeleteAutomaticOperationsByParentId(automaticQuota.AutomaticQuotaId);
                    }

                }

                return automaticQuotaDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //guardado general

        public AutomaticQuotaDTO SaveAutomaticQuotaGeneralJSON(AutomaticQuotaDTO automaticQuotaDTO, bool validatePolicies = true)
        {
            try
            {

                AQMOD.AutomaticQuota automatic = ModelAssembler.CreateModelAutomaticQuota(automaticQuotaDTO);
                //Implementacion politicas

                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies.AddRange(ValidateAuthorizationPoliciesGeneral(automatic));
                }
                automatic.InfringementPolicies = infringementPolicies;
                automatic.AutomaticQuotaId = this.CreateCompanyAutomaticQuotaOperation(
                            new AutomaticQuotaOperationDTO
                            {
                                ParentId = 0,
                                AutomaticOperationType = (int)EnumAutomaticOperationType.General,
                                CreationDate = DateTime.Now,
                                ModificationDate = DateTime.Now,
                                User = automatic.RequestedById,
                                Operation = JsonConvert.SerializeObject(automatic),
                            }
                    ).Id;

                automaticQuotaDTO.AutomaticQuotaId = automatic.AutomaticQuotaId;
                return automaticQuotaDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //guardado terceros
        public ThirdDTO SaveAutomaticQuotaThirdJSON(ThirdDTO thirdDTO, bool validatePolicies = true)
        {
            try
            {
                AQMOD.Third third = new AQMOD.Third();
                third = ModelAssembler.CreateModelThird(thirdDTO);
                //Implementacion politicas
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies.AddRange(ValidateAuthorizationPoliciesThird(third));
                }
                third.InfringementPolicies = infringementPolicies;
                thirdDTO.InfringementPolicies = infringementPolicies;
                third.Id = this.CreateCompanyAutomaticQuotaOperation(
                                new AutomaticQuotaOperationDTO
                                {
                                    ParentId = thirdDTO.Id,
                                    AutomaticOperationType = (int)EnumAutomaticOperationType.Terceros,
                                    CreationDate = DateTime.Now,
                                    ModificationDate = DateTime.Now,
                                    User = 10,
                                    Operation = JsonConvert.SerializeObject(third),
                                }
                        ).Id;

                return thirdDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        //guardado utilities

        public AutomaticQuotaDTO SaveAutomaticQuotaUtilityJSON(List<UtilityDTO> utilityDTO, AutomaticQuotaDTO automaticDTO, bool validatePolicies = true)
        {
            try
            {
                List<AQMOD.Utility> utility = new List<AQMOD.Utility>();
                AQMOD.AutomaticQuota automaticQuota = new AQMOD.AutomaticQuota();
                AQMOD.AutomaticQuota automaticResult = new AQMOD.AutomaticQuota();
                automaticQuota = ModelAssembler.CreateModelAutomaticQuota(automaticDTO);
                utility = ModelAssembler.CreateListUtility(utilityDTO);
                //Implementacion politicas
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies.AddRange(ValidateAuthorizationPoliciesUtility(utility));
                }
                utility.FirstOrDefault().InfringementPolicies = infringementPolicies;
                //implementacion reglas
                automaticResult = RunRulesUtilities(utility, automaticQuota, ruleIdIndicators);
                automaticResult.AutomaticQuotaId = this.CreateCompanyAutomaticQuotaOperation(
                                new AutomaticQuotaOperationDTO
                                {
                                    ParentId = automaticQuota.AutomaticQuotaId,
                                    AutomaticOperationType = (int)EnumAutomaticOperationType.Utilidades,
                                    CreationDate = DateTime.Now,
                                    ModificationDate = DateTime.Now,
                                    User = automaticQuota.RequestedById,
                                    Operation = JsonConvert.SerializeObject(automaticResult.Utility),
                                }
                        ).Id;
                automaticDTO = DTOAssembler.CreateautomaticQuota(automaticResult);
                return automaticDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        public AutomaticQuotaOperationDTO CreateCompanyAutomaticQuotaOperation(AutomaticQuotaOperationDTO automaticQuotaOperationDTO)
        {
            AQMOD.AutomaticQuotaOperation companyAutomaticOperation = this.CreateCompanyAutomaticOperation(ModelAssembler.CreateModelAutomaticOperation(automaticQuotaOperationDTO));

            if (companyAutomaticOperation != null)
            {
                automaticQuotaOperationDTO = DTOAssembler.CreateAutomaticQuotaOperation(companyAutomaticOperation);
            }

            return automaticQuotaOperationDTO;

        }

        public AutomaticQuotaOperationDTO UpdateCompanyAutomaticQuotaOperation(AutomaticQuotaOperationDTO automaticQuotaOperationDTO)
        {
            AQMOD.AutomaticQuotaOperation companyAutomaticOperation = this.UpdateCompanyAutomaticOperation(ModelAssembler.CreateModelAutomaticOperation(automaticQuotaOperationDTO));

            if (companyAutomaticOperation != null)
            {
                automaticQuotaOperationDTO = DTOAssembler.CreateAutomaticQuotaOperation(companyAutomaticOperation);
            }

            return automaticQuotaOperationDTO;

        }

        public AQMOD.AutomaticQuotaOperation CreateCompanyAutomaticOperation(AQMOD.AutomaticQuotaOperation companyAutomaticOperation)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return operationQuotaDAO.CreateCompanyAutomaticOperation(companyAutomaticOperation);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public AQMOD.AutomaticQuotaOperation UpdateCompanyAutomaticOperation(AQMOD.AutomaticQuotaOperation companyAutomaticOperation)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return operationQuotaDAO.UpdateCompanyAutomaticOperation(companyAutomaticOperation);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //ejeutar politicas general

        public List<PoliciesAut> ValidateAuthorizationPoliciesGeneral(AQMOD.AutomaticQuota automatic)
        {
            try
            {
                int package = 22;
                List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
                Facade facade = new Facade();
                EntityAssembler.CreateFacadeGeneral(facade, automatic);
                policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "1", facade, FacadeType.RULE_FACADE_GENERAL_AUTOMATIC_QUOTA));

                return policiesAuts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        //ejecutar politicas tercero
        public List<PoliciesAut> ValidateAuthorizationPoliciesThird(AQMOD.Third third)
        {
            try
            {
                int package = 22;
                List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
                Facade facade = new Facade();
                if (third != null)
                {
                    EntityAssembler.CreateFacadeThird(facade, third);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "1", facade, FacadeType.RULE_FACADE_THIRD_AUTOMATIC_QUOTA));
                }
                return policiesAuts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesUtility(List<AQMOD.Utility> utility)
        {
            try
            {
                int package = 22;
                List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
                Facade facade = new Facade();

                if (utility.Count > 0)
                {
                    EntityAssembler.CreateFacadeBusiness(facade, utility);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "1", facade, FacadeType.RULE_FACADE_BUSINESS_AUTOMATIC_QUOTA));
                }
                return policiesAuts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Politicas

        public AQMOD.AutomaticQuota CreateAutomaticQuotaGeneral(AQMOD.AutomaticQuota automatic)
        {
            try
            {
                OperationQuotaDAO operationDAO = new OperationQuotaDAO();
                return operationDAO.CreateAutomaticQuotaGeneral(automatic);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public AutomaticQuotaDTO ExecuteCalculate(int id, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                AutomaticQuotaDTO automaticDto = new AutomaticQuotaDTO();
                automaticDto = RunRulesGeneral(id, dynamicProperties);
                return automaticDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        //ejecutar reglas

        public AutomaticQuotaDTO RunRulesGeneral(int temp, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                List<AutomaticQuotaOperationDTO> getAutomatic = GetAutomaticQuotaOperation(temp);
                AQMOD.AutomaticQuota automaticQuota = JsonConvert.DeserializeObject<AQMOD.AutomaticQuota>(getAutomatic.FirstOrDefault().Operation);

                Facade facade = new Facade();
                if (automaticQuota != null)
                {
                    automaticQuota.DynamicProperties = dynamicProperties;
                    EntityAssembler.CreateFacadeGeneral(facade, automaticQuota);
                    if (automaticQuota.Third != null)
                    {
                        EntityAssembler.CreateFacadeThird(facade, automaticQuota.Third);
                    }
                    if (automaticQuota.Utility != null)
                    {
                        EntityAssembler.CreateFacadeBusiness(facade, automaticQuota.Utility);
                    }
                    if (ruleIdAqGeneral > 0)
                    {
                        facade = RulesEngineDelegate.ExecuteRules(ruleIdAqGeneral, facade);
                    }
                    else
                    {
                        throw new Exception("Regla no encotrada " + ruleIdAqGeneral);
                    }

                }
                automaticQuota = ModelAssembler.CreateModelRulesGeneral(automaticQuota, facade);
                AutomaticQuotaDTO dtoAutomatic = DTOAssembler.CreateautomaticQuota(automaticQuota);
                return dtoAutomatic;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public AQMOD.AutomaticQuota RunRulesUtilities(List<AQMOD.Utility> utilities, AQMOD.AutomaticQuota automatic, int idRule)
        {
            try
            {

                Facade facade = new Facade();
                EntityAssembler.CreateFacadeBusiness(facade, utilities);
                EntityAssembler.CreateFacadeGeneral(facade, automatic);
                if (idRule > 0)
                {
                    facade = RulesEngineDelegate.ExecuteRules(idRule, facade);
                }
                else
                {
                    throw new BusinessException("Regla no encontrada " + idRule);
                }
                return ModelAssembler.CreateModelRulesUtility(utilities, automatic, facade);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public AQMOD.AutomaticQuota CreateAutomaticTemporal(AQMOD.AutomaticQuota automaticQuota)
        {
            try
            {
                OperationQuotaDAO automaticQuotaDAO = new OperationQuotaDAO();
                //automaticQuotaDAO.(automaticQuotaDTO);
                if (automaticQuota.Prospect != null)
                {
                    automaticQuota = automaticQuotaDAO.SaveQuotaTables(automaticQuota);
                }
                return automaticQuota;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Insertar en tablas
        /// </summary>
        public AQMOD.AutomaticQuota SaveQuotaTables(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            GetDatatables dts = new GetDatatables();
            CommonDataTables datatables = dts.GetcommonDataTables(automaticQuota);
            NameValue[] parameters = new NameValue[3];
            return automaticQuota;
        }

        //actualizacion de perifericos json

        public AutomaticQuotaDTO UpdateAutomaticQuotaGeneralJSON(AutomaticQuotaDTO automaticQuotaDTO, bool validatePolicies = true)
        {
            try
            {

                AQMOD.AutomaticQuota automatic = ModelAssembler.CreateModelAutomaticQuota(automaticQuotaDTO);
                List<AQMOD.Indicator> indicator = new List<AQMOD.Indicator>();

                //Implementacion politicas
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies.AddRange(ValidateAuthorizationPoliciesGeneral(automatic));
                }
                automatic.InfringementPolicies = infringementPolicies;
                automatic.AutomaticQuotaId = this.UpdateCompanyAutomaticQuotaOperation(
                            new AutomaticQuotaOperationDTO
                            {
                                Id = automatic.AutomaticQuotaId,
                                ParentId = 0,
                                AutomaticOperationType = (int)EnumAutomaticOperationType.General,
                                CreationDate = DateTime.Now,
                                ModificationDate = DateTime.Now,
                                User = automatic.RequestedById,
                                Operation = JsonConvert.SerializeObject(automatic),
                            }
                    ).Id;

                automaticQuotaDTO.AutomaticQuotaId = automatic.AutomaticQuotaId;
                return automaticQuotaDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ThirdDTO UpdateAutomaticQuotaThirdJSON(ThirdDTO thirdDTO, int id, bool validatePolicies = true)
        {
            try
            {
                AQMOD.Third third = new AQMOD.Third();
                third = ModelAssembler.CreateModelThird(thirdDTO);
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies.AddRange(ValidateAuthorizationPoliciesThird(third));
                }
                third.InfringementPolicies = infringementPolicies;
                thirdDTO.InfringementPolicies = infringementPolicies;
                third.Id = this.UpdateCompanyAutomaticQuotaOperation(
                                new AutomaticQuotaOperationDTO
                                {
                                    Id = id,
                                    ParentId = thirdDTO.Id,
                                    AutomaticOperationType = (int)EnumAutomaticOperationType.Terceros,
                                    CreationDate = DateTime.Now,
                                    ModificationDate = DateTime.Now,
                                    User = 10,
                                    Operation = JsonConvert.SerializeObject(third),
                                }
                        ).Id;

                return thirdDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public AutomaticQuotaDTO UpdateAutomaticQuotaUtilityJSON(List<UtilityDTO> utilityDTO, AutomaticQuotaDTO automaticDTO, int id, bool validatePolicies = true)
        {
            try
            {
                List<AQMOD.Utility> utility = new List<AQMOD.Utility>();
                AQMOD.AutomaticQuota automaticQuota = new AQMOD.AutomaticQuota();
                AQMOD.AutomaticQuota automaticResult = new AQMOD.AutomaticQuota();
                automaticQuota = ModelAssembler.CreateModelAutomaticQuota(automaticDTO);
                utility = ModelAssembler.CreateListUtility(utilityDTO);
                //Implementacion politicas
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies.AddRange(ValidateAuthorizationPoliciesUtility(utility));
                }
                utility.FirstOrDefault().InfringementPolicies = infringementPolicies;
                //implementacion reglas
                automaticResult = RunRulesUtilities(utility, automaticQuota, ruleIdIndicators);
                automaticResult.AutomaticQuotaId = this.UpdateCompanyAutomaticQuotaOperation(
                                new AutomaticQuotaOperationDTO
                                {
                                    Id = id,
                                    ParentId = automaticDTO.AutomaticQuotaId,
                                    AutomaticOperationType = (int)EnumAutomaticOperationType.Utilidades,
                                    CreationDate = DateTime.Now,
                                    ModificationDate = DateTime.Now,
                                    User = automaticDTO.RequestedById,
                                    Operation = JsonConvert.SerializeObject(automaticResult.Utility),
                                }
                        ).Id;
                automaticDTO = DTOAssembler.CreateautomaticQuota(automaticResult);
                return automaticDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public AutomaticQuotaDTO GetAutomaticQuotaDeserealizado(int Id)
        {
            try
            {
                AutomaticQuotaDTO automaticQuotaDTO = new AutomaticQuotaDTO();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                return automaticQuotaDTO = DTOAssembler.CreateautomaticQuota(operationQuotaDAO.GetAutomaticQuotaDeserealizado(Id));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public bool SaveProspect(AutomaticQuotaDTO automaticQuotaDTO)
        {

            try
            {
                CompanyProspectNatural companyProspectNatural = new CompanyProspectNatural();
                companyProspectNatural.Street = automaticQuotaDTO.ProspecDTO.Address;
                companyProspectNatural.CityCode = automaticQuotaDTO.ProspecDTO.City;
                companyProspectNatural.CountryCode = automaticQuotaDTO.ProspecDTO.CountryCd;
                companyProspectNatural.StateCode = automaticQuotaDTO.ProspecDTO.StateCd;
                companyProspectNatural.EmailAddress = automaticQuotaDTO.ProspecDTO.Email;
                companyProspectNatural.IdCardTypeCode = automaticQuotaDTO.ProspecDTO.DocumentType;
                companyProspectNatural.IdCardNo = automaticQuotaDTO.ProspecDTO.DocumentNumber;
                companyProspectNatural.MotherLastName = automaticQuotaDTO.ProspecDTO.BusinessName;
                companyProspectNatural.PhoneNumber = automaticQuotaDTO.ProspecDTO.Phone;
                companyProspectNatural.IndividualTyepCode = 1;

                CPEMCV1.CompanyProspectNatural prospectPersonNatural = DelegateService.uniquePersonServiceV1.CreateProspectPersonNatural(companyProspectNatural);
                if (prospectPersonNatural == null)
                {
                    prospectPersonNatural = DelegateService.uniquePersonServiceV1.GetProspectByDocumentNumber(automaticQuotaDTO.ProspecDTO.DocumentNumber, 3);
                    companyProspectNatural.ProspectCode = prospectPersonNatural.ProspectCode;
                    prospectPersonNatural = DelegateService.uniquePersonServiceV1.UpdateProspectPersonNatural(companyProspectNatural);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        public bool DeleteAutomaticOperation(int id)
        {
            try
            {
                OperationQuotaDAO automaticOperationDAO = new OperationQuotaDAO();
                return automaticOperationDAO.DeleteAutomaticQuotaOperation(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString());
            }
        }

        /// <summary>
        /// Eliminar Hijos de un JSON
        /// </summary>
        /// <param name="parentId">Id Padre</param>
        /// <returns>Eliminados Si/No</returns>
        public bool DeleteAutomaticOperationsByParentId(int parentId)
        {
            try
            {
                OperationQuotaDAO automaticOperationsDAO = new OperationQuotaDAO();
                return automaticOperationsDAO.DeleteAutomaticOperationsByParentId(parentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString());
            }
        }


    }
}