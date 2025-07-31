using System.Collections.Generic;
using AutoMapper;
using System;
using Sistran.Company.Application.UniqueUserApplicationServices.DTO;
using Sistran.Company.Application.UniqueUserBusinessService.Models;

namespace Sistran.Company.Application.UniqueUserApplicationServiceProvider.Assemblers
{
    /// <summary>
    /// CompanyAplicationAssembler. Assembler of Company to DTO 
    /// </summary>
    public class CompanyAplicationAssembler
    {
        /// <summary>
        /// Get list of Groups
        /// </summary>
        /// <param name="group">Object list CompanyGroup.</param>
        /// <returns>list of Models.GroupDTO</returns>
        public static List<GroupDTO> MappParamGroups(List<CompanyGroup> group)
        {
            List<GroupDTO> groups = new List<GroupDTO>();

            foreach (var item in group)
            {
                groups.Add(MappParamGroup(item));
            }
            return groups;
        }

        /// <summary>
        /// Get Group
        /// </summary>
        /// <param name="group">Object CompanyGroup.</param>
        /// <returns>Object of Models.GroupDTO</returns>
        public static GroupDTO MappParamGroup(CompanyGroup group)
        {
            GroupDTO groupDTO = new GroupDTO
            {
                Id = group.Id,
                SmallDescription = group.SmallDescription,
                Description = group.Description,
                State = group.State
            };

            return groupDTO;
        }

        /// <summary>
        /// Get list of User Group
        /// </summary>
        /// <param name="userGroup">Object list CompanyUserGroup.</param>
        /// <returns>list of Models.UserGroupDTO</returns>
        public static List<UserGroupDTO> MappParamUserGroups(List<CompanyUserGroup> userGroup)
        {
            List<UserGroupDTO> userGroups = new List<UserGroupDTO>();

            foreach (var item in userGroup)
            {
                userGroups.Add(MappParamUserGroup(item));
            }
            return userGroups;
        }

        /// <summary>
        /// Get User Group
        /// </summary>
        /// <param name="userGroup">Object CompanyUserGroup.</param>
        /// <returns>Object of Models.UserGroupDTO</returns>
        public static UserGroupDTO MappParamUserGroup(CompanyUserGroup userGroup)
        {
            UserGroupDTO userGroupDTO = new UserGroupDTO
            {
                IdUser = userGroup.IdUser,
                IdGroup = userGroup.IdGroup
            };

            return userGroupDTO;
        }

        /// <summary>
        /// Get list of Company User Group
        /// </summary>
        /// <param name="userGroupDTO">Object list UserGroupDTO.</param>
        /// <returns>list of Models.CompanyUserGroup</returns>
        public static List<CompanyUserGroup> MappCompanyUserGroups(List<UserGroupDTO> userGroupDTO)
        {
            List<CompanyUserGroup> companyUserGroups = new List<CompanyUserGroup>();

            foreach (var item in userGroupDTO)
            {
                companyUserGroups.Add(MappCompanyUserGroup(item));
            }
            return companyUserGroups;
        }

        /// <summary>
        /// Get Company User Group
        /// </summary>
        /// <param name="userGroupDTO">Object UserGroupDTO.</param>
        /// <returns>Object of Models.CompanyUserGroup</returns>
        public static CompanyUserGroup MappCompanyUserGroup(UserGroupDTO userGroupDTO)
        {
            CompanyUserGroup companyUserGroup = new CompanyUserGroup
            {
                IdUser = userGroupDTO.IdUser,
                IdGroup = userGroupDTO.IdGroup
            };

            return companyUserGroup;
        }
    }
}
