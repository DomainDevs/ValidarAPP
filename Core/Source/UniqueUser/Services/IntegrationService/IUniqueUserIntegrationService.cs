using Sistran.Core.Integration.UniqueUserServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices
{
    [ServiceContract]

    public interface IUniqueUserIntegrationService
    {
        [OperationContract]
        UserDTO GetUserByLogin(string login);

        /// <summary>
        /// Obtener lista de sucursales asociadas a un usuario
        /// </summary>
        /// <param name="userId">Identificador usuario</param>
        /// <returns></returns>
        [OperationContract]
        List<BranchDTO> GetBranchesByUserId(int userId);

        [OperationContract]
        UserDTO GetUserByUserId(int userId);
    }
}
