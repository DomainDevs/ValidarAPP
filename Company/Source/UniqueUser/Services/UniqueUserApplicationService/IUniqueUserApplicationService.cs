using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Company.Application.UniqueUserApplicationServices.DTO;

// -----------------------------------------------------------------------
// <copyright file="IUniqueUserApplicationService.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserApplicationServices
{
    /// <summary>
    /// interfaz "IParametrizationAplicationService"
    /// </summary>  
    [ServiceContract]
    public interface IUniqueUserApplicationService
    {
        /// <summary>
        /// List of groups, Initial charge
        /// </summary>
        /// <returns>GroupQueryDTO. Object Group DTO</returns>
        [OperationContract]
        GroupQueryDTO GetApplicationGroup();

        /// <summary>
        /// List of user groups, Initial charge
        /// </summary>
        /// <returns>UserGroupQueryDTO. Object User Group DTO</returns>
        [OperationContract]
        UserGroupQueryDTO GetApplicationUserGroup(int userId);

        /// <summary>
        /// Save list of user groups.
        /// </summary>
        /// <returns>UserGroupDTO. Object User Group DTO</returns>
        [OperationContract]
        UserGroupDTO SaveApplicationUserGroup(List<UserGroupDTO> userGroupDTO);
    }
}
