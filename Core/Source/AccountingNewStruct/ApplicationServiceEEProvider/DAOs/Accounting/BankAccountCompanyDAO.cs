using System;
using System.Collections.Generic;
using System.Data;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

//Sitran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class BankAccountCompanyDAO
    {
        private const int PageSize = 250;

        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Get

        /// <summary>
        /// GetBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>BankAccountCompany</returns>
        public BankAccountCompany GetBankAccountCompany(BankAccountCompany bankAccountCompany)
        {
            BankAccountCompany newBankAccountCompany = new BankAccountCompany();

            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId, bankAccountCompany.Id);

                UIView companyBankAccounts = _dataFacadeManager.GetDataFacade().GetView("CompanyBankAccountView", criteriaBuilder.GetPredicate(), null, 0, PageSize, null, false, out int rows);

                newBankAccountCompany.Id = Convert.ToInt32(companyBankAccounts.Rows[0]["BankAccountCompanyId"]);
                newBankAccountCompany.BankAccountType = new BankAccountType();
                newBankAccountCompany.BankAccountType.Id = companyBankAccounts.Rows[0]["AccountTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccounts.Rows[0]["AccountTypeCode"]);
                newBankAccountCompany.BankAccountType.Description = companyBankAccounts.Rows[0]["AccountTypeDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccounts.Rows[0]["AccountTypeDescription"]);
                newBankAccountCompany.Number = companyBankAccounts.Rows[0]["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(companyBankAccounts.Rows[0]["AccountNumber"]);
                newBankAccountCompany.Bank = new Bank();
                newBankAccountCompany.Bank.Id = companyBankAccounts.Rows[0]["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccounts.Rows[0]["BankCode"]);
                newBankAccountCompany.Bank.Description = companyBankAccounts.Rows[0]["BankDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccounts.Rows[0]["BankDescription"]);
                newBankAccountCompany.IsEnabled = companyBankAccounts.Rows[0]["Enabled"] != DBNull.Value && Convert.ToBoolean(companyBankAccounts.Rows[0]["Enabled"]);
                newBankAccountCompany.Currency = new Currency();
                newBankAccountCompany.Currency.Id = companyBankAccounts.Rows[0]["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccounts.Rows[0]["CurrencyCode"]);
                newBankAccountCompany.Currency.Description = companyBankAccounts.Rows[0]["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccounts.Rows[0]["CurrencyDescription"]);
                newBankAccountCompany.AccountingAccount = new GeneralLedgerServices.EEProvider.Models.AccountingAccount();
                newBankAccountCompany.AccountingAccount.AccountingAccountId = companyBankAccounts.Rows[0]["AccountingAccountId"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccounts.Rows[0]["AccountingAccountId"]); //TODO ACE-650 Falta campo AccountingAccountId en el modelo
                newBankAccountCompany.DisableDate = Convert.ToDateTime(companyBankAccounts.Rows[0]["DisabledDate"] == DBNull.Value ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(companyBankAccounts.Rows[0]["DisabledDate"]));
                newBankAccountCompany.Branch = new Branch();
                newBankAccountCompany.Branch.Id = companyBankAccounts.Rows[0]["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccounts.Rows[0]["BranchCode"]);
                newBankAccountCompany.Branch.Description = companyBankAccounts.Rows[0]["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccounts.Rows[0]["BranchDescription"]);

                return newBankAccountCompany;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountCompany
        /// Obtiene una cuenta bancaria de la compañía
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="type"></param>
        /// <returns>BankAccountCompany</returns>
        public BankAccountCompany GetBankAccountCompany(BankAccountCompany bankAccountCompany, int type)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankAccountCompany.CreatePrimaryKey(bankAccountCompany.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankAccountCompany bankAccountCompanyEntity = (ACCOUNTINGEN.BankAccountCompany)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCompanyBankAccount(bankAccountCompanyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankAccountCompanies
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <returns>List<BankAccountCompany/></returns>
        public List<BankAccountCompany> GetBankAccountCompanies()
        {
            try
            {
                List<BankAccountCompany> bankAccountCompanies = new List<BankAccountCompany>();

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                UIView companyBankAccounts = _dataFacadeManager.GetDataFacade().GetView("CompanyBankAccountView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                foreach (DataRow companyBankAccount in companyBankAccounts)
                {
                    BankAccountCompany bankAccountCompany = new BankAccountCompany();
                    bankAccountCompany.Id = Convert.ToInt32(companyBankAccount["BankAccountCompanyId"]);
                    bankAccountCompany.BankAccountType = new BankAccountType();
                    bankAccountCompany.BankAccountType.Id = companyBankAccount["AccountTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccount["AccountTypeCode"]);
                    bankAccountCompany.BankAccountType.Description = companyBankAccount["AccountTypeDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountTypeDescription"]);
                    bankAccountCompany.Number = companyBankAccount["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountNumber"]);
                    bankAccountCompany.Bank = new Bank();
                    bankAccountCompany.Bank.Id = Convert.ToInt32(companyBankAccount["BankCode"]); //NO DEVOLVER -1 PORQUE EL 0 TAMBIEN ES UN BANCO
                    bankAccountCompany.Bank.Description = companyBankAccount["BankDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["BankDescription"]);
                    bankAccountCompany.IsEnabled = companyBankAccount["Enabled"] != DBNull.Value && Convert.ToBoolean(companyBankAccount["Enabled"]);
                    bankAccountCompany.Currency = new Currency();
                    bankAccountCompany.Currency.Id = Convert.ToInt32(companyBankAccount["CurrencyCode"]); //NO DEVOLVER -1 PORQUE EL 0 TAMBIEN ES UN TIPO DE MONEDA
                    bankAccountCompany.Currency.Description = companyBankAccount["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["CurrencyDescription"]);
                    bankAccountCompany.AccountingAccount = new GeneralLedgerServices.EEProvider.Models.AccountingAccount();
                    bankAccountCompany.AccountingAccount.AccountingAccountId = companyBankAccount["AccountingAccountId"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccount["AccountingAccountId"]);
                    bankAccountCompany.AccountingAccount.Number = companyBankAccount["AccountingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountingAccountNumber"]);
                    bankAccountCompany.AccountingAccount.Description = companyBankAccount["AccountName"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountName"]);
                    bankAccountCompany.DisableDate = companyBankAccount["DisabledDate"] == DBNull.Value ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(companyBankAccount["DisabledDate"]);
                    bankAccountCompany.Branch = new Branch();
                    bankAccountCompany.Branch.Id = companyBankAccount["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccount["BranchCode"]);
                    bankAccountCompany.Branch.Description = companyBankAccount["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["BranchDescription"]);

                    bankAccountCompanies.Add(bankAccountCompany);
                }


                return bankAccountCompanies;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankAccountCompaniesByCurrencyCode
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <returns>List<BankAccountCompany/></returns>
        public List<BankAccountCompany> GetBankAccountCompaniesByCurrencyCode(int currencyCode)
        {
            try
            {
                List<BankAccountCompany> bankAccountCompanies = new List<BankAccountCompany>();

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(currencyCode);


                UIView companyBankAccounts = _dataFacadeManager.GetDataFacade().GetView("CompanyBankAccountView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                foreach (DataRow companyBankAccount in companyBankAccounts)
                {
                    BankAccountCompany bankAccountCompany = new BankAccountCompany();
                    bankAccountCompany.Id = Convert.ToInt32(companyBankAccount["BankAccountCompanyId"]);
                    bankAccountCompany.BankAccountType = new BankAccountType();
                    bankAccountCompany.BankAccountType.Id = companyBankAccount["AccountTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccount["AccountTypeCode"]);
                    bankAccountCompany.BankAccountType.Description = companyBankAccount["AccountTypeDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountTypeDescription"]);
                    bankAccountCompany.Number = companyBankAccount["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountNumber"]);
                    bankAccountCompany.Bank = new Bank();
                    bankAccountCompany.Bank.Id = Convert.ToInt32(companyBankAccount["BankCode"]); //NO DEVOLVER -1 PORQUE EL 0 TAMBIEN ES UN BANCO
                    bankAccountCompany.Bank.Description = companyBankAccount["BankDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["BankDescription"]);
                    bankAccountCompany.IsEnabled = companyBankAccount["Enabled"] != DBNull.Value && Convert.ToBoolean(companyBankAccount["Enabled"]);
                    bankAccountCompany.Currency = new Currency();
                    bankAccountCompany.Currency.Id = Convert.ToInt32(companyBankAccount["CurrencyCode"]); //NO DEVOLVER -1 PORQUE EL 0 TAMBIEN ES UN TIPO DE MONEDA
                    bankAccountCompany.Currency.Description = companyBankAccount["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["CurrencyDescription"]);
                    bankAccountCompany.AccountingAccount = new GeneralLedgerServices.EEProvider.Models.AccountingAccount();
                    bankAccountCompany.AccountingAccount.AccountingAccountId = companyBankAccount["AccountingAccountId"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccount["AccountingAccountId"]);
                    bankAccountCompany.AccountingAccount.Number = companyBankAccount["AccountingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountingAccountNumber"]);
                    bankAccountCompany.AccountingAccount.Description = companyBankAccount["AccountName"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["AccountName"]);
                    bankAccountCompany.DisableDate = companyBankAccount["DisabledDate"] == DBNull.Value ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(companyBankAccount["DisabledDate"]);
                    bankAccountCompany.Branch = new Branch();
                    bankAccountCompany.Branch.Id = companyBankAccount["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(companyBankAccount["BranchCode"]);
                    bankAccountCompany.Branch.Description = companyBankAccount["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(companyBankAccount["BranchDescription"]);

                    bankAccountCompanies.Add(bankAccountCompany);
                }


                return bankAccountCompanies;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankAccountsCompanies
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List<BankAccountCompany/></returns>
        public List<BankAccountCompany> GetBankAccountsCompanies(int type)
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.BankAccountCompany)));

                // Return  como Lista
                return ModelAssembler.CreateCompanyBankAccounts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion



        /// <summary>
        /// SaveBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>int</returns>
        public int SaveBankAccountCompany(BankAccountCompany bankAccountCompany)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.BankAccountCompany bankAccountCompanyEntity = EntityAssembler.CreateBankAccountCompany(bankAccountCompany, "");

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankAccountCompanyEntity);

                // Return del model
                return bankAccountCompanyEntity.BankAccountCompanyId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// UpdateBankAccountCompany
        /// bcardenas
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>int</returns>
        public int UpdateBankAccountCompany(BankAccountCompany bankAccountCompany)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankAccountCompany.CreatePrimaryKey(bankAccountCompany.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.BankAccountCompany bankAccountCompanyEntity = (ACCOUNTINGEN.BankAccountCompany)
                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankAccountCompanyEntity.Enabled = bankAccountCompany.IsEnabled;

                if (bankAccountCompany.DisableDate == Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    bankAccountCompanyEntity.DisabledDate = null;
                }
                else
                {
                    bankAccountCompanyEntity.DisabledDate = bankAccountCompany.DisableDate;
                }

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankAccountCompanyEntity);

                // Return del model
                return bankAccountCompanyEntity.BankAccountCompanyId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// DeleteBankAccountCompany
        /// Elimina una cuenta bancaria de la compañía
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        public void DeleteBankAccountCompany(BankAccountCompany bankAccountCompany)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankAccountCompany.CreatePrimaryKey(bankAccountCompany.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankAccountCompany bankAccountCompanyEntity = (ACCOUNTINGEN.BankAccountCompany)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(bankAccountCompanyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<int> GetCurrencyCodes()
        {
            List<int> currencies = new List<int>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.Enabled, true);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(
                new Column(ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode, typeof(ACCOUNTINGEN.BankAccountCompany).Name),
                ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode));
            selectQuery.Table = new ClassNameTable(typeof(ACCOUNTINGEN.BankAccountCompany), typeof(ACCOUNTINGEN.BankAccountCompany).Name);
            selectQuery.Distinct = true;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            int currency;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    if (reader[ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode] != null)
                    {
                        currency = Convert.ToInt32(reader[ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode]);
                        if (!currencies.Contains(currency))
                            currencies.Add(currency);
                    }
                }
            }
            return currencies;
        }

        public List<int> GetBankCodesByCurrencyId(int currencyId)
        {
            List<int> banks = new List<int>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode, currencyId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.Enabled, true);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(
                new Column(ACCOUNTINGEN.BankAccountCompany.Properties.BankCode, typeof(ACCOUNTINGEN.BankAccountCompany).Name),
                ACCOUNTINGEN.BankAccountCompany.Properties.BankCode));
            selectQuery.Table = new ClassNameTable(typeof(ACCOUNTINGEN.BankAccountCompany), typeof(ACCOUNTINGEN.BankAccountCompany).Name);
            selectQuery.Distinct = true;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            int bank;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    if (reader[ACCOUNTINGEN.BankAccountCompany.Properties.BankCode] != null)
                    {
                        bank = Convert.ToInt32(reader[ACCOUNTINGEN.BankAccountCompany.Properties.BankCode]);
                        if (!banks.Contains(bank))
                            banks.Add(bank);
                    }
                }
            }
            return banks;
        }

        public List<BankAccountCompany> GetBankAccountsByCurrencyIdBankId(int currencyId, int bankId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode, currencyId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.BankCode, bankId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.Enabled, true);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(
                new Column(ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId, typeof(ACCOUNTINGEN.BankAccountCompany).Name),
                ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId));
            selectQuery.AddSelectValue(new SelectValue(
                new Column(ACCOUNTINGEN.BankAccountCompany.Properties.AccountNumber, typeof(ACCOUNTINGEN.BankAccountCompany).Name),
                ACCOUNTINGEN.BankAccountCompany.Properties.AccountNumber));
            selectQuery.Table = new ClassNameTable(typeof(ACCOUNTINGEN.BankAccountCompany), typeof(ACCOUNTINGEN.BankAccountCompany).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.Distinct = true;

            List<BankAccountCompany> bankAccountCompanies = new List<BankAccountCompany>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    bankAccountCompanies.Add(new BankAccountCompany()
                    {
                        Id = Convert.ToInt32(reader[ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId]),
                        Number = Convert.ToString(reader[ACCOUNTINGEN.BankAccountCompany.Properties.AccountNumber])
                    });
                }
            }
            return bankAccountCompanies;
        }
    }
}
