using Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using UUEntity = Sistran.Core.Application.UniqueUser.Entities;
using UUModel = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    using System.Data;
    using Models;
    using TypePolicies = Enums.TypePolicies;

    public class UserNotificationDao
    {
        /// <summary>
        /// Obtiene la lista de usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns>lista de usuarios notificadores</returns>
        public List<Models.UserNotification> GetUsersNotificationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, List<Models.UserGroupModel> usersGroup)
        {
            SelectQuery select = new SelectQuery();
            select.Distinct = true;
            select.AddSelectValue(new SelectValue(new Column(UUEntity.UniqueUsers.Properties.UserId, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.UniqueUsers.Properties.AccountName, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.HierarchyCode, "cha2")));
            select.AddSelectValue(new SelectValue(new Column(UUEntity.CoHierarchyAssociation.Properties.Description, "cha2")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.UserNotification.Properties.Default, "un")));


            Join join = new Join(new ClassNameTable(typeof(APEntity.UserNotification), "un"), new ClassNameTable(typeof(APEntity.Policies), "p"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.Policies.Properties.PoliciesId, "p")
                    .Equal().Property(APEntity.UserNotification.Properties.PoliciesId, "un").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(APEntity.GroupPolicies), "gp"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.Policies.Properties.GroupPoliciesId, "p")
                    .Equal().Property(APEntity.GroupPolicies.Properties.GroupPoliciesId, "gp").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(UUEntity.CoHierarchyAccesses), "cha"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.GroupPolicies.Properties.ModuleId, "gp")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.ModuleCode, "cha")

                    .And().Property(APEntity.GroupPolicies.Properties.SubmoduleId, "gp")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.SubmoduleCode, "cha")

                    .And().Property(APEntity.UserNotification.Properties.HierarchyId, "un")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.HierarchyCode, "cha")

                    .And().Property(APEntity.UserNotification.Properties.UserId, "un")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.UserId, "cha")

                    .GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(UUEntity.CoHierarchyAssociation), "cha2"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UUEntity.CoHierarchyAccesses.Properties.ModuleCode, "cha")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.ModuleCode, "cha2")

                    .And().Property(UUEntity.CoHierarchyAccesses.Properties.SubmoduleCode, "cha")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.SubmoduleCode, "cha2")

                    .And().Property(UUEntity.CoHierarchyAccesses.Properties.HierarchyCode, "cha")
                    .Equal().Property(UUEntity.CoHierarchyAccesses.Properties.HierarchyCode, "cha2")

                    .GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(UUEntity.UniqueUsers), "uu"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.UserNotification.Properties.UserId, "un")
                    .Equal().Property(UUEntity.UniqueUsers.Properties.UserId, "uu").GetPredicate()
            };

            if (usersGroup != null && usersGroup.Any())
            {
                join = new Join(join, new ClassNameTable(typeof(UUEntity.UserGroup), "ug"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder().Property(UUEntity.UniqueUsers.Properties.UserId, "uu")
                    .Equal().Property(UUEntity.UserGroup.Properties.UserId, "ug").GetPredicate()
                };
            }


            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(APEntity.UserAuthorization.Properties.PoliciesId, "p").Equal().Constant(idPolicies);
            filter.And().Property(UUEntity.UniqueUsers.Properties.DisabledDate, "uu").IsNull();
            if (idHierarchy.HasValue)
            {
                filter.And().Property(APEntity.UserNotification.Properties.HierarchyId, "un").Equal().Constant(idHierarchy);
            }
            if (usersGroup != null && usersGroup.Any())
            {
                filter.And().Property(UUEntity.UserGroup.Properties.GroupCode, "ug").In();
                filter.ListValue();
                usersGroup.ForEach(x => filter.Constant(x.GroupId));
                filter.EndList();
            }


            select.Table = join;
            select.Where = filter.GetPredicate();
            List<UserNotification> result = new List<Models.UserNotification>();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    UserNotification userAuthorization = new UserNotification
                    {
                        Policies = new PoliciesAut { IdPolicies = idPolicies, Type = TypePolicies.Notification },
                        User = new UUModel.User { UserId = int.Parse(reader["UserId"].ToString()), AccountName = reader["AccountName"].ToString() },
                        Hierarchy = new UUModel.CoHierarchyAssociation { Id = int.Parse(reader["HierarchyCode"].ToString()), Description = reader["Description"].ToString() },
                        Default = (bool)reader["Default"]
                    };

                    result.Add(userAuthorization);
                }
            }
            return result;
        }

        /// <summary>
        /// Crea los usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <returns></returns>
        public void CreateUsersNotification(int idPolicies, List<Models.UserNotification> users)
        {
            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter;
                    var businessCollection = new BusinessCollection<APEntity.UserNotification>();
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(APEntity.UserNotification.Properties.PoliciesId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(APEntity.UserNotification), filter.GetPredicate());

                    if (users != null)
                    {
                        foreach (UserNotification user in users)
                        {
                            APEntity.UserNotification entityUser = EntityAssembler.CreateUserNotification(user);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityUser);
                        }
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
    }
}
