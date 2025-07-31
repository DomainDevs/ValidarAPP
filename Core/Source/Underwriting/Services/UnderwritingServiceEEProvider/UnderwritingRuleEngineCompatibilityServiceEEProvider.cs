using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using COMMON = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using REQEN = Sistran.Core.Application.Request.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider
{
    /// <summary>
    /// Funciones Motor Reglas
    /// </summary>
    public class UnderwritingRuleEngineCompatibilityServiceEEProvider
    {
        /// <summary>
        /// Agregar Cláusula a Riesgo
        /// </summary>
        /// <param name="facade"></param>
        public void AddRiskClause(Rules.Facade facade)
        {
            int parameterId = 139;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? clauseId = facade.GetConcept<int?>(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value));

                if (clauseId != null)
                {
                    List<int> clausesAdd = facade.GetConcept<List<int>>(RuleConceptRisk.ClausesAdd);
                    List<int> clausesRemove = facade.GetConcept<List<int>>(RuleConceptRisk.ClausesRemove);

                    if (clausesAdd == null)
                    {
                        clausesAdd = new List<int>();
                    }

                    if (clausesRemove == null)
                    {
                        clausesRemove = new List<int>();
                    }

                    if (!clausesAdd.Exists(x => x == clauseId.Value))
                    {
                        clausesAdd.Add(clauseId.Value);
                    }

                    clausesRemove.Remove(clauseId.Value);

                    facade.SetConcept(RuleConceptRisk.ClausesAdd, clausesAdd);
                    facade.SetConcept(RuleConceptRisk.ClausesRemove, clausesRemove);
                }
            }
        }

        /// <summary>
        /// Eliminar Cláusula de Riesgo
        /// </summary>
        /// <param name="facade"></param>
        public void DeleteRiskClause(Rules.Facade facade)
        {
            int parameterId = 139;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? clauseId = facade.GetConcept<int?>(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value));

                if (clauseId != null)
                {
                    List<int> clausesAdd = facade.GetConcept<List<int>>(RuleConceptRisk.ClausesAdd);
                    List<int> clausesRemove = facade.GetConcept<List<int>>(RuleConceptRisk.ClausesRemove);

                    if (clausesAdd == null)
                    {
                        clausesAdd = new List<int>();
                    }

                    if (clausesRemove == null)
                    {
                        clausesRemove = new List<int>();
                    }

                    if (!clausesRemove.Exists(x => x == clauseId.Value))
                    {
                        clausesRemove.Add(clauseId.Value);
                    }

                    clausesAdd.Remove(clauseId.Value);

                    facade.SetConcept(RuleConceptRisk.ClausesAdd, clausesAdd);
                    facade.SetConcept(RuleConceptRisk.ClausesRemove, clausesRemove);
                }
            }
        }

        /// <summary>
        /// Agregar Cobertura
        /// </summary>
        /// <param name="facade">The facade.</param>
        public void AddCoverage(Rules.Facade facade)
        {
            int parameterId = 134;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? coverageId = facade.GetConcept<int?>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));

                if (coverageId != null)
                {
                    List<int> coverageAdd = facade.GetConcept<List<int>>(RuleConceptRisk.CoveragesAdd);
                    List<int> coverageRemove = facade.GetConcept<List<int>>(RuleConceptRisk.CoveragesRemove);
                    if (coverageAdd == null)
                    {
                        coverageAdd = new List<int>();
                    }
                    if (coverageRemove == null)
                    {
                        coverageRemove = new List<int>();
                    }
                    if (!coverageAdd.Exists(x => x == coverageId.Value))
                    {
                        coverageAdd.Add(coverageId.Value);
                    }
                    coverageRemove.Remove(coverageId.Value);
                    facade.SetConcept(RuleConceptRisk.CoveragesAdd, coverageAdd);
                    facade.SetConcept(RuleConceptRisk.CoveragesRemove, coverageRemove);
                }
            }
        }

        /// <summary>
        /// Elimina una cobertura
        /// </summary>
        /// <param name="facade"></param>
        public void DeleteCoverage(Rules.Facade facade)
        {
            int parameterId = 134;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? coverageId = facade.GetConcept<int?>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));

                if (coverageId != null)
                {
                    List<int> coverageAdd = facade.GetConcept<List<int>>(RuleConceptRisk.CoveragesAdd);
                    List<int> coverageRemove = facade.GetConcept<List<int>>(RuleConceptRisk.CoveragesRemove);
                    if (coverageAdd == null)
                    {
                        coverageAdd = new List<int>();
                    }
                    if (coverageRemove == null)
                    {
                        coverageRemove = new List<int>();
                    }
                    if (!coverageRemove.Exists(x => x == coverageId.Value))
                    {
                        coverageRemove.Add(coverageId.Value);
                    }
                    coverageAdd.Remove(coverageId.Value);
                    facade.SetConcept(RuleConceptRisk.CoveragesAdd, coverageAdd);
                    facade.SetConcept(RuleConceptRisk.CoveragesRemove, coverageRemove);
                }
            }
        }

        /// <summary>
        /// Asignar Deducible
        /// </summary>
        /// <param name="facade"></param>
        public void AddDeductible(Rules.Facade facade)
        {
            int parameterId = 522;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? deductId = facade.GetConcept<int?>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));

                if (deductId != null)
                {
                    facade.SetConcept(RuleConceptCoverage.DeductId, deductId);
                }
            }
        }

        /// <summary>
        /// Desasignar Deducible
        /// </summary>
        /// <param name="facade"></param>
        public void DeleteDeductible(Rules.Facade facade)
        {
            facade.SetConcept(RuleConceptCoverage.DeductId, 0);
            facade.SetConcept(RuleConceptCoverage.DeductRateTypeCode, null);
            facade.SetConcept(RuleConceptCoverage.DeductRate, null);
            facade.SetConcept(RuleConceptCoverage.DeductValue, null);
            facade.SetConcept(RuleConceptCoverage.DeductUnitCode, null);
            facade.SetConcept(RuleConceptCoverage.DeductSubjectCode, null);
            facade.SetConcept(RuleConceptCoverage.MinDeductValue, null);
            facade.SetConcept(RuleConceptCoverage.MinDeductUnitCode, null);
            facade.SetConcept(RuleConceptCoverage.MinDeductSubjectCode, null);
            facade.SetConcept(RuleConceptCoverage.MaxDeductValue, null);
            facade.SetConcept(RuleConceptCoverage.MaxDeductUnitCode, null);
            facade.SetConcept(RuleConceptCoverage.MaxDeductSubjectCode, null);
            facade.SetConcept(RuleConceptCoverage.CurrencyCode, null);
            facade.SetConcept(RuleConceptCoverage.AccDeductAmt, null);
        }

        /// <summary>
        /// Cuenta el número de coberturas
        /// </summary>
        /// <param name="facade"></param>
        public void GetCountCoverages(Rules.Facade facade)
        {
            int parameterId = 2167;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value),
                    facade.GetConcept<int>(RuleConceptRisk.CoveragesCount));
            }
        }

        /// <summary>
        /// Asigna Prima Mínima
        /// </summary>
        /// <param name="facade"></param>
        public void GetMinimumPremiumCoverage(Rules.Facade facade)
        {
            int parameterId = 2206;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value),
                    facade.GetConcept<int>(RuleConceptCoverage.MinimumPremiumCoverage));
            }
        }

        public void AssignNewProduct(Rules.Facade facade)
        {
            int parameterId = 540;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? productId = facade.GetConcept<int?>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));

                if (productId != null)
                {
                    facade.SetConcept(RuleConceptGeneral.ProductId, productId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">Lista de facade</param>
        /// <returns>La lista de los parátros con los conceptos dinámicos asignados</returns>
        public void GetRiskContractualDate(Rules.Facade facade)
        {
            int parameterId = 529;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? coverageGroupId = facade.GetConcept<int?>(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value));

                if (coverageGroupId != null)
                {
                    List<int> coverageGroup = facade.GetConcept<List<int>>(RuleConceptRisk.CoverageGroupId);

                    if (coverageGroup == null)
                    {
                        coverageGroup = new List<int>();
                    }
                    facade.SetConcept(RuleConceptRisk.CoverageGroupId, coverageGroup);
                }
            }
        }

        //TODO Actualizar esta función con la consulta de 2G sin temporales
        /// <summary>
        /// Recuperación de Datos de Sise 2G
        /// </summary>
        /// <param name="facade"></param>
        public void GetClaimsHistoryFrom2G(Rules.Facade facade)
        {
            int parameterSinisterQuantity = 2108;
            int parameterRenewalQuantity = 2109;
            int parameterPortfolioBalance = 2110;
            int parameterTotalLoss = 2111;
            int parameterSinisterLastThreeYears = 2112;

            List<Parameter> modelParameters = new List<Parameter>();

            modelParameters.Add(new Parameter
            {
                Id = parameterSinisterQuantity
            });
            modelParameters.Add(new Parameter
            {
                Id = parameterRenewalQuantity
            });
            modelParameters.Add(new Parameter
            {
                Id = parameterPortfolioBalance
            });
            modelParameters.Add(new Parameter
            {
                Id = parameterTotalLoss
            });
            modelParameters.Add(new Parameter
            {
                Id = parameterSinisterLastThreeYears
            });

            modelParameters = DelegateService.commonServiceCore.GetParametersByParameterIds(modelParameters);

            if (facade != null)
            {
                if (modelParameters.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterSinisterQuantity).NumberParameter.Value), 0);
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterRenewalQuantity).NumberParameter.Value), 0);
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterPortfolioBalance).NumberParameter.Value), 0);
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterTotalLoss).NumberParameter.Value), 0);
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterSinisterLastThreeYears).NumberParameter.Value), 0);
                }

                TMPEN.CoTempSubscription2g coTempSubscription2g = CoTempSubscription2gDAO.GetCoTempSubscription2gByTempId(RuleConceptRisk.TempId.Value);

                if (coTempSubscription2g != null)
                {
                    if (modelParameters.Count > 0)
                    {
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterSinisterQuantity).NumberParameter.Value), coTempSubscription2g.SinisterQuantity);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterRenewalQuantity).NumberParameter.Value), coTempSubscription2g.RenewalQuantity);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterPortfolioBalance).NumberParameter.Value), coTempSubscription2g.PortfolioBalance);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterTotalLoss).NumberParameter.Value), coTempSubscription2g.HasTotalLoss);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(modelParameters.First(x => x.Id == parameterSinisterLastThreeYears).NumberParameter.Value), coTempSubscription2g.StroLastThreeYears);
                    }
                }
            }

        }

        /// <summary>
        /// Obtener Id de deducible del temporal
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void GetDeductibleCoverageIdToRule(Rules.Facade facade)
        {
            int parameterId = 2138;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value), RuleConceptCoverage.DeductId);
            }

        }

        #region EventosEmision

        public void ExecuteQuery(Rules.Facade facade)
        {
            try
            {
                int? eventId = facade.GetConcept<int?>(RuleConceptPolicies.EventId);
                if (eventId != null)
                {
                    Func<SCREN.Concept, bool> predicate = (str) => str.ConceptName == "SourceTable";
                    SCREN.Concept sourceTableConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    predicate = (str) => str.ConceptName == "SourceColumn";
                    SCREN.Concept sourceColumnConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    predicate = (str) => str.ConceptName == "ParamWhere";
                    SCREN.Concept paramWhereConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    predicate = (str) => str.ConceptName == "WhereValue_1";
                    SCREN.Concept whereValue1Concept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    predicate = (str) => str.ConceptName == "WhereValue_2";
                    SCREN.Concept whereValue2Concept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    predicate = (str) => str.ConceptName == "WhereValue_3";
                    SCREN.Concept whereValue3Concept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    predicate = (str) => str.ConceptName == "ResultValue";
                    SCREN.Concept resultValueConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, predicate);

                    if (sourceTableConcept != null && sourceColumnConcept != null && paramWhereConcept != null)
                    {

                        var sourceTable = facade.GetConcept<object>(RuleConceptPolicies.DynamicConcept(sourceTableConcept.ConceptId));
                        var sourceColumn = facade.GetConcept<object>(RuleConceptPolicies.DynamicConcept(sourceColumnConcept.ConceptId));
                        var paramWhere = facade.GetConcept<object>(RuleConceptPolicies.DynamicConcept(paramWhereConcept.ConceptId));
                        var whereValue1 = facade.GetConcept<object>(RuleConceptPolicies.DynamicConcept(whereValue1Concept.ConceptId));
                        var whereValue2 = facade.GetConcept<object>(RuleConceptPolicies.DynamicConcept(whereValue2Concept.ConceptId));
                        var whereValue3 = facade.GetConcept<object>(RuleConceptPolicies.DynamicConcept(whereValue3Concept.ConceptId));

                        if (sourceTable != null && sourceColumn != null && paramWhere != null)
                        {
                            NameValue[] nameValues = new NameValue[3];
                            nameValues[0] = new NameValue("@TABLES", sourceTable.ToString());
                            nameValues[1] = new NameValue("@FIELDS", sourceColumn.ToString());

                            string filter = paramWhere.ToString();

                            if (whereValue1 != null && whereValue2 != null && whereValue3 != null)
                            {
                                filter = string.Format(paramWhere.ToString(), whereValue1, whereValue2, whereValue3);
                            }
                            else if (whereValue1 != null && whereValue2 != null)
                            {
                                filter = string.Format(paramWhere.ToString(), whereValue1, whereValue2);
                            }
                            else if (whereValue1 != null)
                            {
                                filter = string.Format(paramWhere.ToString(), whereValue1);
                            }
                            nameValues[2] = new NameValue("@FILTER", filter);

                            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                            {
                                DataTable datas = dynamicDataAccess.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", nameValues);

                                foreach (DataRow objeData in datas.Rows)
                                {
                                    string result = objeData.ItemArray[0].ToString();
                                    facade.SetConcept(RuleConceptPolicies.DynamicConcept(resultValueConcept.ConceptId), result);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void ChangeAgent(Rules.Facade facade)
        {
            try
            {
                int? facadeGeneral = facade.GetConcept<int?>(RuleConceptGeneral.EndorsementId);
                if (facadeGeneral != null)
                {
                    PolicyDAO policyDAO = new PolicyDAO();
                    var agentActual = facade.GetConcept<int>(RuleConceptGeneral.PrimaryAgentCode);
                    var agentOld = policyDAO.GetAgentsByPolicyIdEndorsementId(RuleConceptGeneral.PolicyId.Value, RuleConceptGeneral.EndorsementId.Value);



                    if (agentOld != null)
                    {
                        if (agentActual != agentOld.Result.FirstOrDefault().Agent.IndividualId)
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        //Creación de funciones R1
        /// <summary>
        /// Agregar Cláusula General
        /// </summary>
        /// <param name="facade"></param>
        public void AddGeneralClause(Rules.Facade facade)
        {
            int parameterId = 138;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? clauseId = facade.GetConcept<int?>(RuleConceptGeneral.DynamicConcept(parameter.NumberParameter.Value));

                if (clauseId != null)
                {
                    List<int> clausesAdd = facade.GetConcept<List<int>>(RuleConceptGeneral.ClausesAdd);
                    List<int> clausesRemove = facade.GetConcept<List<int>>(RuleConceptGeneral.ClausesRemove);

                    if (clausesAdd == null)
                    {
                        clausesAdd = new List<int>();
                    }

                    if (clausesRemove == null)
                    {
                        clausesRemove = new List<int>();
                    }

                    if (!clausesAdd.Exists(x => x == clauseId.Value))
                    {
                        clausesAdd.Add(clauseId.Value);
                    }

                    clausesRemove.Remove(clauseId.Value);

                    facade.SetConcept(RuleConceptGeneral.ClausesAdd, clausesAdd);
                    facade.SetConcept(RuleConceptGeneral.ClausesRemove, clausesRemove);
                }
            }
        }

        /// <summary>
        /// Eliminar Cláusula General
        /// </summary>
        /// <param name="facade"></param>
        public void DeleteGeneralClause(Rules.Facade facade)
        {
            int parameterId = 138;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? clauseId = facade.GetConcept<int?>(RuleConceptGeneral.DynamicConcept(parameter.NumberParameter.Value));

                if (clauseId != null)
                {
                    List<int> clausesAdd = facade.GetConcept<List<int>>(RuleConceptGeneral.ClausesAdd);
                    List<int> clausesRemove = facade.GetConcept<List<int>>(RuleConceptGeneral.ClausesRemove);
                    if (clausesAdd == null)
                    {
                        clausesAdd = new List<int>();
                    }
                    if (clausesRemove == null)
                    {
                        clausesRemove = new List<int>();
                    }
                    if (!clausesRemove.Exists(x => x == clauseId.Value))
                    {
                        clausesRemove.Add(clauseId.Value);
                    }
                    clausesAdd.Remove(clauseId.Value);
                    facade.SetConcept(RuleConceptGeneral.ClausesAdd, clausesAdd);
                    facade.SetConcept(RuleConceptGeneral.ClausesRemove, clausesRemove);
                }
            }
        }

        /// <summary>
        /// Eliminar Cláusula de Cobertura
        /// </summary>
        /// <param name="facade"></param>
        public void DeleteCoverageClause(Rules.Facade facade)
        {
            int parameterId = 137;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int? clauseId = facade.GetConcept<int?>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));

                if (clauseId != null)
                {
                    List<int> clausesAdd = facade.GetConcept<List<int>>(RuleConceptCoverage.ClausesAdd);
                    List<int> clausesRemove = facade.GetConcept<List<int>>(RuleConceptCoverage.ClausesRemove);

                    if (clausesAdd == null)
                    {
                        clausesAdd = new List<int>();
                    }

                    if (clausesRemove == null)
                    {
                        clausesRemove = new List<int>();
                    }

                    if (!clausesRemove.Exists(x => x == clauseId.Value))
                    {
                        clausesRemove.Add(clauseId.Value);
                    }

                    clausesAdd.Remove(clauseId.Value);

                    facade.SetConcept(RuleConceptCoverage.ClausesAdd, clausesAdd);
                    facade.SetConcept(RuleConceptCoverage.ClausesRemove, clausesRemove);
                }
            }
        }

        /// <summary>
        /// Total Prima Técnica
        /// </summary>
        /// <param name="facade"></param>
        public void GetTotalTechnicalPremiunAmount(Rules.Facade facade)
        {
            int parameterId = 145;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(RuleConceptComponent.DynamicConcept(parameter.NumberParameter.Value), RuleConceptComponent.CalculationBaseAmount);
            }
        }

        /// <summary>
        /// Cantidad de Riesgos por Tipo
        /// </summary>
        /// <param name="facade"></param>
        public void GetSusbcriptionAmountTotalRiskofTypeRiskCovered(Rules.Facade facade)
        {
            int coveredRiskTypeParameterId = 450;
            int countRisksParameterId = 451;
            int riskStatusParameterId = 452;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter
            {
                Id = coveredRiskTypeParameterId
            });
            parameters.Add(new Parameter
            {
                Id = countRisksParameterId
            });
            parameters.Add(new Parameter
            {
                Id = riskStatusParameterId
            });

            parameters = DelegateService.commonServiceCore.GetParametersByParameterIds(parameters);

            if (parameters.Count == 3)
            {
                int coveredRiskType = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(parameters.First(x => x.Id == coveredRiskTypeParameterId).NumberParameter.Value));
                int riskStatus = facade.GetConcept<int>(RuleConceptGeneral.DynamicConcept(parameters.First(x => x.Id == riskStatusParameterId).NumberParameter.Value));

                if (coveredRiskType > 0 && riskStatus > 0)
                {
                    List<Risk> risks = facade.GetConcept<List<Risk>>(RuleConceptGeneral.Risks);

                    if (risks != null)
                    {
                        int countRisks = risks.Count(x => x.CoveredRiskType == (CommonService.Enums.CoveredRiskType)coveredRiskType && x.Status == (Enums.RiskStatusType)riskStatus);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(parameters.First(x => x.Id == countRisks).NumberParameter.Value), countRisks);
                    }
                }
            }
        }

        /// <summary>
        /// Código de Agente Principal
        /// </summary>
        /// <param name="facade"></param>
        public void GetPrimaryAgentAgencyCode(Rules.Facade facade)
        {
            const int parameterId = 2117;
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int primaryAgentCode = facade.GetConcept<int>(RuleConceptGeneral.PrimaryAgentCode);
                if (primaryAgentCode > 0)
                {
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(parameter.NumberParameter.Value), primaryAgentCode);
                }
            }
        }

        /// <summary>
        /// Obtener gastos de solicitud
        /// </summary>
        /// <param name="facade"></param>
        public void GetExpensesRequestToRule(Rules.Facade facade)
        {
            const int parameterId = 2127;

            int requestEndorsmentId = facade.GetConcept<int>(RuleConceptGeneral.RequestEndorsementId);
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            int ConceptId = 0;
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                ConceptId = parameter.NumberParameter.Value;
            }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REQEN.CoRequestEndorsement.Properties.RequestEndorsementId, requestEndorsmentId);
            BusinessCollection<REQEN.CoRequestEndorsement> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<REQEN.CoRequestEndorsement>(filter.GetPredicate());
            if (businessCollection != null && businessCollection.Count > 0 && ConceptId > 0)
            {
                REQEN.CoRequestEndorsement requestEndorsment = businessCollection[0];
                facade.SetConcept(RuleConceptGeneral.DynamicConcept(ConceptId), requestEndorsment.IssueExpensesAmount);
            }
        }

        /// <summary>
        /// Setear valor limite rc
        /// </summary>
        /// <param name="facade"></param>
        public void GetLimitRcValue(Rules.Facade facade)
        {
            //Se debe esperar validacio nde tablas dinamicas
            const int parameterId = 2185; //LimitsRcCod concept
            const int parameterIdValue = 2186; //LimitsRcValue concept

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int RcCode = facade.GetConcept<int>(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value));

                COMMON.CoLimitsRc rcObject = (COMMON.CoLimitsRc)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(COMMON.CoLimitsRc.CreatePrimaryKey(RcCode));

                decimal limitRcValue = 0;
                if (rcObject != null)
                {
                    //Validación R1
                    if (rcObject.LimitUnique.Value == 0)
                        limitRcValue = rcObject.Limit1.Value + rcObject.Limit3.Value;
                    else
                        limitRcValue = rcObject.LimitUnique.Value;

                    if (rcObject.LimitUnique.HasValue)
                        if (rcObject.LimitUnique.Value != 0)
                            limitRcValue = rcObject.LimitUnique.Value;
                }

                parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterIdValue);
                if (parameter != null && parameter.NumberParameter.HasValue)
                {
                    facade.SetConcept(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), limitRcValue);
                }
            }

        }

        /// <summary>
        /// Total Beneficiarios Onerosos/No Onerosos
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void GetCountBenefOnerousNoOnerousToRule(Rules.Facade facade)
        {
            int BeneficiaryOnerosoParameter = 2102;
            int BeneficiaryNotOnerosoParameter = 2103;

            List<Parameter> modelParameters = new List<Parameter>();
            modelParameters.Add(new Parameter
            {
                Id = BeneficiaryOnerosoParameter
            });
            modelParameters.Add(new Parameter
            {
                Id = BeneficiaryNotOnerosoParameter
            });

            modelParameters = DelegateService.commonServiceCore.GetParametersByParameterIds(modelParameters);
            if (modelParameters.Exists(x => x.Id == BeneficiaryOnerosoParameter))
            {
                facade.SetConcept(RuleConceptRisk.DynamicConcept(modelParameters.First(x => x.Id == BeneficiaryOnerosoParameter).NumberParameter.Value), RuleConceptRisk.BeneficiariesCountOneroso);
            }

            if (modelParameters.Exists(x => x.Id == BeneficiaryNotOnerosoParameter))
            {
                facade.SetConcept(RuleConceptRisk.DynamicConcept(modelParameters.First(x => x.Id == BeneficiaryNotOnerosoParameter).NumberParameter.Value), RuleConceptRisk.BeneficiariesCountNoOneroso);
            }


        }

        /// <summary>
        /// Tiene Obligaciones Pesados
        /// </summary>
        /// <param name="facade"></param>
        public void GetIsLiabilities(Rules.Facade facade)
        {
            //Se debe esperar validacio nde tablas dinamicas
            const int parameterId = 2197;
            const int parameterCoverageId = 2198;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterCoverageId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                List<Coverage> coverages = facade.GetConcept<List<Coverage>>(RuleConceptRisk.Coverages);

                bool IsLiabilities = false;

                if (coverages.Exists(x => x.Id == parameter.NumberParameter.Value))
                {
                    IsLiabilities = true;
                }

                parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

                if (parameter != null && parameter.NumberParameter.HasValue)
                {
                    facade.SetConcept(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), IsLiabilities);
                }
            }
        }

        /// <summary>
        /// Tiene Lucro Pesados
        /// </summary>
        /// <param name="facade"></param>
        public void GetIslucrePRV(Rules.Facade facade)
        {
            //Se debe esperar validacio nde tablas dinamicas
            const int parameterId = 2195;
            const int parameterCoverageId = 2196;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterCoverageId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                List<Coverage> coverages = facade.GetConcept<List<Coverage>>(RuleConceptRisk.Coverages);

                bool IsLucrePRV = false;

                if (coverages.Exists(x => x.Id == parameter.NumberParameter.Value))
                {
                    IsLucrePRV = true;
                }

                parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

                if (parameter != null && parameter.NumberParameter.HasValue)
                {
                    facade.SetConcept(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), IsLucrePRV);
                }
            }
        }

        //Eventos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateParametrizedCommission(Rules.Facade facade)
        {
            //valor para comparar
            int StandardCommissionPercentage = facade.GetConcept<int>(RuleConceptGeneral.StandardCommissionPercentage);
            //Valores para consultar
            int PrimaryAgentId = facade.GetConcept<int>(RuleConceptGeneral.PrimaryAgentId);
            int PrimnaryAgentAgencyId = facade.GetConcept<int>(RuleConceptGeneral.PrimnaryAgentAgencyId);
            int ProductId = facade.GetConcept<int>(RuleConceptGeneral.ProductId);

            ProductAgencyCommiss productAgencyCommiss = DelegateService.productServiceCore.GetCommissByAgentIdAgencyIdProductId(PrimaryAgentId, PrimnaryAgentAgencyId, ProductId);

            if (StandardCommissionPercentage > productAgencyCommiss.CommissPercentage)
            {
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }

        /// <summary>
        /// Función Validar Cobertura Eliminada
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateCoverageRemoved(Rules.Facade facade)
        {
            const int parameterId = 136;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                int dynamicId = facade.GetConcept<int>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));
                //Listas asociadas al riesgo
                List<Coverage> coverages = facade.GetConcept<List<Coverage>>(RuleConceptRisk.Coverages);


                if (coverages.Exists(x => x.Id == dynamicId))
                {
                    Coverage c = coverages.FirstOrDefault(x => x.Id == dynamicId);
                    if (c.CoverStatus == CoverageStatusType.Excluded)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
                else
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }

            }
        }

        /// <summary>
        /// Función Validar Anulación de Cancelación
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateReversionCancellation(Rules.Facade facade)
        {
            int endorsementId = facade.GetConcept<int>(RuleConceptGeneral.EndorsementId);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.Endorsement.Properties.EndorsementId, endorsementId);

            BusinessCollection<ISSEN.Endorsement> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.Endorsement>(filter.GetPredicate());
            if (businessCollection != null && businessCollection.Count > 0)
            {
                if (businessCollection[0].EndoTypeCode == (int)EndorsementType.Cancellation || businessCollection[0].EndoTypeCode == (int)EndorsementType.Nominative_cancellation)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
        }
    }
}