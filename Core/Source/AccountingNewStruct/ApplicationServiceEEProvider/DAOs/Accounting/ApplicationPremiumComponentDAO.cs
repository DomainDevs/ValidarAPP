using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;


namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    internal class ApplicationPremiumComponentDAO {

        public List<Models.Imputations.ApplicationPremiumComponent> GetApplicationPremiumComponentsByAppPremium(int appPremiumId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode, appPremiumId);
            
            var applicationPremiumComponents = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.ApplicationPremiumComponent), criteriaBuilder.GetPredicate());
            return ModelAssembler.CreateApplicationPremiumComponents(applicationPremiumComponents);
        }
    }
}
