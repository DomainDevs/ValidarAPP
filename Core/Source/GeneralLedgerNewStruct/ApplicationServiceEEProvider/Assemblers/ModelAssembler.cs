using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.Rules;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using ReclassificationModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;

using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using ACMOD=Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;

using TAXESMOD = Sistran.Core.Application.TaxServices.Models;

using TAXEN = Sistran.Core.Application.Tax.Entities;
namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region GL

        #region AccountingAccount

        /// <summary>
        /// CreateAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns></returns>
        public static AccountingAccount CreateAccountingAccount(GENERALLEDGEREN.AccountingAccount accountingAccount)
        {
            Branch defaultBranch = new Branch();
            if (accountingAccount.DefaultBranchCode == null)
            {
                defaultBranch.Id = -1;
            }
            else
            {
                defaultBranch.Id = Convert.ToInt32(accountingAccount.DefaultBranchCode);
            }

            Currency currency = new Currency();
            if (accountingAccount.DefaultCurrencyCode == null)
            {
                currency.Id = -1;
            }
            else
            {
                currency.Id = Convert.ToInt32(accountingAccount.DefaultCurrencyCode);
            }

            Analysis analysis = new Analysis();
            analysis.AnalysisId = Convert.ToInt32(accountingAccount.AnalysisId);

            AccountingAccountType accountingAccountType = new AccountingAccountType() { Id = Convert.ToInt32(accountingAccount.AccountTypeId) };

            return new AccountingAccount
            {
                AccountingAccountId = accountingAccount.AccountingAccountId,
                AccountingAccountParentId = Convert.ToInt32(accountingAccount.AccountingAccountParentId),
                Number = accountingAccount.AccountNumber,
                Description = accountingAccount.AccountName,
                Branch = defaultBranch,
                AccountingNature = (AccountingNatures)accountingAccount.AccountingNature,
                Currency = currency,
                RequiresAnalysis = Convert.ToBoolean(accountingAccount.RequireAnalysis),
                Analysis = analysis,
                RequiresCostCenter = Convert.ToBoolean(accountingAccount.RequireCostCenter),
                Comments = accountingAccount.Comments != null ? accountingAccount.Comments : "",
                AccountingAccountApplication = accountingAccount.AccountApplication != null ? (AccountingAccountApplications)accountingAccount.AccountApplication : (AccountingAccountApplications)1,
                AccountingAccountType = accountingAccountType,
                IsReclassify = accountingAccount.IsReclassify,
                RecAccounting = accountingAccount.RecAccounting,
                IsRevalue = accountingAccount.IsRevalue,
                RevAcountingPos = accountingAccount.RevAcountingPos,
                RevAcountingNeg = accountingAccount.RevAcountingNeg
            };
        }

        internal static AccountingAccount CreateAccountingAccount(AccountingAccountDTO accountingAccount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CreateAccountingAccounts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingAccount> CreateAccountingAccounts(BusinessCollection businessCollection)
        {
            var accountingAccounts = new List<AccountingAccount>();
            foreach (GENERALLEDGEREN.AccountingAccount accountingAccountEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
            {
                accountingAccounts.Add(CreateAccountingAccount(accountingAccountEntity));
            }
            return accountingAccounts;
        }

       

        #endregion AccountingAccount

        #region AccountingAccountParent

        /// <summary>
        /// CreateAccountingAccountParent
        /// </summary>
        /// <param name="accountingAccountParent"></param>
        /// <returns></returns>
        public static AccountingAccount CreateAccountingAccountParent(GENERALLEDGEREN.AccountingAccountParent accountingAccountParent)
        {
            return new AccountingAccount
            {
                AccountingAccountId = accountingAccountParent.Id,
                Description = accountingAccountParent.Description,
                Number = Convert.ToString(accountingAccountParent.Id)
            };
        }

        internal static int CreateAccountingAccount(int accountingAccountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CreateAccountingAccountParents
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingAccount> CreateAccountingAccountParents(BusinessCollection businessCollection)
        {
            var accountingAccounts = new List<AccountingAccount>();
            foreach (GENERALLEDGEREN.AccountingAccountParent accountingAccountParentEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountParent>())
            {
                accountingAccounts.Add(CreateAccountingAccountParent(accountingAccountParentEntity));
            }
            return accountingAccounts;
        }

        #endregion AccountingAccount

        #region LedgerEntry

        /// <summary>
        /// CreateLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns>LedgerEntry</returns>
        public static LedgerEntry CreateLedgerEntry(GENERALLEDGEREN.LedgerEntry ledgerEntry)
        {
            AccountingCompany accountingCompany = new AccountingCompany();
            accountingCompany.AccountingCompanyId = ledgerEntry.AccountingCompanyId;

            AccountingMovementType accountingMovementType = new AccountingMovementType();
            accountingMovementType.AccountingMovementTypeId = ledgerEntry.AccountingMovementTypeId;

            int moduleDateId = ledgerEntry.AccountingModuleId;

            Branch branch = new Branch();
            branch.Id = ledgerEntry.BranchCode;

            SalePoint salePoint = new SalePoint();
            salePoint.Id = Convert.ToInt32(ledgerEntry.SalePointCode);

            EntryDestination entryDestination = new EntryDestination();
            entryDestination.DestinationId = ledgerEntry.EntryDestinationId;

            return new LedgerEntry()
            {
                Id = ledgerEntry.LedgerEntryId,
                AccountingCompany = accountingCompany,
                AccountingMovementType = accountingMovementType,
                ModuleDateId = moduleDateId,
                Branch = branch,
                SalePoint = salePoint,
                EntryDestination = entryDestination,
                Description = ledgerEntry.Description,
                EntryNumber = Convert.ToInt32(ledgerEntry.EntryNumber),
                AccountingDate = Convert.ToDateTime(ledgerEntry.AccountingDate),
                RegisterDate = Convert.ToDateTime(ledgerEntry.RegisterDate),
                UserId = ledgerEntry.UserCode
            };
        }

        #endregion LedgerEntry

        #region LedgerEntryItem

        /// <summary>
        /// CreateAccountingEntry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static LedgerEntryItem CreateLedgerEntryItem(GENERALLEDGEREN.LedgerEntryItem ledgerEntryItem)
        {
            AccountingAccount accountingAccount = new AccountingAccount();
            accountingAccount.AccountingAccountId = Convert.ToInt32(ledgerEntryItem.AccountingAccountId);

            Amount amount = new Amount() { Value = Convert.ToDecimal(ledgerEntryItem.AmountValue) };
            amount.Currency = new Currency() { Id = Convert.ToInt32(ledgerEntryItem.CurrencyCode) };
            Amount localAmount = new Amount() { Value = Convert.ToDecimal(ledgerEntryItem.AmountLocalValue) };
            ExchangeRate exchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(ledgerEntryItem.ExchangeRate) };


            Currency currency = new Currency();
            currency.Id = Convert.ToInt32(ledgerEntryItem.CurrencyCode);

            ReconciliationMovementType reconciliationMovementType = new ReconciliationMovementType();
            reconciliationMovementType.Id = Convert.ToInt32(ledgerEntryItem.BankReconciliationId);

            Person person = new Person();
            person.IndividualId = Convert.ToInt32(ledgerEntryItem.IndividualId);
            Individual individual = new Individual() { IndividualId = Convert.ToInt32(ledgerEntryItem.IndividualId) };

            Receipt receipt = new Receipt();
            receipt.Date = Convert.ToDateTime(ledgerEntryItem.ReceiptDate);
            receipt.Number = Convert.ToInt32(ledgerEntryItem.ReceiptNumber);

            List<Analysis> analysis = new List<Analysis>();
            List<CostCenter> costCenters = new List<CostCenter>();
            List<PostDated> postDateds = new List<PostDated>();

            EntryType entryType = new EntryType() { EntryTypeId = 0 };

            return new LedgerEntryItem()
            {
                AccountingAccount = accountingAccount,
                Amount = amount,
                AccountingNature = (AccountingNatures)ledgerEntryItem.AccountingNature,
                Analysis = analysis,
                ReconciliationMovementType = reconciliationMovementType,
                CostCenters = costCenters,
                Currency = currency,
                Description = ledgerEntryItem.Description,
                EntryType = entryType,
                Id = ledgerEntryItem.LedgerEntryItemId,
                Individual = individual,
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount,
                PostDated = postDateds,
                Receipt = receipt
            };
        }

        /// <summary>
        /// CreateAccountingEntries
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<LedgerEntryItem> CreateLedgerEntryItems(BusinessCollection businessCollection)
        {
            List<LedgerEntryItem> ledgerEntryItems = new List<LedgerEntryItem>();
            foreach (GENERALLEDGEREN.LedgerEntryItem ledgerEntryEntity in businessCollection.OfType<GENERALLEDGEREN.LedgerEntryItem>())
            {
                ledgerEntryItems.Add(CreateLedgerEntryItem(ledgerEntryEntity));
            }
            return ledgerEntryItems;
        }

        #endregion

        #region JournalEntry

        /// <summary>
        /// CreateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public static JournalEntry CreateJournalEntry(GENERALLEDGEREN.JournalEntry journalEntry)
        {
            AccountingCompany accountingCompany = new AccountingCompany();
            accountingCompany.AccountingCompanyId = Convert.ToInt32(journalEntry.AccountingCompanyId);

            int moduleDateId = Convert.ToInt32(journalEntry.AccountingModuleId);

            Branch branch = new Branch();
            branch.Id = Convert.ToInt32(journalEntry.BranchCode);

            SalePoint salePoint = new SalePoint();
            salePoint.Id = Convert.ToInt32(journalEntry.SalePointCode);

            return new JournalEntry()
            {
                Id = journalEntry.JournalEntryId,
                AccountingCompany = accountingCompany,
                ModuleDateId = moduleDateId,
                Branch = branch,
                SalePoint = salePoint,
                TechnicalTransaction = Convert.ToInt32(journalEntry.TechnicalTransaction),
                EntryNumber = Convert.ToInt32(journalEntry.EntryNumber),
                Description = journalEntry.Description,
                AccountingDate = Convert.ToDateTime(journalEntry.AccountingDate),
                RegisterDate = Convert.ToDateTime(journalEntry.RegisterDate),
                Status = Convert.ToInt32(journalEntry.Status),
                UserId = Convert.ToInt32(journalEntry.UserCode)                
            };
        }

        /// <summary>
        /// CreateJournalEntries
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<JournalEntry> CreateJournalEntries(BusinessCollection businessCollection)
        {
            try
            {
                List<JournalEntry> journalEntries = new List<JournalEntry>();
                foreach (GENERALLEDGEREN.JournalEntry journalEntryEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntry>())
                {
                    journalEntries.Add(CreateJournalEntry(journalEntryEntity));
                }
                return journalEntries;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion JournalEntry

        #region JournalEntryItem

        /// <summary>
        /// CreateJournalEntryItem
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <returns></returns>
        public static JournalEntryItem CreateJournalEntryItem(GENERALLEDGEREN.JournalEntryItem journalEntryItem)
        {
            try
            {
                Currency currency = new Currency();
                currency.Id = Convert.ToInt32(journalEntryItem.CurrencyCode);

                AccountingAccount accountingAccount = new AccountingAccount();
                accountingAccount.AccountingAccountId = Convert.ToInt32(journalEntryItem.AccountingAccountId);

                ReconciliationMovementType reconciliationMovementType = new ReconciliationMovementType();
                reconciliationMovementType.Id = Convert.ToInt32(journalEntryItem.BankReconciliationId);

                Receipt receipt = new Receipt();
                receipt.Date = Convert.ToDateTime(journalEntryItem.ReceiptDate);
                receipt.Number = Convert.ToInt32(journalEntryItem.ReceiptNumber);

                Amount amount = new Amount();
                amount.Currency = new Currency() { Id = Convert.ToInt32(journalEntryItem.CurrencyCode) };
                amount.Value = Convert.ToDecimal(journalEntryItem.Amount);
                Amount localAmount = new Amount() { Value = Convert.ToDecimal(journalEntryItem.LocalAmount) };
                ExchangeRate exchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(journalEntryItem.ExchangeRate) };

                Branch branch = new Branch() { Id = Convert.ToInt32(journalEntryItem.BranchCode) };
                SalePoint sailPoint = new SalePoint() { Id = Convert.ToInt32(journalEntryItem.SalePointCode) };

                Individual individual = new Individual();
                individual.IndividualId = Convert.ToInt32(journalEntryItem.IndividualId);

                return new JournalEntryItem()
                {
                    Id = journalEntryItem.JournalEntryItemId,
                    Currency = currency,
                    AccountingAccount = accountingAccount,
                    ReconciliationMovementType = reconciliationMovementType,
                    Receipt = receipt,
                    AccountingNature = (AccountingNatures)journalEntryItem.AccountingNature,
                    Description = journalEntryItem.Description,
                    Amount = amount,
                    ExchangeRate = exchangeRate,
                    LocalAmount = localAmount,
                    //EntryNumber = Convert.ToInt32(journalEntryItem.EntryNumber),
                    Individual = individual,
                    AccountingConcept = journalEntryItem.AccountingConceptCode.HasValue ? (int)journalEntryItem.AccountingConceptCode : 0,
                    Branch = branch,
                    SalePoint = sailPoint,
                    SourceCode = journalEntryItem.SourceCode
                };
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// CreateJournalEntryItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<JournalEntryItem> CreateJournalEntryItems(BusinessCollection businessCollection)
        {
            try
            {
                List<JournalEntryItem> journalEntryItems = new List<JournalEntryItem>();
                foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntryItem>())
                {
                    journalEntryItems.Add(CreateJournalEntryItem(journalEntryItemEntity));
                }
                return journalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion JournalEntryItem

        #region EntryNumber

        /// <summary>
        /// CreateEntryNumber
        /// </summary>
        /// <param name="entryNumberEntity"></param>
        /// <returns></returns>
        public static EntryNumber CreateEntryNumber(GENERALLEDGEREN.EntryNumber entryNumberEntity)
        {
            AccountingMovementType accountingMovementType = new AccountingMovementType();
            accountingMovementType.AccountingMovementTypeId = Convert.ToInt32(entryNumberEntity.AccountingMovementTypeId);

            EntryDestination entryDestination = new EntryDestination();
            entryDestination.DestinationId = Convert.ToInt32(entryNumberEntity.EntryDestinationId);

            return new EntryNumber()
            {
                AccountingMovementType = accountingMovementType,
                Date = new DateTime(Convert.ToInt32(entryNumberEntity.GenerationYear), Convert.ToInt32(entryNumberEntity.GenerationMonth), 1),
                EntryDestination = entryDestination,
                Id = entryNumberEntity.EntryNumberId,
                IsJournalEntry = Convert.ToBoolean(entryNumberEntity.IsJournalEntry),
                Number = Convert.ToInt32(entryNumberEntity.Number),
                Year = Convert.ToInt32(entryNumberEntity.LastGeneratedEntryYear)
            };
        }

        /// <summary>
        /// CreateEntryNumbers
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<EntryNumber> CreateEntryNumbers(BusinessCollection businessCollection)
        {
            List<EntryNumber> entryNumbers = new List<EntryNumber>();
            foreach (GENERALLEDGEREN.EntryNumber entryNumberEntity in businessCollection.OfType<GENERALLEDGEREN.EntryNumber>())
            {
                entryNumbers.Add(CreateEntryNumber(entryNumberEntity));
            }
            return entryNumbers;
        }

        #endregion EntryNumber

        #region AccountingMovementType

        /// <summary>
        /// CreateAccountingMovementType
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns></returns>
        public static AccountingMovementType CreateAccountingMovementType(GENERALLEDGEREN.AccountingMovementType accountingMovementType)
        {
            return new AccountingMovementType()
            {
                AccountingMovementTypeId = accountingMovementType.AccountingMovementTypeId,
                Description = accountingMovementType.Description,
                IsAutomatic = Convert.ToBoolean(accountingMovementType.IsAutomatic)
            };
        }

        /// <summary>
        /// CreateAccountingMovementTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingMovementType> CreateAccountingMovementTypes(BusinessCollection businessCollection)
        {
            List<AccountingMovementType> accountingMovementTypes = new List<AccountingMovementType>();
            foreach (GENERALLEDGEREN.AccountingMovementType accountingMovementTypeEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingMovementType>())
            {
                accountingMovementTypes.Add(CreateAccountingMovementType(accountingMovementTypeEntity));
            }
            return accountingMovementTypes;
        }

        #endregion AccountingMovementType

        #region PostDated

        /// <summary>
        /// CreatePostDated
        /// </summary>
        /// <param name="postdatedEntryItem"></param>
        /// <returns></returns>
        public static PostDated CreatePostDated(GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItem)
        {
            Amount amount = new Amount();
            amount.Currency = new Currency();
            amount.Value = Convert.ToDecimal(postdatedEntryItem.AmountValue);
            Amount localAmount = new Amount() { Value = Convert.ToDecimal(postdatedEntryItem.AmountLocalValue) };
            amount.Currency.Id = Convert.ToInt32(postdatedEntryItem.CurrencyCode);
            ExchangeRate exchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(postdatedEntryItem.ExchangeRate) };

            return new PostDated()
            {
                PostDatedId = postdatedEntryItem.PostdatedEntryItemId,
                PostDateType = (PostDateTypes)postdatedEntryItem.PostdatedType,
                Amount = amount,
                DocumentNumber = Convert.ToInt32(postdatedEntryItem.DocumentNumber),
                ExchangeRate = exchangeRate,
                LocalAmount = localAmount
            };
        }

        /// <summary>
        /// CreatePostDates
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<PostDated> CreatePostDates(BusinessCollection businessCollection)
        {
            List<PostDated> postDateds = new List<PostDated>();
            foreach (GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.PostdatedEntryItem>())
            {
                postDateds.Add(CreatePostDated(postdatedEntryItemEntity));
            }

            return postDateds;
        }

        #endregion PostDated

        #region EntryMassiveLoad

        /// <summary>
        /// CreateEntryMassiveLoad
        /// </summary>
        /// <param name="entryMassiveLoad"></param>
        /// <returns></returns>
        public static MassiveEntryDTO CreateEntryMassiveLoad(GENERALLEDGEREN.EntryMassiveLoad entryMassiveLoad)
        {
            return new MassiveEntryDTO()
            {
                Id = Convert.ToInt32(entryMassiveLoad.EntryMassiveLoadId),
                BranchId = Convert.ToInt32(entryMassiveLoad.BranchId),
                SalePointId = Convert.ToInt32(entryMassiveLoad.SalePointId),
                AccoutingCompanyId = Convert.ToInt32(entryMassiveLoad.AccountingCompanyId),
                EntryDestinationId = Convert.ToInt32(entryMassiveLoad.EntryDestinationId),
                AccountingMovementTypeId = Convert.ToInt32(entryMassiveLoad.AccountingMovementTypeId),
                OperationDate = Convert.ToDateTime(entryMassiveLoad.OperationDate),
                CurrencyId = Convert.ToInt32(entryMassiveLoad.CurrencyId),
                ExchangeRate = Convert.ToDecimal(entryMassiveLoad.ExchangeRate),
                AccountingAccountId = Convert.ToInt32(entryMassiveLoad.AccountingAccountId),
                AccountingNature = Convert.ToInt32(entryMassiveLoad.AccountingNature),
                Amount = Convert.ToDecimal(entryMassiveLoad.Amount),
                IndividualId = Convert.ToInt32(entryMassiveLoad.IndividualId),
                Description = entryMassiveLoad.Description,
                BankReconciliationId = entryMassiveLoad.BankReconciliationId,
                ReceiptNumber = Convert.ToInt32(entryMassiveLoad.ReceiptNumber),
                ReceiptDate = Convert.ToDateTime(entryMassiveLoad.ReceiptDate),
                CostCenterId = entryMassiveLoad.CostCenterId,
                Percentage = entryMassiveLoad.Percentage,
                AnalysisId = entryMassiveLoad.AnalysisId,
                ConceptId = entryMassiveLoad.ConceptId,
                ConceptKey = entryMassiveLoad.ConceptKey,
                AnalysisDescription = entryMassiveLoad.AnalysisDescription,
                PostdatedId = entryMassiveLoad.PostdatedId,
                PostdatedCurrencyId = entryMassiveLoad.PostdatedCurrencyId,
                PostdatedExchangeRate = Convert.ToDecimal(entryMassiveLoad.PostdatedExchangeRate),
                PosdatedDocumentNumber = entryMassiveLoad.PostdatedDocumentNumber,
                PostdatedAmount = entryMassiveLoad.PostdatedAmount
            };
        }

        /// <summary>
        /// CreateEntryMassiveLoads
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<MassiveEntryDTO> CreateEntryMassiveLoads(BusinessCollection businessCollection)
        {
            List<MassiveEntryDTO> massiveEntryDTOs = new List<MassiveEntryDTO>();
            foreach (GENERALLEDGEREN.EntryMassiveLoad entryMassiveLoadEntity in businessCollection.OfType<GENERALLEDGEREN.EntryMassiveLoad>())
            {
                massiveEntryDTOs.Add(CreateEntryMassiveLoad(entryMassiveLoadEntity));
            }
            return massiveEntryDTOs;
        }

        #endregion EntryMassiveLoad

        #region EntryMassiveLoadLog

        /// <summary>
        /// CreateEntryMassiveLoadLog
        /// </summary>
        /// <param name="entryMassiveLoadLog"></param>
        /// <returns></returns>
        public static MassiveEntryLogDTO CreateEntryMassiveLoadLog(GENERALLEDGEREN.EntryMassiveLoadLog entryMassiveLoadLog)
        {
            return new MassiveEntryLogDTO()
            {
                Id = entryMassiveLoadLog.Id,
                ProcessDate = Convert.ToDateTime(entryMassiveLoadLog.ProcessDate),
                OperationDate = Convert.ToDateTime(entryMassiveLoadLog.OperationDate),
                RowNumber = Convert.ToInt32(entryMassiveLoadLog.RowNumber),
                Success = Convert.ToBoolean(entryMassiveLoadLog.Success),
                Description = entryMassiveLoadLog.ErrorDescription,
                Enabled = Convert.ToBoolean(entryMassiveLoadLog.Enabled)
            };
        }

        /// <summary>
        /// CreateEntryMassiveLoadLogs
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<MassiveEntryLogDTO> CreateEntryMassiveLoadLogs(BusinessCollection businessCollection)
        {
            List<MassiveEntryLogDTO> massiveEntryLogDTOs = new List<MassiveEntryLogDTO>();
            foreach (GENERALLEDGEREN.EntryMassiveLoadLog entryMassiveLoadEntity in businessCollection.OfType<GENERALLEDGEREN.EntryMassiveLoadLog>())
            {
                massiveEntryLogDTOs.Add(CreateEntryMassiveLoadLog(entryMassiveLoadEntity));
            }
            return massiveEntryLogDTOs;
        }

        #endregion EntryMassiveLoadLog

        #region AccountReclassification

        /// <summary>
        /// CreateAccountReclassification
        /// </summary>
        /// <param name="reclassification"></param>
        /// <returns></returns>
        public static ReclassificationModels.AccountReclassification CreateAccountReclassification(GENERALLEDGEREN.Reclassification reclassification)
        {
            AccountingAccount sourceAccountingAccount = new AccountingAccount();
            sourceAccountingAccount.AccountingAccountId = Convert.ToInt32(reclassification.SourceAccountingAccountId);

            AccountingAccount destinationAccountingAccount = new AccountingAccount();
            destinationAccountingAccount.AccountingAccountId = Convert.ToInt32(reclassification.DestinationAccountingAccountId);

            return new ReclassificationModels.AccountReclassification
            {
                DestinationAccountingAccount = destinationAccountingAccount,
                Month = reclassification.ProcessMonth,
                OpeningBranch = reclassification.OpeningBranch,
                OpeningPrefix = reclassification.OpeningPrefix,
                SourceAccountingAccount = sourceAccountingAccount,
                Year = reclassification.ProcessYear
            };
        }

        /// <summary>
        /// CreateAccountReclassifications
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ReclassificationModels.AccountReclassification> CreateAccountReclassifications(BusinessCollection businessCollection)
        {
            var accountReclassifications = new List<ReclassificationModels.AccountReclassification>();
            foreach (GENERALLEDGEREN.Reclassification reclassificationEntity in businessCollection.OfType<GENERALLEDGEREN.Reclassification>())
            {
                accountReclassifications.Add(CreateAccountReclassification(reclassificationEntity));
            }
            return accountReclassifications;
        }

        #endregion AccountReclassification

        #region EntryReclassification

        /// <summary>
        /// CreateAccountingReclassification
        /// </summary>
        /// <param name="accountingReclassification"></param>
        /// <returns></returns>
        public static ReclassificationModels.AccountReclassificationResult CreateAccountingReclassification(GENERALLEDGEREN.AccountingReclassification accountingReclassification)
        {
            AccountingAccount sourceAccountingAccount = new AccountingAccount();
            sourceAccountingAccount.AccountingAccountId = Convert.ToInt32(accountingReclassification.SourceAccountingAccountId);

            AccountingAccount destinationAccountingAccount = new AccountingAccount();
            destinationAccountingAccount.AccountingAccountId = Convert.ToInt32(accountingReclassification.AccountingAccountId);

            AccountingNatures accountingNatures = accountingReclassification.AccountingNature == 1 ? AccountingNatures.Credit :
                                                                                                    AccountingNatures.Debit;

            Amount amount = new Amount()
            {
                Currency = new Currency()
                {
                    Id = accountingReclassification.CurrencyCode
                },
                Value = accountingReclassification.IncomeAmount
            };
            ExchangeRate exchangeRate = new ExchangeRate()
            {
                SellAmount = accountingReclassification.ExchangeRate
            };
            Amount localAmount = new Amount() { Value = accountingReclassification.Amount };

            Analysis analysis = new Analysis()
            {
                AnalysisConcept = new AnalysisConcept()
                {
                    AnalysisConceptId = Convert.ToInt32(accountingReclassification.AnalysisConceptCode)
                },
                AnalysisId = Convert.ToInt32(accountingReclassification.AnalysisCode),
                Key = accountingReclassification.ConceptKey
            };

            Branch branch = new Branch() { Id = accountingReclassification.BranchCode };
            CostCenter costCenter = new CostCenter()
            {
                CostCenterId = Convert.ToInt32(accountingReclassification.CostCenterCode)
            };

            return new ReclassificationModels.AccountReclassificationResult
            {
                AccountingNature = accountingNatures,
                Amount = amount,
                Analysis = analysis,
                Branch = branch,
                CostCenter = costCenter,
                DestinationAccountingAccount = destinationAccountingAccount,
                ExchangeRate = exchangeRate,
                Id = accountingReclassification.AccountingReclassificationId,
                LocalAmount = localAmount,
                Month = accountingReclassification.ProcessMonth,
                SourceAccountingAccount = sourceAccountingAccount,
                Year = accountingReclassification.ProcessYear
            };
        }

        /// <summary>
        /// CreateAccountingReclassifications
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ReclassificationModels.AccountReclassificationResult> CreateAccountingReclassifications(BusinessCollection businessCollection)
        {
            var accountingReclassifications = new List<ReclassificationModels.AccountReclassificationResult>();
            foreach (GENERALLEDGEREN.AccountingReclassification accountingReclassificationEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingReclassification>())
            {
                accountingReclassifications.Add(CreateAccountingReclassification(accountingReclassificationEntity));
            }
            return accountingReclassifications;
        }

        /// <summary>
        /// CreateReclassificationLog
        /// </summary>
        /// <param name="reclassificationLog"></param>
        /// <returns></returns>
        public static ProcessLog CreateReclassificationLog(GENERALLEDGEREN.ReclassificationLog reclassificationLog)
        {
            return new ProcessLog
            {
                EndDate = Convert.ToDateTime(reclassificationLog.EndDate),
                Id = reclassificationLog.ReclassificationLogId,
                ProcessLogStatus = (ProcessLogStatus)reclassificationLog.ProcessStatus,
                StartDate = reclassificationLog.StartDate,
                UserId = reclassificationLog.UserCode,
                Month = reclassificationLog.ProcessMonth,
                Year = reclassificationLog.ProcessYear
            };
        }

        /// <summary>
        /// CreateReclassificationLogs
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ProcessLog> CreateReclassificationLogs(BusinessCollection businessCollection)
        {
            var processLogs = new List<ProcessLog>();
            foreach (GENERALLEDGEREN.ReclassificationLog reclassificationLogEntity in businessCollection.OfType<GENERALLEDGEREN.ReclassificationLog>())
            {
                processLogs.Add(CreateReclassificationLog(reclassificationLogEntity));
            }
            return processLogs;
        }

        #endregion

        #region AccountingConcept

        /// <summary>
        /// CreateAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns></returns>
        public static ACMOD.AccountingConcept CreateAccountingConcept(GENERALLEDGEREN.AccountingConcept accountingConcept)
        {
            AccountingAccount accountingAccount = new AccountingAccount();
            accountingAccount.AccountingAccountId = Convert.ToInt32(accountingConcept.AccountingAccountId);

            return new ACMOD.AccountingConcept
            {
                AccountingAccount = accountingAccount,
                AgentEnabled = Convert.ToBoolean(accountingConcept.AgentEnabled),
                CoInsurancedEnabled = Convert.ToBoolean(accountingConcept.CoinsuranceEnabled),
                Description = accountingConcept.Description != null ? Convert.ToString(accountingConcept.Description) : "",
                Id = accountingConcept.AccountingConceptCode,
                InsuredEnabled = Convert.ToBoolean(accountingConcept.InsuredEnabled),
                ItemEnabled = Convert.ToBoolean(accountingConcept.ItemEnabled),
                ReInsuranceEnabled = Convert.ToBoolean(accountingConcept.ReinsuranceEnabled)
            };
        }

        /// <summary>
        /// CreateAccountingConcepts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ACMOD.AccountingConcept> CreateAccountingConcepts(BusinessCollection businessCollection)
        {
            List<ACMOD.AccountingConcept> accountingConcepts = new List<ACMOD.AccountingConcept>();
            foreach (GENERALLEDGEREN.AccountingConcept accountingConceptTypeEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingConcept>())
            {
                accountingConcepts.Add(CreateAccountingConcept(accountingConceptTypeEntity));
            }
            return accountingConcepts;
        }

        #endregion

        #region ConceptSource

        /// <summary>
        /// CreateConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns></returns>
        public static ConceptSource CreateConceptSource(GENERALLEDGEREN.ConceptSource conceptSource)
        {
            return new ConceptSource
            {
                Id = conceptSource.ConceptSourceCode,
                Description = conceptSource.Description
            };
        }

        /// <summary>
        /// CreateConceptSources
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ConceptSource> CreateConceptSources(BusinessCollection businessCollection)
        {
            List<ConceptSource> conceptSources = new List<ConceptSource>();
            foreach (GENERALLEDGEREN.ConceptSource conceptSourceTypeEntity in businessCollection.OfType<GENERALLEDGEREN.ConceptSource>())
            {
                conceptSources.Add(CreateConceptSource(conceptSourceTypeEntity));
            }
            return conceptSources;
        }


        #endregion

        #region MovementType

        /// <summary>
        /// CreateMovementType
        /// </summary>
        /// <param name="movementType">GENERALLEDGEREN</param>
        /// <returns>ModelAssembler</returns>
        public static MovementType CreateMovementType(GENERALLEDGEREN.MovementType movementType)
        {
            ConceptSource conceptSource = new ConceptSource();
            conceptSource.Id = Convert.ToInt32(movementType.ConceptSourceCode);
            return new MovementType
            {
                Id = movementType.MovementTypeCode,
                ConceptSource = conceptSource,
                Description = movementType.Description
            };
        }

        /// <summary>
        /// CreateMovementTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<MovementType> CreateMovementTypes(BusinessCollection businessCollection)
        {
            List<MovementType> movementType = new List<MovementType>();
            foreach (GENERALLEDGEREN.MovementType movementTypeTypeEntity in businessCollection.OfType<GENERALLEDGEREN.MovementType>())
            {
                movementType.Add(CreateMovementType(movementTypeTypeEntity));
            }
            return movementType;
        }

        /// <summary>
        /// CreateMovementTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<MovementType> CreateMovementTypesFilter(BusinessCollection businessCollection)
        {
            List<MovementType> movementType = new List<MovementType>();
            foreach (GENERALLEDGEREN.MovementType movementTypeTypeEntity in businessCollection.OfType<GENERALLEDGEREN.MovementType>())
            {
                movementType.Add(CreateMovementType(movementTypeTypeEntity));
            }
            return movementType.Where(x => x.Description != "COASEGURADORES" && x.Description != "REASEGURADORAS").ToList();

        }

        #endregion

        #region BranchAccountingConcept

        /// <summary>
        /// CreateBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept">GENERALLEDGEREN</param>
        /// <returns>ModelAssembler</returns>
        public static BranchAccountingConcept CreateBranchAccountingConcept(GENERALLEDGEREN.BranchAccountingConcept branchAccountingConcept)
        {
            Branch branch = new Branch();
            branch.Id = Convert.ToInt32(branchAccountingConcept.BranchCode);

            ACMOD.AccountingConcept accountingConcept = new ACMOD.AccountingConcept();
            accountingConcept.Id = Convert.ToInt32(branchAccountingConcept.AccountingConceptCode);

            MovementType movementType = new MovementType()
            {
                Id = branchAccountingConcept.MovementTypeCode,
                ConceptSource = new ConceptSource()
                {
                    Id = Convert.ToInt32(branchAccountingConcept.ConceptSourceCode)
                },
            };

            return new BranchAccountingConcept
            {
                Id = branchAccountingConcept.BranchAccountingConceptId,
                Branch = branch,
                AccountingConcept = accountingConcept,
                MovementType = movementType
            };
        }

        /// <summary>
        /// CreateBranchAccountingConcepts
        /// </summary>
        /// <param name="businessCollection">businessCollection</param>
        /// <returns>List_ModelAssembler</returns>
        public static List<BranchAccountingConcept> CreateBranchAccountingConcepts(BusinessCollection businessCollection)
        {
            List<BranchAccountingConcept> branchAccountingConcepts = new List<BranchAccountingConcept>();
            foreach (GENERALLEDGEREN.BranchAccountingConcept branchAccountingConceptEntity in businessCollection.OfType<GENERALLEDGEREN.BranchAccountingConcept>())
            {
                branchAccountingConcepts.Add(CreateBranchAccountingConcept(branchAccountingConceptEntity));
            }
            return branchAccountingConcepts;
        }

        #endregion

        #region UserBranchAccountingConcept

        /// <summary>
        /// CreateUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept">GENERALLEDGEREN</param>
        /// <returns>ModelAssembler</returns>
        public static UserBranchAccountingConcept CreateUserBranchAccountingConcept(GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConcept)
        {
            BranchAccountingConcept branchAccountingConcept = new BranchAccountingConcept();
            branchAccountingConcept.Id = Convert.ToInt32(userBranchAccountingConcept.BranchAccountingConceptId);

            return new UserBranchAccountingConcept
            {
                Id = userBranchAccountingConcept.UserBranchAccountingConceptId,
                BranchAccountingConcept = branchAccountingConcept,
                UserId = Convert.ToInt32(userBranchAccountingConcept.UserCode)
            };
        }

        /// <summary>
        /// CreateUserBranchAccountingConcepts
        /// </summary>
        /// <param name="businessCollection">businessCollection</param>
        /// <returns>List_ModelAssembler</returns>
        public static List<UserBranchAccountingConcept> CreateUserBranchAccountingConcepts(BusinessCollection businessCollection)
        {
            List<UserBranchAccountingConcept> userBranchAccountingConcepts = new List<UserBranchAccountingConcept>();
            foreach (GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConceptEntity in businessCollection.OfType<GENERALLEDGEREN.UserBranchAccountingConcept>())
            {
                userBranchAccountingConcepts.Add(CreateUserBranchAccountingConcept(userBranchAccountingConceptEntity));
            }
            return userBranchAccountingConcepts;
        }

        #endregion

        #endregion GL

        #region PARAM

        #region AccountingCompany

        /// <summary>
        /// CreateAccountingCompany
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns></returns>
        public static AccountingCompany CreateAccountingCompany(GENERALLEDGEREN.AccountingCompany accountingCompany)
        {
            return new AccountingCompany
            {
                AccountingCompanyId = accountingCompany.AccountingCompanyId,
                Description = accountingCompany.Description,
                Default = Convert.ToBoolean(accountingCompany.Default)
            };
        }

        /// <summary>
        /// CreateAccountingCompanies
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingCompany> CreateAccountingCompanies(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.AccountingCompany accountingCompanyEntity in businessCollection select CreateAccountingCompany(accountingCompanyEntity)).ToList();
        }

        #endregion AccountingCompany

        #region CostCenter

        /// <summary>
        /// CreateCostCenter
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns></returns>
        public static CostCenter CreateCostCenter(GENERALLEDGEREN.CostCenter costCenter)
        {
            CostCenterType costCenterType = new CostCenterType();
            costCenterType.CostCenterTypeId = Convert.ToInt32(costCenter.CostCenterTypeId);

            return new CostCenter
            {
                CostCenterId = costCenter.CostCenterId,
                Description = costCenter.Description,
                IsProrated = Convert.ToBoolean(costCenter.IsProrated),
                CostCenterType = costCenterType
            };
        }

        /// <summary>
        /// CreateCostCenters
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<CostCenter> CreateCostCenters(BusinessCollection businessCollection)
        {
            var costCenters = new List<CostCenter>();
            foreach (GENERALLEDGEREN.CostCenter costCenterEntity in businessCollection.OfType<GENERALLEDGEREN.CostCenter>())
            {
                costCenters.Add(CreateCostCenter(costCenterEntity));
            }
            return costCenters;
        }

        #endregion CostCenter

        #region Analysis

        /// <summary>
        /// CreateAnalysis
        /// </summary>
        /// <param name="entryAnalysis"></param>
        /// <returns></returns>
        public static Analysis CreateAnalysis(GENERALLEDGEREN.AnalysisEntryItem analysisEntryItem)
        {
            AnalysisConcept analysisConcept = new AnalysisConcept();
            analysisConcept.AnalysisConceptId = Convert.ToInt32(analysisEntryItem.AnalysisConceptId);
            analysisConcept.AnalysisCode = new GeneralLedgerModels.AnalysisCode();
            analysisConcept.AnalysisCode.AnalysisCodeId = Convert.ToInt32(analysisEntryItem.AnalysisId);

            return new Analysis
            {
                AnalysisId = analysisEntryItem.AnalysisEntryItemId,
                AnalysisConcept = analysisConcept,
                Key = analysisEntryItem.ConceptKey,
                Description = analysisEntryItem.Description,
                IsJournalEntry = Convert.ToBoolean(analysisEntryItem.IsJournalEntry),
                Id = Convert.ToInt32(analysisEntryItem.AnalysisEntryItemId),
                EntryItemId = Convert.ToInt32(analysisEntryItem.EntryItemId)
            };
        }

        /// <summary>
        /// CreateAnalyses
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Analysis> CreateAnalyses(BusinessCollection businessCollection)
        {
            List<Analysis> analysis = new List<Analysis>();
            foreach (GENERALLEDGEREN.AnalysisEntryItem entryAnalysisEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisEntryItem>())
            {
                analysis.Add(CreateAnalysis(entryAnalysisEntity));
            }
            return analysis;
        }

        #endregion Analysis

        #region AnalysisCode

        /// <summary>
        /// CreateAnalysisCode
        /// </summary>
        /// <param name="entityAnalysis"></param>
        /// <returns></returns>
        public static AnalysisCode CreateAnalysisCode(GENERALLEDGEREN.Analysis entityAnalysis)
        {
            return new AnalysisCode
            {
                AnalysisCodeId = entityAnalysis.AnalysisId,
                Description = entityAnalysis.Description,
                AccountingNature = (AccountingNatures)entityAnalysis.AccountingNature,
                CheckModuleType = Convert.ToBoolean(entityAnalysis.RequireOrigin),
                CheckBalance = Convert.ToBoolean(entityAnalysis.ControlBalance)
            };
        }

        /// <summary>
        /// CreateAnalysisCodes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AnalysisCode> CreateAnalysisCodes(BusinessCollection businessCollection)
        {
            List<AnalysisCode> analysisCodes = new List<AnalysisCode>();
            foreach (GENERALLEDGEREN.Analysis analysisEntity in businessCollection.OfType<GENERALLEDGEREN.Analysis>())
            {
                analysisCodes.Add(CreateAnalysisCode(analysisEntity));
            }
            return analysisCodes;
        }

        #endregion AnalysisCode

        #region AnalysisTreatment

        /// <summary>
        /// CreateAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns></returns>
        public static AnalysisTreatment CreateAnalysisTreatment(GENERALLEDGEREN.AnalysisTreatment analysisTreatment)
        {
            return new AnalysisTreatment
            {
                AnalysisTreatmentId = analysisTreatment.AnalysisTreatmentId,
                Description = analysisTreatment.Description
            };
        }

        /// <summary>
        /// CreateAnalysisTreatments
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AnalysisTreatment> CreateAnalysisTreatments(BusinessCollection businessCollection)
        {
            List<AnalysisTreatment> analysisTreatments = new List<AnalysisTreatment>();
            foreach (GENERALLEDGEREN.AnalysisTreatment analysisTreatmentEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisTreatment>())
            {
                analysisTreatments.Add(CreateAnalysisTreatment(analysisTreatmentEntity));
            }
            return analysisTreatments;
        }

        #endregion AnalysisTreatment

        #region AnalysisConcept

        /// <summary>
        /// CreateAnalysisConcept
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns></returns>
        public static AnalysisConcept CreateAnalysisConcept(GENERALLEDGEREN.AnalysisConcept analysisConcept)
        {
            AnalysisTreatment analysisTreatment = new AnalysisTreatment();
            analysisTreatment.AnalysisTreatmentId = Convert.ToInt32(analysisConcept.AnalysisTreatmentId);

            return new AnalysisConcept
            {
                AnalysisConceptId = analysisConcept.AnalysisConceptId,
                Description = analysisConcept.Description,
                AnalysisTreatment = analysisTreatment
            };
        }

        /// <summary>
        /// CreateAnalysisConcepts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AnalysisConcept> CreateAnalysisConcepts(BusinessCollection businessCollection)
        {
            var analysisConcepts = new List<AnalysisConcept>();
            foreach (GENERALLEDGEREN.AnalysisConcept analysisConceptTypeEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisConcept>())
            {
                analysisConcepts.Add(CreateAnalysisConcept(analysisConceptTypeEntity));
            }
            return analysisConcepts;
        }

        #endregion AnalysisConcept

        #region AnalysisConceptKey

        /// <summary>
        /// Model CreateAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns></returns>
        public static AnalysisConceptKey CreateAnalysisConceptKey(GENERALLEDGEREN.AnalysisConceptKey analysisConceptKey)
        {

            AnalysisConcept analysisConcept = new AnalysisConcept();
            analysisConcept.AnalysisConceptId = Convert.ToInt32(analysisConceptKey.AnalysisConceptId);

            return new AnalysisConceptKey
            {
                Id = analysisConceptKey.AnalysisConceptKeyId,
                AnalysisConcept = analysisConcept,
                TableName = analysisConceptKey.Table,
                ColumnName = analysisConceptKey.Column,
                ColumnDescription = analysisConceptKey.ColumnDescription
            };
        }

        /// <summary>
        /// Model List CreateAnalysisConceptKeys
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AnalysisConceptKey> CreateAnalysisConceptKeys(BusinessCollection businessCollection)
        {
            List<AnalysisConceptKey> analysisConceptKeys = new List<AnalysisConceptKey>();
            foreach (GENERALLEDGEREN.AnalysisConceptKey analysisConceptKeyTypeEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisConceptKey>())
            {
                analysisConceptKeys.Add(CreateAnalysisConceptKey(analysisConceptKeyTypeEntity));
            }
            return analysisConceptKeys;
        }
        #endregion

        #region CostCenterType

        /// <summary>
        /// CreateCostCenterType
        /// </summary>
        /// <param name="costCenterType"></param>
        /// <returns></returns>
        public static CostCenterType CreateCostCenterType(GENERALLEDGEREN.CostCenterType costCenterType)
        {
            return new CostCenterType
            {
                CostCenterTypeId = Convert.ToInt32(costCenterType.CostCenterTypeId),
                Description = Convert.ToString(costCenterType.Description)
            };
        }

        /// <summary>
        /// CreateCostCenterTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<CostCenterType> CreateCostCenterTypes(BusinessCollection businessCollection)
        {
            var costCenterTypes = new List<CostCenterType>();
            foreach (GENERALLEDGEREN.CostCenterType costCenterTypeEntity in businessCollection.OfType<GENERALLEDGEREN.CostCenterType>())
            {
                costCenterTypes.Add(CreateCostCenterType(costCenterTypeEntity));
            }
            return costCenterTypes;
        }

        #endregion CostCenterType

        #region Destination

        /// <summary>
        /// CreateEntryDestination
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns></returns>
        public static EntryDestination CreateEntryDestination(GENERALLEDGEREN.EntryDestination entryDestination)
        {
            return new EntryDestination
            {
                DestinationId = entryDestination.EntryDestinationId,
                Description = entryDestination.Description
            };
        }

        /// <summary>
        /// CreateDestinations
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<EntryDestination> CreateEntryDestinations(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.EntryDestination entryDestinationEntity in businessCollection select CreateEntryDestination(entryDestinationEntity)).ToList();
        }

        #endregion Destination

        #region EntryType

        /// <summary>
        /// CreateEntryType
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns></returns>
        public static EntryType CreateEntryType(GENERALLEDGEREN.EntryType entryType)
        {
            return new EntryType
            {
                EntryTypeId = entryType.EntryTypeId,
                Description = entryType.Description,
                SmallDescription = entryType.SmallDescription
            };
        }

        /// <summary>
        /// CreateEntryTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<EntryType> CreateEntryTypes(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.EntryType entryTypeEntity in businessCollection select CreateEntryType(entryTypeEntity)).ToList();
        }

        #endregion EntryType

        #region EntryTypeItem

        /// <summary>
        /// CreateEntryTypeItem
        /// </summary>
        /// <param name="entryTypeItem"></param>
        /// <returns></returns>
        public static EntryTypeItem CreateEntryTypeItem(GENERALLEDGEREN.EntryTypeItem entryTypeItem)
        {
            AccountingAccount accountingAccount = new AccountingAccount();
            accountingAccount.AccountingAccountId = Convert.ToInt32(entryTypeItem.AccountingAccountId);

            AccountingMovementType accountingMovementType = new AccountingMovementType();
            accountingMovementType.AccountingMovementTypeId = Convert.ToInt32(entryTypeItem.AccountingMovementTypeId);

            Analysis analysis = new Analysis();
            analysis.AnalysisId = Convert.ToInt32(entryTypeItem.AnalysisId);

            ACMOD.AccountingConcept accountingConcept = new ACMOD.AccountingConcept();
            accountingConcept.Id = Convert.ToInt32(entryTypeItem.AccountingConceptCode);

            CostCenter costCenter = new CostCenter();
            costCenter.CostCenterId = Convert.ToInt32(entryTypeItem.CostCenterId);

            Currency currency = new Currency();
            currency.Id = Convert.ToInt32(entryTypeItem.CurrencyCode);

            return new EntryTypeItem
            {
                AccountingAccount = accountingAccount,
                AccountingConcept = accountingConcept,
                AccountingMovementType = accountingMovementType,
                AccountingNature = (AccountingNatures)(entryTypeItem.AccountingNature),
                Analysis = analysis,
                CostCenter = costCenter,
                Currency = currency,
                Description = entryTypeItem.Description,
                Id = entryTypeItem.EntryTypeItemId
            };
        }

        /// <summary>
        /// CreateEntryTypeItems
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<EntryTypeItem> CreateEntryTypeItems(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.EntryTypeItem entryTypeItemEntity in businessCollection select CreateEntryTypeItem(entryTypeItemEntity)).ToList();
        }

        #endregion EntryTypeItem

        #region EntryParameter

        #region Operator

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="operatorEntity"></param>
        /// <returns></returns>
        public static OperatorDTO CreateOperator(GENERALLEDGEREN.Operator operatorEntity)
        {
            return new OperatorDTO()
            {
                Id = operatorEntity.OperatorId,
                OperationSign = operatorEntity.OperationSign,
                Description = operatorEntity.Description
            };
        }

        /// <summary>
        /// CreateActions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<OperatorDTO> CreateOperators(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.Operator operatorEntity in businessCollection select CreateOperator(operatorEntity)).ToList();
        }

        #endregion Operator

        #region Parameter

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GeneralLedgerModels.AccountingRules.Parameter CreateParameter(GENERALLEDGEREN.Parameter parameter)
        {
            return new GeneralLedgerModels.AccountingRules.Parameter()
            {
                Id = Convert.ToInt32(parameter.ParameterId),
                Description = parameter.Description,
                DataType = Convert.ToString(parameter.DataTypeCode),
                Order = Convert.ToInt32(parameter.Order),
                ModuleDateId = Convert.ToInt32(parameter.ModuleCode)
            };
        }

        /// <summary>
        /// CreateParameters
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Models.AccountingRules.Parameter> CreateParameters(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.Parameter parameterEntity in businessCollection select CreateParameter(parameterEntity)).ToList();
        }

        #endregion Parameter

        #region AccountingRule

        /// <summary>
        /// CreateAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns></returns>
        public static AccountingRule CreateAccountingRule(GENERALLEDGEREN.AccountingRule accountingRule)
        {
            return new AccountingRule()
            {
                Id = accountingRule.AccountingRuleId,
                ModuleDateId = Convert.ToInt32(accountingRule.ModuleCode),
                Observation = accountingRule.Observations,
                Description = accountingRule.Description
            };
        }

        /// <summary>
        /// CreateAccountingRules
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingRule> CreateAccountingRules(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.AccountingRule accountingRuleEntity in businessCollection select CreateAccountingRule(accountingRuleEntity)).ToList();
        }

        #endregion AccountingRule

        #region AccountingRulePackage

        /// <summary>
        /// CreateParameterEntry
        /// </summary>
        /// <param name="entityAccountingRulePackage"></param>
        /// <returns></returns>
        public static AccountingRulePackage CreateAccountingRulePackage(GENERALLEDGEREN.AccountingRulePackage entityAccountingRulePackage)
        {
            return new AccountingRulePackage()
            {
                Id = entityAccountingRulePackage.AccountingRulePackageId,
                Description = entityAccountingRulePackage.Description,
                ModuleDateId = Convert.ToInt32(entityAccountingRulePackage.ModuleCode),
                RulePackageId = entityAccountingRulePackage.RulePackageCode
            };
        }

        /// <summary>
        /// CreateParameterEntries
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingRulePackage> CreateAccountingRulePackages(BusinessCollection businessCollection)
        {
            return (from GENERALLEDGEREN.AccountingRulePackage accountingRulePackageEntity in businessCollection select CreateAccountingRulePackage(accountingRulePackageEntity)).ToList();
        }

        #endregion AccountingRulePackage

        #region Condition

        /// <summary>
        /// CreateCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Models.AccountingRules.Condition CreateCondition(GENERALLEDGEREN.Condition condition)
        {
            Models.AccountingRules.Parameter parameter = new Models.AccountingRules.Parameter();
            parameter.Id = Convert.ToInt32(condition.ParameterId);

            Models.AccountingRules.AccountingRule accountingRule = new Models.AccountingRules.AccountingRule();
            accountingRule.Id = Convert.ToInt32(condition.AccountingRuleId);

            return new Condition()
            {
                Id = condition.ConditionId,
                AccountingRule = accountingRule,
                Parameter = parameter,
                Operator = condition.Operator,
                Value = Convert.ToDecimal(condition.Value),
                IdRightCondition = Convert.ToInt32(condition.RightConditionId),
                IdLeftCondition = Convert.ToInt32(condition.LeftConditionId),
                IdResult = Convert.ToInt32(condition.ResultId)
            };
        }

        /// <summary>
        /// CreateConditions
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Models.AccountingRules.Condition> CreateConditions(BusinessCollection businessCollection)
        {
            var conditions = new List<Condition>();
            foreach (GENERALLEDGEREN.Condition condition in businessCollection.OfType<GENERALLEDGEREN.Condition>())
            {
                conditions.Add(CreateCondition(condition));
            }
            return conditions;
        }

        #endregion Condition

        #region Result

        /// <summary>
        /// CreateResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Models.AccountingRules.Result CreateResult(GENERALLEDGEREN.Result result)
        {
            Models.AccountingRules.Parameter parameter = new Models.AccountingRules.Parameter();
            parameter.Id = Convert.ToInt32(result.ParameterId);

            return new Result()
            {
                Id = result.ResultId,
                AccountingNature = (AccountingNatures)(result.AccountingNatureId),
                AccountingAccount = Convert.ToString(result.AccountingAccountNumber),
                Parameter = parameter
            };
        }

        #endregion Result

        #region AccountingAccountMask

        /// <summary>
        /// CreateAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        /// <returns></returns>
        public static AccountingAccountMask CreateAccountingAccountMask(GENERALLEDGEREN.AccountingAccountMask accountingAccountMask)
        {
            Models.AccountingRules.Parameter parameter = new Models.AccountingRules.Parameter();
            parameter.Id = Convert.ToInt32(accountingAccountMask.ParameterId);

            return new AccountingAccountMask()
            {
                Id = accountingAccountMask.AccountingAccountMaskId,
                Mask = accountingAccountMask.Mask,
                Parameter = parameter,
                Start = Convert.ToInt32(accountingAccountMask.StartPosition)
            };
        }

        /// <summary>
        /// CreateAccountingAccountMasks
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AccountingAccountMask> CreateAccountingAccountMasks(BusinessCollection businessCollection)
        {
            var accountingAccountMasks = new List<AccountingAccountMask>();
            foreach (GENERALLEDGEREN.AccountingAccountMask accountingAccountMask in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountMask>())
            {
                accountingAccountMasks.Add(CreateAccountingAccountMask(accountingAccountMask));
            }
            return accountingAccountMasks;
        }

        #endregion AccountingAccountMask

        #endregion EntryParameter

        #region ReconciliationMovementTypes

        /// <summary>
        /// CreateReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliation"></param>
        /// <returns></returns>
        public static ReconciliationMovementType CreateReconciliationMovementType(GENERALLEDGEREN.BankReconciliation bankReconciliation)
        {
            return new ReconciliationMovementType()
            {
                AccountingNature = Convert.ToBoolean(bankReconciliation.DebitCompany) ? AccountingNatures.Debit : AccountingNatures.Credit,
                Description = bankReconciliation.Description,
                Id = bankReconciliation.BankReconciliationId,
                SmallDescription = bankReconciliation.ShortDescription
            };
        }

        /// <summary>
        /// CreateReconciliationMovementTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ReconciliationMovementType> CreateReconciliationMovementTypes(BusinessCollection businessCollection)
        {
            List<ReconciliationMovementType> reconciliationMovementTypes = new List<ReconciliationMovementType>();
            foreach (GENERALLEDGEREN.BankReconciliation bankReconciliationEntity in businessCollection.OfType<GENERALLEDGEREN.BankReconciliation>())
            {
                reconciliationMovementTypes.Add(CreateReconciliationMovementType(bankReconciliationEntity));
            }

            return reconciliationMovementTypes;
        }

        #endregion

        #endregion PARAM     
    }
}