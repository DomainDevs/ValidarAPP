using AutoMapper;
using Sistran.Company.Application.UniqueUserParamService.EEProvider.Assemblers;
using Sistran.Company.Application.UniqueUserParamService.EEProvider.DAOs;
using Sistran.Company.Application.UniqueUserParamService.Models;
using COMMDAO = Sistran.Core.Application.CommonServices.EEProvider.DAOs;
using Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniqueUserParamService.EEProvider
{
    /// <summary>
    /// Clase que implementa la interfaz IUnderwritingParamServiceWeb.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniqueUserParamServiceEEProviderWeb : IUniqueUserParamServiceWeb
    {

        /// <summary>
        /// Obtiene el listado de puntos de venta de aliado por usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="allianceId">Identificador del aliado.</param>
        /// <param name="individualId">IndividualId del intermediario.</param>
        /// <param name="agentAgencyId">Identificador de la agencia del intermediario.</param>
        /// <returns></returns>
        public List<UniqueUserSalePointBranch> GetUniqueUserSalePointBranch(int userId, int allianceId, int individualId, int agentAgencyId)
        {
            try
            {
                CptUniqueUserSalePointAllianceDAO cptUniqueUserSalePointAllianceDAO = new CptUniqueUserSalePointAllianceDAO();
                return cptUniqueUserSalePointAllianceDAO.GetUniqueUserSalePointBranch(userId, allianceId, individualId, agentAgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda el objeto User
        /// </summary>
        /// <param name="User">Model User</param>
        /// <returns>User</returns>
        public int CreateUniqueUser(CompanyUserParam companyUserParam)
        {


            try
            {
                UserDAO userDAO = new UserDAO();
                var imapper = ModelAssembler.CreateMapUser();
                var user = imapper.Map<CompanyUserParam, User>(companyUserParam);
                int userId = userDAO.CreateUniqueUser(user);
                if (companyUserParam.CptUniqueUserSalePointAlliance != null)
                {
                    CptUniqueUserSalePointAllianceDAO uniqueUserSalePointAllianceDAO = new CptUniqueUserSalePointAllianceDAO();
                    uniqueUserSalePointAllianceDAO.SaveCptUniqueUserSalePointAlliance(companyUserParam.CptUniqueUserSalePointAlliance, userId);
                }

                return userId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<UniqueUserSalePointBranch> GetUniqueUserSalePointByUserIdAllianceIdIndividualIdAgentAgencyId(int userId, int allianceId, int branchId, int individualId, int agentAgencyId)
        {
            try
            {
                CptUniqueUserSalePointAllianceDAO cptUniqueUserSalePointAllianceDAO = new CptUniqueUserSalePointAllianceDAO();
                return cptUniqueUserSalePointAllianceDAO.GetUniqueUserSalePointByUserIdAllianceIdIndividualIdAgentAgencyId(userId, allianceId, branchId, individualId, agentAgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el texto de validación para el formulario usuarios puntos de venta aliados. 
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Texto de validación</returns>
        public string GetUniqueUserSalePointText(int userId)
        {
            CptUniqueUserSalePointAllianceDAO cptUniqueUserSalePointAllianceDAO = new CptUniqueUserSalePointAllianceDAO();
            return cptUniqueUserSalePointAllianceDAO.GetUniqueUserSalePointText(userId);
        }


        public List<CptUniqueUserSalePointAlliance> GetUniqueUserAlliedText(int userId)
        {
            CptUniqueUserSalePointAllianceDAO cptUniqueUserSalePointAllianceDAO = new CptUniqueUserSalePointAllianceDAO();
            return cptUniqueUserSalePointAllianceDAO.GetUniqueUserAlliedText(userId);
        }

    }
}
