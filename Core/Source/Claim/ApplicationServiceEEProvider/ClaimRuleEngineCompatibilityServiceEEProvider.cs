using System;
using System.Collections.Generic;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using SCREN = Sistran.Core.Application.Script.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider
{
    public class ClaimRuleEngineCompatibilityServiceEEProvider
    {
        public void ValidatePolicyReinsurance(Rules.Facade facade)
        {
            int endorsementId = Convert.ToInt32(facade.GetConcept<string>(RuleConceptClaim.EndorsementId));

            facade.SetConcept(RuleConceptPolicies.GenerateEvent, !DelegateService.claimsReinsuranceWorkerIntegrationServices.ValidateEndorsementReinsurance(endorsementId));
        }

        public void ValidateClaimIndividualListRisk(Rules.Facade facade)
        {
            Func<SCREN.Concept, bool> entityPredicate;
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_ESTIMATION).ToString());

            entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityRiskList == null || entityTypeRiskList == null)
            { return; }

            string documentNumber = facade.GetConcept<string>(RuleConceptEstimation.AffectedDocumentNumber);
            string fullName = facade.GetConcept<string>(RuleConceptEstimation.AffectedName);


            if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName))
            { return; }

            int riskListType = facade.GetConcept<int>(RuleConceptEstimation.DynamicConcept(entityTypeRiskList.ConceptId));
            List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
            if (riskList.Count > 0)
            {
                List<string> lists = new List<string>();
                riskList.ForEach(x => lists.Add(x.listType));

                var listRisk = String.Join(",", lists);

                KeyValuePair<int, int> key = new KeyValuePair<int, int>(key: entityRiskList.ConceptId, value: entityRiskList.EntityId);
                facade.SetConcept(key, listRisk);
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }

        public void ValidatePaymentIndividualListRisk(Rules.Facade facade)
        {
            Func<SCREN.Concept, bool> entityPredicate;
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_PAYMENT_REQUEST).ToString());

            entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityRiskList == null || entityTypeRiskList == null)
            { return; }

            string documentNumber = facade.GetConcept<string>(RuleConceptPaymentRequest.PaymentIndividualDocumentNumber);
            string fullName = facade.GetConcept<string>(RuleConceptPaymentRequest.PaymentIndividualName);

            if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName))
            { return; }

            int riskListType = facade.GetConcept<int>(RuleConceptPaymentRequest.DynamicConcept(entityTypeRiskList.ConceptId));
            List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
            if (riskList.Count > 0)
            {
                List<string> lists = new List<string>();
                riskList.ForEach(x => lists.Add(x.listType));

                var listRisk = String.Join(",", lists);

                KeyValuePair<int, int> key = new KeyValuePair<int, int>(key: entityRiskList.ConceptId, value: entityRiskList.EntityId);
                facade.SetConcept(key, listRisk);
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }

        public void ValidateChargeIndividualListRisk(Rules.Facade facade)
        {
            Func<SCREN.Concept, bool> entityPredicate;
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_CHARGE_REQUEST).ToString());

            entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityRiskList == null || entityTypeRiskList == null)
            { return; }

            string documentNumber = facade.GetConcept<string>(RuleConceptChargeRequest.ChargeIndividualDocumentNumber);
            string fullName = facade.GetConcept<string>(RuleConceptChargeRequest.ChargeIndividualName);

            if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName))
            { return; }

            int riskListType = facade.GetConcept<int>(RuleConceptChargeRequest.DynamicConcept(entityTypeRiskList.ConceptId));
            List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
            if (riskList.Count > 0)
            {
                List<string> lists = new List<string>();
                riskList.ForEach(x => lists.Add(x.listType));

                var listRisk = String.Join(",", lists);

                KeyValuePair<int, int> key = new KeyValuePair<int, int>(key: entityRiskList.ConceptId, value: entityRiskList.EntityId);
                facade.SetConcept(key, listRisk);
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }

        public void SinisterPolicy(Rules.Facade facade)
        {
            int policyId = facade.GetConcept<int>(RuleConceptGeneral.PolicyId);
            if (policyId != 0)
            {
                ClaimDAO claimDAO = new ClaimDAO();
                List<Claim> listClaim = claimDAO.GetClaimsByPolicyId(policyId);
                NoticeDAO noticeDAO = new NoticeDAO();
                List<Notice> claimNotice = noticeDAO.GetNoticesByPolicyId(policyId);

                if (claimNotice.Count > 0 || listClaim.Count > 0)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }


            }

        }

        public void MainInsuredWithClaims(Rules.Facade facade)
        {
            Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "IndividualId (Validar siniestro)" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
            SCREN.Concept entityConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityConcept != null)
            {
                int individualId = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConcept.ConceptId, entityConcept.EntityId));

                if (individualId != 0)
                {
                    ClaimDAO claimDAO = new ClaimDAO();
                    List<Claim> listClaim = claimDAO.GetClaimsByIndividualId(individualId);
                    NoticeDAO noticeDAO = new NoticeDAO();
                    List<Notice> claimNotice = noticeDAO.GetNoticesByIndividualId(individualId);

                    if (claimNotice.Count > 0 || listClaim.Count > 0)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
            }

        }

        public void ConsortiumMemberWithClaims(Rules.Facade facade)
        {

            Func<SCREN.Concept, bool> entityPredicate = (str) => str == null ? false : str.ConceptName == "Integrantes del consorcio con siniestros" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
            SCREN.Concept entityConcept = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityConcept != null)
            {
                Func<SCREN.Concept, bool> entityPredicateConcept = (str) => str == null ? false : str.ConceptName == "IndividualId (Validar siniestro)" && str.EntityId == int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
                SCREN.Concept entityConceptInd = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicateConcept);

                if (entityConceptInd != null)
                {
                    int individualId = facade.GetConcept<int>(new KeyValuePair<int, int>(entityConceptInd.ConceptId, entityConceptInd.EntityId));

                    if (individualId != 0)
                    {

                        ClaimDAO claimDAO = new ClaimDAO();
                        NoticeDAO noticeDAO = new NoticeDAO();


                        Insured insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(individualId);
                        if (insured != null)
                        {
                            List<Consortium> consortiums = DelegateService.uniquePersonServiceCore.GetConsortiumByInsurendId(insured.InsuredCode);
                            List<string> consortiumNames = new List<string>();
                            if (consortiums != null)
                            {
                                foreach (UniquePersonService.V1.Models.Consortium consortium in consortiums)
                                {
                                    List<Claim> listClaim = claimDAO.GetClaimsByIndividualId(consortium.IndividualId);
                                    List<Notice> claimNotice = noticeDAO.GetNoticesByIndividualId(consortium.IndividualId);
                                    if (claimNotice.Count > 0 || listClaim.Count > 0)
                                    {
                                        consortiumNames.Add(consortium.FullName);
                                    }
                                }

                                if (consortiumNames.Count > 0)
                                {
                                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                                    facade.SetConcept(new KeyValuePair<int, int>(entityConcept.ConceptId, entityConcept.EntityId), string.Join(",", consortiumNames));
                                }
                            }
                        }

                    }
                }

            }

        }
    }
}
