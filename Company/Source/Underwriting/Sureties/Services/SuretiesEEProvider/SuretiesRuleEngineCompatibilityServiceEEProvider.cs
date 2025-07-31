using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.Sureties.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using SCREN = Sistran.Core.Application.Script.Entities;

using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Sureties.EEProvider
{
    class SuretiesRuleEngineCompatibilityServiceEEProvider
    {

        public void OpenGuarantee(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Numero de polizas" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                Func<SCREN.Concept, bool> entityPredicateConcept = (str) => str == null ? false : str.ConceptName == "Nombre de las contragarantias" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConceptName = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicateConcept);

                List<CiaRiskSuretyGuarantee> guarantees = facade.GetConcept<List<CiaRiskSuretyGuarantee>>(CompanyRuleConceptRisk.Guarantees);

                if (entityConcept != null && guarantees != null && guarantees.Any(x => x.InsuredGuarantee.IsCloseInd == false))
                {
                    List<int> guaranteesName = new List<int>();
                    int valor = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConcept.ConceptId, entityConcept.EntityId));
                    

                    TP.Parallel.ForEach(guarantees.AsParallel().Where(x => x.InsuredGuarantee.IsCloseInd = false), z =>
                    // guarantees.Where(x => x.InsuredGuarantee.IsCloseInd == false), z =>
                    {
                        List<Endorsement> listEndorsement = DelegateService.underwritingServiceCore.GetPoliciesByGuaranteeId(z.InsuredGuarantee.Id);
                        if (listEndorsement.Count > valor)
                        {
                            guaranteesName.Add(z.InsuredGuarantee.Id);
                        }
                    });
                    //foreach (CiaRiskSuretyGuarantee guarantee in guarantees.Where(x => x.InsuredGuarantee.IsCloseInd == false))
                    //{
                    //    List<Endorsement> listEndorsement = DelegateService.underwritingServiceCore.GetPoliciesByGuaranteeId(guarantee.InsuredGuarantee.Id);

                    //    if (listEndorsement.Count > valor)
                    //    {
                    //        guaranteesName.Add(guarantee.InsuredGuarantee.Id);
                    //    }
                    //}

                    if (guaranteesName.Count > 0)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConceptName.ConceptId, entityConceptName.EntityId), string.Join(",", guaranteesName));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Resources.Errors.ErrorExecuteFunction, "OpenGuarantee"), ex);
            }
        }

        public void ValidateStatusGuarantee(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicateConcept = (str) => str == null ? false : str.ConceptName == "Nombre de las contragarantias" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConceptName = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicateConcept);

                Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Estado a validar" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConceptStatus = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                Func<SCREN.Concept, bool> entityPredicateDays = (str) => str == null ? false : str.ConceptName == "Numero de dias (Contragarantias)" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConceptDays = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicateDays);


                List<int> guaranteesName = new List<int>();

                if (entityConceptStatus != null && entityConceptDays != null)
                {
                    int status = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConceptStatus.ConceptId, entityConceptStatus.EntityId));
                    int guaranteeDays = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConceptDays.ConceptId, entityConceptDays.EntityId));

                    if (status > 0 && guaranteeDays > 0)
                    {
                        List<CiaRiskSuretyGuarantee> guarantees = facade.GetConcept<List<CiaRiskSuretyGuarantee>>(CompanyRuleConceptRisk.Guarantees);

                        if (guarantees != null && guarantees.Count > 0)
                        {
                            TP.Parallel.ForEach(guarantees.AsParallel().Where(x => x.InsuredGuarantee.InsuredGuaranteeLog != null), z =>
                                 TP.Parallel.ForEach(z.InsuredGuarantee.InsuredGuaranteeLog.AsParallel().
                                 Where(g => g.GuaranteeStatusCode == status)
                                 .Where(i => ((DateTime.Now - i.LogDate).Days) > guaranteeDays), h =>
                                           guaranteesName.AddRange(guarantees.Select(x => x.Id))
                                 ));

                            //foreach (CiaRiskSuretyGuarantee guarantee in guarantees)
                            //{
                            //if (guarantee.InsuredGuarantee.InsuredGuaranteeLog != null)
                            // {
                            //foreach (InsuredGuaranteeLog guaranteeLog in guarantee.InsuredGuarantee.InsuredGuaranteeLog)
                            //        {
                            //            int calculateDays = (DateTime.Now - guaranteeLog.LogDate).Days;
                            //            if (guaranteeLog.GuaranteeStatusCode == status && calculateDays > guaranteeDays)
                            //            {
                            //                guaranteesName.AddRange(guarantees.Select(x => x.Id));
                            //            }
                            //        }
                            //    }
                            //}

                            if (guaranteesName.Count > 0)
                            {
                                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                                facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConceptName.ConceptId, entityConceptName.EntityId), string.Join(",", guaranteesName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Resources.Errors.ErrorExecuteFunction, "ValidateStatusGuarantee"), ex); ;
            }
        }


        public void ValidateGuaranteeValue(Rules.Facade facade)
        {
            try
            {
                List<CiaRiskSuretyGuarantee> guarantees = facade.GetConcept<List<CiaRiskSuretyGuarantee>>(RuleConceptRisk.Guarantees);
                Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Numero de polizas" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                Func<SCREN.Concept, bool> entityPredicateConcept = (str) => str == null ? false : str.ConceptName == "Nombre de las contragarantias" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_RISK).ToString());
                SCREN.Concept entityConceptName = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicateConcept);

                if (guarantees != null && guarantees.Count > 0)
                {
                    int individualId = facade.GetConcept<int>(RuleConceptRisk.IndividualId);
                    var guaranteesList = DelegateService.underwritingServiceCore.GetInsuredGuaranteesByIndividualId(individualId);
                    string guaranteesName = "";
                    //decimal? valueGuarantee = 0;
                    decimal? totalValueGuarantees = 0;
                    int policyCount = 0;

                    //foreach (var guaranteesDocumentNumber in guaranteesList)
                    //{
                    //    foreach (var guaranteIndividual in guarantees)
                    //    {
                    //        List<Endorsement> listEndorsement = DelegateService.underwritingServiceCore.GetPoliciesByGuaranteeId(guaranteIndividual.InsuredGuarantee.Id);
                    //        policyCount = listEndorsement.Count;
                    //        if (guaranteesDocumentNumber.InsuredGuarantee.Id == guaranteIndividual.InsuredGuarantee.Id)
                    //        {
                    //            valueGuarantee = guaranteesDocumentNumber.InsuredGuarantee.DocumentValueAmount;
                    //            totalValueGuarantees = valueGuarantee + totalValueGuarantees;
                    //            guaranteesName = guaranteesDocumentNumber.InsuredGuarantee.Id + "-" + guaranteesDocumentNumber.Description + "(" + guaranteesDocumentNumber.InsuredGuarantee.GuaranteeStatus.Description + ") " + guaranteesName;
                    //        }
                    //    }
                    //}


                    List<int> guaranteesIds = guarantees.Select(g => g.InsuredGuarantee.Id).ToList();
                    List<IssuanceGuarantee> guaranteesToProcess = guaranteesList.Where(g => guaranteesIds.Contains(g.InsuredGuarantee.Id)).ToList();

                    List<Endorsement> listEndorsement = DelegateService.underwritingServiceCore.GetPoliciesByGuaranteeId(guaranteesToProcess.Select(g => g.InsuredGuarantee.Id).FirstOrDefault());
                    totalValueGuarantees = guaranteesToProcess.Sum(g => g.InsuredGuarantee.DocumentValueAmount);
                    guaranteesName = string.Join(", ", guaranteesToProcess.Select(g => $"{g.InsuredGuarantee.Id}-{g.Description}({g.InsuredGuarantee.GuaranteeStatus.Description})"));
                    policyCount = listEndorsement.Count;

                    decimal value = facade.GetConcept<decimal>(RuleConceptRisk.AmountInsured);

                    if (value > 0 && value > totalValueGuarantees)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConceptName.ConceptId, entityConceptName.EntityId), string.Join(",", guaranteesName));
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(entityConcept.ConceptId, entityConcept.EntityId), policyCount);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Resources.Errors.ErrorExecuteFunction, "ValidateGuaranteeValue"), ex);
            }
        }

        public void ValidatePolicyGuarantees(Rules.Facade facade)
        {
            try
            {
                int riskId = facade.GetConcept<int>(RuleConceptEstimation.RiskId);

                Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Estado a validar" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_ESTIMATION).ToString());
                SCREN.Concept entityConceptStatus = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityConceptStatus != null)
                {
                    int status = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConceptStatus.ConceptId, entityConceptStatus.EntityId));

                    CiaSuretiesEEProvider ciaSuretiesEEProvider = new CiaSuretiesEEProvider();
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, !ciaSuretiesEEProvider.GetRiskSuretyGuaranteesByRiskId(riskId).Any(x => x.GuaranteeStatus.Code == status));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Resources.Errors.ErrorExecuteFunction, "ValidatePolicyGuarantees"), ex);
            }
        }

        public void ValidatePaymentPolicyGuarantees(Rules.Facade facade)
        {
            try
            {
                int riskId = facade.GetConcept<int>(RuleConceptPaymentRequest.RiskId);

                Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Estado a validar" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_PAYMENT_REQUEST).ToString());
                SCREN.Concept entityConceptStatus = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityConceptStatus != null)
                {
                    int status = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConceptStatus.ConceptId, entityConceptStatus.EntityId));

                    CiaSuretiesEEProvider ciaSuretiesEEProvider = new CiaSuretiesEEProvider();
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, !ciaSuretiesEEProvider.GetRiskSuretyGuaranteesByRiskId(riskId).Any(x => x.GuaranteeStatus.Code == status));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Resources.Errors.ErrorExecuteFunction, "ValidatePaymentPolicyGuarantees"), ex);
            }
        }
    }
}
