using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using UUEntity = Sistran.Core.Application.UniqueUser.Entities;
using UUModel = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    using System.Collections.Concurrent;
    using Entities.Views;
    using Framework.Contexts;
    using Framework.DAF.Engine;
    using Framework.Transactions;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using System.Collections.Concurrent;
    using UniqueUserServices.Enums;
    using Utilities.Helper;

    public class ReasignDao
    {
        /// <summary>
        /// Reasigna una politica
        /// </summary>
        /// <param name="reasigns">reasignacion de que va a realizar</param>
        /// <returns></returns>
        public void ReasignAuthorizationAnswer(int policiesId, int userAnswerId, string key, int hierarchyId, int userReasignId, string reason, List<int> policiesToReassign, int userReassigning, TypeFunction functionType)
        {
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
                    where.And().Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "AutorizarionRequest").Equal().Constant(policiesId);
                    where.And().Property(APEntity.AuthorizationRequest.Properties.FunctionId, "AutorizarionRequest").Equal().Constant((int)functionType);
                    where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Equal().Constant((int)TypeStatus.Pending);
                    where.And().Property(APEntity.AuthorizationAnswer.Properties.StatusId, "AutorizarionAnswer").Equal().Constant((int)TypeStatus.Pending);
                    where.And().Property(APEntity.AuthorizationAnswer.Properties.UserAnswerId, "AutorizarionAnswer").Equal().Constant(userAnswerId);

                    if (policiesToReassign != null)
                    {
                        where.And().Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "AutorizarionAnswer").In();
                        where.ListValue();
                        policiesToReassign.ForEach(x => where.Constant(x));
                        where.EndList();
                    }

                    builder.Filter = where.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    //// se obtiene los valores de la VIEW
                    List<APEntity.AuthorizationAnswer> answers = view.AutorizarionAnswer.Cast<APEntity.AuthorizationAnswer>().ToList();
                    if (answers.Any())
                    {

                    //// Se insertan las reasignaciones
                    ConcurrentBag<APEntity.Reasign> reasigns = new ConcurrentBag<APEntity.Reasign>();

                    ParallelHelper.ForEach(answers, answer =>
                    {
                        APEntity.Reasign reasing = new APEntity.Reasign
                        {
                            AuthorizationAnswerId = answer.AuthorizationAnswerId,
                            UserAnswerId = answer.UserAnswerId,
                            HierarchyAnswerId = answer.HierarchyAnswerId,
                            UserReasignId = userReasignId,
                            HierarchyReasignId = hierarchyId,
                            DescriptionReasign = reason,
                            DateReasign = dateNow,
                            UserReAssigningId = userReassigning
                        };
                        reasigns.Add(reasing);


                    });
                    BusinessCollection businessCollection = new BusinessCollection();
                    businessCollection.AddRange(reasigns);
                    DataFacadeManager.Instance.GetDataFacade().InsertObjects(businessCollection);

                    //// Se realiza el update de los answers
                    UpdateQuery update = new UpdateQuery { Table = new ClassNameTable(typeof(APEntity.AuthorizationAnswer)) };
                    update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.UserAnswerId), new Constant(userReasignId, DbType.Int16));
                    update.ColumnValues.Add(new Column(APEntity.AuthorizationAnswer.Properties.HierarchyAnswerId), new Constant(hierarchyId, DbType.Int16));

                    where.Clear();
                    where.Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId).In();
                    where.ListValue();
                    answers.ToArray().ToList().ForEach(x => where.Constant(x.AuthorizationAnswerId));
                    where.EndList();

                    update.Where = where.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().Execute(update);

                    /*Se envia la notificacion*/
                    APEntity.AuthorizationRequest request = view.AutorizarionRequest.Cast<APEntity.AuthorizationRequest>().First();
                    //UniqueUser.Entities.UniqueUsers user = view.Users.Cast<UniqueUser.Entities.UniqueUsers>().First(x => x.UserId == userAnswerId);
                    User user = DelegateService.UniqueUserService.GetUserById(userReassigning);
                    APEntity.Policies policie = view.Policies.Cast<APEntity.Policies>().First(x => x.PoliciesId == policiesId);

                    TypeFunction function = (TypeFunction)Enum.Parse(typeof(TypeFunction), request.FunctionId.ToString());
                    UUModel.NotificationUser notificationUser = new UUModel.NotificationUser
                    {
                        UserId = userReasignId,
                        NotificationType = new UUModel.NotificationType { Type = NotificationTypes.AutorizationPolicies },
                        Parameters = new Dictionary<string, object> { { "key", request.Key }, { "IdPolicies", policiesId }, { "FunctionId", request.FunctionId } },
                    };

                    switch (function)
                    {
                        case TypeFunction.Individual:
                            notificationUser.Message = $"{user.AccountName} le ha reasignado la Politica - {policie.Description} (Temporal {request.Key})";
                            break;

                        case TypeFunction.Massive:
                        case TypeFunction.Collective:
                            notificationUser.Message = $"{user.AccountName} le ha reasignado la Politica - {policie.Description} (Cargue {request.Key})";
                            break;
                        case TypeFunction.Claim:
                        case TypeFunction.ClaimNotice:
                        case TypeFunction.PaymentRequest:
                        case TypeFunction.ChargeRequest:
                            notificationUser.Message = $"{user.AccountName} le ha reasignado la Politica - {policie.Description} (Temporal {request.Key})";
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
                        case TypeFunction.SarlaftGeneral:
                        case TypeFunction.PersonBasicInfo:
                            notificationUser.Message = $"{user.AccountName} le ha reasignado la Politica - {policie.Description} (Temporal {request.Key})";
                            break;
                    }
                    DelegateService.UniqueUserService.CreateNotification(notificationUser);
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
        /// Obtiene el historias de las autorizaciones reasignadas
        /// </summary>
        /// <param name="authorizationsAnswer">autorizaciones a consultar</param>
        /// <param name="userId">usuario de la consulta</param>
        /// <returns></returns>
        public List<Models.Reasign> GetHistoryReasign(int policiesId, int userAnswerId, string key)
        {
            ////  SE ARMA LA VIEW
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            AuthorizationPoliciesGetAuthorizations view = new AuthorizationPoliciesGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            filter.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(key);
            filter.And().Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "AutorizarionRequest").Equal().Constant(policiesId);

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            //// se obtiene los valores de la VIEW
            List<APEntity.AuthorizationAnswer> answers = view.AutorizarionAnswer.Cast<APEntity.AuthorizationAnswer>().ToList();

            ////
            List<int> idAutho = answers.Select(x => x.AuthorizationAnswerId).ToList();

            filter.Clear();
            filter.Property(APEntity.Reasign.Properties.UserAnswerId, typeof(APEntity.Reasign).Name).Equal().Constant(userAnswerId);
            filter.And().Property(APEntity.Reasign.Properties.AuthorizationAnswerId, typeof(APEntity.Reasign).Name).In().ListValue();
            answers.ForEach(x => filter.Constant(x.AuthorizationAnswerId));
            filter.EndList();


            APEntity.Reasign reasign = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                    .SelectObjects(typeof(APEntity.Reasign), filter.GetPredicate(), new[] { APEntity.Reasign.Properties.DateReasign }))
                .Cast<APEntity.Reasign>().OrderBy(x => x.DateReasign).First();


            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(APEntity.Reasign.Properties.UserAnswerId, "r")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Reasign.Properties.HierarchyAnswerId, "r")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Reasign.Properties.UserReasignId, "r")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Reasign.Properties.HierarchyReasignId, "r")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Reasign.Properties.DescriptionReasign, "r")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.Reasign.Properties.DateReasign, "r")));

            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.StatusId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.DescriptionAnswer, "aa")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.AuthorizationAnswer.Properties.DateAnswer, "aa")));

            select.AddSelectValue(new SelectValue(new Column(UUEntity.UniqueUsers.Properties.AccountName, "ua"), "AccountNameUA"));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.UniqueUsers.Properties.AccountName, "ur"), "AccountNameUR"));

            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.Description, "ha"), "HierarchyUA"));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.Description, "hr"), "HierarchyUR"));

            Join join = new Join(new ClassNameTable(typeof(APEntity.Reasign), "r"), new ClassNameTable(typeof(APEntity.AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.Reasign.Properties.AuthorizationAnswerId, "r").Equal().Property(APEntity.AuthorizationAnswer.Properties.AuthorizationAnswerId, "aa").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.AuthorizationRequest), "ar"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.AuthorizationRequest.Properties.AuthorizationRequestId, "ar").Equal().Property(APEntity.AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.Policies), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.AuthorizationRequest.Properties.PoliciesId, "ar").Equal().Property(APEntity.Policies.Properties.PoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal().Property(APEntity.Policies.Properties.GroupPoliciesId, "p").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.UniqueUsers), "ua"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.Reasign.Properties.UserAnswerId, "r").Equal().Property(UUEntity.UniqueUsers.Properties.UserId, "ua").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.UniqueUsers), "ur"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.Reasign.Properties.UserReasignId, "r").Equal().Property(UUEntity.UniqueUsers.Properties.UserId, "ur").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.CoHierarchyAssociation), "ha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").Equal().Property(UUEntity.CoHierarchyAssociation.Properties.ModuleCode, "ha").And()

                    .Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").Equal().Property(UUEntity.CoHierarchyAssociation.Properties.SubmoduleCode, "ha").And()

                    .Property(APEntity.Reasign.Properties.HierarchyAnswerId, "r").Equal().Property(UUEntity.CoHierarchyAssociation.Properties.HierarchyCode, "ha").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(UUEntity.CoHierarchyAssociation), "hr"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").Equal().Property(UUEntity.CoHierarchyAssociation.Properties.ModuleCode, "hr").And()

                    .Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").Equal().Property(UUEntity.CoHierarchyAssociation.Properties.SubmoduleCode, "hr").And()

                    .Property(APEntity.Reasign.Properties.HierarchyAnswerId, "r").Equal().Property(UUEntity.CoHierarchyAssociation.Properties.HierarchyCode, "hr").GetPredicate()
            };


            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.Reasign.Properties.AuthorizationAnswerId, "r").In().ListValue();
            idAutho.ForEach(x => where.Constant(x));
            where.EndList();
            where.And().Property(APEntity.Reasign.Properties.DateReasign, "r").GreaterEqual().Constant(reasign.DateReasign);

            select.Distinct = true;
            select.AddSortValue(new SortValue(new Column(APEntity.Reasign.Properties.DateReasign, "r"), SortOrderType.Ascending));

            select.Table = join;
            select.Where = where.GetPredicate();

            List<Models.Reasign> listReasign = new List<Models.Reasign>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    listReasign.Add(new Models.Reasign
                    {
                        AuthorizationAnswer = new Models.AuthorizationAnswer { Status = (TypeStatus)(int)reader["StatusId"], DescriptionAnswer = (string)reader["DescriptionAnswer"], DateAnswer = (DateTime?)reader["DateAnswer"] },
                        DescriptionReasign = (string)reader["DescriptionReasign"],
                        DateReasign = (DateTime)reader["DateReasign"],
                        UserAnswer = new UUModel.User { UserId = (int)reader["UserAnswerId"], AccountName = (string)reader["AccountNameUA"] },
                        UserReasign = new UUModel.User { UserId = (int)reader["UserReasignId"], AccountName = (string)reader["AccountNameUR"] },
                        HierarchyAnswer = new UUModel.CoHierarchyAssociation { Id = (int)reader["HierarchyAnswerId"], Description = (string)reader["HierarchyUA"] },
                        HierarchyReasign = new UUModel.CoHierarchyAssociation { Id = (int)reader["HierarchyReasignId"], Description = (string)reader["HierarchyUR"] },
                    });
                }
            }

            return listReasign;
        }

        public List<CoHierarchyAssociation> GetHierarchyByGroupPolicies(int groupId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.HierarchyCode, "ha")));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.Description, "ha")));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.ModuleCode, "ha")));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.SubmoduleCode, "ha")));

            Join join = new Join(new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), new ClassNameTable(typeof(UUEntity.CoHierarchyAssociation), "ha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(APEntity.GroupPolicies.Properties.ModuleId, "gp").Equal()
                    .Property(UUEntity.CoHierarchyAssociation.Properties.ModuleCode, "ha")

                    .And().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp").Equal()
                    .Property(UUEntity.CoHierarchyAssociation.Properties.SubmoduleCode, "ha").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            where.Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").Equal().Constant(groupId);
            
            select.Table = join;
            select.Where = where.GetPredicate();

            List<CoHierarchyAssociation> hierarchyList = new List<CoHierarchyAssociation>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    hierarchyList.Add(new CoHierarchyAssociation
                    {
                        Id = (int)reader["HierarchyCode"],
                        Description = (string)reader["Description"]
                    });
                }
            }

            return hierarchyList;
        }
        
    }
}
