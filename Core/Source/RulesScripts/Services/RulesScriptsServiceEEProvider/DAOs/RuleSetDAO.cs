using Sistran.Co.Application.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Entity = Sistran.Core.Application.Parameters.Entities.Entity;
using RuleSerEnum = Sistran.Core.Application.RulesScriptsServices.Enums;
using RuleSetEntities = Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using RuleSetmodels = Sistran.Core.Application.RulesScriptsServices.Models;
using SCREN = Sistran.Core.Application.Script.Entities;
using Sistran.Core.Application.Utilities.RulesEngine;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    using ModelServices.Enums;
    using Sistran.Core.Application.Utilities.Cache;

    //using Sistran.Core.Application.Cache;

    public static class RuleSetDAO
    {
        /// <summary>
        /// crea un RuleSet
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        public static RuleSetEntities.RuleSet CreateRuleSet(RuleSetEntities.RuleSet ruleSet)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(ruleSet);
                return ruleSet;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleSet", ex);
            }

        }

        /// <summary>
        /// actualiza un RuleSet
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <returns></returns>
        public static RuleSetEntities.RuleSet UpdateRuleSet(RuleSetEntities.RuleSet ruleSet)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ruleSet);
                return ruleSet;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRuleSet", ex);
            }

        }

        /// <summary>
        /// elimina un RuleSet
        /// </summary>
        /// <param name="ruleSet"></param>
        public static void DeleteRuleSet(int ruleSetId)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(FindRuleSet(ruleSetId));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener RuleSetEntities", ex);
            }

        }

        /// <summary>
        /// obtien los rulesSet a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListRuleSet(Predicate filter, string[] sort)
        {
            try
            {
                List<RuleSetEntities.RuleSet> listRuleSet = new List<RuleSetEntities.RuleSet>();
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.RuleSetId)));
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.Description)));
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.LevelId)));
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.PackageId)));
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.CurrentFrom)));
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.RuleSetVer)));
                select.AddSelectValue(new SelectValue(new Column(RuleSetEntities.RuleSet.Properties.IsEvent)));
                select.Table = new ClassNameTable(typeof(RuleSetEntities.RuleSet), "RuleSet");
                select.Where = filter;

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        listRuleSet.Add(new RuleSet((int)reader["RuleSetId"])
                        {
                            RuleSetId = (int)reader["RuleSetId"],
                            Description = (string)reader["Description"],
                            LevelId = (int)reader["LevelId"],
                            PackageId = (int)reader["PackageId"],
                            CurrentFrom = (DateTime)reader["CurrentFrom"],
                            RuleSetVer = (int)reader["RuleSetVer"],
                            IsEvent = (bool)reader["IsEvent"]
                        });
                    }
                }
                BusinessCollection businessCollection = new BusinessCollection();
                businessCollection.AddRange(listRuleSet);
                return businessCollection;
                //return new BusinessCollection(
                //    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RuleSetEntities.RuleSet),
                //        filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRuleSet", ex);
            }


        }

        /// <summary>
        /// obtiene un ruleSet a partir de su Id
        /// </summary>
        /// <param name="ListEntityId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static RuleSetEntities.RuleSet FindRuleSet(int ruleSetId)
        {
            try
            {
                PrimaryKey key = RuleSetEntities.RuleSet.CreatePrimaryKey(ruleSetId);
                RuleSetEntities.RuleSet ruleSet =
                    (RuleSetEntities.RuleSet)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return ruleSet;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener RuleSetEntities", ex);
            }

        }

        /// <summary>
        /// obtiene un RuleEditorData a partir del ruleSetId
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public static RuleEditorData FillRuleEditorData(int ruleSetId)
        {
            try
            {
                RuleEditorData ruleEditorData = new RuleEditorData();
                if (ruleSetId > 0)
                {
                    RuleSetEntities.RuleSet ruleSet = FindRuleSet(ruleSetId);

                    ruleEditorData.IsNew = false;
                    ruleEditorData.RuleSetId = ruleSet.RuleSetId;
                    ruleEditorData.Description = ruleSet.Description;
                    ruleEditorData.CurrentFrom = ruleSet.CurrentFrom;
                    ruleEditorData.PackageId = ruleSet.PackageId;
                    ruleEditorData.LevelId = ruleSet.LevelId;
                    ruleEditorData.Version = ruleSet.RuleSetVer;

                    RuleBaseXmlReader reader = new RuleBaseXmlReader();
                    XPathDocument xpdoc = new XPathDocument(new MemoryStream(ruleSet.RuleSetXml));
                    XPathNavigator nav = xpdoc.CreateNavigator();
                    RuleBaseDef ruleSetDef = reader.LoadRuleBaseDef(nav);

                    RuleSetDefinitionReader rsReader = new RuleSetDefinitionReader(ruleSetDef.RuleSet);

                    ruleEditorData.Rules = rsReader.GetRuleCollection();

                }

                return ruleEditorData;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FillRuleEditorData", ex);
            }

        }

        /// <summary>
        /// obtiene una nlista de RuleSetmodels.RuleSet
        /// </summary>
        /// <returns></returns>
        public static List<RuleSetmodels.RuleSet> GetRuleSets(bool IsEvent)
        {
            try
            {
                GetRulesSetView view = new GetRulesSetView();
                ViewBuilder builder = new ViewBuilder("GetRulesSetView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                var query = from t1 in ((IList)view.RuleSets).Cast<RuleSetEntities.RuleSet>().ToList()
                            join t2 in ((IList)view.Packages).Cast<RuleSetEntities.Package>().ToList()
                            on t1.PackageId equals t2.PackageId
                            join t3 in ((IList)view.Levels).Cast<RuleSetEntities.Level>().ToList()
                            on new { t1.LevelId, t2.PackageId } equals new { t3.LevelId, t3.PackageId }
                            where !t1.Description.Contains("DT")
                            && t2.PackageId == 1
                            && t1.IsEvent.Equals(IsEvent)
                            select new
                            {
                                RuleSetId = t1.RuleSetId,
                                RuleSetDescription = t1.Description,
                                LevelId = t1.LevelId,
                                LevelDescription = t3.Description,
                                PackageId = t2.PackageId,
                                PackageDescription = t2.Description,
                                IsEvent = t1.IsEvent
                            };

                List<RuleSetmodels.RuleSet> ruleSets = new List<RuleSetmodels.RuleSet>();

                foreach (var item in query)
                {
                    RuleSetmodels.RuleSet ruleSet = new RuleSetmodels.RuleSet();

                    ruleSet.RuleSetId = item.RuleSetId;
                    ruleSet.Description = item.RuleSetDescription;
                    ruleSet.LevelId = item.LevelId;
                    ruleSet.Level = new RuleSetmodels.Level();
                    ruleSet.Level.Description = item.LevelDescription;
                    ruleSet.Package = new RuleSetmodels.Package();
                    ruleSet.PackageId = item.PackageId;
                    ruleSet.Package.Description = item.PackageDescription;
                    ruleSet.IsEvent = (bool)item.IsEvent;

                    ruleSets.Add(ruleSet);
                }

                return ruleSets.OrderByDescending(x => x.RuleSetId).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSets", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de RuleSetmodels.RuleSet a partir de packageId,levelId,productId
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="levelId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.RuleSet> GetRuleSetByPackageIdLevelIdProductId(int? packageId, int? levelId,
            int? productId)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetByPackageIdLevelIdProductId", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de RuleSetmodels.RuleComposite a partir de ruleSetId
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.RuleComposite> GetRuleNames(int ruleSetId)
        {
            try
            {
                RuleEditorData ruleEditorData = FillRuleEditorData(ruleSetId);

                List<RuleSetmodels.RuleComposite> ruleComposites = new List<RuleSetmodels.RuleComposite>();

                if (ruleEditorData.Rules != null)
                {
                    int i = 0;

                    foreach (RuleDTO ruleDTO in ruleEditorData.Rules)
                    {
                        RuleSetmodels.RuleComposite ruleComposite =
                            new RuleSetmodels.RuleComposite { RuleId = i, RuleName = ruleDTO.Name };
                        i++;
                        //Models.RuleName rn = new Models.RuleName { RuleNameId = i, Name = ruleDTO.Name };
                        ruleComposites.Add(ruleComposite);
                    }
                }
                return ruleComposites;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleNames", ex);
            }

        }

        /// <summary>
        /// obtiene RuleSetmodels.ConceptControl a partir del conceptId, entityId
        /// </summary>
        /// <param name="conceptId">The concept identifier.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error Obtener GetConceptControl</exception>
        public static RuleSetmodels.ConceptControl GetConceptControl(int conceptId, int entityId)
        {
            try
            {
                SCREN.Concept concept = null;
                Func<SCREN.Concept, bool> predicate = (str) => str == null ? false : str.ConceptId == conceptId && str.EntityId == entityId;
                concept = (SCREN.Concept)InProcCache.Instance.GetCurrent<SCREN.Concept>(RulesConstant.Entityconcept, predicate);
                if (concept == null)
                {
                    concept = ConceptDAO.GetConceptByConceptIdEntityId(conceptId, entityId);
                }

                RuleSetmodels.ConceptControl conceptControl = null;
                ObjectCriteriaBuilder filter;
                IList basicConceptCheckList;
                if (concept != null)
                {
                    switch (concept.ConceptControlCode)
                    {

                        case 1: //TextBoxEditor
                            conceptControl = new RuleSetmodels.TextBoxControl
                            {
                                ConceptControlCode = concept.ConceptControlCode,
                                Description = ((RuleSerEnum.ConceptControlType)concept.ConceptControlCode).ToString(),
                                ConceptTypeCode = RuleSerEnum.ConceptType.Basic,
                                BasicType = RuleSerEnum.BasicType.Text
                            };

                            filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(Entities.BasicConceptCheck.Properties.ConceptId, conceptId)
                                .And()
                                .PropertyEquals(Entities.BasicConceptCheck.Properties.EntityId, entityId);

                            basicConceptCheckList = BasicConceptCheckDAO.List(filter.GetPredicate(), null);

                            foreach (Entities.BasicConceptCheck basicConceptCheck in basicConceptCheckList)
                            {
                                ((RuleSetmodels.TextBoxControl)conceptControl).Maxlength =
                                    (int)basicConceptCheck.IntValue;
                                break;
                            }

                            break;

                    case 2: // NumberEditor
                        conceptControl = new RuleSetmodels.NumericEditorControl
                        {
                            ConceptControlCode = concept.ConceptControlCode,
                            Description = ((RuleSerEnum.ConceptControlType)concept.ConceptControlCode).ToString(),
                            ConceptTypeCode = RuleSerEnum.ConceptType.Basic
                        };
                        Func<SCREN.BasicConcept, bool> predicateBasic = (str) => str == null ? false : str.ConceptId == conceptId && str.EntityId == entityId;
                        SCREN.BasicConcept basicConcept = null;
                        basicConcept = (SCREN.BasicConcept)InProcCache.Instance.GetCurrent<SCREN.BasicConcept>(RulesConstant.entityBasicConcept, predicateBasic);
                        if (basicConcept == null)
                        {
                            basicConcept = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(conceptId, entityId);
                        }

                        if (basicConcept != null)
                        {
                            if (basicConcept.BasicTypeCode == 1)
                            {
                                conceptControl.BasicType = RuleSerEnum.BasicType.Numeric;
                            }

                                else if (basicConcept.BasicTypeCode == 3)
                                {
                                    conceptControl.BasicType = RuleSerEnum.BasicType.Decimal;
                                }
                                else if (basicConcept.BasicTypeCode == 4)
                                {
                                    conceptControl.BasicType = RuleSerEnum.BasicType.Date;
                                }
                                else if (basicConcept.BasicTypeCode == 2)
                                {
                                    conceptControl.BasicType = RuleSerEnum.BasicType.Text;
                                }
                            }
                            else
                            {
                                filter = new ObjectCriteriaBuilder();
                                filter.PropertyEquals(Entities.BasicConceptCheck.Properties.ConceptId, conceptId)
                                    .And()
                                    .PropertyEquals(Entities.BasicConceptCheck.Properties.EntityId, entityId);

                                basicConceptCheckList = BasicConceptCheckDAO.List(filter.GetPredicate(), null);

                                foreach (Entities.BasicConceptCheck basicConceptCheck in basicConceptCheckList)
                                {
                                    if (basicConceptCheck.BasicCheckCode == 1)
                                    {
                                        ((RuleSetmodels.NumericEditorControl)conceptControl).MaxValue =
                                            basicConceptCheck.IntValue;
                                    }
                                    if (basicConceptCheck.BasicCheckCode == 2)
                                    {
                                        ((RuleSetmodels.NumericEditorControl)conceptControl).MinValue =
                                            basicConceptCheck.IntValue;
                                    }

                                }
                            }
                            break;

                        case 3: // DateEditor
                            conceptControl = new RuleSetmodels.DateEditorControl
                            {
                                ConceptControlCode = concept.ConceptControlCode,
                                Description = ((RuleSerEnum.ConceptControlType)concept.ConceptControlCode).ToString(),
                                ConceptTypeCode = RuleSerEnum.ConceptType.Basic
                            };

                            conceptControl.BasicType = RuleSerEnum.BasicType.Date;

                            filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(Entities.BasicConceptCheck.Properties.ConceptId, conceptId)
                                .And()
                                .PropertyEquals(Entities.BasicConceptCheck.Properties.EntityId, entityId);

                            basicConceptCheckList = BasicConceptCheckDAO.List(filter.GetPredicate(), null);

                            foreach (Entities.BasicConceptCheck basicConceptCheck in basicConceptCheckList)
                            {
                                if (basicConceptCheck.BasicCheckCode == 1)
                                {
                                    ((RuleSetmodels.DateEditorControl)conceptControl).MaxValue =
                                        basicConceptCheck.DateValue;
                                }
                                if (basicConceptCheck.BasicCheckCode == 2)
                                {
                                    ((RuleSetmodels.DateEditorControl)conceptControl).MinValue =
                                        basicConceptCheck.DateValue;
                                }

                            }
                            break;

                    case 4: // ListBox
                        SCREN.ListConcept listConcept = null;
                        Func<SCREN.ListConcept, bool> predicateList = (str) => str == null ? false : str.ConceptId == conceptId && str.EntityId == entityId;
                        listConcept = (SCREN.ListConcept)InProcCache.Instance.GetCurrent<SCREN.ListConcept>(RulesConstant.entityListConcept, predicateList);
                        if (listConcept == null)
                        {
                            listConcept = ListConceptDAO.FindListConcept(conceptId, entityId);
                        }
                        if (listConcept != null)
                        {
                            conceptControl = new RuleSetmodels.ListBoxControl
                            {
                                ConceptControlCode = concept.ConceptControlCode,
                                Description = ((RuleSerEnum.ConceptControlType)concept.ConceptControlCode).ToString(),
                                ConceptTypeCode = RuleSerEnum.ConceptType.List,
                                BasicType = RuleSerEnum.BasicType.Null,
                                ListEntityCode = listConcept.ListEntityCode
                            };

                            filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(SCREN.ListEntityValue.Properties.ListEntityCode,
                                listConcept.ListEntityCode);

                            IList listEntityValueList = ListEntityValueDAO.ListListEntityValue(filter.GetPredicate(),
                                new string[] { SCREN.ListEntityValue.Properties.ListValueCode });

                            ((RuleSetmodels.ListBoxControl)conceptControl).ListListEntityValues =
                                new List<RuleSetmodels.ListEntityValue>();
                            foreach (SCREN.ListEntityValue listEntityValue in listEntityValueList)
                            {
                                RuleSetmodels.ListEntityValue lev = new RuleSetmodels.ListEntityValue();
                                lev.ListEntityCode = listEntityValue.ListEntityCode;
                                lev.ListValueCode = listEntityValue.ListValueCode;
                                lev.ListValue = listEntityValue.ListValue;
                                lev.StatusTypeService = StatusTypeService.Original;

                                    ((RuleSetmodels.ListBoxControl)conceptControl).ListListEntityValues.Add(lev);
                                }
                            }

                        SCREN.RangeConcept rangeConcept =
                            RangeConceptDAO.FindRangeConcept(conceptId, entityId);

                            if (rangeConcept != null)
                            {
                                conceptControl = new RuleSetmodels.RangeControl
                                {
                                    ConceptControlCode = concept.ConceptControlCode,
                                    Description = ((RuleSerEnum.ConceptControlType)concept.ConceptControlCode).ToString(),
                                    ConceptTypeCode = RuleSerEnum.ConceptType.List,
                                    BasicType = RuleSerEnum.BasicType.Null,
                                    ListRangeEntityValues = new List<RuleSetmodels.RangeEntityValue> {
                                    new RuleSetmodels.RangeEntityValue
                                    {
                                        RangeEntityCode = rangeConcept.RangeEntityCode
                                    }
                                }

                                };

                                filter = new ObjectCriteriaBuilder();
                                filter.PropertyEquals(RuleSetEntities.RangeEntityValue.Properties.RangeEntityCode,
                                    rangeConcept.RangeEntityCode);

                                IList listEntityValueList = RangeEntityValueDAO.ListRangeEntityValue(filter.GetPredicate(),
                                    new string[] { RuleSetEntities.RangeEntityValue.Properties.RangeValueCode });

                                ((RuleSetmodels.RangeControl)conceptControl).ListRangeEntityValues =
                                    new List<RuleSetmodels.RangeEntityValue>();
                                foreach (RuleSetEntities.RangeEntityValue listEntityValue in listEntityValueList)
                                {
                                    RuleSetmodels.RangeEntityValue lev = new RuleSetmodels.RangeEntityValue();
                                    lev.RangeEntityCode = listEntityValue.RangeEntityCode;
                                    lev.RangeValueCode = listEntityValue.RangeValueCode;
                                    lev.ToValue = listEntityValue.ToValue;
                                    lev.FromValue = listEntityValue.FromValue;
                                    lev.StatusTypeService = StatusTypeService.Create;

                                    ((RuleSetmodels.RangeControl)conceptControl).ListRangeEntityValues.Add(lev);
                                }
                            }

                            break;

                        case 5: // SearchCombo
                            conceptControl = new RuleSetmodels.SearchComboControl
                            {
                                ConceptControlCode = concept.ConceptControlCode,
                                Description = ((RuleSerEnum.ConceptControlType)concept.ConceptControlCode).ToString(),
                                ConceptTypeCode = RuleSerEnum.ConceptType.Reference,
                                BasicType = RuleSerEnum.BasicType.Null
                            };

                            RuleSetEntities.ReferenceConcept referenceConcept =
                                ReferenceConceptDAO.FindReferenceConcept(conceptId, entityId);

                            ((RuleSetmodels.SearchComboControl)conceptControl).ForeignEntity = referenceConcept.FentityId;
                            break;

                        case 6: // RichTextEditor (No se usa)
                            break;
                    }
                }
                return conceptControl;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptControl", ex);
            }

        }

        /// <summary>
        /// obtiene object a partir de  conceptId,  entityId
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static object GetDynamicConcept(int conceptId, int entityId)
        {
            try
            {
                object conceptValue =
                    DynamicConceptValueDAO.GetValueDynamicConceptByConceptIdEntiteId(conceptId, entityId);
                return conceptValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDynamicConcept", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de RuleSetmodels.Operator  a partir de conceptId, entityId
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.Operator> GetOperationTypes(int conceptId, int entityId)
        {
            try
            {
                SCREN.Concept concept = ConceptDAO.GetConceptByConceptIdEntityId(conceptId, entityId);

                int ConceptControlCode = 0;
                if (concept != null)
                {
                    ConceptControlCode = concept.ConceptControlCode;
                }

                List<RuleSetmodels.Operator> listOperationTypes = new List<RuleSetmodels.Operator>();

                listOperationTypes.Add(new RuleSetmodels.Operator
                {
                    Symbol = "=",
                    Description = "Asignar",
                    OperatorCode = 5,
                    CodeBinaryOperatorType = System.CodeDom.CodeBinaryOperatorType.Assign
                });

                switch (ConceptControlCode)
                {

                    case 2: // NumberEditor 
                        listOperationTypes.Add(new RuleSetmodels.Operator
                        {
                            Symbol = "+",
                            Description = "Sumar",
                            OperatorCode = 0,
                            CodeBinaryOperatorType = System.CodeDom.CodeBinaryOperatorType.Add
                        });
                        listOperationTypes.Add(new RuleSetmodels.Operator
                        {
                            Symbol = "-",
                            Description = "Restar",
                            OperatorCode = 1,
                            CodeBinaryOperatorType = System.CodeDom.CodeBinaryOperatorType.Subtract
                        });
                        listOperationTypes.Add(new RuleSetmodels.Operator
                        {
                            Symbol = "*",
                            Description = "Multiplicarlo",
                            OperatorCode = 2,
                            CodeBinaryOperatorType = System.CodeDom.CodeBinaryOperatorType.Multiply
                        });
                        listOperationTypes.Add(new RuleSetmodels.Operator
                        {
                            Symbol = "/",
                            Description = "Dividirlo",
                            OperatorCode = 3,
                            CodeBinaryOperatorType = System.CodeDom.CodeBinaryOperatorType.Divide
                        });
                        listOperationTypes.Add(new RuleSetmodels.Operator
                        {
                            Symbol = "Round",
                            Description = "Redondear",
                            OperatorCode = 17
                        });
                        break;
                }
                return listOperationTypes;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetOperationTypes", ex);
            }

        }

        /// <summary>
        /// crea un RuleSet
        /// </summary>
        /// <param name="ruleSetComposite"></param>
        /// <returns></returns>
        public static bool CreateRuleSet(RuleSetmodels.RuleSetComposite ruleSetComposite)
        {
            try
            {
                //RuleSetEntities.RuleSet ruleSet = RuleSetDAO.FindRuleSet(ruleSetComposite.RuleSet.RuleSetId);

                //RuleSetDefinitionBuilder ruleSetBldr = new RuleSetDefinitionBuilder();
                //RuleBaseDef ruleSetDef = ruleSetBldr.GetRuleSetBaseDefinition(
                //    ruleSetComposite.RuleSet.Description,
                //    ruleSetComposite.RuleComposites,
                //    ruleSetComposite.RuleSet.IsTable
                //);

                //RuleBaseXmlReader reader = new RuleBaseXmlReader();
                //MemoryStream ms = new MemoryStream();
                //XmlWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
                //reader.SaveRuleBaseDef(writer, ruleSetDef);
                //writer.Flush();

                //int len = Convert.ToInt32(ms.Length);
                //byte[] bs = new byte[len];
                //ms.Position = 0;
                //ms.Read(bs, 0, len);

                //CommonService.DAOs.ParameterDAO parameterDAO = new CommonService.DAOs.ParameterDAO();
                //Parameter parameter = parameterDAO.GetParameterByParameterId(200);

                //if (ruleSet == null && !ruleSetComposite.RuleSet.IsTable)
                //{
                //    ruleSet = new RuleSetEntities.RuleSet(parameter.NumberParameter.Value + 1);
                //    parameter.NumberParameter = parameter.NumberParameter.Value + 1;
                //    parameterDAO.UpdateParameters(parameter);

                //    ruleSet.RuleSetXml = bs;
                //    ruleSet.Description = ruleSetComposite.RuleSet.IsTable
                //        ? "DT: " + ruleSetComposite.RuleSet.Description
                //        : ruleSetComposite.RuleSet.Description;
                //    ruleSet.LevelId = ruleSetComposite.RuleSet.LevelId;
                //    ruleSet.PackageId = ruleSetComposite.RuleSet.PackageId;
                //    ruleSet.CurrentFrom = DateTime.Now;
                //    ruleSet.RuleSetVer = 1;
                //    ruleSet.IsEvent = ruleSetComposite.RuleSet.IsEvent;
                //    RuleSetDAO.CreateRuleSet(ruleSet);
                //}
                //if (ruleSet == null && ruleSetComposite.RuleSet.IsTable)
                //{
                //    ruleSet = new RuleSetEntities.RuleSet(ruleSetComposite.RuleSet.RuleSetId);

                //    ruleSet.RuleSetXml = bs;
                //    ruleSet.Description = ruleSetComposite.RuleSet.IsTable
                //        ? "DT: " + ruleSetComposite.RuleSet.Description
                //        : ruleSetComposite.RuleSet.Description;
                //    ruleSet.LevelId = ruleSetComposite.RuleSet.LevelId;
                //    ruleSet.PackageId = ruleSetComposite.RuleSet.PackageId;
                //    ruleSet.CurrentFrom = DateTime.Now;
                //    ruleSet.RuleSetVer = 1;
                //    RuleSetDAO.CreateRuleSet(ruleSet);
                //}
                //else
                //{
                //    ruleSet.RuleSetXml = bs;
                //    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ruleSet);
                //    RuleSetDAO.UpdateRuleSet(ruleSet);
                //}

                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleSet", ex);
            }

        }

        /// <summary>
        /// obtiene la Data a partir del filtro entityId, RuleSetmodels.ConditionFilter
        /// </summary>
        /// <param name="filter"></param>
        public static string GetDataFromFilter(int entityId, List<RuleSetmodels.ConditionFilter> conditionFilters)
        {
            try
            {
                Entity entityFind = EntityDAO.FindEntity(entityId);

                List<RuleSetmodels.PropertyFilter> propertyFilters = GetPropertyFilter(entityId);

                StringBuilder sb = new StringBuilder();
                sb.Append("1 = 1");

                if (conditionFilters != null)
                {
                    foreach (RuleSetmodels.ConditionFilter conditionFilter in conditionFilters)
                    {

                        sb.Append(" AND ");

                        sb.Append(conditionFilter.PropertyName);
                        if (conditionFilter.SymbolComparator == "Comienza con")
                        {
                            sb.Append(" LIKE '" + conditionFilter.Value + "%'");
                        }
                        else if (conditionFilter.SymbolComparator == "Contiene")
                        {
                            sb.Append(" LIKE '%" + conditionFilter.Value + "%'");
                        }
                        else
                        {
                            sb.Append(" " + conditionFilter.SymbolComparator);

                            foreach (var propertyFilter in propertyFilters)
                            {
                                if (propertyFilter.PropertyName == conditionFilter.PropertyName)
                                {
                                    if (propertyFilter.Type == "int")
                                    {
                                        sb.Append(" " + conditionFilter.Value);
                                    }

                                    if (propertyFilter.Type == "varchar")
                                    {
                                        sb.Append(" '" + conditionFilter.Value + "'");
                                    }
                                }
                            }
                        }
                    }
                }

                //campos que mostrala la consulta
                string Fields = "";

                for (int i = 0; i < propertyFilters.Count(); i++)
                {
                    Fields += propertyFilters[i].PropertyName + " as " + propertyFilters[i].Description;

                    if (propertyFilters.Count() - 1 != i)
                    {
                        Fields += ", ";
                    }
                }

                NameValue[] pars = new NameValue[3];
                pars[0] = new NameValue("TABLES", entityFind.BusinessView);
                pars[1] = new NameValue("FIELDS", Fields);
                pars[2] = new NameValue("FILTER", sb.ToString());

                DataTable result;

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", pars);
                }

                sb = new StringBuilder();
                sb.Append("[");

                if (result != null && result.Rows.Count > 0)
                {
                    for (int i = 0; i < result.Rows.Count; i++)
                    {
                        sb.Append("{");

                        for (int y = 0; y < result.Columns.Count; y++)
                        {
                            string colName = propertyFilters[y].Description;
                            string celValue = result.Rows[i][y].ToString();

                            sb.Append('\u0022' + colName + '\u0022' + ": " + '\u0022' + celValue + '\u0022');

                            if (y < result.Columns.Count - 1)
                            {
                                sb.Append(",");
                            }
                        }
                        sb.Append("}");

                        if (i < result.Rows.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }

                    sb.Append("]");

                    return sb.ToString();
                }
                return " ";
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDataFromFilter", ex);
            }

        }

        /// <summary>
        /// obtiene la Data a partir del filtro entityId, RuleSetmodels.ConditionFilter
        /// </summary>
        /// <param name="filter"></param>
        public static Dictionary<string, string> GetDataEntitiesDictionary(int entityId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Entity entityFind = EntityDAO.FindEntity(entityId);

            List<RuleSetmodels.PropertyFilter> propertyFilters = GetPropertyFilter(entityId);

            //campos que mostrala la consulta
            string Fields = "";

            for (int i = 0; i < propertyFilters.Count(); i++)
            {
                Fields += propertyFilters[i].PropertyName + " as " + propertyFilters[i].Description;

                if (propertyFilters.Count() - 1 != i)
                {
                    Fields += ", ";
                }
            }

            NameValue[] pars = new NameValue[3];
            pars[0] = new NameValue("TABLES", entityFind.BusinessView);
            pars[1] = new NameValue("FIELDS", Fields);
            pars[2] = new NameValue("FILTER", "1=1");

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("SCR.GET_DATA_FROM_FILTER", pars);
            }

            foreach (DataRow objeData in result.Rows)
            {
                dictionary.Add(objeData.ItemArray[0].ToString(), objeData.ItemArray[1].ToString());
            }

            return dictionary;
        }


        /// <summary>
        /// obtiene una lsista de RuleSetmodels.PropertyFilter a partir de entityId
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.PropertyFilter> GetPropertyFilter(int entityId)
        {
            try
            {
                Entity entity = EntityDAO.FindEntity(entityId);

                List<RuleSetmodels.PropertyFilter> propertyFilters = new List<RuleSetmodels.PropertyFilter>();

                foreach (string propertys in entity.PropertySearch.Split(';'))
                {
                    string[] prop = propertys.Trim().Split(',');

                    RuleSetmodels.PropertyFilter propertyFilter = new RuleSetmodels.PropertyFilter();
                    propertyFilter.EntityId = entityId;
                    propertyFilter.PropertyName = prop[0].Trim().Split(' ')[0];
                    propertyFilter.Description = prop[0].Trim().Split(' ')[2];
                    propertyFilter.Type = prop[1].Trim().Split(' ')[0];
                    propertyFilter.PrimaryKey = false;

                    if (prop.Length > 2)
                    {
                        propertyFilter.PrimaryKey = true;
                    }

                    propertyFilters.Add(propertyFilter);
                }

                return propertyFilters;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetPropertyFilter", ex);
            }

        }

        /// Lista de Concept segun el LevelId especificado
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">PRPERR_ENTITY_DOES_NOT_HAVE_KEY_CONCEPTS</exception>
        public static List<RuleSetmodels.RuleSet> GetConceptsByLevelId(int levelId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RuleSetEntities.RuleSet.Properties.LevelId);
                filter.Equal();
                filter.Constant(levelId);
                return ModelAssembler.CreateRuleSets(ListRuleSet(filter.GetPredicate(), null));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptsByLevelId", ex);
            }


        }

        /// Obtener Lista de Reglas por Nivel
        /// </summary>
        /// <param name="entity">lista Reglas</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">PRPERR_ENTITY_DOES_NOT_HAVE_KEY_CONCEPTS</exception>
        public static List<RuleSetmodels.RuleSet> GetRuleSetByLevels(List<RuleSetmodels.Level> level)
        {
            try
            {
                Predicate filter = null;
                ObjectCriteriaBuilder Objectfilter = null;
                if (level != null && level.Count > 0)
                {
                    Objectfilter = new ObjectCriteriaBuilder();
                    Objectfilter.Property(RuleSetEntities.RuleSet.Properties.LevelId);
                    Objectfilter.In();
                    Objectfilter.ListValue();
                    foreach (RuleSetmodels.Level item in level)
                    {
                        Objectfilter.Constant(item.LevelId);
                    }
                    Objectfilter.EndList();
                    Objectfilter.And().Not().Property(RuleSetEntities.RuleSet.Properties.Description, typeof(RuleSetEntities.RuleSet).Name).Like().Constant("DT%");
                    filter = Objectfilter.GetPredicate();
                }
                else
                {
                    filter = null;
                }
                return ModelAssembler.CreateRuleSets(ListRuleSet(filter, null));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetByLevels", ex);
            }

        }
        /// <summary>
        /// Obtener reglas por el Identificador
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error Obtener GetRuleSetByLevels</exception>
        public static List<RuleSetmodels.RuleSet> GetRuleSetByIds(List<int> ids)
        {
            try
            {
                Predicate filter = null;
                ObjectCriteriaBuilder Objectfilter = null;
                if (ids != null && ids.Count > 0)
                {
                    Objectfilter = new ObjectCriteriaBuilder();
                    Objectfilter.Property(RuleSetEntities.RuleSet.Properties.RuleSetId);
                    Objectfilter.In();
                    Objectfilter.ListValue();
                    foreach (int item in ids)
                    {
                        Objectfilter.Constant(item);
                    }
                    Objectfilter.EndList();
                    filter = Objectfilter.GetPredicate();
                }
                else
                {
                    filter = null;
                }
                return ModelAssembler.CreateRuleSets(ListRuleSet(filter, null));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetByLevels", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de RuleSetmodels.RuleSet a partir de levelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static List<RuleSetmodels.RuleSet> GetRuleSetDTOsByLevelId(int levelId, bool IsEvent)
        {
            try
            {
                GetRulesSetView view = new GetRulesSetView();
                ViewBuilder builder = new ViewBuilder("GetRulesSetView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                DataFacadeManager.Dispose();
                var query = from t1 in ((IList)view.RuleSets).Cast<RuleSetEntities.RuleSet>().ToList()
                            join t2 in ((IList)view.Packages).Cast<RuleSetEntities.Package>().ToList()
                              on t1.PackageId equals t2.PackageId
                            join t3 in ((IList)view.Levels).Cast<RuleSetEntities.Level>().ToList()
                              on t1.LevelId equals t3.LevelId
                            where t1.LevelId == levelId &&
                                   !t1.Description.Contains("DT")
                            //t1.IsEvent.Equals(IsEvent)
                            select new
                            {
                                RuleSetId = t1.RuleSetId,
                                RuleSetDescription = t1.Description,
                                LevelId = t1.LevelId,
                                LevelDescription = t3.Description,
                                PackageId = t2.PackageId,
                                PackageDescription = t2.Description,
                                IsEvent = t1.IsEvent
                            };

                List<RuleSetmodels.RuleSet> ruleSets = new List<RuleSetmodels.RuleSet>();

                foreach (var item in query)
                {
                    RuleSetmodels.RuleSet ruleSet = new RuleSetmodels.RuleSet();

                    ruleSet.RuleSetId = item.RuleSetId;
                    ruleSet.Description = item.RuleSetDescription;
                    ruleSet.LevelId = item.LevelId;
                    ruleSet.Level = new RuleSetmodels.Level();
                    ruleSet.Level.Description = item.LevelDescription;
                    ruleSet.Package = new RuleSetmodels.Package();
                    ruleSet.PackageId = item.PackageId;
                    ruleSet.Package.Description = item.PackageDescription;
                    ruleSet.IsEvent = (bool)item.IsEvent;

                    ruleSets.Add(ruleSet);
                }

                return ruleSets;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleSetDTOsByLevelId", ex);
            }

        }
        #region reglas
        /// <summary>
        /// Finds the rule set.
        /// </summary>
        /// <param name="ruleSetId">The rule set identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static RuleSetmodels._RuleSet GetRuleSetById(int ruleSetId)
        {
            SCREN.RuleSet entityRuleSet = RuleEngineDAO.GetRuleSetByRuleSetId(ruleSetId);
            if (entityRuleSet == null)
            {
                throw new Exception(String.Format("No existe La Regla {0}", ruleSetId));
            }
            Framework.Rules.Integration.RuleSet ruleSet = LoadXml(entityRuleSet);
            return ModelAssembler.CreateRuleSet(ruleSet);
        }

        /// <summary>
        /// Loads the XML.
        /// </summary>
        /// <param name="entityRuleSet">The entity rule set.</param>
        /// <returns></returns>
        private static Sistran.Core.Framework.Rules.Integration.RuleSet LoadXml(SCREN.RuleSet entityRuleSet)
        {
            XmlDocument xmlDocument = GetXmlDocumentByRuleSet(entityRuleSet);
            Sistran.Core.Framework.Rules.Integration.RuleSet ruleSet = (new Framework.Rules.Integration.XmlRuleSetReader(xmlDocument)).Read();
            ruleSet.LevelId = entityRuleSet.LevelId;
            ruleSet.PackageId = entityRuleSet.PackageId;
            ruleSet.Id = entityRuleSet.RuleSetId;
            ruleSet.Version = entityRuleSet.RuleSetVer;
            ruleSet.Name = entityRuleSet.Description;
            return ruleSet;
        }

        /// <summary>
        /// Obtener Xml De Una Regla
        /// </summary>
        /// <param name="ruleSet">Regla</param>
        /// <returns>Xml Regla</returns>
        private static XmlDocument GetXmlDocumentByRuleSet(SCREN.RuleSet ruleSet)
        {
            MemoryStream stream = new MemoryStream(ruleSet.RuleSetXml);

            XmlDocument definition = new XmlDocument();
            definition.Load(stream);

            return definition;
        }
        #endregion
    }
}
