using System;
using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UniqueUserBusinessService;
using Sistran.Company.Application.UniqueUserBusinessService.Models;
using Sistran.Company.Application.UniqueUserBusinessServiceProvider.Business;


namespace Sistran.Company.Application.UniqueUserBusinessServiceProvider
{
    /// <summary>
    /// Class that implement the interface "IUniqueUserBusinessService"
    /// </summary>
    public class UniqueUserBusinessServiceProvider : IUniqueUserBusinessService
    {
        /// <summary>
        /// GetBusinessGroup: Called to Business to consult list of groups - table UU.[GROUP]
        /// </summary>
        /// <returns>List<CompanyGroup>. Object company Group</returns>
        public List<CompanyGroup> GetBusinessGroup()
        {
            try
            {
                GroupBusiness group = new GroupBusiness();
                return group.GetBusinessGroup();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetBusinessUserGroup: Called to Business to consult list of groups - table UU.USER_GROUP
        /// </summary>
        /// <returns>List<CompanyUserGroup>. Object company User Group</returns>
        public List<CompanyUserGroup> GetBusinessUserGroup(int userId)
        {
            try
            {
                GroupBusiness userGroup = new GroupBusiness();
                return userGroup.GetBusinessUserGroup(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveBusinessUserGroup: Save the complete list of user groups - table UU.USER_GROUP
        /// </summary>
        /// <returns>CompanyUserGroup. Object company User Group</returns>
        public CompanyUserGroup SaveBusinessUserGroup(List<CompanyUserGroup> companyUserGroups)
        {
            try
            {
                GroupBusiness userGroup = new GroupBusiness();
                return userGroup.SaveBusinessUserGroup(companyUserGroups);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
