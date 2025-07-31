using Sistran.Company.Application.UniqueUserServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UniqueUserServices;

namespace Sistran.Company.Application.UniqueUserServices
{
    [ServiceContract]
    public interface IUniqueUserService : IUniqueUserServiceCore
    {
        [OperationContract]
        List<UserAgency> GetCompanyUserAgenciesByAgentIdDescription(int agentId, string description, int userId);

        /// <summary>
        /// Obtiene la lista de grupos de usuarios por is de usuario
        /// </summary>
        /// <author>Germán F. Grimaldi</author>
        /// <date>24/10/2018</date>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyUserGroup> GetUsersGroupByUserId(int userId);

        [OperationContract]
        List<CompanyUserBranch> GetCompanyBranchesByUserId(int userId, int isIssue);
    }
}
