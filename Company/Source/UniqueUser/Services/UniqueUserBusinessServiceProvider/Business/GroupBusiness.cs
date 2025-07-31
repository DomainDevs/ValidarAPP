using System;
using System.Collections.Generic;
using Sistran.Company.Application.UniqueUserBusinessService.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UniqueUserBusinessServiceProvider.DAOs;

// -----------------------------------------------------------------------
// <copyright file="GroupBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserBusinessServiceProvider.Business
{
    /// <summary>
    /// Make the call to the corresponding DAOs for the functionality of groups user module.
    /// </summary>
    class GroupBusiness
    {
        /// <summary>
        /// Calling DAO to consult the full list of groups
        /// </summary>
        /// <returns>List<CompanyGroup>. Object company Group</returns>
        public List<CompanyGroup> GetBusinessGroup()
        {
            try
            {
                GroupDao groupDao = new GroupDao();
                return groupDao.GetAllGroup();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calling DAO to consult the full list of user Vs groups
        /// </summary>
        /// <returns>List<CompanyUserGroup>. Object company User Group</returns>
        public List<CompanyUserGroup> GetBusinessUserGroup(int userId)
        {
            try
            {
                GroupDao userGroupDao = new GroupDao();
                return userGroupDao.GetAllUserGroup(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calling DAO to save the full list of user Vs groups
        /// </summary>
        /// <returns>CompanyUserGroup. Object company User Group</returns>
        public CompanyUserGroup SaveBusinessUserGroup(List<CompanyUserGroup> companyUserGroups)
        {
            try
            {
                GroupDao userGroupDao = new GroupDao();

                if (companyUserGroups.Count > 0)
                {
                    userGroupDao.DeleteAllUserGroup(companyUserGroups[0].IdUser);
                    userGroupDao.SaveAllUserGroup(companyUserGroups);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
