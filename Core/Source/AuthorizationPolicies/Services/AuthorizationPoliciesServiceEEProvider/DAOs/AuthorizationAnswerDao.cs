namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Assemblers;
    using Co.Application.Data;
    using Entities.Views;
    using Enums;
    using Framework.Contexts;
    using Framework.DAF;
    using Framework.DAF.Engine;
    using Framework.Queries;
    using Framework.Transactions;
    using Models;
    using Services.UtilitiesServices.Models;
    using UniqueUserServices.Enums;
    using UniqueUserServices.Models;
    using Utilities.DataFacade;
    using Utilities.Helper;
    using APEntity = AuthorizationPolicies.Entities;
    using UUEntiy = UniqueUser.Entities;
    using UUModel = UniqueUserServices.Models;

    public class AuthorizationAnswerDao
    {

        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion 
        /// </summary>
        /// <param name="answer">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        public AuthorizationAnswer CreateAuthorizationAnswer(AuthorizationAnswer answer)
        {
            APEntity.AuthorizationAnswer entity = EntityAssembler.CreateAuthorizationAnswer(answer);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            answer.AuthorizationAnswerId = entity.AuthorizationAnswerId;

            return answer;
        }

        ///  <summary>
        /// consulta las autorizacion de politicas segun el filtro
        ///  </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="idUser">Id del usuuario autorizador</param>
        /// <param name="status"> estado de la politica</param>
        /// <param name="dateInit">  fecha inicial</param>
        /// <param name="dateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        public List<AuthorizationAnswerGroup> GetAuthorizationAnswersByFilter(int? idGroup, int? idPolicies, int idUser, int status, DateTime? dateInit, DateTime? dateEnd, string sort)
        {
            // dateInit = dateInit ?? DateTime.Today.AddDays(-7);
            dateInit = dateInit ?? DateTime.Today;
            dateEnd = dateEnd ?? DateTime.Now;

            NameValue[] parameters = new NameValue[7];
            parameters[0] = new NameValue("@idGroup", idGroup);
            parameters[1] = new NameValue("@idPolicies", idPolicies);
            parameters[2] = new NameValue("@idUser", idUser);
            parameters[3] = new NameValue("@status", status);
            parameters[4] = new NameValue("@dateInit", dateInit);
            parameters[5] = new NameValue("@dateEnd", dateEnd);
            parameters[6] = new NameValue("@sort", sort);

            List<AuthorizationAnswerGroup> answerGroups = new List<AuthorizationAnswerGroup>();
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                DataTable datas = dynamicDataAccess.ExecuteSPDataTable("AUTHO.GET_SUMMARY_POLICIES", parameters);


                foreach (DataRow objeData in datas.Rows)
                {
                    answerGroups.Add(new AuthorizationAnswerGroup
                    {
                        GroupPoliciesId = (int)objeData.ItemArray[0],
                        DescriptionGroup = (string)objeData.ItemArray[1],
                        PoliciesId = (int)objeData.ItemArray[2],
                        DescriptionPolicie = (string)objeData.ItemArray[3],
                        Key = (string)objeData.ItemArray[4],
                        AccountName = (string)objeData.ItemArray[5],
                        DateRequest = DateTime.ParseExact((string)objeData.ItemArray[6], "yyyy/MM/dd", CultureInfo.InvariantCulture),
                        Required = (bool)objeData.ItemArray[7],
                        DescriptionRequest = (string)objeData.ItemArray[8],
                        Count = (int)objeData.ItemArray[9],
                        UserAnswerId = (int)objeData.ItemArray[10],
                        HierarchyAnswerId = (int)objeData.ItemArray[11],
                        DescriptionAnswer = objeData.ItemArray[12].ToString(),
                        DateAnswer = !string.IsNullOrEmpty(objeData.ItemArray[13].ToString()) ? DateTime.ParseExact(objeData.ItemArray[13].ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture) : (DateTime?)null,
                        FunctionType = (TypeFunction)Enum.Parse(typeof(TypeFunction), objeData.ItemArray[14].ToString())
                    });
                }
            }

            return answerGroups;
        }

        ///  <summary>
        /// consulta las autorizacion de politicas que han sido reasignadas por el usuario
        ///  </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="idUser">Id del usuuario autorizador</param>
        /// <param name="status"> estado de la politica</param>
        /// <param name="dateInit">  fecha inicial</param>
        /// <param name="dateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        public List<AuthorizationAnswerGroup> GetAuthorizationAnswersReasignByFilter(int? idGroup, int? idPolicies, int idUser, DateTime? dateInit, DateTime? dateEnd, string sort)
        {
            dateInit = dateInit ?? DateTime.Now;
            dateEnd = dateEnd ?? DateTime.Now;

            NameValue[] parameters = new NameValue[6];
            parameters[0] = new NameValue("@idGroup", idGroup);
            parameters[1] = new NameValue("@idPolicies", idPolicies);
            parameters[2] = new NameValue("@idUser", idUser);
            parameters[3] = new NameValue("@dateInit", dateInit);
            parameters[4] = new NameValue("@dateEnd", dateEnd);
            parameters[5] = new NameValue("@sort", sort);


            List<AuthorizationAnswerGroup> answerGroups = new List<AuthorizationAnswerGroup>();
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                DataTable datas = dynamicDataAccess.ExecuteSPDataTable("AUTHO.GET_SUMMARY_REASIGN_POLICIES", parameters);


                foreach (DataRow objeData in datas.Rows)
                {
                    answerGroups.Add(new AuthorizationAnswerGroup
                    {
                        GroupPoliciesId = (int)objeData.ItemArray[0],
                        DescriptionGroup = (string)objeData.ItemArray[1],
                        PoliciesId = (int)objeData.ItemArray[2],
                        DescriptionPolicie = (string)objeData.ItemArray[3],
                        Key = (string)objeData.ItemArray[4],
                        AccountName = (string)objeData.ItemArray[5],
                        DateRequest = DateTime.ParseExact((string)objeData.ItemArray[6], "yyyy/MM/dd", CultureInfo.InvariantCulture),
                        Required = false,
                        DescriptionRequest = (string)objeData.ItemArray[7],
                        Count = (int)objeData.ItemArray[8],
                        UserAnswerId = (int)objeData.ItemArray[9],
                        HierarchyAnswerId = (int)objeData.ItemArray[10],
                        DescriptionAnswer = objeData.ItemArray[11].ToString(),
                        DateAnswer = !string.IsNullOrEmpty(objeData.ItemArray[12].ToString()) ? DateTime.ParseExact(objeData.ItemArray[12].ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture) : (DateTime?)null,
                        FunctionType = (TypeFunction)Enum.Parse(typeof(TypeFunction), objeData.ItemArray[13].ToString())
                    });
                }
            }

            return answerGroups;
        }

        public List<string> GetAuthorizationAnswerDescriptions(int idPolicies, int idUser, int status, string key)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.Description, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa")));

            Join join = new Join(new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal()
                    .Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            where.Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar").Equal().Constant(idPolicies);
            where.And().Property(APEntity.AuthorizationRequest.Properties.Key, "ar").Equal().Constant(key);
            where.And().Property(APEntity.AuthorizationAnswer.Properties.UserAnswerId, "aa").Equal().Constant(idUser);

            if (status != -1)
            {
                where.And().Property(APEntity.AuthorizationAnswer.Properties.StatusId, "aa").Equal().Constant(status);
            }


            select.Table = join;
            select.Where = where.GetPredicate();

            List<string> authorizationAnswers = new List<string>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    authorizationAnswers.Add(reader["AuthorizationRequestId"].ToString() + "|" + reader["Description"].ToString());
                }
            }

            return authorizationAnswers;
        }

        /// <summary>
        /// consultar las jerarquias superiores parametrizadas a la politica
        /// </summary>
        /// <param name="policiesId">id de la politica</param>
        /// <param name="hierarchyId">jerarquia del usuario actual</param>
        /// <param name="userId">id del usuario actual</param>
        /// <returns>lista de las jerarquias autorizadoras</returns>
        public List<CoHierarchyAssociation> GetAuthorizationHierarchy(int policiesId, int hierarchyId, int userId)
        {
            List<CoHierarchyAssociation> listHierarchy = new List<CoHierarchyAssociation>();


            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.CoHierarchyAssociation.Properties.HierarchyCode,
                "cha")));
            select.AddSelectValue(
                new SelectValue(new Column(UUEntiy.CoHierarchyAssociation.Properties.Description, "cha")));

            select.Distinct = true;

            Join join = new Join(new ClassNameTable(typeof(APEntity.UserAuthorization), "ua"),
                new ClassNameTable(typeof(APEntity.Policies), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.UserAuthorization.Properties.PoliciesId, "ua").Equal()
                    .Property(APEntity.Policies.Properties.PoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal()
                    .Property(APEntity.Policies.Properties.GroupPoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntiy.CoHierarchyAssociation), "cha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").Equal()
                    .Property(UUEntiy.CoHierarchyAssociation.Properties.ModuleCode, "cha")

                    .And().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").Equal()
                    .Property(UUEntiy.CoHierarchyAssociation.Properties.SubmoduleCode, "cha")

                    .And().Property(APEntity.UserAuthorization.Properties.HierarchyId, "ua").Equal()
                    .Property(UUEntiy.CoHierarchyAssociation.Properties.HierarchyCode, "cha").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.Policies.Properties.PoliciesId, "p").Equal().Constant(policiesId);
            where.And().Property(APEntity.UserAuthorization.Properties.HierarchyId, "ua").LessEqual()
                .Constant(hierarchyId);
            where.And().Property(APEntity.UserAuthorization.Properties.UserId, "ua").Distinct().Constant(userId);

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade()
                .Select(select))
            {
                while (reader.Read())
                {
                    listHierarchy.Add(new CoHierarchyAssociation
                    {
                        Id = (int)reader["HierarchyCode"],
                        Description = (string)reader["Description"]
                    });
                }
            }

            return listHierarchy;
        }

        /// <summary>
        /// consultar los usuarios autorizadores de la politica en esa jerarquia
        /// </summary>
        /// <param name="autorizatioAnswerId">id de la autorizacion</param> 
        /// <param name="hierarchyId">jerarquia autorizadora</param>
        /// <returns>lista usuarios autorizadores de la jerarquia</returns>
        public List<User> GetUsersAuthorizationHierarchy(int autorizatioAnswerId, int hierarchyId)
        {
            List<User> listUsers = new List<User>();


            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.UniqueUsers.Properties.UserId, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.UniqueUsers.Properties.AccountName, "uu")));


            Join join = new Join(new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"),
                new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal()
                    .Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.Policies), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar").Equal()
                    .Property(APEntity.Policies.Properties.PoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal()
                    .Property(APEntity.Policies.Properties.GroupPoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.UserAuthorization), "ua"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.UserAuthorization.Properties.PoliciesId, "ua").Equal()
                    .Property(APEntity.Policies.Properties.PoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntiy.CoHierarchyAssociation), "cha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").Equal()
                    .Property(UUEntiy.CoHierarchyAssociation.Properties.ModuleCode, "cha")

                    .And().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").Equal()
                    .Property(UUEntiy.CoHierarchyAssociation.Properties.SubmoduleCode, "cha")

                    .And().Property(APEntity.UserAuthorization.Properties.HierarchyId, "ua").Equal()
                    .Property(UUEntiy.CoHierarchyAssociation.Properties.HierarchyCode, "cha").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntiy.UniqueUsers), "uu"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(UUEntiy.UniqueUsers.Properties.UserId, "uu").Equal()
                    .Property(APEntity.UserAuthorization.Properties.UserId, "ua").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId, "aa").Equal()
                .Constant(autorizatioAnswerId);
            where.And().Property(APEntity.UserAuthorization.Properties.HierarchyId, "ua").Equal().Constant(hierarchyId);
            where.And().Property(APEntity.UserAuthorization.Properties.UserId, "ua").Distinct()
                .Property(APEntity.AuthorizationAnswer.Properties.UserAnswerId, "aa");

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade()
                .Select(select))
            {
                while (reader.Read())
                {
                    listUsers.Add(new User
                    {
                        UserId = (int)reader["UserId"],
                        AccountName = (string)reader["AccountName"]
                    });
                }
            }

            return listUsers;
        }

        /// <summary>
        /// Realiza el rechazo de las autorizaciones, de forma automatica rechaza las pendientes
        /// </summary>
        /// <param name="answers">lista de autorizaciones</param>
        public void RejectAuthorization(int policiesId, int userAnswerId, string key, string reason, int idRejection, ref EmailCriteria email, List<int> policiesToReject, TypeFunction functionType)
        {
            email.Subject = "Politica rechazada - ";
            email.Addressed = new List<string>();

            ConcurrentBag<string> messages = new ConcurrentBag<string>();
            ConcurrentBag<int> answerstoUpdate = new ConcurrentBag<int>();
            DateTime dateNow = DateTime.Now;

            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ////  SE ARMA LA VIEW
                    ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                    AuthorizationPoliciesGetAuthorizations view = new AuthorizationPoliciesGetAuthorizations();
                    ViewBuilder builder = new ViewBuilder("GetAuthorizations");

                    where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(key);
                    where.And().Property(APEntity.AuthorizationRequest.Properties.FunctionId, "AutorizarionRequest").Equal().Constant((int)functionType);
                    where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Equal().Constant((int)TypeStatus.Pending);

                    builder.Filter = where.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    //// se obtiene los valores de la VIEW
                    List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.Cast<APEntity.AuthorizationRequest>().ToList();
                    List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.Cast<APEntity.AuthorizationAnswer>().ToList();
                    List<APEntity.Policies> totalPolicies = view.Policies.Cast<APEntity.Policies>().ToList();
                    List<UUEntiy.UniqueUsers> totalUsers = view.Users.Cast<UUEntiy.UniqueUsers>().ToList();

                    //// 
                    List<APEntity.AuthorizationRequest> requests = new List<APEntity.AuthorizationRequest>();
                    if (policiesToReject != null)
                    {
                        requests = totalRequests.Where(x => x.PoliciesId == policiesId && policiesToReject.Contains(x.AuthorizationRequestId)).ToList();
                    }
                    else
                    {
                        requests = totalRequests.Where(x => x.PoliciesId == policiesId).ToList();
                    }

                    if (requests.Any())
                    {
                        List<APEntity.AuthorizationAnswer> answers = totalAnswers.Where(x => requests.Select(y => y.AuthorizationRequestId).Contains(x.AuthorizationRequestId)).ToList();
                        List<APEntity.Policies> policies = totalPolicies.Where(x => x.PoliciesId == policiesId).ToList();
                        List<UUEntiy.UniqueUsers> users = totalUsers.Where(x => x.UserId == userAnswerId).ToList();

                        ///// Se actualizan los answer
                        bool sendNotifications = false;
                        ParallelHelper.ForEach(answers.Where(x => x.StatusId == (int)TypeStatus.Pending && x.UserAnswerId == userAnswerId).ToList(), answer =>
                        {
                            sendNotifications = true;

                            APEntity.AuthorizationRequest request = requests.First(x => x.AuthorizationRequestId == answer.AuthorizationRequestId);

                            answer.DateAnswer = dateNow;
                            answer.DescriptionAnswer = reason;
                            answer.RejectionCausesId = idRejection;
                            answer.StatusId = (int)TypeStatus.Rejected;

                            answerstoUpdate.Add(answer.AuthorizationAnswerId);

                            messages.Add($"<h2>{policies.First().Description}</h2>" +
                                        $"{policies.First().Message}</br></br>" +
                                        $"Usuario: {users.First().AccountName}</br>" +
                                        $"Motivo de rechazo: {answer.DescriptionAnswer}</br></br>" +
                                        $"{request.Description.Replace("|", "</br>")}");
                        });

                        UpdateQuery update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationAnswer)) };
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer), new Constant(dateNow, DbType.DateTime));
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer), new Constant(reason, DbType.String));
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.StatusId), new Constant((int)TypeStatus.Rejected, DbType.Int16));
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.RejectionCausesId), new Constant(idRejection, DbType.Int16));

                        where.Clear();
                        where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId).In();
                        where.ListValue();
                        answerstoUpdate.ToArray().ToList().ForEach(x => where.Constant(x));
                        where.EndList();

                        update.Where = where.GetPredicate();
                        DataFacadeManager.Instance.GetDataFacade().Execute(update);


                        ///// Se actualizan el resto de los answer



                        //// actualizo los request
                        update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationRequest)) };
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationRequest.Properties.StatusId), new Constant((int)TypeStatus.Rejected, DbType.Int16));

                        where.Clear();
                        where.Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId).In();
                        where.ListValue();
                        requests.ForEach(x => where.Constant(x.AuthorizationRequestId));
                        where.EndList();

                        update.Where = where.GetPredicate();
                        DataFacadeManager.Instance.GetDataFacade().Execute(update);


                        //// Se rechazan los faltantes BY SISE
                        List<APEntity.AuthorizationAnswer> answersToReject = new List<APEntity.AuthorizationAnswer>();
                        IEnumerable<int> filterRiskCoverages;

                        switch ((TypeFunction)requests.First().FunctionId)
                        {
                            case TypeFunction.Massive:
                                filterRiskCoverages = totalRequests.Where(y => requests.Select(x => x.Key2).Contains(y.Key2)).Select(x => x.AuthorizationRequestId);
                                answersToReject = totalAnswers.Where(x => filterRiskCoverages.Contains(x.AuthorizationRequestId) && x.StatusId == (int)TypeStatus.Pending).ToList();
                                break;
                            case TypeFunction.Collective:
                                if (!requests.First().Key2.Contains("|")) // General
                                {
                                    answersToReject = totalAnswers.Where(x => x.StatusId == (int)TypeStatus.Pending).ToList();
                                }
                                else // Riesgo y cobertura
                                {
                                    filterRiskCoverages = totalRequests.Where(y => requests.Select(x => x.Key2).Contains(y.Key2)).Select(x => x.AuthorizationRequestId);
                                    answersToReject = totalAnswers.Where(x => filterRiskCoverages.Contains(x.AuthorizationRequestId) && x.StatusId == (int)TypeStatus.Pending).ToList();
                                }
                                break;
                            default:
                                answersToReject = totalAnswers.Where(x => x.StatusId == (int)TypeStatus.Pending).ToList();
                                break;
                        }

                        if (answersToReject.Any())
                        {
                            //rechachar los anwers 
                            update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationAnswer)) };
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer), new Constant(dateNow, DbType.DateTime));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer), new Constant("Rejected by SISE3G", DbType.String));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.StatusId), new Constant((int)TypeStatus.Rejected, DbType.Int16));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.Enabled), new Constant(false, DbType.Boolean));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.RejectionCausesId), new Constant(idRejection, DbType.Int16));

                            where.Clear();
                            where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId).In();
                            where.ListValue();
                            answersToReject.AsParallel().ToList().ForEach(x => where.Constant(x.AuthorizationAnswerId));
                            where.EndList();

                            update.Where = where.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().Execute(update);

                            ///// rechazar los request
                            update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationRequest)) };
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationRequest.Properties.StatusId), new Constant((int)TypeStatus.Rejected, DbType.Int16));

                            where.Clear();
                            where.Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId).In();
                            where.ListValue();
                            answersToReject.AsParallel().ToList().ForEach(x => where.Constant(x.AuthorizationRequestId));
                            where.EndList();

                            update.Where = where.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().Execute(update);
                        }

                        if (sendNotifications)
                        {
                            TypeFunction function = (TypeFunction)Enum.Parse(typeof(TypeFunction), requests.First().FunctionId.ToString());
                            UUModel.NotificationUser notificationUser = new UUModel.NotificationUser { UserId = requests.First().UserRequestId };
                            switch (function)
                            {
                                case TypeFunction.Individual:
                                    notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Temporal {key}). Motivo: {reason}";
                                    notificationUser.NotificationType = new UUModel.NotificationType { Type = NotificationTypes.RejectPolicies };
                                    break;

                                case TypeFunction.Massive:
                                case TypeFunction.Collective:
                                    notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Cargue {key}). Motivo: {reason}";
                                    notificationUser.NotificationType = new UUModel.NotificationType { Type = NotificationTypes.RejectPoliciesMassive };
                                    notificationUser.Parameters = new Dictionary<string, object> { { "Load", key } };
                                    break;
                                case TypeFunction.Claim:
                                    notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Denuncia {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.RejectPoliciesClaim };
                                    notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "Claim", key
                                    }
                                };
                                    break;
                                case TypeFunction.PaymentRequest:
                                    notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Solicitud de pago {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.RejectPoliciesPaymentRequest };
                                    notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "PaymentRequest", key
                                    }
                                };
                                break;
                            case TypeFunction.ChargeRequest:
                                notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Solicitud de cobro {key})";
                                notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.RejectPoliciesChargeRequest };
                                notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "ChargeRequest", key
                                    }
                                };
                                break;
                            case TypeFunction.ClaimNotice:
                                notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Aviso {key})";
                                notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.RejectPoliciesClaimNotice };
                                notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "ClaimNotice", key
                                    }
                                };
                                    break;
                                case TypeFunction.PersonGeneral:
                                case TypeFunction.PersonInsured:
                                case TypeFunction.PersonProvider:
                                case TypeFunction.PersonThird:
                                case TypeFunction.PersonIntermediary:
                                case TypeFunction.PersonEmployed:
                                case TypeFunction.PersonPersonalInf:
                                case TypeFunction.PersonPaymentMethods:
                                case TypeFunction.PersonGuarantees:
                                case TypeFunction.PersonOperatingQuota:
                                case TypeFunction.PersonTaxes:
                                case TypeFunction.PersonBankTransfers:
                                case TypeFunction.PersonReinsurer:
                                case TypeFunction.PersonCoinsurer:
                                case TypeFunction.PersonConsortiates:
                                case TypeFunction.PersonBusinessName:
                                case TypeFunction.PersonBasicInfo:
                                    notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Temporal {key}). Motivo: {reason}";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.RejectPolicies };
                                    break;


                                case TypeFunction.SarlaftGeneral:
                                    notificationUser.Message = $"{users.First().AccountName} ha rechazado la Politica - {policies.First().Description} (Temporal {key}). Motivo: {reason}";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.RejectPolicies };
                                    break;
                            }
                            DelegateService.UniqueUserService.CreateNotification(notificationUser);
                        }

                        email.Subject += key;
                        email.Message = string.Join(" ", messages);
                        email.Addressed.Add(requests.First().UserRequestId.ToString());
                    }
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Realiza la autorizacion de las autorizaciones pendientes
        /// </summary>
        /// <param name="answers">lista de autorizaciones</param>
        public List<AuthorizationRequest> AcceptAuthorization(int policiesId, int userAnswerId, string key, string reason, ref EmailCriteria email, List<int> policiesToAccept, TypeFunction functionType)
        {
            email.Subject = "Politica autorizada - ";
            email.Addressed = new List<string>();

            List<string> messages = new List<string>();
            ConcurrentBag<int> requestsToUpdate = new ConcurrentBag<int>();
            ConcurrentBag<int> answerstoUpdate = new ConcurrentBag<int>();

            DateTime dateNow = DateTime.Now;

            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ////  SE ARMA LA VIEW
                    ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                    AuthorizationPoliciesGetAuthorizations view = new AuthorizationPoliciesGetAuthorizations();
                    ViewBuilder builder = new ViewBuilder("GetAuthorizations");

                    where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(key);
                    where.And().Property(APEntity.AuthorizationRequest.Properties.FunctionId, "AutorizarionRequest").Equal().Constant((int)functionType);
                    where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Equal().Constant((int)TypeStatus.Pending);

                    builder.Filter = where.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    //// se obtiene los valores de la VIEW
                    List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.Cast<APEntity.AuthorizationRequest>().ToList();
                    List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.Cast<APEntity.AuthorizationAnswer>().ToList();
                    List<APEntity.Policies> totalPolicies = view.Policies.Cast<APEntity.Policies>().ToList();
                    List<UUEntiy.UniqueUsers> totalUsers = view.Users.Cast<UUEntiy.UniqueUsers>().ToList();

                    List<AuthorizationRequest> result = new List<AuthorizationRequest>();

                    //// 
                    List<APEntity.AuthorizationRequest> requests = new List<APEntity.AuthorizationRequest>();
                    if (policiesToAccept != null)
                    {
                        requests = totalRequests.Where(x => x.PoliciesId == policiesId && policiesToAccept.Contains(x.AuthorizationRequestId)).ToList();
                    }
                    else
                    {
                        requests = totalRequests.Where(x => x.PoliciesId == policiesId).ToList();
                    }

                    if (requests.Any())
                    {
                        List<APEntity.AuthorizationAnswer> answers = totalAnswers.Where(x => requests.Select(y => y.AuthorizationRequestId).Contains(x.AuthorizationRequestId)).ToList();
                        List<APEntity.Policies> policies = totalPolicies.Where(x => x.PoliciesId == policiesId).ToList();
                        List<UUEntiy.UniqueUsers> users = totalUsers.Where(x => x.UserId == userAnswerId).ToList();

                        ///// Se actualizan los answer
                        List<APEntity.AuthorizationAnswer> anwerToAuthorize = answers.Where(x => x.StatusId == (int)TypeStatus.Pending && x.UserAnswerId == userAnswerId).ToList();
                        ParallelHelper.ForEach(anwerToAuthorize, answer =>
                        {
                            answer.DateAnswer = dateNow;
                            answer.DescriptionAnswer = reason;
                            answer.StatusId = (int)TypeStatus.Authorized;

                            answerstoUpdate.Add(answer.AuthorizationAnswerId);
                        });

                        UpdateQuery update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationAnswer)) };
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer), new Constant(dateNow, DbType.DateTime));
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer), new Constant(reason, DbType.String));
                        update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.StatusId), new Constant((int)TypeStatus.Authorized, DbType.Int16));

                        where.Clear();
                        where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId).In();
                        where.ListValue();
                        answerstoUpdate.ToArray().ToList().ForEach(x => where.Constant(x));
                        where.EndList();

                        update.Where = where.GetPredicate();
                        DataFacadeManager.Instance.GetDataFacade().Execute(update);


                        //// 

                        ConcurrentBag<int> answerstoUpdate2 = new ConcurrentBag<int>();
                        bool sendNotifications = false;

                        var queryCount = (from ar in requests
                                          join aa in answers on ar.AuthorizationRequestId equals aa.AuthorizationRequestId
                                          group aa by ar.AuthorizationRequestId into g
                                          select new { requestId = g.Key, data = g.ToList() }).ToList();


                        ParallelHelper.ForEach(queryCount.ToList(), item =>
                        {
                            APEntity.AuthorizationRequest request = requests.First(x => x.AuthorizationRequestId == item.requestId);
                            int autRequired = item.data.Count(x => x.Required && x.StatusId == (int)TypeStatus.Pending);
                            int autNum = item.data.Count(x => x.StatusId == (int)TypeStatus.Authorized);

                            if (autRequired == 0 && autNum >= request.NumberAut)
                            {
                                sendNotifications = true;

                                APEntity.Policies policie = policies.First(x => x.PoliciesId == request.PoliciesId);
                                UUEntiy.UniqueUsers user = users.First(x => x.UserId == userAnswerId);

                                messages.Add($"<h2>{policie.Description}</h2>" +
                                           $"{policie.Message}</br></br>" +
                                           $"Usuario: {user.AccountName}</br>" +
                                           $"Motivo de autorizacion: {reason}</br></br>" +
                                           $"{request.Description.Replace("|", "</br>")}");

                                requestsToUpdate.Add(request.AuthorizationRequestId);
                                item.data.Where(x => x.StatusId == (int)TypeStatus.Pending).ToList().ForEach(x => answerstoUpdate2.Add(x.AuthorizationAnswerId));
                            }
                        });


                        //// actualizo los request
                        if (requestsToUpdate.Count > 0)
                        {
                            update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationRequest)) };
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationRequest.Properties.StatusId),
                                new Constant((int)TypeStatus.Authorized, DbType.Int16));

                            where.Clear();
                            where.Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId).In();
                            where.ListValue();
                            requestsToUpdate.ToArray().ToList().ForEach(x => where.Constant(x));
                            where.EndList();

                            update.Where = where.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().Execute(update);
                        }
                        totalRequests.ForEach(x =>
                        {
                            if (requestsToUpdate.Contains(x.AuthorizationRequestId))
                            {
                                x.StatusId = (int)TypeStatus.Authorized;
                            }
                        });


                        //// Actualizo los faltantes BY SISE
                        if (answerstoUpdate2.Count > 0)
                        {
                            update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationAnswer)) };
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer), new Constant(dateNow, DbType.DateTime));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer), new Constant("Autorized by SISE3G", DbType.String));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.StatusId), new Constant((int)TypeStatus.Authorized, DbType.Int16));
                            update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.Enabled), new Constant(false, DbType.Boolean));

                            where.Clear();
                            where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId).In();
                            where.ListValue();
                            answerstoUpdate2.ToArray().ToList().ForEach(x => where.Constant(x));
                            where.EndList();

                            update.Where = where.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().Execute(update);
                        }

                        if (sendNotifications)
                        {
                            TypeFunction function = (TypeFunction)Enum.Parse(typeof(TypeFunction), requests.First().FunctionId.ToString());
                            UUModel.NotificationUser notificationUser = new UUModel.NotificationUser { UserId = requests.First().UserRequestId };
                            switch (function)
                            {
                                case TypeFunction.Individual:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Temporal {key})";
                                    notificationUser.NotificationType = new UUModel.NotificationType { Type = NotificationTypes.AcceptPolicies };
                                    break;
                                case TypeFunction.Massive:
                                case TypeFunction.Collective:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Cargue {key})";
                                    notificationUser.NotificationType = new UUModel.NotificationType { Type = NotificationTypes.AcceptPoliciesMassive };
                                    notificationUser.Parameters = new Dictionary<string, object> { { "Load", key } };
                                    break;
                                case TypeFunction.Claim:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Denuncia {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPoliciesClaim };
                                    notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "Claim", key
                                    }
                                };
                                    break;
                                case TypeFunction.PaymentRequest:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Solicitud de pago {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPoliciesPaymentRequest };
                                    notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "PaymentRequest", key
                                    }
                                };
                                break;
                            case TypeFunction.ChargeRequest:
                                notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Solicitud de cobro {key})";
                                notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPoliciesChargeRequest };
                                notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "ChargeRequest", key
                                    }
                                };
                                break;
                            case TypeFunction.ClaimNotice:
                                notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Temporal de aviso: {key})";
                                notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPoliciesClaimNotice };
                                notificationUser.Parameters = new Dictionary<string, object>
                                {
                                    {
                                        "ClaimNotice", key
                                    }
                                };
                                    break;
                                case TypeFunction.PersonGeneral:
                                case TypeFunction.PersonInsured:
                                case TypeFunction.PersonProvider:
                                case TypeFunction.PersonThird:
                                case TypeFunction.PersonIntermediary:
                                case TypeFunction.PersonEmployed:
                                case TypeFunction.PersonPersonalInf:
                                case TypeFunction.PersonPaymentMethods:
                                case TypeFunction.PersonGuarantees:
                                case TypeFunction.PersonOperatingQuota:
                                case TypeFunction.PersonTaxes:
                                case TypeFunction.PersonBankTransfers:
                                case TypeFunction.PersonReinsurer:
                                case TypeFunction.PersonCoinsurer:
                                case TypeFunction.PersonConsortiates:
                                case TypeFunction.PersonBusinessName:
                                case TypeFunction.PersonBasicInfo:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Temporal {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPolicies };
                                    break;

                                case TypeFunction.SarlaftGeneral:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Temporal {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPolicies };
                                    break;
                                case TypeFunction.AutomaticQuota:
                                    notificationUser.Message = $"{users.First().AccountName} ha autorizado la Politica - {policies.First().Description} (Temporal {key})";
                                    notificationUser.NotificationType = new NotificationType { Type = NotificationTypes.AcceptPolicies };
                                    break;
                            }
                            DelegateService.UniqueUserService.CreateNotification(notificationUser);
                        }

                        email.Subject += key;
                        email.Message = string.Join(" ", messages);
                        email.Addressed.Add(requests.First().UserRequestId.ToString());


                        //// Answers a validar
                        var queryCount2 = (from ar in totalRequests
                                           group ar by new { ar.Key, ar.Key2, ar.StatusId } into g
                                           select new { key = g.Key, data = g.ToList() }).ToList();

                        foreach (var item in queryCount2.Where(x => x.key.StatusId == (int)TypeStatus.Authorized))
                        {
                            int count = queryCount2.Count(x => x.key.Key == item.key.Key && x.key.Key2 == item.key.Key2 && x.key.StatusId != item.key.StatusId);

                            if (count == 0)
                            {
                                result.AddRange(item.data.DistinctBy(x => new { x.Key, x.Key2 }).Select(ar => new AuthorizationRequest
                                {
                                    AuthorizationRequestId = ar.AuthorizationRequestId,
                                    UserRequest = new User { UserId = ar.UserRequestId },
                                    Key = ar.Key,
                                    Key2 = ar.Key2,
                                    FunctionType = (TypeFunction)ar.FunctionId
                                }));
                            }
                        }

                    }
                    transaction.Complete();
                    return result;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Actualiza Enabled =0
        /// </summary>
        /// <param name="key1">key1</param>
        /// <param name="key2">key2</param>
        public void UpdateAuthorizationAnswer(string key1, string key2)
        {
            AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
            List<AuthorizationRequest> resquest = authorizationRequestDao.GetAuthorizationRequestsByKeyKey2(key1, key2);

            foreach (AuthorizationRequest item in resquest)
            {

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, typeof(APEntity.AuthorizationAnswer).Name)
                    .Equal().Constant(item.AuthorizationRequestId);
                filter.And();
                filter.Property(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer, typeof(APEntity.AuthorizationAnswer).Name)
                  .IsNull();
                filter.And();
                filter.Property(APEntity.AuthorizationAnswer.Properties.DateAnswer, typeof(APEntity.AuthorizationAnswer).Name)
                  .IsNull();

                List<APEntity.AuthorizationAnswer> answers = new BusinessCollection<APEntity.AuthorizationAnswer>(DataFacadeManager.Instance.GetDataFacade()
                           .SelectObjects(typeof(APEntity.AuthorizationAnswer), filter.GetPredicate()))
                       .Cast<APEntity.AuthorizationAnswer>().ToList();

                foreach (APEntity.AuthorizationAnswer answer in answers)
                {
                    answer.Enabled = false;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(answer);
                }
            }
        }

        public void UpdateAuthorizationAnswersByAuthorizationRequests(List<AuthorizationRequest> authorizationRequests, string userName)
        {
            foreach (AuthorizationRequest authorizationRequest in authorizationRequests)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, typeof(APEntity.AuthorizationAnswer).Name)
                    .Equal().Constant(authorizationRequest.AuthorizationRequestId);

                List<APEntity.AuthorizationAnswer> answers = new BusinessCollection<APEntity.AuthorizationAnswer>(DataFacadeManager.Instance.GetDataFacade()
                           .SelectObjects(typeof(APEntity.AuthorizationAnswer), filter.GetPredicate()))
                       .Cast<APEntity.AuthorizationAnswer>().ToList();

                foreach (APEntity.AuthorizationAnswer answer in answers)
                {
                    answer.Enabled = false;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(answer);
                }
            }
        }

        public List<AuthorizationAnswer> GetAuthorizationAnswersByRequestId(int requestId)
        {
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.UniqueUsers.Properties.AccountName, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.CoHierarchyAssociation.Properties.Description, "cha"), "HierarchyDescription"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Status.Properties.Description, "s"), "StatusDescription"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.StatusId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer, "aa")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer, "aa")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.RejectionCauses.Properties.Description, "rc"), "RejectionDescription"));

            Join join = new Join(new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa")
                    .Equal().Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.Policies), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar")
                    .Equal().Property(APEntity.Policies.Properties.PoliciesId, "p").GetPredicate()

            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp")
                    .Equal().Property(APEntity.Policies.Properties.GroupPoliciesId, "p").GetPredicate()

            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.Status), "s"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.Status.Properties.StatusId, "s")
                    .Equal().Property(APEntity.AuthorizationAnswer.Properties.StatusId, "aa").GetPredicate()

            };

            join = new Join(join, new ClassNameTable(typeof(UUEntiy.CoHierarchyAssociation), "cha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntiy.CoHierarchyAssociation.Properties.ModuleCode, "cha")
                    .Equal().Property(APEntity.GroupPolicies.Properties.ModuleId, "gp")

                    .And().Property(UUEntiy.CoHierarchyAssociation.Properties.SubmoduleCode, "cha")
                    .Equal().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp")

                    .And().Property(UUEntiy.CoHierarchyAssociation.Properties.HierarchyCode, "cha")
                    .Equal().Property(APEntity.AuthorizationAnswer.Properties.HierarchyAnswerId, "aa").GetPredicate()

            };

            join = new Join(join, new ClassNameTable(typeof(UUEntiy.UniqueUsers), "uu"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntiy.UniqueUsers.Properties.UserId, "uu")
                    .Equal().Property(APEntity.AuthorizationAnswer.Properties.UserAnswerId, "aa").GetPredicate()

            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.RejectionCauses), "rc"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.RejectionCauses.Properties.GroupPoliciesId, "rc")
                    .Equal().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp")

                    .And().Property(APEntity.RejectionCauses.Properties.RejectionCausesId, "rc")
                    .Equal().Property(APEntity.AuthorizationAnswer.Properties.RejectionCausesId, "aa").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal().Constant(requestId);

            select.Table = join;
            select.Where = where.GetPredicate();

            List<AuthorizationAnswer> answers = new List<AuthorizationAnswer>();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    AuthorizationAnswer answer = new AuthorizationAnswer
                    {
                        AuthorizationAnswerId = int.Parse(reader["AuthorizationAnswerId"].ToString()),
                        UserAnswer = new UUModel.User { AccountName = reader["AccountName"].ToString() },
                        HierarchyAnswer = new UUModel.CoHierarchyAssociation
                        {
                            Description = reader["HierarchyDescription"].ToString()
                        },
                        StatusDescription = reader["StatusDescription"].ToString(),
                        DateAnswer = reader["DateAnswer"] != null ? DateTime.Parse(reader["DateAnswer"].ToString()) : (DateTime?)null,
                        DescriptionAnswer = reader["DescriptionAnswer"]?.ToString() ?? "",
                        RejectionCausesDescription = reader["RejectionDescription"]?.ToString() ?? "",
                        Status = (TypeStatus)Enum.Parse(typeof(TypeStatus), reader["StatusId"].ToString())
                    };

                    answers.Add(answer);
                }
            }

            return answers;
        }


        public List<string> GetAuthorizationAnswerDescriptions(int idPolicies, string key)
        {


            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.StatusId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer, "aa")));
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.UniqueUsers.Properties.AccountName, "un")));

            Join join = new Join(new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal()
                    .Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(UUEntiy.UniqueUsers), "un"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                   .Property(APEntity.AuthorizationAnswer.Properties.UserAnswerId, "aa").Equal()
                   .Property(UUEntiy.UniqueUsers.Properties.UserId, "un").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            where.Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar").Equal().Constant(idPolicies);
            where.And().Property(APEntity.AuthorizationRequest.Properties.Key, "ar").Equal().Constant(key);



            where.And().Property(APEntity.AuthorizationAnswer.Properties.StatusId, "aa").In();
            where.ListValue();
            where.Constant(2);
            where.Constant(3);
            where.EndList();



            select.Table = join;
            select.Where = where.GetPredicate();

            List<string> authorizationAnswers = new List<string>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    authorizationAnswers.Add(reader["StatusId"].ToString() + "|" + reader["AccountName"].ToString() + "|" + reader["DateAnswer"].ToString());
                }
            }

            return authorizationAnswers;
        }

        public List<User> GetUsersAuthorization(int groupId, int policiesId)
        {
            List<User> listUsers = new List<User>();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.UniqueUsers.Properties.UserId, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEntiy.UniqueUsers.Properties.AccountName, "uu")));

            select.Distinct = true;
            Join join = new Join(new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"),
                new ClassNameTable(typeof(APEntity.Policies), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal()
                    .Property(APEntity.Policies.Properties.GroupPoliciesId, "p").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                  .Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar").Equal()
                  .Property(APEntity.Policies.Properties.PoliciesId, "p").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                  .Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal()
                  .Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(UUEntiy.UniqueUsers), "uu"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                .Property(UUEntiy.UniqueUsers.Properties.UserId, "uu").Equal()
                .Property(APEntity.AuthorizationAnswer.Properties.UserAnswerId, "aa").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            where.Property(APEntity.AuthorizationAnswer.Properties.StatusId, "aa").Equal().Constant(1);
            where.And().Property(APEntity.AuthorizationAnswer.Properties.Enabled, "aa").Equal().Constant(1);
            where.And().Property(APEntity.Policies.Properties.GroupPoliciesId, "p").Equal().Constant(groupId);
            where.And().Property(APEntity.Policies.Properties.PoliciesId, "p").Equal().Constant(policiesId);

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade()
                .Select(select))
            {
                while (reader.Read())
                {
                    listUsers.Add(new User
                    {
                        UserId = (int)reader["UserId"],
                        AccountName = (string)reader["AccountName"]
                    });
                }
            }

            return listUsers;

        }
    }
}
