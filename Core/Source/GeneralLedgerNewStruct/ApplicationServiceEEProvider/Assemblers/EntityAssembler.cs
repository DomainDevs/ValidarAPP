//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using GeneralLedgerEntities = Sistran.Core.Application.GeneralLedger.Entities;
using ReclassificationModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification;
using RuleModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;
using INTEN = Sistran.Core.Application.Integration.Entities;
//Sistran FWK
using Sistran.Core.Framework.BAF;

using System;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using System.Collections.Generic;
using TAXMOD = Sistran.Core.Application.TaxServices.Models;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        #region AccountingAccount

        /// <summary>
        /// CreateAccountingAccount
        /// </summary>
        /// <param name="accountingAccount"> </param>
        /// <returns>AccountingCompany</returns>
        public static GeneralLedgerEntities.AccountingAccount CreateAccountingAccount(AccountingAccount accountingAccount)
        {
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

            //explicar esta parte pq se pasa un id y luego se construye un objeto.
            return new GeneralLedgerEntities.AccountingAccount(accountingAccount.AccountingAccountId)
            {
                AccountingAccountId = accountingAccount.AccountingAccountId,
                AccountNumber = accountingAccount.Number,
                AccountName = accountingAccount.Description,
                AccountingNature = Convert.ToInt32(accountingAccount.AccountingNature),
                AccountingAccountParentId = accountingAccount.AccountingAccountParentId,
                DefaultCurrencyCode = currencyId,
                DefaultBranchCode = branchId,
                RequireCostCenter = accountingAccount.RequiresCostCenter,
                RequireAnalysis = accountingAccount.RequiresAnalysis,
                AnalysisId = analysisId,
                AccountTypeId = Convert.ToInt32(accountingAccount.AccountingAccountType.Id),
                AccountApplication = Convert.ToInt32(accountingAccount.AccountingAccountApplication),
                IsOfficialNomenclature = (accountingAccount.AccountingAccountApplication == AccountingAccountApplications.Others), //Cuando la aplicaci�n es "Otros" se habilita el campo "Nomeclatura Oficial"
                Comments = accountingAccount.Comments,
                IsMultibranch = branchId == null,
                IsMulticurrency = currencyId == null,
                IsReclassify = accountingAccount.IsReclassify,
                RecAccounting = accountingAccount.RecAccounting,
                IsRevalue = accountingAccount.IsRevalue,
                RevAcountingPos = accountingAccount.RevAcountingPos,
                RevAcountingNeg = accountingAccount.RevAcountingNeg
            };
        }

        /// <summary>
        /// CreateAccountingAccountCostCenter
        /// </summary>
        /// <param name="accountingAccountCostCenter"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingAccountCostCenter CreateAccountingAccountCostCenter(int accountingAccountCostCenter, int accountingAccountId, int costCenterId)
        {
            return new GeneralLedgerEntities.AccountingAccountCostCenter(accountingAccountCostCenter)
            {
                AccountingAccountCostCenterId = accountingAccountCostCenter,
                AccountingAccountId = accountingAccountId,
                CostCenterId = costCenterId,
            };
        }

        /// <summary>
        /// CreateAccountingAccountPrefix
        /// </summary>
        /// <param name="accountingAccountPrefix"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingAccountPrefix CreateAccountingAccountPrefix(int accountingAccountPrefix, int accountingAccountId, int prefixId)
        {
            int? accountingAccountCode = null;
            int? prefixCode = null;

            if (accountingAccountId > 0)
            {
                accountingAccountCode = accountingAccountId;
            }

            if (prefixId > 0)
            {
                prefixCode = prefixId;
            }

            return new GeneralLedgerEntities.AccountingAccountPrefix(accountingAccountPrefix)
            {
                AccountingAccountPrefixId = accountingAccountPrefix,
                AccountingAccountId = accountingAccountCode,
                PrefixCode = prefixCode
            };
        }

        #endregion AccountingAccount

        #region AccountingAccountParent

        /// <summary>
        /// CreateAccountingAccountParent
        /// </summary>
        /// <param name="accountingAccount"> </param>
        /// <returns>AccountingCompany</returns>
        public static GeneralLedgerEntities.AccountingAccountParent CreateAccountingAccountParent(AccountingAccount accountingAccount)
        {
            return new GeneralLedgerEntities.AccountingAccountParent(accountingAccount.AccountingAccountId)
            {
                Id = accountingAccount.AccountingAccountId,
                Description = accountingAccount.Description
            };
        }

        #endregion AccountingAccountParent

        #region LedgerEntry

        /// <summary>
        /// CreateLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.LedgerEntry CreateLedgerEntry(LedgerEntry ledgerEntry)
        {
            int? salePointId = null;
            int? companyId = null;

            if (ledgerEntry.SalePoint.Id > 0)
            {
                salePointId = ledgerEntry.SalePoint.Id;
            }

            if (ledgerEntry.AccountingCompany.AccountingCompanyId > 0)
            {
                companyId = ledgerEntry.AccountingCompany.AccountingCompanyId;
            }

            return new GeneralLedgerEntities.LedgerEntry(ledgerEntry.Id)
            {
                AccountingCompanyId = Convert.ToInt32(companyId),
                AccountingDate = ledgerEntry.AccountingDate.Date,
                AccountingModuleId = ledgerEntry.ModuleDateId,
                AccountingMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId,
                BranchCode = ledgerEntry.Branch.Id,
                Description = ledgerEntry.Description,
                EntryDestinationId = ledgerEntry.EntryDestination.DestinationId,
                EntryNumber = ledgerEntry.EntryNumber,
                LedgerEntryId = ledgerEntry.Id,
                RegisterDate = DateTime.Now,
                SalePointCode = salePointId,
                UserCode = ledgerEntry.UserId
            };
        }

        /// <summary>
        /// CreateLedgerEntryItem
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="ledgerEntryId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.LedgerEntryItem CreateLedgerEntryItem(LedgerEntryItem ledgerEntryItem, int ledgerEntryId, bool isJournalEntry)
        {
            int? bankReconciliationId = null;
            int? individualId = null;
            int? accountingAccountCode = null;

            if (ledgerEntryItem.ReconciliationMovementType.Id > 0)
            {
                bankReconciliationId = Convert.ToInt32(ledgerEntryItem.ReconciliationMovementType.Id);
            }

            if (ledgerEntryItem.Individual.IndividualId > 0)
            {
                individualId = Convert.ToInt32(ledgerEntryItem.Individual.IndividualId);
            }

            if (ledgerEntryItem.AccountingAccount.AccountingAccountId > 0)
            {
                accountingAccountCode = ledgerEntryItem.AccountingAccount.AccountingAccountId;
            }

            return new GeneralLedgerEntities.LedgerEntryItem(ledgerEntryItem.Id)
            {
                AccountingAccountId = accountingAccountCode,
                AccountingNature = Convert.ToInt32(ledgerEntryItem.AccountingNature),
                AmountLocalValue = Convert.ToDecimal(ledgerEntryItem.LocalAmount.Value),
                AmountValue = Convert.ToDecimal(ledgerEntryItem.Amount.Value),
                BankReconciliationId = bankReconciliationId,
                CurrencyCode = ledgerEntryItem.Currency.Id,
                Description = ledgerEntryItem.Description,
                DueDate = null,
                ExchangeRate = ledgerEntryItem.ExchangeRate.SellAmount,
                IndividualId = individualId,
                IsJournalEntry = isJournalEntry,
                LedgerEntryId = ledgerEntryId,
                LedgerEntryItemId = ledgerEntryItem.Id,
                ReceiptDate = ledgerEntryItem.Receipt.Date == Convert.ToDateTime("01/01/0001 0:00:00") ? null : ledgerEntryItem.Receipt.Date,
                ReceiptNumber = ledgerEntryItem.Receipt.Number == 0 ? null : ledgerEntryItem.Receipt.Number,
                ReconciliationCode = null,
                ReconciliationDate = null
            };
        }

        #endregion

        #region JournalEntry

        /// <summary>
        /// CreateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.JournalEntry CreateJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                int? accountingCompanyId = 0;
                int? salePointId = 0;

                if (journalEntry.AccountingCompany == null 
                    || journalEntry.AccountingCompany.AccountingCompanyId <= 0)
                {
                    accountingCompanyId = null;
                }
                else
                {
                    accountingCompanyId = journalEntry.AccountingCompany.AccountingCompanyId;
                }

                if (journalEntry.SalePoint == null || journalEntry.SalePoint.Id == 0)
                {
                    salePointId = null;
                }
                else
                {
                    salePointId = journalEntry.SalePoint.Id;
                }
                
                if (journalEntry.AccountingDate <= DateTime.MinValue )
                {
                    journalEntry.AccountingDate = DateTime.Now;
                }

                return new GeneralLedgerEntities.JournalEntry(journalEntry.Id)
                {
                    JournalEntryId = journalEntry.Id,
                    AccountingCompanyId = accountingCompanyId,
                    AccountingModuleId = journalEntry.ModuleDateId,
                    BranchCode = journalEntry.Branch.Id,
                    SalePointCode = salePointId,
                    TechnicalTransaction = journalEntry.TechnicalTransaction,
                    EntryNumber = journalEntry.EntryNumber,
                    Description = journalEntry.Description,
                    RegisterDate = journalEntry.RegisterDate,
                    UserCode = journalEntry.UserId,
                    Status = journalEntry.Status,  
                    AccountingDate = journalEntry.AccountingDate
                };
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
        /// <param name="journalEntryId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.JournalEntryItem CreateJournalEntryItem(JournalEntryItem journalEntryItem, int journalEntryId)
        {
            try
            {
                int? accountingAccountId = null;
                if (journalEntryItem.AccountingAccount != null && journalEntryItem.AccountingAccount.AccountingAccountId > 0)
                {
                    accountingAccountId = Convert.ToInt32(journalEntryItem.AccountingAccount.AccountingAccountId);
                }

                return new GeneralLedgerEntities.JournalEntryItem(journalEntryItem.Id)
                {
                    JournalEntryItemId = journalEntryItem.Id,
                    JournalEntryId = journalEntryId,
                    CurrencyCode = journalEntryItem.Currency.Id,
                    AccountingAccountId = accountingAccountId,
                    BankReconciliationId = journalEntryItem.ReconciliationMovementType.Id == 0 ? (int?)null : journalEntryItem.ReconciliationMovementType.Id,
                    ReceiptNumber = journalEntryItem.Receipt.Number == 0 ? null : journalEntryItem.Receipt.Number,
                    ReceiptDate = journalEntryItem.Receipt.Date == Convert.ToDateTime("01/01/0001 0:00:00") ? null : journalEntryItem.Receipt.Date,
                    AccountingNature = Convert.ToInt32(journalEntryItem.AccountingNature),
                    Description = journalEntryItem.Description,
                    Amount = journalEntryItem.Amount.Value,
                    LocalAmount = journalEntryItem.LocalAmount.Value,
                    ExchangeRate = journalEntryItem.ExchangeRate.SellAmount,
                    IndividualId = journalEntryItem.Individual.IndividualId,
                    SourceCode = journalEntryItem.SourceCode,
                    AccountingConceptCode = journalEntryItem.AccountingConcept,
                    BranchCode = journalEntryItem.Branch != null ? journalEntryItem.Branch.Id : 0,
                    SalePointCode = journalEntryItem.SalePoint != null ? journalEntryItem.SalePoint.Id : 0   
                };
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
        /// <param name="entryNumber"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryNumber CreateEntryNumber(EntryNumber entryNumber)
        {
            int? accountingMovementId = 0;
            int? destinationId = 0;

            if (entryNumber.AccountingMovementType.AccountingMovementTypeId == 0)
            {
                accountingMovementId = null;
            }
            else
            {
                accountingMovementId = entryNumber.AccountingMovementType.AccountingMovementTypeId;
            }

            if (entryNumber.EntryDestination.DestinationId == 0)
            {
                destinationId = null;
            }
            else
            {
                destinationId = entryNumber.EntryDestination.DestinationId;
            }

            return new GeneralLedgerEntities.EntryNumber(Convert.ToInt32(entryNumber.Id))
            {
                AccountingMovementTypeId = accountingMovementId,
                EntryDestinationId = destinationId,
                EntryNumberId = entryNumber.Id,
                GenerationMonth = entryNumber.Date.Month,
                GenerationYear = entryNumber.Date.Year,
                IsJournalEntry = entryNumber.IsJournalEntry,
                LastGeneratedEntryYear = entryNumber.Year,
                Number = entryNumber.Number
            };
        }

        #endregion EntryNumber

        #region EntryRevertion

        /// <summary>
        /// CreateEntryRevertion
        /// </summary>
        /// <param name="entryRevertionId"></param>
        /// <param name="entrySourceId"></param>
        /// <param name="entryDestinationId"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryRevertion CreateEntryRevertion(int entryRevertionId, int entrySourceId, int entryDestinationId, int userId, DateTime date, bool isJournalEntry)
        {
            return new GeneralLedgerEntities.EntryRevertion(entryRevertionId)
            {
                EntryRevertionId = entryRevertionId, //clave autonum�rica
                EntrySourceId = entrySourceId,
                EntryDestinationId = entryDestinationId,
                UserCode = userId,
                Date = date,
                IsJournalEntry = isJournalEntry
            };
        }

        #endregion EntryRevertion

        #region PostDatedEntry

        /// <summary>
        /// CreatePostDatedEntry
        /// </summary>
        /// <param name="postDated"></param>
        /// <param name="entryId"></param>
        /// <param name="isDailyEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.PostdatedEntryItem CreatePostdatedEntryItem(PostDated postDated, int entryId, bool isDailyEntry)
        {
            return new GeneralLedgerEntities.PostdatedEntryItem(postDated.PostDatedId)
            {
                PostdatedEntryItemId = postDated.PostDatedId,
                EntryItemId = entryId,
                PostdatedType = Convert.ToInt32(postDated.PostDateType),
                CurrencyCode = postDated.Amount.Currency.Id,
                ExchangeRate = postDated.ExchangeRate.SellAmount,
                DocumentNumber = postDated.DocumentNumber,
                AmountValue = postDated.Amount.Value,
                AmountLocalValue = postDated.LocalAmount.Value,
                IsJournalEntry = isDailyEntry
            };
        }

        #endregion PostDatedEntry

        #region EntryMassiveLoad

        /// <summary>
        /// CreateEntryMassiveLoad
        /// </summary>
        /// <param name="massiveEntryDTO"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryMassiveLoad CreateEntryMassiveLoad(MassiveEntryDTO massiveEntryDTO)
        {
            decimal? percentage = 0;
            decimal? postDatedExchangeRate = 0;
            decimal? postDatedAmount = 0;
            int? analysisId = 0;
            int? analysisConceptId = 0;
            int? bankReconciliationId = 0;
            int? costCenterId = 0;
            int? individualId = 0;
            int? postDatedId = 0;
            int? postDatedCurrencyId = 0;
            int? receiptNumber = 0;


            if (massiveEntryDTO.BankReconciliationId == 0)
            {
                bankReconciliationId = null;
            }
            else
            {
                bankReconciliationId = massiveEntryDTO.BankReconciliationId;
            }
            if (massiveEntryDTO.ReceiptNumber == 0)
            {
                receiptNumber = null;
            }
            else
            {
                receiptNumber = massiveEntryDTO.ReceiptNumber;
            }
            if (massiveEntryDTO.CostCenterId == 0)
            {
                costCenterId = null;
                percentage = null;
            }
            else
            {
                costCenterId = massiveEntryDTO.CostCenterId;
                percentage = massiveEntryDTO.Percentage;
            }
            if (massiveEntryDTO.AnalysisId == 0)
            {
                analysisId = null;
            }
            else
            {
                analysisId = massiveEntryDTO.AnalysisId;
            }
            if (massiveEntryDTO.ConceptId == 0)
            {
                analysisConceptId = null;
            }
            else
            {
                analysisConceptId = massiveEntryDTO.ConceptId;
            }
            if (massiveEntryDTO.IndividualId == 0)
            {
                individualId = null;
            }
            else
            {
                individualId = massiveEntryDTO.IndividualId;
            }
            if (massiveEntryDTO.PostdatedId == 0)
            {
                postDatedId = null;
                postDatedCurrencyId = null;
                postDatedExchangeRate = null;
                postDatedAmount = null;
            }
            else
            {
                postDatedId = massiveEntryDTO.PostdatedId;
                postDatedCurrencyId = massiveEntryDTO.PostdatedCurrencyId;
                postDatedExchangeRate = massiveEntryDTO.PostdatedExchangeRate;
                postDatedAmount = massiveEntryDTO.PostdatedAmount;
            }

            return new GeneralLedgerEntities.EntryMassiveLoad(massiveEntryDTO.Id)
            {
                EntryMassiveLoadId = massiveEntryDTO.Id,
                BranchId = massiveEntryDTO.BranchId,
                SalePointId = massiveEntryDTO.SalePointId,
                AccountingCompanyId = massiveEntryDTO.AccoutingCompanyId,
                EntryDestinationId = massiveEntryDTO.EntryDestinationId,
                AccountingMovementTypeId = massiveEntryDTO.AccountingMovementTypeId,
                OperationDate = massiveEntryDTO.OperationDate,
                CurrencyId = massiveEntryDTO.CurrencyId,
                ExchangeRate = massiveEntryDTO.ExchangeRate,
                AccountingAccountId = massiveEntryDTO.AccountingAccountId,
                AccountingNature = massiveEntryDTO.AccountingNature,
                Amount = massiveEntryDTO.Amount,
                IndividualId = individualId,
                Description = massiveEntryDTO.Description,
                BankReconciliationId = bankReconciliationId,
                ReceiptNumber = receiptNumber,
                ReceiptDate = massiveEntryDTO.ReceiptDate == Convert.ToDateTime("01/01/0001 0:00:00") ? null : massiveEntryDTO.ReceiptDate,
                CostCenterId = costCenterId,
                Percentage = percentage,
                AnalysisId = analysisId,
                ConceptId = analysisConceptId,
                ConceptKey = massiveEntryDTO.ConceptKey,
                AnalysisDescription = massiveEntryDTO.AnalysisDescription,
                PostdatedId = postDatedId,
                PostdatedCurrencyId = postDatedCurrencyId,
                PostdatedExchangeRate = postDatedExchangeRate,
                PostdatedDocumentNumber = massiveEntryDTO.PosdatedDocumentNumber,
                PostdatedAmount = postDatedAmount
            };
        }

        #endregion EntryMassiveLoad

        #region EntryMassiveLoadLog

        /// <summary>
        /// CreateEntryMassiveLoadLog
        /// </summary>
        /// <param name="massiveEntryLogDTO"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryMassiveLoadLog CreateEntryMassiveLoadLog(MassiveEntryLogDTO massiveEntryLogDTO)
        {
            return new GeneralLedgerEntities.EntryMassiveLoadLog(massiveEntryLogDTO.Id)
            {
                Id = massiveEntryLogDTO.Id,
                ProcessDate = massiveEntryLogDTO.ProcessDate,
                OperationDate = massiveEntryLogDTO.OperationDate,
                RowNumber = massiveEntryLogDTO.RowNumber,
                Success = Convert.ToInt32(massiveEntryLogDTO.Success),
                ErrorDescription = massiveEntryLogDTO.ErrorDescription,
                Enabled = Convert.ToInt32(massiveEntryLogDTO.Enabled)
            };
        }

        #endregion EntryMassiveLoadLog

        #region AccountingCompany

        /// <summary>
        /// CreateAccountingCompany
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingCompany CreateAccountingCompany(AccountingCompany accountingCompany)
        {
            return new GeneralLedgerEntities.AccountingCompany(accountingCompany.AccountingCompanyId)
            {
                AccountingCompanyId = accountingCompany.AccountingCompanyId,
                Description = accountingCompany.Description,
                Default = accountingCompany.Default
            };
        }

        #endregion AccountingCompany

        #region Analysis

        /// <summary>
        /// CreateAnalysis
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.Analysis CreateAnalysis(AnalysisCode analysisCode)
        {
            return new GeneralLedgerEntities.Analysis(analysisCode.AnalysisCodeId)
            {
                AnalysisId = analysisCode.AnalysisCodeId,
                Description = analysisCode.Description,
                AccountingNature = Convert.ToInt32(analysisCode.AccountingNature),
                RequireOrigin = Convert.ToBoolean(analysisCode.CheckModuleType),
                ControlBalance = Convert.ToBoolean(analysisCode.CheckBalance)
            };
        }

        #endregion Analysis

        #region AnalysisTreatment

        /// <summary>
        /// CreateAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AnalysisTreatment CreateAnalysisTreatment(AnalysisTreatment analysisTreatment)
        {
            return new GeneralLedgerEntities.AnalysisTreatment(analysisTreatment.AnalysisTreatmentId)
            {
                AnalysisTreatmentId = analysisTreatment.AnalysisTreatmentId,
                Description = analysisTreatment.Description
            };
        }

        #endregion AnalysisTreatment

        #region AnalysisConcept

        /// <summary>
        /// CreateAnalysisConcept
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AnalysisConcept CreateAnalysisConcept(AnalysisConcept analysisConcept)
        {
            return new GeneralLedgerEntities.AnalysisConcept(analysisConcept.AnalysisConceptId)
            {
                AnalysisConceptId = analysisConcept.AnalysisConceptId,
                Description = analysisConcept.Description,
                AnalysisTreatmentId = analysisConcept.AnalysisTreatment.AnalysisTreatmentId
            };
        }

        #endregion AnalysisConcept

        #region AnalysisConceptKey
        /// <summary>
        /// CreateAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AnalysisConceptKey CreateAnalysisConceptKey(AnalysisConceptKey analysisConceptKey)
        {

            return new GeneralLedgerEntities.AnalysisConceptKey(0, analysisConceptKey.AnalysisConcept.AnalysisConceptId)
            {
                AnalysisConceptKeyId = 0, // identity
                AnalysisConceptId = analysisConceptKey.AnalysisConcept.AnalysisConceptId,
                Table = analysisConceptKey.TableName,
                Column = analysisConceptKey.ColumnName,
                ColumnDescription = analysisConceptKey.ColumnDescription
            };
        }

        #endregion

        #region AnalysisConceptAnalysis

        /// <summary>
        /// CreateAnalysisConceptAnalysis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AnalysisConceptAnalysis CreateAnalysisConceptAnalysis(int analysisId, int analysisConceptId)
        {
            return new GeneralLedgerEntities.AnalysisConceptAnalysis(0)
            {
                AnalysisConceptAnalysisId = 0, //autonum�rico, identificador de la tabla
                AnalysisId = analysisId,
                AnalysisConceptId = analysisConceptId
            };
        }

        #endregion AnalysisConceptAnalysis

        #region CostCenter

        /// <summary>
        /// CreateCostCenter
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.CostCenter CreateCostCenter(CostCenter costCenter)
        {
            return new GeneralLedgerEntities.CostCenter(costCenter.CostCenterId)
            {
                CostCenterId = costCenter.CostCenterId,
                Description = costCenter.Description,
                CostCenterTypeId = costCenter.CostCenterType.CostCenterTypeId,
                IsProrated = costCenter.IsProrated
            };
        }

        #endregion CostCenter

        #region CostCenterEntry

        /// <summary>
        /// CreateCostCenterEntry
        /// </summary>
        /// <param name="costCenter"></param>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.CostCenterEntryItem CreateCostCenterEntryItem(CostCenter costCenter, int entryItemId, bool isJournalEntry)
        {
            return new GeneralLedgerEntities.CostCenterEntryItem(0)
            {
                CostCenterEntryItemId = 0,//autinum�rico
                CostCenterId = costCenter.CostCenterId,
                EntryItemId = entryItemId,
                CostCenterPercentage = costCenter.PercentageAmount,
                IsJournalEntry = isJournalEntry
            };
        }

        #endregion CostCenterEntry

        #region CostCenterType

        /// <summary>
        /// CreateCostCenterType
        /// </summary>
        /// <param name="costCenterType"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.CostCenterType CreateCostCenterType(CostCenterType costCenterType)
        {
            return new GeneralLedgerEntities.CostCenterType(costCenterType.CostCenterTypeId)
            {
                CostCenterTypeId = costCenterType.CostCenterTypeId,
                Description = costCenterType.Description
            };
        }

        #endregion CostCenterType

        #region Destination

        /// <summary>
        /// CreateEntryDestination
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryDestination CreateEntryDestination(EntryDestination entryDestination)
        {
            return new GeneralLedgerEntities.EntryDestination(entryDestination.DestinationId)
            {
                EntryDestinationId = entryDestination.DestinationId,
                Description = entryDestination.Description
            };
        }

        #endregion Destination

        #region EntryAnalysis

        /// <summary>
        /// CreateEntryAnalysis
        /// </summary>
        /// <param name="analysis"></param>
        /// <param name="entryItemId"></param>
        /// <param name="correlativeNumber"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AnalysisEntryItem CreateAnalysisEntryItem(Analysis analysis, int entryItemId, int correlativeNumber, bool isJournalEntry)
        {
            return new GeneralLedgerEntities.AnalysisEntryItem(analysis.AnalysisId)
            {
                AnalysisEntryItemId = entryItemId,
                AnalysisId = analysis.AnalysisId,
                AnalysisConceptId = analysis.AnalysisConcept.AnalysisConceptId,
                EntryItemId = entryItemId,
                ConceptKey = analysis.Key,
                CorrelativeNumber = correlativeNumber,
                IsJournalEntry = isJournalEntry,
                Description = analysis.Description
            };
        }

        #endregion EntryAnalysis

        #region EntryType

        /// <summary>
        /// CreateEntryType
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryType CreateEntryType(EntryType entryType)
        {
            return new GeneralLedgerEntities.EntryType(entryType.EntryTypeId)
            {
                EntryTypeId = entryType.EntryTypeId,
                Description = entryType.Description,
                SmallDescription = entryType.SmallDescription
            };
        }

        #endregion EntryType

        #region EntryTypeItem

        /// <summary>
        /// CreateEntryTypeItem
        /// </summary>
        /// <param name="entryTypeItem"></param>
        /// <param name="entryTypeId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.EntryTypeItem CreateEntryTypeItem(EntryTypeItem entryTypeItem, int entryTypeId)
        {
            return new GeneralLedgerEntities.EntryTypeItem(entryTypeItem.Id)
            {
                EntryTypeItemId = entryTypeItem.Id,
                AccountingMovementTypeId = entryTypeItem.AccountingMovementType.AccountingMovementTypeId,
                AccountingNature = Convert.ToInt32(entryTypeItem.AccountingNature),
                AnalysisId = entryTypeItem.Analysis.AnalysisId,
                CostCenterId = entryTypeItem.CostCenter.CostCenterId,
                CurrencyCode = entryTypeItem.Currency.Id,
                AccountingAccountId = entryTypeItem.AccountingAccount.AccountingAccountId,
                AccountingConceptCode = entryTypeItem.AccountingConcept.Id,
                EntryTypeId = entryTypeId,
                Description = entryTypeItem.Description
            };
        }

        #endregion EntryTypeItem

        #region AccountingMovementType

        /// <summary>
        /// CreateAccountingMovementType
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingMovementType CreateAccountingMovementType(AccountingMovementType accountingMovementType)
        {
            return new GeneralLedgerEntities.AccountingMovementType(accountingMovementType.AccountingMovementTypeId)
            {
                AccountingMovementTypeId = accountingMovementType.AccountingMovementTypeId,
                Description = accountingMovementType.Description,
                IsAutomatic = accountingMovementType.IsAutomatic
            };
        }

        #endregion AccountingMovementType

        #region TempEntryGeneration

        /// <summary>
        /// CreateTempEntryGeneration
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="transactionNumber"></param>
        /// <param name="isJournalEntry"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.TempEntryGeneration CreateTempEntryGeneration(LedgerEntry ledgerEntry, LedgerEntryItem ledgerEntryItem, int transactionNumber, bool isJournalEntry, int userId)
        {
            int? accountingConceptCode = 0;
            int? branchCode = null;
            int? salePointCode = null;
            int? accountingCompanyCode = null;
            int? bankReconciliationCode = null;
            int? entryDestinationCode = null;
            int? accountingAccountCode = null;

            accountingConceptCode = ledgerEntryItem.AccountingAccount.AccountingConcepts?[0].Id;

            if (ledgerEntry.Branch.Id > 0)
            {
                branchCode = Convert.ToInt32(ledgerEntry.Branch.Id);
            }

            if (ledgerEntry.SalePoint.Id > 0)
            {
                salePointCode = Convert.ToInt32(ledgerEntry.SalePoint.Id);
            }

            if (ledgerEntry.AccountingCompany.AccountingCompanyId > 0)
            {
                accountingCompanyCode = Convert.ToInt32(ledgerEntry.AccountingCompany.AccountingCompanyId);
            }

            if (ledgerEntryItem.ReconciliationMovementType.Id > 0)
            {
                bankReconciliationCode = Convert.ToInt32(ledgerEntryItem.ReconciliationMovementType.Id);
            }

            if (ledgerEntry.EntryDestination.DestinationId > 0)
            {
                entryDestinationCode = ledgerEntry.EntryDestination.DestinationId;
            }

            if (ledgerEntryItem.AccountingAccount.AccountingAccountId > 0)
            {
                accountingAccountCode = ledgerEntryItem.AccountingAccount.AccountingAccountId;
            }

            return new GeneralLedgerEntities.TempEntryGeneration(0)
            {
                AccountingAccountId = accountingAccountCode,
                AccountingCompanyCode = accountingCompanyCode,
                AccountingConceptCode = accountingConceptCode,
                AccountingModuleId = ledgerEntry.ModuleDateId,
                AccountingMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId,
                AccountingNature = Convert.ToInt32(ledgerEntryItem.AccountingNature),
                Amount = ledgerEntryItem.Amount.Value,
                BankReconciliationId = bankReconciliationCode,
                BranchCode = branchCode,
                CurrencyCode = ledgerEntryItem.Amount.Currency.Id,
                Date = ledgerEntry.AccountingDate,
                Description = ledgerEntry.Description,
                DueDate = null,
                EntryDestinationId = entryDestinationCode,
                EntryNumber = ledgerEntry.EntryNumber,
                ExchangeRate = ledgerEntryItem.ExchangeRate.SellAmount,
                IndividualId = ledgerEntryItem.Individual.IndividualId,
                IsJournalEntry = isJournalEntry,
                LocalAmount = ledgerEntryItem.LocalAmount.Value,
                ReceiptDate = ledgerEntryItem.Receipt.Date == Convert.ToDateTime("01/01/0001 0:00:00") ? null : ledgerEntry.LedgerEntryItems[0].Receipt.Date,
                ReceiptNumber = ledgerEntryItem.Receipt.Number,
                ReconciliationCode = null,
                ReconciliationDate = null,
                SalePointCode = salePointCode,
                TempEntryGenerationId = 0,
                TransactionNumber = transactionNumber,
                UserCode = userId
            };
        }

        #endregion TempEntryGeneration

        #region EntryParameter

        #region Parameter

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.Parameter CreateParameter(RuleModels.Parameter parameter)
        {
            return new GeneralLedgerEntities.Parameter(parameter.Id)
            {
                ParameterId = parameter.Id, //autonum�rico
                ModuleCode = parameter.ModuleDateId,
                DataTypeCode = Convert.ToInt32(parameter.DataType),
                Order = parameter.Order,
                Description = parameter.Description
            };
        }

        #endregion Parameter

        #region AccountingRule

        /// <summary>
        /// CreateAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingRule CreateAccountingRule(AccountingRule accountingRule)
        {
            return new GeneralLedgerEntities.AccountingRule(accountingRule.Id)
            {
                AccountingRuleId = accountingRule.Id,
                Description = accountingRule.Description,
                ModuleCode = accountingRule.ModuleDateId,
                Observations = accountingRule.Observation
            };
        }

        #endregion AccountingRule

        #region AccountingRulePackage

        /// <summary>
        /// CreateAccountingRulePackage
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingRulePackage CreateAccountingRulePackage(AccountingRulePackage accountingRulePackage)
        {
            return new GeneralLedgerEntities.AccountingRulePackage(accountingRulePackage.Id)
            {
                AccountingRulePackageId = accountingRulePackage.Id,
                Description = accountingRulePackage.Description,
                ModuleCode = accountingRulePackage.ModuleDateId,
                RulePackageCode = accountingRulePackage.RulePackageId 
            };
        }

        #endregion AccountingRulePackage

        #region RulePackageRule

        /// <summary>
        /// CreateConceptParameterEntry
        /// Crea la relaci�n entre el paquete de reglas y las reglas
        /// </summary>
        /// <param name="accountingRulePackageId"></param>
        /// <param name="accountingRuleId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.RulePackageRule CreateRulePackageRule(int accountingRulePackageId, int accountingRuleId)
        {
            return new GeneralLedgerEntities.RulePackageRule(0)
            {
                RulePackageRuleId = 0, //autonum�rico
                AccountingRulePackageId = accountingRulePackageId,
                AccountingRuleId = accountingRuleId
            };
        }

        #endregion RulePackageRule

        #region Condition

        /// <summary>
        /// CreateCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.Condition CreateCondition(Condition condition)
        {
            int? resultId = null;

            if (condition.IdResult > 0)
            {
                resultId = condition.IdResult;
            }

            return new GeneralLedgerEntities.Condition(condition.Id)
            {
                AccountingRuleId = condition.AccountingRule.Id,
                ConditionId = condition.Id,
                ParameterId = condition.Parameter.Id,
                Operator = condition.Operator,
                Value = condition.Value,
                RightConditionId = condition.IdRightCondition,
                LeftConditionId = condition.IdLeftCondition,
                ResultId = resultId
            };
        }

        #endregion Condition

        #region Result

        /// <summary>
        /// CreateResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.Result CreateResult(Result result)
        {
            return new GeneralLedgerEntities.Result(result.Id)
            {
                ResultId = result.Id,
                AccountingNatureId = Convert.ToInt32(result.AccountingNature),
                AccountingAccountNumber = result.AccountingAccount,
                ParameterId = result.Parameter.Id
            };
        }

        #endregion Result

        #endregion EntryParameter

        #region Reclassification

        /// <summary>
        /// CreateReclassification
        /// </summary>
        /// <param name="accountReclassification"> </param>
        /// <returns>ReclassificationParameterization</returns>
        public static GeneralLedgerEntities.Reclassification CreateReclassification(ReclassificationModels.AccountReclassification accountReclassification)
        {
            // Explicar esta parte pq se pasa un id y luego se construye un objeto.
            return new GeneralLedgerEntities.Reclassification(accountReclassification.Year,
                                                                              accountReclassification.Month,
                                                                              accountReclassification.SourceAccountingAccount.AccountingAccountId)
            {
                DestinationAccountingAccountId = accountReclassification.DestinationAccountingAccount.AccountingAccountId,
                OpeningBranch = accountReclassification.OpeningBranch,
                OpeningPrefix = accountReclassification.OpeningPrefix,
                ProcessMonth = accountReclassification.Month,
                ProcessYear = accountReclassification.Year,
                SourceAccountingAccountId = accountReclassification.SourceAccountingAccount.AccountingAccountId
            };
        }

        /// <summary>
        /// CreateAccountingReclassification
        /// </summary>
        /// <param name="accountReclassificationResult"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingReclassification CreateAccountingReclassification(ReclassificationModels.AccountReclassificationResult accountReclassificationResult)
        {
            int? analysisCode = 0;
            int? analysisConceptCode = 0;
            int? costCenterCode = 0;

            if (accountReclassificationResult.Analysis.AnalysisId == -1)
            {
                analysisCode = null;
            }
            else
            {
                analysisCode = Convert.ToInt32(accountReclassificationResult.Analysis.AnalysisId);
            }

            if (accountReclassificationResult.Analysis.AnalysisConcept.AnalysisConceptId == -1)
            {
                analysisConceptCode = null;
            }
            else
            {
                analysisConceptCode = Convert.ToInt32(accountReclassificationResult.Analysis.AnalysisConcept.AnalysisConceptId);
            }

            if (accountReclassificationResult.CostCenter.CostCenterId == -1)
            {
                costCenterCode = null;
            }
            else
            {
                costCenterCode = Convert.ToInt32(accountReclassificationResult.CostCenter.CostCenterId);
            }

            // Explicar esta parte pq se pasa un id y luego se construye un objeto.
            return new GeneralLedgerEntities.AccountingReclassification(accountReclassificationResult.Id)
            {

                AccountingAccountId = Convert.ToInt32(accountReclassificationResult.DestinationAccountingAccount.AccountingAccountId),
                AccountingNature = Convert.ToInt32(accountReclassificationResult.AccountingNature),
                AccountingReclassificationId = accountReclassificationResult.Id,
                Amount = accountReclassificationResult.LocalAmount.Value,
                AnalysisCode = analysisCode,
                AnalysisConceptCode = analysisConceptCode,
                BranchCode = accountReclassificationResult.Branch.Id,
                ConceptKey = accountReclassificationResult.Analysis.Key == "" ? null : accountReclassificationResult.Analysis.Key,
                CostCenterCode = costCenterCode,
                CurrencyCode = accountReclassificationResult.Amount.Currency.Id,
                ExchangeRate = accountReclassificationResult.ExchangeRate.SellAmount,
                IncomeAmount = accountReclassificationResult.Amount.Value,
                ProcessMonth = accountReclassificationResult.Month,
                ProcessYear = accountReclassificationResult.Year,
                SourceAccountingAccountId = Convert.ToInt32(accountReclassificationResult.SourceAccountingAccount.AccountingAccountId)
            };
        }

        #endregion AccountReclassification

        #region AccountingAccountMask

        /// <summary>
        /// CreateAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingAccountMask CreateAccountingAccountMask(AccountingAccountMask accountingAccountMask, int resultId)
        {
            return new GeneralLedgerEntities.AccountingAccountMask(accountingAccountMask.Id)
            {
                AccountingAccountMaskId = accountingAccountMask.Id,
                ResultId = resultId,
                ParameterId = accountingAccountMask.Parameter.Id,
                Mask = accountingAccountMask.Mask,
                StartPosition = accountingAccountMask.Start
            };
        }

        #endregion AccountingAccountMask

        #region EntryReclassification

        /// <summary>
        /// CreateReclassificationLog
        /// </summary>
        /// <param name="processLog"> </param>
        /// <returns>ReclassificationLog</returns>
        public static GeneralLedgerEntities.ReclassificationLog CreateReclassificationLog(ProcessLog processLog)
        {
            // Explicar esta parte pq se pasa un id y luego se construye un objeto.
            return new GeneralLedgerEntities.ReclassificationLog(processLog.Id)
            {
                //EndDate = processLog.EndDate == Convert.ToDateTime("01/01/0001 0:00:00") ? null : processLog.EndDate,
                EndDate = processLog.EndDate,
                ModuleCode = 9,
                ProcessMonth = processLog.Month,
                ProcessStatus = Convert.ToInt32(processLog.ProcessLogStatus),
                ProcessYear = processLog.Year,
                ReclassificationLogId = processLog.Id,
                StartDate = processLog.StartDate,
                UserCode = processLog.UserId
            };
        }

        #endregion

        #region AccountingConcept

        /// <summary>
        /// CreateAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.AccountingConcept CreateAccountingConcept(GeneralLedgerModels.AccountingConcepts.AccountingConcept accountingConcept)
        {
            int? accountingAccountId = null;

            if (accountingConcept.AccountingAccount.AccountingAccountId > 0)
            {
                accountingAccountId = accountingConcept.AccountingAccount.AccountingAccountId;
            }

            return new GeneralLedgerEntities.AccountingConcept(accountingConcept.Id)
            {
                AccountingAccountId = accountingAccountId,
                AccountingConceptCode = accountingConcept.Id,
                AgentEnabled = accountingConcept.AgentEnabled,
                CoinsuranceEnabled = accountingConcept.CoInsurancedEnabled,
                Description = accountingConcept.Description,
                InsuredEnabled = accountingConcept.InsuredEnabled,
                ItemEnabled = accountingConcept.ItemEnabled,
                ReinsuranceEnabled = accountingConcept.ReInsuranceEnabled,
            };
        }
        #endregion

        #region ConceptSource
        /// <summary>
        /// CreateConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.ConceptSource CreateConceptSource(GeneralLedgerModels.AccountingConcepts.ConceptSource conceptSource)
        {
            return new GeneralLedgerEntities.ConceptSource(conceptSource.Id)
            {
                ConceptSourceCode = conceptSource.Id, // no es identity
                Description = conceptSource.Description
            };
        }

        #endregion

        #region MovementType

        /// <summary>
        /// CreateMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.MovementType CreateMovementType(GeneralLedgerModels.AccountingConcepts.MovementType movementType)
        {
            return new GeneralLedgerEntities.MovementType(movementType.Id, movementType.ConceptSource.Id)
            {
                MovementTypeCode = movementType.Id, // no es identity
                ConceptSourceCode = movementType.ConceptSource.Id,
                Description = movementType.Description
            };
        }

        #endregion

        #region BranchAccountingConcept

        /// <summary>
        /// CreateBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept">Model</param>
        /// <returns>Entities Assembler</returns>
        public static GeneralLedgerEntities.BranchAccountingConcept CreateBranchAccountingConcept(GeneralLedgerModels.AccountingConcepts.BranchAccountingConcept branchAccountingConcept)
        {
            return new GeneralLedgerEntities.BranchAccountingConcept(branchAccountingConcept.Id)
            {
                BranchAccountingConceptId = branchAccountingConcept.Id, // identity
                BranchCode = branchAccountingConcept.Branch.Id,
                AccountingConceptCode = branchAccountingConcept.AccountingConcept.Id,
                MovementTypeCode = branchAccountingConcept.MovementType.Id,
                ConceptSourceCode = branchAccountingConcept.MovementType.ConceptSource.Id
            };
        }
        #endregion

        #region UserBranchAccountingConcept

        /// <summary>
        /// CreateUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept">Model</param>
        /// <returns>Entities Assembler</returns>
        public static GeneralLedgerEntities.UserBranchAccountingConcept CreateUserBranchAccountingConcept(GeneralLedgerModels.AccountingConcepts.UserBranchAccountingConcept userBranchAccountingConcept)
        {
            return new GeneralLedgerEntities.UserBranchAccountingConcept(userBranchAccountingConcept.Id)
            {
                UserBranchAccountingConceptId = userBranchAccountingConcept.Id,// identity
                BranchAccountingConceptId = userBranchAccountingConcept.BranchAccountingConcept.Id,
                UserCode = userBranchAccountingConcept.UserId
            };
        }

        #endregion

        #region ReconciliationMovementTypes

        /// <summary>
        /// CreateBankReconciliation
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        /// <returns>BankNetwork</returns>
        public static GeneralLedgerEntities.BankReconciliation CreateReconciliationMovementType(ReconciliationMovementType reconciliationMovementType)
        {
            bool debitBank;
            bool debitCompany;
            if (reconciliationMovementType.AccountingNature == AccountingNatures.Credit)
            {
                debitBank = true;
                debitCompany = false;
            }
            else
            {
                debitBank = false;
                debitCompany = true;
            }

            return new GeneralLedgerEntities.BankReconciliation(reconciliationMovementType.Id)
            {
                BankReconciliationId = reconciliationMovementType.Id,
                DebitBank = Convert.ToBoolean(debitBank),
                DebitCompany = debitCompany,
                Description = reconciliationMovementType.Description,
                ShortDescription = reconciliationMovementType.SmallDescription
            };
        }

        #endregion

        #region CollectApplicationControl
        public static INTEN.CollectApplicationControl CreateCollectApplicationControl(Models.Integration2G.CollectApplicationControl collectApplicationControl)
        {
            return new INTEN.CollectApplicationControl(0)
            {
                CollectApplicationId = collectApplicationControl.CollectApplicationId,
                AppSource = collectApplicationControl.AppSource,
                Action = collectApplicationControl.Action,
                Origin = collectApplicationControl.Origin
            };
        }


        /// <summary>
        /// CreateJournalEntryItem
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <param name="journalEntryId"></param>
        /// <returns></returns>
        public static GeneralLedgerEntities.LogJournalEntryItem CreateLogJournalEntryItem(LogJournalEntryItem logJournalEntryItem)
        {
            try
            {
                return new GeneralLedgerEntities.LogJournalEntryItem(0)
                {
                    AccountNumber = logJournalEntryItem.AccountNumber,
                    AccountNature = logJournalEntryItem.AccountNature,
                    Amount = logJournalEntryItem.Amount,
                    CodePackagesRules = logJournalEntryItem.CodePackagesRules,
                    CurrencyCode = logJournalEntryItem.CurrencyId,
                    JeiJson = logJournalEntryItem.JeiJson,
                    ModuleCode = logJournalEntryItem.ModuleId
                };
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        #endregion
    }
}