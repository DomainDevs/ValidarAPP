using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using System.Linq;
using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RuleBaseDAO
    {
        /// <summary>
        /// obtiene  Models.RuleBase a partir de ruleBaseId
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <returns></returns>
        public static Models.RuleBase GetRuleBase(int ruleBaseId)
        {
            try
            {
                PrimaryKey key = RuleBase.CreatePrimaryKey(ruleBaseId);
                RuleBase ruleBase = (RuleBase)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                return ModelAssembler.CreateRuleBase(ruleBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleBase", ex);
            }
        }

        /// <summary>
        /// obtiene una lista de RuleBase  a partir de ruleBaseId
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <returns></returns>
        public static BusinessCollection GetRules(int ruleBaseId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.PropertyEquals(Entities.Rule.Properties.RuleBaseId, ruleBaseId);

                BusinessCollection ruleList = new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Entities.Rule),
                    filter.GetPredicate()));

                return ruleList;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRules", ex);
            }

        }

        /// <summary>
        /// crea un RuleBase
        /// </summary>
        /// <param name="RuleBase"></param>
        /// <returns></returns>
        public static RuleBase CreateRuleBase(RuleBase RuleBase)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(RuleBase);
                return RuleBase;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleBase", ex);
            }

        }

        /// <summary>
        /// edita un RuleBase
        /// </summary>
        /// <param name="RuleBase"></param>
        /// <param name="IsPublish"></param>
        /// <returns></returns>
        public static RuleBase UpdateRuleBase(RuleBase RuleBase, bool IsPublish)
        {
            try
            {
                RuleBase.IsPublished = IsPublish;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(RuleBase);
                return RuleBase;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRuleBase", ex);
            }

        }

        /// <summary>
        /// elimina un RuleBase
        /// </summary>
        /// <param name="RuleBase"></param>
        public static void DeleteRuleBase(RuleBase RuleBase)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(RuleBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteRuleBase", ex);
            }

        }

        /// <summary>
        /// actualiza un atabla de decision 
        /// </summary>
        /// <param name="RuleBase"></param>
        /// <param name="Condiciones"></param>
        /// <param name="Acciones"></param>
        /// <returns></returns>
        public static bool UpdateTableDecision(Models.RuleBase RuleBase, List<Models.Concept> Condiciones, List<Models.Concept> Acciones)
        {
            try
            {
                //actualizamos la tabla ruleBase
                #region ruleBase
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.RuleBase.Properties.RuleBaseId, typeof(RuleBase.Properties).Name);
                filter.Equal();
                filter.Constant(RuleBase.RuleBaseId);

                BusinessCollection businessCollection =
                    new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                        .SelectObjects(typeof(RuleBase), filter.GetPredicate()));
                RuleBase updateRuleBase = (RuleBase)(businessCollection).FirstOrDefault();

                if (updateRuleBase != null)
                {
                    updateRuleBase.LevelId = RuleBase.LevelId;
                    updateRuleBase.PackageId = RuleBase.PackageId;
                    updateRuleBase.Description = RuleBase.Description;
                    updateRuleBase.RuleBaseVersion = updateRuleBase.RuleBaseVersion + 1;
                    RuleBaseDAO.UpdateRuleBase(updateRuleBase, false);
                }
                #endregion

                filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.Rule.Properties.RuleBaseId, typeof(Entities.Rule.Properties).Name);
                filter.Equal();
                filter.Constant(RuleBase.RuleBaseId);

                businessCollection = RuleDAO.ListRule(filter.GetPredicate(), null);
                List<Entities.Rule> Rules = (businessCollection).Cast<Entities.Rule>().ToList();                

                #region Condiciones
                filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.RuleConditionConcept.Properties.RuleBaseId, typeof(Entities.RuleConditionConcept.Properties).Name);
                filter.Equal();
                filter.Constant(RuleBase.RuleBaseId);

                businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Entities.RuleConditionConcept), filter.GetPredicate()));

                List<Entities.RuleConditionConcept> RuleConditionConcepts = (businessCollection).Cast<Entities.RuleConditionConcept>().ToList();

                //eliminamos los conceptos de las condiciones
                foreach (var RuleConditionConcept in RuleConditionConcepts)
                {
                    var conceptEncontrado = Condiciones.Where(l => l.EntityId == RuleConditionConcept.EntityId && l.ConceptId == RuleConditionConcept.ConceptId).FirstOrDefault();
                    if (conceptEncontrado == null)
                    {
                        NameValue[] pars = new NameValue[4];
                        pars[0] = new NameValue("RULE_BASE_ID", System.DBNull.Value);
                        pars[1] = new NameValue("ENTITY_ID", RuleConditionConcept.EntityId);
                        pars[2] = new NameValue("CONCEPT_ID", RuleConditionConcept.ConceptId);
                        pars[3] = new NameValue("ALL_TABLE", System.DBNull.Value);

                        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                        {
                            dynamicDataAccess.ExecuteSPNonQuery("SCR.DELETE_CONTENT_TABLE_DECISION", pars);
                        }
                    }
                };


                foreach (var Condicion in Condiciones)
                {
                    var conceptEncontrado = RuleConditionConcepts.Where(l => l.EntityId == Condicion.EntityId && l.ConceptId == Condicion.ConceptId).FirstOrDefault();
                    if (conceptEncontrado != null)
                    {
                        //editelo el orden num
                        conceptEncontrado.OrderNum = Condicion.OrderNum;
                        RuleConditionConceptDAO.UpdateRuleConditionConcept(conceptEncontrado);                       
                    }
                    else
                    {
                        //agregelo
                        RuleConditionConcept ruleConditionConcept = new RuleConditionConcept(RuleBase.RuleBaseId, Condicion.EntityId, Condicion.ConceptId);
                        ruleConditionConcept.RuleBaseId = RuleBase.RuleBaseId;
                        ruleConditionConcept.EntityId = Condicion.EntityId;
                        ruleConditionConcept.ConceptId = Condicion.ConceptId;
                        ruleConditionConcept.OrderNum = Condicion.OrderNum;
                        RuleConditionConceptDAO.CreateRuleConditionConcept(ruleConditionConcept);

                        foreach (var item in Rules)
                        {
                            RuleCondition ruleCondition = new RuleCondition(RuleBase.RuleBaseId, item.RuleId, Condiciones.Count() - 1);
                            ruleCondition.RuleBaseId = RuleBase.RuleBaseId;
                            ruleCondition.RuleId = item.RuleId;
                            ruleCondition.ConditionId = Condicion.OrderNum;
                            ruleCondition.EntityId = ruleConditionConcept.EntityId;
                            ruleCondition.ConceptId = ruleConditionConcept.ConceptId;
                            ruleCondition.RuleValueTypeCode = 1;
                            ruleCondition.CondValue = Convert.ToString(null);//REVISAR JONATHAN              
                            ruleCondition.OrderNum = 0;
                            RuleConditionDAO.CreateRuleCondition(ruleCondition);
                        }

                    }
                };

                #endregion

                #region Acciones
                filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.RuleActionConcept.Properties.RuleBaseId, typeof(Entities.RuleActionConcept.Properties).Name);
                filter.Equal();
                filter.Constant(RuleBase.RuleBaseId);

                businessCollection =
                    new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                        .SelectObjects(typeof(Entities.RuleActionConcept), filter.GetPredicate()));

                List<Entities.RuleActionConcept> RuleActionConcepts = (businessCollection).Cast<Entities.RuleActionConcept>().ToList();

                foreach (var RuleActionConcept in RuleActionConcepts)
                {
                    var conceptEncontrado = Acciones.Where(l => l.EntityId == RuleActionConcept.EntityId && l.ConceptId == RuleActionConcept.ConceptId).FirstOrDefault();
                    if (conceptEncontrado == null)
                    {
                        NameValue[] pars = new NameValue[4];
                        pars[0] = new NameValue("RULE_BASE_ID", System.DBNull.Value);
                        pars[1] = new NameValue("ENTITY_ID", RuleActionConcept.EntityId);
                        pars[2] = new NameValue("CONCEPT_ID", RuleActionConcept.ConceptId);
                        pars[3] = new NameValue("ALL_TABLE", System.DBNull.Value);

                        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                        {
                            dynamicDataAccess.ExecuteSPNonQuery("SCR.DELETE_CONTENT_TABLE_DECISION", pars);
                        }
                    }
                };

                foreach (var Accion in Acciones)
                {
                    var conceptEncontrado = RuleActionConcepts.Where(l => l.EntityId == Accion.EntityId && l.ConceptId == Accion.ConceptId).FirstOrDefault();
                    if (conceptEncontrado != null)
                    {
                        //editelo el orden num
                        conceptEncontrado.OrderNum = Accion.OrderNum;
                        RuleActionConceptDAO.UpdateRuleActionConcept(conceptEncontrado);
                    }
                    else
                    {
                        //agregelo
                        RuleActionConcept ruleActionConcept = new RuleActionConcept(RuleBase.RuleBaseId, Accion.EntityId, Accion.ConceptId);
                        ruleActionConcept.RuleBaseId = RuleBase.RuleBaseId;
                        ruleActionConcept.EntityId = Accion.EntityId;
                        ruleActionConcept.ConceptId = Accion.ConceptId;
                        ruleActionConcept.OrderNum = Accion.OrderNum;
                        RuleActionConceptDAO.CreateRuleActionConcept(ruleActionConcept);

                        foreach (var item in Rules)
                        {
                            RuleAction ruleAction = new RuleAction(RuleBase.RuleBaseId, item.RuleId, Acciones.Count() - 1);
                            ruleAction.RuleBaseId = RuleBase.RuleBaseId;
                            ruleAction.RuleId = item.RuleId;
                            ruleAction.ActionId = Accion.OrderNum;
                            ruleAction.ActionTypeCode = 1;
                            ruleAction.EntityId = ruleActionConcept.EntityId;
                            ruleAction.ConceptId = ruleActionConcept.ConceptId;
                            ruleAction.ValueTypeCode = 1;
                            ruleAction.ActionValue = Convert.ToString(null);
                            ruleAction.OrderNum = 0;
                            RuleActionDAO.CreateRuleAction(ruleAction);
                        }
                    }
                };
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateTableDecision", ex);
            }

        }

        /// <summary>
        /// crea la tabla de decision
        /// </summary>
        /// <param name="RuleBase"></param>
        /// <param name="Condiciones"></param>
        /// <param name="Acciones"></param>
        /// <returns></returns>
        public static RuleBase CreateTableDecision(Models.RuleBase RuleBase, List<Models.Concept> Condiciones, List<Models.Concept> Acciones)
        {
            try
            {
                //falta validar que el mismo concepto no este repetido            
                RuleBase ruleBase = new RuleBase();
                ruleBase.CurrentFrom = DateTime.Now;
                ruleBase.Description = RuleBase.Description;
                ruleBase.LevelId = RuleBase.LevelId;
                ruleBase.PackageId = RuleBase.PackageId;
                ruleBase.RuleEnumerator = RuleBase.RuleEnumerator;
                ruleBase.IsPublished = RuleBase.IsPublished;
                ruleBase.RuleBaseVersion = 1;
                ruleBase.RuleBaseTypeCode = 2;

                Transaction.Created += delegate (object sender, TransactionEventArgs e) { };
                using (Context.Current)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        transaction.Completed += delegate (object sender, TransactionEventArgs e) { };
                        transaction.Disposed += delegate (object sender, TransactionEventArgs e) { };
                        try
                        {
                            RuleBase newRuleBase = RuleBaseDAO.CreateRuleBase(ruleBase);
                            foreach (var item in Condiciones)
                            {
                                RuleConditionConcept ruleConditionConcept = new RuleConditionConcept(newRuleBase.RuleBaseId, item.EntityId, item.ConceptId);
                                ruleConditionConcept.RuleBaseId = newRuleBase.RuleBaseId;
                                ruleConditionConcept.EntityId = item.EntityId;
                                ruleConditionConcept.ConceptId = item.ConceptId;
                                ruleConditionConcept.OrderNum = item.OrderNum;
                                RuleConditionConceptDAO.CreateRuleConditionConcept(ruleConditionConcept);
                            }
                            foreach (var item in Acciones)
                            {
                                RuleActionConcept ruleActionConcept = new RuleActionConcept(newRuleBase.RuleBaseId, item.EntityId, item.ConceptId);
                                ruleActionConcept.RuleBaseId = newRuleBase.RuleBaseId;
                                ruleActionConcept.EntityId = item.EntityId;
                                ruleActionConcept.ConceptId = item.ConceptId;
                                ruleActionConcept.OrderNum = item.OrderNum;
                                RuleActionConceptDAO.CreateRuleActionConcept(ruleActionConcept);
                            }
                            transaction.Complete();
                            return ruleBase;
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateTableDecision", ex);
            }

        }

        /// <summary>
        /// elimina la tabla de decision 
        /// </summary>
        /// <param name="RuleBase"></param>
        public static void DeleteTableDecision(Models.RuleBase RuleBase)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                //elimina RULE_ACTION_CONCEPT
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(RuleActionConcept.Properties.RuleBaseId, RuleBase.RuleBaseId);
                foreach (RuleActionConcept item in RuleActionConceptDAO.GetRuleActionConcept(filter.GetPredicate(), new[] { RuleActionConcept.Properties.ConceptId }))
                {
                    RuleActionConceptDAO.DeleteRuleActionConcept(item);
                }

                //elimina RULE_CONDITION_CONCEPT
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(RuleConditionConcept.Properties.RuleBaseId, RuleBase.RuleBaseId);
                foreach (RuleConditionConcept item in RuleConditionConceptDAO.GetRuleConditionConcept(filter.GetPredicate(), new[] { RuleConditionConcept.Properties.ConceptId }))
                {
                    RuleConditionConceptDAO.DeleteRuleConditionConcept(item);
                }

                //elimina RULE_CONDITION
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(RuleCondition.Properties.RuleBaseId, RuleBase.RuleBaseId);
                foreach (RuleCondition item in RuleConditionDAO.GetRuleCondition(filter.GetPredicate(), new[] { RuleConditionConcept.Properties.ConceptId }))
                {
                    RuleConditionDAO.DeleteRuleCondition(item);
                }

                //elimina RULE_ACTION
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(RuleAction.Properties.RuleBaseId, RuleBase.RuleBaseId);
                foreach (RuleAction item in RuleActionDAO.ListRuleAction(RuleBase.RuleBaseId, null)) 
                {
                    RuleActionDAO.DeleteRuleAction(item);
                }

                //elimina rules
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(Rule.Properties.RuleBaseId, RuleBase.RuleBaseId);
                foreach (Rule item in RuleDAO.ListRule(filter.GetPredicate(), new[] { Rule.Properties.RuleId }))
                {
                    RuleDAO.DeleteRule(item);
                }
                
                //emilina rule base
                DeleteRuleBase(FindRuleBase(RuleBase.RuleBaseId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// obtiene RuleBase a partir de RuleBaseId
        /// </summary>
        /// <param name="RuleBaseId"></param>
        /// <returns></returns>
        public static RuleBase FindRuleBase(int RuleBaseId)
        {
            try
            {
                PrimaryKey key = Entities.RuleBase.CreatePrimaryKey(RuleBaseId);
                RuleBase RuleBase = (RuleBase)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return RuleBase;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Find", ex);
            }

        }
    }
}
