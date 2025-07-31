#region Using

//Sistran Core
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
using GENERALLEDGEREN=Sistran.Core.Application.GeneralLedger.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AccountingAccountCostCenterDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingAccountCostCenter
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public int SaveAccountingAccountCostCenter(int accountingAccountCostCenter, int accountingAccountId, int costCenterId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingAccountCostCenter accountingAccountCostCenterEntity = EntityAssembler.CreateAccountingAccountCostCenter(accountingAccountCostCenter, accountingAccountId, costCenterId);

                // Realizar las operaciones con los GENERALLEDGEREN utilizando DAF
                DataFacadeManager.Insert(accountingAccountCostCenterEntity);

                // Return del model
                return Convert.ToInt32( accountingAccountCostCenterEntity.AccountingAccountId);            
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAccountingAccountCostCenter
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public int UpdateAccountingAccountCostCenter(int accountingAccountCostCenter, int accountingAccountId, int costCenterId)
        {
            try
            {                
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountCostCenter.CreatePrimaryKey(accountingAccountCostCenter);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingAccountCostCenter accountingAccountCostCenterEntity = (GENERALLEDGEREN.AccountingAccountCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                accountingAccountCostCenterEntity.AccountingAccountId = accountingAccountId;
                accountingAccountCostCenterEntity.CostCenterId = costCenterId;
                
                // Realizar las operaciones con los GENERALLEDGEREN utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingAccountCostCenterEntity);

                // Return del model
                return accountingAccountCostCenterEntity.AccountingAccountCostCenterId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// UpdateAccountingCostCenterByAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public int UpdateAccountingCostCenterByAccountingAccount(AccountingAccount accountingAccount )
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            try
            {
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountCostCenter.Properties.AccountingAccountId, accountingAccount.AccountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountCostCenter), criteriaBuilder.GetPredicate()));

                // Return como objeto
                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountCostCenter costCenters in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountCostCenter>())
                    {
                        DeleteAccountingAccountCostCenter(costCenters.AccountingAccountCostCenterId);
                    } 
                }

                if (accountingAccount.CostCenters != null && accountingAccount.CostCenters.Count > 0)
                {
                    foreach (CostCenter costCenter in accountingAccount.CostCenters)
                    {
                        accountingAccount.AccountingAccountId = SaveAccountingAccountCostCenter(0, accountingAccount.AccountingAccountId, costCenter.CostCenterId);
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
        /// DeleteAccountingAccountCostCenter
        /// </summary>
        /// <param name="accountingAccountCostCenterId"></param>
        /// <returns></returns>
        public bool DeleteAccountingAccountCostCenter(int accountingAccountCostCenterId)
        {
            var isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountCostCenter.CreatePrimaryKey(accountingAccountCostCenterId);

                // Realizar las operaciones con los GENERALLEDGEREN utilizando DAF
                GENERALLEDGEREN.AccountingAccountCostCenter accountingAccountCostCenterEntity = (GENERALLEDGEREN.AccountingAccountCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //realizar las operaciones con los GENERALLEDGEREN utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingAccountCostCenterEntity);

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
        /// GetAccountingAccountCostCenter
        /// </summary>
        /// <param name="accountingAccountCostCenterId"></param>
        /// <returns></returns>
        public CostCenter GetAccountingAccountCostCenter(int accountingAccountCostCenterId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccountCostCenter.CreatePrimaryKey(accountingAccountCostCenterId);

                // Realizar las operaciones con los GENERALLEDGEREN utilizando DAF
                GENERALLEDGEREN.AccountingAccountCostCenter accountingAccountCostCenterEntity = (GENERALLEDGEREN.AccountingAccountCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return new CostCenter() { CostCenterId = Convert.ToInt32( accountingAccountCostCenterEntity.CostCenterId )};
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }        

        #endregion

    }
}