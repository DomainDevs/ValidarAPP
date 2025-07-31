using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Company.Application.UniqueUserBusinessService.Models;
using CompanyEntities = Sistran.Company.Application.UniqueUser.Entities;
using CoreEntities = Sistran.Core.Application.UniqueUser.Entities;

// -----------------------------------------------------------------------
// <copyright file="GroupDao.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserBusinessServiceProvider.Assemblers
{
    /// <summary>
    /// ModelAssembler. Assembler of BusinessCollection to Company model 
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Get list of Group.UniqueUsers
        /// </summary>
        /// <param name="businessCollection">Object list BusinessCollection.</param>
        /// <returns>list of Models.CompanyGroup</returns>
        public static List<CompanyGroup> MappParamGroups(BusinessCollection businessCollection)
        {
            List<CompanyGroup> listCompanyGroup = new List<CompanyGroup>();
                        
            foreach (CompanyEntities.Group fields in businessCollection)
            {
                listCompanyGroup.Add(MappParamGroup(fields));
            }
            
            return listCompanyGroup;
        }

        /// <summary>
        /// Creates group.
        /// </summary>
        /// <param name="Group">Object group.</param>
        /// <returns>Models.CompanyGroup</returns>
        public static CompanyGroup MappParamGroup(CompanyEntities.Group Group)
        {
            CompanyGroup companyGroup = new CompanyGroup
            {
                Id = Group.GroupCode,
                SmallDescription = Group.SmallDescription,
                Description = Group.Description,
                State = Group.Enabled
            };

            return companyGroup;
        }

        /// <summary>
        /// Get list of User Group.UniqueUsers
        /// </summary>
        /// <param name="businessCollection">Object list BusinessCollection.</param>
        /// <returns>list of Models.CompanyUserGroup</returns>
        public static List<CompanyUserGroup> MappParamUserGroups(BusinessCollection businessCollection)
        {
            List<CompanyUserGroup> listCompanyUserGroup = new List<CompanyUserGroup>();

            foreach (CoreEntities.UserGroup fields in businessCollection)
            {
                listCompanyUserGroup.Add(MappParamUserGroup(fields));
            }

            return listCompanyUserGroup;
        }

        /// <summary>
        /// Creates group.
        /// </summary>
        /// <param name="UserGroup">Object user group.</param>
        /// <returns>Models.CompanyUserGroup</returns>
        public static CompanyUserGroup MappParamUserGroup(CoreEntities.UserGroup UserGroup)
        {
            CompanyUserGroup companyUserGroup = new CompanyUserGroup
            {
                IdUser = UserGroup.UserId,
                IdGroup = UserGroup.GroupCode
            };

            return companyUserGroup;
        }

        /// <summary>
        /// Get list of CoreEntities.UserGroup
        /// </summary>
        /// <param name="companyUserGroups">Object list companyUserGroups.</param>
        /// <returns>list of CoreEntities.UserGroup</returns>
        public static List<CoreEntities.UserGroup> MappEntityUserGroups(List<CompanyUserGroup> companyUserGroups)
        {
            List<CoreEntities.UserGroup> listEntityUserGroup = new List<CoreEntities.UserGroup>();

            foreach (CompanyUserGroup fields in companyUserGroups)
            {
                listEntityUserGroup.Add(new CoreEntities.UserGroup(fields.IdUser, fields.IdGroup));
            }

            return listEntityUserGroup;
        }
    }
}
