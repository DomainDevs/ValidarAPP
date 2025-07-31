using Sistran.Co.Application.Data;
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
    public class EventAuthorizationDAO
    {
        /// <summary>
        /// consulta los eventos que cumplen con los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="State">stado del evento
        ///     <value val="1">pendientes</value>
        ///     <value val="2">autorizados</value>
        ///     <value val="3">rechazados</value>
        ///     <value val="4">Reasignados</value>
        /// </param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="DateStart">fecha inicial</param>
        /// <param name="DateEnd">fecha final</param>
        /// <returns>sp EVE.CO_GET_EVENT_AUTHORIZATION  -  se agrupan los mismos tipo de eventos</returns>
        public List<Models.EventAuthorization> GetEventAuthorizationByIdGroupStateIdUserDateStartDateEnd(int IdGroup, int State, int IdUser, string DateStart, string DateEnd)
        {
            try
            {
                NameValue[] parameters = new NameValue[8];
                parameters[0] = new NameValue("GROUP_EVENT_ID", IdGroup);
                parameters[1] = new NameValue("USER_NAME", "");
                parameters[2] = new NameValue("NUM_OPERATION", 0);
                parameters[3] = new NameValue("STATE_ID", State);
                parameters[4] = new NameValue("RECOVER", 0);
                parameters[5] = new NameValue("USER_ID", IdUser);
                parameters[6] = new NameValue("CURRENT_FROM", DateStart);
                parameters[7] = new NameValue("CURRENT_TO", DateEnd);

                DataTable result = null;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("EVE.CO_GET_EVENT_AUTHORIZATION", parameters);
                }


                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();
                EventCompanyDAO eventCompanyDAO = new EventCompanyDAO();


                foreach (DataRow item in result.Rows)
                {
                    var groupExist = list.Where(x =>
                                x.Name == (string)item[0] &&
                                x.Operation2Id == (decimal)item[1] &&
                                x.DescriptionErrorMessage == item[2].ToString() &&
                                x.Description == (string)item[4] &&
                                x.StrEventDate == ((DateTime)item[5]).ToString("dd/MM/yyyy") &&
                                x.EventUserId == (int)item[6] &&
                                x.AuthorizedInd == (bool)item[7] &&
                                x.RejectInd == (bool)item[8] &&
                                x.AuthUserID == (int)item[9] &&
                                x.RejectId == (short)item[10] &&
                                x.EventId == Convert.ToInt16(item[11].ToString()) &&
                                x.GroupEventId == Convert.ToInt16(item[12].ToString()) &&
                                x.Active == item[13].ToString() &&
                                x.HierachyCd == Convert.ToInt16(item[14].ToString()) &&
                                x.Query == (int)item[15] &&
                                x.DocumentNum == item[16].ToString() &&
                                x.AuthorizationDescription == (string)item[17] &&
                                x.DescriptionPrefix == (string)item[19] &&
                                x.DescriptionBranch == (string)item[20] &&
                                x.DescriptionEndorsement == (string)item[21] &&
                                x.DocumentNumAditional == item[22].ToString() &&
                                x.Operation1Id == (string)item[23]
                    ).FirstOrDefault();

                    if (groupExist != null)
                    {
                        var AUTHORIZATION_ID = groupExist.AuthorizationId + "-" + item[3].ToString();
                        list.Remove(groupExist);
                        groupExist.AuthorizationId = AUTHORIZATION_ID;
                        groupExist.Count = AUTHORIZATION_ID.Split('-').Count();
                        list.Add(groupExist);
                    }
                    else
                    {
                        list.Add(new Models.EventAuthorization
                        {
                            Count = 1,
                            Name = item[0].ToString(),
                            Operation2Id = (Decimal)item[1],
                            DescriptionErrorMessage = item[2].ToString(),
                            AuthorizationId = item[3].ToString(),
                            Description = (string)item[4],
                            EventDate = ((DateTime)item[5]),
                            StrEventDate = ((DateTime)item[5]).ToString("dd/MM/yyyy"),
                            EventUserId = (int)item[6],
                            AuthorizedInd = (bool)item[7],
                            RejectInd = (bool)item[8],
                            AuthUserID = (int)item[9],
                            RejectId = (short)item[10],
                            EventId = Convert.ToInt16(item[11].ToString()),
                            GroupEventId = Convert.ToInt16(item[12].ToString()),
                            EventName = eventCompanyDAO.GetEventByIdEventGroupIdEvent(Convert.ToInt16(item[12].ToString()), Convert.ToInt16(item[11].ToString())).Description,
                            Active = (string)item[13],
                            HierachyCd = Convert.ToInt16(item[14].ToString()),
                            Query = (int)item[15],
                            DocumentNum = item[16].ToString(),
                            AuthorizationDescription = (string)item[17],
                            EntityDescriptionValues = (string)item[18],
                            DescriptionPrefix = (string)item[19],
                            DescriptionBranch = (string)item[20],
                            DescriptionEndorsement = (string)item[21],
                            DocumentNumAditional = item[22].ToString(),
                            Operation1Id = (string)item[23]
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventAuthorizationByIdGroupStateIdUserDateStartDateEnd", ex);
            }
        }

        /// <summary>
        /// obtiene una evento segun si id de autorizacion
        /// </summary>
        /// <param name="IdAuthorization">id de la autorizacion</param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetEventAuthorizationByIdAuthorization(string IdAuthorization)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Operation1Id, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.UniqueUsers.Properties.AccountName, "UU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventUserId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventDate, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Description, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.Description, "EC"), "EventName"));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.DescriptionErrorMessage, "EC")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationDescription, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EntityDescriptionValues, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.HierarchyCode, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthoUserId, "A")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventAuthorization), "A"), new ClassNameTable(typeof(EVENTEN.UniqueUsers), "UU"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventAuthorization.Properties.EventUserId, "A").Equal().Property(EVENTEN.UniqueUsers.Properties.UserId, "UU").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Property(EVENTEN.CoEventAuthorization.Properties.EventId, "A")
                    .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Property(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "A").GetPredicate());

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "A").In().ListValue();
                foreach (var item in IdAuthorization.Split('-'))
                {
                    where.Constant(item);
                }
                where.EndList();

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EventAuthorization
                        {
                            AuthorizationId = ((int)reader["AuthorizationId"]).ToString(),
                            Operation1Id = (reader["Operation1Id"].ToString()),
                            Name = (string)reader["AccountName"],
                            EventUserId = (int)reader["EventUserId"],
                            EventDate = (DateTime)reader["EventDate"],
                            EventName = (string)reader["EventName"],
                            EventId = (int)reader["EventId"],
                            GroupEventId = (int)reader["GroupEventId"],
                            DescriptionErrorMessage = (string)reader["DescriptionErrorMessage"],
                            Description = (string)reader["Description"],
                            AuthorizationDescription = (string)reader["AuthorizationDescription"],
                            EntityDescriptionValues = reader["EntityDescriptionValues"] == null ? "" : reader["EntityDescriptionValues"].ToString(),
                            HierachyCd = (int)reader["HierarchyCode"],
                            AuthUserID = (int)reader["AuthoUserId"]
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventAuthorizationByIdAuthorization", ex);
            }
        }

        /// <summary>
        /// obtiene lso valores extra parametrizados para la autorizacion del evento
        /// </summary>
        /// <param name="Authorizations"></param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetAnotherValuesToAuthorization(List<Models.EventAuthorization> Authorizations)
        {
            try
            {
                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@TempId", Authorizations.First().Operation1Id);

                DataTable result = null;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("EVE.SP_EVENT_GET_ANOTHERS_VALUES", parameters);
                }

                for (int i = 0; i < Authorizations.Count; i++)
                {
                    foreach (DataRow item in result.Rows)
                    {
                        if (item[0].ToString() == Authorizations[i].AuthorizationId)
                        {
                            Authorizations[i].EntityDescriptionValues += item[1] + "|";
                        }
                    }
                }


                return Authorizations;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetAnotherValuesToAuthorization", ex);
            }
        }

        /// <summary>
        /// obtiene las authorizaciones de un evento en especifico
        /// </summary>
        /// <param name="IdGroup"></param>
        /// <param name="IdEvent"></param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetEventAuthorizationByIdGroupIdEvent(int IdGroup, int IdEvent)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.UniqueUsers.Properties.AccountName, "UU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventUserId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventDate, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Description, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.Description, "EC"), "EventName"));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventCompany.Properties.DescriptionErrorMessage, "EC")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationDescription, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EntityDescriptionValues, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.HierarchyCode, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthoUserId, "A")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventAuthorization), "A"), new ClassNameTable(typeof(EVENTEN.UniqueUsers), "UU"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventAuthorization.Properties.EventUserId, "A").Equal().Property(EVENTEN.UniqueUsers.Properties.UserId, "UU").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Property(EVENTEN.CoEventAuthorization.Properties.EventId, "A")
                    .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Property(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "A").GetPredicate());

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "A").Equal().Constant(IdGroup)
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.EventId, "A").Equal().Constant(IdEvent);

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EventAuthorization
                        {
                            AuthorizationId = (string)reader["AuthorizationId"],
                            Name = (string)reader["AccountName"],
                            EventUserId = (int)reader["EventUserId"],
                            EventDate = (DateTime)reader["EventDate"],
                            EventName = (string)reader["EventName"],
                            EventId = (int)reader["EventId"],
                            GroupEventId = (int)reader["GroupEventId"],
                            DescriptionErrorMessage = (string)reader["DescriptionErrorMessage"],
                            Description = (string)reader["Description"],
                            AuthorizationDescription = (string)reader["AuthorizationDescription"],
                            EntityDescriptionValues = reader["EntityDescriptionValues"] == null ? "" : reader["EntityDescriptionValues"].ToString(),
                            HierachyCd = (int)reader["HierarchyCode"],
                            AuthUserID = (int)reader["AuthoUserId"]
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventAuthorizationByIdGroupIdEvent", ex);
            }
        }

        /// <summary>
        /// actualiza un evento
        /// </summary>
        /// <param name="authorization">evento a autorizar</param>
        public void UpdateEventAuthorization(Models.EventAuthorization authorization)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventAuthorization.CreatePrimaryKey(Convert.ToInt32(authorization.AuthorizationId));
                EVENTEN.CoEventAuthorization entityCoEventAuthorization = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventAuthorization;

                if (authorization.RejectId != 0)
                {
                    entityCoEventAuthorization.AuthorizationDate = DateTime.Now;
                    entityCoEventAuthorization.RejectId = authorization.RejectId;
                }

                if (authorization.HierachyCd != 0 && authorization.AuthUserID != 0)
                {
                    entityCoEventAuthorization.HierarchyCode = authorization.HierachyCd;
                    entityCoEventAuthorization.AuthoUserId = authorization.AuthUserID;
                }

                if (authorization.AuthorizedInd)
                {
                    entityCoEventAuthorization.AuthorizationDate = DateTime.Now;
                }

                entityCoEventAuthorization.AuthorizedInd = authorization.AuthorizedInd;
                entityCoEventAuthorization.RejectInd = authorization.RejectInd;
                entityCoEventAuthorization.AuthorizationDescription = authorization.AuthorizationDescription;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCoEventAuthorization);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateEventAuthorization", ex);
            }
        }

        /// <summary>
        /// retorna los eventos que fueron reasignados, segun los parametros 
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="DateStart">fecha inicial</param>
        /// <param name="DateEnd">fecha final</param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetEventAuthorizationReassignByIdGroupIdUserDateStartDateEnd(int IdGroup, int IdUser, DateTime DateStart, DateTime DateEnd)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.Distinct = true;
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Operation1Id, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventId, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventDate, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EntityDescriptionValues, "AU")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventAuthorization), "AU"), new ClassNameTable(typeof(EVENTEN.CoEventReassignmentUser), "RE"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "AU").Equal().Property(EVENTEN.CoEventReassignmentUser.Properties.AuthorizationId, "RE").GetPredicate());

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "AU").Equal().Constant(IdGroup)
                    .And().Property(EVENTEN.CoEventReassignmentUser.Properties.ReassUserId, "RE").Equal().Constant(IdUser)
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.EventDate, "AU").LessEqual().Constant(DateEnd)
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.EventDate, "AU").GreaterEqual().Constant(DateStart);

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {

                        var ExistGroup = list.Where(x =>
                             x.Operation1Id == (string)reader["Operation1Id"] &&
                             x.EventId == (int)reader["EventId"] &&
                             x.GroupEventId == (int)reader["GroupEventId"]
                         ).FirstOrDefault();

                        if (ExistGroup != null)
                        {
                            var AUTHORIZATION_ID = ExistGroup.AuthorizationId + "-" + reader["AuthorizationId"].ToString();
                            list.Remove(ExistGroup);
                            ExistGroup.AuthorizationId = AUTHORIZATION_ID;
                            ExistGroup.Count = AUTHORIZATION_ID.Split('-').Count();
                            list.Add(ExistGroup);
                        }
                        else
                        {
                            list.Add(new Models.EventAuthorization
                            {
                                Count = 1,
                                Operation1Id = (string)reader["Operation1Id"],
                                Operation2Id = Convert.ToDecimal(reader["Operation1Id"].ToString()),
                                AuthorizationId = reader["AuthorizationId"].ToString(),
                                EventDate = (DateTime)reader["EventDate"],
                                StrEventDate = ((DateTime)reader["EventDate"]).ToString("dd/MM/yyyy"),
                                EventId = (int)reader["EventId"],
                                GroupEventId = (int)reader["GroupEventId"],
                                EntityDescriptionValues = reader["EntityDescriptionValues"] == null ? "" : reader["EntityDescriptionValues"].ToString(),
                            });
                        }
                    }
                }

                EventCompanyDAO eventCompanyDAO = new EventCompanyDAO();
                foreach (var item in list)
                {
                    item.EventName = eventCompanyDAO.GetEventByIdEventGroupIdEvent(item.GroupEventId, item.EventId).Description;
                }

                return list.OrderByDescending(x => x.EventDate).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventAuthorizationReassignByIdGroupIdUserDateStartDateEnd", ex);
            }
        }

        /// <summary>
        /// crea las autorizaciones para cada uno de los eventos del temporal
        /// </summary>
        /// <param name="delegation">lista de delegation result a crear</param>
        /// <param name="IdUser">id del usuario</param>
        public void CreateEventAuthorizationByIdTempIduser(List<Models.EventDelegationResult> delegation, int IdUser)
        {
            try
            {
                foreach (Models.EventDelegationResult item in delegation)
                {
                    foreach (string ResultId in item.ResultId.Split('-'))
                    {
                        NameValue[] parameters = new NameValue[5];
                        parameters[0] = new NameValue("RESULT_ID", Convert.ToInt32(ResultId));
                        parameters[1] = new NameValue("AUTHO_USER_ID", item.IdAuthorizer);
                        parameters[2] = new NameValue("DESCRIPTION", item.ReasonRequest);
                        parameters[3] = new NameValue("HAS_FILES", false);
                        parameters[4] = new NameValue("NOTIF_USER_ID", item.IdNotifier);
                        using (DynamicDataAccess pdb = new DynamicDataAccess())
                        {
                            pdb.ExecuteSPScalar("EVE.CO_SAVE_TEMP_SHIP", parameters);
                        }
                    }
                }

                var operations = delegation.GroupBy(x => x.IdTemporal);
                foreach (var operation in operations)
                {
                    NameValue[] paramsp = new NameValue[9];
                    paramsp[0] = new NameValue("MODULE_CD", delegation[0].ModuleId);
                    paramsp[1] = new NameValue("SUBMODULE_CD", delegation[0].SubModuleId);
                    paramsp[2] = new NameValue("USER_ID", IdUser);
                    paramsp[3] = new NameValue("OPERATION1_ID", operation.Key);
                    paramsp[4] = new NameValue("KEY1_FIELD_IN", null, DbType.String);
                    paramsp[5] = new NameValue("KEY2_FIELD_IN", null, DbType.String);
                    paramsp[6] = new NameValue("KEY3_FIELD_IN", null, DbType.String);
                    paramsp[7] = new NameValue("KEY4_FIELD_IN", null, DbType.String);
                    paramsp[8] = new NameValue("@OPERATION2_ID", delegation[0].Operation2Id);

                    using (DynamicDataAccess pdb = new DynamicDataAccess())
                    {
                        pdb.ExecuteSPScalar("EVE.CO_SP_EVENT_AUTHORIZA_TRANSFER", paramsp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateEventAuthorizationByIdTempIduser", ex);
            }
        }

        /// <summary>
        /// obtiene los eventos pendientes de autorizacion para el usuario
        /// </summary>
        /// <param name="IdUser">id del usuario</param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetCountEventsByIdUser(int IdUser)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventId, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "AU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventDate, "AU")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorization.Properties.AuthoUserId, "AU").Equal().Constant(IdUser)
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.AuthorizedInd, "AU").Equal().Constant(0)
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.RejectInd, "AU").Equal().Constant(0)
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "AU").Distinct().Constant(8);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventAuthorization), "AU");
                select.Where = where.GetPredicate();

                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EventAuthorization
                        {
                            AuthorizationId = reader["AuthorizationId"].ToString(),
                            EventDate = (DateTime)reader["EventDate"],
                            StrEventDate = ((DateTime)reader["EventDate"]).ToString("dd/MM/yyyy"),
                            EventId = (int)reader["EventId"],
                            GroupEventId = (int)reader["GroupEventId"],
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetCountEventsByIdUser", ex);
            }
        }

        /// <summary>
        /// obtiene todos los eventos generados para el temporal
        /// </summary>
        /// <param name="idOperation">id del temporal</param>
        /// <returns>lsita de Models.EventAuthorization</returns>
        public List<Models.EventAuthorization> GetEventAuthorizationByIdOperation(int idOperation)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Operation1Id, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizedInd, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.RejectInd, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventUserId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventDate, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EventId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.GroupEventId, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Description, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizationDescription, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.EntityDescriptionValues, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.HierarchyCode, "A")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthoUserId, "A")));


                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorization.Properties.Operation1Id, "A").Equal().Constant(idOperation.ToString());

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventAuthorization), "A");
                select.Where = where.GetPredicate();



                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EventAuthorization
                        {
                            AuthorizedInd = (bool)reader["AuthorizedInd"],
                            RejectInd = (bool)reader["RejectInd"],
                            AuthorizationId = ((int)reader["AuthorizationId"]).ToString(),
                            Operation1Id = (reader["Operation1Id"].ToString()),
                            EventUserId = (int)reader["EventUserId"],
                            EventDate = (DateTime)reader["EventDate"],
                            EventId = (int)reader["EventId"],
                            GroupEventId = (int)reader["GroupEventId"],
                            Description = (string)reader["Description"],
                            AuthorizationDescription = (string)reader["AuthorizationDescription"],
                            EntityDescriptionValues = reader["EntityDescriptionValues"] == null ? "" : reader["EntityDescriptionValues"].ToString(),
                            HierachyCd = (int)reader["HierarchyCode"],
                            AuthUserID = (int)reader["AuthoUserId"]
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventAuthorizationByIdOperation", ex);
            }
        }

        /// <summary>
        /// realiza el conteo de los eventos por riesgo para el temporal
        /// </summary>
        /// <param name="idTemp">id del temporal</param>
        /// <returns></returns>
        public Dictionary<string, int> GetCountEventsRisk(int idTemp)
        {
            try
            {
                Dictionary<string, int> listEvents = new Dictionary<string, int>();

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.Operation2Id, "eve")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorization.Properties.AuthorizedInd, "eve")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorization.Properties.Operation1Id, "eve").Equal().Constant(idTemp.ToString())
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.Operation2Id, "eve").IsNotNull()
                    .And().Property(EVENTEN.CoEventAuthorization.Properties.Operation2Id, "eve").Distinct().Constant("");

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventAuthorization), "eve");
                select.Where = where.GetPredicate();

                List<Models.EventAuthorization> listRisk = new List<Models.EventAuthorization>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        listRisk.Add(new Models.EventAuthorization
                        {
                            Operation2Id = Int32.Parse(reader["Operation2Id"].ToString()),
                            AuthorizedInd = Boolean.Parse(reader["AuthorizedInd"].ToString())
                        });
                    }
                }

                foreach (var item in listRisk)
                {
                    if (!listEvents.ContainsKey(item.Operation2Id.ToString()))
                    {
                        if (item.AuthorizedInd == true)
                        {
                            listEvents.Add(item.Operation2Id.ToString(), 0);
                        }
                        else
                        {
                            listEvents.Add(item.Operation2Id.ToString(), 1);
                        }
                    }
                    else
                    {
                        int valor;
                        listEvents.TryGetValue(item.Operation2Id.ToString(), out valor);

                        if (!item.AuthorizedInd == true)
                        {
                            listEvents.Remove(item.Operation2Id.ToString());
                            listEvents.Add(item.Operation2Id.ToString(), valor + 1);
                        }
                    }
                }

                return listEvents;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetCountEventsRisk", ex);
            }
        }
    }
}
