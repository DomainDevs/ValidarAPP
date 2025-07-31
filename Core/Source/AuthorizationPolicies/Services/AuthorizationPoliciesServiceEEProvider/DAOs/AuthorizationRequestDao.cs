using Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using UUModel = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    using Co.Application.Data;
    using Entities.Views;
    using Framework.DAF.Engine;
    using Models;
    using Parameters.Entities;
    using TypePolicies = Enums.TypePolicies;
    using System.Collections.Concurrent;
    using System.Linq;
    using UniqueUser.Entities;
    using Utilities.Helper;   
    using System.Linq;

    public class AuthorizationRequestDao
    {
        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion 
        /// </summary>
        /// <param name="authorization">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        public Models.AuthorizationRequest CreateAuthorizationRequest(Models.AuthorizationRequest authorization)
        {
            APEntity.AuthorizationRequest entity = EntityAssembler.CreateAuthorizationRequest(authorization);

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);

            authorization.AuthorizationRequestId = entity.AuthorizationRequestId;
            return authorization;
        }

        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <returns></returns>
        public List<Models.AuthorizationRequest> GetAuthorizationRequestsByKey(string key)
        {
            ////  SE ARMA LA VIEW
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            AuthorizationPoliciesGetAuthorizations view = new AuthorizationPoliciesGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(key);

            builder.Filter = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            //// se obtiene los valores de la VIEW
            List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.AsParallel().Cast<APEntity.AuthorizationRequest>().ToList();
            List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.AsParallel().Cast<APEntity.AuthorizationAnswer>().ToList();
            List<APEntity.Policies> totalPolicies = view.Policies.AsParallel().Cast<APEntity.Policies>().ToList();
            List<UniqueUsers> totalUsers = view.Users.AsParallel().Cast<UniqueUsers>().ToList();

            ////
            ConcurrentBag<Models.AuthorizationRequest> requests = new ConcurrentBag<Models.AuthorizationRequest>();
            ParallelHelper.ForEach(totalRequests, request =>
            {
                Models.AuthorizationRequest authorizationRequest = ModelAssembler.CreateAuthorizationRequest(request);
                authorizationRequest.AuthorizationAnswers = totalAnswers.AsParallel().Where(x => x.AuthorizationRequestId == request.AuthorizationRequestId)
                .Select(x =>
                {
                    Models.AuthorizationAnswer answer = ModelAssembler.CreateAuthorizationAnswer(x);
                    UniqueUsers user = totalUsers.AsParallel().First(u => u.UserId == x.UserAnswerId);
                    answer.UserAnswer.AccountName = user.AccountName;
                    return answer;
                }).ToList();

                authorizationRequest.Policies = ModelAssembler.CreatePolicies(totalPolicies.First(x => x.PoliciesId == request.PoliciesId));
                requests.Add(authorizationRequest);
            });

            return requests.ToList();
        }

        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <param name="status">estado de la solicitud de autorizacion</param>
        /// <returns></returns>
        public List<Models.AuthorizationRequest> GetAuthorizationRequestsByKeyStatus(string key, int status)
        {
            ////  SE ARMA LA VIEW
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            AuthorizationPoliciesGetAuthorizations view = new AuthorizationPoliciesGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(key);
            where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Equal().Constant(status);


            builder.Filter = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            //// se obtiene los valores de la VIEW
            List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.Cast<APEntity.AuthorizationRequest>().ToList();
            List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.Cast<APEntity.AuthorizationAnswer>().ToList();
            List<APEntity.Policies> totalPolicies = view.Policies.Cast<APEntity.Policies>().ToList();
            List<UniqueUsers> totalUsers = view.Users.Cast<UniqueUsers>().ToList();

            ConcurrentBag<Models.AuthorizationRequest> requests = new ConcurrentBag<Models.AuthorizationRequest>();
            ParallelHelper.ForEach(totalRequests, request =>
            {
                Models.AuthorizationRequest authorizationRequest = ModelAssembler.CreateAuthorizationRequest(request);
                authorizationRequest.AuthorizationAnswers = totalAnswers.Where(x => x.AuthorizationRequestId == request.AuthorizationRequestId)
                    .Select(x =>
                    {
                        Models.AuthorizationAnswer answer = ModelAssembler.CreateAuthorizationAnswer(x);
                        UniqueUsers user = totalUsers.First(u => u.UserId == x.UserAnswerId);
                        answer.UserAnswer.AccountName = user.AccountName;
                        return answer;
                    }).ToList();

                authorizationRequest.Policies = ModelAssembler.CreatePolicies(totalPolicies.First(x => x.PoliciesId == request.PoliciesId));
                requests.Add(authorizationRequest);
            });

            return requests.ToList();

        }

        public Models.AuthorizationRequest GetAuthorizationRequestByIdAuthorizationAnswer(int answerAuthorizationAnswerId)
        {

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.Key, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.Key2, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.Description, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.StatusId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.UserRequestId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.HierarchyRequestId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.DescriptionRequest, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.DateRequest, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.FunctionId, "ar")));


            Join join = new Join(new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").Equal()
                    .Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId, "aa").Equal().Constant(answerAuthorizationAnswerId);

            select.Table = join;
            select.Where = where.GetPredicate();


            Models.AuthorizationRequest result = new Models.AuthorizationRequest();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    result.AuthorizationRequestId = (int)reader["AuthorizationRequestId"];
                    result.Policies = new Models.PoliciesAut
                    {
                        IdPolicies = (int)reader["PoliciesId"],
                        Type = Enums.TypePolicies.Authorization
                    };
                    result.Key = (string)reader["Key"];
                    result.Key2 = (string)reader["Key2"];
                    result.Description = (string)reader["Description"];
                    result.Status = (TypeStatus)(int)reader["StatusId"];
                    result.DescriptionRequest = (string)reader["DescriptionRequest"];
                    result.DateRequest = (DateTime)reader["DateRequest"];
                    result.UserRequest = new UUModel.User { UserId = (int)reader["UserRequestId"] };
                    result.HierarchyRequest = new UUModel.CoHierarchyAssociation { Id = (int)reader["HierarchyRequestId"] };
                    result.FunctionType = (TypeFunction)(int)reader["FunctionId"];
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <returns></returns>
        public List<Models.AuthorizationRequest> GetAuthorizationRequestsByKeyKey2(string key, string key2)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.AuthorizationRequest.Properties.Key, typeof(APEntity.AuthorizationRequest).Name)
                .Equal().Constant(key);
            filter.And();
            filter.Property(APEntity.AuthorizationRequest.Properties.Key2, typeof(APEntity.AuthorizationRequest).Name)
    .Equal().Constant(key2);

            BusinessCollection<APEntity.AuthorizationRequest> businessCollection = new BusinessCollection<APEntity.AuthorizationRequest>(
                   DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(APEntity.AuthorizationRequest), filter.GetPredicate()));

            return ModelAssembler.CreateListAuthorizationRequest(businessCollection
                .Cast<APEntity.AuthorizationRequest>().ToList());
        }

        /// <summary>
        /// Actualiza el Identificador del proceso, en los eventos asociados
        /// </summary>
        /// <param name="key">llave de identificacion 1</param>
        /// <param name="key2">llave de identificacion 2</param>
        /// <param name="processId">identificador del proceso</param>
        public void UpdateProcessIdByKeyKey2(Enums.TypeFunction typeFunction, string key, string key2, string processId)
        {
            UpdateQuery update = new UpdateQuery
            {
                Table = new ClassNameTable(typeof(APEntity.AuthorizationRequest))
            };

            update.ColumnValues.Add(new Column(APEntity.AuthorizationRequest.Properties.ProcessId), new Constant(processId, DbType.String));
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(APEntity.AuthorizationRequest.Properties.Key);
            filter.Equal();
            filter.Constant(key);
            filter.And();
            filter.Property(APEntity.AuthorizationRequest.Properties.FunctionId);
            filter.Equal();
            filter.Constant((int)typeFunction);
            filter.And();
            if (!string.IsNullOrEmpty(key2))
            {
                filter.Property(APEntity.AuthorizationRequest.Properties.Key2);
                filter.Equal();
                filter.Constant(key2);
                filter.And();
            }
            filter.Property(APEntity.AuthorizationRequest.Properties.StatusId);
            filter.Equal();
            filter.Constant((int)TypeStatus.Authorized);

            update.Where = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(update);
        }

        public List<Models.IssueWithPolicies> GetIssueWithPolicies(int? temporalId, int userId)
        {
            temporalId = temporalId ?? -1;

            List<Models.IssueWithPolicies> issueWithPolicieses = new List<Models.IssueWithPolicies>();
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@OPERATION_ID", temporalId);
            parameters[1] = new NameValue("@USER_ID", userId);

            DataSet result = new DataSet();
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataSet("AUTHO.GET_ISSUE_WITH_POLICIES", parameters);
            }

            if (result.Tables.Count > 0)
            {
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    Models.IssueWithPolicies issueWithPolicies = new Models.IssueWithPolicies
                    {
                        TemporalId = int.Parse(row["N° TEMPORAL"].ToString()),
                        EndorsementType = row["TIPO DE ENDOSO"].ToString(),
                        DocumentNumber = row["N° POLIZA"].ToString(),
                        Branch = row["SUCURSAL"].ToString(),
                        Prefix = row["RAMO"].ToString(),
                        DateRequest = DateTime.Parse(row["FECHA_SOLICITUD"].ToString())
                    };

                    issueWithPolicieses.Add(issueWithPolicies);
                }
            }

            return issueWithPolicieses;
        }
		
		   /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion para masivos
        /// </summary>
        /// <param name="authorizationRequests">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        public void CreateMassiveAutorizationRequest(List<AuthorizationRequest> authorizationRequests)
        {
            NameValue[] parameters = new NameValue[2];

            //AUTHO_PARAM_REQUEST
            DataTable tableRequest = new DataTable("AUTHO_PARAM_REQUEST");
            tableRequest.Columns.Add("ID", typeof(int));
            tableRequest.Columns.Add("POLICIES_ID", typeof(int));
            tableRequest.Columns.Add("KEY", typeof(string));
            tableRequest.Columns.Add("KEY2", typeof(string));
            tableRequest.Columns.Add("DESCRIPTION", typeof(string));
            tableRequest.Columns.Add("NUMBER_AUT", typeof(int));
            tableRequest.Columns.Add("USER_REQUEST_ID", typeof(int));
            tableRequest.Columns.Add("HIERARCHY_REQUEST_ID", typeof(int));
            tableRequest.Columns.Add("DESCRIPTION_REQUEST", typeof(string));
            tableRequest.Columns.Add("FUNCTION_ID", typeof(int));

            //AUTHO_PARAM_ANSWER
            DataTable tableAnswer = new DataTable("AUTHO_PARAM_ANSWER");
            tableAnswer.Columns.Add("ID", typeof(int));
            tableAnswer.Columns.Add("USER_ANSWER_ID", typeof(int));
            tableAnswer.Columns.Add("HIERARCHY_ANSWER_ID", typeof(int));
            tableAnswer.Columns.Add("REQUIRED", typeof(bool));
            tableAnswer.Columns.Add("ENABLED", typeof(bool));


            for (int i = 0; i < authorizationRequests.Count; i++)
            {
                DataRow rowRequest = tableRequest.NewRow();
                rowRequest["ID"] = i;
                rowRequest["POLICIES_ID"] = authorizationRequests[i].Policies.IdPolicies;
                rowRequest["KEY"] = authorizationRequests[i].Key;
                rowRequest["KEY2"] = authorizationRequests[i].Key2;
                rowRequest["DESCRIPTION"] = authorizationRequests[i].Description;
                rowRequest["NUMBER_AUT"] = authorizationRequests[i].NumberAut;
                rowRequest["USER_REQUEST_ID"] = authorizationRequests[i].UserRequest.UserId;
                rowRequest["HIERARCHY_REQUEST_ID"] = authorizationRequests[i].HierarchyRequest.Id;
                rowRequest["DESCRIPTION_REQUEST"] = authorizationRequests[i].DescriptionRequest;
                rowRequest["FUNCTION_ID"] = (int)authorizationRequests[i].FunctionType;
                tableRequest.Rows.Add(rowRequest);

                DataRow rowAnswer = tableAnswer.NewRow();
                rowAnswer["ID"] = i;
                rowAnswer["USER_ANSWER_ID"] = authorizationRequests[i].AuthorizationAnswers[0].UserAnswer.UserId;
                rowAnswer["HIERARCHY_ANSWER_ID"] = authorizationRequests[i].AuthorizationAnswers[0].HierarchyAnswer.Id;
                rowAnswer["REQUIRED"] = authorizationRequests[i].AuthorizationAnswers[0].Required;
                rowAnswer["ENABLED"] = authorizationRequests[i].AuthorizationAnswers[0].Enabled;
                tableAnswer.Rows.Add(rowAnswer);
            }


            parameters[0] = new NameValue(tableRequest.TableName, tableRequest);
            parameters[1] = new NameValue(tableAnswer.TableName, tableAnswer);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteNonQuery("AUTHO.INSERT_AUTORIZATION_REQUESTS", parameters);
            }
        }
        /// <summary>
        /// Consulta las solicitudes de autorizacion por el usuario que solicita, fecha inicio/fin y los estados
        /// </summary>
        /// <param name="idUser">id usuario que solicita</param>
        /// <param name="status">lista de estados</param>
        /// <param name="dateInit">fecha inicial</param>
        /// <param name="dateEnd">fecha final</param>
        /// <returns></returns>
        public List<Models.AuthorizationRequestGroup> GetAuthorizationRequestGroups(int idUser, List<int> status, DateTime dateInit, DateTime dateEnd)
        {
            List<Models.AuthorizationRequestGroup> authorizationRequestGroups = new List<Models.AuthorizationRequestGroup>();

            NameValue[] parameters = new NameValue[6];
            parameters[0] = new NameValue("@USER_ID", idUser);
            parameters[1] = new NameValue("@DATE_INIT", dateInit);
            parameters[2] = new NameValue("@DATE_END", dateEnd);
            parameters[3] = new NameValue("@AUTORIZED", (int)(status.Contains((int)TypeStatus.Authorized) ? TypeStatus.Authorized : 0));
            parameters[4] = new NameValue("@REJECTED", (int)(status.Contains((int)TypeStatus.Rejected) ? TypeStatus.Rejected : 0));
            parameters[5] = new NameValue("@PENDING", (int)(status.Contains((int)TypeStatus.Pending) ? TypeStatus.Pending : 0));

            DataSet result = new DataSet();
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataSet("AUTHO.GET_AUTHORIZATION_REQUEST_GROUP", parameters);
            }

            if (result.Tables.Count > 0)
            {
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    Models.AuthorizationRequestGroup requestGroup = new Models.AuthorizationRequestGroup
                    {
                        GroupPoliciesId = int.Parse(row["GroupPoliciesId"].ToString()),
                        DescriptionGroup = row["DescriptionGroup"].ToString(),
                        PoliciesId = int.Parse(row["PoliciesId"].ToString()),
                        DescriptionPolicie = row["DescriptionPolicie"].ToString(),
                        Reference = row["Reference"].ToString(),
                        DateRequest = DateTime.Parse(row["DateRequest"].ToString()),
                        DescriptionRequest = row["DescriptionRequest"].ToString(),
                        Count = int.Parse(row["Count"].ToString()),
                        Status = (TypeStatus)Enum.Parse(typeof(TypeStatus), row["StatusId"].ToString()),
                        StatusDescription = row["Status"].ToString(),
                        ProcessDescription = row["ProcessDescription"].ToString(),
                        FunctionType = (TypeFunction)Enum.Parse(typeof(TypeFunction), row["FunctionType"].ToString()),
                        Key = row["Key"].ToString()
                    };

                    authorizationRequestGroups.Add(requestGroup);
                }
            }

            return authorizationRequestGroups;
        }

        public List<Models.AuthorizationRequestGroup> GetDetailsAuthorizationRequestGroups(int idUser, string key, int policiesId)
        {
            List<Models.AuthorizationRequestGroup> requestGroups = new List<Models.AuthorizationRequestGroup>();

            SelectQuery select = new SelectQuery();
            Function count = new Function(FunctionType.Count);
            count.AddParameter(new Column(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar"));

            select.AddSelectValue(new SelectValue(count, "Count"));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.Description, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.StatusId, "ar")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationRequest.Properties.FunctionId, "ar")));

            Join join = new Join(new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar")
                    .Equal().Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").GetPredicate()
            };

            select.Table = join;

            select.AddGroupValue(new Column(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar"));
            select.AddGroupValue(new Column(APEntity.AuthorizationRequest.Properties.Description, "ar"));
            select.AddGroupValue(new Column(APEntity.AuthorizationRequest.Properties.StatusId, "ar"));
            select.AddGroupValue(new Column(APEntity.AuthorizationRequest.Properties.FunctionId, "ar"));

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(APEntity.AuthorizationRequest.Properties.UserRequestId, "ar").Equal().Constant(idUser);
            filter.And().Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar").Equal().Constant(policiesId);
            filter.And().Property(APEntity.AuthorizationRequest.Properties.Key, "ar").Equal().Constant(key);

            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    Models.AuthorizationRequestGroup requestGroup = new Models.AuthorizationRequestGroup
                    {
                        AuthorizationRequestId = int.Parse(reader["AuthorizationRequestId"].ToString()),
                        PoliciesId = policiesId,
                        DescriptionRequest = reader["Description"].ToString(),
                        Count = int.Parse(reader["Count"].ToString()),
                        Status = (TypeStatus)Enum.Parse(typeof(TypeStatus), reader["StatusId"].ToString()),
                        FunctionType = (TypeFunction)Enum.Parse(typeof(TypeFunction), reader["FunctionId"].ToString()),
                        Key = key
                    };

                    requestGroups.Add(requestGroup);
                }
            }

            return requestGroups;
        }

        /// <summary>
        /// Consulta las solicitudes pendientes de autorizacion por el usuario que solicita, fecha inicio/fin y los estados
        /// </summary>
        /// <param name="idUser">id usuario que solicita</param>
        /// <param name="status">lista de estados</param>
        /// <param name="dateInit">fecha inicial</param>
        /// <param name="dateEnd">fecha final</param>
        /// <returns></returns>
        public List<Models.AuthorizationRequestGroup> GetAuthorizationRequestPendingGroups(int groupPolicies, int policies, int idUser, int userAuthorization, DateTime dateInit, DateTime dateEnd)
        {
            int status = 1;
            List<Models.AuthorizationRequestGroup> authorizationRequestGroups = new List<Models.AuthorizationRequestGroup>();


            NameValue[] parameters = new NameValue[7];
            parameters[0] = new NameValue("@USER_ID", idUser);
            parameters[1] = new NameValue("@DATE_INIT", dateInit);
            parameters[2] = new NameValue("@DATE_END", dateEnd);
            parameters[3] = new NameValue("@PENDING", (status == (int)TypeStatus.Pending ? (int)TypeStatus.Pending : 0));
            parameters[4] = new NameValue("@GROUP_POLICIES", groupPolicies);
            parameters[5] = new NameValue("@POLICIES", policies);
            parameters[6] = new NameValue("@USER_AUTHORIZATION", userAuthorization);


            DataSet result = new DataSet();
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataSet("AUTHO.GET_AUTHORIZATION_REQUEST_PENDING_GROUP", parameters);
            }

            if (result.Tables.Count > 0)
            {
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    Models.AuthorizationRequestGroup requestGroup = new Models.AuthorizationRequestGroup
                    {
                        GroupPoliciesId = int.Parse(row["GroupPoliciesId"].ToString()),
                        DescriptionGroup = row["DescriptionGroup"].ToString(),
                        PoliciesId = int.Parse(row["PoliciesId"].ToString()),
                        DescriptionPolicie = row["DescriptionPolicie"].ToString(),
                        Reference = row["Reference"].ToString(),
                        DateRequest = DateTime.Parse(row["DateRequest"].ToString()),
                        DescriptionRequest = row["DescriptionRequest"].ToString(),
                        Count = int.Parse(row["Count"].ToString()),
                        Status = (TypeStatus)Enum.Parse(typeof(TypeStatus), row["StatusId"].ToString()),
                        StatusDescription = row["Status"].ToString(),
                        ProcessDescription = row["ProcessDescription"].ToString(),
                        FunctionType = (TypeFunction)Enum.Parse(typeof(TypeFunction), row["FunctionType"].ToString()),
                        Key = row["Key"].ToString()
                    };

                    authorizationRequestGroups.Add(requestGroup);
                }
            }

            return authorizationRequestGroups;
        }
    }
}
