using Sistran.Co.Application.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using RuleModel = Sistran.Core.Application.RulesScriptsServices.Models;
using SCREN = Sistran.Core.Application.Script.Entities;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    /// <summary>
    /// 
    /// </summary>
    public class ConceptDAO
    {
        /// <summary>
        /// crea un concepto
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static SCREN.Concept CreateConcept(SCREN.Concept concept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(concept);
                return concept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateConcept", ex);
            }

        }

        /// <summary>
        /// edita un concepto
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static SCREN.Concept UpdateConcept(SCREN.Concept concept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(concept);
                return concept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateConcept", ex);
            }

        }

        /// <summary>
        /// obtiene un concepto a partir del concept
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static SCREN.Concept GetConcept(SCREN.Concept concept)
        {
            try
            {
                PrimaryKey key = SCREN.Concept.CreatePrimaryKey(concept.ConceptId, concept.EntityId);
                return (SCREN.Concept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateConcept", ex);
            }

        }

        /// <summary>
        /// elimina un concepto
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static void DeleteConcept(SCREN.Concept concept)
        {
            try
            {
                ObjectCriteriaBuilder filter = null;
                switch (concept.ConceptTypeCode)
                {
                    case (int)Enums.ConceptType.Basic:
                        //BasicConceptCheck
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(BasicConceptCheck.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(BasicConceptCheck.Properties.EntityId, concept.EntityId);

                        foreach (BasicConceptCheck item in BasicConceptCheckDAO.List(filter.GetPredicate(), new[] { BasicConceptCheck.Properties.BasicCheckCode }))
                        {
                            BasicConceptCheckDAO.Delete(item);
                        }

                        //BasicConcept
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(BasicConcept.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(BasicConcept.Properties.EntityId, concept.EntityId);

                        foreach (BasicConcept item in BasicConceptDAO.List(filter.GetPredicate(), new[] { BasicConcept.Properties.BasicTypeCode }))
                        {
                            BasicConceptDAO.Delete(item);
                        }
                        break;

                    case (int)Enums.ConceptType.Range:
                        //RangeConcept
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(SCREN.RangeConcept.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(SCREN.RangeConcept.Properties.EntityId, concept.EntityId);

                        foreach (SCREN.RangeConcept rangeConcept in RangeConceptDAO.ListRangeConcept(filter.GetPredicate(), new[] { SCREN.RangeConcept.Properties.RangeEntityCode }))
                        {
                            RangeConceptDAO.DeleteRangeConcept(rangeConcept);
                        }
                        break;

                    case (int)Enums.ConceptType.List:
                        //ListConcept
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(ListConcept.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(ListConcept.Properties.EntityId, concept.EntityId);

                        foreach (ListConcept listConcept in ListConceptDAO.ListListConcept(filter.GetPredicate(), new[] { ListConcept.Properties.ListEntityCode }))
                        {
                            ListConceptDAO.DeleteListConcept(listConcept);
                        }
                        break;

                    case (int)Enums.ConceptType.Reference:
                        //ReferenceConcept
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(ReferenceConcept.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(ReferenceConcept.Properties.EntityId, concept.EntityId);

                        foreach (ReferenceConcept referenceConcept in ReferenceConceptDAO.ListReferenceConcept(filter.GetPredicate(), new[] { ReferenceConcept.Properties.FentityId }))
                        {
                            ReferenceConceptDAO.DeleteReferenceConcept(referenceConcept);
                        }
                        break;
                }
                //Question
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Question.Properties.ConceptId, concept.ConceptId)
                    .And().PropertyEquals(Question.Properties.EntityId, concept.EntityId);
                foreach (Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Question.Properties.Description }))
                {
                    QuestionDAO.DeleteQuestion(item);
                }
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(concept);
            }
            catch (Exception ex)
            {
                if (ex.Message == "RELATED_OBJECT")
                {
                    throw new BusinessException(ex.Message, ex);
                }
                else
                {
                    throw new BusinessException("Error en DeleteConcept", ex);
                }
            }
        }


        public static List<SCREN.Concept> GetConceptsByIdModuleIdLevelDescription(int? idModule, int? idLevel, int filter, string description)
        {
            List<SCREN.Concept> result = new List<SCREN.Concept>();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptId, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.EntityId, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.Description, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptName, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptTypeCode, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.KeyOrder, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsStatic, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptControlCode, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsReadOnly, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsVisible, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsNullable, "c")));
            select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsPersistible, "c")));

            Join join = new Join(new ClassNameTable(typeof(SCREN.Concept), "c"), new ClassNameTable(typeof(SCREN.Entity), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(SCREN.Concept.Properties.EntityId, "c").Equal().Property(SCREN.Entity.Properties.EntityId, "e").GetPredicate());


            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(SCREN.Entity.Properties.Description, "e").Like().Constant("Facade%");


            if (idModule.HasValue)
            {
                where.And().Property(SCREN.Entity.Properties.PackageId, "e").Equal().Constant(idModule);
            }

            if (idLevel.HasValue)
            {
                where.And().Property(SCREN.Entity.Properties.LevelId, "e").Equal().Constant(idLevel);
            }

            if (filter != 0)
            {
                where.And().Property(SCREN.Concept.Properties.IsStatic, "c").Equal().Constant(filter == 1 ? true : false);
            }
            if (!string.IsNullOrEmpty(description))
            {
                where.And().Property(SCREN.Concept.Properties.Description, "c").Like().Constant("%" + description + "%");
            }

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    result.Add(new SCREN.Concept((int)reader["ConceptId"], (int)reader["EntityId"])
                    {
                        Description = (string)reader["Description"],
                        ConceptName = (string)reader["ConceptName"],
                        ConceptTypeCode = (int)reader["ConceptTypeCode"],
                        KeyOrder = (int)reader["KeyOrder"],
                        IsStatic = (bool)reader["IsStatic"],
                        ConceptControlCode = (int)reader["ConceptControlCode"],
                        IsReadOnly = (bool)reader["IsReadOnly"],
                        IsVisible = (bool)reader["IsVisible"],
                        IsNullable = (bool)reader["IsNullable"],
                        IsPersistible = (bool)reader["IsPersistible"]
                    });
                }
            }

            if (!idModule.HasValue && !idLevel.HasValue && filter == 0 && string.IsNullOrEmpty(description))
            {
                result = result.ToList();
            }

            return result;
        }


        /// <summary>
        /// obtiene un lista de RuleModel.ComparatorConcept  a partir del  conceptId,  entityId
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static List<RuleModel.ComparatorConcept> GetConceptComparator(int conceptId, int entityId)
        {
            try
            {
                SCREN.Concept concept = GetConceptByConceptIdEntityId(conceptId, entityId);

                List<RuleModel.ComparatorConcept> comparatorConcepts = new List<RuleModel.ComparatorConcept>();

                comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "1", ComparatorText = "Igual al", Symbol = "=" });//IdentityEquality 1
                comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "2", ComparatorText = "Distinto que el", Symbol = "<>" });//IdentityInequality 2

                if (concept.IsNullable)
                {
                    comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "3", ComparatorText = "Es Nulo", Symbol = "null" });//IdentityEquality 3 
                    comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "4", ComparatorText = "Es NO Nulo", Symbol = "no null" });//IdentityInequality 4
                }

                SCREN.BasicConcept basicConcept = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(conceptId, entityId);

                if (concept.ConceptTypeCode == 1)
                {

                    if (basicConcept.BasicTypeCode == 1 || basicConcept.BasicTypeCode == 3 || basicConcept.BasicTypeCode == 4)
                    {
                        comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "15", ComparatorText = "Mayor que el", Symbol = ">" });//GreaterThan 5
                        comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "13", ComparatorText = "Menor que el", Symbol = "<" });//LessThan 6
                        comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "16", ComparatorText = "Mayor o igual que el", Symbol = ">=" });//GreaterThanOrEqual 7
                        comparatorConcepts.Add(new RuleModel.ComparatorConcept { ComparatorSymbol = "14", ComparatorText = "Menor o igual que el", Symbol = "<=" });//LessThanOrEqual 8
                    }
                }

                return comparatorConcepts;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptComparator", ex);
            }
        }

        /// <summary>
        /// Busca el Concept por PrimaryKey
        /// </summary>
        /// <param name="conceptId">Identificador de Concept.</param>
        /// <param name="entityId">Identificador de Entity.</param>
        /// <returns></returns>
        public static SCREN.Concept GetConceptByConceptIdEntityId(int conceptId, int entityId)
        {
            try
            {
                SCREN.Concept concept = null;
                PrimaryKey key = SCREN.Concept.CreatePrimaryKey(conceptId, entityId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    concept = (SCREN.Concept)daf.GetObjectByPrimaryKey(key);
                }

                return concept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptByConceptIdEntityId", ex);
            }

        }

        /// <summary>
        /// Lista de Concept segun el Entity especificado
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">PRPERR_ENTITY_DOES_NOT_HAVE_KEY_CONCEPTS</exception>
        public static IList GetConceptsByEntity(PARAMEN.Entity entity)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(SCREN.Concept.Properties.EntityId);
                filter.Equal();
                filter.Constant(entity.EntityId);
                filter.And();
                filter.Property(SCREN.Concept.Properties.KeyOrder);
                filter.IsNotNull();
                filter.And();
                filter.Property(SCREN.Concept.Properties.KeyOrder);
                filter.Greater();
                filter.Constant(0);

                string[] sort = new[] { "+" + SCREN.Concept.Properties.KeyOrder };

                IList keyConcepts = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(SCREN.Concept), filter.GetPredicate(), sort);

                if (keyConcepts.Count < 1)
                {
                    throw new BusinessException("PRPERR_ENTITY_DOES_NOT_HAVE_KEY_CONCEPTS", new object[] { entity.EntityId });
                }
                return keyConcepts;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptsByEntity", ex);
            }

        }

        /// <summary>
        /// Obtiene IList de Concept segun el filtro especificado
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList GetConceptsByFilterSort(Predicate filter, string[] sort)
        {
            try
            {
                return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(SCREN.Concept), filter, sort);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptsByFilterSort", ex);
            }

        }

        /// <summary>
        /// Gets the concept by concept name entity identifier.
        /// </summary>
        /// <param name="conceptName">Name of the concept.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="Sistran.Core.Framework.BAF.BusinessException">Concept not found</exception>
        public static SCREN.Concept GetConceptByConceptNameEntityId(string conceptName, int entityId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(SCREN.Concept.Properties.ConceptName)
                      .Equal()
                      .Constant(conceptName)
                   .And()
                      .Property(SCREN.Concept.Properties.EntityId)
                      .Equal()
                      .Constant(entityId);

                BusinessCollection concepts = DataFacadeManager.Instance.GetDataFacade().List(typeof(SCREN.Concept), filter.GetPredicate());

                if (concepts.Count == 0)
                {
                    throw new BusinessException("Concept not found");
                }

                return (SCREN.Concept)concepts[0];
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptByConceptNameEntityId", ex);
            }

        }
        /// <summary>
        /// obtiene una lista de Concepts sin filtro
        /// </summary>
        /// <returns></returns>
        public static BusinessCollection ListConcepts()
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(SCREN.Concept), null, null));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListConcepts", ex);
            }
        }
        /// <summary>
        /// obtiene una lista de COncepts a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListConcepts(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(SCREN.Concept), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListConcepts", ex);
            }
        }

        /// <summary>
        /// Obtener El Nombre Del Tipo De Concepto
        /// </summary>
        /// <param name="conceptId">Id Concepto</param>
        /// <returns>Nombre Del Tipo De Concepto</returns>
        public static System.Type GetTypeByConceptId(int conceptId, int entityId)
        {
            try
            {
                PrimaryKey key = SCREN.Concept.CreatePrimaryKey(conceptId, entityId);
                SCREN.Concept concept = (SCREN.Concept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (concept != null)
                {
                    object conceptValue = null;

                    switch ((ConceptType.Types)concept.ConceptTypeCode)
                    {
                        case ConceptType.Types.Basic:
                            SCREN.BasicConcept basicConcept = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(concept.ConceptId, concept.EntityId);

                            switch ((BasicType.Types)basicConcept.BasicTypeCode)
                            {
                                case BasicType.Types.Number:
                                    conceptValue = 0;
                                    break;
                                case BasicType.Types.Text:
                                    conceptValue = string.Empty;
                                    break;
                                case BasicType.Types.Decimal:
                                    conceptValue = (decimal)0;
                                    break;
                                case BasicType.Types.Date:
                                    conceptValue = new DateTime();
                                    break;
                                case BasicType.Types.Long:
                                    conceptValue = (long)0;
                                    break;
                                case BasicType.Types.Boolean:
                                    conceptValue = false;
                                    break;
                                default:
                                    conceptValue = null;
                                    break;
                            }
                            break;
                        case ConceptType.Types.Range:
                            conceptValue = 0;
                            break;
                        case ConceptType.Types.List:
                            ListConcept listConcept = ListConceptDAO.GetListConceptByConceptIdEntityId(concept.ConceptId, concept.EntityId);

                            if (listConcept.ListEntityCode == 2)
                            {
                                conceptValue = Convert.ToBoolean(conceptValue);
                            }
                            else
                            {
                                conceptValue = 0;
                            }
                            break;
                        case ConceptType.Types.Reference:
                            conceptValue = 0;
                            break;
                    }

                    return typeof(Nullable<>).MakeGenericType(conceptValue.GetType());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetTypeByConceptId", ex);
            }

        }

        /// <summary>
        /// valida si el concepto esta en uso
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static bool IsInUse(SCREN.Concept concept)
        {
            try
            {
                //SCR.RULE_CONDITION_CONCEPT 
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(RuleConditionConcept.Properties.ConceptId, concept.ConceptId)
                    .And().PropertyEquals(RuleConditionConcept.Properties.EntityId, concept.EntityId);

                var list = RuleConditionConceptDAO.GetRuleConditionConcept(filter.GetPredicate(), new[] { RuleConditionConcept.Properties.RuleBaseId });
                if (list.Count() != 0)
                {
                    return true;
                }

                //SCR.RULE_ACTION_CONCEPT
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(RuleActionConcept.Properties.ConceptId, concept.ConceptId)
                    .And().PropertyEquals(RuleActionConcept.Properties.EntityId, concept.EntityId);

                list = RuleActionConceptDAO.GetRuleActionConcept(filter.GetPredicate(), new[] { RuleActionConcept.Properties.RuleBaseId });
                if (list.Count() != 0)
                {
                    return true;
                }

                //scr.question
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Question.Properties.ConceptId, concept.ConceptId)
                    .And().PropertyEquals(Question.Properties.EntityId, concept.EntityId);

                foreach (Question question in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Question.Properties.Description }))
                {
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(NodeQuestion.Properties.QuestionId, question.QuestionId);

                    if (NodeQuestionDAO.ListNodeQuestion(filter.GetPredicate(), new[] { NodeQuestion.Properties.OrderNum }).Count != 0)
                    {
                        return true;
                    }
                }

                //scr.Edge_Answer
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(EdgeAnswer.Properties.ConceptId, concept.ConceptId);
                if (EdgeAnswerDAO.ListEdgeAnswer(filter.GetPredicate(), new[] { EdgeAnswer.Properties.EdgeId }).Count != 0)
                {
                    return true;
                }

                //scr.rule_set
                NameValue[] pars = new NameValue[2];
                pars[0] = new NameValue("CONCEPT_ID", concept.ConceptId);
                pars[1] = new NameValue("ENTITY_ID", concept.EntityId);

                DataTable result;

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("SCR.COUNT_CONCEPT_RULESET", pars);
                }

                if (result.Rows.Count != 0)
                {
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en IsInUse", ex);
            }
        }

        /// <summary>
        /// valida si la lista esta en uso
        /// </summary>
        /// <param name="listEntityCode"></param>
        /// <returns></returns>
        public static bool IsInUseListEntity(int listEntityCode)
        {
            try
            {
                //Consultar si esta en uso la lista de valores
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ListConcept.Properties.ListEntityCode, listEntityCode);

                var list = ListConceptDAO.ListListConcept(filter.GetPredicate(), new[] { ListConcept.Properties.ListEntityCode });
                if (list.Count != 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en IsInUse", ex);
            }
        }

        /// <summary>
        /// valida si el rango esta en uso 
        /// </summary>
        /// <param name="rangeEntity"></param>
        /// <returns></returns>
        public static bool IsInUseRangeEntity(int rangeEntity)
        {
            try
            {
                //Consultar si esta en uso el rango de valores
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(SCREN.RangeConcept.Properties.RangeEntityCode, rangeEntity);

                var list = RangeConceptDAO.ListRangeConcept(filter.GetPredicate(), new[] { SCREN.RangeConcept.Properties.RangeEntityCode });
                if (list.Count != 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en IsInUse", ex);
            }
        }

        public string ExportConcepts()
        {
            var conceptsService = new ConceptsServiceEEProvider();
            FileDAO commonFileDAO = new FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationConcepts;
            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                //file.Name = fileName;
                List<Row> rows = new List<Row>();
                var conceptList = conceptsService.GetConceptsFile();
                foreach (RuleModel.Concept concept in conceptList)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    var entity = conceptsService.GetEntity(concept.EntityId);
                    fields[0].Value = concept.ConceptId.ToString();
                    fields[1].Value = concept.EntityId.ToString();
                    fields[2].Value = entity.Description.ToString();
                    fields[3].Value = concept.Description.ToString();
                    fields[4].Value = concept.ConceptName.ToString();
                    fields[5].Value = concept.ConceptTypeCode.ToString();
                    fields[6].Value = concept.ConceptControlCode.ToString();
                    fields[7].Value = concept.IsStatic ? "Si" : "No";
                    fields[8].Value = concept.IsVisible ? "Si" : "No";
                    fields[9].Value = concept.IsPersistible ? "Si" : "No";
                    fields[10].Value = concept.IsNull ? "Si" : "No";
                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }
                file.Templates[0].Rows = rows;
                file.Name += "CONCEPTOS_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Obtiene el Concepto por descripción
        /// </summary>
        /// <param name="description">Description de Concepto</param>
        /// <returns>Bool de respuesta</returns>
        public bool? GetConceptsByDescription(string description)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(SCREN.Concept.Properties.Description, typeof(SCREN.Concept).Name);
                filter.Equal();
                filter.Constant(description);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(SCREN.Concept), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
