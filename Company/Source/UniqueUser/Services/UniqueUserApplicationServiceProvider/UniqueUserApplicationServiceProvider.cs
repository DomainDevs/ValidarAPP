namespace Sistran.Company.Application.UniqueUserApplicationServiceProvider
{
    using System;
    using System.ServiceModel;
    using System.Collections.Generic;
    using Sistran.Core.Framework.BAF;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.UniqueUserApplicationServices;
    using Sistran.Company.Application.UniqueUserApplicationServices.DTO;
    using Sistran.Company.Application.UniqueUserBusinessService.Models;
    using Sistran.Company.Application.UniqueUserBusinessServiceProvider;

    /// <summary>
    /// Class that implement the interface "IUniqueUserApplicationService"
    /// </summary>  
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniqueUserApplicationServiceProvider : IUniqueUserApplicationService
    {
        /// <summary>
        /// Get list of groups.
        /// </summary>
        /// <returns>GroupQueryDTO. Object list Group query DTO</returns>
        public GroupQueryDTO GetApplicationGroup()
        {
            UniqueUserBusinessServiceProvider providerBusiness = new UniqueUserBusinessServiceProvider();
            GroupQueryDTO GroupQueryDTO = new GroupQueryDTO();
            List<CompanyGroup> groups = new List<CompanyGroup>();
            List<GroupDTO> groupsDTO = new List<GroupDTO>();

            try
            {                
                groups = providerBusiness.GetBusinessGroup();

                if (groups != null)
                {
                   groupsDTO = Assemblers.CompanyAplicationAssembler.MappParamGroups(groups);                    
                }

                GroupQueryDTO.GroupDTO = groupsDTO;
                GroupQueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return GroupQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        // <summary>
        /// Get list of user groups.
        /// </summary>
        /// <returns>UserGroupQueryDTO. Object list user group query DTO</returns>
        public UserGroupQueryDTO GetApplicationUserGroup(int userId)
        {
            UniqueUserBusinessServiceProvider providerBusiness = new UniqueUserBusinessServiceProvider();
            UserGroupQueryDTO UserGroupQueryDTO = new UserGroupQueryDTO();
            List<CompanyUserGroup> userGroups = new List<CompanyUserGroup>();
            List<UserGroupDTO> userGroupsDTO = new List<UserGroupDTO>();

            try
            {                
                userGroups = providerBusiness.GetBusinessUserGroup(userId);

                if (userGroups != null)
                {
                    userGroupsDTO = Assemblers.CompanyAplicationAssembler.MappParamUserGroups(userGroups);                    
                }

                UserGroupQueryDTO.UserGroupDTO = userGroupsDTO;
                UserGroupQueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return UserGroupQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        // <summary>
        /// Save list of user groups.
        /// </summary>
        /// <returns>UserGroupDTO. Object list user group DTO</returns>
        public UserGroupDTO SaveApplicationUserGroup(List<UserGroupDTO> userGroupDTO)
        {
            UserGroupDTO saveUserGroupDTO = new UserGroupDTO();
            CompanyUserGroup companyUserGroup = new CompanyUserGroup();

            try
            {
                List<CompanyUserGroup> companyUserGroups = Assemblers.CompanyAplicationAssembler.MappCompanyUserGroups(userGroupDTO);
                UniqueUserBusinessServiceProvider providerBusiness = new UniqueUserBusinessServiceProvider();
                companyUserGroup = providerBusiness.SaveBusinessUserGroup(companyUserGroups);
                saveUserGroupDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return saveUserGroupDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
