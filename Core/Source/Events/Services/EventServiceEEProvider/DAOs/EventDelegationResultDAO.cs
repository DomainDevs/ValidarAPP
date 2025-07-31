using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventDelegationResultDAO
    {
        /// <summary>
        /// obtine el resumen de los eventos que se lanzaron 
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdSubmodule">id del submodulo</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="IdTemp">id del temporal</param>
        /// <returns>sp CO_GET_TMP_POLICY_EVENTS</returns>
        public List<Models.EventDelegationResult> GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp(int IdModule, int IdSubModule, int IdUser, string IdTemp)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.ResultId, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.Key1Field, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.GroupEventId, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.EventId, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.EventDate, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.DelegationId, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.Description, "EC")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.EnabledStop, "EC")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventDelegationResult.Properties.DescriptionErrorMessage, "EDR")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.TypeCode, "EC")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventGroup.Properties.ModuleCode, "EG")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventGroup.Properties.SubmoduleCode, "EG")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventDelegationResult), "EDR"), new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventDelegationResult.Properties.GroupEventId, "EDR").Equal().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC")
                    .And().Property(EVENTEN.CoEventDelegationResult.Properties.EventId, "EDR").Equal().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventGroup), "EG"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventDelegationResult.Properties.GroupEventId, "EDR").Equal().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC")
                    .And().Property(EVENTEN.CoEventGroup.Properties.GroupEventId, "EG").Equal().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").GetPredicate());

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventGroup.Properties.ModuleCode, "EG").Equal().Constant(IdModule)
                .And().Property(EVENTEN.CoEventGroup.Properties.SubmoduleCode, "EG").Equal().Constant(IdSubModule)
                .And().Property(EVENTEN.CoEventDelegationResult.Properties.UserId, "EDR").Equal().Constant(IdUser)
                .And().Property(EVENTEN.CoEventDelegationResult.Properties.Operation1Id, "EDR").Equal().Constant(IdTemp)
                .And().Property(EVENTEN.CoEventDelegationResult.Properties.RejectInd, "EDR").Equal().Constant(1)
                .And().Property(EVENTEN.CoEventCompany.Properties.EnabledAuthorize, "EC").Distinct().Constant(0);

                select.Table = join;
                select.Where = where.GetPredicate();

                var list = new List<Models.EventDelegationResult>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        var GroupEventId = (int)reader["GroupEventId"];
                        var EventId = (int)reader["EventId"];
                        var DelegationId = (int)reader["DelegationId"];

                        var Existgroup = list.Where(x => x.GroupEventId == GroupEventId && x.EventId == EventId && x.DelegationId == DelegationId).FirstOrDefault();

                        if (Existgroup != null)
                        {
                            list.Remove(Existgroup);
                            Existgroup.ResultId += "-" + reader["ResultId"].ToString();
                            Existgroup.RiskId += "-" + reader["Key1Field"].ToString();
                            Existgroup.Count = Existgroup.ResultId.Split('-').Count();
                            list.Add(Existgroup);
                        }
                        else
                        {
                            list.Add(new Models.EventDelegationResult
                            {
                                Count = 1,
                                ResultId = reader["ResultId"].ToString(),
                                RiskId = reader["Key1Field"].ToString(),
                                GroupEventId = (int)reader["GroupEventId"],
                                EventId = (int)reader["EventId"],
                                DelegationId = (int)reader["DelegationId"],
                                Description = (string)reader["Description"],
                                IsNotification = (bool)reader["EnabledStop"],
                                DescriptionErrorMessage = (string)reader["DescriptionErrorMessage"],
                                //TypeCode = reader["TypeCode"] == null ? null:((int)reader["TypeCode"]),
                                EventDate = (DateTime)reader["EventDate"],
                                ModuleId = (int)reader["ModuleCode"],
                                SubModuleId = (int)reader["SubmoduleCode"],
                                IdTemporal = IdTemp,
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp", ex);
            }
        }
    }
}
