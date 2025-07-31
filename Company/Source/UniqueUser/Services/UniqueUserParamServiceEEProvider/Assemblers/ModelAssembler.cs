namespace Sistran.Company.Application.UniqueUserParamService.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Sistran.Company.Application.CommonServices.Models;
    using Sistran.Company.Application.UniqueUserParamService.EEProvider.Entities.Views;
    using Sistran.Company.Application.UniqueUserParamService.Models;
    using Sistran.Company.Application.UniqueUserServices.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Framework.DAF;
    using entitiesUPersonCompany = Sistran.Company.Application.UniquePerson.Entities;
    using entitiesUUserCompany = Sistran.Company.Application.UniqueUser.Entities;
    using UserModel = Sistran.Core.Application.UniqueUserServices.Models;
    public static class ModelAssembler
    {
        /// <summary>
        /// Mapea una Vista de negocio de tipo CptUniqueUserSalePointAllianceView a modelo de negocio UniqueUserSalePointBranch.
        /// </summary>
        /// <param name="objCptUniqueUserSalePointAllianceView">Vista de negocio de tipo CptUniqueUserSalePointAllianceView.</param>
        /// <param name="cptUniqueUserSalePointAllianceList">Lista de tipo CptUniqueUserSalePointAlliance.</param>
        /// <returns>Lista de modelos de negocio UniqueUserSalePointBranch.</returns>
        public static List<UniqueUserSalePointBranch> MappUniqueUserSalePointBranch(CptUniqueUserSalePointAllianceView objCptUniqueUserSalePointAllianceView, List<CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceList)
        {
            List<UniqueUserSalePointBranch> uniqueUserSalePointBranchList = new List<UniqueUserSalePointBranch>();
            List<entitiesUPersonCompany.CptAllianceBranchSalePoint> cptAllianceBranchSalePointList = new List<entitiesUPersonCompany.CptAllianceBranchSalePoint>();
            List<entitiesUPersonCompany.CptBranchAlliance> cptBranchAllianceList = new List<entitiesUPersonCompany.CptBranchAlliance>();
            List<entitiesUPersonCompany.CptAgentAlliance> cptAgentAllianceList = new List<entitiesUPersonCompany.CptAgentAlliance>();

            cptAllianceBranchSalePointList = objCptUniqueUserSalePointAllianceView.CptAllianceBranchSalePoint.Cast<entitiesUPersonCompany.CptAllianceBranchSalePoint>().ToList();
            cptBranchAllianceList = objCptUniqueUserSalePointAllianceView.CptBranchAlliance.Cast<entitiesUPersonCompany.CptBranchAlliance>().ToList();
            cptAgentAllianceList = objCptUniqueUserSalePointAllianceView.CptAgentAlliance.Cast<entitiesUPersonCompany.CptAgentAlliance>().ToList();

            var queryResult = (from allyBranchSalePoint in cptAllianceBranchSalePointList
                               join branchAlly in cptBranchAllianceList
                               on new { allyBranchSalePoint.AllianceId, allyBranchSalePoint.BranchId } equals new { branchAlly.AllianceId, branchAlly.BranchId }
                               join agentAlliance in cptAgentAllianceList
                               on new { allyBranchSalePoint.AllianceId } equals new { agentAlliance.AllianceId }
                               select new
                               {
                                   SalePointId = allyBranchSalePoint.SalePointId,
                                   IndividualId = agentAlliance.IndividualId,
                                   AgentAgencyId = agentAlliance.AgentAgencyId,
                                   AllianceId = agentAlliance.AllianceId,
                                   BranchId = branchAlly.BranchId,
                                   SalePointDescription = allyBranchSalePoint.SalePointDescription,
                                   BranchDescription = branchAlly.BranchDescription
                               }).ToList();

            if (queryResult.Count == 0)
            {
                return new List<UniqueUserSalePointBranch>();
            }
            else if (cptUniqueUserSalePointAllianceList.Count == 0)
            {
                uniqueUserSalePointBranchList = (from q1 in queryResult
                                                 select new UniqueUserSalePointBranch
                                                 {
                                                     SalePointAllianceId = q1.SalePointId,
                                                     IndividualId = q1.IndividualId,
                                                     AgentAgencyId = q1.AgentAgencyId,
                                                     AllianceId = q1.AllianceId,
                                                     BranchAllianceId = q1.BranchId,
                                                     SalePointDescription = q1.SalePointDescription,
                                                     BranchDescription = q1.BranchDescription,
                                                     IsAssign = false
                                                 }).ToList();
            }
            else
            {
                uniqueUserSalePointBranchList = (from q1 in queryResult
                                                 join q2 in cptUniqueUserSalePointAllianceList.DefaultIfEmpty()
                                                 on new { q1.IndividualId, q1.AgentAgencyId, q1.AllianceId, branchId = q1.BranchId, salePointId = q1.SalePointId }
                                                 equals new { q2.IndividualId, q2.AgentAgencyId, q2.AllianceId, branchId = q2.BranchAllianceId, salePointId = q2.SalePointAllianceId }
                                                 into qAssign
                                                 from leftAssign in qAssign.DefaultIfEmpty()
                                                 select new UniqueUserSalePointBranch
                                                 {
                                                     SalePointAllianceId = q1.SalePointId,
                                                     IndividualId = q1.IndividualId,
                                                     AgentAgencyId = q1.AgentAgencyId,
                                                     AllianceId = q1.AllianceId,
                                                     BranchAllianceId = q1.BranchId,
                                                     SalePointDescription = q1.SalePointDescription,
                                                     BranchDescription = q1.BranchDescription,
                                                     IsAssign = leftAssign == null ? false : (bool)leftAssign.IsAssign
                                                 }).ToList();
                return uniqueUserSalePointBranchList;
            }
            return uniqueUserSalePointBranchList;
        }

        /// <summary>
        /// Mapea una entidad de tipo CptUniqueUserSalePointAlliance a modelo de negocio CptUniqueUserSalePointAlliance.
        /// </summary>
        /// <param name="cptUniqueUserSalePointAllianceEntity">Entidad de tipo CptUniqueUserSalePointAlliance.</param>
        /// <returns>Modelo de negocio CptUniqueUserSalePointAlliance.</returns>
        public static CptUniqueUserSalePointAlliance MappCptUniqueUserSalePointAlliance(entitiesUUserCompany.CptUniqueUserSalePointAlliance cptUniqueUserSalePointAllianceEntity)
        {
            return new Models.CptUniqueUserSalePointAlliance
            {
                UserId = cptUniqueUserSalePointAllianceEntity.UserId,
                AllianceId = cptUniqueUserSalePointAllianceEntity.AllianceId,
                BranchAllianceId = cptUniqueUserSalePointAllianceEntity.BranchAllianceId,
                SalePointAllianceId = cptUniqueUserSalePointAllianceEntity.SalePointAllianceId,
                IndividualId = cptUniqueUserSalePointAllianceEntity.IndividualId,
                AgentAgencyId = cptUniqueUserSalePointAllianceEntity.AgentAgencyId,
                IsAssign = (bool)cptUniqueUserSalePointAllianceEntity.IsAssign
            };
        }

        /// <summary>
        /// Mapea una lista de entidades CptUniqueUserSalePointAlliance a una lista de modelos de negocio CptUniqueUserSalePointAlliance.
        /// </summary>
        /// <param name="businessCollection">Lista de entidades CptUniqueUserSalePointAlliance.</param>
        /// <returns>Lista de modelos de negocio CptUniqueUserSalePointAlliance.</returns>
        public static List<CptUniqueUserSalePointAlliance> MappCptUniqueUserSalePointAllianceList(BusinessCollection businessCollection)
        {
            List<CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceList = new List<CptUniqueUserSalePointAlliance>();

            foreach (entitiesUUserCompany.CptUniqueUserSalePointAlliance cptUniqueUserSalePointAllianceEntity in businessCollection)
            {
                cptUniqueUserSalePointAllianceList.Add(MappCptUniqueUserSalePointAlliance(cptUniqueUserSalePointAllianceEntity));
            }

            return cptUniqueUserSalePointAllianceList;
        }

        /// <summary>
        /// Mapea una Vista de negocio de tipo CptUniqueUserSalePointAllianceView a modelo de negocio UniqueUserSalePointBranch.
        /// </summary>
        /// <param name="objCptUniqueUserSalePointAllianceView">Vista de negocio de tipo CptUniqueUserSalePointAllianceView.</param>
        /// <param name="cptUniqueUserSalePointAllianceList">Lista de tipo CptUniqueUserSalePointAlliance.</param>
        /// <returns>Lista de modelos de negocio UniqueUserSalePointBranch.</returns>
        public static List<UniqueUserSalePointBranch> MappUniqueUserSalePointBrancByCptUniqueUserSalePointAllianceh(CptUniqueUserSalePointAllianceView objCptUniqueUserSalePointAllianceView)
        {
            List<UniqueUserSalePointBranch> uniqueUserSalePointBranchList = new List<UniqueUserSalePointBranch>();
            List<entitiesUPersonCompany.CptAllianceBranchSalePoint> cptAllianceBranchSalePointList = new List<entitiesUPersonCompany.CptAllianceBranchSalePoint>();
            List<entitiesUPersonCompany.CptBranchAlliance> cptBranchAllianceList = new List<entitiesUPersonCompany.CptBranchAlliance>();
            List<entitiesUPersonCompany.CptAgentAlliance> cptAgentAllianceList = new List<entitiesUPersonCompany.CptAgentAlliance>();

            cptAllianceBranchSalePointList = objCptUniqueUserSalePointAllianceView.CptAllianceBranchSalePoint.Cast<entitiesUPersonCompany.CptAllianceBranchSalePoint>().ToList();
            cptBranchAllianceList = objCptUniqueUserSalePointAllianceView.CptBranchAlliance.Cast<entitiesUPersonCompany.CptBranchAlliance>().ToList();
            cptAgentAllianceList = objCptUniqueUserSalePointAllianceView.CptAgentAlliance.Cast<entitiesUPersonCompany.CptAgentAlliance>().ToList();

            var queryResult = (from allyBranchSalePoint in cptAllianceBranchSalePointList
                               join branchAlly in cptBranchAllianceList
                               on new { allyBranchSalePoint.AllianceId, allyBranchSalePoint.BranchId } equals new { branchAlly.AllianceId, branchAlly.BranchId }
                               join agentAlliance in cptAgentAllianceList
                               on new { allyBranchSalePoint.AllianceId } equals new { agentAlliance.AllianceId }
                               select new
                               {
                                   SalePointId = allyBranchSalePoint.SalePointId,
                                   IndividualId = agentAlliance.IndividualId,
                                   AgentAgencyId = agentAlliance.AgentAgencyId,
                                   AllianceId = agentAlliance.AllianceId,
                                   BranchId = branchAlly.BranchId,
                                   SalePointDescription = allyBranchSalePoint.SalePointDescription,
                                   BranchDescription = branchAlly.BranchDescription
                               }).ToList();

            if (queryResult.Count == 0)
            {
                return new List<UniqueUserSalePointBranch>();
            }
            else
            {
                uniqueUserSalePointBranchList = (from q1 in queryResult
                                                 select new UniqueUserSalePointBranch
                                                 {
                                                     SalePointAllianceId = q1.SalePointId,
                                                     IndividualId = q1.IndividualId,
                                                     AgentAgencyId = q1.AgentAgencyId,
                                                     AllianceId = q1.AllianceId,
                                                     BranchAllianceId = q1.BranchId,
                                                     SalePointDescription = q1.SalePointDescription,
                                                     BranchDescription = q1.BranchDescription,
                                                     IsAssign = false
                                                 }).ToList();
                return uniqueUserSalePointBranchList;
            }
        }


        #region Automapper
        #region User
        public static IMapper CreateMapUser()
        {
            var config = MapperCache.GetMapper<CompanyUserParam, UserModel.User>(cfg =>
            {
                cfg.CreateMap<CompanyUserParam, UserModel.User>();
                cfg.CreateMap<CompanyProfile, UserModel.Profile>();
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanySalesPoint, SalePoint>();
            });
            return config;
        }
        #endregion User
        #endregion Automapper

    }
}
