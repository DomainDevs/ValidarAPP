// -----------------------------------------------------------------------
// <copyright file="UniquePersonRuleEngineCompatibilityServiceEEProvider.cs" company="Sistran">
// Copyright (c). All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonAplicationServices.EEProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Application.Utilities.Cache;
    using Core.Application.Utilities.Enums;
    using Core.Application.Utilities.Helper;
    using Core.Application.Utilities.RulesEngine;
    using Core.Framework.BAF;
    using SarlaftApplicationServices.DTO;
    using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
    using UniquePersonServices.EEProvider;
    using Rules = Core.Framework.Rules;
    using SCREN = Core.Application.Script.Entities;

    /// <summary>
    /// Clase que especifica las funciones de Reglas
    /// </summary>
    public class UniquePersonRuleEngineCompatibilityServiceEEProvider
    {
        #region Politicas
        public void ValidatePersonRiskList(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate;
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_PERSON).ToString());

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityRiskList == null || entityTypeRiskList == null)
                { return; }

                string fullName = string.Empty;
                string documentNumber = facade.GetConcept<string>(RuleConceptGeneralPerson.DocumentNumber);
                int documentType = facade.GetConcept<int>(RuleConceptGeneralPerson.DocumentType);

                if (facade.GetConcept<int>(RuleConceptGeneralPerson.IndividualType) == (int)Core.Services.UtilitiesServices.Enums.IndividualType.Company)
                {
                    fullName = facade.GetConcept<string>(RuleConceptGeneralPerson.BusinessName);
                }
                if (facade.GetConcept<int>(RuleConceptGeneralPerson.IndividualType) == (int)Core.Services.UtilitiesServices.Enums.IndividualType.Person)
                {
                    fullName += facade.GetConcept<string>(RuleConceptGeneralPerson.Surname) + " " ?? string.Empty;
                    fullName += facade.GetConcept<string>(RuleConceptGeneralPerson.SecondSurname) + " " ?? string.Empty;
                    fullName += facade.GetConcept<string>(RuleConceptGeneralPerson.Names) + " " ?? string.Empty;
                }

                if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName) || documentType == 0)
                { return; }

                int riskListType = facade.GetConcept<int>(RuleConceptGeneralPerson.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralPerson.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x=>x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidatePersonRiskList", ex);
            }
        }


        public void ValidateUniquePersonBasicInfoRiskList(Rules.Facade facade)
        {
            try
            {
                Func<SCREN.Concept, bool> entityPredicate;
                int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_BASIC_INFO).ToString());

                entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
                SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                if (entityRiskList == null || entityTypeRiskList == null)
                { return; }

                string fullName = string.Empty;
                string documentNumber = facade.GetConcept<string>(RuleConceptGeneralBasicInfo.DocumentNumber);
                int documentType = facade.GetConcept<int>(RuleConceptGeneralBasicInfo.DocumentType);

                if (facade.GetConcept<int>(RuleConceptGeneralBasicInfo.IndividualType) == (int)Core.Services.UtilitiesServices.Enums.IndividualType.Company)
                {
                    fullName = facade.GetConcept<string>(RuleConceptGeneralBasicInfo.BusinessName);
                }
                if (facade.GetConcept<int>(RuleConceptGeneralBasicInfo.IndividualType) == (int)Core.Services.UtilitiesServices.Enums.IndividualType.Person)
                {
                    fullName += facade.GetConcept<string>(RuleConceptGeneralBasicInfo.Surname) + " " ?? string.Empty;
                    fullName += facade.GetConcept<string>(RuleConceptGeneralBasicInfo.SecondSurname) + " " ?? string.Empty;
                    fullName += facade.GetConcept<string>(RuleConceptGeneralBasicInfo.Names) + " " ?? string.Empty;
                }

                if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName) || documentType == 0)
                { return; }

                int riskListType = facade.GetConcept<int>(RuleConceptGeneralBasicInfo.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(documentNumber, fullName, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralBasicInfo.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en la Funcion de reglas: ValidateUniquePersonBasicInfoRiskList", ex);
            }
        }


        public void ValidateSarlaft(Rules.Facade facade)
        {
            try
            {
                int individualId = facade.GetConcept<int>(RuleConceptGeneralPerson.IndividualId);

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

        public void ValidateUniquePersonBasicInfoSarlaft(Rules.Facade facade)
        {
            try
            {
                int individualId = facade.GetConcept<int>(RuleConceptGeneralBasicInfo.IndividualId);

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
                throw new BusinessException("Error en la Funcion de reglas: ValidateUniquePersonBasicInfoSarlaft", ex);
            }
        }

        public void ValidateListRiskConsortium (Rules.Facade facade)
        {
            Func<SCREN.Concept, bool> entityPredicate;
            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL_PERSON).ToString());

            entityPredicate = concept => concept != null && concept.ConceptName == "Listas de riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            entityPredicate = concept => concept != null && concept.ConceptName == "Tipo Lista de Riesgo" && concept.EntityId == entityId;
            SCREN.Concept entityTypeRiskList = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

            if (entityRiskList == null || entityTypeRiskList == null)
            { return; }

            string numberConsortium = facade.GetConcept<string>(RuleConceptConsortiates.DocumentNumberConsortium);
            //int individualId = facade.GetConcept<int>(RuleConceptConsortiates.IndividualIdConsortium);
            string nameConsortium = facade.GetConcept<string>(RuleConceptConsortiates.NameConsortium);

            if (nameConsortium != null && numberConsortium != null)
            {
                int riskListType = facade.GetConcept<int>(RuleConceptGeneralPerson.DynamicConcept(entityTypeRiskList.ConceptId));
                List<ListRiskMatchDTO> riskList = DelegateService.coreUniqueListRiskPersonService.ValidateListRiskPerson(numberConsortium, nameConsortium, riskListType);
                if (riskList.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneralPerson.DynamicConcept(entityRiskList.ConceptId, entityRiskList.EntityId), string.Join(",", riskList.Select(x => x.listType).Distinct()));
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
        }

        public void ValidateSarlaftConsortium(Rules.Facade facade)
        {
            try
            {
                int individualId = facade.GetConcept<int>(RuleConceptConsortiates.IndividualIdConsortium);

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