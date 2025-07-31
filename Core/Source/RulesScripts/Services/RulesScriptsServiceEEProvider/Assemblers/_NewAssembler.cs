using System.Collections.Generic;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using ERules = Sistran.Core.Application.Script.Entities;
using EnParam = Sistran.Core.Application.Parameters.Entities;
using System;
using System.Linq;
using Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers
{
    public static class _EntityAssembler
    {
        #region Concepts
        public static ERules.Concept CreateConcept(MRules._Concept concept)
        {
            return new ERules.Concept(concept.Entity.EntityId)
            {
                Description = concept.Description,
                ConceptControlCode = (int)concept.ConceptControlType,
                IsStatic = concept.IsStatic,
                KeyOrder = concept.KeyOrder,
                ConceptTypeCode = (int)concept.ConceptType,
                ConceptName = concept.ConceptName,
                IsReadOnly = concept.IsReadOnly,
                IsNullable = concept.IsNulleable,
                IsPersistible = concept.IsPersistible,
                IsVisible = concept.IsVisible
            };
        }

        public static ERules.BasicConcept CreateBasicConcept(MRules._BasicConcept concept)
        {
            return new ERules.BasicConcept(concept.ConceptId, concept.Entity.EntityId)
            {
                BasicTypeCode = (int)concept.BasicType
            };
        }

        public static ERules.RangeConcept CreateRangeConcept(MRules._RangeConcept concept)
        {
            return new ERules.RangeConcept(concept.ConceptId, concept.Entity.EntityId)
            {
                RangeEntityCode = concept.RangeEntity.RangeEntityCode
            };
        }

        public static ERules.ListConcept CreateListConcept(MRules._ListConcept concept)
        {
            return new ERules.ListConcept(concept.ConceptId, concept.Entity.EntityId)
            {
                ListEntityCode = concept.ListEntity.ListEntityCode
            };
        }

        public static ERules.ReferenceConcept CreateReferenceConcept(MRules._ReferenceConcept concept)
        {
            return new ERules.ReferenceConcept(concept.ConceptId, concept.Entity.EntityId)
            {
                FentityId = concept.FEntity.EntityId,
            };
        }
        #endregion

        #region RangeEntity
        public static ERules.RangeEntity CreateRangeEntity(MRules._RangeEntity rangeEntity)
        {
            return new ERules.RangeEntity
            {
                Description = rangeEntity.DescriptionRange,
                RangeValueAt = rangeEntity.RangeValueAt
            };
        }

        public static ERules.RangeEntityValue CreateRangeEntityValue(MRules._RangeEntityValue entityValue)
        {
            return new ERules.RangeEntityValue(0, entityValue.RangeValueCode)
            {
                FromValue = entityValue.FromValue,
                ToValue = entityValue.ToValue
            };
        }
        #endregion


        #region ListEntity
        public static ERules.ListEntity CreateListEntity(MRules._ListEntity listEntity)
        {
            return new ERules.ListEntity
            {
                Description = listEntity.DescriptionList,
                ListValueAt = listEntity.ListValueAt
            };
        }

        public static ERules.ListEntityValue CreateListEntityValue(MRules._ListEntityValue entityValue)
        {
            return new ERules.ListEntityValue(entityValue.ListValueCode, 0)
            {
                ListValue = entityValue.ListValue
            };
        }
        #endregion
    }

    public static class _ModelAssembler
    {
        #region RuleSet
        public static List<MRules._RuleSet> CreateListRuleSet(List<ERules.RuleSet> ruleSets)
        {
            List<MRules._RuleSet> result = new List<MRules._RuleSet>();

            foreach (ERules.RuleSet rule in ruleSets)
            {
                result.Add(CreateRuleSet(rule));
            }
            return result;
        }
        public static MRules._RuleSet CreateRuleSet(ERules.RuleSet ruleSets)
        {
            return new MRules._RuleSet
            {
                RuleSetId = ruleSets.RuleSetId,
                Description = ruleSets.Description,
                CurrentFrom = ruleSets.CurrentFrom,
                Level = new MRules._Level { LevelId = ruleSets.LevelId },
                Package = new MRules._Package { PackageId = ruleSets.PackageId },
                RuleSetVer = ruleSets.RuleSetVer,
                IsEvent = ruleSets.IsEvent,
                Rules = new List<MRules._Rule>(),
                Type = Enums.RuleBaseType.Sequence,
                CurrentTo = ruleSets.CurrentTo,
                Active = ruleSets.Active,
                ActiveType = ruleSets.TypeActive == null ? (Utilities.Enums.ActiveRuleSetType?)null : (Utilities.Enums.ActiveRuleSetType)ruleSets.TypeActive
            };
        }
        #endregion

        #region Concepts
        public static List<MRules._Concept> CreateListConcepts(List<ERules.Concept> concepts)
        {
            List<MRules._Concept> result = new List<MRules._Concept>();

            foreach (ERules.Concept concept in concepts)
            {
                result.Add(CreateConcept(concept));
            }
            return result;
        }

        public static MRules._Concept CreateConcept(ERules.Concept concept)
        {
            return new MRules._Concept
            {
                ConceptId = concept.ConceptId,
                Entity = new Entity { EntityId = concept.EntityId },
                Description = concept.Description,
                ConceptName = concept.ConceptName,
                ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                KeyOrder = concept.KeyOrder,
                IsStatic = concept.IsStatic,
                ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                IsReadOnly = concept.IsReadOnly,
                IsVisible = concept.IsVisible,
                IsNulleable = concept.IsNullable,
                IsPersistible = concept.IsPersistible,
                ConceptDependences = new List<MRules._ConceptDependence>()
            };
        }

        public static MRules._BasicConcept CreateBasicConcept(ERules.Concept concept)
        {
            return new MRules._BasicConcept
            {
                ConceptId = concept.ConceptId,
                Entity = new Entity { EntityId = concept.EntityId },
                Description = concept.Description,
                ConceptName = concept.ConceptName,
                ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                KeyOrder = concept.KeyOrder,
                IsStatic = concept.IsStatic,
                ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                IsReadOnly = concept.IsReadOnly,
                IsVisible = concept.IsVisible,
                IsNulleable = concept.IsNullable,
                IsPersistible = concept.IsPersistible,
                ConceptDependences = new List<MRules._ConceptDependence>()
            };
        }

        public static MRules._RangeConcept CreateRangeConcept(ERules.Concept concept)
        {
            return new MRules._RangeConcept
            {
                RangeEntity = new MRules._RangeEntity(),

                ConceptId = concept.ConceptId,
                Entity = new Entity { EntityId = concept.EntityId },
                Description = concept.Description,
                ConceptName = concept.ConceptName,
                ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                KeyOrder = concept.KeyOrder,
                IsStatic = concept.IsStatic,
                ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                IsReadOnly = concept.IsReadOnly,
                IsVisible = concept.IsVisible,
                IsNulleable = concept.IsNullable,
                IsPersistible = concept.IsPersistible,
                ConceptDependences = new List<MRules._ConceptDependence>()
            };
        }


        public static MRules._ListConcept CreateListConcept(ERules.Concept concept)
        {
            return new MRules._ListConcept
            {
                ListEntity = new MRules._ListEntity(),

                ConceptId = concept.ConceptId,
                Entity = new Entity { EntityId = concept.EntityId },
                Description = concept.Description,
                ConceptName = concept.ConceptName,
                ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                KeyOrder = concept.KeyOrder,
                IsStatic = concept.IsStatic,
                ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                IsReadOnly = concept.IsReadOnly,
                IsVisible = concept.IsVisible,
                IsNulleable = concept.IsNullable,
                IsPersistible = concept.IsPersistible,
                ConceptDependences = new List<MRules._ConceptDependence>()
            };
        }



        public static MRules._ReferenceConcept CreateReferenceConcept(ERules.Concept concept)
        {
            return new MRules._ReferenceConcept
            {
                ConceptId = concept.ConceptId,
                Entity = new Entity { EntityId = concept.EntityId },
                Description = concept.Description,
                ConceptName = concept.ConceptName,
                ConceptType = (Enums.ConceptType)concept.ConceptTypeCode,
                KeyOrder = concept.KeyOrder,
                IsStatic = concept.IsStatic,
                ConceptControlType = (Enums.ConceptControlType)concept.ConceptControlCode,
                IsReadOnly = concept.IsReadOnly,
                IsVisible = concept.IsVisible,
                IsNulleable = concept.IsNullable,
                IsPersistible = concept.IsPersistible,
                ConceptDependences = new List<MRules._ConceptDependence>()
            };
        }
        #endregion

        #region Entity

        public static Entity CreateEntity(ERules.Entity entity)
        {
            return new Entity
            {
                EntityId = entity.EntityId,
                Description = entity.Description,
                EntityName = entity.EntityName,
                LevelId = entity.LevelId,
                PackageId = entity.PackageId,
                ConfigFile = entity.ConfigFile,
                PropertySearch = entity.PropertySearch,
                BusinessView = entity.BusinessView
            };
        }


        #endregion

        #region rule function
        public static List<MRules._RuleFunction> CreateListRuleFunctions(List<ERules.RuleFunction> ruleFunctions)
        {
            List<MRules._RuleFunction> result = new List<MRules._RuleFunction>();

            foreach (ERules.RuleFunction ruleFunction in ruleFunctions)
            {
                result.Add(CreateRuleFunction(ruleFunction));
            }

            return result;
        }

        public static MRules._RuleFunction CreateRuleFunction(ERules.RuleFunction ruleFunction)
        {
            return new MRules._RuleFunction
            {
                RuleFunctionId = ruleFunction.RuleFunctionId,
                Description = ruleFunction.Description,
                FunctionName = ruleFunction.FunctionName,
                Level = new MRules._Level { LevelId = ruleFunction.LevelId },
                Package = new MRules._Package { PackageId = ruleFunction.PackageId }
            };
        }
        #endregion

        #region Package

        public static List<MRules._Package> CreateListPackages(List<EnParam.Package> packages)
        {
            List<MRules._Package> result = new List<MRules._Package>();

            foreach (EnParam.Package package in packages)
            {
                result.Add(CreatePackage(package));
            }
            return result;
        }

        public static MRules._Package CreatePackage(EnParam.Package package)
        {
            return new MRules._Package
            {
                Description = package.Description,
                PackageId = package.PackageId

            };
        }
        #endregion

        #region Level
        public static List<MRules._Level> CreateListLevels(List<EnParam.Levels> levels)
        {
            List<MRules._Level> result = new List<MRules._Level>();

            foreach (EnParam.Levels level in levels)
            {
                result.Add(CreateLevel(level));
            }
            return result;
        }

        public static MRules._Level CreateLevel(EnParam.Levels level)
        {
            return new MRules._Level
            {
                Description = level.Description,
                LevelId = level.LevelId
            };
        }

        public static MRules._RuleBase CreateRuleBase(ERules.RuleBase ruleBase)
        {
            return new MRules._RuleBase
            {
                Description = ruleBase.Description,
                RuleBaseId = ruleBase.RuleBaseId,
                IsEvent = ruleBase.IsEvent,
                CurrentFrom = ruleBase.CurrentFrom.ToString("dd/MM/yyyy"),
                RuleBaseType = Enums.RuleBaseType.Option,
                Level = new MRules._Level { LevelId = ruleBase.LevelId },
                Package = new MRules._Package { PackageId = ruleBase.PackageId },
                IsPublished = ruleBase.IsPublished,
                RuleEnumerator = ruleBase.RuleEnumerator,
                Version = ruleBase.RuleBaseVersion
            };
        }



        #endregion

        #region RangeEntity
        public static List<MRules._RangeEntity> CreateRangeEntities(List<ERules.RangeEntity> rangeEntities)
        {
            return rangeEntities.Select(CreateRangeEntity).ToList();
        }

        public static MRules._RangeEntity CreateRangeEntity(ERules.RangeEntity rangeEntity)
        {
            return new MRules._RangeEntity
            {
                RangeEntityCode = rangeEntity.RangeEntityCode,
                RangeValueAt = rangeEntity.RangeValueAt,
                DescriptionRange = rangeEntity.Description,
                RangeEntityValues = new List<MRules._RangeEntityValue>()
            };
        }

        public static List<MRules._RangeEntityValue> CreateRangeEntityValues(List<ERules.RangeEntityValue> rangeEntities)
        {
            return rangeEntities.Select(CreateRangeEntityValue).ToList();
        }

        public static MRules._RangeEntityValue CreateRangeEntityValue(ERules.RangeEntityValue rangeEntity)
        {
            return new MRules._RangeEntityValue
            {
                RangeValueCode = rangeEntity.RangeValueCode,
                FromValue = rangeEntity.FromValue,
                ToValue = rangeEntity.ToValue

            };
        }

        #endregion

        #region ListEntity
        public static List<MRules._ListEntity> CreateListEntities(List<ERules.ListEntity> listEntities)
        {
            return listEntities.Select(CreateListEntity).ToList();
        }

        public static MRules._ListEntity CreateListEntity(ERules.ListEntity listEntity)
        {
            return new MRules._ListEntity
            {
                ListEntityCode = listEntity.ListEntityCode,
                ListValueAt = listEntity.ListValueAt,
                DescriptionList = listEntity.Description,
                ListEntityValues = new List<MRules._ListEntityValue>()
            };
        }

        public static List<MRules._ListEntityValue> CreateListEntityValues(List<ERules.ListEntityValue> entityValues)
        {
            return entityValues.Select(CreateListEntityValue).ToList();
        }

        public static MRules._ListEntityValue CreateListEntityValue(ERules.ListEntityValue listEntityValue)
        {
            return new MRules._ListEntityValue
            {
                ListValueCode = listEntityValue.ListValueCode,
                ListValue = listEntityValue.ListValue
            };
        }

        #endregion
    }
}
