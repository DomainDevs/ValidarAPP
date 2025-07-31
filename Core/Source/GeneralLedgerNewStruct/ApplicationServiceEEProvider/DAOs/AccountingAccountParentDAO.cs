#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

#endregion
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AccountingAccountParentDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingAccountParent
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public AccountingAccount SaveAccountingAccountParent(AccountingAccount accountingAccount)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingAccountParent accountingAccountParentEntity = EntityAssembler.CreateAccountingAccountParent(accountingAccount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingAccountParentEntity);

                // Return del model
                return ModelAssembler.CreateAccountingAccountParent(accountingAccountParentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAccountingAccountParent
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public AccountingAccount UpdateAccountingAccountParent(AccountingAccount accountingAccount)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountParent.CreatePrimaryKey(accountingAccount.AccountingAccountId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingAccountParent accountingAccountParentEntity = (GENERALLEDGEREN.AccountingAccountParent)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                accountingAccountParentEntity.Description = accountingAccount.Description;
                
                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingAccountParentEntity);

                // Return del model
                return ModelAssembler.CreateAccountingAccountParent(accountingAccountParentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAccountingAccountParent
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        public bool DeleteAccountingAccountParent(int accountingAccountId)
        {
            var isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountParent.CreatePrimaryKey(accountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccountParent accountingAccountParentEntity = (GENERALLEDGEREN.AccountingAccountParent)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingAccountParentEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetAccountingAccountParent
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        public AccountingAccount GetAccountingAccountParent(int accountingAccountId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountParent.CreatePrimaryKey(accountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccountParent accountingAccountParentEntity = (GENERALLEDGEREN.AccountingAccountParent)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingAccountParent(accountingAccountParentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccountParents
        /// </summary>
        /// <returns></returns>
        public List<AccountingAccount> GetAccountingAccountParents()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccountParent)));
                
                // Return como Lista
                return ModelAssembler.CreateAccountingAccountParents(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}