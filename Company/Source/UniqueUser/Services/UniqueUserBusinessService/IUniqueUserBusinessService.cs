using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UniqueUserBusinessService.Models;

// -----------------------------------------------------------------------
// <copyright file="IUniqueUserBusinessService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserBusinessService
{
    [ServiceContract]
    public interface IUniqueUserBusinessService
    {
        /// <summary>
        /// Get the complete list of groups 
        /// </summary>
        /// <returns>List<CompanyGroup>. Object company Group</returns>
        [OperationContract]
        List<CompanyGroup> GetBusinessGroup();

        /// <summary>
        /// Get the complete list of user groups 
        /// </summary>
        /// <returns>List<CompanyUserGroup>. Object company User Group</returns>
        [OperationContract]
        List<CompanyUserGroup> GetBusinessUserGroup(int userId);

        /// <summary>
        /// Save the complete list of user groups 
        /// </summary>
        /// <returns>CompanyUserGroup. Object company User Group</returns>
        [OperationContract]
        CompanyUserGroup SaveBusinessUserGroup(List<CompanyUserGroup> companyUserGroups);
    }
}
