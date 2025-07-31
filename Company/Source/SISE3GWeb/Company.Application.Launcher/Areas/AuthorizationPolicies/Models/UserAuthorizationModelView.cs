using System;
using System.Collections.Generic;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class UserAuthorizationModelView
    {
        public int IdPolicies { set; get; }

        public int IdUser { set; get; }

        public string UserName { set; get; }

        public int IdHierarchy { set; get; }

        public bool Default { set; get; }

        public bool Required { set; get; }

        public DateTime? DisabledDate { set; get; }


        #region Assemblers
        public static List<UserAuthorizationModelView> GetListModelView(List<UserAuthorization> usersAuthorization)
        {
            var list = new List<UserAuthorizationModelView>();

            foreach (var user in usersAuthorization)
            {
                list.Add(GetModelView(user));
            }
            return list;
        }

        public static UserAuthorizationModelView GetModelView(UserAuthorization userAuthorization)
        {
            return new UserAuthorizationModelView
            {
                IdPolicies = userAuthorization.Policies.IdPolicies,
                IdUser = userAuthorization.User.UserId,
                UserName = userAuthorization.User.AccountName,
                IdHierarchy = userAuthorization.Hierarchy.Id,
                Default = userAuthorization.Default,
                Required = userAuthorization.Required,
                DisabledDate = userAuthorization.User.DisableDate ?? null
            };
        }
        #endregion
    }
}