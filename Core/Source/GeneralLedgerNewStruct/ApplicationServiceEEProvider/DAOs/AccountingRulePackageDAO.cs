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
    internal class AccountingRulePackageDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveEntry
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns></returns>
        public Models.AccountingRules.AccountingRulePackage SaveAccountingRulePackage(Models.AccountingRules.AccountingRulePackage accountingRulePackage)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingRulePackage entityAccountingRulePackage = EntityAssembler.CreateAccountingRulePackage(accountingRulePackage);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entityAccountingRulePackage);

                // Return del model
                return ModelAssembler.CreateAccountingRulePackage(entityAccountingRulePackage);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateEntry
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns></returns>
        public Models.AccountingRules.AccountingRulePackage UpdateAccountingRulePackage(Models.AccountingRules.AccountingRulePackage accountingRulePackage)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingRulePackage.CreatePrimaryKey(accountingRulePackage.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingRulePackage entityAccountingRulePackage = (GENERALLEDGEREN.AccountingRulePackage)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entityAccountingRulePackage.Description = accountingRulePackage.Description;
                entityAccountingRulePackage.ModuleCode = accountingRulePackage.ModuleDateId;
                entityAccountingRulePackage.RulePackageCode = accountingRulePackage.RulePackageId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entityAccountingRulePackage);

                // Return del model
                return ModelAssembler.CreateAccountingRulePackage(entityAccountingRulePackage);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteEntry
        /// </summary>
        /// <param name="accountingRulePackageId"></param>
        public void DeleteAccountingRulePackage(int accountingRulePackageId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingRulePackage.CreatePrimaryKey(accountingRulePackageId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingRulePackage accountingRulePackageEntity = (GENERALLEDGEREN.AccountingRulePackage)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingRulePackageEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetAccountingRulePackage
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns></returns>
        public Models.AccountingRules.AccountingRulePackage GetAccountingRulePackage(Models.AccountingRules.AccountingRulePackage accountingRulePackage)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingRulePackage.CreatePrimaryKey(accountingRulePackage.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingRulePackage entityAccountingRulePackage = (GENERALLEDGEREN.AccountingRulePackage)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingRulePackage(entityAccountingRulePackage);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingRulePackages
        /// </summary>
        /// <returns></returns>
        public List<Models.AccountingRules.AccountingRulePackage> GetAccountingRulePackages()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingRulePackage)));

                // Return como Lista
                return ModelAssembler.CreateAccountingRulePackages(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}
