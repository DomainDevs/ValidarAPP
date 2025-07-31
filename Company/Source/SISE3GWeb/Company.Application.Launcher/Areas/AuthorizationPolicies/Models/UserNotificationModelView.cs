using System;
using System.Collections.Generic;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class UserNotificationModelView
    {
        public int IdPolicies { set; get; }

        public int IdUser { set; get; }

        public string UserName { set; get; }

        public int IdHierarchy { set; get; }

        public bool Default { set; get; }

        public DateTime? DisabledDate { set; get; }


        #region Assemblers

        public static List<UserNotificationModelView> GetListModelView(List<UserNotification> usersNotification)
        {
            var list = new List<UserNotificationModelView>();

            foreach (var user in usersNotification)
            {
                list.Add(GetModelView(user));
            }
            return list;
        }

        public static UserNotificationModelView GetModelView(UserNotification userNotification)
        {
            return new UserNotificationModelView
            {
                IdPolicies = userNotification.Policies.IdPolicies,
                IdUser = userNotification.User.UserId,
                UserName = userNotification.User.AccountName,
                IdHierarchy = userNotification.Hierarchy.Id,
                Default = userNotification.Default,
                DisabledDate = userNotification.User.DisableDate ?? null
            };
        }

        #endregion
    }
}