//System
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Exceptions;

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AccountingRuleDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns></returns>
        public AccountingRule SaveAccountingRule(AccountingRule accountingRule)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingRule accountingRuleEntity = EntityAssembler.CreateAccountingRule(accountingRule);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingRuleEntity);

                // Return del model
                return ModelAssembler.CreateAccountingRule(accountingRuleEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns>AccountingRule</returns>
        public AccountingRule UpdateAccountingRule(AccountingRule accountingRule)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingRule.CreatePrimaryKey(accountingRule.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingRule accountingRuleEntity = (GENERALLEDGEREN.AccountingRule)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                accountingRuleEntity.Description = accountingRule.Description;
                accountingRuleEntity.ModuleCode = accountingRule.ModuleDateId;
                accountingRuleEntity.Observations = accountingRule.Observation;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingRuleEntity);

                // Return del model
                return ModelAssembler.CreateAccountingRule(accountingRuleEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteConcept
        /// </summary>
        /// <param name="accountingRuleId"></param>
        public void DeleteAccountingRule(int accountingRuleId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingRule.CreatePrimaryKey(accountingRuleId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingRule accountingRuleEntity = (GENERALLEDGEREN.AccountingRule)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingRuleEntity);
            }
            catch (BusinessException ex)            
            {
                
                throw new UnhandledException(ex.ExceptionMessages.ToString());
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetConcept
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns></returns>
        public AccountingRule GetAccountingRule(AccountingRule accountingRule)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingRule.CreatePrimaryKey(accountingRule.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingRule accountingRuleEntity = (GENERALLEDGEREN.AccountingRule)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingRule(accountingRuleEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetConcepts
        /// </summary>
        /// <returns></returns>
        public List<AccountingRule> GetAccountingRules()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingRule)));

                // Return  como Lista
                return ModelAssembler.CreateAccountingRules(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}
