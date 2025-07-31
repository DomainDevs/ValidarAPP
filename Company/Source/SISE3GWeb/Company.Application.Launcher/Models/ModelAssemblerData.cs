using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sistran.Company.Application.UniqueUserServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class ModelAssemblerData
    {
        public static UserGroupModel CreateUserGroup(CompanyUserGroup userGroupModel)
        {
            return new UserGroupModel
            {
                UserId = userGroupModel.UserId,
                GroupId = userGroupModel.GroupId
            };
        }
        public static List<UserGroupModel> CreateUserGroups(List<CompanyUserGroup> userGroupModel)
        {
            var result = new List<UserGroupModel>();
            try
            {
                userGroupModel.ForEach(x => result.Add(CreateUserGroup(x)));
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }
    }
}