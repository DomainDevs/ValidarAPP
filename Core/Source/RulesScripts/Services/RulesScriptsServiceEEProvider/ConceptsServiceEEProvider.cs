namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    using Framework.DAF;
    using ModelServices.Enums;
    using ModelServices.Models.Param;
    using Utilities.DataFacade;
    using System.ServiceModel;
    using System.Collections.Generic;
    using Sistran.Core.Application.RulesScriptsServices.Models;
    using System;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
    using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
    using System.Linq;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Framework.Contexts;
    using SCREN = Sistran.Core.Application.Script.Entities;
    //using EnUtil = Sistran.Core.Application.Utilities.Entities;
    using PARAMEN = Sistran.Core.Application.Parameters.Entities;
    using entity = Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
    using Sistran.Core.Application.Utilities.Enums;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ConceptsServiceEEProvider : IConceptsService
    {
        /// <summary>
        /// Obtiene los conceptos segun el filtro 
        /// </summary>
        /// <param name="listEntities">lista de id de entidades</param>
        /// <param name="filter">like de la descripcion</param>
        /// <returns></returns>
        public List<Models._Concept> GetConceptsByFilter(List<int> listEntities, string filter)
        {
            try
            {
                _ConceptDao conceptsDao = new _ConceptDao();
                return conceptsDao.GetConceptByFilter(listEntities, filter);
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptsByFilter", ex);
            }
        }

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public _Concept GetConceptByIdConceptIdEntity(int idConcept, int idEntity)
        {
            try
            {
                _ConceptDao conceptDao = new _ConceptDao();
                return conceptDao.GetConceptByIdConceptIdEntity(idConcept, idEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptsByFilter", ex);
            }
        }

        /// <summary>
        /// Obtiene el concepto especifico con sus respectivos valores
        /// </summary>
        /// <param name="idEntity">id de la entidad</param>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="conceptType">tipo de concepto</param>
        /// <returns></returns>
        public object GetSpecificConceptWithVales(int idConcept, int idEntity, string[] dependency, Enums.ConceptType conceptType)
        {
            try
            {
                _ConceptDao conceptDao = new _ConceptDao();
                return conceptDao.GetSpecificConceptWithVales(idConcept, idEntity, dependency, conceptType);
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptsByFilter", ex);
            }
        }

        public string ExportConcepts()
        {
            try
            {
                ConceptDAO fileDao = new ConceptDAO();
                return fileDao.ExportConcepts();
            }
            catch (Exception e)
            {
                throw new BusinessException("error en ExportConcepts", e);
            }
        }


        /// <summary>
        /// Obtiene el Concepto por descripción
        /// </summary>
        /// <param name="description">Description de Concepto</param>
        /// <returns>Bool de respuesta</returns>
        public bool? GetConceptsByDescription(string description)
        {
            ConceptDAO conceptDAO = new ConceptDAO();
            bool? respuesta = conceptDAO.GetConceptsByDescription(description);
            return respuesta;
        }










        /***********************************/
        //ANTIGUO
        /***********************************/
        #region Antiguo
        /// <summary>
        /// obtiene todos los conceptos 
        /// </summary>
        /// <returns></returns>
        public List<Concept> GetConcepts()
        {
            try
            {
                List<Concept> result = new List<Concept>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(SCREN.Concept.Properties.EntityId).In().ListValue();


                foreach (string facadeName in Enum.GetNames(typeof(FacadeType)))
                {
                    FacadeType facadeTypes = (FacadeType)Enum.Parse(typeof(FacadeType), facadeName);
                    filter.Constant((int)Utilities.Helper.EnumHelper.GetEnumParameterValue(facadeTypes));
                }
                filter.EndList();


                var list = ConceptDAO.ListConcepts(filter.GetPredicate(), new[] { SCREN.Concept.Properties.Description });


                foreach (SCREN.Concept item in list)
                {
                    if (!item.Description.Contains("z(No Usar)"))
                    {
                        var concept = ModelAssembler.CreateConcept(item);

                        result.Add(concept);
                    }
                }
                return result.OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConcepts", ex);
            }
        }

        /// <summary>
        /// obtiene todos los conceptos del Excel
        /// </summary>
        /// <returns></returns>
        public List<Concept> GetConceptsFile()
        {
            try
            {
                List<Concept> result = new List<Concept>();


                var list = ConceptDAO.ListConcepts();


                foreach (SCREN.Concept item in list)
                {
                    var concept = ModelAssembler.CreateConcept(item);
                    result.Add(concept);
                }
                return result.OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConcepts", ex);
            }
        }

        public List<Concept> GetConceptsByIdModuleIdLevelDescription(int? idModule, int? idLevel, int filter, string description)
        {
            try
            {
                List<Concept> result = new List<Concept>();
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

                var list = ConceptDAO.GetConceptsByIdModuleIdLevelDescription(idModule, idLevel, filter, description);

                foreach (SCREN.Concept item in list)
                {
                    if (!item.Description.Contains("z(No Usar)"))
                    {
                        var concept = ModelAssembler.CreateConcept(item);
                        where = new ObjectCriteriaBuilder();
                        where.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                                  .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);

                        var listQuestion = QuestionDAO.ListQuestions(where.GetPredicate(), new[] { Entities.Question.Properties.Description });
                        foreach (Entities.Question itemQuestion in listQuestion)
                        {
                            concept.Question = ModelAssembler.CreateQuestion(itemQuestion);
                            break;
                        }

                        switch (concept.ConceptTypeCode)
                        {
                            case Enums.ConceptType.Basic:
                                where = new ObjectCriteriaBuilder();
                                where.PropertyEquals(Entities.BasicConcept.Properties.ConceptId, concept.ConceptId)
                                    .And().PropertyEquals(Entities.BasicConcept.Properties.EntityId, concept.EntityId);

                                var listResutl = BasicConceptDAO.List(where.GetPredicate(), new[] { Entities.BasicConcept.Properties.ConceptId });
                                foreach (Entities.BasicConcept itemBasicConcept in listResutl)
                                {
                                    ((BasicConcept)concept).BasicTypeCode = (Enums.BasicType)itemBasicConcept.BasicTypeCode;
                                    break;
                                }
                                result.Add(concept);
                                break;
                            case Enums.ConceptType.Range:
                                ((RangeConcept)concept).EntityValues = new List<RangeEntityValue>();
                                var RangeConcept = RangeConceptDAO.FindRangeConcept(concept.ConceptId, concept.EntityId);

                                where = new ObjectCriteriaBuilder();
                                where.PropertyEquals(Entities.RangeEntityValue.Properties.RangeEntityCode, RangeConcept.RangeEntityCode);

                                foreach (Entities.RangeEntityValue itemRangeValue in RangeEntityValueDAO.ListRangeEntityValue(where.GetPredicate(), new[] { Entities.RangeEntityValue.Properties.RangeEntityCode }))
                                {
                                    ((RangeConcept)concept).EntityValues.Add(new RangeEntityValue()
                                    {
                                        RangeEntityCode = itemRangeValue.RangeEntityCode,
                                        RangeValueCode = itemRangeValue.RangeValueCode,
                                        FromValue = itemRangeValue.FromValue,
                                        ToValue = itemRangeValue.ToValue,
                                        StatusTypeService = StatusTypeService.Original
                                    });
                                }
                                ((RangeConcept)concept).RangeEntityCode = RangeConcept.RangeEntityCode;
                                result.Add(concept);
                                break;
                            case Enums.ConceptType.List:
                                ((ListConcept)concept).EntityValues = new List<EntityValue>();
                                var listConcept = ListConceptDAO.FindListConcept(concept.ConceptId, concept.EntityId);

                                where = new ObjectCriteriaBuilder();
                                where.PropertyEquals(SCREN.ListEntityValue.Properties.ListEntityCode, listConcept.ListEntityCode);

                                foreach (SCREN.ListEntityValue itemListValue in ListEntityValueDAO.ListListEntityValue(where.GetPredicate(), new[] { SCREN.ListEntityValue.Properties.ListEntityCode }))
                                {
                                    ((ListConcept)concept).EntityValues.Add(new EntityValue()
                                    {
                                        EntityValueCode = itemListValue.ListValueCode,
                                        Value = itemListValue.ListValue,
                                    });
                                }
                                ((ListConcept)concept).ListEntityCode = listConcept.ListEntityCode;
                                result.Add(concept);
                                break;
                            case Enums.ConceptType.Reference:
                                ((ReferenceConcept)concept).EntityValues = new List<EntityValue>();
                                var ReferenceConcept = ReferenceConceptDAO.FindReferenceConcept(concept.ConceptId, concept.EntityId);

                                ((ReferenceConcept)concept).FEntityId = ReferenceConcept == null ? 0 : ReferenceConcept.FentityId;
                                result.Add(concept);
                                break;
                        }

                    }
                }
                return result.OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConcepts", ex);
            }
        }

        /// <summary>
        /// Guarda los cambios realizados para los conceptos
        /// </summary>
        /// <param name="ConceptBasicAdd">lista de conceptos basicos a crear</param>
        /// <param name="ConceptBasicEdit">lista de conceptos basicos a editar</param>
        /// <param name="ConceptBasicDelete">lista de conceptos basicos a eliminar</param>
        /// <param name="ConceptListAdd">lista de conceptos lista a crear</param>
        /// <param name="ConceptListEdit">lista de conceptos lista a editar</param>
        /// <param name="ConceptListDelete">lista de conceptos lista a eliminar</param>
        /// <param name="ConceptRangeAdd">lista de conceptos rango a crear</param>
        /// <param name="ConceptRangeEdit">lista de conceptos rango a editar</param>
        /// <param name="ConceptRangeDelete">lista de conceptos rango a eliminar</param>
        /// <param name="ConceptReferenceAdd">lista de conceptos referencia a crear</param>
        /// <param name="ConceptReferenceEdit">lista de conceptos referencia a editar</param>
        /// <param name="ConceptReferenceDelete">lista de conceptos referencia a eliminar</param>
        /// <returns></returns>
        public void SaveConcepts(
            List<BasicConcept> ConceptBasicAdd, List<BasicConcept> ConceptBasicEdit, List<BasicConcept> ConceptBasicDelete,
            List<ListConcept> ConceptListAdd, List<ListConcept> ConceptListEdit, List<ListConcept> ConceptListDelete,
            List<RangeConcept> ConceptRangeAdd, List<RangeConcept> ConceptRangeEdit, List<RangeConcept> ConceptRangeDelete,
            List<ReferenceConcept> ConceptReferenceAdd, List<ReferenceConcept> ConceptReferenceEdit, List<ReferenceConcept> ConceptReferenceDelete
        )
        {
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e) { };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e) { };
                    try
                    {
                        CreateBasicConcept(ConceptBasicAdd);
                        UpdateBasicConcept(ConceptBasicEdit);
                        //DeleteBasicConcept(ConceptBasicDelete);

                        CreateListConcept(ConceptListAdd);
                        UpdateListConcept(ConceptListEdit);
                        //DeleteListConcept(ConceptListDelete);

                        CreateRangeConcept(ConceptRangeAdd);
                        UpdateRangeConcept(ConceptRangeEdit);
                        //DeleteRangeConcept(ConceptRangeDelete);


                        CreateReferenceConcept(ConceptReferenceAdd);
                        UpdateReferenceConcept(ConceptReferenceEdit);
                        //DeleteReferenceConcept(ConceptReferenceDelete);

                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// crea los conceptos de tipo basico
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void CreateBasicConcept(List<BasicConcept> concepts)
        {
            try
            {
                foreach (BasicConcept concept in concepts)
                {
                    SCREN.Concept conceptEntity = EntityAssembler.CreateConcept(concept);
                    concept.ConceptId = ConceptDAO.CreateConcept(conceptEntity).ConceptId;
                    BasicConceptDAO.Create(new Entities.BasicConcept(concept.ConceptId, concept.EntityId) { BasicTypeCode = (int)concept.BasicTypeCode });

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Entities.BasicConceptCheck.Properties.ConceptId, concept.ConceptId)
                        .And().PropertyEquals(Entities.BasicConceptCheck.Properties.EntityId, concept.EntityId);

                    foreach (Entities.BasicConceptCheck item in BasicConceptCheckDAO.List(filter.GetPredicate(), new[] { Entities.BasicConceptCheck.Properties.ConceptId }))
                    {
                        BasicConceptCheckDAO.Delete(item);
                    }

                    switch (concept.BasicTypeCode)
                    {
                        case Enums.BasicType.Numeric:
                        case Enums.BasicType.Decimal:

                            if (concept.MaxValue != null || concept.MinValue != null)
                            {
                                if (concept.MaxValue != null)
                                {
                                    BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(1) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, IntValue = (int)concept.MaxValue });
                                }

                                if (concept.MinValue != null)
                                {
                                    BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(2) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, IntValue = (int)concept.MinValue });
                                }
                            }
                            break;
                        case Enums.BasicType.Text:
                            if (concept.Length != null)
                            {
                                BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(3)
                                {
                                    ConceptId = concept.ConceptId,
                                    EntityId = concept.EntityId,
                                    IntValue = (int)concept.Length
                                });
                            }
                            break;
                        case Enums.BasicType.Date:
                            if (concept.MaxDate != null || concept.MinDate != null)
                            {
                                if (concept.MaxDate != null)
                                {
                                    BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(1) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, DateValue = (DateTime)concept.MaxDate });
                                }

                                if (concept.MinDate != null)
                                {
                                    BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(2) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, DateValue = (DateTime)concept.MinDate });
                                }
                            }
                            break;
                    }

                    if (concept.Question != null)
                    {
                        QuestionDAO.CreateQuestion(new Entities.Question
                        {
                            ConceptId = concept.ConceptId,
                            EntityId = concept.EntityId,
                            Description = concept.Question.Description
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// actualiza los conceptos de tipo basico
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void UpdateBasicConcept(List<BasicConcept> concepts)
        {
            try
            {

                foreach (BasicConcept concept in concepts)
                {
                    var entityConcept = ConceptDAO.GetConcept(EntityAssembler.CreateConceptEdit(concept));
                    entityConcept.Description = concept.Description;
                    if (!entityConcept.IsStatic)
                    {
                        entityConcept.ConceptName = concept.Description;
                        entityConcept.IsNullable = concept.IsNull;
                        entityConcept.IsPersistible = concept.IsPersistible;
                        entityConcept.IsVisible = concept.IsVisible;
                    }
                    ConceptDAO.UpdateConcept(entityConcept);

                    //var basicConcept = BasicConceptDAO.GetBasicConceptByEntityIdConceptId(concept.ConceptId, concept.EntityId);
                    //basicConcept.BasicTypeCode = (int)concept.BasicTypeCode;
                    //BasicConceptDAO.Update(basicConcept);

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    //filter.PropertyEquals(Entities.BasicConceptCheck.Properties.ConceptId, concept.ConceptId)
                    //    .And().PropertyEquals(Entities.BasicConceptCheck.Properties.EntityId, concept.EntityId);

                    //foreach (Entities.BasicConceptCheck item in BasicConceptCheckDAO.List(filter.GetPredicate(), new[] { Entities.BasicConceptCheck.Properties.ConceptId }))
                    //{
                    //    BasicConceptCheckDAO.Delete(item);
                    //}

                    //switch (concept.BasicTypeCode)
                    //{
                    //    case Enums.BasicType.Numeric:
                    //    case Enums.BasicType.Decimal:

                    //        if (concept.MaxValue != null || concept.MinValue != null)
                    //        {
                    //            if (concept.MaxValue != null)
                    //            {
                    //                BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(1) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, IntValue = (int)concept.MaxValue });
                    //            }

                    //            if (concept.MinValue != null)
                    //            {
                    //                BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(2) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, IntValue = (int)concept.MinValue });
                    //            }
                    //        }
                    //        break;
                    //    case Enums.BasicType.Text:
                    //        if (concept.Length != null)
                    //        {
                    //            BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(3)
                    //            {
                    //                ConceptId = concept.ConceptId,
                    //                EntityId = concept.EntityId,
                    //                IntValue = (int)concept.Length
                    //            });
                    //        }
                    //        break;
                    //    case Enums.BasicType.Date:
                    //        if (concept.MaxDate != null || concept.MinDate != null)
                    //        {
                    //            if (concept.MaxDate != null)
                    //            {
                    //                BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(1) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, DateValue = (DateTime)concept.MaxDate });
                    //            }

                    //            if (concept.MinDate != null)
                    //            {
                    //                BasicConceptCheckDAO.Create(new Entities.BasicConceptCheck(2) { ConceptId = concept.ConceptId, EntityId = concept.EntityId, DateValue = (DateTime)concept.MinDate });
                    //            }
                    //        }
                    //        break;
                    //}

                    if (concept.Question == null)
                    {
                        filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                        foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                        {
                            QuestionDAO.DeleteQuestion(item);
                        }
                    }
                    else
                    {
                        if (concept.Question.QuestionId != 0)
                        {
                            filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                                .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                            foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                            {
                                item.Description = concept.Question.Description;
                                item.ConceptId = concept.ConceptId;
                                item.EntityId = concept.EntityId;
                                QuestionDAO.UpdateQuestion(item);
                            }
                        }
                        else
                        {
                            filter = new ObjectCriteriaBuilder();
                            int idQuestion = ModelAssembler.CreateQuestions(QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description })).Max(x => x.QuestionId) + 1;
                            QuestionDAO.CreateQuestion(new Entities.Question(idQuestion)
                            {
                                ConceptId = concept.ConceptId,
                                EntityId = concept.EntityId,
                                Description = concept.Question.Description
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// elimina los conceptos de tipo basico
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private bool DeleteBasicConcept(List<BasicConcept> concepts)
        {
            try
            {
                foreach (Concept concept in concepts)
                {
                    var entity = new SCREN.Concept(concept.ConceptId, concept.EntityId);

                    SCREN.Concept conceptEntity = ConceptDAO.GetConcept(entity);
                    ConceptDAO.DeleteConcept(conceptEntity);

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new BusinessException("error en DeleteConcept", ex);
            }
        }

        /// <summary>
        /// crea los conceptos de tipo lista
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void CreateListConcept(List<ListConcept> concepts)
        {
            try
            {
                foreach (ListConcept concept in concepts)
                {
                    SCREN.Concept conceptEntity = EntityAssembler.CreateConcept(concept);
                    concept.ConceptId = ConceptDAO.CreateConcept(conceptEntity).ConceptId;
                    ListConceptDAO.CreateListConcept(new Entities.ListConcept(concept.ConceptId, concept.EntityId) { ListEntityCode = concept.ListEntityCode });

                    if (concept.Question != null)
                    {
                        QuestionDAO.CreateQuestion(new Entities.Question
                        {
                            ConceptId = concept.ConceptId,
                            EntityId = concept.EntityId,
                            Description = concept.Question.Description
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// edita los conceptos de tipo lista
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void UpdateListConcept(List<ListConcept> concepts)
        {
            try
            {
                foreach (ListConcept concept in concepts)
                {
                    var entityConcept = ConceptDAO.GetConcept(EntityAssembler.CreateConceptEdit(concept));
                    entityConcept.Description = concept.Description;
                    if (!entityConcept.IsStatic)
                    {
                        entityConcept.ConceptName = concept.Description;
                        entityConcept.IsNullable = concept.IsNull;
                        entityConcept.IsPersistible = concept.IsPersistible;
                        entityConcept.IsVisible = concept.IsVisible;
                    }
                    ConceptDAO.UpdateConcept(entityConcept);

                    //var listConcept = ListConceptDAO.GetListConceptByConceptIdEntityId(concept.ConceptId, concept.EntityId);
                    //listConcept.ListEntityCode = concept.ListEntityCode;
                    //ListConceptDAO.UpdateListConcept(listConcept);

                    if (concept.Question == null)
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                        foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                        {
                            QuestionDAO.DeleteQuestion(item);
                        }
                    }
                    else
                    {
                        if (concept.Question.QuestionId != 0)
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                                .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                            foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                            {
                                item.Description = concept.Question.Description;
                                item.ConceptId = concept.ConceptId;
                                item.EntityId = concept.EntityId;
                                QuestionDAO.UpdateQuestion(item);
                            }
                        }
                        else
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            int idQuestion = ModelAssembler.CreateQuestions(QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description })).Max(x => x.QuestionId) + 1;
                            QuestionDAO.CreateQuestion(new Entities.Question(idQuestion)
                            {
                                ConceptId = concept.ConceptId,
                                EntityId = concept.EntityId,
                                Description = concept.Question.Description
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// elimina los conceptos de tipo lista
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private bool DeleteListConcept(List<ListConcept> concepts)
        {
            try
            {
                foreach (Concept concept in concepts)
                {
                    var entity = new SCREN.Concept(concept.ConceptId, concept.EntityId);

                    SCREN.Concept conceptEntity = ConceptDAO.GetConcept(entity);
                    ConceptDAO.DeleteConcept(conceptEntity);

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new BusinessException("error en DeleteConcept", ex);
            }
        }

        /// <summary>
        /// crea los conceptos de tipo rango
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void CreateRangeConcept(List<RangeConcept> concepts)
        {
            try
            {
                foreach (RangeConcept concept in concepts)
                {
                    SCREN.Concept conceptEntity = EntityAssembler.CreateConcept(concept);
                    concept.ConceptId = ConceptDAO.CreateConcept(conceptEntity).ConceptId;

                    RangeConceptDAO.CreateRangeConcept(new SCREN.RangeConcept(concept.ConceptId, concept.EntityId) { RangeEntityCode = concept.RangeEntityCode });

                    if (concept.Question != null)
                    {
                        QuestionDAO.CreateQuestion(new Entities.Question
                        {
                            ConceptId = concept.ConceptId,
                            EntityId = concept.EntityId,
                            Description = concept.Question.Description
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// edita los conceptos de tipo rango
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void UpdateRangeConcept(List<RangeConcept> concepts)
        {
            try
            {
                foreach (RangeConcept concept in concepts)
                {
                    var entityConcept = ConceptDAO.GetConcept(EntityAssembler.CreateConceptEdit(concept));
                    entityConcept.Description = concept.Description;
                    if (!entityConcept.IsStatic)
                    {
                        entityConcept.ConceptName = concept.Description;
                        entityConcept.IsNullable = concept.IsNull;
                        entityConcept.IsPersistible = concept.IsPersistible;
                        entityConcept.IsVisible = concept.IsVisible;
                    }
                    ConceptDAO.UpdateConcept(entityConcept);

                    //var rangeConcept = RangeConceptDAO.FindRangeConcept(concept.ConceptId, concept.EntityId);
                    //rangeConcept.RangeEntityCode = concept.RangeEntityCode;
                    //RangeConceptDAO.UpdateRangeConcept(rangeConcept);

                    if (concept.Question == null)
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                        foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                        {
                            QuestionDAO.DeleteQuestion(item);
                        }
                    }
                    else
                    {
                        if (concept.Question.QuestionId != 0)
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                                .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                            foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                            {
                                item.Description = concept.Question.Description;
                                item.ConceptId = concept.ConceptId;
                                item.EntityId = concept.EntityId;
                                QuestionDAO.UpdateQuestion(item);
                            }
                        }
                        else
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            int idQuestion = ModelAssembler.CreateQuestions(QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description })).Max(x => x.QuestionId) + 1;
                            QuestionDAO.CreateQuestion(new Entities.Question(idQuestion)
                            {
                                ConceptId = concept.ConceptId,
                                EntityId = concept.EntityId,
                                Description = concept.Question.Description
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// elimina los conceptos de tipo rango
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private bool DeleteRangeConcept(List<RangeConcept> concepts)
        {
            try
            {
                foreach (Concept concept in concepts)
                {
                    var entity = new SCREN.Concept(concept.ConceptId, concept.EntityId);

                    SCREN.Concept conceptEntity = ConceptDAO.GetConcept(entity);
                    ConceptDAO.DeleteConcept(conceptEntity);

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new BusinessException("error en DeleteConcept", ex);
            }
        }

        /// <summary>
        /// crea los conceptos de tipo referencia
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void CreateReferenceConcept(List<ReferenceConcept> concepts)
        {
            try
            {
                foreach (ReferenceConcept concept in concepts)
                {
                    SCREN.Concept conceptEntity = EntityAssembler.CreateConcept(concept);
                    concept.ConceptId = ConceptDAO.CreateConcept(conceptEntity).ConceptId;

                    ReferenceConceptDAO.CreateReferenceConcept(new Entities.ReferenceConcept(concept.ConceptId, concept.EntityId) { FentityId = concept.FEntityId });

                    if (concept.Question != null)
                    {
                        QuestionDAO.CreateQuestion(new Entities.Question
                        {
                            ConceptId = concept.ConceptId,
                            EntityId = concept.EntityId,
                            Description = concept.Question.Description
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// edita los conceptos de tipo referencia
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private void UpdateReferenceConcept(List<ReferenceConcept> concepts)
        {
            try
            {
                foreach (ReferenceConcept concept in concepts)
                {
                    var entityConcept = ConceptDAO.GetConcept(EntityAssembler.CreateConceptEdit(concept));
                    entityConcept.Description = concept.Description;
                    if (!entityConcept.IsStatic)
                    {
                        entityConcept.ConceptName = concept.Description;
                        entityConcept.IsNullable = concept.IsNull;
                        entityConcept.IsPersistible = concept.IsPersistible;
                        entityConcept.IsVisible = concept.IsVisible;
                    }
                    ConceptDAO.UpdateConcept(entityConcept);

                    //var referenceConcept = ReferenceConceptDAO.FindReferenceConcept(concept.ConceptId, concept.EntityId);
                    //referenceConcept.FentityId = concept.FEntityId;
                    //ReferenceConceptDAO.UpdateReferenceConcept(referenceConcept);

                    if (concept.Question == null)
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                            .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                        foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                        {
                            QuestionDAO.DeleteQuestion(item);
                        }
                    }
                    else
                    {
                        if (concept.Question.QuestionId != 0)
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(Entities.Question.Properties.ConceptId, concept.ConceptId)
                                .And().PropertyEquals(Entities.Question.Properties.EntityId, concept.EntityId);
                            foreach (Entities.Question item in QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description }))
                            {
                                item.Description = concept.Question.Description;
                                item.ConceptId = concept.ConceptId;
                                item.EntityId = concept.EntityId;
                                QuestionDAO.UpdateQuestion(item);
                            }
                        }
                        else
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            int idQuestion = ModelAssembler.CreateQuestions(QuestionDAO.ListQuestions(filter.GetPredicate(), new[] { Entities.Question.Properties.Description })).Max(x => x.QuestionId) + 1;
                            QuestionDAO.CreateQuestion(new Entities.Question(idQuestion)
                            {
                                ConceptId = concept.ConceptId,
                                EntityId = concept.EntityId,
                                Description = concept.Question.Description
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CreateConcept", ex);
            }
        }

        /// <summary>
        /// elimina los conceptos de tipo referencia
        /// </summary>
        /// <param name="concepts">lista de conceptos</param>
        private bool DeleteReferenceConcept(List<ReferenceConcept> concepts)
        {
            try
            {
                foreach (Concept concept in concepts)
                {
                    var entity = new SCREN.Concept(concept.ConceptId, concept.EntityId);

                    SCREN.Concept conceptEntity = ConceptDAO.GetConcept(entity);
                    ConceptDAO.DeleteConcept(conceptEntity);

                }
                return true;

            }
            catch (Exception ex)
            {
                throw new BusinessException("error en DeleteConcept", ex);
            }
        }

        /// <summary>
        /// valida si el concepto esta siendo usado 
        /// </summary>
        /// <param name="conceptSrt">concepto serializado</param>
        /// <returns></returns>
        public bool IsInUse(Concept concept)
        {
            try
            {
                var entity = new SCREN.Concept(concept.ConceptId, concept.EntityId);
                return ConceptDAO.IsInUse(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en DeleteConcept", ex);
            }
        }

        /// <summary>
        /// obtiene los tipos de conceptos
        /// </summary>
        /// <returns></returns>
        public List<ConceptType> GetConceptTypes()
        {
            try
            {
                var ConceptTypeList = ConceptTypeDAO.GetConceptType(null, new[] { Entities.ConceptType.Properties.Description });

                List<ConceptType> list = new List<ConceptType>();

                foreach (Entities.ConceptType item in ConceptTypeList)
                {
                    list.Add(ModelAssembler.CreateConceptType(item));
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptTypes", ex);
            }
        }

        /// <summary>
        /// obtiene los tipos de conceptos basicos
        /// </summary>
        /// <returns></returns>
        public List<BasicType> GetBasicTypes()
        {
            try
            {
                var listEntity = ConceptTypeDAO.GetBasicType(null, new[] { Entities.BasicType.Properties.Description });

                List<BasicType> list = new List<BasicType>();

                foreach (Entities.BasicType item in listEntity)
                {
                    list.Add(ModelAssembler.CreateBasicType(item));
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptTypes", ex);
            }
        }

        /// <summary>
        /// obtiene los valores para los tipos de conceptos basicos
        /// </summary>
        /// <param name="conceptSrt">concepto basico serializado</param>
        /// <returns></returns>
        public BasicConcept GetBasicConceptsValues(BasicConcept concept)
        {
            try
            {
                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.PropertyEquals(Entities.BasicConceptCheck.Properties.ConceptId, concept.ConceptId)
                    .And().PropertyEquals(Entities.BasicConceptCheck.Properties.EntityId, concept.EntityId);

                var listResutl = BasicConceptCheckDAO.List(where.GetPredicate(), new[] { Entities.BasicConceptCheck.Properties.ConceptId });

                if (listResutl.Count != 0)
                {
                    foreach (Entities.BasicConceptCheck itemBasicConcept in listResutl)
                    {
                        if (itemBasicConcept.BasicCheckCode == 1)
                        {
                            if (itemBasicConcept.IsDateValueNull)
                            {
                                concept.MaxValue = itemBasicConcept.IntValue;
                            }
                            else
                            {
                                concept.MaxDate = itemBasicConcept.DateValue;
                            }
                        }
                        else if (itemBasicConcept.BasicCheckCode == 2)
                        {
                            if (itemBasicConcept.IsDateValueNull)
                            {
                                concept.MinValue = itemBasicConcept.IntValue;
                            }
                            else
                            {
                                concept.MinDate = itemBasicConcept.DateValue;
                            }
                        }
                        else if (itemBasicConcept.BasicCheckCode == 3)
                        {
                            concept.Length = itemBasicConcept.IntValue;
                        }
                    }
                    return concept;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetBasicConceptsValues", ex);
            }
        }

        /// <summary>
        /// Otiene la entidad por IdEntity
        /// </summary>
        /// <param name="EntityId">Id de la entidad</param>
        /// <returns></returns>
        public Entity GetEntity(int EntityId)
        {
            try
            {
                return ModelAssembler.CreateEntity(EntityDAO.FindEntity(EntityId));
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetEntity", ex);
            }
        }

        /// <summary>
        /// obtiene las entidades de tipo referencia
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetForeignEntities(int packageId, int levelId)
        {
            try
            {
                List<Entity> listEntities = new List<Entity>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PARAMEN.Entity.Properties.PropertySearch).IsNotNull();
                //    .And().Property(PARAMEN.Entity.Properties.PackageId).Equal().Constant(packageId)
                //    .And().Property(PARAMEN.Entity.Properties.LevelId).Equal().Constant(levelId);

                foreach (PARAMEN.Entity item in EntityDAO.ListEntity(filter.GetPredicate(), new[] { PARAMEN.Entity.Properties.Description }))
                {
                    listEntities.Add(ModelAssembler.CreateEntity(item));
                }

                return listEntities;
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetEntity", ex);
            }
        }

        /// <summary>
        /// Obtiene la entidad dependiendo el modulo y el nivel
        /// </summary>
        /// <param name="IdPackage">modulo Id</param>
        /// <param name="IdLevel">nivel Id</param>
        /// <returns></returns>
        public Entity GetEntityByIdPackageIdLevel(int IdPackage, int IdLevel)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(PARAMEN.Entity.Properties.PackageId, IdPackage)
                    .And().PropertyEquals(PARAMEN.Entity.Properties.LevelId, IdLevel)
                     .And().Property(PARAMEN.Entity.Properties.Description).Like().Constant("Facade%");


                var list = EntityDAO.ListEntity(filter.GetPredicate(), new[] { PARAMEN.Entity.Properties.EntityId });

                foreach (PARAMEN.Entity item in list)
                {
                    return ModelAssembler.CreateEntity(item);
                }

                throw new BusinessException("Error en obtener EntityDAO.ListEntity");
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetEntityByIdPackageIdLevel", ex);
            }
        }


        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public List<Models.ListEntity> GetListEntity()
        {
            try
            {
                return ListEntityDAO.GetListEntity().OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntity", ex);
            }
        }

        /// <summary>
        /// Obtener las listas de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public List<Models.ListEntity> GetListEntityByDescription(string Description)
        {
            try
            {
                return ListEntityDAO.GetListEntityByDescription(Description);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntityByDescription", ex);
            }
        }

        /// <summary>
        /// Obtener los valores de lista por código de lista de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public List<Models.ListEntity> GetListEntityValueByListEntityCode(int listEntityCode)
        {
            try
            {
                return ListEntityDAO.GetListEntityValueByListEntityCode(listEntityCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntityValueByListEntityCode", ex);
            }
        }

        public List<ListEntity> ExecuteOperationListEntities(List<ListEntity> listEntity)
        {
            for (int index = 0; index < listEntity.Count; index++)
            {
                ListEntity list = listEntity[index];

                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        switch (list.StatusTypeService)
                        {
                            case StatusTypeService.Create:
                                list = ListEntityDAO.CreateListEntity(list);
                                list = this.ExecuteOperationListEntitiesValues(list);
                                break;

                            case StatusTypeService.Update:
                                if (list.ListEntityValue.Count(x => x.StatusTypeService == StatusTypeService.Delete) > 0)
                                {
                                    if (ConceptDAO.IsInUseListEntity(list.ListEntityCode))
                                    {
                                        throw new BusinessException(string.Format(Resources.Errors.MsgListEntityValueIsInUse, list.Description));
                                    }
                                }

                                list = ListEntityDAO.UpdateListEntity(list);
                                list = this.ExecuteOperationListEntitiesValues(list);
                                break;

                            case StatusTypeService.Delete:
                                if (!ConceptDAO.IsInUseListEntity(list.ListEntityCode))
                                {
                                    list.ListEntityValue.ForEach(x => x.StatusTypeService = StatusTypeService.Delete);
                                    list = this.ExecuteOperationListEntitiesValues(list);
                                    ListEntityDAO.DeleteListEntity(list);
                                }
                                else
                                {
                                    throw new BusinessException(string.Format(Resources.Errors.MsgListEntityIsInUse, list.Description));
                                }

                                break;
                        }

                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        list.StatusTypeService = StatusTypeService.Error;
                        list.ErrorServiceModel = new ErrorServiceModel
                        {
                            ErrorTypeService = ErrorTypeService.TechnicalFault,
                            ErrorDescription = new List<string> { ex.Message }
                        };

                        listEntity[index] = list;
                    }
                }
            }

            return listEntity;
        }

        private ListEntity ExecuteOperationListEntitiesValues(ListEntity listEntities)
        {
            int maxCode = 1;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(SCREN.ListEntityValue.Properties.ListEntityCode, typeof(SCREN.ListEntityValue).Name).Equal().Constant(listEntities.ListEntityCode);
            List<SCREN.ListEntityValue> listEntityValues = ListEntityValueDAO.ListListEntityValue(filter.GetPredicate(), new[] { SCREN.ListEntityValue.Properties.ListValueCode }).Cast<SCREN.ListEntityValue>().ToList();

            if (listEntityValues.Count > 0)
            {
                maxCode = listEntityValues.Max(x => x.ListValueCode) + 1;
            }

            for (int index = 0; index < listEntities.ListEntityValue.Count; index++)
            {
                ListEntityValue listEntityValue = listEntities.ListEntityValue[index];
                listEntityValue.ListEntityCode = listEntities.ListEntityCode;

                switch (listEntityValue.StatusTypeService)
                {
                    case StatusTypeService.Create:
                        listEntityValue.ListValueCode = maxCode++;
                        listEntityValue = ListEntityValueDAO.CreateListEntityValue(listEntityValue);
                        break;

                    case StatusTypeService.Update:
                        listEntityValue = ListEntityValueDAO.UpdateListEntityValue(listEntityValue);
                        break;

                    case StatusTypeService.Delete:
                        ListEntityValueDAO.DeleteListEntityValue(listEntityValue);
                        break;
                }

                listEntities.ListEntityValue[index] = listEntityValue;
            }

            return listEntities;
        }



        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de rangos de valores</returns>
        public List<Models.RangeEntity> GetRangeEntity()
        {
            try
            {
                return RangeEntityDAO.GetRangeEntity().OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRangeEntity", ex);
            }
        }

        /// <summary>
        /// Obtener los rangos de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public List<Models.RangeEntity> GetRangeEntityByDescription(string Description)
        {
            try
            {
                return RangeEntityDAO.GetRangeEntityByDescription(Description);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRangeEntityByDescription", ex);
            }
        }

        /// <summary>
        /// Obtener los rangos de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// lista de listas de valores
        /// </returns>
        public List<Models.RangeEntity> GetRangeEntityValueByRangeEntityCode(int rangeEntityCode)
        {
            try
            {
                return RangeEntityDAO.GetRangeEntityValueByRangeEntityCode(rangeEntityCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntityValueByListEntityCode", ex);
            }
        }

        public List<RangeEntity> ExecuteOperationRangeEntities(List<RangeEntity> rangeEntities)
        {
            for (int index = 0; index < rangeEntities.Count; index++)
            {
                RangeEntity range = rangeEntities[index];

                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        switch (range.StatusTypeService)
                        {
                            case StatusTypeService.Create:
                                range = RangeEntityDAO.CreateRangeEntity(range);
                                range = this.ExecuteOperationRangeEntitiesValues(range);
                                break;

                            case StatusTypeService.Update:
                                if (range.RangeEntityValue.Count(x => x.StatusTypeService == StatusTypeService.Delete) > 0)
                                {
                                    if (ConceptDAO.IsInUseRangeEntity(range.RangeEntityCode))
                                    {
                                        throw new BusinessException(string.Format(Resources.Errors.MsgRangeEntityValueIsInUse, range.Description));
                                    }
                                }

                                range = RangeEntityDAO.UpdateRangeEntity(range);
                                range = this.ExecuteOperationRangeEntitiesValues(range);
                                break;

                            case StatusTypeService.Delete:
                                if (!ConceptDAO.IsInUseRangeEntity(range.RangeEntityCode))
                                {
                                    range.RangeEntityValue.ForEach(x => x.StatusTypeService = StatusTypeService.Delete);
                                    range = this.ExecuteOperationRangeEntitiesValues(range);
                                    RangeEntityDAO.DeleteRangeEntity(range);
                                }
                                else
                                {
                                    throw new BusinessException(string.Format(Resources.Errors.MsgRangeEntityIsInUse, range.Description));
                                }

                                break;
                        }

                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        range.StatusTypeService = StatusTypeService.Error;
                        range.ErrorServiceModel = new ErrorServiceModel
                        {
                            ErrorTypeService = ErrorTypeService.TechnicalFault,
                            ErrorDescription = new List<string> { ex.Message }
                        };

                        rangeEntities[index] = range;
                    }
                }
            }

            return rangeEntities;
        }

        private RangeEntity ExecuteOperationRangeEntitiesValues(RangeEntity rangeEntity)
        {
            int maxCode = 1;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Entities.RangeEntityValue.Properties.RangeEntityCode, typeof(Entities.RangeEntityValue).Name).Equal().Constant(rangeEntity.RangeEntityCode);
            List<Entities.RangeEntityValue> rangeEntityValues = RangeEntityValueDAO.ListRangeEntityValue(filter.GetPredicate(), new[] { Entities.RangeEntityValue.Properties.RangeValueCode }).Cast<Entities.RangeEntityValue>().ToList();

            if (rangeEntityValues.Count > 0)
            {
                maxCode = rangeEntityValues.Max(x => x.RangeValueCode) + 1;
            }

            for (int index = 0; index < rangeEntity.RangeEntityValue.Count; index++)
            {
                RangeEntityValue rangeEntityValue = rangeEntity.RangeEntityValue[index];
                rangeEntityValue.RangeEntityCode = rangeEntity.RangeEntityCode;

                switch (rangeEntityValue.StatusTypeService)
                {
                    case StatusTypeService.Create:
                        rangeEntityValue.RangeValueCode = maxCode++;
                        rangeEntityValue = RangeEntityValueDAO.CreateRangeEntityValue(rangeEntityValue);
                        break;

                    case StatusTypeService.Update:
                        rangeEntityValue = RangeEntityValueDAO.UpdateRangeEntityValue(rangeEntityValue);
                        break;

                    case StatusTypeService.Delete:
                        RangeEntityValueDAO.DeleteRangeEntityValue(rangeEntityValue);
                        break;
                }

                rangeEntity.RangeEntityValue[index] = rangeEntityValue;
            }

            return rangeEntity;
        }

        #endregion
    }
}
