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
    class BrokerCheckingAccountTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveBrokerCheckingAccountTransactionItem
        /// </summary>
        /// <param name="brokersCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        ///<param name="tempBrokerParentId"> </param>
        ///<param name="billCode"></param>
        /// <param name="agentTypeId"></param>
        /// <param name="accountingDate"></param>
        ///<returns>BrokersCheckingAccountTransactionItem</returns>
        public BrokersCheckingAccountTransactionItem SaveBrokerCheckingAccountTransactionItem(BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem, int imputationId, int tempBrokerParentId, int billCode, int agentTypeId, DateTime accountingDate)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccountTransEntity = EntityAssembler.CreateBrokerCheckingAccount(brokersCheckingAccountTransactionItem, imputationId, tempBrokerParentId, billCode, agentTypeId, accountingDate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(brokerCheckingAccountTransEntity);

                // Return del model
                return ModelAssembler.CreateBrokersCheckingAccountTransactionItem(brokerCheckingAccountTransEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateBrokerCheckingAccountTransactionItem
        /// </summary>
        /// <param name="brokersCheckingAccountTransactionItem"></param>
        /// <param name="brokerParentId"></param>
        /// <returns>BrokersCheckingAccountTransactionItem</returns>
        public BrokersCheckingAccountTransactionItem UpdateBrokerCheckingAccountTransactionItem(BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem, int brokerParentId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BrokerCheckingAccountTrans.CreatePrimaryKey(brokersCheckingAccountTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingTransAccountEntity = (ACCOUNTINGEN.BrokerCheckingAccountTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                brokerCheckingTransAccountEntity.BrokerParentCode = brokerParentId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(brokerCheckingTransAccountEntity);

                // Return del model
                return ModelAssembler.CreateBrokersCheckingAccountTransactionItem(brokerCheckingTransAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBrokerCheckingAccountTransactionItem
        /// </summary>
        /// <param name="brokerCheckingAccountTransactionItem"> </param>
        /// <returns>BrokersCheckingAccountTransactionItem</returns>
        public BrokersCheckingAccountTransactionItem GetBrokerCheckingAccountTransactionItem(BrokersCheckingAccountTransactionItem brokerCheckingAccountTransactionItem)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.BrokerCheckingAccountTrans.CreatePrimaryKey(brokerCheckingAccountTransactionItem.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccountTransEntity = (ACCOUNTINGEN.BrokerCheckingAccountTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateBrokersCheckingAccountTransactionItem(brokerCheckingAccountTransEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
