//System
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class TempApplicationPremiumComponentDAO
    {

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempApplicationPremiumComponent"></param>
        /// <returns></returns>
        public bool SaveTempApplicationPremiumComponent(TempApplicationPremiumComponent TempApplicationPremiumComponent)
        {
            try
            {
                // Convertir de model a entity
                var entityApplicationPremium = EntityAssembler.CreateTempApplicationPremiumComponent(TempApplicationPremiumComponent);

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityApplicationPremium);

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempApplicationPremiumComponent"></param>
        /// <returns></returns>
        public TempApplicationPremiumComponent UpdateTempApplicationPremiumComponent(TempApplicationPremiumComponent tempApplicationPremiumComponent)
        {

            // Convertir de model a entity
            var primaryKey = ACCOUNTINGEN.TempApplicationPremiumComponent.CreatePrimaryKey(tempApplicationPremiumComponent.Id);
            var entityApplicationPremium = (ACCOUNTINGEN.TempApplicationPremiumComponent)DataFacadeManager.GetObject(primaryKey);

            entityApplicationPremium.TempAppPremiumCode = tempApplicationPremiumComponent.TempApplicationPremiumCode;
            entityApplicationPremium.ComponentCode = tempApplicationPremiumComponent.ComponentCode;
            entityApplicationPremium.CurrencyCode = tempApplicationPremiumComponent.ComponentCurrencyCode;
            entityApplicationPremium.ExchangeRate = tempApplicationPremiumComponent.ExchangeRate;
            entityApplicationPremium.Amount = tempApplicationPremiumComponent.Amount;
            entityApplicationPremium.LocalAmount = tempApplicationPremiumComponent.LocalAmount;
            entityApplicationPremium.MainAmount = tempApplicationPremiumComponent.MainAmount;
            entityApplicationPremium.MainLocalAmount = tempApplicationPremiumComponent.MainLocalAmount;

            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Update(entityApplicationPremium);

            return ModelAssembler.CreateTempApplicationComponent(entityApplicationPremium);

        }

        public List<Models.Imputations.TempApplicationPremiumComponent> GetTempApplicationPremiumComponentsByTemAppPremium(int tempAppPremium)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumComponent.Properties.TempAppPremiumCode, tempAppPremium);
            TempApplicationPremiumComponentView view = new TempApplicationPremiumComponentView();
            ViewBuilder builder = new ViewBuilder("TempApplicationPremiumComponentView");
            builder.Filter = criteriaBuilder.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            var tempAppComps=  ModelAssembler.CreateTempApplicationComponents(view.TempApplicationPremiumComponents);
            var components = view.Components.Select(m => (Parameters.Entities.Component)m);
            var componentTypes = view.ComponentTypes.Select(m => (Parameters.Entities.ComponentType)m);

            var tempApplicationPremiumComponents =  from tac in tempAppComps
            join c in components
            on tac.ComponentCode equals c.ComponentCode
                     join ct in componentTypes
            on c.ComponentTypeCode equals ct.ComponentTypeCode
                    select new TempApplicationPremiumComponent
                    {
                        Amount = tac.Amount,
                        ComponentCode = tac.ComponentCode,
                        ComponentCurrencyCode = tac.ComponentCurrencyCode,
                        ComponentTinyDescription = ct.TinyDescription,
                        ExchangeRate = tac.ExchangeRate,
                        Id = tac.Id,
                        LocalAmount = tac.LocalAmount,
                        MainAmount = tac.MainAmount,
                        MainLocalAmount = tac.MainLocalAmount,
                        TempApplicationPremiumCode = tac.TempApplicationPremiumCode,
                    };

            return tempApplicationPremiumComponents.ToList();
        }

        public void DeleteTempApplicationPremiumComponentsByTemApp(int tempApp)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumComponent.Properties.TempAppPremiumCode, tempApp);

            var tempApplicationPremiumComponents= DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.TempApplicationPremiumComponent), criteriaBuilder.GetPredicate());

            tempApplicationPremiumComponents.Select(m => DataFacadeManager.Delete(m.PrimaryKey));
        }
        #endregion
    }
}
