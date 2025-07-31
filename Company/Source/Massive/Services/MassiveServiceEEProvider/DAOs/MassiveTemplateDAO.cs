using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using Sistran.Company.Application.MassiveServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.Massive.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.Utilities.Enums;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class MassiveTemplateDAO
    {

        #region Validaciones plantillas Automoviles 

        decimal percentage;
        decimal percentageOwn;
        decimal percentageExpenses;
        string templateName = "";
        /// <summary>
        /// Crea los accesorios 
        /// </summary>
        /// <param name="templateScripts"></param>
        /// <param name="policy"></param>
        /// <param name="companyVehicle"></param>
        /// <param name="coverIdAccesoryNoORig"></param>
        /// <param name="coverIdAccesoryORig"></param>
        /// <param name="error"></param>
        /// <returns>Lista Accesorios</returns>
        public List<CompanyAccessory> GetAccesorysByTemplate(Template templateScripts, CompanyPolicy policy, CompanyVehicle companyVehicle, int coverIdAccesoryNoORig, int coverIdAccesoryORig, ref string error)
        {
            List<CompanyAccessory> companyAccessories = new List<CompanyAccessory>();
            string currentError = string.Empty;
            try
            {
                if (templateScripts != null)
                {
                    foreach (Row row in templateScripts.Rows)
                    {
                        CompanyAccessory accessory = new CompanyAccessory();

                        accessory.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId));
                        accessory.IsOriginal = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesIsOriginal));
                        if (!accessory.IsOriginal)
                        {
                            accessory.Amount = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesPrice));
                        }
                        companyAccessories.Add(accessory);
                    }

                    foreach (CompanyAccessory itemAccessory in companyAccessories)
                    {

                        if (itemAccessory.Id == 0)
                        {
                            currentError += string.Format(Resources.Errors.ValidateErrorAcessory) + KeySettings.ReportErrorSeparatorMessage();
                        }
                        else if (!itemAccessory.IsOriginal && itemAccessory.Amount == 0)
                        {
                            Row rowAccessory = templateScripts.Rows.FirstOrDefault(r => r.HasError && r.ErrorDescription == Errors.ErrorAccessoryAmount
                            && r.Fields.FirstOrDefault(f => f.PropertyName == FieldPropertyName.AccesoriesAccessoryId
                            && f.Value == itemAccessory.Id.ToString()) != null);
                            currentError += string.Format(Errors.ValidateAccesoryAmmountRequired, (rowAccessory != null ? rowAccessory.Number.ToString() : "")) + KeySettings.ReportErrorSeparatorMessage();
                        }
                        else
                        {
                            if (companyAccessories != null && companyAccessories.Any())
                            {
                                List<CompanyCoverage> coveragesAccessories = DelegateService.underwritingService.
                                    GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id,
                                    policy.Prefix.Id);
                                if (coveragesAccessories != null && companyVehicle?.Risk?.Coverages != null)
                                {
                                    if (coverIdAccesoryNoORig != 0 && coverIdAccesoryORig != 0 && companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == coverIdAccesoryNoORig || x.Id == coverIdAccesoryORig) == null)
                                    {
                                        currentError += string.Format(Resources.Errors.ErrorCoverAccesory) + " " + companyVehicle.Risk.GroupCoverage.Id + KeySettings.ReportErrorSeparatorMessage();
                                    }

                                    var coverageDuplicate = companyVehicle.Risk.Coverages.Where(x => coveragesAccessories.Select(y => y.Id).Contains(x.Id)).ToList();
                                    var coverageAdd = coveragesAccessories.Where(x => !coverageDuplicate.Select(p => p.Id).Contains(x.Id)).ToList();
                                    if (coverageAdd != null && coverageAdd.Any())
                                    {
                                        companyVehicle.Risk.Coverages.AddRange(coverageAdd);
                                    }
                                }
                                companyVehicle.PriceAccesories = companyAccessories.Sum(u => u.Amount);
                            }

                        }


                        if (!string.IsNullOrEmpty(currentError))
                        {
                            error += currentError + KeySettings.ReportErrorSeparatorMessage();
                        }
                        currentError = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                error += Resources.Errors.ErrorGetAccesorysByTemplate + KeySettings.ReportErrorSeparatorMessage();
            }
            return companyAccessories;
        }
        /// <summary>
        /// Crea Clausulas
        /// </summary>
        /// <param name="template"></param>
        /// <param name="companyVehicle"></param>
        /// <param name="riskClauses"></param>
        /// <param name="coverageClauses"></param>
        /// <param name="error"></param>
        public void GetClauseByTemplate(Template template, ref List<CompanyClause> companyClauses, ref List<CompanyCoverage> companyCoverages, List<CompanyClause> riskClauses, List<CompanyClause> coverageClauses, ref string error)
        {
            string currentError = string.Empty;
            string templateName = "";
            templateName = template.Description;
            companyClauses = new List<CompanyClause>();
            companyCoverages.ForEach(c => c.Clauses = new List<CompanyClause>());
            List<ConditionLevel> conditionLevels = DelegateService.underwritingService.GetConditionLevels();

            foreach (Row clausesRow in template.Rows)
            {
                int levelCode = (int)DelegateService.utilitiesService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.LevelCode));
                int clauseCode = (int)DelegateService.utilitiesService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.ClauseCode));

                if (levelCode == 0)
                {
                    currentError += string.Format(Resources.Errors.ClauseErrorLevel) + KeySettings.ReportErrorSeparatorMessage();
                }

                if (clauseCode == 0)
                {
                    currentError += string.Format(Resources.Errors.ClauseError) + KeySettings.ReportErrorSeparatorMessage();
                }

                ConditionLevel conditionLevel = conditionLevels.FirstOrDefault(c => c.Id == levelCode);

                if (conditionLevel != null)
                {

                    if (conditionLevel.EmissionLevel == EmissionLevel.Risk)
                    {
                        if (riskClauses.Any(c => c.Id == clauseCode))
                        {
                            companyClauses.Add(riskClauses.First(c => c.Id == clauseCode));
                        }
                        else
                        {
                            currentError += string.Format(Resources.Errors.ErrorClauseNotFound) + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    else if (conditionLevel.EmissionLevel == EmissionLevel.Coverage)
                    {
                        if (coverageClauses.Any(a => a.Id == clauseCode))
                        {
                            CompanyClause clauseToAdd = coverageClauses.First(c => c.Id == clauseCode);

                            if (companyCoverages.Any(c => c.Id == clauseToAdd.ConditionLevel.ConditionValue))
                            {
                                companyCoverages.First(c => c.Id == clauseToAdd.ConditionLevel.ConditionValue).Clauses.Add(clauseToAdd);
                            }
                            else
                            {
                                currentError += string.Format(Resources.Errors.ClauseCoverageNotPresentOnRisk, clauseCode) + KeySettings.ReportErrorSeparatorMessage();
                            }

                        }
                        else
                        {
                            currentError += string.Format(Resources.Errors.ClauseNotRelatedToCoverage, clauseCode) + KeySettings.ReportErrorSeparatorMessage();

                        }
                    }
                }

                if (!string.IsNullOrEmpty(currentError))
                {
                    error += currentError + KeySettings.ReportErrorSeparatorMessage();
                }

            }
        }

        /// <summary>
        /// Crear coverturas adicionales
        /// </summary>
        /// <param name="Allcoverages"></param>
        /// <param name="Actualcoverages"></param>
        /// <param name="rows"></param>
        /// <returns>Lista CompanyCoverage</returns>
        public List<CompanyCoverage> CreateAdditionalCoverages(List<CompanyCoverage> Allcoverages, List<CompanyCoverage> Actualcoverages, List<Row> rows)
        {
            List<int> coverageIds = new List<int>();
            foreach (Row row in rows)
            {
                int coverageId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));

                if (!Actualcoverages.Exists(c => c.Id == coverageId))
                {
                    if (Allcoverages.Exists(c => c.Id == coverageId))
                    {
                        coverageIds.Add(coverageId);
                    }
                    else
                    {
                        row.HasError = true;
                        row.ErrorDescription = string.Format(Errors.CoverageNotParameterized, coverageId);
                    }
                }
                else
                {
                    row.HasError = true;
                    row.ErrorDescription = string.Format(Errors.CoverageExists, coverageId);
                }
            }
            Actualcoverages.AddRange(Allcoverages.Where(t => coverageIds.Contains(t.Id)));

            return Actualcoverages;
        }

        /// <summary>
        /// Crear deducibles 
        /// </summary>
        /// <param name="coverages"></param>
        /// <param name="template"></param>
        /// <returns>Lista CompanyCoverage</returns>
        public List<CompanyCoverage> CreateDeductibles(List<CompanyCoverage> coverages, Template template)
        {
            if (template != null)
            {
                foreach (Row row in template.Rows)
                {
                    int coverageId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    int deductibleId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                    if (coverages.Exists(c => c.Id == coverageId))
                    {
                        coverages.First(c => c.Id == coverageId).Deductible = new CompanyDeductible
                        {
                            Id = deductibleId,
                        };
                    }
                }
            }

            return coverages;
        }

        /// <summary>
        /// Crear agencias adicionales
        /// </summary>
        /// <param name="template"></param>
        /// <returns>Lista IssuanceAgency</returns>
        public List<IssuanceAgency> CreateAdditionalAgencies(Template template, ref string errroAgencies)
        {
            List<IssuanceAgency> agencies = new List<IssuanceAgency>();
            string error = String.Empty;

            if (template != null)
            {
                templateName = template.Description;

                foreach (Row row in template.Rows)
                {
                    int agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    int agentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));

                    IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeId);
                    if (agency == null)
                    {
                        error = Errors.ErrorIntermediaryNotExist + agentCode;
                    }
                    else
                    {
                        agency.Participation = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentParticipation));
                        agencies.Add(agency);
                    }

                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                errroAgencies += error + KeySettings.ReportErrorSeparatorMessage();
            }
            return agencies;
        }


        /// <summary>
        /// Coseguro Asignado
        /// </summary>
        /// <param name="template">template</param>
        /// <returns>Lista de CompanyIssuanceCoInsuranceCompany</returns>
        public List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAssigned(CompanyPolicy companyPolicy, Template template)
        {
            if (template != null)
            {
                templateName = template.Description;
                List<CompanyIssuanceCoInsuranceCompany> coInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();

                foreach (Row row in template.Rows)
                {
                    CompanyIssuanceCoInsuranceCompany coInsuranceCompany = new CompanyIssuanceCoInsuranceCompany();
                    coInsuranceCompany.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany));
                    coInsuranceCompany.ParticipationPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsurancePercentage));
                    coInsuranceCompany.ExpensesPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceExpenses));
                    coInsuranceCompanies.Add(coInsuranceCompany);
                }
                companyPolicy.CoInsuranceCompanies = coInsuranceCompanies;

                if (companyPolicy.CoInsuranceCompanies.GroupBy(c => c.Id, assigned => assigned).Select(x => x.First()).Count() != companyPolicy.CoInsuranceCompanies.Count)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceDuplicate);
                }

                percentage = companyPolicy.CoInsuranceCompanies.Sum(p => p.ParticipationPercentage);
                if (percentage > 99)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageExceeds);
                }
                else if (percentage < 1)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageIsLess);
                }

                percentageOwn = 100 - percentage;
                companyPolicy.CoInsuranceCompanies.ForEach(p => p.ParticipationPercentageOwn = percentageOwn);
                percentageExpenses = companyPolicy.CoInsuranceCompanies.Sum(x => x.ExpensesPercentage);

                if (percentageExpenses > 100)
                {
                    throw new ValidationException(Errors.MessageCoInsurancePercentageExpensesExceeds);
                }

                return coInsuranceCompanies;
            }
            else
            {
                throw new ValidationException(Errors.MessageCoinsuranceAssignedTemplateNotFound);
            }
        }

        /// <summary>
        /// Coaseguro Aceptado , Inter Part Coaseguro Aceptado
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAccepted(CompanyPolicy companyPolicy, File file)
        {
            Template template = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);
            if (template != null)
            {
                List<CompanyIssuanceCoInsuranceCompany> coInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();

                foreach (Row row in template.Rows)
                {
                    CompanyIssuanceCoInsuranceCompany coInsuranceCompany = new CompanyIssuanceCoInsuranceCompany();

                    coInsuranceCompany.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany));
                    coInsuranceCompany.PolicyNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedPolicy));
                    coInsuranceCompany.EndorsementNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedEndorsement));
                    coInsuranceCompany.ParticipationPercentageOwn = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompanyPercentage));
                    coInsuranceCompany.ParticipationPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsurancePercentage));
                    coInsuranceCompany.ExpensesPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceExpenses));
                    coInsuranceCompanies.Add(coInsuranceCompany);
                }

                companyPolicy.CoInsuranceCompanies = coInsuranceCompanies;

                Template templateCoinsuranceAcceptedAgency = file.Templates.FirstOrDefault(x => x.PropertyName == CompanyTemplatePropertyName.CoinsuranceAcceptedAgency);
                if (templateCoinsuranceAcceptedAgency == null)
                {
                    templateName = "";
                    throw new ValidationException(Errors.ErrorInterPartCoinsuranceAcceptedTemplateMandatory);
                }

                List<CompanyAcceptCoInsuranceAgent> companyAcceptCoInsuranceAgent = CreateCompanyAcceptCoInsuranceAgent(templateCoinsuranceAcceptedAgency);

                if (companyPolicy.CoInsuranceCompanies.Count > 1)
                {
                    throw new ValidationException(Errors.MessageCoinsuranceAcceptedAllowOnlyOne);
                }

                percentageOwn = companyPolicy.CoInsuranceCompanies.Sum(x => x.ParticipationPercentageOwn);
                if (percentageOwn > 100)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageExceeds);
                }
                else if (percentageOwn < 1)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageIsLess);
                }

                percentage = companyPolicy.CoInsuranceCompanies.Sum(p => p.ParticipationPercentage);
                if (percentage > 100)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageExceeds);
                }
                else if (percentage < 1)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageIsLess);
                }

                decimal totalPercentage = percentage + percentageOwn;
                if (totalPercentage > 100)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageExceeds);
                }
                else if (totalPercentage < 100)
                {
                    throw new ValidationException(Errors.MessageCoInsuranceParticipationPercentageIsLess);
                }

                percentageExpenses = companyPolicy.CoInsuranceCompanies.Sum(p => p.ExpensesPercentage);
                if (percentageExpenses > 100 || percentage < 0)
                {
                    throw new ValidationException(Errors.MessageCoInsurancePercentageExpensesExceeds);
                }

                if (companyAcceptCoInsuranceAgent.Any())
                {
                    decimal companyAcceptCoInsuranceAgentParticipation = companyAcceptCoInsuranceAgent.Sum(x => x.ParticipationPercentage);

                    if (companyAcceptCoInsuranceAgentParticipation != 100)
                    {
                        throw new ValidationException(Errors.MessageAgentsParticipationPercentageExceeds);
                    }
                    companyPolicy.CoInsuranceCompanies[0].acceptCoInsuranceAgent = new List<CompanyAcceptCoInsuranceAgent>();
                    foreach (CompanyAcceptCoInsuranceAgent item in companyAcceptCoInsuranceAgent)
                    {
                        companyPolicy.CoInsuranceCompanies[0].acceptCoInsuranceAgent.Add(item);
                    }
                }
                else
                {
                    throw new ValidationException(Errors.ErrorValidateCoInsuranceAcceptedAgents);
                }

                return coInsuranceCompanies;
            }
            else
            {
                throw new ValidationException(Errors.MessageCoinsuranceAcceptedTemplateNotFound);
            }
        }

        /// <summary>
        /// Crea lista de Inter Part Coaseguro Aceptado
        /// </summary>
        /// <param name="template">template</param>
        /// <returns>Lista de CompanyAcceptCoInsuranceAgent</returns>
        private List<CompanyAcceptCoInsuranceAgent> CreateCompanyAcceptCoInsuranceAgent(Template template)
        {
            List<CompanyAcceptCoInsuranceAgent> companyAcceptCoInsuranceAgents = new List<CompanyAcceptCoInsuranceAgent>();

            if (template != null)
            {
                //Obtiene el máximo de intermediarios permitidos para coaseguros aceptados
                int maxIntemediaries = DelegateService.commonService.GetParameterByParameterId((int)CompanyParameterType.MaxAgentCoinsuranceAccepted).NumberParameter.GetValueOrDefault();

                if (maxIntemediaries > 0 && template.Rows.Count > maxIntemediaries)
                {
                    throw new ValidationException(string.Format(Errors.ErrorMaxAgentCoinsuranceAccepted, maxIntemediaries));
                }

                //Obtiene los fields del código de agente
                var fields = template.Rows.SelectMany(r => r.Fields.Where(f => f.PropertyName == "AgentCode")).ToArray();

                //Valida que no esté el mismo agente más de una vez
                foreach (var field in fields)
                {
                    if (fields.Where(f => f.Value == field.Value).Count() > 1)
                        throw new ValidationException(Errors.ErrorRepeatedAgentCode);
                }

                templateName = template.Description;

                foreach (Row row in template.Rows)
                {
                    int agentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    if (agentTypeId == 2 || agentTypeId == 3 || agentTypeId == 4)
                    {

                        int agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                        IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeId);

                        if (agency != null)
                        {
                            CompanyAcceptCoInsuranceAgent companyAcceptCoInsuranceAgent = ModelAssembler.MapCompanyAcceptCoInsuranceAgentFromAgency(agency);
                            companyAcceptCoInsuranceAgent.ParticipationPercentage = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentParticipation));
                            companyAcceptCoInsuranceAgents.Add(companyAcceptCoInsuranceAgent);
                        }
                        else
                        {
                            throw new ValidationException(Errors.AgentNotFound + string.Format(Errors.ErrorInTemplate, templateName));
                        }
                    }
                    else
                    {
                        row.HasError = true;
                        row.ErrorDescription = Errors.AgentTypeNotValid + string.Format(Errors.ErrorInTemplate, templateName);
                        throw new ValidationException(Errors.AgentTypeNotValid + string.Format(Errors.ErrorInTemplate, templateName));
                    }

                }
            }

            return companyAcceptCoInsuranceAgents;
        }

        public List<DynamicConcept> GetDynamicConceptsByTemplate(int? scriptId, Template templateScripts, ref string error)
        {

            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();
            List<ScriptComposite> scriptComposites = new List<ScriptComposite>();
            string currentError = string.Empty;
            try
            {
                if (templateScripts != null)
                {
                    foreach (Row row in templateScripts.Rows)
                    {
                        ScriptComposite scriptComposite = null;
                        List<Edge> edges = null;
                        int? conceptId = null;
                        int script = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Script));
                        string answer = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Answer));
                        int questionId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Question));
                        int entityId = (int)FacadeType.RULE_FACADE_RISK;
                        if (script == scriptId)
                        {
                            try
                            {
                                scriptComposite = scriptComposites.FirstOrDefault(u => u.Script.ScriptId == script);
                                if (scriptComposite == null)
                                {
                                    scriptComposite = DelegateService.scriptsService.GetScriptComposite(script);
                                    scriptComposites.Add(scriptComposite);
                                }

                            }
                            catch (Exception)
                            {
                                error += string.Format(Resources.Errors.ErrorGetScript, script) + KeySettings.ReportErrorSeparatorMessage();
                                continue;
                            }

                            if (scriptComposite != null)
                            {
                                if (scriptComposite.Nodes.Any())
                                {
                                    if (scriptComposite.Nodes.Exists(u => u.Questions.Exists(z => z.QuestionId == questionId)))
                                    {
                                        conceptId = scriptComposite.Nodes.SelectMany(u => u.Questions).Distinct().FirstOrDefault(u => u.QuestionId == questionId).ConceptId;
                                        edges = scriptComposite.Nodes.SelectMany(u => u.Questions).Distinct().FirstOrDefault(u => u.QuestionId == questionId).Edges;
                                    }
                                    else
                                    {
                                        currentError += string.Format(Resources.Errors.ErrorQuestionNotFound, questionId) + KeySettings.ReportErrorSeparatorMessage();
                                    }
                                }
                                else
                                {
                                    currentError += string.Format(Resources.Errors.ErrorQuestionNotFound, questionId) + KeySettings.ReportErrorSeparatorMessage();
                                }

                                int value = 0;
                                bool result = int.TryParse(answer, out value);
                                if (result && edges != null)
                                {
                                    var edge = edges.FirstOrDefault(u => u.ValueCode == value && !string.IsNullOrEmpty(u.Description));
                                    if (edge != null)
                                    {
                                        switch (edge.Description)
                                        {
                                            case "NO":
                                                answer = "false";
                                                break;
                                            case "SI":
                                                answer = "true";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        currentError += string.Format(Resources.Errors.ErrorInvalidAnswer, answer) + KeySettings.ReportErrorSeparatorMessage();
                                    }
                                }

                                if (string.IsNullOrEmpty(currentError))
                                {
                                    DynamicConcept dynamicConcept = new DynamicConcept
                                    {
                                        Id = conceptId.Value,
                                        Value = answer,
                                        EntityId = entityId,
                                        QuestionId = questionId

                                    };
                                    if (!dynamicConcepts.Any(u => u.Id == dynamicConcept.Id))
                                    {
                                        dynamicConcepts.Add(dynamicConcept);
                                    }
                                    else
                                    {
                                        error += Resources.Errors.ErrorDuplicateQuestion + questionId + " Fila " + row.Number + KeySettings.ReportErrorSeparatorMessage();

                                    }
                                }
                                else
                                {
                                    error += currentError + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                            else
                            {
                                error += string.Format(Resources.Errors.ErrorScriptNotFound, script) + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                        else
                        {
                            error += string.Format(Resources.Errors.ErrorScriptToProduct, script);
                        }

                        currentError = string.Empty;
                    }
                }

            }
            catch (Exception)
            {

                error += Resources.Errors.ErrorGetDynamicConceptsByTemplate + KeySettings.ReportErrorSeparatorMessage();
            }

            return dynamicConcepts;
        }

        /// <summary>
        /// Agrega los benifiarios Adicionales
        /// </summary>
        /// <param name="file"></param>
        /// <param name="template"></param>
        /// <param name="filterIndividuals"></param>
        /// <param name="error"></param>
        /// <returns>lista beneficiarios</returns>
        public List<CompanyBeneficiary> GetBeficiaresAdditional(File file, Template template, List<FilterIndividual> filterIndividuals, List<CompanyBeneficiary> companyBeneficiaries, ref string error)
        {
            string currentError = string.Empty;
            IMassiveService massiveService = new MassiveServiceEEProvider();
            List<CompanyBeneficiary> beneficiaries = massiveService.CreateAdditionalBeneficiaries(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), filterIndividuals);
            foreach (CompanyBeneficiary beneficiary in beneficiaries)
            {
                if (beneficiary.Participation == 0)
                {
                    currentError += Errors.ErrorBeneficiaryParticipationCero + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            companyBeneficiaries.AddRange(beneficiaries);
            foreach (CompanyBeneficiary beneficiary in beneficiaries)
            {
                if (beneficiary.Participation == 0)
                {
                    currentError += Errors.ErrorBeneficiaryParticipation + KeySettings.ReportErrorSeparatorMessage();
                }
            }

            if (companyBeneficiaries.GroupBy(b => b.IndividualId, ben => ben).Select(b => b.First()).ToList().Count != companyBeneficiaries.Count)
            {
                throw new ValidationException(Errors.ErrorBeneficiariesAdditionalDuplicated);
            }

            decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);

            if (beneficiariesParticipation < 100)
            {
                companyBeneficiaries[0].Participation -= beneficiariesParticipation;
            }
            else
            {
                currentError += Errors.ErrorParticipationBeneficiary + KeySettings.ReportErrorSeparatorMessage();
            }

            if (!string.IsNullOrEmpty(currentError))
            {
                error += currentError + KeySettings.ReportErrorSeparatorMessage();
            }
            currentError = string.Empty;
            return companyBeneficiaries;
        }

        public List<Row> GetMassivePlatesValidation(List<Row> rows)
        {
            List<string> plates = new List<string>();
            string currentPlate = null;

            foreach (var row in rows)
            {
                currentPlate = row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskLicensePlate).Value;
                if (plates.Contains(currentPlate))
                {
                    row.HasError = true;
                    row.ErrorDescription = String.Format(Errors.DuplicatedPlate, row.Fields.First(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value);
                }
                else
                {
                    plates.Add(currentPlate);
                }
            }

            return rows;
        }

        public List<IssuanceAgency> GetAgenciesValidation(File file, List<IssuanceAgency> issuanceAgency, ref string error)
        {
            string currentError = string.Empty;
            string errorAgencies = string.Empty;
            List<IssuanceAgency> agencies = CreateAdditionalAgencies(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries), ref errorAgencies);
            if (string.IsNullOrEmpty(errorAgencies))
            {
                decimal agenciesParticipation = agencies.Sum(x => x.Participation);

                if (agenciesParticipation < 100)
                {
                    issuanceAgency[0].Participation -= agenciesParticipation;
                    agencies.ForEach(x => x.Commissions = issuanceAgency[0].Commissions);
                    issuanceAgency.AddRange(agencies);
                }
                else
                {
                    currentError += Errors.MessageIntermediariesParticipationPercentageExceeds + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            else
            {
                currentError = errorAgencies;
                errorAgencies = string.Empty;
            }

            if (!string.IsNullOrEmpty(currentError))
            {
                error += currentError + KeySettings.ReportErrorSeparatorMessage();
            }
            currentError = string.Empty;
            return issuanceAgency;
        }

        #endregion

    }

}
