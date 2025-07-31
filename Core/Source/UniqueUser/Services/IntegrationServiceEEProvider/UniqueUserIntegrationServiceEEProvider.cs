using IntegrationServiceEEProvider.Assemblers;
using Sistran.Core.Integration.UniqueUserServices;
using Sistran.Core.Integration.UniqueUserServices.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Integration.UniqueUserIntegrationService.EEProvider
{
    public class UniqueUserIntegrationServiceEEProvider: IUniqueUserIntegrationService
    {
        public UserDTO GetUserByLogin(string login)
        {
            return DTOAssembler.CreateUserByLogin(DelegateService.uniqueUserService.GetUserByLogin(login));
        }

        public List<BranchDTO> GetBranchesByUserId(int userId)
        {
            return DelegateService.uniqueUserService.GetBranchesByUserId(userId).ToDTOs().ToList();
        }

        public UserDTO GetUserByUserId(int userId)
        {
            return DTOAssembler.CreateUserByLogin(DelegateService.uniqueUserService.GetUserById(userId));
        }
    }
}
