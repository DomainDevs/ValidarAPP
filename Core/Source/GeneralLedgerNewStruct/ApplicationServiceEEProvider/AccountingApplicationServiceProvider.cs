using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Views;

//Sitran Core
using CommonModels = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.Rules;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.GeneralLedgerServices.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountReclassification;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.Utilities.Helper;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Business;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs.Integration2G;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider
{
    public class AccountingApplicationServiceProvider : IAccountingApplicationService
    {
        #region Instance Variables
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance Variables

        #region Public Methods

        //ConfigurationManager.AppSettings["ModuleDateAccounting"]

        #region GL

        #region AccountingAccount

        /// <summary>
        /// Guarda una cuenta contable
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>GeneralLedgerModels.AccountingAccount</returns>
        public AccountingAccountDTO SaveAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            try
            {
                AccountingAccountPrefixDAO accountingAccountPrefixDAO = new AccountingAccountPrefixDAO();
                AccountingAccountCostCenterDAO accountingAccountCostCenterDAO = new AccountingAccountCostCenterDAO();
                AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
                AccountingAccountDTO accountingAccountDTO = accountingAccountDAO.SaveAccountingAccount(accountingAccount.ToModel()).ToDTO();

                accountingAccount.AccountingAccountId = accountingAccountDTO.AccountingAccountId;
                AccountingAccount accountingAccountModel = accountingAccount.ToModel();
                // Se graba centro de costos y ramos asociados
                accountingAccount.AccountingAccountId = accountingAccountCostCenterDAO.UpdateAccountingCostCenterByAccountingAccount(accountingAccountModel);

                accountingAccount.AccountingAccountId = accountingAccountPrefixDAO.UpdateAccountingPrefixByAccountingAccount(accountingAccountModel);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingAccount;
        }

        /// <summary>
        /// UpdateAccountingAccount
        /// Actualiza una cuenta contable
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>AccountingAccount</returns>
        public AccountingAccountDTO UpdateAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            try
            {
                AccountingAccountPrefixDAO accountingAccountPrefixDAO = new AccountingAccountPrefixDAO();
                AccountingAccountCostCenterDAO accountingAccountCostCenterDAO = new AccountingAccountCostCenterDAO();
                AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
                // Se graba centro de costos y ramos asociados
                accountingAccount.AccountingAccountId = accountingAccountCostCenterDAO.UpdateAccountingCostCenterByAccountingAccount(ModelDTOAssembler.ToModel(accountingAccount));
                accountingAccount.AccountingAccountId = accountingAccountPrefixDAO.UpdateAccountingPrefixByAccountingAccount(ModelDTOAssembler.ToModel(accountingAccount));

                // Se actualiza la cuenta contable
                accountingAccount = DTOAssembler.ToDTO(accountingAccountDAO.UpdateAccountingAccount(ModelDTOAssembler.ToModel(accountingAccount)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingAccount;
        }

        /// <summary>
        /// DeleteAccountingAccount
        /// Borra una cuenta contable
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>bool</returns>
        public bool DeleteAccountingAccount(int accountingAccountId)
        {
            AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
            bool deleted = false;
            AccountingAccountDTO accountingAccount = new AccountingAccountDTO();
            DTOAssembler.ToDTO(ModelDTOAssembler.ToModel(accountingAccount));
            accountingAccount = GetAccountingAccount(accountingAccountId);

            try
            {
                if (DeleteCostCentersByAccountingAccount(accountingAccount) &&
                    DeletePrefixesByAccountingAccount(accountingAccount))
                {
                    deleted = accountingAccountDAO.DeleteAccountingAccount(accountingAccountId);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return deleted;
        }

        /// <summary>
        /// GetAccountingAccount
        /// Obtiene datos de una cuenta contable
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>AccountingAccount</returns>
        public AccountingAccountDTO GetAccountingAccount(int accountingAccountId)
        {
            AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
            AccountingAccountDTO accountingAccount = new AccountingAccountDTO();
            accountingAccount = accountingAccountDAO.GetAccountingAccount(accountingAccountId).ToDTO();

            accountingAccount = GetCostCentersByAccountingAccount(accountingAccount);
            accountingAccount = GetPrefixesByAccountingAccount(accountingAccount);
            return accountingAccount;
        }

        /// <summary>
        /// GetAccountingAccounts
        /// Obtiene todas las cuentas contables
        /// </summary>
        /// <returns>List<GeneralLedgerModels.AccountingAccount></returns>
        public List<AccountingAccountDTO> GetAccountingAccounts()
        {
            AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
            return DTOAssembler.ToDTOs(accountingAccountDAO.GetAccountingAccounts()).ToList();
        }

        /// <summary>
        /// GetAccountingAccountsByNumberDescription
        /// Obtiene una cuenta contable haciendo uso de su número de cuenta o descripción
        /// <param name="accountingAccount"></param>
        /// </summary>
        /// <returns>List<GeneralLedgerModels.AccountingAccount></returns>
        public List<AccountingAccountDTO> GetAccountingAccountsByNumberDescription(AccountingAccountDTO accountingAccount)
        {
            List<AccountingAccountDTO> filteredAccountingAccounts;

            try
            {
                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                if (!string.IsNullOrEmpty(accountingAccount.Number))
                {
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccount.Number + "%");
                }

                if (!string.IsNullOrEmpty(accountingAccount.Description))
                {
                    criteriaBuilder.Or();
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountName);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccount.Description);
                }

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                List<AccountingAccountDTO> accountingAccounts = new List<AccountingAccountDTO>();

                foreach (GENERALLEDGEREN.AccountingAccount accountingAccountEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                {
                    BranchDTO branch = new BranchDTO();
                    branch.Id = Convert.ToInt32(accountingAccountEntity.DefaultBranchCode);

                    CurrencyDTO currency = new CurrencyDTO();
                    currency.Id = Convert.ToInt32(accountingAccountEntity.DefaultCurrencyCode);

                    AnalysisDTO analysis = new AnalysisDTO();
                    analysis.AnalysisId = Convert.ToInt32(accountingAccountEntity.AnalysisId);

                    accountingAccounts.Add(new AccountingAccountDTO
                    {
                        AccountingAccountId = accountingAccountEntity.AccountingAccountId,
                        Number = accountingAccountEntity.AccountNumber,
                        Description = accountingAccountEntity.AccountName,
                        Branch = branch,
                        Currency = currency,
                        AccountingNature = Convert.ToInt32(accountingAccountEntity.AccountingNature),
                        AccountingAccountParentId = (int)accountingAccountEntity.AccountingAccountParentId,
                    });
                }

                filteredAccountingAccounts = accountingAccounts.Count > 10 ? accountingAccounts.GetRange(0, 10) : accountingAccounts;
            }
            catch
            {
                filteredAccountingAccounts = new List<AccountingAccountDTO>();
            }

            return filteredAccountingAccounts;
        }

        /// <summary>
        /// HasChildren
        /// Verifica si la cuenta contable tiene cuentas hijas
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>bool</returns>
        public bool HasChildren(int accountingAccountId)
        {
            bool isChildren = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountParentId, accountingAccountId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    isChildren = true;
                }
            }
            catch
            {
                isChildren = false;
            }

            return isChildren;
        }

        /// <summary>
        /// OnEntry
        /// Función que comprueba que la cuenta no está siendo usada en asientos de diario o de mayor
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>bool</returns>
        public bool OnEntry(int accountingAccountId)
        {
            bool isEntry = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.AccountingAccountId, accountingAccountId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    isEntry = true;
                }
                else
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.AccountingAccountId, accountingAccountId);
                    BusinessCollection businessCollectionEntry = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntryItem), criteriaBuilder.GetPredicate()));

                    if (businessCollectionEntry.Count > 0)
                    {
                        isEntry = true;
                    }
                }
            }
            catch
            {
                isEntry = false;
            }

            return isEntry;
        }

        /// <summary>
        /// ValidateAccountingAccount
        /// Método para validar número de Cuenta
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <param name="edit"></param>
        /// <returns>AccountingAccountValidationDTO</returns>
        public AccountingAccountValidationDTO ValidateAccountingAccount(AccountingAccountDTO accountingAccount, int edit)
        {
            AccountingAccountValidationDTO validationDTO = new AccountingAccountValidationDTO { IsSucessful = false, TypeId = 100 };
            accountingAccount.Description = String.Empty;
            var lengthLevel = 0;

            if (edit == 0)
            {
                validationDTO = ValidateAccountingAccountNumberDoesNotExist(accountingAccount.Number);

                if (!validationDTO.IsSucessful)
                {
                    return validationDTO;
                }
                else
                {
                    // Compruebo que la cuenta exista, quitándole los ceros.
                    AccoutingAccountLevelDTO accountLevelDTO = GetAccountingAccountLevel(accountingAccount);
                    accountingAccount.Number = accountingAccount.Number.Substring(0, accountLevelDTO.Length);
                    lengthLevel = accountLevelDTO.Length;
                }
            }
            if (edit == 1)
            {
                try
                {
                    AccoutingAccountLevelDTO accountLevelDTO = GetAccountingAccountLevel(accountingAccount);
                    accountingAccount.Number = accountingAccount.Number.Substring(0, accountLevelDTO.Length);
                    lengthLevel = accountLevelDTO.Length;
                }
                catch (BusinessException)
                {
                    validationDTO.IsSucessful = false;
                }
            }

            try
            {
                AccountingAccountDTO parentAccount;

                // Obtengo el nivel y longitud de la cuenta padre
                if (accountingAccount.AccountingAccountParentId >= 10)
                {
                    parentAccount = GetAccountingAccount(accountingAccount.AccountingAccountParentId);

                    //se elimina los datos de sucursal y ramo en el número de cuenta padre para realizar la validación
                    parentAccount = AssembleParentAccountForValidation(parentAccount);
                }
                else
                {
                    parentAccount = GetAccountingAccountParent(accountingAccount.AccountingAccountParentId);
                }

                AccoutingAccountLevelDTO accountingAccountLevelParent = GetAccountingAccountLevel(parentAccount);
                AccoutingAccountLevelDTO accountingAccountLevel = GetAccountingAccountLevel(accountingAccount);

                validationDTO = ValidateAccountingAccountBaseNumber(accountingAccount, accountingAccountLevelParent, accountingAccountLevel, lengthLevel);

                if (validationDTO.IsSucessful)
                {
                    string accountParentNumber = "";
                    string accountNumber = "";

                    accountParentNumber = parentAccount.Number.Substring(0, accountingAccountLevelParent.Length);
                    accountNumber = accountingAccount.Number.Substring(0, accountingAccountLevelParent.Length);

                    if (accountParentNumber.Trim() != accountNumber.Trim())
                    {
                        validationDTO.TypeId = 2;
                        validationDTO.IsSucessful = false;
                    }
                }
            }
            catch (BusinessException)
            {
                validationDTO.IsSucessful = false;
                validationDTO.TypeId = 5; // No existe cuenta Padre
            }

            return validationDTO;
        }

        /// <summary>
        /// GetAccountingAccountsByParentId
        /// </summary>
        /// <param name="accountParentId"></param>
        /// <returns>List<GeneralLedgerModels.AccountingAccount></returns>
        public List<AccountingAccountDTO> GetAccountingAccountsByParentId(int accountParentId)
        {
            List<AccountingAccountDTO> accountingAccounts = new List<AccountingAccountDTO>();

            try
            {
                AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountParentId, accountParentId);

                BusinessCollection accountsCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccount), filter.GetPredicate()));

                if (accountsCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccount accountingAccountEntity in accountsCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                    {
                        AccountingAccountDTO accountingAccount = new AccountingAccountDTO();
                        accountingAccount = DTOAssembler.ToDTO(accountingAccountDAO.GetAccountingAccount(accountingAccountEntity.AccountingAccountId));
                        accountingAccounts.Add(accountingAccount);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return accountingAccounts.ToList();
        }

        public List<JournalEntryItemDTO> getAccountingAcountWhitCodeRule(JournalEntryItemDTO journalEntryItem, int moduleDateId, List<DTOs.AccountingRules.ParameterDTO> parameters, string codeRulePackage)
        {
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();
            try
            {
                EntryParameterApplicationServiceProvider _entryParameterService = new EntryParameterApplicationServiceProvider();
                List<ResultDTO> results = _entryParameterService.ExecuteAccountingRulePackage(moduleDateId, parameters, codeRulePackage);

                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        //Detalle
                        JournalEntryItemDTO newJournalEntryItem = new JournalEntryItemDTO();
                        newJournalEntryItem.AccountingAccount = new AccountingAccountDTO();
                        newJournalEntryItem.AccountingAccount.Number = result.AccountingAccount;
                        var accountings = GetAccountingAccountsByNumberDescription(newJournalEntryItem.AccountingAccount);
                        newJournalEntryItem.AccountingAccount = (accountings != null || accountings.Any()) ?
                            accountings.First() : new AccountingAccountDTO();
                        newJournalEntryItem.AccountingNature = result.AccountingNature;
                        newJournalEntryItem.ExchangeRate = journalEntryItem.ExchangeRate;
                        newJournalEntryItem.ExchangeRate.SellAmount = journalEntryItem.ExchangeRate.SellAmount;

                        newJournalEntryItem.Amount = new AmountDTO();
                        newJournalEntryItem.Amount.Currency = journalEntryItem.Amount.Currency;
                        newJournalEntryItem.LocalAmount = new AmountDTO();
                        newJournalEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture));
                        newJournalEntryItem.LocalAmount.Value = Math.Abs(journalEntryItem.LocalAmount.Value);

                        newJournalEntryItem.Analysis = new List<AnalysisDTO>();
                        newJournalEntryItem.ReconciliationMovementType = journalEntryItem.ReconciliationMovementType;
                        newJournalEntryItem.CostCenters = new List<CostCenterDTO>();
                        newJournalEntryItem.Currency = journalEntryItem.Currency;
                        newJournalEntryItem.Description = journalEntryItem.Description;
                        newJournalEntryItem.EntryType = new EntryTypeDTO();
                        newJournalEntryItem.Id = 0;
                        newJournalEntryItem.Individual = journalEntryItem.Individual;
                        newJournalEntryItem.PostDated = new List<PostDatedDTO>();
                        newJournalEntryItem.Receipt = journalEntryItem.Receipt;
                        newJournalEntryItem.SourceCode = journalEntryItem.SourceCode;
                        newJournalEntryItems.Add(newJournalEntryItem);
                    }
                }
                return newJournalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        #endregion AccountingAccount

        #region AccountingAccountParent

        /// <summary>
        /// GetAccountingAccountParent
        /// Obtiene cuenta contable principal
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>AccountingAccount</returns>
        public AccountingAccountDTO GetAccountingAccountParent(int accountingAccountId)
        {
            try
            {
                AccountingAccountParentDAO accountingAccountParentDAO = new AccountingAccountParentDAO();
                return DTOAssembler.ToDTO(accountingAccountParentDAO.GetAccountingAccountParent(accountingAccountId));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccountParents
        /// Obtiene Listado de cuentas contables principales
        /// </summary>
        /// <returns>List<GeneralLedgerModels.AccountingAccount></returns>
        public List<AccountingAccountDTO> GetAccountingAccountParents()
        {
            try
            {
                AccountingAccountParentDAO accountingAccountParentDAO = new AccountingAccountParentDAO();
                return DTOAssembler.ToDTOs(accountingAccountParentDAO.GetAccountingAccountParents()).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingAccountParent

        #region LedgerEntry

        /// <summary>
        /// SaveLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns>int</returns>
        public int SaveLedgerEntry(LedgerEntryDTO ledgerEntry)
        {
            LedgerEntryDAO ledgerEntryDAO = new LedgerEntryDAO();
            return ledgerEntryDAO.SaveLedgerEntryTransaction(ModelDTOAssembler.ToModel(ledgerEntry)).EntryNumber;
        }

        /// <summary>
        /// ReverseLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns>int</returns>
        public int ReverseLedgerEntry(LedgerEntryDTO ledgerEntry)
        {
            try
            {
                EntryRevertionDAO entryRevertionDAO = new EntryRevertionDAO();
                LedgerEntryDAO ledgerEntryDAO = new LedgerEntryDAO();
                //Se obtiene el asiento por clave primaria
                LedgerEntryDTO newLedgerEntry = new LedgerEntryDTO();
                newLedgerEntry = DTOAssembler.ToDTO(ledgerEntryDAO.GetLedgerEntryById(ledgerEntry.Id));
                newLedgerEntry.Id = 0; //es un nuevo registro
                newLedgerEntry.AccountingDate = ledgerEntry.AccountingDate;
                newLedgerEntry.RegisterDate = DateTime.Now;
                newLedgerEntry.Description = "REVERSION DEL ASIENTO # " + ledgerEntry.EntryNumber;

                //Se cambia la naturaleza de los movimientos.
                if (newLedgerEntry.LedgerEntryItems.Count > 0)
                {
                    foreach (LedgerEntryItemDTO ledgerEntryItem in newLedgerEntry.LedgerEntryItems)
                    {
                        ledgerEntryItem.AccountingNature = ledgerEntryItem.AccountingNature == (int)AccountingNatures.Credit ? (int)AccountingNatures.Debit : (int)AccountingNatures.Credit;
                    }
                }

                //se graba el nuevo asiento
                newLedgerEntry = DTOAssembler.ToDTO(ledgerEntryDAO.SaveLedgerEntryTransaction(ModelDTOAssembler.ToModel(newLedgerEntry), false));

                //se graba la relación del asiento de origen y destino
                entryRevertionDAO.SaveEntryRevertion(0, ledgerEntry.Id, newLedgerEntry.Id, ledgerEntry.UserId, DateTime.Now, false);

                return newLedgerEntry.EntryNumber;
            }
            catch (BusinessException ex)
            {
                if (ex.Message.Contains("BusinessException"))
                {
                    throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_BUSINESS_EXCEPTION_MSJ).ToString());
                }

                throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_UNHANDLED_EXCEPTION_MSJ).ToString());
            }
        }

        /// <summary>
        /// GetLedgerEntries
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>List<LedgerEntry/></returns>
        public List<LedgerEntryDTO> GetLedgerEntries(int ledgerEntryId, DateTime dateFrom, DateTime dateTo, int branchId, int destinationId, int accountingMovementTypeId)
        {
            LedgerEntryDAO ledgerEntryDAO = new LedgerEntryDAO();
            return DTOAssembler.ToDTOs(ledgerEntryDAO.GetLedgerEntries(ledgerEntryId, dateFrom, dateTo, branchId, destinationId, accountingMovementTypeId)).ToList();
        }

        #endregion LedgerEntry

        #region EntryConsultation

        /// <summary>
        /// SearchEntryMovements
        /// Busca asientos de mayor
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>List<EntryConsultationDTO/></returns>
        public List<EntryConsultationDTO> SearchEntryMovements(int entryNumber, DateTime startDate, DateTime endDate, int branchId, int destinationId, int accountingMovementTypeId)
        {
            try
            {
                int rows;
                var entryConsultationDTOs = new List<EntryConsultationDTO>();
                var criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Entry.Properties.EntryNumber, entryNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.Entry.Properties.Date);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(startDate);
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.Entry.Properties.Date);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(endDate);

                if (branchId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Entry.Properties.BranchCode, branchId);
                }
                if (destinationId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Entry.Properties.EntryDestinationId, destinationId);
                }
                if (accountingMovementTypeId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Entry.Properties.AccountingMovementTypeId, accountingMovementTypeId);
                }

                UIView entryConsultation = _dataFacadeManager.GetDataFacade().GetView("EntryConsultation", criteriaBuilder.GetPredicate(), null, 0, 100, null, true, out rows);

                if (entryConsultation.Count > 0)
                {
                    entryConsultationDTOs = (from DataRow dataRow in entryConsultation.Rows
                                             select new EntryConsultationDTO()
                                             {
                                                 EntryId = Convert.ToInt32(dataRow["EntryId"].ToString()),
                                                 AccountingAccountId = Convert.ToInt32(dataRow["AccountingAccountId"].ToString()),
                                                 AccountingAccountNumber = dataRow["AccountNumber"].ToString(),
                                                 AccountingAccountName = Convert.ToString(dataRow["AccountName"]),
                                                 CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"].ToString()),
                                                 CurrencyDescription = "",
                                                 AccountingNature = Convert.ToInt32(dataRow["AccountingNature"].ToString()),
                                                 DebitsAmountValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Debit ? Convert.ToDecimal(dataRow["AmountValue"].ToString()) : 0,
                                                 DebitsAmountLocalValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Debit ? Convert.ToDecimal(dataRow["AmountLocalValue"].ToString()) : 0,
                                                 CreditsAmountValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Credit ? Convert.ToDecimal(dataRow["AmountValue"].ToString()) : 0,
                                                 CreditsAmountLocalValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Credit ? Convert.ToDecimal(dataRow["AmountLocalValue"].ToString()) : 0,
                                                 Date = String.Format("{0:dd/MM/yyyy}", dataRow["Date"].ToString()),
                                                 EntryDescription = dataRow["EntryDescription"].ToString(),
                                                 EntryNumber = Convert.ToInt32(dataRow["EntryNumber"].ToString()),
                                                 BranchId = Convert.ToInt32(dataRow["BranchCode"].ToString()),
                                                 BranchDescription = "",
                                                 UserId = Convert.ToInt32(dataRow["UserCode"].ToString()),
                                                 UserName = "",
                                                 Status = GetEntryStatus(Convert.ToInt32(dataRow["EntryId"].ToString()), false),
                                                 EntryDestinationId = Convert.ToInt32(dataRow["EntryDestinationId"].ToString()),
                                                 EntryDestinationDescription = dataRow["EntryDestinationDescription"].ToString(),
                                                 AccountingMovementTypeId = Convert.ToInt32(dataRow["AccountingMovementTypeId"].ToString()),
                                                 AccountingMovementTypeDescription = dataRow["AccountingMovementTypeDescription"].ToString()
                                             }).ToList();
                }

                return entryConsultationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SearchDailyEntryMovements
        /// Busca asientos de diario
        /// </summary>
        /// <param name="technicalTransaction"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>List<EntryConsultationDTO/></returns>
        public List<EntryConsultationDTO> SearchDailyEntryMovements(int technicalTransaction, DateTime startDate, DateTime endDate, int branchId, int destinationId, int accountingMovementTypeId)
        {
            try
            {
                int rows;
                var entryConsultationDTOs = new List<EntryConsultationDTO>();
                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntry.Properties.TechnicalTransaction, technicalTransaction);

                if (branchId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntry.Properties.BranchCode, branchId);
                }

                if (startDate != Convert.ToDateTime("01/01/1900"))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.AccountingDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(startDate);
                }

                if (endDate != Convert.ToDateTime("01/01/1900"))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.AccountingDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(endDate);
                }

                var entryConsultation = _dataFacadeManager.GetDataFacade().GetView("DailyEntryConsultation", criteriaBuilder.GetPredicate(), null, 0, 10000, null, true, out rows);

                if (entryConsultation.Count > 0)
                {
                    entryConsultationDTOs = (from DataRow dataRow in entryConsultation.Rows
                                             select new EntryConsultationDTO()
                                             {
                                                 EntryId = Convert.ToInt32(dataRow["JournalEntryItemId"].ToString()),
                                                 AccountingAccountId = Convert.ToInt32(dataRow["AccountingAccountId"].ToString()),
                                                 AccountingAccountNumber = dataRow["AccountNumber"].ToString(),
                                                 AccountingAccountName = dataRow["AccountName"].ToString(),
                                                 CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"].ToString()),
                                                 CurrencyDescription = "",
                                                 AccountingNature = Convert.ToInt32(dataRow["AccountingNature"].ToString()),
                                                 DebitsAmountValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Debit ? Convert.ToDecimal(dataRow["AmountValue"].ToString()) : 0,
                                                 DebitsAmountLocalValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Debit ? Convert.ToDecimal(dataRow["AmountLocalValue"].ToString()) : 0,
                                                 CreditsAmountValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Credit ? Convert.ToDecimal(dataRow["AmountValue"].ToString()) : 0,
                                                 CreditsAmountLocalValue = (AccountingNatures)Convert.ToInt32(dataRow["AccountingNature"].ToString()) == AccountingNatures.Credit ? Convert.ToDecimal(dataRow["AmountLocalValue"].ToString()) : 0,
                                                 EntryHeaderDescription = dataRow["EntryHeaderDescription"].ToString(),
                                                 Date = Convert.ToDateTime(dataRow["Date"].ToString()).ToString("dd/MM/yyyy"),
                                                 EntryDescription = dataRow["Description"].ToString(),
                                                 EntryNumber = Convert.ToInt32(dataRow["EntryNumber"].ToString()),
                                                 BranchId = Convert.ToInt32(dataRow["BranchCode"].ToString()),
                                                 BranchDescription = "",
                                                 UserId = Convert.ToInt32(dataRow["UserCode"].ToString()),
                                                 UserName = "",
                                                 DailyEntryHeaderId = Convert.ToInt32(dataRow["JournalEntryId"].ToString()),
                                                 Status = GetEntryStatus(Convert.ToInt32(dataRow["JournalEntryId"].ToString()), true)
                                             }).ToList();
                }

                return entryConsultationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCostCentersByEntryId
        /// Obtiene centros de costo por Id de asiento
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<CostCenterDTO></returns>
        public List<CostCenterDTO> GetCostCentersByEntryId(int entryItemId, bool isJournalEntry)
        {
            List<CostCenterDTO> costCenters = new List<CostCenterDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.EntryItemId, entryItemId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.IsJournalEntry, isJournalEntry);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.CostCenterEntryItem), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.CostCenterEntryItem costCenterEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.CostCenterEntryItem>())
                    {
                        CostCenterDTO costCenter = new CostCenterDTO();
                        costCenter.CostCenterId = Convert.ToInt32(costCenterEntryItemEntity.CostCenterId);
                        costCenter = GetCostCenter(costCenter);
                        costCenter.PercentageAmount = Convert.ToDecimal(costCenterEntryItemEntity.CostCenterPercentage);
                        costCenters.Add(costCenter);
                    }
                }

                return costCenters;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryAnalysesByEntryId
        /// Obtiene análisis por Id de asiento
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<EntryAnalysisDTO></returns>
        public List<EntryAnalysisDTO> GetEntryAnalysesByEntryId(int entryItemId, bool isJournalEntry)
        {
            List<EntryAnalysisDTO> entryAnalysisDTOs = new List<EntryAnalysisDTO>();

            try
            {
                int rows;
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.EntryItemId, entryItemId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.IsJournalEntry, isJournalEntry);

                UIView analysisEntries = _dataFacadeManager.GetDataFacade().GetView("GetAnalysisByEntry", criteriaBuilder.GetPredicate(), null, 0, 50, null, true, out rows);

                if (analysisEntries.Count > 0)
                {
                    foreach (DataRow data in analysisEntries)
                    {
                        entryAnalysisDTOs.Add(new EntryAnalysisDTO
                        {
                            EntryAnalysisId = Convert.ToInt32(data["AnalysisEntryItemId"]),
                            AnalysisId = Convert.ToInt32(data["AnalysisId"]),
                            AnalysisDescription = Convert.ToString(data["AnalysisDescription"]),
                            AnalysisConceptId = Convert.ToInt32(data["AnalysisConceptId"]),
                            AnalysisConceptDescription = Convert.ToString(data["AnalysisConceptDescription"]),
                            EntryId = Convert.ToInt32(data["EntryItemId"]),
                            ConceptKey = Convert.ToString(data["ConceptKey"]),
                            EntryAnalysisDescription = Convert.ToString(data["EntryAnalysisDescription"])
                        });
                    }
                }

                return entryAnalysisDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPostdatedByEntryId
        /// Obtiene postfechados por Id de asiento
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<GeneralLedgerModels.PostDated></returns>
        public List<PostDatedDTO> GetPostdatedByEntryId(int entryItemId, bool isJournalEntry)
        {
            List<PostDatedDTO> postdated = new List<PostDatedDTO>();
            PostDatedDAO postDatedDAO = new PostDatedDAO();
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.PostdatedEntryItem.Properties.EntryItemId, entryItemId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.PostdatedEntryItem.Properties.IsJournalEntry, isJournalEntry);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.PostdatedEntryItem), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.PostdatedEntryItem>())
                    {
                        PostDatedDTO postDated = DTOAssembler.ToDTO(postDatedDAO.GetPostDated(postdatedEntryItemEntity.PostdatedEntryItemId));
                        postdated.Add(postDated);
                    }
                }
                return postdated.ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryStatus
        /// Obtiene el estado de un movimiento
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>AccountingEntryStatus</returns>
        public AccountingEntryStatus GetEntryStatus(int entryId, bool isJournalEntry)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryRevertion.Properties.EntrySourceId, entryId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryRevertion.Properties.IsJournalEntry, isJournalEntry);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryRevertion), criteriaBuilder.GetPredicate()));

            return businessCollection.Count > 0 ? AccountingEntryStatus.Reverted : AccountingEntryStatus.Active;
        }

        /// <summary>
        /// GetJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntryDTO GetJournalEntry(JournalEntryDTO journalEntry)
        {
            try
            {
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                journalEntry = DTOAssembler.ToDTO(journalEntryDAO.GetJournalEntryByTechnicalTransaction((journalEntry.Id)));
                journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.JournalEntryId, journalEntry.Id);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), filter.GetPredicate()));
                //se llena los movimientos del asiento de diario
                if (businessCollection.Any())
                {
                    foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntryItem>())
                    {
                        JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                        journalEntryItem.Id = journalEntryItemEntity.JournalEntryItemId;
                        journalEntryItem = DTOAssembler.ToDTO(journalEntryItemDAO.GetJournalEntryItem(ModelDTOAssembler.ToModel(journalEntryItem)));
                        journalEntryItem.CostCenters = new List<CostCenterDTO>();
                        journalEntryItem.CostCenters = GetCostCentersByEntryId(journalEntryItemEntity.JournalEntryItemId, true);
                        journalEntryItem.Analysis = new List<AnalysisDTO>();
                        journalEntryItem.Analysis = GetAnalysesByEntryId(journalEntryItemEntity.JournalEntryItemId, true);
                        journalEntryItem.CostCenters = new List<CostCenterDTO>();
                        journalEntryItem.CostCenters = GetCostCentersByEntryId(journalEntryItemEntity.JournalEntryItemId, true);

                        journalEntry.JournalEntryItems.Add(journalEntryItem);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return journalEntry;
        }

        #endregion EntryConsultation

        #region JournalEntry

        /// <summary>
        /// SaveJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int SaveJournalEntry(JournalEntryDTO journalEntry)
        {
            CommonModels.Parameter journalEntryParameter = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_JOURNAL_ENTRY_TRANSACTION_NUMBER)));

            int journalEntryNumber = Convert.ToInt32(journalEntryParameter.NumberParameter);
            int journalEntryId = 0;

            try
            {
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                //se graba la cabecera del asiento.
                journalEntry.EntryNumber = journalEntryNumber;
                JournalEntryDTO newJournalEntry = new JournalEntryDTO();


                //using (Context.Current)
                //{
                    //using (Transaction transaction = new Transaction())
                    //{
                        // Bloque transacccional de persistencia
                        try
                        {
                            newJournalEntry = journalEntryDAO.SaveJournalEntry(journalEntry.ToModel()).ToDTO();

                            //si se graba la cabecera, se procede a grabar los movimientos.
                            if (newJournalEntry.Id > 0)
                            {
                                if (journalEntry.JournalEntryItems.Any())
                                {
                                    // NASE INICIO: Se deja este bloque para controlar la inforación incompleta
                                    bool validateIsNull = (journalEntry.ModuleDateId == 2 && journalEntry.JournalEntryItems.Count == journalEntry.JournalEntryItems.Count(x => x.AccountingAccount == null || x.AccountingAccount?.AccountingAccountId == 0));
                                    if (validateIsNull)
                                    {
                                        int cashAccount = CommonBusiness.GetIntParameter(GeneralLederKeys.GL_ACCOUNTING_ACCOUNT_CASH);
                                        int bridgeAccount = CommonBusiness.GetIntParameter(GeneralLederKeys.GL_ACCOUNTING_ACCOUNT_BRIDGE);

                                        if (cashAccount > 0)
                                        {
                                            journalEntry.JournalEntryItems.ForEach(x =>
                                            {
                                                if (x.AccountingNature == Convert.ToInt32(AccountingNatures.Debit))
                                                {
                                                    x.AccountingAccount = new AccountingAccountDTO()
                                                    {
                                                        AccountingAccountId = cashAccount
                                                    };
                                                }
                                            });
                                        }
                                        if (bridgeAccount > 0)
                                        {
                                            journalEntry.JournalEntryItems.ForEach(x =>
                                            {
                                                if (x.AccountingNature == Convert.ToInt32(AccountingNatures.Credit))
                                                {
                                                    x.AccountingAccount = new AccountingAccountDTO()
                                                    {
                                                        AccountingAccountId = bridgeAccount
                                                    };
                                                }
                                            });
                                        }
                                    }
                                    // NASE FIN: Se deja este bloque para controlar la inforación incompleta

                                    foreach (JournalEntryItem journalEntryItem in journalEntry.JournalEntryItems.ToModels())
                                    {
                                        JournalEntryItem newJournalEntryItem = new JournalEntryItem();
                                        (journalEntryItemDAO.SaveJournalEntryItem(journalEntryItem, newJournalEntry.Id)).ToDTO();
                                        SaveItemGroup(journalEntryItem.ToDTO(), newJournalEntryItem.ToDTO());
                                    }

                                    journalEntryId = newJournalEntry.Id;

                                    if (journalEntryId > 0)
                                    {
                                        //se actualiza el número de asiento
                                        journalEntryParameter.NumberParameter = journalEntryNumber + 1;
                                        DelegateService.commonService.UpdateParameter(journalEntryParameter);

                                        //se realiza integracion en 2g
                                        CollectApplicationControlDAO integration2G = new CollectApplicationControlDAO();
                                        integration2G.Insert(ModelDTOAssembler.ToModelIntegration(newJournalEntry.TechnicalTransaction));
                                       // transaction.Complete();
                                    }
                                }
                                else
                                {
                                    //Si no existen items en el asiento elimina la cabecera y envía el código de mensaje de error de grabación de asiento.
                                    journalEntryDAO.DeleteJournalEntry(ModelDTOAssembler.ToModel(journalEntry));
                                    journalEntryId = -2;
                                    //transaction.Dispose();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //transaction.Dispose();
                            throw new BusinessException(Resources.Resources.ErrorSavingJournalEntry, ex);
                        }
                    //}
                //}

            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores
                journalEntryId = 0;
            }

            return journalEntryId;
        }

        /// <summary>
        /// SaveJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int SaveJournalEntryWithoutTransaction(JournalEntryDTO journalEntry)
        {
            int technicalTransaction = journalEntry.TechnicalTransaction;
            int journalEntryId = 0;

            try
            {
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                JournalEntry newJournalEntry = new JournalEntry();
                if (technicalTransaction > 0)
                    newJournalEntry = journalEntryDAO.GetJournalEntryByTechnicalTransaction(technicalTransaction);
                else
                    newJournalEntry = journalEntryDAO.GetJournalEntryByJournaEntryId(journalEntry.Id);

                if (newJournalEntry.Id == 0)
                    newJournalEntry = journalEntryDAO.SaveJournalEntry(journalEntry.ToModel());

                //si se graba la cabecera, se procede a grabar los movimientos.
                if (newJournalEntry.Id > 0)
                {
                    if (journalEntry.JournalEntryItems.Any())
                    {
                        bool validateIsNull = (journalEntry.ModuleDateId == 2 && journalEntry.JournalEntryItems.Count == journalEntry.JournalEntryItems.Count(x => x.AccountingAccount == null || x.AccountingAccount?.AccountingAccountId == 0));
                        if (validateIsNull)
                        {
                            int cashAccount = CommonBusiness.GetIntParameter(GeneralLederKeys.GL_ACCOUNTING_ACCOUNT_CASH);
                            int bridgeAccount = CommonBusiness.GetIntParameter(GeneralLederKeys.GL_ACCOUNTING_ACCOUNT_BRIDGE);

                            if (cashAccount > 0)
                            {
                                journalEntry.JournalEntryItems.ForEach(x => {
                                    if (x.AccountingNature == Convert.ToInt32(AccountingNatures.Debit))
                                    {
                                        x.AccountingAccount = new AccountingAccountDTO()
                                        {
                                            AccountingAccountId = cashAccount
                                        };
                                    }
                                });
                            }
                            if (bridgeAccount > 0)
                            {
                                journalEntry.JournalEntryItems.ForEach(x => {
                                    if (x.AccountingNature == Convert.ToInt32(AccountingNatures.Credit))
                                    {
                                        x.AccountingAccount = new AccountingAccountDTO()
                                        {
                                            AccountingAccountId = bridgeAccount
                                        };
                                    }
                                });
                            }
                        }

                        JournalEntryItem newJournalEntryItem;
                        foreach (JournalEntryItem journalEntryItem in journalEntry.JournalEntryItems.ToModels())
                        {
                            newJournalEntryItem = journalEntryItemDAO.SaveJournalEntryItem(journalEntryItem, newJournalEntry.Id);
                            SaveItemGroup(journalEntryItem.ToDTO(), newJournalEntryItem.ToDTO());
                        }
                        journalEntryId = newJournalEntry.Id;
                    }
                    else
                    {
                        //Si no existen items en el asiento elimina la cabecera y envía el código de mensaje de error de grabación de asiento.
                        journalEntryDAO.DeleteJournalEntry(ModelDTOAssembler.ToModel(journalEntry));
                        journalEntryId = -2;
                    }

                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores
                journalEntryId = 0;
            }
            return journalEntryId;
        }

        public int SaveJournalEntryItem(int technicalTransaction)
        {
            //CommonModels.Parameter journalEntryParameter = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_JOURNAL_ENTRY_TRANSACTION_NUMBER)));
            //int journalEntryNumber = Convert.ToInt32(journalEntryParameter.NumberParameter);

            int journalEntryItemId = 0;

            try
            {
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                //se graba la cabecera del asiento.
                JournalEntryDTO journalEntry = new JournalEntryDTO();
                journalEntry = DTOAssembler.ToDTO(journalEntryItemDAO.GetJournalEntryItemsByTechnicalTransaction(technicalTransaction));


                //si se graba la cabecera, se procede a grabar los movimientos.
                if (journalEntry.Id > 0)
                {
                    //if (journalEntry.JournalEntryItems.Any())
                    //{
                    foreach (JournalEntryItem journalEntryItem in journalEntry.JournalEntryItems.ToModels())
                    {
                        if (journalEntryItem.AccountingNature == AccountingNatures.Credit)
                        {
                            JournalEntryItem newJournalEntryItem = new JournalEntryItem();
                            journalEntryItem.AccountingNature = AccountingNatures.Debit;
                            newJournalEntryItem = (journalEntryItemDAO.SaveJournalEntryItem(journalEntryItem, journalEntry.Id));
                            SaveItemGroup(journalEntryItem.ToDTO(), newJournalEntryItem.ToDTO());
                        }

                    }

                    journalEntryItemId = journalEntry.Id;

                    //if (journalEntryId > 0)
                    //{
                    //    //se actualiza el número de asiento
                    //    journalEntryParameter.NumberParameter = journalEntryNumber + 1;
                    //    DelegateService.commonService.UpdateParameter(journalEntryParameter);
                    //}
                    //}
                    //else
                    //{
                    //    //Si no existen items en el asiento elimina la cabecera y envía el código de mensaje de error de grabación de asiento.
                    //    journalEntryDAO.DeleteJournalEntry(ModelDTOAssembler.ToModel(journalEntry));
                    //    journalEntryItemId = -2;
                    //}

                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores
                journalEntryItemId = 0;
            }

            return journalEntryItemId;
        }

        /// <summary>
        /// ReverseJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int ReverseJournalEntry(JournalEntryDTO journalEntry, int NewTechnicalTransaction = 0)
        {
            int newJournalEntryId = 0;

            try
            {
                EntryRevertionDAO entryRevertionDAO = new EntryRevertionDAO();
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                int idModuleTypeClaim = -1;
                //se actualiza el asiento de origen.
                journalEntry = DTOAssembler.ToDTO(journalEntryDAO.GetJournalEntry(ModelDTOAssembler.ToModel(journalEntry)));
                journalEntry.Status = Convert.ToInt32(AccountingEntryStatus.Reverted);
                journalEntry = DTOAssembler.ToDTO(journalEntryDAO.UpdateJournalEntry(ModelDTOAssembler.ToModel(journalEntry)));

                //se obtiene la cabecera del asiento.
                JournalEntryDTO newJournalEntry = new JournalEntryDTO();
                newJournalEntry = DTOAssembler.ToDTO(journalEntryDAO.GetJournalEntry(ModelDTOAssembler.
                    ToModel(journalEntry)));
                newJournalEntry.Id = 0; //se resetea el id para el nuevo asiento
                newJournalEntry.EntryNumber = journalEntry.EntryNumber;
                newJournalEntry.RegisterDate = DateTime.Now;
                try
                {
                    idModuleTypeClaim = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.ACL_CLAIMS_MODULE));
                }
                catch (Exception ex)
                {

                }

                // Se verifica que sea un movimiento de denuncias
                if (journalEntry.ModuleDateId == idModuleTypeClaim)
                {
                    journalEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(journalEntry.ModuleDateId, DateTime.Today).Date;
                }

                newJournalEntry.AccountingDate = journalEntry.AccountingDate;

                //se obtienen los movimientos del asiento.
                newJournalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

                newJournalEntry.JournalEntryItems = SetRevertionJournalEntryItems(journalEntry.Id);
                if (NewTechnicalTransaction > 0)
                    newJournalEntry.TechnicalTransaction = NewTechnicalTransaction;
                newJournalEntry.Description = "REVERSION DEL ASIENTO # " + journalEntry.Id;
                newJournalEntry.Status = Convert.ToInt32(AccountingEntryStatus.Reverted);
                newJournalEntryId = SaveJournalEntry(newJournalEntry);

                //se graba la relación del asiento de origen y destino.
                entryRevertionDAO.SaveEntryRevertion(0, journalEntry.Id, newJournalEntryId, journalEntry.UserId, DateTime.Now, true);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return newJournalEntryId;
        }

        /// <summary>
        /// GetJournalEntries
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>List<JournalEntryDTO></returns>
        public List<JournalEntryDTO> GetJournalEntries(int journalEntryId, DateTime dateFrom, DateTime dateTo, int branchId)
        {
            return new List<JournalEntryDTO>();
        }

        #endregion JournalEntry

        #region CostCenterEntry

        /// <summary>
        /// SaveCostCenterEntry
        /// Graba Centros de costos asociados a un movimiento de Asiento de diario o de mayor
        /// </summary>
        /// <param name="costCenter"></param>
        /// <param name="accountingEntry"></param>
        /// <param name="isDailyEntry"></param>
        public void SaveCostCenterEntry(CostCenterDTO costCenter, JournalEntryDTO accountingEntry, bool isDailyEntry)
        {
            CostCenterEntryDAO costCenterEntryDAO = new CostCenterEntryDAO();
            costCenterEntryDAO.SaveCostCenterEntry(ModelDTOAssembler.ToModel(costCenter), accountingEntry.Id, isDailyEntry);
        }

        #endregion CostCenterEntry

        #endregion GL

        #region PARAM

        #region AccountingCompany

        /// <summary>
        /// SaveAccountingCompany
        /// Graba una compania
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns>AccountingCompany</returns>
        public AccountingCompanyDTO SaveAccountingCompany(AccountingCompanyDTO accountingCompany)
        {
            try
            {
                AccountingCompanyDAO accountingCompanyDAO = new AccountingCompanyDAO();
                accountingCompany = DTOAssembler.ToDTO(accountingCompanyDAO.SaveAccountingCompany(ModelDTOAssembler.ToModel(accountingCompany)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingCompany;
        }

        /// <summary>
        /// UpdateAccountingCompany
        /// Actualiza compañía
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns>AccountingCompany</returns>
        public AccountingCompanyDTO UpdateAccountingCompany(AccountingCompanyDTO accountingCompany)
        {
            try
            {
                AccountingCompanyDAO accountingCompanyDAO = new AccountingCompanyDAO();
                accountingCompany = DTOAssembler.ToDTO(accountingCompanyDAO.UpdateAccountingCompany(ModelDTOAssembler.ToModel(accountingCompany)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingCompany;
        }

        /// <summary>
        /// DeleteAccountingCompany
        /// Borra compañía
        /// </summary>
        /// <param name="accountingCompanyId"></param>
        /// <returns>bool</returns>
        public bool DeleteAccountingCompany(int accountingCompanyId)
        {
            AccountingCompanyDAO accountingCompanyDAO = new AccountingCompanyDAO();

            bool isDeleted;

            try
            {
                isDeleted = accountingCompanyDAO.DeleteAccountingCompany(accountingCompanyId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return isDeleted;
        }

        /// <summary>
        /// GetAccountingCompany
        /// Obtiene compañía
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns>AccountingCompany</returns>
        public AccountingCompanyDTO GetAccountingCompany(AccountingCompanyDTO accountingCompany)
        {
            try
            {
                AccountingCompanyDAO accountingCompanyDAO = new AccountingCompanyDAO();
                return DTOAssembler.ToDTO(accountingCompanyDAO.GetAccountingCompany(ModelDTOAssembler.ToModel(accountingCompany)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingCompanies
        /// Obtiene el listado de compañías
        /// </summary>
        /// <returns>List<AccountingCompanyDTO></returns>
        public List<AccountingCompanyDTO> GetAccountingCompanies()
        {
            try
            {
                AccountingCompanyDAO accountingCompanyDAO = new AccountingCompanyDAO();
                return DTOAssembler.ToDTOs(accountingCompanyDAO.GetAccountingCompanies()).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// VerifyCompanyUsed
        /// Verifica si una compañía de la tabla esta siendo usada por un movimiento Contable
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>bool</returns>
        public bool VerifyCompanyUsed(int companyId)
        {
            try
            {
                // Creación del  filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                if (companyId != 0)
                {
                    criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.AccountingCompanyId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(companyId);
                }

                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntry),
                                                                        criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    if (companyId != 0)
                    {
                        criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingCompanyId);
                        criteriaBuilder.Equal();
                        criteriaBuilder.Constant(companyId);
                    }

                    // Asignamos BusinessCollection a una Lista
                    businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntry),
                                                                criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingCompany

        #region AccountingMovementType

        /// <summary>
        /// GetAccountingMovementType
        /// Obtiene Tipo de movimiento contable
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns>AccountingMovementType</returns>
        public AccountingMovementTypeDTO GetAccountingMovementType(AccountingMovementTypeDTO accountingMovementType)
        {
            AccountingMovementTypeDAO accountingMovementTypeDAO = new AccountingMovementTypeDAO();
            return DTOAssembler.ToDTO(accountingMovementTypeDAO.GetAccountingMovementType(ModelDTOAssembler.ToModel(accountingMovementType)));
        }

        /// <summary>
        /// GetAccountingMovementTypes
        /// Obtiene listado de tipos de movimientos contables
        /// </summary>
        /// <returns>List<AccountingMovementTypeDTO></returns>
        public List<AccountingMovementTypeDTO> GetAccountingMovementTypes()
        {
            AccountingMovementTypeDAO accountingMovementTypeDAO = new AccountingMovementTypeDAO();
            return DTOAssembler.ToDTOs(accountingMovementTypeDAO.GetAccountingMovementTypes()).ToList();
        }

        /// <summary>
        /// GetManualAccountingMovementTypes
        /// Obtiene Tipo de movimiento contable manuales
        /// </summary>
        /// <returns>List<AccountingMovementTypeDTO></returns>
        public List<AccountingMovementTypeDTO> GetManualAccountingMovementTypes()
        {
            List<AccountingMovementTypeDTO> accountingMovementTypes = new List<AccountingMovementTypeDTO>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            // Se obtiene los movimientos manuales
            criteriaBuilder.Property(GENERALLEDGEREN.AccountingMovementType.Properties.IsAutomatic);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(0);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingMovementType), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                foreach (GENERALLEDGEREN.AccountingMovementType accountingMovementTypeEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingMovementType>())
                {
                    AccountingMovementTypeDTO accountingMovementType = new AccountingMovementTypeDTO();
                    accountingMovementType.AccountingMovementTypeId = accountingMovementTypeEntity.AccountingMovementTypeId;
                    accountingMovementType.Description = accountingMovementTypeEntity.Description;
                    accountingMovementType.IsAutomatic = Convert.ToBoolean(accountingMovementTypeEntity.IsAutomatic);
                    accountingMovementTypes.Add(accountingMovementType);
                }
            }
            return accountingMovementTypes;
        }

        #endregion AccountingMovementType

        #region Analysis

        /// <summary>
        /// GetAnalysis
        /// Obtiene Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <returns>Analysis</returns>
        public AnalysisDTO GetAnalysis(int analysisId)
        {
            AnalysisDAO analysisDAO = new AnalysisDAO();
            return DTOAssembler.ToDTO(analysisDAO.GetAnalysis(analysisId));
        }

        /// <summary>
        /// GetAnalyses
        /// Obtiene listado de Analisis
        /// </summary>
        /// <returns>List<AnalysisDTO></returns>
        public List<AnalysisDTO> GetAnalyses()
        {
            AnalysisDAO analysisDAO = new AnalysisDAO();
            return DTOAssembler.ToDTOs(analysisDAO.GetAnalyses()).ToList();
        }

        #endregion Analysis

        #region AnalysisCode

        /// <summary>
        /// SaveAnalysisCode
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns>AnalysisCode</returns>
        public AnalysisCodeDTO SaveAnalysisCode(AnalysisCodeDTO analysisCode)
        {
            try
            {
                AnalysisCodeDAO analysisCodeDAO = new AnalysisCodeDAO();
                analysisCode = DTOAssembler.ToDTO(analysisCodeDAO.SaveAnalysisCode(ModelDTOAssembler.ToModel(analysisCode)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisCode;
        }

        /// <summary>
        /// UpdateAnalysisCode
        /// Actualiza Código de Analisis
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns>AnalysisCode</returns>
        public AnalysisCodeDTO UpdateAnalysisCode(AnalysisCodeDTO analysisCode)
        {
            try
            {
                AnalysisCodeDAO analysisCodeDAO = new AnalysisCodeDAO();
                analysisCode = DTOAssembler.ToDTO(analysisCodeDAO.UpdateAnalysisCode(ModelDTOAssembler.ToModel(analysisCode)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisCode;
        }

        /// <summary>
        /// DeleteAnalysisCode
        /// Borra Código de Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        public void DeleteAnalysisCode(int analysisId)
        {
            try
            {
                AnalysisCodeDAO analysisCodeDAO = new AnalysisCodeDAO();
                analysisCodeDAO.DeleteAnalysisCode(analysisId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisCode
        /// Obtiene Código de Analisis
        /// </summary>
        /// <param name="analysisCodeId"></param>
        /// <returns>AnalysisCode</returns>
        public AnalysisCodeDTO GetAnalysisCode(int analysisCodeId)
        {
            AnalysisCodeDAO analysisCodeDAO = new AnalysisCodeDAO();
            return DTOAssembler.ToDTO(analysisCodeDAO.GetAnalysisCode(analysisCodeId));
        }

        /// <summary>
        /// GetAnalysisCodes
        /// Obtiene listado de Códigos de Analisis
        /// </summary>
        /// <returns>List<AnalysisCodeDTO></returns>
        public List<AnalysisCodeDTO> GetAnalysisCodes()
        {
            try
            {
                AnalysisCodeDAO analysisCodeDAO = new AnalysisCodeDAO();
                return analysisCodeDAO.GetAnalysisCodes().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAnalysisCodes);
            }
        }

        #endregion AnalysisCode

        #region AnalysisTreatment

        /// <summary>
        /// SaveAnalysisTreatment
        /// Guarda Tratamiento de Analisis
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns>AnalysisTreatment</returns>
        public AnalysisTreatmentDTO SaveAnalysisTreatment(AnalysisTreatmentDTO analysisTreatment)
        {
            try
            {
                AnalysisTreatmentDAO analysisTreatmentDAO = new AnalysisTreatmentDAO();
                analysisTreatment = DTOAssembler.ToDTO(analysisTreatmentDAO.SaveAnalysisTreatment(ModelDTOAssembler.ToModel(analysisTreatment)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisTreatment;
        }

        /// <summary>
        /// UpdateAnalysisTreatment
        /// Actualiza Tratamiento de Analisis
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns>AnalysisTreatment</returns>
        public AnalysisTreatmentDTO UpdateAnalysisTreatment(AnalysisTreatmentDTO analysisTreatment)
        {
            try
            {
                AnalysisTreatmentDAO analysisTreatmentDAO = new AnalysisTreatmentDAO();
                analysisTreatment = DTOAssembler.ToDTO(analysisTreatmentDAO.UpdateAnalysisTreatment(ModelDTOAssembler.ToModel(analysisTreatment)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisTreatment;
        }

        /// <summary>
        /// DeleteAnalysisTreatment
        /// Borra Tratamiento de Analisis
        /// </summary>
        /// <param name="analysisTreatmentId"></param>
        public void DeleteAnalysisTreatment(int analysisTreatmentId)
        {
            try
            {
                AnalysisTreatmentDAO analysisTreatmentDAO = new AnalysisTreatmentDAO();
                analysisTreatmentDAO.DeleteAnalysisTreatment(analysisTreatmentId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisTreatment
        /// Obtiene Tratamiento de análisis
        /// </summary>
        /// <param name="analysisTreatmentId"></param>
        /// <returns>AnalysisTreatment</returns>
        public AnalysisTreatmentDTO GetAnalysisTreatment(int analysisTreatmentId)
        {
            AnalysisTreatmentDAO analysisTreatmentDAO = new AnalysisTreatmentDAO();
            return DTOAssembler.ToDTO(analysisTreatmentDAO.GetAnalysisTreatment(analysisTreatmentId));
        }

        /// <summary>
        /// GetAnalysisTreatments
        /// Obtiene listado de Tratamientos de análisis
        /// </summary>
        /// <returns>List<AnalysisTreatmentDTO></returns>
        public List<AnalysisTreatmentDTO> GetAnalysisTreatments()
        {
            AnalysisTreatmentDAO analysisTreatmentDAO = new AnalysisTreatmentDAO();
            return DTOAssembler.ToDTOs(analysisTreatmentDAO.GetAnalysisTreatments()).ToList();
        }

        #endregion AnalysisTreatment

        #region AnalysisConcept

        /// <summary>
        /// SaveAnalysisConcept
        /// Guarda Concepto de Análisis
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns>AnalysisConcept</returns>
        public AnalysisConceptDTO SaveAnalysisConcept(AnalysisConceptDTO analysisConcept)
        {
            try
            {
                AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();
                analysisConcept = DTOAssembler.ToDTO(analysisConceptDAO.SaveAnalysisConcept(ModelDTOAssembler.ToModel(analysisConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisConcept;
        }

        /// <summary>
        /// UpdateAnalysisConcept
        /// Actualiza Concepto de Análisis
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns>AnalysisConcept</returns>
        public AnalysisConceptDTO UpdateAnalysisConcept(AnalysisConceptDTO analysisConcept)
        {
            try
            {
                AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();
                analysisConcept = DTOAssembler.ToDTO(analysisConceptDAO.UpdateAnalysisConcept(ModelDTOAssembler.ToModel(analysisConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisConcept;
        }

        /// <summary>
        /// DeleteAnalysisConcept
        /// Borra Concepto de Análisis
        /// </summary>
        /// <param name="analysisConceptId"></param>
        public void DeleteAnalysisConcept(int analysisConceptId)
        {
            try
            {
                AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();
                analysisConceptDAO.DeleteAnalysisConcept(analysisConceptId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisConcept
        /// Obtiene Concepto de Análisis
        /// </summary>
        /// <param name="analysisConceptId"></param>
        /// <returns>AnalysisConcept</returns>
        public AnalysisConceptDTO GetAnalysisConcept(int analysisConceptId)
        {
            AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();
            return DTOAssembler.ToDTO(analysisConceptDAO.GetAnalysisConcept(analysisConceptId));
        }

        /// <summary>
        /// GetAnalysisConcepts
        /// Obtiene listado de Conceptos de Análisis
        /// </summary>
        /// <returns>List<AnalysisConceptDTO></returns>
        public List<AnalysisConceptDTO> GetAnalysisConcepts()
        {
            AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();
            return DTOAssembler.ToDTOs(analysisConceptDAO.GetAnalysisConcepts()).ToList();
        }

        #endregion AnalysisConcept

        #region AnalysisConceptKey

        /// <summary>
        /// SaveAnalysisConceptKey 
        /// Guardar clave de concepto de Análisis
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns>AnalysisConceptKey</returns>
        public AnalysisConceptKeyDTO SaveAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey)
        {
            try
            {
                AnalysisConceptKeyDAO analysisConceptKeyDAO = new AnalysisConceptKeyDAO();
                analysisConceptKey = DTOAssembler.ToDTO(analysisConceptKeyDAO.SaveAnalysisConceptKey(ModelDTOAssembler.ToModel(analysisConceptKey)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisConceptKey;
        }

        /// <summary>
        /// UpdateAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns>AnalysisConceptKey</returns>
        public AnalysisConceptKeyDTO UpdateAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey)
        {
            try
            {
                AnalysisConceptKeyDAO analysisConceptKeyDAO = new AnalysisConceptKeyDAO();
                analysisConceptKey = DTOAssembler.ToDTO(analysisConceptKeyDAO.UpdateAnalysisConceptKey(ModelDTOAssembler.ToModel(analysisConceptKey)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return analysisConceptKey;
        }

        /// <summary>
        /// DeleteAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        public void DeleteAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey)
        {
            try
            {
                AnalysisConceptKeyDAO analysisConceptKeyDAO = new AnalysisConceptKeyDAO();
                analysisConceptKeyDAO.DeleteAnalysisConceptKey(ModelDTOAssembler.ToModel(analysisConceptKey));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns>AnalysisConceptKey</returns>
        public AnalysisConceptKeyDTO GetAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey)
        {
            AnalysisConceptKeyDAO analysisConceptKeyDAO = new AnalysisConceptKeyDAO();
            return DTOAssembler.ToDTO(analysisConceptKeyDAO.GetAnalysisConceptKey(ModelDTOAssembler.ToModel(analysisConceptKey)));
        }

        /// <summary>
        /// GetAnalysisConceptKeys
        /// </summary>
        /// <returns>List<AnalysisConceptKey/></returns>
        public List<AnalysisConceptKeyDTO> GetAnalysisConceptKeys()
        {
            AnalysisConceptKeyDAO analysisConceptKeyDAO = new AnalysisConceptKeyDAO();
            return DTOAssembler.ToDTOs(analysisConceptKeyDAO.GetAnalysisConceptKeys()).ToList();
        }

        /// <summary>
        /// GetAnalysisConceptKeysByAnalysisConcept
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns>List<AnalysisConceptKey/></returns>
        public List<AnalysisConceptKeyDTO> GetAnalysisConceptKeysByAnalysisConcept(AnalysisConceptDTO analysisConcept)
        {
            AnalysisConceptKeyDAO analysisConceptKeyDAO = new AnalysisConceptKeyDAO();
            return DTOAssembler.ToDTOs(analysisConceptKeyDAO.GetAnalysisConceptKeysByAnalysisConcept(ModelDTOAssembler.ToModel(analysisConcept))).ToList();
        }

        #endregion

        #region AnalysisConceptAnalysis

        /// <summary>
        /// SaveAnalysisConceptAnalysis
        /// Graba relación Analisis y Concepto de Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        public void SaveAnalysisConceptAnalysis(int analysisId, int analysisConceptId)
        {
            try
            {
                AnalysisConceptAnalysisDAO analysisConceptAnalysisDAO = new AnalysisConceptAnalysisDAO();
                analysisConceptAnalysisDAO.SaveAnalysisConceptAnalysis(analysisId, analysisConceptId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteAnalysisConceptAnalysis
        /// Borra relación Analisis y Concepto de Analisis
        /// </summary>
        /// <param name="analysisConceptAnalysisId"></param>
        public void DeleteAnalysisConceptAnalysis(int analysisConceptAnalysisId)
        {
            try
            {
                AnalysisConceptAnalysisDAO analysisConceptAnalysisDAO = new AnalysisConceptAnalysisDAO();
                analysisConceptAnalysisDAO.DeleteAnalysisConceptAnalysis(analysisConceptAnalysisId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentConceptsByAnalysisCode
        /// Obtiene los conceptos de pago a partir del Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <returns>List<AnalysisConceptAnalysisDTO></returns>
        public List<AnalysisConceptAnalysisDTO> GetPaymentConceptsByAnalysisCode(int analysisId)
        {
            List<AnalysisConceptAnalysisDTO> analysisConcepts = new List<AnalysisConceptAnalysisDTO>();
            AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();
            AnalysisConceptAnalysisDAO analysisConceptAnalysisDAO = new AnalysisConceptAnalysisDAO();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisConceptAnalysis.Properties.AnalysisId, analysisId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisConceptAnalysis), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                foreach (GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysisEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisConceptAnalysis>())
                {
                    try
                    {
                        AnalysisConceptAnalysisDTO analysisConceptAnalysisDTO = new AnalysisConceptAnalysisDTO();
                        // Se obtiene los datos del concepto de análisis para llenarlos en el dto
                        AnalysisConceptDTO analysisConcept = DTOAssembler.ToDTO(analysisConceptDAO.GetAnalysisConcept(Convert.ToInt32(analysisConceptAnalysisEntity.AnalysisConceptId)));
                        analysisConceptAnalysisDTO.AnalysisConceptAnalysisId = analysisConceptAnalysisEntity.AnalysisConceptAnalysisId;
                        analysisConceptAnalysisDTO.AnalysisId = Convert.ToInt32(analysisConceptAnalysisEntity.AnalysisId);
                        analysisConceptAnalysisDTO.AnalysisConceptId = analysisConcept.AnalysisConceptId;
                        analysisConceptAnalysisDTO.AnalysisConceptDescription = analysisConcept.Description;
                        analysisConcepts.Add(analysisConceptAnalysisDTO);
                    }
                    catch (BusinessException)
                    {
                        // En caso de haber sido borrado o cambiado un AnalisisConcept se borra la relación para evitar errores futuros
                        analysisConceptAnalysisDAO.DeleteAnalysisConceptAnalysis(analysisConceptAnalysisEntity.AnalysisConceptAnalysisId);
                    }
                }
            }

            return analysisConcepts;
        }

        /// <summary>
        /// GetRemainingAnalysisConcepts
        /// Obtiene los conceptos de pago que aún no han sido relacionados con el Analisis
        /// </summary>
        /// <param name="analysisCodeId"></param>
        /// <returns>List<AnalysisConceptDTO></returns>
        public List<AnalysisConceptDTO> GetRemainingAnalysisConcepts(int analysisCodeId)
        {
            List<AnalysisConceptDTO> analysisConcepts = new List<AnalysisConceptDTO>();
            AnalysisConceptDAO analysisConceptDAO = new AnalysisConceptDAO();

            // Hago select a la tabla analysisConceptAnalysis
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(GENERALLEDGEREN.AnalysisConceptAnalysis.Properties.AnalysisConceptAnalysisId);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(0);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisConceptAnalysis), criteriaBuilder.GetPredicate()));

            // Hago select a la tabla analysisConcept
            criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(GENERALLEDGEREN.AnalysisConcept.Properties.AnalysisConceptId);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(0);
            BusinessCollection conceptBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisConcept), criteriaBuilder.GetPredicate()));

            // Se obtiene los registros de analysisConceptAnalysis que complen con la condición de ser igual al parametro analysisCodeId
            var innerQuery = from GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysis in businessCollection where analysisConceptAnalysis.AnalysisId == analysisCodeId select analysisConceptAnalysis.AnalysisConceptId;

            //Obtengo los registros de analysisConcept que no se encuentran dentro de la consulta anterior(not in innerQuery)
            var concepts = from GENERALLEDGEREN.AnalysisConcept analysisConcept in conceptBusinessCollection where !innerQuery.Contains(analysisConcept.AnalysisConceptId) select analysisConcept;

            foreach (GENERALLEDGEREN.AnalysisConcept analysisConceptEntity in concepts)
            {
                AnalysisConceptDTO analysisConcept = DTOAssembler.ToDTO(analysisConceptDAO.GetAnalysisConcept(analysisConceptEntity.AnalysisConceptId));
                analysisConcepts.Add(analysisConcept);
            }

            return analysisConcepts;
        }

        #endregion AnalysisConceptAnalysis

        #region BankReconciliation

        /// <summary>
        /// SaveReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public void SaveReconciliationMovementType(ReconciliationMovementTypeDTO reconciliationMovementType)
        {
            ReconciliationMovementTypeDAO reconciliationMovementTypeDAO = new ReconciliationMovementTypeDAO();
            reconciliationMovementTypeDAO.SaveReconciliationMovementType(ModelDTOAssembler.ToModel(reconciliationMovementType));
        }

        /// <summary>
        /// UpdateReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public void UpdateReconciliationMovementType(ReconciliationMovementTypeDTO reconciliationMovementType)
        {
            ReconciliationMovementTypeDAO reconciliationMovementTypeDAO = new ReconciliationMovementTypeDAO();
            reconciliationMovementTypeDAO.UpdateReconciliationMovementType(ModelDTOAssembler.ToModel(reconciliationMovementType));
        }

        /// <summary>
        /// DeleteReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        /// <returns>bool</returns>
        public bool DeleteReconciliationMovementType(ReconciliationMovementTypeDTO reconciliationMovementType)
        {
            ReconciliationMovementTypeDAO reconciliationMovementTypeDAO = new ReconciliationMovementTypeDAO();
            return reconciliationMovementTypeDAO.DeleteReconciliationMovementType(ModelDTOAssembler.ToModel(reconciliationMovementType));
        }

        /// <summary>
        /// GetReconciliationMovementTypes
        /// Carga listado de conciliaciones  bancarias
        /// </summary>
        /// <returns>List<ReconciliationMovementTypeDTO></returns>
        public List<ReconciliationMovementTypeDTO> GetReconciliationMovementTypes()
        {
            try
            {
                ReconciliationMovementTypeDAO reconciliationMovementTypeDAO = new ReconciliationMovementTypeDAO();
                return DTOAssembler.ToDTOs(reconciliationMovementTypeDAO.GetReconciliationMovementTypes()).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion BankReconciliation

        #region CostCenter

        /// <summary>
        /// SaveCostCenter
        /// Guarda Centro de costos
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns>CostCenter</returns>
        public CostCenterDTO SaveCostCenter(CostCenterDTO costCenter)
        {
            try
            {
                CostCenterDAO costCenterDAO = new CostCenterDAO();
                costCenter = DTOAssembler.ToDTO(costCenterDAO.SaveCostCenter(ModelDTOAssembler.ToModel(costCenter)));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return costCenter;
        }

        /// <summary>
        /// UpdateCostCenter
        /// Actualiza Centro de costos
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns>CostCenter</returns>
        public CostCenterDTO UpdateCostCenter(CostCenterDTO costCenter)
        {
            try
            {
                CostCenterDAO costCenterDAO = new CostCenterDAO();
                costCenter = DTOAssembler.ToDTO(costCenterDAO.UpdateCostCenter(ModelDTOAssembler.ToModel(costCenter)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return costCenter;
        }

        /// <summary>
        /// DeleteCostCenter
        /// Borra Centro de costos
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns>bool</returns>
        public bool DeleteCostCenter(int costCenterId)
        {
            bool isDeleted;

            try
            {
                CostCenterDAO costCenterDAO = new CostCenterDAO();

                // Se obtiene datos de las cuentas contables asociadas a un centro de costo
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountCostCenter.Properties.CostCenterId, costCenterId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccountCostCenter), criteriaBuilder.GetPredicate()));

                isDeleted = businessCollection.Count == 0 && costCenterDAO.DeleteCostCenter(costCenterId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return isDeleted;
        }

        /// <summary>
        /// GetCostCenter
        /// Obtiene Centro de costos
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns>CostCenter</returns>
        public CostCenterDTO GetCostCenter(CostCenterDTO costCenter)
        {
            CostCenterDAO costCenterDAO = new CostCenterDAO();
            costCenter = DTOAssembler.ToDTO(costCenterDAO.GetCostCenter(ModelDTOAssembler.ToModel(costCenter)));
            costCenter.CostCenterType = GetCostCenterTypeById(costCenter.CostCenterType.CostCenterTypeId);
            return costCenter;
        }

        /// <summary>
        /// GetCostCenters
        /// Obtiene listado de Centros de costos
        /// </summary>
        /// <returns>List<CostCenterDTO></returns>
        public List<CostCenterDTO> GetCostCenters()
        {
            CostCenterDAO costCenterDAO = new CostCenterDAO();

            var costCenters = DTOAssembler.ToDTOs(costCenterDAO.GetCostCenters()).ToList();

            return costCenters.Select(GetCostCenter).ToList();
        }

        #endregion CostCenter

        #region CostCenterType

        /// <summary>
        /// SaveCostCenterType
        /// Guarda Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterType"></param>
        public void SaveCostCenterType(CostCenterTypeDTO costCenterType)
        {
            try
            {
                CostCenterTypeDAO costCenterTypeDAO = new CostCenterTypeDAO();
                DTOAssembler.ToDTO(costCenterTypeDAO.SaveCostCenterType(ModelDTOAssembler.ToModel(costCenterType)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCostCenterType
        /// Actualiza Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterType"></param>
        public void UpdateCostCenterType(CostCenterTypeDTO costCenterType)
        {
            try
            {
                CostCenterTypeDAO costCenterTypeDAO = new CostCenterTypeDAO();
                DTOAssembler.ToDTO(costCenterTypeDAO.UpdateCostCenterType(ModelDTOAssembler.ToModel(costCenterType)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCostCenterType
        /// Borra Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterTypeId"></param>
        /// <returns>bool</returns>
        public bool DeleteCostCenterType(int costCenterTypeId)
        {
            bool isDeleted;

            try
            {
                CostCenterTypeDAO costCenterTypeDAO = new CostCenterTypeDAO();
                isDeleted = costCenterTypeDAO.DeleteCostCenterType(costCenterTypeId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return isDeleted;
        }

        /// <summary>
        /// GetCostCenterTypeById
        /// Obtiene Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterTypeId"></param>
        /// <returns>CostCenterType</returns>
        public CostCenterTypeDTO GetCostCenterTypeById(int costCenterTypeId)
        {
            CostCenterTypeDAO costCenterTypeDAO = new CostCenterTypeDAO();
            return DTOAssembler.ToDTO(costCenterTypeDAO.GetCostCenterTypeById(costCenterTypeId));
        }

        /// <summary>
        /// GetCostCenterTypes
        /// Obtiene listado de Tipos de centro de costos
        /// </summary>
        /// <returns>List<CostCenterTypeDTO></returns>
        public List<CostCenterTypeDTO> GetCostCenterTypes()
        {
            try
            {
                CostCenterTypeDAO costCenterTypeDAO = new CostCenterTypeDAO();
                return DTOAssembler.ToDTOs(costCenterTypeDAO.GetCostCenterTypes()).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion CostCenterType

        #region EntryDestination

        /// <summary>
        /// SaveDestination
        /// Guarda Destino
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns>EntryDestination</returns>
        public EntryDestinationDTO SaveEntryDestination(EntryDestinationDTO entryDestination)
        {
            try
            {
                DestinationDAO destinationDAO = new DestinationDAO();
                entryDestination = DTOAssembler.ToDTO(destinationDAO.SaveEntryDestination(ModelDTOAssembler.ToModel(entryDestination)));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return entryDestination;
        }

        /// <summary>
        /// UpdateDestination
        /// Actualiza Destino
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns>EntryDestination</returns>
        public EntryDestinationDTO UpdateEntryDestination(EntryDestinationDTO entryDestination)
        {
            try
            {
                DestinationDAO destinationDAO = new DestinationDAO();
                entryDestination = DTOAssembler.ToDTO(destinationDAO.UpdateEntryDestination(ModelDTOAssembler.ToModel(entryDestination)));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return entryDestination;
        }

        /// <summary>
        /// DeleteEntryDestination
        /// Borra Destino
        /// </summary>
        /// <param name="entryDestinationId"></param>
        public void DeleteEntryDestination(int entryDestinationId)
        {
            try
            {
                DestinationDAO destinationDAO = new DestinationDAO();
                destinationDAO.DeleteEntryDestination(entryDestinationId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetDestination
        /// Obtiene un Destino
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns>EntryDestination</returns>
        public EntryDestinationDTO GetDestination(EntryDestinationDTO entryDestination)
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            return DTOAssembler.ToDTO(destinationDAO.GetEntryDestination(ModelDTOAssembler.ToModel(entryDestination)));
        }

        /// <summary>
        /// GetEntryDestinations
        /// Obtiene listado de Destinos
        /// </summary>
        /// <returns>List<EntryDestination/></returns>
        public List<EntryDestinationDTO> GetEntryDestinations()
        {
            DestinationDAO destinationDAO = new DestinationDAO();
            return DTOAssembler.ToDTOs(destinationDAO.GetEntryDestinations()).ToList();
        }

        #endregion Destination

        #region EntryType

        /// <summary>
        /// SaveEntryType
        /// Guarda Asiento Tipo
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns>EntryType</returns>
        public EntryTypeDTO SaveEntryType(EntryTypeDTO entryType)
        {
            try
            {
                EntryTypeDAO entryTypeDAO = new EntryTypeDAO();
                entryType = DTOAssembler.ToDTO(entryTypeDAO.SaveEntryType(ModelDTOAssembler.ToModel(entryType)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return entryType;
        }

        /// <summary>
        /// UpdateEntryType
        /// Actualiza Asiento Tipo
        /// </summary>
        /// <param name="entryType"></param>
        public void UpdateEntryType(EntryTypeDTO entryType)
        {
            try
            {
                EntryTypeDAO entryTypeDAO = new EntryTypeDAO();
                DTOAssembler.ToDTO(entryTypeDAO.UpdateEntryType(ModelDTOAssembler.ToModel(entryType)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteEntryTypeRequest
        /// Borra Asiento Tipo
        /// </summary>
        /// <param name="entryTypeId"></param>
        public void DeleteEntryTypeRequest(int entryTypeId)
        {
            try
            {
                EntryTypeDAO entryTypeDAO = new EntryTypeDAO();

                // Se obtiene las cuentas relacionadas al tipo de asiento
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryTypeItem.Properties.EntryTypeId, entryTypeId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryTypeItem), criteriaBuilder.GetPredicate()));

                // Se borra las cuentas relacionadas
                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.EntryTypeItem entryTypeItemEntity in businessCollection.OfType<GENERALLEDGEREN.EntryTypeItem>())
                    {
                        entryTypeDAO.DeleteEntryTypeItem(entryTypeItemEntity.EntryTypeItemId);
                    }
                }

                // Se borra el asiento
                EntryTypeDTO entryType = new EntryTypeDTO();
                entryType.EntryTypeId = entryTypeId;

                entryTypeDAO.DeleteEntryType(ModelDTOAssembler.ToModel(entryType));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryType
        /// Obtiene Asiento Tipo
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns>EntryType</returns>
        public EntryTypeDTO GetEntryType(EntryTypeDTO entryType)
        {
            EntryTypeDAO entryTypeDAO = new EntryTypeDAO();
            EntryTypeAccountingDAO entryTypeAccountingDAO = new EntryTypeAccountingDAO();
            var criteriaBuilder = new ObjectCriteriaBuilder();
            var entryTypeItems = new List<EntryTypeItemDTO>();

            try
            {
                var newEntryType = DTOAssembler.ToDTO(entryTypeDAO.GetEntryType(ModelDTOAssembler.ToModel(entryType)));

                if (entryType != null)
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryTypeItem.Properties.EntryTypeId, entryType.EntryTypeId);
                }
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryTypeItem), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count != 0)
                {
                    foreach (GENERALLEDGEREN.EntryTypeItem entryTypeItemEntity in businessCollection.OfType<GENERALLEDGEREN.EntryTypeItem>())
                    {
                        var entryTypeItem = new EntryTypeItemDTO();
                        entryTypeItem.Id = entryTypeItemEntity.EntryTypeItemId;
                        entryTypeItem = DTOAssembler.ToDTO(entryTypeAccountingDAO.GetEntryTypeItem(ModelDTOAssembler.ToModel(entryTypeItem)));
                        entryTypeItems.Add(entryTypeItem);
                    }
                }
                newEntryType.EntryTypeItems = entryTypeItems;
                return newEntryType;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryTypes
        /// Obtiene listado de Asientos Tipo
        /// </summary>
        /// <returns>List<EntryType></returns>
        public List<EntryTypeDTO> GetEntryTypes()
        {
            try
            {
                EntryTypeDAO entryTypeDAO = new EntryTypeDAO();
                return DTOAssembler.ToDTOs(entryTypeDAO.GetEntryTypes()).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion EntryType

        #region EntryTypeItem

        /// <summary>
        /// SaveLedgerEntryItem
        /// Guarda Cuenta contable relacionada al Asiento tipo
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <returns>int</returns>
        public int SaveLedgerEntryItem(LedgerEntryItemDTO ledgerEntryItem)
        {
            return ledgerEntryItem.Id;
        }

        /// <summary>
        /// UpdateLedgerEntryItem
        /// Actualiza Cuenta contable relacionada al Asiento tipo
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <returns>int</returns>
        public int UpdateLedgerEntryItem(LedgerEntryItemDTO ledgerEntryItem)
        {
            return ledgerEntryItem.Id;
        }

        /// <summary>
        /// DeleteEntryTypeAccounting
        /// Borra Cuenta contable relacionada al Asiento tipo
        /// </summary>
        /// <param name="entryTypeAccountingId"></param>
        /// <returns>bool</returns>
        public bool DeleteEntryTypeAccounting(int entryTypeAccountingId)
        {
            bool isDeleted;

            try
            {
                EntryTypeAccountingDAO entryTypeAccountingDAO = new EntryTypeAccountingDAO();
                isDeleted = entryTypeAccountingDAO.DeleteEntryTypeAccounting(entryTypeAccountingId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return isDeleted;
        }

        #endregion EntryTypeItem

        #region EntryNumber

        #endregion EntryNumber

        #region CorrelativeNumber

        /// <summary>
        /// GetCorrelativeNumber
        /// Obtiene el número correlativo para la tabla Entry Análisis 
        /// </summary>
		/// <param name="analysisCodeId"></param>
		/// <param name="analysisConceptId"></param>
		/// <param name="key"></param>
        /// <returns>int</returns>
        public int GetCorrelativeNumber(int analysisCodeId, int analysisConceptId, string key)
        {
            int correlativeNumber = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisId, analysisCodeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisConceptId, analysisConceptId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.ConceptKey, key);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                var maxNumber = (from GENERALLEDGEREN.AnalysisEntryItem entryAnalisis in businessCollection select entryAnalisis.CorrelativeNumber).Max();
                correlativeNumber = Convert.ToInt32(maxNumber);
            }
            else
            {
                correlativeNumber = 1;
            }

            return correlativeNumber;
        }

        #endregion CorrelativeNumber

        #region ConceptSource

        /// <summary>
        /// SaveConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>ConceptSource</returns>
        public ConceptSourceDTO SaveConceptSource(ConceptSourceDTO conceptSource)
        {
            try
            {
                ConceptSourceDAO conceptSourceDAO = new ConceptSourceDAO();
                conceptSource = DTOAssembler.ToDTO(conceptSourceDAO.SaveConceptSource(ModelDTOAssembler.ToModel(conceptSource)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return conceptSource;
        }

        /// <summary>
        /// UpdateConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>ConceptSource</returns>
        public ConceptSourceDTO UpdateConceptSource(ConceptSourceDTO conceptSource)
        {
            try
            {
                ConceptSourceDAO conceptSourceDAO = new ConceptSourceDAO();
                conceptSource = DTOAssembler.ToDTO(conceptSourceDAO.UpdateConceptSource(ModelDTOAssembler.ToModel(conceptSource)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return conceptSource;
        }

        /// <summary>
        /// DeleteConceptSource
        /// </summary>
        /// <param name="conceptSource">conceptSource</param>
        /// <returns>bool</returns>
        public bool DeleteConceptSource(ConceptSourceDTO conceptSource)
        {
            bool deleted;
            try
            {
                ConceptSourceDAO conceptSourceDAO = new ConceptSourceDAO();
                deleted = conceptSourceDAO.DeleteConceptSource(ModelDTOAssembler.ToModel(conceptSource));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return deleted;
        }

        /// <summary>
        /// GetConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>ConceptSource</returns>
        public ConceptSourceDTO GetConceptSource(ConceptSourceDTO conceptSource)
        {
            ConceptSourceDAO conceptSourceDAO = new ConceptSourceDAO();
            return DTOAssembler.ToDTO(conceptSourceDAO.GetConceptSource(ModelDTOAssembler.ToModel(conceptSource)));
        }

        /// <summary>
        /// GetConceptSources
        /// </summary>
        /// <returns>List<ConceptSource/></returns>
        public List<ConceptSourceDTO> GetConceptSources()
        {
            ConceptSourceDAO conceptSourceDAO = new ConceptSourceDAO();
            return DTOAssembler.ToDTOs(conceptSourceDAO.GetConceptSources()).ToList();
        }

        #endregion

        #region MovementType

        /// <summary>
        /// SaveMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>MovementType</returns>
        public MovementTypeDTO SaveMovementType(MovementTypeDTO movementType)
        {
            try
            {
                MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
                movementType = DTOAssembler.ToDTO(movementTypeDAO.SaveMovementType(ModelDTOAssembler.ToModel(movementType)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return movementType;
        }

        /// <summary>
        /// UpdateMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>MovementType</returns>
        public MovementTypeDTO UpdateMovementType(MovementTypeDTO movementType)
        {
            try
            {
                MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
                movementType = DTOAssembler.ToDTO(movementTypeDAO.UpdateMovementType(ModelDTOAssembler.ToModel(movementType)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return movementType;
        }


        /// <summary>
        /// DeleteMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>bool</returns>
        public bool DeleteMovementType(MovementTypeDTO movementType)
        {

            bool deleted;
            try
            {
                MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
                deleted = movementTypeDAO.DeleteMovementType(ModelDTOAssembler.ToModel(movementType));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return deleted;
        }

        /// <summary>
        /// GetMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>MovementType</returns>      
        public MovementTypeDTO GetMovementType(MovementTypeDTO movementType)
        {
            MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
            return DTOAssembler.ToDTO(movementTypeDAO.GetMovementType(ModelDTOAssembler.ToModel(movementType)));
        }

        /// <summary>
        /// GetMovementTypes
        /// </summary>
        /// <returns>List<MovementType/></returns>
        public List<MovementTypeDTO> GetMovementTypes()
        {
            MovementTypeDAO movementTypeDAO = new MovementTypeDAO();

            return DTOAssembler.ToDTOs(movementTypeDAO.GetMovementTypes()).ToList();
        }

        /// <summary>
        /// GetMovementTypesByConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>List<MovementType/></returns>
        public List<MovementTypeDTO> GetMovementTypesByConceptSource(ConceptSourceDTO conceptSource)
        {
            MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
            return DTOAssembler.ToDTOs(movementTypeDAO.GetMovementTypesByConceptSource(ModelDTOAssembler.ToModel(conceptSource))).ToList();
        }

        /// <summary>
        /// GetMovementTypesByConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>List<MovementType/></returns>
        public List<MovementTypeDTO> GetMovementTypesByConceptSourceFilter(ConceptSourceDTO conceptSource)
        {
            MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
            return DTOAssembler.ToDTOs(movementTypeDAO.GetMovementTypesByConceptSourceFilter(ModelDTOAssembler.ToModel(conceptSource))).ToList();
        }

        #endregion

        #region AccountingConcept

        /// <summary>
        /// SaveAccountingConcept
        /// Guardar Conceptos Contables 
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>AccountingConcept</returns>
        public AccountingConceptDTO SaveAccountingConcept(AccountingConceptDTO accountingConcept)
        {
            try
            {
                AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
                accountingConcept = DTOAssembler.ToDTO(accountingConceptDAO.SaveAccountingConcept(ModelDTOAssembler.ToModel(accountingConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingConcept;
        }

        /// <summary>
        /// UpdateAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>AccountingConcept</returns>
        public AccountingConceptDTO UpdateAccountingConcept(AccountingConceptDTO accountingConcept)
        {
            try
            {
                AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
                accountingConcept = DTOAssembler.ToDTO(accountingConceptDAO.UpdateAccountingConcept(ModelDTOAssembler.ToModel(accountingConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingConcept;
        }

        /// <summary>
        /// DeleteAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>bool</returns>   
        public bool DeleteAccountingConcept(AccountingConceptDTO accountingConcept)
        {
            bool deleted;
            try
            {
                AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
                deleted = accountingConceptDAO.DeleteAccountingConcept(ModelDTOAssembler.ToModel(accountingConcept));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return deleted;
        }

        /// <summary>
        /// GetAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>AccountingConcept</returns>     
        public AccountingConceptDTO GetAccountingConcept(AccountingConceptDTO accountingConcept)
        {
            AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
            return DTOAssembler.ToDTO(accountingConceptDAO.GetAccountingConcept(ModelDTOAssembler.ToModel(accountingConcept)));
        }

        /// <summary>
        /// GetAccountingConcepts
        /// </summary>        
        /// <returns>List<AccountingConcept/></returns>        
        public List<AccountingConceptDTO> GetAccountingConcepts()
        {
            AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
            return accountingConceptDAO.GetAccountingConcepts().ToDTOs().ToList();
        }

        /// <summary>
        /// GetAccountingConceptsByCriteria
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <returns>List<AccountingConcept/></returns>
        public List<AccountingConceptDTO> GetAccountingConceptsByCriteria(int userId, int branchId, int individualId)
        {
            AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
            return accountingConceptDAO.GetAccountingConceptsByCriteria(userId, branchId, individualId).ToDTOs().ToList();
        }

        /// <summary>
        /// GetAccountingConceptsByCriteria
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>List of accounting concepts</returns>
        public List<AccountingConceptDTO> GetAccountingConceptsByFilter(AccountingAccountFilterDTO filter)
        {
            try
            {
                AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
                return accountingConceptDAO.GetAccountingConceptsByCriteria(filter.ToModel()).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAccountingConceptsByCriteria);
            }
        }

        public List<AccountingConceptDTO> GetAccountingConceptsByBranchId(int branchId)
        {
            AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
            return accountingConceptDAO.GetAccountingConceptsByBranchId(branchId).ToDTOs().ToList();
        }

        #endregion

        #region BranchAccountingConcept

        /// <summary>
        /// SaveBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>BranchAccountingConcept</returns>
        public BranchAccountingConceptDTO SaveBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept)
        {
            try
            {
                BranchAccountingConceptDAO branchAccountingConceptDAO = new BranchAccountingConceptDAO();
                branchAccountingConcept = DTOAssembler.ToDTO(branchAccountingConceptDAO.SaveBranchAccountingConcept(ModelDTOAssembler.ToModel(branchAccountingConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return branchAccountingConcept;
        }

        /// <summary>
        /// UpdateBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>BranchAccountingConcept</returns>
        public BranchAccountingConceptDTO UpdateBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept)
        {
            try
            {
                BranchAccountingConceptDAO branchAccountingConceptDAO = new BranchAccountingConceptDAO();
                branchAccountingConcept = DTOAssembler.ToDTO(branchAccountingConceptDAO.UpdateBranchAccountingConcept(ModelDTOAssembler.ToModel(branchAccountingConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return branchAccountingConcept;
        }

        /// <summary>
        /// DeleteBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>bool</returns>
        public bool DeleteBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept)
        {
            bool deleted;
            try
            {
                BranchAccountingConceptDAO branchAccountingConceptDAO = new BranchAccountingConceptDAO();
                deleted = branchAccountingConceptDAO.DeleteBranchAccountingConcept(ModelDTOAssembler.ToModel(branchAccountingConcept));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return deleted;
        }

        /// <summary>
        /// GetBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>BranchAccountingConcept</returns>
        public BranchAccountingConceptDTO GetBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept)
        {
            BranchAccountingConceptDAO branchAccountingConceptDAO = new BranchAccountingConceptDAO();
            return DTOAssembler.ToDTO(branchAccountingConceptDAO.GetBranchAccountingConcept(ModelDTOAssembler.ToModel(branchAccountingConcept)));
        }

        /// <summary>
        /// GetBranchAccountingConcepts
        /// </summary>
        /// <returns>List<BranchAccountingConcept/></returns>
        public List<BranchAccountingConceptDTO> GetBranchAccountingConcepts()
        {
            BranchAccountingConceptDAO branchAccountingConceptDAO = new BranchAccountingConceptDAO();
            return DTOAssembler.ToDTOs(branchAccountingConceptDAO.GetBranchAccountingConcepts()).ToList();
        }

        /// <summary>
        /// GetBranchAccountingConceptByBranch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns>List<BranchAccountingConcept/></returns>
        public List<BranchAccountingConceptDTO> GetBranchAccountingConceptByBranch(BranchDTO branch)
        {
            BranchAccountingConceptDAO branchAccountingConceptDAO = new BranchAccountingConceptDAO();
            return DTOAssembler.ToDTOs(branchAccountingConceptDAO.GetBranchAccountingConceptByBranch(branch.ToModel())).ToList();
        }

        #endregion

        #region UserBranchAccountingConcept

        /// <summary>
        /// SaveUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>UserBranchAccountingConcept</returns>
        public UserBranchAccountingConceptDTO SaveUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept)
        {
            try
            {
                UserBranchAccountingConceptDAO userBranchAccountingConceptDAO = new UserBranchAccountingConceptDAO();
                userBranchAccountingConcept = DTOAssembler.ToDTO(userBranchAccountingConceptDAO.SaveUserBranchAccountingConcept(ModelDTOAssembler.ToModel(userBranchAccountingConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return userBranchAccountingConcept;
        }

        /// <summary>
        /// UpdateUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>UserBranchAccountingConcept</returns>
        public UserBranchAccountingConceptDTO UpdateUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept)
        {
            try
            {
                UserBranchAccountingConceptDAO userBranchAccountingConceptDAO = new UserBranchAccountingConceptDAO();
                userBranchAccountingConcept = DTOAssembler.ToDTO(userBranchAccountingConceptDAO.UpdateUserBranchAccountingConcept(ModelDTOAssembler.ToModel(userBranchAccountingConcept)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return userBranchAccountingConcept;
        }

        /// <summary>
        /// DeleteUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>bool</returns>
        public bool DeleteUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept)
        {
            bool deleted;
            try
            {
                UserBranchAccountingConceptDAO userBranchAccountingConceptDAO = new UserBranchAccountingConceptDAO();
                deleted = userBranchAccountingConceptDAO.DeleteUserBranchAccountingConcept(ModelDTOAssembler.ToModel(userBranchAccountingConcept));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return deleted;
        }

        /// <summary>
        /// GetUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>UserBranchAccountingConcept</returns>
        public UserBranchAccountingConceptDTO GetUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept)
        {
            UserBranchAccountingConceptDAO userBranchAccountingConceptDAO = new UserBranchAccountingConceptDAO();
            return DTOAssembler.ToDTO(userBranchAccountingConceptDAO.GetUserBranchAccountingConcept(ModelDTOAssembler.ToModel(userBranchAccountingConcept)));
        }

        /// <summary>
        /// GetUserBranchAccountingConcepts
        /// </summary>
        /// <returns>List<UserBranchAccountingConceptDTO></returns>
        public List<UserBranchAccountingConceptDTO> GetUserBranchAccountingConcepts()
        {
            UserBranchAccountingConceptDAO userBranchAccountingConceptDAO = new UserBranchAccountingConceptDAO();
            return DTOAssembler.ToDTOs(userBranchAccountingConceptDAO.GetUserBranchAccountingConcepts()).ToList();
        }

        /// <summary>
        /// GetUserBranchAccountingConceptByUserByBranch
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branch"></param>
        /// <returns>List<UserBranchAccountingConcept/></returns>
        public List<UserBranchAccountingConceptDTO> GetUserBranchAccountingConceptByUserByBranch(int userId, BranchDTO branch)
        {
            UserBranchAccountingConceptDAO userBranchAccountingConceptDAO = new UserBranchAccountingConceptDAO();
            return DTOAssembler.ToDTOs(userBranchAccountingConceptDAO.GetUserBranchAccountingConceptByUserByBranch(userId, ModelDTOAssembler.ToModel(branch))).ToList();
        }

        #endregion


        #endregion PARAM

        #region EntryPosting

        /// <summary>
        /// DaysMonth
        /// Calcula los días del mes de proceso
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        public int DaysMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        /// DeleteProcessEntries
        /// Elimina asientos de mayor marcados como CONTABILIDAD DIARIA
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="movementType"></param>
        public void DeleteProcessEntries(int year, int month, int movementType)
        {
            try
            {
                LedgerEntryDAO ledgerEntryDAO = new LedgerEntryDAO();
                int numberOfDays = DateTime.DaysInMonth(year, month);
                string date = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
                date = date + " 23:59:59";

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(new DateTime(year, month, 1));
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(date);

                if (movementType == 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_DAILY_ACCOUNTING)));
                }
                if (movementType == 1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId, movementType);
                }
                if (movementType == 2)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_AUTOMATIC_ENTRIES)));
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Entry.Properties.IsDailyEntry, 1); //indica que fue mayorizacíon de asientos.
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntry), criteriaBuilder.GetPredicate()));

                LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
                ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.LedgerEntry ledgerEntryEntity in businessCollection.OfType<GENERALLEDGEREN.LedgerEntry>())
                    {
                        ledgerEntryDAO.DeleteLedgerEntryItem(ledgerEntryEntity.LedgerEntryId);
                        ledgerEntryDAO.DeleteLedgerEntry(ledgerEntryEntity.LedgerEntryId);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveProcessEntries
        /// Guarda agrupando por cuenta contable, naturaleza del movimiento, etc
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <param name="isClosure"></param>
        /// <returns>int</returns>
        public int SaveProcessEntries(int year, int month, int userId, DateTime date, int isClosure)
        {
            int saved = 0;
            int rows;

            try
            {
                // Primero borro registros mayorizados anteriormente y que sean asientos automáticos.
                DeleteProcessEntries(year, month, 2);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.GetSummaryEntries.Properties.Date);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(new DateTime(year, month, 1));
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.GetSummaryEntries.Properties.Date);
                criteriaBuilder.LessEqual();
                if (isClosure == 1)
                {
                    criteriaBuilder.Constant(new DateTime(year, month, DaysMonth(year, month), 23, 59, 59));
                }
                else
                {
                    criteriaBuilder.Constant(DateTime.Now);
                }

                UIView accountingEntries = _dataFacadeManager.GetDataFacade().GetView("GetSummaryEntries", criteriaBuilder.GetPredicate(), null, 0, 2147483647, null, true, out rows);

                LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
                ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                if (accountingEntries.Count > 0)
                {
                    foreach (DataRow item in accountingEntries)
                    {
                        //Cabecera
                        ledgerEntry.AccountingCompany = new AccountingCompanyDTO();
                        ledgerEntry.AccountingCompany.AccountingCompanyId = Convert.ToInt32(item["AccountingCompanyId"]);
                        ledgerEntry.AccountingDate = date;
                        ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO();
                        ledgerEntry.AccountingMovementType.AccountingMovementTypeId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_AUTOMATIC_ENTRIES));
                        ledgerEntry.Branch = new BranchDTO();
                        ledgerEntry.Branch.Id = Convert.ToInt32(item["BranchCode"]);
                        ledgerEntry.Description = "";
                        ledgerEntry.EntryDestination = new EntryDestinationDTO();
                        ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_ENTRY_DESTINATION_BOTH));
                        ledgerEntry.Id = 0;
                        ledgerEntry.ModuleDateId = item["AccountingModuleId"] == DBNull.Value ? 0 : Convert.ToInt32(item["AccountingModuleId"]);
                        ledgerEntry.RegisterDate = DateTime.Now;
                        ledgerEntry.SalePoint = new SalePointDTO();
                        ledgerEntry.SalePoint.Id = Convert.ToInt32(item["SalePointCode"]);
                        ledgerEntry.UserId = userId;

                        //Detalle
                        LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                        ledgerEntryItem.Id = 0;
                        ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                        ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(item["AccountingAccountId"]);
                        ledgerEntryItem.AccountingNature = Convert.ToInt32(item["AccountingNature"]) == Convert.ToInt32(AccountingNatures.Credit) ? Convert.ToInt32(AccountingNatures.Credit) : Convert.ToInt32(AccountingNatures.Debit);

                        ledgerEntryItem.Amount = new AmountDTO();
                        ledgerEntryItem.Amount.Value = Convert.ToDecimal(item["TotalAmountValue"]);
                        ledgerEntryItem.Amount.Currency = new CurrencyDTO();
                        ledgerEntryItem.Amount.Currency.Id = Convert.ToInt32(item["CurrencyCode"]);
                        ledgerEntryItem.ExchangeRate = new ExchangeRateDTO();
                        ledgerEntryItem.ExchangeRate.SellAmount = Convert.ToDecimal(item["ExchangeRate"]);
                        ledgerEntryItem.LocalAmount = new AmountDTO();
                        ledgerEntryItem.LocalAmount.Value = Convert.ToDecimal(item["TotalAmountLocalValue"]);
                        ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                        ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                        ledgerEntryItem.ReconciliationMovementType.Id = 0;
                        ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                        ledgerEntryItem.Currency = new CurrencyDTO() { Id = Convert.ToInt32(item["CurrencyCode"]) };
                        ledgerEntryItem.Description = "MAYORIZACIÓN DEL MES " + Convert.ToString(month) + "/" + Convert.ToString(year);
                        ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = Convert.ToInt32(item["IndividualId"]) };
                        ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                        ledgerEntryItem.Receipt = new ReceiptDTO();
                        ledgerEntryItem.Receipt.Date = null;
                        ledgerEntryItem.Receipt.Number = 0;
                        ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                    }

                    saved = SaveLedgerEntry(ledgerEntry);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return saved;
        }

        #endregion EntryPosting

        #region Reports

        #region EntryReport

        /// <summary>
        /// GetEntriesByDate
        /// Obtiene Asientos por fecha
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>List<EntryDTO></returns>
        public List<EntryDTO> GetEntriesByDate(DateTime dateFrom, DateTime dateTo, int branchId)
        {
            List<EntryDTO> entryDTOs = new List<EntryDTO>();

            var year = dateFrom.Year;
            var month = dateFrom.Month;
            var parameters = new NameValue[6];
            parameters[0] = new NameValue("START_YEAR", year);
            parameters[1] = new NameValue("START_MONTH", month);
            parameters[2] = new NameValue("START_DAY", dateFrom.Day);
            parameters[3] = new NameValue("YEAR", dateTo.Year);
            parameters[4] = new NameValue("MONTH", dateTo.Month);
            parameters[5] = new NameValue("DAY", dateTo.Day);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("GL.GET_ENTRIES_DETAIL_PROCEDURE", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow arrayItem in result.Rows)
                {
                    EntryDTO dailyEntryDto = new EntryDTO();
                    dailyEntryDto.AccountingAccountDescription = Convert.ToString(EvalDetails(arrayItem[0], "-"));
                    dailyEntryDto.AccountingAccountId = Convert.ToDecimal(EvalDetails(arrayItem[1], "0"));
                    dailyEntryDto.AccountingCompanyDescription = Convert.ToString(EvalDetails(arrayItem[2], "-"));
                    dailyEntryDto.AccountingCompanyId = Convert.ToInt32(EvalDetails(arrayItem[3], "0"));
                    dailyEntryDto.AccountingNumber = Convert.ToDecimal(EvalDetails(arrayItem[4], "0"));
                    dailyEntryDto.AccountingModuleId = Convert.ToInt32(EvalDetails(arrayItem[5], "0"));
                    dailyEntryDto.AccountingMovementTypeDescription = Convert.ToString(EvalDetails(arrayItem[6], "-"));
                    dailyEntryDto.AccountingMovementTypeId = Convert.ToInt32(EvalDetails(arrayItem[7], "0"));
                    dailyEntryDto.AccountingNature = Convert.ToInt32(EvalDetails(arrayItem[8], "0"));
                    dailyEntryDto.AmountLocalValue = Convert.ToDecimal(EvalDetails(arrayItem[9], "0"));
                    dailyEntryDto.AmountValue = Convert.ToDecimal(EvalDetails(arrayItem[10], "0"));
                    dailyEntryDto.BankReconciliationDescription = Convert.ToString(EvalDetails(arrayItem[11], "-"));
                    dailyEntryDto.BankReconciliationId = Convert.ToInt32(EvalDetails(arrayItem[12], "0"));
                    dailyEntryDto.BranchCd = Convert.ToInt32(EvalDetails(arrayItem[13], "0"));
                    dailyEntryDto.CurrencyCd = Convert.ToInt32(EvalDetails(arrayItem[14], "0"));
                    dailyEntryDto.Date = Convert.ToDateTime(EvalDetails(arrayItem[15], "01/01/1900"));
                    dailyEntryDto.ReceiptDate = Convert.ToDateTime(EvalDetails(arrayItem[16], "01/01/1900"));
                    dailyEntryDto.ReceiptNumber = Convert.ToInt32(EvalDetails(arrayItem[17], "0"));
                    dailyEntryDto.Description = Convert.ToString(EvalDetails(arrayItem[18], "-"));
                    dailyEntryDto.EntryDestinationId = Convert.ToInt32(EvalDetails(arrayItem[19], "0"));
                    dailyEntryDto.EntryNumber = Convert.ToInt32(EvalDetails(arrayItem[20], "0"));
                    dailyEntryDto.ExchangeRate = Convert.ToDecimal(EvalDetails(arrayItem[21], "0"));
                    dailyEntryDto.IsDailyEntry = Convert.ToBoolean(EvalDetails(arrayItem[22], "0"));

                    dailyEntryDto.SalePointCd = Convert.ToInt32(EvalDetails(arrayItem[23], "0"));
                    dailyEntryDto.EntryId = Convert.ToInt32(EvalDetails(arrayItem[25], "0"));
                    dailyEntryDto.BranchDescription = Convert.ToString(EvalDetails(arrayItem[27], "-"));
                    dailyEntryDto.CurrencyDescription = Convert.ToString(EvalDetails(arrayItem[28], "-"));
                    entryDTOs.Add(dailyEntryDto);
                }
            }

            return entryDTOs;
        }

        #endregion


        /// <summary>
        /// GetDailyEntriesByRangeDateBranchId
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>List<EntryDTO></returns>
        public List<EntryDTO> GetDailyEntriesByRangeDateBranchId(DateTime dateFrom, DateTime dateTo, int branchId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(GENERALLEDGEREN.GetDailyEntriesDetail.Properties.Dailyentryheaderdate);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(dateFrom);
            criteriaBuilder.And();
            criteriaBuilder.Property(GENERALLEDGEREN.GetDailyEntriesDetail.Properties.Dailyentryheaderdate);
            criteriaBuilder.LessEqual();
            criteriaBuilder.Constant(dateTo);

            if (branchId != 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.GetDailyEntriesDetail.Properties.Branchcd);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(branchId);
            }

            List<EntryDTO> entries = new List<EntryDTO>();

            BusinessCollection dailyEntries = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                                              SelectObjects(typeof(GENERALLEDGEREN.GetDailyEntriesDetail),
                                              criteriaBuilder.GetPredicate()));
            if (dailyEntries.Count > 0)
            {
                foreach (GENERALLEDGEREN.GetDailyEntriesDetail item in dailyEntries.OfType<GENERALLEDGEREN.GetDailyEntriesDetail>())
                {
                    EntryDTO dailyEntryDto = new EntryDTO();
                    dailyEntryDto.DailyEntryId = item.Dailyentryid;
                    dailyEntryDto.AccountingAccountDescription = item.Accountingaccountname;
                    dailyEntryDto.AccountingAccountId = Convert.ToDecimal(item.Accountingaccountid);
                    dailyEntryDto.AccountingCompanyDescription = item.Accountingcompanydescription;
                    dailyEntryDto.AccountingCompanyId = Convert.ToInt32(item.Accountingcompanyid);
                    dailyEntryDto.AccountingNumber = Convert.ToDecimal(DBNull.ReferenceEquals(item.Accountingaccountnumber, DBNull.Value) ? 0 : Convert.ToDecimal(item.Accountingaccountnumber));
                    dailyEntryDto.AccountingNature = Convert.ToInt32(item.Accountingnature);
                    dailyEntryDto.AmountLocalValue = Convert.ToDecimal(item.Amountlocalvalue);
                    dailyEntryDto.AmountValue = Convert.ToDecimal(item.Amountvalue);
                    dailyEntryDto.BranchCd = Convert.ToInt32(item.Branchcd);
                    dailyEntryDto.BranchDescription = item.Branchdescription;     // Se aumenta para el reporte
                    dailyEntryDto.CurrencyDescription = item.Currencydescription; // Se aumenta para el reporte
                    dailyEntryDto.CurrencyCd = Convert.ToInt32(item.Currencycd);
                    dailyEntryDto.ReceiptDate = Convert.ToDateTime(item.Dailyentryheaderdate);
                    dailyEntryDto.ReceiptNumber = item.Receiptnumber;
                    dailyEntryDto.Description = item.Description;
                    dailyEntryDto.EntryNumber = Convert.ToInt32(item.Entrynumber);
                    dailyEntryDto.DailyEntryHeaderId = Convert.ToInt32(item.Dailyentryheaderid);
                    dailyEntryDto.TransactionNumber = Convert.ToInt32(item.Transactionnumber);
                    dailyEntryDto.ImputationCode = Convert.ToInt32(item.Originidentifycode);
                    dailyEntryDto.DailyEntryHeaderDescription = item.Dailyentryheaderdescription;
                    entries.Add(dailyEntryDto);
                }
            }
            return entries;
        }

        #endregion

        #region BalanceChecking

        /// <summary>
        /// GetBalanceCheckingDTO
        /// Obtiene el balance de cuentas
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<BalanceCheckingDTO></returns>
        public List<BalanceCheckingDTO> GetBalanceCheckingDTO(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                int rows;
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.Entry.Properties.Date);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.Entry.Properties.Date);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);

                UIView entries = _dataFacadeManager.GetDataFacade().GetView("ReportCheking", criteriaBuilder.GetPredicate(), null, 0, 300, null, true, out rows);

                var balances = from row in entries.AsEnumerable()
                               group row by
                               new
                               {
                                   Number = row.Field<string>("AccountNumber"),
                                   AccountName = row.Field<string>("AccountName"),
                               }
                                into g
                               select new
                               {
                                   AccountNumber = g.Key.Number,
                                   DescriptionAccount = g.Key.AccountName,
                                   SumDebit = g.Where(row => row.Field<int>("AccountingNature") == Convert.ToInt32(AccountingNatures.Debit)).Sum(row => row.Field<decimal>("AmountValue")),
                                   SumCredit = g.Where(row => row.Field<int>("AccountingNature") == Convert.ToInt32(AccountingNatures.Credit)).Sum(row => row.Field<decimal>("AmountValue")),
                                   Creditbalance = g.Where(row => row.Field<int>("AccountingNature") == Convert.ToInt32(AccountingNatures.Credit)).Sum(row => row.Field<decimal>("AmountValue")) -
                                                                  g.Where(row => row.Field<int>("AccountingNature") == Convert.ToInt32(AccountingNatures.Debit)).Sum(row => row.Field<decimal>("AmountValue")), //SALDO ACREEDOR
                                   Debitbalance = g.Where(row => row.Field<int>("AccountingNature") == Convert.ToInt32(AccountingNatures.Debit)).Sum(row => row.Field<decimal>("AmountValue")) -
                                                                  g.Where(row => row.Field<int>("AccountingNature") == Convert.ToInt32(AccountingNatures.Credit)).Sum(row => row.Field<decimal>("AmountValue")), //SALDO DEUDOR
                               };

                return balances.Select(data => new BalanceCheckingDTO
                {
                    AccountingAccount = data.AccountNumber,
                    DescriptionAccount = data.DescriptionAccount,
                    SumCredit = data.SumCredit,
                    SumDebit = data.SumDebit,
                    Creditbalance = data.Creditbalance,
                    Debitbalance = data.Debitbalance
                }).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion BalanceCheking

        #region Closing

        /// <summary>
        /// IncomeOutcomeClosing
        /// Cierre de Ingresos y Egresos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int IncomeOutcomeClosing(int year, int userId)
        {
            ClosingDAO closingDAO = new ClosingDAO();
            return closingDAO.IncomeOutcomeClosing(year, userId);
        }

        /// <summary>
        /// MonthlyIncomeClosing
        /// Cierre de utilidad mensual - Cierre de Activo y Pasivo
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        public int MonthlyIncomeClosing(int year, int userId, int month)
        {
            ClosingDAO closingDAO = new ClosingDAO();
            return closingDAO.MonthlyIncomeClosing(year, userId, month);
        }

        /// <summary>
        /// AssetAndLiabilityOpening
        /// Asiento de Apertura de Activos y Pasivos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int AssetAndLiabilityOpening(int year, int userId)
        {
            ClosingDAO closingDAO = new ClosingDAO();
            return closingDAO.AssetAndLiabilityOpening(year, userId);
        }

        /// <summary>
        /// RevertAnualEntryOpening
        /// Revesión de Asiento Anual de Apertura
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int RevertAnualEntryOpening(int year, int userId)
        {
            ClosingDAO closingDAO = new ClosingDAO();
            return closingDAO.RevertAnualEntryOpening(year, userId);
        }

        /// <summary>
        /// RevertIncomeOutcomeClosing
        /// Reversar Cierre Anual de Ingresos y Egresos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int RevertIncomeOutcomeClosing(int year, int userId)
        {
            ClosingDAO closingDAO = new ClosingDAO();
            return closingDAO.RevertIncomeOutcomeClosing(year, userId);
        }

        #endregion Closing

        #region EntryMassiveLoad

        /// <summary>
        /// SaveEntryMassiveLoadRequest 
        /// </summary>
        /// <param name="massiveEntryDTO"></param>
        /// <returns>bool</returns>
        public bool SaveEntryMassiveLoadRequest(MassiveEntryDTO massiveEntryDTO)
        {
            bool isSaved = false;

            try
            {
                EntryMassiveLoadDAO entryMassiveLoadDAO = new EntryMassiveLoadDAO();
                massiveEntryDTO = entryMassiveLoadDAO.SaveEntryMassiveLoad(massiveEntryDTO);
                isSaved = (massiveEntryDTO.Id > 0);
            }
            catch
            {
                isSaved = false;
            }

            return isSaved;
        }

        /// <summary>
        /// ClearEntryMassiveLoad
        /// </summary>
        public void ClearEntryMassiveLoad()
        {
            try
            {
                EntryMassiveLoadDAO entryMassiveLoadDAO = new EntryMassiveLoadDAO();
                List<MassiveEntryDTO> massiveEntries = entryMassiveLoadDAO.GetEntryMassiveLoads();

                if (massiveEntries.Count > 0)
                {
                    foreach (MassiveEntryDTO massiveEntryDTO in massiveEntries)
                    {
                        entryMassiveLoadDAO.DeleteEntryMassiveLoad(massiveEntryDTO.Id);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DisableEntryMassiveLoadLog
        /// Deshabilita todos los registros de la tabla de log de asientos masivos.
        /// </summary>
        public void DisableEntryMassiveLoadLog()
        {
            try
            {
                EntryMassiveLoadLogDAO entryMassiveLoadLogDAO = new EntryMassiveLoadLogDAO();
                // Se obiene todos los registros de la tabla de log
                List<MassiveEntryLogDTO> massiveEntryLogs = entryMassiveLoadLogDAO.GetEntryMassiveLoadLogs();

                if (massiveEntryLogs.Count > 0)
                {
                    foreach (MassiveEntryLogDTO massiveEntryLogDTO in massiveEntryLogs)
                    {
                        MassiveEntryLogDTO massiveEntryLogDtoNew = entryMassiveLoadLogDAO.GetEntryMassiveLoadLog(massiveEntryLogDTO);
                        massiveEntryLogDtoNew.Enabled = false;
                        entryMassiveLoadLogDAO.UpdateEntryMassiveLoadLog(massiveEntryLogDtoNew);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveEntryMassiveLoadLogRequest
        /// graba el regsitro en la tabla de log
        /// </summary>
        /// <param name="massiveEntryLogDTO"></param>
        /// <returns>MassiveEntryLogDTO</returns>
        public MassiveEntryLogDTO SaveEntryMassiveLoadLogRequest(MassiveEntryLogDTO massiveEntryLogDTO)
        {
            EntryMassiveLoadLogDAO entryMassiveLoadLogDAO = new EntryMassiveLoadLogDAO();
            return entryMassiveLoadLogDAO.SaveEntryMassiveLoadLog(massiveEntryLogDTO);
        }

        /// <summary>
        /// GetEntryMassiveLoadRecords
        /// Obtienes los totales del proceso masivo de asiento
        /// </summary>
        /// <returns>EntryMassiveLoadResultDTO</returns>
        public EntryMassiveLoadResultDTO GetEntryMassiveLoadRecords()
        {
            int totalRecords = 0;
            int successfulRecords = 0;
            int failedRecords = 0;
            EntryMassiveLoadResultDTO entryMassiveLoadResultDTO = new EntryMassiveLoadResultDTO();

            List<MassiveEntryLogDTO> massiveEntryLogs = new List<MassiveEntryLogDTO>();

            try
            {
                EntryMassiveLoadLogDAO entryMassiveLoadLogDAO = new EntryMassiveLoadLogDAO();
                // Total de movimientos procesados
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.EntryMassiveLoad.Properties.EntryMassiveLoadId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection totalCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryMassiveLoad), criteriaBuilder.GetPredicate()));

                if (totalCollection.Count > 0)
                {
                    totalRecords = Convert.ToInt32(totalCollection.Count);
                }

                // Se obtiene los registros habilitados de la tabla de log.
                massiveEntryLogs = entryMassiveLoadLogDAO.GetEntryMassiveLoadLogs();

                if (massiveEntryLogs.Count > 0)
                {
                    var successQuery = from MassiveEntryLogDTO successMassiveEntryRecords in massiveEntryLogs where successMassiveEntryRecords.Enabled == Convert.ToBoolean(1) && successMassiveEntryRecords.Success == Convert.ToBoolean(1) select successMassiveEntryRecords;
                    if (successQuery.Any())
                    {
                        successfulRecords = Convert.ToInt32(successQuery.Count());
                    }

                    var failedQuery = from MassiveEntryLogDTO successMassiveEntryRecords in massiveEntryLogs where successMassiveEntryRecords.Enabled == Convert.ToBoolean(1) && successMassiveEntryRecords.Success == Convert.ToBoolean(0) select successMassiveEntryRecords;

                    if (failedQuery.Any())
                    {
                        failedRecords = Convert.ToInt32(failedQuery.Count());
                    }
                }

                entryMassiveLoadResultDTO.TotalRecords = totalRecords;
                entryMassiveLoadResultDTO.SuccessfulRecords = successfulRecords;
                entryMassiveLoadResultDTO.FailedRecords = failedRecords;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return entryMassiveLoadResultDTO;
        }

        /// <summary>
        /// GetMassiveEntryFailedRecords
        /// Obtiene el listado de registros fallidos para mostrarlo en pantalla
        /// </summary>
        /// <returns>List<MassiveEntryLogDTO></returns>
        public List<MassiveEntryLogDTO> GetMassiveEntryFailedRecords()
        {
            List<MassiveEntryLogDTO> massiveEntryLogs = new List<MassiveEntryLogDTO>();

            try
            {
                EntryMassiveLoadLogDAO entryMassiveLoadLogDAO = new EntryMassiveLoadLogDAO();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GENERALLEDGEREN.EntryMassiveLoadLog.Properties.Enabled, 1);
                filter.And();
                filter.PropertyEquals(GENERALLEDGEREN.EntryMassiveLoadLog.Properties.Success, 0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryMassiveLoadLog), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.EntryMassiveLoadLog entryMassiveLoadLogEntity in businessCollection.OfType<GENERALLEDGEREN.EntryMassiveLoadLog>())
                    {
                        MassiveEntryLogDTO massiveEntryLogDTO = new MassiveEntryLogDTO();
                        massiveEntryLogDTO.Id = entryMassiveLoadLogEntity.Id;
                        massiveEntryLogDTO = entryMassiveLoadLogDAO.GetEntryMassiveLoadLog(massiveEntryLogDTO);
                        massiveEntryLogs.Add(massiveEntryLogDTO);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return massiveEntryLogs;
        }

        /// <summary>
        /// GenerateEntry
        /// Genera y graba el asiento desde carga masiva
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int GenerateEntry(int userId)
        {
            int entryNumber = 0;

            try
            {
                EntryMassiveLoadDAO entryMassiveLoadDAO = new EntryMassiveLoadDAO();
                // Se obtiene los registros de la tabla de trabajo.
                List<MassiveEntryDTO> massiveEntries = entryMassiveLoadDAO.GetEntryMassiveLoads();

                // Se arma el asiento
                if (massiveEntries.Count > 0)
                {
                    LedgerEntryDTO ledgerEntry = GenerateLedgerEntryFromMassiveEntryList(massiveEntries, userId);
                    entryNumber = SaveLedgerEntry(ledgerEntry);

                    //Se elimina la data de la tabla de trabajo
                    ClearEntryMassiveLoad();
                }
            }
            catch (BusinessException ex)
            {
                if (ex.Message.Contains("BusinessException"))
                {
                    throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_BUSINESS_EXCEPTION_MSJ).ToString());
                }

                throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_UNHANDLED_EXCEPTION_MSJ).ToString());
            }

            return entryNumber;
        }

        /// <summary>
        /// Obtiene el listado de operadores
        /// </summary>
        /// <returns>List<OperatorDTO/></returns>
        public List<OperatorDTO> GetOperators()
        {
            OperatorDAO operatorDAO = new OperatorDAO();
            return operatorDAO.GetOperators();
        }

        #endregion EntryMassiveLoad

        #region TempEntry

        /// <summary>
        /// ClearTempAccountEntry
        /// la temporal de generación de asientos.
        /// </summary>
        public void ClearTempAccountEntry()
        {
            try
            {
                TempEntryGenerationDAO tempEntryGenerationDAO = new TempEntryGenerationDAO();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.TempEntryGeneration.Properties.TempEntryGenerationId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.TempEntryGeneration), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.TempEntryGeneration tempEntryGeneration in businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>())
                    {
                        tempEntryGenerationDAO.DeleteTempEntryGeneration(tempEntryGeneration.TempEntryGenerationId);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveTempEntryItem
        /// Graba los movimientos del asiento en una tabla temporal
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="transactionNumber"></param>
        /// <param name="isJournalEntry"></param>
        /// <param name="userId"></param>
        public void SaveTempEntryItem(LedgerEntryDTO ledgerEntry, int transactionNumber, bool isJournalEntry, int userId)
        {
            if (ledgerEntry.LedgerEntryItems.Count > 0)
            {
                foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                {
                    TempEntryGenerationDAO tempEntryGenerationDAO = new TempEntryGenerationDAO();
                    tempEntryGenerationDAO.SaveTempEntryGeneration(ModelDTOAssembler.ToModel(ledgerEntry), ModelDTOAssembler.ToModel(ledgerEntryItem), transactionNumber, isJournalEntry, userId);
                }
            }
        }

        /// <summary>
        /// SaveTempEntry
        /// Método para generar el asiento en cierres.
        /// </summary>
        /// <param name="transactionNumber"></param>
        /// <param name="sourceId"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int SaveTempEntry(int technicalTransaction, int sourceId, string description, int userId)
        {
            // Se obtiene todos los movimientos por número de transacción.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.TempEntryGeneration.Properties.TransactionNumber, technicalTransaction);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.TempEntryGeneration), criteriaBuilder.GetPredicate()));

            int entryNumber = 0;

            // Si envía sourceId es asiento de diario
            if (sourceId == 0)
            {
                LedgerEntryDTO ledgerEntry = GenerateTempLedgerEntry(businessCollection, userId);
                entryNumber = SaveLedgerEntry(ledgerEntry);
            }
            else
            {
                JournalEntryDTO journalEntry = GenerateTempJournalEntry(businessCollection, description, userId, technicalTransaction);
                entryNumber = SaveJournalEntry(journalEntry);
            }

            return entryNumber;
        }

        #endregion TempEntry

        #region AccountReclassification

        /// <summary>
        /// SaveAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns>AccountReclassification</returns>
        public AccountReclassificationDTO SaveAccountReclassification(AccountReclassificationDTO accountReclassification)
        {
            AccountReclassificationDAO accountReclassificationDAO = new AccountReclassificationDAO();
            return DTOAssembler.ToDTO(accountReclassificationDAO.SaveAccountReclassification(ModelDTOAssembler.ToModel(accountReclassification)));
        }

        /// <summary>
        /// UpdateAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns>AccountReclassification</returns>
        public AccountReclassificationDTO UpdateAccountReclassification(AccountReclassificationDTO accountReclassification)
        {
            AccountReclassificationDAO accountReclassificationDAO = new AccountReclassificationDAO();
            return DTOAssembler.ToDTO(accountReclassificationDAO.UpdateAccountReclassification(ModelDTOAssembler.ToModel(accountReclassification)));
        }

        /// <summary>
        /// DeleteAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns></returns>
        public void DeleteAccountReclassification(AccountReclassificationDTO accountReclassification)
        {
            AccountReclassificationDAO accountReclassificationDAO = new AccountReclassificationDAO();
            accountReclassificationDAO.DeleteAccountReclassification(ModelDTOAssembler.ToModel(accountReclassification));
        }

        /// <summary>
        /// GetAccountReclassification
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>List<AccountReclassification/></returns>
        public List<AccountReclassificationDTO> GetAccountReclassification(int month, int year)
        {
            AccountReclassificationDAO accountReclassificationDAO = new AccountReclassificationDAO();
            return DTOAssembler.ToDTOs(accountReclassificationDAO.GetAccountReclassificationByMonthAndYear(month, year)).ToList();
        }

        /// <summary>
        /// GenerateAccountReclassification
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>int</returns>
        public int GenerateEntryReclassification(int month, int year)
        {
            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {
                    EntryReclassificationDAO entryReclassificationDAO = new EntryReclassificationDAO();
                    // Se elimina la reclasificación de cuentas contables por mes y año
                    entryReclassificationDAO.DeleteEntryReclassification(month, year);

                    DateTime dateFrom = new DateTime(year, 1, 1);
                    DateTime dateTo = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

                    // Se obtiene la parametrización de reclasificación para el mes y año.
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Reclassification.Properties.ProcessMonth, month).And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Reclassification.Properties.ProcessYear, year);

                    ProcessLogDTO processLog = new ProcessLogDTO()
                    {
                        EndDate = new DateTime(1900, 1, 1),
                        Id = 0,
                        ProcessLogStatus = Convert.ToInt32(ProcessLogStatus.Started),
                        StartDate = DateTime.Now,
                        UserId = 1,
                        Month = month,
                        Year = year,
                    };

                    int rows;
                    var newProcessLog = entryReclassificationDAO.SaveProcessLog(ModelDTOAssembler.ToModel(processLog));

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Reclassification), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (GENERALLEDGEREN.Reclassification reclassificationEntity in businessCollection.OfType<GENERALLEDGEREN.Reclassification>())
                        {
                            // Se obtiene la moneda, centro de costo y sucursal 
                            criteriaBuilder = new ObjectCriteriaBuilder();
                            criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                            criteriaBuilder.GreaterEqual();
                            criteriaBuilder.Constant(dateFrom);
                            criteriaBuilder.And();
                            criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                            criteriaBuilder.LessEqual();
                            criteriaBuilder.Constant(dateTo);
                            criteriaBuilder.And();
                            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.AccountingAccountId, reclassificationEntity.SourceAccountingAccountId);

                            UIView costCenterEntries = _dataFacadeManager.GetDataFacade().GetView("EntryCostCenter", criteriaBuilder.GetPredicate(), null, 0, 2147483647, null, true, out rows);

                            ProcessReclassificationCostCenters(costCenterEntries, dateFrom, dateTo, reclassificationEntity, month, year);
                        }
                    }

                    newProcessLog.ProcessLogStatus = ProcessLogStatus.Finished;
                    entryReclassificationDAO.UpdateProcessLog(newProcessLog);

                    transaction.Complete();

                    return 1;
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message);
                }
            }
        }

        /// <summary>
        /// SaveAccountReclassificationResult
        /// </summary>
        /// <param name="accountReclassificationResult"></param>
        /// <returns>AccountReclassificationResult</returns>
        public AccountReclassificationResultDTO SaveAccountReclassificationResult(AccountReclassificationResultDTO accountReclassificationResult)
        {
            EntryReclassificationDAO entryReclassificationDAO = new EntryReclassificationDAO();
            return DTOAssembler.ToDTO(entryReclassificationDAO.SaveAccountReclassificationResult(ModelDTOAssembler.ToModel(accountReclassificationResult)));
        }

        /// <summary>
        /// GetAccountReclassificationResult
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>List<AccountReclassificationResult/></returns>
        public List<AccountReclassificationResultDTO> GetAccountReclassificationResults(int month, int year)
        {
            EntryReclassificationDAO entryReclassificationDAO = new EntryReclassificationDAO();
            return DTOAssembler.ToDTOs(entryReclassificationDAO.GetAccountingReclassificationByMonthAndYear(month, year)).ToList();
        }

        #endregion AccountReclassification

        #region ProcessLog

        /// <summary>
        /// SaveProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLog</returns>
        public ProcessLogDTO SaveProcessLog(ProcessLogDTO processLog)
        {
            EntryReclassificationDAO entryReclassificationDAO = new EntryReclassificationDAO();
            return DTOAssembler.ToDTO(entryReclassificationDAO.SaveProcessLog(ModelDTOAssembler.ToModel(processLog)));
        }

        /// <summary>
        /// UpdateProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLog</returns>
        public ProcessLogDTO UpdateProcessLog(ProcessLogDTO processLog)
        {
            EntryReclassificationDAO entryReclassificationDAO = new EntryReclassificationDAO();
            return DTOAssembler.ToDTO(entryReclassificationDAO.UpdateProcessLog(ModelDTOAssembler.ToModel(processLog)));
        }

        /// <summary>
        /// GetProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLog</returns>
        public ProcessLogDTO GetProcessLog(ProcessLogDTO processLog)
        {
            EntryReclassificationDAO entryReclassificationDAO = new EntryReclassificationDAO();
            return DTOAssembler.ToDTO(entryReclassificationDAO.GetProcessLog(ModelDTOAssembler.ToModel(processLog)));
        }

        #endregion ProcessLog

        #region AutomaticLedgerEntry

        /// <summary>
        /// SaveAutomaticLedgerEntry
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int SaveAutomaticLedgerEntry(int moduleDateId, DateTime date, int userId)
        {
            AutomaticLedgerEntryDAO automaticLedgerEntryDAO = new AutomaticLedgerEntryDAO();
            return automaticLedgerEntryDAO.SaveAutomaticLedgerEntry(moduleDateId, date, userId);
        }

        #endregion

        #region Accounting

        /// <summary>
        /// Accounting
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int Accounting(int moduleDateId, List<List<DTOs.AccountingRules.ParameterDTO>> parameters, JournalEntryDTO journalEntry)
        {
            int journalEntryId = 0;

            JournalEntryDTO newJournalEntry = journalEntry;

            //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();

            try
            {
                //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                for (int i = 0; i < journalEntry.JournalEntryItems.Count; i++)
                {
                    if (parameters[i].Count > 0)
                    {
                        //se realiza el cálculo de los movimientos.
                        List<JournalEntryItemDTO> entryItems = AssembleAccountingJournalEntryItems(journalEntry.JournalEntryItems[i], moduleDateId, parameters[i]);

                        if (entryItems.Count > 0)
                        {
                            foreach (var entryItem in entryItems)
                            {
                                newJournalEntryItems.Add(entryItem);
                            }
                        }
                    }
                    else
                    {
                        newJournalEntryItems.Add(journalEntry.JournalEntryItems[i]);
                    }
                }

                //se asigna los nuevos detalles generados al asiento.
                newJournalEntry.JournalEntryItems = newJournalEntryItems;

                //Valida débitos y créditos
                if (ValidateJournalEntryDebitsAndCredits(journalEntry.JournalEntryItems))
                {
                    journalEntryId = SaveJournalEntry(newJournalEntry);
                }
                else
                {
                    //asiento descuadrado
                    journalEntryId = 0;
                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores

                //error en grabación de asiento.
                journalEntryId = -2;
            }

            return journalEntryId;
        }

        /// <summary>
        /// Accounting without transaction
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int CreateAccountingTransactionalItems(string accountingJournalEntryParametersCollection, string codeRulePackage)
        {
            int journalEntryId = 0;

            JournalParameterDTO journalEntryParametersDTO = new JournalParameterDTO();

            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalParameterDTO>(accountingJournalEntryParametersCollection);

            string bridgeAccount = Convert.ToString(journalEntryParametersDTO.BridgeAccounting);
            int moduleId = journalEntryParametersDTO.JournalEntry.ModuleDateId;
            if (journalEntryParametersDTO.OriginalGeneralLedger != null)
            {
                bridgeAccount = Convert.ToString(journalEntryParametersDTO.OriginalGeneralLedger.BridgeAccountingId);
                moduleId = journalEntryParametersDTO.OriginalGeneralLedger.ModuleId;
            }


            //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();
            List<JournalEntryItemDTO> entryItems;
            try
            {
                //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                for (int i = 0; i < journalEntryParametersDTO.JournalEntry.JournalEntryItems.Count; i++)
                {
                    if (journalEntryParametersDTO.Parameters[i].Count > 0)
                    {
                        AccountingParameterDTO accountingParameterDTO = new AccountingParameterDTO()
                        {
                            AccountingConceptId = journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].AccountingConcept.ToString()
                        };

                        //se realiza el cálculo de los movimientos.
                        entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i], accountingParameterDTO);

                        if (entryItems.Count > 0)
                        {
                            foreach (var entryItem in entryItems)
                            {
                                newJournalEntryItems.Add(entryItem);
                            }
                        }
                    }
                    else
                    {
                        if (journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].BridgeAccountId > 0)
                        {
                            List<ParameterDTO> itemsParameters = new List<ParameterDTO>();
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].BridgeAccountId) }); //tipo de pago crédito
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].Currency.Id, CultureInfo.InvariantCulture) }); //moneda
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].Amount.Value, CultureInfo.InvariantCulture) }); //valor
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos

                            //se realiza el cálculo de los movimientos.
                            entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i],
                                moduleId, itemsParameters,
                                Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].PackageRuleCodeId));

                            if (entryItems.Count > 0)
                            {
                                foreach (var entryItem in entryItems)
                                {
                                    newJournalEntryItems.Add(entryItem);
                                }
                            }
                        }
                        else
                        {
                            newJournalEntryItems.Add(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i]);
                        }
                    }
                }

                // Se asigna los nuevos detalles generados al asiento.
                journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;
                // Si los movimientos no están balanceados, genera un movimiento por cuenta puenta
                if (!ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    decimal total = newJournalEntryItems.Where(x => x.AccountingNature == (int)AccountingNatures.Debit).Sum(x => x.LocalAmount.Value)
                         - newJournalEntryItems.Where(x => x.AccountingNature == (int)AccountingNatures.Credit).Sum(x => x.LocalAmount.Value);
                    
                    // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                    List<ParameterDTO> itemsParameters = new List<ParameterDTO>();
                    itemsParameters.Add(new ParameterDTO() { Value = bridgeAccount }); //tipo de pago crédito
                    // Currency Code: Local currency
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //moneda
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(total, CultureInfo.InvariantCulture) }); //valor
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                                                                                                            //Detalle con parámetros fijos.
                    JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                    journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    journalEntryItem.Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = 0 }
                    };
                    journalEntryItem.LocalAmount = new AmountDTO()
                    {
                        Value = total
                    };
                    journalEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = 1 };
                    journalEntryItem.Analysis = new List<AnalysisDTO>();
                    journalEntryItem.CostCenters = new List<CostCenterDTO>();
                    journalEntryItem.Currency = new CurrencyDTO() { Id = 0 };
                    journalEntryItem.Description = journalEntryParametersDTO.JournalEntry.Description;
                    journalEntryItem.EntryType = new EntryTypeDTO();
                    journalEntryItem.Id = 0;
                    journalEntryItem.Individual = new IndividualDTO() { IndividualId = journalEntryParametersDTO.JournalEntry.JournalEntryItems[0].Individual.IndividualId };
                    journalEntryItem.PostDated = new List<PostDatedDTO>();
                    journalEntryItem.Receipt = new ReceiptDTO() { Number = 0, Date = null };
                    journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = 0 };
                    journalEntryItem.SourceCode = 0;
                    journalEntryItem.Branch = new BranchDTO() { Id = journalEntryParametersDTO.JournalEntry.Branch.Id };
                    journalEntryItem.SalePoint = new SalePointDTO() { Id = journalEntryParametersDTO.JournalEntry.SalePoint.Id };

                    //se realiza el cálculo de los movimientos.
                    entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, moduleId, itemsParameters, codeRulePackage);

                    if (entryItems.Count > 0)
                    {
                        foreach (var entryItem in entryItems)
                        {
                            newJournalEntryItems.Add(entryItem);
                        }
                    }

                    //se asigna los nuevos detalles generados al asiento.
                    journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;
                }


                //Valida débitos y créditos
                if (ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    journalEntryId = SaveJournalEntryWithoutTransaction(journalEntryParametersDTO.JournalEntry);
                }
                else
                {
                    //asiento descuadrado
                    journalEntryId = 0;
                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores

                //error en grabación de asiento.
                journalEntryId = -2;
            }

            return journalEntryId;
        }

        /// <summary>
        /// Accounting Rejection 
        /// </summary>
        public int AccountingChecking(string accountingJournalEntryParametersCollection)
        {
            int journalEntryId = 0;
            int sourceCode = 0;
            decimal total = 0;
            JournalParameterDTO journalEntryParametersDTO = new JournalParameterDTO();

            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalParameterDTO>(accountingJournalEntryParametersCollection);

            sourceCode = journalEntryParametersDTO.SourceCode;
            //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();
            List<JournalEntryItemDTO> entryItems;
            try
            {
                //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                for (int i = 0; i < journalEntryParametersDTO.JournalEntry.JournalEntryItems.Count; i++)
                {
                    if (journalEntryParametersDTO.Parameters[i].Count > 0)
                    {
                        //se realiza el cálculo de los movimientos.
                        entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i], journalEntryParametersDTO.JournalEntry.ModuleDateId, journalEntryParametersDTO.Parameters[i]);

                        if (entryItems.Count > 0)
                        {
                            foreach (JournalEntryItemDTO entryItem in entryItems)
                            {
                                newJournalEntryItems.Add(entryItem);
                            }
                        }
                    }
                    else
                    {
                        JournalEntryItemDTO item = journalEntryParametersDTO.JournalEntry.JournalEntryItems[i];
                        if (item.AccountingAccount != null && item.AccountingAccount.AccountingAccountId > 0)
                        {
                            item.AccountingAccount = GetAccountingAccount(item.AccountingAccount.AccountingAccountId);
                        }
                        newJournalEntryItems.Add(item);
                    }
                }

                // Se asigna los nuevos detalles generados al asiento.
                journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;

                // Si los movimientos no están balanceados, genera un movimiento por cuenta puenta
                if (!ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    total = newJournalEntryItems.Where(x => x.AccountingNature == (int)AccountingNatures.Credit).Sum(x => x.LocalAmount.Value)
                    - newJournalEntryItems.Where(x => x.AccountingNature == (int)AccountingNatures.Debit).Sum(x => x.LocalAmount.Value);
                    // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                    List<ParameterDTO> itemsParameters = new List<ParameterDTO>();
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.BridgeAccounting) }); //tipo de pago segun criterio
                    // Currency code: local currency
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //moneda
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(total, CultureInfo.InvariantCulture) }); //valor
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                                                                                                            //Detalle con parámetros fijos.
                    JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                    journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    journalEntryItem.Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = 0 }
                    };
                    journalEntryItem.LocalAmount = new AmountDTO()
                    {
                        Value = total
                    };
                    journalEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = 1 };
                    journalEntryItem.Analysis = new List<AnalysisDTO>();
                    journalEntryItem.CostCenters = new List<CostCenterDTO>();
                    journalEntryItem.Currency = new CurrencyDTO() { Id = 0 };
                    journalEntryItem.Description = journalEntryParametersDTO.JournalEntry.Description;
                    journalEntryItem.EntryType = new EntryTypeDTO();
                    journalEntryItem.Id = 0;
                    journalEntryItem.Individual = new IndividualDTO() { IndividualId = journalEntryParametersDTO.JournalEntry.JournalEntryItems[0].Individual.IndividualId };
                    journalEntryItem.PostDated = new List<PostDatedDTO>();
                    journalEntryItem.Receipt = new ReceiptDTO() { Number = 0, Date = null };
                    journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = 0 };
                    journalEntryItem.SourceCode = sourceCode;
                    journalEntryItem.Branch = new BranchDTO() { Id = journalEntryParametersDTO.JournalEntry.Branch.Id };
                    journalEntryItem.SalePoint = new SalePointDTO() { Id = journalEntryParametersDTO.JournalEntry.SalePoint.Id };

                    //se realiza el cálculo de los movimientos.
                    entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, journalEntryParametersDTO.JournalEntry.ModuleDateId, itemsParameters, journalEntryParametersDTO.CodeRulePackage);
                    //depornto toca invertir la naturaleza
                    if (entryItems.Count > 0)
                    {
                        foreach (JournalEntryItemDTO entryItem in entryItems)
                        {
                            newJournalEntryItems.Add(entryItem);
                        }
                    }
                    //se asigna los nuevos detalles generados al asiento.
                    journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;
                }


                //Valida débitos y créditos
                if (ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    journalEntryId = SaveJournalEntryWithoutTransaction(journalEntryParametersDTO.JournalEntry);
                }
                else
                {
                    //asiento descuadrado
                    journalEntryId = 0;
                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores

                //error en grabación de asiento.
                journalEntryId = -2;
            }

            return journalEntryId;
        }

        /// <summary>
        /// Accounting without transaction
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int AccountingWithoutTransactional(string accountingJournalEntryParametersCollection)
        {
            int journalEntryId = 0;

            JournalParameterDTO journalEntryParametersDTO = new JournalParameterDTO();

            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalParameterDTO>(accountingJournalEntryParametersCollection);


            //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();
            List<JournalEntryItemDTO> entryItems;
            try
            {
                //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                for (int i = 0; i < journalEntryParametersDTO.JournalEntry.JournalEntryItems.Count; i++)
                {
                    if (journalEntryParametersDTO.Parameters[i].Count > 0)
                    {
                        //se realiza el cálculo de los movimientos.
                        entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i], journalEntryParametersDTO.JournalEntry.ModuleDateId, journalEntryParametersDTO.Parameters[i]);

                        if (entryItems.Count > 0)
                        {
                            foreach (var entryItem in entryItems)
                            {
                                newJournalEntryItems.Add(entryItem);
                            }
                        }
                    }
                    else
                    {
                        JournalEntryItemDTO item = journalEntryParametersDTO.JournalEntry.JournalEntryItems[i];
                        if (item.AccountingAccount != null && item.AccountingAccount.AccountingAccountId > 0)
                        {
                            item.AccountingAccount = GetAccountingAccount(item.AccountingAccount.AccountingAccountId);
                        }
                        newJournalEntryItems.Add(item);
                    }
                }

                // Se asigna los nuevos detalles generados al asiento.
                journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;

                // Si los movimientos no están balanceados, genera un movimiento por cuenta puenta
                if (!ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    decimal total = newJournalEntryItems.Where(x => x.AccountingNature == (int)AccountingNatures.Debit).Sum(x => x.LocalAmount.Value)
                         - newJournalEntryItems.Where(x => x.AccountingNature == (int)AccountingNatures.Credit).Sum(x => x.LocalAmount.Value);

                    // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                    List<ParameterDTO> itemsParameters = new List<ParameterDTO>();
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.BridgeAccounting) }); //tipo de pago crédito
                    // Currency code: local currency
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //moneda
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(total, CultureInfo.InvariantCulture) }); //valor
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                    itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                                                                                                            //Detalle con parámetros fijos.
                    JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                    journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    journalEntryItem.Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = 0 }
                    };
                    journalEntryItem.LocalAmount = new AmountDTO()
                    {
                        Value = total
                    };
                    journalEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = 1 };
                    journalEntryItem.Analysis = new List<AnalysisDTO>();
                    journalEntryItem.CostCenters = new List<CostCenterDTO>();
                    journalEntryItem.Currency = new CurrencyDTO() { Id = 0 };
                    journalEntryItem.Description = journalEntryParametersDTO.JournalEntry.Description;
                    journalEntryItem.EntryType = new EntryTypeDTO();
                    journalEntryItem.Id = 0;
                    journalEntryItem.Individual = new IndividualDTO() { IndividualId = journalEntryParametersDTO.JournalEntry.JournalEntryItems[0].Individual.IndividualId };
                    journalEntryItem.PostDated = new List<PostDatedDTO>();
                    journalEntryItem.Receipt = new ReceiptDTO() { Number = 0, Date = null };
                    journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = 0 };
                    journalEntryItem.SourceCode = 0;
                    journalEntryItem.Branch = new BranchDTO() { Id = journalEntryParametersDTO.JournalEntry.Branch.Id };
                    journalEntryItem.SalePoint = new SalePointDTO() { Id = journalEntryParametersDTO.JournalEntry.SalePoint.Id };

                    //se realiza el cálculo de los movimientos.
                    entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, journalEntryParametersDTO.JournalEntry.ModuleDateId, itemsParameters, journalEntryParametersDTO.CodeRulePackage);

                    if (entryItems.Count > 0)
                    {
                        foreach (var entryItem in entryItems)
                        {
                            newJournalEntryItems.Add(entryItem);
                        }
                    }

                    //se asigna los nuevos detalles generados al asiento.
                    journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;
                }


                //Valida débitos y créditos
                if (ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    journalEntryId = SaveJournalEntryWithoutTransaction(journalEntryParametersDTO.JournalEntry);
                }
                else
                {
                    //asiento descuadrado
                    journalEntryId = 0;
                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores

                //error en grabación de asiento.
                journalEntryId = -2;
            }

            return journalEntryId;
        }

        public int AccountingPaymentBallot(string accountingJournalEntryParametersCollection)
        {
            JournalParameterDTO journalEntryParametersDTO = new JournalParameterDTO();

            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalParameterDTO>(accountingJournalEntryParametersCollection);

            //Datos de reconciliación bancaria
            ReconciliationMovementTypeDTO bankReconciliation = new ReconciliationMovementTypeDTO()
            {
                Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_BANK_RECONCILIATION_DEPOSIT).ToString())
            };
            ReceiptDTO receipt = new ReceiptDTO()
            {
                Date = journalEntryParametersDTO.ReceiptDate,
                Number = Convert.ToInt32(journalEntryParametersDTO.ReceiptNumber)
            };

            int accountingCompanyId = (from item in GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;
            journalEntryParametersDTO.JournalEntry.AccountingCompany = new AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId };


            #region JournalEntryItem

            //Listado en donde se llevaran los grupos de parametros al servicio
            List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();

            decimal exchangeRateBallot = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(journalEntryParametersDTO.checkBallotAccountingParameter.RegisterDate, journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode).SellAmount;

            //cuenta puente
            if (journalEntryParametersDTO.checkBallotAccountingParameter.BallotAmount > 0) //valor de la boleta
            {

                //// Cálculo de la cuenta contable y la naturaleza
                //// Se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> ballotParameters = new List<ParameterDTO>();
                parametersCollection.Add(ballotParameters);

                //Detalley
                JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                journalEntryItem.Id = 0;
                journalEntryItem.Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode };
                journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                journalEntryItem.AccountingAccount.AccountingAccountId = journalEntryParametersDTO.checkBallotAccountingParameter.PaymentAccountingAccountId;
                journalEntryItem.AccountingAccount.AccountingNature = (int)AccountingNatures.Debit;
                journalEntryItem.AccountingNature = (int)AccountingNatures.Debit;
                journalEntryItem.ReconciliationMovementType = bankReconciliation;
                journalEntryItem.Receipt = receipt;
                journalEntryItem.Description = Resources.Resources.AccountDepositBallot + " " + journalEntryParametersDTO.checkBallotAccountingParameter.BallotNumber;
                journalEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode },
                };
                journalEntryItem.LocalAmount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode },
                };

                journalEntryItem.ExchangeRate = new ExchangeRateDTO()
                {
                    SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(journalEntryParametersDTO.checkBallotAccountingParameter.RegisterDate, journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode).SellAmount
                };


                if (journalEntryItem.Currency.Id == 0)//0 siempre moneda local
                {
                    journalEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(journalEntryParametersDTO.checkBallotAccountingParameter.BallotAmount, CultureInfo.InvariantCulture));
                    journalEntryItem.LocalAmount.Value = Math.Abs(Convert.ToDecimal(journalEntryParametersDTO.checkBallotAccountingParameter.BallotAmount, CultureInfo.InvariantCulture)) * journalEntryItem.ExchangeRate.SellAmount;
                }
                else
                {  //para poder obtener el IncomeAmount original 
                    journalEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(journalEntryParametersDTO.checkBallotAccountingParameter.BallotAmount, CultureInfo.InvariantCulture)) / journalEntryItem.ExchangeRate.SellAmount;
                    journalEntryItem.LocalAmount.Value = journalEntryParametersDTO.checkBallotAccountingParameter.BallotAmount * journalEntryItem.ExchangeRate.SellAmount;
                }
                journalEntryItem.Individual = new IndividualDTO() { IndividualId = journalEntryParametersDTO.IndividualId };
                journalEntryItem.EntryType = new EntryTypeDTO();
                journalEntryItem.Analysis = new List<AnalysisDTO>();
                journalEntryItem.CostCenters = new List<CostCenterDTO>();
                journalEntryItem.PostDated = new List<PostDatedDTO>();
                journalEntryItem.SourceCode = journalEntryParametersDTO.SourceCode;

                journalEntryParametersDTO.JournalEntry.JournalEntryItems.Add(journalEntryItem);
            }
            if (journalEntryParametersDTO.checkBallotAccountingParameter.CashAmount > 0) //valor en efectivo
            {
                // Cálculo de la cuenta contable y la naturaleza
                // Se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> cashParameters = new List<ParameterDTO>();

                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode, CultureInfo.InvariantCulture) }); //moneda
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_DEPOSIT_BALLOT_CASH_VALUE_TYPE_ID).ToString(), CultureInfo.InvariantCulture) }); //tipo de pago
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.BankCode, CultureInfo.InvariantCulture) }); //id de banco
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.AccountNumber, CultureInfo.InvariantCulture) }); //número de cuenta
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //valor de la boleta
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.CashAmount * exchangeRateBallot, CultureInfo.InvariantCulture) }); //valor en efectivo
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //valor de comisión.
                cashParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //valor de los cheques/tarjetas

                parametersCollection.Add(cashParameters);

                //Detalle
                JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                journalEntryItem.Id = 0;
                journalEntryItem.Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode };
                journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                journalEntryItem.ReconciliationMovementType = bankReconciliation;
                journalEntryItem.Receipt = receipt;
                journalEntryItem.Description = Resources.Resources.AccountDepositBallot + " " + journalEntryParametersDTO.checkBallotAccountingParameter.BallotNumber;
                journalEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode }
                };
                journalEntryItem.ExchangeRate = new ExchangeRateDTO()
                {
                    SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(journalEntryParametersDTO.checkBallotAccountingParameter.RegisterDate, journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode).SellAmount
                };
                journalEntryItem.Individual = new IndividualDTO() { IndividualId = 0 };
                journalEntryItem.EntryType = new EntryTypeDTO();
                journalEntryItem.Analysis = new List<AnalysisDTO>();
                journalEntryItem.CostCenters = new List<CostCenterDTO>();
                journalEntryItem.PostDated = new List<PostDatedDTO>();
                journalEntryItem.SourceCode = journalEntryParametersDTO.SourceCode;
                journalEntryParametersDTO.JournalEntry.JournalEntryItems.Add(journalEntryItem);
            }
            if (journalEntryParametersDTO.checkBallotAccountingParameter.CommissionAmount > 0) // Valor de comisiones
            {
                // Cálculo de la cuenta contable y la naturaleza
                // Se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> commissionParameters = new List<ParameterDTO>();

                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode, CultureInfo.InvariantCulture) }); //moneda
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_DEPOSIT_BALLOT_COMISSIONS_VALUE_TYPE_ID).ToString(), CultureInfo.InvariantCulture) }); //tipo de pago
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.BankCode, CultureInfo.InvariantCulture) }); //id de banco
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.AccountNumber, CultureInfo.InvariantCulture) }); //número de cuenta
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); // Valor de la boleta
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); // Valor en efectivo
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.CommissionAmount * exchangeRateBallot, CultureInfo.InvariantCulture) }); //valor de comisión.
                commissionParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //valor de los cheques/tarjetas

                parametersCollection.Add(commissionParameters);

                //Detalle
                JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                journalEntryItem.Id = 0;
                journalEntryItem.Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode };
                journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                journalEntryItem.ReconciliationMovementType = bankReconciliation;
                journalEntryItem.Receipt = receipt;
                journalEntryItem.Description = Resources.Resources.AccountDepositBallot + " " + journalEntryParametersDTO.checkBallotAccountingParameter.BallotNumber;
                journalEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode },
                };
                journalEntryItem.ExchangeRate = new ExchangeRateDTO()
                {
                    SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(journalEntryParametersDTO.checkBallotAccountingParameter.RegisterDate, journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode).SellAmount
                };
                journalEntryItem.Individual = new IndividualDTO() { IndividualId = 0 };
                journalEntryItem.EntryType = new EntryTypeDTO();
                journalEntryItem.Analysis = new List<AnalysisDTO>();
                journalEntryItem.CostCenters = new List<CostCenterDTO>();
                journalEntryItem.PostDated = new List<PostDatedDTO>();
                journalEntryItem.SourceCode = journalEntryParametersDTO.SourceCode;
                journalEntryParametersDTO.JournalEntry.JournalEntryItems.Add(journalEntryItem);
            }
            if (journalEntryParametersDTO.checkBallotAccountingParameter.Amount > 0) //valor de los cheques/tarjetas
            {
                // Cálculo de la cuenta contable y la naturaleza
                // Se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> amountParameters = new List<ParameterDTO>();

                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de ingreso de caja
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode, CultureInfo.InvariantCulture) }); //moneda
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.TypeId, CultureInfo.InvariantCulture) }); //tipo de pago
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.BankCode, CultureInfo.InvariantCulture) }); //id de banco
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.AccountNumber, CultureInfo.InvariantCulture) }); //número de cuenta
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); // Valor de la boleta
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); // Valor en efectivo
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //valor de comisión.
                amountParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.checkBallotAccountingParameter.Amount * exchangeRateBallot, CultureInfo.InvariantCulture) }); //valor de los cheques/tarjetas

                parametersCollection.Add(amountParameters);

                //Detalle
                JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                journalEntryItem.Id = 0;
                journalEntryItem.Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode };
                journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                journalEntryItem.ReconciliationMovementType = bankReconciliation;
                journalEntryItem.Receipt = receipt;
                journalEntryItem.Description = Resources.Resources.AccountDepositBallot + " " + journalEntryParametersDTO.checkBallotAccountingParameter.BallotNumber;
                journalEntryItem.Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode },
                };
                journalEntryItem.ExchangeRate = new ExchangeRateDTO()
                {
                    SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(journalEntryParametersDTO.checkBallotAccountingParameter.RegisterDate, journalEntryParametersDTO.checkBallotAccountingParameter.CurrencyCode).SellAmount
                };
                journalEntryItem.Individual = new IndividualDTO() { IndividualId = 0 };
                journalEntryItem.EntryType = new EntryTypeDTO();
                journalEntryItem.Analysis = new List<AnalysisDTO>();
                journalEntryItem.CostCenters = new List<CostCenterDTO>();
                journalEntryItem.PostDated = new List<PostDatedDTO>();
                journalEntryItem.SourceCode = journalEntryParametersDTO.SourceCode;
                journalEntryParametersDTO.JournalEntry.JournalEntryItems.Add(journalEntryItem);
            }

            #endregion JournalEntryItem



            int journalEntryId = 0;

            JournalEntryDTO newJournalEntry = journalEntryParametersDTO.JournalEntry;

            //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();

            try
            {
                //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                for (int i = 0; i < journalEntryParametersDTO.JournalEntry.JournalEntryItems.Count; i++)
                {
                    if (parametersCollection[i].Count > 0)
                    {
                        journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].SourceCode = 0;
                        //se realiza el cálculo de los movimientos.
                        List<JournalEntryItemDTO> entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i], journalEntryParametersDTO.JournalEntry.ModuleDateId, parametersCollection[i]);

                        if (entryItems.Count > 0)
                        {
                            foreach (JournalEntryItemDTO entryItemPayment in entryItems)
                            {
                                newJournalEntryItems.Add(entryItemPayment);
                            }
                        }
                    }
                    else
                    {
                        newJournalEntryItems.Add(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i]);
                    }
                }


                //se asigna los nuevos detalles generados al asiento.
                newJournalEntry.JournalEntryItems = newJournalEntryItems;

                //Valida débitos y créditos
                if (ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    journalEntryId = SaveJournalEntry(newJournalEntry);
                }
                else
                {
                    //asiento descuadrado
                    journalEntryId = 0;
                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores

                //error en grabación de asiento.
                journalEntryId = -2;
            }

            return journalEntryId;
        }

        public int GetJournalEntryTechnicalTransaction(int technicalTransaction, int technicalTransactionRevertion)
        {
            JournalEntryDTO journalEntry = new JournalEntryDTO();
            journalEntry.TechnicalTransaction = technicalTransaction;

            journalEntry = GetJournalEntryReversion(journalEntry);

            int resultReverse = ReverseJournalEntry(journalEntry, technicalTransactionRevertion);

            return resultReverse;
        }

        public JournalEntryDTO GetJournalEntryReversion(JournalEntryDTO journalEntry)
        {
            try
            {
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                journalEntry = DTOAssembler.ToDTO(journalEntryDAO.GetJournalEntryByTechnicalTransaction(journalEntry.TechnicalTransaction));
                journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.JournalEntryId, typeof(GENERALLEDGEREN.JournalEntryItem).Name, journalEntry.Id);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), filter.GetPredicate()));
                //se llena los movimientos del asiento de diario
                if (businessCollection.Any())
                {
                    foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntryItem>())
                    {
                        JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                        journalEntryItem.Id = journalEntryItemEntity.JournalEntryItemId;
                        journalEntryItem = DTOAssembler.ToDTO(journalEntryItemDAO.GetJournalEntryItem(ModelDTOAssembler.ToModel(journalEntryItem)));
                        journalEntryItem.CostCenters = new List<CostCenterDTO>();
                        journalEntryItem.CostCenters = GetCostCentersByEntryId(journalEntryItemEntity.JournalEntryItemId, true);
                        journalEntryItem.Analysis = new List<AnalysisDTO>();
                        journalEntryItem.Analysis = GetAnalysesByEntryId(journalEntryItemEntity.JournalEntryItemId, true);
                        journalEntryItem.CostCenters = new List<CostCenterDTO>();
                        journalEntryItem.CostCenters = GetCostCentersByEntryId(journalEntryItemEntity.JournalEntryItemId, true);

                        journalEntry.JournalEntryItems.Add(journalEntryItem);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return journalEntry;
        }

        public JournalEntryDTO GetJournalEntryItemsByTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                JournalEntryDTO journalEntry = DTOAssembler.ToDTO(journalEntryItemDAO.GetJournalEntryItemsByTechnicalTransaction(technicalTransaction));
                return journalEntry;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }
        public List<JournalEntryItemDTO> GetJournalEntryItemsBySourceCode(int sourceCode)
        {
            try
            {
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                List<JournalEntryItemDTO> journalEntry = journalEntryItemDAO.GetJournalEntryItemsBySourceCode(sourceCode).ToDTOs().ToList();
                return journalEntry;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }


        #endregion Accounting

        #endregion Public Methods

        #region Private Methods

        #region AccountingAccount

        /// <summary>
        /// Obtiene el nivel de la cuenta contable.
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>AccoutingAccountLevelDTO</returns>
        private AccoutingAccountLevelDTO GetAccountingAccountLevel(AccountingAccountDTO accountingAccount)
        {
            char[] accountNumber = accountingAccount.Number.Trim().ToCharArray();
            string subString = "";
            AccoutingAccountLevelDTO accountingAccountLevel = new AccoutingAccountLevelDTO();

            try
            {
                for (int i = accountNumber.Length - 1; i >= 1; i--)
                {
                    if (accountNumber[i] == '0')
                    {
                        subString = accountingAccount.Number.Substring(0, i);
                    }
                    else
                    {
                        break;
                    }
                }

                // Se obtiene los registros del nivel de la cuenta contable
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccountLevel.Properties.Length);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                int length = subString == "" ? accountingAccount.Number.Trim().Length : subString.Length;

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccountLevel), criteriaBuilder.GetPredicate()));

                var query = (from GENERALLEDGEREN.AccountingAccountLevel item in businessCollection where item.Length == length select item).ToList();

                int rows = query.Count;

                if (rows > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountLevel accountingAccountLevelEntity in query)
                    {
                        accountingAccountLevel.LevelCode = accountingAccountLevelEntity.LevelCode;
                        accountingAccountLevel.Description = accountingAccountLevelEntity.Description;
                        accountingAccountLevel.Length = Convert.ToInt32(accountingAccountLevelEntity.Length);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingAccountLevel;
        }

        /// <summary>
        /// ValidateAccountingAccountNumberNotExists
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>AccountingAccountValidationDTO</returns>
        private AccountingAccountValidationDTO ValidateAccountingAccountNumberDoesNotExist(string accountNumber)
        {
            AccountingAccountValidationDTO validationDTO = new AccountingAccountValidationDTO();
            validationDTO.TypeId = 1;

            AccountingAccountDTO accountingAccount = new AccountingAccountDTO() { Number = accountNumber };

            try
            {
                // Valida que el número de cuenta no exista
                var accountingAccounts = GetAccountingAccountsByNumberDescription(accountingAccount);

                validationDTO.IsSucessful = (accountingAccounts.Count <= 0);
            }
            catch (BusinessException)
            {
                validationDTO.IsSucessful = false;
            }

            return validationDTO;
        }

        /// <summary>
        /// ValidateAccountingAccountBaseNumber
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <param name="accountingAccountParentLevel"></param>
        /// <param name="accountingAccountLevel"></param>
        /// <param name="lengthLevel"></param>
        /// <returns>AccountingAccountValidationDTO</returns>
        private AccountingAccountValidationDTO ValidateAccountingAccountBaseNumber(AccountingAccountDTO accountingAccount, AccoutingAccountLevelDTO accountingAccountParentLevel, AccoutingAccountLevelDTO accountingAccountLevel, int lengthLevel)
        {
            AccountingAccountValidationDTO validationDTO = new AccountingAccountValidationDTO();
            AccountingAccountDTO parentAccount = new AccountingAccountDTO();

            if (lengthLevel > 2)
            {
                parentAccount.Number = accountingAccount.Number.Substring(0, lengthLevel - 2);
                accountingAccountParentLevel = GetAccountingAccountLevel(parentAccount);
            }

            if (accountingAccountLevel.LevelCode - accountingAccountParentLevel.LevelCode != 1)
            {
                validationDTO.TypeId = 2;
                validationDTO.IsSucessful = false;
            }
            else
            {
                validationDTO.IsSucessful = true;
            }

            return validationDTO;
        }

        /// <summary>
        /// AssembleParentAccountForValidation
        /// </summary>
        /// <param name="parentAccount"></param>
        /// <returns>AccountingAccount</returns>
        private AccountingAccountDTO AssembleParentAccountForValidation(AccountingAccountDTO parentAccount)
        {
            int branchStartPosition = 0;
            int prefixStartPosition = 0;

            if (parentAccount.Branch.Id != -1)
            {
                branchStartPosition = parentAccount.Number.Length - 2;
                parentAccount.Number = Convert.ToString(parentAccount.Number).Remove(branchStartPosition, 2);
                parentAccount.Number = parentAccount.Number.Insert(branchStartPosition, "00");
            }
            if (parentAccount.Prefixes.Count > 0)
            {
                prefixStartPosition = parentAccount.Number.Length - 4;
                parentAccount.Number = parentAccount.Number.Remove(prefixStartPosition, 2);
                parentAccount.Number = parentAccount.Number.Insert(prefixStartPosition, "00");
            }

            return parentAccount;
        }

        /// <summary>
        /// DeleteCostCentersByAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>bool</returns>
        private bool DeleteCostCentersByAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            var isDeleted = false;

            try
            {
                AccountingAccountCostCenterDAO accountingAccountCostCenterDAO = new AccountingAccountCostCenterDAO();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountCostCenter.Properties.AccountingAccountId, accountingAccount.AccountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountCostCenter), criteriaBuilder.GetPredicate()));

                // Return como objeto
                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountCostCenter costCenters in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountCostCenter>())
                    {
                        isDeleted = accountingAccountCostCenterDAO.DeleteAccountingAccountCostCenter(costCenters.AccountingAccountCostCenterId);
                    }
                }
                else
                {
                    isDeleted = true;
                }

                return isDeleted;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePrefixesByAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>bool</returns>
        private bool DeletePrefixesByAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            var isDeleted = false;

            try
            {
                AccountingAccountPrefixDAO accountingAccountPrefixDAO = new AccountingAccountPrefixDAO();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountPrefix.Properties.AccountingAccountId, accountingAccount.AccountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountPrefix), criteriaBuilder.GetPredicate()));

                // Return como objeto
                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountPrefix prefixs in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountPrefix>())
                    {
                        isDeleted = accountingAccountPrefixDAO.DeleteAccountingAccountPrefix(prefixs.AccountingAccountPrefixId);
                    }
                }
                else
                {
                    isDeleted = true;
                }

                return isDeleted;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPrefixesByAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>AccountingAccount</returns>
        private AccountingAccountDTO GetPrefixesByAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountPrefix.Properties.AccountingAccountId, accountingAccount.AccountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountPrefix), criteriaBuilder.GetPredicate()));

                if (accountingAccount.Prefixes == null)
                {
                    accountingAccount.Prefixes = new List<PrefixDTO>();
                }

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountPrefix prefixs in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountPrefix>())
                    {
                        accountingAccount.Prefixes.Add(new PrefixDTO() { Id = Convert.ToInt32(prefixs.PrefixCode) });
                    }
                }

                return accountingAccount;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCostCentersByAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>AccountingAccount</returns>
        private AccountingAccountDTO GetCostCentersByAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountCostCenter.Properties.AccountingAccountId, accountingAccount.AccountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountCostCenter), criteriaBuilder.GetPredicate()));

                if (accountingAccount.CostCenters == null)
                {
                    accountingAccount.CostCenters = new List<CostCenterDTO>();
                }

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountCostCenter costCenters in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountCostCenter>())
                    {
                        accountingAccount.CostCenters.Add(new CostCenterDTO() { CostCenterId = Convert.ToInt32(costCenters.CostCenterId) });
                    }
                }

                return accountingAccount;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingAccount

        #region Entry

        /// <summary>
        /// EvalDetails
        /// Metodo Usado para reducir el Cognitivo detectado por SanarQube
        /// Agrupa funcionalidad individual en un Método para reutilizar. 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="typeResult"></param>
        /// <returns>object</returns>
        private object EvalDetails(object valueObject, string typeResult)
        {
            if (ReferenceEquals(valueObject, DBNull.Value))
            {
                return typeResult;
            }
            else
            {
                return valueObject;
            }
        }

        #endregion

        #region JournalEntry

        /// <summary>
        /// SaveItemGroup
        /// Método agrupa los guardar relacionados de JournalEntry 
        /// Centro de Costos, Analisis, PostDated
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <param name="newJournalEntryItem"></param>
        private void SaveItemGroup(JournalEntryItemDTO journalEntryItem, JournalEntryItemDTO newJournalEntryItem)
        {
            AnalysisDAO analysisDAO = new AnalysisDAO();
            PostDatedDAO postDatedDAO = new PostDatedDAO();
            CostCenterEntryDAO costCenterEntryDAO = new CostCenterEntryDAO();
            if (journalEntryItem.CostCenters.Any())
            {
                foreach (CostCenterDTO costCenter in journalEntryItem.CostCenters)
                {
                    costCenterEntryDAO.SaveCostCenterEntry(ModelDTOAssembler.ToModel(costCenter), newJournalEntryItem.Id, true);
                }
            }

            if (journalEntryItem.Analysis.Any())
            {
                foreach (AnalysisDTO analysis in journalEntryItem.Analysis)
                {
                    int correlativeNumber = GetCorrelativeNumber(analysis.AnalysisId, analysis.AnalysisConcept.AnalysisConceptId, analysis.ConceptKey) + 1;
                    analysisDAO.SaveAnalysis(ModelDTOAssembler.ToModel(analysis), newJournalEntryItem.Id, correlativeNumber, true);
                }
            }

            if (journalEntryItem.PostDated.Any())
            {
                foreach (PostDatedDTO postDated in journalEntryItem.PostDated)
                {
                    postDatedDAO.SavePostDated(ModelDTOAssembler.ToModel(postDated), newJournalEntryItem.Id, true);
                }
            }
        }

        /// <summary>
        /// ValidateJournalEntryDebitsAndCredits
        /// </summary>
        /// <param name="journalEntryItems"></param>
        /// <returns>bool</returns>
        private bool ValidateJournalEntryDebitsAndCredits(List<JournalEntryItemDTO> journalEntryItems)
        {
            bool isValid = false;

            try
            {
                decimal debits = 0;
                decimal credits = 0;

                foreach (JournalEntryItemDTO journalEntryItem in journalEntryItems)
                {
                    if (journalEntryItem.AccountingNature == Convert.ToInt32(AccountingNatures.Debit))
                    {
                        debits = debits + journalEntryItem.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + journalEntryItem.LocalAmount.Value;
                    }
                }

                if ((System.Math.Abs(debits) > 0) && (System.Math.Abs(credits) > 0))
                {
                    isValid = (System.Math.Abs(debits) == System.Math.Abs(credits));
                }
            }
            catch (BusinessException)
            {
                isValid = false;
            }

            return isValid;
        }

        #endregion

        #region JournalEntryItems

        /// <summary>
        /// AssembleAccountingJournalEntryItems
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <returns>List<JournalEntryItemDTO></returns>
        private List<JournalEntryItemDTO> AssembleAccountingJournalEntryItems(JournalEntryItemDTO journalEntryItem, int moduleDateId, List<DTOs.AccountingRules.ParameterDTO> parameters, string codeRulePackage = "")
        {
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();
            try
            {
                EntryParameterApplicationServiceProvider _entryParameterService = new EntryParameterApplicationServiceProvider();
                List<ResultDTO> results = _entryParameterService.ExecuteAccountingRulePackage(moduleDateId, parameters, codeRulePackage);

                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        //Detalle
                        JournalEntryItemDTO newJournalEntryItem = new JournalEntryItemDTO();
                        newJournalEntryItem.AccountingAccount = new AccountingAccountDTO();
                        newJournalEntryItem.AccountingAccount.Number = result.AccountingAccount;
                        var accountings = GetAccountingAccountsByNumberDescription(newJournalEntryItem.AccountingAccount);
                        newJournalEntryItem.AccountingAccount = (accountings != null && accountings.Any()) ?
                            accountings.First() : new AccountingAccountDTO();
                        newJournalEntryItem.AccountingNature = result.AccountingNature;

                        if (!accountings.Any())
                        {
                            new LogJournalEntryItemDAO().SaveJournalEntryItem(new LogJournalEntryItem()
                            {
                                AccountNature = Convert.ToString(result.AccountingNature),
                                AccountNumber = Convert.ToString(result.AccountingAccount),
                                Amount = Convert.ToString(result.Parameter.Value),
                                CodePackagesRules = codeRulePackage,
                                CurrencyId = Convert.ToString(journalEntryItem.Currency.Id),
                                ModuleId = Convert.ToString(moduleDateId),
                                JeiJson = Newtonsoft.Json.JsonConvert.SerializeObject(journalEntryItem)
                            });
                        }

                        newJournalEntryItem.ExchangeRate = journalEntryItem.ExchangeRate;
                        newJournalEntryItem.ExchangeRate.SellAmount = journalEntryItem.ExchangeRate.SellAmount;

                        newJournalEntryItem.Amount = new AmountDTO();
                        newJournalEntryItem.LocalAmount = new AmountDTO();
                        newJournalEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture));
                        if (journalEntryItem.LocalAmount == null)
                        {
                            newJournalEntryItem.LocalAmount.Value = newJournalEntryItem.Amount.Value * journalEntryItem.ExchangeRate.SellAmount;
                        }
                        else
                        {
                            newJournalEntryItem.LocalAmount.Value = Math.Abs(journalEntryItem.LocalAmount.Value);
                        }

                        newJournalEntryItem.Analysis = new List<AnalysisDTO>();
                        newJournalEntryItem.ReconciliationMovementType = journalEntryItem.ReconciliationMovementType;
                        newJournalEntryItem.CostCenters = new List<CostCenterDTO>();
                        newJournalEntryItem.Currency = journalEntryItem.Currency;
                        newJournalEntryItem.Description = journalEntryItem.Description;
                        newJournalEntryItem.EntryType = new EntryTypeDTO();
                        newJournalEntryItem.Id = 0;
                        newJournalEntryItem.Individual = journalEntryItem.Individual;
                        newJournalEntryItem.PostDated = new List<PostDatedDTO>();
                        newJournalEntryItem.Receipt = journalEntryItem.Receipt;
                        newJournalEntryItem.SourceCode = journalEntryItem.SourceCode;
                        newJournalEntryItems.Add(newJournalEntryItem);
                    }
                }

                return newJournalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// AssembleAccountingJournalEntryItems
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <returns></returns>
        private List<JournalEntryItemDTO> AssembleAccountingJournalEntryItems(JournalEntryItemDTO journalEntryItem, AccountingParameterDTO accountingParameterDTO)
        {
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();

            try
            {
                AccountingAccountDTO result = GetAccountingNumberByAccountingConcept(accountingParameterDTO);

                if (result != null && result.Number != "")
                {
                    //Detalle
                    JournalEntryItemDTO newJournalEntryItem = new JournalEntryItemDTO();
                    newJournalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    newJournalEntryItem.AccountingAccount.Number = result.Number;
                    newJournalEntryItem.AccountingAccount = result;
                    newJournalEntryItem.AccountingNature = result.AccountingNature;

                    newJournalEntryItem.ExchangeRate = journalEntryItem.ExchangeRate;
                    newJournalEntryItem.ExchangeRate.SellAmount = journalEntryItem.ExchangeRate.SellAmount;

                    newJournalEntryItem.Amount = new AmountDTO();
                    newJournalEntryItem.Amount.Currency = journalEntryItem.Amount.Currency;
                    newJournalEntryItem.LocalAmount = new AmountDTO();
                    newJournalEntryItem.Amount.Value = Math.Abs(journalEntryItem.Amount.Value);
                    newJournalEntryItem.LocalAmount.Value = Math.Abs(journalEntryItem.LocalAmount.Value);

                    newJournalEntryItem.Analysis = new List<AnalysisDTO>();
                    newJournalEntryItem.ReconciliationMovementType = journalEntryItem.ReconciliationMovementType;
                    newJournalEntryItem.CostCenters = new List<CostCenterDTO>();
                    newJournalEntryItem.Currency = journalEntryItem.Currency;
                    newJournalEntryItem.Description = journalEntryItem.Description;
                    newJournalEntryItem.EntryType = new EntryTypeDTO();
                    newJournalEntryItem.Id = 0;
                    newJournalEntryItem.Individual = journalEntryItem.Individual;
                    newJournalEntryItem.PostDated = new List<PostDatedDTO>();
                    newJournalEntryItem.Receipt = journalEntryItem.Receipt;
                    newJournalEntryItem.SourceCode = journalEntryItem.SourceCode;
                    newJournalEntryItems.Add(newJournalEntryItem);
                }

                return newJournalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// AssembleAccountingJournalEntryItems
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <returns></returns>
        private List<JournalEntryItemDTO> AssembleAccountingJournalEntryItems(JournalEntryItemDTO journalEntryItem, int accountingAccountId)
        {
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();

            try
            {
                AccountingAccountBusiness accountingAccountBusiness = new AccountingAccountBusiness();
                var result = accountingAccountBusiness.GetAccountingAccountByAccountingAccountId(accountingAccountId);

                if (result != null && result.Number != "")
                {
                    //Detalle
                    JournalEntryItemDTO newJournalEntryItem = new JournalEntryItemDTO();
                    newJournalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    newJournalEntryItem.AccountingAccount.Number = result.Number;
                    newJournalEntryItem.AccountingAccount = result.ToDTO();
                    newJournalEntryItem.AccountingNature = Convert.ToInt32(result.AccountingNature);

                    newJournalEntryItem.ExchangeRate = journalEntryItem.ExchangeRate;
                    newJournalEntryItem.ExchangeRate.SellAmount = journalEntryItem.ExchangeRate.SellAmount;

                    newJournalEntryItem.Amount = new AmountDTO();
                    newJournalEntryItem.Amount.Currency = journalEntryItem.Amount.Currency;
                    newJournalEntryItem.LocalAmount = new AmountDTO();
                    newJournalEntryItem.Amount.Value = Math.Abs(journalEntryItem.Amount.Value);
                    newJournalEntryItem.LocalAmount.Value = Math.Abs(journalEntryItem.LocalAmount.Value);

                    newJournalEntryItem.Analysis = new List<AnalysisDTO>();
                    newJournalEntryItem.ReconciliationMovementType = journalEntryItem.ReconciliationMovementType;
                    newJournalEntryItem.CostCenters = new List<CostCenterDTO>();
                    newJournalEntryItem.Currency = journalEntryItem.Currency;
                    newJournalEntryItem.Description = journalEntryItem.Description;
                    newJournalEntryItem.EntryType = new EntryTypeDTO();
                    newJournalEntryItem.Id = 0;
                    newJournalEntryItem.Individual = journalEntryItem.Individual;
                    newJournalEntryItem.PostDated = new List<PostDatedDTO>();
                    newJournalEntryItem.Receipt = journalEntryItem.Receipt;
                    newJournalEntryItem.SourceCode = journalEntryItem.SourceCode;
                    newJournalEntryItems.Add(newJournalEntryItem);
                }

                return newJournalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SetRevertionJournalEntryItems
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>List<JournalEntryItemDTO></returns>
        private List<JournalEntryItemDTO> SetRevertionJournalEntryItems(int journalEntryId)
        {
            List<JournalEntryItemDTO> revertionJournalEntryItems = new List<JournalEntryItemDTO>();

            try
            {
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                ObjectCriteriaBuilder journalEntryFilter = new ObjectCriteriaBuilder();
                journalEntryFilter.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.JournalEntryId, journalEntryId);
                BusinessCollection journalEntryItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), journalEntryFilter.GetPredicate()));

                if (journalEntryItems.Count > 0)
                {
                    foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in journalEntryItems.OfType<GENERALLEDGEREN.JournalEntryItem>())
                    {
                        JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                        journalEntryItem.Id = journalEntryItemEntity.JournalEntryItemId;
                        journalEntryItem = DTOAssembler.ToDTO(journalEntryItemDAO.GetJournalEntryItem(ModelDTOAssembler.ToModel(journalEntryItem)));
                        journalEntryItem.Id = 0; //se reinicia el identificador.
                        journalEntryItem.AccountingNature = Convert.ToInt32(journalEntryItemEntity.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? Convert.ToInt32(AccountingNatures.Debit) : Convert.ToInt32(AccountingNatures.Credit);
                        journalEntryItem.CostCenters = new List<CostCenterDTO>();
                        journalEntryItem.Analysis = new List<AnalysisDTO>();
                        journalEntryItem.PostDated = new List<PostDatedDTO>();

                        //se obtiene los centros de costos.
                        journalEntryItem.CostCenters = SetRevertionJournalEntryItemCostCenters(journalEntryItemEntity.JournalEntryItemId, true);

                        //se obtienen los análisis
                        journalEntryItem.Analysis = SetRevertionJournalEntryItemAnalyses(journalEntryItemEntity.JournalEntryItemId, true);

                        //se obtienen postfechados
                        journalEntryItem.PostDated = SetRevertionJournalEntryItemPostDated(journalEntryItemEntity.JournalEntryItemId, true);

                        revertionJournalEntryItems.Add(journalEntryItem);
                    }
                }
            }
            catch (BusinessException)
            {
                revertionJournalEntryItems = new List<JournalEntryItemDTO>();
            }

            return revertionJournalEntryItems;
        }

        /// <summary>
        /// SetRevertionJournalEntryItemCostCenters
        /// </summary>
        /// <param name="journalEntryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<CostCenterDTO></returns>
        private List<CostCenterDTO> SetRevertionJournalEntryItemCostCenters(int journalEntryItemId, bool isJournalEntry)
        {
            List<CostCenterDTO> costCenters = GetCostCentersByEntryId(journalEntryItemId, isJournalEntry);

            if (costCenters.Count > 0)
            {
                foreach (CostCenterDTO costCenter in costCenters)
                {
                    costCenter.CostCenterId = 0;
                }
            }

            return costCenters;
        }

        /// <summary>
        /// SetRevertionJournalEntryItemAnalyses
        /// </summary>
        /// <param name="journalEntryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<AnalysisDTO></returns>
        private List<AnalysisDTO> SetRevertionJournalEntryItemAnalyses(int journalEntryItemId, bool isJournalEntry)
        {
            List<AnalysisDTO> analyses = GetAnalysesByEntryId(journalEntryItemId, isJournalEntry);

            if (analyses.Count > 0)
            {
                foreach (AnalysisDTO analysis in analyses)
                {
                    analysis.AnalysisId = 0;
                }
            }

            return analyses;
        }

        /// <summary>
        /// SetRevertionJournalEntryItemPostDated
        /// </summary>
        /// <param name="journalEntryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<GeneralLedgerModels.PostDated></returns>
        private List<PostDatedDTO> SetRevertionJournalEntryItemPostDated(int journalEntryItemId, bool isJournalEntry)
        {
            List<PostDatedDTO> postDateds = GetPostdatedByEntryId(journalEntryItemId, isJournalEntry);

            if (postDateds.Count > 0)
            {
                foreach (PostDatedDTO postDated in postDateds)
                {
                    postDated.PostDatedId = 0;
                }
            }

            return postDateds;
        }

        #endregion JournalEntryItems

        #region TempEntry

        /// <summary>
        /// GenerateTempLedgerEntry
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <param name="userId"></param>
        /// <returns>LedgerEntry</returns>
        private LedgerEntryDTO GenerateTempLedgerEntry(BusinessCollection businessCollection, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();

            try
            {
                if (businessCollection.Count > 0)
                {
                    //Cabecera
                    ledgerEntry.AccountingCompany = new AccountingCompanyDTO()
                    {
                        AccountingCompanyId = Convert.ToInt32(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().AccountingCompanyCode)
                    };
                    ledgerEntry.AccountingDate = Convert.ToDateTime(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().Date);
                    ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                    {
                        AccountingMovementTypeId = Convert.ToInt32(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().AccountingMovementTypeId)
                    };
                    ledgerEntry.Branch = new BranchDTO() { Id = Convert.ToInt32(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().BranchCode) };
                    ledgerEntry.Description = businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().Description;
                    ledgerEntry.EntryDestination = new EntryDestinationDTO()
                    {
                        DestinationId = Convert.ToInt32(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().EntryDestinationId)
                    };
                    ledgerEntry.EntryNumber = 0;
                    ledgerEntry.Id = 0;
                    ledgerEntry.ModuleDateId = Convert.ToInt32(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().AccountingModuleId);
                    ledgerEntry.RegisterDate = DateTime.Now;
                    ledgerEntry.SalePoint = new SalePointDTO() { Id = Convert.ToInt32(businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>().First().SalePointCode) };// No existe este dato en ingreso de caja
                    ledgerEntry.Status = 1; //Activo
                    ledgerEntry.UserId = userId;
                    ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                    foreach (GENERALLEDGEREN.TempEntryGeneration tempEntryGeneration in businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>())
                    {

                        //Detalle                       
                        LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                        ledgerEntryItem.AccountingAccount = new AccountingAccountDTO()
                        {
                            AccountingAccountId = Convert.ToInt32(tempEntryGeneration.AccountingAccountId)
                        };
                        ledgerEntryItem.AccountingNature = Convert.ToInt32(tempEntryGeneration.AccountingNature);
                        ledgerEntryItem.Amount = new AmountDTO();
                        ledgerEntryItem.Amount.Value = Convert.ToDecimal(tempEntryGeneration.Amount);
                        ledgerEntryItem.Amount.Currency = new CurrencyDTO();
                        ledgerEntryItem.Amount.Currency.Id = Convert.ToInt32(tempEntryGeneration.CurrencyCode);
                        ledgerEntryItem.ExchangeRate = new ExchangeRateDTO();
                        ledgerEntryItem.ExchangeRate.SellAmount = Convert.ToDecimal(tempEntryGeneration.ExchangeRate);
                        ledgerEntryItem.LocalAmount = new AmountDTO();
                        ledgerEntryItem.LocalAmount.Value = Convert.ToDecimal(tempEntryGeneration.LocalAmount);
                        ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                        ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Id = Convert.ToInt32(tempEntryGeneration.BankReconciliationId)
                        };
                        ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                        ledgerEntryItem.Currency = new CurrencyDTO() { Id = Convert.ToInt32(tempEntryGeneration.CurrencyCode) };
                        ledgerEntryItem.Description = tempEntryGeneration.Description;
                        ledgerEntryItem.EntryType = new EntryTypeDTO();
                        ledgerEntryItem.Id = 0;
                        ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = Convert.ToInt32(tempEntryGeneration.IndividualId) };
                        ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                        ledgerEntryItem.Receipt = new ReceiptDTO()
                        {
                            Date = Convert.ToDateTime(tempEntryGeneration.ReceiptDate) == Convert.ToDateTime("01/01/0001 0:00:00") ? null :
                                                                                         tempEntryGeneration.ReceiptDate,
                            Number = Convert.ToInt32(tempEntryGeneration.ReceiptNumber)
                        };

                        ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                    }
                }
            }
            catch (BusinessException)
            {
                ledgerEntry = new LedgerEntryDTO();
            }

            return ledgerEntry;
        }

        /// <summary>
        /// GenerateTempJournalEntry
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="transactionNumber"></param>
        /// <returns>JournalEntry</returns>
        private JournalEntryDTO GenerateTempJournalEntry(BusinessCollection businessCollection, string description, int userId, int technicalTransaction)
        {
            JournalEntryDTO journalEntry = new JournalEntryDTO();

            try
            {
                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.TempEntryGeneration tempEntryGeneration in businessCollection.OfType<GENERALLEDGEREN.TempEntryGeneration>())
                    {
                        //Cabecera
                        journalEntry.AccountingCompany = new AccountingCompanyDTO()
                        {
                            AccountingCompanyId = Convert.ToInt32(tempEntryGeneration.AccountingCompanyCode)
                        };
                        journalEntry.AccountingDate = Convert.ToDateTime(tempEntryGeneration.Date);
                        journalEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                        {
                            AccountingMovementTypeId = Convert.ToInt32(tempEntryGeneration.AccountingMovementTypeId)
                        };
                        journalEntry.Branch = new BranchDTO() { Id = Convert.ToInt32(tempEntryGeneration.BranchCode) };
                        journalEntry.Description = description;
                        journalEntry.EntryNumber = technicalTransaction;
                        journalEntry.Id = 0;

                        //Detalle
                        List<JournalEntryItemDTO> journalEntryItems = new List<JournalEntryItemDTO>();
                        JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                        journalEntryItem.AccountingAccount = new AccountingAccountDTO()
                        {
                            AccountingAccountId = Convert.ToInt32(tempEntryGeneration.AccountingAccountId)
                        };
                        journalEntryItem.AccountingNature = Convert.ToInt32(tempEntryGeneration.AccountingNature);
                        journalEntryItem.Amount = new AmountDTO();
                        journalEntryItem.Amount.Value = Convert.ToDecimal(tempEntryGeneration.Amount);
                        journalEntryItem.Amount.Currency = new CurrencyDTO();
                        journalEntryItem.Amount.Currency.Id = Convert.ToInt32(tempEntryGeneration.CurrencyCode);
                        journalEntryItem.ExchangeRate = new ExchangeRateDTO();
                        journalEntryItem.ExchangeRate.SellAmount = Convert.ToDecimal(tempEntryGeneration.ExchangeRate);
                        journalEntryItem.LocalAmount = new AmountDTO();
                        journalEntryItem.LocalAmount.Value = Convert.ToDecimal(tempEntryGeneration.LocalAmount);
                        journalEntryItem.Analysis = new List<AnalysisDTO>();
                        journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Id = Convert.ToInt32(tempEntryGeneration.BankReconciliationId)
                        };
                        journalEntryItem.CostCenters = new List<CostCenterDTO>();
                        journalEntryItem.Currency = new CurrencyDTO() { Id = Convert.ToInt32(tempEntryGeneration.CurrencyCode) };
                        journalEntryItem.Description = description;
                        journalEntryItem.EntryType = new EntryTypeDTO();
                        journalEntryItem.Id = 0;
                        journalEntryItem.Individual = new IndividualDTO() { IndividualId = Convert.ToInt32(tempEntryGeneration.IndividualId) };
                        journalEntryItem.PostDated = new List<PostDatedDTO>();
                        journalEntryItem.Receipt = new ReceiptDTO()
                        {
                            Date = Convert.ToDateTime(tempEntryGeneration.ReceiptDate) == Convert.ToDateTime("01/01/0001 0:00:00") ? null :
                                                                                         tempEntryGeneration.ReceiptDate,
                            Number = Convert.ToInt32(tempEntryGeneration.ReceiptNumber)
                        };
                        journalEntryItems.Add(journalEntryItem);

                        journalEntry.JournalEntryItems = journalEntryItems;
                        journalEntry.ModuleDateId = Convert.ToInt32(tempEntryGeneration.AccountingModuleId);
                        journalEntry.RegisterDate = DateTime.Now;
                        journalEntry.SalePoint = new SalePointDTO() { Id = Convert.ToInt32(tempEntryGeneration.SalePointCode) };// No existe este dato en ingreso de caja
                        journalEntry.Status = 1; //Activo
                        journalEntry.UserId = userId;
                    }
                }
            }
            catch (BusinessException)
            {
                journalEntry = new JournalEntryDTO();
            }

            return journalEntry;
        }

        #endregion TempEntry

        #region EntryMassiveLoad

        /// <summary>
        /// GenerateLedgerEntryFromMassiveEntry
        /// </summary>
        /// <param name="massiveEntries"></param>
        /// <returns>LedgerEntry</returns>
        private LedgerEntryDTO GenerateLedgerEntryFromMassiveEntryList(List<MassiveEntryDTO> massiveEntries, int userId)
        {
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();

            try
            {
                //Se obtienen los datos de la cabecera.
                List<MassiveEntryDTO> headerLedgerEntryGroup = massiveEntries.GroupBy(p => new { p.BranchId, p.SalePointId, p.AccoutingCompanyId, p.EntryDestinationId, p.AccountingMovementTypeId, p.OperationDate }).Select(g => g.First()).ToList();

                if (headerLedgerEntryGroup.Count > 0)
                {
                    ledgerEntry.AccountingCompany = new AccountingCompanyDTO();
                    ledgerEntry.AccountingCompany.AccountingCompanyId = headerLedgerEntryGroup[0].AccoutingCompanyId;
                    ledgerEntry.AccountingDate = headerLedgerEntryGroup[0].OperationDate;
                    ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO();
                    ledgerEntry.AccountingMovementType.AccountingMovementTypeId = headerLedgerEntryGroup[0].AccountingMovementTypeId;
                    ledgerEntry.Branch = new BranchDTO();
                    ledgerEntry.Branch.Id = Convert.ToInt32(headerLedgerEntryGroup[0].BranchId);
                    ledgerEntry.Description = "CARGA MASIVA SUCURSAL " + Convert.ToString(headerLedgerEntryGroup[0].BranchId) + ", FECHA " + Convert.ToString(headerLedgerEntryGroup[0].OperationDate);
                    ledgerEntry.EntryDestination = new EntryDestinationDTO();
                    ledgerEntry.EntryDestination.DestinationId = headerLedgerEntryGroup[0].EntryDestinationId;
                    ledgerEntry.EntryNumber = 0;
                    ledgerEntry.Id = 0;
                    ledgerEntry.ModuleDateId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_LEDGER_ENTRY_MODULE));
                    ledgerEntry.RegisterDate = DateTime.Now;
                    ledgerEntry.SalePoint = new SalePointDTO();
                    ledgerEntry.SalePoint.Id = headerLedgerEntryGroup[0].SalePointId;
                    ledgerEntry.UserId = userId;
                }

                //se generan los movimientos
                ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                foreach (MassiveEntryDTO massiveEntryDTO in massiveEntries)
                {
                    LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                    ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                    ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                    ledgerEntryItem.PostDated = new List<PostDatedDTO>();

                    ledgerEntryItem.AccountingAccount = new AccountingAccountDTO();
                    ledgerEntryItem.AccountingAccount.AccountingAccountId = Convert.ToInt32(massiveEntryDTO.AccountingAccountId);
                    ledgerEntryItem.AccountingNature = Convert.ToInt32(massiveEntryDTO.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? Convert.ToInt32(AccountingNatures.Credit) : Convert.ToInt32(AccountingNatures.Debit);

                    ledgerEntryItem.Amount = new AmountDTO();
                    ledgerEntryItem.Amount.Value = Convert.ToDecimal(massiveEntryDTO.Amount);
                    ledgerEntryItem.Amount.Currency = new CurrencyDTO();
                    ledgerEntryItem.Amount.Currency.Id = Convert.ToInt32(massiveEntryDTO.CurrencyId);
                    ledgerEntryItem.ExchangeRate = new ExchangeRateDTO();
                    ledgerEntryItem.ExchangeRate.SellAmount = Convert.ToDecimal(massiveEntryDTO.ExchangeRate);
                    ledgerEntryItem.LocalAmount = new AmountDTO();
                    ledgerEntryItem.LocalAmount.Value = Convert.ToDecimal(massiveEntryDTO.Amount * massiveEntryDTO.ExchangeRate);

                    ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                    {
                        Id = Convert.ToInt32(massiveEntryDTO.BankReconciliationId)
                    };

                    ledgerEntryItem.Currency = new CurrencyDTO() { Id = Convert.ToInt32(massiveEntryDTO.CurrencyId) };
                    ledgerEntryItem.Description = massiveEntryDTO.Description;
                    ledgerEntryItem.Id = 0;
                    ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = massiveEntryDTO.IndividualId };

                    ledgerEntryItem.Receipt = new ReceiptDTO()
                    {
                        Date = Convert.ToDateTime(massiveEntryDTO.ReceiptDate),
                        Number = massiveEntryDTO.ReceiptNumber
                    };

                    //centros de costos
                    if (massiveEntryDTO.CostCenterId > 0)
                    {
                        CostCenterDTO costCenter = new CostCenterDTO()
                        {
                            CostCenterId = Convert.ToInt32(massiveEntryDTO.CostCenterId),
                            PercentageAmount = Convert.ToDecimal(massiveEntryDTO.Percentage)
                        };

                        ledgerEntryItem.CostCenters.Add(costCenter);
                    }

                    //analisis
                    if (massiveEntryDTO.AnalysisId > 0)
                    {
                        AnalysisConceptDTO analysisConcept = new AnalysisConceptDTO();
                        analysisConcept.AnalysisConceptId = Convert.ToInt32(massiveEntryDTO.ConceptId);
                        analysisConcept.AnalysisCode = new AnalysisCodeDTO();
                        analysisConcept.AnalysisCode.AnalysisCodeId = Convert.ToInt32(massiveEntryDTO.AnalysisId);

                        AnalysisDTO analysis = new AnalysisDTO();
                        analysis.AnalysisId = Convert.ToInt32(massiveEntryDTO.AnalysisId);
                        analysis.AnalysisConcept = analysisConcept;
                        analysis.ConceptKey = massiveEntryDTO.ConceptKey;
                        analysis.Description = massiveEntryDTO.AnalysisDescription;

                        ledgerEntryItem.Analysis.Add(analysis);
                    }

                    //postfechados
                    if (massiveEntryDTO.PostdatedId > 0)
                    {
                        AmountDTO postDatedAmount = new AmountDTO();
                        postDatedAmount.Value = Convert.ToDecimal(massiveEntryDTO.PostdatedAmount);
                        postDatedAmount.Currency = new CurrencyDTO() { Id = Convert.ToInt32(massiveEntryDTO.PostdatedCurrencyId) };
                        decimal postDatedExchangeRate = Convert.ToDecimal(massiveEntryDTO.PostdatedExchangeRate);
                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { SellAmount = postDatedExchangeRate };
                        AmountDTO localAmount = new AmountDTO() { Value = Convert.ToDecimal(massiveEntryDTO.PostdatedAmount) * postDatedExchangeRate };

                        PostDatedDTO postDated = new PostDatedDTO();
                        postDated.PostDatedId = 0; //autonumérico
                        postDated.PostDateType = Convert.ToInt32(massiveEntryDTO.PostdatedId);
                        postDated.Amount = postDatedAmount;
                        postDated.DocumentNumber = Convert.ToInt32(massiveEntryDTO.PosdatedDocumentNumber);
                        postDated.ExchangeRate = exchangeRate;
                        postDated.LocalAmount = localAmount;

                        ledgerEntryItem.PostDated.Add(postDated);
                    }

                    ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return ledgerEntry;
        }

        #endregion EntryMassiveLoad

        #region Analysis

        /// <summary>
        /// GetAnalysesByEntryId
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<AnalysisDTO></returns>
        private List<AnalysisDTO> GetAnalysesByEntryId(int entryItemId, bool isJournalEntry)
        {
            AnalysisDAO analysisDAO = new AnalysisDAO();
            List<AnalysisDTO> analyses = new List<AnalysisDTO>();

            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.EntryItemId, entryItemId);
                filter.And();
                filter.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.IsJournalEntry, isJournalEntry);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AnalysisEntryItem analysisEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisEntryItem>())
                    {
                        AnalysisDTO analysis = DTOAssembler.ToDTO(analysisDAO.GetAnalysis(Convert.ToInt32(analysisEntryItemEntity.AnalysisEntryItemId)));
                        analyses.Add(analysis);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return analyses;
        }

        #endregion Analysis

        #region AccountReclassification

        /// <summary>
        /// SaveAccountingReclassification
        /// </summary>
        /// <param name="accountReclassificationResult"></param>
        private void SaveAccountingReclassification(AccountReclassificationResultDTO accountReclassificationResult)
        {
            // Se inserta la entidad
            GENERALLEDGEREN.AccountingReclassification accountingReclassificationEntity = EntityAssembler.CreateAccountingReclassification(ModelDTOAssembler.ToModel(accountReclassificationResult));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(accountingReclassificationEntity);
        }

        /// <summary>
        /// ProcessReclassificationCostCenters
        /// </summary>
        /// <param name="costCenterEntries"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reclassificationEntity"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        private void ProcessReclassificationCostCenters(UIView costCenterEntries, DateTime dateFrom, DateTime dateTo, GENERALLEDGEREN.Reclassification reclassificationEntity, int month, int year)
        {
            if (costCenterEntries.Count > 0)
            {
                var costCenters = costCenterEntries.AsEnumerable()
                                    .GroupBy(r => new { Col1 = r["CurrencyCode"], Col2 = r["CostCenterId"], Col3 = r["BranchCode"] })
                                    .Select(g => g.OrderBy(r => r["AccountingAccountId"]).First())
                                    .CopyToDataTable();

                foreach (DataRow item in costCenters.Rows)
                {
                    /////////////////////////////////////
                    //// Sin analíticos                //
                    /////////////////////////////////////
                    GenerateEntriesWithoutAnalytical(item, dateFrom, dateTo, reclassificationEntity, month, year);

                    ///////////////////////////////////
                    // Con analíticos                //
                    ///////////////////////////////////
                    GenerateEntriesWithAnalytical(item, dateFrom, dateTo, reclassificationEntity, month, year);
                }
            }
        }

        /// <summary>
        /// GenerateEntriesWithoutAnalytical
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reclassificationEntity"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        private void GenerateEntriesWithoutAnalytical(DataRow item, DateTime dateFrom, DateTime dateTo, GENERALLEDGEREN.Reclassification reclassificationEntity, int month, int year)
        {
            int rows;

            int branchId = Convert.ToInt32(item["BranchCode"]);
            int costCenterId = item["CostCenterId"] == DBNull.Value ? -1 : Convert.ToInt32(item["CostCenterId"]);
            int currencyId = Convert.ToInt32(item["CurrencyCode"]);

            #region filter

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(dateFrom);
            criteriaBuilder.And();
            criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
            criteriaBuilder.LessEqual();
            criteriaBuilder.Constant(dateTo);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.AccountingAccountId, reclassificationEntity.SourceAccountingAccountId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.BranchCode, branchId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.CurrencyCode, currencyId);
            if (costCenterId > 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.CostCenterId, costCenterId);
            }

            #endregion filter

            UIView analyticalEntries = _dataFacadeManager.GetDataFacade().GetView("EntryWithoutAnalytical", criteriaBuilder.GetPredicate(), null, 0, 2147483647, null, true, out rows);

            GenerateReclassificationEntries(analyticalEntries, currencyId, branchId, costCenterId, reclassificationEntity, month, year);
        }

        /// <summary>
        /// GenerateEntriesWithAnalytical
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reclassificationEntity"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        private void GenerateEntriesWithAnalytical(DataRow item, DateTime dateFrom, DateTime dateTo, GENERALLEDGEREN.Reclassification reclassificationEntity, int month, int year)
        {
            int rows;

            int branchId = Convert.ToInt32(item["BranchCode"]);
            int costCenterId = item["CostCenterId"] == DBNull.Value ? -1 : Convert.ToInt32(item["CostCenterId"]);
            int currencyId = Convert.ToInt32(item["CurrencyCode"]);

            #region filter

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(dateFrom);
            criteriaBuilder.And();
            criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
            criteriaBuilder.LessEqual();
            criteriaBuilder.Constant(dateTo);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.AccountingAccountId, reclassificationEntity.SourceAccountingAccountId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.BranchCode, branchId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.CurrencyCode, currencyId);
            if (costCenterId > 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.CostCenterId, costCenterId);
            }
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId, 2);

            #endregion filter

            UIView analyticalEntries = _dataFacadeManager.GetDataFacade().GetView("EntryWithAnalytical", criteriaBuilder.GetPredicate(), null, 0, 2147483647, null, true, out rows);

            GenerateReclassificationEntries(analyticalEntries, currencyId, branchId, costCenterId, reclassificationEntity, month, year);
        }

        /// <summary>
        /// GenerateReclassificationEntries
        /// </summary>
        /// <param name="analyticalEntries"></param>
        /// <param name="currencyId"></param>
        /// <param name="branchId"></param>
        /// <param name="costCenterId"></param>
        /// <param name="reclassificationEntity"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        private void GenerateReclassificationEntries(UIView analyticalEntries, int currencyId, int branchId, int costCenterId, GENERALLEDGEREN.Reclassification reclassificationEntity, int month, int year)
        {
            decimal incomeAmount = 0;
            decimal amount = 0;
            decimal incomeAmountDebit = 0;
            decimal amountDebit = 0;
            decimal exchangeRate = 0;
            int accountingAccountId = 0;
            int accountingReclassificationId = 0;
            int analysisId = 0;
            int analysisConceptId = 0;
            int sourceAccountingAccountId = 0;
            string conceptKey = "";

            if (analyticalEntries.Count > 0)
            {
                incomeAmount = GetReclassificationEntriesIncomeAmount(analyticalEntries);
                amount = GetReclassificationEntriesAmount(analyticalEntries);

                exchangeRate = Convert.ToDecimal(analyticalEntries.Rows[0]["ExchangeRate"]);
                accountingAccountId = Convert.ToInt32(analyticalEntries.Rows[0]["AccountingAccountId"]);
                analysisId = analyticalEntries.Rows[0]["AnalysisId"] == DBNull.Value ? -1 : Convert.ToInt32(analyticalEntries.Rows[0]["AnalysisId"]);
                analysisConceptId = analyticalEntries.Rows[0]["AnalysisConceptId"] == DBNull.Value ? -1 : Convert.ToInt32(analyticalEntries.Rows[0]["AnalysisConceptId"]);
                sourceAccountingAccountId = Convert.ToInt32(analyticalEntries.Rows[0]["AccountingAccountId"]);
                conceptKey = analyticalEntries.Rows[0]["ConceptKey"] == DBNull.Value ? "" : Convert.ToString(analyticalEntries.Rows[0]["ConceptKey"]);

                incomeAmountDebit = incomeAmount * -1;
                amountDebit = amount * -1;

                ///////////////////////////////////////
                // Crédito                           //
                ///////////////////////////////////////
                if (incomeAmount > 0)
                {
                    AccountReclassificationResultDTO accountReclassificationResult = new AccountReclassificationResultDTO();

                    accountReclassificationResult.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                    accountReclassificationResult.Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO()
                        {
                            Id = currencyId
                        },
                        Value = incomeAmount
                    };
                    accountReclassificationResult.ExchangeRate = new ExchangeRateDTO()
                    {
                        SellAmount = exchangeRate
                    };
                    accountReclassificationResult.LocalAmount = new AmountDTO()
                    {
                        Value = amount
                    };
                    accountReclassificationResult.Analysis = new AnalysisDTO()
                    {
                        AnalysisConcept = new AnalysisConceptDTO()
                        {
                            AnalysisConceptId = analysisConceptId
                        },
                        AnalysisId = analysisId,
                        ConceptKey = conceptKey
                    };

                    accountReclassificationResult.Branch = new BranchDTO() { Id = branchId };
                    accountReclassificationResult.CostCenter = new CostCenterDTO()
                    {
                        CostCenterId = costCenterId
                    };
                    accountReclassificationResult.DestinationAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingAccountId
                    };
                    accountReclassificationResult.Id = accountingReclassificationId;
                    accountReclassificationResult.Month = month;
                    accountReclassificationResult.SourceAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = sourceAccountingAccountId
                    };
                    accountReclassificationResult.Year = year;

                    SaveAccountingReclassification(accountReclassificationResult);

                    // Se inserta débito asiento a la contrapartida
                    accountingAccountId = reclassificationEntity.DestinationAccountingAccountId;

                    accountReclassificationResult.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                    accountReclassificationResult.DestinationAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingAccountId
                    };

                    SaveAccountingReclassification(accountReclassificationResult);
                }

                ///////////////////////////////////////
                // Débito                            //
                ///////////////////////////////////////
                if (incomeAmount < 0)
                {
                    AccountReclassificationResultDTO accountReclassificationResult = new AccountReclassificationResultDTO();

                    accountReclassificationResult.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                    accountReclassificationResult.Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO()
                        {
                            Id = currencyId
                        },
                        Value = incomeAmountDebit
                    };
                    accountReclassificationResult.ExchangeRate = new ExchangeRateDTO()
                    {
                        SellAmount = exchangeRate,
                    };
                    accountReclassificationResult.LocalAmount = new AmountDTO()
                    {
                        Value = amountDebit,
                    };
                    accountReclassificationResult.Analysis = new AnalysisDTO()
                    {
                        AnalysisConcept = new AnalysisConceptDTO()
                        {
                            AnalysisConceptId = analysisConceptId
                        },
                        AnalysisId = analysisId,
                        ConceptKey = conceptKey
                    };

                    accountReclassificationResult.Branch = new BranchDTO() { Id = branchId };
                    accountReclassificationResult.CostCenter = new CostCenterDTO()
                    {
                        CostCenterId = costCenterId
                    };
                    accountReclassificationResult.DestinationAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingAccountId
                    };
                    accountReclassificationResult.Id = accountingReclassificationId;
                    accountReclassificationResult.Month = month;
                    accountReclassificationResult.SourceAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = sourceAccountingAccountId
                    };
                    accountReclassificationResult.Year = year;

                    SaveAccountingReclassification(accountReclassificationResult);

                    // Se inserta crédito asiento a la contrapartida
                    accountingAccountId = reclassificationEntity.DestinationAccountingAccountId;

                    accountReclassificationResult.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                    accountReclassificationResult.DestinationAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingAccountId
                    };

                    SaveAccountingReclassification(accountReclassificationResult);
                }
            }
        }

        /// <summary>
        /// GetReclassificationEntriesIncomeAmount
        /// </summary>
        /// <param name="analyticalEntries"></param>
        /// <returns>decimal</returns>
        private decimal GetReclassificationEntriesIncomeAmount(UIView analyticalEntries)
        {
            decimal incomeAmount = 0;

            foreach (DataRow entry in analyticalEntries)
            {
                if (Convert.ToInt32(entry["AccountingNature"]) == Convert.ToInt32(AccountingNatures.Debit))
                {
                    incomeAmount += Convert.ToDecimal(entry["AmountValue"]);
                }
                else
                {
                    incomeAmount += Convert.ToDecimal(entry["AmountValue"]) * -1;
                }
            }

            return incomeAmount;
        }

        /// <summary>
        /// GetReclassificationEntriesAmount
        /// </summary>
        /// <param name="analyticalEntries"></param>
        /// <returns>decimal</returns>
        private decimal GetReclassificationEntriesAmount(UIView analyticalEntries)
        {
            decimal amount = 0;

            foreach (DataRow entry in analyticalEntries)
            {
                if (Convert.ToInt32(entry["AccountingNature"]) == Convert.ToInt32(AccountingNatures.Debit))
                {
                    amount += Convert.ToDecimal(entry["AmountLocalValue"]);
                }
                else
                {
                    amount += Convert.ToDecimal(entry["AmountLocalValue"]) * -1;
                }
            }

            return amount;
        }

        #endregion AccountReclassification        

        #endregion Private Methods


        #region New Accounting Concepts
        /// <summary>
        /// Devuelve el número de cuenta a partir del código del concepto
        /// </summary>
        /// <param name="parameters">Parámetros necesarios para la consulta</param>
        /// <returns>Número de cuenta</returns>
        public AccountingAccountDTO GetAccountingNumberByAccountingConcept(AccountingParameterDTO parameters)
        {
            try
            {
                AccountingConceptBusiness accountingConceptBusiness = new AccountingConceptBusiness();
                AccountingAccount accountingAccount = accountingConceptBusiness.GetAccountingNumberByAccountingConcept(parameters.AccountingConceptId, parameters.BranchId, parameters.PrefixId);
                return accountingAccount.ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        #endregion
        
        /// <summary>
        /// Saves journal entry
        /// </summary>
        /// <param name="accountingJournalEntryParametersCollection">Parameters</param>
        /// <returns></returns>
        public int SaveGenericJournalEntry(string accountingJournalEntryParametersCollection)
        {
            int journalEntryId = 0;

            JournalParameterDTO journalEntryParametersDTO = new JournalParameterDTO();

            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalParameterDTO>(accountingJournalEntryParametersCollection);


            //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
            List<JournalEntryItemDTO> newJournalEntryItems = new List<JournalEntryItemDTO>();
            List<JournalEntryItemDTO> entryItems;
            try
            {
                //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                for (int i = 0; i < journalEntryParametersDTO.JournalEntry.JournalEntryItems.Count; i++)
                {
                    if (journalEntryParametersDTO.Parameters[i].Count > 0)
                    {
                        if (journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].AccountingConcept > 0)
                        {
                            //se realiza el cálculo de los movimientos.
                            entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i], journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].AccountingConcept);

                            if (entryItems.Count > 0)
                            {
                                foreach (var entryItem in entryItems)
                                {
                                    newJournalEntryItems.Add(entryItem);
                                }
                            }
                        }
                        else
                        {
                            //se realiza el cálculo de los movimientos.
                            entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i], journalEntryParametersDTO.JournalEntry.ModuleDateId, journalEntryParametersDTO.Parameters[i]);

                            if (entryItems.Count > 0)
                            {
                                foreach (var entryItem in entryItems)
                                {
                                    newJournalEntryItems.Add(entryItem);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].BridgeAccountId > 0)
                        {
                            List<ParameterDTO> itemsParameters = new List<ParameterDTO>();
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].BridgeAccountId) }); //tipo de pago crédito
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].Currency.Id, CultureInfo.InvariantCulture) }); //moneda
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i].Amount.Value, CultureInfo.InvariantCulture) }); //valor
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos

                            //se realiza el cálculo de los movimientos.
                            entryItems = AssembleAccountingJournalEntryItems(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i],
                                journalEntryParametersDTO.JournalEntry.ModuleDateId, itemsParameters,
                                Convert.ToString(journalEntryParametersDTO.CodeRulePackage));

                            if (entryItems.Count > 0)
                            {
                                foreach (var entryItem in entryItems)
                                {
                                    newJournalEntryItems.Add(entryItem);
                                }
                            }
                        }
                        else
                        {
                            newJournalEntryItems.Add(journalEntryParametersDTO.JournalEntry.JournalEntryItems[i]);
                        }
                    }
                }

                // Se asigna los nuevos detalles generados al asiento.
                journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;
                // Si los movimientos no están balanceados, genera un movimiento por cuenta puenta
                if (!ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    decimal creditLocalAmount = newJournalEntryItems.Where(x => x.AccountingNature == Convert.ToInt32(AccountingNatures.Credit)).Sum(x => x.LocalAmount.Value);
                    decimal debitLocalAmount = newJournalEntryItems.Where(x => x.AccountingNature == Convert.ToInt32(AccountingNatures.Debit)).Sum(x => x.LocalAmount.Value);

                    decimal total = debitLocalAmount - creditLocalAmount;

                    JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();
                    journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    journalEntryItem.Amount = new AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = 0 }
                    };
                    journalEntryItem.LocalAmount = new AmountDTO()
                    {
                        Value = total
                    };
                    journalEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = 1 };
                    journalEntryItem.Analysis = new List<AnalysisDTO>();
                    journalEntryItem.CostCenters = new List<CostCenterDTO>();
                    journalEntryItem.Currency = new CurrencyDTO() { Id = 0 };
                    journalEntryItem.Description = journalEntryParametersDTO.JournalEntry.Description;
                    journalEntryItem.EntryType = new EntryTypeDTO();
                    journalEntryItem.Id = 0;
                    journalEntryItem.Individual = new IndividualDTO() { IndividualId = journalEntryParametersDTO.JournalEntry.JournalEntryItems[0].Individual.IndividualId };
                    journalEntryItem.PostDated = new List<PostDatedDTO>();
                    if (journalEntryParametersDTO.JournalEntry.Receipt != null)
                    {
                        journalEntryItem.Receipt = new ReceiptDTO() { Number = journalEntryParametersDTO.JournalEntry.Receipt.Number, Date = journalEntryParametersDTO.JournalEntry.Receipt.Date };
                    }
                    else
                    {
                        journalEntryItem.Receipt = new ReceiptDTO() { Number = 0, Date = null };
                    }
                    if (journalEntryParametersDTO.JournalEntry.ReconciliationMovementType != null)
                    {
                        journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = journalEntryParametersDTO.JournalEntry.ReconciliationMovementType.Id };
                    }
                    else
                    {
                        journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = 0 };
                    }

                    journalEntryItem.SourceCode = 0;
                    journalEntryItem.Branch = new BranchDTO() { Id = journalEntryParametersDTO.JournalEntry.Branch.Id };
                    journalEntryItem.SalePoint = new SalePointDTO() { Id = journalEntryParametersDTO.JournalEntry.SalePoint.Id };

                    if (journalEntryParametersDTO.BridgeAccounting > 0)
                    {
                        // Movimientos con cuenta puente parametrizada
                        // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                        List<ParameterDTO> itemsParameters = new List<ParameterDTO>();
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(journalEntryParametersDTO.BridgeAccounting) }); //tipo de pago crédito
                                                                                                                                          // Currency Code: Local currency
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //moneda
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(total, CultureInfo.InvariantCulture) }); //valor
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                        itemsParameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                                                                                                                //Detalle con parámetros fijo
                                                                                                                                //se realiza el cálculo de los movimientos.
                        entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, journalEntryParametersDTO.JournalEntry.ModuleDateId, itemsParameters, journalEntryParametersDTO.CodeRulePackage);

                        if (entryItems.Count > 0)
                        {   
                            foreach (JournalEntryItemDTO entryItem in entryItems)
                            {
                                newJournalEntryItems.Add(entryItem);
                            }
                        }
                    }
                    else if (journalEntryParametersDTO.AccountingAccountId > 0)
                    {
                        // Boletas de depósito
                        decimal creditAmount = newJournalEntryItems.Where(x => x.AccountingNature == Convert.ToInt32(AccountingNatures.Credit)).Sum(x => x.Amount.Value);
                        decimal debitAmount = newJournalEntryItems.Where(x => x.AccountingNature == Convert.ToInt32(AccountingNatures.Debit)).Sum(x => x.Amount.Value);
                        if (newJournalEntryItems.FirstOrDefault() != null)
                        {
                            journalEntryItem.Amount.Currency.Id = newJournalEntryItems.FirstOrDefault().Currency.Id;
                            journalEntryItem.ExchangeRate.SellAmount = newJournalEntryItems.FirstOrDefault().ExchangeRate.SellAmount;
                            journalEntryItem.Currency = journalEntryItem.Amount.Currency;
                        }
                        journalEntryItem.Amount.Value = Math.Abs(debitAmount - creditAmount);

                        //se realiza el cálculo de los movimientos.
                        entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, journalEntryParametersDTO.AccountingAccountId);

                        if (entryItems.Count > 0)
                        {
                            foreach (JournalEntryItemDTO entryItem in entryItems)
                            {
                                // La naturaleza depende del asiento
                                if (creditAmount > debitAmount)
                                    entryItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                                else
                                    entryItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                                newJournalEntryItems.Add(entryItem);
                            }
                        }
                    }

                    //se asigna los nuevos detalles generados al asiento.
                    journalEntryParametersDTO.JournalEntry.JournalEntryItems = newJournalEntryItems;
                }


                //Valida débitos y créditos
                if (ValidateJournalEntryDebitsAndCredits(journalEntryParametersDTO.JournalEntry.JournalEntryItems))
                {
                    journalEntryId = SaveJournalEntryWithoutTransaction(journalEntryParametersDTO.JournalEntry);
                }
                else
                {
                    //asiento descuadrado
                    journalEntryId = 0;
                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores

                //error en grabación de asiento.
                journalEntryId = -2;
            }

            return journalEntryId;
        }

        public List<AccountingConceptDTO> GetAccountingConceptsByUserIdBranchIdIndividualId(int userId, int branchId, int individualId)
        {
            try
            {
                AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
                return accountingConceptDAO.GetAccountingConceptsByUserIdBranchIdIndividualId(userId, branchId, individualId).ToDTOs().ToList();
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAccountingConceptsByUserIdBranchIdIndividual);
            }
        }

        public List<AccountingConceptDTO> GetLiteAccountingConcepts()
        {
            try
            {
                AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
                return accountingConceptDAO.GetLiteAccountingConcepts().ToDTOs().ToList();
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetLiteAccountingConcepts);
            }
        }

        public int SaveBasicJournalEntry(string accountingJournalEntryParametersCollection)
        {
            try
            {
                GeneralLedgerBusiness generalLedgerBusiness = new GeneralLedgerBusiness();
                JournalParameterDTO journalEntryParameters = new JournalParameterDTO();

                journalEntryParameters = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalParameterDTO>(accountingJournalEntryParametersCollection);

                JournalParameter journalParameter = journalEntryParameters.ToModel();
                journalParameter.BridgeAccounts = new List<int>();
                if (journalParameter != null && journalParameter.JournalEntry != null && journalParameter.JournalEntry.JournalEntryItems.Any())
                {
                    journalParameter.BridgeAccounts = new List<int>();
                    journalEntryParameters.JournalEntry.JournalEntryItems.ForEach(item =>
                    {
                        journalParameter.BridgeAccounts.Add(item.BridgeAccountId);
                    });
                }

                return generalLedgerBusiness.SaveGenericJournalEntry(journalParameter);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveBasicJournalEntry);
            }
        }

        public int ReverseBasicJournalEntry(string reverseParameters)
        {
            try
            {
                GeneralLedgerBusiness generalLedgerBusiness = new GeneralLedgerBusiness();
                JournalEntryReversionParametersDTO parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalEntryReversionParametersDTO>(reverseParameters);
                return generalLedgerBusiness.ReverseJournalEntry(parameters.ToModel());
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorReverseBasicJournalEntry);
            }
        }
    }
}
