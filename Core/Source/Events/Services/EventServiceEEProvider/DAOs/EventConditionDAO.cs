using Sistran.Co.Application.Data;
using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.EventsServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventConditionDAO
    {
        /// <summary>
        /// obtiene las condiciones segun los parametros , crea la consulta dinamica (sp)
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="delegationId">id de la delegacion</param>
        public ArrayList GetEventConditionsByIdGroupIdEventIdDelegation(int IdGroup, int IdEvent, int IdDelegation)
        {
            try
            {
                NameValue[] parameters = new NameValue[3];
                parameters[0] = new NameValue("IdGroupEvent", IdGroup);
                parameters[1] = new NameValue("IdEvent", IdEvent);
                parameters[2] = new NameValue("IdDelegation", IdDelegation);

                DataTable result = null;
                using (DynamicDataAccess pdb = new DynamicDataAccess()) {
                    result = pdb.ExecuteSPDataTable("EVE.SP_EVENT_GET_CONDITIONS_EVENT", parameters);
                }
                ArrayList list = new ArrayList();

                foreach (DataRow item in result.Rows)
                {
                    list.Add((Object[])item.ItemArray);
                }

                return list;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventConditionsByIdGroupIdEventIdDelegation", ex);
            }
        }

        /// <summary>
        /// obtiene las condiciones segun los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="delegationId">id de la delegacion</param>
        public List<EventCondition> GetConditionsByIdGroupIdEventIdDelegationIdEntity(int IdGroup, int IdEvent, int IdDelegation)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.GroupEventId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.EventId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.DelegationId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.EntityId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.ConditionQuantity, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.EventQuantity, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.ComparatorCode, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCondition.Properties.ConditionValue, "c")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventCondition.Properties.GroupEventId, "c").Equal().Constant(IdGroup)
                    .And().Property(EVENTEN.CoEventCondition.Properties.EventId, "c").Equal().Constant(IdEvent)
                    .And().Property(EVENTEN.CoEventCondition.Properties.DelegationId, "c").Equal().Constant(IdDelegation);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventCondition), "c");
                select.Where = where.GetPredicate();


                List<EventCondition> list = new List<EventCondition>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new EventCondition
                        {
                            GroupEventId = (int)reader["GroupEventId"],
                            EventId = (int)reader["EventId"],
                            DelegationId = (int)reader["DelegationId"],
                            EntityId = (int)reader["EntityId"],
                            ConditionQuantity = (int)reader["ConditionQuantity"],
                            EventQuantity = (int)reader["EventQuantity"],
                            ComparatorCode = (int)reader["ComparatorCode"],
                            ConditionValue = (string)reader["ConditionValue"]
                        });
                    }
                }
                return list;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetConditionsByIdGroupIdEventIdDelegationIdEntity", ex);
            }
        }

        /// <summary>
        /// obtiene la lista de valores para la condicion que cumpla con los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo</param>
        /// <param name="EVENTEN">entidad</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="Dependences">valores actuales de las dependencias</param>
        /// <returns></returns>
        public List<Models.Objects> GetValuesByIdGroupEntityIdEventIdOperator(int IdGroup, EventEntity EVENTEN, int IdEvent, List<Models.Objects> Dependences)
        {
            try
            {
                List<Models.Objects> list = new List<Objects>();
                var sColumns = "";
                var sFrom = "";
                var sCondition = "";

                if (EVENTEN.QueryType.QueryTypeCode == 1)
                {
                    return list;
                }

                sColumns = EVENTEN.SourceCode + " CODE, " + EVENTEN.SourceDescription + " DESCRIPTION";
                sFrom = EVENTEN.SourceTable + " T1";

                //Hacer Join con tabla adicional en caso de haberla 
                if (EVENTEN.JoinTable != null && EVENTEN.JoinTable.Trim() != string.Empty)
                {
                    sFrom += " INNER JOIN " + EVENTEN.JoinTable + " T2 ON T1." + EVENTEN.JoinSourceField + " = " + EVENTEN.JoinTargetField;
                }

                if (EVENTEN.ParamWhere != null && EVENTEN.ParamWhere.Trim() != string.Empty)
                {
                    if (sCondition != string.Empty)
                    {
                        sCondition += " AND ";
                    }
                    sCondition += EVENTEN.ParamWhere;
                }

                //Revisar Dependencias
                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                var dtDepends = entityDependenciesDAO.GetDependencesByIdGroupIdEventIdEntity(IdGroup, IdEvent, EVENTEN.EntityId, 0);
                if (dtDepends.Count > 0)
                {
                    foreach (var dr in dtDepends)
                    {
                        if (sCondition != string.Empty)
                        {
                            sCondition += " AND ";
                        }
                        sCondition += dr.ColumnName + " = ";
                        foreach (var ev in Dependences)
                        {
                            if (ev.Id == dr.DependsId)
                            {
                                sCondition += ev.Description;
                                break;
                            }
                        }
                    }
                }

                NameValue[] parameters = new NameValue[3];
                parameters[0] = new NameValue("sColumns", sColumns);
                parameters[1] = new NameValue("sFrom", sFrom);
                parameters[2] = new NameValue("sCondition", sCondition);

                DataTable result = null;
                using (DynamicDataAccess pdb = new DynamicDataAccess()) {
                    result = pdb.ExecuteSPDataTable("EVE.SP_EVENT_GET_VALUES_CONDITION", parameters);
                }

                ArrayList listRow = new ArrayList();

                foreach (DataRow item in result.Rows)
                {
                    listRow.Add((Object[])item.ItemArray);
                }

                list = ModelAssembler.CreateListObjects(listRow);

                return list.OrderBy(x => x.Description).ToList();

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetValuesByIdGroupEntityIdEventIdOperator", ex);
            }
        }

        /// <summary>
        /// Crea una nueva condicion del evento
        /// </summary>
        /// <param name="conditions">condicion a crear</param>
        public void CreateConditionsEntity(List<Models.EventCondition> conditions)
        {
            try
            {
                var index = 1;
                var conditionsModel = GetConditionsByIdGroupIdEventIdDelegationIdEntity(conditions[0].GroupEventId, conditions[0].EventId, conditions[0].DelegationId);

                if (conditionsModel.Count != 0)
                {
                    index = conditionsModel.Max(x => x.ConditionQuantity) + 1;
                }


                for (int i = 0; i < conditions.Count; i++)
                {
                    conditions[i].ConditionQuantity = index;
                    conditions[i].EventQuantity = i + 1;
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateEventCondition(conditions[i]));
                }

                try
                {
                    GetEventConditionsByIdGroupIdEventIdDelegation(conditions[0].GroupEventId, conditions[0].EventId, conditions[0].DelegationId);
                }
                catch (Exception ex)
                {
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        DeleteConditionsEntity(conditions[i].GroupEventId, conditions[i].EventId, conditions[i].DelegationId, conditions[i].ConditionQuantity);
                    }

                    throw new BusinessException("Excepcion en: CreateConditionsEntity -- EVE.SP_EVENT_GET_CONDITIONS_EVENT", ex);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateConditionsEntity", ex);
            }
        }

        /// <summary>
        /// edita una condicion del evento
        /// </summary>
        /// <param name="conditions">condicion a editar</param>
        public void UpdateConditionsEntity(List<Models.EventCondition> conditions)
        {
            try
            {
                var conditionsModel = GetConditionsByIdGroupIdEventIdDelegationIdEntity(conditions[0].GroupEventId, conditions[0].EventId, conditions[0].DelegationId)
                                            .Where(x => x.ConditionQuantity == conditions[0].ConditionQuantity).ToList();

                for (int i = 0; i < conditionsModel.Count; i++)
                {
                    PrimaryKey key = EVENTEN.CoEventCondition.CreatePrimaryKey(conditionsModel[i].GroupEventId, conditionsModel[i].EventId, conditionsModel[i].DelegationId, conditionsModel[i].EntityId, conditionsModel[i].ConditionQuantity, conditionsModel[i].EventQuantity);
                    var condition = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventCondition;

                    var modelCondition = conditions.Where(x => x.GroupEventId == conditionsModel[i].GroupEventId && x.EventId == conditionsModel[i].EventId && x.DelegationId == conditionsModel[i].DelegationId && x.EntityId == conditionsModel[i].EntityId && x.ConditionQuantity == conditionsModel[i].ConditionQuantity && x.EventQuantity == conditionsModel[i].EventQuantity).First();

                    condition.ConditionValue = modelCondition.ConditionValue;
                    condition.ComparatorCode = modelCondition.ComparatorCode;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(condition);
                }
                try
                {
                    GetEventConditionsByIdGroupIdEventIdDelegation(conditions[0].GroupEventId, conditions[0].EventId, conditions[0].DelegationId);
                }
                catch (Exception ex)
                {
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        DeleteConditionsEntity(conditions[i].GroupEventId, conditions[i].EventId, conditions[i].DelegationId, conditions[i].ConditionQuantity);
                    }

                    throw new BusinessException("Excepcion en: UpdateConditionsEntity -- EVE.SP_EVENT_GET_CONDITIONS_EVENT", ex);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateConditionsEntity", ex);
            }
        }

        /// <summary>
        /// elimina una condicion 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <param name="IdContidion">id de la condicion</param>
        public void DeleteConditionsEntity(int idGroup, int idEvent, int delegationId, int idContidion)
        {
            try
            {
                var conditionsModel = GetConditionsByIdGroupIdEventIdDelegationIdEntity(idGroup, idEvent, delegationId)
                    .Where(x => x.ConditionQuantity == idContidion).ToList();

                for (int i = 0; i < conditionsModel.Count; i++)
                {
                    PrimaryKey key = EVENTEN.CoEventCondition.CreatePrimaryKey(conditionsModel[i].GroupEventId, conditionsModel[i].EventId, conditionsModel[i].DelegationId, conditionsModel[i].EntityId, conditionsModel[i].ConditionQuantity, conditionsModel[i].EventQuantity);
                    var condition = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventCondition;

                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(condition);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteConditionsEntity", ex);
            }
        }
    }
}
