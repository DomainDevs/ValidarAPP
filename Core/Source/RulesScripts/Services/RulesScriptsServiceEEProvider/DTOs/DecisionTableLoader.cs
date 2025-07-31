using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using ConceptType = Sistran.Core.Application.RulesScriptsServices.Enums.ConceptType;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Helper;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    public class DecisionTableLoader
    {
        private DataSet dataSet;
        private DecisionTableMapping dtm;
        private Dictionary<string, DecisionTableMappingExcelPageColumn> condicionConcepts = new Dictionary<string, DecisionTableMappingExcelPageColumn>();
        private Dictionary<string, DecisionTableMappingExcelPageColumn> actionConcepts = new Dictionary<string, DecisionTableMappingExcelPageColumn>();
        private string pathXml;
        private string pathXls;

        public DecisionTableLoader(string pathXml, string pathXls)
        {
            this.pathXml = pathXml;
            this.pathXls = pathXls;
        }

        public bool ReadMappingFile()
        {
            try
            {
                using (var reader = new StreamReader(pathXml))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(DecisionTableMapping));
                    dtm = (DecisionTableMapping)ser.Deserialize(reader);

                    //Obtener los IDs de entidad y concepto a partir del nombre
                    condicionConcepts.Clear();
                    foreach (DecisionTableMappingExcelPage excelPage in dtm.ExcelPage)
                    {
                        foreach (DecisionTableMappingExcelPageColumn column in excelPage.Column)
                        {
                            if (column.entity != "Operator")
                            {
                                var entity = EntityDAO.GetEntityByName(column.entity);
                                var concept = ConceptDAO.GetConceptByConceptNameEntityId(column.concept, entity.EntityId);

                                column.EntityId = entity.EntityId;
                                column.ConceptId = concept.ConceptId;
                                column.IsDecimal = (byte)concept.ConceptControlCode == 2;

                                if (column.type == DecisionTableMappingExcelPageColumnType.condition && !condicionConcepts.ContainsKey(column.entity))
                                { // se hace esto para luego insertar en las tablas scr.rule_action_concept y scr.rule_condition_concept
                                    condicionConcepts.Add(column.entity_concept, column);
                                }
                                else
                                {
                                    if (column.type == DecisionTableMappingExcelPageColumnType.action && !actionConcepts.ContainsKey(column.entity_concept))
                                    {
                                        actionConcepts.Add(column.entity_concept, column);
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReadExcelFile()
        {
            try
            {
                var errorMessage = "";
                foreach (DecisionTableMappingExcelPage excelPage in dtm.ExcelPage)
                {
                    DataTable dt = ExcelFilesLoadHelper.GetExcelFileWorkSheetInfo(pathXls, ref errorMessage, excelPage.name, excelPage.range);
                    if (errorMessage != "")
                    {
                        throw new Exception(errorMessage);
                    }
                    else
                    {
                        if (excelPage.Column.Length != dt.Columns.Count)
                        {
                            throw new Exception("El rango no coincide con la definicion en el archivo de mapeo:\nNombre hoja: " + excelPage.name + "\nColumnas definidas: " + excelPage.Column.Length + "\nRango definido: " + excelPage.range);
                        }
                        bool firtLine = true;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (firtLine)
                            {
                                firtLine = false;
                            }
                            else
                            {
                                DataRow row = dataSet.Tables[excelPage.name].NewRow();
                                foreach (DecisionTableMappingExcelPageColumn column in excelPage.Column)
                                {
                                    row[column.entity_concept] = dr[column.order - 1];
                                }
                                dataSet.Tables[excelPage.name].Rows.Add(row);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RuleBase InsertRules()
        {
            try
            {
                Models.RuleBase RuleBase = new RuleBase();
                List<Models.Concept> Condiciones = new List<Concept>();
                List<Models.Concept> Acciones = new List<Concept>();

                foreach (DecisionTableMappingExcelPage page in dtm.ExcelPage)
                {
                    PackageDAO packageDAO = new PackageDAO();
                    int packageId = packageDAO.ListPackage2().Where(x => x.Disabled == false && x.Description == page.package.ToString()).FirstOrDefault().PackageId;

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Entities.Level.Properties.PackageId, packageId);
                    var levelList = LevelDAO.ListLevel(filter.GetPredicate(), new[] { Entities.Level.Properties.LevelId }).Cast<Entities.Level>().ToList();


                    RuleBase.Description = page.rulebase.ToString();
                    RuleBase.PackageId = packageId;
                    RuleBase.LevelId = levelList.Where(x => x.Description == page.level.ToString()).FirstOrDefault().LevelId;
                    RuleBase.CurrentFrom = DateTime.Now;
                    RuleBase.IsPublished = false;

                    //recorre las condiciones
                    int i = 1;
                    foreach (DecisionTableMappingExcelPageColumn column in condicionConcepts.Values)
                    {
                        if (column.entity != "Operator")
                        {
                            Condiciones.Add(new Models.Concept()
                            {
                                EntityId = column.EntityId,
                                ConceptId = column.ConceptId,
                                OrderNum = i++
                            });
                        }
                    }

                    //Recorre las acciones
                    i = 1;
                    foreach (DecisionTableMappingExcelPageColumn column in actionConcepts.Values)
                    {
                        if (column.entity != "Operator")
                        {
                            Acciones.Add(new Concept()
                            {
                                EntityId = column.EntityId,
                                ConceptId = column.ConceptId,
                                OrderNum = i++
                            });
                        }
                    }
                }

                RuleBase.RuleBaseId = RuleBaseDAO.CreateTableDecision(RuleBase, Condiciones, Acciones).RuleBaseId;
                return RuleBase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DecisionTableMappingResult InsertDataRules(RuleBase ruleBase)
        {
            if (Context.Current == null)
            {
                Context context = new Context();
            }

            List<Entities.RuleConditionComparator> ruleConditionComparator = new List<Entities.RuleConditionComparator>();
            BusinessCollection<Entities.Rule> listRules = new BusinessCollection<Entities.Rule>();
            BusinessCollection<Entities.RuleCondition> listRuleCondition = new BusinessCollection<Entities.RuleCondition>();
            BusinessCollection<Entities.RuleAction> listRuleAction = new BusinessCollection<Entities.RuleAction>();

            int ruleConditionCount = 0;
            int ruleActionCount = 0;
            int ruleCount = 0;

            try
            {

                foreach (DecisionTableMappingExcelPage page in dtm.ExcelPage)
                {
                    //Recorre los datos a insertar
                    var ruleId = 1;
                    foreach (DataRow row in dataSet.Tables[page.name].Rows)
                    {
                        //Se crea la regla
                        var Rule = new Entities.Rule(ruleBase.RuleBaseId)
                        {
                            RuleBaseId = ruleBase.RuleBaseId,
                            RuleId = ruleId++,
                            Order = 0,
                        };
                        listRules.Add(Rule);

                        int conditionId = 1;
                        int actionId = 1;
                        int iComparatorCd = 1;

                        foreach (DecisionTableMappingExcelPageColumn column in page.Column)
                        {
                            if (column.entity != "Operator")
                            {
                                var Value = row[column.entity_concept];
                                var concept = ModelAssembler.CreateConcept(ConceptDAO.GetConceptByConceptIdEntityId(column.ConceptId, column.EntityId));

                                if (!string.IsNullOrEmpty(Value.ToString()))
                                {
                                    if (concept.ConceptTypeCode == ConceptType.List)
                                    {
                                        var listConcept =
                                            ListConceptDAO.GetListConceptByConceptIdEntityId(column.ConceptId,
                                                column.EntityId);
                                        var listEntityCode =
                                            ListEntityDAO
                                                .GetListEntityValueByListEntityCode(listConcept.ListEntityCode);

                                        if (listEntityCode[0].ListEntityValue
                                                .Count(x => x.ListValueCode.ToString() == Value.ToString()) == 0)
                                        {
                                            throw new Exception("El concepto (" + concept.ConceptId + "-" +
                                                                        concept.EntityId + " " + concept.ConceptName +
                                                                        ") no posee el valor " + Value);
                                        }
                                    }
                                    else if (concept.ConceptTypeCode == ConceptType.Range)
                                    {
                                        var rangeConcept =
                                            RangeConceptDAO.FindRangeConcept(column.ConceptId, column.EntityId);
                                        var rangeEntityCode =
                                            RangeEntityDAO.GetRangeEntityValueByRangeEntityCode(rangeConcept
                                                .RangeEntityCode);

                                        if (rangeEntityCode[0].RangeEntityValue
                                                .Count(x => x.RangeValueCode.ToString() == Value.ToString()) == 0)
                                        {
                                            throw new Exception("El concepto (" + concept.ConceptId + "-" +
                                                                        concept.EntityId + " " + concept.ConceptName +
                                                                        ") no posee el valor " + Value);
                                        }
                                    }
                                    else if (concept.ConceptTypeCode == ConceptType.Reference)
                                    {
                                        var referenceConcept =
                                            ReferenceConceptDAO.FindReferenceConcept(column.ConceptId, column.EntityId);
                                        var datos = RuleSetDAO.GetDataEntitiesDictionary(referenceConcept.FentityId);

                                        if (!datos.ContainsKey(Value.ToString()))
                                        {
                                            throw new Exception("El concepto (" + concept.ConceptId + "-" +
                                                                        concept.EntityId + " " + concept.ConceptName +
                                                                        ") no posee el valor " + Value);
                                        }
                                    }
                                }


                                if (column.type == DecisionTableMappingExcelPageColumnType.condition)
                                {
                                    string theValue;

                                    Entities.RuleCondition ruleCondition = new Entities.RuleCondition(ruleBase.RuleBaseId, Rule.RuleId, conditionId);
                                    ruleCondition.EntityId = column.EntityId;
                                    ruleCondition.ConceptId = column.ConceptId;
                                    ruleCondition.RuleValueTypeCode = 1;
                                    ruleCondition.OrderNum = 0;

                                    if (Value.ToString() != "")
                                    {
                                        ruleCondition.ComparatorCode = iComparatorCd;
                                    }
                                    if (Value.GetType() == typeof(decimal))
                                    {
                                        theValue = ConvertHelper.ConvertToDecimal(Value.ToString()).ToString();
                                    }
                                    else
                                    {
                                        theValue = Value.ToString();
                                    }
                                    ruleCondition.CondValue = theValue;
                                    conditionId++;
                                    ruleConditionCount++;

                                    listRuleCondition.Add(ruleCondition);
                                }
                                else
                                {
                                    string theValue;


                                    Entities.RuleAction ruleAction = new Entities.RuleAction(ruleBase.RuleBaseId, Rule.RuleId, actionId);
                                    ruleAction.EntityId = column.EntityId;
                                    ruleAction.ConceptId = column.ConceptId;
                                    ruleAction.ActionTypeCode = 1;
                                    ruleAction.ValueTypeCode = 1;
                                    ruleAction.OrderNum = 0;

                                    if (Value.ToString() != "")
                                    {
                                        ruleAction.OperatorCode = iComparatorCd;
                                    }
                                    if (Value.GetType() == typeof(decimal))
                                    {
                                        theValue = ConvertHelper.ConvertToDecimal(Value.ToString()).ToString();
                                    }
                                    else
                                    {
                                        theValue = Value.ToString();
                                    }
                                    ruleAction.ActionValue = theValue;
                                    
                                    actionId++;
                                    ruleActionCount++;

                                    listRuleAction.Add(ruleAction);
                                }
                                iComparatorCd = 1;
                            }
                            else
                            {
                                var Symbol = row[column.entity_concept].ToString().Trim();
                                if (!string.IsNullOrEmpty(Symbol))
                                {
                                    if (ruleConditionComparator.Count(x => x.Symbol.Equals(Symbol)) == 0)
                                    {
                                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                                        filter.PropertyEquals(Entities.RuleConditionComparator.Properties.Symbol, row[column.entity_concept]);

                                        var comparator = RuleConditionComparatorDAO.GetComparators(filter.GetPredicate(), new[] { Entities.RuleConditionComparator.Properties.ComparatorCode })
                                            .Cast<Entities.RuleConditionComparator>().ToList().FirstOrDefault();

                                        iComparatorCd = comparator.ComparatorCode;

                                        ruleConditionComparator.Add(comparator);
                                    }
                                    else
                                    {
                                        iComparatorCd = ruleConditionComparator.Where(x => x.Symbol.Equals(Symbol)).FirstOrDefault().ComparatorCode;
                                    }
                                }
                            }
                        }
                        ruleCount++;
                    }
                }

                #region Guardado por Framework
                /*REaliza el guardado de las entidades*/
                //RuleDAO.CreateRules(listRules);
                //RuleConditionDAO.CreateRuleConditions(listRuleCondition);
                //RuleActionDAO.CreateRuleActions(listRuleAction);
                #endregion

                #region Guardado Por Store Procedure
                DataTable CreateRules = new DataTable("SCR.PARAM_RULES_RULE");
                CreateRules.Columns.Add("RULE_BASE_ID", typeof(int));
                CreateRules.Columns.Add("RULE_ID", typeof(int));
                CreateRules.Columns.Add("ORDER_NUM", typeof(int));

                DataTable CreateRuleConditions = new DataTable("SCR.PARAM_RULES_CONDITION");
                CreateRuleConditions.Columns.Add("RULE_BASE_ID", typeof(int));
                CreateRuleConditions.Columns.Add("RULE_ID", typeof(int));
                CreateRuleConditions.Columns.Add("CONDITION_ID", typeof(int));
                CreateRuleConditions.Columns.Add("ENTITY_ID", typeof(int));
                CreateRuleConditions.Columns.Add("CONCEPT_ID", typeof(int));
                CreateRuleConditions.Columns.Add("COMPARATOR_CD", typeof(int));
                CreateRuleConditions.Columns.Add("RULE_VALUE_TYPE_CD", typeof(int));
                CreateRuleConditions.Columns.Add("COND_VALUE", typeof(string));
                CreateRuleConditions.Columns.Add("ORDER_NUM", typeof(int));

                DataTable CreateRuleActions = new DataTable("SCR.PARAM_RULES_ACTION");
                CreateRuleActions.Columns.Add("RULE_BASE_ID", typeof(int));
                CreateRuleActions.Columns.Add("RULE_ID", typeof(int));
                CreateRuleActions.Columns.Add("ACTION_ID", typeof(int));
                CreateRuleActions.Columns.Add("ACTION_TYPE_CD", typeof(int));
                CreateRuleActions.Columns.Add("ENTITY_ID", typeof(int));
                CreateRuleActions.Columns.Add("CONCEPT_ID", typeof(int));
                CreateRuleActions.Columns.Add("OPERATOR_CD", typeof(int));
                CreateRuleActions.Columns.Add("RULE_VALUE_TYPE_CD", typeof(int));
                CreateRuleActions.Columns.Add("ACTION_VALUE", typeof(string));
                CreateRuleActions.Columns.Add("ORDER_NUM", typeof(int));




                NameValue[] parameters = new NameValue[3];
                foreach (Entities.Rule item in listRules)
                {
                    DataRow row = CreateRules.NewRow();
                    row["RULE_BASE_ID"] = item.RuleBaseId;
                    row["RULE_ID"] = item.RuleId;
                    row["ORDER_NUM"] = item.Order;

                    CreateRules.Rows.Add(row);
                }
                foreach (Entities.RuleCondition item in listRuleCondition)
                {
                    DataRow row = CreateRuleConditions.NewRow();
                    row["RULE_BASE_ID"] = item.RuleBaseId;
                    row["RULE_ID"] = item.RuleId;
                    row["CONDITION_ID"] = item.ConditionId;
                    row["ENTITY_ID"] = item.EntityId;
                    row["CONCEPT_ID"] = item.ConceptId;
                    row["COMPARATOR_CD"] = item.ComparatorCode != null ? item.ComparatorCode : (object)DBNull.Value;
                    row["RULE_VALUE_TYPE_CD"] = item.RuleValueTypeCode;
                    row["COND_VALUE"] = item.CondValue != null ? item.CondValue : (object)DBNull.Value;
                    row["ORDER_NUM"] = item.OrderNum;

                    CreateRuleConditions.Rows.Add(row);
                }
                foreach (Entities.RuleAction item in listRuleAction)
                {
                    DataRow row = CreateRuleActions.NewRow();
                    row["RULE_BASE_ID"] = item.RuleBaseId;
                    row["RULE_ID"] = item.RuleId;
                    row["ACTION_ID"] = item.ActionId;
                    row["ACTION_TYPE_CD"] = item.ActionTypeCode;
                    row["ENTITY_ID"] = item.EntityId != null ? item.EntityId : (object)DBNull.Value;
                    row["CONCEPT_ID"] = item.ConceptId != null ? item.ConceptId : (object)DBNull.Value;
                    row["OPERATOR_CD"] = item.OperatorCode != null ? item.OperatorCode : (object)DBNull.Value;
                    row["RULE_VALUE_TYPE_CD"] = item.ValueTypeCode;
                    row["ACTION_VALUE"] = item.ActionValue != null ? item.ActionValue : (object)DBNull.Value;
                    row["ORDER_NUM"] = item.OrderNum;

                    CreateRuleActions.Rows.Add(row);
                }
                parameters[0] = new NameValue("Rules", CreateRules);
                parameters[1] = new NameValue("Conditions", CreateRuleConditions);
                parameters[2] = new NameValue("Actions", CreateRuleActions);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPScalar("SCR.INSERT_DESICION_TABLE", parameters);
                }
                #endregion

                return new DecisionTableMappingResult()
                {
                    RuleBase = ruleBase,
                    CountCondition = ruleConditionCount,
                    CountActions = ruleActionCount,
                    CountRules = ruleCount
                };
            }
            catch (Exception ex)
            {
                RuleBaseDAO.DeleteTableDecision(ruleBase);
                throw ex;
            }
        }

        public void DeleteFiles()
        {
            if (File.Exists(pathXml))
            {
                File.Delete(pathXml);
            }
            if (File.Exists(pathXls))
            {
                File.Delete(pathXls);
            }
        }

        public void CreateDataSet()
        {
            try
            {
                dataSet = new DataSet();
                foreach (DecisionTableMappingExcelPage excelPage in dtm.ExcelPage)
                {
                    dataSet.Tables.Add(excelPage.name);
                    foreach (DecisionTableMappingExcelPageColumn column in excelPage.Column)
                    {
                        if (column.IsDecimal)
                        {
                            dataSet.Tables[excelPage.name].Columns.Add(column.entity_concept, typeof(decimal));
                        }
                        else
                        {
                            dataSet.Tables[excelPage.name].Columns.Add(column.entity_concept, typeof(string));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataSet()
        {
            return dataSet.Tables[0];
        }
    }
}

