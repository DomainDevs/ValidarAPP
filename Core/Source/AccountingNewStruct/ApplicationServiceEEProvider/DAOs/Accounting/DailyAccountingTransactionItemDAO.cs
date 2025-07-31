//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class DailyAccountingTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveDailyAccountingTransactionItem
        /// </summary>
        /// <param name="dailyAccountingTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        /// 
        public int SaveDailyAccountingTransactionItem(DailyAccountingTransactionItem dailyAccountingTransactionItem, int imputationId,
                                                      int paymentConceptCode, string description, int bankReconciliationId,
                                                      int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.DailyAccountingTrans dailyAccountingEntity =
                    EntityAssembler.CreateDailyAccountingTrans(dailyAccountingTransactionItem, imputationId,
                                                          paymentConceptCode, description, bankReconciliationId,
                                                          receiptNumber, receiptDate, postdatedAmount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(dailyAccountingEntity);

                // Return del model
                return dailyAccountingEntity.DailyAccountingTransId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteDailyAccountingTransactionItem
        /// </summary>
        /// <param name="dailyAccountingTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteDailyAccountingTransactionItem(int dailyAccountingTransactionItemId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.DailyAccountingTrans.CreatePrimaryKey(dailyAccountingTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.DailyAccountingTrans dailyAccountingEntity = (ACCOUNTINGEN.DailyAccountingTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(dailyAccountingEntity);
                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }
    }
}
