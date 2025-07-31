//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AccountingAccountMaskDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        /// <param name="resultId"></param>
        /// <returns>AccountingAccountMask</returns>
        public Models.AccountingRules.AccountingAccountMask SaveAccountingAccountMask(Models.AccountingRules.AccountingAccountMask accountingAccountMask, int resultId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingAccountMask accountingAccountMaskEntity = EntityAssembler.CreateAccountingAccountMask(accountingAccountMask, resultId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingAccountMaskEntity);

                // Return del model
                return ModelAssembler.CreateAccountingAccountMask(accountingAccountMaskEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        /// <param name="resultId"></param>
        /// <returns>AccountingAccountMask</returns>
        public Models.AccountingRules.AccountingAccountMask UpdateAccountingAccountMask(Models.AccountingRules.AccountingAccountMask accountingAccountMask, int resultId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountMask.CreatePrimaryKey(accountingAccountMask.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingAccountMask accountingAccountMaskEntity = (GENERALLEDGEREN.AccountingAccountMask)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                accountingAccountMaskEntity.ResultId = resultId;
                accountingAccountMaskEntity.ParameterId = accountingAccountMask.Parameter.Id;
                accountingAccountMaskEntity.Mask = accountingAccountMask.Mask;
                accountingAccountMaskEntity.StartPosition = accountingAccountMask.Start;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingAccountMaskEntity);

                // Return del model
                return ModelAssembler.CreateAccountingAccountMask(accountingAccountMaskEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMaskId"></param>
        public void DeleteAccountingAccountMask(int accountingAccountMaskId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountMask.CreatePrimaryKey(accountingAccountMaskId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccountMask accountingAccountMaskEntity = (GENERALLEDGEREN.AccountingAccountMask)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingAccountMaskEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        /// <returns></returns>
        public Models.AccountingRules.AccountingAccountMask GetAccountingAccountMask(Models.AccountingRules.AccountingAccountMask accountingAccountMask)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountMask.CreatePrimaryKey(accountingAccountMask.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccountMask accountingAccountMaskEntity = (GENERALLEDGEREN.AccountingAccountMask)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingAccountMask(accountingAccountMaskEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccountMasks
        /// </summary>
        /// <returns>List<AccountingAccountMask></returns>
        public List<Models.AccountingRules.AccountingAccountMask> GetAccountingAccountMasks()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccountMask)));

                // Return como Lista
                return ModelAssembler.CreateAccountingAccountMasks(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        #endregion
    }
}
