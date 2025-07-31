namespace Sistran.Company.Application.UniqueUserParamService.EEProvider.Assemblers
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using entitiesUPersonCompany = Sistran.Company.Application.UniquePerson.Entities;
    using entitiesUUserCompany = Sistran.Company.Application.UniqueUser.Entities;
    using Sistran.Company.Application.UniqueUserParamService.Models;
    public static class EntityAssembler
    {
        /// <summary>
        /// Mapea una lista de modelos de negocio UniqueUsersProduct a una lista de entidades UniqueUsersProduct.
        /// </summary>
        /// <param name="uniqueUsersProductModelList">Listado de modelos de negocio de tipo UniqueUsersProduct.</param>
        /// <returns>Listado de entidades de tipo UniqueUsersProduct.</returns>
        public static List<entitiesUUserCompany.CptUniqueUserSalePointAlliance> MappCptUniqueUserSalePointAllianceList(List<CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceModelList)
        {
            List<entitiesUUserCompany.CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceEntityList = new List<entitiesUUserCompany.CptUniqueUserSalePointAlliance>();

            foreach (CptUniqueUserSalePointAlliance cptUniqueUserSalePointAllianceModel in cptUniqueUserSalePointAllianceModelList)
            {
                cptUniqueUserSalePointAllianceEntityList.Add(EntityAssembler.MappCptUniqueUserSalePointAlliance(cptUniqueUserSalePointAllianceModel));
            }

            return cptUniqueUserSalePointAllianceEntityList;
        }

        public static entitiesUUserCompany.CptUniqueUserSalePointAlliance MappCptUniqueUserSalePointAlliance(CptUniqueUserSalePointAlliance cptUniqueUserSalePointAllianceModel)
        {
            return new entitiesUUserCompany.CptUniqueUserSalePointAlliance(cptUniqueUserSalePointAllianceModel.UserId, cptUniqueUserSalePointAllianceModel.AllianceId, cptUniqueUserSalePointAllianceModel.BranchAllianceId, cptUniqueUserSalePointAllianceModel.SalePointAllianceId, cptUniqueUserSalePointAllianceModel.IndividualId, cptUniqueUserSalePointAllianceModel.AgentAgencyId)
            {
                UserId = cptUniqueUserSalePointAllianceModel.UserId,
                AllianceId = cptUniqueUserSalePointAllianceModel.AllianceId,
                BranchAllianceId = cptUniqueUserSalePointAllianceModel.BranchAllianceId,
                SalePointAllianceId = cptUniqueUserSalePointAllianceModel.SalePointAllianceId,
                IndividualId = cptUniqueUserSalePointAllianceModel.IndividualId,
                AgentAgencyId = cptUniqueUserSalePointAllianceModel.AgentAgencyId,
                IsAssign = (bool)cptUniqueUserSalePointAllianceModel.IsAssign
            };
        }
    }
}
