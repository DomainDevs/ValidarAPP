
namespace Sistran.Company.Application.UniqueUserParamService
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.UniqueUserParamService.Models;
    using Sistran.Core.Application.UniqueUserServices.Models;

    /// <summary>
    /// Interfaz de parametrización.
    /// </summary>
    [ServiceContract]
    public interface IUniqueUserParamServiceWeb
    {

        /// <summary>
        /// Obtiene el listado de puntos de venta de aliado por usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="allianceId">Identificador del aliado.</param>
        /// <param name="individualId">IndividualId del intermediario.</param>
        /// <param name="agentAgencyId">Identificador de la agencia del intermediario.</param>
        /// <returns></returns>
        [OperationContract]
        List<UniqueUserSalePointBranch> GetUniqueUserSalePointBranch(int userId, int allianceId, int individualId, int agentAgencyId);

        /// <summary>
        /// Obtiene el listado de puntos de venta de aliado por usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="allianceId">Identificador del aliado.</param>
        /// <param name="individualId">IndividualId del intermediario.</param>
        /// <param name="agentAgencyId">Identificador de la agencia del intermediario.</param>
        /// <returns></returns>
        [OperationContract]
        List<UniqueUserSalePointBranch> GetUniqueUserSalePointByUserIdAllianceIdIndividualIdAgentAgencyId(int userId, int allianceId,int branchId,  int individualId, int agentAgencyId);

        /// <summary>
        /// Guarda el objeto User
        /// </summary>
        /// <param name="User">Model User</param>
        /// <returns>User</returns>
        [OperationContract]
        int CreateUniqueUser(CompanyUserParam user);


        /// <summary>
        /// Obtiene el texto de validación para el formulario usuarios puntos de venta aliados. 
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Texto de validación</returns>
        [OperationContract]
        string GetUniqueUserSalePointText(int userId);

        /// <summary>
        /// Obtiene el objeto de validación para el formulario usuarios aliados. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CptUniqueUserSalePointAlliance> GetUniqueUserAlliedText(int userId);

    }
}
