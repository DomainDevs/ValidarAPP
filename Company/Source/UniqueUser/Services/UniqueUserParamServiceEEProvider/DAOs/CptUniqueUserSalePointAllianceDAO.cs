// -----------------------------------------------------------------------
// <copyright file="UniqueUserSalePointAllianceDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniqueUserParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniqueUserParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using System.Collections.Generic;
    using System.Linq;    
    using entitiesUUserCompany = Sistran.Company.Application.UniqueUser.Entities;
    using entitiesUPersonCompany = Sistran.Company.Application.UniquePerson.Entities;
    using Sistran.Company.Application.UniqueUserParamService.EEProvider.Entities.Views;
    using Sistran.Company.Application.UniqueUserParamService.EEProvider.Assemblers;
    using UniqueUserEntities = Sistran.Company.Application.UniqueUser.Entities;

    /// <summary>
    /// Clase de acceso a datos Unique User Product.
    /// </summary>
    public class CptUniqueUserSalePointAllianceDAO
    {
        /// <summary>
        /// Obtiene el listado de productos del usuario por ramo comercial.
        /// </summary>
        /// <param name="prefixCode">Código del ramo comercial.</param>
        /// <returns>Listado de productos del usuario por ramo comercial.</returns>  
        public List<CptUniqueUserSalePointAlliance> GetCptUniqueUserSalePointAlliances(int userId, int allianceId, int individualId, int agentAgencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.UserId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.AllianceId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(allianceId);
            filter.And();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.IndividualId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.AgentAgencyId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(agentAgencyId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance), filter.GetPredicate()));
            List<CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceList = ModelAssembler.MappCptUniqueUserSalePointAllianceList(businessCollection);

            return cptUniqueUserSalePointAllianceList;
        }

        /// <summary>
        /// Obtiene el punto de venta aliado del usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="allianceId">Identificador del aliado.</param>
        /// <param name="branchAllianceId">Identificador de la sucursal del aliado.</param>
        /// <param name="salePointAllianceId">Identificador del punto de venta del aliado.</param>
        /// <param name="individualId">Individual Id del agente intermediario.</param>
        /// <param name="agentAgencyId">Identificador del agente intermediario.</param>
        /// <returns>Retorna la entidad de tipo CptUniqueUserSalePointAlliance.</returns>
        public entitiesUUserCompany.CptUniqueUserSalePointAlliance GetCptUniqueUserSalePointAllianceByPrimaryKey(int userId, int allianceId, int branchAllianceId, int salePointAllianceId, int individualId, int agentAgencyId)
        {

            PrimaryKey key = entitiesUUserCompany.CptUniqueUserSalePointAlliance.CreatePrimaryKey(userId,allianceId,branchAllianceId,salePointAllianceId,individualId,agentAgencyId);
            entitiesUUserCompany.CptUniqueUserSalePointAlliance cptUniqueUserSalePointAlliancetEntity = (entitiesUUserCompany.CptUniqueUserSalePointAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return cptUniqueUserSalePointAlliancetEntity;
        }

        /// <summary>
        /// Asigna los puntos de venta de aliado al usuario.
        /// </summary>        
        public void SaveCptUniqueUserSalePointAlliance(List<CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceModelList, int userId)
        {
            List<entitiesUUserCompany.CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceEntityList = EntityAssembler.MappCptUniqueUserSalePointAllianceList(cptUniqueUserSalePointAllianceModelList);
            foreach (entitiesUUserCompany.CptUniqueUserSalePointAlliance itemCptUniqueUserSalePointAllianceEntity in cptUniqueUserSalePointAllianceEntityList)
            {
                itemCptUniqueUserSalePointAllianceEntity.UserId = userId;
                entitiesUUserCompany.CptUniqueUserSalePointAlliance cptUniqueUserSalePointAllianceEntity = this.GetCptUniqueUserSalePointAllianceByPrimaryKey(itemCptUniqueUserSalePointAllianceEntity.UserId, itemCptUniqueUserSalePointAllianceEntity.AllianceId, itemCptUniqueUserSalePointAllianceEntity.BranchAllianceId, itemCptUniqueUserSalePointAllianceEntity.SalePointAllianceId, itemCptUniqueUserSalePointAllianceEntity.IndividualId, itemCptUniqueUserSalePointAllianceEntity.AgentAgencyId);                
                if (cptUniqueUserSalePointAllianceEntity == null)
                {   
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(itemCptUniqueUserSalePointAllianceEntity);
                }
                else
                {
                    cptUniqueUserSalePointAllianceEntity.IsAssign = itemCptUniqueUserSalePointAllianceEntity.IsAssign;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(cptUniqueUserSalePointAllianceEntity);
                }                
            }            
        }

        /// <summary>
        /// Obtener lista de puntos de venta de aliado por usuario.
        /// </summary>       
        /// <returns>Lista De tipo UniqueUserSalePointBranch.</returns>
        public List<UniqueUserSalePointBranch> GetUniqueUserSalePointBranch(int userId, int allianceId, int individualId, int agentAgencyId)
        {            
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entitiesUPersonCompany.CptAgentAlliance.Properties.AllianceId, typeof(entitiesUPersonCompany.CptAgentAlliance).Name);            
            filter.Equal();
            filter.Constant(allianceId);
            filter.And();
            filter.Property(entitiesUPersonCompany.CptAgentAlliance.Properties.IndividualId, typeof(entitiesUPersonCompany.CptAgentAlliance).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(entitiesUPersonCompany.CptAgentAlliance.Properties.AgentAgencyId, typeof(entitiesUPersonCompany.CptAgentAlliance).Name);
            filter.Equal();
            filter.Constant(agentAgencyId);

            CptUniqueUserSalePointAllianceView objCptUniqueUserSalePointAllianceView = new CptUniqueUserSalePointAllianceView();
            ViewBuilder builder = new ViewBuilder("CptUniqueUserSalePointAllianceView");

            
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, objCptUniqueUserSalePointAllianceView);

            if (objCptUniqueUserSalePointAllianceView.CptAllianceBranchSalePoint.Count > 0)
            {                
                List<CptUniqueUserSalePointAlliance> cptUniqueUserSalePointAllianceList = GetCptUniqueUserSalePointAlliances(userId, allianceId, individualId, agentAgencyId);
                return ModelAssembler.MappUniqueUserSalePointBranch(objCptUniqueUserSalePointAllianceView, cptUniqueUserSalePointAllianceList);
            }

            return null;            
        }

        /// <summary>
        /// Obtiene el texto de validación para el formulario usuarios puntos de venta aliados. 
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Texto de validación</returns>
        public string GetUniqueUserSalePointText(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.UserId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(userId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance), filter.GetPredicate()));
            List<entitiesUUserCompany.CptUniqueUserSalePointAlliance> uniqueUsersSalePointAlliance = new List<entitiesUUserCompany.CptUniqueUserSalePointAlliance>();
            foreach (entitiesUUserCompany.CptUniqueUserSalePointAlliance item in businessCollection)
            {
                if (item.IsAssign!=null && item.IsAssign==true)
                {
                    uniqueUsersSalePointAlliance.Add(item);
                }
            }
            if (uniqueUsersSalePointAlliance.Count>1)
            {
                return "Varios";
            }
            if (uniqueUsersSalePointAlliance.Count==1)
            {
                return this.GetUniqueUserSalePointBranch(userId, uniqueUsersSalePointAlliance[0].AllianceId, uniqueUsersSalePointAlliance[0].IndividualId, uniqueUsersSalePointAlliance[0].AgentAgencyId)[0].SalePointDescription;
            }
            return "";
        }

        /// <summary>
        /// Obtener lista de puntos de venta de aliado por usuario, aliado, individual Id 
        /// </summary>       
        /// <returns>Lista De tipo UniqueUserSalePointBranch.</returns>
        public List<UniqueUserSalePointBranch> GetUniqueUserSalePointByUserIdAllianceIdIndividualIdAgentAgencyId(int userId, int allianceId, int branchId, int individualId, int agentAgencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entitiesUPersonCompany.CptAgentAlliance.Properties.AllianceId, typeof(entitiesUPersonCompany.CptAgentAlliance).Name);
            filter.Equal();
            filter.Constant(allianceId);
            filter.And();
            filter.Property(entitiesUPersonCompany.CptAgentAlliance.Properties.IndividualId, typeof(entitiesUPersonCompany.CptAgentAlliance).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(entitiesUPersonCompany.CptAgentAlliance.Properties.AgentAgencyId, typeof(entitiesUPersonCompany.CptAgentAlliance).Name);
            filter.Equal();
            filter.Constant(agentAgencyId);
            filter.And();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.UserId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(entitiesUUserCompany.CptUniqueUserSalePointAlliance.Properties.BranchAllianceId, typeof(entitiesUUserCompany.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(branchId);

            CptUniqueUserSalePointAllianceView objCptUniqueUserSalePointAllianceView = new CptUniqueUserSalePointAllianceView();
            ViewBuilder builder = new ViewBuilder("CptUniqueUserSalePointAllianceView");


            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, objCptUniqueUserSalePointAllianceView);

            if (objCptUniqueUserSalePointAllianceView.CptAllianceBranchSalePoint.Count > 0)
            {
                return ModelAssembler.MappUniqueUserSalePointBrancByCptUniqueUserSalePointAllianceh(objCptUniqueUserSalePointAllianceView);
            }

            return null;
        }

        public List<CptUniqueUserSalePointAlliance> GetUniqueUserAlliedText(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUserEntities.CptUniqueUserSalePointAlliance.Properties.UserId, typeof(UniqueUserEntities.CptUniqueUserSalePointAlliance).Name);
            filter.Equal();
            filter.Constant(userId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUserEntities.CptUniqueUserSalePointAlliance), filter.GetPredicate()));
            List<UniqueUserEntities.CptUniqueUserSalePointAlliance> uniqueUsersSalePointAlliance = new List<UniqueUserEntities.CptUniqueUserSalePointAlliance>();
            List<CptUniqueUserSalePointAlliance> listUniqueUsersSalePointAllianceMod = new List<CptUniqueUserSalePointAlliance>();
            foreach (UniqueUserEntities.CptUniqueUserSalePointAlliance item in businessCollection)
            {
                if (item.IsAssign != null && item.IsAssign == true)
                {
                    uniqueUsersSalePointAlliance.Add(item);
                    listUniqueUsersSalePointAllianceMod.Add(ModelAssembler.MappCptUniqueUserSalePointAlliance(item));
                }
            }
            if (uniqueUsersSalePointAlliance.Count >= 1)
            {
                return listUniqueUsersSalePointAllianceMod;

            }
            else
            {
                return null;
            }

        }



    }
}
