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
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class AccountingCompanyDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingCompany
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns></returns>
        public AccountingCompany SaveAccountingCompany(AccountingCompany accountingCompany)
        {
            try
            {
                // Convertir de model a entity
                var accountingCompanyEntity = EntityAssembler.CreateAccountingCompany(accountingCompany);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingCompanyEntity);

                // Return del model
                return ModelAssembler.CreateAccountingCompany(accountingCompanyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        #endregion

        #region Update

        /// <summary>
        /// UpdateAccountingCompany
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns></returns>
        public AccountingCompany UpdateAccountingCompany(AccountingCompany accountingCompany)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.AccountingCompany.CreatePrimaryKey(accountingCompany.AccountingCompanyId);

                // Encuentra el objeto en referencia a la llave primaria
                var accountingCompanyEntity = (GENERALLEDGEREN.AccountingCompany)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                accountingCompanyEntity.Description = accountingCompany.Description;
                accountingCompanyEntity.Default = accountingCompany.Default;
                
                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingCompanyEntity);

                // Return del model
                return ModelAssembler.CreateAccountingCompany(accountingCompanyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        } 

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAccountingCompany
        /// </summary>
        /// <param name="accountingCompanyId"></param>
        /// <returns></returns>
        public bool DeleteAccountingCompany(int accountingCompanyId)
        {
            var isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.AccountingCompany.CreatePrimaryKey(accountingCompanyId);

                // Realizar las operaciones con los entities utilizando DAF
                var accountingCompanyEntity = (GENERALLEDGEREN.AccountingCompany)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingCompanyEntity);

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
        /// GetAccountingCompany
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns></returns>
        public AccountingCompany GetAccountingCompany(AccountingCompany accountingCompany)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.AccountingCompany.CreatePrimaryKey(accountingCompany.AccountingCompanyId);

                // Realizar las operaciones con los entities utilizando DAF
                var accountingCompanyEntity = (GENERALLEDGEREN.AccountingCompany)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingCompany(accountingCompanyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingCompanies
        /// </summary>
        /// <returns></returns>
        public List<AccountingCompany> GetAccountingCompanies()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingCompany)));

                // Return como Lista
                return ModelAssembler.CreateAccountingCompanies(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
 
        #endregion
    }
}