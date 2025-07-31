// Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using ReclassificationModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AccountReclassificationDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public ReclassificationModels.AccountReclassification SaveAccountReclassification(ReclassificationModels.AccountReclassification accountReclassification)
        {
            try
            {
                // Se valida que no se ingrese el mismo registro con la clave primaria
                if (!ValidateAccountReclassification(accountReclassification))
                {
                    // Convertir de model a entity
                    GENERALLEDGEREN.Reclassification reclassificationEntity = EntityAssembler.CreateReclassification(accountReclassification);

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(reclassificationEntity);

                    // Return del model
                    return ModelAssembler.CreateAccountReclassification(reclassificationEntity);
                }
                else
                {
                    ReclassificationModels.AccountReclassification newAccountReclassification = new ReclassificationModels.AccountReclassification()
                    {
                        Id = -1,
                        SourceAccountingAccount = new AccountingAccount()
                        {
                            Description = "Ya existe la parametrización para el año: " + accountReclassification.Year + 
                                          ", mes: " + accountReclassification.Month + " y cuenta contable origen: " +
                                          accountReclassification.SourceAccountingAccount.Number
                        }
                    };

                    return newAccountReclassification;
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns></returns>
        public ReclassificationModels.AccountReclassification UpdateAccountReclassification(ReclassificationModels.AccountReclassification accountReclassification)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Reclassification.CreatePrimaryKey(
                                                                        accountReclassification.Year, 
                                                                        accountReclassification.Month,
                                                                        accountReclassification.SourceAccountingAccount.AccountingAccountId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.Reclassification reclassificationEntity = (GENERALLEDGEREN.Reclassification)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                reclassificationEntity.DestinationAccountingAccountId = accountReclassification.DestinationAccountingAccount.AccountingAccountId;
                reclassificationEntity.OpeningBranch = accountReclassification.OpeningBranch;
                reclassificationEntity.OpeningPrefix = accountReclassification.OpeningPrefix;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(reclassificationEntity);

                //return del model
                return ModelAssembler.CreateAccountReclassification(reclassificationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns></returns>
        public bool DeleteAccountReclassification(ReclassificationModels.AccountReclassification accountReclassification)
        {
            var isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Reclassification.CreatePrimaryKey(
                                                                        accountReclassification.Year,
                                                                        accountReclassification.Month,
                                                                        accountReclassification.SourceAccountingAccount.AccountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Reclassification reclassificationEntity = (GENERALLEDGEREN.Reclassification)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(reclassificationEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetAccountReclassificationByMonthAndYear
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        public List<ReclassificationModels.AccountReclassification> GetAccountReclassificationByMonthAndYear(int month, int year)
        {
            try
            {
                var accounts = GetAccountReclassifications();
                List<ReclassificationModels.AccountReclassification> accountReclassifications = new List<ReclassificationModels.AccountReclassification>();
                List<ReclassificationModels.AccountReclassification> reclassifications = (from ReclassificationModels.AccountReclassification item in accounts
                                                                          where item.Year == year && item.Month == month
                                                                          select item).ToList();

                foreach(ReclassificationModels.AccountReclassification accountReclassification in reclassifications)
                {
                    accountReclassification.DestinationAccountingAccount.Description = GetAccountingAccountsById(accountReclassification.DestinationAccountingAccount.AccountingAccountId)[0].Description;
                    accountReclassification.DestinationAccountingAccount.Number = GetAccountingAccountsById(accountReclassification.DestinationAccountingAccount.AccountingAccountId)[0].Number;
                    accountReclassification.SourceAccountingAccount.Description = GetAccountingAccountsById(accountReclassification.SourceAccountingAccount.AccountingAccountId)[0].Description;
                    accountReclassification.SourceAccountingAccount.Number = GetAccountingAccountsById(accountReclassification.SourceAccountingAccount.AccountingAccountId)[0].Number;

                    accountReclassifications.Add(accountReclassification);
                }

                return accountReclassifications;
            }
            catch
            {
                return new List<ReclassificationModels.AccountReclassification>();
            }
        }

        /// <summary>
        /// GetAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns></returns>
        public ReclassificationModels.AccountReclassification GetAccountReclassification(ReclassificationModels.AccountReclassification accountReclassification)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Reclassification.CreatePrimaryKey( 
                                                                        accountReclassification.Year,
                                                                        accountReclassification.Month, 
                                                                        accountReclassification.SourceAccountingAccount.AccountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Reclassification reclassificationEntity = (GENERALLEDGEREN.Reclassification)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountReclassification(reclassificationEntity);
            }
            catch
            {
                return new ReclassificationModels.AccountReclassification();
            }
        }

        /// <summary>
        /// GetAccountingAccounts
        /// </summary>
        /// <returns></returns>
        public List<ReclassificationModels.AccountReclassification> GetAccountReclassifications()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Reclassification)));
                
                // Return como Lista
                return ModelAssembler.CreateAccountReclassifications(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccountsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<AccountingAccount> GetAccountingAccountsById(int id)
        {
            List<AccountingAccount> filteredAccountingAccounts;

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(id + "%");

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                List<AccountingAccount> accountingAccounts = new List<AccountingAccount>();

                foreach (GENERALLEDGEREN.AccountingAccount accountingAccountEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                {
                    Branch branch = new Branch();
                    branch.Id = Convert.ToInt32(accountingAccountEntity.DefaultBranchCode);

                    Currency currency = new Currency();
                    currency.Id = Convert.ToInt32(accountingAccountEntity.DefaultCurrencyCode);

                    Analysis analysis = new Analysis();
                    analysis.AnalysisId = Convert.ToInt32(accountingAccountEntity.AnalysisId);

                    accountingAccounts.Add(new AccountingAccount
                    {
                        AccountingAccountId = accountingAccountEntity.AccountingAccountId,
                        Number = accountingAccountEntity.AccountNumber,
                        Description = accountingAccountEntity.AccountName,
                        Branch = branch,
                        Currency = currency,
                        AccountingNature = (AccountingNatures)accountingAccountEntity.AccountingNature
                    });
                }

                filteredAccountingAccounts = accountingAccounts.Count > 10 ? accountingAccounts.GetRange(0, 10) : accountingAccounts;
            }
            catch
            {
                filteredAccountingAccounts = new List<AccountingAccount>();
            }

            return filteredAccountingAccounts;
        }

        /// <summary>
        /// ValidateAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns></returns>
        private bool ValidateAccountReclassification(ReclassificationModels.AccountReclassification accountReclassification)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Reclassification.CreatePrimaryKey(
                                                                        accountReclassification.Year,
                                                                        accountReclassification.Month, 
                                                                        accountReclassification.SourceAccountingAccount.AccountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Reclassification reclassificationEntity = (GENERALLEDGEREN.Reclassification)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                var account = ModelAssembler.CreateAccountReclassification(reclassificationEntity);

                if(account.Year == accountReclassification.Year && account.Month == accountReclassification.Month
                    && account.SourceAccountingAccount.AccountingAccountId == accountReclassification.SourceAccountingAccount.AccountingAccountId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion Get

        #region Validation

        /// <summary>
        /// ValidateEntryAnalysis
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <returns></returns>
        public bool ValidateEntryAnalysis(int entryItemId)
        {
            try
            {
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.EntryItemId, entryItemId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
