using SCREN = Sistran.Core.Application.Script.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Integration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.Assemblers;
using Sistran.Core.Application.Utilities.Enums;
using System.Configuration;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleBridgeDelegate : RulesBridge
    {
        public override Vocabulary GetVocabulary()
        {
            Vocabulary vocabulary = new Vocabulary("Vocabulary");

            foreach (Rules.Concept concept in GetConcepts())
            {
                vocabulary.AddConcept(concept);
            }

            return vocabulary;
        }

        public override Rules.Concept GetConceptByName(string name, string paramFacade)
        {
            List<Rules.Concept> concepts = (List<Rules.Concept>)InProcCache.Instance.GetCurrentDic(RulesConstant.concept, RulesConstant.concepts);

            if (concepts == null)
            {
                concepts = GetConcepts();
            }

            int entityId = GetEntityIdByName(paramFacade.Substring(3));

            return concepts.First(x => x.Name == name && x.EntityId == entityId);
        }

        public override Rules.Concept GetConceptByConceptId(int conceptId, string paramFacade)
        {
            List<Rules.Concept> concepts = (List<Rules.Concept>)InProcCache.Instance.GetCurrentDic(RulesConstant.concept, RulesConstant.concepts);

            if (concepts == null)
            {
                concepts = GetConcepts();
            }

            int entityId = GetEntityIdByName(paramFacade.Substring(3));

            return concepts.First(x => x.Id == conceptId && x.EntityId == entityId);
        }

        protected override Rules.Integration.RuleSet FindRuleSet(int ruleSetId)
        {
            SCREN.RuleSet entityRuleSet = null;
            entityRuleSet = (SCREN.RuleSet)InProcCache.Instance.GetCurrentDic(RulesConstant.ruleSet, ruleSetId.ToString());
            if (entityRuleSet == null)
            {
                entityRuleSet = RuleEngineDAO.GetRuleSetByRuleSetId(ruleSetId);
                InProcCache.Instance.InsertCurrentDic(RulesConstant.ruleSet, ruleSetId.ToString(), entityRuleSet);
                if (!entityRuleSet.Active)
                {
                    entityRuleSet.Active = true;
                    entityRuleSet.TypeActive = (int)ActiveRuleSetType.system;
                    entityRuleSet = RuleEngineDAO.UpdateRuleSetByRuleSet(entityRuleSet);
                }
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(new MemoryStream(entityRuleSet.RuleSetXml));

            Rules.Integration.RuleSet ruleSet = (new XmlRuleSetReader(xmlDocument)).Read();
            ruleSet.Name = entityRuleSet.Description;
            ruleSet.LevelId = entityRuleSet.LevelId;
            ruleSet.PackageId = entityRuleSet.PackageId;
            ruleSet.Id = entityRuleSet.RuleSetId;
            ruleSet.Version = entityRuleSet.RuleSetVer;

            if (bool.TryParse(ConfigurationManager.AppSettings["RuleLog"], out bool log))
            {
                if (log)
                {
                    RuleLog ruleLog = new RuleLog(ruleSet);
                    ruleSet = ruleLog.SetLog();
                }
            }

            return ruleSet;
        }

        private List<Rules.Concept> GetConcepts()
        {
            List<Rules.Concept> concepts = (List<Rules.Concept>)InProcCache.Instance.GetCurrentDic(RulesConstant.concept, RulesConstant.concepts);

            if (concepts == null)
            {
                List<SCREN.Concept> entityconcepts = GetEntityConcepts();
                concepts = ModelAssembler.CreatConcepts(entityconcepts);
                InProcCache.Instance.InsertCurrentDic(RulesConstant.concept, RulesConstant.concepts, concepts);
            }

            return concepts;
        }
        protected List<SCREN.Concept> GetEntityConcepts()
        {
            List<SCREN.Concept> entityConcepts = (List<SCREN.Concept>)InProcCache.Instance.GetCurrentList<SCREN.Concept>(RulesConstant.Entityconcept);
            if (entityConcepts == null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(SCREN.Concept.Properties.EntityId).In().ListValue();
                foreach (string facadeName in Enum.GetNames(typeof(Enums.FacadeType)))
                {
                    Enums.FacadeType facadeTypes = (Enums.FacadeType)Enum.Parse(typeof(Enums.FacadeType), facadeName);
                    int facadeId = int.Parse(EnumHelper.GetEnumParameterValue(facadeTypes).ToString());
                    filter.Constant(facadeId);
                }
                filter.EndList();
                entityConcepts = DataFacadeManager.Instance.GetDataFacade().List(typeof(SCREN.Concept), filter.GetPredicate()).Cast<SCREN.Concept>().ToList();
                if (entityConcepts != null)
                {
                    InProcCache.StateCache.TryAdd(RulesConstant.Entityconcept, entityConcepts);
                }
            }

            return entityConcepts;
        }

        protected Rules.Concept CreateConcept(SCREN.Concept entityConcept)
        {
            System.Type type = GetConceptType(entityConcept);

            if (type.IsValueType && (entityConcept.IsNullable || !entityConcept.IsStatic))
            {
                type = typeof(Nullable<>).MakeGenericType(type);
            }

            return new Concept(entityConcept.ConceptId, entityConcept.ConceptName, type, entityConcept.IsStatic, entityConcept.EntityId);
        }

        public System.Type GetConceptType(SCREN.Concept entityConcept)
        {
            switch (entityConcept.ConceptTypeCode)
            {
                case 0:
                    throw new ValidationException("No existe el tipo de dato del concepto " + entityConcept.Description);
                case 1:
                    return GetBasicType(entityConcept);
                case 2:
                    return typeof(int);
                case 3:
                    return GetListType(entityConcept);
                case 4:
                    return typeof(int);
                default:
                    throw new ValidationException("No existe el tipo de dato del concepto " + entityConcept.Description);
            }
        }

        protected System.Type GetBasicType(SCREN.Concept entityConcept)
        {
            System.Type type = (System.Type)InProcCache.Instance.GetCurrentDic(RulesConstant.basicConcept, entityConcept.ConceptName + entityConcept.EntityId.ToString());

            if (type == null)
            {
                Func<SCREN.BasicConcept, bool> predicate = (str) => str == null ? false : str.ConceptId == entityConcept.ConceptId && str.EntityId == entityConcept.EntityId;
                SCREN.BasicConcept entityBasicConcept = null;
                entityBasicConcept = (SCREN.BasicConcept)InProcCache.Instance.GetCurrent<SCREN.BasicConcept>(RulesConstant.entityBasicConcept, predicate);
                if (entityBasicConcept == null)
                {
                    entityBasicConcept = new SCREN.BasicConcept(entityConcept.ConceptId, entityConcept.EntityId);
                    entityBasicConcept = (SCREN.BasicConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(entityBasicConcept.PrimaryKey);
                    if (entityBasicConcept == null)
                    {
                        InProcCache.Instance.InsertCurrent<SCREN.BasicConcept>(RulesConstant.entityBasicConcept, entityBasicConcept);
                    }
                }

                if (entityBasicConcept != null)
                {
                    switch (entityBasicConcept.BasicTypeCode)
                    {
                        case 1:
                            type = typeof(int);
                            break;
                        case 2:
                            type = typeof(string);
                            break;
                        case 3:
                            type = typeof(decimal);
                            break;
                        case 4:
                            type = typeof(DateTime);
                            break;
                        case 5:
                            type = typeof(long);
                            break;
                        default:
                            throw new ValidationException("No existe el tipo de dato del concepto " + entityConcept.ConceptName);
                    }
                }
                else
                {
                    throw new ValidationException("No existe el tipo de dato del concepto " + entityConcept.ConceptName);
                }

                InProcCache.Instance.InsertCurrentDic(RulesConstant.basicConcept, entityConcept.ConceptName + entityConcept.EntityId.ToString(), type);
            }

            return type;
        }

        protected System.Type GetListType(SCREN.Concept entityConcept)
        {
            System.Type type = (System.Type)InProcCache.Instance.GetCurrentDic(RulesConstant.listConcept, entityConcept.ConceptName + entityConcept.EntityId.ToString());

            if (type == null)
            {
                Func<SCREN.ListConcept, bool> predicate = (str) => str == null ? false : str.ConceptId == entityConcept.ConceptId && str.EntityId == entityConcept.EntityId;
                SCREN.ListConcept entityListConcept = null;
                entityListConcept = (SCREN.ListConcept)InProcCache.Instance.GetCurrent<SCREN.ListConcept>(RulesConstant.entityListConcept, predicate);
                if (entityListConcept == null)
                {
                    List<SCREN.ListConcept> listConcept = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        listConcept = daf.List(typeof(SCREN.ListConcept), null).Cast<SCREN.ListConcept>().ToList();
                    }
                    if (listConcept != null)
                    {
                        InProcCache.StateCache.TryAdd(RulesConstant.entityListConcept, listConcept);
                    }
                    entityListConcept = listConcept.FirstOrDefault(x => x.ConceptId == entityConcept.ConceptId && x.EntityId == entityConcept.EntityId);
                }

                if (entityListConcept != null)
                {
                    if (entityListConcept.ListEntityCode == 2)
                    {
                        type = typeof(bool);
                    }
                    else
                    {
                        type = typeof(int);
                    }
                }
                else
                {
                    throw new ValidationException("No existe el tipo de dato del concepto " + entityConcept.ConceptName);

                }
                InProcCache.Instance.InsertCurrentDic(RulesConstant.listConcept, entityConcept.ConceptName + entityConcept.EntityId.ToString(), type);
            }
            return type;
        }

        private int GetEntityIdByName(string entityName)
        {
            int? entityId = (int?)InProcCache.Instance.GetCurrentDic(RulesConstant.entityId, entityName);

            if (entityId == null)
            {
                entityId = RuleEngineDAO.GetEntityIdByEntityName(entityName);
                InProcCache.Instance.InsertCurrentDic(RulesConstant.entityId, entityName, entityId);
            }

            return entityId.Value;
        }
    }
}