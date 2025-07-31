//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class CoinsuranceBalanceDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveCoinsuranceBalance
        /// </summary>
        /// <param name="coinsuredBalance"></param>
        /// <returns>bool</returns>
        public bool SaveCoinsuranceBalance(CoinsuranceBalanceDTO coinsuredBalance)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CoinsuranceBalance coinsuredBalanceEntity = EntityAssembler.CreateCoinsuranceBalance(coinsuredBalance);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(coinsuredBalanceEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// UpdateCoinsuranceBalance
        /// </summary>
        /// <param name="coinsuranceBalance"></param>
        /// <returns>bool</returns>
        public bool UpdateCoinsuranceBalance(CoinsuranceBalanceDTO coinsuranceBalance)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CoinsuranceBalance.CreatePrimaryKey(coinsuranceBalance.CoinsuranceBalanceId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CoinsuranceBalance coinsuranceBalanceEntity = (ACCOUNTINGEN.CoinsuranceBalance)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                coinsuranceBalanceEntity.CoinsuredCompanyId = coinsuranceBalance.CoinsuredCompanyId;
                coinsuranceBalanceEntity.BalanceDate = Convert.ToDateTime(coinsuranceBalance.BalanceDate);
                coinsuranceBalanceEntity.CurrencyCode = Convert.ToInt32(coinsuranceBalance.CurrencyId);
                coinsuranceBalanceEntity.LastBalanceDate = Convert.ToDateTime(coinsuranceBalance.LastBalanceDate);
                coinsuranceBalanceEntity.BalanceAmount = Convert.ToDecimal(coinsuranceBalance.BalanceAmount);
                coinsuranceBalanceEntity.BalanceIncomeAmount = Convert.ToDecimal(coinsuranceBalance.BalanceIncomeAmount);
                coinsuranceBalanceEntity.NumSheet = Convert.ToInt32(coinsuranceBalance.NumSheet);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(coinsuranceBalanceEntity);

                // Return del model
                return true;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// GetCoinsuranceBalance
        /// </summary>
        /// <param name="coinsuranceBalance"></param>
        /// <returns>CoinsuranceBalanceDTO</returns>
        public CoinsuranceBalanceDTO GetCoinsuranceBalance(CoinsuranceBalanceDTO coinsuranceBalance)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.CoinsuranceBalance.CreatePrimaryKey(coinsuranceBalance.CoinsuranceBalanceId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CoinsuranceBalance coinsuranceBalanceEntity = (ACCOUNTINGEN.CoinsuranceBalance)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateCoinsuranceBalance(coinsuranceBalanceEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
