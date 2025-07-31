using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.UniqueUser.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.UniqueUserServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static List<CompanyUserBranch> CreateBranches(Core.Framework.DAF.BusinessCollection businessCollection, Core.Framework.DAF.BusinessCollection userBranchCollection)
        {
            List<CompanyUserBranch> branches = new List<CompanyUserBranch>();
            List<UserBranch> userbranches = new List<UserBranch>();
            if (userBranchCollection != null)
            {
                userbranches = userBranchCollection.Cast<UserBranch>().ToList();
            }
            foreach (COMMEN.Branch entity in businessCollection)
            {
                UserBranch userBranch = userbranches.Where(x => x.BranchCode == entity.BranchCode).FirstOrDefault();
                branches.Add(ModelAssembler.CreateBranch(entity, userBranch));
            }

            return branches;
        }

        public static CompanyUserBranch CreateBranch(COMMEN.Branch entityBranch, UUEN.UserBranch entityUser)
        {
            return new CompanyUserBranch
            {
                Id = entityBranch.BranchCode,
                Description = entityBranch.Description,
                IsDefault = entityUser != null ? (entityUser.DefaultBranch == 0 ? false : true) : false
            };
        }

        internal static List<CompanyUserGroup> CreateUserGroups(BusinessCollection usersGroups)
        {
            List<CompanyUserGroup> userGroups = new List<CompanyUserGroup>();
            foreach (UUEN.UserGroup entity in usersGroups)
            {
                userGroups.Add(new CompanyUserGroup
                {
                    UserId = entity.UserId,
                    GroupId = entity.GroupCode
                });
                //UserGroup userGroup = usersGroups.Where(x => x.UserId == entity.UserId).FirstOrDefault();
                //userGroups.Add(ModelAssembler.CreateGroup(entity, userGroup));
            }

            return userGroups;
        }

        private static CompanyUserGroup CreateGroup(UUEN.UniqueUsers entity, UserGroup userGroup)
        {
            return new CompanyUserGroup
            {
                UserId = entity.UserId,
                GroupId = userGroup.GroupCode
            };
        }
    }
}
