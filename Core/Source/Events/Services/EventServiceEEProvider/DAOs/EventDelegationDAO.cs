using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventDelegationDAO
    {
        /// <summary>
        /// obtiene las delegaciones a partir de IdGroup y IdEvent
        /// </summary>
        /// <param name="IdGroup">grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// funcionalidad del sp --EVE.CO_GET_DELEGATIONS--
        public List<Models.EventDelegationSP> GetDelegationsByIdGroupIdEvent(int IdGroup, int IdEvent)
        {
            try
            {

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegation.Properties.DelegationId, "ED")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoHierarchyAssociation.Properties.HierarchyCode, "HAS")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoHierarchyAssociation.Properties.Description, "HAS")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoHierarchyAssociation), "HAS"), new ClassNameTable(typeof(EVENTEN.CoEventGroup), "EG"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAssociation.Properties.ModuleCode, "HAS").Equal().Property(EVENTEN.CoEventGroup.Properties.ModuleCode, "EG")
                .And().Property(EVENTEN.CoHierarchyAssociation.Properties.SubmoduleCode, "HAS").Equal().Property(EVENTEN.CoEventGroup.Properties.SubmoduleCode, "EG").GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventGroup.Properties.GroupEventId, "EG").Equal().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventDelegation), "ED"), JoinType.Left);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventDelegation.Properties.GroupEventId, "ED").Equal().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC")
                .And().Property(EVENTEN.CoEventDelegation.Properties.EventId, "ED").Equal().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC")
                .And().Property(EVENTEN.CoEventDelegation.Properties.HierarchyCode, "ED").Equal().Property(EVENTEN.CoHierarchyAssociation.Properties.HierarchyCode, "HAS")
                .GetPredicate();

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Constant(IdGroup)
                .And().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Constant(IdEvent);

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Models.EventDelegationSP> delegations = new List<Models.EventDelegationSP>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        delegations.Add(new Models.EventDelegationSP
                        {
                            DelegationId = (reader["DelegationId"] != null) ? (int)reader["DelegationId"] : -1,
                            HierarchyId = (int)reader["HierarchyCode"],
                            Description = (string)reader["Description"]
                        });
                    }
                }

                return delegations.OrderBy(x => x.HierarchyId).ToList();

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetDelegationsByIdGroupIdEvent", ex);
            }
        }

        /// <summary>
        /// crea la delegacion en la tabla Co_Event_Delegation
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la jerarquia</param>
        /// <returns>Id de la delegacion</returns>
        /// funcionalidad del sp EVE.CO_GET_DELEGATION
        public int CreateDelegationByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {

                var listDelegations = GetDelegationsByIdGroupIdEvent(IdGroup, IdEvent);
                var delegation = listDelegations.Where(x => x.HierarchyId == IdHierarchy).First().DelegationId;

                if (delegation == -1)
                {
                    delegation = listDelegations.Max(x => x.DelegationId);

                    if (delegation != -1)
                    {
                        delegation++;
                    }
                    else
                    {
                        delegation = 1;
                    }

                    var newDelegation = new CoEventDelegation(IdGroup, IdEvent, delegation)
                    {
                        HierarchyCode = IdHierarchy
                    };
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(newDelegation);
                }

                return delegation;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateDelegationByIdGroupIdEventIdHierarchy", ex);
            }
        }

        /// <summary>
        /// Elimina la delegacion 
        /// </summary>
        /// <param name="idGroup">id del grupo de eventos</param>
        /// <param name="idEvent">id del evento</param>
        /// <param name="delegationId">id de la delegacion</param>
        public void DeleteDelegationByIdGroupIdEventIdDelegation(int idGroup, int idEvent, int delegationId)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventDelegation.CreatePrimaryKey(idGroup, idEvent, delegationId);
                EVENTEN.CoEventDelegation entityCoEventDelegation = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventDelegation;
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityCoEventDelegation);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteDelegationByIdGroupIdEventIdDelegation", ex);
            }
        }

        /// <summary>
        /// obtiene las delegaciones de nivel igual o superior a la especificada que tengas usuarios autorizadores
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la jerarquia</param>
        /// <returns></returns>
        public List<Models.EventDelegationSP> GetTopDelegationsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.Distinct = true;
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoHierarchyAssociation.Properties.HierarchyCode, "HA")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoHierarchyAssociation.Properties.Description, "HA")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegation.Properties.DelegationId, "ED")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventGroup), "EG"), new ClassNameTable(typeof(EVENTEN.CoHierarchyAssociation), "HA"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventGroup.Properties.ModuleCode, "EG").Equal().Property(EVENTEN.CoHierarchyAssociation.Properties.ModuleCode, "HA")
                    .And().Property(EVENTEN.CoEventGroup.Properties.SubmoduleCode, "EG").Equal().Property(EVENTEN.CoHierarchyAssociation.Properties.SubmoduleCode, "HA")
                    .GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventDelegation), "ED"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventDelegation.Properties.GroupEventId, "ED").Equal().Property(EVENTEN.CoEventGroup.Properties.GroupEventId, "EG")
                    .And().Property(EVENTEN.CoEventDelegation.Properties.HierarchyCode, "ED").Equal().Property(EVENTEN.CoHierarchyAssociation.Properties.HierarchyCode, "HA")
                    .GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventAuthorizationUsers), "AU"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventAuthorizationUsers.Properties.GroupEventId, "AU").Equal().Property(EVENTEN.CoEventDelegation.Properties.GroupEventId, "ED")
                    .And().Property(EVENTEN.CoEventAuthorizationUsers.Properties.EventId, "AU").Equal().Property(EVENTEN.CoEventDelegation.Properties.EventId, "ED")
                    .And().Property(EVENTEN.CoEventAuthorizationUsers.Properties.DelegationId, "AU").Equal().Property(EVENTEN.CoEventDelegation.Properties.DelegationId, "ED")
                    .GetPredicate();

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventGroup.Properties.GroupEventId, "EG").Equal().Constant(IdGroup)
                    .And().Property(EVENTEN.CoEventDelegation.Properties.EventId, "ED").Equal().Constant(IdEvent)
                    .And().Property(EVENTEN.CoHierarchyAssociation.Properties.HierarchyCode, "HA").LessEqual().Constant(IdHierarchy);

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Models.EventDelegationSP> list = new List<Models.EventDelegationSP>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EventDelegationSP
                        {
                            DelegationId = (int)reader["DelegationId"],
                            HierarchyId = (int)reader["HierarchyCode"],
                            Description = (string)reader["Description"]
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetTopDelegationsByIdGroupIdEventIdHierarchy", ex);
            }
        }
    }
}
