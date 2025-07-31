using System;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class UsedAmountDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        /// <summary>
        /// SaveUsedAmount
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="depositPremiumTransactionId"></param>
        /// <returns>int</returns>
        public int SaveUsedAmount(CommonModels.Amount amount, int depositPremiumTransactionId, CommonModels.ExchangeRate exchangeRate, CommonModels.Amount localAmount)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.Amount usedAmountEntity = EntityAssembler.CreateUsedAmount(amount, depositPremiumTransactionId, exchangeRate, localAmount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(usedAmountEntity);

                return Convert.ToInt32(usedAmountEntity.DepositPremiumTransactionCode);
            }
            catch (ArgumentException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteUsedAmountsByDepositPremiumTransactionId
        /// </summary>
        /// <param name="depositPremiumTransactionId"></param>
        /// <returns>bool</returns>
        public bool DeleteUsedAmountsByDepositPremiumTransactionId(int depositPremiumTransactionId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Amount.Properties.DepositPremiumTransactionCode, depositPremiumTransactionId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Amount), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.Amount usedAmountEntity in businessCollection.OfType<ACCOUNTINGEN.Amount>())
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(usedAmountEntity);
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        #endregion
    }
}
