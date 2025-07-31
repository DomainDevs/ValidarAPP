using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class PolicyComponentDistributionDAO
    {
        #region Instance variables
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #endregion 
        public ApplicationPremiumComponent saveApplicationPremiumComponent(ApplicationPremiumComponent applicationPremiumComponent)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationPremiumComponent entityAppPremiumComponentEntity = EntityAssembler.CreateApplicationPremiumComponent(applicationPremiumComponent);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entityAppPremiumComponentEntity);

                // Return del model
                return ModelAssembler.CreateApplicationPremiumComponent(entityAppPremiumComponentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ApplicationPremiumComponentLBSB saveApplicationPremiumComponentLBSB(ApplicationPremiumComponentLBSB applicationPremiumComponentLBSB)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationPremiumComponentLbsb entityApplicationPremiumComponentLbsb = EntityAssembler.CreateApplicationPremiumComponentLbsb(applicationPremiumComponentLBSB);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entityApplicationPremiumComponentLbsb);

                // Return del model
                return ModelAssembler.CreateApplicationPremiumComponentLbsb(entityApplicationPremiumComponentLbsb);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ApplicationPremiumComponent> GetApplicationPremiumComponentsByAppPremium(int appPremiumId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode, appPremiumId);

            var applicationPremiumComponents = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.ApplicationPremiumComponent), criteriaBuilder.GetPredicate());
            return ModelAssembler.CreateApplicationPremiumComponents(applicationPremiumComponents);
        }

        public List<ApplicationPremiumComponentLBSB> GetApplicationPremiumComponentsLBSBByAppPremiumComponent(int appPremiumComponentId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.AppComponentCode, appPremiumComponentId);

            var applicationPremiumComponentsLbsb = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.ApplicationPremiumComponentLbsb), criteriaBuilder.GetPredicate());
            return ModelAssembler.CreateApplicationPremiumComponentsLbsb(applicationPremiumComponentsLbsb);
        }
    }
}
