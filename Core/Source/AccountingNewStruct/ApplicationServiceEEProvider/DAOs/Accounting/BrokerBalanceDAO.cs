//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class BrokerBalanceDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveBrokerBalance
        /// </summary>
        /// <param name="brokerBalance"></param>
        /// <returns>bool</returns>
        public bool SaveBrokerBalance(BrokerBalanceDTO brokerBalance)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.BrokerBalance brokerBalanceEntity = EntityAssembler.CreateBrokerBalance(brokerBalance);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(brokerBalanceEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// UpdateBrokerBalance
        /// </summary>
        /// <param name="brokerBalance"></param>
        /// <returns>bool</returns>
        public bool UpdateBrokerBalance(BrokerBalanceDTO brokerBalance)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BrokerBalance.CreatePrimaryKey(brokerBalance.BrokerBalanceId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.BrokerBalance brokerBalanceEntity = (ACCOUNTINGEN.BrokerBalance)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                brokerBalanceEntity.AgentTypeCode = brokerBalance.AgentTypeCode;
                brokerBalanceEntity.AgentId = brokerBalance.AgentCode;
                brokerBalanceEntity.BalanceDate = Convert.ToDateTime(brokerBalance.BalanceDate);
                brokerBalanceEntity.CurrencyCode = brokerBalance.CurrencyId;
                brokerBalanceEntity.LastBalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate);
                brokerBalanceEntity.PartialBalanceAmount = brokerBalance.PartialBalanceAmount;
                brokerBalanceEntity.PartialBalanceIncomeAmount = brokerBalance.PartialBalanceIncomeAmount;
                brokerBalanceEntity.TaxPartialSum = brokerBalance.TaxPartialSum;
                brokerBalanceEntity.TaxPartialSubtraction = brokerBalance.TaxPartialSubtraction;
                brokerBalanceEntity.TaxSum = brokerBalance.TaxSum;
                brokerBalanceEntity.TaxSubtraction = brokerBalance.TaxSubtraction;
                brokerBalanceEntity.NumSheet = brokerBalance.NumSheet;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(brokerBalanceEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// GetBrokerBalance
        /// </summary>
        /// <param name="brokerBalance"></param>
        /// <returns>BrokerBalanceDTO</returns>
        public BrokerBalanceDTO GetBrokerBalance(BrokerBalanceDTO brokerBalance)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.BrokerBalance.CreatePrimaryKey(brokerBalance.BrokerBalanceId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BrokerBalance brokerBalanceEntity = (ACCOUNTINGEN.BrokerBalance)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateBrokerBalance(brokerBalanceEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
