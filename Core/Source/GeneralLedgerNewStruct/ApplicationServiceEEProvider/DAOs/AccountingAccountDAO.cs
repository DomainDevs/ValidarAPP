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
    internal class AccountingAccountDAO
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
        public AccountingAccount SaveAccountingAccount(AccountingAccount accountingAccount)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingAccount accountingAccountEntity = EntityAssembler.CreateAccountingAccount(accountingAccount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingAccountEntity);

                // Return del model
                return ModelAssembler.CreateAccountingAccount(accountingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public AccountingAccount UpdateAccountingAccount(AccountingAccount accountingAccount)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccount.CreatePrimaryKey(accountingAccount.AccountingAccountId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingAccount accountingAccountEntity = (GENERALLEDGEREN.AccountingAccount)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                int? currencyId = null;
                int? analysisId = null;
                int? branchId = null;

                if (accountingAccount.Currency.Id > -1)
                {
                    currencyId = accountingAccount.Currency.Id;
                }
                if (accountingAccount.Analysis.AnalysisId > 0)
                {
                    analysisId = accountingAccount.Analysis.AnalysisId;
                }
                if (accountingAccount.Branch.Id > 0)
                {
                    branchId = accountingAccount.Branch.Id;
                }

                accountingAccountEntity.AccountNumber = accountingAccount.Number;
                accountingAccountEntity.AccountName = accountingAccount.Description;
                accountingAccountEntity.AccountingNature = Convert.ToInt32(accountingAccount.AccountingNature);
                accountingAccountEntity.AccountingAccountParentId = accountingAccount.AccountingAccountParentId;
                accountingAccountEntity.DefaultCurrencyCode = currencyId;
                accountingAccountEntity.DefaultBranchCode = branchId;
                accountingAccountEntity.RequireCostCenter = accountingAccount.RequiresCostCenter;
                accountingAccountEntity.RequireAnalysis = accountingAccount.RequiresAnalysis;
                accountingAccountEntity.AnalysisId = analysisId;
                accountingAccountEntity.AccountTypeId = Convert.ToInt32(accountingAccount.AccountingAccountType.Id);
                accountingAccountEntity.AccountApplication = Convert.ToInt32(accountingAccount.AccountingAccountApplication);
                accountingAccountEntity.IsOfficialNomenclature = (accountingAccount.AccountingAccountApplication == AccountingAccountApplications.Others); //Cuando la aplicación es "Otros" se habilita el campo "Nomeclatura Oficial"
                accountingAccountEntity.Comments = accountingAccount.Comments;
                accountingAccountEntity.IsReclassify = accountingAccount.IsReclassify;
                accountingAccountEntity.RecAccounting = accountingAccount.RecAccounting;
                accountingAccountEntity.IsRevalue = accountingAccount.IsRevalue;
                accountingAccountEntity.RevAcountingPos = accountingAccount.RevAcountingPos;
                accountingAccountEntity.RevAcountingNeg = accountingAccount.RevAcountingNeg;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountingAccountEntity);

                // Return del model
                return ModelAssembler.CreateAccountingAccount(accountingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAccountingAccount
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        public bool DeleteAccountingAccount(int accountingAccountId)
        {
            var isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccount.CreatePrimaryKey(accountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccount accountingAccountEntity = (GENERALLEDGEREN.AccountingAccount)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountingAccountEntity);

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
        /// GetAccountingAccount
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        public AccountingAccount GetAccountingAccount(int accountingAccountId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingAccount.CreatePrimaryKey(accountingAccountId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingAccount accountingAccountEntity = (GENERALLEDGEREN.AccountingAccount)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingAccount(accountingAccountEntity);
            }
            catch
            {
                return new AccountingAccount();
            }
        }

        /// <summary>
        /// GetAccountingAccounts
        /// </summary>
        /// <returns></returns>
        public List<AccountingAccount> GetAccountingAccounts()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccount)));

                // Return  como Lista
                return ModelAssembler.CreateAccountingAccounts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}