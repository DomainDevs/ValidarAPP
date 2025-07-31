// -----------------------------------------------------------------------
// <copyright file="UniquePersonRuleEngineCompatibilityServiceEEProvider.cs" company="Sistran">
// Copyright (c). All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.SarlaftApplicationServicesProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Application.Utilities.Cache;
    using Core.Application.Utilities.Enums;
    using Core.Application.Utilities.Helper;
    using Core.Application.Utilities.RulesEngine;
    using Core.Framework.BAF;
    using Sistran.Company.Application.SarlaftApplicationServices.DTO;
    using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
    using Rules = Core.Framework.Rules;
    using SCREN = Core.Application.Script.Entities;

    /// <summary>
    /// Clase que especifica las funciones de Reglas
    /// </summary>
    public class SarlaftRuleEngineCompatibilityServiceEEProvider
    {
        #region Politicas

        public void ValidatePersonRiskList(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate;
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_SARLAFT).ToString());

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);


                if (entityRiskList == null || entityTypeRiskList == null)
                { return; }

                string documentNumber = facade.GetConcept<string>(RuleConceptGeneralSarlaft.DocumentNumberSarlaft);
                string fullName = facade.GetConcept<string>(RuleConceptGeneralSarlaft.NamesBusinessNameSarlaft);
                int documentType = facade.GetConcept<int>(RuleConceptGeneralSarlaft.DocumentTypeSarlaft);

                if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName) || documentType == 0)
                { return; }

                if (documentType == 2 && documentNumber.Length > 9)
                {
                    documentNumber = documentNumber.Substring(0, 9);
                }

                int riskListType = facade.GetConcept<int>(RuleConceptGeneralSarlaft.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralSarlaft.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateClintonList", ex);
            }
        }


        public void ValidatePartnerRiskList(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate;
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_SARLAFT).ToString());

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityRiskList == null || entityTypeRiskList == null)
                { return; }

                string documentNumber = facade.GetConcept<string>(RuleConceptPartners.DocumentNumberShareholders);
                string fullName = facade.GetConcept<string>(RuleConceptPartners.NameShareholders);
                int documentType = facade.GetConcept<int>(RuleConceptPartners.DocumentTypePartner);

                if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName) || documentType == 0)
                { return; }

                if (documentType == 2 && documentNumber.Length > 9)
                {
                    documentNumber = documentNumber.Substring(0, 9);
                }

                int riskListType = facade.GetConcept<int>(RuleConceptGeneralSarlaft.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralSarlaft.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateClintonList", ex);
            }
        }

        public void ValidateLegalRepresentativeRiskList(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate;
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_SARLAFT).ToString());

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityRiskList == null || entityTypeRiskList == null)
                { return; }

                string documentNumber = facade.GetConcept<string>(RuleConceptLegalRepresentative.DocumentNumberLegal);
                string fullName = facade.GetConcept<string>(RuleConceptLegalRepresentative.NameLegal);
                int documentType = facade.GetConcept<int>(RuleConceptLegalRepresentative.DocumentTypeLegal);

                if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName) || documentType == 0)
                { return; }

                if (documentType == 2 && documentNumber.Length > 9)
                {
                    documentNumber = documentNumber.Substring(0, 9);
                }

                int riskListType = facade.GetConcept<int>(RuleConceptGeneralSarlaft.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralSarlaft.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateClintonList", ex);
            }
        }

        public void ValidateBeneficiaryRiskList(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate;
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_SARLAFT).ToString());

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityRiskList == null || entityTypeRiskList == null)
                { return; }

                string documentNumber = facade.GetConcept<string>(RuleConceptBeneficiaries.DocumentNumberBeneficiary);
                string fullName = facade.GetConcept<string>(RuleConceptBeneficiaries.NameBeneficiary);
                int documentType = facade.GetConcept<int>(RuleConceptBeneficiaries.DocumentTypeBeneficiary);

                if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName) || documentType == 0)
                { return; }

                if (documentType == 2 && documentNumber.Length > 9)
                {
                    documentNumber = documentNumber.Substring(0, 9);
                }

                int riskListType = facade.GetConcept<int>(RuleConceptGeneralSarlaft.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralSarlaft.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateBeneficiaryRiskList", ex);
            }
        }

        public void ValidatePaymentIndividualSarlaft(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_PAYMENT_REQUEST).ToString());

                int individualId = Convert.ToInt32(facade.GetConcept<string>(RuleConceptPaymentRequest.PaymentIndividualId));

                if (individualId == 0)
                { return; }

                List<SarlaftExonerationtDTO> exoneration = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(individualId);
                if (exoneration.Count > 0 && exoneration[0].IsExonerated)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, false);
                    return;
                }

                List<SarlaftDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaft(individualId);
                if (result.Count > 0)
                {
                    SarlaftDTO objSarlaft = result.OrderByDescending(x => x.FillingDate).FirstOrDefault();
                    if (objSarlaft == null)
                    { return; }

                    DateTime fillingDate = (DateTime)objSarlaft.FillingDate;
                    if (DateTime.Now.Subtract(fillingDate).Days > 365)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                    else
                    {
                        int parameter = DelegateService.commonService.GetParameterByDescription("Vencimiento Previo Sarlaft").NumberParameter.GetValueOrDefault(0);

                        if (DateTime.Now.Subtract(fillingDate).Days > 365 - parameter)
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                        else
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, objSarlaft.PendingEvent);
                        }
                    }
                }
                else
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateSarlaft", ex);
            }

        }

        public void ValidateChargeIndividualSarlaft(Rules.Facade facade)
        {
            try
            {
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_CHARGE_REQUEST).ToString());

                int individualId = Convert.ToInt32(facade.GetConcept<string>(RuleConceptChargeRequest.ChargeIndividualId));

                if (individualId == 0)
                { return; }

                List<SarlaftExonerationtDTO> exoneration = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(individualId);
                if (exoneration.Count > 0 && exoneration[0].IsExonerated)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, false);
                    return;
                }

                List<SarlaftDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaft(individualId);
                if (result.Count > 0)
                {
                    SarlaftDTO objSarlaft = result.OrderByDescending(x => x.FillingDate).FirstOrDefault();
                    if (objSarlaft == null)
                    { return; }

                    DateTime fillingDate = (DateTime)objSarlaft.FillingDate;
                    if (DateTime.Now.Subtract(fillingDate).Days > 365)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                    else
                    {
                        int parameter = DelegateService.commonService.GetParameterByDescription("Vencimiento Previo Sarlaft").NumberParameter.GetValueOrDefault(0);

                        if (DateTime.Now.Subtract(fillingDate).Days > 365 - parameter)
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                        else
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, objSarlaft.PendingEvent);
                        }
                    }
                }
                else
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateSarlaft", ex);
            }

        }

        public void ValidateEstimationIndividualSarlaft(Rules.Facade facade)
        {
            try
            {
                int individualId = Convert.ToInt32(facade.GetConcept<string>(RuleConceptEstimation.AffectedId));

                if (individualId == 0)
                { return; }

                List<SarlaftExonerationtDTO> exoneration = DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(individualId);
                if (exoneration.Count > 0 && exoneration[0].IsExonerated)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, false);
                    return;
                }

                List<SarlaftDTO> result = DelegateService.SarlaftApplicationServices.GetSarlaft(individualId);
                if (result.Count > 0)
                {
                    SarlaftDTO objSarlaft = result.OrderByDescending(x => x.FillingDate).FirstOrDefault();
                    if (objSarlaft == null)
                    { return; }

                    DateTime fillingDate = (DateTime)objSarlaft.FillingDate;
                    if (DateTime.Now.Subtract(fillingDate).Days > 365)
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                    else
                    {
                        int parameter = DelegateService.commonService.GetParameterByDescription("Vencimiento Previo Sarlaft").NumberParameter.GetValueOrDefault(0);

                        if (DateTime.Now.Subtract(fillingDate).Days > 365 - parameter)
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        }
                        else
                        {
                            facade.SetConcept(RuleConceptPolicies.GenerateEvent, objSarlaft.PendingEvent);
                        }
                    }
                }
                else
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateSarlaft", ex);
            }

        }
        #endregion
    }
}