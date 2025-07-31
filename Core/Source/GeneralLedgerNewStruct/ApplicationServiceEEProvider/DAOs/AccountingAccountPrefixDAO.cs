//Sistran Core
using COMMOD=Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AccountingAccountPrefixDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingAccountPrefix
        /// </summary>
        /// <param name="accountingAccountPrefix"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public int SaveAccountingAccountPrefix(int accountingAccountPrefix, int accountingAccountId, int prefixId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingAccountPrefix accountingAccountPrefixEntity = EntityAssembler.CreateAccountingAccountPrefix(accountingAccountPrefix, accountingAccountId, prefixId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingAccountPrefixEntity);

                // Return del model
                return Convert.ToInt32(accountingAccountPrefixEntity.AccountingAccountId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAccountingAccountPrefix
        /// </summary>
        /// <param name="accountingAccountPrefix"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public int UpdateAccountingAccountPrefix(int accountingAccountPrefix, int accountingAccountId, int prefixId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountPrefix.CreatePrimaryKey(accountingAccountPrefix);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingAccountPrefix accountingAccountPrefixEntity = (GENERALLEDGEREN.AccountingAccountPrefix)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                int? accountingAccountCode = null;
                int? prefixCode = null;

                if (accountingAccountId > 0)
                {
                    accountingAccountCode = accountingAccountId;
                }

                if (prefixId > 0)
                {
                    prefixCode = prefixId;
                }

                accountingAccountPrefixEntity.AccountingAccountId = accountingAccountCode;
                accountingAccountPrefixEntity.PrefixCode = prefixCode;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingAccountPrefixEntity);

                // Return del model
                return accountingAccountPrefixEntity.AccountingAccountPrefixId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateAccountingPrefixByAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public int UpdateAccountingPrefixByAccountingAccount(AccountingAccount accountingAccount)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            try
            {
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountPrefix.Properties.AccountingAccountId, accountingAccount.AccountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountPrefix), criteriaBuilder.GetPredicate()));

                // Return como objeto
                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountPrefix prefixs in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountPrefix>())
                    {
                        DeleteAccountingAccountPrefix(prefixs.AccountingAccountPrefixId);
                    } 
                }

                if (accountingAccount.Prefixes != null && accountingAccount.Prefixes.Count > 0)
                {
                    foreach (COMMOD.Prefix prefix in accountingAccount.Prefixes)
                    {
                        accountingAccount.AccountingAccountId = SaveAccountingAccountPrefix(0, accountingAccount.AccountingAccountId, prefix.Id);
                    }
                }

                // Return del model
                return accountingAccount.AccountingAccountId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAccountingAccountPrefix
        /// </summary>
        /// <param name="accountingAccountPrefixId"></param>
        /// <returns></returns>
        public bool DeleteAccountingAccountPrefix(int accountingAccountPrefixId)
        {
            var isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountPrefix.CreatePrimaryKey(accountingAccountPrefixId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccountPrefix accountingAccountPrefixEntity = (GENERALLEDGEREN.AccountingAccountPrefix)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingAccountPrefixEntity);

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
        /// GetAccountingAccountPrefix
        /// </summary>
        /// <param name="accountingAccountPrefixId"></param>
        /// <returns></returns>
        public COMMOD.Prefix GetAccountingAccountPrefix(int accountingAccountPrefixId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountPrefix.CreatePrimaryKey(accountingAccountPrefixId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccountPrefix accountingAccountPrefixEntity = (GENERALLEDGEREN.AccountingAccountPrefix)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return new COMMOD.Prefix() { Id = Convert.ToInt32(accountingAccountPrefixEntity.PrefixCode) };
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}