using Sistran.Company.Application.UniqueUserServices.EEProvider.Resources;
using Sistran.Company.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UniqueUserServices.EEProvider;

namespace Sistran.Company.Application.UniqueUserServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniqueUserServiceEEProvider : UniqueUserServiceEEProviderCore, IUniqueUserService
    {

        public List<UserAgency> GetCompanyUserAgenciesByAgentIdDescription(int agentId, string description, int userId)
        {
            List<UserAgency> agencies = GetAgenciesByAgentIdDescriptionUserId(agentId, description, userId);

            if (agencies.Count == 1)
            {
                if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
                {
                    throw new Exception(Errors.ErrorIntermediaryDischarged);
                }
                else if (agencies[0].DateDeclined > DateTime.MinValue)
                {
                    throw new Exception(Errors.ErrorAgencyDischarged);
                }
                else
                {
                    return agencies;
                }
            }
            else if (agencies.Count == 0)
            {
                return agencies = GetAgenciesByIndividualIdAgentId(description);
            }
            else
            {
                return agencies;
            }
        }

        public List<CompanyUserGroup> GetUsersGroupByUserId(int userId)
        {
            //int[,] listGroups;//Hasta acá se retorna el company model pero el launcher sólo espera la lista de enteros
            var usersGroup = new List<CompanyUserGroup>();
            try
            {
                DAOs.UserGroupDAO userDao = new DAOs.UserGroupDAO();
                usersGroup = userDao.GetListUserGroupsByUserId(userId);
                //usersGroup.ForEach(x => listGroups.SetValue(x, i++));//Add(x.GroupId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            return usersGroup;
        }
        public List<CompanyUserBranch> GetCompanyBranchesByUserId(int userId, int isIssue)
        {
            try
            {
                DAOs.UserBranchDAO userBranchProvider = new DAOs.UserBranchDAO();
                return userBranchProvider.GetCompanyBranchesByUserId(userId, isIssue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
