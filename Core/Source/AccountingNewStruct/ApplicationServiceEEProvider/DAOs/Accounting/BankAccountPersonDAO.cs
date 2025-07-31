//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using PERSONMODEL = Sistran.Core.Application.UniquePersonService.V1.Models;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class BankAccountPersonDAO
    {
        #region Constants

        private const int PageSize = 50;

        #endregion

        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountBank
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns>BankAccountPerson</returns>
        public BankAccountPerson SaveBankAccountPerson(BankAccountPerson bankAccountPerson)
        {
            try
            {
                // Convertir de model a entity
                UPEN.AccountBank accountBankEntity = EntityAssembler.CreateBankAccountPerson(bankAccountPerson);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountBankEntity);

                // Return del model
                return ModelAssembler.CreateBankAccountPerson(accountBankEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion 

        #region Update

        /// <summary>
        /// UpdateBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns>BankAccountPerson</returns>
        public BankAccountPerson UpdateBankAccountPerson(BankAccountPerson bankAccountPerson)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = UPEN.AccountBank.CreatePrimaryKey(bankAccountPerson.Id, bankAccountPerson.Individual.IndividualId);

                // Encuentra el objeto en referencia a la llave primaria
                UPEN.AccountBank accountBankEntity = (UPEN.AccountBank)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                accountBankEntity.AccountTypeCode = bankAccountPerson.BankAccountType.Id;
                accountBankEntity.Number = Convert.ToString(bankAccountPerson.Number);
                accountBankEntity.BankCode = bankAccountPerson.Bank.Id;
                accountBankEntity.Enabled = bankAccountPerson.IsEnabled;
                accountBankEntity.Default = bankAccountPerson.IsDefault;
                accountBankEntity.CurrencyCode = bankAccountPerson.Currency.Id;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(accountBankEntity);

                // Return del model
                return ModelAssembler.CreateBankAccountPerson(accountBankEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns></returns>
        public void DeleteBankAccountPerson(BankAccountPerson bankAccountPerson)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = UPEN.AccountBank.CreatePrimaryKey(bankAccountPerson.Id, bankAccountPerson.Individual.IndividualId);

                // Realizar las operaciones con los entities utilizando DAF
                UPEN.AccountBank accountBankEntity = (UPEN.AccountBank)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(accountBankEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetBankAccountPerson
        /// </summary>
        /// <param name="BankAccountPerson"></param>
        /// <returns>BankAccountCompany</returns>
        public BankAccountPerson GetBankAccountPerson(BankAccountPerson BankAccountPerson)
        {
            BankAccountPerson newBankAccountPerson = new BankAccountPerson();

            List<BankAccountPerson> ListbankAccountPersons = new List<BankAccountPerson>();

            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UPEN.AccountBank.Properties.AccountBankCode, BankAccountPerson.Id);

                //UIView personBankAccounts = _dataFacadeManager.GetDataFacade().GetView("PersonBankAccountView",
                //                                criteriaBuilder.GetPredicate(), null, 0, PageSize, null, false, out int rows);
                List<ACCOUNTINGEN.GetPersonBankAccountV> data = _dataFacadeManager.GetDataFacade().List(typeof(ACCOUNTINGEN.GetPersonBankAccountV),
                                            criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetPersonBankAccountV> ().ToList();

                if (data.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetPersonBankAccountV personBankAccount in data)
                    {
                        newBankAccountPerson.Bank = new Bank();                        
                        newBankAccountPerson.Bank.Id = personBankAccount.AccountbankAccountBankCode/*["BankCode"]*/ == 0 ? -1 : Convert.ToInt32(personBankAccount.AccountbankAccountBankCode);
                        newBankAccountPerson.Bank.Description = personBankAccount.BankDescription == null ? "" : Convert.ToString(personBankAccount.BankDescription);
                        newBankAccountPerson.BankAccountType = new BankAccountType();
                        newBankAccountPerson.BankAccountType.Id = personBankAccount.AccountbankAccountTypeCode == 0 ? -1 : Convert.ToInt32(personBankAccount.AccountbankAccountTypeCode);
                        newBankAccountPerson.BankAccountType.Description = personBankAccount.AccounttypeDescription == null ? "" : Convert.ToString(personBankAccount.AccounttypeDescription);
                        newBankAccountPerson.Currency = new Currency();
                        newBankAccountPerson.Currency.Id = Convert.ToInt32(personBankAccount.AccountbankCurrencyCode);
                        newBankAccountPerson.Currency.Description = personBankAccount.CurrencyDescription == null ? "" : Convert.ToString(personBankAccount.CurrencyDescription);
                        newBankAccountPerson.Id = Convert.ToInt32(personBankAccount.AccountbankBankCode);
                        newBankAccountPerson.Individual = new UniquePersonService.V1.Models.Individual();
                        newBankAccountPerson.Individual.IndividualId = personBankAccount.AccountbankIndividualId == 0 ? -1 : Convert.ToInt32(personBankAccount.AccountbankIndividualId);
                        newBankAccountPerson.Individual.FullName = personBankAccount.ListpersonsName == null ? "" : Convert.ToString(personBankAccount.ListpersonsName);
                        newBankAccountPerson.Individual.EconomicActivity = new PERSONMODEL.EconomicActivity();
                        newBankAccountPerson.Individual.EconomicActivity.Description = personBankAccount.ListpersonsDocumentNumber == null ? "" : Convert.ToString(personBankAccount.ListpersonsDocumentNumber);
                        newBankAccountPerson.Number = personBankAccount.AccountbankNumber == null ? "" : Convert.ToString(personBankAccount.AccountbankNumber);
                        newBankAccountPerson.IsDefault = personBankAccount.AccountbankDefault != null && Convert.ToBoolean(personBankAccount.AccountbankDefault);
                        newBankAccountPerson.IsEnabled = personBankAccount.AccountbankEnabled != null && Convert.ToBoolean(personBankAccount.AccountbankEnabled);
                        ListbankAccountPersons.Add(newBankAccountPerson);
                    }
                }
                return ListbankAccountPersons.FirstOrDefault();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountPerson
        /// Obtiene una cuenta bancaria de la compañía
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <param name="type"></param>
        /// <returns>BankAccountPerson</returns>
        public BankAccountPerson GetBankAccountPerson(BankAccountPerson bankAccountPerson, int type)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = UPEN.AccountBank.CreatePrimaryKey(bankAccountPerson.Id, bankAccountPerson.Individual.IndividualId);

                // Realizar las operaciones con los entities utilizando DAF
                UPEN.AccountBank personBankAccountEntity = (UPEN.AccountBank)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateBankAccountPerson(personBankAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPayments
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <returns>List<BankAccountPerson/></returns>
        public List<BankAccountPerson> GetBankAccountPersons()
        {
            try
            {
                List<BankAccountPerson> bankAccountPersons = new List<BankAccountPerson>();

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.GetPersonBankAccountV.Properties.AccountbankAccountBankCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                List<ACCOUNTINGEN.GetPersonBankAccountV> data = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.GetPersonBankAccountV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetPersonBankAccountV>().ToList();
                
                if (data.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetPersonBankAccountV personBankAccount in data)
                    {
                        BankAccountPerson bankAccountPerson = new BankAccountPerson();
                        bankAccountPerson.Bank = new Bank();
                        bankAccountPerson.Id = personBankAccount.AccountbankAccountBankCode == 0 ? -1 : Convert.ToInt32(personBankAccount.AccountbankAccountBankCode);
                        bankAccountPerson.Bank.Id = Convert.ToInt32(personBankAccount.AccountbankBankCode);
                        bankAccountPerson.Bank.Description = personBankAccount.BankDescription == null ? "" : Convert.ToString(personBankAccount.BankDescription);
                        bankAccountPerson.BankAccountType = new BankAccountType();
                        bankAccountPerson.BankAccountType.Id = personBankAccount.AccountbankAccountTypeCode == 0 ? -1 : Convert.ToInt32(personBankAccount.AccountbankAccountTypeCode);
                        bankAccountPerson.BankAccountType.Description = personBankAccount.AccounttypeDescription == null ? "" : Convert.ToString(personBankAccount.AccounttypeDescription);
                        bankAccountPerson.Currency = new Currency();
                        bankAccountPerson.Currency.Id = Convert.ToInt32(personBankAccount.AccountbankCurrencyCode);
                        bankAccountPerson.Currency.Description = personBankAccount.CurrencyDescription == null ? "" : Convert.ToString(personBankAccount.CurrencyDescription);                       
                        bankAccountPerson.Individual = new Individual();
                        bankAccountPerson.Individual.IndividualId = personBankAccount.AccountbankIndividualId == 0 ? -1 : Convert.ToInt32(personBankAccount.AccountbankIndividualId);
                        bankAccountPerson.Individual.FullName = personBankAccount.ListpersonsName == null ? "" : Convert.ToString(personBankAccount.ListpersonsName);
                        bankAccountPerson.Individual.EconomicActivity = new PERSONMODEL.EconomicActivity();
                        bankAccountPerson.Individual.EconomicActivity.Description = personBankAccount.ListpersonsDocumentNumber == null ? "" : Convert.ToString(personBankAccount.ListpersonsDocumentNumber);
                        bankAccountPerson.Number = personBankAccount.AccountbankNumber == null ? "" : Convert.ToString(personBankAccount.AccountbankNumber);
                        bankAccountPerson.IsDefault = personBankAccount.AccountbankDefault != null && Convert.ToBoolean(personBankAccount.AccountbankDefault);
                        bankAccountPerson.IsEnabled = personBankAccount.AccountbankEnabled != null && Convert.ToBoolean(personBankAccount.AccountbankEnabled);
                        bankAccountPersons.Add(bankAccountPerson);
                    }
                }
                

                return bankAccountPersons;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPayments
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List<Payment/></returns>
        public List<BankAccountPerson> GetBankAccountPersons(int type)
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(UPEN.AccountBank)));

                // Return  como Lista
                return ModelAssembler.CreatePersonBankAccounts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
